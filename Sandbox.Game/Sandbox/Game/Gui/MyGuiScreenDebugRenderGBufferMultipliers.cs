// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderGBufferMultipliers
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
  [MyDebugScreen("Render", "GBuffer Multipliers")]
  internal class MyGuiScreenDebugRenderGBufferMultipliers : MyGuiScreenDebugBase
  {
    public MyGuiScreenDebugRenderGBufferMultipliers()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("GBuffer Multipliers", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddLabel("Multipliers", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Albedo *", MySector.SunProperties.TextureMultipliers.AlbedoMultiplier, 0.0f, 4f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.AlbedoMultiplier = x.Value));
      this.AddSlider("Albedo +", MySector.SunProperties.TextureMultipliers.AlbedoShift, -1f, 1f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.AlbedoShift = x.Value));
      this.AddSlider("Metalness *", MySector.SunProperties.TextureMultipliers.MetalnessMultiplier, 0.0f, 4f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.MetalnessMultiplier = x.Value));
      this.AddSlider("Metalness +", MySector.SunProperties.TextureMultipliers.MetalnessShift, -1f, 1f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.MetalnessShift = x.Value));
      this.AddSlider("Gloss *", MySector.SunProperties.TextureMultipliers.GlossMultiplier, 0.0f, 4f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.GlossMultiplier = x.Value));
      this.AddSlider("Gloss +", MySector.SunProperties.TextureMultipliers.GlossShift, -1f, 1f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.GlossShift = x.Value));
      this.AddSlider("AO *", MySector.SunProperties.TextureMultipliers.AoMultiplier, 0.0f, 4f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.AoMultiplier = x.Value));
      this.AddSlider("AO +", MySector.SunProperties.TextureMultipliers.AoShift, -1f, 1f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.AoShift = x.Value));
      this.AddSlider("Emissive *", MySector.SunProperties.TextureMultipliers.EmissiveMultiplier, 0.0f, 4f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.EmissiveMultiplier = x.Value));
      this.AddSlider("Emissive +", MySector.SunProperties.TextureMultipliers.EmissiveShift, -1f, 1f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.EmissiveShift = x.Value));
      this.AddSlider("Color Mask *", MySector.SunProperties.TextureMultipliers.ColorMaskMultiplier, 0.0f, 4f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.ColorMaskMultiplier = x.Value));
      this.AddSlider("Color Mask +", MySector.SunProperties.TextureMultipliers.ColorMaskShift, -1f, 1f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.ColorMaskShift = x.Value));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Colorize", Color.Yellow.ToVector4(), 1.2f);
      this.AddSlider("Color Hue", MySector.SunProperties.TextureMultipliers.ColorizeHSV.X, -1f, 1f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.ColorizeHSV.X = x.Value));
      this.AddSlider("Color Saturation", MySector.SunProperties.TextureMultipliers.ColorizeHSV.Y, -1f, 1f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.ColorizeHSV.Y = x.Value));
      this.AddSlider("Color Value", MySector.SunProperties.TextureMultipliers.ColorizeHSV.Z, -1f, 1f, (Action<MyGuiControlSlider>) (x => MySector.SunProperties.TextureMultipliers.ColorizeHSV.Z = x.Value));
      this.AddLabel("Glass", Color.Yellow.ToVector4(), 1.2f);
      this.AddColor("Color", (Color) MyRenderProxy.Settings.TransparentColorMultiplier, (Action<MyGuiControlColor>) (v =>
      {
        Vector3 color = (Vector3) v.Color;
        MyRenderProxy.Settings.TransparentColorMultiplier.X = color.X;
        MyRenderProxy.Settings.TransparentColorMultiplier.Y = color.Y;
        MyRenderProxy.Settings.TransparentColorMultiplier.Z = color.Z;
        MyRenderProxy.SetSettingsDirty();
      }));
      this.AddSlider("Alpha", MyRenderProxy.Settings.TransparentColorMultiplier.W, 0.0f, 10f, (Action<MyGuiControlSlider>) (x => MyRenderProxy.Settings.TransparentColorMultiplier.W = x.Value));
      this.AddSlider("Reflectivity", MyRenderProxy.Settings.TransparentReflectivityMultiplier, 0.0f, 10f, (Action<MyGuiControlSlider>) (x => MyRenderProxy.Settings.TransparentReflectivityMultiplier = x.Value));
      this.AddSlider("Fresnel", MyRenderProxy.Settings.TransparentFresnelMultiplier, 0.0f, 10f, (Action<MyGuiControlSlider>) (x => MyRenderProxy.Settings.TransparentFresnelMultiplier = x.Value));
      this.AddSlider("Gloss", MyRenderProxy.Settings.TransparentGlossMultiplier, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MyRenderProxy.Settings.TransparentGlossMultiplier = x.Value));
      this.m_currentPosition.Y += 0.01f;
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderGBufferMultipliers);

    protected override void ValueChanged(MyGuiControlBase sender) => MyRenderProxy.SetSettingsDirty();
  }
}
