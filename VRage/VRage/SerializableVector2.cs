// Decompiled with JetBrains decompiler
// Type: VRage.SerializableVector2
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
  [ProtoContract]
  public struct SerializableVector2
  {
    public float X;
    public float Y;

    public bool ShouldSerializeX() => false;

    public bool ShouldSerializeY() => false;

    public SerializableVector2(float x, float y)
    {
      this.X = x;
      this.Y = y;
    }

    [ProtoMember(1)]
    [XmlAttribute]
    [NoSerialize]
    public float x
    {
      get => this.X;
      set => this.X = value;
    }

    [ProtoMember(4)]
    [XmlAttribute]
    [NoSerialize]
    public float y
    {
      get => this.Y;
      set => this.Y = value;
    }

    public static implicit operator Vector2(SerializableVector2 v) => new Vector2(v.X, v.Y);

    public static implicit operator SerializableVector2(Vector2 v) => new SerializableVector2(v.X, v.Y);

    protected class VRage_SerializableVector2\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<SerializableVector2, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector2 owner, in float value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector2 owner, out float value) => value = owner.X;
    }

    protected class VRage_SerializableVector2\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<SerializableVector2, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector2 owner, in float value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector2 owner, out float value) => value = owner.Y;
    }

    protected class VRage_SerializableVector2\u003C\u003Ex\u003C\u003EAccessor : IMemberAccessor<SerializableVector2, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector2 owner, in float value) => owner.x = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector2 owner, out float value) => value = owner.x;
    }

    protected class VRage_SerializableVector2\u003C\u003Ey\u003C\u003EAccessor : IMemberAccessor<SerializableVector2, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector2 owner, in float value) => owner.y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector2 owner, out float value) => value = owner.y;
    }
  }
}
