// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.MyLibraryUtils
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VRage.Library.Utils
{
  public class MyLibraryUtils
  {
    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public static void AssertBlittable<T>()
    {
      try
      {
        if ((object) default (T) == null)
          return;
        GCHandle.Alloc((object) default (T), GCHandleType.Pinned).Free();
      }
      catch
      {
      }
    }

    public static void ThrowNonBlittable<T>()
    {
      try
      {
        if ((object) default (T) == null)
          throw new InvalidOperationException("Class is never blittable");
        GCHandle.Alloc((object) default (T), GCHandleType.Pinned).Free();
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Type '" + (object) typeof (T) + "' is not blittable", ex);
      }
    }

    public static uint NormalizeFloat(float value, float min, float max, int bits)
    {
      int num = (1 << bits) - 1;
      value = (float) (((double) value - (double) min) / ((double) max - (double) min));
      return (uint) ((double) value * (double) num + 0.5);
    }

    public static float DenormalizeFloat(uint value, float min, float max, int bits)
    {
      int num1 = (1 << bits) - 1;
      float num2 = (float) value / (float) num1;
      return min + num2 * (max - min);
    }

    public static uint NormalizeFloatCenter(float value, float min, float max, int bits)
    {
      int num = (1 << bits) - 2;
      value = (float) (((double) value - (double) min) / ((double) max - (double) min));
      return (uint) ((double) value * (double) num + 0.5);
    }

    public static float DenormalizeFloatCenter(uint value, float min, float max, int bits)
    {
      int num1 = (1 << bits) - 2;
      float num2 = (float) value / (float) num1;
      return min + num2 * (max - min);
    }

    public static int GetDivisionCeil(int num, int div) => (num - 1) / div + 1;

    public static long GetDivisionCeil(long num, long div) => (num - 1L) / div + 1L;
  }
}
