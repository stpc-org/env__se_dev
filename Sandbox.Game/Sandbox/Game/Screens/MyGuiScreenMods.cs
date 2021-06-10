// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenMods
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenMods : MyGuiScreenBase
  {
    private MyGuiControlTable m_modsTableEnabled;
    private MyGuiControlTable m_modsTableDisabled;
    private MyGuiControlButton m_moveUpButton;
    private MyGuiControlButton m_moveDownButton;
    private MyGuiControlButton m_moveTopButton;
    private MyGuiControlButton m_moveBottomButton;
    private MyGuiControlButton m_moveLeftButton;
    private MyGuiControlButton m_moveLeftAllButton;
    private MyGuiControlButton m_moveRightButton;
    private MyGuiControlButton m_moveRightAllButton;
    private MyGuiControlButton m_openInWorkshopButton;
    private MyGuiControlButton m_refreshButton;
    private MyGuiControlButton m_browseWorkshopButton;
    private MyGuiControlButton m_publishModButton;
    private MyGuiControlButton m_okButton;
    private MyGuiControlTable.Row m_selectedRow;
    private MyGuiControlTable m_selectedTable;
    private bool m_listNeedsReload;
    private bool m_keepActiveMods;
    private List<MyWorkshopItem> m_subscribedMods;
    private List<MyWorkshopItem> m_worldMods;
    private List<MyObjectBuilder_Checkpoint.ModItem> m_modListToEdit;
    private MyGuiControlLabel m_workshopError;
    private bool m_workshopPermitted;
    private bool m_refreshWhenInFocusNext;
    private MyObjectBuilder_Checkpoint.ModItem m_selectedMod;
    private HashSet<string> m_worldLocalMods = new HashSet<string>();
    private HashSet<WorkshopId> m_worldWorkshopMods = new HashSet<WorkshopId>();
    private List<MyGuiControlButton> m_categoryButtonList = new List<MyGuiControlButton>();
    private MyGuiControlSearchBox m_searchBox;
    private MyGuiControlButton m_searchClear;
    private List<string> m_tmpSearch = new List<string>();
    private List<string> m_selectedCategories = new List<string>();
    private List<string> m_selectedServiceNames = new List<string>();
    private Dictionary<ulong, StringBuilder> m_modsToolTips = new Dictionary<ulong, StringBuilder>();
    private WorkshopId[] m_selectedModWorkshopIds;
    private int m_consentUpdateFrameTimer = 30;
    private int m_consentUpdateFrameTimer_current;

    public MyGuiScreenMods(
      List<MyObjectBuilder_Checkpoint.ModItem> modListToEdit)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(1.015f, 0.934f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_workshopPermitted = true;
      MyGameService.Service.RequestPermissions(Permissions.UGC, true, (Action<bool>) (x => this.m_workshopPermitted = x));
      this.m_modListToEdit = modListToEdit;
      if (this.m_modListToEdit == null)
      {
        this.m_modListToEdit = new List<MyObjectBuilder_Checkpoint.ModItem>();
      }
      else
      {
        for (int index = 0; index < this.m_modListToEdit.Count; ++index)
        {
          MyObjectBuilder_Checkpoint.ModItem modItem = this.m_modListToEdit[index];
          if (modItem.PublishedServiceName == null)
          {
            modItem.PublishedServiceName = MyGameService.GetDefaultUGC().ServiceName;
            this.m_modListToEdit[index] = modItem;
          }
        }
      }
      this.EnabledBackgroundFade = true;
      this.RefreshWorldMods((ListReader<MyObjectBuilder_Checkpoint.ModItem>) this.m_modListToEdit);
      this.m_listNeedsReload = true;
      this.RecreateControls(true);
    }

    private void RefreshWorldMods(
      ListReader<MyObjectBuilder_Checkpoint.ModItem> mods)
    {
      this.m_worldLocalMods.Clear();
      this.m_worldWorkshopMods.Clear();
      foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
      {
        if (mod.PublishedFileId == 0UL)
          this.m_worldLocalMods.Add(mod.Name);
        else
          this.m_worldWorkshopMods.Add(new WorkshopId(mod.PublishedFileId, mod.PublishedServiceName));
      }
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MyCommonTexts.ScreenCaptionWorkshop, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.894999980926514 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.895f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      Vector2 start = new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.894999980926514 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465));
      controlSeparatorList2.AddHorizontal(start, this.m_size.Value.X * 0.895f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      Vector2 vector2_1 = new Vector2(-0.454f, -0.417f);
      Vector2 vector2_2 = new Vector2(-0.0f, -4.75f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA.Y);
      this.m_modsTableDisabled = new MyGuiControlTable();
      this.m_modsTableEnabled = new MyGuiControlTable();
      if (MyFakes.ENABLE_MOD_CATEGORIES)
      {
        this.m_modsTableDisabled.Position = vector2_1 + new Vector2(0.0f, 0.1f);
        this.m_modsTableDisabled.VisibleRowsCount = 18;
        this.m_modsTableEnabled.VisibleRowsCount = 18;
      }
      else
      {
        this.m_modsTableDisabled.Position = vector2_1;
        this.m_modsTableDisabled.VisibleRowsCount = 20;
        this.m_modsTableEnabled.VisibleRowsCount = 20;
      }
      this.m_modsTableDisabled.Size = new Vector2(this.m_size.Value.X * 0.428f, 1f);
      this.m_modsTableDisabled.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_modsTableDisabled.ColumnsCount = 2;
      this.m_modsTableDisabled.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemSelected);
      this.m_modsTableDisabled.ItemDoubleClicked += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemConfirmedOrDoubleClick);
      this.m_modsTableDisabled.ItemConfirmed += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemConfirmedOrDoubleClick);
      float[] p = new float[2]{ 0.065f, 0.945f };
      this.m_modsTableDisabled.SetCustomColumnWidths(p);
      this.m_modsTableDisabled.SetColumnComparison(1, (Comparison<MyGuiControlTable.Cell>) ((a, b) => a.Text.CompareToIgnoreCase(b.Text)));
      this.Controls.Add((MyGuiControlBase) this.m_modsTableDisabled);
      this.m_modsTableEnabled.Position = vector2_1 + new Vector2(this.m_modsTableDisabled.Size.X + 0.04f, 0.1f);
      this.m_modsTableEnabled.Size = new Vector2(this.m_size.Value.X * 0.428f, 1f);
      this.m_modsTableEnabled.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_modsTableEnabled.ColumnsCount = 2;
      this.m_modsTableEnabled.ItemSelected += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemSelected);
      this.m_modsTableEnabled.ItemDoubleClicked += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemConfirmedOrDoubleClick);
      this.m_modsTableEnabled.ItemConfirmed += new Action<MyGuiControlTable, MyGuiControlTable.EventArgs>(this.OnTableItemConfirmedOrDoubleClick);
      this.m_modsTableEnabled.SetCustomColumnWidths(p);
      this.m_modsTableEnabled.SetColumnComparison(1, (Comparison<MyGuiControlTable.Cell>) ((a, b) => a.Text.CompareToIgnoreCase(b.Text)));
      this.Controls.Add((MyGuiControlBase) this.m_modsTableEnabled);
      this.Controls.Add((MyGuiControlBase) (this.m_moveRightAllButton = this.MakeButtonTiny(vector2_2 + 0.0f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 0.0f, MyCommonTexts.ToolTipScreenMods_MoveRightAll, MyGuiConstants.TEXTURE_BUTTON_ARROW_DOUBLE, new Action<MyGuiControlButton>(this.OnMoveRightAllClick))));
      this.Controls.Add((MyGuiControlBase) (this.m_moveRightButton = this.MakeButtonTiny(vector2_2 + 1f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 0.0f, MyCommonTexts.ToolTipScreenMods_MoveRight, MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE, new Action<MyGuiControlButton>(this.OnMoveRightClick))));
      this.Controls.Add((MyGuiControlBase) (this.m_moveUpButton = this.MakeButtonTiny(vector2_2 + 2.5f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, -1.570796f, MyCommonTexts.ToolTipScreenMods_MoveUp, MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE, new Action<MyGuiControlButton>(this.OnMoveUpClick))));
      this.Controls.Add((MyGuiControlBase) (this.m_moveTopButton = this.MakeButtonTiny(vector2_2 + 3.5f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, -1.570796f, MyCommonTexts.ToolTipScreenMods_MoveTop, MyGuiConstants.TEXTURE_BUTTON_ARROW_DOUBLE, new Action<MyGuiControlButton>(this.OnMoveTopClick))));
      this.Controls.Add((MyGuiControlBase) (this.m_moveBottomButton = this.MakeButtonTiny(vector2_2 + 4.5f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 1.570796f, MyCommonTexts.ToolTipScreenMods_MoveBottom, MyGuiConstants.TEXTURE_BUTTON_ARROW_DOUBLE, new Action<MyGuiControlButton>(this.OnMoveBottomClick))));
      this.Controls.Add((MyGuiControlBase) (this.m_moveDownButton = this.MakeButtonTiny(vector2_2 + 5.5f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 1.570796f, MyCommonTexts.ToolTipScreenMods_MoveDown, MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE, new Action<MyGuiControlButton>(this.OnMoveDownClick))));
      this.Controls.Add((MyGuiControlBase) (this.m_moveLeftButton = this.MakeButtonTiny(vector2_2 + 7f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 3.141593f, MyCommonTexts.ToolTipScreenMods_MoveLeft, MyGuiConstants.TEXTURE_BUTTON_ARROW_SINGLE, new Action<MyGuiControlButton>(this.OnMoveLeftClick))));
      this.Controls.Add((MyGuiControlBase) (this.m_moveLeftAllButton = this.MakeButtonTiny(vector2_2 + 8f * MyGuiConstants.MENU_BUTTONS_POSITION_DELTA, 3.141593f, MyCommonTexts.ToolTipScreenMods_MoveLeftAll, MyGuiConstants.TEXTURE_BUTTON_ARROW_DOUBLE, new Action<MyGuiControlButton>(this.OnMoveLeftAllClick))));
      float num = 0.0075f;
      this.m_okButton = this.MakeButton(new Vector2(this.m_modsTableDisabled.Position.X + 1f / 500f, 0.0f) - new Vector2(0.0f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0970000028610229)), MyCommonTexts.Ok, MyCommonTexts.Ok, new Action<MyGuiControlButton>(this.OnOkClick));
      this.m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipMods_Ok));
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      this.m_refreshButton = this.MakeButton(this.m_okButton.Position + new Vector2(this.m_okButton.Size.X + num, 0.0f), MyCommonTexts.ScreenLoadSubscribedWorldRefresh, MyCommonTexts.ToolTipWorkshopRefreshMod, new Action<MyGuiControlButton>(this.OnRefreshClick));
      this.m_refreshButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_refreshButton);
      this.m_browseWorkshopButton = this.MakeButton(this.m_okButton.Position + new Vector2((float) ((double) this.m_okButton.Size.X * 2.0 + (double) num * 2.0), 0.0f), MyCommonTexts.ScreenLoadSubscribedWorldBrowseWorkshop, MyCommonTexts.ToolTipWorkshopBrowseWorkshop, new Action<MyGuiControlButton>(this.OnBrowseWorkshopClick));
      this.m_browseWorkshopButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_browseWorkshopButton);
      this.m_openInWorkshopButton = this.MakeButton(this.m_okButton.Position + new Vector2((float) ((double) this.m_okButton.Size.X * 3.0 + (double) num * 3.0), 0.0f), MyCommonTexts.ScreenLoadSubscribedWorldOpenInWorkshop, MyCommonTexts.ToolTipWorkshopPublish, new Action<MyGuiControlButton>(this.OnOpenInWorkshopClick));
      this.Controls.Add((MyGuiControlBase) this.m_openInWorkshopButton);
      this.m_publishModButton = this.MakeButton(this.m_okButton.Position + new Vector2((float) ((double) this.m_okButton.Size.X * 4.0 + (double) num * 4.0), 0.0f), MyCommonTexts.LoadScreenButtonPublish, MyCommonTexts.LoadScreenButtonPublish, new Action<MyGuiControlButton>(this.OnPublishModClick));
      this.Controls.Add((MyGuiControlBase) this.m_publishModButton);
      this.m_workshopError = new MyGuiControlLabel(colorMask: new Vector4?((Vector4) Color.Red));
      this.m_workshopError.Position = new Vector2(-0.454f, this.m_okButton.Position.Y + 0.07f);
      this.m_workshopError.Visible = false;
      this.Controls.Add((MyGuiControlBase) this.m_workshopError);
      if (MyFakes.ENABLE_MOD_CATEGORIES)
      {
        Vector2 vector2_3 = this.m_modsTableDisabled.Position + new Vector2(0.015f, -0.036f);
        Vector2 vector2_4 = new Vector2(0.0335f, 0.0f);
        MyWorkshop.Category[] modCategories = MyWorkshop.ModCategories;
        for (int index = 0; index < modCategories.Length; ++index)
        {
          if (modCategories[index].IsVisibleForFilter)
            this.Controls.Add((MyGuiControlBase) this.MakeButtonCategory(vector2_3 + vector2_4 * (float) index, modCategories[index]));
        }
        this.m_searchBox = new MyGuiControlSearchBox(new Vector2?(new Vector2(this.m_modsTableEnabled.Position.X, 0.0f) - new Vector2(0.0f, (float) ((double) this.m_size.Value.Y / 2.0 - 0.0989999994635582)) + new Vector2(0.0f, 0.013f)));
        this.m_searchBox.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipMods_Search));
        this.m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
        this.m_searchBox.Size = new Vector2(this.m_modsTableEnabled.Size.X, 0.2f);
        this.m_searchBox.OnTextChanged += new MyGuiControlSearchBox.TextChangedDelegate(this.OnSearchTextChanged);
        Vector2 vector2_5 = new Vector2(0.0f, 0.05f);
        MyGuiControlButton moveUpButton = this.m_moveUpButton;
        moveUpButton.Position = moveUpButton.Position + vector2_5;
        MyGuiControlButton moveTopButton = this.m_moveTopButton;
        moveTopButton.Position = moveTopButton.Position + vector2_5;
        MyGuiControlButton moveBottomButton = this.m_moveBottomButton;
        moveBottomButton.Position = moveBottomButton.Position + vector2_5;
        MyGuiControlButton moveDownButton = this.m_moveDownButton;
        moveDownButton.Position = moveDownButton.Position + vector2_5;
        MyGuiControlButton moveLeftButton = this.m_moveLeftButton;
        moveLeftButton.Position = moveLeftButton.Position + vector2_5;
        MyGuiControlButton moveLeftAllButton = this.m_moveLeftAllButton;
        moveLeftAllButton.Position = moveLeftAllButton.Position + vector2_5;
        MyGuiControlButton moveRightAllButton = this.m_moveRightAllButton;
        moveRightAllButton.Position = moveRightAllButton.Position + vector2_5;
        MyGuiControlButton moveRightButton = this.m_moveRightButton;
        moveRightButton.Position = moveRightButton.Position + vector2_5;
        this.Controls.Add((MyGuiControlBase) this.m_searchBox);
      }
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(start.X, this.m_okButton.Position.Y + minSizeGui.Y / 2f)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.CloseButtonEnabled = true;
      if ((double) MySandboxGame.ScreenSize.X / (double) MySandboxGame.ScreenSize.Y == 1.25)
        this.SetCloseButtonOffset_5_to_4();
      else
        this.SetDefaultCloseButtonOffset();
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.ModsScreen_Help_Screen);
      if (!MyInput.Static.IsJoystickLastUsed)
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

    public override string GetFriendlyName() => nameof (MyGuiScreenMods);

    private MyGuiControlLabel MakeLabel(Vector2 position, string text) => new MyGuiControlLabel(new Vector2?(position), text: MyTexts.GetString(text), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);

    private MyGuiControlButton MakeButton(
      Vector2 position,
      MyStringId text,
      MyStringId toolTip,
      Action<MyGuiControlButton> onClick,
      MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
    {
      Vector2? position1 = new Vector2?(position);
      Vector2? size = new Vector2?();
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(text);
      string str = MyTexts.GetString(toolTip);
      Action<MyGuiControlButton> action = onClick;
      int num = (int) originAlign;
      string toolTip1 = str;
      StringBuilder text1 = stringBuilder;
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      return new MyGuiControlButton(position1, size: size, colorMask: colorMask, originAlign: ((MyGuiDrawAlignEnum) num), toolTip: toolTip1, text: text1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
    }

    private MyGuiControlButton MakeButton(
      Vector2 position,
      MyStringId text,
      string toolTip,
      Action<MyGuiControlButton> onClick,
      MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
    {
      Vector2? position1 = new Vector2?(position);
      Vector2? size = new Vector2?();
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(text);
      string str = toolTip;
      Action<MyGuiControlButton> action = onClick;
      int num = (int) originAlign;
      string toolTip1 = str;
      StringBuilder text1 = stringBuilder;
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      return new MyGuiControlButton(position1, size: size, colorMask: colorMask, originAlign: ((MyGuiDrawAlignEnum) num), toolTip: toolTip1, text: text1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
    }

    private MyGuiControlButton MakeButtonTiny(
      Vector2 position,
      float rotation,
      MyStringId toolTip,
      MyGuiHighlightTexture icon,
      Action<MyGuiControlButton> onClick,
      Vector2? size = null)
    {
      Vector2? position1 = new Vector2?(position);
      string str = MyTexts.GetString(toolTip);
      Action<MyGuiControlButton> action = onClick;
      Vector2? size1 = size;
      Vector4? colorMask = new Vector4?();
      string toolTip1 = str;
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      MyGuiControlButton guiControlButton = new MyGuiControlButton(position1, MyGuiControlButtonStyleEnum.Square, size1, colorMask, toolTip: toolTip1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
      icon.SizePx = new Vector2(64f, 64f);
      guiControlButton.Icon = new MyGuiHighlightTexture?(icon);
      guiControlButton.IconRotation = rotation;
      guiControlButton.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      return guiControlButton;
    }

    private MyGuiControlButton MakeButtonCategoryTiny(
      Vector2 position,
      float rotation,
      MyStringId toolTip,
      MyGuiHighlightTexture icon,
      Action<MyGuiControlButton> onClick,
      Vector2? size = null)
    {
      Vector2? position1 = new Vector2?(position);
      string str = MyTexts.GetString(toolTip);
      Action<MyGuiControlButton> action = onClick;
      Vector2? size1 = size;
      Vector4? colorMask = new Vector4?();
      string toolTip1 = str;
      Action<MyGuiControlButton> onButtonClick = action;
      int? buttonIndex = new int?();
      MyGuiControlButton guiControlButton = new MyGuiControlButton(position1, MyGuiControlButtonStyleEnum.Square48, size1, colorMask, toolTip: toolTip1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
      icon.SizePx = new Vector2(48f, 48f);
      guiControlButton.Icon = new MyGuiHighlightTexture?(icon);
      guiControlButton.IconRotation = rotation;
      guiControlButton.IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      return guiControlButton;
    }

    private MyGuiControlButton MakeButtonCategory(
      Vector2 position,
      MyWorkshop.Category category)
    {
      string str = category.Id.Replace(" ", "");
      MyGuiControlButton guiControlButton = this.MakeButtonCategoryTiny(position, 0.0f, category.LocalizableName, new MyGuiHighlightTexture()
      {
        Normal = string.Format("Textures\\GUI\\Icons\\buttons\\small_variant\\{0}.dds", (object) str),
        Highlight = string.Format("Textures\\GUI\\Icons\\buttons\\small_variant\\{0}.dds", (object) str),
        Focus = string.Format("Textures\\GUI\\Icons\\buttons\\small_variant\\{0}_focus.dds", (object) str),
        SizePx = new Vector2(48f, 48f)
      }, new Action<MyGuiControlButton>(this.OnCategoryButtonClick));
      guiControlButton.UserData = (object) category.Id;
      this.m_categoryButtonList.Add(guiControlButton);
      guiControlButton.Size = new Vector2(0.005f, 0.005f);
      return guiControlButton;
    }

    private void MoveSelectedItem(MyGuiControlTable from, MyGuiControlTable to)
    {
      to.Add(from.SelectedRow);
      from.RemoveSelectedRow();
      this.m_selectedRow = from.SelectedRow;
    }

    private void GetActiveMods(
      List<MyObjectBuilder_Checkpoint.ModItem> outputList)
    {
      for (int index = this.m_modsTableEnabled.RowsCount - 1; index >= 0; --index)
        outputList.Add((MyObjectBuilder_Checkpoint.ModItem) this.m_modsTableEnabled.GetRow(index).UserData);
    }

    public override bool RegisterClicks() => true;

    private void OnTableItemSelected(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs eventArgs)
    {
      sender.CanHaveFocus = true;
      this.FocusedControl = (MyGuiControlBase) sender;
      this.m_selectedRow = sender.SelectedRow;
      this.m_selectedTable = sender;
      if (sender == this.m_modsTableEnabled)
        this.m_modsTableDisabled.SelectedRowIndex = new int?();
      if (sender == this.m_modsTableDisabled)
        this.m_modsTableEnabled.SelectedRowIndex = new int?();
      if (!MyInput.Static.IsAnyCtrlKeyPressed())
        return;
      this.OnTableItemConfirmedOrDoubleClick(sender, eventArgs);
    }

    private void OnTableItemConfirmedOrDoubleClick(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs eventArgs)
    {
      if (sender.SelectedRow == null)
        return;
      MyGuiControlTable.Row selectedRow = sender.SelectedRow;
      MyObjectBuilder_Checkpoint.ModItem userData = (MyObjectBuilder_Checkpoint.ModItem) selectedRow.UserData;
      MyGuiControlTable to = sender == this.m_modsTableEnabled ? this.m_modsTableDisabled : this.m_modsTableEnabled;
      this.MoveSelectedItem(sender, to);
      if (to != this.m_modsTableEnabled || userData.PublishedFileId == 0UL)
        return;
      this.GetModDependenciesAsync(selectedRow, userData);
    }

    private void GetModDependenciesAsync(
      MyGuiControlTable.Row parentRow,
      MyObjectBuilder_Checkpoint.ModItem selectedMod)
    {
      Parallel.Start((Action<WorkData>) (workData =>
      {
        MyGuiScreenMods.ModDependenciesWorkData dependenciesWorkData = workData as MyGuiScreenMods.ModDependenciesWorkData;
        bool hasReferenceIssue;
        dependenciesWorkData.Dependencies = MyWorkshop.GetModsDependencyHiearchy(new HashSet<WorkshopId>()
        {
          new WorkshopId(dependenciesWorkData.ParentId, dependenciesWorkData.ServiceName)
        }, out hasReferenceIssue);
        dependenciesWorkData.HasReferenceIssue = hasReferenceIssue;
      }), new Action<WorkData>(this.OnGetModDependencyHiearchyCompleted), (WorkData) new MyGuiScreenMods.ModDependenciesWorkData()
      {
        ParentId = selectedMod.PublishedFileId,
        ServiceName = selectedMod.PublishedServiceName,
        ParentModRow = parentRow
      });
    }

    private void OnGetModDependencyHiearchyCompleted(WorkData workData)
    {
      if (this.State != MyGuiScreenState.OPENED || !(workData is MyGuiScreenMods.ModDependenciesWorkData dependenciesWorkData) || (dependenciesWorkData.Dependencies == null || dependenciesWorkData.Dependencies.Count <= 1))
        return;
      dependenciesWorkData.Dependencies.RemoveAt(dependenciesWorkData.Dependencies.Count - 1);
      MyGuiControlTable.Row parentModRow = dependenciesWorkData.ParentModRow;
      if (parentModRow == null)
        return;
      MyGuiControlTable.Cell cell = parentModRow.GetCell(1);
      string toolTip = MyTexts.GetString(MyCommonTexts.ScreenMods_ModDependencies);
      cell.ToolTip.ToolTips.Clear();
      StringBuilder stringBuilder = (StringBuilder) null;
      if (this.m_modsToolTips.TryGetValue(dependenciesWorkData.ParentId, out stringBuilder))
        cell.ToolTip.AddToolTip(stringBuilder.ToString());
      cell.ToolTip.AddToolTip(toolTip);
      foreach (MyWorkshopItem dependency in dependenciesWorkData.Dependencies)
        cell.ToolTip.AddToolTip(dependency.Title);
    }

    private void OnMoveUpClick(MyGuiControlButton sender) => this.m_selectedTable.MoveSelectedRowUp();

    private void OnMoveDownClick(MyGuiControlButton sender) => this.m_selectedTable.MoveSelectedRowDown();

    private void OnMoveTopClick(MyGuiControlButton sender) => this.m_selectedTable.MoveSelectedRowTop();

    private void OnMoveBottomClick(MyGuiControlButton sender) => this.m_selectedTable.MoveSelectedRowBottom();

    private void OnMoveLeftClick(MyGuiControlButton sender) => this.MoveSelectedItem(this.m_modsTableEnabled, this.m_modsTableDisabled);

    private void OnMoveRightClick(MyGuiControlButton sender) => this.MoveSelectedItem(this.m_modsTableDisabled, this.m_modsTableEnabled);

    private void OnMoveLeftAllClick(MyGuiControlButton sender)
    {
      while (this.m_modsTableEnabled.RowsCount > 0)
      {
        this.m_modsTableEnabled.SelectedRowIndex = new int?(0);
        this.MoveSelectedItem(this.m_modsTableEnabled, this.m_modsTableDisabled);
      }
    }

    private void OnMoveRightAllClick(MyGuiControlButton sender)
    {
      while (this.m_modsTableDisabled.RowsCount > 0)
      {
        this.m_modsTableDisabled.SelectedRowIndex = new int?(0);
        this.MoveSelectedItem(this.m_modsTableDisabled, this.m_modsTableEnabled);
      }
    }

    private void OnCategoryButtonClick(MyGuiControlButton sender)
    {
      if (sender.UserData == null || !(sender.UserData is string))
        return;
      string userData = (string) sender.UserData;
      if (this.m_selectedCategories.Contains(userData))
      {
        this.m_selectedCategories.Remove(userData);
        sender.Selected = false;
      }
      else
      {
        this.m_selectedCategories.Add(userData);
        sender.Selected = true;
      }
      this.RefreshModList();
    }

    private void OnSearchTextChanged(string text)
    {
      if (!string.IsNullOrEmpty(text))
        this.m_tmpSearch = ((IEnumerable<string>) text.Split(' ')).ToList<string>();
      else
        this.m_tmpSearch.Clear();
      this.RefreshModList();
    }

    private void OnPublishModClick(MyGuiControlButton sender)
    {
      if (this.m_selectedRow == null || this.m_selectedRow.UserData == null)
        return;
      this.m_selectedMod = (MyObjectBuilder_Checkpoint.ModItem) this.m_selectedRow.UserData;
      this.m_selectedMod.FriendlyName = this.m_selectedRow.GetCell(1).Text.ToString();
      this.m_selectedModWorkshopIds = MyWorkshop.GetWorkshopIdFromLocalMod(this.m_selectedMod.Name, new WorkshopId(this.m_selectedMod.PublishedFileId, this.m_selectedMod.PublishedServiceName));
      if (this.m_selectedMod.PublishedFileId != 0UL || this.m_selectedModWorkshopIds != null && this.m_selectedModWorkshopIds.Length != 0)
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextDoYouWishToUpdateMod), MyTexts.Get(MyCommonTexts.MessageBoxCaptionDoYouWishToUpdateMod), callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnPublishModQuestionAnswer)));
      else
        this.OnPublishModQuestionAnswer(MyGuiScreenMessageBox.ResultEnum.YES);
    }

    private void OnPublishModQuestionAnswer(MyGuiScreenMessageBox.ResultEnum val)
    {
      if (val != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      string[] tags = (string[]) null;
      MyWorkshopItem subscribedItem = this.GetSubscribedItem(this.m_selectedMod.PublishedFileId, this.m_selectedMod.PublishedServiceName);
      if (this.m_selectedMod.PublishedFileId == 0UL && this.m_selectedModWorkshopIds != null && this.m_selectedModWorkshopIds.Length != 0)
        subscribedItem = this.GetSubscribedItem(this.m_selectedModWorkshopIds[0].Id, this.m_selectedMod.PublishedServiceName);
      if (subscribedItem != null)
      {
        tags = subscribedItem.Tags.ToArray<string>();
        if ((long) subscribedItem.OwnerId != (long) Sync.MyId)
        {
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextPublishFailed_OwnerMismatchMod), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionModPublishFailed)));
          return;
        }
      }
      MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenWorkshopTags("mod", MyWorkshop.ModCategories, tags, new Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]>(this.OnPublishWorkshopTagsResult)));
    }

    private void OnPublishWorkshopTagsResult(
      MyGuiScreenMessageBox.ResultEnum tagsResult,
      string[] outTags,
      string[] serviceNames)
    {
      if (tagsResult != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      string modFullPath = Path.Combine(MyFileSystem.ModsPath, this.m_selectedMod.Name);
      WorkshopId[] workshopIds = MyWorkshop.FilterWorkshopIds(this.m_selectedModWorkshopIds, serviceNames);
      MyWorkshop.PublishModAsync(modFullPath, this.m_selectedMod.FriendlyName, (string) null, workshopIds, outTags, MyPublishedFileVisibility.Public, (Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]>) ((success, result, resultServiceName, publishedFiles) =>
      {
        if (publishedFiles != null && publishedFiles.Length != 0)
          MyWorkshop.GenerateModInfo(modFullPath, publishedFiles, Sync.MyId);
        MyWorkshop.ReportPublish(publishedFiles, result, resultServiceName, new Action(this.FillList));
      }));
    }

    private void OnOpenInWorkshopClick(MyGuiControlButton obj)
    {
      if (this.m_selectedRow == null || !(this.m_selectedRow.UserData is MyObjectBuilder_Checkpoint.ModItem userData))
        return;
      MyWorkshopItem subscribedItem = this.GetSubscribedItem(userData.PublishedFileId, userData.PublishedServiceName);
      if (subscribedItem == null)
        return;
      MyGuiSandbox.OpenUrlWithFallback(subscribedItem.GetItemUrl(), subscribedItem.ServiceName + " Workshop");
    }

    private void OnBrowseWorkshopClick(MyGuiControlButton obj) => MyWorkshop.OpenWorkshopBrowser(MySteamConstants.TAG_MODS, (Action) (() => this.m_refreshWhenInFocusNext = true));

    private void OnRefreshClick(MyGuiControlButton obj)
    {
      if (this.m_listNeedsReload)
        return;
      this.m_listNeedsReload = true;
      this.FillList();
    }

    private void OnOkClick(MyGuiControlButton obj)
    {
      this.m_modListToEdit.Clear();
      this.GetActiveMods(this.m_modListToEdit);
      this.CloseScreen();
    }

    private void OnCancelClick(MyGuiControlButton obj) => this.CloseScreen();

    public override bool Update(bool hasFocus)
    {
      bool flag1 = this.m_selectedRow != null;
      int num1 = !flag1 ? 0 : (this.m_selectedRow.UserData != null ? 1 : 0);
      bool flag2 = num1 != 0 && ((MyObjectBuilder_Checkpoint.ModItem) this.m_selectedRow.UserData).PublishedFileId == 0UL;
      bool flag3 = num1 != 0 && ((MyObjectBuilder_Checkpoint.ModItem) this.m_selectedRow.UserData).PublishedFileId > 0UL;
      bool flag4 = num1 != 0 && ((MyObjectBuilder_Checkpoint.ModItem) this.m_selectedRow.UserData).IsDependency;
      this.m_openInWorkshopButton.Enabled = flag1 & flag3;
      this.m_publishModButton.Enabled = flag1 & flag2 && MyFakes.ENABLE_WORKSHOP_PUBLISH;
      MyGuiControlButton moveUpButton = this.m_moveUpButton;
      MyGuiControlButton moveTopButton = this.m_moveTopButton;
      int? selectedRowIndex;
      int num2;
      if (flag1)
      {
        selectedRowIndex = this.m_selectedTable.SelectedRowIndex;
        if (selectedRowIndex.HasValue)
        {
          selectedRowIndex = this.m_selectedTable.SelectedRowIndex;
          if (selectedRowIndex.Value > 0)
          {
            num2 = !flag4 ? 1 : 0;
            goto label_5;
          }
        }
      }
      num2 = 0;
label_5:
      bool flag5 = num2 != 0;
      moveTopButton.Enabled = num2 != 0;
      int num3 = flag5 ? 1 : 0;
      moveUpButton.Enabled = num3 != 0;
      MyGuiControlButton moveDownButton = this.m_moveDownButton;
      MyGuiControlButton moveBottomButton = this.m_moveBottomButton;
      int num4;
      if (flag1)
      {
        selectedRowIndex = this.m_selectedTable.SelectedRowIndex;
        if (selectedRowIndex.HasValue)
        {
          selectedRowIndex = this.m_selectedTable.SelectedRowIndex;
          if (selectedRowIndex.Value < this.m_selectedTable.RowsCount - 1)
          {
            num4 = !flag4 ? 1 : 0;
            goto label_10;
          }
        }
      }
      num4 = 0;
label_10:
      bool flag6 = num4 != 0;
      moveBottomButton.Enabled = num4 != 0;
      int num5 = flag6 ? 1 : 0;
      moveDownButton.Enabled = num5 != 0;
      this.m_moveLeftButton.Enabled = flag1 && this.m_selectedTable == this.m_modsTableEnabled && !flag4;
      this.m_moveRightButton.Enabled = flag1 && this.m_selectedTable == this.m_modsTableDisabled && !flag4;
      this.m_moveLeftAllButton.Enabled = this.m_modsTableEnabled.RowsCount > 0 && !flag4;
      this.m_moveRightAllButton.Enabled = this.m_modsTableDisabled.RowsCount > 0 && !flag4;
      if (MySession.Static == null)
        Parallel.RunCallbacks();
      if (hasFocus)
      {
        this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.m_refreshButton.Visible = !MyInput.Static.IsJoystickLastUsed;
        this.m_browseWorkshopButton.Visible = !MyInput.Static.IsJoystickLastUsed;
        if (this.m_refreshWhenInFocusNext)
        {
          this.m_refreshWhenInFocusNext = false;
          this.OnRefreshClick((MyGuiControlButton) null);
        }
      }
      ++this.m_consentUpdateFrameTimer_current;
      return base.Update(hasFocus);
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
        this.SetWorkshopErrorText(str + MyTexts.GetString(MySpaceTexts.UGC_ServiceNotAvailable_NoConsent));
      else
        this.SetWorkshopErrorText(visible: false, skipUGCCheck: true);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (receivedFocusInThisUpdate)
        return;
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OnOkClick((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        this.OnRefreshClick((MyGuiControlButton) null);
      if (!MyControllerHelper.GetControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU).IsNewReleased())
        return;
      this.OnBrowseWorkshopClick((MyGuiControlButton) null);
    }

    public override bool Draw() => !this.m_listNeedsReload && base.Draw();

    protected override void OnShow()
    {
      base.OnShow();
      if (!this.m_listNeedsReload)
        return;
      this.FillList();
    }

    private void FillList() => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, new MyStringId?(), new Func<IMyAsyncResult>(this.beginAction), new Action<IMyAsyncResult, MyGuiScreenProgressAsync>(this.endAction)));

    private void AddHeaders()
    {
      this.m_modsTableEnabled.SetColumnName(1, new StringBuilder(MyTexts.GetString(MyCommonTexts.ScreenMods_ActiveMods) + "     "));
      this.m_modsTableEnabled.SetHeaderColumnAlign(1, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      this.m_modsTableDisabled.SetColumnName(1, new StringBuilder(MyTexts.GetString(MyCommonTexts.ScreenMods_AvailableMods) + "     "));
      this.m_modsTableDisabled.SetHeaderColumnAlign(1, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
    }

    private MyGuiControlTable.Row AddMod(
      bool active,
      StringBuilder title,
      StringBuilder toolTip,
      StringBuilder modState,
      MyGuiHighlightTexture? icon,
      MyObjectBuilder_Checkpoint.ModItem mod,
      Color? textColor = null)
    {
      MyGuiControlTable.Row row = new MyGuiControlTable.Row((object) mod);
      row.AddCell(new MyGuiControlTable.Cell(string.Empty, toolTip: modState.ToString(), icon: icon));
      row.AddCell(new MyGuiControlTable.Cell(title, toolTip: toolTip.ToString(), textColor: textColor));
      if (active)
        this.m_modsTableEnabled.Insert(0, row);
      else
        this.m_modsTableDisabled.Add(row);
      if (mod.PublishedFileId != 0UL)
        this.m_modsToolTips[mod.PublishedFileId] = toolTip;
      return row;
    }

    private MyWorkshopItem GetSubscribedItem(
      ulong publishedFileId,
      string serviceName)
    {
      foreach (MyWorkshopItem subscribedMod in this.m_subscribedMods)
      {
        if ((long) subscribedMod.Id == (long) publishedFileId && subscribedMod.ServiceName == serviceName)
          return subscribedMod;
      }
      foreach (MyWorkshopItem worldMod in this.m_worldMods)
      {
        if ((long) worldMod.Id == (long) publishedFileId && worldMod.ServiceName == serviceName)
          return worldMod;
      }
      return (MyWorkshopItem) null;
    }

    private void RefreshCategoryButtons()
    {
      foreach (MyGuiControlButton categoryButton in this.m_categoryButtonList)
      {
        if (categoryButton.UserData != null)
        {
          string lower = (categoryButton.UserData as string).ToLower();
          categoryButton.Selected = this.m_selectedCategories.Contains(lower);
        }
      }
    }

    private void RefreshModList()
    {
      this.m_selectedRow = (MyGuiControlTable.Row) null;
      this.m_selectedTable = (MyGuiControlTable) null;
      if (this.m_modsTableEnabled == null)
        return;
      ListReader<MyObjectBuilder_Checkpoint.ModItem> mods;
      if (this.m_keepActiveMods)
      {
        List<MyObjectBuilder_Checkpoint.ModItem> outputList = new List<MyObjectBuilder_Checkpoint.ModItem>(this.m_modsTableEnabled.RowsCount);
        this.GetActiveMods(outputList);
        mods = (ListReader<MyObjectBuilder_Checkpoint.ModItem>) outputList;
      }
      else
        mods = (ListReader<MyObjectBuilder_Checkpoint.ModItem>) this.m_modListToEdit;
      this.m_keepActiveMods = true;
      this.RefreshWorldMods(mods);
      this.m_modsTableEnabled.Clear();
      this.m_modsTableDisabled.Clear();
      this.m_modsToolTips.Clear();
      this.AddHeaders();
      foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
      {
        if (!mod.IsDependency)
        {
          if (mod.PublishedFileId == 0UL)
          {
            StringBuilder title = new StringBuilder(mod.Name);
            string path = Path.Combine(MyFileSystem.ModsPath, mod.Name);
            StringBuilder toolTip = new StringBuilder(path);
            StringBuilder modState = MyTexts.Get(MyCommonTexts.ScreenMods_LocalMod);
            Color? textColor = new Color?();
            MyGuiHighlightTexture iconBlueprintsLocal = MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL;
            if (!Directory.Exists(path) && !File.Exists(path))
            {
              toolTip = MyTexts.Get(MyCommonTexts.ScreenMods_MissingLocalMod);
              modState = toolTip;
              textColor = new Color?(MyHudConstants.MARKER_COLOR_RED);
            }
            this.AddMod(true, title, toolTip, modState, new MyGuiHighlightTexture?(iconBlueprintsLocal), mod, textColor);
          }
          else
          {
            StringBuilder title = new StringBuilder();
            StringBuilder toolTip = new StringBuilder();
            StringBuilder modState = MyTexts.Get(MyCommonTexts.ScreenMods_WorkshopMod);
            Color? textColor = new Color?();
            MyWorkshopItem subscribedItem = this.GetSubscribedItem(mod.PublishedFileId, mod.PublishedServiceName);
            if (subscribedItem != null)
            {
              if (!string.IsNullOrEmpty(subscribedItem.Title))
                title.Append(subscribedItem.Title);
              else
                title.Append(string.Format(MyTexts.GetString(MyCommonTexts.ModNotReceived), (object) mod.PublishedFileId));
              if (!string.IsNullOrEmpty(subscribedItem.Description))
              {
                int num1 = Math.Min(subscribedItem.Description.Length, 128);
                int num2 = subscribedItem.Description.IndexOf("\n");
                if (num2 > 0)
                  num1 = Math.Min(num1, num2 - 1);
                toolTip.Append(subscribedItem.Description.Substring(0, num1));
              }
              else
                toolTip.Append(string.Format(MyTexts.GetString(MyCommonTexts.ModNotReceived_ToolTip), (object) mod.PublishedServiceName));
            }
            else
            {
              title.Append(mod.PublishedFileId.ToString());
              toolTip = new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.ScreenMods_MissingDetails), (object) mod.PublishedServiceName);
              textColor = new Color?(MyHudConstants.MARKER_COLOR_RED);
            }
            MyGuiHighlightTexture workshopIcon = MyGuiConstants.GetWorkshopIcon(mod.PublishedServiceName);
            this.AddMod(true, title, toolTip, modState, new MyGuiHighlightTexture?(workshopIcon), mod, textColor);
          }
        }
      }
      this.FillLocalMods();
      if (this.m_subscribedMods == null)
        return;
      this.FillSubscribedMods();
    }

    private void FillSubscribedMods()
    {
      foreach (MyWorkshopItem subscribedMod in this.m_subscribedMods)
      {
        MyWorkshopItem mod = subscribedMod;
        if (mod != null && !this.m_worldWorkshopMods.Contains(new WorkshopId(mod.Id, mod.ServiceName)) && (this.m_selectedServiceNames.Count == 0 || !this.m_selectedServiceNames.All<string>((Func<string, bool>) (x => x != mod.ServiceName))))
        {
          if (MyFakes.ENABLE_MOD_CATEGORIES)
          {
            bool flag = false;
            foreach (string tag in mod.Tags)
            {
              if (this.m_selectedCategories.Contains(tag.ToLower()) || this.m_selectedCategories.Count == 0)
              {
                flag = true;
                break;
              }
            }
            if (!this.CheckSearch(mod.Title) || !flag)
              continue;
          }
          StringBuilder title = new StringBuilder(mod.Title);
          int num1 = Math.Min(mod.Description.Length, 128);
          int num2 = mod.Description.IndexOf("\n");
          if (num2 > 0)
            num1 = Math.Min(num1, num2 - 1);
          StringBuilder toolTip = new StringBuilder();
          StringBuilder modState = MyTexts.Get(MyCommonTexts.ScreenMods_WorkshopMod);
          toolTip.Append(mod.Description.Substring(0, num1));
          MyGuiHighlightTexture workshopIcon = MyGuiConstants.GetWorkshopIcon(mod);
          MyObjectBuilder_Checkpoint.ModItem mod1 = new MyObjectBuilder_Checkpoint.ModItem((string) null, mod.Id, mod.ServiceName, mod.Title);
          this.AddMod(false, title, toolTip, modState, new MyGuiHighlightTexture?(workshopIcon), mod1);
        }
      }
    }

    private void FillLocalMods()
    {
      if (!Directory.Exists(MyFileSystem.ModsPath))
        Directory.CreateDirectory(MyFileSystem.ModsPath);
      foreach (string directory in Directory.GetDirectories(MyFileSystem.ModsPath, "*", SearchOption.TopDirectoryOnly))
      {
        string fileName = Path.GetFileName(directory);
        if (!this.m_worldLocalMods.Contains(fileName) && Directory.GetFileSystemEntries(directory).Length != 0 && (!MyFakes.ENABLE_MOD_CATEGORIES || this.CheckSearch(fileName)))
          this.AddMod(false, new StringBuilder(fileName), new StringBuilder(directory), MyTexts.Get(MyCommonTexts.ScreenMods_LocalMod), new MyGuiHighlightTexture?(MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL), new MyObjectBuilder_Checkpoint.ModItem(fileName, 0UL, MyGameService.GetDefaultUGC().ServiceName));
      }
    }

    private bool CheckSearch(string name)
    {
      bool flag = true;
      string lower = name.ToLower();
      foreach (string str in this.m_tmpSearch)
      {
        if (!lower.Contains(str.ToLower()))
        {
          flag = false;
          break;
        }
      }
      return flag;
    }

    private IMyAsyncResult beginAction() => (IMyAsyncResult) new MyModsLoadListResult(this.m_worldWorkshopMods);

    private void endAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
    {
      this.m_listNeedsReload = false;
      if (!(result is MyModsLoadListResult modsLoadListResult))
        return;
      if (modsLoadListResult.Result.Item1 != MyGameServiceCallResult.OK)
        this.SetWorkshopErrorText(MyWorkshop.GetWorkshopErrorText(modsLoadListResult.Result.Item1, modsLoadListResult.Result.Item2, this.m_workshopPermitted));
      else
        this.SetWorkshopErrorText(visible: false);
      this.m_subscribedMods = modsLoadListResult.SubscribedMods;
      this.m_worldMods = modsLoadListResult.SetMods;
      this.RefreshModList();
      screen.CloseScreen();
    }

    private class ModDependenciesWorkData : WorkData
    {
      public ulong ParentId;
      public string ServiceName;
      public MyGuiControlTable.Row ParentModRow;
      public List<MyWorkshopItem> Dependencies;
      public bool HasReferenceIssue;
    }
  }
}
