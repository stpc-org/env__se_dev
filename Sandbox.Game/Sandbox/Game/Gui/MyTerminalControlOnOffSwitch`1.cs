// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControlOnOffSwitch`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
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
  public class MyTerminalControlOnOffSwitch<TBlock> : MyTerminalValueControl<TBlock, bool>, IMyTerminalControlOnOffSwitch, IMyTerminalControl, IMyTerminalValueControl<bool>, ITerminalProperty, IMyTerminalControlTitleTooltip
    where TBlock : MyTerminalBlock
  {
    private MyGuiControlOnOffSwitch m_onOffSwitch;
    public MyStringId Title;
    public MyStringId OnText;
    public MyStringId OffText;
    public MyStringId Tooltip;
    private float m_maxWidth = float.PositiveInfinity;
    private bool m_isAutoEllipsisEnabled;
    private bool m_isAutoScaleEnabled;
    private bool m_isButtonAutoScaleEnabled;
    private Action<MyGuiControlOnOffSwitch> m_valueChanged;

    public MyTerminalControlOnOffSwitch(
      string id,
      MyStringId title,
      MyStringId tooltip = default (MyStringId),
      MyStringId? on = null,
      MyStringId? off = null,
      float max_Width = float.PositiveInfinity,
      bool is_AutoEllipsisEnabled = false,
      bool is_AutoScaleEnabled = false)
      : base(id)
    {
      this.m_isAutoEllipsisEnabled = is_AutoEllipsisEnabled;
      this.m_isAutoScaleEnabled = is_AutoScaleEnabled;
      this.m_maxWidth = max_Width;
      this.Title = title;
      MyStringId? nullable = on;
      this.OnText = nullable ?? MySpaceTexts.SwitchText_On;
      nullable = off;
      this.OffText = nullable ?? MySpaceTexts.SwitchText_Off;
      this.Tooltip = tooltip;
      this.Serializer = (MyTerminalValueControl<TBlock, bool>.SerializerDelegate) ((BitStream stream, ref bool value) => stream.Serialize(ref value));
    }

    protected override MyGuiControlBase CreateGui()
    {
      if (MySession.Static.Config.Language == MyLanguagesEnum.Portuguese_Brazil)
        this.m_isButtonAutoScaleEnabled = true;
      this.m_onOffSwitch = new MyGuiControlOnOffSwitch(onText: MyTexts.GetString(this.OnText), offText: MyTexts.GetString(this.OffText), is_buttonAutoScaleEnabled: this.m_isButtonAutoScaleEnabled);
      this.m_onOffSwitch.SetToolTip(MyTexts.GetString(this.Tooltip));
      this.m_valueChanged = new Action<MyGuiControlOnOffSwitch>(this.OnValueChanged);
      this.m_onOffSwitch.ValueChanged += this.m_valueChanged;
      MyGuiControlBlockProperty controlBlockProperty = new MyGuiControlBlockProperty(MyTexts.GetString(this.Title), MyTexts.GetString(this.Tooltip), (MyGuiControlBase) this.m_onOffSwitch, showExtraInfo: false, isAutoEllipsisEnabled: this.m_isAutoEllipsisEnabled, isAutoScaleEnabled: this.m_isAutoScaleEnabled, max_Width: this.m_maxWidth);
      controlBlockProperty.Size = new Vector2(MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH, controlBlockProperty.Size.Y);
      return (MyGuiControlBase) controlBlockProperty;
    }

    private void OnValueChanged(MyGuiControlOnOffSwitch obj)
    {
      bool flag = obj.Value;
      foreach (TBlock targetBlock in this.TargetBlocks)
      {
        if (targetBlock.HasLocalPlayerAccess())
          this.SetValue(targetBlock, flag);
      }
    }

    protected override void OnUpdateVisual()
    {
      base.OnUpdateVisual();
      TBlock firstBlock = this.FirstBlock;
      if ((object) firstBlock != null)
      {
        this.m_onOffSwitch.ValueChanged -= this.m_valueChanged;
        this.m_onOffSwitch.Value = this.GetValue(firstBlock);
        this.m_onOffSwitch.ValueChanged += this.m_valueChanged;
      }
      this.m_onOffSwitch.Position = new Vector2((float) (0.5 * ((double) this.m_onOffSwitch.Size.X - (double) MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH)), this.m_onOffSwitch.Position.Y);
      this.m_onOffSwitch.RefreshInternals();
    }

    private void SwitchAction(TBlock block) => this.SetValue(block, !this.GetValue(block));

    private void OnAction(TBlock block) => this.SetValue(block, true);

    private void OffAction(TBlock block) => this.SetValue(block, false);

    private void Writer(
      TBlock block,
      StringBuilder result,
      StringBuilder onText,
      StringBuilder offText)
    {
      result.AppendStringBuilder(this.GetValue(block) ? onText : offText);
    }

    private void AppendAction(MyTerminalAction<TBlock> action)
    {
      MyTerminalAction<TBlock>[] array = this.Actions ?? new MyTerminalAction<TBlock>[0];
      Array.Resize<MyTerminalAction<TBlock>>(ref array, array.Length + 1);
      array[array.Length - 1] = action;
      this.Actions = array;
    }

    public MyTerminalAction<TBlock> EnableToggleAction(
      string icon,
      StringBuilder name,
      StringBuilder onText,
      StringBuilder offText)
    {
      MyTerminalAction<TBlock> action = new MyTerminalAction<TBlock>(this.Id, name, new Action<TBlock>(this.SwitchAction), (MyTerminalControl<TBlock>.WriterDelegate) ((x, r) => this.Writer(x, r, onText, offText)), icon);
      this.AppendAction(action);
      return action;
    }

    public MyTerminalAction<TBlock> EnableOnAction(
      string icon,
      StringBuilder name,
      StringBuilder onText,
      StringBuilder offText)
    {
      MyTerminalAction<TBlock> action = new MyTerminalAction<TBlock>(this.Id + "_On", name, new Action<TBlock>(this.OnAction), (MyTerminalControl<TBlock>.WriterDelegate) ((x, r) => this.Writer(x, r, onText, offText)), icon);
      this.AppendAction(action);
      return action;
    }

    public MyTerminalAction<TBlock> EnableOffAction(
      string icon,
      StringBuilder name,
      StringBuilder onText,
      StringBuilder offText)
    {
      MyTerminalAction<TBlock> action = new MyTerminalAction<TBlock>(this.Id + "_Off", name, new Action<TBlock>(this.OffAction), (MyTerminalControl<TBlock>.WriterDelegate) ((x, r) => this.Writer(x, r, onText, offText)), icon);
      this.AppendAction(action);
      return action;
    }

    public override bool GetDefaultValue(TBlock block) => false;

    public override bool GetMinimum(TBlock block) => false;

    public override bool GetMaximum(TBlock block) => true;

    public override void SetValue(TBlock block, bool value) => base.SetValue(block, value);

    public override bool GetValue(TBlock block) => base.GetValue(block);

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

    MyStringId IMyTerminalControlOnOffSwitch.OnText
    {
      get => this.OnText;
      set => this.OnText = value;
    }

    MyStringId IMyTerminalControlOnOffSwitch.OffText
    {
      get => this.OffText;
      set => this.OffText = value;
    }
  }
}
