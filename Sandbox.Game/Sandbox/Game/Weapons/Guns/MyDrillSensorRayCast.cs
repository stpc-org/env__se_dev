// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.Guns.MyDrillSensorRayCast
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.WorldEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons.Guns
{
  public class MyDrillSensorRayCast : MyDrillSensorBase
  {
    private readonly List<MyLineSegmentOverlapResult<MyEntity>> m_raycastResults = new List<MyLineSegmentOverlapResult<MyEntity>>();
    private readonly float m_rayLength;
    private readonly float m_originOffset;
    private Vector3D m_origin;
    private readonly List<MyPhysics.HitInfo> m_hits;
    private bool m_parallelRaycastRunning;

    public MyDrillSensorRayCast(
      float originOffset,
      float rayLength,
      MyDefinitionBase drillDefinition)
    {
      this.m_rayLength = rayLength;
      this.m_originOffset = originOffset;
      this.m_hits = new List<MyPhysics.HitInfo>();
      this.m_drillDefinition = drillDefinition;
    }

    public override void OnWorldPositionChanged(ref MatrixD worldMatrix)
    {
      if (this.m_parallelRaycastRunning)
        return;
      Vector3D forward = worldMatrix.Forward;
      this.m_origin = worldMatrix.Translation + forward * (double) this.m_originOffset;
      this.FrontPoint = this.m_origin + (double) this.m_rayLength * forward;
      this.Center = this.m_origin;
    }

    protected override void ReadEntitiesInRange()
    {
      if (this.m_parallelRaycastRunning)
        return;
      this.m_parallelRaycastRunning = true;
      this.m_hits.Clear();
      Vector3D frontPoint = this.FrontPoint;
      if (MyFakes.USE_PARALLEL_TOOL_RAYCAST)
      {
        MyPhysics.CastRayParallel(ref this.m_origin, ref frontPoint, this.m_hits, 24, new Action<List<MyPhysics.HitInfo>>(this.RayCastResult));
      }
      else
      {
        MyPhysics.CastRay(this.m_origin, frontPoint, this.m_hits, 24);
        this.ProcessToolRaycast();
        this.m_parallelRaycastRunning = false;
      }
    }

    private void RayCastResult(List<MyPhysics.HitInfo> list) => MySandboxGame.Static.Invoke((Action) (() =>
    {
      this.ProcessToolRaycast();
      this.m_parallelRaycastRunning = false;
    }), "Tool Sensor Raycast");

    private void ProcessToolRaycast()
    {
      this.m_entitiesInRange.Clear();
      MyDrillSensorBase.DetectionInfo detectionInfo = new MyDrillSensorBase.DetectionInfo();
      bool flag = false;
      foreach (MyPhysics.HitInfo hit in this.m_hits)
      {
        HkHitInfo hkHitInfo = hit.HkHitInfo;
        if (!((HkReferenceObject) hkHitInfo.Body == (HkReferenceObject) null))
        {
          IMyEntity hitEntity = hkHitInfo.GetHitEntity();
          if (hitEntity != null)
          {
            IMyEntity topMostParent = hitEntity.GetTopMostParent();
            if (!((IEnumerable<IMyEntity>) this.IgnoredEntities).Contains<IMyEntity>(topMostParent))
            {
              Vector3D position = hit.Position;
              if (topMostParent is MyCubeGrid myCubeGrid)
              {
                if (myCubeGrid.GridSizeEnum == MyCubeSize.Large)
                  position += hit.HkHitInfo.Normal * -0.08f;
                else
                  position += hit.HkHitInfo.Normal * -0.02f;
              }
              if (this.m_entitiesInRange.TryGetValue(topMostParent.EntityId, out detectionInfo))
              {
                if ((double) Vector3.DistanceSquared((Vector3) detectionInfo.DetectionPoint, (Vector3) this.m_origin) > (double) Vector3.DistanceSquared((Vector3) position, (Vector3) this.m_origin))
                  this.m_entitiesInRange[topMostParent.EntityId] = new MyDrillSensorBase.DetectionInfo(topMostParent as MyEntity, position);
              }
              else
                this.m_entitiesInRange[topMostParent.EntityId] = new MyDrillSensorBase.DetectionInfo(topMostParent as MyEntity, position);
              if (hitEntity is MyEnvironmentSector && !flag)
              {
                MyEnvironmentSector environmentSector = hitEntity as MyEnvironmentSector;
                uint shapeKey = hkHitInfo.GetShapeKey(0);
                int itemFromShapeKey = environmentSector.GetItemFromShapeKey(shapeKey);
                if (environmentSector.DataView.Items[itemFromShapeKey].ModelIndex >= (short) 0)
                {
                  flag = true;
                  this.m_entitiesInRange[hitEntity.EntityId] = new MyDrillSensorBase.DetectionInfo((MyEntity) environmentSector, position, itemFromShapeKey);
                }
              }
            }
          }
        }
      }
      LineD ray = new LineD(this.m_origin, this.FrontPoint);
      using (this.m_raycastResults.GetClearToken<MyLineSegmentOverlapResult<MyEntity>>())
      {
        MyGamePruningStructure.GetAllEntitiesInRay(ref ray, this.m_raycastResults);
        foreach (MyLineSegmentOverlapResult<MyEntity> raycastResult in this.m_raycastResults)
        {
          if (raycastResult.Element != null)
          {
            MyEntity topMostParent = raycastResult.Element.GetTopMostParent((System.Type) null);
            if (!this.IgnoredEntities.Contains(topMostParent) && raycastResult.Element is MyCubeBlock element)
            {
              Vector3D vector3D1 = new Vector3D();
              if (!element.SlimBlock.BlockDefinition.HasPhysics)
              {
                MatrixD matrix = element.PositionComp.WorldMatrixNormalizedInv;
                Vector3D vector3D2 = Vector3D.Transform(this.m_origin, ref matrix);
                Vector3D vector3D3 = Vector3D.Transform(this.FrontPoint, ref matrix);
                float? nullable1 = new Ray((Vector3) vector3D2, Vector3.Normalize(vector3D3 - vector3D2)).Intersects(element.PositionComp.LocalAABB);
                float num = 0.01f;
                float? nullable2 = nullable1.HasValue ? new float?(nullable1.GetValueOrDefault() + num) : new float?();
                if (nullable2.HasValue)
                {
                  nullable1 = nullable2;
                  float rayLength = this.m_rayLength;
                  if ((double) nullable1.GetValueOrDefault() <= (double) rayLength & nullable1.HasValue)
                  {
                    Vector3D detectionPoint = this.m_origin + Vector3D.Normalize(this.FrontPoint - this.m_origin) * (double) nullable2.Value;
                    if (this.m_entitiesInRange.TryGetValue(topMostParent.EntityId, out detectionInfo))
                    {
                      if ((double) Vector3.DistanceSquared((Vector3) detectionInfo.DetectionPoint, (Vector3) this.m_origin) > (double) Vector3.DistanceSquared((Vector3) detectionPoint, (Vector3) this.m_origin))
                        this.m_entitiesInRange[topMostParent.EntityId] = new MyDrillSensorBase.DetectionInfo(topMostParent, detectionPoint);
                    }
                    else
                      this.m_entitiesInRange[topMostParent.EntityId] = new MyDrillSensorBase.DetectionInfo(topMostParent, detectionPoint);
                  }
                }
              }
            }
          }
        }
      }
    }

    public override void DebugDraw() => MyRenderProxy.DebugDrawLine3D(this.m_origin, this.FrontPoint, Color.Red, Color.Blue, false);
  }
}
