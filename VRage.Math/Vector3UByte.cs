// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector3UByte
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [ProtoContract]
  public struct Vector3UByte
  {
    public static readonly Vector3UByte.EqualityComparer Comparer = new Vector3UByte.EqualityComparer();
    public static Vector3UByte Zero = new Vector3UByte((byte) 0, (byte) 0, (byte) 0);
    [ProtoMember(1)]
    public byte X;
    [ProtoMember(4)]
    public byte Y;
    [ProtoMember(7)]
    public byte Z;
    private static Vector3 m_clampBoundary = new Vector3((float) byte.MaxValue);

    public Vector3UByte(byte x, byte y, byte z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public Vector3UByte(Vector3I vec)
    {
      this.X = (byte) vec.X;
      this.Y = (byte) vec.Y;
      this.Z = (byte) vec.Z;
    }

    public override string ToString() => this.X.ToString() + ", " + (object) this.Y + ", " + (object) this.Z;

    public override int GetHashCode() => (int) this.Z << 16 | (int) this.Y << 8 | (int) this.X;

    public override bool Equals(object obj)
    {
      if (obj != null)
      {
        Vector3UByte? nullable = obj as Vector3UByte?;
        if (nullable.HasValue)
          return this == nullable.Value;
      }
      return false;
    }

    public static bool operator ==(Vector3UByte a, Vector3UByte b) => (int) a.X == (int) b.X && (int) a.Y == (int) b.Y && (int) a.Z == (int) b.Z;

    public static bool operator !=(Vector3UByte a, Vector3UByte b) => (int) a.X != (int) b.X || (int) a.Y != (int) b.Y || (int) a.Z != (int) b.Z;

    public static Vector3UByte Round(Vector3 vec) => new Vector3UByte((byte) Math.Round((double) vec.X), (byte) Math.Round((double) vec.Y), (byte) Math.Round((double) vec.Z));

    public static Vector3UByte Floor(Vector3 vec) => new Vector3UByte((byte) Math.Floor((double) vec.X), (byte) Math.Floor((double) vec.Y), (byte) Math.Floor((double) vec.Z));

    public static implicit operator Vector3I(Vector3UByte vec) => new Vector3I((int) vec.X, (int) vec.Y, (int) vec.Z);

    public int LengthSquared() => (int) this.X * (int) this.X + (int) this.Y * (int) this.Y + (int) this.Z * (int) this.Z;

    public static bool IsMiddle(Vector3UByte vec) => vec.X == (byte) 127 && vec.Y == (byte) 127 && vec.Z == (byte) 127;

    public static Vector3UByte Normalize(Vector3 vec, float range)
    {
      Vector3 result = (vec / range / 2f + new Vector3(0.5f)) * (float) byte.MaxValue;
      Vector3.Clamp(ref result, ref Vector3.Zero, ref Vector3UByte.m_clampBoundary, out result);
      return new Vector3UByte((byte) result.X, (byte) result.Y, (byte) result.Z);
    }

    public static Vector3 Denormalize(Vector3UByte vec, float range)
    {
      float num = 0.001960784f;
      return (new Vector3((float) vec.X, (float) vec.Y, (float) vec.Z) / (float) byte.MaxValue - new Vector3(0.5f - num)) * 2f * range;
    }

    public class EqualityComparer : IEqualityComparer<Vector3UByte>, IComparer<Vector3UByte>
    {
      public bool Equals(Vector3UByte x, Vector3UByte y) => (int) x.X == (int) y.X & (int) x.Y == (int) y.Y & (int) x.Z == (int) y.Z;

      public int GetHashCode(Vector3UByte obj) => ((int) obj.X * 397 ^ (int) obj.Y) * 397 ^ (int) obj.Z;

      public int Compare(Vector3UByte a, Vector3UByte b)
      {
        int num1 = (int) a.X - (int) b.X;
        int num2 = (int) a.Y - (int) b.Y;
        int num3 = (int) a.Z - (int) b.Z;
        if (num1 != 0)
          return num1;
        return num2 == 0 ? num3 : num2;
      }
    }

    protected class VRageMath_Vector3UByte\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Vector3UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3UByte owner, in byte value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3UByte owner, out byte value) => value = owner.X;
    }

    protected class VRageMath_Vector3UByte\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Vector3UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3UByte owner, in byte value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3UByte owner, out byte value) => value = owner.Y;
    }

    protected class VRageMath_Vector3UByte\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<Vector3UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector3UByte owner, in byte value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector3UByte owner, out byte value) => value = owner.Z;
    }
  }
}
