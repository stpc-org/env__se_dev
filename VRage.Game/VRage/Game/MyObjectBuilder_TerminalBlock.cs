// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_TerminalBlock
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_TerminalBlock : MyObjectBuilder_CubeBlock
  {
    [ProtoMember(1)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public string CustomName;
    [ProtoMember(4)]
    public bool ShowOnHUD;
    [ProtoMember(7)]
    public bool ShowInTerminal = true;
    [ProtoMember(10)]
    public bool ShowInToolbarConfig = true;
    [ProtoMember(13)]
    public bool ShowInInventory = true;

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003ECustomName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TerminalBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in string value) => owner.CustomName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out string value) => value = owner.CustomName;
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowOnHUD\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TerminalBlock, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in bool value) => owner.ShowOnHUD = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out bool value) => value = owner.ShowOnHUD;
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowInTerminal\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TerminalBlock, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in bool value) => owner.ShowInTerminal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out bool value) => value = owner.ShowInTerminal;
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowInToolbarConfig\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TerminalBlock, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in bool value) => owner.ShowInToolbarConfig = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out bool value) => value = owner.ShowInToolbarConfig;
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowInInventory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TerminalBlock, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in bool value) => owner.ShowInInventory = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out bool value) => value = owner.ShowInInventory;
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in string value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out string value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EMin\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMin\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TerminalBlock owner,
        in SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TerminalBlock owner,
        out SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003Em_orientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003Em_orientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TerminalBlock owner,
        in SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TerminalBlock owner,
        out SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EIntegrityPercent\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EIntegrityPercent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EBuildPercent\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBuildPercent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EBlockOrientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBlockOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, SerializableBlockOrientation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TerminalBlock owner,
        in SerializableBlockOrientation value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TerminalBlock owner,
        out SerializableBlockOrientation value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EConstructionInventory\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EConstructionInventory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyObjectBuilder_Inventory>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TerminalBlock owner,
        in MyObjectBuilder_Inventory value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TerminalBlock owner,
        out MyObjectBuilder_Inventory value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EColorMaskHSV\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EColorMaskHSV\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in SerializableVector3 value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TerminalBlock owner,
        out SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003ESkinSubtypeId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESkinSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in string value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out string value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EConstructionStockpile\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EConstructionStockpile\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyObjectBuilder_ConstructionStockpile>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TerminalBlock owner,
        in MyObjectBuilder_ConstructionStockpile value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TerminalBlock owner,
        out MyObjectBuilder_ConstructionStockpile value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EOwner\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EOwner\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EBuiltBy\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBuiltBy\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShareMode\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EShareMode\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyOwnershipShareModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TerminalBlock owner,
        in MyOwnershipShareModeEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TerminalBlock owner,
        out MyOwnershipShareModeEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EDeformationRatio\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EDeformationRatio\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003ESubBlocks\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESubBlocks\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyObjectBuilder_CubeBlock.MySubBlockId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TerminalBlock owner,
        in MyObjectBuilder_CubeBlock.MySubBlockId[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TerminalBlock owner,
        out MyObjectBuilder_CubeBlock.MySubBlockId[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EMultiBlockId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in int value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out int value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EMultiBlockDefinition\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TerminalBlock owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TerminalBlock owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EMultiBlockIndex\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockIndex\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in int value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out int value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EBlockGeneralDamageModifier\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBlockGeneralDamageModifier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TerminalBlock owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TerminalBlock owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EOrientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TerminalBlock owner,
        in SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TerminalBlock owner,
        out SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TerminalBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TerminalBlock owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TerminalBlock owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_TerminalBlock>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_TerminalBlock();

      MyObjectBuilder_TerminalBlock IActivator<MyObjectBuilder_TerminalBlock>.CreateInstance() => new MyObjectBuilder_TerminalBlock();
    }
  }
}
