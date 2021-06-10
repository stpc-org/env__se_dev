// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.IMyIsoMesher
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.Voxels;
using VRageMath;

namespace VRage.Voxels
{
  public interface IMyIsoMesher
  {
    int InvalidatedRangeInflate { get; }

    MyIsoMesh Precalc(
      IMyStorage storage,
      int lod,
      Vector3I lodVoxelMin,
      Vector3I lodVoxelMax,
      MyStorageDataTypeFlags properties = MyStorageDataTypeFlags.ContentAndMaterial,
      MyVoxelRequestFlags flags = (MyVoxelRequestFlags) 0);
  }
}
