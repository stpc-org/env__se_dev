// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Analytics.MyDropContainerEvent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Engine.Analytics
{
  public class MyDropContainerEvent : MyAnalyticsEvent
  {
    public MyDropContainerEvent(MyWorldStartEvent worldStartProperties) => this.WorldStartProperties = worldStartProperties;

    public override string GetEventName() => "DropContainer";

    [Required]
    public MyWorldStartEvent WorldStartProperties { get; set; }

    public bool? Competetive { get; set; }
  }
}
