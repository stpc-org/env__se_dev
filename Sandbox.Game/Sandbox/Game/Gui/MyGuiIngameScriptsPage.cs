// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiIngameScriptsPage
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [PreloadRequired]
  public class MyGuiIngameScriptsPage : MyGuiScreenDebugBase
  {
    public const string STEAM_THUMBNAIL_NAME = "Textures\\GUI\\Icons\\IngameProgrammingIcon.png";
    public const string DEFAULT_SCRIPT_NAME = "Script";
    public const string SCRIPTS_DIRECTORY = "IngameScripts";
    public const string SCRIPT_EXTENSION = ".cs";
    public const string WORKSHOP_SCRIPT_EXTENSION = ".bin";
    private static readonly Vector2 SCREEN_SIZE = new Vector2(0.37f, 1.2f);
    private static readonly float HIDDEN_PART_RIGHT = 0.04f;
    private static Task m_task;
    private static List<MyWorkshopItem> m_subscribedItemsList = new List<MyWorkshopItem>();
    private Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);
    private float m_textScale = 0.8f;
    private MyGuiControlButton m_createFromEditorButton;
    private MyGuiControlButton m_okButton;
    private MyGuiControlButton m_detailsButton;
    private MyGuiControlButton m_deleteButton;
    private MyGuiControlButton m_replaceButton;
    private MyGuiControlTextbox m_searchBox;
    private MyGuiControlButton m_searchClear;
    private static MyGuiControlListbox m_scriptList = new MyGuiControlListbox(visualStyle: MyGuiControlListboxStyleEnum.IngameScipts);
    private MyGuiDetailScreenScriptLocal m_detailScreen;
    private bool m_activeDetail;
    private MyGuiControlListbox.Item m_selectedItem;
    private MyGuiControlRotatingWheel m_wheel;
    private string m_localScriptFolder;
    private string m_workshopFolder;
    private Action OnClose;
    private Action<string> OnScriptOpened;
    private Func<string> GetCodeFromEditor;

    public override string GetFriendlyName() => "MyIngameScriptScreen";

    public MyGuiIngameScriptsPage(
      Action<string> onScriptOpened,
      Func<string> getCodeFromEditor,
      Action close)
      : base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - MyGuiIngameScriptsPage.SCREEN_SIZE.X * 0.5f + MyGuiIngameScriptsPage.HIDDEN_PART_RIGHT, 0.5f), new Vector2?(MyGuiIngameScriptsPage.SCREEN_SIZE), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), false)
    {
      this.EnabledBackgroundFade = true;
      this.OnClose = close;
      this.GetCodeFromEditor = getCodeFromEditor;
      this.OnScriptOpened = onScriptOpened;
      this.m_localScriptFolder = Path.Combine(MyFileSystem.UserDataPath, "IngameScripts", "local");
      this.m_workshopFolder = Path.Combine(MyFileSystem.UserDataPath, "IngameScripts", "workshop");
      if (!Directory.Exists(this.m_localScriptFolder))
        Directory.CreateDirectory(this.m_localScriptFolder);
      if (!Directory.Exists(this.m_workshopFolder))
        Directory.CreateDirectory(this.m_workshopFolder);
      MyGuiIngameScriptsPage.m_scriptList.Items.Clear();
      this.GetLocalScriptNames(MyGuiIngameScriptsPage.m_subscribedItemsList.Count == 0);
      this.RecreateControls(true);
      MyGuiIngameScriptsPage.m_scriptList.ItemsSelected += new Action<MyGuiControlListbox>(this.OnSelectItem);
      MyGuiIngameScriptsPage.m_scriptList.ItemDoubleClicked += new Action<MyGuiControlListbox>(this.OnItemDoubleClick);
      this.OnEnterCallback = this.OnEnterCallback + new Action(this.Ok);
      this.m_canShareInput = false;
      this.CanBeHidden = true;
      this.CanHideOthers = false;
      this.m_canCloseInCloseAllScreenCalls = true;
      this.m_isTopScreen = false;
      this.m_isTopMostScreen = false;
      this.m_searchBox.TextChanged += new Action<MyGuiControlTextbox>(this.OnSearchTextChange);
    }

    private void CreateButtons()
    {
      Vector2 vector2_1 = new Vector2(-0.083f, 0.15f);
      Vector2 vector2_2 = new Vector2(0.134f, 0.038f);
      float num1 = 0.131f;
      float usableWidth = 0.265f;
      double num2 = (double) num1;
      StringBuilder text1 = MyTexts.Get(MyCommonTexts.Ok);
      Action<MyGuiControlButton> onClick1 = new Action<MyGuiControlButton>(this.OnOk);
      float textScale1 = this.m_textScale;
      MyStringId? tooltip1 = new MyStringId?(MyCommonTexts.Scripts_NoSelectedScript);
      double num3 = (double) textScale1;
      this.m_okButton = this.CreateButton((float) num2, text1, onClick1, false, tooltip1, (float) num3);
      this.m_okButton.Position = vector2_1;
      this.m_okButton.ShowTooltipWhenDisabled = true;
      double num4 = (double) num1;
      StringBuilder text2 = MyTexts.Get(MySpaceTexts.ProgrammableBlock_ButtonDetails);
      Action<MyGuiControlButton> onClick2 = new Action<MyGuiControlButton>(this.OnDetails);
      float textScale2 = this.m_textScale;
      MyStringId? tooltip2 = new MyStringId?(MyCommonTexts.Scripts_NoSelectedScript);
      double num5 = (double) textScale2;
      this.m_detailsButton = this.CreateButton((float) num4, text2, onClick2, false, tooltip2, (float) num5);
      this.m_detailsButton.Position = vector2_1 + new Vector2(1f, 0.0f) * vector2_2;
      this.m_detailsButton.ShowTooltipWhenDisabled = true;
      double num6 = (double) usableWidth;
      StringBuilder text3 = MyTexts.Get(MySpaceTexts.ProgrammableBlock_ButtonReplaceFromEditor);
      Action<MyGuiControlButton> onClick3 = new Action<MyGuiControlButton>(this.OnReplaceFromEditor);
      float textScale3 = this.m_textScale;
      MyStringId? tooltip3 = new MyStringId?(MyCommonTexts.Scripts_NoSelectedScript);
      double num7 = (double) textScale3;
      this.m_replaceButton = this.CreateButton((float) num6, text3, onClick3, false, tooltip3, (float) num7);
      this.m_replaceButton.Position = vector2_1 + new Vector2(0.0f, 1f) * vector2_2;
      this.m_replaceButton.PositionX += vector2_2.X / 2f;
      this.m_replaceButton.ShowTooltipWhenDisabled = true;
      double num8 = (double) usableWidth;
      StringBuilder text4 = MyTexts.Get(MyCommonTexts.LoadScreenButtonDelete);
      Action<MyGuiControlButton> onClick4 = new Action<MyGuiControlButton>(this.OnDelete);
      float textScale4 = this.m_textScale;
      MyStringId? tooltip4 = new MyStringId?(MyCommonTexts.Scripts_NoSelectedScript);
      double num9 = (double) textScale4;
      this.m_deleteButton = this.CreateButton((float) num8, text4, onClick4, false, tooltip4, (float) num9);
      this.m_deleteButton.Position = vector2_1 + new Vector2(0.0f, 2f) * vector2_2;
      this.m_deleteButton.PositionX += vector2_2.X / 2f;
      this.m_deleteButton.ShowTooltipWhenDisabled = true;
      vector2_1 = new Vector2(-0.083f, 0.305f);
      double num10 = (double) usableWidth;
      StringBuilder text5 = MyTexts.Get(MySpaceTexts.ProgrammableBlock_ButtonCreateFromEditor);
      Action<MyGuiControlButton> onClick5 = new Action<MyGuiControlButton>(this.OnCreateFromEditor);
      float textScale5 = this.m_textScale;
      MyStringId? tooltip5 = new MyStringId?(MyCommonTexts.Scripts_NewFromEditorTooltip);
      double num11 = (double) textScale5;
      MyGuiControlButton button1 = this.CreateButton((float) num10, text5, onClick5, tooltip: tooltip5, textScale: ((float) num11));
      button1.ShowTooltipWhenDisabled = true;
      button1.Position = vector2_1 + new Vector2(0.0f, 0.0f) * vector2_2;
      button1.PositionX += vector2_2.X / 2f;
      double num12 = (double) usableWidth;
      StringBuilder text6 = MyTexts.Get(MySpaceTexts.DetailScreen_Button_OpenInWorkshop);
      Action<MyGuiControlButton> onClick6 = new Action<MyGuiControlButton>(this.OnOpenWorkshop);
      float textScale6 = this.m_textScale;
      MyStringId? tooltip6 = new MyStringId?(MyCommonTexts.ScreenLoadSubscribedWorldBrowseWorkshop);
      double num13 = (double) textScale6;
      MyGuiControlButton button2 = this.CreateButton((float) num12, text6, onClick6, tooltip: tooltip6, textScale: ((float) num13));
      button2.Position = vector2_1 + new Vector2(0.0f, 1f) * vector2_2;
      button2.PositionX += vector2_2.X / 2f;
      double num14 = (double) usableWidth;
      StringBuilder text7 = MyTexts.Get(MySpaceTexts.ProgrammableBlock_ButtonRefreshScripts);
      Action<MyGuiControlButton> onClick7 = new Action<MyGuiControlButton>(this.OnReload);
      float textScale7 = this.m_textScale;
      MyStringId? tooltip7 = new MyStringId?(MyCommonTexts.Scripts_RefreshTooltip);
      double num15 = (double) textScale7;
      MyGuiControlButton button3 = this.CreateButton((float) num14, text7, onClick7, tooltip: tooltip7, textScale: ((float) num15));
      button3.Position = vector2_1 + new Vector2(0.0f, 2f) * vector2_2;
      button3.PositionX += vector2_2.X / 2f;
      MyGuiControlButton button4 = this.CreateButton(usableWidth, MyTexts.Get(MyCommonTexts.Close), new Action<MyGuiControlButton>(this.OnCancel), tooltip: new MyStringId?(MySpaceTexts.ToolTipNewsletter_Close), textScale: this.m_textScale);
      button4.Position = vector2_1 + new Vector2(0.0f, 3f) * vector2_2;
      button4.PositionX += vector2_2.X / 2f;
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      Vector2 vector2 = new Vector2(0.02f, MyGuiIngameScriptsPage.SCREEN_SIZE.Y - 1.076f);
      float num = (float) (((double) MyGuiIngameScriptsPage.SCREEN_SIZE.Y - 1.0) / 2.0);
      this.AddCaption(MyTexts.Get(MySpaceTexts.ProgrammableBlock_ScriptsScreenTitle).ToString(), new Vector4?(Color.White.ToVector4()), new Vector2?(this.m_controlPadding + new Vector2(-MyGuiIngameScriptsPage.HIDDEN_PART_RIGHT, num - 0.03f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), 0.44f), this.m_size.Value.X * 0.73f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), -0.123f), this.m_size.Value.X * 0.73f);
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), -0.278f), this.m_size.Value.X * 0.73f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      MyGuiControlLabel myGuiControlLabel1 = this.MakeLabel(MyTexts.GetString(MyCommonTexts.ScreenCubeBuilderBlockSearch), vector2 + new Vector2(-0.129f, -0.015f), this.m_textScale);
      myGuiControlLabel1.Position = new Vector2(-0.15f, -0.406f);
      this.m_searchBox = new MyGuiControlTextbox();
      this.m_searchBox.Position = new Vector2(0.115f, -0.401f);
      this.m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
      this.m_searchBox.Size = new Vector2(0.257f - myGuiControlLabel1.Size.X, 0.2f);
      this.m_searchBox.SetToolTip(MyCommonTexts.Scripts_SearchTooltip);
      MyGuiControlButton guiControlButton = new MyGuiControlButton();
      guiControlButton.Position = vector2 + new Vector2(0.068f, -0.521f);
      guiControlButton.Size = new Vector2(0.045f, 0.05666667f);
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Close;
      guiControlButton.ActivateOnMouseRelease = true;
      this.m_searchClear = guiControlButton;
      this.m_searchClear.ButtonClicked += new Action<MyGuiControlButton>(this.OnSearchClear);
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
      myGuiControlLabel2.Position = new Vector2(-0.145f, -0.357f);
      myGuiControlLabel2.Name = "ControlLabel";
      myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      myGuiControlLabel2.Text = MyTexts.GetString(MyCommonTexts.Scripts_ListOfScripts);
      MyGuiControlLabel myGuiControlLabel3 = myGuiControlLabel2;
      MyGuiControlPanel myGuiControlPanel1 = new MyGuiControlPanel(new Vector2?(new Vector2(-0.1535f, -0.362f)), new Vector2?(new Vector2(0.2685f, 0.035f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel1.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      MyGuiControlPanel myGuiControlPanel2 = myGuiControlPanel1;
      MyGuiControlListbox scriptList = MyGuiIngameScriptsPage.m_scriptList;
      scriptList.Size = scriptList.Size - new Vector2(0.5f, 0.0f);
      MyGuiIngameScriptsPage.m_scriptList.Position = new Vector2(-0.019f, -0.115f);
      MyGuiIngameScriptsPage.m_scriptList.VisibleRowsCount = 12;
      MyGuiIngameScriptsPage.m_scriptList.MultiSelect = false;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.Controls.Add((MyGuiControlBase) this.m_searchBox);
      this.Controls.Add((MyGuiControlBase) this.m_searchClear);
      this.Controls.Add((MyGuiControlBase) MyGuiIngameScriptsPage.m_scriptList);
      this.Controls.Add((MyGuiControlBase) myGuiControlPanel2);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
      this.CreateButtons();
      string texture = "Textures\\GUI\\screens\\screen_loading_wheel.dds";
      this.m_wheel = new MyGuiControlRotatingWheel(new Vector2?(new Vector2(-0.02f, -0.1f)), new Vector4?(MyGuiConstants.ROTATING_WHEEL_COLOR), 0.28f, texture: texture, multipleSpinningWheels: MyPerGameSettings.GUI.MultipleSpinningWheels);
      this.Controls.Add((MyGuiControlBase) this.m_wheel);
      this.m_wheel.Visible = false;
    }

    private void GetLocalScriptNames(bool reload = false)
    {
      if (!Directory.Exists(this.m_localScriptFolder))
        return;
      foreach (string directory in Directory.GetDirectories(this.m_localScriptFolder))
      {
        string fileName = Path.GetFileName(directory);
        MyBlueprintItemInfo blueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.LOCAL);
        blueprintItemInfo.SetAdditionalBlueprintInformation(fileName);
        StringBuilder text = new StringBuilder(fileName);
        string toolTip = fileName;
        object obj1 = (object) blueprintItemInfo;
        string normal = MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL.Normal;
        object userData = obj1;
        MyGuiControlListbox.Item obj2 = new MyGuiControlListbox.Item(text, toolTip, normal, userData);
        MyGuiIngameScriptsPage.m_scriptList.Add(obj2);
      }
      if (MyGuiIngameScriptsPage.m_task.IsComplete & reload)
        this.GetWorkshopScripts();
      else
        MyGuiIngameScriptsPage.AddWorkshopItemsToList();
    }

    private static void AddWorkshopItemsToList()
    {
      foreach (MyWorkshopItem subscribedItems in MyGuiIngameScriptsPage.m_subscribedItemsList)
      {
        MyBlueprintItemInfo blueprintItemInfo = new MyBlueprintItemInfo(MyBlueprintTypeEnum.WORKSHOP)
        {
          Item = subscribedItems
        };
        blueprintItemInfo.SetAdditionalBlueprintInformation(subscribedItems.Title, subscribedItems.Description, subscribedItems.DLCs.ToArray<uint>());
        StringBuilder text = new StringBuilder(subscribedItems.Title);
        string title = subscribedItems.Title;
        object obj1 = (object) blueprintItemInfo;
        string normal = MyGuiConstants.GetWorkshopIcon(subscribedItems).Normal;
        object userData = obj1;
        MyGuiControlListbox.Item obj2 = new MyGuiControlListbox.Item(text, title, normal, userData);
        MyGuiIngameScriptsPage.m_scriptList.Add(obj2);
      }
    }

    private void GetScriptsInfo()
    {
      MyGuiIngameScriptsPage.m_subscribedItemsList.Clear();
      (MyGameServiceCallResult, string) ingameScriptsBlocking = MyWorkshop.GetSubscribedIngameScriptsBlocking(MyGuiIngameScriptsPage.m_subscribedItemsList);
      if (Directory.Exists(this.m_workshopFolder))
      {
        try
        {
          Directory.Delete(this.m_workshopFolder, true);
        }
        catch (IOException ex)
        {
        }
      }
      Directory.CreateDirectory(this.m_workshopFolder);
      MyGuiIngameScriptsPage.AddWorkshopItemsToList();
      if (ingameScriptsBlocking.Item1 == MyGameServiceCallResult.OK)
        return;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(MyWorkshop.GetWorkshopErrorText(ingameScriptsBlocking.Item1, ingameScriptsBlocking.Item2, true)), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
    }

    private void GetWorkshopScripts() => MyGuiIngameScriptsPage.m_task = Parallel.Start(new Action(this.GetScriptsInfo));

    public void RefreshBlueprintList(bool fromTask = false)
    {
      MyGuiIngameScriptsPage.m_scriptList.Items.Clear();
      this.GetLocalScriptNames(fromTask);
    }

    public void RefreshAndReloadScriptsList(bool refreshWorkshopList = false)
    {
      MyGuiIngameScriptsPage.m_scriptList.Items.Clear();
      this.GetLocalScriptNames(refreshWorkshopList);
    }

    private void OnSearchClear(MyGuiControlButton button) => this.m_searchBox.Text = "";

    private void OnSelectItem(MyGuiControlListbox list)
    {
      if (list.SelectedItems.Count == 0)
        return;
      this.m_selectedItem = list.SelectedItems[0];
      this.m_detailsButton.Enabled = true;
      switch ((this.m_selectedItem.UserData as MyBlueprintItemInfo).Type)
      {
        case MyBlueprintTypeEnum.WORKSHOP:
          this.m_okButton.Enabled = true;
          this.m_detailsButton.Enabled = true;
          this.m_deleteButton.Enabled = false;
          this.m_replaceButton.Enabled = false;
          this.m_deleteButton.SetToolTip(MyTexts.GetString(MyCommonTexts.Scripts_LocalScriptsOnly));
          this.m_replaceButton.SetToolTip(MyTexts.GetString(MyCommonTexts.Scripts_LocalScriptsOnly));
          break;
        case MyBlueprintTypeEnum.LOCAL:
          this.m_okButton.Enabled = true;
          this.m_detailsButton.Enabled = true;
          this.m_replaceButton.Enabled = true;
          this.m_deleteButton.Enabled = true;
          this.m_okButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_OkTooltip));
          this.m_detailsButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_DetailsTooltip));
          this.m_replaceButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_ReplaceTooltip));
          this.m_deleteButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_DeleteTooltip));
          break;
        case MyBlueprintTypeEnum.SHARED:
          this.m_detailsButton.Enabled = false;
          this.m_deleteButton.Enabled = false;
          break;
      }
    }

    private bool ValidateSelecteditem() => this.m_selectedItem != null && this.m_selectedItem.UserData != null && this.m_selectedItem.Text != null;

    private void OnSearchTextChange(MyGuiControlTextbox box)
    {
      if (box.Text != "")
      {
        string[] strArray = box.Text.Split(' ');
        foreach (MyGuiControlListbox.Item obj in MyGuiIngameScriptsPage.m_scriptList.Items)
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
        foreach (MyGuiControlListbox.Item obj in MyGuiIngameScriptsPage.m_scriptList.Items)
          obj.Visible = true;
      }
    }

    private void OpenSharedScript(MyBlueprintItemInfo itemInfo)
    {
      MyGuiIngameScriptsPage.m_scriptList.Enabled = false;
      MyGuiIngameScriptsPage.m_task = Parallel.Start(new Action(this.DownloadScriptFromSteam), new Action(this.OnScriptDownloaded));
    }

    private void DownloadScriptFromSteam()
    {
      if (this.m_selectedItem == null)
        return;
      MyWorkshop.DownloadScriptBlocking((this.m_selectedItem.UserData as MyBlueprintItemInfo).Item);
    }

    private void OnScriptDownloaded()
    {
      if (this.OnScriptOpened != null && this.m_selectedItem != null)
        this.OnScriptOpened((this.m_selectedItem.UserData as MyBlueprintItemInfo).Item.Folder);
      MyGuiIngameScriptsPage.m_scriptList.Enabled = true;
    }

    private void OnItemDoubleClick(MyGuiControlListbox list)
    {
      this.m_selectedItem = list.SelectedItems[0];
      object userData = this.m_selectedItem.UserData;
      this.OpenSelectedSript();
    }

    private void Ok()
    {
      if (this.m_selectedItem == null)
        this.CloseScreen(false);
      else
        this.OpenSelectedSript();
    }

    private void OpenSelectedSript()
    {
      MyBlueprintItemInfo userData = this.m_selectedItem.UserData as MyBlueprintItemInfo;
      if (userData.Type == MyBlueprintTypeEnum.WORKSHOP)
        this.OpenSharedScript(userData);
      else if (this.OnScriptOpened != null)
        this.OnScriptOpened(Path.Combine(MyFileSystem.UserDataPath, "IngameScripts", "local", userData.Data.Name, "Script.cs"));
      this.CloseScreen(false);
    }

    private void OnOk(MyGuiControlButton button) => this.Ok();

    private void OnCancel(MyGuiControlButton button) => this.CloseScreen(false);

    private void OnReload(MyGuiControlButton button)
    {
      this.m_selectedItem = (MyGuiControlListbox.Item) null;
      this.m_okButton.Enabled = false;
      this.m_detailsButton.Enabled = false;
      this.m_deleteButton.Enabled = false;
      this.m_replaceButton.Enabled = false;
      this.m_okButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
      this.m_detailsButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
      this.m_replaceButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
      this.m_deleteButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
      this.RefreshAndReloadScriptsList(true);
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
        if (this.m_activeDetail)
          return;
        if ((this.m_selectedItem.UserData as MyBlueprintItemInfo).Type == MyBlueprintTypeEnum.LOCAL)
        {
          if (Directory.Exists(Path.Combine(this.m_localScriptFolder, this.m_selectedItem.Text.ToString())))
          {
            this.m_detailScreen = new MyGuiDetailScreenScriptLocal((Action<MyBlueprintItemInfo>) (item =>
            {
              if (item == null)
              {
                this.m_okButton.Enabled = false;
                this.m_detailsButton.Enabled = false;
                this.m_deleteButton.Enabled = false;
                this.m_replaceButton.Enabled = false;
              }
              this.m_activeDetail = false;
              if (!MyGuiIngameScriptsPage.m_task.IsComplete)
                return;
              this.RefreshBlueprintList(this.m_detailScreen.WasPublished);
            }), this.m_selectedItem.UserData as MyBlueprintItemInfo, this, this.m_textScale);
            this.m_activeDetail = true;
            MyScreenManager.InputToNonFocusedScreens = true;
            MyScreenManager.AddScreen((MyGuiScreenBase) this.m_detailScreen);
          }
          else
          {
            StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_ScriptNotFound), messageCaption: messageCaption));
          }
        }
        else
        {
          if ((this.m_selectedItem.UserData as MyBlueprintItemInfo).Type != MyBlueprintTypeEnum.WORKSHOP)
            return;
          this.m_detailScreen = new MyGuiDetailScreenScriptLocal((Action<MyBlueprintItemInfo>) (item =>
          {
            this.m_activeDetail = false;
            if (!MyGuiIngameScriptsPage.m_task.IsComplete)
              return;
            this.RefreshBlueprintList();
          }), this.m_selectedItem.UserData as MyBlueprintItemInfo, this, this.m_textScale);
          this.m_activeDetail = true;
          MyScreenManager.InputToNonFocusedScreens = true;
          MyScreenManager.AddScreen((MyGuiScreenBase) this.m_detailScreen);
        }
      }
    }

    private void OnRename(MyGuiControlButton button)
    {
      if (this.m_selectedItem == null)
        return;
      Vector2 position = new Vector2(0.5f, 0.5f);
      Action<string> callBack = (Action<string>) (result =>
      {
        if (result == null)
          return;
        this.ChangeName(result);
      });
      string str = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_NewScriptName);
      string defaultName = this.m_selectedItem.Text.ToString();
      string caption = str;
      MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiBlueprintTextDialog(position, callBack, defaultName, caption, 50, 0.3f));
    }

    public void ChangeName(string newName)
    {
      newName = MyUtils.StripInvalidChars(newName);
      string oldName = this.m_selectedItem.Text.ToString();
      string path = Path.Combine(this.m_localScriptFolder, oldName);
      string newFile = Path.Combine(this.m_localScriptFolder, newName);
      if (path == newFile || !Directory.Exists(path))
        return;
      if (Directory.Exists(newFile))
      {
        if (path.ToLower() == newFile.ToLower())
        {
          this.RenameScript(oldName, newName);
          this.RefreshAndReloadScriptsList();
        }
        else
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendFormat(MySpaceTexts.ProgrammableBlock_ReplaceScriptNameDialogText, (object) newName);
          StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptNameDialogTitle);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, stringBuilder, messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
          {
            if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
              return;
            Directory.Delete(newFile, true);
            this.RenameScript(oldName, newName);
            this.RefreshAndReloadScriptsList();
          }))));
        }
      }
      else
      {
        try
        {
          this.RenameScript(oldName, newName);
          this.RefreshAndReloadScriptsList();
        }
        catch (IOException ex)
        {
          StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.LoadScreenButtonDelete);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptNameUsed), messageCaption: messageCaption));
        }
      }
    }

    public void OnDelete(MyGuiControlButton button)
    {
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.LoadScreenButtonDelete);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.ProgrammableBlock_DeleteScriptDialogText), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
      {
        if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES || this.m_selectedItem == null)
          return;
        if (this.DeleteScript(this.m_selectedItem.Text.ToString()))
        {
          this.m_okButton.Enabled = false;
          this.m_detailsButton.Enabled = false;
          this.m_deleteButton.Enabled = false;
          this.m_replaceButton.Enabled = false;
          this.m_okButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
          this.m_detailsButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
          this.m_replaceButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
          this.m_deleteButton.SetTooltip(MyTexts.GetString(MyCommonTexts.Scripts_NoSelectedScript));
          this.m_selectedItem = (MyGuiControlListbox.Item) null;
        }
        this.RefreshBlueprintList();
      }))));
    }

    private void RenameScript(string oldName, string newName)
    {
      string str = Path.Combine(this.m_localScriptFolder, oldName);
      if (Directory.Exists(str))
      {
        string destDirName = Path.Combine(this.m_localScriptFolder, newName);
        Directory.Move(str, destDirName);
      }
      this.DeleteScript(oldName);
    }

    private bool DeleteScript(string p)
    {
      string path = Path.Combine(this.m_localScriptFolder, p);
      if (!Directory.Exists(path))
        return false;
      Directory.Delete(path, true);
      return true;
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
      if (!MyGuiIngameScriptsPage.m_task.IsComplete)
        this.m_wheel.Visible = true;
      if (MyGuiIngameScriptsPage.m_task.IsComplete)
        this.m_wheel.Visible = false;
      return base.Update(hasFocus);
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      if (this.OnClose != null)
        this.OnClose();
      return base.CloseScreen(isUnloading);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (!MyInput.Static.IsNewKeyPressed(MyKeys.F12) && !MyInput.Static.IsNewKeyPressed(MyKeys.F11) && !MyInput.Static.IsNewKeyPressed(MyKeys.F10))
        return;
      this.CloseScreen(false);
    }

    public void OnCreateFromEditor(MyGuiControlButton button)
    {
      if (this.GetCodeFromEditor == null || !Directory.Exists(this.m_localScriptFolder))
        return;
      int num = 0;
      while (Directory.Exists(Path.Combine(this.m_localScriptFolder, "Script_" + num.ToString())))
        ++num;
      string str = Path.Combine(this.m_localScriptFolder, "Script_" + (object) num);
      Directory.CreateDirectory(str);
      File.Copy(Path.Combine(MyFileSystem.ContentPath, "Textures\\GUI\\Icons\\IngameProgrammingIcon.png"), Path.Combine(str, MyBlueprintUtils.THUMB_IMAGE_NAME), true);
      string contents = this.GetCodeFromEditor();
      File.WriteAllText(Path.Combine(str, "Script.cs"), contents, Encoding.UTF8);
      this.RefreshAndReloadScriptsList();
    }

    public void OnReplaceFromEditor(MyGuiControlButton button)
    {
      if (this.m_selectedItem == null || this.GetCodeFromEditor == null || !Directory.Exists(this.m_localScriptFolder))
        return;
      StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptNameDialogTitle);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.ProgrammableBlock_ReplaceScriptDialogText), messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (callbackReturn =>
      {
        if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        string path = Path.Combine(this.m_localScriptFolder, (this.m_selectedItem.UserData as MyBlueprintItemInfo).Data.Name, "Script.cs");
        if (!File.Exists(path))
          return;
        string contents = this.GetCodeFromEditor();
        File.WriteAllText(path, contents, Encoding.UTF8);
      }))));
    }

    private void OnOpenWorkshop(MyGuiControlButton button) => MyWorkshop.OpenWorkshopBrowser(MySteamConstants.TAG_SCRIPTS);

    protected MyGuiControlButton CreateButton(
      float usableWidth,
      StringBuilder text,
      Action<MyGuiControlButton> onClick,
      bool enabled = true,
      MyStringId? tooltip = null,
      float textScale = 1f)
    {
      MyGuiControlButton guiControlButton = this.AddButton(text, onClick);
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
      guiControlButton.TextScale = textScale;
      guiControlButton.Size = new Vector2(usableWidth, guiControlButton.Size.Y);
      guiControlButton.Position = guiControlButton.Position + new Vector2(-0.02f, 0.0f);
      guiControlButton.Enabled = enabled;
      if (tooltip.HasValue)
        guiControlButton.SetToolTip(tooltip.Value);
      return guiControlButton;
    }

    protected MyGuiControlLabel MakeLabel(
      string text,
      Vector2 position,
      float textScale = 1f)
    {
      string text1 = text;
      return new MyGuiControlLabel(new Vector2?(position), text: text1, textScale: textScale, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
    }
  }
}
