// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetMaps
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public struct MyPlanetMaps
  {
    [ProtoMember(43)]
    [XmlAttribute]
    public bool Material;
    [ProtoMember(44)]
    [XmlAttribute]
    public bool Ores;
    [ProtoMember(45)]
    [XmlAttribute]
    public bool Biome;
    [ProtoMember(46)]
    [XmlAttribute]
    public bool Occlusion;

    public MyPlanetMapTypeSet ToSet()
    {
      MyPlanetMapTypeSet planetMapTypeSet = (MyPlanetMapTypeSet) 0;
      if (this.Material)
        planetMapTypeSet |= MyPlanetMapTypeSet.Material;
      if (this.Ores)
        planetMapTypeSet |= MyPlanetMapTypeSet.Ore;
      if (this.Biome)
        planetMapTypeSet |= MyPlanetMapTypeSet.Biome;
      return planetMapTypeSet;
    }

    protected class VRage_Game_MyPlanetMaps\u003C\u003EMaterial\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaps, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaps owner, in bool value) => owner.Material = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaps owner, out bool value) => value = owner.Material;
    }

    protected class VRage_Game_MyPlanetMaps\u003C\u003EOres\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaps, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaps owner, in bool value) => owner.Ores = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaps owner, out bool value) => value = owner.Ores;
    }

    protected class VRage_Game_MyPlanetMaps\u003C\u003EBiome\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaps, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaps owner, in bool value) => owner.Biome = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaps owner, out bool value) => value = owner.Biome;
    }

    protected class VRage_Game_MyPlanetMaps\u003C\u003EOcclusion\u003C\u003EAccessor : IMemberAccessor<MyPlanetMaps, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetMaps owner, in bool value) => owner.Occlusion = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetMaps owner, out bool value) => value = owner.Occlusion;
    }

    private class VRage_Game_MyPlanetMaps\u003C\u003EActor : IActivator, IActivator<MyPlanetMaps>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetMaps();

      MyPlanetMaps IActivator<MyPlanetMaps>.CreateInstance() => new MyPlanetMaps();
    }
  }
}
