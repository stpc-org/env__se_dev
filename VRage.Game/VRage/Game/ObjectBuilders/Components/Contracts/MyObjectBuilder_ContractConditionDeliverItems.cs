// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.Contracts.MyObjectBuilder_ContractConditionDeliverItems
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components.Contracts
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ContractConditionDeliverItems : MyObjectBuilder_ContractCondition
  {
    [ProtoMember(1)]
    public SerializableDefinitionId ItemType;
    [ProtoMember(3)]
    public int ItemAmount;
    [ProtoMember(4)]
    public float ItemVolume;
    [ProtoMember(5)]
    public bool TransferItems;

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003EItemType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in SerializableDefinitionId value)
      {
        owner.ItemType = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out SerializableDefinitionId value)
      {
        value = owner.ItemType;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003EItemAmount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in int value)
      {
        owner.ItemAmount = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out int value)
      {
        value = owner.ItemAmount;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003EItemVolume\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in float value)
      {
        owner.ItemVolume = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out float value)
      {
        value = owner.ItemVolume;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003ETransferItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in bool value)
      {
        owner.TransferItems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out bool value)
      {
        value = owner.TransferItems;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_ContractCondition.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractCondition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractCondition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003EIsFinished\u003C\u003EAccessor : MyObjectBuilder_ContractCondition.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition\u003C\u003EIsFinished\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractCondition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractCondition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003EContractId\u003C\u003EAccessor : MyObjectBuilder_ContractCondition.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition\u003C\u003EContractId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractCondition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractCondition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003ESubId\u003C\u003EAccessor : MyObjectBuilder_ContractCondition.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition\u003C\u003ESubId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractCondition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractCondition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003EStationEndId\u003C\u003EAccessor : MyObjectBuilder_ContractCondition.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition\u003C\u003EStationEndId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractCondition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractCondition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003EFactionEndId\u003C\u003EAccessor : MyObjectBuilder_ContractCondition.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition\u003C\u003EFactionEndId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractCondition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractCondition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003EBlockEndId\u003C\u003EAccessor : MyObjectBuilder_ContractCondition.VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition\u003C\u003EBlockEndId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in long value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_ContractCondition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out long value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_ContractCondition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ContractConditionDeliverItems, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ContractConditionDeliverItems owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractConditionDeliverItems\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ContractConditionDeliverItems>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ContractConditionDeliverItems();

      MyObjectBuilder_ContractConditionDeliverItems IActivator<MyObjectBuilder_ContractConditionDeliverItems>.CreateInstance() => new MyObjectBuilder_ContractConditionDeliverItems();
    }
  }
}
