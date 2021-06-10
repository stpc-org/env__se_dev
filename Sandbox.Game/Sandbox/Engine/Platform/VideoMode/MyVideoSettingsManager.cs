// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Platform.VideoMode.MyVideoSettingsManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.AppCode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Engine.Platform.VideoMode
{
  public static class MyVideoSettingsManager
  {
    private static Dictionary<int, MyAspectRatio> m_recommendedAspectRatio;
    private static MyAdapterInfo[] m_adapters;
    private static readonly MyAspectRatio[] m_aspectRatios = new MyAspectRatio[MyUtils.GetMaxValueFromEnum<MyAspectRatioEnum>() + 1];
    private static MyRenderDeviceSettings m_currentDeviceSettings;
    private static bool m_currentDeviceIsTripleHead;
    private static MyGraphicsSettings m_currentGraphicsSettings;
    public static readonly MyDisplayMode[] DebugDisplayModes;

    public static event Action OnSettingsChanged;

    public static MyAdapterInfo[] Adapters => MyVideoSettingsManager.m_adapters;

    public static MyRenderDeviceSettings CurrentDeviceSettings => MyVideoSettingsManager.m_currentDeviceSettings;

    public static MyGraphicsSettings CurrentGraphicsSettings => MyVideoSettingsManager.m_currentGraphicsSettings;

    public static MyStringId RunningGraphicsRenderer { get; private set; }

    public static bool GpuUnderMinimum { get; private set; }

    static MyVideoSettingsManager()
    {
      Action<bool, MyAspectRatioEnum, float, string, bool> action = (Action<bool, MyAspectRatioEnum, float, string, bool>) ((isTripleHead, aspectRatioEnum, aspectRatioNumber, textShort, isSupported) => MyVideoSettingsManager.m_aspectRatios[(int) aspectRatioEnum] = new MyAspectRatio(isTripleHead, aspectRatioEnum, aspectRatioNumber, textShort, isSupported));
      action(false, MyAspectRatioEnum.Normal_4_3, 1.333333f, "4:3", true);
      action(false, MyAspectRatioEnum.Normal_16_9, 1.777778f, "16:9", true);
      action(false, MyAspectRatioEnum.Normal_16_10, 1.6f, "16:10", true);
      action(false, MyAspectRatioEnum.Dual_4_3, 2.666667f, "Dual 4:3", true);
      action(false, MyAspectRatioEnum.Dual_16_9, 3.555556f, "Dual 16:9", true);
      action(false, MyAspectRatioEnum.Dual_16_10, 3.2f, "Dual 16:10", true);
      action(true, MyAspectRatioEnum.Triple_4_3, 4f, "Triple 4:3", true);
      action(true, MyAspectRatioEnum.Triple_16_9, 5.333333f, "Triple 16:9", true);
      action(true, MyAspectRatioEnum.Triple_16_10, 4.8f, "Triple 16:10", true);
      action(false, MyAspectRatioEnum.Unsupported_5_4, 1.25f, "5:4", false);
      MyVideoSettingsManager.DebugDisplayModes = new MyDisplayMode[0];
    }

    public static void UpdateRenderSettingsFromConfig(
      ref MyPerformanceSettings defaults,
      bool force = false)
    {
      int num = (int) MyVideoSettingsManager.Apply(MyVideoSettingsManager.GetGraphicsSettingsFromConfig(ref defaults, force));
    }

    public static MyGraphicsSettings GetGraphicsSettingsFromConfig(
      ref MyPerformanceSettings defaults,
      bool force)
    {
      MyGraphicsSettings graphicsSettings = MyVideoSettingsManager.CurrentGraphicsSettings;
      MyConfig config = MySandboxGame.Config;
      graphicsSettings.PerformanceSettings = defaults;
      graphicsSettings.GraphicsRenderer = config.GraphicsRenderer;
      graphicsSettings.FieldOfView = config.FieldOfView;
      graphicsSettings.PostProcessingEnabled = config.PostProcessingEnabled;
      graphicsSettings.FlaresIntensity = config.FlaresIntensity;
      if (!config.EnableDamageEffects.HasValue)
        config.EnableDamageEffects = new bool?(defaults.EnableDamageEffects);
      if (force)
      {
        graphicsSettings.PerformanceSettings.EnableDamageEffects = defaults.EnableDamageEffects;
        graphicsSettings.PerformanceSettings.RenderSettings = defaults.RenderSettings;
      }
      else
      {
        graphicsSettings.PerformanceSettings.EnableDamageEffects = config.EnableDamageEffects.Value;
        ref MyRenderSettings1 local1 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        float? vegetationDrawDistance = config.VegetationDrawDistance;
        double num1 = vegetationDrawDistance.HasValue ? (double) vegetationDrawDistance.GetValueOrDefault() : (double) defaults.RenderSettings.DistanceFade;
        local1.DistanceFade = (float) num1;
        ref MyRenderSettings1 local2 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        float? nullable1 = config.GrassDensityFactor;
        double num2 = nullable1.HasValue ? (double) nullable1.GetValueOrDefault() : (double) defaults.RenderSettings.GrassDensityFactor;
        local2.GrassDensityFactor = (float) num2;
        ref MyRenderSettings1 local3 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        nullable1 = config.GrassDrawDistance;
        double num3 = nullable1.HasValue ? (double) nullable1.GetValueOrDefault() : (double) defaults.RenderSettings.GrassDrawDistance;
        local3.GrassDrawDistance = (float) num3;
        ref MyRenderSettings1 local4 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        MyAntialiasingMode? antialiasingMode = config.AntialiasingMode;
        int num4 = antialiasingMode.HasValue ? (int) antialiasingMode.GetValueOrDefault() : (int) defaults.RenderSettings.AntialiasingMode;
        local4.AntialiasingMode = (MyAntialiasingMode) num4;
        ref MyRenderSettings1 local5 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        MyShadowsQuality? shadowQuality = config.ShadowQuality;
        int num5 = shadowQuality.HasValue ? (int) shadowQuality.GetValueOrDefault() : (int) defaults.RenderSettings.ShadowQuality;
        local5.ShadowQuality = (MyShadowsQuality) num5;
        ref MyRenderSettings1 local6 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        bool? occlusionEnabled = config.AmbientOcclusionEnabled;
        int num6 = occlusionEnabled.HasValue ? (occlusionEnabled.GetValueOrDefault() ? 1 : 0) : (defaults.RenderSettings.AmbientOcclusionEnabled ? 1 : 0);
        local6.AmbientOcclusionEnabled = num6 != 0;
        ref MyRenderSettings1 local7 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        MyTextureQuality? nullable2 = config.TextureQuality;
        int num7 = nullable2.HasValue ? (int) nullable2.GetValueOrDefault() : (int) defaults.RenderSettings.TextureQuality;
        local7.TextureQuality = (MyTextureQuality) num7;
        ref MyRenderSettings1 local8 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        nullable2 = config.VoxelTextureQuality;
        int num8 = nullable2.HasValue ? (int) nullable2.GetValueOrDefault() : (int) defaults.RenderSettings.VoxelTextureQuality;
        local8.VoxelTextureQuality = (MyTextureQuality) num8;
        ref MyRenderSettings1 local9 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        MyTextureAnisoFiltering? anisotropicFiltering = config.AnisotropicFiltering;
        int num9 = anisotropicFiltering.HasValue ? (int) anisotropicFiltering.GetValueOrDefault() : (int) defaults.RenderSettings.AnisotropicFiltering;
        local9.AnisotropicFiltering = (MyTextureAnisoFiltering) num9;
        ref MyRenderSettings1 local10 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        MyRenderQualityEnum? nullable3 = config.ModelQuality;
        int num10 = nullable3.HasValue ? (int) nullable3.GetValueOrDefault() : (int) defaults.RenderSettings.ModelQuality;
        local10.ModelQuality = (MyRenderQualityEnum) num10;
        ref MyRenderSettings1 local11 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        nullable3 = config.VoxelQuality;
        int num11 = nullable3.HasValue ? (int) nullable3.GetValueOrDefault() : (int) defaults.RenderSettings.VoxelQuality;
        local11.VoxelQuality = (MyRenderQualityEnum) num11;
        graphicsSettings.PerformanceSettings.RenderSettings.HqDepth = true;
        ref MyRenderSettings1 local12 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        nullable3 = config.ShaderQuality;
        int num12 = nullable3.HasValue ? (int) nullable3.GetValueOrDefault() : (int) defaults.RenderSettings.ShadowGPUQuality;
        local12.ShadowGPUQuality = (MyRenderQualityEnum) num12;
        ref MyRenderSettings1 local13 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        nullable3 = config.ShaderQuality;
        int num13 = nullable3.HasValue ? (int) nullable3.GetValueOrDefault() : (int) defaults.RenderSettings.VoxelShaderQuality;
        local13.VoxelShaderQuality = (MyRenderQualityEnum) num13;
        ref MyRenderSettings1 local14 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        nullable3 = config.ShaderQuality;
        int num14 = nullable3.HasValue ? (int) nullable3.GetValueOrDefault() : (int) defaults.RenderSettings.AlphaMaskedShaderQuality;
        local14.AlphaMaskedShaderQuality = (MyRenderQualityEnum) num14;
        ref MyRenderSettings1 local15 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        nullable3 = config.ShaderQuality;
        int num15 = nullable3.HasValue ? (int) nullable3.GetValueOrDefault() : (int) defaults.RenderSettings.AtmosphereShaderQuality;
        local15.AtmosphereShaderQuality = (MyRenderQualityEnum) num15;
        ref MyRenderSettings1 local16 = ref graphicsSettings.PerformanceSettings.RenderSettings;
        nullable3 = config.ShaderQuality;
        int num16 = nullable3.HasValue ? (int) nullable3.GetValueOrDefault() : (int) defaults.RenderSettings.ParticleQuality;
        local16.ParticleQuality = (MyRenderQualityEnum) num16;
      }
      return graphicsSettings;
    }

    public static MyRenderDeviceSettings? Initialize()
    {
      MyConfig config = MySandboxGame.Config;
      MyRenderProxy.RequestVideoAdapters();
      MyVideoSettingsManager.RunningGraphicsRenderer = config.GraphicsRenderer;
      int? screenWidth = config.ScreenWidth;
      int? screenHeight = config.ScreenHeight;
      int? nullable = new int?(config.VideoAdapter);
      if (!nullable.HasValue || !screenWidth.HasValue || !screenHeight.HasValue)
        return MyPerGameSettings.DefaultRenderDeviceSettings;
      MyRenderDeviceSettings renderDeviceSettings = new MyRenderDeviceSettings()
      {
        AdapterOrdinal = nullable.Value,
        NewAdapterOrdinal = nullable.Value,
        BackBufferHeight = screenHeight.Value,
        BackBufferWidth = screenWidth.Value,
        RefreshRate = config.RefreshRate,
        VSync = config.VerticalSync,
        WindowMode = config.WindowMode,
        InitParallel = MyVRage.Platform.Render.UseParallelRenderInit
      };
      if (MyPerGameSettings.DefaultRenderDeviceSettings.HasValue)
      {
        renderDeviceSettings.UseStereoRendering = MyPerGameSettings.DefaultRenderDeviceSettings.Value.UseStereoRendering;
        renderDeviceSettings.SettingsMandatory = MyPerGameSettings.DefaultRenderDeviceSettings.Value.SettingsMandatory;
      }
      return new MyRenderDeviceSettings?(renderDeviceSettings);
    }

    public static MyVideoSettingsManager.ChangeResult Apply(
      MyRenderDeviceSettings settings)
    {
      MySandboxGame.Log.WriteLine("MyVideoModeManager.Apply(MyRenderDeviceSettings)");
      using (MySandboxGame.Log.IndentUsing())
      {
        MySandboxGame.Log.WriteLine("VideoAdapter: " + (object) settings.AdapterOrdinal);
        MySandboxGame.Log.WriteLine("Width: " + (object) settings.BackBufferWidth);
        MySandboxGame.Log.WriteLine("Height: " + (object) settings.BackBufferHeight);
        MySandboxGame.Log.WriteLine("RefreshRate: " + (object) settings.RefreshRate);
        MySandboxGame.Log.WriteLine("WindowMode: " + (settings.WindowMode == MyWindowModeEnum.Fullscreen ? "Fullscreen" : (settings.WindowMode == MyWindowModeEnum.Window ? "Window" : "Fullscreen window")));
        MySandboxGame.Log.WriteLine("VerticalSync: " + (object) settings.VSync);
        if (settings.Equals(ref MyVideoSettingsManager.m_currentDeviceSettings) && settings.NewAdapterOrdinal == settings.AdapterOrdinal)
          return MyVideoSettingsManager.ChangeResult.NothingChanged;
        if (!MyVideoSettingsManager.IsSupportedDisplayMode(settings.AdapterOrdinal, settings.BackBufferWidth, settings.BackBufferHeight, settings.WindowMode))
          return MyVideoSettingsManager.ChangeResult.Failed;
        MyVideoSettingsManager.m_currentDeviceSettings = settings;
        MyVideoSettingsManager.m_currentDeviceSettings.VSync = settings.VSync;
        MySandboxGame.Static.SwitchSettings(MyVideoSettingsManager.m_currentDeviceSettings);
        double num = (double) MyVideoSettingsManager.m_currentDeviceSettings.BackBufferWidth / (double) MyVideoSettingsManager.m_currentDeviceSettings.BackBufferHeight;
        MyVideoSettingsManager.m_currentDeviceIsTripleHead = MyVideoSettingsManager.GetAspectRatio(MyVideoSettingsManager.GetClosestAspectRatio((float) num)).IsTripleHead;
        float minRadians;
        float maxRadians;
        MyVideoSettingsManager.GetFovBounds((float) num, out minRadians, out maxRadians);
        MyVideoSettingsManager.SetFov(MathHelper.Clamp(MyVideoSettingsManager.m_currentGraphicsSettings.FieldOfView, minRadians, maxRadians));
        MyVideoSettingsManager.SetPostProcessingEnabled(MyVideoSettingsManager.m_currentGraphicsSettings.PostProcessingEnabled);
      }
      return MyVideoSettingsManager.ChangeResult.Success;
    }

    private static void SetEnableDamageEffects(bool enableDamageEffects)
    {
      MyVideoSettingsManager.m_currentGraphicsSettings.PerformanceSettings.EnableDamageEffects = enableDamageEffects;
      MySandboxGame.Static.EnableDamageEffects = enableDamageEffects;
    }

    private static void SetHardwareCursor(bool useHardwareCursor)
    {
      MySandboxGame.Static.SetMouseVisible(MyVideoSettingsManager.IsHardwareCursorUsed());
      MyGuiSandbox.SetMouseCursorVisibility(MyVideoSettingsManager.IsHardwareCursorUsed(), false);
    }

    public static MyVideoSettingsManager.ChangeResult Apply(
      MyGraphicsSettings settings)
    {
      MySandboxGame.Log.WriteLine("MyVideoModeManager.Apply(MyGraphicsSettings1)");
      using (MySandboxGame.Log.IndentUsing())
      {
        MySandboxGame.Log.WriteLine("Flares Intensity: " + (object) settings.FlaresIntensity);
        MySandboxGame.Log.WriteLine("Field of view: " + (object) settings.FieldOfView);
        MySandboxGame.Log.WriteLine("PostProcessingEnabled: " + settings.PostProcessingEnabled.ToString());
        MySandboxGame.Log.WriteLine("Render.GrassDensityFactor: " + (object) settings.PerformanceSettings.RenderSettings.GrassDensityFactor);
        MySandboxGame.Log.WriteLine("Render.GrassDrawDistance: " + (object) settings.PerformanceSettings.RenderSettings.GrassDrawDistance);
        MySandboxGame.Log.WriteLine("Render.DistanceFade: " + (object) settings.PerformanceSettings.RenderSettings.DistanceFade);
        MySandboxGame.Log.WriteLine("Render.AntialiasingMode: " + (object) settings.PerformanceSettings.RenderSettings.AntialiasingMode);
        MySandboxGame.Log.WriteLine("Render.ShadowQuality: " + (object) settings.PerformanceSettings.RenderSettings.ShadowQuality);
        MySandboxGame.Log.WriteLine("Render.ShadowGPUQuality: " + (object) settings.PerformanceSettings.RenderSettings.ShadowGPUQuality);
        MySandboxGame.Log.WriteLine("Render.AmbientOcclusionEnabled: " + settings.PerformanceSettings.RenderSettings.AmbientOcclusionEnabled.ToString());
        MySandboxGame.Log.WriteLine("Render.TextureQuality: " + (object) settings.PerformanceSettings.RenderSettings.TextureQuality);
        MySandboxGame.Log.WriteLine("Render.VoxelTextureQuality: " + (object) settings.PerformanceSettings.RenderSettings.VoxelTextureQuality);
        MySandboxGame.Log.WriteLine("Render.AnisotropicFiltering: " + (object) settings.PerformanceSettings.RenderSettings.AnisotropicFiltering);
        MySandboxGame.Log.WriteLine("Render.VoxelShaderQuality: " + (object) settings.PerformanceSettings.RenderSettings.VoxelShaderQuality);
        MySandboxGame.Log.WriteLine("Render.AlphaMaskedShaderQuality: " + (object) settings.PerformanceSettings.RenderSettings.AlphaMaskedShaderQuality);
        MySandboxGame.Log.WriteLine("Render.AtmosphereShaderQuality: " + (object) settings.PerformanceSettings.RenderSettings.AtmosphereShaderQuality);
        MySandboxGame.Log.WriteLine("Render.ParticleQuality: " + (object) settings.PerformanceSettings.RenderSettings.ParticleQuality);
        if (MyVideoSettingsManager.m_currentGraphicsSettings.Equals(ref settings))
          return MyVideoSettingsManager.ChangeResult.NothingChanged;
        MyVideoSettingsManager.SetEnableDamageEffects(settings.PerformanceSettings.EnableDamageEffects);
        MyVideoSettingsManager.SetFov(settings.FieldOfView);
        MyVideoSettingsManager.SetPostProcessingEnabled(settings.PostProcessingEnabled);
        if ((double) MyRenderProxy.Settings.FlaresIntensity != (double) settings.FlaresIntensity)
        {
          MyRenderProxy.Settings.FlaresIntensity = settings.FlaresIntensity;
          MyRenderProxy.SetSettingsDirty();
        }
        if (!MyVideoSettingsManager.m_currentGraphicsSettings.PerformanceSettings.RenderSettings.Equals(ref settings.PerformanceSettings.RenderSettings))
          MyRenderProxy.SwitchRenderSettings(settings.PerformanceSettings.RenderSettings);
        if (MyVideoSettingsManager.m_currentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality != settings.PerformanceSettings.RenderSettings.VoxelQuality)
          MyRenderComponentVoxelMap.SetLodQuality(settings.PerformanceSettings.RenderSettings.VoxelQuality);
        MyVideoSettingsManager.m_currentGraphicsSettings = settings;
        MySector.Lodding.SelectQuality(settings.PerformanceSettings.RenderSettings.ModelQuality);
        MyVideoSettingsManager.OnSettingsChanged.InvokeIfNotNull();
      }
      return MyVideoSettingsManager.ChangeResult.Success;
    }

    private static void SetFov(float fov)
    {
      if ((double) MyVideoSettingsManager.m_currentGraphicsSettings.FieldOfView == (double) fov)
        return;
      MyVideoSettingsManager.m_currentGraphicsSettings.FieldOfView = fov;
      if (MySector.MainCamera == null)
        return;
      MySector.MainCamera.FieldOfView = fov;
      if (MySector.MainCamera.Zoom == null)
        return;
      MySector.MainCamera.Zoom.Update(0.01666667f);
    }

    private static void SetPostProcessingEnabled(bool enable)
    {
      if (MyVideoSettingsManager.m_currentGraphicsSettings.PostProcessingEnabled == enable)
        return;
      MyVideoSettingsManager.m_currentGraphicsSettings.PostProcessingEnabled = enable;
    }

    public static MyVideoSettingsManager.ChangeResult ApplyVideoSettings(
      MyRenderDeviceSettings deviceSettings,
      MyGraphicsSettings graphicsSettings)
    {
      MyVideoSettingsManager.ChangeResult changeResult1 = MyVideoSettingsManager.Apply(deviceSettings);
      if (changeResult1 == MyVideoSettingsManager.ChangeResult.Failed)
        return changeResult1;
      MyVideoSettingsManager.ChangeResult changeResult2 = MyVideoSettingsManager.Apply(graphicsSettings);
      return changeResult1 != MyVideoSettingsManager.ChangeResult.Success ? changeResult2 : changeResult1;
    }

    private static bool IsSupportedDisplayMode(
      int videoAdapter,
      int width,
      int height,
      MyWindowModeEnum windowMode)
    {
      bool flag = false;
      if (windowMode == MyWindowModeEnum.Fullscreen)
      {
        foreach (MyDisplayMode supportedDisplayMode in MyVideoSettingsManager.m_adapters[videoAdapter].SupportedDisplayModes)
        {
          if (supportedDisplayMode.Width == width && supportedDisplayMode.Height == height)
            flag = true;
        }
      }
      else
        flag = true;
      int maxTextureSize = MyVideoSettingsManager.m_adapters[videoAdapter].MaxTextureSize;
      if (width > maxTextureSize || height > maxTextureSize)
      {
        MySandboxGame.Log.WriteLine(string.Format("VideoMode {0}x{1} requires texture size which is not supported by this HW (this HW supports max {2})", (object) width, (object) height, (object) maxTextureSize));
        flag = false;
      }
      return flag;
    }

    public static void LogApplicationInformation()
    {
      MySandboxGame.Log.WriteLine("MyVideoModeManager.LogApplicationInformation - START");
      MySandboxGame.Log.IncreaseIndent();
      try
      {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        MySandboxGame.Log.WriteLine("Assembly.GetName: " + executingAssembly.GetName().ToString());
        MySandboxGame.Log.WriteLine("Assembly.FullName: " + executingAssembly.FullName);
        MySandboxGame.Log.WriteLine("Assembly.Location: " + executingAssembly.Location);
        MySandboxGame.Log.WriteLine("Assembly.ImageRuntimeVersion: " + executingAssembly.ImageRuntimeVersion);
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine("Error occured during enumerating application information. Application will still continue. Detail description: " + ex.ToString());
      }
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MyVideoModeManager.LogApplicationInformation - END");
    }

    public static bool IsTripleHead() => MyVideoSettingsManager.m_currentDeviceIsTripleHead;

    public static bool IsTripleHead(Vector2I screenSize) => MyVideoSettingsManager.GetAspectRatio(MyVideoSettingsManager.GetClosestAspectRatio((float) screenSize.X / (float) screenSize.Y)).IsTripleHead;

    public static bool IsHardwareCursorUsed()
    {
      if (MyExternalAppBase.Static != null)
        return false;
      OperatingSystem osVersion = Environment.OSVersion;
      return (osVersion.Platform != PlatformID.Win32NT || osVersion.Version.Major != 6 || osVersion.Version.Minor != 0) && (osVersion.Platform != PlatformID.Win32NT || osVersion.Version.Major != 5 || osVersion.Version.Minor != 1);
    }

    public static bool IsCurrentAdapterNvidia() => MyVideoSettingsManager.m_adapters.Length > MyVideoSettingsManager.m_currentDeviceSettings.AdapterOrdinal && MyVideoSettingsManager.m_currentDeviceSettings.AdapterOrdinal >= 0 && MyVideoSettingsManager.m_adapters[MyVideoSettingsManager.m_currentDeviceSettings.AdapterOrdinal].VendorId == VendorIds.Nvidia;

    public static MyAspectRatio GetAspectRatio(MyAspectRatioEnum aspectRatioEnum) => MyVideoSettingsManager.m_aspectRatios[(int) aspectRatioEnum];

    public static MyAspectRatio GetRecommendedAspectRatio(int adapterIndex) => MyVideoSettingsManager.m_recommendedAspectRatio[adapterIndex];

    public static MyAspectRatioEnum GetClosestAspectRatio(float aspectRatio)
    {
      MyAspectRatioEnum myAspectRatioEnum = MyAspectRatioEnum.Normal_4_3;
      float num1 = float.MaxValue;
      for (int index = 0; index < MyVideoSettingsManager.m_aspectRatios.Length; ++index)
      {
        float num2 = Math.Abs(aspectRatio - MyVideoSettingsManager.m_aspectRatios[index].AspectRatioNumber);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          myAspectRatioEnum = MyVideoSettingsManager.m_aspectRatios[index].AspectRatioEnum;
        }
      }
      return myAspectRatioEnum;
    }

    public static void GetFovBounds(out float minRadians, out float maxRadians) => MyVideoSettingsManager.GetFovBounds((float) MyVideoSettingsManager.m_currentDeviceSettings.BackBufferWidth / (float) MyVideoSettingsManager.m_currentDeviceSettings.BackBufferHeight, out minRadians, out maxRadians);

    public static void GetFovBounds(float aspectRatio, out float minRadians, out float maxRadians)
    {
      minRadians = MyConstants.FIELD_OF_VIEW_CONFIG_MIN;
      if ((double) aspectRatio >= 4.0)
        maxRadians = MyConstants.FIELD_OF_VIEW_CONFIG_MAX_TRIPLE_HEAD;
      else if ((double) aspectRatio >= 8.0 / 3.0)
        maxRadians = MyConstants.FIELD_OF_VIEW_CONFIG_MAX_DUAL_HEAD;
      else
        maxRadians = MyConstants.FIELD_OF_VIEW_CONFIG_MAX;
    }

    internal static void OnVideoAdaptersResponse(MyRenderMessageVideoAdaptersResponse message)
    {
      MyRenderProxy.Log.WriteLine("MyVideoSettingsManager.OnVideoAdaptersResponse");
      using (MyRenderProxy.Log.IndentUsing())
      {
        MyVideoSettingsManager.m_adapters = message.Adapters;
        int index = -1;
        MyAdapterInfo adapter1;
        adapter1.Priority = 1000;
        try
        {
          index = MySandboxGame.Static.GameRenderComponent.RenderThread.CurrentAdapter;
          adapter1 = MyVideoSettingsManager.m_adapters[index];
          MyVideoSettingsManager.GpuUnderMinimum = !adapter1.Has512MBRam;
        }
        catch
        {
        }
        MyVideoSettingsManager.m_recommendedAspectRatio = new Dictionary<int, MyAspectRatio>();
        if (MyVideoSettingsManager.m_adapters.Length == 0)
          MyRenderProxy.Log.WriteLine("ERROR: Adapters count is 0!");
        for (int key = 0; key < MyVideoSettingsManager.m_adapters.Length; ++key)
        {
          MyAdapterInfo adapter2 = MyVideoSettingsManager.m_adapters[key];
          MyRenderProxy.Log.WriteLine(string.Format("Adapter {0}", (object) adapter2));
          using (MyRenderProxy.Log.IndentUsing())
          {
            float aspectRatio = (float) adapter2.DesktopBounds.Width / (float) adapter2.DesktopBounds.Height;
            MyVideoSettingsManager.m_recommendedAspectRatio.Add(key, MyVideoSettingsManager.GetAspectRatio(MyVideoSettingsManager.GetClosestAspectRatio(aspectRatio)));
            if (adapter2.SupportedDisplayModes.Length == 0)
              MyRenderProxy.Log.WriteLine(string.Format("WARNING: Adapter {0} count of supported display modes is 0!", (object) key));
            int maxTextureSize = adapter2.MaxTextureSize;
            foreach (MyDisplayMode supportedDisplayMode in adapter2.SupportedDisplayModes)
            {
              MyRenderProxy.Log.WriteLine(supportedDisplayMode.ToString());
              if (supportedDisplayMode.Width > maxTextureSize || supportedDisplayMode.Height > maxTextureSize)
                MyRenderProxy.Log.WriteLine(string.Format("WARNING: Display mode {0} requires texture size which is not supported by this HW (this HW supports max {1})", (object) supportedDisplayMode, (object) maxTextureSize));
            }
          }
          MySandboxGame.ShowIsBetterGCAvailableNotification = ((MySandboxGame.ShowIsBetterGCAvailableNotification ? 1 : 0) | (index == key ? 0 : (adapter1.Priority < adapter2.Priority ? 1 : 0))) != 0;
        }
      }
    }

    internal static void OnCreatedDeviceSettings(MyRenderMessageCreatedDeviceSettings message)
    {
      MyVideoSettingsManager.m_currentDeviceSettings = message.Settings;
      MyVideoSettingsManager.m_currentDeviceSettings.NewAdapterOrdinal = MyVideoSettingsManager.m_currentDeviceSettings.AdapterOrdinal;
      MyVideoSettingsManager.m_currentDeviceIsTripleHead = MyVideoSettingsManager.GetAspectRatio(MyVideoSettingsManager.GetClosestAspectRatio((float) MyVideoSettingsManager.m_currentDeviceSettings.BackBufferWidth / (float) MyVideoSettingsManager.m_currentDeviceSettings.BackBufferHeight)).IsTripleHead;
    }

    public static void WriteCurrentSettingsToConfig()
    {
      MyConfig config = MySandboxGame.Config;
      config.VideoAdapter = MyVideoSettingsManager.m_currentDeviceSettings.NewAdapterOrdinal;
      config.ScreenWidth = new int?(MyVideoSettingsManager.m_currentDeviceSettings.BackBufferWidth);
      config.ScreenHeight = new int?(MyVideoSettingsManager.m_currentDeviceSettings.BackBufferHeight);
      config.RefreshRate = MyVideoSettingsManager.m_currentDeviceSettings.RefreshRate;
      config.WindowMode = MyVideoSettingsManager.m_currentDeviceSettings.WindowMode;
      config.VerticalSync = MyVideoSettingsManager.m_currentDeviceSettings.VSync;
      config.FieldOfView = MyVideoSettingsManager.m_currentGraphicsSettings.FieldOfView;
      config.PostProcessingEnabled = MyVideoSettingsManager.m_currentGraphicsSettings.PostProcessingEnabled;
      config.FlaresIntensity = MyVideoSettingsManager.m_currentGraphicsSettings.FlaresIntensity;
      config.GraphicsRenderer = MyVideoSettingsManager.m_currentGraphicsSettings.GraphicsRenderer;
      config.EnableDamageEffects = new bool?(MyVideoSettingsManager.m_currentGraphicsSettings.PerformanceSettings.EnableDamageEffects);
      MyRenderSettings1 renderSettings = MyVideoSettingsManager.m_currentGraphicsSettings.PerformanceSettings.RenderSettings;
      config.VegetationDrawDistance = new float?(renderSettings.DistanceFade);
      config.GrassDensityFactor = new float?(renderSettings.GrassDensityFactor);
      config.GrassDrawDistance = new float?(renderSettings.GrassDrawDistance);
      config.AntialiasingMode = new MyAntialiasingMode?(renderSettings.AntialiasingMode);
      config.ShadowQuality = new MyShadowsQuality?(renderSettings.ShadowQuality);
      config.AmbientOcclusionEnabled = new bool?(renderSettings.AmbientOcclusionEnabled);
      config.TextureQuality = new MyTextureQuality?(renderSettings.TextureQuality);
      config.VoxelTextureQuality = new MyTextureQuality?(renderSettings.VoxelTextureQuality);
      config.AnisotropicFiltering = new MyTextureAnisoFiltering?(renderSettings.AnisotropicFiltering);
      config.ModelQuality = new MyRenderQualityEnum?(renderSettings.ModelQuality);
      config.VoxelQuality = new MyRenderQualityEnum?(renderSettings.VoxelQuality);
      config.ShaderQuality = new MyRenderQualityEnum?(renderSettings.VoxelShaderQuality);
      config.LowMemSwitchToLow = MyConfig.LowMemSwitch.ARMED;
      MySandboxGame.Static.UpdateMouseCapture();
    }

    public static void SaveCurrentSettings()
    {
      MyVideoSettingsManager.WriteCurrentSettingsToConfig();
      MySandboxGame.Config.Save();
    }

    public enum ChangeResult
    {
      Success,
      NothingChanged,
      Failed,
    }
  }
}
