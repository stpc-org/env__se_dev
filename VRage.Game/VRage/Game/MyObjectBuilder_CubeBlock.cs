// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_CubeBlock
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_CubeBlock : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    [DefaultValue(0)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public long EntityId;
    [ProtoMember(4)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string Name;
    [ProtoMember(7)]
    [Serialize(MyPrimitiveFlags.Variant, Kind = MySerializeKind.Item)]
    public SerializableVector3I Min = new SerializableVector3I(0, 0, 0);
    private SerializableQuaternion m_orientation;
    [ProtoMember(10)]
    [DefaultValue(1f)]
    [Serialize(MyPrimitiveFlags.Normalized | MyPrimitiveFlags.FixedPoint16)]
    public float IntegrityPercent = 1f;
    [ProtoMember(13)]
    [DefaultValue(1f)]
    [Serialize(MyPrimitiveFlags.Normalized | MyPrimitiveFlags.FixedPoint16)]
    public float BuildPercent = 1f;
    [ProtoMember(16)]
    public SerializableBlockOrientation BlockOrientation = SerializableBlockOrientation.Identity;
    [ProtoMember(19)]
    [DefaultValue(null)]
    [NoSerialize]
    public MyObjectBuilder_Inventory ConstructionInventory;
    [ProtoMember(22)]
    public SerializableVector3 ColorMaskHSV = new SerializableVector3(0.0f, -1f, 0.0f);
    [ProtoMember(25)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string SkinSubtypeId;
    [ProtoMember(28)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public MyObjectBuilder_ConstructionStockpile ConstructionStockpile;
    [ProtoMember(31)]
    [DefaultValue(0)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public long Owner;
    [ProtoMember(34)]
    [DefaultValue(0)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public long BuiltBy;
    [ProtoMember(37)]
    [DefaultValue(MyOwnershipShareModeEnum.None)]
    public MyOwnershipShareModeEnum ShareMode;
    [ProtoMember(40)]
    [DefaultValue(0)]
    [NoSerialize]
    public float DeformationRatio;
    [XmlArrayItem("SubBlock")]
    [ProtoMember(52)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public MyObjectBuilder_CubeBlock.MySubBlockId[] SubBlocks;
    [ProtoMember(55)]
    [DefaultValue(0)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public int MultiBlockId;
    [ProtoMember(58)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public SerializableDefinitionId? MultiBlockDefinition;
    [ProtoMember(61)]
    [DefaultValue(-1)]
    [Serialize]
    public int MultiBlockIndex = -1;
    [ProtoMember(64)]
    [DefaultValue(1f)]
    [Serialize]
    public float BlockGeneralDamageModifier = 1f;
    [ProtoMember(67)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public MyObjectBuilder_ComponentContainer ComponentContainer;

    public bool ShouldSerializeEntityId() => (ulong) this.EntityId > 0UL;

    public bool ShouldSerializeMin() => this.Min != new SerializableVector3I(0, 0, 0);

    [NoSerialize]
    public SerializableQuaternion Orientation
    {
      get => this.m_orientation;
      set
      {
        this.m_orientation = MyUtils.IsZero((Quaternion) value) ? (SerializableQuaternion) Quaternion.Identity : value;
        this.BlockOrientation = new SerializableBlockOrientation(Base6Directions.GetForward((Quaternion) this.m_orientation), Base6Directions.GetUp((Quaternion) this.m_orientation));
      }
    }

    public bool ShouldSerializeOrientation() => false;

    public bool ShouldSerializeBlockOrientation() => this.BlockOrientation != SerializableBlockOrientation.Identity;

    public bool ShouldSerializeConstructionInventory() => false;

    public bool ShouldSerializeColorMaskHSV() => this.ColorMaskHSV != new SerializableVector3(0.0f, -1f, 0.0f);

    public bool ShouldSerializeSkinSubtypeId() => !string.IsNullOrEmpty(this.SkinSubtypeId);

    public static MyObjectBuilder_CubeBlock Upgrade(
      MyObjectBuilder_CubeBlock cubeBlock,
      MyObjectBuilderType newType,
      string newSubType)
    {
      if (!(MyObjectBuilderSerializer.CreateNewObject(newType, newSubType) is MyObjectBuilder_CubeBlock newObject))
        return (MyObjectBuilder_CubeBlock) null;
      newObject.EntityId = cubeBlock.EntityId;
      newObject.Name = cubeBlock.Name;
      newObject.Owner = cubeBlock.Owner;
      newObject.ShareMode = cubeBlock.ShareMode;
      newObject.BuiltBy = cubeBlock.BuiltBy;
      newObject.SkinSubtypeId = cubeBlock.SkinSubtypeId;
      newObject.ConstructionStockpile = cubeBlock.ConstructionStockpile;
      newObject.ComponentContainer = cubeBlock.ComponentContainer;
      newObject.Min = cubeBlock.Min;
      newObject.m_orientation = cubeBlock.m_orientation;
      newObject.IntegrityPercent = cubeBlock.IntegrityPercent;
      newObject.BuildPercent = cubeBlock.BuildPercent;
      newObject.BlockOrientation = cubeBlock.BlockOrientation;
      newObject.ConstructionInventory = cubeBlock.ConstructionInventory;
      newObject.ColorMaskHSV = cubeBlock.ColorMaskHSV;
      return newObject;
    }

    public bool ShouldSerializeConstructionStockpile() => this.ConstructionStockpile != null;

    public bool ShouldSerializeMultiBlockId() => (uint) this.MultiBlockId > 0U;

    public bool ShouldSerializeMultiBlockDefinition() => this.MultiBlockId != 0 && this.MultiBlockDefinition.HasValue;

    public bool ShouldSerializeComponentContainer() => this.ComponentContainer != null && this.ComponentContainer.Components != null && this.ComponentContainer.Components.Count > 0;

    public virtual void Remap(IMyRemapHelper remapHelper)
    {
      if (this.EntityId != 0L)
        this.EntityId = remapHelper.RemapEntityId(this.EntityId);
      if (!string.IsNullOrEmpty(this.Name))
        this.Name = remapHelper.RemapEntityName(this.EntityId);
      if (this.SubBlocks != null)
      {
        for (int index = 0; index < this.SubBlocks.Length; ++index)
        {
          if (this.SubBlocks[index].SubGridId != 0L)
            this.SubBlocks[index].SubGridId = remapHelper.RemapEntityId(this.SubBlocks[index].SubGridId);
        }
      }
      if (this.MultiBlockId == 0 || !this.MultiBlockDefinition.HasValue)
        return;
      this.MultiBlockId = remapHelper.RemapGroupId("MultiBlockId", this.MultiBlockId);
    }

    public virtual void SetupForProjector()
    {
      this.Owner = 0L;
      this.ShareMode = MyOwnershipShareModeEnum.None;
      if (this.ConstructionStockpile != null && this.ConstructionStockpile.Items != null && this.ConstructionStockpile.Items.Length != 0)
        this.ConstructionStockpile.Items = new MyObjectBuilder_StockpileItem[0];
      if (this.ConstructionInventory != null)
        this.ConstructionInventory.Clear();
      if (this.ComponentContainer == null)
        return;
      MyObjectBuilder_ComponentContainer.ComponentData componentData = this.ComponentContainer.Components.Find((Predicate<MyObjectBuilder_ComponentContainer.ComponentData>) (s => s.Component.TypeId == typeof (MyObjectBuilder_Inventory)));
      if (componentData != null)
        (componentData.Component as MyObjectBuilder_Inventory).Clear();
      foreach (MyObjectBuilder_ComponentContainer.ComponentData component in this.ComponentContainer.Components)
      {
        if (component.Component.TypeId == typeof (MyObjectBuilder_InventoryAggregate))
          (component.Component as MyObjectBuilder_InventoryAggregate).Clear();
      }
    }

    public virtual void SetupForGridPaste()
    {
    }

    [ProtoContract]
    public struct MySubBlockId
    {
      [ProtoMember(43)]
      public long SubGridId;
      [ProtoMember(46)]
      public string SubGridName;
      [ProtoMember(49)]
      public SerializableVector3I SubBlockPosition;

      protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMySubBlockId\u003C\u003ESubGridId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock.MySubBlockId, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_CubeBlock.MySubBlockId owner, in long value) => owner.SubGridId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_CubeBlock.MySubBlockId owner, out long value) => value = owner.SubGridId;
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMySubBlockId\u003C\u003ESubGridName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock.MySubBlockId, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_CubeBlock.MySubBlockId owner, in string value) => owner.SubGridName = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_CubeBlock.MySubBlockId owner, out string value) => value = owner.SubGridName;
      }

      protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMySubBlockId\u003C\u003ESubBlockPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock.MySubBlockId, SerializableVector3I>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_CubeBlock.MySubBlockId owner,
          in SerializableVector3I value)
        {
          owner.SubBlockPosition = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_CubeBlock.MySubBlockId owner,
          out SerializableVector3I value)
        {
          value = owner.SubBlockPosition;
        }
      }

      private class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMySubBlockId\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlock.MySubBlockId>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlock.MySubBlockId();

        MyObjectBuilder_CubeBlock.MySubBlockId IActivator<MyObjectBuilder_CubeBlock.MySubBlockId>.CreateInstance() => new MyObjectBuilder_CubeBlock.MySubBlockId();
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in long value) => owner.EntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out long value) => value = owner.EntityId;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in SerializableVector3I value) => owner.Min = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out SerializableVector3I value) => value = owner.Min;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003Em_orientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in SerializableQuaternion value) => owner.m_orientation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out SerializableQuaternion value) => value = owner.m_orientation;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EIntegrityPercent\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in float value) => owner.IntegrityPercent = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out float value) => value = owner.IntegrityPercent;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBuildPercent\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in float value) => owner.BuildPercent = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out float value) => value = owner.BuildPercent;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBlockOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, SerializableBlockOrientation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlock owner,
        in SerializableBlockOrientation value)
      {
        owner.BlockOrientation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlock owner,
        out SerializableBlockOrientation value)
      {
        value = owner.BlockOrientation;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EConstructionInventory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, MyObjectBuilder_Inventory>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlock owner,
        in MyObjectBuilder_Inventory value)
      {
        owner.ConstructionInventory = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlock owner,
        out MyObjectBuilder_Inventory value)
      {
        value = owner.ConstructionInventory;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EColorMaskHSV\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in SerializableVector3 value) => owner.ColorMaskHSV = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out SerializableVector3 value) => value = owner.ColorMaskHSV;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESkinSubtypeId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in string value) => owner.SkinSubtypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out string value) => value = owner.SkinSubtypeId;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EConstructionStockpile\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, MyObjectBuilder_ConstructionStockpile>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlock owner,
        in MyObjectBuilder_ConstructionStockpile value)
      {
        owner.ConstructionStockpile = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlock owner,
        out MyObjectBuilder_ConstructionStockpile value)
      {
        value = owner.ConstructionStockpile;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EOwner\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in long value) => owner.Owner = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out long value) => value = owner.Owner;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBuiltBy\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in long value) => owner.BuiltBy = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out long value) => value = owner.BuiltBy;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EShareMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, MyOwnershipShareModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlock owner,
        in MyOwnershipShareModeEnum value)
      {
        owner.ShareMode = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlock owner,
        out MyOwnershipShareModeEnum value)
      {
        value = owner.ShareMode;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EDeformationRatio\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in float value) => owner.DeformationRatio = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out float value) => value = owner.DeformationRatio;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESubBlocks\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, MyObjectBuilder_CubeBlock.MySubBlockId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlock owner,
        in MyObjectBuilder_CubeBlock.MySubBlockId[] value)
      {
        owner.SubBlocks = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlock owner,
        out MyObjectBuilder_CubeBlock.MySubBlockId[] value)
      {
        value = owner.SubBlocks;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in int value) => owner.MultiBlockId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out int value) => value = owner.MultiBlockId;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockDefinition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlock owner,
        in SerializableDefinitionId? value)
      {
        owner.MultiBlockDefinition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlock owner,
        out SerializableDefinitionId? value)
      {
        value = owner.MultiBlockDefinition;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockIndex\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in int value) => owner.MultiBlockIndex = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out int value) => value = owner.MultiBlockIndex;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBlockGeneralDamageModifier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in float value) => owner.BlockGeneralDamageModifier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out float value) => value = owner.BlockGeneralDamageModifier;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EComponentContainer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CubeBlock owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        owner.ComponentContainer = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CubeBlock owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        value = owner.ComponentContainer;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlock, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CubeBlock, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in SerializableQuaternion value) => owner.Orientation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out SerializableQuaternion value) => value = owner.Orientation;
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlock, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CubeBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CubeBlock owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CubeBlock owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CubeBlock>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CubeBlock();

      MyObjectBuilder_CubeBlock IActivator<MyObjectBuilder_CubeBlock>.CreateInstance() => new MyObjectBuilder_CubeBlock();
    }
  }
}
