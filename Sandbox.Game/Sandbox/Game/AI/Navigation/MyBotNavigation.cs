// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Navigation.MyBotNavigation
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Navigation
{
  public class MyBotNavigation
  {
    private readonly List<MySteeringBase> m_steerings;
    private readonly MyPathSteering m_pathSteering;
    private readonly MyBotAiming m_aiming;
    private readonly MyDestinationSphere m_destinationSphere;
    private Vector3 m_forwardVector;
    private Vector3 m_correction;
    private Vector3 m_upVector;
    private bool m_wasStopped;
    private float m_rotationSpeedModifier;
    private float? m_maximumRotationAngle;
    private readonly MyStuckDetection m_stuckDetection;
    private MatrixD m_worldMatrix;
    private MatrixD m_invWorldMatrix;
    private MatrixD m_aimingPositionAndOrientation;
    private MatrixD m_invAimingPositionAndOrientation;
    public int WaitForClearPathCountdown;

    public Vector3 ForwardVector => this.m_forwardVector;

    public Vector3 UpVector => this.m_upVector;

    public float Speed { get; private set; }

    public bool Navigating => this.m_pathSteering.TargetSet;

    public bool IsWaitingForTileGeneration => this.m_pathSteering.IsWaitingForTileGeneration;

    public bool Stuck => this.m_stuckDetection.IsStuck;

    public Vector3D TargetPoint => this.m_destinationSphere.GetDestination();

    public MyEntity BotEntity { get; private set; }

    public float? MaximumRotationAngle
    {
      set => this.m_maximumRotationAngle = value;
      get => this.m_maximumRotationAngle;
    }

    public Vector3 GravityDirection { get; private set; }

    public MatrixD PositionAndOrientation => this.BotEntity == null ? MatrixD.Identity : this.m_worldMatrix;

    public MatrixD PositionAndOrientationInverted => this.BotEntity == null ? MatrixD.Identity : this.m_invWorldMatrix;

    public MatrixD AimingPositionAndOrientation => this.BotEntity == null ? MatrixD.Identity : this.m_aimingPositionAndOrientation;

    public MatrixD AimingPositionAndOrientationInverted => this.BotEntity == null ? MatrixD.Identity : this.m_invAimingPositionAndOrientation;

    public bool HasRotation(float epsilon = 0.0316f) => (double) this.m_aiming.RotationHint.LengthSquared() > (double) epsilon * (double) epsilon;

    public bool HasXRotation(float epsilon) => (double) Math.Abs(this.m_aiming.RotationHint.Y) > (double) epsilon;

    public bool HasYRotation(float epsilon) => (double) Math.Abs(this.m_aiming.RotationHint.X) > (double) epsilon;

    public MyBotNavigation(MyPlayer player)
    {
      this.m_steerings = new List<MySteeringBase>();
      this.m_pathSteering = new MyPathSteering(this);
      this.m_steerings.Add((MySteeringBase) this.m_pathSteering);
      if (player.Identity.Model.Contains("Wolf"))
        this.m_steerings.Add((MySteeringBase) new MyForwardCollisionAvoidanceSteering(this));
      else
        this.m_steerings.Add((MySteeringBase) new MyCharacterAvoidanceSteering(this, 0.06f));
      this.m_aiming = new MyBotAiming(this);
      this.m_stuckDetection = new MyStuckDetection(0.05f, MathHelper.ToRadians(1f));
      this.m_destinationSphere = new MyDestinationSphere(ref Vector3D.Zero, 0.0f);
      this.m_wasStopped = false;
    }

    public void Cleanup()
    {
      foreach (MySteeringBase steering in this.m_steerings)
        steering.Cleanup();
    }

    public void ChangeEntity(IMyControllableEntity newEntity)
    {
      this.BotEntity = newEntity?.Entity;
      if (this.BotEntity == null)
        return;
      this.m_forwardVector = (Vector3) this.PositionAndOrientation.Forward;
      this.m_upVector = (Vector3) this.PositionAndOrientation.Up;
      this.Speed = 0.0f;
      this.m_rotationSpeedModifier = 1f;
    }

    public void Update(int behaviorTicks)
    {
      this.m_stuckDetection.SetCurrentTicks(behaviorTicks);
      if (this.BotEntity == null)
        return;
      this.UpdateMatrices();
      MatrixD matrixD = this.BotEntity.PositionComp.WorldMatrixRef;
      this.GravityDirection = MyGravityProviderSystem.CalculateTotalGravityInPoint(matrixD.Translation);
      if (!Vector3.IsZero(this.GravityDirection, 0.01f))
        this.GravityDirection = (Vector3) Vector3D.Normalize((Vector3D) this.GravityDirection);
      this.m_upVector = !MyPerGameSettings.NavmeshPresumesDownwardGravity ? -this.GravityDirection : Vector3.Up;
      if (!this.Speed.IsValid())
      {
        matrixD = this.PositionAndOrientation;
        this.m_forwardVector = (Vector3) matrixD.Forward;
        this.Speed = 0.0f;
        this.m_rotationSpeedModifier = 1f;
      }
      foreach (MySteeringBase steering in this.m_steerings)
        steering.Update();
      this.m_aiming.Update();
      this.CorrectMovement(this.m_aiming.RotationHint);
      if ((double) this.Speed < 0.100000001490116)
        this.Speed = 0.0f;
      this.MoveCharacter();
    }

    private void UpdateMatrices()
    {
      if (this.BotEntity is MyCharacter botEntity)
      {
        this.m_worldMatrix = botEntity.WorldMatrix;
        Matrix matrix = Matrix.Invert((Matrix) ref this.m_worldMatrix);
        this.m_invWorldMatrix = (MatrixD) ref matrix;
        this.m_aimingPositionAndOrientation = botEntity.GetHeadMatrix(true, true, false, true, false);
        this.m_invAimingPositionAndOrientation = MatrixD.Invert(this.m_aimingPositionAndOrientation);
      }
      else
      {
        this.m_worldMatrix = this.BotEntity.PositionComp.WorldMatrixRef;
        this.m_invWorldMatrix = this.BotEntity.PositionComp.WorldMatrixInvScaled;
        this.m_aimingPositionAndOrientation = this.m_worldMatrix;
        this.m_invAimingPositionAndOrientation = this.m_invWorldMatrix;
      }
    }

    private void AccumulateCorrection()
    {
      this.m_rotationSpeedModifier = 1f;
      float weight = 0.0f;
      for (int index = 0; index < this.m_steerings.Count; ++index)
        this.m_steerings[index].AccumulateCorrection(ref this.m_correction, ref weight);
      if (this.m_maximumRotationAngle.HasValue)
      {
        double num1 = Math.Cos((double) this.m_maximumRotationAngle.Value);
        double num2 = Vector3D.Dot(Vector3D.Normalize((Vector3D) (this.m_forwardVector - this.m_correction)), this.m_forwardVector);
        if (num2 < num1)
        {
          this.m_rotationSpeedModifier = (float) Math.Acos(MathHelper.Clamp(num2, -1.0, 1.0)) / this.m_maximumRotationAngle.Value;
          this.m_correction /= this.m_rotationSpeedModifier;
        }
      }
      if ((double) weight <= 1.0)
        return;
      this.m_correction /= weight;
    }

    private void CorrectMovement(Vector3 rotationHint)
    {
      this.m_correction = Vector3.Zero;
      if (!this.Navigating)
      {
        this.Speed = 0.0f;
      }
      else
      {
        this.AccumulateCorrection();
        if (this.HasRotation(10f))
        {
          this.m_correction = Vector3.Zero;
          this.Speed = 0.0f;
          this.m_stuckDetection.SetRotating(true);
        }
        else
          this.m_stuckDetection.SetRotating(false);
        Vector3 vector3 = this.m_forwardVector * this.Speed + this.m_correction;
        this.Speed = vector3.Length();
        if ((double) this.Speed <= 1.0 / 1000.0)
        {
          this.Speed = 0.0f;
        }
        else
        {
          this.m_forwardVector = vector3 / this.Speed;
          if ((double) this.Speed <= 1.0)
            return;
          this.Speed = 1f;
        }
      }
    }

    private void MoveCharacter()
    {
      if (this.WaitForClearPathCountdown > 0)
      {
        --this.WaitForClearPathCountdown;
        this.Speed = 0.0f;
      }
      if (this.BotEntity is MyCharacter botEntity)
      {
        if ((double) this.Speed != 0.0)
        {
          MyCharacterJetpackComponent jetpackComp = botEntity.JetpackComp;
          if (jetpackComp != null && !jetpackComp.TurnedOn && this.m_pathSteering.Flying)
            jetpackComp.TurnOnJetpack(true);
          else if (jetpackComp != null && jetpackComp.TurnedOn && !this.m_pathSteering.Flying)
            jetpackComp.TurnOnJetpack(false);
          Vector3 vector3_1 = Vector3.TransformNormal(this.m_forwardVector, botEntity.PositionComp.WorldMatrixNormalizedInv);
          Vector3 vector3_2 = this.m_aiming.RotationHint * this.m_rotationSpeedModifier;
          if (this.m_pathSteering.Flying)
          {
            if ((double) vector3_1.Y > 0.0)
              botEntity.Up();
            else
              botEntity.Down();
          }
          if ((double) vector3_1.Y <= 0.699999988079071)
            botEntity.MoveAndRotate(vector3_1 * this.Speed, new Vector2(vector3_2.X * 30f, vector3_2.Y * 30f), 0.0f);
        }
        else if ((double) this.Speed == 0.0)
        {
          if (this.HasRotation())
          {
            float num = botEntity.WantsWalk || botEntity.IsCrouching ? 1f : 2f;
            Vector3 vector3 = this.m_aiming.RotationHint * this.m_rotationSpeedModifier;
            botEntity.MoveAndRotate(Vector3.Zero, new Vector2(vector3.X * 20f * num, vector3.Y * 25f * num), 0.0f);
            this.m_wasStopped = false;
          }
          else if (this.m_wasStopped)
          {
            botEntity.MoveAndRotate(Vector3.Zero, Vector2.Zero, 0.0f);
            this.m_wasStopped = true;
          }
        }
      }
      if (this.WaitForClearPathCountdown > 0)
        return;
      this.m_stuckDetection.Update(this.m_worldMatrix.Translation, this.m_aiming.RotationHint);
    }

    public void AddSteering(MySteeringBase steering) => this.m_steerings.Add(steering);

    public void RemoveSteering(MySteeringBase steering) => this.m_steerings.Remove(steering);

    public bool HasSteeringOfType(Type steeringType)
    {
      foreach (object steering in this.m_steerings)
      {
        if (steering.GetType() == steeringType)
          return true;
      }
      return false;
    }

    public MySteeringBase GetSteeringOfType(Type steeringType)
    {
      foreach (MySteeringBase steering in this.m_steerings)
      {
        if (steering.GetType() == steeringType)
          return steering;
      }
      return (MySteeringBase) null;
    }

    public void Goto(Vector3D position, float radius = 0.0f, MyEntity relativeEntity = null)
    {
      this.m_destinationSphere.Init(ref position, radius);
      this.Goto((IMyDestinationShape) this.m_destinationSphere, relativeEntity);
    }

    public void Goto(IMyDestinationShape destination, MyEntity relativeEntity = null)
    {
      if (MyAIComponent.Static.Pathfinding == null)
        return;
      IMyPath pathGlobal = MyAIComponent.Static.Pathfinding.FindPathGlobal(this.PositionAndOrientation.Translation, destination, relativeEntity);
      if (pathGlobal == null)
      {
        this.m_pathSteering.UnsetPath();
      }
      else
      {
        this.m_pathSteering.SetPath(pathGlobal);
        this.m_stuckDetection.Reset();
      }
    }

    public void GotoNoPath(
      Vector3D worldPosition,
      float radius = 0.0f,
      MyEntity relativeEntity = null,
      bool resetStuckDetection = true)
    {
      this.m_pathSteering.SetTarget(worldPosition, radius, relativeEntity);
      if (!resetStuckDetection)
        return;
      this.m_stuckDetection.Reset();
    }

    public bool CheckReachability(Vector3D worldPosition, float threshold, MyEntity relativeEntity = null)
    {
      if (MyAIComponent.Static.Pathfinding == null)
        return false;
      this.m_destinationSphere.Init(ref worldPosition, 0.0f);
      return MyAIComponent.Static.Pathfinding.ReachableUnderThreshold(this.PositionAndOrientation.Translation, (IMyDestinationShape) this.m_destinationSphere, threshold);
    }

    public void FlyTo(Vector3D worldPosition, MyEntity relativeEntity = null)
    {
      this.m_pathSteering.SetTarget(worldPosition, relativeEntity: relativeEntity, fly: true);
      this.m_stuckDetection.Reset();
    }

    public void Stop()
    {
      if (!this.IsWaitingForTileGeneration)
        this.m_pathSteering.UnsetPath();
      this.m_stuckDetection.Stop();
    }

    public void StopImmediate(bool forceUpdate = false)
    {
      this.Stop();
      this.Speed = 0.0f;
      if (!forceUpdate)
        return;
      this.MoveCharacter();
    }

    public void FollowPath(IMyPath path)
    {
      this.m_pathSteering.SetPath(path);
      this.m_stuckDetection.Reset();
    }

    public void AimAt(MyEntity entity, Vector3D? worldPosition = null)
    {
      if (worldPosition.HasValue)
      {
        if (entity != null)
        {
          MatrixD matrix = entity.PositionComp.WorldMatrixNormalizedInv;
          Vector3 vector3 = (Vector3) Vector3D.Transform(worldPosition.Value, matrix);
          this.m_aiming.SetTarget(entity, new Vector3?(vector3));
        }
        else
          this.m_aiming.SetAbsoluteTarget((Vector3) worldPosition.Value);
      }
      else
        this.m_aiming.SetTarget(entity);
    }

    public void AimWithMovement() => this.m_aiming.FollowMovement();

    public void StopAiming() => this.m_aiming.StopAiming();

    [Conditional("DEBUG")]
    private void AssertIsValid()
    {
    }

    [Conditional("DEBUG")]
    public void DebugDraw()
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        return;
      this.m_aiming.DebugDraw(this.m_aimingPositionAndOrientation);
      if (MyDebugDrawSettings.DEBUG_DRAW_BOT_STEERING)
      {
        foreach (MySteeringBase steering in this.m_steerings)
          ;
      }
      if (!MyDebugDrawSettings.DEBUG_DRAW_BOT_NAVIGATION)
        return;
      Vector3 translation1 = (Vector3) this.PositionAndOrientation.Translation;
      Vector3.Cross(this.m_forwardVector, this.UpVector);
      if (this.Stuck)
        MyRenderProxy.DebugDrawSphere((Vector3D) translation1, 1f, (Color) Color.Red.ToVector3(), depthRead: false);
      MyRenderProxy.DebugDrawArrow3D((Vector3D) translation1, (Vector3D) (translation1 + this.ForwardVector), Color.Blue, new Color?(Color.Blue), text: "Nav. FW");
      MyRenderProxy.DebugDrawArrow3D((Vector3D) (translation1 + this.ForwardVector), (Vector3D) (translation1 + this.ForwardVector + this.m_correction), Color.LightBlue, new Color?(Color.LightBlue), text: "Correction");
      this.m_destinationSphere?.DebugDraw();
      if (!(this.BotEntity is MyCharacter botEntity))
        return;
      MatrixD matrix = MatrixD.Invert(botEntity.GetViewMatrix());
      MatrixD headMatrix = botEntity.GetHeadMatrix(true, true, false, false, false);
      MyRenderProxy.DebugDrawLine3D(matrix.Translation, Vector3D.Transform(Vector3D.Forward * 50.0, matrix), Color.Yellow, Color.White, false);
      MyRenderProxy.DebugDrawLine3D(headMatrix.Translation, Vector3D.Transform(Vector3D.Forward * 50.0, headMatrix), Color.Red, Color.Red, false);
      if (botEntity.CurrentWeapon == null)
        return;
      Vector3 target = botEntity.CurrentWeapon.DirectionToTarget(botEntity.AimedPoint);
      Vector3D translation2 = (botEntity.CurrentWeapon as MyEntity).WorldMatrix.Translation;
      MyRenderProxy.DebugDrawSphere(botEntity.AimedPoint, 1f, Color.Yellow, depthRead: false);
      MyRenderProxy.DebugDrawLine3D(translation2, translation2 + target * 20f, Color.Purple, Color.Purple, false);
    }
  }
}
