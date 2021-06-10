// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.MyVoxelCoordSystems
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRage.Voxels
{
  public static class MyVoxelCoordSystems
  {
    public static void WorldPositionToLocalPosition(
      Vector3D worldPosition,
      MatrixD worldMatrix,
      MatrixD worldMatrixInv,
      Vector3 halfSize,
      out Vector3 localPosition)
    {
      localPosition = (Vector3) Vector3D.Transform(worldPosition + Vector3D.TransformNormal(halfSize, worldMatrix), worldMatrixInv);
    }

    public static void WorldPositionToLocalPosition(
      MatrixD matrix,
      Vector3D referenceVoxelMapPosition,
      Vector3 halfSize,
      ref Vector3D worldPosition,
      out Vector3 localPosition)
    {
      localPosition = (Vector3) (Vector3D.Transform(worldPosition, matrix) + halfSize);
    }

    public static void WorldPositionToLocalPosition(
      Vector3D referenceVoxelMapPosition,
      ref Vector3D worldPosition,
      out Vector3 localPosition)
    {
      localPosition = (Vector3) (worldPosition - referenceVoxelMapPosition);
    }

    public static void LocalPositionToWorldPosition(
      MatrixD matrix,
      Vector3D referenceVoxelMapPosition,
      Vector3 halfSize,
      ref Vector3 localPosition,
      out Vector3D worldPosition)
    {
      worldPosition = Vector3D.Transform(localPosition - halfSize, matrix);
    }

    public static void LocalPositionToWorldPosition(
      Vector3D referenceVoxelMapPosition,
      ref Vector3 localPosition,
      out Vector3D worldPosition)
    {
      worldPosition = (Vector3D) localPosition + referenceVoxelMapPosition;
    }

    public static void LocalPositionToVoxelCoord(ref Vector3 localPosition, out Vector3I voxelCoord)
    {
      Vector3 v = localPosition / 1f;
      Vector3I.Floor(ref v, out voxelCoord);
    }

    public static void LocalPositionToVoxelCoord(ref Vector3 localPosition, out Vector3D voxelCoord) => voxelCoord = (Vector3D) (localPosition / 1f);

    public static void LocalPositionToGeometryCellCoord(
      ref Vector3 localPosition,
      out Vector3I geometryCellCoord)
    {
      Vector3D v = (Vector3D) (localPosition / 8f);
      Vector3I.Floor(ref v, out geometryCellCoord);
    }

    public static void WorldPositionToVoxelCoord(
      ref Vector3D worldPosition,
      MatrixD worldMatrix,
      MatrixD worldMatrixInv,
      Vector3 halfSize,
      out Vector3I voxelCoord)
    {
      Vector3 localPosition;
      MyVoxelCoordSystems.WorldPositionToLocalPosition(worldPosition, worldMatrix, worldMatrixInv, halfSize, out localPosition);
      MyVoxelCoordSystems.LocalPositionToVoxelCoord(ref localPosition, out voxelCoord);
    }

    public static void WorldPositionToVoxelCoord(
      Vector3D referenceVoxelMapPosition,
      ref Vector3D worldPosition,
      out Vector3I voxelCoord)
    {
      Vector3 localPosition;
      MyVoxelCoordSystems.WorldPositionToLocalPosition(referenceVoxelMapPosition, ref worldPosition, out localPosition);
      MyVoxelCoordSystems.LocalPositionToVoxelCoord(ref localPosition, out voxelCoord);
    }

    public static void WorldPositionToGeometryCellCoord(
      Vector3D referenceVoxelMapPosition,
      ref Vector3D worldPosition,
      out Vector3I geometryCellCoord)
    {
      Vector3 localPosition;
      MyVoxelCoordSystems.WorldPositionToLocalPosition(referenceVoxelMapPosition, ref worldPosition, out localPosition);
      MyVoxelCoordSystems.LocalPositionToGeometryCellCoord(ref localPosition, out geometryCellCoord);
    }

    public static void VoxelCoordToLocalPosition(ref Vector3I voxelCoord, out Vector3 localPosition) => localPosition = voxelCoord * 1f;

    public static void GeometryCellCoordToLocalPosition(
      ref MyCellCoord geometryCellCoord,
      out Vector3 localPosition)
    {
      localPosition = geometryCellCoord.CoordInLod * 8f * (float) (1 << geometryCellCoord.Lod);
    }

    public static void GeometryCellCoordToLocalPosition(
      ref Vector3I geometryCellCoord,
      out Vector3 localPosition)
    {
      localPosition = geometryCellCoord * 8f;
    }

    public static void VoxelCoordToWorldPosition(
      MatrixD matrix,
      Vector3D referenceVoxelMapPosition,
      Vector3 halfsize,
      ref Vector3I voxelCoord,
      out Vector3D worldPosition)
    {
      Vector3 localPosition;
      MyVoxelCoordSystems.VoxelCoordToLocalPosition(ref voxelCoord, out localPosition);
      MyVoxelCoordSystems.LocalPositionToWorldPosition(matrix, referenceVoxelMapPosition, halfsize, ref localPosition, out worldPosition);
    }

    public static void WorldPositionToVoxelCoord(
      MatrixD matrix,
      Vector3D referenceVoxelMapPosition,
      Vector3 halfsize,
      ref Vector3D worldPosition,
      out Vector3I voxelCoord)
    {
      Vector3 localPosition;
      MyVoxelCoordSystems.WorldPositionToLocalPosition(matrix, referenceVoxelMapPosition, halfsize, ref worldPosition, out localPosition);
      MyVoxelCoordSystems.LocalPositionToVoxelCoord(ref localPosition, out voxelCoord);
    }

    public static void VoxelCoordToWorldPosition(
      Vector3D referenceVoxelMapPosition,
      ref Vector3I voxelCoord,
      out Vector3D worldPosition)
    {
      Vector3 localPosition;
      MyVoxelCoordSystems.VoxelCoordToLocalPosition(ref voxelCoord, out localPosition);
      MyVoxelCoordSystems.LocalPositionToWorldPosition(referenceVoxelMapPosition, ref localPosition, out worldPosition);
    }

    public static void GeometryCellCoordToLocalAABB(
      ref Vector3I geometryCellCoord,
      out BoundingBox localAABB)
    {
      Vector3 localPosition;
      MyVoxelCoordSystems.GeometryCellCoordToLocalPosition(ref geometryCellCoord, out localPosition);
      localAABB = new BoundingBox(localPosition, localPosition + 8f);
    }

    public static void VoxelCoordToWorldAABB(
      Vector3D referenceVoxelMapPosition,
      ref Vector3I voxelCoord,
      out BoundingBoxD worldAABB)
    {
      Vector3D worldPosition;
      MyVoxelCoordSystems.VoxelCoordToWorldPosition(referenceVoxelMapPosition, ref voxelCoord, out worldPosition);
      worldAABB = new BoundingBoxD(worldPosition, worldPosition + 1f);
    }

    public static void GeometryCellCoordToWorldAABB(
      Vector3D referenceVoxelMapPosition,
      ref Vector3I geometryCellCoord,
      out BoundingBoxD worldAABB)
    {
      Vector3 localPosition;
      MyVoxelCoordSystems.GeometryCellCoordToLocalPosition(ref geometryCellCoord, out localPosition);
      Vector3D worldPosition = (Vector3D) localPosition;
      MyVoxelCoordSystems.LocalPositionToWorldPosition(referenceVoxelMapPosition, ref localPosition, out worldPosition);
      worldAABB = new BoundingBoxD(worldPosition, worldPosition + 8f);
    }

    public static void GeometryCellCoordToWorldAABB(
      Vector3D referenceVoxelMapPosition,
      ref MyCellCoord geometryCellCoord,
      out BoundingBoxD worldAABB)
    {
      Vector3 localPosition;
      MyVoxelCoordSystems.GeometryCellCoordToLocalPosition(ref geometryCellCoord, out localPosition);
      Vector3D worldPosition = (Vector3D) localPosition;
      MyVoxelCoordSystems.LocalPositionToWorldPosition(referenceVoxelMapPosition, ref localPosition, out worldPosition);
      worldAABB = new BoundingBoxD(worldPosition, worldPosition + 8f * (float) (1 << geometryCellCoord.Lod));
    }

    public static void GeometryCellCenterCoordToWorldPos(
      Vector3D referenceVoxelMapPosition,
      ref Vector3I geometryCellCoord,
      out Vector3D worldPos)
    {
      Vector3 localPosition;
      MyVoxelCoordSystems.GeometryCellCoordToLocalPosition(ref geometryCellCoord, out localPosition);
      Vector3D worldPosition = (Vector3D) localPosition;
      MyVoxelCoordSystems.LocalPositionToWorldPosition(referenceVoxelMapPosition, ref localPosition, out worldPosition);
      worldPos = worldPosition + 4.0;
    }

    public static void VoxelCoordToGeometryCellCoord(
      ref Vector3I voxelCoord,
      out Vector3I geometryCellCoord)
    {
      geometryCellCoord = voxelCoord >> 3;
    }

    public static void LocalPositionToVertexCell(
      int lod,
      ref Vector3 localPosition,
      out Vector3I vertexCell)
    {
      float num = 1f * (float) (1 << lod);
      vertexCell = Vector3I.Floor(localPosition / num);
    }

    public static void VertexCellToLocalPosition(
      int lod,
      ref Vector3I vertexCell,
      out Vector3 localPosition)
    {
      float num = 1f * (float) (1 << lod);
      localPosition = vertexCell * num;
    }

    public static void VertexCellToLocalAABB(
      int lod,
      ref Vector3I vertexCell,
      out BoundingBoxD localAABB)
    {
      float num = 1f * (float) (1 << lod);
      Vector3 vector3 = vertexCell * num;
      localAABB = new BoundingBoxD((Vector3D) vector3, (Vector3D) (vector3 + num));
    }

    public static Vector3I FindBestOctreeSize(float radius)
    {
      int num = 32;
      while ((double) num < (double) radius)
        num *= 2;
      return new Vector3I(num, num, num);
    }
  }
}
