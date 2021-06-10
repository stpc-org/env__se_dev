// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyMultiplayerJoinResult
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.GameServices;

namespace Sandbox.Engine.Multiplayer
{
  public class MyMultiplayerJoinResult
  {
    public event Action<bool, IMyLobby, MyLobbyStatusCode, MyMultiplayerBase> JoinDone;

    public bool Cancelled { get; private set; }

    public void Cancel() => this.Cancelled = true;

    public void RaiseJoined(
      bool success,
      IMyLobby lobby,
      MyLobbyStatusCode response,
      MyMultiplayerBase multiplayer)
    {
      Action<bool, IMyLobby, MyLobbyStatusCode, MyMultiplayerBase> joinDone = this.JoinDone;
      if (joinDone == null)
        return;
      joinDone(success, lobby, response, multiplayer);
    }
  }
}
