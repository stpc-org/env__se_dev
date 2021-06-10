// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyEntityReplicableBaseEvent`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Diagnostics;
using VRage.Game.Entity;
using VRage.Network;

namespace Sandbox.Game.Replication
{
  public abstract class MyEntityReplicableBaseEvent<T> : MyEntityReplicableBase<T>, IMyProxyTarget, IMyNetObject, IMyEventOwner
    where T : MyEntity, IMyEventProxy
  {
    private IMyEventProxy m_proxy;

    IMyEventProxy IMyProxyTarget.Target => this.m_proxy;

    protected override void OnHook()
    {
      base.OnHook();
      this.m_proxy = (IMyEventProxy) this.Instance;
    }

    [Conditional("DEBUG")]
    private void RegisterAsserts()
    {
      if (Sync.IsServer)
        return;
      this.Instance.OnMarkForClose += new Action<MyEntity>(this.OnMarkForCloseOnClient);
      this.Instance.OnClose += new Action<MyEntity>(this.OnMarkForCloseOnClient);
    }

    private void OnMarkForCloseOnClient(MyEntity entity)
    {
      if (MyMultiplayer.Static == null)
        return;
      IMyProxyTarget proxyTarget = MyMultiplayer.Static.ReplicationLayer.GetProxyTarget(this.m_proxy);
      if (!MySession.Static.Ready || proxyTarget == null)
        return;
      MyMultiplayer.Static.ReplicationLayer.TryGetNetworkIdByObject((IMyNetObject) proxyTarget, out NetworkId _);
    }
  }
}
