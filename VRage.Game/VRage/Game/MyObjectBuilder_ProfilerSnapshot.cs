// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ProfilerSnapshot
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Profiler;
using VRage.Utils;
using VRageRender;

namespace VRage.Game
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ProfilerSnapshot : MyObjectBuilder_Base
  {
    public List<MyObjectBuilder_Profiler> Profilers;
    public List<MyRenderProfiler.FrameInfo> SimulationFrames;

    public static MyObjectBuilder_ProfilerSnapshot GetObjectBuilder(
      MyRenderProfiler profiler)
    {
      MyObjectBuilder_ProfilerSnapshot profilerSnapshot = new MyObjectBuilder_ProfilerSnapshot();
      List<MyProfiler> threadProfilers = MyRenderProfiler.ThreadProfilers;
      lock (threadProfilers)
      {
        profilerSnapshot.Profilers = new List<MyObjectBuilder_Profiler>(threadProfilers.Count);
        profilerSnapshot.Profilers.AddRange(threadProfilers.Select<MyProfiler, MyObjectBuilder_Profiler>(new Func<MyProfiler, MyObjectBuilder_Profiler>(MyObjectBuilder_Profiler.GetObjectBuilder)));
      }
      profilerSnapshot.SimulationFrames = MyRenderProfiler.FrameTimestamps.ToList<MyRenderProfiler.FrameInfo>();
      return profilerSnapshot;
    }

    public void Init(MyRenderProfiler profiler, SnapshotType type, bool subtract)
    {
      List<MyProfiler> list = this.Profilers.Select<MyObjectBuilder_Profiler, MyProfiler>(new Func<MyObjectBuilder_Profiler, MyProfiler>(MyObjectBuilder_Profiler.Init)).ToList<MyProfiler>();
      ConcurrentQueue<MyRenderProfiler.FrameInfo> frameTimestamps = new ConcurrentQueue<MyRenderProfiler.FrameInfo>((IEnumerable<MyRenderProfiler.FrameInfo>) this.SimulationFrames);
      if (subtract)
        MyRenderProfiler.SubtractOnlineSnapshot(type, list, frameTimestamps);
      else
        MyRenderProfiler.PushOnlineSnapshot(type, list, frameTimestamps);
      MyRenderProfiler.SelectedProfiler = list.Count > 0 ? list[0] : (MyProfiler) null;
    }

    private static void SaveToFile(int index)
    {
      try
      {
        MyObjectBuilder_ProfilerSnapshot.SaveCurrentViewToCsv(index);
        MyObjectBuilder_ProfilerSnapshot profilerBuilder = MyObjectBuilder_ProfilerSnapshot.GetObjectBuilder(MyRenderProxy.GetRenderProfiler());
        new Thread((ThreadStart) (() =>
        {
          MyObjectBuilderSerializer.SerializeXML(MyObjectBuilder_ProfilerSnapshot.GetProfilerDumpPath(index), true, (MyObjectBuilder_Base) profilerBuilder);
          Action profilerSnapshotSaved = MyRenderProfiler.OnProfilerSnapshotSaved;
          if (profilerSnapshotSaved == null)
            return;
          MyRenderProxy.EnqueueMainThreadCallback(profilerSnapshotSaved);
        })).Start();
      }
      catch (Exception ex)
      {
      }
    }

    private static void SaveCurrentViewToCsv(int index)
    {
      string str1 = Path.Combine(MyFileSystem.UserDataPath, "Profiler-" + (object) index);
      MyProfiler selectedProfiler = MyRenderProfiler.SelectedProfiler;
      int lastValidFrame;
      using (selectedProfiler.LockHistory(out lastValidFrame))
      {
        int frameToSortBy = (lastValidFrame + 1) % MyProfiler.MAX_FRAMES;
        string str2 = "-" + (selectedProfiler.SelectedRoot?.Name ?? selectedProfiler.DisplayName);
        List<MyProfilerBlock> sortedChildren = MyRenderProfiler.GetSortedChildren(frameToSortBy);
        using (StreamWriter streamWriter1 = new StreamWriter(str1 + str2 + "-time.csv"))
        {
          using (StreamWriter streamWriter2 = new StreamWriter(str1 + str2 + "-custom.csv"))
          {
            using (StreamWriter streamWriter3 = new StreamWriter(str1 + str2 + "-calls.csv"))
            {
              string str3 = string.Join(", ", sortedChildren.Select<MyProfilerBlock, string>((Func<MyProfilerBlock, string>) (x => x.Name)));
              streamWriter1.WriteLine(str3);
              streamWriter2.WriteLine(str3);
              streamWriter3.WriteLine(str3);
              int frame = (frameToSortBy + 1) % MyProfiler.MAX_FRAMES;
              while (frame != frameToSortBy)
              {
                for (int index1 = 0; index1 < sortedChildren.Count; ++index1)
                {
                  if (index1 > 0)
                  {
                    streamWriter1.Write(", ");
                    streamWriter2.Write(", ");
                    streamWriter3.Write(",");
                  }
                  MyProfilerBlock myProfilerBlock = sortedChildren[index1];
                  streamWriter1.Write(myProfilerBlock.GetMillisecondsReader(false)[frame]);
                  streamWriter2.Write(myProfilerBlock.CustomValues[frame]);
                  streamWriter3.Write(myProfilerBlock.NumCallsArray[frame]);
                }
                streamWriter1.WriteLine();
                streamWriter2.WriteLine();
                streamWriter3.WriteLine();
                ++frame;
                if (frame >= MyProfiler.MAX_FRAMES)
                  frame = 0;
              }
            }
          }
        }
      }
    }

    private static void LoadFromFile(int index, bool subtract)
    {
      try
      {
        MyObjectBuilder_ProfilerSnapshot objectBuilder;
        MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_ProfilerSnapshot>(MyObjectBuilder_ProfilerSnapshot.GetProfilerDumpPath(index), out objectBuilder);
        objectBuilder.Init(MyRenderProxy.GetRenderProfiler(), SnapshotType.Snapshot, subtract);
      }
      catch
      {
      }
    }

    public static string GetProfilerDumpPath(int index) => Path.Combine(MyFileSystem.UserDataPath, "FullProfiler-" + (object) index);

    public static int GetProfilerDumpCount() => Directory.EnumerateFiles(MyFileSystem.UserDataPath, "FullProfiler-*").Count<string>();

    public static void ClearProfilerDumps()
    {
      foreach (string path in Directory.EnumerateFiles(MyFileSystem.UserDataPath, "FullProfiler-*").ToArray<string>())
        File.Delete(path);
    }

    public static void SetDelegates()
    {
      MyRenderProfiler.SaveProfilerToFile = new Action<int>(MyObjectBuilder_ProfilerSnapshot.SaveToFile);
      MyRenderProfiler.LoadProfilerFromFile = new Action<int, bool>(MyObjectBuilder_ProfilerSnapshot.LoadFromFile);
    }

    protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot\u003C\u003EProfilers\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, List<MyObjectBuilder_Profiler>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProfilerSnapshot owner,
        in List<MyObjectBuilder_Profiler> value)
      {
        owner.Profilers = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProfilerSnapshot owner,
        out List<MyObjectBuilder_Profiler> value)
      {
        value = owner.Profilers;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot\u003C\u003ESimulationFrames\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, List<MyRenderProfiler.FrameInfo>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProfilerSnapshot owner,
        in List<MyRenderProfiler.FrameInfo> value)
      {
        owner.SimulationFrames = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProfilerSnapshot owner,
        out List<MyRenderProfiler.FrameInfo> value)
      {
        value = owner.SimulationFrames;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ProfilerSnapshot\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProfilerSnapshot, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ProfilerSnapshot owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ProfilerSnapshot owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ProfilerSnapshot\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ProfilerSnapshot>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ProfilerSnapshot();

      MyObjectBuilder_ProfilerSnapshot IActivator<MyObjectBuilder_ProfilerSnapshot>.CreateInstance() => new MyObjectBuilder_ProfilerSnapshot();
    }
  }
}
