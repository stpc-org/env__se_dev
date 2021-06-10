// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MySyncLayer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Engine.Multiplayer;
using System;
using System.Reflection;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Multiplayer
{
  public class MySyncLayer
  {
    internal readonly MyTransportLayer TransportLayer;
    internal readonly MyClientCollection Clients;

    internal MySyncLayer(MyTransportLayer transportLayer)
    {
      this.TransportLayer = transportLayer;
      this.Clients = new MyClientCollection();
    }

    internal void RegisterClientEvents(MyMultiplayerBase multiplayer)
    {
      multiplayer.ClientJoined += new Action<ulong, string>(this.OnClientJoined);
      multiplayer.ClientLeft += new Action<ulong, MyChatMemberStateChangeEnum>(this.OnClientLeft);
      foreach (ulong member in multiplayer.Members)
      {
        if ((long) member != (long) Sync.MyId)
          this.OnClientJoined(member, multiplayer.GetMemberName(member));
      }
    }

    private void OnClientJoined(ulong steamUserId, string userName)
    {
      if (this.Clients.HasClient(steamUserId))
        return;
      this.Clients.AddClient(steamUserId, userName);
    }

    private void OnClientLeft(ulong steamUserId, MyChatMemberStateChangeEnum leaveReason) => this.Clients.RemoveClient(steamUserId);

    public static bool CheckSendPermissions(ulong target, MyMessagePermissions permission)
    {
      bool flag;
      switch (permission)
      {
        case MyMessagePermissions.FromServer:
          flag = Sync.IsServer;
          break;
        case MyMessagePermissions.ToServer:
          flag = (long) Sync.ServerId == (long) target;
          break;
        case MyMessagePermissions.FromServer | MyMessagePermissions.ToServer:
          flag = (long) Sync.ServerId == (long) target || Sync.IsServer;
          break;
        default:
          flag = false;
          break;
      }
      return flag;
    }

    public static bool CheckReceivePermissions(ulong sender, MyMessagePermissions permission)
    {
      bool flag;
      switch (permission)
      {
        case MyMessagePermissions.FromServer:
          flag = (long) Sync.ServerId == (long) sender;
          break;
        case MyMessagePermissions.ToServer:
          flag = Sync.IsServer;
          break;
        case MyMessagePermissions.FromServer | MyMessagePermissions.ToServer:
          flag = (long) Sync.ServerId == (long) sender || Sync.IsServer;
          break;
        default:
          flag = false;
          break;
      }
      return flag;
    }

    internal static ISerializer<TMsg> GetSerializer<TMsg>() where TMsg : struct
    {
      if (Attribute.IsDefined((MemberInfo) typeof (TMsg), typeof (ProtoContractAttribute)))
        return MySyncLayer.CreateProto<TMsg>();
      if (!BlittableHelper<TMsg>.IsBlittable)
        return (ISerializer<TMsg>) null;
      return (ISerializer<TMsg>) Activator.CreateInstance(typeof (BlitSerializer<>).MakeGenericType(typeof (TMsg)));
    }

    private static ISerializer<TMsg> CreateProto<TMsg>() => (ISerializer<TMsg>) MySyncLayer.DefaultProtoSerializer<TMsg>.Default;

    private static ISerializer<TMsg> CreateBlittable<TMsg>() where TMsg : unmanaged => (ISerializer<TMsg>) BlitSerializer<TMsg>.Default;

    private class DefaultProtoSerializer<T>
    {
      public static readonly ProtoSerializer<T> Default = new ProtoSerializer<T>(MyObjectBuilderSerializer.Serializer);
    }
  }
}
