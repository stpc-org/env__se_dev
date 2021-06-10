// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiBlueprintScreen
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  public class MyGuiBlueprintScreen : MyGuiBlueprintScreenBase
  {
    private readonly string m_thumbImageName = "thumb.png";
    public static Task Task;
    private static bool m_downloadFromSteam = true;
    private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);
    private static readonly float HIDDEN_PART_RIGHT = 0.04f;
    private static List<MyGuiControlListbox.Item> m_recievedBlueprints = new List<MyGuiControlListbox.Item>();
    private static bool m_needsExtract = false;
    public static List<MyWorkshopItem> m_subscribedItemsList = new List<MyWorkshopItem>();
    private Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);
    private float m_textScale = 0.8f;
    private MyGuiControlButton m_detailsButton;
    private MyGuiControlButton m_screenshotButton;
    private MyGuiControlButton m_replaceButton;
    private MyGuiControlButton m_deleteButton;
    private MyGuiControlButton m_okButton;
    private MyGuiControlCombobox m_sortCombobox;
    private MyGuiControlTextbox m_searchBox;
    private MyGuiControlButton m_searchClear;
    private static MyBlueprintSortingOptions m_sortBy = MyBlueprintSortingOptions.SortBy_None;
    private static MyGuiControlListbox m_blueprintList = new MyGuiControlListbox(visualStyle: MyGuiControlListboxStyleEnum.Blueprints);
    private MyGuiDetailScreenBase m_detailScreen;
    private MyGuiControlImage m_thumbnailImage;
    private bool m_activeDetail;
    private MyGuiControlListbox.Item m_selectedItem;
    private MyGuiControlRotatingWheel m_wheel;
    private MyGridClipboard m_clipboard;
    private bool m_allowCopyToClipboard;
    private string m_selectedThumbnailPath;
    private bool m_blueprintBeingLoaded;
    private MyBlueprintAccessType m_accessType;
    private static string m_currentLocalDirectory = string.Empty;
    private static HashSet<ulong> m_downloadQueued = new HashSet<ulong>();
    private static MyConcurrentHashSet<ulong> m_downloadFinished = new MyConcurrentHashSet<ulong>();
    private static string TEMP_PATH = MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP;
    private string[] filenames;
    private static MyGuiBlueprintScreen.LoadPrefabData m_LoadPrefabData;
    private List<string> m_preloadedTextures = new List<string>();
    private MyGuiControlListbox.Item m_previousItem;

    public static bool FirstTime
    {
      get => MyGuiBlueprintScreen.m_downloadFromSteam;
      set => MyGuiBlueprintScreen.m_downloadFromSteam = value;
    }

    [Event(null, 205)]
    [Reliable]
    [Server]
    public static void ShareBlueprintRequest(
      ulong workshopId,
      string name,
      ulong sendToId,
      string senderName)
    {
      if (Sync.IsServer && (long) sendToId != (long) Sync.MyId)
        MyMultiplayer.RaiseStaticEvent<ulong, string, ulong, string>((Func<IMyEventOwner, Action<ulong, string, ulong, string>>) (x => new Action<ulong, string, ulong, string>(MyGuiBlueprintScreen.ShareBlueprintRequestClient)), workshopId, name, sendToId, senderName, new EndpointId(sendToId));
      else
        MyGuiBlueprintScreen.ShareBlueprintRequestClient(workshopId, name, sendToId, senderName);
    }

    [Event(null, 218)]
    [Reliable]
    [Client]
    private static void ShareBlueprintRequestClient(
      ulong workshopId,
      string name,
      ulong sendToId,
      string senderName)
    {
      MyBlueprintItemInfo blueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.SHARED);
      StringBuilder text = new StringBuilder(name.ToString());
      object obj = (object) blueprintItemInfo;
      string normal = MyGuiConstants.TEXTURE_BLUEPRINTS_ARROW.Normal;
      object userData = obj;
      MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(text, icon: normal, userData: userData);
      item.ColorMask = new Vector4?(new Vector4(0.7f));
      if (MyGuiBlueprintScreen.m_recievedBlueprints.Any<MyGuiControlListbox.Item>((Func<MyGuiControlListbox.Item, bool>) (item2 => (long) (item2.UserData as MyBlueprintItemInfo).Item.Id == (long) (item.UserData as MyBlueprintItemInfo).Item.Id)))
        return;
      MyGuiBlueprintScreen.m_recievedBlueprints.Add(item);
      MyGuiBlueprintScreen.m_blueprintList.Add(item);
      MyHudNotificationDebug notificationDebug = new MyHudNotificationDebug(string.Format(MyTexts.Get(MySpaceTexts.SharedBlueprintNotify).ToString(), (object) senderName));
      MyHud.Notifications.Add((MyHudNotificationBase) notificationDebug);
    }

    public override string GetFriendlyName() => "MyBlueprintScreen";

    public MyGuiBlueprintScreen(
      MyGridClipboard clipboard,
      bool allowCopyToClipboard,
      MyBlueprintAccessType accessType)
      : base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - MyGuiBlueprintScreen.SCREEN_SIZE.X * 0.5f + MyGuiBlueprintScreen.HIDDEN_PART_RIGHT, 0.5f), MyGuiBlueprintScreen.SCREEN_SIZE, MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, false)
    {
      this.m_accessType = accessType;
      this.m_clipboard = clipboard;
      this.m_allowCopyToClipboard = allowCopyToClipboard;
      if (!Directory.Exists(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL))
        Directory.CreateDirectory(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL);
      if (!Directory.Exists(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP))
        Directory.CreateDirectory(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP);
      MyGuiBlueprintScreen.m_blueprintList.Items.Clear();
      MyGuiBlueprintScreen.CheckCurrentLocalDirectory();
      this.GetLocalBlueprintNames(MyGuiBlueprintScreen.m_downloadFromSteam);
      if (MyGuiBlueprintScreen.m_downloadFromSteam)
        MyGuiBlueprintScreen.m_downloadFromSteam = false;
      this.CreateTempDirectory();
      this.RecreateControls(true);
      MyGuiBlueprintScreen.m_blueprintList.ItemsSelected += new Action<MyGuiControlListbox>(this.OnSelectItem);
      MyGuiBlueprintScreen.m_blueprintList.ItemDoubleClicked += new Action<MyGuiControlListbox>(this.OnItemDoubleClick);
      MyGuiBlueprintScreen.m_blueprintList.ItemMouseOver += new Action<MyGuiControlListbox>(this.OnMouseOverItem);
      this.OnEnterCallback = this.OnEnterCallback + new Action(this.Ok);
      this.m_searchBox.TextChanged += new Action<MyGuiControlTextbox>(this.OnSearchTextChange);
      if (MyGuiScreenHudSpace.Static == null)
        return;
      MyGuiScreenHudSpace.Static.HideScreen();
    }

    private void CreateButtons()
    {
      Vector2 vector2_1 = new Vector2(-0.091f, 0.194f);
      Vector2 vector2_2 = new Vector2(0.144f, 0.035f);
      float num1 = 0.143f;
      float usableWidth = 0.287f;
      double num2 = (double) num1;
      StringBuilder text1 = MyTexts.Get(MyCommonTexts.Ok);
      Action<MyGuiControlButton> onClick1 = new Action<MyGuiControlButton>(this.OnOk);
      float textScale1 = this.m_textScale;
      MyStringId? nullable = new MyStringId?(!this.m_allowCopyToClipboard || this.m_selectedItem == null ? MyCommonTexts.Blueprints_OkTooltipDisabled : MyCommonTexts.Blueprints_OkTooltip);
      int num3 = !this.m_allowCopyToClipboard ? 0 : (this.m_selectedItem != null ? 1 : 0);
      MyStringId? tooltip1 = nullable;
      double num4 = (double) textScale1;
      this.m_okButton = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num2, text1, onClick1, num3 != 0, tooltip1, (float) num4);
      this.m_okButton.Position = vector2_1;
      this.m_okButton.ShowTooltipWhenDisabled = true;
      double num5 = (double) num1;
      StringBuilder text2 = MyTexts.Get(MyCommonTexts.Details);
      Action<MyGuiControlButton> onClick2 = new Action<MyGuiControlButton>(this.OnDetails);
      float textScale2 = this.m_textScale;
      MyStringId? tooltip2 = new MyStringId?(MyCommonTexts.Blueprints_OkTooltipDisabled);
      double num6 = (double) textScale2;
      this.m_detailsButton = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num5, text2, onClick2, false, tooltip2, (float) num6);
      this.m_detailsButton.Position = vector2_1 + new Vector2(1f, 0.0f) * vector2_2;
      this.m_detailsButton.ShowTooltipWhenDisabled = true;
      double num7 = (double) num1;
      StringBuilder text3 = MyTexts.Get(MyCommonTexts.TakeScreenshot);
      Action<MyGuiControlButton> onClick3 = new Action<MyGuiControlButton>(this.OnScreenshot);
      float textScale3 = this.m_textScale;
      MyStringId? tooltip3 = new MyStringId?(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
      double num8 = (double) textScale3;
      this.m_screenshotButton = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num7, text3, onClick3, false, tooltip3, (float) num8);
      this.m_screenshotButton.Position = vector2_1 + new Vector2(0.0f, 1f) * vector2_2;
      this.m_screenshotButton.ShowTooltipWhenDisabled = true;
      double num9 = (double) num1;
      StringBuilder text4 = MyTexts.Get(MyCommonTexts.Delete);
      Action<MyGuiControlButton> onClick4 = new Action<MyGuiControlButton>(this.OnDelete);
      float textScale4 = this.m_textScale;
      MyStringId? tooltip4 = new MyStringId?(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
      double num10 = (double) textScale4;
      this.m_deleteButton = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num9, text4, onClick4, false, tooltip4, (float) num10);
      this.m_deleteButton.Position = vector2_1 + new Vector2(1f, 1f) * vector2_2;
      this.m_deleteButton.ShowTooltipWhenDisabled = true;
      double num11 = (double) usableWidth;
      StringBuilder text5 = MyTexts.Get(MySpaceTexts.ReplaceWithClipboard);
      Action<MyGuiControlButton> onClick5 = new Action<MyGuiControlButton>(this.OnReplace);
      int num12 = this.m_clipboard != null ? (!this.m_clipboard.HasCopiedGrids() ? 0 : (this.m_selectedItem != null ? 1 : 0)) : 0;
      float textScale5 = this.m_textScale;
      MyStringId? tooltip5 = new MyStringId?(MyCommonTexts.Blueprints_OkTooltipDisabled);
      double num13 = (double) textScale5;
      this.m_replaceButton = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num11, text5, onClick5, num12 != 0, tooltip5, (float) num13);
      this.m_replaceButton.Position = vector2_1 + new Vector2(0.0f, 2f) * vector2_2;
      this.m_replaceButton.PositionX += vector2_2.X / 2f;
      this.m_replaceButton.ShowTooltipWhenDisabled = true;
      vector2_1 = new Vector2(-0.091f, 0.343f);
      double num14 = (double) usableWidth;
      StringBuilder text6 = MyTexts.Get(MySpaceTexts.CreateFromClipboard);
      Action<MyGuiControlButton> onClick6 = new Action<MyGuiControlButton>(this.OnCreate);
      int num15 = this.m_clipboard != null ? (this.m_clipboard.HasCopiedGrids() ? 1 : 0) : 0;
      float textScale6 = this.m_textScale;
      MyStringId? tooltip6 = new MyStringId?(this.m_clipboard == null || !this.m_clipboard.HasCopiedGrids() ? MyCommonTexts.Blueprints_CreateTooltipDisabled : MyCommonTexts.Blueprints_CreateTooltip);
      double num16 = (double) textScale6;
      MyGuiControlButton button1 = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num14, text6, onClick6, num15 != 0, tooltip6, (float) num16);
      button1.ShowTooltipWhenDisabled = true;
      button1.Position = vector2_1 + new Vector2(0.0f, 0.0f) * vector2_2;
      button1.PositionX += vector2_2.X / 2f;
      double num17 = (double) usableWidth;
      StringBuilder text7 = MyTexts.Get(MySpaceTexts.RefreshBlueprints);
      Action<MyGuiControlButton> onClick7 = new Action<MyGuiControlButton>(this.OnReload);
      float textScale7 = this.m_textScale;
      MyStringId? tooltip7 = new MyStringId?(MyCommonTexts.Blueprints_RefreshTooltip);
      double num18 = (double) textScale7;
      MyGuiControlButton button2 = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, (float) num17, text7, onClick7, tooltip: tooltip7, textScale: ((float) num18));
      button2.Position = vector2_1 + new Vector2(0.0f, 1f) * vector2_2;
      button2.PositionX += vector2_2.X / 2f;
      MyGuiControlButton button3 = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, usableWidth, MyTexts.Get(MyCommonTexts.Close), new Action<MyGuiControlButton>(this.OnCancel), tooltip: new MyStringId?(MySpaceTexts.ToolTipNewsletter_Close), textScale: this.m_textScale);
      button3.Position = vector2_1 + new Vector2(0.0f, 2f) * vector2_2;
      button3.PositionX += vector2_2.X / 2f;
    }

    public void RefreshThumbnail()
    {
      this.m_thumbnailImage = new MyGuiControlImage();
      this.m_thumbnailImage.Position = new Vector2(-0.31f, -0.224f);
      this.m_thumbnailImage.Size = new Vector2(0.2f, 0.175f);
      this.m_thumbnailImage.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
      this.m_thumbnailImage.SetPadding(new MyGuiBorderThickness(2f, 2f, 2f, 2f));
      this.m_thumbnailImage.Visible = false;
      this.m_thumbnailImage.BorderEnabled = true;
      this.m_thumbnailImage.BorderSize = 1;
      this.m_thumbnailImage.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      Vector2 vector2 = new Vector2(0.02f, MyGuiBlueprintScreen.SCREEN_SIZE.Y - 1.076f);
      float num = (float) (((double) MyGuiBlueprintScreen.SCREEN_SIZE.Y - 1.0) / 2.0);
      MyGuiControlLabel myGuiControlLabel1 = this.MakeLabel(MyTexts.Get(MyCommonTexts.Search).ToString() + ":", vector2 + new Vector2(-0.175f, -0.015f), this.m_textScale);
      myGuiControlLabel1.Position = new Vector2(-0.164f, -0.406f);
      this.m_searchBox = new MyGuiControlTextbox();
      this.m_searchBox.Position = new Vector2(0.123f, -0.401f);
      this.m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.m_searchBox.Size = new Vector2(0.279f - myGuiControlLabel1.Size.X, 0.2f);
      this.m_searchBox.SetToolTip(MyCommonTexts.Blueprints_SearchTooltip);
      MyGuiControlButton guiControlButton = new MyGuiControlButton();
      guiControlButton.Position = vector2 + new Vector2(0.076f, -0.521f);
      guiControlButton.Size = new Vector2(0.045f, 0.05666667f);
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Close;
      guiControlButton.ActivateOnMouseRelease = true;
      this.m_searchClear = guiControlButton;
      this.m_searchClear.ButtonClicked += new Action<MyGuiControlButton>(this.OnSearchClear);
      this.m_sortCombobox = new MyGuiControlCombobox();
      foreach (object obj in Enum.GetValues(typeof (MyBlueprintSortingOptions)))
        this.m_sortCombobox.AddItem((long) (int) obj, new StringBuilder(MyTexts.TrySubstitute(obj.ToString())));
      this.m_sortCombobox.SelectItemByIndex((int) MyGuiBlueprintScreen.m_sortBy);
      this.m_sortCombobox.ItemSelected += (MyGuiControlCombobox.ItemSelectedDelegate) (() => this.SortOptionChanged((MyBlueprintSortingOptions) this.m_sortCombobox.GetSelectedKey()));
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.Blueprint_Sort_Label));
      myGuiControlLabel2.Position = new Vector2(-0.164f, -0.348f);
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_sortCombobox.Position = new Vector2(0.123f, -0.348f);
      this.m_sortCombobox.Size = new Vector2(0.28f - myGuiControlLabel2.Size.X, 0.04f);
      this.m_sortCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.m_sortCombobox.SetToolTip(MyCommonTexts.Blueprints_SortByTooltip);
      this.AddCaption(MyTexts.Get(MySpaceTexts.BlueprintsScreen).ToString(), new Vector4?(Color.White.ToVector4()), new Vector2?(this.m_controlPadding + new Vector2(-MyGuiBlueprintScreen.HIDDEN_PART_RIGHT, num - 0.03f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.44f), this.m_size.Value.X * 0.73f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), -0.17f), this.m_size.Value.X * 0.73f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), -0.318f), this.m_size.Value.X * 0.73f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel();
      myGuiControlLabel3.Position = new Vector2(-0.155f, -0.307f);
      myGuiControlLabel3.Name = "ControlLabel";
      myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel3.Text = MyTexts.GetString(MyCommonTexts.Blueprints_ListOfBlueprints);
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      MyGuiControlPanel myGuiControlPanel1 = new MyGuiControlPanel(new Vector2?(new Vector2(-0.1635f, -0.312f)), new Vector2?(new Vector2(0.2865f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      MyGuiControlPanel myGuiControlPanel2 = myGuiControlPanel1;
      MyGuiBlueprintScreen.m_blueprintList.Position = new Vector2(-0.02f, -0.066f);
      MyGuiBlueprintScreen.m_blueprintList.VisibleRowsCount = 12;
      MyGuiBlueprintScreen.m_blueprintList.MultiSelect = false;
      this.Controls.Add((MyGuiControlBase) myGuiControlPanel2);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.Controls.Add((MyGuiControlBase) this.m_searchBox);
      this.Controls.Add((MyGuiControlBase) this.m_searchClear);
      this.Controls.Add((MyGuiControlBase) MyGuiBlueprintScreen.m_blueprintList);
      this.Controls.Add((MyGuiControlBase) this.m_sortCombobox);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      this.RefreshThumbnail();
      this.Controls.Add((MyGuiControlBase) this.m_thumbnailImage);
      this.CreateButtons();
      string texture = "Textures\\GUI\\screens\\screen_loading_wheel.dds";
      this.m_wheel = new MyGuiControlRotatingWheel(new Vector2?(new Vector2(-0.02f, -0.12f)), new Vector4?(MyGuiConstants.ROTATING_WHEEL_COLOR), 0.28f, texture: texture, multipleSpinningWheels: MyPerGameSettings.GUI.MultipleSpinningWheels);
      this.Controls.Add((MyGuiControlBase) this.m_wheel);
      this.m_wheel.Visible = false;
    }

    public void SortOptionChanged(MyBlueprintSortingOptions option)
    {
      MyGuiBlueprintScreen.m_sortBy = option;
      this.OnReload((MyGuiControlButton) null);
    }

    private void GetLocalBlueprintNames(bool reload = false)
    {
      this.GetBlueprints(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, MyGuiBlueprintScreen.m_currentLocalDirectory), MyBlueprintTypeEnum.LOCAL);
      if (MySandboxGame.Config.EnableSteamCloud && MyGameService.IsActive)
        this.GetBlueprintsFromCloud();
      if (MyGuiBlueprintScreen.Task.IsComplete)
      {
        if (reload)
          this.GetWorkshopBlueprints();
        else
          this.GetWorkshopItemsSteam();
      }
      foreach (MyGuiControlListbox.Item recievedBlueprint in MyGuiBlueprintScreen.m_recievedBlueprints)
        MyGuiBlueprintScreen.m_blueprintList.Add(recievedBlueprint);
      if (!MyFakes.ENABLE_DEFAULT_BLUEPRINTS)
        return;
      this.GetBlueprints(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, MyBlueprintTypeEnum.DEFAULT);
    }

    private void SortBlueprints(List<MyGuiControlListbox.Item> list, MyBlueprintTypeEnum type)
    {
      MyItemComparer myItemComparer = (MyItemComparer) null;
      switch (type)
      {
        case MyBlueprintTypeEnum.WORKSHOP:
          switch (MyGuiBlueprintScreen.m_sortBy)
          {
            case MyBlueprintSortingOptions.SortBy_Name:
              myItemComparer = new MyItemComparer((Func<MyGuiControlListbox.Item, MyGuiControlListbox.Item, int>) ((x, y) => ((MyBlueprintItemInfo) x.UserData).BlueprintName.CompareTo(((MyBlueprintItemInfo) y.UserData).BlueprintName)));
              break;
            case MyBlueprintSortingOptions.SortBy_CreationDate:
              myItemComparer = new MyItemComparer((Func<MyGuiControlListbox.Item, MyGuiControlListbox.Item, int>) ((x, y) =>
              {
                DateTime timeCreated1 = ((MyBlueprintItemInfo) x.UserData).Item.TimeCreated;
                DateTime timeCreated2 = ((MyBlueprintItemInfo) y.UserData).Item.TimeCreated;
                if (timeCreated1 < timeCreated2)
                  return 1;
                return timeCreated1 > timeCreated2 ? -1 : 0;
              }));
              break;
            case MyBlueprintSortingOptions.SortBy_UpdateDate:
              myItemComparer = new MyItemComparer((Func<MyGuiControlListbox.Item, MyGuiControlListbox.Item, int>) ((x, y) =>
              {
                DateTime timeUpdated1 = ((MyBlueprintItemInfo) x.UserData).Item.TimeUpdated;
                DateTime timeUpdated2 = ((MyBlueprintItemInfo) y.UserData).Item.TimeUpdated;
                if (timeUpdated1 < timeUpdated2)
                  return 1;
                return timeUpdated1 > timeUpdated2 ? -1 : 0;
              }));
              break;
          }
          break;
        case MyBlueprintTypeEnum.LOCAL:
        case MyBlueprintTypeEnum.CLOUD:
          switch (MyGuiBlueprintScreen.m_sortBy)
          {
            case MyBlueprintSortingOptions.SortBy_Name:
              myItemComparer = new MyItemComparer((Func<MyGuiControlListbox.Item, MyGuiControlListbox.Item, int>) ((x, y) => ((MyBlueprintItemInfo) x.UserData).BlueprintName.CompareTo(((MyBlueprintItemInfo) y.UserData).BlueprintName)));
              break;
            case MyBlueprintSortingOptions.SortBy_CreationDate:
              myItemComparer = new MyItemComparer((Func<MyGuiControlListbox.Item, MyGuiControlListbox.Item, int>) ((x, y) =>
              {
                DateTime? timeCreated1 = ((MyBlueprintItemInfo) x.UserData).TimeCreated;
                DateTime? timeCreated2 = ((MyBlueprintItemInfo) y.UserData).TimeCreated;
                return timeCreated1.HasValue && timeCreated2.HasValue ? -1 * DateTime.Compare(timeCreated1.Value, timeCreated2.Value) : 0;
              }));
              break;
            case MyBlueprintSortingOptions.SortBy_UpdateDate:
              myItemComparer = new MyItemComparer((Func<MyGuiControlListbox.Item, MyGuiControlListbox.Item, int>) ((x, y) =>
              {
                DateTime? timeUpdated1 = ((MyBlueprintItemInfo) x.UserData).TimeUpdated;
                DateTime? timeUpdated2 = ((MyBlueprintItemInfo) y.UserData).TimeUpdated;
                return timeUpdated1.HasValue && timeUpdated2.HasValue ? -1 * DateTime.Compare(timeUpdated1.Value, timeUpdated2.Value) : 0;
              }));
              break;
          }
          break;
      }
      if (myItemComparer == null)
        return;
      list.Sort((IComparer<MyGuiControlListbox.Item>) myItemComparer);
    }

    private void GetBlueprintsFromCloud()
    {
      List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles("Blueprints/cloud");
      if (cloudFiles == null)
        return;
      List<MyGuiControlListbox.Item> list = new List<MyGuiControlListbox.Item>();
      Dictionary<string, MyBlueprintItemInfo> dictionary = new Dictionary<string, MyBlueprintItemInfo>();
      foreach (MyCloudFileInfo myCloudFileInfo in cloudFiles)
      {
        string[] strArray = myCloudFileInfo.Name.Split('/');
        string key = strArray[strArray.Length - 2];
        MyBlueprintItemInfo blueprintItemInfo = (MyBlueprintItemInfo) null;
        if (!dictionary.TryGetValue(key, out blueprintItemInfo))
        {
          blueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.CLOUD)
          {
            TimeCreated = new DateTime?(DateTime.FromFileTimeUtc(myCloudFileInfo.Timestamp)),
            TimeUpdated = new DateTime?(DateTime.FromFileTimeUtc(myCloudFileInfo.Timestamp)),
            BlueprintName = key,
            CloudInfo = myCloudFileInfo
          };
          StringBuilder text = new StringBuilder(key);
          string name = myCloudFileInfo.Name;
          object obj1 = (object) blueprintItemInfo;
          string normal = MyGuiConstants.TEXTURE_ICON_MODS_CLOUD.Normal;
          object userData = obj1;
          MyGuiControlListbox.Item obj2 = new MyGuiControlListbox.Item(text, name, normal, userData);
          dictionary.Add(key, blueprintItemInfo);
          list.Add(obj2);
        }
        if (myCloudFileInfo.Name.EndsWith("B5"))
          blueprintItemInfo.CloudPathPB = myCloudFileInfo.Name;
        else if (myCloudFileInfo.Name.EndsWith(MyBlueprintUtils.BLUEPRINT_LOCAL_NAME))
          blueprintItemInfo.CloudPathXML = myCloudFileInfo.Name;
      }
      this.SortBlueprints(list, MyBlueprintTypeEnum.CLOUD);
      foreach (MyGuiControlListbox.Item obj in list)
        MyGuiBlueprintScreen.m_blueprintList.Add(obj);
    }

    private void GetBlueprints(string directory, MyBlueprintTypeEnum type)
    {
      List<MyGuiControlListbox.Item> list1 = new List<MyGuiControlListbox.Item>();
      List<MyGuiControlListbox.Item> list2 = new List<MyGuiControlListbox.Item>();
      if (!Directory.Exists(directory))
        return;
      string[] directories = Directory.GetDirectories(directory);
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      foreach (string str in directories)
      {
        stringList1.Add(str + "\\bp.sbc");
        string[] strArray = str.Split('\\');
        stringList2.Add(strArray[strArray.Length - 1]);
      }
      for (int index = 0; index < stringList2.Count; ++index)
      {
        string str = stringList2[index];
        string path = stringList1[index];
        MyBlueprintItemInfo blueprintItemInfo = new MyBlueprintItemInfo(type)
        {
          TimeCreated = new DateTime?(File.GetCreationTimeUtc(path)),
          TimeUpdated = new DateTime?(File.GetLastWriteTimeUtc(path)),
          BlueprintName = str
        };
        string empty = string.Empty;
        string normal;
        if (!File.Exists(path))
        {
          blueprintItemInfo.IsDirectory = true;
          normal = MyGuiConstants.TEXTURE_ICON_MODS_LOCAL.Normal;
        }
        else
          normal = MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL.Normal;
        StringBuilder text = new StringBuilder(str);
        string toolTip = str;
        object obj1 = (object) blueprintItemInfo;
        string icon = normal;
        object userData = obj1;
        MyGuiControlListbox.Item obj2 = new MyGuiControlListbox.Item(text, toolTip, icon, userData);
        if (blueprintItemInfo.IsDirectory)
          list2.Add(obj2);
        else
          list1.Add(obj2);
      }
      if (!string.IsNullOrEmpty(MyGuiBlueprintScreen.m_currentLocalDirectory))
      {
        MyBlueprintItemInfo blueprintItemInfo = new MyBlueprintItemInfo(type)
        {
          TimeCreated = new DateTime?(DateTime.Now),
          TimeUpdated = new DateTime?(DateTime.Now),
          BlueprintName = string.Empty,
          IsDirectory = true
        };
        StringBuilder text = new StringBuilder("[..]");
        string currentLocalDirectory = MyGuiBlueprintScreen.m_currentLocalDirectory;
        object obj1 = (object) blueprintItemInfo;
        string normal = MyGuiConstants.TEXTURE_ICON_MODS_LOCAL.Normal;
        object userData = obj1;
        MyGuiControlListbox.Item obj2 = new MyGuiControlListbox.Item(text, currentLocalDirectory, normal, userData);
        MyGuiBlueprintScreen.m_blueprintList.Add(obj2);
      }
      this.SortBlueprints(list2, MyBlueprintTypeEnum.LOCAL);
      foreach (MyGuiControlListbox.Item obj in list2)
        MyGuiBlueprintScreen.m_blueprintList.Add(obj);
      this.SortBlueprints(list1, MyBlueprintTypeEnum.LOCAL);
      foreach (MyGuiControlListbox.Item obj in list1)
        MyGuiBlueprintScreen.m_blueprintList.Add(obj);
    }

    private bool ValidateModInfo(MyObjectBuilder_ModInfo info) => info != null && info.SubtypeName != null;

    private void GetWorkshopItemsSteam()
    {
      List<MyGuiControlListbox.Item> list = new List<MyGuiControlListbox.Item>();
      for (int index = 0; index < MyGuiBlueprintScreen.m_subscribedItemsList.Count; ++index)
      {
        MyWorkshopItem subscribedItems = MyGuiBlueprintScreen.m_subscribedItemsList[index];
        string title = subscribedItems.Title;
        MyBlueprintItemInfo blueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.WORKSHOP)
        {
          Item = subscribedItems,
          BlueprintName = title
        };
        StringBuilder text = new StringBuilder(title);
        string toolTip = title;
        object obj1 = (object) blueprintItemInfo;
        string normal = MyGuiConstants.GetWorkshopIcon(subscribedItems).Normal;
        object userData = obj1;
        MyGuiControlListbox.Item obj2 = new MyGuiControlListbox.Item(text, toolTip, normal, userData);
        list.Add(obj2);
      }
      this.SortBlueprints(list, MyBlueprintTypeEnum.WORKSHOP);
      foreach (MyGuiControlListbox.Item obj in list)
        MyGuiBlueprintScreen.m_blueprintList.Add(obj);
    }

    private bool IsExtracted(MyWorkshopItem subItem) => Directory.Exists(Path.Combine(MyGuiBlueprintScreen.TEMP_PATH, subItem.Id.ToString()));

    private void ExtractWorkshopItem(MyWorkshopItem subItem)
    {
      if (MyFileSystem.IsDirectory(subItem.Folder))
      {
        MyObjectBuilder_ModInfo objectBuilderModInfo = new MyObjectBuilder_ModInfo();
        objectBuilderModInfo.SubtypeName = subItem.Title;
        objectBuilderModInfo.WorkshopId = subItem.Id;
        objectBuilderModInfo.SteamIDOwner = subItem.OwnerId;
      }
      else
      {
        try
        {
          string folder = subItem.Folder;
          string tempPath1 = MyGuiBlueprintScreen.TEMP_PATH;
          ulong id = subItem.Id;
          string path2_1 = id.ToString();
          string str = Path.Combine(tempPath1, path2_1);
          if (Directory.Exists(str))
            Directory.Delete(str, true);
          Directory.CreateDirectory(str);
          MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(folder);
          MyObjectBuilder_ModInfo objectBuilderModInfo = new MyObjectBuilder_ModInfo();
          objectBuilderModInfo.SubtypeName = subItem.Title;
          objectBuilderModInfo.WorkshopId = subItem.Id;
          objectBuilderModInfo.SteamIDOwner = subItem.OwnerId;
          string tempPath2 = MyGuiBlueprintScreen.TEMP_PATH;
          id = subItem.Id;
          string path2_2 = id.ToString();
          string path = Path.Combine(tempPath2, path2_2, "info.temp");
          if (File.Exists(path))
            File.Delete(path);
          MyObjectBuilderSerializer.SerializeXML(path, false, (MyObjectBuilder_Base) objectBuilderModInfo);
          if (myZipArchive.FileExists(this.m_thumbImageName))
          {
            Stream stream = myZipArchive.GetFile(this.m_thumbImageName).GetStream();
            if (stream != null)
            {
              using (FileStream fileStream = File.Create(Path.Combine(str, this.m_thumbImageName)))
                stream.CopyTo((Stream) fileStream);
            }
            stream.Close();
          }
          myZipArchive.Dispose();
        }
        catch (IOException ex)
        {
          MyLog.Default.WriteLine((Exception) ex);
        }
      }
      MyGuiControlListbox.Item listItem = new MyGuiControlListbox.Item(new StringBuilder(subItem.Title), subItem.Title, MyGuiConstants.GetWorkshopIcon(subItem).Normal, (object) new MyBlueprintItemInfo(MyBlueprintTypeEnum.WORKSHOP)
      {
        Item = subItem
      });
      if (MyGuiBlueprintScreen.m_blueprintList.Items.FindIndex((Predicate<MyGuiControlListbox.Item>) (item => (long) (item.UserData as MyBlueprintItemInfo).Item.Id == (long) (listItem.UserData as MyBlueprintItemInfo).Item.Id && (item.UserData as MyBlueprintItemInfo).Type == MyBlueprintTypeEnum.WORKSHOP)) != -1)
        return;
      MyGuiBlueprintScreen.m_blueprintList.Add(listItem);
    }

    private DirectoryInfo CreateTempDirectory() => Directory.CreateDirectory(MyGuiBlueprintScreen.TEMP_PATH);

    private void DownloadBlueprints()
    {
      MyGuiBlueprintScreen.m_downloadFromSteam = true;
      MyGuiBlueprintScreen.m_subscribedItemsList.Clear();
      (MyGameServiceCallResult, string) blueprintsBlocking = MyWorkshop.GetSubscribedBlueprintsBlocking(MyGuiBlueprintScreen.m_subscribedItemsList);
      Directory.CreateDirectory(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP);
      foreach (MyWorkshopItem subscribedItems in MyGuiBlueprintScreen.m_subscribedItemsList)
      {
        if (File.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, subscribedItems.Id.ToString() + MyBlueprintUtils.BLUEPRINT_WORKSHOP_EXTENSION)))
        {
          MyGuiBlueprintScreen.m_downloadFinished.Add(subscribedItems.Id);
        }
        else
        {
          this.DownloadBlueprintFromSteam(subscribedItems);
          MyGuiBlueprintScreen.m_downloadFinished.Add(subscribedItems.Id);
        }
      }
      MyGuiBlueprintScreen.m_needsExtract = true;
      MyGuiBlueprintScreen.m_downloadFromSteam = false;
      if (blueprintsBlocking.Item1 == MyGameServiceCallResult.OK)
        return;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(MyWorkshop.GetWorkshopErrorText(blueprintsBlocking.Item1, blueprintsBlocking.Item2, true)), messageCaption: MyTexts.Get(MyCommonTexts.Error)));
    }

    private void GetWorkshopBlueprints() => MyGuiBlueprintScreen.Task = Parallel.Start(new Action(this.DownloadBlueprints));

    public override void RefreshBlueprintList(bool fromTask = false)
    {
      MyGuiBlueprintScreen.m_blueprintList.StoreSituation();
      MyGuiBlueprintScreen.m_blueprintList.Items.Clear();
      this.GetLocalBlueprintNames(fromTask);
      this.m_selectedItem = (MyGuiControlListbox.Item) null;
      this.m_screenshotButton.Enabled = false;
      this.m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
      this.m_detailsButton.Enabled = false;
      this.m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
      this.m_replaceButton.Enabled = false;
      this.m_replaceButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
      this.m_deleteButton.Enabled = false;
      this.m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
      this.m_okButton.Enabled = false;
      this.m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
      MyGuiBlueprintScreen.m_blueprintList.RestoreSituation(false, true);
      this.OnSearchTextChange(this.m_searchBox);
    }

    private void ReloadTextures()
    {
      List<string> textures = new List<string>();
      ProbeDir(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL);
      ProbeDir(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY);
      ProbeDir(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, "temp"));
      MyRenderProxy.UnloadTextures(textures);

      void ProbeDir(string rootDir)
      {
        if (!Directory.Exists(rootDir))
          return;
        textures.AddRange(MyFileSystem.GetFiles(rootDir, "*/" + this.m_thumbImageName));
      }
    }

    public void RefreshAndReloadBlueprintList()
    {
      MyGuiBlueprintScreen.m_blueprintList.StoreSituation();
      MyGuiBlueprintScreen.m_blueprintList.Items.Clear();
      this.GetLocalBlueprintNames(true);
      this.ReloadTextures();
      MyGuiBlueprintScreen.m_blueprintList.RestoreSituation(false, true);
      this.OnSearchTextChange(this.m_searchBox);
    }

    private void OnSearchClear(MyGuiControlButton button) => this.m_searchBox.Text = "";

    private void OnMouseOverItem(MyGuiControlListbox listBox)
    {
      MyGuiControlListbox.Item mouseOverItem = listBox.MouseOverItem;
      if (this.m_previousItem == mouseOverItem)
        return;
      this.m_previousItem = mouseOverItem;
      if (mouseOverItem == null)
      {
        this.m_thumbnailImage.Visible = false;
      }
      else
      {
        string str = string.Empty;
        MyBlueprintItemInfo userData = mouseOverItem.UserData as MyBlueprintItemInfo;
        if (userData.Type == MyBlueprintTypeEnum.LOCAL)
          str = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, MyGuiBlueprintScreen.m_currentLocalDirectory, mouseOverItem.Text.ToString(), this.m_thumbImageName);
        else if (userData.Type == MyBlueprintTypeEnum.WORKSHOP)
        {
          ulong id = userData.Item.Id;
          if (userData.Item != null)
          {
            bool flag1 = false;
            if (MyFileSystem.IsDirectory(userData.Item.Folder))
            {
              str = Path.Combine(userData.Item.Folder, this.m_thumbImageName);
              flag1 = true;
            }
            else
              str = Path.Combine(MyGuiBlueprintScreen.TEMP_PATH, id.ToString(), this.m_thumbImageName);
            int num = MyGuiBlueprintScreen.m_downloadQueued.Contains(userData.Item.Id) ? 1 : 0;
            bool flag2 = MyGuiBlueprintScreen.m_downloadFinished.Contains(userData.Item.Id);
            MyWorkshopItem worshopData = userData.Item;
            if (flag2 && !this.IsExtracted(worshopData) && !flag1)
            {
              MyGuiBlueprintScreen.m_blueprintList.Enabled = false;
              this.m_okButton.Enabled = false;
              this.m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
              this.ExtractWorkshopItem(worshopData);
              MyGuiBlueprintScreen.m_blueprintList.Enabled = true;
              this.m_okButton.Enabled = true;
              this.m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltip);
            }
            if (num == 0 && !flag2)
            {
              MyGuiBlueprintScreen.m_blueprintList.Enabled = false;
              this.m_okButton.Enabled = false;
              MyGuiBlueprintScreen.m_downloadQueued.Add(userData.Item.Id);
              MyGuiBlueprintScreen.Task = Parallel.Start((Action) (() => this.DownloadBlueprintFromSteam(worshopData)), (Action) (() => this.OnBlueprintDownloadedThumbnail(worshopData)));
              str = string.Empty;
            }
          }
        }
        else if (userData.Type == MyBlueprintTypeEnum.DEFAULT)
          str = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, mouseOverItem.Text.ToString(), this.m_thumbImageName);
        if (File.Exists(str))
        {
          this.m_preloadedTextures.Clear();
          this.m_preloadedTextures.Add(str);
          MyRenderProxy.PreloadTextures((IEnumerable<string>) this.m_preloadedTextures, TextureType.GUIWithoutPremultiplyAlpha);
          this.m_thumbnailImage.SetTexture(str);
          if (this.m_activeDetail || !this.m_thumbnailImage.IsAnyTextureValid())
            return;
          this.m_thumbnailImage.Visible = true;
        }
        else
        {
          this.m_thumbnailImage.Visible = false;
          this.m_thumbnailImage.SetTexture();
        }
      }
    }

    private void OnSelectItem(MyGuiControlListbox list)
    {
      if (list.SelectedItems.Count == 0)
        return;
      this.m_selectedItem = list.SelectedItems[0];
      this.m_okButton.Enabled = true;
      this.m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltip);
      this.m_detailsButton.Enabled = true;
      this.m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_DetailsTooltip);
      this.m_screenshotButton.Enabled = true;
      this.m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltip);
      this.m_replaceButton.Enabled = this.m_clipboard.HasCopiedGrids();
      this.m_replaceButton.SetToolTip(this.m_replaceButton.Enabled ? MyCommonTexts.Blueprints_ReplaceBlueprintTooltip : MyCommonTexts.Blueprints_CreateTooltipDisabled);
      MyBlueprintItemInfo userData = this.m_selectedItem.UserData as MyBlueprintItemInfo;
      MyBlueprintTypeEnum type = userData.Type;
      ulong id = userData.Item.Id;
      string path = "";
      switch (type)
      {
        case MyBlueprintTypeEnum.WORKSHOP:
          if (userData.Item == null)
            return;
          path = !MyFileSystem.IsDirectory(userData.Item.Folder) ? Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, "temp", id.ToString(), this.m_thumbImageName) : Path.Combine(userData.Item.Folder, this.m_thumbImageName);
          this.m_screenshotButton.Enabled = false;
          this.m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
          this.m_replaceButton.Enabled = false;
          this.m_deleteButton.Enabled = false;
          this.m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
          break;
        case MyBlueprintTypeEnum.LOCAL:
          path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, MyGuiBlueprintScreen.m_currentLocalDirectory, this.m_selectedItem.Text.ToString(), this.m_thumbImageName);
          this.m_deleteButton.Enabled = true;
          this.m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltip);
          if (userData.IsDirectory)
          {
            this.m_detailsButton.Enabled = false;
            this.m_screenshotButton.Enabled = false;
            this.m_replaceButton.Enabled = false;
            break;
          }
          break;
        case MyBlueprintTypeEnum.SHARED:
          this.m_replaceButton.Enabled = false;
          this.m_screenshotButton.Enabled = false;
          this.m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
          this.m_detailsButton.Enabled = false;
          this.m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
          this.m_deleteButton.Enabled = false;
          this.m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
          break;
        case MyBlueprintTypeEnum.DEFAULT:
          path = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, this.m_selectedItem.Text.ToString(), this.m_thumbImageName);
          this.m_replaceButton.Enabled = false;
          this.m_screenshotButton.Enabled = false;
          this.m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
          this.m_deleteButton.Enabled = false;
          this.m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
          break;
        case MyBlueprintTypeEnum.CLOUD:
          this.m_deleteButton.Enabled = true;
          this.m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltip);
          this.m_detailsButton.Enabled = true;
          this.m_screenshotButton.Enabled = false;
          break;
      }
      if (File.Exists(path))
        this.m_selectedThumbnailPath = path;
      else
        this.m_selectedThumbnailPath = (string) null;
    }

    private bool ValidateSelecteditem() => this.m_selectedItem != null && this.m_selectedItem.UserData != null && this.m_selectedItem.Text != null;

    private bool CopySelectedItemToClipboard()
    {
      if (!this.ValidateSelecteditem())
        return false;
      MyObjectBuilder_Definitions prefab = (MyObjectBuilder_Definitions) null;
      MyBlueprintItemInfo userData = this.m_selectedItem.UserData as MyBlueprintItemInfo;
      switch (userData.Type)
      {
        case MyBlueprintTypeEnum.WORKSHOP:
          ulong id = userData.Item.Id;
          string folder = userData.Item.Folder;
          if (File.Exists(folder) || MyFileSystem.IsDirectory(folder))
          {
            MyGuiBlueprintScreen.m_LoadPrefabData = new MyGuiBlueprintScreen.LoadPrefabData(prefab, folder, this, new ulong?(id));
            MyGuiBlueprintScreen.Task = Parallel.Start(new Action<WorkData>(MyGuiBlueprintScreen.m_LoadPrefabData.CallLoadWorkshopPrefab), (Action<WorkData>) null, (WorkData) MyGuiBlueprintScreen.m_LoadPrefabData);
            break;
          }
          break;
        case MyBlueprintTypeEnum.LOCAL:
          string path1 = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, MyGuiBlueprintScreen.m_currentLocalDirectory, this.m_selectedItem.Text.ToString(), "bp.sbc");
          if (File.Exists(path1))
          {
            MyGuiBlueprintScreen.m_LoadPrefabData = new MyGuiBlueprintScreen.LoadPrefabData(prefab, path1, this);
            MyGuiBlueprintScreen.Task = Parallel.Start(new Action<WorkData>(MyGuiBlueprintScreen.m_LoadPrefabData.CallLoadPrefab), (Action<WorkData>) null, (WorkData) MyGuiBlueprintScreen.m_LoadPrefabData);
            break;
          }
          break;
        case MyBlueprintTypeEnum.SHARED:
          return false;
        case MyBlueprintTypeEnum.DEFAULT:
          string path2 = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, this.m_selectedItem.Text.ToString(), "bp.sbc");
          if (File.Exists(path2))
          {
            MyGuiBlueprintScreen.m_LoadPrefabData = new MyGuiBlueprintScreen.LoadPrefabData(prefab, path2, this);
            MyGuiBlueprintScreen.Task = Parallel.Start(new Action<WorkData>(MyGuiBlueprintScreen.m_LoadPrefabData.CallLoadPrefab), (Action<WorkData>) null, (WorkData) MyGuiBlueprintScreen.m_LoadPrefabData);
            break;
          }
          break;
        case MyBlueprintTypeEnum.CLOUD:
          MyGuiBlueprintScreen.m_LoadPrefabData = new MyGuiBlueprintScreen.LoadPrefabData(prefab, userData, this);
          MyGuiBlueprintScreen.Task = Parallel.Start(new Action<WorkData>(MyGuiBlueprintScreen.m_LoadPrefabData.CallLoadPrefabFromCloud), (Action<WorkData>) null, (WorkData) MyGuiBlueprintScreen.m_LoadPrefabData);
          break;
      }
      return false;
    }

    internal void OnPrefabLoaded(MyObjectBuilder_Definitions prefab)
    {
      this.m_blueprintBeingLoaded = false;
      if (prefab != null)
      {
        if (MySandboxGame.Static.SessionCompatHelper != null)
          MySandboxGame.Static.SessionCompatHelper.CheckAndFixPrefab(prefab);
        if (!MyGuiBlueprintScreen.CheckBlueprintForMods(prefab))
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextDoYouWantToPasteGridWithMissingBlocks), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
          {
            if (result == MyGuiScreenMessageBox.ResultEnum.YES)
            {
              this.CloseScreen(false);
              if (MyGuiBlueprintScreen.CopyBlueprintPrefabToClipboard(prefab, this.m_clipboard) && this.m_accessType == MyBlueprintAccessType.NORMAL)
              {
                if (MySession.Static.IsCopyPastingEnabled)
                  MySandboxGame.Static.Invoke((Action) (() => MyClipboardComponent.Static.Paste()), "BlueprintSelectionAutospawn2");
                else
                  MyClipboardComponent.ShowCannotPasteWarning();
              }
            }
            if (result != MyGuiScreenMessageBox.ResultEnum.NO)
              return;
            this.m_selectedItem = MyGuiBlueprintScreen.m_blueprintList.SelectedItems[0];
            this.m_okButton.Enabled = true;
            this.m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltip);
          }))));
        }
        else
        {
          this.CloseScreen(false);
          if (!MyGuiBlueprintScreen.CopyBlueprintPrefabToClipboard(prefab, this.m_clipboard) || this.m_accessType != MyBlueprintAccessType.NORMAL)
            return;
          if (MySession.Static.IsCopyPastingEnabled)
            MySandboxGame.Static.Invoke((Action) (() => MyClipboardComponent.Static.Paste()), "BlueprintSelectionAutospawn1");
          else
            MyClipboardComponent.ShowCannotPasteWarning();
        }
      }
      else
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Error);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.CannotFindBlueprint), messageCaption: messageCaption));
      }
    }

    private static bool CheckBlueprintForMods(MyObjectBuilder_Definitions prefab)
    {
      if (prefab.ShipBlueprints == null)
        return true;
      MyObjectBuilder_CubeGrid[] cubeGrids = prefab.ShipBlueprints[0].CubeGrids;
      if (cubeGrids == null || cubeGrids.Length == 0)
        return true;
      foreach (MyObjectBuilder_CubeGrid objectBuilderCubeGrid in cubeGrids)
      {
        foreach (MyObjectBuilder_Base cubeBlock in objectBuilderCubeGrid.CubeBlocks)
        {
          MyDefinitionId id = cubeBlock.GetId();
          MyCubeBlockDefinition blockDefinition = (MyCubeBlockDefinition) null;
          if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(id, out blockDefinition))
            return false;
        }
      }
      return true;
    }

    public static bool CopyBlueprintPrefabToClipboard(
      MyObjectBuilder_Definitions prefab,
      MyGridClipboard clipboard,
      bool setOwner = true)
    {
      if (prefab.ShipBlueprints == null)
        return false;
      MyObjectBuilder_CubeGrid[] cubeGrids = prefab.ShipBlueprints[0].CubeGrids;
      if (cubeGrids == null || cubeGrids.Length == 0)
        return false;
      BoundingSphere boundingSphere1 = cubeGrids[0].CalculateBoundingSphere();
      MyPositionAndOrientation positionAndOrientation = cubeGrids[0].PositionAndOrientation.Value;
      MatrixD world = MatrixD.CreateWorld((Vector3D) positionAndOrientation.Position, (Vector3) positionAndOrientation.Forward, (Vector3) positionAndOrientation.Up);
      Matrix matrix = Matrix.Normalize(Matrix.Invert((Matrix) ref world));
      BoundingSphere boundingSphere2 = boundingSphere1.Transform((Matrix) ref world);
      Vector3 dragPointDelta = Vector3.TransformNormal((Vector3) (Vector3D) positionAndOrientation.Position - boundingSphere2.Center, matrix);
      float dragVectorLength = boundingSphere1.Radius + 10f;
      if (setOwner)
      {
        foreach (MyObjectBuilder_CubeGrid objectBuilderCubeGrid in cubeGrids)
        {
          foreach (MyObjectBuilder_CubeBlock cubeBlock in objectBuilderCubeGrid.CubeBlocks)
          {
            if (cubeBlock.Owner != 0L)
              cubeBlock.Owner = MySession.Static.LocalPlayerId;
          }
        }
      }
      if (MyFakes.ENABLE_FRACTURE_COMPONENT)
      {
        for (int index = 0; index < cubeGrids.Length; ++index)
          cubeGrids[index] = MyFracturedBlock.ConvertFracturedBlocksToComponents(cubeGrids[index]);
      }
      clipboard.SetGridFromBuilders(cubeGrids, dragPointDelta, dragVectorLength);
      clipboard.ShowModdedBlocksWarning = false;
      return true;
    }

    private void OnSearchTextChange(MyGuiControlTextbox box)
    {
      if (box.Text != "")
      {
        string[] strArray = box.Text.Split(' ');
        foreach (MyGuiControlListbox.Item obj in MyGuiBlueprintScreen.m_blueprintList.Items)
        {
          string lower = obj.Text.ToString().ToLower();
          bool flag = true;
          foreach (string str in strArray)
          {
            if (!lower.Contains(str.ToLower()))
            {
              flag = false;
              break;
            }
          }
          obj.Visible = flag;
        }
      }
      else
      {
        foreach (MyGuiControlListbox.Item obj in MyGuiBlueprintScreen.m_blueprintList.Items)
          obj.Visible = true;
      }
      MyGuiBlueprintScreen.m_blueprintList.ScrollToolbarToTop();
    }

    private void OpenSharedBlueprint(MyBlueprintItemInfo itemInfo) => MyGuiSandbox.OpenUrl(itemInfo.Item.GetItemUrl(), UrlOpenMode.SteamOrExternalWithConfirm, new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.SharedBlueprintQuestion), (object) itemInfo.Item.ServiceName), MyTexts.Get(MySpaceTexts.SharedBlueprint), new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.SharedBlueprintQuestion), (object) itemInfo.Item.ServiceName), MyTexts.Get(MySpaceTexts.SharedBlueprint), (Action<bool>) (success =>
    {
      MyGuiBlueprintScreen.m_recievedBlueprints.Remove(this.m_selectedItem);
      this.m_selectedItem = (MyGuiControlListbox.Item) null;
      this.RefreshBlueprintList(false);
    }));

    private void OnItemDoubleClick(MyGuiControlListbox list)
    {
      this.m_selectedItem = list.SelectedItems[0];
      this.Ok();
    }

    private void CopyBlueprintAndClose()
    {
      if (!this.CopySelectedItemToClipboard())
        return;
      this.CloseScreen(false);
    }

    private void Ok()
    {
      if (this.m_selectedItem == null)
      {
        this.CloseScreen(false);
      }
      else
      {
        MyBlueprintItemInfo itemInfo = this.m_selectedItem.UserData as MyBlueprintItemInfo;
        if (itemInfo.IsDirectory)
        {
          if (string.IsNullOrEmpty(itemInfo.BlueprintName))
          {
            string[] strArray = MyGuiBlueprintScreen.m_currentLocalDirectory.Split(Path.DirectorySeparatorChar);
            if (strArray.Length > 1)
            {
              strArray[strArray.Length - 1] = string.Empty;
              MyGuiBlueprintScreen.m_currentLocalDirectory = Path.Combine(strArray);
            }
            else
              MyGuiBlueprintScreen.m_currentLocalDirectory = string.Empty;
          }
          else
            MyGuiBlueprintScreen.m_currentLocalDirectory = Path.Combine(MyGuiBlueprintScreen.m_currentLocalDirectory, itemInfo.BlueprintName);
          MyGuiBlueprintScreen.CheckCurrentLocalDirectory();
          this.RefreshAndReloadBlueprintList();
        }
        else
        {
          this.m_blueprintBeingLoaded = true;
          switch (itemInfo.Type)
          {
            case MyBlueprintTypeEnum.WORKSHOP:
              MyGuiBlueprintScreen.Task = Parallel.Start((Action) (() =>
              {
                if (MyWorkshop.IsUpToDate(itemInfo.Item))
                  return;
                this.DownloadBlueprintFromSteam(itemInfo.Item);
              }), (Action) (() => this.CopyBlueprintAndClose()));
              break;
            case MyBlueprintTypeEnum.LOCAL:
            case MyBlueprintTypeEnum.DEFAULT:
            case MyBlueprintTypeEnum.CLOUD:
              this.CopyBlueprintAndClose();
              break;
            case MyBlueprintTypeEnum.SHARED:
              this.OpenSharedBlueprint(itemInfo);
              break;
          }
        }
      }
    }

    private static void CheckCurrentLocalDirectory()
    {
      if (Directory.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, MyGuiBlueprintScreen.m_currentLocalDirectory)))
        return;
      MyGuiBlueprintScreen.m_currentLocalDirectory = string.Empty;
    }

    private void OnOk(MyGuiControlButton button)
    {
      button.Enabled = false;
      this.Ok();
    }

    private void OnCancel(MyGuiControlButton button) => this.CloseScreen(false);

    private void OnReload(MyGuiControlButton button)
    {
      this.m_selectedItem = (MyGuiControlListbox.Item) null;
      this.m_detailsButton.Enabled = false;
      this.m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
      this.m_screenshotButton.Enabled = false;
      this.m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
      MyGuiBlueprintScreen.m_downloadFinished.Clear();
      MyGuiBlueprintScreen.m_downloadQueued.Clear();
      this.RefreshAndReloadBlueprintList();
    }

    private void OnDetails(MyGuiControlButton button)
    {
      if (this.m_selectedItem == null)
      {
        if (!this.m_activeDetail)
          return;
        MyScreenManager.RemoveScreen((MyGuiScreenBase) this.m_detailScreen);
      }
      else if (this.m_activeDetail)
      {
        MyScreenManager.RemoveScreen((MyGuiScreenBase) this.m_detailScreen);
      }
      else
      {
        if (this.m_activeDetail || !(this.m_selectedItem.UserData is MyBlueprintItemInfo userData))
          return;
        switch (userData.Type)
        {
          case MyBlueprintTypeEnum.WORKSHOP:
            this.OpenSteamWorkshopDetail(userData);
            break;
          case MyBlueprintTypeEnum.LOCAL:
            this.OpenLocalBlueprintDetail();
            break;
          case MyBlueprintTypeEnum.DEFAULT:
            this.OpenDefaultBlueprintDetail();
            break;
          case MyBlueprintTypeEnum.CLOUD:
            this.OpenCloudBlueprintDetail();
            break;
        }
      }
    }

    private void OpenCloudBlueprintDetail()
    {
      this.m_thumbnailImage.Visible = false;
      this.m_detailScreen = (MyGuiDetailScreenBase) new MyGuiDetailScreenCloud((Action<MyGuiControlListbox.Item>) (item =>
      {
        if (item == null)
        {
          this.m_screenshotButton.Enabled = false;
          this.m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
          this.m_detailsButton.Enabled = false;
          this.m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
          this.m_replaceButton.Enabled = false;
          this.m_deleteButton.Enabled = false;
          this.m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
        }
        this.m_selectedItem = item;
        this.m_activeDetail = false;
        this.m_detailScreen = (MyGuiDetailScreenBase) null;
        if (!MyGuiBlueprintScreen.Task.IsComplete)
          return;
        this.RefreshBlueprintList(false);
      }), this.m_selectedItem, this, this.m_selectedThumbnailPath, this.m_textScale);
      this.m_activeDetail = true;
      MyScreenManager.InputToNonFocusedScreens = true;
      MyScreenManager.AddScreen((MyGuiScreenBase) this.m_detailScreen);
    }

    private void OpenDefaultBlueprintDetail()
    {
      if (File.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, this.m_selectedItem.Text.ToString(), "bp.sbc")))
      {
        this.m_thumbnailImage.Visible = false;
        this.m_detailScreen = (MyGuiDetailScreenBase) new MyGuiDetailScreenDefault((Action<MyGuiControlListbox.Item>) (item =>
        {
          if (item == null)
          {
            this.m_screenshotButton.Enabled = false;
            this.m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
            this.m_detailsButton.Enabled = false;
            this.m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
            this.m_replaceButton.Enabled = false;
            this.m_deleteButton.Enabled = false;
            this.m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
          }
          this.m_selectedItem = item;
          this.m_activeDetail = false;
          this.m_detailScreen = (MyGuiDetailScreenBase) null;
          if (!MyGuiBlueprintScreen.Task.IsComplete)
            return;
          this.RefreshBlueprintList(false);
        }), this.m_selectedItem, this, this.m_selectedThumbnailPath, this.m_textScale);
        this.m_activeDetail = true;
        MyScreenManager.InputToNonFocusedScreens = true;
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_detailScreen);
      }
      else
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Error);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.CannotFindBlueprint), messageCaption: messageCaption));
      }
    }

    private void OpenLocalBlueprintDetail()
    {
      if (File.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, MyGuiBlueprintScreen.m_currentLocalDirectory, this.m_selectedItem.Text.ToString(), "bp.sbc")))
      {
        this.m_thumbnailImage.Visible = false;
        this.m_detailScreen = (MyGuiDetailScreenBase) new MyGuiDetailScreenLocal((Action<MyGuiControlListbox.Item>) (item =>
        {
          if (item == null)
          {
            this.m_screenshotButton.Enabled = false;
            this.m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
            this.m_detailsButton.Enabled = false;
            this.m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
            this.m_replaceButton.Enabled = false;
            this.m_deleteButton.Enabled = false;
            this.m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
          }
          this.m_selectedItem = item;
          this.m_activeDetail = false;
          this.m_detailScreen = (MyGuiDetailScreenBase) null;
          if (!MyGuiBlueprintScreen.Task.IsComplete)
            return;
          this.RefreshBlueprintList(false);
        }), this.m_selectedItem, (MyGuiBlueprintScreenBase) this, this.m_selectedThumbnailPath, this.m_textScale, MyGuiBlueprintScreen.m_currentLocalDirectory);
        this.m_activeDetail = true;
        MyScreenManager.InputToNonFocusedScreens = true;
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_detailScreen);
      }
      else
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Error);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.CannotFindBlueprint), messageCaption: messageCaption));
      }
    }

    private void OpenSteamWorkshopDetail(MyBlueprintItemInfo blueprintInfo)
    {
      MyWorkshopItem workshopData = blueprintInfo.Item;
      MyGuiBlueprintScreen.Task = Parallel.Start((Action) (() =>
      {
        if (MyWorkshop.IsUpToDate(workshopData))
          return;
        this.DownloadBlueprintFromSteam(workshopData);
      }), (Action) (() => this.OnBlueprintDownloadedDetails(workshopData)));
    }

    private void DownloadBlueprintFromSteam(MyWorkshopItem item)
    {
      if (MyWorkshop.IsUpToDate(item))
        return;
      MyWorkshop.DownloadBlueprintBlockingUGC(item, false);
      this.ExtractWorkshopItem(item);
    }

    private void OnBlueprintDownloadedDetails(MyWorkshopItem workshopDetails)
    {
      if (File.Exists(workshopDetails.Folder))
      {
        this.m_thumbnailImage.Visible = false;
        this.m_detailScreen = (MyGuiDetailScreenBase) new MyGuiDetailScreenSteam((Action<MyGuiControlListbox.Item>) (item =>
        {
          this.m_selectedItem = item;
          this.m_activeDetail = false;
          this.m_detailScreen = (MyGuiDetailScreenBase) null;
          if (!MyGuiBlueprintScreen.Task.IsComplete)
            return;
          this.RefreshBlueprintList(false);
        }), this.m_selectedItem, this, this.m_selectedThumbnailPath, this.m_textScale);
        this.m_activeDetail = true;
        MyScreenManager.InputToNonFocusedScreens = true;
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_detailScreen);
      }
      else
      {
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Error);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.CannotFindBlueprint), messageCaption: messageCaption));
      }
    }

    private void OnBlueprintDownloadedThumbnail(MyWorkshopItem item)
    {
      this.m_okButton.Enabled = true;
      this.m_okButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltip);
      MyGuiBlueprintScreen.m_blueprintList.Enabled = true;
      string str = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, "temp", item.Id.ToString(), this.m_thumbImageName);
      if (File.Exists(str))
      {
        this.m_thumbnailImage.SetTexture(str);
        if (!this.m_activeDetail && this.m_thumbnailImage.IsAnyTextureValid())
          this.m_thumbnailImage.Visible = true;
      }
      else
      {
        this.m_thumbnailImage.Visible = false;
        this.m_thumbnailImage.SetTexture();
      }
      MyGuiBlueprintScreen.m_downloadQueued.Remove(item.Id);
      MyGuiBlueprintScreen.m_downloadFinished.Add(item.Id);
    }

    public void TakeScreenshot(string name)
    {
      MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, MyGuiBlueprintScreen.m_currentLocalDirectory, name, this.m_thumbImageName), false, true, false);
      this.m_thumbnailImage.Visible = true;
    }

    private void OnScreenshot(MyGuiControlButton button)
    {
      if (this.m_selectedItem == null)
        return;
      this.TakeScreenshot(this.m_selectedItem.Text.ToString());
    }

    public void CreateFromClipboard(bool withScreenshot = false, bool replace = false)
    {
      if (this.m_clipboard.CopiedGridsName == null)
        return;
      string str1 = MyUtils.StripInvalidChars(this.m_clipboard.CopiedGridsName);
      string str2 = str1;
      string str3 = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, MyGuiBlueprintScreen.m_currentLocalDirectory, str1);
      int num = 1;
      while (MyFileSystem.DirectoryExists(str3))
      {
        str2 = str1 + "_" + (object) num;
        str3 = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, MyGuiBlueprintScreen.m_currentLocalDirectory, str2);
        ++num;
      }
      Path.Combine(str3, this.m_thumbImageName);
      if (withScreenshot && !MySandboxGame.Config.EnableSteamCloud)
        this.TakeScreenshot(str2);
      MyObjectBuilder_ShipBlueprintDefinition newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ShipBlueprintDefinition>();
      newObject1.Id = (SerializableDefinitionId) new MyDefinitionId(new MyObjectBuilderType(typeof (MyObjectBuilder_ShipBlueprintDefinition)), MyUtils.StripInvalidChars(str1));
      newObject1.CubeGrids = this.m_clipboard.CopiedGrids.ToArray();
      newObject1.RespawnShip = false;
      newObject1.DisplayName = MyGameService.UserName;
      newObject1.OwnerSteamId = Sync.MyId;
      newObject1.CubeGrids[0].DisplayName = str1;
      MyObjectBuilder_Definitions newObject2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
      newObject2.ShipBlueprints = new MyObjectBuilder_ShipBlueprintDefinition[1];
      newObject2.ShipBlueprints[0] = newObject1;
      MyBlueprintUtils.SavePrefabToFile(newObject2, this.m_clipboard.CopiedGridsName, MyGuiBlueprintScreen.m_currentLocalDirectory, replace);
      this.RefreshBlueprintList(false);
    }

    private void OnDelete(MyGuiControlButton button)
    {
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Delete);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.DeleteBlueprintQuestion), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
      {
        if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES || this.m_selectedItem == null || !(this.m_selectedItem.UserData is MyBlueprintItemInfo userData))
          return;
        switch (userData.Type)
        {
          case MyBlueprintTypeEnum.LOCAL:
          case MyBlueprintTypeEnum.DEFAULT:
            if (this.DeleteItem(Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, MyGuiBlueprintScreen.m_currentLocalDirectory, userData.BlueprintName)))
            {
              this.ResetBlueprintUI();
              break;
            }
            break;
          case MyBlueprintTypeEnum.CLOUD:
            if (MyGameService.DeleteFromCloud("Blueprints/cloud/" + userData.BlueprintName + "/"))
            {
              this.ResetBlueprintUI();
              break;
            }
            break;
        }
        this.RefreshBlueprintList(false);
      }))));
    }

    private void ResetBlueprintUI()
    {
      this.m_deleteButton.Enabled = false;
      this.m_deleteButton.SetToolTip(MyCommonTexts.Blueprints_DeleteTooltipDisabled);
      this.m_detailsButton.Enabled = false;
      this.m_detailsButton.SetToolTip(MyCommonTexts.Blueprints_OkTooltipDisabled);
      this.m_screenshotButton.Enabled = false;
      this.m_screenshotButton.SetToolTip(MyCommonTexts.Blueprints_TakeScreenshotTooltipDisabled);
      this.m_replaceButton.Enabled = false;
      this.m_selectedItem = (MyGuiControlListbox.Item) null;
    }

    private void OnCreate(MyGuiControlButton button) => this.CreateFromClipboard();

    private void OnReplace(MyGuiControlButton button)
    {
      if (this.m_selectedItem == null)
        return;
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxTitle_Replace);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxDesc_Replace), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
      {
        if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        string path3 = this.m_selectedItem.Text.ToString();
        string str = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, MyGuiBlueprintScreen.m_currentLocalDirectory, path3, "bp.sbc");
        if (!File.Exists(str))
          return;
        MyObjectBuilder_Definitions prefab = MyBlueprintUtils.LoadPrefab(str);
        this.m_clipboard.CopiedGrids[0].DisplayName = path3;
        prefab.ShipBlueprints[0].CubeGrids = this.m_clipboard.CopiedGrids.ToArray();
        MyBlueprintUtils.SavePrefabToFile(prefab, this.m_clipboard.CopiedGridsName, MyGuiBlueprintScreen.m_currentLocalDirectory, true);
        this.RefreshBlueprintList(false);
      }))));
    }

    protected override void OnClosed()
    {
      base.OnClosed();
      if (!this.m_activeDetail)
        return;
      this.m_detailScreen.CloseScreen();
    }

    public override bool Update(bool hasFocus)
    {
      if (!MyGuiBlueprintScreen.m_blueprintList.IsMouseOver)
        this.m_thumbnailImage.Visible = false;
      if (!MyGuiBlueprintScreen.Task.IsComplete)
        this.m_wheel.Visible = true;
      if (MyGuiBlueprintScreen.Task.IsComplete)
      {
        this.m_wheel.Visible = false;
        if (MyGuiBlueprintScreen.m_needsExtract)
        {
          this.GetWorkshopItemsSteam();
          MyGuiBlueprintScreen.m_needsExtract = false;
          this.RefreshBlueprintList(false);
        }
      }
      return base.Update(hasFocus);
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      if (!this.m_blueprintBeingLoaded)
        return base.CloseScreen(isUnloading);
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxDesc_StillLoading);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxTitle_StillLoading), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        this.m_blueprintBeingLoaded = false;
        MyGuiBlueprintScreen.Task.valid = false;
        this.CloseScreen(isUnloading);
      }))));
      return false;
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (!MyInput.Static.IsNewKeyPressed(MyKeys.F12) && !MyInput.Static.IsNewKeyPressed(MyKeys.F11) && !MyInput.Static.IsNewKeyPressed(MyKeys.F10))
        return;
      this.CloseScreen(false);
    }

    private class LoadPrefabData : WorkData
    {
      private MyObjectBuilder_Definitions m_prefab;
      private string m_path;
      private MyGuiBlueprintScreen m_blueprintScreen;
      private ulong? m_id;
      private MyBlueprintItemInfo m_info;

      public LoadPrefabData(
        MyObjectBuilder_Definitions prefab,
        string path,
        MyGuiBlueprintScreen blueprintScreen,
        ulong? id = null)
      {
        this.m_prefab = prefab;
        this.m_path = path;
        this.m_blueprintScreen = blueprintScreen;
        this.m_id = id;
      }

      public LoadPrefabData(
        MyObjectBuilder_Definitions prefab,
        MyBlueprintItemInfo info,
        MyGuiBlueprintScreen blueprintScreen)
      {
        this.m_prefab = prefab;
        this.m_blueprintScreen = blueprintScreen;
        this.m_info = info;
      }

      public void CallLoadPrefab(WorkData workData)
      {
        this.m_prefab = MyBlueprintUtils.LoadPrefab(this.m_path);
        this.CallOnPrefabLoaded();
      }

      public void CallLoadWorkshopPrefab(WorkData workData)
      {
        this.m_prefab = MyBlueprintUtils.LoadWorkshopPrefab(this.m_path, this.m_id, "", true);
        this.CallOnPrefabLoaded();
      }

      public void CallLoadPrefabFromCloud(WorkData workData)
      {
        this.m_prefab = MyBlueprintUtils.LoadPrefabFromCloud(this.m_info);
        this.CallOnPrefabLoaded();
      }

      public void CallOnPrefabLoaded()
      {
        if (this.m_blueprintScreen.State != MyGuiScreenState.OPENED)
          return;
        this.m_blueprintScreen.OnPrefabLoaded(this.m_prefab);
      }
    }

    protected sealed class ShareBlueprintRequest\u003C\u003ESystem_UInt64\u0023System_String\u0023System_UInt64\u0023System_String : ICallSite<IMyEventOwner, ulong, string, ulong, string, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong workshopId,
        in string name,
        in ulong sendToId,
        in string senderName,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiBlueprintScreen.ShareBlueprintRequest(workshopId, name, sendToId, senderName);
      }
    }

    protected sealed class ShareBlueprintRequestClient\u003C\u003ESystem_UInt64\u0023System_String\u0023System_UInt64\u0023System_String : ICallSite<IMyEventOwner, ulong, string, ulong, string, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong workshopId,
        in string name,
        in ulong sendToId,
        in string senderName,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiBlueprintScreen.ShareBlueprintRequestClient(workshopId, name, sendToId, senderName);
      }
    }
  }
}
