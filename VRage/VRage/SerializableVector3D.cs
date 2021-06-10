// Decompiled with JetBrains decompiler
// Type: VRage.SerializableVector3D
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
  public struct SerializableVector3D
  {
    public double X;
    public double Y;
    public double Z;

    public bool ShouldSerializeX() => false;

    public bool ShouldSerializeY() => false;

    public bool ShouldSerializeZ() => false;

    public SerializableVector3D(double x, double y, double z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public SerializableVector3D(Vector3D v)
    {
      this.X = v.X;
      this.Y = v.Y;
      this.Z = v.Z;
    }

    [ProtoMember(1)]
    [XmlAttribute]
    [NoSerialize]
    public double x
    {
      get => this.X;
      set => this.X = value;
    }

    [ProtoMember(4)]
    [XmlAttribute]
    [NoSerialize]
    public double y
    {
      get => this.Y;
      set => this.Y = value;
    }

    [ProtoMember(7)]
    [XmlAttribute]
    [NoSerialize]
    public double z
    {
      get => this.Z;
      set => this.Z = value;
    }

    public bool IsZero => this.X == 0.0 && this.Y == 0.0 && this.Z == 0.0;

    public static implicit operator Vector3D(SerializableVector3D v) => new Vector3D(v.X, v.Y, v.Z);

    public static implicit operator SerializableVector3D(Vector3D v) => new SerializableVector3D(v.X, v.Y, v.Z);

    public override string ToString() => "X: " + (object) this.X + " Y: " + (object) this.Y + " Z: " + (object) this.Z;

    protected class VRage_SerializableVector3D\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<SerializableVector3D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3D owner, in double value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3D owner, out double value) => value = owner.X;
    }

    protected class VRage_SerializableVector3D\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<SerializableVector3D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3D owner, in double value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3D owner, out double value) => value = owner.Y;
    }

    protected class VRage_SerializableVector3D\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<SerializableVector3D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3D owner, in double value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3D owner, out double value) => value = owner.Z;
    }

    protected class VRage_SerializableVector3D\u003C\u003Ex\u003C\u003EAccessor : IMemberAccessor<SerializableVector3D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3D owner, in double value) => owner.x = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3D owner, out double value) => value = owner.x;
    }

    protected class VRage_SerializableVector3D\u003C\u003Ey\u003C\u003EAccessor : IMemberAccessor<SerializableVector3D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3D owner, in double value) => owner.y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3D owner, out double value) => value = owner.y;
    }

    protected class VRage_SerializableVector3D\u003C\u003Ez\u003C\u003EAccessor : IMemberAccessor<SerializableVector3D, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableVector3D owner, in double value) => owner.z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableVector3D owner, out double value) => value = owner.z;
    }
  }
}
