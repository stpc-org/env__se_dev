// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenBotSettings
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.AI;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenBotSettings : MyGuiScreenBase
  {
    public override string GetFriendlyName() => nameof (MyGuiScreenBotSettings);

    public MyGuiScreenBotSettings()
      : base()
    {
      this.Size = new Vector2?(new Vector2(650f, 350f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
      this.BackgroundColor = new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR);
      this.RecreateControls(true);
      this.CanHideOthers = false;
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_position = new Vector2(MyGuiManager.GetMaxMouseCoord().X - 0.25f, 0.5f);
      MyLayoutVertical myLayoutVertical = new MyLayoutVertical((IMyGuiControlsParent) this, 35f);
      myLayoutVertical.Advance(20f);
      myLayoutVertical.Add((MyGuiControlBase) new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.BotSettingsScreen_Title)), MyAlignH.Center);
      myLayoutVertical.Advance(30f);
      MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(isChecked: MyDebugDrawSettings.DEBUG_DRAW_BOTS);
      guiControlCheckbox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.enableDebuggingCheckBox_IsCheckedChanged);
      myLayoutVertical.Add((MyGuiControlBase) new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.BotSettingsScreen_EnableBotsDebugging)), MyAlignH.Left, false);
      myLayoutVertical.Add((MyGuiControlBase) guiControlCheckbox, MyAlignH.Right);
      myLayoutVertical.Advance(15f);
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.BotSettingsScreen_NextBot), onButtonClick: new Action<MyGuiControlButton>(this.nextButton_OnButtonClick));
      MyGuiControlButton guiControlButton2 = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.BotSettingsScreen_PreviousBot), onButtonClick: new Action<MyGuiControlButton>(this.previousButton_OnButtonClick));
      myLayoutVertical.Add((MyGuiControlBase) guiControlButton1, (MyGuiControlBase) guiControlButton2);
      myLayoutVertical.Advance(30f);
      myLayoutVertical.Add((MyGuiControlBase) new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Close), onButtonClick: new Action<MyGuiControlButton>(this.OnCloseClicked)), MyAlignH.Center);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate) => base.HandleInput(receivedFocusInThisUpdate);

    private void enableDebuggingCheckBox_IsCheckedChanged(MyGuiControlCheckbox checkBox) => MyDebugDrawSettings.DEBUG_DRAW_BOTS = checkBox.IsChecked;

    private void nextButton_OnButtonClick(MyGuiControlButton button) => MyAIComponent.Static.DebugSelectNextBot();

    private void previousButton_OnButtonClick(MyGuiControlButton button) => MyAIComponent.Static.DebugSelectPreviousBot();

    private void OnCloseClicked(MyGuiControlButton button) => this.CloseScreen();
  }
}
