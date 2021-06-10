// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenServerDetailsBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public abstract class MyGuiScreenServerDetailsBase : MyGuiScreenBase
  {
    private MyGuiControlButton m_btSettings;
    private MyGuiControlButton m_btMods;
    private MyGuiControlButton m_btPlayers;
    private MyGuiControlRotatingWheel m_loadingWheel;
    private bool m_serverIsFavorited;
    protected MyGuiScreenServerDetailsBase.DetailsPageEnum m_currentPage;
    protected Vector2 m_currentPosition;
    protected List<MyWorkshopItem> m_mods;
    protected float m_padding = 0.02f;
    protected Dictionary<string, float> m_players;
    protected MyCachedServerItem m_server;
    private MyGuiControlButton m_btnJoin;
    private MyGuiControlButton m_btnAddFavorite;
    private MyGuiControlButton m_btnRemoveFavorite;
    private const int PAGE_COUNT = 3;

    protected MyObjectBuilder_SessionSettings Settings => this.m_server.Settings;

    protected MyGuiScreenServerDetailsBase(MyCachedServerItem server)
      : base(new Vector2?(new Vector2(0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.6535714f, 0.9398855f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_server = server;
      this.CreateScreen();
      this.TestFavoritesGameList();
    }

    private void CreateScreen()
    {
      this.CanHideOthers = true;
      this.CanBeHidden = true;
      this.EnabledBackgroundFade = true;
      this.CloseButtonEnabled = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyCommonTexts.JoinGame_ServerDetails, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      MyGuiControlSeparatorList controlSeparatorList3 = new MyGuiControlSeparatorList();
      controlSeparatorList3.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.150000005960464)), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList3);
      MyGuiControlSeparatorList controlSeparatorList4 = new MyGuiControlSeparatorList();
      float num = 0.303f;
      if (this.m_server.ExperimentalMode)
        num = 0.34f;
      controlSeparatorList4.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0), this.m_size.Value.Y / 2f - num), this.m_size.Value.X * 0.835f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList4);
      this.m_currentPosition = new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.834999978542328 / 2.0 - 3.0 / 1000.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.11599999666214));
      this.DrawButtons();
      this.m_loadingWheel = new MyGuiControlRotatingWheel(new Vector2?(this.m_btPlayers.Position + new Vector2(0.137f, -0.004f)), new Vector4?(MyGuiConstants.ROTATING_WHEEL_COLOR), 0.2f);
      this.Controls.Add((MyGuiControlBase) this.m_loadingWheel);
      this.m_loadingWheel.Visible = false;
      if (!this.m_serverIsFavorited)
      {
        this.m_btnAddFavorite = new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.0f) - new Vector2(-3f / 1000f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0710000023245811))), text: MyTexts.Get(MyCommonTexts.ServerDetails_AddFavorite));
        this.m_btnAddFavorite.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_AddFavorite));
        this.m_btnAddFavorite.ButtonClicked += new Action<MyGuiControlButton>(this.FavoriteButtonClick);
        this.m_btnAddFavorite.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.Controls.Add((MyGuiControlBase) this.m_btnAddFavorite);
      }
      else
      {
        this.m_btnRemoveFavorite = new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.0f) - new Vector2(-3f / 1000f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0710000023245811))), text: MyTexts.Get(MyCommonTexts.ServerDetails_RemoveFavorite));
        this.m_btnRemoveFavorite.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_RemoveFavorite));
        this.m_btnRemoveFavorite.ButtonClicked += new Action<MyGuiControlButton>(this.UnFavoriteButtonClick);
        this.m_btnRemoveFavorite.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.Controls.Add((MyGuiControlBase) this.m_btnRemoveFavorite);
      }
      this.m_btnJoin = new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.0f) - new Vector2(0.18f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0710000023245811))), text: MyTexts.Get(MyCommonTexts.JoinGame_Title));
      this.m_btnJoin.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGame_JoinWorld));
      this.m_btnJoin.ButtonClicked += new Action<MyGuiControlButton>(this.ConnectButtonClick);
      this.m_btnJoin.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_btnJoin);
      this.m_currentPosition.Y += 0.012f;
      this.AddLabel(MyCommonTexts.ServerDetails_Server, (object) this.m_server.Server.Name);
      this.AddLabel(MyCommonTexts.ServerDetails_Map, (object) this.m_server.Server.Map);
      this.AddLabel(MyCommonTexts.ServerDetails_Version, (object) new MyVersion((int) this.m_server.Server.GetGameTagByPrefixUlong("version")).FormattedText.ToString().Replace("_", "."));
      this.AddLabel(MyCommonTexts.ServerDetails_IPAddress, (object) this.m_server.Server.ConnectionString);
      if (this.m_server.ExperimentalMode)
        this.AddLabel(MyCommonTexts.ServerIsExperimental);
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_btnJoin.Position.X - MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui.X / 2f, this.m_btnJoin.Position.Y)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      if (!this.m_serverIsFavorited)
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.ServerDetails_Help_ScreenAddFavorites);
      else
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.ServerDetails_Help_ScreenRemoveFavorites);
      this.m_currentPosition.Y += 0.028f;
      switch (this.m_currentPage)
      {
        case MyGuiScreenServerDetailsBase.DetailsPageEnum.Settings:
          this.FocusedControl = (MyGuiControlBase) this.m_btSettings;
          this.m_btSettings.Checked = true;
          this.m_btSettings.Selected = true;
          this.DrawSettings();
          break;
        case MyGuiScreenServerDetailsBase.DetailsPageEnum.Mods:
          this.FocusedControl = (MyGuiControlBase) this.m_btMods;
          this.m_btMods.Checked = true;
          this.m_btMods.Selected = true;
          this.DrawMods();
          break;
        case MyGuiScreenServerDetailsBase.DetailsPageEnum.Players:
          this.FocusedControl = (MyGuiControlBase) this.m_btPlayers;
          this.m_btPlayers.Checked = true;
          this.m_btPlayers.Selected = true;
          this.DrawPlayers();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public override bool Draw()
    {
      base.Draw();
      if (MyInput.Static.IsJoystickLastUsed)
      {
        MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
        Vector2 positionAbsoluteTopLeft = this.m_btSettings.GetPositionAbsoluteTopLeft();
        Vector2 size = this.m_btSettings.Size;
        Vector2 normalizedCoord1 = positionAbsoluteTopLeft;
        normalizedCoord1.Y += size.Y / 2f;
        normalizedCoord1.X -= size.X / 6f;
        Vector2 normalizedCoord2 = positionAbsoluteTopLeft;
        normalizedCoord2.Y = normalizedCoord1.Y;
        Color color = MyGuiControlBase.ApplyColorMaskModifiers(MyGuiConstants.LABEL_TEXT_COLOR, true, this.m_transitionAlpha);
        normalizedCoord2.X += (float) (3.0 * (double) size.X + (double) size.X / 6.0);
        MyGuiManager.DrawString("Blue", MyTexts.Get(MyCommonTexts.Gamepad_Help_TabControl_Left).ToString(), normalizedCoord1, 1f, new Color?(color), drawAlign);
        MyGuiManager.DrawString("Blue", MyTexts.Get(MyCommonTexts.Gamepad_Help_TabControl_Right).ToString(), normalizedCoord2, 1f, new Color?(color), drawAlign);
      }
      return true;
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (receivedFocusInThisUpdate)
        return;
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.ParseIPAndConnect();
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
      {
        if (!this.m_serverIsFavorited)
          this.FavoriteButtonClick((MyGuiControlButton) null);
        else
          this.UnFavoriteButtonClick((MyGuiControlButton) null);
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT))
        this.ChangeCurentPage(false);
      else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
        this.ChangeCurentPage(true);
      this.m_btnJoin.Visible = !MyInput.Static.IsJoystickLastUsed;
      if (this.m_btnAddFavorite != null)
        this.m_btnAddFavorite.Visible = !MyInput.Static.IsJoystickLastUsed;
      if (this.m_btnRemoveFavorite == null)
        return;
      this.m_btnRemoveFavorite.Visible = !MyInput.Static.IsJoystickLastUsed;
    }

    protected abstract void DrawSettings();

    private void ChangeCurentPage(bool forward)
    {
      int num = (int) (this.m_currentPage + (forward ? 1 : -1)) % 3;
      this.m_currentPage = num < 0 ? MyGuiScreenServerDetailsBase.DetailsPageEnum.Players : (MyGuiScreenServerDetailsBase.DetailsPageEnum) num;
      switch (this.m_currentPage)
      {
        case MyGuiScreenServerDetailsBase.DetailsPageEnum.Mods:
          if (this.m_btMods.Enabled)
          {
            this.ModsButtonClick((MyGuiControlButton) null);
            break;
          }
          this.ChangeCurentPage(forward);
          break;
        case MyGuiScreenServerDetailsBase.DetailsPageEnum.Players:
          this.PlayersButtonClick((MyGuiControlButton) null);
          break;
        default:
          this.SettingButtonClick((MyGuiControlButton) null);
          break;
      }
    }

    private void DrawMods()
    {
      if (this.m_mods != null && this.m_mods.Count > 0)
      {
        double byteSize = (double) this.m_mods.Sum<MyWorkshopItem>((Func<MyWorkshopItem, long>) (m => (long) m.Size));
        string str = MyUtils.FormatByteSizePrefix(ref byteSize);
        this.AddLabel(MyCommonTexts.ServerDetails_ModDownloadSize, (object) (byteSize.ToString("0.") + " " + str + "B"));
      }
      this.AddLabel(MyCommonTexts.WorldSettings_Mods, (object) null);
      if (this.m_mods == null)
        this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: MyTexts.GetString(MyCommonTexts.ServerDetails_ModError), font: "Red"));
      else if (this.m_mods.Count == 0)
      {
        this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: MyTexts.GetString(MyCommonTexts.ServerDetails_NoMods)));
      }
      else
      {
        this.m_mods.Sort((Comparison<MyWorkshopItem>) ((a, b) => string.Compare(a.Title, b.Title, StringComparison.CurrentCultureIgnoreCase)));
        MyGuiControlParent guiControlParent = new MyGuiControlParent();
        MyGuiControlScrollablePanel controlScrollablePanel = new MyGuiControlScrollablePanel((MyGuiControlBase) guiControlParent);
        controlScrollablePanel.ScrollbarVEnabled = true;
        controlScrollablePanel.Position = this.m_currentPosition;
        controlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        controlScrollablePanel.Size = new Vector2(this.Size.Value.X - 0.112f, (float) ((double) this.Size.Value.Y / 2.0 - (double) this.m_currentPosition.Y - 0.144999995827675));
        controlScrollablePanel.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
        controlScrollablePanel.ScrolledAreaPadding = new MyGuiBorderThickness(0.005f);
        this.Controls.Add((MyGuiControlBase) controlScrollablePanel);
        MyGuiControlButton guiControlButton1 = new MyGuiControlButton(visualStyle: MyGuiControlButtonStyleEnum.Close);
        guiControlParent.Size = new Vector2(controlScrollablePanel.Size.X, (float) ((double) this.m_mods.Count * ((double) guiControlButton1.Size.Y / 2.0 + (double) this.m_padding) + (double) guiControlButton1.Size.Y / 2.0));
        Vector2 vector2 = new Vector2((float) (-(double) controlScrollablePanel.Size.X / 2.0), (float) (-(double) guiControlParent.Size.Y / 2.0));
        foreach (MyWorkshopItem mod in this.m_mods)
        {
          MyGuiControlButton guiControlButton2 = new MyGuiControlButton(new Vector2?(vector2), MyGuiControlButtonStyleEnum.ClickableText, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: new StringBuilder(mod.Title));
          guiControlButton2.UserData = (object) mod;
          int num1 = Math.Min(mod.Description.Length, 128);
          int num2 = mod.Description.IndexOf("\n");
          if (num2 > 0)
            num1 = Math.Min(num1, num2 - 1);
          guiControlButton2.SetToolTip(mod.Description.Substring(0, num1));
          guiControlButton2.ButtonClicked += new Action<MyGuiControlButton>(this.ModURLClick);
          vector2.Y += guiControlButton2.Size.Y / 2f + this.m_padding;
          guiControlParent.Controls.Add((MyGuiControlBase) guiControlButton2);
        }
      }
    }

    private void ModURLClick(MyGuiControlButton button) => MyGuiSandbox.OpenUrl((button.UserData as MyWorkshopItem).GetItemUrl(), UrlOpenMode.SteamOrExternalWithConfirm);

    private void DrawPlayers()
    {
      MyGuiControlLabel myGuiControlLabel1 = this.AddLabel(MyCommonTexts.ScreenCaptionPlayers, (object) null);
      if (this.m_players == null)
        this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: MyTexts.GetString(MyCommonTexts.ServerDetails_PlayerError), font: "Red"));
      else if (this.m_players.Count == 0)
      {
        this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: MyTexts.GetString(MyCommonTexts.ServerDetails_ServerEmpty)));
      }
      else
      {
        MyGuiControlParent guiControlParent = new MyGuiControlParent();
        MyGuiControlScrollablePanel controlScrollablePanel1 = new MyGuiControlScrollablePanel((MyGuiControlBase) guiControlParent);
        controlScrollablePanel1.ScrollbarVEnabled = true;
        controlScrollablePanel1.Position = this.m_currentPosition;
        controlScrollablePanel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
        MyGuiControlScrollablePanel controlScrollablePanel2 = controlScrollablePanel1;
        Vector2? size = this.Size;
        double num1 = (double) size.Value.X - 0.112000003457069;
        size = this.Size;
        double num2 = (double) size.Value.Y / 2.0 - (double) this.m_currentPosition.Y - 0.144999995827675;
        Vector2 vector2_1 = new Vector2((float) num1, (float) num2);
        controlScrollablePanel2.Size = vector2_1;
        controlScrollablePanel1.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
        controlScrollablePanel1.ScrolledAreaPadding = new MyGuiBorderThickness(0.005f);
        this.Controls.Add((MyGuiControlBase) controlScrollablePanel1);
        guiControlParent.Size = new Vector2(controlScrollablePanel1.Size.X, (float) ((double) this.m_players.Count * ((double) myGuiControlLabel1.Size.Y / 2.0 + (double) this.m_padding) + (double) myGuiControlLabel1.Size.Y / 2.0));
        Vector2 vector2_2 = new Vector2((float) (-(double) controlScrollablePanel1.Size.X / 2.0), (float) (-(double) guiControlParent.Size.Y / 2.0 + (double) myGuiControlLabel1.Size.Y / 2.0));
        foreach (KeyValuePair<string, float> player in this.m_players)
        {
          StringBuilder output = new StringBuilder(player.Key);
          if ((double) player.Value >= 0.0)
          {
            output.Append(": ");
            MyValueFormatter.AppendTimeInBestUnit((float) (int) player.Value, output);
          }
          MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(vector2_2), text: output.ToString());
          vector2_2.Y += myGuiControlLabel2.Size.Y / 2f + this.m_padding;
          guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
        }
      }
    }

    protected void DrawButtons()
    {
      float x = this.m_currentPosition.X;
      this.m_btSettings = new MyGuiControlButton(new Vector2?(this.m_currentPosition), MyGuiControlButtonStyleEnum.ToolbarButton, text: MyTexts.Get(MyCommonTexts.ServerDetails_Settings));
      this.m_btSettings.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_Settings));
      this.m_btSettings.PositionX += this.m_btSettings.Size.X / 2f;
      this.m_currentPosition.X += this.m_btSettings.Size.X + this.m_padding / 4f;
      this.m_btSettings.ButtonClicked += new Action<MyGuiControlButton>(this.SettingButtonClick);
      this.Controls.Add((MyGuiControlBase) this.m_btSettings);
      this.m_btMods = new MyGuiControlButton(new Vector2?(this.m_currentPosition), MyGuiControlButtonStyleEnum.ToolbarButton, text: MyTexts.Get(MyCommonTexts.WorldSettings_Mods));
      this.m_btMods.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_Mods));
      this.m_btMods.PositionX += this.m_btMods.Size.X / 2f;
      this.m_currentPosition.X += this.m_btMods.Size.X + this.m_padding / 4f;
      this.m_btMods.ButtonClicked += new Action<MyGuiControlButton>(this.ModsButtonClick);
      this.m_btMods.Enabled = MyPlatformGameSettings.IsModdingAllowed;
      this.Controls.Add((MyGuiControlBase) this.m_btMods);
      this.m_btPlayers = new MyGuiControlButton(new Vector2?(this.m_currentPosition), MyGuiControlButtonStyleEnum.ToolbarButton, text: MyTexts.Get(MyCommonTexts.ScreenCaptionPlayers));
      this.m_btPlayers.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_Players));
      this.m_btPlayers.PositionX += this.m_btPlayers.Size.X / 2f;
      this.m_currentPosition.X += this.m_btPlayers.Size.X + this.m_padding / 4f;
      this.m_btPlayers.ButtonClicked += new Action<MyGuiControlButton>(this.PlayersButtonClick);
      this.Controls.Add((MyGuiControlBase) this.m_btPlayers);
      this.m_currentPosition.X = x;
      this.m_currentPosition.Y += this.m_btSettings.Size.Y + this.m_padding / 2f;
    }

    public override string GetFriendlyName() => "ServerDetails";

    protected SortedList<string, object> LoadSessionSettings(VRage.Game.Game game)
    {
      if (this.Settings == null)
        return (SortedList<string, object>) null;
      SortedList<string, object> result = new SortedList<string, object>();
      foreach (FieldInfo field in typeof (MyObjectBuilder_SessionSettings).GetFields(BindingFlags.Instance | BindingFlags.Public))
      {
        GameRelationAttribute customAttribute1 = field.GetCustomAttribute<GameRelationAttribute>();
        if (customAttribute1 != null && (customAttribute1.RelatedTo == VRage.Game.Game.Shared || customAttribute1.RelatedTo == game))
        {
          DisplayAttribute customAttribute2 = field.GetCustomAttribute<DisplayAttribute>();
          if (customAttribute2 != null && !string.IsNullOrEmpty(customAttribute2.Name))
          {
            string str = "ServerDetails_" + field.Name;
            if (!(MyTexts.GetString(str) == str))
              result.Add(str, field.GetValue((object) this.Settings));
          }
        }
      }
      this.AddAdditionalSettings(ref result);
      return result;
    }

    private void AddAdditionalSettings(ref SortedList<string, object> result)
    {
      if (this.Settings == null)
        return;
      result.Add(MyTexts.GetString(MyCommonTexts.ServerDetails_PCU_Initial), (object) MyObjectBuilder_SessionSettings.GetInitialPCU(this.Settings));
    }

    private void ConnectButtonClick(MyGuiControlButton obj) => this.ParseIPAndConnect();

    private void ParseIPAndConnect()
    {
      try
      {
        MyGameService.OnPingServerResponded += new EventHandler<MyGameServerItem>(MySandboxGame.Static.ServerResponded);
        MyGameService.OnPingServerFailedToRespond += new EventHandler(MySandboxGame.Static.ServerFailedToRespond);
        MyGameService.PingServer(this.m_server.Server.ConnectionString);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(ex);
        MyGuiSandbox.Show(MyTexts.Get(MyCommonTexts.MultiplayerJoinIPError), MyCommonTexts.MessageBoxCaptionError);
      }
    }

    private void CloseButtonClick(MyGuiControlButton myGuiControlButton) => this.CloseScreen();

    private void TestFavoritesGameList()
    {
      MyGameService.OnFavoritesServerListResponded += new EventHandler<int>(this.OnFavoritesServerListResponded);
      MyGameService.OnFavoritesServersCompleteResponse += new EventHandler<MyMatchMakingServerResponse>(this.OnFavoritesServersCompleteResponse);
      MyGameService.RequestFavoritesServerList(new MySessionSearchFilter());
      this.m_loadingWheel.Visible = true;
    }

    private void OnFavoritesServerListResponded(object sender, int server)
    {
      MyCachedServerItem cachedServerItem = new MyCachedServerItem(MyGameService.GetFavoritesServerDetails(server));
      if (cachedServerItem == null)
        return;
      MyCachedServerItem server1 = this.m_server;
      if (!(cachedServerItem.Server.ConnectionString == server1.Server.ConnectionString))
        return;
      this.m_serverIsFavorited = true;
      this.RecreateControls(false);
      this.m_loadingWheel.Visible = false;
    }

    private void OnFavoritesServersCompleteResponse(
      object sender,
      MyMatchMakingServerResponse response)
    {
      this.CloseFavoritesRequest();
    }

    private void CloseFavoritesRequest()
    {
      MyGameService.OnFavoritesServerListResponded -= new EventHandler<int>(this.OnFavoritesServerListResponded);
      MyGameService.OnFavoritesServersCompleteResponse -= new EventHandler<MyMatchMakingServerResponse>(this.OnFavoritesServersCompleteResponse);
      MyGameService.CancelFavoritesServersRequest();
      this.m_loadingWheel.Visible = false;
    }

    private void FavoriteButtonClick(MyGuiControlButton myGuiControlButton)
    {
      MyGameService.AddFavoriteGame(this.m_server.Server);
      this.m_serverIsFavorited = true;
      this.RecreateControls(false);
    }

    private void UnFavoriteButtonClick(MyGuiControlButton myGuiControlButton)
    {
      MyGameService.RemoveFavoriteGame(this.m_server.Server);
      MyGuiScreenJoinGame firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenJoinGame>();
      if (firstScreenOfType.m_selectedPage.Name == "PageFavoritesPanel")
        firstScreenOfType.RemoveFavoriteServer(this.m_server);
      this.m_serverIsFavorited = false;
      this.RecreateControls(false);
    }

    private void JoinButtonClick(MyGuiControlButton myGuiControlButton) => this.CloseScreen();

    private void SettingButtonClick(MyGuiControlButton myGuiControlButton)
    {
      this.m_currentPage = MyGuiScreenServerDetailsBase.DetailsPageEnum.Settings;
      this.RecreateControls(false);
    }

    private void ModsButtonClick(MyGuiControlButton myGuiControlButton)
    {
      this.m_currentPage = MyGuiScreenServerDetailsBase.DetailsPageEnum.Mods;
      if (this.m_server.Mods != null && this.m_server.Mods.Count > 0)
        MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, new MyStringId?(), new Func<IMyAsyncResult>(this.BeginModResultAction), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(this.EndModResultAction)));
      else if (this.m_server.Mods != null && this.m_server.Mods.Count == 0)
      {
        this.m_mods = new List<MyWorkshopItem>();
        this.RecreateControls(false);
      }
      else
        this.RecreateControls(false);
    }

    private void PlayersButtonClick(MyGuiControlButton myGuiControlButton)
    {
      this.m_currentPage = MyGuiScreenServerDetailsBase.DetailsPageEnum.Players;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, new MyStringId?(), new Func<IMyAsyncResult>(this.BeginPlayerResultAction), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(this.EndPlayerResultAction)));
    }

    private IMyAsyncResult BeginPlayerResultAction() => (IMyAsyncResult) new MyGuiScreenServerDetailsBase.LoadPlayersResult(this.m_server);

    private void EndPlayerResultAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
    {
      this.m_players = ((MyGuiScreenServerDetailsBase.LoadPlayersResult) result).Players;
      screen.CloseScreen();
      this.m_loadingWheel.Visible = false;
      this.RecreateControls(false);
    }

    private IMyAsyncResult BeginModResultAction() => (IMyAsyncResult) new MyGuiScreenServerDetailsBase.LoadModsResult(this.m_server);

    private void EndModResultAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
    {
      this.m_mods = ((MyGuiScreenServerDetailsBase.LoadModsResult) result).ServerMods;
      screen.CloseScreen();
      this.m_loadingWheel.Visible = false;
      this.RecreateControls(false);
    }

    protected void AddSeparator(MyGuiControlParent parent, Vector2 localPos, float size = 1f)
    {
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.Size = new Vector2(1f, 0.01f);
      controlSeparatorList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      controlSeparatorList.AddHorizontal(Vector2.Zero, size);
      controlSeparatorList.Position = new Vector2(localPos.X, localPos.Y - 0.02f);
      controlSeparatorList.Alpha = 0.4f;
      parent.Controls.Add((MyGuiControlBase) controlSeparatorList);
    }

    protected MyGuiControlLabel AddLabel(MyStringId description, object value)
    {
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: string.Format("{0}: {1}", (object) MyTexts.GetString(description), value));
      this.m_currentPosition.Y += myGuiControlLabel.Size.Y / 2f + this.m_padding;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      return myGuiControlLabel;
    }

    protected MyGuiControlLabel AddLabel(MyStringId description)
    {
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: MyTexts.GetString(description));
      this.m_currentPosition.Y += myGuiControlLabel.Size.Y / 2f + this.m_padding;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      return myGuiControlLabel;
    }

    protected MyGuiControlMultilineText AddMultilineText(
      string text,
      float size)
    {
      return this.AddMultilineText(new StringBuilder(text), size);
    }

    protected MyGuiControlMultilineText AddMultilineText(
      StringBuilder text,
      float size)
    {
      this.m_currentPosition.Y -= this.m_padding / 2f;
      this.m_currentPosition.X += 3f / 1000f;
      MyGuiControlMultilineText controlMultilineText1 = new MyGuiControlMultilineText(new Vector2?(this.m_currentPosition), new Vector2?(new Vector2(this.Size.Value.X - 0.112f, size)), textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_currentPosition.X -= 3f / 1000f;
      controlMultilineText1.Text = text;
      MyGuiControlMultilineText controlMultilineText2 = controlMultilineText1;
      controlMultilineText2.Position = controlMultilineText2.Position + controlMultilineText1.Size / 2f;
      MyGuiControlCompositePanel controlCompositePanel1 = new MyGuiControlCompositePanel();
      controlCompositePanel1.Position = this.m_currentPosition;
      controlCompositePanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      controlCompositePanel1.Size = controlMultilineText1.Size;
      controlCompositePanel1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiControlCompositePanel controlCompositePanel2 = controlCompositePanel1;
      controlMultilineText1.Size = new Vector2(controlMultilineText1.Size.X / 1.01f, controlMultilineText1.Size.Y / 1.09f);
      this.m_currentPosition.Y += controlMultilineText1.Size.Y + this.m_padding * 1.5f;
      this.Controls.Add((MyGuiControlBase) controlCompositePanel2);
      this.Controls.Add((MyGuiControlBase) controlMultilineText1);
      return controlMultilineText1;
    }

    protected MyGuiControlCheckbox AddCheckbox(
      MyStringId text,
      Action<MyGuiControlCheckbox> onClick,
      MyStringId? tooltip = null)
    {
      MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(new Vector2?(this.m_currentPosition), toolTip: (tooltip.HasValue ? MyTexts.GetString(tooltip.Value) : string.Empty));
      guiControlCheckbox.PositionX += guiControlCheckbox.Size.X / 2f;
      this.Controls.Add((MyGuiControlBase) guiControlCheckbox);
      if (onClick != null)
        guiControlCheckbox.IsCheckedChanged += onClick;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: MyTexts.GetString(text));
      myGuiControlLabel.PositionX += guiControlCheckbox.Size.X + this.m_padding;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.m_currentPosition.Y += guiControlCheckbox.Size.Y;
      return guiControlCheckbox;
    }

    protected enum DetailsPageEnum
    {
      Settings,
      Mods,
      Players,
    }

    private class LoadPlayersResult : IMyAsyncResult
    {
      public LoadPlayersResult(MyCachedServerItem server) => MyGameService.GetPlayerDetails(server.Server, new PlayerDetailsResponse(this.LoadCompleted), (Action) (() => this.LoadCompleted((Dictionary<string, float>) null)));

      public Dictionary<string, float> Players { get; private set; }

      public bool IsCompleted { get; private set; }

      public Task Task { get; private set; }

      private void LoadCompleted(Dictionary<string, float> players)
      {
        this.Players = players;
        this.IsCompleted = true;
      }
    }

    private class LoadModsResult : IMyAsyncResult
    {
      public bool IsCompleted => this.Task.IsComplete;

      public Task Task { get; private set; }

      public List<MyWorkshopItem> ServerMods { get; private set; }

      public LoadModsResult(MyCachedServerItem server)
      {
        MyGuiScreenServerDetailsBase.LoadModsResult loadModsResult = this;
        this.ServerMods = new List<MyWorkshopItem>();
        this.Task = Parallel.Start((Action) (() =>
        {
          if (!MyGameService.IsOnline || server.Mods == null || server.Mods.Count <= 0)
            return;
          MyWorkshop.GetItemsBlockingUGC(server.Mods, loadModsResult.ServerMods);
        }));
      }
    }
  }
}
