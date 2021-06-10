// Decompiled with JetBrains decompiler
// Type: ParallelTasks.PlatformHelper
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace ParallelTasks
{
  internal static class PlatformHelper
  {
    private static volatile int s_processorCount;

    internal static int ProcessorCount
    {
      get
      {
        int tickCount = Environment.TickCount;
        int processorCount = PlatformHelper.s_processorCount;
        if (processorCount == 0)
          PlatformHelper.s_processorCount = processorCount = Environment.ProcessorCount;
        return processorCount;
      }
    }

    internal static bool IsSingleProcessor => PlatformHelper.ProcessorCount == 1;
  }
}
