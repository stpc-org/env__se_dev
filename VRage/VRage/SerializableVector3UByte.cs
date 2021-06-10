// Decompiled with JetBrains decompiler
// Type: VRage.SerializableVector3UByte
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
  public struct SerializableVector3UByte
  {
    public byte X;
    public byte Y;
    public byte Z;

    public bool ShouldSerializeX() => false;

    public bool ShouldSerializeY() => false;

    public bool ShouldSerializeZ() => false;

    public SerializableVector3UByte(byte x, byte y, byte z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    [ProtoMember(1)]
    [XmlAttribute]
    [NoSerialize]
    public byte x
    {
      get => this.X;
      set => this.X = value;
    }

    [ProtoMember(4)]
    [XmlAttribute]
    [NoSerialize]
    public byte y
    {
      get => this.Y;
      set => this.Y = value;
    }

    [ProtoMember(7)]
    [XmlAttribute]
    [NoSerialize]
    public byte z
    {
      get => this.Z;
      set => this.Z = value;
    }

    public static implicit operator Vector3UByte(SerializableVector3UByte v) => new Vector3UByte(v.X, v.Y, v.Z);

    public static implicit operator SerializableVector3UByte(Vector3UByte v) => new SerializableVector3UByte(v.X, v.Y, v.Z);

    protected class VRage_SerializableVector3UByte\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<SerializableVector3UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3UByte owner, in byte value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3UByte owner, out byte value) => value = owner.X;
    }

    protected class VRage_SerializableVector3UByte\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<SerializableVector3UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3UByte owner, in byte value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3UByte owner, out byte value) => value = owner.Y;
    }

    protected class VRage_SerializableVector3UByte\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<SerializableVector3UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3UByte owner, in byte value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3UByte owner, out byte value) => value = owner.Z;
    }

    protected class VRage_SerializableVector3UByte\u003C\u003Ex\u003C\u003EAccessor : IMemberAccessor<SerializableVector3UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3UByte owner, in byte value) => owner.x = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3UByte owner, out byte value) => value = owner.x;
    }

    protected class VRage_SerializableVector3UByte\u003C\u003Ey\u003C\u003EAccessor : IMemberAccessor<SerializableVector3UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3UByte owner, in byte value) => owner.y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3UByte owner, out byte value) => value = owner.y;
    }

    protected class VRage_SerializableVector3UByte\u003C\u003Ez\u003C\u003EAccessor : IMemberAccessor<SerializableVector3UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3UByte owner, in byte value) => owner.z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3UByte owner, out byte value) => value = owner.z;
    }
  }
}
