// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Linq.Expressions;
using System.Text;
using VRage;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  [MyDebugScreen("VRage", "System")]
  internal class MyGuiScreenDebugSystem : MyGuiScreenDebugBase
  {
    private MyGuiControlMultilineText m_havokStatsMultiline;
    private static StringBuilder m_buffer = new StringBuilder();
    private static string m_statsFromServer = string.Empty;

    public MyGuiScreenDebugSystem()
      : base()
      => this.RecreateControls(true);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_scale = 0.7f;
      this.AddCaption("System debug", new Vector4?(Color.Yellow.ToVector4()));
      this.AddShareFocusHint();
      this.m_currentPosition = -this.m_size.Value / 2f + new Vector2(0.02f, 0.1f);
      this.AddLabel("System", Color.Yellow.ToVector4(), 1.2f);
      this.AddCheckBox("Simulate slow update", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.SIMULATE_SLOW_UPDATE)));
      this.AddButton(new StringBuilder("Force GC"), new Action<MyGuiControlButton>(this.OnClick_ForceGC));
      this.AddCheckBox("Pause physics", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.PAUSE_PHYSICS)));
      this.AddCheckBox("Performance logging", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.ENABLE_PERFORMANCELOGGING)));
      this.AddButton(new StringBuilder("Step physics"), (Action<MyGuiControlButton>) (button => MyFakes.STEP_PHYSICS = true));
      this.AddSlider("Simulation speed", 1f / 1000f, 3f, (object) null, MemberHelper.GetMember<float>((Expression<Func<float>>) (() => MyFakes.SIMULATION_SPEED)));
      this.AddSlider("Statistics Logging Frequency [s]", (float) MyGeneralStats.Static.LogInterval.Seconds, 0.0f, 120f, (Action<MyGuiControlSlider>) (slider => MyGeneralStats.Static.LogInterval = MyTimeSpan.FromSeconds((double) slider.Value)));
      if (MySession.Static != null && MySession.Static.Settings != null)
        this.AddCheckBox("Enable save", (object) MySession.Static.Settings, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MySession.Static.Settings.EnableSaving)));
      this.AddCheckBox("Enable scenario settings edit", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyGuiScreenLoadSandbox.ENABLE_SCENARIO_EDIT)));
      this.AddCheckBox("Optimize grid update", (object) null, MemberHelper.GetMember<bool>((Expression<Func<bool>>) (() => MyFakes.OPTIMIZE_GRID_UPDATES)));
      this.AddCheckBox("Enable Parallel Entity Update", (Func<bool>) (() => !MyParallelEntityUpdateOrchestrator.ForceSerialUpdate), (Action<bool>) (x => MyParallelEntityUpdateOrchestrator.ForceSerialUpdate = !x));
      this.AddCheckBox("UGC Test Environment", MyPlatformGameSettings.UGC_TEST_ENVIRONMENT, (Action<MyGuiControlCheckbox>) (c =>
      {
        MyPlatformGameSettings.UGC_TEST_ENVIRONMENT = c.IsChecked;
        MyGameService.WorkshopService.SetTestEnvironment(MyPlatformGameSettings.UGC_TEST_ENVIRONMENT);
      }));
      this.AddCheckBox("GameService tracing", false, (Action<MyGuiControlCheckbox>) (c => MyGameService.Trace(c.IsChecked)));
      this.AddButton(new StringBuilder("Clear achievements and stats"), (Action<MyGuiControlButton>) (button =>
      {
        MyGameService.ResetAllStats(true);
        MyGameService.StoreStats();
      }));
      this.AddLabel("Simplified simulation", Color.Yellow.ToVector4(), 1f);
      MyGuiScreenDebugSystem.SimulationQualityOptions selectedItem = MyGuiScreenDebugSystem.SimulationQualityOptions.UseWorldSetting;
      if (MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE.HasValue)
        selectedItem = MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE.Value ? MyGuiScreenDebugSystem.SimulationQualityOptions.Simplified : MyGuiScreenDebugSystem.SimulationQualityOptions.Normal;
      this.AddCombo<MyGuiScreenDebugSystem.SimulationQualityOptions>(selectedItem, (Action<MyGuiScreenDebugSystem.SimulationQualityOptions>) (x =>
      {
        switch (x)
        {
          case MyGuiScreenDebugSystem.SimulationQualityOptions.UseWorldSetting:
            MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE = new bool?();
            break;
          case MyGuiScreenDebugSystem.SimulationQualityOptions.Normal:
            MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE = new bool?(false);
            break;
          case MyGuiScreenDebugSystem.SimulationQualityOptions.Simplified:
            MyPlatformGameSettings.SIMPLIFIED_SIMULATION_OVERRIDE = new bool?(true);
            break;
        }
      }));
      this.AddLabel("VST simulation quality", Color.Yellow.ToVector4(), 1f);
      SimulationQuality? simulationQualityOverride = MyPlatformGameSettings.VST_SIMULATION_QUALITY_OVERRIDE;
      this.AddCombo<MyGuiScreenDebugSystem.VSTSimulationQualityOptions>((MyGuiScreenDebugSystem.VSTSimulationQualityOptions) ((simulationQualityOverride.HasValue ? new int?((int) simulationQualityOverride.GetValueOrDefault()) : new int?()) ?? 3), (Action<MyGuiScreenDebugSystem.VSTSimulationQualityOptions>) (x =>
      {
        if (x == MyGuiScreenDebugSystem.VSTSimulationQualityOptions.PlatformDefault)
          MyPlatformGameSettings.VST_SIMULATION_QUALITY_OVERRIDE = new SimulationQuality?();
        else
          MyPlatformGameSettings.VST_SIMULATION_QUALITY_OVERRIDE = new SimulationQuality?((SimulationQuality) x);
      }));
      this.m_currentPosition.Y += 0.01f;
      this.m_havokStatsMultiline = this.AddMultilineText(textScale: 0.8f);
    }

    public override bool Draw()
    {
      this.m_havokStatsMultiline.Clear();
      this.m_havokStatsMultiline.AppendText(MyGuiScreenDebugSystem.GetHavokMemoryStats());
      return base.Draw();
    }

    private static string GetHavokMemoryStats()
    {
      if (Sync.IsServer || MySession.Static == null)
      {
        MyGuiScreenDebugSystem.m_buffer.Append("Out of mem: ").Append(HkBaseSystem.IsOutOfMemory).AppendLine();
        HkBaseSystem.GetMemoryStatistics(MyGuiScreenDebugSystem.m_buffer);
        string str = MyGuiScreenDebugSystem.m_buffer.ToString();
        MyGuiScreenDebugSystem.m_buffer.Clear();
        return str;
      }
      if (MySession.Static.GameplayFrameCounter % 100 == 0)
        MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyGuiScreenDebugSystem.HavokMemoryStatsRequest)));
      return MyGuiScreenDebugSystem.m_statsFromServer;
    }

    [Event(null, 163)]
    [Reliable]
    [Server]
    private static void HavokMemoryStatsRequest()
    {
      if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
      else
        MyMultiplayer.RaiseStaticEvent<string>((Func<IMyEventOwner, Action<string>>) (x => new Action<string>(MyGuiScreenDebugSystem.HavokMemoryStatsReply)), MyGuiScreenDebugSystem.GetHavokMemoryStats(), MyEventContext.Current.Sender);
    }

    [Event(null, 175)]
    [Reliable]
    [Client]
    private static void HavokMemoryStatsReply(string stats) => MyGuiScreenDebugSystem.m_statsFromServer = stats;

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugSystem);

    private void OnClick_ForceGC(MyGuiControlButton button) => GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);

    private enum SimulationQualityOptions
    {
      UseWorldSetting,
      Normal,
      Simplified,
    }

    private enum VSTSimulationQualityOptions
    {
      Normal,
      Low,
      VeryLow,
      PlatformDefault,
    }

    protected sealed class HavokMemoryStatsRequest\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyGuiScreenDebugSystem.HavokMemoryStatsRequest();
      }
    }

    protected sealed class HavokMemoryStatsReply\u003C\u003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in string stats,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenDebugSystem.HavokMemoryStatsReply(stats);
      }
    }
  }
}
