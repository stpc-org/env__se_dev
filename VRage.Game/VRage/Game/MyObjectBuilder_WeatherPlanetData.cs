// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WeatherPlanetData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_WeatherPlanetData
  {
    [ProtoMember(5)]
    public long PlanetId;
    [ProtoMember(10)]
    public int NextWeather;
    [ProtoMember(30)]
    public List<MyObjectBuilder_WeatherEffect> Weathers = new List<MyObjectBuilder_WeatherEffect>();

    protected class VRage_Game_MyObjectBuilder_WeatherPlanetData\u003C\u003EPlanetId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherPlanetData, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherPlanetData owner, in long value) => owner.PlanetId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherPlanetData owner, out long value) => value = owner.PlanetId;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherPlanetData\u003C\u003ENextWeather\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherPlanetData, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherPlanetData owner, in int value) => owner.NextWeather = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherPlanetData owner, out int value) => value = owner.NextWeather;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherPlanetData\u003C\u003EWeathers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherPlanetData, List<MyObjectBuilder_WeatherEffect>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WeatherPlanetData owner,
        in List<MyObjectBuilder_WeatherEffect> value)
      {
        owner.Weathers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WeatherPlanetData owner,
        out List<MyObjectBuilder_WeatherEffect> value)
      {
        value = owner.Weathers;
      }
    }

    private class VRage_Game_MyObjectBuilder_WeatherPlanetData\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WeatherPlanetData>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WeatherPlanetData();

      MyObjectBuilder_WeatherPlanetData IActivator<MyObjectBuilder_WeatherPlanetData>.CreateInstance() => new MyObjectBuilder_WeatherPlanetData();
    }
  }
}
