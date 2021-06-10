// Decompiled with JetBrains decompiler
// Type: VRage.SerializableQuaternion
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
  public struct SerializableQuaternion
  {
    public float X;
    public float Y;
    public float Z;
    public float W;

    public bool ShouldSerializeX() => false;

    public bool ShouldSerializeY() => false;

    public bool ShouldSerializeZ() => false;

    public bool ShouldSerializeW() => false;

    public SerializableQuaternion(float x, float y, float z, float w)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
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

    [ProtoMember(7)]
    [XmlAttribute]
    [NoSerialize]
    public float z
    {
      get => this.Z;
      set => this.Z = value;
    }

    [ProtoMember(10)]
    [XmlAttribute]
    [NoSerialize]
    public float w
    {
      get => this.W;
      set => this.W = value;
    }

    public static implicit operator Quaternion(SerializableQuaternion q) => new Quaternion(q.X, q.Y, q.Z, q.W);

    public static implicit operator SerializableQuaternion(Quaternion q) => new SerializableQuaternion(q.X, q.Y, q.Z, q.W);

    protected class VRage_SerializableQuaternion\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<SerializableQuaternion, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableQuaternion owner, in float value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableQuaternion owner, out float value) => value = owner.X;
    }

    protected class VRage_SerializableQuaternion\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<SerializableQuaternion, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableQuaternion owner, in float value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableQuaternion owner, out float value) => value = owner.Y;
    }

    protected class VRage_SerializableQuaternion\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<SerializableQuaternion, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableQuaternion owner, in float value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableQuaternion owner, out float value) => value = owner.Z;
    }

    protected class VRage_SerializableQuaternion\u003C\u003EW\u003C\u003EAccessor : IMemberAccessor<SerializableQuaternion, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableQuaternion owner, in float value) => owner.W = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableQuaternion owner, out float value) => value = owner.W;
    }

    protected class VRage_SerializableQuaternion\u003C\u003Ex\u003C\u003EAccessor : IMemberAccessor<SerializableQuaternion, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableQuaternion owner, in float value) => owner.x = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableQuaternion owner, out float value) => value = owner.x;
    }

    protected class VRage_SerializableQuaternion\u003C\u003Ey\u003C\u003EAccessor : IMemberAccessor<SerializableQuaternion, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableQuaternion owner, in float value) => owner.y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableQuaternion owner, out float value) => value = owner.y;
    }

    protected class VRage_SerializableQuaternion\u003C\u003Ez\u003C\u003EAccessor : IMemberAccessor<SerializableQuaternion, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableQuaternion owner, in float value) => owner.z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableQuaternion owner, out float value) => value = owner.z;
    }

    protected class VRage_SerializableQuaternion\u003C\u003Ew\u003C\u003EAccessor : IMemberAccessor<SerializableQuaternion, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableQuaternion owner, in float value) => owner.w = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableQuaternion owner, out float value) => value = owner.w;
    }
  }
}
