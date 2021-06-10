// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyCsgTorus
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Noise;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
  internal sealed class MyCsgTorus : MyCsgShapeBase
  {
    private Vector3 m_translation;
    private Quaternion m_invRotation;
    private float m_primaryRadius;
    private float m_secondaryRadius;
    private float m_secondaryHalfDeviation;
    private float m_deviationFrequency;
    private float m_detailFrequency;
    private float m_potentialHalfDeviation;

    internal MyCsgTorus(
      Vector3 translation,
      Quaternion invRotation,
      float primaryRadius,
      float secondaryRadius,
      float secondaryHalfDeviation,
      float deviationFrequency,
      float detailFrequency)
    {
      this.m_translation = translation;
      this.m_invRotation = invRotation;
      this.m_primaryRadius = primaryRadius;
      this.m_secondaryRadius = secondaryRadius;
      this.m_deviationFrequency = deviationFrequency;
      this.m_detailFrequency = detailFrequency;
      this.m_potentialHalfDeviation = this.m_secondaryHalfDeviation + this.m_detailSize;
      if ((double) this.m_detailFrequency != 0.0)
        return;
      this.m_enableModulation = false;
    }

    internal override ContainmentType Contains(
      ref BoundingBox queryAabb,
      ref BoundingSphere querySphere,
      float lodVoxelSize)
    {
      BoundingSphere boundingSphere = querySphere;
      boundingSphere.Center -= this.m_translation;
      Vector3.Transform(ref boundingSphere.Center, ref this.m_invRotation, out boundingSphere.Center);
      boundingSphere.Radius += lodVoxelSize;
      Vector2 vector2 = new Vector2(boundingSphere.Center.X, boundingSphere.Center.Z);
      vector2 = new Vector2(vector2.Length() - this.m_primaryRadius, boundingSphere.Center.Y);
      float num1 = vector2.Length() - this.m_secondaryRadius;
      float num2 = this.m_potentialHalfDeviation + lodVoxelSize + boundingSphere.Radius;
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
      Vector3 result = position - this.m_translation;
      Vector3.Transform(ref result, ref this.m_invRotation, out result);
      Vector2 vector2 = new Vector2(result.X, result.Z);
      vector2 = new Vector2(vector2.Length() - this.m_primaryRadius, result.Y);
      float signedDistance = vector2.Length() - this.m_secondaryRadius;
      float num = this.m_potentialHalfDeviation + lodVoxelSize;
      if ((double) signedDistance > (double) num)
        return 1f;
      return (double) signedDistance < -(double) num ? -1f : this.SignedDistanceInternal(lodVoxelSize, macroModulator, detailModulator, ref result, ref signedDistance);
    }

    private float SignedDistanceInternal(
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator,
      ref Vector3 localPosition,
      ref float signedDistance)
    {
      if (this.m_enableModulation)
      {
        float num1 = 0.5f * this.m_deviationFrequency;
        Vector3 vector3 = localPosition * num1;
        float num2 = (float) macroModulator.GetValue((double) vector3.X, (double) vector3.Y, (double) vector3.Z);
        signedDistance -= num2 * this.m_secondaryHalfDeviation;
      }
      if (this.m_enableModulation && -(double) this.m_detailSize < (double) signedDistance && (double) signedDistance < (double) this.m_detailSize)
      {
        float num = 0.5f * this.m_detailFrequency;
        Vector3 vector3 = localPosition * num;
        signedDistance += this.m_detailSize * (float) detailModulator.GetValue((double) vector3.X, (double) vector3.Y, (double) vector3.Z);
      }
      return signedDistance / lodVoxelSize;
    }

    internal override float SignedDistanceUnchecked(
      ref Vector3 position,
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator)
    {
      Vector3 result = position - this.m_translation;
      Vector3.Transform(ref result, ref this.m_invRotation, out result);
      Vector2 vector2 = new Vector2(result.X, result.Z);
      vector2 = new Vector2(vector2.Length() - this.m_primaryRadius, result.Y);
      float signedDistance = vector2.Length() - this.m_secondaryRadius;
      return this.SignedDistanceInternal(lodVoxelSize, macroModulator, detailModulator, ref result, ref signedDistance);
    }

    internal override void DebugDraw(ref MatrixD worldTranslation, Color color)
    {
      MatrixD matrixD1 = MatrixD.CreateTranslation(this.m_translation) * worldTranslation;
      float num1 = (float) (((double) this.m_primaryRadius + (double) this.m_secondaryRadius) * 2.0);
      float num2 = this.m_secondaryRadius * 2f;
      MatrixD scale = MatrixD.CreateScale((double) num1, (double) num2, (double) num1);
      MatrixD result;
      MatrixD.CreateFromQuaternion(ref this.m_invRotation, out result);
      MatrixD.Transpose(ref result, out result);
      MatrixD matrixD2 = result;
      MyRenderProxy.DebugDrawCylinder(scale * matrixD2 * matrixD1, (Color) color.ToVector3(), 0.5f, true, false);
    }

    internal override MyCsgShapeBase DeepCopy() => (MyCsgShapeBase) new MyCsgTorus(this.m_translation, this.m_invRotation, this.m_primaryRadius, this.m_secondaryRadius, this.m_secondaryHalfDeviation, this.m_deviationFrequency, this.m_detailFrequency);

    internal override void ShrinkTo(float percentage)
    {
      this.m_secondaryRadius *= percentage;
      this.m_secondaryHalfDeviation *= percentage;
    }

    internal override Vector3 Center() => this.m_translation;
  }
}
