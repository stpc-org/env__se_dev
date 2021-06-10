// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyTriggerComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  [MyComponentBuilder(typeof (MyObjectBuilder_TriggerBase), true)]
  public class MyTriggerComponent : MyEntityComponentBase
  {
    private static uint m_triggerCounter;
    private const uint PRIME = 31;
    private readonly uint m_updateOffset;
    protected readonly List<MyEntity> m_queryResult = new List<MyEntity>();
    protected MyTriggerComponent.TriggerType m_triggerType;
    protected BoundingBoxD m_AABB;
    protected BoundingSphereD m_boundingSphere;
    protected MyOrientedBoundingBoxD m_orientedBoundingBox;
    protected Vector3 m_relativeOffset;
    private bool m_updatedOnce;
    private bool m_registered;

    protected bool DoQuery { get; set; }

    protected List<MyEntity> QueryResult => this.m_queryResult;

    public uint UpdateFrequency { get; set; }

    public virtual bool Enabled { get; protected set; }

    public override string ComponentTypeDebugString => "Trigger";

    public Color? CustomDebugColor { get; set; }

    public Vector3D Center
    {
      get
      {
        switch (this.m_triggerType)
        {
          case MyTriggerComponent.TriggerType.AABB:
            return this.m_AABB.Center;
          case MyTriggerComponent.TriggerType.Sphere:
            return this.m_boundingSphere.Center;
          case MyTriggerComponent.TriggerType.OBB:
            return this.m_orientedBoundingBox.Center;
          default:
            return Vector3D.Zero;
        }
      }
      set
      {
        this.m_AABB.Centerize(value);
        this.m_boundingSphere.Center = value;
        this.m_orientedBoundingBox.Center = value;
        this.CalculateRelativeOffset();
      }
    }

    public MyOrientedBoundingBoxD OBB
    {
      get => this.m_orientedBoundingBox;
      set => this.m_orientedBoundingBox = value;
    }

    public MyTriggerComponent.TriggerType TriggerAreaType
    {
      get => this.m_triggerType;
      set => this.m_triggerType = value;
    }

    public MyTriggerComponent(MyTriggerComponent.TriggerType type, uint updateFrequency = 300)
    {
      this.m_triggerType = type;
      this.UpdateFrequency = updateFrequency;
      this.m_updateOffset = MyTriggerComponent.m_triggerCounter++ * 31U % this.UpdateFrequency;
      this.DoQuery = true;
    }

    public MyTriggerComponent()
    {
      this.m_triggerType = MyTriggerComponent.TriggerType.AABB;
      this.UpdateFrequency = 300U;
      this.m_updateOffset = MyTriggerComponent.m_triggerCounter++ * 31U % this.UpdateFrequency;
      this.DoQuery = true;
    }

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      if (base.Serialize() is MyObjectBuilder_TriggerBase builderTriggerBase)
      {
        builderTriggerBase.AABB = (SerializableBoundingBoxD) this.m_AABB;
        builderTriggerBase.BoundingSphere = (SerializableBoundingSphereD) this.m_boundingSphere;
        builderTriggerBase.Type = (int) this.m_triggerType;
        builderTriggerBase.OrientedBoundingBox = (SerializableOrientedBoundingBoxD) this.m_orientedBoundingBox;
      }
      return (MyObjectBuilder_ComponentBase) builderTriggerBase;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      base.Deserialize(builder);
      if (!(builder is MyObjectBuilder_TriggerBase builderTriggerBase))
        return;
      this.m_AABB = (BoundingBoxD) builderTriggerBase.AABB;
      this.m_boundingSphere = (BoundingSphereD) builderTriggerBase.BoundingSphere;
      this.m_orientedBoundingBox = builderTriggerBase.OrientedBoundingBox.HalfExtent.IsZero ? (this.m_boundingSphere.Radius <= 0.0 ? new MyOrientedBoundingBoxD(this.m_AABB.Center, this.m_AABB.Size / 2.0, Quaternion.Identity) : new MyOrientedBoundingBoxD(this.m_boundingSphere.Center, new Vector3D(this.m_boundingSphere.Radius), Quaternion.Identity)) : (MyOrientedBoundingBoxD) builderTriggerBase.OrientedBoundingBox;
      this.m_triggerType = builderTriggerBase.Type == -1 ? MyTriggerComponent.TriggerType.AABB : (MyTriggerComponent.TriggerType) builderTriggerBase.Type;
    }

    private void CalculateRelativeOffset()
    {
      Vector3D vector3D = Vector3D.Zero;
      switch (this.m_triggerType)
      {
        case MyTriggerComponent.TriggerType.AABB:
          vector3D = this.m_AABB.Center;
          break;
        case MyTriggerComponent.TriggerType.Sphere:
          vector3D = this.m_boundingSphere.Center;
          break;
        case MyTriggerComponent.TriggerType.OBB:
          vector3D = this.m_orientedBoundingBox.Center;
          break;
      }
      this.m_relativeOffset = (Vector3) Vector3D.TransformNormal(vector3D - this.Entity.PositionComp.WorldMatrixRef.Translation, this.Entity.PositionComp.WorldMatrixNormalizedInv);
    }

    private void RegisterComponent()
    {
      if (this.m_registered)
        return;
      this.m_registered = true;
      IMyEntity topMostParent = this.Entity.GetTopMostParent();
      topMostParent.PositionComp.OnPositionChanged += new Action<MyPositionComponentBase>(this.OnEntityPositionCompPositionChanged);
      topMostParent.NeedsWorldMatrix = true;
      MySessionComponentTriggerSystem.Static.AddTrigger(this);
      this.CalculateRelativeOffset();
    }

    private void UnRegisterComponent()
    {
      if (!this.m_registered)
        return;
      this.m_registered = false;
      MySessionComponentTriggerSystem.RemoveTrigger((MyEntity) this.Entity, this);
      this.Entity.GetTopMostParent().PositionComp.OnPositionChanged -= new Action<MyPositionComponentBase>(this.OnEntityPositionCompPositionChanged);
      this.Dispose();
    }

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      this.RegisterComponent();
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      if (!this.Entity.InScene)
        return;
      this.RegisterComponent();
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      this.UnRegisterComponent();
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
      this.UnRegisterComponent();
    }

    private void OnEntityPositionCompPositionChanged(MyPositionComponentBase myPositionComponentBase)
    {
      if (this.Entity == null || this.Entity.PositionComp == null)
        return;
      Vector3 vector3 = Vector3.TransformNormal(this.m_relativeOffset, this.Entity.PositionComp.WorldMatrixRef);
      switch (this.m_triggerType)
      {
        case MyTriggerComponent.TriggerType.AABB:
          this.m_AABB.Centerize(this.Entity.PositionComp.GetPosition() + vector3);
          break;
        case MyTriggerComponent.TriggerType.Sphere:
          this.m_boundingSphere.Center = this.Entity.PositionComp.GetPosition() + vector3;
          break;
        case MyTriggerComponent.TriggerType.OBB:
          MatrixD translation = MatrixD.CreateTranslation(this.m_relativeOffset);
          translation.Right *= this.m_orientedBoundingBox.HalfExtent.X * 2.0;
          translation.Up *= this.m_orientedBoundingBox.HalfExtent.Y * 2.0;
          translation.Forward *= this.m_orientedBoundingBox.HalfExtent.Z * 2.0;
          this.m_orientedBoundingBox = new MyOrientedBoundingBoxD(translation * this.Entity.PositionComp.WorldMatrixRef);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public void Update()
    {
      if ((long) MySession.Static.GameplayFrameCounter % (long) this.UpdateFrequency != (long) this.m_updateOffset && this.m_updatedOnce)
        return;
      this.m_updatedOnce = true;
      this.UpdateInternal();
    }

    protected virtual void UpdateInternal()
    {
      if (!this.DoQuery)
        return;
      this.m_queryResult.Clear();
      switch (this.m_triggerType)
      {
        case MyTriggerComponent.TriggerType.AABB:
          MyGamePruningStructure.GetTopMostEntitiesInBox(ref this.m_AABB, this.m_queryResult);
          break;
        case MyTriggerComponent.TriggerType.Sphere:
          MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref this.m_boundingSphere, this.m_queryResult);
          break;
        case MyTriggerComponent.TriggerType.OBB:
          MyGamePruningStructure.GetAllEntitiesInOBB(ref this.m_orientedBoundingBox, this.m_queryResult);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      int index = 0;
      while (index < this.m_queryResult.Count)
      {
        if (!this.QueryEvaluator(this.m_queryResult[index]))
        {
          this.m_queryResult.RemoveAtFast<MyEntity>(index);
        }
        else
        {
          MyOrientedBoundingBoxD fromBoundingBox = MyOrientedBoundingBoxD.CreateFromBoundingBox((BoundingBoxD) this.m_queryResult[index].PositionComp.LocalAABB);
          fromBoundingBox.Transform(this.m_queryResult[index].PositionComp.WorldMatrixRef);
          switch (this.m_triggerType)
          {
            case MyTriggerComponent.TriggerType.AABB:
              if (!fromBoundingBox.Intersects(ref this.m_AABB))
              {
                this.m_queryResult.RemoveAtFast<MyEntity>(index);
                continue;
              }
              ++index;
              continue;
            case MyTriggerComponent.TriggerType.Sphere:
              if (!fromBoundingBox.Intersects(ref this.m_boundingSphere))
              {
                this.m_queryResult.RemoveAtFast<MyEntity>(index);
                continue;
              }
              ++index;
              continue;
            case MyTriggerComponent.TriggerType.OBB:
              if (!fromBoundingBox.Intersects(ref this.m_orientedBoundingBox))
              {
                this.m_queryResult.RemoveAtFast<MyEntity>(index);
                continue;
              }
              ++index;
              continue;
            default:
              ++index;
              continue;
          }
        }
      }
    }

    public virtual void Dispose() => this.m_queryResult.Clear();

    public virtual void DebugDraw()
    {
      Color red = Color.Red;
      if (this.CustomDebugColor.HasValue)
        red = this.CustomDebugColor.Value;
      switch (this.m_triggerType)
      {
        case MyTriggerComponent.TriggerType.AABB:
          MyRenderProxy.DebugDrawAABB(this.m_AABB, this.m_queryResult.Count == 0 ? red : Color.Green, depthRead: false);
          break;
        case MyTriggerComponent.TriggerType.Sphere:
          MyRenderProxy.DebugDrawSphere(this.m_boundingSphere.Center, (float) this.m_boundingSphere.Radius, this.m_queryResult.Count == 0 ? red : Color.Green, depthRead: false);
          break;
        case MyTriggerComponent.TriggerType.OBB:
          MyRenderProxy.DebugDrawOBB(this.m_orientedBoundingBox, this.m_queryResult.Count == 0 ? red : Color.Green, 1f, false, false);
          break;
      }
      if (this.Entity != null)
        MyRenderProxy.DebugDrawLine3D(this.Center, this.Entity.PositionComp.GetPosition(), Color.Yellow, Color.Green, false);
      foreach (MyEntity myEntity in this.m_queryResult)
      {
        MyOrientedBoundingBoxD fromBoundingBox = MyOrientedBoundingBoxD.CreateFromBoundingBox((BoundingBoxD) myEntity.PositionComp.LocalAABB);
        fromBoundingBox.Transform(myEntity.PositionComp.WorldMatrixRef);
        MyRenderProxy.DebugDrawOBB(fromBoundingBox, Color.Yellow, 1f, false, false);
        MatrixD worldMatrix = myEntity.WorldMatrix;
        Vector3D translation1 = worldMatrix.Translation;
        worldMatrix = this.Entity.WorldMatrix;
        Vector3D translation2 = worldMatrix.Translation;
        Color yellow = Color.Yellow;
        Color green = Color.Green;
        MyRenderProxy.DebugDrawLine3D(translation1, translation2, yellow, green, false);
      }
    }

    protected virtual bool QueryEvaluator(MyEntity entity) => true;

    public override bool IsSerialized() => true;

    public bool Contains(Vector3D point)
    {
      switch (this.m_triggerType)
      {
        case MyTriggerComponent.TriggerType.AABB:
          return this.m_AABB.Contains(point) == ContainmentType.Contains;
        case MyTriggerComponent.TriggerType.Sphere:
          return this.m_boundingSphere.Contains(point) == ContainmentType.Contains;
        case MyTriggerComponent.TriggerType.OBB:
          return this.m_orientedBoundingBox.Contains(ref point);
        default:
          return false;
      }
    }

    public enum TriggerType
    {
      AABB,
      Sphere,
      OBB,
    }

    private class Sandbox_Game_Components_MyTriggerComponent\u003C\u003EActor : IActivator, IActivator<MyTriggerComponent>
    {
      object IActivator.CreateInstance() => (object) new MyTriggerComponent();

      MyTriggerComponent IActivator<MyTriggerComponent>.CreateInstance() => new MyTriggerComponent();
    }
  }
}
