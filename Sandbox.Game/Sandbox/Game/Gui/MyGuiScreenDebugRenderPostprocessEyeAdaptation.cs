// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderPostprocessEyeAdaptation
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Render", "Postprocess Eye Adaptation")]
  internal class MyGuiScreenDebugRenderPostprocessEyeAdaptation : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugRenderPostprocessEyeAdaptation()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Postprocess Eye Adaptation", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddLabel("Eye adaptation", Color.Yellow.ToVector4(), 1.2f);
      this.AddCheckBox("Enable", (Func<bool>) (() => MyPostprocessSettingsWrapper.Settings.EnableEyeAdaptation), (Action<bool>) (b => MyPostprocessSettingsWrapper.Settings.EnableEyeAdaptation = b));
      this.AddSlider("Tau", 0.0f, 10f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.EyeAdaptationTau), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.EyeAdaptationTau = f));
      this.AddCheckBox("Display Histogram", MyRenderProxy.Settings.DisplayHistogram, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayHistogram = x.IsChecked));
      this.AddCheckBox("Display HDR intensity", MyRenderProxy.Settings.DisplayHdrIntensity, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayHdrIntensity = x.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddSlider("Histogram Log Min", MyPostprocessSettingsWrapper.Settings.HistogramLogMin, -8f, 8f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.HistogramLogMin = x.Value));
      this.AddSlider("Histogram Log Max", MyPostprocessSettingsWrapper.Settings.HistogramLogMax, -8f, 8f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.HistogramLogMax = x.Value));
      this.AddSlider("Histogram Filter Min", MyPostprocessSettingsWrapper.Settings.HistogramFilterMin, 0.0f, 100f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.HistogramFilterMin = x.Value));
      this.AddSlider("Histogram Filter Max", MyPostprocessSettingsWrapper.Settings.HistogramFilterMax, 0.0f, 100f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.HistogramFilterMax = x.Value));
      this.AddSlider("Min Eye Adaptation Log Brightness", MyPostprocessSettingsWrapper.Settings.MinEyeAdaptationLogBrightness, -8f, 8f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.MinEyeAdaptationLogBrightness = x.Value));
      this.AddSlider("Max Eye Adaptation Log Brightness", MyPostprocessSettingsWrapper.Settings.MaxEyeAdaptationLogBrightness, -8f, 8f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.MaxEyeAdaptationLogBrightness = x.Value));
      this.AddSlider("Adaptation Speed Up", MyPostprocessSettingsWrapper.Settings.Data.EyeAdaptationSpeedUp, 0.0f, 4f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.Data.EyeAdaptationSpeedUp = x.Value));
      this.AddSlider("Adaptation Speed Down", MyPostprocessSettingsWrapper.Settings.Data.EyeAdaptationSpeedDown, 0.0f, 4f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.Data.EyeAdaptationSpeedDown = x.Value));
      this.AddCheckBox("Prioritize Screen Center", MyPostprocessSettingsWrapper.Settings.EyeAdaptationPrioritizeScreenCenter, (Action<MyGuiControlCheckbox>) (x => MyPostprocessSettingsWrapper.Settings.EyeAdaptationPrioritizeScreenCenter = x.IsChecked));
      this.AddSlider("Histogram Luminance Threshold", MyPostprocessSettingsWrapper.Settings.HistogramLuminanceThreshold, 0.0f, 0.5f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.HistogramLuminanceThreshold = x.Value));
      this.AddSlider("Histogram Skybox Factor", MyPostprocessSettingsWrapper.Settings.HistogramSkyboxFactor, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.HistogramSkyboxFactor = x.Value));
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderPostprocessEyeAdaptation);

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();
  }
}
