// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridWheelSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
  [StaticEventOwner]
  public class MyGridWheelSystem : MyUpdateableGridSystem
  {
    private Vector3 m_angularVelocity;
    private const int JUMP_FULL_CHARGE_TIME = 600;
    private bool m_wheelsChanged;
    private float m_maxRequiredPowerInput;
    private readonly HashSet<MyMotorSuspension> m_wheels;
    private readonly HashSet<MyMotorSuspension> m_wheelsNeedingUpdate;
    private readonly MyResourceSinkComponent m_sinkComp;
    private bool m_handbrake;
    private bool m_brake;
    private ulong m_jumpStartTime;
    private bool m_lastJumpInput;
    public MyGridWheelSystem.WheelJumpSate m_jumpState;
    private int m_consecutiveCorrectionFrames;
    private Vector3 m_lastPhysicsAngularVelocityLateral;

    public Vector3 AngularVelocity
    {
      get => this.m_angularVelocity;
      set
      {
        if (!(this.m_angularVelocity != value))
          return;
        this.m_angularVelocity = value;
        if (this.m_wheels.Count <= 0)
          return;
        this.Schedule();
      }
    }

    public HashSet<MyMotorSuspension> Wheels => this.m_wheels;

    public int WheelCount => this.m_wheels.Count;

    public bool HandBrake
    {
      get => this.m_handbrake;
      set
      {
        if (this.m_handbrake == value)
          return;
        this.m_handbrake = value;
        if (Sync.IsServer)
          this.UpdateBrake();
        if (!value)
          return;
        this.Schedule();
      }
    }

    public bool Brake
    {
      get => this.m_brake;
      set
      {
        if (this.m_brake == value)
          return;
        this.m_brake = value;
        this.UpdateBrake();
        if (!value)
          return;
        this.Schedule();
      }
    }

    public bool LastJumpInput => this.m_lastJumpInput;

    public MyGridWheelSystem(MyCubeGrid grid)
      : base(grid)
    {
      this.m_wheels = new HashSet<MyMotorSuspension>();
      this.m_wheelsNeedingUpdate = new HashSet<MyMotorSuspension>();
      this.m_wheelsChanged = false;
      this.m_sinkComp = new MyResourceSinkComponent();
      this.m_sinkComp.Init(MyStringHash.GetOrCompute("Utility"), this.m_maxRequiredPowerInput, (Func<float>) (() => this.m_maxRequiredPowerInput));
      this.m_sinkComp.IsPoweredChanged += new Action(this.ReceiverIsPoweredChanged);
      grid.OnPhysicsChanged += new Action<MyEntity>(this.OnGridPhysicsChanged);
    }

    public void Register(MyMotorSuspension motor)
    {
      this.m_wheels.Add(motor);
      this.OnBlockNeedsUpdateChanged(motor);
      motor.EnabledChanged += new Action<MyTerminalBlock>(this.MotorEnabledChanged);
      if (Sync.IsServer)
        motor.Brake = this.m_handbrake;
      this.MarkDirty();
    }

    public event Action<MyCubeGrid> OnMotorUnregister;

    public void Unregister(MyMotorSuspension motor)
    {
      if (motor == null)
        return;
      if (motor.RotorGrid != null && this.OnMotorUnregister != null)
        this.OnMotorUnregister(motor.RotorGrid);
      this.m_wheels.Remove(motor);
      this.m_wheelsNeedingUpdate.Remove(motor);
      motor.EnabledChanged -= new Action<MyTerminalBlock>(this.MotorEnabledChanged);
      this.MarkDirty();
    }

    public void OnBlockNeedsUpdateChanged(MyMotorSuspension motor)
    {
      if (motor.NeedsPerFrameUpdate)
      {
        this.m_wheelsNeedingUpdate.Add(motor);
        this.MarkDirty();
      }
      else
        this.m_wheelsNeedingUpdate.Remove(motor);
    }

    protected override void Update()
    {
      if (!MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
        return;
      if (this.m_wheelsChanged)
        this.RecomputeWheelParameters();
      if (this.m_jumpState == MyGridWheelSystem.WheelJumpSate.Pushing || this.m_jumpState == MyGridWheelSystem.WheelJumpSate.Restore)
      {
        MyWheel.WheelExplosionLog(this.Grid, (MyTerminalBlock) null, "JumpUpdate: " + (object) this.m_jumpState);
        foreach (MyMotorSuspension wheel in this.Wheels)
          wheel.OnSuspensionJumpStateUpdated();
        switch (this.m_jumpState)
        {
          case MyGridWheelSystem.WheelJumpSate.Pushing:
            this.m_jumpState = MyGridWheelSystem.WheelJumpSate.Restore;
            break;
          case MyGridWheelSystem.WheelJumpSate.Restore:
            this.m_jumpState = MyGridWheelSystem.WheelJumpSate.Idle;
            break;
        }
      }
      MyGridPhysics physics = this.Grid.Physics;
      if (physics != null)
      {
        Vector3 linearVelocity = physics.LinearVelocity;
        float maxSpeedRatio = linearVelocity.Normalize() / MyGridPhysics.GetShipMaxLinearVelocity(this.Grid.GridSizeEnum);
        if ((double) maxSpeedRatio > 1.0)
          maxSpeedRatio = 1f;
        bool forward = (double) this.m_angularVelocity.Z < 0.0;
        bool backwards = (double) this.m_angularVelocity.Z > 0.0;
        bool flag1 = forward | backwards;
        if (this.m_wheels.Count > 0)
        {
          int num = 0;
          bool flag2 = !this.Grid.GridSystems.GyroSystem.HasOverrideInput;
          Vector3 zero1 = Vector3.Zero;
          Vector3 zero2 = Vector3.Zero;
          foreach (MyMotorSuspension wheel in this.m_wheels)
          {
            bool flag3 = wheel.IsWorking && wheel.Propulsion;
            wheel.AxleFrictionLogic(maxSpeedRatio, flag1 & flag3);
            wheel.Update();
            if (flag2 && wheel.LateralCorrectionLogicInfo(ref zero2, ref zero1))
              ++num;
            if (wheel.IsWorking)
            {
              if (wheel.Steering)
                wheel.Steer(this.m_angularVelocity.X, maxSpeedRatio);
              if (wheel.Propulsion)
                wheel.UpdatePropulsion(forward, backwards);
            }
          }
          bool flag4 = ((flag2 ? 1 : 0) & (num == 0 || Vector3.IsZero(ref zero1) ? 0 : (!Vector3.IsZero(ref zero2) ? 1 : 0))) != 0;
          bool flag5 = false;
          if (flag4)
            flag5 = this.LateralCorrectionLogic(ref zero1, ref zero2, ref linearVelocity);
          if (flag5)
          {
            if (this.m_consecutiveCorrectionFrames < 10)
              ++this.m_consecutiveCorrectionFrames;
          }
          else
            this.m_consecutiveCorrectionFrames = (int) ((double) this.m_consecutiveCorrectionFrames * 0.800000011920929);
        }
      }
      if (this.m_wheelsNeedingUpdate.Count != 0 || this.Grid.Physics != null && (double) this.Grid.Physics.LinearVelocity.LengthSquared() >= 0.1 || !(this.m_angularVelocity == Vector3.Zero))
        return;
      this.DeSchedule();
    }

    private bool LateralCorrectionLogic(
      ref Vector3 gridDownNormal,
      ref Vector3 lateralCorrectionNormal,
      ref Vector3 linVelocityNormal)
    {
      if (!Sync.IsServer && !this.Grid.IsClientPredicted)
        return false;
      MyGridPhysics physics = this.Grid.Physics;
      bool flag = false;
      MatrixD worldMatrix = this.Grid.WorldMatrix;
      Vector3.TransformNormal(ref gridDownNormal, ref worldMatrix, out gridDownNormal);
      gridDownNormal = Vector3.ProjectOnPlane(ref gridDownNormal, ref linVelocityNormal);
      lateralCorrectionNormal = Vector3.ProjectOnPlane(ref lateralCorrectionNormal, ref linVelocityNormal);
      Vector3 vector2_1 = Vector3.Cross(gridDownNormal, linVelocityNormal);
      double num1 = (double) gridDownNormal.Normalize();
      double num2 = (double) lateralCorrectionNormal.Normalize();
      Vector3 angularVelocity = physics.AngularVelocity;
      Vector3 vector2_2 = Vector3.ProjectOnVector(ref angularVelocity, ref linVelocityNormal);
      float num3 = vector2_2.Length();
      float num4 = this.m_lastPhysicsAngularVelocityLateral.Length();
      if ((double) num3 > (double) num4)
        flag = true;
      float num5 = Vector3.Dot(lateralCorrectionNormal, vector2_1) * (float) Math.Max(1, this.m_consecutiveCorrectionFrames);
      double num6 = (double) num5 * (double) num3 * (double) num3;
      if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_SYSTEMS)
      {
        Vector3D translation = worldMatrix.Translation;
        MyRenderProxy.DebugDrawArrow3DDir(translation, (Vector3D) (lateralCorrectionNormal * 5f), Color.Yellow);
        MyRenderProxy.DebugDrawArrow3DDir(translation, (Vector3D) (gridDownNormal * 5f), Color.Pink);
        MyRenderProxy.DebugDrawArrow3DDir(translation, (Vector3D) (vector2_2 * 5f), Color.Red);
      }
      this.m_lastPhysicsAngularVelocityLateral = vector2_2;
      if ((double) Math.Abs((float) num6) > 0.0199999995529652)
      {
        Vector3 vector3 = linVelocityNormal * num5;
        if ((double) Vector3.Dot(vector3, vector2_2) > 0.0)
        {
          Vector3 sphere = Vector3.ClampToSphere(vector3, vector2_2.Length());
          if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_SYSTEMS)
            MyRenderProxy.DebugDrawArrow3DDir(worldMatrix.Translation - gridDownNormal * 5f, (Vector3D) (sphere * 100f), Color.Red);
          MyGridPhysics myGridPhysics = physics;
          myGridPhysics.AngularVelocity = myGridPhysics.AngularVelocity - sphere;
          flag = true;
        }
      }
      return flag;
    }

    public bool HasWorkingWheels(bool propulsion)
    {
      foreach (MyMotorSuspension wheel in this.m_wheels)
      {
        if (wheel.IsWorking && (!propulsion || wheel.RotorGrid != null && (double) wheel.RotorAngularVelocity.LengthSquared() > 2.0))
          return true;
      }
      return false;
    }

    internal void InitControl(MyEntity controller)
    {
      foreach (MyMotorSuspension wheel in this.m_wheels)
        wheel.InitControl(controller);
    }

    internal void ReleaseControl(MyEntity controller)
    {
      foreach (MyMotorSuspension wheel in this.m_wheels)
        wheel.ReleaseControl(controller);
      this.UpdateJumpControlState(false, false);
    }

    private void UpdateBrake()
    {
      foreach (MyMotorSuspension wheel in this.m_wheels)
        wheel.Brake = this.m_brake | this.m_handbrake;
    }

    private void OnGridPhysicsChanged(MyEntity obj)
    {
      if (this.Grid.GridSystems == null || this.Grid.GridSystems.ControlSystem == null)
        return;
      MyShipController shipController = this.Grid.GridSystems.ControlSystem.GetShipController();
      if (shipController == null)
        return;
      this.InitControl((MyEntity) shipController);
    }

    private void RecomputeWheelParameters()
    {
      this.m_wheelsChanged = false;
      this.m_maxRequiredPowerInput = 0.0f;
      foreach (MyMotorSuspension wheel in this.m_wheels)
      {
        if (MyGridWheelSystem.IsUsed(wheel))
          this.m_maxRequiredPowerInput += wheel.RequiredPowerInput;
      }
      this.m_sinkComp.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, this.m_maxRequiredPowerInput);
      this.m_sinkComp.Update();
    }

    private static bool IsUsed(MyMotorSuspension motor) => motor.Enabled && motor.IsFunctional;

    private void MotorEnabledChanged(MyTerminalBlock obj) => this.MarkDirty();

    private void ReceiverIsPoweredChanged()
    {
      foreach (MyCubeBlock wheel in this.m_wheels)
        wheel.UpdateIsWorking();
    }

    public void UpdateJumpControlState(bool isCharging, bool sync)
    {
      if (isCharging && this.Grid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId) != MyResourceStateEnum.Ok)
        isCharging = false;
      bool lastJumpInput = this.m_lastJumpInput;
      if (lastJumpInput == isCharging)
        return;
      this.Schedule();
      if (sync || Sync.IsServer)
        MyMultiplayer.RaiseStaticEvent<long, bool>((Func<IMyEventOwner, Action<long, bool>>) (s => new Action<long, bool>(MyGridWheelSystem.InvokeJumpInternal)), this.Grid.EntityId, !lastJumpInput);
      this.m_lastJumpInput = isCharging;
    }

    [Event(null, 448)]
    [Reliable]
    [Server]
    [Broadcast]
    public static void InvokeJumpInternal(long gridId, bool initiate)
    {
      MyCubeGrid entityById = (MyCubeGrid) Sandbox.Game.Entities.MyEntities.GetEntityById(gridId);
      if (entityById == null)
        return;
      if (Sync.IsServer && !MyEventContext.Current.IsLocallyInvoked)
      {
        MyPlayer controllingPlayer1 = MySession.Static.Players.GetControllingPlayer((MyEntity) entityById);
        if ((controllingPlayer1 == null || (long) controllingPlayer1.Client.SteamUserId != (long) MyEventContext.Current.Sender.Value) && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
        {
          MyPlayer controllingPlayer2 = MySession.Static.Players.GetPreviousControllingPlayer((MyEntity) entityById);
          (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, (controllingPlayer2 == null || (long) controllingPlayer2.Client.SteamUserId != (long) MyEventContext.Current.Sender.Value) && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value), (string) null, true);
          MyEventContext.ValidationFailed();
          return;
        }
      }
      MyWheel.WheelExplosionLog(entityById, (MyTerminalBlock) null, "InvokeJump" + initiate.ToString());
      MyGridWheelSystem wheelSystem = entityById.GridSystems.WheelSystem;
      if (initiate)
      {
        wheelSystem.m_jumpState = MyGridWheelSystem.WheelJumpSate.Charging;
        wheelSystem.m_jumpStartTime = MySandboxGame.Static.SimulationFrameCounter;
      }
      else
        wheelSystem.m_jumpState = MyGridWheelSystem.WheelJumpSate.Pushing;
      foreach (MyMotorSuspension wheel in wheelSystem.Wheels)
        wheel.OnSuspensionJumpStateUpdated();
    }

    public void SetWheelJumpStrengthRatioIfJumpEngaged(ref float strength, float defaultStrength)
    {
      switch (this.m_jumpState)
      {
        case MyGridWheelSystem.WheelJumpSate.Charging:
          strength = 0.0001f;
          break;
        case MyGridWheelSystem.WheelJumpSate.Pushing:
          float num = Math.Min(1f, (float) (MySandboxGame.Static.SimulationFrameCounter - this.m_jumpStartTime) / 600f);
          strength = defaultStrength + (1f - defaultStrength) * num;
          break;
      }
    }

    private void MarkDirty()
    {
      this.m_wheelsChanged = true;
      this.Schedule();
    }

    public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.BeforeSimulation;

    public override int UpdatePriority => 9;

    public enum WheelJumpSate
    {
      Idle,
      Charging,
      Pushing,
      Restore,
    }

    protected sealed class InvokeJumpInternal\u003C\u003ESystem_Int64\u0023System_Boolean : ICallSite<IMyEventOwner, long, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long gridId,
        in bool initiate,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGridWheelSystem.InvokeJumpInternal(gridId, initiate);
      }
    }
  }
}
