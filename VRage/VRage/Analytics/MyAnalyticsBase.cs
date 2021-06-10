// Decompiled with JetBrains decompiler
// Type: VRage.Analytics.MyAnalyticsBase
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using LitJson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace VRage.Analytics
{
  public abstract class MyAnalyticsBase : IMyAnalytics
  {
    private MyObjectFileStorage m_eventStorage;

    protected MyAnalyticsBase(string eventStoragePath, int maxStoredEvents = -1)
    {
      if (eventStoragePath == null)
        return;
      this.m_eventStorage = new MyObjectFileStorage(eventStoragePath, maxStoredEvents);
    }

    protected abstract void ReportEvent(MyAnalyticsBase.MyEvent myEvent);

    protected bool StoreUnsentEvent(MyAnalyticsBase.MyEvent eventToStore, DateTime timestamp)
    {
      MyObjectFileStorage eventStorage = this.m_eventStorage;
      return eventStorage != null && eventStorage.StoreObject<MyAnalyticsBase.MyEvent>(eventToStore, timestamp);
    }

    public void ReportEvent(
      IMyAnalyticsEvent analyticsEvent,
      DateTime timestamp,
      string sessionID,
      string userID,
      string clientVersion,
      string platform,
      Exception exception = null)
    {
      this.ReportEvent(new MyAnalyticsBase.MyEvent()
      {
        EventName = analyticsEvent.GetEventName(),
        EventTimestamp = timestamp,
        SessionID = sessionID,
        UserID = userID,
        ClientVersion = clientVersion,
        Platform = platform,
        Exception = this.BuildExceptionStackString(exception),
        ReportTypeAndArgs = analyticsEvent.GetReportTypeAndArgs(),
        P = analyticsEvent.GetPropertiesDictionary()
      });
    }

    public void ReportEventLater(
      IMyAnalyticsEvent analyticsEvent,
      DateTime timestamp,
      string sessionID,
      string userID,
      string clientVersion,
      string platform,
      Exception exception = null)
    {
      this.StoreUnsentEvent(new MyAnalyticsBase.MyEvent()
      {
        EventName = analyticsEvent.GetEventName(),
        EventTimestamp = timestamp,
        SessionID = sessionID,
        UserID = userID,
        ClientVersion = clientVersion,
        Platform = platform,
        Exception = this.BuildExceptionStackString(exception),
        ReportTypeAndArgs = analyticsEvent.GetReportTypeAndArgs(),
        P = analyticsEvent.GetPropertiesDictionary()
      }, timestamp);
    }

    public void ReportPostponedEvents()
    {
      foreach (MyAnalyticsBase.MyEvent andWipeUnsentEvent in this.GetAndWipeUnsentEvents())
        this.ReportEvent(andWipeUnsentEvent);
    }

    private List<MyAnalyticsBase.MyEvent> GetAndWipeUnsentEvents() => this.m_eventStorage?.RetrieveStoredObjectsByType<MyAnalyticsBase.MyEvent>(true) ?? new List<MyAnalyticsBase.MyEvent>();

    private string BuildExceptionStackString(Exception exception)
    {
      if (exception == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(exception?.Message ?? "Native crash");
      stringBuilder.AppendLine();
      stringBuilder.AppendLine(exception?.StackTrace);
      return stringBuilder.ToString();
    }

    protected class MyEvent
    {
      public string EventName { get; set; }

      public DateTime EventTimestamp { get; set; }

      public string UserID { get; set; }

      public string SessionID { get; set; }

      public string Platform { get; set; }

      public string ClientVersion { get; set; }

      public string Exception { get; set; }

      public MyReportTypeData ReportTypeAndArgs { get; set; }

      public Dictionary<string, object> P { get; set; }

      public string ToJSON()
      {
        JsonMapper.RegisterExporter<DateTime>((ExporterFunc<DateTime>) ((value, writer) => writer.Write(value.ToString("o", (IFormatProvider) CultureInfo.InvariantCulture))));
        try
        {
          return JsonMapper.ToJson((object) this);
        }
        finally
        {
          JsonMapper.UnregisterExporters();
        }
      }
    }
  }
}
