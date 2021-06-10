// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetSurfaceRule
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  public class MyPlanetSurfaceRule : ICloneable
  {
    [ProtoMember(11)]
    public SerializableRange Height = new SerializableRange(0.0f, 1f);
    [ProtoMember(12)]
    public SymmetricSerializableRange Latitude = new SymmetricSerializableRange(-90f, 90f);
    [ProtoMember(13)]
    public SerializableRange Longitude = new SerializableRange(-180f, 180f);
    [ProtoMember(14)]
    public SerializableRange Slope = new SerializableRange(0.0f, 90f);

    public bool Check(float height, float latitude, float longitude, float slope) => this.Height.ValueBetween(height) && this.Latitude.ValueBetween(latitude) && this.Longitude.ValueBetween(longitude) && this.Slope.ValueBetween(slope);

    public object Clone() => (object) new MyPlanetSurfaceRule()
    {
      Height = this.Height,
      Latitude = this.Latitude,
      Longitude = this.Longitude,
      Slope = this.Slope
    };

    protected class VRage_Game_MyPlanetSurfaceRule\u003C\u003EHeight\u003C\u003EAccessor : IMemberAccessor<MyPlanetSurfaceRule, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetSurfaceRule owner, in SerializableRange value) => owner.Height = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetSurfaceRule owner, out SerializableRange value) => value = owner.Height;
    }

    protected class VRage_Game_MyPlanetSurfaceRule\u003C\u003ELatitude\u003C\u003EAccessor : IMemberAccessor<MyPlanetSurfaceRule, SymmetricSerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetSurfaceRule owner, in SymmetricSerializableRange value) => owner.Latitude = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetSurfaceRule owner, out SymmetricSerializableRange value) => value = owner.Latitude;
    }

    protected class VRage_Game_MyPlanetSurfaceRule\u003C\u003ELongitude\u003C\u003EAccessor : IMemberAccessor<MyPlanetSurfaceRule, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetSurfaceRule owner, in SerializableRange value) => owner.Longitude = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetSurfaceRule owner, out SerializableRange value) => value = owner.Longitude;
    }

    protected class VRage_Game_MyPlanetSurfaceRule\u003C\u003ESlope\u003C\u003EAccessor : IMemberAccessor<MyPlanetSurfaceRule, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetSurfaceRule owner, in SerializableRange value) => owner.Slope = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetSurfaceRule owner, out SerializableRange value) => value = owner.Slope;
    }

    private class VRage_Game_MyPlanetSurfaceRule\u003C\u003EActor : IActivator, IActivator<MyPlanetSurfaceRule>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetSurfaceRule();

      MyPlanetSurfaceRule IActivator<MyPlanetSurfaceRule>.CreateInstance() => new MyPlanetSurfaceRule();
    }
  }
}
