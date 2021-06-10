// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDialogRemoveTriangle
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenDialogRemoveTriangle : MyGuiScreenBase
  {
    private MyGuiControlTextbox m_textbox;
    private MyGuiControlButton m_confirmButton;
    private MyGuiControlButton m_cancelButton;

    public MyGuiScreenDialogRemoveTriangle()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDialogRemoveTriangle);

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.1f)), text: "Enter the number of a navmesh triangle to remove", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      this.m_textbox = new MyGuiControlTextbox(new Vector2?(new Vector2(0.2f, 0.0f)), type: MyGuiControlTextboxType.DigitsOnly);
      this.m_confirmButton = new MyGuiControlButton(new Vector2?(new Vector2(0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: new StringBuilder("Confirm"));
      this.m_cancelButton = new MyGuiControlButton(new Vector2?(new Vector2(-0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: new StringBuilder("Cancel"));
      this.Controls.Add((MyGuiControlBase) this.m_textbox);
      this.Controls.Add((MyGuiControlBase) this.m_confirmButton);
      this.Controls.Add((MyGuiControlBase) this.m_cancelButton);
      this.m_confirmButton.ButtonClicked += new Action<MyGuiControlButton>(this.confirmButton_OnButtonClick);
      this.m_cancelButton.ButtonClicked += new Action<MyGuiControlButton>(this.cancelButton_OnButtonClick);
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate) => base.HandleUnhandledInput(receivedFocusInThisUpdate);

    private void confirmButton_OnButtonClick(MyGuiControlButton sender)
    {
      Convert.ToInt32(this.m_textbox.Text);
      this.CloseScreen();
    }

    private void cancelButton_OnButtonClick(MyGuiControlButton sender) => this.CloseScreen();
  }
}
