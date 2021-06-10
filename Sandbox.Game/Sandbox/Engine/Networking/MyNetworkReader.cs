// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Networking.MyNetworkReader
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
  internal static class MyNetworkReader
  {
    private static int m_byteCountReceived;
    private static int m_tamperred;
    private static readonly ConcurrentDictionary<int, MyNetworkReader.ChannelInfo> m_channels = new ConcurrentDictionary<int, MyNetworkReader.ChannelInfo>();

    public static void SetHandler(
      int channel,
      NetworkMessageDelegate handler,
      Action<ulong> disconnectPeerOnError)
    {
      MyNetworkReader.ChannelInfo channelInfo1;
      if (MyNetworkReader.m_channels.TryGetValue(channel, out channelInfo1))
        channelInfo1.Queue.Dispose();
      MyNetworkReader.ChannelInfo channelInfo2 = new MyNetworkReader.ChannelInfo()
      {
        Handler = handler,
        Queue = new MyReceiveQueue(channel, disconnectPeerOnError)
      };
      MyNetworkReader.m_channels[channel] = channelInfo2;
    }

    public static void ClearHandler(int channel)
    {
      MyNetworkReader.ChannelInfo channelInfo;
      if (MyNetworkReader.m_channels.TryGetValue(channel, out channelInfo))
        channelInfo.Queue.Dispose();
      MyNetworkReader.m_channels.Remove<int, MyNetworkReader.ChannelInfo>(channel);
    }

    public static void Clear()
    {
      foreach (KeyValuePair<int, MyNetworkReader.ChannelInfo> channel in MyNetworkReader.m_channels)
        channel.Value.Queue.Dispose();
      MyNetworkReader.m_channels.Clear();
      MyLog.Default.WriteLine("Network readers disposed");
    }

    public static void Process()
    {
      foreach (KeyValuePair<int, MyNetworkReader.ChannelInfo> channel in MyNetworkReader.m_channels)
        channel.Value.Queue.Process(channel.Value.Handler);
    }

    public static void GetAndClearStats(out int received, out int tamperred)
    {
      received = Interlocked.Exchange(ref MyNetworkReader.m_byteCountReceived, 0);
      tamperred = Interlocked.Exchange(ref MyNetworkReader.m_tamperred, 0);
    }

    public static void ReceiveAll()
    {
      int num1 = 0;
      int num2 = 0;
      using (IEnumerator<KeyValuePair<int, MyNetworkReader.ChannelInfo>> enumerator = MyNetworkReader.m_channels.GetEnumerator())
      {
label_6:
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, MyNetworkReader.ChannelInfo> current = enumerator.Current;
          while (true)
          {
            MyReceiveQueue.ReceiveStatus one;
            do
            {
              uint length;
              one = current.Value.Queue.ReceiveOne(out length);
              if (one != MyReceiveQueue.ReceiveStatus.None)
                num1 += (int) length;
              else
                goto label_6;
            }
            while (one != MyReceiveQueue.ReceiveStatus.TamperredPacket);
            ++num2;
          }
        }
      }
      Interlocked.Add(ref MyNetworkReader.m_byteCountReceived, num1);
      Interlocked.Add(ref MyNetworkReader.m_tamperred, num2);
    }

    private class ChannelInfo
    {
      public MyReceiveQueue Queue;
      public NetworkMessageDelegate Handler;
    }
  }
}
