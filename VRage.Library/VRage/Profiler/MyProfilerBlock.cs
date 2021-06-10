// Decompiled with JetBrains decompiler
// Type: VRage.Profiler.MyProfilerBlock
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRage.Library.Utils;

namespace VRage.Profiler
{
  public class MyProfilerBlock
  {
    public static Func<ulong> GetThreadAllocationStamp;
    public int NumCalls;
    public int Allocated;
    public float CustomValue;
    public MyTimeSpan Elapsed;
    public int ForceOrder;
    public string TimeFormat;
    public string ValueFormat;
    public string CallFormat;
    private bool m_isOptimized;
    public MyProfilerBlock Parent;
    public List<MyProfilerBlock> Children = new List<MyProfilerBlock>();
    public float AverageMilliseconds;
    public MyProfilerBlock.OptimizableDataCache RawAllocations;
    public MyProfilerBlock.OptimizableDataCache RawMilliseconds;
    public int[] NumCallsArray = new int[MyProfiler.MAX_FRAMES];
    public float[] CustomValues = new float[MyProfiler.MAX_FRAMES];
    private int m_beginThreadId;
    private ulong m_beginAllocationStamp;
    private long m_measureStartTimestamp;
    public MyProfilerBlock.BlockTypes BlockType;

    public int Id { get; private set; }

    public MyProfilerBlockKey Key { get; private set; }

    public string Name => this.Key.Name;

    public bool IsOptimized
    {
      get => this.m_isOptimized;
      set
      {
        if (this.m_isOptimized == value)
          return;
        this.m_isOptimized = value;
        MyProfilerBlock.ForceInvalidateSelfAndParentsOptimizationsRecursive(this);
      }
    }

    public bool IsDeepTreeRoot { get; private set; }

    public MyProfilerBlock()
    {
      this.RawMilliseconds = (MyProfilerBlock.OptimizableDataCache) new MyProfilerBlock.TimeCache(this);
      this.RawAllocations = (MyProfilerBlock.OptimizableDataCache) new MyProfilerBlock.AllocationCache(this);
    }

    public MyProfilerBlock.DataReader GetAllocationsReader(bool useOptimizations) => new MyProfilerBlock.DataReader(this.RawAllocations, useOptimizations);

    public MyProfilerBlock.DataReader GetMillisecondsReader(bool useOptimizations) => new MyProfilerBlock.DataReader(this.RawMilliseconds, useOptimizations);

    public void SetBlockData(
      ref MyProfilerBlockKey key,
      int blockId,
      int forceOrder = 2147483647,
      bool isDeepTreeRoot = true)
    {
      this.Id = blockId;
      this.Key = key;
      this.ForceOrder = forceOrder;
      this.IsDeepTreeRoot = isDeepTreeRoot;
    }

    public void Reset()
    {
      this.m_measureStartTimestamp = Stopwatch.GetTimestamp();
      this.Elapsed = MyTimeSpan.Zero;
      this.Allocated = 0;
    }

