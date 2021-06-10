// Decompiled with JetBrains decompiler
// Type: VRage.Game.SessionComponents.MySessionComponentExtDebug
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Debugging;
using VRage.Game.ModAPI;
using VRage.Library;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
  public class MySessionComponentExtDebug : MySessionComponentBase
  {
    public static MySessionComponentExtDebug Static;
    public static bool ForceDisable;
    public const int GameDebugPort = 13000;
    private const int MsgSizeLimit = 1048576;
    private Thread m_listenerThread;
    private TcpListener m_listener;
    private ConcurrentCachingList<MySessionComponentExtDebug.MyDebugClientInfo> m_clients = new ConcurrentCachingList<MySessionComponentExtDebug.MyDebugClientInfo>(1);
    private bool m_active;
    private byte[] m_arrayBuffer = new byte[1048576];
    private NativeArray m_tempBuffer;
    private ConcurrentCachingList<MySessionComponentExtDebug.ReceivedMsgHandler> m_receivedMsgHandlers = new ConcurrentCachingList<MySessionComponentExtDebug.ReceivedMsgHandler>();

    public event MySessionComponentExtDebug.ReceivedMsgHandler ReceivedMsg
    {
      add
      {
        this.m_receivedMsgHandlers.Add(value);
        this.m_receivedMsgHandlers.ApplyAdditions();
      }
      remove
      {
        if (!this.m_receivedMsgHandlers.Contains<MySessionComponentExtDebug.ReceivedMsgHandler>(value))
          return;
        this.m_receivedMsgHandlers.Remove(value);
        this.m_receivedMsgHandlers.ApplyRemovals();
      }
    }

    public bool IsHandlerRegistered(
      MySessionComponentExtDebug.ReceivedMsgHandler handler)
    {
      return this.m_receivedMsgHandlers.Contains<MySessionComponentExtDebug.ReceivedMsgHandler>(handler);
    }

    public bool HasClients => this.m_clients.Count > 0;

    public override void LoadData()
    {
      if (MySessionComponentExtDebug.Static != null)
      {
        this.m_listenerThread = MySessionComponentExtDebug.Static.m_listenerThread;
        this.m_listener = MySessionComponentExtDebug.Static.m_listener;
        this.m_clients = MySessionComponentExtDebug.Static.m_clients;
        this.m_active = MySessionComponentExtDebug.Static.m_active;
        this.m_arrayBuffer = MySessionComponentExtDebug.Static.m_arrayBuffer;
        this.m_tempBuffer = MySessionComponentExtDebug.Static.m_tempBuffer;
        MySessionComponentExtDebug.Static.m_tempBuffer = (NativeArray) null;
        this.m_receivedMsgHandlers = MySessionComponentExtDebug.Static.m_receivedMsgHandlers;
        MySessionComponentExtDebug.Static = this;
        base.LoadData();
      }
      else
      {
        MySessionComponentExtDebug.Static = this;
        if (!MySessionComponentExtDebug.ForceDisable && MyVRage.Platform.System.IsRemoteDebuggingSupported)
        {
          this.m_tempBuffer = MyDebug.DebugMemoryAllocator.Allocate(1048576);
          this.StartServer();
        }
        base.LoadData();
      }
    }

    protected override void UnloadData()
    {
      this.m_receivedMsgHandlers.ClearImmediate();
      base.UnloadData();
      this.Session = (IMySession) null;
    }

    public void Dispose()
    {
      this.m_receivedMsgHandlers.ClearList();
      if (this.m_active)
        this.StopServer();
      if (this.m_tempBuffer == null)
        return;
      MyDebug.DebugMemoryAllocator.Dispose(this.m_tempBuffer);
    }

    private bool StartServer()
    {
      if (this.m_active)
        return false;
      this.m_listenerThread = new Thread(new ThreadStart(MySessionComponentExtDebug.ServerListenerProc))
      {
        IsBackground = true
      };
      this.m_listenerThread.Start();
      this.m_active = true;
      return true;
    }

    private void StopServer()
    {
      if (!this.m_active || this.m_listenerThread == null)
        return;
      this.m_listener.Stop();
      foreach (MySessionComponentExtDebug.MyDebugClientInfo client in this.m_clients)
      {
        if (client.TcpClient != null)
        {
          client.TcpClient.Client.Disconnect(true);
          client.TcpClient.Close();
        }
      }
      this.m_clients.ClearImmediate();
      this.m_active = false;
    }

    private static void ServerListenerProc()
    {
      Thread.CurrentThread.Name = "External Debugging Listener";
      TcpListener tcpListener;
      try
      {
        tcpListener = new TcpListener(IPAddress.Loopback, 13000)
        {
          ExclusiveAddressUse = false
        };
        tcpListener.Start();
        MySessionComponentExtDebug.Static.m_listener = tcpListener;
      }
      catch (SocketException ex)
      {
        MyLog.Default.WriteLine("Cannot start debug listener.");
        MyLog.Default.WriteLine((Exception) ex);
        MySessionComponentExtDebug.Static.m_active = false;
        return;
      }
      MyLog.Default.WriteLine("External debugger: listening...");
      while (true)
      {
        try
        {
          TcpClient tcpClient = tcpListener.AcceptTcpClient();
          tcpClient.Client.Blocking = true;
          MyLog.Default.WriteLine("External debugger: accepted client.");
          MySessionComponentExtDebug.Static.m_clients.Add(new MySessionComponentExtDebug.MyDebugClientInfo()
          {
            TcpClient = tcpClient,
            IsHeaderValid = false,
            Header = new MyExternalDebugStructures.CommonMsgHeader()
          });
          MySessionComponentExtDebug.Static.m_clients.ApplyAdditions();
        }
        catch (SocketException ex)
        {
          if (ex.SocketErrorCode == SocketError.Interrupted)
          {
            tcpListener.Stop();
            MyLog.Default.WriteLine("External debugger: interrupted.");
            return;
          }
          if (MyLog.Default != null)
          {
            if (MyLog.Default.LogEnabled)
            {
              MyLog.Default.WriteLine((Exception) ex);
              break;
            }
            break;
          }
          break;
        }
      }
      tcpListener.Stop();
      MySessionComponentExtDebug.Static.m_listener = (TcpListener) null;
    }

    public override void UpdateBeforeSimulation()
    {
      foreach (MySessionComponentExtDebug.MyDebugClientInfo client in this.m_clients)
      {
        if (client == null || client.TcpClient == null || (client.TcpClient.Client == null || !client.TcpClient.Connected))
        {
          if (client != null && client.TcpClient != null && (client.TcpClient.Client != null && client.TcpClient.Client.Connected))
          {
            client.TcpClient.Client.Disconnect(true);
            client.TcpClient.Close();
          }
          this.m_clients.Remove(client);
        }
        else if (client.TcpClient.Connected && client.TcpClient.Available > 0)
          this.ReadMessagesFromClients(client);
      }
      this.m_clients.ApplyRemovals();
    }

    private void ReadMessagesFromClients(
      MySessionComponentExtDebug.MyDebugClientInfo clientInfo)
    {
      Socket client = clientInfo.TcpClient.Client;
      while (client.Available >= 0)
      {
        bool flag = false;
        if (!clientInfo.IsHeaderValid && client.Available >= MyExternalDebugStructures.MsgHeaderSize)
        {
          client.Receive(this.m_arrayBuffer, MyExternalDebugStructures.MsgHeaderSize, SocketFlags.None);
          Marshal.Copy(this.m_arrayBuffer, 0, this.m_tempBuffer.Ptr, MyExternalDebugStructures.MsgHeaderSize);
          MyExternalDebugStructures.CommonMsgHeader structure = (MyExternalDebugStructures.CommonMsgHeader) Marshal.PtrToStructure(this.m_tempBuffer.Ptr, typeof (MyExternalDebugStructures.CommonMsgHeader));
          clientInfo.IsHeaderValid = true;
          clientInfo.Header = structure;
          flag = true;
        }
        if (clientInfo.IsHeaderValid && client.Available >= clientInfo.Header.MsgSize)
        {
          if (clientInfo.Header.MsgSize > 0)
            client.Receive(this.m_arrayBuffer, clientInfo.Header.MsgSize, SocketFlags.None);
          if (this.m_receivedMsgHandlers != null && this.m_receivedMsgHandlers.Count > 0)
          {
            foreach (MySessionComponentExtDebug.ReceivedMsgHandler receivedMsgHandler in this.m_receivedMsgHandlers)
            {
              if (receivedMsgHandler != null)
                receivedMsgHandler(clientInfo.Header, this.m_arrayBuffer);
            }
          }
          clientInfo.IsHeaderValid = false;
          flag = true;
        }
        if (!flag)
          break;
      }
    }

    public void SendMessageToClients<TMessage>(TMessage msg) where TMessage : struct, MyExternalDebugStructures.IExternalDebugMsg
    {
      if (this.m_tempBuffer == null)
        return;
      ISerializer<TMessage> serializer = MyExternalDebugStructures.GetSerializer<TMessage>();
      ByteStream byteStream = new ByteStream(256);
      ByteStream destination = byteStream;
      ref TMessage local = ref msg;
      serializer.Serialize(destination, ref local);
      Marshal.StructureToPtr<MyExternalDebugStructures.CommonMsgHeader>(MyExternalDebugStructures.CommonMsgHeader.Create(msg.GetTypeStr(), (int) byteStream.Position), this.m_tempBuffer.Ptr, true);
      Marshal.Copy(this.m_tempBuffer.Ptr, this.m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize);
      Array.Copy((Array) byteStream.Data, 0L, (Array) this.m_arrayBuffer, (long) MyExternalDebugStructures.MsgHeaderSize, byteStream.Position);
      foreach (MySessionComponentExtDebug.MyDebugClientInfo client in this.m_clients)
      {
        try
        {
          if (client.TcpClient.Client != null)
            client.TcpClient.Client.Send(this.m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize + (int) byteStream.Position, SocketFlags.None);
        }
        catch (SocketException ex)
        {
          client.TcpClient.Close();
        }
      }
    }

    private class MyDebugClientInfo
    {
      public TcpClient TcpClient;
      public bool IsHeaderValid;
      public MyExternalDebugStructures.CommonMsgHeader Header;
    }

    public delegate void ReceivedMsgHandler(
      MyExternalDebugStructures.CommonMsgHeader messageHeader,
      byte[] messageData);
  }
}
