// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenHudSpace
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Definitions.GUI;
using Sandbox.Engine;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Game.GUI;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenHudSpace : MyGuiScreenHudBase
  {
    public static MyGuiScreenHudSpace Static;
    private const float ALTITUDE_CHANGE_THRESHOLD = 500f;
    public const int PING_THRESHOLD_MILLISECONDS = 250;
    public bool EnableDrawAsync = true;
    private MyGuiControlToolbar m_toolbarControl;
    private MyGuiControlDPad m_DPadControl;
    private MyGuiControlContextHelp m_contextHelp;
    private MyGuiControlBlockInfo m_blockInfo;
    private MyGuiControlLabel m_rotatingWheelLabel;
    private MyGuiControlRotatingWheel m_rotatingWheelControl;
    private MyGuiControlMultilineText m_cameraInfoMultilineControl;
    private MyGuiControlQuestlog m_questlogControl;
    private MyGuiControlLabel m_buildModeLabel;
    private MyGuiControlLabel m_blocksLeft;
    private MyHudCameraOverlay m_overlay;
    private MyHudControlChat m_chatControl;
    private MyHudMarkerRender m_markerRender;
    private int m_oreHudMarkerStyle;
    private int m_gpsHudMarkerStyle;
    private int m_buttonPanelHudMarkerStyle;
    private MyHudEntityParams m_tmpHudEntityParams;
    private MyTuple<Vector3D, MyEntityOreDeposit>[] m_nearestOreDeposits;
    private float[] m_nearestDistanceSquared;
    private MyHudControlGravityIndicator m_gravityIndicator;
    private MyObjectBuilder_GuiTexture m_visorOverlayTexture;
    private readonly List<MyStatControls> m_statControls = new List<MyStatControls>();
    private HashSet<MyStringId> m_suppressedWarnings = new HashSet<MyStringId>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    private bool m_hiddenToolbar;
    public float m_gravityHudWidth;
    private float m_altitude;
    private List<MyStringId> m_warningNotifications = new List<MyStringId>();
    private readonly byte m_warningFrameCount = 200;
    private byte m_currentFrameCount;
    private readonly MyHudWeaponHitIndicator m_hitIndicator = new MyHudWeaponHitIndicator();
    private Task? m_hudTask;
    private MyRenderMessageDrawCommands m_drawAsyncMessages;
    private List<MyDamageIndicator> m_damageIndicators = new List<MyDamageIndicator>();
    private readonly TimeSpan DAMAGE_INDICATOR_VISIBILITY_TIME = TimeSpan.FromSeconds(1.5);

    public MyGuiScreenHudSpace()
    {
      MyGuiScreenHudSpace.Static = this;
      this.RecreateControls(true);
      this.m_markerRender = new MyHudMarkerRender((MyGuiScreenHudBase) this);
      this.m_oreHudMarkerStyle = this.m_markerRender.AllocateMarkerStyle("White", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_neutral, Color.White);
      this.m_gpsHudMarkerStyle = this.m_markerRender.AllocateMarkerStyle("DarkBlue", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_me, MyHudConstants.GPS_COLOR);
      this.m_buttonPanelHudMarkerStyle = this.m_markerRender.AllocateMarkerStyle("DarkBlue", MyHudTexturesEnum.DirectionIndicator, MyHudTexturesEnum.Target_me, MyHudConstants.GPS_COLOR);
      this.m_tmpHudEntityParams = new MyHudEntityParams()
      {
        Text = new StringBuilder(),
        FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ALL
      };
      if (this.m_contextHelp != null)
        return;
      this.m_contextHelp = new MyGuiControlContextHelp(this.GetDefaultStyle());
      this.m_contextHelp.BlockInfo = MyHud.BlockInfo;
    }

    private MyGuiControlBlockInfo.MyControlBlockInfoStyle GetDefaultStyle() => new MyGuiControlBlockInfo.MyControlBlockInfoStyle()
    {
      BackgroundColormask = new Vector4(0.1529412f, 0.2039216f, 0.2313726f, 0.9f),
      BlockNameLabelFont = "Blue",
      EnableBlockTypeLabel = true,
      ComponentsLabelText = MySpaceTexts.HudBlockInfo_Components,
      ComponentsLabelFont = "Blue",
      InstalledRequiredLabelText = MySpaceTexts.HudBlockInfo_Installed_Required,
      InstalledRequiredLabelFont = "Blue",
      RequiredLabelText = MyCommonTexts.HudBlockInfo_Required,
      IntegrityLabelFont = "White",
      IntegrityBackgroundColor = new Vector4(0.2666667f, 0.3019608f, 0.3372549f, 0.9f),
      IntegrityForegroundColor = new Vector4(0.4509804f, 0.2705882f, 0.3137255f, 1f),
      IntegrityForegroundColorOverCritical = new Vector4(0.4784314f, 0.5490196f, 0.6039216f, 1f),
      LeftColumnBackgroundColor = new Vector4(0.1803922f, 0.2980392f, 0.3686275f, 1f),
      TitleBackgroundColor = new Vector4(0.2078431f, 0.2666667f, 0.2980392f, 0.9f),
      ComponentLineMissingFont = "Red",
      ComponentLineAllMountedFont = "White",
      ComponentLineAllInstalledFont = "Blue",
      ComponentLineDefaultFont = "Blue",
      ComponentLineDefaultColor = new Vector4(0.6f, 0.6f, 0.6f, 1f),
      ShowAvailableComponents = false,
      EnableBlockTypePanel = false
    };

    public override void UnloadData()
    {
      this.m_DPadControl?.Dispose();
      base.UnloadData();
      if (this.m_DPadControl != null)
        this.m_DPadControl.UnregisterEvents();
      MyGuiScreenHudSpace.Static = (MyGuiScreenHudSpace) null;
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      if (MyHud.Static == null)
        return;
      this.Elements.Add((MyGuiControlBase) new MyGuiControlBloodOverlay());
      this.InitHudStatControls();
      MyHudDefinition hudDefinition = MyHud.HudDefinition;
      this.m_gravityIndicator = new MyHudControlGravityIndicator(hudDefinition.GravityIndicator);
      if (hudDefinition.VisorOverlayTexture.HasValue)
        this.m_visorOverlayTexture = MyGuiTextures.Static.GetTexture(hudDefinition.VisorOverlayTexture.Value);
      this.m_toolbarControl = new MyGuiControlToolbar(hudDefinition.Toolbar, false);
      this.m_toolbarControl.Position = hudDefinition.Toolbar.CenterPosition;
      this.m_toolbarControl.OriginAlign = hudDefinition.Toolbar.OriginAlign;
      this.m_toolbarControl.IsActiveControl = false;
      this.Elements.Add((MyGuiControlBase) this.m_toolbarControl);
      MyObjectBuilder_DPadControlVisualStyle style = hudDefinition.DPad == null ? MyObjectBuilder_DPadControlVisualStyle.DefaultStyle() : hudDefinition.DPad;
      this.m_DPadControl = new MyGuiControlDPad(style);
      this.m_DPadControl.Position = style.CenterPosition;
      this.m_DPadControl.OriginAlign = style.OriginAlign;
      this.m_DPadControl.IsActiveControl = false;
      this.Elements.Add((MyGuiControlBase) this.m_DPadControl);
      this.m_textScale = 0.8f * MyGuiManager.LanguageTextScale;
      MyGuiControlBlockInfo.MyControlBlockInfoStyle defaultStyle = this.GetDefaultStyle();
      this.m_contextHelp = new MyGuiControlContextHelp(defaultStyle);
      this.m_contextHelp.IsActiveControl = false;
      this.Controls.Add((MyGuiControlBase) this.m_contextHelp);
      this.m_blockInfo = new MyGuiControlBlockInfo(defaultStyle);
      this.m_blockInfo.IsActiveControl = false;
      MyGuiControlBlockInfo.ShowComponentProgress = true;
      MyGuiControlBlockInfo.CriticalIntegrityColor = (Vector4) new Color(115, 69, 80);
      MyGuiControlBlockInfo.OwnershipIntegrityColor = (Vector4) new Color(56, 67, 147);
      this.Controls.Add((MyGuiControlBase) this.m_blockInfo);
      this.m_questlogControl = new MyGuiControlQuestlog(new Vector2(20f, 20f));
      this.m_questlogControl.IsActiveControl = false;
      this.m_questlogControl.RecreateControls();
      this.Controls.Add((MyGuiControlBase) this.m_questlogControl);
      this.m_chatControl = new MyHudControlChat(MyHud.Chat, new Vector2?(Vector2.Zero), new Vector2?(new Vector2(0.339f, 0.28f)), textScale: 0.7f);
      this.Elements.Add((MyGuiControlBase) this.m_chatControl);
      this.m_cameraInfoMultilineControl = new MyGuiControlMultilineText(new Vector2?(Vector2.Zero), new Vector2?(new Vector2(0.4f, 0.25f)), font: "White", textScale: 0.7f, textAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, drawScrollbarV: false, drawScrollbarH: false, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
      this.m_cameraInfoMultilineControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      this.Elements.Add((MyGuiControlBase) this.m_cameraInfoMultilineControl);
      this.m_rotatingWheelControl = new MyGuiControlRotatingWheel(new Vector2?(new Vector2(0.5f, 0.8f)));
      this.Controls.Add((MyGuiControlBase) this.m_rotatingWheelControl);
      this.Controls.Add((MyGuiControlBase) (this.m_rotatingWheelLabel = new MyGuiControlLabel()));
      Vector2 hudPos = new Vector2(0.5f, 0.02f);
      hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
      this.m_buildModeLabel = new MyGuiControlLabel(new Vector2?(hudPos), text: MyTexts.GetString(MyCommonTexts.Hud_BuildMode), font: "White", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.Controls.Add((MyGuiControlBase) this.m_buildModeLabel);
      this.m_blocksLeft = new MyGuiControlLabel(new Vector2?(new Vector2(0.238f, 0.89f)), text: MyHud.BlocksLeft.GetStringBuilder().ToString(), font: "White", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
      this.Controls.Add((MyGuiControlBase) this.m_blocksLeft);
      this.m_overlay = new MyHudCameraOverlay();
      this.Controls.Add((MyGuiControlBase) this.m_overlay);
      this.RegisterAlphaMultiplier(VisualStyleCategory.Background, MySandboxGame.Config.HUDBkOpacity);
      MyHud.ReloadTexts();
      this.Controls.Add((MyGuiControlBase) this.m_hitIndicator.GuiControlImage);
    }

    private void InitHudStatControls()
    {
      MyHudDefinition hudDefinition = MyHud.HudDefinition;
      this.m_statControls.Clear();
      if (hudDefinition.StatControls == null)
        return;
      foreach (MyObjectBuilder_StatControls statControl in hudDefinition.StatControls)
      {
        float uiScale = statControl.ApplyHudScale ? MyGuiManager.GetSafeScreenScale() * MyHud.HudElementsScaleMultiplier : MyGuiManager.GetSafeScreenScale();
        MyStatControls myStatControls = new MyStatControls(statControl, uiScale);
        Vector2 coordScreen = statControl.Position * (Vector2) MySandboxGame.ScreenSize;
        myStatControls.Position = MyUtils.AlignCoord(coordScreen, (Vector2) MySandboxGame.ScreenSize, statControl.OriginAlign);
        this.m_statControls.Add(myStatControls);
      }
    }

    private void RefreshRotatingWheel()
    {
      this.m_rotatingWheelLabel.Visible = MyHud.RotatingWheelVisible;
      this.m_rotatingWheelControl.Visible = MyHud.RotatingWheelVisible;
      if (!MyHud.RotatingWheelVisible || this.m_rotatingWheelLabel.TextToDraw == MyHud.RotatingWheelText)
        return;
      this.m_rotatingWheelLabel.Position = this.m_rotatingWheelControl.Position + new Vector2(0.0f, 0.05f);
      this.m_rotatingWheelLabel.TextToDraw = MyHud.RotatingWheelText;
      this.m_rotatingWheelLabel.PositionX -= this.m_rotatingWheelLabel.GetTextSize().X / 2f;
    }

    public void AddDamageIndicator(float damage, MyHitInfo hitInfo, Vector3 origin)
    {
      Vector2 projectedPoint2D;
      if (!MyGuiScreenHudSpace.TryComputeScreenPoint((Vector3D) origin, out projectedPoint2D))
        return;
      this.m_damageIndicators.Add(new MyDamageIndicator()
      {
        IndicatorCreationTime = MySession.Static.ElapsedGameTime,
        Damage = damage,
        ScreenPosition = projectedPoint2D
      });
    }

    public void AddPlayerMarker(
      MyEntity target,
      MyRelationsBetweenPlayers relation,
      bool isAlwaysVisible)
    {
      this.m_markerRender.AddPlayerIndicator(target, relation, isAlwaysVisible);
    }

    public void RegisterAlphaMultiplier(VisualStyleCategory category, float multiplier) => this.m_statControls.ForEach((Action<MyStatControls>) (c => c.RegisterAlphaMultiplier(category, multiplier)));

    public override void PrepareDraw()
    {
      if ((double) this.m_transitionAlpha < 1.0 || !MyHud.IsVisible)
        return;
      if (MySession.Static.ControlledEntity != null && MySession.Static.CameraController != null)
        MySession.Static.ControlledEntity.DrawHud(MySession.Static.CameraController, MySession.Static.LocalPlayerId);
      bool flag = MyHud.BlockInfo.Components.Count > 0;
      IMyHudStat stat = MyHud.Stats.GetStat(MyStringHash.GetOrCompute("hud_mode"));
      this.m_contextHelp.Visible = MyHud.BlockInfo.Visible && !MyHud.MinimalHud && !MyHud.CutsceneHud;
      if ((double) stat.CurrentValue == 1.0)
        this.m_contextHelp.Visible &= !string.IsNullOrEmpty(MyHud.BlockInfo.ContextHelp);
      if ((double) stat.CurrentValue == 2.0)
        this.m_contextHelp.Visible &= flag;
      this.m_contextHelp.BlockInfo = MyHud.BlockInfo.Visible ? MyHud.BlockInfo : (MyHudBlockInfo) null;
      Vector2 hudPos = new Vector2(0.99f, 0.985f);
      if (MySession.Static.ControlledEntity is MyShipController)
        hudPos.Y = 0.65f;
      hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
      if (MyVideoSettingsManager.IsTripleHead())
        ++hudPos.X;
      if ((double) stat.CurrentValue == 2.0)
      {
        this.m_contextHelp.Position = new Vector2(hudPos.X, 0.38f);
        this.m_contextHelp.ShowJustTitle = true;
      }
      else
      {
        if (!MyHud.ShipInfo.Visible)
          this.m_contextHelp.Position = new Vector2(hudPos.X, 0.28f);
        else
          this.m_contextHelp.Position = new Vector2(hudPos.X, 0.1f);
        this.m_contextHelp.ShowJustTitle = false;
      }
      this.m_contextHelp.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_contextHelp.ShowBuildInfo = flag;
      this.m_blockInfo.Visible = ((!MyHud.BlockInfo.Visible || MyHud.MinimalHud ? 0 : (!MyHud.CutsceneHud ? 1 : 0)) & (flag ? 1 : 0)) != 0;
      this.m_blockInfo.BlockInfo = this.m_blockInfo.Visible ? MyHud.BlockInfo : (MyHudBlockInfo) null;
      this.m_blockInfo.Position = this.m_contextHelp.Position + new Vector2(0.0f, this.m_contextHelp.Size.Y + 3f / 500f);
      this.m_blockInfo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      this.m_questlogControl.Visible = MyHud.Questlog.Visible && !MyHud.IsHudMinimal && !MyHud.MinimalHud && !MyHud.CutsceneHud;
      this.m_rotatingWheelControl.Visible = MyHud.RotatingWheelVisible && !MyHud.MinimalHud && !MyHud.CutsceneHud;
      this.m_rotatingWheelLabel.Visible = this.m_rotatingWheelControl.Visible;
      this.m_chatControl.Visible = !MyHud.MinimalHud || this.m_chatControl.HasFocus || MyHud.CutsceneHud;
    }

    public override bool Draw()
    {
      if ((double) this.m_transitionAlpha < 1.0 || !MyHud.IsVisible)
        return false;
      if (MyInput.Static.IsNewKeyPressed(MyKeys.J) && MyFakes.ENABLE_OBJECTIVE_LINE)
        MyHud.ObjectiveLine.AdvanceObjective();
      this.m_toolbarControl.Visible = this.m_DPadControl.Visible = !this.m_hiddenToolbar && !MyHud.MinimalHud && !MyHud.CutsceneHud;
      if (!this.EnableDrawAsync)
      {
        this.AsyncUpdate(false);
        this.DrawAsync();
      }
      else
      {
        if (this.m_drawAsyncMessages == null)
        {
          this.AsyncUpdate(false);
          this.DrawAsync();
        }
        ref Task? local = ref this.m_hudTask;
        if (local.HasValue)
          local.GetValueOrDefault().WaitOrExecute();
        this.m_hudTask = new Task?();
      }
      MyRenderProxy.ExecuteCommands(this.m_drawAsyncMessages);
      if (this.EnableDrawAsync)
        this.AsyncUpdate();
      if (!base.Draw())
        return false;
      Vector2 hudPos1 = new Vector2(0.014f, 0.81f);
      Vector2 hudPos2 = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos1);
      this.m_chatControl.Position = hudPos2 + new Vector2(1f / 500f, -0.07f);
      this.m_chatControl.TextScale = 0.7f;
      hudPos2 = new Vector2(0.03f, 0.1f);
      hudPos2 = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos2);
      this.m_cameraInfoMultilineControl.Position = hudPos2;
      this.m_cameraInfoMultilineControl.TextScale = 0.9f;
      if (!MyHud.MinimalHud && !MyHud.CutsceneHud)
      {
        bool flag = false;
        if (MySession.Static.ControlledEntity is MyShipController controlledEntity)
          flag = (double) MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(controlledEntity.PositionComp.GetPosition()) != 0.0;
        if (flag && !MySession.Static.IsCameraUserAnySpectator())
          this.DrawArtificialHorizonAndAltitude();
      }
      if (!MyHud.MinimalHud && !MyHud.CutsceneHud)
      {
        this.m_buildModeLabel.Visible = MyHud.IsBuildMode;
        if (MyHud.BlocksLeft.Visible)
        {
          StringBuilder stringBuilder = MyHud.BlocksLeft.GetStringBuilder();
          if (!this.m_blocksLeft.Text.EqualsStrFast(stringBuilder))
            this.m_blocksLeft.Text = stringBuilder.ToString();
          this.m_blocksLeft.Visible = true;
        }
        else
          this.m_blocksLeft.Visible = false;
        if (MyHud.ObjectiveLine.Visible && MyFakes.ENABLE_OBJECTIVE_LINE)
          this.DrawObjectiveLine(MyHud.ObjectiveLine);
      }
      else
      {
        this.m_buildModeLabel.Visible = false;
        this.m_blocksLeft.Visible = false;
      }
      this.m_blockInfo.BlockInfo = (MyHudBlockInfo) null;
      MyGuiScreenHudBase.HandleSelectedObjectHighlight(MyHud.SelectedObjectHighlight, new MyHudObjectHighlightStyleData?(new MyHudObjectHighlightStyleData()
      {
        AtlasTexture = this.m_atlas,
        TextureCoord = this.GetTextureCoord(MyHudTexturesEnum.corner)
      }));
      if (MyPetaInputComponent.DRAW_WARNINGS && this.m_warningNotifications.Count != 0)
        this.DrawPerformanceWarning();
      this.DrawCameraInfo(MyHud.CameraInfo);
      if (MyHud.VoiceChat.Visible)
        this.DrawVoiceChat(MyHud.VoiceChat);
      this.m_hitIndicator.Update();
      return true;
    }

    private void UpdatePerfWarnings()
    {
      List<string> suppressedWarnings = MySession.Static.Settings.SuppressedWarnings;
      if (suppressedWarnings != null && this.m_suppressedWarnings.Count == 0)
      {
        foreach (string str in suppressedWarnings)
          this.m_suppressedWarnings.Add(MyStringId.GetOrCompute(str));
      }
      if (!MySandboxGame.Config.EnablePerformanceWarnings)
        return;
      if (MySession.Static.IsRunningExperimental)
        AddWarning(MyCommonTexts.PerformanceWarningHeading_ExperimentalMode, true);
      if (MyMultiplayer.Static?.ReplicationLayer is MyReplicationClient replicationLayer && replicationLayer.ReplicationRange.HasValue)
        AddWarning(MyCommonTexts.PerformanceWarningHeading_ReducedReplicationRange, true);
      if (!MyGameService.Service.GetInstallStatus(out int _))
        AddWarning(MyCommonTexts.PerformanceWarningHeading_InstallInProgress, true);
      if (MyUnsafeGridsSessionComponent.UnsafeGrids.Count > 0)
        AddWarning(MyCommonTexts.PerformanceWarningHeading_UnsafeGrids, true);
      foreach (KeyValuePair<MySimpleProfiler.MySimpleProfilingBlock, MySimpleProfiler.PerformanceWarning> currentWarning in MySimpleProfiler.CurrentWarnings)
      {
        if (!this.m_warningNotifications.Contains(MyCommonTexts.PerformanceWarningHeading))
        {
          if (currentWarning.Value.Time < 120)
          {
            AddWarning(MyCommonTexts.PerformanceWarningHeading, false);
            break;
          }
        }
        else
          break;
      }
      if (MyGeneralStats.Static.LowNetworkQuality || !MySession.Static.MultiplayerDirect || !MySession.Static.MultiplayerAlive && !MySession.Static.ServerSaving || !Sync.IsServer && MySession.Static.MultiplayerPing.Milliseconds > 250.0)
        AddWarning(MyCommonTexts.PerformanceWarningHeading_Connection, true);
      if (!MySession.Static.HighSimulationQualityNotification)
        AddWarning(MyCommonTexts.PerformanceWarningHeading_SimSpeed, true);
      if (MySession.Static.ServerSaving)
        AddWarning(MyCommonTexts.PerformanceWarningHeading_Saving, true);
      if (MyPlatformGameSettings.PUBLIC_BETA_MP_TEST)
        AddWarning(MyCommonTexts.PerformanceWarningHeading_ExperimentalBetaBuild, true);
      if (!MyGameService.IsOnline)
        AddWarning(MyCommonTexts.PerformanceWarningHeading_SteamOffline, true);
      if (MySession.Static.GetComponent<MySessionComponentDLC>().UsedUnownedDLCs.Count > 0)
        AddWarning(MyCommonTexts.PerformanceWarningHeading_PaidContent, true);
      if (MySession.Static.MultiplayerLastMsg <= 1.0)
        return;
      AddWarning(MyCommonTexts.PerformanceWarningHeading_Connection, true);

      void AddWarning(MyStringId id, bool check)
      {
        if (this.m_suppressedWarnings.Contains(id) || check && this.m_warningNotifications.Contains(id))
          return;
        this.m_warningNotifications.Add(id);
      }
    }

    private void DrawAsync()
    {
      MyRenderProxy.BeginRecordingDeferredMessages();
      if (!MyHud.MinimalHud && !MyHud.CutsceneHud)
      {
        foreach (MyStatControls statControl in this.m_statControls)
          statControl.Draw(this.m_transitionAlpha, this.m_backgroundTransition);
        this.DrawTexts();
      }
      if (!MyHud.IsHudMinimal && !MyHud.MinimalHud && !MyHud.CutsceneHud || MyPetaInputComponent.SHOW_HUD_ALWAYS)
        this.m_markerRender.Draw();
      if (!MyHud.IsHudMinimal)
        MyHud.Notifications.Draw();
      this.m_drawAsyncMessages = MyRenderProxy.FinishRecordingDeferredMessages();
    }

    public override bool Update(bool hasFocus)
    {
      this.RefreshRotatingWheel();
      return base.Update(hasFocus);
    }

    private void AsyncUpdate(bool startTask = true)
    {
      this.m_markerRender.Update();
      if (!MyHud.IsHudMinimal && !MyHud.MinimalHud && !MyHud.CutsceneHud || MyPetaInputComponent.SHOW_HUD_ALWAYS)
      {
        if (MyHud.SinkGroupInfo.Visible && MyFakes.LEGACY_HUD)
          this.DrawPowerGroupInfo(MyHud.SinkGroupInfo);
        if (MyHud.LocationMarkers.Visible)
          this.m_markerRender.DrawLocationMarkers(MyHud.LocationMarkers);
        if (MyHud.GpsMarkers.Visible && MyFakes.ENABLE_GPS)
          this.DrawGpsMarkers(MyHud.GpsMarkers);
        if (MyHud.ButtonPanelMarkers.Visible)
          this.DrawButtonPanelMarkers(MyHud.ButtonPanelMarkers);
        if (MyHud.OreMarkers.Visible)
          this.DrawOreMarkers(MyHud.OreMarkers);
        if (MyHud.LargeTurretTargets.Visible)
          this.DrawLargeTurretTargets(MyHud.LargeTurretTargets);
        this.DrawWorldBorderIndicator(MyHud.WorldBorderChecker);
        if (MyHud.HackingMarkers.Visible)
          this.DrawHackingMarkers(MyHud.HackingMarkers);
        this.m_gravityIndicator.Draw(this.m_transitionAlpha);
      }
      if (!MyHud.MinimalHud && !MyHud.CutsceneHud)
        this.UpdatePerfWarnings();
      if (!MyHud.IsHudMinimal)
        MyHud.Notifications.Update();
      this.DrawDamageIndicators(this.m_damageIndicators);
      if (!startTask || (double) this.m_transitionAlpha < 1.0 || !MyHud.IsVisible)
        return;
      ref Task? local = ref this.m_hudTask;
      if (local.HasValue)
        local.GetValueOrDefault().WaitOrExecute();
      this.m_hudTask = new Task?(Parallel.Start(new Action(this.DrawAsync), Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.GUI, "HUD"), (Action) null, WorkPriority.VeryHigh));
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenHudSpace);

    private static Vector2 GetRealPositionOnCenterScreen(Vector2 value)
    {
      Vector2 vector2 = !MyGuiManager.FullscreenHudEnabled ? MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(value) : MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate_FULLSCREEN(value);
      if (MyVideoSettingsManager.IsTripleHead())
        ++vector2.X;
      return vector2;
    }

    public void SetToolbarVisible(bool visible)
    {
      if (this.m_toolbarControl == null)
        return;
      this.m_toolbarControl.Visible = visible;
      this.m_hiddenToolbar = !visible;
    }

    private void DrawVoiceChat(MyHudVoiceChat voiceChat)
    {
      MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      MyGuiPaddedTexture textureVoiceChat = MyGuiConstants.TEXTURE_VOICE_CHAT;
      Vector2 hudPos = new Vector2(0.01f, 0.99f);
      Vector2 normalizedGuiPosition = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
      MyGuiManager.DrawSpriteBatch(textureVoiceChat.Texture, normalizedGuiPosition, textureVoiceChat.SizeGui, Color.White, drawAlign);
    }

    private void DrawPowerGroupInfo(MyHudSinkGroupInfo info)
    {
      Rectangle fullscreenRectangle = MyGuiManager.GetSafeFullscreenRectangle();
      float num = (float) (-0.25 / ((double) fullscreenRectangle.Width / (double) fullscreenRectangle.Height));
      Vector2 hudPos1 = new Vector2(0.985f, 0.65f);
      Vector2 hudPos2 = new Vector2(hudPos1.X + num, hudPos1.Y);
      Vector2 normalizedGuiPosition1 = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos1);
      Vector2 normalizedGuiPosition2 = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos2);
      info.Data.DrawBottomUp(normalizedGuiPosition2, normalizedGuiPosition1, this.m_textScale);
    }

    private float FindDistanceToNearestPlanetSeaLevel(
      BoundingBoxD worldBB,
      out MyPlanet closestPlanet)
    {
      closestPlanet = MyGamePruningStructure.GetClosestPlanet(ref worldBB);
      double num = double.MaxValue;
      if (closestPlanet != null)
        num = (worldBB.Center - closestPlanet.PositionComp.GetPosition()).Length() - (double) closestPlanet.AverageRadius;
      return (float) num;
    }

    private void DrawArtificialHorizonAndAltitude()
    {
      if (!(MySession.Static.ControlledEntity is MyCubeBlock controlledEntity) || controlledEntity.CubeGrid.Physics == null)
        return;
      Vector3D centerOfMassWorld1 = controlledEntity.CubeGrid.Physics.CenterOfMassWorld;
      Vector3D centerOfMassWorld2 = controlledEntity.GetTopMostParent((Type) null).Physics.CenterOfMassWorld;
      if (controlledEntity is MyShipController myShipController && !myShipController.HorizonIndicatorEnabled)
        return;
      MyPlanet closestPlanet;
      double nearestPlanetSeaLevel = (double) this.FindDistanceToNearestPlanetSeaLevel(controlledEntity.PositionComp.WorldAABB, out closestPlanet);
      if (closestPlanet == null)
        return;
      double num1 = Vector3D.Distance(closestPlanet.GetClosestSurfacePointGlobal(ref centerOfMassWorld1), centerOfMassWorld1);
      string font = "Blue";
      MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      float number = (float) num1;
      if ((double) Math.Abs(number - this.m_altitude) > 500.0 && controlledEntity.CubeGrid.GridSystems.GasSystem != null)
      {
        controlledEntity.CubeGrid.GridSystems.GasSystem.OnAltitudeChanged();
        this.m_altitude = number;
      }
      StringBuilder stringBuilder = new StringBuilder().AppendDecimal(number, 0).Append(" m");
      float num2 = 0.03f;
      int num3 = MyGuiManager.GetFullscreenRectangle().Width / MyGuiManager.GetSafeFullscreenRectangle().Width;
      int num4 = MyGuiManager.GetFullscreenRectangle().Height / MyGuiManager.GetSafeFullscreenRectangle().Height;
      Vector2 normalizedCoord = new Vector2(MyHud.Crosshair.Position.X * (float) num3 / MyGuiManager.GetHudSize().X, MyHud.Crosshair.Position.Y * (float) num4 / MyGuiManager.GetHudSize().Y + num2);
      if (MyVideoSettingsManager.IsTripleHead())
        --normalizedCoord.X;
      MyGuiManager.DrawString(font, stringBuilder.ToString(), normalizedCoord, this.m_textScale, drawAlign: drawAlign, useFullClientArea: true);
      Vector3 v = -closestPlanet.Components.Get<MyGravityProviderComponent>().GetWorldGravity(centerOfMassWorld2);
      double num5 = (double) v.Normalize();
      double num6 = (double) v.Dot((Vector3) controlledEntity.WorldMatrix.Forward);
      float num7 = 0.4f;
      Vector2 vector2 = MyHud.Crosshair.Position / MyGuiManager.GetHudSize() * new Vector2((float) MyGuiManager.GetSafeFullscreenRectangle().Width, (float) MyGuiManager.GetSafeFullscreenRectangle().Height);
      MyGuiPaddedTexture hudGravityHorizon = MyGuiConstants.TEXTURE_HUD_GRAVITY_HORIZON;
      double num8 = MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.ThirdPersonSpectator ? (double) (0.35f * MySector.MainCamera.Viewport.Height) : (double) (0.45f * MySector.MainCamera.Viewport.Height);
      double num9 = num6 * num8;
      Vector2D vector2D = new Vector2D(controlledEntity.WorldMatrix.Right.Dot(v), controlledEntity.WorldMatrix.Up.Dot(v));
      float num10 = vector2D.LengthSquared() > 9.99999974737875E-06 ? (float) Math.Atan2(vector2D.Y, vector2D.X) : 0.0f;
      Vector2 size = hudGravityHorizon.SizePx * num7;
      RectangleF destination = new RectangleF(vector2 - size * 0.5f + new Vector2(0.0f, (float) num9), size);
      Rectangle? sourceRectangle = new Rectangle?();
      Vector2 rightVector = new Vector2((float) Math.Sin((double) num10), (float) Math.Cos((double) num10));
      Vector2 origin = vector2;
      MyRenderProxy.DrawSpriteExt(hudGravityHorizon.Texture, ref destination, sourceRectangle, Color.White, ref rightVector, ref origin, false, true);
    }

    internal void ActivateHitIndicator(MySession.MyHitIndicatorTarget hitTarget) => this.m_hitIndicator.Hit(hitTarget);

    private void DrawObjectiveLine(MyHudObjectiveLine objective)
    {
      MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      Color aliceBlue = Color.AliceBlue;
      Vector2 hudPos1 = new Vector2(0.45f, 0.01f);
      Vector2 vector2 = new Vector2(0.0f, 0.02f);
      Vector2 normalizedGuiPosition1 = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos1);
      string title = objective.Title;
      Vector2 normalizedCoord = normalizedGuiPosition1;
      MyGuiDrawAlignEnum guiDrawAlignEnum = drawAlign;
      Color? colorMask = new Color?(aliceBlue);
      int num = (int) guiDrawAlignEnum;
      MyGuiManager.DrawString("Debug", title, normalizedCoord, 1f, colorMask, (MyGuiDrawAlignEnum) num);
      Vector2 hudPos2 = hudPos1 + vector2;
      Vector2 normalizedGuiPosition2 = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos2);
      MyGuiManager.DrawString("Debug", "- " + objective.CurrentObjective, normalizedGuiPosition2, 1f, drawAlign: drawAlign);
    }

    private void DrawGpsMarkers(MyHudGpsMarkers gpsMarkers)
    {
      this.m_tmpHudEntityParams.FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ALL;
      MySession.Static.Gpss.updateForHud();
      foreach (MyGps markerEntity in gpsMarkers.MarkerEntities)
        this.m_markerRender.AddGPS(markerEntity);
    }

    private void DrawDamageIndicators(List<MyDamageIndicator> damageIndicators)
    {
      List<MyDamageIndicator> myDamageIndicatorList = new List<MyDamageIndicator>();
      Vector2I vector2I1 = new Vector2I(128, 128);
      Vector2I vector2I2 = new Vector2I(22, 38);
      Rectangle rectangle = new Rectangle(vector2I1.X - vector2I2.X, (vector2I1.Y - vector2I2.Y) / 2, vector2I2.X, vector2I2.Y);
      Vector2I vector2I3 = new Vector2I(MyGuiManager.GetSafeFullscreenRectangle().Width / 2, MyGuiManager.GetSafeFullscreenRectangle().Height / 2);
      int num = vector2I3.Y / 3;
      foreach (MyDamageIndicator damageIndicator in damageIndicators)
      {
        TimeSpan timeSpan = MySession.Static.ElapsedGameTime - damageIndicator.IndicatorCreationTime;
        if (timeSpan <= this.DAMAGE_INDICATOR_VISIBILITY_TIME)
        {
          float a = (float) (1.0 - this.EaseInBack(timeSpan.TotalMilliseconds / this.DAMAGE_INDICATOR_VISIBILITY_TIME.TotalMilliseconds)) * 0.8f;
          Vector2 screenPosition = damageIndicator.ScreenPosition;
          float rotation = MyMath.ArcTanAngle(screenPosition.X, screenPosition.Y);
          RectangleF destination = new RectangleF((Vector2) vector2I3 + screenPosition * (float) num - (Vector2) (vector2I2 / 2), (Vector2) vector2I2);
          MyRenderProxy.DrawSprite("Textures\\GUI\\Indicators\\DamageIndicator03.png", ref destination, new Rectangle?(rectangle), new Color(Color.Red, a), rotation, false, false);
        }
        else
          myDamageIndicatorList.Add(damageIndicator);
      }
      foreach (MyDamageIndicator myDamageIndicator in myDamageIndicatorList)
        damageIndicators.Remove(myDamageIndicator);
    }

    private double EaseInBack(double x)
    {
      double num = 1.70158004760742;
      return (num + 1.0) * x * x * x - num * x * x;
    }

    public static bool TryComputeScreenPoint(Vector3D worldPosition, out Vector2 projectedPoint2D)
    {
      if (MySession.Static.LocalCharacter == null)
      {
        projectedPoint2D = Vector2.Zero;
        return false;
      }
      MatrixD invertedWorldMatrix = MySession.Static.LocalCharacter.GetSpineInvertedWorldMatrix();
      MatrixD.Invert(ref invertedWorldMatrix);
      MatrixD matrixD;
      if (!MySession.Static.LocalCharacter.IsInFirstPersonView)
      {
        matrixD = MatrixD.Invert(MySector.MainCamera.ViewMatrix);
        matrixD.Translation = invertedWorldMatrix.Translation;
      }
      else
      {
        matrixD = MySession.Static.LocalCharacter.WorldMatrix;
        matrixD.Translation = invertedWorldMatrix.Translation;
      }
      Matrix matrix1 = Matrix.Invert((Matrix) ref matrixD);
      MatrixD matrix2 = (MatrixD) ref matrix1;
      Vector3D vector3D = Vector3D.Transform(worldPosition, matrix2);
      double x = vector3D.X;
      double z = vector3D.Z;
      projectedPoint2D = new Vector2((float) x, (float) z);
      projectedPoint2D.Normalize();
      return true;
    }

    private void DrawButtonPanelMarkers(MyHudGpsMarkers buttonPanelMarkers)
    {
      foreach (MyGps markerEntity in buttonPanelMarkers.MarkerEntities)
        this.m_markerRender.AddButtonMarker(markerEntity.Coords, markerEntity.Name);
    }

    private void DrawOreMarkers(MyHudOreMarkers oreMarkers)
    {
      if (this.m_nearestOreDeposits == null || this.m_nearestOreDeposits.Length < MyDefinitionManager.Static.VoxelMaterialCount)
      {
        this.m_nearestOreDeposits = new MyTuple<Vector3D, MyEntityOreDeposit>[MyDefinitionManager.Static.VoxelMaterialCount];
        this.m_nearestDistanceSquared = new float[this.m_nearestOreDeposits.Length];
      }
      for (int index = 0; index < this.m_nearestOreDeposits.Length; ++index)
      {
        this.m_nearestOreDeposits[index] = new MyTuple<Vector3D, MyEntityOreDeposit>();
        this.m_nearestDistanceSquared[index] = float.MaxValue;
      }
      Vector3D vector3D = Vector3D.Zero;
      if (MySession.Static != null && MySession.Static.ControlledEntity != null)
        vector3D = (MySession.Static.ControlledEntity as MyEntity).WorldMatrix.Translation;
      foreach (MyEntityOreDeposit oreMarker in oreMarkers)
      {
        for (int index = 0; index < oreMarker.Materials.Count; ++index)
        {
          MyEntityOreDeposit.Data material1 = oreMarker.Materials[index];
          MyVoxelMaterialDefinition material2 = material1.Material;
          Vector3D oreWorldPosition;
          material1.ComputeWorldPosition(oreMarker.VoxelMap, out oreWorldPosition);
          float num1 = (float) (vector3D - oreWorldPosition).LengthSquared();
          float num2 = this.m_nearestDistanceSquared[(int) material2.Index];
          if ((double) num1 < (double) num2)
          {
            this.m_nearestOreDeposits[(int) material2.Index] = MyTuple.Create<Vector3D, MyEntityOreDeposit>(oreWorldPosition, oreMarker);
            this.m_nearestDistanceSquared[(int) material2.Index] = num1;
          }
        }
      }
      for (int index = 0; index < this.m_nearestOreDeposits.Length; ++index)
      {
        MyTuple<Vector3D, MyEntityOreDeposit> nearestOreDeposit = this.m_nearestOreDeposits[index];
        if (nearestOreDeposit.Item2 != null && nearestOreDeposit.Item2.VoxelMap != null && !nearestOreDeposit.Item2.VoxelMap.Closed)
        {
          MyVoxelMaterialDefinition materialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition((byte) index);
          this.m_markerRender.AddOre(nearestOreDeposit.Item1, oreMarkers.GetOreName(materialDefinition));
        }
      }
    }

    private void DrawCameraInfo(MyHudCameraInfo cameraInfo) => cameraInfo.Draw(this.m_cameraInfoMultilineControl);

    private void DrawLargeTurretTargets(MyHudLargeTurretTargets largeTurretTargets)
    {
      foreach (KeyValuePair<MyEntity, MyHudEntityParams> target in largeTurretTargets.Targets)
      {
        MyHudEntityParams myHudEntityParams = target.Value;
        if (myHudEntityParams.ShouldDraw == null || myHudEntityParams.ShouldDraw())
          this.m_markerRender.AddTarget(target.Key.PositionComp.WorldAABB.Center);
      }
    }

    private void DrawWorldBorderIndicator(MyHudWorldBorderChecker checker)
    {
      if (!checker.WorldCenterHintVisible)
        return;
      this.m_markerRender.AddPOI(Vector3D.Zero, MyHudWorldBorderChecker.HudEntityParams.Text, MyRelationsBetweenPlayerAndBlock.Enemies);
    }

    private void DrawHackingMarkers(MyHudHackingMarkers hackingMarkers)
    {
      try
      {
        hackingMarkers.UpdateMarkers();
        if (MySandboxGame.TotalTimeInMilliseconds % 200 > 100)
          return;
        foreach (KeyValuePair<long, MyHudEntityParams> markerEntity in hackingMarkers.MarkerEntities)
        {
          MyHudEntityParams myHudEntityParams = markerEntity.Value;
          if (myHudEntityParams.ShouldDraw == null || myHudEntityParams.ShouldDraw())
            this.m_markerRender.AddHacking(markerEntity.Value.Position, myHudEntityParams.Text);
        }
      }
      finally
      {
      }
    }

    private void DrawPerformanceWarning()
    {
      Vector2 normalizedCoord = MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, 4, 42) - new Vector2(MyGuiConstants.TEXTURE_HUD_BG_PERFORMANCE.SizeGui.X / 1.5f, 0.0f);
      MyGuiPaddedTexture hudBgPerformance = MyGuiConstants.TEXTURE_HUD_BG_PERFORMANCE;
      MyGuiManager.DrawSpriteBatch(hudBgPerformance.Texture, normalizedCoord, hudBgPerformance.SizeGui / 1.5f, Color.White, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      string str = MyTexts.GetString(this.m_warningNotifications[0]);
      if (this.m_warningNotifications[0] == MyCommonTexts.PerformanceWarningHeading_SteamOffline)
        str = string.Format(str, (object) MyGameService.Service.ServiceName);
      MyGuiManager.DrawString("White", str, normalizedCoord + new Vector2(0.09f, -11f / 1000f), 0.7f, drawAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      MyGuiManager.DrawString("White", MyInput.Static.IsJoystickLastUsed ? string.Format(MyTexts.GetString(MyCommonTexts.PerformanceWarningCombinationGamepad), (object) MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_BASE, MyControlsSpace.WARNING_SCREEN)) : string.Format(MyTexts.GetString(MyCommonTexts.PerformanceWarningCombination), (object) MyGuiSandbox.GetKeyName(MyControlsSpace.HELP_SCREEN)), normalizedCoord + new Vector2(0.09f, 0.018f), 0.6f, drawAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      MyGuiManager.DrawString("White", string.Format("({0})", (object) this.m_warningNotifications.Count), normalizedCoord + new Vector2(0.177f, -23f / 1000f), 0.55f, drawAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      if ((int) this.m_currentFrameCount < (int) this.m_warningFrameCount)
      {
        ++this.m_currentFrameCount;
      }
      else
      {
        this.m_currentFrameCount = (byte) 0;
        this.m_warningNotifications.RemoveAt(0);
      }
    }

    protected override void OnHide()
    {
      base.OnHide();
      if (!MyHud.VoiceChat.Visible)
        return;
      MyHud.VoiceChat.Hide();
    }
  }
}
