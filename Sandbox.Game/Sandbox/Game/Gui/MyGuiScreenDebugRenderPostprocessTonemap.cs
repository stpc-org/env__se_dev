// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderPostprocessTonemap
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Render", "Postprocess Tonemap")]
  internal class MyGuiScreenDebugRenderPostprocessTonemap : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugRenderPostprocessTonemap()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Postprocess Tonemap", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddLabel("Tonemapping", Color.Yellow.ToVector4(), 1.2f);
      this.AddCheckBox("Enable", (Func<bool>) (() => MyPostprocessSettingsWrapper.Settings.EnableTonemapping), (Action<bool>) (b => MyPostprocessSettingsWrapper.Settings.EnableTonemapping = b));
      this.AddCheckBox("Display HDR Test", MyRenderProxy.Settings.DisplayHDRTest, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayHDRTest = x.IsChecked));
      this.AddSlider("Constant Luminance", 0.0001f, 2f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.ConstantLuminance), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.ConstantLuminance = f));
      this.AddSlider("Exposure", -5f, 5f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.LuminanceExposure), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.LuminanceExposure = f));
      this.AddSlider("White Point", MyPostprocessSettingsWrapper.Settings.Data.WhitePoint, 0.0f, 15f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.Data.WhitePoint = x.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddSlider("Grain Size", (float) MyPostprocessSettingsWrapper.Settings.Data.GrainSize, 0.0f, 5f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.Data.GrainSize = (int) x.Value));
      this.AddSlider("Grain Amount", MyPostprocessSettingsWrapper.Settings.Data.GrainAmount, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.Data.GrainAmount = x.Value));
      this.AddSlider("Grain Strength", MyPostprocessSettingsWrapper.Settings.Data.GrainStrength, 0.0f, 0.5f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.Data.GrainStrength = x.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddSlider("Chromatic Factor", MyPostprocessSettingsWrapper.Settings.Data.ChromaticFactor, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.Data.ChromaticFactor = x.Value));
      this.AddSlider("Chromatic Iterations", (float) MyPostprocessSettingsWrapper.Settings.ChromaticIterations, 1f, 15f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.ChromaticIterations = (int) x.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddSlider("Vignette Start", MyPostprocessSettingsWrapper.Settings.Data.VignetteStart, 0.0f, 10f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.Data.VignetteStart = x.Value));
      this.AddSlider("Vignette Length", MyPostprocessSettingsWrapper.Settings.Data.VignetteLength, 0.0f, 10f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.Data.VignetteLength = x.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddSlider("Saturation", 0.0f, 5f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.Saturation), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.Saturation = f));
      this.AddSlider("Brightness", 0.0f, 5f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.Brightness), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.Brightness = f));
      this.AddSlider("Brightness Factor R", 0.0f, 1f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorR), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorR = f));
      this.AddSlider("Brightness Factor G", 0.0f, 1f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorG), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorG = f));
      this.AddSlider("Brightness Factor B", 0.0f, 1f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorB), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorB = f));
      this.AddSlider("Contrast", 0.0f, 2f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.Contrast), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.Contrast = f));
      this.m_currentPosition.Y += 0.01f;
      this.AddSlider("Vibrance", -1f, 1f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.Vibrance), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.Vibrance = f));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Sepia", Color.Yellow.ToVector4(), 1.2f);
      this.AddColor("Light Color", (Color) MyPostprocessSettingsWrapper.Settings.Data.LightColor, (Action<MyGuiControlColor>) (v => MyPostprocessSettingsWrapper.Settings.Data.LightColor = (Vector3) v.Color));
      this.AddColor("Dark Color", (Color) MyPostprocessSettingsWrapper.Settings.Data.DarkColor, (Action<MyGuiControlColor>) (v => MyPostprocessSettingsWrapper.Settings.Data.DarkColor = (Vector3) v.Color));
      this.AddSlider("Sepia Strength", 0.0f, 1f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.SepiaStrength), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.SepiaStrength = f));
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderPostprocessTonemap);

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();
  }
}
