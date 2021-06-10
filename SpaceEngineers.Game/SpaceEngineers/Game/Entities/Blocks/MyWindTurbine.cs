// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyWindTurbine
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using SpaceEngineers.Game.EntityComponents.Renders;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_WindTurbine))]
  public class MyWindTurbine : MyEnvironmentalPowerProducer
  {
    private HashSet<IMyEntity> m_children = new HashSet<IMyEntity>();
    private int m_nextUpdateRay;
    private float m_effectivity;
    private bool m_paralleRaycastRunning;
    private readonly Action<MyPhysics.HitInfo?> m_onRaycastCompleted;
    private readonly Action<List<MyPhysics.HitInfo>> m_onRaycastCompletedList;
    private List<MyPhysics.HitInfo> m_cachedHitList = new List<MyPhysics.HitInfo>();
    private Action m_updateEffectivity;

    protected float Effectivity
    {
      get => this.m_effectivity;
      set
      {
        if ((double) this.m_effectivity == (double) value)
          return;
        this.m_effectivity = value;
        this.OnProductionChanged();
        this.UpdateVisuals();
      }
    }

    protected override float CurrentProductionRatio => !this.Enabled || !this.IsWorking ? 0.0f : this.m_effectivity * Math.Min(1f, this.GetOrCreateSharedComponent().WindSpeed / this.BlockDefinition.OptimalWindSpeed);

    public MyWindTurbineDefinition BlockDefinition => (MyWindTurbineDefinition) base.BlockDefinition;

    public float[] RayEffectivities { get; private set; }

    public MyWindTurbine()
    {
      this.m_updateEffectivity = new Action(this.UpdateEffectivity);
      this.m_onRaycastCompleted = new Action<MyPhysics.HitInfo?>(this.OnRaycastCompleted);
      this.m_onRaycastCompletedList = new Action<List<MyPhysics.HitInfo>>(this.OnRaycastCompleted);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.RayEffectivities = ((MyObjectBuilder_WindTurbine) objectBuilder).ImmediateEffectivities;
      if (this.RayEffectivities == null)
        this.RayEffectivities = new float[this.BlockDefinition.RaycastersCount];
      base.Init(objectBuilder, cubeGrid);
      this.SourceComp.Enabled = this.Enabled;
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    public override void InitComponents()
    {
      this.Render = (MyRenderComponentBase) new MyRenderComponentWindTurbine();
      base.InitComponents();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_WindTurbine builderCubeBlock = (MyObjectBuilder_WindTurbine) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.ImmediateEffectivities = (float[]) this.RayEffectivities.Clone();
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.GetOrCreateSharedComponent().UpdateWindSpeed();
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      this.GetOrCreateSharedComponent().Update10();
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      this.GetOrCreateSharedComponent().UpdateWindSpeed();
    }

    public void UpdateNextRay()
    {
      if (this.m_paralleRaycastRunning)
        return;
      this.m_paralleRaycastRunning = true;
      Vector3D start;
      Vector3D end;
      this.GetRaycaster(this.m_nextUpdateRay, out start, out end);
      if (this.m_nextUpdateRay != 0)
      {
        MyPhysics.CastRayParallel(ref start, ref end, 0, this.m_onRaycastCompleted);
      }
      else
      {
        this.m_cachedHitList.AssertEmpty<MyPhysics.HitInfo>();
        MyPhysics.CastRayParallel(ref start, ref end, this.m_cachedHitList, 28, this.m_onRaycastCompletedList);
      }
    }

    private void OnRaycastCompleted(List<MyPhysics.HitInfo> hitList)
    {
      using (hitList.GetClearToken<MyPhysics.HitInfo>())
      {
        foreach (MyPhysics.HitInfo hit in hitList)
        {
          if (hit.HkHitInfo.Body.GetEntity(0U) is MyVoxelBase)
          {
            this.OnRaycastCompleted(new MyPhysics.HitInfo?(hit));
            return;
          }
        }
        this.OnRaycastCompleted(new MyPhysics.HitInfo?());
      }
    }

    private void OnRaycastCompleted(MyPhysics.HitInfo? hitInfo)
    {
      float num = 1f;
      if (hitInfo.HasValue)
      {
        float hitFraction = hitInfo.Value.HkHitInfo.HitFraction;
        float raycasterClearance = this.BlockDefinition.MinRaycasterClearance;
        num = (double) hitFraction > (double) raycasterClearance ? (float) (((double) hitFraction - (double) raycasterClearance) / (1.0 - (double) raycasterClearance)) : 0.0f;
      }
      this.RayEffectivities[this.m_nextUpdateRay] = num;
      ++this.m_nextUpdateRay;
      if (this.m_nextUpdateRay >= this.BlockDefinition.RaycastersCount)
        this.m_nextUpdateRay = 0;
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        if (this.MarkedForClose)
          return;
        this.UpdateEffectivity();
        this.m_paralleRaycastRunning = false;
      }), "Turbine update");
    }

    private void UpdateEffectivity()
    {
      if (!this.IsWorking)
      {
        this.Effectivity = 0.0f;
      }
      else
      {
        float num = 0.0f;
        for (int index = 1; index < this.RayEffectivities.Length; ++index)
          num += this.RayEffectivities[index];
        this.Effectivity = num / this.BlockDefinition.RaycastersToFullEfficiency * MathHelper.Lerp(0.5f, 1f, this.RayEffectivities[0]) * this.GetOrCreateSharedComponent().WindSpeedModifier;
      }
    }

    public void GetRaycaster(int id, out Vector3D start, out Vector3D end)
    {
      MatrixD worldMatrix = this.WorldMatrix;
      start = worldMatrix.Translation;
      if (id == 0)
      {
        end = start + this.GetOrCreateSharedComponent().GravityNormal * (double) this.BlockDefinition.OptimalGroundClearance;
      }
      else
      {
        float angle = 6.283185f / (float) (this.RayEffectivities.Length - 1) * (float) (id - 1);
        int raycasterSize = this.BlockDefinition.RaycasterSize;
        end = start + (double) raycasterSize * ((double) MyMath.FastSin(angle) * worldMatrix.Left + (double) MyMath.FastCos(angle) * worldMatrix.Forward);
      }
    }

    public override void OnRegisteredToGridSystems()
    {
      base.OnRegisteredToGridSystems();
      this.GetOrCreateSharedComponent().Register(this);
    }

    public override void OnUnregisteredFromGridSystems()
    {
      base.OnUnregisteredFromGridSystems();
      this.GetOrCreateSharedComponent().Unregister(this);
    }

    private MySharedWindComponent GetOrCreateSharedComponent()
    {
      MyEntityComponentContainer components = this.CubeGrid.Components;
      MySharedWindComponent component = components.Get<MySharedWindComponent>();
      if (component == null)
      {
        component = new MySharedWindComponent();
        components.Add<MySharedWindComponent>(component);
      }
      return component;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (!this.IsWorking)
        return;
      this.OnStartWorking();
    }

    protected override void OnStartWorking()
    {
      base.OnStartWorking();
      this.OnIsWorkingChanged();
    }

    protected override void OnStopWorking()
    {
      base.OnStopWorking();
      this.OnIsWorkingChanged();
    }

    private void OnIsWorkingChanged()
    {
      float effectivity = this.Effectivity;
      this.UpdateEffectivity();
      if ((double) this.Effectivity != (double) effectivity)
        return;
      this.UpdateVisuals();
    }

    public override bool GetIntersectionWithAABB(ref BoundingBoxD aabb)
    {
      this.Hierarchy.GetChildrenRecursive(this.m_children);
      foreach (MyEntity child in this.m_children)
      {
        MyModel model = child.Model;
        if (model != null && model.GetTrianglePruningStructure().GetIntersectionWithAABB((IMyEntity) child, ref aabb))
          return true;
      }
      MyModel model1 = this.Model;
      return model1 != null && model1.GetTrianglePruningStructure().GetIntersectionWithAABB((IMyEntity) this, ref aabb);
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      if (this.Enabled)
        return;
      this.UpdateVisuals();
    }

    public override void RefreshModels(string modelPath, string modelCollisionPath)
    {
      base.RefreshModels(modelPath, modelCollisionPath);
      this.UpdateVisuals();
    }

    private void UpdateVisuals()
    {
      MyEmissiveColorStateResult result;
      if (!MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, this.GetEmissiveState(), out result))
        result.EmissiveColor = Color.Green;
      float speed = this.CurrentProductionRatio * this.BlockDefinition.TurbineRotationSpeed;
      foreach (MyWindTurbine.TurbineSubpart turbineSubpart in this.Subparts.Values)
      {
        turbineSubpart.Render.SetSpeed(speed);
        turbineSubpart.Render.SetColor(result.EmissiveColor);
      }
    }

    private MyStringHash GetEmissiveState()
    {
      this.CheckIsWorking();
      return this.IsWorking ? (this.GetOrCreateSharedComponent().IsEnabled && (double) this.Effectivity > 0.0 ? MyCubeBlock.m_emissiveNames.Working : MyCubeBlock.m_emissiveNames.Warning) : (this.IsFunctional ? MyCubeBlock.m_emissiveNames.Disabled : MyCubeBlock.m_emissiveNames.Damaged);
    }

    public void OnEnvironmentChanged()
    {
      this.UpdateVisuals();
      this.OnProductionChanged();
    }

    protected override void UpdateDetailedInfo(StringBuilder sb)
    {
      base.UpdateDetailedInfo(sb);
      MyStringId myStringId = (double) this.Effectivity <= 0.95 ? ((double) this.Effectivity <= 0.600000023841858 ? ((double) this.Effectivity <= 0.0 ? MySpaceTexts.Turbine_WindClearanceNone : MySpaceTexts.Turbine_WindClearancePoor) : MySpaceTexts.Turbine_WindClearanceGood) : MySpaceTexts.Turbine_WindClearanceOptimal;
      MyTexts.AppendFormat(sb, MySpaceTexts.Turbine_WindClearance, myStringId);
    }

    protected override MyEntitySubpart InstantiateSubpart(
      MyModelDummy subpartDummy,
      ref MyEntitySubpart.Data data)
    {
      return (MyEntitySubpart) new MyWindTurbine.TurbineSubpart();
    }

    public class TurbineSubpart : MyEntitySubpart
    {
      public MyWindTurbine Parent => (MyWindTurbine) base.Parent;

      public MyRenderComponentWindTurbine.TurbineRenderComponent Render => (MyRenderComponentWindTurbine.TurbineRenderComponent) base.Render;

      public override void InitComponents()
      {
        this.Render = (MyRenderComponentBase) new MyRenderComponentWindTurbine.TurbineRenderComponent();
        base.InitComponents();
      }
    }
  }
}
