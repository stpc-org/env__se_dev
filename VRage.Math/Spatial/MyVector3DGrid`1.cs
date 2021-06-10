// Decompiled with JetBrains decompiler
// Type: VRageMath.Spatial.MyVector3DGrid`1
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Collections;

namespace VRageMath.Spatial
{
  public class MyVector3DGrid<T>
  {
    private double m_cellSize;
    private double m_divisor;
    private int m_nextFreeEntry;
    private int m_count;
    private MyList<MyVector3DGrid<T>.Entry> m_storage;
    private Dictionary<Vector3I, int> m_bins;

    public int Count => this.m_count;

    public MyVector3DGrid(double cellSize)
    {
      this.m_cellSize = cellSize;
      this.m_divisor = 1.0 / this.m_cellSize;
      this.m_storage = new MyList<MyVector3DGrid<T>.Entry>();
      this.m_bins = new Dictionary<Vector3I, int>();
      this.Clear();
    }

    public void Clear()
    {
      this.m_nextFreeEntry = 0;
      this.m_count = 0;
      this.m_storage.Clear();
      this.m_bins.Clear();
    }

    public void ClearFast()
    {
      this.m_nextFreeEntry = 0;
      this.m_count = 0;
      this.m_storage.ClearFast();
      this.m_bins.Clear();
    }

    public void AddPoint(ref Vector3D point, T data)
    {
      Vector3I key = Vector3I.Floor(point * this.m_divisor);
      int index;
      if (!this.m_bins.TryGetValue(key, out index))
      {
        int num = this.AddNewEntry(ref point, data);
        this.m_bins.Add(key, num);
      }
      else
      {
        MyVector3DGrid<T>.Entry entry = this.m_storage[index];
        for (int nextEntry = entry.NextEntry; nextEntry != -1; nextEntry = entry.NextEntry)
        {
          index = nextEntry;
          entry = this.m_storage[index];
        }
        int num = this.AddNewEntry(ref point, data);
        entry.NextEntry = num;
        this.m_storage[index] = entry;
      }
    }

    public void RemovePoint(ref Vector3D point)
    {
      Vector3I key = Vector3I.Floor(point * this.m_divisor);
      int num1;
      if (!this.m_bins.TryGetValue(key, out num1))
        return;
      int index = -1;
      int num2 = num1;
      MyVector3DGrid<T>.Entry entry1 = new MyVector3DGrid<T>.Entry();
      while (num1 != -1)
      {
        MyVector3DGrid<T>.Entry entry2 = this.m_storage[num1];
        if (entry2.Point == point)
        {
          int num3 = this.RemoveEntry(num1);
          if (num2 == num1)
          {
            num2 = num3;
          }
          else
          {
            entry1.NextEntry = num3;
            this.m_storage[index] = entry1;
          }
          num1 = num3;
        }
        else
        {
          index = num1;
          entry1 = entry2;
          num1 = entry2.NextEntry;
        }
      }
      if (num2 == -1)
        this.m_bins.Remove(key);
      else
        this.m_bins[key] = num2;
    }

    public MyVector3DGrid<T>.Enumerator GetPointsCloserThan(
      ref Vector3D point,
      double dist)
    {
      return new MyVector3DGrid<T>.Enumerator(this, ref point, dist);
    }

    public void RemoveTwo(
      ref MyVector3DGrid<T>.Enumerator en0,
      ref MyVector3DGrid<T>.Enumerator en1)
    {
      if (en0.CurrentBin == en1.CurrentBin)
      {
        if (en0.StorageIndex == en1.PreviousIndex)
        {
          en1.RemoveCurrent();
          en0.RemoveCurrent();
          en1 = en0;
        }
        else if (en1.StorageIndex == en0.PreviousIndex)
        {
          en0.RemoveCurrent();
          en1.RemoveCurrent();
          en0 = en1;
        }
        else if (en0.StorageIndex == en1.StorageIndex)
        {
          en0.RemoveCurrent();
          en1 = en0;
        }
        else
        {
          en0.RemoveCurrent();
          en1.RemoveCurrent();
        }
      }
      else
      {
        en0.RemoveCurrent();
        en1.RemoveCurrent();
      }
    }

    public Dictionary<Vector3I, int>.Enumerator EnumerateBins() => this.m_bins.GetEnumerator();

    public void GetLocalBinBB(ref Vector3I binPosition, out BoundingBoxD output)
    {
      output.Min = binPosition * this.m_cellSize;
      output.Max = output.Min + new Vector3D(this.m_cellSize);
    }

    public void CollectStorage(int startingIndex, ref List<T> output)
    {
      output.Clear();
      MyVector3DGrid<T>.Entry entry = this.m_storage[startingIndex];
      output.Add(entry.Data);
      while (entry.NextEntry != -1)
      {
        entry = this.m_storage[entry.NextEntry];
        output.Add(entry.Data);
      }
    }

