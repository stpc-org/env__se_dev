// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenDialogText
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.IO;
using VRage.FileSystem;
using VRage.Game;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenDialogText : MyGuiScreenBase
  {
    private MyGuiControlLabel m_captionLabel;
    private MyGuiControlTextbox m_valueTextbox;
    private MyGuiControlButton m_confirmButton;
    private MyGuiControlButton m_cancelButton;
    private MyStringId m_caption;
    private readonly string m_value;

    public event Action<string> OnConfirmed;

    public event Action<string> OnCancelled;

    public MyGuiScreenDialogText(string initialValue = null, MyStringId? caption = null, bool isTopMostScreen = false)
      : base(isTopMostScreen: isTopMostScreen)
    {
      this.m_backgroundTransition = MySandboxGame.Config.UIBkOpacity;
      this.m_guiTransition = MySandboxGame.Config.UIOpacity;
      this.m_value = initialValue ?? string.Empty;
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = true;
      this.m_caption = caption ?? MyCommonTexts.DialogAmount_SetValueCaption;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDialogText);

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      MyObjectBuilder_GuiScreen objectBuilder;
      MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_GuiScreen>(Path.Combine(MyFileSystem.ContentPath, MyGuiScreenBase.MakeScreenFilepath("DialogText")), out objectBuilder);
      this.Init(objectBuilder);
      this.m_valueTextbox = (MyGuiControlTextbox) this.Controls.GetControlByName("ValueTextbox");
      this.m_confirmButton = (MyGuiControlButton) this.Controls.GetControlByName("ConfirmButton");
      this.m_cancelButton = (MyGuiControlButton) this.Controls.GetControlByName("CancelButton");
      this.m_captionLabel = (MyGuiControlLabel) this.Controls.GetControlByName("CaptionLabel");
      this.m_captionLabel.Text = (string) null;
      this.m_captionLabel.TextEnum = this.m_caption;
      this.m_confirmButton.ButtonClicked += new Action<MyGuiControlButton>(this.confirmButton_OnButtonClick);
      this.m_cancelButton.ButtonClicked += new Action<MyGuiControlButton>(this.cancelButton_OnButtonClick);
      this.m_valueTextbox.Text = this.m_value;
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
    {
      base.HandleUnhandledInput(receivedFocusInThisUpdate);
      if (!MyInput.Static.IsNewKeyPressed(MyKeys.Enter) && !MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01))
        return;
      this.confirmButton_OnButtonClick(this.m_confirmButton);
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      if (this.Cancelled)
        this.OnCancelled.InvokeIfNotNull<string>(this.m_valueTextbox.Text);
      else
        this.OnConfirmed.InvokeIfNotNull<string>(this.m_valueTextbox.Text);
      return base.CloseScreen(isUnloading);
    }

    private void confirmButton_OnButtonClick(MyGuiControlButton sender) => this.CloseScreen(false);

    private void cancelButton_OnButtonClick(MyGuiControlButton sender) => this.CloseScreen(false);
  }
}
