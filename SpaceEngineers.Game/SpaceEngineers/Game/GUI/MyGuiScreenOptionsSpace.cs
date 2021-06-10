// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenOptionsSpace
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.GUI
{
  internal class MyGuiScreenOptionsSpace : MyGuiScreenBase
  {
    private MyGuiControlElementGroup m_elementGroup;
    private bool m_isLimitedMenu;

    public MyGuiScreenOptionsSpace(bool isLimitedMenu = false)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(isLimitedMenu ? new Vector2(0.3214286f, 0.4055344f) : new Vector2(0.3214286f, 0.5200382f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_isLimitedMenu = isLimitedMenu;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_elementGroup = new MyGuiControlElementGroup();
      this.AddCaption(MyCommonTexts.ScreenCaptionOptions, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      this.m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
      this.m_guiTransition = MySandboxGame.Config.UIOpacity;
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(-new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.83f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      Vector2 start = -new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0500000007450581));
      controlSeparatorList2.AddHorizontal(start, this.m_size.Value.X * 0.83f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      MyStringId optionsScreenHelpMenu = MySpaceTexts.OptionsScreen_Help_Menu;
      Vector2 vector2_1 = new Vector2(1f / 1000f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.126000002026558));
      int num1 = 0;
      Vector2 vector2_2 = vector2_1;
      int num2 = num1;
      int num3 = num2 + 1;
      Vector2 vector2_3 = (float) num2 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton(new Vector2?(vector2_2 + vector2_3), text: MyTexts.Get(MyCommonTexts.ScreenOptionsButtonGame), onButtonClick: ((Action<MyGuiControlButton>) (sender => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenOptionsGame(this.m_isLimitedMenu)))));
      guiControlButton1.GamepadHelpTextId = optionsScreenHelpMenu;
      this.Controls.Add((MyGuiControlBase) guiControlButton1);
      this.m_elementGroup.Add((MyGuiControlBase) guiControlButton1);
      if (!this.m_isLimitedMenu)
      {
        Vector2 vector2_4 = vector2_1;
        int num4 = num3;
        int num5 = num4 + 1;
        Vector2 vector2_5 = (float) num4 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
        MyGuiControlButton guiControlButton2 = new MyGuiControlButton(new Vector2?(vector2_4 + vector2_5), text: MyTexts.Get(MyCommonTexts.ScreenOptionsButtonDisplay), onButtonClick: ((Action<MyGuiControlButton>) (sender => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenOptionsDisplay()))));
        guiControlButton2.GamepadHelpTextId = optionsScreenHelpMenu;
        this.Controls.Add((MyGuiControlBase) guiControlButton2);
        this.m_elementGroup.Add((MyGuiControlBase) guiControlButton2);
        Vector2 vector2_6 = vector2_1;
        int num6 = num5;
        num3 = num6 + 1;
        Vector2 vector2_7 = (float) num6 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
        MyGuiControlButton guiControlButton3 = new MyGuiControlButton(new Vector2?(vector2_6 + vector2_7), text: MyTexts.Get(MyCommonTexts.ScreenOptionsButtonGraphics), onButtonClick: ((Action<MyGuiControlButton>) (sender => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenOptionsGraphics()))));
        guiControlButton3.GamepadHelpTextId = optionsScreenHelpMenu;
        this.Controls.Add((MyGuiControlBase) guiControlButton3);
        this.m_elementGroup.Add((MyGuiControlBase) guiControlButton3);
      }
      Vector2 vector2_8 = vector2_1;
      int num7 = num3;
      int num8 = num7 + 1;
      Vector2 vector2_9 = (float) num7 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
      MyGuiControlButton guiControlButton4 = new MyGuiControlButton(new Vector2?(vector2_8 + vector2_9), text: MyTexts.Get(MyCommonTexts.ScreenOptionsButtonAudio), onButtonClick: ((Action<MyGuiControlButton>) (sender => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenOptionsAudio(this.m_isLimitedMenu)))));
      guiControlButton4.GamepadHelpTextId = optionsScreenHelpMenu;
      this.Controls.Add((MyGuiControlBase) guiControlButton4);
      this.m_elementGroup.Add((MyGuiControlBase) guiControlButton4);
      Vector2 vector2_10 = vector2_1;
      int num9 = num8;
      int num10 = num9 + 1;
      Vector2 vector2_11 = (float) num9 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
      MyGuiControlButton guiControlButton5 = new MyGuiControlButton(new Vector2?(vector2_10 + vector2_11), text: MyTexts.Get(MyCommonTexts.ScreenOptionsButtonControls), onButtonClick: ((Action<MyGuiControlButton>) (sender => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenOptionsControls()))));
      guiControlButton5.GamepadHelpTextId = optionsScreenHelpMenu;
      this.Controls.Add((MyGuiControlBase) guiControlButton5);
      this.m_elementGroup.Add((MyGuiControlBase) guiControlButton5);
      Vector2 vector2_12 = vector2_1;
      int num11 = num10;
      int num12 = num11 + 1;
      Vector2 vector2_13 = (float) num11 * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
      MyGuiControlButton guiControlButton6 = new MyGuiControlButton(new Vector2?(vector2_12 + vector2_13), text: MyTexts.Get(MyCommonTexts.ScreenMenuButtonCredits), onButtonClick: ((Action<MyGuiControlButton>) (sender => this.OnClickCredits(sender))));
      guiControlButton6.GamepadHelpTextId = optionsScreenHelpMenu;
      this.Controls.Add((MyGuiControlBase) guiControlButton6);
      this.m_elementGroup.Add((MyGuiControlBase) guiControlButton6);
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, start.Y + minSizeGui.Y / 2f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.Gamepad_Help_Back);
      this.CloseButtonEnabled = true;
    }

    private void OnClickCredits(MyGuiControlButton sender) => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenGameCredits());

    protected override void OnShow()
    {
      base.OnShow();
      this.m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
      this.m_guiTransition = MySandboxGame.Config.UIOpacity;
    }

    public override string GetFriendlyName() => "MyGuiScreenOptions";

    public void OnBackClick(MyGuiControlButton sender) => this.CloseScreen();
  }
}
