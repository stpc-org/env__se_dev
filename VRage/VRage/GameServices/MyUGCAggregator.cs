// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyUGCAggregator
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Generic;

namespace VRage.GameServices
{
  public class MyUGCAggregator
  {
    private readonly List<IMyUGCService> m_aggregates = new List<IMyUGCService>();

    public MyWorkshopItemPublisher CreateWorkshopPublisher(
      MyWorkshopItem item)
    {
      return this.GetAggregate(item.ServiceName).CreateWorkshopPublisher(item);
    }

    public MyWorkshopQuery CreateWorkshopQuery() => (MyWorkshopQuery) new MyWorkshopQueryAggregated(this);

    public void SuspendWorkshopDownloads()
    {
      foreach (IMyUGCService aggregate in this.m_aggregates)
        aggregate.SuspendWorkshopDownloads();
    }

    public void ResumeWorkshopDownloads()
    {
      foreach (IMyUGCService aggregate in this.m_aggregates)
        aggregate.ResumeWorkshopDownloads();
    }

    public void SetTestEnvironment(bool testEnabled)
    {
      foreach (IMyUGCService aggregate in this.m_aggregates)
        aggregate.SetTestEnvironment(testEnabled);
    }

    public void Update()
    {
      foreach (IMyUGCService aggregate in this.m_aggregates)
        aggregate.Update();
    }

    public void AddAggregate(IMyUGCService ugc) => this.m_aggregates.Add(ugc);

    public List<IMyUGCService> GetAggregates() => this.m_aggregates;

    public IMyUGCService GetAggregate(string serviceName)
    {
      foreach (IMyUGCService aggregate in this.m_aggregates)
      {
        if (aggregate.ServiceName == serviceName)
          return aggregate;
      }
      return (IMyUGCService) null;
    }
  }
}
