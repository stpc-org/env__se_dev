// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderEnvironmentFog
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
  [MyDebugScreen("Render", "Environment Fog")]
  internal class MyGuiScreenDebugRenderEnvironmentFog : MyGuiScreenDebugBase
  {
    private static float timeOfDay;
    private static TimeSpan? OriginalTime;

    public MyGuiScreenDebugRenderEnvironmentFog()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Environment Fog", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddSlider("Fog multiplier", MySector.FogProperties.FogMultiplier, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MySector.FogProperties.FogMultiplier = x.Value));
      this.AddSlider("Fog density", MySector.FogProperties.FogDensity, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MySector.FogProperties.FogDensity = x.Value));
      this.AddColor("Fog color", (Color) MySector.FogProperties.FogColor, (Action<MyGuiControlColor>) (x => MySector.FogProperties.FogColor = (Vector3) x.Color));
      this.AddSlider("Fog Skybox", MySector.FogProperties.FogSkybox, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MySector.FogProperties.FogSkybox = x.Value));
      this.AddSlider("Fog Atmo", MySector.FogProperties.FogAtmo, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MySector.FogProperties.FogAtmo = x.Value));
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderEnvironmentFog);

    protected override void ValueChanged(MyGuiControlBase sender)
    {
      MyRenderFogSettings settings = new MyRenderFogSettings()
      {
        FogMultiplier = MySector.FogProperties.FogMultiplier,
        FogColor = (Color) MySector.FogProperties.FogColor,
        FogDensity = MySector.FogProperties.FogDensity,
        FogSkybox = MySector.FogProperties.FogSkybox,
        FogAtmo = MySector.FogProperties.FogAtmo
      };
      MyRenderProxy.UpdateFogSettings(ref settings);
      MyRenderProxy.SetSettingsDirty();
    }
  }
}
