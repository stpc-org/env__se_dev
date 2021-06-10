// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyMultiplayerHostResult
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using System;
using System.Threading;
using VRage.GameServices;

namespace Sandbox.Engine.Multiplayer
{
  public class MyMultiplayerHostResult
  {
    private bool m_done;

    public event Action<bool, MyLobbyStatusCode, MyMultiplayerBase> Done;

    public bool Cancelled { get; private set; }

    public bool Success { get; private set; }

    public MyLobbyStatusCode StatusCode { get; private set; }

    public void Cancel() => this.Cancelled = true;

    public void RaiseDone(bool success, MyLobbyStatusCode reason, MyMultiplayerBase multiplayer)
    {
      this.Success = success;
      this.StatusCode = reason;
      Action<bool, MyLobbyStatusCode, MyMultiplayerBase> done = this.Done;
      if (done != null)
        done(success, reason, multiplayer);
      this.m_done = true;
    }

    public void Wait(bool runCallbacks = true)
    {
      while (!this.Cancelled && !this.m_done)
      {
        if (runCallbacks)
          MyGameService.Update();
        Thread.Sleep(10);
      }
    }
  }
}
