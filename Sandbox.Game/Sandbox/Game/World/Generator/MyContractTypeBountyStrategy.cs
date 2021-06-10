// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyContractTypeBountyStrategy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;

namespace Sandbox.Game.World.Generator
{
  public class MyContractTypeBountyStrategy : MyContractTypeBaseStrategy
  {
    public MyContractTypeBountyStrategy(
      MySessionComponentEconomyDefinition economyDefinition)
      : base(economyDefinition)
    {
    }

    public override bool CanBeGenerated(MyStation station, MyFaction faction) => MySession.Static.Settings.EnableBountyContracts && this.IsAnyPiratePlayerAvailable();

    public override MyContractCreationResults GenerateContract(
      out MyContract contract,
      long factionId,
      long stationId,
      MyMinimalPriceCalculator calculator,
      MyTimeSpan now)
    {
      MyFactionCollection factions = MySession.Static.Factions;
      contract = (MyContract) null;
      MyContractHunt myContractHunt = new MyContractHunt();
      if (!(myContractHunt.GetDefinition() is MyContractTypeHuntDefinition definition))
        return MyContractCreationResults.Error;
      MyStation myStation = factions.TryGetFactionById(factionId) is MyFaction factionById ? factionById.GetStationById(stationId) : (MyStation) null;
      if (myStation == null)
        return MyContractCreationResults.Error;
      long playerWeightedByRep = this.GetRandomPiratePlayer_WeightedByRep(factions);
      if (playerWeightedByRep == 0L)
        return MyContractCreationResults.Fail_Impossible;
      MyObjectBuilder_ContractHunt builderContractHunt = new MyObjectBuilder_ContractHunt();
      builderContractHunt.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      builderContractHunt.IsPlayerMade = false;
      builderContractHunt.State = MyContractStateEnum.Inactive;
      builderContractHunt.RewardMoney = this.GetMoneyRewardForBountyContract(playerWeightedByRep, definition.MinimumMoney, definition.MoneyReputationCoeficient);
      if (myStation.IsDeepSpaceStation)
        builderContractHunt.RewardMoney = (long) ((double) builderContractHunt.RewardMoney * (double) this.m_economyDefinition.DeepSpaceStationContractBonus);
      builderContractHunt.RewardReputation = this.GetReputationRewardForBountyContract(definition.MinimumReputation);
      builderContractHunt.StartingDeposit = (long) (MyRandom.Instance.NextDouble() * (double) (definition.MaxStartingDeposit - definition.MinStartingDeposit));
      builderContractHunt.FailReputationPrice = definition.FailReputationPrice;
      builderContractHunt.StartFaction = factionId;
      builderContractHunt.StartStation = stationId;
      builderContractHunt.StartBlock = 0L;
      builderContractHunt.Target = playerWeightedByRep;
      builderContractHunt.RemarkPeriod = MyTimeSpan.FromSeconds((double) definition.RemarkPeriodInS).Ticks;
      builderContractHunt.RemarkVariance = definition.RemarkVariance;
      builderContractHunt.KillRange = definition.KillRange;
      builderContractHunt.KillRangeMultiplier = definition.KillRangeMultiplier;
      builderContractHunt.ReputationLossForTarget = definition.ReputationLossForTarget;
      builderContractHunt.RewardRadius = definition.RewardRadius;
      builderContractHunt.Creation = now.Ticks;
      builderContractHunt.RemainingTimeInS = new double?(this.GetDurationForBountyContract(definition).Seconds);
      builderContractHunt.TicksToDiscard = new int?(MyContractTypeBaseStrategy.TICKS_TO_LIVE);
      myContractHunt.Init((MyObjectBuilder_Contract) builderContractHunt);
      contract = (MyContract) myContractHunt;
      return MyContractCreationResults.Success;
    }

