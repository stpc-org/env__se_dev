// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyExternalReplicable`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;

namespace Sandbox.Game.Replication
{
  public abstract class MyExternalReplicable<T> : MyExternalReplicable
  {
    public T Instance { get; private set; }

    public override string InstanceName => (object) this.Instance == null ? "" : this.Instance.ToString();

    public override bool IsReadyForReplication
    {
      get
      {
        MyEntity instance1 = (object) this.Instance as MyEntity;
        MyEntityComponentBase instance2 = (object) this.Instance as MyEntityComponentBase;
        if (instance1 != null)
          return instance1.IsReadyForReplication;
        return instance2 != null ? ((MyEntity) instance2.Entity).IsReadyForReplication : base.IsReadyForReplication;
      }
    }

    public override Dictionary<IMyReplicable, Action> ReadyForReplicationAction
    {
      get
      {
        MyEntity instance1 = (object) this.Instance as MyEntity;
        MyEntityComponentBase instance2 = (object) this.Instance as MyEntityComponentBase;
        if (instance1 != null)
          return instance1.ReadyForReplicationAction;
        if (instance2 == null)
          return base.ReadyForReplicationAction;
        return (MyEntity) instance2.Entity != null ? ((MyEntity) instance2.Entity).ReadyForReplicationAction : base.ReadyForReplicationAction;
      }
    }

    protected abstract void OnLoad(BitStream stream, Action<T> loadingDoneHandler);

    protected override sealed object GetInstance() => (object) this.Instance;

    protected override void RaiseDestroyed() => base.RaiseDestroyed();

    protected void OnLoadDone(T instance, Action<bool> loadingDoneHandler)
    {
      if ((object) instance != null)
      {
        this.HookInternal((object) instance);
        loadingDoneHandler(true);
      }
      else
        loadingDoneHandler(false);
    }

    public override sealed void OnLoad(BitStream stream, Action<bool> loadingDoneHandler) => this.OnLoad(stream, (Action<T>) (instance => this.OnLoadDone(instance, loadingDoneHandler)));

    public override sealed void Reload(Action<bool> loadingDoneHandler) => this.OnLoad((BitStream) null, (Action<T>) (instance => this.OnLoadDone(instance, loadingDoneHandler)));

    public override sealed void OnRemovedFromReplication() => this.Instance = default (T);

    public override sealed void Hook(object obj) => this.HookInternal(obj);

    private void HookInternal(object obj)
    {
      this.Instance = (T) obj;
      base.Hook(obj);
      this.OnHook();
    }

    public override sealed bool CheckConsistency() => (object) this.Instance != null;
  }
}
