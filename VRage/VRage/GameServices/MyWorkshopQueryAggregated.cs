// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyWorkshopQueryAggregated
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
  public class MyWorkshopQueryAggregated : MyWorkshopQuery
  {
    private readonly List<MyWorkshopQuery> m_queries = new List<MyWorkshopQuery>();
    private int m_queryCounter;

    public override uint ItemsPerPage
    {
      get
      {
        uint val1 = 0;
        foreach (MyWorkshopQuery query in this.m_queries)
          val1 = Math.Max(val1, query.ItemsPerPage);
        return val1;
      }
      set
      {
        foreach (MyWorkshopQuery query in this.m_queries)
          query.ItemsPerPage = value;
      }
    }

    public override bool IsRunning
    {
      get
      {
        foreach (MyWorkshopQuery query in this.m_queries)
        {
          if (query.IsRunning)
            return true;
        }
        return false;
      }
    }

    public MyWorkshopQueryAggregated(MyUGCAggregator aggregator)
    {
      foreach (IMyUGCService aggregate in aggregator.GetAggregates())
      {
        MyWorkshopQuery query = aggregate.CreateWorkshopQuery();
        query.QueryCompleted += (MyWorkshopQuery.QueryCompletedDelegate) (x => this.AggregateQueryCompleted(query, x));
        this.m_queries.Add(query);
      }
    }

    private void AggregateQueryCompleted(MyWorkshopQuery query, MyGameServiceCallResult result)
    {
      if (this.Items == null)
        this.Items = new List<MyWorkshopItem>();
      this.Items.AddRange((IEnumerable<MyWorkshopItem>) query.Items);
      this.TotalResults += query.TotalResults;
      --this.m_queryCounter;
      if (this.m_queryCounter != 0)
        return;
      this.OnQueryCompleted(result);
    }

    public override void Run() => this.Run(1U);

    public override void Run(uint startingPage)
    {
      this.Items?.Clear();
      this.TotalResults = 0U;
      this.m_queryCounter = this.m_queries.Count;
      foreach (MyWorkshopQuery query in this.m_queries)
      {
        query.SearchString = this.SearchString;
        query.ItemType = this.ItemType;
        query.ListType = this.ListType;
        query.QueryType = this.QueryType;
        query.RequiredTags = this.RequiredTags;
        query.RequireAllTags = this.RequireAllTags;
        query.ExcludedTags = this.ExcludedTags;
        query.UserId = this.UserId;
        query.ItemIds = this.ItemIds;
        query.Run(startingPage);
      }
    }

    public override void Stop()
    {
      foreach (MyWorkshopQuery query in this.m_queries)
        query.Stop();
    }

    public override void Dispose()
    {
      foreach (MyWorkshopQuery query in this.m_queries)
        query.Dispose();
      this.m_queries.Clear();
    }
  }
}
