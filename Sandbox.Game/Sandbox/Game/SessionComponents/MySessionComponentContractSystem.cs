// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentContractSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 666, typeof (MyObjectBuilder_SessionComponentContractSystem), null, true)]
  public class MySessionComponentContractSystem : MySessionComponentBase, IMyContractSystem
  {
    private static readonly int CONTRACT_CREATION_TRIES_MAX = 20;
    private static readonly MyDefinitionId FactionTypeId_Miner = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FactionTypeDefinition), "Miner");
    private static readonly MyDefinitionId FactionTypeId_Trader = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FactionTypeDefinition), "Trader");
    private static readonly MyDefinitionId FactionTypeId_Builder = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FactionTypeDefinition), "Builder");
    private static readonly MyDefinitionId ContractTypeId_Hauling = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Deliver");
    private static readonly MyDefinitionId ContractTypeId_Acquisition = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "ObtainAndDeliver");
    private static readonly MyDefinitionId ContractTypeId_Escort = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Escort");
    private static readonly MyDefinitionId ContractTypeId_Search = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Find");
    private static readonly MyDefinitionId ContractTypeId_Bounty = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Hunt");
    private static readonly MyDefinitionId ContractTypeId_Repair = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Repair");
    private Dictionary<MyDefinitionId, MyContractTypeBaseStrategy> m_contractTypeStrategies = new Dictionary<MyDefinitionId, MyContractTypeBaseStrategy>();
    private MySessionComponentContractSystemDefinition m_definition;
    private int m_updateTimer;
    private int m_updatePeriod = 100;
    private Queue<MySessionComponentContractSystem.MyContractStateChanged> m_pendingContractChanges = new Queue<MySessionComponentContractSystem.MyContractStateChanged>();
    private Dictionary<long, MyContract> m_inactiveContracts = new Dictionary<long, MyContract>();
    private Dictionary<long, MyContract> m_activeContracts = new Dictionary<long, MyContract>();
    private MyMinimalPriceCalculator m_minimalPriceCalculator = new MyMinimalPriceCalculator();
    private Dictionary<MyDefinitionId, Dictionary<MyDefinitionId, float>> m_contractChanceCache = new Dictionary<MyDefinitionId, Dictionary<MyDefinitionId, float>>();
    private bool m_isContractChanceCacheInitialized;

    public Func<long, long, bool> CustomFinishCondition { get; set; }

    public Func<long, long, MyActivationCustomResults> CustomCanActivateContract { get; set; }

    public Func<long, bool> CustomNeedsUpdate { get; set; }

    public event MyContractConditionDelegate CustomConditionFinished;

    public event MyContractActivateDelegate CustomActivateContract;

    public event MyContractFailedDelegate CustomFailFor;

    public event MyContractFinishedDelegate CustomFinishFor;

    public event MyContractChangeDelegate CustomFinish;

    public event MyContractChangeDelegate CustomFail;

    public event MyContractChangeDelegate CustomCleanUp;

    public event MyContractChangeDelegate CustomTimeRanOut;

    public event MyContractUpdateDelegate CustomUpdate;

    public int GetContractLimitPerPlayer() => 20;

    public int GetContractCreationLimitPerPlayer() => 20;

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      this.m_inactiveContracts.Clear();
      this.m_activeContracts.Clear();
      if (!(sessionComponent is MyObjectBuilder_SessionComponentContractSystem componentContractSystem) || !Sync.IsServer)
        return;
      if (componentContractSystem.InactiveContracts != null)
      {
        foreach (MyObjectBuilder_Contract inactiveContract in (List<MyObjectBuilder_Contract>) componentContractSystem.InactiveContracts)
          this.AddContract(MyContractFactory.CreateInstance(inactiveContract));
      }
      if (componentContractSystem.ActiveContracts == null)
        return;
      foreach (MyObjectBuilder_Contract activeContract in (List<MyObjectBuilder_Contract>) componentContractSystem.ActiveContracts)
        this.AddContract(MyContractFactory.CreateInstance(activeContract));
    }

    private void UpdateActiveGpss(MyStation station)
    {
      foreach (KeyValuePair<long, MyContract> activeContract in this.m_activeContracts)
      {
        if (activeContract.Value.ContractCondition != null && activeContract.Value.ContractCondition.StationEndId == station.Id)
          activeContract.Value.ReshareConditionWithAll();
      }
    }

    private float GetContractChance(MyDefinitionId factionTypeId, MyDefinitionId contractTypeId)
    {
      this.InitializeContractChanceCache();
      if (!this.m_contractChanceCache.ContainsKey(factionTypeId))
        return 0.0f;
      Dictionary<MyDefinitionId, float> dictionary = this.m_contractChanceCache[factionTypeId];
      return !dictionary.ContainsKey(contractTypeId) ? 0.0f : dictionary[contractTypeId];
    }

    private void InitializeContractChanceCache()
    {
      if (this.m_isContractChanceCacheInitialized)
        return;
      foreach (KeyValuePair<MyDefinitionId, MyContractTypeDefinition> contractTypeDefinition in MyDefinitionManager.Static.GetContractTypeDefinitions())
      {
        if (contractTypeDefinition.Value.ChancesPerFactionType != null)
        {
          foreach (KeyValuePair<SerializableDefinitionId, float> keyValuePair in contractTypeDefinition.Value.ChancesPerFactionType)
          {
            if (!this.m_contractChanceCache.ContainsKey((MyDefinitionId) keyValuePair.Key))
              this.m_contractChanceCache.Add((MyDefinitionId) keyValuePair.Key, new Dictionary<MyDefinitionId, float>());
            this.m_contractChanceCache[(MyDefinitionId) keyValuePair.Key].Add(contractTypeDefinition.Value.Id, keyValuePair.Value);
          }
        }
      }
      this.m_isContractChanceCacheInitialized = true;
    }

    private void FailContractsForBlock(long entityId, bool punishOwner = true)
    {
      MyContractBlock entityById1 = Sandbox.Game.Entities.MyEntities.GetEntityById(entityId, true) as MyContractBlock;
      List<long> longList = new List<long>();
      foreach (KeyValuePair<long, MyContract> inactiveContract in this.m_inactiveContracts)
      {
        if (inactiveContract.Value.StartBlock > 0L && inactiveContract.Value.StartBlock == entityId)
        {
          inactiveContract.Value.RefundRewardOnDelete(entityById1);
          longList.Add(inactiveContract.Value.Id);
        }
        else if (inactiveContract.Value.IsPlayerMade && inactiveContract.Value.ContractCondition != null && (inactiveContract.Value.ContractCondition.BlockEndId != 0L && inactiveContract.Value.ContractCondition.BlockEndId == entityId))
        {
          MyContractBlock entityById2 = Sandbox.Game.Entities.MyEntities.GetEntityById(inactiveContract.Value.StartBlock) as MyContractBlock;
          inactiveContract.Value.RefundRewardOnDelete(entityById2);
          longList.Add(inactiveContract.Value.Id);
        }
      }
      foreach (long key in longList)
        this.m_inactiveContracts.Remove(key);
      foreach (KeyValuePair<long, MyContract> activeContract in this.m_activeContracts)
      {
        if (activeContract.Value.IsPlayerMade && activeContract.Value.ContractCondition != null && (activeContract.Value.ContractCondition.BlockEndId != 0L && activeContract.Value.ContractCondition.BlockEndId == entityId))
          activeContract.Value.Fail(punishOwner: false);
      }
    }

    public override void InitFromDefinition(MySessionComponentDefinition definition)
    {
      base.InitFromDefinition(definition);
      this.m_definition = definition as MySessionComponentContractSystemDefinition;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_SessionComponentContractSystem objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_SessionComponentContractSystem;
      objectBuilder.InactiveContracts = new MySerializableList<MyObjectBuilder_Contract>();
      objectBuilder.ActiveContracts = new MySerializableList<MyObjectBuilder_Contract>();
      foreach (KeyValuePair<long, MyContract> inactiveContract in this.m_inactiveContracts)
        objectBuilder.InactiveContracts.Add(inactiveContract.Value.GetObjectBuilder());
      foreach (KeyValuePair<long, MyContract> activeContract in this.m_activeContracts)
        objectBuilder.ActiveContracts.Add(activeContract.Value.GetObjectBuilder());
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public override void BeforeStart()
    {
      this.m_contractTypeStrategies.Add(MySessionComponentContractSystem.ContractTypeId_Hauling, (MyContractTypeBaseStrategy) new MyContractTypeHaulingStrategy(MySession.Static.GetComponent<MySessionComponentEconomy>().EconomyDefinition));
      this.m_contractTypeStrategies.Add(MySessionComponentContractSystem.ContractTypeId_Acquisition, (MyContractTypeBaseStrategy) new MyContractTypeAcquisitionStrategy(MySession.Static.GetComponent<MySessionComponentEconomy>().EconomyDefinition));
      this.m_contractTypeStrategies.Add(MySessionComponentContractSystem.ContractTypeId_Escort, (MyContractTypeBaseStrategy) new MyContractTypeEscortStrategy(MySession.Static.GetComponent<MySessionComponentEconomy>().EconomyDefinition));
      this.m_contractTypeStrategies.Add(MySessionComponentContractSystem.ContractTypeId_Search, (MyContractTypeBaseStrategy) new MyContractTypeSearchStrategy(MySession.Static.GetComponent<MySessionComponentEconomy>().EconomyDefinition));
      this.m_contractTypeStrategies.Add(MySessionComponentContractSystem.ContractTypeId_Bounty, (MyContractTypeBaseStrategy) new MyContractTypeBountyStrategy(MySession.Static.GetComponent<MySessionComponentEconomy>().EconomyDefinition));
      this.m_contractTypeStrategies.Add(MySessionComponentContractSystem.ContractTypeId_Repair, (MyContractTypeBaseStrategy) new MyContractTypeRepairStrategy(MySession.Static.GetComponent<MySessionComponentEconomy>().EconomyDefinition));
      foreach (KeyValuePair<long, MyContract> inactiveContract in this.m_inactiveContracts)
        inactiveContract.Value.BeforeStart();
      foreach (KeyValuePair<long, MyContract> activeContract in this.m_activeContracts)
        activeContract.Value.BeforeStart();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (!MySession.Static.IsServer)
        return;
      --this.m_updateTimer;
      if (this.m_updateTimer <= 0)
      {
        this.m_updateTimer = this.m_updatePeriod;
        MyTimeSpan currentTime = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
        foreach (KeyValuePair<long, MyContract> activeContract in this.m_activeContracts)
        {
          if (activeContract.Value.NeedsUpdate)
            activeContract.Value.Update(currentTime);
        }
      }
      while (this.m_pendingContractChanges.Count > 0)
        this.ProcessContractStateChanges(this.m_pendingContractChanges.Dequeue());
    }

    internal void StationGridSpawned(MyStation station) => this.UpdateActiveGpss(station);

    internal void CleanOldContracts()
    {
      MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      List<long> longList = new List<long>();
      foreach (KeyValuePair<long, MyContract> inactiveContract in this.m_inactiveContracts)
      {
        if (!inactiveContract.Value.IsPlayerMade)
        {
          int? ticksToDiscard = inactiveContract.Value.TicksToDiscard;
          if (ticksToDiscard.HasValue)
          {
            inactiveContract.Value.DecreaseTicksToDiscard();
            ticksToDiscard = inactiveContract.Value.TicksToDiscard;
            int num = 0;
            if (ticksToDiscard.GetValueOrDefault() <= num & ticksToDiscard.HasValue)
              longList.Add(inactiveContract.Key);
          }
        }
      }
      foreach (long key in longList)
        this.m_inactiveContracts.Remove(key);
    }

    internal void CreateContractsForStation(
      MyContractGenerator cGen,
      MyFaction faction,
      MyStation station,
      int currentContractCount,
      ref List<MyContract> existingContracts)
    {
      MyFactionTypeDefinition definition;
      switch (faction.FactionType)
      {
        case MyFactionTypes.None:
          return;
        case MyFactionTypes.PlayerMade:
          return;
        case MyFactionTypes.Miner:
          definition = MyDefinitionManager.Static.GetDefinition<MyFactionTypeDefinition>(MySessionComponentContractSystem.FactionTypeId_Miner);
          break;
        case MyFactionTypes.Trader:
          definition = MyDefinitionManager.Static.GetDefinition<MyFactionTypeDefinition>(MySessionComponentContractSystem.FactionTypeId_Trader);
          break;
        case MyFactionTypes.Builder:
          definition = MyDefinitionManager.Static.GetDefinition<MyFactionTypeDefinition>(MySessionComponentContractSystem.FactionTypeId_Builder);
          break;
        default:
          return;
      }
      if (currentContractCount >= definition.MaxContractCount)
        return;
      int num1 = definition.MaxContractCount - currentContractCount;
      MyTimeSpan now = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      DictionaryReader<MyDefinitionId, MyContractTypeDefinition> contractTypeDefinitions = MyDefinitionManager.Static.GetContractTypeDefinitions();
      List<float> floatList = new List<float>();
      int num2 = 0;
      foreach (KeyValuePair<MyDefinitionId, MyContractTypeDefinition> keyValuePair in contractTypeDefinitions)
      {
        float num3 = num2 > 0 ? floatList[num2 - 1] : 0.0f;
        if (this.m_contractTypeStrategies.ContainsKey(keyValuePair.Key))
        {
          MyContractTypeBaseStrategy contractTypeStrategy = this.m_contractTypeStrategies[keyValuePair.Key];
          if (contractTypeStrategy != null && contractTypeStrategy.CanBeGenerated(station, faction))
            num3 += this.GetContractChance(definition.Id, keyValuePair.Key);
        }
        floatList.Add(num3);
        ++num2;
      }
      float num4 = floatList.Count > 0 ? floatList[floatList.Count - 1] : 0.0f;
      int creationTriesMax = MySessionComponentContractSystem.CONTRACT_CREATION_TRIES_MAX;
      while (creationTriesMax > 0 && num1 > 0)
      {
        float num3 = MyRandom.Instance.NextFloat() * num4;
        MyContract contract = (MyContract) null;
        MyContractCreationResults contractCreationResults = MyContractCreationResults.Error;
        int index = 0;
        foreach (KeyValuePair<MyDefinitionId, MyContractTypeDefinition> keyValuePair in contractTypeDefinitions)
        {
          if ((double) num3 < (double) floatList[index])
          {
            contractCreationResults = this.m_contractTypeStrategies[keyValuePair.Key].GenerateContract(out contract, faction.FactionId, station.Id, this.m_minimalPriceCalculator, now);
            break;
          }
          ++index;
        }
        if (contractCreationResults == MyContractCreationResults.Success && this.CheckContractAgainstOther(contract, existingContracts))
        {
          this.AddContract(contract);
          existingContracts.Add(contract);
          --num1;
        }
        else
          --creationTriesMax;
      }
    }

    internal bool CheckContractAgainstOther(MyContract testedContract, List<MyContract> contracts)
    {
      switch (testedContract)
      {
        case MyContractHunt _:
          MyContractHunt myContractHunt1 = testedContract as MyContractHunt;
          foreach (MyContract contract in contracts)
          {
            if (contract is MyContractHunt myContractHunt2 && myContractHunt1.Target == myContractHunt2.Target)
              return false;
          }
          return true;
        case MyContractObtainAndDeliver _:
          MyContractObtainAndDeliver obtainAndDeliver1 = testedContract as MyContractObtainAndDeliver;
          foreach (MyContract contract in contracts)
          {
            if (contract is MyContractObtainAndDeliver obtainAndDeliver2)
            {
              MyDefinitionId? nullable1 = new MyDefinitionId?();
              MyDefinitionId? nullable2 = new MyDefinitionId?();
              nullable1 = obtainAndDeliver1.GetItemId();
              nullable2 = obtainAndDeliver2.GetItemId();
              if (nullable1.HasValue && nullable2.HasValue && nullable1.Value == nullable2.Value)
                return false;
            }
          }
          return true;
        default:
          return true;
      }
    }

    internal void GetAvailableContractCountsByStation(
      ref Dictionary<long, int> counts,
      ref Dictionary<long, List<MyContract>> lists)
    {
      foreach (KeyValuePair<long, MyContract> inactiveContract in this.m_inactiveContracts)
      {
        if (inactiveContract.Value.StartStation > 0L)
        {
          if (!lists.ContainsKey(inactiveContract.Value.StartStation))
            lists.Add(inactiveContract.Value.StartStation, new List<MyContract>());
          lists[inactiveContract.Value.StartStation].Add(inactiveContract.Value);
          if (!counts.ContainsKey(inactiveContract.Value.StartStation))
            counts.Add(inactiveContract.Value.StartStation, 0);
          counts[inactiveContract.Value.StartStation] = counts[inactiveContract.Value.StartStation] + 1;
        }
      }
    }

    internal void ContractBlockDestroyed(long entityId) => this.FailContractsForBlock(entityId, false);

    internal MyContractResults ActivateContract(
      long identityId,
      long contractId,
      long stationId,
      long blockId)
    {
      if (!this.m_inactiveContracts.ContainsKey(contractId))
        return MyContractResults.Fail_ContractNotFound_Activation;
      MyContract inactiveContract = this.m_inactiveContracts[contractId];
      if (inactiveContract.StartStation != stationId && inactiveContract.StartBlock != blockId)
        return MyContractResults.Error_InvalidData;
      switch (inactiveContract.CanActivate(identityId))
      {
        case MyActivationResults.Success:
          if (!inactiveContract.Activate(identityId, MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds)))
            return MyContractResults.Error_Unknown;
          this.m_inactiveContracts.Remove(inactiveContract.Id);
          this.m_activeContracts.Add(inactiveContract.Id, inactiveContract);
          return MyContractResults.Success;
        case MyActivationResults.Fail_InsufficientFunds:
          return MyContractResults.Fail_ActivationConditionsNotMet_InsufficientFunds;
        case MyActivationResults.Fail_InsufficientInventorySpace:
          return MyContractResults.Fail_ActivationConditionsNotMet_InsufficientSpace;
        case MyActivationResults.Fail_ContractLimitReachedHard:
          return MyContractResults.Fail_ActivationConditionsNotMet_ContractLimitReachedHard;
        case MyActivationResults.Fail_TargetNotOnline:
          return MyContractResults.Fail_ActivationConditionsNotMet_TargetOffline;
        case MyActivationResults.Fail_YouAreTargetOfThisHunt:
          return MyContractResults.Fail_ActivationConditionsNotMet_YouAreTargetOfThisHunt;
        default:
          return MyContractResults.Fail_ActivationConditionsNotMet;
      }
    }

    internal MyContractResults AbandonContract(long identityId, long contractId)
    {
      if (!this.m_activeContracts.ContainsKey(contractId))
        return MyContractResults.Fail_ContractNotFound_Abandon;
      MyContract activeContract = this.m_activeContracts[contractId];
      if (!activeContract.Owners.Contains(identityId))
        return MyContractResults.Error_InvalidData;
      activeContract.Abandon(identityId);
      return MyContractResults.Success;
    }

    internal MyContractResults FinishContractCondition(
      long identityId,
      MyContract contract,
      MyContractCondition condition,
      long targetEntityId)
    {
      if (!this.m_activeContracts.ContainsKey(condition.ContractId))
        return MyContractResults.Fail_ContractNotFound_Finish;
      switch (condition)
      {
        case MyContractConditionDeliverPackage _:
          MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
          if (identity == null)
          {
            MyLog.Default.WriteToLogAndAssert("MyContractBlock - identity not found");
            return MyContractResults.Error_MissingKeyStructure;
          }
          MyCharacter character = identity.Character;
          if (character == null)
          {
            MyLog.Default.WriteToLogAndAssert("MyContractBlock - character not found");
            return MyContractResults.Error_MissingKeyStructure;
          }
          if (character.InventoryCount <= 0)
          {
            MyLog.Default.WriteToLogAndAssert("MyContractBlock - no character inventory");
            return MyContractResults.Error_MissingKeyStructure;
          }
          MyInventoryBase inventoryBase = character.GetInventoryBase();
          List<MyPhysicalInventoryItem> items = inventoryBase.GetItems();
          MyPhysicalInventoryItem? nullable = new MyPhysicalInventoryItem?();
          foreach (MyPhysicalInventoryItem physicalInventoryItem in items)
          {
            if (physicalInventoryItem.Content is MyObjectBuilder_Package content && content.ContractId == condition.ContractId && content.ContractConditionId == condition.Id)
            {
              nullable = new MyPhysicalInventoryItem?(physicalInventoryItem);
              break;
            }
          }
          if (!nullable.HasValue)
            return MyContractResults.Fail_FinishConditionsNotMet_MissingPackage;
          inventoryBase.Remove((VRage.Game.ModAPI.Ingame.IMyInventoryItem) nullable, (MyFixedPoint) 1);
          condition.FinalizeCondition();
          return MyContractResults.Success;
        case MyContractConditionDeliverItems conditionDeliverItems:
          if (targetEntityId == 0L || Sandbox.Game.Entities.MyEntities.GetEntityById(targetEntityId) == null)
            return MyContractResults.Fail_FinishConditionsNotMet_IncorrectTargetEntity;
          MySessionComponentContractSystem.MyTransferItemsFromGridResults itemsFromGridResults;
          if (conditionDeliverItems.TransferItems)
          {
            MyStation stationByStationId = MySession.Static.Factions.GetStationByStationId(conditionDeliverItems.StationEndId);
            MyContractBlock targetContractBlock = (MyContractBlock) null;
            if (stationByStationId != null)
            {
              if (Sandbox.Game.Entities.MyEntities.GetEntityById(stationByStationId.StationEntityId) is MyCubeGrid entityById)
                targetContractBlock = entityById.GetFirstBlockOfType<MyContractBlock>();
            }
            else
              targetContractBlock = Sandbox.Game.Entities.MyEntities.GetEntityById(conditionDeliverItems.BlockEndId) as MyContractBlock;
            if (targetContractBlock == null)
            {
              MyLog.Default.WriteToLogAndAssert("Acquisition contract - Cannot transfer as there is nowhere to transfer to (target lacks Contract block)");
              return MyContractResults.Error_MissingKeyStructure;
            }
            itemsFromGridResults = this.TransferItemsFromEntity(conditionDeliverItems.ItemType, conditionDeliverItems.ItemAmount, targetEntityId, targetContractBlock);
          }
          else
            itemsFromGridResults = this.DeleteItemsFromEntity(conditionDeliverItems.ItemType, conditionDeliverItems.ItemAmount, targetEntityId);
          if (itemsFromGridResults == MySessionComponentContractSystem.MyTransferItemsFromGridResults.Success)
          {
            condition.FinalizeCondition();
            return MyContractResults.Success;
          }
          switch (itemsFromGridResults)
          {
            case MySessionComponentContractSystem.MyTransferItemsFromGridResults.Fail_NoAccess:
              return MyContractResults.Fail_CannotAccess;
            case MySessionComponentContractSystem.MyTransferItemsFromGridResults.Fail_NotEnoughItems:
              return MyContractResults.Fail_FinishConditionsNotMet_NotEnoughItems;
            case MySessionComponentContractSystem.MyTransferItemsFromGridResults.Fail_NotEnoughSpace:
              return MyContractResults.Fail_FinishConditionsNotMet_NotEnoughSpace;
            case MySessionComponentContractSystem.MyTransferItemsFromGridResults.Error_MissingKeyStructures:
              return MyContractResults.Error_MissingKeyStructure;
            default:
              return MyContractResults.Error_Unknown;
          }
        case MyContractConditionCustom contractConditionCustom:
          bool flag = true;
          if (this.CustomFinishCondition != null)
            flag |= this.CustomFinishCondition(condition.Id, condition.ContractId);
          if (!flag)
            return MyContractResults.Fail_NotPossible;
          contractConditionCustom.FinalizeCondition();
          if (this.CustomConditionFinished != null)
            this.CustomConditionFinished(condition.Id, condition.ContractId);
          return MyContractResults.Success;
        default:
          return MyContractResults.Error_Unknown;
      }
    }

    internal MyContractCreationResults GenerateCustomContract_Deliver(
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long targetBlockId,
      out long contractId,
      out long contractConditionId)
    {
      contractId = 0L;
      contractConditionId = 0L;
      MySessionComponentEconomy component = MySession.Static?.GetComponent<MySessionComponentEconomy>();
      if (component == null)
        return MyContractCreationResults.Error_MissingKeyStructure;
      MyContractGenerator contractGenerator = new MyContractGenerator(component.EconomyDefinition);
      MyTimeSpan now = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      if (MyBankingSystem.GetBalance(startBlock.OwnerId) < (long) rewardMoney)
        return MyContractCreationResults.Fail_NotEnoughFunds;
      MyContract contract;
      int customHaulingContract = (int) contractGenerator.CreateCustomHaulingContract(out contract, startBlock, rewardMoney, startingDeposit, durationInMin, targetBlockId, now);
      if (customHaulingContract != 0)
        return (MyContractCreationResults) customHaulingContract;
      this.AddContract(contract);
      contractId = contract.Id;
      contractConditionId = contract.ContractCondition != null ? contract.ContractCondition.Id : 0L;
      MyBankingSystem.ChangeBalance(startBlock.OwnerId, (long) -rewardMoney);
      return (MyContractCreationResults) customHaulingContract;
    }

    internal MyContractCreationResults GenerateCustomContract_ObtainAndDeliver(
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long targetBlockId,
      MyDefinitionId itemTypeId,
      int itemAmount,
      out long contractId,
      out long contractConditionId)
    {
      contractId = 0L;
      contractConditionId = 0L;
      MySessionComponentEconomy component = MySession.Static?.GetComponent<MySessionComponentEconomy>();
      if (component == null)
        return MyContractCreationResults.Error_MissingKeyStructure;
      MyContractGenerator contractGenerator = new MyContractGenerator(component.EconomyDefinition);
      MyTimeSpan now = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      if (MyBankingSystem.GetBalance(startBlock.OwnerId) < (long) rewardMoney)
        return MyContractCreationResults.Fail_NotEnoughFunds;
      MyContract contract;
      int acquisitionContract = (int) contractGenerator.CreateCustomAcquisitionContract(out contract, startBlock, rewardMoney, startingDeposit, durationInMin, targetBlockId, itemTypeId, itemAmount, now);
      if (acquisitionContract != 0)
        return (MyContractCreationResults) acquisitionContract;
      this.AddContract(contract);
      contractId = contract.Id;
      contractConditionId = contract.ContractCondition != null ? contract.ContractCondition.Id : 0L;
      MyBankingSystem.ChangeBalance(startBlock.OwnerId, (long) -rewardMoney);
      return (MyContractCreationResults) acquisitionContract;
    }

    internal MyContractCreationResults GenerateCustomContract_Find(
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long targetGridId,
      double searchRadius,
      out long contractId,
      out long contractConditionId)
    {
      contractId = 0L;
      contractConditionId = 0L;
      MySessionComponentEconomy component = MySession.Static?.GetComponent<MySessionComponentEconomy>();
      if (component == null)
        return MyContractCreationResults.Error_MissingKeyStructure;
      MyContractGenerator contractGenerator = new MyContractGenerator(component.EconomyDefinition);
      MyTimeSpan now = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      if (MyBankingSystem.GetBalance(startBlock.OwnerId) < (long) rewardMoney)
        return MyContractCreationResults.Fail_NotEnoughFunds;
      MyContract contract;
      int customSearchContract = (int) contractGenerator.CreateCustomSearchContract(out contract, startBlock, rewardMoney, startingDeposit, durationInMin, targetGridId, searchRadius, now);
      if (customSearchContract != 0)
        return (MyContractCreationResults) customSearchContract;
      this.AddContract(contract);
      contractId = contract.Id;
      contractConditionId = contract.ContractCondition != null ? contract.ContractCondition.Id : 0L;
      MyBankingSystem.ChangeBalance(startBlock.OwnerId, (long) -rewardMoney);
      return (MyContractCreationResults) customSearchContract;
    }

    internal MyContractCreationResults GenerateCustomContract_Escort(
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      Vector3D startPoint,
      Vector3D endPoint,
      long owner,
      out long contractId,
      out long contractConditionId)
    {
      contractId = 0L;
      contractConditionId = 0L;
      MySessionComponentEconomy component = MySession.Static?.GetComponent<MySessionComponentEconomy>();
      if (component == null)
        return MyContractCreationResults.Error_MissingKeyStructure;
      MyContractGenerator contractGenerator = new MyContractGenerator(component.EconomyDefinition);
      MyTimeSpan now = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      if (MyBankingSystem.GetBalance(startBlock.OwnerId) < (long) rewardMoney)
        return MyContractCreationResults.Fail_NotEnoughFunds;
      MyContract contract;
      int customEscortContract = (int) contractGenerator.CreateCustomEscortContract(out contract, startBlock, rewardMoney, startingDeposit, durationInMin, startPoint, endPoint, owner, now);
      if (customEscortContract != 0)
        return (MyContractCreationResults) customEscortContract;
      this.AddContract(contract);
      contractId = contract.Id;
      contractConditionId = contract.ContractCondition != null ? contract.ContractCondition.Id : 0L;
      MyBankingSystem.ChangeBalance(startBlock.OwnerId, (long) -rewardMoney);
      return (MyContractCreationResults) customEscortContract;
    }

    internal MyContractCreationResults GenerateCustomContract_Hunt(
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long targetIdentityId,
      out long contractId,
      out long contractConditionId)
    {
      contractId = 0L;
      contractConditionId = 0L;
      MySessionComponentEconomy component = MySession.Static?.GetComponent<MySessionComponentEconomy>();
      if (component == null)
        return MyContractCreationResults.Error_MissingKeyStructure;
      MyContractGenerator contractGenerator = new MyContractGenerator(component.EconomyDefinition);
      MyTimeSpan now = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      if (MyBankingSystem.GetBalance(startBlock.OwnerId) < (long) rewardMoney)
        return MyContractCreationResults.Fail_NotEnoughFunds;
      MyContract contract;
      int customBountyContract = (int) contractGenerator.CreateCustomBountyContract(out contract, startBlock, rewardMoney, startingDeposit, durationInMin, targetIdentityId, now);
      if (customBountyContract != 0)
        return (MyContractCreationResults) customBountyContract;
      this.AddContract(contract);
      contractId = contract.Id;
      contractConditionId = contract.ContractCondition != null ? contract.ContractCondition.Id : 0L;
      MyBankingSystem.ChangeBalance(startBlock.OwnerId, (long) -rewardMoney);
      return (MyContractCreationResults) customBountyContract;
    }

    internal MyContractCreationResults GenerateCustomContract_Repair(
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long gridId,
      out long contractId,
      out long contractConditionId)
    {
      contractId = 0L;
      contractConditionId = 0L;
      MySessionComponentEconomy component = MySession.Static?.GetComponent<MySessionComponentEconomy>();
      if (component == null)
        return MyContractCreationResults.Error_MissingKeyStructure;
      MyContractGenerator contractGenerator = new MyContractGenerator(component.EconomyDefinition);
      MyTimeSpan now = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      if (MyBankingSystem.GetBalance(startBlock.OwnerId) < (long) rewardMoney)
        return MyContractCreationResults.Fail_NotEnoughFunds;
      MyContract contract;
      int customRepairContract = (int) contractGenerator.CreateCustomRepairContract(out contract, startBlock, rewardMoney, startingDeposit, durationInMin, gridId, now);
      if (customRepairContract != 0)
        return (MyContractCreationResults) customRepairContract;
      this.AddContract(contract);
      contractId = contract.Id;
      contractConditionId = contract.ContractCondition != null ? contract.ContractCondition.Id : 0L;
      MyBankingSystem.ChangeBalance(startBlock.OwnerId, (long) -rewardMoney);
      return (MyContractCreationResults) customRepairContract;
    }

    internal MyContractCreationResults GenerateCustomContract_Custom(
      MyDefinitionId definitionId,
      string name,
      string description,
      MyContractBlock startBlock,
      int rewardMoney,
      int startingDeposit,
      int reputationReward,
      int failReputationPrice,
      int durationInMin,
      out long contractId,
      out long contractConditionId,
      MyContractBlock endBlock = null)
    {
      contractId = 0L;
      contractConditionId = 0L;
      MySessionComponentEconomy component = MySession.Static?.GetComponent<MySessionComponentEconomy>();
      if (component == null)
        return MyContractCreationResults.Error_MissingKeyStructure;
      MyContractGenerator contractGenerator = new MyContractGenerator(component.EconomyDefinition);
      MyTimeSpan now = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      if (MyBankingSystem.GetBalance(startBlock.OwnerId) < (long) rewardMoney)
        return MyContractCreationResults.Fail_NotEnoughFunds;
      MyContract contract;
      int customCustomContract = (int) contractGenerator.CreateCustomCustomContract(out contract, definitionId, name, description, startBlock, rewardMoney, startingDeposit, reputationReward, failReputationPrice, durationInMin, now, endBlock);
      if (customCustomContract != 0)
        return (MyContractCreationResults) customCustomContract;
      this.AddContract(contract);
      contractId = contract.Id;
      contractConditionId = contract.ContractCondition != null ? contract.ContractCondition.Id : 0L;
      MyBankingSystem.ChangeBalance(startBlock.OwnerId, (long) -rewardMoney);
      return (MyContractCreationResults) customCustomContract;
    }

    internal bool DeleteCustomContract(MyContractBlock startBlock, long contractId)
    {
      MyContract inactiveContractById = this.GetInactiveContractById(contractId);
      if (inactiveContractById == null || inactiveContractById.State != MyContractStateEnum.Inactive || (!inactiveContractById.IsPlayerMade || inactiveContractById.StartStation != 0L) || inactiveContractById.StartBlock == 0L)
        return false;
      inactiveContractById.RefundRewardOnDelete(startBlock);
      this.RemoveInactiveContract(contractId);
      return true;
    }

    internal void OnCustomActivateContract(long contractId, long identityId)
    {
      MyContractActivateDelegate activateContract = this.CustomActivateContract;
      if (activateContract == null)
        return;
      activateContract(contractId, identityId);
    }

    internal void OnCustomFailFor(long contractId, long identityId, bool isAbandon)
    {
      MyContractFailedDelegate customFailFor = this.CustomFailFor;
      if (customFailFor == null)
        return;
      customFailFor(contractId, identityId, isAbandon);
    }

    internal void OnCustomFinishFor(long contractId, long identityId, int rewardeeCount)
    {
      MyContractFinishedDelegate customFinishFor = this.CustomFinishFor;
      if (customFinishFor == null)
        return;
      customFinishFor(contractId, identityId, rewardeeCount);
    }

    internal void OnCustomFinish(long contractId)
    {
      MyContractChangeDelegate customFinish = this.CustomFinish;
      if (customFinish == null)
        return;
      customFinish(contractId);
    }

    internal void OnCustomFail(long contractId)
    {
      MyContractChangeDelegate customFail = this.CustomFail;
      if (customFail == null)
        return;
      customFail(contractId);
    }

    internal void OnCustomCleanUp(long contractId)
    {
      MyContractChangeDelegate customCleanUp = this.CustomCleanUp;
      if (customCleanUp == null)
        return;
      customCleanUp(contractId);
    }

    internal void OnCustomTimeRanOut(long contractId)
    {
      MyContractChangeDelegate customTimeRanOut = this.CustomTimeRanOut;
      if (customTimeRanOut == null)
        return;
      customTimeRanOut(contractId);
    }

    internal void OnCustomUpdate(
      long contractId,
      MyCustomContractStateEnum state,
      MyTimeSpan currentTime)
    {
      MyContractUpdateDelegate customUpdate = this.CustomUpdate;
      if (customUpdate == null)
        return;
      customUpdate(contractId, state, currentTime);
    }

    private MySessionComponentContractSystem.MyTransferItemsFromGridResults TransferItemsFromEntity(
      MyDefinitionId id,
      int amount,
      long entityId,
      MyContractBlock targetContractBlock)
    {
      if (targetContractBlock == null)
        return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Error_MissingKeyStructures;
      MyEntity entityById = Sandbox.Game.Entities.MyEntities.GetEntityById(entityId);
      MyCubeGrid nodeInGroup = entityById as MyCubeGrid;
      MyCharacter myCharacter = entityById as MyCharacter;
      if (nodeInGroup != null)
      {
        List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.GetGroups(GridLinkTypeEnum.Logical).GetGroupNodes(nodeInGroup);
        if (groupNodes == null || groupNodes.Count == 0)
          return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Error_MissingKeyStructures;
        List<MyInventory> myInventoryList = new List<MyInventory>();
        foreach (MyCubeGrid myCubeGrid in groupNodes)
        {
          foreach (MySlimBlock cubeBlock in myCubeGrid.CubeBlocks)
          {
            if (cubeBlock.FatBlock != null && (cubeBlock.FatBlock is MyCargoContainer || cubeBlock.FatBlock is MyShipConnector || (cubeBlock.FatBlock is MyRefinery || cubeBlock.FatBlock is MyAssembler)))
            {
              for (int index = 0; index < cubeBlock.FatBlock.InventoryCount; ++index)
                myInventoryList.Add(MyEntityExtensions.GetInventory(cubeBlock.FatBlock, index));
            }
          }
        }
        int num1 = amount;
        foreach (MyInventoryBase myInventoryBase in myInventoryList)
        {
          MyFixedPoint itemAmount = myInventoryBase.GetItemAmount(id);
          num1 -= itemAmount.ToIntSafe();
          if (num1 <= 0)
            break;
        }
        if (num1 > 0)
          return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Fail_NotEnoughItems;
        if (!targetContractBlock.CubeGrid.GridSystems.ConveyorSystem.PushGenerateItem(id, new MyFixedPoint?((MyFixedPoint) amount), (IMyConveyorEndpointBlock) targetContractBlock, false))
          return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Fail_NotEnoughSpace;
        int num2 = amount;
        foreach (MyInventory myInventory in myInventoryList)
        {
          MyFixedPoint itemAmount = myInventory.GetItemAmount(id, MyItemFlags.None, false);
          if (itemAmount < (MyFixedPoint) num2)
          {
            myInventory.RemoveItemsOfType(itemAmount, id, MyItemFlags.None, false);
            num2 -= itemAmount.ToIntSafe();
          }
          else
          {
            myInventory.RemoveItemsOfType((MyFixedPoint) num2, id, MyItemFlags.None, false);
            num2 = 0;
          }
          if (num2 <= 0)
            break;
        }
        return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Success;
      }
      if (myCharacter == null || myCharacter.InventoryCount <= 0)
        return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Error_MissingKeyStructures;
      MyInventoryBase inventoryBase = myCharacter.GetInventoryBase();
      if (inventoryBase.GetItemAmount(id) < (MyFixedPoint) amount)
        return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Fail_NotEnoughItems;
      if (!targetContractBlock.CubeGrid.GridSystems.ConveyorSystem.PushGenerateItem(id, new MyFixedPoint?((MyFixedPoint) amount), (IMyConveyorEndpointBlock) targetContractBlock, false))
        return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Fail_NotEnoughSpace;
      inventoryBase.RemoveItemsOfType((MyFixedPoint) amount, id);
      return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Success;
    }

    private MySessionComponentContractSystem.MyTransferItemsFromGridResults DeleteItemsFromEntity(
      MyDefinitionId id,
      int amount,
      long entityId)
    {
      MyEntity entityById1 = Sandbox.Game.Entities.MyEntities.GetEntityById(entityId);
      MyCubeGrid myCubeGrid1 = entityById1 as MyCubeGrid;
      MyCharacter myCharacter = entityById1 as MyCharacter;
      if (myCubeGrid1 != null)
      {
        if (!(Sandbox.Game.Entities.MyEntities.GetEntityById(entityId) is MyCubeGrid entityById2))
          return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Error_MissingKeyStructures;
        List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.GetGroups(GridLinkTypeEnum.Logical).GetGroupNodes(entityById2);
        if (groupNodes == null || groupNodes.Count == 0)
          return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Error_MissingKeyStructures;
        List<MyInventory> myInventoryList = new List<MyInventory>();
        foreach (MyCubeGrid myCubeGrid2 in groupNodes)
        {
          foreach (MySlimBlock cubeBlock in myCubeGrid2.CubeBlocks)
          {
            if (cubeBlock.FatBlock != null && (cubeBlock.FatBlock is MyCargoContainer || cubeBlock.FatBlock is MyShipConnector || (cubeBlock.FatBlock is MyRefinery || cubeBlock.FatBlock is MyAssembler)))
            {
              for (int index = 0; index < cubeBlock.FatBlock.InventoryCount; ++index)
                myInventoryList.Add(MyEntityExtensions.GetInventory(cubeBlock.FatBlock, index));
            }
          }
        }
        int num1 = amount;
        foreach (MyInventoryBase myInventoryBase in myInventoryList)
        {
          MyFixedPoint itemAmount = myInventoryBase.GetItemAmount(id);
          num1 -= itemAmount.ToIntSafe();
          if (num1 <= 0)
            break;
        }
        if (num1 > 0)
          return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Fail_NotEnoughItems;
        int num2 = amount;
        foreach (MyInventory myInventory in myInventoryList)
        {
          MyFixedPoint itemAmount = myInventory.GetItemAmount(id, MyItemFlags.None, false);
          if (itemAmount < (MyFixedPoint) num2)
          {
            myInventory.RemoveItemsOfType(itemAmount, id, MyItemFlags.None, false);
            num2 -= itemAmount.ToIntSafe();
          }
          else
          {
            myInventory.RemoveItemsOfType((MyFixedPoint) num2, id, MyItemFlags.None, false);
            num2 = 0;
          }
          if (num2 <= 0)
            break;
        }
        return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Success;
      }
      if (myCharacter == null || myCharacter.InventoryCount <= 0)
        return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Error_MissingKeyStructures;
      MyInventoryBase inventoryBase = myCharacter.GetInventoryBase();
      if (inventoryBase.GetItemAmount(id) < (MyFixedPoint) amount)
        return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Fail_NotEnoughItems;
      inventoryBase.RemoveItemsOfType((MyFixedPoint) amount, id);
      return MySessionComponentContractSystem.MyTransferItemsFromGridResults.Success;
    }

    internal MyContractResults FinishContract(long identityId, MyContract contract)
    {
      if (!this.m_activeContracts.ContainsKey(contract.Id))
        return MyContractResults.Fail_ContractNotFound_Finish;
      if (!contract.Owners.Contains(identityId))
        return MyContractResults.Fail_CannotAccess;
      int num = (int) contract.Finish();
      return MyContractResults.Success;
    }

    internal List<MyObjectBuilder_Contract> GetActiveContractsForPlayer_OB(
      long identityId)
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      if (identity == null)
        return (List<MyObjectBuilder_Contract>) null;
      List<MyObjectBuilder_Contract> objectBuilderContractList = new List<MyObjectBuilder_Contract>();
      foreach (long activeContract1 in identity.ActiveContracts)
      {
        if (this.m_activeContracts.ContainsKey(activeContract1))
        {
          MyContract activeContract2 = this.m_activeContracts[activeContract1];
          if (activeContract2.State == MyContractStateEnum.Active)
            objectBuilderContractList.Add(activeContract2.GetObjectBuilder());
        }
      }
      return objectBuilderContractList;
    }

    internal List<MyObjectBuilder_Contract> GetAvailableContractsForStation_OB(
      long stationId)
    {
      List<MyObjectBuilder_Contract> objectBuilderContractList = new List<MyObjectBuilder_Contract>();
      foreach (KeyValuePair<long, MyContract> inactiveContract in this.m_inactiveContracts)
      {
        if (inactiveContract.Value.StartStation == stationId && inactiveContract.Value.State == MyContractStateEnum.Inactive)
          objectBuilderContractList.Add(inactiveContract.Value.GetObjectBuilder());
      }
      return objectBuilderContractList;
    }

    internal List<MyObjectBuilder_Contract> GetAvailableContractsForBlock_OB(
      long blockId)
    {
      List<MyObjectBuilder_Contract> objectBuilderContractList = new List<MyObjectBuilder_Contract>();
      foreach (KeyValuePair<long, MyContract> inactiveContract in this.m_inactiveContracts)
      {
        if (inactiveContract.Value.StartBlock == blockId && inactiveContract.Value.State == MyContractStateEnum.Inactive)
          objectBuilderContractList.Add(inactiveContract.Value.GetObjectBuilder());
      }
      return objectBuilderContractList;
    }

    private void AddContract(MyContract contract) => this.AddContract(contract, contract.State);

    private void AddContract(MyContract contract, MyContractStateEnum state)
    {
      if (this.m_activeContracts.ContainsKey(contract.Id) || this.m_inactiveContracts.ContainsKey(contract.Id))
        MyLog.Default.WriteToLogAndAssert("ContractSystem - Adding Contract that have already been added!!! There is something seriously wrong.");
      switch (state)
      {
        case MyContractStateEnum.Inactive:
          this.m_inactiveContracts.Add(contract.Id, contract);
          contract.ContractChangedState += new Action<long, MyContractStateEnum, MyContractStateEnum>(this.ContractChangedState_Callback);
          break;
        case MyContractStateEnum.Active:
        case MyContractStateEnum.Finished:
        case MyContractStateEnum.Failed:
        case MyContractStateEnum.ToBeDisposed:
          this.m_activeContracts.Add(contract.Id, contract);
          contract.ContractChangedState += new Action<long, MyContractStateEnum, MyContractStateEnum>(this.ContractChangedState_Callback);
          break;
        default:
          MyLog.Default.WriteToLogAndAssert("ContractSystem - Cannot add contract with such state: " + contract.State.ToString());
          break;
      }
    }

    private void ContractChangedState_Callback(
      long id,
      MyContractStateEnum stateOld,
      MyContractStateEnum stateNew)
    {
      this.m_pendingContractChanges.Enqueue(new MySessionComponentContractSystem.MyContractStateChanged()
      {
        Id = id,
        StateOld = stateOld,
        StateNew = stateNew
      });
    }

    private void ProcessContractStateChanges(
      MySessionComponentContractSystem.MyContractStateChanged state)
    {
      MyContract contract = (MyContract) null;
      switch (state.StateOld)
      {
        case MyContractStateEnum.Inactive:
          return;
        case MyContractStateEnum.Active:
        case MyContractStateEnum.Finished:
        case MyContractStateEnum.Failed:
        case MyContractStateEnum.ToBeDisposed:
          contract = this.m_activeContracts[state.Id];
          this.RemoveActiveContract(state.Id);
          break;
        default:
          MyLog.Default.WriteToLogAndAssert("ContractSystem - contract in such state (" + state.StateOld.ToString() + " cannot have changed state as it is no longer within any collection and should not call this function.");
          break;
      }
      if (contract == null)
        return;
      switch (state.StateNew)
      {
        case MyContractStateEnum.Inactive:
        case MyContractStateEnum.Active:
        case MyContractStateEnum.Finished:
        case MyContractStateEnum.Failed:
        case MyContractStateEnum.ToBeDisposed:
          this.AddContract(contract, state.StateNew);
          break;
      }
    }

    private void RemoveContractInternal(long contractId)
    {
      this.RemoveActiveContract(contractId);
      this.RemoveInactiveContract(contractId);
    }

    private void RemoveActiveContract(long contractId)
    {
      if (!this.m_activeContracts.ContainsKey(contractId))
        return;
      MyContract activeContract = this.m_activeContracts[contractId];
      this.m_activeContracts.Remove(contractId);
      activeContract.ContractChangedState -= new Action<long, MyContractStateEnum, MyContractStateEnum>(this.ContractChangedState_Callback);
    }

    private void RemoveInactiveContract(long contractId)
    {
      if (!this.m_inactiveContracts.ContainsKey(contractId))
        return;
      MyContract inactiveContract = this.m_inactiveContracts[contractId];
      this.m_inactiveContracts.Remove(contractId);
      inactiveContract.ContractChangedState -= new Action<long, MyContractStateEnum, MyContractStateEnum>(this.ContractChangedState_Callback);
    }

    internal MyContract GetActiveContractById(long contractId) => this.m_activeContracts.ContainsKey(contractId) ? this.m_activeContracts[contractId] : (MyContract) null;

    internal MyContract GetInactiveContractById(long contractId) => this.m_inactiveContracts.ContainsKey(contractId) ? this.m_inactiveContracts[contractId] : (MyContract) null;

    public void SendNotificationToPlayer(
      MyContractNotificationTypes notifType,
      long targetIdentityId)
    {
      if (MySession.Static.LocalPlayerId == targetIdentityId)
      {
        MySessionComponentContractSystem.DisplayNotificationToPlayer_Internal(notifType);
      }
      else
      {
        ulong steamId = MySession.Static.Players.TryGetSteamId(targetIdentityId);
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyContractNotificationTypes>((Func<IMyEventOwner, Action<MyContractNotificationTypes>>) (x => new Action<MyContractNotificationTypes>(MySessionComponentContractSystem.DisplayNotificationToPlayer)), notifType, new EndpointId(steamId));
      }
    }

    [Event(null, 1519)]
    [Reliable]
    [Client]
    private static void DisplayNotificationToPlayer(MyContractNotificationTypes notifType) => MySessionComponentContractSystem.DisplayNotificationToPlayer_Internal(notifType);

    private static void DisplayNotificationToPlayer_Internal(MyContractNotificationTypes notifType)
    {
      MyHudNotification myHudNotification;
      switch (notifType)
      {
        case MyContractNotificationTypes.ContractSuccessful:
          myHudNotification = new MyHudNotification(MySpaceTexts.ContractSystem_Notifications_ContractSuccess, 3500, level: MyNotificationLevel.Important);
          break;
        case MyContractNotificationTypes.ContractFailed:
          myHudNotification = new MyHudNotification(MySpaceTexts.ContractSystem_Notifications_ContractFailed, 3500, level: MyNotificationLevel.Important);
          break;
        default:
          myHudNotification = (MyHudNotification) null;
          break;
      }
      if (myHudNotification == null)
        return;
      MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
    }

    public MyAddContractResultWrapper AddContract(IMyContract contract)
    {
      MyAddContractResultWrapper contractResultWrapper = new MyAddContractResultWrapper()
      {
        Success = false,
        ContractId = 0
      };
      switch (contract)
      {
        case null:
          return contractResultWrapper;
        case IMyContractHauling haul:
          contractResultWrapper = this.AddContract_Hauling(haul);
          break;
        case IMyContractAcquisition acqui:
          contractResultWrapper = this.AddContract_Acquisition(acqui);
          break;
        case IMyContractSearch search:
          contractResultWrapper = this.AddContract_Search(search);
          break;
        case IMyContractEscort escort:
          contractResultWrapper = this.AddContract_Escort(escort);
          break;
        case IMyContractBounty bounty:
          contractResultWrapper = this.AddContract_Bounty(bounty);
          break;
        case IMyContractRepair repair:
          contractResultWrapper = this.AddContract_Repair(repair);
          break;
        case IMyContractCustom custom:
          contractResultWrapper = this.AddContract_Custom(custom);
          break;
      }
      if (contractResultWrapper.Success && this.m_inactiveContracts.ContainsKey(contractResultWrapper.ContractId))
      {
        MyContract inactiveContract = this.m_inactiveContracts[contractResultWrapper.ContractId];
        if (contract.OnContractAcquired != null)
          inactiveContract.OnContractAcquired += contract.OnContractAcquired;
        if (contract.OnContractFailed != null)
          inactiveContract.OnContractFailed += contract.OnContractFailed;
        if (contract.OnContractSucceeded != null)
          inactiveContract.OnContractSucceeded += contract.OnContractSucceeded;
      }
      return contractResultWrapper;
    }

    private MyAddContractResultWrapper AddContract_Hauling(
      IMyContractHauling haul)
    {
      MyAddContractResultWrapper contractResultWrapper = new MyAddContractResultWrapper()
      {
        Success = false,
        ContractId = 0
      };
      if (!(Sandbox.Game.Entities.MyEntities.GetEntityById(haul.StartBlockId) is MyContractBlock entityById))
        return contractResultWrapper;
      MyContractCreationResults customContractDeliver = this.GenerateCustomContract_Deliver(entityById, haul.MoneyReward, haul.Collateral, haul.Duration, haul.EndBlockId, out contractResultWrapper.ContractId, out contractResultWrapper.ContractConditionId);
      contractResultWrapper.Success = customContractDeliver == MyContractCreationResults.Success;
      return contractResultWrapper;
    }

    private MyAddContractResultWrapper AddContract_Acquisition(
      IMyContractAcquisition acqui)
    {
      MyAddContractResultWrapper contractResultWrapper = new MyAddContractResultWrapper()
      {
        Success = false,
        ContractId = 0
      };
      if (!(Sandbox.Game.Entities.MyEntities.GetEntityById(acqui.StartBlockId) is MyContractBlock entityById))
        return contractResultWrapper;
      MyContractCreationResults obtainAndDeliver = this.GenerateCustomContract_ObtainAndDeliver(entityById, acqui.MoneyReward, acqui.Collateral, acqui.Duration, acqui.EndBlockId, acqui.ItemTypeId, acqui.ItemAmount, out contractResultWrapper.ContractId, out contractResultWrapper.ContractConditionId);
      contractResultWrapper.Success = obtainAndDeliver == MyContractCreationResults.Success;
      return contractResultWrapper;
    }

    private MyAddContractResultWrapper AddContract_Search(
      IMyContractSearch search)
    {
      MyAddContractResultWrapper contractResultWrapper = new MyAddContractResultWrapper()
      {
        Success = false,
        ContractId = 0
      };
      if (!(Sandbox.Game.Entities.MyEntities.GetEntityById(search.StartBlockId) is MyContractBlock entityById))
        return contractResultWrapper;
      MyContractCreationResults customContractFind = this.GenerateCustomContract_Find(entityById, search.MoneyReward, search.Collateral, search.Duration, search.TargetGridId, search.SearchRadius, out contractResultWrapper.ContractId, out contractResultWrapper.ContractConditionId);
      contractResultWrapper.Success = customContractFind == MyContractCreationResults.Success;
      return contractResultWrapper;
    }

    private MyAddContractResultWrapper AddContract_Escort(
      IMyContractEscort escort)
    {
      MyAddContractResultWrapper contractResultWrapper = new MyAddContractResultWrapper()
      {
        Success = false,
        ContractId = 0
      };
      if (!(Sandbox.Game.Entities.MyEntities.GetEntityById(escort.StartBlockId) is MyContractBlock entityById))
        return contractResultWrapper;
      MyContractCreationResults customContractEscort = this.GenerateCustomContract_Escort(entityById, escort.MoneyReward, escort.Collateral, escort.Duration, escort.Start, escort.End, escort.OwnerIdentityId, out contractResultWrapper.ContractId, out contractResultWrapper.ContractConditionId);
      contractResultWrapper.Success = customContractEscort == MyContractCreationResults.Success;
      return contractResultWrapper;
    }

    private MyAddContractResultWrapper AddContract_Bounty(
      IMyContractBounty bounty)
    {
      MyAddContractResultWrapper contractResultWrapper = new MyAddContractResultWrapper()
      {
        Success = false,
        ContractId = 0
      };
      if (!(Sandbox.Game.Entities.MyEntities.GetEntityById(bounty.StartBlockId) is MyContractBlock entityById))
        return contractResultWrapper;
      MyContractCreationResults customContractHunt = this.GenerateCustomContract_Hunt(entityById, bounty.MoneyReward, bounty.Collateral, bounty.Duration, bounty.TargetIdentityId, out contractResultWrapper.ContractId, out contractResultWrapper.ContractConditionId);
      contractResultWrapper.Success = customContractHunt == MyContractCreationResults.Success;
      return contractResultWrapper;
    }

    private MyAddContractResultWrapper AddContract_Repair(
      IMyContractRepair repair)
    {
      MyAddContractResultWrapper contractResultWrapper = new MyAddContractResultWrapper()
      {
        Success = false,
        ContractId = 0
      };
      if (!(Sandbox.Game.Entities.MyEntities.GetEntityById(repair.StartBlockId) is MyContractBlock entityById))
        return contractResultWrapper;
      MyContractCreationResults customContractRepair = this.GenerateCustomContract_Repair(entityById, repair.MoneyReward, repair.Collateral, repair.Duration, repair.GridId, out contractResultWrapper.ContractId, out contractResultWrapper.ContractConditionId);
      contractResultWrapper.Success = customContractRepair == MyContractCreationResults.Success;
      return contractResultWrapper;
    }

    private MyAddContractResultWrapper AddContract_Custom(
      IMyContractCustom custom)
    {
      MyAddContractResultWrapper contractResultWrapper = new MyAddContractResultWrapper()
      {
        Success = false,
        ContractId = 0
      };
      if (!(Sandbox.Game.Entities.MyEntities.GetEntityById(custom.StartBlockId) is MyContractBlock entityById))
        return contractResultWrapper;
      endBlock = (MyContractBlock) null;
      long? endBlockId = custom.EndBlockId;
      if (endBlockId.HasValue)
      {
        endBlockId = custom.EndBlockId;
        if (!(Sandbox.Game.Entities.MyEntities.GetEntityById(endBlockId.Value) is MyContractBlock endBlock))
          return contractResultWrapper;
      }
      MyContractCreationResults customContractCustom = this.GenerateCustomContract_Custom(custom.DefinitionId, custom.Name, custom.Description, entityById, custom.MoneyReward, custom.Collateral, custom.ReputationReward, custom.FailReputationPrice, custom.Duration, out contractResultWrapper.ContractId, out contractResultWrapper.ContractConditionId, endBlock);
      contractResultWrapper.Success = customContractCustom == MyContractCreationResults.Success;
      return contractResultWrapper;
    }

    public bool RemoveContract(long contractId)
    {
      if (this.m_inactiveContracts.ContainsKey(contractId))
      {
        this.RemoveContractInternal(contractId);
        return true;
      }
      if (!this.m_activeContracts.ContainsKey(contractId))
        return false;
      this.m_activeContracts[contractId].Fail();
      return true;
    }

    public bool IsContractInInactive(long contractId) => this.m_inactiveContracts.ContainsKey(contractId);

    public bool IsContractActive(long contractId) => this.m_activeContracts.ContainsKey(contractId) && this.m_activeContracts[contractId].State == MyContractStateEnum.Active;

    public MyCustomContractStateEnum GetContractState(long contractId)
    {
      MyContract myContract = (MyContract) null;
      return this.m_activeContracts.TryGetValue(contractId, out myContract) || this.m_inactiveContracts.TryGetValue(contractId, out myContract) ? Sandbox.Game.Contracts.MyContractCustom.ConvertContractState(myContract.State) : MyCustomContractStateEnum.Invalid;
    }

    public bool TryFinishCustomContract(long contractId)
    {
      if (!this.m_activeContracts.ContainsKey(contractId))
        return false;
      MyContract activeContract = this.m_activeContracts[contractId];
      return activeContract.State == MyContractStateEnum.Active && activeContract.Finish() == MyContractResults.Success;
    }

    public bool TryFailCustomContract(long contractId)
    {
      if (!this.m_activeContracts.ContainsKey(contractId))
        return false;
      MyContract activeContract = this.m_activeContracts[contractId];
      if (activeContract.State != MyContractStateEnum.Active)
        return false;
      activeContract.Fail();
      return true;
    }

    public bool TryAbandonCustomContract(long contractId, long playerId)
    {
      if (!this.m_activeContracts.ContainsKey(contractId))
        return false;
      MyContract activeContract = this.m_activeContracts[contractId];
      if (activeContract.State != MyContractStateEnum.Active)
        return false;
      activeContract.Abandon(playerId);
      return true;
    }

    public MyDefinitionId? GetContractDefinitionId(long contractId)
    {
      if (this.m_inactiveContracts.ContainsKey(contractId))
        return this.m_inactiveContracts[contractId].GetDefinitionId();
      return this.m_activeContracts.ContainsKey(contractId) ? this.m_activeContracts[contractId].GetDefinitionId() : new MyDefinitionId?();
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.m_contractTypeStrategies.Clear();
      foreach (MyContract myContract in this.m_inactiveContracts.Values)
      {
        myContract.ContractChangedState = (Action<long, MyContractStateEnum, MyContractStateEnum>) null;
        myContract.OnContractAcquired = (Action<long>) null;
        myContract.OnContractFailed = (Action) null;
        myContract.OnContractSucceeded = (Action) null;
      }
      this.m_inactiveContracts.Clear();
      foreach (MyContract myContract in this.m_activeContracts.Values)
      {
        myContract.ContractChangedState = (Action<long, MyContractStateEnum, MyContractStateEnum>) null;
        myContract.OnContractAcquired = (Action<long>) null;
        myContract.OnContractFailed = (Action) null;
        myContract.OnContractSucceeded = (Action) null;
      }
      this.m_activeContracts.Clear();
      this.m_contractChanceCache.Clear();
      this.CustomFinishCondition = (Func<long, long, bool>) null;
      this.CustomCanActivateContract = (Func<long, long, MyActivationCustomResults>) null;
      this.CustomNeedsUpdate = (Func<long, bool>) null;
      this.CustomConditionFinished = (MyContractConditionDelegate) null;
      this.CustomActivateContract = (MyContractActivateDelegate) null;
      this.CustomFailFor = (MyContractFailedDelegate) null;
      this.CustomFinishFor = (MyContractFinishedDelegate) null;
      this.CustomFinish = (MyContractChangeDelegate) null;
      this.CustomFail = (MyContractChangeDelegate) null;
      this.CustomCleanUp = (MyContractChangeDelegate) null;
      this.CustomTimeRanOut = (MyContractChangeDelegate) null;
      this.CustomUpdate = (MyContractUpdateDelegate) null;
      MyAPIGateway.ContractSystem = (IMyContractSystem) null;
      this.Session = (IMySession) null;
    }

    private enum MyTransferItemsFromGridResults
    {
      Success,
      Fail_NoAccess,
      Fail_NotEnoughItems,
      Fail_NotEnoughSpace,
      Error_MissingKeyStructures,
    }

    private struct MyContractStateChanged
    {
      public long Id;
      public MyContractStateEnum StateOld;
      public MyContractStateEnum StateNew;
    }

    protected sealed class DisplayNotificationToPlayer\u003C\u003ESandbox_Game_SessionComponents_MyContractNotificationTypes : ICallSite<IMyEventOwner, MyContractNotificationTypes, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyContractNotificationTypes notifType,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentContractSystem.DisplayNotificationToPlayer(notifType);
      }
    }
  }
}
