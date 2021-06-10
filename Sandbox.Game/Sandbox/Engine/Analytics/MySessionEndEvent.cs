// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Analytics.MySessionEndEvent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Analytics;

namespace Sandbox.Engine.Analytics
{
  public class MySessionEndEvent : MyAnalyticsEvent
  {
    public MySessionEndEvent(MySessionStartEvent sessionStartProperties) => this.SessionStartProperties = sessionStartProperties;

    public override string GetEventName() => "SessionEnd";

    public override MyReportTypeData GetReportTypeAndArgs() => new MyReportTypeData(MyReportType.ProgressionComplete, "Game", this.game_quit_reason);

    [Required]
    public MySessionStartEvent SessionStartProperties { get; set; }

    [Required]
    public int? session_duration { get; set; }

    [Required]
    public string game_quit_reason { get; set; }

    public string Exception { get; set; }
  }
}
