// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyMotorSuspension
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_MotorSuspension))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyMotorSuspension), typeof (Sandbox.ModAPI.Ingame.IMyMotorSuspension)})]
  public class MyMotorSuspension : MyMotorBase, Sandbox.ModAPI.IMyMotorSuspension, Sandbox.ModAPI.Ingame.IMyMotorSuspension, Sandbox.ModAPI.Ingame.IMyMotorBase, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyMotorBase, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
  {
    private const float MaxSpeedLimit = 360f;
    private const float LOCK_SPEED_SQ = 1f;
    private float m_steerAngle;
    private bool m_wasBreaking;
    private MyMotorSuspension.MyWheelInversions m_wheelInversions;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_brake;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_strenth;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_height;
    private readonly VRage.Sync.Sync<bool, SyncDirection.FromServer> m_breakingConstraint;
    private static readonly List<HkBodyCollision> m_tmpList = new List<HkBodyCollision>();
    private bool m_wasAccelerating;
    private bool m_updateBrakeNeeded;
    private bool m_updateFrictionNeeded;
    private bool m_updateDampingNeeded;
    private bool m_updateStrengthNeeded;
    private bool m_steeringChanged;
    private Vector3? m_constraintPositionA;
    private Vector3? m_constraintPositionB;
    private HkCustomWheelConstraintData m_wheelConstraintData;
    private bool m_defaultInternalFriction = true;
    private bool m_internalFrictionEnabled = true;
    private bool m_needsUpdate;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_speedLimit;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_friction;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_maxSteerAngle;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_invertSteer;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_invertPropulsion;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_power;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_steering;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_propulsion;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_airShockEnabled;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_brakingEnabled;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_propulsionOverride;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_steeringOverride;
    private HkWheelResponseModifierUtil m_modifier;
    private float m_angleBeforeRemove;
    private bool m_CoMVectorsCacheValid;
    private Vector3 m_adjustmentVectorCache;
    private Vector3 m_realCoMCache;
    private Vector3 m_suspensionPositionCache;

    public bool NeedsPerFrameUpdate
    {
      get => this.m_needsUpdate;
      set
      {
        if (this.m_needsUpdate == value)
          return;
        this.m_needsUpdate = value;
        this.CubeGrid.GridSystems.WheelSystem.OnBlockNeedsUpdateChanged(this);
      }
    }

    private bool InternalFrictionEnabled
    {
      get => this.m_defaultInternalFriction && this.m_internalFrictionEnabled;
      set
      {
        if (!this.m_defaultInternalFriction || this.m_internalFrictionEnabled == value)
          return;
        this.m_internalFrictionEnabled = value;
        this.ResetConstraintFriction();
      }
    }

    public float SpeedLimit
    {
      get => (float) this.m_speedLimit;
      set => this.m_speedLimit.Value = value;
    }

    internal float Strength
    {
      get => (float) Math.Sqrt((double) (float) this.m_strenth);
      set
      {
        if (float.IsNaN(value))
        {
          this.m_strenth.Value = 0.0f;
        }
        else
        {
          if ((double) (float) this.m_strenth == (double) value * (double) value)
            return;
          this.m_strenth.Value = value * value;
        }
      }
    }

    private HkRigidBody SafeBody => this.TopGrid == null || this.TopGrid.Physics == null ? (HkRigidBody) null : this.TopGrid.Physics.RigidBody;

    public bool Brake
    {
      get => (bool) this.m_brake;
      set => this.m_brake.Value = value;
    }

    public float Friction
    {
      get => (float) this.m_friction;
      set
      {
        if (float.IsNaN(value))
        {
          this.m_friction.Value = 0.0f;
        }
        else
        {
          if ((double) this.m_friction.Value == (double) value)
            return;
          this.m_friction.Value = value;
        }
      }
    }

    private void PropagateFriction()
    {
      this.m_updateFrictionNeeded = false;
      if (this.TopBlock is MyWheel topBlock)
      {
        double num = 35.0 * ((double) MyMath.FastTanH((float) (6.0 * (double) (float) this.m_friction - 3.0)) / 2.0 + 0.5);
        if (this.ShouldBrake)
          num *= 2.0;
        topBlock.Friction = (float) num;
        topBlock.CubeGrid.Physics.RigidBody.Friction = topBlock.Friction;
      }
      else
      {
        this.m_updateFrictionNeeded = true;
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      }
    }

    public float Height
    {
      get => (float) this.m_height;
      set
      {
        if ((double) (float) this.m_height == (double) value)
          return;
        this.m_height.Value = value;
      }
    }

    public float MaxSteerAngle
    {
      get => (float) this.m_maxSteerAngle;
      set
      {
        this.m_steeringChanged = true;
        this.m_maxSteerAngle.Value = value;
        this.OnPerFrameUpdatePropertyChanged();
      }
    }

    public bool InvertSteer
    {
      get => (bool) this.m_invertSteer;
      set => this.m_invertSteer.Value = value;
    }

    public bool InvertPropulsion
    {
      get => (bool) this.m_invertPropulsion;
      set => this.m_invertPropulsion.Value = value;
    }

    public float SteerAngle
    {
      get => this.m_steerAngle;
      set => this.m_steerAngle = value;
    }

    public float Power
    {
      get => (float) this.m_power;
      set
      {
        if (float.IsNaN(value))
          this.m_power.Value = 0.0f;
        else
          this.m_power.Value = value;
      }
    }

    public bool Steering
    {
      get => (bool) this.m_steering;
      set => this.m_steering.Value = value;
    }

    public bool Propulsion
    {
      get => (bool) this.m_propulsion;
      set => this.m_propulsion.Value = value;
    }

    public bool AirShockEnabled
    {
      get => (bool) this.m_airShockEnabled;
      set => this.m_airShockEnabled.Value = value;
    }

    public bool BrakingEnabled
    {
      get => (bool) this.m_brakingEnabled;
      set => this.m_brakingEnabled.Value = value;
    }

    public float PropulsionOverride
    {
      get => (float) this.m_propulsionOverride;
      set => this.m_propulsionOverride.Value = value;
    }

    public float SteeringOverride
    {
      get => (float) this.m_steeringOverride;
      set
      {
        this.m_steeringChanged = true;
        if (float.IsNaN(value))
          this.m_steeringOverride.Value = 0.0f;
        else
          this.m_steeringOverride.Value = value;
      }
    }

    protected override bool AllowShareInertiaTensor() => false;

    private bool ShouldBrake => this.Brake && this.BrakingEnabled;

    public float CurrentAirShock { get; private set; }

    public MyMotorSuspensionDefinition BlockDefinition => (MyMotorSuspensionDefinition) ((MyCubeBlock) this).BlockDefinition;

    public new float MaxRotorAngularVelocity => 37.69911f;

    public MyMotorSuspension()
    {
      this.CreateTerminalControls();
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.OnIsWorkingChanged);
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyMotorSuspension>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlButton<MyMotorSuspension> button = new MyTerminalControlButton<MyMotorSuspension>("Add Top Part", MySpaceTexts.BlockActionTitle_AddWheel, MySpaceTexts.BlockActionTooltip_AddWheel, (Action<MyMotorSuspension>) (b => b.RecreateTop()));
      button.Enabled = (Func<MyMotorSuspension, bool>) (b => b.TopBlock == null);
      button.EnableAction<MyMotorSuspension>(MyTerminalActionIcons.STATION_ON);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) button);
      MyTerminalControlCheckbox<MyMotorSuspension> checkbox1 = new MyTerminalControlCheckbox<MyMotorSuspension>("Steering", MySpaceTexts.BlockPropertyTitle_Motor_Steering, MySpaceTexts.BlockPropertyDescription_Motor_Steering);
      checkbox1.Getter = (MyTerminalValueControl<MyMotorSuspension, bool>.GetterDelegate) (x => x.Steering);
      checkbox1.Setter = (MyTerminalValueControl<MyMotorSuspension, bool>.SetterDelegate) ((x, v) => x.Steering = v);
      checkbox1.EnableAction<MyMotorSuspension>();
      checkbox1.Enabled = (Func<MyMotorSuspension, bool>) (x => (HkReferenceObject) x.m_constraint != (HkReferenceObject) null);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) checkbox1);
      MyTerminalControlCheckbox<MyMotorSuspension> checkbox2 = new MyTerminalControlCheckbox<MyMotorSuspension>("Propulsion", MySpaceTexts.BlockPropertyTitle_Motor_Propulsion, MySpaceTexts.BlockPropertyDescription_Motor_Propulsion);
      checkbox2.Getter = (MyTerminalValueControl<MyMotorSuspension, bool>.GetterDelegate) (x => x.Propulsion);
      checkbox2.Setter = (MyTerminalValueControl<MyMotorSuspension, bool>.SetterDelegate) ((x, v) => x.Propulsion = v);
      checkbox2.EnableAction<MyMotorSuspension>();
      checkbox2.Enabled = (Func<MyMotorSuspension, bool>) (x => (HkReferenceObject) x.m_constraint != (HkReferenceObject) null);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) checkbox2);
      MyTerminalControlCheckbox<MyMotorSuspension> checkbox3 = new MyTerminalControlCheckbox<MyMotorSuspension>("Braking", MySpaceTexts.BlockPropertyTitle_Suspension_Brake, MySpaceTexts.BlockPropertyDescription_Suspension_Brake);
      checkbox3.Getter = (MyTerminalValueControl<MyMotorSuspension, bool>.GetterDelegate) (x => x.BrakingEnabled);
      checkbox3.Setter = (MyTerminalValueControl<MyMotorSuspension, bool>.SetterDelegate) ((x, v) => x.BrakingEnabled = v);
      checkbox3.EnableAction<MyMotorSuspension>();
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) checkbox3);
      MyTerminalControlCheckbox<MyMotorSuspension> checkbox4 = new MyTerminalControlCheckbox<MyMotorSuspension>("AirShock", MySpaceTexts.BlockPropertyTitle_Suspension_AirShock, MySpaceTexts.BlockPropertyDescription_Suspension_AirShock);
      checkbox4.Getter = (MyTerminalValueControl<MyMotorSuspension, bool>.GetterDelegate) (x => x.AirShockEnabled);
      checkbox4.Setter = (MyTerminalValueControl<MyMotorSuspension, bool>.SetterDelegate) ((x, v) => x.AirShockEnabled = v);
      checkbox4.EnableAction<MyMotorSuspension>();
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) checkbox4);
      MyTerminalControlCheckbox<MyMotorSuspension> checkbox5 = new MyTerminalControlCheckbox<MyMotorSuspension>("InvertSteering", MySpaceTexts.BlockPropertyTitle_Motor_InvertSteer, MySpaceTexts.BlockPropertyDescription_Motor_InvertSteer);
      checkbox5.Getter = (MyTerminalValueControl<MyMotorSuspension, bool>.GetterDelegate) (x => x.InvertSteer);
      checkbox5.Setter = (MyTerminalValueControl<MyMotorSuspension, bool>.SetterDelegate) ((x, v) => x.InvertSteer = v);
      checkbox5.EnableAction<MyMotorSuspension>();
      checkbox5.Enabled = (Func<MyMotorSuspension, bool>) (x => (HkReferenceObject) x.m_constraint != (HkReferenceObject) null);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) checkbox5);
      MyTerminalControlCheckbox<MyMotorSuspension> checkbox6 = new MyTerminalControlCheckbox<MyMotorSuspension>("InvertPropulsion", MySpaceTexts.BlockPropertyTitle_Motor_InvertPropulsion, MySpaceTexts.BlockPropertyDescription_Motor_InvertPropulsion);
      checkbox6.Getter = (MyTerminalValueControl<MyMotorSuspension, bool>.GetterDelegate) (x => x.InvertPropulsion);
      checkbox6.Setter = (MyTerminalValueControl<MyMotorSuspension, bool>.SetterDelegate) ((x, v) => x.InvertPropulsion = v);
      checkbox6.EnableAction<MyMotorSuspension>();
      checkbox6.Enabled = (Func<MyMotorSuspension, bool>) (x => (HkReferenceObject) x.m_constraint != (HkReferenceObject) null);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) checkbox6);
      MyTerminalControlSlider<MyMotorSuspension> slider1 = new MyTerminalControlSlider<MyMotorSuspension>("MaxSteerAngle", MySpaceTexts.BlockPropertyTitle_Motor_MaxSteerAngle, MySpaceTexts.BlockPropertyDescription_Motor_MaxSteerAngle);
      slider1.SetLimits((MyTerminalValueControl<MyMotorSuspension, float>.GetterDelegate) (x => 0.0f), (MyTerminalValueControl<MyMotorSuspension, float>.GetterDelegate) (x => MathHelper.ToDegrees(x.BlockDefinition.MaxSteer)));
      slider1.DefaultValue = new float?(20f);
      slider1.Getter = (MyTerminalValueControl<MyMotorSuspension, float>.GetterDelegate) (x => MathHelper.ToDegrees(x.MaxSteerAngle));
      slider1.Setter = (MyTerminalValueControl<MyMotorSuspension, float>.SetterDelegate) ((x, v) => x.MaxSteerAngle = MathHelper.ToRadians(v));
      slider1.Writer = (MyTerminalControl<MyMotorSuspension>.WriterDelegate) ((x, res) => MyMotorStator.WriteAngle(x.MaxSteerAngle, res));
      slider1.EnableActionsWithReset<MyMotorSuspension>();
      slider1.Enabled = (Func<MyMotorSuspension, bool>) (x => (HkReferenceObject) x.m_constraint != (HkReferenceObject) null && x.Steering);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) slider1);
      MyTerminalControlSlider<MyMotorSuspension> slider2 = new MyTerminalControlSlider<MyMotorSuspension>("Power", MySpaceTexts.BlockPropertyTitle_Motor_Power, MySpaceTexts.BlockPropertyDescription_Motor_Power);
      slider2.SetLimits(0.0f, 100f);
      slider2.DefaultValue = new float?(60f);
      slider2.Getter = (MyTerminalValueControl<MyMotorSuspension, float>.GetterDelegate) (x => x.GetPowerForTerminal());
      slider2.Setter = (MyTerminalValueControl<MyMotorSuspension, float>.SetterDelegate) ((x, v) => x.Power = v / 100f);
      slider2.Writer = (MyTerminalControl<MyMotorSuspension>.WriterDelegate) ((x, res) => res.AppendInt32((int) ((double) x.Power * 100.0)).Append("%"));
      slider2.EnableActions<MyMotorSuspension>();
      slider2.Enabled = (Func<MyMotorSuspension, bool>) (x => (HkReferenceObject) x.m_constraint != (HkReferenceObject) null);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) slider2);
      MyTerminalControlSlider<MyMotorSuspension> slider3 = new MyTerminalControlSlider<MyMotorSuspension>("Strength", MySpaceTexts.BlockPropertyTitle_Motor_Strength, MySpaceTexts.BlockPropertyTitle_Motor_Strength);
      slider3.SetLimits(0.0f, 100f);
      slider3.DefaultValue = new float?(10f);
      slider3.Getter = (MyTerminalValueControl<MyMotorSuspension, float>.GetterDelegate) (x => x.GetStrengthForTerminal());
      slider3.Setter = (MyTerminalValueControl<MyMotorSuspension, float>.SetterDelegate) ((x, v) => x.Strength = v / 100f);
      slider3.Writer = (MyTerminalControl<MyMotorSuspension>.WriterDelegate) ((x, res) => res.AppendInt32((int) x.GetStrengthForTerminal()).Append("%"));
      slider3.EnableActions<MyMotorSuspension>();
      slider3.Enabled = (Func<MyMotorSuspension, bool>) (x => (HkReferenceObject) x.m_constraint != (HkReferenceObject) null);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) slider3);
      MyTerminalControlSlider<MyMotorSuspension> slider4 = new MyTerminalControlSlider<MyMotorSuspension>("Height", MySpaceTexts.BlockPropertyTitle_Motor_Height, MySpaceTexts.BlockPropertyDescription_Motor_Height);
      slider4.SetLimits((MyTerminalValueControl<MyMotorSuspension, float>.GetterDelegate) (x => x.BlockDefinition.MinHeight), (MyTerminalValueControl<MyMotorSuspension, float>.GetterDelegate) (x => x.BlockDefinition.MaxHeight));
      slider4.DefaultValue = new float?(0.0f);
      slider4.Getter = (MyTerminalValueControl<MyMotorSuspension, float>.GetterDelegate) (x => x.GetHeightForTerminal());
      slider4.Setter = (MyTerminalValueControl<MyMotorSuspension, float>.SetterDelegate) ((x, v) => x.Height = v);
      slider4.Writer = (MyTerminalControl<MyMotorSuspension>.WriterDelegate) ((x, res) => MyValueFormatter.AppendDistanceInBestUnit(x.Height, res));
      slider4.EnableActionsWithReset<MyMotorSuspension>();
      slider4.Enabled = (Func<MyMotorSuspension, bool>) (x => (HkReferenceObject) x.m_constraint != (HkReferenceObject) null);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) slider4);
      MyTerminalControlSlider<MyMotorSuspension> slider5 = new MyTerminalControlSlider<MyMotorSuspension>("Friction", MySpaceTexts.BlockPropertyTitle_Motor_Friction, MySpaceTexts.BlockPropertyDescription_Motor_Friction);
      slider5.SetLimits(0.0f, 100f);
      slider5.DefaultValue = new float?(50f);
      slider5.Getter = (MyTerminalValueControl<MyMotorSuspension, float>.GetterDelegate) (x => x.Friction * 100f);
      slider5.Setter = (MyTerminalValueControl<MyMotorSuspension, float>.SetterDelegate) ((x, v) => x.Friction = v / 100f);
      slider5.Writer = (MyTerminalControl<MyMotorSuspension>.WriterDelegate) ((x, res) => res.AppendDecimal(x.Friction * 100f, 2).Append("%"));
      slider5.EnableActions<MyMotorSuspension>();
      slider5.Enabled = (Func<MyMotorSuspension, bool>) (x => (HkReferenceObject) x.m_constraint != (HkReferenceObject) null);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) slider5);
      MyTerminalControlSlider<MyMotorSuspension> slider6 = new MyTerminalControlSlider<MyMotorSuspension>("Speed Limit", MySpaceTexts.BlockPropertyTitle_Motor_SuspensionSpeed, MySpaceTexts.BlockPropertyDescription_Motor_SuspensionSpeed);
      slider6.SetLimits(0.0f, 360f);
      slider6.DefaultValue = new float?(180f);
      slider6.Getter = (MyTerminalValueControl<MyMotorSuspension, float>.GetterDelegate) (x => x.SpeedLimit);
      slider6.Setter = (MyTerminalValueControl<MyMotorSuspension, float>.SetterDelegate) ((x, v) => x.SpeedLimit = v);
      slider6.Writer = (MyTerminalControl<MyMotorSuspension>.WriterDelegate) ((x, res) =>
      {
        if ((double) x.SpeedLimit >= 360.0)
          res.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_MotorAngleUnlimited));
        else
          res.AppendInt32((int) x.SpeedLimit).Append("km/h");
      });
      slider6.EnableActionsWithReset<MyMotorSuspension>();
      slider6.Enabled = (Func<MyMotorSuspension, bool>) (x => (HkReferenceObject) x.m_constraint != (HkReferenceObject) null);
      slider6.Visible = (Func<MyMotorSuspension, bool>) (x => MySession.Static.Settings.AdjustableMaxVehicleSpeed);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) slider6);
      MyTerminalControlSlider<MyMotorSuspension> slider7 = new MyTerminalControlSlider<MyMotorSuspension>("Propulsion override", MySpaceTexts.BlockPropertyTitle_Motor_PropulsionOverride, MySpaceTexts.BlockPropertyDescription_Motor_PropulsionOverride, true, true);
      slider7.SetLimits(-1f, 1f);
      slider7.DefaultValue = new float?(0.0f);
      slider7.Getter = (MyTerminalValueControl<MyMotorSuspension, float>.GetterDelegate) (x => x.PropulsionOverride);
      slider7.Setter = (MyTerminalValueControl<MyMotorSuspension, float>.SetterDelegate) ((x, v) => x.PropulsionOverride = v);
      slider7.Writer = (MyTerminalControl<MyMotorSuspension>.WriterDelegate) ((x, res) => res.AppendDecimal(x.PropulsionOverride * 100f, 2).Append("%"));
      slider7.EnableActionsWithReset<MyMotorSuspension>();
      slider7.Enabled = (Func<MyMotorSuspension, bool>) (x => (HkReferenceObject) x.m_constraint != (HkReferenceObject) null);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) slider7);
      MyTerminalControlSlider<MyMotorSuspension> slider8 = new MyTerminalControlSlider<MyMotorSuspension>("Steer override", MySpaceTexts.BlockPropertyTitle_Motor_SteerOverride, MySpaceTexts.BlockPropertyDescription_Motor_SteerOverride, true, true);
      slider8.SetLimits(-1f, 1f);
      slider8.DefaultValue = new float?(0.0f);
      slider8.Getter = (MyTerminalValueControl<MyMotorSuspension, float>.GetterDelegate) (x => x.SteeringOverride);
      slider8.Setter = (MyTerminalValueControl<MyMotorSuspension, float>.SetterDelegate) ((x, v) => x.SteeringOverride = v);
      slider8.Writer = (MyTerminalControl<MyMotorSuspension>.WriterDelegate) ((x, res) => res.AppendDecimal(x.SteeringOverride * 100f, 2).Append("%"));
      slider8.EnableActionsWithReset<MyMotorSuspension>();
      slider8.Enabled = (Func<MyMotorSuspension, bool>) (x => (HkReferenceObject) x.m_constraint != (HkReferenceObject) null);
      MyTerminalControlFactory.AddControl<MyMotorSuspension>((MyTerminalControl<MyMotorSuspension>) slider8);
    }

    private void FrictionChanged() => this.PropagateFriction();

    private void DampingChanged()
    {
      this.m_updateDampingNeeded = false;
      if ((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null && this.TopBlock != null)
      {
        this.UpdateConstraintData();
      }
      else
      {
        this.m_updateDampingNeeded = true;
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      }
    }

    private void StrengthChanged()
    {
      this.m_updateStrengthNeeded = false;
      if ((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null && this.TopBlock != null)
      {
        this.UpdateConstraintData();
      }
      else
      {
        this.m_updateStrengthNeeded = true;
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      }
    }

    private void HeightChanged()
    {
      if (!((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null) || this.TopBlock == null)
        return;
      this.UpdateConstraintData();
    }

    private void BreakingConstraintChanged()
    {
      if ((HkReferenceObject) this.m_wheelConstraintData == (HkReferenceObject) null)
        return;
      if ((bool) this.m_breakingConstraint)
      {
        float currentAngle = HkCustomWheelConstraintData.GetCurrentAngle(this.m_constraint);
        MyWheel.WheelExplosionLog(this.CubeGrid, (MyTerminalBlock) this, "OnBrakeAngleLimit: " + (object) currentAngle);
        this.m_wheelConstraintData.SetAngleLimits(currentAngle, currentAngle);
        this.ActivatePhysics();
      }
      else
      {
        MyWheel.WheelExplosionLog(this.CubeGrid, (MyTerminalBlock) this, "OnBrakeAngleLimitReleased");
        this.m_wheelConstraintData.DisableLimits();
      }
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_MotorSuspension builderMotorSuspension = objectBuilder as MyObjectBuilder_MotorSuspension;
      this.m_steerAngle = MathHelper.Clamp(builderMotorSuspension.SteerAngle, -this.BlockDefinition.MaxSteer, this.BlockDefinition.MaxSteer);
      float num1 = MathHelper.Clamp(Math.Min(builderMotorSuspension.Strength, builderMotorSuspension.StrengthNew), 0.0f, 1f);
      this.m_strenth.SetLocalValue(num1 * num1);
      this.m_steering.SetLocalValue(builderMotorSuspension.Steering);
      this.m_propulsion.SetLocalValue(builderMotorSuspension.Propulsion);
      this.m_power.SetLocalValue(MathHelper.Clamp(Math.Max(builderMotorSuspension.Power, builderMotorSuspension.PowerNew), 0.0f, 1f));
      this.m_height.SetLocalValue(MathHelper.Clamp(builderMotorSuspension.Height, this.BlockDefinition.MinHeight, this.BlockDefinition.MaxHeight));
      this.m_maxSteerAngle.SetLocalValue(MathHelper.Clamp(builderMotorSuspension.MaxSteerAngle, 0.0f, this.BlockDefinition.MaxSteer));
      this.m_invertSteer.SetLocalValue(builderMotorSuspension.InvertSteer);
      this.m_invertPropulsion.SetLocalValue(builderMotorSuspension.InvertPropulsion);
      this.m_speedLimit.SetLocalValue(builderMotorSuspension.SpeedLimit);
      VRage.Sync.Sync<float, SyncDirection.BothWays> friction = this.m_friction;
      float? frictionNew = builderMotorSuspension.FrictionNew;
      double num2 = (double) MathHelper.Clamp(frictionNew.HasValue ? frictionNew.GetValueOrDefault() : ((double) builderMotorSuspension.Friction / 4.0 < 0.2 ? 0.05f : 0.5f), 0.0f, 1f);
      friction.SetLocalValue((float) num2);
      this.m_airShockEnabled.SetLocalValue(builderMotorSuspension.AirShockEnabled);
      this.m_brakingEnabled.SetLocalValue(builderMotorSuspension.BrakingEnabled);
      this.m_steeringOverride.SetLocalValue(MathHelper.Clamp(builderMotorSuspension.SteeringOverride, -1f, 1f));
      this.m_propulsionOverride.SetLocalValue(MathHelper.Clamp(builderMotorSuspension.PropulsionOverride, -1f, 1f));
      this.CubeGrid.OnPhysicsChanged += new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      this.CubeGrid.OnHavokSystemIDChanged += new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      this.CubeGrid.OnMassPropertiesChanged += new Action<MyCubeGrid>(this.CubeGrid_OnMassPropertiesChanged);
      this.m_brake.ValueChanged += (Action<SyncBase>) (x => this.UpdateBrake());
      this.m_brakingEnabled.ValueChanged += (Action<SyncBase>) (x => this.UpdateBrake());
      this.m_friction.ValueChanged += (Action<SyncBase>) (x => this.FrictionChanged());
      this.m_strenth.ValueChanged += (Action<SyncBase>) (x => this.StrengthChanged());
      this.m_height.ValueChanged += (Action<SyncBase>) (x => this.HeightChanged());
      this.m_breakingConstraint.ValueChanged += (Action<SyncBase>) (x => this.BreakingConstraintChanged());
      this.m_steeringOverride.ValueChanged += (Action<SyncBase>) (x => this.SteeringOverrideChanged());
      this.m_propulsionOverride.ValueChanged += (Action<SyncBase>) (x => this.PropulsionOverrideChanged());
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentMotorSuspension(this));
    }

    private void OnLostUnbreakableState()
    {
      if (this.MarkedForClose || this.Closed || (this.TopGrid == null || this.TopGrid.MarkedForClose) || this.TopGrid.Closed)
        return;
      this.OnRotorPhysicsChanged((MyEntity) this.TopGrid);
    }

    private void PropulsionOverrideChanged() => this.OnPerFrameUpdatePropertyChanged();

    private void SteeringOverrideChanged()
    {
      this.m_steeringChanged = true;
      this.OnPerFrameUpdatePropertyChanged();
    }

    public override void OnRemovedFromScene(object source)
    {
      MyWheel.DumpActivityLog();
      base.OnRemovedFromScene(source);
      this.CubeGrid.OnMassPropertiesChanged -= new Action<MyCubeGrid>(this.CubeGrid_OnMassPropertiesChanged);
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      MyWheel.WheelExplosionLog(oldGrid, (MyTerminalBlock) this, "OnSuspensionMoved " + this.CubeGrid.DisplayName);
      base.OnCubeGridChanged(oldGrid);
      oldGrid.OnMassPropertiesChanged -= new Action<MyCubeGrid>(this.CubeGrid_OnMassPropertiesChanged);
      this.CubeGrid.OnMassPropertiesChanged += new Action<MyCubeGrid>(this.CubeGrid_OnMassPropertiesChanged);
    }

    private void CubeGrid_OnMassPropertiesChanged(MyCubeGrid obj)
    {
      if ((HkReferenceObject) this.SafeConstraint == (HkReferenceObject) null)
        return;
      this.m_CoMVectorsCacheValid = false;
    }

    private void CubeGrid_OnHavokSystemIDChanged(int obj) => this.CubeGrid_OnPhysicsChanged((MyEntity) this.CubeGrid);

    private void CubeGrid_OnPhysicsChanged(MyEntity obj)
    {
      if (this.CubeGrid.Physics == null || this.TopGrid == null || this.TopGrid.Physics == null)
        return;
      HkRigidBody rigidBody = this.TopGrid.Physics.RigidBody;
      if ((HkReferenceObject) rigidBody == (HkReferenceObject) null)
        return;
      uint info = HkGroupFilter.CalcFilterInfo(rigidBody.Layer, this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, 1, 1);
      rigidBody.SetCollisionFilterInfo(info);
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.UpdateBrake();
    }

    public override void UpdateBeforeSimulation()
    {
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
      base.UpdateBeforeSimulation();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_MotorSuspension builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_MotorSuspension;
      builderCubeBlock.FrictionNew = new float?(this.Friction);
      builderCubeBlock.SteerAngle = this.m_steerAngle;
      builderCubeBlock.Steering = this.Steering;
      builderCubeBlock.Strength = float.MaxValue;
      builderCubeBlock.StrengthNew = this.Strength;
      builderCubeBlock.Propulsion = this.Propulsion;
      builderCubeBlock.Height = this.Height;
      builderCubeBlock.MaxSteerAngle = this.MaxSteerAngle;
      builderCubeBlock.InvertSteer = this.InvertSteer;
      builderCubeBlock.InvertPropulsion = this.InvertPropulsion;
      builderCubeBlock.SpeedLimit = this.SpeedLimit;
      builderCubeBlock.PowerNew = this.Power;
      builderCubeBlock.Power = 0.0f;
      builderCubeBlock.AirShockEnabled = this.AirShockEnabled;
      builderCubeBlock.BrakingEnabled = this.BrakingEnabled;
      builderCubeBlock.SteeringOverride = this.SteeringOverride;
      builderCubeBlock.PropulsionOverride = this.PropulsionOverride;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override bool CheckIsWorking() => ((base.CheckIsWorking() ? 1 : 0) & (this.TopBlock == null ? 0 : (this.TopBlock.IsWorking ? 1 : 0))) != 0;

    protected override float ComputeRequiredPowerInput()
    {
      if (this.TopBlock == null)
        return 0.0f;
      float num1 = base.ComputeRequiredPowerInput();
      if ((double) num1 > 0.0)
      {
        float requiredIdlePowerInput = this.BlockDefinition.RequiredIdlePowerInput;
        float amount = this.Power * ((double) this.PropulsionOverride > 0.0 ? this.PropulsionOverride : 1f);
        float max = MathHelper.Lerp(requiredIdlePowerInput, num1, amount);
        float num2 = (float) (((double) max - (double) requiredIdlePowerInput) / (this.m_wasAccelerating ? 15.0 : -50.0));
        num1 = MathHelper.Clamp(this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) + num2, requiredIdlePowerInput, max);
        this.OnPerFrameUpdatePropertyChanged();
      }
      return num1;
    }

    public void UpdateBrake()
    {
      this.m_updateBrakeNeeded = false;
      if ((HkReferenceObject) this.SafeBody != (HkReferenceObject) null)
      {
        this.PropagateFriction();
        if (this.ShouldBrake)
        {
          this.SafeBody.AngularDamping = this.BlockDefinition.PropulsionForce;
        }
        else
        {
          this.SafeBody.AngularDamping = this.CubeGrid.Physics.AngularDamping;
          if (!Sandbox.Game.Multiplayer.Sync.IsServer || !((HkReferenceObject) this.m_constraint != (HkReferenceObject) null) || !(bool) this.m_breakingConstraint)
            return;
          this.m_breakingConstraint.Value = false;
        }
      }
      else
      {
        this.m_updateBrakeNeeded = true;
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      }
    }

    public void InitControl(MyEntity controller)
    {
      MatrixD matrixD = controller.WorldMatrix * this.PositionComp.WorldMatrixNormalizedInv;
      Vector3 vector3 = Base6Directions.GetClosestDirection((Vector3) matrixD.Forward) == Base6Directions.Direction.Up || Base6Directions.GetClosestDirection((Vector3) matrixD.Forward) == Base6Directions.Direction.Down ? (Vector3) controller.WorldMatrix.Forward : (Base6Directions.GetClosestDirection((Vector3) matrixD.Up) == Base6Directions.Direction.Up || Base6Directions.GetClosestDirection((Vector3) matrixD.Up) == Base6Directions.Direction.Down ? (Vector3) controller.WorldMatrix.Up : (Vector3) controller.WorldMatrix.Right);
      Vector3 vector2 = Base6Directions.GetClosestDirection((Vector3) matrixD.Forward) == Base6Directions.Direction.Forward || Base6Directions.GetClosestDirection((Vector3) matrixD.Forward) == Base6Directions.Direction.Backward ? (Vector3) controller.WorldMatrix.Forward : (Base6Directions.GetClosestDirection((Vector3) matrixD.Up) == Base6Directions.Direction.Forward || Base6Directions.GetClosestDirection((Vector3) matrixD.Up) == Base6Directions.Direction.Backward ? (Vector3) controller.WorldMatrix.Up : (Vector3) controller.WorldMatrix.Right);
      if (this.CubeGrid.Physics == null)
        return;
      double num1 = (double) Vector3.Dot((Vector3) controller.WorldMatrix.Forward, (Vector3) (this.WorldMatrix.Translation - this.CubeGrid.Physics.CenterOfMassWorld)) - 0.0001;
      float num2 = Vector3.Dot((Vector3) this.WorldMatrix.Forward, vector2);
      this.m_wheelInversions = new MyMotorSuspension.MyWheelInversions()
      {
        SteerInvert = num1 * (double) num2 < 0.0,
        RevolveInvert = (this.WorldMatrix.Up - vector3).Length() > 0.100000001490116
      };
    }

    public void ReleaseControl(MyEntity controller)
    {
    }

    protected override bool Attach(MyAttachableTopBlockBase rotor, bool updateGroup = true)
    {
      if (!(rotor is MyMotorRotor) || !base.Attach(rotor, updateGroup))
        return false;
      MyWheel.WheelExplosionLog(this.CubeGrid, (MyTerminalBlock) this, "OnAttach" + Environment.NewLine + Environment.StackTrace);
      this.CreateConstraint(rotor);
      this.PropagateFriction();
      this.UpdateIsWorking();
      if (this.m_updateBrakeNeeded)
        this.UpdateBrake();
      return true;
    }

    protected override void Detach(bool updateGroups = true)
    {
      base.Detach(updateGroups);
      if (this.m_modifier != null)
      {
        this.m_modifier.Dispose();
        this.m_modifier = (HkWheelResponseModifierUtil) null;
      }
      MyWheel.WheelExplosionLog(this.CubeGrid, (MyTerminalBlock) this, "OnDetach" + Environment.NewLine + Environment.StackTrace);
      MyWheel.DumpActivityLog();
    }

    protected override bool CreateConstraint(MyAttachableTopBlockBase rotor)
    {
      if (!base.CreateConstraint(rotor))
        return false;
      MyCubeGrid topGrid = this.TopGrid;
      HkRigidBody rigidBody = this.TopGrid.Physics.RigidBody;
      if (this.m_modifier != null)
        this.m_modifier.Dispose();
      this.m_modifier = HkWheelResponseModifierUtil.Create(rigidBody, (HkWheelResponseModifierUtil.ReturnFloat) (() => MyPhysicsConfig.WheelSoftnessRatio), (HkWheelResponseModifierUtil.ReturnFloat) (() => MyPhysicsConfig.WheelSoftnessVelocity));
      rigidBody.MaxAngularVelocity = float.MaxValue;
      rigidBody.Restitution = 0.5f;
      topGrid.OnPhysicsChanged += new Action<MyEntity>(this.OnRotorPhysicsChanged);
      this.OnRotorPhysicsChanged((MyEntity) topGrid);
      if (MyFakes.WHEEL_SOFTNESS)
        HkUtils.SetSoftContact(rigidBody, (HkRigidBody) null, MyPhysicsConfig.WheelSoftnessRatio, MyPhysicsConfig.WheelSoftnessVelocity);
      uint info = HkGroupFilter.CalcFilterInfo(rigidBody.Layer, this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, 1, 1);
      rigidBody.SetCollisionFilterInfo(info);
      MyPhysics.RefreshCollisionFilter(this.CubeGrid.GetPhysicsBody());
      this.m_wheelConstraintData = new HkCustomWheelConstraintData();
      Vector3 posA;
      Vector3 posB;
      this.FillConstraintData(rotor, out posA, out posB);
      this.m_constraint = new HkConstraint(rigidBody, this.CubeGrid.Physics.RigidBody, (HkConstraintData) this.m_wheelConstraintData);
      this.m_constraint.WantRuntime = true;
      this.CubeGrid.Physics.AddConstraint(this.m_constraint);
      if (!this.m_constraint.InWorld)
      {
        this.CubeGrid.Physics.RemoveConstraint(this.m_constraint);
        this.m_constraint = (HkConstraint) null;
        return false;
      }
      this.m_constraint.Enabled = true;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_breakingConstraint.Value = false;
      this.BreakingConstraintChanged();
      this.ActivatePhysics();
      this.m_constraintPositionA = !(posA == Vector3.Zero) ? new Vector3?(posA) : new Vector3?();
      this.m_constraintPositionB = !(posB == Vector3.Zero) ? new Vector3?(posB) : new Vector3?();
      this.m_constraint.OnRemovedFromWorldCallback += (Action) (() => this.m_angleBeforeRemove = HkCustomWheelConstraintData.GetCurrentAngle(this.m_constraint));
      this.m_constraint.OnAddedToWorldCallback += (Action) (() =>
      {
        HkCustomWheelConstraintData.SetCurrentAngle(this.m_constraint, this.m_angleBeforeRemove);
        this.OnLostUnbreakableState();
      });
      return true;
    }

    protected override void DisposeConstraint(MyCubeGrid topGrid)
    {
      if (topGrid == null)
        return;
      topGrid.OnPhysicsChanged -= new Action<MyEntity>(this.OnRotorPhysicsChanged);
      base.DisposeConstraint(topGrid);
      this.m_wheelConstraintData = (HkCustomWheelConstraintData) null;
    }

    protected override MatrixD GetTopGridMatrix()
    {
      Vector3D position = Vector3D.Transform(this.DummyPosition + this.PositionComp.LocalMatrixRef.Forward * (float) this.m_height, this.CubeGrid.WorldMatrix);
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D forward = worldMatrix.Forward;
      worldMatrix = this.WorldMatrix;
      Vector3D up = worldMatrix.Up;
      return MatrixD.CreateWorld(position, forward, up);
    }

    private void UpdateConstraintData()
    {
      MyAttachableTopBlockBase topBlock = this.TopBlock;
      if (topBlock == null || (HkReferenceObject) this.m_wheelConstraintData == (HkReferenceObject) null)
        return;
      this.FillConstraintData(topBlock, out Vector3 _, out Vector3 _);
    }

    private void OnRotorPhysicsChanged(MyEntity rotorGrid)
    {
      if (rotorGrid != this.TopGrid)
      {
        rotorGrid.OnPhysicsChanged -= new Action<MyEntity>(this.OnRotorPhysicsChanged);
      }
      else
      {
        MyGridPhysics physics = ((MyCubeGrid) rotorGrid).Physics;
        physics?.Shape.UnmarkBreakable(physics.RigidBody);
      }
    }

    private void FillConstraintData(
      MyAttachableTopBlockBase rotor,
      out Vector3 posA,
      out Vector3 posB)
    {
      float currentAirShock = this.CurrentAirShock;
      float v = MathHelper.Lerp(0.7f, 1f, currentAirShock);
      float strength = MathHelper.Lerp((float) this.m_strenth, 1f, currentAirShock);
      if (this.IsWorking)
        this.CubeGrid.GridSystems.WheelSystem.SetWheelJumpStrengthRatioIfJumpEngaged(ref strength, (float) this.m_strenth);
      float num = MathHelper.Lerp((float) this.m_height, this.BlockDefinition.MinHeight, currentAirShock);
      MyWheel.WheelExplosionLog(this.CubeGrid, (MyTerminalBlock) this, "FillConstraint: " + (object) this.CubeGrid.GridSystems.WheelSystem.m_jumpState + " " + (object) strength + " " + (object) num + " " + (object) currentAirShock);
      Vector3 forward = this.PositionComp.LocalMatrixRef.Forward;
      posA = this.DummyPosition + forward * num;
      posB = (rotor as MyMotorRotor).WheelDummy;
      Matrix matrix = this.PositionComp.LocalMatrixRef;
      Vector3 up1 = matrix.Up;
      matrix = rotor.PositionComp.LocalMatrixRef;
      Vector3 up2 = matrix.Up;
      this.m_wheelConstraintData.SetInBodySpace(posB, posA, up2, up1, forward, forward, (MyPhysicsBody) this.RotorGrid.Physics, (MyPhysicsBody) this.CubeGrid.Physics);
      this.m_wheelConstraintData.SetSteeringAngle(this.m_steerAngle);
      this.m_wheelConstraintData.SetSuspensionDamping(v);
      this.m_wheelConstraintData.SetSuspensionStrength(strength);
      this.m_wheelConstraintData.SuspensionMinLimit = this.BlockDefinition.MinHeight - num;
      this.m_wheelConstraintData.SuspensionMaxLimit = this.BlockDefinition.MaxHeight - num;
      this.m_wheelConstraintData.FrictionEnabled = this.InternalFrictionEnabled;
      this.m_wheelConstraintData.MaxFrictionTorque = this.BlockDefinition.AxleFriction;
      this.ActivatePhysics();
    }

    public void GetCoMVectors(out Vector3 adjustmentVector) => this.GetCoMVectors(out adjustmentVector, out Vector3 _, out Vector3 _);

    public void GetCoMVectors(
      out Vector3 adjustmentVector,
      out Vector3 realCoM,
      out Vector3 suspensionPosition)
    {
      if (this.m_CoMVectorsCacheValid)
      {
        realCoM = this.m_realCoMCache;
        adjustmentVector = this.m_adjustmentVectorCache;
        suspensionPosition = this.m_suspensionPositionCache;
      }
      else
      {
        realCoM = this.CubeGrid.Physics.RigidBody.CenterOfMassLocal;
        suspensionPosition = this.Position * this.CubeGrid.GridSize;
        Vector3 projectedOntoVector = -Base6Directions.GetVector(this.Orientation.Forward);
        adjustmentVector = projectedOntoVector.Project(suspensionPosition - realCoM);
        adjustmentVector *= 0.9f;
        this.m_CoMVectorsCacheValid = true;
        this.m_realCoMCache = realCoM;
        this.m_adjustmentVectorCache = adjustmentVector;
        this.m_suspensionPositionCache = suspensionPosition;
      }
    }

    public void DebugDrawConstraint()
    {
      if (!((HkReferenceObject) this.m_constraint != (HkReferenceObject) null))
        return;
      Vector3 pivotA;
      Vector3 pivotB;
      this.m_constraint.GetPivotsInWorld(out pivotA, out pivotB);
      Vector3D world1 = this.CubeGrid.Physics.ClusterToWorld(pivotA);
      Vector3D world2 = this.CubeGrid.Physics.ClusterToWorld(pivotB);
      Vector3D up = this.WorldMatrix.Up;
      Vector3D forward = this.WorldMatrix.Forward;
      Vector3D vector3D = world2 + up;
      Vector3D pointFrom1 = world2 + forward * (double) this.m_wheelConstraintData.SuspensionMaxLimit;
      Vector3D pointFrom2 = world2 + forward * (double) this.m_wheelConstraintData.SuspensionMinLimit;
      MyRenderProxy.DebugDrawSphere(world2, 0.05f, Color.Red, depthRead: false);
      MyRenderProxy.DebugDrawSphere(vector3D, 0.05f, Color.Red, depthRead: false);
      MyRenderProxy.DebugDrawSphere(world1 + up, 0.05f, Color.Red, depthRead: false);
      MyRenderProxy.DebugDrawLine3D(world2, vector3D, Color.Red, Color.Red, false);
      MyRenderProxy.DebugDrawLine3D(world2, world1 + up, Color.Red, Color.Red, false);
      MyRenderProxy.DebugDrawLine3D(pointFrom1, pointFrom1 + up, Color.Blue, Color.Blue, false);
      MyRenderProxy.DebugDrawLine3D(pointFrom2, pointFrom2 + up, Color.Blue, Color.Blue, false);
    }

    public override void ComputeTopQueryBox(
      out Vector3D pos,
      out Vector3 halfExtents,
      out Quaternion orientation)
    {
      MatrixD matrix = this.WorldMatrix;
      orientation = Quaternion.CreateFromRotationMatrix(in matrix);
      halfExtents = Vector3.One * this.CubeGrid.GridSize * 0.35f;
      halfExtents.Y = this.CubeGrid.GridSize;
      pos = matrix.Translation + 0.349999994039536 * (double) this.CubeGrid.GridSize * this.WorldMatrix.Up;
    }

    protected override bool CanPlaceTop(MyAttachableTopBlockBase topBlock, long builtBy) => this.CanPlaceRotor(topBlock, builtBy);

    protected override bool CanPlaceRotor(MyAttachableTopBlockBase rotorBlock, long builtBy)
    {
      Vector3D vector3D = Vector3D.Transform(this.DummyPosition + this.PositionComp.LocalMatrixRef.Forward * (float) this.m_height, this.CubeGrid.WorldMatrix);
      Matrix matrix = rotorBlock.PositionComp.LocalMatrixRef;
      Vector3 vector3 = -Vector3.TransformNormal(Vector3.Transform(rotorBlock.WheelDummy, Matrix.Invert(matrix)), rotorBlock.WorldMatrix);
      Vector3D translation = vector3D + vector3;
      float num = rotorBlock.ModelCollision.HavokCollisionShapes[0].ConvexRadius * 0.9f;
      BoundingSphereD boundingSphere = (BoundingSphereD) rotorBlock.Model.BoundingSphere;
      boundingSphere.Center = translation;
      boundingSphere.Radius = (double) num;
      this.CubeGrid.GetBlocksInsideSphere(ref boundingSphere, MyMechanicalConnectionBlockBase.m_tmpSet);
      if (MyMechanicalConnectionBlockBase.m_tmpSet.Count > 1)
      {
        MyMechanicalConnectionBlockBase.m_tmpSet.Clear();
        if (builtBy == MySession.Static.LocalPlayerId)
          MyHud.Notifications.Add(MyNotificationSingletons.WheelNotPlaced);
        return false;
      }
      MyMechanicalConnectionBlockBase.m_tmpSet.Clear();
      MyPhysics.GetPenetrationsShape(rotorBlock.ModelCollision.HavokCollisionShapes[0], ref translation, ref Quaternion.Identity, MyMotorSuspension.m_tmpList, 15);
      for (int index = 0; index < MyMotorSuspension.m_tmpList.Count; ++index)
      {
        VRage.ModAPI.IMyEntity collisionEntity = MyMotorSuspension.m_tmpList[index].GetCollisionEntity();
        if (!collisionEntity.Physics.IsPhantom && (!(collisionEntity.GetTopMostParent() is MyCubeGrid topMostParent) || topMostParent != this.CubeGrid))
        {
          MyMotorSuspension.m_tmpList.Clear();
          if (builtBy == MySession.Static.LocalPlayerId)
            MyHud.Notifications.Add(MyNotificationSingletons.WheelNotPlaced);
          return false;
        }
      }
      MyMotorSuspension.m_tmpList.Clear();
      return true;
    }

    internal void AxleFrictionLogic(float maxSpeedRatio, bool anyPropulsion)
    {
      if (MyPhysicsConfig.OverrideWheelAxleFriction || (HkReferenceObject) this.m_wheelConstraintData == (HkReferenceObject) null)
        return;
      bool flag = !anyPropulsion;
      if (flag)
        this.m_wheelConstraintData.MaxFrictionTorque = this.BlockDefinition.AxleFriction * Math.Max(0.1f, maxSpeedRatio);
      if (!MyDebugDrawSettings.DEBUG_DRAW_WHEEL_SYSTEMS)
        return;
      MyRenderProxy.DebugDrawText3D(this.WorldMatrix.Translation, "F: " + (flag ? this.m_wheelConstraintData.MaxFrictionTorque : 0.0f).ToString("F1"), Color.Red, 0.5f, false);
    }

    private void ResetConstraintFriction()
    {
      if ((HkReferenceObject) this.m_wheelConstraintData == (HkReferenceObject) null)
        return;
      int num = this.InternalFrictionEnabled ? 1 : 0;
      this.m_wheelConstraintData.FrictionEnabled = this.InternalFrictionEnabled;
    }

    internal void UpdatePropulsion(bool forward, bool backwards)
    {
      if (this.ShouldBrake)
        return;
      bool flag = false;
      float propulsionOverride = this.PropulsionOverride;
      if ((double) propulsionOverride != 0.0)
      {
        bool forward1 = (double) propulsionOverride > 0.0;
        if (this.InvertPropulsion)
          forward1 = !forward1;
        flag = true;
        this.Accelerate(Math.Abs(propulsionOverride) * this.BlockDefinition.PropulsionForce, forward1);
      }
      else if (forward)
      {
        flag = true;
        this.Accelerate(this.BlockDefinition.PropulsionForce * this.Power, !this.InvertPropulsion);
      }
      else if (backwards)
      {
        flag = true;
        this.Accelerate(this.BlockDefinition.PropulsionForce * this.Power, this.InvertPropulsion);
      }
      this.InternalFrictionEnabled = !flag;
    }

    private void Accelerate(float force, bool forward)
    {
      if (!this.IsWorking)
        return;
      MyCubeGrid topGrid = this.TopGrid;
      if (topGrid == null)
        return;
      MatrixD worldMatrix = topGrid.WorldMatrix;
      MyGridPhysics physics1 = this.TopGrid.Physics;
      MyGridPhysics physics2 = this.CubeGrid.Physics;
      if (physics1 == null)
        return;
      Vector3 vector3 = physics2.LinearVelocity;
      float val2 = vector3.Length();
      bool flag = (double) this.PropulsionOverride == 0.0 ? this.m_wheelInversions.RevolveInvert == forward : forward;
      float num1 = Vector3.Dot(physics1.AngularVelocity, flag ? (Vector3) worldMatrix.Up : (Vector3) worldMatrix.Down);
      if ((double) val2 > (double) this.SpeedLimit * 0.277777791023254 && (double) num1 > 0.0)
        return;
      float num2 = (float) this.TopBlock.BlockDefinition.Size.X * topGrid.GridSizeHalf;
      vector3 = physics1.AngularVelocity;
      double num3 = (double) vector3.LengthSquared();
      float num4 = this.SpeedLimit * 0.2777778f / num2;
      double num5 = (double) (num4 * num4);
      if (num3 > num5 && (double) num1 > 0.0)
        return;
      float num6 = 1f;
      if (MyFakes.SUSPENSION_POWER_RATIO)
      {
        if (MyDebugDrawSettings.DEBUG_DRAW_SUSPENSION_POWER)
        {
          for (int index = 2; index < 20; ++index)
          {
            float num7 = Math.Min(1f, (float) (1.0 - ((double) ((index - 1) * 10) - 10.0) / ((double) physics2.RigidBody.MaxLinearVelocity - 20.0)));
            float num8 = Math.Min(1f, (float) (1.0 - ((double) (index * 10) - 10.0) / ((double) physics2.RigidBody.MaxLinearVelocity - 20.0)));
            MyRenderProxy.DebugDrawLine2D(new Vector2((float) (300 + index * 20), (float) (400.0 - (double) num8 * 200.0)), new Vector2((float) (300 + (index - 1) * 20), (float) (400.0 - (double) num7 * 200.0)), Color.Yellow, Color.Yellow);
            MyRenderProxy.DebugDrawText2D(new Vector2((float) (300 + (index - 1) * 20), 400f), ((index - 1) * 10).ToString(), Color.Yellow, 0.35f);
          }
        }
        vector3 = physics1.AngularVelocity;
        float num9 = vector3.Length() * num2;
        num6 = MathHelper.Clamp((float) (1.0 - ((double) num9 - 10.0) / ((double) physics2.RigidBody.MaxLinearVelocity - 20.0)), 0.0f, 1f);
        if (MyDebugDrawSettings.DEBUG_DRAW_SUSPENSION_POWER)
        {
          MyRenderProxy.DebugDrawText2D(new Vector2((float) (300.0 + (double) num9 * 2.0), (float) (400.0 - (double) num6 * 200.0)), "I", Color.Red, 0.3f);
          MyRenderProxy.DebugDrawText2D(new Vector2(290f, (float) (400.0 - (double) num6 * 200.0)), num6.ToString(), Color.Yellow, 0.35f);
        }
      }
      float num10 = num6 / MathHelper.Lerp(1f, 2f, Math.Min(10f, val2) / 10f);
      force *= num10;
      HkRigidBody rigidBody = physics1.RigidBody;
      if (flag)
        rigidBody.ApplyAngularImpulse(rigidBody.GetRigidBodyMatrix().Up * force);
      else
        rigidBody.ApplyAngularImpulse((Vector3) worldMatrix.Down * force);
      this.m_wasAccelerating = true;
    }

    public void Steer(float destIndicator, float maxSpeedRatio)
    {
      if ((double) this.SteeringOverride != 0.0)
        destIndicator = this.SteeringOverride;
      else if (this.m_wheelInversions.SteerInvert)
        destIndicator = -destIndicator;
      if (!this.InvertSteer)
        destIndicator = -destIndicator;
      destIndicator = MathHelper.Clamp(destIndicator, -1f, 1f);
      float num1 = 1f - maxSpeedRatio;
      float num2 = num1 * num1 * num1;
      double maxSteerAngle = (double) this.MaxSteerAngle;
      float num3 = MathHelper.Lerp((float) maxSteerAngle, (float) (maxSteerAngle / 10.0), 1f - num2) * destIndicator;
      int num4 = (double) this.m_steerAngle < (double) num3 ? 1 : 0;
      float steeringSpeed = this.BlockDefinition.SteeringSpeed;
      if ((double) maxSpeedRatio < 0.01)
        steeringSpeed *= 2f;
      float num5 = steeringSpeed * (float) (0.200000002980232 + 0.800000011920929 * (double) num2);
      float f = num3 - this.m_steerAngle;
      if (float.IsNaN(f))
        f = 0.0f;
      this.m_steerAngle += num5 * (float) Math.Sign(f);
      int num6 = (double) this.m_steerAngle < (double) num3 ? 1 : 0;
      bool flag = num4 == num6 && (double) f != 0.0;
      if (this.m_steeringChanged != flag)
      {
        this.m_steeringChanged = flag;
        this.OnPerFrameUpdatePropertyChanged();
      }
      if (!flag || (double) this.m_steerAngle == (double) num3 || (!((HkReferenceObject) this.SafeConstraint != (HkReferenceObject) null) || !this.Steering))
        return;
      this.ActivatePhysics();
      this.m_wheelConstraintData.SetSteeringAngle(this.m_steerAngle);
    }

    public void Update()
    {
      this.AirShockLogic();
      this.ArtificialBreakingLogic();
      if (MyPhysicsConfig.OverrideWheelAxleFriction && (HkReferenceObject) this.m_wheelConstraintData != (HkReferenceObject) null)
      {
        this.m_defaultInternalFriction = true;
        this.m_wheelConstraintData.MaxFrictionTorque = MyPhysicsConfig.WheelAxleFriction;
        this.ResetConstraintFriction();
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS)
      {
        Vector3 adjustmentVector;
        Vector3 realCoM;
        Vector3 suspensionPosition;
        this.GetCoMVectors(out adjustmentVector, out realCoM, out suspensionPosition);
        MatrixD worldMatrix = this.CubeGrid.WorldMatrix;
        MyRenderProxy.DebugDrawSphere(Vector3D.Transform(realCoM, worldMatrix), 0.1f, Color.Red, depthRead: false);
        MyRenderProxy.DebugDrawSphere(Vector3D.Transform(suspensionPosition, worldMatrix), 0.1f, Color.Green, depthRead: false);
        MyRenderProxy.DebugDrawArrow3DDir(Vector3D.Transform(realCoM, worldMatrix), Vector3D.TransformNormal(adjustmentVector, worldMatrix), Color.Yellow);
        MyRenderProxy.DebugDrawArrow3DDir(Vector3D.Transform(suspensionPosition, worldMatrix), Vector3D.TransformNormal(adjustmentVector, worldMatrix), Color.Blue);
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer && this.ShouldBrake && ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null && !(bool) this.m_breakingConstraint) && (double) this.TopGrid.Physics.LinearVelocity.LengthSquared() < 1.0)
        this.m_breakingConstraint.Value = true;
      this.UpdateSoundState();
      this.ResourceSink.Update();
      this.m_wasAccelerating = false;
    }

    protected override void UpdateSoundState()
    {
      if (!MySandboxGame.IsGameReady || this.m_soundEmitter == null)
        return;
      if (this.TopGrid == null || this.TopGrid.Physics == null)
      {
        this.m_soundEmitter.StopSound(true);
      }
      else
      {
        if (this.IsWorking && (double) Math.Abs(this.TopGrid.Physics.RigidBody.DeltaAngle.W - this.CubeGrid.Physics.RigidBody.DeltaAngle.W) > 1.0 / 400.0)
          this.m_soundEmitter.PlaySingleSound(this.BlockDefinition.PrimarySound, true);
        else if (this.m_soundEmitter != null && this.m_soundEmitter.IsPlaying)
          this.m_soundEmitter.StopSound(false);
        if (this.m_soundEmitter.Sound == null || !this.m_soundEmitter.Sound.IsPlaying)
          return;
        this.m_soundEmitter.Sound.FrequencyRatio = MyAudio.Static.SemitonesToFrequencyRatio((float) (4.0 * ((double) Math.Abs(this.RotorAngularVelocity.Length()) - 0.5 * (double) this.MaxRotorAngularVelocity)) / this.MaxRotorAngularVelocity) * (this.m_wasAccelerating ? 1f : 0.95f);
      }
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_10TH_FRAME;
      if (this.m_updateBrakeNeeded)
        this.UpdateBrake();
      if (this.m_updateFrictionNeeded)
        this.FrictionChanged();
      if (this.m_updateDampingNeeded)
        this.DampingChanged();
      if (!this.m_updateStrengthNeeded)
        return;
      this.StrengthChanged();
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      this.CheckSafetyDetach();
    }

    public float GetStrengthForTerminal() => this.Strength * 100f;

    public float GetPowerForTerminal() => this.Power * 100f;

    public float GetHeightForTerminal() => this.Height;

    public override Vector3? GetConstraintPosition(MyCubeGrid grid, bool opposite = false)
    {
      if ((HkReferenceObject) this.m_constraint == (HkReferenceObject) null)
        return new Vector3?();
      if (grid == this.CubeGrid)
        return !opposite ? this.m_constraintPositionA : this.m_constraintPositionB;
      if (grid != this.TopGrid)
        return new Vector3?();
      return !opposite ? this.m_constraintPositionB : this.m_constraintPositionA;
    }

    protected override float GetConstraintDisplacementSq()
    {
      Vector3 forward = (Vector3) this.WorldMatrix.Forward;
      Vector3 pivotA;
      Vector3 pivotB;
      this.m_constraint.GetPivotsInWorld(out pivotA, out pivotB);
      Vector3 vec = pivotB - pivotA;
      Vector3 vector3 = Vector3.ProjectOnVector(ref vec, ref forward);
      return (vec - vector3).LengthSquared();
    }

    private void AirShockLogic()
    {
      float val1 = this.CalculateCurrentAirShockRatio();
      if ((double) val1 != (double) this.CurrentAirShock)
      {
        if ((double) val1 < (double) this.CurrentAirShock)
          val1 = Math.Max(val1, this.CurrentAirShock - 0.05f);
        this.CurrentAirShock = val1;
        this.UpdateConstraintData();
      }
      if (!MyDebugDrawSettings.DEBUG_DRAW_WHEEL_SYSTEMS || (double) this.CurrentAirShock <= 0.0)
        return;
      MyRenderProxy.DebugDrawText3D(this.WorldMatrix.Translation + this.WorldMatrix.Up, "AirShock " + this.CurrentAirShock.ToString("F2"), Color.Red, 0.5f, false);
    }

    private float CalculateCurrentAirShockRatio()
    {
      MyWheel wheel;
      Vector3 linearVelocity;
      if (!this.AirShockEnabled || this.CubeGrid.GridSystems.WheelSystem.HandBrake || !this.GetWheelAndLinearVelocity(out wheel, out linearVelocity))
        return 0.0f;
      MyMotorSuspensionDefinition blockDefinition = this.BlockDefinition;
      if (wheel.FramesSinceLastContact < (ulong) blockDefinition.AirShockActivationDelay)
        return 0.0f;
      float airShockMinSpeed = blockDefinition.AirShockMinSpeed;
      float airShockMaxSpeed = blockDefinition.AirShockMaxSpeed;
      Vector3 naturalGravityInPoint = MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.WorldMatrix.Translation);
      float num = Vector3.ProjectOnVector(ref linearVelocity, ref naturalGravityInPoint).LengthSquared();
      return (double) num < (double) airShockMinSpeed * (double) airShockMinSpeed ? 0.0f : Math.Min(1f, ((float) Math.Sqrt((double) num) - airShockMinSpeed) / (airShockMaxSpeed - airShockMinSpeed));
    }

    private void ArtificialBreakingLogic()
    {
      MyWheel wheel;
      Vector3 linearVelocity;
      if (!this.ShouldBrake || !this.GetWheelAndLinearVelocity(out wheel, out linearVelocity) || ((double) linearVelocity.LengthSquared() < 1.0 || !wheel.IsConsideredInContactWithStaticSurface))
        return;
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3 left = (Vector3) worldMatrix.Left;
      Vector3 vector3_1 = Vector3.ProjectOnVector(ref linearVelocity, ref left);
      float num1 = vector3_1.Length();
      Vector3 vector3_2 = vector3_1 / num1;
      float maxLinearVelocity = MyGridPhysics.GetShipMaxLinearVelocity(this.CubeGrid.GridSizeEnum);
      float num2 = Math.Min(1f, num1 / maxLinearVelocity);
      if ((double) num2 < 0.01)
        return;
      Vector3 dir = this.CubeGrid.Physics.Mass * 0.5f * MyPhysicsConfig.ArtificialBrakingMultiplier * num2 * -vector3_2;
      Vector3 adjustmentVector;
      this.GetCoMVectors(out adjustmentVector);
      Vector3 vector3_3 = -Vector3.TransformNormal(adjustmentVector, this.CubeGrid.WorldMatrix) * MyPhysicsConfig.ArtificialBrakingCoMStabilization;
      Vector3D pos = worldMatrix.Translation + vector3_3;
      this.CubeGrid.Physics.ApplyImpulse(dir, pos);
    }

    public bool LateralCorrectionLogicInfo(ref Vector3 groundNormal, ref Vector3 suspensionNormal)
    {
      MyWheel wheel;
      Vector3 linearVelocity;
      if (!this.GetWheelAndLinearVelocity(out wheel, out linearVelocity) || !wheel.IsConsideredInContactWithStaticSurface || (double) linearVelocity.LengthSquared() <= 25.0)
        return false;
      suspensionNormal += -Base6Directions.GetVector(this.Orientation.Forward);
      groundNormal += wheel.LastUsedGroundNormal;
      return true;
    }

    private bool GetWheelAndLinearVelocity(out MyWheel wheel, out Vector3 linearVelocity)
    {
      wheel = this.Rotor as MyWheel;
      linearVelocity = Vector3.Zero;
      if (wheel == null)
        return false;
      MyGridPhysics physics = this.CubeGrid.Physics;
      if (physics == null)
        return false;
      linearVelocity = physics.LinearVelocity;
      return true;
    }

    public void OnSuspensionJumpStateUpdated() => this.StrengthChanged();

    private void OnIsWorkingChanged(MyCubeBlock myCubeBlock)
    {
      if (this.IsWorking)
        return;
      this.m_steeringChanged = false;
      this.OnPerFrameUpdatePropertyChanged();
    }

    private void OnPerFrameUpdatePropertyChanged() => this.NeedsPerFrameUpdate = this.m_steeringChanged || (double) this.PropulsionOverride != 0.0 || (double) this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) > (double) this.BlockDefinition.RequiredIdlePowerInput;

    private void ActivatePhysics()
    {
      this.TopGrid.Physics.ActivateIfNeeded();
      this.CubeGrid.Physics.ActivateIfNeeded();
    }

    float Sandbox.ModAPI.Ingame.IMyMotorSuspension.Strength
    {
      get => this.GetStrengthForTerminal();
      set => this.Strength = MathHelper.Clamp(value / 100f, 0.0f, 1f);
    }

    float Sandbox.ModAPI.Ingame.IMyMotorSuspension.Friction
    {
      get => this.Friction * 100f;
      set => this.Friction = MathHelper.Clamp(value / 100f, 0.0f, 1f);
    }

    float Sandbox.ModAPI.Ingame.IMyMotorSuspension.Power
    {
      get => this.GetPowerForTerminal();
      set => this.Power = MathHelper.Clamp(value / 100f, 0.0f, 1f);
    }

    float Sandbox.ModAPI.Ingame.IMyMotorSuspension.Height
    {
      get => this.GetHeightForTerminal();
      set => this.Height = MathHelper.Clamp(value, this.BlockDefinition.MinHeight, this.BlockDefinition.MaxHeight);
    }

    float Sandbox.ModAPI.Ingame.IMyMotorSuspension.SteerAngle => this.m_steerAngle;

    float Sandbox.ModAPI.Ingame.IMyMotorSuspension.MaxSteerAngle
    {
      get => this.MaxSteerAngle;
      set => this.MaxSteerAngle = MathHelper.Clamp(value, 0.0f, this.BlockDefinition.MaxSteer);
    }

    bool Sandbox.ModAPI.Ingame.IMyMotorSuspension.Brake
    {
      get => this.BrakingEnabled;
      set => this.BrakingEnabled = value;
    }

    [Obsolete]
    float Sandbox.ModAPI.Ingame.IMyMotorSuspension.Damping => 70f;

    [Obsolete]
    float Sandbox.ModAPI.Ingame.IMyMotorSuspension.SteerSpeed => 0.01f;

    [Obsolete]
    float Sandbox.ModAPI.Ingame.IMyMotorSuspension.SteerReturnSpeed => 0.01f;

    [Obsolete]
    float Sandbox.ModAPI.Ingame.IMyMotorSuspension.SuspensionTravel => 100f;

    private struct MyWheelInversions
    {
      public bool SteerInvert;
      public bool RevolveInvert;
    }

    protected class m_brake\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_brake = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_strenth\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_strenth = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_height\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_height = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_breakingConstraint\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.FromServer>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_breakingConstraint = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_speedLimit\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_speedLimit = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_friction\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_friction = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_maxSteerAngle\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_maxSteerAngle = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_invertSteer\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_invertSteer = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_invertPropulsion\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_invertPropulsion = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_power\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_power = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_steering\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_steering = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_propulsion\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_propulsion = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_airShockEnabled\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_airShockEnabled = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_brakingEnabled\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_brakingEnabled = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_propulsionOverride\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_propulsionOverride = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_steeringOverride\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyMotorSuspension) obj0).m_steeringOverride = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Cube_MyMotorSuspension\u003C\u003EActor : IActivator, IActivator<MyMotorSuspension>
    {
      object IActivator.CreateInstance() => (object) new MyMotorSuspension();

      MyMotorSuspension IActivator<MyMotorSuspension>.CreateInstance() => new MyMotorSuspension();
    }
  }
}
