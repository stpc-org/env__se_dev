// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugVoxelMaterials
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VRage;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Game", "Voxel materials")]
  public class MyGuiScreenDebugVoxelMaterials : MyGuiScreenDebugBase
  {
    private MyGuiControlCombobox m_materialsCombo;
    private MyDx11VoxelMaterialDefinition m_selectedVoxelMaterial;
    private bool m_canUpdate;
    private MyGuiControlSlider m_sliderInitialScale;
    private MyGuiControlSlider m_sliderScaleMultiplier;
    private MyGuiControlSlider m_sliderInitialDistance;
    private MyGuiControlSlider m_sliderDistanceMultiplier;
    private MyGuiControlSlider m_sliderTilingScale;
    private MyGuiControlSlider m_sliderFar1Scale;
    private MyGuiControlSlider m_sliderFar1Distance;
    private MyGuiControlSlider m_sliderFar2Scale;
    private MyGuiControlSlider m_sliderFar2Distance;
    private MyGuiControlSlider m_sliderFar3Distance;
    private MyGuiControlSlider m_sliderFar3Scale;
    private MyGuiControlColor m_colorFar3;
    private MyGuiControlSlider m_sliderExtScale;
    private MyGuiControlSlider m_sliderFriction;
    private MyGuiControlSlider m_sliderRestitution;

    public MyGuiScreenDebugVoxelMaterials()
      : base()
      => this.RecreateControls(true);

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugVoxelMaterials);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.BackgroundColor = new Vector4?(new Vector4(1f, 1f, 1f, 0.5f));
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.13f);
      this.AddCaption("Voxel materials", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_materialsCombo = this.AddCombo();
      foreach (MyVoxelMaterialDefinition materialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions().OrderBy<MyVoxelMaterialDefinition, string>((Func<MyVoxelMaterialDefinition, string>) (x => x.Id.SubtypeName)).ToList<MyVoxelMaterialDefinition>())
        this.m_materialsCombo.AddItem((long) materialDefinition.Index, new StringBuilder(materialDefinition.Id.SubtypeName));
      this.m_materialsCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.materialsCombo_OnSelect);
      this.m_currentPosition.Y += 0.01f;
      this.m_sliderInitialScale = this.AddSlider("Initial scale", 1f, 1f, 20f);
      this.m_sliderInitialScale.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_sliderScaleMultiplier = this.AddSlider("Scale multiplier", 1f, 1f, 30f);
      this.m_sliderScaleMultiplier.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_sliderInitialDistance = this.AddSlider("Initial distance", 0.0f, 0.0f, 30f);
      this.m_sliderInitialDistance.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_sliderDistanceMultiplier = this.AddSlider("Distance multiplier", 1f, 1f, 30f);
      this.m_sliderDistanceMultiplier.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_currentPosition.Y += 0.01f;
      this.m_sliderTilingScale = this.AddSlider("Tiling scale", 1f, 1f, 1024f);
      this.m_sliderTilingScale.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_currentPosition.Y += 0.01f;
      this.m_sliderFar1Distance = this.AddSlider("Far1 distance", 0.0f, 0.0f, 500f);
      this.m_sliderFar1Distance.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_sliderFar1Scale = this.AddSlider("Far1 scale", 1f, 1f, 1000f);
      this.m_sliderFar1Scale.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_sliderFar2Distance = this.AddSlider("Far2 distance", 0.0f, 0.0f, 1500f);
      this.m_sliderFar2Distance.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_sliderFar2Scale = this.AddSlider("Far2 scale", 1f, 1f, 2000f);
      this.m_sliderFar2Scale.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_sliderFar3Distance = this.AddSlider("Far3 distance", 0.0f, 0.0f, 40000f);
      this.m_sliderFar3Distance.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_sliderFar3Scale = this.AddSlider("Far3 scale", 1f, 1f, 50000f);
      this.m_sliderFar3Scale.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_currentPosition.Y += 0.01f;
      this.m_sliderExtScale = this.AddSlider("Detail scale (/1000)", 0.0f, 0.0f, 10f);
      this.m_sliderExtScale.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_currentPosition.Y += 0.01f;
      this.m_sliderFriction = this.AddSlider("Friction", 0.0f, 2f, 0.0f, (Func<float>) null, (Action<float>) null);
      this.m_sliderFriction.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_sliderRestitution = this.AddSlider("Restitution", 0.0f, 2f, 0.0f, (Func<float>) null, (Action<float>) null);
      this.m_sliderRestitution.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      this.m_materialsCombo.SelectItemByIndex(0);
      this.m_colorFar3 = this.AddColor("Far3 color", (object) this.m_selectedVoxelMaterial.RenderParams, MemberHelper.GetMember<Vector4>((Expression<Func<Vector4>>) (() => this.m_selectedVoxelMaterial.RenderParams.Far3Color)));
      this.m_colorFar3.SetColor(this.m_selectedVoxelMaterial.RenderParams.Far3Color);
      this.m_colorFar3.OnChange += new Action<MyGuiControlColor>(this.ValueChanged);
      this.m_currentPosition.Y += 0.01f;
      this.AddButton(new StringBuilder("Reload definition"), new Action<MyGuiControlButton>(this.OnReloadDefinition));
    }

    private void materialsCombo_OnSelect()
    {
      this.m_selectedVoxelMaterial = (MyDx11VoxelMaterialDefinition) MyDefinitionManager.Static.GetVoxelMaterialDefinition((byte) this.m_materialsCombo.GetSelectedKey());
      this.UpdateValues();
    }

    private void UpdateValues()
    {
      this.m_canUpdate = false;
      this.m_sliderInitialScale.Value = this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.InitialScale;
      this.m_sliderScaleMultiplier.Value = this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.ScaleMultiplier;
      this.m_sliderInitialDistance.Value = this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.InitialDistance;
      this.m_sliderDistanceMultiplier.Value = this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.DistanceMultiplier;
      this.m_sliderTilingScale.Value = this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.TilingScale;
      this.m_sliderFar1Scale.Value = this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far1Scale;
      this.m_sliderFar1Distance.Value = this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far1Distance;
      this.m_sliderFar2Scale.Value = this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far2Scale;
      this.m_sliderFar2Distance.Value = this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far2Distance;
      this.m_sliderFar3Scale.Value = this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far3Scale;
      this.m_sliderFar3Distance.Value = this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far3Distance;
      if (this.m_colorFar3 != null)
        this.m_colorFar3.SetColor(this.m_selectedVoxelMaterial.RenderParams.Far3Color);
      this.m_sliderExtScale.Value = 1000f * this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.ExtensionDetailScale;
      this.m_sliderFriction.Value = this.m_selectedVoxelMaterial.Friction;
      this.m_sliderRestitution.Value = this.m_selectedVoxelMaterial.Restitution;
      this.m_canUpdate = true;
    }

    private new void ValueChanged(MyGuiControlBase sender)
    {
      if (!this.m_canUpdate)
        return;
      this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.InitialScale = this.m_sliderInitialScale.Value;
      this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.ScaleMultiplier = this.m_sliderScaleMultiplier.Value;
      this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.InitialDistance = this.m_sliderInitialDistance.Value;
      this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.DistanceMultiplier = this.m_sliderDistanceMultiplier.Value;
      this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.TilingScale = this.m_sliderTilingScale.Value;
      this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far1Scale = this.m_sliderFar1Scale.Value;
      this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far1Distance = this.m_sliderFar1Distance.Value;
      this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far2Scale = this.m_sliderFar2Scale.Value;
      this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far2Distance = this.m_sliderFar2Distance.Value;
      this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far3Scale = this.m_sliderFar3Scale.Value;
      this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.Far3Distance = this.m_sliderFar3Distance.Value;
      this.m_selectedVoxelMaterial.RenderParams.Far3Color = (Vector4) this.m_colorFar3.GetColor();
      this.m_selectedVoxelMaterial.RenderParams.StandardTilingSetup.ExtensionDetailScale = this.m_sliderExtScale.Value / 1000f;
      this.m_selectedVoxelMaterial.Friction = this.m_sliderFriction.Value;
      this.m_selectedVoxelMaterial.Restitution = this.m_sliderRestitution.Value;
      this.m_selectedVoxelMaterial.UpdateVoxelMaterial();
    }

    private void OnReloadDefinition(MyGuiControlButton button)
    {
      MyDefinitionManager.Static.ReloadVoxelMaterials();
      this.materialsCombo_OnSelect();
      this.m_selectedVoxelMaterial.UpdateVoxelMaterial();
    }
  }
}
