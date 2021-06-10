// Decompiled with JetBrains decompiler
// Type: VRage.Library.Threading.ProcessUtil
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace VRage.Library.Threading
{
  public static class ProcessUtil
  {
    public static void SetThreadProcessorAffinity(params int[] cpus)
    {
      if (cpus == null)
        throw new ArgumentNullException(nameof (cpus));
      if (cpus.Length == 0)
        throw new ArgumentException("You must specify at least one CPU.", nameof (cpus));
      long num = 0;
      foreach (int cpu in cpus)
      {
        if (cpu < 0 || cpu >= Environment.ProcessorCount)
          throw new ArgumentException("Invalid CPU number.");
        num |= 1L << cpu;
      }
      Thread.BeginThreadAffinity();
      int osThreadId = AppDomain.GetCurrentThreadId();
      Process.GetCurrentProcess().Threads.Cast<ProcessThread>().Single<ProcessThread>((Func<ProcessThread, bool>) (t => t.Id == osThreadId)).ProcessorAffinity = new IntPtr(num);
    }
  }
}
