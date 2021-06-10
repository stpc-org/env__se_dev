// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenNewWorkshopGame
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Localization;
using VRage.GameServices;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenNewWorkshopGame : MyGuiScreenBase
  {
    private MyGuiControlScreenSwitchPanel m_screenSwitchPanel;
    private MyGuiBlueprintScreen_Reworked.SortOption m_sort;
    private bool m_showThumbnails = true;
    private MyGuiControlButton m_buttonRefresh;
    private MyGuiControlButton m_buttonSorting;
    private MyGuiControlButton m_buttonToggleThumbnails;
    private MyGuiControlImage m_iconRefresh;
    private MyGuiControlImage m_iconSorting;
    private MyGuiControlImage m_iconToggleThumbnails;
    private MyGuiControlSearchBox m_searchBox;
    private MyGuiControlList m_worldList;
    private MyGuiControlRadioButtonGroup m_worldTypesGroup;
    private MyObjectBuilder_Checkpoint m_selectedWorld;
    private MyGuiControlContentButton m_selectedButton;
    private MyWorkshopItem m_selectedWorkshopItem;
    private readonly object m_selectedWorldLock = new object();
    private List<MyWorkshopItem> SubscribedWorlds;
    private MyLayoutTable m_tableLayout;
    private MyGuiControlLabel m_nameLabel;
    private MyGuiControlLabel m_nameText;
    private MyGuiControlLabel m_onlineModeLabel;
    private MyGuiControlCombobox m_onlineMode;
    private MyGuiControlSlider m_maxPlayersSlider;
    private MyGuiControlLabel m_maxPlayersLabel;
    private MyGuiControlLabel m_authorLabel;
    private MyGuiControlLabel m_authorText;
    private MyGuiControlLabel m_ratingLabel;
    private MyGuiControlRating m_ratingDisplay;
    private MyGuiControlButton m_buttonRateUp;
    private MyGuiControlButton m_buttonRateDown;
    private MyGuiControlImage m_iconRateUp;
    private MyGuiControlImage m_iconRateDown;
    private MyGuiControlMultilineText m_noSubscribedItemsText;
    private MyGuiControlPanel m_noSubscribedItemsPanel;
    private MyGuiControlMultilineText m_descriptionMultilineText;
    private MyGuiControlPanel m_descriptionPanel;
    private MyGuiControlLabel m_workshopError;
    private MyGuiControlRotatingWheel m_asyncLoadingWheel;
    private float MARGIN_TOP = 0.22f;
    private float MARGIN_BOTTOM = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
    private float MARGIN_LEFT_INFO = 15f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
    private float MARGIN_RIGHT = 81f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
    private float MARGIN_LEFT_LIST = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
    private bool m_displayTabScenario;
    private bool m_displayTabWorkshop;
    private bool m_displayTabCustom;
    private MyGuiControlButton m_okButton;
    private MyGuiControlButton m_workshopButton;
    private int m_maxPlayers;
    private MyGuiControlButton m_buttonGroup;
    private int m_groupSelection;
    private MyGuiControlImage m_iconGroupSelection;
    private bool m_workshopPermitted;

    public MyGuiScreenNewWorkshopGame(
      bool displayTabScenario = true,
      bool displayTabWorkshop = true,
      bool displayTabCustom = true)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.878f, 0.97f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_workshopPermitted = true;
      MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<bool>) (x => this.m_workshopPermitted = x));
      this.m_displayTabScenario = displayTabScenario;
      this.m_displayTabWorkshop = displayTabWorkshop;
      this.m_displayTabCustom = displayTabCustom;
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => "New Workshop Game";

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyCommonTexts.ScreenMenuButtonCampaign);
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.379999995231628 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.625f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      this.m_workshopError = new MyGuiControlLabel(colorMask: new Vector4?((Vector4) Color.Red));
      this.m_workshopError.Position = new Vector2(-0.382f, 0.46f);
      this.m_workshopError.Visible = false;
      this.Controls.Add((MyGuiControlBase) this.m_workshopError);
      this.m_screenSwitchPanel = new MyGuiControlScreenSwitchPanel((MyGuiScreenBase) this, MyTexts.Get(MyCommonTexts.WorkshopScreen_Description), this.m_displayTabScenario, this.m_displayTabWorkshop, this.m_displayTabCustom);
      this.InitWorldList();
      this.InitRightSide();
      this.m_asyncLoadingWheel = new MyGuiControlRotatingWheel(new Vector2?(new Vector2((float) ((double) this.m_size.Value.X / 2.0 - 0.0769999995827675), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.108000002801418))), new Vector4?(MyGuiConstants.ROTATING_WHEEL_COLOR), 0.2f);
      this.m_asyncLoadingWheel.Visible = false;
      this.Controls.Add((MyGuiControlBase) this.m_asyncLoadingWheel);
      this.RefreshList();
      this.FocusedControl = (MyGuiControlBase) this.m_searchBox.TextBox;
      this.CheckUGCServices();
    }

    public void SetWorkshopErrorText(string text = "", bool visible = true, bool skipUGCCheck = false)
    {
      if (!skipUGCCheck && string.IsNullOrEmpty(text))
      {
        this.CheckUGCServices();
      }
      else
      {
        this.m_workshopError.Text = text;
        this.m_workshopError.Visible = visible;
      }
    }

    public override bool RegisterClicks() => true;

    private void InitWorldList()
    {
      float y = 0.31f;
      Vector2 vector2 = -this.m_size.Value / 2f + new Vector2(90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, y);
      int index1 = 0;
      int num1 = index1 + 1;
      this.m_buttonRefresh = this.CreateToolbarButton(index1, MyCommonTexts.WorldSettings_Tooltip_Refresh, new Action<MyGuiControlButton>(this.OnRefreshClicked));
      if (MyGameService.WorkshopService.GetAggregates().Count > 1)
      {
        string tooltip = string.Empty;
        if (MyGameService.WorkshopService.GetAggregates().Count > 1)
          tooltip = string.Format(MyTexts.Get(MySpaceTexts.WorldSettings_Tooltip_ButGrouping).ToString(), (object) MyGameService.WorkshopService.GetAggregates()[0].ServiceName, (object) MyGameService.WorkshopService.GetAggregates()[1].ServiceName);
        this.m_buttonGroup = this.CreateToolbarButton(num1++, tooltip, new Action<MyGuiControlButton>(this.OnButton_GroupSelection));
        this.m_iconGroupSelection = this.CreateToolbarButtonIcon(this.m_buttonGroup, "");
        this.SetIconForGroupSelection();
      }
      int index2 = num1;
      int num2 = index2 + 1;
      this.m_buttonSorting = this.CreateToolbarButton(index2, MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButSort, new Action<MyGuiControlButton>(this.OnSortingClicked));
      int index3 = num2;
      int num3 = index3 + 1;
      this.m_buttonToggleThumbnails = this.CreateToolbarButton(index3, MyCommonTexts.WorldSettings_Tooltip_ToggleThumbnails, new Action<MyGuiControlButton>(this.OnToggleThumbnailsClicked));
      int index4 = num3;
      int num4 = index4 + 1;
      this.CreateToolbarButtonIcon(this.CreateToolbarButton(index4, MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButOpenWorkshop, new Action<MyGuiControlButton>(this.OnOpenWorkshopClicked)), "Textures\\GUI\\Icons\\Browser\\WorkshopBrowser.dds");
      this.m_iconRefresh = this.CreateToolbarButtonIcon(this.m_buttonRefresh, "Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
      this.m_iconSorting = this.CreateToolbarButtonIcon(this.m_buttonSorting, "");
      this.SetIconForSorting();
      this.m_iconToggleThumbnails = this.CreateToolbarButtonIcon(this.m_buttonToggleThumbnails, "");
      this.SetIconForHideThumbnails();
      this.m_worldTypesGroup = new MyGuiControlRadioButtonGroup();
      this.m_worldTypesGroup.SelectedChanged += new Action<MyGuiControlRadioButtonGroup>(this.WorldSelectionChanged);
      this.m_worldTypesGroup.MouseDoubleClick += new Action<MyGuiControlRadioButton>(this.WorldDoubleClick);
      MyGuiControlList myGuiControlList = new MyGuiControlList();
      myGuiControlList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlList.Position = vector2;
      myGuiControlList.Size = new Vector2(MyGuiConstants.LISTBOX_WIDTH, (float) ((double) this.m_size.Value.Y - (double) y - 0.0480000004172325));
      this.m_worldList = myGuiControlList;
      this.Controls.Add((MyGuiControlBase) this.m_worldList);
      this.m_searchBox = new MyGuiControlSearchBox(new Vector2?(new Vector2(-0.382f, -0.22f)), new Vector2?(new Vector2(this.m_worldList.Size.X, 0.032f)), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_searchBox.OnTextChanged += new MyGuiControlSearchBox.TextChangedDelegate(this.OnSearchTextChange);
      this.Controls.Add((MyGuiControlBase) this.m_searchBox);
    }

    private void OnButton_GroupSelection(MyGuiControlButton button)
    {
      ++this.m_groupSelection;
      if (this.m_groupSelection > MyGameService.WorkshopService.GetAggregates().Count)
        this.m_groupSelection = 0;
      this.UpdateGroupSelection();
    }

    private void UpdateGroupSelection()
    {
      this.SetIconForGroupSelection();
      this.ClearItems();
      this.FillItems();
      this.TrySelectFirstItem();
    }

    private void SetIconForGroupSelection()
    {
      if (this.m_groupSelection == 0)
        this.m_iconGroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\WNG_Service_Mixed.png");
      else
        this.m_iconGroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_" + MyGameService.WorkshopService.GetAggregates()[this.m_groupSelection - 1].ServiceName + ".png");
    }

    private MyGuiControlButton CreateToolbarButton(
      int index,
      MyStringId tooltip,
      Action<MyGuiControlButton> onClick)
    {
      return this.CreateToolbarButton(index, MyTexts.Get(tooltip).ToString(), onClick);
    }

    private MyGuiControlButton CreateToolbarButton(
      int index,
      string tooltip,
      Action<MyGuiControlButton> onClick)
    {
      MyGuiControlButton guiControlButton1 = new MyGuiControlButton(new Vector2?(new Vector2(-0.366f, -0.261f) + (index > 0 ? new Vector2(this.m_buttonRefresh.Size.X, 0.0f) * (float) index : Vector2.Zero)), onButtonClick: onClick);
      guiControlButton1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      guiControlButton1.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
      guiControlButton1.ShowTooltipWhenDisabled = true;
      guiControlButton1.Size = new Vector2(0.029f, 0.03358333f);
      MyGuiControlButton guiControlButton2 = guiControlButton1;
      guiControlButton2.SetToolTip(tooltip);
      this.Controls.Add((MyGuiControlBase) guiControlButton2);
      return guiControlButton2;
    }

    private MyGuiControlImage CreateToolbarButtonIcon(
      MyGuiControlButton button,
      string texture)
    {
      MyGuiControlImage icon = new MyGuiControlImage(textures: new string[1]
      {
        texture
      });
      this.AdjustButtonForIcon(button, icon);
      float y = 0.95f * Math.Min(button.Size.X, button.Size.Y);
      icon.Size = new Vector2(y * 0.75f, y);
      icon.Position = button.Position + new Vector2(-1f / 625f, 0.018f);
      this.Controls.Add((MyGuiControlBase) icon);
      return icon;
    }

    private MyGuiControlButton CreateRateButton(bool positive)
    {
      Vector2? position = new Vector2?();
      Action<MyGuiControlButton> action = positive ? new Action<MyGuiControlButton>(this.OnRateUpClicked) : new Action<MyGuiControlButton>(this.OnRateDownClicked);
      Vector2? size = new Vector2?(new Vector2(0.03f));
      Vector4? colorMask = new Vector4?();
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Rectangular, size, colorMask, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
    }

    private MyGuiControlImage CreateRateIcon(
      MyGuiControlButton button,
      string texture)
    {
      MyGuiControlImage icon = new MyGuiControlImage(textures: new string[1]
      {
        texture
      });
      this.AdjustButtonForIcon(button, icon);
      icon.Size = button.Size * 0.6f;
      return icon;
    }

    private void AdjustButtonForIcon(MyGuiControlButton button, MyGuiControlImage icon)
    {
      button.Size = new Vector2(button.Size.X, (float) ((double) button.Size.X * 4.0 / 3.0));
      button.HighlightChanged += (Action<MyGuiControlBase>) (x => icon.ColorMask = x.HasHighlight ? MyGuiConstants.HIGHLIGHT_TEXT_COLOR : Vector4.One);
    }

    private void SetIconForSorting()
    {
      switch (this.m_sort)
      {
        case MyGuiBlueprintScreen_Reworked.SortOption.None:
          this.m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\NoSorting.png");
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
          this.m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\Alphabetical.png");
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
          this.m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ByCreationDate.png");
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
          this.m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ByUpdateDate.png");
          break;
        default:
          this.m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\NoSorting.png");
          break;
      }
    }

    private void SetIconForHideThumbnails() => this.m_iconToggleThumbnails.SetTexture(this.m_showThumbnails ? "Textures\\GUI\\Icons\\Blueprints\\ThumbnailsON.png" : "Textures\\GUI\\Icons\\Blueprints\\ThumbnailsOFF.png");

    private void WorldSelectionChanged(MyGuiControlRadioButtonGroup args)
    {
      if (!(args.SelectedButton is MyGuiControlContentButton selectedButton) || selectedButton.UserData == null)
        return;
      MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem> userData = (MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>) selectedButton.UserData;
      lock (this.m_selectedWorldLock)
      {
        this.m_selectedWorld = userData.Item1;
        this.m_selectedWorkshopItem = userData.Item2;
      }
      if (this.m_selectedButton != null)
        this.m_selectedButton.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
      this.m_selectedButton = selectedButton;
      this.m_selectedButton.HighlightType = MyGuiControlHighlightType.CUSTOM;
      this.m_selectedButton.HasHighlight = true;
      string text = (string) null;
      StringBuilder stringBuilder1 = (StringBuilder) null;
      MyLocalizationContext localizationContext = MyLocalization.Static[this.m_selectedWorld.SessionName];
      if (localizationContext != null)
      {
        StringBuilder stringBuilder2 = localizationContext["Name"];
        if (stringBuilder2 != null)
          text = stringBuilder2.ToString();
        stringBuilder1 = localizationContext["Description"];
      }
      if (string.IsNullOrEmpty(text))
        text = this.m_selectedWorld.SessionName;
      if (stringBuilder1 == null)
        stringBuilder1 = new StringBuilder(this.m_selectedWorld.Description);
      this.m_nameText.IsAutoEllipsisEnabled = true;
      this.m_nameText.SetMaxWidth(0.31f);
      this.m_nameText.SetToolTip(text);
      this.m_nameLabel.SetToolTip(text);
      this.m_nameText.Text = text;
      this.m_nameText.DoEllipsisAndScaleAdjust(resetEllipsis: true);
      this.m_ratingDisplay.Value = (int) Math.Round((double) this.m_selectedWorkshopItem.Score * 10.0);
      int myRating = this.m_selectedWorkshopItem.MyRating;
      this.m_buttonRateUp.Checked = myRating == 1;
      this.m_buttonRateDown.Checked = myRating == -1;
      this.m_descriptionMultilineText.Text = stringBuilder1;
      this.m_descriptionMultilineText.SetScrollbarPageV(0.0f);
      this.m_descriptionMultilineText.SetScrollbarPageH(0.0f);
      MyGuiScreenNewWorkshopGame.WorldDataLoaderInfo worldDataLoaderInfo = new MyGuiScreenNewWorkshopGame.WorldDataLoaderInfo()
      {
        World = userData.Item1,
        WorkshopItem = userData.Item2
      };
      this.WorldDataLoaded((WorkData) worldDataLoaderInfo);
      this.m_asyncLoadingWheel.Visible = true;
      Parallel.Start(new Action<WorkData>(this.LoadWorldData), new Action<WorkData>(this.WorldDataLoaded), (WorkData) worldDataLoaderInfo);
    }

    private void LoadWorldData(WorkData data)
    {
      MyGuiScreenNewWorkshopGame.WorldDataLoaderInfo worldDataLoaderInfo = (MyGuiScreenNewWorkshopGame.WorldDataLoaderInfo) data;
      lock (this.m_selectedWorldLock)
      {
        if (worldDataLoaderInfo.WorkshopItem == this.m_selectedWorkshopItem)
        {
          if (worldDataLoaderInfo.World == this.m_selectedWorld)
            goto label_7;
        }
        worldDataLoaderInfo.IsSkipped = true;
        return;
      }
label_7:
      string path = Path.Combine(worldDataLoaderInfo.WorkshopItem.Folder + "\\Sandbox.sbc");
      if (!MyFileSystem.FileExists(path))
        return;
      MyObjectBuilder_Checkpoint checkpoint;
      if (!MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Checkpoint>(path, out checkpoint))
        return;
      MyObjectBuilder_Identity objectBuilderIdentity = checkpoint.Identities.Find((Predicate<MyObjectBuilder_Identity>) (x => x.CharacterEntityId == checkpoint.ControlledObject));
      if (objectBuilderIdentity != null)
        worldDataLoaderInfo.Author = objectBuilderIdentity.DisplayName;
      worldDataLoaderInfo.IsMultiplayer = (uint) checkpoint.OnlineMode > 0U;
    }

    private void WorldDataLoaded(WorkData data)
    {
      MyGuiScreenNewWorkshopGame.WorldDataLoaderInfo worldDataLoaderInfo = (MyGuiScreenNewWorkshopGame.WorldDataLoaderInfo) data;
      if (worldDataLoaderInfo.IsSkipped)
        return;
      lock (this.m_selectedWorldLock)
      {
        if (worldDataLoaderInfo.WorkshopItem != this.m_selectedWorkshopItem)
          return;
        if (worldDataLoaderInfo.World != this.m_selectedWorld)
          return;
      }
      if (worldDataLoaderInfo.IsMultiplayer)
      {
        this.m_onlineMode.Enabled = true;
      }
      else
      {
        this.m_onlineMode.Enabled = false;
        this.m_onlineMode.SelectItemByIndex(0);
      }
      this.m_authorText.Text = worldDataLoaderInfo.Author;
      this.m_maxPlayersSlider.Enabled = this.m_onlineMode.Enabled && this.m_onlineMode.GetSelectedIndex() > 0 && this.m_maxPlayers > 2;
      this.m_asyncLoadingWheel.Visible = false;
    }

    private void InitRightSide()
    {
      int num1 = 5;
      Vector2 topLeft = -this.m_size.Value / 2f + new Vector2((float) ((double) this.MARGIN_LEFT_LIST + (double) this.m_worldList.Size.X + (double) this.MARGIN_LEFT_INFO + 0.0120000001043081), this.MARGIN_TOP - 11f / 1000f);
      Vector2 vector2_1 = this.m_size.Value;
      Vector2 size = new Vector2(vector2_1.X / 2f - topLeft.X, (float) ((double) vector2_1.Y - (double) this.MARGIN_TOP - (double) this.MARGIN_BOTTOM - 0.0344999991357327)) - new Vector2(this.MARGIN_RIGHT, 0.12f);
      float num2 = size.X * 0.6f;
      float num3 = size.X - num2;
      float num4 = 0.052f;
      float num5 = size.Y - (float) num1 * num4;
      this.m_tableLayout = new MyLayoutTable((IMyGuiControlsParent) this, topLeft, size);
      this.m_tableLayout.SetColumnWidthsNormalized(num2 - 0.055f, num3 + 0.055f);
      this.m_tableLayout.SetRowHeightsNormalized(num4, num4, num4, num4, num4, num5);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.Text = MyTexts.GetString(MyCommonTexts.Name);
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_nameLabel = myGuiControlLabel1;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_nameText = myGuiControlLabel2;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Text = MyTexts.GetString(MyCommonTexts.WorldSettings_Author);
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      this.m_authorLabel = myGuiControlLabel3;
      MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel();
      myGuiControlLabel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_authorText = myGuiControlLabel4;
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel();
      myGuiControlLabel5.Text = MyTexts.GetString(MyCommonTexts.WorldSettings_Rating);
      myGuiControlLabel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      this.m_ratingLabel = myGuiControlLabel5;
      MyGuiControlRating guiControlRating = new MyGuiControlRating(8);
      guiControlRating.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_ratingDisplay = guiControlRating;
      this.m_buttonRateUp = this.CreateRateButton(true);
      this.m_iconRateUp = this.CreateRateIcon(this.m_buttonRateUp, "Textures\\GUI\\Icons\\Blueprints\\like_test.png");
      this.m_buttonRateDown = this.CreateRateButton(false);
      this.m_iconRateDown = this.CreateRateIcon(this.m_buttonRateDown, "Textures\\GUI\\Icons\\Blueprints\\dislike_test.png");
      MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel();
      myGuiControlLabel6.Text = MyTexts.GetString(MyCommonTexts.WorldSettings_OnlineMode);
      myGuiControlLabel6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      this.m_onlineModeLabel = myGuiControlLabel6;
      MyGuiControlCombobox guiControlCombobox = new MyGuiControlCombobox();
      guiControlCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_onlineMode = guiControlCombobox;
      this.m_onlineMode.AddItem(0L, MyCommonTexts.WorldSettings_OnlineModeOffline);
      this.m_onlineMode.AddItem(3L, MyCommonTexts.WorldSettings_OnlineModePrivate);
      this.m_onlineMode.AddItem(2L, MyCommonTexts.WorldSettings_OnlineModeFriends);
      this.m_onlineMode.AddItem(1L, MyCommonTexts.WorldSettings_OnlineModePublic);
      this.m_onlineMode.SelectItemByIndex(0);
      this.m_onlineMode.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_onlineMode_ItemSelected);
      this.m_onlineMode.Enabled = false;
      this.m_maxPlayers = MyMultiplayerLobby.MAX_PLAYERS;
      Vector2? position = new Vector2?(Vector2.Zero);
      float x = this.m_onlineMode.Size.X;
      double num6 = (double) Math.Max(this.m_maxPlayers, 3);
      double num7 = (double) x;
      float? defaultValue = new float?();
      Vector4? color = new Vector4?();
      string labelText = new StringBuilder("{0}").ToString();
      this.m_maxPlayersSlider = new MyGuiControlSlider(position, 2f, (float) num6, (float) num7, defaultValue, color, labelText, 0, labelSpaceWidth: 0.028f, intValue: true, showLabel: true);
      this.m_maxPlayersSlider.Value = (float) this.m_maxPlayers;
      this.m_maxPlayersLabel = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.MaxPlayers));
      this.m_maxPlayersSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxPlayer));
      MyGuiControlMultilineText controlMultilineText1 = new MyGuiControlMultilineText(new Vector2?(), new Vector2?(), new Vector4?(), "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, (StringBuilder) null, true, true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, new int?(), false, false, (MyGuiCompositeTexture) null, new MyGuiBorderThickness?());
      controlMultilineText1.Name = "BriefingMultilineText";
      controlMultilineText1.Position = new Vector2(-0.009f, -0.115f);
      controlMultilineText1.Size = new Vector2(0.419f, 0.412f);
      controlMultilineText1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText1.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText1.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_descriptionMultilineText = controlMultilineText1;
      MyGuiControlCompositePanel controlCompositePanel1 = new MyGuiControlCompositePanel();
      controlCompositePanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      this.m_descriptionPanel = (MyGuiControlPanel) controlCompositePanel1;
      this.m_tableLayout.Add((MyGuiControlBase) this.m_nameLabel, MyAlignH.Left, MyAlignV.Center, 0, 0);
      this.m_tableLayout.Add((MyGuiControlBase) this.m_authorLabel, MyAlignH.Left, MyAlignV.Center, 1, 0);
      this.m_tableLayout.Add((MyGuiControlBase) this.m_onlineModeLabel, MyAlignH.Left, MyAlignV.Center, 2, 0);
      this.m_tableLayout.Add((MyGuiControlBase) this.m_maxPlayersLabel, MyAlignH.Left, MyAlignV.Center, 3, 0);
      this.m_tableLayout.Add((MyGuiControlBase) this.m_ratingLabel, MyAlignH.Left, MyAlignV.Center, 4, 0);
      this.m_nameLabel.PositionX -= 3f / 1000f;
      this.m_authorLabel.PositionX -= 3f / 1000f;
      this.m_onlineModeLabel.PositionX -= 3f / 1000f;
      this.m_maxPlayersLabel.PositionX -= 3f / 1000f;
      this.m_ratingLabel.PositionX -= 3f / 1000f;
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_nameText, MyAlignH.Left, MyAlignV.Center, 0, 1);
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_authorText, MyAlignH.Left, MyAlignV.Center, 1, 1);
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_onlineMode, MyAlignH.Left, MyAlignV.Center, 2, 1);
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_maxPlayersSlider, MyAlignH.Left, MyAlignV.Center, 3, 1);
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_ratingDisplay, MyAlignH.Left, MyAlignV.Center, 4, 1);
      this.m_tableLayout.Add((MyGuiControlBase) this.m_buttonRateUp, MyAlignH.Right, MyAlignV.Center, 4, 1);
      this.m_tableLayout.Add((MyGuiControlBase) this.m_iconRateUp, MyAlignH.Center, MyAlignV.Center, 4, 1);
      this.m_tableLayout.Add((MyGuiControlBase) this.m_buttonRateDown, MyAlignH.Right, MyAlignV.Center, 4, 1);
      this.m_tableLayout.Add((MyGuiControlBase) this.m_iconRateDown, MyAlignH.Center, MyAlignV.Center, 4, 1);
      this.m_nameText.PositionX -= 1f / 1000f;
      MyGuiControlLabel nameText = this.m_nameText;
      nameText.Size = nameText.Size + new Vector2(1f / 500f, 0.0f);
      this.m_onlineMode.PositionX -= 1f / 500f;
      this.m_onlineMode.PositionY -= 0.005f;
      this.m_maxPlayersSlider.PositionX -= 3f / 1000f;
      this.m_buttonRateUp.PositionX -= 0.05f;
      this.m_buttonRateDown.PositionX -= 0.007f;
      this.m_iconRateUp.Position = this.m_buttonRateUp.Position + new Vector2(-0.0015f, -1f / 500f) - new Vector2(this.m_buttonRateUp.Size.X / 2f, 0.0f);
      this.m_iconRateDown.Position = this.m_buttonRateDown.Position + new Vector2(-0.0015f, -1f / 500f) - new Vector2(this.m_buttonRateDown.Size.X / 2f, 0.0f);
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_descriptionPanel, MyAlignH.Left, MyAlignV.Top, 5, 0, colSpan: 2);
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_descriptionMultilineText, MyAlignH.Left, MyAlignV.Top, 5, 0, colSpan: 2);
      this.m_descriptionMultilineText.PositionY += 0.012f;
      float num8 = 0.01f;
      this.m_descriptionPanel.Position = new Vector2(this.m_descriptionPanel.PositionX - num8, (float) ((double) this.m_descriptionPanel.PositionY - (double) num8 + 0.0120000001043081));
      this.m_descriptionPanel.Size = new Vector2(this.m_descriptionPanel.Size.X, (float) ((double) this.m_descriptionPanel.Size.Y + (double) MyGuiConstants.BACK_BUTTON_SIZE.Y + 0.0149999996647239));
      this.m_descriptionMultilineText.Size = new Vector2(this.m_descriptionMultilineText.Size.X, this.m_descriptionMultilineText.Size.Y + MyGuiConstants.BACK_BUTTON_SIZE.Y);
      Vector2 vector2_2 = this.m_size.Value / 2f;
      vector2_2.X -= this.MARGIN_RIGHT + 0.004f;
      vector2_2.Y -= this.MARGIN_BOTTOM + 0.004f;
      Vector2 backButtonSize = MyGuiConstants.BACK_BUTTON_SIZE;
      Vector2 genericButtonSpacing1 = MyGuiConstants.GENERIC_BUTTON_SPACING;
      Vector2 genericButtonSpacing2 = MyGuiConstants.GENERIC_BUTTON_SPACING;
      this.m_okButton = new MyGuiControlButton(new Vector2?(vector2_2), size: new Vector2?(backButtonSize), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.Start), onButtonClick: new Action<MyGuiControlButton>(this.OnOkButtonClicked));
      this.m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Start));
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      this.m_workshopButton = new MyGuiControlButton(new Vector2?(this.m_okButton.Position - new Vector2(this.m_okButton.Size.X + 0.01f, 0.0f)), size: new Vector2?(backButtonSize), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.ScreenLoadSubscribedWorldOpenInWorkshop), onButtonClick: new Action<MyGuiControlButton>(this.OnOpenInWorkshopClicked));
      this.m_workshopButton.SetToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorkshopOpenInWorkshop), (object) MyGameService.Service.ServiceName));
      this.m_workshopButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_workshopButton);
      this.CloseButtonEnabled = true;
      MyGuiControlCompositePanel controlCompositePanel2 = new MyGuiControlCompositePanel();
      controlCompositePanel2.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      this.m_noSubscribedItemsPanel = (MyGuiControlPanel) controlCompositePanel2;
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_noSubscribedItemsPanel, MyAlignH.Left, MyAlignV.Top, 0, 0, 6, 2);
      this.m_noSubscribedItemsPanel.Position = new Vector2(this.m_descriptionPanel.Position.X, this.m_worldList.Position.Y);
      this.m_noSubscribedItemsPanel.Size = new Vector2(this.m_descriptionPanel.Size.X, this.m_worldList.Size.Y - 1.63f * MyGuiConstants.BACK_BUTTON_SIZE.Y);
      MyGuiControlMultilineText controlMultilineText2 = new MyGuiControlMultilineText(textScale: 0.7394595f);
      controlMultilineText2.Size = new Vector2(this.m_descriptionMultilineText.Size.X, this.m_descriptionMultilineText.Size.Y * 2f);
      controlMultilineText2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText2.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlMultilineText2.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_noSubscribedItemsText = controlMultilineText2;
      this.m_tableLayout.AddWithSize((MyGuiControlBase) this.m_noSubscribedItemsText, MyAlignH.Left, MyAlignV.Top, 0, 0, 6, 2);
      this.m_noSubscribedItemsText.Position = this.m_noSubscribedItemsPanel.Position + new Vector2(num8);
      this.m_noSubscribedItemsText.Size = this.m_noSubscribedItemsPanel.Size - new Vector2(2f * num8);
      this.m_noSubscribedItemsText.Clear();
      this.m_noSubscribedItemsText.AppendText(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_NoWorkshopWorld), "Blue", this.m_noSubscribedItemsText.TextScale, Vector4.One);
      this.m_noSubscribedItemsText.ScrollbarOffsetV = 1f;
      this.m_noSubscribedItemsPanel.Visible = false;
      this.m_noSubscribedItemsText.Visible = false;
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_nameLabel.Position.X, this.m_okButton.Position.Y - backButtonSize.Y / 2f)));
      myGuiControlLabel7.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel7);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.NewGameWorkshop_Help_Screen);
    }

    private void OnLinkClicked(MyGuiControlBase sender, string url) => MyGuiSandbox.OpenUrlWithFallback(url, "Space Engineers Steam Workshop");

    private void OnOpenInWorkshopClicked(MyGuiControlButton button)
    {
      if (this.m_selectedWorld == null)
        return;
      MyGuiSandbox.OpenUrlWithFallback(this.m_selectedWorkshopItem.GetItemUrl(), this.m_selectedWorkshopItem.ServiceName + " Workshop");
    }

    private void OnRateUpClicked(MyGuiControlButton button) => this.UpdateRateState(true);

    private void OnRateDownClicked(MyGuiControlButton button) => this.UpdateRateState(false);

    private void UpdateRateState(bool positive)
    {
      if (this.m_selectedWorkshopItem == null)
        return;
      this.m_selectedWorkshopItem.Rate(positive);
      this.m_selectedWorkshopItem.ChangeRatingValue(positive);
      this.m_buttonRateUp.Checked = positive;
      this.m_buttonRateDown.Checked = !positive;
    }

    private void m_onlineMode_ItemSelected() => this.m_maxPlayersSlider.Enabled = this.m_onlineMode.Enabled && this.m_onlineMode.GetSelectedIndex() > 0 && this.m_maxPlayers > 2;

    private void OnRefreshClicked(MyGuiControlButton button)
    {
      this.ClearItems();
      this.RefreshList();
    }

    private void OnSortingClicked(MyGuiControlButton button)
    {
      switch (this.m_sort)
      {
        case MyGuiBlueprintScreen_Reworked.SortOption.None:
          this.m_sort = MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical;
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
          this.m_sort = MyGuiBlueprintScreen_Reworked.SortOption.CreationDate;
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
          this.m_sort = MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate;
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
          this.m_sort = MyGuiBlueprintScreen_Reworked.SortOption.None;
          break;
      }
      this.SetIconForSorting();
      this.ClearItems();
      this.OnSuccess();
    }

    private void OnOpenWorkshopClicked(MyGuiControlButton button) => MyWorkshop.OpenWorkshopBrowser(MySteamConstants.TAG_WORLDS);

    private void OnToggleThumbnailsClicked(MyGuiControlButton button)
    {
      this.m_showThumbnails = !this.m_showThumbnails;
      this.SetIconForHideThumbnails();
      foreach (MyGuiControlBase control in this.m_worldList.Controls)
      {
        if (control is MyGuiControlContentButton controlContentButton)
          controlContentButton.SetPreviewVisibility(this.m_showThumbnails);
      }
      this.m_worldList.Recalculate();
    }

    private void OnSearchTextChange(string message)
    {
      this.ApplyFiltering();
      this.TrySelectFirstItem();
    }

    private void OnOkButtonClicked(MyGuiControlButton myGuiControlButton) => this.StartSelectedWorld();

    private void WorldDoubleClick(MyGuiControlRadioButton obj) => this.StartSelectedWorld();

    private void StartSelectedWorld()
    {
      if (this.m_selectedWorld == null || this.m_worldTypesGroup.SelectedButton == null || this.m_worldTypesGroup.SelectedButton.UserData == null)
        return;
      MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem> world = (MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>) this.m_worldTypesGroup.SelectedButton.UserData;
      if (this.m_onlineMode.GetSelectedIndex() != 0)
        MyGameService.Service.RequestPermissions(Permissions.Multiplayer, true, (Action<PermissionResult>) (granted =>
        {
          switch (granted)
          {
            case PermissionResult.Granted:
              MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<PermissionResult>) (ugcGranted =>
              {
                if (ugcGranted == PermissionResult.Granted)
                {
                  this.DownloadSelectedWorld(world);
                }
                else
                {
                  if (ugcGranted != PermissionResult.Error)
                    return;
                  MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info);
                }
              }));
              break;
            case PermissionResult.Error:
              MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, type: MyMessageBoxStyleEnum.Info);
              break;
          }
        }));
      else
        this.DownloadSelectedWorld(world);
    }

    private void DownloadSelectedWorld(
      MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem> world)
    {
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, new MyStringId?(), new Func<IMyAsyncResult>(this.beginActionLoadSaves), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(this.endActionLoadSaves), (object) world));
    }

    private IMyAsyncResult beginActionLoadSaves() => (IMyAsyncResult) new MyLoadWorldInfoListResult();

    private void endActionLoadSaves(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
    {
      screen.CloseScreen();
      MyWorkshopItem world = ((MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>) screen.UserData).Item2;
      string sessionSavesPath = MyLocalCache.GetSessionSavesPath(MyUtils.StripInvalidChars(world.Title), false, false);
      bool flag;
      if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
      {
        List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(MyCloudHelper.LocalToCloudWorldPath(sessionSavesPath));
        // ISSUE: explicit non-virtual call
        flag = cloudFiles != null && __nonvirtual (cloudFiles.Count) > 0;
      }
      else
        flag = Directory.Exists(sessionSavesPath);
      if (flag)
        this.OverwriteWorldDialog(world);
      else
        MyWorkshop.CreateWorldInstanceAsync(world, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo(), false, (Action<bool, string>) ((success, sessionPath) =>
        {
          if (success)
          {
            this.OnSuccessStart(sessionPath, world);
          }
          else
          {
            StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded), messageCaption: messageCaption));
          }
        }));
    }

    private void OverwriteWorldDialog(MyWorkshopItem world) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextWorldExistsDownloadOverwrite), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
    {
      if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      MyWorkshop.CreateWorldInstanceAsync(world, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo(), true, (Action<bool, string>) ((success, sessionPath) =>
      {
        if (success)
        {
          this.OnSuccessStart(sessionPath, world);
        }
        else
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded), messageCaption: messageCaption));
        }
      }));
    }))));

    private void OnSuccessStart(string sessionPath, MyWorkshopItem workshopItem) => MySessionLoader.LoadSingleplayerSession(sessionPath, (Action) (() =>
    {
      MySessionLoader.LastLoadedSessionWorkshopItem = workshopItem;
      MySession.Static.OnReady += (Action) (() => MySessionLoader.LastLoadedSessionWorkshopItem = (MyWorkshopItem) null);
      MySession.Static.CurrentPath = sessionPath;
      MyAsyncSaving.DelayedSaveAfterLoad(sessionPath);
    }));

    private void OnCancelButtonClick(MyGuiControlButton myGuiControlButton) => this.CloseScreen();

    private void AddWorldButton(MyObjectBuilder_Checkpoint world, MyWorkshopItem workshopItem)
    {
      string sessionName = world.SessionName;
      MyLocalizationContext localizationContext = MyLocalization.Static[world.SessionName];
      if (localizationContext != null)
      {
        StringBuilder stringBuilder = localizationContext["Name"];
        if (stringBuilder != null)
          sessionName = stringBuilder.ToString();
      }
      MyGuiControlContentButton controlContentButton = new MyGuiControlContentButton(sessionName, this.GetImagePath(workshopItem));
      controlContentButton.UserData = (object) new MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>(world, workshopItem);
      controlContentButton.Key = this.m_worldTypesGroup.Count;
      MyGuiControlContentButton control = controlContentButton;
      control.FocusHighlightAlsoSelects = true;
      control.SetModType(MyBlueprintTypeEnum.WORKSHOP, workshopItem.ServiceName);
      control.SetPreviewVisibility(this.m_showThumbnails);
      control.SetTooltip(sessionName);
      this.m_worldTypesGroup.Add((MyGuiControlRadioButton) control);
      this.m_worldList.Controls.Add((MyGuiControlBase) control);
    }

    private string GetImagePath(MyWorkshopItem workshopItem) => Path.Combine(workshopItem.Folder + "\\" + MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);

    private void SortItems(List<MyWorkshopItem> list)
    {
      MyGuiScreenNewWorkshopGame.MyWorkshopItemComparer workshopItemComparer = (MyGuiScreenNewWorkshopGame.MyWorkshopItemComparer) null;
      switch (this.m_sort)
      {
        case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
          workshopItemComparer = new MyGuiScreenNewWorkshopGame.MyWorkshopItemComparer((Func<MyWorkshopItem, MyWorkshopItem, int>) ((x, y) => x.Title.CompareTo(y.Title)));
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
          workshopItemComparer = new MyGuiScreenNewWorkshopGame.MyWorkshopItemComparer((Func<MyWorkshopItem, MyWorkshopItem, int>) ((x, y) => x.TimeCreated.CompareTo(y.TimeCreated)));
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
          workshopItemComparer = new MyGuiScreenNewWorkshopGame.MyWorkshopItemComparer((Func<MyWorkshopItem, MyWorkshopItem, int>) ((x, y) => x.TimeUpdated.CompareTo(y.TimeUpdated)));
          break;
      }
      if (workshopItemComparer == null)
        return;
      list.Sort((IComparer<MyWorkshopItem>) workshopItemComparer);
    }

    private void ApplyFiltering()
    {
      bool flag1 = false;
      string[] strArray = new string[0];
      if (this.m_searchBox != null)
      {
        flag1 = this.m_searchBox.SearchText != "";
        strArray = this.m_searchBox.SearchText.Split(' ');
      }
      foreach (MyGuiControlBase control in this.m_worldList.Controls)
      {
        if (control is MyGuiControlContentButton controlContentButton)
        {
          bool flag2 = true;
          if (flag1)
          {
            string lower = controlContentButton.Title.ToLower();
            foreach (string str in strArray)
            {
              if (!lower.Contains(str.ToLower()))
              {
                flag2 = false;
                break;
              }
            }
          }
          control.Visible = flag2;
        }
      }
      this.m_worldList.SetScrollBarPage();
    }

    private void AddSeparator(string sectionName)
    {
      MyGuiControlCompositePanel controlCompositePanel1 = new MyGuiControlCompositePanel();
      controlCompositePanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
      controlCompositePanel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlCompositePanel1.Position = Vector2.Zero;
      MyGuiControlCompositePanel controlCompositePanel2 = controlCompositePanel1;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel();
      myGuiControlLabel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel1.Text = sectionName;
      myGuiControlLabel1.Font = "Blue";
      myGuiControlLabel1.PositionX = 0.005f;
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      float num = 3f / 1000f;
      Color themedGuiLineColor = MyGuiConstants.THEMED_GUI_LINE_COLOR;
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage(textures: new string[1]
      {
        "Textures\\GUI\\FogSmall3.dds"
      });
      myGuiControlImage1.Size = new Vector2(myGuiControlLabel2.Size.X + num * 10f, 0.007f);
      myGuiControlImage1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlImage1.ColorMask = themedGuiLineColor.ToVector4();
      myGuiControlImage1.Position = new Vector2(-num, myGuiControlLabel2.Size.Y);
      MyGuiControlImage myGuiControlImage2 = myGuiControlImage1;
      MyGuiControlParent guiControlParent1 = new MyGuiControlParent();
      guiControlParent1.Size = new Vector2(this.m_worldList.Size.X, myGuiControlLabel2.Size.Y);
      guiControlParent1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      guiControlParent1.Position = Vector2.Zero;
      MyGuiControlParent guiControlParent2 = guiControlParent1;
      controlCompositePanel2.Size = guiControlParent2.Size + new Vector2(-0.035f, 0.01f);
      MyGuiControlCompositePanel controlCompositePanel3 = controlCompositePanel2;
      controlCompositePanel3.Position = controlCompositePanel3.Position - (guiControlParent2.Size / 2f - new Vector2(-0.01f, 0.0f));
      MyGuiControlLabel myGuiControlLabel3 = myGuiControlLabel2;
      myGuiControlLabel3.Position = myGuiControlLabel3.Position - guiControlParent2.Size / 2f;
      MyGuiControlImage myGuiControlImage3 = myGuiControlImage2;
      myGuiControlImage3.Position = myGuiControlImage3.Position - guiControlParent2.Size / 2f;
      guiControlParent2.Controls.Add((MyGuiControlBase) controlCompositePanel2);
      guiControlParent2.Controls.Add((MyGuiControlBase) myGuiControlImage2);
      guiControlParent2.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.m_worldList.Controls.Add((MyGuiControlBase) guiControlParent2);
    }

    private IMyAsyncResult beginAction() => (IMyAsyncResult) new MyGuiScreenNewWorkshopGame.LoadListResult();

    private void endAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
    {
      MyGuiScreenNewWorkshopGame.LoadListResult loadListResult = (MyGuiScreenNewWorkshopGame.LoadListResult) result;
      this.SubscribedWorlds = loadListResult.SubscribedWorlds;
      this.UpdateWorkshopError(loadListResult.Result.Item1, loadListResult.Result.Item2);
      this.OnSuccess();
      screen.CloseScreen();
      bool flag = this.SubscribedWorlds != null && (uint) this.SubscribedWorlds.Count > 0U;
      this.m_noSubscribedItemsPanel.Visible = !flag;
      this.m_noSubscribedItemsText.Visible = !flag;
      this.m_nameLabel.Visible = flag;
      this.m_nameText.Visible = flag;
      this.m_authorLabel.Visible = flag;
      this.m_authorText.Visible = flag;
      this.m_ratingDisplay.Visible = flag;
      this.m_ratingLabel.Visible = flag;
      this.m_buttonRateUp.Visible = flag;
      this.m_buttonRateDown.Visible = flag;
      this.m_iconRateUp.Visible = flag;
      this.m_iconRateDown.Visible = flag;
      this.m_descriptionMultilineText.Visible = flag;
      this.m_onlineModeLabel.Visible = flag;
      this.m_onlineMode.Visible = flag;
      this.m_maxPlayersLabel.Visible = flag;
      this.m_maxPlayersSlider.Visible = flag;
      this.m_descriptionPanel.Visible = flag;
    }

    private void PreloadItemTextures()
    {
      List<string> stringList = new List<string>();
      foreach (MyWorkshopItem subscribedWorld in this.SubscribedWorlds)
      {
        string imagePath = this.GetImagePath(subscribedWorld);
        if (MyFileSystem.FileExists(imagePath))
          stringList.Add(imagePath);
      }
      MyRenderProxy.PreloadTextures((IEnumerable<string>) stringList, TextureType.GUIWithoutPremultiplyAlpha);
    }

    private void ClearItems()
    {
      this.m_selectedWorld = (MyObjectBuilder_Checkpoint) null;
      this.m_worldList.Clear();
      this.m_worldTypesGroup.Clear();
    }

    private void FillItems()
    {
      foreach (MyWorkshopItem subscribedWorld in this.SubscribedWorlds)
      {
        if (this.m_groupSelection <= 0 || !(subscribedWorld.ServiceName != MyGameService.WorkshopService.GetAggregates()[this.m_groupSelection - 1].ServiceName))
          this.AddWorldButton(new MyObjectBuilder_Checkpoint()
          {
            SessionName = subscribedWorld.Title,
            Description = subscribedWorld.Description,
            WorkshopId = new ulong?(subscribedWorld.Id),
            WorkshopServiceName = subscribedWorld.ServiceName
          }, subscribedWorld);
      }
    }

    private void TrySelectFirstItem()
    {
      if (this.m_worldTypesGroup.SelectedIndex.HasValue || this.m_worldList.Controls.Count <= 0)
        return;
      this.m_worldTypesGroup.SelectByIndex(0);
    }

    private void OnSuccess()
    {
      if (this.SubscribedWorlds != null)
      {
        this.SortItems(this.SubscribedWorlds);
        this.PreloadItemTextures();
        this.FillItems();
      }
      this.TrySelectFirstItem();
    }

    private void RefreshList()
    {
      this.ClearItems();
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, new MyStringId?(), new Func<IMyAsyncResult>(this.beginAction), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(this.endAction)));
    }

    private void UpdateWorkshopError(MyGameServiceCallResult result, string serviceName)
    {
      if (result != MyGameServiceCallResult.OK)
        this.SetWorkshopErrorText(MyWorkshop.GetWorkshopErrorText(result, serviceName, this.m_workshopPermitted));
      else
        this.SetWorkshopErrorText(visible: false);
    }

    public override bool Update(bool hasFocus)
    {
      int num = base.Update(hasFocus) ? 1 : 0;
      if (!hasFocus)
        return num != 0;
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_workshopButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      return num != 0;
    }

    private void CheckUGCServices()
    {
      string str = "";
      foreach (IMyUGCService aggregate in MyGameService.WorkshopService.GetAggregates())
      {
        if (!aggregate.IsConsentGiven)
          str = aggregate.ServiceName;
      }
      if (str != "")
        this.SetWorkshopErrorText(str + MyTexts.GetString(MySpaceTexts.UGC_ServiceNotAvailable_NoConsent));
      else
        this.SetWorkshopErrorText(visible: false, skipUGCCheck: true);
    }

    public override void OnScreenOrderChanged(MyGuiScreenBase oldLast, MyGuiScreenBase newLast)
    {
      base.OnScreenOrderChanged(oldLast, newLast);
      this.CheckUGCServices();
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (receivedFocusInThisUpdate)
        return;
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.StartSelectedWorld();
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        this.OnRefreshClicked((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.VIEW))
        this.OnOpenInWorkshopClicked((MyGuiControlButton) null);
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU))
        return;
      this.OnOpenWorkshopClicked((MyGuiControlButton) null);
    }

    private class MyWorkshopItemComparer : IComparer<MyWorkshopItem>
    {
      private Func<MyWorkshopItem, MyWorkshopItem, int> comparator;

      public MyWorkshopItemComparer(Func<MyWorkshopItem, MyWorkshopItem, int> comp) => this.comparator = comp;

      public int Compare(MyWorkshopItem x, MyWorkshopItem y) => this.comparator != null ? this.comparator(x, y) : 0;
    }

    private class LoadListResult : IMyAsyncResult
    {
      public (MyGameServiceCallResult, string) Result;
      public List<MyWorkshopItem> SubscribedWorlds;

      public bool IsCompleted => this.Task.IsComplete;

      public Task Task { get; private set; }

      public LoadListResult() => this.Task = Parallel.Start((Action) (() => this.LoadListAsync(out this.SubscribedWorlds)));

      private void LoadListAsync(out List<MyWorkshopItem> list)
      {
        if (!MyGameService.IsActive || !MyGameService.IsOnline)
        {
          this.Result = (MyGameServiceCallResult.NoUser, MyGameService.GetDefaultUGC().ServiceName);
          list = (List<MyWorkshopItem>) null;
        }
        else
        {
          List<MyWorkshopItem> results1 = new List<MyWorkshopItem>();
          this.Result = MyWorkshop.GetSubscribedWorldsBlocking(results1);
          list = results1;
          List<MyWorkshopItem> results2 = new List<MyWorkshopItem>();
          (MyGameServiceCallResult, string) scenariosBlocking = MyWorkshop.GetSubscribedScenariosBlocking(results2);
          if (results2.Count > 0)
            list.InsertRange(list.Count, (IEnumerable<MyWorkshopItem>) results2);
          this.SubscribedWorlds = list;
          MyWorkshop.TryUpdateWorldsBlocking(this.SubscribedWorlds, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo());
          if (this.Result.Item1 != MyGameServiceCallResult.OK)
            return;
          this.Result = scenariosBlocking;
        }
      }
    }

    private class WorldDataLoaderInfo : WorkData
    {
      public MyObjectBuilder_Checkpoint World;
      public MyWorkshopItem WorkshopItem;
      public string Author = "";
      public bool IsMultiplayer;
      public bool IsSkipped;
    }
  }
}
