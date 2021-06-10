// Decompiled with JetBrains decompiler
// Type: VRage.Stats.MyStatKeys
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;

namespace VRage.Stats
{
  public class MyStatKeys
  {
    private static Dictionary<MyStatKeys.StatKeysEnum, MyStatKeys.MyNamePriorityPair> m_collection = new Dictionary<MyStatKeys.StatKeysEnum, MyStatKeys.MyNamePriorityPair>()
    {
      {
        MyStatKeys.StatKeysEnum.Frame,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Frame",
          Priority = 1100
        }
      },
      {
        MyStatKeys.StatKeysEnum.FPS,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "FPS",
          Priority = 1000
        }
      },
      {
        MyStatKeys.StatKeysEnum.UPS,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "UPS",
          Priority = 900
        }
      },
      {
        MyStatKeys.StatKeysEnum.SimSpeed,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Simulation speed",
          Priority = 800
        }
      },
      {
        MyStatKeys.StatKeysEnum.SimCpuLoad,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Simulation CPU Load: {0}% {3:0.00}ms",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.ThreadCpuLoad,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Thread CPU Load: {0}% {3:0.00}ms",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.RenderCpuLoad,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Render CPU Load: {0}% {3:0.00}ms",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.RenderGpuLoad,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Render GPU Load: {0}% {3:0.00}ms",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.ServerSimSpeed,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Server simulation speed",
          Priority = 700
        }
      },
      {
        MyStatKeys.StatKeysEnum.ServerSimCpuLoad,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Server simulation CPU Load: {0}%",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.ServerThreadCpuLoad,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Server thread CPU Load: {0}%",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.Up,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Up: {0.##} kB/s",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.Down,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Down: {0.##} kB/s",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.ServerUp,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Server Up: {0.##} kB/s",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.ServerDown,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Server Down: {0.##} kB/s",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.Roundtrip,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Roundtrip: {0}ms",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.PlayoutDelayBuffer,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "PlayoutDelayBufferSize: {0}",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.FrameTime,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Frame time: {0} ms",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.FrameAvgTime,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Frame avg time: {0} ms",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.FrameMinTime,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Frame min time: {0} ms",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.FrameMaxTime,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Frame max time: {0} ms",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.UpdateLag,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Update lag (per s)",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.GcMemory,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "GC Memory (MB)",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.ProcessMemory,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Process memory",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.ActiveParticleEffs,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Active particle effects",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.PhysWorldCount,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Physics worlds count: {0}",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.ActiveRigBodies,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Active rigid bodies: {0}",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.PhysStepTimeSum,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Physics step time (sum): {0} ms",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.PhysStepTimeAvg,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Physics step time (avg): {0} ms",
          Priority = 0
        }
      },
      {
        MyStatKeys.StatKeysEnum.PhysStepTimeMax,
        new MyStatKeys.MyNamePriorityPair()
        {
          Name = "Physics step time (max): {0} ms",
          Priority = 0
        }
      }
    };

    public static string GetName(MyStatKeys.StatKeysEnum key)
    {
      MyStatKeys.MyNamePriorityPair namePriorityPair;
      return !MyStatKeys.m_collection.TryGetValue(key, out namePriorityPair) ? string.Empty : namePriorityPair.Name;
    }

    public static int GetPriority(MyStatKeys.StatKeysEnum key)
    {
      MyStatKeys.MyNamePriorityPair namePriorityPair;
      return !MyStatKeys.m_collection.TryGetValue(key, out namePriorityPair) ? 0 : namePriorityPair.Priority;
    }

    public static void GetNameAndPriority(
      MyStatKeys.StatKeysEnum key,
      out string name,
      out int priority)
    {
      MyStatKeys.MyNamePriorityPair namePriorityPair;
      if (!MyStatKeys.m_collection.TryGetValue(key, out namePriorityPair))
      {
        name = string.Empty;
        priority = 0;
      }
      else
      {
        name = namePriorityPair.Name;
        priority = namePriorityPair.Priority;
      }
    }

    public enum StatKeysEnum
    {
      None,
      Frame,
      FPS,
      UPS,
      SimSpeed,
      SimCpuLoad,
      ThreadCpuLoad,
      RenderCpuLoad,
      RenderGpuLoad,
      ServerSimSpeed,
      ServerSimCpuLoad,
      ServerThreadCpuLoad,
      Up,
      Down,
      ServerUp,
      ServerDown,
      Roundtrip,
      PlayoutDelayBuffer,
      FrameTime,
      FrameAvgTime,
      FrameMinTime,
      FrameMaxTime,
      UpdateLag,
      GcMemory,
      ProcessMemory,
      ActiveParticleEffs,
      PhysWorldCount,
      ActiveRigBodies,
      PhysStepTimeSum,
      PhysStepTimeAvg,
      PhysStepTimeMax,
    }

    public struct MyNamePriorityPair
    {
      public string Name;
      public int Priority;
    }
  }
}
