// Decompiled with JetBrains decompiler
// Type: BulletXNA.BulletCollision.GImpactQuantization
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using BulletXNA.LinearMath;

namespace BulletXNA.BulletCollision
{
  public class GImpactQuantization
  {
    public static void CalcQuantizationParameters(
      out IndexedVector3 outMinBound,
      out IndexedVector3 outMaxBound,
      out IndexedVector3 bvhQuantization,
      ref IndexedVector3 srcMinBound,
      ref IndexedVector3 srcMaxBound,
      float quantizationMargin)
    {
      IndexedVector3 indexedVector3_1 = new IndexedVector3(quantizationMargin);
      outMinBound = srcMinBound - indexedVector3_1;
      outMaxBound = srcMaxBound + indexedVector3_1;
      IndexedVector3 indexedVector3_2 = outMaxBound - outMinBound;
      bvhQuantization = new IndexedVector3((float) ushort.MaxValue) / indexedVector3_2;
    }

    public static void QuantizeClamp(
      out UShortVector3 output,
      ref IndexedVector3 point,
      ref IndexedVector3 min_bound,
      ref IndexedVector3 max_bound,
      ref IndexedVector3 bvhQuantization)
    {
      IndexedVector3 output1 = point;
      MathUtil.VectorMax(ref min_bound, ref output1);
      MathUtil.VectorMin(ref max_bound, ref output1);
      IndexedVector3 indexedVector3 = (output1 - min_bound) * bvhQuantization;
      output = new UShortVector3();
      output[0] = (ushort) ((double) indexedVector3.X + 0.5);
      output[1] = (ushort) ((double) indexedVector3.Y + 0.5);
      output[2] = (ushort) ((double) indexedVector3.Z + 0.5);
    }

    public static IndexedVector3 Unquantize(
      ref UShortVector3 vecIn,
      ref IndexedVector3 offset,
      ref IndexedVector3 bvhQuantization)
    {
      return new IndexedVector3((float) vecIn[0] / bvhQuantization.X, (float) vecIn[1] / bvhQuantization.Y, (float) vecIn[2] / bvhQuantization.Z) + offset;
    }
  }
}
