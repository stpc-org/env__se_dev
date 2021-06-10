// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Networking.MyNetworkMonitor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using VRage.GameServices;
using VRage.Profiler;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
  public static class MyNetworkMonitor
  {
    public static volatile int UpdateLatency = 8;
    private static Thread m_workerThread;
    private static bool m_sessionEnabled;
    private static bool m_running;

    public static event Action OnTick;

    public static void Init()
    {
      MyNetworkMonitor.m_running = true;
      if (MyFakes.NETWORK_SINGLE_THREADED || MyNetworkMonitor.m_workerThread != null)
        return;
      MyNetworkMonitor.m_workerThread = new Thread(new ThreadStart(MyNetworkMonitor.Worker))
      {
        CurrentCulture = CultureInfo.InvariantCulture,
        CurrentUICulture = CultureInfo.InvariantCulture,
        Name = "Network Monitor"
      };
      MyNetworkMonitor.m_workerThread.Start();
    }

    public static void Done()
    {
      MyNetworkMonitor.m_running = false;
      if (MyNetworkMonitor.m_workerThread == null)
        return;
      MyNetworkMonitor.m_workerThread.Join();
      MyNetworkMonitor.m_workerThread = (Thread) null;
    }

    public static void StartSession() => MyNetworkMonitor.m_sessionEnabled = true;

    public static void EndSession() => MyNetworkMonitor.m_sessionEnabled = false;

    public static void Update()
    {
      if (MyNetworkMonitor.m_workerThread != null)
        return;
      MyNetworkMonitor.UpdateInternal();
    }

    private static int UpdateInternal()
    {
      MyGameService.UpdateNetworkThread();
      IMyPeer2Peer peer2Peer = MyGameService.Peer2Peer;
      int num = MyNetworkMonitor.UpdateLatency;
      if (peer2Peer != null)
      {
        MyNetworkWriter.SendAll();
        peer2Peer.BeginFrameProcessing();
        try
        {
          if (MyNetworkMonitor.m_sessionEnabled)
            MyNetworkReader.ReceiveAll();
        }
        finally
        {
          peer2Peer.EndFrameProcessing();
        }
        num = peer2Peer.NetworkUpdateLatency;
      }
      Action onTick = MyNetworkMonitor.OnTick;
      if (onTick != null)
        onTick();
      ProfilerShort.Commit();
      return num;
    }

    private static void Worker()
    {
      try
      {
        ProfilerShort.Autocommit = false;
        int val2 = MyNetworkMonitor.UpdateLatency;
        while (MyNetworkMonitor.m_running)
        {
          Thread.Sleep(Math.Max(Math.Min(MyNetworkMonitor.UpdateLatency, val2), 1));
          val2 = MyNetworkMonitor.UpdateInternal();
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(ex);
        Debugger.Break();
        throw;
      }
    }
  }
}
