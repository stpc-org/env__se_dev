// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers.Game.ModAPI.Ingame
{
  public interface IMyAirVent : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    [Obsolete("IsPressurized() is deprecated, please use CanPressurize instead.")]
    bool IsPressurized();

    bool CanPressurize { get; }

    float GetOxygenLevel();

    [Obsolete("IsDepressurizing is deprecated, please use Depressurize instead.")]
    bool IsDepressurizing { get; }

    bool Depressurize { get; set; }

    VentStatus Status { get; }

    bool PressurizationEnabled { get; }
  }
}
