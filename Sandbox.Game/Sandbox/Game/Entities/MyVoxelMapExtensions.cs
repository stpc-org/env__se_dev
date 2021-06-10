// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyVoxelMapExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Voxels;
using VRage.Game.Components;
using VRage.Game.Models;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities
{
  internal static class MyVoxelMapExtensions
  {
    public static Vector3D GetPositionOnVoxel(
      this MyVoxelMap map,
      Vector3D position,
      float maxVertDistance)
    {
      Vector3D worldPosition = position;
      Vector3I geometryCellCoord;
      MyVoxelCoordSystems.WorldPositionToGeometryCellCoord(map.PositionLeftBottomCorner, ref position, out geometryCellCoord);
      BoundingBox localAABB;
      MyVoxelCoordSystems.GeometryCellCoordToLocalAABB(ref geometryCellCoord, out localAABB);
      Vector3 center = localAABB.Center;
      Line localLine = new Line((Vector3) (center + Vector3D.Up * (double) maxVertDistance), (Vector3) (center + Vector3D.Down * (double) maxVertDistance));
      MyIntersectionResultLineTriangle result;
      if (map.Storage.GetGeometry().Intersect(ref localLine, out result, IntersectionFlags.ALL_TRIANGLES))
      {
        Vector3 vertex0 = result.InputTriangle.Vertex0;
        MyVoxelCoordSystems.LocalPositionToWorldPosition(map.PositionLeftBottomCorner - (Vector3D) map.StorageMin, ref vertex0, out worldPosition);
      }
      return worldPosition;
    }
  }
}
