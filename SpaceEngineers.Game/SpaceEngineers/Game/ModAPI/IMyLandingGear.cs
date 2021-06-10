// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.ModAPI.IMyLandingGear
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using SpaceEngineers.Game.ModAPI.Ingame;
using System;

namespace SpaceEngineers.Game.ModAPI
{
  public interface IMyLandingGear : Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyLandingGear
  {
    event Action<IMyLandingGear, LandingGearMode> LockModeChanged;

    [Obsolete("Use LockModeChanged instead.")]
    event Action<bool> StateChanged;

    VRage.ModAPI.IMyEntity GetAttachedEntity();
  }
}
