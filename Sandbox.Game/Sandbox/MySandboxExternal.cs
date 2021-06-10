// Decompiled with JetBrains decompiler
// Type: Sandbox.MySandboxExternal
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.AppCode;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game;
using System;
using System.Threading;
using VRage;
using VRageMath;
using VRageRender;
using VRageRender.ExternalApp;
using VRageRender.Messages;

namespace Sandbox
{
  public class MySandboxExternal : MySandboxGame
  {
    public readonly IExternalApp ExternalApp;
    private MyRenderDeviceSettings m_currentSettings;

    public MySandboxExternal(
      IExternalApp externalApp,
      string[] commandlineArgs,
      IntPtr windowHandle)
      : base(commandlineArgs, windowHandle)
    {
      this.ExternalApp = externalApp;
    }

    public override void SwitchSettings(MyRenderDeviceSettings settings)
    {
      this.m_currentSettings = settings;
      this.m_currentSettings.WindowMode = MyWindowModeEnum.Window;
      base.SwitchSettings(this.m_currentSettings);
    }

    protected override void StartRenderComponent(
      MyRenderDeviceSettings? settings,
      IntPtr windowHandle)
    {
      this.DrawThread = Thread.CurrentThread;
      MySandboxGame.InitMultithreading();
      MyRenderProxy.EnableAppEventsCall = false;
      MyVRage.Platform.Windows.CreateToolWindow(windowHandle);
      IVRageWindow window = MyVRage.Platform.Windows.Window;
      MySandboxGame.m_windowCreatedEvent.Set();
      MyVRage.Platform.Windows.Window.OnExit += new Action(MySandboxGame.ExitThreadSafe);
      if (!settings.HasValue)
        settings = new MyRenderDeviceSettings?(new MyRenderDeviceSettings(0, MyWindowModeEnum.Window, window.ClientSize.X, window.ClientSize.Y, 0, 0, false, false));
      this.GameRenderComponent.StartSync(this.m_gameTimer, window, settings, MyPerGameSettings.MaxFrameRate);
      this.GameRenderComponent.RenderThread.SizeChanged += new SizeChangedHandler(((MySandboxGame) this).RenderThread_SizeChanged);
      this.GameRenderComponent.RenderThread.BeforeDraw += new Action(((MySandboxGame) this).RenderThread_BeforeDraw);
      MyViewport viewport = new MyViewport(0.0f, 0.0f, (float) window.ClientSize.X, (float) window.ClientSize.Y);
      this.RenderThread_SizeChanged(window.ClientSize.X, window.ClientSize.Y, viewport);
    }

    protected override void Update()
    {
      base.Update();
      this.ExternalApp.Update();
    }

    protected override void CheckGraphicsCard(
      MyRenderMessageVideoAdaptersResponse msgVideoAdapters)
    {
      base.CheckGraphicsCard(msgVideoAdapters);
      MyPerformanceSettings defaults = new MyPerformanceSettings()
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
      MyVideoSettingsManager.UpdateRenderSettingsFromConfig(ref defaults);
    }

    protected override void AfterDraw()
    {
      base.AfterDraw();
      if (this.GameRenderComponent.RenderThread == null)
        return;
      Vector2I clientSize = MyVRage.Platform.Windows.Window.ClientSize;
      if ((this.m_currentSettings.BackBufferWidth != clientSize.X || this.m_currentSettings.BackBufferHeight != clientSize.Y) && (clientSize.X > 0 && clientSize.Y > 0))
        this.SwitchSettings(new MyRenderDeviceSettings()
        {
          AdapterOrdinal = this.m_currentSettings.AdapterOrdinal,
          RefreshRate = this.m_currentSettings.RefreshRate,
          VSync = this.m_currentSettings.VSync,
          WindowMode = this.m_currentSettings.WindowMode,
          BackBufferWidth = clientSize.X,
          BackBufferHeight = clientSize.Y
        });
      this.GameRenderComponent.RenderThread.TickSync();
    }
  }
}
