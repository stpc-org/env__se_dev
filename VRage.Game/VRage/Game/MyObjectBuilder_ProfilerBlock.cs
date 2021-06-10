// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ProfilerBlock
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Profiler;

namespace VRage.Game
{
  public class MyObjectBuilder_ProfilerBlock
  {
    public int Id;
    public MyProfilerBlockKey Key;
    public string TimeFormat;
    public string ValueFormat;
    public string CallFormat;
    public CompactSerializedArray<float>? ProcessMemory;
    public CompactSerializedArray<long>? ManagedMemoryBytes;
    public CompactSerializedArray<float>? Allocations;
    public CompactSerializedArray<float>? Milliseconds;
    public CompactSerializedArray<float>? CustomValues;
    public CompactSerializedArray<int>? NumCallsArray;
    public List<MyProfilerBlockKey> Children;
    public MyProfilerBlockKey Parent;

    public static MyObjectBuilder_ProfilerBlock GetObjectBuilder(
      MyProfilerBlock profilerBlock,
      bool serializeAllocations)
    {
      MyProfilerBlock.MyProfilerBlockObjectBuilderInfo objectBuilderInfo = profilerBlock.GetObjectBuilderInfo(serializeAllocations);
      MyObjectBuilder_ProfilerBlock builderProfilerBlock = new MyObjectBuilder_ProfilerBlock();
      builderProfilerBlock.Id = objectBuilderInfo.Id;
      builderProfilerBlock.Key = objectBuilderInfo.Key;
      builderProfilerBlock.TimeFormat = objectBuilderInfo.TimeFormat;
      builderProfilerBlock.ValueFormat = objectBuilderInfo.ValueFormat;
      builderProfilerBlock.CallFormat = objectBuilderInfo.CallFormat;
      builderProfilerBlock.Allocations = new CompactSerializedArray<float>?((CompactSerializedArray<float>) ref objectBuilderInfo.Allocations);
      builderProfilerBlock.Milliseconds = new CompactSerializedArray<float>?((CompactSerializedArray<float>) ref objectBuilderInfo.Milliseconds);
      builderProfilerBlock.CustomValues = new CompactSerializedArray<float>?((CompactSerializedArray<float>) ref objectBuilderInfo.CustomValues);
      builderProfilerBlock.NumCallsArray = new CompactSerializedArray<int>?((CompactSerializedArray<int>) ref objectBuilderInfo.NumCallsArray);
      builderProfilerBlock.Children = new List<MyProfilerBlockKey>();
      foreach (MyProfilerBlock child in objectBuilderInfo.Children)
        builderProfilerBlock.Children.Add(child.Key);
      if (objectBuilderInfo.Parent != null)
        builderProfilerBlock.Parent = objectBuilderInfo.Parent.Key;
      return builderProfilerBlock;
    }

    public static MyProfilerBlock Init(
      MyObjectBuilder_ProfilerBlock objectBuilder,
      MyProfiler.MyProfilerObjectBuilderInfo profiler)
    {
      MyProfilerBlock.MyProfilerBlockObjectBuilderInfo data = new MyProfilerBlock.MyProfilerBlockObjectBuilderInfo();
      data.Id = objectBuilder.Id;
      data.Key = objectBuilder.Key;
      data.TimeFormat = objectBuilder.TimeFormat;
      data.ValueFormat = objectBuilder.ValueFormat;
      data.CallFormat = objectBuilder.CallFormat;
      MyProfilerBlock.MyProfilerBlockObjectBuilderInfo objectBuilderInfo1 = data;
      CompactSerializedArray<float>? allocations = objectBuilder.Allocations;
      CompactSerializedArray<float> valueOrDefault1;
      float[] numArray1;
      if (!allocations.HasValue)
      {
        numArray1 = (float[]) null;
      }
      else
      {
        valueOrDefault1 = allocations.GetValueOrDefault();
        numArray1 = (float[]) ref valueOrDefault1;
      }
      objectBuilderInfo1.Allocations = numArray1;
      MyProfilerBlock.MyProfilerBlockObjectBuilderInfo objectBuilderInfo2 = data;
      CompactSerializedArray<float>? nullable = objectBuilder.Milliseconds;
      float[] numArray2;
      if (!nullable.HasValue)
      {
        numArray2 = (float[]) null;
      }
      else
      {
        valueOrDefault1 = nullable.GetValueOrDefault();
        numArray2 = (float[]) ref valueOrDefault1;
      }
      objectBuilderInfo2.Milliseconds = numArray2;
      MyProfilerBlock.MyProfilerBlockObjectBuilderInfo objectBuilderInfo3 = data;
      nullable = objectBuilder.CustomValues;
      float[] numArray3;
      if (!nullable.HasValue)
      {
        numArray3 = (float[]) null;
      }
      else
      {
        valueOrDefault1 = nullable.GetValueOrDefault();
        numArray3 = (float[]) ref valueOrDefault1;
      }
      objectBuilderInfo3.CustomValues = numArray3;
      MyProfilerBlock.MyProfilerBlockObjectBuilderInfo objectBuilderInfo4 = data;
      CompactSerializedArray<int>? numCallsArray = objectBuilder.NumCallsArray;
      int[] numArray4;
      if (!numCallsArray.HasValue)
      {
        numArray4 = (int[]) null;
      }
      else
      {
        CompactSerializedArray<int> valueOrDefault2 = numCallsArray.GetValueOrDefault();
        numArray4 = (int[]) ref valueOrDefault2;
      }
      objectBuilderInfo4.NumCallsArray = numArray4;
      data.Children = new List<MyProfilerBlock>();
      foreach (MyProfilerBlockKey child in objectBuilder.Children)
        data.Children.Add(profiler.ProfilingBlocks[child]);
      if (objectBuilder.Parent.File != null)
        data.Parent = profiler.ProfilingBlocks[objectBuilder.Parent];
      MyProfilerBlock profilingBlock = profiler.ProfilingBlocks[data.Key];
      profilingBlock.Init(data);
      return profilingBlock;
    }
  }
}
