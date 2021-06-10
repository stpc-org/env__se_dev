// Decompiled with JetBrains decompiler
// Type: VRage.Analytics.MyAnalyticsManager
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.Analytics
{
  public abstract class MyAnalyticsManager : IMyAnalytics
  {
    private List<IMyAnalytics> m_analyticsTrackers = new List<IMyAnalytics>();

    public void RegisterAnalyticsTracker(IMyAnalytics tracker)
    {
      if (tracker == null || this.m_analyticsTrackers.Contains(tracker))
        return;
      this.m_analyticsTrackers.Add(tracker);
    }

    void IMyAnalytics.ReportEvent(
      IMyAnalyticsEvent analyticsEvent,
      DateTime timestamp,
      string sessionID,
      string userID,
      string clientVersion,
      string platform,
      Exception exception)
    {
      foreach (IMyAnalytics analyticsTracker in this.m_analyticsTrackers)
        analyticsTracker.ReportEvent(analyticsEvent, timestamp, sessionID, userID, clientVersion, platform, exception);
    }

    void IMyAnalytics.ReportEventLater(
      IMyAnalyticsEvent analyticsEvent,
      DateTime timestamp,
      string sessionID,
      string userID,
      string clientVersion,
      string platform,
      Exception exception)
    {
      foreach (IMyAnalytics analyticsTracker in this.m_analyticsTrackers)
        analyticsTracker.ReportEventLater(analyticsEvent, timestamp, sessionID, userID, clientVersion, platform, exception);
    }

    public void ReportPostponedEvents()
    {
      foreach (IMyAnalytics analyticsTracker in this.m_analyticsTrackers)
        analyticsTracker.ReportPostponedEvents();
    }
  }
}
