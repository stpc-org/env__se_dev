// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyWorkshopQuery
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
  public class MyWorkshopQuery : IDisposable
  {
    public List<MyWorkshopItem> Items { get; protected set; }

    public uint TotalResults { get; protected set; }

    public virtual uint ItemsPerPage { get; set; }

    public virtual bool IsRunning { get; protected set; }

    public string SearchString { get; set; }

    public WorkshopItemType ItemType { get; set; }

    public WorkshopListType ListType { get; set; }

    public MyWorkshopQueryType QueryType { get; set; }

    public List<string> RequiredTags { get; set; }

    public bool RequireAllTags { get; set; }

    public List<string> ExcludedTags { get; set; }

    public ulong UserId { get; set; }

    public List<ulong> ItemIds { get; set; }

    public event MyWorkshopQuery.QueryCompletedDelegate QueryCompleted;

    public event MyWorkshopQuery.PageQueryCompletedDelegate PageQueryCompleted;

    protected MyWorkshopQuery()
    {
    }

    ~MyWorkshopQuery() => this.Dispose();

    public virtual void Run()
    {
    }

    public virtual void Run(uint startingPage)
    {
    }

    public virtual void Stop()
    {
    }

    public virtual void Dispose()
    {
      this.QueryCompleted = (MyWorkshopQuery.QueryCompletedDelegate) null;
      this.PageQueryCompleted = (MyWorkshopQuery.PageQueryCompletedDelegate) null;
    }

    protected void OnQueryCompleted(MyGameServiceCallResult result)
    {
      MyWorkshopQuery.QueryCompletedDelegate queryCompleted = this.QueryCompleted;
      if (queryCompleted == null)
        return;
      queryCompleted(result);
    }

    protected void OnPageQueryCompleted(MyGameServiceCallResult result, uint page)
    {
      MyWorkshopQuery.PageQueryCompletedDelegate pageQueryCompleted = this.PageQueryCompleted;
      if (pageQueryCompleted == null)
        return;
      pageQueryCompleted(result, page);
    }

    public delegate void QueryCompletedDelegate(MyGameServiceCallResult result);

    public delegate void PageQueryCompletedDelegate(MyGameServiceCallResult result, uint page);
  }
}
