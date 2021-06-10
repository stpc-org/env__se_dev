// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenServerReconnector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  internal class MyGuiScreenServerReconnector : MyGuiScreenBase
  {
    private string m_connectionString = string.Empty;
    private static int WAIT = 300;
    private static int WAIT_RETRY_DELAY = 75;
    private int m_counter = 600;
    private MyGuiScreenServerReconnector.MyReconnectionState m_state;
    private MyGuiScreenServerReconnector.MyReconnectionState m_stateLast;
    private int m_timeToReconnect;
    private int m_timeToReconnectLastFrame;
    private MyGuiControlMultilineText m_messageBoxText;
    private MyGuiControlLabel m_reconnectingCaption;

    public static MyGuiScreenServerReconnector ReconnectToLastSession()
    {
      MyObjectBuilder_LastSession lastSession = MyLocalCache.GetLastSession();
      if (lastSession == null)
        return (MyGuiScreenServerReconnector) null;
      if (!lastSession.IsOnline)
        return (MyGuiScreenServerReconnector) null;
      if (lastSession.IsLobby)
      {
        MyJoinGameHelper.JoinGame(ulong.Parse(lastSession.ServerIP));
        return (MyGuiScreenServerReconnector) null;
      }
      MyGuiScreenServerReconnector serverReconnector = new MyGuiScreenServerReconnector(lastSession.GetConnectionString());
      MyGuiSandbox.AddScreen((MyGuiScreenBase) serverReconnector);
      return serverReconnector;
    }

    public MyGuiScreenServerReconnector(string connectionString)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      this.CanHideOthers = true;
      this.m_drawEvenWithoutFocus = true;
      this.m_connectionString = connectionString;
      this.m_state = MyGuiScreenServerReconnector.MyReconnectionState.RETRY;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_size = new Vector2?(new Vector2(0.35f, 0.3f));
      Vector2 vector2_1 = new Vector2(0.1f, 0.1f);
      Vector2 vector2_2 = new Vector2(0.05f, 0.05f);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.13f)), text: MyTexts.GetString(MyCommonTexts.MultiplayerReconnector_Caption), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP));
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.07f)), text: MyTexts.GetString(MyCommonTexts.MultiplayerErrorServerHasLeft), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP));
      this.m_reconnectingCaption = new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, -0.01f)), text: string.Format(MyTexts.GetString(MyCommonTexts.MultiplayerReconnector_Reconnection), (object) 0), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
      this.Controls.Add((MyGuiControlBase) this.m_reconnectingCaption);
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(-0.15f, -0.09f), 0.3f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      this.Controls.Add((MyGuiControlBase) ((MyGuiControlButton) null = this.MakeButton(new Vector2(0.0f, 0.12f), MyCommonTexts.Cancel, new Action<MyGuiControlButton>(this.OnCancelClick), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM)));
    }

    public override bool Update(bool hasFocus)
    {
      base.Update(hasFocus);
      switch (this.m_state)
      {
        case MyGuiScreenServerReconnector.MyReconnectionState.RETRY:
          if (this.m_stateLast != this.m_state)
            this.m_reconnectingCaption.Text = string.Format(MyTexts.GetString(MyCommonTexts.MultiplayerReconnector_Reconnection), (object) this.m_timeToReconnect);
          --this.m_counter;
          this.m_timeToReconnect = this.m_counter / 60;
          if (this.m_timeToReconnectLastFrame != this.m_timeToReconnect)
            this.m_reconnectingCaption.Text = string.Format(MyTexts.GetString(MyCommonTexts.MultiplayerReconnector_Reconnection), (object) this.m_timeToReconnect);
          this.m_timeToReconnectLastFrame = this.m_timeToReconnect;
          if (this.m_counter < 0)
          {
            this.m_counter = MyGuiScreenServerReconnector.WAIT;
            try
            {
              MyGameService.OnPingServerResponded += new EventHandler<MyGameServerItem>(this.ServerResponded_Ping);
              MyGameService.OnPingServerFailedToRespond += new EventHandler(this.ServerFailedToRespond_Ping);
              MyGameService.PingServer(this.m_connectionString);
              this.m_state = MyGuiScreenServerReconnector.MyReconnectionState.WAITING;
              break;
            }
            catch (Exception ex)
            {
              MyLog.Default.WriteLine(ex);
              MyGuiSandbox.Show(MyTexts.Get(MyCommonTexts.MultiplayerJoinIPError), MyCommonTexts.MessageBoxCaptionError);
              this.OnCancelClick((MyGuiControlButton) null);
              break;
            }
          }
          else
            break;
        case MyGuiScreenServerReconnector.MyReconnectionState.WAITING:
          this.m_reconnectingCaption.Text = MyTexts.GetString(MyCommonTexts.MultiplayerReconnector_ReconnectionInProgress);
          break;
        case MyGuiScreenServerReconnector.MyReconnectionState.RETRY_DELAY:
          if (this.m_counter > 0)
          {
            --this.m_counter;
            break;
          }
          this.m_state = MyGuiScreenServerReconnector.MyReconnectionState.RETRY;
          this.m_counter = MyGuiScreenServerReconnector.WAIT;
          break;
      }
      this.m_stateLast = this.m_state;
      return true;
    }

    public void ServerResponded_Ping(object sender, MyGameServerItem serverItem)
    {
      if (this.m_state != MyGuiScreenServerReconnector.MyReconnectionState.WAITING)
        return;
      this.m_state = MyGuiScreenServerReconnector.MyReconnectionState.CONNECTING;
      MyLog.Default.WriteLineAndConsole("Server responded");
      this.CloseHandlers_Ping();
      MyJoinGameHelper.JoinGame(serverItem, false, new Action(this.ServerJoinFailed));
    }

    public void ServerJoinFailed()
    {
      this.m_state = MyGuiScreenServerReconnector.MyReconnectionState.RETRY_DELAY;
      MyLog.Default.WriteLineAndConsole("Failed to join server");
    }

    public void ServerFailedToRespond_Ping(object sender, object e)
    {
      MyLog.Default.WriteLineAndConsole("Server failed to respond");
      this.CloseHandlers_Ping();
      this.m_reconnectingCaption.Text = MyTexts.GetString(MyCommonTexts.MultiplayerReconnector_ServerNoResponse);
      this.m_state = MyGuiScreenServerReconnector.MyReconnectionState.RETRY_DELAY;
      this.m_counter = MyGuiScreenServerReconnector.WAIT_RETRY_DELAY;
    }

    private void CloseHandlers_Ping()
    {
      MyGameService.OnPingServerResponded -= new EventHandler<MyGameServerItem>(this.ServerResponded_Ping);
      MyGameService.OnPingServerFailedToRespond -= new EventHandler(this.ServerFailedToRespond_Ping);
    }

    private MyGuiControlButton MakeButton(
      Vector2 position,
      MyStringId text,
      Action<MyGuiControlButton> onClick,
      MyGuiDrawAlignEnum align)
    {
      Vector2? position1 = new Vector2?(position);
      Vector2? size = new Vector2?();
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(text);
      Action<MyGuiControlButton> action = onClick;
      int num = (int) align;
      StringBuilder text1 = stringBuilder;
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      return new MyGuiControlButton(position1, size: size, colorMask: colorMask, originAlign: ((MyGuiDrawAlignEnum) num), text: text1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
    }

    public void OnCancelClick(MyGuiControlButton sender)
    {
      this.m_state = MyGuiScreenServerReconnector.MyReconnectionState.IDLE;
      this.CloseScreen();
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenServerReconnector);

    private enum MyReconnectionState
    {
      IDLE,
      RETRY,
      WAITING,
      CONNECTING,
      RETRY_DELAY,
    }
  }
}
