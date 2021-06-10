// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_FactionTypeDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
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
  public class MyObjectBuilder_FactionTypeDefinition : MyObjectBuilder_DefinitionBase
  {
    [XmlArrayItem("ItemId")]
    [ProtoMember(1)]
    [DefaultValue(null)]
    public SerializableDefinitionId[] OffersList;
    [XmlArrayItem("ItemId")]
    [ProtoMember(3)]
    [DefaultValue(null)]
    public SerializableDefinitionId[] OrdersList;
    [ProtoMember(5)]
    public bool CanSellOxygen;
    [ProtoMember(6)]
    public bool CanSellHydrogen;
    [ProtoMember(7)]
    public float OfferPriceUpMultiplierMax = 1.1f;
    [ProtoMember(9)]
    public float OfferPriceUpMultiplierMin = 1.05f;
    [ProtoMember(11)]
    public float OfferPriceDownMultiplierMax = 0.925f;
    [ProtoMember(13)]
    public float OfferPriceDownMultiplierMin = 0.9f;
    [ProtoMember(15)]
    public float OfferPriceUpDownPoint = 0.5f;
    [ProtoMember(17)]
    public float OfferPriceBellowMinimumMultiplier = 0.85f;
    [ProtoMember(19)]
    public float OfferPriceStartingMultiplier = 1.2f;
    [ProtoMember(21)]
    public byte OfferMaxUpdateCount = 3;
    [ProtoMember(23)]
    public float OrderPriceStartingMultiplier = 0.75f;
    [ProtoMember(25)]
    public float OrderPriceUpMultiplierMax = 1.1f;
    [ProtoMember(27)]
    public float OrderPriceUpMultiplierMin = 1.05f;
    [ProtoMember(29)]
    public float OrderPriceDownMultiplierMax = 0.95f;
    [ProtoMember(31)]
    public float OrderPriceDownMultiplierMin = 0.75f;
    [ProtoMember(33)]
    public float OrderPriceOverMinimumMultiplier = 1.1f;
    [ProtoMember(35)]
    public float OrderPriceUpDownPoint = 0.25f;
    [ProtoMember(37)]
    public byte OrderMaxUpdateCount = 4;
    [ProtoMember(39)]
    public int MinimumOfferGasAmount = 100;
    [ProtoMember(41)]
    public int MaximumOfferGasAmount = 10000;
    [ProtoMember(45)]
    public float BaseCostProductionSpeedMultiplier = 1f;
    [ProtoMember(47)]
    public int MinimumOxygenPrice = 150;
    [ProtoMember(49)]
    public int MinimumHydrogenPrice = 150;
    [XmlArrayItem("PrefabName")]
    [ProtoMember(51)]
    [DefaultValue(null)]
    public string[] GridsForSale;

    [ProtoMember(63)]
    public int MaxContractCount { get; set; }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOffersList\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, SerializableDefinitionId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionTypeDefinition owner,
        in SerializableDefinitionId[] value)
      {
        owner.OffersList = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionTypeDefinition owner,
        out SerializableDefinitionId[] value)
      {
        value = owner.OffersList;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOrdersList\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, SerializableDefinitionId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionTypeDefinition owner,
        in SerializableDefinitionId[] value)
      {
        owner.OrdersList = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionTypeDefinition owner,
        out SerializableDefinitionId[] value)
      {
        value = owner.OrdersList;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003ECanSellOxygen\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in bool value) => owner.CanSellOxygen = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out bool value) => value = owner.CanSellOxygen;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003ECanSellHydrogen\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in bool value) => owner.CanSellHydrogen = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out bool value) => value = owner.CanSellHydrogen;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOfferPriceUpMultiplierMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OfferPriceUpMultiplierMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OfferPriceUpMultiplierMax;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOfferPriceUpMultiplierMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OfferPriceUpMultiplierMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OfferPriceUpMultiplierMin;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOfferPriceDownMultiplierMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OfferPriceDownMultiplierMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OfferPriceDownMultiplierMax;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOfferPriceDownMultiplierMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OfferPriceDownMultiplierMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OfferPriceDownMultiplierMin;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOfferPriceUpDownPoint\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OfferPriceUpDownPoint = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OfferPriceUpDownPoint;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOfferPriceBellowMinimumMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OfferPriceBellowMinimumMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OfferPriceBellowMinimumMultiplier;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOfferPriceStartingMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OfferPriceStartingMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OfferPriceStartingMultiplier;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOfferMaxUpdateCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in byte value) => owner.OfferMaxUpdateCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out byte value) => value = owner.OfferMaxUpdateCount;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOrderPriceStartingMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OrderPriceStartingMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OrderPriceStartingMultiplier;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOrderPriceUpMultiplierMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OrderPriceUpMultiplierMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OrderPriceUpMultiplierMax;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOrderPriceUpMultiplierMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OrderPriceUpMultiplierMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OrderPriceUpMultiplierMin;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOrderPriceDownMultiplierMax\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OrderPriceDownMultiplierMax = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OrderPriceDownMultiplierMax;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOrderPriceDownMultiplierMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OrderPriceDownMultiplierMin = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OrderPriceDownMultiplierMin;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOrderPriceOverMinimumMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OrderPriceOverMinimumMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OrderPriceOverMinimumMultiplier;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOrderPriceUpDownPoint\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.OrderPriceUpDownPoint = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.OrderPriceUpDownPoint;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EOrderMaxUpdateCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in byte value) => owner.OrderMaxUpdateCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out byte value) => value = owner.OrderMaxUpdateCount;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EMinimumOfferGasAmount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in int value) => owner.MinimumOfferGasAmount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out int value) => value = owner.MinimumOfferGasAmount;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EMaximumOfferGasAmount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in int value) => owner.MaximumOfferGasAmount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out int value) => value = owner.MaximumOfferGasAmount;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EBaseCostProductionSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in float value) => owner.BaseCostProductionSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out float value) => value = owner.BaseCostProductionSpeedMultiplier;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EMinimumOxygenPrice\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in int value) => owner.MinimumOxygenPrice = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out int value) => value = owner.MinimumOxygenPrice;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EMinimumHydrogenPrice\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in int value) => owner.MinimumHydrogenPrice = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out int value) => value = owner.MinimumHydrogenPrice;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EGridsForSale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string[] value) => owner.GridsForSale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string[] value) => value = owner.GridsForSale;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionTypeDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionTypeDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionTypeDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionTypeDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EMaxContractCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in int value) => owner.MaxContractCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out int value) => value = owner.MaxContractCount;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FactionTypeDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FactionTypeDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FactionTypeDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FactionTypeDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FactionTypeDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_FactionTypeDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FactionTypeDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FactionTypeDefinition();

      MyObjectBuilder_FactionTypeDefinition IActivator<MyObjectBuilder_FactionTypeDefinition>.CreateInstance() => new MyObjectBuilder_FactionTypeDefinition();
    }
  }
}
