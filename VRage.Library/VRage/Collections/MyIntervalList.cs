// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MyIntervalList
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Collections
{
  public class MyIntervalList
  {
    private List<int> m_list;
    private int m_count;

    public int Count => this.m_count;

    public int IntervalCount => this.m_list.Count / 2;

    public MyIntervalList() => this.m_list = new List<int>(8);

    private MyIntervalList(int capacity) => this.m_list = new List<int>(capacity);

    public override string ToString()
    {
      string str = "";
      for (int index = 0; index < this.m_list.Count; index += 2)
      {
        if (index != 0)
          str += "; ";
        str = str + "<" + (object) this.m_list[index] + "," + (object) this.m_list[index + 1] + ">";
      }
      return str;
    }

    public int IndexOf(int value)
    {
      int num = 0;
      for (int index = 0; index < this.m_list.Count && value >= this.m_list[index]; index += 2)
      {
        if (value <= this.m_list[index + 1])
          return num + value - this.m_list[index];
        num += this.m_list[index + 1] - this.m_list[index] + 1;
      }
      return -1;
    }

    public int this[int index]
    {
      get
      {
        int num1 = index >= 0 && index < this.m_count ? index : throw new IndexOutOfRangeException("Index " + (object) index + " is out of range in MyIntervalList. Valid indices are in range <0, Count)");
        for (int index1 = 0; index1 < this.m_list.Count; index1 += 2)
        {
          int num2 = this.m_list[index1 + 1] - this.m_list[index1] + 1;
          if (num1 < num2)
            return this.m_list[index1] + num1;
          num1 -= num2;
        }
        return 0;
      }
    }

    public void Add(int value)
    {
      switch (value)
      {
        case int.MinValue:
          if (this.m_list.Count == 0)
          {
            this.InsertInterval(0, value, value);
            break;
          }
          if (this.m_list[0] == -2147483647)
          {
            this.ExtendIntervalDown(0);
            break;
          }
          if (this.m_list[0] == int.MinValue)
            break;
          this.InsertInterval(0, value, value);
          break;
        case int.MaxValue:
          int i = this.m_list.Count - 2;
          if (i < 0)
          {
            this.InsertInterval(0, value, value);
            break;
          }
          if (this.m_list[i + 1] == 2147483646)
          {
            this.ExtendIntervalUp(i);
            break;
          }
          if (this.m_list[i + 1] == int.MaxValue)
            break;
          this.InsertInterval(this.m_list.Count, value, value);
          break;
        default:
          for (int index = 0; index < this.m_list.Count; index += 2)
          {
            if (value + 1 < this.m_list[index])
            {
              this.InsertInterval(index, value, value);
              return;
            }
            if (value - 1 <= this.m_list[index + 1])
            {
              if (value + 1 == this.m_list[index])
              {
                this.ExtendIntervalDown(index);
                return;
              }
              if (value - 1 != this.m_list[index + 1])
                return;
              this.ExtendIntervalUp(index);
              return;
            }
          }
          this.InsertInterval(this.m_list.Count, value, value);
          break;
      }
    }

    public void Clear()
    {
      this.m_list.Clear();
      this.m_count = 0;
    }

    public MyIntervalList GetCopy()
    {
      MyIntervalList myIntervalList = new MyIntervalList(this.m_list.Count);
      for (int index = 0; index < this.m_list.Count; ++index)
        myIntervalList.m_list.Add(this.m_list[index]);
      myIntervalList.m_count = this.m_count;
      return myIntervalList;
    }

    public bool Contains(int value)
    {
      for (int index = 0; index < this.m_list.Count && value >= this.m_list[index]; index += 2)
      {
        if (value <= this.m_list[index + 1])
          return true;
      }
      return false;
    }

    public MyIntervalList.Enumerator GetEnumerator() => new MyIntervalList.Enumerator(this);

    private void InsertInterval(int listPosition, int min, int max)
    {
      if (listPosition == this.m_list.Count)
      {
        this.m_list.Add(min);
        this.m_list.Add(max);
        this.m_count += max - min + 1;
      }
      else
      {
        int index = this.m_list.Count - 2;
        this.m_list.Add(this.m_list[index]);
        this.m_list.Add(this.m_list[index + 1]);
        for (; index > listPosition; index -= 2)
        {
          this.m_list[index] = this.m_list[index - 2];
          this.m_list[index + 1] = this.m_list[index - 1];
        }
        this.m_list[index] = min;
        this.m_list[index + 1] = max;
        this.m_count += max - min + 1;
      }
    }

    private void ExtendIntervalDown(int i)
    {
      this.m_list[i]--;
      ++this.m_count;
      if (i == 0)
        return;
      this.TryMergeIntervals(i - 1, i);
    }

    private void ExtendIntervalUp(int i)
    {
      this.m_list[i + 1]++;
      ++this.m_count;
      if (i >= this.m_list.Count - 2)
        return;
      this.TryMergeIntervals(i + 1, i + 2);
    }

    private void TryMergeIntervals(int i1, int i2)
    {
      if (this.m_list[i1] + 1 != this.m_list[i2])
        return;
      for (int index = i1; index < this.m_list.Count - 2; ++index)
        this.m_list[index] = this.m_list[index + 2];
      this.m_list.RemoveAt(this.m_list.Count - 1);
      this.m_list.RemoveAt(this.m_list.Count - 1);
    }

    public struct Enumerator
    {
      private int m_interval;
      private int m_dist;
      private int m_lowerBound;
      private int m_upperBound;
      private MyIntervalList m_parent;

      public Enumerator(MyIntervalList parent)
      {
        this.m_interval = -1;
        this.m_dist = 0;
        this.m_lowerBound = 0;
        this.m_upperBound = 0;
        this.m_parent = parent;
      }

      public int Current => this.m_lowerBound + this.m_dist;

      public bool MoveNext()
      {
        if (this.m_interval == -1 || this.m_lowerBound + this.m_dist >= this.m_upperBound)
          return this.MoveNextInterval();
        ++this.m_dist;
        return true;
      }

      private bool MoveNextInterval()
      {
        ++this.m_interval;
        if (this.m_interval >= this.m_parent.IntervalCount)
          return false;
        this.m_dist = 0;
        this.m_lowerBound = this.m_parent.m_list[this.m_interval * 2];
        this.m_upperBound = this.m_parent.m_list[this.m_interval * 2 + 1];
        return true;
      }
    }
  }
}
