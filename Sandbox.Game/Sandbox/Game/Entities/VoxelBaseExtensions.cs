// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.VoxelBaseExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Voxels;
using VRage.Game;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public static class VoxelBaseExtensions
  {
    public static MyVoxelMaterialDefinition GetMaterialAt(
      this MyVoxelBase self,
      ref Vector3D worldPosition)
    {
      if (self.Storage == null)
        return (MyVoxelMaterialDefinition) null;
      Vector3 localPosition;
      MyVoxelCoordSystems.WorldPositionToLocalPosition(worldPosition, self.PositionComp.WorldMatrixRef, self.PositionComp.WorldMatrixInvScaled, self.SizeInMetresHalf, out localPosition);
      Vector3I voxelCoords = new Vector3I(localPosition / 1f) + self.StorageMin;
      return self.Storage.GetMaterialAt(ref voxelCoords);
    }
  }
}
