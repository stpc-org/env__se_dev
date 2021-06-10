// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenPerformanceWarnings
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.GUI
{
  internal class MyGuiScreenPerformanceWarnings : MyGuiScreenBase
  {
    private MyGuiControlList m_warningsList;
    private MyGuiControlCheckbox m_showWarningsCheckBox;
    private MyGuiControlCheckbox m_showAllCheckBox;
    private MyGuiControlCheckbox m_showAllBlockLimitsCheckBox;
    private MyGuiControlButton m_okButton;
    private Dictionary<MyStringId, MyGuiScreenPerformanceWarnings.WarningLine> m_warningLines = new Dictionary<MyStringId, MyGuiScreenPerformanceWarnings.WarningLine>();
    internal MyGuiScreenPerformanceWarnings.WarningArea m_areaTitleGraphics = new MyGuiScreenPerformanceWarnings.WarningArea(MyCommonTexts.PerformanceWarningIssuesGraphics, MySessionComponentWarningSystem.Category.Graphics, true, false, false);
    internal MyGuiScreenPerformanceWarnings.WarningArea m_areaTitleBlocks = new MyGuiScreenPerformanceWarnings.WarningArea(MyCommonTexts.PerformanceWarningIssuesBlocks, MySessionComponentWarningSystem.Category.Blocks, true, false, false);
    internal MyGuiScreenPerformanceWarnings.WarningArea m_areaTitleOther = new MyGuiScreenPerformanceWarnings.WarningArea(MyCommonTexts.PerformanceWarningIssuesOther, MySessionComponentWarningSystem.Category.Other, false, false, false);
    internal MyGuiScreenPerformanceWarnings.WarningArea m_areaTitleUnsafeGrids = new MyGuiScreenPerformanceWarnings.WarningArea(MyCommonTexts.PerformanceWarningIssuesUnsafeGrids, MySessionComponentWarningSystem.Category.UnsafeGrids, false, true, false);
    internal MyGuiScreenPerformanceWarnings.WarningArea m_areaTitleBlockLimits = new MyGuiScreenPerformanceWarnings.WarningArea(MyCommonTexts.PerformanceWarningIssuesBlockBuildingLimits, MySessionComponentWarningSystem.Category.BlockLimits, false, true, false);
    internal MyGuiScreenPerformanceWarnings.WarningArea m_areaTitleServer = new MyGuiScreenPerformanceWarnings.WarningArea(MyCommonTexts.PerformanceWarningIssuesServer, MySessionComponentWarningSystem.Category.Server, false, false, true);
    internal MyGuiScreenPerformanceWarnings.WarningArea m_areaTitlePerformance = new MyGuiScreenPerformanceWarnings.WarningArea(MyCommonTexts.PerformanceWarningIssues, MySessionComponentWarningSystem.Category.Performance, false, false, false);
    internal MyGuiScreenPerformanceWarnings.WarningArea m_areaTitleGeneral = new MyGuiScreenPerformanceWarnings.WarningArea(MyCommonTexts.PerformanceWarningIssuesGeneral, MySessionComponentWarningSystem.Category.General, false, false, false);
    private int m_refreshFrameCounter = 120;
    private static bool m_showAll;
    private static bool m_showAllBlockLimits;
    private MyGuiControlMultilineText m_screenDescText;
    private MySessionComponentWarningSystem.Category? m_storedButtonCategory;

    public MyGuiScreenPerformanceWarnings()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.8436f, 0.97f)))
    {
      this.EnabledBackgroundFade = true;
      this.CloseButtonEnabled = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenPerformanceWarnings);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyTexts.GetString(MyCommonTexts.PerformanceWarningHelpHeader), captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.870000004768372 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.87f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.870000004768372 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.847000002861023)), this.m_size.Value.X * 0.87f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      this.m_warningsList = new MyGuiControlList(new Vector2?(new Vector2(0.0f, -0.05f)), new Vector2?(new Vector2(0.731f, 0.685f)));
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(new Vector2?(new Vector2(-0.365f, 0.329f)), text: MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_EnablePerformanceWarnings));
      string toolTip1 = MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsEnablePerformanceWarnings);
      this.m_showWarningsCheckBox = new MyGuiControlCheckbox(new Vector2?(new Vector2((float) ((double) myGuiControlLabel1.Position.X + (double) myGuiControlLabel1.Size.X + 0.00999999977648258), 0.329f)), toolTip: toolTip1, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_showWarningsCheckBox.IsChecked = MySandboxGame.Config.EnablePerformanceWarnings;
      this.m_showWarningsCheckBox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.ShowWarningsChanged);
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_showWarningsCheckBox.PositionX + 0.04f, 0.329f)), text: MyTexts.GetString(MyCommonTexts.PerformanceWarningShowAll));
      string toolTip2 = MyTexts.GetString(MyCommonTexts.ToolTipPerformanceWarningShowAll);
      this.m_showAllCheckBox = new MyGuiControlCheckbox(new Vector2?(new Vector2((float) ((double) myGuiControlLabel2.Position.X + (double) myGuiControlLabel2.Size.X + 0.00999999977648258), 0.329f)), toolTip: toolTip2, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_showAllCheckBox.IsChecked = MyGuiScreenPerformanceWarnings.m_showAll;
      this.m_showAllCheckBox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.KeepInListChanged);
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_showAllCheckBox.PositionX + 0.04f, 0.329f)), text: MyTexts.GetString(MyCommonTexts.PerformanceWarningShowAllBlockLimits));
      string toolTip3 = MyTexts.GetString(MyCommonTexts.ToolTipPerformanceWarningShowAllBlockLimits);
      this.m_showAllBlockLimitsCheckBox = new MyGuiControlCheckbox(new Vector2?(new Vector2((float) ((double) myGuiControlLabel3.Position.X + (double) myGuiControlLabel3.Size.X + 0.00999999977648258), 0.329f)), toolTip: toolTip3, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_showAllBlockLimitsCheckBox.IsChecked = MyGuiScreenPerformanceWarnings.m_showAllBlockLimits;
      this.m_showAllBlockLimitsCheckBox.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.KeepInListChangedBlockLimits);
      this.m_screenDescText = new MyGuiControlMultilineText(new Vector2?(new Vector2(-0.365f, 0.381f)), new Vector2?(new Vector2(0.4f, 0.2f)), contents: new StringBuilder(MyTexts.GetString(MyCommonTexts.PerformanceWarningInfoText)));
      this.m_screenDescText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_screenDescText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_screenDescText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      Vector2? position = new Vector2?(new Vector2(0.281f, 0.415f));
      Vector2? size = new Vector2?();
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(MyCommonTexts.Close);
      string toolTip4 = MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Close);
      StringBuilder text = stringBuilder;
      int? buttonIndex = new int?();
      this.m_okButton = new MyGuiControlButton(position, size: size, colorMask: colorMask, toolTip: toolTip4, text: text, buttonIndex: buttonIndex);
      this.m_okButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnOkButtonClicked);
      this.Controls.Add((MyGuiControlBase) this.m_warningsList);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.Controls.Add((MyGuiControlBase) this.m_showWarningsCheckBox);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.Controls.Add((MyGuiControlBase) this.m_showAllCheckBox);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
      this.Controls.Add((MyGuiControlBase) this.m_showAllBlockLimitsCheckBox);
      this.Controls.Add((MyGuiControlBase) this.m_screenDescText);
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_screenDescText.PositionX, this.m_okButton.Position.Y - MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui.Y / 2f)));
      myGuiControlLabel4.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.PerformanceWarnings_Help_Screen);
      this.FocusedControl = (MyGuiControlBase) this.m_showWarningsCheckBox;
    }

    protected override void OnClosed()
    {
      base.OnClosed();
      MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
    }

    private void PersistentButtonsSelectionStore()
    {
      this.m_storedButtonCategory = new MySessionComponentWarningSystem.Category?();
      if (this.FocusedControl == null)
        return;
      if (this.FocusedControl == this.m_areaTitleOther.Button)
        this.m_storedButtonCategory = new MySessionComponentWarningSystem.Category?(MySessionComponentWarningSystem.Category.Other);
      else if (this.FocusedControl == this.m_areaTitleServer.Button)
        this.m_storedButtonCategory = new MySessionComponentWarningSystem.Category?(MySessionComponentWarningSystem.Category.Server);
      else if (this.FocusedControl == this.m_areaTitleBlocks.Button)
        this.m_storedButtonCategory = new MySessionComponentWarningSystem.Category?(MySessionComponentWarningSystem.Category.Blocks);
      else if (this.FocusedControl == this.m_areaTitleGeneral.Button)
        this.m_storedButtonCategory = new MySessionComponentWarningSystem.Category?(MySessionComponentWarningSystem.Category.General);
      else if (this.FocusedControl == this.m_areaTitleGraphics.Button)
        this.m_storedButtonCategory = new MySessionComponentWarningSystem.Category?(MySessionComponentWarningSystem.Category.Graphics);
      else if (this.FocusedControl == this.m_areaTitleBlockLimits.Button)
      {
        this.m_storedButtonCategory = new MySessionComponentWarningSystem.Category?(MySessionComponentWarningSystem.Category.BlockLimits);
      }
      else
      {
        if (this.FocusedControl != this.m_areaTitlePerformance.Button)
          return;
        this.m_storedButtonCategory = new MySessionComponentWarningSystem.Category?(MySessionComponentWarningSystem.Category.Performance);
      }
    }

    private void PersistentButtonsSelectionRestore()
    {
      if (!this.m_storedButtonCategory.HasValue)
        return;
      MySessionComponentWarningSystem.Category? storedButtonCategory = this.m_storedButtonCategory;
      if (!storedButtonCategory.HasValue)
        return;
      switch (storedButtonCategory.GetValueOrDefault())
      {
        case MySessionComponentWarningSystem.Category.Graphics:
          if (this.m_areaTitleGraphics.Button == null)
            break;
          this.FocusedControl = (MyGuiControlBase) this.m_areaTitleGraphics.Button;
          break;
        case MySessionComponentWarningSystem.Category.Blocks:
          if (this.m_areaTitleBlocks.Button == null)
            break;
          this.FocusedControl = (MyGuiControlBase) this.m_areaTitleBlocks.Button;
          break;
        case MySessionComponentWarningSystem.Category.Other:
          if (this.m_areaTitleOther.Button == null)
            break;
          this.FocusedControl = (MyGuiControlBase) this.m_areaTitleOther.Button;
          break;
        case MySessionComponentWarningSystem.Category.BlockLimits:
          if (this.m_areaTitleBlockLimits.Button == null)
            break;
          this.FocusedControl = (MyGuiControlBase) this.m_areaTitleBlockLimits.Button;
          break;
        case MySessionComponentWarningSystem.Category.Server:
          if (this.m_areaTitleServer.Button == null)
            break;
          this.FocusedControl = (MyGuiControlBase) this.m_areaTitleServer.Button;
          break;
        case MySessionComponentWarningSystem.Category.Performance:
          if (this.m_areaTitlePerformance.Button == null)
            break;
          this.FocusedControl = (MyGuiControlBase) this.m_areaTitlePerformance.Button;
          break;
        case MySessionComponentWarningSystem.Category.General:
          if (this.m_areaTitleGeneral.Button == null)
            break;
          this.FocusedControl = (MyGuiControlBase) this.m_areaTitleGeneral.Button;
          break;
      }
    }

    private void Refresh()
    {
      if ((double) this.m_refreshFrameCounter < 60.0)
      {
        ++this.m_refreshFrameCounter;
      }
      else
      {
        float num = this.m_warningsList.GetScrollBar().Visible ? this.m_warningsList.GetScrollBar().Value : 0.0f;
        this.PersistentButtonsSelectionStore();
        this.m_warningsList.Controls.Clear();
        this.m_areaTitleOther.Warnings.Clear();
        this.m_areaTitleServer.Warnings.Clear();
        this.m_areaTitleBlocks.Warnings.Clear();
        this.m_areaTitleGeneral.Warnings.Clear();
        this.m_areaTitleGraphics.Warnings.Clear();
        this.m_areaTitleBlockLimits.Warnings.Clear();
        this.m_areaTitlePerformance.Warnings.Clear();
        this.CreateNonProfilerWarnings();
        this.CreateBlockLimitsWarnings();
        this.m_warningLines.Clear();
        foreach (MySimpleProfiler.PerformanceWarning warning in MySimpleProfiler.CurrentWarnings.Values)
        {
          MyGuiScreenPerformanceWarnings.WarningLine warningLine;
          if (((double) warning.Time < 300.0 || MyGuiScreenPerformanceWarnings.m_showAll) && !this.m_warningLines.TryGetValue(warning.Block.DisplayStringId, out warningLine))
          {
            warningLine = new MyGuiScreenPerformanceWarnings.WarningLine(warning, this);
            this.m_warningLines.Add(warning.Block.DisplayStringId, warningLine);
          }
        }
        this.m_refreshFrameCounter = 0;
        if (MyPlatformGameSettings.LIMITED_MAIN_MENU)
          this.m_areaTitleGraphics.Add(this.m_warningsList, MyGuiScreenPerformanceWarnings.m_showAll);
        this.m_areaTitleBlocks.Add(this.m_warningsList, MyGuiScreenPerformanceWarnings.m_showAll);
        this.m_areaTitleOther.Add(this.m_warningsList, MyGuiScreenPerformanceWarnings.m_showAll);
        this.m_areaTitleUnsafeGrids.Add(this.m_warningsList, MyGuiScreenPerformanceWarnings.m_showAll);
        this.m_areaTitleBlockLimits.Add(this.m_warningsList, MyGuiScreenPerformanceWarnings.m_showAll);
        this.m_areaTitleServer.Add(this.m_warningsList, MyGuiScreenPerformanceWarnings.m_showAll);
        this.m_areaTitlePerformance.Add(this.m_warningsList, MyGuiScreenPerformanceWarnings.m_showAll);
        this.m_areaTitleGeneral.Add(this.m_warningsList, MyGuiScreenPerformanceWarnings.m_showAll);
        this.m_warningsList.GetScrollBar().Value = num;
        this.PersistentButtonsSelectionRestore();
      }
    }

    private void CreateBlockLimitsWarnings()
    {
      if (MySession.Static == null)
        return;
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(MySession.Static.LocalPlayerId);
      if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
      {
        MyGuiScreenPerformanceWarnings.WarningLine warningLine1 = new MyGuiScreenPerformanceWarnings.WarningLine(MyTexts.GetString(MyCommonTexts.WorldSettings_BlockLimits), MyTexts.GetString(MyCommonTexts.Disabled), this.m_areaTitleBlockLimits);
      }
      if (identity == null)
        return;
      if (MySession.Static.MaxBlocksPerPlayer > 0 && (identity.BlockLimits.BlocksBuilt >= identity.BlockLimits.MaxBlocks || MyGuiScreenPerformanceWarnings.m_showAllBlockLimits))
      {
        MyGuiScreenPerformanceWarnings.WarningLine warningLine2 = new MyGuiScreenPerformanceWarnings.WarningLine(MyTexts.GetString(MyCommonTexts.PerformanceWarningBlocks), string.Format("{0}/{1} {2}", (object) identity.BlockLimits.BlocksBuilt, (object) identity.BlockLimits.MaxBlocks, (object) MyTexts.GetString(MyCommonTexts.PerformanceWarningBlocksBuilt)), this.m_areaTitleBlockLimits);
      }
      MyBlockLimits blockLimits = identity.BlockLimits;
      if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION)
      {
        MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(identity.IdentityId);
        if (playerFaction != null)
          blockLimits = playerFaction.BlockLimits;
      }
      if (MySession.Static.TotalPCU > -1 && (blockLimits.PCU == 0 || MyGuiScreenPerformanceWarnings.m_showAllBlockLimits))
      {
        MyGuiScreenPerformanceWarnings.WarningLine warningLine3 = new MyGuiScreenPerformanceWarnings.WarningLine("PCU", string.Format("{0} {1}", (object) blockLimits.PCU, (object) MyTexts.GetString(MyCommonTexts.PerformanceWarningPCUAvailable)), this.m_areaTitleBlockLimits);
      }
      foreach (KeyValuePair<string, short> blockTypeLimit in MySession.Static.BlockTypeLimits)
      {
        MyBlockLimits.MyTypeLimitData myTypeLimitData;
        identity.BlockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit.Key, out myTypeLimitData);
        MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.TryGetDefinitionGroup(blockTypeLimit.Key);
        if (definitionGroup != null && myTypeLimitData != null && (myTypeLimitData.BlocksBuilt >= (int) MySession.Static.GetBlockTypeLimit(blockTypeLimit.Key) || MyGuiScreenPerformanceWarnings.m_showAllBlockLimits))
        {
          MyGuiScreenPerformanceWarnings.WarningLine warningLine4 = new MyGuiScreenPerformanceWarnings.WarningLine(definitionGroup.Any.DisplayNameText, string.Format("{0}/{1} {2}", (object) myTypeLimitData.BlocksBuilt, (object) MySession.Static.GetBlockTypeLimit(blockTypeLimit.Key), (object) MyTexts.GetString(MyCommonTexts.PerformanceWarningBlocksBuilt)), this.m_areaTitleBlockLimits);
        }
      }
    }

    private void CreateNonProfilerWarnings()
    {
      if (MySessionComponentWarningSystem.Static != null)
      {
        DateTime now = DateTime.Now;
        foreach (MySessionComponentWarningSystem.Warning currentWarning in MySessionComponentWarningSystem.Static.CurrentWarnings)
        {
          if (MyGuiScreenPerformanceWarnings.m_showAll || !currentWarning.Time.HasValue || currentWarning.Time.Value.Subtract(now).Seconds < 5)
          {
            MyGuiScreenPerformanceWarnings.WarningLine warningLine = new MyGuiScreenPerformanceWarnings.WarningLine(currentWarning.Title, currentWarning.Description, this.GetWarningAreaForCategory(currentWarning.Category), currentWarning.Time);
          }
        }
      }
      if (MyPlatformGameSettings.PUBLIC_BETA_MP_TEST)
      {
        MyGuiScreenPerformanceWarnings.WarningLine warningLine1 = new MyGuiScreenPerformanceWarnings.WarningLine(MyTexts.GetString(MyCommonTexts.PerformanceWarningIssues_BetaTest_Header), MyTexts.GetString(MyCommonTexts.PerformanceWarningIssues_BetaTest_Message), this.m_areaTitleServer);
      }
      if (MySession.Static != null && MySession.Static.MultiplayerLastMsg > 3.0)
      {
        string description = string.Format(MyTexts.GetString(MyCommonTexts.Multiplayer_LastMsg), (object) (int) MySession.Static.MultiplayerLastMsg);
        MyGuiScreenPerformanceWarnings.WarningLine warningLine2 = new MyGuiScreenPerformanceWarnings.WarningLine(MyTexts.GetString(MyCommonTexts.PerformanceWarningIssuesServer_Response), description, this.m_areaTitleServer);
      }
      if (!MyGameService.IsOnline)
      {
        MyGuiScreenPerformanceWarnings.WarningLine warningLine3 = new MyGuiScreenPerformanceWarnings.WarningLine(string.Format(MyTexts.GetString(MyCommonTexts.GeneralWarningIssues_SteamOffline), (object) MyGameService.Service.ServiceName), string.Format(MyTexts.GetString(MyCommonTexts.General_SteamOffline), (object) MyGameService.Service.ServiceName), this.m_areaTitlePerformance);
      }
      if (MySession.Static != null && MySession.Static.IsRunningExperimental || (MySandboxGame.Config.ExperimentalMode || MyDebugDrawSettings.DEBUG_DRAW_SERVER_WARNINGS))
      {
        MyGuiScreenPerformanceWarnings.WarningLine warningLine4 = new MyGuiScreenPerformanceWarnings.WarningLine(MyTexts.GetString(MyCommonTexts.GeneralWarningIssues_Experimental), MyTexts.GetString(MyCommonTexts.General_Experimental), this.m_areaTitlePerformance);
      }
      if (!MyGameService.Service.GetInstallStatus(out int _))
      {
        MyGuiScreenPerformanceWarnings.WarningLine warningLine5 = new MyGuiScreenPerformanceWarnings.WarningLine(MyTexts.GetString(MyCommonTexts.GeneralWarningIssues_InstallInProgress), MyTexts.GetString(MyCommonTexts.General_InstallInProgress), this.m_areaTitleGeneral);
      }
      if (MySession.Static == null)
        return;
      foreach (KeyValuePair<MyDLCs.MyDLC, int> usedUnownedDlC in MySession.Static.GetComponent<MySessionComponentDLC>().UsedUnownedDLCs)
      {
        MyGuiScreenPerformanceWarnings.WarningLine warningLine2 = new MyGuiScreenPerformanceWarnings.WarningLine(MyTexts.GetString(MyCommonTexts.PerformanceWarningTitle_PaidContent), string.Format(MyTexts.GetString(MyCommonTexts.PerformanceWarningIssuesPaidContent), (object) MyTexts.GetString(usedUnownedDlC.Key.DisplayName)), this.m_areaTitleGeneral);
      }
    }

    private void ShowWarningsChanged(MyGuiControlCheckbox obj) => MySandboxGame.Config.EnablePerformanceWarnings = obj.IsChecked;

    private void KeepInListChanged(MyGuiControlCheckbox obj) => MyGuiScreenPerformanceWarnings.m_showAll = obj.IsChecked;

    private void KeepInListChangedBlockLimits(MyGuiControlCheckbox obj)
    {
      MyGuiScreenPerformanceWarnings.m_showAllBlockLimits = obj.IsChecked;
      this.m_areaTitleBlockLimits.Warnings.Clear();
      this.CreateBlockLimitsWarnings();
    }

    private MyGuiScreenPerformanceWarnings.WarningArea GetWarningAreaForCategory(
      MySessionComponentWarningSystem.Category category)
    {
      switch (category)
      {
        case MySessionComponentWarningSystem.Category.Graphics:
          return this.m_areaTitleGraphics;
        case MySessionComponentWarningSystem.Category.Blocks:
          return this.m_areaTitleBlocks;
        case MySessionComponentWarningSystem.Category.Other:
          return this.m_areaTitleOther;
        case MySessionComponentWarningSystem.Category.UnsafeGrids:
          return this.m_areaTitleServer;
        case MySessionComponentWarningSystem.Category.BlockLimits:
          return this.m_areaTitleBlockLimits;
        case MySessionComponentWarningSystem.Category.Server:
          return this.m_areaTitleServer;
        case MySessionComponentWarningSystem.Category.Performance:
          return this.m_areaTitlePerformance;
        case MySessionComponentWarningSystem.Category.General:
          return this.m_areaTitleGeneral;
        default:
          return this.m_areaTitleOther;
      }
    }

    private void OnOkButtonClicked(MyGuiControlButton obj) => this.CloseScreen();

    public override bool Update(bool hasFocus)
    {
      this.Refresh();
      if (MyInput.Static.IsJoystickLastUsed && !this.m_okButton.Visible)
        this.UpdateGamepadHelp(this.FocusedControl);
      this.m_screenDescText.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      return base.Update(hasFocus);
    }

    internal class WarningLine
    {
      public MySimpleProfiler.PerformanceWarning Warning;
      private MyGuiControlLabel m_name;
      private MyGuiControlMultilineText m_description;
      public MyGuiControlParent Parent;
      private MyGuiControlSeparatorList m_separator;
      private MyGuiControlLabel m_time;

      public string Name => this.m_name != null ? this.m_name.Text : string.Empty;

      public string Time => this.m_time != null ? this.m_time.Text : string.Empty;

      public WarningLine(
        MySimpleProfiler.PerformanceWarning warning,
        MyGuiScreenPerformanceWarnings screen)
      {
        this.Parent = new MyGuiControlParent();
        string displayName = warning.Block.DisplayName;
        this.m_name = new MyGuiControlLabel(new Vector2?(new Vector2(-0.33f, 0.0f)), text: displayName, font: "Red");
        if ((double) this.m_name.Size.X > 0.140000000596046)
          this.m_name.Text = this.Truncate(displayName, 15, "..");
        this.m_name.SetToolTip(displayName);
        this.m_description = new MyGuiControlMultilineText();
        this.m_description.Position = new Vector2(-0.18f, 0.0f);
        this.m_description.Size = new Vector2(0.45f, 0.2f);
        MyStringId myStringId;
        if (!MyPlatformGameSettings.ENABLE_TRASH_REMOVAL_SETTING)
        {
          myStringId = warning.Block.DescriptionSimple;
          if (!string.IsNullOrEmpty(myStringId.String) && MyTexts.Exists(warning.Block.DescriptionSimple))
          {
            this.m_description.Text = MyTexts.Get(warning.Block.DescriptionSimple);
            goto label_6;
          }
        }
        MyGuiControlMultilineText description = this.m_description;
        myStringId = warning.Block.Description;
        StringBuilder stringBuilder = new StringBuilder(string.IsNullOrEmpty(myStringId.String) ? "" : MyTexts.GetString(warning.Block.Description));
        description.Text = stringBuilder;
label_6:
        this.m_description.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        this.m_description.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        this.m_description.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        this.m_description.Size = new Vector2(0.45f, this.m_description.TextSize.Y);
        this.Parent.Size = new Vector2(this.Parent.Size.X, this.m_description.Size.Y);
        this.m_separator = new MyGuiControlSeparatorList();
        this.m_separator.AddVertical(new Vector2(-0.19f, (float) (-(double) this.Parent.Size.Y / 2.0 - 3.0 / 500.0)), this.Parent.Size.Y + 0.016f);
        this.m_separator.AddVertical(new Vector2(0.26f, (float) (-(double) this.Parent.Size.Y / 2.0 - 3.0 / 500.0)), this.Parent.Size.Y + 0.016f);
        this.m_time = new MyGuiControlLabel(new Vector2?(new Vector2(0.33f, 0.0f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
        switch (warning.Block.Type)
        {
          case MySimpleProfiler.ProfilingBlockType.GPU:
          case MySimpleProfiler.ProfilingBlockType.RENDER:
            screen.m_areaTitleGraphics.Warnings.Add(this);
            break;
          case MySimpleProfiler.ProfilingBlockType.MOD:
          case MySimpleProfiler.ProfilingBlockType.OTHER:
            screen.m_areaTitleOther.Warnings.Add(this);
            break;
          case MySimpleProfiler.ProfilingBlockType.BLOCK:
            screen.m_areaTitleBlocks.Warnings.Add(this);
            break;
        }
        this.Warning = warning;
      }

      public WarningLine(
        string name,
        string description,
        MyGuiScreenPerformanceWarnings.WarningArea area,
        DateTime? time = null)
      {
        this.Parent = new MyGuiControlParent();
        string str = name;
        this.m_name = new MyGuiControlLabel(new Vector2?(new Vector2(-0.33f, 0.0f)), text: str, font: "Red");
        if ((double) this.m_name.Size.X > 0.140000000596046)
          this.m_name.Text = this.Truncate(str, 15, "..");
        this.m_name.SetToolTip(str);
        this.m_name.ShowTooltipWhenDisabled = true;
        this.m_description = new MyGuiControlMultilineText();
        this.m_description.Position = new Vector2(-0.18f, 0.0f);
        this.m_description.Size = new Vector2(0.45f, 0.2f);
        this.m_description.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        this.m_description.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        this.m_description.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        MyWikiMarkupParser.ParseText(description, ref this.m_description);
        this.m_description.Size = new Vector2(0.45f, this.m_description.TextSize.Y);
        MyWikiMarkupParser.ParseText(description, ref this.m_description);
        this.m_description.OnLinkClicked += new LinkClicked(this.OnLinkClicked);
        this.m_separator = new MyGuiControlSeparatorList();
        this.Parent.Size = new Vector2(this.Parent.Size.X, this.m_description.Size.Y);
        this.m_separator.AddVertical(new Vector2(-0.19f, (float) (-(double) this.Parent.Size.Y / 2.0 - 3.0 / 500.0)), this.Parent.Size.Y + 0.016f);
        this.m_separator.AddVertical(new Vector2(0.35f, (float) (-(double) this.Parent.Size.Y / 2.0 - 3.0 / 500.0)), this.Parent.Size.Y + 0.016f);
        this.m_time = new MyGuiControlLabel(new Vector2?(new Vector2(0.33f, 0.0f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
        if (time.HasValue)
        {
          this.m_separator.AddVertical(new Vector2(0.26f, (float) (-(double) this.Parent.Size.Y / 2.0 - 3.0 / 500.0)), this.Parent.Size.Y + 0.016f);
          TimeSpan timeSpan = DateTime.Now - time.Value;
          this.m_time.Text = string.Format("{0}:{1:00}:{2:00}", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
        }
        area.Warnings.Add(this);
      }

      public void Prepare()
      {
        this.Parent.Position = Vector2.Zero;
        if (this.Warning != null)
        {
          TimeSpan timeSpan = TimeSpan.FromSeconds((double) (int) ((double) this.Warning.Time * 0.0166666675359011));
          this.m_time.Text = string.Format("{0}:{1:00}:{2:00}", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
        }
        if (this.Parent.Controls.Count != 0)
          return;
        this.Parent.Controls.Add((MyGuiControlBase) this.m_name);
        this.Parent.Controls.Add((MyGuiControlBase) this.m_description);
        this.Parent.Controls.Add((MyGuiControlBase) this.m_separator);
        this.Parent.Controls.Add((MyGuiControlBase) this.m_time);
      }

      private string Truncate(string input, int maxLenght, string tooLongSuffix) => input.Length < maxLenght ? input : input.Substring(0, maxLenght - tooLongSuffix.Length) + tooLongSuffix;

      private void OnLinkClicked(MyGuiControlBase sender, string url) => MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);
    }

    internal class WarningArea
    {
      internal List<MyGuiScreenPerformanceWarnings.WarningLine> Warnings;
      private MyGuiControlParent m_header;
      private MyGuiControlPanel m_titleBackground;
      private MyGuiControlLabel m_title;
      private MyGuiControlLabel m_lastOccurence;
      private MyGuiControlSeparatorList m_separator;
      private MyGuiControlButton m_refButton;

      public MyGuiControlButton Button => this.m_refButton;

      public WarningArea(
        MyStringId name,
        MySessionComponentWarningSystem.Category areaType,
        bool refButton,
        bool unsafeGrid,
        bool serverMessage)
      {
        this.Warnings = new List<MyGuiScreenPerformanceWarnings.WarningLine>();
        this.m_header = new MyGuiControlParent();
        this.m_titleBackground = new MyGuiControlPanel(texture: "Textures\\GUI\\Controls\\item_highlight_dark.dds");
        this.m_title = new MyGuiControlLabel(text: MyTexts.GetString(name));
        this.m_separator = new MyGuiControlSeparatorList();
        this.m_separator.AddHorizontal(new Vector2(-0.45f, 0.018f), 0.9f);
        this.m_title.Position = new Vector2(-0.33f, 0.0f);
        this.m_titleBackground.Size = new Vector2(this.m_titleBackground.Size.X, 0.035f);
        this.m_header.Size = new Vector2(this.m_header.Size.X, this.m_titleBackground.Size.Y);
        if (!unsafeGrid)
        {
          this.m_lastOccurence = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.PerformanceWarningLastOccurrence), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
          this.m_lastOccurence.Position = new Vector2(0.33f, 0.0f);
        }
        if (!refButton || MySession.Static == null)
          return;
        switch (areaType)
        {
          case MySessionComponentWarningSystem.Category.Graphics:
            if (MyPlatformGameSettings.LIMITED_MAIN_MENU)
              break;
            this.m_refButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.ToolbarButton, text: MyTexts.Get(MyCommonTexts.ScreenCaptionGraphicsOptions), onButtonClick: ((Action<MyGuiControlButton>) (sender => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenOptionsGraphics()))));
            break;
          case MySessionComponentWarningSystem.Category.Blocks:
            if (!MyPlatformGameSettings.ENABLE_TRASH_REMOVAL_SETTING && !MyFakes.FORCE_ADD_TRASH_REMOVAL_MENU)
              break;
            this.m_refButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.ToolbarButton, text: MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Cleanup), onButtonClick: ((Action<MyGuiControlButton>) (sender => MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen)))));
            break;
          case MySessionComponentWarningSystem.Category.Other:
            this.m_refButton = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.ToolbarButton, text: MyTexts.Get(MyCommonTexts.ScreenCaptionGraphicsOptions), onButtonClick: ((Action<MyGuiControlButton>) (sender => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenOptionsGame()))));
            break;
        }
      }

      public void Add(MyGuiControlList list, bool showAll)
      {
        this.m_header.Position = Vector2.Zero;
        if (this.m_header.Controls.Count == 0)
        {
          this.m_header.Controls.Add((MyGuiControlBase) this.m_titleBackground);
          this.m_header.Controls.Add((MyGuiControlBase) this.m_title);
          this.m_header.Controls.Add((MyGuiControlBase) this.m_separator);
          if (this.m_lastOccurence != null)
            this.m_header.Controls.Add((MyGuiControlBase) this.m_lastOccurence);
        }
        bool flag = false;
        this.Warnings.Sort((Comparison<MyGuiScreenPerformanceWarnings.WarningLine>) ((x, y) =>
        {
          if (x.Warning == null && y.Warning == null)
            return string.Compare(x.Name, y.Name);
          if (x.Warning == null)
            return -y.Warning.Time;
          if (y.Warning == null)
            return x.Warning.Time;
          return y.Warning.Time == 0 && x.Warning.Time == 0 ? string.Compare(x.Name, y.Name) : x.Warning.Time - y.Warning.Time;
        }));
        foreach (MyGuiScreenPerformanceWarnings.WarningLine warning in this.Warnings)
        {
          if (((warning.Warning == null ? 1 : ((double) warning.Warning.Time < 300.0 ? 1 : 0)) | (showAll ? 1 : 0)) != 0)
          {
            if (!flag)
            {
              list.Controls.Add((MyGuiControlBase) this.m_header);
              flag = true;
            }
            warning.Prepare();
            list.Controls.Add((MyGuiControlBase) warning.Parent);
          }
        }
        if (!flag)
          return;
        if (this.m_refButton != null)
          list.Controls.Add((MyGuiControlBase) this.m_refButton);
        MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
        controlSeparatorList.AddHorizontal(new Vector2(-0.45f, 0.0f), 0.9f);
        controlSeparatorList.Size = new Vector2(1f, 0.005f);
        controlSeparatorList.ColorMask = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        list.Controls.Add((MyGuiControlBase) controlSeparatorList);
      }
    }
  }
}
