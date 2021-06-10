// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers.Game.ModAPI.Ingame
{
  public interface IMyLandingGear : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    [Obsolete("Landing gear are not breakable anymore.")]
    bool IsBreakable { get; }

    bool IsLocked { get; }

    bool AutoLock { get; set; }

    LandingGearMode LockMode { get; }

    void Lock();

    void ToggleLock();

    void Unlock();

    void ResetAutoLock();
  }
}
