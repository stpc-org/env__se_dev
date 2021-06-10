// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyCsgCapsule
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Noise;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
  internal class MyCsgCapsule : MyCsgShapeBase
  {
    private Vector3 m_pointA;
    private Vector3 m_pointB;
    private float m_radius;
    private float m_halfDeviation;
    private float m_deviationFrequency;
    private float m_detailFrequency;
    private float m_potentialHalfDeviation;

    public MyCsgCapsule(
      Vector3 pointA,
      Vector3 pointB,
      float radius,
      float halfDeviation,
      float deviationFrequency,
      float detailFrequency)
    {
      this.m_pointA = pointA;
      this.m_pointB = pointB;
      this.m_radius = radius;
      this.m_halfDeviation = halfDeviation;
      this.m_deviationFrequency = deviationFrequency;
      this.m_detailFrequency = detailFrequency;
      if ((double) deviationFrequency == 0.0)
        this.m_enableModulation = false;
      this.m_potentialHalfDeviation = this.m_halfDeviation + this.m_detailSize;
    }

    internal override ContainmentType Contains(
      ref BoundingBox queryAabb,
      ref BoundingSphere querySphere,
      float lodVoxelSize)
    {
      Vector3 v = this.m_pointB - this.m_pointA;
      float max = v.Normalize();
      Vector3 vector3 = this.m_pointA + MathHelper.Clamp((querySphere.Center - this.m_pointA).Dot(ref v), 0.0f, max) * v;
      float num1 = (querySphere.Center - vector3).Length() - this.m_radius;
      float num2 = this.m_potentialHalfDeviation + lodVoxelSize + querySphere.Radius;
      if ((double) num1 > (double) num2)
        return ContainmentType.Disjoint;
      return (double) num1 < -(double) num2 ? ContainmentType.Contains : ContainmentType.Intersects;
    }

    internal override float SignedDistance(
      ref Vector3 position,
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator)
    {
      Vector3 vector3 = position - this.m_pointA;
      Vector3 v = this.m_pointB - this.m_pointA;
      float num1 = MathHelper.Clamp(vector3.Dot(ref v) / v.LengthSquared(), 0.0f, 1f);
      float signedDistance = (vector3 - v * num1).Length() - this.m_radius;
      float num2 = this.m_potentialHalfDeviation + lodVoxelSize;
      if ((double) signedDistance > (double) num2)
        return 1f;
      return (double) signedDistance < -(double) num2 ? -1f : this.SignedDistanceInternal(ref position, lodVoxelSize, macroModulator, detailModulator, ref signedDistance);
    }

    private float SignedDistanceInternal(
      ref Vector3 position,
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator,
      ref float signedDistance)
    {
      if (this.m_enableModulation)
      {
        float num = (float) macroModulator.GetValue((double) position.X * (double) this.m_deviationFrequency, (double) position.Y * (double) this.m_deviationFrequency, (double) position.Z * (double) this.m_deviationFrequency);
        signedDistance -= num * this.m_halfDeviation;
      }
      if (this.m_enableModulation && -(double) this.m_detailSize < (double) signedDistance && (double) signedDistance < (double) this.m_detailSize)
        signedDistance += this.m_detailSize * (float) detailModulator.GetValue((double) position.X * (double) this.m_detailFrequency, (double) position.Y * (double) this.m_detailFrequency, (double) position.Z * (double) this.m_detailFrequency);
      return signedDistance / lodVoxelSize;
    }

    internal override float SignedDistanceUnchecked(
      ref Vector3 position,
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator)
    {
      Vector3 vector3 = position - this.m_pointA;
      Vector3 v = this.m_pointB - this.m_pointA;
      float num = MathHelper.Clamp(vector3.Dot(ref v) / v.LengthSquared(), 0.0f, 1f);
      float signedDistance = (vector3 - v * num).Length() - this.m_radius;
      return this.SignedDistanceInternal(ref position, lodVoxelSize, macroModulator, detailModulator, ref signedDistance);
    }

    internal override void DebugDraw(ref MatrixD worldTranslation, Color color) => MyRenderProxy.DebugDrawCapsule(Vector3D.Transform(this.m_pointA, worldTranslation), Vector3D.Transform(this.m_pointB, worldTranslation), this.m_radius, color, true, true);

    internal override MyCsgShapeBase DeepCopy() => (MyCsgShapeBase) new MyCsgCapsule(this.m_pointA, this.m_pointB, this.m_radius, this.m_halfDeviation, this.m_deviationFrequency, this.m_detailFrequency);

    internal override void ShrinkTo(float percentage)
    {
      this.m_radius *= percentage;
      this.m_halfDeviation *= percentage;
    }

    internal override Vector3 Center() => (this.m_pointA + this.m_pointB) / 2f;
  }
}
