// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyPositionComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace VRage.Game.Components
{
  public class MyPositionComponent : MyPositionComponentBase
  {
    public Action<object> WorldPositionChanged;
    private MySyncComponentBase m_syncObject;
    private MyPhysicsComponentBase m_physics;
    private MyHierarchyComponentBase m_hierarchy;
    public static bool SynchronizationEnabled = true;

    public override BoundingBox LocalAABB
    {
      get => this.m_localAABB;
      set
      {
        base.LocalAABB = value;
        this.Container.Entity.UpdateGamePruningStructure();
      }
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_syncObject = this.Container.Get<MySyncComponentBase>();
      this.m_physics = this.Container.Get<MyPhysicsComponentBase>();
      this.m_hierarchy = this.Container.Get<MyHierarchyComponentBase>();
      this.Container.ComponentAdded += new Action<Type, MyEntityComponentBase>(this.container_ComponentAdded);
      this.Container.ComponentRemoved += new Action<Type, MyEntityComponentBase>(this.container_ComponentRemoved);
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      this.Container.ComponentAdded -= new Action<Type, MyEntityComponentBase>(this.container_ComponentAdded);
      this.Container.ComponentRemoved -= new Action<Type, MyEntityComponentBase>(this.container_ComponentRemoved);
    }

    private void container_ComponentAdded(Type type, MyEntityComponentBase comp)
    {
      if (type == typeof (MySyncComponentBase))
        this.m_syncObject = comp as MySyncComponentBase;
      else if (type == typeof (MyPhysicsComponentBase))
      {
        this.m_physics = comp as MyPhysicsComponentBase;
      }
      else
      {
        if (!(type == typeof (MyHierarchyComponentBase)))
          return;
        this.m_hierarchy = comp as MyHierarchyComponentBase;
      }
    }

    private void container_ComponentRemoved(Type type, MyEntityComponentBase comp)
    {
      if (type == typeof (MySyncComponentBase))
        this.m_syncObject = (MySyncComponentBase) null;
      else if (type == typeof (MyPhysicsComponentBase))
      {
        this.m_physics = (MyPhysicsComponentBase) null;
      }
      else
      {
        if (!(type == typeof (MyHierarchyComponentBase)))
          return;
        this.m_hierarchy = (MyHierarchyComponentBase) null;
      }
    }

    protected override bool ShouldSync => MyPositionComponent.SynchronizationEnabled && this.Container.Get<MySyncComponentBase>() != null && this.m_syncObject != null;

    protected virtual void UpdateChildren(object source, bool forceUpdateAllChildren)
    {
      MatrixD parentWorldMatrix = this.WorldMatrixRef;
      ListReader<MyHierarchyComponentBase> listReader = forceUpdateAllChildren ? this.m_hierarchy.Children : this.m_hierarchy.ChildrenNeedingWorldMatrix;
      for (int index = 0; index < listReader.Count; ++index)
        listReader[index].Entity.PositionComp.UpdateWorldMatrix(ref parentWorldMatrix, source, forceUpdateAllChildren: forceUpdateAllChildren);
    }

    protected override void OnWorldPositionChanged(
      object source,
      bool updateChildren,
      bool forceUpdateAllChildren)
    {
      MyEntity entity = (MyEntity) this.Container.Entity;
      entity.UpdateGamePruningStructure();
      if (updateChildren && this.m_hierarchy != null && this.m_hierarchy.Children.Count > 0)
        this.UpdateChildren(source, forceUpdateAllChildren);
      this.m_worldVolumeDirty = true;
      this.m_worldAABBDirty = true;
      this.m_normalizedInvMatrixDirty = true;
      this.m_invScaledMatrixDirty = true;
      if (this.m_physics != null && this.m_physics != source && this.m_physics.Enabled)
        this.m_physics.OnWorldPositionChanged(source);
      this.RaiseOnPositionChanged((MyPositionComponentBase) this);
      this.WorldPositionChanged.InvokeIfNotNull<object>(source);
      if (entity.Render == null || (entity.Flags & EntityFlags.InvalidateOnMove) == (EntityFlags) 0)
        return;
      entity.Render.InvalidateRenderObjects();
    }

    private class VRage_Game_Components_MyPositionComponent\u003C\u003EActor : IActivator, IActivator<MyPositionComponent>
    {
      object IActivator.CreateInstance() => (object) new MyPositionComponent();

      MyPositionComponent IActivator<MyPositionComponent>.CreateInstance() => new MyPositionComponent();
    }
  }
}
