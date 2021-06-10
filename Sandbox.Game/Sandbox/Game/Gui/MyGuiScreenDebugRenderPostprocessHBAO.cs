// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderPostprocessHBAO
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
  [MyDebugScreen("Render", "Postprocess HBAO")]
  internal class MyGuiScreenDebugRenderPostprocessHBAO : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugRenderPostprocessHBAO()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Postprocess HBAO", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddCheckBox("Use HBAO", MySector.HBAOSettings.Enabled, (Action<MyGuiControlCheckbox>) (state => MySector.HBAOSettings.Enabled = state.IsChecked));
      this.AddCheckBox("Show only HBAO", MyRenderProxy.Settings.DisplayAO, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayAO = x.IsChecked));
      this.AddCheckBox("Show Normals", MyRenderProxy.Settings.DisplayNormals, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayNormals = x.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddSlider("Radius", MySector.HBAOSettings.Radius, 0.0f, 5f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.Radius = state.Value));
      this.AddSlider("Bias", MySector.HBAOSettings.Bias, 0.0f, 0.5f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.Bias = state.Value));
      this.AddSlider("SmallScaleAO", MySector.HBAOSettings.SmallScaleAO, 0.0f, 4f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.SmallScaleAO = state.Value));
      this.AddSlider("LargeScaleAO", MySector.HBAOSettings.LargeScaleAO, 0.0f, 4f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.LargeScaleAO = state.Value));
      this.AddSlider("PowerExponent", MySector.HBAOSettings.PowerExponent, 1f, 8f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.PowerExponent = state.Value));
      this.AddCheckBox("Use Normals", MySector.HBAOSettings.UseGBufferNormals, (Action<MyGuiControlCheckbox>) (state => MySector.HBAOSettings.UseGBufferNormals = state.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("ForegroundAOEnable", MySector.HBAOSettings.ForegroundAOEnable, (Action<MyGuiControlCheckbox>) (state => MySector.HBAOSettings.ForegroundAOEnable = state.IsChecked));
      this.AddSlider("ForegroundViewDepth", MySector.HBAOSettings.ForegroundViewDepth, 0.0f, 1000f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.ForegroundViewDepth = state.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("BackgroundAOEnable", MySector.HBAOSettings.BackgroundAOEnable, (Action<MyGuiControlCheckbox>) (state => MySector.HBAOSettings.BackgroundAOEnable = state.IsChecked));
      this.AddCheckBox("AdaptToFOV", MySector.HBAOSettings.AdaptToFOV, (Action<MyGuiControlCheckbox>) (state => MySector.HBAOSettings.AdaptToFOV = state.IsChecked));
      this.AddSlider("BackgroundViewDepth", MySector.HBAOSettings.BackgroundViewDepth, 0.0f, 1000f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.BackgroundViewDepth = state.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("DepthClampToEdge", MySector.HBAOSettings.DepthClampToEdge, (Action<MyGuiControlCheckbox>) (state => MySector.HBAOSettings.DepthClampToEdge = state.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("DepthThresholdEnable", MySector.HBAOSettings.DepthThresholdEnable, (Action<MyGuiControlCheckbox>) (state => MySector.HBAOSettings.DepthThresholdEnable = state.IsChecked));
      this.AddSlider("DepthThreshold", MySector.HBAOSettings.DepthThreshold, 0.0f, 1000f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.DepthThreshold = state.Value));
      this.AddSlider("DepthThresholdSharpness", MySector.HBAOSettings.DepthThresholdSharpness, 0.0f, 500f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.DepthThresholdSharpness = state.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Use blur", MySector.HBAOSettings.BlurEnable, (Action<MyGuiControlCheckbox>) (state => MySector.HBAOSettings.BlurEnable = state.IsChecked));
      this.AddCheckBox("Radius 4", MySector.HBAOSettings.BlurRadius4, (Action<MyGuiControlCheckbox>) (state => MySector.HBAOSettings.BlurRadius4 = state.IsChecked));
      this.AddSlider("Sharpness", MySector.HBAOSettings.BlurSharpness, 0.0f, 100f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.BlurSharpness = state.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Blur Sharpness Function", MySector.HBAOSettings.BlurSharpnessFunctionEnable, (Action<MyGuiControlCheckbox>) (state => MySector.HBAOSettings.BlurSharpnessFunctionEnable = state.IsChecked));
      this.AddSlider("ForegroundScale", MySector.HBAOSettings.BlurSharpnessFunctionForegroundScale, 0.0f, 100f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.BlurSharpnessFunctionForegroundScale = state.Value));
      this.AddSlider("ForegroundViewDepth", MySector.HBAOSettings.BlurSharpnessFunctionForegroundViewDepth, 0.0f, 1f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.BlurSharpnessFunctionForegroundViewDepth = state.Value));
      this.AddSlider("BackgroundViewDepth", MySector.HBAOSettings.BlurSharpnessFunctionBackgroundViewDepth, 0.0f, 1f, (Action<MyGuiControlSlider>) (state => MySector.HBAOSettings.BlurSharpnessFunctionBackgroundViewDepth = state.Value));
      this.m_currentPosition.Y += 0.01f;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderPostprocessHBAO);

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();
  }
}
