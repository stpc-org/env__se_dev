// Decompiled with JetBrains decompiler
// Type: BulletXNA.LinearMath.IndexedVector3
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace BulletXNA.LinearMath
{
  public struct IndexedVector3
  {
    private static IndexedVector3 _zero = new IndexedVector3();
    private static IndexedVector3 _one = new IndexedVector3(1f);
    private static IndexedVector3 _up = new IndexedVector3(0.0f, 1f, 0.0f);
    public float X;
    public float Y;
    public float Z;

    public IndexedVector3(float x, float y, float z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public IndexedVector3(float x)
    {
      this.X = x;
      this.Y = x;
      this.Z = x;
    }

    public IndexedVector3(IndexedVector3 v)
    {
      this.X = v.X;
      this.Y = v.Y;
      this.Z = v.Z;
    }

    public static IndexedVector3 operator +(
      IndexedVector3 value1,
      IndexedVector3 value2)
    {
      IndexedVector3 indexedVector3;
      indexedVector3.X = value1.X + value2.X;
      indexedVector3.Y = value1.Y + value2.Y;
      indexedVector3.Z = value1.Z + value2.Z;
      return indexedVector3;
    }

    public static IndexedVector3 operator -(
      IndexedVector3 value1,
      IndexedVector3 value2)
    {
      IndexedVector3 indexedVector3;
      indexedVector3.X = value1.X - value2.X;
      indexedVector3.Y = value1.Y - value2.Y;
      indexedVector3.Z = value1.Z - value2.Z;
      return indexedVector3;
    }

    public static IndexedVector3 operator *(IndexedVector3 value, float scaleFactor)
    {
      IndexedVector3 indexedVector3;
      indexedVector3.X = value.X * scaleFactor;
      indexedVector3.Y = value.Y * scaleFactor;
      indexedVector3.Z = value.Z * scaleFactor;
      return indexedVector3;
    }

    public static IndexedVector3 operator *(float scaleFactor, IndexedVector3 value)
    {
      IndexedVector3 indexedVector3;
      indexedVector3.X = value.X * scaleFactor;
      indexedVector3.Y = value.Y * scaleFactor;
      indexedVector3.Z = value.Z * scaleFactor;
      return indexedVector3;
    }

    public static IndexedVector3 operator -(IndexedVector3 value)
    {
      IndexedVector3 indexedVector3;
      indexedVector3.X = -value.X;
      indexedVector3.Y = -value.Y;
      indexedVector3.Z = -value.Z;
      return indexedVector3;
    }

    public static IndexedVector3 operator *(
      IndexedVector3 value1,
      IndexedVector3 value2)
    {
      IndexedVector3 indexedVector3;
      indexedVector3.X = value1.X * value2.X;
      indexedVector3.Y = value1.Y * value2.Y;
      indexedVector3.Z = value1.Z * value2.Z;
      return indexedVector3;
    }

    public static IndexedVector3 operator /(
      IndexedVector3 value1,
      IndexedVector3 value2)
    {
      IndexedVector3 indexedVector3;
      indexedVector3.X = value1.X / value2.X;
      indexedVector3.Y = value1.Y / value2.Y;
      indexedVector3.Z = value1.Z / value2.Z;
      return indexedVector3;
    }

    public float this[int i]
    {
      get
      {
        switch (i)
        {
          case 0:
            return this.X;
          case 1:
            return this.Y;
          case 2:
            return this.Z;
          default:
            return 0.0f;
        }
      }
      set
      {
        switch (i)
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
        }
      }
    }

    public bool Equals(IndexedVector3 other) => (double) this.X == (double) other.X && (double) this.Y == (double) other.Y && (double) this.Z == (double) other.Z;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is IndexedVector3 other)
        flag = this.Equals(other);
      return flag;
    }

    public static IndexedVector3 Zero => IndexedVector3._zero;

    public float Dot(ref IndexedVector3 v) => (float) ((double) this.X * (double) v.X + (double) this.Y * (double) v.Y + (double) this.Z * (double) v.Z);

    public float Dot(IndexedVector3 v) => (float) ((double) this.X * (double) v.X + (double) this.Y * (double) v.Y + (double) this.Z * (double) v.Z);

    public override int GetHashCode() => (this.X.GetHashCode() * 397 ^ this.Y.GetHashCode()) * 397 ^ this.Z.GetHashCode();
  }
}
