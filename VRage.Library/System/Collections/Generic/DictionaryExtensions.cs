// Decompiled with JetBrains decompiler
// Type: System.Collections.Generic.DictionaryExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
  public static class DictionaryExtensions
  {
    public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dictionary, K key)
    {
      V v;
      dictionary.TryGetValue(key, out v);
      return v;
    }

    public static V GetValueOrDefault<K, V>(
      this Dictionary<K, V> dictionary,
      K key,
      V defaultValue)
    {
      V v;
      return !dictionary.TryGetValue(key, out v) ? defaultValue : v;
    }

    public static KeyValuePair<K, V> FirstPair<K, V>(this Dictionary<K, V> dictionary)
    {
      Dictionary<K, V>.Enumerator enumerator = dictionary.GetEnumerator();
      enumerator.MoveNext();
      return enumerator.Current;
    }

    public static V GetValueOrDefault<K, V>(
      this ConcurrentDictionary<K, V> dictionary,
      K key,
      V defaultValue)
    {
      V v;
      return !dictionary.TryGetValue(key, out v) ? defaultValue : v;
    }

    public static void Remove<K, V>(this ConcurrentDictionary<K, V> dictionary, K key) => dictionary.TryRemove(key, out V _);

    public static TValue GetOrAdd<TKey, TValue, TContext>(
      this ConcurrentDictionary<TKey, TValue> dictionary,
      TKey key,
      TContext context,
      Func<TContext, TKey, TValue> activator)
    {
      TValue orAdd;
      if (!dictionary.TryGetValue(key, out orAdd))
        orAdd = dictionary.GetOrAdd(key, activator(context, key));
      return orAdd;
    }

    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AssertEmpty<K, V>(this Dictionary<K, V> collection)
    {
      if (collection.Count == 0)
        return;
      collection.Clear();
    }
  }
}
