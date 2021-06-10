// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Interfaces.IMyControllableEntity
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI.Interfaces
{
  public interface IMyControllableEntity
  {
    IMyControllerInfo ControllerInfo { get; }

    IMyEntity Entity { get; }

    bool ForceFirstPersonCamera { get; set; }

    MatrixD GetHeadMatrix(
      bool includeY,
      bool includeX = true,
      bool forceHeadAnim = false,
      bool forceHeadBone = false);

    Vector3 LastMotionIndicator { get; }

    Vector3 LastRotationIndicator { get; }

    void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator);

    void MoveAndRotateStopped();

    void Use();

    void UseContinues();

    void PickUp();

    void PickUpContinues();

    void Jump(Vector3 moveindicator = default (Vector3));

    void SwitchWalk();

    void Up();

    void Crouch();

    void Down();

    void ShowInventory();

    void ShowTerminal();

    void SwitchThrusts();

    void SwitchDamping();

    void SwitchLights();

    void SwitchLandingGears();

    void SwitchHandbrake();

    void SwitchReactors();

    void SwitchHelmet();

    bool EnabledThrusts { get; }

    bool EnabledDamping { get; }

    bool EnabledLights { get; }

    bool EnabledLeadingGears { get; }

    bool EnabledReactors { get; }

    bool EnabledHelmet { get; }

    void DrawHud(IMyCameraController camera, long playerId);

    void Die();

    bool PrimaryLookaround { get; }
  }
}
