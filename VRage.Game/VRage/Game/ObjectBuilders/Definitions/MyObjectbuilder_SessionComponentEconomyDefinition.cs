// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectbuilder_SessionComponentEconomyDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectbuilder_SessionComponentEconomyDefinition : MyObjectBuilder_SessionComponentDefinition
  {
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
    public SerializableDefinitionId PirateId;
    public float OffersFriendlyBonus = 0.1f;
    public float OrdersFriendlyBonus = 0.05f;
    public float TransactionFee = 0.02f;
    public float ListingFee = 0.03f;
    public SerializableDefinitionId DatapadDefinition;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EPerFactionInitialCurrency\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in long value)
      {
        owner.PerFactionInitialCurrency = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out long value)
      {
        value = owner.PerFactionInitialCurrency;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EFactionRatio_Miners\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.FactionRatio_Miners = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.FactionRatio_Miners;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EFactionRatio_Traders\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.FactionRatio_Traders = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.FactionRatio_Traders;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EFactionRatio_Builders\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.FactionRatio_Builders = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.FactionRatio_Builders;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EDeepSpaceStationStoreBonus\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.DeepSpaceStationStoreBonus = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.DeepSpaceStationStoreBonus;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EDeepSpaceStationContractBonus\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.DeepSpaceStationContractBonus = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.DeepSpaceStationContractBonus;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003ERepMult_Pos_Owner\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.RepMult_Pos_Owner = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.RepMult_Pos_Owner;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003ERepMult_Pos_Friend\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.RepMult_Pos_Friend = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.RepMult_Pos_Friend;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003ERepMult_Pos_Neutral\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.RepMult_Pos_Neutral = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.RepMult_Pos_Neutral;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003ERepMult_Pos_Enemy\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.RepMult_Pos_Enemy = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.RepMult_Pos_Enemy;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003ERepMult_Neg_Owner\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.RepMult_Neg_Owner = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.RepMult_Neg_Owner;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003ERepMult_Neg_Friend\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.RepMult_Neg_Friend = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.RepMult_Neg_Friend;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003ERepMult_Neg_Neutral\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.RepMult_Neg_Neutral = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.RepMult_Neg_Neutral;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003ERepMult_Neg_Enemy\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.RepMult_Neg_Enemy = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.RepMult_Neg_Enemy;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EReputationHostileMin\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.ReputationHostileMin = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.ReputationHostileMin;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EReputationHostileMid\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.ReputationHostileMid = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.ReputationHostileMid;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EReputationNeutralMin\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.ReputationNeutralMin = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.ReputationNeutralMin;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EReputationNeutralMid\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.ReputationNeutralMid = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.ReputationNeutralMid;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EReputationFriendlyMin\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.ReputationFriendlyMin = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.ReputationFriendlyMin;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EReputationFriendlyMid\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.ReputationFriendlyMid = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.ReputationFriendlyMid;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EReputationFriendlyMax\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.ReputationFriendlyMax = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.ReputationFriendlyMax;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EReputationPlayerDefault\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.ReputationPlayerDefault = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.ReputationPlayerDefault;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EReputationLevelValue\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.ReputationLevelValue = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.ReputationLevelValue;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Distance_MinimalFromOtherStation\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in double value)
      {
        owner.Station_Distance_MinimalFromOtherStation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out double value)
      {
        value = owner.Station_Distance_MinimalFromOtherStation;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Miner_Min_StationM\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Miner_Min_StationM = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Miner_Min_StationM;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Miner_Max_StationM\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Miner_Max_StationM = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Miner_Max_StationM;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Miner_Min_Outpost\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Miner_Min_Outpost = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Miner_Min_Outpost;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Miner_Max_Outpost\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Miner_Max_Outpost = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Miner_Max_Outpost;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Trader_Min_Orbit\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Trader_Min_Orbit = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Trader_Min_Orbit;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Trader_Max_Orbit\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Trader_Max_Orbit = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Trader_Max_Orbit;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Trader_Min_Outpost\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Trader_Min_Outpost = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Trader_Min_Outpost;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Trader_Max_Outpost\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Trader_Max_Outpost = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Trader_Max_Outpost;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Trader_Min_Deep\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Trader_Min_Deep = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Trader_Min_Deep;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Trader_Max_Deep\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Trader_Max_Deep = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Trader_Max_Deep;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Builder_Min_Orbit\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Builder_Min_Orbit = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Builder_Min_Orbit;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Builder_Max_Orbit\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Builder_Max_Orbit = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Builder_Max_Orbit;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Builder_Min_Outpost\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Builder_Min_Outpost = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Builder_Min_Outpost;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Builder_Max_Outpost\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Builder_Max_Outpost = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Builder_Max_Outpost;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Builder_Min_Station\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Builder_Min_Station = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Builder_Min_Station;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EStation_Rule_Builder_Max_Station\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in int value)
      {
        owner.Station_Rule_Builder_Max_Station = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out int value)
      {
        value = owner.Station_Rule_Builder_Max_Station;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EPirateId\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in SerializableDefinitionId value)
      {
        owner.PirateId = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out SerializableDefinitionId value)
      {
        value = owner.PirateId;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EOffersFriendlyBonus\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.OffersFriendlyBonus = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.OffersFriendlyBonus;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EOrdersFriendlyBonus\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.OrdersFriendlyBonus = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.OrdersFriendlyBonus;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003ETransactionFee\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.TransactionFee = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.TransactionFee;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EListingFee\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in float value)
      {
        owner.ListingFee = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out float value)
      {
        value = owner.ListingFee;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EDatapadDefinition\u003C\u003EAccessor : IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in SerializableDefinitionId value)
      {
        owner.DatapadDefinition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out SerializableDefinitionId value)
      {
        value = owner.DatapadDefinition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectbuilder_SessionComponentEconomyDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectbuilder_SessionComponentEconomyDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectbuilder_SessionComponentEconomyDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectbuilder_SessionComponentEconomyDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectbuilder_SessionComponentEconomyDefinition();

      MyObjectbuilder_SessionComponentEconomyDefinition IActivator<MyObjectbuilder_SessionComponentEconomyDefinition>.CreateInstance() => new MyObjectbuilder_SessionComponentEconomyDefinition();
    }
  }
}
