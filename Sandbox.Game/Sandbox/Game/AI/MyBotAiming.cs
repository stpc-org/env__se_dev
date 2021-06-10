// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.MyBotAiming
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.AI.Navigation;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Weapons;
using System;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI
{
  public class MyBotAiming
  {
    public const float MISSING_PROBABILITY = 0.3f;
    private readonly MyBotNavigation m_parent;
    private MyBotAiming.AimingMode m_mode;
    private MyEntity m_aimTarget;
    private Vector3 m_rotationHint;
    private Vector3? m_relativeTarget;
    private Vector3 m_dbgDesiredForward;

    public Vector3 RotationHint => this.m_rotationHint;

    public MyBotAiming(MyBotNavigation parent)
    {
      this.m_parent = parent;
      this.m_mode = MyBotAiming.AimingMode.FOLLOW_MOVEMENT;
      this.m_rotationHint = Vector3.Zero;
    }

    public void SetTarget(MyEntity entity, Vector3? relativeTarget = null)
    {
      this.m_mode = MyBotAiming.AimingMode.TARGET;
      this.m_aimTarget = entity;
      this.m_relativeTarget = relativeTarget;
      this.Update();
    }

    public void SetAbsoluteTarget(Vector3 absoluteTarget)
    {
      this.m_mode = MyBotAiming.AimingMode.TARGET;
      this.m_aimTarget = (MyEntity) null;
      this.m_relativeTarget = new Vector3?(absoluteTarget);
      this.Update();
    }

    public void FollowMovement()
    {
      this.m_aimTarget = (MyEntity) null;
      this.m_mode = MyBotAiming.AimingMode.FOLLOW_MOVEMENT;
      this.m_relativeTarget = new Vector3?();
    }

    public void StopAiming()
    {
      this.m_aimTarget = (MyEntity) null;
      this.m_mode = MyBotAiming.AimingMode.FIXATED;
      this.m_relativeTarget = new Vector3?();
    }

    public void Update()
    {
      if (this.m_mode == MyBotAiming.AimingMode.FIXATED)
      {
        this.m_rotationHint = Vector3.Zero;
      }
      else
      {
        MyCharacter botEntity = this.m_parent.BotEntity as MyCharacter;
        MatrixD positionAndOrientation = this.m_parent.AimingPositionAndOrientation;
        if (this.m_mode == MyBotAiming.AimingMode.FOLLOW_MOVEMENT)
        {
          Vector3 forwardVector = this.m_parent.ForwardVector;
          this.CalculateRotationHint(ref positionAndOrientation, ref forwardVector);
        }
        else if (this.m_aimTarget != null)
        {
          if (this.m_aimTarget.MarkedForClose)
          {
            this.m_aimTarget = (MyEntity) null;
            this.m_rotationHint = Vector3.Zero;
          }
          else
          {
            Vector3 transformedRelativeTarget;
            MatrixD matrixD;
            if (this.m_relativeTarget.HasValue)
            {
              transformedRelativeTarget = (Vector3) Vector3D.Transform(this.m_relativeTarget.Value, this.m_aimTarget.PositionComp.WorldMatrixRef);
            }
            else
            {
              matrixD = this.m_aimTarget.PositionComp.WorldMatrixRef;
              transformedRelativeTarget = (Vector3) matrixD.Translation;
            }
            this.PredictTargetPosition(ref transformedRelativeTarget, botEntity);
            Vector3 vector3 = transformedRelativeTarget;
            matrixD = this.m_parent.AimingPositionAndOrientation;
            Vector3D translation = matrixD.Translation;
            Vector3 desiredForward = (Vector3) (vector3 - translation);
            double num1 = (double) desiredForward.Normalize();
            this.CalculateRotationHint(ref positionAndOrientation, ref desiredForward);
            if (botEntity == null)
              return;
            botEntity.AimedPoint = (Vector3D) transformedRelativeTarget;
            MyCharacter myCharacter = botEntity;
            MyPositionComponentBase positionComp = this.m_aimTarget.PositionComp;
            double num2 = positionComp != null ? (double) positionComp.LocalVolume.Radius * 1.5 : 1.0;
            MyBotAiming.AddErrorToAiming((IMyCharacter) myCharacter, (float) num2);
          }
        }
        else if (this.m_relativeTarget.HasValue)
        {
          Vector3 desiredForward = (Vector3) (this.m_relativeTarget.Value - this.m_parent.AimingPositionAndOrientation.Translation);
          double num = (double) desiredForward.Normalize();
          this.CalculateRotationHint(ref positionAndOrientation, ref desiredForward);
          if (botEntity == null)
            return;
          botEntity.AimedPoint = (Vector3D) this.m_relativeTarget.Value;
        }
        else
          this.m_rotationHint = Vector3.Zero;
      }
    }

    private static void AddErrorToAiming(IMyCharacter character, float errorLength)
    {
      if ((double) MyUtils.GetRandomFloat() >= 0.300000011920929)
        return;
      character.AimedPoint += Vector3D.Normalize((Vector3D) MyUtils.GetRandomVector3()) * (double) errorLength;
    }

    private void PredictTargetPosition(ref Vector3 transformedRelativeTarget, MyCharacter bot)
    {
      if (bot?.CurrentWeapon == null || !(bot.CurrentWeapon.GunBase is MyGunBase gunBase))
        return;
      MyWeaponPrediction.GetPredictedTargetPosition(gunBase, (MyEntity) bot, this.m_aimTarget, out transformedRelativeTarget, out float _, 0.1666667f);
    }

    private void CalculateRotationHint(ref MatrixD parentMatrix, ref Vector3 desiredForward)
    {
      Vector3D upVector = (Vector3D) this.m_parent.UpVector;
      if ((double) desiredForward.LengthSquared() == 0.0)
      {
        this.m_rotationHint.X = this.m_rotationHint.Y = 0.0f;
      }
      else
      {
        Vector3D vector2_1 = Vector3D.Reject((Vector3D) desiredForward, parentMatrix.Up);
        Vector3D vector2_2 = Vector3D.Reject((Vector3D) desiredForward, parentMatrix.Right);
        vector2_1.Normalize();
        vector2_2.Normalize();
        this.m_dbgDesiredForward = desiredForward;
        double num1 = Vector3D.Dot(parentMatrix.Forward, upVector);
        double num2 = Vector3D.Dot((Vector3D) desiredForward, upVector);
        double num3 = Math.Acos(MathHelper.Clamp(Vector3D.Dot(parentMatrix.Forward, vector2_2), -1.0, 1.0));
        double num4 = num1;
        if (num2 > num4)
          num3 = -num3;
        double num5 = Math.Acos(MathHelper.Clamp(Vector3D.Dot(parentMatrix.Forward, vector2_1), -1.0, 1.0));
        if (Vector3D.Dot(parentMatrix.Right, vector2_1) < 0.0)
          num5 = -num5;
        this.m_rotationHint.X = MathHelper.Clamp((float) num3, -3f, 3f);
        this.m_rotationHint.Y = MathHelper.Clamp((float) num5, -3f, 3f);
      }
    }

    public void DebugDraw(MatrixD posAndOri)
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_BOT_AIMING)
        return;
      Vector3 translation = (Vector3) posAndOri.Translation;
      MyRenderProxy.DebugDrawArrow3D((Vector3D) translation, translation + posAndOri.Right, Color.Red, new Color?(Color.Red), text: "X");
      MyRenderProxy.DebugDrawArrow3D((Vector3D) translation, translation + posAndOri.Up, Color.Green, new Color?(Color.Green), text: "Y");
      MyRenderProxy.DebugDrawArrow3D((Vector3D) translation, translation + posAndOri.Forward, Color.Blue, new Color?(Color.Blue), text: "-Z");
      MyRenderProxy.DebugDrawArrow3D((Vector3D) translation, (Vector3D) (translation + this.m_dbgDesiredForward), Color.Yellow, new Color?(Color.Yellow), text: "Des.-Z");
      Vector3D pointFrom = translation + posAndOri.Forward;
      MyRenderProxy.DebugDrawArrow3D(pointFrom, pointFrom + (double) this.m_rotationHint.X * 10.0 * posAndOri.Right, Color.Salmon, new Color?(Color.Salmon), text: "Rot.X");
      MyRenderProxy.DebugDrawArrow3D(pointFrom, pointFrom - (double) this.m_rotationHint.Y * 10.0 * posAndOri.Up, Color.LimeGreen, new Color?(Color.LimeGreen), text: "Rot.Y");
      if (!(this.m_parent.BotEntity is MyCharacter botEntity))
        return;
      MyRenderProxy.DebugDrawSphere(botEntity.AimedPoint, 0.2f, Color.Orange, depthRead: false);
    }

    private enum AimingMode : byte
    {
      FIXATED,
      TARGET,
      FOLLOW_MOVEMENT,
    }
  }
}
