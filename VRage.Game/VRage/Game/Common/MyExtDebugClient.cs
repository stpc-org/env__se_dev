// Decompiled with JetBrains decompiler
// Type: VRage.Game.Common.MyExtDebugClient
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
using VRage.Game.Debugging;
using VRage.Serialization;

namespace VRage.Game.Common
{
  public class MyExtDebugClient : IDisposable
  {
    public const int GameDebugPort = 13000;
    private const int MsgSizeLimit = 1048576;
    private TcpClient m_client;
    private readonly byte[] m_arrayBuffer = new byte[1048576];
    private IntPtr m_tempBuffer;
    private Thread m_clientThread;
    private bool m_finished;
    private readonly ConcurrentCachingList<MyExtDebugClient.ReceivedMsgHandler> m_receivedMsgHandlers = new ConcurrentCachingList<MyExtDebugClient.ReceivedMsgHandler>();

    public event MyExtDebugClient.ReceivedMsgHandler ReceivedMsg
    {
      add
      {
        if (this.m_receivedMsgHandlers.Contains<MyExtDebugClient.ReceivedMsgHandler>(value))
          return;
        this.m_receivedMsgHandlers.Add(value);
        this.m_receivedMsgHandlers.ApplyAdditions();
      }
      remove
      {
        if (!this.m_receivedMsgHandlers.Contains<MyExtDebugClient.ReceivedMsgHandler>(value))
          return;
        this.m_receivedMsgHandlers.Remove(value);
        this.m_receivedMsgHandlers.ApplyRemovals();
      }
    }

    public bool ConnectedToGame => this.m_client != null && this.m_client.Connected;

    public MyExtDebugClient()
    {
      this.m_tempBuffer = Marshal.AllocHGlobal(1048576);
      this.m_finished = false;
      this.m_clientThread = new Thread(new ThreadStart(this.ClientThreadProc))
      {
        IsBackground = true
      };
      this.m_clientThread.Start();
    }

    public void Dispose()
    {
      this.m_finished = true;
      if (this.m_client != null)
      {
        if (this.m_client.Client.Connected)
          this.m_client.Client.Disconnect(false);
        this.m_client.Close();
      }
      Marshal.FreeHGlobal(this.m_tempBuffer);
    }

    private void ClientThreadProc()
    {
      while (!this.m_finished)
      {
        if (this.m_client == null || this.m_client.Client == null || !this.m_client.Connected)
        {
          try
          {
            this.m_client = new TcpClient();
            this.m_client.Connect(IPAddress.Loopback, 13000);
          }
          catch (Exception ex)
          {
          }
          if (this.m_client == null || this.m_client.Client == null || !this.m_client.Connected)
          {
            Thread.Sleep(2500);
            continue;
          }
        }
        try
        {
          if (this.m_client.Client != null)
          {
            if (this.m_client.Client.Receive(this.m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize, SocketFlags.None) == 0)
            {
              this.m_client.Client.Close();
              this.m_client.Client = (Socket) null;
              this.m_client = (TcpClient) null;
            }
            else
            {
              Marshal.Copy(this.m_arrayBuffer, 0, this.m_tempBuffer, MyExternalDebugStructures.MsgHeaderSize);
              MyExternalDebugStructures.CommonMsgHeader structure = (MyExternalDebugStructures.CommonMsgHeader) Marshal.PtrToStructure(this.m_tempBuffer, typeof (MyExternalDebugStructures.CommonMsgHeader));
              if (structure.IsValid)
              {
                if (structure.MsgSize > 0)
                {
                  this.m_client.Client.Receive(this.m_arrayBuffer, structure.MsgSize, SocketFlags.None);
                  Marshal.Copy(this.m_arrayBuffer, 0, this.m_tempBuffer, structure.MsgSize);
                }
                if (this.m_receivedMsgHandlers != null)
                {
                  foreach (MyExtDebugClient.ReceivedMsgHandler receivedMsgHandler in this.m_receivedMsgHandlers)
                  {
                    if (receivedMsgHandler != null)
                      receivedMsgHandler(structure, this.m_arrayBuffer);
                  }
                }
              }
            }
          }
        }
        catch (SocketException ex)
        {
          if (this.m_client.Client != null)
          {
            this.m_client.Client.Close();
            this.m_client.Client = (Socket) null;
            this.m_client = (TcpClient) null;
          }
        }
        catch (ObjectDisposedException ex)
        {
          if (this.m_client.Client != null)
          {
            this.m_client.Client.Close();
            this.m_client.Client = (Socket) null;
            this.m_client = (TcpClient) null;
          }
        }
        catch (Exception ex)
        {
        }
      }
    }

    public bool SendMessageToGame<TMessage>(TMessage msg) where TMessage : struct, MyExternalDebugStructures.IExternalDebugMsg
    {
      if (this.m_client == null || this.m_client.Client == null || !this.m_client.Connected)
        return false;
      ISerializer<TMessage> serializer = MyExternalDebugStructures.GetSerializer<TMessage>();
      ByteStream byteStream = new ByteStream(256);
      ByteStream destination = byteStream;
      ref TMessage local = ref msg;
      serializer.Serialize(destination, ref local);
      Marshal.StructureToPtr<MyExternalDebugStructures.CommonMsgHeader>(MyExternalDebugStructures.CommonMsgHeader.Create(msg.GetTypeStr(), (int) byteStream.Position), this.m_tempBuffer, true);
      Marshal.Copy(this.m_tempBuffer, this.m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize);
      Array.Copy((Array) byteStream.Data, 0L, (Array) this.m_arrayBuffer, (long) MyExternalDebugStructures.MsgHeaderSize, byteStream.Position);
      try
      {
        this.m_client.Client.Send(this.m_arrayBuffer, 0, MyExternalDebugStructures.MsgHeaderSize + (int) byteStream.Position, SocketFlags.None);
      }
      catch (SocketException ex)
      {
        return false;
      }
      return true;
    }

    public delegate void ReceivedMsgHandler(
      MyExternalDebugStructures.CommonMsgHeader messageHeader,
      byte[] messageData);
  }
}
