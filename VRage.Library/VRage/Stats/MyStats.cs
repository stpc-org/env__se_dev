// Decompiled with JetBrains decompiler
// Type: VRage.Stats.MyStats
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRage.Library.Utils;

namespace VRage.Stats
{
  public class MyStats
  {
    public volatile MyStats.SortEnum Sort = MyStats.SortEnum.Priority;
    private static Comparer<KeyValuePair<string, MyStat>> m_nameComparer = (Comparer<KeyValuePair<string, MyStat>>) new MyNameComparer();
    private static Comparer<KeyValuePair<string, MyStat>> m_priorityComparer = (Comparer<KeyValuePair<string, MyStat>>) new MyPriorityComparer();
    private MyGameTimer m_timer = new MyGameTimer();
    private NumberFormatInfo m_format = new NumberFormatInfo()
    {
      NumberDecimalSeparator = ".",
      NumberGroupSeparator = " "
    };
    private FastResourceLock m_lock = new FastResourceLock();
    private Dictionary<string, MyStat> m_stats = new Dictionary<string, MyStat>(1024);
    private List<KeyValuePair<string, MyStat>> m_tmpWriteList = new List<KeyValuePair<string, MyStat>>(1024);

    private MyStat GetStat(string name)
    {
      MyStat myStat;
      using (this.m_lock.AcquireSharedUsing())
      {
        if (this.m_stats.TryGetValue(name, out myStat))
          return myStat;
      }
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (this.m_stats.TryGetValue(name, out myStat))
          return myStat;
        myStat = new MyStat(0);
        this.m_stats[name] = myStat;
        return myStat;
      }
    }

    private MyStat GetStat(MyStatKeys.StatKeysEnum key)
    {
      string name;
      int priority;
      MyStatKeys.GetNameAndPriority(key, out name, out priority);
      MyStat myStat;
      using (this.m_lock.AcquireSharedUsing())
      {
        if (this.m_stats.TryGetValue(name, out myStat))
          return myStat;
      }
      using (this.m_lock.AcquireExclusiveUsing())
      {
        if (this.m_stats.TryGetValue(name, out myStat))
          return myStat;
        myStat = new MyStat(priority);
        this.m_stats[name] = myStat;
        return myStat;
      }
    }

    public void Clear()
    {
      using (this.m_lock.AcquireSharedUsing())
      {
        foreach (KeyValuePair<string, MyStat> stat in this.m_stats)
          stat.Value.Clear();
      }
    }

