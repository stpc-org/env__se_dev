// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.Contracts.MyObjectBuilder_Contract
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components.Contracts
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Contract : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public long Id;
    [ProtoMember(2)]
    public bool IsPlayerMade;
    [ProtoMember(3)]
    public MyContractStateEnum State;
    [ProtoMember(5)]
    public MySerializableList<long> Owners;
    [ProtoMember(7)]
    public long RewardMoney;
    [ProtoMember(9)]
    public int RewardReputation;
    [ProtoMember(11)]
    public long StartingDeposit;
    [ProtoMember(13)]
    public int FailReputationPrice;
    [ProtoMember(15)]
    public long StartFaction;
    [ProtoMember(17)]
    public long StartStation;
    [ProtoMember(18)]
    public long StartBlock;
    [ProtoMember(19)]
    public long Creation;
    [ProtoMember(21)]
    public int? TicksToDiscard;
    [ProtoMember(23)]
    public double? RemainingTimeInS;
    [ProtoMember(25)]
    [Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof (MyObjectBuilderDynamicSerializer))]
    [XmlElement(Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_ContractCondition>))]
    public MyObjectBuilder_ContractCondition ContractCondition;

    public MyObjectBuilder_Contract() => this.Owners = new MySerializableList<long>();

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in long value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out long value) => value = owner.Id;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EIsPlayerMade\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in bool value) => owner.IsPlayerMade = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out bool value) => value = owner.IsPlayerMade;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EState\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, MyContractStateEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in MyContractStateEnum value) => owner.State = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out MyContractStateEnum value) => value = owner.State;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EOwners\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, MySerializableList<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in MySerializableList<long> value) => owner.Owners = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Contract owner,
        out MySerializableList<long> value)
      {
        value = owner.Owners;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERewardMoney\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in long value) => owner.RewardMoney = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out long value) => value = owner.RewardMoney;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERewardReputation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in int value) => owner.RewardReputation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out int value) => value = owner.RewardReputation;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartingDeposit\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in long value) => owner.StartingDeposit = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out long value) => value = owner.StartingDeposit;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EFailReputationPrice\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in int value) => owner.FailReputationPrice = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out int value) => value = owner.FailReputationPrice;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartFaction\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in long value) => owner.StartFaction = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out long value) => value = owner.StartFaction;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartStation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in long value) => owner.StartStation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out long value) => value = owner.StartStation;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartBlock\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in long value) => owner.StartBlock = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out long value) => value = owner.StartBlock;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ECreation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in long value) => owner.Creation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out long value) => value = owner.Creation;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ETicksToDiscard\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in int? value) => owner.TicksToDiscard = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out int? value) => value = owner.TicksToDiscard;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERemainingTimeInS\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, double?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in double? value) => owner.RemainingTimeInS = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out double? value) => value = owner.RemainingTimeInS;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EContractCondition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Contract, MyObjectBuilder_ContractCondition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Contract owner,
        in MyObjectBuilder_ContractCondition value)
      {
        owner.ContractCondition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Contract owner,
        out MyObjectBuilder_ContractCondition value)
      {
        value = owner.ContractCondition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Contract, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Contract, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Contract, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Contract, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Contract owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Contract owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Contract>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Contract();

      MyObjectBuilder_Contract IActivator<MyObjectBuilder_Contract>.CreateInstance() => new MyObjectBuilder_Contract();
    }
  }
}
