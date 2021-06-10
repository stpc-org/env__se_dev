// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.RecastDetour.MyNavigationInputMesh
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Game.WorldEnvironment.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Entity;
using VRage.Game.Voxels;
using VRage.Groups;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
  internal class MyNavigationInputMesh
  {
    private readonly bool ENABLE_GRID_PATHFINDING = true;
    private static readonly MyNavigationInputMesh.IcoSphereMesh m_icosphereMesh = new MyNavigationInputMesh.IcoSphereMesh();
    private static readonly MyNavigationInputMesh.CapsuleMesh m_capsuleMesh = new MyNavigationInputMesh.CapsuleMesh();
    private static List<HkShape> m_tmpShapes;
    private const int NAVMESH_LOD = 0;
    private readonly Dictionary<Vector3I, MyIsoMesh> m_meshCache = new Dictionary<Vector3I, MyIsoMesh>(1024, (IEqualityComparer<Vector3I>) new Vector3I.EqualityComparer());
    private readonly List<MyNavigationInputMesh.CacheInterval> m_invalidateMeshCacheCoord = new List<MyNavigationInputMesh.CacheInterval>();
    private readonly List<MyNavigationInputMesh.CacheInterval> m_tmpInvalidCache = new List<MyNavigationInputMesh.CacheInterval>();
    private readonly MyPlanet m_planet;
    private readonly Vector3D m_center;
    private readonly Quaternion m_rdWorldQuaternion;
    private readonly MyRDPathfinding m_rdPathfinding;
    private readonly List<MyNavigationInputMesh.GridInfo> m_lastGridsInfo = new List<MyNavigationInputMesh.GridInfo>();
    private readonly List<MyNavigationInputMesh.CubeInfo> m_lastIntersectedGridsInfoCubes = new List<MyNavigationInputMesh.CubeInfo>();

    public MyNavigationInputMesh(MyRDPathfinding rdPathfinding, MyPlanet planet, Vector3D center)
    {
      this.m_rdPathfinding = rdPathfinding;
      this.m_planet = planet;
      this.m_center = center;
      Vector3 vector3 = -(Vector3) Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(this.m_center));
      this.m_rdWorldQuaternion = Quaternion.Inverse(Quaternion.CreateFromForwardUp(Vector3.CalculatePerpendicularVector(vector3), vector3));
    }

    public void CreateWorldVerticesAndShapes(
      float border,
      Vector3D originPosition,
      MyOrientedBoundingBoxD obb,
      List<MyVoxelMap> trackedEntities,
      out List<BoundingBoxD> boundingBoxes,
      out WorldVerticesInfo worldVertices,
      out MyShapesInfo myShapesInfo)
    {
      worldVertices = new WorldVerticesInfo();
      myShapesInfo = new MyShapesInfo();
      boundingBoxes = new List<BoundingBoxD>();
      this.AddEntities(border, originPosition, obb, boundingBoxes, trackedEntities, worldVertices, myShapesInfo);
    }

    public WorldVerticesInfo AddGroundToWorldVertices(
      float border,
      Vector3D originPosition,
      MyOrientedBoundingBoxD obb,
      List<BoundingBoxD> boundingBoxes,
      WorldVerticesInfo worldVertices)
    {
      this.AddGround(border, originPosition, obb, boundingBoxes, worldVertices);
      return worldVertices;
    }

    public WorldVerticesInfo AddPhysicalShapeVertices(
      MyShapesInfo shapesInfo,
      WorldVerticesInfo worldVertices)
    {
      MyNavigationInputMesh.AddPhysicalShapes(worldVertices, shapesInfo);
      return worldVertices;
    }

    public void AddTrees(MyOrientedBoundingBoxD obb, List<MyBoxShapeInfo> boxShapes)
    {
      MyEnvironmentSector sectorForPosition = this.m_planet.Components.Get<MyPlanetEnvironmentComponent>()?.GetSectorForPosition(obb.Center);
      if (sectorForPosition?.DataView == null)
        return;
      foreach (Sandbox.Game.WorldEnvironment.ItemInfo itemInfo in sectorForPosition.DataView.Items)
      {
        MyRuntimeEnvironmentItemInfo environmentItemInfo;
        if (sectorForPosition.EnvironmentDefinition.Items.TryGetValue<MyRuntimeEnvironmentItemInfo>((int) itemInfo.DefinitionIndex, out environmentItemInfo) && !(environmentItemInfo.Type.Name != "Tree"))
        {
          Vector3D point = itemInfo.Position + sectorForPosition.SectorCenter;
          if (obb.GetAABB().Contains(point) == ContainmentType.Contains)
          {
            Matrix translation = Matrix.CreateTranslation((Vector3) point);
            ref Matrix local = ref translation;
            local.Translation = (Vector3) (local.Translation - this.m_center);
            MatrixD matrixD = MatrixD.Transform((MatrixD) ref translation, this.m_rdWorldQuaternion);
            boxShapes.Add(new MyBoxShapeInfo((Matrix) ref matrixD, 2f, 2f, 15f));
          }
        }
      }
    }

    public void DebugDraw()
    {
      foreach (MyNavigationInputMesh.GridInfo gridInfo in this.m_lastGridsInfo)
      {
        foreach (MyNavigationInputMesh.CubeInfo cube in gridInfo.Cubes)
        {
          if (this.m_lastIntersectedGridsInfoCubes.Contains(cube))
            MyRenderProxy.DebugDrawAABB(cube.BoundingBox, Color.White);
          else
            MyRenderProxy.DebugDrawAABB(cube.BoundingBox, Color.Yellow);
        }
      }
    }

    public void InvalidateCache(BoundingBoxD box)
    {
      Vector3D vector3D1 = Vector3D.Transform(box.Min, this.m_planet.PositionComp.WorldMatrixInvScaled);
      Vector3D vector3D2 = Vector3D.Transform(box.Max, this.m_planet.PositionComp.WorldMatrixInvScaled);
      Vector3D xyz1 = vector3D1 + this.m_planet.SizeInMetresHalf;
      Vector3D xyz2 = vector3D2 + this.m_planet.SizeInMetresHalf;
      Vector3I voxelCoord1 = new Vector3I(xyz1);
      Vector3I voxelCoord2 = new Vector3I(xyz2);
      Vector3I geometryCellCoord1;
      MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord1, out geometryCellCoord1);
      Vector3I geometryCellCoord2;
      MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord2, out geometryCellCoord2);
      this.m_invalidateMeshCacheCoord.Add(new MyNavigationInputMesh.CacheInterval()
      {
        Min = geometryCellCoord1,
        Max = geometryCellCoord2
      });
    }

    public void RefreshCache() => this.m_meshCache.Clear();

    public void Clear() => this.m_meshCache.Clear();

    private void AddEntities(
      float border,
      Vector3D originPosition,
      MyOrientedBoundingBoxD obb,
      List<BoundingBoxD> boundingBoxes,
      List<MyVoxelMap> trackedEntities,
      WorldVerticesInfo worldVertices,
      MyShapesInfo myShapesInfo)
    {
      obb.HalfExtent += new Vector3D((double) border, 0.0, (double) border);
      BoundingBoxD aabb = obb.GetAABB();
      this.AddTrees(obb, myShapesInfo.Boxes);
      List<MyEntity> result = new List<MyEntity>();
      MyGamePruningStructure.GetTopMostEntitiesInBox(ref aabb, result, MyEntityQueryType.Static);
      foreach (MyEntity myEntity in result)
      {
        using (myEntity.Pin())
        {
          if (!myEntity.MarkedForClose)
          {
            if (this.ENABLE_GRID_PATHFINDING && myEntity is MyCubeGrid grid)
              this.AddGridVerticesInsideOBB(grid, obb, myShapesInfo);
            else if (myEntity is MyVoxelMap voxelMap)
            {
              trackedEntities.Add(voxelMap);
              this.AddVoxelVertices(voxelMap, border, originPosition, obb, boundingBoxes, worldVertices);
            }
          }
        }
      }
    }

    private void AddGridVerticesInsideOBB(
      MyCubeGrid grid,
      MyOrientedBoundingBoxD obb,
      MyShapesInfo myShapesInfo)
    {
      BoundingBoxD aabb = obb.GetAABB();
      foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in MyCubeGridGroups.Static.Logical.GetGroup(grid).Nodes)
      {
        MyCubeGrid nodeData = node.NodeData;
        this.m_rdPathfinding.AddToTrackedGrids(nodeData);
        MatrixD worldMatrix = nodeData.WorldMatrix;
        worldMatrix.Translation -= this.m_center;
        MatrixD matrixD = MatrixD.Transform(worldMatrix, this.m_rdWorldQuaternion);
        if (MyPerGameSettings.Game == GameEnum.SE_GAME)
        {
          BoundingBoxD boundingBoxD = aabb.TransformFast(nodeData.PositionComp.WorldMatrixNormalizedInv);
          Vector3I min = new Vector3I((int) Math.Round(boundingBoxD.Min.X), (int) Math.Round(boundingBoxD.Min.Y), (int) Math.Round(boundingBoxD.Min.Z));
          Vector3I max = new Vector3I((int) Math.Round(boundingBoxD.Max.X), (int) Math.Round(boundingBoxD.Max.Y), (int) Math.Round(boundingBoxD.Max.Z));
          min = Vector3I.Min(min, max);
          max = Vector3I.Max(min, max);
          if (nodeData.Physics != null)
          {
            using (MyUtils.ReuseCollection<HkShape>(ref MyNavigationInputMesh.m_tmpShapes))
            {
              nodeData.Physics.Shape.GetShapesInInterval(min, max, MyNavigationInputMesh.m_tmpShapes);
              foreach (HkShape tmpShape in MyNavigationInputMesh.m_tmpShapes)
                MyNavigationInputMesh.ParsePhysicalShape(tmpShape, (Matrix) ref matrixD, myShapesInfo.Boxes, myShapesInfo.Spheres, myShapesInfo.ConvexVertices);
            }
          }
        }
      }
    }

    private static void ParsePhysicalShape(
      HkShape shape,
      Matrix rdWorldMatrix,
      List<MyBoxShapeInfo> boxes,
      List<MySphereShapeInfo> spheres,
      List<MyConvexVerticesInfo> convexVertices)
    {
      while (true)
      {
        switch (shape.ShapeType)
        {
          case HkShapeType.Sphere:
            goto label_10;
          case HkShapeType.Cylinder:
            goto label_6;
          case HkShapeType.Triangle:
            goto label_13;
          case HkShapeType.Box:
            goto label_2;
          case HkShapeType.Capsule:
            goto label_14;
          case HkShapeType.ConvexVertices:
            goto label_11;
          case HkShapeType.TriSampledHeightFieldCollection:
            goto label_15;
          case HkShapeType.TriSampledHeightFieldBvTree:
            goto label_16;
          case HkShapeType.List:
            goto label_3;
          case HkShapeType.Mopp:
            shape = (HkShape) ((HkMoppBvTreeShape) shape).ShapeCollection;
            continue;
          case HkShapeType.ConvexTranslate:
            HkConvexTranslateShape convexTranslateShape = (HkConvexTranslateShape) shape;
            Matrix translation = Matrix.CreateTranslation(convexTranslateShape.Translation);
            shape = (HkShape) convexTranslateShape.ChildShape;
            Matrix matrix = rdWorldMatrix;
            rdWorldMatrix = translation * matrix;
            continue;
          case HkShapeType.ConvexTransform:
            HkConvexTransformShape convexTransformShape = (HkConvexTransformShape) shape;
            shape = (HkShape) convexTransformShape.ChildShape;
            rdWorldMatrix = convexTransformShape.Transform * rdWorldMatrix;
            continue;
          default:
            goto label_12;
        }
      }
label_6:
      return;
label_13:
      return;
label_14:
      return;
label_15:
      return;
label_16:
      return;
label_12:
      return;
label_2:
      HkBoxShape hkBoxShape = (HkBoxShape) shape;
      boxes.Add(new MyBoxShapeInfo(rdWorldMatrix, hkBoxShape.HalfExtents.X, hkBoxShape.HalfExtents.Y, hkBoxShape.HalfExtents.Z));
      return;
label_3:
      HkShapeContainerIterator iterator = ((HkListShape) shape).GetIterator();
      while (iterator.IsValid)
      {
        MyNavigationInputMesh.ParsePhysicalShape(iterator.CurrentValue, rdWorldMatrix, boxes, spheres, convexVertices);
        iterator.Next();
      }
      return;
label_10:
      spheres.Add(new MySphereShapeInfo(rdWorldMatrix.Translation, ((HkSphereShape) shape).Radius));
      return;
label_11:
      Vector3[] vertices;
      ((HkConvexVerticesShape) shape).GetVertices(out vertices);
      convexVertices.Add(new MyConvexVerticesInfo(rdWorldMatrix, vertices));
    }

    private static void AddPhysicalShapes(WorldVerticesInfo worldVertices, MyShapesInfo shapesInfo)
    {
      foreach (MyConvexVerticesInfo convexVertex in shapesInfo.ConvexVertices)
      {
        HkConvexVerticesShape convexVerticesShape = new HkConvexVerticesShape(convexVertex.Vertices, convexVertex.Vertices.Length);
        using (HkGeometry geometry = new HkGeometry())
        {
          convexVerticesShape.GetGeometry(geometry, out Vector3 _);
          for (int triangleIndex = 0; triangleIndex < geometry.TriangleCount; ++triangleIndex)
          {
            int i0;
            int i1;
            int i2;
            geometry.GetTriangle(triangleIndex, out i0, out i1, out i2, out int _);
            worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + i0);
            worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + i1);
            worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + i2);
          }
          for (int vertexIndex = 0; vertexIndex < geometry.VertexCount; ++vertexIndex)
          {
            Vector3 result = geometry.GetVertex(vertexIndex);
            Vector3.Transform(ref result, ref convexVertex.m_rdWorldMatrix, out result);
            worldVertices.Vertices.Add(result);
          }
          worldVertices.VerticesMaxValue += geometry.VertexCount;
        }
        convexVerticesShape.Base.RemoveReference();
      }
      foreach (MyBoxShapeInfo box in shapesInfo.Boxes)
        MyNavigationInputMesh.BoundingBoxToTranslatedTriangles(new BoundingBoxD(new Vector3D(-(double) box.HalfExtentsX, -(double) box.HalfExtentsY, -(double) box.HalfExtentsZ), new Vector3D((double) box.HalfExtentsX, (double) box.HalfExtentsY, (double) box.HalfExtentsZ)), box.RdWorldMatrix, worldVertices);
      foreach (MySphereShapeInfo sphere in shapesInfo.Spheres)
        MyNavigationInputMesh.m_icosphereMesh.AddTrianglesToWorldVertices(sphere.RdWorldTranslation, sphere.Radius, worldVertices);
    }

    private void AddVoxelVertices(
      MyVoxelMap voxelMap,
      float border,
      Vector3D originPosition,
      MyOrientedBoundingBoxD obb,
      List<BoundingBoxD> bbList,
      WorldVerticesInfo worldVertices)
    {
      this.AddVoxelMesh((MyVoxelBase) voxelMap, voxelMap.Storage, (Dictionary<Vector3I, MyIsoMesh>) null, border, originPosition, obb, bbList, worldVertices);
    }

    private static void BoundingBoxToTranslatedTriangles(
      BoundingBoxD bbox,
      Matrix worldMatrix,
      WorldVerticesInfo worldVertices)
    {
      Vector3 result1 = new Vector3(bbox.Min.X, bbox.Max.Y, bbox.Max.Z);
      Vector3 result2 = new Vector3(bbox.Max.X, bbox.Max.Y, bbox.Max.Z);
      Vector3 result3 = new Vector3(bbox.Min.X, bbox.Max.Y, bbox.Min.Z);
      Vector3 result4 = new Vector3(bbox.Max.X, bbox.Max.Y, bbox.Min.Z);
      Vector3 result5 = new Vector3(bbox.Min.X, bbox.Min.Y, bbox.Max.Z);
      Vector3 result6 = new Vector3(bbox.Max.X, bbox.Min.Y, bbox.Max.Z);
      Vector3 result7 = new Vector3(bbox.Min.X, bbox.Min.Y, bbox.Min.Z);
      Vector3 result8 = new Vector3(bbox.Max.X, bbox.Min.Y, bbox.Min.Z);
      Vector3.Transform(ref result1, ref worldMatrix, out result1);
      Vector3.Transform(ref result2, ref worldMatrix, out result2);
      Vector3.Transform(ref result3, ref worldMatrix, out result3);
      Vector3.Transform(ref result4, ref worldMatrix, out result4);
      Vector3.Transform(ref result5, ref worldMatrix, out result5);
      Vector3.Transform(ref result6, ref worldMatrix, out result6);
      Vector3.Transform(ref result7, ref worldMatrix, out result7);
      Vector3.Transform(ref result8, ref worldMatrix, out result8);
      worldVertices.Vertices.Add(result1);
      worldVertices.Vertices.Add(result2);
      worldVertices.Vertices.Add(result3);
      worldVertices.Vertices.Add(result4);
      worldVertices.Vertices.Add(result5);
      worldVertices.Vertices.Add(result6);
      worldVertices.Vertices.Add(result7);
      worldVertices.Vertices.Add(result8);
      int verticesMaxValue = worldVertices.VerticesMaxValue;
      int num1 = worldVertices.VerticesMaxValue + 1;
      int num2 = worldVertices.VerticesMaxValue + 2;
      int num3 = worldVertices.VerticesMaxValue + 3;
      int num4 = worldVertices.VerticesMaxValue + 4;
      int num5 = worldVertices.VerticesMaxValue + 5;
      int num6 = worldVertices.VerticesMaxValue + 6;
      int num7 = worldVertices.VerticesMaxValue + 7;
      worldVertices.Triangles.Add(num3);
      worldVertices.Triangles.Add(num2);
      worldVertices.Triangles.Add(verticesMaxValue);
      worldVertices.Triangles.Add(verticesMaxValue);
      worldVertices.Triangles.Add(num1);
      worldVertices.Triangles.Add(num3);
      worldVertices.Triangles.Add(num4);
      worldVertices.Triangles.Add(num6);
      worldVertices.Triangles.Add(num7);
      worldVertices.Triangles.Add(num7);
      worldVertices.Triangles.Add(num5);
      worldVertices.Triangles.Add(num4);
      worldVertices.Triangles.Add(num2);
      worldVertices.Triangles.Add(num7);
      worldVertices.Triangles.Add(num6);
      worldVertices.Triangles.Add(num2);
      worldVertices.Triangles.Add(num3);
      worldVertices.Triangles.Add(num7);
      worldVertices.Triangles.Add(verticesMaxValue);
      worldVertices.Triangles.Add(num4);
      worldVertices.Triangles.Add(num5);
      worldVertices.Triangles.Add(num5);
      worldVertices.Triangles.Add(num1);
      worldVertices.Triangles.Add(verticesMaxValue);
      worldVertices.Triangles.Add(num6);
      worldVertices.Triangles.Add(num4);
      worldVertices.Triangles.Add(verticesMaxValue);
      worldVertices.Triangles.Add(verticesMaxValue);
      worldVertices.Triangles.Add(num2);
      worldVertices.Triangles.Add(num6);
      worldVertices.Triangles.Add(num1);
      worldVertices.Triangles.Add(num5);
      worldVertices.Triangles.Add(num7);
      worldVertices.Triangles.Add(num7);
      worldVertices.Triangles.Add(num3);
      worldVertices.Triangles.Add(num1);
      worldVertices.VerticesMaxValue += 8;
    }

    private static void AddMeshTriangles(
      MyIsoMesh mesh,
      Vector3 offset,
      Matrix rotation,
      Matrix ownRotation,
      WorldVerticesInfo worldVertices)
    {
      for (int index = 0; index < mesh.TrianglesCount; ++index)
      {
        ushort v0 = mesh.Triangles[index].V0;
        ushort v1 = mesh.Triangles[index].V1;
        ushort v2 = mesh.Triangles[index].V2;
        worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + (int) v2);
        worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + (int) v1);
        worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + (int) v0);
      }
      for (int idx = 0; idx < mesh.VerticesCount; ++idx)
      {
        Vector3 position;
        mesh.GetUnpackedPosition(idx, out position);
        Vector3.Transform(ref position, ref ownRotation, out position);
        position -= offset;
        Vector3.Transform(ref position, ref rotation, out position);
        worldVertices.Vertices.Add(position);
      }
      worldVertices.VerticesMaxValue += mesh.VerticesCount;
    }

    private unsafe void GetMiddleOBBLocalPoints(MyOrientedBoundingBoxD obb, ref Vector3* points)
    {
      Vector3 vector3_1 = obb.Orientation.Right * (float) obb.HalfExtent.X;
      Vector3 vector3_2 = obb.Orientation.Forward * (float) obb.HalfExtent.Z;
      Vector3 vector3_3 = (Vector3) (obb.Center - this.m_planet.PositionComp.GetPosition());
      *points = vector3_3 - vector3_1 - vector3_2;
      points[1] = vector3_3 + vector3_1 - vector3_2;
      points[2] = vector3_3 + vector3_1 + vector3_2;
      points[3] = vector3_3 - vector3_1 + vector3_2;
    }

    private unsafe bool SetTerrainLimits(ref MyOrientedBoundingBoxD obb)
    {
      Vector3* points = stackalloc Vector3[4];
      this.GetMiddleOBBLocalPoints(obb, ref points);
      float minHeight;
      float maxHeight;
      this.m_planet.Provider.Shape.GetBounds(points, 4, out minHeight, out maxHeight);
      if (!minHeight.IsValid() || !maxHeight.IsValid())
        return false;
      Vector3D vector3D1 = obb.Orientation.Up * minHeight + this.m_planet.PositionComp.GetPosition();
      Vector3D vector3D2 = obb.Orientation.Up * maxHeight + this.m_planet.PositionComp.GetPosition();
      obb.Center = (vector3D1 + vector3D2) * 0.5;
      float num = Math.Max(maxHeight - minHeight, 1f);
      obb.HalfExtent.Y = (double) num * 0.5;
      return true;
    }

    private void AddGround(
      float border,
      Vector3D originPosition,
      MyOrientedBoundingBoxD obb,
      List<BoundingBoxD> bbList,
      WorldVerticesInfo worldVertices)
    {
      if (!this.SetTerrainLimits(ref obb))
        return;
      this.AddVoxelMesh((MyVoxelBase) this.m_planet, this.m_planet.Storage, this.m_meshCache, border, originPosition, obb, bbList, worldVertices);
    }

    private void CheckCacheValidity()
    {
      if (this.m_invalidateMeshCacheCoord.Count <= 0)
        return;
      this.m_tmpInvalidCache.AddRange((IEnumerable<MyNavigationInputMesh.CacheInterval>) this.m_invalidateMeshCacheCoord);
      this.m_invalidateMeshCacheCoord.Clear();
      foreach (MyNavigationInputMesh.CacheInterval cacheInterval in this.m_tmpInvalidCache)
      {
        for (int index = 0; index < this.m_meshCache.Count; ++index)
        {
          Vector3I key = this.m_meshCache.ElementAt<KeyValuePair<Vector3I, MyIsoMesh>>(index).Key;
          if (key.X >= cacheInterval.Min.X && key.Y >= cacheInterval.Min.Y && (key.Z >= cacheInterval.Min.Z && key.X <= cacheInterval.Max.X) && (key.Y <= cacheInterval.Max.Y && key.Z <= cacheInterval.Max.Z))
          {
            this.m_meshCache.Remove(key);
            break;
          }
        }
      }
      this.m_tmpInvalidCache.Clear();
    }

    private void AddVoxelMesh(
      MyVoxelBase voxelBase,
      IMyStorage storage,
      Dictionary<Vector3I, MyIsoMesh> cache,
      float border,
      Vector3D originPosition,
      MyOrientedBoundingBoxD obb,
      List<BoundingBoxD> bbList,
      WorldVerticesInfo worldVertices)
    {
      bool flag = cache != null;
      if (flag)
        this.CheckCacheValidity();
      obb.HalfExtent += new Vector3D((double) border, 0.0, (double) border);
      BoundingBoxD boundingBoxD1 = obb.GetAABB();
      int num1 = (int) Math.Round(boundingBoxD1.HalfExtents.Max() * 2.0);
      boundingBoxD1 = new BoundingBoxD(boundingBoxD1.Min, boundingBoxD1.Min + (float) num1);
      boundingBoxD1.Translate(obb.Center - boundingBoxD1.Center);
      bbList.Add(new BoundingBoxD(boundingBoxD1.Min, boundingBoxD1.Max));
      boundingBoxD1 = boundingBoxD1.TransformFast(voxelBase.PositionComp.WorldMatrixInvScaled);
      boundingBoxD1.Translate((Vector3D) voxelBase.SizeInMetresHalf);
      Vector3I voxelCoord1 = Vector3I.Round(boundingBoxD1.Min);
      Vector3I voxelCoord2 = voxelCoord1 + num1;
      Vector3I geometryCellCoord1;
      MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord1, out geometryCellCoord1);
      Vector3I geometryCellCoord2;
      MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord2, out geometryCellCoord2);
      MyOrientedBoundingBoxD orientedBoundingBoxD = obb;
      orientedBoundingBoxD.Transform(voxelBase.PositionComp.WorldMatrixInvScaled);
      orientedBoundingBoxD.Center += voxelBase.SizeInMetresHalf;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref geometryCellCoord1, ref geometryCellCoord2);
      MyCellCoord myCellCoord = new MyCellCoord();
      myCellCoord.Lod = 0;
      int num2 = 0;
      Vector3 offset = (Vector3) (originPosition - voxelBase.PositionLeftBottomCorner);
      Vector3 vector3 = -Vector3.Normalize(MyGravityProviderSystem.CalculateTotalGravityInPoint(originPosition));
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(Quaternion.Inverse(Quaternion.CreateFromForwardUp(Vector3.CalculatePerpendicularVector(vector3), vector3)));
      MatrixD matrixD = voxelBase.PositionComp.WorldMatrixRef;
      matrixD = matrixD.GetOrientation();
      Matrix ownRotation = (Matrix) ref matrixD;
      while (vector3IRangeIterator.IsValid())
      {
        MyIsoMesh mesh1;
        if (flag && cache.TryGetValue(vector3IRangeIterator.Current, out mesh1))
        {
          if (mesh1 != null)
            MyNavigationInputMesh.AddMeshTriangles(mesh1, offset, fromQuaternion, ownRotation, worldVertices);
          vector3IRangeIterator.MoveNext();
        }
        else
        {
          myCellCoord.CoordInLod = vector3IRangeIterator.Current;
          BoundingBox localAABB;
          MyVoxelCoordSystems.GeometryCellCoordToLocalAABB(ref myCellCoord.CoordInLod, out localAABB);
          if (!orientedBoundingBoxD.Intersects(ref localAABB))
          {
            ++num2;
            vector3IRangeIterator.MoveNext();
          }
          else
          {
            BoundingBoxD boundingBoxD2 = new BoundingBoxD((Vector3D) localAABB.Min, (Vector3D) localAABB.Max).Translate((Vector3D) -voxelBase.SizeInMetresHalf);
            bbList.Add(boundingBoxD2);
            Vector3I lodVoxelMin = myCellCoord.CoordInLod * 8 - 1;
            Vector3I lodVoxelMax = lodVoxelMin + 8 + 1 + 1;
            MyIsoMesh mesh2 = MyPrecalcComponent.IsoMesher.Precalc(storage, 0, lodVoxelMin, lodVoxelMax, MyStorageDataTypeFlags.Content);
            if (flag)
              cache[vector3IRangeIterator.Current] = mesh2;
            if (mesh2 != null)
              MyNavigationInputMesh.AddMeshTriangles(mesh2, offset, fromQuaternion, ownRotation, worldVertices);
            vector3IRangeIterator.MoveNext();
          }
        }
      }
    }

    public class CubeInfo
    {
      public int Id { get; set; }

      public BoundingBoxD BoundingBox { get; set; }

      public List<Vector3D> TriangleVertices { get; set; }
    }

    private struct GridInfo
    {
      public long Id { get; set; }

      public List<MyNavigationInputMesh.CubeInfo> Cubes { get; set; }
    }

    private struct CacheInterval
    {
      public Vector3I Min;
      public Vector3I Max;
    }

    public class IcoSphereMesh
    {
      private const int RECURSION_LEVEL = 1;
      private int m_index;
      private Dictionary<long, int> m_middlePointIndexCache;
      private List<int> m_triangleIndices;
      private List<Vector3> m_positions;

      public IcoSphereMesh() => this.Create();

      private int AddVertex(Vector3 p)
      {
        double num = Math.Sqrt((double) p.X * (double) p.X + (double) p.Y * (double) p.Y + (double) p.Z * (double) p.Z);
        this.m_positions.Add(new Vector3((double) p.X / num, (double) p.Y / num, (double) p.Z / num));
        return this.m_index++;
      }

      private int GetMiddlePoint(int p1, int p2)
      {
        int num1 = p1 < p2 ? 1 : 0;
        long key = ((num1 != 0 ? (long) p1 : (long) p2) << 32) + (num1 != 0 ? (long) p2 : (long) p1);
        int num2;
        if (this.m_middlePointIndexCache.TryGetValue(key, out num2))
          return num2;
        Vector3 position1 = this.m_positions[p1];
        Vector3 position2 = this.m_positions[p2];
        int num3 = this.AddVertex(new Vector3(((double) position1.X + (double) position2.X) / 2.0, ((double) position1.Y + (double) position2.Y) / 2.0, ((double) position1.Z + (double) position2.Z) / 2.0));
        this.m_middlePointIndexCache.Add(key, num3);
        return num3;
      }

      private void Create()
      {
        this.m_middlePointIndexCache = new Dictionary<long, int>();
        this.m_triangleIndices = new List<int>();
        this.m_positions = new List<Vector3>();
        this.m_index = 0;
        double num = (1.0 + Math.Sqrt(5.0)) / 2.0;
        this.AddVertex(new Vector3(-1.0, num, 0.0));
        this.AddVertex(new Vector3(1.0, num, 0.0));
        this.AddVertex(new Vector3(-1.0, -num, 0.0));
        this.AddVertex(new Vector3(1.0, -num, 0.0));
        this.AddVertex(new Vector3(0.0, -1.0, num));
        this.AddVertex(new Vector3(0.0, 1.0, num));
        this.AddVertex(new Vector3(0.0, -1.0, -num));
        this.AddVertex(new Vector3(0.0, 1.0, -num));
        this.AddVertex(new Vector3(num, 0.0, -1.0));
        this.AddVertex(new Vector3(num, 0.0, 1.0));
        this.AddVertex(new Vector3(-num, 0.0, -1.0));
        this.AddVertex(new Vector3(-num, 0.0, 1.0));
        List<MyNavigationInputMesh.IcoSphereMesh.TriangleIndices> triangleIndicesList1 = new List<MyNavigationInputMesh.IcoSphereMesh.TriangleIndices>();
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(0, 11, 5));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(0, 5, 1));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(0, 1, 7));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(0, 7, 10));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(0, 10, 11));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(1, 5, 9));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(5, 11, 4));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(11, 10, 2));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(10, 7, 6));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(7, 1, 8));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(3, 9, 4));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(3, 4, 2));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(3, 2, 6));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(3, 6, 8));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(3, 8, 9));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(4, 9, 5));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(2, 4, 11));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(6, 2, 10));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(8, 6, 7));
        triangleIndicesList1.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(9, 8, 1));
        for (int index = 0; index < 1; ++index)
        {
          List<MyNavigationInputMesh.IcoSphereMesh.TriangleIndices> triangleIndicesList2 = new List<MyNavigationInputMesh.IcoSphereMesh.TriangleIndices>();
          foreach (MyNavigationInputMesh.IcoSphereMesh.TriangleIndices triangleIndices in triangleIndicesList1)
          {
            int middlePoint1 = this.GetMiddlePoint(triangleIndices.v1, triangleIndices.v2);
            int middlePoint2 = this.GetMiddlePoint(triangleIndices.v2, triangleIndices.v3);
            int middlePoint3 = this.GetMiddlePoint(triangleIndices.v3, triangleIndices.v1);
            triangleIndicesList2.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(triangleIndices.v1, middlePoint1, middlePoint3));
            triangleIndicesList2.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(triangleIndices.v2, middlePoint2, middlePoint1));
            triangleIndicesList2.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(triangleIndices.v3, middlePoint3, middlePoint2));
            triangleIndicesList2.Add(new MyNavigationInputMesh.IcoSphereMesh.TriangleIndices(middlePoint1, middlePoint2, middlePoint3));
          }
          triangleIndicesList1 = triangleIndicesList2;
        }
        foreach (MyNavigationInputMesh.IcoSphereMesh.TriangleIndices triangleIndices in triangleIndicesList1)
        {
          this.m_triangleIndices.Add(triangleIndices.v1);
          this.m_triangleIndices.Add(triangleIndices.v2);
          this.m_triangleIndices.Add(triangleIndices.v3);
        }
      }

      public void AddTrianglesToWorldVertices(
        Vector3 center,
        float radius,
        WorldVerticesInfo worldVertices)
      {
        foreach (int triangleIndex in this.m_triangleIndices)
          worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + triangleIndex);
        foreach (Vector3 position in this.m_positions)
          worldVertices.Vertices.Add(center + position * radius);
        worldVertices.VerticesMaxValue += this.m_positions.Count;
      }

      private struct TriangleIndices
      {
        public readonly int v1;
        public readonly int v2;
        public readonly int v3;

        public TriangleIndices(int v1, int v2, int v3)
        {
          this.v1 = v1;
          this.v2 = v2;
          this.v3 = v3;
        }
      }
    }

    public class CapsuleMesh
    {
      private const double P_ID2 = 1.5707963267949;
      private const double P_IM2 = 6.28318530717959;
      private readonly List<Vector3> m_verticeList = new List<Vector3>();
      private readonly List<int> m_triangleList = new List<int>();
      private readonly int N = 8;
      private readonly float m_radius = 1f;
      private readonly float m_height;

      public CapsuleMesh() => this.Create();

      private void Create()
      {
        for (int index1 = 0; index1 <= this.N / 4; ++index1)
        {
          for (int index2 = 0; index2 <= this.N; ++index2)
          {
            Vector3 vector3 = new Vector3();
            double num1 = (double) index2 * (2.0 * Math.PI) / (double) this.N;
            double num2 = Math.PI * (double) index1 / (double) (this.N / 2) - Math.PI / 2.0;
            vector3.X = this.m_radius * (float) (Math.Cos(num2) * Math.Cos(num1));
            vector3.Y = this.m_radius * (float) (Math.Cos(num2) * Math.Sin(num1));
            vector3.Z = (float) ((double) this.m_radius * Math.Sin(num2) - (double) this.m_height / 2.0);
            this.m_verticeList.Add(vector3);
          }
        }
        for (int index1 = this.N / 4; index1 <= this.N / 2; ++index1)
        {
          for (int index2 = 0; index2 <= this.N; ++index2)
          {
            Vector3 vector3 = new Vector3();
            double num1 = (double) index2 * (2.0 * Math.PI) / (double) this.N;
            double num2 = Math.PI * (double) index1 / (double) (this.N / 2) - Math.PI / 2.0;
            vector3.X = this.m_radius * (float) (Math.Cos(num2) * Math.Cos(num1));
            vector3.Y = this.m_radius * (float) (Math.Cos(num2) * Math.Sin(num1));
            vector3.Z = (float) ((double) this.m_radius * Math.Sin(num2) + (double) this.m_height / 2.0);
            this.m_verticeList.Add(vector3);
          }
        }
        for (int index1 = 0; index1 <= this.N / 2; ++index1)
        {
          for (int index2 = 0; index2 < this.N; ++index2)
          {
            int num1 = index1 * (this.N + 1) + index2;
            int num2 = index1 * (this.N + 1) + (index2 + 1);
            int num3 = (index1 + 1) * (this.N + 1) + (index2 + 1);
            int num4 = (index1 + 1) * (this.N + 1) + index2;
            this.m_triangleList.Add(num1);
            this.m_triangleList.Add(num2);
            this.m_triangleList.Add(num3);
            this.m_triangleList.Add(num1);
            this.m_triangleList.Add(num3);
            this.m_triangleList.Add(num4);
          }
        }
      }

      public void AddTrianglesToWorldVertices(
        Matrix transformMatrix,
        float radius,
        Line axisLine,
        WorldVerticesInfo worldVertices)
      {
        Matrix fromDir = Matrix.CreateFromDir(axisLine.Direction);
        Vector3 translation = transformMatrix.Translation;
        transformMatrix.Translation = Vector3.Zero;
        int num = this.m_verticeList.Count / 2;
        Vector3 vector3 = new Vector3(0.0f, 0.0f, axisLine.Length * 0.5f);
        for (int index = 0; index < num; ++index)
          worldVertices.Vertices.Add(Vector3.Transform(translation + this.m_verticeList[index] * radius - vector3, fromDir));
        for (int index = num; index < this.m_verticeList.Count; ++index)
          worldVertices.Vertices.Add(Vector3.Transform(translation + this.m_verticeList[index] * radius + vector3, fromDir));
        foreach (int triangle in this.m_triangleList)
          worldVertices.Triangles.Add(worldVertices.VerticesMaxValue + triangle);
        worldVertices.VerticesMaxValue += this.m_verticeList.Count;
      }
    }
  }
}
