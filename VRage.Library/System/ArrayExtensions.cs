// Decompiled with JetBrains decompiler
// Type: System.ArrayExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;
using VRage.Extensions;
using VRage.Library.Collections;

namespace System
{
  public static class ArrayExtensions
  {
    public static bool IsValidIndex<T>(this T[] self, int index) => (uint) index < (uint) self.Length;

    public static bool IsNullOrEmpty<T>(this T[] self) => self == null || self.Length == 0;

    public static bool TryGetValue<T>(this T[] self, int index, out T value)
    {
      if ((uint) index < (uint) self.Length)
      {
        value = self[index];
        return true;
      }
      value = default (T);
      return false;
    }

    public static T[] Without<T>(this T[] self, List<int> indices) => self.RemoveIndices<T>(indices);

    public static T[] RemoveIndices<T>(this T[] self, List<int> indices)
    {
      if (indices.Count >= self.Length)
        return new T[0];
      if (indices.Count == 0)
        return self;
      T[] objArray = new T[self.Length - indices.Count];
      int index1 = 0;
      for (int index2 = 0; index2 < self.Length - indices.Count; ++index2)
      {
        while (index1 < indices.Count && index2 == indices[index1] - index1)
          ++index1;
        objArray[index2] = self[index2 + index1];
      }
      return objArray;
    }

    public static T[] Without<T>(this T[] self, int position)
    {
      T[] objArray = new T[self.Length - 1];
      for (int index = position; index < objArray.Length; ++index)
        objArray[index] = self[index + 1];
      return objArray;
    }

    public static int BinaryIntervalSearch<T>(this T[] self, T value) where T : IComparable<T>
    {
      if (self.Length == 0)
        return 0;
      if (self.Length == 1)
        return value.CompareTo(self[0]) < 0 ? 0 : 1;
      int index1 = 0;
      int num1 = self.Length;
      while (num1 - index1 > 1)
      {
        int index2 = (index1 + num1) / 2;
        if (value.CompareTo(self[index2]) >= 0)
          index1 = index2;
        else
          num1 = index2;
      }
      int num2 = index1;
      if (value.CompareTo(self[index1]) >= 0)
        num2 = num1;
      return num2;
    }

    public static int BinaryIntervalSearch<T>(this T[] self, Func<T, int> selector)
    {
      if (self.Length == 0)
        return 0;
      if (self.Length == 1)
        return selector(self[0]) < 0 ? 0 : 1;
      int index1 = 0;
      int num1 = self.Length;
      while (num1 - index1 > 1)
      {
        int index2 = (index1 + num1) / 2;
        if (selector(self[index2]) >= 0)
          index1 = index2;
        else
          num1 = index2;
      }
      int num2 = index1;
      if (selector(self[index1]) >= 0)
        num2 = num1;
      return num2;
    }

    public static MyRangeIterator<T>.Enumerable Range<T>(
      this T[] array,
      int start,
      int end)
    {
      return MyRangeIterator<T>.ForRange(array, start, end);
    }

    public static ArrayOfTypeEnumerator<TBase, ArrayEnumerator<TBase>, T> OfTypeFast<TBase, T>(
      this TBase[] array)
      where T : TBase
    {
      return new ArrayOfTypeEnumerator<TBase, ArrayEnumerator<TBase>, T>(new ArrayEnumerator<TBase>(array));
    }

    public static T[] CreateSubarray<T>(
      this T[] inputArray,
      int theFirstElement,
      int elementsCount)
    {
      int num = theFirstElement + elementsCount;
      if (inputArray.Length < num)
        throw new ArgumentOutOfRangeException("The requested interval for the subarray is out of the boundaries");
      T[] objArray = new T[elementsCount];
      for (int index = 0; index < elementsCount; ++index)
        objArray[index] = inputArray[theFirstElement + index];
      return objArray;
    }

    public static unsafe bool Compare(this byte[] a1, byte[] a2)
    {
      if (a1 == null || a2 == null || a1.Length != a2.Length)
        return false;
      fixed (byte* numPtr1 = a1)
        fixed (byte* numPtr2 = a2)
        {
          byte* numPtr3 = numPtr1;
          byte* numPtr4 = numPtr2;
          int length = a1.Length;
          int num = 0;
          while (num < length / 8)
          {
            if (*(long*) numPtr3 != *(long*) numPtr4)
              return false;
            ++num;
            numPtr3 += 8;
            numPtr4 += 8;
          }
          if ((length & 4) != 0)
          {
            if (*(int*) numPtr3 != *(int*) numPtr4)
              return false;
            numPtr3 += 4;
            numPtr4 += 4;
          }
          if ((length & 2) != 0)
          {
            if ((int) *(short*) numPtr3 != (int) *(short*) numPtr4)
              return false;
            numPtr3 += 2;
            numPtr4 += 2;
          }
          return (length & 1) == 0 || (int) *numPtr3 == (int) *numPtr4;
        }
    }

    public static void ForEach(this Array array, Action<Array, int[]> action)
    {
      if (array.LongLength == 0L)
        return;
      ArrayTraverse arrayTraverse = new ArrayTraverse(array);
      do
      {
        action(array, arrayTraverse.Position);
      }
      while (arrayTraverse.Step());
    }

    public static bool Contains<T>(this T[] array, T element)
    {
      EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
      for (int index = 0; index < array.Length; ++index)
      {
        if (equalityComparer.Equals(array[index], element))
          return true;
      }
      return false;
    }

    public static System.Span<T> Span<T>(this T[] array, int offset, int? count = null) => new System.Span<T>(array, offset, count ?? array.Length - offset);

    public static void EnsureCapacity<T>(ref T[] array, int size, float growFactor = 1f)
    {
      if (array != null && array.Length >= size)
        return;
      T[] objArray = array;
      int newSize = Math.Max((int) ((objArray != null ? (double) objArray.Length : 0.0) * (double) growFactor), size);
      Array.Resize<T>(ref array, newSize);
    }
  }
}
