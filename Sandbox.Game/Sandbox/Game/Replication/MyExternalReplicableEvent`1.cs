// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyExternalReplicableEvent`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Network;

namespace Sandbox.Game.Replication
{
  public abstract class MyExternalReplicableEvent<T> : MyExternalReplicable<T>, IMyProxyTarget, IMyNetObject, IMyEventOwner
    where T : IMyEventProxy
  {
    private IMyEventProxy m_proxy;

    IMyEventProxy IMyProxyTarget.Target => this.m_proxy;

    protected override void OnHook() => this.m_proxy = (IMyEventProxy) this.Instance;
  }
}
