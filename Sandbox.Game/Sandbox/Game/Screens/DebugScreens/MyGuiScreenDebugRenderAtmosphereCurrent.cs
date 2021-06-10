// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.DebugScreens.MyGuiScreenDebugRenderAtmosphereCurrent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens.DebugScreens
{
  [MyDebugScreen("Render", "Atmosphere Current")]
  public class MyGuiScreenDebugRenderAtmosphereCurrent : MyGuiScreenDebugBase
  {
    private static long m_selectedPlanetEntityID;
    private static MyAtmosphereSettings m_originalAtmosphereSettings;
    private static MyAtmosphereSettings m_atmosphereSettings;
    private static bool m_atmosphereEnabled = true;

    private static MyPlanet SelectedPlanet
    {
      get
      {
        MyEntity entity;
        return MyEntities.TryGetEntityById(MyGuiScreenDebugRenderAtmosphereCurrent.m_selectedPlanetEntityID, out entity) ? entity as MyPlanet : (MyPlanet) null;
      }
    }

    public MyGuiScreenDebugRenderAtmosphereCurrent()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.m_sliderDebugScale = 0.7f;
      this.AddCaption("Atmosphere Current", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.m_currentPosition.Y += 0.01f;
      this.PickPlanet();
      if (MyGuiScreenDebugRenderAtmosphereCurrent.SelectedPlanet == null)
        return;
      if ((double) MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieColorScattering.X == 0.0)
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieColorScattering = new Vector3(MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieScattering);
      if ((double) MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.Intensity == 0.0)
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.Intensity = 1f;
      this.AddLabel("Atmosphere Settings", (Vector4) Color.White, 1f);
      this.AddSlider("Rayleigh Scattering R", 1f, 100f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.RayleighScattering.X), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.RayleighScattering.X = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Rayleigh Scattering G", 1f, 100f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.RayleighScattering.Y), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.RayleighScattering.Y = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Rayleigh Scattering B", 1f, 100f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.RayleighScattering.Z), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.RayleighScattering.Z = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Mie Scattering R", 5f, 150f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieColorScattering.X), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieColorScattering.X = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Mie Scattering G", 5f, 150f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieColorScattering.Y), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieColorScattering.Y = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Mie Scattering B", 5f, 150f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieColorScattering.Z), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieColorScattering.Z = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Rayleigh Height Surfrace", 1f, 50f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.RayleighHeight), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.RayleighHeight = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Rayleigh Height Space", 1f, 25f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.RayleighHeightSpace), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.RayleighHeightSpace = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Rayleigh Transition", 0.1f, 1.5f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.RayleighTransitionModifier), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.RayleighTransitionModifier = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Mie Height", 5f, 200f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieHeight), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieHeight = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Sun size", 0.99f, 1f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieG), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.MieG = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Sea floor modifier", 0.9f, 1.1f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.SeaLevelModifier), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.SeaLevelModifier = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Atmosphere top modifier", 0.9f, 1.1f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.AtmosphereTopModifier), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.AtmosphereTopModifier = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Intensity", 0.1f, 200f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.Intensity), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.Intensity = f;
        this.UpdateAtmosphere();
      }));
      this.AddSlider("Fog Intensity", 0.0f, 1f, (Func<float>) (() => MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.FogIntensity), (Action<float>) (f =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.FogIntensity = f;
        this.UpdateAtmosphere();
      }));
      this.AddColor("Sun Light Color", (Color) MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.SunColor, (Action<MyGuiControlColor>) (v =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.SunColor = (Vector3) v.Color;
        this.UpdateAtmosphere();
      }));
      this.AddColor("Sun Light Specular Color", (Color) MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.SunSpecularColor, (Action<MyGuiControlColor>) (v =>
      {
        MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings.SunSpecularColor = (Vector3) v.Color;
        this.UpdateAtmosphere();
      }));
      this.AddButton(new StringBuilder("Restore"), new Action<MyGuiControlButton>(this.OnRestoreButtonClicked));
      this.AddButton(new StringBuilder("Earth settings"), new Action<MyGuiControlButton>(this.OnResetButtonClicked));
    }

    private void OnRestoreButtonClicked(MyGuiControlButton button)
    {
      MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings = MyGuiScreenDebugRenderAtmosphereCurrent.m_originalAtmosphereSettings;
      this.RecreateControls(false);
      this.UpdateAtmosphere();
    }

    private void OnResetButtonClicked(MyGuiControlButton button)
    {
      MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings = MyAtmosphereSettings.Defaults();
      this.RecreateControls(false);
      this.UpdateAtmosphere();
    }

    private void PickPlanet()
    {
      List<MyLineSegmentOverlapResult<MyEntity>> result = new List<MyLineSegmentOverlapResult<MyEntity>>();
      LineD ray = new LineD(MySector.MainCamera.Position, (Vector3D) MySector.MainCamera.ForwardVector);
      MyGamePruningStructure.GetAllEntitiesInRay(ref ray, result);
      float maxValue = float.MaxValue;
      MyPlanet myPlanet = (MyPlanet) null;
      foreach (MyLineSegmentOverlapResult<MyEntity> segmentOverlapResult in result)
      {
        if (segmentOverlapResult.Element is MyPlanet element && element.EntityId != MyGuiScreenDebugRenderAtmosphereCurrent.m_selectedPlanetEntityID && segmentOverlapResult.Distance < (double) maxValue)
          myPlanet = element;
      }
      if (myPlanet == null)
        return;
      MyGuiScreenDebugRenderAtmosphereCurrent.m_selectedPlanetEntityID = myPlanet.EntityId;
      MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings = myPlanet.AtmosphereSettings;
      MyGuiScreenDebugRenderAtmosphereCurrent.m_originalAtmosphereSettings = MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings;
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

    private void UpdateAtmosphere()
    {
      if (MyGuiScreenDebugRenderAtmosphereCurrent.SelectedPlanet == null)
        return;
      MyGuiScreenDebugRenderAtmosphereCurrent.SelectedPlanet.AtmosphereSettings = MyGuiScreenDebugRenderAtmosphereCurrent.m_atmosphereSettings;
    }
  }
}
