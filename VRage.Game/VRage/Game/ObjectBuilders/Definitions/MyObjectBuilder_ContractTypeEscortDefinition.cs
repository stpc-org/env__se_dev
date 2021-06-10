// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_ContractTypeEscortDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ContractTypeEscortDefinition : MyObjectBuilder_ContractTypeDefinition
  {
    [ProtoMember(1)]
    public double RewardRadius;
    [ProtoMember(3)]
    public double TriggerRadius;
    [ProtoMember(5)]
    public double TravelDistanceMax;
    [ProtoMember(7)]
    public double TravelDistanceMin;
    [ProtoMember(9)]
    public int DroneFirstDelayInS;
    [ProtoMember(11)]
    public int DroneAttackPeriodInS;
    [ProtoMember(13)]
    public int InitialDelayInS;
    [ProtoMember(14)]
    public int DronesPerWave;
    [ProtoMember(15)]
    public MySerializableList<string> PrefabsAttackDrones;
    [ProtoMember(17)]
    public MySerializableList<string> PrefabsEscortShips;
    [ProtoMember(19)]
    public MySerializableList<string> DroneBehaviours;
    [ProtoMember(21)]
    public float Duration_BaseTime;
    [ProtoMember(23)]
    public float Duration_FlightTimeMultiplier;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003ERewardRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in double value)
      {
        owner.RewardRadius = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out double value)
      {
        value = owner.RewardRadius;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003ETriggerRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in double value)
      {
        owner.TriggerRadius = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out double value)
      {
        value = owner.TriggerRadius;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003ETravelDistanceMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in double value)
      {
        owner.TravelDistanceMax = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out double value)
      {
        value = owner.TravelDistanceMax;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003ETravelDistanceMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in double value)
      {
        owner.TravelDistanceMin = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out double value)
      {
        value = owner.TravelDistanceMin;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EDroneFirstDelayInS\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in int value)
      {
        owner.DroneFirstDelayInS = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out int value)
      {
        value = owner.DroneFirstDelayInS;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EDroneAttackPeriodInS\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in int value)
      {
        owner.DroneAttackPeriodInS = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out int value)
      {
        value = owner.DroneAttackPeriodInS;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EInitialDelayInS\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in int value)
      {
        owner.InitialDelayInS = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out int value)
      {
        value = owner.InitialDelayInS;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EDronesPerWave\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in int value)
      {
        owner.DronesPerWave = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out int value)
      {
        value = owner.DronesPerWave;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EPrefabsAttackDrones\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, MySerializableList<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in MySerializableList<string> value)
      {
        owner.PrefabsAttackDrones = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out MySerializableList<string> value)
      {
        value = owner.PrefabsAttackDrones;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EPrefabsEscortShips\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, MySerializableList<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in MySerializableList<string> value)
      {
        owner.PrefabsEscortShips = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out MySerializableList<string> value)
      {
        value = owner.PrefabsEscortShips;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EDroneBehaviours\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, MySerializableList<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in MySerializableList<string> value)
      {
        owner.DroneBehaviours = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out MySerializableList<string> value)
      {
        value = owner.DroneBehaviours;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EDuration_BaseTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in float value)
      {
        owner.Duration_BaseTime = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out float value)
      {
        value = owner.Duration_BaseTime;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EDuration_FlightTimeMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in float value)
      {
        owner.Duration_FlightTimeMultiplier = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out float value)
      {
        value = owner.Duration_FlightTimeMultiplier;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EMinimumReputation\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMinimumReputation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EFailReputationPrice\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EFailReputationPrice\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EMinimumMoney\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMinimumMoney\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EMoneyReputationCoeficient\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMoneyReputationCoeficient\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EMinStartingDeposit\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMinStartingDeposit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EMaxStartingDeposit\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMaxStartingDeposit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EDurationMultiplier\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EDurationMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in double value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out double value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EChancesPerFactionType\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EChancesPerFactionType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, MyContractChancePair[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in MyContractChancePair[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out MyContractChancePair[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeEscortDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeEscortDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeEscortDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ContractTypeEscortDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ContractTypeEscortDefinition();

      MyObjectBuilder_ContractTypeEscortDefinition IActivator<MyObjectBuilder_ContractTypeEscortDefinition>.CreateInstance() => new MyObjectBuilder_ContractTypeEscortDefinition();
    }
  }
}
