// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.Contracts.MyObjectBuilder_ContractEscort
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
  public class MyObjectBuilder_ContractEscort : MyObjectBuilder_Contract
  {
    [ProtoMember(1)]
    public long GridId;
    [ProtoMember(3)]
    public SerializableVector3D StartPosition;
    [ProtoMember(5)]
    public SerializableVector3D EndPosition;
    [ProtoMember(7)]
    public double PathLength;
    [ProtoMember(10)]
    public double RewardRadius;
    [ProtoMember(11)]
    public long TriggerEntityId;
    [ProtoMember(13)]
    public double TriggerRadius;
    [ProtoMember(15)]
    public long DroneAttackPeriod;
    [ProtoMember(17)]
    public long DroneFirstDelay;
    [ProtoMember(19)]
    public long InnerTimer;
    [ProtoMember(21)]
    public long InitialDelay;
    [ProtoMember(22)]
    public int DronesPerWave;
    [ProtoMember(23)]
    public bool IsBehaviorAttached;
    [ProtoMember(24)]
    public long WaveFactionId;
    [ProtoMember(25)]
    public MySerializableList<long> Drones;
    [ProtoMember(27)]
    public long EscortShipOwner;
    [ProtoMember(29)]
    public bool DestinationReached;

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EGridId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => owner.GridId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => value = owner.GridId;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EStartPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractEscort owner,
        in SerializableVector3D value)
      {
        owner.StartPosition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractEscort owner,
        out SerializableVector3D value)
      {
        value = owner.StartPosition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EEndPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, SerializableVector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractEscort owner,
        in SerializableVector3D value)
      {
        owner.EndPosition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractEscort owner,
        out SerializableVector3D value)
      {
        value = owner.EndPosition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EPathLength\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in double value) => owner.PathLength = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out double value) => value = owner.PathLength;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003ERewardRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in double value) => owner.RewardRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out double value) => value = owner.RewardRadius;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003ETriggerEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => owner.TriggerEntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => value = owner.TriggerEntityId;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003ETriggerRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in double value) => owner.TriggerRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out double value) => value = owner.TriggerRadius;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EDroneAttackPeriod\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => owner.DroneAttackPeriod = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => value = owner.DroneAttackPeriod;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EDroneFirstDelay\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => owner.DroneFirstDelay = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => value = owner.DroneFirstDelay;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EInnerTimer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => owner.InnerTimer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => value = owner.InnerTimer;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EInitialDelay\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => owner.InitialDelay = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => value = owner.InitialDelay;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EDronesPerWave\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in int value) => owner.DronesPerWave = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out int value) => value = owner.DronesPerWave;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EIsBehaviorAttached\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in bool value) => owner.IsBehaviorAttached = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out bool value) => value = owner.IsBehaviorAttached;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EWaveFactionId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => owner.WaveFactionId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => value = owner.WaveFactionId;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EDrones\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, MySerializableList<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractEscort owner,
        in MySerializableList<long> value)
      {
        owner.Drones = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractEscort owner,
        out MySerializableList<long> value)
      {
        value = owner.Drones;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EEscortShipOwner\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => owner.EscortShipOwner = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => value = owner.EscortShipOwner;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EDestinationReached\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractEscort, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in bool value) => owner.DestinationReached = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out bool value) => value = owner.DestinationReached;
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EIsPlayerMade\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EIsPlayerMade\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in bool value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out bool value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EState\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EState\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, MyContractStateEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractEscort owner,
        in MyContractStateEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Contract&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractEscort owner,
        out MyContractStateEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Contract&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EOwners\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EOwners\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, MySerializableList<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractEscort owner,
        in MySerializableList<long> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Contract&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractEscort owner,
        out MySerializableList<long> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Contract&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003ERewardMoney\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERewardMoney\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003ERewardReputation\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERewardReputation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in int value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out int value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EStartingDeposit\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartingDeposit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EFailReputationPrice\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EFailReputationPrice\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in int value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out int value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EStartFaction\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartFaction\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EStartStation\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartStation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EStartBlock\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EStartBlock\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003ECreation\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ECreation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in long value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out long value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003ETicksToDiscard\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ETicksToDiscard\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in int? value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out int? value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003ERemainingTimeInS\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003ERemainingTimeInS\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, double?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in double? value) => this.Set((MyObjectBuilder_Contract&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out double? value) => this.Get((MyObjectBuilder_Contract&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EContractCondition\u003C\u003EAccessor : MyObjectBuilder_Contract.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003C\u003EContractCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, MyObjectBuilder_ContractCondition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractEscort owner,
        in MyObjectBuilder_ContractCondition value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Contract&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractEscort owner,
        out MyObjectBuilder_ContractCondition value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Contract&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractEscort, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ContractEscort owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ContractEscort owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractEscort\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ContractEscort>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ContractEscort();

      MyObjectBuilder_ContractEscort IActivator<MyObjectBuilder_ContractEscort>.CreateInstance() => new MyObjectBuilder_ContractEscort();
    }
  }
}
