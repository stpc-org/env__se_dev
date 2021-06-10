// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderEnvironmentShadows
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Render", "Environment Shadows")]
  internal class MyGuiScreenDebugRenderEnvironmentShadows : MyGuiScreenDebugBase
  {
    private static float timeOfDay;
    private static TimeSpan? OriginalTime;

    public MyGuiScreenDebugRenderEnvironmentShadows()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Environment Shadows", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      MySunProperties sunProperties = MySector.SunProperties;
      this.AddLabel("Sun", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Time of day", 0.0f, MySession.Static.Settings.SunRotationIntervalMinutes, (Func<float>) (() => MyTimeOfDayHelper.TimeOfDay), new Action<float>(MyTimeOfDayHelper.UpdateTimeOfDay));
      this.m_currentPosition.Y += 0.01f;
      this.AddSlider("Shadow fadeout", MySector.SunProperties.EnvironmentLight.ShadowFadeoutMultiplier, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.EnvironmentLight.ShadowFadeoutMultiplier = x.Value));
      this.AddSlider("Env Shadow fadeout", MySector.SunProperties.EnvironmentLight.EnvShadowFadeoutMultiplier, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.EnvironmentLight.EnvShadowFadeoutMultiplier = x.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Ambient Occlusion", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("IndirectLight", MySector.SunProperties.EnvironmentLight.AOIndirectLight, 0.0f, 2f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.AOIndirectLight = v.Value));
      this.AddSlider("DirLight", MySector.SunProperties.EnvironmentLight.AODirLight, 0.0f, 2f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.AODirLight = v.Value));
      this.AddSlider("AOPointLight", MySector.SunProperties.EnvironmentLight.AOPointLight, 0.0f, 2f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.AOPointLight = v.Value));
      this.AddSlider("AOSpotLight", MySector.SunProperties.EnvironmentLight.AOSpotLight, 0.0f, 2f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.AOSpotLight = v.Value));
      this.m_currentPosition.Y += 0.01f;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderEnvironmentShadows);
  }
}