    private int AddNewEntry(ref Vector3D point, T data)
    {
      ++this.m_count;
      if (this.m_nextFreeEntry == this.m_storage.Count)
      {
        this.m_storage.Add(new MyVector3DGrid<T>.Entry()
        {
          Point = point,
          Data = data,
          NextEntry = -1
        });
        return this.m_nextFreeEntry++;
      }
      if ((long) (uint) this.m_nextFreeEntry > (long) this.m_storage.Count)
        return -1;
      int nextFreeEntry = this.m_nextFreeEntry;
      this.m_nextFreeEntry = this.m_storage[this.m_nextFreeEntry].NextEntry;
      this.m_storage[nextFreeEntry] = new MyVector3DGrid<T>.Entry()
      {
        Point = point,
        Data = data,
        NextEntry = -1
      };
      return nextFreeEntry;
    }

    private int RemoveEntry(int toRemove)
    {
      --this.m_count;
      MyVector3DGrid<T>.Entry entry = this.m_storage[toRemove];
      int nextEntry = entry.NextEntry;
      entry.NextEntry = this.m_nextFreeEntry;
      entry.Data = default (T);
      this.m_nextFreeEntry = toRemove;
      this.m_storage[toRemove] = entry;
      return nextEntry;
    }

    [Conditional("DEBUG")]
    private void CheckIndexIsValid(int index)
    {
      int index1 = this.m_nextFreeEntry;
      while (index1 != -1 && index1 != this.m_storage.Count)
        index1 = this.m_storage[index1].NextEntry;
    }

    private struct Entry
    {
      public Vector3D Point;
      public T Data;
      public int NextEntry;

      public override string ToString() => this.Point.ToString() + ", -> " + this.NextEntry.ToString() + ", Data: " + this.Data.ToString();
    }

    public struct Enumerator
    {
      private MyVector3DGrid<T> m_parent;
      private Vector3D m_point;
      private double m_distSq;
      private int m_previousIndex;
      private int m_storageIndex;
      private Vector3I_RangeIterator m_rangeIterator;

      public Enumerator(MyVector3DGrid<T> parent, ref Vector3D point, double dist)
      {
        this.m_parent = parent;
        this.m_point = point;
        this.m_distSq = dist * dist;
        Vector3D vector3D = new Vector3D(dist);
        Vector3I start = Vector3I.Floor((point - vector3D) * parent.m_divisor);
        Vector3I end = Vector3I.Floor((point + vector3D) * parent.m_divisor);
        this.m_rangeIterator = new Vector3I_RangeIterator(ref start, ref end);
        this.m_previousIndex = -1;
        this.m_storageIndex = -1;
      }

      public T Current => this.m_parent.m_storage[this.m_storageIndex].Data;

      public Vector3I CurrentBin => this.m_rangeIterator.Current;

      public int PreviousIndex => this.m_previousIndex;

      public int StorageIndex => this.m_storageIndex;

      public bool RemoveCurrent()
      {
        this.m_storageIndex = this.m_parent.RemoveEntry(this.m_storageIndex);
        if (this.m_previousIndex == -1)
        {
          if (this.m_storageIndex == -1)
            this.m_parent.m_bins.Remove(this.m_rangeIterator.Current);
          else
            this.m_parent.m_bins[this.m_rangeIterator.Current] = this.m_storageIndex;
        }
        else
        {
          MyVector3DGrid<T>.Entry entry = this.m_parent.m_storage[this.m_previousIndex];
          entry.NextEntry = this.m_storageIndex;
          this.m_parent.m_storage[this.m_previousIndex] = entry;
        }
        return this.FindFirstAcceptableEntry();
      }

      public bool MoveNext()
      {
        if (this.m_storageIndex == -1)
        {
          if (!this.FindNextNonemptyBin())
            return false;
        }
        else
        {
          this.m_previousIndex = this.m_storageIndex;
          this.m_storageIndex = this.m_parent.m_storage[this.m_storageIndex].NextEntry;
        }
        return this.FindFirstAcceptableEntry();
      }

      private bool FindFirstAcceptableEntry()
      {
        do
        {
          MyVector3DGrid<T>.Entry entry;
          for (; this.m_storageIndex != -1; this.m_storageIndex = entry.NextEntry)
          {
            entry = this.m_parent.m_storage[this.m_storageIndex];
            if ((entry.Point - this.m_point).LengthSquared() < this.m_distSq)
              return true;
            this.m_previousIndex = this.m_storageIndex;
          }
          this.m_rangeIterator.MoveNext();
        }
        while (this.FindNextNonemptyBin());
        return false;
      }

      private bool FindNextNonemptyBin()
      {
        this.m_previousIndex = -1;
        if (!this.m_rangeIterator.IsValid())
          return false;
        Vector3I next = this.m_rangeIterator.Current;
        while (!this.m_parent.m_bins.TryGetValue(next, out this.m_storageIndex))
        {
          this.m_rangeIterator.GetNext(out next);
          if (!this.m_rangeIterator.IsValid())
            return false;
        }
        return true;
      }
    }
  }
}
