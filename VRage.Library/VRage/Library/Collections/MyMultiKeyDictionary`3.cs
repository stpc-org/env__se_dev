// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.MyMultiKeyDictionary`3
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public class MyMultiKeyDictionary<TKey1, TKey2, TValue> : IEnumerable<MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple>, IEnumerable
  {
    private Dictionary<TKey1, MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple> m_index1 = new Dictionary<TKey1, MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple>();
    private Dictionary<TKey2, MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple> m_index2 = new Dictionary<TKey2, MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple>();

    public TValue this[TKey1 key] => this.m_index1[key].Value;

    public TValue this[TKey2 key] => this.m_index2[key].Value;

    public int Count => this.m_index1.Count;

    public MyMultiKeyDictionary(
      int capacity = 0,
      EqualityComparer<TKey1> keyComparer1 = null,
      EqualityComparer<TKey2> keyComparer2 = null)
    {
      this.m_index1 = new Dictionary<TKey1, MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple>(capacity, (IEqualityComparer<TKey1>) keyComparer1);
      this.m_index2 = new Dictionary<TKey2, MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple>(capacity, (IEqualityComparer<TKey2>) keyComparer2);
    }

    public void Add(TKey1 key1, TKey2 key2, TValue value)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple triple = new MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple(key1, key2, value);
      this.m_index1.Add(key1, triple);
      try
      {
        this.m_index2.Add(key2, triple);
      }
      catch
      {
        this.m_index1.Remove(key1);
        throw;
      }
    }

    public bool ContainsKey(TKey1 key1) => this.m_index1.ContainsKey(key1);

    public bool ContainsKey(TKey2 key2) => this.m_index2.ContainsKey(key2);

    public bool Remove(TKey1 key1)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple triple;
      return this.m_index1.TryGetValue(key1, out triple) && this.m_index2.Remove(triple.Key2) && this.m_index1.Remove(key1);
    }

    public bool Remove(TKey2 key2)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple triple;
      return this.m_index2.TryGetValue(key2, out triple) && this.m_index1.Remove(triple.Key1) && this.m_index2.Remove(key2);
    }

    public bool Remove(TKey1 key1, TKey2 key2) => this.m_index1.Remove(key1) && this.m_index2.Remove(key2);

    public bool TryRemove(
      TKey1 key1,
      out MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple removedValue)
    {
      return this.m_index1.TryGetValue(key1, out removedValue) && this.m_index2.Remove(removedValue.Key2) && this.m_index1.Remove(key1);
    }

    public bool TryRemove(
      TKey2 key2,
      out MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple removedValue)
    {
      return this.m_index2.TryGetValue(key2, out removedValue) && this.m_index1.Remove(removedValue.Key1) && this.m_index2.Remove(key2);
    }

    public bool TryRemove(TKey1 key1, out TValue removedValue)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple triple;
      bool flag = this.m_index1.TryGetValue(key1, out triple) && this.m_index2.Remove(triple.Key2) && this.m_index1.Remove(key1);
      removedValue = triple.Value;
      return flag;
    }

    public bool TryRemove(TKey2 key2, out TValue removedValue)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple triple;
      bool flag = this.m_index2.TryGetValue(key2, out triple) && this.m_index1.Remove(triple.Key1) && this.m_index2.Remove(key2);
      removedValue = triple.Value;
      return flag;
    }

    public bool TryGetValue(
      TKey1 key1,
      out MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple result)
    {
      return this.m_index1.TryGetValue(key1, out result);
    }

    public bool TryGetValue(
      TKey2 key2,
      out MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple result)
    {
      return this.m_index2.TryGetValue(key2, out result);
    }

    public bool TryGetValue(TKey1 key1, out TValue result)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple triple;
      bool flag = this.m_index1.TryGetValue(key1, out triple);
      result = triple.Value;
      return flag;
    }

    public bool TryGetValue(TKey2 key2, out TValue result)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple triple;
      bool flag = this.m_index2.TryGetValue(key2, out triple);
      result = triple.Value;
      return flag;
    }

    private Dictionary<TKey1, MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple>.ValueCollection.Enumerator GetEnumerator() => this.m_index1.Values.GetEnumerator();

    IEnumerator<MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple> IEnumerable<MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple>.GetEnumerator() => (IEnumerator<MyMultiKeyDictionary<TKey1, TKey2, TValue>.Triple>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public struct Triple
    {
      public TKey1 Key1;
      public TKey2 Key2;
      public TValue Value;

      public Triple(TKey1 key1, TKey2 key2, TValue value)
      {
        this.Key1 = key1;
        this.Key2 = key2;
        this.Value = value;
      }
    }
  }
}
