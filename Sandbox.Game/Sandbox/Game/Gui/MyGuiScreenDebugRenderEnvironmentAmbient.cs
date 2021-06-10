// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderEnvironmentAmbient
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
  [MyDebugScreen("Render", "Environment Ambient")]
  internal class MyGuiScreenDebugRenderEnvironmentAmbient : MyGuiScreenDebugBase
  {
    private static float timeOfDay;
    private static TimeSpan? OriginalTime;
    private MyGuiControlSlider m_resolutionSlider;
    private MyGuiControlSlider m_resolutionFilteredSlider;

    public MyGuiScreenDebugRenderEnvironmentAmbient()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Environment Ambient", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddLabel("Indirect light", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Diffuse factor", MySector.SunProperties.EnvironmentLight.AmbientDiffuseFactor, 0.0f, 5f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.AmbientDiffuseFactor = v.Value));
      this.AddCheckBox("Show Indirect Diffuse", MyRenderProxy.Settings.DisplayAmbientDiffuse, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayAmbientDiffuse = x.IsChecked));
      this.AddSlider("Specular factor", MySector.SunProperties.EnvironmentLight.AmbientSpecularFactor, 0.0f, 15f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.AmbientSpecularFactor = v.Value));
      this.AddCheckBox("Show Indirect Specular", MyRenderProxy.Settings.DisplayAmbientSpecular, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayAmbientSpecular = x.IsChecked));
      this.AddSlider("Glass Ambient", MySector.SunProperties.EnvironmentLight.GlassAmbient, 0.0f, 5f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.GlassAmbient = v.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Environment probe", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Distance", MySector.SunProperties.EnvironmentProbe.DrawDistance, 5f, 1000f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentProbe.DrawDistance = v.Value));
      this.m_resolutionSlider = this.AddSlider("Resolution", (float) MySector.SunProperties.EnvMapResolution, 32f, 4096f, (Action<MyGuiControlSlider>) (v =>
      {
        MySector.SunProperties.EnvMapResolution = MathHelper.GetNearestBiggerPowerOfTwo((int) v.Value);
        if (MySector.SunProperties.EnvMapFilteredResolution > MySector.SunProperties.EnvMapResolution)
          this.m_resolutionFilteredSlider.Value = (float) MySector.SunProperties.EnvMapResolution;
        if ((double) v.Value == (double) MySector.SunProperties.EnvMapResolution)
          return;
        v.Value = (float) MySector.SunProperties.EnvMapResolution;
      }));
      this.m_resolutionFilteredSlider = this.AddSlider("Filtered Resolution", (float) MySector.SunProperties.EnvMapFilteredResolution, 32f, 4096f, (Action<MyGuiControlSlider>) (v =>
      {
        MySector.SunProperties.EnvMapFilteredResolution = MathHelper.GetNearestBiggerPowerOfTwo((int) v.Value);
        if (MySector.SunProperties.EnvMapFilteredResolution > MySector.SunProperties.EnvMapResolution)
          this.m_resolutionSlider.Value = (float) MySector.SunProperties.EnvMapFilteredResolution;
        if ((double) v.Value == (double) MySector.SunProperties.EnvMapFilteredResolution)
          return;
        v.Value = (float) MySector.SunProperties.EnvMapFilteredResolution;
      }));
      this.AddSlider("Dim Distance", MySector.SunProperties.EnvironmentLight.ForwardDimDistance, 0.1f, 10f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.ForwardDimDistance = v.Value));
      this.AddSlider("Minimum Ambient", MySector.SunProperties.EnvironmentLight.AmbientForwardPass, 0.0f, 1f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.AmbientForwardPass = v.Value));
      this.AddSlider("Ambient radius", MySector.SunProperties.EnvironmentLight.AmbientRadius, 0.0f, 100f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.AmbientRadius = v.Value));
      this.AddSlider("Ambient Gather radius", MySector.SunProperties.EnvironmentLight.AmbientLightsGatherRadius, 0.0f, 100f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.AmbientLightsGatherRadius = v.Value));
      this.AddSlider("Ambient Gather scale", MySector.SunProperties.EnvironmentProbe.AmbientScale, 0.0f, 1f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentProbe.AmbientScale = v.Value));
      this.AddSlider("Ambient Gather Min clamp", MySector.SunProperties.EnvironmentProbe.AmbientMinClamp, 0.0f, 0.1f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentProbe.AmbientMinClamp = v.Value));
      this.AddSlider("Ambient Gather Max clamp", MySector.SunProperties.EnvironmentProbe.AmbientMaxClamp, 0.0f, 1f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentProbe.AmbientMaxClamp = v.Value));
      this.AddSlider("Atmosphere Intensity", MySector.SunProperties.EnvironmentLight.EnvAtmosphereBrightness, 0.0f, 5f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.EnvAtmosphereBrightness = v.Value));
      this.AddSlider("Timeout", MySector.SunProperties.EnvironmentProbe.TimeOut, 0.0f, 10f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentProbe.TimeOut = v.Value));
      this.AddCheckBox("Render Blocks", MyRenderProxy.Settings.RenderBlocksToEnvProbe, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.RenderBlocksToEnvProbe = x.IsChecked));
      this.AddSlider("Cubemap mipmap", (float) MyRenderProxy.Settings.DisplayEnvProbeMipLevel, 0.0f, 30f, (Action<MyGuiControlSlider>) (v => MyRenderProxy.Settings.DisplayEnvProbeMipLevel = (int) v.Value));
      this.AddCheckBox("DebugDisplay", MyRenderProxy.Settings.DisplayEnvProbe, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayEnvProbe = x.IsChecked));
      this.AddCheckBox("DebugDisplayFar", MyRenderProxy.Settings.DisplayEnvProbeFar, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayEnvProbeFar = x.IsChecked));
      this.AddCheckBox("Use Intensity display", MyRenderProxy.Settings.DisplayEnvProbeIntensities, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayEnvProbeIntensities = x.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Skybox", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Screen Intensity", MySector.SunProperties.EnvironmentLight.SkyboxBrightness, 0.0f, 5f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.SkyboxBrightness = v.Value));
      this.AddSlider("Environment Intensity", MySector.SunProperties.EnvironmentLight.EnvSkyboxBrightness, 0.0f, 50f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.EnvSkyboxBrightness = v.Value));
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderEnvironmentAmbient);

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();
  }
}
