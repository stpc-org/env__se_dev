// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_SchematicItemDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_SchematicItemDefinition : MyObjectBuilder_UsableItemDefinition
  {
    [ProtoMember(1)]
    public SerializableDefinitionId? Research;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EResearch\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SchematicItemDefinition owner,
        in SerializableDefinitionId? value)
      {
        owner.Research = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SchematicItemDefinition owner,
        out SerializableDefinitionId? value)
      {
        value = owner.Research;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EUseSound\u003C\u003EAccessor : MyObjectBuilder_UsableItemDefinition.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_UsableItemDefinition\u003C\u003EUseSound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value) => this.Set((MyObjectBuilder_UsableItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value) => this.Get((MyObjectBuilder_UsableItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in Vector3 value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out Vector3 value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EMass\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMass\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in float value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out float value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EModel\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EModel\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EModels\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EModels\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string[] value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string[] value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EIconSymbol\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EIconSymbol\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EVolume\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EVolume\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in float? value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out float? value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EModelVolume\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EModelVolume\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in float? value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out float? value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EVoxelMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EVoxelMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003ECanSpawnFromScreen\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ECanSpawnFromScreen\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003ERotateOnSpawnX\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ERotateOnSpawnX\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003ERotateOnSpawnY\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ERotateOnSpawnY\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003ERotateOnSpawnZ\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ERotateOnSpawnZ\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EHealth\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EHealth\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EDestroyedPieceId\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EDestroyedPieceId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SchematicItemDefinition owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SchematicItemDefinition owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EDestroyedPieces\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EDestroyedPieces\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EExtraInventoryTooltipLine\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EExtraInventoryTooltipLine\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EMaxStackAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaxStackAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, MyFixedPoint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SchematicItemDefinition owner,
        in MyFixedPoint value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SchematicItemDefinition owner,
        out MyFixedPoint value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EMinimalPricePerUnit\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimalPricePerUnit\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EMinimumOfferAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimumOfferAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EMaximumOfferAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaximumOfferAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EMinimumOrderAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimumOrderAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EMaximumOrderAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaximumOrderAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003ECanPlayerOrder\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003ECanPlayerOrder\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EMinimumAcquisitionAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMinimumAcquisitionAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EMaximumAcquisitionAmount\u003C\u003EAccessor : MyObjectBuilder_PhysicalItemDefinition.VRage_Game_MyObjectBuilder_PhysicalItemDefinition\u003C\u003EMaximumAcquisitionAmount\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in int value) => this.Set((MyObjectBuilder_PhysicalItemDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out int value) => this.Get((MyObjectBuilder_PhysicalItemDefinition&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SchematicItemDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SchematicItemDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SchematicItemDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SchematicItemDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SchematicItemDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SchematicItemDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SchematicItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SchematicItemDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SchematicItemDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_SchematicItemDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SchematicItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SchematicItemDefinition();

      MyObjectBuilder_SchematicItemDefinition IActivator<MyObjectBuilder_SchematicItemDefinition>.CreateInstance() => new MyObjectBuilder_SchematicItemDefinition();
    }
  }
}
