// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.MyCellCoord
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using VRageMath;

namespace VRage.Voxels
{
  public struct MyCellCoord : IComparable<MyCellCoord>, IEquatable<MyCellCoord>
  {
    private const int BITS_LOD = 4;
    private const int BITS_X_32 = 10;
    private const int BITS_Y_32 = 8;
    private const int BITS_Z_32 = 10;
    private const int BITS_X_64 = 20;
    private const int BITS_Y_64 = 20;
    private const int BITS_Z_64 = 20;
    private const int SHIFT_Z_32 = 0;
    private const int SHIFT_Y_32 = 10;
    private const int SHIFT_X_32 = 18;
    private const int SHIFT_LOD_32 = 28;
    private const int SHIFT_Z_64 = 0;
    private const int SHIFT_Y_64 = 20;
    private const int SHIFT_X_64 = 40;
    private const int SHIFT_LOD_64 = 60;
    private const int MASK_LOD = 15;
    private const int MASK_X_32 = 1023;
    private const int MASK_Y_32 = 255;
    private const int MASK_Z_32 = 1023;
    private const int MASK_X_64 = 1048575;
    private const int MASK_Y_64 = 1048575;
    private const int MASK_Z_64 = 1048575;
    public const int MAX_LOD_COUNT = 16;
    public int Lod;
    public Vector3I CoordInLod;
    public static readonly MyCellCoord.EqualityComparer Comparer = new MyCellCoord.EqualityComparer();

    public override bool Equals(object obj) => obj != null && obj is MyCellCoord other && this.Equals(other);

    public override int GetHashCode() => this.Lod * 397 ^ this.CoordInLod.GetHashCode();

    public MyCellCoord(ulong packedId)
    {
      this.CoordInLod.Z = (int) ((long) packedId & 1023L);
      packedId >>= 10;
      this.CoordInLod.Y = (int) ((long) packedId & (long) byte.MaxValue);
      packedId >>= 8;
      this.CoordInLod.X = (int) ((long) packedId & 1023L);
      packedId >>= 10;
      this.Lod = (int) packedId;
    }

    public MyCellCoord(int lod, Vector3I coordInLod)
      : this(lod, ref coordInLod)
    {
    }

    public MyCellCoord(int lod, ref Vector3I coordInLod)
    {
      this.Lod = lod;
      this.CoordInLod = coordInLod;
    }

    public void SetUnpack(uint id)
    {
      this.CoordInLod.Z = (int) id & 1023;
      id >>= 10;
      this.CoordInLod.Y = (int) id & (int) byte.MaxValue;
      id >>= 8;
      this.CoordInLod.X = (int) id & 1023;
      id >>= 10;
      this.Lod = (int) id;
    }

    public void SetUnpack(ulong id)
    {
      this.CoordInLod.Z = (int) ((long) id & 1048575L);
      id >>= 20;
      this.CoordInLod.Y = (int) ((long) id & 1048575L);
      id >>= 20;
      this.CoordInLod.X = (int) ((long) id & 1048575L);
      id >>= 20;
      this.Lod = (int) id;
    }

    public static int UnpackLod(ulong id) => (int) (id >> 60);

    public static Vector3I UnpackCoord(ulong id)
    {
      Vector3I vector3I;
      vector3I.Z = (int) ((long) id & 1048575L);
      id >>= 20;
      vector3I.Y = (int) ((long) id & 1048575L);
      id >>= 20;
      vector3I.X = (int) ((long) id & 1048575L);
      id >>= 20;
      return vector3I;
    }

    public static ulong PackId64Static(int lod, Vector3I coordInLod) => (ulong) ((long) lod << 60 | (long) coordInLod.X << 40 | (long) coordInLod.Y << 20) | (ulong) coordInLod.Z;

    public uint PackId32() => (uint) (this.Lod << 28 | this.CoordInLod.X << 18 | this.CoordInLod.Y << 10 | this.CoordInLod.Z);

    public ulong PackId64() => (ulong) ((long) this.Lod << 60 | (long) this.CoordInLod.X << 40 | (long) this.CoordInLod.Y << 20) | (ulong) this.CoordInLod.Z;

    public bool IsCoord64Valid() => (this.CoordInLod.X & 1048575) == this.CoordInLod.X && (this.CoordInLod.Y & 1048575) == this.CoordInLod.Y && (this.CoordInLod.Z & 1048575) == this.CoordInLod.Z;

    public static ulong GetClipmapCellHash(uint clipmap, MyCellCoord cellId) => MyCellCoord.GetClipmapCellHash(clipmap, cellId.PackId64());

    public static ulong GetClipmapCellHash(uint clipmap, ulong cellId) => (ulong) ((long) cellId * 997L * 397L) ^ (ulong) (clipmap * 997U);

    public static bool operator ==(MyCellCoord x, MyCellCoord y) => x.CoordInLod.X == y.CoordInLod.X && x.CoordInLod.Y == y.CoordInLod.Y && x.CoordInLod.Z == y.CoordInLod.Z && x.Lod == y.Lod;

    public static bool operator !=(MyCellCoord x, MyCellCoord y) => x.CoordInLod.X != y.CoordInLod.X || x.CoordInLod.Y != y.CoordInLod.Y || x.CoordInLod.Z != y.CoordInLod.Z || x.Lod != y.Lod;

    public bool Equals(MyCellCoord other) => this == other;

    public override string ToString() => string.Format("{0}, {1}", (object) this.Lod, (object) this.CoordInLod);

    public int CompareTo(MyCellCoord other)
    {
      int num1 = this.CoordInLod.X - other.CoordInLod.X;
      int num2 = this.CoordInLod.Y - other.CoordInLod.Y;
      int num3 = this.CoordInLod.Z - other.CoordInLod.Z;
      int num4 = this.Lod - other.Lod;
      if (num1 != 0)
        return num1;
      if (num2 != 0)
        return num2;
      return num3 == 0 ? num4 : num3;
    }

    public class EqualityComparer : IEqualityComparer<MyCellCoord>, IComparer<MyCellCoord>
    {
      public bool Equals(MyCellCoord x, MyCellCoord y) => x.CoordInLod.X == y.CoordInLod.X && x.CoordInLod.Y == y.CoordInLod.Y && x.CoordInLod.Z == y.CoordInLod.Z && x.Lod == y.Lod;

      public int GetHashCode(MyCellCoord obj) => ((obj.CoordInLod.X * 397 ^ obj.CoordInLod.Y) * 397 ^ obj.CoordInLod.Z) * 397 ^ obj.Lod;

      public int Compare(MyCellCoord x, MyCellCoord y) => x.CompareTo(y);
    }
  }
}
