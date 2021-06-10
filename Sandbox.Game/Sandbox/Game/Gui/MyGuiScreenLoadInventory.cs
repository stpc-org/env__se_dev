// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenLoadInventory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.GameServices;
using VRage.Input;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;
using VRageRender.Utils;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenLoadInventory : MyGuiScreenBase
  {
    public static readonly string IMAGE_PREFIX = "IN_";
    private static readonly bool SKIN_STORE_FEATURES_ENABLED = false;
    private readonly string m_hueScaleTexture = "Textures\\GUI\\HueScale.png";
    private readonly string m_equipCheckbox = "equipCheckbox";
    private Vector2 m_itemsTableSize;
    private MyGuiControlButton m_viewDetailsButton;
    private MyGuiControlButton m_openStoreButton;
    private MyGuiControlButton m_refreshButton;
    private MyGuiControlButton m_browseItemsButton;
    private MyGuiControlButton m_characterButton;
    private MyGuiControlButton m_toolsButton;
    private MyGuiControlButton m_recyclingButton;
    private MyGuiControlButton m_currentButton;
    private bool m_inGame;
    private MyGuiScreenLoadInventory.TabState m_activeTabState;
    private MyGuiScreenLoadInventory.LowerTabState m_activeLowTabState;
    private string m_rotatingWheelTexture;
    private MyGuiControlRotatingWheel m_wheel;
    private MyEntityRemoteController m_entityController;
    private List<MyGuiControlCheckbox> m_itemCheckboxes;
    private bool m_itemCheckActive;
    private MyGuiControlCombobox m_modelPicker;
    private MyGuiControlSlider m_sliderHue;
    private MyGuiControlSlider m_sliderSaturation;
    private MyGuiControlSlider m_sliderValue;
    private MyGuiControlLabel m_labelHue;
    private MyGuiControlLabel m_labelSaturation;
    private MyGuiControlLabel m_labelValue;
    private string m_selectedModel;
    private Vector3 m_selectedHSV;
    private Dictionary<string, int> m_displayModels;
    private Dictionary<int, string> m_models;
    private string m_storedModel;
    private Vector3 m_storedHSV;
    private bool m_colorOrModelChanged;
    private MyGameInventoryItemSlot m_filteredSlot;
    private MyGuiControlContextMenu m_contextMenu;
    private MyGuiControlImageButton m_contextMenuLastButton;
    private bool m_hideDuplicatesEnabled;
    private bool m_showOnlyDuplicatesEnabled;
    private MyGuiControlParent m_itemsTableParent;
    private List<MyGameInventoryItem> m_userItems;
    private List<MyPhysicalInventoryItem> m_allTools = new List<MyPhysicalInventoryItem>();
    private MyGuiControlCombobox m_toolPicker;
    private string m_selectedTool;
    private MyGuiControlButton m_OkButton;
    private MyGuiControlButton m_cancelButton;
    private MyGuiControlButton m_craftButton;
    private MyGuiControlCombobox m_rarityPicker;
    private MyGameInventoryItem m_lastCraftedItem;
    private MyGuiControlButton m_coloringButton;
    private bool m_audioSet;
    private bool? m_savedStateAnselEnabled;
    private List<MyGuiScreenLoadInventory.CategoryButton> m_categoryButtonsData;
    private int m_currentCategoryIndex;
    private MyGuiControlStackPanel m_categoryButtonLayout;
    private MyGuiControlCheckbox m_duplicateCheckboxRecycle;
    private bool m_focusButton;
    private MyGuiControlWrapPanel m_itemsTable;
    private const int PAGE_COUNT = 2;

    public static event MyLookChangeDelegate LookChanged;

    public MyGuiScreenLoadInventory()
      : base(new Vector2?(new Vector2(0.32f, 0.05f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.65f, 0.9f)), true, backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
      => this.EnabledBackgroundFade = false;

    public MyGuiScreenLoadInventory(bool inGame = false, HashSet<string> customCharacterNames = null)
      : base(new Vector2?(new Vector2(0.32f, 0.05f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.65f, 0.9f)), true, backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.EnabledBackgroundFade = false;
      this.Initialize(inGame, customCharacterNames);
    }

    public void Initialize(bool inGame, HashSet<string> customCharacterNames)
    {
      this.m_inGame = inGame;
      this.m_audioSet = inGame;
      this.m_rotatingWheelTexture = "Textures\\GUI\\screens\\screen_loading_wheel_loading_screen.dds";
      this.Align = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_filteredSlot = MyGameInventoryItemSlot.None;
      this.IsHitTestVisible = true;
      MyGameService.CheckItemDataReady += new EventHandler<MyGameItemsEventArgs>(this.MyGameService_CheckItemDataReady);
      this.m_storedModel = MySession.Static.LocalCharacter != null ? MySession.Static.LocalCharacter.ModelName : string.Empty;
      this.InitModels(customCharacterNames);
      this.m_entityController = new MyEntityRemoteController((MyEntity) MySession.Static.LocalCharacter);
      this.m_entityController.LockRotationAxis(GlobalAxis.Y | GlobalAxis.Z);
      this.m_allTools = this.m_entityController.GetInventoryTools();
      this.RecreateControls(true);
      this.UpdateSliderTooltips();
      MyScreenManager.GetFirstScreenOfType<MyGuiScreenIntroVideo>()?.HideScreen();
      if (!inGame)
        MyLocalCache.LoadInventoryConfig(MySession.Static.LocalCharacter);
      this.EquipTool();
      this.UpdateCheckboxes();
      this.m_isTopMostScreen = false;
      switch (this.m_activeTabState)
      {
        case MyGuiScreenLoadInventory.TabState.Character:
          this.m_currentCategoryIndex = -1;
          break;
        case MyGuiScreenLoadInventory.TabState.Tools:
          this.m_currentCategoryIndex = 0;
          break;
      }
      MyScreenManager.GetFirstScreenOfType<MyGuiScreenGamePlay>()?.HideScreen();
    }

    private void InitModels(HashSet<string> customCharacterNames)
    {
      this.m_displayModels = new Dictionary<string, int>();
      this.m_models = new Dictionary<int, string>();
      int num = 0;
      if (customCharacterNames == null)
      {
        foreach (MyCharacterDefinition character in MyDefinitionManager.Static.Characters)
        {
          if ((character.UsableByPlayer || !MySession.Static.SurvivalMode && this.m_inGame && MySession.Static.IsRunningExperimental) && character.Public)
          {
            this.m_displayModels[this.GetDisplayName(character.Name)] = num;
            this.m_models[num++] = character.Name;
          }
        }
      }
      else
      {
        DictionaryValuesReader<string, MyCharacterDefinition> characters = MyDefinitionManager.Static.Characters;
        foreach (string customCharacterName in customCharacterNames)
        {
          MyCharacterDefinition result;
          if (characters.TryGetValue(customCharacterName, out result) && (!MySession.Static.SurvivalMode || result.UsableByPlayer) && result.Public)
          {
            this.m_displayModels[this.GetDisplayName(result.Name)] = num;
            this.m_models[num++] = result.Name;
          }
        }
      }
    }

    private string GetDisplayName(string name) => MyTexts.GetString(name);

    private void SubscribeValueChangedEventsToSliders(bool subsribe)
    {
      if (subsribe)
      {
        if (this.m_sliderHue != null)
          this.m_sliderHue.ValueChanged += new Action<MyGuiControlSlider>(this.OnValueChange);
        if (this.m_sliderSaturation != null)
          this.m_sliderSaturation.ValueChanged += new Action<MyGuiControlSlider>(this.OnValueChange);
        if (this.m_sliderValue == null)
          return;
        this.m_sliderValue.ValueChanged += new Action<MyGuiControlSlider>(this.OnValueChange);
      }
      else
      {
        if (this.m_sliderHue != null)
          this.m_sliderHue.ValueChanged -= new Action<MyGuiControlSlider>(this.OnValueChange);
        if (this.m_sliderSaturation != null)
          this.m_sliderSaturation.ValueChanged -= new Action<MyGuiControlSlider>(this.OnValueChange);
        if (this.m_sliderValue == null)
          return;
        this.m_sliderValue.ValueChanged -= new Action<MyGuiControlSlider>(this.OnValueChange);
      }
    }

    private MyGuiControlSeparatorList Prepare_MyGuiControlSeparatorList()
    {
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.072f), 0.5385f);
      controlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.147f), 0.5385f);
      controlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.228f), 0.5385f);
      controlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.548f), 0.5385f);
      controlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.629f), 0.5385f);
      controlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.778f), 0.5385f);
      return controlSeparatorList;
    }

    private MyGuiControlStackPanel Prepare_sideStackPanel()
    {
      MyGuiControlStackPanel controlStackPanel = new MyGuiControlStackPanel();
      controlStackPanel.BackgroundTexture = MyGuiConstants.TEXTURE_COMPOSITE_ROUND_ALL;
      controlStackPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      return controlStackPanel;
    }

    private MyGuiControlStackPanel Prepare_lowTabPanel()
    {
      MyGuiControlStackPanel controlStackPanel = new MyGuiControlStackPanel();
      controlStackPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlStackPanel.Margin = new Thickness(0.016f, 0.009f, 0.015f, 0.015f);
      controlStackPanel.Orientation = MyGuiOrientation.Horizontal;
      return controlStackPanel;
    }

    private MyGuiControlButton Prepare_coloringButton()
    {
      MyGuiControlButton guiControlButton = this.MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryColoring, MyCommonTexts.ScreenLoadInventoryColoringFilterTooltip, new Action<MyGuiControlButton>(this.OnViewTabColoring));
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.ToolbarButton;
      guiControlButton.CanHaveFocus = false;
      guiControlButton.Margin = new Thickness(0.0415f, 0.0285f, 1f / 400f, 0.0f);
      if (this.m_activeLowTabState == MyGuiScreenLoadInventory.LowerTabState.Coloring)
      {
        guiControlButton.Checked = true;
        guiControlButton.Selected = true;
      }
      return guiControlButton;
    }

    private MyGuiControlStackPanel Prepare_tabsPanel()
    {
      MyGuiControlStackPanel controlStackPanel = new MyGuiControlStackPanel();
      controlStackPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlStackPanel.Margin = new Thickness(0.016f, 0.0f, 0.015f, 0.015f);
      controlStackPanel.Orientation = MyGuiOrientation.Horizontal;
      return controlStackPanel;
    }

    private MyGuiControlScrollablePanel Prepare_scrollPanel()
    {
      MyGuiControlScrollablePanel controlScrollablePanel = new MyGuiControlScrollablePanel((MyGuiControlBase) this.m_itemsTableParent);
      controlScrollablePanel.CompleteScissor = true;
      controlScrollablePanel.ScrollBarOffset = new Vector2(0.005f, 0.0f);
      controlScrollablePanel.ScrollbarVEnabled = true;
      controlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlScrollablePanel.Size = this.m_itemsTableSize;
      controlScrollablePanel.Position = this.m_itemsTable.Position;
      return controlScrollablePanel;
    }

    private void Prepare_m_recyclingButton()
    {
      this.m_recyclingButton = this.MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryRecycling, MyCommonTexts.ScreenLoadInventoryRecyclingFilterTooltip, new Action<MyGuiControlButton>(this.OnViewTabRecycling));
      this.m_recyclingButton.VisualStyle = MyGuiControlButtonStyleEnum.ToolbarButton;
      this.m_recyclingButton.CanHaveFocus = false;
      this.m_recyclingButton.Margin = new Thickness(1f / 400f, 0.0285f, 1f / 400f, 0.0f);
      if (this.m_activeLowTabState != MyGuiScreenLoadInventory.LowerTabState.Recycling)
        return;
      this.m_recyclingButton.Checked = true;
      this.m_recyclingButton.Selected = true;
    }

    private void Prepare_m_characterButton()
    {
      this.m_characterButton = this.MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryCharacter, MyCommonTexts.ScreenLoadInventoryCharacterFilterTooltip, new Action<MyGuiControlButton>(this.OnViewTabCharacter));
      this.m_characterButton.VisualStyle = MyGuiControlButtonStyleEnum.ToolbarButton;
      this.m_characterButton.CanHaveFocus = false;
      this.m_characterButton.Margin = new Thickness(0.0415f, 0.0285f, 1f / 400f, 0.0f);
      this.m_categoryButtonsData = new List<MyGuiScreenLoadInventory.CategoryButton>();
      if (this.m_activeTabState != MyGuiScreenLoadInventory.TabState.Character)
        return;
      this.FocusedControl = (MyGuiControlBase) this.m_characterButton;
      this.m_characterButton.Checked = true;
      this.m_characterButton.Selected = true;
      this.m_categoryButtonsData.Add(new MyGuiScreenLoadInventory.CategoryButton(MyCommonTexts.ScreenLoadInventoryHelmetTooltip, MyGameInventoryItemSlot.Helmet, "Textures\\GUI\\Icons\\Skins\\Categories\\helmet.png", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryHelmet)));
      this.m_categoryButtonsData.Add(new MyGuiScreenLoadInventory.CategoryButton(MyCommonTexts.ScreenLoadInventorySuitTooltip, MyGameInventoryItemSlot.Suit, "Textures\\GUI\\Icons\\Skins\\Categories\\suit.png", MyTexts.GetString(MyCommonTexts.ScreenLoadInventorySuit)));
      this.m_categoryButtonsData.Add(new MyGuiScreenLoadInventory.CategoryButton(MyCommonTexts.ScreenLoadInventoryGlovesTooltip, MyGameInventoryItemSlot.Gloves, "Textures\\GUI\\Icons\\Skins\\Categories\\glove.png", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryGloves)));
      this.m_categoryButtonsData.Add(new MyGuiScreenLoadInventory.CategoryButton(MyCommonTexts.ScreenLoadInventoryBootsTooltip, MyGameInventoryItemSlot.Boots, "Textures\\GUI\\Icons\\Skins\\Categories\\boot.png", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryBoots)));
    }

    private void Prepare_m_toolsButton_And_m_categoryButtonsData()
    {
      this.m_toolsButton = this.MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryTools, MyCommonTexts.ScreenLoadInventoryToolsFilterTooltip, new Action<MyGuiControlButton>(this.OnViewTools));
      this.m_toolsButton.CanHaveFocus = false;
      this.m_toolsButton.VisualStyle = MyGuiControlButtonStyleEnum.ToolbarButton;
      this.m_toolsButton.Margin = new Thickness(1f / 400f, 0.0285f, 0.0f, 0.0f);
      if (this.m_activeTabState != MyGuiScreenLoadInventory.TabState.Tools)
        return;
      this.FocusedControl = (MyGuiControlBase) this.m_toolsButton;
      this.m_toolsButton.Checked = true;
      this.m_toolsButton.Selected = true;
      this.m_categoryButtonsData.Add(new MyGuiScreenLoadInventory.CategoryButton(MyCommonTexts.ScreenLoadInventoryWelderTooltip, MyGameInventoryItemSlot.Welder, "Textures\\GUI\\Icons\\WeaponWelder.dds", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryWelder)));
      this.m_categoryButtonsData.Add(new MyGuiScreenLoadInventory.CategoryButton(MyCommonTexts.ScreenLoadInventoryGrinderTooltip, MyGameInventoryItemSlot.Grinder, "Textures\\GUI\\Icons\\WeaponGrinder.dds", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryGrinder)));
      this.m_categoryButtonsData.Add(new MyGuiScreenLoadInventory.CategoryButton(MyCommonTexts.ScreenLoadInventoryDrillTooltip, MyGameInventoryItemSlot.Drill, "Textures\\GUI\\Icons\\WeaponDrill.dds", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryDrill)));
      this.m_categoryButtonsData.Add(new MyGuiScreenLoadInventory.CategoryButton(MyCommonTexts.ScreenLoadInventoryRifleTooltip, MyGameInventoryItemSlot.Rifle, "Textures\\GUI\\Icons\\WeaponAutomaticRifle.dds", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryRifle)));
    }

    private void Prepare_m_categoryButtonLayout(
      MyGuiControlImageButton.StyleDefinition basicButtonStyle,
      Vector2 basicButtonSize)
    {
      this.m_categoryButtonLayout = new MyGuiControlStackPanel();
      this.m_categoryButtonLayout.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_categoryButtonLayout.Margin = new Thickness(0.055f, 0.05f, 0.0f, 0.0f);
      this.m_categoryButtonLayout.Orientation = MyGuiOrientation.Horizontal;
      foreach (MyGuiScreenLoadInventory.CategoryButton categoryButton in this.m_categoryButtonsData)
      {
        MyGuiControlImageButton controlImageButton = this.MakeImageButton(Vector2.Zero, basicButtonSize, basicButtonStyle, categoryButton.Tooltip, new Action<MyGuiControlImageButton>(this.OnCategoryClicked));
        controlImageButton.UserData = (object) categoryButton.Slot;
        controlImageButton.Margin = new Thickness(0.0f, 0.0f, 0.004f, 0.0f);
        this.m_categoryButtonLayout.Add((MyGuiControlBase) controlImageButton);
      }
    }

    private void Prepare_LowerTabState_Coloring_ModelPanel(MyGuiControlStackPanel modelPanel)
    {
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.PlayerCharacterModel));
      myGuiControlLabel.Margin = new Thickness(0.045f, -0.03f, 0.005f, 0.0f);
      this.m_modelPicker = new MyGuiControlCombobox();
      this.m_modelPicker.Size = new Vector2(0.225f, 1f);
      this.m_modelPicker.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_modelPicker.Margin = new Thickness(0.005f, -0.03f, 0.005f, 0.0f);
      this.m_modelPicker.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipCharacterScreen_Model));
      foreach (KeyValuePair<string, int> displayModel in this.m_displayModels)
        this.m_modelPicker.AddItem((long) displayModel.Value, new StringBuilder(displayModel.Key));
      this.m_selectedModel = MySession.Static.LocalCharacter.ModelName;
      if (this.m_displayModels.ContainsKey(this.GetDisplayName(this.m_selectedModel)))
        this.m_modelPicker.SelectItemByKey((long) this.m_displayModels[this.GetDisplayName(this.m_selectedModel)]);
      else if (this.m_displayModels.Count > 0)
        this.m_modelPicker.SelectItemByKey((long) this.m_displayModels.First<KeyValuePair<string, int>>().Value);
      this.m_modelPicker.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnItemSelected);
      if (this.m_activeTabState != MyGuiScreenLoadInventory.TabState.Character && this.m_activeTabState != MyGuiScreenLoadInventory.TabState.Tools)
        return;
      modelPanel.Add((MyGuiControlBase) myGuiControlLabel);
      modelPanel.Add((MyGuiControlBase) this.m_modelPicker);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.Controls.Add((MyGuiControlBase) this.m_modelPicker);
    }

    private void TabState_Filter()
    {
      this.m_toolPicker = new MyGuiControlCombobox();
      this.m_toolPicker.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_toolPicker.Margin = new Thickness(0.015f, 0.01f, 0.01f, 0.01f);
      foreach (MyPhysicalInventoryItem allTool in this.m_allTools)
      {
        if (this.m_entityController.GetToolSlot(allTool.Content.SubtypeName) == this.m_filteredSlot)
        {
          MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) allTool.Content);
          if (physicalItemDefinition != null)
            this.m_toolPicker.AddItem((long) allTool.ItemId, new StringBuilder(physicalItemDefinition.DisplayNameText));
          else if (allTool.Content != null)
            this.m_toolPicker.AddItem((long) allTool.ItemId, new StringBuilder(allTool.Content.SubtypeName));
        }
      }
      if (string.IsNullOrEmpty(this.m_selectedTool))
      {
        if (this.m_toolPicker.GetItemsCount() > 0)
        {
          this.m_toolPicker.SelectItemByIndex(0);
          uint key = (uint) this.m_toolPicker.GetSelectedKey();
          MyPhysicalInventoryItem physicalInventoryItem = this.m_allTools.FirstOrDefault<MyPhysicalInventoryItem>((Func<MyPhysicalInventoryItem, bool>) (t => (int) t.ItemId == (int) key));
          if (physicalInventoryItem.Content != null)
            this.m_selectedTool = physicalInventoryItem.Content.SubtypeName;
        }
      }
      else
        this.m_toolPicker.SelectItemByKey((long) this.m_allTools.FirstOrDefault<MyPhysicalInventoryItem>((Func<MyPhysicalInventoryItem, bool>) (t => t.Content.SubtypeName == this.m_selectedTool)).ItemId);
      this.m_toolPicker.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_toolPicker_ItemSelected);
    }

    private void Prepare_LowerTabState_Coloring(MyGuiControlStackPanel modelPanel)
    {
      MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      guiControlCheckbox.IsChecked = this.m_hideDuplicatesEnabled;
      guiControlCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnHideDuplicates);
      guiControlCheckbox.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipCharacterScreen_HideDuplicates));
      guiControlCheckbox.Margin = new Thickness(0.005f, -0.03f, 0.005f, 0.01f);
      modelPanel.Add((MyGuiControlBase) guiControlCheckbox);
      this.Controls.Add((MyGuiControlBase) guiControlCheckbox);
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryHideDuplicates));
      myGuiControlLabel.Margin = new Thickness(0.0f, -0.03f, 0.005f, 0.01f);
      modelPanel.Add((MyGuiControlBase) myGuiControlLabel);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
    }

    private void Prepare_LowerTabState_NotColoring(MyGuiControlStackPanel modelPanel)
    {
      this.m_duplicateCheckboxRecycle = new MyGuiControlCheckbox(originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      this.m_duplicateCheckboxRecycle.IsChecked = this.m_showOnlyDuplicatesEnabled;
      this.m_duplicateCheckboxRecycle.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnShowOnlyDuplicates);
      this.m_duplicateCheckboxRecycle.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipCharacterScreen_ShowOnlyDuplicates));
      this.m_duplicateCheckboxRecycle.Margin = new Thickness(0.039f, -0.03f, 0.01f, 0.01f);
      modelPanel.Add((MyGuiControlBase) this.m_duplicateCheckboxRecycle);
      this.Controls.Add((MyGuiControlBase) this.m_duplicateCheckboxRecycle);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryShowOnlyDuplicates));
      myGuiControlLabel1.Margin = new Thickness(0.005f, -0.03f, 0.01f, 0.01f);
      modelPanel.Add((MyGuiControlBase) myGuiControlLabel1);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(text: string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryCurrencyCurrent), (object) MyGameService.RecycleTokens), maxWidth: 0.1f, isAutoScaleEnabled: true);
      myGuiControlLabel2.Margin = new Thickness(0.19f, -0.05f, 0.01f, 0.01f);
      modelPanel.Add((MyGuiControlBase) myGuiControlLabel2);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
    }

    private void Prepare_LowerTabState_Sliders(MyGuiControlStackPanel colorPanel)
    {
      this.m_sliderHue = new MyGuiControlSlider(maxValue: 360f, width: 0.177f, labelDecimalPlaces: 0, toolTip: string.Empty, visualStyle: MyGuiControlSliderStyleEnum.Hue, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, intValue: true);
      this.m_sliderHue.Margin = new Thickness(0.055f, -0.0425f, 0.0f, 0.0f);
      this.m_sliderHue.Enabled = this.m_activeTabState == MyGuiScreenLoadInventory.TabState.Character;
      colorPanel.Add((MyGuiControlBase) this.m_sliderHue);
      this.Controls.Add((MyGuiControlBase) this.m_sliderHue);
      this.m_sliderSaturation = new MyGuiControlSlider(width: 0.177f, defaultValue: new float?(0.0f), toolTip: string.Empty, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_sliderSaturation.Margin = new Thickness(0.0052f, -0.0425f, 0.0f, 0.0f);
      this.m_sliderSaturation.Enabled = this.m_activeTabState == MyGuiScreenLoadInventory.TabState.Character;
      colorPanel.Add((MyGuiControlBase) this.m_sliderSaturation);
      this.Controls.Add((MyGuiControlBase) this.m_sliderSaturation);
      this.m_sliderValue = new MyGuiControlSlider(width: 0.177f, defaultValue: new float?(0.0f), toolTip: string.Empty, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      this.m_sliderValue.Margin = new Thickness(3f / 500f, -0.0425f, 0.0f, 0.0f);
      this.m_sliderValue.Enabled = this.m_activeTabState == MyGuiScreenLoadInventory.TabState.Character;
      colorPanel.Add((MyGuiControlBase) this.m_sliderValue);
      this.Controls.Add((MyGuiControlBase) this.m_sliderValue);
    }

    private void Prepare_categoryButton(
      MyGuiControlImageButton control,
      int i,
      MyGuiControlImageButton.StyleDefinition selectedButtonStyle)
    {
      this.Controls.Add((MyGuiControlBase) control);
      if (this.m_filteredSlot != MyGameInventoryItemSlot.None && this.m_filteredSlot == (MyGameInventoryItemSlot) control.UserData)
      {
        control.ApplyStyle(selectedButtonStyle);
        control.Checked = true;
        control.Selected = true;
      }
      control.CanHaveFocus = false;
      control.Size = new Vector2(0.1f, 0.1f);
      float x1 = control.Position.X;
      if (this.m_categoryButtonsData == null || this.m_categoryButtonsData.Count <= i)
        return;
      Vector2? size;
      if (!string.IsNullOrEmpty(this.m_categoryButtonsData[i].ImageName))
      {
        size = new Vector2?(new Vector2(0.03f, 0.04f));
        MyGuiControlImage myGuiControlImage = new MyGuiControlImage(new Vector2?(control.Position + new Vector2(0.005f, 1f / 1000f)), size, textures: new string[1]
        {
          this.m_categoryButtonsData[i].ImageName
        }, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
        this.Controls.Add((MyGuiControlBase) myGuiControlImage);
        x1 += myGuiControlImage.Size.X;
      }
      if (string.IsNullOrEmpty(this.m_categoryButtonsData[i].ButtonText))
        return;
      size = new Vector2?(control.Size);
      MyGuiControlLabel buttonLabel = new MyGuiControlLabel(new Vector2?(new Vector2((float) (((double) x1 + (double) control.Position.X + (double) control.Size.X) / 2.0), control.Position.Y + control.Size.Y / 2.18f)), size, this.m_categoryButtonsData[i].ButtonText, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: true, maxWidth: (control.Size.X - 0.04f), isAutoScaleEnabled: true);
      this.Controls.Add((MyGuiControlBase) buttonLabel);
      control.HighlightChanged += (Action<MyGuiControlBase>) (x =>
      {
        if (x.HasHighlight)
          buttonLabel.ColorMask = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
        else
          buttonLabel.ColorMask = Vector4.One;
      });
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.SubscribeValueChangedEventsToSliders(false);
      MyGuiControlImageButton.StyleDefinition basicButtonStyle = new MyGuiControlImageButton.StyleDefinition()
      {
        Highlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_HIGHLIGHT
        },
        ActiveHighlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_ACTIVE
        },
        Active = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_ACTIVE
        },
        Focus = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_FOCUS
        },
        Normal = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_NORMAL
        }
      };
      Vector2 basicButtonSize = new Vector2(0.14f, 0.05f);
      MyGuiControlImageButton.StyleDefinition selectedButtonStyle = new MyGuiControlImageButton.StyleDefinition()
      {
        Highlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_HIGHLIGHT
        },
        ActiveHighlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_ACTIVE
        },
        Active = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_ACTIVE
        },
        Focus = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_FOCUS
        },
        Normal = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_NORMAL
        }
      };
      this.Controls.Add((MyGuiControlBase) this.Prepare_MyGuiControlSeparatorList());
      MyGuiControlStackPanel controlStackPanel1 = this.Prepare_sideStackPanel();
      MyGuiControlStackPanel controlStackPanel2 = this.Prepare_lowTabPanel();
      MyGuiControlStackPanel controlStackPanel3 = this.Prepare_tabsPanel();
      this.m_coloringButton = this.Prepare_coloringButton();
      controlStackPanel2.Add((MyGuiControlBase) this.m_coloringButton);
      this.Controls.Add((MyGuiControlBase) this.m_coloringButton);
      if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
      {
        this.Prepare_m_recyclingButton();
        controlStackPanel2.Add((MyGuiControlBase) this.m_recyclingButton);
        this.Controls.Add((MyGuiControlBase) this.m_recyclingButton);
      }
      this.Prepare_m_characterButton();
      controlStackPanel3.Add((MyGuiControlBase) this.m_characterButton);
      this.Controls.Add((MyGuiControlBase) this.m_characterButton);
      this.Prepare_m_toolsButton_And_m_categoryButtonsData();
      controlStackPanel3.Add((MyGuiControlBase) this.m_toolsButton);
      this.Controls.Add((MyGuiControlBase) this.m_toolsButton);
      this.Prepare_m_categoryButtonLayout(basicButtonStyle, basicButtonSize);
      if (this.m_modelPicker != null)
        this.m_modelPicker.ItemSelected -= new MyGuiControlCombobox.ItemSelectedDelegate(this.OnItemSelected);
      MyGuiControlStackPanel modelPanel = new MyGuiControlStackPanel();
      modelPanel.Orientation = MyGuiOrientation.Horizontal;
      modelPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      modelPanel.Margin = new Thickness(0.014f);
      if (this.m_activeLowTabState == MyGuiScreenLoadInventory.LowerTabState.Coloring)
        this.Prepare_LowerTabState_Coloring_ModelPanel(modelPanel);
      if (this.m_activeTabState == MyGuiScreenLoadInventory.TabState.Tools && this.m_filteredSlot != MyGameInventoryItemSlot.None)
        this.TabState_Filter();
      this.m_itemsTableSize = new Vector2(0.582f, 0.29f);
      this.m_itemsTableParent = new MyGuiControlParent(size: new Vector2?(new Vector2(this.m_itemsTableSize.X, 0.1f)));
      this.m_itemsTableParent.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_itemsTableParent.SkipForMouseTest = true;
      this.m_itemsTable = this.CreateItemsTable(this.m_itemsTableParent);
      controlStackPanel1.Add((MyGuiControlBase) this.m_itemsTable);
      controlStackPanel1.Add((MyGuiControlBase) controlStackPanel2);
      if (this.m_activeLowTabState == MyGuiScreenLoadInventory.LowerTabState.Coloring)
        this.Prepare_LowerTabState_Coloring(modelPanel);
      else
        this.Prepare_LowerTabState_NotColoring(modelPanel);
      MyGuiControlStackPanel controlStackPanel4 = new MyGuiControlStackPanel();
      controlStackPanel4.Orientation = MyGuiOrientation.Horizontal;
      controlStackPanel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlStackPanel4.Margin = new Thickness(0.018f);
      if (this.m_activeLowTabState == MyGuiScreenLoadInventory.LowerTabState.Coloring)
      {
        bool flag = this.m_OkButton == null || this.m_OkButton != null && this.m_OkButton.Enabled;
        this.m_OkButton = this.MakeButton(Vector2.Zero, MyCommonTexts.Ok, MyCommonTexts.ScreenLoadInventoryOkTooltip, new Action<MyGuiControlButton>(this.OnOkClick));
        this.m_OkButton.Enabled = flag;
        this.m_OkButton.Margin = new Thickness(0.0395f, -0.029f, 0.0075f, 0.0f);
        controlStackPanel4.Add((MyGuiControlBase) this.m_OkButton);
      }
      else
      {
        this.m_craftButton = this.MakeButton(Vector2.Zero, MyCommonTexts.CraftButton, MyCommonTexts.ScreenLoadInventoryCraftTooltip, new Action<MyGuiControlButton>(this.OnCraftClick));
        this.m_craftButton.Enabled = true;
        this.m_craftButton.Margin = new Thickness(0.0395f, -0.029f, 0.0075f, 0.0f);
        controlStackPanel4.Add((MyGuiControlBase) this.m_craftButton);
        uint craftingCost = MyGameService.GetCraftingCost(MyGameInventoryItemQuality.Common);
        this.m_craftButton.Text = string.Format(MyTexts.GetString(MyCommonTexts.CraftButton), (object) craftingCost);
        this.m_craftButton.Enabled = (long) MyGameService.RecycleTokens >= (long) craftingCost;
      }
      this.m_cancelButton = this.MakeButton(Vector2.Zero, MyCommonTexts.Cancel, MyCommonTexts.ScreenLoadInventoryCancelTooltip, new Action<MyGuiControlButton>(this.OnCancelClick));
      this.m_cancelButton.Margin = new Thickness(0.0f, -0.029f, 0.0075f, 0.03f);
      controlStackPanel4.Add((MyGuiControlBase) this.m_cancelButton);
      if (MyGuiScreenLoadInventory.SKIN_STORE_FEATURES_ENABLED)
      {
        this.m_openStoreButton = this.MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryBrowseItems, MyCommonTexts.ScreenLoadInventoryBrowseItems, new Action<MyGuiControlButton>(this.OnOpenStore));
        controlStackPanel4.Add((MyGuiControlBase) this.m_openStoreButton);
      }
      else
      {
        this.m_refreshButton = this.MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadSubscribedWorldRefresh, MyCommonTexts.ScreenLoadInventoryRefreshTooltip, new Action<MyGuiControlButton>(this.OnRefreshClick));
        this.m_refreshButton.Margin = new Thickness(0.0f, -0.029f, 0.0f, 0.0f);
        controlStackPanel4.Add((MyGuiControlBase) this.m_refreshButton);
      }
      this.m_wheel = new MyGuiControlRotatingWheel(new Vector2?(Vector2.Zero), new Vector4?(MyGuiConstants.ROTATING_WHEEL_COLOR), 0.2f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, this.m_rotatingWheelTexture, false, MyPerGameSettings.GUI.MultipleSpinningWheels);
      this.m_wheel.ManualRotationUpdate = false;
      this.m_wheel.Margin = new Thickness(0.21f, 0.047f, 0.0f, 0.0f);
      this.Elements.Add((MyGuiControlBase) this.m_wheel);
      controlStackPanel3.Add((MyGuiControlBase) this.m_wheel);
      MyGuiControlStackPanel controlStackPanel5 = new MyGuiControlStackPanel();
      controlStackPanel5.Orientation = MyGuiOrientation.Horizontal;
      controlStackPanel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      if (this.m_activeLowTabState == MyGuiScreenLoadInventory.LowerTabState.Coloring)
        this.Prepare_LowerTabState_Sliders(controlStackPanel5);
      else
        this.CreateRecyclerUI(controlStackPanel5);
      MyGuiControlLayoutGrid controlLayoutGrid1 = new MyGuiControlLayoutGrid(new GridLength[1]
      {
        new GridLength(1f)
      }, new GridLength[7]
      {
        new GridLength(0.6f),
        new GridLength(0.5f),
        new GridLength(0.8f),
        new GridLength(4.6f),
        new GridLength(0.6f),
        new GridLength(0.6f),
        new GridLength(0.8f)
      });
      controlLayoutGrid1.Size = new Vector2(0.65f, 0.9f);
      controlLayoutGrid1.Add((MyGuiControlBase) controlStackPanel3, 0, 1);
      controlLayoutGrid1.Add((MyGuiControlBase) this.m_categoryButtonLayout, 0, 2);
      if (MyGameService.InventoryItems != null && MyGameService.InventoryItems.Count == 0)
      {
        MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryNoItem));
        myGuiControlLabel.Margin = new Thickness(0.015f);
        controlLayoutGrid1.Add((MyGuiControlBase) myGuiControlLabel, 0, 3);
        this.Elements.Add((MyGuiControlBase) myGuiControlLabel);
      }
      controlLayoutGrid1.Add((MyGuiControlBase) controlStackPanel1, 0, 3);
      controlLayoutGrid1.Add((MyGuiControlBase) modelPanel, 0, 4);
      controlLayoutGrid1.Add((MyGuiControlBase) controlStackPanel5, 0, 5);
      controlLayoutGrid1.Add((MyGuiControlBase) controlStackPanel4, 0, 6);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenCaptionInventory), colorMask: new Vector4?(Vector4.One), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
      myGuiControlLabel1.Name = "CaptionLabel";
      myGuiControlLabel1.Font = "ScreenCaption";
      this.Elements.Add((MyGuiControlBase) myGuiControlLabel1);
      controlLayoutGrid1.Add((MyGuiControlBase) myGuiControlLabel1, 0, 0);
      MyGuiControlLayoutGrid controlLayoutGrid2 = new MyGuiControlLayoutGrid(new GridLength[1]
      {
        new GridLength(1f)
      }, new GridLength[1]{ new GridLength(1f) });
      controlLayoutGrid2.Size = new Vector2(1f, 1f);
      controlLayoutGrid2.Add((MyGuiControlBase) controlLayoutGrid1, 0, 0);
      controlLayoutGrid2.UpdateMeasure();
      this.m_itemsTableParent.Size = new Vector2(this.m_itemsTableSize.X, this.m_itemsTable.Size.Y);
      this.m_itemsTable.Size = this.m_itemsTableSize;
      controlLayoutGrid2.UpdateArrange();
      this.Controls.Add((MyGuiControlBase) this.Prepare_scrollPanel());
      myGuiControlLabel1.Position = new Vector2(myGuiControlLabel1.Position.X + this.Size.Value.X / 2f, (float) ((double) myGuiControlLabel1.Position.Y + (double) MyGuiConstants.SCREEN_CAPTION_DELTA_Y / 3.0 + 23.0 / 1000.0));
      foreach (MyGuiControlBase control in controlStackPanel4.GetControls())
        this.Controls.Add(control);
      List<MyGuiControlBase> controls = this.m_categoryButtonLayout.GetControls();
      for (int index = 0; index < controls.Count; ++index)
      {
        if (controls[index] is MyGuiControlImageButton control)
          this.Prepare_categoryButton(control, index, selectedButtonStyle);
      }
      this.m_wheel.Visible = false;
      this.CloseButtonEnabled = true;
      this.m_storedHSV = MySession.Static.LocalCharacter.ColorMask;
      this.m_selectedHSV = this.m_storedHSV;
      this.m_sliderHue.Value = this.m_selectedHSV.X * 360f;
      this.m_sliderSaturation.Value = MathHelper.Clamp(this.m_selectedHSV.Y + MyColorPickerConstants.SATURATION_DELTA, 0.0f, 1f);
      this.m_sliderValue.Value = MathHelper.Clamp(this.m_selectedHSV.Z + MyColorPickerConstants.VALUE_DELTA - MyColorPickerConstants.VALUE_COLORIZE_DELTA, 0.0f, 1f);
      this.SubscribeValueChangedEventsToSliders(true);
      this.m_contextMenu = new MyGuiControlContextMenu();
      this.m_contextMenu.CreateNewContextMenu();
      if (MyGuiScreenLoadInventory.SKIN_STORE_FEATURES_ENABLED && MyGameService.IsOverlayEnabled)
      {
        StringBuilder text = MyTexts.Get(MyCommonTexts.ScreenLoadInventoryBuyItem);
        this.m_contextMenu.AddItem(text, text.ToString(), "", (object) MyGuiScreenLoadInventory.InventoryItemAction.Buy);
      }
      StringBuilder text1 = MyTexts.Get(MyCommonTexts.ScreenLoadInventorySellItem);
      if (MyGameService.IsOverlayEnabled)
        this.m_contextMenu.AddItem(text1, text1.ToString(), "", (object) MyGuiScreenLoadInventory.InventoryItemAction.Sell);
      StringBuilder text2 = MyTexts.Get(MyCommonTexts.ScreenLoadInventoryRecycleItem);
      string.Format(text2.ToString(), (object) 0);
      this.m_contextMenu.AddItem(text2, string.Empty, "", (object) MyGuiScreenLoadInventory.InventoryItemAction.Recycle);
      this.m_contextMenu.ItemClicked += new Action<MyGuiControlContextMenu, MyGuiControlContextMenu.EventArgs>(this.m_contextMenu_ItemClicked);
      this.Controls.Add((MyGuiControlBase) this.m_contextMenu);
      this.m_contextMenu.Deactivate();
      if (constructor)
        this.m_colorOrModelChanged = false;
      Vector2 vector2 = new Vector2();
      if (this.m_OkButton != null)
        vector2 = this.m_OkButton.Position;
      else if (this.m_craftButton != null)
        vector2 = this.m_craftButton.Position;
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2?(new Vector2(vector2.X, vector2.Y + minSizeGui.Y / 2f)));
      myGuiControlLabel2.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
      if (this.m_activeLowTabState == MyGuiScreenLoadInventory.LowerTabState.Coloring)
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.CharacterSkinInventory_Help_ScreenOK);
      else
        this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.CharacterSkinInventory_Help_ScreenCraft);
      this.UpdateSliderTooltips();
      this.UpdateGamepadHelp(this.FocusedControl);
      this.m_focusButton = true;
    }

    private void FocusButton(MyGuiScreenBase obj)
    {
      if (MyScreenManager.GetScreenWithFocus() != this || this.m_itemsTable == null)
        return;
      MyGuiControlBase firstVisible = this.m_itemsTable.GetFirstVisible();
      if (firstVisible == null || !(firstVisible is MyGuiControlLayoutGrid controlLayoutGrid))
        return;
      foreach (MyGuiControlBase myGuiControlBase in controlLayoutGrid.GetControlsAt(0, 0))
      {
        if (myGuiControlBase is MyGuiControlImageButton)
        {
          this.FocusedControl = myGuiControlBase;
          break;
        }
      }
    }

    private void OnViewTabColoring(MyGuiControlButton obj)
    {
      this.m_activeLowTabState = MyGuiScreenLoadInventory.LowerTabState.Coloring;
      this.EquipTool();
      this.RecreateControls(false);
      this.UpdateCheckboxes();
    }

    private void CreateRecyclerUI(MyGuiControlStackPanel panel)
    {
      MyGuiControlLayoutGrid controlLayoutGrid = new MyGuiControlLayoutGrid(new GridLength[3]
      {
        new GridLength(1.4f),
        new GridLength(0.6f),
        new GridLength(0.8f)
      }, new GridLength[1]{ new GridLength(1f) });
      controlLayoutGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlLayoutGrid.Margin = new Thickness(0.055f, -0.035f, 0.0f, 0.0f);
      controlLayoutGrid.Size = new Vector2(0.65f, 0.1f);
      MyGuiControlStackPanel controlStackPanel = new MyGuiControlStackPanel();
      controlStackPanel.Orientation = MyGuiOrientation.Horizontal;
      controlStackPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      controlStackPanel.Margin = new Thickness(0.0f);
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenLoadInventorySelectRarity));
      myGuiControlLabel1.Margin = new Thickness(0.0f, 0.0f, 0.01f, 0.0f);
      controlStackPanel.Add((MyGuiControlBase) myGuiControlLabel1);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
      this.m_rarityPicker = new MyGuiControlCombobox();
      this.m_rarityPicker.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
      this.m_rarityPicker.Size = new Vector2(0.15f, 0.0f);
      foreach (object obj in Enum.GetValues(typeof (MyGameInventoryItemQuality)))
        this.m_rarityPicker.AddItem((long) (int) obj, MyTexts.GetString(MyStringId.GetOrCompute(obj.ToString())));
      this.m_rarityPicker.SelectItemByIndex(0);
      this.m_rarityPicker.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.m_rarityPicker_ItemSelected);
      controlStackPanel.Add((MyGuiControlBase) this.m_rarityPicker);
      this.Controls.Add((MyGuiControlBase) this.m_rarityPicker);
      if (this.m_lastCraftedItem != null)
      {
        Vector2 vector2 = new Vector2(0.07f, 0.09f);
        MyGuiControlImage myGuiControlImage1;
        if (string.IsNullOrEmpty(this.m_lastCraftedItem.ItemDefinition.BackgroundColor))
        {
          myGuiControlImage1 = new MyGuiControlImage(size: new Vector2?(vector2), textures: new string[1]
          {
            "Textures\\GUI\\Controls\\grid_item_highlight.dds"
          }, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
          this.Controls.Add((MyGuiControlBase) myGuiControlImage1);
        }
        else
        {
          Vector4 vector4 = string.IsNullOrEmpty(this.m_lastCraftedItem.ItemDefinition.BackgroundColor) ? Vector4.One : ColorExtensions.HexToVector4(this.m_lastCraftedItem.ItemDefinition.BackgroundColor);
          myGuiControlImage1 = new MyGuiControlImage(size: new Vector2?(new Vector2(vector2.X - 0.004f, vector2.Y - 1f / 500f)), backgroundColor: new Vector4?(vector4), textures: new string[1]
          {
            "Textures\\GUI\\blank.dds"
          });
          this.Controls.Add((MyGuiControlBase) myGuiControlImage1);
        }
        string[] textures = new string[1]
        {
          "Textures\\GUI\\Blank.dds"
        };
        if (!string.IsNullOrEmpty(this.m_lastCraftedItem.ItemDefinition.IconTexture))
          textures[0] = this.m_lastCraftedItem.ItemDefinition.IconTexture;
        MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(size: new Vector2?(new Vector2(0.06f, 0.08f)), textures: textures);
        this.Controls.Add((MyGuiControlBase) myGuiControlImage2);
        MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(text: this.m_lastCraftedItem.ItemDefinition.Name, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
        MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryCraftedLabel), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
        myGuiControlLabel3.Margin = new Thickness(0.0f, -0.035f, 0.0f, 0.0f);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
        controlLayoutGrid.Add((MyGuiControlBase) myGuiControlImage1, 1, 0);
        controlLayoutGrid.Add((MyGuiControlBase) myGuiControlImage2, 1, 0);
        controlLayoutGrid.Add((MyGuiControlBase) myGuiControlLabel2, 2, 0);
        controlLayoutGrid.Add((MyGuiControlBase) myGuiControlLabel3, 2, 0);
      }
      controlLayoutGrid.Add((MyGuiControlBase) controlStackPanel, 0, 0);
      panel.Add((MyGuiControlBase) controlLayoutGrid);
    }

    private void OnOkClick(MyGuiControlButton obj)
    {
      if (this.m_colorOrModelChanged && MyGuiScreenLoadInventory.LookChanged != null && MySession.Static != null)
        MyGuiScreenLoadInventory.LookChanged();
      if (MySession.Static.LocalCharacter.Definition.UsableByPlayer)
        MyLocalCache.SaveInventoryConfig(MySession.Static.LocalCharacter);
      this.CloseScreen(false);
    }

    private void OnCancelClick(MyGuiControlButton obj)
    {
      MyGuiScreenLoadInventory.Cancel();
      this.CloseScreen(false);
    }

    private void OnCraftClick(MyGuiControlButton obj)
    {
      MyGameService.ItemsAdded -= new EventHandler<MyGameItemsEventArgs>(this.MyGameService_ItemsAdded);
      MyGameService.ItemsAdded += new EventHandler<MyGameItemsEventArgs>(this.MyGameService_ItemsAdded);
      if (MyGameService.CraftSkin((MyGameInventoryItemQuality) this.m_rarityPicker.GetSelectedKey()))
        this.RotatingWheelShow();
      else
        MyGameService.ItemsAdded -= new EventHandler<MyGameItemsEventArgs>(this.MyGameService_ItemsAdded);
    }

    private void MyGameService_ItemsAdded(object sender, MyGameItemsEventArgs e)
    {
      if (e.NewItems != null && e.NewItems.Count > 0)
      {
        this.m_lastCraftedItem = e.NewItems[0];
        this.m_lastCraftedItem.IsNew = true;
        MyGameService.ItemsAdded -= new EventHandler<MyGameItemsEventArgs>(this.MyGameService_ItemsAdded);
        this.RefreshUI();
      }
      this.RotatingWheelHide();
    }

    private static void Cancel()
    {
      if (MyGameService.InventoryItems != null)
      {
        foreach (MyGameInventoryItem inventoryItem in (IEnumerable<MyGameInventoryItem>) MyGameService.InventoryItems)
          inventoryItem.UsingCharacters.Remove(MySession.Static.LocalCharacter.EntityId);
      }
      MyLocalCache.LoadInventoryConfig(MySession.Static.LocalCharacter);
    }

    private void m_toolPicker_ItemSelected()
    {
      MyPhysicalInventoryItem physicalInventoryItem = this.m_allTools.FirstOrDefault<MyPhysicalInventoryItem>((Func<MyPhysicalInventoryItem, bool>) (t => (long) t.ItemId == this.m_toolPicker.GetSelectedKey()));
      if (physicalInventoryItem.Content == null)
        return;
      this.m_selectedTool = physicalInventoryItem.Content.SubtypeName;
      this.EquipTool();
    }

    private void m_rarityPicker_ItemSelected()
    {
      uint craftingCost = MyGameService.GetCraftingCost((MyGameInventoryItemQuality) this.m_rarityPicker.GetSelectedIndex());
      this.m_craftButton.Text = string.Format(MyTexts.GetString(MyCommonTexts.CraftButton), (object) craftingCost);
      this.m_craftButton.Enabled = (long) MyGameService.RecycleTokens >= (long) craftingCost;
    }

    private void OnHideDuplicates(MyGuiControlCheckbox obj)
    {
      this.m_hideDuplicatesEnabled = obj.IsChecked;
      this.RefreshUI();
    }

    private void OnShowOnlyDuplicates(MyGuiControlCheckbox obj)
    {
      this.m_showOnlyDuplicatesEnabled = obj.IsChecked;
      this.RefreshUI();
    }

    private void m_contextMenu_ItemClicked(
      MyGuiControlContextMenu contextMenu,
      MyGuiControlContextMenu.EventArgs selectedItem)
    {
      switch ((MyGuiScreenLoadInventory.InventoryItemAction) selectedItem.UserData)
      {
        case MyGuiScreenLoadInventory.InventoryItemAction.Apply:
          this.hiddenButton_ButtonClicked(this.m_contextMenuLastButton);
          break;
        case MyGuiScreenLoadInventory.InventoryItemAction.Sell:
          this.OpenUserInventory();
          break;
        case MyGuiScreenLoadInventory.InventoryItemAction.Recycle:
          this.RecycleItemRequest();
          break;
        case MyGuiScreenLoadInventory.InventoryItemAction.Delete:
          this.DeleteItemRequest();
          break;
        case MyGuiScreenLoadInventory.InventoryItemAction.Buy:
          this.OpenCurrentItemInStore();
          break;
      }
    }

    private void OnViewTools(MyGuiControlButton obj)
    {
      this.m_activeTabState = MyGuiScreenLoadInventory.TabState.Tools;
      this.m_filteredSlot = MyGameInventoryItemSlot.Welder;
      this.m_selectedTool = string.Empty;
      this.m_currentCategoryIndex = 0;
      this.RefreshUI();
    }

    private void OnViewTabCharacter(MyGuiControlButton obj)
    {
      this.m_activeTabState = MyGuiScreenLoadInventory.TabState.Character;
      this.m_filteredSlot = MyGameInventoryItemSlot.None;
      this.m_selectedTool = string.Empty;
      this.m_currentCategoryIndex = -1;
      this.EquipTool();
      this.RecreateControls(false);
      this.UpdateCheckboxes();
    }

    private void OnViewTabRecycling(MyGuiControlButton obj)
    {
      this.m_activeLowTabState = MyGuiScreenLoadInventory.LowerTabState.Recycling;
      this.EquipTool();
      this.RecreateControls(false);
      this.UpdateCheckboxes();
    }

    private void OnItemSelected()
    {
      MyGuiScreenLoadInventory.Cancel();
      this.m_selectedModel = this.m_models[(int) this.m_modelPicker.GetSelectedKey()];
      this.ChangeCharacter(this.m_selectedModel, this.m_selectedHSV);
      this.RecreateControls(false);
      MyLocalCache.ResetAllInventorySlots(MySession.Static.LocalCharacter);
      this.RefreshItems();
    }

    private void ChangeCharacter(string model, Vector3 colorMaskHSV, bool resetToDefault = true)
    {
      this.m_colorOrModelChanged = true;
      MySession.Static.LocalCharacter.ChangeModelAndColor(model, colorMaskHSV, resetToDefault, MySession.Static.LocalPlayerId);
    }

    public static void ResetOnFinish(string model, bool resetToDefault)
    {
      MyGuiScreenLoadInventory firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoadInventory>();
      if (firstScreenOfType == null || firstScreenOfType.m_selectedModel == MySession.Static.LocalCharacter.ModelName)
        return;
      if (resetToDefault)
        firstScreenOfType.ResetOnFinishInternal(model);
      firstScreenOfType.RecreateControls(false);
      MyLocalCache.ResetAllInventorySlots(MySession.Static.LocalCharacter);
      firstScreenOfType.RefreshItems();
    }

    private void ResetOnFinishInternal(string model)
    {
      if (model == "Default_Astronaut" || model == "Default_Astronaut_Female")
      {
        MyLocalCache.LoadInventoryConfig(MySession.Static.LocalCharacter, false);
      }
      else
      {
        this.m_selectedModel = this.m_models[(int) this.m_modelPicker.GetSelectedKey()];
        MyGuiScreenLoadInventory.Cancel();
        this.RecreateControls(false);
        MyLocalCache.ResetAllInventorySlots(MySession.Static.LocalCharacter);
        this.RefreshItems();
      }
    }

    private void OnValueChange(MyGuiControlSlider sender)
    {
      this.UpdateSliderTooltips();
      this.m_selectedHSV.X = this.m_sliderHue.Value / 360f;
      this.m_selectedHSV.Y = this.m_sliderSaturation.Value - MyColorPickerConstants.SATURATION_DELTA;
      this.m_selectedHSV.Z = this.m_sliderValue.Value - MyColorPickerConstants.VALUE_DELTA + MyColorPickerConstants.VALUE_COLORIZE_DELTA;
      this.ChangeCharacter(this.m_selectedModel, this.m_selectedHSV, false);
    }

    private void UpdateSliderTooltips()
    {
      this.m_sliderHue.Tooltips.ToolTips.Clear();
      this.m_sliderHue.Tooltips.AddToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryHue), (object) this.m_sliderHue.Value));
      this.m_sliderSaturation.Tooltips.ToolTips.Clear();
      this.m_sliderSaturation.Tooltips.AddToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventorySaturation), (object) this.m_sliderSaturation.Value.ToString("P1")));
      this.m_sliderValue.Tooltips.ToolTips.Clear();
      this.m_sliderValue.Tooltips.AddToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryValue), (object) this.m_sliderValue.Value.ToString("P1")));
    }

    private MyGuiControlWrapPanel CreateItemsTable(MyGuiControlParent parent)
    {
      Vector2 itemSize = new Vector2(0.07f, 0.09f);
      MyGuiControlWrapPanel controlWrapPanel = new MyGuiControlWrapPanel(itemSize);
      controlWrapPanel.Size = this.m_itemsTableSize;
      controlWrapPanel.Margin = new Thickness(0.018f, 0.044f, 0.0f, 0.0f);
      controlWrapPanel.InnerOffset = new Vector2(0.005f, 0.0065f);
      controlWrapPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiControlImageButton.StyleDefinition style1 = new MyGuiControlImageButton.StyleDefinition()
      {
        Highlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = new MyGuiCompositeTexture("Textures\\Gui\\Screens\\screen_background_fade.dds")
        },
        ActiveHighlight = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = new MyGuiCompositeTexture("Textures\\Gui\\Screens\\screen_background_fade.dds")
        },
        Normal = new MyGuiControlImageButton.StateDefinition()
        {
          Texture = new MyGuiCompositeTexture("Textures\\Gui\\Screens\\screen_background_fade.dds")
        }
      };
      MyGuiControlCheckbox.StyleDefinition style2 = new MyGuiControlCheckbox.StyleDefinition()
      {
        NormalCheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_GREEN_CHECKED,
        NormalUncheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_BLANK,
        HighlightCheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_BLANK,
        HighlightUncheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_BLANK
      };
      this.m_itemCheckboxes = new List<MyGuiControlCheckbox>();
      this.m_userItems = this.GetInventoryItems();
      if (MyGuiScreenLoadInventory.SKIN_STORE_FEATURES_ENABLED)
        this.m_userItems.AddRange((IEnumerable<MyGameInventoryItem>) MyGuiScreenLoadInventory.GetStoreItems(this.m_userItems));
      for (int index = 0; index < this.m_userItems.Count; ++index)
      {
        MyGameInventoryItem userItem = this.m_userItems[index];
        if (userItem.ItemDefinition.ItemSlot != MyGameInventoryItemSlot.None && userItem.ItemDefinition.ItemSlot != MyGameInventoryItemSlot.Emote && userItem.ItemDefinition.ItemSlot != MyGameInventoryItemSlot.Armor && (this.m_filteredSlot == MyGameInventoryItemSlot.None || this.m_filteredSlot == userItem.ItemDefinition.ItemSlot))
        {
          if (this.m_filteredSlot == MyGameInventoryItemSlot.None)
          {
            MyGameInventoryItemSlot itemSlot = userItem.ItemDefinition.ItemSlot;
            switch (this.m_activeTabState)
            {
              case MyGuiScreenLoadInventory.TabState.Character:
                if (itemSlot == MyGameInventoryItemSlot.Grinder || itemSlot == MyGameInventoryItemSlot.Rifle || (itemSlot == MyGameInventoryItemSlot.Welder || itemSlot == MyGameInventoryItemSlot.Drill))
                  continue;
                break;
              case MyGuiScreenLoadInventory.TabState.Tools:
                if (itemSlot == MyGameInventoryItemSlot.Helmet || itemSlot == MyGameInventoryItemSlot.Gloves || (itemSlot == MyGameInventoryItemSlot.Suit || itemSlot == MyGameInventoryItemSlot.Boots))
                  continue;
                break;
            }
          }
          MyGuiControlLayoutGrid controlLayoutGrid = new MyGuiControlLayoutGrid(new GridLength[2]
          {
            new GridLength(1f),
            new GridLength(1f)
          }, new GridLength[2]
          {
            new GridLength(1f),
            new GridLength(0.0f)
          });
          controlLayoutGrid.Size = itemSize;
          controlLayoutGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          Vector2? position = new Vector2?(controlLayoutGrid.Position);
          Vector2? size = new Vector2?();
          Vector4? colorMask = new Vector4?();
          Action<MyGuiControlImageButton> action1 = new Action<MyGuiControlImageButton>(this.hiddenButton_ButtonClicked);
          Action<MyGuiControlImageButton> action2 = new Action<MyGuiControlImageButton>(this.hiddenButton_ButtonRightClicked);
          string name = userItem.ItemDefinition.Name;
          Action<MyGuiControlImageButton> onButtonClick = action1;
          Action<MyGuiControlImageButton> onButtonRightClick = action2;
          int? buttonIndex = new int?();
          MyGuiControlImageButton controlImageButton = new MyGuiControlImageButton(position: position, size: size, colorMask: colorMask, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, toolTip: name, onButtonClick: onButtonClick, onButtonRightClick: onButtonRightClick, buttonIndex: buttonIndex);
          if (!MyInput.Static.IsJoystickLastUsed)
            controlImageButton.Tooltips.AddToolTip(string.Empty);
          if (!MyInput.Static.IsJoystickLastUsed && userItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Helmet)
          {
            MyControl gameControl = MyInput.Static.GetGameControl(MyControlsSpace.HELMET);
            string toolTip = string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryToggleHelmet), (object) gameControl.GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
            controlImageButton.Tooltips.AddToolTip(toolTip);
          }
          if (!MyInput.Static.IsJoystickLastUsed)
          {
            controlImageButton.Tooltips.AddToolTip(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryLeftClickTip));
            controlImageButton.Tooltips.AddToolTip(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryRightClickTip));
          }
          controlImageButton.ApplyStyle(style1);
          controlImageButton.Size = controlLayoutGrid.Size;
          controlImageButton.HighlightChanged += (Action<MyGuiControlBase>) (x => this.ChangeButtonBorder(x));
          controlImageButton.FocusChanged += (Action<MyGuiControlBase, bool>) ((x, y) => this.ChangeButtonBorder(x));
          parent.Controls.Add((MyGuiControlBase) controlImageButton);
          MyGuiControlImage myGuiControlImage1;
          if (string.IsNullOrEmpty(userItem.ItemDefinition.BackgroundColor))
          {
            myGuiControlImage1 = new MyGuiControlImage(size: new Vector2?(itemSize), textures: new string[1]
            {
              "Textures\\GUI\\Controls\\grid_item_highlight.dds"
            }, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            parent.Controls.Add((MyGuiControlBase) myGuiControlImage1);
          }
          else
          {
            Vector4 vector4 = string.IsNullOrEmpty(userItem.ItemDefinition.BackgroundColor) ? Vector4.One : ColorExtensions.HexToVector4(userItem.ItemDefinition.BackgroundColor);
            myGuiControlImage1 = new MyGuiControlImage(size: new Vector2?(new Vector2(itemSize.X - 0.004f, itemSize.Y - 1f / 500f)), backgroundColor: new Vector4?(vector4), textures: new string[1]
            {
              "Textures\\GUI\\blank.dds"
            }, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            parent.Controls.Add((MyGuiControlBase) myGuiControlImage1);
            myGuiControlImage1.Margin = new Thickness(0.0023f, 1f / 1000f, 0.0f, 0.0f);
          }
          myGuiControlImage1.Name = MyGuiScreenLoadInventory.IMAGE_PREFIX;
          string[] textures = new string[1]
          {
            "Textures\\GUI\\Blank.dds"
          };
          if (!string.IsNullOrEmpty(userItem.ItemDefinition.IconTexture))
            textures[0] = userItem.ItemDefinition.IconTexture;
          MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(size: new Vector2?(new Vector2(0.06f, 0.08f)), textures: textures, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
          myGuiControlImage2.Margin = new Thickness(0.005f, 0.005f, 0.0f, 0.0f);
          parent.Controls.Add((MyGuiControlBase) myGuiControlImage2);
          MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
          guiControlCheckbox.Name = this.m_equipCheckbox;
          guiControlCheckbox.ApplyStyle(style2);
          guiControlCheckbox.Margin = new Thickness(0.005f, 0.005f, 0.01f, 0.01f);
          guiControlCheckbox.IsHitTestVisible = false;
          guiControlCheckbox.CanHaveFocus = false;
          parent.Controls.Add((MyGuiControlBase) guiControlCheckbox);
          guiControlCheckbox.UserData = (object) userItem;
          controlImageButton.UserData = (object) controlLayoutGrid;
          this.m_itemCheckboxes.Add(guiControlCheckbox);
          controlLayoutGrid.Add((MyGuiControlBase) myGuiControlImage1, 0, 0);
          controlLayoutGrid.Add((MyGuiControlBase) controlImageButton, 0, 0);
          controlLayoutGrid.Add((MyGuiControlBase) myGuiControlImage2, 0, 0);
          controlLayoutGrid.Add((MyGuiControlBase) guiControlCheckbox, 1, 0);
          if (userItem.IsNew)
          {
            MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage(size: new Vector2?(new Vector2(7f / 400f, 23f / 1000f)), textures: new string[1]
            {
              "Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png"
            }, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
            myGuiControlImage3.Margin = new Thickness(0.01f, -0.035f, 0.0f, 0.0f);
            parent.Controls.Add((MyGuiControlBase) myGuiControlImage3);
            controlLayoutGrid.Add((MyGuiControlBase) myGuiControlImage3, 1, 1);
          }
          controlWrapPanel.Add((MyGuiControlBase) controlLayoutGrid);
          parent.Controls.Add((MyGuiControlBase) controlLayoutGrid);
        }
      }
      return controlWrapPanel;
    }

    private void ChangeButtonBorder(MyGuiControlBase obj)
    {
      if (obj.HasHighlight)
      {
        MyGuiControlImage myGuiControlImage = (MyGuiControlImage) null;
        foreach (MyGuiControlBase myGuiControlBase in (obj.UserData as MyGuiControlLayoutGrid).GetControlsAt(0, 0))
        {
          if (myGuiControlBase.Name.Length >= MyGuiScreenLoadInventory.IMAGE_PREFIX.Length && myGuiControlBase.Name.Substring(0, MyGuiScreenLoadInventory.IMAGE_PREFIX.Length) == MyGuiScreenLoadInventory.IMAGE_PREFIX)
          {
            myGuiControlImage = myGuiControlBase as MyGuiControlImage;
            break;
          }
        }
        if (myGuiControlImage == null)
          return;
        myGuiControlImage.ColorMask = MyGuiConstants.HIGHLIGHT_BACKGROUND_COLOR;
      }
      else if (obj.HasFocus)
      {
        MyGuiControlImage myGuiControlImage = (MyGuiControlImage) null;
        foreach (MyGuiControlBase myGuiControlBase in (obj.UserData as MyGuiControlLayoutGrid).GetControlsAt(0, 0))
        {
          if (myGuiControlBase.Name.Length >= MyGuiScreenLoadInventory.IMAGE_PREFIX.Length && myGuiControlBase.Name.Substring(0, MyGuiScreenLoadInventory.IMAGE_PREFIX.Length) == MyGuiScreenLoadInventory.IMAGE_PREFIX)
          {
            myGuiControlImage = myGuiControlBase as MyGuiControlImage;
            break;
          }
        }
        if (myGuiControlImage == null)
          return;
        myGuiControlImage.ColorMask = MyGuiConstants.FOCUS_BACKGROUND_COLOR;
      }
      else
      {
        List<MyGuiControlBase> controlsAt = (obj.UserData as MyGuiControlLayoutGrid).GetControlsAt(1, 0);
        if (controlsAt == null || controlsAt.Count <= 0 || !(controlsAt[0].UserData is MyGameInventoryItem userData))
          return;
        MyGuiControlImage myGuiControlImage = (MyGuiControlImage) null;
        foreach (MyGuiControlBase myGuiControlBase in (obj.UserData as MyGuiControlLayoutGrid).GetControlsAt(0, 0))
        {
          if (myGuiControlBase.Name.Length >= MyGuiScreenLoadInventory.IMAGE_PREFIX.Length && myGuiControlBase.Name.Substring(0, MyGuiScreenLoadInventory.IMAGE_PREFIX.Length) == MyGuiScreenLoadInventory.IMAGE_PREFIX)
          {
            myGuiControlImage = myGuiControlBase as MyGuiControlImage;
            break;
          }
        }
        if (myGuiControlImage == null)
          return;
        myGuiControlImage.ColorMask = string.IsNullOrEmpty(userData.ItemDefinition.BackgroundColor) ? Vector4.One : ColorExtensions.HexToVector4(userData.ItemDefinition.BackgroundColor);
      }
    }

    private static List<MyGameInventoryItem> GetStoreItems(
      List<MyGameInventoryItem> userItems)
    {
      List<MyGameInventoryItemDefinition> inventoryItemDefinitionList = new List<MyGameInventoryItemDefinition>((IEnumerable<MyGameInventoryItemDefinition>) MyGameService.Definitions.Values);
      List<MyGameInventoryItem> source = new List<MyGameInventoryItem>();
      foreach (MyGameInventoryItemDefinition inventoryItemDefinition in inventoryItemDefinitionList)
      {
        MyGameInventoryItemDefinition item = inventoryItemDefinition;
        if (item.DefinitionType == MyGameInventoryItemDefinitionType.item && item.ItemSlot != MyGameInventoryItemSlot.None && (!item.Hidden && !item.IsStoreHidden) && userItems.FirstOrDefault<MyGameInventoryItem>((Func<MyGameInventoryItem, bool>) (i => i.ItemDefinition.ID == item.ID)) == null)
        {
          MyGameInventoryItem gameInventoryItem = new MyGameInventoryItem()
          {
            ID = 0,
            IsStoreFakeItem = true,
            ItemDefinition = item,
            Quantity = 1
          };
          source.Add(gameInventoryItem);
        }
      }
      return source.OrderBy<MyGameInventoryItem, string>((Func<MyGameInventoryItem, string>) (i => i.ItemDefinition.Name)).ToList<MyGameInventoryItem>();
    }

    private List<MyGameInventoryItem> GetInventoryItems()
    {
      List<MyGameInventoryItem> source1 = new List<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>) MyGameService.InventoryItems);
      List<MyGameInventoryItem> list;
      if (this.m_activeLowTabState == MyGuiScreenLoadInventory.LowerTabState.Coloring)
      {
        if (source1.Count > 0 && this.m_hideDuplicatesEnabled)
        {
          List<MyGameInventoryItem> source2 = new List<MyGameInventoryItem>();
          source2.AddRange(source1.Where<MyGameInventoryItem>((Func<MyGameInventoryItem, bool>) (i => i.IsNew)));
          source2.AddRange(source1.Where<MyGameInventoryItem>((Func<MyGameInventoryItem, bool>) (i => i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))));
          foreach (MyGameInventoryItem gameInventoryItem in source1)
          {
            MyGameInventoryItem item = gameInventoryItem;
            if (!item.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId) && source2.FirstOrDefault<MyGameInventoryItem>((Func<MyGameInventoryItem, bool>) (i => i.ItemDefinition.AssetModifierId == item.ItemDefinition.AssetModifierId)) == null)
              source2.Add(item);
          }
          list = source2.OrderByDescending<MyGameInventoryItem, bool>((Func<MyGameInventoryItem, bool>) (i => i.IsNew)).ThenByDescending<MyGameInventoryItem, bool>((Func<MyGameInventoryItem, bool>) (i => i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))).ThenBy<MyGameInventoryItem, string>((Func<MyGameInventoryItem, string>) (i => i.ItemDefinition.Name)).ToList<MyGameInventoryItem>();
        }
        else
          list = source1.OrderByDescending<MyGameInventoryItem, bool>((Func<MyGameInventoryItem, bool>) (i => i.IsNew)).ThenByDescending<MyGameInventoryItem, bool>((Func<MyGameInventoryItem, bool>) (i => i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))).ThenBy<MyGameInventoryItem, string>((Func<MyGameInventoryItem, string>) (i => i.ItemDefinition.Name)).ToList<MyGameInventoryItem>();
      }
      else if (source1.Count > 0 && this.m_showOnlyDuplicatesEnabled)
      {
        HashSet<string> stringSet = new HashSet<string>();
        List<MyGameInventoryItem> source2 = new List<MyGameInventoryItem>();
        foreach (MyGameInventoryItem gameInventoryItem in source1)
        {
          if (!gameInventoryItem.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))
          {
            if (!stringSet.Contains(gameInventoryItem.ItemDefinition.AssetModifierId))
              stringSet.Add(gameInventoryItem.ItemDefinition.AssetModifierId);
            else
              source2.Add(gameInventoryItem);
          }
        }
        list = source2.OrderByDescending<MyGameInventoryItem, bool>((Func<MyGameInventoryItem, bool>) (i => i.IsNew)).ThenByDescending<MyGameInventoryItem, bool>((Func<MyGameInventoryItem, bool>) (i => i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))).ThenBy<MyGameInventoryItem, string>((Func<MyGameInventoryItem, string>) (i => i.ItemDefinition.Name)).ToList<MyGameInventoryItem>();
      }
      else
      {
        List<MyGameInventoryItem> source2 = new List<MyGameInventoryItem>();
        source2.AddRange(source1.Where<MyGameInventoryItem>((Func<MyGameInventoryItem, bool>) (i => !i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))));
        list = source2.OrderByDescending<MyGameInventoryItem, bool>((Func<MyGameInventoryItem, bool>) (i => i.IsNew)).ThenByDescending<MyGameInventoryItem, bool>((Func<MyGameInventoryItem, bool>) (i => i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))).ThenBy<MyGameInventoryItem, string>((Func<MyGameInventoryItem, string>) (i => i.ItemDefinition.Name)).ToList<MyGameInventoryItem>();
      }
      return list;
    }

    private void hiddenButton_ButtonClicked(MyGuiControlImageButton obj)
    {
      if (!(obj.UserData is MyGuiControlLayoutGrid userData) || !(userData.GetAllControls().FirstOrDefault<MyGuiControlBase>((Func<MyGuiControlBase, bool>) (c => c.Name.StartsWith(this.m_equipCheckbox))) is MyGuiControlCheckbox guiControlCheckbox))
        return;
      guiControlCheckbox.IsChecked = !guiControlCheckbox.IsChecked;
    }

    private void hiddenButton_ButtonRightClicked(MyGuiControlImageButton obj)
    {
      this.m_contextMenuLastButton = obj;
      MyGuiControlListbox.Item obj1 = this.m_contextMenu.Items.FirstOrDefault<MyGuiControlListbox.Item>((Func<MyGuiControlListbox.Item, bool>) (i => i.UserData != null && (MyGuiScreenLoadInventory.InventoryItemAction) i.UserData == MyGuiScreenLoadInventory.InventoryItemAction.Recycle));
      if (obj1 != null)
      {
        MyGameInventoryItem currentItem = this.GetCurrentItem();
        if (currentItem != null)
        {
          string str = string.Format(MyTexts.Get(MyCommonTexts.ScreenLoadInventoryRecycleItem).ToString(), (object) MyGameService.GetRecyclingReward(currentItem.ItemDefinition.ItemQuality));
          obj1.Text = new StringBuilder(str);
        }
      }
      this.m_contextMenu.Activate();
      this.FocusedControl = this.m_contextMenu.GetInnerList();
      obj.BlockAutofocusOnHandlingOnce = true;
    }

    private void OnCategoryClicked(MyGuiControlImageButton obj)
    {
      if (obj.UserData == null)
        return;
      MyGameInventoryItemSlot userData = (MyGameInventoryItemSlot) obj.UserData;
      if (userData == this.m_filteredSlot)
      {
        if (this.m_activeTabState == MyGuiScreenLoadInventory.TabState.Character)
          this.m_filteredSlot = MyGameInventoryItemSlot.None;
      }
      else
        this.m_filteredSlot = userData;
      this.m_selectedTool = string.Empty;
      this.RecreateControls(false);
      this.EquipTool();
      this.UpdateCheckboxes();
    }

    private void EquipTool()
    {
      if (this.m_filteredSlot != MyGameInventoryItemSlot.None && this.m_activeTabState == MyGuiScreenLoadInventory.TabState.Tools)
      {
        long key = this.m_toolPicker.GetSelectedKey();
        MyPhysicalInventoryItem physicalInventoryItem = this.m_allTools.FirstOrDefault<MyPhysicalInventoryItem>((Func<MyPhysicalInventoryItem, bool>) (t => (long) t.ItemId == key));
        if (physicalInventoryItem.Content != null)
          this.m_entityController.ActivateCharacterToolbarItem(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), physicalInventoryItem.Content.SubtypeName));
        foreach (MyGameInventoryItem userItem in this.m_userItems)
        {
          if (userItem.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId) && userItem.ItemDefinition.ItemSlot == this.m_filteredSlot)
          {
            this.m_itemCheckActive = true;
            MyGameService.GetItemCheckData(userItem, (Action<byte[]>) null);
            break;
          }
        }
      }
      else
        this.m_entityController.ActivateCharacterToolbarItem(new MyDefinitionId());
    }

    private void OnItemCheckChanged(MyGuiControlCheckbox sender)
    {
      if (sender == null || !(sender.UserData is MyGameInventoryItem userData))
        return;
      if (sender.IsChecked)
      {
        this.m_itemCheckActive = true;
        MyGameService.GetItemCheckData(userData, (Action<byte[]>) null);
      }
      else
      {
        this.m_itemCheckActive = false;
        userData.UsingCharacters.Remove(MySession.Static.LocalCharacter.EntityId);
        MyCharacter localCharacter = MySession.Static.LocalCharacter;
        if (localCharacter == null)
          return;
        if (localCharacter != null)
        {
          switch (userData.ItemDefinition.ItemSlot)
          {
            case MyGameInventoryItemSlot.Face:
            case MyGameInventoryItemSlot.Helmet:
            case MyGameInventoryItemSlot.Gloves:
            case MyGameInventoryItemSlot.Boots:
            case MyGameInventoryItemSlot.Suit:
              MyAssetModifierComponent component1;
              if (localCharacter.Components.TryGet<MyAssetModifierComponent>(out component1))
              {
                component1.ResetSlot(userData.ItemDefinition.ItemSlot);
                break;
              }
              break;
            case MyGameInventoryItemSlot.Rifle:
            case MyGameInventoryItemSlot.Welder:
            case MyGameInventoryItemSlot.Grinder:
            case MyGameInventoryItemSlot.Drill:
              MyAssetModifierComponent component2;
              if (localCharacter.CurrentWeapon is MyEntity currentWeapon && currentWeapon.Components.TryGet<MyAssetModifierComponent>(out component2))
              {
                component2.ResetSlot(userData.ItemDefinition.ItemSlot);
                break;
              }
              break;
          }
        }
        this.UpdateOKButton();
      }
    }

    private void MyGameService_CheckItemDataReady(object sender, MyGameItemsEventArgs itemArgs)
    {
      if (itemArgs.NewItems == null || itemArgs.NewItems.Count == 0)
        return;
      MyGameInventoryItem newItem = itemArgs.NewItems[0];
      this.UseItem(newItem, itemArgs.CheckData);
      foreach (MyGameInventoryItem gameInventoryItem in new List<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>) MyGameService.InventoryItems))
      {
        if (gameInventoryItem != null && (long) gameInventoryItem.ID != (long) newItem.ID && gameInventoryItem.ItemDefinition.ItemSlot == newItem.ItemDefinition.ItemSlot)
          gameInventoryItem.UsingCharacters.Remove(MySession.Static.LocalCharacter.EntityId);
      }
      this.UpdateCheckboxes();
      this.UpdateOKButton();
    }

    private void UpdateOKButton()
    {
      bool flag = true;
      foreach (MyGameInventoryItem userItem in this.m_userItems)
      {
        if (userItem.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))
          flag &= !userItem.IsStoreFakeItem;
      }
      this.m_OkButton.Enabled = flag;
    }

    private void UpdateCheckboxes()
    {
      if (!MySession.Static.LocalCharacter.Components.TryGet<MyAssetModifierComponent>(out MyAssetModifierComponent _))
        return;
      foreach (MyGuiControlCheckbox itemCheckbox in this.m_itemCheckboxes)
      {
        if (itemCheckbox.UserData is MyGameInventoryItem userData)
        {
          itemCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) null;
          itemCheckbox.IsChecked = userData.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId);
          itemCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.OnItemCheckChanged);
        }
      }
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenLoadInventory);

    private void OpenCurrentItemInStore()
    {
      MyGameInventoryItem currentItem = this.GetCurrentItem();
      if (currentItem == null)
        return;
      MyGuiSandbox.OpenUrlWithFallback(string.Format(MySteamConstants.URL_INVENTORY_BUY_ITEM_FORMAT, (object) MyGameService.AppId, (object) currentItem.ItemDefinition.ID), "Buy Item");
    }

    private void OpenUserInventory() => MyGuiSandbox.OpenUrlWithFallback(string.Format(MySteamConstants.URL_INVENTORY, (object) MyGameService.UserId, (object) MyGameService.AppId), "User Inventory");

    private void OnOpenStore(MyGuiControlButton obj) => MyGuiSandbox.OpenUrlWithFallback(string.Format(MySteamConstants.URL_INVENTORY_BROWSE_ALL_ITEMS, (object) MyGameService.AppId), "Store");

    private MyGameInventoryItem GetCurrentItem()
    {
      if (this.m_contextMenuLastButton == null)
        return (MyGameInventoryItem) null;
      if (!(this.m_contextMenuLastButton.UserData is MyGuiControlLayoutGrid userData))
        return (MyGameInventoryItem) null;
      return !(userData.GetAllControls().FirstOrDefault<MyGuiControlBase>((Func<MyGuiControlBase, bool>) (c => c.Name.StartsWith(this.m_equipCheckbox))) is MyGuiControlCheckbox guiControlCheckbox) ? (MyGameInventoryItem) null : guiControlCheckbox.UserData as MyGameInventoryItem;
    }

    private void RecycleItemRequest()
    {
      if (this.GetCurrentItem() == null)
        return;
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.ScreenLoadInventoryRecycleItemMessageTitle);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.ScreenLoadInventoryRecycleItemMessageText), messageCaption: messageCaption, callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnRecycleItem)));
    }

    private void OnRecycleItem(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        MyGameInventoryItem currentItem = this.GetCurrentItem();
        if (currentItem != null && MyGameService.RecycleItem(currentItem))
          this.RemoveItemFromUI(currentItem);
      }
      this.State = MyGuiScreenState.OPENING;
    }

    private void DeleteItemRequest()
    {
      if (this.GetCurrentItem() == null)
        return;
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.ScreenLoadInventoryDeleteItemMessageTitle);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MyCommonTexts.ScreenLoadInventoryDeleteItemMessageText), messageCaption: messageCaption, callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.DeleteItemRequestMessageHandler)));
    }

    private void DeleteItemRequestMessageHandler(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        MyGameInventoryItem currentItem = this.GetCurrentItem();
        if (currentItem != null)
        {
          MyGameService.ConsumeItem(currentItem);
          this.RemoveItemFromUI(currentItem);
        }
      }
      this.State = MyGuiScreenState.OPENING;
    }

    private void RemoveItemFromUI(MyGameInventoryItem item)
    {
      if (this.m_contextMenuLastButton.UserData is MyGuiControlLayoutGrid userData)
      {
        foreach (MyGuiControlBase allControl in userData.GetAllControls())
        {
          allControl.Visible = false;
          allControl.Enabled = false;
        }
      }
      this.m_contextMenuLastButton = (MyGuiControlImageButton) null;
      MyAssetModifierComponent component;
      if (MySession.Static.LocalCharacter != null && item.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId) && MySession.Static.LocalCharacter.Components.TryGet<MyAssetModifierComponent>(out component))
        component.ResetSlot(item.ItemDefinition.ItemSlot);
      MyLocalCache.SaveInventoryConfig(MySession.Static.LocalCharacter);
    }

    private void OnRefreshClick(MyGuiControlButton obj)
    {
      if (MyPlatformGameSettings.LIMITED_MAIN_MENU)
        return;
      this.m_refreshButton.Enabled = false;
      this.RotatingWheelShow();
      this.RefreshItems();
    }

    private void UseItem(MyGameInventoryItem item, byte[] checkData)
    {
      if (MySession.Static.LocalCharacter == null)
        return;
      MyCharacter localCharacter = MySession.Static.LocalCharacter;
      item.IsNew = false;
      string assetModifierId = item.ItemDefinition.AssetModifierId;
      this.m_colorOrModelChanged = true;
      switch (item.ItemDefinition.ItemSlot)
      {
        case MyGameInventoryItemSlot.Face:
        case MyGameInventoryItemSlot.Helmet:
        case MyGameInventoryItemSlot.Gloves:
        case MyGameInventoryItemSlot.Boots:
        case MyGameInventoryItemSlot.Suit:
          MyAssetModifierComponent component1;
          if (!localCharacter.Components.TryGet<MyAssetModifierComponent>(out component1) || (MyFakes.OWN_ALL_ITEMS ? (component1.TryAddAssetModifier(assetModifierId) ? 1 : 0) : (component1.TryAddAssetModifier(checkData) ? 1 : 0)) == 0)
            break;
          item.UsingCharacters.Add(MySession.Static.LocalCharacter.EntityId);
          this.m_entityController.PlayRandomCharacterAnimation();
          break;
        case MyGameInventoryItemSlot.Rifle:
        case MyGameInventoryItemSlot.Welder:
        case MyGameInventoryItemSlot.Grinder:
        case MyGameInventoryItemSlot.Drill:
          MyAssetModifierComponent component2;
          if (!(localCharacter.CurrentWeapon is MyEntity currentWeapon) || !currentWeapon.Components.TryGet<MyAssetModifierComponent>(out component2) || (MyFakes.OWN_ALL_ITEMS ? (component2.TryAddAssetModifier(assetModifierId) ? 1 : 0) : (component2.TryAddAssetModifier(checkData) ? 1 : 0)) == 0)
            break;
          item.UsingCharacters.Add(MySession.Static.LocalCharacter.EntityId);
          break;
      }
    }

    public override bool Update(bool hasFocus)
    {
      if (this.State != MyGuiScreenState.CLOSING && this.State != MyGuiScreenState.CLOSED)
        MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFixed, (IMyEntity) null, new Vector3D?());
      if (MyInput.Static.IsNewPrimaryButtonPressed() && this.m_contextMenu.IsActiveControl && !this.m_contextMenu.IsMouseOver)
        this.m_contextMenu.Deactivate();
      if (this.m_focusButton & hasFocus)
      {
        this.m_focusButton = false;
        this.FocusButton((MyGuiScreenBase) null);
      }
      base.Update(hasFocus);
      if (!this.m_audioSet && MySandboxGame.IsGameReady && (MyAudio.Static != null && MyRenderProxy.VisibleObjectsRead != null) && MyRenderProxy.VisibleObjectsRead.Count > 0)
      {
        MyGuiScreenLoadInventory.SetAudioVolumes();
        this.m_audioSet = true;
      }
      if (this.m_OkButton != null)
        this.m_OkButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      if (this.m_craftButton != null)
        this.m_craftButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_refreshButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      return true;
    }

    private static void SetAudioVolumes()
    {
      MyAudio.Static.StopMusic();
      MyAudio.Static.ChangeGlobalVolume(1f, 5f);
      if (MyPerGameSettings.UseMusicController && MyFakes.ENABLE_MUSIC_CONTROLLER && (MySandboxGame.Config.EnableDynamicMusic && !Sandbox.Engine.Platform.Game.IsDedicated) && MyMusicController.Static == null)
        MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
      MyAudio.Static.MusicAllowed = MyMusicController.Static == null;
      if (MyMusicController.Static != null)
        MyMusicController.Static.Active = true;
      else
        MyAudio.Static.PlayMusic();
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (this.m_entityController != null)
      {
        this.m_entityController.Update(this.IsMouseOver() || this.m_lastHandlingControl != null);
        if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.VIEW) && this.m_entityController.GetEntity() is IMyControllableEntity entity)
          entity.SwitchHelmet();
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.PAGE_DOWN))
      {
        MyGuiControlBase myGuiControlBase = this.FocusedControl;
        bool flag;
        for (flag = myGuiControlBase == this.m_itemsTableParent; !flag && myGuiControlBase != null; myGuiControlBase = myGuiControlBase.Owner as MyGuiControlBase)
        {
          if (myGuiControlBase.Owner == this.m_itemsTableParent)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          switch (this.m_activeLowTabState)
          {
            case MyGuiScreenLoadInventory.LowerTabState.Coloring:
              this.FocusedControl = (MyGuiControlBase) this.m_modelPicker;
              break;
            case MyGuiScreenLoadInventory.LowerTabState.Recycling:
              this.FocusedControl = (MyGuiControlBase) this.m_duplicateCheckboxRecycle;
              break;
          }
        }
      }
      if (receivedFocusInThisUpdate)
        return;
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
      {
        if (this.m_activeLowTabState == MyGuiScreenLoadInventory.LowerTabState.Coloring)
          this.OnOkClick((MyGuiControlButton) null);
        else
          this.OnCraftClick((MyGuiControlButton) null);
      }
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        this.OnRefreshClick((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
        this.SwitchCategory();
      if (!MyPlatformGameSettings.LIMITED_MAIN_MENU && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
        this.SwitchAction();
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT) && !MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
        return;
      this.SwitchMainTab();
    }

    public override bool Draw()
    {
      int num = base.Draw() ? 1 : 0;
      this.DrawScene();
      if (!MyInput.Static.IsJoystickLastUsed)
        return num != 0;
      MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
      Vector2 vector2 = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.ToolbarButton).SizeOverride.Value;
      Vector2 positionAbsoluteTopLeft;
      Vector2 normalizedCoord1 = positionAbsoluteTopLeft = this.m_characterButton.GetPositionAbsoluteTopLeft();
      normalizedCoord1.Y += vector2.Y / 2f;
      normalizedCoord1.X -= vector2.X / 6f;
      Vector2 normalizedCoord2 = positionAbsoluteTopLeft;
      normalizedCoord2.Y = normalizedCoord1.Y;
      Color color = MyGuiControlBase.ApplyColorMaskModifiers(MyGuiConstants.LABEL_TEXT_COLOR, true, this.m_transitionAlpha);
      normalizedCoord2.X += (float) (2.0 * (double) vector2.X + (double) vector2.X / 8.0);
      MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Left), normalizedCoord1, 1f, new Color?(color), drawAlign);
      MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Right), normalizedCoord2, 1f, new Color?(color), drawAlign);
      return num != 0;
    }

    private void DrawScene()
    {
      if (MySession.Static == null)
        return;
      if ((MySession.Static.CameraController == null || !MySession.Static.CameraController.IsInFirstPersonView) && MyThirdPersonSpectator.Static != null)
        MyThirdPersonSpectator.Static.Update();
      if (MySector.MainCamera != null)
      {
        MySession.Static.CameraController.ControlCamera(MySector.MainCamera);
        MySector.MainCamera.Update(0.01666667f);
        MySector.MainCamera.UploadViewMatrixToRender();
      }
      MySector.UpdateSunLight();
      MyRenderProxy.UpdateGameplayFrame(MySession.Static.GameplayFrameCounter);
      MyRenderFogSettings settings1 = new MyRenderFogSettings()
      {
        FogMultiplier = MySector.FogProperties.FogMultiplier,
        FogColor = (Color) MySector.FogProperties.FogColor,
        FogDensity = MySector.FogProperties.FogDensity / 100f,
        FogSkybox = MySector.FogProperties.FogSkybox,
        FogAtmo = MySector.FogProperties.FogAtmo
      };
      MyRenderProxy.UpdateFogSettings(ref settings1);
      MyRenderPlanetSettings settings2 = new MyRenderPlanetSettings()
      {
        AtmosphereIntensityMultiplier = MySector.PlanetProperties.AtmosphereIntensityMultiplier,
        AtmosphereIntensityAmbientMultiplier = MySector.PlanetProperties.AtmosphereIntensityAmbientMultiplier,
        AtmosphereDesaturationFactorForward = MySector.PlanetProperties.AtmosphereDesaturationFactorForward,
        CloudsIntensityMultiplier = MySector.PlanetProperties.CloudsIntensityMultiplier
      };
      MyRenderProxy.UpdatePlanetSettings(ref settings2);
      MyRenderProxy.UpdateSSAOSettings(ref MySector.SSAOSettings);
      MyRenderProxy.UpdateHBAOSettings(ref MySector.HBAOSettings);
      MyEnvironmentData environmentData = MySector.SunProperties.EnvironmentData;
      environmentData.Skybox = !string.IsNullOrEmpty(MySession.Static.CustomSkybox) ? MySession.Static.CustomSkybox : MySector.EnvironmentDefinition.EnvironmentTexture;
      environmentData.SkyboxOrientation = MySector.EnvironmentDefinition.EnvironmentOrientation.ToQuaternion();
      environmentData.EnvironmentLight.SunLightDirection = -MySector.SunProperties.SunDirectionNormalized;
      MyRenderProxy.UpdateRenderEnvironment(ref environmentData, MySector.ResetEyeAdaptation);
      MySector.ResetEyeAdaptation = false;
      MyRenderProxy.UpdateEnvironmentMap();
      if (MyVideoSettingsManager.CurrentGraphicsSettings.PostProcessingEnabled != MyPostprocessSettingsWrapper.AllEnabled || MyPostprocessSettingsWrapper.IsDirty)
      {
        if (MyVideoSettingsManager.CurrentGraphicsSettings.PostProcessingEnabled)
          MyPostprocessSettingsWrapper.SetWardrobePostProcessing();
        else
          MyPostprocessSettingsWrapper.ReducePostProcessing();
      }
      MyRenderProxy.SwitchPostprocessSettings(ref MyPostprocessSettingsWrapper.Settings);
      if (MyRenderProxy.SettingsDirty)
        MyRenderProxy.SwitchRenderSettings(MyRenderProxy.Settings);
      MyRenderProxy.Draw3DScene();
      using (Stats.Generic.Measure("GamePrepareDraw"))
      {
        if (MySession.Static == null)
          return;
        MySession.Static.Draw();
      }
    }

    protected override void Canceling()
    {
      MyGuiScreenLoadInventory.Cancel();
      base.Canceling();
    }

    protected override void OnHide()
    {
      base.OnHide();
      this.DrawScene();
    }

    protected override void OnClosed()
    {
      if (MyGameService.IsActive)
        MyGameService.InventoryRefreshed -= new EventHandler(this.MySteamInventory_Refreshed);
      this.m_sliderHue.ValueChanged -= new Action<MyGuiControlSlider>(this.OnValueChange);
      this.m_sliderSaturation.ValueChanged -= new Action<MyGuiControlSlider>(this.OnValueChange);
      this.m_sliderValue.ValueChanged -= new Action<MyGuiControlSlider>(this.OnValueChange);
      MyGameService.CheckItemDataReady -= new EventHandler<MyGameItemsEventArgs>(this.MyGameService_CheckItemDataReady);
      base.OnClosed();
      if (!this.m_inGame)
        MyVRage.Platform.Ansel.IsSessionEnabled = false;
      else if (this.m_savedStateAnselEnabled.HasValue)
      {
        MyVRage.Platform.Ansel.IsSessionEnabled = this.m_savedStateAnselEnabled.Value;
        this.m_savedStateAnselEnabled = new bool?();
      }
      MyScreenManager.GetFirstScreenOfType<MyGuiScreenGamePlay>()?.UnhideScreen();
    }

    protected override void OnShow()
    {
      this.m_savedStateAnselEnabled = new bool?(MyVRage.Platform.Ansel.IsSessionEnabled);
      MyVRage.Platform.Ansel.IsSessionEnabled = false;
      if (MySector.MainCamera != null && !this.m_inGame)
        MySector.MainCamera.FieldOfViewDegrees = 55f;
      if (MyGameService.IsActive)
        MyGameService.InventoryRefreshed += new EventHandler(this.MySteamInventory_Refreshed);
      base.OnShow();
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      MyScreenManager.GetFirstScreenOfType<MyGuiScreenIntroVideo>()?.UnhideScreen();
      return base.CloseScreen(isUnloading);
    }

    private void RefreshItems()
    {
      if (!MyGameService.IsActive)
        return;
      MyGameService.GetAllInventoryItems();
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
      MyGuiControlButton guiControlButton = new MyGuiControlButton(position1, size: size, colorMask: colorMask, toolTip: toolTip1, text: text1, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      return guiControlButton;
    }

    private MyGuiControlImageButton MakeImageButton(
      Vector2 position,
      Vector2 size,
      MyGuiControlImageButton.StyleDefinition style,
      MyStringId toolTip,
      Action<MyGuiControlImageButton> onClick)
    {
      MyGuiControlImageButton controlImageButton = new MyGuiControlImageButton(position: new Vector2?(position), toolTip: MyTexts.GetString(toolTip), onButtonClick: onClick);
      if (style != null)
        controlImageButton.ApplyStyle(style);
      controlImageButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlImageButton.Size = size;
      return controlImageButton;
    }

    private void MySteamInventory_Refreshed(object sender, System.EventArgs e)
    {
      if (this.m_itemCheckActive)
        this.m_itemCheckActive = false;
      else
        this.RefreshUI();
    }

    private void RefreshUI()
    {
      this.RecreateControls(false);
      this.EquipTool();
      this.UpdateCheckboxes();
    }

    private void RotatingWheelShow()
    {
      this.m_wheel.ManualRotationUpdate = true;
      this.m_wheel.Visible = true;
    }

    private void RotatingWheelHide()
    {
      this.m_wheel.ManualRotationUpdate = false;
      this.m_wheel.Visible = false;
    }

    private void SwitchCategory()
    {
      ++this.m_currentCategoryIndex;
      if (this.m_currentCategoryIndex >= this.m_categoryButtonsData.Count)
        this.m_currentCategoryIndex = 0;
      List<MyGuiControlBase> controls = this.m_categoryButtonLayout.GetControls();
      if (this.m_currentCategoryIndex < controls.Count)
        this.OnCategoryClicked(controls[this.m_currentCategoryIndex] as MyGuiControlImageButton);
      this.FocusButton((MyGuiScreenBase) null);
    }

    private void SwitchMainTab()
    {
      if (this.m_activeTabState == MyGuiScreenLoadInventory.TabState.Character)
        this.OnViewTools((MyGuiControlButton) null);
      else
        this.OnViewTabCharacter((MyGuiControlButton) null);
      this.FocusButton((MyGuiScreenBase) null);
    }

    private void SwitchAction()
    {
      if (this.m_activeLowTabState == MyGuiScreenLoadInventory.LowerTabState.Coloring)
        this.OnViewTabRecycling((MyGuiControlButton) null);
      else
        this.OnViewTabColoring((MyGuiControlButton) null);
    }

    private enum InventoryItemAction
    {
      Apply,
      Sell,
      Trade,
      Recycle,
      Delete,
      Buy,
    }

    private enum TabState
    {
      Character,
      Tools,
    }

    private enum LowerTabState
    {
      Coloring,
      Recycling,
    }

    private struct CategoryButton
    {
      public MyStringId Tooltip;
      public MyGameInventoryItemSlot Slot;
      public string ImageName;
      public string ButtonText;

      public CategoryButton(
        MyStringId tooltip,
        MyGameInventoryItemSlot slot,
        string imageName = null,
        string buttonText = null)
      {
        this.Tooltip = tooltip;
        this.Slot = slot;
        this.ImageName = imageName;
        this.ButtonText = buttonText;
      }
    }
  }
}
