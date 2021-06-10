// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyComponentReplicableBase`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Replication
{
  public abstract class MyComponentReplicableBase<T> : MyExternalReplicableEvent<T>
    where T : MyEntityComponentBase, IMyEventProxy
  {
    private readonly Action<MyEntity> m_raiseDestroyedHandler;

    protected MyComponentReplicableBase() => this.m_raiseDestroyedHandler = (Action<MyEntity>) (entity => this.RaiseDestroyed());

    protected override void OnHook()
    {
      base.OnHook();
      if ((object) this.Instance == null)
        return;
      ((MyEntity) this.Instance.Entity).OnClose += this.m_raiseDestroyedHandler;
      this.Instance.BeforeRemovedFromContainer += (Action<MyEntityComponentBase>) (component => this.OnComponentRemovedFromContainer());
    }

    private void OnComponentRemovedFromContainer()
    {
      if ((object) this.Instance == null || this.Instance.Entity == null)
        return;
      ((MyEntity) this.Instance.Entity).OnClose -= this.m_raiseDestroyedHandler;
      this.RaiseDestroyed();
    }

    public override bool HasToBeChild => true;

    protected override void OnLoad(BitStream stream, Action<T> loadingDoneHandler)
    {
      long entityId;
      MySerializer.CreateAndRead<long>(stream, out entityId);
      MyEntities.CallAsync((Action) (() => this.LoadAsync(entityId, loadingDoneHandler)));
    }

    private void LoadAsync(long entityId, Action<T> loadingDoneHandler)
    {
      Type componentType = MyComponentTypeFactory.GetComponentType(typeof (T));
      MyEntity entity = (MyEntity) null;
      MyComponentBase component = (MyComponentBase) null;
      if (MyEntities.TryGetEntityById(entityId, out entity))
        entity.Components.TryGet(componentType, out component);
      loadingDoneHandler(component as T);
    }

    public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
    {
      long entityId = this.Instance.Entity.EntityId;
      MySerializer.Write<long>(stream, ref entityId);
      return true;
    }

    public override void OnDestroyClient()
    {
      if ((object) this.Instance == null || this.Instance.Entity == null)
        return;
      ((MyEntity) this.Instance.Entity).OnClose -= this.m_raiseDestroyedHandler;
    }

    public override void GetStateGroups(List<IMyStateGroup> resultList)
    {
    }

    public override BoundingBoxD GetAABB() => BoundingBoxD.CreateInvalid();
  }
}
