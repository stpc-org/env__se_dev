// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.IMyShipController
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRageMath;

namespace Sandbox.ModAPI
{
  public interface IMyShipController : IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyShipController, IMyControllableEntity
  {
    bool HasFirstPersonCamera { get; }

    IMyCharacter LastPilot { get; }

    IMyCharacter Pilot { get; }

    bool IsShooting { get; }

    new Vector3 MoveIndicator { get; }

    new Vector2 RotationIndicator { get; }

    new float RollIndicator { get; }

    bool IsDefault3rdView { get; }
  }
}
