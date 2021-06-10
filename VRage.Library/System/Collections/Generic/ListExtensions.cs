// Decompiled with JetBrains decompiler
// Type: System.Collections.Generic.ListExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Diagnostics;
using System.Runtime.CompilerServices;
using VRage.Collections;

namespace System.Collections.Generic
{
  public static class ListExtensions
  {
    public static ClearToken<T> GetClearToken<T>(this List<T> list) => new ClearToken<T>()
    {
      List = list
    };

    public static void RemoveAtFast<T>(this List<T> list, int index)
    {
      int index1 = list.Count - 1;
      if (index != index1)
        list[index] = list[index1];
      list.RemoveAt(index1);
    }

    public static void RemoveAtFast<T>(this IList<T> list, int index)
    {
      int index1 = list.Count - 1;
      if (index != index1)
        list[index] = list[index1];
      list.RemoveAt(index1);
    }

    [Obsolete("Due to changes required for XBOX this method is obsolete. Do not use it, as now it simply does list.ToArray")]
    public static T[] GetInternalArray<T>(this List<T> list) => list.ToArray();

    public static void AddOrInsert<T>(this List<T> list, T item, int index)
    {
      if (index < 0 || index > list.Count)
        list.Add(item);
      else
        list.Insert(index, item);
    }

    public static void AddHashsetCasting<T1, T2>(this List<T1> list, HashSet<T2> hashset)
    {
      foreach (T2 obj in hashset)
        list.Add((T1) (object) obj);
    }

    public static void Move<T>(this List<T> list, int originalIndex, int targetIndex)
    {
      int num = Math.Sign(targetIndex - originalIndex);
      if (num == 0)
        return;
      T obj = list[originalIndex];
      for (int index = originalIndex; index != targetIndex; index += num)
        list[index] = list[index + num];
      list[targetIndex] = obj;
    }

    public static bool IsValidIndex<T>(this List<T> list, int index) => 0 <= index && index < list.Count;

    public static void RemoveIndices<T>(this List<T> list, List<int> indices)
    {
      if (indices.Count == 0)
        return;
      int index1 = 0;
      for (int index2 = indices[index1]; index2 < list.Count - indices.Count; ++index2)
      {
        while (index1 < indices.Count && index2 == indices[index1] - index1)
          ++index1;
        list[index2] = list[index2 + index1];
      }
      list.RemoveRange(list.Count - indices.Count, indices.Count);
    }

    public static void Swap<T>(this List<T> list, int a, int b)
    {
      T obj = list[a];
      list[a] = list[b];
      list[b] = obj;
    }

    public static void AddList<T>(this List<T> list, List<T> itemsToAdd)
    {
      if (itemsToAdd.Count == 0)
        return;
      if (list.Capacity < list.Count + itemsToAdd.Count)
        list.Capacity = list.Count + itemsToAdd.Count;
      for (int index = 0; index < itemsToAdd.Count; ++index)
        list.Add(itemsToAdd[index]);
    }

    public static void AddArray<T>(this List<T> list, T[] itemsToAdd) => list.AddArray<T>(itemsToAdd, itemsToAdd.Length);

    public static void AddArray<T>(this List<T> list, T[] itemsToAdd, int itemCount)
    {
      if (list.Capacity < list.Count + itemCount)
        list.Capacity = list.Count + itemCount;
      for (int index = 0; index < itemsToAdd.Length; ++index)
        list.Add(itemsToAdd[index]);
    }

    public static int BinaryIntervalSearch<T>(this IList<T> self, T value, IComparer<T> comparer = null)
    {
      if (self.Count == 0)
        return 0;
      if (comparer == null)
        comparer = (IComparer<T>) Comparer<T>.Default;
      if (self.Count == 1)
        return comparer.Compare(value, self[0]) < 0 ? 0 : 1;
      int index1 = 0;
      int num1 = self.Count;
      while (num1 - index1 > 1)
      {
        int index2 = (index1 + num1) / 2;
        if (comparer.Compare(value, self[index2]) >= 0)
          index1 = index2;
        else
          num1 = index2;
      }
      int num2 = index1;
      if (comparer.Compare(value, self[index1]) >= 0)
        num2 = num1;
      return num2;
    }

    public static int BinaryIntervalSearch<T>(this IList<T> self, Func<T, bool> less)
    {
      if (less == null)
        throw new ArgumentNullException(nameof (less));
      if (self.Count == 0)
        return 0;
      if (self.Count == 1)
        return !less(self[0]) ? 0 : 1;
      int index1 = 0;
      int num1 = self.Count;
      while (num1 - index1 > 1)
      {
        int index2 = (index1 + num1) / 2;
        if (less(self[index2]))
          index1 = index2;
        else
          num1 = index2;
      }
      int num2 = index1;
      if (less(self[index1]))
        num2 = num1;
      return num2;
    }

