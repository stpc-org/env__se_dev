// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.IntergridCommunication.MyIntergridCommunicationContext
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems.IntergridCommunication
{
  internal class MyIntergridCommunicationContext : IMyIntergridCommunicationSystem
  {
    private HashSet<MyMessageListener> m_pendingCallbacks;
    private HashSet<BroadcastListener> m_broadcastListeners;
    private LRUCache<long, MyIntergridCommunicationContext.ConnectionData> m_connectionCache = new LRUCache<long, MyIntergridCommunicationContext.ConnectionData>(10);
    private static HashSet<MyDataBroadcaster> m_broadcasters;

    private static MyIGCSystemSessionComponent Context => MyIGCSystemSessionComponent.Static;

    public bool IsActive => this.ProgrammableBlock != null;

    public UnicastListener UnicastListener { get; private set; }

    public MyProgrammableBlock ProgrammableBlock { get; private set; }

    public MyIntergridCommunicationContext(MyProgrammableBlock programmableBlock)
    {
      this.ProgrammableBlock = programmableBlock;
      this.ProgrammableBlock.OnClosing += new Action<MyEntity>(this.ProgrammableBlock_OnClosing);
      this.UnicastListener = new UnicastListener(this);
    }

    public long GetAddressOfThisContext() => this.ProgrammableBlock.EntityId;

    public void InvokeSinglePendingCallback() => this.m_pendingCallbacks.FirstElement<MyMessageListener>().InvokeCallback();

    public void RegisterForCallback(MyMessageListener messageListener)
    {
      if (this.m_pendingCallbacks == null)
        this.m_pendingCallbacks = new HashSet<MyMessageListener>();
      if (this.m_pendingCallbacks.Count == 0)
        MyIntergridCommunicationContext.Context.RegisterContextWithPendingCallbacks(this);
      this.m_pendingCallbacks.Add(messageListener);
    }

    public void UnregisterFromCallback(MyMessageListener messageListener)
    {
      this.m_pendingCallbacks.Remove(messageListener);
      if (this.m_pendingCallbacks.Count != 0)
        return;
      MyIntergridCommunicationContext.Context.UnregisterContextWithPendingCallbacks(this);
    }

    public void DisposeBroadcastListener(
      BroadcastListener broadcastListener,
      bool keepIfHavingPendingMessages)
    {
      if (broadcastListener.IsActive)
      {
        broadcastListener.IsActive = false;
        broadcastListener.DisableMessageCallback();
        MyIntergridCommunicationContext.Context.UnregisterBroadcastListener(broadcastListener);
      }
      if (keepIfHavingPendingMessages && broadcastListener.HasPendingMessage)
        return;
      this.m_broadcastListeners.Remove(broadcastListener);
    }

    public bool IsConnectedTo(
      MyIntergridCommunicationContext targetContext,
      TransmissionDistance transmissionDistance)
    {
      long addressOfThisContext = targetContext.GetAddressOfThisContext();
      MyIntergridCommunicationContext.ConnectionData connectionData1 = this.m_connectionCache.Read(addressOfThisContext);
      if (connectionData1 == null)
      {
        connectionData1 = new MyIntergridCommunicationContext.ConnectionData();
        this.m_connectionCache.Write(addressOfThisContext, connectionData1);
      }
      int gameplayFrameCounter = MySession.Static.GameplayFrameCounter;
      if (gameplayFrameCounter >= connectionData1.ValidTill)
      {
        TransmissionDistance? connectionTo = this.EvaluateConnectionTo(targetContext, connectionData1);
        MyIntergridCommunicationContext.ConnectionData connectionData2 = connectionData1;
        TransmissionDistance? nullable = connectionTo;
        int num = nullable.HasValue ? (int) nullable.GetValueOrDefault() : -1;
        connectionData2.ConnectionType = (TransmissionDistance) num;
        connectionData1.ValidTill = gameplayFrameCounter + MyIntergridCommunicationContext.ConnectionData.ValidDurationFrames;
      }
      return connectionData1.ConnectionType != ~TransmissionDistance.CurrentConstruct && connectionData1.ConnectionType <= transmissionDistance;
    }

    private TransmissionDistance? EvaluateConnectionTo(
      MyIntergridCommunicationContext targetContext,
      MyIntergridCommunicationContext.ConnectionData connectionData)
    {
      MyProgrammableBlock programmableBlock1 = this.ProgrammableBlock;
      MyProgrammableBlock programmableBlock2 = targetContext.ProgrammableBlock;
      if (!programmableBlock1.GetUserRelationToOwner(programmableBlock2.OwnerId).IsFriendly())
      {
        connectionData.ReleaseBroadcaster();
        return new TransmissionDistance?();
      }
      MyCubeGrid cubeGrid1 = programmableBlock1.CubeGrid;
      MyCubeGrid cubeGrid2 = programmableBlock2.CubeGrid;
      if (cubeGrid1 == cubeGrid2 || MyCubeGridGroups.Static.Mechanical.HasSameGroup(cubeGrid1, cubeGrid2))
        return new TransmissionDistance?(TransmissionDistance.CurrentConstruct);
      if (MyCubeGridGroups.Static.Logical.HasSameGroup(cubeGrid1, cubeGrid2))
        return new TransmissionDistance?(TransmissionDistance.ConnectedConstructs);
      MyDataBroadcaster target = (MyDataBroadcaster) null;
      if (connectionData.Broadcaster != null && connectionData.Broadcaster.TryGetTarget(out target) && (!target.Closed && MyIntergridCommunicationContext.Context.ConnectionProvider(programmableBlock2, target, programmableBlock1.OwnerId)))
        return new TransmissionDistance?(TransmissionDistance.AntennaRelay);
      using (MyUtils.ReuseCollection<MyDataBroadcaster>(ref MyIntergridCommunicationContext.m_broadcasters))
      {
        MyIntergridCommunicationContext.Context.BroadcasterProvider(programmableBlock1.CubeGrid, MyIntergridCommunicationContext.m_broadcasters, programmableBlock1.OwnerId);
        foreach (MyDataBroadcaster broadcaster in MyIntergridCommunicationContext.m_broadcasters)
        {
          if (MyIntergridCommunicationContext.Context.ConnectionProvider(programmableBlock2, broadcaster, programmableBlock1.OwnerId))
          {
            if (connectionData.Broadcaster == null)
              connectionData.Broadcaster = new WeakReference<MyDataBroadcaster>(broadcaster);
            else
              connectionData.Broadcaster.SetTarget(broadcaster);
            return new TransmissionDistance?(TransmissionDistance.AntennaRelay);
          }
        }
      }
      connectionData.ReleaseBroadcaster();
      return new TransmissionDistance?();
    }

    public void DisposeContext()
    {
      this.UnicastListener.DisableMessageCallback();
      if (this.m_broadcastListeners != null)
      {
        while (this.m_broadcastListeners.Count > 0)
          this.DisposeBroadcastListener(this.m_broadcastListeners.FirstElement<BroadcastListener>(), false);
      }
      if (this.m_pendingCallbacks != null && this.m_pendingCallbacks.Count != 0)
      {
        while (this.m_pendingCallbacks.Count > 0)
          this.UnregisterFromCallback(this.m_pendingCallbacks.FirstElement<MyMessageListener>());
      }
      this.ProgrammableBlock.OnClosing -= new Action<MyEntity>(this.ProgrammableBlock_OnClosing);
      this.ProgrammableBlock = (MyProgrammableBlock) null;
    }

    private void ProgrammableBlock_OnClosing(MyEntity block) => MyIGCSystemSessionComponent.Static.EvictContextFor((MyProgrammableBlock) block);

    long IMyIntergridCommunicationSystem.Me => this.GetAddressOfThisContext();

    IMyUnicastListener IMyIntergridCommunicationSystem.UnicastListener => (IMyUnicastListener) this.UnicastListener;

    bool IMyIntergridCommunicationSystem.IsEndpointReachable(
      long address,
      TransmissionDistance transmissionDistance)
    {
      MyIntergridCommunicationContext contextForPb = MyIntergridCommunicationContext.Context.GetContextForPB(address);
      if (contextForPb == null)
        return false;
      if (MyDebugDrawSettings.DEBUG_DRAW_IGC)
      {
        MatrixD worldMatrix = this.ProgrammableBlock.WorldMatrix;
        Vector3D from = worldMatrix.Translation;
        worldMatrix = MyEntities.GetEntityById(address).WorldMatrix;
        Vector3D to = worldMatrix.Translation;
        MyIGCSystemSessionComponent.Static.AddDebugDraw((Action) (() => MyRenderProxy.DebugDrawArrow3D(from, to, Color.Red)));
      }
      return this.IsConnectedTo(contextForPb, transmissionDistance);
    }

    void IMyIntergridCommunicationSystem.SendBroadcastMessage<TData>(
      string tag,
      TData data,
      TransmissionDistance transmissionDistance)
    {
      MyIntergridCommunicationContext.Context.EnqueueMessage(MyIGCSystemSessionComponent.Message.FromBroadcast(MyIGCSystemSessionComponent.BoxMessage<TData>(data), tag, transmissionDistance, this));
    }

    bool IMyIntergridCommunicationSystem.SendUnicastMessage<TData>(
      long addressee,
      string tag,
      TData data)
    {
      MyIntergridCommunicationContext contextForPb = MyIntergridCommunicationContext.Context.GetContextForPB(addressee);
      if (contextForPb == null || contextForPb == this || !this.IsConnectedTo(contextForPb, TransmissionDistance.AntennaRelay))
        return false;
      MyIntergridCommunicationContext.Context.EnqueueMessage(MyIGCSystemSessionComponent.Message.FromUnicast(MyIGCSystemSessionComponent.BoxMessage<TData>(data), tag, this, contextForPb));
      return true;
    }

    IMyBroadcastListener IMyIntergridCommunicationSystem.RegisterBroadcastListener(
      string tag)
    {
      if (this.m_broadcastListeners == null)
        this.m_broadcastListeners = new HashSet<BroadcastListener>();
      BroadcastListener listener = (BroadcastListener) null;
      foreach (BroadcastListener broadcastListener in this.m_broadcastListeners)
      {
        if (broadcastListener.Tag == tag)
        {
          listener = broadcastListener;
          break;
        }
      }
      if (listener == null)
      {
        listener = new BroadcastListener(this, tag);
        this.m_broadcastListeners.Add(listener);
      }
      if (!listener.IsActive)
      {
        listener.IsActive = true;
        MyIntergridCommunicationContext.Context.RegisterBroadcastListener(listener);
      }
      return (IMyBroadcastListener) listener;
    }

    void IMyIntergridCommunicationSystem.DisableBroadcastListener(
      IMyBroadcastListener broadcastListener)
    {
      if (!(broadcastListener is BroadcastListener broadcastListener1) || broadcastListener1.Context != this)
        throw new ArgumentException(nameof (broadcastListener));
      if (!this.m_broadcastListeners.Contains(broadcastListener1))
        return;
      this.DisposeBroadcastListener(broadcastListener1, true);
    }

    void IMyIntergridCommunicationSystem.GetBroadcastListeners(
      List<IMyBroadcastListener> broadcastListeners,
      Func<IMyBroadcastListener, bool> collect)
    {
      if (this.m_broadcastListeners == null)
        return;
      foreach (BroadcastListener broadcastListener in this.m_broadcastListeners)
      {
        if (collect == null || collect((IMyBroadcastListener) broadcastListener))
          broadcastListeners.Add((IMyBroadcastListener) broadcastListener);
      }
    }

    private class ConnectionData
    {
      public static int ValidDurationFrames = 1;
      public int ValidTill;
      public TransmissionDistance ConnectionType;
      public WeakReference<MyDataBroadcaster> Broadcaster;

      public void ReleaseBroadcaster()
      {
        if (this.Broadcaster == null)
          return;
        this.Broadcaster.SetTarget((MyDataBroadcaster) null);
      }
    }
  }
}
