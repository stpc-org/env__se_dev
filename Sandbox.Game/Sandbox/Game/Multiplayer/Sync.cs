// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.Sync
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Game.World;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
  public static class Sync
  {
    private static bool? m_steamOnline;
    private static float m_serverCPULoad;
    private static float m_serverCPULoadSmooth;
    private static float m_serverThreadLoad;
    private static float m_serverThreadLoadSmooth;

    public static bool MultiplayerActive => MyMultiplayer.Static != null;

    public static bool IsServer => !Sync.MultiplayerActive || MyMultiplayer.Static.IsServer;

    public static bool IsValidEventOnServer => MyMultiplayer.Static != null && MyMultiplayer.Static.IsServer && MyEventContext.Current.IsValid;

    public static bool IsDedicated => Sandbox.Engine.Platform.Game.IsDedicated;

    public static ulong ServerId => !Sync.MultiplayerActive ? Sync.MyId : MyMultiplayer.Static.ServerId;

    public static MySyncLayer Layer => MySession.Static == null ? (MySyncLayer) null : MySession.Static.SyncLayer;

    public static ulong MyId => MyGameService.UserId;

    public static string MyName => MyGameService.UserName;

    public static float ServerSimulationRatio
    {
      get
      {
        MyMultiplayerBase myMultiplayerBase = MyMultiplayer.Static;
        return myMultiplayerBase == null || myMultiplayerBase.IsServer ? MyPhysics.SimulationRatio : myMultiplayerBase.ServerSimulationRatio;
      }
      set
      {
        if (!Sync.MultiplayerActive || Sync.IsServer)
          return;
        MyMultiplayer.Static.ServerSimulationRatio = value;
      }
    }

    public static float ServerCPULoad
    {
      get
      {
        MyMultiplayerBase myMultiplayerBase = MyMultiplayer.Static;
        return myMultiplayerBase == null || myMultiplayerBase.IsServer ? MySandboxGame.Static.CPULoad : Sync.m_serverCPULoad;
      }
      set => Sync.m_serverCPULoad = value;
    }

    public static float ServerCPULoadSmooth
    {
      get
      {
        MyMultiplayerBase myMultiplayerBase = MyMultiplayer.Static;
        return myMultiplayerBase == null || myMultiplayerBase.IsServer ? MySandboxGame.Static.CPULoadSmooth : Sync.m_serverCPULoadSmooth;
      }
      set => Sync.m_serverCPULoadSmooth = MathHelper.Smooth(value, Sync.m_serverCPULoadSmooth);
    }

    public static float ServerThreadLoad
    {
      get
      {
        MyMultiplayerBase myMultiplayerBase = MyMultiplayer.Static;
        return myMultiplayerBase == null || myMultiplayerBase.IsServer ? MySandboxGame.Static.ThreadLoad : Sync.m_serverThreadLoad;
      }
      set => Sync.m_serverThreadLoad = value;
    }

    public static float ServerThreadLoadSmooth
    {
      get
      {
        MyMultiplayerBase myMultiplayerBase = MyMultiplayer.Static;
        return myMultiplayerBase == null || myMultiplayerBase.IsServer ? MySandboxGame.Static.ThreadLoadSmooth : Sync.m_serverThreadLoadSmooth;
      }
      set => Sync.m_serverThreadLoadSmooth = MathHelper.Smooth(value, Sync.m_serverThreadLoadSmooth);
    }

    public static MyClientCollection Clients => Sync.Layer != null ? Sync.Layer.Clients : (MyClientCollection) null;

    public static MyPlayerCollection Players => MySession.Static.Players;

    public static bool IsProcessingBufferedMessages => Sync.Layer.TransportLayer.IsProcessingBuffer;

    public static bool IsGameServer(this MyNetworkClient client) => client != null && (long) client.SteamUserId == (long) Sync.ServerId;

    public static void ClientConnected(ulong sender, string senderName)
    {
      if (Sync.Layer == null || Sync.Layer.Clients == null || Sync.Layer.Clients.HasClient(sender))
        return;
      Sync.Layer.Clients.AddClient(sender, senderName);
    }
  }
}