    public void Clear()
    {
      this.Reset();
      this.NumCalls = 0;
      this.Allocated = 0;
      this.CustomValue = 0.0f;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Start(bool memoryProfiling)
    {
      ++this.NumCalls;
      this.m_measureStartTimestamp = Stopwatch.GetTimestamp();
      if (!memoryProfiling)
        return;
      this.m_beginAllocationStamp = MyProfilerBlock.GetThreadAllocationStamp();
      this.m_beginThreadId = Thread.CurrentThread.ManagedThreadId;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void End(bool memoryProfiling, MyTimeSpan? customTime = null, int numCalls = 0)
    {
      if (memoryProfiling)
        this.Allocated += (int) ((long) MyProfilerBlock.GetThreadAllocationStamp() - (long) this.m_beginAllocationStamp);
      MyTimeSpan myTimeSpan = !customTime.HasValue ? MyTimeSpan.FromTicks(Stopwatch.GetTimestamp() - this.m_measureStartTimestamp) : customTime.Value;
      this.NumCalls += numCalls;
      this.Elapsed += myTimeSpan;
    }

    private static void ForceInvalidateSelfAndParentsOptimizationsRecursive(MyProfilerBlock block)
    {
      for (; block != null; block = block.Parent)
      {
        for (int frame = 0; frame < MyProfiler.MAX_FRAMES; ++frame)
        {
          block.RawAllocations.InvalidateFrameOptimizations(frame);
          block.RawMilliseconds.InvalidateFrameOptimizations(frame);
        }
      }
    }

    public override string ToString() => this.Key.Name + " (" + (object) this.NumCalls + " calls)";

    internal void Dump(StringBuilder sb, int frame)
    {
      if ((double) this.NumCallsArray[frame] < 0.01)
        return;
      sb.Append(string.Format("<Block Name=\"{0}\">\n", (object) this.Name));
      sb.Append(string.Format("<Time>{0}</Time>\n<Calls>{1}</Calls>\n", (object) this.RawMilliseconds[frame], (object) this.NumCallsArray[frame]));
      foreach (MyProfilerBlock child in this.Children)
        child.Dump(sb, frame);
      sb.Append("</Block>\n");
    }

    public MyProfilerBlock.MyProfilerBlockObjectBuilderInfo GetObjectBuilderInfo(
      bool serializeAllocations)
    {
      return new MyProfilerBlock.MyProfilerBlockObjectBuilderInfo()
      {
        Id = this.Id,
        Key = this.Key,
        Parent = this.Parent,
        Children = this.Children,
        CallFormat = this.CallFormat,
        TimeFormat = this.TimeFormat,
        ValueFormat = this.ValueFormat,
        CustomValues = this.CustomValues,
        NumCallsArray = this.NumCallsArray,
        Milliseconds = this.RawMilliseconds.RawData,
        Allocations = serializeAllocations ? this.RawAllocations.RawData : (float[]) null
      };
    }

    public void Init(
      MyProfilerBlock.MyProfilerBlockObjectBuilderInfo data)
    {
      this.Id = data.Id;
      this.Key = data.Key;
      this.CallFormat = data.CallFormat;
      this.TimeFormat = data.TimeFormat;
      this.ValueFormat = data.ValueFormat;
      this.CustomValues = data.CustomValues;
      this.NumCallsArray = data.NumCallsArray;
      this.Parent = data.Parent;
      this.Children = data.Children;
      this.RawMilliseconds = (MyProfilerBlock.OptimizableDataCache) new MyProfilerBlock.TimeCache(this, data.Milliseconds);
      this.RawAllocations = (MyProfilerBlock.OptimizableDataCache) new MyProfilerBlock.AllocationCache(this, data.Allocations);
    }

    private void Init(
      MyProfilerBlock.MyProfilerBlockObjectBuilderInfo data,
      int id,
      MyProfilerBlock parent)
    {
      this.Init(data);
      this.Children = new List<MyProfilerBlock>();
      this.CustomValues = (float[]) data.CustomValues.Clone();
      this.NumCallsArray = (int[]) data.NumCallsArray.Clone();
      this.RawMilliseconds = (MyProfilerBlock.OptimizableDataCache) new MyProfilerBlock.TimeCache(this, (float[]) data.Milliseconds.Clone());
      this.RawAllocations = (MyProfilerBlock.OptimizableDataCache) new MyProfilerBlock.AllocationCache(this, (float[]) data.Allocations.Clone());
      this.Id = id;
      this.BlockType = MyProfilerBlock.BlockTypes.Added;
      if (parent == null)
        return;
      this.Parent = parent;
      MyProfilerBlockKey key = this.Key;
      key.ParentId = parent.Id;
      this.Key = key;
      parent.Children.Add(this);
    }

    public void SubtractFrom(MyProfilerBlock otherBlock)
    {
      this.NumCalls = otherBlock.NumCalls - this.NumCalls;
      this.Allocated = otherBlock.Allocated - this.Allocated;
      this.CustomValue = otherBlock.CustomValue - this.CustomValue;
      this.Elapsed = otherBlock.Elapsed - this.Elapsed;
      this.AverageMilliseconds = otherBlock.AverageMilliseconds - this.AverageMilliseconds;
      for (int frame = 0; frame < MyProfiler.MAX_FRAMES; ++frame)
      {
        this.RawAllocations[frame] = otherBlock.RawAllocations[frame] - this.RawAllocations[frame];
        this.RawMilliseconds[frame] = otherBlock.RawMilliseconds[frame] - this.RawMilliseconds[frame];
        this.NumCallsArray[frame] = otherBlock.NumCallsArray[frame] - this.NumCallsArray[frame];
        this.CustomValues[frame] = otherBlock.CustomValues[frame] - this.CustomValues[frame];
      }
      this.BlockType = MyProfilerBlock.BlockTypes.Diffed;
    }

    public void Invert()
    {
      this.Allocated = -this.Allocated;
      this.Elapsed -= this.Elapsed;
      this.AverageMilliseconds -= this.AverageMilliseconds;
      for (int frame = 0; frame < MyProfiler.MAX_FRAMES; ++frame)
      {
        this.RawAllocations[frame] = -this.RawAllocations[frame];
        this.RawMilliseconds[frame] = -this.RawMilliseconds[frame];
      }
      this.BlockType = MyProfilerBlock.BlockTypes.Inverted;
    }

    public MyProfilerBlock Duplicate(int id, MyProfilerBlock parent)
    {
      MyProfilerBlock.MyProfilerBlockObjectBuilderInfo objectBuilderInfo = this.GetObjectBuilderInfo(true);
      MyProfilerBlock myProfilerBlock = new MyProfilerBlock();
      myProfilerBlock.Init(objectBuilderInfo, id, parent);
      return myProfilerBlock;
    }

    public enum BlockTypes
    {
      Normal,
      Diffed,
      Inverted,
      Added,
    }

    public class MyProfilerBlockObjectBuilderInfo
    {
      public int Id;
      public MyProfilerBlockKey Key;
      public string TimeFormat;
      public string ValueFormat;
      public string CallFormat;
      public int[] NumCallsArray;
      public float[] Allocations;
      public float[] Milliseconds;
      public float[] CustomValues;
      public MyProfilerBlock Parent;
      public List<MyProfilerBlock> Children;
    }

    public struct DataReader
    {
      private readonly bool m_useOptimizations;
      private readonly MyProfilerBlock.OptimizableDataCache Data;

      public DataReader(MyProfilerBlock.OptimizableDataCache data, bool useOptimizations)
      {
        this.Data = data;
        this.m_useOptimizations = useOptimizations;
      }

      public float this[int frame]
      {
        get
        {
          float num = this.Data[frame];
          if (this.m_useOptimizations)
            num -= this.Data.GetOptimizedCutout(frame);
          return num;
        }
      }
    }

    public abstract class OptimizableDataCache
    {
      public readonly float[] RawData;
      private readonly bool[] m_valid;
      private readonly float[] m_optimizedCutout;
      private readonly MyProfilerBlock m_block;

      protected OptimizableDataCache(MyProfilerBlock mBlock, float[] data = null)
      {
        this.m_block = mBlock;
        this.m_valid = new bool[MyProfiler.MAX_FRAMES];
        this.RawData = data ?? new float[MyProfiler.MAX_FRAMES];
        this.m_optimizedCutout = new float[MyProfiler.MAX_FRAMES];
      }

      public float this[int frame]
      {
        get => this.RawData[frame];
        set
        {
          this.RawData[frame] = value;
          this.InvalidateFrameOptimizations(frame);
        }
      }

      public void InvalidateFrameOptimizations(int frame) => this.m_valid[frame] = false;

      public float GetOptimizedCutout(int frame)
      {
        if (!this.m_valid[frame])
        {
          float num = 0.0f;
          if (this.m_block.IsOptimized)
          {
            num = this.RawData[frame];
          }
          else
          {
            foreach (MyProfilerBlock child in this.m_block.Children)
              num += this.GetBlockData(child).GetOptimizedCutout(frame);
          }
          this.m_valid[frame] = true;
          this.m_optimizedCutout[frame] = num;
        }
        return this.m_optimizedCutout[frame];
      }

      protected abstract MyProfilerBlock.OptimizableDataCache GetBlockData(
        MyProfilerBlock block);
    }

    private sealed class AllocationCache : MyProfilerBlock.OptimizableDataCache
    {
      public AllocationCache(MyProfilerBlock block, float[] data = null)
        : base(block, data)
      {
      }

      protected override MyProfilerBlock.OptimizableDataCache GetBlockData(
        MyProfilerBlock block)
      {
        return block.RawAllocations;
      }
    }

    private sealed class TimeCache : MyProfilerBlock.OptimizableDataCache
    {
      public TimeCache(MyProfilerBlock block, float[] data = null)
        : base(block, data)
      {
      }

      protected override MyProfilerBlock.OptimizableDataCache GetBlockData(
        MyProfilerBlock block)
      {
        return block.RawMilliseconds;
      }
    }
  }
}
