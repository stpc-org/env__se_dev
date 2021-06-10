// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.RecastDetour.MyNavmeshManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using RecastDetour;
using Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Library.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
  public class MyNavmeshManager
  {
    private static readonly MyRandom m_myRandom = new MyRandom(0);
    public Color m_debugColor;
    private const float RECAST_CELL_SIZE = 0.2f;
    private const int MAX_TILES_TO_GENERATE = 7;
    private const int MAX_TICKS_WITHOUT_HEARTBEAT = 5000;
    private int m_ticksAfterLastPathRequest;
    private readonly int m_tileSize;
    private readonly int m_tileHeight;
    private readonly int m_tileLineCount;
    private readonly float m_border;
    private readonly float m_heightCoordTransformationIncrease;
    private bool m_allTilesGenerated;
    private bool m_isManagerAlive = true;
    private bool m_isTileBeingGenerated;
    private MyNavmeshOBBs m_navmeshOBBs;
    private MyRecastOptions m_recastOptions;
    private MyNavigationInputMesh m_navInputMesh;
    private HashSet<MyNavmeshOBBs.OBBCoords> m_obbCoordsToUpdate = new HashSet<MyNavmeshOBBs.OBBCoords>((IEqualityComparer<MyNavmeshOBBs.OBBCoords>) new MyNavmeshManager.OBBCoordComparer());
    private HashSet<Vector2I> m_coordsAlreadyGenerated = new HashSet<Vector2I>((IEqualityComparer<Vector2I>) new MyNavmeshManager.CoordComparer());
    private Dictionary<Vector2I, List<MyFormatPositionColor>> m_obbCoordsPolygons = new Dictionary<Vector2I, List<MyFormatPositionColor>>();
    private Dictionary<Vector2I, List<MyFormatPositionColor>> m_newObbCoordsPolygons = new Dictionary<Vector2I, List<MyFormatPositionColor>>();
    private MyRDWrapper m_rdWrapper;
    private MyOrientedBoundingBoxD m_extendedBaseOBB;
    private readonly List<MyVoxelMap> m_tmpTrackedVoxelMaps = new List<MyVoxelMap>();
    private readonly Dictionary<long, MyVoxelMap> m_trackedVoxelMaps = new Dictionary<long, MyVoxelMap>();
    private readonly int?[][] m_debugTileSize;
    private bool m_drawMesh;
    private bool m_updateDrawMesh;
    private List<BoundingBoxD> m_groundCaptureAABBs = new List<BoundingBoxD>();
    private int m_remainingTilesToGenerate;
    private readonly object m_tileGenerationLock = new object();
    private uint m_drawNavmeshId = uint.MaxValue;

    public Vector3D Center => this.m_navmeshOBBs.CenterOBB.Center;

    public MyOrientedBoundingBoxD CenterOBB => this.m_navmeshOBBs.CenterOBB;

    public MyPlanet Planet { get; }

    private bool TilesAreAwaitingGeneration => this.m_obbCoordsToUpdate.Count > 0;

    public bool DrawNavmesh
    {
      get => this.m_drawMesh;
      set
      {
        this.m_drawMesh = value;
        if (this.m_drawMesh)
          this.DrawPersistentDebugNavmesh();
        else
          this.HidePersistentDebugNavmesh();
      }
    }

    public MyNavmeshManager(
      MyRDPathfinding rdPathfinding,
      Vector3D center,
      Vector3D forwardDirection,
      int tileSize,
      int tileHeight,
      int tileLineCount,
      MyRecastOptions recastOptions)
    {
      Vector3 vector3_1 = new Vector3(MyNavmeshManager.m_myRandom.NextFloat(), MyNavmeshManager.m_myRandom.NextFloat(), MyNavmeshManager.m_myRandom.NextFloat());
      Vector3 vector3_2 = vector3_1 - Math.Min(vector3_1.X, Math.Min(vector3_1.Y, vector3_1.Z));
      this.m_debugColor = new Color(vector3_2 / Math.Max(vector3_2.X, Math.Max(vector3_2.Y, vector3_2.Z)));
      this.m_tileSize = tileSize;
      this.m_tileHeight = tileHeight;
      this.m_tileLineCount = tileLineCount;
      this.Planet = this.GetPlanet(center);
      this.m_heightCoordTransformationIncrease = 0.5f;
      this.m_recastOptions = recastOptions;
      float num1 = (float) ((double) this.m_tileSize * 0.5 + (double) this.m_tileSize * Math.Floor((double) this.m_tileLineCount * 0.5));
      float num2 = (float) this.m_tileHeight * 0.5f;
      this.m_border = this.m_recastOptions.agentRadius + 0.6f;
      float[] bMin = new float[3]{ -num1, -num2, -num1 };
      float[] bMax = new float[3]{ num1, num2, num1 };
      this.m_rdWrapper = new MyRDWrapper();
      this.m_rdWrapper.Init(0.2f, (float) this.m_tileSize, bMin, bMax);
      Vector3D perpendicularVector = Vector3D.CalculatePerpendicularVector(-Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(center)));
      this.m_navmeshOBBs = new MyNavmeshOBBs(this.Planet, center, perpendicularVector, this.m_tileLineCount, this.m_tileSize, this.m_tileHeight);
      this.m_debugTileSize = new int?[this.m_tileLineCount][];
      for (int index = 0; index < this.m_tileLineCount; ++index)
        this.m_debugTileSize[index] = new int?[this.m_tileLineCount];
      this.m_extendedBaseOBB = new MyOrientedBoundingBoxD(this.m_navmeshOBBs.BaseOBB.Center, new Vector3D(this.m_navmeshOBBs.BaseOBB.HalfExtent.X, (double) this.m_tileHeight, this.m_navmeshOBBs.BaseOBB.HalfExtent.Z), this.m_navmeshOBBs.BaseOBB.Orientation);
      this.m_navInputMesh = new MyNavigationInputMesh(rdPathfinding, this.Planet, center);
    }

    public bool InvalidateArea(BoundingBoxD areaAABB)
    {
      bool flag1 = false;
      if (!this.Intersects(areaAABB))
        return false;
      bool flag2 = false;
      for (int index1 = 0; index1 < this.m_tileLineCount; ++index1)
      {
        bool flag3 = false;
        bool flag4 = false;
        for (int index2 = 0; index2 < this.m_tileLineCount; ++index2)
        {
          if (this.m_navmeshOBBs.GetOBB(index1, index2).Value.Intersects(ref areaAABB))
          {
            Vector2I key = new Vector2I(index1, index2);
            flag3 = flag4 = true;
            if (this.m_coordsAlreadyGenerated.Remove(key))
            {
              flag1 = true;
              this.m_allTilesGenerated = false;
              this.m_newObbCoordsPolygons[key] = (List<MyFormatPositionColor>) null;
              this.m_navInputMesh.InvalidateCache(areaAABB);
            }
          }
          else if (flag4)
            break;
        }
        if (flag3)
          flag2 = true;
        else if (flag2)
          break;
      }
      if (flag1)
        this.m_updateDrawMesh = true;
      return flag1;
    }

    public bool ContainsPosition(Vector3D position)
    {
      LineD line = new LineD(this.Planet.PositionComp.WorldAABB.Center, position);
      return this.m_navmeshOBBs.BaseOBB.Intersects(ref line).HasValue;
    }

    public void TilesToGenerate(Vector3D initialPosition, Vector3D targetPosition) => this.TilesToGenerateInternal(initialPosition, targetPosition, out int _);

    public bool GetPathPoints(
      Vector3D initialPosition,
      Vector3D targetPosition,
      out List<Vector3D> path,
      out bool noTilesToGenerate)
    {
      this.Heartbeat();
      bool flag1 = false;
      noTilesToGenerate = true;
      path = new List<Vector3D>();
      if (!this.m_allTilesGenerated)
      {
        int tilesAddedToGeneration;
        this.TilesToGenerateInternal(initialPosition, targetPosition, out tilesAddedToGeneration);
        noTilesToGenerate = tilesAddedToGeneration == 0;
      }
      Vector3D localNavmeshPosition1 = this.WorldPositionToLocalNavmeshPosition(initialPosition, this.m_heightCoordTransformationIncrease);
      Vector3D position1 = targetPosition;
      bool flag2 = !this.ContainsPosition(targetPosition);
      if (flag2)
        position1 = this.GetPositionAtDistanceFromPlanetCenter(this.GetBorderPoint(initialPosition, targetPosition), (initialPosition - this.Planet.PositionComp.WorldAABB.Center).Length());
      Vector3D localNavmeshPosition2 = this.WorldPositionToLocalNavmeshPosition(position1, this.m_heightCoordTransformationIncrease);
      List<Vector3> path1 = this.m_rdWrapper.GetPath((Vector3) localNavmeshPosition1, (Vector3) localNavmeshPosition2);
      if (path1.Count > 0)
      {
        foreach (Vector3 vector3 in path1)
          path.Add(this.LocalPositionToWorldPosition((Vector3D) vector3));
        Vector3D position2 = path.Last<Vector3D>();
        int num = (position1 - position2).Length() <= 0.25 ? 1 : 0;
        flag1 = num != 0 && !flag2;
        if (num != 0)
        {
          if (flag2)
          {
            path.RemoveAt(path.Count - 1);
            path.Add(targetPosition);
          }
          else if (noTilesToGenerate && MyNavmeshManager.GetPathDistance(path) > 3.0 * Vector3D.Distance(initialPosition, targetPosition))
            noTilesToGenerate = !this.TryGenerateTilesAroundPosition(initialPosition);
        }
        if (((num != 0 ? 0 : (!this.m_allTilesGenerated ? 1 : 0)) & (noTilesToGenerate ? 1 : 0)) != 0)
          noTilesToGenerate = !this.TryGenerateTilesAroundPosition(position2);
      }
      noTilesToGenerate = noTilesToGenerate && this.m_remainingTilesToGenerate == 0;
      return flag1;
    }

    public bool Update()
    {
      if (!this.CheckManagerHeartbeat())
        return false;
      this.GenerateNextQueuedTile();
      if (this.m_updateDrawMesh)
      {
        this.m_updateDrawMesh = false;
        this.UpdatePersistentDebugNavmesh();
      }
      return true;
    }

    public void UnloadData()
    {
      this.m_isManagerAlive = false;
      foreach (KeyValuePair<long, MyVoxelMap> trackedVoxelMap in this.m_trackedVoxelMaps)
        trackedVoxelMap.Value.RangeChanged -= new MyVoxelBase.StorageChanged(this.VoxelMapRangeChanged);
      this.m_trackedVoxelMaps.Clear();
      this.m_rdWrapper.Clear();
      this.m_rdWrapper = (MyRDWrapper) null;
      this.m_navInputMesh.Clear();
      this.m_navInputMesh = (MyNavigationInputMesh) null;
      this.m_navmeshOBBs.Clear();
      this.m_navmeshOBBs = (MyNavmeshOBBs) null;
      this.m_obbCoordsToUpdate.Clear();
      this.m_obbCoordsToUpdate = (HashSet<MyNavmeshOBBs.OBBCoords>) null;
      this.m_coordsAlreadyGenerated.Clear();
      this.m_coordsAlreadyGenerated = (HashSet<Vector2I>) null;
      this.m_obbCoordsPolygons.Clear();
      this.m_obbCoordsPolygons = (Dictionary<Vector2I, List<MyFormatPositionColor>>) null;
      this.m_newObbCoordsPolygons.Clear();
      this.m_newObbCoordsPolygons = (Dictionary<Vector2I, List<MyFormatPositionColor>>) null;
    }

    public void DebugDraw()
    {
      this.m_navmeshOBBs.DebugDraw();
      this.m_navInputMesh.DebugDraw();
      MyRenderProxy.DebugDrawOBB(this.m_extendedBaseOBB, Color.White, 0.0f, true, false);
      for (int index = 0; index < this.m_groundCaptureAABBs.Count; ++index)
        MyRenderProxy.DebugDrawAABB(this.m_groundCaptureAABBs[index], Color.Yellow);
    }

    private Vector3D LocalPositionToWorldPosition(Vector3D position)
    {
      Vector3D vector3D1 = position;
      if (this.m_navmeshOBBs != null)
        vector3D1 = this.Center;
      Vector3D vector3D2 = -Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(vector3D1));
      return this.LocalNavmeshPositionToWorldPosition(this.m_navmeshOBBs.CenterOBB, position, vector3D1, -(double) this.m_heightCoordTransformationIncrease * vector3D2);
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
      MatrixD positionTransform = MyNavmeshManager.LocalNavmeshPositionToWorldPositionTransform(obb, center);
      return Vector3D.Transform(position, positionTransform) + this.Center + heightIncrease;
    }

    private Vector3D WorldPositionToLocalNavmeshPosition(
      Vector3D position,
      float heightIncrease)
    {
      Vector3D v = -Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(this.Center));
      MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(Quaternion.Inverse(Quaternion.CreateFromForwardUp((Vector3) Vector3D.CalculatePerpendicularVector(v), (Vector3) v)));
      return Vector3D.Transform(position - this.Center + (double) heightIncrease * v, fromQuaternion);
    }

    private Vector3D GetBorderPoint(Vector3D startingPoint, Vector3D outsidePoint)
    {
      LineD line = new LineD(startingPoint, outsidePoint);
      double? nullable = this.m_extendedBaseOBB.Intersects(ref line);
      if (!nullable.HasValue)
        return outsidePoint;
      line.Length = nullable.Value - 1.0;
      line.To = startingPoint + line.Direction * nullable.Value;
      return line.To;
    }

    private void Heartbeat() => this.m_ticksAfterLastPathRequest = 0;

    private bool CheckManagerHeartbeat()
    {
      if (!this.m_isManagerAlive)
        return false;
      ++this.m_ticksAfterLastPathRequest;
      this.m_isManagerAlive = this.m_ticksAfterLastPathRequest < 5000;
      if (!this.m_isManagerAlive)
        this.UnloadData();
      return this.m_isManagerAlive;
    }

    private static double GetPathDistance(List<Vector3D> path)
    {
      double num = 0.0;
      for (int index = 0; index < path.Count - 1; ++index)
        num += Vector3D.Distance(path[index], path[index + 1]);
      return num;
    }

    private bool Intersects(BoundingBoxD obb) => this.m_extendedBaseOBB.Intersects(ref obb);

    private bool TryGenerateTilesAroundPosition(Vector3D position)
    {
      MyNavmeshOBBs.OBBCoords? obbCoord = this.m_navmeshOBBs.GetOBBCoord(position);
      return obbCoord.HasValue && this.TryGenerateNeighbourTiles(obbCoord.Value);
    }

    private bool TryGenerateNeighbourTiles(MyNavmeshOBBs.OBBCoords obbCoord, int radius = 1)
    {
      int num1 = 0;
      bool flag = false;
      for (int index1 = -radius; index1 <= radius; ++index1)
      {
        int num2 = index1 == -radius || index1 == radius ? 1 : 2 * radius;
        for (int index2 = -radius; index2 <= radius; index2 += num2)
        {
          Vector2I vector2I;
          vector2I.X = obbCoord.Coords.X + index2;
          vector2I.Y = obbCoord.Coords.Y + index1;
          MyNavmeshOBBs.OBBCoords? obbCoord1 = this.m_navmeshOBBs.GetOBBCoord(vector2I.X, vector2I.Y);
          if (obbCoord1.HasValue)
          {
            flag = true;
            if (this.AddTileToGeneration(obbCoord1.Value))
            {
              ++num1;
              if (num1 >= 7)
                return true;
            }
          }
        }
      }
      if (num1 > 0)
        return true;
      this.m_allTilesGenerated = !flag;
      return !this.m_allTilesGenerated && this.TryGenerateNeighbourTiles(obbCoord, radius + 1);
    }

    private List<MyNavmeshOBBs.OBBCoords> TilesToGenerateInternal(
      Vector3D initialPosition,
      Vector3D targetPosition,
      out int tilesAddedToGeneration)
    {
      tilesAddedToGeneration = 0;
      List<MyNavmeshOBBs.OBBCoords> intersectedObb = this.m_navmeshOBBs.GetIntersectedOBB(new LineD(initialPosition, targetPosition));
      foreach (MyNavmeshOBBs.OBBCoords obbCoord in intersectedObb)
      {
        if (this.AddTileToGeneration(obbCoord))
        {
          ++tilesAddedToGeneration;
          if (tilesAddedToGeneration == 7)
            break;
        }
      }
      return intersectedObb;
    }

    private bool AddTileToGeneration(MyNavmeshOBBs.OBBCoords obbCoord)
    {
      if (this.m_coordsAlreadyGenerated.Contains(obbCoord.Coords) || !this.m_obbCoordsToUpdate.Add(obbCoord))
        return false;
      lock (this.m_tileGenerationLock)
        ++this.m_remainingTilesToGenerate;
      return true;
    }

    private Vector3D GetPositionAtDistanceFromPlanetCenter(
      Vector3D position,
      double distance)
    {
      (position - this.Planet.PositionComp.WorldAABB.Center).Length();
      return -Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(position)) * distance + this.Planet.PositionComp.WorldAABB.Center;
    }

    private MyPlanet GetPlanet(Vector3D position)
    {
      int num = 200;
      BoundingBoxD box = new BoundingBoxD(position - (double) num * 0.5, position + (float) num * 0.5f);
      return MyGamePruningStructure.GetClosestPlanet(ref box);
    }

    private void GenerateNextQueuedTile()
    {
      if (!this.TilesAreAwaitingGeneration || this.m_isTileBeingGenerated)
        return;
      this.m_isTileBeingGenerated = true;
      MyNavmeshOBBs.OBBCoords obb = this.m_obbCoordsToUpdate.First<MyNavmeshOBBs.OBBCoords>();
      this.m_obbCoordsToUpdate.Remove(obb);
      this.m_coordsAlreadyGenerated.Add(obb.Coords);
      List<BoundingBoxD> bbs;
      WorldVerticesInfo worldVertices;
      MyShapesInfo shapesInfo;
      this.m_navInputMesh.CreateWorldVerticesAndShapes(this.m_border, this.Center, obb.OBB, this.m_tmpTrackedVoxelMaps, out bbs, out worldVertices, out shapesInfo);
      Parallel.Start((Action) (() => this.GenerateTile(obb, bbs, worldVertices, shapesInfo)), (Action) (() =>
      {
        lock (this.m_tileGenerationLock)
        {
          --this.m_remainingTilesToGenerate;
          this.m_isTileBeingGenerated = false;
        }
      }));
    }

    private unsafe void GenerateTile(
      MyNavmeshOBBs.OBBCoords obbCoord,
      List<BoundingBoxD> bbs,
      WorldVerticesInfo worldVertices,
      MyShapesInfo shapesInfo)
    {
      Vector3 localNavmeshPosition = (Vector3) this.WorldPositionToLocalNavmeshPosition(obbCoord.OBB.Center, 0.0f);
      this.m_groundCaptureAABBs = bbs;
      foreach (MyVoxelMap tmpTrackedVoxelMap in this.m_tmpTrackedVoxelMaps)
      {
        if (!this.m_trackedVoxelMaps.ContainsKey(tmpTrackedVoxelMap.EntityId))
        {
          tmpTrackedVoxelMap.RangeChanged += new MyVoxelBase.StorageChanged(this.VoxelMapRangeChanged);
          this.m_trackedVoxelMaps.Add(tmpTrackedVoxelMap.EntityId, tmpTrackedVoxelMap);
        }
      }
      this.m_tmpTrackedVoxelMaps.Clear();
      worldVertices = this.m_navInputMesh.AddGroundToWorldVertices(this.m_border, this.Center, obbCoord.OBB, bbs, worldVertices);
      worldVertices = this.m_navInputMesh.AddPhysicalShapeVertices(shapesInfo, worldVertices);
      if (worldVertices.Triangles.Count > 0)
      {
        List<MyRecastDetourPolygon> polygons = (List<MyRecastDetourPolygon>) null;
        fixed (Vector3* vector3Ptr = worldVertices.Vertices.ToArray())
          fixed (int* triangles = worldVertices.Triangles.ToArray())
            this.m_rdWrapper.CreateNavmeshTile((Vector3D) localNavmeshPosition, ref this.m_recastOptions, ref polygons, obbCoord.Coords.X, obbCoord.Coords.Y, 0, (float*) vector3Ptr, worldVertices.Vertices.Count, triangles, worldVertices.Triangles.Count / 3);
        this.GenerateDebugDrawPolygonNavmesh(this.Planet, (IReadOnlyCollection<MyRecastDetourPolygon>) polygons, this.m_navmeshOBBs.CenterOBB, obbCoord.Coords);
        this.GenerateDebugTileDataSize(localNavmeshPosition, obbCoord.Coords.X, obbCoord.Coords.Y);
        if (polygons == null)
          return;
        polygons.Clear();
        this.m_updateDrawMesh = true;
      }
      else
        this.m_newObbCoordsPolygons[obbCoord.Coords] = (List<MyFormatPositionColor>) null;
    }

    private void VoxelMapRangeChanged(
      MyVoxelBase storage,
      Vector3I minVoxelChanged,
      Vector3I maxVoxelChanged,
      MyStorageDataTypeFlags changedData)
    {
      this.InvalidateArea(MyRDPathfinding.GetVoxelAreaAABB(storage, minVoxelChanged, maxVoxelChanged));
    }

    private void GenerateDebugTileDataSize(Vector3 center, int xCoord, int yCoord)
    {
      int tileDataSize = this.m_rdWrapper.GetTileDataSize((Vector3D) center, 0);
      this.m_debugTileSize[xCoord][yCoord] = new int?(tileDataSize);
    }

    private void GenerateDebugDrawPolygonNavmesh(
      MyPlanet planet,
      IReadOnlyCollection<MyRecastDetourPolygon> polygons,
      MyOrientedBoundingBoxD centerOBB,
      Vector2I coords)
    {
      if (polygons == null)
        return;
      List<MyFormatPositionColor> formatPositionColorList = new List<MyFormatPositionColor>();
      int num = 0;
      foreach (MyRecastDetourPolygon polygon in (IEnumerable<MyRecastDetourPolygon>) polygons)
      {
        foreach (Vector3 vertex in polygon.Vertices)
        {
          MyFormatPositionColor formatPositionColor = new MyFormatPositionColor()
          {
            Position = (Vector3) this.LocalNavmeshPositionToWorldPosition(centerOBB, (Vector3D) vertex, this.Center, Vector3D.Zero),
            Color = new Color(0, 10 + num, 0)
          };
          formatPositionColorList.Add(formatPositionColor);
        }
        num = (num + 10) % 95;
      }
      if (formatPositionColorList.Count <= 0)
        return;
      this.m_newObbCoordsPolygons[coords] = formatPositionColorList;
    }

    private void DrawPersistentDebugNavmesh()
    {
      foreach (KeyValuePair<Vector2I, List<MyFormatPositionColor>> obbCoordsPolygon in this.m_newObbCoordsPolygons)
      {
        if (this.m_newObbCoordsPolygons[obbCoordsPolygon.Key] == null)
          this.m_obbCoordsPolygons.Remove(obbCoordsPolygon.Key);
        else
          this.m_obbCoordsPolygons[obbCoordsPolygon.Key] = obbCoordsPolygon.Value;
      }
      this.m_newObbCoordsPolygons.Clear();
      if (this.m_obbCoordsPolygons.Count <= 0)
        return;
      List<MyFormatPositionColor> vertices = new List<MyFormatPositionColor>();
      foreach (List<MyFormatPositionColor> formatPositionColorList in this.m_obbCoordsPolygons.Values)
      {
        for (int index = 0; index < formatPositionColorList.Count; ++index)
          vertices.Add(formatPositionColorList[index]);
      }
      if (this.m_drawNavmeshId == uint.MaxValue)
        this.m_drawNavmeshId = MyRenderProxy.DebugDrawMesh(vertices, MatrixD.Identity, true, true);
      else
        MyRenderProxy.DebugDrawUpdateMesh(this.m_drawNavmeshId, vertices, MatrixD.Identity, true, true);
    }

    private void HidePersistentDebugNavmesh()
    {
      if (this.m_drawNavmeshId == uint.MaxValue)
        return;
      MyRenderProxy.RemoveRenderObject(this.m_drawNavmeshId, MyRenderProxy.ObjectType.DebugDrawMesh);
      this.m_drawNavmeshId = uint.MaxValue;
    }

    private void UpdatePersistentDebugNavmesh() => this.DrawNavmesh = this.DrawNavmesh;

    public class CoordComparer : IEqualityComparer<Vector2I>
    {
      public bool Equals(Vector2I a, Vector2I b) => a.X == b.X && a.Y == b.Y;

      public int GetHashCode(Vector2I point) => (point.X.ToString() + point.Y.ToString()).GetHashCode();
    }

    public class OBBCoordComparer : IEqualityComparer<MyNavmeshOBBs.OBBCoords>
    {
      public bool Equals(MyNavmeshOBBs.OBBCoords a, MyNavmeshOBBs.OBBCoords b) => a.Coords.X == b.Coords.X && a.Coords.Y == b.Coords.Y;

      public int GetHashCode(MyNavmeshOBBs.OBBCoords point) => (point.Coords.X.ToString() + point.Coords.Y.ToString()).GetHashCode();
    }

    private struct Vertex
    {
      public Vector3D pos;
      public Color color;
    }
  }
}
