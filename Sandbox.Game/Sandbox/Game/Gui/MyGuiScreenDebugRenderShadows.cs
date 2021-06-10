// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugRenderShadows
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
  [MyDebugScreen("Render", "Shadows")]
  internal class MyGuiScreenDebugRenderShadows : MyGuiScreenDebugBase
  {
    private int m_selectedVolume;
    private MyGuiControlCheckbox m_checkboxHigherRange;
    private MyGuiControlSlider m_sliderFullCoveredDepth;
    private MyGuiControlSlider m_sliderExtCoveredDepth;
    private MyGuiControlSlider m_sliderShadowNormalOffset;

    public MyGuiScreenDebugRenderShadows()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Shadows", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.AddLabel("Setup", Color.Yellow.ToVector4(), 1.2f);
      this.AddCheckBox("Enable Shadows", (Func<bool>) (() => MyRenderProxy.Settings.EnableShadows), (Action<bool>) (newValue => MyRenderProxy.Settings.EnableShadows = newValue));
      this.AddCheckBox("Enable Shadow Blur", (Func<bool>) (() => MySector.ShadowSettings.Data.EnableShadowBlur), (Action<bool>) (newValue => MySector.ShadowSettings.Data.EnableShadowBlur = newValue));
      this.AddCheckBox("Force per-frame updating", MySector.ShadowSettings.Data.UpdateCascadesEveryFrame, (Action<MyGuiControlCheckbox>) (x => MySector.ShadowSettings.Data.UpdateCascadesEveryFrame = x.IsChecked));
      this.AddCheckBox("Shadow cascade usage based skip", MyRenderProxy.Settings.ShadowCascadeUsageBasedSkip, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.ShadowCascadeUsageBasedSkip = x.IsChecked));
      this.AddCheckBox("Use Occlusion culling", !MyRenderProxy.Settings.DisableShadowCascadeOcclusionQueries, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisableShadowCascadeOcclusionQueries = !x.IsChecked));
      this.AddSlider("Max base shadow cascade distance", MySector.ShadowSettings.Data.ShadowCascadeMaxDistance, 1f, 20000f, (Action<MyGuiControlSlider>) (x => MySector.ShadowSettings.Data.ShadowCascadeMaxDistance = x.Value));
      this.AddSlider("Back offset", MySector.ShadowSettings.Data.ShadowCascadeZOffset, 1f, 50000f, (Action<MyGuiControlSlider>) (x => MySector.ShadowSettings.Data.ShadowCascadeZOffset = x.Value));
      this.AddSlider("Spread factor", MySector.ShadowSettings.Data.ShadowCascadeSpreadFactor, 0.0f, 2f, (Action<MyGuiControlSlider>) (x => MySector.ShadowSettings.Data.ShadowCascadeSpreadFactor = x.Value));
      this.AddSlider("LightDirectionChangeDelayMultiplier", MySector.ShadowSettings.Data.LightDirectionChangeDelayMultiplier, 0.0f, 180f, (Action<MyGuiControlSlider>) (x => MySector.ShadowSettings.Data.LightDirectionChangeDelayMultiplier = x.Value));
      this.AddSlider("LightDirectionDifferenceThreshold", MySector.ShadowSettings.Data.LightDirectionDifferenceThreshold, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MySector.ShadowSettings.Data.LightDirectionDifferenceThreshold = x.Value));
      this.AddSlider("Small objects threshold (broken)", 0.0f, 0.0f, 1000f, new Action<MyGuiControlSlider>(this.OnChangeSmallObjectsThreshold));
      this.m_sliderShadowNormalOffset = this.AddSlider("Shadow normal offset", MySector.ShadowSettings.Cascades[this.m_selectedVolume].ShadowNormalOffset, 0.0f, 1f, (Action<MyGuiControlSlider>) (x => MySector.ShadowSettings.Cascades[this.m_selectedVolume].ShadowNormalOffset = x.Value));
      MyGuiControlSlider guiControlSlider = this.AddSlider("ZBias", MySector.ShadowSettings.Data.ZBias, 0.0f, 0.02f, (Action<MyGuiControlSlider>) (x => MySector.ShadowSettings.Data.ZBias = x.Value));
      guiControlSlider.LabelDecimalPlaces = 9;
      float zbias = MySector.ShadowSettings.Data.ZBias;
      guiControlSlider.Value = -1f;
      guiControlSlider.Value = zbias;
      this.AddLabel("Debug", Color.Yellow.ToVector4(), 1.2f);
      this.AddCheckBox("Show shadows", MyRenderProxy.Settings.DisplayShadowsWithDebug, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayShadowsWithDebug = x.IsChecked));
      this.AddCheckBox("Show cascade splits", MyRenderProxy.Settings.DisplayShadowSplitsWithDebug, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayShadowSplitsWithDebug = x.IsChecked));
      this.AddCheckBox("Show cascade splits for particles", MyRenderProxy.Settings.DisplayParticleShadowSplitsWithDebug, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayParticleShadowSplitsWithDebug = x.IsChecked));
      this.AddCheckBox("Show cascade volumes", MyRenderProxy.Settings.DisplayShadowVolumes, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DisplayShadowVolumes = x.IsChecked));
      this.AddCheckBox("Show cascade textures", MyRenderProxy.Settings.DrawCascadeShadowTextures, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawCascadeShadowTextures = x.IsChecked));
      this.AddCheckBox("Show spot textures", MyRenderProxy.Settings.DrawSpotShadowTextures, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.DrawSpotShadowTextures = x.IsChecked));
      this.AddSlider("Zoom to cascade texture", (float) MyRenderProxy.Settings.ZoomCascadeTextureIndex, -1f, 8f, (Action<MyGuiControlSlider>) (x => MyRenderProxy.Settings.ZoomCascadeTextureIndex = (int) x.Value));
      this.AddCheckBox("Freeze camera", MyRenderProxy.Settings.ShadowCameraFrozen, (Action<MyGuiControlCheckbox>) (x => MyRenderProxy.Settings.ShadowCameraFrozen = x.IsChecked));
      for (int index = 0; index < MySector.ShadowSettings.Data.CascadesCount; ++index)
      {
        int captureIndex = index;
        this.AddCheckBox("Freeze cascade " + index.ToString(), MySector.ShadowSettings.ShadowCascadeFrozen[captureIndex], (Action<MyGuiControlCheckbox>) (x =>
        {
          bool[] shadowCascadeFrozen = MySector.ShadowSettings.ShadowCascadeFrozen;
          shadowCascadeFrozen[captureIndex] = x.IsChecked;
          MySector.ShadowSettings.ShadowCascadeFrozen = shadowCascadeFrozen;
        }));
      }
    }

    private void OnChangeSmallObjectsThreshold(MyGuiControlSlider slider)
    {
      float num = slider.Value;
      for (int index = 0; index < MySector.ShadowSettings.Cascades.Length; ++index)
        MySector.ShadowSettings.Cascades[index].SkippingSmallObjectThreshold = num;
    }

    private void SetSelectedVolume(float value)
    {
      int num = MathHelper.Clamp((int) Math.Floor((double) value), 0, MySector.ShadowSettings.Data.CascadesCount - 1);
      if (this.m_selectedVolume == num)
        return;
      this.m_selectedVolume = num;
      MyShadowsSettings.Cascade cascade = MySector.ShadowSettings.Cascades[this.m_selectedVolume];
      this.m_checkboxHigherRange.IsChecked = true;
      this.m_sliderFullCoveredDepth.Value = cascade.FullCoverageDepth;
      this.m_sliderExtCoveredDepth.Value = cascade.ExtendedCoverageDepth;
      this.m_sliderShadowNormalOffset.Value = cascade.ShadowNormalOffset;
    }

    private float GetSelectedVolume() => (float) this.m_selectedVolume;

    protected override void ValueChanged(MyGuiControlBase sender)
    {
      MyRenderProxy.SetSettingsDirty();
      MyRenderProxy.UpdateShadowsSettings(MySector.ShadowSettings);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugRenderShadows);
  }
}
