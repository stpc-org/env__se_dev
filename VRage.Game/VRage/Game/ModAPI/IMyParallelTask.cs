// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyParallelTask
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ParallelTasks;
using System;
using System.Collections.Generic;

namespace VRage.Game.ModAPI
{
  public interface IMyParallelTask
  {
    WorkOptions DefaultOptions { get; }

    Task StartBackground(IWork work, Action completionCallback);

    Task StartBackground(IWork work);

    Task StartBackground(Action action);

    Task StartBackground(Action action, Action completionCallback);

    Task StartBackground(
      Action<WorkData> action,
      Action<WorkData> completionCallback,
      WorkData workData);

    void Do(IWork a, IWork b);

    void Do(params IWork[] work);

    void Do(Action action1, Action action2);

    void Do(params Action[] actions);

    void For(int startInclusive, int endExclusive, Action<int> body);

    void For(int startInclusive, int endExclusive, Action<int> body, int stride);

    void ForEach<T>(IEnumerable<T> collection, Action<T> action);

    Task Start(Action action, WorkOptions options, Action completionCallback);

    Task Start(Action action, WorkOptions options);

    Task Start(Action action, Action completionCallback);

    Task Start(Action action);

    Task Start(Action<WorkData> action, Action<WorkData> completionCallback, WorkData workData);

    Task Start(IWork work, Action completionCallback);

    Task Start(IWork work);

    void Sleep(int millisecondsTimeout);

    void Sleep(TimeSpan timeout);
  }
}
