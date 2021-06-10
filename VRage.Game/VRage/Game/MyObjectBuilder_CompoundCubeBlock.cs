// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_CompoundCubeBlock
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_CompoundCubeBlock : MyObjectBuilder_CubeBlock
  {
    [ProtoMember(1)]
    [DynamicItem(typeof (MyObjectBuilderDynamicSerializer), true)]
    [XmlArrayItem("MyObjectBuilder_CubeBlock", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_CubeBlock>))]
    public MyObjectBuilder_CubeBlock[] Blocks;
    [ProtoMember(4)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public ushort[] BlockIds;

    public override void Remap(IMyRemapHelper remapHelper)
    {
      base.Remap(remapHelper);
      foreach (MyObjectBuilder_CubeBlock block in this.Blocks)
        block.Remap(remapHelper);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EBlocks\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyObjectBuilder_CubeBlock[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        in MyObjectBuilder_CubeBlock[] value)
      {
        owner.Blocks = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        out MyObjectBuilder_CubeBlock[] value)
      {
        value = owner.Blocks;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EBlockIds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, ushort[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in ushort[] value) => owner.BlockIds = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out ushort[] value) => value = owner.BlockIds;
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in string value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out string value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EMin\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMin\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        in SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        out SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003Em_orientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003Em_orientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        in SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        out SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EIntegrityPercent\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EIntegrityPercent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EBuildPercent\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBuildPercent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EBlockOrientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBlockOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, SerializableBlockOrientation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        in SerializableBlockOrientation value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        out SerializableBlockOrientation value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EConstructionInventory\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EConstructionInventory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyObjectBuilder_Inventory>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        in MyObjectBuilder_Inventory value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        out MyObjectBuilder_Inventory value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EColorMaskHSV\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EColorMaskHSV\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        in SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        out SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003ESkinSubtypeId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESkinSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in string value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out string value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EConstructionStockpile\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EConstructionStockpile\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyObjectBuilder_ConstructionStockpile>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        in MyObjectBuilder_ConstructionStockpile value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        out MyObjectBuilder_ConstructionStockpile value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EOwner\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EOwner\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EBuiltBy\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBuiltBy\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EShareMode\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EShareMode\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyOwnershipShareModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        in MyOwnershipShareModeEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        out MyOwnershipShareModeEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EDeformationRatio\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EDeformationRatio\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003ESubBlocks\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESubBlocks\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyObjectBuilder_CubeBlock.MySubBlockId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        in MyObjectBuilder_CubeBlock.MySubBlockId[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        out MyObjectBuilder_CubeBlock.MySubBlockId[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EMultiBlockId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in int value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out int value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EMultiBlockDefinition\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EMultiBlockIndex\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockIndex\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in int value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out int value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EBlockGeneralDamageModifier\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBlockGeneralDamageModifier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EOrientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        in SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CompoundCubeBlock owner,
        out SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CompoundCubeBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CompoundCubeBlock owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CompoundCubeBlock owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_CompoundCubeBlock\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CompoundCubeBlock>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CompoundCubeBlock();

      MyObjectBuilder_CompoundCubeBlock IActivator<MyObjectBuilder_CompoundCubeBlock>.CreateInstance() => new MyObjectBuilder_CompoundCubeBlock();
    }
  }
}
