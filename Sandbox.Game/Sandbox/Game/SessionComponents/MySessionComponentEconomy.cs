// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentEconomy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Contracts;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
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
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 887, typeof (MyObjectBuilder_SessionComponentEconomy), null, false)]
  public class MySessionComponentEconomy : MySessionComponentBase
  {
    public static readonly float ORE_AROUND_STATION_REMOVAL_RADIUS = 5000f;
    private MyStoreItemsGenerator m_storeItemsGenerator = new MyStoreItemsGenerator();
    private MyMinimalPriceCalculator m_priceCalculator = new MyMinimalPriceCalculator();
    private Dictionary<long, MyDynamicAABBTree> m_stationOreBlockTrees = new Dictionary<long, MyDynamicAABBTree>();
    private Dictionary<long, long> m_analysisPerPlayerCurrency;
    private Dictionary<long, long> m_analysisPerFactionCurrency;
    private HashSet<long> m_stationIds = new HashSet<long>();
    private bool m_stationStoreItemsFirstGeneration;
    private Dictionary<long, List<long>> m_factionFriends;
    private Dictionary<long, string> m_factionFriendTooltips;

    internal MySessionComponentEconomyDefinition EconomyDefinition { get; private set; }

    internal MyTimeSpan LastEconomyTick { get; private set; }

    internal MyTimeSpan EconomyTick { get; private set; }

    public bool GenerateFactionsOnStart { get; private set; }

    public long AnalysisTotalCurrency { get; private set; }

    public Dictionary<long, long> AnalysisPerPlayerCurrency
    {
      get => this.m_analysisPerPlayerCurrency;
      set => this.m_analysisPerPlayerCurrency = value;
    }

    public Dictionary<long, long> AnalysisPerFactionCurrency
    {
      get => this.m_analysisPerFactionCurrency;
      set => this.m_analysisPerFactionCurrency = value;
    }

    public long AnalysisCurrencyFaucet { get; private set; }

    public long AnalysisCurrencySink { get; private set; }

    public long CurrencyGeneratedThisTick { get; set; }

    public long CurrencyDestroyedThisTick { get; set; }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      if (!(sessionComponent is MyObjectBuilder_SessionComponentEconomy componentEconomy))
        return;
      this.GenerateFactionsOnStart = componentEconomy.GenerateFactionsOnStart;
      this.AnalysisTotalCurrency = componentEconomy.AnalysisTotalCurrency;
      this.AnalysisCurrencyFaucet = componentEconomy.AnalysisCurrencyFaucet;
      this.AnalysisCurrencySink = componentEconomy.AnalysisCurrencySink;
      this.CurrencyGeneratedThisTick = componentEconomy.CurrencyGeneratedThisTick;
      this.CurrencyDestroyedThisTick = componentEconomy.CurrencyDestroyedThisTick;
      if (this.AnalysisPerPlayerCurrency == null)
        this.AnalysisPerPlayerCurrency = new Dictionary<long, long>();
      if (componentEconomy.AnalysisPerPlayerCurrency != null)
      {
        foreach (MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair myIdBalancePair in (List<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair>) componentEconomy.AnalysisPerPlayerCurrency)
          this.AnalysisPerPlayerCurrency.Add(myIdBalancePair.Id, myIdBalancePair.Balance);
      }
      if (this.AnalysisPerFactionCurrency == null)
        this.AnalysisPerFactionCurrency = new Dictionary<long, long>();
      if (componentEconomy.AnalysisPerFactionCurrency == null)
        return;
      foreach (MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair myIdBalancePair in (List<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair>) componentEconomy.AnalysisPerFactionCurrency)
        this.AnalysisPerFactionCurrency.Add(myIdBalancePair.Id, myIdBalancePair.Balance);
    }

    public override void InitFromDefinition(MySessionComponentDefinition definition)
    {
      base.InitFromDefinition(definition);
      this.EconomyDefinition = definition as MySessionComponentEconomyDefinition;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_SessionComponentEconomy objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_SessionComponentEconomy;
      objectBuilder.GenerateFactionsOnStart = this.GenerateFactionsOnStart;
      objectBuilder.AnalysisTotalCurrency = this.AnalysisTotalCurrency;
      objectBuilder.AnalysisCurrencyFaucet = this.AnalysisCurrencyFaucet;
      objectBuilder.AnalysisCurrencySink = this.AnalysisCurrencySink;
      objectBuilder.CurrencyGeneratedThisTick = this.CurrencyGeneratedThisTick;
      objectBuilder.CurrencyDestroyedThisTick = this.CurrencyDestroyedThisTick;
      if (objectBuilder.AnalysisPerPlayerCurrency == null)
        objectBuilder.AnalysisPerPlayerCurrency = new MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair>();
      foreach (KeyValuePair<long, long> keyValuePair in this.AnalysisPerPlayerCurrency)
        objectBuilder.AnalysisPerPlayerCurrency.Add(new MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair()
        {
          Id = keyValuePair.Key,
          Balance = keyValuePair.Value
        });
      if (objectBuilder.AnalysisPerFactionCurrency == null)
        objectBuilder.AnalysisPerFactionCurrency = new MySerializableList<MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair>();
      foreach (KeyValuePair<long, long> keyValuePair in this.AnalysisPerFactionCurrency)
        objectBuilder.AnalysisPerFactionCurrency.Add(new MyObjectBuilder_SessionComponentEconomy.MyIdBalancePair()
        {
          Id = keyValuePair.Key,
          Balance = keyValuePair.Value
        });
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public override void BeforeStart()
    {
      if (Sync.IsServer)
      {
        if (MySession.Static.Settings.EnableEconomy)
          this.EconomyTick = MyTimeSpan.FromSeconds((double) MySession.Static.Settings.EconomyTickInSeconds);
        if (MySession.Static.Settings.EnableEconomy && this.GenerateFactionsOnStart)
        {
          new MyFactionGenerator().GenerateFactions(this.EconomyDefinition);
          new MyStationGenerator(this.EconomyDefinition).GenerateStations(MySession.Static.Factions);
          new MyFactionRelationGenerator(this.EconomyDefinition).GenerateFactionRelations(MySession.Static.Factions);
          this.GenerateFactionsOnStart = false;
          this.m_stationStoreItemsFirstGeneration = true;
          this.UpdateStations();
          this.m_stationStoreItemsFirstGeneration = false;
        }
      }
      this.RemoveOreAroundStations();
    }

    private void CreateTestingData()
    {
      MyObjectBuilder_Ore newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Ice");
      MyObjectBuilder_Ore newObject2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Gold");
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        foreach (MyStation station in faction.Value.Stations)
        {
          int num = 0;
          for (int index = 0; index < 5; ++index)
          {
            MyStoreItem myStoreItem = new MyStoreItem((long) num, newObject1.GetId(), index * 20, index * 100, StoreItemTypes.Offer);
            station.StoreItems.Add(myStoreItem);
            ++num;
          }
          for (int index = 0; index < 5; ++index)
          {
            MyStoreItem myStoreItem = new MyStoreItem((long) num, newObject2.GetId(), index * 10, index * 100, StoreItemTypes.Offer);
            station.StoreItems.Add(myStoreItem);
            ++num;
          }
          for (int index = 0; index < 5; ++index)
          {
            MyStoreItem myStoreItem = new MyStoreItem((long) num, newObject1.GetId(), index * 100, index * 100, StoreItemTypes.Order);
            station.StoreItems.Add(myStoreItem);
            ++num;
          }
          for (int index = 0; index < 5; ++index)
          {
            MyStoreItem myStoreItem = new MyStoreItem((long) num, newObject2.GetId(), index * 10, index * 100, StoreItemTypes.Order);
            station.StoreItems.Add(myStoreItem);
            ++num;
          }
          for (int index = 0; index < 5; ++index)
          {
            MyStoreItem myStoreItem = new MyStoreItem((long) num, index * 10, index * 100, StoreItemTypes.Offer, ItemTypes.Hydrogen);
            station.StoreItems.Add(myStoreItem);
            ++num;
          }
        }
      }
    }

    public override void UpdateAfterSimulation()
    {
      if (!Sync.IsServer)
        return;
      base.UpdateAfterSimulation();
      if (!MySession.Static.Settings.EnableEconomy || (double) MySandboxGame.TotalGamePlayTimeInMilliseconds <= (this.LastEconomyTick + this.EconomyTick).Milliseconds)
        return;
      this.UpdateStations();
      this.UpdateCurrencyAnalysis();
      this.LastEconomyTick = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
    }

    public void ForceEconomyTick() => this.LastEconomyTick = MyTimeSpan.FromMilliseconds(-2147483647.0);

    private void UpdateStations()
    {
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      component.CleanOldContracts();
      Dictionary<long, int> counts = new Dictionary<long, int>();
      Dictionary<long, List<MyContract>> lists = new Dictionary<long, List<MyContract>>();
      component.GetAvailableContractCountsByStation(ref counts, ref lists);
      MyContractGenerator cGen = new MyContractGenerator(this.EconomyDefinition);
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        foreach (MyStation station in faction.Value.Stations)
        {
          station.Update(faction.Value);
          if (!counts.ContainsKey(station.Id))
          {
            List<MyContract> existingContracts = new List<MyContract>();
            component.CreateContractsForStation(cGen, faction.Value, station, 0, ref existingContracts);
            lists.Add(station.Id, existingContracts);
          }
          else
          {
            List<MyContract> existingContracts = lists[station.Id];
            component.CreateContractsForStation(cGen, faction.Value, station, counts[station.Id], ref existingContracts);
          }
        }
        this.m_storeItemsGenerator.Update(faction.Value, this.m_stationStoreItemsFirstGeneration);
      }
    }

    internal int GetDefaultReputationForRelation(MyRelationsBetweenFactions relation)
    {
      switch (relation)
      {
        case MyRelationsBetweenFactions.Enemies:
          return this.EconomyDefinition.ReputationHostileMid;
        case MyRelationsBetweenFactions.Friends:
          return this.EconomyDefinition.ReputationFriendlyMid;
        default:
          return this.EconomyDefinition.ReputationNeutralMid;
      }
    }

    public int GetDefaultReputationPlayer() => this.EconomyDefinition.ReputationPlayerDefault;

    internal int TranslateRelationToReputation(MyRelationsBetweenFactions relation)
    {
      switch (relation)
      {
        case MyRelationsBetweenFactions.Neutral:
        case MyRelationsBetweenFactions.Allies:
          return this.EconomyDefinition.ReputationNeutralMid;
        case MyRelationsBetweenFactions.Enemies:
          return this.EconomyDefinition.ReputationHostileMid;
        case MyRelationsBetweenFactions.Friends:
          return this.EconomyDefinition.ReputationNeutralMid;
        default:
          return 0;
      }
    }

    internal MyRelationsBetweenFactions TranslateReputationToRelationship(
      int reputation)
    {
      if (reputation < this.EconomyDefinition.ReputationHostileMin || reputation < this.EconomyDefinition.ReputationNeutralMin)
        return MyRelationsBetweenFactions.Enemies;
      if (reputation < this.EconomyDefinition.ReputationFriendlyMin)
        return MyRelationsBetweenFactions.Neutral;
      int reputationFriendlyMax = this.EconomyDefinition.ReputationFriendlyMax;
      return MyRelationsBetweenFactions.Friends;
    }

    internal Tuple<MyRelationsBetweenFactions, int> ValidateReputationConsistency(
      MyRelationsBetweenFactions relation,
      int reputation)
    {
      return this.TranslateReputationToRelationship(reputation) == relation ? new Tuple<MyRelationsBetweenFactions, int>(relation, reputation) : new Tuple<MyRelationsBetweenFactions, int>(relation, this.TranslateRelationToReputation(relation));
    }

    public int ClampReputation(int reputation)
    {
      if (reputation < this.EconomyDefinition.ReputationHostileMin)
        return this.EconomyDefinition.ReputationHostileMin;
      return reputation > this.EconomyDefinition.ReputationFriendlyMax ? this.EconomyDefinition.ReputationFriendlyMax : reputation;
    }

    public int GetHostileMax() => this.EconomyDefinition.ReputationHostileMin;

    public int GetNeutralMin() => this.EconomyDefinition.ReputationNeutralMin;

    public int GetFriendlyMin() => this.EconomyDefinition.ReputationFriendlyMin;

    public int GetFriendlyMax() => this.EconomyDefinition.ReputationFriendlyMax;

    internal MyFactionCollection.MyReputationModifiers GetReputationModifiers(
      bool positive = true)
    {
      MyFactionCollection.MyReputationModifiers reputationModifiers;
      if (positive)
        reputationModifiers = new MyFactionCollection.MyReputationModifiers()
        {
          Owner = this.EconomyDefinition.RepMult_Pos_Owner,
          Friend = this.EconomyDefinition.RepMult_Pos_Friend,
          Neutral = this.EconomyDefinition.RepMult_Pos_Neutral,
          Hostile = this.EconomyDefinition.RepMult_Pos_Enemy
        };
      else
        reputationModifiers = new MyFactionCollection.MyReputationModifiers()
        {
          Owner = this.EconomyDefinition.RepMult_Neg_Owner,
          Friend = this.EconomyDefinition.RepMult_Neg_Friend,
          Neutral = this.EconomyDefinition.RepMult_Neg_Neutral,
          Hostile = this.EconomyDefinition.RepMult_Neg_Enemy
        };
      return reputationModifiers;
    }

    public int ConvertPirateReputationToChance(int reputation)
    {
      float num = 900f;
      return (int) (100.0 + (double) ((float) (reputation - this.GetNeutralMin()) / (float) (this.GetFriendlyMax() - this.GetNeutralMin())) * (double) num);
    }

    internal float GetOrdersFriendlyBonus(int relationValue) => (float) (1.0 + (double) this.EconomyDefinition.OrdersFriendlyBonus * ((double) (relationValue - this.EconomyDefinition.ReputationFriendlyMin) / (double) (this.EconomyDefinition.ReputationFriendlyMax - this.EconomyDefinition.ReputationFriendlyMin)));

    internal float GetOffersFriendlyBonus(int relationValue) => (float) (1.0 - (double) this.EconomyDefinition.OffersFriendlyBonus * ((double) (relationValue - this.EconomyDefinition.ReputationFriendlyMin) / (double) (this.EconomyDefinition.ReputationFriendlyMax - this.EconomyDefinition.ReputationFriendlyMin)));

    internal float GetOrdersFriendlyBonusMax() => this.EconomyDefinition.OrdersFriendlyBonus;

    internal float GetOffersFriendlyBonusMax() => this.EconomyDefinition.OffersFriendlyBonus;

    public void AddCurrencyGenerated(long amount) => this.CurrencyGeneratedThisTick += amount;

    public void AddCurrencyDestroyed(long amount) => this.CurrencyDestroyedThisTick += amount;

    private void UpdateCurrencyAnalysis()
    {
      if (MyBankingSystem.Static == null || !MyFakes.ENABLE_ECONOMY_ANALYTICS)
        return;
      this.AnalysisTotalCurrency = MyBankingSystem.Static.OverallBalance;
      MyBankingSystem.Static.GetPerPlayerBalances(ref this.m_analysisPerPlayerCurrency);
      MyBankingSystem.Static.GetPerFactionBalances(ref this.m_analysisPerFactionCurrency);
      this.AnalysisCurrencyFaucet = this.CurrencyGeneratedThisTick;
      this.AnalysisCurrencySink = this.CurrencyDestroyedThisTick;
      this.CurrencyGeneratedThisTick = 0L;
      this.CurrencyDestroyedThisTick = 0L;
    }

    internal int GetMinimumItemPrice(SerializableDefinitionId itemDefinitionId)
    {
      int minimalPrice = 0;
      if (!this.m_priceCalculator.TryGetItemMinimalPrice((MyDefinitionId) itemDefinitionId, out minimalPrice))
      {
        this.m_priceCalculator.CalculateMinimalPrices(new SerializableDefinitionId[1]
        {
          itemDefinitionId
        });
        if (!this.m_priceCalculator.TryGetItemMinimalPrice((MyDefinitionId) itemDefinitionId, out minimalPrice))
          return 0;
      }
      return minimalPrice;
    }

    public int GetStoreCreationLimitPerPlayer() => 30;

    public static void PrepareDatapad(
      ref MyObjectBuilder_Datapad datapad,
      MyFaction faction,
      MyStation station)
    {
      datapad.Name = string.Format(MyTexts.GetString(MySpaceTexts.Datapad_GPS_Name), (object) faction.Tag);
      string name = string.Format(MyTexts.GetString(MySpaceTexts.Datapad_GPS_Data), (object) faction.Tag);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(MyTexts.GetString(string.Format("Datapad_Station_GPS_Content_{0}", (object) MyRandom.Instance.Next(0, 9))));
      stringBuilder.AppendLine();
      stringBuilder.AppendLine(MyGps.ConvertToString(name, station.Position));
      datapad.Data = stringBuilder.ToString();
    }

    public SerializableDefinitionId GetDatapadDefinitionId() => this.EconomyDefinition.DatapadDefinition;

    public bool IsGridStation(long gridId)
    {
      if (!Sync.IsServer)
        MyLog.Default.WriteToLogAndAssert("Checking if grid is station on client. Client has no such information.");
      return this.m_stationIds.Contains(gridId);
    }

    public void AddStationGrid(long gridId)
    {
      if (!Sync.IsServer)
        MyLog.Default.WriteToLogAndAssert("Adding grid into station collection on client. Client shouldn't do that");
      if (this.m_stationIds.Contains(gridId))
        MyLog.Default.WriteToLogAndAssert("Grid has already been added to station collection, should not happen again.");
      else
        this.m_stationIds.Add(gridId);
    }

    public void RemoveStationGrid(long gridId)
    {
      if (!Sync.IsServer)
        MyLog.Default.WriteToLogAndAssert("Removing grid from station collection on client. Client shouldn't do that");
      if (!this.m_stationIds.Contains(gridId))
        return;
      this.m_stationIds.Remove(gridId);
    }

    public MyDynamicAABBTree GetStationBlockTree(long planetId)
    {
      if (!this.m_stationOreBlockTrees.ContainsKey(planetId))
        this.m_stationOreBlockTrees.Add(planetId, new MyDynamicAABBTree(Vector3.Zero, 0.0f));
      return this.m_stationOreBlockTrees[planetId];
    }

    private void RemoveOreAroundStations()
    {
      Dictionary<long, List<Vector3D>> dictionary1 = new Dictionary<long, List<Vector3D>>();
      Dictionary<long, MyPlanet> dictionary2 = new Dictionary<long, MyPlanet>();
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        foreach (MyStation station in faction.Value.Stations)
        {
          if (station.Type == MyStationTypeEnum.Outpost)
          {
            MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(station.Position);
            if (closestPlanet != null)
            {
              if (!dictionary2.ContainsKey(closestPlanet.EntityId))
              {
                dictionary2.Add(closestPlanet.EntityId, closestPlanet);
                dictionary1.Add(closestPlanet.EntityId, new List<Vector3D>());
              }
              dictionary1[closestPlanet.EntityId].Add(station.Position);
            }
          }
        }
      }
      foreach (KeyValuePair<long, List<Vector3D>> keyValuePair in dictionary1)
      {
        MyDynamicAABBTree stationBlockTree = this.GetStationBlockTree(keyValuePair.Key);
        foreach (Vector3D position in keyValuePair.Value)
          dictionary2[keyValuePair.Key].AddToStationOreBlockTree(ref stationBlockTree, position, MySessionComponentEconomy.ORE_AROUND_STATION_REMOVAL_RADIUS);
        dictionary2[keyValuePair.Key].SetStationOreBlockTree(stationBlockTree);
      }
    }

    internal string GetFactionFriendTooltip(long factionId)
    {
      if (this.m_factionFriends == null)
        this.InitializeFactionFriendCollection();
      return this.m_factionFriendTooltips.ContainsKey(factionId) ? this.m_factionFriendTooltips[factionId] : string.Empty;
    }

    private void InitializeFactionFriendCollection()
    {
      this.m_factionFriends = new Dictionary<long, List<long>>();
      this.m_factionFriendTooltips = new Dictionary<long, string>();
      Dictionary<long, StringBuilder> dictionary = new Dictionary<long, StringBuilder>();
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        if (!this.m_factionFriends.ContainsKey(faction.Value.FactionId) && MySession.Static.Factions.IsNpcFaction(faction.Value.Tag))
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(string.Format("{0}\nFriends:", (object) faction.Value.Name));
          this.m_factionFriends.Add(faction.Value.FactionId, new List<long>());
          dictionary.Add(faction.Value.FactionId, stringBuilder);
        }
      }
      foreach (KeyValuePair<MyFactionCollection.MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> allFactionRelation in MySession.Static.Factions.GetAllFactionRelations())
      {
        if (allFactionRelation.Value.Item1 == MyRelationsBetweenFactions.Friends || allFactionRelation.Value.Item1 == MyRelationsBetweenFactions.Allies)
        {
          MyFaction factionById1 = MySession.Static.Factions.TryGetFactionById(allFactionRelation.Key.RelateeId1) as MyFaction;
          MyFaction factionById2 = MySession.Static.Factions.TryGetFactionById(allFactionRelation.Key.RelateeId2) as MyFaction;
          if (factionById1 == null)
            MyLog.Default.Error("Faction relation exists for nonexisting faction: " + (object) allFactionRelation.Key.RelateeId1);
          else if (factionById2 == null)
            MyLog.Default.Error("Faction relation exists for nonexisting faction: " + (object) allFactionRelation.Key.RelateeId2);
          else if (MySession.Static.Factions.IsNpcFaction(factionById1.Tag) && MySession.Static.Factions.IsNpcFaction(factionById2.Tag))
          {
            if (this.m_factionFriends.ContainsKey(factionById1.FactionId) && this.m_factionFriends.ContainsKey(factionById2.FactionId))
            {
              this.m_factionFriends[factionById1.FactionId].Add(factionById2.FactionId);
              this.m_factionFriends[factionById2.FactionId].Add(factionById1.FactionId);
            }
            if (dictionary.ContainsKey(factionById1.FactionId) && dictionary.ContainsKey(factionById2.FactionId))
            {
              dictionary[factionById1.FactionId].Append(string.Format("\n   [{0}] {1}", (object) factionById2.Tag, (object) factionById2.Name));
              dictionary[factionById2.FactionId].Append(string.Format("\n   [{0}] {1}", (object) factionById1.Tag, (object) factionById1.Name));
            }
          }
        }
      }
      foreach (KeyValuePair<long, StringBuilder> keyValuePair in dictionary)
        this.m_factionFriendTooltips.Add(keyValuePair.Key, keyValuePair.Value.ToString());
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.m_stationOreBlockTrees.Clear();
      if (this.m_analysisPerPlayerCurrency != null)
        this.m_analysisPerPlayerCurrency.Clear();
      if (this.m_analysisPerFactionCurrency != null)
        this.m_analysisPerFactionCurrency.Clear();
      this.m_stationIds.Clear();
      this.Session = (IMySession) null;
    }
  }
}
