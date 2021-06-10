// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenJoinGame
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ObjectBuilders.Gui;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenJoinGame : MyGuiScreenBase
  {
    private MyGuiControlTabControl m_joinGameTabs;
    private MyGuiControlContextMenu m_contextMenu;
    private readonly StringBuilder m_textCache = new StringBuilder();
    private readonly StringBuilder m_gameTypeText = new StringBuilder();
    private readonly StringBuilder m_gameTypeToolTip = new StringBuilder();
    private MyGuiControlTable m_gamesTable;
    private MyGuiControlButton m_joinButton;
    private MyGuiControlButton m_refreshButton;
    private MyGuiControlButton m_detailsButton;
    private MyGuiControlButton m_directConnectButton;
    private MyGuiControlSearchBox m_searchBox;
    private MyGuiControlButton m_advancedSearchButton;
    private MyGuiControlRotatingWheel m_loadingWheel;
    private readonly string m_dataHash;
    private bool m_searchChanged;
    private DateTime m_searchLastChanged = DateTime.Now;
    private Action m_searchChangedFunc;
    private MyRankedServers m_rankedServers;
    public MyGuiControlTabPage m_selectedPage;
    private int m_remainingTimeUpdateFrame;
    public MyServerFilterOptions FilterOptions;
    public bool EnableAdvancedSearch;
    public bool refresh_favorites;
    private MyGuiControlImageButton m_bannerImage;
    private Action m_stopServerRequest;
    private readonly bool m_enableDedicatedServers;
    private readonly List<MyGuiControlButton> m_networkingButtons = new List<MyGuiControlButton>();
    private readonly List<MyGuiControlImage> m_networkingIcons = new List<MyGuiControlImage>();
    private MyServerDiscoveryAggregator m_serverDiscoveryAggregator;
    private bool m_networkingService0Selected;
    private bool m_networkingButtonsVisible;
    private IMyServerDiscovery m_currentServerDiscovery;
    private IMyServerDiscovery m_serverDiscovery;
    private MyGuiControlTabPage m_serversPage;
    private readonly HashSet<MyCachedServerItem> m_dedicatedServers = new HashSet<MyCachedServerItem>();
    private MyGuiScreenJoinGame.RefreshStateEnum m_nextState;
    private bool m_refreshPaused;
    private bool m_dedicatedResponding;
    private bool m_lastVersionCheck;
    private readonly List<IMyLobby> m_lobbies = new List<IMyLobby>();
    private MyGuiControlTabPage m_lobbyPage;
    private MyGuiControlTabPage m_favoritesPage;
    private HashSet<MyCachedServerItem> m_favoriteServers = new HashSet<MyCachedServerItem>();
    private bool m_favoritesResponding;
    private MyGuiControlTabPage m_historyPage;
    private HashSet<MyCachedServerItem> m_historyServers = new HashSet<MyCachedServerItem>();
    private bool m_historyResponding;
    private MyGuiControlTabPage m_LANPage;
    private HashSet<MyCachedServerItem> m_lanServers = new HashSet<MyCachedServerItem>();
    private bool m_lanResponding;
    private MyGuiControlTabPage m_friendsPage;
    private HashSet<ulong> m_friendIds;
    private HashSet<string> m_friendNames;

    private event Action<MyGuiControlButton> RefreshRequest;

    public bool IsMultipleNetworking => this.m_serverDiscoveryAggregator != null && this.m_serverDiscoveryAggregator.GetAggregates().Count > 1;

    public MyGuiScreenJoinGame(bool enableDedicatedServers)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(1f, 0.9f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_serverDiscoveryAggregator = MyGameService.ServerDiscovery as MyServerDiscoveryAggregator;
      this.m_serverDiscovery = this.m_serverDiscoveryAggregator?.GetAggregates()[0] ?? MyGameService.ServerDiscovery;
      this.m_networkingService0Selected = true;
      this.EnabledBackgroundFade = true;
      this.m_enableDedicatedServers = enableDedicatedServers;
      this.m_dataHash = MyDataIntegrityChecker.GetHashBase64();
      MyObjectBuilder_ServerFilterOptions serverSearchSettings = MySandboxGame.Config.ServerSearchSettings;
      this.FilterOptions = serverSearchSettings == null ? (MyServerFilterOptions) new MySpaceServerFilterOptions() : (MyServerFilterOptions) new MySpaceServerFilterOptions(serverSearchSettings);
      this.RecreateControls(true);
      this.m_selectedPage = (MyGuiControlTabPage) this.m_joinGameTabs.Controls.GetControlByName("PageFavoritesPanel");
      this.joinGameTabs_OnPageChanged();
      MyRankedServers.LoadAsync(MyPerGameSettings.RankedServersUrl, new Action<MyRankedServers>(this.OnRankedServersLoaded));
    }

    public bool SupportsPing { get; private set; }

    public bool SupportsGroups { get; private set; }

    private void OnRankedServersLoaded(MyRankedServers rankedServers)
    {
      if (!this.IsOpened)
        return;
      this.m_rankedServers = rankedServers;
      this.m_searchChangedFunc();
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenJoinGame);

    protected override void OnClosed()
    {
      if (this.m_currentServerDiscovery != null)
      {
        this.m_currentServerDiscovery.OnDedicatedServerListResponded -= new EventHandler<int>(this.OnFriendsServerListResponded);
        this.m_currentServerDiscovery.OnDedicatedServersCompleteResponse -= new EventHandler<MyMatchMakingServerResponse>(this.OnFriendsServersCompleteResponse);
      }
      this.CloseDedicatedServerListRequest();
      this.CloseFavoritesRequest();
      this.CloseHistoryRequest();
      this.CloseLANRequest();
      MySandboxGame.Config.ServerSearchSettings = this.FilterOptions.GetObjectBuilder();
      MySandboxGame.Config.Save();
      base.OnClosed();
    }

    private int PlayerCountComparison(MyGuiControlTable.Cell b, MyGuiControlTable.Cell a)
    {
      List<StringBuilder> stringBuilderList1 = a.Text.Split('/');
      List<StringBuilder> stringBuilderList2 = b.Text.Split('/');
      int result1 = 0;
      int result2 = 0;
      int result3 = 0;
      int result4 = 0;
      bool flag1 = true;
      bool flag2 = stringBuilderList1.Count >= 2 && stringBuilderList2.Count >= 2 && flag1 & int.TryParse(stringBuilderList1[0].ToString(), out result1) & int.TryParse(stringBuilderList2[0].ToString(), out result2) & int.TryParse(stringBuilderList1[1].ToString(), out result3) & int.TryParse(stringBuilderList2[1].ToString(), out result4);
      if (result1 != result2 && flag2)
        return result1.CompareTo(result2);
      if (result3 != result4 && flag2)
        return result3.CompareTo(result4);
      if (!(a.Row.UserData is IMyMultiplayerGame userData))
        return 0;
      ulong gameId1 = userData.GameID;
      if (!(b.Row.UserData is IMyMultiplayerGame userData))
        return 0;
      ulong gameId2 = userData.GameID;
      return gameId1.CompareTo(gameId2);
    }

    private int TextComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
    {
      int ignoreCase = a.Text.CompareToIgnoreCase(b.Text);
      if (ignoreCase != 0)
        return ignoreCase;
      if (!(a.Row.UserData is IMyMultiplayerGame userData))
        return 0;
      ulong gameId1 = userData.GameID;
      if (!(b.Row.UserData is IMyMultiplayerGame userData))
        return 0;
      ulong gameId2 = userData.GameID;
      return gameId1.CompareTo(gameId2);
    }

    private int PingComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
    {
      int result1;
      if (!int.TryParse(a.Text.ToString(), out result1))
        result1 = -1;
      int result2;
      if (!int.TryParse(b.Text.ToString(), out result2))
        result2 = -1;
      if (result1 != result2)
        return result1.CompareTo(result2);
      if (!(a.Row.UserData is IMyMultiplayerGame userData))
        return 0;
      ulong gameId1 = userData.GameID;
      if (!(b.Row.UserData is IMyMultiplayerGame userData))
        return 0;
      ulong gameId2 = userData.GameID;
      return gameId1.CompareTo(gameId2);
    }

    private int ModsComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
    {
      int result1 = 0;
      int.TryParse(a.Text.ToString(), out result1);
      int result2 = 0;
      int.TryParse(b.Text.ToString(), out result2);
      if (result1 != result2)
        return result1.CompareTo(result2);
      if (!(a.Row.UserData is IMyMultiplayerGame userData))
        return 0;
      ulong gameId1 = userData.GameID;
      if (!(b.Row.UserData is IMyMultiplayerGame userData))
        return 0;
      ulong gameId2 = userData.GameID;
      return gameId1.CompareTo(gameId2);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      MyObjectBuilder_GuiScreen objectBuilder;
      MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_GuiScreen>(Path.Combine(MyFileSystem.ContentPath, MyGuiScreenBase.MakeScreenFilepath("JoinScreen")), out objectBuilder);
      this.Init(objectBuilder);
      this.m_joinGameTabs = this.Controls.GetControlByName("JoinGameTabs") as MyGuiControlTabControl;
      this.m_joinGameTabs.PositionY -= 0.018f;
      this.m_joinGameTabs.TabButtonScale = 0.86f;
      this.m_joinGameTabs.OnPageChanged += new Action(this.joinGameTabs_OnPageChanged);
      this.joinGameTabs_OnPageChanged();
      this.AddCaption(MyCommonTexts.ScreenMenuButtonJoinGame, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      this.CloseButtonEnabled = true;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_detailsButton.Position.X - MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui.X / 2f, this.m_detailsButton.Position.Y)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      if ((double) MySandboxGame.ScreenSize.X / (double) MySandboxGame.ScreenSize.Y == 1.25)
        this.SetCloseButtonOffset_5_to_4();
      else
        this.SetDefaultCloseButtonOffset();
      this.CreateGTXGamingBanner();
    }

    private void CreateGTXGamingBanner()
    {
      MyGuiControlImageButton.StyleDefinition style = new MyGuiControlImageButton.StyleDefinition()
      {
        Highlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = new MyGuiCompositeTexture(MyPerGameSettings.JoinScreenBannerTextureHighlight)
        },
        ActiveHighlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = new MyGuiCompositeTexture(MyPerGameSettings.JoinScreenBannerTexture)
        },
        Normal = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = new MyGuiCompositeTexture(MyPerGameSettings.JoinScreenBannerTexture)
        }
      };
      Vector2 vector2 = new Vector2(0.2375f, 0.13f) * 0.7f;
      Vector2? size = this.Size;
      double num1 = (double) size.Value.X / 2.0 - 0.0399999991059303;
      size = this.Size;
      double num2 = -(double) size.Value.Y / 2.0 - 0.00999999977648258;
      MyGuiControlImageButton controlImageButton = new MyGuiControlImageButton(position: new Vector2?(new Vector2((float) num1, (float) num2)), size: new Vector2?(vector2), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, onButtonClick: new Action<MyGuiControlImageButton>(this.onBannerClick));
      controlImageButton.BackgroundTexture = new MyGuiCompositeTexture(MyPerGameSettings.JoinScreenBannerTexture);
      controlImageButton.CanHaveFocus = false;
      controlImageButton.UserData = (object) MyPerGameSettings.JoinScreenBannerURL;
      this.m_bannerImage = controlImageButton;
      this.m_bannerImage.ApplyStyle(style);
      this.m_bannerImage.SetToolTip(MyTexts.GetString(MySpaceTexts.JoinScreen_GTXGamingBanner));
      this.Controls.Add((MyGuiControlBase) this.m_bannerImage);
    }

    private void onBannerClick(MyGuiControlImageButton button) => MyGuiSandbox.OpenUrl((string) button.UserData, UrlOpenMode.SteamOrExternalWithConfirm);

    private void joinGameTabs_OnPageChanged()
    {
      if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.JoinGameScreen_Help_Screen);
      else
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.JoinGameScreen_Help_ScreenXbox);
      MyGuiControlTabPage controlByName1 = (MyGuiControlTabPage) this.m_joinGameTabs.Controls.GetControlByName("PageServersPanel");
      if (controlByName1 != null)
      {
        controlByName1.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Servers));
        controlByName1.Enabled = controlByName1.IsTabVisible = MyGameService.ServerDiscovery.DedicatedSupport && this.m_enableDedicatedServers;
      }
      MyGuiControlTabPage controlByName2 = (MyGuiControlTabPage) this.m_joinGameTabs.Controls.GetControlByName("PageLobbiesPanel");
      if (controlByName2 != null)
      {
        controlByName2.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Lobbies));
        controlByName2.Enabled = controlByName2.IsTabVisible = MyGameService.LobbyDiscovery.Supported;
      }
      MyGuiControlTabPage controlByName3 = (MyGuiControlTabPage) this.m_joinGameTabs.Controls.GetControlByName("PageFavoritesPanel");
      if (controlByName3 != null)
      {
        controlByName3.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Favorites));
        controlByName3.Enabled = controlByName3.IsTabVisible = MyGameService.ServerDiscovery.FavoritesSupport && this.m_enableDedicatedServers;
      }
      MyGuiControlTabPage controlByName4 = (MyGuiControlTabPage) this.m_joinGameTabs.Controls.GetControlByName("PageHistoryPanel");
      if (controlByName4 != null)
      {
        controlByName4.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_History));
        controlByName4.Enabled = controlByName4.IsTabVisible = MyGameService.ServerDiscovery.HistorySupport && this.m_enableDedicatedServers;
      }
      MyGuiControlTabPage controlByName5 = (MyGuiControlTabPage) this.m_joinGameTabs.Controls.GetControlByName("PageLANPanel");
      if (controlByName5 != null)
      {
        controlByName5.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_LAN));
        controlByName5.Enabled = controlByName5.IsTabVisible = MyGameService.ServerDiscovery.LANSupport;
      }
      MyGuiControlTabPage controlByName6 = (MyGuiControlTabPage) this.m_joinGameTabs.Controls.GetControlByName("PageFriendsPanel");
      if (controlByName6 != null)
      {
        controlByName6.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Friends));
        controlByName6.Enabled = controlByName6.IsTabVisible = MyGameService.ServerDiscovery.FriendSupport || MyGameService.LobbyDiscovery.FriendSupport;
      }
      if (this.m_selectedPage == controlByName1)
        this.CloseServersPage();
      else if (this.m_selectedPage == controlByName2)
        this.CloseLobbyPage();
      else if (this.m_selectedPage == controlByName3)
        this.CloseFavoritesPage();
      else if (this.m_selectedPage == controlByName5)
        this.CloseLANPage();
      else if (this.m_selectedPage == controlByName4)
        this.CloseHistoryPage();
      else if (this.m_selectedPage == controlByName6)
        this.CloseFriendsPage();
      for (this.m_selectedPage = this.m_joinGameTabs.GetTabSubControl(this.m_joinGameTabs.SelectedPage); !this.m_selectedPage.IsTabVisible && this.m_joinGameTabs.SelectedPage < this.m_joinGameTabs.PagesCount; this.m_selectedPage = this.m_joinGameTabs.GetTabSubControl(this.m_joinGameTabs.SelectedPage))
        ++this.m_joinGameTabs.SelectedPage;
      this.InitPageControls(this.m_selectedPage);
      if (this.m_selectedPage == controlByName1)
      {
        this.InitServersPage();
        this.EnableAdvancedSearch = true;
      }
      else if (this.m_selectedPage == controlByName2)
      {
        this.InitLobbyPage();
        this.EnableAdvancedSearch = false;
        if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
          this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.JoinGameScreen_Help_ScreenGamesTab);
        else
          this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.JoinGameScreen_Help_ScreenGamesTabXbox);
        this.UpdateGamepadHelp(this.FocusedControl);
      }
      else if (this.m_selectedPage == controlByName3)
      {
        this.InitFavoritesPage();
        this.EnableAdvancedSearch = true;
      }
      else if (this.m_selectedPage == controlByName4)
      {
        this.InitHistoryPage();
        this.EnableAdvancedSearch = true;
      }
      else if (this.m_selectedPage == controlByName5)
      {
        this.InitLANPage();
        this.EnableAdvancedSearch = true;
      }
      else if (this.m_selectedPage == controlByName6)
      {
        this.InitFriendsPage();
        this.EnableAdvancedSearch = false;
      }
      if (this.m_contextMenu != null)
      {
        this.m_contextMenu.Deactivate();
        this.m_contextMenu = (MyGuiControlContextMenu) null;
      }
      this.m_contextMenu = new MyGuiControlContextMenu();
      this.m_contextMenu.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      this.m_contextMenu.Deactivate();
      this.m_contextMenu.ItemClicked += new Action<MyGuiControlContextMenu, MyGuiControlContextMenu.EventArgs>(this.OnContextMenu_ItemClicked);
      this.Controls.Add((MyGuiControlBase) this.m_contextMenu);
    }

    private void OnContextMenu_ItemClicked(
      MyGuiControlContextMenu sender,
      MyGuiControlContextMenu.EventArgs eventArgs)
    {
      MyGuiScreenJoinGame.ContextMenuFavoriteActionItem userData = (MyGuiScreenJoinGame.ContextMenuFavoriteActionItem) eventArgs.UserData;
      MyGameServerItem server = userData.Server;
      if (server == null)
        return;
      switch (userData._Action)
      {
        case MyGuiScreenJoinGame.ContextMenuFavoriteAction.Add:
          MyGameService.AddFavoriteGame(server);
          break;
        case MyGuiScreenJoinGame.ContextMenuFavoriteAction.Remove:
          MyGameService.RemoveFavoriteGame(server);
          this.m_gamesTable.RemoveSelectedRow();
          this.m_favoritesPage.Text = new StringBuilder().Append((object) MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Favorites)).Append(" (").Append(this.m_gamesTable.RowsCount).Append(")");
          break;
        default:
          throw new InvalidBranchException();
      }
    }

    private void InitPageControls(MyGuiControlTabPage page)
    {
      page.Controls.Clear();
      if (this.m_joinButton != null)
        this.Controls.Remove((MyGuiControlBase) this.m_joinButton);
      if (this.m_detailsButton != null)
        this.Controls.Remove((MyGuiControlBase) this.m_detailsButton);
      if (this.m_directConnectButton != null)
        this.Controls.Remove((MyGuiControlBase) this.m_directConnectButton);
      if (this.m_refreshButton != null)
        this.Controls.Remove((MyGuiControlBase) this.m_refreshButton);
      Vector2 vector2_1 = new Vector2(-0.676f, -0.352f);
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      float y = 0.033f;
      this.m_searchBox = new MyGuiControlSearchBox(new Vector2?(vector2_1 + new Vector2(minSizeGui.X, y)));
      this.m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_searchBox.OnTextChanged += new MyGuiControlSearchBox.TextChangedDelegate(this.OnBlockSearchTextChanged);
      page.Controls.Add((MyGuiControlBase) this.m_searchBox);
      if (this.IsMultipleNetworking)
      {
        this.m_networkingButtons.Clear();
        this.m_networkingIcons.Clear();
        for (int index = 0; index < this.m_serverDiscoveryAggregator.GetAggregates().Count; ++index)
        {
          IMyServerDiscovery aggregate = this.m_serverDiscoveryAggregator.GetAggregates()[index];
          MyGuiControlButton guiControlButton1 = new MyGuiControlButton();
          guiControlButton1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
          guiControlButton1.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
          guiControlButton1.ShowTooltipWhenDisabled = true;
          guiControlButton1.Size = new Vector2(0.03f, 0.04f);
          MyGuiControlButton guiControlButton2 = guiControlButton1;
          guiControlButton2.SetToolTip(aggregate.ServiceName);
          page.Controls.Add((MyGuiControlBase) guiControlButton2);
          MyGuiControlImage icon = new MyGuiControlImage(textures: new string[1]
          {
            "Textures\\GUI\\Icons\\Browser\\" + aggregate.ServiceName + "CB.png"
          });
          icon.Size = guiControlButton2.Size * 0.75f;
          guiControlButton2.FocusChanged += (Action<MyGuiControlBase, bool>) ((x, state) => icon.ColorMask = state ? MyGuiConstants.HIGHLIGHT_TEXT_COLOR : Vector4.One);
          page.Controls.Add((MyGuiControlBase) icon);
          this.m_networkingButtons.Add(guiControlButton2);
          this.m_networkingIcons.Add(icon);
          guiControlButton2.ButtonClicked += (Action<MyGuiControlButton>) (x => this.OnToggleNetworkingClicked());
        }
        this.UpdateNetworkingButtons();
      }
      else
        this.m_networkingButtonsVisible = false;
      this.SetSearchBoxSize();
      MyGuiControlButton guiControlButton = new MyGuiControlButton();
      guiControlButton.Position = vector2_1 + new Vector2(minSizeGui.X, 0.033f) + new Vector2(0.909f, 3f / 500f);
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.ComboBoxButton;
      guiControlButton.Text = MyTexts.GetString(MyCommonTexts.Advanced);
      this.m_advancedSearchButton = guiControlButton;
      this.m_advancedSearchButton.ButtonClicked += new Action<MyGuiControlButton>(this.AdvancedSearchButtonClicked);
      this.m_advancedSearchButton.SetToolTip(MySpaceTexts.ToolTipJoinGame_Advanced);
      this.m_advancedSearchButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      page.Controls.Add((MyGuiControlBase) this.m_advancedSearchButton);
      this.m_gamesTable = new MyGuiControlTable();
      this.m_gamesTable.Position = vector2_1 + new Vector2(minSizeGui.X, 0.067f);
      this.m_gamesTable.Size = new Vector2(1450f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 1f);
      this.m_gamesTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_gamesTable.VisibleRowsCount = 17;
      page.Controls.Add((MyGuiControlBase) this.m_gamesTable);
      Vector2 vector2_2 = new Vector2(vector2_1.X, 0.0f) - new Vector2(-0.3137f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0710000023245811));
      Vector2 vector2_3 = new Vector2(0.1825f, 0.0f);
      int num1 = 0;
      Vector2 vector2_4 = vector2_2;
      Vector2 vector2_5 = vector2_3;
      int num2 = num1;
      int num3 = num2 + 1;
      double num4 = (double) num2;
      Vector2 vector2_6 = vector2_5 * (float) num4;
      this.m_detailsButton = this.MakeButton(vector2_4 + vector2_6, MyCommonTexts.JoinGame_ServerDetails, MySpaceTexts.ToolTipJoinGame_ServerDetails, new Action<MyGuiControlButton>(this.ServerDetailsClick));
      this.m_detailsButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_detailsButton);
      if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
      {
        this.m_directConnectButton = this.MakeButton(vector2_2 + vector2_3 * (float) num3++, MyCommonTexts.JoinGame_DirectConnect, MySpaceTexts.ToolTipJoinGame_DirectConnect, new Action<MyGuiControlButton>(this.DirectConnectClick));
        this.m_directConnectButton.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.Controls.Add((MyGuiControlBase) this.m_directConnectButton);
      }
      Vector2 vector2_7 = vector2_2;
      Vector2 vector2_8 = vector2_3;
      int num5 = num3;
      int num6 = num5 + 1;
      double num7 = (double) num5;
      Vector2 vector2_9 = vector2_8 * (float) num7;
      this.m_refreshButton = this.MakeButton(vector2_7 + vector2_9, MyCommonTexts.ScreenLoadSubscribedWorldRefresh, MySpaceTexts.ToolTipJoinGame_Refresh, (Action<MyGuiControlButton>) null);
      this.m_refreshButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_refreshButton);
      Vector2 vector2_10 = vector2_2;
      Vector2 vector2_11 = vector2_3;
      int num8 = num6;
      int num9 = num8 + 1;
      double num10 = (double) num8;
      Vector2 vector2_12 = vector2_11 * (float) num10;
      this.m_joinButton = this.MakeButton(vector2_10 + vector2_12, MyCommonTexts.ScreenMenuButtonJoinWorld, MySpaceTexts.ToolTipJoinGame_JoinWorld, (Action<MyGuiControlButton>) null);
      this.m_joinButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_joinButton);
      this.m_joinButton.Enabled = false;
      this.m_detailsButton.Enabled = false;
      this.m_loadingWheel = new MyGuiControlRotatingWheel(new Vector2?(this.m_joinButton.Position + new Vector2(0.2f, -0.026f)), new Vector4?(MyGuiConstants.ROTATING_WHEEL_COLOR), 0.22f);
      page.Controls.Add((MyGuiControlBase) this.m_loadingWheel);
      this.m_loadingWheel.Visible = false;
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.894999980926514 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.895f);
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.894999980926514 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.151999995112419)), this.m_size.Value.X * 0.895f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.894999980926514 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.895f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      this.FocusedControl = (MyGuiControlBase) this.m_searchBox.TextBox;
    }

    private void OnToggleNetworkingClicked()
    {
      this.m_networkingService0Selected = !this.m_networkingService0Selected;
      this.m_serverDiscovery = this.m_serverDiscoveryAggregator.GetAggregates()[this.m_networkingService0Selected ? 0 : 1];
      this.UpdateNetworkingButtons();
      this.m_stopServerRequest();
      this.RefreshRequest.InvokeIfNotNull<MyGuiControlButton>(this.m_refreshButton);
    }

    private void ShowNetworkingButtons()
    {
      if (!this.IsMultipleNetworking)
        return;
      this.m_networkingButtonsVisible = true;
      foreach (MyGuiControlBase networkingIcon in this.m_networkingIcons)
        networkingIcon.Visible = true;
      foreach (MyGuiControlBase networkingButton in this.m_networkingButtons)
        networkingButton.Visible = true;
      this.SetSearchBoxSize();
    }

    private void HideNetworkingButtons()
    {
      if (!this.IsMultipleNetworking)
        return;
      this.m_networkingButtonsVisible = false;
      foreach (MyGuiControlBase networkingIcon in this.m_networkingIcons)
        networkingIcon.Visible = false;
      foreach (MyGuiControlBase networkingButton in this.m_networkingButtons)
        networkingButton.Visible = false;
      this.SetSearchBoxSize();
    }

    private void UpdateNetworkingButtons()
    {
      for (int index = 0; index < this.m_networkingButtons.Count; ++index)
        this.m_networkingButtons[index].VisualStyle = (this.m_networkingService0Selected ? 0 : 1) == index ? MyGuiControlButtonStyleEnum.RectangularChecked : MyGuiControlButtonStyleEnum.Rectangular;
    }

    private void SetSearchBoxSize()
    {
      if (this.m_searchBox == null)
        return;
      this.m_searchBox.Size = (!MyInput.Static.IsJoystickLastUsed ? new Vector2(0.754f, 0.02f) : new Vector2(1450f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.02f)) - (this.m_networkingButtonsVisible ? new Vector2(0.06f, 0.0f) : Vector2.Zero);
      Vector2 vector2 = this.m_searchBox.Position + new Vector2(this.m_searchBox.Size.X, 0.0f) + new Vector2(0.0022f, 0.0065f);
      for (int index = 0; index < this.m_networkingButtons.Count; ++index)
      {
        this.m_networkingButtons[index].Position = vector2;
        this.m_networkingIcons[index].Position = this.m_networkingButtons[index].Position + new Vector2(this.m_networkingButtons[index].Size.X / 2f, 0.0f) + new Vector2(-1f / 625f, -1f / 625f);
        vector2 += new Vector2(this.m_networkingButtons[index].Size.X, 0.0f);
      }
    }

    private MyGuiControlButton MakeButton(
      Vector2 position,
      MyStringId text,
      MyStringId toolTip,
      Action<MyGuiControlButton> onClick)
    {
      Vector2? position1 = new Vector2?(position);
      Vector2? size = new Vector2?();
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(text);
      string toolTip1 = MyTexts.GetString(toolTip);
      StringBuilder text1 = stringBuilder;
      Action<MyGuiControlButton> onButtonClick = onClick;
      int? buttonIndex = new int?();
      return new MyGuiControlButton(position1, size: size, colorMask: colorMask, toolTip: toolTip1, text: text1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
    }

    public override bool RegisterClicks() => true;

    public override bool Update(bool hasFocus)
    {
      if (this.refresh_favorites & hasFocus)
      {
        this.refresh_favorites = false;
        this.m_joinButton.Enabled = false;
        this.m_detailsButton.Enabled = false;
        this.RebuildFavoritesList();
      }
      if (this.m_searchChanged && DateTime.Now.Subtract(this.m_searchLastChanged).Milliseconds > 500)
      {
        this.m_searchChanged = false;
        this.m_searchChangedFunc.InvokeIfNotNull();
      }
      if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
      {
        ++this.m_remainingTimeUpdateFrame;
        if (this.m_remainingTimeUpdateFrame % 50 == 0)
        {
          for (int index = 0; index < this.m_gamesTable.RowsCount; ++index)
            this.m_gamesTable.GetRow(index).Update();
          this.m_remainingTimeUpdateFrame = 0;
        }
      }
      if (hasFocus)
      {
        if (this.FocusedControl == this.m_joinGameTabs)
          this.FocusedControl = (MyGuiControlBase) this.m_gamesTable;
        this.m_detailsButton.Visible = !MyInput.Static.IsJoystickLastUsed;
        if (this.m_directConnectButton != null)
          this.m_directConnectButton.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.m_refreshButton.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.m_joinButton.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.m_advancedSearchButton.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.SetSearchBoxSize();
      }
      return base.Update(hasFocus);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (MyInput.Static.IsNewKeyPressed(MyKeys.F5))
        this.RefreshRequest.InvokeIfNotNull<MyGuiControlButton>(this.m_refreshButton);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT) && this.m_gamesTable == this.FocusedControl)
        this.OnJoinServer(this.m_joinButton);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.ServerDetailsClick(this.m_detailsButton);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        this.m_refreshButton.RaiseButtonClicked();
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU))
        this.AdvancedSearchButtonClicked((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.VIEW) && !MyPlatformGameSettings.LIMITED_MAIN_MENU)
        this.DirectConnectClick((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_BUTTON))
        this.onBannerClick(this.m_bannerImage);
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_BUTTON) || MyPlatformGameSettings.LIMITED_MAIN_MENU || !this.m_networkingButtonsVisible)
        return;
      this.OnToggleNetworkingClicked();
    }

    private bool FilterSimple(MyCachedServerItem item, string searchText = null)
    {
      MyGameServerItem server = item.Server;
      if ((int) server.AppID != (int) MyGameService.AppId)
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Name + " by appId: " + (object) server.AppID);
        return false;
      }
      string map = server.Map;
      int serverVersion = server.ServerVersion;
      if (string.IsNullOrEmpty(map))
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Name + " by sessionName: " + map);
        return false;
      }
      if (!string.IsNullOrWhiteSpace(searchText) && !server.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) && !server.Map.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Name + " by searchText: " + searchText);
        return false;
      }
      if (this.FilterOptions.AllowedGroups && !item.AllowedInGroup)
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Name + " by AllowedGroups");
        return false;
      }
      if (this.FilterOptions.SameVersion && serverVersion != (int) MyFinalBuildConstants.APP_VERSION)
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Name + " by appVersion: " + (object) serverVersion);
        return false;
      }
      if (this.FilterOptions.HasPassword.HasValue && this.FilterOptions.HasPassword.Value != server.Password)
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Name + " by HasPassword");
        return false;
      }
      if (MyFakes.ENABLE_MP_DATA_HASHES && this.FilterOptions.SameData)
      {
        string gameTagByPrefix = server.GetGameTagByPrefix("datahash");
        if (gameTagByPrefix != "" && gameTagByPrefix != this.m_dataHash)
        {
          MyLog.Default.WriteLine("Server filtered: " + server.Name + " by SameData");
          return false;
        }
      }
      string gameTagByPrefix1 = server.GetGameTagByPrefix("gamemode");
      if (gameTagByPrefix1 == "C" && !this.FilterOptions.CreativeMode)
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Name + " by CreativeMode: " + gameTagByPrefix1);
        return false;
      }
      if (gameTagByPrefix1.StartsWith("S") && !this.FilterOptions.SurvivalMode)
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Name + " by SurvivalMode: " + gameTagByPrefix1);
        return false;
      }
      ulong tagByPrefixUlong = server.GetGameTagByPrefixUlong("mods");
      if (this.FilterOptions.CheckMod && !this.FilterOptions.ModCount.ValueBetween((float) tagByPrefixUlong))
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Name + " by MOD_COUNT_TAG: " + (object) tagByPrefixUlong);
        return false;
      }
      if (this.FilterOptions.CheckPlayer && !this.FilterOptions.PlayerCount.ValueBetween((float) server.Players))
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Name + " by PlayerCount: " + (object) this.FilterOptions.PlayerCount);
        return false;
      }
      if (this.FilterOptions.Ping > -1 && server.Ping > this.FilterOptions.Ping)
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Name + " by Ping: " + (object) this.FilterOptions.Ping);
        return false;
      }
      float result;
      if (!float.TryParse(server.GetGameTagByPrefix("view"), out result) || !this.FilterOptions.CheckDistance || this.FilterOptions.ViewDistance.ValueBetween(result))
        return true;
      MyLog.Default.WriteLine("Server filtered: " + server.Name + " by viewDistance: " + (object) result);
      return false;
    }

    private bool FilterAdvanced(MyCachedServerItem item, string searchText = null)
    {
      if (!this.FilterSimple(item, searchText))
        return false;
      if (item.Rules == null || !item.Rules.Any<KeyValuePair<string, string>>())
      {
        MyLog.Default.WriteLine("Server filtered: " + item.Server.Name + " by Rules: " + (object) item.Rules);
        return false;
      }
      if (!this.FilterOptions.FilterServer(item))
      {
        MyLog.Default.WriteLine("Server filtered: " + item.Server.Name + " by FilterServer");
        return false;
      }
      if (this.FilterOptions.Mods != null && this.FilterOptions.Mods.Any<WorkshopId>() && this.FilterOptions.AdvancedFilter)
      {
        if (this.FilterOptions.ModsExclusive)
        {
          if (!this.FilterOptions.Mods.All<WorkshopId>((Func<WorkshopId, bool>) (modId => item.Mods.Contains(modId))))
          {
            MyLog.Default.WriteLine("Server filtered: " + item.Server.Name + " by Mods");
            return false;
          }
        }
        else if (item.Mods == null || !item.Mods.Any<WorkshopId>((Func<WorkshopId, bool>) (modId => this.FilterOptions.Mods.Contains(modId))))
        {
          MyLog.Default.WriteLine("Server filtered: " + item.Server.Name + " by Mods");
          return false;
        }
      }
      this.m_loadingWheel.Visible = false;
      return true;
    }

    private void AdvancedSearchButtonClicked(MyGuiControlButton myGuiControlButton)
    {
      if (this.m_detailsButton != null && this.m_joinButton != null)
      {
        this.m_detailsButton.Enabled = false;
        this.m_joinButton.Enabled = false;
      }
      MyGuiScreenServerSearchSpace serverSearchSpace = new MyGuiScreenServerSearchSpace(this);
      serverSearchSpace.Closed += (MyGuiScreenBase.ScreenHandler) ((b, isUnloading) => this.m_searchChangedFunc.InvokeIfNotNull());
      this.m_loadingWheel.Visible = false;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) serverSearchSpace);
    }

    private void ServerDetailsClick(MyGuiControlButton detailButton)
    {
      if (this.m_gamesTable.SelectedRow == null)
        return;
      MyGameServerItem ser = this.m_gamesTable.SelectedRow.UserData as MyGameServerItem;
      if (ser == null)
        return;
      MyCachedServerItem server = (detailButton.UserData as HashSet<MyCachedServerItem>).FirstOrDefault<MyCachedServerItem>((Func<MyCachedServerItem, bool>) (x => x.Server.ConnectionString.Equals(ser.ConnectionString)));
      if (server == null)
        return;
      this.m_loadingWheel.Visible = false;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenServerDetailsSpace(server));
    }

    private void DirectConnectClick(MyGuiControlButton button) => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenServerConnect());

    private void OnBlockSearchTextChanged(string text)
    {
      if (this.m_detailsButton != null && this.m_joinButton != null)
      {
        this.m_detailsButton.Enabled = false;
        this.m_joinButton.Enabled = false;
      }
      this.m_searchChanged = true;
      this.m_searchLastChanged = DateTime.Now;
    }

    private void InitServersPage()
    {
      this.InitServersTable(MyGameService.ServerDiscovery.PingSupport, MyGameService.ServerDiscovery.GroupSupport);
      this.m_joinButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnJoinServer);
      this.m_refreshButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnRefreshServersClick);
      this.RefreshRequest = new Action<MyGuiControlButton>(this.OnRefreshServersClick);
      this.m_stopServerRequest = new Action(this.CloseDedicatedServerListRequest);
      this.ShowNetworkingButtons();
      this.m_detailsButton.UserData = (object) this.m_dedicatedServers;
      this.m_dedicatedResponding = true;
      this.m_searchChangedFunc += (Action) (() => this.RefreshServerGameList(this.m_currentServerDiscovery.SupportsDirectServerSearch));
      this.m_serversPage = this.m_selectedPage;
      this.m_serversPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Servers));
      this.RefreshServerGameList(true);
    }

    private void CloseServersPage()
    {
      this.CloseDedicatedServerListRequest();
      this.m_dedicatedResponding = false;
      this.m_searchChangedFunc -= (Action) (() => this.RefreshServerGameList(false));
    }

    private void InitServersTable(bool pingSupport, bool groupSupport)
    {
      int num1 = MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME ? 9 : 8;
      if (pingSupport)
        ++num1;
      if (MyPlatformGameSettings.IsModdingAllowed)
        ++num1;
      this.m_gamesTable.ColumnsCount = num1;
      this.m_gamesTable.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemSelected);
      this.m_gamesTable.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnServerTableItemSelected);
      this.m_gamesTable.ItemDoubleClicked += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemDoubleClick);
      List<float> floatList = new List<float>();
      if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
      {
        floatList.AddRange((IEnumerable<float>) new float[9]
        {
          0.024f,
          0.024f,
          0.024f,
          0.24f,
          0.15f,
          0.024f,
          0.16f,
          0.15f,
          0.09f
        });
        if (pingSupport)
          floatList.Add(0.07f);
        if (MyPlatformGameSettings.IsModdingAllowed)
          floatList.Add(0.07f);
        this.m_gamesTable.SetCustomColumnWidths(floatList.ToArray());
      }
      else
      {
        floatList.AddRange((IEnumerable<float>) new float[8]
        {
          0.024f,
          0.024f,
          0.024f,
          0.26f,
          0.15f,
          0.024f,
          0.26f,
          0.09f
        });
        if (pingSupport)
          floatList.Add(0.07f);
        if (MyPlatformGameSettings.IsModdingAllowed)
          floatList.Add(0.07f);
        this.m_gamesTable.SetCustomColumnWidths(floatList.ToArray());
      }
      this.m_gamesTable.SetHeaderColumnMargin(num1 - 1, new Thickness(0.01f, 0.01f, 0.005f, 0.01f));
      int num2 = 3;
      MyGuiControlTable gamesTable1 = this.m_gamesTable;
      int colIdx1 = num2;
      int num3 = colIdx1 + 1;
      Comparison<MyGuiControlTable.Cell> ascendingComparison1 = new Comparison<MyGuiControlTable.Cell>(this.TextComparison);
      gamesTable1.SetColumnComparison(colIdx1, ascendingComparison1);
      MyGuiControlTable gamesTable2 = this.m_gamesTable;
      int colIdx2 = num3;
      int num4 = colIdx2 + 1;
      Comparison<MyGuiControlTable.Cell> ascendingComparison2 = new Comparison<MyGuiControlTable.Cell>(this.TextComparison);
      gamesTable2.SetColumnComparison(colIdx2, ascendingComparison2);
      MyGuiControlTable gamesTable3 = this.m_gamesTable;
      int colIdx3 = num4;
      int num5 = colIdx3 + 1;
      Comparison<MyGuiControlTable.Cell> ascendingComparison3 = new Comparison<MyGuiControlTable.Cell>(this.TextComparison);
      gamesTable3.SetColumnComparison(colIdx3, ascendingComparison3);
      MyGuiControlTable gamesTable4 = this.m_gamesTable;
      int colIdx4 = num5;
      int colIdx5 = colIdx4 + 1;
      Comparison<MyGuiControlTable.Cell> ascendingComparison4 = new Comparison<MyGuiControlTable.Cell>(this.TextComparison);
      gamesTable4.SetColumnComparison(colIdx4, ascendingComparison4);
      if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
      {
        this.m_gamesTable.SetColumnComparison(colIdx5, new Comparison<MyGuiControlTable.Cell>(this.TextComparison));
        this.m_gamesTable.SetColumnAlign(colIdx5);
        this.m_gamesTable.SetHeaderColumnAlign(colIdx5);
        ++colIdx5;
      }
      this.m_gamesTable.SetColumnComparison(colIdx5, new Comparison<MyGuiControlTable.Cell>(this.PlayerCountComparison));
      this.m_gamesTable.SetColumnAlign(colIdx5);
      this.m_gamesTable.SetHeaderColumnAlign(colIdx5);
      int colIdx6 = colIdx5 + 1;
      int num6 = colIdx6;
      if (pingSupport)
      {
        this.m_gamesTable.SetColumnComparison(colIdx6, new Comparison<MyGuiControlTable.Cell>(this.PingComparison));
        this.m_gamesTable.SetColumnAlign(colIdx6);
        this.m_gamesTable.SetHeaderColumnAlign(colIdx6);
        ++colIdx6;
      }
      if (MyPlatformGameSettings.IsModdingAllowed)
      {
        this.m_gamesTable.SetColumnComparison(colIdx6, new Comparison<MyGuiControlTable.Cell>(this.ModsComparison));
        this.m_gamesTable.SetColumnAlign(colIdx6);
        this.m_gamesTable.SetHeaderColumnAlign(colIdx6);
        int num7 = colIdx6 + 1;
      }
      this.SupportsPing = pingSupport;
      this.SupportsGroups = groupSupport;
      this.m_gamesTable.SortByColumn(pingSupport ? num6 : 0);
    }

    private void OnJoinServer(MyGuiControlButton obj) => this.JoinSelectedServer();

    private void OnServerTableItemSelected(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs eventArgs)
    {
      if (sender.SelectedRow == null || !(sender.SelectedRow.UserData is MyGameServerItem userData) || string.IsNullOrEmpty(userData.ConnectionString))
        return;
      MyGuiControlTable.Cell cell = sender.SelectedRow.GetCell(5);
      if (cell == null || cell.ToolTip == null)
        return;
      if (eventArgs.MouseButton == MyMouseButtonsEnum.Right)
      {
        this.m_contextMenu.CreateNewContextMenu();
        MyGuiScreenJoinGame.ContextMenuFavoriteAction menuFavoriteAction = this.m_selectedPage == this.m_favoritesPage ? MyGuiScreenJoinGame.ContextMenuFavoriteAction.Remove : MyGuiScreenJoinGame.ContextMenuFavoriteAction.Add;
        MyStringId id = MyCommonTexts.JoinGame_Favorites_Remove;
        if (menuFavoriteAction == MyGuiScreenJoinGame.ContextMenuFavoriteAction.Add)
          id = MyCommonTexts.JoinGame_Favorites_Add;
        this.m_contextMenu.AddItem(MyTexts.Get(id), "", "", (object) new MyGuiScreenJoinGame.ContextMenuFavoriteActionItem()
        {
          Server = userData,
          _Action = menuFavoriteAction
        });
        this.m_contextMenu.ItemList_UseSimpleItemListMouseOverCheck = true;
        this.m_contextMenu.Activate();
      }
      else
        this.m_contextMenu.Deactivate();
    }

    private void OnTableItemDoubleClick(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs eventArgs)
    {
      this.JoinSelectedServer();
    }

    private void JoinSelectedServer(bool checkPing = true)
    {
      MyGuiControlTable.Row selectedRow = this.m_gamesTable.SelectedRow;
      if (selectedRow == null)
        return;
      if (selectedRow.UserData is MyGameServerItem userData)
      {
        if (!MySandboxGame.Config.ExperimentalMode && userData.Experimental)
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MultiplayerErrorExperimental), messageCaption: messageCaption));
        }
        else if (checkPing && userData.Ping > 150)
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.MultiplayerWarningPing), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
          {
            if (result != MyGuiScreenMessageBox.ResultEnum.YES)
              return;
            this.JoinSelectedServer(false);
          }))));
        }
        else
        {
          MyJoinGameHelper.JoinGame(userData);
          MyLocalCache.SaveLastSessionInfo((string) null, true, false, userData.Name, userData.ConnectionString);
        }
      }
      else
      {
        if (!(selectedRow.UserData is IMyLobby userData))
          return;
        MyJoinGameHelper.JoinGame(userData);
        MyLocalCache.SaveLastSessionInfo((string) null, true, true, selectedRow.GetCell(0).Text.ToString(), userData.LobbyId.ToString(), 0);
      }
    }

    private void OnRefreshServersClick(MyGuiControlButton obj)
    {
      if (this.m_detailsButton != null && this.m_joinButton != null)
      {
        this.m_detailsButton.Enabled = false;
        this.m_joinButton.Enabled = false;
      }
      switch (this.m_nextState)
      {
        case MyGuiScreenJoinGame.RefreshStateEnum.Pause:
          this.m_refreshPaused = true;
          this.m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldResume);
          this.m_nextState = MyGuiScreenJoinGame.RefreshStateEnum.Resume;
          this.m_loadingWheel.Visible = false;
          break;
        case MyGuiScreenJoinGame.RefreshStateEnum.Resume:
          this.m_refreshPaused = false;
          if (this.m_loadingWheel.Visible)
          {
            this.m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldPause);
            this.m_nextState = MyGuiScreenJoinGame.RefreshStateEnum.Pause;
            this.m_loadingWheel.Visible = true;
          }
          else
          {
            this.m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldRefresh);
            this.m_nextState = MyGuiScreenJoinGame.RefreshStateEnum.Refresh;
            this.m_loadingWheel.Visible = false;
          }
          this.RefreshServerGameList(false);
          break;
        case MyGuiScreenJoinGame.RefreshStateEnum.Refresh:
          if (!MyInput.Static.IsJoystickLastUsed)
          {
            this.m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldPause);
            this.m_nextState = MyGuiScreenJoinGame.RefreshStateEnum.Pause;
          }
          this.m_dedicatedServers.Clear();
          this.RefreshServerGameList(true);
          this.m_loadingWheel.Visible = true;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void OnDedicatedServerListResponded(object sender, int server) => this.OnDedicateServerResult(this.m_currentServerDiscovery.GetDedicatedServerDetails(server));

    private void OnDedicateServerResult(MyGameServerItem game, bool fromRankedQuery = false)
    {
      MyCachedServerItem serverItem = new MyCachedServerItem(game);
      if (this.m_currentServerDiscovery.SupportsDirectServerSearch && !fromRankedQuery && (this.IsRanked(serverItem) && this.m_dedicatedServers.Any<MyCachedServerItem>((Func<MyCachedServerItem, bool>) (x => x.Server.ConnectionString == game.ConnectionString))))
        return;
      this.m_currentServerDiscovery.GetServerRules(serverItem.Server, (ServerRulesResponse) (rules => this.DedicatedRulesResponse(rules, serverItem)), (Action) (() => this.DedicatedRulesResponse((Dictionary<string, string>) null, serverItem)));
    }

    private void DedicatedRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
    {
      if (server.Server.ConnectionString == null || this.m_dedicatedServers.Any<MyCachedServerItem>(new Func<MyCachedServerItem, bool>(Predicate)))
      {
        MyLog.Default.WriteLine("Connection string duplicate: " + server.Server.ConnectionString);
      }
      else
      {
        if (this.m_serverDiscoveryAggregator != null && this.m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) != this.m_currentServerDiscovery)
          return;
        server.Rules = rules;
        if (rules != null)
          server.DeserializeSettings();
        this.m_dedicatedServers.Add(server);
        if (!this.m_dedicatedResponding)
        {
          MyLog.Default.WriteLine("Server page closed.");
        }
        else
        {
          server.Server.IsRanked = this.IsRanked(server);
          this.AddServerItem(server);
          if (this.m_refreshPaused)
            return;
          this.m_serversPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Servers)).Append(" (").Append(this.m_gamesTable.RowsCount).Append(")");
        }
      }

      bool Predicate(MyCachedServerItem x)
      {
        if (x == null)
          MyLog.Default.WriteLine("Existing item in dedicated server list is null.");
        else if (x.Server == null)
          MyLog.Default.WriteLine("Existing item in dedicated server list has null server.");
        else if (server?.Server == null)
          MyLog.Default.WriteLine("Incoming item has become null after this call started.");
        return server.Server.ConnectionString.Equals(x.Server.ConnectionString);
      }
    }

    private bool IsRanked(MyCachedServerItem server) => this.IsRanked(server.Server);

    private bool IsRanked(MyGameServerItem gameServer)
    {
      if (this.m_rankedServers == null)
        return false;
      string connectionString = gameServer.ConnectionString;
      return this.m_rankedServers.GetByPrefix(this.m_currentServerDiscovery.ConnectionStringPrefix).Any<MyRankServer>((Func<MyRankServer, bool>) (server => server.ConnectionString == connectionString));
    }

    private void OnDedicatedServersCompleteResponse(
      object sender,
      MyMatchMakingServerResponse response)
    {
      this.CloseDedicatedServerListRequest();
    }

    private void CloseDedicatedServerListRequest()
    {
      if (this.m_loadingWheel != null)
        this.m_loadingWheel.Visible = false;
      if (this.m_currentServerDiscovery != null)
      {
        this.m_currentServerDiscovery.OnDedicatedServerListResponded -= new EventHandler<int>(this.OnDedicatedServerListResponded);
        this.m_currentServerDiscovery.OnDedicatedServersCompleteResponse -= new EventHandler<MyMatchMakingServerResponse>(this.OnDedicatedServersCompleteResponse);
        this.m_currentServerDiscovery.CancelInternetServersRequest();
      }
      if (this.m_nextState != MyGuiScreenJoinGame.RefreshStateEnum.Pause)
        return;
      if (this.m_refreshButton != null)
        this.m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldRefresh);
      this.m_nextState = MyGuiScreenJoinGame.RefreshStateEnum.Refresh;
      this.m_refreshPaused = false;
    }

    private void AddServerHeaders(bool pingSupport)
    {
      int num1 = 0;
      int columnsCount = this.m_gamesTable.ColumnsCount;
      if (num1 < columnsCount)
        this.m_gamesTable.SetColumnName(num1++, new StringBuilder());
      if (num1 < columnsCount)
        this.m_gamesTable.SetColumnName(num1++, new StringBuilder());
      if (num1 < columnsCount)
        this.m_gamesTable.SetColumnName(num1++, new StringBuilder());
      if (num1 < columnsCount)
        this.m_gamesTable.SetColumnName(num1++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_World));
      if (num1 < columnsCount)
        this.m_gamesTable.SetColumnName(num1++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_GameMode));
      if (num1 < columnsCount)
        this.m_gamesTable.SetColumnName(num1++, new StringBuilder());
      if (num1 < columnsCount)
        this.m_gamesTable.SetColumnName(num1++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Server));
      if (num1 < columnsCount && MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
        this.m_gamesTable.SetColumnName(num1++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_RemainingTime));
      if (num1 < columnsCount)
        this.m_gamesTable.SetColumnName(num1++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Players));
      if (num1 < columnsCount & pingSupport)
        this.m_gamesTable.SetColumnName(num1++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Ping));
      if (num1 >= columnsCount || !MyPlatformGameSettings.IsModdingAllowed)
        return;
      MyGuiControlTable gamesTable = this.m_gamesTable;
      int colIdx = num1;
      int num2 = colIdx + 1;
      StringBuilder name = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Mods);
      gamesTable.SetColumnName(colIdx, name);
    }

    private void RefreshServerGameList(bool resetSteamQuery)
    {
      if (this.m_lastVersionCheck != this.FilterOptions.SameVersion || this.FilterOptions.AdvancedFilter)
        resetSteamQuery = true;
      this.m_lastVersionCheck = this.FilterOptions.SameVersion;
      this.m_detailsButton.Enabled = false;
      this.m_joinButton.Enabled = false;
      this.m_gamesTable.Clear();
      this.AddServerHeaders(MyGameService.ServerDiscovery.PingSupport);
      this.m_textCache.Clear();
      this.m_gameTypeText.Clear();
      this.m_gameTypeToolTip.Clear();
      this.m_serversPage.TextEnum = MyCommonTexts.JoinGame_TabTitle_Servers;
      if (resetSteamQuery)
      {
        this.m_dedicatedServers.Clear();
        this.CloseDedicatedServerListRequest();
        if (!MyInput.Static.IsJoystickLastUsed)
        {
          this.m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldPause);
          this.m_nextState = MyGuiScreenJoinGame.RefreshStateEnum.Pause;
        }
        this.m_refreshPaused = false;
        if (this.m_enableDedicatedServers)
        {
          this.m_currentServerDiscovery = this.m_serverDiscovery;
          IMyServerDiscovery currentServerDiscovery = this.m_currentServerDiscovery;
          MySessionSearchFilter networkFilter = this.FilterOptions.GetNetworkFilter(currentServerDiscovery.SupportedSearchParameters, this.m_searchBox.SearchText);
          MySandboxGame.Log.WriteLine("Requesting dedicated servers, filterOps: " + (object) networkFilter);
          currentServerDiscovery.OnDedicatedServerListResponded += new EventHandler<int>(this.OnDedicatedServerListResponded);
          currentServerDiscovery.OnDedicatedServersCompleteResponse += new EventHandler<MyMatchMakingServerResponse>(this.OnDedicatedServersCompleteResponse);
          if (currentServerDiscovery.SupportsDirectServerSearch && this.m_rankedServers != null)
          {
            string[] array = this.m_rankedServers.GetByPrefix(currentServerDiscovery.ConnectionStringPrefix).Select<MyRankServer, string>((Func<MyRankServer, string>) (x => x.ConnectionString)).ToArray<string>();
            if (array.Length != 0)
              currentServerDiscovery.RequestServerItems(array, networkFilter, new Action<IEnumerable<MyGameServerItem>>(this.RankedServerQueryComplete));
          }
          currentServerDiscovery.RequestInternetServerList(networkFilter);
          this.m_loadingWheel.Visible = true;
        }
      }
      this.m_gamesTable.SelectedRowIndex = new int?();
      this.RebuildServerList();
    }

    private void RankedServerQueryComplete(IEnumerable<MyGameServerItem> servers)
    {
      foreach (MyGameServerItem server in servers)
        this.OnDedicateServerResult(server, true);
    }

    private void RebuildServerList()
    {
      string searchText = this.m_searchBox.SearchText;
      if (string.IsNullOrWhiteSpace(searchText))
        searchText = (string) null;
      this.m_detailsButton.Enabled = false;
      this.m_joinButton.Enabled = false;
      this.m_gamesTable.Clear();
      foreach (MyCachedServerItem dedicatedServer in this.m_dedicatedServers)
      {
        if (this.FilterOptions.AdvancedFilter)
        {
          if (!this.FilterAdvanced(dedicatedServer, searchText))
            continue;
        }
        else if (!this.FilterSimple(dedicatedServer, searchText))
          continue;
        MyGameServerItem server = dedicatedServer.Server;
        StringBuilder gamemodeSB = new StringBuilder();
        StringBuilder gamemodeToolTipSB = new StringBuilder();
        string gameTagByPrefix = server.GetGameTagByPrefix("gamemode");
        if (gameTagByPrefix == "C")
        {
          gamemodeSB.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
          gamemodeToolTipSB.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
        }
        else if (!string.IsNullOrWhiteSpace(gameTagByPrefix))
        {
          string str = gameTagByPrefix.Substring(1);
          string[] strArray = str.Split('-');
          if (strArray.Length == 4)
          {
            gamemodeSB.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival)).Append(" ").Append(str);
            gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_MultipliersFormat), (object) strArray[0], (object) strArray[1], (object) strArray[2], (object) strArray[3]);
          }
          else
          {
            gamemodeSB.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival));
            gamemodeToolTipSB.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival));
          }
        }
        this.AddServerItem(server, server.Map, gamemodeSB, gamemodeToolTipSB, false, dedicatedServer.Settings);
      }
      this.m_gamesTable.Sort(false);
      this.m_serversPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Servers)).Append(" (").Append(this.m_gamesTable.RowsCount).Append(")");
    }

    private bool AddServerItem(MyCachedServerItem item)
    {
      MyGameServerItem server = item.Server;
      server.Name = MySandboxGame.Static.FilterOffensive(server.Name);
      server.Map = MySandboxGame.Static.FilterOffensive(server.Map);
      server.Experimental = item.ExperimentalMode;
      if (this.FilterOptions.AdvancedFilter && item.Rules != null)
      {
        if (!this.FilterAdvanced(item, this.m_searchBox.SearchText))
          return false;
      }
      else if (!this.FilterSimple(item, this.m_searchBox.SearchText))
        return false;
      string map = server.Map;
      StringBuilder gamemodeSB = new StringBuilder();
      StringBuilder gamemodeToolTipSB = new StringBuilder();
      string gameTagByPrefix = server.GetGameTagByPrefix("gamemode");
      if (gameTagByPrefix == "C")
      {
        gamemodeSB.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
        gamemodeToolTipSB.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
      }
      else if (!string.IsNullOrWhiteSpace(gameTagByPrefix))
      {
        string str = gameTagByPrefix.Substring(1);
        string[] strArray = str.Split('-');
        if (strArray.Length == 4)
        {
          gamemodeSB.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival)).Append(" ").Append(str);
          gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_MultipliersFormat), (object) strArray[0], (object) strArray[1], (object) strArray[2], (object) strArray[3]);
        }
        else
        {
          gamemodeSB.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival));
          gamemodeToolTipSB.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival));
        }
      }
      if (!this.m_refreshPaused)
        this.AddServerItem(server, map, gamemodeSB, gamemodeToolTipSB, settings: item.Settings);
      return true;
    }

    private void AddServerItem(
      MyGameServerItem server,
      string sessionName,
      StringBuilder gamemodeSB,
      StringBuilder gamemodeToolTipSB,
      bool sort = true,
      MyObjectBuilder_SessionSettings settings = null)
    {
      ulong tagByPrefixUlong = server.GetGameTagByPrefixUlong("mods");
      string str1 = server.MaxPlayers.ToString();
      StringBuilder text1 = new StringBuilder(server.Players.ToString() + "/" + str1);
      string gameTagByPrefix = server.GetGameTagByPrefix("view");
      if (!string.IsNullOrEmpty(gameTagByPrefix))
      {
        gamemodeToolTipSB.AppendLine();
        gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_ViewDistance), (object) gameTagByPrefix);
      }
      if (settings != null)
      {
        gamemodeToolTipSB.AppendLine();
        gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_PCU_Max), (object) settings.TotalPCU);
        gamemodeToolTipSB.AppendLine();
        gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_PCU_Settings), (object) settings.BlockLimitsEnabled);
        gamemodeToolTipSB.AppendLine();
        gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_PCU_Initial), (object) MyObjectBuilder_SessionSettings.GetInitialPCU(settings));
        gamemodeToolTipSB.AppendLine();
        gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_Airtightness), settings.EnableOxygenPressurization ? (object) MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_ON) : (object) MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_OFF));
      }
      Color? textColor1 = new Color?(Color.White);
      if (server.Experimental && !MySandboxGame.Config.ExperimentalMode)
        textColor1 = new Color?(Color.DarkGray);
      MyGuiControlTable.Row row1 = new MyGuiControlTable.Row((object) server);
      string str2 = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Rank).ToString();
      MyGuiHighlightTexture? nullable1;
      if (server.IsRanked)
      {
        MyGuiControlTable.Row row2 = row1;
        StringBuilder text2 = new StringBuilder();
        string toolTip = str2;
        nullable1 = new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_ICON_STAR);
        Color? textColor2 = textColor1;
        MyGuiHighlightTexture? icon = nullable1;
        MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(text2, toolTip: toolTip, textColor: textColor2, icon: icon, iconOriginAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
        row2.AddCell(cell);
      }
      else
      {
        MyGuiControlTable.Row row2 = row1;
        StringBuilder text2 = new StringBuilder();
        string toolTip = str2;
        Color? textColor2 = textColor1;
        nullable1 = new MyGuiHighlightTexture?();
        MyGuiHighlightTexture? icon = nullable1;
        MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(text2, toolTip: toolTip, textColor: textColor2, icon: icon, iconOriginAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
        row2.AddCell(cell);
      }
      string str3 = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Passworded).ToString();
      if (server.Password)
      {
        MyGuiControlTable.Row row2 = row1;
        StringBuilder text2 = new StringBuilder();
        string toolTip = str3;
        nullable1 = new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_ICON_LOCK);
        Color? textColor2 = textColor1;
        MyGuiHighlightTexture? icon = nullable1;
        MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(text2, toolTip: toolTip, textColor: textColor2, icon: icon, iconOriginAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
        row2.AddCell(cell);
      }
      else
      {
        MyGuiControlTable.Row row2 = row1;
        StringBuilder text2 = new StringBuilder();
        Color? textColor2 = textColor1;
        nullable1 = new MyGuiHighlightTexture?();
        MyGuiHighlightTexture? icon = nullable1;
        MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(text2, textColor: textColor2, icon: icon, iconOriginAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
        row2.AddCell(cell);
      }
      if (server.Experimental)
      {
        MyGuiControlTable.Row row2 = row1;
        StringBuilder text2 = new StringBuilder();
        string toolTip = MyTexts.GetString(MyCommonTexts.ServerIsExperimental);
        nullable1 = new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_ICON_EXPERIMENTAL);
        Color? textColor2 = textColor1;
        MyGuiHighlightTexture? icon = nullable1;
        MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(text2, toolTip: toolTip, textColor: textColor2, icon: icon, iconOriginAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
        row2.AddCell(cell);
      }
      else
        row1.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), textColor: textColor1, iconOriginAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
      this.m_textCache.Clear().Append(sessionName);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(sessionName);
      if (server.Experimental)
        stringBuilder.Append(MyTexts.GetString(MyCommonTexts.ServerIsExperimental));
      row1.AddCell(new MyGuiControlTable.Cell(this.m_textCache, (object) server.GameID, stringBuilder.ToString(), textColor1));
      MyGuiControlTable.Row row3 = row1;
      StringBuilder text3 = gamemodeSB;
      string toolTip1 = gamemodeToolTipSB.ToString();
      Color? textColor3 = textColor1;
      MyGuiHighlightTexture? nullable2 = new MyGuiHighlightTexture?();
      MyGuiHighlightTexture? icon1 = nullable2;
      MyGuiControlTable.Cell cell1 = new MyGuiControlTable.Cell(text3, toolTip: toolTip1, textColor: textColor3, icon: icon1);
      row3.AddCell(cell1);
      int length = server.ConnectionString.IndexOf("://");
      string str4 = length == -1 ? "steam" : server.ConnectionString.Substring(0, length);
      string str5 = MyTexts.Get(MyStringId.GetOrCompute("JoinGame_Networking_" + str4)).ToString();
      MyGuiControlTable.Row row4 = row1;
      StringBuilder text4 = new StringBuilder();
      string toolTip2 = str5;
      nullable2 = new MyGuiHighlightTexture?(new MyGuiHighlightTexture()
      {
        Normal = "Textures\\GUI\\Icons\\Services\\" + str4 + ".png",
        Highlight = "Textures\\GUI\\Icons\\Services\\" + str4 + ".png",
        SizePx = new Vector2(24f, 24f)
      });
      Color? textColor4 = textColor1;
      MyGuiHighlightTexture? icon2 = nullable2;
      MyGuiControlTable.Cell cell2 = new MyGuiControlTable.Cell(text4, toolTip: toolTip2, textColor: textColor4, icon: icon2, iconOriginAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      row4.AddCell(cell2);
      row1.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear().Append(server.Name), toolTip: this.m_gameTypeToolTip.Clear().AppendLine(server.Name).Append(server.ConnectionString).ToString(), textColor: textColor1));
      row1.AddCell(new MyGuiControlTable.Cell(text1, toolTip: text1.ToString(), textColor: textColor1));
      if (this.SupportsPing)
        row1.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear().Append(server.Ping < 0 ? "---" : server.Ping.ToString()), toolTip: this.m_textCache.ToString(), textColor: textColor1));
      if (MyPlatformGameSettings.IsModdingAllowed)
        row1.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear().Append(tagByPrefixUlong == 0UL ? "---" : tagByPrefixUlong.ToString()), textColor: textColor1));
      if (server.IsRanked)
      {
        row1.IsGlobalSortEnabled = false;
        this.m_gamesTable.Insert(0, row1);
      }
      else
        this.m_gamesTable.Add(row1);
      if (!sort || server.IsRanked)
        return;
      MyGuiControlTable.Row selectedRow = this.m_gamesTable.SelectedRow;
      this.m_gamesTable.Sort(false);
      this.m_gamesTable.SelectedRowIndex = new int?(this.m_gamesTable.FindRow(selectedRow));
    }

    private void InitLobbyPage()
    {
      this.InitLobbyTable();
      this.m_detailsButton.Enabled = false;
      this.HideNetworkingButtons();
      this.m_joinButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnJoinServer);
      this.m_refreshButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnRefreshLobbiesClick);
      this.RefreshRequest = new Action<MyGuiControlButton>(this.OnRefreshLobbiesClick);
      this.m_searchChangedFunc += new Action(this.RefreshGameList);
      this.m_lobbyPage = this.m_selectedPage;
      this.m_lobbyPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Lobbies));
      this.LoadPublicLobbies();
    }

    private void CloseLobbyPage() => this.m_searchChangedFunc -= new Action(this.LoadPublicLobbies);

    private void InitLobbyTable()
    {
      this.m_gamesTable.ColumnsCount = MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME ? 6 : 5;
      this.m_gamesTable.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemSelected);
      this.m_gamesTable.ItemDoubleClicked += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemDoubleClick);
      if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
        this.m_gamesTable.SetCustomColumnWidths(new float[6]
        {
          0.3f,
          0.18f,
          0.2f,
          0.16f,
          0.08f,
          MyPlatformGameSettings.IsModdingAllowed ? 0.07f : 0.0f
        });
      else
        this.m_gamesTable.SetCustomColumnWidths(new float[5]
        {
          0.29f,
          0.19f,
          0.37f,
          0.08f,
          MyPlatformGameSettings.IsModdingAllowed ? 0.07f : 0.0f
        });
      int num1 = 0;
      MyGuiControlTable gamesTable1 = this.m_gamesTable;
      int colIdx1 = num1;
      int num2 = colIdx1 + 1;
      Comparison<MyGuiControlTable.Cell> ascendingComparison1 = new Comparison<MyGuiControlTable.Cell>(this.TextComparison);
      gamesTable1.SetColumnComparison(colIdx1, ascendingComparison1);
      MyGuiControlTable gamesTable2 = this.m_gamesTable;
      int colIdx2 = num2;
      int num3 = colIdx2 + 1;
      Comparison<MyGuiControlTable.Cell> ascendingComparison2 = new Comparison<MyGuiControlTable.Cell>(this.TextComparison);
      gamesTable2.SetColumnComparison(colIdx2, ascendingComparison2);
      MyGuiControlTable gamesTable3 = this.m_gamesTable;
      int colIdx3 = num3;
      int colIdx4 = colIdx3 + 1;
      Comparison<MyGuiControlTable.Cell> ascendingComparison3 = new Comparison<MyGuiControlTable.Cell>(this.TextComparison);
      gamesTable3.SetColumnComparison(colIdx3, ascendingComparison3);
      if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
      {
        this.m_gamesTable.SetColumnComparison(colIdx4, new Comparison<MyGuiControlTable.Cell>(this.TextComparison));
        this.m_gamesTable.SetColumnAlign(colIdx4);
        this.m_gamesTable.SetHeaderColumnAlign(colIdx4);
        ++colIdx4;
      }
      this.m_gamesTable.SetColumnComparison(colIdx4, new Comparison<MyGuiControlTable.Cell>(this.PlayerCountComparison));
      this.m_gamesTable.SetColumnAlign(colIdx4);
      this.m_gamesTable.SetHeaderColumnAlign(colIdx4);
      int colIdx5 = colIdx4 + 1;
      this.m_gamesTable.SetColumnComparison(colIdx5, new Comparison<MyGuiControlTable.Cell>(this.ModsComparison));
      this.m_gamesTable.SetColumnAlign(colIdx5);
      this.m_gamesTable.SetHeaderColumnAlign(colIdx5);
      int num4 = colIdx5 + 1;
      this.SupportsPing = false;
      this.SupportsGroups = false;
      this.AddHeaders();
    }

    private void OnTableItemSelected(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs eventArgs)
    {
      sender.CanHaveFocus = true;
      this.FocusedControl = (MyGuiControlBase) sender;
      if (this.m_gamesTable.SelectedRow != null)
      {
        this.m_joinButton.Enabled = true;
        if (!(this.m_gamesTable.SelectedRow.UserData is MyGameServerItem))
          return;
        this.m_detailsButton.Enabled = true;
      }
      else
      {
        this.m_joinButton.Enabled = false;
        this.m_detailsButton.Enabled = false;
      }
    }

    private void OnRefreshLobbiesClick(MyGuiControlButton obj) => this.LoadPublicLobbies();

    private void PublicLobbiesCallback(bool success)
    {
      if (this.m_selectedPage != this.m_lobbyPage)
        return;
      if (!success)
      {
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("Cannot enumerate worlds"), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
      }
      else
      {
        this.m_lobbies.Clear();
        MyGameService.AddPublicLobbies(this.m_lobbies);
        this.RefreshGameList();
        this.m_loadingWheel.Visible = false;
      }
    }

    private void LoadPublicLobbies()
    {
      this.m_loadingWheel.Visible = true;
      MySandboxGame.Log.WriteLine("Requesting lobbies");
      if (this.FilterOptions.SameVersion)
        MyGameService.AddLobbyFilter("appVersion", MyFinalBuildConstants.APP_VERSION.ToString());
      MySandboxGame.Log.WriteLine("Requesting worlds, only compatible: " + this.FilterOptions.SameVersion.ToString());
      MyGameService.RequestLobbyList(new Action<bool>(this.PublicLobbiesCallback));
    }

    private void AddHeaders()
    {
      int num1 = 0;
      MyGuiControlTable gamesTable1 = this.m_gamesTable;
      int colIdx1 = num1;
      int num2 = colIdx1 + 1;
      StringBuilder name1 = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_World);
      gamesTable1.SetColumnName(colIdx1, name1);
      MyGuiControlTable gamesTable2 = this.m_gamesTable;
      int colIdx2 = num2;
      int num3 = colIdx2 + 1;
      StringBuilder name2 = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_GameMode);
      gamesTable2.SetColumnName(colIdx2, name2);
      MyGuiControlTable gamesTable3 = this.m_gamesTable;
      int colIdx3 = num3;
      int num4 = colIdx3 + 1;
      StringBuilder name3 = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Username);
      gamesTable3.SetColumnName(colIdx3, name3);
      if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
        this.m_gamesTable.SetColumnName(num4++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_RemainingTime));
      MyGuiControlTable gamesTable4 = this.m_gamesTable;
      int colIdx4 = num4;
      int num5 = colIdx4 + 1;
      StringBuilder name4 = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Players);
      gamesTable4.SetColumnName(colIdx4, name4);
      if (!MyPlatformGameSettings.IsModdingAllowed)
        return;
      MyGuiControlTable gamesTable5 = this.m_gamesTable;
      int colIdx5 = num5;
      int num6 = colIdx5 + 1;
      StringBuilder name5 = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Mods);
      gamesTable5.SetColumnName(colIdx5, name5);
    }

    private void RefreshGameList()
    {
      if (this.State == MyGuiScreenState.CLOSED)
        return;
      this.m_gamesTable.Clear();
      this.AddHeaders();
      this.m_textCache.Clear();
      this.m_gameTypeText.Clear();
      this.m_gameTypeToolTip.Clear();
      this.m_lobbyPage.Text = MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Lobbies);
      if (this.m_lobbies != null)
      {
        int num1 = 0;
        for (int index = 0; index < this.m_lobbies.Count; ++index)
        {
          IMyLobby lobby = this.m_lobbies[index];
          MyGuiControlTable.Row row = new MyGuiControlTable.Row((object) lobby);
          if (!this.FilterOptions.AdvancedFilter || this.FilterOptions.FilterLobby(lobby))
          {
            string lobbyWorldName = MyMultiplayerLobby.GetLobbyWorldName(lobby);
            long lobbyWorldSize = (long) MyMultiplayerLobby.GetLobbyWorldSize(lobby);
            int lobbyAppVersion = MyMultiplayerLobby.GetLobbyAppVersion(lobby);
            int lobbyModCount = MyMultiplayerLobby.GetLobbyModCount(lobby);
            string str1 = (string) null;
            float? nullable = new float?();
            string str2 = this.m_searchBox.SearchText.Trim();
            if (string.IsNullOrWhiteSpace(str2) || lobbyWorldName.ToLower().Contains(str2.ToLower()))
            {
              this.m_gameTypeText.Clear();
              this.m_gameTypeToolTip.Clear();
              float lobbyFloat1 = MyMultiplayerLobby.GetLobbyFloat("inventoryMultiplier", lobby, 1f);
              float lobbyFloat2 = MyMultiplayerLobby.GetLobbyFloat("refineryMultiplier", lobby, 1f);
              float lobbyFloat3 = MyMultiplayerLobby.GetLobbyFloat("assemblerMultiplier", lobby, 1f);
              float lobbyFloat4 = MyMultiplayerLobby.GetLobbyFloat("blocksInventoryMultiplier", lobby, 1f);
              MyGameModeEnum lobbyGameMode = MyMultiplayerLobby.GetLobbyGameMode(lobby);
              if (MyMultiplayerLobby.GetLobbyScenario(lobby))
              {
                this.m_gameTypeText.AppendStringBuilder(MyTexts.Get(MySpaceTexts.WorldSettings_GameScenario));
                DateTime lobbyDateTime = MyMultiplayerLobby.GetLobbyDateTime("scenarioStartTime", lobby, DateTime.MinValue);
                if (lobbyDateTime > DateTime.MinValue)
                {
                  TimeSpan timeSpan = DateTime.UtcNow - lobbyDateTime;
                  double num2 = Math.Truncate(timeSpan.TotalHours);
                  int num3 = (int) ((timeSpan.TotalHours - num2) * 60.0);
                  this.m_gameTypeText.Append(" ").Append(num2).Append(":").Append(num3.ToString("D2"));
                }
                else
                  this.m_gameTypeText.Append(" Lobby");
              }
              else
              {
                switch (lobbyGameMode)
                {
                  case MyGameModeEnum.Creative:
                    if (this.FilterOptions.CreativeMode)
                    {
                      this.m_gameTypeText.AppendStringBuilder(MyTexts.Get(MyCommonTexts.WorldSettings_GameModeCreative));
                      break;
                    }
                    continue;
                  case MyGameModeEnum.Survival:
                    if (this.FilterOptions.SurvivalMode)
                    {
                      this.m_gameTypeText.AppendStringBuilder(MyTexts.Get(MyCommonTexts.WorldSettings_GameModeSurvival));
                      this.m_gameTypeText.Append(string.Format(" {0}-{1}-{2}-{3}", (object) lobbyFloat1, (object) lobbyFloat4, (object) lobbyFloat3, (object) lobbyFloat2));
                      break;
                    }
                    continue;
                }
              }
              this.m_gameTypeToolTip.AppendFormat(MyTexts.Get(MyCommonTexts.JoinGame_GameTypeToolTip_MultipliersFormat).ToString(), (object) lobbyFloat1, (object) lobbyFloat4, (object) lobbyFloat3, (object) lobbyFloat2);
              int lobbyViewDistance = MyMultiplayerLobby.GetLobbyViewDistance(lobby);
              this.m_gameTypeToolTip.AppendLine();
              this.m_gameTypeToolTip.AppendFormat(MyTexts.Get(MyCommonTexts.JoinGame_GameTypeToolTip_ViewDistance).ToString(), (object) lobbyViewDistance);
              if (!string.IsNullOrEmpty(lobbyWorldName) && (!this.FilterOptions.SameVersion || lobbyAppVersion == (int) MyFinalBuildConstants.APP_VERSION) && (!this.FilterOptions.SameData || !MyFakes.ENABLE_MP_DATA_HASHES || MyMultiplayerLobby.HasSameData(lobby)))
              {
                string lobbyHostName = MyMultiplayerLobby.GetLobbyHostName(lobby);
                int memberLimit = lobby.MemberLimit;
                if (memberLimit != 0)
                {
                  string str3 = memberLimit.ToString();
                  string str4 = lobby.MemberCount.ToString() + "/" + str3;
                  if ((!this.FilterOptions.CheckDistance || this.FilterOptions.ViewDistance.ValueBetween((float) MyMultiplayerLobby.GetLobbyViewDistance(lobby))) && (!this.FilterOptions.CheckPlayer || this.FilterOptions.PlayerCount.ValueBetween((float) lobby.MemberCount)) && (!this.FilterOptions.CheckMod || this.FilterOptions.ModCount.ValueBetween((float) lobbyModCount)))
                  {
                    List<MyObjectBuilder_Checkpoint.ModItem> lobbyMods = MyMultiplayerLobby.GetLobbyMods(lobby);
                    if (this.FilterOptions.Mods != null && this.FilterOptions.Mods.Any<WorkshopId>() && this.FilterOptions.AdvancedFilter)
                    {
                      if (this.FilterOptions.ModsExclusive)
                      {
                        bool flag = false;
                        foreach (WorkshopId mod in this.FilterOptions.Mods)
                        {
                          WorkshopId modId = mod;
                          if (lobbyMods == null || !lobbyMods.Any<MyObjectBuilder_Checkpoint.ModItem>((Func<MyObjectBuilder_Checkpoint.ModItem, bool>) (m => (long) m.PublishedFileId == (long) modId.Id && m.PublishedServiceName == modId.ServiceName)))
                          {
                            flag = true;
                            break;
                          }
                        }
                        if (flag)
                          continue;
                      }
                      else if (lobbyMods == null || !lobbyMods.Any<MyObjectBuilder_Checkpoint.ModItem>((Func<MyObjectBuilder_Checkpoint.ModItem, bool>) (m => this.FilterOptions.Mods.Contains(new WorkshopId(m.PublishedFileId, m.PublishedServiceName)))))
                        continue;
                    }
                    StringBuilder stringBuilder = new StringBuilder();
                    int val1 = 15;
                    int num2 = Math.Min(val1, lobbyModCount - 1);
                    foreach (MyObjectBuilder_Checkpoint.ModItem modItem in lobbyMods)
                    {
                      if (val1-- <= 0)
                      {
                        stringBuilder.Append("...");
                        break;
                      }
                      if (num2-- <= 0)
                        stringBuilder.Append(modItem.FriendlyName);
                      else
                        stringBuilder.AppendLine(modItem.FriendlyName);
                    }
                    row.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear().Append(lobbyWorldName), (object) lobby.LobbyId, this.m_textCache.ToString()));
                    row.AddCell(new MyGuiControlTable.Cell(this.m_gameTypeText, toolTip: (this.m_gameTypeToolTip.Length > 0 ? this.m_gameTypeToolTip.ToString() : (string) null)));
                    row.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear().Append(lobbyHostName), toolTip: this.m_textCache.ToString()));
                    if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
                    {
                      if (str1 != null)
                        row.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear().Append(str1)));
                      else if (nullable.HasValue)
                        row.AddCell((MyGuiControlTable.Cell) new MyGuiScreenJoinGame.CellRemainingTime(nullable.Value));
                      else
                        row.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear()));
                    }
                    row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(str4)));
                    if (MyPlatformGameSettings.IsModdingAllowed)
                      row.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear().Append(lobbyModCount == 0 ? "---" : lobbyModCount.ToString()), toolTip: stringBuilder.ToString()));
                    else
                      row.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear()));
                    this.m_gamesTable.Add(row);
                    ++num1;
                  }
                }
              }
            }
          }
        }
        this.m_lobbyPage.Text = new StringBuilder().Append((object) MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Lobbies)).Append(" (").Append(num1).Append(")");
      }
      this.m_gamesTable.SelectedRowIndex = new int?();
    }

    public void RemoveFavoriteServer(MyCachedServerItem server)
    {
      this.m_favoriteServers.Remove(server);
      this.refresh_favorites = true;
    }

    private void InitFavoritesPage()
    {
      this.InitServersTable(MyGameService.ServerDiscovery.PingSupport, MyGameService.ServerDiscovery.GroupSupport);
      this.m_joinButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnJoinServer);
      this.m_refreshButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnRefreshFavoritesServersClick);
      this.RefreshRequest = new Action<MyGuiControlButton>(this.OnRefreshFavoritesServersClick);
      this.m_stopServerRequest = new Action(this.CloseFavoritesRequest);
      this.ShowNetworkingButtons();
      this.m_detailsButton.UserData = (object) this.m_favoriteServers;
      this.m_favoritesResponding = true;
      this.m_searchChangedFunc += new Action(this.RefreshFavoritesGameList);
      this.m_favoritesPage = this.m_selectedPage;
      this.m_favoritesPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Favorites));
      this.RefreshFavoritesGameList();
    }

    private void CloseFavoritesPage()
    {
      this.CloseFavoritesRequest();
      this.m_searchChangedFunc -= new Action(this.RefreshFavoritesGameList);
      this.m_favoritesResponding = false;
    }

    private void OnRefreshFavoritesServersClick(MyGuiControlButton obj) => this.RefreshFavoritesGameList();

    private void RefreshFavoritesGameList()
    {
      this.CloseFavoritesRequest();
      this.m_gamesTable.Clear();
      this.AddServerHeaders(MyGameService.ServerDiscovery.PingSupport);
      this.m_textCache.Clear();
      this.m_gameTypeText.Clear();
      this.m_gameTypeToolTip.Clear();
      this.m_favoriteServers.Clear();
      this.m_favoritesPage.Text = new StringBuilder().Append((object) MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Favorites));
      if (!this.m_enableDedicatedServers)
        return;
      MySandboxGame.Log.WriteLine("Requesting dedicated servers");
      this.m_currentServerDiscovery = this.m_serverDiscovery;
      this.m_currentServerDiscovery.OnFavoritesServerListResponded += new EventHandler<int>(this.OnFavoritesServerListResponded);
      this.m_currentServerDiscovery.OnFavoritesServersCompleteResponse += new EventHandler<MyMatchMakingServerResponse>(this.OnFavoritesServersCompleteResponse);
      MySessionSearchFilter networkFilter = this.FilterOptions.GetNetworkFilter(this.m_currentServerDiscovery.SupportedSearchParameters, this.m_searchBox.SearchText);
      MySandboxGame.Log.WriteLine("Requesting favorite servers, filterOps: " + (object) networkFilter);
      this.m_currentServerDiscovery.RequestFavoritesServerList(networkFilter);
      this.m_loadingWheel.Visible = true;
      this.m_gamesTable.SelectedRowIndex = new int?();
    }

    private void OnFavoritesServerListResponded(object sender, int server)
    {
      MyCachedServerItem serverItem = new MyCachedServerItem(this.m_currentServerDiscovery.GetFavoritesServerDetails(server));
      if (serverItem == null)
        return;
      MyGameService.GetServerRules(serverItem.Server, (ServerRulesResponse) (rules => this.FavoritesRulesResponse(rules, serverItem)), (Action) (() => this.FavoritesRulesResponse((Dictionary<string, string>) null, serverItem)));
    }

    private void FavoritesRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
    {
      if (string.IsNullOrEmpty(server.Server.ConnectionString) || this.m_favoriteServers.Any<MyCachedServerItem>((Func<MyCachedServerItem, bool>) (x => server.Server.ConnectionString.Equals(x.Server.ConnectionString))) || this.m_serverDiscoveryAggregator != null && this.m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) != this.m_currentServerDiscovery)
        return;
      server.Rules = rules;
      if (rules != null)
        server.DeserializeSettings();
      this.m_favoriteServers.Add(server);
      if (!this.m_favoritesResponding)
        return;
      server.Server.IsRanked = this.IsRanked(server);
      this.AddServerItem(server);
      if (this.m_refreshPaused)
        return;
      this.m_favoritesPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Favorites)).Append(" (").Append(this.m_gamesTable.RowsCount).Append(")");
    }

    private void RebuildFavoritesList()
    {
      this.m_detailsButton.Enabled = false;
      this.m_joinButton.Enabled = false;
      this.m_gamesTable.Clear();
      foreach (MyCachedServerItem favoriteServer in this.m_favoriteServers)
        this.AddServerItem(favoriteServer);
      this.m_favoritesPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Favorites)).Append(" (").Append(this.m_gamesTable.RowsCount).Append(")");
    }

    private void OnFavoritesServersCompleteResponse(
      object sender,
      MyMatchMakingServerResponse response)
    {
      this.CloseFavoritesRequest();
    }

    private void CloseFavoritesRequest()
    {
      if (this.m_currentServerDiscovery != null)
      {
        this.m_currentServerDiscovery.OnFavoritesServerListResponded -= new EventHandler<int>(this.OnFavoritesServerListResponded);
        this.m_currentServerDiscovery.OnFavoritesServersCompleteResponse -= new EventHandler<MyMatchMakingServerResponse>(this.OnFavoritesServersCompleteResponse);
        this.m_currentServerDiscovery.CancelFavoritesServersRequest();
      }
      this.m_loadingWheel.Visible = false;
    }

    private void InitHistoryPage()
    {
      this.InitServersTable(MyGameService.ServerDiscovery.PingSupport, MyGameService.ServerDiscovery.GroupSupport);
      this.m_joinButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnJoinServer);
      this.m_refreshButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnRefreshHistoryServersClick);
      this.RefreshRequest = new Action<MyGuiControlButton>(this.OnRefreshHistoryServersClick);
      this.m_stopServerRequest = new Action(this.CloseHistoryRequest);
      this.ShowNetworkingButtons();
      this.m_historyResponding = true;
      this.m_searchChangedFunc += new Action(this.RefreshHistoryGameList);
      this.m_historyPage = this.m_selectedPage;
      this.m_historyPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_History));
      this.m_detailsButton.UserData = (object) this.m_historyServers;
      this.RefreshHistoryGameList();
    }

    private void CloseHistoryPage()
    {
      this.CloseHistoryRequest();
      this.m_historyResponding = false;
      this.m_searchChangedFunc -= new Action(this.RefreshHistoryGameList);
    }

    private void OnRefreshHistoryServersClick(MyGuiControlButton obj) => this.RefreshHistoryGameList();

    private void RefreshHistoryGameList()
    {
      this.CloseHistoryRequest();
      this.m_gamesTable.Clear();
      this.AddServerHeaders(MyGameService.ServerDiscovery.PingSupport);
      this.m_textCache.Clear();
      this.m_gameTypeText.Clear();
      this.m_gameTypeToolTip.Clear();
      this.m_historyServers.Clear();
      this.m_historyPage.Text = new StringBuilder().Append((object) MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_History));
      if (!this.m_enableDedicatedServers)
        return;
      MySandboxGame.Log.WriteLine("Requesting dedicated servers");
      this.m_currentServerDiscovery = this.m_serverDiscovery;
      this.m_currentServerDiscovery.OnHistoryServerListResponded += new EventHandler<int>(this.OnHistoryServerListResponded);
      this.m_currentServerDiscovery.OnHistoryServersCompleteResponse += new EventHandler<MyMatchMakingServerResponse>(this.OnHistoryServersCompleteResponse);
      MySessionSearchFilter networkFilter = this.FilterOptions.GetNetworkFilter(this.m_currentServerDiscovery.SupportedSearchParameters, this.m_searchBox.SearchText);
      MySandboxGame.Log.WriteLine("Requesting history servers, filterOps: " + (object) networkFilter);
      this.m_currentServerDiscovery.RequestHistoryServerList(networkFilter);
      this.m_loadingWheel.Visible = true;
      this.m_gamesTable.SelectedRowIndex = new int?();
    }

    private void OnHistoryServerListResponded(object sender, int server)
    {
      MyCachedServerItem serverItem = new MyCachedServerItem(this.m_currentServerDiscovery.GetHistoryServerDetails(server));
      if (serverItem == null)
        return;
      MyGameService.GetServerRules(serverItem.Server, (ServerRulesResponse) (rules => this.HistoryRulesResponse(rules, serverItem)), (Action) (() => this.HistoryRulesResponse((Dictionary<string, string>) null, serverItem)));
    }

    private void HistoryRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
    {
      if (string.IsNullOrEmpty(server.Server.ConnectionString) || this.m_historyServers.Any<MyCachedServerItem>((Func<MyCachedServerItem, bool>) (x => server.Server.ConnectionString.Equals(x.Server.ConnectionString))) || this.m_serverDiscoveryAggregator != null && this.m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) != this.m_currentServerDiscovery)
        return;
      server.Rules = rules;
      if (rules != null)
        server.DeserializeSettings();
      this.m_historyServers.Add(server);
      if (!this.m_historyResponding)
        return;
      server.Server.IsRanked = this.IsRanked(server);
      this.AddServerItem(server);
      if (this.m_refreshPaused)
        return;
      this.m_historyPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_History)).Append(" (").Append(this.m_gamesTable.RowsCount).Append(")");
    }

    private void OnHistoryServersCompleteResponse(
      object sender,
      MyMatchMakingServerResponse response)
    {
      this.CloseHistoryRequest();
    }

    private void CloseHistoryRequest()
    {
      if (this.m_currentServerDiscovery != null)
      {
        this.m_currentServerDiscovery.OnHistoryServerListResponded -= new EventHandler<int>(this.OnHistoryServerListResponded);
        this.m_currentServerDiscovery.OnHistoryServersCompleteResponse -= new EventHandler<MyMatchMakingServerResponse>(this.OnHistoryServersCompleteResponse);
        this.m_currentServerDiscovery.CancelHistoryServersRequest();
      }
      this.m_loadingWheel.Visible = false;
    }

    private void InitLANPage()
    {
      this.InitServersTable(MyGameService.ServerDiscovery.PingSupport, MyGameService.ServerDiscovery.GroupSupport);
      this.m_joinButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnJoinServer);
      this.m_refreshButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnRefreshLANServersClick);
      this.RefreshRequest = new Action<MyGuiControlButton>(this.OnRefreshLANServersClick);
      this.m_stopServerRequest = new Action(this.CloseLANRequest);
      this.ShowNetworkingButtons();
      this.m_detailsButton.UserData = (object) this.m_lanServers;
      this.m_lanResponding = true;
      this.m_searchChangedFunc += new Action(this.RefreshLANGameList);
      this.m_LANPage = this.m_selectedPage;
      this.m_LANPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_LAN));
      this.RefreshLANGameList();
    }

    private void CloseLANPage()
    {
      this.CloseLANRequest();
      this.m_lanResponding = false;
      this.m_searchChangedFunc -= new Action(this.RefreshLANGameList);
    }

    private void OnRefreshLANServersClick(MyGuiControlButton obj) => this.RefreshLANGameList();

    private void RefreshLANGameList()
    {
      this.CloseLANRequest();
      this.m_gamesTable.Clear();
      this.AddServerHeaders(MyGameService.ServerDiscovery.PingSupport);
      this.m_textCache.Clear();
      this.m_gameTypeText.Clear();
      this.m_gameTypeToolTip.Clear();
      this.m_lanServers.Clear();
      this.m_LANPage.Text = new StringBuilder().Append((object) MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_LAN));
      MySandboxGame.Log.WriteLine("Requesting dedicated servers");
      this.m_currentServerDiscovery = this.m_serverDiscovery;
      this.m_currentServerDiscovery.OnLANServerListResponded += new EventHandler<int>(this.OnLANServerListResponded);
      this.m_currentServerDiscovery.OnLANServersCompleteResponse += new EventHandler<MyMatchMakingServerResponse>(this.OnLANServersCompleteResponse);
      this.m_currentServerDiscovery.RequestLANServerList();
      this.m_loadingWheel.Visible = true;
      this.m_gamesTable.SelectedRowIndex = new int?();
    }

    private void OnLANServerListResponded(object sender, int server)
    {
      MyCachedServerItem serverItem = new MyCachedServerItem(this.m_currentServerDiscovery.GetLANServerDetails(server));
      if (serverItem == null)
        return;
      MyGameService.GetServerRules(serverItem.Server, (ServerRulesResponse) (rules => this.LanRulesResponse(rules, serverItem)), (Action) (() => this.LanRulesResponse((Dictionary<string, string>) null, serverItem)));
    }

    private void LanRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
    {
      if (string.IsNullOrEmpty(server.Server.ConnectionString) || this.m_lanServers.Any<MyCachedServerItem>((Func<MyCachedServerItem, bool>) (x => server.Server.ConnectionString.Equals(x.Server.ConnectionString))) || this.m_serverDiscoveryAggregator != null && this.m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) != this.m_currentServerDiscovery)
        return;
      server.Rules = rules;
      if (rules != null)
        server.DeserializeSettings();
      this.m_lanServers.Add(server);
      if (!this.m_lanResponding)
        return;
      server.Server.IsRanked = this.IsRanked(server);
      this.AddServerItem(server);
      if (this.m_refreshPaused)
        return;
      this.m_LANPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_LAN)).Append(" (").Append(this.m_gamesTable.RowsCount).Append(")");
    }

    private void OnLANServersCompleteResponse(object sender, MyMatchMakingServerResponse response) => this.CloseLANRequest();

    private void CloseLANRequest()
    {
      if (this.m_currentServerDiscovery != null)
      {
        this.m_currentServerDiscovery.OnLANServerListResponded -= new EventHandler<int>(this.OnLANServerListResponded);
        this.m_currentServerDiscovery.OnLANServersCompleteResponse -= new EventHandler<MyMatchMakingServerResponse>(this.OnLANServersCompleteResponse);
        this.m_currentServerDiscovery.CancelLANServersRequest();
      }
      this.m_loadingWheel.Visible = false;
    }

    private void InitFriendsPage()
    {
      this.InitServersTable(MyGameService.ServerDiscovery.PingSupport, MyGameService.ServerDiscovery.GroupSupport);
      this.m_joinButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnJoinServer);
      this.m_refreshButton.ButtonClicked += new Action<MyGuiControlButton>(this.OnRefreshFriendsServersClick);
      this.RefreshRequest = new Action<MyGuiControlButton>(this.OnRefreshFriendsServersClick);
      this.m_stopServerRequest = new Action(this.CloseFriendsRequest);
      this.ShowNetworkingButtons();
      this.m_searchChangedFunc += new Action(this.RefreshFriendsGameList);
      this.m_detailsButton.UserData = (object) this.m_dedicatedServers;
      this.m_friendsPage = this.m_selectedPage;
      this.m_friendsPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Friends));
      if (this.m_friendIds == null)
      {
        this.m_friendIds = new HashSet<ulong>();
        this.m_friendNames = new HashSet<string>();
        this.RequestFriendsList();
      }
      this.RefreshFriendsGameList();
    }

    private void CloseFriendsPage()
    {
      this.CloseFriendsRequest();
      this.m_searchChangedFunc -= new Action(this.RefreshFriendsGameList);
    }

    private void OnRefreshFriendsServersClick(MyGuiControlButton obj) => this.RefreshFriendsGameList();

    private void RequestFriendsList()
    {
      DateTime now = DateTime.Now;
      int friendsCount = MyGameService.GetFriendsCount();
      for (int index = 0; index < friendsCount; ++index)
      {
        ulong friendIdByIndex = MyGameService.GetFriendIdByIndex(index);
        string friendNameByIndex = MyGameService.GetFriendNameByIndex(index);
        this.m_friendIds.Add(friendIdByIndex);
        this.m_friendNames.Add(friendNameByIndex);
      }
    }

    private void RefreshFriendsGameList()
    {
      this.CloseFriendsRequest();
      this.m_gamesTable.Clear();
      this.AddServerHeaders(MyGameService.ServerDiscovery.PingSupport);
      this.m_textCache.Clear();
      this.m_gameTypeText.Clear();
      this.m_gameTypeToolTip.Clear();
      this.m_friendsPage.Text = new StringBuilder().Append((object) MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Friends));
      MySandboxGame.Log.WriteLine("Requesting dedicated servers");
      this.CloseFriendsRequest();
      if (this.FilterOptions.SameVersion)
        MyGameService.AddLobbyFilter("appVersion", MyFinalBuildConstants.APP_VERSION.ToString());
      MyGameService.RequestLobbyList(new Action<bool>(this.FriendsLobbyResponse));
      this.m_dedicatedServers.Clear();
      this.m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.Refresh);
      this.m_nextState = MyGuiScreenJoinGame.RefreshStateEnum.Pause;
      this.m_refreshPaused = false;
      if (this.m_enableDedicatedServers)
      {
        MySessionSearchFilter networkFilter = this.FilterOptions.GetNetworkFilter(this.m_currentServerDiscovery.SupportedSearchParameters, this.m_searchBox.SearchText);
        MySandboxGame.Log.WriteLine("Requesting dedicated servers, filterOps: " + (object) networkFilter);
        this.m_currentServerDiscovery = this.m_serverDiscovery;
        this.m_currentServerDiscovery.OnDedicatedServerListResponded += new EventHandler<int>(this.OnFriendsServerListResponded);
        this.m_currentServerDiscovery.OnDedicatedServersCompleteResponse += new EventHandler<MyMatchMakingServerResponse>(this.OnFriendsServersCompleteResponse);
        this.m_currentServerDiscovery.RequestInternetServerList(networkFilter);
      }
      this.m_loadingWheel.Visible = true;
      this.m_gamesTable.SelectedRowIndex = new int?();
    }

    private void FriendsLobbyResponse(bool success)
    {
      if (!success)
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder("Cannot enumerate worlds"), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
      this.m_lobbies.Clear();
      MyGameService.AddFriendLobbies(this.m_lobbies);
      MyGameService.AddPublicLobbies(this.m_lobbies);
      foreach (IMyLobby lobby in this.m_lobbies)
      {
        if (this.m_friendIds.Contains(lobby.OwnerId) || this.m_friendIds.Contains(MyMultiplayerLobby.GetLobbyHostSteamId(lobby)) || this.m_friendIds.Contains(lobby.LobbyId) || lobby.MemberList != null && lobby.MemberList.Any<ulong>((Func<ulong, bool>) (m => this.m_friendIds.Contains(m))))
        {
          lock (this.m_friendsPage)
            this.AddFriendLobby(lobby);
        }
      }
    }

    private void OnFriendsServerListResponded(object sender, int server)
    {
      MyCachedServerItem serverItem = new MyCachedServerItem(this.m_currentServerDiscovery.GetDedicatedServerDetails(server));
      if (serverItem == null || serverItem.Server.Players <= 0)
        return;
      MyGameService.GetPlayerDetails(serverItem.Server, (PlayerDetailsResponse) (players => this.LoadPlayersCompleted(server, players, serverItem)), (Action) (() => this.LoadPlayersCompleted(server, (Dictionary<string, float>) null, serverItem)));
    }

    private void LoadPlayersCompleted(
      int server,
      Dictionary<string, float> players,
      MyCachedServerItem serverItem)
    {
      if (players == null || !players.Keys.Any<string>((Func<string, bool>) (n => this.m_friendNames.Contains(n))))
        return;
      MyGameService.GetServerRules(serverItem.Server, (ServerRulesResponse) (rules => this.FriendRulesResponse(rules, serverItem)), (Action) (() => this.FriendRulesResponse((Dictionary<string, string>) null, serverItem)));
    }

    private void FriendRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
    {
      if (string.IsNullOrEmpty(server.Server.ConnectionString) || this.m_dedicatedServers.Any<MyCachedServerItem>((Func<MyCachedServerItem, bool>) (x => server.Server.ConnectionString.Equals(x.Server.ConnectionString))) || this.m_serverDiscoveryAggregator != null && this.m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) != this.m_currentServerDiscovery)
        return;
      server.Rules = rules;
      if (rules != null)
        server.DeserializeSettings();
      this.m_dedicatedServers.Add(server);
      server.Server.IsRanked = this.IsRanked(server);
      lock (this.m_friendsPage)
      {
        this.AddServerItem(server);
        this.m_friendsPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Friends)).Append(" (").Append(this.m_gamesTable.RowsCount).Append(")");
      }
    }

    private void OnFriendsServersCompleteResponse(
      object sender,
      MyMatchMakingServerResponse response)
    {
      this.CloseFriendsRequest();
    }

    private void CloseFriendsRequest()
    {
      if (this.m_currentServerDiscovery != null)
      {
        this.m_currentServerDiscovery.OnDedicatedServerListResponded -= new EventHandler<int>(this.OnFriendsServerListResponded);
        this.m_currentServerDiscovery.OnDedicatedServersCompleteResponse -= new EventHandler<MyMatchMakingServerResponse>(this.OnFriendsServersCompleteResponse);
        this.m_currentServerDiscovery.CancelInternetServersRequest();
      }
      this.m_loadingWheel.Visible = false;
    }

    private void AddFriendLobby(IMyLobby lobby)
    {
      if (this.FilterOptions.AdvancedFilter && !this.FilterOptions.FilterLobby(lobby))
        return;
      string lobbyWorldName = MyMultiplayerLobby.GetLobbyWorldName(lobby);
      long lobbyWorldSize = (long) MyMultiplayerLobby.GetLobbyWorldSize(lobby);
      int lobbyAppVersion = MyMultiplayerLobby.GetLobbyAppVersion(lobby);
      int lobbyModCount = MyMultiplayerLobby.GetLobbyModCount(lobby);
      string str1 = this.m_searchBox.SearchText.Trim();
      if (!string.IsNullOrWhiteSpace(str1) && !lobbyWorldName.ToLower().Contains(str1.ToLower()))
        return;
      this.m_gameTypeText.Clear();
      this.m_gameTypeToolTip.Clear();
      float lobbyFloat1 = MyMultiplayerLobby.GetLobbyFloat("blocksInventoryMultiplier", lobby, 1f);
      float lobbyFloat2 = MyMultiplayerLobby.GetLobbyFloat("inventoryMultiplier", lobby, 1f);
      float lobbyFloat3 = MyMultiplayerLobby.GetLobbyFloat("refineryMultiplier", lobby, 1f);
      float lobbyFloat4 = MyMultiplayerLobby.GetLobbyFloat("assemblerMultiplier", lobby, 1f);
      MyGameModeEnum lobbyGameMode = MyMultiplayerLobby.GetLobbyGameMode(lobby);
      if (MyMultiplayerLobby.GetLobbyScenario(lobby))
      {
        this.m_gameTypeText.AppendStringBuilder(MyTexts.Get(MySpaceTexts.WorldSettings_GameScenario));
        DateTime lobbyDateTime = MyMultiplayerLobby.GetLobbyDateTime("scenarioStartTime", lobby, DateTime.MinValue);
        if (lobbyDateTime > DateTime.MinValue)
        {
          TimeSpan timeSpan = DateTime.UtcNow - lobbyDateTime;
          double num1 = Math.Truncate(timeSpan.TotalHours);
          int num2 = (int) ((timeSpan.TotalHours - num1) * 60.0);
          this.m_gameTypeText.Append(" ").Append(num1).Append(":").Append(num2.ToString("D2"));
        }
        else
          this.m_gameTypeText.Append(" Lobby");
      }
      else
      {
        switch (lobbyGameMode)
        {
          case MyGameModeEnum.Creative:
            if (!this.FilterOptions.CreativeMode)
              return;
            this.m_gameTypeText.AppendStringBuilder(MyTexts.Get(MyCommonTexts.WorldSettings_GameModeCreative));
            break;
          case MyGameModeEnum.Survival:
            if (!this.FilterOptions.SurvivalMode)
              return;
            this.m_gameTypeText.AppendStringBuilder(MyTexts.Get(MyCommonTexts.WorldSettings_GameModeSurvival));
            this.m_gameTypeText.Append(string.Format(" {0}-{1}-{2}-{3}", (object) lobbyFloat2, (object) lobbyFloat1, (object) lobbyFloat4, (object) lobbyFloat3));
            break;
        }
      }
      this.m_gameTypeToolTip.AppendFormat(MyTexts.Get(MyCommonTexts.JoinGame_GameTypeToolTip_MultipliersFormat).ToString(), (object) lobbyFloat2, (object) lobbyFloat1, (object) lobbyFloat4, (object) lobbyFloat3);
      int lobbyViewDistance = MyMultiplayerLobby.GetLobbyViewDistance(lobby);
      this.m_gameTypeToolTip.AppendLine();
      this.m_gameTypeToolTip.AppendFormat(MyTexts.Get(MyCommonTexts.JoinGame_GameTypeToolTip_ViewDistance).ToString(), (object) lobbyViewDistance);
      if (string.IsNullOrEmpty(lobbyWorldName) || this.FilterOptions.SameVersion && lobbyAppVersion != (int) MyFinalBuildConstants.APP_VERSION || this.FilterOptions.SameData && MyFakes.ENABLE_MP_DATA_HASHES && !MyMultiplayerLobby.HasSameData(lobby))
        return;
      string lobbyHostName = MyMultiplayerLobby.GetLobbyHostName(lobby);
      string str2 = lobby.MemberLimit.ToString();
      string str3 = lobby.MemberCount.ToString() + "/" + str2;
      if (this.FilterOptions.CheckDistance && !this.FilterOptions.ViewDistance.ValueBetween((float) MyMultiplayerLobby.GetLobbyViewDistance(lobby)) || this.FilterOptions.CheckPlayer && !this.FilterOptions.PlayerCount.ValueBetween((float) lobby.MemberCount) || this.FilterOptions.CheckMod && !this.FilterOptions.ModCount.ValueBetween((float) lobbyModCount))
        return;
      List<MyObjectBuilder_Checkpoint.ModItem> lobbyMods = MyMultiplayerLobby.GetLobbyMods(lobby);
      if (this.FilterOptions.Mods != null && this.FilterOptions.Mods.Any<WorkshopId>() && this.FilterOptions.AdvancedFilter)
      {
        if (this.FilterOptions.ModsExclusive)
        {
          bool flag = false;
          foreach (WorkshopId mod in this.FilterOptions.Mods)
          {
            WorkshopId modId = mod;
            if (lobbyMods == null || !lobbyMods.Any<MyObjectBuilder_Checkpoint.ModItem>((Func<MyObjectBuilder_Checkpoint.ModItem, bool>) (m => (long) m.PublishedFileId == (long) modId.Id && m.PublishedServiceName == modId.ServiceName)))
            {
              flag = true;
              break;
            }
          }
          if (flag)
            return;
        }
        else if (lobbyMods == null || !lobbyMods.Any<MyObjectBuilder_Checkpoint.ModItem>((Func<MyObjectBuilder_Checkpoint.ModItem, bool>) (m => this.FilterOptions.Mods.Contains(new WorkshopId(m.PublishedFileId, m.PublishedServiceName)))))
          return;
      }
      StringBuilder stringBuilder = new StringBuilder();
      int val1 = 15;
      int num = Math.Min(val1, lobbyModCount - 1);
      foreach (MyObjectBuilder_Checkpoint.ModItem modItem in lobbyMods)
      {
        if (val1-- <= 0)
        {
          stringBuilder.Append("...");
          break;
        }
        if (num-- <= 0)
          stringBuilder.Append(modItem.FriendlyName);
        else
          stringBuilder.AppendLine(modItem.FriendlyName);
      }
      MyGuiControlTable.Row row = new MyGuiControlTable.Row((object) lobby);
      string toolTip = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Rank).ToString();
      row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), toolTip: toolTip, iconOriginAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
      row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), iconOriginAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
      row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), iconOriginAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
      row.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear().Append(lobbyWorldName), (object) lobby.LobbyId, this.m_textCache.ToString()));
      row.AddCell(new MyGuiControlTable.Cell(this.m_gameTypeText, toolTip: (this.m_gameTypeToolTip.Length > 0 ? this.m_gameTypeToolTip.ToString() : (string) null)));
      row.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear()));
      row.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear().Append(lobbyHostName), toolTip: this.m_textCache.ToString()));
      row.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear().Append(str3)));
      row.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear().Append("---")));
      row.AddCell(new MyGuiControlTable.Cell(this.m_textCache.Clear().Append(lobbyModCount == 0 ? "---" : lobbyModCount.ToString()), toolTip: stringBuilder.ToString()));
      this.m_gamesTable.Add(row);
      this.m_friendsPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Friends)).Append(" (").Append(this.m_gamesTable.RowsCount).Append(")");
    }

    public override void CloseScreenNow(bool isUnloading = false)
    {
      this.m_searchChangedFunc = (Action) null;
      base.CloseScreenNow(isUnloading);
    }

    private class CellRemainingTime : MyGuiControlTable.Cell
    {
      private readonly DateTime m_timeEstimatedEnd;

      public CellRemainingTime(float remainingTime)
        : base("")
      {
        this.m_timeEstimatedEnd = DateTime.UtcNow + TimeSpan.FromSeconds((double) remainingTime);
        this.FillText();
      }

      public override void Update()
      {
        base.Update();
        this.FillText();
      }

      private void FillText()
      {
        TimeSpan timeSpan = this.m_timeEstimatedEnd - DateTime.UtcNow;
        if (timeSpan < TimeSpan.Zero)
          timeSpan = TimeSpan.Zero;
        this.Text.Clear().Append(timeSpan.ToString("mm\\:ss"));
      }
    }

    private enum RefreshStateEnum
    {
      Pause,
      Resume,
      Refresh,
    }

    private enum ContextMenuFavoriteAction
    {
      Add,
      Remove,
    }

    private struct ContextMenuFavoriteActionItem
    {
      public MyGameServerItem Server;
      public MyGuiScreenJoinGame.ContextMenuFavoriteAction _Action;
    }
  }
}
