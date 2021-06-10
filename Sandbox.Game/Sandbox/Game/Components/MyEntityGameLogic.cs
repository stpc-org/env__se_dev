// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyEntityGameLogic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Text;
using VRage;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.EntityComponents.Interfaces;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Components
{
  public class MyEntityGameLogic : MyGameLogicComponent
  {
    protected MyEntity m_entity;

    public event Action<MyEntity> OnMarkForClose;

    public event Action<MyEntity> OnClose;

    public event Action<MyEntity> OnClosing;

    public MyGameLogicComponent GameLogic { get; set; }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_entity = this.Container.Entity as MyEntity;
    }

    public MyEntityGameLogic() => this.GameLogic = (MyGameLogicComponent) new MyNullGameLogicComponent();

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      if (objectBuilder != null)
      {
        if (objectBuilder.PositionAndOrientation.HasValue)
        {
          MyPositionAndOrientation positionAndOrientation = objectBuilder.PositionAndOrientation.Value;
          MatrixD world = MatrixD.CreateWorld((Vector3D) positionAndOrientation.Position, (Vector3) positionAndOrientation.Forward, (Vector3) positionAndOrientation.Up);
          this.Container.Entity.PositionComp.SetWorldMatrix(ref world);
        }
        if (objectBuilder.EntityId != 0L)
          this.Container.Entity.EntityId = objectBuilder.EntityId;
        this.Container.Entity.Name = objectBuilder.Name;
        this.Container.Entity.Render.PersistentFlags = objectBuilder.PersistentFlags;
      }
      this.AllocateEntityID();
      this.Container.Entity.InScene = false;
      MyEntities.SetEntityName(this.m_entity, false);
      if (this.m_entity.SyncFlag)
        this.m_entity.CreateSync();
      this.GameLogic.Init(objectBuilder);
    }

    public void Init(
      StringBuilder displayName,
      string model,
      MyEntity parentObject,
      float? scale,
      string modelCollision = null)
    {
      this.Container.Entity.DisplayName = displayName?.ToString();
      this.m_entity.RefreshModels(model, modelCollision);
      parentObject?.Hierarchy.AddChild(this.Container.Entity, insertIntoSceneIfNeeded: false);
      this.Container.Entity.PositionComp.Scale = scale;
      this.AllocateEntityID();
    }

    private void AllocateEntityID()
    {
      if (this.Container.Entity.EntityId != 0L || MyEntityIdentifier.AllocationSuspended)
        return;
      this.Container.Entity.EntityId = MyEntityIdentifier.AllocateId();
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_EntityBase objectBuilder = MyEntityFactory.CreateObjectBuilder(this.Container.Entity as MyEntity);
      objectBuilder.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation()
      {
        Position = (SerializableVector3D) this.Container.Entity.PositionComp.GetPosition(),
        Up = (SerializableVector3) (Vector3) this.Container.Entity.WorldMatrix.Up,
        Forward = (SerializableVector3) (Vector3) this.Container.Entity.WorldMatrix.Forward
      });
      objectBuilder.EntityId = this.Container.Entity.EntityId;
      objectBuilder.Name = this.Container.Entity.Name;
      objectBuilder.PersistentFlags = this.Container.Entity.Render.PersistentFlags;
      return objectBuilder;
    }

    public override void UpdateOnceBeforeFrame() => this.GameLogic.UpdateOnceBeforeFrame();

    public override void UpdateBeforeSimulation() => this.GameLogic.UpdateBeforeSimulation();

    public override void UpdateAfterSimulation() => this.GameLogic.UpdateAfterSimulation();

    public override void UpdatingStopped()
    {
    }

    public override void UpdateBeforeSimulation10() => this.GameLogic.UpdateBeforeSimulation10();

    public override void UpdateAfterSimulation10() => this.GameLogic.UpdateAfterSimulation10();

    public override void UpdateBeforeSimulation100() => this.GameLogic.UpdateBeforeSimulation100();

    public override void UpdateAfterSimulation100() => this.GameLogic.UpdateAfterSimulation100();

    public override void MarkForClose()
    {
      this.MarkedForClose = true;
      MyEntities.Close(this.m_entity);
      this.GameLogic.MarkForClose();
      Action<MyEntity> onMarkForClose = this.OnMarkForClose;
      if (onMarkForClose == null)
        return;
      onMarkForClose(this.m_entity);
    }

    public override void Close()
    {
      ((IMyGameLogicComponent) this.GameLogic).Close();
      MyHierarchyComponent<MyEntity> hierarchy = this.m_entity.Hierarchy;
      while (hierarchy != null && hierarchy.Children.Count > 0)
      {
        MyHierarchyComponentBase child = hierarchy.Children[hierarchy.Children.Count - 1];
        child.Container.Entity.Close();
        hierarchy.RemoveByJN(child);
      }
      this.CallAndClearOnClosing();
      MyEntities.RemoveName(this.m_entity);
      MyEntities.RemoveFromClosedEntities(this.m_entity);
      if (this.m_entity.Physics != null)
      {
        this.m_entity.Physics.Close();
        this.m_entity.Physics = (MyPhysicsComponentBase) null;
        this.m_entity.RaisePhysicsChanged();
      }
      MyEntities.UnregisterForUpdate(this.m_entity, true);
      MyEntities.UnregisterForDraw((IMyEntity) this.m_entity);
      if (hierarchy == null || hierarchy.Parent == null)
      {
        MyEntities.Remove(this.m_entity);
      }
      else
      {
        this.m_entity.Parent.Hierarchy.RemoveByJN((MyHierarchyComponentBase) hierarchy);
        if (this.m_entity.Parent.InScene)
          this.m_entity.OnRemovedFromScene((object) this.m_entity);
        MyEntities.RaiseEntityRemove(this.m_entity);
      }
      if (this.m_entity.EntityId != 0L)
        MyEntityIdentifier.RemoveEntity(this.m_entity.EntityId);
      this.CallAndClearOnClose();
      this.Closed = true;
    }

    protected void CallAndClearOnClose()
    {
      if (this.OnClose != null)
        this.OnClose(this.m_entity);
      this.OnClose = (Action<MyEntity>) null;
    }

    protected void CallAndClearOnClosing()
    {
      if (this.OnClosing != null)
        this.OnClosing(this.m_entity);
      this.OnClosing = (Action<MyEntity>) null;
    }

    private class Sandbox_Game_Components_MyEntityGameLogic\u003C\u003EActor : IActivator, IActivator<MyEntityGameLogic>
    {
      object IActivator.CreateInstance() => (object) new MyEntityGameLogic();

      MyEntityGameLogic IActivator<MyEntityGameLogic>.CreateInstance() => new MyEntityGameLogic();
    }
  }
}
