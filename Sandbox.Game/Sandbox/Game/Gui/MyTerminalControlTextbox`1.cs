// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControlTextbox`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
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
  public class MyTerminalControlTextbox<TBlock> : MyTerminalValueControl<TBlock, StringBuilder>, ITerminalControlSync, IMyTerminalControlTextbox, IMyTerminalControl, IMyTerminalValueControl<StringBuilder>, ITerminalProperty, IMyTerminalControlTitleTooltip
    where TBlock : MyTerminalBlock
  {
    private char[] m_tmpArray = new char[64];
    private MyGuiControlTextbox m_textbox;
    public MyTerminalControlTextbox<TBlock>.SerializerDelegate Serializer;
    public MyStringId Title;
    public MyStringId Tooltip;
    private StringBuilder m_tmpText = new StringBuilder(15);
    private Action<MyGuiControlTextbox> m_textChanged;

    public MyTerminalControlTextbox<TBlock>.GetterDelegate Getter { private get; set; }

    public MyTerminalControlTextbox<TBlock>.SetterDelegate Setter { private get; set; }

    public MyTerminalControlTextbox<TBlock>.SetterDelegate EnterPressed { get; set; }

    public MyTerminalControlTextbox(string id, MyStringId title, MyStringId tooltip)
      : base(id)
    {
      this.Title = title;
      this.Tooltip = tooltip;
      this.Serializer = (MyTerminalControlTextbox<TBlock>.SerializerDelegate) ((s, sb) => s.Serialize(sb, ref this.m_tmpArray, Encoding.UTF8));
    }

    public new void Serialize(BitStream stream, MyTerminalBlock block)
    {
    }

    protected override MyGuiControlBase CreateGui()
    {
      this.m_textbox = new MyGuiControlTextbox();
      this.m_textbox.Size = new Vector2(MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH, this.m_textbox.Size.Y);
      this.m_textChanged = new Action<MyGuiControlTextbox>(this.OnTextChanged);
      this.m_textbox.TextChanged += this.m_textChanged;
      this.m_textbox.EnterPressed += new Action<MyGuiControlTextbox>(this.OnEnterPressed);
      MyGuiControlBlockProperty controlBlockProperty = new MyGuiControlBlockProperty(MyTexts.GetString(this.Title), MyTexts.GetString(this.Tooltip), (MyGuiControlBase) this.m_textbox);
      controlBlockProperty.Size = new Vector2(MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH, controlBlockProperty.Size.Y);
      return (MyGuiControlBase) controlBlockProperty;
    }

    private void OnTextChanged(MyGuiControlTextbox obj)
    {
      this.m_tmpText.Clear();
      obj.GetText(this.m_tmpText);
      foreach (TBlock targetBlock in this.TargetBlocks)
        this.SetValue(targetBlock, this.m_tmpText);
    }

    private void OnEnterPressed(MyGuiControlTextbox obj)
    {
      if (this.EnterPressed == null)
        return;
      foreach (TBlock targetBlock in this.TargetBlocks)
        this.EnterPressed(targetBlock, this.m_tmpText);
    }

    protected override void OnUpdateVisual()
    {
      base.OnUpdateVisual();
      if (this.m_textbox.IsImeActive)
        return;
      TBlock firstBlock = this.FirstBlock;
      if ((object) firstBlock == null)
        return;
      StringBuilder stringBuilder = this.GetValue(firstBlock);
      if (this.m_textbox.TextEquals(stringBuilder))
        return;
      this.m_textbox.TextChanged -= this.m_textChanged;
      this.m_textbox.SetText(stringBuilder);
      this.m_textbox.TextChanged += this.m_textChanged;
    }

    public override StringBuilder GetValue(TBlock block) => this.Getter(block);

    public override void SetValue(TBlock block, StringBuilder value)
    {
      if (this.Setter != null)
        this.Setter(block, new StringBuilder(value.ToString()));
      block.NotifyTerminalValueChanged((ITerminalControl) this);
    }

    public override StringBuilder GetDefaultValue(TBlock block) => new StringBuilder();

    public override StringBuilder GetMinimum(TBlock block) => new StringBuilder();

    public override StringBuilder GetMaximum(TBlock block) => new StringBuilder();

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

    Func<IMyTerminalBlock, StringBuilder> IMyTerminalValueControl<StringBuilder>.Getter
    {
      get
      {
        MyTerminalControlTextbox<TBlock>.GetterDelegate oldGetter = this.Getter;
        return (Func<IMyTerminalBlock, StringBuilder>) (x => oldGetter((TBlock) x));
      }
      set => this.Getter = new MyTerminalControlTextbox<TBlock>.GetterDelegate(value.Invoke);
    }

    Action<IMyTerminalBlock, StringBuilder> IMyTerminalValueControl<StringBuilder>.Setter
    {
      get
      {
        MyTerminalControlTextbox<TBlock>.SetterDelegate oldSetter = this.Setter;
        return (Action<IMyTerminalBlock, StringBuilder>) ((x, y) => oldSetter((TBlock) x, y));
      }
      set => this.Setter = new MyTerminalControlTextbox<TBlock>.SetterDelegate(value.Invoke);
    }

    public new delegate StringBuilder GetterDelegate(TBlock block) where TBlock : MyTerminalBlock;

    public new delegate void SetterDelegate(TBlock block, StringBuilder value) where TBlock : MyTerminalBlock;

    public new delegate void SerializerDelegate(BitStream stream, StringBuilder value) where TBlock : MyTerminalBlock;
  }
}
