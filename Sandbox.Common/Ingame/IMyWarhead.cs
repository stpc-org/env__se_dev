// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyWarhead
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyWarhead : IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    bool IsCountingDown { get; }

    float DetonationTime { get; set; }

    bool StartCountdown();

    bool StopCountdown();

    bool IsArmed { get; set; }

    void Detonate();
  }
}
