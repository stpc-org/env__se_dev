// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyShapeSphere
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public class MyShapeSphere : MyShape, IMyVoxelShapeSphere, IMyVoxelShape
  {
    public Vector3D Center;
    public float Radius;

    public MyShapeSphere()
    {
    }

    public MyShapeSphere(Vector3D center, float radius)
    {
      this.Center = center;
      this.Radius = radius;
    }

    public override BoundingBoxD GetWorldBoundaries() => new BoundingBoxD(this.Center - (double) this.Radius, this.Center + this.Radius).TransformFast(this.Transformation);

    public override BoundingBoxD PeekWorldBoundaries(ref Vector3D targetPosition) => new BoundingBoxD(targetPosition - (double) this.Radius, targetPosition + this.Radius);

    public override BoundingBoxD GetLocalBounds() => new BoundingBoxD(this.Center - (double) this.Radius, this.Center + this.Radius);

    public override float GetVolume(ref Vector3D voxelPosition)
    {
      if (this.m_inverseIsDirty)
      {
        MatrixD.Invert(ref this.m_transformation, out this.m_inverse);
        this.m_inverseIsDirty = false;
      }
      Vector3D.Transform(ref voxelPosition, ref this.m_inverse, out voxelPosition);
      return this.SignedDistanceToDensity((float) (voxelPosition - this.Center).Length() - this.Radius);
    }

    public override void SendDrillCutOutRequest(MyVoxelBase voxel, bool damage = false) => voxel.RequestVoxelCutoutSphere(this.Center, this.Radius, false, damage);

    public override void SendCutOutRequest(MyVoxelBase voxel) => voxel.RequestVoxelOperationSphere(this.Center, this.Radius, (byte) 0, MyVoxelBase.OperationType.Cut);

    public override void SendPaintRequest(MyVoxelBase voxel, byte newMaterialIndex) => voxel.RequestVoxelOperationSphere(this.Center, this.Radius, newMaterialIndex, MyVoxelBase.OperationType.Paint);

    public override void SendFillRequest(MyVoxelBase voxel, byte newMaterialIndex) => voxel.RequestVoxelOperationSphere(this.Center, this.Radius, newMaterialIndex, MyVoxelBase.OperationType.Fill);

    public override void SendRevertRequest(MyVoxelBase voxel) => voxel.RequestVoxelOperationSphere(this.Center, this.Radius, (byte) 0, MyVoxelBase.OperationType.Revert);

    public override MyShape Clone()
    {
      MyShapeSphere myShapeSphere = new MyShapeSphere();
      myShapeSphere.Transformation = this.Transformation;
      myShapeSphere.Center = this.Center;
      myShapeSphere.Radius = this.Radius;
      return (MyShape) myShapeSphere;
    }

    Vector3D IMyVoxelShapeSphere.Center
    {
      get => this.Center;
      set => this.Center = value;
    }

    float IMyVoxelShapeSphere.Radius
    {
      get => this.Radius;
      set => this.Radius = value;
    }
  }
}
