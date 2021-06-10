// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyShapeCapsule
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public class MyShapeCapsule : MyShape, IMyVoxelShapeCapsule, IMyVoxelShape
  {
    public Vector3D A;
    public Vector3D B;
    public float Radius;

    public override BoundingBoxD GetWorldBoundaries() => new BoundingBoxD(this.A - (double) this.Radius, this.B + this.Radius).TransformFast(this.Transformation);

    public override BoundingBoxD PeekWorldBoundaries(ref Vector3D targetPosition)
    {
      MatrixD transformation = this.Transformation;
      transformation.Translation = targetPosition;
      return new BoundingBoxD(this.A - (double) this.Radius, this.B + this.Radius).TransformFast(transformation);
    }

    public override BoundingBoxD GetLocalBounds() => new BoundingBoxD(this.A - (double) this.Radius, this.B + this.Radius);

    public override float GetVolume(ref Vector3D voxelPosition)
    {
      if (this.m_inverseIsDirty)
      {
        this.m_inverse = MatrixD.Invert(this.m_transformation);
        this.m_inverseIsDirty = false;
      }
      voxelPosition = Vector3D.Transform(voxelPosition, this.m_inverse);
      Vector3D vector3D = voxelPosition - this.A;
      Vector3D v = this.B - this.A;
      double num = MathHelper.Clamp(vector3D.Dot(ref v) / v.LengthSquared(), 0.0, 1.0);
      return this.SignedDistanceToDensity((float) (vector3D - v * num).Length() - this.Radius);
    }

    public override void SendPaintRequest(MyVoxelBase voxel, byte newMaterialIndex) => voxel.RequestVoxelOperationCapsule(this.A, this.B, this.Radius, this.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Paint);

    public override void SendCutOutRequest(MyVoxelBase voxel) => voxel.RequestVoxelOperationCapsule(this.A, this.B, this.Radius, this.Transformation, (byte) 0, MyVoxelBase.OperationType.Cut);

    public override void SendFillRequest(MyVoxelBase voxel, byte newMaterialIndex) => voxel.RequestVoxelOperationCapsule(this.A, this.B, this.Radius, this.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Fill);

    public override void SendRevertRequest(MyVoxelBase voxel) => voxel.RequestVoxelOperationCapsule(this.A, this.B, this.Radius, this.Transformation, (byte) 0, MyVoxelBase.OperationType.Revert);

    public override MyShape Clone()
    {
      MyShapeCapsule myShapeCapsule = new MyShapeCapsule();
      myShapeCapsule.Transformation = this.Transformation;
      myShapeCapsule.A = this.A;
      myShapeCapsule.B = this.B;
      myShapeCapsule.Radius = this.Radius;
      return (MyShape) myShapeCapsule;
    }

    Vector3D IMyVoxelShapeCapsule.A
    {
      get => this.A;
      set => this.A = value;
    }

    Vector3D IMyVoxelShapeCapsule.B
    {
      get => this.B;
      set => this.B = value;
    }

    float IMyVoxelShapeCapsule.Radius
    {
      get => this.Radius;
      set => this.Radius = value;
    }
  }
}
