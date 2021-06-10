// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Contracts.MyContractRepair
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game.ModAPI;

namespace Sandbox.ModAPI.Contracts
{
  public class MyContractRepair : IMyContractRepair, IMyContract
  {
    public long GridId { get; private set; }

    public long StartBlockId { get; private set; }

    public int MoneyReward { get; private set; }

    public int Collateral { get; private set; }

    public int Duration { get; private set; }

    public Action<long> OnContractAcquired { get; set; }

    public Action OnContractSucceeded { get; set; }

    public Action OnContractFailed { get; set; }

    public MyContractRepair(
      long startBlockId,
      int moneyReward,
      int collateral,
      int duration,
      long gridId)
    {
      this.StartBlockId = startBlockId;
      this.MoneyReward = moneyReward;
      this.Collateral = collateral;
      this.Duration = duration;
      this.GridId = gridId;
    }
  }
}
