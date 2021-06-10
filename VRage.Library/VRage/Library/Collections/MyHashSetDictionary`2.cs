// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.MyHashSetDictionary`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public class MyHashSetDictionary<TKey, TValue> : MyCollectionDictionary<TKey, HashSet<TValue>, TValue>
  {
    private readonly IEqualityComparer<TValue> m_valueComparer;

    public MyHashSetDictionary()
    {
    }

    public MyHashSetDictionary(
      IEqualityComparer<TKey> keyComparer = null,
      IEqualityComparer<TValue> valueComparer = null)
      : base(keyComparer)
    {
      this.m_valueComparer = valueComparer ?? (IEqualityComparer<TValue>) EqualityComparer<TValue>.Default;
    }

    protected override HashSet<TValue> CreateCollection() => new HashSet<TValue>(this.m_valueComparer);
  }
}
