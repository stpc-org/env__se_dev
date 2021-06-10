// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyBrushSphere
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
  public class MyBrushSphere : IMyVoxelBrush
  {
    public static MyBrushSphere Static = new MyBrushSphere();
    private MyShapeSphere m_shape;
    private MatrixD m_transform;
    private MyBrushGUIPropertyNumberSlider m_radius;
    private List<MyGuiControlBase> m_list;

    private MyBrushSphere()
    {
      this.m_shape = new MyShapeSphere();
      this.m_shape.Radius = this.MinScale;
      this.m_transform = MatrixD.Identity;
      this.m_radius = new MyBrushGUIPropertyNumberSlider(this.m_shape.Radius, this.MinScale, this.MaxScale, 0.5f, MyVoxelBrushGUIPropertyOrder.First, MyCommonTexts.VoxelHandProperty_Sphere_Radius);
      this.m_radius.SliderValue.MinimumStepOverride = new float?(0.005f);
      this.m_radius.ValueChanged += new Action(this.RadiusChanged);
      this.m_list = new List<MyGuiControlBase>();
      this.m_radius.AddControlsToList(this.m_list);
    }

    private void RadiusChanged() => this.m_shape.Radius = this.m_radius.Value;

    public float MinScale => 1.5f;

    public float MaxScale => MySessionComponentVoxelHand.GRID_SIZE * 40f;

    public bool AutoRotate => false;

    public string SubtypeName => "Sphere";

    public void Fill(MyVoxelBase map, byte matId) => MyVoxelGenerator.RequestFillInShape((IMyVoxelBase) map, (IMyVoxelShape) this.m_shape, matId);

    public void Paint(MyVoxelBase map, byte matId) => MyVoxelGenerator.RequestPaintInShape((IMyVoxelBase) map, (IMyVoxelShape) this.m_shape, matId);

    public void CutOut(MyVoxelBase map) => MyVoxelGenerator.RequestCutOutShape((IMyVoxelBase) map, (IMyVoxelShape) this.m_shape);

    public void Revert(MyVoxelBase map) => MyVoxelGenerator.RequestRevertShape((IMyVoxelBase) map, (IMyVoxelShape) this.m_shape);

    public Vector3D GetPosition() => this.m_transform.Translation;

    public void SetPosition(ref Vector3D targetPosition)
    {
      this.m_shape.Center = targetPosition;
      this.m_transform.Translation = targetPosition;
    }

    public void SetRotation(ref MatrixD rotationMat)
    {
    }

    public BoundingBoxD GetBoundaries() => this.m_shape.GetWorldBoundaries();

    public BoundingBoxD PeekWorldBoundingBox(ref Vector3D targetPosition) => this.m_shape.PeekWorldBoundaries(ref targetPosition);

    public BoundingBoxD GetWorldBoundaries() => this.m_shape.GetWorldBoundaries();

    public void Draw(ref Color color) => MySimpleObjectDraw.DrawTransparentSphere(ref this.m_transform, this.m_shape.Radius, ref color, MySimpleObjectRasterizer.Solid, 20, blendType: MyBillboard.BlendTypeEnum.LDR);

    public List<MyGuiControlBase> GetGuiControls() => this.m_list;

    public bool ShowRotationGizmo() => false;

    public void ScaleShapeUp()
    {
      MySliderScaleHelper.ScaleSliderUp(ref this.m_radius);
      this.RadiusChanged();
    }

    public void ScaleShapeDown()
    {
      MySliderScaleHelper.ScaleSliderDown(ref this.m_radius);
      this.RadiusChanged();
    }

    public string BrushIcon => "Textures\\GUI\\Icons\\Voxelhand_Sphere.dds";
  }
}
