// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_PowerProducerDefinition
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
  public class MyObjectBuilder_PowerProducerDefinition : MyObjectBuilder_CubeBlockDefinition
  {
    [ProtoMember(1)]
    public string ResourceSourceGroup;
    [ProtoMember(4)]
    public float MaxPowerOutput;

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EResourceSourceGroup\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => owner.ResourceSourceGroup = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => value = owner.ResourceSourceGroup;
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EMaxPowerOutput\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in float value) => owner.MaxPowerOutput = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out float value) => value = owner.MaxPowerOutput;
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EVoxelPlacement\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EVoxelPlacement\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, VoxelPlacementOverride?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in VoxelPlacementOverride? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out VoxelPlacementOverride? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ESilenceableByShipSoundSystem\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESilenceableByShipSoundSystem\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ECubeSize\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeSize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyCubeSize>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyCubeSize value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyCubeSize value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EBlockTopology\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBlockTopology\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyBlockTopology>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyBlockTopology value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyBlockTopology value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EModelOffset\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EModelOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ECubeDefinition\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.PatternDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.PatternDefinition value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.PatternDefinition value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EComponents\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EComponents\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EEffects\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEffects\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ECriticalComponent\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECriticalComponent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.CriticalPart>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.CriticalPart value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.CriticalPart value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EMountPoints\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoints\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.MountPoint[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.MountPoint[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.MountPoint[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EVariants\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EVariants\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.Variant[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.Variant[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.Variant[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EEntityComponents\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEntityComponents\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EPhysicsOption\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPhysicsOption\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyPhysicsOption>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyPhysicsOption value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyPhysicsOption value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EBuildProgressModels\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressModels\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EBlockPairName\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBlockPairName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ECenter\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECenter\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, SerializableVector3I?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in SerializableVector3I? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out SerializableVector3I? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EMirroringX\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringX\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MySymmetryAxisEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EMirroringY\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringY\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MySymmetryAxisEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EMirroringZ\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringZ\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MySymmetryAxisEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDeformationRatio\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDeformationRatio\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in float value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out float value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EEdgeType\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEdgeType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EBuildTimeSeconds\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildTimeSeconds\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in float value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out float value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDisassembleRatio\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDisassembleRatio\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in float value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out float value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EAutorotateMode\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EAutorotateMode\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyAutorotateMode>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyAutorotateMode value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyAutorotateMode value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EMirroringBlock\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringBlock\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EUseModelIntersection\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EUseModelIntersection\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EPrimarySound\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPrimarySound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EActionSound\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EActionSound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EBuildType\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EBuildMaterial\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ECompoundTemplates\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECompoundTemplates\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string[] value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string[] value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ECompoundEnabled\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECompoundEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ESubBlockDefinitions\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESubBlockDefinitions\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EMultiBlock\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMultiBlock\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ENavigationDefinition\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ENavigationDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EGuiVisible\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGuiVisible\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EBlockVariants\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBlockVariants\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, SerializableDefinitionId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in SerializableDefinitionId[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out SerializableDefinitionId[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDirection\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDirection\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyBlockDirection>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyBlockDirection value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyBlockDirection value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ERotation\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ERotation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyBlockRotation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyBlockRotation value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyBlockRotation value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EGeneratedBlocks\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGeneratedBlocks\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, SerializableDefinitionId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in SerializableDefinitionId[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out SerializableDefinitionId[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EGeneratedBlockType\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGeneratedBlockType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EMirrored\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirrored\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDamageEffectId\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDamageEffectId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in int value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out int value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDestroyEffect\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDestroyEffect\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDestroySound\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDestroySound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ESkeleton\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESkeleton\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, List<BoneInfo>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in List<BoneInfo> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out List<BoneInfo> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ERandomRotation\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ERandomRotation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EIsAirTight\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EIsAirTight\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool? value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool? value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EIsStandAlone\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EIsStandAlone\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EHasPhysics\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EHasPhysics\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EUseNeighbourOxygenRooms\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EUseNeighbourOxygenRooms\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EPoints\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPoints\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in int value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out int value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EMaxIntegrity\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMaxIntegrity\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in int value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out int value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EBuildProgressToPlaceGeneratedBlocks\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressToPlaceGeneratedBlocks\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in float value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out float value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDamagedSound\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDamagedSound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ECreateFracturedPieces\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECreateFracturedPieces\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EEmissiveColorPreset\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEmissiveColorPreset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EGeneralDamageMultiplier\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGeneralDamageMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in float value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out float value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDamageEffectName\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDamageEffectName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EUsesDeformation\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EUsesDeformation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDestroyEffectOffset\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDestroyEffectOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, Vector3?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in Vector3? value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out Vector3? value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EPCU\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPCU\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in int value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out int value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EPCUConsole\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPCUConsole\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in int? value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out int? value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EPlaceDecals\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPlaceDecals\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDepressurizationEffectOffset\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDepressurizationEffectOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, SerializableVector3?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in SerializableVector3? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out SerializableVector3? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ETieredUpdateTimes\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ETieredUpdateTimes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MySerializableList<uint>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MySerializableList<uint> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MySerializableList<uint> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EModel\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EModel\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EMass\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EMass\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in float value) => this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out float value) => this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_PowerProducerDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PowerProducerDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PowerProducerDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PowerProducerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_PowerProducerDefinition();

      MyObjectBuilder_PowerProducerDefinition IActivator<MyObjectBuilder_PowerProducerDefinition>.CreateInstance() => new MyObjectBuilder_PowerProducerDefinition();
    }
  }
}
