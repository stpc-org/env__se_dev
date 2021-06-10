// Decompiled with JetBrains decompiler
// Type: ParallelTasks.Hashtable`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Library.Threading;

namespace ParallelTasks
{
  public class Hashtable<TKey, TData> : IEnumerable<KeyValuePair<TKey, TData>>, IEnumerable
  {
    private static readonly EqualityComparer<TKey> KeyComparer = EqualityComparer<TKey>.Default;
    public volatile HashtableNode<TKey, TData>[] array;
    private SpinLock writeLock;
    private static readonly HashtableNode<TKey, TData> DeletedNode = new HashtableNode<TKey, TData>(default (TKey), default (TData), HashtableToken.Deleted);

    public Hashtable(int initialCapacity)
    {
      this.array = initialCapacity >= 1 ? new HashtableNode<TKey, TData>[initialCapacity] : throw new ArgumentOutOfRangeException(nameof (initialCapacity), "cannot be < 1");
      this.writeLock = new SpinLock();
    }

    public void Add(TKey key, TData data)
    {
      try
      {
        this.writeLock.Enter();
        if (this.Insert(this.array, key, data))
          return;
        this.Resize();
        this.Insert(this.array, key, data);
      }
      finally
      {
        this.writeLock.Exit();
      }
    }

    private void Resize()
    {
      HashtableNode<TKey, TData>[] table = new HashtableNode<TKey, TData>[this.array.Length * 2];
      for (int index = 0; index < this.array.Length; ++index)
      {
        HashtableNode<TKey, TData> hashtableNode = this.array[index];
        if (hashtableNode.Token == HashtableToken.Used)
          this.Insert(table, hashtableNode.Key, hashtableNode.Data);
      }
      this.array = table;
    }

    private bool Insert(HashtableNode<TKey, TData>[] table, TKey key, TData data)
    {
      int num = Math.Abs(GetHashCode_HashTable<TKey>.GetHashCode(key)) % table.Length;
      int index = num;
      bool flag = false;
      do
      {
        HashtableNode<TKey, TData> hashtableNode = table[index];
        if (hashtableNode.Token == HashtableToken.Empty || hashtableNode.Token == HashtableToken.Deleted || Hashtable<TKey, TData>.KeyComparer.Equals(key, hashtableNode.Key))
        {
          table[index] = new HashtableNode<TKey, TData>()
          {
            Key = key,
            Data = data,
            Token = HashtableToken.Used
          };
          flag = true;
          break;
        }
        index = (index + 1) % table.Length;
      }
      while (index != num);
      return flag;
    }

    public void UnsafeSet(TKey key, TData value)
    {
      bool flag = false;
      HashtableNode<TKey, TData>[] array;
      do
      {
        array = this.array;
        int num = Math.Abs(GetHashCode_HashTable<TKey>.GetHashCode(key)) % array.Length;
        int index = num;
        do
        {
          HashtableNode<TKey, TData> hashtableNode = array[index];
          if (Hashtable<TKey, TData>.KeyComparer.Equals(key, hashtableNode.Key))
          {
            array[index] = new HashtableNode<TKey, TData>()
            {
              Key = key,
              Data = value,
              Token = HashtableToken.Used
            };
            flag = true;
            break;
          }
          index = (index + 1) % array.Length;
        }
        while (index != num);
      }
      while (array != this.array);
      if (flag)
        return;
      this.Add(key, value);
    }

    private bool Find(TKey key, out HashtableNode<TKey, TData> node)
    {
      node = new HashtableNode<TKey, TData>();
      HashtableNode<TKey, TData>[] array = this.array;
      int num = Math.Abs(GetHashCode_HashTable<TKey>.GetHashCode(key)) % array.Length;
      int index = num;
      HashtableNode<TKey, TData> hashtableNode;
      do
      {
        hashtableNode = array[index];
        if (hashtableNode.Token == HashtableToken.Empty)
          return false;
        if (hashtableNode.Token == HashtableToken.Deleted || !Hashtable<TKey, TData>.KeyComparer.Equals(key, hashtableNode.Key))
          index = (index + 1) % array.Length;
        else
          goto label_5;
      }
      while (index != num);
      goto label_6;
label_5:
      node = hashtableNode;
      return true;
label_6:
      return false;
    }

    public bool TryGet(TKey key, out TData data)
    {
      HashtableNode<TKey, TData> node;
      if (this.Find(key, out node))
      {
        data = node.Data;
        return true;
      }
      data = default (TData);
      return false;
    }

    public void Remove(TKey key)
    {
      try
      {
        this.writeLock.Enter();
        HashtableNode<TKey, TData>[] array = this.array;
        int num = Math.Abs(GetHashCode_HashTable<TKey>.GetHashCode(key)) % array.Length;
        int index = num;
        do
        {
          HashtableNode<TKey, TData> hashtableNode = array[index];
          if (hashtableNode.Token != HashtableToken.Empty)
          {
            if (hashtableNode.Token == HashtableToken.Deleted || !Hashtable<TKey, TData>.KeyComparer.Equals(key, hashtableNode.Key))
              index = (index + 1) % array.Length;
            else
              array[index] = Hashtable<TKey, TData>.DeletedNode;
          }
          else
            goto label_7;
        }
        while (index != num);
        goto label_2;
label_7:
        return;
label_2:;
      }
      finally
      {
        this.writeLock.Exit();
      }
    }

    public IEnumerator<KeyValuePair<TKey, TData>> GetEnumerator() => (IEnumerator<KeyValuePair<TKey, TData>>) new HashTableEnumerator<TKey, TData>(this);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
