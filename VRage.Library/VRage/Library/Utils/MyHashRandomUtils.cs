// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.MyHashRandomUtils
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.IO;
using System.Text;

namespace VRage.Library.Utils
{
  public static class MyHashRandomUtils
  {
    public static unsafe float CreateFloatFromMantissa(uint m)
    {
      m &= 8388607U;
      m |= 1065353216U;
      return *(float*) &m - 1f;
    }

    public static uint JenkinsHash(uint x)
    {
      x += x << 10;
      x ^= x >> 6;
      x += x << 3;
      x ^= x >> 11;
      x += x << 15;
      return x;
    }

    public static float UniformFloatFromSeed(int seed) => MyHashRandomUtils.CreateFloatFromMantissa(MyHashRandomUtils.JenkinsHash((uint) seed));

    public static void TestHashSample()
    {
      float[] numArray = new float[100000000];
      using (new MySimpleTestTimer("Int to sample fast"))
      {
        for (int seed = 0; seed < 100000000; ++seed)
          numArray[seed] = MyHashRandomUtils.UniformFloatFromSeed(seed);
      }
      float num1 = 0.0f;
      float maxValue = float.MaxValue;
      float minValue = float.MinValue;
      for (int index = 0; index < 100000000; ++index)
      {
        num1 += numArray[index];
        if ((double) maxValue > (double) numArray[index])
          maxValue = numArray[index];
        if ((double) minValue < (double) numArray[index])
          minValue = numArray[index];
      }
      float num2 = num1 / 1E+08f;
      float num3 = 0.0f;
      for (int index = 0; index < 100000000; ++index)
      {
        float num4 = numArray[index] - num2;
        num3 += num4 * num4;
      }
      float num5 = (float) Math.Sqrt((double) num3) / 1E+08f;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("Min/Max/Avg: {0}/{1}/{2}\n", (object) maxValue, (object) minValue, (object) num2);
      stringBuilder.AppendFormat("Std dev: {0}\n", (object) num5);
      File.AppendAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "perf.log"), stringBuilder.ToString());
    }
  }
}
