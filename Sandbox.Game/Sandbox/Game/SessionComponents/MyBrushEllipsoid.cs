// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyBrushEllipsoid
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
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
  public class MyBrushEllipsoid : IMyVoxelBrush
  {
    public static MyBrushEllipsoid Static = new MyBrushEllipsoid();
    private MyShapeEllipsoid m_shape;
    private MatrixD m_transform;
    private MyBrushGUIPropertyNumberSlider m_radiusX;
    private MyBrushGUIPropertyNumberSlider m_radiusY;
    private MyBrushGUIPropertyNumberSlider m_radiusZ;
    private List<MyGuiControlBase> m_list;

    private MyBrushEllipsoid()
    {
      this.m_shape = new MyShapeEllipsoid();
      this.m_transform = MatrixD.Identity;
      float valueStep = 0.25f;
      this.m_radiusX = new MyBrushGUIPropertyNumberSlider(this.MinScale, this.MinScale, this.MaxScale, valueStep, MyVoxelBrushGUIPropertyOrder.First, MyStringId.GetOrCompute("Radius X"));
      this.m_radiusX.SliderValue.MinimumStepOverride = new float?(0.005f);
      this.m_radiusX.ValueChanged += new Action(this.RadiusChanged);
      this.m_radiusY = new MyBrushGUIPropertyNumberSlider(this.MinScale, this.MinScale, this.MaxScale, valueStep, MyVoxelBrushGUIPropertyOrder.Second, MyStringId.GetOrCompute("Radius Y"));
      this.m_radiusY.SliderValue.MinimumStepOverride = new float?(0.005f);
      this.m_radiusY.ValueChanged += new Action(this.RadiusChanged);
      this.m_radiusZ = new MyBrushGUIPropertyNumberSlider(this.MinScale, this.MinScale, this.MaxScale, valueStep, MyVoxelBrushGUIPropertyOrder.Third, MyStringId.GetOrCompute("Radius Z"));
      this.m_radiusZ.SliderValue.MinimumStepOverride = new float?(0.005f);
      this.m_radiusZ.ValueChanged += new Action(this.RadiusChanged);
      this.m_list = new List<MyGuiControlBase>();
      this.m_radiusX.AddControlsToList(this.m_list);
      this.m_radiusY.AddControlsToList(this.m_list);
      this.m_radiusZ.AddControlsToList(this.m_list);
      this.RecomputeShape();
    }

    private void RadiusChanged() => this.RecomputeShape();

    private void RecomputeShape() => this.m_shape.Radius = new Vector3(this.m_radiusX.Value, this.m_radiusY.Value, this.m_radiusZ.Value);

    public float MinScale => 0.25f;

    public float MaxScale => MySessionComponentVoxelHand.GRID_SIZE * 40f;

    public bool AutoRotate => false;

    public string SubtypeName => "Ellipsoid";

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

    public void Draw(ref Color color)
    {
      BoundingBoxD boundaries = this.m_shape.Boundaries;
      MySimpleObjectDraw.DrawTransparentBox(ref this.m_transform, ref boundaries, ref color, MySimpleObjectRasterizer.Solid, 1, 0.04f, blendType: MyBillboard.BlendTypeEnum.LDR);
    }

    public List<MyGuiControlBase> GetGuiControls() => this.m_list;

    public bool ShowRotationGizmo() => true;

    public void ScaleShapeUp()
    {
      MySliderScaleHelper.ScaleSliderUp(ref this.m_radiusX);
      MySliderScaleHelper.ScaleSliderUp(ref this.m_radiusY);
      MySliderScaleHelper.ScaleSliderUp(ref this.m_radiusZ);
      this.RecomputeShape();
    }

    public void ScaleShapeDown()
    {
      MySliderScaleHelper.ScaleSliderDown(ref this.m_radiusX);
      MySliderScaleHelper.ScaleSliderDown(ref this.m_radiusY);
      MySliderScaleHelper.ScaleSliderDown(ref this.m_radiusZ);
      this.RecomputeShape();
    }

    public string BrushIcon => "Textures\\GUI\\Icons\\Voxelhand_Sphere.dds";
  }
}
