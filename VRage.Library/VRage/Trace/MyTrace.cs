// Decompiled with JetBrains decompiler
// Type: VRage.Trace.MyTrace
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace VRage.Trace
{
  public static class MyTrace
  {
    public const string TracingSymbol = "__RANDOM_UNDEFINED_PROFILING_SYMBOL__";
    private const string WindowName = "SE";
    private static Dictionary<int, ITrace> m_traces;
    private static readonly MyNullTrace m_nullTrace = new MyNullTrace();

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void Init(InitTraceHandler handler)
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void InitWinTrace()
    {
    }

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    [Conditional("DEVELOP")]
    private static void InitInternal(InitTraceHandler handler)
    {
      MyTrace.m_traces = new Dictionary<int, ITrace>();
      string str1 = "SE";
      foreach (object obj in Enum.GetValues(typeof (TraceWindow)))
      {
        string str2 = (TraceWindow) obj == TraceWindow.Default ? str1 : str1 + "_" + obj.ToString();
        MyTrace.m_traces[(int) obj] = handler(str2, str2);
      }
    }

    private static ITrace InitWintraceHandler(string traceId, string traceName) => MyWintraceWrapper.CreateTrace(traceId, traceName);

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void Watch(string name, object value) => MyTrace.GetTrace(TraceWindow.Default).Watch(name, value);

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public static void Send(TraceWindow window, string msg, string comment = null) => MyTrace.GetTrace(window).Send(msg, comment);

    public static ITrace GetTrace(TraceWindow window)
    {
      ITrace nullTrace;
      if (MyTrace.m_traces == null || !MyTrace.m_traces.TryGetValue((int) window, out nullTrace))
        nullTrace = (ITrace) MyTrace.m_nullTrace;
      return nullTrace;
    }
  }
}
