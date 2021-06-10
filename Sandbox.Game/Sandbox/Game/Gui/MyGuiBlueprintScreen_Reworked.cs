// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiBlueprintScreen_Reworked
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
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
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
  public class MyGuiBlueprintScreen_Reworked : MyGuiScreenDebugBase
  {
    public static readonly float MAGIC_SPACING_BIG = 0.00535f;
    public static readonly float MAGIC_SPACING_SMALL = 0.00888f;
    private static readonly Vector2 SCREEN_SIZE = new Vector2(0.95f, 0.97f);
    private static readonly float MARGIN_TOP = 0.22f;
    private const float MAX_BLUEPRINT_NAME_LABEL_WIDTH = 0.4f;
    private static HashSet<WorkshopId> m_downloadQueued = new HashSet<WorkshopId>();
    private static MyConcurrentHashSet<WorkshopId> m_downloadFinished = new MyConcurrentHashSet<WorkshopId>();
    private static MyGuiBlueprintScreen_Reworked.MyWaitForScreenshotData m_waitingForScreenshot = new MyGuiBlueprintScreen_Reworked.MyWaitForScreenshotData();
    private static bool m_downloadFromSteam = true;
    private static bool m_needsExtract = false;
    private static bool m_showDlcIcons = false;
    private static List<MyBlueprintItemInfo> m_recievedBlueprints = new List<MyBlueprintItemInfo>();
    private readonly List<MyGuiControlImage> m_dlcIcons = new List<MyGuiControlImage>();
    private static LoadPrefabData m_LoadPrefabData;
    private static Dictionary<Content, List<MyWorkshopItem>> m_subscribedItemsListDict = new Dictionary<Content, List<MyWorkshopItem>>();
    private static Dictionary<Content, string> m_currentLocalDirectoryDict = new Dictionary<Content, string>();
    private static Dictionary<Content, MyGuiBlueprintScreen_Reworked.SortOption> m_selectedSortDict = new Dictionary<Content, MyGuiBlueprintScreen_Reworked.SortOption>();
    private static Dictionary<Content, MyBlueprintTypeEnum> m_selectedBlueprintTypeDict = new Dictionary<Content, MyBlueprintTypeEnum>();
    private static Dictionary<Content, bool> m_thumbnailsVisibleDict = new Dictionary<Content, bool>();
    public static Task Task;
    public static readonly FastResourceLock SubscribedItemsLock = new FastResourceLock();
    private float m_guiMultilineHeight;
    private float m_guiAdditionalInfoOffset;
    private MyGridClipboard m_clipboard;
    private MyBlueprintAccessType m_accessType;
    private bool m_allowCopyToClipboard;
    private MyObjectBuilder_Definitions m_loadedPrefab;
    private bool m_blueprintBeingLoaded;
    private Action<string> m_onScriptOpened;
    private Func<string> m_getCodeFromEditor;
    private Action m_onCloseAction;
    private MyBlueprintItemInfo m_selectedBlueprint;
    private bool m_wasJoystickLastUsed;
    private float m_margin_left;
    private float ratingButtonsGap = 0.01f;
    private MyGuiControlContentButton m_selectedButton;
    private MyGuiControlRadioButtonGroup m_BPTypesGroup;
    private MyGuiControlList m_BPList;
    private Content m_content;
    private bool m_wasPublished;
    private MyGuiControlSearchBox m_searchBox;
    private MyGuiControlMultilineText m_multiline;
    private MyGuiControlPanel m_detailsBackground;
    private MyGuiControlLabel m_detailName;
    private MyGuiControlLabel m_detailBlockCount;
    private MyGuiControlLabel m_detailBlockCountValue;
    private MyGuiControlLabel m_detailGridTypeValue;
    private MyGuiControlLabel m_detailSize;
    private MyGuiControlLabel m_detailSizeValue;
    private MyGuiControlLabel m_detailAuthor;
    private MyGuiControlLabel m_detailAuthorName;
    private MyGuiControlLabel m_detailDLC;
    private MyGuiControlRating m_detailRatingDisplay;
    private MyGuiControlButton m_button_RateUp;
    private MyGuiControlButton m_button_RateDown;
    private MyGuiControlImage m_icon_RateUp;
    private MyGuiControlImage m_icon_RateDown;
    private MyGuiControlLabel m_detailGridType;
    private MyGuiControlLabel m_detailSendTo;
    private MyGuiControlButton m_button_Refresh;
    private MyGuiControlButton m_button_GroupSelection;
    private MyGuiControlButton m_button_Sorting;
    private MyGuiControlButton m_button_OpenWorkshop;
    private MyGuiControlButton m_button_NewBlueprint;
    private MyGuiControlButton m_button_DirectorySelection;
    private MyGuiControlButton m_button_HideThumbnails;
    private MyGuiControlButton m_button_OpenInWorkshop;
    private MyGuiControlButton m_button_CopyToClipboard;
    private MyGuiControlButton m_button_Rename;
    private MyGuiControlButton m_button_Replace;
    private MyGuiControlButton m_button_Delete;
    private MyGuiControlButton m_button_TakeScreenshot;
    private MyGuiControlButton m_button_Publish;
    private MyGuiControlCombobox m_sendToCombo;
    private MyGuiControlImage m_icon_Refresh;
    private MyGuiControlImage m_icon_GroupSelection;
    private MyGuiControlImage m_icon_Sorting;
    private MyGuiControlImage m_icon_OpenWorkshop;
    private MyGuiControlImage m_icon_DirectorySelection;
    private MyGuiControlImage m_icon_NewBlueprint;
    private MyGuiControlImage m_icon_HideThumbnails;
    private MyGuiControlImage m_thumbnailImage;
    private MyGuiControlLabel m_workshopError;
    private static bool m_newScreenshotTaken;
    private bool m_multipleServices;
    private string m_mixedIcon;
    private int m_workshopIndex;
    private bool m_workshopPermitted;
    private float m_leftSideSizeX;

    private MyBlueprintItemInfo SelectedBlueprint
    {
      get => this.m_selectedBlueprint;
      set
      {
        if (this.m_selectedBlueprint == value)
          return;
        this.m_selectedBlueprint = value;
        this.SelectedBlueprintChanged();
      }
    }

    private void SelectedBlueprintChanged()
    {
      if (MyInput.Static.IsJoystickLastUsed)
        this.GamepadHelpText = string.Format(MyTexts.GetString(this.m_selectedBlueprint == null || this.m_selectedBlueprint.Type != MyBlueprintTypeEnum.LOCAL && this.m_selectedBlueprint.Type != MyBlueprintTypeEnum.CLOUD ? MySpaceTexts.BlueprintScreen_Help_Screen : MySpaceTexts.BlueprintScreen_Help_Screen_Local), this.m_selectedBlueprint?.Item == null ? (object) MyGameService.GetDefaultUGC().ServiceName : (object) this.m_selectedBlueprint.Item.ServiceName);
      this.m_wasJoystickLastUsed = MyInput.Static.IsJoystickLastUsed;
    }

    public List<MyWorkshopItem> GetSubscribedItemsList() => MyGuiBlueprintScreen_Reworked.GetSubscribedItemsList(this.m_content);

    public static List<MyWorkshopItem> GetSubscribedItemsList(Content content)
    {
      if (!MyGuiBlueprintScreen_Reworked.m_subscribedItemsListDict.ContainsKey(content))
        MyGuiBlueprintScreen_Reworked.m_subscribedItemsListDict.Add(content, new List<MyWorkshopItem>());
      return MyGuiBlueprintScreen_Reworked.m_subscribedItemsListDict[content];
    }

    public void SetSubscribeItemList(ref List<MyWorkshopItem> list) => MyGuiBlueprintScreen_Reworked.SetSubscribeItemList(ref list, this.m_content);

    public static void SetSubscribeItemList(ref List<MyWorkshopItem> list, Content content)
    {
      if (MyGuiBlueprintScreen_Reworked.m_subscribedItemsListDict.ContainsKey(content))
        MyGuiBlueprintScreen_Reworked.m_subscribedItemsListDict[content] = list;
      else
        MyGuiBlueprintScreen_Reworked.m_subscribedItemsListDict.Add(content, list);
    }

    public string GetCurrentLocalDirectory() => MyGuiBlueprintScreen_Reworked.GetCurrentLocalDirectory(this.m_content);

    public static string GetCurrentLocalDirectory(Content content)
    {
      if (!MyGuiBlueprintScreen_Reworked.m_currentLocalDirectoryDict.ContainsKey(content))
        MyGuiBlueprintScreen_Reworked.m_currentLocalDirectoryDict.Add(content, string.Empty);
      return MyGuiBlueprintScreen_Reworked.m_currentLocalDirectoryDict[content];
    }

    public void SetCurrentLocalDirectory(string path) => MyGuiBlueprintScreen_Reworked.SetCurrentLocalDirectory(this.m_content, path);

    public static void SetCurrentLocalDirectory(Content content, string path)
    {
      if (MyGuiBlueprintScreen_Reworked.m_currentLocalDirectoryDict.ContainsKey(content))
        MyGuiBlueprintScreen_Reworked.m_currentLocalDirectoryDict[content] = path;
      else
        MyGuiBlueprintScreen_Reworked.m_currentLocalDirectoryDict.Add(content, path);
    }

    private MyGuiBlueprintScreen_Reworked.SortOption GetSelectedSort() => this.GetSelectedSort(this.m_content);

    private MyGuiBlueprintScreen_Reworked.SortOption GetSelectedSort(
      Content content)
    {
      if (!MyGuiBlueprintScreen_Reworked.m_selectedSortDict.ContainsKey(content))
        MyGuiBlueprintScreen_Reworked.m_selectedSortDict.Add(content, MyGuiBlueprintScreen_Reworked.SortOption.None);
      return MyGuiBlueprintScreen_Reworked.m_selectedSortDict[content];
    }

    public MyBlueprintTypeEnum GetSelectedBlueprintType() => this.GetSelectedBlueprintType(this.m_content);

    public MyBlueprintTypeEnum GetSelectedBlueprintType(Content content)
    {
      if (!MyGuiBlueprintScreen_Reworked.m_selectedBlueprintTypeDict.ContainsKey(content))
        MyGuiBlueprintScreen_Reworked.m_selectedBlueprintTypeDict.Add(content, MyBlueprintTypeEnum.MIXED);
      return MyGuiBlueprintScreen_Reworked.m_selectedBlueprintTypeDict[content];
    }

    public bool GetThumbnailVisibility() => this.GetThumbnailVisibility(this.m_content);

    public bool GetThumbnailVisibility(Content content)
    {
      if (!MyGuiBlueprintScreen_Reworked.m_thumbnailsVisibleDict.ContainsKey(content))
        MyGuiBlueprintScreen_Reworked.m_thumbnailsVisibleDict.Add(content, true);
      return MyGuiBlueprintScreen_Reworked.m_thumbnailsVisibleDict[content];
    }

    private void SetSelectedSort(MyGuiBlueprintScreen_Reworked.SortOption option) => MyGuiBlueprintScreen_Reworked.SetSelectedSort(this.m_content, option);

    private static void SetSelectedSort(
      Content content,
      MyGuiBlueprintScreen_Reworked.SortOption option)
    {
      if (MyGuiBlueprintScreen_Reworked.m_selectedSortDict.ContainsKey(content))
        MyGuiBlueprintScreen_Reworked.m_selectedSortDict[content] = option;
      else
        MyGuiBlueprintScreen_Reworked.m_selectedSortDict.Add(content, option);
    }

    public void SetSelectedBlueprintType(MyBlueprintTypeEnum option) => MyGuiBlueprintScreen_Reworked.SetSelectedBlueprintType(this.m_content, option);

    public static void SetSelectedBlueprintType(Content content, MyBlueprintTypeEnum option)
    {
      if (MyGuiBlueprintScreen_Reworked.m_selectedBlueprintTypeDict.ContainsKey(content))
        MyGuiBlueprintScreen_Reworked.m_selectedBlueprintTypeDict[content] = option;
      else
        MyGuiBlueprintScreen_Reworked.m_selectedBlueprintTypeDict.Add(content, option);
    }

    public void SetThumbnailVisibility(bool option) => MyGuiBlueprintScreen_Reworked.SetThumbnailVisibility(this.m_content, option);

    public static void SetThumbnailVisibility(Content content, bool option)
    {
      if (MyGuiBlueprintScreen_Reworked.m_thumbnailsVisibleDict.ContainsKey(content))
        MyGuiBlueprintScreen_Reworked.m_thumbnailsVisibleDict[content] = option;
      else
        MyGuiBlueprintScreen_Reworked.m_thumbnailsVisibleDict.Add(content, option);
    }

    public static MyGuiBlueprintScreen_Reworked CreateBlueprintScreen(
      MyGridClipboard clipboard,
      bool allowCopyToClipboard,
      MyBlueprintAccessType accessType)
    {
      MyGuiBlueprintScreen_Reworked blueprintScreenReworked = new MyGuiBlueprintScreen_Reworked();
      blueprintScreenReworked.SetBlueprintInitData(clipboard, allowCopyToClipboard, accessType);
      blueprintScreenReworked.FinishInitialization();
      return blueprintScreenReworked;
    }

    public static MyGuiBlueprintScreen_Reworked CreateScriptScreen(
      Action<string> onScriptOpened,
      Func<string> getCodeFromEditor,
      Action onCloseAction)
    {
      MyGuiBlueprintScreen_Reworked blueprintScreenReworked = new MyGuiBlueprintScreen_Reworked();
      blueprintScreenReworked.SetScriptInitData(onScriptOpened, getCodeFromEditor, onCloseAction);
      blueprintScreenReworked.FinishInitialization();
      return blueprintScreenReworked;
    }

    private MyGuiBlueprintScreen_Reworked()
      : base(new Vector2(0.5f, 0.5f), new Vector2?(MyGuiBlueprintScreen_Reworked.SCREEN_SIZE), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity), false)
    {
      List<IMyUGCService> aggregates = MyGameService.WorkshopService.GetAggregates();
      if (aggregates.Count > 1)
      {
        this.m_multipleServices = true;
        this.m_mixedIcon = "BP_Mixed.png";
      }
      else
      {
        this.m_multipleServices = false;
        this.m_mixedIcon = "BP_" + aggregates[0].ServiceName + "_Mixed.png";
      }
      this.CanHideOthers = true;
      this.m_canShareInput = false;
      this.CanBeHidden = true;
      this.m_canCloseInCloseAllScreenCalls = true;
      this.m_isTopScreen = false;
      this.m_isTopMostScreen = false;
      this.m_margin_left = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      this.InitializeBPList();
      this.m_BPList.Clear();
      this.m_BPTypesGroup.Clear();
    }

    private void SetBlueprintInitData(
      MyGridClipboard clipboard,
      bool allowCopyToClipboard,
      MyBlueprintAccessType accessType)
    {
      this.m_content = Content.Blueprint;
      this.m_accessType = accessType;
      this.m_clipboard = clipboard;
      this.m_allowCopyToClipboard = allowCopyToClipboard;
      MyGuiBlueprintScreen_Reworked.CheckCurrentLocalDirectory_Blueprint();
      this.GetLocalNames_Blueprints(MyGuiBlueprintScreen_Reworked.m_downloadFromSteam);
      this.ApplyFiltering();
    }

    private void SetScriptInitData(
      Action<string> onScriptOpened,
      Func<string> getCodeFromEditor,
      Action onCloseAction)
    {
      this.m_content = Content.Script;
      this.m_onScriptOpened = onScriptOpened;
      this.m_getCodeFromEditor = getCodeFromEditor;
      this.m_onCloseAction = onCloseAction;
      MyGuiBlueprintScreen_Reworked.CheckCurrentLocalDirectory_Blueprint();
      using (MyGuiBlueprintScreen_Reworked.SubscribedItemsLock.AcquireSharedUsing())
      {
        this.GetLocalNames_Scripts(MyGuiBlueprintScreen_Reworked.m_downloadFromSteam);
        this.ApplyFiltering();
      }
    }

    private void FinishInitialization()
    {
      if (MyGuiBlueprintScreen_Reworked.m_downloadFromSteam)
        MyGuiBlueprintScreen_Reworked.m_downloadFromSteam = false;
      this.RecreateControls(true);
      this.TrySelectFirstBlueprint();
    }

    private void InitializeBPList()
    {
      this.m_BPTypesGroup = new MyGuiControlRadioButtonGroup();
      this.m_BPTypesGroup.SelectedChanged += new Action<MyGuiControlRadioButtonGroup>(this.OnSelectItem);
      this.m_BPTypesGroup.MouseDoubleClick += new Action<MyGuiControlRadioButton>(this.OnMouseDoubleClickItem);
      MyGuiControlList myGuiControlList = new MyGuiControlList();
      myGuiControlList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlList.Position = -this.m_size.Value / 2f + new Vector2(this.m_margin_left, 0.307f);
      myGuiControlList.Size = new Vector2(0.2f, (float) ((double) this.m_size.Value.Y - 0.307000011205673 - 0.068000003695488));
      this.m_BPList = myGuiControlList;
    }

    private void OnMouseDoubleClickItem(MyGuiControlRadioButton obj) => this.CopyToClipboard();

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      Vector2 vector2 = -this.m_size.Value / 2f + new Vector2(this.m_margin_left, MyGuiBlueprintScreen_Reworked.MARGIN_TOP);
      this.m_guiMultilineHeight = 0.29f;
      this.m_guiAdditionalInfoOffset = 0.111f;
      float num1 = this.m_size.Value.X - 2f * this.m_margin_left;
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText();
      controlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlMultilineText.Position = new Vector2(-0.5f * this.m_size.Value.X + this.m_margin_left, -0.345f);
      controlMultilineText.Size = new Vector2(num1, 0.05f);
      controlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlMultilineText.TextEnum = this.m_content == Content.Blueprint ? MyCommonTexts.BlueprintsScreen_Description : MyCommonTexts.ScriptsScreen_Description;
      controlMultilineText.Font = "Blue";
      this.Controls.Add((MyGuiControlBase) controlMultilineText);
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      float num2 = (float) ((double) this.m_size.Value.X - (double) this.m_margin_left - 0.275000005960464);
      float num3 = -0.2f;
      controlSeparatorList.AddHorizontal(new Vector2(vector2.X, -0.39f), num1);
      controlSeparatorList.AddHorizontal(new Vector2(vector2.X, -0.3f), num1);
      controlSeparatorList.AddHorizontal(new Vector2(num3, -0.225f), num2);
      controlSeparatorList.AddHorizontal(new Vector2(num3, 0.354f), num2);
      controlSeparatorList.AddHorizontal(new Vector2(num3, 0.212f), num2);
      controlSeparatorList.AddHorizontal(new Vector2(vector2.X, (float) ((double) this.m_size.Value.Y / 2.0 - 0.0500000007450581)), num1);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      MyStringId id = MySpaceTexts.ScreenBlueprintsRew_Caption;
      switch (this.m_content)
      {
        case Content.Blueprint:
          id = MySpaceTexts.ScreenBlueprintsRew_Caption_Blueprint;
          break;
        case Content.Script:
          id = MySpaceTexts.ScreenBlueprintsRew_Caption_Script;
          break;
      }
      this.AddCaption(MyTexts.GetString(id), new Vector4?(Color.White.ToVector4()), new Vector2?(new Vector2(0.0f, 0.02f)));
      this.m_detailName = this.AddCaption("Blueprint Name", new Vector4?(Color.White.ToVector4()), new Vector2?(new Vector2(0.1035f, 0.175f)));
      this.m_detailName.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_detailName.PositionX -= (float) ((double) num2 / 2.0 - 0.00700000021606684);
      MyGuiControlRating guiControlRating = new MyGuiControlRating(10);
      guiControlRating.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_detailRatingDisplay = guiControlRating;
      this.Controls.Add((MyGuiControlBase) this.m_detailRatingDisplay);
      this.m_button_RateUp = this.CreateRateButton(true);
      this.Controls.Add((MyGuiControlBase) this.m_button_RateUp);
      this.m_icon_RateUp = this.CreateRateIcon(this.m_button_RateUp, "Textures\\GUI\\Icons\\Blueprints\\like_test.png");
      this.Controls.Add((MyGuiControlBase) this.m_icon_RateUp);
      this.m_button_RateDown = this.CreateRateButton(false);
      this.Controls.Add((MyGuiControlBase) this.m_button_RateDown);
      this.m_icon_RateDown = this.CreateRateIcon(this.m_button_RateDown, "Textures\\GUI\\Icons\\Blueprints\\dislike_test.png");
      this.Controls.Add((MyGuiControlBase) this.m_icon_RateDown);
      this.m_multiline = new MyGuiControlMultilineText(textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, textPadding: new MyGuiBorderThickness?(new MyGuiBorderThickness(0.005f, 0.0f, 0.0f, 0.0f)));
      this.m_multiline.CanHaveFocus = true;
      this.m_multiline.VisualStyle = MyGuiControlMultilineStyleEnum.BackgroundBordered;
      this.Controls.Add((MyGuiControlBase) this.m_multiline);
      this.m_detailsBackground = new MyGuiControlPanel();
      this.m_detailsBackground.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK;
      this.Controls.Add((MyGuiControlBase) this.m_detailsBackground);
      this.m_searchBox = new MyGuiControlSearchBox(new Vector2?(new Vector2(-0.382f, -0.21f)), new Vector2?(new Vector2(this.m_BPList.Size.X, 0.032f)), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_searchBox.OnTextChanged += new MyGuiControlSearchBox.TextChangedDelegate(this.OnSearchTextChange);
      this.Controls.Add((MyGuiControlBase) this.m_searchBox);
      this.m_detailBlockCount = new MyGuiControlLabel(text: string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_NumOfBlocks), (object) string.Empty));
      this.Controls.Add((MyGuiControlBase) this.m_detailBlockCount);
      this.m_detailBlockCountValue = new MyGuiControlLabel(text: "0");
      this.Controls.Add((MyGuiControlBase) this.m_detailBlockCountValue);
      this.m_detailGridType = new MyGuiControlLabel(text: string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_GridType), (object) string.Empty));
      this.Controls.Add((MyGuiControlBase) this.m_detailGridType);
      this.m_detailGridTypeValue = new MyGuiControlLabel(text: "Unknown");
      this.Controls.Add((MyGuiControlBase) this.m_detailGridTypeValue);
      this.m_detailAuthor = new MyGuiControlLabel(text: string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_Author), (object) string.Empty));
      this.Controls.Add((MyGuiControlBase) this.m_detailAuthor);
      this.m_detailAuthorName = new MyGuiControlLabel(text: "N/A");
      this.Controls.Add((MyGuiControlBase) this.m_detailAuthorName);
      this.m_detailSize = new MyGuiControlLabel(text: string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_Size), (object) string.Empty));
      this.Controls.Add((MyGuiControlBase) this.m_detailSize);
      this.m_detailSizeValue = new MyGuiControlLabel(text: "Unknown");
      this.Controls.Add((MyGuiControlBase) this.m_detailSizeValue);
      this.m_detailDLC = new MyGuiControlLabel(text: string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_Dlc), (object) string.Empty));
      this.Controls.Add((MyGuiControlBase) this.m_detailDLC);
      this.m_detailSendTo = new MyGuiControlLabel(text: string.Format(MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_PCU), (object) string.Empty));
      this.Controls.Add((MyGuiControlBase) this.m_detailSendTo);
      if (MySandboxGame.Config.EnableSteamCloud && MyGameService.IsActive)
        this.Controls.Add((MyGuiControlBase) new MySCloudStorageQuotaBar(new Vector2(-0.109f, 0.395f)));
      this.UpdatePrefab((MyBlueprintItemInfo) null, false);
      this.UpdateInfo((XDocument) null, (MyBlueprintItemInfo) null);
      this.m_sendToCombo = this.AddCombo(size: new Vector2?(new Vector2(0.215f, 0.1f)));
      this.m_sendToCombo.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_SendToPlayer);
      this.m_sendToCombo.AddItem(0L, new StringBuilder("   "));
      foreach (MyNetworkClient client in Sync.Clients.GetClients())
      {
        if ((long) client.SteamUserId != (long) Sync.MyId)
          this.m_sendToCombo.AddItem(Convert.ToInt64(client.SteamUserId), new StringBuilder(client.DisplayName));
      }
      this.m_sendToCombo.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnSendToPlayer);
      this.CreateButtons();
      this.Controls.Add((MyGuiControlBase) this.m_BPList);
      switch (this.m_content)
      {
        case Content.Blueprint:
          this.m_detailSendTo.Visible = true;
          break;
        case Content.Script:
          this.m_sendToCombo.Visible = false;
          this.m_detailAuthor.Visible = false;
          this.m_detailAuthorName.Visible = false;
          this.m_detailBlockCount.Visible = false;
          this.m_detailBlockCountValue.Visible = false;
          this.m_detailGridType.Visible = false;
          this.m_detailGridTypeValue.Visible = false;
          this.m_detailSize.Visible = false;
          this.m_detailSizeValue.Visible = false;
          this.m_detailRatingDisplay.Visible = false;
          this.m_button_RateUp.Visible = false;
          this.m_icon_RateUp.Visible = false;
          this.m_button_RateDown.Visible = false;
          this.m_icon_RateDown.Visible = false;
          this.m_detailDLC.Visible = false;
          this.m_detailSendTo.Visible = false;
          break;
      }
      this.m_searchBox.Position = new Vector2((float) ((double) this.m_button_Refresh.Position.X - (double) this.m_button_Refresh.Size.X * 0.5 - 1.0 / 500.0), this.m_searchBox.Position.Y);
      this.m_searchBox.Size = new Vector2(this.m_leftSideSizeX, this.m_searchBox.Size.Y);
      this.m_BPList.Position = new Vector2(this.m_searchBox.Position.X, this.m_BPList.Position.Y);
      this.m_BPList.Size = new Vector2(this.m_searchBox.Size.X, this.m_BPList.Size.Y);
      this.m_workshopError = new MyGuiControlLabel(colorMask: new Vector4?((Vector4) Color.Red));
      this.m_workshopError.Visible = false;
      this.m_workshopError.VisibleChanged += (VisibleChangedDelegate) ((_, __) => this.UpdateHintsPositions());
      this.Controls.Add((MyGuiControlBase) this.m_workshopError);
      this.m_workshopPermitted = true;
      MyGameService.Service.RequestPermissions(Permissions.UGC, false, (Action<bool>) (granted =>
      {
        if (granted)
          return;
        this.SetWorkshopErrorText(MyTexts.GetString(MySpaceTexts.WorkshopRestricted));
        this.m_workshopPermitted = false;
      }));
      this.RefreshThumbnail();
      this.Controls.Add((MyGuiControlBase) this.m_thumbnailImage);
      this.RepositionDetailedPage(num3, num2);
      this.SetDetailPageTexts();
      this.UpdateDetailKeyEnable();
      this.m_gamepadHelpLabel = new MyGuiControlLabel();
      this.m_gamepadHelpLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.m_gamepadHelpLabel.VisibleChanged += (VisibleChangedDelegate) ((_, __) => this.UpdateHintsPositions());
      this.Controls.Add((MyGuiControlBase) this.m_gamepadHelpLabel);
      this.SelectedBlueprintChanged();
      this.FocusedControl = (MyGuiControlBase) this.m_searchBox.TextBox;
      float num4 = 0.03f;
      foreach (MyGuiControlBase control in this.Controls)
        control.PositionY -= num4;
      foreach (MyGuiControlBase element in this.Elements)
      {
        if (element != this.m_closeButton)
          element.PositionY -= num4;
      }
      this.UpdateHintsPositions();
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

    private void CreateButtons()
    {
      Vector2 vector2_1 = new Vector2(-0.402f, -0.27f);
      Vector2 vector2_2 = new Vector2(-0.0955f, -0.25f);
      Vector2 vector2_3 = new Vector2(-0.104f, 0.235f);
      Vector2 vector2_4 = new Vector2(-0.104f, 0.375f);
      Vector2 vector2_5 = new Vector2(0.144f, 0.035f);
      float num1 = 0.029f;
      float usableWidth = 0.188f;
      float textScale = 0.8f;
      int num2 = 0;
      this.m_button_Refresh = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, num1, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_Refresh), textScale: textScale);
      MyGuiControlButton buttonRefresh = this.m_button_Refresh;
      Vector2 vector2_6 = vector2_1;
      Vector2 vector2_7 = new Vector2(num1, 0.0f);
      int num3 = num2;
      int num4 = num3 + 1;
      double num5 = (double) num3;
      Vector2 vector2_8 = vector2_7 * (float) num5;
      Vector2 vector2_9 = vector2_6 + vector2_8;
      buttonRefresh.Position = vector2_9;
      this.m_button_Refresh.ShowTooltipWhenDisabled = true;
      this.m_button_GroupSelection = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, num1, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_GroupSelection), textScale: textScale);
      MyGuiControlButton buttonGroupSelection = this.m_button_GroupSelection;
      Vector2 vector2_10 = vector2_1;
      Vector2 vector2_11 = new Vector2(num1, 0.0f);
      int num6 = num4;
      int num7 = num6 + 1;
      double num8 = (double) num6;
      Vector2 vector2_12 = vector2_11 * (float) num8;
      Vector2 vector2_13 = vector2_10 + vector2_12;
      buttonGroupSelection.Position = vector2_13;
      this.m_button_GroupSelection.ShowTooltipWhenDisabled = true;
      this.m_button_Sorting = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, num1, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_Sorting), textScale: textScale);
      MyGuiControlButton buttonSorting = this.m_button_Sorting;
      Vector2 vector2_14 = vector2_1;
      Vector2 vector2_15 = new Vector2(num1, 0.0f);
      int num9 = num7;
      int num10 = num9 + 1;
      double num11 = (double) num9;
      Vector2 vector2_16 = vector2_15 * (float) num11;
      Vector2 vector2_17 = vector2_14 + vector2_16;
      buttonSorting.Position = vector2_17;
      this.m_button_Sorting.ShowTooltipWhenDisabled = true;
      this.m_button_NewBlueprint = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, num1, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_NewBlueprint), textScale: textScale);
      MyGuiControlButton buttonNewBlueprint = this.m_button_NewBlueprint;
      Vector2 vector2_18 = vector2_1;
      Vector2 vector2_19 = new Vector2(num1, 0.0f);
      int num12 = num10;
      int num13 = num12 + 1;
      double num14 = (double) num12;
      Vector2 vector2_20 = vector2_19 * (float) num14;
      Vector2 vector2_21 = vector2_18 + vector2_20;
      buttonNewBlueprint.Position = vector2_21;
      this.m_button_NewBlueprint.ShowTooltipWhenDisabled = true;
      if (MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE)
      {
        this.m_button_DirectorySelection = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, num1, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_DirectorySelection), textScale: textScale);
        this.m_button_DirectorySelection.Position = vector2_1 + new Vector2(num1, 0.0f) * (float) num13++;
        this.m_button_DirectorySelection.ShowTooltipWhenDisabled = true;
      }
      this.m_button_HideThumbnails = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, num1, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_HideThumbnails), textScale: textScale);
      MyGuiControlButton buttonHideThumbnails = this.m_button_HideThumbnails;
      Vector2 vector2_22 = vector2_1;
      Vector2 vector2_23 = new Vector2(num1, 0.0f);
      int num15 = num13;
      int num16 = num15 + 1;
      double num17 = (double) num15;
      Vector2 vector2_24 = vector2_23 * (float) num17;
      Vector2 vector2_25 = vector2_22 + vector2_24;
      buttonHideThumbnails.Position = vector2_25;
      this.m_button_HideThumbnails.ShowTooltipWhenDisabled = true;
      this.m_button_OpenWorkshop = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, num1, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_OpenWorkshop), textScale: textScale);
      MyGuiControlButton buttonOpenWorkshop = this.m_button_OpenWorkshop;
      Vector2 vector2_26 = vector2_1;
      Vector2 vector2_27 = new Vector2(num1, 0.0f);
      int num18 = num16;
      int num19 = num18 + 1;
      double num20 = (double) num18;
      Vector2 vector2_28 = vector2_27 * (float) num20;
      Vector2 vector2_29 = vector2_26 + vector2_28;
      buttonOpenWorkshop.Position = vector2_29;
      this.m_button_OpenWorkshop.ShowTooltipWhenDisabled = true;
      this.m_leftSideSizeX = this.m_button_OpenWorkshop.Position.X + this.m_button_OpenWorkshop.Size.X - this.m_button_Refresh.Position.X;
      if (!MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE)
        this.m_leftSideSizeX += num1;
      float num21 = 0.027f;
      this.m_button_Rename = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, usableWidth, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_Rename), textScale: textScale);
      this.m_button_Rename.Position = vector2_3 + new Vector2(usableWidth + num21, 0.0f) * 0.0f;
      this.m_button_Rename.Size = new Vector2(this.m_button_Rename.Size.X, this.m_button_Rename.Size.Y * 1.3f);
      this.m_button_Rename.ShowTooltipWhenDisabled = true;
      this.m_button_Replace = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, usableWidth, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_Replace), textScale: textScale);
      this.m_button_Replace.Position = vector2_3 + new Vector2(usableWidth + num21, 0.0f) * 1f;
      this.m_button_Replace.Size = new Vector2(this.m_button_Replace.Size.X, this.m_button_Replace.Size.Y * 1.3f);
      this.m_button_Replace.ShowTooltipWhenDisabled = true;
      this.m_button_Delete = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, usableWidth, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_Delete), textScale: textScale);
      this.m_button_Delete.Position = vector2_3 + new Vector2(usableWidth + num21, 0.0f) * 2f;
      this.m_button_Delete.Size = new Vector2(this.m_button_Delete.Size.X, this.m_button_Delete.Size.Y * 1.3f);
      this.m_button_Delete.ShowTooltipWhenDisabled = true;
      this.m_button_TakeScreenshot = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, usableWidth, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_TakeScreenshot), textScale: textScale);
      this.m_button_TakeScreenshot.Position = vector2_3 + new Vector2(usableWidth + num21, 0.0f) * 1f + new Vector2(0.0f, 0.055f);
      this.m_button_TakeScreenshot.Size = new Vector2(this.m_button_TakeScreenshot.Size.X, this.m_button_TakeScreenshot.Size.Y * 1.3f);
      this.m_button_TakeScreenshot.ShowTooltipWhenDisabled = true;
      this.m_button_Publish = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, usableWidth, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_Publish), textScale: textScale);
      this.m_button_Publish.Position = vector2_3 + new Vector2(usableWidth + num21, 0.0f) * 2f + new Vector2(0.0f, 0.055f);
      this.m_button_Publish.Size = new Vector2(this.m_button_Publish.Size.X, this.m_button_Publish.Size.Y * 1.3f);
      this.m_button_Publish.ShowTooltipWhenDisabled = true;
      this.m_button_OpenInWorkshop = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, usableWidth, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_OpenInWorkshop), textScale: textScale);
      this.m_button_OpenInWorkshop.Position = vector2_4 + new Vector2(usableWidth + num21, 0.0f) * 1f;
      this.m_button_OpenInWorkshop.Size = new Vector2(this.m_button_OpenInWorkshop.Size.X, this.m_button_OpenInWorkshop.Size.Y * 1.3f);
      this.m_button_OpenInWorkshop.ShowTooltipWhenDisabled = true;
      this.m_button_CopyToClipboard = MyBlueprintUtils.CreateButton((MyGuiScreenDebugBase) this, usableWidth, (StringBuilder) null, new Action<MyGuiControlButton>(this.OnButton_CopyToClipboard), textScale: textScale);
      this.m_button_CopyToClipboard.Position = vector2_4 + new Vector2(usableWidth + num21, 0.0f) * 2f;
      this.m_button_CopyToClipboard.Size = new Vector2(this.m_button_CopyToClipboard.Size.X, this.m_button_CopyToClipboard.Size.Y * 1.3f);
      this.m_button_CopyToClipboard.ShowTooltipWhenDisabled = true;
      this.CloseButtonEnabled = true;
      this.m_icon_Refresh = this.CreateButtonIcon(this.m_button_Refresh, "Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
      this.m_icon_GroupSelection = this.CreateButtonIcon(this.m_button_GroupSelection, "");
      this.SetIconForGroupSelection();
      this.m_icon_Sorting = this.CreateButtonIcon(this.m_button_Sorting, "");
      this.SetIconForSorting();
      this.m_icon_OpenWorkshop = this.CreateButtonIcon(this.m_button_OpenWorkshop, "Textures\\GUI\\Icons\\Browser\\WorkshopBrowser.dds");
      this.m_icon_DirectorySelection = this.CreateButtonIcon(this.m_button_NewBlueprint, "Textures\\GUI\\Icons\\Blueprints\\BP_New.png");
      this.m_icon_NewBlueprint = this.CreateButtonIcon(this.m_button_DirectorySelection, "Textures\\GUI\\Icons\\Blueprints\\FolderIcon.png");
      this.m_icon_HideThumbnails = this.CreateButtonIcon(this.m_button_HideThumbnails, "");
      this.SetIconForHideThubnails();
    }

    private MyGuiControlImage CreateButtonIcon(
      MyGuiControlButton butt,
      string texture)
    {
      if (butt == null)
        return (MyGuiControlImage) null;
      butt.Size = new Vector2(butt.Size.X, (float) ((double) butt.Size.X * 4.0 / 3.0));
      float y = 0.95f * Math.Min(butt.Size.X, butt.Size.Y);
      Vector2? size = new Vector2?(new Vector2(y * 0.75f, y));
      MyGuiControlImage icon = new MyGuiControlImage(new Vector2?(butt.Position + new Vector2(-1f / 625f, 0.018f)), size, textures: new string[1]
      {
        texture
      });
      this.Controls.Add((MyGuiControlBase) icon);
      butt.FocusChanged += (Action<MyGuiControlBase, bool>) ((x, focus) => icon.ColorMask = focus ? MyGuiConstants.HIGHLIGHT_TEXT_COLOR : Vector4.One);
      return icon;
    }

    private MyGuiControlButton CreateRateButton(bool positive)
    {
      Vector2? position = new Vector2?();
      Action<MyGuiControlButton> action = positive ? new Action<MyGuiControlButton>(this.OnRateUpClicked) : new Action<MyGuiControlButton>(this.OnRateDownClicked);
      Vector2? size = new Vector2?(new Vector2((float) (((double) this.m_detailRatingDisplay.GetWidth() - (double) this.ratingButtonsGap) / 2.0), 0.0342f));
      Vector4? colorMask = new Vector4?();
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Rectangular, size, colorMask, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
    }

    private MyGuiControlImage CreateRateIcon(
      MyGuiControlButton button,
      string texture)
    {
      MyGuiControlImage icon = new MyGuiControlImage(size: new Vector2?(new Vector2((float) ((double) button.Size.Y / 4.0 * 3.0), button.Size.Y) * 0.6f), textures: new string[1]
      {
        texture
      });
      button.HighlightChanged += (Action<MyGuiControlBase>) (x => icon.ColorMask = x.HasHighlight ? MyGuiConstants.HIGHLIGHT_TEXT_COLOR : Vector4.One);
      return icon;
    }

    private void RepositionDetailedPage(float posX, float widthFromRight)
    {
      Vector2 vector2_1 = new Vector2(-0.168f, this.m_guiAdditionalInfoOffset);
      Vector2 vector2_2 = new Vector2(posX, -0.2655f);
      Vector2 vector2_3 = new Vector2(widthFromRight, this.m_guiMultilineHeight);
      Vector2 zero = Vector2.Zero;
      this.m_button_Rename.Visible = true;
      this.m_button_Replace.Visible = true;
      this.m_button_Delete.Visible = true;
      this.m_button_TakeScreenshot.Visible = this.m_content == Content.Blueprint;
      this.m_button_Publish.Visible = true;
      Vector2 vector2_4 = new Vector2(0.394f, 0.0f) + new Vector2(-0.024f, 0.04f);
      this.m_multiline.Position = vector2_2 + 0.5f * vector2_3 + new Vector2(0.0f, 0.05f);
      this.m_multiline.Size = vector2_3;
      this.m_detailRatingDisplay.Position = new Vector2(posX + widthFromRight - this.m_detailRatingDisplay.GetWidth(), -0.2825f);
      this.m_button_RateUp.Position = this.m_detailRatingDisplay.Position + new Vector2((float) (((double) this.m_detailRatingDisplay.GetWidth() - (double) this.ratingButtonsGap) / 2.0), 0.034f);
      this.m_icon_RateUp.Position = this.m_button_RateUp.Position + new Vector2(-0.0015f, -1f / 400f) - new Vector2(this.m_button_RateUp.Size.X / 2f, 0.0f);
      this.m_button_RateDown.Position = this.m_detailRatingDisplay.Position + new Vector2(this.m_detailRatingDisplay.GetWidth(), 0.034f);
      this.m_icon_RateDown.Position = this.m_button_RateDown.Position + new Vector2(-0.0015f, -1f / 400f) - new Vector2(this.m_button_RateDown.Size.X / 2f, 0.0f);
      float x1 = (float) ((double) this.m_detailBlockCount.Position.X + (double) Math.Max(Math.Max(this.m_detailBlockCount.Size.X, this.m_detailGridType.Size.X), this.m_detailAuthor.Size.X) + 1.0 / 1000.0);
      this.m_detailAuthor.Position = vector2_1 + new Vector2(0.0f, 0.0f);
      this.m_detailBlockCount.Position = vector2_1 + new Vector2(0.0f, 0.03f);
      this.m_detailGridType.Position = vector2_1 + new Vector2(0.0f, 0.06f);
      this.m_detailAuthorName.Position = new Vector2(x1, this.m_detailAuthor.Position.Y);
      this.m_detailBlockCountValue.Position = new Vector2(x1, this.m_detailBlockCount.Position.Y);
      this.m_detailGridTypeValue.Position = new Vector2(x1, this.m_detailGridType.Position.Y);
      float x2 = 0.27f;
      float x3 = (vector2_1 + vector2_4).X;
      this.m_detailDLC.Position = vector2_1 + new Vector2(x2, 0.0f);
      this.m_detailSize.Position = vector2_1 + new Vector2(x2, 0.03f);
      this.m_detailSizeValue.Position = new Vector2(x3, this.m_detailSize.Position.Y);
      this.m_detailSendTo.Position = vector2_1 + new Vector2(x2, 0.06f);
      this.m_sendToCombo.Position = vector2_1 + vector2_4;
      Vector2 vector2_5 = this.m_sendToCombo.Position - vector2_1;
      this.m_detailsBackground.Position = this.m_multiline.Position + new Vector2(0.0f, (float) ((double) this.m_multiline.Size.Y / 2.0 + 0.0715000033378601));
      this.m_detailsBackground.Size = new Vector2(this.m_multiline.Size.X, (float) ((double) vector2_5.Y + (double) this.m_sendToCombo.Size.Y + 0.0199999995529652));
      foreach (MyGuiControlImage dlcIcon in this.m_dlcIcons)
      {
        Vector2 position = dlcIcon.Position;
        position.Y = vector2_1.Y;
        dlcIcon.Position = position;
      }
    }

    private void UpdateHintsPositions()
    {
      float x = this.m_BPList.Position.X;
      if (this.m_gamepadHelpLabel.Visible && this.m_workshopError.Visible)
      {
        Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
        this.m_gamepadHelpLabel.Position = new Vector2(x, this.m_button_CopyToClipboard.Position.Y + minSizeGui.Y * 1.66f);
        this.m_workshopError.Position = new Vector2(x, 0.46f);
      }
      else
      {
        Vector2 vector2 = new Vector2(x, 0.44f);
        this.m_workshopError.Position = vector2;
        this.m_gamepadHelpLabel.Position = vector2;
      }
    }

    private void SetDetailPageTexts()
    {
      this.m_button_Refresh.Text = (string) null;
      this.m_button_Refresh.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButRefresh);
      this.m_button_GroupSelection.Text = (string) null;
      List<IMyUGCService> aggregates = MyGameService.WorkshopService.GetAggregates();
      string serviceName = aggregates[0].ServiceName;
      string str = "";
      MyStringId id;
      if (this.m_multipleServices)
      {
        str = aggregates[1].ServiceName;
        id = MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE ? MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButGrouping_Aggregator : MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButGrouping_NoLocal_Aggregator;
      }
      else
        id = MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE ? MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButGrouping : MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButGrouping_NoLocal;
      this.m_button_GroupSelection.SetToolTip(string.Format(MyTexts.GetString(id), (object) serviceName, (object) str));
      this.m_button_Sorting.Text = (string) null;
      this.m_button_Sorting.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButSort);
      this.m_button_OpenWorkshop.Text = (string) null;
      this.m_button_OpenWorkshop.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButOpenWorkshop);
      if (this.m_button_DirectorySelection != null)
      {
        this.m_button_DirectorySelection.Text = (string) null;
        this.m_button_DirectorySelection.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButFolders);
      }
      this.m_button_HideThumbnails.Text = (string) null;
      this.m_button_HideThumbnails.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButVisibility);
      this.m_button_Rename.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButRename);
      this.m_button_Rename.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButRename);
      this.m_button_Replace.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButReplace);
      this.m_button_Replace.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButReplace);
      this.m_button_Delete.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButDelete);
      this.m_button_Delete.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButDelete);
      this.m_button_TakeScreenshot.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButScreenshot);
      this.m_button_TakeScreenshot.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButScreenshot);
      this.m_button_Publish.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButPublish);
      this.m_button_Publish.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButPublish);
      this.m_button_OpenInWorkshop.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButOpenInWorkshop);
      this.m_button_OpenInWorkshop.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButOpenInWorkshop);
      switch (this.m_content)
      {
        case Content.Blueprint:
          this.m_button_NewBlueprint.Text = (string) null;
          this.m_button_NewBlueprint.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButNewBlueprint);
          this.m_button_CopyToClipboard.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButToClipboard);
          this.m_button_CopyToClipboard.IsAutoScaleEnabled = true;
          this.m_button_CopyToClipboard.IsAutoEllipsisEnabled = true;
          this.m_button_CopyToClipboard.IsAutoScaleEnabled = true;
          this.m_button_CopyToClipboard.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButToClipboard);
          break;
        case Content.Script:
          this.m_button_NewBlueprint.Text = (string) null;
          this.m_button_NewBlueprint.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButNewScript);
          this.m_button_CopyToClipboard.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_ButToEditor);
          this.m_button_CopyToClipboard.SetToolTip(MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButToEditor);
          break;
      }
    }

    private void SetIconForGroupSelection()
    {
      switch (this.GetSelectedBlueprintType())
      {
        case MyBlueprintTypeEnum.WORKSHOP:
          this.m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_" + MyGameService.WorkshopService.GetAggregates()[this.m_workshopIndex].ServiceName + ".png");
          break;
        case MyBlueprintTypeEnum.LOCAL:
          this.m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_Local.png");
          break;
        case MyBlueprintTypeEnum.CLOUD:
          this.m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_Cloud.png");
          break;
        default:
          this.m_icon_GroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\" + this.m_mixedIcon);
          break;
      }
    }

    private void SetIconForSorting()
    {
      switch (this.GetSelectedSort())
      {
        case MyGuiBlueprintScreen_Reworked.SortOption.None:
          this.m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\NoSorting.png");
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
          this.m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\Alphabetical.png");
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
          this.m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ByCreationDate.png");
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
          this.m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ByUpdateDate.png");
          break;
        default:
          this.m_icon_Sorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\NoSorting.png");
          break;
      }
    }

    private void SetIconForHideThubnails()
    {
      if (this.GetThumbnailVisibility())
        this.m_icon_HideThumbnails.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ThumbnailsON.png");
      else
        this.m_icon_HideThumbnails.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ThumbnailsOFF.png");
    }

    private void UpdateInfo(XDocument doc, MyBlueprintItemInfo data)
    {
      int num = 0;
      string str1 = string.Empty;
      string str2 = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_NotAvailable);
      string formattedFileSizeInMb = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_NotAvailable);
      MyBlueprintItemInfo selectedBlueprint = this.SelectedBlueprint;
      MyGuiControlContentButton selectedButton = this.m_selectedButton;
      if (data != null && this.SelectedBlueprint != null && data.Equals((object) this.SelectedBlueprint))
      {
        formattedFileSizeInMb = MyValueFormatter.GetFormattedFileSizeInMB(data.Size);
        switch (this.m_content)
        {
          case Content.Blueprint:
            if (doc != null)
            {
              try
              {
                IEnumerable<XElement> source1 = doc.Descendants((XName) "GridSizeEnum");
                IEnumerable<XElement> source2 = doc.Descendants((XName) "DisplayName");
                IEnumerable<XElement> source3 = doc.Descendants((XName) "CubeBlocks");
                IEnumerable<XElement> xelements = doc.Descendants((XName) "DLC");
                str2 = source1 == null || source1.Count<XElement>() <= 0 ? "N/A" : (string) source1.First<XElement>();
                str1 = source2 == null || source2.Count<XElement>() <= 0 ? "N/A" : (string) source2.First<XElement>();
                num = 0;
                if (source3 != null && source3.Count<XElement>() > 0)
                {
                  foreach (XElement xelement in source3)
                    num += xelement.Elements().Count<XElement>();
                }
                if (xelements != null)
                {
                  HashSet<uint> source4 = new HashSet<uint>();
                  foreach (XElement xelement in xelements)
                  {
                    MyDLCs.MyDLC dlc;
                    if (!string.IsNullOrEmpty(xelement.Value) && MyDLCs.TryGetDLC(xelement.Value, out dlc))
                      source4.Add(dlc.AppId);
                  }
                  if (source4.Count > 0)
                  {
                    selectedBlueprint.Data.DLCs = source4.ToArray<uint>();
                    break;
                  }
                  break;
                }
                break;
              }
              catch
              {
                break;
              }
            }
            else
              break;
          case Content.Script:
            str1 = data.Item != null ? data.Item.OwnerId.ToString() : "N/A";
            break;
        }
      }
      if (selectedBlueprint == this.SelectedBlueprint && selectedButton == this.m_selectedButton)
      {
        this.m_detailDLC.Visible = false;
        this.m_detailRatingDisplay.Visible = false;
        this.m_button_RateUp.Visible = false;
        this.m_icon_RateUp.Visible = false;
        this.m_button_RateDown.Visible = false;
        this.m_icon_RateDown.Visible = false;
        foreach (MyGuiControlBase dlcIcon in this.m_dlcIcons)
          this.Controls.Remove(dlcIcon);
        this.m_dlcIcons.Clear();
        if (selectedBlueprint != null)
        {
          if (!selectedBlueprint.Data.DLCs.IsNullOrEmpty<uint>())
          {
            this.m_detailDLC.Visible = true;
            Vector2 vector2 = this.m_detailDLC.Position + new Vector2(MyGuiManager.MeasureString(this.m_detailDLC.Font, this.m_detailDLC.TextToDraw, this.m_detailDLC.TextScale).X, 0.0f);
            foreach (uint dlC in selectedBlueprint.Data.DLCs)
            {
              MyDLCs.MyDLC dlc;
              if (MyDLCs.TryGetDLC(dlC, out dlc))
              {
                MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage(textures: new string[1]
                {
                  dlc.Icon
                }, toolTip: MyDLCs.GetRequiredDLCTooltip(dlC));
                myGuiControlImage1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
                myGuiControlImage1.Size = new Vector2(32f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
                MyGuiControlImage myGuiControlImage2 = myGuiControlImage1;
                myGuiControlImage2.Position = vector2;
                vector2.X += myGuiControlImage2.Size.X + 1f / 500f;
                this.m_dlcIcons.Add(myGuiControlImage2);
                this.Controls.Add((MyGuiControlBase) myGuiControlImage2);
              }
            }
          }
          if (selectedBlueprint.Item != null)
          {
            this.m_detailRatingDisplay.Visible = true;
            this.m_button_RateUp.Visible = true;
            this.m_icon_RateUp.Visible = true;
            this.m_button_RateDown.Visible = true;
            this.m_icon_RateDown.Visible = true;
            this.m_detailRatingDisplay.Value = (int) Math.Round((double) selectedBlueprint.Item.Score * 10.0);
            selectedBlueprint.Item.UpdateRating();
            int myRating = selectedBlueprint.Item.MyRating;
            this.m_button_RateUp.Checked = myRating == 1;
            this.m_button_RateDown.Checked = myRating == -1;
          }
        }
        this.m_detailBlockCountValue.Text = num.ToString();
        this.m_detailGridTypeValue.Text = str2;
        this.m_detailAuthorName.Text = str1;
        this.m_detailSizeValue.Text = formattedFileSizeInMb;
        this.m_detailSendTo.Text = MyTexts.GetString(MySpaceTexts.BlueprintInfo_SendTo);
        float x = (float) ((double) this.m_detailBlockCount.Position.X + (double) Math.Max(Math.Max(this.m_detailBlockCount.Size.X, this.m_detailGridType.Size.X), this.m_detailAuthor.Size.X) + 1.0 / 1000.0);
        this.m_detailBlockCountValue.Position = new Vector2(x, this.m_detailBlockCount.Position.Y);
        this.m_detailGridTypeValue.Position = new Vector2(x, this.m_detailGridType.Position.Y);
        this.m_detailAuthorName.Position = new Vector2(x, this.m_detailAuthor.Position.Y);
      }
      if (this.m_loadedPrefab == null)
        return;
      this.UpdateDetailKeyEnable();
    }

    private XDocument LoadXDocument(Stream sbcStream)
    {
      try
      {
        return XDocument.Load(sbcStream);
      }
      catch
      {
        return (XDocument) null;
      }
    }

    public void UpdateDetailKeyEnable()
    {
      if (this.SelectedBlueprint == null)
      {
        this.m_button_OpenInWorkshop.Enabled = false;
        this.m_button_CopyToClipboard.Enabled = false;
        this.m_button_Rename.Enabled = false;
        this.m_button_Replace.Enabled = false;
        this.m_button_Delete.Enabled = false;
        this.m_button_TakeScreenshot.Enabled = false;
        this.m_button_Publish.Enabled = false;
        this.m_sendToCombo.Enabled = false;
      }
      else
      {
        switch (this.SelectedBlueprint.Type)
        {
          case MyBlueprintTypeEnum.WORKSHOP:
            this.m_button_OpenInWorkshop.Enabled = true;
            this.m_button_CopyToClipboard.Enabled = true;
            this.m_button_Rename.Enabled = false;
            this.m_button_Replace.Enabled = false;
            this.m_button_Delete.Enabled = false;
            this.m_button_TakeScreenshot.Enabled = false;
            this.m_button_Publish.Enabled = false;
            this.m_sendToCombo.Enabled = true;
            break;
          case MyBlueprintTypeEnum.LOCAL:
            this.m_button_OpenInWorkshop.Enabled = false;
            this.m_button_CopyToClipboard.Enabled = true;
            this.m_button_Rename.Enabled = true;
            this.m_button_Replace.Enabled = true;
            this.m_button_Delete.Enabled = true;
            this.m_button_TakeScreenshot.Enabled = true;
            this.m_button_Publish.Enabled = this.m_workshopPermitted && MyFakes.ENABLE_WORKSHOP_PUBLISH;
            this.m_sendToCombo.Enabled = false;
            break;
          case MyBlueprintTypeEnum.SHARED:
            this.m_button_OpenInWorkshop.Enabled = false;
            this.m_button_CopyToClipboard.Enabled = true;
            this.m_button_Rename.Enabled = false;
            this.m_button_Replace.Enabled = false;
            this.m_button_Delete.Enabled = false;
            this.m_button_TakeScreenshot.Enabled = false;
            this.m_button_Publish.Enabled = false;
            this.m_sendToCombo.Enabled = false;
            break;
          case MyBlueprintTypeEnum.DEFAULT:
            this.m_button_OpenInWorkshop.Enabled = false;
            this.m_button_CopyToClipboard.Enabled = true;
            this.m_button_Rename.Enabled = false;
            this.m_button_Replace.Enabled = false;
            this.m_button_Delete.Enabled = false;
            this.m_button_TakeScreenshot.Enabled = false;
            this.m_button_Publish.Enabled = false;
            this.m_sendToCombo.Enabled = false;
            break;
          case MyBlueprintTypeEnum.CLOUD:
            this.m_button_OpenInWorkshop.Enabled = false;
            this.m_button_CopyToClipboard.Enabled = true;
            this.m_button_Rename.Enabled = true;
            this.m_button_Replace.Enabled = true;
            this.m_button_Delete.Enabled = true;
            this.m_button_TakeScreenshot.Enabled = true;
            this.m_button_Publish.Enabled = this.m_workshopPermitted && MyFakes.ENABLE_WORKSHOP_PUBLISH;
            this.m_sendToCombo.Enabled = false;
            break;
          default:
            this.m_button_OpenInWorkshop.Enabled = false;
            this.m_button_CopyToClipboard.Enabled = false;
            this.m_button_Rename.Enabled = false;
            this.m_button_Replace.Enabled = false;
            this.m_button_Delete.Enabled = false;
            this.m_button_TakeScreenshot.Enabled = false;
            this.m_button_Publish.Enabled = false;
            this.m_sendToCombo.Enabled = false;
            break;
        }
      }
    }

    private void TogglePreviewVisibility()
    {
      foreach (MyGuiControlBase control in this.m_BPList.Controls)
      {
        if (control is MyGuiControlContentButton controlContentButton)
          controlContentButton.SetPreviewVisibility(this.GetThumbnailVisibility());
      }
      this.m_BPList.Recalculate();
    }

    private void AddBlueprintButton(MyBlueprintItemInfo data, bool filter = false)
    {
      string blueprintName = data.BlueprintName;
      string imagePath = this.GetImagePath(data);
      if (File.Exists(imagePath))
        MyRenderProxy.PreloadTextures((IEnumerable<string>) new string[1]
        {
          imagePath
        }, TextureType.GUIWithoutPremultiplyAlpha);
      MyGuiControlContentButton controlContentButton1 = new MyGuiControlContentButton(blueprintName, imagePath);
      controlContentButton1.UserData = (object) data;
      controlContentButton1.Key = this.m_BPTypesGroup.Count;
      MyGuiControlContentButton controlContentButton2 = controlContentButton1;
      controlContentButton2.SetModType(data.Type, data.Item?.ServiceName);
      controlContentButton2.MouseOverChanged += new Action<MyGuiControlRadioButton, bool>(this.OnMouseOverItem);
      controlContentButton2.FocusChanged += new Action<MyGuiControlBase, bool>(this.OnFocusedItem);
      controlContentButton2.SetTooltip(blueprintName);
      controlContentButton2.SetPreviewVisibility(this.GetThumbnailVisibility());
      this.m_BPTypesGroup.Add((MyGuiControlRadioButton) controlContentButton2);
      this.m_BPList.Controls.Add((MyGuiControlBase) controlContentButton2);
      if (!filter)
        return;
      this.ApplyFiltering(controlContentButton2);
    }

    public override void OnScreenOrderChanged(MyGuiScreenBase oldLast, MyGuiScreenBase newLast)
    {
      base.OnScreenOrderChanged(oldLast, newLast);
      this.CheckUGCServices();
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
        this.SetWorkshopErrorText(str + MyTexts.GetString(MySpaceTexts.UGC_ServiceNotAvailable_NoConsent), skipUGCCheck: true);
      else
        this.SetWorkshopErrorText(visible: false, skipUGCCheck: true);
    }

    private void AddBlueprintButtons(ref List<MyBlueprintItemInfo> data, bool filter = false)
    {
      int val1 = MyVRage.Platform.System.IsMemoryLimited ? 10 : int.MaxValue;
      List<string> stringList1 = new List<string>(Math.Min(val1, data.Count));
      List<string> stringList2 = new List<string>();
      for (int index = 0; index < data.Count; ++index)
      {
        string imagePath = this.GetImagePath(data[index]);
        stringList2.Add(imagePath);
        if (stringList1.Count < val1 && File.Exists(imagePath))
          stringList1.Add(imagePath);
      }
      MyRenderProxy.PreloadTextures((IEnumerable<string>) stringList1, TextureType.GUIWithoutPremultiplyAlpha);
      for (int index = 0; index < data.Count; ++index)
      {
        string name = data[index].Data.Name;
        MyGuiControlContentButton controlContentButton1 = new MyGuiControlContentButton(name, File.Exists(stringList2[index]) ? stringList2[index] : "");
        controlContentButton1.UserData = (object) data[index];
        controlContentButton1.Key = this.m_BPTypesGroup.Count;
        MyGuiControlContentButton controlContentButton2 = controlContentButton1;
        controlContentButton2.SetModType(data[index].Type, data[index].Item?.ServiceName);
        if (MyGuiBlueprintScreen_Reworked.m_showDlcIcons)
        {
          if (data[index].Item != null)
          {
            ListReader<uint> dlCs = data[index].Item.DLCs;
            if (dlCs.Count > 0)
            {
              dlCs = data[index].Item.DLCs;
              using (List<uint>.Enumerator enumerator = dlCs.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  string dlcIcon = MyDLCs.GetDLCIcon(enumerator.Current);
                  if (!string.IsNullOrEmpty(dlcIcon))
                    controlContentButton2.AddDlcIcon(dlcIcon);
                }
                goto label_22;
              }
            }
          }
          if (data[index].Data.DLCs != null && data[index].Data.DLCs.Length != 0)
          {
            foreach (uint dlC in data[index].Data.DLCs)
            {
              string dlcIcon = MyDLCs.GetDLCIcon(dlC);
              if (!string.IsNullOrEmpty(dlcIcon))
                controlContentButton2.AddDlcIcon(dlcIcon);
            }
          }
        }
label_22:
        controlContentButton2.MouseOverChanged += new Action<MyGuiControlRadioButton, bool>(this.OnMouseOverItem);
        controlContentButton2.FocusChanged += new Action<MyGuiControlBase, bool>(this.OnFocusedItem);
        controlContentButton2.SetTooltip(name);
        controlContentButton2.SetPreviewVisibility(this.GetThumbnailVisibility());
        this.m_BPTypesGroup.Add((MyGuiControlRadioButton) controlContentButton2);
        this.m_BPList.Controls.Add((MyGuiControlBase) controlContentButton2);
        if (filter)
          this.ApplyFiltering(controlContentButton2);
      }
    }

    private void TrySelectFirstBlueprint()
    {
      if (this.m_BPTypesGroup.Count > 0 && (this.m_BPTypesGroup.SelectedIndex.HasValue && this.m_BPTypesGroup.SelectedButton.Visible || this.m_BPTypesGroup.TrySelectFirstVisible()))
        return;
      this.m_multiline.Clear();
      this.m_detailName.Text = MyTexts.GetString(MySpaceTexts.ScreenBlueprintsRew_NotAvailable);
      switch (this.GetSelectedBlueprintType())
      {
        case MyBlueprintTypeEnum.WORKSHOP:
        case MyBlueprintTypeEnum.MIXED:
          this.m_multiline.AppendText(MyTexts.GetString(this.m_content == Content.Blueprint ? MySpaceTexts.ScreenBlueprintsRew_NoWorkshopBlueprints : MySpaceTexts.ScreenBlueprintsRew_NoWorkshopScripts), "Blue", this.m_multiline.TextScale, Vector4.One);
          this.m_multiline.OnLinkClicked += new LinkClicked(this.OnLinkClicked);
          break;
        default:
          this.m_multiline.AppendText(MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_NoBlueprints), "Blue", this.m_multiline.TextScale, Vector4.One);
          break;
      }
      this.m_multiline.ScrollbarOffsetV = 1f;
      this.UpdateInfo((XDocument) null, (MyBlueprintItemInfo) null);
    }

    private void OnLinkClicked(MyGuiControlBase sender, string url) => MyGuiSandbox.OpenUrlWithFallback(url, "Space Engineers Workshop");

    private void OnButton_Refresh(MyGuiControlButton button)
    {
      bool flag = false;
      MyBlueprintItemInfo itemInfo = (MyBlueprintItemInfo) null;
      if (this.SelectedBlueprint != null)
      {
        flag = true;
        itemInfo = this.SelectedBlueprint;
      }
      this.m_selectedButton = (MyGuiControlContentButton) null;
      this.SelectedBlueprint = (MyBlueprintItemInfo) null;
      this.UpdateDetailKeyEnable();
      MyGuiBlueprintScreen_Reworked.m_downloadFinished.Clear();
      MyGuiBlueprintScreen_Reworked.m_downloadQueued.Clear();
      this.RefreshAndReloadItemList();
      this.TrySelectFirstBlueprint();
      if (flag)
        this.SelectBlueprint(itemInfo);
      this.UpdateDetailKeyEnable();
    }

    private void OnButton_GroupSelection(MyGuiControlButton button)
    {
      MyBlueprintTypeEnum option = MyBlueprintTypeEnum.MIXED;
      switch (this.GetSelectedBlueprintType())
      {
        case MyBlueprintTypeEnum.WORKSHOP:
          if (this.m_multipleServices && this.m_workshopIndex == 0)
          {
            option = MyBlueprintTypeEnum.WORKSHOP;
            ++this.m_workshopIndex;
            break;
          }
          option = MyBlueprintTypeEnum.CLOUD;
          this.m_workshopIndex = 0;
          break;
        case MyBlueprintTypeEnum.LOCAL:
          option = MyBlueprintTypeEnum.WORKSHOP;
          this.m_workshopIndex = 0;
          break;
        case MyBlueprintTypeEnum.CLOUD:
          option = MyBlueprintTypeEnum.MIXED;
          break;
        case MyBlueprintTypeEnum.MIXED:
          option = MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE ? MyBlueprintTypeEnum.LOCAL : MyBlueprintTypeEnum.WORKSHOP;
          this.m_workshopIndex = 0;
          break;
      }
      this.SetGroupSelection(option);
    }

    private void OnButton_Sorting(MyGuiControlButton button)
    {
      switch (this.GetSelectedSort())
      {
        case MyGuiBlueprintScreen_Reworked.SortOption.None:
          this.SetSelectedSort(MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical);
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
          this.SetSelectedSort(MyGuiBlueprintScreen_Reworked.SortOption.CreationDate);
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
          this.SetSelectedSort(MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate);
          break;
        case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
          this.SetSelectedSort(MyGuiBlueprintScreen_Reworked.SortOption.None);
          break;
      }
      this.SetIconForSorting();
      this.OnReload((MyGuiControlButton) null);
    }

    private void OnButton_OpenWorkshop(MyGuiControlButton button) => MyWorkshop.OpenWorkshopBrowser(this.m_content == Content.Blueprint ? MySteamConstants.TAG_BLUEPRINTS : MySteamConstants.TAG_SCRIPTS);

    public override bool UnhideScreen()
    {
      if (MyGuiBlueprintScreen_Reworked.Task.IsComplete)
        this.GetWorkshopItems();
      return base.UnhideScreen();
    }

    private void OnButton_NewBlueprint(MyGuiControlButton button)
    {
      switch (this.m_content)
      {
        case Content.Blueprint:
          this.CreateBlueprintFromClipboard(true);
          break;
        case Content.Script:
          this.CreateScriptFromEditor();
          break;
      }
    }

    private void OnButton_DirectorySelection(MyGuiControlButton button)
    {
      string rootPath = string.Empty;
      Func<string, bool> isItem = (Func<string, bool>) null;
      switch (this.m_content)
      {
        case Content.Blueprint:
          rootPath = MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL;
          isItem = new Func<string, bool>(MyBlueprintUtils.IsItem_Blueprint);
          break;
        case Content.Script:
          rootPath = MyBlueprintUtils.SCRIPT_FOLDER_LOCAL;
          isItem = new Func<string, bool>(MyBlueprintUtils.IsItem_Script);
          break;
      }
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiFolderScreen(false, new Action<bool, string>(this.OnPathSelected), rootPath, this.GetCurrentLocalDirectory(), isItem, true));
    }

    private void OnButton_HideThumbnails(MyGuiControlButton button)
    {
      this.SetThumbnailVisibility(!this.GetThumbnailVisibility());
      this.SetIconForHideThubnails();
      this.TogglePreviewVisibility();
    }

    private void OnButton_OpenInWorkshop(MyGuiControlButton button)
    {
      if (this.SelectedBlueprint?.Item != null)
        MyGuiSandbox.OpenUrlWithFallback(this.SelectedBlueprint.Item.GetItemUrl(), this.SelectedBlueprint.Item.ServiceName + " Workshop");
      else
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(""), messageCaption: new StringBuilder("Invalid workshop id")));
    }

    private void OnButton_CopyToClipboard(MyGuiControlButton button) => this.CopyToClipboard();

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (!this.m_wasJoystickLastUsed && MyInput.Static.IsJoystickLastUsed)
        this.SelectedBlueprintChanged();
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        this.OnButton_Refresh((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OnButton_NewBlueprint((MyGuiControlButton) null);
      if (this.SelectedBlueprint?.Item != null && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.VIEW))
        this.OnButton_OpenInWorkshop((MyGuiControlButton) null);
      if (MyControllerHelper.GetControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU).IsNewReleased())
        this.OnButton_OpenWorkshop((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
        this.OnButton_Sorting((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
        this.OnButton_GroupSelection((MyGuiControlButton) null);
      if (this.FocusedControl != null && this.FocusedControl.Owner == this.m_BPList && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT))
        this.CopyToClipboard();
      this.m_button_CopyToClipboard.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_button_OpenInWorkshop.Visible = !MyInput.Static.IsJoystickLastUsed;
    }

    private void CopyToClipboard()
    {
      if (this.SelectedBlueprint == null)
        return;
      switch (this.m_content)
      {
        case Content.Blueprint:
          if (this.m_blueprintBeingLoaded)
            break;
          if (this.SelectedBlueprint.IsDirectory)
          {
            if (string.IsNullOrEmpty(this.SelectedBlueprint.BlueprintName))
            {
              string[] strArray = this.GetCurrentLocalDirectory().Split(Path.DirectorySeparatorChar);
              if (strArray.Length > 1)
              {
                strArray[strArray.Length - 1] = string.Empty;
                this.SetCurrentLocalDirectory(Path.Combine(strArray));
              }
              else
                this.SetCurrentLocalDirectory(string.Empty);
            }
            else
              this.SetCurrentLocalDirectory(Path.Combine(this.GetCurrentLocalDirectory(), this.SelectedBlueprint.BlueprintName));
            MyGuiBlueprintScreen_Reworked.CheckCurrentLocalDirectory_Blueprint();
            this.RefreshAndReloadItemList();
            break;
          }
          this.m_blueprintBeingLoaded = true;
          switch (this.SelectedBlueprint.Type)
          {
            case MyBlueprintTypeEnum.WORKSHOP:
              this.m_blueprintBeingLoaded = true;
              MyGuiBlueprintScreen_Reworked.Task = Parallel.Start((Action) (() =>
              {
                if (MyWorkshop.IsUpToDate(this.SelectedBlueprint.Item))
                  return;
                this.DownloadBlueprintFromSteam(this.SelectedBlueprint.Item);
              }), (Action) (() => this.CopyBlueprintAndClose()));
              return;
            case MyBlueprintTypeEnum.LOCAL:
            case MyBlueprintTypeEnum.DEFAULT:
            case MyBlueprintTypeEnum.CLOUD:
              this.m_blueprintBeingLoaded = true;
              this.CopyBlueprintAndClose();
              return;
            case MyBlueprintTypeEnum.SHARED:
              this.OpenSharedBlueprint(this.SelectedBlueprint);
              return;
            default:
              return;
          }
        case Content.Script:
          this.OpenSelectedSript();
          break;
      }
    }

    private void OnButton_Rename(MyGuiControlButton button)
    {
      if (this.SelectedBlueprint == null)
        return;
      Vector2 position = this.m_position;
      Action<string> callBack = (Action<string>) (result =>
      {
        if (result == null)
          return;
        result = MyUtils.StripInvalidChars(result);
        if (string.Equals(result, this.SelectedBlueprint.Data.Name, StringComparison.InvariantCulture) || string.IsNullOrEmpty(result))
          return;
        if (this.SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
          this.ChangeBlueprintNameCloud(result);
        else
          this.ChangeName(result);
      });
      string str = MyTexts.GetString(MySpaceTexts.DetailScreen_Button_Rename);
      string name = this.SelectedBlueprint.Data.Name;
      string caption = str;
      MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiBlueprintTextDialog(position, callBack, name, caption, 40, 0.3f));
    }

    private void OnButton_Replace(MyGuiControlButton button)
    {
      if (this.SelectedBlueprint == null)
        return;
      if (this.SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD && !MySandboxGame.Config.EnableSteamCloud)
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.Blueprints_ReplaceError_CloudOff), (object) MyGameService.Service.ServiceName)), messageCaption: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.Blueprints_ReplaceError_CloudOff_Caption), (object) MyGameService.Service.ServiceName))));
      else if (this.SelectedBlueprint.Type == MyBlueprintTypeEnum.LOCAL && MySandboxGame.Config.EnableSteamCloud)
      {
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.Blueprints_ReplaceError_CloudOn), (object) MyGameService.Service.ServiceName)), messageCaption: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.Blueprints_ReplaceError_CloudOn_Caption), (object) MyGameService.Service.ServiceName))));
      }
      else
      {
        switch (this.m_content)
        {
          case Content.Blueprint:
            if (this.SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
            {
              StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxTitle_Replace);
              MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxDesc_Replace), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn => this.CreateBlueprintFromClipboard(replace: true, info: this.SelectedBlueprint)))));
              break;
            }
            StringBuilder messageCaption1 = MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxTitle_Replace);
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxDesc_Replace), messageCaption1, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
            {
              if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES || this.m_clipboard == null || (this.m_clipboard.CopiedGrids == null || this.m_clipboard.CopiedGrids.Count == 0))
                return;
              string name = this.SelectedBlueprint.Data.Name;
              string str = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.GetCurrentLocalDirectory(), name, "bp.sbc");
              if (!File.Exists(str))
                return;
              MyObjectBuilder_Definitions prefab = MyBlueprintUtils.LoadPrefab(str);
              this.m_clipboard.CopiedGrids[0].DisplayName = name;
              prefab.ShipBlueprints[0].CubeGrids = this.m_clipboard.CopiedGrids.ToArray();
              prefab.ShipBlueprints[0].DLCs = this.GetNecessaryDLCs(prefab.ShipBlueprints[0].CubeGrids);
              MyBlueprintUtils.SavePrefabToFile(prefab, this.m_clipboard.CopiedGridsName, this.GetCurrentLocalDirectory(), true);
              this.RefreshBlueprintList();
            }))));
            break;
          case Content.Script:
            StringBuilder messageCaption2 = MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptNameDialogTitle);
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptDialogText), messageCaption2, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
            {
              if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
                return;
              string contents = this.m_getCodeFromEditor();
              if (this.SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
              {
                string str = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_TEMP, this.GetCurrentLocalDirectory(), this.SelectedBlueprint.Data.Name);
                string cloudPath = MyCloudHelper.Combine("Scripts/cloud", this.SelectedBlueprint.BlueprintName);
                MyCloudHelper.ExtractFilesTo(cloudPath, str, false);
                File.WriteAllText(Path.Combine(str, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION), contents, Encoding.UTF8);
                int num = (int) MyCloudHelper.UploadFiles(cloudPath, str, false);
              }
              else
              {
                string path = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, this.SelectedBlueprint.Data.Name, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION);
                if (!File.Exists(path))
                  return;
                File.WriteAllText(path, contents, Encoding.UTF8);
              }
            }))));
            break;
        }
      }
    }

    private void OnButton_Delete(MyGuiControlButton button)
    {
      if (this.SelectedBlueprint == null)
        return;
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.Delete);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.DeleteBlueprintQuestion), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
      {
        if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES || this.SelectedBlueprint == null)
          return;
        switch (this.m_content)
        {
          case Content.Blueprint:
            switch (this.SelectedBlueprint.Type)
            {
              case MyBlueprintTypeEnum.LOCAL:
              case MyBlueprintTypeEnum.DEFAULT:
                if (this.DeleteItem(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.GetCurrentLocalDirectory(), this.SelectedBlueprint.BlueprintName)))
                {
                  this.SelectedBlueprint = (MyBlueprintItemInfo) null;
                  this.ResetBlueprintUI();
                  break;
                }
                break;
              case MyBlueprintTypeEnum.CLOUD:
                this.DeleteBlueprintCloud();
                break;
            }
            break;
          case Content.Script:
            switch (this.SelectedBlueprint.Type)
            {
              case MyBlueprintTypeEnum.LOCAL:
              case MyBlueprintTypeEnum.DEFAULT:
                if (this.DeleteItem(Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, this.GetCurrentLocalDirectory(), this.SelectedBlueprint.Data.Name)))
                {
                  this.SelectedBlueprint = (MyBlueprintItemInfo) null;
                  this.ResetBlueprintUI();
                  break;
                }
                break;
              case MyBlueprintTypeEnum.CLOUD:
                this.DeleteScriptCloud();
                break;
            }
            break;
        }
        this.RefreshBlueprintList();
      }))));
    }

    private void OnButton_TakeScreenshot(MyGuiControlButton button)
    {
      if (this.SelectedBlueprint == null)
        return;
      switch (this.SelectedBlueprint.Type)
      {
        case MyBlueprintTypeEnum.LOCAL:
          this.TakeScreenshotLocalBP(this.SelectedBlueprint.Data.Name, this.m_selectedButton);
          break;
        case MyBlueprintTypeEnum.CLOUD:
          string str = Path.Combine("Blueprints/cloud", this.SelectedBlueprint.BlueprintName, MyBlueprintUtils.THUMB_IMAGE_NAME);
          string pathFull = Path.Combine(MyFileSystem.UserDataPath, str);
          this.TakeScreenshotCloud(str, pathFull, this.m_selectedButton);
          break;
      }
    }

    private void OnButton_Publish(MyGuiControlButton button)
    {
      string localDirectory = this.GetCurrentLocalDirectory();
      switch (this.m_content)
      {
        case Content.Blueprint:
          if (this.SelectedBlueprint == null)
            break;
          string path;
          if (this.SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
          {
            path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_TEMP, localDirectory, this.SelectedBlueprint.Data.Name);
            MyCloudHelper.ExtractFilesTo(MyCloudHelper.Combine("Blueprints/cloud", this.SelectedBlueprint.BlueprintName) + "/", path, true);
          }
          else
            path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.SelectedBlueprint.Data.Name);
          string path1 = Path.Combine(path, "bp.sbc");
          if (!File.Exists(path1))
            break;
          MyGuiBlueprintScreen_Reworked.m_LoadPrefabData = new LoadPrefabData((MyObjectBuilder_Definitions) null, path1, (MyGuiBlueprintScreen_Reworked) null);
          Action<WorkData> completionCallback = (Action<WorkData>) (workData =>
          {
            LoadPrefabData loadPrefabData = workData as LoadPrefabData;
            if (loadPrefabData.Prefab != null)
              MyBlueprintUtils.PublishBlueprint(loadPrefabData.Prefab, this.SelectedBlueprint.Data.Name, localDirectory, path, this.SelectedBlueprint.Type);
            LoadPrefabData.CallOnPrefabLoaded((WorkData) loadPrefabData);
          });
          MyGuiBlueprintScreen_Reworked.Task = Parallel.Start(LoadPrefabData.CallLoadPrefab, completionCallback, (WorkData) MyGuiBlueprintScreen_Reworked.m_LoadPrefabData);
          break;
        case Content.Script:
          if (this.SelectedBlueprint == null)
            break;
          string str;
          if (this.SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
          {
            str = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_TEMP, this.SelectedBlueprint.Data.Name);
            MyCloudHelper.ExtractFilesTo(MyCloudHelper.Combine("Scripts/cloud", this.SelectedBlueprint.BlueprintName) + "/", str, false);
          }
          else
            str = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, localDirectory, this.SelectedBlueprint.Data.Name);
          MyBlueprintUtils.PublishScript(str, this.SelectedBlueprint, (Action) (() => this.m_wasPublished = true));
          break;
      }
    }

    private void OnSendToPlayer()
    {
      if (this.m_sendToCombo.GetSelectedIndex() == 0)
        return;
      if (this.SelectedBlueprint?.Item == null)
        this.m_sendToCombo.SelectItemByIndex(0);
      else
        MyMultiplayer.RaiseStaticEvent<ulong, string, string, ulong, string>((Func<IMyEventOwner, Action<ulong, string, string, ulong, string>>) (x => new Action<ulong, string, string, ulong, string>(MyGuiBlueprintScreen_Reworked.ShareBlueprintRequest)), this.SelectedBlueprint.Item.Id, this.SelectedBlueprint.Item.ServiceName, this.SelectedBlueprint.Data.Name, (ulong) this.m_sendToCombo.GetSelectedKey(), MySession.Static.LocalHumanPlayer.DisplayName);
    }

    private void OnSelectItem(MyGuiControlRadioButtonGroup args)
    {
      switch (this.m_content)
      {
        case Content.Blueprint:
          this.SelectedBlueprint = (MyBlueprintItemInfo) null;
          this.m_selectedButton = (MyGuiControlContentButton) null;
          this.m_loadedPrefab = (MyObjectBuilder_Definitions) null;
          MyGuiBlueprintScreen_Reworked.m_LoadPrefabData = (LoadPrefabData) null;
          this.UpdateDetailKeyEnable();
          if (args.SelectedButton is MyGuiControlContentButton selectedButton)
          {
            if (!(selectedButton.UserData is MyBlueprintItemInfo userData))
              break;
            this.m_selectedButton = selectedButton;
            this.SelectedBlueprint = userData;
          }
          this.UpdatePrefab(this.SelectedBlueprint, false);
          break;
        case Content.Script:
          this.SelectedBlueprint = (MyBlueprintItemInfo) null;
          this.m_selectedButton = (MyGuiControlContentButton) null;
          if (args.SelectedButton is MyGuiControlContentButton selectedButton)
          {
            if (!(selectedButton.UserData is MyBlueprintItemInfo userData))
              break;
            this.m_selectedButton = selectedButton;
            this.SelectedBlueprint = userData;
          }
          this.UpdateNameAndDescription();
          this.UpdateInfo((XDocument) null, this.SelectedBlueprint);
          this.UpdateDetailKeyEnable();
          break;
      }
    }

    private void OnSearchTextChange(string message)
    {
      this.ApplyFiltering();
      this.TrySelectFirstBlueprint();
    }

    private void OnRateUpClicked(MyGuiControlButton button) => this.UpdateRateState(true);

    private void OnRateDownClicked(MyGuiControlButton button) => this.UpdateRateState(false);

    private void UpdateRateState(bool positive)
    {
      if (this.SelectedBlueprint == null || this.SelectedBlueprint.Item == null)
        return;
      this.SelectedBlueprint.Item.Rate(positive);
      this.SelectedBlueprint.Item.ChangeRatingValue(positive);
      this.m_button_RateUp.Checked = positive;
      this.m_button_RateDown.Checked = !positive;
    }

    private void OnReload(MyGuiControlButton button)
    {
      this.m_selectedButton = (MyGuiControlContentButton) null;
      this.SelectedBlueprint = (MyBlueprintItemInfo) null;
      this.UpdateDetailKeyEnable();
      MyGuiBlueprintScreen_Reworked.m_downloadFinished.Clear();
      MyGuiBlueprintScreen_Reworked.m_downloadQueued.Clear();
      this.RefreshAndReloadItemList();
      this.ApplyFiltering();
      this.TrySelectFirstBlueprint();
    }

    private void ResetBlueprintUI()
    {
      this.SelectedBlueprint = (MyBlueprintItemInfo) null;
      this.UpdateDetailKeyEnable();
    }

    public void RefreshBlueprintList(bool fromTask = false)
    {
      bool flag = false;
      MyBlueprintItemInfo itemInfo = (MyBlueprintItemInfo) null;
      if (this.SelectedBlueprint != null)
      {
        flag = true;
        itemInfo = this.SelectedBlueprint;
      }
      this.m_BPList.Clear();
      this.m_BPTypesGroup.Clear();
      switch (this.m_content)
      {
        case Content.Blueprint:
          this.GetLocalNames_Blueprints(fromTask);
          break;
        case Content.Script:
          this.GetLocalNames_Scripts(fromTask);
          break;
      }
      this.ApplyFiltering();
      this.m_selectedButton = (MyGuiControlContentButton) null;
      this.SelectedBlueprint = (MyBlueprintItemInfo) null;
      this.TrySelectFirstBlueprint();
      if (flag)
        this.SelectBlueprint(itemInfo);
      this.UpdateDetailKeyEnable();
    }

    public void RefreshAndReloadItemList()
    {
      this.m_BPList.Clear();
      this.m_BPTypesGroup.Clear();
      switch (this.m_content)
      {
        case Content.Blueprint:
          this.GetLocalNames_Blueprints(true);
          break;
        case Content.Script:
          this.GetLocalNames_Scripts(true);
          break;
      }
      this.ApplyFiltering();
      this.TrySelectFirstBlueprint();
    }

    private void SetGroupSelection(MyBlueprintTypeEnum option)
    {
      this.SetSelectedBlueprintType(option);
      this.SetIconForGroupSelection();
      this.ApplyFiltering();
      this.TrySelectFirstBlueprint();
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      switch (this.m_content)
      {
        case Content.Blueprint:
          if (!this.m_blueprintBeingLoaded)
            return base.CloseScreen(isUnloading);
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxDesc_StillLoading);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.BlueprintsMessageBoxTitle_StillLoading), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
          {
            if (result != MyGuiScreenMessageBox.ResultEnum.YES)
              return;
            this.m_blueprintBeingLoaded = false;
            MyGuiBlueprintScreen_Reworked.Task.valid = false;
            this.CloseScreen(isUnloading);
          }))));
          return false;
        case Content.Script:
          if (this.m_onCloseAction != null)
            this.m_onCloseAction();
          return base.CloseScreen(isUnloading);
        default:
          return base.CloseScreen(isUnloading);
      }
    }

    private void OpenSelectedSript()
    {
      if (this.SelectedBlueprint.Type == MyBlueprintTypeEnum.WORKSHOP)
        this.OpenSharedScript(this.SelectedBlueprint);
      else if (this.m_onScriptOpened != null)
      {
        string filePath;
        if (this.SelectedBlueprint.Type == MyBlueprintTypeEnum.CLOUD)
        {
          filePath = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_TEMP, this.GetCurrentLocalDirectory(), this.SelectedBlueprint.Data.Name);
          MyCloudHelper.ExtractFilesTo(MyCloudHelper.Combine(MyCloudHelper.Combine("Scripts/cloud", this.SelectedBlueprint.BlueprintName), MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION), filePath, false);
        }
        else
          filePath = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, this.GetCurrentLocalDirectory(), this.SelectedBlueprint.Data.Name, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION);
        this.m_onScriptOpened(filePath);
      }
      this.CloseScreen(false);
    }

    private void OpenSharedScript(MyBlueprintItemInfo itemInfo)
    {
      this.m_BPList.Enabled = false;
      MyGuiBlueprintScreen_Reworked.Task = Parallel.Start(new Action(this.DownloadScriptFromSteam), new Action(this.OnScriptDownloaded));
    }

    private void DownloadScriptFromSteam()
    {
      if (this.SelectedBlueprint == null)
        return;
      MyWorkshop.DownloadScriptBlocking(this.SelectedBlueprint.Item);
    }

    private void OnScriptDownloaded()
    {
      if (this.m_onScriptOpened != null && this.SelectedBlueprint != null)
        this.m_onScriptOpened(this.SelectedBlueprint.Item.Folder);
      this.m_BPList.Enabled = true;
    }

    private bool DeleteItem(string path)
    {
      if (!Directory.Exists(path))
        return false;
      Directory.Delete(path, true);
      return true;
    }

    private bool ValidateSelecteditem() => this.SelectedBlueprint != null && this.SelectedBlueprint.Data.Name != null;

    internal void OnPrefabLoaded(MyObjectBuilder_Definitions prefab)
    {
      this.m_blueprintBeingLoaded = false;
      if (prefab != null)
      {
        if (MySandboxGame.Static.SessionCompatHelper != null)
          MySandboxGame.Static.SessionCompatHelper.CheckAndFixPrefab(prefab);
        if (!MyGuiBlueprintScreen_Reworked.CheckBlueprintForModsAndModifiedBlocks(prefab))
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextDoYouWantToPasteGridWithMissingBlocks), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
          {
            if (result == MyGuiScreenMessageBox.ResultEnum.YES)
            {
              this.CloseScreen(false);
              if (MyGuiBlueprintScreen_Reworked.CopyBlueprintPrefabToClipboard(prefab, this.m_clipboard) && this.m_accessType == MyBlueprintAccessType.NORMAL)
              {
                if (MySession.Static.IsCopyPastingEnabled)
                  MySandboxGame.Static.Invoke((Action) (() => MyClipboardComponent.Static.Paste()), "BlueprintSelectionAutospawn2");
                else
                  MyClipboardComponent.ShowCannotPasteWarning();
              }
            }
          }))));
        }
        else
        {
          this.CloseScreen(false);
          if (!MyGuiBlueprintScreen_Reworked.CopyBlueprintPrefabToClipboard(prefab, this.m_clipboard) || this.m_accessType != MyBlueprintAccessType.NORMAL)
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

    private static bool CheckBlueprintForModsAndModifiedBlocks(MyObjectBuilder_Definitions prefab)
    {
      MyObjectBuilder_ShipBlueprintDefinition[] shipBlueprints = prefab.ShipBlueprints;
      return shipBlueprints == null || MyGridClipboard.CheckPastedBlocks((IEnumerable<MyObjectBuilder_CubeGrid>) shipBlueprints[0].CubeGrids);
    }

    private bool IsExtracted(MyWorkshopItem subItem)
    {
      switch (this.m_content)
      {
        case Content.Blueprint:
          return Directory.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, subItem.ServiceName, subItem.Id.ToString()));
        case Content.Script:
          return true;
        default:
          return false;
      }
    }

    private string GetImagePath(MyBlueprintItemInfo data)
    {
      string str = string.Empty;
      if (data.Type == MyBlueprintTypeEnum.LOCAL)
      {
        switch (this.m_content)
        {
          case Content.Blueprint:
            str = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.GetCurrentLocalDirectory(), data.BlueprintName, MyBlueprintUtils.THUMB_IMAGE_NAME);
            break;
          case Content.Script:
            str = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, this.GetCurrentLocalDirectory(), data.Data.Name, MyBlueprintUtils.THUMB_IMAGE_NAME);
            break;
        }
      }
      else if (data.Type == MyBlueprintTypeEnum.CLOUD)
      {
        str = data.Data.CloudImagePath;
        if (string.IsNullOrEmpty(str))
        {
          if (data.CloudPathXML != null)
            str = data.CloudPathXML.Replace(MyBlueprintUtils.BLUEPRINT_LOCAL_NAME, "thumb.png");
          else if (data.CloudPathCS != null)
            str = data.CloudPathCS.Replace(MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION, "thumb.png");
        }
      }
      else if (data.Type == MyBlueprintTypeEnum.WORKSHOP)
      {
        if (this.m_content == Content.Script)
          return Path.Combine(MyFileSystem.ContentPath, MyBlueprintUtils.STEAM_THUMBNAIL_NAME);
        if (data.Item != null)
        {
          bool flag1 = false;
          if (data.Item.Folder != null && MyFileSystem.IsDirectory(data.Item.Folder))
          {
            str = Path.Combine(data.Item.Folder, MyBlueprintUtils.THUMB_IMAGE_NAME);
            flag1 = true;
          }
          else
            str = Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, data.Item.ServiceName, data.Item.Id.ToString(), MyBlueprintUtils.THUMB_IMAGE_NAME);
          int num = MyGuiBlueprintScreen_Reworked.m_downloadQueued.Contains(new WorkshopId(data.Item.Id, data.Item.ServiceName)) ? 1 : 0;
          bool flag2 = MyGuiBlueprintScreen_Reworked.m_downloadFinished.Contains(new WorkshopId(data.Item.Id, data.Item.ServiceName));
          MyWorkshopItem worshopData = data.Item;
          if (flag2 && !this.IsExtracted(worshopData) && !flag1)
          {
            this.m_BPList.Enabled = false;
            this.ExtractWorkshopItem(worshopData);
            this.m_BPList.Enabled = true;
          }
          if (num == 0 && !flag2)
          {
            this.m_BPList.Enabled = false;
            MyGuiBlueprintScreen_Reworked.m_downloadQueued.Add(new WorkshopId(data.Item.Id, data.Item.ServiceName));
            MyGuiBlueprintScreen_Reworked.Task = Parallel.Start((Action) (() => this.DownloadBlueprintFromSteam(worshopData)), (Action) (() => this.OnBlueprintDownloadedThumbnail(worshopData)));
            str = string.Empty;
          }
        }
      }
      else if (data.Type == MyBlueprintTypeEnum.DEFAULT)
        str = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, data.BlueprintName, MyBlueprintUtils.THUMB_IMAGE_NAME);
      return str;
    }

    private void ExtractWorkshopItem(MyWorkshopItem subItem)
    {
      if (!MyFileSystem.IsDirectory(subItem.Folder))
      {
        try
        {
          string folder = subItem.Folder;
          string str1 = Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, subItem.ServiceName, subItem.Id.ToString());
          if (Directory.Exists(str1))
            Directory.Delete(str1, true);
          Directory.CreateDirectory(str1);
          MyObjectBuilder_ModInfo objectBuilderModInfo = new MyObjectBuilder_ModInfo();
          objectBuilderModInfo.SubtypeName = subItem.Title;
          objectBuilderModInfo.WorkshopIds = new WorkshopId[1]
          {
            new WorkshopId(subItem.Id, subItem.ServiceName)
          };
          objectBuilderModInfo.SteamIDOwner = subItem.OwnerId;
          string path = Path.Combine(MyBlueprintUtils.BLUEPRINT_WORKSHOP_TEMP, subItem.ServiceName, subItem.Id.ToString(), "info.temp");
          if (File.Exists(path))
            File.Delete(path);
          MyObjectBuilderSerializer.SerializeXML(path, false, (MyObjectBuilder_Base) objectBuilderModInfo);
          if (!string.IsNullOrEmpty(folder))
          {
            MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(folder);
            if (myZipArchive != null && myZipArchive.FileExists(MyBlueprintUtils.THUMB_IMAGE_NAME))
            {
              Stream stream = myZipArchive.GetFile(MyBlueprintUtils.THUMB_IMAGE_NAME).GetStream();
              if (stream != null)
              {
                string str2 = Path.Combine(str1, MyBlueprintUtils.THUMB_IMAGE_NAME);
                using (FileStream fileStream = File.Create(str2))
                  stream.CopyTo((Stream) fileStream);
                MyRenderProxy.UnloadTexture(str2);
              }
              stream.Close();
            }
            myZipArchive.Dispose();
          }
          else
            MyLog.Default.Critical(new StringBuilder("Path in Folder directory of blueprint \"" + subItem.Title + "\" " + (object) subItem.Id + " is null, it shouldn't be and who knows what problems it causes. "));
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine(ex);
        }
      }
      MyBlueprintItemInfo info = new MyBlueprintItemInfo(MyBlueprintTypeEnum.WORKSHOP)
      {
        BlueprintName = subItem.Title,
        Size = subItem.Size,
        Item = subItem
      };
      MyGuiControlListbox.Item listItem = new MyGuiControlListbox.Item(userData: ((object) info));
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        if (this.m_BPList.Controls.FindIndex((Predicate<MyGuiControlBase>) (item =>
        {
          if ((item.UserData as MyBlueprintItemInfo).Type != MyBlueprintTypeEnum.WORKSHOP)
            return false;
          ulong? id1 = (item.UserData as MyBlueprintItemInfo).Item?.Id;
          ulong? id2 = (listItem.UserData as MyBlueprintItemInfo).Item?.Id;
          return (long) id1.GetValueOrDefault() == (long) id2.GetValueOrDefault() & id1.HasValue == id2.HasValue && (item.UserData as MyBlueprintItemInfo).Item?.ServiceName == (listItem.UserData as MyBlueprintItemInfo).Item?.ServiceName;
        })) != -1)
          return;
        this.AddBlueprintButton(info, true);
      }), "AddBlueprintButton");
    }

    private void DownloadBlueprintFromSteam(MyWorkshopItem item)
    {
      if (MyWorkshop.IsUpToDate(item))
        return;
      MyWorkshop.DownloadBlueprintBlockingUGC(item, false);
      this.ExtractWorkshopItem(item);
    }

    private void OnBlueprintDownloadedThumbnail(MyWorkshopItem item)
    {
      MyGuiBlueprintScreen_Reworked.m_downloadQueued.Remove(new WorkshopId(item.Id, item.ServiceName));
      MyGuiBlueprintScreen_Reworked.m_downloadFinished.Add(new WorkshopId(item.Id, item.ServiceName));
    }

    private void GetBlueprints(string directory, MyBlueprintTypeEnum type)
    {
      List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
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
        if (File.Exists(path))
        {
          MyBlueprintItemInfo blueprintItemInfo = new MyBlueprintItemInfo(type)
          {
            TimeCreated = new DateTime?(File.GetCreationTimeUtc(path)),
            TimeUpdated = new DateTime?(File.GetLastWriteTimeUtc(path)),
            BlueprintName = str,
            Size = DirectoryExtensions.GetStorageSize(directories[index])
          };
          blueprintItemInfo.SetAdditionalBlueprintInformation(str, str);
          data.Add(blueprintItemInfo);
        }
      }
      this.SortBlueprints(data, MyBlueprintTypeEnum.LOCAL);
      this.AddBlueprintButtons(ref data);
    }

    private void GetLocalNames_Blueprints(bool reload = false)
    {
      this.GetBlueprints(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.GetCurrentLocalDirectory()), MyBlueprintTypeEnum.LOCAL);
      if (MySandboxGame.Config.EnableSteamCloud && MyGameService.IsActive)
        this.GetBlueprintsFromCloud("Blueprints/cloud", false);
      if (MyGuiBlueprintScreen_Reworked.Task.IsComplete)
      {
        if (reload)
          this.GetWorkshopItems();
        else
          this.GetWorkshopItemsSteam();
      }
      this.SortBlueprints(MyGuiBlueprintScreen_Reworked.m_recievedBlueprints, MyBlueprintTypeEnum.LOCAL);
      this.AddBlueprintButtons(ref MyGuiBlueprintScreen_Reworked.m_recievedBlueprints);
      if (!MyFakes.ENABLE_DEFAULT_BLUEPRINTS)
        return;
      this.GetBlueprints(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, MyBlueprintTypeEnum.DEFAULT);
    }

    private void GetLocalNames_Scripts(bool reload = false)
    {
      if (MySandboxGame.Config.EnableSteamCloud && MyGameService.IsActive)
        this.GetBlueprintsFromCloud("Scripts/cloud", true);
      string path = Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, this.GetCurrentLocalDirectory());
      if (!Directory.Exists(path))
        MyFileSystem.CreateDirectoryRecursive(path);
      if (!Directory.Exists(MyBlueprintUtils.SCRIPT_FOLDER_TEMP))
        MyFileSystem.CreateDirectoryRecursive(MyBlueprintUtils.SCRIPT_FOLDER_TEMP);
      string[] directories = Directory.GetDirectories(path);
      List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
      foreach (string str in directories)
      {
        if (MyBlueprintUtils.IsItem_Script(str))
        {
          string fileName = Path.GetFileName(str);
          MyBlueprintItemInfo blueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.LOCAL)
          {
            BlueprintName = fileName,
            Size = DirectoryExtensions.GetStorageSize(str)
          };
          blueprintItemInfo.SetAdditionalBlueprintInformation(fileName);
          data.Add(blueprintItemInfo);
        }
      }
      this.SortBlueprints(data, MyBlueprintTypeEnum.LOCAL);
      this.AddBlueprintButtons(ref data);
      if (!MyGuiBlueprintScreen_Reworked.Task.IsComplete)
        return;
      if (reload)
        this.GetWorkshopItems();
      else
        this.GetWorkshopItemsSteam();
    }

    private void GetWorkshopItems()
    {
      if (!MyGameService.IsActive)
        return;
      switch (this.m_content)
      {
        case Content.Blueprint:
          MyGuiBlueprintScreen_Reworked.Task = Parallel.Start(new Action(this.DownloadBlueprints));
          break;
        case Content.Script:
          MyGuiBlueprintScreen_Reworked.Task = Parallel.Start(new Action(this.GetScriptsInfo));
          break;
      }
    }

    private void GetScriptsInfo()
    {
      List<MyWorkshopItem> list = new List<MyWorkshopItem>();
      MyGuiBlueprintScreen_Reworked.m_downloadFromSteam = true;
      (MyGameServiceCallResult, string) ingameScriptsBlocking = MyWorkshop.GetSubscribedIngameScriptsBlocking(list);
      if (Directory.Exists(MyBlueprintUtils.SCRIPT_FOLDER_WORKSHOP))
      {
        try
        {
          Directory.Delete(MyBlueprintUtils.SCRIPT_FOLDER_WORKSHOP, true);
        }
        catch (IOException ex)
        {
        }
      }
      Directory.CreateDirectory(MyBlueprintUtils.SCRIPT_FOLDER_WORKSHOP);
      using (MyGuiBlueprintScreen_Reworked.SubscribedItemsLock.AcquireExclusiveUsing())
        this.SetSubscribeItemList(ref list);
      MyGuiBlueprintScreen_Reworked.m_needsExtract = true;
      MyGuiBlueprintScreen_Reworked.m_downloadFromSteam = false;
      this.UpdateWorkshopError(ingameScriptsBlocking.Item1, ingameScriptsBlocking.Item2);
    }

    private void DownloadBlueprints()
    {
      MyGuiBlueprintScreen_Reworked.m_downloadFromSteam = true;
      List<MyWorkshopItem> list = new List<MyWorkshopItem>();
      (MyGameServiceCallResult, string) blueprintsBlocking = MyWorkshop.GetSubscribedBlueprintsBlocking(list);
      Directory.CreateDirectory(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP);
      foreach (MyWorkshopItem myWorkshopItem in list)
      {
        this.DownloadBlueprintFromSteam(myWorkshopItem);
        MyGuiBlueprintScreen_Reworked.m_downloadFinished.Add(new WorkshopId(myWorkshopItem.Id, myWorkshopItem.ServiceName));
      }
      using (MyGuiBlueprintScreen_Reworked.SubscribedItemsLock.AcquireExclusiveUsing())
        this.SetSubscribeItemList(ref list);
      MyGuiBlueprintScreen_Reworked.m_needsExtract = true;
      MyGuiBlueprintScreen_Reworked.m_downloadFromSteam = false;
      this.UpdateWorkshopError(blueprintsBlocking.Item1, blueprintsBlocking.Item2);
    }

    private void UpdateWorkshopError(MyGameServiceCallResult result, string serviceName)
    {
      if (result != MyGameServiceCallResult.OK)
        this.SetWorkshopErrorText(MyWorkshop.GetWorkshopErrorText(result, serviceName, this.m_workshopPermitted));
      else
        this.SetWorkshopErrorText();
    }

    private void GetBlueprintsFromCloud(string cloudDirectory, bool scripts)
    {
      List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(cloudDirectory);
      if (cloudFiles == null)
        return;
      List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
      Dictionary<string, MyBlueprintItemInfo> dictionary = new Dictionary<string, MyBlueprintItemInfo>();
      foreach (MyCloudFileInfo myCloudFileInfo in cloudFiles)
      {
        string[] strArray = myCloudFileInfo.Name.Split('/');
        string name = strArray[strArray.Length - 2];
        string str1 = strArray[strArray.Length - 1];
        if (str1.Equals(MyBlueprintUtils.THUMB_IMAGE_NAME))
        {
          string str2 = Path.Combine(MyFileSystem.UserDataPath, Path.Combine(cloudDirectory, name));
          string imagePath = Path.Combine(str2, MyBlueprintUtils.THUMB_IMAGE_NAME);
          int fileSize = myCloudFileInfo.Size;
          try
          {
            Directory.CreateDirectory(str2);
            MyGameService.LoadFromCloudAsync(myCloudFileInfo.Name, (Action<byte[]>) (x => this.OnCloudImageDownloaded(x, name, imagePath, fileSize)));
          }
          catch (Exception ex)
          {
          }
        }
        else if (str1.Equals(MyBlueprintUtils.BLUEPRINT_LOCAL_NAME) && !scripts || str1.Equals(MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION) & scripts)
        {
          MyBlueprintItemInfo blueprintItemInfo;
          if (!dictionary.TryGetValue(name, out blueprintItemInfo))
          {
            blueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.CLOUD)
            {
              TimeCreated = new DateTime?(DateTime.FromFileTimeUtc(myCloudFileInfo.Timestamp)),
              TimeUpdated = new DateTime?(DateTime.FromFileTimeUtc(myCloudFileInfo.Timestamp)),
              BlueprintName = name,
              CloudInfo = myCloudFileInfo,
              Size = (ulong) myCloudFileInfo.Size
            };
            blueprintItemInfo.SetAdditionalBlueprintInformation(name, myCloudFileInfo.Name);
            dictionary.Add(name, blueprintItemInfo);
            data.Add(blueprintItemInfo);
          }
          blueprintItemInfo.Size += (ulong) myCloudFileInfo.Size;
          if (myCloudFileInfo.Name.EndsWith("B5"))
            blueprintItemInfo.CloudPathPB = myCloudFileInfo.Name;
          else if (myCloudFileInfo.Name.EndsWith(MyBlueprintUtils.BLUEPRINT_LOCAL_NAME))
            blueprintItemInfo.CloudPathXML = myCloudFileInfo.Name;
          else if (myCloudFileInfo.Name.EndsWith(MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION))
            blueprintItemInfo.CloudPathCS = myCloudFileInfo.Name;
        }
      }
      this.SortBlueprints(data, MyBlueprintTypeEnum.CLOUD);
      this.AddBlueprintButtons(ref data);
    }

    private void OnCloudImageDownloaded(
      byte[] data,
      string name,
      string imagePath,
      int fileSize,
      int writeAttempts = 1)
    {
      try
      {
        File.WriteAllBytes(imagePath, data);
        MyRenderProxy.UnloadTexture(imagePath);
        MyRenderProxy.PreloadTextures((IEnumerable<string>) new string[1]
        {
          imagePath
        }, TextureType.GUIWithoutPremultiplyAlpha);
        int index = this.m_BPList.Controls.FindIndex((Predicate<MyGuiControlBase>) (item => ((MyBlueprintItemInfo) item.UserData).BlueprintName == name));
        if (index < 0)
          return;
        MyBlueprintItemInfo userData = (MyBlueprintItemInfo) this.m_BPList.Controls[index].UserData;
        userData.Data.CloudImagePath = imagePath;
        userData.Size += (ulong) fileSize;
        ((MyGuiControlContentButton) this.m_BPList.Controls[index]).CreatePreview(imagePath);
      }
      catch (Exception ex)
      {
        if (writeAttempts >= 5)
          return;
        Thread.CurrentThread.Join(50);
        this.OnCloudImageDownloaded(data, name, imagePath, fileSize, ++writeAttempts);
      }
    }

    private void GetWorkshopItemsSteam()
    {
      List<MyBlueprintItemInfo> data = new List<MyBlueprintItemInfo>();
      using (MyGuiBlueprintScreen_Reworked.SubscribedItemsLock.AcquireSharedUsing())
      {
        List<MyWorkshopItem> subscribedItemsList = this.GetSubscribedItemsList();
        for (int index = 0; index < subscribedItemsList.Count; ++index)
        {
          MyWorkshopItem myWorkshopItem = subscribedItemsList[index];
          IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(myWorkshopItem.ServiceName);
          if (aggregate == null || aggregate.IsConsentGiven)
          {
            string title = myWorkshopItem.Title;
            MyBlueprintItemInfo blueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.WORKSHOP)
            {
              BlueprintName = title,
              Item = myWorkshopItem,
              Size = myWorkshopItem.Size
            };
            blueprintItemInfo.SetAdditionalBlueprintInformation(title, title, myWorkshopItem.DLCs.ToArray<uint>());
            data.Add(blueprintItemInfo);
          }
        }
      }
      this.SortBlueprints(data, MyBlueprintTypeEnum.WORKSHOP);
      this.AddBlueprintButtons(ref data);
    }

    private static void CheckCurrentLocalDirectory_Blueprint()
    {
      if (Directory.Exists(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, MyGuiBlueprintScreen_Reworked.GetCurrentLocalDirectory(Content.Blueprint))))
        return;
      MyGuiBlueprintScreen_Reworked.SetCurrentLocalDirectory(Content.Blueprint, string.Empty);
    }

    private bool UpdatePrefab(MyBlueprintItemInfo data, bool loadPrefab)
    {
      bool flag = true;
      this.m_loadedPrefab = (MyObjectBuilder_Definitions) null;
      if (data != null)
      {
        switch (data.Type)
        {
          case MyBlueprintTypeEnum.WORKSHOP:
            if (data.Item != null)
            {
              flag = false;
              MyWorkshopItem workshopData = data.Item;
              MyGuiBlueprintScreen_Reworked.Task = Parallel.Start((Action) (() =>
              {
                if (!MyWorkshop.IsUpToDate(workshopData))
                  this.DownloadBlueprintFromSteam(workshopData);
                this.OnBlueprintDownloadedDetails(workshopData, loadPrefab);
              }), (Action) (() => {}));
              break;
            }
            break;
          case MyBlueprintTypeEnum.LOCAL:
            string str1 = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, MyGuiBlueprintScreen_Reworked.GetCurrentLocalDirectory(Content.Blueprint), data.Data.Name, "bp.sbc");
            if (File.Exists(str1))
            {
              if (loadPrefab)
                this.m_loadedPrefab = MyBlueprintUtils.LoadPrefab(str1);
              try
              {
                using (FileStream fileStream = new FileStream(str1, FileMode.Open))
                {
                  this.UpdateNameAndDescription();
                  this.UpdateInfo(this.LoadXDocument((Stream) fileStream), data);
                  this.UpdateDetailKeyEnable();
                  break;
                }
              }
              catch (Exception ex)
              {
                MyLog.Default.WriteLine(string.Format("Failed to open {0}.\nException: {1}", (object) str1, (object) ex.Message));
                break;
              }
            }
            else
              break;
          case MyBlueprintTypeEnum.DEFAULT:
            string str2 = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, data.Data.Name, "bp.sbc");
            if (File.Exists(str2))
            {
              if (loadPrefab)
                this.m_loadedPrefab = MyBlueprintUtils.LoadPrefab(str2);
              using (FileStream fileStream = new FileStream(str2, FileMode.Open))
              {
                this.UpdateNameAndDescription();
                this.UpdateInfo(this.LoadXDocument((Stream) fileStream), data);
                this.UpdateDetailKeyEnable();
                break;
              }
            }
            else
              break;
          case MyBlueprintTypeEnum.CLOUD:
            if (loadPrefab)
              this.m_loadedPrefab = MyBlueprintUtils.LoadPrefabFromCloud(data);
            byte[] buffer = MyGameService.LoadFromCloud(data.CloudPathXML);
            if (buffer != null)
            {
              using (MemoryStream stream = new MemoryStream(buffer))
              {
                this.UpdateNameAndDescription();
                this.UpdateInfo(this.LoadXDocument(stream.UnwrapGZip()), data);
                this.UpdateDetailKeyEnable();
                break;
              }
            }
            else
              break;
        }
      }
      return flag;
    }

    private void OnBlueprintDownloadedDetails(MyWorkshopItem workshopDetails, bool loadPrefab = true)
    {
      if (this.SelectedBlueprint == null || this.SelectedBlueprint.Item == null || ((long) workshopDetails.Id != (long) this.SelectedBlueprint.Item.Id || workshopDetails.ServiceName != this.SelectedBlueprint.Item.ServiceName))
        return;
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        this.UpdateDetailKeyEnable();
        if (this.SelectedBlueprint == null || this.SelectedBlueprint.Item == null)
          return;
        this.UpdateNameAndDescription();
      }), nameof (OnBlueprintDownloadedDetails));
      string folder = workshopDetails.Folder;
      MyObjectBuilder_Definitions builderDefinitions = (MyObjectBuilder_Definitions) null;
      if (loadPrefab)
      {
        builderDefinitions = MyBlueprintUtils.LoadWorkshopPrefab(folder, this.SelectedBlueprint.Item?.Id, this.SelectedBlueprint.Item?.ServiceName, false);
        if (builderDefinitions == null)
          return;
      }
      if (this.SelectedBlueprint == null || this.SelectedBlueprint.Item == null || ((long) workshopDetails.Id != (long) this.SelectedBlueprint.Item.Id || workshopDetails.ServiceName != this.SelectedBlueprint.Item.ServiceName))
        return;
      if (loadPrefab)
        this.m_loadedPrefab = builderDefinitions;
      if (folder == null)
        return;
      string path = Path.Combine(folder, "bp.sbc");
      if (!MyFileSystem.FileExists(path))
        return;
      using (Stream sbcStream = MyFileSystem.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        XDocument doc = this.LoadXDocument(sbcStream);
        MySandboxGame.Static.Invoke((Action) (() => this.UpdateInfo(doc, this.SelectedBlueprint)), nameof (OnBlueprintDownloadedDetails));
      }
    }

    private void UpdateNameAndDescription()
    {
      if (this.SelectedBlueprint == null || this.m_detailName == null || this.m_multiline == null)
        return;
      if (this.SelectedBlueprint.Item == null && this.SelectedBlueprint.Data != null)
      {
        this.m_detailName.Text = this.SelectedBlueprint.Data.Name;
        StringBuilder stringBuilder = new StringBuilder(this.SelectedBlueprint.Data.Description);
        if (this.SelectedBlueprint.Data.DLCs == null || this.SelectedBlueprint.Data.DLCs.Length == 0)
        {
          this.m_multiline.Text = stringBuilder;
          this.CheckAndSplitLongName();
        }
        else
          this.m_multiline.Text = stringBuilder;
      }
      else
      {
        if (this.SelectedBlueprint.Item == null)
          return;
        this.m_multiline.Text = new StringBuilder(this.SelectedBlueprint.Item.Description);
        string str = this.SelectedBlueprint.Item.Title;
        if (str.Length > 80)
          str = str.Substring(0, 80);
        this.m_detailName.Text = str;
        this.CheckAndSplitLongName();
      }
    }

    public void OnPathSelected(bool confirmed, string pathNew)
    {
      if (!confirmed)
        return;
      MyGuiBlueprintScreen_Reworked.SetCurrentLocalDirectory(this.m_content, pathNew);
      this.RefreshAndReloadItemList();
    }

    public void CreateBlueprintFromClipboard(
      bool withScreenshot = false,
      bool replace = false,
      MyBlueprintItemInfo info = null)
    {
      string blueprintName = info?.BlueprintName;
      if (this.m_clipboard.CopiedGrids == null || this.m_clipboard.CopiedGrids.Count <= 0 || this.m_clipboard.CopiedGridsName == null && string.IsNullOrEmpty(blueprintName))
        return;
      string str1 = string.IsNullOrEmpty(blueprintName) ? MyUtils.StripInvalidChars(this.m_clipboard.CopiedGridsName) : blueprintName;
      string str2 = str1;
      ulong num1 = 0;
      if (!replace)
      {
        int num2 = 1;
        if (MySandboxGame.Config.EnableSteamCloud && MyGameService.IsActive)
        {
          string str3 = "Blueprints/cloud/";
          string directoryFilter;
          for (List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(str3 + str1 + "/"); cloudFiles != null && cloudFiles.Count > 0; cloudFiles = MyGameService.GetCloudFiles(directoryFilter))
          {
            str2 = str1 + "_" + (object) num2;
            directoryFilter = str3 + str2 + "/";
            ++num2;
          }
        }
        else
        {
          string path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.GetCurrentLocalDirectory(), str1);
          while (MyFileSystem.DirectoryExists(path))
          {
            str2 = str1 + "_" + (object) num2;
            path = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.GetCurrentLocalDirectory(), str2);
            ++num2;
          }
        }
      }
      else if (MySandboxGame.Config.EnableSteamCloud)
      {
        MyObjectBuilder_Definitions builderDefinitions = MyBlueprintUtils.LoadPrefabFromCloud(info);
        if (builderDefinitions != null)
        {
          MyObjectBuilder_ShipBlueprintDefinition[] shipBlueprints = builderDefinitions.ShipBlueprints;
          if ((shipBlueprints != null ? ((uint) shipBlueprints.Length > 0U ? 1 : 0) : 0) != 0)
            num1 = builderDefinitions.ShipBlueprints[0].WorkshopId;
        }
      }
      MyObjectBuilder_ShipBlueprintDefinition newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ShipBlueprintDefinition>();
      newObject1.Id = (SerializableDefinitionId) new MyDefinitionId(new MyObjectBuilderType(typeof (MyObjectBuilder_ShipBlueprintDefinition)), MyUtils.StripInvalidChars(str1));
      newObject1.CubeGrids = this.m_clipboard.CopiedGrids.ToArray();
      if (newObject1.CubeGrids == null || newObject1.CubeGrids.Length == 0)
        return;
      newObject1.DLCs = this.GetNecessaryDLCs(newObject1.CubeGrids);
      newObject1.RespawnShip = false;
      newObject1.DisplayName = MyGameService.UserName;
      newObject1.OwnerSteamId = Sync.MyId;
      newObject1.CubeGrids[0].DisplayName = str1;
      newObject1.WorkshopId = num1;
      MyObjectBuilder_Definitions newObject2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
      newObject2.ShipBlueprints = new MyObjectBuilder_ShipBlueprintDefinition[1];
      newObject2.ShipBlueprints[0] = newObject1;
      if (((!MyGameService.IsActive ? 0 : (MySandboxGame.Config.EnableSteamCloud ? 1 : 0)) & (withScreenshot ? 1 : 0)) != 0)
      {
        this.SavePrefabToCloudWithScreenshot(newObject2, str2, this.GetCurrentLocalDirectory(), replace, new Action<string, CloudResult>(this.SelectNewBlueprintAfterCloudSave));
      }
      else
      {
        MyBlueprintUtils.SavePrefabToFile(newObject2, str2, this.GetCurrentLocalDirectory(), replace);
        this.SetGroupSelection(MyBlueprintTypeEnum.MIXED);
        this.RefreshAndSelectNewBlueprint(str2, !MySandboxGame.Config.EnableSteamCloud || !MyGameService.IsActive ? MyBlueprintTypeEnum.LOCAL : MyBlueprintTypeEnum.CLOUD);
        if (!withScreenshot)
          return;
        this.TakeScreenshotLocalBP(str2, this.m_selectedButton);
      }
    }

    private string[] GetNecessaryDLCs(MyObjectBuilder_CubeGrid[] cubeGrids)
    {
      if (cubeGrids.IsNullOrEmpty<MyObjectBuilder_CubeGrid>())
        return (string[]) null;
      HashSet<string> source = new HashSet<string>();
      foreach (MyObjectBuilder_CubeGrid cubeGrid in cubeGrids)
      {
        foreach (MyObjectBuilder_CubeBlock cubeBlock in cubeGrid.CubeBlocks)
        {
          MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(cubeBlock);
          if (cubeBlockDefinition != null && cubeBlockDefinition.DLCs != null && cubeBlockDefinition.DLCs.Length != 0)
          {
            for (int index = 0; index < cubeBlockDefinition.DLCs.Length; ++index)
              source.Add(cubeBlockDefinition.DLCs[index]);
          }
        }
      }
      return source.ToArray<string>();
    }

    public void SelectBlueprint(MyBlueprintItemInfo itemInfo)
    {
      int idx = -1;
      MyGuiControlContentButton butt = (MyGuiControlContentButton) null;
      foreach (MyGuiControlRadioButton controlRadioButton in this.m_BPTypesGroup)
      {
        ++idx;
        if (controlRadioButton.UserData is MyBlueprintItemInfo userData && userData.Equals((object) itemInfo))
        {
          butt = controlRadioButton as MyGuiControlContentButton;
          break;
        }
      }
      if (butt == null)
        return;
      this.SelectButton(butt, idx);
    }

    public void RefreshAndSelectNewBlueprint(string name, MyBlueprintTypeEnum type)
    {
      this.RefreshBlueprintList();
      this.SelectNewBlueprint(name, type);
    }

    public void SelectNewBlueprint(string name, MyBlueprintTypeEnum type)
    {
      int idx = -1;
      MyGuiControlContentButton butt = (MyGuiControlContentButton) null;
      foreach (MyGuiControlRadioButton controlRadioButton in this.m_BPTypesGroup)
      {
        ++idx;
        if (controlRadioButton.UserData is MyBlueprintItemInfo userData && userData.Type == type && userData.Data.Name.Equals(name))
        {
          butt = controlRadioButton as MyGuiControlContentButton;
          break;
        }
      }
      if (butt == null)
        return;
      this.SelectButton(butt, idx);
    }

    public void SelectButton(
      MyGuiControlContentButton butt,
      int idx = -1,
      bool forceToTop = true,
      bool alwaysScroll = true)
    {
      if (idx < 0)
      {
        bool flag = false;
        int num = -1;
        foreach (MyGuiControlRadioButton controlRadioButton in this.m_BPTypesGroup)
        {
          ++num;
          if (butt == controlRadioButton)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return;
        idx = num;
      }
      if (this.m_selectedButton != butt)
        this.m_BPTypesGroup.SelectByIndex(idx);
      float min;
      float max;
      MyGuiBlueprintScreen_Reworked.ScrollTestResult scrollTestResult = this.ShouldScroll(butt, idx, out min, out max);
      if ((alwaysScroll ? 1 : ((uint) scrollTestResult > 0U ? 1 : 0)) == 0)
        return;
      float page;
      if (!forceToTop)
      {
        switch (scrollTestResult)
        {
          case MyGuiBlueprintScreen_Reworked.ScrollTestResult.Higher:
            page = max - 1f;
            goto label_21;
          case MyGuiBlueprintScreen_Reworked.ScrollTestResult.Lower:
            break;
          default:
            return;
        }
      }
      page = min;
label_21:
      this.m_BPList.SetScrollBarPage(page);
    }

    private MyGuiBlueprintScreen_Reworked.ScrollTestResult ShouldScroll(
      MyGuiControlContentButton butt,
      int idx,
      out float min,
      out float max)
    {
      float num1 = !this.GetThumbnailVisibility() ? MyGuiBlueprintScreen_Reworked.MAGIC_SPACING_SMALL : MyGuiBlueprintScreen_Reworked.MAGIC_SPACING_BIG;
      int num2 = 0;
      for (int index = 0; index < idx; ++index)
      {
        if (!this.m_BPList.Controls[index].Visible)
          ++num2;
      }
      float num3 = butt.Size.Y + this.m_BPList.GetItemMargins().SizeChange.Y - num1;
      float y = this.m_BPList.Size.Y;
      float num4 = (float) (idx - num2) * num3 / y;
      float num5 = ((float) (idx - num2) + 1f) * num3 / y;
      float page = this.m_BPList.GetScrollBar().GetPage();
      min = num4;
      max = num5;
      if ((double) num4 < (double) page)
        return MyGuiBlueprintScreen_Reworked.ScrollTestResult.Lower;
      return (double) num5 > (double) page + 1.0 ? MyGuiBlueprintScreen_Reworked.ScrollTestResult.Higher : MyGuiBlueprintScreen_Reworked.ScrollTestResult.Ok;
    }

    public void SavePrefabToCloudWithScreenshot(
      MyObjectBuilder_Definitions prefab,
      string name,
      string currentDirectory,
      bool replace = false,
      Action<string, CloudResult> onCompleted = null)
    {
      char[] invalidPathChars = Path.GetInvalidPathChars();
      if (name.Any<char>((Func<char, bool>) (x => invalidPathChars.Contains<char>(x))))
      {
        StringBuilder stringBuilder = new StringBuilder(name);
        for (int index = 0; index < name.Length; ++index)
        {
          if (invalidPathChars.Contains<char>(name[index]))
            stringBuilder[index] = '_';
        }
        MyLog.Default.WriteLine("Blueprint name with invalid characters: " + name + ", renamed to " + (object) stringBuilder);
        name = stringBuilder.ToString();
      }
      string path1 = Path.Combine("Blueprints/cloud", name);
      string filePath = Path.Combine(path1, MyBlueprintUtils.BLUEPRINT_LOCAL_NAME);
      string str1 = Path.Combine(path1, MyBlueprintUtils.THUMB_IMAGE_NAME);
      if (MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.IsWaiting())
      {
        if (onCompleted == null)
          return;
        onCompleted(name, CloudResult.Failed);
      }
      else
      {
        string str2 = Path.Combine(MyFileSystem.UserDataPath, str1);
        MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.SetData_CreateNewBlueprintCloud(str1, str2);
        MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), str2, false, true, false);
        MyBlueprintUtils.SaveToCloud(prefab, filePath, replace, onCompleted);
      }
    }

    private void CreateScriptFromEditor()
    {
      if (this.m_getCodeFromEditor == null)
        return;
      bool flag = MySandboxGame.Config.EnableSteamCloud && MyGameService.IsActive;
      string sourceFileName = Path.Combine(MyFileSystem.ContentPath, MyBlueprintUtils.STEAM_THUMBNAIL_NAME);
      string str1 = flag ? MyBlueprintUtils.SCRIPT_FOLDER_TEMP : MyBlueprintUtils.SCRIPT_FOLDER_LOCAL;
      string contents = this.m_getCodeFromEditor();
      Directory.CreateDirectory(str1);
      int num1 = 0;
      if (!Directory.Exists(str1))
        return;
      string str2;
      string str3;
      do
      {
        str2 = MyBlueprintUtils.DEFAULT_SCRIPT_NAME + "_" + (object) num1;
        str3 = Path.Combine(str1, this.GetCurrentLocalDirectory(), str2);
        ++num1;
      }
      while (Directory.Exists(str3));
      Directory.CreateDirectory(str3);
      string str4 = Path.Combine(str3, MyBlueprintUtils.THUMB_IMAGE_NAME);
      File.Copy(sourceFileName, str4, true);
      MyRenderProxy.UnloadTexture(str4);
      File.WriteAllText(Path.Combine(str3, MyBlueprintUtils.DEFAULT_SCRIPT_NAME + MyBlueprintUtils.SCRIPT_EXTENSION), contents, Encoding.UTF8);
      if (flag)
      {
        int num2 = 0;
        string name;
        string str5;
        List<MyCloudFileInfo> cloudFiles;
        do
        {
          name = MyBlueprintUtils.DEFAULT_SCRIPT_NAME + "_" + (object) num2;
          str5 = "Scripts/cloud/" + name + "/";
          cloudFiles = MyGameService.GetCloudFiles(str5);
          ++num2;
        }
        while (cloudFiles != null && cloudFiles.Count > 0);
        int num3 = (int) MyCloudHelper.UploadFiles(str5, str3, false);
        this.RefreshAndSelectNewBlueprint(name, MyBlueprintTypeEnum.CLOUD);
      }
      else
        this.RefreshAndSelectNewBlueprint(str2, MyBlueprintTypeEnum.LOCAL);
    }

    private void ChangeName(string name)
    {
      name = MyUtils.StripInvalidChars(name);
      string path1 = string.Empty;
      switch (this.m_content)
      {
        case Content.Blueprint:
          path1 = MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL;
          break;
        case Content.Script:
          path1 = MyBlueprintUtils.SCRIPT_FOLDER_LOCAL;
          break;
      }
      string name1 = this.SelectedBlueprint.Data.Name;
      string file = Path.Combine(path1, this.GetCurrentLocalDirectory(), name1);
      string newFile = Path.Combine(path1, this.GetCurrentLocalDirectory(), name);
      if (file == newFile || !Directory.Exists(file))
        return;
      if (Directory.Exists(newFile))
      {
        if (file.ToLower() == newFile.ToLower())
        {
          switch (this.m_content)
          {
            case Content.Blueprint:
              this.UpdatePrefab(this.SelectedBlueprint, true);
              this.m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
              this.m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
              this.m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
              string str = Path.Combine(path1, "temp");
              if (Directory.Exists(str))
                Directory.Delete(str, true);
              Directory.Move(file, str);
              Directory.Move(str, newFile);
              MyBlueprintUtils.SavePrefabToFile(this.m_loadedPrefab, name, this.GetCurrentLocalDirectory(), true);
              break;
            case Content.Script:
              if (Directory.Exists(file))
                Directory.Move(file, newFile);
              if (Directory.Exists(file))
              {
                Directory.Delete(file, true);
                break;
              }
              break;
          }
          this.RefreshBlueprintList();
          this.UpdatePrefab(this.SelectedBlueprint, false);
          using (FileStream fileStream = new FileStream(Path.Combine(newFile, "bp.sbc"), FileMode.Open))
            this.UpdateInfo(this.LoadXDocument((Stream) fileStream), this.SelectedBlueprint);
        }
        else
        {
          StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_Replace);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder().Append((object) MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ReplaceMessage1)).Append(name).Append((object) MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ReplaceMessage2)), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
          {
            if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
              return;
            switch (this.m_content)
            {
              case Content.Blueprint:
                this.DeleteItem(newFile);
                this.UpdatePrefab(this.SelectedBlueprint, true);
                this.m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
                this.m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
                this.m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
                Directory.Move(file, newFile);
                MyRenderProxy.UnloadTexture(Path.Combine(newFile, MyBlueprintUtils.THUMB_IMAGE_NAME));
                MyBlueprintUtils.SavePrefabToFile(this.m_loadedPrefab, name, this.GetCurrentLocalDirectory(), true);
                break;
              case Content.Script:
                Directory.Delete(newFile, true);
                if (Directory.Exists(file))
                  Directory.Move(file, newFile);
                if (Directory.Exists(file))
                {
                  Directory.Delete(file, true);
                  break;
                }
                break;
            }
            this.RefreshBlueprintList();
            this.UpdatePrefab(this.SelectedBlueprint, false);
            using (FileStream fileStream = new FileStream(Path.Combine(newFile, "bp.sbc"), FileMode.Open))
              this.UpdateInfo(this.LoadXDocument((Stream) fileStream), this.SelectedBlueprint);
          }))));
        }
      }
      else
      {
        try
        {
          switch (this.m_content)
          {
            case Content.Blueprint:
              this.UpdatePrefab(this.SelectedBlueprint, true);
              this.m_loadedPrefab.ShipBlueprints[0].Id.SubtypeId = name;
              this.m_loadedPrefab.ShipBlueprints[0].Id.SubtypeName = name;
              this.m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = name;
              Directory.Move(file, newFile);
              MyRenderProxy.UnloadTexture(Path.Combine(newFile, MyBlueprintUtils.THUMB_IMAGE_NAME));
              MyBlueprintUtils.SavePrefabToFile(this.m_loadedPrefab, name, this.GetCurrentLocalDirectory(), true);
              break;
            case Content.Script:
              if (Directory.Exists(file))
                Directory.Move(file, newFile);
              if (Directory.Exists(file))
              {
                Directory.Delete(file, true);
                break;
              }
              break;
          }
          this.RefreshBlueprintList();
          this.UpdatePrefab(this.SelectedBlueprint, false);
          using (FileStream fileStream = new FileStream(Path.Combine(newFile, "bp.sbc"), FileMode.Open))
            this.UpdateInfo(this.LoadXDocument((Stream) fileStream), this.SelectedBlueprint);
        }
        catch (IOException ex)
        {
          StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_Delete);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_DeleteMessage), messageCaption: messageCaption));
        }
      }
    }

    private void ChangeBlueprintNameCloud(string name)
    {
      List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(MyCloudHelper.Combine(this.m_content == Content.Blueprint ? "Blueprints/cloud" : "Scripts/cloud", name) + "/");
      if (cloudFiles == null)
        return;
      if (this.SelectedBlueprint != null && this.SelectedBlueprint.Data != null && this.SelectedBlueprint.Data.CloudImagePath != null)
        this.SelectedBlueprint.Data.CloudImagePath = this.SelectedBlueprint.Data.CloudImagePath.Replace("/", "\\");
      if (cloudFiles.Count > 0)
      {
        StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_Replace);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder().Append((object) MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ReplaceMessage1)).Append(name).Append((object) MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ReplaceMessage2)), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
        {
          if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
            return;
          this.ChangeBlueprintNameCloudInternal(name);
        }))));
      }
      else
        this.ChangeBlueprintNameCloudInternal(name);
    }

    private void ChangeBlueprintNameCloudInternal(string name)
    {
      if (this.m_content == Content.Blueprint)
      {
        this.UpdatePrefab(this.SelectedBlueprint, true);
        if (this.m_loadedPrefab == null)
        {
          this.ShowChangeNameCloudError(CloudResult.Failed);
        }
        else
        {
          MyGuiBlueprintScreen_Reworked.CloudBlueprintRenamer blueprintRenamer = new MyGuiBlueprintScreen_Reworked.CloudBlueprintRenamer(this.m_loadedPrefab, this.GetImagePath(this.SelectedBlueprint), name, new Action<CloudResult, string>(this.FinishChangeBlueprintNameCloud));
        }
      }
      else
      {
        if (this.m_content != Content.Script)
          return;
        string str = MyCloudHelper.Combine("Scripts/cloud", this.SelectedBlueprint.BlueprintName) + "/";
        string newSessionPath = MyCloudHelper.Combine("Scripts/cloud", name) + "/";
        CloudResult result = MyCloudHelper.CopyFiles(str, newSessionPath);
        if (result == CloudResult.Ok)
        {
          MyCloudHelper.Delete(str);
          this.SelectedBlueprint = (MyBlueprintItemInfo) null;
          this.ResetBlueprintUI();
          this.RefreshAndSelectNewBlueprint(name, MyBlueprintTypeEnum.CLOUD);
        }
        else
          this.ShowChangeNameCloudError(result);
      }
    }

    private void DeleteBlueprintCloud()
    {
      if (!MyCloudHelper.Delete(MyCloudHelper.Combine("Blueprints/cloud", this.SelectedBlueprint.BlueprintName) + "/"))
        return;
      this.SelectedBlueprint = (MyBlueprintItemInfo) null;
      this.ResetBlueprintUI();
    }

    private void DeleteScriptCloud()
    {
      if (!MyCloudHelper.Delete(MyCloudHelper.Combine("Scripts/cloud", this.SelectedBlueprint.BlueprintName) + "/"))
        return;
      this.SelectedBlueprint = (MyBlueprintItemInfo) null;
      this.ResetBlueprintUI();
    }

    private void FinishChangeBlueprintNameCloud(CloudResult result, string filePath)
    {
      if (result == CloudResult.Ok)
      {
        this.DeleteBlueprintCloud();
        this.SelectNewBlueprintAfterCloudSave(filePath, result);
      }
      else
        this.ShowChangeNameCloudError(result);
    }

    private void SelectNewBlueprintAfterCloudSave(string filePath, CloudResult result)
    {
      MyStringId errorMessage;
      if (!MyCloudHelper.IsError(result, out errorMessage))
      {
        string[] strArray = filePath.Split('\\', '/');
        if (strArray.Length <= 2)
          return;
        this.RefreshAndSelectNewBlueprint(strArray[strArray.Length - 2], MyBlueprintTypeEnum.CLOUD);
      }
      else
      {
        MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(errorMessage), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
        messageBox.SkipTransition = true;
        messageBox.InstantClose = false;
        MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
      }
    }

    private void ShowChangeNameCloudError(CloudResult result)
    {
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCloudHelper.GetErrorMessage(result)), messageCaption: messageCaption));
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
      if ((!MySession.Static.IsUserAdmin(Sync.MyId) || !MySession.Static.AdminSettings.HasFlag((Enum) AdminSettingsEnum.KeepOriginalOwnershipOnPaste)) && setOwner)
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

    private void CopyBlueprintAndClose()
    {
      if (!this.CopySelectedItemToClipboard())
        return;
      this.CloseScreen(false);
    }

    private bool CopySelectedItemToClipboard()
    {
      if (!this.ValidateSelecteditem())
        return false;
      MyObjectBuilder_Definitions prefab = (MyObjectBuilder_Definitions) null;
      this.m_clipboard.Deactivate();
      switch (this.SelectedBlueprint.Type)
      {
        case MyBlueprintTypeEnum.WORKSHOP:
          string folder = this.SelectedBlueprint.Item.Folder;
          if (File.Exists(folder) || MyFileSystem.IsDirectory(folder))
          {
            MyGuiBlueprintScreen_Reworked.m_LoadPrefabData = new LoadPrefabData(prefab, folder, this, new ulong?(this.SelectedBlueprint.Item.Id), this.SelectedBlueprint.Item.ServiceName);
            MyGuiBlueprintScreen_Reworked.Task = Parallel.Start(LoadPrefabData.CallLoadWorkshopPrefab, LoadPrefabData.CallOnPrefabLoaded, (WorkData) MyGuiBlueprintScreen_Reworked.m_LoadPrefabData);
            break;
          }
          break;
        case MyBlueprintTypeEnum.LOCAL:
          string path1 = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.GetCurrentLocalDirectory(), this.SelectedBlueprint.Data.Name, "bp.sbc");
          if (File.Exists(path1))
          {
            MyGuiBlueprintScreen_Reworked.m_LoadPrefabData = new LoadPrefabData(prefab, path1, this);
            MyGuiBlueprintScreen_Reworked.Task = Parallel.Start(LoadPrefabData.CallLoadPrefab, LoadPrefabData.CallOnPrefabLoaded, (WorkData) MyGuiBlueprintScreen_Reworked.m_LoadPrefabData);
            break;
          }
          break;
        case MyBlueprintTypeEnum.SHARED:
          return false;
        case MyBlueprintTypeEnum.DEFAULT:
          string path2 = Path.Combine(MyBlueprintUtils.BLUEPRINT_DEFAULT_DIRECTORY, this.SelectedBlueprint.Data.Name, "bp.sbc");
          if (File.Exists(path2))
          {
            MyGuiBlueprintScreen_Reworked.m_LoadPrefabData = new LoadPrefabData(prefab, path2, this);
            MyGuiBlueprintScreen_Reworked.Task = Parallel.Start(LoadPrefabData.CallLoadPrefab, LoadPrefabData.CallOnPrefabLoaded, (WorkData) MyGuiBlueprintScreen_Reworked.m_LoadPrefabData);
            break;
          }
          break;
        case MyBlueprintTypeEnum.CLOUD:
          MyGuiBlueprintScreen_Reworked.m_LoadPrefabData = new LoadPrefabData(prefab, this.SelectedBlueprint, this);
          MyGuiBlueprintScreen_Reworked.Task = Parallel.Start(LoadPrefabData.CallLoadPrefabFromCloud, LoadPrefabData.CallOnPrefabLoaded, (WorkData) MyGuiBlueprintScreen_Reworked.m_LoadPrefabData);
          break;
      }
      return false;
    }

    private void SortBlueprints(List<MyBlueprintItemInfo> list, MyBlueprintTypeEnum type)
    {
      MyItemComparer_Rew myItemComparerRew = (MyItemComparer_Rew) null;
      switch (type)
      {
        case MyBlueprintTypeEnum.WORKSHOP:
          switch (this.GetSelectedSort())
          {
            case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
              myItemComparerRew = new MyItemComparer_Rew((Func<MyBlueprintItemInfo, MyBlueprintItemInfo, int>) ((x, y) => x.BlueprintName.CompareTo(y.BlueprintName)));
              break;
            case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
              myItemComparerRew = new MyItemComparer_Rew((Func<MyBlueprintItemInfo, MyBlueprintItemInfo, int>) ((x, y) =>
              {
                DateTime timeCreated1 = x.Item.TimeCreated;
                DateTime timeCreated2 = y.Item.TimeCreated;
                if (timeCreated1 < timeCreated2)
                  return 1;
                return timeCreated1 > timeCreated2 ? -1 : 0;
              }));
              break;
            case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
              myItemComparerRew = new MyItemComparer_Rew((Func<MyBlueprintItemInfo, MyBlueprintItemInfo, int>) ((x, y) =>
              {
                DateTime timeUpdated1 = x.Item.TimeUpdated;
                DateTime timeUpdated2 = y.Item.TimeUpdated;
                if (timeUpdated1 < timeUpdated2)
                  return 1;
                return timeUpdated1 > timeUpdated2 ? -1 : 0;
              }));
              break;
          }
          break;
        case MyBlueprintTypeEnum.LOCAL:
        case MyBlueprintTypeEnum.CLOUD:
          switch (this.GetSelectedSort())
          {
            case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
              myItemComparerRew = new MyItemComparer_Rew((Func<MyBlueprintItemInfo, MyBlueprintItemInfo, int>) ((x, y) => x.BlueprintName.CompareTo(y.BlueprintName)));
              break;
            case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
              myItemComparerRew = new MyItemComparer_Rew((Func<MyBlueprintItemInfo, MyBlueprintItemInfo, int>) ((x, y) =>
              {
                DateTime? timeCreated1 = x.TimeCreated;
                DateTime? timeCreated2 = y.TimeCreated;
                return timeCreated1.HasValue && timeCreated2.HasValue ? -1 * DateTime.Compare(timeCreated1.Value, timeCreated2.Value) : 0;
              }));
              break;
            case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
              myItemComparerRew = new MyItemComparer_Rew((Func<MyBlueprintItemInfo, MyBlueprintItemInfo, int>) ((x, y) =>
              {
                DateTime? timeUpdated1 = x.TimeUpdated;
                DateTime? timeUpdated2 = y.TimeUpdated;
                return timeUpdated1.HasValue && timeUpdated2.HasValue ? -1 * DateTime.Compare(timeUpdated1.Value, timeUpdated2.Value) : 0;
              }));
              break;
          }
          break;
      }
      if (myItemComparerRew == null)
        return;
      list.Sort((IComparer<MyBlueprintItemInfo>) myItemComparerRew);
    }

    [Event(null, 4358)]
    [Reliable]
    [Server]
    public static void ShareBlueprintRequest(
      ulong workshopId,
      string workshopServiceName,
      string name,
      ulong sendToId,
      string senderName)
    {
      if (Sync.IsServer && (long) sendToId != (long) Sync.MyId)
        MyMultiplayer.RaiseStaticEvent<ulong, string, string, ulong, string>((Func<IMyEventOwner, Action<ulong, string, string, ulong, string>>) (x => new Action<ulong, string, string, ulong, string>(MyGuiBlueprintScreen_Reworked.ShareBlueprintRequestClient)), workshopId, workshopServiceName, name, sendToId, senderName, new EndpointId(sendToId));
      else
        MyGuiBlueprintScreen_Reworked.ShareBlueprintRequestClient(workshopId, workshopServiceName, name, sendToId, senderName);
    }

    [Event(null, 4371)]
    [Reliable]
    [Client]
    private static void ShareBlueprintRequestClient(
      ulong workshopId,
      string workshopServiceName,
      string name,
      ulong sendToId,
      string senderName)
    {
      ulong num = workshopId;
      MyWorkshopItem workshopItem = MyGameService.CreateWorkshopItem(workshopServiceName);
      if (workshopItem == null)
        return;
      workshopItem.Id = num;
      workshopItem.Title = name;
      MyBlueprintItemInfo info = new MyBlueprintItemInfo(MyBlueprintTypeEnum.SHARED)
      {
        BlueprintName = name,
        Item = workshopItem
      };
      info.SetAdditionalBlueprintInformation(name);
      if (MyGuiBlueprintScreen_Reworked.m_recievedBlueprints.Any<MyBlueprintItemInfo>((Func<MyBlueprintItemInfo, bool>) (item2 =>
      {
        ulong? id1 = item2.Item?.Id;
        ulong id2 = info.Item.Id;
        return (long) id1.GetValueOrDefault() == (long) id2 & id1.HasValue && item2.Item?.ServiceName == info.Item.ServiceName;
      })))
        return;
      MyGuiBlueprintScreen_Reworked.m_recievedBlueprints.Add(info);
      MyHudNotificationDebug notificationDebug = new MyHudNotificationDebug(string.Format(MyTexts.Get(MySpaceTexts.SharedBlueprintNotify).ToString(), (object) senderName));
      MyHud.Notifications.Add((MyHudNotificationBase) notificationDebug);
    }

    private void OpenSharedBlueprint(MyBlueprintItemInfo itemInfo) => MyGuiSandbox.OpenUrl(itemInfo.Item.GetItemUrl(), UrlOpenMode.SteamOrExternalWithConfirm, new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.SharedBlueprintQuestion), (object) itemInfo.Item.ServiceName), MyTexts.Get(MySpaceTexts.SharedBlueprint), new StringBuilder().AppendFormat(MyTexts.GetString(MySpaceTexts.SharedBlueprintQuestion), (object) itemInfo.Item.ServiceName), MyTexts.Get(MySpaceTexts.SharedBlueprint), (Action<bool>) (success =>
    {
      MyGuiBlueprintScreen_Reworked.m_recievedBlueprints.Remove(this.SelectedBlueprint);
      this.SelectedBlueprint = (MyBlueprintItemInfo) null;
      this.UpdateDetailKeyEnable();
      this.RefreshBlueprintList();
    }));

    private void ApplyFiltering(MyGuiControlContentButton button)
    {
      if (button == null)
        return;
      bool flag1 = this.m_searchBox != null && this.m_searchBox.SearchText != "";
      string[] strArray = new string[0];
      if (flag1)
        strArray = this.m_searchBox.SearchText.Split(' ');
      bool flag2 = true;
      MyBlueprintTypeEnum selectedBlueprintType = this.GetSelectedBlueprintType();
      MyBlueprintTypeEnum type = ((MyBlueprintItemInfo) button.UserData).Type;
      if (selectedBlueprintType != MyBlueprintTypeEnum.MIXED && selectedBlueprintType != type || !MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE && type == MyBlueprintTypeEnum.LOCAL)
        flag2 = false;
      if (flag2 & flag1)
      {
        string lower = button.Title.ToString().ToLower();
        foreach (string str in strArray)
        {
          if (!lower.Contains(str.ToLower()))
          {
            flag2 = false;
            break;
          }
        }
      }
      if (flag2)
        button.Visible = true;
      else
        button.Visible = false;
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
      foreach (MyGuiControlBase control in this.m_BPList.Controls)
      {
        if (control is MyGuiControlContentButton controlContentButton)
        {
          bool flag2 = true;
          MyBlueprintTypeEnum selectedBlueprintType = this.GetSelectedBlueprintType();
          MyBlueprintItemInfo userData = (MyBlueprintItemInfo) controlContentButton.UserData;
          MyBlueprintTypeEnum type = userData.Type;
          if (selectedBlueprintType != MyBlueprintTypeEnum.MIXED && (selectedBlueprintType != type || selectedBlueprintType == MyBlueprintTypeEnum.WORKSHOP && this.m_workshopIndex != MyGameService.GetUGCIndex(userData.Item.ServiceName)) && (selectedBlueprintType != MyBlueprintTypeEnum.WORKSHOP || type != MyBlueprintTypeEnum.SHARED) || !MyPlatformGameSettings.BLUEPRINTS_SUPPORT_LOCAL_TYPE && type == MyBlueprintTypeEnum.LOCAL)
            flag2 = false;
          if (flag2 & flag1)
          {
            string lower = controlContentButton.Title.ToString().ToLower();
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
      this.m_BPList.SetScrollBarPage();
    }

    public void TakeScreenshotLocalBP(string name, MyGuiControlContentButton button)
    {
      if (MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.IsWaiting())
      {
        StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ScreenBeingTaken_Caption);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ScreenBeingTaken), messageCaption: messageCaption));
      }
      else
      {
        MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.SetData_TakeScreenshotLocal(button);
        MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, this.GetCurrentLocalDirectory(), name, MyBlueprintUtils.THUMB_IMAGE_NAME), false, true, false);
      }
    }

    public void TakeScreenshotCloud(
      string pathRel,
      string pathFull,
      MyGuiControlContentButton button)
    {
      if (MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.IsWaiting())
      {
        StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ScreenBeingTaken_Caption);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.ScreenBlueprintsRew_ScreenBeingTaken), messageCaption: messageCaption));
      }
      else
      {
        MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.SetData_TakeScreenshotCloud(pathRel, pathFull, button);
        MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), pathFull, false, true, false);
      }
    }

    public static void ScreenshotTaken(bool success, string filename)
    {
      if (!MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.IsWaiting())
        return;
      switch (MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.Option)
      {
        case MyGuiBlueprintScreen_Reworked.WaitForScreenshotOptions.TakeScreenshotLocal:
          if (success)
          {
            MyRenderProxy.UnloadTexture(filename);
            MyGuiControlContentButton usedButton = MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.UsedButton;
            if (usedButton != null)
            {
              usedButton.CreatePreview(filename);
              break;
            }
            break;
          }
          break;
        case MyGuiBlueprintScreen_Reworked.WaitForScreenshotOptions.CreateNewBlueprintCloud:
          if (success)
          {
            if (File.Exists(MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.PathFull))
            {
              int cloudFile = (int) MyBlueprintUtils.SaveToCloudFile(MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.PathFull, MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.PathRel);
            }
            MyGuiBlueprintScreen_Reworked.m_newScreenshotTaken = true;
            break;
          }
          break;
        case MyGuiBlueprintScreen_Reworked.WaitForScreenshotOptions.TakeScreenshotCloud:
          if (success)
          {
            if (File.Exists(MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.PathFull))
            {
              int cloudFile = (int) MyBlueprintUtils.SaveToCloudFile(MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.PathFull, MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.PathRel);
            }
            MyRenderProxy.UnloadTexture(filename);
            if (MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.UsedButton != null)
            {
              if (string.IsNullOrEmpty(MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.UsedButton.PreviewImagePath))
              {
                MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.UsedButton.CreatePreview(filename);
                break;
              }
              MyRenderProxy.UnloadTexture(MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.UsedButton.PreviewImagePath);
              break;
            }
            break;
          }
          break;
      }
      MyGuiBlueprintScreen_Reworked.m_waitingForScreenshot.Clear();
    }

    public override bool Update(bool hasFocus)
    {
      if (this.m_thumbnailImage.Visible)
        this.UpdateThumbnailPosition();
      if (MyGuiBlueprintScreen_Reworked.Task.IsComplete && MyGuiBlueprintScreen_Reworked.m_needsExtract)
      {
        this.GetWorkshopItemsSteam();
        MyGuiBlueprintScreen_Reworked.m_needsExtract = false;
        this.RefreshBlueprintList();
      }
      if (this.m_wasPublished)
      {
        this.m_wasPublished = false;
        this.RefreshBlueprintList(true);
      }
      if (MyGuiBlueprintScreen_Reworked.m_newScreenshotTaken)
      {
        MyGuiBlueprintScreen_Reworked.m_newScreenshotTaken = false;
        this.RefreshBlueprintList(true);
      }
      return base.Update(hasFocus);
    }

    private void UpdateThumbnailPosition()
    {
      Vector2 vector2 = MyGuiManager.MouseCursorPosition + new Vector2(0.02f, 0.055f) + this.m_thumbnailImage.Size;
      if ((double) vector2.X <= 1.0 && (double) vector2.Y <= 1.0)
        this.m_thumbnailImage.Position = MyGuiManager.MouseCursorPosition + 0.5f * this.m_thumbnailImage.Size + new Vector2(-0.48f, -0.445f);
      else
        this.m_thumbnailImage.Position = MyGuiManager.MouseCursorPosition + new Vector2((float) (0.5 * (double) this.m_thumbnailImage.Size.X - 0.479999989271164), (float) (-0.5 * (double) this.m_thumbnailImage.Size.Y - 0.469999998807907));
    }

    private void OnMouseOverItem(MyGuiControlRadioButton butt, bool isMouseOver)
    {
      if (this.GetThumbnailVisibility())
        return;
      if (!isMouseOver)
      {
        this.m_thumbnailImage.SetTexture();
        this.m_thumbnailImage.Visible = false;
      }
      else if (!(butt.UserData is MyBlueprintItemInfo userData))
      {
        this.m_thumbnailImage.SetTexture();
        this.m_thumbnailImage.Visible = false;
      }
      else
      {
        string imagePath = this.GetImagePath(userData);
        if (!File.Exists(imagePath))
          return;
        MyRenderProxy.PreloadTextures((IEnumerable<string>) new string[1]
        {
          imagePath
        }, TextureType.GUIWithoutPremultiplyAlpha);
        this.m_thumbnailImage.SetTexture(imagePath);
        if (!this.m_thumbnailImage.IsAnyTextureValid())
          return;
        this.m_thumbnailImage.Visible = true;
        this.UpdateThumbnailPosition();
      }
    }

    private void OnFocusedItem(MyGuiControlBase control, bool state)
    {
      if (!state)
        return;
      this.SelectButton(control as MyGuiControlContentButton, forceToTop: false, alwaysScroll: false);
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

    private void CheckAndSplitLongName()
    {
      if ((double) this.m_detailName.GetTextSize().X <= 0.400000005960464)
        return;
      float num1 = 5f;
      float num2 = 1.2f;
      float num3 = (float) (0.400000005960464 * (1.0 + 0.400000005960464 / (double) num2) - 0.100000001490116);
      while ((double) this.m_detailName.GetTextSize().X > (double) num3)
        this.m_detailName.Text = this.m_detailName.Text.Remove(this.m_detailName.Text.Length - 1);
      string text = this.m_detailName.Text;
      int num4 = this.m_detailName.Text.Length / 4 * 3;
      float length1 = (float) new string[2]
      {
        this.m_detailName.Text.Substring(0, num4 - 1),
        this.m_detailName.Text.Substring(num4 - 1)
      }[1].Length;
      float length2 = (float) this.m_detailName.Text.Length;
      do
      {
        length2 -= length1;
        this.m_detailName.Text = text.Insert((int) length2, "\n");
        string[] strArray = this.m_detailName.Text.Split('\n');
        float num5 = MyGuiManager.MeasureString(this.m_detailName.Font, new StringBuilder(strArray[0]), this.m_detailName.TextScaleWithLanguage).X / MyGuiManager.MeasureString(this.m_detailName.Font, new StringBuilder(strArray[1]), this.m_detailName.TextScaleWithLanguage).X;
        if ((double) num5 >= (double) num1 && (double) length1 < 0.0)
          length1 /= -2f;
        else if ((double) num5 <= (double) num2 && (double) length1 > 0.0)
          length1 /= -2f;
      }
      while ((double) this.m_detailName.GetTextSize().X > 0.400000005960464);
      if (!(this.m_detailName.Text.Substring((int) length2, 2) == "\n "))
        return;
      this.m_detailName.Text = this.m_detailName.Text.Remove((int) length2 + 1, 1);
    }

    public enum SortOption
    {
      None,
      Alphabetical,
      CreationDate,
      UpdateDate,
    }

    private enum WaitForScreenshotOptions
    {
      None,
      TakeScreenshotLocal,
      CreateNewBlueprintCloud,
      TakeScreenshotCloud,
    }

    private class MyWaitForScreenshotData
    {
      private bool m_isSet;

      public MyGuiBlueprintScreen_Reworked.WaitForScreenshotOptions Option { get; private set; }

      public MyGuiControlContentButton UsedButton { get; private set; }

      public MyObjectBuilder_Definitions Prefab { get; private set; }

      public string PathRel { get; private set; }

      public string PathFull { get; private set; }

      public MyWaitForScreenshotData() => this.Clear();

      public bool SetData_TakeScreenshotLocal(MyGuiControlContentButton button)
      {
        if (this.m_isSet)
          return false;
        this.m_isSet = true;
        this.Option = MyGuiBlueprintScreen_Reworked.WaitForScreenshotOptions.TakeScreenshotLocal;
        this.UsedButton = button;
        return true;
      }

      public bool SetData_CreateNewBlueprintCloud(string pathRel, string pathFull)
      {
        if (this.m_isSet)
          return false;
        this.m_isSet = true;
        this.Option = MyGuiBlueprintScreen_Reworked.WaitForScreenshotOptions.CreateNewBlueprintCloud;
        this.PathRel = pathRel;
        this.PathFull = pathFull;
        return true;
      }

      public bool SetData_TakeScreenshotCloud(
        string pathRel,
        string pathFull,
        MyGuiControlContentButton button)
      {
        if (this.m_isSet)
          return false;
        this.m_isSet = true;
        this.Option = MyGuiBlueprintScreen_Reworked.WaitForScreenshotOptions.TakeScreenshotCloud;
        this.PathRel = pathRel;
        this.PathFull = pathFull;
        this.UsedButton = button;
        return true;
      }

      public bool IsWaiting() => this.m_isSet;

      public void Clear()
      {
        this.m_isSet = false;
        this.Option = MyGuiBlueprintScreen_Reworked.WaitForScreenshotOptions.None;
        this.UsedButton = (MyGuiControlContentButton) null;
        this.PathRel = string.Empty;
        this.PathFull = string.Empty;
        this.UsedButton = (MyGuiControlContentButton) null;
      }
    }

    private enum ScrollTestResult
    {
      Ok,
      Higher,
      Lower,
    }

    private class CloudBlueprintRenamer
    {
      private readonly Action<CloudResult, string> m_onComplete;
      private string m_newFilePath;
      private int m_filesSaved;
      private int m_filesToSave = 2;

      public CloudBlueprintRenamer(
        MyObjectBuilder_Definitions blueprint,
        string localImagePath,
        string bpNewName,
        Action<CloudResult, string> onComplete)
      {
        this.m_onComplete = onComplete;
        this.StartWorkflow(blueprint, bpNewName, localImagePath);
      }

      private void StartWorkflow(
        MyObjectBuilder_Definitions m_blueprint,
        string bpNewName,
        string localImagePath)
      {
        this.m_newFilePath = Path.Combine(Path.Combine("Blueprints/cloud", bpNewName), MyBlueprintUtils.BLUEPRINT_LOCAL_NAME);
        string fileName1 = "Blueprints/cloud/" + m_blueprint.ShipBlueprints[0].Id.SubtypeId + "/" + MyBlueprintUtils.THUMB_IMAGE_NAME;
        this.m_newFilePath = this.m_newFilePath.Replace("\\", "/");
        string fileName2 = this.m_newFilePath.Replace("bp.sbc", "thumb.png");
        m_blueprint.ShipBlueprints[0].Id.SubtypeId = bpNewName;
        m_blueprint.ShipBlueprints[0].Id.SubtypeName = bpNewName;
        m_blueprint.ShipBlueprints[0].CubeGrids[0].DisplayName = bpNewName;
        MyBlueprintUtils.SaveToCloud(m_blueprint, this.m_newFilePath, true, (Action<string, CloudResult>) ((_, result) => this.RenameBlueprintStepFinsihed(result)));
        byte[] buffer = !File.Exists(localImagePath) ? MyGameService.LoadFromCloud(fileName1) : File.ReadAllBytes(localImagePath);
        if (buffer != null && buffer.Length > 10)
        {
          MyGameService.SaveToCloudAsync(fileName2, buffer, new Action<CloudResult>(this.RenameBlueprintStepFinsihed));
          MyGameService.DeleteFromCloud(fileName1);
        }
        else
          --this.m_filesToSave;
      }

      private void RenameBlueprintStepFinsihed(CloudResult result)
      {
        if (result != CloudResult.Ok)
        {
          Action<CloudResult, string> onComplete = this.m_onComplete;
          if (onComplete == null)
            return;
          onComplete(result, this.m_newFilePath);
        }
        else
        {
          ++this.m_filesSaved;
          if (this.m_filesSaved != this.m_filesToSave)
            return;
          Action<CloudResult, string> onComplete = this.m_onComplete;
          if (onComplete == null)
            return;
          onComplete(CloudResult.Ok, this.m_newFilePath);
        }
      }
    }

    protected sealed class ShareBlueprintRequest\u003C\u003ESystem_UInt64\u0023System_String\u0023System_String\u0023System_UInt64\u0023System_String : ICallSite<IMyEventOwner, ulong, string, string, ulong, string, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong workshopId,
        in string workshopServiceName,
        in string name,
        in ulong sendToId,
        in string senderName,
        in DBNull arg6)
      {
        MyGuiBlueprintScreen_Reworked.ShareBlueprintRequest(workshopId, workshopServiceName, name, sendToId, senderName);
      }
    }

    protected sealed class ShareBlueprintRequestClient\u003C\u003ESystem_UInt64\u0023System_String\u0023System_String\u0023System_UInt64\u0023System_String : ICallSite<IMyEventOwner, ulong, string, string, ulong, string, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in ulong workshopId,
        in string workshopServiceName,
        in string name,
        in ulong sendToId,
        in string senderName,
        in DBNull arg6)
      {
        MyGuiBlueprintScreen_Reworked.ShareBlueprintRequestClient(workshopId, workshopServiceName, name, sendToId, senderName);
      }
    }
  }
}
