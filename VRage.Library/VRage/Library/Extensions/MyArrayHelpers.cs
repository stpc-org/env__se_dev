// Decompiled with JetBrains decompiler
// Type: VRage.Library.Extensions.MyArrayHelpers
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Extensions
{
  public static class MyArrayHelpers
  {
    public static void ResizeNoCopy<T>(ref T[] array, int newSize)
    {
      if (array != null && array.Length == newSize)
        return;
      array = new T[newSize];
    }

    public static void Reserve<T>(ref T[] array, int size, int threshold = 1024, float allocScale = 1.5f)
    {
      if (array.Length >= size)
        return;
      int num = size == 0 ? 1 : size;
      Array.Resize<T>(ref array, num < threshold ? num * 2 : (int) ((double) num * (double) allocScale));
    }

    public static void ReserveNoCopy<T>(ref T[] array, int size, int threshold = 1024, float allocScale = 1.5f)
    {
      if (array.Length >= size)
        return;
      int num = size == 0 ? 1 : size;
      array = new T[num < threshold ? num * 2 : (int) ((double) num * (double) allocScale)];
    }

    public static void InitOrReserve<T>(ref T[] array, int size, int threshold = 1024, float allocScale = 1.5f)
    {
      if (array == null)
        array = new T[size];
      else
        MyArrayHelpers.Reserve<T>(ref array, size, threshold, allocScale);
    }

    public static void InitOrReserveNoCopy<T>(
      ref T[] array,
      int size,
      int threshold = 1024,
      float allocScale = 1.5f)
    {
      if (array == null)
        array = new T[size];
      else
        MyArrayHelpers.ReserveNoCopy<T>(ref array, size, threshold, allocScale);
    }
  }
}
