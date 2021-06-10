// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetAtmosphere
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
  public class MyPlanetAtmosphere
  {
    [ProtoMember(74)]
    [XmlElement]
    public bool Breathable;
    [ProtoMember(75)]
    [XmlElement]
    public float OxygenDensity = 1f;
    [ProtoMember(76)]
    [XmlElement]
    public float Density = 1f;
    [ProtoMember(77)]
    [XmlElement]
    public float LimitAltitude = 2f;
    [XmlElement]
    [ProtoMember(78)]
    public float MaxWindSpeed;

    protected class VRage_Game_MyPlanetAtmosphere\u003C\u003EBreathable\u003C\u003EAccessor : IMemberAccessor<MyPlanetAtmosphere, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAtmosphere owner, in bool value) => owner.Breathable = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAtmosphere owner, out bool value) => value = owner.Breathable;
    }

    protected class VRage_Game_MyPlanetAtmosphere\u003C\u003EOxygenDensity\u003C\u003EAccessor : IMemberAccessor<MyPlanetAtmosphere, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAtmosphere owner, in float value) => owner.OxygenDensity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAtmosphere owner, out float value) => value = owner.OxygenDensity;
    }

    protected class VRage_Game_MyPlanetAtmosphere\u003C\u003EDensity\u003C\u003EAccessor : IMemberAccessor<MyPlanetAtmosphere, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAtmosphere owner, in float value) => owner.Density = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAtmosphere owner, out float value) => value = owner.Density;
    }

    protected class VRage_Game_MyPlanetAtmosphere\u003C\u003ELimitAltitude\u003C\u003EAccessor : IMemberAccessor<MyPlanetAtmosphere, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAtmosphere owner, in float value) => owner.LimitAltitude = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAtmosphere owner, out float value) => value = owner.LimitAltitude;
    }

    protected class VRage_Game_MyPlanetAtmosphere\u003C\u003EMaxWindSpeed\u003C\u003EAccessor : IMemberAccessor<MyPlanetAtmosphere, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetAtmosphere owner, in float value) => owner.MaxWindSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetAtmosphere owner, out float value) => value = owner.MaxWindSpeed;
    }

    private class VRage_Game_MyPlanetAtmosphere\u003C\u003EActor : IActivator, IActivator<MyPlanetAtmosphere>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetAtmosphere();

      MyPlanetAtmosphere IActivator<MyPlanetAtmosphere>.CreateInstance() => new MyPlanetAtmosphere();
    }
  }
}
