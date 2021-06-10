// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyShapeRamp
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public class MyShapeRamp : MyShape, IMyVoxelShapeRamp, IMyVoxelShape
  {
    public BoundingBoxD Boundaries;
    public Vector3D RampNormal;
    public double RampNormalW;

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
      Vector3D vector3D = Vector3D.Abs(voxelPosition) - this.Boundaries.HalfExtents;
      double num = Vector3D.Dot(voxelPosition, this.RampNormal) + this.RampNormalW;
      return this.SignedDistanceToDensity((float) Math.Max(vector3D.Max(), -num));
    }

    public override void SendPaintRequest(MyVoxelBase voxel, byte newMaterialIndex) => voxel.RequestVoxelOperationRamp(this.Boundaries, this.RampNormal, this.RampNormalW, this.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Paint);

    public override void SendCutOutRequest(MyVoxelBase voxel) => voxel.RequestVoxelOperationRamp(this.Boundaries, this.RampNormal, this.RampNormalW, this.Transformation, (byte) 0, MyVoxelBase.OperationType.Cut);

    public override void SendFillRequest(MyVoxelBase voxel, byte newMaterialIndex) => voxel.RequestVoxelOperationRamp(this.Boundaries, this.RampNormal, this.RampNormalW, this.Transformation, newMaterialIndex, MyVoxelBase.OperationType.Fill);

    public override void SendRevertRequest(MyVoxelBase voxel) => voxel.RequestVoxelOperationRamp(this.Boundaries, this.RampNormal, this.RampNormalW, this.Transformation, (byte) 0, MyVoxelBase.OperationType.Revert);

    public override MyShape Clone()
    {
      MyShapeRamp myShapeRamp = new MyShapeRamp();
      myShapeRamp.Transformation = this.Transformation;
      myShapeRamp.Boundaries = this.Boundaries;
      myShapeRamp.RampNormal = this.RampNormal;
      myShapeRamp.RampNormalW = this.RampNormalW;
      return (MyShape) myShapeRamp;
    }

    BoundingBoxD IMyVoxelShapeRamp.Boundaries
    {
      get => this.Boundaries;
      set => this.Boundaries = value;
    }

    Vector3D IMyVoxelShapeRamp.RampNormal
    {
      get => this.RampNormal;
      set => this.RampNormal = value;
    }

    double IMyVoxelShapeRamp.RampNormalW
    {
      get => this.RampNormalW;
      set => this.RampNormalW = value;
    }
  }
}
