// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyNetworkClient
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using System;
using VRage.Game.ModAPI;

namespace Sandbox.Game.World
{
  public class MyNetworkClient : IMyNetworkClient
  {
    private readonly ulong m_steamUserId;
    public ushort ClientFrameId;
    private int m_controlledPlayerSerialId;

    public ulong SteamUserId => this.m_steamUserId;

    public bool IsLocal { get; private set; }

    public string DisplayName { get; private set; }

    public int ControlledPlayerSerialId
    {
      private get => this.m_controlledPlayerSerialId;
      set
      {
        if (this.ControlledPlayerSerialId == value)
          return;
        this.FirstPlayer.ReleaseControls();
        this.m_controlledPlayerSerialId = value;
        this.FirstPlayer.AcquireControls();
      }
    }

    public MyPlayer FirstPlayer => this.GetPlayer(this.ControlledPlayerSerialId);

    public event Action ClientLeft;

    public MyNetworkClient(ulong steamId, string senderName)
    {
      this.m_steamUserId = steamId;
      this.IsLocal = (long) Sync.MyId == (long) steamId;
      this.DisplayName = senderName;
    }

    public MyPlayer GetPlayer(int serialId) => Sync.Players.GetPlayerById(new MyPlayer.PlayerId()
    {
      SteamId = this.m_steamUserId,
      SerialId = serialId
    });

    public MyPlayer GetPlayer(ulong steamId, int serialId) => Sync.Players.GetPlayerById(new MyPlayer.PlayerId()
    {
      SteamId = steamId,
      SerialId = serialId
    });
  }
}
