// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyPistonBase
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyPistonBase : IMyMechanicalConnectionBlock, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    float Velocity { get; set; }

    float MaxVelocity { get; }

    float MinLimit { get; set; }

    float MaxLimit { get; set; }

    float LowestPosition { get; }

    float HighestPosition { get; }

    float CurrentPosition { get; }

    PistonStatus Status { get; }

    void Extend();

    void Retract();

    void Reverse();
  }
}
