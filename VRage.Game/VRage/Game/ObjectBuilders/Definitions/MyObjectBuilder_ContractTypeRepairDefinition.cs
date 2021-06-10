// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_ContractTypeRepairDefinition
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
  public class MyObjectBuilder_ContractTypeRepairDefinition : MyObjectBuilder_ContractTypeDefinition
  {
    [ProtoMember(1)]
    public double MaxGridDistance;
    [ProtoMember(3)]
    public double MinGridDistance;
    [ProtoMember(5)]
    public double Duration_BaseTime;
    [ProtoMember(7)]
    public double Duration_TimePerMeter;
    [ProtoMember(9)]
    public MySerializableList<string> PrefabNames;
    [ProtoMember(11)]
    public float PriceToRewardCoeficient;
    [ProtoMember(13)]
    public float PriceSpread;
    [ProtoMember(15)]
    public float TimeToPriceDenominator;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EMaxGridDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in double value)
      {
        owner.MaxGridDistance = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out double value)
      {
        value = owner.MaxGridDistance;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EMinGridDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in double value)
      {
        owner.MinGridDistance = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out double value)
      {
        value = owner.MinGridDistance;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EDuration_BaseTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in double value)
      {
        owner.Duration_BaseTime = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out double value)
      {
        value = owner.Duration_BaseTime;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EDuration_TimePerMeter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in double value)
      {
        owner.Duration_TimePerMeter = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out double value)
      {
        value = owner.Duration_TimePerMeter;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EPrefabNames\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, MySerializableList<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in MySerializableList<string> value)
      {
        owner.PrefabNames = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out MySerializableList<string> value)
      {
        value = owner.PrefabNames;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EPriceToRewardCoeficient\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in float value)
      {
        owner.PriceToRewardCoeficient = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out float value)
      {
        value = owner.PriceToRewardCoeficient;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EPriceSpread\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in float value)
      {
        owner.PriceSpread = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out float value)
      {
        value = owner.PriceSpread;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003ETimeToPriceDenominator\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in float value)
      {
        owner.TimeToPriceDenominator = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out float value)
      {
        value = owner.TimeToPriceDenominator;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EMinimumReputation\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMinimumReputation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EFailReputationPrice\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EFailReputationPrice\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EMinimumMoney\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMinimumMoney\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EMoneyReputationCoeficient\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMoneyReputationCoeficient\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EMinStartingDeposit\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMinStartingDeposit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EMaxStartingDeposit\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMaxStartingDeposit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EDurationMultiplier\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EDurationMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in double value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out double value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EChancesPerFactionType\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EChancesPerFactionType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, MyContractChancePair[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in MyContractChancePair[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out MyContractChancePair[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeRepairDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeRepairDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeRepairDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ContractTypeRepairDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ContractTypeRepairDefinition();

      MyObjectBuilder_ContractTypeRepairDefinition IActivator<MyObjectBuilder_ContractTypeRepairDefinition>.CreateInstance() => new MyObjectBuilder_ContractTypeRepairDefinition();
    }
  }
}
