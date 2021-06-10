// Decompiled with JetBrains decompiler
// Type: System.Linq.LinqExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;

namespace System.Linq
{
  public static class LinqExtensions
  {
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
      foreach (T obj in source)
        action(obj);
    }

    public static void Deconstruct<K, V>(this KeyValuePair<K, V> pair, out K k, out V v)
    {
      k = pair.Key;
      v = pair.Value;
    }
  }
}
