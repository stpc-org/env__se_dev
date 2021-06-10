// Decompiled with JetBrains decompiler
// Type: VRageMath.PackedVector.NormalizedByte4
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Globalization;

namespace VRageMath.PackedVector
{
  public struct NormalizedByte4 : IPackedVector<uint>, IPackedVector, IEquatable<NormalizedByte4>
  {
    private uint packedValue;

    public uint PackedValue
    {
      get => this.packedValue;
      set => this.packedValue = value;
    }

    public NormalizedByte4(float x, float y, float z, float w) => this.packedValue = NormalizedByte4.PackHelper(x, y, z, w);

    public NormalizedByte4(Vector4 vector) => this.packedValue = NormalizedByte4.PackHelper(vector.X, vector.Y, vector.Z, vector.W);

    public static bool operator ==(NormalizedByte4 a, NormalizedByte4 b) => a.Equals(b);

    public static bool operator !=(NormalizedByte4 a, NormalizedByte4 b) => !a.Equals(b);

    void IPackedVector.PackFromVector4(Vector4 vector) => this.packedValue = NormalizedByte4.PackHelper(vector.X, vector.Y, vector.Z, vector.W);

    private static uint PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW) => (uint) ((int) PackUtils.PackSNorm((uint) byte.MaxValue, vectorX) | (int) PackUtils.PackSNorm((uint) byte.MaxValue, vectorY) << 8 | (int) PackUtils.PackSNorm((uint) byte.MaxValue, vectorZ) << 16 | (int) PackUtils.PackSNorm((uint) byte.MaxValue, vectorW) << 24);

    public Vector4 ToVector4()
    {
      Vector4 vector4;
      vector4.X = PackUtils.UnpackSNorm((uint) byte.MaxValue, this.packedValue);
      vector4.Y = PackUtils.UnpackSNorm((uint) byte.MaxValue, this.packedValue >> 8);
      vector4.Z = PackUtils.UnpackSNorm((uint) byte.MaxValue, this.packedValue >> 16);
      vector4.W = PackUtils.UnpackSNorm((uint) byte.MaxValue, this.packedValue >> 24);
      return vector4;
    }

    public override string ToString() => this.packedValue.ToString("X8", (IFormatProvider) CultureInfo.InvariantCulture);

    public override int GetHashCode() => this.packedValue.GetHashCode();

    public override bool Equals(object obj) => obj is NormalizedByte4 other && this.Equals(other);

    public bool Equals(NormalizedByte4 other) => this.packedValue.Equals(other.packedValue);
  }
}
