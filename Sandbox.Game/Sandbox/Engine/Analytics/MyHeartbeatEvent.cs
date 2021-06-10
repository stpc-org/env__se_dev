// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Analytics.MyHeartbeatEvent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Analytics;

namespace Sandbox.Engine.Analytics
{
  public class MyHeartbeatEvent : MyAnalyticsEvent
  {
    public override string GetEventName() => "Heartbeat";

    public override MyReportTypeData GetReportTypeAndArgs() => new MyReportTypeData(MyReportType.ProgressionUndefined, (string) null, (string) null);

    public string Region_ISO2 { get; set; }

    public string Region_ISO3 { get; set; }
  }
}
