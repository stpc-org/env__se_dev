// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Analytics.MySpaceAnalytics
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage;
using VRage.Analytics;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Replication;
using VRage.Utils;
using VRageRender;

namespace Sandbox.Engine.Analytics
{
  [StaticEventOwner]
  public sealed class MySpaceAnalytics : MyAnalyticsManager
  {
    private static MySpaceAnalytics m_instance;
    private bool m_firstRun;
    private bool m_isSessionStarted;
    private bool m_isSessionEnded;
    private string m_hashedUserID;
    private string m_sessionID;
    private string m_clientVersion;
    private string m_platform;
    private Dictionary<string, MyProduct> m_products;
    private MySessionStartEvent m_defaultSessionData;
    private MyWorldStartEvent m_defaultWorldData;
    private MyDamageInformation m_lastDamageInformation = new MyDamageInformation()
    {
      Type = MyStringHash.NullOrEmpty
    };
    private int m_lastCampaignProgressionTime;
    private MyWorldEntryEnum m_worldEntry;
    private DateTime m_loadingStartedAt;
    private DateTime m_worldStartTime;
    private string m_worldSessionID;
    private int m_serverMostConcurrentPlayers;
    private IMyAnalytics m_heartbeatTracker;
    private TimeSpan m_heartbeatPeriod;
    private MyHeartbeatEvent m_heartbeatEvent;
    private bool m_isHeartbeatEnabled;
    private DateTime m_lastHeartbeatSent;

    public static MySpaceAnalytics Instance
    {
      get
      {
        if (MySpaceAnalytics.m_instance == null)
        {
          MySpaceAnalytics mySpaceAnalytics = new MySpaceAnalytics();
          Interlocked.CompareExchange<MySpaceAnalytics>(ref MySpaceAnalytics.m_instance, mySpaceAnalytics, (MySpaceAnalytics) null);
        }
        return MySpaceAnalytics.m_instance;
      }
    }

    public string UserId => this.m_hashedUserID;

    private MySpaceAnalytics()
    {
    }

    public void StartSessionAndIdentifyPlayer(ulong userId, bool firstTimeRun)
    {
      this.m_hashedUserID = this.HashUserId(userId);
      this.StartSession(firstTimeRun);
    }

    public void StartSessionAndIdentifyPlayer(string hashedUserId, bool firstTimeRun)
    {
      this.m_hashedUserID = hashedUserId;
      this.StartSession(firstTimeRun);
    }

    public void ReportGoodBotQuestion(
      ResponseType responseType,
      string question,
      string responseId)
    {
      this.ReportCurrentEvent((MyAnalyticsEvent) new MyGoodBotEvent(this.m_defaultWorldData)
      {
        GoodBot_ResponseType = responseType,
        GoodBot_Question = question,
        GoodBot_ResponseID = responseId
      });
    }

    public void ReportDropContainer(bool competetive) => this.ReportCurrentEvent((MyAnalyticsEvent) new MyDropContainerEvent(this.m_defaultWorldData)
    {
      Competetive = new bool?(competetive)
    });

    public void ReportBannerClick(string caption, uint packageID) => this.ReportCurrentEvent((MyAnalyticsEvent) new MyBannerClickEvent(this.m_defaultSessionData)
    {
      banner_caption = caption,
      banner_package_id = new uint?(packageID)
    });

    public void ReportSessionEnd(string sessionEndReason)
    {
      MySessionEndEvent sessionEndEvent;
      if (!this.TryEndSession(sessionEndReason, out sessionEndEvent, out string _))
        return;
      this.ReportCurrentEvent((MyAnalyticsEvent) sessionEndEvent);
    }

    public void ReportSessionEndByCrash(Exception exception) => this.EndSessionAndReportLater("GameCrash", exception);

    public void StoreWorldLoadingStartTime() => this.m_loadingStartedAt = DateTime.UtcNow;

    public void SetWorldEntry(MyWorldEntryEnum entry) => this.m_worldEntry = entry;

    public void ReportWorldStart(MyObjectBuilder_SessionSettings settings)
    {
      if (!this.m_isSessionStarted)
        return;
      MyMultiplayer.RaiseStaticEvent<string, string>((Func<IMyEventOwner, Action<string, string>>) (x => new Action<string, string>(MySpaceAnalytics.NotifyAnalyticsIds)), this.m_hashedUserID, this.m_sessionID);
      this.m_worldSessionID = Guid.NewGuid().ToString();
      this.m_serverMostConcurrentPlayers = 0;
      this.m_defaultWorldData = this.GatherWorldStartData(settings);
      this.m_worldStartTime = DateTime.UtcNow;
      this.ReportCurrentEvent((MyAnalyticsEvent) this.m_defaultWorldData);
    }

