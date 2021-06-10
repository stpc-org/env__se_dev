// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.Contracts.MyObjectBuilder_ContractObtainAndDeliver
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

namespace VRage.Game.ObjectBuilders.Components.Contracts
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ContractObtainAndDeliver : MyObjectBuilder_Contract
  {
    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003EIsPlayerMade\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EIsPlayerMade\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in bool value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out bool value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003EState\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EState\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, MyContractStateEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractObtainAndDeliver owner,
        in MyContractStateEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Contract&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractObtainAndDeliver owner,
        out MyContractStateEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Contract&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003EOwners\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EOwners\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, MySerializableList<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractObtainAndDeliver owner,
        in MySerializableList<long> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Contract&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractObtainAndDeliver owner,
        out MySerializableList<long> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Contract&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003ERewardMoney\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERewardMoney\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003ERewardReputation\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERewardReputation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in int value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out int value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003EStartingDeposit\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartingDeposit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003EFailReputationPrice\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EFailReputationPrice\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in int value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out int value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003EStartFaction\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartFaction\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003EStartStation\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartStation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003EStartBlock\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartBlock\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003ECreation\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ECreation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003ETicksToDiscard\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ETicksToDiscard\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in int? value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out int? value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003ERemainingTimeInS\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERemainingTimeInS\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, double?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in double? value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out double? value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003EContractCondition\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EContractCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, MyObjectBuilder_ContractCondition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractObtainAndDeliver owner,
        in MyObjectBuilder_ContractCondition value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Contract&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractObtainAndDeliver owner,
        out MyObjectBuilder_ContractCondition value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Contract&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractObtainAndDeliver owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractObtainAndDeliver owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractObtainAndDeliver owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractObtainAndDeliver owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractObtainAndDeliver, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractObtainAndDeliver owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractObtainAndDeliver owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractObtainAndDeliver\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ContractObtainAndDeliver>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ContractObtainAndDeliver();

      MyObjectBuilder_ContractObtainAndDeliver IActivator<MyObjectBuilder_ContractObtainAndDeliver>.CreateInstance() => new MyObjectBuilder_ContractObtainAndDeliver();
    }
  }
}
