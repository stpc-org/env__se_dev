// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WeatherEffect
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_WeatherEffect
  {
    [ProtoMember(5)]
    public Vector3D Position;
    [ProtoMember(7)]
    public Vector3D Velocity;
    [ProtoMember(10)]
    public string Weather;
    [ProtoMember(15)]
    public float Radius;
    [ProtoMember(20)]
    public int Life;
    [ProtoMember(21)]
    public int MaxLife;
    [ProtoMember(25)]
    public float Intensity;
    [ProtoMember(30)]
    public Vector3D StartPoint;
    [ProtoMember(35)]
    public int NextLightning;
    [ProtoMember(40)]
    public int NextLightningCharacter;
    [ProtoMember(45)]
    public int NextLightningGrid;

    public Vector3D EndPoint => this.StartPoint + this.Velocity * ((double) this.MaxLife * 0.0166666675359011);

    protected class VRage_Game_MyObjectBuilder_WeatherEffect\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffect owner, in Vector3D value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffect owner, out Vector3D value) => value = owner.Position;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffect\u003C\u003EVelocity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffect owner, in Vector3D value) => owner.Velocity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffect owner, out Vector3D value) => value = owner.Velocity;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffect\u003C\u003EWeather\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffect owner, in string value) => owner.Weather = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffect owner, out string value) => value = owner.Weather;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffect\u003C\u003ERadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffect owner, in float value) => owner.Radius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffect owner, out float value) => value = owner.Radius;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffect\u003C\u003ELife\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffect owner, in int value) => owner.Life = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffect owner, out int value) => value = owner.Life;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffect\u003C\u003EMaxLife\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffect owner, in int value) => owner.MaxLife = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffect owner, out int value) => value = owner.MaxLife;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffect\u003C\u003EIntensity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffect owner, in float value) => owner.Intensity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffect owner, out float value) => value = owner.Intensity;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffect\u003C\u003EStartPoint\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffect owner, in Vector3D value) => owner.StartPoint = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffect owner, out Vector3D value) => value = owner.StartPoint;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffect\u003C\u003ENextLightning\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffect owner, in int value) => owner.NextLightning = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffect owner, out int value) => value = owner.NextLightning;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffect\u003C\u003ENextLightningCharacter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffect owner, in int value) => owner.NextLightningCharacter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffect owner, out int value) => value = owner.NextLightningCharacter;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherEffect\u003C\u003ENextLightningGrid\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherEffect, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherEffect owner, in int value) => owner.NextLightningGrid = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherEffect owner, out int value) => value = owner.NextLightningGrid;
    }

    private class VRage_Game_MyObjectBuilder_WeatherEffect\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WeatherEffect>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WeatherEffect();

      MyObjectBuilder_WeatherEffect IActivator<MyObjectBuilder_WeatherEffect>.CreateInstance() => new MyObjectBuilder_WeatherEffect();
    }
  }
}
