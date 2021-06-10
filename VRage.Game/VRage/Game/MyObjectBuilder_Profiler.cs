// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Profiler
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Profiler;
using VRage.Utils;

namespace VRage.Game
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Profiler : MyObjectBuilder_Base
  {
    public List<MyObjectBuilder_ProfilerBlock> ProfilingBlocks;
    public List<MyProfilerBlockKey> RootBlocks;
    public List<MyProfiler.TaskInfo> Tasks;
    public CompactSerializedArray<int> TotalCalls;
    public CompactSerializedArray<long> CommitTimes;
    public string CustomName = "";
    public string AxisName = "";
    public bool ShallowProfile;

    public static MyObjectBuilder_Profiler GetObjectBuilder(
      MyProfiler profiler)
    {
      MyProfiler.MyProfilerObjectBuilderInfo objectBuilderInfo = profiler.GetObjectBuilderInfo();
      MyObjectBuilder_Profiler objectBuilderProfiler = new MyObjectBuilder_Profiler();
      objectBuilderProfiler.ProfilingBlocks = new List<MyObjectBuilder_ProfilerBlock>();
      foreach (KeyValuePair<MyProfilerBlockKey, MyProfilerBlock> profilingBlock in objectBuilderInfo.ProfilingBlocks)
        objectBuilderProfiler.ProfilingBlocks.Add(MyObjectBuilder_ProfilerBlock.GetObjectBuilder(profilingBlock.Value, profiler.AllocationProfiling));
      objectBuilderProfiler.RootBlocks = new List<MyProfilerBlockKey>();
      foreach (MyProfilerBlock rootBlock in objectBuilderInfo.RootBlocks)
        objectBuilderProfiler.RootBlocks.Add(rootBlock.Key);
      objectBuilderProfiler.Tasks = objectBuilderInfo.Tasks;
      objectBuilderProfiler.TotalCalls = (CompactSerializedArray<int>) ref objectBuilderInfo.TotalCalls;
      objectBuilderProfiler.CustomName = objectBuilderInfo.CustomName;
      objectBuilderProfiler.AxisName = objectBuilderInfo.AxisName;
      objectBuilderProfiler.ShallowProfile = objectBuilderInfo.ShallowProfile;
      objectBuilderProfiler.CommitTimes = (CompactSerializedArray<long>) ref objectBuilderInfo.CommitTimes;
      return objectBuilderProfiler;
    }

    public static MyProfiler Init(MyObjectBuilder_Profiler objectBuilder)
    {
      MyProfiler.MyProfilerObjectBuilderInfo objectBuilderInfo = new MyProfiler.MyProfilerObjectBuilderInfo();
      objectBuilderInfo.ProfilingBlocks = new Dictionary<MyProfilerBlockKey, MyProfilerBlock>();
      foreach (MyObjectBuilder_ProfilerBlock profilingBlock in objectBuilder.ProfilingBlocks)
        objectBuilderInfo.ProfilingBlocks.Add(profilingBlock.Key, new MyProfilerBlock());
      foreach (MyObjectBuilder_ProfilerBlock profilingBlock in objectBuilder.ProfilingBlocks)
        MyObjectBuilder_ProfilerBlock.Init(profilingBlock, objectBuilderInfo);
      objectBuilderInfo.RootBlocks = new List<MyProfilerBlock>();
      foreach (MyProfilerBlockKey rootBlock in objectBuilder.RootBlocks)
        objectBuilderInfo.RootBlocks.Add(objectBuilderInfo.ProfilingBlocks[rootBlock]);
      objectBuilderInfo.TotalCalls = (int[]) ref objectBuilder.TotalCalls;
      objectBuilderInfo.CustomName = objectBuilder.CustomName;
      objectBuilderInfo.AxisName = objectBuilder.AxisName;
      objectBuilderInfo.ShallowProfile = objectBuilder.ShallowProfile;
      objectBuilderInfo.Tasks = objectBuilder.Tasks;
      objectBuilderInfo.CommitTimes = (long[]) ref objectBuilder.CommitTimes;
      MyProfiler myProfiler = new MyProfiler(false, objectBuilderInfo.CustomName, objectBuilderInfo.AxisName, false, 1000, -1);
      myProfiler.Init(objectBuilderInfo);
      return myProfiler;
    }

    public static void SaveToFile(int index)
    {
      try
      {
        MyObjectBuilder_Profiler objectBuilder = MyObjectBuilder_Profiler.GetObjectBuilder(MyRenderProfiler.SelectedProfiler);
        MyObjectBuilderSerializer.SerializeXML(Path.Combine(MyFileSystem.UserDataPath, "Profiler-" + (object) index), true, (MyObjectBuilder_Base) objectBuilder);
      }
      catch
      {
      }
    }

    public static void LoadFromFile(int index)
    {
      try
      {
        MyObjectBuilder_Profiler objectBuilder;
        MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Profiler>(Path.Combine(MyFileSystem.UserDataPath, "Profiler-" + (object) index), out objectBuilder);
        MyRenderProfiler.SelectedProfiler = MyObjectBuilder_Profiler.Init(objectBuilder);
      }
      catch
      {
      }
    }

    protected class VRage_Game_MyObjectBuilder_Profiler\u003C\u003EProfilingBlocks\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, List<MyObjectBuilder_ProfilerBlock>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Profiler owner,
        in List<MyObjectBuilder_ProfilerBlock> value)
      {
        owner.ProfilingBlocks = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Profiler owner,
        out List<MyObjectBuilder_ProfilerBlock> value)
      {
        value = owner.ProfilingBlocks;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Profiler\u003C\u003ERootBlocks\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, List<MyProfilerBlockKey>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Profiler owner, in List<MyProfilerBlockKey> value) => owner.RootBlocks = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Profiler owner,
        out List<MyProfilerBlockKey> value)
      {
        value = owner.RootBlocks;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Profiler\u003C\u003ETasks\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, List<MyProfiler.TaskInfo>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Profiler owner,
        in List<MyProfiler.TaskInfo> value)
      {
        owner.Tasks = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Profiler owner,
        out List<MyProfiler.TaskInfo> value)
      {
        value = owner.Tasks;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Profiler\u003C\u003ETotalCalls\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, CompactSerializedArray<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Profiler owner,
        in CompactSerializedArray<int> value)
      {
        owner.TotalCalls = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Profiler owner,
        out CompactSerializedArray<int> value)
      {
        value = owner.TotalCalls;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Profiler\u003C\u003ECommitTimes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, CompactSerializedArray<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Profiler owner,
        in CompactSerializedArray<long> value)
      {
        owner.CommitTimes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Profiler owner,
        out CompactSerializedArray<long> value)
      {
        value = owner.CommitTimes;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Profiler\u003C\u003ECustomName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Profiler owner, in string value) => owner.CustomName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Profiler owner, out string value) => value = owner.CustomName;
    }

    protected class VRage_Game_MyObjectBuilder_Profiler\u003C\u003EAxisName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Profiler owner, in string value) => owner.AxisName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Profiler owner, out string value) => value = owner.AxisName;
    }

    protected class VRage_Game_MyObjectBuilder_Profiler\u003C\u003EShallowProfile\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Profiler, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Profiler owner, in bool value) => owner.ShallowProfile = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Profiler owner, out bool value) => value = owner.ShallowProfile;
    }

    protected class VRage_Game_MyObjectBuilder_Profiler\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Profiler, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Profiler owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Profiler owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Profiler\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Profiler, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Profiler owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Profiler owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Profiler\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Profiler, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Profiler owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Profiler owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Profiler\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Profiler, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Profiler owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Profiler owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Profiler\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Profiler>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Profiler();

      MyObjectBuilder_Profiler IActivator<MyObjectBuilder_Profiler>.CreateInstance() => new MyObjectBuilder_Profiler();
    }
  }
}
