// Decompiled with JetBrains decompiler
// Type: VRage.Library.Memory.MyMemoryTracker
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using ParallelTasks;

namespace VRage.Library.Memory
{
  public class MyMemoryTracker : Singleton<MyMemoryTracker>
  {
    public const bool ENABLED = true;

    public MyMemorySystem ProcessMemorySystem { get; }

    public MyMemoryTracker() => this.ProcessMemorySystem = MyMemorySystem.CreateRootMemorySystem("Systems");

    public void LogMemoryStats<TLogger>(ref TLogger logger) where TLogger : struct, MyMemoryTracker.ILogger => this.ProcessMemorySystem.LogMemoryStats<TLogger>(ref logger);

    public interface ILogger
    {
      void BeginSystem(string systemName);

      void EndSystem(long systemBytes, int totalAllocations);
    }
  }
}
