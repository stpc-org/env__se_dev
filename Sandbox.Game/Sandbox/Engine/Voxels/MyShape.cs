// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyShape
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public abstract class MyShape : IMyVoxelShape
  {
    protected MatrixD m_transformation = MatrixD.Identity;
    protected MatrixD m_inverse = MatrixD.Identity;
    protected bool m_inverseIsDirty;

    public MatrixD Transformation
    {
      get => this.m_transformation;
      set
      {
        this.m_transformation = value;
        this.m_inverseIsDirty = true;
      }
    }

    public MatrixD InverseTransformation
    {
      get
      {
        if (this.m_inverseIsDirty)
        {
          MatrixD.Invert(ref this.m_transformation, out this.m_inverse);
          this.m_inverseIsDirty = false;
        }
        return this.m_inverse;
      }
    }

    public abstract BoundingBoxD GetWorldBoundaries();

    public abstract BoundingBoxD PeekWorldBoundaries(ref Vector3D targetPosition);

    public abstract BoundingBoxD GetLocalBounds();

    public abstract float GetVolume(ref Vector3D voxelPosition);

    protected float SignedDistanceToDensity(float signedDistance) => (float) ((double) MathHelper.Clamp(-signedDistance, -1f, 1f) * 0.5 + 0.5);

    public abstract void SendPaintRequest(MyVoxelBase voxel, byte newMaterialIndex);

    public abstract void SendCutOutRequest(MyVoxelBase voxelbool);

    public virtual void SendDrillCutOutRequest(MyVoxelBase voxel, bool damage = false)
    {
    }

    public abstract void SendFillRequest(MyVoxelBase voxel, byte newMaterialIndex);

    public abstract void SendRevertRequest(MyVoxelBase voxel);

    public abstract MyShape Clone();

    MatrixD IMyVoxelShape.Transform
    {
      get => this.Transformation;
      set => this.Transformation = value;
    }

    BoundingBoxD IMyVoxelShape.GetWorldBoundary() => this.GetWorldBoundaries();

    BoundingBoxD IMyVoxelShape.PeekWorldBoundary(
      ref Vector3D targetPosition)
    {
      return this.PeekWorldBoundaries(ref targetPosition);
    }

    float IMyVoxelShape.GetIntersectionVolume(ref Vector3D voxelPosition) => this.GetVolume(ref voxelPosition);
  }
}
