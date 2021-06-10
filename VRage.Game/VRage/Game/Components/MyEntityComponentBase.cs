// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyEntityComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.ModAPI;
using VRage.Network;

namespace VRage.Game.Components
{
  [GenerateActivator]
  public abstract class MyEntityComponentBase : MyComponentBase
  {
    public MyEntityComponentContainer Container => this.ContainerBase as MyEntityComponentContainer;

    public IMyEntity Entity => !(this.ContainerBase is MyEntityComponentContainer containerBase) ? (IMyEntity) null : containerBase.Entity;

    public abstract string ComponentTypeDebugString { get; }

    public virtual bool AttachSyncToEntity => true;

    public static event Action<MyEntityComponentBase> OnAfterAddedToContainer;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      Action<MyEntityComponentBase> addedToContainer = MyEntityComponentBase.OnAfterAddedToContainer;
      if (addedToContainer != null)
        addedToContainer(this);
      if (this.Entity == null || !(this.Entity is IMySyncedEntity entity) || (entity.SyncType == null || !this.AttachSyncToEntity))
        return;
      entity.SyncType.Append((object) this);
    }

    public event Action<MyEntityComponentBase> BeforeRemovedFromContainer;

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      Action<MyEntityComponentBase> removedFromContainer = this.BeforeRemovedFromContainer;
      if (removedFromContainer == null)
        return;
      removedFromContainer(this);
    }
  }
}
