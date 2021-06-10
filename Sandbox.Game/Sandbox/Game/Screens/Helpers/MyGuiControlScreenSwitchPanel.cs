// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyGuiControlScreenSwitchPanel
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Networking;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyGuiControlScreenSwitchPanel : MyGuiControlParent
  {
    private List<Action> m_screenSwithingActions = new List<Action>();
    private List<bool> m_screenEnabled = new List<bool>();
    private int m_activeScreenIndex = -1;
    private const int PAGE_COUNT = 3;
    private Vector2 m_gamepadHelpTopLeftPos;

    public MyGuiControlScreenSwitchPanel(
      MyGuiScreenBase owner,
      StringBuilder ownerDescription,
      bool displayTabScenario = true,
      bool displayTabWorkshop = true,
      bool displayTabCustom = true)
    {
      this.m_screenSwithingActions.Add(new Action(this.OpenCampaignScreen));
      this.m_screenSwithingActions.Add(new Action(this.OpenWorkshopScreen));
      this.m_screenSwithingActions.Add(new Action(this.OpenCustomWorldScreen));
      this.m_screenEnabled.Add(displayTabScenario);
      this.m_screenEnabled.Add(displayTabWorkshop);
      this.m_screenEnabled.Add(displayTabCustom);
      Vector2 vector2_1 = new Vector2(1f / 500f, 0.05f);
      Vector2 vector2_2 = new Vector2(50f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      Vector2 vector2_3 = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      MyGuiControlMultilineText controlMultilineText1 = new MyGuiControlMultilineText();
      controlMultilineText1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlMultilineText1.Position = new Vector2(1f / 500f, 0.13f);
      controlMultilineText1.Size = new Vector2(owner.Size.Value.X - 0.1f, 0.07f);
      controlMultilineText1.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlMultilineText1.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlMultilineText1.Text = ownerDescription;
      controlMultilineText1.Font = "Blue";
      MyGuiControlMultilineText controlMultilineText2 = controlMultilineText1;
      Vector2? position1 = new Vector2?(vector2_1);
      Vector2? size1 = new Vector2?();
      Vector4? colorMask1 = new Vector4?();
      StringBuilder stringBuilder1 = MyTexts.Get(MyCommonTexts.ScreenCaptionNewGame);
      string toolTip1 = MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Campaign);
      StringBuilder text1 = stringBuilder1;
      Action<MyGuiControlButton> onButtonClick1 = new Action<MyGuiControlButton>(this.OnCampaignButtonClick);
      int? buttonIndex1 = new int?();
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton(position1, MyGuiControlButtonStyleEnum.ToolbarButton, size1, colorMask1, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, toolTip1, text1, onButtonClick: onButtonClick1, buttonIndex: buttonIndex1);
      guiControlButton1.CanHaveFocus = false;
      MyGuiControlButton guiControlButton2 = guiControlButton1;
      guiControlButton2.Enabled = displayTabScenario;
      vector2_1.X += guiControlButton2.Size.X + MyGuiConstants.GENERIC_BUTTON_SPACING.X;
      Vector2? position2 = new Vector2?(vector2_1);
      Vector2? size2 = new Vector2?();
      Vector4? colorMask2 = new Vector4?();
      StringBuilder stringBuilder2 = MyTexts.Get(MyCommonTexts.ScreenCaptionWorkshop);
      string toolTip2 = MyTexts.GetString(MySpaceTexts.ToolTipNewGame_WorkshopContent);
      StringBuilder text2 = stringBuilder2;
      Action<MyGuiControlButton> onButtonClick2 = new Action<MyGuiControlButton>(this.OnWorkshopButtonClick);
      int? buttonIndex2 = new int?();
      MyGuiControlButton guiControlButton3 = new MyGuiControlButton(position2, MyGuiControlButtonStyleEnum.ToolbarButton, size2, colorMask2, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, toolTip2, text2, onButtonClick: onButtonClick2, buttonIndex: buttonIndex2);
      guiControlButton3.CanHaveFocus = false;
      MyGuiControlButton guiControlButton4 = guiControlButton3;
      guiControlButton4.Enabled = displayTabWorkshop;
      vector2_1.X += guiControlButton4.Size.X + MyGuiConstants.GENERIC_BUTTON_SPACING.X;
      Vector2? position3 = new Vector2?(vector2_1);
      Vector2? size3 = new Vector2?();
      Vector4? colorMask3 = new Vector4?();
      StringBuilder stringBuilder3 = MyTexts.Get(MyCommonTexts.ScreenCaptionCustomWorld);
      string toolTip3 = MyTexts.GetString(MySpaceTexts.ToolTipNewGame_CustomGame);
      StringBuilder text3 = stringBuilder3;
      Action<MyGuiControlButton> onButtonClick3 = new Action<MyGuiControlButton>(this.OnCustomWorldButtonClick);
      int? buttonIndex3 = new int?();
      MyGuiControlButton guiControlButton5 = new MyGuiControlButton(position3, MyGuiControlButtonStyleEnum.ToolbarButton, size3, colorMask3, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, toolTip3, text3, onButtonClick: onButtonClick3, buttonIndex: buttonIndex3, isAutoscaleEnabled: true);
      guiControlButton5.CanHaveFocus = false;
      MyGuiControlButton guiControlButton6 = guiControlButton5;
      guiControlButton6.Enabled = displayTabCustom;
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0305f), owner.Size.Value.X - 2f * vector2_3.X);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.098f), owner.Size.Value.X - 2f * vector2_3.X);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.166f), owner.Size.Value.X - 2f * vector2_3.X);
      switch (owner)
      {
        case MyGuiScreenNewGame _:
          owner.FocusedControl = (MyGuiControlBase) guiControlButton2;
          guiControlButton2.Checked = true;
          guiControlButton2.Selected = true;
          guiControlButton2.Name = "CampaignButton";
          this.m_activeScreenIndex = 0;
          break;
        case MyGuiScreenWorldSettings _:
          owner.FocusedControl = (MyGuiControlBase) guiControlButton6;
          guiControlButton6.Checked = true;
          guiControlButton6.Selected = true;
          this.m_activeScreenIndex = 2;
          break;
        case MyGuiScreenLoadSubscribedWorld _:
        case MyGuiScreenNewWorkshopGame _:
          if (guiControlButton4 != null)
          {
            owner.FocusedControl = (MyGuiControlBase) guiControlButton4;
            guiControlButton4.Checked = true;
            guiControlButton4.Selected = true;
            this.m_activeScreenIndex = 1;
            break;
          }
          break;
      }
      this.Controls.Add((MyGuiControlBase) controlMultilineText2);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      this.Controls.Add((MyGuiControlBase) guiControlButton2);
      this.Controls.Add((MyGuiControlBase) guiControlButton6);
      if (guiControlButton4 != null)
        this.Controls.Add((MyGuiControlBase) guiControlButton4);
      this.Position = -owner.Size.Value / 2f + new Vector2(vector2_3.X, vector2_2.Y);
      this.Size = new Vector2(1f, 0.2f);
      owner.Controls.Add((MyGuiControlBase) this);
      this.m_gamepadHelpTopLeftPos = guiControlButton2.GetPositionAbsoluteTopLeft();
    }

    public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      base.Draw(transitionAlpha, backgroundTransitionAlpha);
      if (!MyInput.Static.IsJoystickLastUsed)
        return;
      MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      Vector2 vector2 = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.ToolbarButton).SizeOverride.Value;
      Vector2 gamepadHelpTopLeftPos1 = this.m_gamepadHelpTopLeftPos;
      gamepadHelpTopLeftPos1.Y += vector2.Y / 2f;
      gamepadHelpTopLeftPos1.X -= vector2.X / 6f;
      Vector2 gamepadHelpTopLeftPos2 = this.m_gamepadHelpTopLeftPos;
      gamepadHelpTopLeftPos2.Y = gamepadHelpTopLeftPos1.Y;
      Color color = MyGuiControlBase.ApplyColorMaskModifiers(MyGuiConstants.LABEL_TEXT_COLOR, true, transitionAlpha);
      gamepadHelpTopLeftPos2.X += (float) (3.0 * (double) vector2.X + (double) vector2.X / 8.0);
      MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Left), gamepadHelpTopLeftPos1, 1f, new Color?(color), drawAlign);
      MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Right), gamepadHelpTopLeftPos2, 1f, new Color?(color), drawAlign);
    }

    private void OnCampaignButtonClick(MyGuiControlButton myGuiControlButton) => this.OpenCampaignScreen();

    private void OpenCampaignScreen()
    {
      MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
      if (screenWithFocus is MyGuiScreenNewGame)
        return;
      MyGuiControlScreenSwitchPanel.SeamlesslyChangeScreen(screenWithFocus, (MyGuiScreenBase) new MyGuiScreenNewGame(this.m_screenEnabled[0], this.m_screenEnabled[1], this.m_screenEnabled[2]));
    }

    private void OnCustomWorldButtonClick(MyGuiControlButton myGuiControlButton) => this.OpenCustomWorldScreen();

    private void OpenCustomWorldScreen()
    {
      MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
      if (screenWithFocus is MyGuiScreenWorldSettings)
        return;
      MyGuiControlScreenSwitchPanel.SeamlesslyChangeScreen(screenWithFocus, (MyGuiScreenBase) new MyGuiScreenWorldSettings(this.m_screenEnabled[0], this.m_screenEnabled[1], this.m_screenEnabled[2]));
    }

    private void OnWorkshopButtonClick(MyGuiControlButton myGuiControlButton) => this.OpenWorkshopScreen();

    private void OpenWorkshopScreen()
    {
      MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
      if (screenWithFocus is MyGuiScreenNewWorkshopGame)
        return;
      MyGuiControlScreenSwitchPanel.SeamlesslyChangeScreen(screenWithFocus, (MyGuiScreenBase) new MyGuiScreenNewWorkshopGame(this.m_screenEnabled[0], this.m_screenEnabled[1], this.m_screenEnabled[2]));
    }

    private static void SeamlesslyChangeScreen(
      MyGuiScreenBase focusedScreen,
      MyGuiScreenBase exchangedFor)
    {
      focusedScreen.SkipTransition = true;
      focusedScreen.CloseScreen();
      exchangedFor.SkipTransition = true;
      MyScreenManager.AddScreenNow(exchangedFor);
    }

    private void SwitchToNextScreen(bool positiveDirection = true)
    {
      if (this.m_activeScreenIndex < 0 || this.m_activeScreenIndex >= this.m_screenSwithingActions.Count)
        return;
      Action nextAction = this.GetNextAction(positiveDirection, this.m_activeScreenIndex);
      if (!this.CheckWorkshopConsent(positiveDirection, nextAction))
        return;
      nextAction();
    }

    private Action GetNextAction(bool positiveDirection, int activeScreenIndex)
    {
      if (activeScreenIndex < 0 || activeScreenIndex >= this.m_screenSwithingActions.Count)
        return (Action) null;
      if (positiveDirection)
      {
        int index = activeScreenIndex;
        bool flag = false;
        do
        {
          index = (index + 1) % this.m_screenSwithingActions.Count;
          if (this.m_screenEnabled[index])
          {
            flag = true;
            break;
          }
        }
        while (index != activeScreenIndex);
        if (flag)
          return this.m_screenSwithingActions[index];
      }
      else
      {
        int index = activeScreenIndex;
        bool flag = false;
        do
        {
          index = (index + this.m_screenSwithingActions.Count - 1) % this.m_screenSwithingActions.Count;
          if (this.m_screenEnabled[index])
          {
            flag = true;
            break;
          }
        }
        while (index != activeScreenIndex);
        if (flag)
          return this.m_screenSwithingActions[index];
      }
      return (Action) null;
    }

    private bool CheckWorkshopConsent(bool positiveDirection, Action openingAction)
    {
      if (!(openingAction == new Action(this.OpenWorkshopScreen)) || MyGameService.AtLeastOneUGCServiceConsented)
        return true;
      Action nextAction1 = this.GetNextAction(positiveDirection, this.m_activeScreenIndex);
      Action nextAction2 = this.GetNextAction(positiveDirection, this.m_screenSwithingActions.IndexOf(nextAction1));
      MyModIoConsentViewModel consentViewModel = new MyModIoConsentViewModel(nextAction1, nextAction2);
      ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen((ViewModelBase) consentViewModel);
      return false;
    }

    public override MyGuiControlBase HandleInput()
    {
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT))
        this.SwitchToNextScreen(false);
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
        this.SwitchToNextScreen();
      return base.HandleInput();
    }
  }
}
