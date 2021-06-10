// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyContractTypeDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ContractTypeDefinition), null)]
  public class MyContractTypeDefinition : MyDefinitionBase
  {
    public string TitleName { get; set; }

    public string DescriptionName { get; set; }

    public int MinimumReputation { get; set; }

    public int FailReputationPrice { get; set; }

    public long MinimumMoney { get; set; }

    public long MoneyReputationCoeficient { get; set; }

    public long MinStartingDeposit { get; set; }

    public long MaxStartingDeposit { get; set; }

    public double DurationMultiplier { get; set; }

    public Dictionary<SerializableDefinitionId, float> ChancesPerFactionType { get; set; }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_ContractTypeDefinition contractTypeDefinition))
        return;
      this.MinimumReputation = contractTypeDefinition.MinimumReputation;
      this.FailReputationPrice = contractTypeDefinition.FailReputationPrice;
      this.MinimumMoney = contractTypeDefinition.MinimumMoney;
      this.MoneyReputationCoeficient = contractTypeDefinition.MoneyReputationCoeficient;
      this.MinStartingDeposit = contractTypeDefinition.MinStartingDeposit;
      this.MaxStartingDeposit = contractTypeDefinition.MaxStartingDeposit;
      this.DurationMultiplier = contractTypeDefinition.DurationMultiplier;
      this.ChancesPerFactionType = new Dictionary<SerializableDefinitionId, float>();
      if (contractTypeDefinition.ChancesPerFactionType == null)
        return;
      foreach (MyContractChancePair contractChancePair in contractTypeDefinition.ChancesPerFactionType)
        this.ChancesPerFactionType.Add(contractChancePair.DefinitionId, contractChancePair.Value);
    }

    private class Sandbox_Definitions_MyContractTypeDefinition\u003C\u003EActor : IActivator, IActivator<MyContractTypeDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyContractTypeDefinition();

      MyContractTypeDefinition IActivator<MyContractTypeDefinition>.CreateInstance() => new MyContractTypeDefinition();
    }
  }
}