    public static int BinaryIntervalSearch<T>(
      this IList<T> self,
      T value,
      Comparison<T> comparison)
    {
      if (comparison == null)
        throw new ArgumentNullException(nameof (comparison));
      return self.Count == 0 ? 0 : self.BinaryIntervalSearch<T>(value, (IComparer<T>) ListExtensions.FunctorComparer<T>.Get(comparison));
    }

    public static void InsertInOrder<T>(this List<T> self, T value, IComparer<T> comparer)
    {
      int index = self.BinarySearch(value, comparer);
      if (index < 0)
        index = ~index;
      self.Insert(index, value);
    }

    public static void InsertInOrder<T>(this List<T> self, T value) where T : IComparable<T> => self.InsertInOrder<T>(value, (IComparer<T>) Comparer<T>.Default);

    public static bool IsSorted<T>(this List<T> self, IComparer<T> comparer)
    {
      for (int index = 1; index < self.Count; ++index)
      {
        if (comparer.Compare(self[index - 1], self[index]) > 0)
          return false;
      }
      return true;
    }

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertEmpty<T>(this List<T> list)
    {
      if (list.Count == 0)
        return;
      list.Clear();
    }

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnsureCapacity<T>(this List<T> list, int capacity)
    {
      if (list.Capacity >= capacity)
        return;
      list.Capacity = capacity;
    }

    public static TValue Pop<TValue>(this List<TValue> self)
    {
      TValue obj = self[self.Count - 1];
      self.RemoveAt(self.Count - 1);
      return obj;
    }

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SortNoAlloc<T>(this List<T> list, Comparison<T> comparator) => list.Sort((IComparer<T>) ListExtensions.FunctorComparer<T>.Get(comparator));

    public static T AtMod<T>(this List<T> list, int index) => list[index % list.Count];

    public static T AtMod<T>(this ListReader<T> list, int index) => list[index % list.Count];

    public static T MinBy<T>(this IEnumerable<T> source, Func<T, float> selector) => source.MaxBy<T>((Func<T, float>) (x => -selector(x)));

    public static T MaxBy<T>(this IEnumerable<T> source, Func<T, float> selector)
    {
      T obj = default (T);
      using (IEnumerator<T> enumerator = source.GetEnumerator())
      {
        obj = enumerator.MoveNext() ? enumerator.Current : throw new Exception("No elements");
        float num1 = selector(obj);
        while (enumerator.MoveNext())
        {
          T current = enumerator.Current;
          float num2 = selector(current);
          if ((double) num2 > (double) num1)
          {
            num1 = num2;
            obj = current;
          }
        }
      }
      return obj;
    }

    public static TItem MaxBy<TItem, TKey>(
      this IEnumerable<TItem> source,
      Func<TItem, TKey> selector,
      IComparer<TKey> comparer = null)
      where TKey : IComparable<TKey>
    {
      comparer = comparer ?? (IComparer<TKey>) Comparer<TKey>.Default;
      TItem obj;
      using (IEnumerator<TItem> enumerator = source.GetEnumerator())
      {
        obj = enumerator.MoveNext() ? enumerator.Current : throw new Exception("No elements");
        TKey y = selector(obj);
        while (enumerator.MoveNext())
        {
          TItem current = enumerator.Current;
          TKey x = selector(current);
          if (comparer.Compare(x, y) > 0)
          {
            y = x;
            obj = current;
          }
        }
      }
      return obj;
    }

    public static O[] ToArray<I, O>(this IList<I> collection, Func<I, O> selector)
    {
      int count = collection.Count;
      O[] oArray = new O[collection.Count];
      for (int index = 0; index < count; ++index)
        oArray[index] = selector(collection[index]);
      return oArray;
    }

    public static void ClearAndTrim<T>(this List<T> list, int maxElements)
    {
      list.Clear();
      if (list.Capacity <= maxElements)
        return;
      list.Capacity = maxElements;
    }

    private sealed class FunctorComparer<T> : IComparer<T>
    {
      private Comparison<T> m_comparison;
      [ThreadStatic]
      private static ListExtensions.FunctorComparer<T> m_Instance;

      public int Compare(T x, T y) => this.m_comparison(x, y);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static ListExtensions.FunctorComparer<T> Get(Comparison<T> comparison)
      {
        ListExtensions.FunctorComparer<T> functorComparer = ListExtensions.FunctorComparer<T>.m_Instance;
        if (functorComparer == null)
          ListExtensions.FunctorComparer<T>.m_Instance = functorComparer = new ListExtensions.FunctorComparer<T>();
        functorComparer.m_comparison = comparison;
        return functorComparer;
      }
    }
  }
}
