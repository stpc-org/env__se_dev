// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySectorWeatherComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation, 555, typeof (MyObjectBuilder_SectorWeatherComponent), null, false)]
  public class MySectorWeatherComponent : MySessionComponentBase, IMyWeatherEffects
  {
    public const float ExtremeFreeze = 0.0f;
    public const float Freeze = 0.25f;
    public const float Cozy = 0.5f;
    public const float Hot = 0.75f;
    public const float ExtremeHot = 1f;
    private static MySectorWeatherComponent Static;
    private float m_speed;
    private Vector3 m_sunRotationAxis;
    private Vector3 m_baseSunDirection;
    private Vector3 m_sunDirectionNormalized;
    public static Func<Vector3D, Vector3> CalculateGravityInPoint;
    private const int UPDATE_DELAY = 60;
    private int m_updateCounter = -1;
    private const int MAX_DECOY_RADIUS = 50;
    private readonly float REALISTIC_SOUND_MULTIPLIER_WITH_HELMET = 0.3f;
    private List<MyEntity> m_nearbyEntities = new List<MyEntity>();
    private MyWeatherEffectDefinition m_defaultWeather = new MyWeatherEffectDefinition();
    private MyWeatherEffectDefinition m_sourceWeather;
    private MyWeatherEffectDefinition m_targetWeather;
    private MyWeatherEffectDefinition m_currentWeather;
    private float m_targetVolume;
    private float m_volumeTransitionSpeed = 0.05f;
    private float m_nightValue;
    private float m_currentTransition = 1f;
    private readonly float m_transitionSpeed = 1f / 1000f;
    private Vector3 m_gravityVector;
    private List<MyParticleEffect> m_particleEffects = new List<MyParticleEffect>(256);
    private int m_particleEffectIndex;
    private Vector3D[] m_particleSpread = new Vector3D[5];
    private float m_weatherIntensity;
    private float m_originAltitude;
    private float m_surfaceAltitude;
    private MyPlanet m_closestPlanet;
    private bool m_insideVoxel;
    private bool m_insideGrid;
    private bool m_inCockpit;
    private bool m_insideClosedCockpit;
    private IMySourceVoice m_ambientSound;
    private string m_currentSound = "";
    private List<MyObjectBuilder_WeatherPlanetData> m_weatherPlanetData = new List<MyObjectBuilder_WeatherPlanetData>();
    private List<MySectorWeatherComponent.EffectLightning> m_lightnings = new List<MySectorWeatherComponent.EffectLightning>();
    private static readonly MyStringId m_lightningMaterial = MyStringId.GetOrCompute("WeaponLaser");
    private List<MyEntity> m_foundEntities = new List<MyEntity>();
    private List<MyCubeGrid> m_foundGrids = new List<MyCubeGrid>();
    private List<MyPlayer> m_foundPlayers = new List<MyPlayer>();
    private List<MyEntity> m_decoyGrids = new List<MyEntity>();

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      MySectorWeatherComponent.Static = this;
      MyObjectBuilder_SectorWeatherComponent ob = (MyObjectBuilder_SectorWeatherComponent) sessionComponent;
      this.m_speed = 60f * MySession.Static.Settings.SunRotationIntervalMinutes;
      if (!ob.BaseSunDirection.IsZero)
        this.m_baseSunDirection = (Vector3) ob.BaseSunDirection;
      this.m_sunDirectionNormalized = ob.SunDirectionNormalized.IsZero ? this.CalculateSunDirection() : (Vector3) ob.SunDirectionNormalized;
      this.UpdateOnPause = true;
      this.InitWeather(ob);
    }

    public override void BeforeStart() => this.UpdateSunProperties();

    private void UpdateSunProperties()
    {
      if ((double) Math.Abs(this.m_baseSunDirection.X) + (double) Math.Abs(this.m_baseSunDirection.Y) + (double) Math.Abs(this.m_baseSunDirection.Z) < 0.001)
      {
        this.m_baseSunDirection = MySector.SunProperties.BaseSunDirectionNormalized;
        this.m_sunRotationAxis = MySector.SunProperties.SunRotationAxis;
        if (MySession.Static.Settings.EnableSunRotation && (double) this.m_speed > 0.0)
        {
          TimeSpan elapsedGameTime = MySession.Static.ElapsedGameTime;
          if (elapsedGameTime.Ticks != 0L)
          {
            elapsedGameTime = MySession.Static.ElapsedGameTime;
            Vector3 vector3 = Vector3.Transform(this.m_baseSunDirection, Matrix.CreateFromAxisAngle(this.m_sunRotationAxis, (float) (-6.2831859588623 * (double) ((float) elapsedGameTime.TotalSeconds / this.m_speed))));
            double num = (double) vector3.Normalize();
            this.m_baseSunDirection = vector3;
          }
        }
      }
      else
        this.m_sunRotationAxis = MySector.SunProperties.SunRotationAxis;
      if (MySession.Static.Settings.EnableSunRotation)
        MySector.SunProperties.SunDirectionNormalized = this.m_sunDirectionNormalized = this.CalculateSunDirection();
      else
        MySector.SunProperties.SunDirectionNormalized = this.m_sunDirectionNormalized;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_SectorWeatherComponent objectBuilder = (MyObjectBuilder_SectorWeatherComponent) base.GetObjectBuilder();
      objectBuilder.BaseSunDirection = (SerializableVector3) this.m_baseSunDirection;
      objectBuilder.SunDirectionNormalized = (SerializableVector3) this.m_sunDirectionNormalized;
      objectBuilder.WeatherPlanetData = this.m_weatherPlanetData.ToArray();
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public override void UpdateBeforeSimulation()
    {
      if (!MySession.Static.Settings.EnableSunRotation)
        return;
      Vector3 sunDirection = this.CalculateSunDirection();
      MySector.SunProperties.SunDirectionNormalized = this.m_sunDirectionNormalized = sunDirection;
    }

    private Vector3 CalculateSunDirection()
    {
      Vector3 vector3 = Vector3.Transform(this.m_baseSunDirection, Matrix.CreateFromAxisAngle(this.m_sunRotationAxis, (double) this.m_speed > 0.0 ? 6.283186f * ((float) MySession.Static.ElapsedGameTime.TotalSeconds / this.m_speed) : 0.0f));
      double num = (double) vector3.Normalize();
      return vector3;
    }

    public float RotationInterval
    {
      set => this.m_speed = value;
      get => this.m_speed;
    }

    public static float GetTemperatureInPoint(Vector3D worldPoint)
    {
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(worldPoint);
      if (closestPlanet == null)
        return 0.0f;
      float oxygenInPoint = MyOxygenProviderSystem.GetOxygenInPoint(worldPoint);
      if ((double) oxygenInPoint < 0.00999999977648258)
        return 0.0f;
      float amount1 = MathHelper.Saturate(oxygenInPoint / 0.6f);
      float num1 = (float) Vector3D.Distance(closestPlanet.PositionComp.GetPosition(), worldPoint) / closestPlanet.AverageRadius;
      float amount2 = 1f - (float) Math.Pow(1.0 - ((double) Vector3.Dot(-MySector.SunProperties.SunDirectionNormalized, Vector3.Normalize(worldPoint - closestPlanet.PositionComp.GetPosition())) + 1.0) / 2.0, 0.5);
      MyTemperatureLevel level = MyTemperatureLevel.Cozy;
      if (closestPlanet.Generator != null)
        level = closestPlanet.Generator.DefaultSurfaceTemperature;
      double num2 = (double) MySectorWeatherComponent.LevelToTemperature(level) * (double) MySession.Static.GetComponent<MySectorWeatherComponent>().GetTemperatureMultiplier(worldPoint);
      float num3 = MathHelper.Lerp((float) num2, MathHelper.Min((float) num2, 0.25f), amount2);
      float num4;
      if ((double) num1 < 1.0)
      {
        float num5 = 0.8f;
        float amount3 = MathHelper.Saturate(num1 / num5);
        num4 = MathHelper.Lerp(1f, num3, amount3);
      }
      else
        num4 = MathHelper.Lerp(0.0f, num3, amount1);
      return num4;
    }

    public static MyTemperatureLevel TemperatureToLevel(float temperature)
    {
      if ((double) temperature < 0.125)
        return MyTemperatureLevel.ExtremeFreeze;
      if ((double) temperature < 0.375)
        return MyTemperatureLevel.Freeze;
      if ((double) temperature < 0.625)
        return MyTemperatureLevel.Cozy;
      return (double) temperature < 0.875 ? MyTemperatureLevel.Hot : MyTemperatureLevel.ExtremeHot;
    }

    public static float LevelToTemperature(MyTemperatureLevel level)
    {
      switch (level)
      {
        case MyTemperatureLevel.ExtremeFreeze:
          return 0.0f;
        case MyTemperatureLevel.Freeze:
          return 0.25f;
        case MyTemperatureLevel.Cozy:
          return 0.5f;
        case MyTemperatureLevel.Hot:
          return 0.75f;
        case MyTemperatureLevel.ExtremeHot:
          return 1f;
        default:
          return 0.5f;
      }
    }

    public static bool IsOnDarkSide(Vector3D point)
    {
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(point);
      return closestPlanet != null && MySectorWeatherComponent.IsThereNight(closestPlanet, ref point);
    }

    public static bool IsThereNight(MyPlanet planet, ref Vector3D position)
    {
      Vector3D vector3D = position - planet.PositionComp.GetPosition();
      return vector3D.Length() <= (double) planet.MaximumRadius * 1.10000002384186 && (double) Vector3.Dot(MySector.DirectionToSunNormalized, Vector3.Normalize(vector3D)) < -0.100000001490116;
    }

    public float? FogMultiplierOverride { get; set; }

    public float? FogDensityOverride { get; set; }

    public Vector3? FogColorOverride { get; set; }

    public float? FogSkyboxOverride { get; set; }

    public float? FogAtmoOverride { get; set; }

    public MatrixD? ParticleDirectionOverride { get; set; }

    public Vector3? ParticleVelocityOverride { get; set; }

    public float? SunIntensityOverride { get; set; }

    private void InitWeather(MyObjectBuilder_SectorWeatherComponent ob)
    {
      if (ob.WeatherPlanetData != null)
        this.m_weatherPlanetData = ((IEnumerable<MyObjectBuilder_WeatherPlanetData>) ob.WeatherPlanetData).ToList<MyObjectBuilder_WeatherPlanetData>();
      this.ResetWeather(false);
    }

    public void CreateRandomWeather(MyPlanet planet, bool verbose = false)
    {
      if (planet == null)
      {
        if (!verbose)
          return;
        MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_NoPlanet).ToString(), Color.Red);
      }
      else if (planet.Generator.WeatherGenerators == null || planet.Generator.WeatherGenerators.Count == 0)
      {
        if (!verbose)
          return;
        MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_NoWeatherSystem).ToString(), Color.Red);
      }
      else if (planet.Generator.GlobalWeather)
      {
        if (this.GetWeather(planet.PositionComp.GetPosition()) != null || planet.Generator.WeatherGenerators[0] == null)
          return;
        List<int> intList = new List<int>();
        for (int index1 = 0; index1 < planet.Generator.WeatherGenerators[0].Weathers.Count; ++index1)
        {
          for (int index2 = 0; index2 < planet.Generator.WeatherGenerators[0].Weathers[index1].Weight; ++index2)
            intList.Add(index1);
        }
        if (intList.Count <= 0)
          return;
        int randomInt1 = MyUtils.GetRandomInt(intList.Count);
        int randomInt2 = MyUtils.GetRandomInt(planet.Generator.WeatherGenerators[0].Weathers[intList[randomInt1]].MinLength, planet.Generator.WeatherGenerators[0].Weathers[intList[randomInt1]].MaxLength);
        this.SetWeather(planet.Generator.WeatherGenerators[0].Weathers[intList[randomInt1]].Name, planet.AtmosphereRadius, new Vector3D?(planet.PositionComp.GetPosition()), false, (Vector3D) Vector3.Zero, randomInt2, 1f);
        if (!verbose)
          return;
        MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_RandomWeather).ToString(), Color.Red);
      }
      else
      {
        foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
        {
          if (onlinePlayer != null)
          {
            long? entityId1 = MyGamePruningStructure.GetClosestPlanet(onlinePlayer.GetPosition())?.EntityId;
            long entityId2 = planet.EntityId;
            if (entityId1.GetValueOrDefault() == entityId2 & entityId1.HasValue)
            {
              Vector3D surfacePointGlobal = planet.GetClosestSurfacePointGlobal(onlinePlayer.GetPosition());
              if (!this.GetWeather(surfacePointGlobal, out MyObjectBuilder_WeatherEffect _))
              {
                Vector3 axis = (Vector3) Vector3D.Normalize(planet.PositionComp.GetPosition() - surfacePointGlobal);
                Vector3D perpendicularVector = (Vector3D) MyUtils.GetRandomPerpendicularVector(in axis);
                MyVoxelMaterialDefinition materialAt = planet.GetMaterialAt(ref surfacePointGlobal);
                if (materialAt != null && materialAt.MaterialTypeName != null)
                {
                  foreach (MyWeatherGeneratorSettings weatherGenerator in planet.Generator.WeatherGenerators)
                  {
                    if (weatherGenerator.Voxel.Equals(materialAt.MaterialTypeName))
                    {
                      List<int> intList = new List<int>();
                      for (int index1 = 0; index1 < weatherGenerator.Weathers.Count; ++index1)
                      {
                        for (int index2 = 0; index2 < weatherGenerator.Weathers[index1].Weight; ++index2)
                          intList.Add(index1);
                      }
                      if (intList.Count > 0)
                      {
                        int randomInt1 = MyUtils.GetRandomInt(intList.Count);
                        int randomInt2 = MyUtils.GetRandomInt(weatherGenerator.Weathers[intList[randomInt1]].MinLength, weatherGenerator.Weathers[intList[randomInt1]].MaxLength);
                        int spawnOffset = weatherGenerator.Weathers[intList[randomInt1]].SpawnOffset;
                        float radius = 0.1122755f * planet.AtmosphereRadius;
                        if (!this.GetWeather(surfacePointGlobal - perpendicularVector * ((double) spawnOffset + (double) radius), out MyObjectBuilder_WeatherEffect _) && !this.GetWeather(surfacePointGlobal + perpendicularVector * ((double) spawnOffset + (double) radius), out MyObjectBuilder_WeatherEffect _))
                        {
                          surfacePointGlobal -= perpendicularVector * ((double) spawnOffset + (double) radius);
                          this.SetWeather(weatherGenerator.Weathers[intList[randomInt1]].Name, radius, new Vector3D?(surfacePointGlobal), false, perpendicularVector * (2.0 * ((double) spawnOffset + (double) radius) / (double) randomInt2), randomInt2, 1f);
                          if (verbose)
                            MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_RandomWeather).ToString(), Color.Red);
                        }
                      }
                    }
                  }
                }
              }
              else if (verbose)
                MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_WeatherIncoming).ToString(), Color.Red);
            }
          }
        }
      }
    }

    public void CreateRandomLightning(
      MyObjectBuilder_WeatherEffect weatherEffect,
      MyObjectBuilder_WeatherLightning lightningBuilder,
      bool doHitGrid,
      bool doHitPlayer)
    {
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(weatherEffect.Position);
      MyDefinitionManager.Static.GetWeatherEffect(weatherEffect.Weather);
      Vector3D? hitPosition = new Vector3D?();
      bool doDamage = true;
      if (doHitGrid)
      {
        BoundingSphereD sphere = new BoundingSphereD(weatherEffect.Position, (double) weatherEffect.Radius);
        MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, this.m_foundEntities);
        foreach (MyEntity foundEntity in this.m_foundEntities)
        {
          if (foundEntity != null && foundEntity is MyCubeGrid)
          {
            MyCubeGrid myCubeGrid = foundEntity as MyCubeGrid;
            if (!myCubeGrid.Immune && !myCubeGrid.IsRespawnGrid && (!myCubeGrid.IsPreview && !myCubeGrid.IsRespawnGrid) && (!myCubeGrid.MarkedForClose && myCubeGrid.InScene && !myCubeGrid.Closed))
              this.m_foundGrids.Add(myCubeGrid);
          }
        }
        if (this.m_foundGrids.Count > 0)
        {
          int randomInt1 = MyUtils.GetRandomInt(0, this.m_foundGrids.Count);
          MyCubeGrid foundGrid = this.m_foundGrids[randomInt1];
          int count = foundGrid.CubeBlocks.Count;
          if (count > 0)
          {
            int randomInt2 = MyUtils.GetRandomInt(0, count);
            if (!foundGrid.Immune && !foundGrid.IsRespawnGrid && (!foundGrid.IsPreview && !foundGrid.IsRespawnGrid) && (!foundGrid.MarkedForClose && foundGrid.InScene && !foundGrid.Closed))
              hitPosition = new Vector3D?(this.m_foundGrids[randomInt1].CubeBlocks.ElementAt<MySlimBlock>(randomInt2).WorldAABB.Center);
          }
        }
        this.m_foundEntities.Clear();
        this.m_foundGrids.Clear();
      }
      if (doHitPlayer && !hitPosition.HasValue)
      {
        foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
        {
          AdminSettingsEnum adminSettingsEnum;
          if ((!MySession.Static.IsUserAdmin(onlinePlayer.Id.SteamId) || onlinePlayer.Id.SteamId == 0UL || (!MySession.Static.RemoteAdminSettings.TryGetValue(onlinePlayer.Id.SteamId, out adminSettingsEnum) || !adminSettingsEnum.HasFlag((Enum) AdminSettingsEnum.Untargetable))) && this.InsideWeather(onlinePlayer.GetPosition(), weatherEffect))
            this.m_foundPlayers.Add(onlinePlayer);
        }
        if (this.m_foundPlayers.Count > 0)
          hitPosition = new Vector3D?(this.m_foundPlayers[MyUtils.GetRandomInt(0, this.m_foundPlayers.Count)].GetPosition());
        this.m_foundPlayers.Clear();
      }
      if (!hitPosition.HasValue)
      {
        hitPosition = new Vector3D?(weatherEffect.Position + MyUtils.GetRandomVector3Normalized() * weatherEffect.Radius);
        doDamage = false;
      }
      if (!hitPosition.HasValue || closestPlanet == null)
        return;
      this.ProcessDecoys(ref hitPosition, closestPlanet);
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay(hitPosition.Value - 50f * Vector3.Normalize(closestPlanet.PositionComp.GetPosition() - hitPosition.Value), hitPosition.Value, 15);
      if (nullable.HasValue)
        this.CreateLightning(nullable.Value.Position, lightningBuilder, doDamage);
      else
        this.CreateLightning(hitPosition.Value, lightningBuilder, doDamage);
    }

    internal void RequestLightning(Vector3D cameraTranslation, Vector3D hitPosition) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<Vector3D, Vector3D>((Func<IMyEventOwner, Action<Vector3D, Vector3D>>) (x => new Action<Vector3D, Vector3D>(MySectorWeatherComponent.RequestLightningServer)), cameraTranslation, hitPosition);

    [Event(null, 367)]
    [Reliable]
    [Server]
    private static void RequestLightningServer(Vector3D cameraTranslation, Vector3D hitPosition)
    {
      if (MySession.Static.GetUserPromoteLevel(MyEventContext.Current.Sender.Value) < MyPromoteLevel.Admin)
      {
        MyEventContext.ValidationFailed();
      }
      else
      {
        MyObjectBuilder_WeatherLightning lightning = new MyObjectBuilder_WeatherLightning();
        string weather = MySession.Static.GetComponent<MySectorWeatherComponent>().GetWeather(cameraTranslation);
        if (weather != null)
        {
          MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather);
          if (weatherEffect != null && weatherEffect.Lightning != null)
            lightning = MyDefinitionManager.Static.GetWeatherEffect(weather).Lightning;
        }
        MySession.Static.GetComponent<MySectorWeatherComponent>().CreateLightning(hitPosition, lightning, true);
      }
    }

    private bool ProcessDecoys(ref Vector3D? hitPosition, MyPlanet planet)
    {
      if (planet == null)
        return false;
      this.m_decoyGrids.Clear();
      MyPlanetEnvironmentComponent environmentComponent = planet.Components.Get<MyPlanetEnvironmentComponent>();
      if (environmentComponent != null)
      {
        MyEnvironmentSector sectorForPosition = environmentComponent.GetSectorForPosition(hitPosition.Value);
        if (sectorForPosition != null && sectorForPosition.DataView != null)
        {
          for (int index = 0; index < sectorForPosition.DataView.Items.Count; ++index)
          {
            Sandbox.Game.WorldEnvironment.ItemInfo itemInfo = sectorForPosition.DataView.Items[index];
            MyRuntimeEnvironmentItemInfo environmentItemInfo;
            if (sectorForPosition.EnvironmentDefinition.Items.TryGetValue<MyRuntimeEnvironmentItemInfo>((int) itemInfo.DefinitionIndex, out environmentItemInfo) && environmentItemInfo.Type.Name == "Tree")
            {
              Vector3D vector3D = itemInfo.Position + sectorForPosition.SectorCenter;
              if (Vector3D.DistanceSquared(vector3D, hitPosition.Value) < 400.0)
              {
                hitPosition = new Vector3D?(vector3D);
                return true;
              }
            }
          }
        }
      }
      BoundingSphereD boundingSphereD = new BoundingSphereD(hitPosition.Value, 50.0);
      MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref boundingSphereD, this.m_decoyGrids, MyEntityQueryType.Static);
      bool flag = this.AreDecoysWithinRadius(ref boundingSphereD, planet, ref hitPosition);
      if (!flag)
      {
        MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref boundingSphereD, this.m_decoyGrids, MyEntityQueryType.Dynamic);
        flag = this.AreDecoysWithinRadius(ref boundingSphereD, planet, ref hitPosition);
      }
      if (!flag)
      {
        foreach (MyEntity decoyGrid in this.m_decoyGrids)
        {
          if (decoyGrid != null && decoyGrid is MyCubeGrid myCubeGrid)
          {
            foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
            {
              if (!planet.IsUnderGround(fatBlock.PositionComp.GetPosition()))
              {
                if (fatBlock is MyDecoy myDecoy && myDecoy.IsWorking && Vector3D.Distance(myDecoy.PositionComp.GetPosition(), hitPosition.Value) < 50.0)
                {
                  hitPosition = new Vector3D?(myDecoy.PositionComp.GetPosition());
                  flag = true;
                  break;
                }
                if (fatBlock is MyRadioAntenna myRadioAntenna && myRadioAntenna.IsWorking && Vector3D.Distance(myRadioAntenna.PositionComp.GetPosition(), hitPosition.Value) < (double) myRadioAntenna.GetRodRadius())
                {
                  hitPosition = new Vector3D?(myRadioAntenna.PositionComp.GetPosition());
                  flag = true;
                  break;
                }
              }
            }
          }
        }
      }
      return flag;
    }

    public bool AreDecoysWithinRadius(
      ref BoundingSphereD localSphere,
      MyPlanet planet,
      ref Vector3D? hitPosition)
    {
      MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref localSphere, this.m_decoyGrids, MyEntityQueryType.Static);
      foreach (MyEntity decoyGrid in this.m_decoyGrids)
      {
        if (decoyGrid != null && decoyGrid is MyCubeGrid myCubeGrid && myCubeGrid.Decoys.IsValid)
        {
          foreach (MyDecoy decoy in myCubeGrid.Decoys)
          {
            if (!planet.IsUnderGround(decoy.PositionComp.GetPosition()) && decoy.IsWorking && Vector3D.Distance(decoy.PositionComp.GetPosition(), hitPosition.Value) < (double) decoy.GetSafetyRodRadius())
            {
              hitPosition = new Vector3D?(decoy.PositionComp.GetPosition());
              return true;
            }
          }
        }
      }
      return false;
    }

    public void CreateLightning(
      Vector3D position,
      MyObjectBuilder_WeatherLightning lightning,
      bool doDamage = true)
    {
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
      Vector3D? hitPosition = new Vector3D?(position);
      this.ProcessDecoys(ref hitPosition, closestPlanet);
      MyObjectBuilder_WeatherLightning weatherLightning = lightning != null ? (MyObjectBuilder_WeatherLightning) MyObjectBuilderSerializer.Clone((MyObjectBuilder_Base) lightning) : new MyObjectBuilder_WeatherLightning();
      if (!doDamage)
        weatherLightning.ExplosionRadius = 0.0f;
      weatherLightning.Position = hitPosition.Value;
      this.m_lightnings.Add(new MySectorWeatherComponent.EffectLightning()
      {
        ObjectBuilder = weatherLightning
      });
      this.SyncLightning();
    }

    public string GetWeather(Vector3D position)
    {
      foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in this.m_weatherPlanetData)
      {
        foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetData.Weathers)
        {
          if (this.InsideWeather(position, weather))
            return weather.Weather;
        }
      }
      return "Clear";
    }

    public bool GetWeather(Vector3D position, out MyObjectBuilder_WeatherEffect weatherEffect)
    {
      if (this.m_weatherPlanetData != null)
      {
        foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in this.m_weatherPlanetData)
        {
          if (weatherPlanetData != null && weatherPlanetData.Weathers != null)
          {
            foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetData.Weathers)
            {
              if (weather != null)
              {
                Vector3D startPoint = weather.StartPoint;
                Vector3D endPoint = weather.EndPoint;
                if (startPoint != endPoint && Vector3D.Distance(position, MyUtils.GetClosestPointOnLine(ref startPoint, ref endPoint, ref position)) < (double) weather.Radius)
                {
                  weatherEffect = weather;
                  return true;
                }
                if (startPoint == endPoint && Vector3D.Distance(position, weather.Position) < (double) weather.Radius)
                {
                  weatherEffect = weather;
                  return true;
                }
              }
            }
          }
        }
      }
      weatherEffect = (MyObjectBuilder_WeatherEffect) null;
      return false;
    }

    public List<MyObjectBuilder_WeatherPlanetData> GetWeatherPlanetData() => this.m_weatherPlanetData;

    public bool ReplaceWeather(string weatherEffect, Vector3D? weatherPosition)
    {
      if (string.IsNullOrEmpty(weatherEffect))
        return false;
      foreach (MyWeatherEffectDefinition weatherDefinition in MyDefinitionManager.Static.GetWeatherDefinitions())
      {
        if (weatherDefinition.Public && weatherDefinition.Id.SubtypeName.ToLower() == weatherEffect.ToLower())
        {
          weatherEffect = weatherDefinition.Id.SubtypeName;
          break;
        }
      }
      if (!weatherPosition.HasValue)
        weatherPosition = new Vector3D?(MySector.MainCamera.Position);
      for (int index1 = 0; index1 < this.m_weatherPlanetData.Count; ++index1)
      {
        for (int index2 = this.m_weatherPlanetData[index1].Weathers.Count - 1; index2 > -1; --index2)
        {
          if (Vector3D.Distance(weatherPosition.Value, this.m_weatherPlanetData[index1].Weathers[index2].Position) <= (double) this.m_weatherPlanetData[index1].Weathers[index2].Radius)
          {
            this.m_weatherPlanetData[index1].Weathers[index2].Weather = weatherEffect;
            this.SyncWeathers();
            return true;
          }
        }
      }
      return false;
    }

    public bool SetWeather(
      string weatherEffect,
      float radius,
      Vector3D? weatherPosition,
      bool verbose,
      Vector3D velocity,
      int length = 0,
      float intensity = 1f)
    {
      if (string.IsNullOrEmpty(weatherEffect))
        return false;
      string str = (string) null;
      foreach (MyWeatherEffectDefinition weatherDefinition in MyDefinitionManager.Static.GetWeatherDefinitions())
      {
        if (weatherDefinition.Public && weatherDefinition.Id.SubtypeName.ToLower() == weatherEffect.ToLower())
        {
          str = weatherDefinition.Id.SubtypeName;
          break;
        }
      }
      MyPlanet closestPlanet = this.m_closestPlanet;
      if (!weatherPosition.HasValue)
      {
        if (this.m_closestPlanet == null)
        {
          if (verbose)
            MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_NoPlanet), Color.Red);
          return false;
        }
        Vector3D translation = MySector.MainCamera.WorldMatrix.Translation;
        weatherPosition = new Vector3D?(this.m_closestPlanet.GetClosestSurfacePointGlobal(ref translation));
      }
      else
        closestPlanet = MyGamePruningStructure.GetClosestPlanet(weatherPosition.Value);
      if (closestPlanet == null)
      {
        if (verbose)
          MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_NoPlanet), Color.Red);
        return false;
      }
      if (weatherPosition.HasValue && weatherEffect.ToLower().Equals("clear"))
      {
        if (this.RemoveWeather(weatherPosition.Value))
        {
          if (verbose)
            MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_RemovedWeather), Color.Red);
          return true;
        }
        if (verbose)
          MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_NoWeather), Color.Red);
        return true;
      }
      if (str != null)
      {
        radius = (double) radius != 0.0 ? Math.Abs(radius) : (float) Math.Max(75.0 / 668.0 * (double) closestPlanet.AtmosphereRadius, 5000.0);
        BoundingSphereD boundingSphereD = new BoundingSphereD(weatherPosition.Value, (double) radius);
        for (int index1 = 0; index1 < this.m_weatherPlanetData.Count; ++index1)
        {
          if (this.m_weatherPlanetData[index1].PlanetId == closestPlanet.EntityId)
          {
            for (int index2 = this.m_weatherPlanetData[index1].Weathers.Count - 1; index2 > -1; --index2)
            {
              BoundingSphereD sphere = new BoundingSphereD(this.m_weatherPlanetData[index1].Weathers[index2].Position, (double) this.m_weatherPlanetData[index1].Weathers[index2].Radius);
              if (boundingSphereD.Intersects(sphere))
                this.m_weatherPlanetData[index1].Weathers.RemoveAtFast<MyObjectBuilder_WeatherEffect>(index2);
            }
          }
        }
        MyObjectBuilder_WeatherEffect builderWeatherEffect = new MyObjectBuilder_WeatherEffect()
        {
          Position = weatherPosition.Value,
          Velocity = velocity,
          Weather = str,
          Radius = radius,
          MaxLife = (int) ((double) length / 0.0166666675359011),
          Intensity = intensity,
          StartPoint = weatherPosition.Value
        };
        for (int index = 0; index < this.m_weatherPlanetData.Count; ++index)
        {
          if (this.m_weatherPlanetData[index].PlanetId == closestPlanet.EntityId)
          {
            this.m_weatherPlanetData[index].Weathers.Add(builderWeatherEffect);
            break;
          }
        }
        if ((double) Vector3.Distance((Vector3) MySector.MainCamera.WorldMatrix.Translation, (Vector3) builderWeatherEffect.Position) < (double) builderWeatherEffect.Radius && verbose)
          MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), string.Format(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_SetWeather), (object) str), Color.Red);
        this.SyncWeathers();
        return true;
      }
      if (verbose)
        MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), string.Format(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_NoWeatherFound), (object) weatherEffect), Color.Red);
      return false;
    }

    public bool RemoveWeather(Vector3D position)
    {
      if (this.m_weatherPlanetData != null)
      {
        foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in this.m_weatherPlanetData)
        {
          if (weatherPlanetData.Weathers != null)
          {
            for (int index = 0; index < weatherPlanetData.Weathers.Count; ++index)
            {
              if (this.InsideWeather(position, weatherPlanetData.Weathers[index]))
              {
                weatherPlanetData.Weathers.RemoveAt(index);
                this.SyncWeathers();
                return true;
              }
            }
          }
        }
      }
      return false;
    }

    public void RemoveWeather(MyObjectBuilder_WeatherEffect weatherEffect)
    {
      if (this.m_weatherPlanetData == null)
        return;
      foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in this.m_weatherPlanetData)
      {
        if (weatherPlanetData.Weathers != null)
        {
          for (int index = 0; index < weatherPlanetData.Weathers.Count; ++index)
          {
            if (weatherPlanetData.Weathers[index] == weatherEffect)
            {
              weatherPlanetData.Weathers.RemoveAt(index);
              this.SyncWeathers();
              return;
            }
          }
        }
      }
    }

    public bool InsideWeather(Vector3D position, MyObjectBuilder_WeatherEffect weather) => Vector3D.Distance(position, weather.Position) < (double) weather.Radius;

    private float AdjustIntensity(float weatherIntensity)
    {
      if (this.m_closestPlanet == null)
        return weatherIntensity;
      if ((double) this.m_surfaceAltitude < 0.0)
      {
        float num = (float) (2.0 * ((double) MathHelper.Clamp(-this.m_surfaceAltitude, 0.0f, 100f) / 100.0));
        weatherIntensity *= 1f - Math.Min(num * num, 1f);
      }
      else
      {
        float num = MathHelper.Clamp((float) ((double) this.m_surfaceAltitude * (double) this.m_surfaceAltitude * 0.0399999991059303 / (double) this.m_closestPlanet.AtmosphereRadius - 0.25), 0.0f, 1f);
        weatherIntensity *= 1f - num;
      }
      return weatherIntensity;
    }

    public float GetWeatherIntensity(Vector3D position)
    {
      foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in this.m_weatherPlanetData)
      {
        foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetData.Weathers)
        {
          if (this.InsideWeather(position, weather))
          {
            float num = Vector3.Distance((Vector3) position, (Vector3) weather.Position) / weather.Radius;
            return this.AdjustIntensity((float) (1.0 - (double) num * (double) num * (double) num) * weather.Intensity);
          }
        }
      }
      return 0.0f;
    }

    public float GetWeatherIntensity(Vector3D position, MyObjectBuilder_WeatherEffect weatherEffect)
    {
      if (!this.InsideWeather(position, weatherEffect))
        return 0.0f;
      float num = Vector3.Distance((Vector3) position, (Vector3) weatherEffect.Position) / weatherEffect.Radius;
      return this.AdjustIntensity((float) (1.0 - (double) num * (double) num * (double) num) * weatherEffect.Intensity);
    }

    public float GetOxygenMultiplier(Vector3D position)
    {
      foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in this.m_weatherPlanetData)
      {
        foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetData.Weathers)
        {
          if (weather.Weather != null && (double) Vector3.Distance((Vector3) position, (Vector3) weather.Position) < (double) weather.Radius)
          {
            float num = Vector3.Distance((Vector3) position, (Vector3) weather.Position) / weather.Radius;
            MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
            if (weatherEffect != null)
              return MathHelper.Lerp(1f, weatherEffect.OxygenLevelModifier, (float) (1.0 - (double) num * (double) num * (double) num));
          }
        }
      }
      return 1f;
    }

    public float GetOxygenMultiplier(Vector3D position, MyObjectBuilder_WeatherEffect weatherEffect)
    {
      if ((double) Vector3.Distance((Vector3) position, (Vector3) weatherEffect.Position) < (double) weatherEffect.Radius)
      {
        float num = Vector3.Distance((Vector3) position, (Vector3) weatherEffect.Position) / weatherEffect.Radius;
        MyWeatherEffectDefinition weatherEffect1 = MyDefinitionManager.Static.GetWeatherEffect(weatherEffect.Weather);
        if (weatherEffect1 != null)
          return MathHelper.Lerp(1f, weatherEffect1.OxygenLevelModifier, (float) (1.0 - (double) num * (double) num * (double) num));
      }
      return 1f;
    }

    public float GetSolarMultiplier(Vector3D position)
    {
      foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in this.m_weatherPlanetData)
      {
        foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetData.Weathers)
        {
          if ((double) Vector3.Distance((Vector3) position, (Vector3) weather.Position) < (double) weather.Radius)
          {
            float num = Vector3.Distance((Vector3) position, (Vector3) weather.Position) / weather.Radius;
            MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
            if (weatherEffect != null)
              return MathHelper.Lerp(1f, weatherEffect.SolarOutputModifier, (float) (1.0 - (double) num * (double) num * (double) num));
          }
        }
      }
      return 1f;
    }

    public float GetSolarMultiplier(Vector3D position, MyObjectBuilder_WeatherEffect weather)
    {
      if ((double) Vector3.Distance((Vector3) position, (Vector3) weather.Position) < (double) weather.Radius)
      {
        float num = Vector3.Distance((Vector3) position, (Vector3) weather.Position) / weather.Radius;
        MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
        if (weatherEffect != null)
          return MathHelper.Lerp(1f, weatherEffect.SolarOutputModifier, (float) (1.0 - (double) num * (double) num * (double) num));
      }
      return 1f;
    }

    public float GetTemperatureMultiplier(Vector3D position)
    {
      foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in this.m_weatherPlanetData)
      {
        foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetData.Weathers)
        {
          if ((double) Vector3.Distance((Vector3) position, (Vector3) weather.Position) < (double) weather.Radius)
          {
            float num = Vector3.Distance((Vector3) position, (Vector3) weather.Position) / weather.Radius;
            MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
            if (weatherEffect != null)
              return MathHelper.Lerp(1f, weatherEffect.TemperatureModifier, (float) (1.0 - (double) num * (double) num * (double) num));
          }
        }
      }
      return 1f;
    }

    public float GetTemperatureMultiplier(Vector3D position, MyObjectBuilder_WeatherEffect weather)
    {
      if ((double) Vector3.Distance((Vector3) position, (Vector3) weather.Position) < (double) weather.Radius)
      {
        float num = Vector3.Distance((Vector3) position, (Vector3) weather.Position) / weather.Radius;
        MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
        if (weatherEffect != null)
          return MathHelper.Lerp(1f, weatherEffect.TemperatureModifier, (float) (1.0 - (double) num * (double) num * (double) num));
      }
      return 1f;
    }

    public float GetWindMultiplier(Vector3D position)
    {
      foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in this.m_weatherPlanetData)
      {
        foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetData.Weathers)
        {
          if ((double) Vector3.Distance((Vector3) position, (Vector3) weather.Position) < (double) weather.Radius)
          {
            float num = Vector3.Distance((Vector3) position, (Vector3) weather.Position) / weather.Radius;
            MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
            if (weatherEffect != null)
              return MathHelper.Lerp(1f, weatherEffect.WindOutputModifier, (float) (1.0 - (double) num * (double) num * (double) num));
          }
        }
      }
      return 1f;
    }

    public float GetWindMultiplier(Vector3D position, MyObjectBuilder_WeatherEffect weather)
    {
      if ((double) Vector3.Distance((Vector3) position, (Vector3) weather.Position) < (double) weather.Radius)
      {
        float num = Vector3.Distance((Vector3) position, (Vector3) weather.Position) / weather.Radius;
        MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
        if (weatherEffect != null)
          return MathHelper.Lerp(1f, weatherEffect.WindOutputModifier, (float) (1.0 - (double) num * (double) num * (double) num));
      }
      return 1f;
    }

    public override void UpdateAfterSimulation()
    {
      if (this.m_updateCounter == -1 && !Sync.IsServer)
      {
        this.RequestWeathersUpdate();
        this.m_updateCounter = 0;
      }
      if (this.m_updateCounter == 0)
        this.UpdateAfterSimulationDelay();
      ++this.m_updateCounter;
      if (this.m_updateCounter > 60)
        this.m_updateCounter = 0;
      if (!MySandboxGame.IsPaused)
      {
        this.UpdatePlanetDataClient();
        this.UpdatePlanetDataServer();
      }
      for (int index = 0; index < this.m_lightnings.Count; ++index)
      {
        if (!this.m_lightnings[index].Initialized)
          this.m_lightnings[index].Init();
      }
      if (!Sync.IsDedicated && this.Session != null)
      {
        if (this.m_closestPlanet != null && MySector.MainCamera != null)
        {
          this.m_originAltitude = (float) (MySector.MainCamera.Position - this.m_closestPlanet.PositionComp.GetPosition()).Length();
          this.m_surfaceAltitude = this.m_originAltitude - (float) (this.m_closestPlanet.GetClosestSurfacePointGlobal(MySector.MainCamera.WorldMatrix.Translation) - this.m_closestPlanet.PositionComp.GetPosition()).Length();
        }
        else
        {
          this.m_originAltitude = 0.0f;
          this.m_surfaceAltitude = 0.0f;
        }
      }
      this.UpdateDefaultWeather();
      if (this.m_currentWeather != null && !Sandbox.Engine.Platform.Game.IsDedicated)
      {
        this.ApplyCurrentWeather();
        this.ApplySound();
        this.ApplyParticle();
      }
      this.UpdateTargetWeather();
    }

    private void UpdateAfterSimulationDelay()
    {
      if (MySector.MainCamera == null)
        return;
      bool flag1 = false;
      if (MySession.Static != null)
      {
        if (!Sync.IsServer)
        {
          foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in this.m_weatherPlanetData)
            weatherPlanetData.NextWeather -= 60;
        }
        foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
        {
          if (instance is MyPlanet myPlanet)
          {
            bool flag2 = false;
            foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in this.m_weatherPlanetData)
            {
              if (weatherPlanetData.PlanetId == myPlanet.EntityId)
              {
                flag2 = true;
                break;
              }
            }
            if (!flag2)
            {
              this.m_weatherPlanetData.Add(new MyObjectBuilder_WeatherPlanetData()
              {
                PlanetId = myPlanet.EntityId,
                NextWeather = (int) ((double) MyUtils.GetRandomFloat((float) myPlanet.Generator.WeatherFrequencyMin, (float) myPlanet.Generator.WeatherFrequencyMax) / 0.0166666675359011)
              });
              flag1 = true;
            }
          }
        }
        for (int index = this.m_weatherPlanetData.Count - 1; index > -1; --index)
        {
          bool flag2 = false;
          foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
          {
            if (instance is MyPlanet myPlanet && this.m_weatherPlanetData[index].PlanetId == myPlanet.EntityId)
            {
              flag2 = true;
              break;
            }
          }
          if (!flag2)
          {
            this.m_weatherPlanetData.RemoveAtFast<MyObjectBuilder_WeatherPlanetData>(index);
            flag1 = true;
          }
        }
      }
      if (flag1 && Sync.IsServer)
        this.SyncWeathers();
      BoundingSphereD sphere = new BoundingSphereD(MySector.MainCamera.WorldMatrix.Translation, 100.0);
      this.m_nearbyEntities.Clear();
      MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, this.m_nearbyEntities);
      this.UpdateLocalData();
    }

    private void UpdateLocalData()
    {
      if (Sync.IsDedicated || MySession.Static == null)
        return;
      if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.ReverbDetectorComp != null)
      {
        this.m_insideVoxel = MySession.Static.LocalCharacter.ReverbDetectorComp.Voxels > 20;
        bool flag1 = MySession.Static.LocalCharacter.ReverbDetectorComp.Grids > 20;
        bool flag2 = false;
        if (this.Session.Player != null && this.Session.Player.Controller != null && this.Session.Player.Controller.ControlledEntity != null)
          flag2 = this.Session.Player.Controller.ControlledEntity.Entity is IMyCockpit;
        if (this.m_insideGrid != flag1 || this.m_inCockpit != flag2)
        {
          this.m_insideGrid = flag1;
          this.m_inCockpit = flag2;
          foreach (MyParticleEffect particleEffect in this.m_particleEffects)
          {
            if (particleEffect != null)
              this.UpdateCameraSoftRadiusMultiplier(particleEffect);
          }
        }
      }
      this.m_insideClosedCockpit = MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity != null && MySession.Static.ControlledEntity.Entity is MyCockpit && (MySession.Static.ControlledEntity.Entity as MyCockpit).BlockDefinition.IsPressurized;
      this.m_closestPlanet = MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.Position);
      if (this.m_closestPlanet != null)
        this.m_gravityVector = (Vector3) Vector3D.Normalize(this.m_closestPlanet.PositionComp.GetPosition() - MySector.MainCamera.Position);
      else
        this.m_gravityVector = Vector3.Zero;
    }

    private void UpdatePlanetDataClient()
    {
      for (int index = this.m_lightnings.Count - 1; index > -1; --index)
      {
        ++this.m_lightnings[index].ObjectBuilder.Life;
        if ((int) this.m_lightnings[index].ObjectBuilder.Life > (int) this.m_lightnings[index].ObjectBuilder.MaxLife)
          this.m_lightnings.RemoveAtFast<MySectorWeatherComponent.EffectLightning>(index);
      }
      for (int index1 = 0; index1 < this.m_weatherPlanetData.Count; ++index1)
      {
        MyObjectBuilder_WeatherPlanetData weatherPlanetData = this.m_weatherPlanetData[index1];
        if (weatherPlanetData != null)
        {
          for (int index2 = 0; index2 < weatherPlanetData.Weathers.Count; ++index2)
          {
            MyObjectBuilder_WeatherEffect weather = weatherPlanetData.Weathers[index2];
            if (weather.Velocity != Vector3D.Zero)
              weather.Position += weather.Velocity * 0.0166666675359011;
            if (weather.MaxLife > 0)
            {
              ++weather.Life;
              if (weather.Life <= 1000)
                weather.Intensity = Math.Min((float) weather.Life / 1000f, 1f);
              else if (weather.Life >= weather.MaxLife - 1000)
                weather.Intensity = (float) (weather.MaxLife - weather.Life) / 1000f;
            }
          }
        }
      }
    }

    private void UpdatePlanetDataServer()
    {
      if (!Sync.IsServer || MySession.Static == null || !MySession.Static.Settings.WeatherSystem)
        return;
      for (int index1 = 0; index1 < this.m_weatherPlanetData.Count; ++index1)
      {
        MyObjectBuilder_WeatherPlanetData weatherPlanetData = this.m_weatherPlanetData[index1];
        if (weatherPlanetData != null)
        {
          for (int index2 = 0; index2 < weatherPlanetData.Weathers.Count; ++index2)
          {
            MyObjectBuilder_WeatherEffect weather = weatherPlanetData.Weathers[index2];
            MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
            if (weatherEffect != null && weatherEffect.Lightning != null)
            {
              --weather.NextLightning;
              if (weather.NextLightning == 0)
                this.CreateRandomLightning(weather, weatherEffect.Lightning, false, false);
              if (weather.NextLightning <= 0)
              {
                float randomFloat = MyUtils.GetRandomFloat(weatherEffect.LightningIntervalMin, weatherEffect.LightningIntervalMax);
                weather.NextLightning = (int) ((double) randomFloat / 0.0166666675359011);
              }
              --weather.NextLightningCharacter;
              if (weather.NextLightningCharacter == 0)
                this.CreateRandomLightning(weather, weatherEffect.Lightning, false, true);
              if (weather.NextLightningCharacter <= 0)
              {
                float randomFloat = MyUtils.GetRandomFloat(weatherEffect.LightningCharacterHitIntervalMin, weatherEffect.LightningCharacterHitIntervalMax);
                weather.NextLightningCharacter = (int) ((double) randomFloat / 0.0166666675359011);
              }
              --weather.NextLightningGrid;
              if (weather.NextLightningGrid == 0)
                this.CreateRandomLightning(weather, weatherEffect.Lightning, true, false);
              if (weather.NextLightningGrid <= 0)
              {
                float randomFloat = MyUtils.GetRandomFloat(weatherEffect.LightningGridHitIntervalMin, weatherEffect.LightningGridHitIntervalMax);
                weather.NextLightningGrid = (int) ((double) randomFloat / 0.0166666675359011);
              }
            }
          }
        }
      }
      for (int index1 = 0; index1 < this.m_weatherPlanetData.Count; ++index1)
      {
        MyObjectBuilder_WeatherPlanetData weatherPlanetData = this.m_weatherPlanetData[index1];
        if (weatherPlanetData != null)
        {
          MyPlanet entityById = (MyPlanet) Sandbox.Game.Entities.MyEntities.GetEntityById(weatherPlanetData.PlanetId);
          if (entityById != null && entityById.Generator != null && entityById.Generator.WeatherGenerators != null)
          {
            --weatherPlanetData.NextWeather;
            if (weatherPlanetData.NextWeather <= 0)
            {
              this.CreateRandomWeather(entityById);
              weatherPlanetData.NextWeather = (int) ((double) MyUtils.GetRandomFloat((float) entityById.Generator.WeatherFrequencyMin, (float) entityById.Generator.WeatherFrequencyMax) / 0.0166666675359011);
            }
          }
          for (int index2 = 0; index2 < weatherPlanetData.Weathers.Count; ++index2)
          {
            MyObjectBuilder_WeatherEffect weather = weatherPlanetData.Weathers[index2];
            if (weather.Life > weather.MaxLife || weather.Weather.Equals("Clear"))
              this.RemoveWeather(weather.Position);
          }
        }
      }
    }

    private void UpdateTargetWeather()
    {
      if (MySector.MainCamera == null)
        return;
      if (this.m_closestPlanet != null)
      {
        MyObjectBuilder_WeatherEffect weatherEffect = (MyObjectBuilder_WeatherEffect) null;
        if (this.GetWeather(MySector.MainCamera.Position, out weatherEffect))
        {
          this.m_nightValue = MySectorWeatherComponent.NightValue(this.m_closestPlanet, MySector.MainCamera.Position);
          this.m_weatherIntensity = this.GetWeatherIntensity(MySector.MainCamera.Position);
          if (this.m_targetWeather != null && !(this.m_targetWeather.Id.SubtypeName != weatherEffect.Weather))
            return;
          this.SetTargetWeather(MyDefinitionManager.Static.GetWeatherEffect(weatherEffect.Weather), true);
        }
        else
        {
          if (this.m_targetWeather == null || !(this.m_targetWeather.Id.SubtypeName != "Default"))
            return;
          this.ResetWeather(true);
        }
      }
      else
      {
        if (this.m_targetWeather == null || !(this.m_targetWeather.Id.SubtypeName != "Default"))
          return;
        this.ResetWeather(true);
      }
    }

    private void UpdateCameraSoftRadiusMultiplier(MyParticleEffect particleEffect)
    {
      if (!this.m_insideGrid)
      {
        if (this.m_inCockpit)
        {
          if (this.Session.Player != null && this.Session.Player.Controller != null && (this.Session.Player.Controller.ControlledEntity != null && this.Session.Player.Controller.ControlledEntity.Entity is IMyCockpit))
            particleEffect.CameraSoftRadiusMultiplier = this.Session.Player.Controller.ControlledEntity.Entity.LocalVolume.Radius;
        }
        else
          particleEffect.CameraSoftRadiusMultiplier = 1f;
      }
      if (this.m_insideGrid || this.m_inCockpit)
        particleEffect.SoftParticleDistanceScaleMultiplier = 1000f;
      else
        particleEffect.SoftParticleDistanceScaleMultiplier = 1f;
    }

    public static float NightValue(MyPlanet planet, Vector3D position)
    {
      Vector3D vector3D = position - planet.PositionComp.GetPosition();
      if (vector3D.Length() > (double) planet.MaximumRadius * 1.10000002384186)
        return 0.0f;
      Vector3 vector2 = Vector3.Normalize(vector3D);
      return (double) Vector3.Dot(MySector.DirectionToSunNormalized, vector2) < 0.0 ? Math.Abs(Vector3.Dot(MySector.DirectionToSunNormalized, vector2)) : 0.0f;
    }

    protected override void UnloadData()
    {
      if (this.m_ambientSound != null && this.m_ambientSound.IsPlaying)
        this.m_ambientSound.Stop(true);
      this.ResetWeather(true);
      MySectorWeatherComponent.Static = (MySectorWeatherComponent) null;
    }

    private void UpdateDefaultWeather()
    {
      if (MySector.EnvironmentDefinition != null)
      {
        this.m_defaultWeather.FogProperties = MySector.EnvironmentDefinition.FogProperties;
        this.m_defaultWeather.SunColor = MySector.EnvironmentDefinition.SunProperties.EnvironmentLight.SunColor;
        this.m_defaultWeather.SunSpecularColor = MySector.EnvironmentDefinition.SunProperties.EnvironmentLight.SunSpecularColor;
        this.m_defaultWeather.SunIntensity = MySector.EnvironmentDefinition.SunProperties.SunIntensity;
        this.m_defaultWeather.ShadowFadeout = MySector.EnvironmentDefinition.SunProperties.EnvironmentLight.ShadowFadeoutMultiplier;
      }
      else
      {
        this.m_defaultWeather.FogProperties = MyFogProperties.Default;
        this.m_defaultWeather.SunColor = MyEnvironmentLightData.Default.SunColor;
        this.m_defaultWeather.SunSpecularColor = MyEnvironmentLightData.Default.SunSpecularColor;
        this.m_defaultWeather.SunIntensity = MySunProperties.Default.SunIntensity;
        this.m_defaultWeather.ShadowFadeout = MyEnvironmentLightData.Default.ShadowFadeoutMultiplier;
      }
    }

    public override void Draw()
    {
      if (this.m_defaultWeather == null)
        return;
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && Sync.IsServer)
      {
        foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetData in this.m_weatherPlanetData)
        {
          foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetData.Weathers)
          {
            MyRenderProxy.DebugDrawSphere(weather.Position, weather.Radius, Color.Red, cull: false);
            MyRenderProxy.DebugDrawLine3D(weather.StartPoint, weather.EndPoint, Color.Green, Color.Yellow, true);
          }
        }
      }
      if (MySandboxGame.IsPaused)
        return;
      for (int index1 = 0; index1 < this.m_lightnings.Count; ++index1)
      {
        Vector3D start = this.m_lightnings[index1].ObjectBuilder.Position;
        for (int index2 = 1; index2 <= (int) this.m_lightnings[index1].ObjectBuilder.BoltParts; ++index2)
        {
          float num = (float) index2 / (float) this.m_lightnings[index1].ObjectBuilder.BoltParts;
          Vector3D end = start - this.m_gravityVector * (this.m_lightnings[index1].ObjectBuilder.BoltLength / (float) this.m_lightnings[index1].ObjectBuilder.BoltParts) + MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(0.0f, (float) this.m_lightnings[index1].ObjectBuilder.BoltVariation);
          MySimpleObjectDraw.DrawLine(start, end, new MyStringId?(MySectorWeatherComponent.m_lightningMaterial), ref this.m_lightnings[index1].ObjectBuilder.Color, num * this.m_lightnings[index1].ObjectBuilder.BoltRadius);
          if (index2 > 2)
          {
            for (int index3 = 0; index3 < MyUtils.GetRandomInt(0, 2); ++index3)
              MySimpleObjectDraw.DrawLine(start, start + this.m_gravityVector * (float) ((double) this.m_lightnings[index1].ObjectBuilder.BoltLength / (double) this.m_lightnings[index1].ObjectBuilder.BoltParts / 2.0) + MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(0.0f, (float) ((int) this.m_lightnings[index1].ObjectBuilder.BoltVariation / 2)), new MyStringId?(MySectorWeatherComponent.m_lightningMaterial), ref this.m_lightnings[index1].ObjectBuilder.Color, (float) ((double) num * (double) this.m_lightnings[index1].ObjectBuilder.BoltRadius / 2.0));
          }
          start = end;
        }
      }
    }

    private void ApplyCurrentWeather()
    {
      if (this.m_targetWeather == null)
        this.m_targetWeather = this.m_defaultWeather;
      this.m_sourceWeather.Lerp(this.m_defaultWeather, this.m_sourceWeather, this.m_currentTransition);
      this.m_sourceWeather.Lerp(this.m_targetWeather, this.m_currentWeather, this.m_currentTransition * this.m_weatherIntensity);
      this.m_currentTransition += this.m_transitionSpeed;
      this.m_currentTransition = MathHelper.Clamp(this.m_currentTransition, 0.0f, 1f);
      ref MyFogProperties local1 = ref MySector.FogProperties;
      float? nullable;
      double fogMultiplier;
      if (!this.FogMultiplierOverride.HasValue)
      {
        fogMultiplier = (double) this.m_currentWeather.FogProperties.FogMultiplier;
      }
      else
      {
        nullable = this.FogMultiplierOverride;
        fogMultiplier = (double) nullable.Value;
      }
      local1.FogMultiplier = (float) fogMultiplier;
      ref MyFogProperties local2 = ref MySector.FogProperties;
      nullable = this.FogDensityOverride;
      double fogDensity;
      if (!nullable.HasValue)
      {
        fogDensity = (double) this.m_currentWeather.FogProperties.FogDensity;
      }
      else
      {
        nullable = this.FogDensityOverride;
        fogDensity = (double) nullable.Value;
      }
      local2.FogDensity = (float) fogDensity;
      MySector.FogProperties.FogColor = this.FogColorOverride.HasValue ? this.FogColorOverride.Value : this.m_currentWeather.FogProperties.FogColor * (float) (1.0 - (double) this.m_nightValue * 0.5);
      ref MyFogProperties local3 = ref MySector.FogProperties;
      nullable = this.FogSkyboxOverride;
      double fogSkybox;
      if (!nullable.HasValue)
      {
        fogSkybox = (double) this.m_currentWeather.FogProperties.FogSkybox;
      }
      else
      {
        nullable = this.FogSkyboxOverride;
        fogSkybox = (double) nullable.Value;
      }
      local3.FogSkybox = (float) fogSkybox;
      ref MyFogProperties local4 = ref MySector.FogProperties;
      nullable = this.FogAtmoOverride;
      double fogAtmo;
      if (!nullable.HasValue)
      {
        fogAtmo = (double) this.m_currentWeather.FogProperties.FogAtmo;
      }
      else
      {
        nullable = this.FogAtmoOverride;
        fogAtmo = (double) nullable.Value;
      }
      local4.FogAtmo = (float) fogAtmo;
      MySector.SunProperties.EnvironmentLight.SunColor = this.m_currentWeather.SunColor;
      MySector.SunProperties.EnvironmentLight.SunSpecularColor = this.m_currentWeather.SunSpecularColor;
      ref MySunProperties local5 = ref MySector.SunProperties;
      nullable = this.SunIntensityOverride;
      double sunIntensity;
      if (!nullable.HasValue)
      {
        sunIntensity = (double) this.m_currentWeather.SunIntensity;
      }
      else
      {
        nullable = this.SunIntensityOverride;
        sunIntensity = (double) nullable.Value;
      }
      local5.SunIntensity = (float) sunIntensity;
      double windStrength = (double) MyRenderProxy.Settings.WindStrength;
      float num1 = MyRenderSettings.Default.WindStrength * this.m_currentWeather.FoliageWindModifier;
      double num2 = (double) num1;
      if ((double) Math.Abs((float) (windStrength - num2)) > 0.00999999977648258)
      {
        MyRenderProxy.Settings.WindStrength = num1;
        MyRenderProxy.SetSettingsDirty();
      }
      if (MySector.EnvironmentDefinition == null)
        return;
      MySector.SunProperties.EnvironmentLight.ShadowFadeoutMultiplier = MySector.EnvironmentDefinition.SunProperties.EnvironmentLight.ShadowFadeoutMultiplier;
    }

    private void SetTargetWeather(MyWeatherEffectDefinition targetWeather, bool instant = false)
    {
      if (this.m_currentWeather == null)
      {
        this.m_currentWeather = new MyWeatherEffectDefinition();
        this.m_defaultWeather.Lerp(this.m_defaultWeather, this.m_currentWeather, 1f);
      }
      if (this.m_sourceWeather == null)
        this.m_sourceWeather = new MyWeatherEffectDefinition();
      this.m_currentWeather.Lerp(this.m_currentWeather, this.m_sourceWeather, 1f);
      this.m_targetWeather = targetWeather;
      if (!instant)
        this.m_currentTransition = 0.0f;
      else
        this.m_currentTransition = 1f;
    }

    private void ApplySound()
    {
      string ambientSound = this.m_currentWeather.AmbientSound;
      if (string.IsNullOrEmpty(ambientSound))
      {
        if (this.m_ambientSound == null || !this.m_ambientSound.IsPlaying)
          return;
        this.m_ambientSound.Stop(true);
        this.m_currentSound = "";
      }
      else
      {
        if (this.m_ambientSound != null && this.m_ambientSound.IsPlaying && !this.m_currentSound.Equals(ambientSound))
          this.m_ambientSound.Stop(true);
        if (this.m_ambientSound == null || !this.m_ambientSound.IsPlaying || !this.m_currentSound.Equals(ambientSound))
        {
          this.m_ambientSound = MyAudio.Static.PlaySound(new MyCueId(MyStringHash.GetOrCompute(ambientSound)));
          if (this.m_ambientSound == null)
          {
            int num = MyAudio.Static.CanPlay ? 1 : 0;
            return;
          }
          this.m_ambientSound.SetVolume(0.0f);
          this.m_currentSound = ambientSound;
        }
        if (this.m_ambientSound != null && this.m_ambientSound.IsPlaying)
        {
          if (this.m_insideClosedCockpit)
            this.m_targetVolume = MathHelper.Clamp((float) ((double) this.m_currentWeather.AmbientVolume * (double) this.m_currentTransition * (double) this.m_weatherIntensity * 0.300000011920929), 0.0f, this.m_currentWeather.AmbientVolume);
          else if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.ReverbDetectorComp != null)
            this.m_targetVolume = MathHelper.Clamp(this.m_currentWeather.AmbientVolume * this.m_currentTransition * this.m_weatherIntensity * Math.Min((float) ((25.0 - (double) (MySession.Static.LocalCharacter.ReverbDetectorComp.Grids - 10)) / 25.0), 1f), 0.0f, this.m_currentWeather.AmbientVolume);
          if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.OxygenComponent.HelmetEnabled && MySession.Static.Settings.RealisticSound)
            this.m_targetVolume *= this.REALISTIC_SOUND_MULTIPLIER_WITH_HELMET;
          this.m_ambientSound.SetVolume(MathHelper.Lerp(this.m_ambientSound.Volume, this.m_targetVolume, this.m_volumeTransitionSpeed));
        }
        this.m_currentSound = ambientSound;
      }
    }

    private void ApplyParticle()
    {
      if (this.m_currentWeather == null || this.Session == null || (Sync.IsDedicated || this.Session == null) || MySector.MainCamera == null)
        return;
      int num = (int) Math.Round((double) this.m_currentWeather.ParticleCount * (double) this.m_weatherIntensity);
      MatrixD? directionOverride = this.ParticleDirectionOverride;
      MatrixD fromDir;
      if (directionOverride.HasValue)
      {
        directionOverride = this.ParticleDirectionOverride;
        fromDir = directionOverride.Value;
      }
      else
        fromDir = MatrixD.CreateFromDir((Vector3D) this.m_gravityVector);
      IMyCockpit myCockpit = (IMyCockpit) null;
      Vector3D position1 = MySector.MainCamera.Position;
      if (this.m_particleEffects == null || this.m_particleEffects.Count != this.m_currentWeather.ParticleCount)
      {
        this.ResetParticles(false);
        if (this.m_particleEffectIndex >= this.m_currentWeather.ParticleCount)
          this.m_particleEffectIndex = 0;
      }
      if (this.m_particleEffectIndex < num && this.m_particleEffectIndex < this.m_particleEffects.Count)
      {
        Vector3D zero = Vector3D.Zero;
        MyParticleEffect effect = this.m_particleEffects[this.m_particleEffectIndex];
        if (effect == null || effect.IsStopped || effect.GetName() != this.m_currentWeather.EffectName)
        {
          if (effect != null && !effect.IsStopped)
            effect.Stop(false);
          MyParticlesManager.TryCreateParticleEffect(this.m_currentWeather.EffectName, out effect);
          this.m_particleEffects[this.m_particleEffectIndex] = effect;
          if (effect != null)
            this.UpdateCameraSoftRadiusMultiplier(effect);
        }
        if (effect != null)
        {
          Vector3 vector3_1 = MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(0.0f, this.m_currentWeather.ParticleRadius);
          Vector3D position2 = myCockpit != null ? myCockpit.CubeGrid.Physics.LinearVelocity + position1 + vector3_1 : (this.Session.Player == null || this.Session.Player.Character == null ? position1 + vector3_1 : this.Session.Player.Character.Physics.LinearVelocity + position1 + vector3_1);
          if (this.IsPositionSafe(ref position2))
          {
            fromDir.Translation = position2;
            effect.WorldMatrix = fromDir;
            effect.UserScale = this.m_currentWeather.ParticleScale;
            Vector3? velocityOverride = this.ParticleVelocityOverride;
            if (velocityOverride.HasValue)
            {
              MyParticleEffect myParticleEffect = effect;
              velocityOverride = this.ParticleVelocityOverride;
              Vector3 vector3_2 = velocityOverride.Value;
              myParticleEffect.Velocity = vector3_2;
            }
            else
              effect.Velocity = Vector3.Zero;
          }
        }
      }
      else if (this.m_particleEffectIndex < this.m_particleEffects.Count && this.m_particleEffects[this.m_particleEffectIndex] != null)
      {
        this.m_particleEffects[this.m_particleEffectIndex].Stop(false);
        this.m_particleEffects[this.m_particleEffectIndex] = (MyParticleEffect) null;
      }
      ++this.m_particleEffectIndex;
      if (this.m_particleEffectIndex < this.m_particleEffects.Count)
        return;
      this.m_particleEffectIndex = 0;
    }

    private bool IsPositionSafe(ref Vector3D position)
    {
      if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.ReverbDetectorComp != null)
      {
        int voxels = MySession.Static.LocalCharacter.ReverbDetectorComp.Voxels;
      }
      if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.ReverbDetectorComp != null)
      {
        int grids = MySession.Static.LocalCharacter.ReverbDetectorComp.Grids;
      }
      for (int index = 0; index < this.m_nearbyEntities.Count; ++index)
      {
        if (this.m_nearbyEntities[index] != null)
        {
          if (this.m_nearbyEntities[index] is MyVoxelPhysics)
          {
            LineD line = new LineD(position, position + 30f * this.m_gravityVector);
            MyIntersectionResultLineTriangleEx? t;
            if (this.m_insideVoxel && (this.m_nearbyEntities[index] as MyVoxelPhysics).RootVoxel.GetIntersectionWithLine(ref line, out t, IntersectionFlags.ALL_TRIANGLES))
              return false;
            line = new LineD(position, position - 30f * this.m_gravityVector);
            if ((this.m_nearbyEntities[index] as MyVoxelPhysics).RootVoxel.GetIntersectionWithLine(ref line, out t, IntersectionFlags.ALL_TRIANGLES))
              return false;
          }
          if (this.m_nearbyEntities[index] is MyCubeGrid && ((MyCubeGrid) this.m_nearbyEntities[index]).GridSizeEnum == MyCubeSize.Large)
          {
            MatrixD fromDir = MatrixD.CreateFromDir((Vector3D) this.m_gravityVector);
            float num = 5f;
            this.m_particleSpread[0] = position;
            this.m_particleSpread[1] = position + (double) num * fromDir.Left;
            this.m_particleSpread[2] = position + (double) num * fromDir.Right;
            this.m_particleSpread[3] = position + (double) num * fromDir.Up;
            this.m_particleSpread[4] = position + (double) num * fromDir.Down;
            Vector3I? nullable;
            if (this.m_insideGrid)
            {
              foreach (Vector3D from in this.m_particleSpread)
              {
                LineD lineD = new LineD(from, from + 30f * this.m_gravityVector);
                if ((this.m_nearbyEntities[index] as MyCubeGrid).RayCastBlocks(lineD.From, lineD.To).HasValue)
                {
                  Vector3D vector3D = from - 28f * this.m_gravityVector;
                  Vector3D worldStart = vector3D;
                  Vector3D worldEnd = from;
                  nullable = (this.m_nearbyEntities[index] as MyCubeGrid).RayCastBlocks(worldStart, worldEnd);
                  if (nullable.HasValue)
                    return false;
                  position = vector3D;
                  break;
                }
              }
            }
            foreach (Vector3D vector3D in this.m_particleSpread)
            {
              Vector3D from;
              Vector3D to = (from = vector3D) - 30f * this.m_gravityVector;
              LineD lineD = new LineD(from, to);
              nullable = (this.m_nearbyEntities[index] as MyCubeGrid).RayCastBlocks(lineD.From, lineD.To);
              if (nullable.HasValue)
                return false;
            }
          }
        }
      }
      return true;
    }

    private void ResetWeather(bool instant)
    {
      if (this.m_defaultWeather == null)
        return;
      this.SetTargetWeather(this.m_defaultWeather, instant);
      this.ApplyCurrentWeather();
      this.ResetParticles(instant);
    }

    private void ResetParticles(bool instant)
    {
      if (this.m_currentWeather.ParticleCount < 0)
        return;
      if (this.m_currentWeather.ParticleCount > this.m_particleEffects.Count)
      {
        this.m_particleEffects.AddRange((IEnumerable<MyParticleEffect>) new MyParticleEffect[this.m_currentWeather.ParticleCount - this.m_particleEffects.Count]);
      }
      else
      {
        if (this.m_currentWeather.ParticleCount >= this.m_particleEffects.Count)
          return;
        for (int particleCount = this.m_currentWeather.ParticleCount; particleCount < this.m_particleEffects.Count; ++particleCount)
        {
          if (this.m_particleEffects[particleCount] != null)
            this.m_particleEffects[particleCount].Stop(instant);
        }
        this.m_particleEffects.RemoveRange(this.m_currentWeather.ParticleCount, this.m_particleEffects.Count - this.m_currentWeather.ParticleCount);
      }
    }

    public bool IsPositionAirtight(Vector3D position, out MyCubeGrid grid)
    {
      for (int index = 0; index < this.m_nearbyEntities.Count; ++index)
      {
        MyCubeGrid myCubeGrid = (MyCubeGrid) null;
        if (this.m_nearbyEntities[index] != null)
          myCubeGrid = this.m_nearbyEntities[index] as MyCubeGrid;
        if (myCubeGrid != null && myCubeGrid.GridSizeEnum == MyCubeSize.Large)
        {
          Vector3I gridInteger = myCubeGrid.WorldToGridInteger(position);
          if (myCubeGrid.IsRoomAtPositionAirtight(gridInteger))
          {
            grid = myCubeGrid;
            return true;
          }
        }
      }
      grid = (MyCubeGrid) null;
      return false;
    }

    public bool IsPositionAirtight(Vector3D position)
    {
      for (int index = 0; index < this.m_nearbyEntities.Count; ++index)
      {
        MyCubeGrid myCubeGrid = (MyCubeGrid) null;
        if (this.m_nearbyEntities[index] != null)
          myCubeGrid = this.m_nearbyEntities[index] as MyCubeGrid;
        if (myCubeGrid != null && myCubeGrid.IsRoomAtPositionAirtight(myCubeGrid.WorldToGridInteger(position)))
          return true;
      }
      return false;
    }

    private void SyncWeathers() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyObjectBuilder_WeatherPlanetData[]>((Func<IMyEventOwner, Action<MyObjectBuilder_WeatherPlanetData[]>>) (x => new Action<MyObjectBuilder_WeatherPlanetData[]>(MySectorWeatherComponent.UpdateWeathersOnClients)), this.m_weatherPlanetData.ToArray());

    [Event(null, 1872)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void UpdateWeathersOnClients(MyObjectBuilder_WeatherPlanetData[] planetData)
    {
      if (MySession.Static == null || Sandbox.Engine.Multiplayer.MyMultiplayer.Static == null || MySectorWeatherComponent.Static == null)
        return;
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        MyEventContext.ValidationFailed();
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        MySectorWeatherComponent.Static.m_weatherPlanetData.Clear();
        MySectorWeatherComponent.Static.m_weatherPlanetData.AddArray<MyObjectBuilder_WeatherPlanetData>(planetData);
      }
    }

    private void SyncLightning() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyObjectBuilder_WeatherLightning[]>((Func<IMyEventOwner, Action<MyObjectBuilder_WeatherLightning[]>>) (x => new Action<MyObjectBuilder_WeatherLightning[]>(MySectorWeatherComponent.UpdateLightningsOnClients)), this.m_lightnings.Select<MySectorWeatherComponent.EffectLightning, MyObjectBuilder_WeatherLightning>((Func<MySectorWeatherComponent.EffectLightning, MyObjectBuilder_WeatherLightning>) (x => x.ObjectBuilder)).ToArray<MyObjectBuilder_WeatherLightning>());

    [Event(null, 1895)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void UpdateLightningsOnClients(MyObjectBuilder_WeatherLightning[] lightnings)
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
      {
        MyEventContext.ValidationFailed();
        (Sandbox.Engine.Multiplayer.MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      }
      else
      {
        if (MySectorWeatherComponent.Static == null)
          return;
        MySectorWeatherComponent.Static.m_lightnings.Clear();
        foreach (MyObjectBuilder_WeatherLightning lightning in lightnings)
        {
          MySectorWeatherComponent.EffectLightning effectLightning = new MySectorWeatherComponent.EffectLightning()
          {
            ObjectBuilder = lightning
          };
          MySectorWeatherComponent.Static.m_lightnings.Add(effectLightning);
        }
      }
    }

    public void RequestWeathersUpdate() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MySectorWeatherComponent.RequestWeathersUpdate_Implementation)));

    [Event(null, 1926)]
    [Reliable]
    [Server]
    private static void RequestWeathersUpdate_Implementation() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<MyObjectBuilder_WeatherPlanetData[]>((Func<IMyEventOwner, Action<MyObjectBuilder_WeatherPlanetData[]>>) (s => new Action<MyObjectBuilder_WeatherPlanetData[]>(MySectorWeatherComponent.UpdateWeathersOnClient)), MySectorWeatherComponent.Static.m_weatherPlanetData.ToArray(), MyEventContext.Current.Sender);

    [Event(null, 1932)]
    [Reliable]
    [Client]
    private static void UpdateWeathersOnClient(MyObjectBuilder_WeatherPlanetData[] planetData)
    {
      if (MySectorWeatherComponent.Static == null)
        return;
      MySectorWeatherComponent.Static.m_weatherPlanetData.Clear();
      MySectorWeatherComponent.Static.m_weatherPlanetData.AddArray<MyObjectBuilder_WeatherPlanetData>(planetData);
    }

    private class EffectLightning
    {
      public MyObjectBuilder_WeatherLightning ObjectBuilder;
      public bool Initialized;
      public MyEntity3DSoundEmitter BoltEmitter = new MyEntity3DSoundEmitter((MyEntity) null);
      public MySoundPair LightningSound = new MySoundPair();

      public void Init()
      {
        if (this.ObjectBuilder == null)
          return;
        if (!Sync.IsDedicated)
        {
          this.LightningSound.Init(this.ObjectBuilder.Sound, false);
          this.BoltEmitter.SetPosition(new Vector3D?(this.ObjectBuilder.Position));
          this.BoltEmitter.PlaySound(this.LightningSound);
          if (MySession.Static != null && MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.ReverbDetectorComp != null)
            this.BoltEmitter.VolumeMultiplier = MyUtils.GetRandomFloat(100f, 150f) / 100f * Math.Min((float) ((25.0 - (double) (MySession.Static.LocalCharacter.ReverbDetectorComp.Grids - 10)) / 25.0), 1f);
        }
        if (Sync.IsServer && (double) this.ObjectBuilder.ExplosionRadius != 0.0)
        {
          MyExplosionTypeEnum explosionTypeEnum = MyExplosionTypeEnum.CUSTOM;
          MyEntity closestPlanet = (MyEntity) MyGamePruningStructure.GetClosestPlanet(this.ObjectBuilder.Position);
          MyExplosionInfo explosionInfo = new MyExplosionInfo()
          {
            PlayerDamage = 0.0f,
            Damage = (float) this.ObjectBuilder.Damage,
            ExplosionType = explosionTypeEnum,
            ExplosionSphere = new BoundingSphereD(this.ObjectBuilder.Position, (double) this.ObjectBuilder.ExplosionRadius),
            LifespanMiliseconds = 700,
            ParticleScale = 1f,
            Direction = new Vector3?(Vector3.Up),
            AffectVoxels = false,
            KeepAffectedBlocks = true,
            VoxelExplosionCenter = this.ObjectBuilder.Position,
            ExplosionFlags = MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.APPLY_DEFORMATION,
            VoxelCutoutScale = 0.0f,
            PlaySound = true,
            ApplyForceAndDamage = true,
            ObjectsRemoveDelayInMiliseconds = 40,
            StrengthImpulse = this.ObjectBuilder.BoltImpulseMultiplier,
            OwnerEntity = closestPlanet ?? (MyEntity) null,
            OriginEntity = closestPlanet != null ? closestPlanet.EntityId : 0L
          };
          MyExplosions.AddExplosion(ref explosionInfo);
        }
        this.Initialized = true;
      }
    }

    protected sealed class RequestLightningServer\u003C\u003EVRageMath_Vector3D\u0023VRageMath_Vector3D : ICallSite<IMyEventOwner, Vector3D, Vector3D, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in Vector3D cameraTranslation,
        in Vector3D hitPosition,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySectorWeatherComponent.RequestLightningServer(cameraTranslation, hitPosition);
      }
    }

    protected sealed class UpdateWeathersOnClients\u003C\u003EVRage_Game_MyObjectBuilder_WeatherPlanetData\u003C\u0023\u003E : ICallSite<IMyEventOwner, MyObjectBuilder_WeatherPlanetData[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyObjectBuilder_WeatherPlanetData[] planetData,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySectorWeatherComponent.UpdateWeathersOnClients(planetData);
      }
    }

    protected sealed class UpdateLightningsOnClients\u003C\u003EVRage_Game_MyObjectBuilder_WeatherLightning\u003C\u0023\u003E : ICallSite<IMyEventOwner, MyObjectBuilder_WeatherLightning[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyObjectBuilder_WeatherLightning[] lightnings,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySectorWeatherComponent.UpdateLightningsOnClients(lightnings);
      }
    }

    protected sealed class RequestWeathersUpdate_Implementation\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySectorWeatherComponent.RequestWeathersUpdate_Implementation();
      }
    }

    protected sealed class UpdateWeathersOnClient\u003C\u003EVRage_Game_MyObjectBuilder_WeatherPlanetData\u003C\u0023\u003E : ICallSite<IMyEventOwner, MyObjectBuilder_WeatherPlanetData[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyObjectBuilder_WeatherPlanetData[] planetData,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySectorWeatherComponent.UpdateWeathersOnClient(planetData);
      }
    }
  }
}
