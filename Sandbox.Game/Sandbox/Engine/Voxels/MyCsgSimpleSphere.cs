// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyCsgSimpleSphere
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Noise;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
  internal class MyCsgSimpleSphere : MyCsgShapeBase
  {
    private Vector3 m_translation;
    private float m_radius;

    public MyCsgSimpleSphere(Vector3 translation, float radius)
    {
      this.m_translation = translation;
      this.m_radius = radius;
    }

    internal override ContainmentType Contains(
      ref BoundingBox queryAabb,
      ref BoundingSphere querySphere,
      float lodVoxelSize)
    {
      BoundingSphere boundingSphere = new BoundingSphere(this.m_translation, this.m_radius + lodVoxelSize);
      ContainmentType result1;
      boundingSphere.Contains(ref queryAabb, out result1);
      if (result1 == ContainmentType.Disjoint)
        return ContainmentType.Disjoint;
      boundingSphere.Radius = this.m_radius - lodVoxelSize;
      ContainmentType result2;
      boundingSphere.Contains(ref queryAabb, out result2);
      return result2 == ContainmentType.Contains ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    internal override float SignedDistance(
      ref Vector3 position,
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator)
    {
      float distance = (position - this.m_translation).Length();
      if ((double) this.m_radius - (double) lodVoxelSize > (double) distance)
        return -1f;
      return (double) this.m_radius + (double) lodVoxelSize < (double) distance ? 1f : this.SignedDistanceInternal(lodVoxelSize, distance);
    }

    private float SignedDistanceInternal(float lodVoxelSize, float distance) => (distance - this.m_radius) / lodVoxelSize;

    internal override float SignedDistanceUnchecked(
      ref Vector3 position,
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator)
    {
      float distance = (position - this.m_translation).Length();
      return this.SignedDistanceInternal(lodVoxelSize, distance);
    }

    internal override void DebugDraw(ref MatrixD worldTranslation, Color color) => MyRenderProxy.DebugDrawSphere(Vector3D.Transform(this.m_translation, worldTranslation), this.m_radius, (Color) color.ToVector3(), 0.5f);

    internal override MyCsgShapeBase DeepCopy() => (MyCsgShapeBase) new MyCsgSimpleSphere(this.m_translation, this.m_radius);

    internal override void ShrinkTo(float percentage) => this.m_radius *= percentage;

    internal override Vector3 Center() => this.m_translation;
  }
}
