// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlSearchBox
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyGuiControlSearchBox : MyGuiControlParent
  {
    private static readonly float m_offset = 0.004f;
    private MyGuiControlLabel m_label;
    private MyGuiControlTextbox m_textbox;
    private MyGuiControlButton m_clearButton;

    public MyGuiControlTextbox TextBox => this.m_textbox;

    public event MyGuiControlSearchBox.TextChangedDelegate OnTextChanged;

    public Vector4 SearchLabelColor
    {
      get => this.m_label != null ? this.m_label.ColorMask : Vector4.One;
      set
      {
        if (this.m_label == null)
          return;
        this.m_label.ColorMask = value;
      }
    }

    public string SearchText
    {
      get => this.m_textbox.Text;
      set => this.m_textbox.Text = value;
    }

    public MyGuiControlSearchBox(Vector2? position = null, Vector2? size = null, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
      : base(position, size)
    {
      this.OriginAlign = originAlign;
      this.m_textbox = new MyGuiControlTextbox();
      this.m_textbox.VisualStyle = MyGuiControlTextboxStyleEnum.Default;
      this.m_textbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_textbox.Position = new Vector2((float) (-(double) this.Size.X / 2.0), 0.0f);
      this.m_textbox.Size = new Vector2(this.Size.X, this.m_textbox.Size.Y);
      this.m_textbox.TextChanged += new Action<MyGuiControlTextbox>(this.m_textbox_TextChanged);
      this.Controls.Add((MyGuiControlBase) this.m_textbox);
      this.m_label = new MyGuiControlLabel();
      this.m_label.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_label.Position = new Vector2((float) (-(double) this.Size.X / 2.0 + 0.00749999983236194), MyGuiControlSearchBox.m_offset);
      this.m_label.Text = MyTexts.GetString(MyCommonTexts.ScreenMods_SearchLabel);
      this.m_label.Font = "DarkBlue";
      this.Controls.Add((MyGuiControlBase) this.m_label);
      MyGuiControlButton guiControlButton = new MyGuiControlButton();
      guiControlButton.Position = this.m_textbox.Position + new Vector2(this.m_textbox.Size.X - 0.005f, MyGuiControlSearchBox.m_offset);
      guiControlButton.Size = new Vector2(0.0234f, 0.029466f);
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlButton.ActivateOnMouseRelease = true;
      this.m_clearButton = guiControlButton;
      this.m_clearButton.VisualStyle = MyGuiControlButtonStyleEnum.Close;
      this.m_clearButton.Size = new Vector2(0.0234f, 0.029466f);
      this.m_clearButton.ButtonClicked += new Action<MyGuiControlButton>(this.m_clearButton_ButtonClicked);
      this.Controls.Add((MyGuiControlBase) this.m_clearButton);
      this.Size = new Vector2(this.Size.X, this.m_textbox.Size.Y);
      this.GamepadHelpTextId = MyCommonTexts.Gamepad_Help_SearchBox;
    }

    public MyGuiControlTextbox GetTextbox() => this.m_textbox;

    protected override void OnSizeChanged()
    {
      base.OnSizeChanged();
      this.m_label.Position = new Vector2((float) (-(double) this.Size.X / 2.0 + 0.00749999983236194), MyGuiControlSearchBox.m_offset);
      this.m_textbox.Position = new Vector2((float) (-(double) this.Size.X / 2.0), 0.0f);
      this.m_textbox.Size = this.Size;
      this.m_clearButton.Position = this.m_textbox.Position + new Vector2(this.m_textbox.Size.X - 0.005f, MyGuiControlSearchBox.m_offset);
    }

    private void m_textbox_TextChanged(MyGuiControlTextbox obj)
    {
      this.m_label.Visible = string.IsNullOrEmpty(obj.Text);
      if (this.OnTextChanged == null)
        return;
      this.OnTextChanged(obj.Text);
    }

    private void m_clearButton_ButtonClicked(MyGuiControlButton obj) => this.m_textbox.Text = "";

    public delegate void TextChangedDelegate(string newText);
  }
}
