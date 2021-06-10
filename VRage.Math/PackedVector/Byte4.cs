// Decompiled with JetBrains decompiler
// Type: VRageMath.PackedVector.Byte4
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Globalization;

namespace VRageMath.PackedVector
{
  public struct Byte4 : IPackedVector<uint>, IPackedVector, IEquatable<Byte4>
  {
    private uint packedValue;

    public uint PackedValue
    {
      get => this.packedValue;
      set => this.packedValue = value;
    }

    public Byte4(float x, float y, float z, float w) => this.packedValue = Byte4.PackHelper(x, y, z, w);

    public Byte4(Vector4 vector) => this.packedValue = Byte4.PackHelper(vector.X, vector.Y, vector.Z, vector.W);

    public Byte4(uint packedValue) => this.packedValue = packedValue;

    public static bool operator ==(Byte4 a, Byte4 b) => a.Equals(b);

    public static bool operator !=(Byte4 a, Byte4 b) => !a.Equals(b);

    void IPackedVector.PackFromVector4(Vector4 vector) => this.packedValue = Byte4.PackHelper(vector.X, vector.Y, vector.Z, vector.W);

    private static uint PackHelper(float vectorX, float vectorY, float vectorZ, float vectorW) => (uint) ((int) PackUtils.PackUnsigned((float) byte.MaxValue, vectorX) | (int) PackUtils.PackUnsigned((float) byte.MaxValue, vectorY) << 8 | (int) PackUtils.PackUnsigned((float) byte.MaxValue, vectorZ) << 16 | (int) PackUtils.PackUnsigned((float) byte.MaxValue, vectorW) << 24);

    public Vector4 ToVector4()
    {
      Vector4 vector4;
      vector4.X = (float) (this.packedValue & (uint) byte.MaxValue);
      vector4.Y = (float) (this.packedValue >> 8 & (uint) byte.MaxValue);
      vector4.Z = (float) (this.packedValue >> 16 & (uint) byte.MaxValue);
      vector4.W = (float) (this.packedValue >> 24 & (uint) byte.MaxValue);
      return vector4;
    }

    public Vector4UByte ToVector4UByte()
    {
      Vector4UByte vector4Ubyte;
      vector4Ubyte.X = (byte) (this.packedValue & (uint) byte.MaxValue);
      vector4Ubyte.Y = (byte) (this.packedValue >> 8 & (uint) byte.MaxValue);
      vector4Ubyte.Z = (byte) (this.packedValue >> 16 & (uint) byte.MaxValue);
      vector4Ubyte.W = (byte) (this.packedValue >> 24 & (uint) byte.MaxValue);
      return vector4Ubyte;
    }

    public override string ToString() => this.packedValue.ToString("X8", (IFormatProvider) CultureInfo.InvariantCulture);

    public override int GetHashCode() => this.packedValue.GetHashCode();

    public override bool Equals(object obj) => obj is Byte4 other && this.Equals(other);

    public bool Equals(Byte4 other) => this.packedValue.Equals(other.packedValue);
  }
}
