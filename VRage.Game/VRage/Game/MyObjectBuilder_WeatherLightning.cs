// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WeatherLightning
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_WeatherLightning : MyObjectBuilder_Base
  {
    [ProtoMember(5)]
    public Vector3D Position;
    [ProtoMember(10)]
    public byte Life;
    [ProtoMember(15)]
    public short MaxLife = 7;
    [ProtoMember(20)]
    public float BoltLength = 5000f;
    [ProtoMember(25)]
    public byte BoltParts = 50;
    [ProtoMember(30)]
    public short BoltVariation = 100;
    [ProtoMember(35)]
    public float BoltRadius = 30f;
    [ProtoMember(40)]
    public int Damage;
    [ProtoMember(45)]
    public string Sound = "WM_Lightning";
    [ProtoMember(50)]
    public Vector4 Color = new Vector4(100f, 100f, 100f, 1000f);
    [ProtoMember(55)]
    public float BoltImpulseMultiplier = 1f;
    [ProtoMember(65)]
    public float ExplosionRadius = 1f;

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in Vector3D value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out Vector3D value) => value = owner.Position;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003ELife\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in byte value) => owner.Life = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out byte value) => value = owner.Life;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003EMaxLife\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, short>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in short value) => owner.MaxLife = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out short value) => value = owner.MaxLife;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003EBoltLength\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in float value) => owner.BoltLength = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out float value) => value = owner.BoltLength;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003EBoltParts\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in byte value) => owner.BoltParts = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out byte value) => value = owner.BoltParts;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003EBoltVariation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, short>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in short value) => owner.BoltVariation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out short value) => value = owner.BoltVariation;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003EBoltRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in float value) => owner.BoltRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out float value) => value = owner.BoltRadius;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003EDamage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in int value) => owner.Damage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out int value) => value = owner.Damage;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003ESound\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in string value) => owner.Sound = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out string value) => value = owner.Sound;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003EColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in Vector4 value) => owner.Color = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out Vector4 value) => value = owner.Color;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003EBoltImpulseMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in float value) => owner.BoltImpulseMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out float value) => value = owner.BoltImpulseMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003EExplosionRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WeatherLightning, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in float value) => owner.ExplosionRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out float value) => value = owner.ExplosionRadius;
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherLightning, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherLightning, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherLightning, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WeatherLightning, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_WeatherLightning owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_WeatherLightning owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_WeatherLightning\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WeatherLightning>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WeatherLightning();

      MyObjectBuilder_WeatherLightning IActivator<MyObjectBuilder_WeatherLightning>.CreateInstance() => new MyObjectBuilder_WeatherLightning();
    }
  }
}
