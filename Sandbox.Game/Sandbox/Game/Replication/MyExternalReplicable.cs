// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyExternalReplicable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Replication;
using VRageMath;

namespace Sandbox.Game.Replication
{
  public abstract class MyExternalReplicable : IMyReplicable, IMyNetObject, IMyEventOwner
  {
    protected IMyStateGroup m_physicsSync;
    protected IMyReplicable m_parent;
    private static readonly MyConcurrentDictionary<object, MyExternalReplicable> m_objectExternalReplicables = new MyConcurrentDictionary<object, MyExternalReplicable>();

    public IMyStateGroup PhysicsSync => this.m_physicsSync;

    public virtual string InstanceName => "";

    public virtual int? PCU => new int?();

    public static event Action<MyExternalReplicable> Destroyed;

    public static MyExternalReplicable FindByObject(object obj) => MyExternalReplicable.m_objectExternalReplicables.GetValueOrDefault(obj, (MyExternalReplicable) null);

    public virtual void Hook(object obj) => MyExternalReplicable.m_objectExternalReplicables[obj] = this;

    public virtual void OnServerReplicate()
    {
    }

    public virtual bool IsReadyForReplication
    {
      get
      {
        if (this.GetInstance() == null)
          return false;
        if (!this.HasToBeChild)
          return true;
        return this.GetParent() != null && this.GetParent().IsReadyForReplication;
      }
    }

    public virtual Dictionary<IMyReplicable, Action> ReadyForReplicationAction => this.GetParent()?.ReadyForReplicationAction;

    public virtual bool PriorityUpdate => true;

    public virtual bool IncludeInIslands => true;

    public virtual bool ShouldReplicate(MyClientInfo client) => true;

    public abstract bool IsValid { get; }

    protected virtual void RaiseDestroyed()
    {
      this.ReadyForReplicationAction?.Remove((IMyReplicable) this);
      object instance = this.GetInstance();
      if (instance != null)
        MyExternalReplicable.m_objectExternalReplicables.Remove(instance);
      Action<MyExternalReplicable> destroyed = MyExternalReplicable.Destroyed;
      if (destroyed == null)
        return;
      destroyed(this);
    }

    protected abstract object GetInstance();

    protected abstract void OnHook();

    public abstract bool HasToBeChild { get; }

    public virtual bool IsSpatial => !this.HasToBeChild;

    public abstract IMyReplicable GetParent();

    public abstract bool OnSave(BitStream stream, Endpoint clientEndpoint);

    public abstract void OnLoad(BitStream stream, Action<bool> loadingDoneHandler);

    public abstract void Reload(Action<bool> loadingDoneHandler);

    public abstract void OnDestroyClient();

    public abstract void OnRemovedFromReplication();

    public abstract void GetStateGroups(List<IMyStateGroup> resultList);

    public abstract BoundingBoxD GetAABB();

    public Action<IMyReplicable> OnAABBChanged { get; set; }

    public virtual HashSet<IMyReplicable> GetDependencies(bool forPlayer) => (HashSet<IMyReplicable>) null;

    public virtual HashSet<IMyReplicable> GetPhysicalDependencies(
      MyTimeSpan timeStamp,
      MyReplicablesBase replicables)
    {
      return (HashSet<IMyReplicable>) null;
    }

    public virtual HashSet<IMyReplicable> GetCriticalDependencies() => (HashSet<IMyReplicable>) null;

    public abstract bool CheckConsistency();

    public virtual ValidationResult HasRights(
      EndpointId endpointId,
      ValidationType validationFlags)
    {
      return ValidationResult.Passed;
    }

    public virtual void OnReplication()
    {
    }

    public virtual void OnUnreplication()
    {
    }
  }
}
