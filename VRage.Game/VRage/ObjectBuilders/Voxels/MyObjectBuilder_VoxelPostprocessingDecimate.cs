// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilders.Voxels.MyObjectBuilder_VoxelPostprocessingDecimate
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.Utils;

namespace VRage.ObjectBuilders.Voxels
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_VoxelPostprocessingDecimate : MyObjectBuilder_VoxelPostprocessing
  {
    [ProtoMember(1)]
    [Description("Set of lod range settings pairs.")]
    public List<MyObjectBuilder_VoxelPostprocessingDecimate.Settings> LodSettings;

    [ProtoContract]
    public class Settings
    {
      [XmlAttribute]
      [Description("Minimum lod level these settings apply. Subsequent sets must have strictly ascending lods.")]
      public int FromLod;
      [Description("The minimum angle to be considered a feature edge. Value is In Radians")]
      public float FeatureAngle;
      [Description("Distance threshold for an edge vertex to be discarded.")]
      public float EdgeThreshold;
      [Description("Distance threshold for an internal vertex to be discarded.")]
      public float PlaneThreshold;
      [Description("Weather edge vertices should be considered or not for removal.")]
      public bool IgnoreEdges = true;

      protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003ESettings\u003C\u003EFromLod\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate.Settings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_VoxelPostprocessingDecimate.Settings owner,
          in int value)
        {
          owner.FromLod = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_VoxelPostprocessingDecimate.Settings owner,
          out int value)
        {
          value = owner.FromLod;
        }
      }

      protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003ESettings\u003C\u003EFeatureAngle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate.Settings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_VoxelPostprocessingDecimate.Settings owner,
          in float value)
        {
          owner.FeatureAngle = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_VoxelPostprocessingDecimate.Settings owner,
          out float value)
        {
          value = owner.FeatureAngle;
        }
      }

      protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003ESettings\u003C\u003EEdgeThreshold\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate.Settings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_VoxelPostprocessingDecimate.Settings owner,
          in float value)
        {
          owner.EdgeThreshold = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_VoxelPostprocessingDecimate.Settings owner,
          out float value)
        {
          value = owner.EdgeThreshold;
        }
      }

      protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003ESettings\u003C\u003EPlaneThreshold\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate.Settings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_VoxelPostprocessingDecimate.Settings owner,
          in float value)
        {
          owner.PlaneThreshold = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_VoxelPostprocessingDecimate.Settings owner,
          out float value)
        {
          value = owner.PlaneThreshold;
        }
      }

      protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003ESettings\u003C\u003EIgnoreEdges\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate.Settings, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_VoxelPostprocessingDecimate.Settings owner,
          in bool value)
        {
          owner.IgnoreEdges = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_VoxelPostprocessingDecimate.Settings owner,
          out bool value)
        {
          value = owner.IgnoreEdges;
        }
      }

      private class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003ESettings\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_VoxelPostprocessingDecimate.Settings>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_VoxelPostprocessingDecimate.Settings();

        MyObjectBuilder_VoxelPostprocessingDecimate.Settings IActivator<MyObjectBuilder_VoxelPostprocessingDecimate.Settings>.CreateInstance() => new MyObjectBuilder_VoxelPostprocessingDecimate.Settings();
      }
    }

    protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003ELodSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate, List<MyObjectBuilder_VoxelPostprocessingDecimate.Settings>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelPostprocessingDecimate owner,
        in List<MyObjectBuilder_VoxelPostprocessingDecimate.Settings> value)
      {
        owner.LodSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelPostprocessingDecimate owner,
        out List<MyObjectBuilder_VoxelPostprocessingDecimate.Settings> value)
      {
        value = owner.LodSettings;
      }
    }

    protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003EForPhysics\u003C\u003EAccessor : MyObjectBuilder_VoxelPostprocessing.VRage_ObjectBuilders_Definitions_Components_MyObjectBuilder_VoxelPostprocessing\u003C\u003EForPhysics\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelPostprocessingDecimate owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_VoxelPostprocessing&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelPostprocessingDecimate owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_VoxelPostprocessing&) ref owner, out value);
      }
    }

    protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelPostprocessingDecimate owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelPostprocessingDecimate owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelPostprocessingDecimate owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelPostprocessingDecimate owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelPostprocessingDecimate owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelPostprocessingDecimate owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelPostprocessingDecimate, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelPostprocessingDecimate owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelPostprocessingDecimate owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_ObjectBuilders_Voxels_MyObjectBuilder_VoxelPostprocessingDecimate\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_VoxelPostprocessingDecimate>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_VoxelPostprocessingDecimate();

      MyObjectBuilder_VoxelPostprocessingDecimate IActivator<MyObjectBuilder_VoxelPostprocessingDecimate>.CreateInstance() => new MyObjectBuilder_VoxelPostprocessingDecimate();
    }
  }
}
