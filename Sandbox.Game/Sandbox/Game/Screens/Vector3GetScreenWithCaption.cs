// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Vector3GetScreenWithCaption
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class Vector3GetScreenWithCaption : MyGuiScreenBase
  {
    private MyGuiControlTextbox m_nameTextbox1;
    private MyGuiControlTextbox m_nameTextbox2;
    private MyGuiControlTextbox m_nameTextbox3;
    private MyGuiControlButton m_confirmButton;
    private MyGuiControlButton m_cancelButton;
    private string m_title;
    private string m_caption1;
    private string m_caption2;
    private string m_caption3;
    private Vector3GetScreenWithCaption.Vector3GetScreenAction m_acceptCallback;

    public Vector3GetScreenWithCaption(
      string title,
      string caption1,
      string caption2,
      string caption3,
      Vector3GetScreenWithCaption.Vector3GetScreenAction ValueAcceptedCallback)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      this.m_acceptCallback = ValueAcceptedCallback;
      this.m_title = title;
      this.m_caption1 = caption1;
      this.m_caption2 = caption2;
      this.m_caption3 = caption3;
      this.m_canShareInput = false;
      this.m_isTopMostScreen = true;
      this.m_isTopScreen = true;
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (Vector3GetScreenWithCaption);

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.1f)), text: this.m_title, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      float num1 = 0.0f;
      float num2 = 0.04f;
      this.m_nameTextbox1 = new MyGuiControlTextbox(new Vector2?(new Vector2(0.0f - num1, 0.0f - num2)), this.m_caption1);
      this.m_nameTextbox2 = new MyGuiControlTextbox(new Vector2?(new Vector2(0.0f, 0.0f)), this.m_caption2);
      this.m_nameTextbox3 = new MyGuiControlTextbox(new Vector2?(new Vector2(0.0f + num1, 0.0f + num2)), this.m_caption3);
      this.m_confirmButton = new MyGuiControlButton(new Vector2?(new Vector2(0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: MyTexts.Get(MyCommonTexts.Confirm));
      this.m_cancelButton = new MyGuiControlButton(new Vector2?(new Vector2(-0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: MyTexts.Get(MyCommonTexts.Cancel));
      this.Controls.Add((MyGuiControlBase) this.m_nameTextbox1);
      this.Controls.Add((MyGuiControlBase) this.m_nameTextbox2);
      this.Controls.Add((MyGuiControlBase) this.m_nameTextbox3);
      this.Controls.Add((MyGuiControlBase) this.m_confirmButton);
      this.Controls.Add((MyGuiControlBase) this.m_cancelButton);
      this.m_confirmButton.ButtonClicked += new Action<MyGuiControlButton>(this.confirmButton_OnButtonClick);
      this.m_cancelButton.ButtonClicked += new Action<MyGuiControlButton>(this.cancelButton_OnButtonClick);
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
    {
      base.HandleUnhandledInput(receivedFocusInThisUpdate);
      if (MyInput.Static.IsKeyPress(MyKeys.Enter))
        this.confirmButton_OnButtonClick(this.m_confirmButton);
      if (!MyInput.Static.IsKeyPress(MyKeys.Escape))
        return;
      this.cancelButton_OnButtonClick(this.m_cancelButton);
    }

    private void confirmButton_OnButtonClick(MyGuiControlButton sender)
    {
      if (!this.m_acceptCallback(this.m_nameTextbox1.Text, this.m_nameTextbox2.Text, this.m_nameTextbox3.Text))
        return;
      this.CloseScreen();
    }

    private void cancelButton_OnButtonClick(MyGuiControlButton sender) => this.CloseScreen();

    public delegate bool Vector3GetScreenAction(string value1, string value2, string value3);
  }
}
