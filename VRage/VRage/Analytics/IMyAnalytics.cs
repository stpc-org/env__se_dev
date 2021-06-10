// Decompiled with JetBrains decompiler
// Type: VRage.Analytics.IMyAnalytics
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage.Analytics
{
  public interface IMyAnalytics
  {
    void ReportEvent(
      IMyAnalyticsEvent analyticsEvent,
      DateTime timestamp,
      string sessionID,
      string userID,
      string clientVersion,
      string platform,
      Exception exception = null);

    void ReportEventLater(
      IMyAnalyticsEvent analyticsEvent,
      DateTime timestamp,
      string sessionID,
      string userID,
      string clientVersion,
      string platform,
      Exception exception = null);

    void ReportPostponedEvents();
  }
}
