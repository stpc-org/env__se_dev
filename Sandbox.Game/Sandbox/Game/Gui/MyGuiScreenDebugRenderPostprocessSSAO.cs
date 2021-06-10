// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderPostprocessSSAO
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Render", "Postprocess SSAO")]
  internal class MyGuiScreenDebugRenderPostprocessSSAO : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugRenderPostprocessSSAO()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption("Postprocess SSAO", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddCheckBox("Use SSAO", MySector.SSAOSettings.Enabled, (Action<MyGuiControlCheckbox>) (state => MySector.SSAOSettings.Enabled = state.IsChecked));
      this.AddCheckBox("Use blur", MySector.SSAOSettings.UseBlur, (Action<MyGuiControlCheckbox>) (state => MySector.SSAOSettings.UseBlur = state.IsChecked));
      this.AddCheckBox("Show only SSAO", MyRenderProxy.Settings.DisplayAO, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayAO = x.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddSlider("MinRadius", MySector.SSAOSettings.Data.MinRadius, 0.0f, 10f, (Action<MyGuiControlSlider>) (state => MySector.SSAOSettings.Data.MinRadius = state.Value));
      this.AddSlider("MaxRadius", MySector.SSAOSettings.Data.MaxRadius, 0.0f, 1000f, (Action<MyGuiControlSlider>) (state => MySector.SSAOSettings.Data.MaxRadius = state.Value));
      this.AddSlider("RadiusGrowZScale", MySector.SSAOSettings.Data.RadiusGrowZScale, 0.0f, 10f, (Action<MyGuiControlSlider>) (state => MySector.SSAOSettings.Data.RadiusGrowZScale = state.Value));
      this.AddSlider("Falloff", MySector.SSAOSettings.Data.Falloff, 0.0f, 10f, (Action<MyGuiControlSlider>) (state => MySector.SSAOSettings.Data.Falloff = state.Value));
      this.AddSlider("RadiusBias", MySector.SSAOSettings.Data.RadiusBias, 0.0f, 10f, (Action<MyGuiControlSlider>) (state => MySector.SSAOSettings.Data.RadiusBias = state.Value));
      this.AddSlider("Contrast", MySector.SSAOSettings.Data.Contrast, 0.0f, 10f, (Action<MyGuiControlSlider>) (state => MySector.SSAOSettings.Data.Contrast = state.Value));
      this.AddSlider("Normalization", MySector.SSAOSettings.Data.Normalization, 0.0f, 10f, (Action<MyGuiControlSlider>) (state => MySector.SSAOSettings.Data.Normalization = state.Value));
      this.AddSlider("ColorScale", MySector.SSAOSettings.Data.ColorScale, 0.0f, 1f, (Action<MyGuiControlSlider>) (state => MySector.SSAOSettings.Data.ColorScale = state.Value));
      this.m_currentPosition.Y += 0.01f;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderPostprocessSSAO);

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();
  }
}
