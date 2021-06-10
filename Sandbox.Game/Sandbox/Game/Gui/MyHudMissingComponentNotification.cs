// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudMissingComponentNotification
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
  public class MyHudMissingComponentNotification : MyHudNotificationBase
  {
    private MyStringId m_originalText;

    public MyHudMissingComponentNotification(
      MyStringId text,
      int disapearTimeMs = 2500,
      string font = "White",
      MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
      int priority = 0,
      MyNotificationLevel level = MyNotificationLevel.Normal)
      : base(disapearTimeMs, font, textAlign, priority, level)
    {
      this.m_originalText = text;
    }

    protected override string GetOriginalText() => MyTexts.GetString(this.m_originalText);

    public void SetBlockDefinition(MyCubeBlockDefinition definition) => this.SetTextFormatArguments((object) definition.Components[0].Definition.DisplayNameText.ToString(), (object) definition.DisplayNameText.ToString());

    public override void BeforeAdd() => MyHud.BlockInfo.MissingComponentIndex = 0;

    public override void BeforeRemove() => MyHud.BlockInfo.MissingComponentIndex = -1;
  }
}
