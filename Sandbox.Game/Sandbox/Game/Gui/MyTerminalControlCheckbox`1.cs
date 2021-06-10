// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControlCheckbox`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Text;
using VRage;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyTerminalControlCheckbox<TBlock> : MyTerminalValueControl<TBlock, bool>, IMyTerminalControlCheckbox, IMyTerminalControl, IMyTerminalValueControl<bool>, ITerminalProperty, IMyTerminalControlTitleTooltip
    where TBlock : MyTerminalBlock
  {
    private Action<TBlock> m_action;
    private MyGuiControlCheckbox m_checkbox;
    private MyGuiControlBlockProperty m_checkboxAligner;
    private Action<MyGuiControlCheckbox> m_checkboxClicked;
    private bool m_isAutoScaleEnabled;
    private bool m_isAutoEllipsisEnabled;
    public float MaxWidth;
    public MyStringId Title;
    public MyStringId OnText;
    public MyStringId OffText;
    public MyStringId Tooltip;
    private bool m_justify;

    public MyTerminalControlCheckbox(
      string id,
      MyStringId title,
      MyStringId tooltip,
      MyStringId? on = null,
      MyStringId? off = null,
      bool justify = false,
      bool isAutoscaleEnabled = false,
      bool isAutoEllipsisEnabled = false,
      float maxWidth = 1f)
      : base(id)
    {
      this.Title = title;
      MyStringId? nullable = on;
      this.OnText = nullable ?? MySpaceTexts.SwitchText_On;
      nullable = off;
      this.OffText = nullable ?? MySpaceTexts.SwitchText_Off;
      this.Tooltip = tooltip;
      this.Serializer = (MyTerminalValueControl<TBlock, bool>.SerializerDelegate) ((BitStream stream, ref bool value) => stream.Serialize(ref value));
      this.m_justify = justify;
      this.m_isAutoScaleEnabled = isAutoscaleEnabled;
      this.m_isAutoEllipsisEnabled = isAutoEllipsisEnabled;
      this.MaxWidth = maxWidth;
    }

    protected override MyGuiControlBase CreateGui()
    {
      this.m_checkbox = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(this.Tooltip));
      this.m_checkboxClicked = new Action<MyGuiControlCheckbox>(this.OnCheckboxClicked);
      this.m_checkbox.IsCheckedChanged = this.m_checkboxClicked;
      string title = MyTexts.GetString(this.Title);
      string tooltip = MyTexts.GetString(this.Tooltip);
      MyGuiControlCheckbox checkbox = this.m_checkbox;
      bool autoScaleEnabled = this.m_isAutoScaleEnabled;
      int num1 = this.m_isAutoEllipsisEnabled ? 1 : 0;
      int num2 = autoScaleEnabled ? 1 : 0;
      double maxWidth = (double) this.MaxWidth;
      this.m_checkboxAligner = new MyGuiControlBlockProperty(title, tooltip, (MyGuiControlBase) checkbox, MyGuiControlBlockPropertyLayoutEnum.Horizontal, isAutoEllipsisEnabled: (num1 != 0), isAutoScaleEnabled: (num2 != 0), max_Width: ((float) maxWidth));
      return (MyGuiControlBase) this.m_checkboxAligner;
    }

    private void OnCheckboxClicked(MyGuiControlCheckbox obj)
    {
      foreach (TBlock targetBlock in this.TargetBlocks)
        this.SetValue(targetBlock, obj.IsChecked);
    }

    protected override void OnUpdateVisual()
    {
      base.OnUpdateVisual();
      TBlock firstBlock = this.FirstBlock;
      if ((object) firstBlock != null)
        this.m_checkbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) null;
      this.m_checkbox.IsChecked = this.GetValue(firstBlock);
      this.m_checkbox.IsCheckedChanged = this.m_checkboxClicked;
      if (!this.m_justify || !(this.m_checkboxAligner.Owner is MyGuiControlList))
        return;
      this.m_checkboxAligner.Size = new Vector2(0.235f, this.m_checkboxAligner.Size.Y);
    }

    private void SwitchAction(TBlock block) => this.SetValue(block, !this.GetValue(block));

    private void CheckAction(TBlock block) => this.SetValue(block, true);

    private void UncheckAction(TBlock block) => this.SetValue(block, false);

    private void Writer(
      TBlock block,
      StringBuilder result,
      StringBuilder onText,
      StringBuilder offText)
    {
      result.Append(this.GetValue(block) ? (object) onText : (object) offText);
    }

    public MyTerminalAction<TBlock> EnableAction(
      string icon,
      StringBuilder name,
      StringBuilder onText,
      StringBuilder offText,
      Func<TBlock, bool> enabled = null,
      Func<TBlock, bool> callable = null)
    {
      MyTerminalAction<TBlock> myTerminalAction = new MyTerminalAction<TBlock>(this.Id, name, new Action<TBlock>(this.SwitchAction), (MyTerminalControl<TBlock>.WriterDelegate) ((x, r) => this.Writer(x, r, onText, offText)), icon, enabled, callable);
      this.Actions = new MyTerminalAction<TBlock>[1]
      {
        myTerminalAction
      };
      return myTerminalAction;
    }

    public override bool GetDefaultValue(TBlock block) => false;

    public override bool GetMinimum(TBlock block) => false;

    public override bool GetMaximum(TBlock block) => true;

    public override void SetValue(TBlock block, bool value) => base.SetValue(block, value);

    public override bool GetValue(TBlock block) => base.GetValue(block);

    MyStringId IMyTerminalControlCheckbox.OnText
    {
      get => this.OnText;
      set => this.OnText = value;
    }

    MyStringId IMyTerminalControlCheckbox.OffText
    {
      get => this.OffText;
      set => this.OffText = value;
    }

    MyStringId IMyTerminalControlTitleTooltip.Title
    {
      get => this.Title;
      set => this.Title = value;
    }

    MyStringId IMyTerminalControlTitleTooltip.Tooltip
    {
      get => this.Tooltip;
      set => this.Tooltip = value;
    }
  }
}
