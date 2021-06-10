// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_VoxelMap
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_VoxelMap : MyObjectBuilder_EntityBase
  {
    private string m_storageName;
    public bool MutableStorage = true;
    [Serialize(MyObjectFlags.DefaultZero)]
    public bool? ContentChanged;
    [ProtoMember(4)]
    [Nullable]
    public string Filename;
    [ProtoMember(5)]
    [Nullable]
    public long? BoulderPlanetId;
    [ProtoMember(6)]
    [Nullable]
    public long? BoulderSectorId;
    [ProtoMember(7)]
    [Nullable]
    public int? BoulderItemId;
    [ProtoMember(8)]
    public bool CreatedByUser;

    [ProtoMember(1)]
    [Nullable]
    public string StorageName
    {
      get => this.m_storageName ?? this.Name ?? this.Filename;
      set => this.m_storageName = value;
    }

    public bool ShouldSerializeMutableStorage() => !this.MutableStorage;

    public MyObjectBuilder_VoxelMap()
    {
      this.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation((Vector3D) Vector3.Zero, Vector3.Forward, Vector3.Up));
      this.ContentChanged = new bool?(false);
    }

    public MyObjectBuilder_VoxelMap(Vector3 position, string storageName)
    {
      this.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation((Vector3D) position, Vector3.Forward, Vector3.Up));
      this.ContentChanged = new bool?(false);
      this.StorageName = storageName;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003Em_storageName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMap, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in string value) => owner.m_storageName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out string value) => value = owner.m_storageName;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EMutableStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMap, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in bool value) => owner.MutableStorage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out bool value) => value = owner.MutableStorage;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EContentChanged\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMap, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in bool? value) => owner.ContentChanged = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out bool? value) => value = owner.ContentChanged;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EFilename\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMap, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in string value) => owner.Filename = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out string value) => value = owner.Filename;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EBoulderPlanetId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMap, long?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in long? value) => owner.BoulderPlanetId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out long? value) => value = owner.BoulderPlanetId;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EBoulderSectorId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMap, long?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in long? value) => owner.BoulderSectorId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out long? value) => value = owner.BoulderSectorId;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EBoulderItemId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMap, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in int? value) => owner.BoulderItemId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out int? value) => value = owner.BoulderItemId;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003ECreatedByUser\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMap, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in bool value) => owner.CreatedByUser = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out bool value) => value = owner.CreatedByUser;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMap, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in long value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out long value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EPersistentFlags\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPersistentFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMap, MyPersistentEntityFlags2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in MyPersistentEntityFlags2 value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelMap owner,
        out MyPersistentEntityFlags2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMap, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in string value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out string value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMap, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelMap owner,
        in MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelMap owner,
        out MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMap, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelMap owner,
        in MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelMap owner,
        out MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMap, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelMap owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelMap owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EEntityDefinitionId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityDefinitionId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMap, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VoxelMap owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VoxelMap owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMap, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMap, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EStorageName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VoxelMap, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in string value) => owner.StorageName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out string value) => value = owner.StorageName;
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMap, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VoxelMap, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VoxelMap owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VoxelMap owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_VoxelMap\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_VoxelMap>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_VoxelMap();

      MyObjectBuilder_VoxelMap IActivator<MyObjectBuilder_VoxelMap>.CreateInstance() => new MyObjectBuilder_VoxelMap();
    }
  }
}
