// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiBlueprintTextDialog
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiBlueprintTextDialog : MyGuiBlueprintScreenBase
  {
    private MyGuiControlTextbox m_nameBox;
    private MyGuiControlButton m_okButton;
    private MyGuiControlButton m_cancelButton;
    private string m_defaultName;
    private string m_caption;
    private int m_maxTextLength;
    private float m_textBoxWidth;
    private Action<string> callBack;
    private Vector2 WINDOW_SIZE = new Vector2(0.3f, 0.5f);

    public MyGuiBlueprintTextDialog(
      Vector2 position,
      Action<string> callBack,
      string defaultName,
      string caption = "",
      int maxLenght = 20,
      float textBoxWidth = 0.2f)
      : base(position, new Vector2(0.4971429f, 0.2805344f), MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, true)
    {
      this.m_maxTextLength = maxLenght;
      this.m_caption = caption;
      this.m_textBoxWidth = textBoxWidth;
      this.callBack = callBack;
      this.m_defaultName = defaultName;
      this.RecreateControls(true);
      this.OnEnterCallback = new Action(this.ReturnOk);
      this.CanBeHidden = true;
      this.CanHideOthers = true;
      this.CloseButtonEnabled = true;
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(this.m_caption, new Vector4?(Color.White.ToVector4()), new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.78f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.78f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      this.m_nameBox = new MyGuiControlTextbox(new Vector2?(new Vector2(0.0f, -0.027f)), maxLength: this.m_maxTextLength);
      this.m_nameBox.Text = this.m_defaultName;
      this.m_nameBox.Size = new Vector2(0.385f, 1f);
      this.Controls.Add((MyGuiControlBase) this.m_nameBox);
      this.m_okButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnOk));
      this.m_cancelButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Cancel), onButtonClick: new Action<MyGuiControlButton>(this.OnCancel));
      Vector2 vector2_1 = new Vector2(1f / 500f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.0710000023245811));
      Vector2 vector2_2 = new Vector2(0.018f, 0.0f);
      this.m_okButton.Position = vector2_1 - vector2_2;
      this.m_cancelButton.Position = vector2_1 + vector2_2;
      this.m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
      this.m_cancelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      this.Controls.Add((MyGuiControlBase) this.m_cancelButton);
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
      myGuiControlLabel.Position = this.m_okButton.Position;
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.DialogBlueprintRename_GamepadHelp);
    }

    public override bool Update(bool hasFocus)
    {
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      return base.Update(hasFocus);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (receivedFocusInThisUpdate)
        return;
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OnOk((MyGuiControlButton) null);
      if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_B))
        return;
      this.OnCancel((MyGuiControlButton) null);
    }

    private void CallResultCallback(string val)
    {
      if (val == null)
        return;
      this.callBack(val);
    }

    private void ReturnOk()
    {
      if (this.m_nameBox.Text.Length <= 0)
        return;
      this.CallResultCallback(this.m_nameBox.Text);
      this.CloseScreen();
    }

    private void OnOk(MyGuiControlButton button) => this.ReturnOk();

    private void OnCancel(MyGuiControlButton button) => this.CloseScreen();

    public override string GetFriendlyName() => "MyGuiRenameDialog";
  }
}
