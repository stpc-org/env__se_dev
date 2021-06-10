// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderPostprocessBloom
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Render", "Postprocess Bloom")]
  internal class MyGuiScreenDebugRenderPostprocessBloom : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugRenderPostprocessBloom()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Postprocess Bloom", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddLabel("Bloom", Color.Yellow.ToVector4(), 1.2f);
      this.AddCheckBox("Enabled", MyPostprocessSettingsWrapper.Settings.BloomEnabled, (Action<MyGuiControlCheckbox>) (x => MyPostprocessSettingsWrapper.Settings.BloomEnabled = x.IsChecked));
      this.AddCheckBox("Display filter", MyRenderProxy.Settings.DisplayBloomFilter, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayBloomFilter = x.IsChecked));
      this.AddCheckBox("Display min", MyRenderProxy.Settings.DisplayBloomMin, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayBloomMin = x.IsChecked));
      this.AddSlider("Exposure", 0.0f, 10f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.BloomExposure), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.BloomExposure = f));
      this.AddSlider("Luma threshold", 0.0f, 100f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.BloomLumaThreshold), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.BloomLumaThreshold = f));
      this.AddSlider("Emissiveness", 0.0f, 400f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.BloomEmissiveness), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.BloomEmissiveness = f));
      this.AddSlider("Size", 0.0f, 10f, (Func<float>) (() => (float) MyPostprocessSettingsWrapper.Settings.BloomSize), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.BloomSize = (int) f));
      this.AddSlider("Depth slope", 0.0f, 5f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.BloomDepthSlope), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.BloomDepthSlope = f));
      this.AddSlider("Depth strength", 0.0f, 4f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.BloomDepthStrength), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.BloomDepthStrength = f));
      this.AddSlider("Dirt/Bloom Ratio", MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio = x.Value));
      this.AddSlider("Magnitude", 0.0f, 0.1f, (Func<float>) (() => MyPostprocessSettingsWrapper.Settings.Data.BloomMult), (Action<float>) (f => MyPostprocessSettingsWrapper.Settings.Data.BloomMult = f));
      this.AddCheckBox("High Quality Bloom", MyPostprocessSettingsWrapper.Settings.HighQualityBloom, (Action<MyGuiControlCheckbox>) (x => MyPostprocessSettingsWrapper.Settings.HighQualityBloom = x.IsChecked));
      this.AddCheckBox("AntiFlicker Filter", MyPostprocessSettingsWrapper.Settings.BloomAntiFlickerFilter, (Action<MyGuiControlCheckbox>) (x => MyPostprocessSettingsWrapper.Settings.BloomAntiFlickerFilter = x.IsChecked));
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderPostprocessBloom);

    protected override void ValueChanged(MyGuiControlBase sender)
    {
      MyRenderProxy.SetSettingsDirty();
      MyRenderProxy.UpdateDebugOverrides();
    }
  }
}
