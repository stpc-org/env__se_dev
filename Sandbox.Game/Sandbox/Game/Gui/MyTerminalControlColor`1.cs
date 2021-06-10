// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControlColor`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using VRage;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyTerminalControlColor<TBlock> : MyTerminalValueControl<TBlock, Color>, IMyTerminalControlColor, IMyTerminalControl, IMyTerminalValueControl<Color>, ITerminalProperty, IMyTerminalControlTitleTooltip
    where TBlock : MyTerminalBlock
  {
    public MyStringId Title;
    public MyStringId Tooltip;
    private bool m_isAutoscaleEnabled;
    private bool m_isAutoEllipsisEnabled;
    private float m_maxTitleWidth;
    private MyGuiControlColor m_color;
    private Action<MyGuiControlColor> m_changeColor;

    public MyTerminalControlColor(
      string id,
      MyStringId title,
      bool isAutoscaleEnabled = false,
      float maxTitleWidth = 1f,
      bool isAutosEllipsisEnabled = false)
      : base(id)
    {
      this.Title = title;
      this.m_isAutoscaleEnabled = isAutoscaleEnabled;
      this.m_isAutoEllipsisEnabled = isAutosEllipsisEnabled;
      this.m_maxTitleWidth = maxTitleWidth;
      this.Serializer = (MyTerminalValueControl<TBlock, Color>.SerializerDelegate) ((BitStream stream, ref Color value) => stream.Serialize(ref value.PackedValue));
    }

    protected override MyGuiControlBase CreateGui()
    {
      string text = MyTexts.Get(this.Title).ToString();
      Vector2 zero = Vector2.Zero;
      Color white1 = Color.White;
      Color white2 = Color.White;
      MyStringId amountSetValueCaption = MyCommonTexts.DialogAmount_SetValueCaption;
      int num1 = this.m_isAutoscaleEnabled ? 1 : 0;
      float maxTitleWidth = this.m_maxTitleWidth;
      int num2 = this.m_isAutoEllipsisEnabled ? 1 : 0;
      double num3 = (double) maxTitleWidth;
      this.m_color = new MyGuiControlColor(text, 0.95f, zero, white1, white2, amountSetValueCaption, true, isAutoscaleEnabled: (num1 != 0), isAutoEllipsisEnabled: (num2 != 0), maxTitleWidth: ((float) num3));
      this.m_changeColor = new Action<MyGuiControlColor>(this.OnChangeColor);
      this.m_color.OnChange += this.m_changeColor;
      this.m_color.Size = new Vector2(MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH, this.m_color.Size.Y);
      return (MyGuiControlBase) new MyGuiControlBlockProperty(string.Empty, string.Empty, (MyGuiControlBase) this.m_color);
    }

    private void OnChangeColor(MyGuiControlColor obj)
    {
      foreach (TBlock targetBlock in this.TargetBlocks)
        this.SetValue(targetBlock, obj.GetColor());
    }

    protected override void OnUpdateVisual()
    {
      base.OnUpdateVisual();
      TBlock firstBlock = this.FirstBlock;
      if ((object) firstBlock == null)
        return;
      this.m_color.OnChange -= this.m_changeColor;
      this.m_color.SetColor(this.GetValue(firstBlock));
      this.m_color.OnChange += this.m_changeColor;
    }

    public override void SetValue(TBlock block, Color value) => base.SetValue(block, new Color(Vector4.Clamp(value.ToVector4(), Vector4.Zero, Vector4.One)));

    public override Color GetDefaultValue(TBlock block) => new Color(Vector4.One);

    public override Color GetMinimum(TBlock block) => new Color(Vector4.Zero);

    public override Color GetMaximum(TBlock block) => new Color(Vector4.One);

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
