// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyAirtightDoorGeneric
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyAirtightDoorBase), typeof (Sandbox.ModAPI.Ingame.IMyAirtightDoorBase)})]
  public abstract class MyAirtightDoorGeneric : MyDoorBase, Sandbox.ModAPI.IMyAirtightDoorBase, Sandbox.ModAPI.IMyDoor, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDoor, Sandbox.ModAPI.Ingame.IMyAirtightDoorBase
  {
    private MySoundPair m_sound;
    private MySoundPair m_openSound;
    private MySoundPair m_closeSound;
    protected float m_currOpening;
    protected float m_subpartMovementDistance = 2.5f;
    protected float m_openingSpeed = 0.3f;
    protected float m_currSpeed;
    private int m_lastUpdateTime;
    private static readonly float EPSILON = 1E-09f;
    protected List<MyEntitySubpart> m_subparts = new List<MyEntitySubpart>();
    protected List<HkConstraint> m_subpartConstraints = new List<HkConstraint>();
    protected List<HkFixedConstraintData> m_subpartConstraintsData = new List<HkFixedConstraintData>();
    protected static string[] m_emissiveTextureNames;
    protected Color m_prevEmissiveColor;
    protected float m_prevEmissivity = -1f;
    private HashSet<VRage.ModAPI.IMyEntity> m_children = new HashSet<VRage.ModAPI.IMyEntity>();
    private bool m_updated;
    private bool m_stateChange;

    public event Action<bool> DoorStateChanged;

    public event Action<Sandbox.ModAPI.IMyDoor, bool> OnDoorStateChanged;

    DoorStatus Sandbox.ModAPI.Ingame.IMyDoor.Status => (bool) this.m_open ? (1.0 - (double) this.m_currOpening >= (double) MyAirtightDoorGeneric.EPSILON ? DoorStatus.Opening : DoorStatus.Open) : ((double) this.m_currOpening >= (double) MyAirtightDoorGeneric.EPSILON ? DoorStatus.Closing : DoorStatus.Closed);

    public float OpenRatio => this.m_currOpening;

    void Sandbox.ModAPI.Ingame.IMyDoor.OpenDoor()
    {
      if (!this.IsWorking)
        return;
      switch (((Sandbox.ModAPI.Ingame.IMyDoor) this).Status)
      {
        case DoorStatus.Opening:
        case DoorStatus.Open:
          break;
        default:
          ((Sandbox.ModAPI.Ingame.IMyDoor) this).ToggleDoor();
          break;
      }
    }

    void Sandbox.ModAPI.Ingame.IMyDoor.CloseDoor()
    {
      if (!this.IsWorking)
        return;
      switch (((Sandbox.ModAPI.Ingame.IMyDoor) this).Status)
      {
        case DoorStatus.Closing:
        case DoorStatus.Closed:
          break;
        default:
          ((Sandbox.ModAPI.Ingame.IMyDoor) this).ToggleDoor();
          break;
      }
    }

    void Sandbox.ModAPI.Ingame.IMyDoor.ToggleDoor()
    {
      if (!this.IsWorking)
        return;
      this.m_open.Value = !(bool) this.m_open;
    }

    bool Sandbox.ModAPI.IMyDoor.IsFullyClosed => (double) this.m_currOpening < (double) MyAirtightDoorGeneric.EPSILON;

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public MyAirtightDoorGeneric()
    {
      this.m_currOpening = 0.0f;
      this.m_currSpeed = 0.0f;
      this.m_open.ValueChanged += (Action<SyncBase>) (x => this.DoChangeOpenClose());
    }

    private MyAirtightDoorGenericDefinition BlockDefinition => (MyAirtightDoorGenericDefinition) base.BlockDefinition;

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      this.ResourceSink = new MyResourceSinkComponent();
      this.ResourceSink.Init(MyStringHash.GetOrCompute(this.BlockDefinition.ResourceSinkGroup), this.BlockDefinition.PowerConsumptionMoving, new Func<float>(this.UpdatePowerInput));
      base.Init(builder, cubeGrid);
      this.NeedsWorldMatrix = false;
      MyObjectBuilder_AirtightDoorGeneric airtightDoorGeneric = (MyObjectBuilder_AirtightDoorGeneric) builder;
      this.m_open.SetLocalValue(airtightDoorGeneric.Open);
      this.m_currOpening = MathHelper.Clamp(airtightDoorGeneric.CurrOpening, 0.0f, 1f);
      this.m_openingSpeed = this.BlockDefinition.OpeningSpeed;
      this.m_sound = new MySoundPair(this.BlockDefinition.Sound);
      this.m_openSound = new MySoundPair(this.BlockDefinition.OpenSound);
      this.m_closeSound = new MySoundPair(this.BlockDefinition.CloseSound);
      this.m_subpartMovementDistance = this.BlockDefinition.SubpartMovementDistance;
      if (!this.Enabled || !this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
        this.UpdateDoorPosition();
      this.OnStateChange();
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink.Update();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.ResourceSink.Update();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.NeedsWorldMatrix = true;
    }

    protected virtual void FillSubparts()
    {
    }

    protected override void BeforeDelete()
    {
      this.CubeGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      this.DisposeConstraints();
      base.BeforeDelete();
    }

    public override void OnRemovedFromScene(object source)
    {
      this.DisposeConstraints();
      base.OnRemovedFromScene(source);
    }

    private void InitSubparts()
    {
      this.FillSubparts();
      MyCubeGridRenderCell orAddCell = this.CubeGrid.RenderData.GetOrAddCell(this.Position * this.CubeGrid.GridSize);
      foreach (MyEntitySubpart subpart in this.m_subparts)
      {
        subpart.Render.SetParent(0, orAddCell.ParentCullObject);
        subpart.NeedsWorldMatrix = false;
        subpart.InvalidateOnMove = false;
      }
      this.UpdateEmissivity(true);
      this.DisposeConstraints();
      if (!this.CubeGrid.CreatePhysics)
      {
        this.UpdateDoorPosition();
      }
      else
      {
        foreach (MyEntitySubpart subpart in this.m_subparts)
        {
          if (subpart.Physics != null)
          {
            subpart.Physics.Close();
            subpart.Physics = (MyPhysicsComponentBase) null;
          }
        }
        if (this.CubeGrid.Projector != null)
        {
          this.UpdateDoorPosition();
        }
        else
        {
          this.CreateConstraints();
          this.UpdateDoorPosition();
        }
      }
    }

    private void CreateConstraints()
    {
      this.UpdateDoorPosition();
      bool flag = !Sandbox.Game.Multiplayer.Sync.IsServer;
      foreach (MyEntitySubpart subpart in this.m_subparts)
      {
        if (subpart.Physics == null && subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0)
        {
          HkShape havokCollisionShape = subpart.ModelCollision.HavokCollisionShapes[0];
          subpart.Physics = (MyPhysicsComponentBase) new MyPhysicsBody((VRage.ModAPI.IMyEntity) subpart, flag ? RigidBodyFlag.RBF_STATIC : RigidBodyFlag.RBF_DOUBLED_KINEMATIC | RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
          Vector3 center = subpart.PositionComp.LocalVolume.Center;
          HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(subpart.PositionComp.LocalAABB.HalfExtents, 100f);
          volumeMassProperties.Volume = subpart.PositionComp.LocalAABB.Volume();
          subpart.GetPhysicsBody().CreateFromCollisionObject(havokCollisionShape, center, subpart.WorldMatrix, new HkMassProperties?(volumeMassProperties), 9);
          ((MyPhysicsBody) subpart.Physics).IsSubpart = true;
        }
        if (subpart.Physics != null)
        {
          if (!flag)
          {
            HkFixedConstraintData constraintData;
            HkConstraint constraint;
            this.CreateSubpartConstraint((MyEntity) subpart, out constraintData, out constraint);
            this.m_subpartConstraintsData.Add(constraintData);
            this.m_subpartConstraints.Add(constraint);
            this.CubeGrid.Physics.AddConstraint(constraint);
            constraint.SetVirtualMassInverse(Vector4.Zero, Vector4.One);
          }
          else
            subpart.Physics.Enabled = true;
        }
      }
      this.CubeGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      this.CubeGrid.OnHavokSystemIDChanged += new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      if (this.CubeGrid.Physics == null)
        return;
      this.UpdateHavokCollisionSystemID(this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, false);
    }

    private void DisposeConstraints()
    {
      for (int index = 0; index < this.m_subpartConstraints.Count; ++index)
      {
        HkConstraint subpartConstraint = this.m_subpartConstraints[index];
        HkFixedConstraintData constraintData = this.m_subpartConstraintsData[index];
        this.DisposeSubpartConstraint(ref subpartConstraint, ref constraintData);
      }
      this.m_subpartConstraints.Clear();
      this.m_subpartConstraintsData.Clear();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_AirtightDoorGeneric builderCubeBlock = (MyObjectBuilder_AirtightDoorGeneric) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.Open = (bool) this.m_open;
      builderCubeBlock.CurrOpening = this.m_currOpening;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.CubeGrid.Physics == null || this.m_subparts.Count == 0 || ((double) this.m_currSpeed == 0.0 || !this.Enabled) || (!this.IsWorking || !this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId)))
        return;
      this.UpdateDoorPosition();
    }

    public override bool GetIntersectionWithAABB(ref BoundingBoxD aabb)
    {
      this.Hierarchy.GetChildrenRecursive(this.m_children);
      foreach (MyEntity child in this.m_children)
      {
        MyModel model = child.Model;
        if (model != null && model.GetTrianglePruningStructure().GetIntersectionWithAABB((VRage.ModAPI.IMyEntity) child, ref aabb))
          return true;
      }
      MyModel model1 = this.Model;
      return model1 != null && model1.GetTrianglePruningStructure().GetIntersectionWithAABB((VRage.ModAPI.IMyEntity) this, ref aabb);
    }

    public override void UpdateBeforeSimulation()
    {
      if (this.m_stateChange && ((bool) this.m_open && 1.0 - (double) this.m_currOpening < (double) MyAirtightDoorGeneric.EPSILON || !(bool) this.m_open && (double) this.m_currOpening < (double) MyAirtightDoorGeneric.EPSILON))
      {
        if (this.m_soundEmitter != null && this.m_soundEmitter.Loop)
        {
          this.m_soundEmitter.StopSound(false);
          this.m_soundEmitter.PlaySingleSound(this.m_sound, skipToEnd: true);
        }
        this.m_currSpeed = 0.0f;
        if (!this.HasDamageEffect)
          this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
        this.ResourceSink.Update();
        this.RaisePropertiesChanged();
        if (!(bool) this.m_open)
        {
          this.DoorStateChanged.InvokeIfNotNull<bool>((bool) this.m_open);
          this.OnDoorStateChanged.InvokeIfNotNull<Sandbox.ModAPI.IMyDoor, bool>((Sandbox.ModAPI.IMyDoor) this, (bool) this.m_open);
        }
        this.m_stateChange = false;
      }
      if (this.m_soundEmitter != null && this.Enabled && (this.IsWorking && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId)) && (double) this.m_currSpeed != 0.0)
      {
        if (this.Open)
        {
          if (this.m_openSound.Equals((object) MySoundPair.Empty))
            this.StartSound(this.m_sound);
          else
            this.StartSound(this.m_openSound);
        }
        else if (this.m_closeSound.Equals((object) MySoundPair.Empty))
          this.StartSound(this.m_sound);
        else
          this.StartSound(this.m_closeSound);
      }
      base.UpdateBeforeSimulation();
      this.UpdateCurrentOpening();
      this.m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    private void UpdateCurrentOpening()
    {
      if (!this.Enabled || !this.IsWorking || !this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
        return;
      this.m_currOpening = MathHelper.Clamp(this.m_currOpening + this.m_currSpeed * ((float) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastUpdateTime) / 1000f), 0.0f, 1f);
    }

    protected abstract void UpdateDoorPosition();

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateEmissivity();
    }

    public override void OnAddedToScene(object source)
    {
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      base.OnAddedToScene(source);
      this.UpdateEmissivity();
    }

    protected virtual void UpdateEmissivity(bool force = false)
    {
    }

    protected void SetEmissive(Color color, float emissivity = 1f, bool force = false)
    {
      if (this.Render.RenderObjectIDs[0] == uint.MaxValue || !force && !(color != this.m_prevEmissiveColor) && (double) this.m_prevEmissivity == (double) emissivity)
        return;
      foreach (string emissiveTextureName in MyAirtightDoorGeneric.m_emissiveTextureNames)
        MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], emissiveTextureName, color, emissivity);
      this.m_prevEmissiveColor = color;
      this.m_prevEmissivity = emissivity;
    }

    public void ChangeOpenClose(bool open)
    {
      if (open == (bool) this.m_open)
        return;
      this.m_open.Value = open;
    }

    internal void DoChangeOpenClose()
    {
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(false);
      this.OnStateChange();
      this.RaisePropertiesChanged();
    }

    private void OnStateChange()
    {
      this.m_currSpeed = !(bool) this.m_open ? -this.m_openingSpeed : this.m_openingSpeed;
      this.ResourceSink.Update();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      this.m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds - 1;
      this.UpdateCurrentOpening();
      this.UpdateDoorPosition();
      if ((bool) this.m_open)
      {
        this.DoorStateChanged.InvokeIfNotNull<bool>((bool) this.m_open);
        this.OnDoorStateChanged.InvokeIfNotNull<Sandbox.ModAPI.IMyDoor, bool>((Sandbox.ModAPI.IMyDoor) this, (bool) this.m_open);
      }
      this.m_stateChange = true;
    }

    private void RecreateConstraints()
    {
      MyCubeGridRenderCell orAddCell = this.CubeGrid.RenderData.GetOrAddCell(this.Position * this.CubeGrid.GridSize);
      foreach (MyEntitySubpart subpart in this.m_subparts)
      {
        if (subpart.Closed || subpart.MarkedForClose)
          return;
        subpart.Render.SetParent(0, orAddCell.ParentCullObject);
        subpart.NeedsWorldMatrix = false;
        subpart.InvalidateOnMove = false;
      }
      this.DisposeConstraints();
      if (this.InScene && this.CubeGrid.Physics != null && (this.CubeGrid.Physics.IsInWorld || MyPhysicsExtensions.IsInWorldWelded(this.CubeGrid.Physics)))
        this.CreateConstraints();
      if (this.CubeGrid.Physics != null)
        this.UpdateHavokCollisionSystemID(this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, false);
      this.UpdateDoorPosition();
    }

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
      this.UpdateEmissivity();
    }

    public override void OnBuildSuccess(long builtBy, bool instantBuild)
    {
      this.ResourceSink.Update();
      if (this.CubeGrid.Physics != null)
        this.UpdateHavokCollisionSystemID(this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, true);
      base.OnBuildSuccess(builtBy, instantBuild);
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      oldGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      this.CubeGrid.OnHavokSystemIDChanged += new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      if (this.CubeGrid.Physics != null)
        this.UpdateHavokCollisionSystemID(this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, true);
      if (this.InScene)
      {
        MyCubeGridRenderCell orAddCell = this.CubeGrid.RenderData.GetOrAddCell(this.Position * this.CubeGrid.GridSize);
        foreach (MyEntity subpart in this.m_subparts)
          subpart.Render.SetParent(0, orAddCell.ParentCullObject);
      }
      base.OnCubeGridChanged(oldGrid);
    }

    private void CubeGrid_OnHavokSystemIDChanged(int id)
    {
      bool flag = true;
      foreach (MyEntitySubpart subpart in this.m_subparts)
      {
        int num1 = flag ? 1 : 0;
        MyPhysicsComponentBase physics = subpart.Physics;
        int num2 = physics != null ? (physics.IsInWorld ? 1 : 0) : 0;
        flag = (num1 & num2) != 0;
      }
      if (!(this.CubeGrid.Physics != null & flag))
        return;
      this.UpdateHavokCollisionSystemID(this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, true);
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.InitSubparts();
      this.RecreateConstraints();
    }

    protected override void WorldPositionChanged(object source)
    {
      base.WorldPositionChanged(source);
      this.UpdateDoorPosition();
    }

    internal void UpdateHavokCollisionSystemID(int havokCollisionSystemID, bool refreshInPlace)
    {
      foreach (MyEntitySubpart subpart in this.m_subparts)
        MyDoorBase.SetupDoorSubpart(subpart, havokCollisionSystemID, refreshInPlace);
    }

    protected float UpdatePowerInput()
    {
      if (!this.Enabled || !this.IsFunctional)
        return 0.0f;
      return (double) this.m_currSpeed == 0.0 ? this.BlockDefinition.PowerConsumptionIdle : this.BlockDefinition.PowerConsumptionMoving;
    }

    protected bool IsEnoughPower() => this.ResourceSink != null && this.ResourceSink.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, this.BlockDefinition.PowerConsumptionMoving);

    private void StartSound(MySoundPair cuePair)
    {
      if (this.m_soundEmitter.Sound != null && this.m_soundEmitter.Sound.IsPlaying && (this.m_soundEmitter.SoundId == cuePair.Arcade || this.m_soundEmitter.SoundId == cuePair.Realistic))
        return;
      this.m_soundEmitter.StopSound(true);
      this.m_soundEmitter.PlaySingleSound(cuePair, true);
    }

    protected override void Closing()
    {
      this.CubeGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(true);
      base.Closing();
    }

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.UpdateEmissivity();
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      this.ResourceSink.Update();
      this.UpdateEmissivity();
    }
  }
}
