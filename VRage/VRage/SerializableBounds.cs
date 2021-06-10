// Decompiled with JetBrains decompiler
// Type: VRage.SerializableBounds
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRageMath;

namespace VRage
{
  [ProtoContract]
  public struct SerializableBounds
  {
    [ProtoMember(1)]
    [XmlAttribute]
    public float Min;
    [ProtoMember(4)]
    [XmlAttribute]
    public float Max;
    [ProtoMember(7)]
    [XmlAttribute]
    public float Default;

    public SerializableBounds(float min, float max, float def)
    {
      this.Min = min;
      this.Max = max;
      this.Default = def;
    }

    public static implicit operator MyBounds(SerializableBounds v) => new MyBounds(v.Min, v.Max, v.Default);

    public static implicit operator SerializableBounds(MyBounds v) => new SerializableBounds(v.Min, v.Max, v.Default);

    protected class VRage_SerializableBounds\u003C\u003EMin\u003C\u003EAccessor : IMemberAccessor<SerializableBounds, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableBounds owner, in float value) => owner.Min = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableBounds owner, out float value) => value = owner.Min;
    }

    protected class VRage_SerializableBounds\u003C\u003EMax\u003C\u003EAccessor : IMemberAccessor<SerializableBounds, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableBounds owner, in float value) => owner.Max = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableBounds owner, out float value) => value = owner.Max;
    }

    protected class VRage_SerializableBounds\u003C\u003EDefault\u003C\u003EAccessor : IMemberAccessor<SerializableBounds, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableBounds owner, in float value) => owner.Default = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableBounds owner, out float value) => value = owner.Default;
    }
  }
}
