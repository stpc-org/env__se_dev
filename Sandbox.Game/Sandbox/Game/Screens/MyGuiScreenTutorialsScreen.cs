// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenTutorialsScreen
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenTutorialsScreen : MyGuiScreenBase
  {
    private MyGuiControlButton m_okBtn;
    private MyGuiControlCheckbox m_dontShowAgainCheckbox;
    private const string m_linkImgTex = "Textures\\GUI\\link.dds";
    private Action m_okAction;

    public MyGuiScreenTutorialsScreen(Action okAction)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.5264286f, 0.6679389f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      MySandboxGame.Log.WriteLine("MyGuiScreenWelcomeScreen.ctor START");
      this.m_okAction = okAction;
      this.EnabledBackgroundFade = true;
      this.m_closeOnEsc = true;
      this.m_drawEvenWithoutFocus = true;
      this.CanHideOthers = true;
      this.CanBeHidden = true;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.BuildControls();
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenTutorialsScreen);

    protected void BuildControls()
    {
      this.AddCaption("Tutorials", captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      int newsletterCurrentStatus = (int) MySandboxGame.Config.NewsletterCurrentStatus;
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(-new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.79f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(-new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.79f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      float num1 = 0.145f;
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(new Vector2?(new Vector2(0.015f, num1 - 0.162f)), new Vector2?(new Vector2(0.44f, 0.45f)), textScale: 0.76f, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      controlMultilineText.AppendText(MyTexts.GetString(MyCommonTexts.HelpScreen_HelloEngineer), "Blue", 0.76f, (Vector4) Color.White);
      controlMultilineText.AppendText("\n\n", "Blue", 0.76f, (Vector4) Color.White);
      controlMultilineText.OnLinkClicked += new LinkClicked(this.OnLinkClicked);
      controlMultilineText.AppendText(MyTexts.GetString(MyCommonTexts.HelpScreen_AccessHelpScreen), "Blue", 0.76f, (Vector4) Color.White);
      bool joystickLastUsed = MyInput.Static.IsJoystickLastUsed;
      float num2 = 0.02f;
      float x = -0.205f;
      float y1 = -0.04f;
      if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
      {
        this.Controls.Add((MyGuiControlBase) this.MakeURLButton(new Vector2(x, y1), MyTexts.GetString(MyCommonTexts.HelpScreen_Introduction), joystickLastUsed, 0));
        y1 += num2;
      }
      this.Controls.Add((MyGuiControlBase) this.MakeURLButton(new Vector2(x, y1), MyTexts.GetString(MyCommonTexts.HelpScreen_BasicControls), joystickLastUsed, 1));
      float y2 = y1 + num2;
      this.Controls.Add((MyGuiControlBase) this.MakeURLButton(new Vector2(x, y2), MyTexts.GetString(MyCommonTexts.HelpScreen_PossibilitiesWithinTheGameModes), joystickLastUsed, 2));
      float y3 = y2 + num2;
      this.Controls.Add((MyGuiControlBase) this.MakeURLButton(new Vector2(x, y3), MyTexts.GetString(MyCommonTexts.HelpScreen_DrillingRefiningAssemblingSurvival), joystickLastUsed, 3));
      float y4 = y3 + num2;
      this.Controls.Add((MyGuiControlBase) this.MakeURLButton(new Vector2(x, y4), MyTexts.GetString(MyCommonTexts.HelpScreen_BuildingYour1stShipCreative), joystickLastUsed, 4));
      float y5 = y4 + num2;
      this.Controls.Add((MyGuiControlBase) this.MakeURLButton(new Vector2(x, y5), MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival), joystickLastUsed, 9));
      float y6 = y5 + num2;
      this.Controls.Add((MyGuiControlBase) this.MakeURLButton(new Vector2(x, y6), MyTexts.GetString(MyCommonTexts.ExperimentalMode), joystickLastUsed, 5));
      float y7 = y6 + num2;
      this.Controls.Add((MyGuiControlBase) this.MakeURLButton(new Vector2(x, y7), MyTexts.GetString(MyCommonTexts.HelpScreen_BuildingYour1stGroundVehicle), joystickLastUsed, 6));
      float y8 = y7 + num2;
      this.Controls.Add((MyGuiControlBase) this.MakeURLButton(new Vector2(x, y8), MyTexts.GetString(MyCommonTexts.HelpScreen_OtherAdviceClosingThoughts), joystickLastUsed, 8));
      this.m_dontShowAgainCheckbox = new MyGuiControlCheckbox(new Vector2?(new Vector2(0.08f, 0.017f + num1)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
      this.Controls.Add((MyGuiControlBase) this.m_dontShowAgainCheckbox);
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(0.195f, 0.047f + num1)), text: MyTexts.GetString(MyCommonTexts.HelpScreen_DontShowAgain));
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      Vector2 backButtonSize = MyGuiConstants.BACK_BUTTON_SIZE;
      this.m_okBtn = new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.155f + num1)), size: new Vector2?(backButtonSize), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnOKButtonClick));
      this.m_okBtn.Enabled = true;
      this.m_okBtn.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
      this.Controls.Add((MyGuiControlBase) controlMultilineText);
      this.Controls.Add((MyGuiControlBase) this.m_okBtn);
      this.CloseButtonEnabled = true;
    }

    private MyGuiControlButton MakeURLButton(
      Vector2 position,
      string text,
      bool isForController,
      int tutorialPart)
    {
      MyGuiControlButton guiControlButton = new MyGuiControlButton(new Vector2?(position), colorMask: new Vector4?(MyGuiConstants.BACK_BUTTON_BACKGROUND_COLOR), text: new StringBuilder(text), onButtonClick: ((Action<MyGuiControlButton>) (x => MyGuiSandbox.OpenUrl(MyGuiScreenHelpSpace.GetTutorialPartUrl(tutorialPart, isForController), UrlOpenMode.SteamOrExternalWithConfirm))));
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      guiControlButton.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      guiControlButton.Alpha = 1f;
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
      guiControlButton.Size = new Vector2(0.22f, 0.13f);
      guiControlButton.TextScale = 0.736f;
      guiControlButton.CanHaveFocus = true;
      guiControlButton.ColorMask = (Vector4) Color.PowderBlue;
      Vector2 vector2 = MyGuiManager.MeasureString(guiControlButton.TextFont, new StringBuilder(guiControlButton.Text), guiControlButton.TextScale);
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage();
      myGuiControlImage1.Size = new Vector2(0.0128f, 11f / 625f);
      myGuiControlImage1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      myGuiControlImage1.Position = guiControlButton.Position;
      myGuiControlImage1.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
      MyGuiControlImage myGuiControlImage2 = myGuiControlImage1;
      myGuiControlImage2.PositionX += vector2.X + 0.005f;
      myGuiControlImage2.SetTexture("Textures\\GUI\\link.dds");
      myGuiControlImage2.ColorMask = (Vector4) Color.White;
      myGuiControlImage2.Visible = true;
      this.Controls.Add((MyGuiControlBase) myGuiControlImage2);
      return guiControlButton;
    }

    private void OnOKButtonClick(object sender)
    {
      MySandboxGame.Config.FirstTimeTutorials = !this.m_dontShowAgainCheckbox.IsChecked;
      MySandboxGame.Config.Save();
      this.CloseScreen();
      this.m_okAction();
    }

    protected override void Canceling()
    {
      this.m_okAction();
      base.Canceling();
    }

    private void OnLinkClicked(MyGuiControlBase sender, string url) => MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);
  }
}
