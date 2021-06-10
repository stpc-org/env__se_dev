// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.ValueGetScreenWithCaption
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
  public class ValueGetScreenWithCaption : MyGuiScreenBase
  {
    private MyGuiControlTextbox m_nameTextbox;
    private MyGuiControlButton m_confirmButton;
    private MyGuiControlButton m_cancelButton;
    private string m_title;
    private string m_caption;
    private ValueGetScreenWithCaption.ValueGetScreenAction m_acceptCallback;

    public ValueGetScreenWithCaption(
      string title,
      string caption,
      ValueGetScreenWithCaption.ValueGetScreenAction ValueAcceptedCallback)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      this.m_acceptCallback = ValueAcceptedCallback;
      this.m_title = title;
      this.m_caption = caption;
      this.m_canShareInput = false;
      this.m_isTopMostScreen = true;
      this.m_isTopScreen = true;
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (ValueGetScreenWithCaption);

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.1f)), text: this.m_title, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      this.m_nameTextbox = new MyGuiControlTextbox(new Vector2?(new Vector2(0.0f, 0.0f)), this.m_caption);
      this.m_confirmButton = new MyGuiControlButton(new Vector2?(new Vector2(0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: MyTexts.Get(MyCommonTexts.Confirm));
      this.m_cancelButton = new MyGuiControlButton(new Vector2?(new Vector2(-0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: MyTexts.Get(MyCommonTexts.Cancel));
      this.Controls.Add((MyGuiControlBase) this.m_nameTextbox);
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
      if (!this.m_acceptCallback(this.m_nameTextbox.Text))
        return;
      this.CloseScreen();
    }

    private void cancelButton_OnButtonClick(MyGuiControlButton sender) => this.CloseScreen();

    public delegate bool ValueGetScreenAction(string valueText);
  }
}
