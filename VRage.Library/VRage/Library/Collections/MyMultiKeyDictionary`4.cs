// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.MyMultiKeyDictionary`4
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public class MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue> : IEnumerable<MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple>, IEnumerable
  {
    private Dictionary<TKey1, MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple> m_index1 = new Dictionary<TKey1, MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple>();
    private Dictionary<TKey2, MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple> m_index2 = new Dictionary<TKey2, MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple>();
    private Dictionary<TKey3, MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple> m_index3 = new Dictionary<TKey3, MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple>();

    public TValue this[TKey1 key] => this.m_index1[key].Value;

    public TValue this[TKey2 key] => this.m_index2[key].Value;

    public TValue this[TKey3 key] => this.m_index3[key].Value;

    public int Count => this.m_index1.Count;

    public MyMultiKeyDictionary(
      int capacity = 0,
      EqualityComparer<TKey1> keyComparer1 = null,
      EqualityComparer<TKey2> keyComparer2 = null,
      EqualityComparer<TKey3> keyComparer3 = null)
    {
      this.m_index1 = new Dictionary<TKey1, MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple>(capacity, (IEqualityComparer<TKey1>) keyComparer1);
      this.m_index2 = new Dictionary<TKey2, MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple>(capacity, (IEqualityComparer<TKey2>) keyComparer2);
      this.m_index3 = new Dictionary<TKey3, MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple>(capacity, (IEqualityComparer<TKey3>) keyComparer3);
    }

    public void Add(TKey1 key1, TKey2 key2, TKey3 key3, TValue value)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple quadruple = new MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple(key1, key2, key3, value);
      this.m_index1.Add(key1, quadruple);
      try
      {
        this.m_index2.Add(key2, quadruple);
        try
        {
          this.m_index3.Add(key3, quadruple);
        }
        catch
        {
          this.m_index2.Remove(key2);
          throw;
        }
      }
      catch
      {
        this.m_index1.Remove(key1);
        throw;
      }
    }

    public bool ContainsKey(TKey1 key1) => this.m_index1.ContainsKey(key1);

    public bool ContainsKey(TKey2 key2) => this.m_index2.ContainsKey(key2);

    public bool ContainsKey(TKey3 key3) => this.m_index3.ContainsKey(key3);

    public bool Remove(TKey1 key1)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple quadruple;
      return this.m_index1.TryGetValue(key1, out quadruple) && this.m_index3.Remove(quadruple.Key3) && this.m_index2.Remove(quadruple.Key2) && this.m_index1.Remove(key1);
    }

    public bool Remove(TKey2 key2)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple quadruple;
      return this.m_index2.TryGetValue(key2, out quadruple) && this.m_index3.Remove(quadruple.Key3) && this.m_index1.Remove(quadruple.Key1) && this.m_index2.Remove(key2);
    }

    public bool Remove(TKey3 key3)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple quadruple;
      return this.m_index3.TryGetValue(key3, out quadruple) && this.m_index1.Remove(quadruple.Key1) && this.m_index2.Remove(quadruple.Key2) && this.m_index3.Remove(key3);
    }

    public bool Remove(TKey1 key1, TKey2 key2, TKey3 key3) => this.m_index1.Remove(key1) && this.m_index2.Remove(key2) && this.m_index3.Remove(key3);

    public bool TryRemove(TKey1 key1, out TValue removedValue)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple quadruple;
      bool flag = this.m_index1.TryGetValue(key1, out quadruple) && this.m_index3.Remove(quadruple.Key3) && this.m_index2.Remove(quadruple.Key2) && this.m_index1.Remove(key1);
      removedValue = quadruple.Value;
      return flag;
    }

    public bool TryRemove(TKey2 key2, out TValue removedValue)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple quadruple;
      bool flag = this.m_index2.TryGetValue(key2, out quadruple) && this.m_index3.Remove(quadruple.Key3) && this.m_index1.Remove(quadruple.Key1) && this.m_index2.Remove(key2);
      removedValue = quadruple.Value;
      return flag;
    }

    public bool TryRemove(TKey3 key3, out TValue removedValue)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple quadruple;
      bool flag = this.m_index3.TryGetValue(key3, out quadruple) && this.m_index1.Remove(quadruple.Key1) && this.m_index2.Remove(quadruple.Key2) && this.m_index3.Remove(key3);
      removedValue = quadruple.Value;
      return flag;
    }

    public bool TryRemove(
      TKey1 key1,
      out MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple removedValue)
    {
      return this.m_index1.TryGetValue(key1, out removedValue) && this.m_index3.Remove(removedValue.Key3) && this.m_index2.Remove(removedValue.Key2) && this.m_index1.Remove(key1);
    }

    public bool TryRemove(
      TKey2 key2,
      out MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple removedValue)
    {
      return this.m_index2.TryGetValue(key2, out removedValue) && this.m_index3.Remove(removedValue.Key3) && this.m_index1.Remove(removedValue.Key1) && this.m_index2.Remove(key2);
    }

    public bool TryRemove(
      TKey3 key3,
      out MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple removedValue)
    {
      return this.m_index3.TryGetValue(key3, out removedValue) && this.m_index1.Remove(removedValue.Key1) && this.m_index2.Remove(removedValue.Key2) && this.m_index3.Remove(key3);
    }

    public bool TryGetValue(
      TKey1 key1,
      out MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple result)
    {
      return this.m_index1.TryGetValue(key1, out result);
    }

    public bool TryGetValue(
      TKey2 key2,
      out MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple result)
    {
      return this.m_index2.TryGetValue(key2, out result);
    }

    public bool TryGetValue(
      TKey3 key3,
      out MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple result)
    {
      return this.m_index3.TryGetValue(key3, out result);
    }

    public bool TryGetValue(TKey1 key1, out TValue result)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple quadruple;
      bool flag = this.m_index1.TryGetValue(key1, out quadruple);
      result = quadruple.Value;
      return flag;
    }

    public bool TryGetValue(TKey2 key2, out TValue result)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple quadruple;
      bool flag = this.m_index2.TryGetValue(key2, out quadruple);
      result = quadruple.Value;
      return flag;
    }

    public bool TryGetValue(TKey3 key3, out TValue result)
    {
      MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple quadruple;
      bool flag = this.m_index3.TryGetValue(key3, out quadruple);
      result = quadruple.Value;
      return flag;
    }

    private Dictionary<TKey1, MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple>.ValueCollection.Enumerator GetEnumerator() => this.m_index1.Values.GetEnumerator();

    IEnumerator<MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple> IEnumerable<MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple>.GetEnumerator() => (IEnumerator<MyMultiKeyDictionary<TKey1, TKey2, TKey3, TValue>.Quadruple>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public struct Quadruple
    {
      public TKey1 Key1;
      public TKey2 Key2;
      public TKey3 Key3;
      public TValue Value;

      public Quadruple(TKey1 key1, TKey2 key2, TKey3 key3, TValue value)
      {
        this.Key1 = key1;
        this.Key2 = key2;
        this.Key3 = key3;
        this.Value = value;
      }
    }
  }
}
