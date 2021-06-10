// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyHudToggleControlHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyHudToggleControlHelper : MyAbstractControlMenuItem
  {
    private string m_value;

    public MyHudToggleControlHelper()
      : base(MyControlsSpace.TOGGLE_HUD)
    {
    }

    public override string CurrentValue => this.m_value;

    public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ToggleHud);

    public override void Activate() => MyHud.MinimalHud = !MyHud.MinimalHud;

    public override void UpdateValue()
    {
      if (!MyHud.MinimalHud)
        this.m_value = MyTexts.GetString(MyCommonTexts.ControlMenuItemValue_On);
      else
        this.m_value = MyTexts.GetString(MyCommonTexts.ControlMenuItemValue_Off);
    }

    public override void Next() => this.Activate();

    public override void Previous() => this.Activate();
  }
}
