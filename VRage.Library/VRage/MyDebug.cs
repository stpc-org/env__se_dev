// Decompiled with JetBrains decompiler
// Type: VRage.MyDebug
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using ParallelTasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using VRage.Library;
using VRage.Library.Memory;

namespace VRage
{
  public static class MyDebug
  {
    public static readonly List<TraceListener> Listeners = new List<TraceListener>();
    public static readonly NativeArrayAllocator DebugMemoryAllocator = new NativeArrayAllocator(Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("Debug"));

    [Conditional("NEVER")]
    public static void Assert(bool condition, string message = null)
    {
    }

    [Conditional("NEVER")]
    public static void Fail(string message)
    {
    }

    public static void WriteLine(string message, [CallerFilePath] string file = null, [CallerLineNumber] int line = -1)
    {
      foreach (TraceListener listener in MyDebug.Listeners)
      {
        if (listener is IAdvancedDebugListener advancedDebugListener)
          advancedDebugListener.WriteLine(message, file, line);
        else
          listener.WriteLine(message);
      }
    }

    [DebuggerStepThrough]
    public static void AssertRelease(bool condition, string message = null, [CallerFilePath] string file = null, [CallerLineNumber] int line = -1)
    {
      if (condition)
        return;
      MyDebug.FailRelease(message, file, line);
    }

    [DebuggerStepThrough]
    public static void FailRelease(string message, [CallerFilePath] string file = null, [CallerLineNumber] int line = -1)
    {
      foreach (TraceListener listener in MyDebug.Listeners)
      {
        if (listener is IAdvancedDebugListener advancedDebugListener)
          advancedDebugListener.Fail(message, (string) null, file, line);
        else
          listener.Fail(message);
      }
    }
  }
}
