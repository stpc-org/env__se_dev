// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControlExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using System;
using System.Text;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
  public static class MyTerminalControlExtensions
  {
    private static StringBuilder Combine(MyStringId prefix, MyStringId title)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      StringBuilder stringBuilder2 = MyTexts.Get(prefix);
      if (stringBuilder2.Length > 0)
        stringBuilder1.Append((object) stringBuilder2).Append(" ");
      return stringBuilder1.Append(MyTexts.GetString(title)).TrimTrailingWhitespace();
    }

    private static StringBuilder GetTitle(MyStringId title)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = MyTexts.GetString(title);
      if (str.Length > 0)
        stringBuilder.Append(str);
      return stringBuilder;
    }

    private static StringBuilder CombineOnOff(
      MyStringId title,
      MyStringId? on = null,
      MyStringId? off = null)
    {
      StringBuilder stringBuilder1 = MyTerminalControlExtensions.GetTitle(title).Append(" ");
      MyStringId? nullable = on;
      string str1 = MyTexts.GetString(nullable ?? MySpaceTexts.SwitchText_On);
      StringBuilder stringBuilder2 = stringBuilder1.Append(str1).Append("/");
      nullable = off;
      string str2 = MyTexts.GetString(nullable ?? MySpaceTexts.SwitchText_Off);
      return stringBuilder2.Append(str2);
    }

    public static void EnableActions<TBlock>(
      this MyTerminalControlSlider<TBlock> slider,
      float step = 0.05f,
      Func<TBlock, bool> enabled = null,
      Func<TBlock, bool> callable = null)
      where TBlock : MyTerminalBlock
    {
      slider.EnableActions<TBlock>(MyTerminalActionIcons.INCREASE, MyTerminalActionIcons.DECREASE, step, enabled, callable);
    }

    public static void EnableActions<TBlock>(
      this MyTerminalControlSlider<TBlock> slider,
      string increaseIcon,
      string decreaseIcon,
      float step = 0.05f,
      Func<TBlock, bool> enabled = null,
      Func<TBlock, bool> callable = null)
      where TBlock : MyTerminalBlock
    {
      StringBuilder increaseName = MyTerminalControlExtensions.Combine(MySpaceTexts.ToolbarAction_Increase, slider.Title);
      StringBuilder decreaseName = MyTerminalControlExtensions.Combine(MySpaceTexts.ToolbarAction_Decrease, slider.Title);
      slider.EnableActions(increaseIcon, decreaseIcon, increaseName, decreaseName, step, enabled: enabled, callable: callable);
    }

    public static void EnableActionsWithReset<TBlock>(
      this MyTerminalControlSlider<TBlock> slider,
      float step = 0.05f)
      where TBlock : MyTerminalBlock
    {
      slider.EnableActionsWithReset<TBlock>(MyTerminalActionIcons.INCREASE, MyTerminalActionIcons.DECREASE, MyTerminalActionIcons.RESET, step);
    }

    public static void EnableActionsWithReset<TBlock>(
      this MyTerminalControlSlider<TBlock> slider,
      string increaseIcon,
      string decreaseIcon,
      string resetIcon,
      float step = 0.05f)
      where TBlock : MyTerminalBlock
    {
      StringBuilder increaseName = MyTerminalControlExtensions.Combine(MySpaceTexts.ToolbarAction_Increase, slider.Title);
      StringBuilder decreaseName = MyTerminalControlExtensions.Combine(MySpaceTexts.ToolbarAction_Decrease, slider.Title);
      StringBuilder resetName = MyTerminalControlExtensions.Combine(MySpaceTexts.ToolbarAction_Reset, slider.Title);
      slider.EnableActions(increaseIcon, decreaseIcon, increaseName, decreaseName, step, resetIcon, resetName);
    }

    public static MyTerminalAction<TBlock> EnableAction<TBlock>(
      this MyTerminalControlButton<TBlock> button,
      string icon = null,
      MyStringId? title = null,
      MyTerminalControl<TBlock>.WriterDelegate writer = null)
      where TBlock : MyTerminalBlock
    {
      return button.EnableAction(icon ?? MyTerminalActionIcons.TOGGLE, MyTexts.Get(title ?? button.Title), writer);
    }

    public static MyTerminalAction<TBlock> EnableAction<TBlock>(
      this MyTerminalControlButton<TBlock> button,
      Func<TBlock, bool> enabled,
      string icon = null)
      where TBlock : MyTerminalBlock
    {
      MyTerminalAction<TBlock> myTerminalAction = button.EnableAction(icon ?? MyTerminalActionIcons.TOGGLE, MyTexts.Get(button.Title));
      myTerminalAction.Enabled = enabled;
      return myTerminalAction;
    }

    public static MyTerminalAction<TBlock> EnableAction<TBlock>(
      this MyTerminalControlCheckbox<TBlock> checkbox,
      Func<TBlock, bool> callable = null)
      where TBlock : MyTerminalBlock
    {
      StringBuilder name = MyTerminalControlExtensions.CombineOnOff(checkbox.Title);
      StringBuilder onText = MyTexts.Get(checkbox.OnText);
      StringBuilder offText = MyTexts.Get(checkbox.OffText);
      MyTerminalAction<TBlock> myTerminalAction = checkbox.EnableAction(MyTerminalActionIcons.TOGGLE, name, onText, offText);
      myTerminalAction.Enabled = checkbox.Enabled;
      if (callable != null)
        myTerminalAction.Callable = callable;
      return myTerminalAction;
    }

    public static MyTerminalAction<TBlock> EnableToggleAction<TBlock>(
      this MyTerminalControlOnOffSwitch<TBlock> onOff)
      where TBlock : MyTerminalBlock
    {
      return onOff.EnableToggleAction<TBlock>(MyTerminalActionIcons.TOGGLE);
    }

    public static MyTerminalAction<TBlock> EnableToggleAction<TBlock>(
      this MyTerminalControlOnOffSwitch<TBlock> onOff,
      string iconPath)
      where TBlock : MyTerminalBlock
    {
      StringBuilder name = MyTerminalControlExtensions.CombineOnOff(onOff.Title, new MyStringId?(onOff.OnText), new MyStringId?(onOff.OffText));
      StringBuilder onText = MyTexts.Get(onOff.OnText);
      StringBuilder offText = MyTexts.Get(onOff.OffText);
      return onOff.EnableToggleAction(iconPath, name, onText, offText);
    }

    public static void EnableOnOffActions<TBlock>(this MyTerminalControlOnOffSwitch<TBlock> onOff) where TBlock : MyTerminalBlock => onOff.EnableOnOffActions<TBlock>(MyTerminalActionIcons.ON, MyTerminalActionIcons.OFF);

    public static void EnableOnOffActions<TBlock>(
      this MyTerminalControlOnOffSwitch<TBlock> onOff,
      string onIcon,
      string OffIcon)
      where TBlock : MyTerminalBlock
    {
      StringBuilder onText = MyTexts.Get(onOff.OnText);
      StringBuilder offText = MyTexts.Get(onOff.OffText);
      onOff.EnableOnAction(onIcon, MyTerminalControlExtensions.GetTitle(onOff.Title).Append(" ").Append((object) onText), onText, offText);
      onOff.EnableOffAction(OffIcon, MyTerminalControlExtensions.GetTitle(onOff.Title).Append(" ").Append((object) offText), onText, offText);
    }
  }
}
