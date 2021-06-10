// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.Contracts.MyObjectBuilder_ContractRepair
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
using VRageMath;

namespace VRage.Game.ObjectBuilders.Components.Contracts
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ContractRepair : MyObjectBuilder_Contract
  {
    [ProtoMember(1)]
    public SerializableVector3D GridPosition;
    [ProtoMember(3)]
    public long GridId;
    [ProtoMember(5)]
    public string PrefabName;
    [ProtoMember(7)]
    public MySerializableList<Vector3I> BlocksToRepair;
    [ProtoMember(9)]
    public bool KeepGridAtTheEnd;
    [ProtoMember(11)]
    public int UnrepairedBlockCount;

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EGridPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractRepair, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractRepair owner,
        in SerializableVector3D value)
      {
        owner.GridPosition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractRepair owner,
        out SerializableVector3D value)
      {
        value = owner.GridPosition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EGridId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractRepair, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in long value) => owner.GridId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out long value) => value = owner.GridId;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EPrefabName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractRepair, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in string value) => owner.PrefabName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out string value) => value = owner.PrefabName;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EBlocksToRepair\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractRepair, MySerializableList<Vector3I>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractRepair owner,
        in MySerializableList<Vector3I> value)
      {
        owner.BlocksToRepair = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractRepair owner,
        out MySerializableList<Vector3I> value)
      {
        value = owner.BlocksToRepair;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EKeepGridAtTheEnd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractRepair, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in bool value) => owner.KeepGridAtTheEnd = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out bool value) => value = owner.KeepGridAtTheEnd;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EUnrepairedBlockCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractRepair, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in int value) => owner.UnrepairedBlockCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out int value) => value = owner.UnrepairedBlockCount;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EIsPlayerMade\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EIsPlayerMade\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in bool value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out bool value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EState\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EState\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, MyContractStateEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractRepair owner,
        in MyContractStateEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Contract&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractRepair owner,
        out MyContractStateEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Contract&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EOwners\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EOwners\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, MySerializableList<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractRepair owner,
        in MySerializableList<long> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Contract&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractRepair owner,
        out MySerializableList<long> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Contract&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003ERewardMoney\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERewardMoney\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003ERewardReputation\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERewardReputation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in int value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out int value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EStartingDeposit\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartingDeposit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EFailReputationPrice\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EFailReputationPrice\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in int value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out int value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EStartFaction\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartFaction\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EStartStation\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartStation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EStartBlock\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartBlock\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003ECreation\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ECreation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003ETicksToDiscard\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ETicksToDiscard\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in int? value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out int? value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003ERemainingTimeInS\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERemainingTimeInS\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, double?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in double? value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out double? value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EContractCondition\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EContractCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, MyObjectBuilder_ContractCondition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractRepair owner,
        in MyObjectBuilder_ContractCondition value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Contract&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractRepair owner,
        out MyObjectBuilder_ContractCondition value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Contract&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractRepair, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractRepair owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractRepair owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractRepair\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ContractRepair>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ContractRepair();

      MyObjectBuilder_ContractRepair IActivator<MyObjectBuilder_ContractRepair>.CreateInstance() => new MyObjectBuilder_ContractRepair();
    }
  }
}
