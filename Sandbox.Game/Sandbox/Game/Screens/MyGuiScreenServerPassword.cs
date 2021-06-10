// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenServerPassword
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenServerPassword : MyGuiScreenBase
  {
    private readonly float _padding = 0.02f;
    private MyGuiControlTextbox m_passwordTextbox;
    private Action<string> m_connectAction;

    public MyGuiScreenServerPassword(Action<string> connectAction)
      : base(new Vector2?(new Vector2(0.5f, 0.75f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.4971429f, 0.1908397f)), true)
    {
      this.m_connectAction = connectAction;
      this.CreateScreen();
    }

    private void CreateScreen()
    {
      this.CanHideOthers = false;
      this.CanBeHidden = false;
      this.EnabledBackgroundFade = false;
      this.CloseButtonEnabled = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      this.AddCaption(MyCommonTexts.MultiplayerEnterPassword, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(-new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.78f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(-new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0500000007450581)), this.m_size.Value.X * 0.78f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      this.m_passwordTextbox = new MyGuiControlTextbox(new Vector2?(-new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.104999996721745))), string.Empty);
      this.m_passwordTextbox.Size = new Vector2(this.m_passwordTextbox.Size.X / 1.33f, this.m_passwordTextbox.Size.Y);
      this.m_passwordTextbox.PositionX += this.m_passwordTextbox.Size.X / 2f;
      this.m_passwordTextbox.EnterPressed += new Action<MyGuiControlTextbox>(this.AddressEnterPressed);
      this.m_passwordTextbox.FocusChanged += new Action<MyGuiControlBase, bool>(this.AddressFocusChanged);
      this.m_passwordTextbox.SetToolTip(MyTexts.GetString(MyCommonTexts.MultiplayerEnterPassword));
      this.m_passwordTextbox.Type = MyGuiControlTextboxType.Password;
      this.m_passwordTextbox.MoveCarriageToEnd();
      this.Controls.Add((MyGuiControlBase) this.m_passwordTextbox);
      MyGuiControlButton guiControlButton = new MyGuiControlButton(new Vector2?(new Vector2(this.m_passwordTextbox.PositionX + this.m_passwordTextbox.Size.X / 2f, this.m_passwordTextbox.PositionY + 0.007f)), MyGuiControlButtonStyleEnum.ComboBoxButton, text: MyTexts.Get(MyCommonTexts.MultiplayerJoinConnect));
      guiControlButton.PositionX += (float) ((double) guiControlButton.Size.X / 2.0 + (double) this._padding * 0.660000026226044);
      guiControlButton.ButtonClicked += new Action<MyGuiControlButton>(this.ConnectButtonClick);
      guiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGame_JoinWorld));
      this.Controls.Add((MyGuiControlBase) guiControlButton);
    }

    private void AddressEnterPressed(MyGuiControlTextbox obj) => this.ConnectButtonClick((MyGuiControlButton) null);

    private void AddressFocusChanged(MyGuiControlBase obj, bool focused)
    {
      if (!focused)
        return;
      this.m_passwordTextbox.SelectAll();
      this.m_passwordTextbox.MoveCarriageToEnd();
    }

    private void ConnectButtonClick(MyGuiControlButton obj)
    {
      this.CloseScreen();
      if (this.m_connectAction == null)
        return;
      this.m_connectAction(this.m_passwordTextbox.Text);
    }

    public override string GetFriendlyName() => "ServerPassword";
  }
}
