// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenText
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenText : MyGuiScreenBase
  {
    private static readonly MyGuiScreenText.Style[] m_styles;
    private static Vector2 m_defaultWindowSize = new Vector2(0.6f, 0.7f);
    private static Vector2 m_defaultDescSize = new Vector2(0.5f, 0.44f);
    private Vector2 m_windowSize;
    protected Vector2 m_descSize;
    private string m_currentObjectivePrefix = "Current objective: ";
    private StringBuilder m_okButtonCaption;
    private string m_missionTitle = "Mission Title";
    private string m_currentObjective = "";
    protected string m_description = "";
    protected bool m_enableEdit;
    protected MyGuiControlLabel m_titleLabel;
    private MyGuiControlLabel m_currentObjectiveLabel;
    protected MyGuiControlMultilineText m_descriptionBox;
    protected MyGuiControlButton m_okButton;
    protected MyGuiControlCompositePanel m_descriptionBackgroundPanel;
    private Action<VRage.Game.ModAPI.ResultEnum> m_resultCallback;
    private VRage.Game.ModAPI.ResultEnum m_screenResult = VRage.Game.ModAPI.ResultEnum.CANCEL;
    private MyGuiScreenText.Style m_style;

    public MyGuiControlMultilineText Description => this.m_descriptionBox;

    static MyGuiScreenText()
    {
      MyGuiScreenText.m_styles = new MyGuiScreenText.Style[MyUtils.GetMaxValueFromEnum<MyMessageBoxStyleEnum>() + 1];
      MyGuiScreenText.m_styles[0] = new MyGuiScreenText.Style()
      {
        BackgroundTextureName = MyGuiConstants.TEXTURE_SCREEN_BACKGROUND_RED.Texture,
        CaptionFont = "White",
        TextFont = "White",
        ButtonStyle = MyGuiControlButtonStyleEnum.Red,
        ShowBackgroundPanel = false
      };
      MyGuiScreenText.m_styles[1] = new MyGuiScreenText.Style()
      {
        BackgroundTextureName = MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.Texture,
        CaptionFont = "White",
        TextFont = "Blue",
        ButtonStyle = MyGuiControlButtonStyleEnum.Default,
        ShowBackgroundPanel = false
      };
    }

    public MyGuiScreenText(
      string missionTitle = null,
      string currentObjectivePrefix = null,
      string currentObjective = null,
      string description = null,
      Action<VRage.Game.ModAPI.ResultEnum> resultCallback = null,
      string okButtonCaption = null,
      Vector2? windowSize = null,
      Vector2? descSize = null,
      bool editEnabled = false,
      bool canHideOthers = true,
      bool enableBackgroundFade = false,
      MyMissionScreenStyleEnum style = MyMissionScreenStyleEnum.BLUE)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(windowSize.HasValue ? windowSize.Value : MyGuiScreenText.m_defaultWindowSize))
    {
      this.m_style = MyGuiScreenText.m_styles[(int) style];
      this.m_enableEdit = editEnabled;
      this.m_descSize = descSize.HasValue ? descSize.Value : MyGuiScreenText.m_defaultDescSize;
      this.m_windowSize = windowSize.HasValue ? windowSize.Value : MyGuiScreenText.m_defaultWindowSize;
      this.m_missionTitle = missionTitle ?? this.m_missionTitle;
      this.m_currentObjectivePrefix = currentObjectivePrefix ?? this.m_currentObjectivePrefix;
      this.m_currentObjective = currentObjective ?? this.m_currentObjective;
      this.m_description = description ?? this.m_description;
      this.m_resultCallback = resultCallback;
      this.m_okButtonCaption = okButtonCaption != null ? new StringBuilder(okButtonCaption) : MyTexts.Get(MyCommonTexts.Ok);
      this.m_closeOnEsc = true;
      this.RecreateControls(true);
      this.m_titleLabel.Font = this.m_style.CaptionFont;
      this.m_currentObjectiveLabel.Font = this.m_style.CaptionFont;
      this.m_descriptionBox.Font = this.m_style.TextFont;
      this.m_backgroundTexture = this.m_style.BackgroundTextureName;
      this.m_okButton.VisualStyle = this.m_style.ButtonStyle;
      this.m_descriptionBackgroundPanel.Visible = this.m_style.ShowBackgroundPanel;
      this.m_isTopScreen = false;
      this.m_isTopMostScreen = false;
      this.CanHideOthers = canHideOthers;
      this.EnabledBackgroundFade = enableBackgroundFade;
    }

    public override string GetFriendlyName() => "MyGuiScreenMission";

    public override void RecreateControls(bool constructor)
    {
      Vector2 vector2_1 = new Vector2(0.0f, -0.3f);
      Vector2 descSize = this.m_descSize;
      Vector2 position1 = new Vector2((float) (-(double) descSize.X / 2.0), vector2_1.Y + 0.1f);
      Vector2 vector2_2 = new Vector2(0.2f, 0.3f);
      Vector2 vector2_3 = new Vector2(0.32f, 0.0f);
      Vector2 vector2_4 = new Vector2(0.005f, 0.0f);
      Vector2 vector2_5 = new Vector2(0.0f, vector2_1.Y + 0.05f);
      base.RecreateControls(constructor);
      this.CloseButtonEnabled = true;
      this.m_okButton = new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.29f)), size: new Vector2?(MyGuiConstants.BACK_BUTTON_SIZE), text: this.m_okButtonCaption, onButtonClick: new Action<MyGuiControlButton>(this.OkButtonClicked));
      this.m_okButton.GamepadHelpTextId = MyStringId.NullOrEmpty;
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      string missionTitle = this.m_missionTitle;
      this.m_titleLabel = new MyGuiControlLabel(new Vector2?(vector2_1), text: missionTitle, textScale: 1.5f, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      this.Controls.Add((MyGuiControlBase) this.m_titleLabel);
      Vector2? position2 = new Vector2?(vector2_5);
      Vector2? offset = new Vector2?();
      Vector2? size = offset;
      Vector4? colorMask = new Vector4?();
      this.m_currentObjectiveLabel = new MyGuiControlLabel(position2, size, colorMask: colorMask, textScale: 1f, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      this.Controls.Add((MyGuiControlBase) this.m_currentObjectiveLabel);
      this.SetCurrentObjective(this.m_currentObjective);
      this.m_descriptionBackgroundPanel = this.AddCompositePanel(MyGuiConstants.TEXTURE_RECTANGLE_DARK, position1, descSize, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      offset = new Vector2?(position1 + vector2_4);
      this.m_descriptionBox = this.AddMultilineText(new Vector2?(descSize), offset);
      this.m_descriptionBox.Text = new StringBuilder(this.m_description);
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(position1.X, this.m_okButton.Position.Y - 0.01f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MyPlatformGameSettings.IsMultilineEditableByGamepad ? MyCommonTexts.ScreenTextPanel_GamepadHelp_Xbox : MyCommonTexts.ScreenTextPanel_GamepadHelp);
    }

    protected MyGuiControlCompositePanel AddCompositePanel(
      MyGuiCompositeTexture texture,
      Vector2 position,
      Vector2 size,
      MyGuiDrawAlignEnum panelAlign)
    {
      MyGuiControlCompositePanel controlCompositePanel1 = new MyGuiControlCompositePanel();
      controlCompositePanel1.BackgroundTexture = texture;
      MyGuiControlCompositePanel controlCompositePanel2 = controlCompositePanel1;
      controlCompositePanel2.Position = position;
      controlCompositePanel2.Size = size;
      controlCompositePanel2.OriginAlign = panelAlign;
      this.Controls.Add((MyGuiControlBase) controlCompositePanel2);
      return controlCompositePanel2;
    }

    protected virtual MyGuiControlMultilineText AddMultilineText(
      Vector2? size = null,
      Vector2? offset = null,
      float textScale = 1f,
      bool selectable = false,
      MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
      MyGuiDrawAlignEnum textBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
    {
      Vector2 vector2 = size ?? this.Size ?? new Vector2(1.2f, 0.5f);
      MyGuiControlMultilineText controlMultilineText = !this.m_enableEdit ? new MyGuiControlMultilineText(new Vector2?(vector2 / 2f + (offset ?? Vector2.Zero)), new Vector2?(vector2), new Vector4?(Color.White.ToVector4()), "White", textAlign: textAlign, textBoxAlign: textBoxAlign, selectable: this.m_enableEdit) : (MyGuiControlMultilineText) new MyGuiControlMultilineEditableText(new Vector2?(vector2 / 2f + (offset ?? Vector2.Zero)), new Vector2?(vector2), new Vector4?(Color.White.ToVector4()), "White", textAlign: textAlign, textBoxAlign: textBoxAlign);
      this.Controls.Add((MyGuiControlBase) controlMultilineText);
      return controlMultilineText;
    }

    private void OkButtonClicked(MyGuiControlButton button)
    {
      this.m_screenResult = VRage.Game.ModAPI.ResultEnum.OK;
      this.CloseScreen(false);
    }

    public void SetTitle(string title)
    {
      this.m_missionTitle = title;
      this.m_titleLabel.Text = title;
    }

    public void SetCurrentObjective(string objective)
    {
      this.m_currentObjective = objective;
      this.m_currentObjectiveLabel.Text = this.m_currentObjectivePrefix + this.m_currentObjective;
    }

    public void SetDescription(string desc)
    {
      this.m_description = desc;
      this.m_descriptionBox.Clear();
      this.m_descriptionBox.Text = new StringBuilder(this.m_description);
    }

    public void AppendTextToDescription(string text, Vector4 color, string font = "White", float scale = 1f)
    {
      this.m_description += text;
      this.m_descriptionBox.AppendText(text, font, scale, color);
    }

    public void AppendTextToDescription(string text, string font = "White", float scale = 1f)
    {
      this.m_description += text;
      this.m_descriptionBox.AppendText(text, font, scale, Vector4.One);
    }

    public void SetCurrentObjectivePrefix(string prefix) => this.m_currentObjectivePrefix = prefix;

    public void SetOkButtonCaption(string caption) => this.m_okButtonCaption = new StringBuilder(caption);

    protected override void Canceling()
    {
      base.Canceling();
      this.m_screenResult = VRage.Game.ModAPI.ResultEnum.CANCEL;
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      this.CallResultCallback(this.m_screenResult);
      return base.CloseScreen(isUnloading);
    }

    protected void CallResultCallback(VRage.Game.ModAPI.ResultEnum result)
    {
      if (this.m_resultCallback == null)
        return;
      this.m_resultCallback(result);
    }

    public override bool Update(bool hasFocus)
    {
      if (this.m_gamepadHelpLabel != null)
        this.m_okButton.Visible = !this.m_gamepadHelpLabel.Visible;
      return base.Update(hasFocus);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      if (!receivedFocusInThisUpdate && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OkButtonClicked((MyGuiControlButton) null);
      base.HandleInput(receivedFocusInThisUpdate);
    }

    public class Style
    {
      public string BackgroundTextureName;
      public string CaptionFont;
      public string TextFont;
      public MyGuiControlButtonStyleEnum ButtonStyle;
      public bool ShowBackgroundPanel;
    }
  }
}
