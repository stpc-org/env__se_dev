// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.MyContractCustom
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.ModAPI
{
  internal class MyContractCustom : IMyContractCustom, IMyContract
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
      string name,
      string description,
      long startBlockId,
      int moneyReward,
      int collateral,
      int duration,
      long? endBlockId = null)
    {
      this.StartBlockId = startBlockId;
      this.MoneyReward = moneyReward;
      this.Collateral = collateral;
      this.Duration = duration;
      this.DefinitionId = definitionId;
      this.EndBlockId = endBlockId;
      this.Name = name;
      this.Description = description;
    }
  }
}
