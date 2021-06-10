// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugRenderAtmosphereGlobal
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Render", "Atmosphere Global")]
  public class MyGuiScreenDebugRenderAtmosphereGlobal : MyGuiScreenDebugBase
  {
    private static bool m_atmosphereEnabled = true;

    public MyGuiScreenDebugRenderAtmosphereGlobal()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Atmosphere", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      if (MySession.Static.GetComponent<MySectorWeatherComponent>() != null)
      {
        this.AddCheckBox("Enable Sun Rotation", (Func<bool>) (() => MySession.Static.Settings.EnableSunRotation), (Action<bool>) (x => MySession.Static.Settings.EnableSunRotation = x));
        this.AddSlider("Time of day", 0.0f, MySession.Static == null ? 1f : MySession.Static.Settings.SunRotationIntervalMinutes, (Func<float>) (() => MyTimeOfDayHelper.TimeOfDay), new Action<float>(MyTimeOfDayHelper.UpdateTimeOfDay));
        this.AddSlider("Sun Speed", 0.5f, 60f, (Func<float>) (() => MySession.Static.GetComponent<MySectorWeatherComponent>().RotationInterval), (Action<float>) (f => MySession.Static.GetComponent<MySectorWeatherComponent>().RotationInterval = f));
      }
      this.AddCheckBox("Enable atmosphere", (Func<bool>) (() => MyGuiScreenDebugRenderAtmosphereGlobal.m_atmosphereEnabled), (Action<bool>) (b => this.EnableAtmosphere(b)));
      this.AddSlider("Atmosphere Intensity", MySector.PlanetProperties.AtmosphereIntensityMultiplier, 0.1f, 150f, (Action<MyGuiControlSlider>) (f =>
      {
        MySector.PlanetProperties.AtmosphereIntensityMultiplier = f.Value;
        MyRenderProxy.SetSettingsDirty();
      }));
      this.AddSlider("Atmosphere Intensity in Ambient", MySector.PlanetProperties.AtmosphereIntensityAmbientMultiplier, 0.1f, 150f, (Action<MyGuiControlSlider>) (f =>
      {
        MySector.PlanetProperties.AtmosphereIntensityAmbientMultiplier = f.Value;
        MyRenderProxy.SetSettingsDirty();
      }));
      this.AddSlider("Atmosphere Desaturation in Ambient", MySector.PlanetProperties.AtmosphereDesaturationFactorForward, 0.0f, 1f, (Action<MyGuiControlSlider>) (f =>
      {
        MySector.PlanetProperties.AtmosphereDesaturationFactorForward = f.Value;
        MyRenderProxy.SetSettingsDirty();
      }));
      this.AddSlider("Clouds Intensity", MySector.PlanetProperties.CloudsIntensityMultiplier, 0.5f, 150f, (Action<MyGuiControlSlider>) (f =>
      {
        MySector.PlanetProperties.CloudsIntensityMultiplier = f.Value;
        MyRenderProxy.SetSettingsDirty();
      }));
    }

    protected override void ValueChanged(MyGuiControlBase sender)
    {
      MyRenderPlanetSettings settings = new MyRenderPlanetSettings()
      {
        AtmosphereIntensityMultiplier = MySector.PlanetProperties.AtmosphereIntensityMultiplier,
        AtmosphereIntensityAmbientMultiplier = MySector.PlanetProperties.AtmosphereIntensityAmbientMultiplier,
        AtmosphereDesaturationFactorForward = MySector.PlanetProperties.AtmosphereDesaturationFactorForward,
        CloudsIntensityMultiplier = MySector.PlanetProperties.CloudsIntensityMultiplier
      };
      MyRenderProxy.UpdatePlanetSettings(ref settings);
    }

    private void EnableAtmosphere(bool enabled)
    {
      MyGuiScreenDebugRenderAtmosphereGlobal.m_atmosphereEnabled = enabled;
      MyRenderProxy.EnableAtmosphere(MyGuiScreenDebugRenderAtmosphereGlobal.m_atmosphereEnabled);
    }
  }
}
