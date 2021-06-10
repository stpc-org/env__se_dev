// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlOnOffSwitch
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  [MyGuiControlType(typeof (MyObjectBuilder_GuiControlOnOffSwitch))]
  public class MyGuiControlOnOffSwitch : MyGuiControlBase
  {
    private static MyGuiControlOnOffSwitch.StyleDefinition[] m_styles = new MyGuiControlOnOffSwitch.StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlOnOffSwitchStyleEnum>() + 1];
    private MyGuiControlOnOffSwitchStyleEnum m_visualStyle;
    private MyGuiControlOnOffSwitch.StyleDefinition m_styleDef;
    private MyGuiControlCheckbox m_onButton;
    private MyGuiControlLabel m_onLabel;
    private MyGuiControlCheckbox m_offButton;
    private MyGuiControlLabel m_offLabel;
    private bool m_value;

    static MyGuiControlOnOffSwitch() => MyGuiControlOnOffSwitch.m_styles[0] = new MyGuiControlOnOffSwitch.StyleDefinition()
    {
      NormalTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER,
      HighlightTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER_HIGHLIGHT,
      FocusTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER_FOCUS
    };

    public static MyGuiControlOnOffSwitch.StyleDefinition GetVisualStyle(
      MyGuiControlOnOffSwitchStyleEnum style)
    {
      return MyGuiControlOnOffSwitch.m_styles[(int) style];
    }

    public MyGuiControlOnOffSwitchStyleEnum VisualStyle
    {
      get => this.m_visualStyle;
      set
      {
        this.m_visualStyle = value;
        this.RefreshVisualStyle();
      }
    }

    private void RefreshVisualStyle()
    {
      this.m_styleDef = MyGuiControlOnOffSwitch.GetVisualStyle(this.VisualStyle);
      this.RefreshInternals();
    }

    public void RefreshInternals()
    {
      if (this.HasHighlight)
        this.BackgroundTexture = this.m_styleDef.HighlightTexture;
      else if (this.HasFocus)
        this.BackgroundTexture = this.m_styleDef.FocusTexture ?? this.m_styleDef.HighlightTexture;
      else
        this.BackgroundTexture = this.m_styleDef.NormalTexture;
      this.m_onButton.Refresh();
      this.m_offButton.Refresh();
    }

    protected override void OnHasHighlightChanged()
    {
      base.OnHasHighlightChanged();
      this.RefreshInternals();
    }

    public override void OnFocusChanged(bool focus)
    {
      base.OnFocusChanged(focus);
      this.RefreshInternals();
    }

    public bool Value
    {
      get => this.m_value;
      set
      {
        if (this.m_value == value)
          return;
        this.m_value = value;
        this.UpdateButtonState();
        if (this.ValueChanged == null)
          return;
        this.ValueChanged(this);
      }
    }

    public event Action<MyGuiControlOnOffSwitch> ValueChanged;

    public MyGuiControlOnOffSwitch(
      bool initialValue = false,
      string onText = null,
      string offText = null,
      bool is_buttonAutoScaleEnabled = false)
      : base(canHaveFocus: true)
    {
      this.m_onButton = new MyGuiControlCheckbox(visualStyle: MyGuiControlCheckboxStyleEnum.SwitchOnOffLeft, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      this.m_offButton = new MyGuiControlCheckbox(visualStyle: MyGuiControlCheckboxStyleEnum.SwitchOnOffRight, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      float x = this.m_offButton.Size.X;
      this.m_onLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_onButton.Size.X * -0.5f, 0.0f)), text: (onText ?? MyTexts.GetString(MySpaceTexts.SwitchText_On)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, maxWidth: x, isAutoScaleEnabled: is_buttonAutoScaleEnabled);
      this.m_offLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_onButton.Size.X * 0.5f, 0.0f)), text: (offText ?? MyTexts.GetString(MySpaceTexts.SwitchText_Off)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, maxWidth: x, isAutoScaleEnabled: is_buttonAutoScaleEnabled);
      this.VisualStyle = MyGuiControlOnOffSwitchStyleEnum.Default;
      this.Size = new Vector2(this.m_onButton.Size.X + this.m_offButton.Size.X + 3f / 1000f, Math.Max(this.m_onButton.Size.Y, this.m_offButton.Size.Y));
      this.Elements.Add((MyGuiControlBase) this.m_onButton);
      this.Elements.Add((MyGuiControlBase) this.m_offButton);
      this.Elements.Add((MyGuiControlBase) this.m_onLabel);
      this.Elements.Add((MyGuiControlBase) this.m_offLabel);
      this.m_value = initialValue;
      this.UpdateButtonState();
    }

    public override void Init(MyObjectBuilder_GuiControlBase builder)
    {
      base.Init(builder);
      this.Size = new Vector2(this.m_onButton.Size.X + this.m_offButton.Size.X, Math.Max(this.m_onButton.Size.Y, this.m_offButton.Size.Y));
      this.UpdateButtonState();
    }

    public override MyGuiControlBase HandleInput()
    {
      MyGuiControlBase myGuiControlBase = base.HandleInput();
      if (myGuiControlBase != null)
        return myGuiControlBase;
      if (!this.IsMouseOver)
      {
        int num = this.HasFocus ? 1 : 0;
      }
      bool flag = MyInput.Static.IsNewLeftMouseReleased() || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT, MyControlStateType.NEW_RELEASED);
      if (((!this.Enabled ? 0 : (this.IsMouseOver ? 1 : 0)) & (flag ? 1 : 0)) != 0 || this.Enabled && this.HasFocus && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01)))
      {
        this.Value = !this.Value;
        myGuiControlBase = (MyGuiControlBase) this;
        MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
      }
      return myGuiControlBase;
    }

    private void UpdateButtonState()
    {
      this.m_onButton.IsChecked = this.Value;
      this.m_offButton.IsChecked = !this.Value;
      this.m_onLabel.Font = this.Value ? "White" : "Blue";
      this.m_offLabel.Font = this.Value ? "Blue" : "White";
    }

    protected override void OnVisibleChanged()
    {
      if (this.m_onButton != null)
        this.m_onButton.Visible = this.Visible;
      if (this.m_offButton != null)
        this.m_offButton.Visible = this.Visible;
      base.OnVisibleChanged();
    }

    public class StyleDefinition
    {
      public MyGuiCompositeTexture NormalTexture;
      public MyGuiCompositeTexture HighlightTexture;
      public MyGuiCompositeTexture FocusTexture;
    }
  }
}
