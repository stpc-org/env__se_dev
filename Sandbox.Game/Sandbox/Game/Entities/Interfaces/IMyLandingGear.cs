// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Interfaces.IMyLandingGear
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.Game.Entities.Interfaces
{
  public interface IMyLandingGear
  {
    bool AutoLock { get; }

    LandingGearMode LockMode { get; }

    event LockModeChangedHandler LockModeChanged;

    void RequestLock(bool enable);

    void ResetAutolock();

    IMyEntity GetAttachedEntity();
  }
}
