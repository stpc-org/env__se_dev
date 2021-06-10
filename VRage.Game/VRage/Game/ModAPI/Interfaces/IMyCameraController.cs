// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Interfaces.IMyCameraController
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Entity;
using VRage.Game.Utils;
using VRageMath;

namespace VRage.Game.ModAPI.Interfaces
{
  public interface IMyCameraController
  {
    bool IsInFirstPersonView { get; set; }

    bool ForceFirstPersonCamera { get; set; }

    bool EnableFirstPersonView { get; set; }

    bool AllowCubeBuilding { get; }

    MyEntity Entity { get; }

    void ControlCamera(MyCamera currentCamera);

    void Rotate(Vector2 rotationIndicator, float rollIndicator);

    void RotateStopped();

    void OnAssumeControl(IMyCameraController previousCameraController);

    void OnReleaseControl(IMyCameraController newCameraController);

    bool HandleUse();

    bool HandlePickUp();
  }
}
