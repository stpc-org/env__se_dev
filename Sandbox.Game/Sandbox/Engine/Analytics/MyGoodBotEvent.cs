// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Analytics.MyGoodBotEvent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GameSystems;

namespace Sandbox.Engine.Analytics
{
  public class MyGoodBotEvent : MyAnalyticsEvent
  {
    public MyGoodBotEvent(MyWorldStartEvent worldStartProperties) => this.WorldStartProperties = worldStartProperties;

    public override string GetEventName() => "GoodBot";

    [Required]
    public MyWorldStartEvent WorldStartProperties { get; set; }

    public ResponseType GoodBot_ResponseType { get; internal set; }

    public string GoodBot_Question { get; internal set; }

    public string GoodBot_ResponseID { get; internal set; }
  }
}
