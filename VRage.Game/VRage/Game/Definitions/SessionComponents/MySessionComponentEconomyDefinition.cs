// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.SessionComponents.MySessionComponentEconomyDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game.Definitions.SessionComponents
{
  [MyDefinitionType(typeof (MyObjectbuilder_SessionComponentEconomyDefinition), null)]
  public class MySessionComponentEconomyDefinition : MySessionComponentDefinition
  {
    public const int StoreCreationLimitPerPlayer = 30;
    public long PerFactionInitialCurrency;
    public int FactionRatio_Miners;
    public int FactionRatio_Traders;
    public int FactionRatio_Builders;
    public float DeepSpaceStationStoreBonus;
    public float DeepSpaceStationContractBonus;
    public float RepMult_Pos_Owner;
    public float RepMult_Pos_Friend;
    public float RepMult_Pos_Neutral;
    public float RepMult_Pos_Enemy;
    public float RepMult_Neg_Owner;
    public float RepMult_Neg_Friend;
    public float RepMult_Neg_Neutral;
    public float RepMult_Neg_Enemy;
    public int ReputationHostileMin;
    public int ReputationHostileMid;
    public int ReputationNeutralMin;
    public int ReputationNeutralMid;
    public int ReputationFriendlyMin;
    public int ReputationFriendlyMid;
    public int ReputationFriendlyMax;
    public int ReputationPlayerDefault;
    public int ReputationLevelValue;
    public double Station_Distance_MinimalFromOtherStation;
    public int Station_Rule_Miner_Min_StationM;
    public int Station_Rule_Miner_Max_StationM;
    public int Station_Rule_Miner_Min_Outpost;
    public int Station_Rule_Miner_Max_Outpost;
    public int Station_Rule_Trader_Min_Orbit;
    public int Station_Rule_Trader_Max_Orbit;
    public int Station_Rule_Trader_Min_Outpost;
    public int Station_Rule_Trader_Max_Outpost;
    public int Station_Rule_Trader_Min_Deep;
    public int Station_Rule_Trader_Max_Deep;
    public int Station_Rule_Builder_Min_Orbit;
    public int Station_Rule_Builder_Max_Orbit;
    public int Station_Rule_Builder_Min_Outpost;
    public int Station_Rule_Builder_Max_Outpost;
    public int Station_Rule_Builder_Min_Station;
    public int Station_Rule_Builder_Max_Station;
    public MyDefinitionId PirateId;

    public float OffersFriendlyBonus { get; set; }

    public float OrdersFriendlyBonus { get; set; }

    public float TransactionFee { get; set; }

    public float ListingFee { get; set; }

    public SerializableDefinitionId DatapadDefinition { get; set; }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectbuilder_SessionComponentEconomyDefinition economyDefinition = (MyObjectbuilder_SessionComponentEconomyDefinition) builder;
      if (economyDefinition == null)
        return;
      this.PerFactionInitialCurrency = economyDefinition.PerFactionInitialCurrency;
      this.FactionRatio_Miners = economyDefinition.FactionRatio_Miners;
      this.FactionRatio_Traders = economyDefinition.FactionRatio_Traders;
      this.FactionRatio_Builders = economyDefinition.FactionRatio_Builders;
      this.DeepSpaceStationStoreBonus = economyDefinition.DeepSpaceStationStoreBonus;
      this.DeepSpaceStationContractBonus = economyDefinition.DeepSpaceStationContractBonus;
      this.RepMult_Pos_Owner = economyDefinition.RepMult_Pos_Owner;
      this.RepMult_Pos_Friend = economyDefinition.RepMult_Pos_Friend;
      this.RepMult_Pos_Neutral = economyDefinition.RepMult_Pos_Neutral;
      this.RepMult_Pos_Enemy = economyDefinition.RepMult_Pos_Enemy;
      this.RepMult_Neg_Owner = economyDefinition.RepMult_Neg_Owner;
      this.RepMult_Neg_Friend = economyDefinition.RepMult_Neg_Friend;
      this.RepMult_Neg_Neutral = economyDefinition.RepMult_Neg_Neutral;
      this.RepMult_Neg_Enemy = economyDefinition.RepMult_Neg_Enemy;
      this.ReputationHostileMin = economyDefinition.ReputationHostileMin;
      this.ReputationHostileMid = economyDefinition.ReputationHostileMid;
      this.ReputationNeutralMin = economyDefinition.ReputationNeutralMin;
      this.ReputationNeutralMid = economyDefinition.ReputationNeutralMid;
      this.ReputationFriendlyMin = economyDefinition.ReputationFriendlyMin;
      this.ReputationFriendlyMid = economyDefinition.ReputationFriendlyMid;
      this.ReputationFriendlyMax = economyDefinition.ReputationFriendlyMax;
      this.ReputationPlayerDefault = economyDefinition.ReputationPlayerDefault;
      this.ReputationLevelValue = economyDefinition.ReputationLevelValue;
      this.Station_Distance_MinimalFromOtherStation = economyDefinition.Station_Distance_MinimalFromOtherStation;
      this.Station_Rule_Miner_Min_StationM = economyDefinition.Station_Rule_Miner_Min_StationM;
      this.Station_Rule_Miner_Max_StationM = economyDefinition.Station_Rule_Miner_Max_StationM;
      this.Station_Rule_Miner_Min_Outpost = economyDefinition.Station_Rule_Miner_Min_Outpost;
      this.Station_Rule_Miner_Max_Outpost = economyDefinition.Station_Rule_Miner_Max_Outpost;
      this.Station_Rule_Trader_Min_Orbit = economyDefinition.Station_Rule_Trader_Min_Orbit;
      this.Station_Rule_Trader_Max_Orbit = economyDefinition.Station_Rule_Trader_Max_Orbit;
      this.Station_Rule_Trader_Min_Outpost = economyDefinition.Station_Rule_Trader_Min_Outpost;
      this.Station_Rule_Trader_Max_Outpost = economyDefinition.Station_Rule_Trader_Max_Outpost;
      this.Station_Rule_Trader_Min_Deep = economyDefinition.Station_Rule_Trader_Min_Deep;
      this.Station_Rule_Trader_Max_Deep = economyDefinition.Station_Rule_Trader_Max_Deep;
      this.Station_Rule_Builder_Min_Orbit = economyDefinition.Station_Rule_Builder_Min_Orbit;
      this.Station_Rule_Builder_Max_Orbit = economyDefinition.Station_Rule_Builder_Max_Orbit;
      this.Station_Rule_Builder_Min_Outpost = economyDefinition.Station_Rule_Builder_Min_Outpost;
      this.Station_Rule_Builder_Max_Outpost = economyDefinition.Station_Rule_Builder_Max_Outpost;
      this.Station_Rule_Builder_Min_Station = economyDefinition.Station_Rule_Builder_Min_Station;
      this.Station_Rule_Builder_Max_Station = economyDefinition.Station_Rule_Builder_Max_Station;
      this.PirateId = (MyDefinitionId) economyDefinition.PirateId;
      this.OffersFriendlyBonus = economyDefinition.OffersFriendlyBonus;
      this.OrdersFriendlyBonus = economyDefinition.OrdersFriendlyBonus;
      this.TransactionFee = economyDefinition.TransactionFee;
      this.ListingFee = economyDefinition.ListingFee;
      this.DatapadDefinition = economyDefinition.DatapadDefinition;
    }

    private class VRage_Game_Definitions_SessionComponents_MySessionComponentEconomyDefinition\u003C\u003EActor : IActivator, IActivator<MySessionComponentEconomyDefinition>
    {
      object IActivator.CreateInstance() => (object) new MySessionComponentEconomyDefinition();

      MySessionComponentEconomyDefinition IActivator<MySessionComponentEconomyDefinition>.CreateInstance() => new MySessionComponentEconomyDefinition();
    }
  }
}
