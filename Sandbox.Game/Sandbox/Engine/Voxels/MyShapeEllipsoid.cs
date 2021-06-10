// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyShapeEllipsoid
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public class MyShapeEllipsoid : MyShape
  {
    private BoundingBoxD m_boundaries;
    private Matrix m_scaleMatrix = Matrix.Identity;
    private Matrix m_scaleMatrixInverse = Matrix.Identity;
    private Vector3 m_radius;

    public Vector3 Radius
    {
      get => this.m_radius;
      set
      {
        this.m_radius = value;
        this.m_scaleMatrix = Matrix.CreateScale(this.m_radius);
        this.m_scaleMatrixInverse = Matrix.Invert(this.m_scaleMatrix);
        this.m_boundaries = new BoundingBoxD((Vector3D) -this.Radius, (Vector3D) this.Radius);
      }
    }

    public BoundingBoxD Boundaries => this.m_boundaries;

    public override BoundingBoxD GetWorldBoundaries() => this.m_boundaries.TransformFast(this.Transformation);

    public override BoundingBoxD PeekWorldBoundaries(ref Vector3D targetPosition)
    {
      MatrixD transformation = this.Transformation;
      transformation.Translation = targetPosition;
      return this.m_boundaries.TransformFast(transformation);
    }

    public override BoundingBoxD GetLocalBounds() => this.m_boundaries;

    public override float GetVolume(ref Vector3D voxelPosition)
    {
      if (this.m_inverseIsDirty)
      {
        this.m_inverse = MatrixD.Invert(this.m_transformation);
        this.m_inverseIsDirty = false;
      }
      voxelPosition = Vector3D.Transform(voxelPosition, this.m_inverse);
      Vector3 position = (Vector3) Vector3D.Transform(voxelPosition, this.m_scaleMatrixInverse);
      double num = (double) position.Normalize();
      Vector3 vector3 = Vector3.Transform(position, this.m_scaleMatrix);
      return this.SignedDistanceToDensity((float) voxelPosition.Length() - vector3.Length());
    }

    public override void SendPaintRequest(MyVoxelBase voxel, byte newMaterialIndex) => voxel.RequestVoxelOperationElipsoid(this.Radius, this.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Paint);

    public override void SendCutOutRequest(MyVoxelBase voxel) => voxel.RequestVoxelOperationElipsoid(this.Radius, this.Transformation, (byte) 0, MyVoxelBase.OperationType.Cut);

    public override void SendFillRequest(MyVoxelBase voxel, byte newMaterialIndex) => voxel.RequestVoxelOperationElipsoid(this.Radius, this.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Fill);

    public override void SendRevertRequest(MyVoxelBase voxel) => voxel.RequestVoxelOperationElipsoid(this.Radius, this.Transformation, (byte) 0, MyVoxelBase.OperationType.Revert);

    public override MyShape Clone()
    {
      MyShapeEllipsoid myShapeEllipsoid = new MyShapeEllipsoid();
      myShapeEllipsoid.Transformation = this.Transformation;
      myShapeEllipsoid.Radius = this.Radius;
      return (MyShape) myShapeEllipsoid;
    }
  }
}
