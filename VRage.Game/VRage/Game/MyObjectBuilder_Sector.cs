// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Sector
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
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
  public class MyObjectBuilder_Sector : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public Vector3I Position;
    [ProtoMember(7)]
    public MyObjectBuilder_GlobalEvents SectorEvents;
    [ProtoMember(10)]
    public int AppVersion;
    [ProtoMember(13)]
    [Obsolete]
    public MyObjectBuilder_Encounters Encounters;
    [ProtoMember(16)]
    public MyObjectBuilder_EnvironmentSettings Environment;
    [ProtoMember(19)]
    public ulong VoxelHandVolumeChanged;

    [ProtoMember(4)]
    [DynamicObjectBuilder(false)]
    [XmlArrayItem("MyObjectBuilder_EntityBase", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_EntityBase>))]
    public List<MyObjectBuilder_EntityBase> SectorObjects { get; set; }

    public bool ShouldSerializeEnvironment() => this.Environment != null;

    protected class VRage_Game_MyObjectBuilder_Sector\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Sector owner, in Vector3I value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Sector owner, out Vector3I value) => value = owner.Position;
    }

    protected class VRage_Game_MyObjectBuilder_Sector\u003C\u003ESectorEvents\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, MyObjectBuilder_GlobalEvents>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Sector owner,
        in MyObjectBuilder_GlobalEvents value)
      {
        owner.SectorEvents = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Sector owner,
        out MyObjectBuilder_GlobalEvents value)
      {
        value = owner.SectorEvents;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Sector\u003C\u003EAppVersion\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Sector owner, in int value) => owner.AppVersion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Sector owner, out int value) => value = owner.AppVersion;
    }

    protected class VRage_Game_MyObjectBuilder_Sector\u003C\u003EEncounters\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, MyObjectBuilder_Encounters>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Sector owner, in MyObjectBuilder_Encounters value) => owner.Encounters = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Sector owner,
        out MyObjectBuilder_Encounters value)
      {
        value = owner.Encounters;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Sector\u003C\u003EEnvironment\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, MyObjectBuilder_EnvironmentSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Sector owner,
        in MyObjectBuilder_EnvironmentSettings value)
      {
        owner.Environment = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Sector owner,
        out MyObjectBuilder_EnvironmentSettings value)
      {
        value = owner.Environment;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Sector\u003C\u003EVoxelHandVolumeChanged\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Sector owner, in ulong value) => owner.VoxelHandVolumeChanged = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Sector owner, out ulong value) => value = owner.VoxelHandVolumeChanged;
    }

    protected class VRage_Game_MyObjectBuilder_Sector\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Sector, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Sector owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Sector owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Sector\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Sector, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Sector owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Sector owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Sector\u003C\u003ESectorObjects\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Sector, List<MyObjectBuilder_EntityBase>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Sector owner,
        in List<MyObjectBuilder_EntityBase> value)
      {
        owner.SectorObjects = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Sector owner,
        out List<MyObjectBuilder_EntityBase> value)
      {
        value = owner.SectorObjects;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Sector\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Sector, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Sector owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Sector owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Sector\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Sector, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Sector owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Sector owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Sector\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Sector>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Sector();

      MyObjectBuilder_Sector IActivator<MyObjectBuilder_Sector>.CreateInstance() => new MyObjectBuilder_Sector();
    }
  }
}
