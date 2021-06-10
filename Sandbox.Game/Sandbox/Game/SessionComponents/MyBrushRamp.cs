// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyBrushRamp
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
  public class MyBrushRamp : IMyVoxelBrush
  {
    public static MyBrushRamp Static = new MyBrushRamp();
    private MyShapeRamp m_shape;
    private MatrixD m_transform;
    private MyBrushGUIPropertyNumberSlider m_width;
    private MyBrushGUIPropertyNumberSlider m_height;
    private MyBrushGUIPropertyNumberSlider m_depth;
    private List<MyGuiControlBase> m_list;

    private MyBrushRamp()
    {
      this.m_shape = new MyShapeRamp();
      this.m_transform = MatrixD.Identity;
      this.m_width = new MyBrushGUIPropertyNumberSlider(this.MinScale, this.MinScale, this.MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.First, MyCommonTexts.VoxelHandProperty_Box_Width);
      this.m_width.SliderValue.MinimumStepOverride = new float?(0.005f);
      this.m_width.ValueChanged += new Action(this.RecomputeShape);
      this.m_height = new MyBrushGUIPropertyNumberSlider(this.MinScale, this.MinScale, this.MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.Second, MyCommonTexts.VoxelHandProperty_Box_Height);
      this.m_height.SliderValue.MinimumStepOverride = new float?(0.005f);
      this.m_height.ValueChanged += new Action(this.RecomputeShape);
      this.m_depth = new MyBrushGUIPropertyNumberSlider(this.MinScale, this.MinScale, this.MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.Third, MyCommonTexts.VoxelHandProperty_Box_Depth);
      this.m_depth.SliderValue.MinimumStepOverride = new float?(0.005f);
      this.m_depth.ValueChanged += new Action(this.RecomputeShape);
      this.m_list = new List<MyGuiControlBase>();
      this.m_width.AddControlsToList(this.m_list);
      this.m_height.AddControlsToList(this.m_list);
      this.m_depth.AddControlsToList(this.m_list);
      this.RecomputeShape();
    }

    private void RecomputeShape()
    {
      Vector3D vector3D1 = new Vector3D((double) this.m_width.Value, (double) this.m_height.Value, (double) this.m_depth.Value) * 0.5;
      this.m_shape.Boundaries.Min = -vector3D1;
      this.m_shape.Boundaries.Max = vector3D1;
      Vector3D min = this.m_shape.Boundaries.Min;
      min.X -= this.m_shape.Boundaries.Size.Z;
      Vector3D vector3D2 = Vector3D.Normalize((this.m_shape.Boundaries.Min - min).Cross(this.m_shape.Boundaries.Max - min));
      double num = vector3D2.Dot(min);
      this.m_shape.RampNormal = vector3D2;
      this.m_shape.RampNormalW = -num;
    }

    public float MinScale => 4.5f;

    public float MaxScale => MySessionComponentVoxelHand.GRID_SIZE * 40f;

    public bool AutoRotate => true;

    public string SubtypeName => "Ramp";

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

    public BoundingBoxD GetBoundaries() => this.m_shape.Boundaries;

    public BoundingBoxD PeekWorldBoundingBox(ref Vector3D targetPosition) => this.m_shape.PeekWorldBoundaries(ref targetPosition);

    public BoundingBoxD GetWorldBoundaries() => this.m_shape.GetWorldBoundaries();

    public void Draw(ref Color color) => MySimpleObjectDraw.DrawTransparentRamp(ref this.m_transform, ref this.m_shape.Boundaries, ref color, blendType: MyBillboard.BlendTypeEnum.LDR);

    public List<MyGuiControlBase> GetGuiControls() => this.m_list;

    public bool ShowRotationGizmo() => true;

    public void ScaleShapeUp()
    {
      MySliderScaleHelper.ScaleSliderUp(ref this.m_width);
      MySliderScaleHelper.ScaleSliderUp(ref this.m_height);
      MySliderScaleHelper.ScaleSliderUp(ref this.m_depth);
      this.RecomputeShape();
    }

    public void ScaleShapeDown()
    {
      MySliderScaleHelper.ScaleSliderDown(ref this.m_width);
      MySliderScaleHelper.ScaleSliderDown(ref this.m_height);
      MySliderScaleHelper.ScaleSliderDown(ref this.m_depth);
      this.RecomputeShape();
    }

    public string BrushIcon => "Textures\\GUI\\Icons\\Voxelhand_Ramp.dds";
  }
}
