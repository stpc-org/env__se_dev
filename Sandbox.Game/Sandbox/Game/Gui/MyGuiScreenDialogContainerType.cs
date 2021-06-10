// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDialogContainerType
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenDialogContainerType : MyGuiScreenBase
  {
    private MyGuiControlTextbox m_containerTypeTextbox;
    private MyGuiControlButton m_confirmButton;
    private MyGuiControlButton m_cancelButton;
    private MyCargoContainer m_container;

    public MyGuiScreenDialogContainerType(MyCargoContainer container)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = true;
      this.m_container = container;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDialogContainerType);

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.1f)), text: "Type the container type name for this cargo container:", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      this.m_containerTypeTextbox = new MyGuiControlTextbox(new Vector2?(new Vector2(0.0f, 0.0f)), this.m_container.ContainerType, 100);
      this.m_confirmButton = new MyGuiControlButton(new Vector2?(new Vector2(0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: new StringBuilder("Confirm"));
      this.m_cancelButton = new MyGuiControlButton(new Vector2?(new Vector2(-0.21f, 0.1f)), size: new Vector2?(new Vector2(0.2f, 0.05f)), text: new StringBuilder("Cancel"));
      this.Controls.Add((MyGuiControlBase) this.m_containerTypeTextbox);
      this.Controls.Add((MyGuiControlBase) this.m_confirmButton);
      this.Controls.Add((MyGuiControlBase) this.m_cancelButton);
      this.m_confirmButton.ButtonClicked += new Action<MyGuiControlButton>(this.confirmButton_OnButtonClick);
      this.m_cancelButton.ButtonClicked += new Action<MyGuiControlButton>(this.cancelButton_OnButtonClick);
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate) => base.HandleUnhandledInput(receivedFocusInThisUpdate);

    private void confirmButton_OnButtonClick(MyGuiControlButton sender)
    {
      this.m_container.ContainerType = this.m_containerTypeTextbox.Text == null || !(this.m_containerTypeTextbox.Text != "") ? (string) null : this.m_containerTypeTextbox.Text;
      this.CloseScreen();
    }

    private void cancelButton_OnButtonClick(MyGuiControlButton sender) => this.CloseScreen();
  }
}
