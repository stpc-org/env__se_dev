// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Planet
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Planet : MyObjectBuilder_VoxelMap
  {
    [ProtoMember(10)]
    public float Radius;
    [ProtoMember(13)]
    public bool HasAtmosphere;
    [ProtoMember(16)]
    public float AtmosphereRadius;
    [ProtoMember(19)]
    public float MinimumSurfaceRadius;
    [ProtoMember(22)]
    public float MaximumHillRadius;
    [ProtoMember(25)]
    public Vector3 AtmosphereWavelengths;
    [ProtoMember(28)]
    [XmlArrayItem("Sector")]
    [Nullable]
    public MyObjectBuilder_Planet.SavedSector[] SavedEnviromentSectors;
    [ProtoMember(31)]
    public float GravityFalloff;
    [ProtoMember(34)]
    public bool MarkAreaEmpty;
    [ProtoMember(37)]
    [Nullable]
    public MyAtmosphereSettings? AtmosphereSettings;
    [ProtoMember(40)]
    public float SurfaceGravity = 1f;
    [ProtoMember(43)]
    public bool SpawnsFlora;
    [ProtoMember(46)]
    public bool ShowGPS;
    [ProtoMember(49)]
    public bool SpherizeWithDistance = true;
    [ProtoMember(52)]
    [Nullable]
    public string PlanetGenerator = "";
    [ProtoMember(55)]
    public int Seed;

    [ProtoContract]
    public struct SavedSector
    {
      [ProtoMember(1)]
      public Vector3S IdPos;
      [ProtoMember(4)]
      public Vector3B IdDir;
      [ProtoMember(7)]
      [XmlElement("Item")]
      [Nullable]
      public HashSet<int> RemovedItems;

      protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003ESavedSector\u003C\u003EIdPos\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet.SavedSector, Vector3S>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Planet.SavedSector owner, in Vector3S value) => owner.IdPos = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Planet.SavedSector owner, out Vector3S value) => value = owner.IdPos;
      }

      protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003ESavedSector\u003C\u003EIdDir\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet.SavedSector, Vector3B>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Planet.SavedSector owner, in Vector3B value) => owner.IdDir = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Planet.SavedSector owner, out Vector3B value) => value = owner.IdDir;
      }

      protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003ESavedSector\u003C\u003ERemovedItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet.SavedSector, HashSet<int>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Planet.SavedSector owner, in HashSet<int> value) => owner.RemovedItems = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Planet.SavedSector owner,
          out HashSet<int> value)
        {
          value = owner.RemovedItems;
        }
      }

      private class VRage_Game_MyObjectBuilder_Planet\u003C\u003ESavedSector\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Planet.SavedSector>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Planet.SavedSector();

        MyObjectBuilder_Planet.SavedSector IActivator<MyObjectBuilder_Planet.SavedSector>.CreateInstance() => new MyObjectBuilder_Planet.SavedSector();
      }
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003ERadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in float value) => owner.Radius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out float value) => value = owner.Radius;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EHasAtmosphere\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in bool value) => owner.HasAtmosphere = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out bool value) => value = owner.HasAtmosphere;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EAtmosphereRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in float value) => owner.AtmosphereRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out float value) => value = owner.AtmosphereRadius;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EMinimumSurfaceRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in float value) => owner.MinimumSurfaceRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out float value) => value = owner.MinimumSurfaceRadius;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EMaximumHillRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in float value) => owner.MaximumHillRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out float value) => value = owner.MaximumHillRadius;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EAtmosphereWavelengths\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in Vector3 value) => owner.AtmosphereWavelengths = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out Vector3 value) => value = owner.AtmosphereWavelengths;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003ESavedEnviromentSectors\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, MyObjectBuilder_Planet.SavedSector[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Planet owner,
        in MyObjectBuilder_Planet.SavedSector[] value)
      {
        owner.SavedEnviromentSectors = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Planet owner,
        out MyObjectBuilder_Planet.SavedSector[] value)
      {
        value = owner.SavedEnviromentSectors;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EGravityFalloff\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in float value) => owner.GravityFalloff = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out float value) => value = owner.GravityFalloff;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EMarkAreaEmpty\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in bool value) => owner.MarkAreaEmpty = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out bool value) => value = owner.MarkAreaEmpty;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EAtmosphereSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, MyAtmosphereSettings?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in MyAtmosphereSettings? value) => owner.AtmosphereSettings = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out MyAtmosphereSettings? value) => value = owner.AtmosphereSettings;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003ESurfaceGravity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in float value) => owner.SurfaceGravity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out float value) => value = owner.SurfaceGravity;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003ESpawnsFlora\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in bool value) => owner.SpawnsFlora = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out bool value) => value = owner.SpawnsFlora;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EShowGPS\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in bool value) => owner.ShowGPS = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out bool value) => value = owner.ShowGPS;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003ESpherizeWithDistance\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in bool value) => owner.SpherizeWithDistance = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out bool value) => value = owner.SpherizeWithDistance;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EPlanetGenerator\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in string value) => owner.PlanetGenerator = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out string value) => value = owner.PlanetGenerator;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003ESeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Planet, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in int value) => owner.Seed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out int value) => value = owner.Seed;
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003Em_storageName\u003C\u003EAccessor : MyObjectBuilder_VoxelMap.VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003Em_storageName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in string value) => this.Set((MyObjectBuilder_VoxelMap&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out string value) => this.Get((MyObjectBuilder_VoxelMap&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EMutableStorage\u003C\u003EAccessor : MyObjectBuilder_VoxelMap.VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EMutableStorage\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in bool value) => this.Set((MyObjectBuilder_VoxelMap&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out bool value) => this.Get((MyObjectBuilder_VoxelMap&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EContentChanged\u003C\u003EAccessor : MyObjectBuilder_VoxelMap.VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EContentChanged\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in bool? value) => this.Set((MyObjectBuilder_VoxelMap&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out bool? value) => this.Get((MyObjectBuilder_VoxelMap&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EFilename\u003C\u003EAccessor : MyObjectBuilder_VoxelMap.VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EFilename\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in string value) => this.Set((MyObjectBuilder_VoxelMap&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out string value) => this.Get((MyObjectBuilder_VoxelMap&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EBoulderPlanetId\u003C\u003EAccessor : MyObjectBuilder_VoxelMap.VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EBoulderPlanetId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, long?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in long? value) => this.Set((MyObjectBuilder_VoxelMap&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out long? value) => this.Get((MyObjectBuilder_VoxelMap&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EBoulderSectorId\u003C\u003EAccessor : MyObjectBuilder_VoxelMap.VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EBoulderSectorId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, long?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in long? value) => this.Set((MyObjectBuilder_VoxelMap&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out long? value) => this.Get((MyObjectBuilder_VoxelMap&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EBoulderItemId\u003C\u003EAccessor : MyObjectBuilder_VoxelMap.VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EBoulderItemId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in int? value) => this.Set((MyObjectBuilder_VoxelMap&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out int? value) => this.Get((MyObjectBuilder_VoxelMap&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003ECreatedByUser\u003C\u003EAccessor : MyObjectBuilder_VoxelMap.VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003ECreatedByUser\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in bool value) => this.Set((MyObjectBuilder_VoxelMap&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out bool value) => this.Get((MyObjectBuilder_VoxelMap&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in long value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out long value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EPersistentFlags\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPersistentFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, MyPersistentEntityFlags2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in MyPersistentEntityFlags2 value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out MyPersistentEntityFlags2 value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in string value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out string value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in MyPositionAndOrientation? value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out MyPositionAndOrientation? value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in MyPositionAndOrientation? value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out MyPositionAndOrientation? value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Planet owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Planet owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EEntityDefinitionId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityDefinitionId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in SerializableDefinitionId? value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out SerializableDefinitionId? value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003EStorageName\u003C\u003EAccessor : MyObjectBuilder_VoxelMap.VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EStorageName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in string value) => this.Set((MyObjectBuilder_VoxelMap&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out string value) => this.Get((MyObjectBuilder_VoxelMap&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Planet\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Planet, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Planet owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Planet owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Planet\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Planet>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Planet();

      MyObjectBuilder_Planet IActivator<MyObjectBuilder_Planet>.CreateInstance() => new MyObjectBuilder_Planet();
    }
  }
}
