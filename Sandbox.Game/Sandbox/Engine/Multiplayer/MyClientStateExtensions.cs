// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Multiplayer.MyClientStateExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
  public static class MyClientStateExtensions
  {
    public static MyNetworkClient GetClient(this MyClientStateBase state)
    {
      if (state == null)
        return (MyNetworkClient) null;
      MyNetworkClient client;
      Sync.Clients.TryGetClient(state.EndpointId.Id.Value, out client);
      return client;
    }

    public static MyPlayer GetPlayer(this MyClientStateBase state) => state.GetClient()?.FirstPlayer;
  }
}
