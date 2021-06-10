// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_ContractTypeHuntDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ContractTypeHuntDefinition : MyObjectBuilder_ContractTypeDefinition
  {
    [ProtoMember(1)]
    public int RemarkPeriodInS;
    [ProtoMember(3)]
    public float RemarkVariance;
    [ProtoMember(5)]
    public double KillRange;
    [ProtoMember(7)]
    public float KillRangeMultiplier;
    [ProtoMember(8)]
    public int ReputationLossForTarget;
    [ProtoMember(9)]
    public double RewardRadius;
    [ProtoMember(11)]
    public double Duration_BaseTime;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003ERemarkPeriodInS\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in int value)
      {
        owner.RemarkPeriodInS = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out int value)
      {
        value = owner.RemarkPeriodInS;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003ERemarkVariance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in float value)
      {
        owner.RemarkVariance = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out float value)
      {
        value = owner.RemarkVariance;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EKillRange\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in double value)
      {
        owner.KillRange = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out double value)
      {
        value = owner.KillRange;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EKillRangeMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in float value)
      {
        owner.KillRangeMultiplier = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out float value)
      {
        value = owner.KillRangeMultiplier;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EReputationLossForTarget\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in int value)
      {
        owner.ReputationLossForTarget = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out int value)
      {
        value = owner.ReputationLossForTarget;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003ERewardRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in double value)
      {
        owner.RewardRadius = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out double value)
      {
        value = owner.RewardRadius;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EDuration_BaseTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in double value)
      {
        owner.Duration_BaseTime = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out double value)
      {
        value = owner.Duration_BaseTime;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EMinimumReputation\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMinimumReputation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EFailReputationPrice\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EFailReputationPrice\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EMinimumMoney\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMinimumMoney\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EMoneyReputationCoeficient\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMoneyReputationCoeficient\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EMinStartingDeposit\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMinStartingDeposit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EMaxStartingDeposit\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EMaxStartingDeposit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EDurationMultiplier\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EDurationMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in double value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out double value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EChancesPerFactionType\u003C\u003EAccessor : MyObjectBuilder_ContractTypeDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeDefinition\u003C\u003EChancesPerFactionType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, MyContractChancePair[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in MyContractChancePair[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractTypeDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out MyContractChancePair[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractTypeDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractTypeHuntDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractTypeHuntDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ContractTypeHuntDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ContractTypeHuntDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ContractTypeHuntDefinition();

      MyObjectBuilder_ContractTypeHuntDefinition IActivator<MyObjectBuilder_ContractTypeHuntDefinition>.CreateInstance() => new MyObjectBuilder_ContractTypeHuntDefinition();
    }
  }
}
