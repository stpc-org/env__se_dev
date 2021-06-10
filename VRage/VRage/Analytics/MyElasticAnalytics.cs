// Decompiled with JetBrains decompiler
// Type: VRage.Analytics.MyElasticAnalytics
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Net;
using System.Text;
using VRage.Http;
using VRage.Utils;

namespace VRage.Analytics
{
  public class MyElasticAnalytics : MyAnalyticsBase
  {
    private readonly string m_apiKey;
    private readonly string m_apiUrl;

    public MyElasticAnalytics(
      string apiUrl,
      string apiKeyId,
      string apiKey,
      string eventStoragePath,
      int maxStoredEvents)
      : base(eventStoragePath, maxStoredEvents)
    {
      this.m_apiKey = "ApiKey " + Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKeyId + ":" + apiKey));
      this.m_apiUrl = apiUrl;
      this.ReportPostponedEvents();
    }

    protected override void ReportEvent(MyAnalyticsBase.MyEvent myEvent) => this.SendEventToElasticOrStore(myEvent);

    private void SendEventToElasticOrStore(MyAnalyticsBase.MyEvent eventToSend)
    {
      HttpData[] parameters = new HttpData[3]
      {
        new HttpData("Authorization", (object) this.m_apiKey, HttpDataType.HttpHeader),
        new HttpData("Content-Type", (object) "application/json", HttpDataType.HttpHeader),
        new HttpData("application/json", (object) eventToSend.ToJSON(), HttpDataType.RequestBody)
      };
      MyLog.Default.WriteLine("Sending event to ElasticSearch: " + eventToSend.EventName);
      MyVRage.Platform.Http.SendRequestAsync(this.m_apiUrl, parameters, HttpMethod.POST, (Action<HttpStatusCode, string>) ((code, response) =>
      {
        if (code == HttpStatusCode.OK || code == HttpStatusCode.Created)
          return;
        MyLog.Default.WriteLine(string.Format("Elastic http response error {0}: {1}", (object) code, (object) response));
        this.StoreUnsentEvent(eventToSend, eventToSend.EventTimestamp);
      }));
    }
  }
}
