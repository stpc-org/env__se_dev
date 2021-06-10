﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyBrushCapsule
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
  public class MyBrushCapsule : IMyVoxelBrush
  {
    public static MyBrushCapsule Static = new MyBrushCapsule();
    private MyShapeCapsule m_shape;
    private MatrixD m_transform;
    private MyBrushGUIPropertyNumberSlider m_radius;
    private MyBrushGUIPropertyNumberSlider m_length;
    private List<MyGuiControlBase> m_list;

    private MyBrushCapsule()
    {
      this.m_shape = new MyShapeCapsule();
      this.m_transform = MatrixD.Identity;
      this.m_radius = new MyBrushGUIPropertyNumberSlider(this.MinScale, this.MinScale, this.MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.First, MyCommonTexts.VoxelHandProperty_Capsule_Radius);
      this.m_radius.SliderValue.MinimumStepOverride = new float?(0.005f);
      this.m_radius.ValueChanged += new Action(this.RecomputeShape);
      this.m_length = new MyBrushGUIPropertyNumberSlider(this.MinScale, this.MinScale, this.MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.Second, MyCommonTexts.VoxelHandProperty_Capsule_Length);
      this.m_length.SliderValue.MinimumStepOverride = new float?(0.005f);
      this.m_length.ValueChanged += new Action(this.RecomputeShape);
      this.m_list = new List<MyGuiControlBase>();
      this.m_radius.AddControlsToList(this.m_list);
      this.m_length.AddControlsToList(this.m_list);
      this.RecomputeShape();
    }

    private void RecomputeShape()
    {
      this.m_shape.Radius = this.m_radius.Value;
      double num = (double) this.m_length.Value * 0.5;
      this.m_shape.A.X = this.m_shape.A.Z = 0.0;
      this.m_shape.B.X = this.m_shape.B.Z = 0.0;
      this.m_shape.A.Y = -num;
      this.m_shape.B.Y = num;
    }

    public float MinScale => 1.5f;

    public float MaxScale => MySessionComponentVoxelHand.GRID_SIZE * 40f;

    public bool AutoRotate => true;

    public string SubtypeName => "Capsule";

    public void Fill(MyVoxelBase map, byte matId) => MyVoxelGenerator.RequestFillInShape((IMyVoxelBase) map, (IMyVoxelShape) this.m_shape, matId);

    public void Paint(MyVoxelBase map, byte matId) => MyVoxelGenerator.RequestPaintInShape((IMyVoxelBase) map, (IMyVoxelShape) this.m_shape, matId);

    public void CutOut(MyVoxelBase map) => MyVoxelGenerator.RequestCutOutShape((IMyVoxelBase) map, (IMyVoxelShape) this.m_shape);

    public void Revert(MyVoxelBase map) => MyVoxelGenerator.RequestRevertShape((IMyVoxelBase) map, (IMyVoxelShape) this.m_shape);

    public Vector3D GetPosition() => this.m_transform.Translation;

    public void SetPosition(ref Vector3D targetPosition)
    {
      this.m_transform.Translation = targetPosition;
      this.m_shape.Transformation = this.m_transform;
    }

    public void SetRotation(ref MatrixD rotationMat)
    {
      if (!rotationMat.IsRotation())
        return;
      this.m_transform.M11 = rotationMat.M11;
      this.m_transform.M12 = rotationMat.M12;
      this.m_transform.M13 = rotationMat.M13;
      this.m_transform.M21 = rotationMat.M21;
      this.m_transform.M22 = rotationMat.M22;
      this.m_transform.M23 = rotationMat.M23;
      this.m_transform.M31 = rotationMat.M31;
      this.m_transform.M32 = rotationMat.M32;
      this.m_transform.M33 = rotationMat.M33;
      this.m_shape.Transformation = this.m_transform;
    }

    public BoundingBoxD GetBoundaries() => this.m_shape.GetWorldBoundaries();

    public BoundingBoxD PeekWorldBoundingBox(ref Vector3D targetPosition) => this.m_shape.PeekWorldBoundaries(ref targetPosition);

    public BoundingBoxD GetWorldBoundaries() => this.m_shape.GetWorldBoundaries();

    public void Draw(ref Color color) => MySimpleObjectDraw.DrawTransparentCapsule(ref this.m_transform, this.m_shape.Radius, this.m_length.Value, ref color, 20, blendType: MyBillboard.BlendTypeEnum.LDR);

    public List<MyGuiControlBase> GetGuiControls() => this.m_list;

    public bool ShowRotationGizmo() => true;

    public void ScaleShapeUp()
    {
      MySliderScaleHelper.ScaleSliderUp(ref this.m_radius);
      MySliderScaleHelper.ScaleSliderUp(ref this.m_length);
      this.RecomputeShape();
    }

    public void ScaleShapeDown()
    {
      MySliderScaleHelper.ScaleSliderDown(ref this.m_radius);
      MySliderScaleHelper.ScaleSliderDown(ref this.m_length);
      this.RecomputeShape();
    }

    public string BrushIcon => "Textures\\GUI\\Icons\\Voxelhand_Capsule.dds";
  }
}
