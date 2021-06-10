// Decompiled with JetBrains decompiler
// Type: BulletXNA.BulletCollision.AABB
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using BulletXNA.LinearMath;
using System;

namespace BulletXNA.BulletCollision
{
  public struct AABB
  {
    public IndexedVector3 m_min;
    public IndexedVector3 m_max;

    public AABB(ref IndexedVector3 min, ref IndexedVector3 max)
    {
      this.m_min = min;
      this.m_max = max;
    }

    public AABB(IndexedVector3 min, IndexedVector3 max)
    {
      this.m_min = min;
      this.m_max = max;
    }

    public void Invalidate()
    {
      this.m_min.X = float.MaxValue;
      this.m_min.Y = float.MaxValue;
      this.m_min.Z = float.MaxValue;
      this.m_max.X = float.MinValue;
      this.m_max.Y = float.MinValue;
      this.m_max.Z = float.MinValue;
    }

    public void Merge(AABB box) => this.Merge(ref box);

    public void Merge(ref AABB box)
    {
      this.m_min.X = BoxCollision.BT_MIN(this.m_min.X, box.m_min.X);
      this.m_min.Y = BoxCollision.BT_MIN(this.m_min.Y, box.m_min.Y);
      this.m_min.Z = BoxCollision.BT_MIN(this.m_min.Z, box.m_min.Z);
      this.m_max.X = BoxCollision.BT_MAX(this.m_max.X, box.m_max.X);
      this.m_max.Y = BoxCollision.BT_MAX(this.m_max.Y, box.m_max.Y);
      this.m_max.Z = BoxCollision.BT_MAX(this.m_max.Z, box.m_max.Z);
    }

    public void GetCenterExtend(out IndexedVector3 center, out IndexedVector3 extend)
    {
      center = new IndexedVector3((this.m_max + this.m_min) * 0.5f);
      extend = new IndexedVector3(this.m_max - center);
    }

    public bool CollideRay(ref IndexedVector3 vorigin, ref IndexedVector3 vdir)
    {
      IndexedVector3 center;
      IndexedVector3 extend;
      this.GetCenterExtend(out center, out extend);
      float x1 = vorigin.X - center.X;
      if (BoxCollision.BT_GREATER(x1, extend.X) && (double) x1 * (double) vdir.X >= 0.0)
        return false;
      float x2 = vorigin.Y - center.Y;
      if (BoxCollision.BT_GREATER(x2, extend.Y) && (double) x2 * (double) vdir.Y >= 0.0)
        return false;
      float x3 = vorigin.Z - center.Z;
      return (!BoxCollision.BT_GREATER(x3, extend.Z) || (double) x3 * (double) vdir.Z < 0.0) && ((double) Math.Abs((float) ((double) vdir.Y * (double) x3 - (double) vdir.Z * (double) x2)) <= (double) extend.Y * (double) Math.Abs(vdir.Z) + (double) extend.Z * (double) Math.Abs(vdir.Y) && (double) Math.Abs((float) ((double) vdir.Z * (double) x1 - (double) vdir.X * (double) x3)) <= (double) extend.X * (double) Math.Abs(vdir.Z) + (double) extend.Z * (double) Math.Abs(vdir.X)) && (double) Math.Abs((float) ((double) vdir.X * (double) x2 - (double) vdir.Y * (double) x1)) <= (double) extend.X * (double) Math.Abs(vdir.Y) + (double) extend.Y * (double) Math.Abs(vdir.X);
    }

    public float? CollideRayDistance(ref IndexedVector3 origin, ref IndexedVector3 direction)
    {
      IndexedVector3 indexedVector3 = new IndexedVector3(1f / direction.X, 1f / direction.Y, 1f / direction.Z);
      float val1_1 = (this.m_min.X - origin.X) * indexedVector3.X;
      float val2_1 = (this.m_max.X - origin.X) * indexedVector3.X;
      float val1_2 = (this.m_min.Y - origin.Y) * indexedVector3.Y;
      float val2_2 = (this.m_max.Y - origin.Y) * indexedVector3.Y;
      float val1_3 = (this.m_min.Z - origin.Z) * indexedVector3.Z;
      float val2_3 = (this.m_max.Z - origin.Z) * indexedVector3.Z;
      float num1 = Math.Max(Math.Max(Math.Min(val1_1, val2_1), Math.Min(val1_2, val2_2)), Math.Min(val1_3, val2_3));
      float num2 = Math.Min(Math.Min(Math.Max(val1_1, val2_1), Math.Max(val1_2, val2_2)), Math.Max(val1_3, val2_3));
      float num3;
      if ((double) num2 < 0.0)
      {
        num3 = num2;
        return new float?();
      }
      if ((double) num1 <= (double) num2)
        return new float?(num1);
      num3 = num2;
      return new float?();
    }
  }
}
