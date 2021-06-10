// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyCsgSphere
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Noise;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
  internal class MyCsgSphere : MyCsgShapeBase
  {
    private Vector3 m_translation;
    private float m_radius;
    private float m_halfDeviation;
    private float m_deviationFrequency;
    private float m_detailFrequency;
    private float m_outerRadius;
    private float m_innerRadius;

    public MyCsgSphere(
      Vector3 translation,
      float radius,
      float halfDeviation = 0.0f,
      float deviationFrequency = 0.0f,
      float detailFrequency = 0.0f)
    {
      this.m_translation = translation;
      this.m_radius = radius;
      this.m_halfDeviation = halfDeviation;
      this.m_deviationFrequency = deviationFrequency;
      this.m_detailFrequency = detailFrequency;
      if ((double) this.m_halfDeviation == 0.0 && (double) this.m_deviationFrequency == 0.0 && (double) detailFrequency == 0.0)
      {
        this.m_enableModulation = false;
        this.m_detailSize = 0.0f;
      }
      this.ComputeDerivedProperties();
    }

    internal override ContainmentType Contains(
      ref BoundingBox queryAabb,
      ref BoundingSphere querySphere,
      float lodVoxelSize)
    {
      BoundingSphere boundingSphere = new BoundingSphere(this.m_translation, this.m_outerRadius + lodVoxelSize);
      ContainmentType result1;
      boundingSphere.Contains(ref queryAabb, out result1);
      if (result1 == ContainmentType.Disjoint)
        return ContainmentType.Disjoint;
      boundingSphere.Radius = this.m_innerRadius - lodVoxelSize;
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
      Vector3 localPosition = position - this.m_translation;
      float distance = localPosition.Length();
      if ((double) this.m_innerRadius - (double) lodVoxelSize > (double) distance)
        return -1f;
      return (double) this.m_outerRadius + (double) lodVoxelSize < (double) distance ? 1f : this.SignedDistanceInternal(lodVoxelSize, macroModulator, detailModulator, ref localPosition, distance);
    }

    private float SignedDistanceInternal(
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator,
      ref Vector3 localPosition,
      float distance)
    {
      float num1;
      if (this.m_enableModulation)
      {
        float num2 = 0.0f;
        if ((double) distance != 0.0)
          num2 = 1f / distance;
        float num3 = this.m_deviationFrequency * this.m_radius * num2;
        Vector3 vector3 = localPosition * num3;
        num1 = (float) macroModulator.GetValue((double) vector3.X, (double) vector3.Y, (double) vector3.Z);
      }
      else
        num1 = 0.0f;
      float num4 = (float) ((double) distance - (double) this.m_radius - (double) num1 * (double) this.m_halfDeviation);
      if (this.m_enableModulation && -(double) this.m_detailSize < (double) num4 && (double) num4 < (double) this.m_detailSize)
      {
        float num2 = (float) ((double) this.m_detailFrequency * (double) this.m_radius / ((double) distance == 0.0 ? 1.0 : (double) distance));
        Vector3 vector3 = localPosition * num2;
        num4 += this.m_detailSize * (float) detailModulator.GetValue((double) vector3.X, (double) vector3.Y, (double) vector3.Z);
      }
      return num4 / lodVoxelSize;
    }

    internal override float SignedDistanceUnchecked(
      ref Vector3 position,
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator)
    {
      Vector3 localPosition = position - this.m_translation;
      float distance = localPosition.Length();
      return this.SignedDistanceInternal(lodVoxelSize, macroModulator, detailModulator, ref localPosition, distance);
    }

    internal override void DebugDraw(ref MatrixD worldTranslation, Color color) => MyRenderProxy.DebugDrawSphere(Vector3D.Transform(this.m_translation, worldTranslation), this.m_radius, (Color) color.ToVector3(), 0.5f);

    internal override MyCsgShapeBase DeepCopy() => (MyCsgShapeBase) new MyCsgSphere(this.m_translation, this.m_radius, this.m_halfDeviation, this.m_deviationFrequency, this.m_detailFrequency);

    internal override void ShrinkTo(float percentage)
    {
      this.m_radius *= percentage;
      this.m_halfDeviation *= percentage;
      this.ComputeDerivedProperties();
    }

    private void ComputeDerivedProperties()
    {
      this.m_outerRadius = this.m_radius + this.m_halfDeviation + this.m_detailSize;
      this.m_innerRadius = this.m_radius - this.m_halfDeviation - this.m_detailSize;
    }

    internal override Vector3 Center() => this.m_translation;
  }
}
