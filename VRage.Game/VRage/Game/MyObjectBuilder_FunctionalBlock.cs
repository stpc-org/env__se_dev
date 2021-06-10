// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_FunctionalBlock
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_FunctionalBlock : MyObjectBuilder_TerminalBlock
  {
    [ProtoMember(1)]
    public bool Enabled = true;

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionalBlock, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in bool value) => owner.Enabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out bool value) => value = owner.Enabled;
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003ECustomName\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003ECustomName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in string value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out string value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EShowOnHUD\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowOnHUD\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in bool value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out bool value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EShowInTerminal\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowInTerminal\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in bool value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out bool value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EShowInToolbarConfig\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowInToolbarConfig\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in bool value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out bool value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EShowInInventory\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowInInventory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in bool value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out bool value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in string value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out string value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EMin\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMin\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionalBlock owner,
        in SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionalBlock owner,
        out SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003Em_orientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003Em_orientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionalBlock owner,
        in SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionalBlock owner,
        out SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EIntegrityPercent\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EIntegrityPercent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EBuildPercent\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBuildPercent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EBlockOrientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBlockOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, SerializableBlockOrientation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionalBlock owner,
        in SerializableBlockOrientation value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionalBlock owner,
        out SerializableBlockOrientation value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EConstructionInventory\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EConstructionInventory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, MyObjectBuilder_Inventory>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionalBlock owner,
        in MyObjectBuilder_Inventory value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionalBlock owner,
        out MyObjectBuilder_Inventory value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EColorMaskHSV\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EColorMaskHSV\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionalBlock owner,
        in SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionalBlock owner,
        out SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003ESkinSubtypeId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESkinSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in string value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out string value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EConstructionStockpile\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EConstructionStockpile\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, MyObjectBuilder_ConstructionStockpile>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionalBlock owner,
        in MyObjectBuilder_ConstructionStockpile value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionalBlock owner,
        out MyObjectBuilder_ConstructionStockpile value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EOwner\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EOwner\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EBuiltBy\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBuiltBy\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EShareMode\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EShareMode\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, MyOwnershipShareModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionalBlock owner,
        in MyOwnershipShareModeEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionalBlock owner,
        out MyOwnershipShareModeEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EDeformationRatio\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EDeformationRatio\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003ESubBlocks\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESubBlocks\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, MyObjectBuilder_CubeBlock.MySubBlockId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionalBlock owner,
        in MyObjectBuilder_CubeBlock.MySubBlockId[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionalBlock owner,
        out MyObjectBuilder_CubeBlock.MySubBlockId[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EMultiBlockId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in int value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out int value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EMultiBlockDefinition\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionalBlock owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionalBlock owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EMultiBlockIndex\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockIndex\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in int value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out int value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EBlockGeneralDamageModifier\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBlockGeneralDamageModifier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionalBlock owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionalBlock owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EOrientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionalBlock owner,
        in SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionalBlock owner,
        out SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionalBlock, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionalBlock owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionalBlock owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FunctionalBlock>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FunctionalBlock();

      MyObjectBuilder_FunctionalBlock IActivator<MyObjectBuilder_FunctionalBlock>.CreateInstance() => new MyObjectBuilder_FunctionalBlock();
    }
  }
}
