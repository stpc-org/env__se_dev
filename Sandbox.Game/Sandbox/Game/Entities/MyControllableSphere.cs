// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyControllableSphere
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  internal class MyControllableSphere : MyEntity, IMyCameraController, IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity
  {
    private MyControllerInfo m_info = new MyControllerInfo();
    private MyToolbar m_toolbar;

    public MyControllerInfo ControllerInfo => this.m_info;

    public MyControllableSphere()
    {
      this.ControllerInfo.ControlAcquired += new Action<MyEntityController>(this.OnControlAcquired);
      this.ControllerInfo.ControlReleased += new Action<MyEntityController>(this.OnControlReleased);
      this.m_toolbar = new MyToolbar(this.ToolbarType);
    }

    public void Init()
    {
      this.Init((StringBuilder) null, "Models\\Debug\\Sphere", (MyEntity) null, new float?());
      this.WorldMatrix = MatrixD.Identity;
      this.InitSpherePhysics(MyMaterialType.METAL, Vector3.Zero, 0.5f, 100f, MyPerGameSettings.DefaultLinearDamping, MyPerGameSettings.DefaultAngularDamping, (ushort) 15, RigidBodyFlag.RBF_DEFAULT);
      this.Render.SkipIfTooSmall = false;
      this.Save = false;
    }

    public bool IsInFirstPersonView { get; set; }

    public void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float roll)
    {
      float num1 = 0.1f;
      MatrixD worldMatrix = this.WorldMatrix;
      Vector3D translation = worldMatrix.Translation;
      double num2 = (double) num1;
      worldMatrix = this.WorldMatrix;
      Vector3D right = worldMatrix.Right;
      Vector3D vector3D1 = num2 * right * (double) moveIndicator.X;
      Vector3D vector3D2 = translation + vector3D1;
      double num3 = (double) num1;
      worldMatrix = this.WorldMatrix;
      Vector3D up = worldMatrix.Up;
      Vector3D vector3D3 = num3 * up * (double) moveIndicator.Y;
      Vector3D vector3D4 = vector3D2 + vector3D3;
      double num4 = (double) num1;
      worldMatrix = this.WorldMatrix;
      Vector3D forward = worldMatrix.Forward;
      Vector3D vector3D5 = num4 * forward * (double) moveIndicator.Z;
      Vector3D vector3D6 = vector3D4 - vector3D5;
      Matrix rotation = this.GetRotation(rotationIndicator, roll);
      MatrixD matrixD = (MatrixD) ref rotation * this.WorldMatrix;
      matrixD.Translation = vector3D6;
      this.WorldMatrix = matrixD;
      this.LastMotionIndicator = moveIndicator;
      this.LastRotationIndicator = new Vector3(rotationIndicator, roll);
    }

    public void OnAssumeControl(IMyCameraController previousCameraController)
    {
    }

    public void OnReleaseControl(IMyCameraController newCameraController)
    {
    }

    public void MoveAndRotateStopped()
    {
    }

    public void Rotate(Vector2 rotationIndicator, float roll)
    {
      Matrix rotation = this.GetRotation(rotationIndicator, roll);
      this.WorldMatrix = (MatrixD) ref rotation * this.WorldMatrix;
    }

    public void RotateStopped()
    {
    }

    private Matrix GetRotation(Vector2 rotationIndicator, float roll)
    {
      float num = 1f / 1000f;
      return Matrix.CreateRotationY(-num * rotationIndicator.Y) * Matrix.CreateRotationX(-num * rotationIndicator.X) * Matrix.CreateRotationZ((float) (-(double) num * (double) roll * 10.0));
    }

    public void BeginShoot(MyShootActionEnum action)
    {
    }

    public void OnBeginShoot(MyShootActionEnum action)
    {
    }

    private void ShootInternal()
    {
    }

    private void ShootFailedLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    private void ShootBeginFailed(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    private void ShootSuccessfulLocal(MyShootActionEnum action)
    {
    }

    public void SwitchOnEndShoot(MyDefinitionId? weaponDefinition)
    {
    }

    private void EndShootAll()
    {
    }

    public void EndShoot(MyShootActionEnum action)
    {
    }

    public void OnEndShoot(MyShootActionEnum action)
    {
    }

    public void Zoom(bool newKeyPress)
    {
    }

    private void EnableIronsight(
      bool enable,
      bool newKeyPress,
      bool changeCamera,
      bool updateSync = true)
    {
    }

    public void Use()
    {
    }

    public void UseContinues()
    {
    }

    public void UseFinished()
    {
    }

    public void PickUp()
    {
    }

    public void PickUpContinues()
    {
    }

    public void PickUpFinished()
    {
    }

    public void Crouch()
    {
    }

    public void Down()
    {
    }

    public void Up()
    {
    }

    public void Jump(Vector3 moveIndicator)
    {
    }

    public void SwitchWalk()
    {
    }

    public void Sprint(bool enabled)
    {
    }

    public void SwitchBroadcasting()
    {
    }

    public void ShowInventory()
    {
    }

    public void ShowTerminal()
    {
    }

    public void SwitchHelmet()
    {
    }

    public void EnableDampeners(bool enable, bool updateSync = true)
    {
    }

    public void EnableJetpack(bool enable, bool fromLoad = false, bool updateSync = true, bool fromInit = false)
    {
    }

    public void SwitchDamping()
    {
    }

    public void SwitchThrusts()
    {
    }

    public void SwitchLights()
    {
    }

    public void SwitchReactors()
    {
    }

    public bool EnabledThrusts => false;

    public bool EnabledDamping => false;

    public bool EnabledLights => false;

    public bool EnabledLeadingGears => false;

    public bool EnabledReactors => false;

    public bool EnabledBroadcasting => false;

    public bool EnabledHelmet => false;

    public bool CanSwitchToWeapon(MyDefinitionId? weaponDefinition) => false;

    public void OnControlAcquired(MyEntityController controller)
    {
    }

    public void OnControlReleased(MyEntityController controller)
    {
    }

    public void Die()
    {
    }

    public void DrawHud(IMyCameraController camera, long playerId)
    {
    }

    public bool PrimaryLookaround => false;

    public MyEntity Entity => (MyEntity) this;

    public bool ForceFirstPersonCamera { get; set; }

    public MatrixD GetHeadMatrix(
      bool includeY,
      bool includeX = true,
      bool forceHeadAnim = false,
      bool forceHeadBone = false)
    {
      MatrixD worldMatrix = this.WorldMatrix;
      worldMatrix.Translation -= 4.0 * this.WorldMatrix.Forward;
      return worldMatrix;
    }

    public override MatrixD GetViewMatrix() => MatrixD.Invert(this.GetHeadMatrix(true, true, false, false));

    public void SwitchToWeapon(MyDefinitionId weaponDefinition)
    {
    }

    public void SwitchToWeapon(MyToolbarItemWeapon weapon)
    {
    }

    public void SwitchAmmoMagazine()
    {
    }

    public bool CanSwitchAmmoMagazine() => false;

    public void SwitchLandingGears()
    {
    }

    public void SwitchHandbrake()
    {
    }

    public float HeadLocalXAngle { get; set; }

    public float HeadLocalYAngle { get; set; }

    public MyToolbarType ToolbarType => MyToolbarType.Spectator;

    public MyToolbar Toolbar => this.m_toolbar;

    void IMyCameraController.ControlCamera(MyCamera currentCamera) => currentCamera.SetViewMatrix(this.GetViewMatrix());

    void IMyCameraController.Rotate(
      Vector2 rotationIndicator,
      float rollIndicator)
    {
      this.Rotate(rotationIndicator, rollIndicator);
    }

    void IMyCameraController.RotateStopped() => this.RotateStopped();

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

    bool IMyCameraController.HandlePickUp() => false;

    bool IMyCameraController.AllowCubeBuilding => false;

    public MyEntityCameraSettings GetCameraEntitySettings() => (MyEntityCameraSettings) null;

    public MyStringId ControlContext => MyStringId.NullOrEmpty;

    public MyStringId AuxiliaryContext => MyStringId.NullOrEmpty;

    public bool ShouldEndShootingOnPause(MyShootActionEnum action) => true;

    public MyEntity RelativeDampeningEntity { get; set; }

    IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => (IMyEntity) this.Entity;

    public Vector3 LastMotionIndicator { get; set; }

    public Vector3 LastRotationIndicator { get; set; }

    void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.DrawHud(
      IMyCameraController entity,
      long player)
    {
      if (entity == null)
        return;
      this.DrawHud(entity, player);
    }

    IMyControllerInfo VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ControllerInfo => (IMyControllerInfo) this.ControllerInfo;

    private class Sandbox_Game_Entities_MyControllableSphere\u003C\u003EActor : IActivator, IActivator<MyControllableSphere>
    {
      object IActivator.CreateInstance() => (object) new MyControllableSphere();

      MyControllableSphere IActivator<MyControllableSphere>.CreateInstance() => new MyControllableSphere();
    }
  }
}
