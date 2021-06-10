// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenServerConnect
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenServerConnect : MyGuiScreenBase
  {
    private readonly float _padding = 0.02f;
    private MyGuiControlTextbox m_addrTextbox;
    private MyGuiControlCheckbox m_favoriteCheckbox;
    private MyGuiScreenProgress m_progressScreen;

    public MyGuiScreenServerConnect()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.4971429f, 0.269084f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
      => this.CreateScreen();

    private void CreateScreen()
    {
      this.CanHideOthers = true;
      this.CanBeHidden = true;
      this.EnabledBackgroundFade = true;
      this.CloseButtonEnabled = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      this.AddCaption(MyCommonTexts.MultiplayerJoinDirectConnect, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(-new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.78f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(-new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0500000007450581)), this.m_size.Value.X * 0.78f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(-new Vector2(this.m_size.Value.X * 0.385f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.11599999666214))), text: MyTexts.GetString(MyCommonTexts.JoinGame_Favorites_Add));
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.m_favoriteCheckbox = new MyGuiControlCheckbox(new Vector2?(myGuiControlLabel.Position), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_favoriteCheckbox.PositionX += myGuiControlLabel.Size.X + 0.01f;
      this.m_favoriteCheckbox.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameDirectConnect_Favorite));
      this.Controls.Add((MyGuiControlBase) this.m_favoriteCheckbox);
      this.m_addrTextbox = new MyGuiControlTextbox(new Vector2?(-new Vector2((float) ((double) this.m_size.Value.X * 0.779999971389771 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.170000001788139))), "0.0.0.0:27016");
      this.m_addrTextbox.Size = new Vector2(this.m_addrTextbox.Size.X / 1.33f, this.m_addrTextbox.Size.Y);
      this.m_addrTextbox.PositionX += this.m_addrTextbox.Size.X / 2f;
      this.m_addrTextbox.EnterPressed += new Action<MyGuiControlTextbox>(this.AddressEnterPressed);
      this.m_addrTextbox.FocusChanged += new Action<MyGuiControlBase, bool>(this.AddressFocusChanged);
      this.m_addrTextbox.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameDirectConnect_IP));
      this.m_addrTextbox.MoveCarriageToEnd();
      this.Controls.Add((MyGuiControlBase) this.m_addrTextbox);
      MyGuiControlButton guiControlButton = new MyGuiControlButton(new Vector2?(new Vector2(this.m_addrTextbox.PositionX + this.m_addrTextbox.Size.X / 2f, this.m_addrTextbox.PositionY + 0.007f)), MyGuiControlButtonStyleEnum.ComboBoxButton, text: MyTexts.Get(MyCommonTexts.MultiplayerJoinConnect));
      guiControlButton.PositionX += (float) ((double) guiControlButton.Size.X / 2.0 + (double) this._padding * 0.660000026226044);
      guiControlButton.ButtonClicked += new Action<MyGuiControlButton>(this.ConnectButtonClick);
      guiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGame_JoinWorld));
      this.Controls.Add((MyGuiControlBase) guiControlButton);
    }

    private void AddressEnterPressed(MyGuiControlTextbox obj) => this.ParseIPAndConnect();

    private void AddressFocusChanged(MyGuiControlBase obj, bool focused)
    {
      if (!focused)
        return;
      this.m_addrTextbox.SelectAll();
      this.m_addrTextbox.MoveCarriageToEnd();
    }

    private void ConnectButtonClick(MyGuiControlButton obj) => this.ParseIPAndConnect();

    private void ParseIPAndConnect()
    {
      try
      {
        string connectionString = this.m_addrTextbox.Text.Trim();
        if (this.m_favoriteCheckbox.IsChecked)
          MyGameService.AddFavoriteGame(connectionString);
        this.m_progressScreen = new MyGuiScreenProgress(MyTexts.Get(MyCommonTexts.DialogTextJoiningWorld), new MyStringId?(MyCommonTexts.Cancel), false);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) this.m_progressScreen);
        this.m_progressScreen.ProgressCancelled += (Action) (() =>
        {
          this.CloseHandlers();
          MySessionLoader.UnloadAndExitToMenu();
        });
        MyGameService.OnPingServerResponded += new EventHandler<MyGameServerItem>(this.ServerResponded);
        MyGameService.OnPingServerFailedToRespond += new EventHandler(this.ServerFailedToRespond);
        MyGameService.PingServer(connectionString);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(ex);
        MyGuiSandbox.Show(MyTexts.Get(MyCommonTexts.MultiplayerJoinIPError), MyCommonTexts.MessageBoxCaptionError);
      }
    }

    private void ServerResponded(object sender, MyGameServerItem serverItem)
    {
      this.CloseHandlers();
      this.m_progressScreen.CloseScreen();
      MyLocalCache.SaveLastSessionInfo((string) null, true, false, serverItem.Name, serverItem.ConnectionString);
      MyJoinGameHelper.JoinGame(serverItem);
    }

    private void ServerFailedToRespond(object sender, object e)
    {
      this.CloseHandlers();
      this.m_progressScreen.CloseScreen();
      MyGuiSandbox.Show(MyCommonTexts.MultiplaterJoin_ServerIsNotResponding);
    }

    private void CloseHandlers()
    {
      MyGameService.OnPingServerResponded -= new EventHandler<MyGameServerItem>(this.ServerResponded);
      MyGameService.OnPingServerFailedToRespond -= new EventHandler(this.ServerFailedToRespond);
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      this.CloseHandlers();
      if (this.m_progressScreen != null)
        this.m_progressScreen.CloseScreen(isUnloading);
      return base.CloseScreen(isUnloading);
    }

    public override string GetFriendlyName() => "ServerConnect";
  }
}
