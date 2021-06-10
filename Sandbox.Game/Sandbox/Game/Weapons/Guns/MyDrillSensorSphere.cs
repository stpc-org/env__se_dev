// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.Guns.MyDrillSensorSphere
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.WorldEnvironment;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons.Guns
{
  internal class MyDrillSensorSphere : MyDrillSensorBase
  {
    private float m_radius;
    private float m_centerForwardOffset;

    public MyDrillSensorSphere(
      float radius,
      float centerForwardOffset,
      MyDefinitionBase drillDefinition)
    {
      this.m_radius = radius;
      this.m_centerForwardOffset = centerForwardOffset;
      this.Center = (Vector3D) (centerForwardOffset * Vector3.Forward);
      this.FrontPoint = this.Center + Vector3.Forward * this.m_radius;
      this.m_drillDefinition = drillDefinition;
    }

    public override void OnWorldPositionChanged(ref MatrixD worldMatrix)
    {
      this.Center = worldMatrix.Translation + worldMatrix.Forward * (double) this.m_centerForwardOffset;
      this.FrontPoint = this.Center + worldMatrix.Forward * (double) this.m_radius;
    }

    protected override void ReadEntitiesInRange()
    {
      this.m_entitiesInRange.Clear();
      BoundingSphereD boundingSphere = new BoundingSphereD(this.Center, (double) this.m_radius);
      List<MyEntity> entitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
      bool flag = false;
      foreach (MyEntity entity in entitiesInSphere)
      {
        if (entity is MyEnvironmentSector)
          flag = true;
        if (!this.IgnoredEntities.Contains(entity))
          this.m_entitiesInRange[entity.EntityId] = new MyDrillSensorBase.DetectionInfo(entity, this.FrontPoint);
      }
      entitiesInSphere.Clear();
      if (!flag)
        return;
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay(this.Center, this.FrontPoint, 24);
      if (!nullable.HasValue || !nullable.HasValue)
        return;
      IMyEntity hitEntity = nullable.Value.HkHitInfo.GetHitEntity();
      if (!(hitEntity is MyEnvironmentSector))
        return;
      MyEnvironmentSector environmentSector = hitEntity as MyEnvironmentSector;
      uint shapeKey = nullable.Value.HkHitInfo.GetShapeKey(0);
      int itemFromShapeKey = environmentSector.GetItemFromShapeKey(shapeKey);
      if (environmentSector.DataView.Items[itemFromShapeKey].ModelIndex < (short) 0)
        return;
      this.m_entitiesInRange[hitEntity.EntityId] = new MyDrillSensorBase.DetectionInfo((MyEntity) environmentSector, this.FrontPoint, itemFromShapeKey);
    }

    public override void DebugDraw()
    {
      MyRenderProxy.DebugDrawSphere(this.Center, this.m_radius, Color.Yellow);
      MyRenderProxy.DebugDrawArrow3D(this.Center, this.FrontPoint, Color.Yellow, new Color?(Color.Red));
    }
  }
}