    public void ReportWorldEnd()
    {
      if (!this.m_isSessionStarted)
        return;
      MyWorldEndEvent myWorldEndEvent = this.GatherWorldEndData();
      if (myWorldEndEvent.WorldStartProperties == null)
      {
        this.m_worldSessionID = (string) null;
      }
      else
      {
        this.ReportCurrentEvent((MyAnalyticsEvent) myWorldEndEvent);
        this.m_worldSessionID = (string) null;
      }
    }

    public void SetLastDamageInformation(MyDamageInformation lastDamageInformation)
    {
      if (lastDamageInformation.Type == new MyStringHash())
        return;
      this.m_lastDamageInformation = lastDamageInformation;
    }

    public void OnSuspending() => this.EndSessionAndReportLater("Suspended");

    public void OnResuming()
    {
      this.StartSession(false);
      if (MySession.Static == null)
        return;
      MySession.Static.ResetStatistics();
      this.ReportWorldStart(MySession.Static.Settings);
    }

    public void RegisterHeartbeatTracker(IMyAnalytics heartbeatTracker, TimeSpan heartbeatPeriod)
    {
      this.m_heartbeatTracker = heartbeatTracker ?? throw new ArgumentNullException("heartbeatTracker can't be null");
      this.m_heartbeatPeriod = heartbeatPeriod;
      this.m_lastHeartbeatSent = DateTime.MinValue;
    }

    public void Update(MyTimeSpan elapsedTime)
    {
      this.TrySendHeartbeat();
      if (MySession.Static == null || MyMultiplayer.Static == null)
        return;
      this.m_serverMostConcurrentPlayers = Math.Max(this.m_serverMostConcurrentPlayers, MyMultiplayer.Static.MemberCount - (MyMultiplayer.Static is MyMultiplayerClient ? 1 : 0));
    }

    private void StartSession(bool firstTimeRun)
    {
      if (this.m_isSessionStarted)
        return;
      if (this.m_hashedUserID == null)
        throw new Exception("UserID not set before SessionStart");
      this.ReportPostponedEvents();
      this.m_firstRun = firstTimeRun;
      this.m_defaultSessionData = this.GatherSessionStartData();
      this.m_products = this.GetProducts();
      this.m_clientVersion = MyFinalBuildConstants.APP_VERSION_STRING_DOTS.ToString();
      this.m_platform = MyPerGameSettings.BasicGameInfo.GameAcronym;
      this.m_sessionID = Guid.NewGuid().ToString();
      this.ReportCurrentEvent((MyAnalyticsEvent) this.m_defaultSessionData);
      this.m_isSessionEnded = false;
      this.m_isSessionStarted = true;
      MyLog.Default.WriteLine("Analytics session started: UserID = (" + this.m_hashedUserID + ") SessionID = (" + this.m_sessionID + ")");
      this.StartHeartbeat();
    }

    private void ReportCurrentEvent(MyAnalyticsEvent analyticsEvent) => ((IMyAnalytics) this).ReportEvent((IMyAnalyticsEvent) analyticsEvent, DateTime.UtcNow, this.m_sessionID, this.m_hashedUserID, this.m_clientVersion, this.m_platform);

    private void EndSessionAndReportLater(string reason, Exception exception = null)
    {
      if (MySession.Static != null && this.m_isSessionStarted)
        ((IMyAnalytics) this).ReportEventLater((IMyAnalyticsEvent) this.GatherWorldEndData(), DateTime.UtcNow, this.m_sessionID, this.m_hashedUserID, this.m_clientVersion, this.m_platform);
      MySessionEndEvent sessionEndEvent;
      string sessionId;
      if (!this.TryEndSession(reason, out sessionEndEvent, out sessionId))
        return;
      ((IMyAnalytics) this).ReportEventLater((IMyAnalyticsEvent) sessionEndEvent, DateTime.UtcNow + TimeSpan.FromMilliseconds(1.0), sessionId, this.m_hashedUserID, this.m_clientVersion, this.m_platform, exception);
    }

