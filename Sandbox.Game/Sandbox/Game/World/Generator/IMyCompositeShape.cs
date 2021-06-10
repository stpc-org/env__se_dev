// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.IMyCompositeShape
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public interface IMyCompositeShape
  {
    ContainmentType Contains(
      ref BoundingBox queryBox,
      ref BoundingSphere querySphere,
      int lodVoxelSize);

    float SignedDistance(ref Vector3 localPos, int lodVoxelSize);

    void ComputeContent(
      MyStorageData storage,
      int lodIndex,
      Vector3I lodVoxelRangeMin,
      Vector3I lodVoxelRangeMax,
      int lodVoxelSize);

    void DebugDraw(ref MatrixD worldMatrix, Color color);

    void Close();
  }
}
