// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyPistonBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_PistonBase))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyPistonBase), typeof (Sandbox.ModAPI.Ingame.IMyPistonBase)})]
  public class MyPistonBase : MyMechanicalConnectionBlockBase, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyPistonBase, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyPistonBase
  {
    private HkConstraint m_subpartsConstraint;
    private MyPhysicsBody m_subpartPhysics;
    private MyEntitySubpart m_subpart1;
    private MyEntitySubpart m_subpart2;
    public MyEntitySubpart Subpart3;
    private Vector3 m_subpart1LocPos;
    private Vector3 m_subpart2LocPos;
    private Vector3 m_subpart3LocPos;
    private Vector3 m_constraintBasePos;
    private HkFixedConstraintData m_fixedData;
    private HkFixedConstraintData m_subpartsFixedData;
    private bool m_subPartContraintInScene;
    private MyAttachableConveyorEndpoint m_conveyorEndpoint;
    private bool m_posChanged;
    private Vector3 m_subpartsConstraintPos;
    private float m_lastPosition = float.MaxValue;
    private float m_currentPos;
    private float m_lastVelocity;
    private int m_ignoreNonAxialForcesForNMoreFrames;
    private const int IGNORE_NONAXIAL_FORCES_AFTER_VELOCITY_CHANGE_FOR_N_FRAMES = 5;
    public readonly VRage.Sync.Sync<float, SyncDirection.BothWays> Velocity;
    public readonly VRage.Sync.Sync<float, SyncDirection.BothWays> MinLimit;
    public readonly VRage.Sync.Sync<float, SyncDirection.BothWays> MaxLimit;
    public readonly VRage.Sync.Sync<float, SyncDirection.BothWays> MaxImpulseAxis;
    public readonly VRage.Sync.Sync<float, SyncDirection.BothWays> MaxImpulseNonAxis;

    private float Range => this.BlockDefinition.Maximum - this.BlockDefinition.Minimum;

    public MyPistonBaseDefinition BlockDefinition => (MyPistonBaseDefinition) ((MyCubeBlock) this).BlockDefinition;

    public float CurrentPosition => this.m_currentPos;

    public PistonStatus Status
    {
      get
      {
        if ((double) (float) this.Velocity < 0.0)
          return (double) this.m_currentPos > (double) (float) this.MinLimit ? PistonStatus.Retracting : PistonStatus.Retracted;
        if ((double) (float) this.Velocity <= 0.0)
          return PistonStatus.Stopped;
        return (double) this.m_currentPos < (double) (float) this.MaxLimit ? PistonStatus.Extending : PistonStatus.Extended;
      }
    }

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public float BreakOffTreshold => this.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2E+07f : 1000000f;

    private event Action<bool> LimitReached;

    public MyPistonBase()
    {
      this.CreateTerminalControls();
      this.Velocity.ValueChanged += (Action<SyncBase>) (o => this.UpdatePhysicsShape());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyPistonBase>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlButton<MyPistonBase> button1 = new MyTerminalControlButton<MyPistonBase>("Add Top Part", MySpaceTexts.BlockActionTitle_AddPistonHead, MySpaceTexts.BlockActionTooltip_AddPistonHead, (Action<MyPistonBase>) (b => b.RecreateTop()));
      button1.Enabled = (Func<MyPistonBase, bool>) (b => b.TopBlock == null);
      button1.EnableAction<MyPistonBase>(MyTerminalActionIcons.STATION_ON);
      MyTerminalControlFactory.AddControl<MyPistonBase>((MyTerminalControl<MyPistonBase>) button1);
      MyTerminalControlButton<MyPistonBase> button2 = new MyTerminalControlButton<MyPistonBase>("Reverse", MySpaceTexts.BlockActionTitle_Reverse, MySpaceTexts.Blank, (Action<MyPistonBase>) (x => x.Velocity.Value = -(float) x.Velocity));
      button2.Enabled = (Func<MyPistonBase, bool>) (b => b.IsFunctional);
      button2.EnableAction<MyPistonBase>(MyTerminalActionIcons.REVERSE);
      MyTerminalControlFactory.AddControl<MyPistonBase>((MyTerminalControl<MyPistonBase>) button2);
      MyTerminalControlFactory.AddAction<MyPistonBase>(new MyTerminalAction<MyPistonBase>("Extend", MyTexts.Get(MySpaceTexts.BlockActionTitle_Extend), new Action<MyPistonBase>(MyPistonBase.OnExtendApplied), (MyTerminalControl<MyPistonBase>.WriterDelegate) null, MyTerminalActionIcons.REVERSE)
      {
        Enabled = (Func<MyPistonBase, bool>) (b => b.IsFunctional)
      });
      MyTerminalControlFactory.AddAction<MyPistonBase>(new MyTerminalAction<MyPistonBase>("Retract", MyTexts.Get(MySpaceTexts.BlockActionTitle_Retract), new Action<MyPistonBase>(MyPistonBase.OnRetractApplied), (MyTerminalControl<MyPistonBase>.WriterDelegate) null, MyTerminalActionIcons.REVERSE)
      {
        Enabled = (Func<MyPistonBase, bool>) (b => b.IsFunctional)
      });
      MyTerminalControlSlider<MyPistonBase> slider1 = new MyTerminalControlSlider<MyPistonBase>("Velocity", MySpaceTexts.BlockPropertyTitle_Velocity, MySpaceTexts.Blank);
      slider1.SetLimits((MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (block => -block.BlockDefinition.MaxVelocity), (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (block => block.BlockDefinition.MaxVelocity));
      slider1.DefaultValue = new float?(-0.5f);
      slider1.Getter = (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (x => (float) x.Velocity);
      slider1.Setter = (MyTerminalValueControl<MyPistonBase, float>.SetterDelegate) ((x, v) => x.Velocity.Value = v);
      slider1.Writer = (MyTerminalControl<MyPistonBase>.WriterDelegate) ((x, res) => res.AppendDecimal((float) x.Velocity, 1).Append("m/s"));
      slider1.EnableActionsWithReset<MyPistonBase>();
      MyTerminalControlFactory.AddControl<MyPistonBase>((MyTerminalControl<MyPistonBase>) slider1);
      MyTerminalControlSlider<MyPistonBase> slider2 = new MyTerminalControlSlider<MyPistonBase>("UpperLimit", MySpaceTexts.BlockPropertyTitle_MaximalDistance, MySpaceTexts.Blank);
      slider2.SetLimits((MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (block => block.BlockDefinition.Minimum), (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (block => block.BlockDefinition.Maximum));
      slider2.DefaultValueGetter = (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (block => block.BlockDefinition.Maximum);
      slider2.Getter = (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (x => (float) x.MaxLimit);
      slider2.Setter = (MyTerminalValueControl<MyPistonBase, float>.SetterDelegate) ((x, v) => x.MaxLimit.Value = v);
      slider2.Writer = (MyTerminalControl<MyPistonBase>.WriterDelegate) ((x, res) => res.AppendDecimal((float) x.MaxLimit, 1).Append("m"));
      slider2.EnableActions<MyPistonBase>();
      MyTerminalControlFactory.AddControl<MyPistonBase>((MyTerminalControl<MyPistonBase>) slider2);
      MyTerminalControlSlider<MyPistonBase> slider3 = new MyTerminalControlSlider<MyPistonBase>("LowerLimit", MySpaceTexts.BlockPropertyTitle_MinimalDistance, MySpaceTexts.Blank);
      slider3.SetLimits((MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (block => block.BlockDefinition.Minimum), (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (block => block.BlockDefinition.Maximum));
      slider3.DefaultValueGetter = (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (block => block.BlockDefinition.Minimum);
      slider3.Getter = (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (x => (float) x.MinLimit);
      slider3.Setter = (MyTerminalValueControl<MyPistonBase, float>.SetterDelegate) ((x, v) => x.MinLimit.Value = v);
      slider3.Writer = (MyTerminalControl<MyPistonBase>.WriterDelegate) ((x, res) => res.AppendDecimal((float) x.MinLimit, 1).Append("m"));
      slider3.EnableActions<MyPistonBase>();
      MyTerminalControlFactory.AddControl<MyPistonBase>((MyTerminalControl<MyPistonBase>) slider3);
      MyTerminalControlSlider<MyPistonBase> slider4 = new MyTerminalControlSlider<MyPistonBase>("MaxImpulseAxis", MySpaceTexts.BlockPropertyTitle_MaxImpulseAxis, MySpaceTexts.BlockPropertyTooltip_MaxImpulseAxis, true, true);
      slider4.SetLogLimits((MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (x => 100f), (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (x => !MySession.Static.IsRunningExperimental ? x.BlockDefinition.UnsafeImpulseThreshold : float.MaxValue));
      slider4.DefaultValueGetter = (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (block => block.BlockDefinition.DefaultMaxImpulseAxis);
      slider4.Getter = (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (x => (float) x.MaxImpulseAxis);
      slider4.Setter = (MyTerminalValueControl<MyPistonBase, float>.SetterDelegate) ((x, v) => x.MaxImpulseAxis.Value = v);
      slider4.Writer = (MyTerminalControl<MyPistonBase>.WriterDelegate) ((x, res) => MyPistonBase.WriteImpulse(res, (float) x.MaxImpulseAxis));
      slider4.AdvancedWriter = (MyTerminalControl<MyPistonBase>.AdvancedWriterDelegate) ((x, control, res) => MyPistonBase.ImpulseWriter(x, control, res, true));
      slider4.EnableActions<MyPistonBase>();
      MyTerminalControlFactory.AddControl<MyPistonBase>((MyTerminalControl<MyPistonBase>) slider4);
      MyTerminalControlSlider<MyPistonBase> slider5 = new MyTerminalControlSlider<MyPistonBase>("MaxImpulseNonAxis", MySpaceTexts.BlockPropertyTitle_MaxImpulseNonAxis, MySpaceTexts.BlockPropertyTooltip_MaxImpulseNonAxis, true, true);
      slider5.SetLogLimits((MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (x => 100f), (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (x => !MySession.Static.IsRunningExperimental ? x.BlockDefinition.UnsafeImpulseThreshold : float.MaxValue));
      slider5.DefaultValueGetter = (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (block => block.BlockDefinition.DefaultMaxImpulseNonAxis);
      slider5.Getter = (MyTerminalValueControl<MyPistonBase, float>.GetterDelegate) (x => (float) x.MaxImpulseNonAxis);
      slider5.Setter = (MyTerminalValueControl<MyPistonBase, float>.SetterDelegate) ((x, v) => x.MaxImpulseNonAxis.Value = v);
      slider5.Writer = (MyTerminalControl<MyPistonBase>.WriterDelegate) ((x, res) => MyPistonBase.WriteImpulse(res, (float) x.MaxImpulseNonAxis));
      slider5.AdvancedWriter = (MyTerminalControl<MyPistonBase>.AdvancedWriterDelegate) ((x, control, res) => MyPistonBase.ImpulseWriter(x, control, res, false));
      slider5.EnableActions<MyPistonBase>();
      MyTerminalControlFactory.AddControl<MyPistonBase>((MyTerminalControl<MyPistonBase>) slider5);
    }

    private static void ImpulseWriter(
      MyPistonBase block,
      MyGuiControlBlockProperty control,
      StringBuilder output,
      bool axial)
    {
      Vector4 vector4 = control.ColorMask;
      float impulse = axial ? block.MaxImpulseAxis.Value : block.MaxImpulseNonAxis.Value;
      if ((double) impulse > (double) block.BlockDefinition.UnsafeImpulseThreshold)
        vector4 = Color.Red.ToVector4();
      MyPistonBase.WriteImpulse(output, impulse);
      control.TitleLabel.ColorMask = vector4;
      control.ExtraInfoLabel.ColorMask = vector4;
    }

    private static void WriteImpulse(StringBuilder sb, float impulse)
    {
      if ((double) impulse < 1.00000001504747E+30)
      {
        MyValueFormatter.AppendForceInBestUnit(impulse, sb);
      }
      else
      {
        impulse /= 1E+30f;
        int num = 30;
        for (; (double) impulse > 1000.0; impulse /= 10f)
          ++num;
        sb.AppendDecimal(impulse, 0).Append('E').Append(num).Append(" N");
      }
    }

    private static void OnExtendApplied(MyPistonBase piston) => ((Sandbox.ModAPI.Ingame.IMyPistonBase) piston).Extend();

    private static void OnRetractApplied(MyPistonBase piston) => ((Sandbox.ModAPI.Ingame.IMyPistonBase) piston).Retract();

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.RequiredPowerInput, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink.Update();
      MyObjectBuilder_PistonBase builderPistonBase = objectBuilder as MyObjectBuilder_PistonBase;
      if (float.IsNaN(builderPistonBase.Velocity) || float.IsInfinity(builderPistonBase.Velocity))
        builderPistonBase.Velocity = 0.0f;
      this.Velocity.SetLocalValue(MathHelper.Clamp(builderPistonBase.Velocity, -1f, 1f) * this.BlockDefinition.MaxVelocity);
      MyCubeBlock.ClampExperimentalValue(ref builderPistonBase.MaxImpulseAxis, this.BlockDefinition.UnsafeImpulseThreshold);
      MyCubeBlock.ClampExperimentalValue(ref builderPistonBase.MaxImpulseNonAxis, this.BlockDefinition.UnsafeImpulseThreshold);
      VRage.Sync.Sync<float, SyncDirection.BothWays> maxImpulseAxis = this.MaxImpulseAxis;
      float? nullable = builderPistonBase.MaxImpulseAxis;
      double num1 = nullable.HasValue ? (double) nullable.GetValueOrDefault() : (double) this.BlockDefinition.DefaultMaxImpulseAxis;
      maxImpulseAxis.SetLocalValue((float) num1);
      VRage.Sync.Sync<float, SyncDirection.BothWays> maxImpulseNonAxis = this.MaxImpulseNonAxis;
      nullable = builderPistonBase.MaxImpulseNonAxis;
      double num2 = nullable.HasValue ? (double) nullable.GetValueOrDefault() : (double) this.BlockDefinition.DefaultMaxImpulseNonAxis;
      maxImpulseNonAxis.SetLocalValue((float) num2);
      this.MaxLimit.SetLocalValue(builderPistonBase.MaxLimit.HasValue ? Math.Min(Math.Max(this.DenormalizeDistance(builderPistonBase.MaxLimit.Value), this.BlockDefinition.Minimum), this.BlockDefinition.Maximum) : this.BlockDefinition.Maximum);
      this.MinLimit.SetLocalValue(builderPistonBase.MinLimit.HasValue ? Math.Max(Math.Min(this.DenormalizeDistance(builderPistonBase.MinLimit.Value), this.BlockDefinition.Maximum), this.BlockDefinition.Minimum) : this.BlockDefinition.Minimum);
      this.m_currentPos = MathHelper.Clamp(builderPistonBase.CurrentPosition, this.BlockDefinition.Minimum, this.BlockDefinition.Maximum);
      this.m_lastVelocity = this.Velocity.Value;
      this.MaxImpulseAxis.ValueChanged += (Action<SyncBase>) (x => this.OnUnsafeSettingsChanged());
      this.MaxImpulseNonAxis.ValueChanged += (Action<SyncBase>) (x => this.OnUnsafeSettingsChanged());
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_PistonBase builderCubeBlock = (MyObjectBuilder_PistonBase) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.Velocity = (float) this.Velocity / this.BlockDefinition.MaxVelocity;
      builderCubeBlock.MaxLimit = new float?(this.NormalizeDistance((float) this.MaxLimit));
      builderCubeBlock.MinLimit = new float?(this.NormalizeDistance((float) this.MinLimit));
      builderCubeBlock.CurrentPosition = this.m_currentPos;
      builderCubeBlock.MaxImpulseAxis = new float?(this.MaxImpulseAxis.Value);
      builderCubeBlock.MaxImpulseNonAxis = new float?(this.MaxImpulseNonAxis.Value);
      if (float.IsNaN(builderCubeBlock.Velocity) || float.IsInfinity(builderCubeBlock.Velocity))
        builderCubeBlock.Velocity = 0.0f;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.ResourceSink.Update();
    }

    private void OnPhysicsEnabledChanged()
    {
      if (this.m_subpartPhysics == null)
        return;
      if (this.CubeGrid.Physics.Enabled)
      {
        if (!this.m_subPartContraintInScene)
          return;
        this.m_subpartPhysics.Enabled = true;
      }
      else
        this.m_subpartPhysics.Enabled = false;
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      base.OnCubeGridChanged(oldGrid);
      oldGrid.OnPhysicsChanged -= new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      this.CubeGrid.OnPhysicsChanged += new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      if (oldGrid.Physics != null)
      {
        MyGridPhysics physics = oldGrid.Physics;
        physics.EnabledChanged = physics.EnabledChanged - new Action(this.OnPhysicsEnabledChanged);
      }
      if (this.CubeGrid.Physics == null)
        return;
      MyGridPhysics physics1 = this.CubeGrid.Physics;
      physics1.EnabledChanged = physics1.EnabledChanged + new Action(this.OnPhysicsEnabledChanged);
    }

    private void CubeGrid_OnPhysicsChanged(MyEntity obj)
    {
      if (this.CubeGrid.Physics == null)
        return;
      MyGridPhysics physics1 = this.CubeGrid.Physics;
      physics1.EnabledChanged = physics1.EnabledChanged - new Action(this.OnPhysicsEnabledChanged);
      MyGridPhysics physics2 = this.CubeGrid.Physics;
      physics2.EnabledChanged = physics2.EnabledChanged + new Action(this.OnPhysicsEnabledChanged);
    }

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.CubeGrid.OnPhysicsChanged += new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      if (this.CubeGrid.Physics == null)
        return;
      MyGridPhysics physics = this.CubeGrid.Physics;
      physics.EnabledChanged = physics.EnabledChanged + new Action(this.OnPhysicsEnabledChanged);
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private float NormalizeDistance(float value) => (value + this.BlockDefinition.Minimum) / this.Range;

    private float DenormalizeDistance(float value)
    {
      value = MathHelper.Clamp(value, 0.0f, 1f);
      return value * this.Range + this.BlockDefinition.Minimum;
    }

    private void LoadSubparts()
    {
      this.DisposeSubpartsPhysics();
      if (this.Closed || !this.Subparts.TryGetValue("PistonSubpart1", out this.m_subpart1) || (!this.m_subpart1.Subparts.TryGetValue("PistonSubpart2", out this.m_subpart2) || !this.m_subpart2.Subparts.TryGetValue("PistonSubpart3", out this.Subpart3)))
        return;
      MyModelDummy myModelDummy;
      if (this.Subpart3.Model.Dummies.TryGetValue("TopBlock", out myModelDummy))
        this.m_constraintBasePos = myModelDummy.Matrix.Translation;
      if (this.Model.Dummies.TryGetValue("subpart_PistonSubpart1", out myModelDummy))
      {
        this.m_subpartsConstraintPos = myModelDummy.Matrix.Translation;
        this.m_subpart1LocPos = this.m_subpartsConstraintPos;
      }
      if (this.m_subpart1.Model.Dummies.TryGetValue("subpart_PistonSubpart2", out myModelDummy))
        this.m_subpart2LocPos = myModelDummy.Matrix.Translation;
      if (this.m_subpart2.Model.Dummies.TryGetValue("subpart_PistonSubpart3", out myModelDummy))
        this.m_subpart3LocPos = myModelDummy.Matrix.Translation;
      if (!this.CubeGrid.CreatePhysics)
        return;
      this.InitSubpartsPhysics();
    }

    private void SetSubpartBodyOffset() => this.m_subpartPhysics.Center = (Vector3) (Vector3D.Transform(Vector3D.Transform(this.m_constraintBasePos, this.Subpart3.WorldMatrix), this.PositionComp.WorldMatrixNormalizedInv) - this.m_currentPos * Vector3.Up * 0.5f);

    private void InitSubpartsPhysics()
    {
      MyEntitySubpart subpart1 = this.m_subpart1;
      if (subpart1 == null || this.CubeGrid.Physics == null)
        return;
      this.m_subpartPhysics = (MyPhysicsBody) new MyPistonBase.PistonSubpartPhysicsComponent(this, (VRage.ModAPI.IMyEntity) this, (RigidBodyFlag) ((this.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 128 : 0) | 2048));
      HkCylinderShape hkCylinderShape = new HkCylinderShape(new Vector3(0.0f, -2f, 0.0f), new Vector3(0.0f, 2f, 0.0f), (float) ((double) this.CubeGrid.GridSize / 2.0 - 0.109999999403954), 0.05f);
      HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeCylinderVolumeMassProperties(new Vector3(0.0f, -2f, 0.0f), new Vector3(0.0f, 2f, 0.0f), this.CubeGrid.GridSize / 2f, 40f * this.CubeGrid.GridSize);
      volumeMassProperties.Mass = this.BlockDefinition.Mass;
      this.m_subpartPhysics.CreateFromCollisionObject((HkShape) hkCylinderShape, Vector3.Zero, subpart1.WorldMatrix, new HkMassProperties?(volumeMassProperties));
      this.m_subpartPhysics.RigidBody.Layer = this.CubeGrid.Physics.RigidBody.Layer;
      this.m_subpartPhysics.RigidBody.SetCollisionFilterInfo(HkGroupFilter.CalcFilterInfo(this.m_subpartPhysics.RigidBody.Layer, this.CubeGrid.Physics.HavokCollisionSystemID, 1, 1));
      hkCylinderShape.Base.RemoveReference();
      if ((HkReferenceObject) this.m_subpartPhysics.RigidBody2 != (HkReferenceObject) null)
        this.m_subpartPhysics.RigidBody2.Layer = 17;
      this.CubeGrid.OnHavokSystemIDChanged += new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      this.m_subpartPhysics.IsSubpart = true;
      this.CreateSubpartsConstraint(subpart1);
      this.SetSubpartBodyOffset();
      this.Physics = this.m_subpartPhysics;
      this.m_posChanged = true;
    }

    private void CubeGrid_OnHavokSystemIDChanged(int sysId)
    {
      if (this.CubeGrid.Physics == null || !((HkReferenceObject) this.CubeGrid.Physics.RigidBody != (HkReferenceObject) null) || (this.m_subpartPhysics == null || !((HkReferenceObject) this.m_subpartPhysics.RigidBody != (HkReferenceObject) null)))
        return;
      this.m_subpartPhysics.RigidBody.SetCollisionFilterInfo(HkGroupFilter.CalcFilterInfo(this.CubeGrid.Physics.RigidBody.Layer, sysId, 1, 1));
    }

    private void DisposeSubpartsPhysics()
    {
      if ((HkReferenceObject) this.m_subpartsConstraint != (HkReferenceObject) null)
        this.DisposeSubpartsConstraint();
      if (this.m_subpart1 == null || this.m_subpartPhysics == null)
        return;
      this.CubeGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      this.m_subpartPhysics.Enabled = false;
      this.m_subpartPhysics.Close();
      this.m_subpartPhysics = (MyPhysicsBody) null;
    }

    private void CreateSubpartsConstraint(MyEntitySubpart subpart)
    {
      this.m_subpartsFixedData = new HkFixedConstraintData();
      this.m_subpartsFixedData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
      this.m_subpartsFixedData.SetInertiaStabilizationFactor(1f);
      Vector3D position = this.Position * this.CubeGrid.GridSize + Vector3D.Transform(Vector3D.Transform(this.m_subpartsConstraintPos, this.WorldMatrix), this.CubeGrid.PositionComp.LocalMatrixRef);
      Matrix matrix1 = this.PositionComp.LocalMatrixRef;
      Vector3 forward = matrix1.Forward;
      matrix1 = this.PositionComp.LocalMatrixRef;
      Vector3 up = matrix1.Up;
      MatrixD world = MatrixD.CreateWorld(position, forward, up);
      world.Translation = this.CubeGrid.Physics.WorldToCluster(world.Translation);
      Matrix bodyATransform = (Matrix) ref world;
      Matrix matrix2 = subpart.PositionComp.LocalMatrixRef;
      this.m_subpartsFixedData.SetInWorldSpace(ref bodyATransform, ref matrix2, ref matrix2);
      this.m_subpartsConstraint = new HkConstraint(this.CubeGrid.Physics.RigidBody, this.m_subpartPhysics.RigidBody, (HkConstraintData) this.m_subpartsFixedData);
      this.m_subpartPhysics.RigidBody.SetCollisionFilterInfo(HkGroupFilter.CalcFilterInfo(this.CubeGrid.Physics.RigidBody.Layer, this.CubeGrid.Physics.HavokCollisionSystemID, 1, 1));
      if (this.m_subpartPhysics.IsInWorld)
        MyPhysics.RefreshCollisionFilter(this.m_subpartPhysics);
      this.m_subpartsConstraint.WantRuntime = true;
    }

    private void DisposeSubpartsConstraint()
    {
      if (this.m_subPartContraintInScene)
      {
        this.m_subPartContraintInScene = false;
        this.CubeGrid.Physics.RemoveConstraint(this.m_subpartsConstraint);
      }
      this.m_subpartsConstraint.Dispose();
      this.m_subpartsConstraint = (HkConstraint) null;
      this.m_subpartsFixedData = (HkFixedConstraintData) null;
    }

    private void CheckSubpartConstraint()
    {
      if (MyPhysicsBody.IsConstraintValid(this.m_subpartsConstraint))
        return;
      this.DisposeSubpartsConstraint();
      this.CreateSubpartsConstraint(this.m_subpart1);
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    private void Receiver_IsPoweredChanged() => this.UpdateIsWorking();

    public override void OnBuildSuccess(long builtBy, bool instantBuild)
    {
      this.LoadSubparts();
      base.OnBuildSuccess(builtBy, instantBuild);
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      if (!this.m_welded)
      {
        this.SetDetailedInfoDirty();
        this.RaisePropertiesChanged();
        this.UpdatePhysicsShape();
      }
      this.UpdateSoundState();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
    }

    private void UpdatePhysicsShape()
    {
      MyEntitySubpart subpart1 = this.m_subpart1;
      if (!this.m_posChanged || subpart1 == null || (this.m_subpartPhysics == null || (HkReferenceObject) this.m_subpartPhysics.RigidBody == (HkReferenceObject) null))
        return;
      this.m_posChanged = false;
      float num1 = Math.Abs((double) (float) this.Velocity < 0.0 ? (float) this.Velocity / 6f : 0.0f);
      float num2 = 0.0f;
      Vector3 vector3_1 = new Vector3(0.0f, (float) ((double) num2 - (double) this.m_currentPos * 0.5 + (double) num1 - 0.100000001490116), 0.0f);
      Vector3 vector3_2 = new Vector3(0.0f, num2 + this.m_currentPos * 0.5f - num1, 0.0f);
      if ((double) vector3_2.Y - (double) vector3_1.Y > 0.100000001490116)
      {
        HkShape shape1 = this.m_subpartPhysics.RigidBody.GetShape();
        if (shape1.ShapeType == HkShapeType.Cylinder)
        {
          double num3 = (double) Math.Abs(vector3_1.Y - vector3_2.Y);
          HkCylinderShape hkCylinderShape1 = (HkCylinderShape) shape1;
          double num4 = (double) Math.Abs(hkCylinderShape1.VertexA.Y - hkCylinderShape1.VertexB.Y);
          if ((double) Math.Abs((float) (num3 - num4)) < 1.0 / 1000.0)
            return;
          hkCylinderShape1.VertexA = vector3_1;
          hkCylinderShape1.VertexB = vector3_2;
          int num5 = (int) this.m_subpartPhysics.RigidBody.UpdateShape();
          if ((HkReferenceObject) this.m_subpartPhysics.RigidBody2 != (HkReferenceObject) null)
          {
            HkShape shape2 = this.m_subpartPhysics.RigidBody2.GetShape();
            if (shape2.ShapeType == HkShapeType.Cylinder)
            {
              HkCylinderShape hkCylinderShape2 = (HkCylinderShape) shape2;
              hkCylinderShape2.VertexA = vector3_1;
              hkCylinderShape2.VertexB = vector3_2;
              int num6 = (int) this.m_subpartPhysics.RigidBody2.UpdateShape();
            }
          }
        }
        if (!this.m_subpartPhysics.Enabled)
        {
          this.SetSubpartBodyOffset();
          this.m_subpartPhysics.Enabled = true;
        }
        this.CheckSubpartConstraint();
        this.UpdateSubpartFixedData();
        if (MyPhysicsExtensions.IsInWorldWelded(this.CubeGrid.Physics) && MyPhysicsExtensions.IsInWorldWelded(this.m_subpartPhysics) && ((HkReferenceObject) this.m_subpartsConstraint != (HkReferenceObject) null && !this.m_subpartsConstraint.InWorld) && !this.m_subPartContraintInScene)
        {
          this.m_subPartContraintInScene = true;
          this.CubeGrid.Physics.AddConstraint(this.m_subpartsConstraint);
        }
        if (!((HkReferenceObject) this.m_subpartsConstraint != (HkReferenceObject) null) || this.m_subpartsConstraint.Enabled)
          return;
        this.m_subPartContraintInScene = true;
        if (this.m_subpartsConstraint.ForceDisabled)
          return;
        this.m_subpartsConstraint.Enabled = true;
      }
      else
      {
        if (this.m_subpartsConstraint.Enabled && this.m_subpartsConstraint.InWorld)
        {
          this.m_subPartContraintInScene = false;
          this.m_subpartsConstraint.Enabled = false;
          this.CubeGrid.Physics.RemoveConstraint(this.m_subpartsConstraint);
        }
        this.m_subpartPhysics.Enabled = false;
      }
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      this.SetDetailedInfoDirty();
      if ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null && MyPhysicsConfig.EnablePistonImpulseDebugDraw)
      {
        float axial;
        float nonAxial;
        this.GetConstraintImpulses(this.m_constraint, out axial, out nonAxial);
        MyRenderProxy.DebugDrawText3D(this.WorldMatrix.Translation + this.WorldMatrix.Up, axial.ToString("F2"), Color.Yellow, 1f, true);
        MatrixD worldMatrix = this.WorldMatrix;
        Vector3D translation = worldMatrix.Translation;
        worldMatrix = this.WorldMatrix;
        Vector3D vector3D = worldMatrix.Up * 2.0;
        MyRenderProxy.DebugDrawText3D(translation + vector3D, nonAxial.ToString("F2"), Color.Blue, 1f, true);
      }
      if (this.m_welded)
        return;
      if ((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null && (HkReferenceObject) this.SafeConstraint.RigidBodyA == (HkReferenceObject) this.SafeConstraint.RigidBodyB)
      {
        this.SafeConstraint.Enabled = false;
      }
      else
      {
        this.UpdatePosition();
        if (this.m_ignoreNonAxialForcesForNMoreFrames > 0)
          --this.m_ignoreNonAxialForcesForNMoreFrames;
        if (this.m_subpartPhysics != null && (HkReferenceObject) this.m_subpartPhysics.RigidBody2 != (HkReferenceObject) null)
        {
          if (this.m_subpartPhysics.RigidBody.IsActive)
          {
            this.m_subpartPhysics.RigidBody2.LinearVelocity = this.m_subpartPhysics.LinearVelocity;
            this.m_subpartPhysics.RigidBody2.AngularVelocity = this.m_subpartPhysics.AngularVelocity;
          }
          else
            this.m_subpartPhysics.RigidBody2.Deactivate();
        }
        if (this.m_soundEmitter == null || !this.m_soundEmitter.IsPlaying || !this.m_lastPosition.Equals(float.MaxValue))
          return;
        this.m_soundEmitter.StopSound(true);
        this.m_lastPosition = this.m_currentPos;
      }
    }

    private float CalcHeadLinearDisplacement(bool positive)
    {
      float val1 = this.LinearDispacementOf(this.m_constraint);
      int num = 0;
      return positive ? Math.Max(val1, (float) num) : Math.Min(val1, (float) num);
    }

    private float LinearDispacementOf(HkConstraint constraint)
    {
      if ((HkReferenceObject) constraint == (HkReferenceObject) null || !constraint.InWorld)
        return 0.0f;
      Vector3 pivotA;
      Vector3 pivotB;
      constraint.GetPivotsInWorld(out pivotA, out pivotB);
      Vector3D up = this.WorldMatrix.Up;
      return Vector3.Dot(pivotB - pivotA, (Vector3) up);
    }

    private bool IsImpulseOverThreshold(bool ignoreNonAxialImpulse) => this.IsImpulseOverThreshold(this.m_constraint, ignoreNonAxialImpulse) || this.IsImpulseOverThreshold(this.m_subpartsConstraint, ignoreNonAxialImpulse);

    private bool IsImpulseOverThreshold(HkConstraint constraint, bool ignoreNonAxialImpulse)
    {
      if (!MyPhysicsConfig.EnablePistonImpulseChecking || (HkReferenceObject) constraint == (HkReferenceObject) null || !constraint.InWorld)
        return false;
      float axial;
      float nonAxial;
      this.GetConstraintImpulses(constraint, out axial, out nonAxial);
      if ((double) (float) this.Velocity > 0.0)
        axial = -axial;
      if ((double) axial >= (double) (float) this.MaxImpulseAxis)
        return true;
      return !ignoreNonAxialImpulse && (double) nonAxial >= (double) (float) this.MaxImpulseNonAxis;
    }

    private void GetConstraintImpulses(
      HkConstraint constraint,
      out float axial,
      out float nonAxial)
    {
      Vector3 vector3 = Vector3.TransformNormal(new Vector3(HkFixedConstraintData.GetSolverImpulseInLastStep(constraint, (byte) 0), HkFixedConstraintData.GetSolverImpulseInLastStep(constraint, (byte) 1), HkFixedConstraintData.GetSolverImpulseInLastStep(constraint, (byte) 2)), this.PositionComp.WorldMatrixNormalizedInv);
      axial = -vector3.Y;
      nonAxial = Math.Max(Math.Abs(vector3.X), Math.Abs(vector3.Z));
    }

    private void UpdatePosition(bool forceUpdate = false)
    {
      if (this.m_subpart1 == null || !this.IsWorking && !forceUpdate)
        return;
      if (Math.Sign(this.m_lastVelocity) != Math.Sign((float) this.Velocity))
      {
        this.m_lastVelocity = (float) this.Velocity;
        this.m_ignoreNonAxialForcesForNMoreFrames = 5;
      }
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      float num = (float) this.Velocity / 60f;
      bool flag4 = !this.IsImpulseOverThreshold(this.m_ignoreNonAxialForcesForNMoreFrames > 0);
      MyCubeGrid topGrid = this.TopGrid;
      if (topGrid != null)
      {
        if (topGrid == this.CubeGrid)
          flag4 = false;
        else if ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null && this.m_constraint.RigidBodyA.IsFixed && this.m_constraint.RigidBodyB.IsFixed)
          flag4 = false;
      }
      if (!forceUpdate)
      {
        if ((double) num < 0.0)
        {
          if ((double) this.m_currentPos > (double) (float) this.MinLimit)
          {
            flag3 = true;
            if (flag4)
            {
              MyCubeGrid root = MyGridPhysicalHierarchy.Static.GetRoot(this.CubeGrid);
              if (!MySessionComponentSafeZones.IsActionAllowed(root.GetPhysicalGroupAABB(), (MySafeZoneAction) 0, root.EntityId))
                flag2 = true;
              if (!flag2)
              {
                this.m_currentPos = Math.Max(this.m_currentPos + num, (float) this.MinLimit);
                flag1 = true;
                if ((double) this.m_currentPos <= (double) (float) this.MinLimit)
                  flag2 = true;
              }
            }
          }
        }
        else if ((double) this.m_currentPos < (double) (float) this.MaxLimit)
        {
          flag3 = true;
          if (flag4)
          {
            MyCubeGrid root = MyGridPhysicalHierarchy.Static.GetRoot(this.CubeGrid);
            if (!MySessionComponentSafeZones.IsActionAllowed(root.GetPhysicalGroupAABB(), (MySafeZoneAction) 0, root.EntityId))
              flag2 = true;
            if (!flag2)
            {
              this.m_currentPos = Math.Min(this.m_currentPos + num, (float) this.MaxLimit);
              flag1 = true;
              if ((double) this.m_currentPos >= (double) (float) this.MaxLimit)
                flag2 = true;
            }
          }
        }
      }
      if (flag2)
      {
        this.LimitReached.InvokeIfNotNull<bool>((double) num >= 0.0);
        this.StopMovingSound();
        this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
      }
      else if (!forceUpdate)
      {
        if (flag3)
        {
          if (this.TopGrid != null && this.TopGrid.Physics != null)
            this.TopGrid.Physics.RigidBody.Activate();
          if (this.CubeGrid != null && this.CubeGrid.Physics != null)
            this.CubeGrid.Physics.RigidBody.Activate();
          if (this.m_subpartPhysics != null)
            this.m_subpartPhysics.RigidBody.Activate();
        }
        if (flag1)
        {
          this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
          MyGridPhysicalGroupData.InvalidateSharedMassPropertiesCache(this.CubeGrid);
        }
        else
        {
          this.StopMovingSound();
          this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
        }
      }
      if (!(flag1 | forceUpdate))
        return;
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      this.UpdateAnimation();
      this.m_posChanged = true;
      if (this.CubeGrid == null)
        MySandboxGame.Log.WriteLine("CubeGrid is null");
      if (this.Subpart3 == null)
        MySandboxGame.Log.WriteLine("Subpart is null");
      if (this.TopGrid != null && this.TopGrid.Physics != null)
        this.FillFixedData();
      this.UpdateSubpartFixedData();
    }

    private void StopMovingSound()
    {
      if (this.m_soundEmitter == null || !this.m_soundEmitter.IsPlaying)
        return;
      this.m_soundEmitter.StopSound(false);
    }

    private void UpdateSubpartFixedData()
    {
      Matrix topMatrixLocal = this.GetTopMatrixLocal();
      topMatrixLocal.Translation -= this.m_currentPos * this.PositionComp.LocalMatrixRef.Up * 0.5f;
      Matrix identity = Matrix.Identity;
      if ((HkReferenceObject) this.m_subpartsFixedData != (HkReferenceObject) null)
        this.m_subpartsFixedData.SetInBodySpace(topMatrixLocal, identity, (MyPhysicsBody) this.CubeGrid.Physics, this.m_subpartPhysics);
      else
        MySandboxGame.Log.WriteLine("m_subpartsFixedData is null");
    }

    protected override MatrixD GetTopGridMatrix()
    {
      this.UpdateAnimation();
      Vector3D position = Vector3D.Transform(this.m_constraintBasePos, this.Subpart3.WorldMatrix);
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D forward = worldMatrix.Forward;
      worldMatrix = this.WorldMatrix;
      Vector3D up = worldMatrix.Up;
      return MatrixD.CreateWorld(position, forward, up);
    }

    private Matrix GetTopMatrixLocal()
    {
      Matrix matrix = this.PositionComp.LocalMatrixRef;
      matrix.Translation = (Vector3) Vector3D.Transform(Vector3D.Transform(this.m_constraintBasePos, this.Subpart3.WorldMatrix), this.CubeGrid.PositionComp.WorldMatrixNormalizedInv);
      return matrix;
    }

    protected override void OnStartWorking()
    {
      base.OnStartWorking();
      this.StopMovingSound();
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(this.GetAttachState())).AppendLine();
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_PistonCurrentPosition)).AppendDecimal(this.m_currentPos, 1).Append("m");
      this.RaisePropertiesChanged();
    }

    private void UpdateAnimation()
    {
      double currentPos = (double) this.m_currentPos;
      float num1 = Math.Max(0.0f, Math.Min((float) (currentPos - 2.0 * (double) this.Range / 3.0), this.Range / 3f));
      if (this.m_subpart1 != null)
      {
        Matrix world = Matrix.CreateWorld(this.m_subpart1LocPos + Vector3.Up * num1, Vector3.Forward, Vector3.Up);
        this.m_subpart1.PositionComp.SetLocalMatrix(ref world);
      }
      int num2 = Sandbox.Game.Multiplayer.Sync.IsServer ? 1 : 0;
      float num3 = Math.Max(0.0f, Math.Min((float) (currentPos - (double) this.Range / 3.0), this.Range / 3f));
      if (this.m_subpart2 != null)
      {
        Matrix world = Matrix.CreateWorld(this.m_subpart2LocPos + Vector3.Up * num3, Vector3.Forward, Vector3.Up);
        this.m_subpart2.PositionComp.SetLocalMatrix(ref world);
      }
      float num4 = Math.Max(0.0f, Math.Min((float) currentPos, this.Range / 3f));
      if (this.Subpart3 == null)
        return;
      Matrix world1 = Matrix.CreateWorld(this.m_subpart3LocPos + Vector3.Up * num4, Vector3.Forward, Vector3.Up);
      this.Subpart3.PositionComp.SetLocalMatrix(ref world1);
    }

    protected override bool Attach(MyAttachableTopBlockBase topBlock, bool updateGroup = true)
    {
      if (!(topBlock is MyPistonTop myPistonTop) || !base.Attach(topBlock, updateGroup))
        return false;
      this.UpdateAnimation();
      this.CreateConstraint(topBlock);
      if (updateGroup)
        this.m_conveyorEndpoint.Attach(myPistonTop.ConveyorEndpoint as MyAttachableConveyorEndpoint);
      this.SetDetailedInfoDirty();
      return true;
    }

    private void FillFixedData()
    {
      if ((HkReferenceObject) this.m_fixedData == (HkReferenceObject) null)
        return;
      Matrix pivotB = Matrix.Identity;
      Matrix topMatrixLocal = this.GetTopMatrixLocal();
      MyAttachableTopBlockBase topBlock = this.TopBlock;
      if (topBlock != null)
      {
        Matrix matrix = topBlock.PositionComp.LocalMatrixRef;
        pivotB = Matrix.CreateWorld(this.TopBlock.Position * this.TopBlock.CubeGrid.GridSize, matrix.Forward, matrix.Up);
      }
      this.m_fixedData.SetInBodySpace(topMatrixLocal, pivotB, (MyPhysicsBody) this.CubeGrid.Physics, (MyPhysicsBody) this.TopGrid.Physics);
    }

    protected override bool CreateConstraint(MyAttachableTopBlockBase topBlock)
    {
      if (!base.CreateConstraint(topBlock))
        return false;
      this.m_fixedData = new HkFixedConstraintData();
      this.m_fixedData.SetInertiaStabilizationFactor(1f);
      this.m_fixedData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
      this.UpdateAnimation();
      this.FillFixedData();
      this.m_constraint = new HkConstraint(this.CubeGrid.Physics.RigidBody, topBlock.CubeGrid.Physics.RigidBody, (HkConstraintData) this.m_fixedData);
      this.m_constraint.WantRuntime = true;
      this.CubeGrid.Physics.AddConstraint(this.m_constraint);
      if (!this.m_constraint.InWorld)
      {
        this.CubeGrid.Physics.RemoveConstraint(this.m_constraint);
        this.m_constraint.Dispose();
        this.m_constraint = (HkConstraint) null;
        this.m_fixedData = (HkFixedConstraintData) null;
        return false;
      }
      this.m_constraint.Enabled = true;
      return true;
    }

    protected override bool CanPlaceTop(MyAttachableTopBlockBase topBlock, long builtBy)
    {
      float y = this.Subpart3.Model.BoundingBoxSize.Y;
      MatrixD worldMatrix = this.Subpart3.WorldMatrix;
      Vector3D translation1 = worldMatrix.Translation;
      worldMatrix = this.WorldMatrix;
      Vector3D vector3D = worldMatrix.Up * (double) y;
      Vector3D translation2 = translation1 + vector3D;
      float num = topBlock.ModelCollision.HavokCollisionShapes[0].ConvexRadius * 0.9f;
      BoundingSphereD boundingSphere = (BoundingSphereD) topBlock.Model.BoundingSphere;
      boundingSphere.Center = translation2;
      boundingSphere.Radius = (double) num;
      using (MyUtils.ReuseCollection<MySlimBlock>(ref MyMechanicalConnectionBlockBase.m_tmpSet))
      {
        this.CubeGrid.GetBlocksInsideSphere(ref boundingSphere, MyMechanicalConnectionBlockBase.m_tmpSet);
        if (MyMechanicalConnectionBlockBase.m_tmpSet.Count > 1)
        {
          if (builtBy == MySession.Static.LocalPlayerId)
            MyHud.Notifications.Add(MyNotificationSingletons.HeadNotPlaced);
          return false;
        }
      }
      using (MyUtils.ReuseCollection<HkBodyCollision>(ref MyMechanicalConnectionBlockBase.m_penetrations))
      {
        Quaternion identity = Quaternion.Identity;
        MyPhysics.GetPenetrationsShape(topBlock.ModelCollision.HavokCollisionShapes[0], ref translation2, ref identity, MyMechanicalConnectionBlockBase.m_penetrations, 15);
        for (int index = 0; index < MyMechanicalConnectionBlockBase.m_penetrations.Count; ++index)
        {
          VRage.ModAPI.IMyEntity collisionEntity = MyMechanicalConnectionBlockBase.m_penetrations[index].GetCollisionEntity();
          if (!collisionEntity.Physics.IsPhantom && (!(collisionEntity.GetTopMostParent() is MyCubeGrid topMostParent) || topMostParent != this.CubeGrid))
          {
            if (builtBy == MySession.Static.LocalPlayerId)
              MyHud.Notifications.Add(MyNotificationSingletons.HeadNotPlaced);
            return false;
          }
        }
      }
      return true;
    }

    public override void UpdateOnceBeforeFrame()
    {
      this.LoadSubparts();
      this.UpdateAnimation();
      if (!this.CubeGrid.IsPreview)
      {
        this.UpdatePosition(true);
        this.UpdatePhysicsShape();
      }
      base.UpdateOnceBeforeFrame();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
    }

    public override void OnRemovedFromScene(object source)
    {
      this.DisposeSubpartsPhysics();
      base.OnRemovedFromScene(source);
      this.CubeGrid.OnPhysicsChanged -= new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      if (this.CubeGrid.Physics == null)
        return;
      MyGridPhysics physics = this.CubeGrid.Physics;
      physics.EnabledChanged = physics.EnabledChanged - new Action(this.OnPhysicsEnabledChanged);
    }

    protected override void BeforeDelete()
    {
      this.DisposeSubpartsPhysics();
      base.BeforeDelete();
    }

    public void InitializeConveyorEndpoint()
    {
      this.m_conveyorEndpoint = new MyAttachableConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
    }

    private void UpdateSoundState()
    {
      if (!MySandboxGame.IsGameReady || this.m_soundEmitter == null || !this.IsWorking)
        return;
      if (this.TopGrid == null || this.TopGrid.Physics == null)
      {
        this.m_soundEmitter.StopSound(true);
      }
      else
      {
        if (this.IsWorking && !this.m_lastPosition.Equals(this.m_currentPos))
          this.m_soundEmitter.PlaySingleSound(this.BlockDefinition.PrimarySound, true);
        else
          this.m_soundEmitter.StopSound(false);
        this.m_lastPosition = this.m_currentPos;
      }
    }

    public override void ComputeTopQueryBox(
      out Vector3D pos,
      out Vector3 halfExtents,
      out Quaternion orientation)
    {
      MatrixD matrix = this.WorldMatrix;
      orientation = Quaternion.CreateFromRotationMatrix(in matrix);
      halfExtents = Vector3.One * this.CubeGrid.GridSize * 0.35f;
      halfExtents.Y = this.CubeGrid.GridSize * 0.25f;
      ref Vector3D local = ref pos;
      MatrixD worldMatrix = this.Subpart3.WorldMatrix;
      Vector3D translation = worldMatrix.Translation;
      double radius = this.Subpart3.PositionComp.WorldVolume.Radius;
      worldMatrix = this.WorldMatrix;
      Vector3D up1 = worldMatrix.Up;
      Vector3D vector3D1 = radius * up1;
      Vector3D vector3D2 = translation + vector3D1;
      double num = 0.5 * (double) this.CubeGrid.GridSize;
      worldMatrix = this.WorldMatrix;
      Vector3D up2 = worldMatrix.Up;
      Vector3D vector3D3 = num * up2;
      Vector3D vector3D4 = vector3D2 + vector3D3;
      local = vector3D4;
    }

    protected override void DisposeConstraint(MyCubeGrid topGrid)
    {
      if ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null)
      {
        this.m_fixedData = (HkFixedConstraintData) null;
        this.CubeGrid.Physics.RemoveConstraint(this.m_constraint);
        this.m_constraint.Dispose();
        this.m_constraint = (HkConstraint) null;
      }
      base.DisposeConstraint(topGrid);
    }

    protected override void Detach(MyCubeGrid grid, bool updateGroup = true)
    {
      if (this.TopBlock != null & updateGroup && this.TopBlock is MyPistonTop topBlock)
        this.m_conveyorEndpoint.Detach(topBlock.ConveyorEndpoint as MyAttachableConveyorEndpoint);
      base.Detach(grid, updateGroup);
    }

    protected override bool HasUnsafeSettingsCollector()
    {
      float impulseThreshold = this.BlockDefinition.UnsafeImpulseThreshold;
      return (double) this.MaxImpulseAxis.Value > (double) impulseThreshold || (double) this.MaxImpulseNonAxis.Value > (double) impulseThreshold || base.HasUnsafeSettingsCollector();
    }

    public void SetCurrentPosByTopGridMatrix()
    {
      Vector3 vector3 = this.m_subpart1LocPos + this.m_subpart2LocPos + this.m_subpart3LocPos + this.m_constraintBasePos;
      this.m_currentPos = (float) Vector3D.Transform(this.TopBlock.WorldMatrix.Translation, MatrixD.Invert(this.PositionComp.WorldMatrixRef)).Y - vector3.Length();
      this.UpdatePosition(true);
      this.UpdatePhysicsShape();
    }

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    float Sandbox.ModAPI.Ingame.IMyPistonBase.Velocity
    {
      get => (float) this.Velocity;
      set
      {
        value = MathHelper.Clamp(value, -this.BlockDefinition.MaxVelocity, this.BlockDefinition.MaxVelocity);
        this.Velocity.Value = value;
      }
    }

    float Sandbox.ModAPI.Ingame.IMyPistonBase.MaxVelocity => this.BlockDefinition.MaxVelocity;

    float Sandbox.ModAPI.Ingame.IMyPistonBase.MinLimit
    {
      get => (float) this.MinLimit;
      set
      {
        value = MathHelper.Clamp(value, this.BlockDefinition.Minimum, this.BlockDefinition.Maximum);
        this.MinLimit.Value = value;
      }
    }

    float Sandbox.ModAPI.Ingame.IMyPistonBase.MaxLimit
    {
      get => (float) this.MaxLimit;
      set
      {
        value = MathHelper.Clamp(value, this.BlockDefinition.Minimum, this.BlockDefinition.Maximum);
        this.MaxLimit.Value = value;
      }
    }

    float Sandbox.ModAPI.Ingame.IMyPistonBase.LowestPosition => this.BlockDefinition.Minimum;

    float Sandbox.ModAPI.Ingame.IMyPistonBase.HighestPosition => this.BlockDefinition.Maximum;

    private Action<MyMechanicalConnectionBlockBase> GetDelegate(
      Action<Sandbox.ModAPI.IMyPistonBase> value)
    {
      return (Action<MyMechanicalConnectionBlockBase>) Delegate.CreateDelegate(typeof (Action<MyMechanicalConnectionBlockBase>), value.Target, value.Method);
    }

    event Action<bool> Sandbox.ModAPI.IMyPistonBase.LimitReached
    {
      add => this.LimitReached += value;
      remove => this.LimitReached -= value;
    }

    event Action<Sandbox.ModAPI.IMyPistonBase> Sandbox.ModAPI.IMyPistonBase.AttachedEntityChanged
    {
      add => this.AttachedEntityChanged += this.GetDelegate(value);
      remove => this.AttachedEntityChanged -= this.GetDelegate(value);
    }

    void Sandbox.ModAPI.IMyPistonBase.Attach(Sandbox.ModAPI.IMyPistonTop top, bool updateGroup) => ((Sandbox.ModAPI.IMyMechanicalConnectionBlock) this).Attach((Sandbox.ModAPI.IMyAttachableTopBlock) top, updateGroup);

    void Sandbox.ModAPI.Ingame.IMyPistonBase.Extend()
    {
      if (!this.IsFunctional || (double) (float) this.Velocity >= 0.0)
        return;
      this.Velocity.Value = -(float) this.Velocity;
    }

    void Sandbox.ModAPI.Ingame.IMyPistonBase.Retract()
    {
      if (!this.IsFunctional || (double) (float) this.Velocity <= 0.0)
        return;
      this.Velocity.Value = -(float) this.Velocity;
    }

    void Sandbox.ModAPI.Ingame.IMyPistonBase.Reverse()
    {
      if (!this.IsFunctional)
        return;
      this.Velocity.Value = -(float) this.Velocity;
    }

    private class PistonSubpartPhysicsComponent : MyPhysicsBody
    {
      private MyPistonBase m_piston;

      public PistonSubpartPhysicsComponent(
        MyPistonBase piston,
        VRage.ModAPI.IMyEntity entity,
        RigidBodyFlag flags)
        : base(entity, flags)
      {
        this.m_piston = piston;
      }

      public override void OnWorldPositionChanged(object source)
      {
        if (source == null || source != this.m_piston.CubeGrid.Physics)
          base.OnWorldPositionChanged(source);
        else
          this.GetRigidBodyMatrix(out this.m_bodyMatrix);
      }

      private class Sandbox_Game_Entities_Blocks_MyPistonBase\u003C\u003EPistonSubpartPhysicsComponent\u003C\u003EActor
      {
      }
    }

    protected class Velocity\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyPistonBase) obj0).Velocity = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class MinLimit\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyPistonBase) obj0).MinLimit = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class MaxLimit\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyPistonBase) obj0).MaxLimit = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class MaxImpulseAxis\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyPistonBase) obj0).MaxImpulseAxis = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class MaxImpulseNonAxis\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyPistonBase) obj0).MaxImpulseNonAxis = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyPistonBase\u003C\u003EActor : IActivator, IActivator<MyPistonBase>
    {
      object IActivator.CreateInstance() => (object) new MyPistonBase();

      MyPistonBase IActivator<MyPistonBase>.CreateInstance() => new MyPistonBase();
    }
  }
}
