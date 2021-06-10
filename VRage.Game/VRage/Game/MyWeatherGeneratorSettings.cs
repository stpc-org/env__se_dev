// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyWeatherGeneratorSettings
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
  public class MyWeatherGeneratorSettings
  {
    [ProtoMember(5)]
    public string Voxel;
    [ProtoMember(10)]
    [XmlArrayItem("Weather")]
    public List<MyWeatherGeneratorVoxelSettings> Weathers;

    protected class VRage_Game_MyWeatherGeneratorSettings\u003C\u003EVoxel\u003C\u003EAccessor : IMemberAccessor<MyWeatherGeneratorSettings, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyWeatherGeneratorSettings owner, in string value) => owner.Voxel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyWeatherGeneratorSettings owner, out string value) => value = owner.Voxel;
    }

    protected class VRage_Game_MyWeatherGeneratorSettings\u003C\u003EWeathers\u003C\u003EAccessor : IMemberAccessor<MyWeatherGeneratorSettings, List<MyWeatherGeneratorVoxelSettings>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyWeatherGeneratorSettings owner,
        in List<MyWeatherGeneratorVoxelSettings> value)
      {
        owner.Weathers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyWeatherGeneratorSettings owner,
        out List<MyWeatherGeneratorVoxelSettings> value)
      {
        value = owner.Weathers;
      }
    }

    private class VRage_Game_MyWeatherGeneratorSettings\u003C\u003EActor : IActivator, IActivator<MyWeatherGeneratorSettings>
    {
      object IActivator.CreateInstance() => (object) new MyWeatherGeneratorSettings();

      MyWeatherGeneratorSettings IActivator<MyWeatherGeneratorSettings>.CreateInstance() => new MyWeatherGeneratorSettings();
    }
  }
}
