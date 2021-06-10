// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyVector3ISet
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using VRageMath;

namespace VRage.Utils
{
  public class MyVector3ISet : IEnumerable<Vector3I>, IEnumerable
  {
    private Dictionary<Vector3I, ulong> m_chunks;

    private int Timestamp { get; set; }

    public bool Empty => this.m_chunks.Count == 0;

    public MyVector3ISet()
    {
      this.m_chunks = new Dictionary<Vector3I, ulong>();
      this.Timestamp = 0;
    }

    public bool Contains(ref Vector3I position)
    {
      ulong num = 0;
      return this.m_chunks.TryGetValue(new Vector3I(position.X >> 2, position.Y >> 2, position.Z >> 2), out num) && (num & MyVector3ISet.GetMask(ref position)) > 0UL;
    }

    public bool Contains(Vector3I position) => this.Contains(ref position);

    public void Add(ref Vector3I position)
    {
      Vector3I key = new Vector3I(position.X >> 2, position.Y >> 2, position.Z >> 2);
      ulong num1 = 0;
      this.m_chunks.TryGetValue(key, out num1);
      ulong num2 = num1 | MyVector3ISet.GetMask(ref position);
      this.m_chunks[key] = num2;
      ++this.Timestamp;
    }

    public void Add(Vector3I position) => this.Add(ref position);

    public void Remove(ref Vector3I position)
    {
      Vector3I key = new Vector3I(position.X >> 2, position.Y >> 2, position.Z >> 2);
      ulong num1 = 0;
      this.m_chunks.TryGetValue(key, out num1);
      ulong num2 = num1 & ~MyVector3ISet.GetMask(ref position);
      if (num2 == 0UL)
        this.m_chunks.Remove(key);
      else
        this.m_chunks[key] = num2;
      ++this.Timestamp;
    }

    public void Remove(Vector3I position) => this.Remove(ref position);

    public void Union(MyVector3ISet otherSet)
    {
      foreach (KeyValuePair<Vector3I, ulong> chunk in otherSet.m_chunks)
      {
        ulong num = 0;
        this.m_chunks.TryGetValue(chunk.Key, out num);
        num |= chunk.Value;
        this.m_chunks[chunk.Key] = num;
      }
      ++this.Timestamp;
    }

    public void Clear()
    {
      this.m_chunks.Clear();
      ++this.Timestamp;
    }

    public MyVector3ISet.Enumerator GetEnumerator() => new MyVector3ISet.Enumerator(this);

    private static ulong GetMask(ref Vector3I position) => 1UL << ((position.Z & 3) << 4) + ((position.Y & 3) << 2) + (position.X & 3);

    IEnumerator<Vector3I> IEnumerable<Vector3I>.GetEnumerator() => (IEnumerator<Vector3I>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public struct Enumerator : IEnumerator<Vector3I>, IEnumerator, IDisposable
    {
      private Dictionary<Vector3I, ulong>.Enumerator m_dictEnum;
      private int m_shift;
      private ulong m_currentData;
      private MyVector3ISet m_parent;
      private int m_timestamp;

      public Enumerator(MyVector3ISet set)
      {
        this.m_parent = set;
        this.m_dictEnum = new Dictionary<Vector3I, ulong>.Enumerator();
        this.m_shift = 0;
        this.m_currentData = 0UL;
        this.m_timestamp = 0;
        this.Init();
      }

      private void Init()
      {
        this.m_dictEnum = this.m_parent.m_chunks.GetEnumerator();
        this.m_shift = 63;
        this.m_currentData = 0UL;
        this.m_timestamp = this.m_parent.Timestamp;
      }

      public Vector3I Current => this.m_dictEnum.Current.Key * 4 + new Vector3I(this.m_shift & 3, this.m_shift >> 2 & 3, this.m_shift >> 4);

      public bool MoveNext()
      {
        while (this.MoveNextInternal())
        {
          if (((long) this.m_currentData & 1L << this.m_shift) != 0L)
            return true;
        }
        return false;
      }

      private bool MoveNextInternal()
      {
        if (this.m_shift == 63)
        {
          this.m_shift = 0;
          if (!this.m_dictEnum.MoveNext())
            return false;
          this.m_currentData = this.m_dictEnum.Current.Value;
          return true;
        }
        ++this.m_shift;
        return true;
      }

      public void Dispose()
      {
      }

      object IEnumerator.Current => (object) this.Current;

      public void Reset() => this.Init();

      [Conditional("DEBUG")]
      private void CheckTimestamp()
      {
        if (this.m_timestamp != this.m_parent.Timestamp)
          throw new InvalidOperationException("A Vector3I set collection was modified during iteration using an enumerator!");
      }
    }
  }
}
