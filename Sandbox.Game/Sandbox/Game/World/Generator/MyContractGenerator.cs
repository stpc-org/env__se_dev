// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyContractGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Multiplayer;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public class MyContractGenerator
  {
    private MySessionComponentEconomyDefinition m_economyDefinition;

    public MyContractGenerator(
      MySessionComponentEconomyDefinition economyDefinition)
    {
      this.m_economyDefinition = economyDefinition;
    }

    public MyContractCreationResults CreateCustomHaulingContract(
      out MyContract contract,
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long targetBlockId,
      MyTimeSpan now)
    {
      contract = (MyContract) null;
      MyContractDeliver myContractDeliver = new MyContractDeliver();
      if (!(MyEntities.GetEntityById(targetBlockId) is MyContractBlock entityById))
        return MyContractCreationResults.Fail_BlockNotFound;
      if (entityById.OwnerId != startBlock.OwnerId)
        return MyContractCreationResults.Fail_NotAnOwnerOfBlock;
      if (startBlock == null || entityById == null)
        return MyContractCreationResults.Error;
      double num1 = (startBlock.PositionComp.GetPosition() - entityById.PositionComp.GetPosition()).Length();
      MyObjectBuilder_ContractDeliver builderContractDeliver = new MyObjectBuilder_ContractDeliver();
      builderContractDeliver.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      builderContractDeliver.IsPlayerMade = true;
      builderContractDeliver.State = MyContractStateEnum.Inactive;
      builderContractDeliver.RewardMoney = (long) rewardMoney;
      builderContractDeliver.RewardReputation = 0;
      builderContractDeliver.StartingDeposit = (long) startingDeposit;
      builderContractDeliver.FailReputationPrice = 0;
      builderContractDeliver.StartFaction = 0L;
      builderContractDeliver.StartStation = 0L;
      builderContractDeliver.StartBlock = startBlock.EntityId;
      builderContractDeliver.Creation = now.Ticks;
      if (durationInMin > 0)
        builderContractDeliver.RemainingTimeInS = new double?(MyTimeSpan.FromMinutes((double) durationInMin).Seconds);
      else
        builderContractDeliver.RemainingTimeInS = new double?();
      builderContractDeliver.TicksToDiscard = new int?();
      builderContractDeliver.DeliverDistance = num1;
      int num2 = 0;
      MyObjectBuilder_ContractConditionDeliverPackage conditionDeliverPackage1 = new MyObjectBuilder_ContractConditionDeliverPackage();
      conditionDeliverPackage1.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT_CONDITION);
      conditionDeliverPackage1.ContractId = builderContractDeliver.Id;
      conditionDeliverPackage1.FactionEndId = 0L;
      conditionDeliverPackage1.StationEndId = 0L;
      conditionDeliverPackage1.BlockEndId = entityById.EntityId;
      conditionDeliverPackage1.SubId = num2;
      conditionDeliverPackage1.IsFinished = false;
      MyObjectBuilder_ContractConditionDeliverPackage conditionDeliverPackage2 = conditionDeliverPackage1;
      int num3 = num2 + 1;
      builderContractDeliver.ContractCondition = (MyObjectBuilder_ContractCondition) conditionDeliverPackage2;
      myContractDeliver.Init((MyObjectBuilder_Contract) builderContractDeliver);
      contract = (MyContract) myContractDeliver;
      return MyContractCreationResults.Success;
    }

    public MyContractCreationResults CreateCustomAcquisitionContract(
      out MyContract contract,
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long targetBlockId,
      MyDefinitionId itemType,
      int itemAmount,
      MyTimeSpan now)
    {
      contract = (MyContract) null;
      MyContractObtainAndDeliver obtainAndDeliver1 = new MyContractObtainAndDeliver();
      long num1 = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      if (MyDefinitionManager.Static.GetPhysicalItemDefinition(itemType) == null)
        return MyContractCreationResults.Error;
      if (!(MyEntities.GetEntityById(targetBlockId) is MyContractBlock entityById))
        return MyContractCreationResults.Fail_BlockNotFound;
      if (entityById.OwnerId != startBlock.OwnerId)
        return MyContractCreationResults.Fail_NotAnOwnerOfBlock;
      int num2 = 0;
      MyObjectBuilder_ContractConditionDeliverItems conditionDeliverItems1 = new MyObjectBuilder_ContractConditionDeliverItems();
      conditionDeliverItems1.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT_CONDITION);
      conditionDeliverItems1.ContractId = num1;
      conditionDeliverItems1.FactionEndId = 0L;
      conditionDeliverItems1.StationEndId = 0L;
      conditionDeliverItems1.BlockEndId = entityById.EntityId;
      conditionDeliverItems1.SubId = num2;
      conditionDeliverItems1.IsFinished = false;
      conditionDeliverItems1.TransferItems = true;
      conditionDeliverItems1.ItemType = (SerializableDefinitionId) itemType;
      conditionDeliverItems1.ItemAmount = itemAmount;
      MyObjectBuilder_ContractConditionDeliverItems conditionDeliverItems2 = conditionDeliverItems1;
      int num3 = num2 + 1;
      MyObjectBuilder_ContractObtainAndDeliver obtainAndDeliver2 = new MyObjectBuilder_ContractObtainAndDeliver();
      obtainAndDeliver2.Id = num1;
      obtainAndDeliver2.IsPlayerMade = true;
      obtainAndDeliver2.State = MyContractStateEnum.Inactive;
      obtainAndDeliver2.RewardMoney = (long) rewardMoney;
      obtainAndDeliver2.RewardReputation = 0;
      obtainAndDeliver2.StartingDeposit = (long) startingDeposit;
      obtainAndDeliver2.FailReputationPrice = 0;
      obtainAndDeliver2.StartFaction = 0L;
      obtainAndDeliver2.StartStation = 0L;
      obtainAndDeliver2.StartBlock = startBlock.EntityId;
      obtainAndDeliver2.Creation = now.Ticks;
      if (durationInMin > 0)
        obtainAndDeliver2.RemainingTimeInS = new double?(MyTimeSpan.FromMinutes((double) durationInMin).Seconds);
      else
        obtainAndDeliver2.RemainingTimeInS = new double?();
      obtainAndDeliver2.TicksToDiscard = new int?();
      obtainAndDeliver2.ContractCondition = (MyObjectBuilder_ContractCondition) conditionDeliverItems2;
      obtainAndDeliver1.Init((MyObjectBuilder_Contract) obtainAndDeliver2);
      contract = (MyContract) obtainAndDeliver1;
      return MyContractCreationResults.Success;
    }

    public MyContractCreationResults CreateCustomEscortContract(
      out MyContract contract,
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      Vector3D start,
      Vector3D end,
      long escortOwner,
      MyTimeSpan now)
    {
      MyFactionCollection factions = MySession.Static.Factions;
      contract = (MyContract) null;
      MyContractEscort myContractEscort = new MyContractEscort();
      if (!(myContractEscort.GetDefinition() is MyContractTypeEscortDefinition definition))
        return MyContractCreationResults.Error;
      double num = (start - end).Length();
      long pirateFactionId = this.GetPirateFactionId();
      if (pirateFactionId == 0L)
        return MyContractCreationResults.Error;
      MyObjectBuilder_ContractEscort builderContractEscort = new MyObjectBuilder_ContractEscort();
      builderContractEscort.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      builderContractEscort.IsPlayerMade = true;
      builderContractEscort.State = MyContractStateEnum.Inactive;
      builderContractEscort.RewardMoney = (long) rewardMoney;
      builderContractEscort.RewardReputation = 0;
      builderContractEscort.StartingDeposit = (long) startingDeposit;
      builderContractEscort.FailReputationPrice = 0;
      builderContractEscort.StartFaction = 0L;
      builderContractEscort.StartStation = 0L;
      builderContractEscort.StartBlock = startBlock.EntityId;
      builderContractEscort.GridId = 0L;
      builderContractEscort.StartPosition = (SerializableVector3D) start;
      builderContractEscort.EndPosition = (SerializableVector3D) end;
      builderContractEscort.PathLength = num;
      builderContractEscort.RewardRadius = definition.RewardRadius;
      builderContractEscort.TriggerEntityId = 0L;
      builderContractEscort.TriggerRadius = definition.TriggerRadius;
      builderContractEscort.DroneFirstDelay = MyTimeSpan.FromSeconds(pirateFactionId == 0L ? (double) int.MaxValue : (double) definition.DroneFirstDelayInS).Ticks;
      builderContractEscort.DroneAttackPeriod = MyTimeSpan.FromSeconds(pirateFactionId == 0L ? (double) int.MaxValue : (double) definition.DroneAttackPeriodInS).Ticks;
      builderContractEscort.DronesPerWave = pirateFactionId == 0L ? 0 : definition.DronesPerWave;
      builderContractEscort.InitialDelay = MyTimeSpan.FromSeconds((double) definition.InitialDelayInS).Ticks;
      builderContractEscort.WaveFactionId = pirateFactionId;
      builderContractEscort.EscortShipOwner = escortOwner;
      builderContractEscort.Creation = now.Ticks;
      if (durationInMin > 0)
        builderContractEscort.RemainingTimeInS = new double?(MyTimeSpan.FromMinutes((double) durationInMin).Seconds);
      else
        builderContractEscort.RemainingTimeInS = new double?();
      builderContractEscort.TicksToDiscard = new int?();
      myContractEscort.Init((MyObjectBuilder_Contract) builderContractEscort);
      contract = (MyContract) myContractEscort;
      return MyContractCreationResults.Success;
    }

    public MyContractCreationResults CreateCustomSearchContract(
      out MyContract contract,
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long targetGridId,
      double searchRadius,
      MyTimeSpan now)
    {
      contract = (MyContract) null;
      MyContractFind myContractFind = new MyContractFind();
      if (!(myContractFind.GetDefinition() is MyContractTypeFindDefinition definition))
        return MyContractCreationResults.Error;
      if (!(MyEntities.GetEntityById(targetGridId) is MyCubeGrid entityById))
        return MyContractCreationResults.Fail_GridNotFound;
      if (!entityById.BigOwners.Contains(startBlock.OwnerId))
        return MyContractCreationResults.Fail_NotAnOwnerOfGrid;
      Vector3D uniformPointInSphere = new BoundingSphereD(entityById.PositionComp.GetPosition(), searchRadius).RandomToUniformPointInSphere((double) MyRandom.Instance.NextFloat(), (double) MyRandom.Instance.NextFloat(), (double) MyRandom.Instance.NextFloat());
      double num = (uniformPointInSphere - startBlock.PositionComp.GetPosition()).Length();
      MyObjectBuilder_ContractFind builderContractFind = new MyObjectBuilder_ContractFind();
      builderContractFind.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      builderContractFind.IsPlayerMade = true;
      builderContractFind.State = MyContractStateEnum.Inactive;
      builderContractFind.RewardMoney = (long) rewardMoney;
      builderContractFind.RewardReputation = 0;
      builderContractFind.StartingDeposit = (long) startingDeposit;
      builderContractFind.FailReputationPrice = 0;
      builderContractFind.StartFaction = 0L;
      builderContractFind.StartStation = 0L;
      builderContractFind.StartBlock = startBlock.EntityId;
      builderContractFind.GridPosition = (SerializableVector3D) startBlock.PositionComp.GetPosition();
      builderContractFind.GpsPosition = (SerializableVector3D) uniformPointInSphere;
      builderContractFind.GpsDistance = num;
      builderContractFind.MaxGpsOffset = (float) searchRadius;
      builderContractFind.TriggerRadius = definition.TriggerRadius;
      builderContractFind.GridId = entityById.EntityId;
      builderContractFind.KeepGridAtTheEnd = true;
      builderContractFind.Creation = now.Ticks;
      if (durationInMin > 0)
        builderContractFind.RemainingTimeInS = new double?(MyTimeSpan.FromMinutes((double) durationInMin).Seconds);
      else
        builderContractFind.RemainingTimeInS = new double?();
      builderContractFind.TicksToDiscard = new int?();
      myContractFind.Init((MyObjectBuilder_Contract) builderContractFind);
      contract = (MyContract) myContractFind;
      return MyContractCreationResults.Success;
    }

    public MyContractCreationResults CreateCustomBountyContract(
      out MyContract contract,
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long targetIdentityId,
      MyTimeSpan now)
    {
      contract = (MyContract) null;
      MyContractHunt myContractHunt = new MyContractHunt();
      if (!(myContractHunt.GetDefinition() is MyContractTypeHuntDefinition definition))
        return MyContractCreationResults.Error;
      MyObjectBuilder_ContractHunt builderContractHunt = new MyObjectBuilder_ContractHunt();
      builderContractHunt.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      builderContractHunt.IsPlayerMade = true;
      builderContractHunt.State = MyContractStateEnum.Inactive;
      builderContractHunt.RewardMoney = (long) rewardMoney;
      builderContractHunt.RewardReputation = 0;
      builderContractHunt.StartingDeposit = (long) startingDeposit;
      builderContractHunt.FailReputationPrice = 0;
      builderContractHunt.StartFaction = 0L;
      builderContractHunt.StartStation = 0L;
      builderContractHunt.StartBlock = startBlock.EntityId;
      builderContractHunt.Target = targetIdentityId;
      builderContractHunt.RemarkPeriod = MyTimeSpan.FromSeconds((double) definition.RemarkPeriodInS).Ticks;
      builderContractHunt.RemarkVariance = definition.RemarkVariance;
      builderContractHunt.KillRange = definition.KillRange;
      builderContractHunt.KillRangeMultiplier = definition.KillRangeMultiplier;
      builderContractHunt.ReputationLossForTarget = definition.ReputationLossForTarget;
      builderContractHunt.RewardRadius = definition.RewardRadius;
      builderContractHunt.Creation = now.Ticks;
      builderContractHunt.RemainingTimeInS = new double?(MyTimeSpan.FromMinutes((double) durationInMin).Seconds);
      builderContractHunt.TicksToDiscard = new int?();
      myContractHunt.Init((MyObjectBuilder_Contract) builderContractHunt);
      contract = (MyContract) myContractHunt;
      return MyContractCreationResults.Success;
    }

    public MyContractCreationResults CreateCustomRepairContract(
      out MyContract contract,
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long gridId,
      MyTimeSpan now)
    {
      MyFactionCollection factions = MySession.Static.Factions;
      contract = (MyContract) null;
      MyContractRepair myContractRepair = new MyContractRepair();
      if (!(myContractRepair.GetDefinition() is MyContractTypeRepairDefinition))
        return MyContractCreationResults.Error;
      MyObjectBuilder_ContractRepair builderContractRepair = new MyObjectBuilder_ContractRepair();
      builderContractRepair.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      builderContractRepair.IsPlayerMade = true;
      builderContractRepair.State = MyContractStateEnum.Inactive;
      builderContractRepair.RewardMoney = (long) rewardMoney;
      builderContractRepair.RewardReputation = 0;
      builderContractRepair.StartingDeposit = (long) startingDeposit;
      builderContractRepair.FailReputationPrice = 0;
      builderContractRepair.StartFaction = 0L;
      builderContractRepair.StartStation = 0L;
      builderContractRepair.StartBlock = startBlock.EntityId;
      builderContractRepair.GridPosition = (SerializableVector3D) Vector3D.Zero;
      builderContractRepair.GridId = gridId;
      builderContractRepair.PrefabName = string.Empty;
      builderContractRepair.BlocksToRepair = new MySerializableList<Vector3I>();
      builderContractRepair.KeepGridAtTheEnd = true;
      builderContractRepair.UnrepairedBlockCount = 0;
      builderContractRepair.Creation = now.Ticks;
      if (durationInMin > 0)
        builderContractRepair.RemainingTimeInS = new double?(MyTimeSpan.FromMinutes((double) durationInMin).Seconds);
      else
        builderContractRepair.RemainingTimeInS = new double?();
      builderContractRepair.TicksToDiscard = new int?();
      myContractRepair.Init((MyObjectBuilder_Contract) builderContractRepair);
      contract = (MyContract) myContractRepair;
      return MyContractCreationResults.Success;
    }

    public MyContractCreationResults CreateCustomCustomContract(
      out MyContract contract,
      MyDefinitionId definitionId,
      string name,
      string description,
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int reputationReward,
      int failReputationPrice,
      int durationInMin,
      MyTimeSpan now,
      MyContractBlock endBlock = null)
    {
      contract = (MyContract) null;
      MyContractCustom myContractCustom = new MyContractCustom();
      MyObjectBuilder_ContractCustom builderContractCustom = new MyObjectBuilder_ContractCustom();
      builderContractCustom.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      builderContractCustom.IsPlayerMade = true;
      builderContractCustom.State = MyContractStateEnum.Inactive;
      builderContractCustom.RewardMoney = (long) rewardMoney;
      builderContractCustom.RewardReputation = reputationReward;
      builderContractCustom.StartingDeposit = (long) startingDeposit;
      builderContractCustom.FailReputationPrice = failReputationPrice;
      builderContractCustom.DefinitionId = (SerializableDefinitionId) definitionId;
      builderContractCustom.ContractName = name;
      builderContractCustom.ContractDescription = description;
      builderContractCustom.StartFaction = 0L;
      builderContractCustom.StartStation = 0L;
      builderContractCustom.StartBlock = startBlock.EntityId;
      builderContractCustom.Creation = now.Ticks;
      if (durationInMin > 0)
        builderContractCustom.RemainingTimeInS = new double?(MyTimeSpan.FromMinutes((double) durationInMin).Seconds);
      else
        builderContractCustom.RemainingTimeInS = new double?();
      builderContractCustom.TicksToDiscard = new int?();
      if (endBlock != null)
      {
        int num1 = 0;
        MyObjectBuilder_ContractConditionCustom contractConditionCustom1 = new MyObjectBuilder_ContractConditionCustom();
        contractConditionCustom1.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT_CONDITION);
        contractConditionCustom1.ContractId = builderContractCustom.Id;
        contractConditionCustom1.FactionEndId = 0L;
        contractConditionCustom1.StationEndId = 0L;
        contractConditionCustom1.BlockEndId = endBlock.EntityId;
        contractConditionCustom1.SubId = num1;
        contractConditionCustom1.IsFinished = false;
        MyObjectBuilder_ContractConditionCustom contractConditionCustom2 = contractConditionCustom1;
        int num2 = num1 + 1;
        builderContractCustom.ContractCondition = (MyObjectBuilder_ContractCondition) contractConditionCustom2;
      }
      myContractCustom.Init((MyObjectBuilder_Contract) builderContractCustom);
      contract = (MyContract) myContractCustom;
      return MyContractCreationResults.Success;
    }

    public long GetRandomPlayer()
    {
      ICollection<MyPlayer.PlayerId> allPlayers = MySession.Static.Players.GetAllPlayers();
      if (allPlayers.Count <= 0)
        return 0;
      int num1 = MyRandom.Instance.Next(0, allPlayers.Count);
      int num2 = 0;
      foreach (MyPlayer.PlayerId playerId in (IEnumerable<MyPlayer.PlayerId>) allPlayers)
      {
        if (num2 == num1)
          return MySession.Static.Players.TryGetIdentityId(playerId.SteamId, 0);
        ++num2;
      }
      return 0;
    }

    public long GetPirateFactionId()
    {
      if (!(MyDefinitionManager.Static.GetDefinition(this.m_economyDefinition.PirateId) is MyFactionDefinition definition))
        return 0;
      MyFaction myFaction = (MyFaction) null;
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        if (faction.Value.Tag == definition.Tag)
        {
          myFaction = faction.Value;
          break;
        }
      }
      return myFaction == null ? 0L : myFaction.FactionId;
    }
  }
}
