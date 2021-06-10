// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Contracts.MyContractCustom
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game;
using VRage.Game.ModAPI;

namespace Sandbox.ModAPI.Contracts
{
  public class MyContractCustom : IMyContractCustom, IMyContract
  {
    public MyDefinitionId DefinitionId { get; private set; }

    public long? EndBlockId { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public int ReputationReward { get; private set; }

    public int FailReputationPrice { get; private set; }

    public long StartBlockId { get; private set; }

    public int MoneyReward { get; private set; }

    public int Collateral { get; private set; }

    public int Duration { get; private set; }

    public Action<long> OnContractAcquired { get; set; }

    public Action OnContractSucceeded { get; set; }

    public Action OnContractFailed { get; set; }

    public MyContractCustom(
      MyDefinitionId definitionId,
      long startBlockId,
      int moneyReward,
      int collateral,
      int duration,
      string name,
      string description,
      int reputationReward,
      int failReputationPrice,
      long? endBlockId)
    {
      this.DefinitionId = definitionId;
      this.EndBlockId = endBlockId;
      this.Name = name;
      this.Description = description;
      this.ReputationReward = reputationReward;
      this.FailReputationPrice = failReputationPrice;
      this.StartBlockId = startBlockId;
      this.MoneyReward = moneyReward;
      this.Collateral = collateral;
      this.Duration = duration;
    }
  }
}
