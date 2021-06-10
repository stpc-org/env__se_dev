// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_DebugSphere2
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
  public class MyObjectBuilder_DebugSphere2 : MyObjectBuilder_FunctionalBlock
  {
    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_FunctionalBlock.VRage_Game_MyObjectBuilder_FunctionalBlock\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in bool value) => this.Set((MyObjectBuilder_FunctionalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out bool value) => this.Get((MyObjectBuilder_FunctionalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003ECustomName\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003ECustomName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in string value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out string value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EShowOnHUD\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowOnHUD\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in bool value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out bool value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EShowInTerminal\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowInTerminal\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in bool value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out bool value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EShowInToolbarConfig\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowInToolbarConfig\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in bool value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out bool value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EShowInInventory\u003C\u003EAccessor : MyObjectBuilder_TerminalBlock.VRage_Game_MyObjectBuilder_TerminalBlock\u003C\u003EShowInInventory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in bool value) => this.Set((MyObjectBuilder_TerminalBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out bool value) => this.Get((MyObjectBuilder_TerminalBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in string value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out string value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EMin\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMin\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in SerializableVector3I value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DebugSphere2 owner,
        out SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003Em_orientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003Em_orientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DebugSphere2 owner,
        in SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DebugSphere2 owner,
        out SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EIntegrityPercent\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EIntegrityPercent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EBuildPercent\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBuildPercent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EBlockOrientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBlockOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, SerializableBlockOrientation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DebugSphere2 owner,
        in SerializableBlockOrientation value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DebugSphere2 owner,
        out SerializableBlockOrientation value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EConstructionInventory\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EConstructionInventory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyObjectBuilder_Inventory>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DebugSphere2 owner,
        in MyObjectBuilder_Inventory value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DebugSphere2 owner,
        out MyObjectBuilder_Inventory value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EColorMaskHSV\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EColorMaskHSV\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in SerializableVector3 value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out SerializableVector3 value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003ESkinSubtypeId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESkinSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in string value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out string value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EConstructionStockpile\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EConstructionStockpile\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyObjectBuilder_ConstructionStockpile>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DebugSphere2 owner,
        in MyObjectBuilder_ConstructionStockpile value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DebugSphere2 owner,
        out MyObjectBuilder_ConstructionStockpile value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EOwner\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EOwner\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EBuiltBy\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBuiltBy\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in long value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out long value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EShareMode\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EShareMode\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyOwnershipShareModeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DebugSphere2 owner,
        in MyOwnershipShareModeEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DebugSphere2 owner,
        out MyOwnershipShareModeEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EDeformationRatio\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EDeformationRatio\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003ESubBlocks\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003ESubBlocks\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyObjectBuilder_CubeBlock.MySubBlockId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DebugSphere2 owner,
        in MyObjectBuilder_CubeBlock.MySubBlockId[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DebugSphere2 owner,
        out MyObjectBuilder_CubeBlock.MySubBlockId[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EMultiBlockId\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in int value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out int value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EMultiBlockDefinition\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DebugSphere2 owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DebugSphere2 owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EMultiBlockIndex\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EMultiBlockIndex\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in int value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out int value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EBlockGeneralDamageModifier\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EBlockGeneralDamageModifier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in float value) => this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out float value) => this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DebugSphere2 owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DebugSphere2 owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EOrientation\u003C\u003EAccessor : MyObjectBuilder_CubeBlock.VRage_Game_MyObjectBuilder_CubeBlock\u003C\u003EOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, SerializableQuaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_DebugSphere2 owner,
        in SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlock&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_DebugSphere2 owner,
        out SerializableQuaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlock&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_DebugSphere2, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_DebugSphere2 owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_DebugSphere2 owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_DebugSphere2\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_DebugSphere2>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_DebugSphere2();

      MyObjectBuilder_DebugSphere2 IActivator<MyObjectBuilder_DebugSphere2>.CreateInstance() => new MyObjectBuilder_DebugSphere2();
    }
  }
}
