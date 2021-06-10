// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_FueledPowerProducerDefinition
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
  public class MyObjectBuilder_FueledPowerProducerDefinition : MyObjectBuilder_PowerProducerDefinition
  {
    [ProtoMember(1)]
    public float FuelProductionToCapacityMultiplier = 3600f;
    [ProtoMember(4)]
    public List<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo> FuelInfos;
    [ProtoMember(7)]
    public string ResourceSinkGroup;

    [ProtoContract]
    public class FuelInfo
    {
      [ProtoMember(10)]
      public float Ratio = 1f;
      [ProtoMember(13)]
      public SerializableDefinitionId Id;

      protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EFuelInfo\u003C\u003ERatio\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo owner,
          in float value)
        {
          owner.Ratio = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo owner,
          out float value)
        {
          value = owner.Ratio;
        }
      }

      protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EFuelInfo\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo owner,
          in SerializableDefinitionId value)
        {
          owner.Id = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo owner,
          out SerializableDefinitionId value)
        {
          value = owner.Id;
        }
      }

      private class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EFuelInfo\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo();

        MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo IActivator<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo>.CreateInstance() => new MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo();
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EFuelProductionToCapacityMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in float value)
      {
        owner.FuelProductionToCapacityMultiplier = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out float value)
      {
        value = owner.FuelProductionToCapacityMultiplier;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EFuelInfos\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, List<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in List<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo> value)
      {
        owner.FuelInfos = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out List<MyObjectBuilder_FueledPowerProducerDefinition.FuelInfo> value)
      {
        value = owner.FuelInfos;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EResourceSinkGroup\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        owner.ResourceSinkGroup = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        value = owner.ResourceSinkGroup;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EResourceSourceGroup\u003C\u003EAccessor : MyObjectBuilder_PowerProducerDefinition.VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EResourceSourceGroup\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PowerProducerDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PowerProducerDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EMaxPowerOutput\u003C\u003EAccessor : MyObjectBuilder_PowerProducerDefinition.VRage_Game_MyObjectBuilder_PowerProducerDefinition\u003C\u003EMaxPowerOutput\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PowerProducerDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PowerProducerDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EVoxelPlacement\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EVoxelPlacement\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, VoxelPlacementOverride?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in VoxelPlacementOverride? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out VoxelPlacementOverride? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ESilenceableByShipSoundSystem\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESilenceableByShipSoundSystem\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ECubeSize\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeSize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyCubeSize>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyCubeSize value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyCubeSize value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EBlockTopology\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBlockTopology\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyBlockTopology>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyBlockTopology value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyBlockTopology value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableVector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out SerializableVector3I value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EModelOffset\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EModelOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out SerializableVector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ECubeDefinition\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECubeDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.PatternDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.PatternDefinition value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.PatternDefinition value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EComponents\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EComponents\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EEffects\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEffects\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.CubeBlockEffectBase[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ECriticalComponent\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECriticalComponent\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.CriticalPart>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.CriticalPart value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.CriticalPart value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EMountPoints\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMountPoints\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.MountPoint[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.MountPoint[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.MountPoint[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EVariants\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EVariants\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.Variant[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.Variant[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.Variant[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EEntityComponents\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEntityComponents\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.EntityComponentDefinition[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EPhysicsOption\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPhysicsOption\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyPhysicsOption>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyPhysicsOption value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyPhysicsOption value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EBuildProgressModels\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressModels\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out List<MyObjectBuilder_CubeBlockDefinition.BuildProgressModel> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EBlockPairName\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBlockPairName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ECenter\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECenter\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableVector3I?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in SerializableVector3I? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out SerializableVector3I? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EMirroringX\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringX\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MySymmetryAxisEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EMirroringY\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringY\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MySymmetryAxisEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EMirroringZ\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringZ\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MySymmetryAxisEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MySymmetryAxisEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDeformationRatio\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDeformationRatio\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EEdgeType\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEdgeType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EBuildTimeSeconds\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildTimeSeconds\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDisassembleRatio\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDisassembleRatio\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EAutorotateMode\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EAutorotateMode\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyAutorotateMode>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyAutorotateMode value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyAutorotateMode value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EMirroringBlock\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirroringBlock\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EUseModelIntersection\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EUseModelIntersection\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EPrimarySound\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPrimarySound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EActionSound\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EActionSound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EBuildType\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EBuildMaterial\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ECompoundTemplates\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECompoundTemplates\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ECompoundEnabled\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECompoundEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ESubBlockDefinitions\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESubBlockDefinitions\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyObjectBuilder_CubeBlockDefinition.MySubBlockDefinition[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EMultiBlock\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMultiBlock\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ENavigationDefinition\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ENavigationDefinition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EGuiVisible\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGuiVisible\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EBlockVariants\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBlockVariants\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableDefinitionId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in SerializableDefinitionId[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out SerializableDefinitionId[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDirection\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDirection\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyBlockDirection>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyBlockDirection value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyBlockDirection value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ERotation\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ERotation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyBlockRotation>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyBlockRotation value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyBlockRotation value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EGeneratedBlocks\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGeneratedBlocks\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableDefinitionId[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in SerializableDefinitionId[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out SerializableDefinitionId[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EGeneratedBlockType\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGeneratedBlockType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EMirrored\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMirrored\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDamageEffectId\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDamageEffectId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDestroyEffect\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDestroyEffect\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDestroySound\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDestroySound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ESkeleton\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ESkeleton\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, List<BoneInfo>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in List<BoneInfo> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out List<BoneInfo> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ERandomRotation\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ERandomRotation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EIsAirTight\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EIsAirTight\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EIsStandAlone\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EIsStandAlone\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EHasPhysics\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EHasPhysics\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EUseNeighbourOxygenRooms\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EUseNeighbourOxygenRooms\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EPoints\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPoints\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EMaxIntegrity\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EMaxIntegrity\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EBuildProgressToPlaceGeneratedBlocks\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EBuildProgressToPlaceGeneratedBlocks\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDamagedSound\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDamagedSound\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ECreateFracturedPieces\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ECreateFracturedPieces\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EEmissiveColorPreset\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EEmissiveColorPreset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EGeneralDamageMultiplier\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EGeneralDamageMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDamageEffectName\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDamageEffectName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EUsesDeformation\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EUsesDeformation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDestroyEffectOffset\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDestroyEffectOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, Vector3?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in Vector3? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out Vector3? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EPCU\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPCU\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EPCUConsole\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPCUConsole\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in int? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out int? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EPlaceDecals\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EPlaceDecals\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDepressurizationEffectOffset\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003EDepressurizationEffectOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableVector3?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in SerializableVector3? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out SerializableVector3? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ETieredUpdateTimes\u003C\u003EAccessor : MyObjectBuilder_CubeBlockDefinition.VRage_Game_MyObjectBuilder_CubeBlockDefinition\u003C\u003ETieredUpdateTimes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MySerializableList<uint>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MySerializableList<uint> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_CubeBlockDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MySerializableList<uint> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_CubeBlockDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EModel\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EModel\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EMass\u003C\u003EAccessor : MyObjectBuilder_PhysicalModelDefinition.VRage_Game_MyObjectBuilder_PhysicalModelDefinition\u003C\u003EMass\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_PhysicalModelDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_PhysicalModelDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FueledPowerProducerDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FueledPowerProducerDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_FueledPowerProducerDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FueledPowerProducerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FueledPowerProducerDefinition();

      MyObjectBuilder_FueledPowerProducerDefinition IActivator<MyObjectBuilder_FueledPowerProducerDefinition>.CreateInstance() => new MyObjectBuilder_FueledPowerProducerDefinition();
    }
  }
}