    public void RemoveAll()
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_stats.Clear();
    }

    public void Remove(string name)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_stats.Remove(name);
    }

    public void Clear(string name) => this.GetStat(name).Clear();

    public void Increment(string name, int refreshMs = 0, int clearRateMs = -1) => this.Write(name, 0L, MyStatTypeEnum.Counter, refreshMs, 0, clearRateMs);

    public void Increment(MyStatKeys.StatKeysEnum key, int refreshMs = 0, int clearRateMs = -1) => this.Write(key, 0.0f, MyStatTypeEnum.Counter, refreshMs, 0, clearRateMs);

    public MyStatToken Measure(
      string name,
      MyStatTypeEnum type,
      int refreshMs = 200,
      int numDecimals = 1,
      int clearRateMs = -1)
    {
      MyStat stat = this.GetStat(name);
      if (stat.DrawText == null)
        stat.DrawText = this.GetMeasureText(name, type);
      stat.ChangeSettings((type | MyStatTypeEnum.FormatFlag) & (MyStatTypeEnum.Sum | MyStatTypeEnum.Counter | MyStatTypeEnum.DontDisappearFlag | MyStatTypeEnum.KeepInactiveLongerFlag | MyStatTypeEnum.FormatFlag), refreshMs, numDecimals, clearRateMs);
      return new MyStatToken(this.m_timer, stat);
    }

    public MyStatToken Measure(string name) => this.Measure(name, MyStatTypeEnum.Avg);

    private string GetMeasureText(string name, MyStatTypeEnum type)
    {
      switch (type & (MyStatTypeEnum.Sum | MyStatTypeEnum.Counter))
      {
        case MyStatTypeEnum.MinMax:
          return name + ": {0}ms / {1}ms";
        case MyStatTypeEnum.MinMaxAvg:
          return name + ": {0}ms / {1}ms / {2}ms";
        case MyStatTypeEnum.Counter:
          return name + ": {0}x";
        case MyStatTypeEnum.CounterSum:
          return name + ": {0}x / {1}ms";
        default:
          return name + ": {0}ms";
      }
    }

    public void Write(
      string name,
      float value,
      MyStatTypeEnum type,
      int refreshMs,
      int numDecimals,
      int clearRateMs = -1)
    {
      this.GetStat(name).Write(value, type, refreshMs, numDecimals, clearRateMs);
    }

    public void Write(
      MyStatKeys.StatKeysEnum key,
      float value,
      MyStatTypeEnum type,
      int refreshMs,
      int numDecimals,
      int clearRateMs = -1)
    {
      this.GetStat(key).Write(value, type, refreshMs, numDecimals, clearRateMs);
    }

    public void Write(
      string name,
      long value,
      MyStatTypeEnum type,
      int refreshMs,
      int numDecimals,
      int clearRateMs = -1)
    {
      this.GetStat(name).Write(value, type, refreshMs, numDecimals, clearRateMs);
    }

    public void WriteFormat(
      string name,
      float value,
      MyStatTypeEnum type,
      int refreshMs,
      int numDecimals,
      int clearRateMs = -1)
    {
      this.GetStat(name).Write(value, type | MyStatTypeEnum.FormatFlag, refreshMs, numDecimals, clearRateMs);
    }

    public void WriteFormat(
      string name,
      float value1,
      float value2,
      MyStatTypeEnum type,
      int refreshMs,
      int numDecimals,
      int clearRateMs = -1)
    {
      this.GetStat(name).Write(value1, type | MyStatTypeEnum.FormatFlag, refreshMs, numDecimals, clearRateMs, value2);
    }

    public void WriteFormat(
      MyStatKeys.StatKeysEnum key,
      float value,
      MyStatTypeEnum type,
      int refreshMs,
      int numDecimals,
      int clearRateMs = -1)
    {
      this.GetStat(key).Write(value, type | MyStatTypeEnum.FormatFlag, refreshMs, numDecimals, clearRateMs);
    }

    public void WriteFormat(
      MyStatKeys.StatKeysEnum key,
      float value1,
      float value2,
      MyStatTypeEnum type,
      int refreshMs,
      int numDecimals,
      int clearRateMs = -1)
    {
      this.GetStat(key).Write(value1, type | MyStatTypeEnum.FormatFlag, refreshMs, numDecimals, clearRateMs, value2);
    }

    public void WriteFormat(
      string name,
      long value,
      MyStatTypeEnum type,
      int refreshMs,
      int numDecimals,
      int clearRateMs = -1)
    {
      this.GetStat(name).Write(value, type | MyStatTypeEnum.FormatFlag, refreshMs, numDecimals, clearRateMs);
    }

    public void WriteTo(StringBuilder writeTo)
    {
      lock (this.m_tmpWriteList)
      {
        try
        {
          using (this.m_lock.AcquireSharedUsing())
          {
            foreach (KeyValuePair<string, MyStat> stat in this.m_stats)
              this.m_tmpWriteList.Add(stat);
          }
          switch (this.Sort)
          {
            case MyStats.SortEnum.Name:
              this.m_tmpWriteList.Sort((IComparer<KeyValuePair<string, MyStat>>) MyStats.m_nameComparer);
              break;
            case MyStats.SortEnum.Priority:
              this.m_tmpWriteList.Sort((IComparer<KeyValuePair<string, MyStat>>) MyStats.m_priorityComparer);
              break;
          }
          foreach (KeyValuePair<string, MyStat> tmpWrite in this.m_tmpWriteList)
            this.AppendStat(writeTo, tmpWrite.Key, tmpWrite.Value);
        }
        finally
        {
          this.m_tmpWriteList.Clear();
        }
      }
    }

    private void AppendStatLine<A, B, C, D>(
      StringBuilder text,
      string statName,
      A arg0,
      B arg1,
      C arg2,
      D arg3,
      NumberFormatInfo format,
      string formatString)
      where A : IConvertible
      where B : IConvertible
      where C : IConvertible
      where D : IConvertible
    {
      if (formatString == null)
        text.ConcatFormat<A, B, C, D>(statName, arg0, arg1, arg2, arg3, format);
      else
        text.ConcatFormat<string, A, B, C, D>(formatString, statName, arg0, arg1, arg2, arg3, format);
      text.AppendLine();
    }

    private MyTimeSpan RequiredInactivity(MyStatTypeEnum type)
    {
      if ((type & MyStatTypeEnum.DontDisappearFlag) == MyStatTypeEnum.DontDisappearFlag)
        return MyTimeSpan.MaxValue;
      return (type & MyStatTypeEnum.KeepInactiveLongerFlag) == MyStatTypeEnum.KeepInactiveLongerFlag ? MyTimeSpan.FromSeconds(30.0) : MyTimeSpan.FromSeconds(3.0);
    }

    private void AppendStat(StringBuilder text, string statKey, MyStat stat)
    {
      MyStat.Value sum;
      int count;
      MyStat.Value min;
      MyStat.Value max;
      MyStat.Value last;
      MyStatTypeEnum type;
      int decimals;
      MyTimeSpan inactivityMs;
      MyStat.Value last2;
      stat.ReadAndClear(this.m_timer.Elapsed, out sum, out count, out min, out max, out last, out type, out decimals, out inactivityMs, out last2);
      if (inactivityMs > this.RequiredInactivity(type))
      {
        this.Remove(statKey);
      }
      else
      {
        string statName = stat.DrawText ?? statKey;
        bool flag1 = (type & MyStatTypeEnum.LongFlag) == MyStatTypeEnum.LongFlag;
        float num = (flag1 ? (float) sum.AsLong : sum.AsFloat) / (float) count;
        this.m_format.NumberDecimalDigits = decimals;
        this.m_format.NumberGroupSeparator = decimals == 0 ? "," : string.Empty;
        bool flag2 = (type & MyStatTypeEnum.FormatFlag) == MyStatTypeEnum.FormatFlag;
        switch (type & (MyStatTypeEnum.Sum | MyStatTypeEnum.Counter))
        {
          case MyStatTypeEnum.CurrentValue:
            if (flag1)
            {
              this.AppendStatLine<long, int, int, long>(text, statName, last.AsLong, 0, 0, last2.AsLong, this.m_format, flag2 ? (string) null : "{0}: {1}");
              break;
            }
            this.AppendStatLine<float, int, int, float>(text, statName, last.AsFloat, 0, 0, last2.AsFloat, this.m_format, flag2 ? (string) null : "{0}: {1}");
            break;
          case MyStatTypeEnum.Min:
            if (flag1)
            {
              this.AppendStatLine<long, int, int, int>(text, statName, min.AsLong, 0, 0, 0, this.m_format, flag2 ? (string) null : "{0}: {1}");
              break;
            }
            this.AppendStatLine<float, int, int, int>(text, statName, min.AsFloat, 0, 0, 0, this.m_format, flag2 ? (string) null : "{0}: {1}");
            break;
          case MyStatTypeEnum.Max:
            if (flag1)
            {
              this.AppendStatLine<long, int, int, int>(text, statName, max.AsLong, 0, 0, 0, this.m_format, flag2 ? (string) null : "{0}: {1}");
              break;
            }
            this.AppendStatLine<float, int, int, int>(text, statName, max.AsFloat, 0, 0, 0, this.m_format, flag2 ? (string) null : "{0}: {1}");
            break;
          case MyStatTypeEnum.Avg:
            this.AppendStatLine<float, int, int, int>(text, statName, num, 0, 0, 0, this.m_format, flag2 ? (string) null : "{0}: {1}");
            break;
          case MyStatTypeEnum.MinMax:
            if (flag1)
            {
              this.AppendStatLine<long, long, int, int>(text, statName, min.AsLong, max.AsLong, 0, 0, this.m_format, flag2 ? (string) null : "{0}: {1} / {2}");
              break;
            }
            this.AppendStatLine<float, float, int, int>(text, statName, min.AsFloat, max.AsFloat, 0, 0, this.m_format, flag2 ? (string) null : "{0}: {1} / {2}");
            break;
          case MyStatTypeEnum.MinMaxAvg:
            if (flag1)
            {
              this.AppendStatLine<long, long, float, int>(text, statName, min.AsLong, max.AsLong, num, 0, this.m_format, flag2 ? (string) null : "{0}: {1} / {2} / {3}");
              break;
            }
            this.AppendStatLine<float, float, float, int>(text, statName, min.AsFloat, max.AsFloat, num, 0, this.m_format, flag2 ? (string) null : "{0}: {1} / {2} / {3}");
            break;
          case MyStatTypeEnum.Sum:
            if (flag1)
            {
              this.AppendStatLine<long, int, int, int>(text, statName, sum.AsLong, 0, 0, 0, this.m_format, flag2 ? (string) null : "{0}: {1}");
              break;
            }
            this.AppendStatLine<float, int, int, int>(text, statName, sum.AsFloat, 0, 0, 0, this.m_format, flag2 ? (string) null : "{0}: {1}");
            break;
          case MyStatTypeEnum.Counter:
            this.AppendStatLine<int, int, int, int>(text, statName, count, 0, 0, 0, this.m_format, flag2 ? (string) null : "{0}: {1}");
            break;
          case MyStatTypeEnum.CounterSum:
            if (flag1)
            {
              this.AppendStatLine<int, long, int, int>(text, statName, count, sum.AsLong, 0, 0, this.m_format, flag2 ? (string) null : "{0}: {1} / {2}");
              break;
            }
            this.AppendStatLine<int, float, int, int>(text, statName, count, sum.AsFloat, 0, 0, this.m_format, flag2 ? (string) null : "{0}: {1} / {2}");
            break;
        }
      }
    }

    public enum SortEnum
    {
      None,
      Name,
      Priority,
    }
  }
}
