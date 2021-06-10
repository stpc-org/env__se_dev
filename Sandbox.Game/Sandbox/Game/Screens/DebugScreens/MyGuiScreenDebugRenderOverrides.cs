// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugRenderOverrides
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Linq.Expressions;
using VRage;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Render", "Overrides")]
  internal class MyGuiScreenDebugRenderOverrides : MyGuiScreenDebugBase
  {
    private MyGuiControlCheckbox m_lighting;
    private MyGuiControlCheckbox m_sun;
    private MyGuiControlCheckbox m_backLight;
    private MyGuiControlCheckbox m_pointLights;
    private MyGuiControlCheckbox m_spotLights;
    private MyGuiControlCheckbox m_envLight;
    private MyGuiControlCheckbox m_transparent;
    private MyGuiControlCheckbox m_oit;
    private MyGuiControlCheckbox m_billboardsDynamic;
    private MyGuiControlCheckbox m_billboardsStatic;
    private MyGuiControlCheckbox m_gpuParticles;
    private MyGuiControlCheckbox m_atmosphere;
    private MyGuiControlCheckbox m_cloud;
    private MyGuiControlCheckbox m_models;
    private MyGuiControlCheckbox m_modelsInstanced;
    private MyGuiControlCheckbox m_postprocess;
    private MyGuiControlCheckbox m_ssao;
    private MyGuiControlCheckbox m_bloom;
    private MyGuiControlCheckbox m_fxaa;
    private MyGuiControlCheckbox m_tonemapping;

    public override string GetFriendlyName() => "MyGuiScreenDebugRenderLayers";

    public MyGuiScreenDebugRenderOverrides()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.AddCaption("Overrides", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition.Y += 0.01f;
      this.m_postprocess = this.AddCheckBox("Offscreen Rendering", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.OffscreenRendering)));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Lighting Pass", Color.Yellow.ToVector4(), 1.2f);
      this.m_lighting = this.AddCheckBox("Enabled", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.Lighting)));
      this.m_sun = this.AddCheckBox("Sun", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.Sun)));
      this.m_backLight = this.AddCheckBox("Back light", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.BackLight)));
      this.m_pointLights = this.AddCheckBox("Point lights", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.PointLights)));
      this.m_spotLights = this.AddCheckBox("Spot lights", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.SpotLights)));
      this.m_envLight = this.AddCheckBox("Env light", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.EnvLight)));
      this.m_currentPosition.Y += 0.01f;
      this.AddCheckBox("Shadows", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.Shadows)));
      this.AddCheckBox("Fog", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.Fog)));
      this.AddCheckBox("Flares", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.Flares)));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Transparent Pass", Color.Yellow.ToVector4(), 1.2f);
      this.m_transparent = this.AddCheckBox("Enabled", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.Transparent)));
      this.m_oit = this.AddCheckBox("Order independent", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.OIT)));
      this.m_billboardsDynamic = this.AddCheckBox("Billboards dynamic", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.BillboardsDynamic)));
      this.m_billboardsStatic = this.AddCheckBox("Billboards static", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.BillboardsStatic)));
      this.m_gpuParticles = this.AddCheckBox("GPU Particles", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.GPUParticles)));
      this.m_cloud = this.AddCheckBox("Cloud", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.Clouds)));
      this.m_atmosphere = this.AddCheckBox("Atmosphere", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.Atmosphere)));
      this.m_models = this.AddCheckBox("Models", MyRenderProxy.Settings.DrawTransparentModels, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawTransparentModels = x.IsChecked));
      this.m_modelsInstanced = this.AddCheckBox("Instanced models", MyRenderProxy.Settings.DrawTransparentModelsInstanced, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawTransparentModelsInstanced = x.IsChecked));
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Postprocessing", Color.Yellow.ToVector4(), 1.2f);
      this.m_postprocess = this.AddCheckBox("Enabled", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.Postprocessing)));
      this.m_ssao = this.AddCheckBox("SSAO", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.SSAO)));
      this.m_bloom = this.AddCheckBox("Bloom", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.Bloom)));
      this.m_fxaa = this.AddCheckBox("Fxaa", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.Fxaa)));
      this.m_tonemapping = this.AddCheckBox("Tonemapping", (object) MyRenderProxy.DebugOverrides, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyRenderProxy.DebugOverrides.Tonemapping)));
      this.m_currentPosition.Y += 0.01f;
    }

    protected override void ValueChanged(MyGuiControlBase sender)
    {
      base.ValueChanged(sender);
      MyRenderProxy.UpdateDebugOverrides();
      MyRenderProxy.SetSettingsDirty();
      this.m_sun.Enabled = this.m_lighting.IsChecked;
      this.m_backLight.Enabled = this.m_lighting.IsChecked;
      this.m_pointLights.Enabled = this.m_lighting.IsChecked;
      this.m_spotLights.Enabled = this.m_lighting.IsChecked;
      this.m_envLight.Enabled = this.m_lighting.IsChecked;
      this.m_oit.Enabled = this.m_transparent.IsChecked;
      this.m_billboardsDynamic.Enabled = this.m_transparent.IsChecked;
      this.m_billboardsStatic.Enabled = this.m_transparent.IsChecked;
      this.m_gpuParticles.Enabled = this.m_transparent.IsChecked;
      this.m_atmosphere.Enabled = this.m_transparent.IsChecked;
      this.m_cloud.Enabled = this.m_transparent.IsChecked;
      this.m_models.Enabled = this.m_transparent.IsChecked;
      this.m_modelsInstanced.Enabled = this.m_transparent.IsChecked;
      this.m_ssao.Enabled = this.m_postprocess.IsChecked;
      this.m_bloom.Enabled = this.m_postprocess.IsChecked;
      this.m_fxaa.Enabled = this.m_postprocess.IsChecked;
      this.m_tonemapping.Enabled = this.m_postprocess.IsChecked;
    }
  }
}
