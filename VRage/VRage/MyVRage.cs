// Decompiled with JetBrains decompiler
// Type: VRage.MyVRage
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ParallelTasks;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VRage.Collections;
using VRage.Library.Extensions;
using VRage.Profiler;
using VRage.Serialization;

namespace VRage
{
  public static class MyVRage
  {
    private static readonly List<Action> m_exitCallbacks = new List<Action>();
    public const string ProtobufferExtension = "B5";
    public static bool EnableMemoryPooling = true;

    public static IVRagePlatform Platform { get; private set; }

    public static void Init(IVRagePlatform platform)
    {
      MyVRage.Platform = platform;
      MyVRage.InitSettings();
      MyProfilerBlock.GetThreadAllocationStamp = new Func<ulong>(MyVRage.Platform.System.GetThreadAllocationStamp);
      DependencyBatch.ErrorReportingFunction = WorkItem.ErrorReportingFunction = new Action<Exception>(MyMiniDump.CollectExceptionDump);
      ExpressionExtension.Factory = (IActivatorFactory) new PrecompiledActivatorFactory();
    }

    public static void Done()
    {
      foreach (Action exitCallback in MyVRage.m_exitCallbacks)
        exitCallback();
      MyVRage.m_exitCallbacks.Clear();
      MyVRage.Platform.Done();
      MyConcurrentBucketPool.OnExit();
    }

    public static void RegisterExitCallback(Action callback) => MyVRage.m_exitCallbacks.Add(callback);

    private static void InitSettings() => MyConcurrentBucketPool.EnablePooling = MyVRage.EnableMemoryPooling;
  }
}
