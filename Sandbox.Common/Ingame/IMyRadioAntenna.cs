// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyRadioAntenna
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyRadioAntenna : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    float Radius { get; set; }

    bool ShowShipName { get; set; }

    bool IsBroadcasting { get; }

    bool EnableBroadcasting { get; set; }

    string HudText { get; set; }
  }
}
