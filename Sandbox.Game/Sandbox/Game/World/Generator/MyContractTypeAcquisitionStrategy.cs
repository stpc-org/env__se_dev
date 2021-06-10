// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyContractTypeAcquisitionStrategy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Multiplayer;
using System;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRage.ObjectBuilders;

namespace Sandbox.Game.World.Generator
{
  public class MyContractTypeAcquisitionStrategy : MyContractTypeBaseStrategy
  {
    public MyContractTypeAcquisitionStrategy(
      MySessionComponentEconomyDefinition economyDefinition)
      : base(economyDefinition)
    {
    }

    public override bool CanBeGenerated(MyStation station, MyFaction faction) => true;

    public override MyContractCreationResults GenerateContract(
      out MyContract contract,
      long factionId,
      long stationId,
      MyMinimalPriceCalculator calculator,
      MyTimeSpan now)
    {
      MyFactionCollection factions = MySession.Static.Factions;
      contract = (MyContract) null;
      MyContractObtainAndDeliver obtainAndDeliver1 = new MyContractObtainAndDeliver();
      if (!(obtainAndDeliver1.GetDefinition() is MyContractTypeObtainAndDeliverDefinition definition))
        return MyContractCreationResults.Error;
      MyStation myStation = factions.TryGetFactionById(factionId) is MyFaction factionById ? factionById.GetStationById(stationId) : (MyStation) null;
      if (myStation == null)
        return MyContractCreationResults.Error;
      long num1 = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      long num2 = 0;
      MyPhysicalItemDefinition itemType;
      int itemAmount;
      float itemVolume;
      long itemPrice;
      if (!this.GenerateObtainAndDeliverItem(factionId, stationId, calculator, out itemType, out itemAmount, out itemVolume, out itemPrice))
        return MyContractCreationResults.Error;
      int num3 = 0;
      MyObjectBuilder_ContractConditionDeliverItems conditionDeliverItems1 = new MyObjectBuilder_ContractConditionDeliverItems();
      conditionDeliverItems1.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT_CONDITION);
      conditionDeliverItems1.ContractId = num1;
      conditionDeliverItems1.FactionEndId = factionId;
      conditionDeliverItems1.StationEndId = stationId;
      conditionDeliverItems1.BlockEndId = 0L;
      conditionDeliverItems1.SubId = num3;
      conditionDeliverItems1.IsFinished = false;
      conditionDeliverItems1.TransferItems = false;
      conditionDeliverItems1.ItemType = (SerializableDefinitionId) itemType.Id;
      conditionDeliverItems1.ItemAmount = itemAmount;
      conditionDeliverItems1.ItemVolume = itemVolume;
      MyObjectBuilder_ContractConditionDeliverItems conditionDeliverItems2 = conditionDeliverItems1;
      int num4 = num3 + 1;
      long num5 = num2 + itemPrice;
      MyObjectBuilder_ContractObtainAndDeliver obtainAndDeliver2 = new MyObjectBuilder_ContractObtainAndDeliver();
      obtainAndDeliver2.Id = num1;
      obtainAndDeliver2.IsPlayerMade = false;
      obtainAndDeliver2.State = MyContractStateEnum.Inactive;
      obtainAndDeliver2.RewardMoney = this.GetMoneyRewardForAcquisitionContract(definition.MinimumMoney, itemAmount) + num5;
      if (myStation.IsDeepSpaceStation)
        obtainAndDeliver2.RewardMoney = (long) ((double) obtainAndDeliver2.RewardMoney * (double) this.m_economyDefinition.DeepSpaceStationContractBonus);
      obtainAndDeliver2.RewardReputation = this.GetReputationRewardForAcquisitionContract(definition.MinimumReputation);
      obtainAndDeliver2.StartingDeposit = (long) (MyRandom.Instance.NextDouble() * (double) (definition.MaxStartingDeposit - definition.MinStartingDeposit));
      obtainAndDeliver2.FailReputationPrice = definition.FailReputationPrice;
      obtainAndDeliver2.StartFaction = factionId;
      obtainAndDeliver2.StartStation = stationId;
      obtainAndDeliver2.StartBlock = 0L;
      obtainAndDeliver2.Creation = now.Ticks;
      obtainAndDeliver2.RemainingTimeInS = new double?();
      obtainAndDeliver2.TicksToDiscard = new int?(MyContractTypeBaseStrategy.TICKS_TO_LIVE);
      obtainAndDeliver2.ContractCondition = (MyObjectBuilder_ContractCondition) conditionDeliverItems2;
      obtainAndDeliver1.Init((MyObjectBuilder_Contract) obtainAndDeliver2);
      contract = (MyContract) obtainAndDeliver1;
      return MyContractCreationResults.Success;
    }

    private bool GenerateObtainAndDeliverItem(
      long factionId,
      long stationId,
      MyMinimalPriceCalculator calculator,
      out MyPhysicalItemDefinition itemType,
      out int itemAmount,
      out float itemVolume,
      out long itemPrice)
    {
      long num1 = 25;
      float baseCostProductionSpeedMultiplier = 1f;
      if (new MyContractObtainAndDeliver().GetDefinition() is MyContractTypeObtainAndDeliverDefinition definition)
      {
        int num2 = 0;
        do
        {
          int num3 = MyRandom.Instance.Next(0, definition.AvailableItems.Count);
          MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyDefinitionId) definition.AvailableItems[(num3 + num2) % definition.AvailableItems.Count]);
          if (physicalItemDefinition != null)
          {
            itemType = physicalItemDefinition;
            itemAmount = MyRandom.Instance.Next(physicalItemDefinition.MinimumAcquisitionAmount, physicalItemDefinition.MaximumAcquisitionAmount + 1);
            itemVolume = physicalItemDefinition.Volume * (float) itemAmount;
            int minimalPrice = 0;
            if (calculator.TryGetItemMinimalPrice(itemType.Id, out minimalPrice))
            {
              itemPrice = (long) (minimalPrice * itemAmount);
            }
            else
            {
              calculator.CalculateMinimalPrices(new SerializableDefinitionId[1]
              {
                (SerializableDefinitionId) itemType.Id
              }, baseCostProductionSpeedMultiplier);
              itemPrice = !calculator.TryGetItemMinimalPrice(itemType.Id, out minimalPrice) ? num1 * (long) itemAmount : (long) (minimalPrice * itemAmount);
            }
            return true;
          }
          ++num2;
        }
        while (num2 < definition.AvailableItems.Count);
      }
      ListReader<MyPhysicalItemDefinition> physicalItemDefinitions = MyDefinitionManager.Static.GetPhysicalItemDefinitions();
      if (physicalItemDefinitions.Count <= 0)
      {
        itemType = (MyPhysicalItemDefinition) null;
        itemAmount = 0;
        itemVolume = 0.0f;
        itemPrice = 0L;
        return false;
      }
      itemType = physicalItemDefinitions[MyRandom.Instance.Next(0, physicalItemDefinitions.Count)];
      itemAmount = 100;
      itemVolume = (float) itemAmount * itemType.Volume;
      itemPrice = 500L;
      return true;
    }

    private long GetMoneyRewardForAcquisitionContract(long baseRew, int amount) => (long) ((double) baseRew * Math.Pow(2.0, Math.Log10((double) amount)));

    private int GetReputationRewardForAcquisitionContract(int baseRew) => baseRew;
  }
}
