// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetDistortionDefinition
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
  public class MyPlanetDistortionDefinition
  {
    [ProtoMember(38)]
    [XmlAttribute(AttributeName = "Type")]
    public string Type;
    [ProtoMember(39)]
    [XmlAttribute(AttributeName = "Value")]
    public byte Value;
    [ProtoMember(40)]
    [XmlAttribute(AttributeName = "Frequency")]
    public float Frequency = 1f;
    [ProtoMember(41)]
    [XmlAttribute(AttributeName = "Height")]
    public float Height = 1f;
    [ProtoMember(42)]
    [XmlAttribute(AttributeName = "LayerCount")]
    public int LayerCount = 1;

    protected class VRage_Game_MyPlanetDistortionDefinition\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyPlanetDistortionDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetDistortionDefinition owner, in string value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetDistortionDefinition owner, out string value) => value = owner.Type;
    }

    protected class VRage_Game_MyPlanetDistortionDefinition\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyPlanetDistortionDefinition, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetDistortionDefinition owner, in byte value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetDistortionDefinition owner, out byte value) => value = owner.Value;
    }

    protected class VRage_Game_MyPlanetDistortionDefinition\u003C\u003EFrequency\u003C\u003EAccessor : IMemberAccessor<MyPlanetDistortionDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetDistortionDefinition owner, in float value) => owner.Frequency = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetDistortionDefinition owner, out float value) => value = owner.Frequency;
    }

    protected class VRage_Game_MyPlanetDistortionDefinition\u003C\u003EHeight\u003C\u003EAccessor : IMemberAccessor<MyPlanetDistortionDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetDistortionDefinition owner, in float value) => owner.Height = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetDistortionDefinition owner, out float value) => value = owner.Height;
    }

    protected class VRage_Game_MyPlanetDistortionDefinition\u003C\u003ELayerCount\u003C\u003EAccessor : IMemberAccessor<MyPlanetDistortionDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetDistortionDefinition owner, in int value) => owner.LayerCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetDistortionDefinition owner, out int value) => value = owner.LayerCount;
    }

    private class VRage_Game_MyPlanetDistortionDefinition\u003C\u003EActor : IActivator, IActivator<MyPlanetDistortionDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetDistortionDefinition();

      MyPlanetDistortionDefinition IActivator<MyPlanetDistortionDefinition>.CreateInstance() => new MyPlanetDistortionDefinition();
    }
  }
}
