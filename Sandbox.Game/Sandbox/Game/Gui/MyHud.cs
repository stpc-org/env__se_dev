// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHud
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Definitions.GUI;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, Priority = 10)]
  public class MyHud : MySessionComponentBase
  {
    private static readonly MyStringHash m_defaultDefinitionId = MyStringHash.GetOrCompute("Default");
    public static readonly StringBuilder Empty = new StringBuilder();
    private static MyHud m_Static;
    private static int m_rotatingWheelVisibleCounter;
    private static bool m_buildMode = false;
    private static MyHudDefinition m_definition;
    public static MyHudScreenEffects ScreenEffects = new MyHudScreenEffects();
    public static MyHudVoiceChat VoiceChat = new MyHudVoiceChat();
    public static MyHudSelectedObject SelectedObjectHighlight = new MyHudSelectedObject();
    public static MyHudBlockInfo BlockInfo = new MyHudBlockInfo();
    public static MyHudGravityIndicator GravityIndicator = new MyHudGravityIndicator();
    public static MyHudOreMarkers OreMarkers = new MyHudOreMarkers();
    public static MyHudLargeTurretTargets LargeTurretTargets = new MyHudLargeTurretTargets();
    public static MyHudQuestlog Questlog = new MyHudQuestlog();
    public static MyHudLocationMarkers LocationMarkers = new MyHudLocationMarkers();
    public static MyHudNotifications Notifications;
    public static MyHudGpsMarkers GpsMarkers = new MyHudGpsMarkers();
    private static int m_hudState;
    private readonly MyHudCrosshair m_Crosshair = new MyHudCrosshair();
    private readonly MyHudShipInfo m_ShipInfo = new MyHudShipInfo();
    private readonly MyHudScenarioInfo m_ScenarioInfo = new MyHudScenarioInfo();
    private readonly MyHudSinkGroupInfo m_SinkGroupInfo = new MyHudSinkGroupInfo();
    private readonly MyHudGpsMarkers m_ButtonPanelMarkers = new MyHudGpsMarkers();
    private readonly MyHudChat m_Chat = new MyHudChat();
    private readonly MyHudWorldBorderChecker m_WorldBorderChecker = new MyHudWorldBorderChecker();
    private readonly MyHudHackingMarkers m_HackingMarkers = new MyHudHackingMarkers();
    private readonly MyHudCameraInfo m_CameraInfo = new MyHudCameraInfo();
    private readonly MyHudObjectiveLine m_ObjectiveLine = new MyHudObjectiveLine();
    private readonly MyHudChangedInventoryItems m_ChangedInventoryItems = new MyHudChangedInventoryItems();
    private readonly MyHudText m_BlocksLeft = new MyHudText();
    private MyHudStatManager m_Stats = new MyHudStatManager();

    public static MyHudCrosshair Crosshair => MyHud.Static?.m_Crosshair;

    public static MyHudShipInfo ShipInfo => MyHud.Static.m_ShipInfo;

    public static MyHudScenarioInfo ScenarioInfo => MyHud.Static.m_ScenarioInfo;

    public static MyHudSinkGroupInfo SinkGroupInfo => MyHud.Static.m_SinkGroupInfo;

    public static MyHudGpsMarkers ButtonPanelMarkers => MyHud.Static.m_ButtonPanelMarkers;

    public static MyHudChat Chat => MyHud.Static.m_Chat;

    public static MyHudWorldBorderChecker WorldBorderChecker => MyHud.Static.m_WorldBorderChecker;

    public static MyHudHackingMarkers HackingMarkers => MyHud.Static.m_HackingMarkers;

    public static MyHudCameraInfo CameraInfo => MyHud.Static.m_CameraInfo;

    public static MyHudObjectiveLine ObjectiveLine => MyHud.Static.m_ObjectiveLine;

    public static MyHudChangedInventoryItems ChangedInventoryItems => MyHud.Static.m_ChangedInventoryItems;

    public static MyHudText BlocksLeft => MyHud.Static.m_BlocksLeft;

    public static MyHudStatManager Stats => MyHud.Static.m_Stats;

    public static MyHud Static => MyHud.m_Static;

    public MyHud()
    {
      if (Sync.IsDedicated)
        this.UpdateOrder = MyUpdateOrder.NoUpdate;
      MyHud.m_Static = this;
    }

    public static MyHudDefinition HudDefinition
    {
      get
      {
        if (MyHud.m_definition == null)
          MyHud.m_definition = MyDefinitionManagerBase.Static.GetDefinition<MyHudDefinition>(MyHud.m_defaultDefinitionId);
        return MyHud.m_definition;
      }
    }

    public static float HudElementsScaleMultiplier
    {
      get
      {
        Vector2I? optimalScreenRatio = MyHud.m_definition.OptimalScreenRatio;
        double x = (double) optimalScreenRatio.Value.X;
        optimalScreenRatio = MyHud.m_definition.OptimalScreenRatio;
        double y = (double) optimalScreenRatio.Value.Y;
        float num = (float) (x / y);
        return MyMath.Clamp((float) MySandboxGame.ScreenSize.X / (float) MySandboxGame.ScreenSize.Y / num, 0.0f, 1f);
      }
    }

    public static bool RotatingWheelVisible => MyHud.m_rotatingWheelVisibleCounter > 0;

    public static StringBuilder RotatingWheelText { get; set; }

    public static int HudState
    {
      get => MyHud.m_hudState;
      set
      {
        if (MyHud.m_hudState == value)
          return;
        MyHud.m_hudState = value;
        MySandboxGame.Config.HudState = value;
      }
    }

    public static void SetHudState(int state)
    {
      MyHud.HudState = state % 3;
      MyHud.MinimalHud = MyHud.IsHudMinimal;
    }

    public static void ToggleGamepadHud()
    {
      if (MyHud.IsHudMinimal)
        MyHud.SetHudState(1);
      else
        MyHud.SetHudState(0);
    }

    public static bool IsHudMinimal => MyHud.m_hudState == 0;

    public static bool MinimalHud { get; set; }

    public static bool IsVisible { get; set; }

    public static bool CutsceneHud { get; set; }

    public static bool IsBuildMode
    {
      get => MyHud.m_buildMode;
      set => MyHud.m_buildMode = value;
    }

    public static void SetHudDefinition(string definition)
    {
      MyHudDefinition myHudDefinition = (MyHudDefinition) null;
      if (!string.IsNullOrEmpty(definition))
        myHudDefinition = MyDefinitionManager.Static.GetDefinition<MyHudDefinition>(MyStringHash.GetOrCompute(definition));
      if (myHudDefinition == null)
        myHudDefinition = MyDefinitionManager.Static.GetDefinition<MyHudDefinition>(MyStringHash.GetOrCompute("Default"));
      if (MyHud.HudDefinition == myHudDefinition)
        return;
      MyHud.m_definition = myHudDefinition;
      if (MyGuiScreenHudSpace.Static == null || MyHud.m_definition == null)
        return;
      MyGuiScreenHudSpace.Static.RecreateControls(false);
    }

    public static bool CheckShowPlayerNamesOnHud() => MySession.Static.ShowPlayerNamesOnHud;

    public static void ReloadTexts()
    {
      MyHud.Notifications.ReloadTexts();
      MyHud.ShipInfo.Reload();
      MyHud.SinkGroupInfo.Reload();
      MyHud.ScenarioInfo.Reload();
      MyHud.OreMarkers.Reload();
    }

    public static void PushRotatingWheelVisible() => ++MyHud.m_rotatingWheelVisibleCounter;

    public static void PopRotatingWheelVisible() => --MyHud.m_rotatingWheelVisibleCounter;

    internal static void HideAll()
    {
      MyHud.Crosshair.HideDefaultSprite();
      MyHud.ShipInfo.Hide();
      MyHud.BlockInfo.ClearDisplayers();
      MyHud.GravityIndicator.Hide();
      MyHud.SinkGroupInfo.Visible = false;
      MyHud.LargeTurretTargets.Visible = false;
    }

    public override void LoadData()
    {
      base.LoadData();
      MyHud.Notifications = new MyHudNotifications();
      this.m_Stats = new MyHudStatManager();
      MyHud.HudState = MySandboxGame.Config.HudState;
      this.m_Chat.RegisterChat(MyMultiplayer.Static);
    }

    public override void BeforeStart() => MyHud.Questlog.Init();

    public override void SaveData()
    {
      if (MyCampaignManager.Static == null || !MyCampaignManager.Static.IsCampaignRunning)
        return;
      MyHud.Questlog.Save();
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyHud.Notifications.Clear();
      MyHud.OreMarkers.Clear();
      MyHud.LocationMarkers.Clear();
      MyHud.GpsMarkers.Clear();
      this.m_HackingMarkers.Clear();
      this.m_ObjectiveLine.Clear();
      this.m_ChangedInventoryItems.Clear();
      MyHud.GravityIndicator.Clean();
      MyHud.SelectedObjectHighlight.Clean();
      MyGuiScreenToolbarConfigBase.Reset();
      this.m_Stats = (MyHudStatManager) null;
      this.m_Chat.UnregisterChat(MyMultiplayer.Static);
      MyHud.m_Static = (MyHud) null;
      MyHud.IsVisible = false;
    }

    public override void UpdateBeforeSimulation()
    {
      if (Sync.IsDedicated)
        return;
      MyHud.IsVisible = MySession.Static.LocalCharacter != null && !MySession.Static.LocalCharacter.IsDead && !MyScreenManager.IsScreenOfTypeOpen("MyGuiScreenMyGuiScreenMedicals");
      MyHud.Notifications.UpdateBeforeSimulation();
      this.m_Chat.Update();
      this.m_WorldBorderChecker.Update();
      MyHud.ScreenEffects.Update();
      this.m_Stats.Update();
      base.UpdateBeforeSimulation();
    }
  }
}
