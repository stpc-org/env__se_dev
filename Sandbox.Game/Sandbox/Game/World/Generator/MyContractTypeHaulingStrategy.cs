// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyContractTypeHaulingStrategy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Multiplayer;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRage.ObjectBuilders;

namespace Sandbox.Game.World.Generator
{
  public class MyContractTypeHaulingStrategy : MyContractTypeBaseStrategy
  {
    private readonly double JUMP_DRIVE_DISTANCE = 2000000.0;
    private readonly float AMOUNT_URANIUM_TO_RECHARGE = 3.75f;
    private MyDefinitionId m_uranium = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Ingot), "Uranium");

    public MyContractTypeHaulingStrategy(
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
      MyContractDeliver myContractDeliver = new MyContractDeliver();
      if (!(myContractDeliver.GetDefinition() is MyContractTypeDeliverDefinition definition))
        return MyContractCreationResults.Error;
      MyStation myStation = factions.TryGetFactionById(factionId) is MyFaction factionById ? factionById.GetStationById(stationId) : (MyStation) null;
      if (myStation == null)
        return MyContractCreationResults.Error;
      MyFaction friendlyFaction;
      MyStation friendlyStation;
      if (!factions.GetRandomFriendlyStation(factionId, stationId, out friendlyFaction, out friendlyStation, true))
        return MyContractCreationResults.Fail_Impossible;
      double num1 = (friendlyStation.Position - myStation.Position).Length();
      MyObjectBuilder_ContractDeliver builderContractDeliver = new MyObjectBuilder_ContractDeliver();
      builderContractDeliver.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      builderContractDeliver.IsPlayerMade = false;
      builderContractDeliver.State = MyContractStateEnum.Inactive;
      int minimalPrice = 0;
      if (!calculator.TryGetItemMinimalPrice(this.m_uranium, out minimalPrice))
      {
        calculator.CalculateMinimalPrices(new SerializableDefinitionId[1]
        {
          (SerializableDefinitionId) this.m_uranium
        });
        calculator.TryGetItemMinimalPrice(this.m_uranium, out minimalPrice);
      }
      builderContractDeliver.RewardMoney = this.GetMoneyRewardForHaulingContract(definition.MinimumMoney, num1, minimalPrice);
      if (myStation.IsDeepSpaceStation)
        builderContractDeliver.RewardMoney = (long) ((double) builderContractDeliver.RewardMoney * (double) this.m_economyDefinition.DeepSpaceStationContractBonus);
      builderContractDeliver.RewardReputation = this.GetReputationRewardForHaulingContract(definition.MinimumReputation, num1);
      builderContractDeliver.StartingDeposit = (long) (MyRandom.Instance.NextDouble() * (double) (definition.MaxStartingDeposit - definition.MinStartingDeposit));
      builderContractDeliver.FailReputationPrice = definition.FailReputationPrice;
      builderContractDeliver.StartFaction = factionId;
      builderContractDeliver.StartStation = stationId;
      builderContractDeliver.StartBlock = 0L;
      builderContractDeliver.Creation = now.Ticks;
      builderContractDeliver.RemainingTimeInS = new double?(this.GetDurationForHaulingContract(definition, num1).Seconds);
      builderContractDeliver.DeliverDistance = num1;
      builderContractDeliver.TicksToDiscard = new int?(MyContractTypeBaseStrategy.TICKS_TO_LIVE);
      int num2 = 0;
      MyObjectBuilder_ContractConditionDeliverPackage conditionDeliverPackage1 = new MyObjectBuilder_ContractConditionDeliverPackage();
      conditionDeliverPackage1.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT_CONDITION);
      conditionDeliverPackage1.ContractId = builderContractDeliver.Id;
      conditionDeliverPackage1.FactionEndId = friendlyFaction.FactionId;
      conditionDeliverPackage1.StationEndId = friendlyStation.Id;
      conditionDeliverPackage1.BlockEndId = 0L;
      conditionDeliverPackage1.SubId = num2;
      conditionDeliverPackage1.IsFinished = false;
      MyObjectBuilder_ContractConditionDeliverPackage conditionDeliverPackage2 = conditionDeliverPackage1;
      int num3 = num2 + 1;
      builderContractDeliver.ContractCondition = (MyObjectBuilder_ContractCondition) conditionDeliverPackage2;
      myContractDeliver.Init((MyObjectBuilder_Contract) builderContractDeliver);
      contract = (MyContract) myContractDeliver;
      return MyContractCreationResults.Success;
    }

    private long GetMoneyRewardForHaulingContract(long baseRew, double distance, int uraniumPrice)
    {
      double num1 = distance / this.JUMP_DRIVE_DISTANCE;
      double num2 = num1 * ((double) uraniumPrice * (double) this.AMOUNT_URANIUM_TO_RECHARGE);
      return (long) ((double) baseRew + (double) baseRew * num1 + num2);
    }

    private int GetReputationRewardForHaulingContract(int baseRew, double distance) => baseRew;

    private MyTimeSpan GetDurationForHaulingContract(
      MyContractTypeDeliverDefinition def,
      double distanceInM,
      bool cutOutJumps = true)
    {
      double seconds;
      if (cutOutJumps)
      {
        int num = (int) (distanceInM / this.JUMP_DRIVE_DISTANCE);
        double jumpDriveDistance = this.JUMP_DRIVE_DISTANCE;
        seconds = def.DurationMultiplier * (def.Duration_BaseTime + (double) (long) (def.Duration_TimePerJumpDist * (double) (num + 1)));
      }
      else
        seconds = def.DurationMultiplier * (def.Duration_BaseTime + (double) (long) (def.Duration_TimePerMeter * distanceInM));
      return MyTimeSpan.FromSeconds(seconds);
    }
  }
}
