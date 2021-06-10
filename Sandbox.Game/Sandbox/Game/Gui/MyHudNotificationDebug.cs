// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudNotificationDebug
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Utils;

namespace Sandbox.Game.Gui
{
  public class MyHudNotificationDebug : MyHudNotificationBase
  {
    private string m_originalText;

    public MyHudNotificationDebug(
      string text,
      int disapearTimeMs = 2500,
      string font = "White",
      MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
      int priority = 0,
      MyNotificationLevel level = MyNotificationLevel.Debug)
      : base(disapearTimeMs, font, textAlign, priority, level)
    {
      this.m_originalText = text;
    }

    protected override string GetOriginalText() => this.m_originalText;
  }
}
