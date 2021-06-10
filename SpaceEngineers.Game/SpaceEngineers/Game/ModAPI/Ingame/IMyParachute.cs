// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.ModAPI.Ingame.IMyParachute
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace SpaceEngineers.Game.ModAPI.Ingame
{
  public interface IMyParachute : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    float Atmosphere { get; }

    bool TryGetClosestPoint(out Vector3D? closestPoint);

    Vector3D GetVelocity();

    Vector3D GetNaturalGravity();

    Vector3D GetArtificialGravity();

    Vector3D GetTotalGravity();

    DoorStatus Status { get; }

    float OpenRatio { get; }

    void OpenDoor();

    void CloseDoor();

    void ToggleDoor();
  }
}
