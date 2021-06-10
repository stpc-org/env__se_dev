// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector4I
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath.PackedVector;

namespace VRageMath
{
  [ProtoContract]
  public struct Vector4I : IComparable<Vector4I>
  {
    [ProtoMember(1)]
    public int X;
    [ProtoMember(4)]
    public int Y;
    [ProtoMember(7)]
    public int Z;
    [ProtoMember(10)]
    public int W;
    public static readonly Vector4I.EqualityComparer Comparer = new Vector4I.EqualityComparer();

    public Vector4I(int x, int y, int z, int w)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
    }

    public Vector4I(Vector3I xyz, int w)
    {
      this.X = xyz.X;
      this.Y = xyz.Y;
      this.Z = xyz.Z;
      this.W = w;
    }

    public static explicit operator Byte4(Vector4I xyzw) => new Byte4((float) xyzw.X, (float) xyzw.Y, (float) xyzw.Z, (float) xyzw.W);

    public int CompareTo(Vector4I other)
    {
      int num1 = this.X - other.X;
      int num2 = this.Y - other.Y;
      int num3 = this.Z - other.Z;
      int num4 = this.W - other.W;
      if (num1 != 0)
        return num1;
      if (num2 != 0)
        return num2;
      return num3 == 0 ? num4 : num3;
    }

    public override string ToString() => this.X.ToString() + ", " + (object) this.Y + ", " + (object) this.Z + ", " + (object) this.W;

    public int this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.X;
          case 1:
            return this.Y;
          case 2:
            return this.Z;
          case 3:
            return this.W;
          default:
            throw new Exception("Index out of bounds");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.X = value;
            break;
          case 1:
            this.Y = value;
            break;
          case 2:
            this.Z = value;
            break;
          case 3:
            this.W = value;
            break;
          default:
            throw new Exception("Index out of bounds");
        }
      }
    }

    public class EqualityComparer : IEqualityComparer<Vector4I>, IComparer<Vector4I>
    {
      public bool Equals(Vector4I x, Vector4I y) => x.X == y.X && x.Y == y.Y && x.Z == y.Z && x.W == y.W;

      public int GetHashCode(Vector4I obj) => ((obj.X * 397 ^ obj.Y) * 397 ^ obj.Z) * 397 ^ obj.W;

      public int Compare(Vector4I x, Vector4I y) => x.CompareTo(y);
    }

    protected class VRageMath_Vector4I\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Vector4I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector4I owner, in int value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector4I owner, out int value) => value = owner.X;
    }

    protected class VRageMath_Vector4I\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Vector4I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector4I owner, in int value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector4I owner, out int value) => value = owner.Y;
    }

    protected class VRageMath_Vector4I\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<Vector4I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector4I owner, in int value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector4I owner, out int value) => value = owner.Z;
    }

    protected class VRageMath_Vector4I\u003C\u003EW\u003C\u003EAccessor : IMemberAccessor<Vector4I, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector4I owner, in int value) => owner.W = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector4I owner, out int value) => value = owner.W;
    }
  }
}