    public long GetRandomPiratePlayer_WeightedByRep(MyFactionCollection factions)
    {
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component == null || !(MyDefinitionManager.Static.GetDefinition(this.m_economyDefinition.PirateId) is MyFactionDefinition definition))
        return 0;
      MyFaction myFaction = (MyFaction) null;
      foreach (KeyValuePair<long, MyFaction> faction in factions)
      {
        if (faction.Value.Tag == definition.Tag)
        {
          myFaction = faction.Value;
          break;
        }
      }
      if (myFaction == null)
        return 0;
      List<int> intList = new List<int>();
      List<long> longList = new List<long>();
      int maxValue = 0;
      foreach (MyPlayer.PlayerId allPlayer in (IEnumerable<MyPlayer.PlayerId>) MySession.Static.Players.GetAllPlayers())
      {
        long identityId = MySession.Static.Players.TryGetIdentityId(allPlayer.SteamId, 0);
        Tuple<MyRelationsBetweenFactions, int> playerAndFaction = factions.GetRelationBetweenPlayerAndFaction(identityId, myFaction.FactionId);
        if (playerAndFaction.Item1 != MyRelationsBetweenFactions.Enemies && playerAndFaction.Item1 != MyRelationsBetweenFactions.Neutral)
        {
          int chance = component.ConvertPirateReputationToChance(playerAndFaction.Item2);
          maxValue += chance;
          intList.Add(chance);
          longList.Add(identityId);
        }
      }
      if (longList.Count <= 0)
        return 0;
      int num = MyRandom.Instance.Next(0, maxValue);
      for (int index = 0; index < longList.Count; ++index)
      {
        if (num < intList[index])
          return longList[index];
        num -= intList[index];
      }
      return 0;
    }

    private long GetMoneyRewardForBountyContract(
      long identityId,
      long baseRew,
      long reputationCoeficient)
    {
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component == null)
        return baseRew;
      MySessionComponentEconomyDefinition economyDefinition = component.EconomyDefinition;
      if (economyDefinition == null || !(MyDefinitionManager.Static.GetDefinition(economyDefinition.PirateId) is MyFactionDefinition definition))
        return baseRew;
      MyFaction myFaction = (MyFaction) null;
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        if (faction.Value.Tag == definition.Tag)
        {
          myFaction = faction.Value;
          break;
        }
      }
      if (myFaction == null)
        return baseRew;
      Tuple<MyRelationsBetweenFactions, int> playerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(identityId, myFaction.FactionId);
      if (playerAndFaction.Item1 == MyRelationsBetweenFactions.Enemies)
        return baseRew;
      int num = Math.Max(playerAndFaction.Item2 - component.GetFriendlyMin(), 0);
      return baseRew + (long) (num * num) * reputationCoeficient;
    }

    private int GetReputationRewardForBountyContract(int baseRew) => baseRew;

    private MyTimeSpan GetDurationForBountyContract(MyContractTypeHuntDefinition def) => MyTimeSpan.FromSeconds(def.DurationMultiplier * def.Duration_BaseTime);

    public bool IsAnyPiratePlayerAvailable()
    {
      MyFactionCollection factions = MySession.Static.Factions;
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component == null || !(MyDefinitionManager.Static.GetDefinition(this.m_economyDefinition.PirateId) is MyFactionDefinition definition))
        return false;
      MyFaction myFaction = (MyFaction) null;
      foreach (KeyValuePair<long, MyFaction> keyValuePair in factions)
      {
        if (keyValuePair.Value.Tag == definition.Tag)
        {
          myFaction = keyValuePair.Value;
          break;
        }
      }
      if (myFaction == null)
        return false;
      List<int> intList = new List<int>();
      List<long> longList = new List<long>();
      int num = 0;
      foreach (MyPlayer.PlayerId allPlayer in (IEnumerable<MyPlayer.PlayerId>) MySession.Static.Players.GetAllPlayers())
      {
        long identityId = MySession.Static.Players.TryGetIdentityId(allPlayer.SteamId, 0);
        Tuple<MyRelationsBetweenFactions, int> playerAndFaction = factions.GetRelationBetweenPlayerAndFaction(identityId, myFaction.FactionId);
        if (playerAndFaction.Item1 != MyRelationsBetweenFactions.Enemies)
        {
          int chance = component.ConvertPirateReputationToChance(playerAndFaction.Item2);
          num += chance;
          intList.Add(chance);
          longList.Add(identityId);
        }
      }
      return longList.Count > 0;
    }
  }
}