    private Dictionary<string, MyProduct> GetProducts()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || !MyGameService.IsActive)
        return (Dictionary<string, MyProduct>) null;
      Dictionary<string, MyProduct> dictionary = new Dictionary<string, MyProduct>();
      DateTime? purchaseTime1;
      if (MyGameService.IsProductOwned(MyGameService.AppId, out purchaseTime1))
      {
        MyProduct myProduct = new MyProduct()
        {
          ProductID = MyGameService.AppId.ToString(),
          ProductName = "MainGame"
        };
        if (purchaseTime1.HasValue)
          myProduct.PurchaseTimestamp = purchaseTime1.Value;
        dictionary.Add(myProduct.ProductName, myProduct);
      }
      foreach (KeyValuePair<uint, MyDLCs.MyDLC> dlC in MyDLCs.DLCs)
      {
        DateTime? purchaseTime2;
        if (MyGameService.IsProductOwned(dlC.Key, out purchaseTime2))
        {
          MyProduct myProduct = new MyProduct()
          {
            ProductID = dlC.Key.ToString(),
            ProductName = dlC.Value.Name,
            PackageID = dlC.Value.PackageId,
            StoreID = dlC.Value.StoreId
          };
          if (purchaseTime2.HasValue)
            myProduct.PurchaseTimestamp = purchaseTime2.Value;
          dictionary.Add(myProduct.ProductName, myProduct);
        }
      }
      return dictionary;
    }

    private MySessionStartEvent GatherSessionStartData()
    {
      MySessionStartEvent sessionStartEvent1 = new MySessionStartEvent();
      sessionStartEvent1.client_version = MyPerGameSettings.BasicGameInfo.GameVersion.ToString();
      sessionStartEvent1.client_branch = MyGameService.BranchNameFriendly;
      uint physicalCores;
      sessionStartEvent1.cpu_info = MyVRage.Platform.System.GetInfoCPU(out uint _, out physicalCores);
      sessionStartEvent1.cpu_number_of_threads = new int?(Environment.ProcessorCount);
      sessionStartEvent1.cpu_number_of_cores = new uint?(physicalCores);
      sessionStartEvent1.os_memory = new ulong?(MyVRage.Platform.System.GetTotalPhysicalMemory() / 1024UL / 1024UL);
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
      {
        MyAdapterInfo adapter = MyVideoSettingsManager.Adapters[MyVideoSettingsManager.CurrentDeviceSettings.AdapterOrdinal];
        sessionStartEvent1.gpu_name = adapter.Name;
        sessionStartEvent1.gpu_memory = new int?((int) (adapter.VRAM / 1024UL / 1024UL));
        sessionStartEvent1.gpu_driver_version = adapter.DriverVersion;
      }
      sessionStartEvent1.os_info = Environment.OSVersion.VersionString;
      sessionStartEvent1.os_architecture = Environment.Is64BitOperatingSystem ? "64bit" : "32bit";
      sessionStartEvent1.is_first_run = new bool?(this.m_firstRun);
      sessionStartEvent1.is_dedicated = new bool?(Sandbox.Engine.Platform.Game.IsDedicated);
      sessionStartEvent1.userLanguage = MyLanguage.CurrentLanguage.ToString();
      sessionStartEvent1.userLocale = MyLanguage.CurrentCultureName.ToString();
      sessionStartEvent1.os_culture = MyLanguage.GetCurrentOSCulture();
      sessionStartEvent1.region_iso2 = MyVRage.Platform.System.TwoLetterISORegionName;
      sessionStartEvent1.region_iso3 = MyVRage.Platform.System.ThreeLetterISORegionName;
      float result1;
      float.TryParse(MyVRage.Platform.System.RegionLongitude, out result1);
      float result2;
      float.TryParse(MyVRage.Platform.System.RegionLatitude, out result2);
      sessionStartEvent1.region_location = new double[2]
      {
        (double) result1,
        (double) result2
      };
      sessionStartEvent1.audio_hud_warnings = new bool?(MySandboxGame.Config.HudWarnings);
      sessionStartEvent1.speed_based_ship_sounds = new bool?(MySandboxGame.Config.ShipSoundsAreBasedOnSpeed);
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
      {
        sessionStartEvent1.display_resolution = MySandboxGame.Config.ScreenWidth.ToString() + " x " + MySandboxGame.Config.ScreenHeight.ToString();
        sessionStartEvent1.display_window_mode = MyVideoSettingsManager.CurrentDeviceSettings.WindowMode.ToString();
        MySessionStartEvent sessionStartEvent2 = sessionStartEvent1;
        MyGraphicsSettings graphicsSettings = MyVideoSettingsManager.CurrentGraphicsSettings;
        string str1 = graphicsSettings.PerformanceSettings.RenderSettings.AnisotropicFiltering.ToString();
        sessionStartEvent2.graphics_anisotropic_filtering = str1;
        MySessionStartEvent sessionStartEvent3 = sessionStartEvent1;
        graphicsSettings = MyVideoSettingsManager.CurrentGraphicsSettings;
        string str2 = graphicsSettings.PerformanceSettings.RenderSettings.AntialiasingMode.ToString();
        sessionStartEvent3.graphics_antialiasing_mode = str2;
        MySessionStartEvent sessionStartEvent4 = sessionStartEvent1;
        graphicsSettings = MyVideoSettingsManager.CurrentGraphicsSettings;
        string str3 = graphicsSettings.PerformanceSettings.RenderSettings.ShadowQuality.ToString();
        sessionStartEvent4.graphics_shadow_quality = str3;
        MySessionStartEvent sessionStartEvent5 = sessionStartEvent1;
        graphicsSettings = MyVideoSettingsManager.CurrentGraphicsSettings;
        string str4 = graphicsSettings.PerformanceSettings.RenderSettings.TextureQuality.ToString();
        sessionStartEvent5.graphics_texture_quality = str4;
        MySessionStartEvent sessionStartEvent6 = sessionStartEvent1;
        graphicsSettings = MyVideoSettingsManager.CurrentGraphicsSettings;
        string str5 = graphicsSettings.PerformanceSettings.RenderSettings.VoxelTextureQuality.ToString();
        sessionStartEvent6.graphics_voxel_texture_quality = str5;
        MySessionStartEvent sessionStartEvent7 = sessionStartEvent1;
        graphicsSettings = MyVideoSettingsManager.CurrentGraphicsSettings;
        string str6 = graphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality.ToString();
        sessionStartEvent7.graphics_voxel_quality = str6;
        sessionStartEvent1.graphics_grass_density_factor = new double?((double) MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.GrassDensityFactor);
        sessionStartEvent1.graphics_grass_draw_distance = new double?((double) MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.GrassDrawDistance);
        sessionStartEvent1.graphics_flares_intensity = new double?((double) MyVideoSettingsManager.CurrentGraphicsSettings.FlaresIntensity);
        MySessionStartEvent sessionStartEvent8 = sessionStartEvent1;
        graphicsSettings = MyVideoSettingsManager.CurrentGraphicsSettings;
        string str7 = graphicsSettings.PerformanceSettings.RenderSettings.VoxelShaderQuality.ToString();
        sessionStartEvent8.graphics_voxel_shader_quality = str7;
        MySessionStartEvent sessionStartEvent9 = sessionStartEvent1;
        graphicsSettings = MyVideoSettingsManager.CurrentGraphicsSettings;
        string str8 = graphicsSettings.PerformanceSettings.RenderSettings.AlphaMaskedShaderQuality.ToString();
        sessionStartEvent9.graphics_alphamasked_shader_quality = str8;
        MySessionStartEvent sessionStartEvent10 = sessionStartEvent1;
        graphicsSettings = MyVideoSettingsManager.CurrentGraphicsSettings;
        string str9 = graphicsSettings.PerformanceSettings.RenderSettings.AtmosphereShaderQuality.ToString();
        sessionStartEvent10.graphics_atmosphere_shader_quality = str9;
        sessionStartEvent1.graphics_distance_fade = new double?((double) MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.DistanceFade);
        sessionStartEvent1.audio_music_volume = new double?((double) MySandboxGame.Config.MusicVolume);
        sessionStartEvent1.audio_sound_volume = new double?((double) MySandboxGame.Config.GameVolume);
        sessionStartEvent1.audio_mute_when_not_in_focus = new bool?(MySandboxGame.Config.EnableMuteWhenNotInFocus);
        sessionStartEvent1.experimental_mode = new bool?(MySandboxGame.Config.ExperimentalMode);
        sessionStartEvent1.building_mode = new int?(MySandboxGame.Config.CubeBuilderBuildingMode);
        sessionStartEvent1.controls_hints = new bool?(MySandboxGame.Config.ControlsHints);
        sessionStartEvent1.goodbot_hints = new bool?(MySandboxGame.Config.GoodBotHints);
        sessionStartEvent1.rotation_hints = new bool?(MySandboxGame.Config.RotationHints);
        sessionStartEvent1.enable_steam_cloud = new bool?(MySandboxGame.Config.EnableSteamCloud);
        sessionStartEvent1.enable_trading = new bool?(MySandboxGame.Config.EnableTrading);
        sessionStartEvent1.gdpr_consent = MySandboxGame.Config.GDPRConsent;
        sessionStartEvent1.area_interaction = new bool?(MySandboxGame.Config.AreaInteraction);
        sessionStartEvent1.mod_io_consent = new bool?(MySandboxGame.Config.ModIoConsent);
        sessionStartEvent1.show_crosshair = MySandboxGame.Config.ShowCrosshair;
      }
      return sessionStartEvent1;
    }

    private MyWorldStartEvent GatherWorldStartData(
      MyObjectBuilder_SessionSettings settings)
    {
      MyWorldStartEvent myWorldStartEvent = new MyWorldStartEvent(this.m_defaultSessionData);
      myWorldStartEvent.WorldSessionID = this.m_worldSessionID;
      myWorldStartEvent.scenario_name = MyCampaignManager.Static.ActiveCampaignName;
      myWorldStartEvent.entry = this.m_worldEntry.ToString();
      myWorldStartEvent.game_mode = settings.GameMode.ToString();
      myWorldStartEvent.online_mode = settings.OnlineMode.ToString();
      myWorldStartEvent.world_type = MySession.Static.Scenario.Id.SubtypeName;
      myWorldStartEvent.worldName = MySession.Static.Name;
      myWorldStartEvent.server_is_dedicated = new bool?(MyMultiplayer.Static is MyMultiplayerClient);
      myWorldStartEvent.server_id = MyMultiplayer.Static is MyMultiplayerClient multiplayerClient ? multiplayerClient.HostAnalyticsId : "";
      myWorldStartEvent.server_max_number_of_players = new int?(MyMultiplayer.Static != null ? MyMultiplayer.Static.MemberLimit : 1);
      myWorldStartEvent.server_current_number_of_players = new int?(MyMultiplayer.Static != null ? MyMultiplayer.Static.MemberCount : 1);
      myWorldStartEvent.is_hosting_player = new bool?(MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer);
      myWorldStartEvent.multiplayer_type = MyMultiplayer.Static == null ? "Off-line" : (MySession.Static == null || MySession.Static.LocalCharacter == null || !MyMultiplayer.Static.HostName.Equals(MySession.Static.LocalCharacter.DisplayNameText) ? (!MyMultiplayer.Static.HostName.Equals("Dedicated server") ? "Client" : "Dedicated server") : "Host");
      myWorldStartEvent.active_mods = MySpaceAnalytics.GetModList();
      myWorldStartEvent.active_mods_count = new int?(MySession.Static.Mods.Count);
      long num = (long) Math.Ceiling((DateTime.UtcNow - this.m_loadingStartedAt).TotalSeconds);
      myWorldStartEvent.loading_duration = new long?(num);
      myWorldStartEvent.networking_type = MyGameService.Networking?.ServiceName;
      MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
      bool flag = MyCampaignManager.Static != null && MyCampaignManager.Static.ActiveCampaign != null;
      myWorldStartEvent.is_campaign_mission = new bool?(flag);
      myWorldStartEvent.is_official_campaign = new bool?(flag && MyCampaignManager.Static.ActiveCampaign != null && MyCampaignManager.Static.ActiveCampaign.IsVanilla);
      myWorldStartEvent.level_script_count = new int?(component == null || component.RunningLevelScriptNames == null ? 0 : ((IEnumerable<string>) component.RunningLevelScriptNames).Count<string>());
      myWorldStartEvent.state_machine_count = new int?(component == null || component.SMManager == null || component.SMManager.MachineDefinitions == null ? 0 : component.SMManager.MachineDefinitions.Count);
      this.m_lastCampaignProgressionTime = 0;
      myWorldStartEvent.voxel_support = new bool?(settings.StationVoxelSupport);
      myWorldStartEvent.destructible_blocks = new bool?(settings.DestructibleBlocks);
      myWorldStartEvent.destructible_voxels = new bool?(settings.EnableVoxelDestruction);
      myWorldStartEvent.jetpack = new bool?(settings.EnableJetpack);
      myWorldStartEvent.hostility = settings.EnvironmentHostility.ToString();
      myWorldStartEvent.drones = new bool?(settings.EnableDrones);
      myWorldStartEvent.wolfs = new bool?(settings.EnableWolfs);
      myWorldStartEvent.spiders = new bool?(settings.EnableSpiders);
      myWorldStartEvent.encounters = new bool?(settings.EnableEncounters);
      myWorldStartEvent.oxygen = new bool?(settings.EnableOxygen);
      myWorldStartEvent.pressurization = new bool?(settings.EnableOxygenPressurization);
      myWorldStartEvent.realistic_sounds = new bool?(settings.RealisticSound);
      myWorldStartEvent.tool_shake = new bool?(settings.EnableToolShake);
      myWorldStartEvent.multiplier_inventory = new double?((double) settings.InventorySizeMultiplier);
      myWorldStartEvent.multiplier_welding_speed = new double?((double) settings.WelderSpeedMultiplier);
      myWorldStartEvent.multiplier_grinding_speed = new double?((double) settings.GrinderSpeedMultiplier);
      myWorldStartEvent.multiplier_refinery_speed = new double?((double) settings.RefinerySpeedMultiplier);
      myWorldStartEvent.multiplier_assembler_speed = new double?((double) settings.AssemblerSpeedMultiplier);
      myWorldStartEvent.multiplier_assembler_efficiency = new double?((double) settings.AssemblerEfficiencyMultiplier);
      myWorldStartEvent.max_floating_objects = new int?((int) settings.MaxFloatingObjects);
      myWorldStartEvent.weather_system = new bool?(settings.WeatherSystem);
      return myWorldStartEvent;
    }

    private MyWorldEndEvent GatherWorldEndData()
    {
      MyWorldEndEvent myWorldEndEvent1 = new MyWorldEndEvent(this.m_defaultWorldData);
      MyFpsManager.PrepareMinMax();
      MySession mySession = MySession.Static;
      if (mySession != null)
      {
        TimeSpan timeSpan = mySession.ElapsedPlayTime;
        double totalSeconds = timeSpan.TotalSeconds;
        myWorldEndEvent1.world_session_duration = new uint?((uint) totalSeconds);
        MyWorldEndEvent myWorldEndEvent2 = myWorldEndEvent1;
        timeSpan = mySession.ElapsedGameTime;
        uint? nullable = new uint?((uint) timeSpan.TotalSeconds);
        myWorldEndEvent2.entire_world_duration = nullable;
        myWorldEndEvent1.fps_average = totalSeconds == 0.0 ? new uint?() : new uint?((uint) ((double) MyFpsManager.GetSessionTotalFrames() / totalSeconds));
        myWorldEndEvent1.fps_minimum = new uint?((uint) MyFpsManager.GetMinSessionFPS());
        myWorldEndEvent1.fps_maximum = new uint?((uint) MyFpsManager.GetMaxSessionFPS());
        myWorldEndEvent1.ups_average = new uint?((uint) ((double) MyGameStats.Static.UpdateCount / totalSeconds));
        myWorldEndEvent1.simspeed_client_average = totalSeconds == 0.0 ? new double?() : new double?((double) mySession.SessionSimSpeedPlayer / totalSeconds);
        myWorldEndEvent1.simspeed_server_average = totalSeconds == 0.0 ? new double?() : new double?((double) mySession.SessionSimSpeedServer / totalSeconds);
      }
      int[] percentileValues = MyGeneralStats.Static.PercentileValues;
      IEnumerable<(string Name, double[] Value, bool Bytes)> aggregatedStats = MyGeneralStats.Static.AggregatedStats;
      myWorldEndEvent1.NetworkStats = new Dictionary<string, double>();
      foreach ((string Name, double[] Value, bool Bytes) tuple in aggregatedStats)
      {
        for (int index = 0; index < percentileValues.Length; ++index)
        {
          double num = tuple.Value[index];
          if (tuple.Bytes)
            num *= 0.0009765625;
          myWorldEndEvent1.NetworkStats[string.Format("{0}.{1}", (object) tuple.Name, (object) percentileValues[index])] = num;
        }
      }
      if (MyMultiplayerMinimalBase.Instance is MyMultiplayerClient instance && !string.IsNullOrEmpty(instance.DisconnectReason))
        myWorldEndEvent1.world_exit_reason = instance.DisconnectReason;
      if (MyCampaignManager.Static != null)
      {
        MyObjectBuilder_Campaign activeCampaign = MyCampaignManager.Static.ActiveCampaign;
      }
      int num1 = 0;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      if (localCharacter != null && localCharacter.Toolbar != null)
      {
        for (int index = 0; index < localCharacter.Toolbar.ItemCount; ++index)
        {
          if (localCharacter.Toolbar.GetItemAtIndex(index) != null)
            ++num1;
        }
      }
      myWorldEndEvent1.toolbar_used_slots = new uint?((uint) num1);
      myWorldEndEvent1.toolbar_page_switches = new uint?(MySession.Static.ToolbarPageSwitches);
      myWorldEndEvent1.total_blocks_created = new uint?(MySession.Static.TotalBlocksCreated);
      myWorldEndEvent1.total_damage_dealt = new uint?(MySession.Static.TotalDamageDealt);
      myWorldEndEvent1.time_grinding_blocks = new uint?((uint) MySession.Static.TimeGrindingBlocks.TotalSeconds);
      MyWorldEndEvent myWorldEndEvent3 = myWorldEndEvent1;
      TimeSpan timeSpan1 = MySession.Static.TimeGrindingFriendlyBlocks;
      uint? nullable1 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent3.time_grinding_friendly_blocks = nullable1;
      MyWorldEndEvent myWorldEndEvent4 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeGrindingNeutralBlocks;
      uint? nullable2 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent4.time_grinding_neutral_blocks = nullable2;
      MyWorldEndEvent myWorldEndEvent5 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeGrindingEnemyBlocks;
      uint? nullable3 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent5.time_grinding_enemy_blocks = nullable3;
      MyWorldEndEvent myWorldEndEvent6 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimePilotingBigShip;
      uint? nullable4 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent6.time_piloting_big_ships = nullable4;
      MyWorldEndEvent myWorldEndEvent7 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimePilotingSmallShip;
      uint? nullable5 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent7.time_piloting_small_ships = nullable5;
      MyWorldEndEvent myWorldEndEvent8 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeOnFoot;
      uint? nullable6 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent8.time_on_foot_all = nullable6;
      MyWorldEndEvent myWorldEndEvent9 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeOnJetpack;
      uint? nullable7 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent9.time_using_jetpack = nullable7;
      MyWorldEndEvent myWorldEndEvent10 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeOnStation;
      uint? nullable8 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent10.time_on_foot_stations = nullable8;
      MyWorldEndEvent myWorldEndEvent11 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeOnShips;
      uint? nullable9 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent11.time_on_foot_ships = nullable9;
      MyWorldEndEvent myWorldEndEvent12 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeOnAsteroids;
      uint? nullable10 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent12.time_on_foot_asteroids = nullable10;
      MyWorldEndEvent myWorldEndEvent13 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeOnPlanets;
      uint? nullable11 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent13.time_on_foot_planets = nullable11;
      MyWorldEndEvent myWorldEndEvent14 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeSprinting;
      uint? nullable12 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent14.time_sprinting = nullable12;
      MyWorldEndEvent myWorldEndEvent15 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeUsingMouseInput;
      uint? nullable13 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent15.time_using_input_mouse = nullable13;
      MyWorldEndEvent myWorldEndEvent16 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeUsingGamepadInput;
      uint? nullable14 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent16.time_using_input_gamepad = nullable14;
      MyWorldEndEvent myWorldEndEvent17 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeInCameraGridFirstPerson;
      uint? nullable15 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent17.time_camera_grid_first_person = nullable15;
      MyWorldEndEvent myWorldEndEvent18 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeInCameraGridThirdPerson;
      uint? nullable16 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent18.time_camera_grid_third_person = nullable16;
      MyWorldEndEvent myWorldEndEvent19 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeInCameraCharFirstPerson;
      uint? nullable17 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent19.time_camera_character_first_person = nullable17;
      MyWorldEndEvent myWorldEndEvent20 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeInCameraCharThirdPerson;
      uint? nullable18 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent20.time_camera_character_third_person = nullable18;
      MyWorldEndEvent myWorldEndEvent21 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeInCameraToolFirstPerson;
      uint? nullable19 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent21.time_camera_tool_first_person = nullable19;
      MyWorldEndEvent myWorldEndEvent22 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeInCameraToolThirdPerson;
      uint? nullable20 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent22.time_camera_tool_third_person = nullable20;
      MyWorldEndEvent myWorldEndEvent23 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeInCameraWeaponFirstPerson;
      uint? nullable21 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent23.time_camera_weapon_first_person = nullable21;
      MyWorldEndEvent myWorldEndEvent24 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeInCameraWeaponThirdPerson;
      uint? nullable22 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent24.time_camera_weapon_third_person = nullable22;
      MyWorldEndEvent myWorldEndEvent25 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeInCameraBuildingFirstPerson;
      uint? nullable23 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent25.time_camera_building_first_person = nullable23;
      MyWorldEndEvent myWorldEndEvent26 = myWorldEndEvent1;
      timeSpan1 = MySession.Static.TimeInCameraBuildingThirdPerson;
      uint? nullable24 = new uint?((uint) timeSpan1.TotalSeconds);
      myWorldEndEvent26.time_camera_building_third_person = nullable24;
      myWorldEndEvent1.TotalAmountMined = new Dictionary<string, int>();
      foreach (KeyValuePair<string, MyFixedPoint> keyValuePair in MySession.Static.AmountMined)
        myWorldEndEvent1.TotalAmountMined[keyValuePair.Key] = (int) keyValuePair.Value.RawValue;
      MyWorldEndEvent myWorldEndEvent27 = myWorldEndEvent1;
      TimeSpan timeSpan2 = MySession.Static.TimeInBuilderMode;
      uint? nullable25 = new uint?((uint) timeSpan2.TotalSeconds);
      myWorldEndEvent27.time_in_ship_builder_mode = nullable25;
      MyWorldEndEvent myWorldEndEvent28 = myWorldEndEvent1;
      timeSpan2 = MySession.Static.TimeCreativeToolsEnabled;
      uint? nullable26 = new uint?((uint) timeSpan2.TotalSeconds);
      myWorldEndEvent28.time_creative_tools_enabled = nullable26;
      myWorldEndEvent1.total_blocks_created_from_ship = new uint?(MySession.Static.TotalBlocksCreatedFromShips);
      myWorldEndEvent1.server_most_concurrent_players = new uint?((uint) Math.Max(1, this.m_serverMostConcurrentPlayers));
      return myWorldEndEvent1;
    }

    private bool TryEndSession(
      string sessionEndReason,
      out MySessionEndEvent sessionEndEvent,
      out string sessionId)
    {
      if (this.m_isSessionEnded)
      {
        sessionEndEvent = (MySessionEndEvent) null;
        sessionId = (string) null;
        return false;
      }
      sessionEndEvent = new MySessionEndEvent(this.m_defaultSessionData)
      {
        game_quit_reason = sessionEndReason,
        session_duration = new int?(MySandboxGame.TotalTimeInMilliseconds / 1000)
      };
      sessionId = this.m_sessionID;
      this.m_isSessionEnded = true;
      this.m_isSessionStarted = false;
      this.PauseHeartbeat();
      return true;
    }

    private string HashUserId(ulong userId)
    {
      Random random = new Random((int) userId);
      byte[] b = new byte[16];
      byte[] buffer = b;
      random.NextBytes(buffer);
      return new Guid(b).ToString();
    }

    private static string GetModList()
    {
      string empty = string.Empty;
      foreach (MyObjectBuilder_Checkpoint.ModItem mod in MySession.Static.Mods)
      {
        if (!string.IsNullOrEmpty(empty))
          empty += ", ";
        empty += mod.FriendlyName.Replace(",", "");
      }
      return empty;
    }

    private void PauseHeartbeat() => this.m_isHeartbeatEnabled = false;

    private void StartHeartbeat() => this.m_isHeartbeatEnabled = true;

    private bool TrySendHeartbeat()
    {
      if (!this.m_isHeartbeatEnabled || this.m_heartbeatTracker == null)
        return false;
      DateTime utcNow = DateTime.UtcNow;
      if (this.m_lastHeartbeatSent + this.m_heartbeatPeriod > utcNow)
        return false;
      this.m_heartbeatEvent = this.m_heartbeatEvent ?? this.GatherHeartbeatData();
      this.m_heartbeatTracker.ReportEvent((IMyAnalyticsEvent) this.m_heartbeatEvent, DateTime.UtcNow, this.m_sessionID, this.m_hashedUserID, this.m_clientVersion, this.m_platform);
      this.m_lastHeartbeatSent = utcNow;
      return true;
    }

    private MyHeartbeatEvent GatherHeartbeatData() => new MyHeartbeatEvent()
    {
      Region_ISO2 = this.m_defaultSessionData.region_iso2,
      Region_ISO3 = this.m_defaultSessionData.region_iso3
    };

    [Event(null, 724)]
    [Reliable]
    [Server]
    private static void NotifyAnalyticsIds(string userId, string sessionId) => MyLog.Default.WriteLine(string.Format("Analytics ids for user ({0}): User = ({1}) Session = ({2}).", (object) MyEventContext.Current.Sender, (object) userId, (object) sessionId));

    protected sealed class NotifyAnalyticsIds\u003C\u003ESystem_String\u0023System_String : ICallSite<IMyEventOwner, string, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string userId,
        in string sessionId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySpaceAnalytics.NotifyAnalyticsIds(userId, sessionId);
      }
    }
  }
}
