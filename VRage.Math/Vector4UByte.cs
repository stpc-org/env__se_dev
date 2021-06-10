// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector4UByte
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [ProtoContract]
  public struct Vector4UByte
  {
    [ProtoMember(1)]
    public byte X;
    [ProtoMember(4)]
    public byte Y;
    [ProtoMember(7)]
    public byte Z;
    [ProtoMember(10)]
    public byte W;

    public Vector4UByte(byte x, byte y, byte z, byte w)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
      this.W = w;
    }

    public override string ToString() => this.X.ToString() + ", " + (object) this.Y + ", " + (object) this.Z + ", " + (object) this.W;

    public static Vector4UByte Round(Vector3 vec) => Vector4UByte.Round(new Vector4(vec.X, vec.Y, vec.Z, 0.0f));

    public static Vector4UByte Round(Vector4 vec) => new Vector4UByte((byte) Math.Round((double) vec.X), (byte) Math.Round((double) vec.Y), (byte) Math.Round((double) vec.Z), (byte) 0);

    public static Vector4UByte Normalize(Vector3 vec, float range) => Vector4UByte.Round((vec / range / 2f + new Vector3(0.5f)) * (float) byte.MaxValue);

    public byte this[int index]
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

    protected class VRageMath_Vector4UByte\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Vector4UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector4UByte owner, in byte value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector4UByte owner, out byte value) => value = owner.X;
    }

    protected class VRageMath_Vector4UByte\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Vector4UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector4UByte owner, in byte value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector4UByte owner, out byte value) => value = owner.Y;
    }

    protected class VRageMath_Vector4UByte\u003C\u003EZ\u003C\u003EAccessor : IMemberAccessor<Vector4UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector4UByte owner, in byte value) => owner.Z = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector4UByte owner, out byte value) => value = owner.Z;
    }

    protected class VRageMath_Vector4UByte\u003C\u003EW\u003C\u003EAccessor : IMemberAccessor<Vector4UByte, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Vector4UByte owner, in byte value) => owner.W = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Vector4UByte owner, out byte value) => value = owner.W;
    }
  }
}
