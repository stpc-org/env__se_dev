// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.Contracts.MyObjectBuilder_ContractFind
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
  public class MyObjectBuilder_ContractFind : MyObjectBuilder_Contract
  {
    [ProtoMember(1)]
    public SerializableVector3D GridPosition;
    [ProtoMember(3)]
    public SerializableVector3D GpsPosition;
    [ProtoMember(5)]
    public long GridId;
    [ProtoMember(7)]
    public double GpsDistance;
    [ProtoMember(11)]
    public double TriggerRadius;
    [ProtoMember(13)]
    public bool GridFound;
    [ProtoMember(15)]
    public bool KeepGridAtTheEnd;
    [ProtoMember(17)]
    public float MaxGpsOffset;

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EGridPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractFind, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in SerializableVector3D value) => owner.GridPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractFind owner,
        out SerializableVector3D value)
      {
        value = owner.GridPosition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EGpsPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractFind, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in SerializableVector3D value) => owner.GpsPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractFind owner,
        out SerializableVector3D value)
      {
        value = owner.GpsPosition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EGridId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractFind, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in long value) => owner.GridId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out long value) => value = owner.GridId;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EGpsDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractFind, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in double value) => owner.GpsDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out double value) => value = owner.GpsDistance;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003ETriggerRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractFind, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in double value) => owner.TriggerRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out double value) => value = owner.TriggerRadius;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EGridFound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractFind, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in bool value) => owner.GridFound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out bool value) => value = owner.GridFound;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EKeepGridAtTheEnd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractFind, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in bool value) => owner.KeepGridAtTheEnd = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out bool value) => value = owner.KeepGridAtTheEnd;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EMaxGpsOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractFind, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in float value) => owner.MaxGpsOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out float value) => value = owner.MaxGpsOffset;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EIsPlayerMade\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EIsPlayerMade\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in bool value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out bool value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EState\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EState\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, MyContractStateEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in MyContractStateEnum value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out MyContractStateEnum value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EOwners\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EOwners\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, MySerializableList<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractFind owner,
        in MySerializableList<long> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Contract&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractFind owner,
        out MySerializableList<long> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Contract&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003ERewardMoney\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERewardMoney\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003ERewardReputation\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERewardReputation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in int value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out int value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EStartingDeposit\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartingDeposit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EFailReputationPrice\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EFailReputationPrice\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in int value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out int value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EStartFaction\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartFaction\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EStartStation\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartStation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EStartBlock\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartBlock\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003ECreation\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ECreation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003ETicksToDiscard\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ETicksToDiscard\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in int? value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out int? value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003ERemainingTimeInS\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERemainingTimeInS\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, double?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in double? value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out double? value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EContractCondition\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EContractCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, MyObjectBuilder_ContractCondition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractFind owner,
        in MyObjectBuilder_ContractCondition value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Contract&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractFind owner,
        out MyObjectBuilder_ContractCondition value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Contract&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractFind, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractFind owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractFind owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractFind\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ContractFind>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ContractFind();

      MyObjectBuilder_ContractFind IActivator<MyObjectBuilder_ContractFind>.CreateInstance() => new MyObjectBuilder_ContractFind();
    }
  }
}
