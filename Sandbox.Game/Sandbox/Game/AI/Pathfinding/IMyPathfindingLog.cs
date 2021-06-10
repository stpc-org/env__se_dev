// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.IMyPathfindingLog
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
  public interface IMyPathfindingLog
  {
    void LogStorageWrite(
      MyVoxelBase map,
      MyStorageData source,
      MyStorageDataTypeFlags dataToWrite,
      Vector3I voxelRangeMin,
      Vector3I voxelRangeMax);
  }
}
