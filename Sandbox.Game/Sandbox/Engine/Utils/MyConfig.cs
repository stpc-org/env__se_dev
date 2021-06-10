// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyConfig
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Utils
{
  public class MyConfig : MyConfigBase, IMyConfig
  {
    private readonly string DX9_RENDER_QUALITY = "RenderQuality";
    private readonly string MODEL_QUALITY = nameof (ModelQuality);
    private readonly string VOXEL_QUALITY = nameof (VoxelQuality);
    private readonly string FIELD_OF_VIEW = nameof (FieldOfView);
    private readonly string ENABLE_DAMAGE_EFFECTS = nameof (EnableDamageEffects);
    private readonly string SCREEN_WIDTH = nameof (ScreenWidth);
    private readonly string SCREEN_HEIGHT = nameof (ScreenHeight);
    private readonly string FULL_SCREEN = "FullScreen";
    private readonly string VIDEO_ADAPTER = nameof (VideoAdapter);
    private readonly string DISABLE_UPDATE_DRIVER_NOTIFICATION = nameof (DisableUpdateDriverNotification);
    private readonly string VERTICAL_SYNC = nameof (VerticalSync);
    private readonly string REFRESH_RATE = nameof (RefreshRate);
    private readonly string FLARES_INTENSITY = nameof (FlaresIntensity);
    private readonly string GAME_VOLUME = nameof (GameVolume);
    private readonly string MUSIC_VOLUME_OLD = nameof (MusicVolume);
    private readonly string MUSIC_VOLUME = "Music_Volume";
    private readonly string VOICE_CHAT_VOLUME = nameof (VoiceChatVolume);
    private readonly string LANGUAGE = nameof (Language);
    private readonly string SKIN = nameof (Skin);
    private readonly string EXPERIMENTAL_MODE = nameof (ExperimentalMode);
    private readonly string CONTROLS_HINTS = nameof (ControlsHints);
    private readonly string GOODBOT_HINTS = nameof (GoodBotHints);
    private readonly string NEW_NEW_GAME_SCREEN = "NewNewGameScreen";
    private readonly string ROTATION_HINTS = nameof (RotationHints);
    private readonly string SHOW_CROSSHAIR = "ShowCrosshair2";
    private readonly string ENABLE_TRADING = nameof (EnableTrading);
    private readonly string ENABLE_STEAM_CLOUD = nameof (EnableSteamCloud);
    private readonly string CONTROLS_GENERAL = nameof (ControlsGeneral);
    private readonly string CONTROLS_BUTTONS = nameof (ControlsButtons);
    private readonly string SCREENSHOT_SIZE_MULTIPLIER = nameof (ScreenshotSizeMultiplier);
    private readonly string FIRST_TIME_RUN = nameof (FirstTimeRun);
    private readonly string FIRST_VT_TIME_RUN = nameof (FirstVTTimeRun);
    private readonly string FIRST_TIME_TUTORIALS = nameof (FirstTimeTutorials);
    private readonly string SYNC_RENDERING = nameof (SyncRendering);
    private readonly string NEED_SHOW_BATTLE_TUTORIAL_QUESTION = nameof (NeedShowBattleTutorialQuestion);
    private readonly string DEBUG_INPUT_COMPONENTS = "DebugInputs";
    private readonly string DEBUG_INPUT_COMPONENTS_INFO = nameof (DebugComponentsInfo);
    private readonly string MINIMAL_HUD = nameof (MinimalHud);
    private readonly string HUD_STATE = nameof (HudState);
    private readonly string MEMORY_LIMITS = nameof (MemoryLimits);
    private readonly string CUBE_BUILDER_USE_SYMMETRY = nameof (CubeBuilderUseSymmetry);
    private readonly string CUBE_BUILDER_BUILDING_MODE = nameof (CubeBuilderBuildingMode);
    private readonly string CUBE_BUILDER_ALIGN_TO_DEFAULT = nameof (CubeBuilderAlignToDefault);
    private readonly string MULTIPLAYER_SHOWCOMPATIBLE = nameof (MultiplayerShowCompatible);
    private readonly string COMPRESS_SAVE_GAMES = "CompressSaveGames";
    private readonly string SHOW_PLAYER_NAMES_ON_HUD = "ShowPlayerNamesOnHud";
    private readonly string RELEASING_ALT_RESETS_CAMERA = "ReleasingAltResetsCamera";
    private readonly string ENABLE_PERFORMANCE_WARNINGS_TEMP = "EnablePerformanceWarningsTempV2";
    private readonly string LAST_CHECKED_VERSION = nameof (LastCheckedVersion);
    private readonly string WINDOW_MODE = nameof (WindowMode);
    private readonly string MOUSE_CAPTURE = nameof (CaptureMouse);
    private readonly string HUD_WARNINGS = nameof (HudWarnings);
    private readonly string DYNAMIC_MUSIC = nameof (EnableDynamicMusic);
    private readonly string SHIP_SOUNDS_SPEED = nameof (ShipSoundsAreBasedOnSpeed);
    private readonly string HQTARGET = "HQTarget";
    private readonly string ANTIALIASING_MODE = nameof (AntialiasingMode);
    private readonly string SHADOW_MAP_RESOLUTION = "ShadowMapResolution";
    private readonly string SHADOW_GPU_QUALITY = "ShadowGPUQuality";
    private readonly string AMBIENT_OCCLUSION_ENABLED = nameof (AmbientOcclusionEnabled);
    private readonly string POSTPROCESSING_ENABLED = nameof (PostProcessingEnabled);
    private readonly string MULTITHREADED_RENDERING = "MultithreadedRendering";
    private readonly string TEXTURE_QUALITY = nameof (TextureQuality);
    private readonly string VOXEL_TEXTURE_QUALITY = nameof (VoxelTextureQuality);
    private readonly string SHADER_QUALITY = nameof (ShaderQuality);
    private readonly string ANISOTROPIC_FILTERING = nameof (AnisotropicFiltering);
    private readonly string FOLIAGE_DETAILS = "FoliageDetails";
    private readonly string GRASS_DENSITY = "GrassDensity";
    private readonly string GRASS_DRAW_DISTANCE = nameof (GrassDrawDistance);
    private readonly string VEGETATION_DISTANCE = "TreeViewDistance";
    private readonly string GRAPHICS_RENDERER = nameof (GraphicsRenderer);
    private readonly string ENABLE_VOICE_CHAT = "VoiceChat";
    private readonly string ENABLE_MUTE_WHEN_NOT_IN_FOCUS = nameof (EnableMuteWhenNotInFocus);
    private readonly string ENABLE_REVERB = nameof (EnableReverb);
    private readonly string UI_TRANSPARENCY = "UiTransparency";
    private readonly string UI_BK_TRANSPARENCY = "UiBkTransparency";
    private readonly string HUD_BK_TRANSPARENCY = "HUDBkTransparency";
    private readonly string TUTORIALS_FINISHED = nameof (TutorialsFinished);
    private readonly string MUTED_PLAYERS = nameof (MutedPlayers);
    private readonly string LOW_MEM_SWITCH_TO_LOW = nameof (LowMemSwitchToLow);
    private readonly string NEWSLETTER_CURRENT_STATUS = nameof (NewsletterCurrentStatus);
    private readonly string SERVER_SEARCH_SETTINGS = nameof (ServerSearchSettings);
    private readonly string ENABLE_DOPPLER = nameof (EnableDoppler);
    private readonly string WELCOMESCREEN_CURRENT_STATUS = "WelcomeScreenCurrentStatus";
    private readonly string DEBUG_OVERRIDE_AUTOSAVE = nameof (DebugOverrideAutosave);
    private readonly string GDPR_CONSENT = nameof (GDPRConsent);
    private readonly string GDPR_CONSENT_SENT = "GDPRConsentSentUpdated";
    private readonly string AREA_INTERACTION = nameof (AreaInteraction);
    private readonly string SHOW_CHAT_TIMESTAMP = nameof (ShowChatTimestamp);
    private readonly string GAMEPAD_SCHEME = "GamepadScheme";
    public readonly string MODIO_CONSENT = nameof (ModIoConsent);
    public readonly string STEAM_CONSENT = nameof (SteamConsent);
    private readonly string MIC_SENSITIVITY = "MicrophoneSensitivity";
    private readonly string AUTOMATIC_VOICE_CHAT_ACTIVATION = "VoiceChatVoiceActivation";
    private readonly string SPRITE_MAIN_VIEWPORT_SCALE = nameof (SpriteMainViewportScale);
    private readonly string CAMPAIGNS_STARTED = nameof (CampaignsStarted);
    private readonly string NEW_NEW_GAME_SCREEN_LAST_SELECTION = nameof (NewNewGameScreenLastSelection);
    private readonly string ZOOM_MULTIPLIER = nameof (ZoomMultiplier);
    private readonly string CONTROLLER_DEFAULT_ON_START = nameof (ControllerDefaultOnStart);
    private readonly string IRON_SIGHT_SWITCH_STATE = nameof (IronSightSwitchState);
    private const char m_numberSeparator = ',';
    private HashSet<ulong> m_mutedPlayers;
    private readonly string HIT_INDICATOR_COLOR_CHARACTER = nameof (HitIndicatorColorCharacter);
    private readonly string HIT_INDICATOR_COLOR_HEADSHOT = nameof (HitIndicatorColorHeadshot);
    private readonly string HIT_INDICATOR_COLOR_KILL = nameof (HitIndicatorColorKill);
    private readonly string HIT_INDICATOR_COLOR_GRID = nameof (HitIndicatorColorGrid);
    private readonly string HIT_INDICATOR_COLOR_FRIEND = nameof (HitIndicatorColorFriend);
    private readonly string HIT_INDICATOR_TEXTURE_CHARACTER = nameof (HitIndicatorTextureCharacter);
    private readonly string HIT_INDICATOR_TEXTURE_HEADSHOT = nameof (HitIndicatorTextureHeadshot);
    private readonly string HIT_INDICATOR_TEXTURE_KILL = nameof (HitIndicatorTextureKill);
    private readonly string HIT_INDICATOR_TEXTURE_GRID = nameof (HitIndicatorTextureGrid);
    private readonly string HIT_INDICATOR_TEXTURE_FRIEND = nameof (HitIndicatorTextureFriend);

    public MyConfig(string fileName)
      : base(fileName)
      => this.RedactedProperties.Add(this.MUTED_PLAYERS);

    public bool FirstTimeRun
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.FIRST_TIME_RUN), true);
      set => this.SetParameterValue(this.FIRST_TIME_RUN, new bool?(value));
    }

    public bool FirstVTTimeRun
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.FIRST_VT_TIME_RUN), true);
      set => this.SetParameterValue(this.FIRST_VT_TIME_RUN, new bool?(value));
    }

    public bool FirstTimeTutorials
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.FIRST_TIME_TUTORIALS), true);
      set => this.SetParameterValue(this.FIRST_TIME_TUTORIALS, new bool?(value));
    }

    public bool SyncRendering
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.SYNC_RENDERING), false);
      set => this.SetParameterValue(this.SYNC_RENDERING, new bool?(value));
    }

    public bool NeedShowBattleTutorialQuestion
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.NEED_SHOW_BATTLE_TUTORIAL_QUESTION), true);
      set => this.SetParameterValue(this.NEED_SHOW_BATTLE_TUTORIAL_QUESTION, new bool?(value));
    }

    public MyRenderQualityEnum? ModelQuality
    {
      get => this.GetOptionalEnum<MyRenderQualityEnum>(this.MODEL_QUALITY);
      set => this.SetOptionalEnum<MyRenderQualityEnum>(this.MODEL_QUALITY, value);
    }

    public MyRenderQualityEnum? VoxelQuality
    {
      get => this.GetOptionalEnum<MyRenderQualityEnum>(this.VOXEL_QUALITY);
      set => this.SetOptionalEnum<MyRenderQualityEnum>(this.VOXEL_QUALITY, value);
    }

    public float? GrassDensityFactor
    {
      get
      {
        float floatFromString = MyUtils.GetFloatFromString(this.GetParameterValue(this.GRASS_DENSITY), -1f);
        return (double) floatFromString < 0.0 ? new float?() : new float?(floatFromString);
      }
      set
      {
        if (value.HasValue)
          this.SetParameterValue(this.GRASS_DENSITY, value.Value);
        else
          this.m_values.Dictionary.Remove(this.GRASS_DENSITY);
      }
    }

    public float? GrassDrawDistance
    {
      get
      {
        float floatFromString = MyUtils.GetFloatFromString(this.GetParameterValue(this.GRASS_DRAW_DISTANCE), -1f);
        return (double) floatFromString < 0.0 ? new float?() : new float?(floatFromString);
      }
      set
      {
        if (value.HasValue)
          this.SetParameterValue(this.GRASS_DRAW_DISTANCE, value.Value);
        else
          this.m_values.Dictionary.Remove(this.GRASS_DRAW_DISTANCE);
      }
    }

    public float? VegetationDrawDistance
    {
      get => MyUtils.GetFloatFromString(this.GetParameterValue(this.VEGETATION_DISTANCE));
      set
      {
        if (value.HasValue)
          this.SetParameterValue(this.VEGETATION_DISTANCE, value.Value);
        else
          this.m_values.Dictionary.Remove(this.VEGETATION_DISTANCE);
      }
    }

    public float FieldOfView
    {
      get
      {
        float? floatFromString = MyUtils.GetFloatFromString(this.GetParameterValue(this.FIELD_OF_VIEW));
        return floatFromString.HasValue ? MathHelper.ToRadians(floatFromString.Value) : MyConstants.FIELD_OF_VIEW_CONFIG_DEFAULT;
      }
      set => this.SetParameterValue(this.FIELD_OF_VIEW, MathHelper.ToDegrees(value));
    }

    public bool? HqTarget
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.HQTARGET));
      set => this.SetParameterValue(this.HQTARGET, value);
    }

    public MyAntialiasingMode? AntialiasingMode
    {
      get => this.GetOptionalEnum<MyAntialiasingMode>(this.ANTIALIASING_MODE);
      set => this.SetOptionalEnum<MyAntialiasingMode>(this.ANTIALIASING_MODE, value);
    }

    public MyShadowsQuality? ShadowQuality
    {
      get => this.GetOptionalEnum<MyShadowsQuality>(this.SHADOW_MAP_RESOLUTION);
      set => this.SetOptionalEnum<MyShadowsQuality>(this.SHADOW_MAP_RESOLUTION, value);
    }

    public bool? AmbientOcclusionEnabled
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.AMBIENT_OCCLUSION_ENABLED));
      set => this.SetParameterValue(this.AMBIENT_OCCLUSION_ENABLED, value);
    }

    public bool PostProcessingEnabled
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.POSTPROCESSING_ENABLED), true);
      set => this.SetParameterValue(this.POSTPROCESSING_ENABLED, new bool?(value));
    }

    public MyTextureQuality? TextureQuality
    {
      get => this.GetOptionalEnum<MyTextureQuality>(this.TEXTURE_QUALITY);
      set => this.SetOptionalEnum<MyTextureQuality>(this.TEXTURE_QUALITY, value);
    }

    public MyTextureQuality? VoxelTextureQuality
    {
      get => this.GetOptionalEnum<MyTextureQuality>(this.VOXEL_TEXTURE_QUALITY);
      set => this.SetOptionalEnum<MyTextureQuality>(this.VOXEL_TEXTURE_QUALITY, value);
    }

    public MyRenderQualityEnum? ShaderQuality
    {
      get => this.GetOptionalEnum<MyRenderQualityEnum>(this.SHADER_QUALITY);
      set => this.SetOptionalEnum<MyRenderQualityEnum>(this.SHADER_QUALITY, value);
    }

    public MyTextureAnisoFiltering? AnisotropicFiltering
    {
      get => this.GetOptionalEnum<MyTextureAnisoFiltering>(this.ANISOTROPIC_FILTERING);
      set => this.SetOptionalEnum<MyTextureAnisoFiltering>(this.ANISOTROPIC_FILTERING, value);
    }

    public int? ScreenWidth
    {
      get => MyUtils.GetInt32FromString(this.GetParameterValue(this.SCREEN_WIDTH));
      set => this.SetParameterValue(this.SCREEN_WIDTH, value);
    }

    public int? ScreenHeight
    {
      get => MyUtils.GetInt32FromString(this.GetParameterValue(this.SCREEN_HEIGHT));
      set => this.SetParameterValue(this.SCREEN_HEIGHT, value);
    }

    public int VideoAdapter
    {
      get => MyUtils.GetIntFromString(this.GetParameterValue(this.VIDEO_ADAPTER), 0);
      set => this.SetParameterValue(this.VIDEO_ADAPTER, value);
    }

    public bool DisableUpdateDriverNotification
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.DISABLE_UPDATE_DRIVER_NOTIFICATION), false);
      set => this.SetParameterValue(this.DISABLE_UPDATE_DRIVER_NOTIFICATION, new bool?(value));
    }

    public MyWindowModeEnum WindowMode
    {
      get
      {
        string parameterValue = this.GetParameterValue(this.WINDOW_MODE);
        byte? nullable = new byte?();
        if (!string.IsNullOrEmpty(parameterValue))
        {
          nullable = MyUtils.GetByteFromString(parameterValue);
        }
        else
        {
          bool? boolFromString = MyUtils.GetBoolFromString(this.GetParameterValue(this.FULL_SCREEN));
          if (boolFromString.HasValue)
          {
            this.RemoveParameterValue(this.FULL_SCREEN);
            nullable = new byte?(boolFromString.Value ? (byte) 2 : (byte) 0);
            this.SetParameterValue(this.WINDOW_MODE, (int) nullable.Value);
          }
        }
        return !nullable.HasValue || !Enum.IsDefined(typeof (MyWindowModeEnum), (object) nullable) ? MyWindowModeEnum.Fullscreen : (MyWindowModeEnum) nullable.Value;
      }
      set => this.SetParameterValue(this.WINDOW_MODE, (int) value);
    }

    public bool CaptureMouse
    {
      get => !this.GetParameterValue(this.MOUSE_CAPTURE).Equals("False");
      set => this.SetParameterValue(this.MOUSE_CAPTURE, value.ToString());
    }

    public int VerticalSync
    {
      get => MyUtils.GetIntFromString(this.GetParameterValue(this.VERTICAL_SYNC), 1);
      set => this.SetParameterValue(this.VERTICAL_SYNC, value);
    }

    public int RefreshRate
    {
      get => MyUtils.GetIntFromString(this.GetParameterValue(this.REFRESH_RATE), 0);
      set => this.SetParameterValue(this.REFRESH_RATE, value);
    }

    public float FlaresIntensity
    {
      get => MyUtils.GetFloatFromString(this.GetParameterValue(this.FLARES_INTENSITY), 1f);
      set => this.SetParameterValue(this.FLARES_INTENSITY, value);
    }

    public bool? EnableDamageEffects
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.ENABLE_DAMAGE_EFFECTS));
      set => this.SetParameterValue(this.ENABLE_DAMAGE_EFFECTS, value);
    }

    public float GameVolume
    {
      get => MyUtils.GetFloatFromString(this.GetParameterValue(this.GAME_VOLUME), 1f);
      set => this.SetParameterValue(this.GAME_VOLUME, value);
    }

    public float MusicVolume
    {
      get
      {
        float? floatFromString = MyUtils.GetFloatFromString(this.GetParameterValue(this.MUSIC_VOLUME_OLD));
        if (floatFromString.HasValue)
        {
          if ((double) floatFromString.Value != 1.0)
            this.SetParameterValue(this.MUSIC_VOLUME, floatFromString.Value);
          this.RemoveParameterValue(this.MUSIC_VOLUME_OLD);
        }
        return MyUtils.GetFloatFromString(this.GetParameterValue(this.MUSIC_VOLUME), 0.5f);
      }
      set => this.SetParameterValue(this.MUSIC_VOLUME, value);
    }

    public float VoiceChatVolume
    {
      get => MyUtils.GetFloatFromString(this.GetParameterValue(this.VOICE_CHAT_VOLUME), 5f);
      set => this.SetParameterValue(this.VOICE_CHAT_VOLUME, value);
    }

    public bool ExperimentalMode
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.EXPERIMENTAL_MODE), true);
      set => this.SetParameterValue(this.EXPERIMENTAL_MODE, new bool?(value));
    }

    public bool ControlsHints
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.CONTROLS_HINTS), true);
      set => this.SetParameterValue(this.CONTROLS_HINTS, new bool?(value));
    }

    public bool GoodBotHints
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.GOODBOT_HINTS), true);
      set => this.SetParameterValue(this.GOODBOT_HINTS, new bool?(value));
    }

    public bool EnableNewNewGameScreen
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.NEW_NEW_GAME_SCREEN), false);
      set => this.SetParameterValue(this.NEW_NEW_GAME_SCREEN, new bool?(value));
    }

    public bool RotationHints
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.ROTATION_HINTS), true);
      set => this.SetParameterValue(this.ROTATION_HINTS, new bool?(value));
    }

    public MyConfig.CrosshairSwitch ShowCrosshair
    {
      get => (MyConfig.CrosshairSwitch) MyUtils.GetIntFromString(this.GetParameterValue(this.SHOW_CROSSHAIR), 0);
      set => this.SetParameterValue(this.SHOW_CROSSHAIR, (int) value);
    }

    public bool ShowCrosshairHUD
    {
      get
      {
        switch (this.ShowCrosshair)
        {
          case MyConfig.CrosshairSwitch.AlwaysVisible:
            return true;
          case MyConfig.CrosshairSwitch.AlwaysHidden:
            return false;
          default:
            return !MyHud.IsHudMinimal;
        }
      }
    }

    public bool EnableTrading
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.ENABLE_TRADING), true);
      set => this.SetParameterValue(this.ENABLE_TRADING, new bool?(value));
    }

    public bool ShowChatTimestamp
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.SHOW_CHAT_TIMESTAMP), true);
      set => this.SetParameterValue(this.SHOW_CHAT_TIMESTAMP, new bool?(value));
    }

    public List<string> CampaignsStarted
    {
      get
      {
        if (!this.m_values.Dictionary.ContainsKey(this.CAMPAIGNS_STARTED))
          this.m_values.Dictionary.Add(this.CAMPAIGNS_STARTED, (object) new List<string>());
        List<string> stringList = this.GetParameterValueT<List<string>>(this.CAMPAIGNS_STARTED);
        if (stringList == null)
        {
          stringList = new List<string>();
          this.m_values.Dictionary[this.CAMPAIGNS_STARTED] = (object) stringList;
        }
        return stringList;
      }
      set
      {
      }
    }

    public int NewNewGameScreenLastSelection
    {
      get => MyUtils.GetIntFromString(this.GetParameterValue(this.NEW_NEW_GAME_SCREEN_LAST_SELECTION), -1);
      set => this.SetParameterValue(this.NEW_NEW_GAME_SCREEN_LAST_SELECTION, value);
    }

    public bool EnableSteamCloud
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.ENABLE_STEAM_CLOUD), false) || MyPlatformGameSettings.CLOUD_ALWAYS_ENABLED;
      set => this.SetParameterValue(this.ENABLE_STEAM_CLOUD, new bool?(value));
    }

    public float ScreenshotSizeMultiplier
    {
      get
      {
        if (string.IsNullOrEmpty(this.GetParameterValue(this.SCREENSHOT_SIZE_MULTIPLIER)))
        {
          this.SetParameterValue(this.SCREENSHOT_SIZE_MULTIPLIER, 1f);
          this.Save();
        }
        return MyUtils.GetFloatFromString(this.GetParameterValue(this.SCREENSHOT_SIZE_MULTIPLIER), 1f);
      }
      set => this.SetParameterValue(this.SCREENSHOT_SIZE_MULTIPLIER, value);
    }

    public MyLanguagesEnum Language
    {
      get
      {
        byte? byteFromString = MyUtils.GetByteFromString(this.GetParameterValue(this.LANGUAGE));
        return !byteFromString.HasValue || !Enum.IsDefined(typeof (MyLanguagesEnum), (object) byteFromString) ? MyLanguage.GetOsLanguageCurrentOfficial() : (MyLanguagesEnum) byteFromString.Value;
      }
      set => this.SetParameterValue(this.LANGUAGE, (int) value);
    }

    public string Skin
    {
      get
      {
        if (string.IsNullOrEmpty(this.GetParameterValue(this.SKIN)))
        {
          this.SetParameterValue(this.SKIN, "Default");
          this.Save();
        }
        return this.GetParameterValue(this.SKIN);
      }
      set => this.SetParameterValue(this.SKIN, value);
    }

    public SerializableDictionary<string, string> ControlsGeneral
    {
      get => this.GetParameterValueDictionary<string>(this.CONTROLS_GENERAL);
      set
      {
      }
    }

    public SerializableDictionary<string, SerializableDictionary<string, string>> ControlsButtons
    {
      get => this.GetParameterValueDictionary<SerializableDictionary<string, string>>(this.CONTROLS_BUTTONS);
      set
      {
      }
    }

    private static string SerialiazeAndEncod64(object p)
    {
      if (p == null)
        return "";
      MemoryStream memoryStream = new MemoryStream();
      new BinaryFormatter().Serialize((Stream) memoryStream, p);
      return Convert.ToBase64String(memoryStream.GetBuffer());
    }

    private static object Decode64AndDeserialize(string p)
    {
      switch (p)
      {
        case "":
        case null:
          return (object) null;
        default:
          return new BinaryFormatter().Deserialize((Stream) new MemoryStream(Convert.FromBase64String(p)));
      }
    }

    public SerializableDictionary<string, MyConfig.MyDebugInputData> DebugInputComponents
    {
      get
      {
        if (!this.m_values.Dictionary.ContainsKey(this.DEBUG_INPUT_COMPONENTS))
          this.m_values.Dictionary.Add(this.DEBUG_INPUT_COMPONENTS, (object) new SerializableDictionary<string, MyConfig.MyDebugInputData>());
        else if (!(this.m_values.Dictionary[this.DEBUG_INPUT_COMPONENTS] is SerializableDictionary<string, MyConfig.MyDebugInputData>))
          this.m_values.Dictionary[this.DEBUG_INPUT_COMPONENTS] = (object) new SerializableDictionary<string, MyConfig.MyDebugInputData>();
        return this.GetParameterValueT<SerializableDictionary<string, MyConfig.MyDebugInputData>>(this.DEBUG_INPUT_COMPONENTS);
      }
      set
      {
      }
    }

    public MyDebugComponent.MyDebugComponentInfoState DebugComponentsInfo
    {
      get
      {
        int? intFromString = MyUtils.GetIntFromString(this.GetParameterValue(this.DEBUG_INPUT_COMPONENTS_INFO));
        return !intFromString.HasValue || !Enum.IsDefined(typeof (MyDebugComponent.MyDebugComponentInfoState), (object) intFromString) ? MyDebugComponent.MyDebugComponentInfoState.EnabledInfo : (MyDebugComponent.MyDebugComponentInfoState) intFromString.Value;
      }
      set => this.SetParameterValue(this.DEBUG_INPUT_COMPONENTS_INFO, (int) value);
    }

    public bool MinimalHud
    {
      get => MyHud.IsHudMinimal || MyHud.MinimalHud;
      set => this.HudState = value ? 0 : 1;
    }

    public int HudState
    {
      get => MyUtils.GetIntFromString(this.GetParameterValue(this.HUD_STATE), 1);
      set => this.SetParameterValue(this.HUD_STATE, value);
    }

    public bool MemoryLimits
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.MEMORY_LIMITS), true);
      set => this.SetParameterValue(this.MEMORY_LIMITS, new bool?(value));
    }

    public bool CubeBuilderUseSymmetry
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.CUBE_BUILDER_USE_SYMMETRY), true);
      set => this.SetParameterValue(this.CUBE_BUILDER_USE_SYMMETRY, new bool?(value));
    }

    public int CubeBuilderBuildingMode
    {
      get => MyUtils.GetIntFromString(this.GetParameterValue(this.CUBE_BUILDER_BUILDING_MODE), 0);
      set => this.SetParameterValue(this.CUBE_BUILDER_BUILDING_MODE, value);
    }

    public bool CubeBuilderAlignToDefault
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.CUBE_BUILDER_ALIGN_TO_DEFAULT), true);
      set => this.SetParameterValue(this.CUBE_BUILDER_ALIGN_TO_DEFAULT, new bool?(value));
    }

    public bool MultiplayerShowCompatible
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.MULTIPLAYER_SHOWCOMPATIBLE), true);
      set => this.SetParameterValue(this.MULTIPLAYER_SHOWCOMPATIBLE, new bool?(value));
    }

    public bool EnablePerformanceWarnings
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.ENABLE_PERFORMANCE_WARNINGS_TEMP), true);
      set => this.SetParameterValue(this.ENABLE_PERFORMANCE_WARNINGS_TEMP, new bool?(value));
    }

    public int LastCheckedVersion
    {
      get => MyUtils.GetIntFromString(this.GetParameterValue(this.LAST_CHECKED_VERSION), 0);
      set => this.SetParameterValue(this.LAST_CHECKED_VERSION, value);
    }

    public float UIOpacity
    {
      get => MyUtils.GetFloatFromString(this.GetParameterValue(this.UI_TRANSPARENCY), 1f);
      set => this.SetParameterValue(this.UI_TRANSPARENCY, value);
    }

    public float UIBkOpacity
    {
      get => MyUtils.GetFloatFromString(this.GetParameterValue(this.UI_BK_TRANSPARENCY), 0.8f);
      set => this.SetParameterValue(this.UI_BK_TRANSPARENCY, value);
    }

    public float HUDBkOpacity
    {
      get => MyUtils.GetFloatFromString(this.GetParameterValue(this.HUD_BK_TRANSPARENCY), 0.6f);
      set => this.SetParameterValue(this.HUD_BK_TRANSPARENCY, value);
    }

    public List<string> TutorialsFinished
    {
      get
      {
        if (!this.m_values.Dictionary.ContainsKey(this.TUTORIALS_FINISHED))
          this.m_values.Dictionary.Add(this.TUTORIALS_FINISHED, (object) new List<string>());
        if (this.m_values.Dictionary[this.TUTORIALS_FINISHED] != null && this.m_values.Dictionary[this.TUTORIALS_FINISHED].GetType() != typeof (List<string>))
          this.m_values.Dictionary[this.TUTORIALS_FINISHED] = (object) new List<string>();
        return (List<string>) this.m_values.Dictionary[this.TUTORIALS_FINISHED];
      }
      set
      {
      }
    }

    public bool HudWarnings
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.HUD_WARNINGS), true);
      set => this.SetParameterValue(this.HUD_WARNINGS, new bool?(value));
    }

    public bool EnableVoiceChat
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.ENABLE_VOICE_CHAT), true);
      set => this.SetParameterValue(this.ENABLE_VOICE_CHAT, new bool?(value));
    }

    public bool EnableMuteWhenNotInFocus
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.ENABLE_MUTE_WHEN_NOT_IN_FOCUS), true);
      set => this.SetParameterValue(this.ENABLE_MUTE_WHEN_NOT_IN_FOCUS, new bool?(value));
    }

    public bool EnableDynamicMusic
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.DYNAMIC_MUSIC), true);
      set => this.SetParameterValue(this.DYNAMIC_MUSIC, new bool?(value));
    }

    public bool ShipSoundsAreBasedOnSpeed
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.SHIP_SOUNDS_SPEED), true);
      set => this.SetParameterValue(this.SHIP_SOUNDS_SPEED, new bool?(value));
    }

    public bool EnableReverb
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.ENABLE_REVERB), true);
      set => this.SetParameterValue(this.ENABLE_REVERB, new bool?(value));
    }

    public MyStringId GraphicsRenderer
    {
      get
      {
        string parameterValue = this.GetParameterValue(this.GRAPHICS_RENDERER);
        if (string.IsNullOrEmpty(parameterValue))
          return MyPerGameSettings.DefaultGraphicsRenderer;
        MyStringId myStringId = MyStringId.TryGet(parameterValue);
        return myStringId == MyStringId.NullOrEmpty ? MyPerGameSettings.DefaultGraphicsRenderer : myStringId;
      }
      set => this.SetParameterValue(this.GRAPHICS_RENDERER, value.ToString());
    }

    public MyObjectBuilder_ServerFilterOptions ServerSearchSettings
    {
      get
      {
        object obj;
        this.m_values.Dictionary.TryGetValue(this.SERVER_SEARCH_SETTINGS, out obj);
        return obj as MyObjectBuilder_ServerFilterOptions;
      }
      set => this.m_values.Dictionary[this.SERVER_SEARCH_SETTINGS] = (object) value;
    }

    public bool EnableDoppler
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.ENABLE_DOPPLER), true);
      set => this.SetParameterValue(this.ENABLE_DOPPLER, new bool?(value));
    }

    private HashSet<ulong> GetSeparatedValues(string key, ref HashSet<ulong> cache)
    {
      if (cache == null)
      {
        string str1 = "";
        if (!this.m_values.Dictionary.ContainsKey(key))
          this.m_values.Dictionary.Add(key, (object) "");
        else
          str1 = this.GetParameterValue(key);
        cache = new HashSet<ulong>();
        string str2 = str1;
        char[] chArray = new char[1]{ ',' };
        foreach (string str3 in str2.Split(chArray))
        {
          if (str3.Length > 0)
            cache.Add(Convert.ToUInt64(str3));
        }
      }
      return cache;
    }

    private void SetSeparatedValues(string key, HashSet<ulong> value, ref HashSet<ulong> cache)
    {
      cache = value;
      string str = "";
      foreach (ulong num in value)
        str = str + num.ToString() + ",";
      this.SetParameterValue(key, str);
    }

    public HashSet<ulong> MutedPlayers
    {
      get => this.GetSeparatedValues(this.MUTED_PLAYERS, ref this.m_mutedPlayers);
      set => this.SetSeparatedValues(this.MUTED_PLAYERS, value, ref this.m_mutedPlayers);
    }

    public MyConfig.LowMemSwitch LowMemSwitchToLow
    {
      get => (MyConfig.LowMemSwitch) MyUtils.GetIntFromString(this.GetParameterValue(this.LOW_MEM_SWITCH_TO_LOW), 0);
      set => this.SetParameterValue(this.LOW_MEM_SWITCH_TO_LOW, (int) value);
    }

    public MyConfig.NewsletterStatus NewsletterCurrentStatus
    {
      get => (MyConfig.NewsletterStatus) MyUtils.GetIntFromString(this.GetParameterValue(this.NEWSLETTER_CURRENT_STATUS), 1);
      set => this.SetParameterValue(this.NEWSLETTER_CURRENT_STATUS, (int) value);
    }

    public MyConfig.WelcomeScreenStatus WelcomScreenCurrentStatus
    {
      get => (MyConfig.WelcomeScreenStatus) MyUtils.GetIntFromString(this.GetParameterValue(this.WELCOMESCREEN_CURRENT_STATUS), 0);
      set => this.SetParameterValue(this.WELCOMESCREEN_CURRENT_STATUS, (int) value);
    }

    public int GamepadSchemeId
    {
      get => MyUtils.GetIntFromString(this.GetParameterValue(this.GAMEPAD_SCHEME), 0);
      set => this.SetParameterValue(this.GAMEPAD_SCHEME, value);
    }

    public bool ControllerDefaultOnStart
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.CONTROLLER_DEFAULT_ON_START), false);
      set => this.SetParameterValue(this.CONTROLLER_DEFAULT_ON_START, new bool?(value));
    }

    public float ZoomMultiplier
    {
      get => MyUtils.GetFloatFromString(this.GetParameterValue(this.ZOOM_MULTIPLIER), 0.2f);
      set => this.SetParameterValue(this.ZOOM_MULTIPLIER, value);
    }

    public bool DebugOverrideAutosave
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.DEBUG_OVERRIDE_AUTOSAVE), false);
      set => this.SetParameterValue(this.DEBUG_OVERRIDE_AUTOSAVE, new bool?(value));
    }

    public bool ModIoConsent
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.MODIO_CONSENT), false);
      set => this.SetParameterValue(this.MODIO_CONSENT, new bool?(value));
    }

    public bool SteamConsent
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.STEAM_CONSENT), false);
      set => this.SetParameterValue(this.STEAM_CONSENT, new bool?(value));
    }

    public bool IsSetToLowQuality()
    {
      MyTextureAnisoFiltering? anisotropicFiltering = this.AnisotropicFiltering;
      MyTextureAnisoFiltering textureAnisoFiltering = MyTextureAnisoFiltering.NONE;
      if (anisotropicFiltering.GetValueOrDefault() == textureAnisoFiltering & anisotropicFiltering.HasValue)
      {
        MyAntialiasingMode? antialiasingMode1 = this.AntialiasingMode;
        MyAntialiasingMode antialiasingMode2 = MyAntialiasingMode.NONE;
        if (antialiasingMode1.GetValueOrDefault() == antialiasingMode2 & antialiasingMode1.HasValue)
        {
          MyShadowsQuality? shadowQuality = this.ShadowQuality;
          MyShadowsQuality myShadowsQuality = MyShadowsQuality.LOW;
          if (shadowQuality.GetValueOrDefault() == myShadowsQuality & shadowQuality.HasValue)
          {
            MyTextureQuality? textureQuality = this.TextureQuality;
            MyTextureQuality myTextureQuality1 = MyTextureQuality.LOW;
            if (textureQuality.GetValueOrDefault() == myTextureQuality1 & textureQuality.HasValue)
            {
              MyTextureQuality? voxelTextureQuality = this.VoxelTextureQuality;
              MyTextureQuality myTextureQuality2 = MyTextureQuality.LOW;
              if (voxelTextureQuality.GetValueOrDefault() == myTextureQuality2 & voxelTextureQuality.HasValue)
              {
                MyRenderQualityEnum? modelQuality = this.ModelQuality;
                MyRenderQualityEnum renderQualityEnum1 = MyRenderQualityEnum.LOW;
                if (modelQuality.GetValueOrDefault() == renderQualityEnum1 & modelQuality.HasValue)
                {
                  MyRenderQualityEnum? voxelQuality = this.VoxelQuality;
                  MyRenderQualityEnum renderQualityEnum2 = MyRenderQualityEnum.LOW;
                  if (voxelQuality.GetValueOrDefault() == renderQualityEnum2 & voxelQuality.HasValue)
                  {
                    float? grassDrawDistance = this.GrassDrawDistance;
                    float num = 0.0f;
                    if ((double) grassDrawDistance.GetValueOrDefault() == (double) num & grassDrawDistance.HasValue)
                      return true;
                  }
                }
              }
            }
          }
        }
      }
      return false;
    }

    public void SetToLowQuality()
    {
      this.AnisotropicFiltering = new MyTextureAnisoFiltering?(MyTextureAnisoFiltering.NONE);
      this.AntialiasingMode = new MyAntialiasingMode?(MyAntialiasingMode.NONE);
      this.ShadowQuality = new MyShadowsQuality?(MyShadowsQuality.LOW);
      this.TextureQuality = new MyTextureQuality?(MyTextureQuality.LOW);
      this.VoxelTextureQuality = new MyTextureQuality?(MyTextureQuality.LOW);
      this.ModelQuality = new MyRenderQualityEnum?(MyRenderQualityEnum.LOW);
      this.VoxelQuality = new MyRenderQualityEnum?(MyRenderQualityEnum.LOW);
      this.GrassDrawDistance = new float?(0.0f);
    }

    MyTextureAnisoFiltering? IMyConfig.AnisotropicFiltering => this.AnisotropicFiltering;

    MyAntialiasingMode? IMyConfig.AntialiasingMode => this.AntialiasingMode;

    bool IMyConfig.ControlsHints => this.ControlsHints;

    int IMyConfig.CubeBuilderBuildingMode => this.CubeBuilderBuildingMode;

    bool IMyConfig.CubeBuilderUseSymmetry => this.CubeBuilderUseSymmetry;

    bool IMyConfig.EnableDamageEffects => !this.EnableDamageEffects.HasValue || this.EnableDamageEffects.Value;

    float IMyConfig.FieldOfView => this.FieldOfView;

    float IMyConfig.GameVolume => this.GameVolume;

    bool IMyConfig.HudWarnings => this.HudWarnings;

    MyLanguagesEnum IMyConfig.Language => this.Language;

    bool IMyConfig.MemoryLimits => this.MemoryLimits;

    int IMyConfig.HudState => this.HudState;

    float IMyConfig.MusicVolume => this.MusicVolume;

    int IMyConfig.RefreshRate => this.RefreshRate;

    MyGraphicsRenderer IMyConfig.GraphicsRenderer => MyStringId.TryGet(this.GetParameterValue(this.GRAPHICS_RENDERER)) == MySandboxGame.DirectX11RendererKey ? MyGraphicsRenderer.DX11 : MyGraphicsRenderer.NONE;

    bool IMyConfig.RotationHints => this.RotationHints;

    int? IMyConfig.ScreenHeight => this.ScreenHeight;

    int? IMyConfig.ScreenWidth => this.ScreenWidth;

    MyShadowsQuality? IMyConfig.ShadowQuality => this.ShadowQuality;

    bool IMyConfig.ShowCrosshair => this.ShowCrosshair == MyConfig.CrosshairSwitch.AlwaysVisible;

    int IMyConfig.ShowCrosshair2 => (int) this.ShowCrosshair;

    bool IMyConfig.EnableTrading => this.EnableTrading;

    MyTextureQuality? IMyConfig.TextureQuality => this.TextureQuality;

    MyTextureQuality? IMyConfig.VoxelTextureQuality => this.VoxelTextureQuality;

    int IMyConfig.VerticalSync => this.VerticalSync;

    int IMyConfig.VideoAdapter => this.VideoAdapter;

    MyWindowModeEnum IMyConfig.WindowMode => this.WindowMode;

    bool IMyConfig.CaptureMouse => this.CaptureMouse;

    bool? IMyConfig.AmbientOcclusionEnabled => this.AmbientOcclusionEnabled;

    DictionaryReader<string, object> IMyConfig.ControlsButtons => DictionaryReader<string, object>.Empty;

    DictionaryReader<string, object> IMyConfig.ControlsGeneral => DictionaryReader<string, object>.Empty;

    bool IMyConfig.EnableDynamicMusic => this.EnableDynamicMusic;

    bool IMyConfig.EnableMuteWhenNotInFocus => this.EnableMuteWhenNotInFocus;

    bool IMyConfig.EnablePerformanceWarnings => this.EnablePerformanceWarnings;

    bool IMyConfig.EnableReverb => this.EnableReverb;

    bool IMyConfig.EnableVoiceChat => this.EnableVoiceChat;

    bool IMyConfig.FirstTimeRun => this.FirstTimeRun;

    float IMyConfig.FlaresIntensity => this.FlaresIntensity;

    float? IMyConfig.GrassDensityFactor => this.GrassDensityFactor;

    float? IMyConfig.GrassDrawDistance => this.GrassDrawDistance;

    float IMyConfig.HUDBkOpacity => this.HUDBkOpacity;

    MyRenderQualityEnum? IMyConfig.ModelQuality => this.ModelQuality;

    HashSetReader<ulong> IMyConfig.MutedPlayers => (HashSetReader<ulong>) this.MutedPlayers;

    float IMyConfig.ScreenshotSizeMultiplier => this.ScreenshotSizeMultiplier;

    MyRenderQualityEnum? IMyConfig.ShaderQuality => this.ShaderQuality;

    bool IMyConfig.ShipSoundsAreBasedOnSpeed => this.ShipSoundsAreBasedOnSpeed;

    string IMyConfig.Skin => this.Skin;

    float IMyConfig.UIBkOpacity => this.UIBkOpacity;

    float IMyConfig.UIOpacity => this.UIOpacity;

    float? IMyConfig.VegetationDrawDistance => this.VegetationDrawDistance;

    float IMyConfig.VoiceChatVolume => this.VoiceChatVolume;

    MyRenderQualityEnum? IMyConfig.VoxelQuality => this.VoxelQuality;

    internal void SetToMediumQuality()
    {
      MyPerformanceSettings performanceSettings = new MyPerformanceSettings()
      {
        RenderSettings = new MyRenderSettings1()
        {
          AnisotropicFiltering = MyTextureAnisoFiltering.NONE,
          AntialiasingMode = MyAntialiasingMode.FXAA,
          ShadowQuality = MyShadowsQuality.MEDIUM,
          ShadowGPUQuality = MyRenderQualityEnum.NORMAL,
          AmbientOcclusionEnabled = true,
          TextureQuality = MyTextureQuality.MEDIUM,
          VoxelTextureQuality = MyTextureQuality.MEDIUM,
          ModelQuality = MyRenderQualityEnum.NORMAL,
          VoxelQuality = MyRenderQualityEnum.NORMAL,
          GrassDrawDistance = 160f,
          GrassDensityFactor = 1f,
          HqDepth = true,
          VoxelShaderQuality = MyRenderQualityEnum.NORMAL,
          AlphaMaskedShaderQuality = MyRenderQualityEnum.NORMAL,
          AtmosphereShaderQuality = MyRenderQualityEnum.NORMAL,
          DistanceFade = 100f,
          ParticleQuality = MyRenderQualityEnum.NORMAL
        },
        EnableDamageEffects = true
      };
      MyGraphicsSettings graphicsSettings = MyVideoSettingsManager.CurrentGraphicsSettings;
      graphicsSettings.PerformanceSettings = performanceSettings;
      int num = (int) MyVideoSettingsManager.Apply(graphicsSettings);
      MyVideoSettingsManager.SaveCurrentSettings();
    }

    public bool? GDPRConsent
    {
      get
      {
        string parameterValue = this.GetParameterValue(this.GDPR_CONSENT);
        return string.IsNullOrWhiteSpace(parameterValue) ? new bool?() : new bool?(MyUtils.GetBoolFromString(parameterValue, false));
      }
      set => this.SetParameterValue(this.GDPR_CONSENT, new bool?(value.Value));
    }

    public bool AreaInteraction
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.AREA_INTERACTION), true);
      set => this.SetParameterValue(this.AREA_INTERACTION, new bool?(value));
    }

    public bool? GDPRConsentSent
    {
      get
      {
        string parameterValue = this.GetParameterValue(this.GDPR_CONSENT_SENT);
        return string.IsNullOrWhiteSpace(parameterValue) ? new bool?() : new bool?(MyUtils.GetBoolFromString(parameterValue, false));
      }
      set => this.SetParameterValue(this.GDPR_CONSENT_SENT, new bool?(value.Value));
    }

    public float MicSensitivity
    {
      get => MyUtils.GetFloatFromString(this.GetParameterValue(this.MIC_SENSITIVITY), MyFakes.VOICE_CHAT_MIC_SENSITIVITY);
      set => this.SetParameterValue(this.MIC_SENSITIVITY, value);
    }

    public bool AutomaticVoiceChatActivation
    {
      get => MyUtils.GetBoolFromString(this.GetParameterValue(this.AUTOMATIC_VOICE_CHAT_ACTIVATION), MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION);
      set => this.SetParameterValue(this.AUTOMATIC_VOICE_CHAT_ACTIVATION, new bool?(value));
    }

    public float SpriteMainViewportScale
    {
      get => MyUtils.GetFloatFromString(this.GetParameterValue(this.SPRITE_MAIN_VIEWPORT_SCALE), 1f);
      set => this.SetParameterValue(this.SPRITE_MAIN_VIEWPORT_SCALE, value);
    }

    protected override void NewConfigWasStarted()
    {
      base.NewConfigWasStarted();
      this.EnableNewNewGameScreen = MyPlatformGameSettings.ENABLE_SIMPLE_NEWGAME_SCREEN;
      MyNewGameScreenABTestHelper.Instance.ActivateTest();
    }

    public IronSightSwitchStateType IronSightSwitchState
    {
      get => (IronSightSwitchStateType) MyUtils.GetIntFromString(this.GetParameterValue(this.IRON_SIGHT_SWITCH_STATE), 1);
      set => this.SetParameterValue(this.IRON_SIGHT_SWITCH_STATE, (int) value);
    }

    public Color HitIndicatorColorCharacter
    {
      get => new Color(MyUtils.GetUIntFromString(this.GetParameterValue(this.HIT_INDICATOR_COLOR_CHARACTER), new Color(149, 169, 179).PackedValue));
      set => this.SetParameterValue(this.HIT_INDICATOR_COLOR_CHARACTER, value.PackedValue);
    }

    public Color HitIndicatorColorHeadshot
    {
      get => new Color(MyUtils.GetUIntFromString(this.GetParameterValue(this.HIT_INDICATOR_COLOR_HEADSHOT), new Color(232, 179, 35).PackedValue));
      set => this.SetParameterValue(this.HIT_INDICATOR_COLOR_HEADSHOT, value.PackedValue);
    }

    public Color HitIndicatorColorKill
    {
      get => new Color(MyUtils.GetUIntFromString(this.GetParameterValue(this.HIT_INDICATOR_COLOR_KILL), new Color(228, 62, 62).PackedValue));
      set => this.SetParameterValue(this.HIT_INDICATOR_COLOR_KILL, value.PackedValue);
    }

    public Color HitIndicatorColorGrid
    {
      get => new Color(MyUtils.GetUIntFromString(this.GetParameterValue(this.HIT_INDICATOR_COLOR_GRID), new Color(117, 201, 241).PackedValue));
      set => this.SetParameterValue(this.HIT_INDICATOR_COLOR_GRID, value.PackedValue);
    }

    public Color HitIndicatorColorFriend
    {
      get => new Color(MyUtils.GetUIntFromString(this.GetParameterValue(this.HIT_INDICATOR_COLOR_FRIEND), new Color(101, 178, 91).PackedValue));
      set => this.SetParameterValue(this.HIT_INDICATOR_COLOR_FRIEND, value.PackedValue);
    }

    public string HitIndicatorTextureCharacter
    {
      get
      {
        string parameterValue = this.GetParameterValue(this.HIT_INDICATOR_TEXTURE_CHARACTER);
        return string.IsNullOrEmpty(parameterValue) ? "Textures\\GUI\\Indicators\\HitIndicator4.png" : parameterValue;
      }
      set => this.SetParameterValue(this.HIT_INDICATOR_TEXTURE_CHARACTER, value);
    }

    public string HitIndicatorTextureHeadshot
    {
      get
      {
        string parameterValue = this.GetParameterValue(this.HIT_INDICATOR_TEXTURE_HEADSHOT);
        return string.IsNullOrEmpty(parameterValue) ? "Textures\\GUI\\Indicators\\HitIndicator4.png" : parameterValue;
      }
      set => this.SetParameterValue(this.HIT_INDICATOR_TEXTURE_HEADSHOT, value);
    }

    public string HitIndicatorTextureKill
    {
      get
      {
        string parameterValue = this.GetParameterValue(this.HIT_INDICATOR_TEXTURE_KILL);
        return string.IsNullOrEmpty(parameterValue) ? "Textures\\GUI\\Indicators\\HitIndicator4.png" : parameterValue;
      }
      set => this.SetParameterValue(this.HIT_INDICATOR_TEXTURE_KILL, value);
    }

    public string HitIndicatorTextureGrid
    {
      get
      {
        string parameterValue = this.GetParameterValue(this.HIT_INDICATOR_TEXTURE_GRID);
        return string.IsNullOrEmpty(parameterValue) ? "Textures\\GUI\\Indicators\\HitIndicator4.png" : parameterValue;
      }
      set => this.SetParameterValue(this.HIT_INDICATOR_TEXTURE_GRID, value);
    }

    public string HitIndicatorTextureFriend
    {
      get
      {
        string parameterValue = this.GetParameterValue(this.HIT_INDICATOR_TEXTURE_FRIEND);
        return string.IsNullOrEmpty(parameterValue) ? "Textures\\GUI\\Indicators\\HitIndicator4.png" : parameterValue;
      }
      set => this.SetParameterValue(this.HIT_INDICATOR_TEXTURE_FRIEND, value);
    }

    public enum LowMemSwitch
    {
      ARMED,
      TRIGGERED,
      USER_SAID_NO,
    }

    public enum NewsletterStatus
    {
      Unknown,
      NoFeedback,
      NotInterested,
      EmailNotConfirmed,
      EmailConfirmed,
    }

    public enum WelcomeScreenStatus
    {
      NotSeen,
      AlreadySeen,
    }

    public enum CrosshairSwitch
    {
      VisibleWithHud,
      AlwaysVisible,
      AlwaysHidden,
    }

    [ProtoContract]
    public struct MyDebugInputData
    {
      [ProtoMember(1)]
      public bool Enabled;
      [ProtoMember(4)]
      public string SerializedData;

      public object Data
      {
        get => MyConfig.Decode64AndDeserialize(this.SerializedData);
        set => this.SerializedData = MyConfig.SerialiazeAndEncod64(value);
      }

      public bool ShouldSerializeData() => false;

      protected class Sandbox_Engine_Utils_MyConfig\u003C\u003EMyDebugInputData\u003C\u003EEnabled\u003C\u003EAccessor : IMemberAccessor<MyConfig.MyDebugInputData, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyConfig.MyDebugInputData owner, in bool value) => owner.Enabled = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyConfig.MyDebugInputData owner, out bool value) => value = owner.Enabled;
      }

      protected class Sandbox_Engine_Utils_MyConfig\u003C\u003EMyDebugInputData\u003C\u003ESerializedData\u003C\u003EAccessor : IMemberAccessor<MyConfig.MyDebugInputData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyConfig.MyDebugInputData owner, in string value) => owner.SerializedData = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyConfig.MyDebugInputData owner, out string value) => value = owner.SerializedData;
      }

      protected class Sandbox_Engine_Utils_MyConfig\u003C\u003EMyDebugInputData\u003C\u003EData\u003C\u003EAccessor : IMemberAccessor<MyConfig.MyDebugInputData, object>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyConfig.MyDebugInputData owner, in object value) => owner.Data = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyConfig.MyDebugInputData owner, out object value) => value = owner.Data;
      }

      private class Sandbox_Engine_Utils_MyConfig\u003C\u003EMyDebugInputData\u003C\u003EActor : IActivator, IActivator<MyConfig.MyDebugInputData>
      {
        object IActivator.CreateInstance() => (object) new MyConfig.MyDebugInputData();

        MyConfig.MyDebugInputData IActivator<MyConfig.MyDebugInputData>.CreateInstance() => new MyConfig.MyDebugInputData();
      }
    }
  }
}
