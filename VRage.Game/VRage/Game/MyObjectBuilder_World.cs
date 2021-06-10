// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_World
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
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
  public class MyObjectBuilder_World : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public MyObjectBuilder_Checkpoint Checkpoint;
    [ProtoMember(4)]
    public MyObjectBuilder_Sector Sector;
    [ProtoMember(7)]
    public SerializableDictionary<string, byte[]> VoxelMaps;
    public List<BoundingBoxD> Clusters;
    public List<MyObjectBuilder_Planet> Planets;

    protected class VRage_Game_MyObjectBuilder_World\u003C\u003ECheckpoint\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_World, MyObjectBuilder_Checkpoint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_World owner, in MyObjectBuilder_Checkpoint value) => owner.Checkpoint = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_World owner, out MyObjectBuilder_Checkpoint value) => value = owner.Checkpoint;
    }

    protected class VRage_Game_MyObjectBuilder_World\u003C\u003ESector\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_World, MyObjectBuilder_Sector>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_World owner, in MyObjectBuilder_Sector value) => owner.Sector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_World owner, out MyObjectBuilder_Sector value) => value = owner.Sector;
    }

    protected class VRage_Game_MyObjectBuilder_World\u003C\u003EVoxelMaps\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_World, SerializableDictionary<string, byte[]>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_World owner,
        in SerializableDictionary<string, byte[]> value)
      {
        owner.VoxelMaps = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_World owner,
        out SerializableDictionary<string, byte[]> value)
      {
        value = owner.VoxelMaps;
      }
    }

    protected class VRage_Game_MyObjectBuilder_World\u003C\u003EClusters\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_World, List<BoundingBoxD>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_World owner, in List<BoundingBoxD> value) => owner.Clusters = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_World owner, out List<BoundingBoxD> value) => value = owner.Clusters;
    }

    protected class VRage_Game_MyObjectBuilder_World\u003C\u003EPlanets\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_World, List<MyObjectBuilder_Planet>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_World owner,
        in List<MyObjectBuilder_Planet> value)
      {
        owner.Planets = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_World owner,
        out List<MyObjectBuilder_Planet> value)
      {
        value = owner.Planets;
      }
    }

    protected class VRage_Game_MyObjectBuilder_World\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_World, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_World owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_World owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_World\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_World, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_World owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_World owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_World\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_World, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_World owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_World owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_World\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_World, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_World owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_World owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_World\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_World>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_World();

      MyObjectBuilder_World IActivator<MyObjectBuilder_World>.CreateInstance() => new MyObjectBuilder_World();
    }
  }
}
