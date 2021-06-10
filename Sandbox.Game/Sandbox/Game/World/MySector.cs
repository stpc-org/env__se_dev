// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MySector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Lights;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Lights;
using VRageRender.Messages;

namespace Sandbox.Game.World
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.Simulation | MyUpdateOrder.AfterSimulation, 800)]
  public class MySector : MySessionComponentBase
  {
    public static Vector3 SunRotationAxis;
    public static MySunProperties SunProperties;
    public static MyFogProperties FogProperties;
    public static MyPlanetProperties PlanetProperties;
    public static MySSAOSettings SSAOSettings;
    public static MyHBAOData HBAOSettings;
    public static MyShadowsSettings ShadowSettings = new MyShadowsSettings();
    public static MySectorLodding Lodding = new MySectorLodding();
    internal static MyParticleDustProperties ParticleDustProperties;
    public static VRageRender.MyImpostorProperties[] ImpostorProperties;
    public static bool UseGenerator = false;
    public static List<int> PrimaryMaterials;
    public static List<int> SecondaryMaterials;
    public static MyEnvironmentDefinition EnvironmentDefinition;
    private static MyCamera m_camera;
    public static bool ResetEyeAdaptation;
    private static MyLight m_sunFlare;

    static MySector() => MySector.SetDefaults();

    public static MyCamera MainCamera
    {
      get => MySector.m_camera;
      private set
      {
        MySector.m_camera = value;
        MyGuiManager.SetCamera(MySector.MainCamera);
        MyTransparentGeometry.SetCamera(MySector.MainCamera);
      }
    }

    private static void SetDefaults()
    {
      MySector.SunProperties = MySunProperties.Default;
      MySector.FogProperties = MyFogProperties.Default;
      MySector.PlanetProperties = MyPlanetProperties.Default;
    }

    public static Vector3 DirectionToSunNormalized => MySector.SunProperties.SunDirectionNormalized;

    public static void InitEnvironmentSettings(
      MyObjectBuilder_EnvironmentSettings environmentBuilder = null)
    {
      if (environmentBuilder != null)
        MySector.EnvironmentDefinition = MyDefinitionManager.Static.GetDefinition<MyEnvironmentDefinition>((MyDefinitionId) environmentBuilder.EnvironmentDefinition);
      else if (MySector.EnvironmentDefinition == null)
        MySector.EnvironmentDefinition = MyDefinitionManager.Static.GetDefinition<MyEnvironmentDefinition>(MyStringHash.GetOrCompute("Default"));
      MyEnvironmentDefinition environmentDefinition = MySector.EnvironmentDefinition;
      MySector.SunProperties = environmentDefinition.SunProperties;
      MySector.FogProperties = environmentDefinition.FogProperties;
      MySector.PlanetProperties = environmentDefinition.PlanetProperties;
      MySector.SSAOSettings = environmentDefinition.SSAOSettings;
      MySector.HBAOSettings = environmentDefinition.HBAOSettings;
      MySector.ShadowSettings.CopyFrom(environmentDefinition.ShadowSettings);
      MySector.SunRotationAxis = MySector.SunProperties.SunRotationAxis;
      MyRenderProxy.UpdateShadowsSettings(MySector.ShadowSettings);
      MySector.Lodding.UpdatePreset(environmentDefinition.LowLoddingSettings, environmentDefinition.MediumLoddingSettings, environmentDefinition.HighLoddingSettings, environmentDefinition.ExtremeLoddingSettings);
      MyPostprocessSettingsWrapper.Settings = environmentDefinition.PostProcessSettings;
      MyPostprocessSettingsWrapper.MarkDirty();
      if (environmentBuilder == null)
        return;
      if (MySession.Static.Settings.EnableSunRotation)
      {
        Vector3 direction;
        Vector3.CreateFromAzimuthAndElevation(environmentBuilder.SunAzimuth, environmentBuilder.SunElevation, out direction);
        double num = (double) direction.Normalize();
        MySector.SunProperties.BaseSunDirectionNormalized = direction;
        MySector.SunProperties.SunDirectionNormalized = direction;
      }
      MySector.FogProperties.FogMultiplier = environmentBuilder.FogMultiplier;
      MySector.FogProperties.FogDensity = environmentBuilder.FogDensity;
      MySector.FogProperties.FogColor = (Vector3) new Color((Vector3) environmentBuilder.FogColor);
    }

    public static void SaveEnvironmentDefinition()
    {
      MySector.EnvironmentDefinition.SunProperties = MySector.SunProperties;
      MySector.EnvironmentDefinition.FogProperties = MySector.FogProperties;
      MySector.EnvironmentDefinition.SSAOSettings = MySector.SSAOSettings;
      MySector.EnvironmentDefinition.HBAOSettings = MySector.HBAOSettings;
      MySector.EnvironmentDefinition.PostProcessSettings = MyPostprocessSettingsWrapper.Settings;
      MySector.EnvironmentDefinition.ShadowSettings.CopyFrom(MySector.ShadowSettings);
      MySector.EnvironmentDefinition.LowLoddingSettings.CopyFrom(MySector.Lodding.LowSettings);
      MySector.EnvironmentDefinition.MediumLoddingSettings.CopyFrom(MySector.Lodding.MediumSettings);
      MySector.EnvironmentDefinition.HighLoddingSettings.CopyFrom(MySector.Lodding.HighSettings);
      MySector.EnvironmentDefinition.ExtremeLoddingSettings.CopyFrom(MySector.Lodding.ExtremeSettings);
      new MyObjectBuilder_Definitions()
      {
        Environments = new MyObjectBuilder_EnvironmentDefinition[1]
        {
          (MyObjectBuilder_EnvironmentDefinition) MySector.EnvironmentDefinition.GetObjectBuilder()
        }
      }.Save(Path.Combine(MyFileSystem.ContentPath, "Data", "Environment.sbc"));
    }

    public static MyObjectBuilder_EnvironmentSettings GetEnvironmentSettings()
    {
      if (MySector.SunProperties.Equals((object) MySector.EnvironmentDefinition.SunProperties) && MySector.FogProperties.Equals((object) MySector.EnvironmentDefinition.FogProperties))
        return (MyObjectBuilder_EnvironmentSettings) null;
      MyObjectBuilder_EnvironmentSettings newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_EnvironmentSettings>();
      float azimuth;
      float elevation;
      Vector3.GetAzimuthAndElevation(MySector.SunProperties.BaseSunDirectionNormalized, out azimuth, out elevation);
      newObject.SunAzimuth = azimuth;
      newObject.SunElevation = elevation;
      newObject.FogMultiplier = MySector.FogProperties.FogMultiplier;
      newObject.FogDensity = MySector.FogProperties.FogDensity;
      newObject.FogColor = (SerializableVector3) MySector.FogProperties.FogColor;
      newObject.EnvironmentDefinition = (SerializableDefinitionId) MySector.EnvironmentDefinition.Id;
      return newObject;
    }

    public override void LoadData()
    {
      MySector.MainCamera = new MyCamera(MySandboxGame.Config.FieldOfView, MySandboxGame.ScreenViewport)
      {
        FarPlaneDistance = MyMultiplayer.Static == null || !Sync.IsServer ? (float) MySession.Static.Settings.ViewDistance : (float) MySession.Static.Settings.SyncDistance
      };
      MyEntities.LoadData();
      this.InitSunGlare();
      MySector.UpdateSunLight();
    }

    private void InitSunGlare()
    {
      MyFlareDefinition definition = MyDefinitionManager.Static.GetDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FlareDefinition), "Sun")) as MyFlareDefinition;
      MySector.m_sunFlare = MyLights.AddLight();
      if (MySector.m_sunFlare == null)
        return;
      MySector.m_sunFlare.Start("Sun");
      MySector.m_sunFlare.GlareOn = MyFakes.SUN_GLARE;
      MySector.m_sunFlare.GlareQuerySize = 100000f;
      MySector.m_sunFlare.GlareQueryFreqMinMs = 0.0f;
      MySector.m_sunFlare.GlareQueryFreqRndMs = 0.0f;
      MySector.m_sunFlare.GlareType = MyGlareTypeEnum.Distant;
      MySector.m_sunFlare.GlareMaxDistance = 2000000f;
      MySector.m_sunFlare.LightOn = false;
      if (definition == null || definition.SubGlares == null)
        return;
      MySector.m_sunFlare.SubGlares = definition.SubGlares;
      MySector.m_sunFlare.GlareIntensity = definition.Intensity;
      MySector.m_sunFlare.GlareSize = definition.Size;
    }

    public static void UpdateSunLight()
    {
      if (MySector.m_sunFlare == null)
        return;
      MySector.m_sunFlare.Position = MySector.MainCamera.Position + MySector.SunProperties.SunDirectionNormalized * 1000000f;
      MySector.m_sunFlare.UpdateLight();
    }

    protected override void UnloadData()
    {
      if (MySector.m_sunFlare != null)
        MyLights.RemoveLight(MySector.m_sunFlare);
      MySector.m_sunFlare = (MyLight) null;
      MyEntities.UnloadData();
      MyGameLogic.UnloadData();
      MySector.MainCamera = (MyCamera) null;
      base.UnloadData();
    }

    public override void UpdateBeforeSimulation()
    {
      MyEntities.UpdateBeforeSimulation();
      MyGameLogic.UpdateBeforeSimulation();
      base.UpdateBeforeSimulation();
    }

    public override void Simulate()
    {
      MyEntities.Simulate();
      base.Simulate();
    }

    public override void UpdateAfterSimulation()
    {
      MyEntities.UpdateAfterSimulation();
      MyGameLogic.UpdateAfterSimulation();
      base.UpdateAfterSimulation();
    }

    public override void UpdatingStopped()
    {
      MyEntities.UpdatingStopped();
      MyGameLogic.UpdatingStopped();
      base.UpdatingStopped();
    }

    public override void Draw()
    {
      base.Draw();
      MyEntities.Draw();
    }

    public override void BeforeStart() => base.BeforeStart();

    public override Type[] Dependencies => new Type[10]
    {
      typeof (MyHud),
      typeof (MyPlanets),
      typeof (MyAntennaSystem),
      typeof (MyGravityProviderSystem),
      typeof (MyIGCSystemSessionComponent),
      typeof (MyUnsafeGridsSessionComponent),
      typeof (MyLights),
      typeof (MyThirdPersonSpectator),
      typeof (MyPhysics),
      typeof (MySessionComponentSafeZones)
    };
  }
}
