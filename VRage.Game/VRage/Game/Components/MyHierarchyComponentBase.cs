// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyHierarchyComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace VRage.Game.Components
{
  [MyComponentBuilder(typeof (MyObjectBuilder_HierarchyComponentBase), true)]
  public class MyHierarchyComponentBase : MyEntityComponentBase
  {
    private readonly List<MyHierarchyComponentBase> m_children = new List<MyHierarchyComponentBase>();
    private readonly List<MyHierarchyComponentBase> m_childrenNeedingWorldMatrix = new List<MyHierarchyComponentBase>();
    private readonly List<(MyObjectBuilder_EntityBase, MyEntity)> m_deserializedEntities = new List<(MyObjectBuilder_EntityBase, MyEntity)>();
    public long ChildId;
    private MyEntityComponentContainer m_parentContainer;
    private MyHierarchyComponentBase m_parent;

    public event Action<IMyEntity> OnChildRemoved;

    public event Action<MyHierarchyComponentBase, MyHierarchyComponentBase> OnParentChanged;

    public MyHierarchyComponentBase GetTopMostParent(Type type = null)
    {
      MyHierarchyComponentBase hierarchyComponentBase = this;
      while (hierarchyComponentBase.Parent != null && (type == (Type) null || !hierarchyComponentBase.Container.Contains(type)))
        hierarchyComponentBase = hierarchyComponentBase.Parent;
      return hierarchyComponentBase;
    }

    public ListReader<MyHierarchyComponentBase> Children => (ListReader<MyHierarchyComponentBase>) this.m_children;

    public ListReader<MyHierarchyComponentBase> ChildrenNeedingWorldMatrix => (ListReader<MyHierarchyComponentBase>) this.m_childrenNeedingWorldMatrix;

    public MyHierarchyComponentBase Parent
    {
      get => this.m_parent;
      set
      {
        MyHierarchyComponentBase parent = this.m_parent;
        if (this.m_parentContainer != null)
        {
          this.m_parentContainer.ComponentAdded -= new Action<Type, MyEntityComponentBase>(this.Container_ComponentAdded);
          this.m_parentContainer.ComponentRemoved -= new Action<Type, MyEntityComponentBase>(this.Container_ComponentRemoved);
          this.m_parentContainer = (MyEntityComponentContainer) null;
        }
        this.m_parent = value;
        if (this.m_parent != null)
        {
          this.m_parentContainer = this.m_parent.Container;
          this.m_parentContainer.ComponentAdded += new Action<Type, MyEntityComponentBase>(this.Container_ComponentAdded);
          this.m_parentContainer.ComponentRemoved += new Action<Type, MyEntityComponentBase>(this.Container_ComponentRemoved);
        }
        this.OnParentChanged.InvokeIfNotNull<MyHierarchyComponentBase, MyHierarchyComponentBase>(parent, this.m_parent);
      }
    }

    private void Container_ComponentRemoved(Type arg1, MyEntityComponentBase arg2)
    {
      if (arg2 != this.m_parent)
        return;
      this.m_parent = (MyHierarchyComponentBase) null;
    }

    private void Container_ComponentAdded(Type arg1, MyEntityComponentBase arg2)
    {
      if (!typeof (MyHierarchyComponentBase).IsAssignableFrom(arg1))
        return;
      this.m_parent = arg2 as MyHierarchyComponentBase;
    }

    public void AddChild(IMyEntity child, bool preserveWorldPos = false, bool insertIntoSceneIfNeeded = true)
    {
      MyHierarchyComponentBase hierarchyComponentBase = child.Components.Get<MyHierarchyComponentBase>();
      if (this.m_children.Contains(hierarchyComponentBase))
        return;
      MatrixD worldMatrix = child.WorldMatrix;
      hierarchyComponentBase.Parent = this;
      this.m_children.Add(hierarchyComponentBase);
      if (child.NeedsWorldMatrix)
        this.m_childrenNeedingWorldMatrix.Add(hierarchyComponentBase);
      if (preserveWorldPos)
      {
        child.PositionComp.SetWorldMatrix(ref worldMatrix, (object) this.Entity, true, ignoreAssert: true);
      }
      else
      {
        MyPositionComponentBase positionComponentBase1 = this.Container.Get<MyPositionComponentBase>();
        MyPositionComponentBase positionComponentBase2 = child.Components.Get<MyPositionComponentBase>();
        MatrixD matrixD = positionComponentBase1.WorldMatrixRef;
        ref MatrixD local = ref matrixD;
        positionComponentBase2.UpdateWorldMatrix(ref local);
      }
      if (((!this.Container.Entity.InScene ? 0 : (!child.InScene ? 1 : 0)) & (insertIntoSceneIfNeeded ? 1 : 0)) == 0)
        return;
      child.OnAddedToScene((object) this.Container.Entity);
    }

    internal void UpdateNeedsWorldMatrix()
    {
      if (this.Entity.Parent == null)
        return;
      if (this.Entity.NeedsWorldMatrix && this.Entity.Parent.Hierarchy.m_children.Contains(this))
      {
        if (this.Entity.Parent.Hierarchy.m_childrenNeedingWorldMatrix.Contains(this))
          return;
        this.Entity.Parent.Hierarchy.m_childrenNeedingWorldMatrix.Add(this);
      }
      else
        this.Entity.Parent.Hierarchy.m_childrenNeedingWorldMatrix.Remove(this);
    }

    public void AddChildWithMatrix(
      IMyEntity child,
      ref Matrix childLocalMatrix,
      bool insertIntoSceneIfNeeded = true)
    {
      MyHierarchyComponentBase hierarchyComponentBase = child.Components.Get<MyHierarchyComponentBase>();
      hierarchyComponentBase.Parent = this;
      this.m_children.Add(hierarchyComponentBase);
      child.PositionComp.SetLocalMatrix(ref childLocalMatrix, (object) this.Entity);
      if (child.NeedsWorldMatrix)
        this.m_childrenNeedingWorldMatrix.Add(hierarchyComponentBase);
      if (((!this.Container.Entity.InScene ? 0 : (!child.InScene ? 1 : 0)) & (insertIntoSceneIfNeeded ? 1 : 0)) == 0)
        return;
      child.OnAddedToScene((object) this);
    }

    public void RemoveChild(IMyEntity child, bool preserveWorldPos = false)
    {
      MyHierarchyComponentBase hierarchyComponentBase = child.Components.Get<MyHierarchyComponentBase>();
      MatrixD matrixD = new MatrixD();
      if (preserveWorldPos)
        matrixD = child.WorldMatrix;
      if (child.InScene)
        child.OnRemovedFromScene((object) this);
      this.m_children.Remove(hierarchyComponentBase);
      this.m_childrenNeedingWorldMatrix.Remove(hierarchyComponentBase);
      if (preserveWorldPos)
        child.WorldMatrix = matrixD;
      hierarchyComponentBase.Parent = (MyHierarchyComponentBase) null;
      this.OnChildRemoved.InvokeIfNotNull<IMyEntity>(child);
    }

    public void GetChildrenRecursive(HashSet<IMyEntity> result)
    {
      for (int index = 0; index < this.Children.Count; ++index)
      {
        MyHierarchyComponentBase child = this.Children[index];
        result.Add(child.Container.Entity);
        child.GetChildrenRecursive(result);
      }
    }

    public void RemoveByJN(MyHierarchyComponentBase childHierarchy)
    {
      this.m_children.Remove(childHierarchy);
      this.m_childrenNeedingWorldMatrix.Remove(childHierarchy);
    }

    public void Delete()
    {
      for (int index = this.m_children.Count - 1; index >= 0; --index)
        this.m_children[index].Container.Entity.Delete();
    }

    public override string ComponentTypeDebugString => "Hierarchy";

    public override void OnBeforeRemovedFromContainer()
    {
      if (this.m_parentContainer != null && !this.m_parentContainer.Entity.MarkedForClose)
      {
        this.m_parentContainer.ComponentAdded -= new Action<Type, MyEntityComponentBase>(this.Container_ComponentAdded);
        this.m_parentContainer.ComponentRemoved -= new Action<Type, MyEntityComponentBase>(this.Container_ComponentRemoved);
      }
      this.m_parent = (MyHierarchyComponentBase) null;
      this.m_parentContainer = (MyEntityComponentContainer) null;
      base.OnBeforeRemovedFromContainer();
    }

    public override bool IsSerialized() => true;

    public override void OnAddedToScene() => base.OnAddedToScene();

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      if (this.Children.Count == 0)
        return (MyObjectBuilder_ComponentBase) null;
      MyObjectBuilder_HierarchyComponentBase hierarchyComponentBase = new MyObjectBuilder_HierarchyComponentBase();
      foreach (MyHierarchyComponentBase child in this.Children)
      {
        if (child.Entity.Save)
        {
          MyObjectBuilder_EntityBase objectBuilder = child.Entity.GetObjectBuilder(copy);
          objectBuilder.LocalPositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation((MatrixD) ref child.Entity.PositionComp.LocalMatrixRef));
          hierarchyComponentBase.Children.Add(objectBuilder);
        }
      }
      return hierarchyComponentBase.Children.Count <= 0 ? (MyObjectBuilder_ComponentBase) null : (MyObjectBuilder_ComponentBase) hierarchyComponentBase;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      base.Deserialize(builder);
      if (!(builder is MyObjectBuilder_HierarchyComponentBase hierarchyComponentBase))
        return;
      foreach (MyObjectBuilder_EntityBase child in hierarchyComponentBase.Children)
      {
        if (!MyEntityIdentifier.ExistsById(child.EntityId))
        {
          MyEntity myEntity = MyEntity.MyEntitiesCreateFromObjectBuilderExtCallback(child, true);
          if (myEntity != null)
            this.m_deserializedEntities.Add((child, myEntity));
        }
      }
      foreach ((MyObjectBuilder_EntityBase builderEntityBase, MyEntity myEntity1) in this.m_deserializedEntities)
      {
        if (builderEntityBase.LocalPositionAndOrientation.HasValue)
        {
          MatrixD local_9 = builderEntityBase.LocalPositionAndOrientation.Value.GetMatrix();
          Matrix local_7 = (Matrix) ref local_9;
          this.AddChildWithMatrix((IMyEntity) myEntity1, ref local_7, false);
        }
        else
          this.AddChild((IMyEntity) myEntity1, true, false);
      }
      this.m_deserializedEntities.Clear();
    }

    private class VRage_Game_Components_MyHierarchyComponentBase\u003C\u003EActor : IActivator, IActivator<MyHierarchyComponentBase>
    {
      object IActivator.CreateInstance() => (object) new MyHierarchyComponentBase();

      MyHierarchyComponentBase IActivator<MyHierarchyComponentBase>.CreateInstance() => new MyHierarchyComponentBase();
    }
  }
}
