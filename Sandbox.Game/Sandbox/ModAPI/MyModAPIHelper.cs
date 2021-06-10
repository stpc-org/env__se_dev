// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.MyModAPIHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI.Physics;
using System;
using System.Collections.Generic;
using System.Threading;
using VRage;
using VRage.Game.ModAPI;
using VRage.GameServices;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.ModAPI
{
  public static class MyModAPIHelper
  {
    public static void Initialize()
    {
      Sandbox.Engine.Platform.Game.EnableSimSpeedLocking = true;
      MyAPIGateway.Session = (IMySession) MySession.Static;
      MyAPIGateway.Entities = (IMyEntities) new MyEntitiesHelper_ModAPI();
      MyAPIGateway.Players = (IMyPlayerCollection) Sync.Players;
      MyAPIGateway.CubeBuilder = (IMyCubeBuilder) MyCubeBuilder.Static;
      MyAPIGateway.IngameScripting = (IMyIngameScripting) new MyVRageIngameScriptingAdapter(MyVRage.Platform.Scripting.GetModAPIScriptingHandle());
      MyAPIGateway.TerminalActionsHelper = (IMyTerminalActionsHelper) MyTerminalControlFactoryHelper.Static;
      MyAPIGateway.Utilities = (IMyUtilities) MyAPIUtilities.Static;
      MyAPIGateway.Parallel = (IMyParallelTask) MyParallelTask.Static;
      MyAPIGateway.Physics = (IMyPhysics) MyPhysics.Static;
      MyAPIGateway.Multiplayer = (IMyMultiplayer) MyModAPIHelper.MyMultiplayer.Static;
      MyAPIGateway.PrefabManager = (IMyPrefabManager) MyPrefabManager.Static;
      MyAPIGateway.Input = (VRage.ModAPI.IMyInput) MyInput.Static;
      MyAPIGateway.TerminalControls = (IMyTerminalControls) MyTerminalControls.Static;
      MyAPIGateway.Gui = (IMyGui) new MyGuiModHelpers();
      MyAPIGateway.GridGroups = (IMyGridGroups) new MyGridGroupsHelper();
      MyAPIGateway.Reflection = (IMyReflection) new ScriptingReflection();
      MyAPIGateway.ContractSystem = MySession.Static == null ? (IMyContractSystem) null : (IMyContractSystem) MySession.Static.GetComponent<MySessionComponentContractSystem>();
      MyAPIGateway.SpectatorTools = MySession.Static == null ? (IMySpectatorTools) null : (IMySpectatorTools) MySession.Static.GetComponent<MySessionComponentSpectatorTools>();
    }

    [StaticEventOwner]
    public class MyMultiplayer : IMyMultiplayer
    {
      public static MyModAPIHelper.MyMultiplayer Static;
      private const int UNRELIABLE_MAX_SIZE = 1024;
      private static Dictionary<ushort, List<Action<byte[]>>> m_registeredListeners = new Dictionary<ushort, List<Action<byte[]>>>();
      private static Dictionary<ushort, List<Action<ushort, byte[], ulong, bool>>> m_registeredSecureListeners = new Dictionary<ushort, List<Action<ushort, byte[], ulong, bool>>>();

      static MyMultiplayer() => MyModAPIHelper.MyMultiplayer.Static = new MyModAPIHelper.MyMultiplayer();

      public bool MultiplayerActive => Sync.MultiplayerActive;

      public bool IsServer => Sync.IsServer;

      public ulong ServerId => Sync.ServerId;

      public ulong MyId => Sync.MyId;

      public string MyName => Sync.MyName;

      public IMyPlayerCollection Players => (IMyPlayerCollection) Sync.Players;

      public bool IsServerPlayer(IMyNetworkClient player) => player is MyNetworkClient && (player as MyNetworkClient).IsGameServer();

      public void SendEntitiesCreated(List<MyObjectBuilder_EntityBase> objectBuilders)
      {
      }

      public bool SendMessageToServer(ushort id, byte[] message, bool reliable)
      {
        if (!reliable && message.Length > 1024)
          return false;
        if (reliable)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<ushort, byte[], ulong>((Func<IMyEventOwner, Action<ushort, byte[], ulong>>) (s => new Action<ushort, byte[], ulong>(MyModAPIHelper.MyMultiplayer.ModMessageServerReliable)), id, message, Sync.ServerId);
        else
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<ushort, byte[], ulong>((Func<IMyEventOwner, Action<ushort, byte[], ulong>>) (s => new Action<ushort, byte[], ulong>(MyModAPIHelper.MyMultiplayer.ModMessageServerUnreliable)), id, message, Sync.ServerId);
        return true;
      }

      public bool SendMessageToOthers(ushort id, byte[] message, bool reliable)
      {
        if (!reliable && message.Length > 1024)
          return false;
        if (this.IsServer)
        {
          if (reliable)
            Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<ushort, byte[]>((Func<IMyEventOwner, Action<ushort, byte[]>>) (s => new Action<ushort, byte[]>(MyModAPIHelper.MyMultiplayer.ModMessageBroadcastReliableFromServer)), id, message);
          else
            Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<ushort, byte[]>((Func<IMyEventOwner, Action<ushort, byte[]>>) (s => new Action<ushort, byte[]>(MyModAPIHelper.MyMultiplayer.ModMessageBroadcastUnreliableFromServer)), id, message);
        }
        else if (reliable)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<ushort, byte[]>((Func<IMyEventOwner, Action<ushort, byte[]>>) (s => new Action<ushort, byte[]>(MyModAPIHelper.MyMultiplayer.ModMessageBroadcastReliable)), id, message);
        else
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<ushort, byte[]>((Func<IMyEventOwner, Action<ushort, byte[]>>) (s => new Action<ushort, byte[]>(MyModAPIHelper.MyMultiplayer.ModMessageBroadcastUnreliable)), id, message);
        return true;
      }

      public bool SendMessageTo(ushort id, byte[] message, ulong recipient, bool reliable)
      {
        if (!reliable && message.Length > 1024)
          return false;
        if (reliable)
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<ushort, byte[], ulong>((Func<IMyEventOwner, Action<ushort, byte[], ulong>>) (s => new Action<ushort, byte[], ulong>(MyModAPIHelper.MyMultiplayer.ModMessageClientReliable)), id, message, recipient, new EndpointId(recipient));
        else
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<ushort, byte[], ulong>((Func<IMyEventOwner, Action<ushort, byte[], ulong>>) (s => new Action<ushort, byte[], ulong>(MyModAPIHelper.MyMultiplayer.ModMessageClientUnreliable)), id, message, recipient, new EndpointId(recipient));
        return true;
      }

      public void JoinServer(string address)
      {
        if (Sandbox.Engine.Platform.Game.IsDedicated && this.IsServer || MySandboxGame.Static == null)
          return;
        MySandboxGame.Static.Invoke((Action) (() =>
        {
          MySessionLoader.UnloadAndExitToMenu();
          MyGameService.OnPingServerResponded += new EventHandler<MyGameServerItem>(MySandboxGame.Static.ServerResponded);
          MyGameService.OnPingServerFailedToRespond += new EventHandler(MySandboxGame.Static.ServerFailedToRespond);
          MyGameService.PingServer(address);
        }), "UnloadAndExitToMenu");
      }

      public void RegisterMessageHandler(ushort id, Action<byte[]> messageHandler)
      {
        if (Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
          throw new InvalidOperationException("Modifying message handlers from another thread is not supported!");
        List<Action<byte[]>> actionList = (List<Action<byte[]>>) null;
        if (MyModAPIHelper.MyMultiplayer.m_registeredListeners.TryGetValue(id, out actionList))
        {
          actionList.Add(messageHandler);
        }
        else
        {
          MyModAPIHelper.MyMultiplayer.m_registeredListeners[id] = new List<Action<byte[]>>();
          MyModAPIHelper.MyMultiplayer.m_registeredListeners[id].Add(messageHandler);
        }
      }

      public void UnregisterMessageHandler(ushort id, Action<byte[]> messageHandler)
      {
        if (Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
          throw new InvalidOperationException("Modifying message handlers from another thread is not supported!");
        List<Action<byte[]>> actionList = (List<Action<byte[]>>) null;
        if (!MyModAPIHelper.MyMultiplayer.m_registeredListeners.TryGetValue(id, out actionList))
          return;
        actionList.Remove(messageHandler);
      }

      public void RegisterSecureMessageHandler(
        ushort id,
        Action<ushort, byte[], ulong, bool> messageHandler)
      {
        if (Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
          throw new InvalidOperationException("Modifying message handlers from another thread is not supported!");
        List<Action<ushort, byte[], ulong, bool>> actionList = (List<Action<ushort, byte[], ulong, bool>>) null;
        if (MyModAPIHelper.MyMultiplayer.m_registeredSecureListeners.TryGetValue(id, out actionList))
        {
          actionList.Add(messageHandler);
        }
        else
        {
          MyModAPIHelper.MyMultiplayer.m_registeredSecureListeners[id] = new List<Action<ushort, byte[], ulong, bool>>();
          MyModAPIHelper.MyMultiplayer.m_registeredSecureListeners[id].Add(messageHandler);
        }
      }

      public void UnregisterSecureMessageHandler(
        ushort id,
        Action<ushort, byte[], ulong, bool> messageHandler)
      {
        if (Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
          throw new InvalidOperationException("Modifying message handlers from another thread is not supported!");
        List<Action<ushort, byte[], ulong, bool>> actionList = (List<Action<ushort, byte[], ulong, bool>>) null;
        if (!MyModAPIHelper.MyMultiplayer.m_registeredSecureListeners.TryGetValue(id, out actionList))
          return;
        actionList.Remove(messageHandler);
      }

      [Event(null, 242)]
      [Reliable]
      [Server]
      private static void ModMessageServerReliable(ushort id, byte[] message, ulong recipient) => MyModAPIHelper.MyMultiplayer.HandleMessageClient(id, message, recipient);

      [Event(null, 248)]
      [Server]
      private static void ModMessageServerUnreliable(ushort id, byte[] message, ulong recipient) => MyModAPIHelper.MyMultiplayer.HandleMessageClient(id, message, recipient);

      [Event(null, 254)]
      [Reliable]
      [Server]
      [Client]
      private static void ModMessageClientReliable(ushort id, byte[] message, ulong recipient) => MyModAPIHelper.MyMultiplayer.HandleMessageClient(id, message, recipient);

      [Event(null, 260)]
      [Server]
      [Client]
      private static void ModMessageClientUnreliable(ushort id, byte[] message, ulong recipient) => MyModAPIHelper.MyMultiplayer.HandleMessageClient(id, message, recipient);

      [Event(null, 266)]
      [Reliable]
      [Server]
      [BroadcastExcept]
      private static void ModMessageBroadcastReliable(ushort id, byte[] message) => MyModAPIHelper.MyMultiplayer.HandleMessage(id, message);

      [Event(null, 272)]
      [Server]
      [BroadcastExcept]
      private static void ModMessageBroadcastUnreliable(ushort id, byte[] message) => MyModAPIHelper.MyMultiplayer.HandleMessage(id, message);

      [Event(null, 278)]
      [Reliable]
      [BroadcastExcept]
      private static void ModMessageBroadcastReliableFromServer(ushort id, byte[] message) => MyModAPIHelper.MyMultiplayer.HandleMessage(id, message);

      [Event(null, 284)]
      [BroadcastExcept]
      private static void ModMessageBroadcastUnreliableFromServer(ushort id, byte[] message) => MyModAPIHelper.MyMultiplayer.HandleMessage(id, message);

      private static void HandleMessageClient(ushort id, byte[] message, ulong recipient)
      {
        if ((long) recipient != (long) Sync.MyId)
          return;
        MyModAPIHelper.MyMultiplayer.HandleMessage(id, message);
      }

      private static void HandleMessage(ushort id, byte[] message)
      {
        List<Action<byte[]>> actionList1 = (List<Action<byte[]>>) null;
        if (MyModAPIHelper.MyMultiplayer.m_registeredListeners.TryGetValue(id, out actionList1) && actionList1 != null)
        {
          foreach (Action<byte[]> action in actionList1)
            action(message);
        }
        List<Action<ushort, byte[], ulong, bool>> actionList2 = (List<Action<ushort, byte[], ulong, bool>>) null;
        if (!MyModAPIHelper.MyMultiplayer.m_registeredSecureListeners.TryGetValue(id, out actionList2) || actionList2 == null)
          return;
        MyEventContext current = MyEventContext.Current;
        if (!current.IsLocallyInvoked && (MyEventContext.Current.Sender.IsNull || !MyEventContext.Current.Sender.IsValid))
          return;
        current = MyEventContext.Current;
        ulong num;
        bool flag;
        if (current.IsLocallyInvoked)
        {
          num = Sync.IsDedicated ? 0UL : MySession.Static.LocalHumanPlayer.Id.SteamId;
          flag = MySession.Static.IsServer;
        }
        else
        {
          ulong steamId = MyEventContext.Current.Sender.Value;
          flag = (long) steamId == (long) Sync.ServerId;
          MyNetworkClient client;
          if (!Sync.Clients.TryGetClient(steamId, out client))
            return;
          num = client.SteamUserId;
        }
        foreach (Action<ushort, byte[], ulong, bool> action in actionList2)
          action(id, message, num, flag);
      }

      public void ReplicateEntityForClient(long entityId, ulong steamId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, ulong>((Func<IMyEventOwner, Action<long, ulong>>) (x => new Action<long, ulong>(MyModAPIHelper.MyMultiplayer.ReplicateEntity_Implmentation)), entityId, steamId);

      [Event(null, 356)]
      [Reliable]
      [Server]
      private static void ReplicateEntity_Implmentation(long entityId, ulong steamId)
      {
      }

      protected sealed class ModMessageServerReliable\u003C\u003ESystem_UInt16\u0023System_Byte\u003C\u0023\u003E\u0023System_UInt64 : ICallSite<IMyEventOwner, ushort, byte[], ulong, DBNull, DBNull, DBNull>
      {
        public virtual void Invoke(
          in IMyEventOwner _param1,
          in ushort id,
          in byte[] message,
          in ulong recipient,
          in DBNull arg4,
          in DBNull arg5,
          in DBNull arg6)
        {
          MyModAPIHelper.MyMultiplayer.ModMessageServerReliable(id, message, recipient);
        }
      }

      protected sealed class ModMessageServerUnreliable\u003C\u003ESystem_UInt16\u0023System_Byte\u003C\u0023\u003E\u0023System_UInt64 : ICallSite<IMyEventOwner, ushort, byte[], ulong, DBNull, DBNull, DBNull>
      {
        public virtual void Invoke(
          in IMyEventOwner _param1,
          in ushort id,
          in byte[] message,
          in ulong recipient,
          in DBNull arg4,
          in DBNull arg5,
          in DBNull arg6)
        {
          MyModAPIHelper.MyMultiplayer.ModMessageServerUnreliable(id, message, recipient);
        }
      }

      protected sealed class ModMessageClientReliable\u003C\u003ESystem_UInt16\u0023System_Byte\u003C\u0023\u003E\u0023System_UInt64 : ICallSite<IMyEventOwner, ushort, byte[], ulong, DBNull, DBNull, DBNull>
      {
        public virtual void Invoke(
          in IMyEventOwner _param1,
          in ushort id,
          in byte[] message,
          in ulong recipient,
          in DBNull arg4,
          in DBNull arg5,
          in DBNull arg6)
        {
          MyModAPIHelper.MyMultiplayer.ModMessageClientReliable(id, message, recipient);
        }
      }

      protected sealed class ModMessageClientUnreliable\u003C\u003ESystem_UInt16\u0023System_Byte\u003C\u0023\u003E\u0023System_UInt64 : ICallSite<IMyEventOwner, ushort, byte[], ulong, DBNull, DBNull, DBNull>
      {
        public virtual void Invoke(
          in IMyEventOwner _param1,
          in ushort id,
          in byte[] message,
          in ulong recipient,
          in DBNull arg4,
          in DBNull arg5,
          in DBNull arg6)
        {
          MyModAPIHelper.MyMultiplayer.ModMessageClientUnreliable(id, message, recipient);
        }
      }

      protected sealed class ModMessageBroadcastReliable\u003C\u003ESystem_UInt16\u0023System_Byte\u003C\u0023\u003E : ICallSite<IMyEventOwner, ushort, byte[], DBNull, DBNull, DBNull, DBNull>
      {
        public virtual void Invoke(
          in IMyEventOwner _param1,
          in ushort id,
          in byte[] message,
          in DBNull arg3,
          in DBNull arg4,
          in DBNull arg5,
          in DBNull arg6)
        {
          MyModAPIHelper.MyMultiplayer.ModMessageBroadcastReliable(id, message);
        }
      }

      protected sealed class ModMessageBroadcastUnreliable\u003C\u003ESystem_UInt16\u0023System_Byte\u003C\u0023\u003E : ICallSite<IMyEventOwner, ushort, byte[], DBNull, DBNull, DBNull, DBNull>
      {
        public virtual void Invoke(
          in IMyEventOwner _param1,
          in ushort id,
          in byte[] message,
          in DBNull arg3,
          in DBNull arg4,
          in DBNull arg5,
          in DBNull arg6)
        {
          MyModAPIHelper.MyMultiplayer.ModMessageBroadcastUnreliable(id, message);
        }
      }

      protected sealed class ModMessageBroadcastReliableFromServer\u003C\u003ESystem_UInt16\u0023System_Byte\u003C\u0023\u003E : ICallSite<IMyEventOwner, ushort, byte[], DBNull, DBNull, DBNull, DBNull>
      {
        public virtual void Invoke(
          in IMyEventOwner _param1,
          in ushort id,
          in byte[] message,
          in DBNull arg3,
          in DBNull arg4,
          in DBNull arg5,
          in DBNull arg6)
        {
          MyModAPIHelper.MyMultiplayer.ModMessageBroadcastReliableFromServer(id, message);
        }
      }

      protected sealed class ModMessageBroadcastUnreliableFromServer\u003C\u003ESystem_UInt16\u0023System_Byte\u003C\u0023\u003E : ICallSite<IMyEventOwner, ushort, byte[], DBNull, DBNull, DBNull, DBNull>
      {
        public virtual void Invoke(
          in IMyEventOwner _param1,
          in ushort id,
          in byte[] message,
          in DBNull arg3,
          in DBNull arg4,
          in DBNull arg5,
          in DBNull arg6)
        {
          MyModAPIHelper.MyMultiplayer.ModMessageBroadcastUnreliableFromServer(id, message);
        }
      }

      protected sealed class ReplicateEntity_Implmentation\u003C\u003ESystem_Int64\u0023System_UInt64 : ICallSite<IMyEventOwner, long, ulong, DBNull, DBNull, DBNull, DBNull>
      {
        public virtual void Invoke(
          in IMyEventOwner _param1,
          in long entityId,
          in ulong steamId,
          in DBNull arg3,
          in DBNull arg4,
          in DBNull arg5,
          in DBNull arg6)
        {
          MyModAPIHelper.MyMultiplayer.ReplicateEntity_Implmentation(entityId, steamId);
        }
      }
    }
  }
}
