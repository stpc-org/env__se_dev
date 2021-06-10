// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.RecastDetour.MyExternalPathfinding
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using ParallelTasks;
using RecastDetour;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Game.Entity;
using VRage.Groups;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
  public class MyExternalPathfinding : IMyPathfinding
  {
    private MyRecastOptions m_recastOptions;
    private readonly List<MyRecastDetourPolygon> m_polygons = new List<MyRecastDetourPolygon>();
    private Vector3D m_meshCenter;
    private Vector3D m_currentCenter;
    private int m_meshMaxSize;
    private int m_singleTileSize;
    private int m_singleTileHeight;
    private int m_tileLineCount;
    private bool m_isNavmeshInitialized;
    private MyNavmeshOBBs m_navmeshOBBs;
    private MyRDWrapper m_rdWrapper;
    private readonly List<MyRDPath> m_debugDrawPaths = new List<MyRDPath>();
    private readonly List<BoundingBoxD> m_lastGroundMeshQuery = new List<BoundingBoxD>();
    private readonly Dictionary<string, MyExternalPathfinding.GeometryCenterPair> m_cachedGeometry = new Dictionary<string, MyExternalPathfinding.GeometryCenterPair>();
    private bool m_drawMesh;
    private bool m_isNavmeshCreationRunning;
    private Vector3D? m_pathfindingDebugTarget;
    private List<MyNavmeshOBBs.OBBCoords> m_debugDrawIntersectedOBBs = new List<MyNavmeshOBBs.OBBCoords>();
    private List<MyFormatPositionColor> m_visualNavmesh = new List<MyFormatPositionColor>();
    private List<MyFormatPositionColor> m_newVisualNavmesh;
    private uint m_drawNavmeshId = uint.MaxValue;

    public bool DrawDebug { get; set; }

    public bool DrawPhysicalMesh { get; set; }

    public bool DrawNavmesh
    {
      get => this.m_drawMesh;
      set
      {
        this.m_drawMesh = value;
        if (this.m_drawMesh)
          this.DrawPersistentDebugNavmesh(true);
        else
          this.HidePersistentDebugNavmesh();
      }
    }

    public IMyPath FindPathGlobal(
      Vector3D begin,
      IMyDestinationShape end,
      MyEntity relativeEntity)
    {
      return (IMyPath) null;
    }

    public bool ReachableUnderThreshold(
      Vector3D begin,
      IMyDestinationShape end,
      float thresholdDistance)
    {
      return true;
    }

    public IMyPathfindingLog GetPathfindingLog() => (IMyPathfindingLog) null;

    public void Update()
    {
    }

    public void UnloadData()
    {
      this.HidePersistentDebugNavmesh();
      this.m_visualNavmesh.Clear();
      this.m_newVisualNavmesh?.Clear();
      this.m_newVisualNavmesh = (List<MyFormatPositionColor>) null;
    }

    public void DebugDraw()
    {
      this.DebugDrawInternal();
      int count = this.m_debugDrawPaths.Count;
      int index = 0;
      while (index < count)
      {
        MyRDPath debugDrawPath = this.m_debugDrawPaths[index];
        if (!debugDrawPath.IsValid || debugDrawPath.PathCompleted)
        {
          this.m_debugDrawPaths.RemoveAt(index);
          count = this.m_debugDrawPaths.Count;
        }
        else
        {
          debugDrawPath.DebugDraw();
          ++index;
        }
      }
    }

    public static Vector3D GetOBBCorner(
      MyOrientedBoundingBoxD obb,
      MyExternalPathfinding.OBBCorner corner)
    {
      Vector3D[] corners = new Vector3D[8];
      obb.GetCorners(corners, 0);
      return corners[(int) corner];
    }

    public static List<Vector3D> GetOBBCorners(
      MyOrientedBoundingBoxD obb,
      List<MyExternalPathfinding.OBBCorner> corners)
    {
      Vector3D[] corners1 = new Vector3D[8];
      obb.GetCorners(corners1, 0);
      List<Vector3D> vector3DList = new List<Vector3D>();
      foreach (MyExternalPathfinding.OBBCorner corner in corners)
        vector3DList.Add(corners1[(int) corner]);
      return vector3DList;
    }

    public void InitializeNavmesh(Vector3D center)
    {
      this.m_isNavmeshInitialized = true;
      float cellSize = 0.2f;
      this.m_singleTileSize = 20;
      this.m_tileLineCount = 50;
      this.m_singleTileHeight = 70;
      this.m_recastOptions = new MyRecastOptions()
      {
        cellHeight = 0.2f,
        agentHeight = 1.5f,
        agentRadius = 0.5f,
        agentMaxClimb = 0.5f,
        agentMaxSlope = 50f,
        regionMinSize = 1f,
        regionMergeSize = 10f,
        edgeMaxLen = 50f,
        edgeMaxError = 3f,
        vertsPerPoly = 6f,
        detailSampleDist = 6f,
        detailSampleMaxError = 1f,
        partitionType = 1
      };
      float num1 = (float) ((double) this.m_singleTileSize * 0.5 + (double) this.m_singleTileSize * Math.Floor((double) this.m_tileLineCount * 0.5));
      float num2 = (float) this.m_singleTileHeight * 0.5f;
      float[] bMin = new float[3]{ -num1, -num2, -num1 };
      float[] bMax = new float[3]{ num1, num2, num1 };
      this.m_rdWrapper = new MyRDWrapper();
      this.m_rdWrapper.Init(cellSize, (float) this.m_singleTileSize, bMin, bMax);
      Vector3D perpendicularVector = Vector3D.CalculatePerpendicularVector(-Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(center)));
      this.UnloadData();
      this.m_navmeshOBBs = new MyNavmeshOBBs(this.GetPlanet(center), center, perpendicularVector, this.m_tileLineCount, this.m_singleTileSize, this.m_singleTileHeight);
      this.m_meshCenter = center;
      this.m_visualNavmesh.Clear();
    }

    public void StartNavmeshTileCreation(List<MyNavmeshOBBs.OBBCoords> obbList)
    {
      if (this.m_isNavmeshCreationRunning)
        return;
      this.m_isNavmeshCreationRunning = true;
      Parallel.Start((Action) (() => this.GenerateTiles(obbList)));
    }

    private MyPlanet GetPlanet(Vector3D position)
    {
      int num = 100;
      BoundingBoxD box = new BoundingBoxD(position - (double) num * 0.5, position + (float) num * 0.5f);
      return MyGamePruningStructure.GetClosestPlanet(ref box);
    }

    private void GenerateTiles(List<MyNavmeshOBBs.OBBCoords> obbList)
    {
      MyPlanet planet = this.GetPlanet(this.m_meshCenter);
      foreach (MyNavmeshOBBs.OBBCoords obb1 in obbList)
      {
        MyOrientedBoundingBoxD obb2 = obb1.OBB;
        if (!this.m_rdWrapper.TileAlreadyGenerated(this.WorldPositionToLocalNavmeshPosition(obb2.Center, 0.0f)))
        {
          List<Vector3D> vector3DList = new List<Vector3D>();
          int num1 = vector3DList.Count / 3;
          float[] numArray1 = new float[vector3DList.Count * 3];
          int index1 = 0;
          int num2 = 0;
          for (; index1 < vector3DList.Count; ++index1)
          {
            float[] numArray2 = numArray1;
            int index2 = num2;
            int num3 = index2 + 1;
            double x = vector3DList[index1].X;
            numArray2[index2] = (float) x;
            float[] numArray3 = numArray1;
            int index3 = num3;
            int num4 = index3 + 1;
            double y = vector3DList[index1].Y;
            numArray3[index3] = (float) y;
            float[] numArray4 = numArray1;
            int index4 = num4;
            num2 = index4 + 1;
            double z = vector3DList[index1].Z;
            numArray4[index4] = (float) z;
          }
          int[] numArray5 = new int[num1 * 3];
          for (int index2 = 0; index2 < num1 * 3; ++index2)
            numArray5[index2] = index2;
          this.m_polygons.Clear();
          if (num1 > 0)
          {
            List<MyFormatPositionColor> navmesh = new List<MyFormatPositionColor>();
            this.GenerateDebugDrawPolygonNavmesh(planet, obb2, navmesh, obb1.Coords.X, obb1.Coords.Y);
            this.m_newVisualNavmesh = navmesh;
            Thread.Sleep(10);
          }
        }
      }
      this.m_isNavmeshCreationRunning = false;
    }

    private void GenerateDebugDrawPolygonNavmesh(
      MyPlanet planet,
      MyOrientedBoundingBoxD obb,
      List<MyFormatPositionColor> navmesh,
      int xCoord,
      int yCoord)
    {
      int num1 = 10;
      int num2 = 0;
      int num3 = 95;
      int num4 = 10;
      foreach (MyRecastDetourPolygon polygon in this.m_polygons)
      {
        foreach (Vector3 vertex in polygon.Vertices)
        {
          Vector3D worldPosition = this.LocalNavmeshPositionToWorldPosition(obb, (Vector3D) vertex, this.m_meshCenter, Vector3D.Zero);
          MyFormatPositionColor formatPositionColor = new MyFormatPositionColor()
          {
            Position = (Vector3) worldPosition,
            Color = new Color(0, num1 + num2, 0)
          };
          navmesh.Add(formatPositionColor);
        }
        num2 = (num2 + num4) % num3;
      }
    }

    private static MatrixD LocalNavmeshPositionToWorldPositionTransform(
      MyOrientedBoundingBoxD obb,
      Vector3D center)
    {
      Vector3D v = -Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(center));
      return MatrixD.CreateFromQuaternion(Quaternion.CreateFromForwardUp((Vector3) Vector3D.CalculatePerpendicularVector(v), (Vector3) v));
    }

    private Vector3D LocalNavmeshPositionToWorldPosition(
      MyOrientedBoundingBoxD obb,
      Vector3D position,
      Vector3D center,
      Vector3D heightIncrease)
    {
      MatrixD positionTransform = MyExternalPathfinding.LocalNavmeshPositionToWorldPositionTransform(obb, center);
      return Vector3D.Transform(position, positionTransform) + this.m_meshCenter;
    }

    public void SetTarget(Vector3D? target) => this.m_pathfindingDebugTarget = target;

    private Vector3D WorldPositionToLocalNavmeshPosition(
      Vector3D position,
      float heightIncrease)
    {
      MyOrientedBoundingBoxD? obb = this.m_navmeshOBBs.GetOBB(position);
      if (obb.HasValue)
      {
        MyOrientedBoundingBoxD orientedBoundingBoxD = obb.Value;
      }
      else
      {
        Vector3D meshCenter = this.m_meshCenter;
      }
      Vector3D v = -Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(this.m_meshCenter));
      MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(Quaternion.Inverse(Quaternion.CreateFromForwardUp((Vector3) Vector3D.CalculatePerpendicularVector(v), (Vector3) v)));
      return Vector3D.Transform(position - this.m_meshCenter + (double) heightIncrease * v, fromQuaternion);
    }

    private Vector3D LocalPositionToWorldPosition(Vector3D position)
    {
      Vector3D vector3D1 = position;
      if (this.m_navmeshOBBs != null)
        vector3D1 = this.m_meshCenter;
      Vector3D vector3D2 = -Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(vector3D1));
      return this.LocalNavmeshPositionToWorldPosition(this.m_navmeshOBBs.CenterOBB, position, vector3D1, 0.5 * vector3D2);
    }

    public List<Vector3D> GetPathPoints(
      Vector3D initialPosition,
      Vector3D targetPosition)
    {
      List<Vector3D> vector3DList = new List<Vector3D>();
      if (this.m_isNavmeshCreationRunning)
        return vector3DList;
      if (!this.m_isNavmeshInitialized)
        this.InitializeNavmesh(initialPosition);
      List<Vector3> path = this.m_rdWrapper.GetPath((Vector3) this.WorldPositionToLocalNavmeshPosition(initialPosition, 0.5f), (Vector3) this.WorldPositionToLocalNavmeshPosition(targetPosition, 0.5f));
      if (path.Count == 0)
      {
        List<MyNavmeshOBBs.OBBCoords> intersectedObb = this.m_navmeshOBBs.GetIntersectedOBB(new LineD(initialPosition, targetPosition));
        this.StartNavmeshTileCreation(intersectedObb);
        this.m_debugDrawIntersectedOBBs = intersectedObb;
      }
      else
      {
        foreach (Vector3 vector3 in path)
          vector3DList.Add(this.LocalPositionToWorldPosition((Vector3D) vector3));
      }
      return vector3DList;
    }

    public void DrawGeometry(
      HkGeometry geometry,
      MatrixD worldMatrix,
      Color color,
      bool depthRead = false,
      bool shaded = false)
    {
      MyRenderMessageDebugDrawTriangles debugDrawTriangles = MyRenderProxy.PrepareDebugDrawTriangles();
      for (int triangleIndex = 0; triangleIndex < geometry.TriangleCount; ++triangleIndex)
      {
        int i0;
        int i1;
        int i2;
        geometry.GetTriangle(triangleIndex, out i0, out i1, out i2, out int _);
        debugDrawTriangles.AddIndex(i0);
        debugDrawTriangles.AddIndex(i1);
        debugDrawTriangles.AddIndex(i2);
      }
      for (int vertexIndex = 0; vertexIndex < geometry.VertexCount; ++vertexIndex)
        debugDrawTriangles.AddVertex((Vector3D) geometry.GetVertex(vertexIndex));
    }

    private void DebugDrawShape(string blockName, HkShape shape, MatrixD worldMatrix)
    {
      float num1 = 1.05f;
      float num2 = 0.02f;
      if (MyPerGameSettings.Game == GameEnum.SE_GAME)
        num2 = 0.1f;
      switch (shape.ShapeType)
      {
        case HkShapeType.Box:
          MyRenderProxy.DebugDrawOBB(MatrixD.CreateScale((Vector3D) (((HkBoxShape) shape).HalfExtents * 2f + new Vector3(num2))) * worldMatrix, Color.Red, 0.0f, true, false);
          break;
        case HkShapeType.ConvexVertices:
          HkConvexVerticesShape convexVerticesShape = (HkConvexVerticesShape) shape;
          MyExternalPathfinding.GeometryCenterPair geometryCenterPair;
          if (!this.m_cachedGeometry.TryGetValue(blockName, out geometryCenterPair))
          {
            HkGeometry geometry = new HkGeometry();
            Vector3 center;
            convexVerticesShape.GetGeometry(geometry, out center);
            geometryCenterPair = new MyExternalPathfinding.GeometryCenterPair()
            {
              Geometry = geometry,
              Center = (Vector3D) center
            };
            if (!string.IsNullOrEmpty(blockName))
              this.m_cachedGeometry.Add(blockName, geometryCenterPair);
          }
          Vector3D vector3D = Vector3D.Transform(geometryCenterPair.Center, worldMatrix.GetOrientation());
          MatrixD matrixD = worldMatrix;
          MatrixD worldMatrix1 = MatrixD.CreateScale((double) num1) * matrixD;
          worldMatrix1.Translation -= vector3D * ((double) num1 - 1.0);
          this.DrawGeometry(geometryCenterPair.Geometry, worldMatrix1, Color.Olive);
          break;
        case HkShapeType.List:
          HkShapeContainerIterator iterator = ((HkListShape) shape).GetIterator();
          int num3 = 0;
          while (iterator.IsValid)
          {
            this.DebugDrawShape(blockName + (object) num3++, iterator.CurrentValue, worldMatrix);
            iterator.Next();
          }
          break;
        case HkShapeType.Mopp:
          HkMoppBvTreeShape hkMoppBvTreeShape = (HkMoppBvTreeShape) shape;
          this.DebugDrawShape(blockName, (HkShape) hkMoppBvTreeShape.ShapeCollection, worldMatrix);
          break;
        case HkShapeType.ConvexTranslate:
          HkConvexTranslateShape convexTranslateShape = (HkConvexTranslateShape) shape;
          this.DebugDrawShape(blockName, (HkShape) convexTranslateShape.ChildShape, Matrix.CreateTranslation(convexTranslateShape.Translation) * worldMatrix);
          break;
        case HkShapeType.ConvexTransform:
          HkConvexTransformShape convexTransformShape = (HkConvexTransformShape) shape;
          this.DebugDrawShape(blockName, (HkShape) convexTransformShape.ChildShape, convexTransformShape.Transform * worldMatrix);
          break;
      }
    }

    public void DebugDrawPhysicalShapes()
    {
      MyCubeGrid targetGrid = MyCubeGrid.GetTargetGrid();
      if (targetGrid == null)
        return;
      List<MyCubeGrid> myCubeGridList = new List<MyCubeGrid>();
      foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in MyCubeGridGroups.Static.Logical.GetGroup(targetGrid).Nodes)
        myCubeGridList.Add(node.NodeData);
      MatrixD.Invert(myCubeGridList[0].WorldMatrix);
      foreach (MyCubeGrid grid in myCubeGridList)
      {
        if (MyPerGameSettings.Game == GameEnum.SE_GAME)
        {
          HkGridShape hkGridShape = new HkGridShape(grid.GridSize, HkReferencePolicy.None);
          MyCubeBlockCollector cubeBlockCollector = new MyCubeBlockCollector();
          MyVoxelSegmentation segmenter = new MyVoxelSegmentation();
          Dictionary<Vector3I, HkMassElement> dictionary = new Dictionary<Vector3I, HkMassElement>();
          cubeBlockCollector.Collect(grid, segmenter, MyVoxelSegmentationType.Simple, (IDictionary<Vector3I, HkMassElement>) dictionary);
          foreach (HkShape shape in cubeBlockCollector.Shapes)
            this.DebugDrawShape("", shape, grid.WorldMatrix);
        }
        else
        {
          foreach (MySlimBlock block1 in grid.GetBlocks())
          {
            if (block1.FatBlock != null)
            {
              if (block1.FatBlock is MyCompoundCubeBlock)
              {
                foreach (MySlimBlock block2 in (block1.FatBlock as MyCompoundCubeBlock).GetBlocks())
                {
                  HkShape havokCollisionShape = block2.FatBlock.ModelCollision.HavokCollisionShapes[0];
                  this.DebugDrawShape(block2.BlockDefinition.Id.SubtypeName, havokCollisionShape, block2.FatBlock.PositionComp.WorldMatrixRef);
                }
              }
              else if (block1.FatBlock.ModelCollision.HavokCollisionShapes != null)
              {
                foreach (HkShape havokCollisionShape in block1.FatBlock.ModelCollision.HavokCollisionShapes)
                  this.DebugDrawShape(block1.BlockDefinition.Id.SubtypeName, havokCollisionShape, block1.FatBlock.PositionComp.WorldMatrixRef);
              }
            }
          }
        }
      }
    }

    private void DrawPersistentDebugNavmesh(bool force)
    {
      if (this.m_newVisualNavmesh != null)
      {
        this.m_visualNavmesh = this.m_newVisualNavmesh;
        this.m_newVisualNavmesh = (List<MyFormatPositionColor>) null;
        force = true;
      }
      if (!force)
        return;
      if (this.m_visualNavmesh.Count > 0)
      {
        if (this.m_drawNavmeshId == uint.MaxValue)
          this.m_drawNavmeshId = MyRenderProxy.DebugDrawMesh(this.m_visualNavmesh, MatrixD.Identity, true, true);
        else
          MyRenderProxy.DebugDrawUpdateMesh(this.m_drawNavmeshId, this.m_visualNavmesh, MatrixD.Identity, true, true);
      }
      else
        this.HidePersistentDebugNavmesh();
    }

    private void HidePersistentDebugNavmesh()
    {
      if (this.m_drawNavmeshId == uint.MaxValue)
        return;
      MyRenderProxy.RemoveRenderObject(this.m_drawNavmeshId, MyRenderProxy.ObjectType.DebugDrawMesh);
      this.m_drawNavmeshId = uint.MaxValue;
    }

    private static unsafe Vector3* GetMiddleOBBPoints(
      MyOrientedBoundingBoxD obb,
      ref Vector3* points)
    {
      Vector3 vector3_1 = obb.Orientation.Right * (float) obb.HalfExtent.X;
      Vector3 vector3_2 = obb.Orientation.Forward * (float) obb.HalfExtent.Z;
      *points = (Vector3) (obb.Center - vector3_1 - vector3_2);
      points[1] = (Vector3) (obb.Center + vector3_1 - vector3_2);
      points[2] = (Vector3) (obb.Center + vector3_1 + vector3_2);
      points[3] = (Vector3) (obb.Center - vector3_1 + vector3_2);
      return points;
    }

    private static unsafe bool DrawTerrainLimits(MyPlanet planet, MyOrientedBoundingBoxD obb)
    {
      int pointCount = 4;
      Vector3* points = stackalloc Vector3[4];
      MyExternalPathfinding.GetMiddleOBBPoints(obb, ref points);
      float minHeight;
      float maxHeight;
      planet.Provider.Shape.GetBounds(points, pointCount, out minHeight, out maxHeight);
      if (!minHeight.IsValid() || !maxHeight.IsValid())
        return false;
      Vector3D vector3D1 = (Vector3D) (obb.Orientation.Up * minHeight);
      Vector3D vector3D2 = (Vector3D) (obb.Orientation.Up * maxHeight);
      obb.Center = vector3D1 + (vector3D2 - vector3D1) * 0.5;
      obb.HalfExtent.Y = ((double) maxHeight - (double) minHeight) * 0.5;
      MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(obb.Center, obb.HalfExtent, obb.Orientation), Color.Blue, 0.0f, true, false);
      return true;
    }

    private unsafe void DebugDrawInternal()
    {
      this.m_navmeshOBBs?.DebugDraw();
      if (this.DrawNavmesh)
        this.DrawPersistentDebugNavmesh(false);
      if (this.DrawPhysicalMesh)
        this.DebugDrawPhysicalShapes();
      Vector3D position1 = MySession.Static.ControlledEntity.ControllerInfo.Controller.Player.GetPosition();
      position1.Y += 2.40000009536743;
      MyRenderProxy.DebugDrawText3D(position1, string.Format("X: {0}\nY: {1}\nZ: {2}", (object) Math.Round(position1.X, 2), (object) Math.Round(position1.Y, 2), (object) Math.Round(position1.Z, 2)), Color.Red, 1f, true);
      if (this.m_lastGroundMeshQuery.Count > 0)
      {
        MyRenderProxy.DebugDrawSphere(this.m_lastGroundMeshQuery[0].Center, 1f, Color.Yellow);
        foreach (BoundingBoxD boundingBoxD in this.m_lastGroundMeshQuery)
          MyRenderProxy.DebugDrawOBB(boundingBoxD.Matrix, Color.Yellow, 0.0f, true, false);
        if (this.m_navmeshOBBs != null)
        {
          foreach (MyNavmeshOBBs.OBBCoords drawIntersectedObB in this.m_debugDrawIntersectedOBBs)
            MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(drawIntersectedObB.OBB.Center, (Vector3D) new Vector3(drawIntersectedObB.OBB.HalfExtent.X, drawIntersectedObB.OBB.HalfExtent.Y / 2.0, drawIntersectedObB.OBB.HalfExtent.Z), drawIntersectedObB.OBB.Orientation), Color.White, 0.0f, true, false);
          MyOrientedBoundingBoxD obb = this.m_navmeshOBBs.GetOBB(0, 0).Value;
          MyPlanet planet = this.GetPlanet(obb.Center);
          Vector3* points = stackalloc Vector3[4];
          MyExternalPathfinding.GetMiddleOBBPoints(obb, ref points);
          float minHeight;
          float maxHeight;
          planet.Provider.Shape.GetBounds(points, 4, out minHeight, out maxHeight);
          if (minHeight.IsValid() && maxHeight.IsValid())
          {
            Vector3D position2 = (Vector3D) (obb.Orientation.Up * minHeight);
            Vector3D position3 = (Vector3D) (obb.Orientation.Up * maxHeight);
            Color blue = Color.Blue;
            MyRenderProxy.DebugDrawSphere(position2, 1f, blue, 0.0f);
            MyRenderProxy.DebugDrawSphere(position3, 1f, Color.Blue, 0.0f);
          }
          MyExternalPathfinding.DrawTerrainLimits(planet, obb);
        }
        MyRenderProxy.DebugDrawSphere(this.m_meshCenter, 2f, Color.Red, 0.0f);
      }
      if (this.m_polygons == null || !this.m_pathfindingDebugTarget.HasValue)
        return;
      MyRenderProxy.DebugDrawSphere(this.m_pathfindingDebugTarget.Value + 1.5 * -Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(this.m_pathfindingDebugTarget.Value)), 0.2f, Color.Red, 0.0f);
    }

    private class GeometryCenterPair
    {
      public HkGeometry Geometry { get; set; }

      public Vector3D Center { get; set; }
    }

    public enum OBBCorner
    {
      UpperFrontLeft,
      UpperBackLeft,
      LowerBackLeft,
      LowerFrontLeft,
      UpperFrontRight,
      UpperBackRight,
      LowerBackRight,
      LowerFrontRight,
    }

    private struct Vertex
    {
      public Vector3D pos;
      public Color color;
    }
  }
}
