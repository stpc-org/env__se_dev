// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyThrust
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.RenderDirect.ActorComponents;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Thrust))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyThrust), typeof (Sandbox.ModAPI.Ingame.IMyThrust)})]
  public class MyThrust : MyFunctionalBlock, Sandbox.ModAPI.IMyThrust, Sandbox.ModAPI.Ingame.IMyThrust, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, IMyConveyorEndpointBlock
  {
    private static readonly uint TIMER_NORMAL_IN_FRAMES = 100;
    private static readonly uint TIMER_TIER1_PLAYER_IN_FRAMES = 3600;
    private static readonly uint TIMER_TIER1_DOUBLE_IN_FRAMES = 0;
    private float m_targetingTimeInFrames = (float) MyThrust.TIMER_NORMAL_IN_FRAMES;
    private Vector3D m_particleLocalOffset = Vector3D.Zero;
    private MyParticleEffect m_landingEffect;
    private static int m_maxNumberLandingEffects = 10;
    private static int m_landingEffectCount = 0;
    private MyPhysics.HitInfo? m_lastHitInfo;
    private MyEntityThrustComponent m_thrustComponent;
    public float ThrustLengthRand;
    private float m_maxBillboardDistanceSquared;
    private bool m_propellerActive;
    private MyEntity m_propellerEntity;
    private bool m_flamesCalculate;
    private bool m_propellerCalculate;
    private float m_propellerMaxDistance;
    private static readonly ConcurrentDictionary<string, List<MyThrustFlameAnimator.FlameInfo>> m_flameCache = new ConcurrentDictionary<string, List<MyThrustFlameAnimator.FlameInfo>>();
    private ListReader<MyThrustFlameAnimator.FlameInfo> m_flames;
    private const int FRAME_DELAY = 100;
    private static readonly List<HkBodyCollision> m_flameCollisionsList = new List<HkBodyCollision>();
    private int m_parallelThrustDamageTaskCount;
    public float LastKnownForceMultiplier;
    private float m_currentStrength;
    private bool m_renderNeedsUpdate;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_thrustOverride;
    private MyStringId m_flameLengthMaterialId;
    private MyStringId m_flamePointMaterialId;
    private static HashSet<HkShape> m_blockSet = new HashSet<HkShape>();
    private static List<VRage.ModAPI.IMyEntity> m_alreadyDamagedEntities = new List<VRage.ModAPI.IMyEntity>();
    private float m_thrustMultiplier = 1f;
    private float m_powerConsumptionMultiplier = 1f;
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;

    public MyThrustDefinition BlockDefinition { get; private set; }

    public MyRenderComponentThrust Render
    {
      set => this.Render = (MyRenderComponentBase) value;
      get => (MyRenderComponentThrust) base.Render;
    }

    public MyFuelConverterInfo FuelConverterDefinition { get; private set; }

    public MyFlareDefinition Flares { get; private set; }

    public MyGasProperties FuelDefinition { get; private set; }

    public MyEntity Propeller => this.m_propellerEntity;

    public Vector3 ThrustForce => -this.ThrustForwardVector * (this.BlockDefinition.ForceMagnitude * this.m_thrustMultiplier);

    public float ThrustForceLength => this.BlockDefinition.ForceMagnitude * this.m_thrustMultiplier;

    public float ThrustOverride
    {
      get => (float) ((double) (float) this.m_thrustOverride * (double) this.m_thrustMultiplier * (double) this.BlockDefinition.ForceMagnitude * 0.00999999977648258);
      set
      {
        float f = value / (float) ((double) this.m_thrustMultiplier * (double) this.BlockDefinition.ForceMagnitude * 0.00999999977648258);
        if (float.IsInfinity(f) || float.IsNaN(f))
          f = 0.0f;
        this.m_thrustOverride.Value = MathHelper.Clamp(f, 0.0f, 100f);
      }
    }

    public float ThrustOverrideOverForceLen => (float) this.m_thrustOverride * 0.01f;

    public bool IsOverridden => (double) this.m_thrustOverride.Value > 0.0;

    public Vector3I ThrustForwardVector => Base6Directions.GetIntVector(this.Orientation.Forward);

    public bool IsPowered => this.m_thrustComponent != null && this.m_thrustComponent.IsThrustPoweredByType((MyEntity) this, ref this.FuelDefinition.Id);

    public float MaxPowerConsumption => this.BlockDefinition.MaxPowerConsumption * this.m_powerConsumptionMultiplier;

    public float MinPowerConsumption => this.BlockDefinition.MinPowerConsumption * this.m_powerConsumptionMultiplier;

    public float CurrentStrength
    {
      get => this.m_currentStrength;
      set
      {
        if ((double) this.m_currentStrength == (double) value)
          return;
        this.m_currentStrength = value;
        this.InvokeRenderUpdate();
      }
    }

    public event Action<MyThrust, float> ThrustOverrideChanged;

    protected override bool CheckIsWorking() => this.IsPowered && base.CheckIsWorking();

    public static void RandomizeFlameProperties(
      float strength,
      float flameScale,
      ref float thrustRadiusRand,
      ref float thrustLengthRand)
    {
      thrustRadiusRand = MyUtils.GetRandomFloat(0.9f, 1.1f);
    }

    public ListReader<MyThrustFlameAnimator.FlameInfo> Flames => this.m_flames;

    public MyStringId FlameLengthMaterial => this.m_flameLengthMaterialId;

    public MyStringId FlamePointMaterial => this.m_flamePointMaterialId;

    public float FlameDamageLengthScale => this.BlockDefinition.FlameDamageLengthScale;

    public override bool IsTieredUpdateSupported => true;

    public MyThrust()
    {
      this.CreateTerminalControls();
      this.Render = new MyRenderComponentThrust();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentThrust(this));
      this.m_thrustOverride.ValueChanged += (Action<SyncBase>) (x => this.ThrustOverrideValueChanged());
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      if (!this.Enabled)
        return;
      this.m_thrustComponent.ResourceSink((MyEntity) this).ClearAllData();
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyThrust>())
        return;
      base.CreateTerminalControls();
      float threshold = 1f;
      MyTerminalControlSlider<MyThrust> slider = new MyTerminalControlSlider<MyThrust>("Override", MySpaceTexts.BlockPropertyTitle_ThrustOverride, MySpaceTexts.BlockPropertyDescription_ThrustOverride, true, true);
      slider.Getter = (MyTerminalValueControl<MyThrust, float>.GetterDelegate) (x => (float) ((double) (float) x.m_thrustOverride * (double) x.BlockDefinition.ForceMagnitude * 0.00999999977648258));
      slider.Setter = (MyTerminalValueControl<MyThrust, float>.SetterDelegate) ((x, v) =>
      {
        x.m_thrustOverride.Value = (double) v <= (double) threshold ? 0.0f : (float) ((double) v / (double) x.BlockDefinition.ForceMagnitude * 100.0);
        x.RaisePropertiesChanged();
      });
      slider.DefaultValue = new float?(0.0f);
      slider.SetLimits((MyTerminalValueControl<MyThrust, float>.GetterDelegate) (x => 0.0f), (MyTerminalValueControl<MyThrust, float>.GetterDelegate) (x => x.BlockDefinition.ForceMagnitude));
      slider.EnableActions<MyThrust>();
      slider.Writer = (MyTerminalControl<MyThrust>.WriterDelegate) ((x, result) =>
      {
        if ((double) x.ThrustOverride < 1.0)
          result.Append((object) MyTexts.Get(MyCommonTexts.Disabled));
        else
          MyValueFormatter.AppendForceInBestUnit(x.m_thrustComponent != null ? x.ThrustOverride * x.m_thrustComponent.GetLastThrustMultiplier((MyEntity) x) : 0.0f, result);
      });
      MyTerminalControlFactory.AddControl<MyThrust>((MyTerminalControl<MyThrust>) slider);
    }

    private void ThrustOverrideValueChanged() => this.ThrustOverrideChanged.InvokeIfNotNull<MyThrust, float>(this, this.ThrustOverride);

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_Thrust builderCubeBlock = (MyObjectBuilder_Thrust) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.ThrustOverride = this.ThrustOverride;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      MyEntityThrustComponent component;
      if (!cubeGrid.Components.TryGet<MyEntityThrustComponent>(out component))
      {
        component = (MyEntityThrustComponent) new MyThrusterBlockThrustComponent();
        component.Init();
        cubeGrid.Components.Add<MyEntityThrustComponent>(component);
      }
      this.m_thrustComponent = component;
      this.BlockDefinition = (MyThrustDefinition) base.BlockDefinition;
      MyDefinitionId defId = new MyDefinitionId();
      if (!this.BlockDefinition.FuelConverter.FuelId.IsNull())
        defId = (MyDefinitionId) this.BlockDefinition.FuelConverter.FuelId;
      this.m_flameLengthMaterialId = MyStringId.GetOrCompute(this.BlockDefinition.FlameLengthMaterial);
      this.m_flamePointMaterialId = MyStringId.GetOrCompute(this.BlockDefinition.FlamePointMaterial);
      MyGasProperties definition = (MyGasProperties) null;
      if (MyFakes.ENABLE_HYDROGEN_FUEL)
        MyDefinitionManager.Static.TryGetDefinition<MyGasProperties>(defId, out definition);
      MyGasProperties myGasProperties1 = definition;
      if (myGasProperties1 == null)
      {
        MyGasProperties myGasProperties2 = new MyGasProperties();
        myGasProperties2.Id = MyResourceDistributorComponent.ElectricityId;
        myGasProperties2.EnergyDensity = 1f;
        myGasProperties1 = myGasProperties2;
      }
      this.FuelDefinition = myGasProperties1;
      base.Init(objectBuilder, cubeGrid);
      this.NeedsWorldMatrix = false;
      this.InvalidateOnMove = false;
      this.m_thrustOverride.SetLocalValue(MathHelper.Clamp(((MyObjectBuilder_Thrust) objectBuilder).ThrustOverride, 0.0f, this.BlockDefinition.ForceMagnitude) * 100f / this.BlockDefinition.ForceMagnitude);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      if (!(MyDefinitionManager.Static.GetDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FlareDefinition), this.BlockDefinition.FlameFlare)) is MyFlareDefinition myFlareDefinition))
        myFlareDefinition = new MyFlareDefinition();
      this.Flares = myFlareDefinition;
      this.m_maxBillboardDistanceSquared = this.BlockDefinition.FlameVisibilityDistance * this.BlockDefinition.FlameVisibilityDistance;
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      MyFuelConverterInfo fuelConverterInfo;
      if (MyFakes.ENABLE_HYDROGEN_FUEL)
      {
        fuelConverterInfo = this.BlockDefinition.FuelConverter;
      }
      else
      {
        fuelConverterInfo = new MyFuelConverterInfo();
        fuelConverterInfo.Efficiency = 1f;
      }
      this.FuelConverterDefinition = fuelConverterInfo;
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.CubeBlock_OnWorkingChanged);
      this.CreateUpdateTimer(this.GetTimerTime(0), MyTimerTypes.Frame100);
    }

    protected override MyEntitySubpart InstantiateSubpart(
      MyModelDummy subpartDummy,
      ref MyEntitySubpart.Data data)
    {
      MyEntitySubpart myEntitySubpart = base.InstantiateSubpart(subpartDummy, ref data);
      myEntitySubpart.NeedsWorldMatrix = false;
      myEntitySubpart.Render = (MyRenderComponentBase) new MyRenderComponentThrust.MyPropellerRenderComponent();
      return myEntitySubpart;
    }

    private bool LoadPropeller()
    {
      MyEntitySubpart myEntitySubpart;
      if (!this.BlockDefinition.PropellerUse || this.BlockDefinition.PropellerEntity == null || !this.Subparts.TryGetValue(this.BlockDefinition.PropellerEntity, out myEntitySubpart))
        return false;
      this.m_propellerEntity = (MyEntity) myEntitySubpart;
      this.m_propellerMaxDistance = this.BlockDefinition.PropellerMaxDistance * this.BlockDefinition.PropellerMaxDistance;
      return true;
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      base.OnCubeGridChanged(oldGrid);
      this.InvokeRenderUpdate();
    }

    private void InvokeRenderUpdate()
    {
      if (this.m_renderNeedsUpdate || Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      this.m_renderNeedsUpdate = true;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
    }

    private void RenderUpdate()
    {
      MyRenderComponentThrust render = this.Render;
      MyThrustDefinition blockDefinition = this.BlockDefinition;
      float currentStrength = this.m_currentStrength;
      render.UpdateFlameProperties(this.m_flamesCalculate && this.IsPowered, currentStrength);
      if (this.m_propellerActive)
      {
        float num = 0.0f;
        if (this.m_propellerCalculate)
          num = (double) currentStrength > 0.0 ? blockDefinition.PropellerFullSpeed : blockDefinition.PropellerIdleSpeed;
        render.UpdatePropellerSpeed(num * 6.283185f);
      }
      this.m_renderNeedsUpdate = false;
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
    }

    public override void OnRegisteredToGridSystems()
    {
      base.OnRegisteredToGridSystems();
      MyEntityThrustComponent component;
      if (!this.CubeGrid.Components.TryGet<MyEntityThrustComponent>(out component))
      {
        component = (MyEntityThrustComponent) new MyThrusterBlockThrustComponent();
        component.Init();
        this.CubeGrid.Components.Add<MyEntityThrustComponent>(component);
      }
      this.m_thrustComponent = component;
      if (this.IsFunctional)
        this.m_thrustComponent.Register((MyEntity) this, this.ThrustForwardVector, new Func<bool>(this.OnRegisteredToThrustComponent));
      this.m_thrustComponent.DampenersEnabled = this.CubeGrid.DampenersEnabled;
    }

    public void ClearThrustComponent() => this.m_thrustComponent = (MyEntityThrustComponent) null;

    private bool OnRegisteredToThrustComponent()
    {
      MyResourceSinkComponent resourceSinkComponent = this.m_thrustComponent.ResourceSink((MyEntity) this);
      resourceSinkComponent.IsPoweredChanged += new Action(this.Sink_IsPoweredChanged);
      resourceSinkComponent.Update();
      return true;
    }

    public override void OnUnregisteredFromGridSystems()
    {
      base.OnUnregisteredFromGridSystems();
      if (this.CubeGrid.MarkedForClose || this.m_thrustComponent == null)
        return;
      this.m_thrustComponent.ResourceSink((MyEntity) this).IsPoweredChanged -= new Action(this.Sink_IsPoweredChanged);
      if (!this.m_thrustComponent.IsRegistered((MyEntity) this, this.ThrustForwardVector))
        return;
      this.m_thrustComponent.Unregister((MyEntity) this, this.ThrustForwardVector);
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.LoadDummies();
    }

    public void Sink_IsPoweredChanged() => this.UpdateIsWorking();

    public void CubeBlock_OnWorkingChanged(MyCubeBlock block)
    {
      if (this.m_landingEffect != null)
      {
        this.m_landingEffect.Stop(false);
        this.m_landingEffect = (MyParticleEffect) null;
        --MyThrust.m_landingEffectCount;
      }
      bool flag;
      if (this.IsWorking)
      {
        flag = this.UpdateRenderDistance();
      }
      else
      {
        flag = this.m_flamesCalculate || this.m_flamesCalculate;
        this.m_flamesCalculate = false;
        this.m_propellerCalculate = false;
      }
      if (!flag)
        return;
      this.InvokeRenderUpdate();
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      if (this.IsFunctional && (this.m_thrustComponent == null || !this.m_thrustComponent.IsRegistered((MyEntity) this, this.ThrustForwardVector)))
        this.OnRegisteredToGridSystems();
      else if (!this.IsFunctional && this.m_thrustComponent != null && this.m_thrustComponent.IsRegistered((MyEntity) this, this.ThrustForwardVector))
        this.OnUnregisteredFromGridSystems();
      if (this.CubeGrid.GridSystems.ResourceDistributor == null)
        return;
      this.CubeGrid.GridSystems.ResourceDistributor.ConveyorSystem_OnPoweredChanged();
    }

    private void LoadDummies()
    {
      MyModel model = this.Model;
      this.m_flames = (ListReader<MyThrustFlameAnimator.FlameInfo>) MyThrust.m_flameCache.GetOrAdd<string, List<MyThrustFlameAnimator.FlameInfo>, MyModel>(model.AssetName, model, (Func<MyModel, string, List<MyThrustFlameAnimator.FlameInfo>>) ((m, _) =>
      {
        List<MyThrustFlameAnimator.FlameInfo> flameInfoList = new List<MyThrustFlameAnimator.FlameInfo>();
        foreach (KeyValuePair<string, MyModelDummy> keyValuePair in (IEnumerable<KeyValuePair<string, MyModelDummy>>) m.Dummies.OrderBy<KeyValuePair<string, MyModelDummy>, string>((Func<KeyValuePair<string, MyModelDummy>, string>) (s => s.Key)))
        {
          if (keyValuePair.Key.StartsWith("thruster_flame", StringComparison.InvariantCultureIgnoreCase))
            flameInfoList.Add(new MyThrustFlameAnimator.FlameInfo()
            {
              Position = keyValuePair.Value.Matrix.Translation,
              Direction = Vector3.Normalize(keyValuePair.Value.Matrix.Forward),
              Radius = Math.Max(keyValuePair.Value.Matrix.Scale.X, keyValuePair.Value.Matrix.Scale.Y) * 0.5f
            });
        }
        return flameInfoList;
      }));
      if (this.BlockDefinition != null)
        this.m_propellerActive = this.LoadPropeller();
      this.Render.UpdateFlameAnimatorData();
    }

    protected override void Closing()
    {
      if (this.m_landingEffect != null)
      {
        this.m_landingEffect.Stop(false);
        this.m_landingEffect = (MyParticleEffect) null;
        --MyThrust.m_landingEffectCount;
      }
      base.Closing();
    }

    public override void GetTerminalName(StringBuilder result)
    {
      string directionString = this.GetDirectionString();
      if (directionString == null)
        base.GetTerminalName(result);
      else
        result.Append(this.DisplayNameText).Append(" (").Append(directionString).Append(") ");
    }

    public override void UpdateAfterSimulation10()
    {
      if (this.m_renderNeedsUpdate)
        this.RenderUpdate();
      base.UpdateAfterSimulation10();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.ThrustParticlesPositionUpdate();
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      this.ThrustParticles();
    }

    private void ThrustParticles()
    {
      if (!this.IsWorking || Sandbox.Game.Multiplayer.Sync.IsDedicated)
        return;
      Matrix localMatrix;
      this.GetLocalMatrix(out localMatrix);
      Vector3 translation = localMatrix.Translation;
      double gridScale = (double) this.CubeGrid.GridScale;
      foreach (MyThrustFlameAnimator.FlameInfo flame in this.Flames)
      {
        Vector3D normal = Vector3D.TransformNormal(flame.Direction, (MatrixD) ref localMatrix);
        Vector3D from = Vector3D.Transform(Vector3D.TransformNormal(flame.Position, (MatrixD) ref localMatrix) + translation, this.CubeGrid.WorldMatrix);
        Vector3D vector3D = Vector3D.TransformNormal(normal, this.CubeGrid.WorldMatrix);
        this.m_lastHitInfo = (double) this.ThrustLengthRand <= 9.99999974737875E-06 ? new MyPhysics.HitInfo?() : MyPhysics.CastRay(from, from + vector3D * (double) this.ThrustLengthRand * 2.5 * (double) flame.Radius, 15);
        MyEntity myEntity = this.m_lastHitInfo.HasValue ? this.m_lastHitInfo.Value.HkHitInfo.GetHitEntity() as MyEntity : (MyEntity) null;
        bool flag = false;
        string effectName = "Landing_Jet_Ground";
        if (myEntity != null)
        {
          if (myEntity is MyVoxelPhysics || myEntity is MyVoxelMap)
          {
            flag = true;
            MyVoxelBase self;
            if (myEntity is MyVoxelPhysics)
            {
              self = (myEntity as MyVoxelPhysics).RootVoxel;
              effectName = "Landing_Jet_Ground";
            }
            else
            {
              self = (MyVoxelBase) (myEntity as MyVoxelMap);
              effectName = "Landing_Jet_Ground_Dust";
            }
            Vector3D position = this.m_lastHitInfo.Value.Position;
            MyVoxelMaterialDefinition materialAt = self.GetMaterialAt(ref position);
            if (materialAt != null && !string.IsNullOrEmpty(materialAt.LandingEffect))
              effectName = materialAt.LandingEffect;
          }
          else if (myEntity.GetTopMostParent((System.Type) null) is MyCubeGrid && myEntity.GetTopMostParent((System.Type) null) != this.GetTopMostParent((System.Type) null))
          {
            flag = true;
            effectName = this.CubeGrid.GridSizeEnum != MyCubeSize.Large ? "Landing_Jet_Grid_Small" : "Landing_Jet_Grid_Large";
          }
        }
        if (!flag)
        {
          if (this.m_landingEffect != null)
          {
            this.m_landingEffect.Stop(false);
            this.m_landingEffect = (MyParticleEffect) null;
            --MyThrust.m_landingEffectCount;
            this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
          }
        }
        else if (this.m_landingEffect == null)
        {
          if (MyThrust.m_landingEffectCount < MyThrust.m_maxNumberLandingEffects && MyParticlesManager.TryCreateParticleEffect(effectName, MatrixD.CreateFromTransformScale(Quaternion.CreateFromForwardUp(-this.m_lastHitInfo.Value.HkHitInfo.Normal, Vector3.CalculatePerpendicularVector(this.m_lastHitInfo.Value.HkHitInfo.Normal)), this.m_lastHitInfo.Value.Position, Vector3D.One), out this.m_landingEffect))
          {
            ++MyThrust.m_landingEffectCount;
            this.m_landingEffect.UserScale = this.CubeGrid.GridSize;
            this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
          }
        }
        else if (this.m_lastHitInfo.HasValue)
          this.m_particleLocalOffset = Vector3D.Transform(this.m_lastHitInfo.Value.Position, this.PositionComp.WorldMatrixInvScaled);
      }
    }

    private void ThrustParticlesPositionUpdate()
    {
      if (this.m_landingEffect == null)
        return;
      Vector3D trans = Vector3D.Transform(this.m_particleLocalOffset, this.WorldMatrix);
      this.m_landingEffect.SetTranslation(ref trans);
    }

    private void ThrustDamageAsync(uint dmgTimeMultiplier)
    {
      if (this.m_flames.Count <= 0 || !MySession.Static.ThrusterDamage || (!this.IsWorking || !this.CubeGrid.InScene) || (this.CubeGrid.Physics == null || !this.CubeGrid.Physics.Enabled))
        return;
      if (!MySandboxGame.IsPaused)
        this.ThrustLengthRand = this.CurrentStrength * 10f * MyUtils.GetRandomFloat(0.6f, 1f) * this.BlockDefinition.FlameLengthScale;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || (double) this.CurrentStrength == 0.0 && !MyFakes.INACTIVE_THRUSTER_DMG || !MyFakes.INACTIVE_THRUSTER_DMG)
        return;
      foreach (MyThrustFlameAnimator.FlameInfo flame in this.m_flames)
      {
        MatrixD worldMatrix = this.WorldMatrix;
        this.ThrustDamageShapeCast(this.GetDamageCapsuleLine(flame, ref worldMatrix), flame, MyThrust.m_flameCollisionsList);
        this.ThrustDamageDealDamage(flame, MyThrust.m_flameCollisionsList, dmgTimeMultiplier);
      }
    }

    private void ThrustDamageShapeCast(
      LineD damageLine,
      MyThrustFlameAnimator.FlameInfo flameInfo,
      List<HkBodyCollision> outFlameCollisionsList)
    {
      HkShape shape = damageLine.Length == 0.0 ? (HkShape) new HkSphereShape(flameInfo.Radius * this.BlockDefinition.FlameDamageLengthScale) : (HkShape) new HkCapsuleShape(Vector3.Zero, (Vector3) (damageLine.To - damageLine.From), flameInfo.Radius * this.BlockDefinition.FlameDamageLengthScale);
      MyPhysics.GetPenetrationsShape(shape, ref damageLine.From, ref Quaternion.Identity, outFlameCollisionsList, 15);
      shape.RemoveReference();
    }

    private void ThrustDamageDealDamage(
      MyThrustFlameAnimator.FlameInfo flameInfo,
      List<HkBodyCollision> flameCollisionsList,
      uint dmgTimeMultiplier)
    {
      using (MyUtils.ReuseCollection<HkShape>(ref MyThrust.m_blockSet))
      {
        using (MyUtils.ReuseCollection<VRage.ModAPI.IMyEntity>(ref MyThrust.m_alreadyDamagedEntities))
        {
          foreach (HkBodyCollision flameCollisions in flameCollisionsList)
          {
            if (flameCollisions.GetCollisionEntity() is MyCubeGrid collisionEntity)
              MyThrust.m_blockSet.Add(collisionEntity.Physics.RigidBody.GetShape().GetContainer().GetShape(flameCollisions.ShapeKey));
          }
          foreach (HkBodyCollision flameCollisions in flameCollisionsList)
          {
            VRage.ModAPI.IMyEntity myEntity = flameCollisions.GetCollisionEntity();
            if (myEntity != null && !myEntity.Equals((object) this))
            {
              if (!(myEntity is MyCharacter))
                myEntity = myEntity.GetTopMostParent();
              if (!MyThrust.m_alreadyDamagedEntities.Contains(myEntity))
              {
                MyThrust.m_alreadyDamagedEntities.Add(myEntity);
                if (myEntity is IMyDestroyableObject)
                  (myEntity as IMyDestroyableObject).DoDamage(flameInfo.Radius * this.BlockDefinition.FlameDamage * (float) dmgTimeMultiplier, MyDamageType.Environment, true, attackerId: this.EntityId);
                else if (myEntity is MyCubeGrid)
                {
                  MyCubeGrid grid = myEntity as MyCubeGrid;
                  if (grid.BlocksDestructionEnabled)
                  {
                    MatrixD worldMatrix = this.WorldMatrix;
                    LineD damageCapsuleLine = this.GetDamageCapsuleLine(flameInfo, ref worldMatrix);
                    this.DamageGrid(flameInfo, damageCapsuleLine, grid, MyThrust.m_blockSet, dmgTimeMultiplier);
                  }
                }
              }
            }
          }
        }
      }
      flameCollisionsList.Clear();
    }

    private void DamageGrid(
      MyThrustFlameAnimator.FlameInfo flameInfo,
      LineD l,
      MyCubeGrid grid,
      HashSet<HkShape> shapes,
      uint dmgTimeMultiplier)
    {
      float num = flameInfo.Radius * this.BlockDefinition.FlameDamageLengthScale;
      Vector3 vector3 = new Vector3((double) num, (double) num, l.Length * 0.5);
      MatrixD boxTransform = this.WorldMatrix;
      boxTransform.Translation = (l.To - l.From) * 0.5 + l.From;
      BoundingBoxD box = new BoundingBoxD((Vector3D) -vector3, (Vector3D) vector3);
      MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD(box, boxTransform);
      List<MySlimBlock> blocks = new List<MySlimBlock>();
      grid.GetBlocksIntersectingOBB(in box, in boxTransform, blocks);
      foreach (MySlimBlock mySlimBlock in blocks)
      {
        if (mySlimBlock != this.SlimBlock && mySlimBlock != null && (this.CubeGrid.GridSizeEnum == MyCubeSize.Large || (double) mySlimBlock.BlockDefinition.DeformationRatio > 0.25))
        {
          List<HkShape> shapesFromPosition = mySlimBlock.CubeGrid.GetShapesFromPosition(mySlimBlock.Min);
          if (shapesFromPosition != null)
          {
            foreach (HkShape hkShape in shapesFromPosition)
            {
              if (shapes.Contains(hkShape))
                mySlimBlock.DoDamage((float) dmgTimeMultiplier * this.BlockDefinition.FlameDamage, MyDamageType.Environment, true, new MyHitInfo?(), this.EntityId);
            }
          }
        }
      }
    }

    public LineD GetDamageCapsuleLine(
      MyThrustFlameAnimator.FlameInfo info,
      ref MatrixD matrixWorld)
    {
      Vector3D from = Vector3D.Transform(info.Position, matrixWorld);
      Vector3D vector3D = (Vector3D) Vector3.TransformNormal(info.Direction, matrixWorld);
      float num = (float) ((double) this.ThrustLengthRand * (double) info.Radius * 0.5) * this.BlockDefinition.FlameDamageLengthScale;
      if ((double) num > (double) info.Radius)
        return new LineD(from, from + vector3D * (2.0 * (double) num - (double) info.Radius), 2.0 * (double) num - (double) info.Radius);
      return new LineD(from + vector3D * (double) num, from + vector3D * (double) num, 0.0)
      {
        Direction = vector3D
      };
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      this.UpdateIsWorking();
      if (!this.IsWorking)
        return;
      this.UpdateSoundState();
      if (!this.UpdateRenderDistance())
        return;
      this.RenderUpdate();
    }

    private bool UpdateRenderDistance()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return false;
      bool flag1 = false;
      double num = Vector3D.DistanceSquared(MySector.MainCamera.Position, this.PositionComp.GetPosition());
      bool flag2 = num < (double) this.m_maxBillboardDistanceSquared;
      if (flag2 != this.m_flamesCalculate)
      {
        flag1 = true;
        this.m_flamesCalculate = flag2;
      }
      if (this.m_propellerActive)
      {
        bool flag3 = num < (double) this.m_propellerMaxDistance;
        if (flag3 != this.m_propellerCalculate)
        {
          flag1 = true;
          this.m_propellerCalculate = flag3;
        }
      }
      return flag1;
    }

    private void UpdateSoundState()
    {
      if (this.m_soundEmitter == null)
        return;
      if ((double) this.CurrentStrength > 0.100000001490116)
      {
        if (!this.m_soundEmitter.IsPlaying)
          this.m_soundEmitter.PlaySound(this.BlockDefinition.PrimarySound, true);
      }
      else
        this.m_soundEmitter.StopSound(false);
      if (this.m_soundEmitter.Sound == null || !this.m_soundEmitter.Sound.IsPlaying)
        return;
      this.m_soundEmitter.Sound.FrequencyRatio = MyAudio.Static.SemitonesToFrequencyRatio((float) (8.0 * ((double) this.CurrentStrength - 0.5 * (double) MyConstants.MAX_THRUST)) / MyConstants.MAX_THRUST);
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.AppendFormat("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      if (this.FuelDefinition.Id.SubtypeName == "Electricity")
        MyValueFormatter.AppendWorkInBestUnit(this.MaxPowerConsumption, detailedInfo);
      else
        MyValueFormatter.AppendVolumeInBestUnit(this.MaxPowerConsumption, detailedInfo);
      detailedInfo.AppendFormat("\n");
    }

    private string GetDirectionString()
    {
      Vector3I gridThrustDirection = this.GridThrustDirection;
      if (gridThrustDirection != Vector3I.Zero)
      {
        if (gridThrustDirection.X == 1)
          return MyTexts.GetString(MyCommonTexts.Thrust_Left);
        if (gridThrustDirection.X == -1)
          return MyTexts.GetString(MyCommonTexts.Thrust_Right);
        if (gridThrustDirection.Y == 1)
          return MyTexts.GetString(MyCommonTexts.Thrust_Down);
        if (gridThrustDirection.Y == -1)
          return MyTexts.GetString(MyCommonTexts.Thrust_Up);
        if (gridThrustDirection.Z == 1)
          return MyTexts.GetString(MyCommonTexts.Thrust_Forward);
        if (gridThrustDirection.Z == -1)
          return MyTexts.GetString(MyCommonTexts.Thrust_Back);
      }
      return (string) null;
    }

    public Vector3I GridThrustDirection
    {
      get
      {
        if (!(MySession.Static.ControlledEntity is MyShipController myShipController))
          myShipController = this.CubeGrid.GridSystems.ControlSystem.GetShipController();
        if (myShipController == null)
          return Vector3I.Zero;
        Quaternion result;
        myShipController.Orientation.GetQuaternion(out result);
        return Vector3I.Transform(this.ThrustForwardVector, Quaternion.Inverse(result));
      }
    }

    float Sandbox.ModAPI.Ingame.IMyThrust.ThrustOverride
    {
      get => this.ThrustOverride;
      set => this.ThrustOverride = value;
    }

    float Sandbox.ModAPI.Ingame.IMyThrust.ThrustOverridePercentage
    {
      get => (float) this.m_thrustOverride / 100f;
      set => this.m_thrustOverride.Value = MathHelper.Clamp(value, 0.0f, 1f) * 100f;
    }

    float Sandbox.ModAPI.IMyThrust.ThrustMultiplier
    {
      get => this.m_thrustMultiplier;
      set
      {
        this.m_thrustMultiplier = value;
        if ((double) this.m_thrustMultiplier < 0.00999999977648258)
          this.m_thrustMultiplier = 0.01f;
        if (this.m_thrustComponent == null)
          return;
        this.m_thrustComponent.MarkDirty();
      }
    }

    float Sandbox.ModAPI.IMyThrust.PowerConsumptionMultiplier
    {
      get => this.m_powerConsumptionMultiplier;
      set
      {
        this.m_powerConsumptionMultiplier = value;
        if ((double) this.m_powerConsumptionMultiplier < 0.00999999977648258)
          this.m_powerConsumptionMultiplier = 0.01f;
        if (this.m_thrustComponent != null)
          this.m_thrustComponent.MarkDirty();
        this.SetDetailedInfoDirty();
        this.RaisePropertiesChanged();
      }
    }

    float Sandbox.ModAPI.Ingame.IMyThrust.MaxThrust => this.BlockDefinition.ForceMagnitude * this.m_thrustMultiplier;

    float Sandbox.ModAPI.Ingame.IMyThrust.MaxEffectiveThrust => this.m_thrustComponent == null ? 0.0f : this.BlockDefinition.ForceMagnitude * this.m_thrustMultiplier * this.m_thrustComponent.GetLastThrustMultiplier((MyEntity) this);

    float Sandbox.ModAPI.Ingame.IMyThrust.CurrentThrust => this.CurrentStrength * this.BlockDefinition.ForceMagnitude * this.m_thrustMultiplier;

    Vector3I Sandbox.ModAPI.Ingame.IMyThrust.GridThrustDirection => this.GridThrustDirection;

    public static Action<MyThrust, float> GetDelegate(Action<Sandbox.ModAPI.IMyThrust, float> value) => (Action<MyThrust, float>) Delegate.CreateDelegate(typeof (Action<MyThrust, float>), value.Target, value.Method);

    event Action<Sandbox.ModAPI.IMyThrust, float> Sandbox.ModAPI.IMyThrust.ThrustOverrideChanged
    {
      add => this.ThrustOverrideChanged += MyThrust.GetDelegate(value);
      remove => this.ThrustOverrideChanged -= MyThrust.GetDelegate(value);
    }

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public void InitializeConveyorEndpoint() => this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    public override void DoUpdateTimerTick()
    {
      base.DoUpdateTimerTick();
      this.ThrustDamageAsync(this.GetFramesFromLastTrigger());
    }

    public override bool GetTimerEnabledState() => this.Enabled && this.IsWorking;

    protected override void TiersChanged()
    {
      MyUpdateTiersPlayerPresence playerPresenceTier = this.CubeGrid.PlayerPresenceTier;
      MyUpdateTiersGridPresence gridPresenceTier = this.CubeGrid.GridPresenceTier;
      switch (playerPresenceTier)
      {
        case MyUpdateTiersPlayerPresence.Normal:
          this.ChangeTimerTick(this.GetTimerTime(0));
          return;
        case MyUpdateTiersPlayerPresence.Tier1:
        case MyUpdateTiersPlayerPresence.Tier2:
          if (gridPresenceTier == MyUpdateTiersGridPresence.Normal)
          {
            this.ChangeTimerTick(this.GetTimerTime(1));
            return;
          }
          if (gridPresenceTier == MyUpdateTiersGridPresence.Tier1)
          {
            this.ChangeTimerTick(this.GetTimerTime(2));
            return;
          }
          break;
      }
      this.ChangeTimerTick(this.GetTimerTime(0));
    }

    protected override uint GetDefaultTimeForUpdateTimer(int index)
    {
      switch (index)
      {
        case 0:
          return MyThrust.TIMER_NORMAL_IN_FRAMES;
        case 1:
          return MyThrust.TIMER_TIER1_PLAYER_IN_FRAMES;
        case 2:
          return MyThrust.TIMER_TIER1_DOUBLE_IN_FRAMES;
        default:
          return 0;
      }
    }

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    protected class m_thrustOverride\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyThrust) obj0).m_thrustOverride = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyThrust\u003C\u003EActor : IActivator, IActivator<MyThrust>
    {
      object IActivator.CreateInstance() => (object) new MyThrust();

      MyThrust IActivator<MyThrust>.CreateInstance() => new MyThrust();
    }
  }
}
