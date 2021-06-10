// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderEnvironmentLight
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
  [MyDebugScreen("Render", "Environment Light")]
  internal class MyGuiScreenDebugRenderEnvironmentLight : MyGuiScreenDebugBase
  {
    private static float timeOfDay;
    private static TimeSpan? OriginalTime;

    public MyGuiScreenDebugRenderEnvironmentLight()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Environment Light", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      MySunProperties sunProperties = MySector.SunProperties;
      this.AddLabel("Sun", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Time of day", 0.0f, MySession.Static.Settings.SunRotationIntervalMinutes, (Func<float>) (() => MyTimeOfDayHelper.TimeOfDay), new Action<float>(MyTimeOfDayHelper.UpdateTimeOfDay));
      this.AddSlider("Intensity", MySector.SunProperties.SunIntensity, 0.0f, 200f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.SunIntensity = v.Value));
      this.AddColor("Color", (Color) MySector.SunProperties.EnvironmentLight.SunColor, (Action<MyGuiControlColor>) (v => MySector.SunProperties.EnvironmentLight.SunColor = (Vector3) v.Color));
      this.AddColor("Specular Color", (Color) MySector.SunProperties.EnvironmentLight.SunSpecularColor, (Action<MyGuiControlColor>) (v => MySector.SunProperties.EnvironmentLight.SunSpecularColor = (Vector3) v.Color));
      this.AddSlider("Specular factor", MySector.SunProperties.EnvironmentLight.SunSpecularFactor, 0.0f, 1f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.SunSpecularFactor = v.Value));
      this.AddSlider("Gloss factor", MySector.SunProperties.EnvironmentLight.SunGlossFactor, 0.0f, 5f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.SunGlossFactor = v.Value));
      this.AddSlider("Diffuse factor", MySector.SunProperties.EnvironmentLight.SunDiffuseFactor, 0.0f, 10f, (Action<MyGuiControlSlider>) (v => MySector.SunProperties.EnvironmentLight.SunDiffuseFactor = v.Value));
      this.m_currentPosition.Y += 0.01f;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderEnvironmentLight);
  }
}
