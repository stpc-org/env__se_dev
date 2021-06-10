// Decompiled with JetBrains decompiler
// Type: BulletXNA.LinearMath.IndexedBasisMatrix
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace BulletXNA.LinearMath
{
  public struct IndexedBasisMatrix
  {
    public IndexedVector3 _Row0;
    public IndexedVector3 _Row1;
    public IndexedVector3 _Row2;

    public IndexedBasisMatrix(
      float m11,
      float m12,
      float m13,
      float m21,
      float m22,
      float m23,
      float m31,
      float m32,
      float m33)
    {
      this._Row0 = new IndexedVector3(m11, m12, m13);
      this._Row1 = new IndexedVector3(m21, m22, m23);
      this._Row2 = new IndexedVector3(m31, m32, m33);
    }

    public IndexedVector3 GetRow(int i)
    {
      switch (i)
      {
        case 0:
          return this._Row0;
        case 1:
          return this._Row1;
        case 2:
          return this._Row2;
        default:
          return IndexedVector3.Zero;
      }
    }

    public float this[int i, int j]
    {
      get
      {
        switch (i)
        {
          case 0:
            switch (j)
            {
              case 0:
                return this._Row0.X;
              case 1:
                return this._Row0.Y;
              case 2:
                return this._Row0.Z;
            }
            break;
          case 1:
            switch (j)
            {
              case 0:
                return this._Row1.X;
              case 1:
                return this._Row1.Y;
              case 2:
                return this._Row1.Z;
            }
            break;
          case 2:
            switch (j)
            {
              case 0:
                return this._Row2.X;
              case 1:
                return this._Row2.Y;
              case 2:
                return this._Row2.Z;
            }
            break;
        }
        return 0.0f;
      }
      set
      {
        switch (i)
        {
          case 0:
            switch (j)
            {
              case 0:
                this._Row0.X = value;
                return;
              case 1:
                this._Row0.Y = value;
                return;
              case 2:
                this._Row0.Z = value;
                return;
              default:
                return;
            }
          case 1:
            switch (j)
            {
              case 0:
                this._Row1.X = value;
                return;
              case 1:
                this._Row1.Y = value;
                return;
              case 2:
                this._Row1.Z = value;
                return;
              default:
                return;
            }
          case 2:
            switch (j)
            {
              case 0:
                this._Row2.X = value;
                return;
              case 1:
                this._Row2.Y = value;
                return;
              case 2:
                this._Row2.Z = value;
                return;
              default:
                return;
            }
        }
      }
    }

    public IndexedVector3 this[int i]
    {
      get
      {
        switch (i)
        {
          case 0:
            return this._Row0;
          case 1:
            return this._Row1;
          case 2:
            return this._Row2;
          default:
            return IndexedVector3.Zero;
        }
      }
      set
      {
        switch (i)
        {
          case 0:
            this._Row0 = value;
            break;
          case 1:
            this._Row1 = value;
            break;
          case 2:
            this._Row2 = value;
            break;
        }
      }
    }

    public static IndexedVector3 operator *(IndexedBasisMatrix m, IndexedVector3 v) => new IndexedVector3(m._Row0.Dot(ref v), m._Row1.Dot(ref v), m._Row2.Dot(ref v));

    public static IndexedBasisMatrix operator *(
      IndexedBasisMatrix m1,
      IndexedBasisMatrix m2)
    {
      return new IndexedBasisMatrix(m2.TDotX(ref m1._Row0), m2.TDotY(ref m1._Row0), m2.TDotZ(ref m1._Row0), m2.TDotX(ref m1._Row1), m2.TDotY(ref m1._Row1), m2.TDotZ(ref m1._Row1), m2.TDotX(ref m1._Row2), m2.TDotY(ref m1._Row2), m2.TDotZ(ref m1._Row2));
    }

    public float TDotX(ref IndexedVector3 v) => (float) ((double) this._Row0.X * (double) v.X + (double) this._Row1.X * (double) v.Y + (double) this._Row2.X * (double) v.Z);

    public float TDotY(ref IndexedVector3 v) => (float) ((double) this._Row0.Y * (double) v.X + (double) this._Row1.Y * (double) v.Y + (double) this._Row2.Y * (double) v.Z);

    public float TDotZ(ref IndexedVector3 v) => (float) ((double) this._Row0.Z * (double) v.X + (double) this._Row1.Z * (double) v.Y + (double) this._Row2.Z * (double) v.Z);

    public IndexedBasisMatrix Inverse()
    {
      IndexedVector3 v = new IndexedVector3(this.Cofac(1, 1, 2, 2), this.Cofac(1, 2, 2, 0), this.Cofac(1, 0, 2, 1));
      float num = 1f / this[0].Dot(v);
      return new IndexedBasisMatrix(v.X * num, this.Cofac(0, 2, 2, 1) * num, this.Cofac(0, 1, 1, 2) * num, v.Y * num, this.Cofac(0, 0, 2, 2) * num, this.Cofac(0, 2, 1, 0) * num, v.Z * num, this.Cofac(0, 1, 2, 0) * num, this.Cofac(0, 0, 1, 1) * num);
    }

    public float Cofac(int r1, int c1, int r2, int c2)
    {
      IndexedVector3 indexedVector3 = this[r1];
      double num1 = (double) indexedVector3[c1];
      indexedVector3 = this[r2];
      double num2 = (double) indexedVector3[c2];
      double num3 = num1 * num2;
      indexedVector3 = this[r1];
      double num4 = (double) indexedVector3[c2];
      indexedVector3 = this[r2];
      double num5 = (double) indexedVector3[c1];
      double num6 = num4 * num5;
      return (float) (num3 - num6);
    }

    public IndexedBasisMatrix Transpose() => new IndexedBasisMatrix(this._Row0.X, this._Row1.X, this._Row2.X, this._Row0.Y, this._Row1.Y, this._Row2.Y, this._Row0.Z, this._Row1.Z, this._Row2.Z);
  }
}
