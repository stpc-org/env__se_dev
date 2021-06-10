// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyModsLoadListResult
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.GameServices;

namespace Sandbox.Game.Screens
{
  public class MyModsLoadListResult : IMyAsyncResult
  {
    public bool IsCompleted => this.Task.IsComplete;

    public Task Task { get; private set; }

    public (MyGameServiceCallResult, string) Result { get; private set; }

    public List<MyWorkshopItem> SubscribedMods { get; private set; }

    public List<MyWorkshopItem> SetMods { get; private set; }

    public MyModsLoadListResult(HashSet<WorkshopId> ids)
    {
      MyModsLoadListResult modsLoadListResult = this;
      HashSet<WorkshopId> toGet = new HashSet<WorkshopId>((IEnumerable<WorkshopId>) ids);
      this.Task = Parallel.Start((Action) (() =>
      {
        closure_0.SubscribedMods = new List<MyWorkshopItem>(ids.Count);
        closure_0.SetMods = new List<MyWorkshopItem>();
        if (!MyGameService.IsOnline)
          return;
        (MyGameServiceCallResult, string) subscribedModsBlocking = MyWorkshop.GetSubscribedModsBlocking(closure_0.SubscribedMods);
        foreach (MyWorkshopItem subscribedMod in closure_0.SubscribedMods)
          toGet.Remove(new WorkshopId(subscribedMod.Id, subscribedMod.ServiceName));
        if (toGet.Count > 0)
          MyWorkshop.GetItemsBlockingUGC(toGet.ToList<WorkshopId>(), closure_0.SetMods);
        closure_0.Result = subscribedModsBlocking;
      }));
    }
  }
}
