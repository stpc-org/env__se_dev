// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.IMyBatteryBlock
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public interface IMyBatteryBlock : IMyPowerProducer, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
  {
    bool HasCapacityRemaining { get; }

    float CurrentStoredPower { get; }

    float MaxStoredPower { get; }

    float CurrentInput { get; }

    float MaxInput { get; }

    bool IsCharging { get; }

    ChargeMode ChargeMode { get; set; }

    [Obsolete("Use ChargeMode instead")]
    bool OnlyRecharge { get; set; }

    [Obsolete("Use ChargeMode instead")]
    bool OnlyDischarge { get; set; }

    [Obsolete("Semi-auto is no longer a valid mode, if you want to check for Auto instead, use ChargeMode")]
    bool SemiautoEnabled { get; set; }
  }
}
