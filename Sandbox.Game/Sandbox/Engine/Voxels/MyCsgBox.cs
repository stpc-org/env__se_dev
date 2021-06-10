// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyCsgBox
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Noise;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
  internal class MyCsgBox : MyCsgShapeBase
  {
    private Vector3 m_translation;
    private float m_halfExtents;

    public MyCsgBox(Vector3 translation, float halfExtents)
    {
      this.m_translation = translation;
      this.m_halfExtents = halfExtents;
    }

    internal override ContainmentType Contains(
      ref BoundingBox queryAabb,
      ref BoundingSphere querySphere,
      float lodVoxelSize)
    {
      ContainmentType result1;
      BoundingBox.CreateFromHalfExtent(this.m_translation, this.m_halfExtents + lodVoxelSize).Contains(ref queryAabb, out result1);
      if (result1 == ContainmentType.Disjoint)
        return ContainmentType.Disjoint;
      ContainmentType result2;
      BoundingBox.CreateFromHalfExtent(this.m_translation, this.m_halfExtents - lodVoxelSize).Contains(ref queryAabb, out result2);
      return result2 == ContainmentType.Contains ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    internal override float SignedDistance(
      ref Vector3 position,
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator)
    {
      return MathHelper.Clamp(this.SignedDistanceUnchecked(ref position, lodVoxelSize, macroModulator, detailModulator), -1f, 1f);
    }

    internal override float SignedDistanceUnchecked(
      ref Vector3 position,
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator)
    {
      Vector3 vector3 = Vector3.Abs(position - this.m_translation) - this.m_halfExtents;
      return (Math.Min(Math.Max(vector3.X, Math.Max(vector3.Y, vector3.Z)), 0.0f) + Vector3.Max(vector3, Vector3.Zero).Length()) / lodVoxelSize;
    }

    internal override void DebugDraw(ref MatrixD worldTranslation, Color color)
    {
      BoundingBoxD aabb = new BoundingBoxD((Vector3D) (this.m_translation - this.m_halfExtents), (Vector3D) (this.m_translation + this.m_halfExtents));
      aabb = aabb.TransformFast(worldTranslation);
      MyRenderProxy.DebugDrawAABB(aabb, color, 0.5f, depthRead: false);
    }

    internal override MyCsgShapeBase DeepCopy() => (MyCsgShapeBase) new MyCsgBox(this.m_translation, this.m_halfExtents);

    internal override void ShrinkTo(float percentage) => this.m_halfExtents *= percentage;

    internal override Vector3 Center() => this.m_translation;

    internal float HalfExtents => this.m_halfExtents;
  }
}
