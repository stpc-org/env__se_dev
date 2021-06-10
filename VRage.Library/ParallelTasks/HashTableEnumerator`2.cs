// Decompiled with JetBrains decompiler
// Type: ParallelTasks.HashTableEnumerator`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace ParallelTasks
{
  public class HashTableEnumerator<TKey, TData> : IEnumerator<KeyValuePair<TKey, TData>>, IEnumerator, IDisposable
  {
    private int currentIndex = -1;
    private Hashtable<TKey, TData> table;

    public HashTableEnumerator(Hashtable<TKey, TData> table) => this.table = table;

    public KeyValuePair<TKey, TData> Current { get; private set; }

    public void Dispose()
    {
    }

    object IEnumerator.Current => (object) this.Current;

    public bool MoveNext()
    {
      HashtableNode<TKey, TData> hashtableNode;
      do
      {
        ++this.currentIndex;
        if (this.table.array.Length <= this.currentIndex)
          return false;
        hashtableNode = this.table.array[this.currentIndex];
      }
      while (hashtableNode.Token != HashtableToken.Used);
      this.Current = new KeyValuePair<TKey, TData>(hashtableNode.Key, hashtableNode.Data);
      return true;
    }

    public void Reset() => this.currentIndex = -1;
  }
}
