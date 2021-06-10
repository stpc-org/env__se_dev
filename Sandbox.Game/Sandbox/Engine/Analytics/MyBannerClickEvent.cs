// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Analytics.MyBannerClickEvent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Analytics;

namespace Sandbox.Engine.Analytics
{
  public class MyBannerClickEvent : MyAnalyticsEvent
  {
    public MyBannerClickEvent(MySessionStartEvent sessionStartProperties) => this.SessionStartProperties = sessionStartProperties;

    public override string GetEventName() => "BannerClick";

    public override MyReportTypeData GetReportTypeAndArgs() => new MyReportTypeData(MyReportType.ProgressionUndefined, "Banner", this.banner_caption);

    [Required]
    public MySessionStartEvent SessionStartProperties { get; set; }

    public uint? banner_package_id { get; set; }

    public string banner_caption { get; set; }
  }
}
