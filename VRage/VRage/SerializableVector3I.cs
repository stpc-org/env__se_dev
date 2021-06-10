// Decompiled with JetBrains decompiler
// Type: VRage.SerializableVector3I
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
  public struct SerializableVector3I
  {
    public int X;
    public int Y;
    public int Z;

    public bool ShouldSerializeX() => false;

    public bool ShouldSerializeY() => false;

    public bool ShouldSerializeZ() => false;

    public SerializableVector3I(int x, int y, int z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    [ProtoMember(1)]
    [XmlAttribute]
    [NoSerialize]
    public int x
    {
      get => this.X;
      set => this.X = value;
    }

    [ProtoMember(4)]
    [XmlAttribute]
    [NoSerialize]
    public int y
    {
      get => this.Y;
      set => this.Y = value;
    }

    [ProtoMember(7)]
    [XmlAttribute]
    [NoSerialize]
    public int z
    {
      get => this.Z;
      set => this.Z = value;
    }

    public static implicit operator Vector3I(SerializableVector3I v) => new Vector3I(v.X, v.Y, v.Z);

    public static implicit operator SerializableVector3I(Vector3I v) => new SerializableVector3I(v.X, v.Y, v.Z);

    public static bool operator ==(SerializableVector3I a, SerializableVector3I b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;

    public static bool operator !=(SerializableVector3I a, SerializableVector3I b) => a.X != b.X || a.Y != b.Y || a.Z != b.Z;

    public override bool Equals(object obj) => obj is SerializableVector3I serializableVector3I && serializableVector3I == this;

    public override int GetHashCode() => this.X.GetHashCode() * 1610612741 ^ this.Y.GetHashCode() * 24593 ^ this.Z.GetHashCode();

    protected class VRage_SerializableVector3I\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<SerializableVector3I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3I owner, in int value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3I owner, out int value) => value = owner.X;
    }

    protected class VRage_SerializableVector3I\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<SerializableVector3I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3I owner, in int value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3I owner, out int value) => value = owner.Y;
    }

    protected class VRage_SerializableVector3I\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<SerializableVector3I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3I owner, in int value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3I owner, out int value) => value = owner.Z;
    }

    protected class VRage_SerializableVector3I\u003C\u003Ex\u003C\u003EAccessor : IMemberAccessor<SerializableVector3I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3I owner, in int value) => owner.x = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3I owner, out int value) => value = owner.x;
    }

    protected class VRage_SerializableVector3I\u003C\u003Ey\u003C\u003EAccessor : IMemberAccessor<SerializableVector3I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3I owner, in int value) => owner.y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3I owner, out int value) => value = owner.y;
    }

    protected class VRage_SerializableVector3I\u003C\u003Ez\u003C\u003EAccessor : IMemberAccessor<SerializableVector3I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3I owner, in int value) => owner.z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3I owner, out int value) => value = owner.z;
    }
  }
}
