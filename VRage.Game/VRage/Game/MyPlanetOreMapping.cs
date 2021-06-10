// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyPlanetOreMapping
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
  public class MyPlanetOreMapping
  {
    [ProtoMember(18)]
    [XmlAttribute(AttributeName = "Value")]
    public byte Value;
    [ProtoMember(19)]
    [XmlAttribute(AttributeName = "Type")]
    public string Type;
    [ProtoMember(20)]
    [XmlAttribute(AttributeName = "Start")]
    public float Start = 5f;
    [ProtoMember(21)]
    [XmlAttribute(AttributeName = "Depth")]
    public float Depth = 10f;
    [ProtoMember(22)]
    [XmlIgnore]
    public ColorDefinitionRGBA? ColorShift;
    private float? m_colorInfluence;

    [ProtoMember(23)]
    [XmlAttribute("TargetColor")]
    public string TargetColor
    {
      get => !this.ColorShift.HasValue ? (string) null : this.ColorShift.Value.Hex;
      set => this.ColorShift = new ColorDefinitionRGBA?(new ColorDefinitionRGBA(value));
    }

    [ProtoMember(24)]
    [XmlAttribute("ColorInfluence")]
    public float ColorInfluence
    {
      get => this.m_colorInfluence ?? 0.0f;
      set => this.m_colorInfluence = new float?(value);
    }

    protected bool Equals(MyPlanetOreMapping other) => (int) this.Value == (int) other.Value;

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((MyPlanetOreMapping) obj);
    }

    public override int GetHashCode() => this.Value.GetHashCode();

    protected class VRage_Game_MyPlanetOreMapping\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyPlanetOreMapping, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetOreMapping owner, in byte value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetOreMapping owner, out byte value) => value = owner.Value;
    }

    protected class VRage_Game_MyPlanetOreMapping\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyPlanetOreMapping, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetOreMapping owner, in string value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetOreMapping owner, out string value) => value = owner.Type;
    }

    protected class VRage_Game_MyPlanetOreMapping\u003C\u003EStart\u003C\u003EAccessor : IMemberAccessor<MyPlanetOreMapping, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetOreMapping owner, in float value) => owner.Start = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetOreMapping owner, out float value) => value = owner.Start;
    }

    protected class VRage_Game_MyPlanetOreMapping\u003C\u003EDepth\u003C\u003EAccessor : IMemberAccessor<MyPlanetOreMapping, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetOreMapping owner, in float value) => owner.Depth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetOreMapping owner, out float value) => value = owner.Depth;
    }

    protected class VRage_Game_MyPlanetOreMapping\u003C\u003EColorShift\u003C\u003EAccessor : IMemberAccessor<MyPlanetOreMapping, ColorDefinitionRGBA?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetOreMapping owner, in ColorDefinitionRGBA? value) => owner.ColorShift = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetOreMapping owner, out ColorDefinitionRGBA? value) => value = owner.ColorShift;
    }

    protected class VRage_Game_MyPlanetOreMapping\u003C\u003Em_colorInfluence\u003C\u003EAccessor : IMemberAccessor<MyPlanetOreMapping, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetOreMapping owner, in float? value) => owner.m_colorInfluence = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetOreMapping owner, out float? value) => value = owner.m_colorInfluence;
    }

    protected class VRage_Game_MyPlanetOreMapping\u003C\u003ETargetColor\u003C\u003EAccessor : IMemberAccessor<MyPlanetOreMapping, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetOreMapping owner, in string value) => owner.TargetColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetOreMapping owner, out string value) => value = owner.TargetColor;
    }

    protected class VRage_Game_MyPlanetOreMapping\u003C\u003EColorInfluence\u003C\u003EAccessor : IMemberAccessor<MyPlanetOreMapping, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyPlanetOreMapping owner, in float value) => owner.ColorInfluence = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyPlanetOreMapping owner, out float value) => value = owner.ColorInfluence;
    }

    private class VRage_Game_MyPlanetOreMapping\u003C\u003EActor : IActivator, IActivator<MyPlanetOreMapping>
    {
      object IActivator.CreateInstance() => (object) new MyPlanetOreMapping();

      MyPlanetOreMapping IActivator<MyPlanetOreMapping>.CreateInstance() => new MyPlanetOreMapping();
    }
  }
}
