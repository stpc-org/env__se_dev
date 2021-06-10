// Decompiled with JetBrains decompiler
// Type: VRage.SerializableVector3
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
  public struct SerializableVector3
  {
    public float X;
    public float Y;
    public float Z;

    public bool ShouldSerializeX() => false;

    public bool ShouldSerializeY() => false;

    public bool ShouldSerializeZ() => false;

    public SerializableVector3(float x, float y, float z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
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

    public bool IsZero => (double) this.X == 0.0 && (double) this.Y == 0.0 && (double) this.Z == 0.0;

    public static implicit operator Vector3(SerializableVector3 v) => new Vector3(v.X, v.Y, v.Z);

    public static implicit operator SerializableVector3(Vector3 v) => new SerializableVector3(v.X, v.Y, v.Z);

    public static bool operator ==(SerializableVector3 a, SerializableVector3 b) => (double) a.X == (double) b.X && (double) a.Y == (double) b.Y && (double) a.Z == (double) b.Z;

    public static bool operator !=(SerializableVector3 a, SerializableVector3 b) => (double) a.X != (double) b.X || (double) a.Y != (double) b.Y || (double) a.Z != (double) b.Z;

    public override bool Equals(object obj) => obj is SerializableVector3 serializableVector3 && serializableVector3 == this;

    public override int GetHashCode() => this.X.GetHashCode() * 1610612741 ^ this.Y.GetHashCode() * 24593 ^ this.Z.GetHashCode();

    protected class VRage_SerializableVector3\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<SerializableVector3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3 owner, in float value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3 owner, out float value) => value = owner.X;
    }

    protected class VRage_SerializableVector3\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<SerializableVector3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3 owner, in float value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3 owner, out float value) => value = owner.Y;
    }

    protected class VRage_SerializableVector3\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<SerializableVector3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3 owner, in float value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3 owner, out float value) => value = owner.Z;
    }

    protected class VRage_SerializableVector3\u003C\u003Ex\u003C\u003EAccessor : IMemberAccessor<SerializableVector3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3 owner, in float value) => owner.x = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3 owner, out float value) => value = owner.x;
    }

    protected class VRage_SerializableVector3\u003C\u003Ey\u003C\u003EAccessor : IMemberAccessor<SerializableVector3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3 owner, in float value) => owner.y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3 owner, out float value) => value = owner.y;
    }

    protected class VRage_SerializableVector3\u003C\u003Ez\u003C\u003EAccessor : IMemberAccessor<SerializableVector3, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3 owner, in float value) => owner.z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3 owner, out float value) => value = owner.z;
    }
  }
}
