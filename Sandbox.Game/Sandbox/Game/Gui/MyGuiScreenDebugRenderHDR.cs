// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderHDR
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Render", "HDR")]
  internal class MyGuiScreenDebugRenderHDR : MyGuiScreenDebugBase
  {
    private static float timeOfDay;
    private static TimeSpan? OriginalTime;

    public MyGuiScreenDebugRenderHDR()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("HDR", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddCheckBox("Enable", MyRenderProxy.Settings.HDREnabled, (Action<MyGuiControlCheckbox>) (x =>
      {
        MyRenderProxy.Settings.HDREnabled = x.IsChecked;
        if (MyRenderProxy.Settings.HDREnabled)
        {
          MyPostprocessSettingsWrapper.Settings.EnableEyeAdaptation = true;
          MyPostprocessSettingsWrapper.Settings.Data.BloomLumaThreshold = 1f;
          MySector.PlanetProperties.AtmosphereIntensityMultiplier = 35f;
          MySector.PlanetProperties.CloudsIntensityMultiplier = 60f;
          MySector.SunProperties.SunIntensity = 150f;
          MyPostprocessSettingsWrapper.MarkDirty();
        }
        else
        {
          MyPostprocessSettingsWrapper.Settings.EnableEyeAdaptation = false;
          MyPostprocessSettingsWrapper.Settings.Data.BloomLumaThreshold = 0.5f;
          MySector.SunProperties.SunIntensity = 5f;
          MySector.PlanetProperties.AtmosphereIntensityMultiplier = 1f;
          MySector.PlanetProperties.CloudsIntensityMultiplier = 1f;
          MyPostprocessSettingsWrapper.MarkDirty();
        }
        MyRenderProxy.SetSettingsDirty();
      }));
      this.AddCheckBox("64bit target", MyRenderProxy.Settings.User.HqTarget, (Action<MyGuiControlCheckbox>) (x =>
      {
        MyRenderProxy.Settings.User.HqTarget = x.IsChecked;
        MyRenderProxy.SetSettingsDirty();
      }));
      this.AddSlider("Time of day", 0.0f, MySession.Static.Settings.SunRotationIntervalMinutes, (Func<float>) (() => MyTimeOfDayHelper.TimeOfDay), new Action<float>(MyTimeOfDayHelper.UpdateTimeOfDay));
      this.AddCheckBox("Unlock PE", false, (Action<MyGuiControlCheckbox>) (x => MyGameService.GetAchievement("Promoted_engineer", (string) null, 0.0f).Unlock()));
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderHDR);
  }
}
