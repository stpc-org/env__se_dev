// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyShapeBox
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public class MyShapeBox : MyShape, IMyVoxelShapeBox, IMyVoxelShape
  {
    public BoundingBoxD Boundaries;

    public override BoundingBoxD GetWorldBoundaries() => this.Boundaries.TransformFast(this.Transformation);

    public override BoundingBoxD PeekWorldBoundaries(ref Vector3D targetPosition)
    {
      MatrixD transformation = this.Transformation;
      transformation.Translation = targetPosition;
      return this.Boundaries.TransformFast(transformation);
    }

    public override BoundingBoxD GetLocalBounds() => this.Boundaries;

    public override float GetVolume(ref Vector3D voxelPosition)
    {
      if (this.m_inverseIsDirty)
      {
        this.m_inverse = MatrixD.Invert(this.m_transformation);
        this.m_inverseIsDirty = false;
      }
      voxelPosition = Vector3D.Transform(voxelPosition, this.m_inverse);
      Vector3D center = this.Boundaries.Center;
      return this.SignedDistanceToDensity((float) (Vector3D.Abs(voxelPosition - center) - (center - this.Boundaries.Min)).Max());
    }

    public override void SendPaintRequest(MyVoxelBase voxel, byte newMaterialIndex) => voxel.RequestVoxelOperationBox(this.Boundaries, this.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Paint);

    public override void SendCutOutRequest(MyVoxelBase voxel) => voxel.RequestVoxelOperationBox(this.Boundaries, this.Transformation, (byte) 0, MyVoxelBase.OperationType.Cut);

    public override void SendFillRequest(MyVoxelBase voxel, byte newMaterialIndex) => voxel.RequestVoxelOperationBox(this.Boundaries, this.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Fill);

    public override void SendRevertRequest(MyVoxelBase voxel) => voxel.RequestVoxelOperationBox(this.Boundaries, this.Transformation, (byte) 0, MyVoxelBase.OperationType.Revert);

    public override MyShape Clone()
    {
      MyShapeBox myShapeBox = new MyShapeBox();
      myShapeBox.Transformation = this.Transformation;
      myShapeBox.Boundaries = this.Boundaries;
      return (MyShape) myShapeBox;
    }

    BoundingBoxD IMyVoxelShapeBox.Boundaries
    {
      get => this.Boundaries;
      set => this.Boundaries = value;
    }
  }
}
