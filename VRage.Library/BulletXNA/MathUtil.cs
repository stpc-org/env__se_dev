// Decompiled with JetBrains decompiler
// Type: BulletXNA.MathUtil
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using BulletXNA.LinearMath;
using System;
using System.Runtime.InteropServices;

namespace BulletXNA
{
  public static class MathUtil
  {
    public const float SIMD_EPSILON = 1.192093E-07f;
    public const float SIMD_INFINITY = 3.402823E+38f;

    public static int MaxAxis(ref IndexedVector3 a) => (double) a.X >= (double) a.Y ? ((double) a.X >= (double) a.Z ? 0 : 2) : ((double) a.Y >= (double) a.Z ? 1 : 2);

    public static void VectorMin(ref IndexedVector3 input, ref IndexedVector3 output)
    {
      output.X = Math.Min(input.X, output.X);
      output.Y = Math.Min(input.Y, output.Y);
      output.Z = Math.Min(input.Z, output.Z);
    }

    public static void VectorMax(ref IndexedVector3 input, ref IndexedVector3 output)
    {
      output.X = Math.Max(input.X, output.X);
      output.Y = Math.Max(input.Y, output.Y);
      output.Z = Math.Max(input.Z, output.Z);
    }

    public static float NextAfter(float x, float y)
    {
      if (float.IsNaN(x) || float.IsNaN(y))
        return x + y;
      if ((double) x == (double) y)
        return y;
      MathUtil.FloatIntUnion floatIntUnion;
      floatIntUnion.i = 0;
      floatIntUnion.f = x;
      if ((double) x == 0.0)
      {
        floatIntUnion.i = 1;
        return (double) y <= 0.0 ? -floatIntUnion.f : floatIntUnion.f;
      }
      if ((double) x > 0.0 == (double) y > (double) x)
        ++floatIntUnion.i;
      else
        --floatIntUnion.i;
      return floatIntUnion.f;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct FloatIntUnion
    {
      [FieldOffset(0)]
      public int i;
      [FieldOffset(0)]
      public float f;
    }
  }
}
