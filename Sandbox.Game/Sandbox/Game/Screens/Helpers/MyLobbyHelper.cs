// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyLobbyHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Timers;
using VRage.GameServices;

namespace Sandbox.Game.Screens.Helpers
{
  public class MyLobbyHelper
  {
    private IMyLobby m_lobby;
    private MyLobbyDataUpdated m_dataUpdateHandler;

    public event Action<IMyLobby, bool> OnSuccess;

    public MyLobbyHelper(IMyLobby lobby)
    {
      this.m_lobby = lobby;
      this.m_dataUpdateHandler = new MyLobbyDataUpdated(this.JoinGame_LobbyUpdate);
    }

    private void t_Elapsed(object sender, ElapsedEventArgs e) => this.m_lobby.OnDataReceived -= this.m_dataUpdateHandler;

    public bool RequestData()
    {
      this.m_lobby.OnDataReceived += this.m_dataUpdateHandler;
      if (this.m_lobby.RequestData())
        return true;
      this.m_lobby.OnDataReceived -= this.m_dataUpdateHandler;
      return false;
    }

    public void Cancel() => this.m_lobby.OnDataReceived -= this.m_dataUpdateHandler;

    private void JoinGame_LobbyUpdate(bool success, IMyLobby lobby, ulong memberOrLobby)
    {
      if ((long) lobby.LobbyId != (long) this.m_lobby.LobbyId)
        return;
      this.m_lobby.OnDataReceived -= this.m_dataUpdateHandler;
      Action<IMyLobby, bool> onSuccess = this.OnSuccess;
      if (onSuccess == null)
        return;
      onSuccess(lobby, success);
    }
  }
}
