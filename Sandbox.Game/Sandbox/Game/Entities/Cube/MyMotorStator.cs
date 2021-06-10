// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyMotorStator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_MotorStator))]
  public class MyMotorStator : MyMotorBase, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyMotorStator, Sandbox.ModAPI.Ingame.IMyMotorStator, Sandbox.ModAPI.Ingame.IMyMotorBase, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyMotorBase, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
  {
    private const float NormalizedToRadians = 6.283185f;
    private const float DegreeToRadians = 0.01745329f;
    private const float RAD361 = 6.300639f;
    private static readonly float MIN_LOWER_LIMIT = -6.283185f - MathHelper.ToRadians(0.5f);
    private static readonly float MAX_UPPER_LIMIT = 6.283185f + MathHelper.ToRadians(0.5f);
    private static readonly float MAX_EXCEEDED_ANGLE_TO_CLAMP_TO_LIMITS = (float) Math.PI / 36f;
    public readonly VRage.Sync.Sync<float, SyncDirection.BothWays> Torque;
    public readonly VRage.Sync.Sync<float, SyncDirection.BothWays> BrakingTorque;
    public readonly VRage.Sync.Sync<float, SyncDirection.BothWays> TargetVelocity;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_minAngle;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_maxAngle;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_rotorLock;
    private HkVelocityConstraintMotor m_motor;
    private bool m_limitsActive;
    private float m_lockAngle;
    private bool m_isLockActive;
    private bool m_canLock;
    private bool m_delayedLock;
    protected bool m_canBeDetached;
    private float m_currentAngle;
    protected MyAttachableConveyorEndpoint m_conveyorEndpoint;
    private float m_currentAngleComputed;
    private bool m_resetDetailedInfo = true;
    private bool m_lastIsOpenedInTerminal;
    private bool m_isRotorFlipped;

    public bool IsLocked
    {
      get => (bool) this.m_rotorLock;
      set => this.m_rotorLock.Value = value;
    }

    public float TargetVelocityRPM
    {
      get => (float) this.TargetVelocity * 9.549296f;
      set => this.TargetVelocity.Value = value * ((float) Math.PI / 30f);
    }

    public float MinAngle
    {
      get => (float) this.m_minAngle / ((float) Math.PI / 180f);
      set => this.SetSafeAngles(false, value * ((float) Math.PI / 180f), (float) this.m_maxAngle);
    }

    public float MaxAngle
    {
      get => (float) this.m_maxAngle / ((float) Math.PI / 180f);
      set => this.SetSafeAngles(true, (float) this.m_minAngle, value * ((float) Math.PI / 180f));
    }

    protected override float ModelDummyDisplacement => this.MotorDefinition.RotorDisplacementInModel;

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    private event Action<bool> LimitReached;

    public MyMotorStator()
    {
      this.CreateTerminalControls();
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this, true);
      this.m_canBeDetached = true;
      this.SyncType.PropertyChanged += new Action<SyncBase>(this.SyncType_PropertyChanged);
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyMotorStator>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlButton<MyMotorStator> button1 = new MyTerminalControlButton<MyMotorStator>("AddRotorTopPart", MySpaceTexts.BlockActionTitle_AddRotorHead, MySpaceTexts.BlockActionTooltip_AddRotorHead, (Action<MyMotorStator>) (b => b.RecreateTop()));
      button1.Enabled = (Func<MyMotorStator, bool>) (b => b.TopBlock == null);
      button1.Visible = (Func<MyMotorStator, bool>) (b => b.MotorDefinition.RotorType == MyRotorType.Rotor);
      button1.EnableAction<MyMotorStator>((Func<MyMotorStator, bool>) (b => b.MotorDefinition.RotorType == MyRotorType.Rotor), MyTerminalActionIcons.STATION_ON);
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) button1);
      MyTerminalControlButton<MyMotorStator> button2 = new MyTerminalControlButton<MyMotorStator>("AddSmallRotorTopPart", MySpaceTexts.BlockActionTitle_AddSmallRotorHead, MySpaceTexts.BlockActionTooltip_AddSmallRotorHead, (Action<MyMotorStator>) (b => b.RecreateTop(smallToLarge: true)));
      button2.Enabled = (Func<MyMotorStator, bool>) (b => b.TopBlock == null);
      button2.Visible = (Func<MyMotorStator, bool>) (b => b.MotorDefinition.RotorType == MyRotorType.Rotor && b.CubeGrid.GridSizeEnum == MyCubeSize.Large);
      button2.EnableAction<MyMotorStator>((Func<MyMotorStator, bool>) (b => b.MotorDefinition.RotorType == MyRotorType.Rotor), MyTerminalActionIcons.STATION_ON);
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) button2);
      MyTerminalControlButton<MyMotorStator> button3 = new MyTerminalControlButton<MyMotorStator>("AddHingeTopPart", MySpaceTexts.BlockActionTitle_AddHingeHead, MySpaceTexts.BlockActionTooltip_AddHingeHead, (Action<MyMotorStator>) (b => b.RecreateTop()));
      button3.Enabled = (Func<MyMotorStator, bool>) (b => b.TopBlock == null);
      button3.Visible = (Func<MyMotorStator, bool>) (b => b.MotorDefinition.RotorType == MyRotorType.Hinge);
      button3.EnableAction<MyMotorStator>((Func<MyMotorStator, bool>) (b => b.MotorDefinition.RotorType == MyRotorType.Hinge), MyTerminalActionIcons.STATION_ON);
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) button3);
      MyTerminalControlButton<MyMotorStator> button4 = new MyTerminalControlButton<MyMotorStator>("AddSmallHingeTopPart", MySpaceTexts.BlockActionTitle_AddSmallHingeHead, MySpaceTexts.BlockActionTooltip_AddSmallHingeHead, (Action<MyMotorStator>) (b => b.RecreateTop(smallToLarge: true)));
      button4.Enabled = (Func<MyMotorStator, bool>) (b => b.TopBlock == null);
      button4.Visible = (Func<MyMotorStator, bool>) (b => b.MotorDefinition.RotorType == MyRotorType.Hinge && b.CubeGrid.GridSizeEnum == MyCubeSize.Large);
      button4.EnableAction<MyMotorStator>((Func<MyMotorStator, bool>) (b => b.MotorDefinition.RotorType == MyRotorType.Hinge && b.CubeGrid.GridSizeEnum == MyCubeSize.Large), MyTerminalActionIcons.STATION_ON);
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) button4);
      MyTerminalControlButton<MyMotorStator> button5 = new MyTerminalControlButton<MyMotorStator>("Reverse", MySpaceTexts.BlockActionTitle_Reverse, MySpaceTexts.Blank, (Action<MyMotorStator>) (b => b.TargetVelocityRPM = -b.TargetVelocityRPM));
      button5.EnableAction<MyMotorStator>(MyTerminalActionIcons.REVERSE);
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) button5);
      MyTerminalControlButton<MyMotorStator> button6 = new MyTerminalControlButton<MyMotorStator>("Detach", MySpaceTexts.BlockActionTitle_Detach, MySpaceTexts.Blank, (Action<MyMotorStator>) (b => b.CallDetach()));
      button6.Enabled = (Func<MyMotorStator, bool>) (b => b.m_connectionState.Value.TopBlockId.HasValue && !b.m_isWelding && !b.m_welded);
      button6.Visible = (Func<MyMotorStator, bool>) (b => b.m_canBeDetached);
      button6.EnableAction<MyMotorStator>(MyTerminalActionIcons.NONE).Enabled = (Func<MyMotorStator, bool>) (b => b.m_canBeDetached);
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) button6);
      MyTerminalControlButton<MyMotorStator> button7 = new MyTerminalControlButton<MyMotorStator>("Attach", MySpaceTexts.BlockActionTitle_Attach, MySpaceTexts.Blank, (Action<MyMotorStator>) (b => b.CallAttach()));
      button7.Enabled = (Func<MyMotorStator, bool>) (b => !b.m_connectionState.Value.TopBlockId.HasValue);
      button7.Visible = (Func<MyMotorStator, bool>) (b => b.m_canBeDetached);
      button7.EnableAction<MyMotorStator>(MyTerminalActionIcons.NONE).Enabled = (Func<MyMotorStator, bool>) (b => b.m_canBeDetached);
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) button7);
      MyTerminalControlCheckbox<MyMotorStator> checkbox1 = new MyTerminalControlCheckbox<MyMotorStator>("RotorLock", MySpaceTexts.BlockPropertyTitle_MotorLock, MySpaceTexts.BlockPropertyDescription_MotorLock);
      checkbox1.Getter = (MyTerminalValueControl<MyMotorStator, bool>.GetterDelegate) (x => x.IsLocked);
      checkbox1.Setter = (MyTerminalValueControl<MyMotorStator, bool>.SetterDelegate) ((x, v) => x.IsLocked = v);
      checkbox1.Visible = (Func<MyMotorStator, bool>) (x => x.MotorDefinition.RotorType == MyRotorType.Rotor);
      checkbox1.EnableAction<MyMotorStator>((Func<MyMotorStator, bool>) (x => x.MotorDefinition.RotorType == MyRotorType.Rotor));
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) checkbox1);
      MyTerminalControlCheckbox<MyMotorStator> checkbox2 = new MyTerminalControlCheckbox<MyMotorStator>("HingeLock", MySpaceTexts.BlockPropertyTitle_HingeLock, MySpaceTexts.BlockPropertyDescription_HingeLock);
      checkbox2.Getter = (MyTerminalValueControl<MyMotorStator, bool>.GetterDelegate) (x => x.IsLocked);
      checkbox2.Setter = (MyTerminalValueControl<MyMotorStator, bool>.SetterDelegate) ((x, v) => x.IsLocked = v);
      checkbox2.Visible = (Func<MyMotorStator, bool>) (x => x.MotorDefinition.RotorType == MyRotorType.Hinge);
      checkbox2.EnableAction<MyMotorStator>((Func<MyMotorStator, bool>) (x => x.MotorDefinition.RotorType == MyRotorType.Hinge));
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) checkbox2);
      MyTerminalControlSlider<MyMotorStator> slider1 = new MyTerminalControlSlider<MyMotorStator>("Torque", MySpaceTexts.BlockPropertyTitle_MotorTorque, MySpaceTexts.BlockPropertyDescription_MotorTorque);
      slider1.DynamicTooltipGetter = (MyTerminalControl<MyMotorStator>.TooltipGetter) (x => string.Format(MyTexts.GetString(MySpaceTexts.BlockPropertyDescription_MotorTorque), (object) MyTexts.GetString(x.MotorDefinition.RotorType == MyRotorType.Rotor ? MySpaceTexts.DisplayName_Block_Rotor : MySpaceTexts.DisplayName_Block_LargeHinge)));
      slider1.Getter = (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => (float) x.Torque);
      slider1.Setter = (MyTerminalValueControl<MyMotorStator, float>.SetterDelegate) ((x, v) => x.Torque.Value = v);
      slider1.DefaultValueGetter = (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.MotorDefinition.MaxForceMagnitude);
      slider1.Writer = (MyTerminalControl<MyMotorStator>.WriterDelegate) ((x, result) => MyValueFormatter.AppendTorqueInBestUnit((float) x.Torque, result));
      slider1.AdvancedWriter = (MyTerminalControl<MyMotorStator>.AdvancedWriterDelegate) ((x, control, res) => MyMotorStator.TorqueWriter(x, control, res, false));
      slider1.EnableActions<MyMotorStator>();
      slider1.Denormalizer = (MyTerminalControlSlider<MyMotorStator>.FloatFunc) ((x, v) => x.DenormalizeTorque(v));
      slider1.Normalizer = (MyTerminalControlSlider<MyMotorStator>.FloatFunc) ((x, v) => x.NormalizeTorque(v));
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) slider1);
      MyTerminalControlSlider<MyMotorStator> slider2 = new MyTerminalControlSlider<MyMotorStator>("BrakingTorque", MySpaceTexts.BlockPropertyTitle_MotorBrakingTorque, MySpaceTexts.BlockPropertyDescription_MotorBrakingTorque);
      slider2.DynamicTooltipGetter = (MyTerminalControl<MyMotorStator>.TooltipGetter) (x => string.Format(MyTexts.GetString(MySpaceTexts.BlockPropertyDescription_MotorBrakingTorque), (object) MyTexts.GetString(x.MotorDefinition.RotorType == MyRotorType.Rotor ? MySpaceTexts.DisplayName_Block_Rotor : MySpaceTexts.DisplayName_Block_LargeHinge)));
      slider2.Getter = (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => (float) x.BrakingTorque);
      slider2.Setter = (MyTerminalValueControl<MyMotorStator, float>.SetterDelegate) ((x, v) => x.BrakingTorque.Value = v);
      slider2.DefaultValue = new float?(0.0f);
      slider2.Writer = (MyTerminalControl<MyMotorStator>.WriterDelegate) ((x, result) => MyValueFormatter.AppendTorqueInBestUnit((float) x.BrakingTorque, result));
      slider2.AdvancedWriter = (MyTerminalControl<MyMotorStator>.AdvancedWriterDelegate) ((x, control, res) => MyMotorStator.TorqueWriter(x, control, res, true));
      slider2.EnableActions<MyMotorStator>();
      slider2.Denormalizer = (MyTerminalControlSlider<MyMotorStator>.FloatFunc) ((x, v) => x.DenormalizeTorque(v));
      slider2.Normalizer = (MyTerminalControlSlider<MyMotorStator>.FloatFunc) ((x, v) => x.NormalizeTorque(v));
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) slider2);
      MyTerminalControlSlider<MyMotorStator> slider3 = new MyTerminalControlSlider<MyMotorStator>("Velocity", MySpaceTexts.BlockPropertyTitle_MotorTargetVelocity, MySpaceTexts.BlockPropertyDescription_MotorVelocity);
      slider3.DynamicTooltipGetter = (MyTerminalControl<MyMotorStator>.TooltipGetter) (x => string.Format(MyTexts.GetString(MySpaceTexts.BlockPropertyDescription_MotorVelocity), (object) MyTexts.GetString(x.MotorDefinition.RotorType == MyRotorType.Rotor ? MySpaceTexts.DisplayName_Block_Rotor : MySpaceTexts.DisplayName_Block_LargeHinge)));
      slider3.Getter = (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.TargetVelocityRPM);
      slider3.Setter = (MyTerminalValueControl<MyMotorStator, float>.SetterDelegate) ((x, v) => x.TargetVelocityRPM = v);
      slider3.DefaultValue = new float?(0.0f);
      slider3.Writer = (MyTerminalControl<MyMotorStator>.WriterDelegate) ((x, result) => result.Concat(x.TargetVelocityRPM, 2U).Append(" rpm"));
      slider3.EnableActionsWithReset<MyMotorStator>();
      slider3.Denormalizer = (MyTerminalControlSlider<MyMotorStator>.FloatFunc) ((x, v) => x.DenormalizeRPM(v));
      slider3.Normalizer = (MyTerminalControlSlider<MyMotorStator>.FloatFunc) ((x, v) => x.NormalizeRPM(v));
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) slider3);
      MyTerminalControlSlider<MyMotorStator> slider4 = new MyTerminalControlSlider<MyMotorStator>("LowerLimit", MySpaceTexts.BlockPropertyTitle_MotorMinAngle, MySpaceTexts.BlockPropertyDescription_MotorLowerLimit);
      slider4.Getter = (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.MinAngle);
      slider4.Setter = (MyTerminalValueControl<MyMotorStator, float>.SetterDelegate) ((x, v) => x.MinAngle = v);
      slider4.DefaultValueGetter = (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.MotorDefinition.MinAngleDeg ?? -361f);
      slider4.SetLimits((MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.MotorDefinition.MinAngleDeg ?? -361f), (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.MotorDefinition.MaxAngleDeg ?? 360f));
      slider4.Writer = (MyTerminalControl<MyMotorStator>.WriterDelegate) ((x, result) => MyMotorStator.WriteAngle((float) x.m_minAngle, result));
      slider4.EnableActions<MyMotorStator>();
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) slider4);
      MyTerminalControlSlider<MyMotorStator> slider5 = new MyTerminalControlSlider<MyMotorStator>("UpperLimit", MySpaceTexts.BlockPropertyTitle_MotorMaxAngle, MySpaceTexts.BlockPropertyDescription_MotorUpperLimit);
      slider5.Getter = (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.MaxAngle);
      slider5.Setter = (MyTerminalValueControl<MyMotorStator, float>.SetterDelegate) ((x, v) => x.MaxAngle = v);
      slider5.DefaultValueGetter = (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.MotorDefinition.MaxAngleDeg ?? 361f);
      slider5.SetLimits((MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.MotorDefinition.MinAngleDeg ?? -360f), (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.MotorDefinition.MaxAngleDeg ?? 361f));
      slider5.Writer = (MyTerminalControl<MyMotorStator>.WriterDelegate) ((x, result) => MyMotorStator.WriteAngle((float) x.m_maxAngle, result));
      slider5.EnableActions<MyMotorStator>();
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) slider5);
      MyTerminalControlSlider<MyMotorStator> slider6 = new MyTerminalControlSlider<MyMotorStator>("Displacement", MySpaceTexts.BlockPropertyTitle_MotorRotorDisplacement, MySpaceTexts.BlockPropertyDescription_MotorRotorDisplacement);
      slider6.Getter = (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.DummyDisplacement);
      slider6.Setter = (MyTerminalValueControl<MyMotorStator, float>.SetterDelegate) ((x, v) => x.DummyDisplacement = v);
      slider6.DefaultValueGetter = (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => 0.0f);
      slider6.SetLimits((MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.TopGrid == null || x.TopGrid.GridSizeEnum != MyCubeSize.Large ? x.MotorDefinition.RotorDisplacementMinSmall : x.MotorDefinition.RotorDisplacementMin), (MyTerminalValueControl<MyMotorStator, float>.GetterDelegate) (x => x.TopGrid == null || x.TopGrid.GridSizeEnum != MyCubeSize.Large ? x.MotorDefinition.RotorDisplacementMaxSmall : x.MotorDefinition.RotorDisplacementMax));
      slider6.Writer = (MyTerminalControl<MyMotorStator>.WriterDelegate) ((x, result) => MyValueFormatter.AppendDistanceInBestUnit(x.DummyDisplacement, result));
      slider6.Enabled = (Func<MyMotorStator, bool>) (b => b.m_isAttached);
      slider6.Visible = (Func<MyMotorStator, bool>) (b => (double) b.MotorDefinition.RotorDisplacementMax - (double) b.MotorDefinition.RotorDisplacementMin > 9.99999974737875E-06 || (double) b.MotorDefinition.RotorDisplacementMaxSmall - (double) b.MotorDefinition.RotorDisplacementMinSmall > 9.99999974737875E-06);
      slider6.EnableActions<MyMotorStator>(enabled: ((Func<MyMotorStator, bool>) (b => (double) b.MotorDefinition.RotorDisplacementMax - (double) b.MotorDefinition.RotorDisplacementMin > 9.99999974737875E-06 || (double) b.MotorDefinition.RotorDisplacementMaxSmall - (double) b.MotorDefinition.RotorDisplacementMinSmall > 9.99999974737875E-06)));
      MyTerminalControlFactory.AddControl<MyMotorStator>((MyTerminalControl<MyMotorStator>) slider6);
    }

    private static void TorqueWriter(
      MyMotorStator block,
      MyGuiControlBlockProperty control,
      StringBuilder output,
      bool braking)
    {
      Vector4 vector4 = control.ColorMask;
      double num = braking ? (double) block.BrakingTorque.Value : (double) block.Torque.Value;
      if (num > (double) block.MotorDefinition.UnsafeTorqueThreshold)
        vector4 = Color.Red.ToVector4();
      MyValueFormatter.AppendTorqueInBestUnit((float) num, output);
      control.TitleLabel.ColorMask = vector4;
      control.ExtraInfoLabel.ColorMask = vector4;
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_MotorStator builderMotorStator = (MyObjectBuilder_MotorStator) objectBuilder;
      if (!builderMotorStator.Torque.HasValue)
      {
        float max = this.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 3.36E+07f : 448000f;
        builderMotorStator.Torque = new float?(MathHelper.Clamp(this.DenormalizeTorque(builderMotorStator.Force), 0.0f, max));
        builderMotorStator.BrakingTorque = new float?(MathHelper.Clamp(this.DenormalizeTorque(builderMotorStator.Friction), 0.0f, max));
      }
      MyCubeBlock.ClampExperimentalValue(ref builderMotorStator.Torque, this.MotorDefinition.UnsafeTorqueThreshold);
      MyCubeBlock.ClampExperimentalValue(ref builderMotorStator.BrakingTorque, this.MotorDefinition.UnsafeTorqueThreshold);
      this.IsLocked = builderMotorStator.RotorLock || builderMotorStator.ForceWeld;
      this.Torque.SetLocalValue(MathHelper.Clamp(builderMotorStator.Torque.Value, 0.0f, this.MotorDefinition.MaxForceMagnitude));
      this.BrakingTorque.SetLocalValue(MathHelper.Clamp(builderMotorStator.BrakingTorque.Value, 0.0f, this.MotorDefinition.MaxForceMagnitude));
      this.TargetVelocity.SetLocalValue(MathHelper.Clamp(builderMotorStator.TargetVelocity * this.MaxRotorAngularVelocity, -this.MaxRotorAngularVelocity, this.MaxRotorAngularVelocity));
      this.m_weldSpeed.SetLocalValue(MathHelper.Clamp(builderMotorStator.WeldSpeed, 0.0f, MyGridPhysics.SmallShipMaxLinearVelocity()));
      this.m_limitsActive = builderMotorStator.LimitsActive;
      this.m_currentAngle = builderMotorStator.CurrentAngle;
      float? nullable = builderMotorStator.MinAngle;
      float num1 = nullable.HasValue ? nullable.GetValueOrDefault() : (this.MotorDefinition.MinAngleDeg.HasValue ? MathHelper.ToRadians(this.MotorDefinition.MinAngleDeg.Value) : float.NegativeInfinity);
      nullable = builderMotorStator.MaxAngle;
      float num2 = nullable.HasValue ? nullable.GetValueOrDefault() : (this.MotorDefinition.MaxAngleDeg.HasValue ? MathHelper.ToRadians(this.MotorDefinition.MaxAngleDeg.Value) : float.PositiveInfinity);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.SetSafeAngles(true, num1, num2);
      }
      else
      {
        this.m_minAngle.SetLocalValue(num1);
        this.m_maxAngle.SetLocalValue(num2);
      }
      this.m_dummyDisplacement.SetLocalValue(builderMotorStator.DummyDisplacement);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentMotorStator(this));
      this.m_canLock = false;
      this.m_delayedLock = this.IsLocked;
      if (!this.m_delayedLock)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_MotorStator builderCubeBlock = (MyObjectBuilder_MotorStator) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.Torque = new float?((float) this.Torque);
      builderCubeBlock.BrakingTorque = new float?((float) this.BrakingTorque);
      builderCubeBlock.TargetVelocity = (float) this.TargetVelocity / this.MaxRotorAngularVelocity;
      builderCubeBlock.MinAngle = float.IsNegativeInfinity((float) this.m_minAngle) ? new float?() : new float?((float) this.m_minAngle);
      builderCubeBlock.MaxAngle = float.IsPositiveInfinity((float) this.m_maxAngle) ? new float?() : new float?((float) this.m_maxAngle);
      builderCubeBlock.CurrentAngle = (HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null ? HkLimitedHingeConstraintData.GetCurrentAngle(this.SafeConstraint) : this.m_currentAngle;
      builderCubeBlock.LimitsActive = this.m_limitsActive;
      builderCubeBlock.DummyDisplacement = (float) this.m_dummyDisplacement;
      builderCubeBlock.ForceWeld = false;
      builderCubeBlock.RotorLock = this.IsLocked;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private void SyncType_PropertyChanged(SyncBase obj)
    {
      if (obj == this.m_dummyDisplacement && MyPhysicsBody.IsConstraintValid(this.m_constraint))
        this.SetConstraintPosition(this.TopBlock, (HkLimitedHingeConstraintData) this.m_constraint.ConstraintData);
      if (obj != this.BrakingTorque && obj != this.Torque)
        return;
      this.OnUnsafeSettingsChanged();
    }

    private float NormalizeRPM(float v) => (float) ((double) v / ((double) this.MaxRotorAngularVelocity * 9.54929637908936) / 2.0 + 0.5);

    private float DenormalizeRPM(float v) => (float) (((double) v - 0.5) * 2.0 * ((double) this.MaxRotorAngularVelocity * 9.54929637908936));

    public static void WriteAngle(float angleRad, StringBuilder result)
    {
      if (float.IsInfinity(angleRad))
        result.Append((object) MyTexts.Get(MySpaceTexts.BlockPropertyValue_MotorAngleUnlimited));
      else
        result.Concat(MathHelper.ToDegrees(angleRad), 0U).Append("°");
    }

    private float NormalizeTorque(float value)
    {
      if ((double) value == 0.0)
        return 0.0f;
      bool runningExperimental = MySession.Static.IsRunningExperimental;
      return MathHelper.InterpLogInv(value, 1f, runningExperimental ? this.MotorDefinition.MaxForceMagnitude : this.MotorDefinition.UnsafeTorqueThreshold);
    }

    private float DenormalizeTorque(float value)
    {
      if ((double) value == 0.0)
        return 0.0f;
      bool runningExperimental = MySession.Static.IsRunningExperimental;
      return MathHelper.InterpLog(value, 1f, runningExperimental ? this.MotorDefinition.MaxForceMagnitude : this.MotorDefinition.UnsafeTorqueThreshold);
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      if (!this.IsOpenedInTerminal && !this.m_resetDetailedInfo)
        return;
      float numberToMove = !((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null) || this.SafeConstraint.Enabled ? this.m_currentAngle : this.m_currentAngleComputed;
      detailedInfo.AppendStringBuilder(MyTexts.Get(this.GetAttachState())).AppendLine();
      if (!((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null))
        return;
      if (this.m_limitsActive && (double) (float) this.m_minAngle < 0.0 && ((double) (float) this.m_minAngle > -3.14159297943115 && (double) numberToMove > 3.14159297943115))
        numberToMove -= 6.283185f;
      float radians = this.MoveDown(this.MoveUp(numberToMove, -6.283185f, 6.283185f), 6.283185f, 6.283185f);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MotorCurrentAngle)).AppendDecimal(MathHelper.ToDegrees(radians), 0).Append("°");
      if (this.IsLocked)
      {
        if (this.m_isLockActive)
          return;
        detailedInfo.AppendLine();
        detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MotoLockOverrideDisabled));
      }
      else
      {
        if (this.m_limitsActive || float.IsNegativeInfinity((float) this.m_minAngle) && float.IsPositiveInfinity((float) this.m_maxAngle))
          return;
        detailedInfo.AppendLine();
        detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MotorLimitsDisabled));
      }
    }

    private void ScaleDown(float limit)
    {
      while ((double) this.m_currentAngle > (double) limit)
        this.m_currentAngle -= 6.283185f;
      this.SetAngleToPhysics();
    }

    private void ScaleUp(float limit)
    {
      while ((double) this.m_currentAngle < (double) limit)
        this.m_currentAngle += 6.283185f;
      this.SetAngleToPhysics();
    }

    private void SetAngleToPhysics()
    {
      if (!((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null))
        return;
      HkLimitedHingeConstraintData.SetCurrentAngle(this.SafeConstraint, !this.m_isRotorFlipped || (double) this.m_currentAngle >= 3.14159297943115 ? this.m_currentAngle : -this.m_currentAngle);
    }

    private void SetSafeAngles(bool lowerIsFixed, float lowerLimit, float upperLimit)
    {
      lowerLimit = MathHelper.Clamp(lowerLimit, -6.300639f, 6.283185f);
      upperLimit = MathHelper.Clamp(upperLimit, -6.283185f, 6.300639f);
      if ((double) this.m_currentAngle < (double) lowerLimit)
        this.ScaleUp(MyMotorStator.MIN_LOWER_LIMIT);
      if ((double) this.m_currentAngle > (double) upperLimit)
        this.ScaleDown(MyMotorStator.MAX_UPPER_LIMIT);
      if ((double) upperLimit < (double) lowerLimit)
      {
        if (lowerIsFixed)
          upperLimit = lowerLimit;
        else
          lowerLimit = upperLimit;
      }
      if ((double) lowerLimit < (double) MyMotorStator.MIN_LOWER_LIMIT)
        lowerLimit = float.NegativeInfinity;
      if ((double) upperLimit > (double) MyMotorStator.MAX_UPPER_LIMIT)
        upperLimit = float.PositiveInfinity;
      this.m_minAngle.Value = lowerLimit;
      this.m_maxAngle.Value = upperLimit;
      this.m_limitsActive = false;
      this.TryActivateLimits();
      if ((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null)
        this.m_currentAngle = HkLimitedHingeConstraintData.GetCurrentAngle(this.SafeConstraint);
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private float MoveUp(float numberToMove, float minimum, float moveByMultipleOf)
    {
      while ((double) numberToMove < (double) minimum)
        numberToMove += moveByMultipleOf;
      return numberToMove;
    }

    private float MoveDown(float numberToMove, float maximum, float moveByMultipleOf)
    {
      while ((double) numberToMove > (double) maximum)
        numberToMove -= moveByMultipleOf;
      return numberToMove;
    }

    private void TryActivateLimits(bool allowLock = false)
    {
      if (this.IsLocked)
      {
        if (!(!this.m_isLockActive & allowLock))
          return;
        this.m_currentAngle = this.MoveUp(this.m_currentAngle, 0.0f, 6.283185f);
        this.m_currentAngle = this.MoveDown(this.m_currentAngle, 6.283185f, 6.283185f);
        this.SetAngleToPhysics();
        this.m_isLockActive = true;
        this.m_limitsActive = false;
        this.m_lockAngle = this.m_currentAngle;
      }
      else
      {
        this.m_isLockActive = false;
        if (float.IsNegativeInfinity((float) this.m_minAngle) && float.IsPositiveInfinity((float) this.m_maxAngle))
        {
          this.m_currentAngle = this.MoveUp(this.m_currentAngle, 0.0f, 6.283185f);
          this.m_currentAngle = this.MoveDown(this.m_currentAngle, 6.283185f, 6.283185f);
          this.SetAngleToPhysics();
          this.m_limitsActive = false;
        }
        else
        {
          if (this.m_limitsActive)
            return;
          float minimum = (float) this.m_minAngle - MathHelper.ToRadians(2f);
          float maximum = (float) this.m_maxAngle + MathHelper.ToRadians(2f);
          float numberToMove = this.m_currentAngle;
          if ((double) numberToMove < (double) minimum)
            numberToMove = this.MoveUp(numberToMove, minimum, 6.283185f);
          else if ((double) numberToMove > (double) maximum)
            numberToMove = this.MoveDown(numberToMove, maximum, 6.283185f);
          if ((double) numberToMove < (double) minimum || (double) numberToMove > (double) maximum)
            return;
          this.m_limitsActive = true;
          this.m_currentAngle = numberToMove;
          this.SetAngleToPhysics();
        }
      }
    }

    private float GetAngle(Quaternion q, Vector3 axis)
    {
      float num = (float) (2.0 * Math.Atan2((double) new Vector3(q.X, q.Y, q.Z).Length(), (double) q.W));
      Vector3 vector3 = new Vector3(q.X, q.Y, q.Z) / (float) Math.Sin((double) num / 2.0);
      Vector3 vector1 = (double) num == 0.0 ? Vector3.Zero : vector3;
      return num * Vector3.Dot(vector1, axis);
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (!this.m_delayedLock)
        return;
      this.m_canLock = true;
      this.m_delayedLock = false;
      this.m_resetDetailedInfo = true;
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (!this.m_lastIsOpenedInTerminal && this.IsOpenedInTerminal)
        this.m_resetDetailedInfo = true;
      this.m_lastIsOpenedInTerminal = this.IsOpenedInTerminal;
      if (this.m_welded)
        return;
      HkConstraint safeConstraint = this.SafeConstraint;
      if (this.TopGrid == null || (HkReferenceObject) safeConstraint == (HkReferenceObject) null)
        return;
      if ((HkReferenceObject) safeConstraint.RigidBodyA == (HkReferenceObject) safeConstraint.RigidBodyB)
      {
        this.SafeConstraint.Enabled = false;
      }
      else
      {
        float f = 0.0f;
        bool allowLock = false;
        if (safeConstraint.Enabled)
        {
          f = this.m_currentAngle;
          this.m_currentAngle = HkLimitedHingeConstraintData.GetCurrentAngle(safeConstraint);
          allowLock = this.m_canLock;
          this.m_canLock = true;
          bool flag = !f.IsEqual(this.m_currentAngle);
          if (flag || !safeConstraint.Enabled || this.m_resetDetailedInfo)
          {
            this.SetDetailedInfoDirty();
            this.RaisePropertiesChanged();
          }
          if (allowLock & flag)
          {
            allowLock = 0.174532920122147 > (double) Math.Abs(f - this.m_currentAngle) / 0.0166666675359011;
            MyGridPhysicalGroupData.InvalidateSharedMassPropertiesCache(this.CubeGrid);
          }
        }
        else if (this.NeedsComputedAngle() || this.m_resetDetailedInfo)
        {
          f = this.m_currentAngleComputed;
          this.m_currentAngleComputed = this.ComputeCurrentAngle();
          allowLock = this.m_canLock;
          this.m_canLock = true;
          bool flag = !f.IsEqual(this.m_currentAngleComputed);
          this.SetDetailedInfoDirty();
          this.RaisePropertiesChanged();
          if (allowLock & flag)
            allowLock = 0.0872664600610733 > (double) Math.Abs(f - this.m_currentAngleComputed) / 0.0166666675359011;
        }
        this.m_resetDetailedInfo = false;
        HkLimitedHingeConstraintData constraintData = (HkLimitedHingeConstraintData) safeConstraint.ConstraintData;
        constraintData.MaxFrictionTorque = (float) this.BrakingTorque;
        this.TryActivateLimits(allowLock);
        if (!this.m_limitsActive && !this.m_isLockActive)
        {
          constraintData.DisableLimits();
        }
        else
        {
          float lockAngle1 = this.m_minAngle.Value;
          float lockAngle2 = this.m_maxAngle.Value;
          if (this.m_isLockActive)
          {
            lockAngle1 = this.m_lockAngle;
            lockAngle2 = this.m_lockAngle;
          }
          if (!constraintData.MinAngularLimit.IsEqual(lockAngle1) || !constraintData.MaxAngularLimit.IsEqual(lockAngle2))
          {
            constraintData.MinAngularLimit = lockAngle1;
            constraintData.MaxAngularLimit = lockAngle2;
            this.ActivatePhyiscs();
          }
        }
        bool flag1 = this.m_limitsActive;
        if (this.m_limitsActive)
        {
          flag1 = (double) this.m_motor.VelocityTarget == 0.0 || (double) this.m_motor.VelocityTarget <= 0.0 ? (double) f < (double) (float) this.m_minAngle + 9.99999974737875E-05 : (double) f > (double) (float) this.m_maxAngle - 9.99999974737875E-05;
          Action<bool> limitReached = this.LimitReached;
          if (limitReached != null)
          {
            if ((double) f > (double) constraintData.MinAngularLimit && (double) this.m_currentAngle <= (double) constraintData.MinAngularLimit)
              limitReached(false);
            if ((double) f < (double) constraintData.MaxAngularLimit && (double) this.m_currentAngle >= (double) constraintData.MaxAngularLimit)
              limitReached(true);
          }
        }
        bool flag2 = !float.IsInfinity((float) this.m_maxAngle) && (double) (float) this.m_maxAngle < (double) this.m_currentAngle;
        bool flag3 = !float.IsInfinity((float) this.m_minAngle) && (double) this.m_currentAngle < (double) (float) this.m_minAngle;
        if (flag2 | flag3 && flag2 != flag3)
        {
          if (flag2 && (double) this.m_currentAngle < (double) (float) this.m_maxAngle + (double) MyMotorStator.MAX_EXCEEDED_ANGLE_TO_CLAMP_TO_LIMITS)
          {
            this.m_currentAngle = (float) this.m_maxAngle;
            this.SetAngleToPhysics();
          }
          if (flag3 && (double) this.m_currentAngle > (double) (float) this.m_minAngle - (double) MyMotorStator.MAX_EXCEEDED_ANGLE_TO_CLAMP_TO_LIMITS)
          {
            this.m_currentAngle = (float) this.m_minAngle;
            this.SetAngleToPhysics();
          }
        }
        if (this.m_limitsActive || this.m_isLockActive)
        {
          float limit1 = (float) (6.28318548202515 + (float.IsPositiveInfinity((float) this.m_maxAngle) ? 6.28318548202515 : 0.0));
          if ((double) this.m_currentAngle > (double) limit1 + 28.6478900909424)
            this.ScaleDown(limit1);
          float limit2 = (float) (-6.28318548202515 - (float.IsNegativeInfinity((float) this.m_minAngle) ? 6.28318548202515 : 0.0));
          if ((double) this.m_currentAngle < (double) limit2 - 28.6478900909424)
            this.ScaleUp(limit2);
        }
        float num1 = Math.Min((float) this.Torque, (double) this.TopGrid.Physics.Mass > 0.0 ? this.TopGrid.Physics.Mass * this.TopGrid.Physics.Mass : (float) this.Torque);
        this.m_motor.MaxForce = num1;
        this.m_motor.MinForce = -num1;
        if (this.m_limitsActive || (double) (float) this.m_minAngle <= (double) MyMotorStator.MIN_LOWER_LIMIT && (double) (float) this.m_maxAngle >= (double) MyMotorStator.MAX_UPPER_LIMIT)
        {
          this.m_motor.VelocityTarget = (float) this.TargetVelocity;
        }
        else
        {
          double num2 = (double) this.m_currentAngle - (double) (float) this.m_minAngle;
          float num3 = this.m_currentAngle - (float) this.m_maxAngle;
          this.m_motor.VelocityTarget = (double) Math.Abs((float) num2) <= (double) Math.Abs(num3) ? Math.Abs((float) this.TargetVelocity) : -Math.Abs((float) this.TargetVelocity);
        }
        bool isWorking = this.IsWorking;
        if (constraintData.MotorEnabled != isWorking)
        {
          constraintData.SetMotorEnabled(this.m_constraint, isWorking);
          this.ActivatePhyiscs();
        }
        if (!isWorking || this.TopGrid == null || (this.m_motor.VelocityTarget.IsZero() || flag1))
          return;
        this.ActivatePhyiscs();
      }
    }

    private void ActivatePhyiscs()
    {
      this.CubeGrid.Physics.RigidBody.Activate();
      this.TopGrid.Physics.RigidBody.Activate();
    }

    private bool NeedsComputedAngle() => this.IsOpenedInTerminal;

    private float ComputeCurrentAngle()
    {
      MatrixD matrixD1 = this.PositionComp.WorldMatrixRef;
      MatrixD matrixD2 = this.TopBlock.PositionComp.WorldMatrixRef;
      double num1 = Vector3D.Dot(matrixD2.Right, matrixD1.Right);
      double num2 = Vector3D.Dot(matrixD2.Right, matrixD1.Backward);
      float num3 = (float) Math.Acos((double) MathHelper.Clamp((float) num1, -1f, 1f));
      return num2 < 0.0 ? 6.283185f - num3 : num3;
    }

    private void SetConstraintPosition(
      MyAttachableTopBlockBase rotor,
      HkLimitedHingeConstraintData data)
    {
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D up1 = worldMatrix.Up;
      worldMatrix = rotor.WorldMatrix;
      Vector3D up2 = worldMatrix.Up;
      this.m_isRotorFlipped = Vector3D.Dot(up1, up2) < -0.949999988079071;
      Vector3 dummyPosition = this.DummyPosition;
      Vector3 posB = rotor.Position * rotor.CubeGrid.GridSize;
      Vector3 up3 = this.PositionComp.LocalMatrixRef.Up;
      Matrix matrix;
      Vector3 vector3_1;
      if (!this.m_isRotorFlipped)
      {
        vector3_1 = this.PositionComp.LocalMatrixRef.Forward;
      }
      else
      {
        matrix = this.PositionComp.LocalMatrixRef;
        vector3_1 = matrix.Backward;
      }
      Vector3 axisAPerp = vector3_1;
      Vector3 vector3_2;
      if (!this.m_isRotorFlipped)
      {
        matrix = rotor.PositionComp.LocalMatrixRef;
        vector3_2 = matrix.Up;
      }
      else
      {
        matrix = rotor.PositionComp.LocalMatrixRef;
        vector3_2 = matrix.Down;
      }
      Vector3 axisB = vector3_2;
      matrix = rotor.PositionComp.LocalMatrixRef;
      Vector3 forward = matrix.Forward;
      data.SetInBodySpace(dummyPosition, posB, up3, axisB, axisAPerp, forward, (MyPhysicsBody) this.CubeGrid.Physics, (MyPhysicsBody) this.TopGrid.Physics);
    }

    protected override bool Attach(MyAttachableTopBlockBase rotor, bool updateGroup = true)
    {
      if (!(rotor is MyMotorRotor) || !base.Attach(rotor, updateGroup))
        return false;
      this.CheckDisplacementLimits();
      this.CreateConstraint(rotor);
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      return true;
    }

    protected override bool CreateConstraint(MyAttachableTopBlockBase rotor)
    {
      if (!base.CreateConstraint(rotor))
        return false;
      this.m_resetDetailedInfo = true;
      HkRigidBody rigidBody = this.TopBlock.CubeGrid.Physics.RigidBody;
      HkLimitedHingeConstraintData data = new HkLimitedHingeConstraintData();
      this.m_motor = new HkVelocityConstraintMotor(1f, 1000000f);
      data.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
      data.Motor = (HkConstraintMotor) this.m_motor;
      data.DisableLimits();
      this.SetConstraintPosition(rotor, data);
      this.m_constraint = new HkConstraint(this.CubeGrid.Physics.RigidBody, rigidBody, (HkConstraintData) data);
      this.m_constraint.WantRuntime = true;
      this.CubeGrid.Physics.AddConstraint(this.m_constraint);
      if (!this.m_constraint.InWorld)
      {
        this.CubeGrid.Physics.RemoveConstraint(this.m_constraint);
        this.m_constraint.Dispose();
        this.m_constraint = (HkConstraint) null;
        return false;
      }
      this.m_constraint.Enabled = true;
      this.SetAngleToPhysics();
      if (this.CubeGrid.Physics != null)
        this.CubeGrid.Physics.ForceActivate();
      if (this.TopGrid.Physics != null)
        this.TopGrid.Physics.ForceActivate();
      this.m_constraint.OnAddedToWorldCallback += (Action) (() => this.SetAngleToPhysics());
      return true;
    }

    protected override void DisposeConstraint(MyCubeGrid topGrid)
    {
      base.DisposeConstraint(topGrid);
      if (!((HkReferenceObject) this.m_motor != (HkReferenceObject) null))
        return;
      this.m_motor.Dispose();
      this.m_motor = (HkVelocityConstraintMotor) null;
    }

    public void InitializeConveyorEndpoint()
    {
      this.m_conveyorEndpoint = new MyAttachableConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
    }

    public bool CanDebugDraw() => this.TopGrid != null && this.TopGrid.Physics != null;

    public override Vector3? GetConstraintPosition(MyCubeGrid grid, bool opposite = false)
    {
      if ((HkReferenceObject) this.m_constraint == (HkReferenceObject) null)
        return new Vector3?();
      HkLimitedHingeConstraintData constraintData = this.m_constraint.ConstraintData as HkLimitedHingeConstraintData;
      if ((HkReferenceObject) constraintData == (HkReferenceObject) null)
        return new Vector3?();
      if (grid == this.CubeGrid)
        return new Vector3?(opposite ? constraintData.BodyBPos : constraintData.BodyAPos);
      return grid == this.TopGrid ? new Vector3?(opposite ? constraintData.BodyAPos : constraintData.BodyBPos) : new Vector3?();
    }

    protected override bool HasUnsafeSettingsCollector()
    {
      float unsafeTorqueThreshold = this.MotorDefinition.UnsafeTorqueThreshold;
      return (double) (float) this.Torque > (double) unsafeTorqueThreshold || (double) (float) this.BrakingTorque > (double) unsafeTorqueThreshold || base.HasUnsafeSettingsCollector();
    }

    float Sandbox.ModAPI.Ingame.IMyMotorStator.Angle => this.m_currentAngle;

    float Sandbox.ModAPI.Ingame.IMyMotorStator.Torque
    {
      get => (float) this.Torque;
      set
      {
        float maxForceMagnitude = this.MotorDefinition.MaxForceMagnitude;
        this.Torque.Value = MathHelper.Clamp(value, -maxForceMagnitude, maxForceMagnitude);
      }
    }

    float Sandbox.ModAPI.Ingame.IMyMotorStator.BrakingTorque
    {
      get => (float) this.BrakingTorque;
      set
      {
        float maxForceMagnitude = this.MotorDefinition.MaxForceMagnitude;
        this.BrakingTorque.Value = MathHelper.Clamp(value, -maxForceMagnitude, maxForceMagnitude);
      }
    }

    float Sandbox.ModAPI.Ingame.IMyMotorStator.TargetVelocityRad
    {
      get => (float) this.TargetVelocity;
      set
      {
        float rotorAngularVelocity = this.MaxRotorAngularVelocity;
        this.TargetVelocity.Value = MathHelper.Clamp(value, -rotorAngularVelocity, rotorAngularVelocity);
      }
    }

    float Sandbox.ModAPI.Ingame.IMyMotorStator.TargetVelocityRPM
    {
      get => (float) this.TargetVelocity * 9.549296f;
      set
      {
        float rotorAngularVelocity = this.MaxRotorAngularVelocity;
        this.TargetVelocity.Value = MathHelper.Clamp(value * ((float) Math.PI / 30f), -rotorAngularVelocity, rotorAngularVelocity);
      }
    }

    float Sandbox.ModAPI.Ingame.IMyMotorStator.LowerLimitRad
    {
      get => (double) (float) this.m_minAngle <= -6.3006386756897 ? float.MinValue : (float) this.m_minAngle;
      set => this.SetSafeAngles(false, value, (float) this.m_maxAngle);
    }

    float Sandbox.ModAPI.Ingame.IMyMotorStator.LowerLimitDeg
    {
      get => (double) (float) this.m_minAngle <= -6.3006386756897 ? float.MinValue : MathHelper.ToDegrees((float) this.m_minAngle);
      set => this.SetSafeAngles(false, MathHelper.ToRadians(value), (float) this.m_maxAngle);
    }

    float Sandbox.ModAPI.Ingame.IMyMotorStator.UpperLimitRad
    {
      get => (double) (float) this.m_maxAngle >= 6.3006386756897 ? float.MaxValue : (float) this.m_maxAngle;
      set => this.SetSafeAngles(true, (float) this.m_minAngle, value);
    }

    float Sandbox.ModAPI.Ingame.IMyMotorStator.UpperLimitDeg
    {
      get => (double) (float) this.m_maxAngle >= 6.3006386756897 ? float.MaxValue : MathHelper.ToDegrees((float) this.m_maxAngle);
      set => this.SetSafeAngles(true, (float) this.m_minAngle, MathHelper.ToRadians(value));
    }

    float Sandbox.ModAPI.Ingame.IMyMotorStator.Displacement
    {
      get => (float) this.m_dummyDisplacement;
      set
      {
        MyMotorStatorDefinition motorDefinition = this.MotorDefinition;
        this.m_dummyDisplacement.Value = MyMath.Clamp(value, motorDefinition.RotorDisplacementMin, motorDefinition.RotorDisplacementMax);
      }
    }

    bool Sandbox.ModAPI.Ingame.IMyMotorStator.RotorLock
    {
      get => this.IsLocked;
      set => this.IsLocked = value;
    }

    event Action<bool> Sandbox.ModAPI.IMyMotorStator.LimitReached
    {
      add => this.LimitReached += value;
      remove => this.LimitReached -= value;
    }

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    protected class Torque\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorStator) obj0).Torque = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class BrakingTorque\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorStator) obj0).BrakingTorque = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class TargetVelocity\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorStator) obj0).TargetVelocity = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_minAngle\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorStator) obj0).m_minAngle = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_maxAngle\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorStator) obj0).m_maxAngle = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_rotorLock\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorStator) obj0).m_rotorLock = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Cube_MyMotorStator\u003C\u003EActor : IActivator, IActivator<MyMotorStator>
    {
      object IActivator.CreateInstance() => (object) new MyMotorStator();

      MyMotorStator IActivator<MyMotorStator>.CreateInstance() => new MyMotorStator();
    }
  }
}
