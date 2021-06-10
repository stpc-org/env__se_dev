// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_SpawnGroupDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_SpawnGroupDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(34)]
    [DefaultValue(1f)]
    public float Frequency = 1f;
    [ProtoMember(37)]
    [XmlArrayItem("Prefab")]
    public MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab[] Prefabs;
    [ProtoMember(40)]
    [XmlArrayItem("Voxel")]
    public MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel[] Voxels;
    [ProtoMember(43)]
    [DefaultValue(false)]
    public bool IsEncounter;
    [ProtoMember(46)]
    [DefaultValue(false)]
    public bool IsPirate;
    [ProtoMember(49)]
    [DefaultValue(false)]
    public bool IsCargoShip;
    [ProtoMember(52)]
    [DefaultValue(false)]
    public bool ReactorsOn;

    [ProtoContract]
    public class SpawnGroupPrefab
    {
      [XmlAttribute]
      [ProtoMember(1)]
      public string SubtypeId;
      [ProtoMember(4)]
      public Vector3 Position;
      [ProtoMember(7)]
      [DefaultValue("")]
      public string BeaconText = "";
      [ProtoMember(10)]
      [DefaultValue(10f)]
      public float Speed = 10f;
      [ProtoMember(13)]
      [DefaultValue(false)]
      public bool PlaceToGridOrigin;
      [ProtoMember(16)]
      public bool ResetOwnership = true;
      [ProtoMember(19)]
      public string Behaviour;
      [ProtoMember(22)]
      public float BehaviourActivationDistance = 1000f;

      protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupPrefab\u003C\u003ESubtypeId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          in string value)
        {
          owner.SubtypeId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          out string value)
        {
          value = owner.SubtypeId;
        }
      }

      protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupPrefab\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          in Vector3 value)
        {
          owner.Position = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          out Vector3 value)
        {
          value = owner.Position;
        }
      }

      protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupPrefab\u003C\u003EBeaconText\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          in string value)
        {
          owner.BeaconText = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          out string value)
        {
          value = owner.BeaconText;
        }
      }

      protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupPrefab\u003C\u003ESpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          in float value)
        {
          owner.Speed = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          out float value)
        {
          value = owner.Speed;
        }
      }

      protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupPrefab\u003C\u003EPlaceToGridOrigin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          in bool value)
        {
          owner.PlaceToGridOrigin = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          out bool value)
        {
          value = owner.PlaceToGridOrigin;
        }
      }

      protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupPrefab\u003C\u003EResetOwnership\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          in bool value)
        {
          owner.ResetOwnership = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          out bool value)
        {
          value = owner.ResetOwnership;
        }
      }

      protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupPrefab\u003C\u003EBehaviour\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          in string value)
        {
          owner.Behaviour = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          out string value)
        {
          value = owner.Behaviour;
        }
      }

      protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupPrefab\u003C\u003EBehaviourActivationDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          in float value)
        {
          owner.BehaviourActivationDistance = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab owner,
          out float value)
        {
          value = owner.BehaviourActivationDistance;
        }
      }

      private class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupPrefab\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab();

        MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab IActivator<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab>.CreateInstance() => new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab();
      }
    }

    [ProtoContract]
    public class SpawnGroupVoxel
    {
      [XmlAttribute]
      [ProtoMember(25)]
      public string StorageName;
      [ProtoMember(28)]
      public Vector3 Offset;
      [ProtoMember(31, IsRequired = false)]
      public bool CenterOffset;

      protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupVoxel\u003C\u003EStorageName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel owner,
          in string value)
        {
          owner.StorageName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel owner,
          out string value)
        {
          value = owner.StorageName;
        }
      }

      protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupVoxel\u003C\u003EOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel, Vector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel owner,
          in Vector3 value)
        {
          owner.Offset = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel owner,
          out Vector3 value)
        {
          value = owner.Offset;
        }
      }

      protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupVoxel\u003C\u003ECenterOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel owner,
          in bool value)
        {
          owner.CenterOffset = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel owner,
          out bool value)
        {
          value = owner.CenterOffset;
        }
      }

      private class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESpawnGroupVoxel\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel();

        MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel IActivator<MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel>.CreateInstance() => new MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel();
      }
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EFrequency\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in float value) => owner.Frequency = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out float value) => value = owner.Frequency;
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EPrefabs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SpawnGroupDefinition owner,
        in MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab[] value)
      {
        owner.Prefabs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SpawnGroupDefinition owner,
        out MyObjectBuilder_SpawnGroupDefinition.SpawnGroupPrefab[] value)
      {
        value = owner.Prefabs;
      }
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EVoxels\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SpawnGroupDefinition owner,
        in MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel[] value)
      {
        owner.Voxels = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SpawnGroupDefinition owner,
        out MyObjectBuilder_SpawnGroupDefinition.SpawnGroupVoxel[] value)
      {
        value = owner.Voxels;
      }
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EIsEncounter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value) => owner.IsEncounter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value) => value = owner.IsEncounter;
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EIsPirate\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value) => owner.IsPirate = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value) => value = owner.IsPirate;
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EIsCargoShip\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value) => owner.IsCargoShip = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value) => value = owner.IsCargoShip;
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EReactorsOn\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value) => owner.ReactorsOn = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value) => value = owner.ReactorsOn;
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SpawnGroupDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SpawnGroupDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SpawnGroupDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SpawnGroupDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SpawnGroupDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SpawnGroupDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SpawnGroupDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_SpawnGroupDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SpawnGroupDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SpawnGroupDefinition();

      MyObjectBuilder_SpawnGroupDefinition IActivator<MyObjectBuilder_SpawnGroupDefinition>.CreateInstance() => new MyObjectBuilder_SpawnGroupDefinition();
    }
  }
}
