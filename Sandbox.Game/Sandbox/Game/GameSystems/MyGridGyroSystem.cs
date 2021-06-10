// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridGyroSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
  public class MyGridGyroSystem : MyUpdateableGridSystem
  {
    private static readonly float INV_TENSOR_MAX_LIMIT = 125000f;
    private static readonly float MAX_SLOWDOWN = MyFakes.WELD_LANDING_GEARS ? 0.8f : 0.93f;
    private static readonly float MAX_ROLL = 1.570796f;
    private const float TORQUE_SQ_LEN_TH = 0.0001f;
    private Vector3 m_controlTorque;
    public bool AutopilotEnabled;
    private HashSet<MyGyro> m_gyros;
    private bool m_gyrosChanged;
    private MyPhysicsComponentBase m_gridPhysics;
    private bool m_scheduled;
    private float m_maxGyroForce;
    private float m_maxOverrideForce;
    private float m_maxRequiredPowerInput;
    private Vector3 m_overrideTargetVelocity;
    private int? m_overrideAccelerationRampFrames;
    public Vector3 SlowdownTorque;

    public Vector3 ControlTorque
    {
      get => this.m_controlTorque;
      set
      {
        if (!(this.m_controlTorque != value))
          return;
        this.m_controlTorque = value;
        if (this.m_gyros.Count <= 0 || !this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
          return;
        this.Schedule();
      }
    }

    public bool HasOverrideInput => !Vector3.IsZero(ref this.m_controlTorque) || !Vector3.IsZero(ref this.m_overrideTargetVelocity);

    public bool IsDirty => this.m_gyrosChanged;

    public MyResourceSinkComponent ResourceSink { get; private set; }

    public int GyroCount => this.m_gyros.Count;

    public HashSet<MyGyro> Gyros => this.m_gyros;

    public Vector3 Torque { get; private set; }

    public MyGridGyroSystem(MyCubeGrid grid)
      : base(grid)
    {
      this.m_gyros = new HashSet<MyGyro>();
      this.m_gyrosChanged = false;
      this.ResourceSink = new MyResourceSinkComponent();
      this.ResourceSink.Init(MyStringHash.GetOrCompute("Gyro"), this.m_maxRequiredPowerInput, (Func<float>) (() => this.m_maxRequiredPowerInput));
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      grid.OnPhysicsChanged += new Action<MyEntity>(this.GridOnOnPhysicsChanged);
      grid.PositionComp.OnPositionChanged += new Action<MyPositionComponentBase>(this.PositionCompOnPositionChanged);
      this.TryHookToPhysics();
    }

    private void PositionCompOnPositionChanged(MyPositionComponentBase obj)
    {
      if (this.m_gridPhysics == null || this.m_gyros.Count <= 0 || (this.m_scheduled || Vector3.IsZero(this.m_gridPhysics.AngularVelocity, 1f / 1000f)) || !this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
        return;
      this.Schedule();
    }

    private void TryHookToPhysics()
    {
      MyGridPhysics physics = this.Grid.Physics;
      if (this.m_gridPhysics == physics)
        return;
      if (this.m_gridPhysics != null)
        this.m_gridPhysics.OnBodyActiveStateChanged -= new Action<MyPhysicsComponentBase, bool>(this.PhysicsOnOnBodyActiveStateChanged);
      if (physics != null)
        physics.OnBodyActiveStateChanged += new Action<MyPhysicsComponentBase, bool>(this.PhysicsOnOnBodyActiveStateChanged);
      this.m_gridPhysics = (MyPhysicsComponentBase) physics;
    }

    private void PhysicsOnOnBodyActiveStateChanged(MyPhysicsComponentBase body, bool active)
    {
    }

    private void GridOnOnPhysicsChanged(MyEntity obj)
    {
      this.TryHookToPhysics();
      if (this.m_gyros.Count == 0 || this.m_scheduled)
        return;
      if (this.Grid.Physics.IsStatic || this.Grid.Physics.IsKinematic)
      {
        this.DeSchedule();
      }
      else
      {
        if (!this.Grid.Physics.IsActive)
          return;
        this.Schedule();
      }
    }

    public void Register(MyGyro gyro)
    {
      this.m_gyros.Add(gyro);
      this.m_gyrosChanged = true;
      gyro.EnabledChanged += new Action<MyTerminalBlock>(this.gyro_EnabledChanged);
      gyro.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      gyro.PropertiesChanged += new Action<MyTerminalBlock>(this.gyro_PropertiesChanged);
      if (this.m_gyros.Count != 1)
        return;
      this.Schedule();
    }

    private void gyro_PropertiesChanged(MyTerminalBlock sender) => this.MarkDirty();

    public void Unregister(MyGyro gyro)
    {
      this.m_gyros.Remove(gyro);
      this.m_gyrosChanged = true;
      gyro.EnabledChanged -= new Action<MyTerminalBlock>(this.gyro_EnabledChanged);
      gyro.SlimBlock.ComponentStack.IsFunctionalChanged -= new Action(this.ComponentStack_IsFunctionalChanged);
    }

    private void UpdateGyros()
    {
      this.SlowdownTorque = Vector3.Zero;
      MyCubeGrid grid = this.Grid;
      MyGridPhysics physics = grid.Physics;
      if (physics == null || physics.IsKinematic || (physics.IsStatic || this.m_gyros.Count == 0))
      {
        this.DeSchedule();
      }
      else
      {
        if (!this.ControlTorque.IsValid())
          this.ControlTorque = Vector3.Zero;
        if (Vector3.IsZero(physics.AngularVelocity, 1f / 1000f) && Vector3.IsZero(this.ControlTorque, 1f / 1000f))
        {
          this.DeSchedule();
        }
        else
        {
          if ((double) this.ResourceSink.SuppliedRatio <= 0.0 || !physics.Enabled && !physics.IsWelded || physics.RigidBody.IsFixed)
            return;
          Matrix inverseInertiaTensor = physics.RigidBody.InverseInertiaTensor;
          inverseInertiaTensor.M44 = 1f;
          MatrixD orientation = grid.PositionComp.WorldMatrixNormalizedInv.GetOrientation();
          Matrix matrix = (Matrix) ref orientation;
          Vector3 vector3_1 = Vector3.Transform(physics.AngularVelocity, ref matrix);
          float num1 = (float) ((1.0 - (double) MyGridGyroSystem.MAX_SLOWDOWN) * (1.0 - (double) this.ResourceSink.SuppliedRatio)) + MyGridGyroSystem.MAX_SLOWDOWN;
          this.SlowdownTorque = -vector3_1;
          float num2 = grid.GridSizeEnum == MyCubeSize.Large ? MyFakes.SLOWDOWN_FACTOR_TORQUE_MULTIPLIER_LARGE_SHIP : MyFakes.SLOWDOWN_FACTOR_TORQUE_MULTIPLIER;
          Vector3 max = new Vector3(this.m_maxGyroForce * num2);
          if (physics.IsWelded)
          {
            this.SlowdownTorque = Vector3.TransformNormal(this.SlowdownTorque, grid.WorldMatrix);
            this.SlowdownTorque = Vector3.TransformNormal(this.SlowdownTorque, Matrix.Invert(physics.RigidBody.GetRigidBodyMatrix()));
          }
          if (!vector3_1.IsValid())
            vector3_1 = Vector3.Zero;
          Vector3 vector3_2 = Vector3.One - Vector3.IsZeroVector(Vector3.Sign(vector3_1) - Vector3.Sign(this.ControlTorque));
          this.SlowdownTorque *= num2;
          this.SlowdownTorque /= inverseInertiaTensor.Scale;
          this.SlowdownTorque = Vector3.Clamp(this.SlowdownTorque, -max, max) * vector3_2;
          if ((double) this.SlowdownTorque.LengthSquared() > 9.99999974737875E-05)
            physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, new Vector3?(), new Vector3D?(), new Vector3?(this.SlowdownTorque * num1), new float?(), true, false);
          Matrix inertiaTensor = MyGridPhysicalGroupData.GetGroupSharedProperties(grid).InertiaTensor;
          float num3 = Math.Max(1f, 1f / Math.Max(Math.Max(inertiaTensor.M11, inertiaTensor.M22), inertiaTensor.M33) * MyGridGyroSystem.INV_TENSOR_MAX_LIMIT);
          this.Torque = Vector3.Clamp(this.ControlTorque, -Vector3.One, Vector3.One) * this.m_maxGyroForce / num3;
          this.Torque *= this.ResourceSink.SuppliedRatio;
          Vector3 vector3_3 = physics.RigidBody.InertiaTensor.Scale;
          vector3_3 = Vector3.Abs(vector3_3 / vector3_3.AbsMax());
          if ((double) this.Torque.LengthSquared() > 9.99999974737875E-05)
          {
            Vector3 normal = this.Torque;
            if (physics.IsWelded)
              normal = Vector3.TransformNormal(Vector3.TransformNormal(normal, grid.WorldMatrix), Matrix.Invert(physics.RigidBody.GetRigidBodyMatrix()));
            physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, new Vector3?(), new Vector3D?(), new Vector3?(normal * vector3_3), new float?(), true, false);
          }
          if (!(this.ControlTorque == Vector3.Zero) || !(physics.AngularVelocity != Vector3.Zero) || ((double) physics.AngularVelocity.LengthSquared() >= 9.00000074466334E-08 || !physics.RigidBody.IsActive))
            return;
          physics.AngularVelocity = Vector3.Zero;
        }
      }
    }

    private void UpdateOverriddenGyros()
    {
      if ((double) this.ResourceSink.SuppliedRatio <= 0.0 || !this.Grid.Physics.Enabled || this.Grid.Physics.RigidBody.IsFixed)
        return;
      MatrixD matrixD = this.Grid.PositionComp.WorldMatrixInvScaled;
      matrixD = matrixD.GetOrientation();
      Matrix matrix1 = (Matrix) ref matrixD;
      matrixD = this.Grid.WorldMatrix;
      matrixD = matrixD.GetOrientation();
      Matrix matrix2 = (Matrix) ref matrixD;
      Vector3 vector3_1 = Vector3.Transform(this.Grid.Physics.AngularVelocity, ref matrix1);
      this.Torque = Vector3.Zero;
      Vector3 velocityDiff = this.m_overrideTargetVelocity - vector3_1;
      if (velocityDiff == Vector3.Zero)
        return;
      this.UpdateOverrideAccelerationRampFrames(velocityDiff);
      Vector3 vector3_2 = velocityDiff * (60f / (float) this.m_overrideAccelerationRampFrames.Value);
      Matrix inverseInertiaTensor = this.Grid.Physics.RigidBody.InverseInertiaTensor;
      Vector3 vector3_3 = new Vector3(inverseInertiaTensor.M11, inverseInertiaTensor.M22, inverseInertiaTensor.M33);
      this.Torque = this.ControlTorque * this.m_maxGyroForce + Vector3.ClampToSphere(vector3_2 / vector3_3, this.m_maxOverrideForce + this.m_maxGyroForce * (1f - this.ControlTorque.Length()));
      this.Torque *= this.ResourceSink.SuppliedRatio;
      if ((double) this.Torque.LengthSquared() < 9.99999974737875E-05)
        return;
      this.Grid.Physics.AddForce(MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE, new Vector3?(), new Vector3D?(), new Vector3?(this.Torque), new float?(), true, false);
    }

    private void UpdateOverrideAccelerationRampFrames(Vector3 velocityDiff)
    {
      if (!this.m_overrideAccelerationRampFrames.HasValue)
      {
        float num = velocityDiff.LengthSquared();
        if ((double) num > 2.46740102767944)
          this.m_overrideAccelerationRampFrames = new int?(120);
        else
          this.m_overrideAccelerationRampFrames = new int?((int) ((double) num * 48.2288856506348) + 1);
      }
      else
      {
        int? accelerationRampFrames = this.m_overrideAccelerationRampFrames;
        int num = 1;
        if (!(accelerationRampFrames.GetValueOrDefault() > num & accelerationRampFrames.HasValue))
          return;
        accelerationRampFrames = this.m_overrideAccelerationRampFrames;
        this.m_overrideAccelerationRampFrames = accelerationRampFrames.HasValue ? new int?(accelerationRampFrames.GetValueOrDefault() - 1) : new int?();
      }
    }

    public Vector3 GetAngularVelocity(Vector3 control)
    {
      if ((double) this.ResourceSink.SuppliedRatio > 0.0 && this.Grid.Physics != null && (this.Grid.Physics.Enabled && !this.Grid.Physics.RigidBody.IsFixed))
      {
        MatrixD matrixD1 = this.Grid.PositionComp.WorldMatrixInvScaled;
        matrixD1 = matrixD1.GetOrientation();
        Matrix matrix1 = (Matrix) ref matrixD1;
        MatrixD matrixD2 = this.Grid.WorldMatrix;
        matrixD2 = matrixD2.GetOrientation();
        Matrix matrix2 = (Matrix) ref matrixD2;
        Vector3 vector3_1 = Vector3.Transform(this.Grid.Physics.AngularVelocity, ref matrix1);
        Matrix inverseInertiaTensor = this.Grid.Physics.RigidBody.InverseInertiaTensor;
        Vector3 vector3_2 = new Vector3(inverseInertiaTensor.M11, inverseInertiaTensor.M22, inverseInertiaTensor.M33);
        float num1 = vector3_2.Min();
        float num2 = Math.Max(1f, num1 * MyGridGyroSystem.INV_TENSOR_MAX_LIMIT);
        Vector3 vector3_3 = Vector3.Zero;
        Vector3 vector3_4 = (this.m_overrideTargetVelocity - vector3_1) * 60f;
        float radius = this.m_maxOverrideForce + this.m_maxGyroForce * (1f - control.Length());
        Vector3 vector3_5 = vector3_4 * Vector3.Normalize(vector3_2);
        Vector3 vector = vector3_5 / vector3_2;
        float num3 = vector.Length() / radius;
        if ((double) num3 < 0.5 && (double) this.m_overrideTargetVelocity.LengthSquared() < 2.49999993684469E-05)
          return this.m_overrideTargetVelocity;
        if (!Vector3.IsZero(vector3_5, 0.0001f))
        {
          float num4 = (float) (1.0 - 0.800000011920929 / Math.Exp(0.5 * (double) num3));
          vector3_3 = Vector3.ClampToSphere(vector, radius) * 0.95f * num4 + vector * 0.05f * (1f - num4);
          if (this.Grid.GridSizeEnum == MyCubeSize.Large)
            vector3_3 *= 2f;
        }
        this.Torque = (control * this.m_maxGyroForce + vector3_3) / num2;
        this.Torque *= this.ResourceSink.SuppliedRatio;
        Vector3 vector3_6 = this.Torque;
        if ((double) vector3_6.LengthSquared() > 9.99999974737875E-05)
        {
          Vector3 vector3_7 = this.Torque * new Vector3(num1) * 0.01666667f;
          return Vector3.Transform(vector3_1 + vector3_7, ref matrix2);
        }
        if (control == Vector3.Zero && this.m_overrideTargetVelocity == Vector3.Zero && this.Grid.Physics.AngularVelocity != Vector3.Zero)
        {
          vector3_6 = this.Grid.Physics.AngularVelocity;
          if ((double) vector3_6.LengthSquared() < 9.00000074466334E-08 && this.Grid.Physics.RigidBody.IsActive)
            return Vector3.Zero;
        }
      }
      return this.Grid.Physics != null ? this.Grid.Physics.AngularVelocity : Vector3.Zero;
    }

    protected override void Update()
    {
      MySimpleProfiler.Begin("Gyro", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (Update));
      if (this.m_gyrosChanged)
        this.RecomputeGyroParameters();
      if ((double) this.m_maxOverrideForce == 0.0)
      {
        if (MyDebugDrawSettings.DEBUG_DRAW_GYROS)
          MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 0.0f), "Old gyros", Color.White, 1f);
        this.UpdateGyros();
        MySimpleProfiler.End(nameof (Update));
      }
      else
      {
        if (MyDebugDrawSettings.DEBUG_DRAW_GYROS)
          MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 0.0f), "New gyros", Color.White, 1f);
        if (this.Grid.Physics != null)
          this.UpdateOverriddenGyros();
        MySimpleProfiler.End(nameof (Update));
      }
    }

    public void DebugDraw()
    {
      double num1 = 4.5;
      double num2 = num1 * 0.045;
      Vector3D translation = this.Grid.WorldMatrix.Translation;
      Vector3D position = MySector.MainCamera.Position;
      Vector3D up = MySector.MainCamera.WorldMatrix.Up;
      Vector3D right = MySector.MainCamera.WorldMatrix.Right;
      double num3 = Math.Atan(num1 / Math.Max(Vector3D.Distance(translation, position), 0.001));
      if (num3 <= 0.270000010728836)
        return;
      MyRenderProxy.DebugDrawText3D(translation, string.Format("Grid {0} Gyro System", (object) this.Grid), Color.Yellow, (float) num3, true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      bool flag1 = (double) this.Torque.LengthSquared() >= 9.99999974737875E-05;
      bool flag2 = (double) this.SlowdownTorque.LengthSquared() > 9.99999974737875E-05;
      bool flag3 = (double) this.m_overrideTargetVelocity.LengthSquared() > 9.99999974737875E-05;
      bool flag4 = this.Grid.Physics != null && (double) this.Grid.Physics.AngularVelocity.LengthSquared() > 9.99999974737875E-06;
      this.DebugDrawText(string.Format("Gyro count: {0}", (object) this.GyroCount), translation + -1.0 * up * num2, right, (float) num3);
      this.DebugDrawText(string.Format("Torque [above threshold - {1}]: {0}", (object) this.Torque, (object) flag1), translation + -2.0 * up * num2, right, (float) num3);
      this.DebugDrawText(string.Format("Slowdown [above threshold - {1}]: {0}", (object) this.SlowdownTorque, (object) flag2), translation + -3.0 * up * num2, right, (float) num3);
      this.DebugDrawText(string.Format("Override [above threshold - {1}]: {0}", (object) this.m_overrideTargetVelocity, (object) flag3), translation + -4.0 * up * num2, right, (float) num3);
      this.DebugDrawText(string.Format("Angular velocity above threshold - {0}", (object) flag4), translation + -5.0 * up * num2, right, (float) num3);
      if (this.Grid.Physics == null)
        return;
      this.DebugDrawText(string.Format("Automatic deactivation enabled - {0}", (object) this.Grid.Physics.RigidBody.EnableDeactivation), translation + -7.0 * up * num2, right, (float) num3);
    }

    private void DebugDrawText(string text, Vector3D origin, Vector3D rightVector, float textSize)
    {
      Vector3D vector3D = 0.0500000007450581 * rightVector;
      Vector3D worldCoord = origin + vector3D + rightVector * 0.0149999996647239;
      MyRenderProxy.DebugDrawLine3D(origin, origin + vector3D, Color.White, Color.White, false);
      string text1 = text;
      Color white = Color.White;
      double num = (double) textSize;
      MyRenderProxy.DebugDrawText3D(worldCoord, text1, white, (float) num, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
    }

    private void RecomputeGyroParameters()
    {
      this.m_gyrosChanged = false;
      double requiredPowerInput = (double) this.m_maxRequiredPowerInput;
      this.m_maxGyroForce = 0.0f;
      this.m_maxOverrideForce = 0.0f;
      this.m_maxRequiredPowerInput = 0.0f;
      this.m_overrideTargetVelocity = Vector3.Zero;
      this.m_overrideAccelerationRampFrames = new int?();
      foreach (MyGyro gyro in this.m_gyros)
      {
        if (this.IsUsed(gyro))
        {
          if (!gyro.GyroOverride || this.AutopilotEnabled)
          {
            this.m_maxGyroForce += gyro.MaxGyroForce;
          }
          else
          {
            this.m_overrideTargetVelocity += gyro.GyroOverrideVelocityGrid * gyro.MaxGyroForce;
            this.m_maxOverrideForce += gyro.MaxGyroForce;
          }
          this.m_maxRequiredPowerInput += gyro.RequiredPowerInput;
        }
      }
      if ((double) this.m_maxOverrideForce != 0.0)
        this.m_overrideTargetVelocity /= this.m_maxOverrideForce;
      this.ResourceSink.MaxRequiredInput = this.m_maxRequiredPowerInput;
      this.ResourceSink.Update();
      this.UpdateAutomaticDeactivation();
    }

    private bool IsUsed(MyGyro gyro) => gyro.Enabled && gyro.IsFunctional;

    private void gyro_EnabledChanged(MyTerminalBlock obj) => this.MarkDirty();

    private void ComponentStack_IsFunctionalChanged() => this.MarkDirty();

    public void MarkDirty()
    {
      this.Schedule();
      this.m_gyrosChanged = true;
    }

    private void Receiver_IsPoweredChanged()
    {
      foreach (MyCubeBlock gyro in this.m_gyros)
        gyro.UpdateIsWorking();
    }

    private void UpdateAutomaticDeactivation()
    {
      if (this.Grid.Physics == null || this.Grid.Physics.RigidBody.IsFixed)
        return;
      if (!Vector3.IsZero(this.m_overrideTargetVelocity) && this.ResourceSink.IsPowered)
        this.Grid.Physics.RigidBody.EnableDeactivation = false;
      else
        this.Grid.Physics.RigidBody.EnableDeactivation = true;
    }

    public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.BeforeSimulation;

    public override int UpdatePriority => 7;

    public override bool UpdateInParallel => true;
  }
}
