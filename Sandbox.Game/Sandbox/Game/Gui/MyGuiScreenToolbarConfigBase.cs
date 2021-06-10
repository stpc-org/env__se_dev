// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenToolbarConfigBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.GameServices;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenToolbarConfigBase : MyGuiScreenBase
  {
    public static MyGuiScreenToolbarConfigBase Static;
    private float m_minVerticalPosition;
    private bool m_researchItemFound;
    private MyGuiControlLabel m_toolbarLabel;
    protected MyGuiControlSearchBox m_searchBox;
    protected MyGuiControlListbox m_categoriesListbox;
    protected MyGuiControlGrid m_gridBlocks;
    protected MyGuiControlBlockGroupInfo m_blockGroupInfo;
    protected MyGuiControlScrollablePanel m_gridBlocksPanel;
    protected MyGuiControlScrollablePanel m_researchPanel;
    protected MyGuiControlResearchGraph m_researchGraph;
    protected MyGuiControlLabel m_blocksLabel;
    protected MyGuiControlGridDragAndDrop m_dragAndDrop;
    protected MyGuiControlToolbar m_toolbarControl;
    protected MyGuiControlContextMenu m_contextMenu;
    protected MyGuiControlContextMenu m_onDropContextMenu;
    protected MyObjectBuilder_ToolbarControlVisualStyle m_toolbarStyle;
    protected MyGuiControlTabControl m_tabControl;
    private MyShipController m_shipController;
    protected MyCharacter m_character;
    protected MyCubeGrid m_screenCubeGrid;
    protected const string SHIP_GROUPS_NAME = "Groups";
    protected const string CHARACTER_ANIMATIONS_GROUP_NAME = "CharacterAnimations";
    protected MyStringHash manipulationToolId = MyStringHash.GetOrCompute("ManipulationTool");
    protected string[] m_forcedCategoryOrder = new string[8]
    {
      "ShipWeapons",
      "ShipTools",
      "Weapons",
      "Tools",
      "CharacterWeapons",
      "CharacterTools",
      "CharacterAnimations",
      "Groups"
    };
    protected MySearchByStringCondition m_nameSearchCondition = new MySearchByStringCondition();
    protected MySearchByCategoryCondition m_categorySearchCondition = new MySearchByCategoryCondition();
    protected SortedDictionary<string, MyGuiBlockCategoryDefinition> m_sortedCategories = new SortedDictionary<string, MyGuiBlockCategoryDefinition>();
    protected static List<MyGuiBlockCategoryDefinition> m_allSelectedCategories = new List<MyGuiBlockCategoryDefinition>();
    protected List<MyGuiBlockCategoryDefinition> m_searchInBlockCategories = new List<MyGuiBlockCategoryDefinition>();
    private HashSet<string> m_tmpUniqueStrings = new HashSet<string>();
    protected MyGuiBlockCategoryDefinition m_shipGroupsCategory = new MyGuiBlockCategoryDefinition();
    protected float m_scrollOffset;
    protected static float m_savedVPosition = 0.0f;
    protected int m_contextBlockX = -1;
    protected int m_contextBlockY = -1;
    protected int m_onDropContextMenuToolbarIndex = -1;
    protected MyToolbarItem m_onDropContextMenuItem;
    protected bool m_shipMode;
    private MyGuiControlLabel m_categoryHintLeft;
    private MyGuiControlLabel m_categoryHintRight;
    public static MyGuiScreenToolbarConfigBase.GroupModes GroupMode = MyGuiScreenToolbarConfigBase.GroupModes.HideEmpty;
    protected MyCubeBlock m_screenOwner;
    protected static bool m_ownerChanged = false;
    protected static MyEntity m_previousOwner = (MyEntity) null;
    private int m_framesBeforeSearchEnabled = 5;
    private ConditionBase m_visibleCondition;
    protected MyGuiControlPcuBar m_PCUControl;
    private int m_frameCounterPCU;
    private readonly int PCU_UPDATE_EACH_N_FRAMES = 1;
    private readonly List<int> m_blockOffsets = new List<int>();
    protected int? m_gamepadSlot;

    public MyGuiScreenToolbarConfigBase(
      MyObjectBuilder_ToolbarControlVisualStyle toolbarStyle,
      int scrollOffset = 0,
      MyCubeBlock owner = null,
      int? gamepadSlot = null)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity, gamepadSlot: gamepadSlot)
    {
      MySandboxGame.Log.WriteLine("MyGuiScreenCubeBuilder.ctor START");
      MyGuiScreenToolbarConfigBase.Static = this;
      this.m_toolbarStyle = toolbarStyle;
      this.m_visibleCondition = this.m_toolbarStyle.VisibleCondition;
      this.m_toolbarStyle.VisibleCondition = (ConditionBase) null;
      this.m_scrollOffset = (float) scrollOffset / 6.5f;
      this.m_size = new Vector2?(new Vector2(1f, 1f));
      this.m_canShareInput = true;
      this.m_drawEvenWithoutFocus = true;
      this.EnabledBackgroundFade = true;
      this.m_screenOwner = owner;
      this.GetType();
      if (typeof (MyGuiScreenToolbarConfigBase) == this.GetType())
        this.RecreateControls(true);
      this.m_framesBeforeSearchEnabled = 10;
      this.m_gamepadSlot = gamepadSlot;
      MySandboxGame.Log.WriteLine("MyGuiScreenCubeBuilder.ctor END");
    }

    protected override void OnClosed()
    {
      this.m_toolbarStyle.VisibleCondition = this.m_visibleCondition;
      MyGuiScreenToolbarConfigBase.Static = (MyGuiScreenToolbarConfigBase) null;
      base.OnClosed();
      MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
    }

    public static void Reset() => MyGuiScreenToolbarConfigBase.m_allSelectedCategories.Clear();

    public override bool RegisterClicks() => true;

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (!this.m_contextMenu.IsActiveControl && !this.m_onDropContextMenu.IsActiveControl)
      {
        if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT) && this.SelectedCategoryMove(this.m_categoriesListbox, -1))
          this.categories_ItemClicked(this.m_categoriesListbox);
        if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT) && this.SelectedCategoryMove(this.m_categoriesListbox))
          this.categories_ItemClicked(this.m_categoriesListbox);
        if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION1) && this.m_gamepadSlot.HasValue)
        {
          MyToolbarComponent.CurrentToolbar.SetItemAtIndex(this.m_gamepadSlot.Value, (MyToolbarItem) null, true);
          this.CloseScreen(false);
        }
      }
      if (this.FocusedControl == null && MyInput.Static.IsKeyPress(MyKeys.Tab))
      {
        if (MyVRage.Platform.ImeProcessor != null)
          MyVRage.Platform.ImeProcessor.RegisterActiveScreen((IVRageGuiScreen) this);
        this.FocusedControl = (MyGuiControlBase) this.m_searchBox.TextBox;
      }
      if (MyInput.Static.IsMouseReleased(MyMouseButtonsEnum.Right))
      {
        if (this.m_onDropContextMenu.Enabled)
        {
          this.m_onDropContextMenu.Enabled = false;
          this.m_contextMenu.Enabled = false;
          Vector2? offset = new Vector2?();
          if (MyInput.Static.IsJoystickLastUsed && this.m_gridBlocks.SelectedIndex.HasValue)
            offset = new Vector2?(this.m_gridBlocks.GetItemPosition(this.m_gridBlocks.SelectedIndex.Value, true));
          this.m_onDropContextMenu.Activate(offset: offset);
          this.FocusedControl = this.m_onDropContextMenu.GetInnerList();
        }
        else if (this.m_contextMenu.Enabled && !this.m_onDropContextMenu.Visible)
        {
          this.m_contextMenu.Enabled = false;
          Vector2? offset = new Vector2?();
          if (MyInput.Static.IsJoystickLastUsed && this.m_gridBlocks.SelectedIndex.HasValue)
            offset = new Vector2?(this.m_gridBlocks.GetItemPosition(this.m_gridBlocks.SelectedIndex.Value, true));
          this.m_contextMenu.Activate(offset: offset);
          this.FocusedControl = this.m_contextMenu.GetInnerList();
        }
      }
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.BUILD_SCREEN))
      {
        if (!this.m_searchBox.TextBox.HasFocus)
        {
          if (this.m_closingCueEnum.HasValue)
            MyGuiSoundManager.PlaySound(this.m_closingCueEnum.Value);
          else
            MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
          this.CloseScreen(false);
        }
        else if (MyInput.Static.IsNewGameControlJoystickOnlyPressed(MyControlsSpace.BUILD_SCREEN))
        {
          if (this.m_closingCueEnum.HasValue)
            MyGuiSoundManager.PlaySound(this.m_closingCueEnum.Value);
          else
            MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
          this.CloseScreen(false);
        }
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION1) && MyGuiScreenToolbarConfigBase.Static != null && MyGuiScreenToolbarConfigBase.Static.IsBuildPlannerShown())
      {
        MyGuiScreenToolbarConfigBase toolbarConfigBase = MyGuiScreenToolbarConfigBase.Static;
        MyGuiGridItem selectedItem = this.m_researchGraph.SelectedItem;
        if (selectedItem != null && selectedItem.ItemDefinition is MyCubeBlockDefinition itemDefinition && MySession.Static.LocalCharacter.AddToBuildPlanner(itemDefinition))
        {
          MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
          this.m_blockGroupInfo.UpdateBuildPlanner();
        }
      }
      if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION2))
        return;
      MySession.Static.LocalCharacter.RemoveLastFromBuildPlanner();
      MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
      this.m_blockGroupInfo.UpdateBuildPlanner();
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
    {
      if (!MyInput.Static.IsNewGameControlPressed(MyControlsSpace.PAUSE_GAME))
        return;
      MySandboxGame.PauseToggle();
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      MyGuiScreenToolbarConfigBase.m_savedVPosition = this.m_gridBlocksPanel.ScrollbarVPosition;
      MyGuiScreenToolbarConfigBase.Static = (MyGuiScreenToolbarConfigBase) null;
      return base.CloseScreen(isUnloading);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenToolbarConfigBase);

    public override void RecreateControls(bool contructor)
    {
      base.RecreateControls(contructor);
      this.m_character = (MyCharacter) null;
      this.m_shipController = (MyShipController) null;
      MyGuiScreenToolbarConfigBase.m_ownerChanged = MyGuiScreenToolbarConfigBase.m_previousOwner != MyToolbarComponent.CurrentToolbar.Owner;
      MyGuiScreenToolbarConfigBase.m_previousOwner = MyToolbarComponent.CurrentToolbar.Owner;
      if (MyToolbarComponent.CurrentToolbar.Owner == null)
        this.m_character = MySession.Static.LocalCharacter;
      else
        this.m_shipController = MyToolbarComponent.CurrentToolbar.Owner as MyShipController;
      this.m_screenCubeGrid = this.m_screenOwner == null ? (MyCubeGrid) null : this.m_screenOwner.CubeGrid;
      bool isShip = this.m_screenCubeGrid != null;
      MyObjectBuilder_GuiScreen objectBuilder;
      MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_GuiScreen>(Path.Combine(MyFileSystem.ContentPath, Path.Combine("Data", "Screens", "CubeBuilder.gsc")), out objectBuilder);
      this.Init(objectBuilder);
      this.m_tabControl = this.Controls.GetControlByName("Tab") as MyGuiControlTabControl;
      this.m_tabControl.TabButtonScale = 0.5f;
      this.m_tabControl.ButtonsOffset = new Vector2(0.0f, -0.03f);
      this.m_tabControl.ShowGamepadHelp = false;
      this.m_tabControl.CanHaveFocus = true;
      MyGuiControlTabPage controlByName1 = this.m_tabControl.Controls.GetControlByName("BlocksPage") as MyGuiControlTabPage;
      this.m_gridBlocks = (MyGuiControlGrid) controlByName1.Controls.GetControlByName("Grid");
      Vector4 colorMask = this.m_gridBlocks.ColorMask;
      this.m_gridBlocks.ColorMask = Vector4.One;
      this.m_gridBlocks.ItemBackgroundColorMask = colorMask;
      this.m_categoriesListbox = (MyGuiControlListbox) this.Controls.GetControlByName("CategorySelector");
      this.m_categoriesListbox.CanHaveFocus = false;
      this.m_categoriesListbox.VisualStyle = MyGuiControlListboxStyleEnum.ToolsBlocks;
      this.m_categoriesListbox.ItemClicked += new Action<MyGuiControlListbox>(this.categories_ItemClicked);
      MyGuiControlTextbox controlByName2 = (MyGuiControlTextbox) this.Controls.GetControlByName("SearchItemTextBox");
      MyGuiControlLabel controlByName3 = (MyGuiControlLabel) this.Controls.GetControlByName("BlockSearchLabel");
      this.m_searchBox = new MyGuiControlSearchBox(new Vector2?(controlByName2.Position + new Vector2(-0.1f, -0.005f)), new Vector2?(controlByName2.Size + new Vector2(0.2f, 0.0f)));
      this.m_searchBox.OnTextChanged += new MyGuiControlSearchBox.TextChangedDelegate(this.searchItemTexbox_TextChanged);
      this.m_searchBox.Enabled = true;
      MyGuiControlTextbox textBox = this.m_searchBox.TextBox;
      MyGuiControlTextbox.MySkipCombination[] mySkipCombinationArray = new MyGuiControlTextbox.MySkipCombination[2];
      MyGuiControlTextbox.MySkipCombination mySkipCombination = new MyGuiControlTextbox.MySkipCombination();
      mySkipCombination.Ctrl = true;
      mySkipCombination.Keys = (MyKeys[]) null;
      mySkipCombinationArray[0] = mySkipCombination;
      mySkipCombination = new MyGuiControlTextbox.MySkipCombination();
      mySkipCombination.Keys = new MyKeys[2]
      {
        MyKeys.Snapshot,
        MyKeys.Delete
      };
      mySkipCombinationArray[1] = mySkipCombination;
      textBox.SkipCombinations = mySkipCombinationArray;
      this.Controls.Add((MyGuiControlBase) this.m_searchBox);
      this.Controls.Remove((MyGuiControlBase) controlByName2);
      this.Controls.Remove((MyGuiControlBase) controlByName3);
      controlByName1.Controls.Remove((MyGuiControlBase) this.m_gridBlocks);
      this.m_gridBlocks.VisualStyle = MyGuiControlGridStyleEnum.Toolbar;
      this.m_gridBlocksPanel = new MyGuiControlScrollablePanel((MyGuiControlBase) null);
      MyGuiStyleDefinition visualStyle = MyGuiControlGrid.GetVisualStyle(MyGuiControlGridStyleEnum.ToolsBlocks);
      this.m_gridBlocksPanel.BackgroundTexture = visualStyle.BackgroundTexture;
      this.m_gridBlocksPanel.ColorMask = colorMask;
      this.m_gridBlocksPanel.ScrolledControl = (MyGuiControlBase) this.m_gridBlocks;
      this.m_gridBlocksPanel.ScrollbarVEnabled = true;
      this.m_gridBlocksPanel.ScrolledAreaPadding = new MyGuiBorderThickness(10f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 10f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
      this.m_gridBlocksPanel.FitSizeToScrolledControl();
      this.m_gridBlocksPanel.Size = this.m_gridBlocksPanel.Size + new Vector2(0.0f, 0.032f);
      this.m_gridBlocksPanel.PanelScrolled += new Action<MyGuiControlScrollablePanel>(this.grid_PanelScrolled);
      this.m_gridBlocksPanel.Position = new Vector2(-0.216f, -0.044f);
      controlByName1.Controls.Add((MyGuiControlBase) this.m_gridBlocksPanel);
      Vector2 vector2_1 = new Vector2(-0.495f, -0.52f) + this.m_categoriesListbox.GetPositionAbsoluteTopLeft();
      Vector2 vector2_2 = new Vector2(-0.505f, -0.52f) + this.m_categoriesListbox.GetPositionAbsoluteTopRight();
      this.m_categoryHintLeft = new MyGuiControlLabel(new Vector2?(vector2_1), text: '\xE005'.ToString(), textScale: 1f);
      this.m_categoryHintRight = new MyGuiControlLabel(new Vector2?(vector2_2), text: '\xE006'.ToString(), textScale: 1f, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      this.Controls.Add((MyGuiControlBase) this.m_categoryHintLeft);
      this.Controls.Add((MyGuiControlBase) this.m_categoryHintRight);
      if ((double) this.m_scrollOffset != 0.0)
        this.m_gridBlocksPanel.SetPageVertical(this.m_scrollOffset);
      else
        this.m_gridBlocksPanel.ScrollbarVPosition = MyGuiScreenToolbarConfigBase.m_savedVPosition;
      this.m_researchGraph = new MyGuiControlResearchGraph();
      this.m_researchGraph.GamepadHelpTextId = MyCommonTexts.ResearchGraph_BuildPlanner_Control;
      this.m_researchGraph.ItemSize = new Vector2(0.05125f, 0.06833334f) * 0.75f;
      this.m_researchGraph.NodePadding = this.m_researchGraph.ItemSize / 7f;
      this.m_researchGraph.NodeMargin = this.m_researchGraph.ItemSize / 7f;
      this.m_researchGraph.Size = new Vector2(0.52f, 0.0f);
      this.m_researchGraph.ItemClicked += new EventHandler<MySharedButtonsEnum>(this.m_researchGraph_ItemClicked);
      this.m_researchGraph.ItemDoubleClicked += new EventHandler(this.m_researchGraph_ItemDoubleClicked);
      this.m_researchGraph.ItemDragged += new EventHandler<MyGuiGridItem>(this.m_researchGraph_ItemDragged);
      this.m_researchGraph.Nodes = this.CreateResearchGraph();
      this.m_researchGraph.SelectedItem = this.m_researchGraph.Nodes[0].Items[0];
      MyGuiControlTabPage controlByName4 = this.m_tabControl.Controls.GetControlByName("ResearchPage") as MyGuiControlTabPage;
      if (MySession.Static != null && MySession.Static.Settings != null && MySession.Static.Settings.EnableResearch)
      {
        controlByName4.SetToolTip((string) null);
        controlByName4.Enabled = true;
      }
      else
      {
        controlByName4.SetToolTip(MySpaceTexts.ToolbarConfig_ResearchTabDisabledTooltip);
        controlByName4.Enabled = false;
      }
      this.m_researchPanel = new MyGuiControlScrollablePanel((MyGuiControlBase) null);
      this.m_researchPanel.BackgroundTexture = visualStyle.BackgroundTexture;
      this.m_researchPanel.ColorMask = colorMask;
      this.m_researchPanel.ScrolledControl = (MyGuiControlBase) this.m_researchGraph;
      this.m_researchPanel.ScrollbarVEnabled = true;
      this.m_researchPanel.ScrollbarHEnabled = true;
      this.m_researchPanel.ScrolledAreaPadding = new MyGuiBorderThickness(10f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 10f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
      this.m_researchPanel.FitSizeToScrolledControl();
      this.m_researchPanel.Size = this.m_gridBlocksPanel.Size;
      this.m_researchPanel.Position = this.m_gridBlocksPanel.Position;
      controlByName4.Controls.Add((MyGuiControlBase) this.m_researchPanel);
      this.m_toolbarLabel = (MyGuiControlLabel) this.Controls.GetControlByName("LabelToolbar");
      this.m_toolbarLabel.Position = new Vector2(this.m_toolbarLabel.Position.X - 0.12f, this.m_toolbarLabel.Position.Y);
      this.m_toolbarControl = (MyGuiControlToolbar) Activator.CreateInstance(MyPerGameSettings.GUI.ToolbarControl, (object) this.m_toolbarStyle, (object) true);
      this.m_toolbarControl.Position = this.m_toolbarStyle.CenterPosition - new Vector2(0.62f, 0.5f);
      this.m_toolbarControl.OriginAlign = this.m_toolbarStyle.OriginAlign;
      this.Controls.Add((MyGuiControlBase) this.m_toolbarControl);
      if (this.m_gamepadSlot.HasValue)
      {
        this.m_toolbarLabel.Visible = false;
        this.m_toolbarControl.Visible = false;
      }
      if (this.m_screenOwner == null)
      {
        Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
        MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_toolbarLabel.Position.X, this.m_toolbarLabel.PositionY)));
        myGuiControlLabel.UseTextShadow = true;
        myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.Gamepad_Help_Back);
      }
      this.m_onDropContextMenu = new MyGuiControlContextMenu();
      this.m_onDropContextMenu.Deactivate();
      this.m_onDropContextMenu.ItemClicked += new Action<MyGuiControlContextMenu, MyGuiControlContextMenu.EventArgs>(this.onDropContextMenu_ItemClicked);
      this.m_onDropContextMenu.OnDeactivated += new Action(this.contextMenu_Deactivated);
      this.Controls.Add((MyGuiControlBase) this.m_onDropContextMenu);
      this.m_gridBlocks.SetItemsToDefault();
      this.m_gridBlocks.ItemDoubleClicked += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_ItemDoubleClicked);
      this.m_gridBlocks.ItemClicked += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_ItemClicked);
      this.m_gridBlocks.ItemDragged += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_OnDrag);
      this.m_gridBlocks.ItemAccepted += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_ItemDoubleClicked);
      this.m_dragAndDrop = new MyGuiControlGridDragAndDrop(MyGuiConstants.DRAG_AND_DROP_BACKGROUND_COLOR, MyGuiConstants.DRAG_AND_DROP_TEXT_COLOR, 0.7f, MyGuiConstants.DRAG_AND_DROP_TEXT_OFFSET, true);
      this.m_dragAndDrop.ItemDropped += new OnItemDropped(this.dragAndDrop_OnDrop);
      this.m_dragAndDrop.DrawBackgroundTexture = false;
      this.Controls.Add((MyGuiControlBase) this.m_dragAndDrop);
      MyGuiControlPcuBar guiControlPcuBar = new MyGuiControlPcuBar(new Vector2?(new Vector2(0.153f, 0.4f)));
      guiControlPcuBar.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_PCUControl = guiControlPcuBar;
      this.Controls.Add((MyGuiControlBase) this.m_PCUControl);
      this.m_PCUControl.UpdatePCU(this.GetIdentity(), false);
      this.SolveAspectRatio();
      this.AddCategoryToDisplayList(MyTexts.GetString(MySpaceTexts.DisplayName_Category_AllBlocks), (MyGuiBlockCategoryDefinition) null);
      Dictionary<string, MyGuiBlockCategoryDefinition> categories = MyDefinitionManager.Static.GetCategories();
      if (this.m_screenCubeGrid != null && (this.m_shipController == null || this.m_shipController != null && !this.m_shipController.BuildingMode))
      {
        if ((!isShip || this.m_shipController == null ? 0 : (!this.m_shipController.EnableShipControl ? 1 : 0)) == 0)
        {
          this.RecreateShipCategories(categories, this.m_sortedCategories, this.m_screenCubeGrid);
          this.AddShipGroupsIntoCategoryList(this.m_screenCubeGrid);
          this.AddShipBlocksDefinitions(this.m_screenCubeGrid, isShip, (IMySearchCondition) null);
          this.AddShipGunsToCategories(categories, this.m_sortedCategories);
        }
        else
          this.m_categoriesListbox.Items.Clear();
        if (this.m_shipController != null && this.m_shipController.ToolbarType != MyToolbarType.None)
        {
          MyGuiBlockCategoryDefinition categoryDefinition = (MyGuiBlockCategoryDefinition) null;
          if (!this.m_sortedCategories.TryGetValue("CharacterAnimations", out categoryDefinition) && categories.TryGetValue("CharacterAnimations", out categoryDefinition))
            this.m_sortedCategories.Add("CharacterAnimations", categoryDefinition);
        }
        this.m_researchGraph.Nodes.Clear();
        this.m_tabControl.GetTab(1).IsTabVisible = this.m_tabControl.GetTab(1).Enabled = false;
        this.m_shipMode = true;
        this.m_PCUControl.Visible = false;
        this.m_PCUControl.Controls.Clear();
      }
      else if (this.m_character != null || this.m_shipController != null && this.m_shipController.BuildingMode)
      {
        if (MyGuiScreenToolbarConfigBase.GroupMode != MyGuiScreenToolbarConfigBase.GroupModes.HideAll)
          this.RecreateBlockCategories(categories, this.m_sortedCategories);
        this.AddCubeDefinitionsToBlocks((IMySearchCondition) this.m_categorySearchCondition);
        this.m_tabControl.GetTab(1).IsTabVisible = this.m_tabControl.GetTab(1).Enabled = true;
        this.m_shipMode = false;
        this.m_PCUControl.Visible = true;
      }
      if (MyFakes.ENABLE_SHIP_BLOCKS_TOOLBAR)
      {
        this.m_gridBlocks.Visible = true;
        this.m_gridBlocksPanel.ScrollbarVEnabled = true;
      }
      else
      {
        this.m_gridBlocksPanel.ScrollbarVEnabled = !isShip;
        this.m_gridBlocks.Visible = !isShip;
      }
      this.SortCategoriesToDisplayList();
      if (this.m_categoriesListbox.Items.Count > 0)
        this.SelectCategories();
      if (this.m_gamepadSlot.HasValue)
      {
        this.m_framesBeforeSearchEnabled = -1;
        this.FocusedControl = this.m_tabControl.SelectedPage == 0 ? (MyGuiControlBase) this.m_gridBlocks : (MyGuiControlBase) this.m_researchGraph;
      }
      if (this.m_gamepadSlot.HasValue)
      {
        char ch1 = ' ';
        int? gamepadSlot = this.m_gamepadSlot;
        int? nullable = gamepadSlot.HasValue ? new int?(gamepadSlot.GetValueOrDefault() % 4) : new int?();
        if (nullable.HasValue)
        {
          switch (nullable.GetValueOrDefault())
          {
            case 0:
              ch1 = '\xE011';
              break;
            case 1:
              ch1 = '\xE010';
              break;
            case 2:
              ch1 = '\xE012';
              break;
            case 3:
              ch1 = '\xE013';
              break;
          }
        }
        MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel();
        myGuiControlPanel.Size = new Vector2(0.268f, 0.678f);
        myGuiControlPanel.Position = new Vector2(0.376f, 0.019f);
        myGuiControlPanel.ColorMask = new Vector4(0.1333333f, 0.1803922f, 0.2039216f, 0.9f);
        myGuiControlPanel.BackgroundTexture = new MyGuiCompositeTexture("Textures\\GUI\\Blank.dds");
        this.Controls.Add((MyGuiControlBase) myGuiControlPanel);
        string text1 = string.Format(MyTexts.GetString(MyCommonTexts.Gamepad_GScreen_Caption), (object) ch1.ToString());
        Vector2 vector2_3 = new Vector2(0.265f, -0.29f);
        MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(new Vector2?(vector2_3), text: text1);
        myGuiControlLabel1.Autowrap(0.235f);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
        Vector2 vector2_4 = new Vector2(0.085f, 0.0f);
        MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
        controlSeparatorList.AddHorizontal(vector2_3 + new Vector2(0.0f, 0.035f), 0.22f);
        this.Controls.Add((MyGuiControlBase) controlSeparatorList);
        char ch2 = '\xE00F';
        string codeForControl1 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT);
        string codeForControl2 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACTION1);
        string text2 = string.Format(MyTexts.GetString(MyCommonTexts.Gamepad_GScreen_Hint), (object) ch2.ToString(), (object) codeForControl1, (object) codeForControl2);
        Vector2 vector2_5 = new Vector2(0.0f, 0.59f);
        MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(vector2_3 + vector2_5), text: text2, textScale: 0.67f);
        myGuiControlLabel2.Autowrap(0.235f);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
        Vector2 vector2_6 = new Vector2(0.29f, -0.2f);
        Vector2 vector2_7 = new Vector2(0.027f, -0.26f);
        Vector2 vector2_8 = new Vector2(72f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
        MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage(new Vector2?(vector2_6), new Vector2?(vector2_8));
        myGuiControlImage1.SetTexture("Textures\\GUI\\Controls\\grid_item_highlight.dds");
        MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(new Vector2?(vector2_6), new Vector2?(vector2_8));
        myGuiControlImage2.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(this.m_gamepadSlot.Value % 4));
        myGuiControlImage2.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(this.m_gamepadSlot.Value % 4);
        float num1 = 0.3f;
        MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage(new Vector2?(vector2_6 + new Vector2(num1 * vector2_8.X, -num1 * vector2_8.Y)), new Vector2?(vector2_8 / 3f));
        myGuiControlImage3.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(this.m_gamepadSlot.Value % 4));
        MyToolbarItem itemAtIndexGamepad = MyToolbarComponent.CurrentToolbar.GetItemAtIndexGamepad(this.m_gamepadSlot.Value % 4);
        string text3;
        string text4;
        if (itemAtIndexGamepad != null && itemAtIndexGamepad is MyToolbarItemTerminalBlock itemTerminalBlock)
        {
          text3 = itemTerminalBlock.GetBlockName();
          text4 = itemTerminalBlock.GetActionName();
        }
        else
        {
          text3 = (MyToolbarComponent.CurrentToolbar.GetItemNameGamepad(this.m_gamepadSlot.Value % 4) ?? string.Empty).Trim();
          text4 = string.Empty;
        }
        float num2 = 0.009f;
        Vector2 vector2_9 = new Vector2(0.032f, (float) (-(double) num2 - 11.0 / 1000.0));
        Vector2 vector2_10 = new Vector2(0.032f, num2 - 11f / 1000f);
        MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2?(vector2_6 + vector2_9), text: text3);
        MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(new Vector2?(vector2_6 + vector2_10), text: text4);
        this.Controls.Add((MyGuiControlBase) myGuiControlImage1);
        this.Controls.Add((MyGuiControlBase) myGuiControlImage2);
        this.Controls.Add((MyGuiControlBase) myGuiControlImage3);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
      }
      this.m_contextMenu = new MyGuiControlContextMenu();
      this.m_contextMenu.ItemClicked += new Action<MyGuiControlContextMenu, MyGuiControlContextMenu.EventArgs>(this.contextMenu_ItemClicked);
      this.m_contextMenu.OnDeactivated += new Action(this.contextMenu_Deactivated);
      this.Controls.Add((MyGuiControlBase) this.m_contextMenu);
      this.m_contextMenu.Deactivate();
    }

    public bool IsBuildPlannerShown() => this.m_blockGroupInfo.Visible;

    private void contextMenu_Deactivated() => this.FocusedControl = (MyGuiControlBase) this.m_gridBlocks;

    private List<MyGuiControlResearchGraph.GraphNode> CreateResearchGraph()
    {
      List<MyGuiControlResearchGraph.GraphNode> graphNodeList = new List<MyGuiControlResearchGraph.GraphNode>();
      Dictionary<string, MyGuiControlResearchGraph.GraphNode> nodesByName = new Dictionary<string, MyGuiControlResearchGraph.GraphNode>();
      List<MyGuiControlResearchGraph.GraphNode> children = new List<MyGuiControlResearchGraph.GraphNode>();
      HashSet<SerializableDefinitionId> serializableDefinitionIdSet = new HashSet<SerializableDefinitionId>();
      foreach (MyResearchGroupDefinition researchGroupDefinition in MyDefinitionManager.Static.GetResearchGroupDefinitions())
      {
        HashSet<string> stringSet = new HashSet<string>();
        List<MyCubeBlockDefinition> items = new List<MyCubeBlockDefinition>();
        if (researchGroupDefinition.Members != null)
        {
          foreach (SerializableDefinitionId member in researchGroupDefinition.Members)
          {
            MyCubeBlockDefinition blockDefinition;
            MyDefinitionManager.Static.TryGetCubeBlockDefinition((MyDefinitionId) member, out blockDefinition);
            if (blockDefinition != null && (blockDefinition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (blockDefinition.AvailableInSurvival || !MySession.Static.SurvivalMode))
            {
              MyResearchBlockDefinition researchBlock = MyDefinitionManager.Static.GetResearchBlock((MyDefinitionId) member);
              if (researchBlock != null && researchBlock.UnlockedByGroups != null && researchBlock.UnlockedByGroups.Length != 0)
              {
                foreach (string unlockedByGroup in researchBlock.UnlockedByGroups)
                  stringSet.Add(unlockedByGroup);
              }
              serializableDefinitionIdSet.Add(member);
              MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(blockDefinition.BlockPairName);
              if (definitionGroup != null && (definitionGroup.Large == null || definitionGroup.Small == null || !(definitionGroup.Small.Id == blockDefinition.Id)))
                items.Add(blockDefinition);
            }
          }
          if (stringSet.Count == 0)
          {
            MyGuiControlResearchGraph.GraphNode node = this.CreateNode(researchGroupDefinition, items);
            graphNodeList.Add(node);
            nodesByName.Add(node.Name, node);
          }
          else
          {
            foreach (string unlockedBy in stringSet)
            {
              MyGuiControlResearchGraph.GraphNode node = this.CreateNode(researchGroupDefinition, items, unlockedBy);
              children.Add(node);
              if (!nodesByName.ContainsKey(node.Name))
                nodesByName.Add(node.Name, node);
            }
          }
        }
      }
      Dictionary<string, MyGuiControlResearchGraph.GraphNode> dictionary = new Dictionary<string, MyGuiControlResearchGraph.GraphNode>();
      foreach (MyResearchBlockDefinition researchBlockDefinition in MyDefinitionManager.Static.GetResearchBlockDefinitions())
      {
        if (!serializableDefinitionIdSet.Contains((SerializableDefinitionId) researchBlockDefinition.Id))
        {
          MyCubeBlockDefinition blockDefinition;
          MyDefinitionManager.Static.TryGetCubeBlockDefinition(researchBlockDefinition.Id, out blockDefinition);
          if (blockDefinition != null && (blockDefinition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && ((blockDefinition.AvailableInSurvival || !MySession.Static.SurvivalMode) && researchBlockDefinition.UnlockedByGroups != null))
          {
            foreach (string unlockedByGroup in researchBlockDefinition.UnlockedByGroups)
            {
              MyGuiControlResearchGraph.GraphNode graphNode = (MyGuiControlResearchGraph.GraphNode) null;
              if (!nodesByName.TryGetValue(unlockedByGroup, out graphNode))
              {
                MyLog.Default.WriteLine(string.Format("Research group {0} was not found for block {1}.", (object) unlockedByGroup, (object) researchBlockDefinition.Id));
              }
              else
              {
                MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(blockDefinition.BlockPairName);
                if (definitionGroup != null && (definitionGroup.Large == null || definitionGroup.Small == null || !(definitionGroup.Small.Id == blockDefinition.Id)))
                {
                  MyGuiControlResearchGraph.GraphNode node = (MyGuiControlResearchGraph.GraphNode) null;
                  if (!dictionary.TryGetValue(unlockedByGroup, out node))
                  {
                    node = new MyGuiControlResearchGraph.GraphNode();
                    dictionary.Add(unlockedByGroup, node);
                    node.Name = "Common_" + unlockedByGroup;
                    node.UnlockedBy = unlockedByGroup;
                    graphNode.Children.Add(node);
                    node.Parent = graphNode;
                  }
                  this.CreateNodeItem(node, blockDefinition);
                }
              }
            }
          }
        }
      }
      MyGuiScreenToolbarConfigBase.CreateGraph(nodesByName, children);
      return graphNodeList;
    }

    private static void CreateGraph(
      Dictionary<string, MyGuiControlResearchGraph.GraphNode> nodesByName,
      List<MyGuiControlResearchGraph.GraphNode> children)
    {
      int num = 0;
      while (children.Count != num)
      {
        bool flag = false;
        foreach (MyGuiControlResearchGraph.GraphNode child in children)
        {
          if (!string.IsNullOrEmpty(child.UnlockedBy))
          {
            MyGuiControlResearchGraph.GraphNode graphNode = (MyGuiControlResearchGraph.GraphNode) null;
            if (nodesByName.TryGetValue(child.UnlockedBy, out graphNode))
            {
              ++num;
              graphNode.Children.Add(child);
              child.Parent = graphNode;
              flag = true;
            }
          }
        }
        if (!flag)
          break;
      }
    }

    private MyGuiControlResearchGraph.GraphNode CreateNode(
      MyResearchGroupDefinition group,
      List<MyCubeBlockDefinition> items,
      string unlockedBy = null)
    {
      MyGuiControlResearchGraph.GraphNode node = new MyGuiControlResearchGraph.GraphNode();
      node.Name = group.Id.SubtypeName;
      node.UnlockedBy = unlockedBy;
      foreach (MyCubeBlockDefinition definition in items)
        this.CreateNodeItem(node, definition);
      return node;
    }

    private void CreateNodeItem(
      MyGuiControlResearchGraph.GraphNode node,
      MyCubeBlockDefinition definition)
    {
      bool researched = !MySession.Static.ResearchEnabled || MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySessionComponentResearch.Static.CanUse(this.m_character ?? (this.m_shipController != null ? this.m_shipController.Pilot : (MyCharacter) null), definition.Id);
      string str1 = (string) null;
      if (definition.BlockStages != null && definition.BlockStages.Length != 0)
        str1 = MyGuiTextures.Static.GetTexture(MyHud.HudDefinition.Toolbar.ItemStyle.VariantTexture).Path;
      string str2 = (string) null;
      bool flag1 = true;
      if (definition.DLCs != null && definition.DLCs.Length != 0)
      {
        MyDLCs.MyDLC missingDefinitionDlc = MySession.Static.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC((MyDefinitionBase) definition, Sync.MyId);
        if (missingDefinitionDlc != null)
        {
          str2 = missingDefinitionDlc.Icon;
          flag1 = false;
        }
        else
        {
          MyDLCs.MyDLC dlc;
          if (MyDLCs.TryGetDLC(definition.DLCs[0], out dlc))
            str2 = dlc.Icon;
        }
      }
      bool flag2 = (MyToolbarComponent.GlobalBuilding || MySession.Static.ControlledEntity is MyCharacter || MySession.Static.ControlledEntity is MyCockpit && (MySession.Static.ControlledEntity as MyCockpit).BuildingMode) & researched & flag1;
      string[] icons = definition.Icons;
      string definitionTooltip = this.GetDefinitionTooltip((MyDefinitionBase) definition, researched);
      string subicon = str1;
      string toolTip = definitionTooltip;
      MyGuiScreenToolbarConfigBase.GridItemUserData gridItemUserData = new MyGuiScreenToolbarConfigBase.GridItemUserData();
      gridItemUserData.ItemData = (Func<MyObjectBuilder_ToolbarItem>) (() => MyToolbarItemFactory.ObjectBuilderFromDefinition((MyDefinitionBase) definition));
      int num = flag2 ? 1 : 0;
      MyGuiGridItem key = new MyGuiGridItem(icons, subicon, toolTip, (object) gridItemUserData, num != 0);
      key.SubIcon2 = str2;
      key.ItemDefinition = (MyDefinitionBase) definition;
      key.OverlayColorMask = new Vector4(0.0f, 1f, 0.0f, 0.25f);
      node.Items.Add(key);
      this.m_researchGraph.ItemToNode.Add(key, node);
    }

    private MyIdentity GetIdentity()
    {
      MyPlayer myPlayer = (MyPlayer) null;
      if (this.m_character != null)
        myPlayer = MyPlayer.GetPlayerFromCharacter(this.m_character);
      else if (this.m_shipController != null)
      {
        if (this.m_shipController.Pilot != null && this.m_shipController.ControllerInfo.Controller != null)
          myPlayer = this.m_shipController.ControllerInfo.Controller.Player;
      }
      else if (MySession.Static.LocalCharacter != null)
        myPlayer = MyPlayer.GetPlayerFromCharacter(MySession.Static.LocalCharacter);
      return myPlayer?.Identity;
    }

    private void SolveAspectRatio()
    {
      Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
      switch (MyVideoSettingsManager.GetClosestAspectRatio((float) fullscreenRectangle.Width / (float) fullscreenRectangle.Height))
      {
        case MyAspectRatioEnum.Normal_4_3:
        case MyAspectRatioEnum.Unsupported_5_4:
          this.m_gridBlocks.ColumnsCount = 8;
          MyGuiControlScrollablePanel gridBlocksPanel = this.m_gridBlocksPanel;
          gridBlocksPanel.Size = gridBlocksPanel.Size * new Vector2(0.82f, 1f);
          this.m_researchPanel.Size = this.m_gridBlocksPanel.Size;
          this.m_researchGraph.Size = new Vector2(0.4f, 0.0f);
          this.m_researchGraph.InvalidateItemsLayout();
          this.m_categoriesListbox.PositionX *= 0.9f;
          this.Controls.GetControlByName("BlockInfoPanel").PositionX *= 0.78f;
          ((MyGuiControlLabel) this.Controls.GetControlByName("CaptionLabel2")).PositionX *= 0.9f;
          ((MyGuiControlLabel) this.Controls.GetControlByName("LabelSubtitle")).PositionX *= 0.9f;
          this.m_searchBox.PositionX *= 0.68f;
          break;
      }
      this.CalculateBlockOffsets();
    }

    private void CalculateBlockOffsets()
    {
      this.m_blockOffsets.Clear();
      foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
      {
        if (MyFakes.ENABLE_MULTIBLOCKS_IN_SURVIVAL || !MySession.Static.SurvivalMode || !definitionPairName.EndsWith("MultiBlock"))
        {
          MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName);
          Vector2I blockScreenPosition = MyDefinitionManager.Static.GetCubeBlockScreenPosition(definitionGroup);
          if (this.IsBlockPairResearched(definitionGroup) && this.IsResearchedItemVisible(definitionGroup))
          {
            if (this.m_blockOffsets.Count <= blockScreenPosition.Y)
            {
              for (int index = this.m_blockOffsets.Count - 1; index < blockScreenPosition.Y; ++index)
                this.m_blockOffsets.Add(0);
            }
            if (blockScreenPosition.Y >= 0)
              this.m_blockOffsets[blockScreenPosition.Y]++;
          }
        }
      }
      int num1 = 0;
      for (int index = 0; index < this.m_blockOffsets.Count; ++index)
      {
        int num2 = (this.m_blockOffsets[index] - 1) / this.m_gridBlocks.ColumnsCount;
        num1 += num2;
        this.m_blockOffsets[index] = num1;
      }
    }

    private bool IsResearchedItemVisible(MyCubeBlockDefinitionGroup group)
    {
      bool flag1 = group.Any.BlockVariantsGroup?.PrimaryGUIBlock == group.Any;
      bool flag2 = this.IsEntireGroupResearched(group);
      if (group.AnyPublic == null)
        return false;
      if (!MyFakes.ENABLE_GUI_HIDDEN_CUBEBLOCKS || flag1 == flag2)
        return true;
      return flag1 && !flag2;
    }

    private bool IsEntireGroupResearched(MyCubeBlockDefinitionGroup group)
    {
      MyBlockVariantGroup blockVariantsGroup;
      if ((blockVariantsGroup = group.AnyPublic?.BlockVariantsGroup) != null)
      {
        foreach (MyCubeBlockDefinitionGroup blockGroup in blockVariantsGroup.BlockGroups)
        {
          if (!this.IsBlockPairResearched(blockGroup))
            return false;
        }
      }
      return true;
    }

    private void OnItemDragged(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs) => this.StartDragging(MyDropHandleType.MouseRelease, sender, ref eventArgs);

    protected void SelectCategories()
    {
      List<MyGuiControlListbox.Item> objList = new List<MyGuiControlListbox.Item>();
      if (MyGuiScreenToolbarConfigBase.m_allSelectedCategories.Count == 0 || MyGuiScreenToolbarConfigBase.m_ownerChanged)
      {
        objList.Add(this.m_categoriesListbox.Items[0]);
      }
      else
      {
        foreach (MyGuiControlListbox.Item obj in this.m_categoriesListbox.Items)
        {
          MyGuiControlListbox.Item item = obj;
          if (MyGuiScreenToolbarConfigBase.m_allSelectedCategories.Exists((Predicate<MyGuiBlockCategoryDefinition>) (x => x == item.UserData)))
            objList.Add(item);
        }
      }
      MyGuiScreenToolbarConfigBase.m_allSelectedCategories.Clear();
      this.m_categoriesListbox.SelectedItems = objList;
      this.categories_ItemClicked(this.m_categoriesListbox);
    }

    protected void SortCategoriesToDisplayList()
    {
      foreach (string key in this.m_forcedCategoryOrder)
      {
        MyGuiBlockCategoryDefinition categoryID = (MyGuiBlockCategoryDefinition) null;
        if (this.m_sortedCategories.TryGetValue(key, out categoryID))
          this.AddCategoryToDisplayList(categoryID.DisplayNameText, categoryID);
      }
      foreach (KeyValuePair<string, MyGuiBlockCategoryDefinition> sortedCategory in this.m_sortedCategories)
      {
        if (!this.m_forcedCategoryOrder.Contains<string>(sortedCategory.Key))
          this.AddCategoryToDisplayList(sortedCategory.Value.DisplayNameText, sortedCategory.Value);
      }
    }

    public void RecreateBlockCategories(
      Dictionary<string, MyGuiBlockCategoryDefinition> loadedCategories,
      SortedDictionary<string, MyGuiBlockCategoryDefinition> categories)
    {
      categories.Clear();
      foreach (KeyValuePair<string, MyGuiBlockCategoryDefinition> loadedCategory in loadedCategories)
        loadedCategory.Value.ValidItems = 0;
      if (MySession.Static.ResearchEnabled && !MySession.Static.CreativeToolsEnabled(Sync.MyId) && MySessionComponentResearch.Static.m_requiredResearch.Count > 0)
      {
        foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
        {
          MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName);
          if (this.IsValidItem(definitionGroup) && definitionGroup.AnyPublic != null && MySessionComponentResearch.Static.CanUse(this.m_character, definitionGroup.AnyPublic.Id))
          {
            foreach (MyGuiBlockCategoryDefinition categoryDefinition in loadedCategories.Values)
            {
              if (categoryDefinition.HasItem(definitionGroup.AnyPublic.Id.ToString()))
                ++categoryDefinition.ValidItems;
            }
          }
        }
      }
      MyPlayer myPlayer = (MyPlayer) null;
      if (this.m_shipController != null && this.m_shipController.BuildingMode)
      {
        if (this.m_shipController.Pilot != null)
          myPlayer = this.m_shipController.ControllerInfo.Controller.Player;
      }
      else
        myPlayer = MyPlayer.GetPlayerFromCharacter(this.m_character);
      if (myPlayer == null)
        return;
      foreach (KeyValuePair<string, MyGuiBlockCategoryDefinition> loadedCategory in loadedCategories)
      {
        if ((!MySession.Static.SurvivalMode || loadedCategory.Value.AvailableInSurvival || MySession.Static.IsUserAdmin(myPlayer.Client.SteamUserId)) && (!MySession.Static.CreativeMode || loadedCategory.Value.ShowInCreative) && ((this.m_character != null && MySession.Static.GetVoxelHandAvailable(this.m_character) || loadedCategory.Key.CompareTo("VoxelHands") != 0) && (MyGuiScreenToolbarConfigBase.GroupMode != MyGuiScreenToolbarConfigBase.GroupModes.HideBlockGroups || loadedCategory.Value.IsAnimationCategory || loadedCategory.Value.IsToolCategory)) && (MyGuiScreenToolbarConfigBase.GroupMode != MyGuiScreenToolbarConfigBase.GroupModes.HideEmpty || loadedCategory.Value.IsAnimationCategory || loadedCategory.Value.IsToolCategory || loadedCategory.Value.ItemIds.Count != 0 && (!MySession.Static.ResearchEnabled || MySession.Static.CreativeToolsEnabled(Sync.MyId) || (MySessionComponentResearch.Static.m_requiredResearch.Count <= 0 || loadedCategory.Value.ValidItems != 0))) && loadedCategory.Value.IsBlockCategory)
          categories.Add(loadedCategory.Value.Name, loadedCategory.Value);
      }
    }

    private void AddCategoryToDisplayList(
      string displayName,
      MyGuiBlockCategoryDefinition categoryID)
    {
      this.m_categoriesListbox.Add(new MyGuiControlListbox.Item(new StringBuilder(displayName), displayName, userData: ((object) categoryID)));
    }

    private void AddShipGunsToCategories(
      Dictionary<string, MyGuiBlockCategoryDefinition> loadedCategories,
      SortedDictionary<string, MyGuiBlockCategoryDefinition> categories)
    {
      if (this.m_shipController == null)
        return;
      foreach (KeyValuePair<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>> gunSet in this.m_shipController.CubeGrid.GridSystems.WeaponSystem.GetGunSets())
      {
        MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(gunSet.Key);
        foreach (KeyValuePair<string, MyGuiBlockCategoryDefinition> loadedCategory in loadedCategories)
        {
          if (loadedCategory.Value.IsShipCategory && loadedCategory.Value.HasItem(definition.Id.ToString()))
          {
            MyGuiBlockCategoryDefinition categoryDefinition = (MyGuiBlockCategoryDefinition) null;
            if (!categories.TryGetValue(loadedCategory.Value.Name, out categoryDefinition))
              categories.Add(loadedCategory.Value.Name, loadedCategory.Value);
          }
        }
      }
    }

    private void RecreateShipCategories(
      Dictionary<string, MyGuiBlockCategoryDefinition> loadedCategories,
      SortedDictionary<string, MyGuiBlockCategoryDefinition> categories,
      MyCubeGrid grid)
    {
      if (grid == null || grid.GridSystems.TerminalSystem == null || grid.GridSystems.TerminalSystem.BlockGroups == null)
        return;
      categories.Clear();
      MyTerminalBlock[] array = grid.GridSystems.TerminalSystem.Blocks.ToArray();
      Array.Sort<MyTerminalBlock>(array, (IComparer<MyTerminalBlock>) MyTerminalComparer.Static);
      List<string> stringList = new List<string>();
      foreach (MyTerminalBlock myTerminalBlock in array)
      {
        if (myTerminalBlock != null)
        {
          string str = myTerminalBlock.BlockDefinition.Id.ToString();
          if (!stringList.Contains(str))
            stringList.Add(str);
        }
      }
      foreach (string itemId in stringList)
      {
        foreach (KeyValuePair<string, MyGuiBlockCategoryDefinition> loadedCategory in loadedCategories)
        {
          if (loadedCategory.Value.IsShipCategory && loadedCategory.Value.HasItem(itemId) && loadedCategory.Value.SearchBlocks)
          {
            MyGuiBlockCategoryDefinition categoryDefinition = (MyGuiBlockCategoryDefinition) null;
            if (!categories.TryGetValue(loadedCategory.Value.Name, out categoryDefinition))
              categories.Add(loadedCategory.Value.Name, loadedCategory.Value);
          }
        }
      }
    }

    private void AddShipGroupsIntoCategoryList(MyCubeGrid grid)
    {
      if (grid == null || grid.GridSystems.TerminalSystem == null || grid.GridSystems.TerminalSystem.BlockGroups == null)
        return;
      MyBlockGroup[] array = grid.GridSystems.TerminalSystem.BlockGroups.ToArray();
      Array.Sort<MyBlockGroup>(array, (IComparer<MyBlockGroup>) MyTerminalComparer.Static);
      List<string> stringList = new List<string>();
      foreach (MyBlockGroup myBlockGroup in array)
      {
        if (myBlockGroup != null)
          stringList.Add(myBlockGroup.Name.ToString());
      }
      if (stringList.Count <= 0)
        return;
      this.m_shipGroupsCategory.DisplayNameString = MyTexts.GetString(MySpaceTexts.DisplayName_Category_ShipGroups);
      this.m_shipGroupsCategory.ItemIds = new HashSet<string>((IEnumerable<string>) stringList);
      this.m_shipGroupsCategory.SearchBlocks = false;
      this.m_shipGroupsCategory.Name = "Groups";
      this.m_sortedCategories.Add(this.m_shipGroupsCategory.Name, this.m_shipGroupsCategory);
    }

    public virtual bool AllowToolbarKeys() => !this.m_searchBox.TextBox.HasFocus;

    protected virtual void UpdateGridBlocksBySearchCondition(IMySearchCondition searchCondition)
    {
      searchCondition?.CleanDefinitionGroups();
      if ((this.m_shipController == null ? 0 : (!this.m_shipController.EnableShipControl ? 1 : 0)) == 0)
      {
        if (this.m_character != null || this.m_shipController != null && this.m_shipController.BuildingMode)
          this.AddCubeDefinitionsToBlocks(searchCondition);
        else if (this.m_screenCubeGrid != null)
          this.AddShipBlocksDefinitions(this.m_screenCubeGrid, true, searchCondition);
      }
      this.m_gridBlocks.SelectedIndex = new int?(0);
      this.m_gridBlocksPanel.ScrollbarVPosition = 0.0f;
    }

    protected virtual void AddToolsAndAnimations(IMySearchCondition searchCondition)
    {
      if (this.m_character != null)
      {
        MyCharacter character = this.m_character;
        foreach (MyDefinitionBase weaponDefinition in MyDefinitionManager.Static.GetWeaponDefinitions())
        {
          if ((searchCondition == null || searchCondition.MatchesCondition(weaponDefinition)) && weaponDefinition.Public)
          {
            MyInventory inventory = MyEntityExtensions.GetInventory(character);
            bool enabled = (weaponDefinition.Id.SubtypeId == this.manipulationToolId || inventory != null && inventory.ContainItems(new MyFixedPoint?((MyFixedPoint) 1), weaponDefinition.Id)) | MySession.Static.CreativeMode;
            if (enabled || MyPerGameSettings.Game == GameEnum.SE_GAME)
              this.AddWeaponDefinition(this.m_gridBlocks, weaponDefinition, enabled);
          }
        }
        foreach (MyDefinitionBase consumableDefinition in MyDefinitionManager.Static.GetConsumableDefinitions())
        {
          if ((searchCondition == null || searchCondition.MatchesCondition(consumableDefinition)) && consumableDefinition.Public)
          {
            MyInventory inventory = MyEntityExtensions.GetInventory(character);
            bool enabled = (consumableDefinition.Id.SubtypeId == this.manipulationToolId || inventory != null && inventory.ContainItems(new MyFixedPoint?((MyFixedPoint) 1), consumableDefinition.Id)) | MySession.Static.CreativeMode;
            if (enabled || MyPerGameSettings.Game == GameEnum.SE_GAME)
              this.AddConsumableDefinition(this.m_gridBlocks, consumableDefinition, enabled);
          }
        }
        if (MyPerGameSettings.EnableAi && MyFakes.ENABLE_BARBARIANS)
        {
          this.AddAiCommandDefinitions(searchCondition);
          this.AddBotDefinitions(searchCondition);
        }
        if (MySession.Static.GetVoxelHandAvailable(character))
          this.AddVoxelHands(searchCondition);
        if (MyFakes.ENABLE_PREFAB_THROWER)
          this.AddPrefabThrowers(searchCondition);
        this.AddAnimations(false, searchCondition);
        this.AddEmotes(false, searchCondition);
        this.AddGridCreators(searchCondition);
      }
      else
      {
        if (this.m_screenOwner == null)
          return;
        this.AddTerminalGroupsToGridBlocks(this.m_screenCubeGrid, this.m_screenOwner.EntityId, searchCondition);
        if (this.m_shipController == null)
          return;
        if (this.m_shipController.EnableShipControl)
          this.AddTools(this.m_shipController, searchCondition);
        this.AddAnimations(true, searchCondition);
        this.AddEmotes(true, searchCondition);
      }
    }

    private bool IsValidItem(MyCubeBlockDefinitionGroup item)
    {
      if (this.IsBlockPairResearched(item))
        return true;
      MyBlockVariantGroup blockVariantsGroup;
      if ((blockVariantsGroup = item.AnyPublic?.BlockVariantsGroup) != null)
      {
        foreach (MyCubeBlockDefinitionGroup blockGroup in blockVariantsGroup.BlockGroups)
        {
          if (blockGroup != item && this.IsBlockPairResearched(blockGroup))
            return true;
        }
      }
      return false;
    }

    private bool IsBlockPairResearched(MyCubeBlockDefinitionGroup group)
    {
      for (int index = 0; index < group.SizeCount; ++index)
      {
        MyCubeBlockDefinition block = group[(MyCubeSize) index];
        if ((MyFakes.ENABLE_NON_PUBLIC_BLOCKS || block != null && block.Public && block.Enabled) && this.IsBlockResearched(block))
          return true;
      }
      return false;
    }

    private bool IsBlockResearched(MyCubeBlockDefinition block) => !MySession.Static.ResearchEnabled || MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySessionComponentResearch.Static.CanUse(this.m_character ?? this.m_shipController?.Pilot, block.Id);

    private bool HasDLCs(MyDefinitionBase definition)
    {
      MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
      if (!(definition is MyCubeBlockDefinition cubeBlockDefinition) || cubeBlockDefinition.BlockVariantsGroup == null || cubeBlockDefinition.BlockStages == null)
        return component.GetFirstMissingDefinitionDLC(definition, Sync.MyId) == null;
      foreach (MyCubeBlockDefinition block in cubeBlockDefinition.BlockVariantsGroup.Blocks)
      {
        if (component.GetFirstMissingDefinitionDLC((MyDefinitionBase) block, Sync.MyId) == null && this.IsBlockResearched(block))
          return true;
      }
      return false;
    }

    protected void AddDefinition(
      MyGuiControlGrid grid,
      MyObjectBuilder_ToolbarItem data,
      MyDefinitionBase definition,
      bool enabled = true)
    {
      if (!definition.Public && !MyFakes.ENABLE_NON_PUBLIC_BLOCKS || !definition.AvailableInSurvival && MySession.Static.SurvivalMode)
        return;
      bool researched = true;
      if (definition is MyCubeBlockDefinition cubeBlockDefinition)
        researched = this.IsValidItem(MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName));
      enabled &= researched;
      enabled &= this.HasDLCs(definition);
      string str = (string) null;
      if (definition.DLCs != null && definition.DLCs.Length != 0)
      {
        MyDLCs.MyDLC missingDefinitionDlc = MySession.Static.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(definition, Sync.MyId);
        if (missingDefinitionDlc != null)
        {
          enabled = false;
          str = missingDefinitionDlc.Icon;
        }
        else
        {
          MyDLCs.MyDLC dlc;
          if (MyDLCs.TryGetDLC(definition.DLCs[0], out dlc))
            str = dlc.Icon;
        }
      }
      string[] icons = definition.Icons;
      string definitionTooltip = this.GetDefinitionTooltip(definition, researched);
      bool flag = enabled;
      MyGuiScreenToolbarConfigBase.GridItemUserData gridItemUserData = new MyGuiScreenToolbarConfigBase.GridItemUserData();
      gridItemUserData.ItemData = (Func<MyObjectBuilder_ToolbarItem>) (() => data);
      int num = flag ? 1 : 0;
      grid.Add(new MyGuiGridItem(icons, (string) null, definitionTooltip, (object) gridItemUserData, num != 0, 1f)
      {
        SubIcon2 = str
      });
    }

    protected void AddDefinitionAtPosition(
      MyGuiControlGrid grid,
      MyCubeBlockDefinition block,
      Vector2I position,
      bool enabled = true,
      string subicon = null)
    {
      if (block == null || !block.Public && !MyFakes.ENABLE_NON_PUBLIC_BLOCKS || !block.AvailableInSurvival && MySession.Static.SurvivalMode)
        return;
      bool researched = this.IsValidItem(MyDefinitionManager.Static.GetDefinitionGroup(block.BlockPairName));
      enabled &= researched;
      enabled &= this.HasDLCs((MyDefinitionBase) block);
      string str = (string) null;
      MyDLCs.MyDLC dlc;
      if (!block.DLCs.IsNullOrEmpty<string>() && MyDLCs.TryGetDLC(block.DLCs[0], out dlc))
        str = dlc.Icon;
      string[] icons = block.Icons;
      string definitionTooltip = this.GetDefinitionTooltip((MyDefinitionBase) block, researched);
      string subicon1 = subicon;
      string toolTip = definitionTooltip;
      MyGuiScreenToolbarConfigBase.GridItemUserData gridItemUserData = new MyGuiScreenToolbarConfigBase.GridItemUserData();
      gridItemUserData.ItemData = (Func<MyObjectBuilder_ToolbarItem>) (() => MyToolbarItemFactory.ObjectBuilderFromDefinition((MyDefinitionBase) block));
      int num1 = enabled ? 1 : 0;
      MyGuiGridItem gridItem = new MyGuiGridItem(icons, subicon1, toolTip, (object) gridItemUserData, num1 != 0);
      gridItem.SubIcon2 = str;
      int num2 = -position.Y - 1;
      if (position.Y < 0)
        grid.Add(gridItem, num2 * 6);
      else if (grid.IsValidIndex(position.Y, position.X))
      {
        this.SetOrReplaceItemOnPosition(grid, gridItem, position);
      }
      else
      {
        if (!grid.IsValidIndex(0, position.X))
          return;
        grid.RecalculateRowsCount();
        grid.AddRows(position.Y - grid.RowsCount + 1);
        this.SetOrReplaceItemOnPosition(grid, gridItem, position);
      }
    }

    private string GetDefinitionTooltip(MyDefinitionBase definition, bool researched)
    {
      StringBuilder stringBuilder = new StringBuilder(definition.DisplayNameText);
      if (!researched)
      {
        stringBuilder.Append("\n").Append(MyTexts.GetString(MyCommonTexts.ScreenCubeBuilderRequiresResearch)).Append(" ");
        if (definition is MyCubeBlockDefinition cubeBlockDefinition)
        {
          MyResearchBlockDefinition researchBlock = MyDefinitionManager.Static.GetResearchBlock(cubeBlockDefinition.Id);
          if (researchBlock != null)
          {
            foreach (string unlockedByGroup in researchBlock.UnlockedByGroups)
            {
              MyResearchGroupDefinition researchGroup = MyDefinitionManager.Static.GetResearchGroup(unlockedByGroup);
              if (researchGroup != null)
              {
                foreach (SerializableDefinitionId member in researchGroup.Members)
                {
                  MyDefinitionBase definition1;
                  if (MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>((MyDefinitionId) member, out definition1) && !this.m_tmpUniqueStrings.Contains(definition1.DisplayNameText))
                  {
                    stringBuilder.Append("\n");
                    stringBuilder.Append(definition1.DisplayNameText);
                    this.m_tmpUniqueStrings.Add(definition1.DisplayNameText);
                  }
                }
              }
            }
          }
        }
        this.m_tmpUniqueStrings.Clear();
      }
      if (!definition.DLCs.IsNullOrEmpty<string>() && MySession.Static.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(definition, Sync.MyId) != null)
      {
        stringBuilder.Append("\n");
        for (int index = 0; index < definition.DLCs.Length; ++index)
        {
          stringBuilder.Append("\n");
          stringBuilder.Append(MyDLCs.GetRequiredDLCTooltip(definition.DLCs[index]));
        }
      }
      return stringBuilder.ToString();
    }

    private void SetOrReplaceItemOnPosition(
      MyGuiControlGrid grid,
      MyGuiGridItem gridItem,
      Vector2I position)
    {
      MyGuiGridItem itemAt = grid.TryGetItemAt(position.Y, position.X);
      grid.SetItemAt(position.Y, position.X, gridItem);
      if (itemAt == null)
        return;
      grid.Add(itemAt, position.Y);
    }

    private void AddCubeDefinitionsToBlocks(IMySearchCondition searchCondition)
    {
      foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
      {
        if (MyFakes.ENABLE_MULTIBLOCKS_IN_SURVIVAL || !MySession.Static.SurvivalMode || !definitionPairName.EndsWith("MultiBlock"))
        {
          MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName);
          Vector2I blockScreenPosition = MyDefinitionManager.Static.GetCubeBlockScreenPosition(definitionGroup);
          if (this.IsBlockPairResearched(definitionGroup))
          {
            if (searchCondition != null)
            {
              bool flag1 = false;
              for (int index1 = 0; index1 < definitionGroup.SizeCount && !flag1; ++index1)
              {
                MyCubeBlockDefinition cubeBlockDefinition1 = definitionGroup[(MyCubeSize) index1];
                if ((MyFakes.ENABLE_NON_PUBLIC_BLOCKS || cubeBlockDefinition1 != null && cubeBlockDefinition1.Public && cubeBlockDefinition1.Enabled) && cubeBlockDefinition1 != null)
                {
                  bool flag2 = searchCondition.MatchesCondition((MyDefinitionBase) cubeBlockDefinition1);
                  if (flag2 && (!MyFakes.ENABLE_GUI_HIDDEN_CUBEBLOCKS || cubeBlockDefinition1.GuiVisible || searchCondition is MySearchByStringCondition))
                  {
                    flag1 = true;
                    break;
                  }
                  if (searchCondition is MySearchByCategoryCondition categoryCondition)
                  {
                    if (categoryCondition.StrictSearch)
                    {
                      if (flag2)
                      {
                        flag1 = true;
                        break;
                      }
                    }
                    else if (cubeBlockDefinition1.BlockStages != null && cubeBlockDefinition1.BlockStages.Length != 0)
                    {
                      for (int index2 = 0; index2 < cubeBlockDefinition1.BlockStages.Length; ++index2)
                      {
                        MyCubeBlockDefinition cubeBlockDefinition2 = MyDefinitionManager.Static.GetCubeBlockDefinition(cubeBlockDefinition1.BlockStages[index2]);
                        if (cubeBlockDefinition2 != null && searchCondition.MatchesCondition((MyDefinitionBase) cubeBlockDefinition2))
                        {
                          flag1 = true;
                          break;
                        }
                      }
                    }
                    else if (searchCondition.MatchesCondition((MyDefinitionBase) cubeBlockDefinition1))
                      flag1 = true;
                  }
                }
              }
              if (flag1)
                searchCondition.AddDefinitionGroup(definitionGroup);
            }
            else if (this.IsResearchedItemVisible(definitionGroup))
            {
              if (blockScreenPosition.Y > 0 && blockScreenPosition.Y < this.m_blockOffsets.Count)
              {
                blockScreenPosition.Y += this.m_blockOffsets[blockScreenPosition.Y - 1];
                blockScreenPosition.Y += blockScreenPosition.X / this.m_gridBlocks.ColumnsCount;
                blockScreenPosition.X %= this.m_gridBlocks.ColumnsCount;
              }
              this.AddCubeDefinition(this.m_gridBlocks, definitionGroup, blockScreenPosition);
            }
          }
        }
      }
      if (searchCondition != null)
      {
        HashSet<MyCubeBlockDefinitionGroup> sortedBlocks = searchCondition.GetSortedBlocks();
        int num = 0;
        foreach (MyCubeBlockDefinitionGroup group in sortedBlocks)
        {
          Vector2I position;
          position.X = num % this.m_gridBlocks.ColumnsCount;
          position.Y = (int) ((double) num / (double) this.m_gridBlocks.ColumnsCount);
          MyCubeBlockDefinition block = MyFakes.ENABLE_NON_PUBLIC_BLOCKS ? group.Any : group.AnyPublic;
          if (this.IsBlockResearched(block))
          {
            ++num;
            this.AddCubeDefinition(this.m_gridBlocks, group, position);
          }
          if (block.BlockStages != null && block.BlockStages.Length != 0 && (searchCondition is MySearchByCategoryCondition categoryCondition && !categoryCondition.StrictSearch))
          {
            foreach (MyDefinitionId blockStage in block.BlockStages)
            {
              bool flag = true;
              MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(blockStage);
              if (cubeBlockDefinition != null && this.IsBlockResearched(cubeBlockDefinition))
              {
                foreach (MyCubeBlockDefinitionGroup blockDefinitionGroup in sortedBlocks)
                {
                  if (group != blockDefinitionGroup && (blockDefinitionGroup.Small == cubeBlockDefinition || blockDefinitionGroup.Large == cubeBlockDefinition))
                  {
                    flag = false;
                    break;
                  }
                }
                if (flag)
                {
                  position.X = num % this.m_gridBlocks.ColumnsCount;
                  position.Y = (int) ((double) num / (double) this.m_gridBlocks.ColumnsCount);
                  ++num;
                  this.AddDefinitionAtPosition(this.m_gridBlocks, cubeBlockDefinition, position);
                }
              }
            }
          }
        }
      }
      else
      {
        int num = 0;
        int colIdx1 = int.MaxValue;
        for (int rowIdx = 0; rowIdx < this.m_gridBlocks.RowsCount; ++rowIdx)
        {
          for (int colIdx2 = 0; colIdx2 < this.m_gridBlocks.ColumnsCount; ++colIdx2)
          {
            MyGuiGridItem itemAt = this.m_gridBlocks.TryGetItemAt(rowIdx, colIdx2);
            if (itemAt != null && colIdx1 != int.MaxValue)
            {
              this.m_gridBlocks.SetItemAt(rowIdx, colIdx2, (MyGuiGridItem) null);
              this.m_gridBlocks.SetItemAt(rowIdx, colIdx1, itemAt);
              ++colIdx1;
            }
            else if (itemAt == null && colIdx1 > colIdx2)
              colIdx1 = colIdx2;
          }
          if (colIdx1 == 0)
            ++num;
          else if (num > 0)
          {
            for (int colIdx2 = 0; colIdx2 < this.m_gridBlocks.ColumnsCount; ++colIdx2)
            {
              MyGuiGridItem itemAt = this.m_gridBlocks.TryGetItemAt(rowIdx, colIdx2);
              this.m_gridBlocks.SetItemAt(rowIdx, colIdx2, (MyGuiGridItem) null);
              this.m_gridBlocks.SetItemAt(rowIdx - num, colIdx2, itemAt);
            }
          }
          colIdx1 = int.MaxValue;
        }
        if (num <= 0)
          return;
        int count = num * this.m_gridBlocks.ColumnsCount;
        this.m_gridBlocks.Items.RemoveRange(this.m_gridBlocks.Items.Count - count, count);
        this.m_gridBlocks.RowsCount -= num;
      }
    }

    private void AddCubeDefinition(
      MyGuiControlGrid grid,
      MyCubeBlockDefinitionGroup group,
      Vector2I position)
    {
      MyCubeBlockDefinition block = MyFakes.ENABLE_NON_PUBLIC_BLOCKS ? group.Any : group.AnyPublic;
      if (!MyFakes.ENABLE_MULTIBLOCK_CONSTRUCTION && MySession.Static.SurvivalMode && block.MultiBlock != null)
        return;
      MyBlockVariantGroup blockVariantsGroup = block.BlockVariantsGroup;
      bool flag = blockVariantsGroup != null ? blockVariantsGroup.BlockGroups.Length > 1 && blockVariantsGroup.PrimaryGUIBlock == block : block.BlockStages != null && (uint) block.BlockStages.Length > 0U;
      string subicon = (string) null;
      if (flag)
        subicon = MyGuiTextures.Static.GetTexture(MyHud.HudDefinition.Toolbar.ItemStyle.VariantTexture).Path;
      bool enabled = MyToolbarComponent.GlobalBuilding || MySession.Static.ControlledEntity is MyCharacter || MySession.Static.ControlledEntity is MyCockpit controlledEntity && controlledEntity.BuildingMode;
      this.AddDefinitionAtPosition(grid, block, position, enabled, subicon);
    }

    private void AddGridGun(
      MyShipController shipController,
      MyDefinitionId gunId,
      IMySearchCondition searchCondition)
    {
      MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(gunId);
      if (searchCondition != null && !searchCondition.MatchesCondition(definition))
        return;
      this.AddWeaponDefinition(this.m_gridBlocks, definition);
    }

    private void AddTools(MyShipController shipController, IMySearchCondition searchCondition)
    {
      if (shipController?.CubeGrid?.GridSystems?.WeaponSystem?.GetGunSets() == null)
        return;
      foreach (KeyValuePair<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>> gunSet in shipController.CubeGrid.GridSystems.WeaponSystem.GetGunSets())
        this.AddGridGun(shipController, gunSet.Key, searchCondition);
    }

    private void AddAnimations(bool shipController, IMySearchCondition searchCondition)
    {
      foreach (MyAnimationDefinition animationDefinition in MyDefinitionManager.Static.GetAnimationDefinitions())
      {
        if (animationDefinition.Public && (!shipController || shipController && animationDefinition.AllowInCockpit) && (searchCondition == null || searchCondition.MatchesCondition((MyDefinitionBase) animationDefinition)))
          this.AddAnimationDefinition(this.m_gridBlocks, (MyDefinitionBase) animationDefinition);
      }
    }

    private void AddEmotes(bool shipController, IMySearchCondition searchCondition)
    {
      IEnumerable<MyGameInventoryItemDefinition> definitionsForSlot = MyGameService.GetDefinitionsForSlot(MyGameInventoryItemSlot.Emote);
      if (definitionsForSlot == null)
        return;
      foreach (MyGameInventoryItemDefinition inventoryItemDefinition in (IEnumerable<MyGameInventoryItemDefinition>) definitionsForSlot.OrderBy<MyGameInventoryItemDefinition, string>((Func<MyGameInventoryItemDefinition, string>) (e => e.Name)))
      {
        MyEmoteDefinition definition = MyDefinitionManager.Static.GetDefinition<MyEmoteDefinition>(inventoryItemDefinition.AssetModifierId);
        if (definition != null && definition.Public)
        {
          MyAnimationDefinition animationDefinition = MyDefinitionManager.Static.TryGetAnimationDefinition(definition.AnimationId.SubtypeName);
          if (animationDefinition != null && (!shipController || shipController && animationDefinition.AllowInCockpit) && (searchCondition == null || searchCondition.MatchesCondition((MyDefinitionBase) definition)))
            this.AddEmoteDefinition(this.m_gridBlocks, (MyDefinitionBase) definition, MyGameService.HasInventoryItemWithDefinitionId(inventoryItemDefinition.ID));
        }
      }
    }

    private void AddVoxelHands(IMySearchCondition searchCondition)
    {
      foreach (MyVoxelHandDefinition voxelHandDefinition in MyDefinitionManager.Static.GetVoxelHandDefinitions())
      {
        if (voxelHandDefinition.Public && (searchCondition == null || searchCondition.MatchesCondition((MyDefinitionBase) voxelHandDefinition)))
          this.AddVoxelHandDefinition(this.m_gridBlocks, (MyDefinitionBase) voxelHandDefinition);
      }
    }

    private void AddGridCreators(IMySearchCondition searchCondition)
    {
      foreach (MyGridCreateToolDefinition creatorDefinition in MyDefinitionManager.Static.GetGridCreatorDefinitions())
      {
        if (creatorDefinition.Public && (searchCondition == null || searchCondition.MatchesCondition((MyDefinitionBase) creatorDefinition)))
          this.AddGridCreatorDefinition(this.m_gridBlocks, (MyDefinitionBase) creatorDefinition);
      }
    }

    private void AddPrefabThrowers(IMySearchCondition searchCondition)
    {
      foreach (MyPrefabThrowerDefinition throwerDefinition in MyDefinitionManager.Static.GetPrefabThrowerDefinitions())
      {
        if ((throwerDefinition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (searchCondition == null || searchCondition.MatchesCondition((MyDefinitionBase) throwerDefinition)))
          this.AddPrefabThrowerDefinition(this.m_gridBlocks, throwerDefinition);
      }
    }

    private void AddBotDefinitions(IMySearchCondition searchCondition)
    {
      foreach (MyBotDefinition definition in MyDefinitionManager.Static.GetDefinitionsOfType<MyBotDefinition>())
      {
        if ((definition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (definition.AvailableInSurvival || MySession.Static.CreativeMode) && (searchCondition == null || searchCondition.MatchesCondition((MyDefinitionBase) definition)))
          this.AddBotDefinition(this.m_gridBlocks, definition);
      }
    }

    private void AddAiCommandDefinitions(IMySearchCondition searchCondition)
    {
      foreach (MyAiCommandDefinition commandDefinition in MyDefinitionManager.Static.GetDefinitionsOfType<MyAiCommandDefinition>())
      {
        if ((commandDefinition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (commandDefinition.AvailableInSurvival || MySession.Static.CreativeMode) && (searchCondition == null || searchCondition.MatchesCondition((MyDefinitionBase) commandDefinition)))
          this.AddToolbarItemDefinition<MyObjectBuilder_ToolbarItemAiCommand>(this.m_gridBlocks, (MyDefinitionBase) commandDefinition);
      }
    }

    private void AddWeaponDefinition(
      MyGuiControlGrid grid,
      MyDefinitionBase definition,
      bool enabled = true)
    {
      if (!definition.Public && !MyFakes.ENABLE_NON_PUBLIC_BLOCKS || !definition.AvailableInSurvival && MySession.Static.SurvivalMode)
        return;
      MyObjectBuilder_ToolbarItemWeapon weaponData = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemWeapon>();
      weaponData.DefinitionId = (SerializableDefinitionId) definition.Id;
      string[] icons = definition.Icons;
      string displayNameText = definition.DisplayNameText;
      MyGuiScreenToolbarConfigBase.GridItemUserData gridItemUserData = new MyGuiScreenToolbarConfigBase.GridItemUserData();
      gridItemUserData.ItemData = (Func<MyObjectBuilder_ToolbarItem>) (() => (MyObjectBuilder_ToolbarItem) weaponData);
      int num = enabled ? 1 : 0;
      MyGuiGridItem myGuiGridItem = new MyGuiGridItem(icons, (string) null, displayNameText, (object) gridItemUserData, num != 0, 1f);
      grid.Add(myGuiGridItem);
    }

    private void AddConsumableDefinition(
      MyGuiControlGrid grid,
      MyDefinitionBase definition,
      bool enabled = true)
    {
      if (!definition.Public && !MyFakes.ENABLE_NON_PUBLIC_BLOCKS || !definition.AvailableInSurvival && MySession.Static.SurvivalMode)
        return;
      MyObjectBuilder_ToolbarItemConsumable consumableData = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemConsumable>();
      consumableData.DefinitionId = (SerializableDefinitionId) definition.Id;
      string[] icons = definition.Icons;
      string displayNameText = definition.DisplayNameText;
      MyGuiScreenToolbarConfigBase.GridItemUserData gridItemUserData = new MyGuiScreenToolbarConfigBase.GridItemUserData();
      gridItemUserData.ItemData = (Func<MyObjectBuilder_ToolbarItem>) (() => (MyObjectBuilder_ToolbarItem) consumableData);
      int num = enabled ? 1 : 0;
      MyGuiGridItem myGuiGridItem = new MyGuiGridItem(icons, (string) null, displayNameText, (object) gridItemUserData, num != 0, 1f);
      grid.Add(myGuiGridItem);
    }

    private void AddAnimationDefinition(MyGuiControlGrid grid, MyDefinitionBase definition)
    {
      MyObjectBuilder_ToolbarItemAnimation newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemAnimation>();
      newObject.DefinitionId = (SerializableDefinitionId) definition.Id;
      this.AddDefinition(grid, (MyObjectBuilder_ToolbarItem) newObject, definition);
    }

    private void AddEmoteDefinition(
      MyGuiControlGrid grid,
      MyDefinitionBase definition,
      bool enabled = true)
    {
      MyObjectBuilder_ToolbarItemEmote newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemEmote>();
      newObject.DefinitionId = (SerializableDefinitionId) definition.Id;
      this.AddDefinition(grid, (MyObjectBuilder_ToolbarItem) newObject, definition, enabled);
    }

    private void AddVoxelHandDefinition(MyGuiControlGrid grid, MyDefinitionBase definition)
    {
      MyObjectBuilder_ToolbarItemVoxelHand newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemVoxelHand>();
      newObject.DefinitionId = (SerializableDefinitionId) definition.Id;
      this.AddDefinition(grid, (MyObjectBuilder_ToolbarItem) newObject, definition);
    }

    private void AddGridCreatorDefinition(MyGuiControlGrid grid, MyDefinitionBase definition)
    {
      MyObjectBuilder_ToolbarItemCreateGrid newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemCreateGrid>();
      newObject.DefinitionId = (SerializableDefinitionId) definition.Id;
      this.AddDefinition(grid, (MyObjectBuilder_ToolbarItem) newObject, definition);
    }

    private void AddPrefabThrowerDefinition(
      MyGuiControlGrid grid,
      MyPrefabThrowerDefinition definition)
    {
      MyObjectBuilder_ToolbarItemPrefabThrower newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemPrefabThrower>();
      newObject.DefinitionId = (SerializableDefinitionId) definition.Id;
      this.AddDefinition(grid, (MyObjectBuilder_ToolbarItem) newObject, (MyDefinitionBase) definition);
    }

    private void AddBotDefinition(MyGuiControlGrid grid, MyBotDefinition definition)
    {
      MyObjectBuilder_ToolbarItemBot newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemBot>();
      newObject.DefinitionId = (SerializableDefinitionId) definition.Id;
      this.AddDefinition(grid, (MyObjectBuilder_ToolbarItem) newObject, (MyDefinitionBase) definition);
    }

    private void AddToolbarItemDefinition<T>(MyGuiControlGrid grid, MyDefinitionBase definition) where T : MyObjectBuilder_ToolbarItemDefinition, new()
    {
      T newObject = MyObjectBuilderSerializer.CreateNewObject<T>();
      newObject.DefinitionId = (SerializableDefinitionId) definition.Id;
      this.AddDefinition(grid, (MyObjectBuilder_ToolbarItem) newObject, definition);
    }

    private void AddShipBlocksDefinitions(
      MyCubeGrid grid,
      bool isShip,
      IMySearchCondition searchCondition)
    {
      if ((!isShip || this.m_shipController == null ? 0 : (!this.m_shipController.EnableShipControl ? 1 : 0)) != 0 || !MyFakes.ENABLE_SHIP_BLOCKS_TOOLBAR)
        return;
      this.AddTerminalSingleBlocksToGridBlocks(grid, searchCondition);
    }

    private void AddTerminalGroupsToGridBlocks(
      MyCubeGrid grid,
      long Owner,
      IMySearchCondition searchCondition)
    {
      if (grid == null || grid.GridSystems.TerminalSystem == null || grid.GridSystems.TerminalSystem.BlockGroups == null)
        return;
      int num1 = 0;
      int columnsCount = this.m_gridBlocks.ColumnsCount;
      MyBlockGroup[] array = grid.GridSystems.TerminalSystem.BlockGroups.ToArray();
      Array.Sort<MyBlockGroup>(array, (IComparer<MyBlockGroup>) MyTerminalComparer.Static);
      foreach (MyBlockGroup group in array)
      {
        if (searchCondition == null || searchCondition.MatchesCondition(group.Name.ToString()))
        {
          MyObjectBuilder_ToolbarItemTerminalGroup groupData = MyToolbarItemFactory.TerminalGroupObjectBuilderFromGroup(group);
          bool flag = false;
          foreach (MyCubeBlock block in group.Blocks)
          {
            if (block.IsFunctional)
            {
              flag = true;
              break;
            }
          }
          groupData.BlockEntityId = Owner;
          MyGuiControlGrid gridBlocks = this.m_gridBlocks;
          string[] forTerminalGroup = MyToolbarItemFactory.GetIconForTerminalGroup(group);
          string toolTip = group.Name.ToString();
          MyGuiScreenToolbarConfigBase.GridItemUserData gridItemUserData = new MyGuiScreenToolbarConfigBase.GridItemUserData();
          gridItemUserData.ItemData = (Func<MyObjectBuilder_ToolbarItem>) (() => (MyObjectBuilder_ToolbarItem) groupData);
          int num2 = flag ? 1 : 0;
          MyGuiGridItem myGuiGridItem = new MyGuiGridItem(forTerminalGroup, (string) null, toolTip, (object) gridItemUserData, num2 != 0, 1f);
          gridBlocks.Add(myGuiGridItem);
          ++num1;
        }
      }
      if (num1 <= 0)
        return;
      int num3 = num1 % columnsCount;
      if (num3 == 0)
        num3 = columnsCount;
      for (int index = 0; index < 2 * columnsCount - num3; ++index)
      {
        if (num1 < this.m_gridBlocks.GetItemsCount())
          this.m_gridBlocks.SetItemAt(num1++, new MyGuiGridItem("", (string) null, string.Empty, (object) new MyGuiScreenToolbarConfigBase.GridItemUserData()
          {
            ItemData = (Func<MyObjectBuilder_ToolbarItem>) (() => (MyObjectBuilder_ToolbarItem) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemEmpty>())
          }, false, 1f));
        else
          this.m_gridBlocks.Add(new MyGuiGridItem("", (string) null, string.Empty, (object) new MyGuiScreenToolbarConfigBase.GridItemUserData()
          {
            ItemData = (Func<MyObjectBuilder_ToolbarItem>) (() => (MyObjectBuilder_ToolbarItem) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemEmpty>())
          }, false, 1f));
      }
    }

    private void AddTerminalSingleBlocksToGridBlocks(
      MyCubeGrid grid,
      IMySearchCondition searchCondition)
    {
      if (grid == null || grid.GridSystems.TerminalSystem == null)
        return;
      MyTerminalBlock[] array = grid.GridSystems.TerminalSystem.Blocks.ToArray();
      Array.Sort<MyTerminalBlock>(array, (IComparer<MyTerminalBlock>) MyTerminalComparer.Static);
      foreach (MyTerminalBlock block in array)
      {
        if (block != null && (MyTerminalControlFactory.GetActions(block.GetType()).Count > 0 && (searchCondition == null || searchCondition.MatchesCondition((MyDefinitionBase) block.BlockDefinition) || searchCondition.MatchesCondition(block.CustomName.ToString())) && (block.ShowInToolbarConfig && (block.BlockDefinition.AvailableInSurvival || !MySession.Static.SurvivalMode))))
        {
          MyObjectBuilder_ToolbarItemTerminalBlock blockData = MyToolbarItemFactory.TerminalBlockObjectBuilderFromBlock(block);
          MyGuiControlGrid gridBlocks = this.m_gridBlocks;
          string[] icons = block.BlockDefinition.Icons;
          string none = MyTerminalActionIcons.NONE;
          string toolTip = block.CustomName.ToString();
          MyGuiScreenToolbarConfigBase.GridItemUserData gridItemUserData = new MyGuiScreenToolbarConfigBase.GridItemUserData();
          gridItemUserData.ItemData = (Func<MyObjectBuilder_ToolbarItem>) (() => (MyObjectBuilder_ToolbarItem) blockData);
          int num = block.IsFunctional ? 1 : 0;
          MyGuiGridItem myGuiGridItem = new MyGuiGridItem(icons, none, toolTip, (object) gridItemUserData, num != 0);
          gridBlocks.Add(myGuiGridItem);
        }
      }
    }

    private void categories_ItemClicked(MyGuiControlListbox sender)
    {
      this.m_gridBlocks.SetItemsToDefault();
      if (sender.SelectedItems.Count == 0)
        return;
      MyGuiScreenToolbarConfigBase.m_allSelectedCategories.Clear();
      this.m_searchInBlockCategories.Clear();
      bool flag1 = true;
      bool flag2 = false;
      foreach (MyGuiControlListbox.Item selectedItem in sender.SelectedItems)
      {
        MyGuiBlockCategoryDefinition userData = (MyGuiBlockCategoryDefinition) selectedItem.UserData;
        if (userData == null)
        {
          flag2 = true;
        }
        else
        {
          if (userData.SearchBlocks)
            this.m_searchInBlockCategories.Add(userData);
          flag1 &= userData.StrictSearch;
          MyGuiScreenToolbarConfigBase.m_allSelectedCategories.Add(userData);
        }
      }
      this.m_categorySearchCondition.SelectedCategories = MyGuiScreenToolbarConfigBase.m_allSelectedCategories;
      this.AddToolsAndAnimations((IMySearchCondition) this.m_categorySearchCondition);
      this.m_categorySearchCondition.StrictSearch = flag1;
      this.m_categorySearchCondition.SelectedCategories = this.m_searchInBlockCategories;
      IMySearchCondition searchCondition = flag2 ? (this.m_nameSearchCondition.IsValid ? (IMySearchCondition) this.m_nameSearchCondition : (IMySearchCondition) null) : (IMySearchCondition) this.m_categorySearchCondition;
      this.UpdateGridBlocksBySearchCondition(searchCondition);
      this.SearchResearch(searchCondition);
    }

    private bool SelectedCategoryMove(MyGuiControlListbox target, int step = 1)
    {
      if (step == 0 || target.Items.Count == 0)
        return false;
      int count = target.Items.Count;
      int num1 = step >= 0 ? step : count + step % count;
      List<MyGuiControlListbox.Item> selectedItems = target.SelectedItems;
      int num2;
      if (selectedItems.Count > 0)
      {
        int num3 = 0;
        MyGuiControlListbox.Item obj1 = step <= 0 ? selectedItems[0] : selectedItems[selectedItems.Count - 1];
        foreach (MyGuiControlListbox.Item obj2 in target.Items)
        {
          if (obj1 != obj2)
            ++num3;
          else
            break;
        }
        if (num3 >= count)
          return false;
        num2 = num3 + num1;
      }
      else
        num2 = step <= 0 ? target.Items.Count - 1 : 0;
      int index = num2 % count;
      target.SelectedItems = new List<MyGuiControlListbox.Item>()
      {
        target.Items[index]
      };
      return true;
    }

    protected override bool HandleKeyboardActiveIndex(MyDirection direction, bool page, bool loop)
    {
      MyGuiControlBase myGuiControlBase = this.FocusedControl;
      if (this.FocusedControl == null)
      {
        myGuiControlBase = this.GetFirstFocusableControl();
        if (myGuiControlBase == null)
          return false;
      }
      MyGuiControlBase focusControl = myGuiControlBase.GetFocusControl(direction, page, loop);
      if (focusControl != null)
        this.FocusedControl = focusControl;
      return true;
    }

    private void m_researchGraph_ItemClicked(object sender, MySharedButtonsEnum button)
    {
      MyGuiGridItem selectedItem = (sender as MyGuiControlResearchGraph).SelectedItem;
      if (selectedItem == null || !selectedItem.Enabled)
        return;
      switch (button)
      {
        case MySharedButtonsEnum.Primary:
          MyToolbarItemFactory.CreateToolbarItem(((MyGuiScreenToolbarConfigBase.GridItemUserData) selectedItem.UserData).ItemData());
          break;
        case MySharedButtonsEnum.Secondary:
          MyGuiScreenToolbarConfigBase.GridItemUserData userData = (MyGuiScreenToolbarConfigBase.GridItemUserData) selectedItem.UserData;
          if (MyToolbarItemFactory.CreateToolbarItem(userData.ItemData()) is MyToolbarItemActions toolbarItem)
          {
            if (this.UpdateContextMenu(ref this.m_contextMenu, toolbarItem, userData))
              break;
            this.m_researchGraph_ItemDoubleClicked(sender, System.EventArgs.Empty);
            break;
          }
          this.m_researchGraph_ItemDoubleClicked(sender, System.EventArgs.Empty);
          break;
        case MySharedButtonsEnum.Ternary:
          if (this.m_blockGroupInfo.SelectedDefinition == null || !MySession.Static.LocalCharacter.AddToBuildPlanner(this.m_blockGroupInfo.SelectedDefinition))
            break;
          this.m_blockGroupInfo.UpdateBuildPlanner();
          MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
          break;
        default:
          if (!MyInput.Static.IsAnyShiftKeyPressed())
            break;
          this.m_researchGraph_ItemDoubleClicked(sender, System.EventArgs.Empty);
          break;
      }
    }

    private void m_researchGraph_ItemDoubleClicked(object sender, System.EventArgs e)
    {
      MyGuiGridItem selectedItem = (sender as MyGuiControlResearchGraph).SelectedItem;
      if (selectedItem == null || !selectedItem.Enabled)
        return;
      MyObjectBuilder_ToolbarItem data = ((MyGuiScreenToolbarConfigBase.GridItemUserData) selectedItem.UserData).ItemData();
      if (data is MyObjectBuilder_ToolbarItemEmpty)
        return;
      this.AddGridItemToToolbar(data);
    }

    private void grid_ItemClicked(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
    {
      if (eventArgs.Button == MySharedButtonsEnum.Primary)
      {
        MyGuiGridItem itemAt = sender.TryGetItemAt(eventArgs.RowIndex, eventArgs.ColumnIndex);
        if (itemAt == null || !itemAt.Enabled)
          return;
        MyToolbarItemFactory.CreateToolbarItem(((MyGuiScreenToolbarConfigBase.GridItemUserData) itemAt.UserData).ItemData());
      }
      else if (eventArgs.Button == MySharedButtonsEnum.Secondary)
      {
        MyGuiGridItem itemAt = sender.TryGetItemAt(eventArgs.RowIndex, eventArgs.ColumnIndex);
        if (itemAt == null || !itemAt.Enabled)
          return;
        MyGuiScreenToolbarConfigBase.GridItemUserData userData = (MyGuiScreenToolbarConfigBase.GridItemUserData) itemAt.UserData;
        MyToolbarItem toolbarItem = MyToolbarItemFactory.CreateToolbarItem(userData.ItemData());
        if (toolbarItem is MyToolbarItemActions)
        {
          this.m_contextBlockX = eventArgs.RowIndex;
          this.m_contextBlockY = eventArgs.ColumnIndex;
          if (this.UpdateContextMenu(ref this.m_contextMenu, toolbarItem as MyToolbarItemActions, userData))
            return;
          this.grid_ItemDoubleClicked(sender, eventArgs, true);
        }
        else
          this.grid_ItemDoubleClicked(sender, eventArgs, true);
      }
      else if (eventArgs.Button == MySharedButtonsEnum.Ternary)
      {
        if (this.m_blockGroupInfo.SelectedDefinition == null || !MySession.Static.LocalCharacter.AddToBuildPlanner(this.m_blockGroupInfo.SelectedDefinition))
          return;
        this.m_blockGroupInfo.UpdateBuildPlanner();
        MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
      }
      else
      {
        if (!MyInput.Static.IsAnyShiftKeyPressed())
          return;
        this.grid_ItemShiftClicked(sender, eventArgs);
      }
    }

    private void grid_ItemShiftClicked(
      MyGuiControlGrid sender,
      MyGuiControlGrid.EventArgs eventArgs)
    {
      if (eventArgs.Button != MySharedButtonsEnum.Primary)
        return;
      MyGuiGridItem itemAt = sender.TryGetItemAt(eventArgs.RowIndex, eventArgs.ColumnIndex);
      if (itemAt == null || !itemAt.Enabled)
        return;
      MyToolbarItem toolbarItem = MyToolbarItemFactory.CreateToolbarItem(((MyGuiScreenToolbarConfigBase.GridItemUserData) itemAt.UserData).ItemData());
      if (toolbarItem.WantsToBeActivated)
        return;
      toolbarItem.Activate();
    }

    private void grid_ItemDoubleClicked(
      MyGuiControlGrid sender,
      MyGuiControlGrid.EventArgs eventArgs)
    {
      this.grid_ItemDoubleClicked(sender, eventArgs, false);
    }

    private void grid_ItemDoubleClicked(
      MyGuiControlGrid sender,
      MyGuiControlGrid.EventArgs eventArgs,
      bool redirected)
    {
      try
      {
        MyGuiGridItem itemAt = sender.TryGetItemAt(eventArgs.RowIndex, eventArgs.ColumnIndex);
        if (itemAt == null || !itemAt.Enabled)
          return;
        MyGuiScreenToolbarConfigBase.GridItemUserData userData = (MyGuiScreenToolbarConfigBase.GridItemUserData) itemAt.UserData;
        MyObjectBuilder_ToolbarItem data = userData.ItemData();
        if (data is MyObjectBuilder_ToolbarItemEmpty)
          return;
        MyToolbarItem toolbarItem = MyToolbarItemFactory.CreateToolbarItem(userData.ItemData());
        if (!redirected && toolbarItem is MyToolbarItemActions && MyInput.Static.IsJoystickLastUsed)
        {
          MyGuiControlGrid.EventArgs eventArgs1 = new MyGuiControlGrid.EventArgs()
          {
            Button = MySharedButtonsEnum.Secondary,
            ColumnIndex = eventArgs.ColumnIndex,
            ItemIndex = eventArgs.ItemIndex,
            RowIndex = eventArgs.RowIndex
          };
          this.grid_ItemClicked(sender, eventArgs1);
        }
        else
          this.AddGridItemToToolbar(data);
      }
      finally
      {
      }
    }

    private void grid_PanelScrolled(MyGuiControlScrollablePanel panel)
    {
      if (this.m_contextMenu == null || !this.m_contextMenu.IsActiveControl)
        return;
      this.m_contextMenu.Deactivate();
    }

    protected void grid_OnDrag(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs) => this.StartDragging(MyDropHandleType.MouseRelease, sender, ref eventArgs);

    private void m_researchGraph_ItemDragged(object sender, MyGuiGridItem item) => this.StartDragging(MyDropHandleType.MouseRelease, sender as MyGuiControlResearchGraph, item);

    private void dragAndDrop_OnDrop(object sender, MyDragAndDropEventArgs eventArgs)
    {
      if (eventArgs.DropTo != null && !this.m_toolbarControl.IsToolbarGrid(eventArgs.DragFrom.Grid) && this.m_toolbarControl.IsToolbarGrid(eventArgs.DropTo.Grid))
      {
        MyGuiScreenToolbarConfigBase.GridItemUserData userData = (MyGuiScreenToolbarConfigBase.GridItemUserData) eventArgs.Item.UserData;
        MyObjectBuilder_ToolbarItem data = userData.ItemData();
        if (data is MyObjectBuilder_ToolbarItemEmpty)
          return;
        if (eventArgs.DropTo.ItemIndex >= 0 && eventArgs.DropTo.ItemIndex < 9)
        {
          MyToolbarItem toolbarItem = MyToolbarItemFactory.CreateToolbarItem(data);
          if (toolbarItem is MyToolbarItemActions)
          {
            if (this.UpdateContextMenu(ref this.m_onDropContextMenu, toolbarItem as MyToolbarItemActions, userData))
            {
              this.m_onDropContextMenuToolbarIndex = eventArgs.DropTo.ItemIndex;
              this.m_onDropContextMenu.Enabled = true;
              this.m_onDropContextMenuItem = toolbarItem;
            }
            else
              MyGuiScreenToolbarConfigBase.DropGridItemToToolbar(toolbarItem, eventArgs.DropTo.ItemIndex);
          }
          else
          {
            MyGuiScreenToolbarConfigBase.DropGridItemToToolbar(toolbarItem, eventArgs.DropTo.ItemIndex);
            if (toolbarItem.WantsToBeActivated)
              MyToolbarComponent.CurrentToolbar.ActivateItemAtSlot(eventArgs.DropTo.ItemIndex, playActivationSound: false, userActivated: false);
          }
        }
      }
      else if (eventArgs.DropTo != null && !this.m_toolbarControl.IsToolbarGrid(eventArgs.DragFrom.Grid) && this.m_blockGroupInfo.IsBuildPlannerGrid(eventArgs.DropTo.Grid))
      {
        if (((MyGuiScreenToolbarConfigBase.GridItemUserData) eventArgs.Item.UserData).ItemData() is MyObjectBuilder_ToolbarItemEmpty)
          return;
        if (this.m_blockGroupInfo.SelectedDefinition != null && MySession.Static.LocalCharacter.AddToBuildPlanner(this.m_blockGroupInfo.SelectedDefinition, eventArgs.DropTo.ItemIndex))
        {
          this.m_blockGroupInfo.UpdateBuildPlanner();
          MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
        }
      }
      this.m_toolbarControl.HandleDragAndDrop(sender, eventArgs);
    }

    private void searchItemTexbox_TextChanged(string text)
    {
      if (this.m_framesBeforeSearchEnabled > 0)
        return;
      this.m_gridBlocks.SetItemsToDefault();
      string str = text;
      if (string.IsNullOrWhiteSpace(str) || string.IsNullOrEmpty(str))
      {
        if (this.m_character != null || this.m_shipController != null && this.m_shipController.BuildingMode)
          this.AddCubeDefinitionsToBlocks((IMySearchCondition) null);
        else
          this.AddShipBlocksDefinitions(this.m_screenCubeGrid, true, (IMySearchCondition) null);
        this.m_nameSearchCondition.Clean();
        this.SearchResearch((IMySearchCondition) null);
      }
      else
      {
        this.m_nameSearchCondition.SearchName = str;
        if ((this.m_shipController == null ? 0 : (!this.m_shipController.EnableShipControl ? 1 : 0)) == 0)
        {
          this.AddToolsAndAnimations((IMySearchCondition) this.m_nameSearchCondition);
          this.UpdateGridBlocksBySearchCondition((IMySearchCondition) this.m_nameSearchCondition);
          this.SearchResearch((IMySearchCondition) this.m_nameSearchCondition);
        }
        else
        {
          this.AddAnimations(true, (IMySearchCondition) this.m_nameSearchCondition);
          this.AddEmotes(true, (IMySearchCondition) this.m_nameSearchCondition);
        }
      }
    }

    private void SearchResearch(IMySearchCondition searchCondition)
    {
      this.m_minVerticalPosition = float.MaxValue;
      this.m_researchItemFound = false;
      if (this.m_researchGraph != null && this.m_researchGraph.Nodes != null)
      {
        foreach (MyGuiControlResearchGraph.GraphNode node in this.m_researchGraph.Nodes)
          this.SearchNode(node, searchCondition);
      }
      if (!this.m_researchItemFound)
        return;
      this.m_researchPanel.SetVerticalScrollbarValue(this.m_minVerticalPosition);
    }

    private void SearchNode(
      MyGuiControlResearchGraph.GraphNode node,
      IMySearchCondition searchCondition)
    {
      foreach (MyGuiGridItem myGuiGridItem in node.Items)
      {
        bool flag = searchCondition != null && searchCondition.MatchesCondition(myGuiGridItem.ItemDefinition);
        myGuiGridItem.OverlayPercent = flag ? 1f : 0.0f;
        if (flag && (double) this.m_minVerticalPosition > (double) node.Position.Y)
        {
          this.m_minVerticalPosition = node.Position.Y;
          this.m_researchItemFound = true;
        }
      }
      foreach (MyGuiControlResearchGraph.GraphNode child in node.Children)
        this.SearchNode(child, searchCondition);
    }

    protected void UpdateGridControl() => this.categories_ItemClicked(this.m_categoriesListbox);

    private void contextMenu_ItemClicked(
      MyGuiControlContextMenu sender,
      MyGuiControlContextMenu.EventArgs args)
    {
      if (this.m_contextBlockX < 0 || this.m_contextBlockX >= this.m_gridBlocks.RowsCount || (this.m_contextBlockY < 0 || this.m_contextBlockY >= this.m_gridBlocks.ColumnsCount))
        return;
      MyGuiGridItem itemAt = this.m_gridBlocks.TryGetItemAt(this.m_contextBlockX, this.m_contextBlockY);
      if (itemAt == null)
        return;
      MyObjectBuilder_ToolbarItemTerminal toolbarItemTerminal = (MyObjectBuilder_ToolbarItemTerminal) (itemAt.UserData as MyGuiScreenToolbarConfigBase.GridItemUserData).ItemData();
      toolbarItemTerminal._Action = (string) args.UserData;
      this.AddGridItemToToolbar((MyObjectBuilder_ToolbarItem) toolbarItemTerminal);
      toolbarItemTerminal._Action = (string) null;
      this.FocusedControl = (MyGuiControlBase) this.m_gridBlocks;
    }

    private void onDropContextMenu_ItemClicked(
      MyGuiControlContextMenu sender,
      MyGuiControlContextMenu.EventArgs args)
    {
      int menuToolbarIndex = this.m_onDropContextMenuToolbarIndex;
      if (menuToolbarIndex < 0 || menuToolbarIndex >= MyToolbarComponent.CurrentToolbar.SlotCount)
        return;
      MyToolbarItem dropContextMenuItem = this.m_onDropContextMenuItem;
      if (!(dropContextMenuItem is MyToolbarItemActions))
        return;
      (dropContextMenuItem as MyToolbarItemActions).ActionId = (string) args.UserData;
      MyGuiScreenToolbarConfigBase.DropGridItemToToolbar(dropContextMenuItem, menuToolbarIndex);
    }

    private void AddGridItemToToolbar(MyObjectBuilder_ToolbarItem data)
    {
      if (this.m_gamepadSlot.HasValue)
      {
        MyToolbarItem newItem = MyToolbarItemFactory.CreateToolbarItem(data);
        if (newItem == null)
          return;
        MyGuiScreenToolbarConfigBase.RequestItemParameters(newItem, (Action<bool>) (success => MyToolbarComponent.CurrentToolbar.SetItemAtIndex(this.m_gamepadSlot.Value, newItem, true)));
        this.CloseScreen(false);
      }
      else
      {
        MyToolbar currentToolbar = MyToolbarComponent.CurrentToolbar;
        int slotCount = currentToolbar.SlotCount;
        MyToolbarItem newItem = MyToolbarItemFactory.CreateToolbarItem(data);
        if (newItem == null)
          return;
        MyGuiScreenToolbarConfigBase.RequestItemParameters(newItem, (Action<bool>) (success =>
        {
          bool flag = false;
          int num = 0;
          for (int slot = 0; slot < slotCount; ++slot)
          {
            MyToolbarItem slotItem = currentToolbar.GetSlotItem(slot);
            if (slotItem != null && slotItem.Equals((object) newItem))
            {
              if (slotItem.WantsToBeActivated)
                currentToolbar.ActivateItemAtSlot(slot, playActivationSound: false, userActivated: false);
              num = slot;
              flag = true;
              break;
            }
          }
          for (int slot = 0; slot < slotCount; ++slot)
          {
            if ((flag ? 0 : (currentToolbar.GetSlotItem(slot) == null ? 1 : 0)) != 0)
            {
              currentToolbar.SetItemAtSlot(slot, newItem);
              if (newItem.WantsToBeActivated)
                currentToolbar.ActivateItemAtSlot(slot, playActivationSound: false, userActivated: false);
              num = slot;
              flag = true;
            }
            else if (slot != num && currentToolbar.GetSlotItem(slot) != null && currentToolbar.GetSlotItem(slot).Equals((object) newItem))
              currentToolbar.SetItemAtSlot(slot, (MyToolbarItem) null);
          }
          if (flag)
            return;
          int slot1 = currentToolbar.SelectedSlot ?? 0;
          currentToolbar.SetItemAtSlot(slot1, newItem);
          if (!newItem.WantsToBeActivated)
            return;
          currentToolbar.ActivateItemAtSlot(slot1, playActivationSound: false, userActivated: false);
        }));
      }
    }

    public static void RequestItemParameters(MyToolbarItem item, Action<bool> callback)
    {
      if (item is MyToolbarItemTerminalBlock itemTerminalBlock)
      {
        ITerminalAction actionOrNull = itemTerminalBlock.GetActionOrNull(itemTerminalBlock.ActionId);
        if (actionOrNull != null && actionOrNull.GetParameterDefinitions().Count > 0)
        {
          actionOrNull.RequestParameterCollection((IList<TerminalActionParameter>) itemTerminalBlock.Parameters, callback);
          return;
        }
      }
      callback(true);
    }

    public static void DropGridItemToToolbar(MyToolbarItem item, int slot) => MyGuiScreenToolbarConfigBase.RequestItemParameters(item, (Action<bool>) (success =>
    {
      if (!success)
        return;
      MyToolbar currentToolbar = MyToolbarComponent.CurrentToolbar;
      for (int slot1 = 0; slot1 < currentToolbar.SlotCount; ++slot1)
      {
        if (currentToolbar.GetSlotItem(slot1) != null && currentToolbar.GetSlotItem(slot1).Equals((object) item))
          currentToolbar.SetItemAtSlot(slot1, (MyToolbarItem) null);
      }
      MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
      MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, item);
    }));

    public static void ReinitializeBlockScrollbarPosition() => MyGuiScreenToolbarConfigBase.m_savedVPosition = 0.0f;

    private bool CanDropItem(
      MyPhysicalInventoryItem item,
      MyGuiControlGrid dropFrom,
      MyGuiControlGrid dropTo)
    {
      return dropTo != dropFrom;
    }

    private void StartDragging(
      MyDropHandleType dropHandlingType,
      MyGuiControlResearchGraph graph,
      MyGuiGridItem draggingItem)
    {
      if (!draggingItem.Enabled)
        return;
      MyDragAndDropInfo draggingFrom = new MyDragAndDropInfo();
      this.m_dragAndDrop.StartDragging(dropHandlingType, MySharedButtonsEnum.Primary, draggingItem, draggingFrom, false);
      graph.HideToolTip();
    }

    private void StartDragging(
      MyDropHandleType dropHandlingType,
      MyGuiControlGrid grid,
      ref MyGuiControlGrid.EventArgs args)
    {
      MyDragAndDropInfo draggingFrom = new MyDragAndDropInfo();
      draggingFrom.Grid = grid;
      draggingFrom.ItemIndex = args.ItemIndex;
      MyGuiGridItem itemAt = grid.GetItemAt(args.ItemIndex);
      if (!itemAt.Enabled)
        return;
      this.m_dragAndDrop.StartDragging(dropHandlingType, args.Button, itemAt, draggingFrom, false);
      grid.HideToolTip();
    }

    private bool UpdateContextMenu(
      ref MyGuiControlContextMenu currentContextMenu,
      MyToolbarItemActions item,
      MyGuiScreenToolbarConfigBase.GridItemUserData data)
    {
      ListReader<ITerminalAction> listReader = item.PossibleActions(this.m_toolbarControl.ShownToolbar.ToolbarType);
      if (listReader.Count <= 0)
        return false;
      currentContextMenu.Enabled = true;
      currentContextMenu.CreateNewContextMenu();
      foreach (ITerminalAction terminalAction in listReader)
        currentContextMenu.AddItem(terminalAction.Name, "", terminalAction.Icon, (object) terminalAction.Id);
      return true;
    }

    public override bool Update(bool hasFocus)
    {
      if (this.m_framesBeforeSearchEnabled > 0)
        --this.m_framesBeforeSearchEnabled;
      if (this.m_framesBeforeSearchEnabled == 0)
      {
        this.m_searchBox.Enabled = true;
        this.m_searchBox.TextBox.CanHaveFocus = true;
        if (MyVRage.Platform.ImeProcessor != null)
          MyVRage.Platform.ImeProcessor.RegisterActiveScreen((IVRageGuiScreen) this);
        this.FocusedControl = (MyGuiControlBase) this.m_searchBox.TextBox;
        --this.m_framesBeforeSearchEnabled;
      }
      if (this.m_frameCounterPCU >= this.PCU_UPDATE_EACH_N_FRAMES && this.m_PCUControl.Visible)
      {
        this.m_PCUControl.UpdatePCU(this.GetIdentity(), true);
        this.m_frameCounterPCU = 0;
      }
      else
        ++this.m_frameCounterPCU;
      bool joystickLastUsed = MyInput.Static.IsJoystickLastUsed;
      this.m_categoryHintLeft.Visible = joystickLastUsed;
      this.m_categoryHintRight.Visible = joystickLastUsed;
      if (this.m_screenOwner == null)
      {
        this.m_toolbarLabel.Visible = !joystickLastUsed;
        this.m_toolbarControl.Visible = !joystickLastUsed;
      }
      return base.Update(hasFocus);
    }

    public enum GroupModes
    {
      Default,
      HideEmpty,
      HideBlockGroups,
      HideAll,
    }

    public class GridItemUserData
    {
      public Func<MyObjectBuilder_ToolbarItem> ItemData;
      public Action Action;
    }
  }
}
