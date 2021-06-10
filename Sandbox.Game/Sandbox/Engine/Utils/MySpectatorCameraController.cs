// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MySpectatorCameraController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Lights;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Utils;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
  public class MySpectatorCameraController : MySpectator, IMyCameraController
  {
    private const int REFLECTOR_RANGE_MULTIPLIER = 5;
    public static MySpectatorCameraController Static;
    private float m_orbitY;
    private float m_orbitX;
    private Vector3D ThirdPersonCameraOrbit = Vector3D.UnitZ * 10.0;
    private CyclingOptions m_cycling;
    private float m_cyclingMetricValue = float.MinValue;
    private long m_entityID;
    private MyEntity m_character;
    private double m_yaw;
    private double m_pitch;
    private double m_roll;
    private Vector3D m_lastRightVec = Vector3D.Right;
    private Vector3D m_lastUpVec = Vector3D.Up;
    private MatrixD m_lastOrientation = MatrixD.Identity;
    private float m_lastOrientationWeight = 1f;
    private MyLight m_light;
    private Vector3 m_lightLocalPosition;
    private Vector3D m_velocity;

    public bool IsLightOn => this.m_light != null && this.m_light.LightOn;

    public bool AlignSpectatorToGravity { get; set; }

    public long TrackedEntity { get; set; }

    public MyEntity Entity => (MyEntity) null;

    public MySpectatorCameraController() => MySpectatorCameraController.Static = this;

    public Vector3D Velocity
    {
      get => this.m_velocity;
      set => this.m_velocity = value;
    }

    public override void MoveAndRotate(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      this.UpdateVelocity();
      if (MyInput.Static.IsAnyCtrlKeyPressed())
      {
        if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
          this.SpeedModeAngular = Math.Min(this.SpeedModeAngular * 1.5f, 6f);
        else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
          this.SpeedModeAngular = Math.Max(this.SpeedModeAngular / 1.5f, 0.0001f);
      }
      else if (MyInput.Static.IsAnyShiftKeyPressed() || MyInput.Static.IsAnyAltKeyPressed())
      {
        if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
          this.SpeedModeLinear = Math.Min(this.SpeedModeLinear * 1.5f, 8000f);
        else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
          this.SpeedModeLinear = Math.Max(this.SpeedModeLinear / 1.5f, 0.0001f);
      }
      Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      float num1 = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.SPECTATOR_CHANGE_SPEED_UP);
      float num2 = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.SPECTATOR_CHANGE_SPEED_DOWN);
      float num3 = 1.1f;
      if ((double) num2 > 0.0)
        this.SpeedModeLinear = Math.Min(this.SpeedModeLinear * (float) ((double) num2 * (double) num3 + (1.0 - (double) num2)), 8000f);
      else if ((double) num1 > 0.0)
        this.SpeedModeLinear = Math.Max(this.SpeedModeLinear / (float) ((double) num1 * (double) num3 + (1.0 - (double) num1)), 0.0001f);
      float num4 = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.SPECTATOR_CHANGE_ROTATION_SPEED_UP);
      float num5 = MyControllerHelper.IsControlAnalog(context, MyControlsSpace.SPECTATOR_CHANGE_ROTATION_SPEED_DOWN);
      float num6 = 1.1f;
      if ((double) num4 > 0.0)
        this.SpeedModeAngular = Math.Min(this.SpeedModeAngular * (float) ((double) num4 * (double) num6 + (1.0 - (double) num4)), 6f);
      else if ((double) num5 > 0.0)
        this.SpeedModeAngular = Math.Max(this.SpeedModeAngular / (float) ((double) num5 * (double) num6 + (1.0 - (double) num5)), 0.0001f);
      switch (this.SpectatorCameraMovement)
      {
        case MySpectatorCameraMovementEnum.UserControlled:
          this.MoveAndRotate_UserControlled(moveIndicator, rotationIndicator, rollIndicator);
          if (!this.IsLightOn)
            break;
          this.UpdateLightPosition();
          break;
        case MySpectatorCameraMovementEnum.ConstantDelta:
          this.MoveAndRotate_ConstantDelta(moveIndicator, rotationIndicator, rollIndicator);
          if (!this.IsLightOn)
            break;
          this.UpdateLightPosition();
          break;
        case MySpectatorCameraMovementEnum.FreeMouse:
          this.MoveAndRotate_FreeMouse(moveIndicator, rotationIndicator, rollIndicator);
          break;
        case MySpectatorCameraMovementEnum.Orbit:
          base.MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
          break;
      }
    }

    public override void Update()
    {
      base.Update();
      this.Position = this.Position + this.m_velocity * 0.0166666675359011;
    }

    private void UpdateVelocity()
    {
      if (!MyInput.Static.IsAnyShiftKeyPressed())
        return;
      if (MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Middle))
        this.TryParentSpectator();
      if (MyInput.Static.IsMousePressed(MyMouseButtonsEnum.Right))
        this.m_velocity = Vector3D.Zero;
      if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
      {
        this.m_velocity *= 1.10000002384186;
      }
      else
      {
        if (MyInput.Static.PreviousMouseScrollWheelValue() <= MyInput.Static.MouseScrollWheelValue())
          return;
        this.m_velocity /= 1.10000002384186;
      }
    }

    private void TryParentSpectator()
    {
      MyCamera mainCamera = MySector.MainCamera;
      List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
      MyPhysics.CastRay(this.Position, this.Position + this.Orientation.Forward * 1000.0, toList);
      IMyEntity myEntity = toList.Count <= 0 ? (IMyEntity) null : toList[0].HkHitInfo.Body.GetEntity(toList[0].HkHitInfo.GetShapeKey(0));
      if (myEntity != null)
        this.m_velocity = (Vector3D) myEntity.Physics.LinearVelocity;
      else
        this.m_velocity = Vector3D.Zero;
    }

    private void MoveAndRotate_UserControlled(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      float num1 = 1.666667f;
      float num2 = 1f / 400f * this.m_speedModeAngular;
      rollIndicator = MyInput.Static.GetDeveloperRoll();
      float angle = 0.0f;
      if ((double) rollIndicator != 0.0)
      {
        angle = MathHelper.Clamp((float) ((double) rollIndicator * (double) this.m_speedModeAngular * 0.100000001490116), -0.02f, 0.02f);
        Vector3D xOut;
        Vector3D yOut;
        MyUtils.VectorPlaneRotation(this.m_orientation.Up, this.m_orientation.Right, out xOut, out yOut, angle);
        this.m_orientation.Right = yOut;
        this.m_orientation.Up = xOut;
      }
      if (this.AlignSpectatorToGravity)
      {
        rotationIndicator.Rotate(this.m_roll);
        this.m_yaw -= (double) rotationIndicator.Y * (double) num2;
        this.m_pitch -= (double) rotationIndicator.X * (double) num2;
        this.m_roll -= (double) angle;
        MathHelper.LimitRadians2PI(ref this.m_yaw);
        this.m_pitch = MathHelper.Clamp(this.m_pitch, -1.0 * Math.PI / 2.0, Math.PI / 2.0);
        MathHelper.LimitRadians2PI(ref this.m_roll);
        this.ComputeGravityAlignedOrientation(out this.m_orientation);
      }
      else
      {
        if ((double) this.m_lastOrientationWeight < 1.0)
        {
          this.m_orientation = MatrixD.Orthogonalize(this.m_orientation);
          this.m_orientation.Forward = Vector3D.Cross(this.m_orientation.Up, this.m_orientation.Right);
        }
        if ((double) rotationIndicator.Y != 0.0)
        {
          Vector3D xOut;
          Vector3D yOut;
          MyUtils.VectorPlaneRotation(this.m_orientation.Right, this.m_orientation.Forward, out xOut, out yOut, -rotationIndicator.Y * num2);
          this.m_orientation.Right = xOut;
          this.m_orientation.Forward = yOut;
        }
        if ((double) rotationIndicator.X != 0.0)
        {
          Vector3D xOut;
          Vector3D yOut;
          MyUtils.VectorPlaneRotation(this.m_orientation.Up, this.m_orientation.Forward, out xOut, out yOut, rotationIndicator.X * num2);
          this.m_orientation.Up = xOut;
          this.m_orientation.Forward = yOut;
        }
        this.m_lastOrientation = this.m_orientation;
        this.m_lastOrientationWeight = 1f;
        this.m_roll = 0.0;
        this.m_pitch = 0.0;
      }
      Sandbox.Game.Entities.IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = MySpaceBindingCreator.CX_SPECTATOR;
      if (!MySession.Static.IsCameraUserControlledSpectator())
        context = controlledEntity != null ? controlledEntity.ControlContext : MySpaceBindingCreator.CX_BASE;
      float num3 = (float) ((MyInput.Static.IsAnyShiftKeyPressed() || MyControllerHelper.IsControl(context, MyControlsSpace.SPECTATOR_SPEED_BOOST, MyControlStateType.PRESSED) ? 1.0 : 0.349999994039536) * (MyInput.Static.IsAnyCtrlKeyPressed() ? 0.300000011920929 : 1.0));
      moveIndicator *= num3 * this.SpeedModeLinear;
      this.Position = this.Position + Vector3.Transform(moveIndicator * num1, this.m_orientation);
    }

    private void ComputeGravityAlignedOrientation(out MatrixD resultOrientationStorage)
    {
      bool flag = true;
      Vector3D vector1 = (Vector3D) -MyGravityProviderSystem.CalculateTotalGravityInPoint(this.Position);
      if (vector1.LengthSquared() < 9.99999974737875E-06)
      {
        vector1 = this.m_lastUpVec;
        this.m_lastOrientationWeight = 1f;
        flag = false;
      }
      else
        this.m_lastUpVec = vector1;
      vector1.Normalize();
      Vector3D vector2 = this.m_lastRightVec - Vector3D.Dot(this.m_lastRightVec, vector1) * vector1;
      if (vector2.LengthSquared() < 9.99999974737875E-06)
      {
        vector2 = this.m_orientation.Right - Vector3D.Dot(this.m_orientation.Right, vector1) * vector1;
        if (vector2.LengthSquared() < 9.99999974737875E-06)
          vector2 = this.m_orientation.Forward - Vector3D.Dot(this.m_orientation.Forward, vector1) * vector1;
      }
      vector2.Normalize();
      this.m_lastRightVec = vector2;
      Vector3D result;
      Vector3D.Cross(ref vector1, ref vector2, out result);
      resultOrientationStorage = MatrixD.Identity;
      resultOrientationStorage.Right = vector2;
      resultOrientationStorage.Up = vector1;
      resultOrientationStorage.Forward = result;
      resultOrientationStorage = MatrixD.CreateFromAxisAngle(Vector3D.Right, this.m_pitch) * resultOrientationStorage * MatrixD.CreateFromAxisAngle(vector1, this.m_yaw);
      Vector3D up = resultOrientationStorage.Up;
      vector2 = resultOrientationStorage.Right;
      resultOrientationStorage.Right = Math.Cos(this.m_roll) * vector2 + Math.Sin(this.m_roll) * up;
      resultOrientationStorage.Up = -Math.Sin(this.m_roll) * vector2 + Math.Cos(this.m_roll) * up;
      if (flag && (double) this.m_lastOrientationWeight > 0.0)
      {
        this.m_lastOrientationWeight = Math.Max(0.0f, this.m_lastOrientationWeight - 0.01666667f);
        resultOrientationStorage = MatrixD.Slerp(resultOrientationStorage, this.m_lastOrientation, MathHelper.SmoothStepStable(this.m_lastOrientationWeight));
        resultOrientationStorage = MatrixD.Orthogonalize(resultOrientationStorage);
        resultOrientationStorage.Forward = Vector3D.Cross(resultOrientationStorage.Up, resultOrientationStorage.Right);
      }
      if (flag)
        return;
      this.m_lastOrientation = resultOrientationStorage;
    }

    private void MoveAndRotate_ConstantDelta(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      this.m_cycling.Enabled = true;
      bool flag = false;
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.TOOLBAR_UP) && MySession.Static.IsUserAdmin(Sync.MyId))
      {
        MyEntityCycling.FindNext(MyEntityCyclingOrder.Characters, ref this.m_cyclingMetricValue, ref this.m_entityID, false, this.m_cycling);
        flag = true;
      }
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.TOOLBAR_DOWN) && MySession.Static.IsUserAdmin(Sync.MyId))
      {
        MyEntityCycling.FindNext(MyEntityCyclingOrder.Characters, ref this.m_cyclingMetricValue, ref this.m_entityID, true, this.m_cycling);
        flag = true;
      }
      if (!MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyShiftKeyPressed())
      {
        if (MyInput.Static.PreviousMouseScrollWheelValue() < MyInput.Static.MouseScrollWheelValue())
          this.ThirdPersonCameraOrbit /= 1.10000002384186;
        else if (MyInput.Static.PreviousMouseScrollWheelValue() > MyInput.Static.MouseScrollWheelValue())
          this.ThirdPersonCameraOrbit *= 1.10000002384186;
      }
      if (flag)
        MyEntities.TryGetEntityById(this.m_entityID, out this.m_character);
      MyEntity entity;
      MyEntities.TryGetEntityById(this.TrackedEntity, out entity);
      if (entity != null)
      {
        Vector3D position = entity.PositionComp.GetPosition();
        if (this.AlignSpectatorToGravity)
        {
          this.m_roll = 0.0;
          this.m_yaw = 0.0;
          this.m_pitch = 0.0;
          MatrixD resultOrientationStorage;
          this.ComputeGravityAlignedOrientation(out resultOrientationStorage);
          this.Position = position + Vector3D.Transform(this.ThirdPersonCameraDelta, resultOrientationStorage);
          this.Target = position;
          this.m_orientation.Up = resultOrientationStorage.Up;
        }
        else
        {
          Vector3D vector3D = Vector3D.Normalize(this.Position - this.Target) * this.ThirdPersonCameraDelta.Length();
          this.Position = position + vector3D;
          this.Target = position;
        }
      }
      if (!MyInput.Static.IsAnyAltKeyPressed() || MyInput.Static.IsAnyCtrlKeyPressed() || MyInput.Static.IsAnyShiftKeyPressed())
        return;
      base.MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
    }

    private void MoveAndRotate_FreeMouse(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      if (MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinition != null || MySessionComponentVoxelHand.Static.Enabled || MyInput.Static.IsRightMousePressed())
        this.MoveAndRotate_UserControlled(moveIndicator, rotationIndicator, rollIndicator);
      else
        this.MoveAndRotate_UserControlled(moveIndicator, Vector2.Zero, rollIndicator);
    }

    protected override void OnChangingMode(
      MySpectatorCameraMovementEnum oldMode,
      MySpectatorCameraMovementEnum newMode)
    {
      if (newMode != MySpectatorCameraMovementEnum.UserControlled || oldMode != MySpectatorCameraMovementEnum.ConstantDelta)
        return;
      MatrixD resultOrientationStorage;
      this.ComputeGravityAlignedOrientation(out resultOrientationStorage);
      this.m_orientation.Up = resultOrientationStorage.Up;
      this.m_orientation.Forward = Vector3D.Normalize(this.Target - this.Position);
      this.m_orientation.Right = Vector3D.Cross(this.m_orientation.Forward, this.m_orientation.Up);
      this.AlignSpectatorToGravity = false;
    }

    void IMyCameraController.ControlCamera(MyCamera currentCamera) => currentCamera.SetViewMatrix(this.GetViewMatrix());

    public void InitLight(bool isLightOn)
    {
      this.m_light = MyLights.AddLight();
      if (this.m_light == null)
        return;
      this.m_light.Start("SpectatorCameraController");
      this.m_light.ReflectorOn = true;
      this.m_light.ReflectorTexture = "Textures\\Lights\\dual_reflector_2.dds";
      this.m_light.Range = 2f;
      this.m_light.ReflectorRange = 35f;
      this.m_light.ReflectorColor = (Color) MyCharacter.REFLECTOR_COLOR;
      this.m_light.ReflectorIntensity = MyCharacter.REFLECTOR_INTENSITY;
      this.m_light.ReflectorGlossFactor = MyCharacter.REFLECTOR_GLOSS_FACTOR;
      this.m_light.ReflectorDiffuseFactor = MyCharacter.REFLECTOR_DIFFUSE_FACTOR;
      this.m_light.Color = (Color) MyCharacter.POINT_COLOR;
      this.m_light.Intensity = MyCharacter.POINT_LIGHT_INTENSITY;
      this.m_light.UpdateReflectorRangeAndAngle(0.373f, 175f);
      this.m_light.LightOn = isLightOn;
      this.m_light.ReflectorOn = isLightOn;
    }

    private void UpdateLightPosition()
    {
      if (this.m_light == null)
        return;
      MatrixD world = MatrixD.CreateWorld(this.Position, this.m_orientation.Forward, this.m_orientation.Up);
      this.m_light.ReflectorDirection = (Vector3) world.Forward;
      this.m_light.ReflectorUp = (Vector3) world.Up;
      this.m_light.Position = this.Position;
      this.m_light.UpdateLight();
    }

    public void SwitchLight()
    {
      if (this.m_light == null)
        return;
      this.m_light.LightOn = !this.m_light.LightOn;
      this.m_light.ReflectorOn = !this.m_light.ReflectorOn;
      this.m_light.UpdateLight();
    }

    public void TurnLightOff()
    {
      if (this.m_light == null)
        return;
      this.m_light.LightOn = false;
      this.m_light.ReflectorOn = false;
      this.m_light.UpdateLight();
    }

    public void CleanLight()
    {
      if (this.m_light == null)
        return;
      MyLights.RemoveLight(this.m_light);
      this.m_light = (MyLight) null;
    }

    void IMyCameraController.Rotate(
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      this.Rotate(rotationIndicator, rollIndicator);
    }

    void IMyCameraController.RotateStopped() => this.RotateStopped();

    public void OnAssumeControl(IMyCameraController previousCameraController)
    {
    }

    public void OnReleaseControl(IMyCameraController newCameraController) => this.TurnLightOff();

    void IMyCameraController.OnAssumeControl(
      IMyCameraController previousCameraController)
    {
      this.OnAssumeControl(previousCameraController);
    }

    void IMyCameraController.OnReleaseControl(
      IMyCameraController newCameraController)
    {
      this.OnReleaseControl(newCameraController);
    }

    bool IMyCameraController.IsInFirstPersonView
    {
      get => this.IsInFirstPersonView;
      set => this.IsInFirstPersonView = value;
    }

    bool IMyCameraController.ForceFirstPersonCamera
    {
      get => this.ForceFirstPersonCamera;
      set => this.ForceFirstPersonCamera = value;
    }

    bool IMyCameraController.EnableFirstPersonView
    {
      get => true;
      set
      {
      }
    }

    bool IMyCameraController.HandleUse() => false;

    bool IMyCameraController.AllowCubeBuilding => true;

    bool IMyCameraController.HandlePickUp() => false;
  }
}
