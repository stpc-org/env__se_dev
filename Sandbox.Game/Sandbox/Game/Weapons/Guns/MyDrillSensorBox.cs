// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.Guns.MyDrillSensorBox
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons.Guns
{
  internal class MyDrillSensorBox : MyDrillSensorBase
  {
    private Vector3 m_halfExtents;
    private float m_centerOffset;
    private Quaternion m_orientation;

    public MyDrillSensorBox(Vector3 halfExtents, float centerOffset)
    {
      this.m_halfExtents = halfExtents;
      this.m_centerOffset = centerOffset;
      this.Center = (Vector3D) (Vector3.Forward * centerOffset);
      this.FrontPoint = this.Center + Vector3.Forward * this.m_halfExtents.Z;
    }

    public override void OnWorldPositionChanged(ref MatrixD worldMatrix)
    {
      MatrixD matrix = worldMatrix.GetOrientation();
      this.m_orientation = Quaternion.CreateFromRotationMatrix(in matrix);
      this.Center = worldMatrix.Translation + worldMatrix.Forward * (double) this.m_centerOffset;
      this.FrontPoint = this.Center + worldMatrix.Forward * (double) this.m_halfExtents.Z;
    }

    protected override void ReadEntitiesInRange()
    {
      this.m_entitiesInRange.Clear();
      BoundingBox aabb = new MyOrientedBoundingBox((Vector3) this.Center, this.m_halfExtents, this.m_orientation).GetAABB();
      List<MyEntity> entitiesInAabb = MyEntities.GetEntitiesInAABB(ref aabb);
      for (int index = 0; index < entitiesInAabb.Count; ++index)
      {
        MyEntity topMostParent = entitiesInAabb[index].GetTopMostParent((Type) null);
        if (!this.IgnoredEntities.Contains(topMostParent))
          this.m_entitiesInRange[topMostParent.EntityId] = new MyDrillSensorBase.DetectionInfo(topMostParent, this.FrontPoint);
      }
      entitiesInAabb.Clear();
    }

    public override void DebugDraw() => MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(this.Center, (Vector3D) this.m_halfExtents, this.m_orientation), (Color) new Vector3(1f, 0.0f, 0.0f), 0.6f, true, false);
  }
}
