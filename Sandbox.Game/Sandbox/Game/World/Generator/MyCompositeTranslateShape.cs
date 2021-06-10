// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyCompositeTranslateShape
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public class MyCompositeTranslateShape : IMyCompositeShape
  {
    private Vector3 m_translation;
    private readonly IMyCompositeShape m_shape;

    public MyCompositeTranslateShape(IMyCompositeShape shape, Vector3 translation)
    {
      this.m_shape = shape;
      this.m_translation = -translation;
    }

    public ContainmentType Contains(
      ref BoundingBox queryBox,
      ref BoundingSphere querySphere,
      int lodVoxelSize)
    {
      BoundingBox queryBox1 = queryBox;
      queryBox1.Translate(this.m_translation);
      BoundingSphere querySphere1 = querySphere.Translate(ref this.m_translation);
      return this.m_shape.Contains(ref queryBox1, ref querySphere1, lodVoxelSize);
    }

    public float SignedDistance(ref Vector3 localPos, int lodVoxelSize)
    {
      Vector3 localPos1 = localPos + this.m_translation;
      return this.m_shape.SignedDistance(ref localPos1, lodVoxelSize);
    }

    public void ComputeContent(
      MyStorageData storage,
      int lodIndex,
      Vector3I lodVoxelRangeMin,
      Vector3I lodVoxelRangeMax,
      int lodVoxelSize)
    {
      Vector3I vector3I = (Vector3I) this.m_translation >> lodIndex;
      this.m_shape.ComputeContent(storage, lodIndex, lodVoxelRangeMin + vector3I, lodVoxelRangeMax + vector3I, lodVoxelSize);
    }

    public void DebugDraw(ref MatrixD worldMatrix, Color color)
    {
      MatrixD worldMatrix1 = MatrixD.CreateTranslation(-this.m_translation) * worldMatrix;
      this.m_shape.DebugDraw(ref worldMatrix1, color);
    }

    public void Close() => this.m_shape.Close();
  }
}
