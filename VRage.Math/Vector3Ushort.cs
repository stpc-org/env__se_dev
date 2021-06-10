// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector3Ushort
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [ProtoContract]
  public struct Vector3Ushort
  {
    [ProtoMember(1)]
    public ushort X;
    [ProtoMember(4)]
    public ushort Y;
    [ProtoMember(7)]
    public ushort Z;

    public Vector3Ushort(ushort x, ushort y, ushort z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public override string ToString() => this.X.ToString() + ", " + (object) this.Y + ", " + (object) this.Z;

    public static Vector3Ushort operator *(Vector3Ushort v, ushort t) => new Vector3Ushort((ushort) ((uint) t * (uint) v.X), (ushort) ((uint) t * (uint) v.Y), (ushort) ((uint) t * (uint) v.Z));

    public static Vector3 operator *(Vector3 vector, Vector3Ushort ushortVector) => ushortVector * vector;

    public static Vector3 operator *(Vector3Ushort ushortVector, Vector3 vector) => new Vector3((float) ushortVector.X * vector.X, (float) ushortVector.Y * vector.Y, (float) ushortVector.Z * vector.Z);

    public static explicit operator Vector3(Vector3Ushort v) => new Vector3((float) v.X, (float) v.Y, (float) v.Z);

    protected class VRageMath_Vector3Ushort\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Vector3Ushort, ushort>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3Ushort owner, in ushort value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3Ushort owner, out ushort value) => value = owner.X;
    }

    protected class VRageMath_Vector3Ushort\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Vector3Ushort, ushort>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3Ushort owner, in ushort value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3Ushort owner, out ushort value) => value = owner.Y;
    }

    protected class VRageMath_Vector3Ushort\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<Vector3Ushort, ushort>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3Ushort owner, in ushort value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3Ushort owner, out ushort value) => value = owner.Z;
    }
  }
}
