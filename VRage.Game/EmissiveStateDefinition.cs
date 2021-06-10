// Decompiled with JetBrains decompiler
// Type: EmissiveStateDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

[ProtoContract]
public struct EmissiveStateDefinition
{
  [ProtoMember(4)]
  [XmlAttribute("StateName")]
  public string StateName;
  [ProtoMember(7)]
  [XmlAttribute("EmissiveColorName")]
  public string EmissiveColorName;
  [ProtoMember(10)]
  [XmlAttribute("DisplayColorName")]
  public string DisplayColorName;
  [ProtoMember(13)]
  [XmlAttribute("Emissivity")]
  public float Emissivity;

  protected class _EmissiveStateDefinition\u003C\u003EStateName\u003C\u003EAccessor : IMemberAccessor<EmissiveStateDefinition, string>
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void Set(ref EmissiveStateDefinition owner, in string value) => owner.StateName = value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void Get(ref EmissiveStateDefinition owner, out string value) => value = owner.StateName;
  }

  protected class _EmissiveStateDefinition\u003C\u003EEmissiveColorName\u003C\u003EAccessor : IMemberAccessor<EmissiveStateDefinition, string>
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void Set(ref EmissiveStateDefinition owner, in string value) => owner.EmissiveColorName = value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void Get(ref EmissiveStateDefinition owner, out string value) => value = owner.EmissiveColorName;
  }

  protected class _EmissiveStateDefinition\u003C\u003EDisplayColorName\u003C\u003EAccessor : IMemberAccessor<EmissiveStateDefinition, string>
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void Set(ref EmissiveStateDefinition owner, in string value) => owner.DisplayColorName = value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void Get(ref EmissiveStateDefinition owner, out string value) => value = owner.DisplayColorName;
  }

  protected class _EmissiveStateDefinition\u003C\u003EEmissivity\u003C\u003EAccessor : IMemberAccessor<EmissiveStateDefinition, float>
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void Set(ref EmissiveStateDefinition owner, in float value) => owner.Emissivity = value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual void Get(ref EmissiveStateDefinition owner, out float value) => value = owner.Emissivity;
  }

  private class _EmissiveStateDefinition\u003C\u003EActor : IActivator, IActivator<EmissiveStateDefinition>
  {
    object IActivator.CreateInstance() => (object) new EmissiveStateDefinition();

    EmissiveStateDefinition IActivator<EmissiveStateDefinition>.CreateInstance() => new EmissiveStateDefinition();
  }
}
