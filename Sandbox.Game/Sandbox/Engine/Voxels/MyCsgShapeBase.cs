// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyCsgShapeBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Noise;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  internal abstract class MyCsgShapeBase
  {
    protected bool m_enableModulation;
    protected float m_detailSize;

    protected MyCsgShapeBase()
    {
      this.m_enableModulation = true;
      this.m_detailSize = 6f;
    }

    internal abstract ContainmentType Contains(
      ref BoundingBox queryAabb,
      ref BoundingSphere querySphere,
      float lodVoxelSize);

    internal abstract float SignedDistance(
      ref Vector3 position,
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator);

    internal abstract float SignedDistanceUnchecked(
      ref Vector3 position,
      float lodVoxelSize,
      IMyModule macroModulator,
      IMyModule detailModulator);

    internal virtual void DebugDraw(ref MatrixD worldTranslation, Color color)
    {
    }

    internal abstract MyCsgShapeBase DeepCopy();

    internal abstract void ShrinkTo(float percentage);

    internal abstract Vector3 Center();

    internal virtual void ReleaseMaps()
    {
    }
  }
}
