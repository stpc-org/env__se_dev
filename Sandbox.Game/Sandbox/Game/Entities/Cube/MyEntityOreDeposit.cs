// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyEntityOreDeposit
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public class MyEntityOreDeposit
  {
    public long DetectorId;
    public MyVoxelBase VoxelMap;
    public Vector3I CellCoord;
    public readonly List<MyEntityOreDeposit.Data> Materials = new List<MyEntityOreDeposit.Data>();
    public static readonly MyEntityOreDeposit.TypeComparer Comparer = new MyEntityOreDeposit.TypeComparer();

    public MyEntityOreDeposit(MyVoxelBase voxelMap, Vector3I cellCoord, long detectorId)
    {
      this.VoxelMap = voxelMap;
      this.CellCoord = cellCoord;
      this.DetectorId = detectorId;
    }

    public struct Data
    {
      public MyVoxelMaterialDefinition Material;
      public Vector3 AverageLocalPosition;

      public Data(MyVoxelMaterialDefinition material, Vector3 averageLocalPosition)
        : this()
      {
        this.Material = material;
        this.AverageLocalPosition = averageLocalPosition;
      }

      internal void ComputeWorldPosition(MyVoxelBase voxelMap, out Vector3D oreWorldPosition) => MyVoxelCoordSystems.LocalPositionToWorldPosition(voxelMap.PositionComp.GetPosition() - (Vector3D) voxelMap.StorageMin, ref this.AverageLocalPosition, out oreWorldPosition);
    }

    public class TypeComparer : IEqualityComparer<MyEntityOreDeposit>
    {
      bool IEqualityComparer<MyEntityOreDeposit>.Equals(
        MyEntityOreDeposit x,
        MyEntityOreDeposit y)
      {
        return x.VoxelMap.EntityId == y.VoxelMap.EntityId && x.CellCoord == y.CellCoord && x.DetectorId == y.DetectorId;
      }

      int IEqualityComparer<MyEntityOreDeposit>.GetHashCode(
        MyEntityOreDeposit obj)
      {
        return (int) (obj.VoxelMap.EntityId ^ (long) obj.CellCoord.GetHashCode() * obj.DetectorId);
      }
    }
  }
}
