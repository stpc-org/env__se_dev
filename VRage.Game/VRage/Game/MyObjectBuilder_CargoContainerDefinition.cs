// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_CargoContainerDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_CargoContainerDefinition : MyObjectBuilder_CubeBlockDefinition
  {
    [ProtoMember(1)]
    public Vector3 InventorySize;

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EInventorySize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in Vector3 value) => owner.InventorySize = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out Vector3 value) => value = owner.InventorySize;
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EVoxelPlacement\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EVoxelPlacement\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, VoxelPlacementOverride?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in VoxelPlacementOverride? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out VoxelPlacementOverride? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ESilenceableByShipSoundSystem\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESilenceableByShipSoundSystem\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ECubeSize\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeSize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyCubeSize>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyCubeSize value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyCubeSize value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EBlockTopology\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBlockTopology\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyBlockTopology>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyBlockTopology value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyBlockTopology value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EModelOffset\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EModelOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ECubeDefinition\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyObjectBuilder_CubeBlockDefinition.PatternDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.PatternDefinition value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.PatternDefinition value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EComponents\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EComponents\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EEffects\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEffects\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ECriticalComponent\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECriticalComponent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyObjectBuilder_CubeBlockDefinition.CriticalPart>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.CriticalPart value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.CriticalPart value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EMountPoints\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoints\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyObjectBuilder_CubeBlockDefinition.MountPoint[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.MountPoint[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.MountPoint[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EVariants\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EVariants\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyObjectBuilder_CubeBlockDefinition.Variant[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.Variant[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.Variant[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EEntityComponents\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEntityComponents\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EPhysicsOption\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPhysicsOption\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyPhysicsOption>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyPhysicsOption value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyPhysicsOption value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EBuildProgressModels\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressModels\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EBlockPairName\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBlockPairName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ECenter\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECenter\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, SerializableVector3I?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in SerializableVector3I? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out SerializableVector3I? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EMirroringX\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringX\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MySymmetryAxisEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EMirroringY\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringY\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MySymmetryAxisEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EMirroringZ\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringZ\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MySymmetryAxisEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDeformationRatio\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDeformationRatio\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in float value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out float value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EEdgeType\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEdgeType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EBuildTimeSeconds\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildTimeSeconds\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in float value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out float value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDisassembleRatio\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDisassembleRatio\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in float value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out float value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EAutorotateMode\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EAutorotateMode\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyAutorotateMode>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyAutorotateMode value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyAutorotateMode value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EMirroringBlock\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringBlock\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EUseModelIntersection\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EUseModelIntersection\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EPrimarySound\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPrimarySound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EActionSound\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EActionSound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EBuildType\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EBuildMaterial\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ECompoundTemplates\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECompoundTemplates\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string[] value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ECompoundEnabled\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECompoundEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ESubBlockDefinitions\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESubBlockDefinitions\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EMultiBlock\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMultiBlock\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ENavigationDefinition\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ENavigationDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EGuiVisible\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGuiVisible\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EBlockVariants\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBlockVariants\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, SerializableDefinitionId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in SerializableDefinitionId[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out SerializableDefinitionId[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDirection\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDirection\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyBlockDirection>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyBlockDirection value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyBlockDirection value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ERotation\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ERotation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyBlockRotation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyBlockRotation value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyBlockRotation value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EGeneratedBlocks\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGeneratedBlocks\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, SerializableDefinitionId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in SerializableDefinitionId[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out SerializableDefinitionId[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EGeneratedBlockType\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGeneratedBlockType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EMirrored\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirrored\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDamageEffectId\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDamageEffectId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in int value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out int value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDestroyEffect\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDestroyEffect\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDestroySound\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDestroySound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ESkeleton\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESkeleton\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, List<BoneInfo>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in List<BoneInfo> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out List<BoneInfo> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ERandomRotation\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ERandomRotation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EIsAirTight\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EIsAirTight\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool? value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool? value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EIsStandAlone\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EIsStandAlone\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EHasPhysics\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EHasPhysics\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EUseNeighbourOxygenRooms\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EUseNeighbourOxygenRooms\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EPoints\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPoints\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in int value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out int value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EMaxIntegrity\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMaxIntegrity\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in int value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out int value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EBuildProgressToPlaceGeneratedBlocks\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressToPlaceGeneratedBlocks\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in float value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out float value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDamagedSound\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDamagedSound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ECreateFracturedPieces\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECreateFracturedPieces\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EEmissiveColorPreset\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEmissiveColorPreset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EGeneralDamageMultiplier\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGeneralDamageMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in float value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out float value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDamageEffectName\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDamageEffectName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EUsesDeformation\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EUsesDeformation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDestroyEffectOffset\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDestroyEffectOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, Vector3?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in Vector3? value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out Vector3? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EPCU\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPCU\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in int value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out int value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EPCUConsole\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPCUConsole\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in int? value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out int? value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EPlaceDecals\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPlaceDecals\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDepressurizationEffectOffset\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDepressurizationEffectOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, SerializableVector3?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in SerializableVector3? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out SerializableVector3? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ETieredUpdateTimes\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ETieredUpdateTimes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MySerializableList<uint>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MySerializableList<uint> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MySerializableList<uint> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EModel\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EModel\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EMass\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EMass\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in float value) => this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out float value) => this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CargoContainerDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CargoContainerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CargoContainerDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CargoContainerDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_CargoContainerDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CargoContainerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CargoContainerDefinition();

      MyObjectBuilder_CargoContainerDefinition IActivator<MyObjectBuilder_CargoContainerDefinition>.CreateInstance() => new MyObjectBuilder_CargoContainerDefinition();
    }
  }
}
