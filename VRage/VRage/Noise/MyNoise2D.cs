// Decompiled with JetBrains decompiler
// Type: VRage.Noise.MyNoise2D
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRageMath;

namespace VRage.Noise
{
  public static class MyNoise2D
  {
    private static MyRNG m_rnd = new MyRNG();
    private const int B = 256;
    private const int BM = 255;
    private static float[] rand = new float[256];
    private static int[] perm = new int[512];

    public static void Init(int seed)
    {
      MyNoise2D.m_rnd.Seed = (uint) seed;
      for (int index = 0; index < 256; ++index)
      {
        MyNoise2D.rand[index] = MyNoise2D.m_rnd.NextFloat();
        MyNoise2D.perm[index] = index;
      }
      for (int index1 = 0; index1 < 256; ++index1)
      {
        int index2 = (int) MyNoise2D.m_rnd.NextInt() & (int) byte.MaxValue;
        int num = MyNoise2D.perm[index2];
        MyNoise2D.perm[index2] = MyNoise2D.perm[index1];
        MyNoise2D.perm[index1] = num;
        MyNoise2D.perm[index1 + 256] = MyNoise2D.perm[index1];
      }
    }

    public static float Noise(float x, float y)
    {
      int num1 = (int) x;
      int num2 = (int) y;
      float amount1 = x - (float) num1;
      float amount2 = y - (float) num2;
      int index1 = (int) byte.MaxValue & num1;
      int index2 = (int) byte.MaxValue & num1 + 1;
      int num3 = (int) byte.MaxValue & num2;
      int num4 = (int) byte.MaxValue & num2 + 1;
      double num5 = (double) MyNoise2D.rand[MyNoise2D.perm[MyNoise2D.perm[index1] + num3]];
      float num6 = MyNoise2D.rand[MyNoise2D.perm[MyNoise2D.perm[index2] + num3]];
      float num7 = MyNoise2D.rand[MyNoise2D.perm[MyNoise2D.perm[index1] + num4]];
      float num8 = MyNoise2D.rand[MyNoise2D.perm[MyNoise2D.perm[index2] + num4]];
      double num9 = (double) num6;
      double num10 = (double) amount1;
      return MathHelper.SmoothStep(MathHelper.SmoothStep((float) num5, (float) num9, (float) num10), MathHelper.SmoothStep(num7, num8, amount1), amount2);
    }

    public static float Rotation(float x, float y, int numLayers)
    {
      float[] numArray1 = new float[numLayers];
      float[] numArray2 = new float[numLayers];
      for (int index = 0; index < numLayers; ++index)
      {
        numArray1[index] = (float) Math.Sin(0.436332315206528 * (double) index);
        numArray2[index] = (float) Math.Cos(0.436332315206528 * (double) index);
      }
      float num1 = 0.0f;
      int num2 = 0;
      for (int index = 0; index < numLayers; ++index)
      {
        num1 += MyNoise2D.Noise((float) ((double) x * (double) numArray2[index] - (double) y * (double) numArray1[index]), (float) ((double) x * (double) numArray1[index] + (double) y * (double) numArray2[index]));
        ++num2;
      }
      return num1 / (float) num2;
    }

    public static float Fractal(float x, float y, int numOctaves)
    {
      int num1 = 1;
      float num2 = 1f;
      float num3 = 0.0f;
      float num4 = 0.0f;
      for (int index = 0; index < numOctaves; ++index)
      {
        num3 += num2;
        num4 += MyNoise2D.Noise(x * (float) num1, y * (float) num1) * num2;
        num2 *= 0.5f;
        num1 <<= 1;
      }
      return num4 / num3;
    }

    public static float FBM(float x, float y, int numLayers, float lacunarity, float gain)
    {
      float num1 = 1f;
      float num2 = 1f;
      float num3 = 0.0f;
      float num4 = 0.0f;
      for (int index = 0; index < numLayers; ++index)
      {
        num3 += num2;
        num4 += MyNoise2D.Noise(x * num1, y * num1) * num2;
        num2 *= gain;
        num1 *= lacunarity;
      }
      return num4 / num3;
    }

    public static float Billow(float x, float y, int numLayers)
    {
      int num1 = 1;
      float num2 = 1f;
      float num3 = 0.0f;
      float num4 = 0.0f;
      for (int index = 0; index < numLayers; ++index)
      {
        num3 += num2;
        num4 += Math.Abs((float) (2.0 * (double) MyNoise2D.Noise(x * (float) num1, y * (float) num1) - 1.0)) * num2;
        num2 *= 0.5f;
        num1 <<= 1;
      }
      return num4 / num3;
    }

    public static float Marble(float x, float y, int numOctaves) => (float) ((Math.Sin(4.0 * ((double) x + (double) MyNoise2D.Fractal(x * 0.5f, y * 0.5f, numOctaves))) + 1.0) * 0.5);

    public static float Wood(float x, float y, float scale)
    {
      double num;
      return (float) ((num = (double) MyNoise2D.Noise(x, y) * (double) scale) - num);
    }
  }
}
