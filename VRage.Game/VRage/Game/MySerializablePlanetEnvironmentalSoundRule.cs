// Decompiled with JetBrains decompiler
// Type: VRage.Game.MySerializablePlanetEnvironmentalSoundRule
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  public class MySerializablePlanetEnvironmentalSoundRule
  {
    [ProtoMember(47)]
    public SerializableRange Height = new SerializableRange(0.0f, 1f);
    [ProtoMember(48)]
    public SymmetricSerializableRange Latitude = new SymmetricSerializableRange(-90f, 90f);
    [ProtoMember(49)]
    public SerializableRange SunAngleFromZenith = new SerializableRange(0.0f, 180f);
    [ProtoMember(50)]
    public string EnvironmentSound;

    protected class VRage_Game_MySerializablePlanetEnvironmentalSoundRule\u003C\u003EHeight\u003C\u003EAccessor : IMemberAccessor<MySerializablePlanetEnvironmentalSoundRule, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MySerializablePlanetEnvironmentalSoundRule owner,
        in SerializableRange value)
      {
        owner.Height = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MySerializablePlanetEnvironmentalSoundRule owner,
        out SerializableRange value)
      {
        value = owner.Height;
      }
    }

    protected class VRage_Game_MySerializablePlanetEnvironmentalSoundRule\u003C\u003ELatitude\u003C\u003EAccessor : IMemberAccessor<MySerializablePlanetEnvironmentalSoundRule, SymmetricSerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MySerializablePlanetEnvironmentalSoundRule owner,
        in SymmetricSerializableRange value)
      {
        owner.Latitude = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MySerializablePlanetEnvironmentalSoundRule owner,
        out SymmetricSerializableRange value)
      {
        value = owner.Latitude;
      }
    }

    protected class VRage_Game_MySerializablePlanetEnvironmentalSoundRule\u003C\u003ESunAngleFromZenith\u003C\u003EAccessor : IMemberAccessor<MySerializablePlanetEnvironmentalSoundRule, SerializableRange>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MySerializablePlanetEnvironmentalSoundRule owner,
        in SerializableRange value)
      {
        owner.SunAngleFromZenith = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MySerializablePlanetEnvironmentalSoundRule owner,
        out SerializableRange value)
      {
        value = owner.SunAngleFromZenith;
      }
    }

    protected class VRage_Game_MySerializablePlanetEnvironmentalSoundRule\u003C\u003EEnvironmentSound\u003C\u003EAccessor : IMemberAccessor<MySerializablePlanetEnvironmentalSoundRule, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MySerializablePlanetEnvironmentalSoundRule owner,
        in string value)
      {
        owner.EnvironmentSound = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MySerializablePlanetEnvironmentalSoundRule owner,
        out string value)
      {
        value = owner.EnvironmentSound;
      }
    }

    private class VRage_Game_MySerializablePlanetEnvironmentalSoundRule\u003C\u003EActor : IActivator, IActivator<MySerializablePlanetEnvironmentalSoundRule>
    {
      object IActivator.CreateInstance() => (object) new MySerializablePlanetEnvironmentalSoundRule();

      MySerializablePlanetEnvironmentalSoundRule IActivator<MySerializablePlanetEnvironmentalSoundRule>.CreateInstance() => new MySerializablePlanetEnvironmentalSoundRule();
    }
  }
}
