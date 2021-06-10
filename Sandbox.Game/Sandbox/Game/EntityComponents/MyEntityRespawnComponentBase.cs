// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyEntityRespawnComponentBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  public abstract class MyEntityRespawnComponentBase : MyEntityComponentBase, IMyCameraController, Sandbox.Game.Entities.IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity
  {
    private static List<MyPhysics.HitInfo> m_raycastList;

    public MyEntity Entity => (MyEntity) base.Entity;

    void IMyCameraController.ControlCamera(MyCamera currentCamera)
    {
      if (MySession.Static.ControlledEntity == null)
      {
        MyThirdPersonSpectator.Static.Update();
        MyThirdPersonSpectator.Static.UpdateZoom();
      }
      if (!MyThirdPersonSpectator.Static.IsCameraForced())
      {
        MyPhysicsComponentBase physics = this.Entity.Physics;
        MatrixD viewMatrix = MyThirdPersonSpectator.Static.GetViewMatrix();
        currentCamera.SetViewMatrix(viewMatrix);
        currentCamera.CameraSpring.Enabled = false;
        currentCamera.CameraSpring.SetCurrentCameraControllerVelocity(physics != null ? physics.LinearVelocity : Vector3.Zero);
      }
      else
      {
        MatrixD matrixD = this.Entity.PositionComp.WorldMatrixRef;
        Vector3D translation = matrixD.Translation;
        Vector3D vector3D = translation + (matrixD.Up + matrixD.Right + matrixD.Forward) * 20.0;
        using (MyUtils.ReuseCollection<MyPhysics.HitInfo>(ref MyEntityRespawnComponentBase.m_raycastList))
        {
          MyPhysics.CastRay(translation, vector3D, MyEntityRespawnComponentBase.m_raycastList);
          float num = 1f;
          foreach (MyPhysics.HitInfo raycast in MyEntityRespawnComponentBase.m_raycastList)
          {
            IMyEntity hitEntity = raycast.HkHitInfo.GetHitEntity();
            if (hitEntity != this.Entity && !(hitEntity is MyFloatingObject) && (!(hitEntity is MyCharacter) && (double) raycast.HkHitInfo.HitFraction < (double) num))
              num = Math.Max(0.1f, raycast.HkHitInfo.HitFraction - 0.1f);
          }
          vector3D = translation + (vector3D - translation) * (double) num;
        }
        MatrixD lookAt = MatrixD.CreateLookAt(vector3D, translation, matrixD.Up);
        currentCamera.SetViewMatrix(lookAt);
      }
    }

    MatrixD VRage.Game.ModAPI.Interfaces.IMyControllableEntity.GetHeadMatrix(
      bool includeY,
      bool includeX,
      bool forceHeadAnim,
      bool forceHeadBone)
    {
      MatrixD matrixD = this.Entity.PositionComp.WorldMatrixRef;
      Vector3D translation = matrixD.Translation;
      return MatrixD.Invert(MatrixD.Normalize(MatrixD.CreateLookAt(translation + matrixD.Right + matrixD.Forward + matrixD.Up, translation, matrixD.Up)));
    }

    public Vector3 LastMotionIndicator => (Vector3) Vector3D.Zero;

    public Vector3 LastRotationIndicator => Vector3.Zero;

    bool IMyCameraController.AllowCubeBuilding => false;

    bool IMyCameraController.ForceFirstPersonCamera
    {
      get => false;
      set
      {
      }
    }

    void IMyCameraController.Rotate(
      Vector2 rotationIndicator,
      float rollIndicator)
    {
    }

    void IMyCameraController.RotateStopped()
    {
    }

    void IMyCameraController.OnAssumeControl(
      IMyCameraController previousCameraController)
    {
    }

    void IMyCameraController.OnReleaseControl(
      IMyCameraController newCameraController)
    {
    }

    bool IMyCameraController.HandleUse() => false;

    bool IMyCameraController.HandlePickUp() => false;

    bool IMyCameraController.IsInFirstPersonView
    {
      get => false;
      set
      {
      }
    }

    bool IMyCameraController.EnableFirstPersonView
    {
      get => false;
      set
      {
      }
    }

    MyEntity Sandbox.Game.Entities.IMyControllableEntity.Entity => this.Entity;

    float Sandbox.Game.Entities.IMyControllableEntity.HeadLocalXAngle
    {
      get => 0.0f;
      set
      {
      }
    }

    float Sandbox.Game.Entities.IMyControllableEntity.HeadLocalYAngle
    {
      get => 0.0f;
      set
      {
      }
    }

    void Sandbox.Game.Entities.IMyControllableEntity.BeginShoot(
      MyShootActionEnum action)
    {
    }

    void Sandbox.Game.Entities.IMyControllableEntity.EndShoot(
      MyShootActionEnum action)
    {
    }

    bool Sandbox.Game.Entities.IMyControllableEntity.ShouldEndShootingOnPause(
      MyShootActionEnum action)
    {
      return true;
    }

    void Sandbox.Game.Entities.IMyControllableEntity.OnBeginShoot(
      MyShootActionEnum action)
    {
    }

    void Sandbox.Game.Entities.IMyControllableEntity.OnEndShoot(
      MyShootActionEnum action)
    {
    }

    void Sandbox.Game.Entities.IMyControllableEntity.UseFinished()
    {
    }

    void Sandbox.Game.Entities.IMyControllableEntity.PickUpFinished()
    {
    }

    void Sandbox.Game.Entities.IMyControllableEntity.Sprint(bool enabled)
    {
    }

    void Sandbox.Game.Entities.IMyControllableEntity.SwitchToWeapon(
      MyDefinitionId weaponDefinition)
    {
    }

    void Sandbox.Game.Entities.IMyControllableEntity.SwitchToWeapon(
      MyToolbarItemWeapon weapon)
    {
    }

    bool Sandbox.Game.Entities.IMyControllableEntity.CanSwitchToWeapon(
      MyDefinitionId? weaponDefinition)
    {
      return false;
    }

    void Sandbox.Game.Entities.IMyControllableEntity.SwitchAmmoMagazine()
    {
    }

    bool Sandbox.Game.Entities.IMyControllableEntity.CanSwitchAmmoMagazine() => false;

    void Sandbox.Game.Entities.IMyControllableEntity.SwitchBroadcasting()
    {
    }

    bool Sandbox.Game.Entities.IMyControllableEntity.EnabledBroadcasting => false;

    MyToolbarType Sandbox.Game.Entities.IMyControllableEntity.ToolbarType => MyToolbarType.None;

    MyEntityCameraSettings Sandbox.Game.Entities.IMyControllableEntity.GetCameraEntitySettings() => new MyEntityCameraSettings();

    MyStringId Sandbox.Game.Entities.IMyControllableEntity.ControlContext => MyStringId.NullOrEmpty;

    public MyStringId AuxiliaryContext => MyStringId.NullOrEmpty;

    MyToolbar Sandbox.Game.Entities.IMyControllableEntity.Toolbar => (MyToolbar) null;

    MyEntity Sandbox.Game.Entities.IMyControllableEntity.RelativeDampeningEntity
    {
      get => (MyEntity) null;
      set
      {
      }
    }

    MyControllerInfo Sandbox.Game.Entities.IMyControllableEntity.ControllerInfo => (MyControllerInfo) null;

    IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => (IMyEntity) this.Entity;

    IMyControllerInfo VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ControllerInfo => (IMyControllerInfo) null;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ForceFirstPersonCamera
    {
      get => false;
      set
      {
      }
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotate(
      Vector3 moveIndicator,
      Vector2 rotationIndicator,
      float rollIndicator)
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotateStopped()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Use()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.UseContinues()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUp()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUpContinues()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Jump(
      Vector3 moveIndicator)
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchWalk()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Up()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Crouch()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Down()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ShowInventory()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ShowTerminal()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchThrusts()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchDamping()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchLights()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchLandingGears()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchHandbrake()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchReactors()
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchHelmet()
    {
    }

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledThrusts => false;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledDamping => false;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledLights => false;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledLeadingGears => false;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledReactors => false;

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledHelmet => false;

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.DrawHud(
      IMyCameraController camera,
      long playerId)
    {
    }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Die()
    {
    }

    bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PrimaryLookaround => false;
  }
}
