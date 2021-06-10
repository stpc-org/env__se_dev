// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalProductionController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyTerminalProductionController : MyTerminalController
  {
    public static readonly int BLUEPRINT_GRID_ROWS = 7;
    public static readonly int QUEUE_GRID_ROWS = 2;
    public static readonly int INVENTORY_GRID_ROWS = 3;
    private static readonly Vector4 ERROR_ICON_COLOR_MASK = new Vector4(1f, 0.5f, 0.5f, 1f);
    private static readonly Vector4 COLOR_MASK_WHITE = Color.White.ToVector4();
    private static readonly MyTimeSpan TIME_EPSILON = MyTimeSpan.FromMilliseconds(35.0);
    private static StringBuilder m_textCache = new StringBuilder();
    private static Dictionary<MyDefinitionId, MyFixedPoint> m_requiredCountCache = new Dictionary<MyDefinitionId, MyFixedPoint>((IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);
    private static List<MyBlueprintDefinitionBase.ProductionInfo> m_blueprintCache = new List<MyBlueprintDefinitionBase.ProductionInfo>();
    private IMyGuiControlsParent m_controlsParent;
    private MyGridTerminalSystem m_terminalSystem;
    private Dictionary<int, MyAssembler> m_assemblersByKey = new Dictionary<int, MyAssembler>();
    private int m_assemblerKeyCounter;
    private MyTimeSpan m_productionStartTime;
    private MyGuiControlSearchBox m_blueprintsSearchBox;
    private MyGuiControlCombobox m_comboboxAssemblers;
    private MyGuiControlGrid m_blueprintsGrid;
    private MyAssembler m_selectedAssembler;
    private MyGuiControlRadioButtonGroup m_blueprintButtonGroup = new MyGuiControlRadioButtonGroup();
    private MyGuiControlRadioButtonGroup m_modeButtonGroup = new MyGuiControlRadioButtonGroup();
    private MyGuiControlGrid m_queueGrid;
    private MyGuiControlGrid m_inventoryGrid;
    private MyGuiControlComponentList m_materialsList;
    private MyGuiControlScrollablePanel m_blueprintsArea;
    private MyGuiControlScrollablePanel m_queueArea;
    private MyGuiControlScrollablePanel m_inventoryArea;
    private MyGuiControlBase m_blueprintsBgPanel;
    private MyGuiControlBase m_blueprintsLabel;
    private MyGuiControlCheckbox m_repeatCheckbox;
    private MyGuiControlCheckbox m_slaveCheckbox;
    private MyGuiControlButton m_disassembleAllButton;
    private MyGuiControlButton m_controlPanelButton;
    private MyGuiControlButton m_inventoryButton;
    private MyGuiControlLabel m_materialsLabel;
    private MyDragAndDropInfo m_dragAndDropInfo;
    private MyGuiControlGridDragAndDrop m_dragAndDrop;
    private StringBuilder m_incompleteAssemblerName = new StringBuilder();
    private MyGuiControlRadioButton m_assemblingButton;
    private MyGuiControlRadioButton m_disassemblingButton;
    private int m_queueRemoveStackTimeMs;
    private int m_queueRemoveStackIndex;
    private HashSet<MyDefinitionId> m_blueprintUnbuildabilityCache = new HashSet<MyDefinitionId>();
    private bool m_isAnimationNeeded;
    private bool m_hadItemsInQueueLastUpdate;
    private const int QUEUE_REMOVE_STACK_WAIT_MS = 600;

    private MyAssembler SelectedAssembler
    {
      get => this.m_selectedAssembler;
      set
      {
        if (this.m_selectedAssembler == value)
          return;
        this.m_selectedAssembler = value;
      }
    }

    private MyTerminalProductionController.AssemblerMode CurrentAssemblerMode => (MyTerminalProductionController.AssemblerMode) this.m_modeButtonGroup.SelectedButton.Key;

    public void Init(
      IMyGuiControlsParent controlsParent,
      MyCubeGrid grid,
      MyCubeBlock currentBlock)
    {
      if (grid == null)
      {
        MyTerminalProductionController.ShowError(MySpaceTexts.ScreenTerminalError_ShipNotConnected, controlsParent);
      }
      else
      {
        grid.OnTerminalOpened();
        this.m_assemblerKeyCounter = 0;
        this.m_assemblersByKey.Clear();
        foreach (MyTerminalBlock block in grid.GridSystems.TerminalSystem.Blocks)
        {
          if (block is MyAssembler myAssembler && myAssembler.HasLocalPlayerAccess())
            this.m_assemblersByKey.Add(this.m_assemblerKeyCounter++, myAssembler);
        }
        this.m_controlsParent = controlsParent;
        this.m_terminalSystem = grid.GridSystems.TerminalSystem;
        this.m_blueprintsArea = (MyGuiControlScrollablePanel) controlsParent.Controls.GetControlByName("BlueprintsScrollableArea");
        this.m_blueprintsSearchBox = (MyGuiControlSearchBox) controlsParent.Controls.GetControlByName("BlueprintsSearchBox");
        this.m_queueArea = (MyGuiControlScrollablePanel) controlsParent.Controls.GetControlByName("QueueScrollableArea");
        this.m_inventoryArea = (MyGuiControlScrollablePanel) controlsParent.Controls.GetControlByName("InventoryScrollableArea");
        this.m_blueprintsBgPanel = controlsParent.Controls.GetControlByName("BlueprintsBackgroundPanel");
        this.m_blueprintsLabel = controlsParent.Controls.GetControlByName("BlueprintsLabel");
        this.m_comboboxAssemblers = (MyGuiControlCombobox) controlsParent.Controls.GetControlByName("AssemblersCombobox");
        this.m_blueprintsGrid = (MyGuiControlGrid) this.m_blueprintsArea.ScrolledControl;
        this.m_queueGrid = (MyGuiControlGrid) this.m_queueArea.ScrolledControl;
        this.m_inventoryGrid = (MyGuiControlGrid) this.m_inventoryArea.ScrolledControl;
        this.m_materialsList = (MyGuiControlComponentList) controlsParent.Controls.GetControlByName("MaterialsList");
        this.m_repeatCheckbox = (MyGuiControlCheckbox) controlsParent.Controls.GetControlByName("RepeatCheckbox");
        this.m_slaveCheckbox = (MyGuiControlCheckbox) controlsParent.Controls.GetControlByName("SlaveCheckbox");
        this.m_disassembleAllButton = (MyGuiControlButton) controlsParent.Controls.GetControlByName("DisassembleAllButton");
        this.m_controlPanelButton = (MyGuiControlButton) controlsParent.Controls.GetControlByName("ControlPanelButton");
        this.m_inventoryButton = (MyGuiControlButton) controlsParent.Controls.GetControlByName("InventoryButton");
        this.m_materialsLabel = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("RequiredLabel");
        this.m_controlPanelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ProductionScreen_TerminalControlScreen));
        this.m_inventoryButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ProductionScreen_TerminalInventoryScreen));
        this.m_assemblingButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("AssemblingButton");
        this.m_assemblingButton.VisualStyle = MyGuiControlRadioButtonStyleEnum.TerminalAssembler;
        this.m_assemblingButton.Key = 0;
        this.m_disassemblingButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("DisassemblingButton");
        this.m_disassemblingButton.VisualStyle = MyGuiControlRadioButtonStyleEnum.TerminalAssembler;
        this.m_disassemblingButton.Key = 1;
        this.m_modeButtonGroup.Add(this.m_assemblingButton);
        this.m_modeButtonGroup.Add(this.m_disassemblingButton);
        foreach (KeyValuePair<int, MyAssembler> keyValuePair in (IEnumerable<KeyValuePair<int, MyAssembler>>) this.m_assemblersByKey.OrderBy<KeyValuePair<int, MyAssembler>, int>((Func<KeyValuePair<int, MyAssembler>, int>) (x =>
        {
          MyAssembler myAssembler = x.Value;
          return myAssembler == currentBlock ? -1 : (myAssembler.IsFunctional ? 0 : 10000) + myAssembler.GUIPriority;
        })))
        {
          MyAssembler myAssembler = keyValuePair.Value;
          if (!myAssembler.IsFunctional)
          {
            this.m_incompleteAssemblerName.Clear();
            this.m_incompleteAssemblerName.AppendStringBuilder(myAssembler.CustomName);
            this.m_incompleteAssemblerName.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockIncomplete));
            this.m_comboboxAssemblers.AddItem((long) keyValuePair.Key, this.m_incompleteAssemblerName);
          }
          else
            this.m_comboboxAssemblers.AddItem((long) keyValuePair.Key, myAssembler.CustomName);
        }
        this.m_comboboxAssemblers.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.Assemblers_ItemSelected);
        this.m_comboboxAssemblers.SetToolTip(MyTexts.GetString(MySpaceTexts.ProductionScreen_AssemblerList));
        this.m_comboboxAssemblers.SelectItemByIndex(0);
        this.m_dragAndDrop = new MyGuiControlGridDragAndDrop(MyGuiConstants.DRAG_AND_DROP_BACKGROUND_COLOR, MyGuiConstants.DRAG_AND_DROP_TEXT_COLOR, 0.7f, MyGuiConstants.DRAG_AND_DROP_TEXT_OFFSET, true);
        controlsParent.Controls.Add((MyGuiControlBase) this.m_dragAndDrop);
        this.m_dragAndDrop.DrawBackgroundTexture = false;
        this.m_dragAndDrop.ItemDropped += new OnItemDropped(this.dragDrop_OnItemDropped);
        this.m_blueprintsGrid.GamepadHelpTextId = MySpaceTexts.ToolTipTerminalProduction_AddToQueueGamepad;
        this.RefreshBlueprints();
        this.Assemblers_ItemSelected();
        this.RegisterEvents();
        if (this.m_assemblersByKey.Count == 0)
          MyTerminalProductionController.ShowError(MySpaceTexts.ScreenTerminalError_NoAssemblers, controlsParent);
        this.m_queueGrid.GamepadHelpTextId = MySpaceTexts.TerminalProduction_Help_QueueGrid;
      }
    }

    private void UpdateBlueprintClassGui()
    {
      foreach (MyGuiControlBase control in this.m_blueprintButtonGroup)
        this.m_controlsParent.Controls.Remove(control);
      this.m_blueprintButtonGroup.Clear();
      float xOffset = 0.0f;
      if (!(this.SelectedAssembler.BlockDefinition is MyProductionBlockDefinition))
        return;
      List<MyBlueprintClassDefinition> blueprintClasses = (this.SelectedAssembler.BlockDefinition as MyProductionBlockDefinition).BlueprintClasses;
      for (int index = 0; index < blueprintClasses.Count; ++index)
      {
        bool selected = index == 0 || blueprintClasses[index].Id.SubtypeName == "Components" || blueprintClasses[index].Id.SubtypeName == "BasicComponents";
        this.AddBlueprintClassButton(blueprintClasses[index], ref xOffset, selected);
      }
    }

    private void AddBlueprintClassButton(
      MyBlueprintClassDefinition classDef,
      ref float xOffset,
      bool selected = false)
    {
      if (classDef == null)
        return;
      MyGuiControlRadioButton controlRadioButton = new MyGuiControlRadioButton(new Vector2?(this.m_blueprintsLabel.Position + new Vector2(xOffset, this.m_blueprintsLabel.Size.Y + 0.012f)), new Vector2?(new Vector2(46f, 46f) / MyGuiConstants.GUI_OPTIMAL_SIZE));
      xOffset += controlRadioButton.Size.X;
      controlRadioButton.Icon = new MyGuiHighlightTexture?(new MyGuiHighlightTexture()
      {
        Normal = classDef.Icons[0],
        Highlight = classDef.HighlightIcon,
        Focus = classDef.FocusIcon,
        SizePx = new Vector2(46f, 46f)
      });
      controlRadioButton.UserData = (object) classDef;
      controlRadioButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      controlRadioButton.SetTooltip(classDef.DescriptionText);
      this.m_blueprintButtonGroup.Add(controlRadioButton);
      this.m_controlsParent.Controls.Add((MyGuiControlBase) controlRadioButton);
      controlRadioButton.Selected = selected;
    }

    private static void ShowError(MyStringId errorText, IMyGuiControlsParent controlsParent)
    {
      foreach (MyGuiControlBase control in controlsParent.Controls)
        control.Visible = false;
      MyGuiControlLabel myGuiControlLabel = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("ErrorMessage") ?? MyGuiScreenTerminal.CreateErrorLabel(errorText, "ErrorMessage");
      myGuiControlLabel.TextEnum = errorText;
      if (controlsParent.Controls.Contains((MyGuiControlBase) myGuiControlLabel))
        return;
      controlsParent.Controls.Add((MyGuiControlBase) myGuiControlLabel);
    }

    private static void HideError(IMyGuiControlsParent controlsParent)
    {
      controlsParent.Controls.RemoveControlByName("ErrorMessage");
      foreach (MyGuiControlBase control in controlsParent.Controls)
        control.Visible = true;
    }

    private void RegisterEvents()
    {
      foreach (KeyValuePair<int, MyAssembler> keyValuePair in this.m_assemblersByKey)
        keyValuePair.Value.CustomNameChanged += new Action<MyTerminalBlock>(this.assembler_CustomNameChanged);
      this.m_terminalSystem.BlockAdded += new Action<MyTerminalBlock>(this.TerminalSystem_BlockAdded);
      this.m_terminalSystem.BlockRemoved += new Action<MyTerminalBlock>(this.TerminalSystem_BlockRemoved);
      this.m_blueprintButtonGroup.SelectedChanged += new Action<MyGuiControlRadioButtonGroup>(this.blueprintButtonGroup_SelectedChanged);
      this.m_modeButtonGroup.SelectedChanged += new Action<MyGuiControlRadioButtonGroup>(this.modeButtonGroup_SelectedChanged);
      this.m_blueprintsSearchBox.OnTextChanged += new MyGuiControlSearchBox.TextChangedDelegate(this.OnSearchTextChanged);
      this.m_blueprintsGrid.ItemClicked += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.blueprintsGrid_ItemClicked);
      this.m_blueprintsGrid.MouseOverIndexChanged += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.blueprintsGrid_MouseOverIndexChanged);
      this.m_blueprintsGrid.ItemSelected += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.blueprintsGrid_FocusChanged);
      this.m_blueprintsGrid.ItemAccepted += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.blueprintsGrid_ItemClicked);
      this.m_inventoryGrid.ItemClicked += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.inventoryGrid_ItemClicked);
      this.m_inventoryGrid.MouseOverIndexChanged += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.inventoryGrid_MouseOverIndexChanged);
      this.m_inventoryGrid.ItemSelected += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.inventoryGrid_FocusChanged);
      this.m_inventoryGrid.ItemAccepted += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.inventoryGrid_ItemClicked);
      this.m_repeatCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.repeatCheckbox_IsCheckedChanged);
      this.m_slaveCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.slaveCheckbox_IsCheckedChanged);
      this.m_queueGrid.ItemClicked += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.queueGrid_ItemClicked);
      this.m_queueGrid.ItemDragged += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.queueGrid_ItemDragged);
      this.m_queueGrid.ItemAccepted += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.queueGrid_ItemClicked);
      this.m_queueGrid.MouseOverIndexChanged += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.queueGrid_MouseOverIndexChanged);
      this.m_queueGrid.ItemSelected += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.queueGrid_FocusChanged);
      this.m_queueGrid.ItemControllerAction += new Func<MyGuiControlGrid, int, MyGridItemAction, bool, bool>(this.queueGrid_ItemControllerAction);
      this.m_controlPanelButton.ButtonClicked += new Action<MyGuiControlButton>(this.controlPanelButton_ButtonClicked);
      this.m_inventoryButton.ButtonClicked += new Action<MyGuiControlButton>(this.inventoryButton_ButtonClicked);
      this.m_disassembleAllButton.ButtonClicked += new Action<MyGuiControlButton>(this.disassembleAllButton_ButtonClicked);
    }

    private bool queueGrid_ItemControllerAction(
      MyGuiControlGrid sender,
      int index,
      MyGridItemAction action,
      bool pressed)
    {
      if (action != MyGridItemAction.Button_A)
        return false;
      if (pressed)
      {
        this.m_queueRemoveStackIndex = index;
        this.m_queueRemoveStackTimeMs = MySandboxGame.TotalGamePlayTimeInMilliseconds + 600;
        return true;
      }
      this.m_queueRemoveStackTimeMs = -1;
      int num1 = MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED) ? 1 : 0;
      bool flag = MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED);
      int num2 = (num1 != 0 ? 10 : 1) * (flag ? 100 : 1);
      this.SelectedAssembler.RemoveQueueItemRequest(index, (MyFixedPoint) num2);
      return true;
    }

    private void UnregisterEvents()
    {
      if (this.m_controlsParent == null)
        return;
      foreach (KeyValuePair<int, MyAssembler> keyValuePair in this.m_assemblersByKey)
        keyValuePair.Value.CustomNameChanged -= new Action<MyTerminalBlock>(this.assembler_CustomNameChanged);
      if (this.m_terminalSystem != null)
      {
        this.m_terminalSystem.BlockAdded -= new Action<MyTerminalBlock>(this.TerminalSystem_BlockAdded);
        this.m_terminalSystem.BlockRemoved -= new Action<MyTerminalBlock>(this.TerminalSystem_BlockRemoved);
      }
      this.m_blueprintButtonGroup.SelectedChanged -= new Action<MyGuiControlRadioButtonGroup>(this.blueprintButtonGroup_SelectedChanged);
      this.m_modeButtonGroup.SelectedChanged -= new Action<MyGuiControlRadioButtonGroup>(this.modeButtonGroup_SelectedChanged);
      this.m_blueprintsSearchBox.OnTextChanged -= new MyGuiControlSearchBox.TextChangedDelegate(this.OnSearchTextChanged);
      this.m_blueprintsGrid.ItemClicked -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.blueprintsGrid_ItemClicked);
      this.m_blueprintsGrid.MouseOverIndexChanged -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.blueprintsGrid_MouseOverIndexChanged);
      this.m_blueprintsGrid.ItemSelected -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.blueprintsGrid_FocusChanged);
      this.m_blueprintsGrid.ItemAccepted -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.blueprintsGrid_ItemClicked);
      this.m_inventoryGrid.ItemClicked -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.inventoryGrid_ItemClicked);
      this.m_inventoryGrid.MouseOverIndexChanged -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.inventoryGrid_MouseOverIndexChanged);
      this.m_inventoryGrid.ItemSelected -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.inventoryGrid_FocusChanged);
      this.m_inventoryGrid.ItemAccepted -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.inventoryGrid_ItemClicked);
      this.m_repeatCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) null;
      this.m_slaveCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) null;
      this.m_queueGrid.ItemClicked -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.queueGrid_ItemClicked);
      this.m_queueGrid.ItemDragged -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.queueGrid_ItemDragged);
      this.m_queueGrid.MouseOverIndexChanged -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.queueGrid_MouseOverIndexChanged);
      this.m_queueGrid.ItemSelected -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.queueGrid_FocusChanged);
      this.m_queueGrid.ItemAccepted -= new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.queueGrid_ItemClicked);
      this.m_controlPanelButton.ButtonClicked -= new Action<MyGuiControlButton>(this.controlPanelButton_ButtonClicked);
      this.m_inventoryButton.ButtonClicked -= new Action<MyGuiControlButton>(this.inventoryButton_ButtonClicked);
      this.m_disassembleAllButton.ButtonClicked -= new Action<MyGuiControlButton>(this.disassembleAllButton_ButtonClicked);
    }

    private void RegisterAssemblerEvents(MyAssembler assembler)
    {
      if (assembler == null)
        return;
      assembler.CurrentModeChanged += new Action<MyAssembler>(this.assembler_CurrentModeChanged);
      assembler.QueueChanged += new Action<MyProductionBlock>(this.assembler_QueueChanged);
      assembler.CurrentProgressChanged += new Action<MyAssembler>(this.assembler_CurrentProgressChanged);
      assembler.CurrentStateChanged += new Action<MyAssembler>(this.assembler_CurrentStateChanged);
      assembler.InputInventory.ContentsChanged += new Action<MyInventoryBase>(this.InputInventory_ContentsChanged);
      assembler.OutputInventory.ContentsChanged += new Action<MyInventoryBase>(this.OutputInventory_ContentsChanged);
    }

    private void UnregisterAssemblerEvents(MyAssembler assembler)
    {
      if (assembler == null)
        return;
      this.SelectedAssembler.CurrentModeChanged -= new Action<MyAssembler>(this.assembler_CurrentModeChanged);
      this.SelectedAssembler.QueueChanged -= new Action<MyProductionBlock>(this.assembler_QueueChanged);
      this.SelectedAssembler.CurrentProgressChanged -= new Action<MyAssembler>(this.assembler_CurrentProgressChanged);
      this.SelectedAssembler.CurrentStateChanged -= new Action<MyAssembler>(this.assembler_CurrentStateChanged);
      if (assembler.InputInventory != null)
        assembler.InputInventory.ContentsChanged -= new Action<MyInventoryBase>(this.InputInventory_ContentsChanged);
      if (this.SelectedAssembler.OutputInventory == null)
        return;
      this.SelectedAssembler.OutputInventory.ContentsChanged -= new Action<MyInventoryBase>(this.OutputInventory_ContentsChanged);
    }

    internal void Close()
    {
      this.UnregisterEvents();
      this.UnregisterAssemblerEvents(this.SelectedAssembler);
      this.m_assemblersByKey.Clear();
      this.m_blueprintButtonGroup.Clear();
      this.m_modeButtonGroup.Clear();
      this.SelectedAssembler = (MyAssembler) null;
      this.m_controlsParent = (IMyGuiControlsParent) null;
      this.m_terminalSystem = (MyGridTerminalSystem) null;
      this.m_comboboxAssemblers = (MyGuiControlCombobox) null;
      this.m_dragAndDrop = (MyGuiControlGridDragAndDrop) null;
      this.m_dragAndDropInfo = (MyDragAndDropInfo) null;
    }

    private void SelectAndShowAssembler(MyAssembler assembler)
    {
      this.UnregisterAssemblerEvents(this.SelectedAssembler);
      this.SelectedAssembler = assembler;
      this.RegisterAssemblerEvents(assembler);
      this.RefreshRepeatMode(assembler.RepeatEnabled);
      this.RefreshSlaveMode(assembler.IsSlave);
      this.SelectModeButton(assembler);
      this.UpdateBlueprintClassGui();
      this.m_blueprintsSearchBox.SearchText = string.Empty;
      this.RefreshQueue(true);
      this.RefreshInventory();
      this.RefreshProgress();
      this.RefreshAssemblerModeView();
    }

    private void RefreshInventory()
    {
      int? selectedIndex = this.m_inventoryGrid.SelectedIndex;
      this.m_inventoryGrid.Clear();
      foreach (MyPhysicalInventoryItem physicalInventoryItem in this.SelectedAssembler.OutputInventory.GetItems())
        this.m_inventoryGrid.Add(MyGuiControlInventoryOwner.CreateInventoryGridItem(physicalInventoryItem));
      int count = this.SelectedAssembler.OutputInventory.GetItems().Count;
      this.m_inventoryGrid.RowsCount = Math.Max(1 + count / this.m_inventoryGrid.ColumnsCount, MyTerminalProductionController.INVENTORY_GRID_ROWS);
      if (!selectedIndex.HasValue)
        return;
      this.m_inventoryGrid.SelectedIndex = new int?(Math.Max(Math.Min(selectedIndex.Value, count - 1), 0));
    }

    private void RefreshQueue(bool resetScroll = false)
    {
      float num1 = this.m_queueArea.ScrollbarVPosition;
      int? selectedIndex = this.m_queueGrid.SelectedIndex;
      int num2 = 0;
      for (int index = 0; index < this.m_queueGrid.Items.Count && this.m_queueGrid.Items[index] != null; ++index)
        ++num2;
      this.m_queueGrid.Clear();
      int num3 = 0;
      foreach (MyProductionBlock.QueueItem queueItem in this.SelectedAssembler.Queue)
      {
        MyTerminalProductionController.m_textCache.Clear().Append((int) queueItem.Amount).Append('x');
        MyGuiGridItem myGuiGridItem = new MyGuiGridItem(queueItem.Blueprint.Icons, (string) null, queueItem.Blueprint.DisplayNameText, (object) queueItem, true, 1f);
        myGuiGridItem.AddText(MyTerminalProductionController.m_textCache, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
        if (MyFakes.SHOW_PRODUCTION_QUEUE_ITEM_IDS)
        {
          MyTerminalProductionController.m_textCache.Clear().Append((int) queueItem.ItemId);
          myGuiGridItem.AddText(MyTerminalProductionController.m_textCache, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
        }
        this.m_queueGrid.Add(myGuiGridItem);
        ++num3;
      }
      if (num2 == 0 && this.m_queueGrid.Items.Count > 0 && this.m_queueGrid.Items[0] != null)
        this.m_productionStartTime = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      this.m_queueGrid.RowsCount = Math.Max(1 + num3 / this.m_queueGrid.ColumnsCount, MyTerminalProductionController.QUEUE_GRID_ROWS);
      this.CheckIfAnimationIsNeeded();
      if (this.m_isAnimationNeeded)
        this.RefreshProgress();
      if (resetScroll || this.m_queueGrid.RowsCount <= 2)
        num1 = 0.0f;
      if ((double) this.m_queueArea.ScrollbarVPosition != (double) num1)
        this.m_queueArea.ScrollbarVPosition = num1;
      if (!selectedIndex.HasValue || this.m_queueGrid.Items.Count <= selectedIndex.Value)
        return;
      this.m_queueGrid.SelectedIndex = selectedIndex;
    }

    private void RefreshBlueprints()
    {
      if (this.m_blueprintButtonGroup.SelectedButton == null || !(this.m_blueprintButtonGroup.SelectedButton.UserData is MyBlueprintClassDefinition userData))
        return;
      this.m_blueprintsGrid.Clear();
      bool flag = !string.IsNullOrEmpty(this.m_blueprintsSearchBox.SearchText);
      string str = MyInput.Static.IsJoystickLastUsed ? "\n" + MyTexts.GetString(MySpaceTexts.ToolTipTerminalProduction_ItemInfoGamepad) : string.Empty;
      int num = 0;
      foreach (MyBlueprintDefinitionBase blueprintDefinitionBase in userData)
      {
        if (blueprintDefinitionBase.Public && (!flag || blueprintDefinitionBase.DisplayNameText.Contains(this.m_blueprintsSearchBox.SearchText, StringComparison.OrdinalIgnoreCase)))
        {
          MyToolTips toolTips = new MyToolTips();
          toolTips.AddToolTip(blueprintDefinitionBase.DisplayNameText + str, MyPlatformGameSettings.ITEM_TOOLTIP_SCALE);
          this.m_blueprintsGrid.Add(new MyGuiGridItem(blueprintDefinitionBase.Icons, toolTips: toolTips, userData: ((object) blueprintDefinitionBase)));
          ++num;
        }
      }
      this.m_blueprintsGrid.RowsCount = Math.Max(1 + num / this.m_blueprintsGrid.ColumnsCount, MyTerminalProductionController.BLUEPRINT_GRID_ROWS);
      this.RefreshBlueprintGridColors();
    }

    private void RefreshBlueprintGridColors()
    {
      this.m_blueprintUnbuildabilityCache.Clear();
      this.SelectedAssembler.InventoryOwnersDirty = true;
      for (int rowIdx = 0; rowIdx < this.m_blueprintsGrid.RowsCount; ++rowIdx)
      {
        for (int colIdx = 0; colIdx < this.m_blueprintsGrid.ColumnsCount; ++colIdx)
        {
          MyGuiGridItem itemAt = this.m_blueprintsGrid.TryGetItemAt(rowIdx, colIdx);
          if (itemAt != null && itemAt.UserData is MyBlueprintDefinitionBase userData)
          {
            itemAt.IconColorMask = Vector4.One;
            if (this.SelectedAssembler != null)
            {
              this.AddComponentPrerequisites(userData, (MyFixedPoint) 1, MyTerminalProductionController.m_requiredCountCache);
              bool flag = false;
              if (this.CurrentAssemblerMode == MyTerminalProductionController.AssemblerMode.Assembling)
              {
                foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> keyValuePair in MyTerminalProductionController.m_requiredCountCache)
                {
                  if (!this.SelectedAssembler.CheckConveyorResources(new MyFixedPoint?(keyValuePair.Value), keyValuePair.Key))
                  {
                    itemAt.IconColorMask = MyTerminalProductionController.ERROR_ICON_COLOR_MASK;
                    flag = true;
                    break;
                  }
                }
              }
              else if (this.CurrentAssemblerMode == MyTerminalProductionController.AssemblerMode.Disassembling && !this.SelectedAssembler.CheckConveyorResources(new MyFixedPoint?(), userData.Results[0].Id))
              {
                itemAt.IconColorMask = MyTerminalProductionController.ERROR_ICON_COLOR_MASK;
                flag = true;
              }
              MyTerminalProductionController.m_requiredCountCache.Clear();
              if (flag)
                this.m_blueprintUnbuildabilityCache.Add(userData.Id);
            }
          }
        }
      }
    }

    private void CheckIfAnimationIsNeeded() => this.m_isAnimationNeeded = true;

    private void RefreshProgress()
    {
      if (this.SelectedAssembler == null)
        return;
      int currentItemIndex = this.SelectedAssembler.CurrentItemIndex;
      MyGuiGridItem itemAt1 = this.m_queueGrid.TryGetItemAt(currentItemIndex);
      bool flag1 = this.SelectedAssembler.CubeGrid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId) == MyResourceStateEnum.Ok;
      MyBlueprintDefinitionBase blueprint = (MyBlueprintDefinitionBase) null;
      if (itemAt1 != null)
        blueprint = ((MyProductionBlock.QueueItem) itemAt1.UserData).Blueprint;
      MyAssembler.StateEnum currentState = this.SelectedAssembler.GetCurrentState(blueprint);
      bool flag2 = currentState == MyAssembler.StateEnum.Ok;
      bool flag3 = currentState == MyAssembler.StateEnum.Ok || currentState == MyAssembler.StateEnum.MissingItems;
      for (int itemIdx = 0; itemIdx < this.m_queueGrid.GetItemsCount(); ++itemIdx)
      {
        MyGuiGridItem itemAt2 = this.m_queueGrid.TryGetItemAt(itemIdx);
        if (itemAt2 == null)
          break;
        if (itemIdx < currentItemIndex)
        {
          itemAt2.IconColorMask = MyTerminalProductionController.ERROR_ICON_COLOR_MASK;
          itemAt2.OverlayColorMask = MyTerminalProductionController.COLOR_MASK_WHITE;
          itemAt2.ToolTip.ToolTips.Clear();
          itemAt2.ToolTip.AddToolTip(MyTerminalProductionController.GetAssemblerStateText(MyAssembler.StateEnum.MissingItems), MyPlatformGameSettings.ITEM_TOOLTIP_SCALE, "Red");
        }
        else
        {
          if (itemIdx == currentItemIndex || currentItemIndex == -1)
          {
            if (currentItemIndex == -1)
              flag2 = false;
            itemAt2.IconColorMask = flag2 ? MyTerminalProductionController.COLOR_MASK_WHITE : MyTerminalProductionController.ERROR_ICON_COLOR_MASK;
            itemAt2.OverlayColorMask = MyTerminalProductionController.COLOR_MASK_WHITE;
          }
          else
          {
            itemAt2.IconColorMask = flag3 ? MyTerminalProductionController.COLOR_MASK_WHITE : MyTerminalProductionController.ERROR_ICON_COLOR_MASK;
            itemAt2.OverlayColorMask = MyTerminalProductionController.COLOR_MASK_WHITE;
          }
          itemAt2.ToolTip.ToolTips.Clear();
          if (itemIdx == currentItemIndex & flag2)
          {
            MyProductionBlock.QueueItem userData = (MyProductionBlock.QueueItem) itemAt2.UserData;
            float currentProgress = this.SelectedAssembler.CurrentProgress;
            MyTimeSpan myTimeSpan1 = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
            float blueprintProductionTime = this.SelectedAssembler.GetCurrentBlueprintProductionTime();
            float num1 = 0.0f;
            if (!this.m_blueprintUnbuildabilityCache.Contains(userData.Blueprint.Id))
            {
              MyTimeSpan myTimeSpan2 = this.SelectedAssembler.GetLastProgressUpdateTime();
              if (myTimeSpan2 < this.m_productionStartTime - MyTerminalProductionController.TIME_EPSILON)
                myTimeSpan2 = this.m_productionStartTime;
              float num2 = (double) blueprintProductionTime <= 0.0 ? 0.0f : (float) (myTimeSpan1 - myTimeSpan2).Milliseconds / blueprintProductionTime;
              float num3 = currentProgress + (flag1 ? num2 : 0.0f);
              int val1 = (int) userData.Amount - (int) num3;
              num1 = val1 > 0 ? num3 % 1f : 1f;
              itemAt2.ClearText(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
              itemAt2.AddText(Math.Max(val1, 1).ToString() + "x", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
            }
            itemAt2.OverlayPercent = num1;
            itemAt2.ToolTip.ToolTips.Clear();
            MyTerminalProductionController.m_textCache.Clear().AppendFormat("{0}: {1}%", (object) userData.Blueprint.DisplayNameText, (object) (int) ((double) num1 * 100.0));
            itemAt2.ToolTip.AddToolTip(MyTerminalProductionController.m_textCache.ToString(), MyPlatformGameSettings.ITEM_TOOLTIP_SCALE);
          }
          if (!flag2)
          {
            MyTerminalProductionController.GetAssemblerStateText(currentState);
            itemAt2.ToolTip.AddToolTip(MyTerminalProductionController.GetAssemblerStateText(currentState), MyPlatformGameSettings.ITEM_TOOLTIP_SCALE, "Red");
          }
        }
        if (MyInput.Static.IsJoystickLastUsed)
          itemAt2.ToolTip.AddToolTip(MyTexts.GetString(MySpaceTexts.ToolTipTerminalProduction_ProductionQueue_ItemInfoGamepad), MyPlatformGameSettings.ITEM_TOOLTIP_SCALE);
      }
    }

    private void RefreshAssemblerModeView()
    {
      bool flag = this.CurrentAssemblerMode == MyTerminalProductionController.AssemblerMode.Assembling;
      bool repeatEnabled = this.SelectedAssembler.RepeatEnabled;
      this.m_blueprintsArea.Enabled = true;
      this.m_blueprintsBgPanel.Enabled = true;
      this.m_blueprintsLabel.Enabled = true;
      foreach (MyGuiControlBase myGuiControlBase in this.m_blueprintButtonGroup)
        myGuiControlBase.Enabled = true;
      this.m_materialsLabel.Text = flag ? MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_RequiredAndAvailable) : MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_GainedAndAvailable);
      this.m_queueGrid.Enabled = flag || !repeatEnabled;
      this.m_disassembleAllButton.Visible = !flag && !repeatEnabled;
      this.RefreshBlueprintGridColors();
    }

    private void RefreshRepeatMode(bool repeatModeEnabled)
    {
      if (this.SelectedAssembler.IsSlave & repeatModeEnabled)
        this.RefreshSlaveMode(false);
      this.SelectedAssembler.CurrentModeChanged -= new Action<MyAssembler>(this.assembler_CurrentModeChanged);
      this.SelectedAssembler.RequestRepeatEnabled(repeatModeEnabled);
      this.SelectedAssembler.CurrentModeChanged += new Action<MyAssembler>(this.assembler_CurrentModeChanged);
      this.m_repeatCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) null;
      this.m_repeatCheckbox.IsChecked = this.SelectedAssembler.RepeatEnabled;
      this.m_repeatCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.repeatCheckbox_IsCheckedChanged);
      this.m_repeatCheckbox.Visible = this.SelectedAssembler.SupportsAdvancedFunctions;
    }

    private void RefreshSlaveMode(bool slaveModeEnabled)
    {
      if (this.SelectedAssembler.RepeatEnabled & slaveModeEnabled)
        this.RefreshRepeatMode(false);
      if (this.SelectedAssembler.DisassembleEnabled)
      {
        this.m_slaveCheckbox.Enabled = false;
        this.m_slaveCheckbox.Visible = false;
      }
      if (!this.SelectedAssembler.DisassembleEnabled)
      {
        this.m_slaveCheckbox.Enabled = true;
        this.m_slaveCheckbox.Visible = true;
      }
      this.SelectedAssembler.CurrentModeChanged -= new Action<MyAssembler>(this.assembler_CurrentModeChanged);
      this.SelectedAssembler.IsSlave = slaveModeEnabled;
      this.SelectedAssembler.CurrentModeChanged += new Action<MyAssembler>(this.assembler_CurrentModeChanged);
      this.m_slaveCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) null;
      this.m_slaveCheckbox.IsChecked = this.SelectedAssembler.IsSlave;
      this.m_slaveCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.slaveCheckbox_IsCheckedChanged);
      if (this.SelectedAssembler.SupportsAdvancedFunctions)
        return;
      this.m_slaveCheckbox.Visible = false;
    }

    private void EnqueueBlueprint(MyBlueprintDefinitionBase blueprint, MyFixedPoint amount)
    {
      MyTerminalProductionController.m_blueprintCache.Clear();
      blueprint.GetBlueprints(MyTerminalProductionController.m_blueprintCache);
      foreach (MyBlueprintDefinitionBase.ProductionInfo productionInfo in MyTerminalProductionController.m_blueprintCache)
        this.SelectedAssembler.InsertQueueItemRequest(-1, productionInfo.Blueprint, productionInfo.Amount * amount);
      MyTerminalProductionController.m_blueprintCache.Clear();
    }

    private void ShowBlueprintComponents(MyBlueprintDefinitionBase blueprint, MyFixedPoint amount)
    {
      this.m_materialsList.Clear();
      if (blueprint == null)
        return;
      this.AddComponentPrerequisites(blueprint, amount, MyTerminalProductionController.m_requiredCountCache);
      this.FillMaterialList(MyTerminalProductionController.m_requiredCountCache);
      MyTerminalProductionController.m_requiredCountCache.Clear();
    }

    private void FillMaterialList(Dictionary<MyDefinitionId, MyFixedPoint> materials)
    {
      bool flag = this.CurrentAssemblerMode == MyTerminalProductionController.AssemblerMode.Disassembling;
      foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> material in materials)
      {
        MyFixedPoint itemAmount = this.SelectedAssembler.InventoryAggregate.GetItemAmount(material.Key, MyItemFlags.None, false);
        string font = flag || material.Value <= itemAmount ? "White" : "Red";
        this.m_materialsList.Add(material.Key, (double) material.Value, (double) itemAmount, font);
      }
    }

    private void AddComponentPrerequisites(
      MyBlueprintDefinitionBase blueprint,
      MyFixedPoint multiplier,
      Dictionary<MyDefinitionId, MyFixedPoint> outputAmounts)
    {
      MyFixedPoint myFixedPoint = (MyFixedPoint) (float) (1.0 / (this.SelectedAssembler != null ? (double) this.SelectedAssembler.GetEfficiencyMultiplierForBlueprint(blueprint) : (double) MySession.Static.AssemblerEfficiencyMultiplier));
      foreach (MyBlueprintDefinitionBase.Item prerequisite in blueprint.Prerequisites)
      {
        if (!outputAmounts.ContainsKey(prerequisite.Id))
          outputAmounts[prerequisite.Id] = (MyFixedPoint) 0;
        outputAmounts[prerequisite.Id] += prerequisite.Amount * multiplier * myFixedPoint;
      }
    }

    private void StartDragging(
      MyDropHandleType dropHandlingType,
      MyGuiControlGrid gridControl,
      ref MyGuiControlGrid.EventArgs args)
    {
      this.m_dragAndDropInfo = new MyDragAndDropInfo();
      this.m_dragAndDropInfo.Grid = gridControl;
      this.m_dragAndDropInfo.ItemIndex = args.ItemIndex;
      MyGuiGridItem itemAt = this.m_dragAndDropInfo.Grid.GetItemAt(this.m_dragAndDropInfo.ItemIndex);
      this.m_dragAndDrop.StartDragging(dropHandlingType, args.Button, itemAt, this.m_dragAndDropInfo, false);
    }

    private void SelectModeButton(MyAssembler assembler)
    {
      bool advancedFunctions = assembler.SupportsAdvancedFunctions;
      foreach (MyGuiControlBase myGuiControlBase in this.m_modeButtonGroup)
        myGuiControlBase.Enabled = advancedFunctions;
      this.m_modeButtonGroup.SelectByKey(assembler.DisassembleEnabled ? 1 : 0);
    }

    private void RefreshMaterialsPreview(bool isFocused)
    {
      int num = 0;
      try
      {
        this.m_materialsList.Clear();
        num = 1;
        if (isFocused)
        {
          num = 2;
          if (this.m_blueprintsGrid.SelectedItem != null)
          {
            num = 3;
            this.ShowBlueprintComponents((MyBlueprintDefinitionBase) this.m_blueprintsGrid.SelectedItem.UserData, (MyFixedPoint) 1);
            num = 4;
          }
          else if (this.m_inventoryGrid.SelectedItem != null && this.CurrentAssemblerMode == MyTerminalProductionController.AssemblerMode.Disassembling)
          {
            num = 5;
            MyPhysicalInventoryItem userData = (MyPhysicalInventoryItem) this.m_inventoryGrid.SelectedItem.UserData;
            num = 6;
            if (MyDefinitionManager.Static.HasBlueprint(userData.Content.GetId()))
            {
              num = 7;
              this.ShowBlueprintComponents(MyDefinitionManager.Static.GetBlueprintDefinition(userData.Content.GetId()), (MyFixedPoint) 1);
              num = 8;
            }
            num = 9;
          }
          else if (this.m_queueGrid.SelectedItem != null)
          {
            num = 10;
            MyProductionBlock.QueueItem userData = (MyProductionBlock.QueueItem) this.m_queueGrid.SelectedItem.UserData;
            num = 11;
            this.ShowBlueprintComponents(userData.Blueprint, userData.Amount);
            num = 12;
          }
          else if (this.SelectedAssembler != null)
          {
            num = 13;
            foreach (MyProductionBlock.QueueItem queueItem in this.SelectedAssembler.Queue)
            {
              num = 14;
              this.AddComponentPrerequisites(queueItem.Blueprint, queueItem.Amount, MyTerminalProductionController.m_requiredCountCache);
              num = 15;
            }
            this.FillMaterialList(MyTerminalProductionController.m_requiredCountCache);
            num = 16;
          }
          num = 17;
        }
        else
        {
          num = 18;
          if (this.m_blueprintsGrid.MouseOverItem != null)
          {
            num = 19;
            this.ShowBlueprintComponents((MyBlueprintDefinitionBase) this.m_blueprintsGrid.MouseOverItem.UserData, (MyFixedPoint) 1);
            num = 20;
          }
          else if (this.m_inventoryGrid.MouseOverItem != null && this.CurrentAssemblerMode == MyTerminalProductionController.AssemblerMode.Disassembling)
          {
            num = 21;
            MyPhysicalInventoryItem userData = (MyPhysicalInventoryItem) this.m_inventoryGrid.MouseOverItem.UserData;
            num = 22;
            if (MyDefinitionManager.Static.HasBlueprint(userData.Content.GetId()))
            {
              num = 23;
              this.ShowBlueprintComponents(MyDefinitionManager.Static.GetBlueprintDefinition(userData.Content.GetId()), (MyFixedPoint) 1);
              num = 24;
            }
            num = 25;
          }
          else if (this.m_queueGrid.MouseOverItem != null)
          {
            num = 26;
            MyProductionBlock.QueueItem userData = (MyProductionBlock.QueueItem) this.m_queueGrid.MouseOverItem.UserData;
            num = 27;
            this.ShowBlueprintComponents(userData.Blueprint, userData.Amount);
            num = 28;
          }
          else if (this.SelectedAssembler != null)
          {
            num = 29;
            foreach (MyProductionBlock.QueueItem queueItem in this.SelectedAssembler.Queue)
            {
              num = 30;
              this.AddComponentPrerequisites(queueItem.Blueprint, queueItem.Amount, MyTerminalProductionController.m_requiredCountCache);
              num = 31;
            }
            num = 32;
            this.FillMaterialList(MyTerminalProductionController.m_requiredCountCache);
            num = 33;
          }
          num = 34;
        }
        num = 35;
        MyTerminalProductionController.m_requiredCountCache.Clear();
      }
      catch
      {
        MyLog.Default.WriteLine("Crash in RefreshMaterialsPreview line " + (object) num);
        throw;
      }
    }

    private static string GetAssemblerStateText(MyAssembler.StateEnum state)
    {
      MyStringId id = MySpaceTexts.Blank;
      switch (state)
      {
        case MyAssembler.StateEnum.Ok:
          id = MySpaceTexts.Blank;
          break;
        case MyAssembler.StateEnum.Disabled:
          id = MySpaceTexts.AssemblerState_Disabled;
          break;
        case MyAssembler.StateEnum.NotWorking:
          id = MySpaceTexts.AssemblerState_NotWorking;
          break;
        case MyAssembler.StateEnum.NotEnoughPower:
          id = MySpaceTexts.AssemblerState_NotEnoughPower;
          break;
        case MyAssembler.StateEnum.MissingItems:
          id = MySpaceTexts.AssemblerState_MissingItems;
          break;
        case MyAssembler.StateEnum.InventoryFull:
          id = MySpaceTexts.AssemblerState_InventoryFull;
          break;
      }
      return MyTexts.GetString(id);
    }

    private void blueprintButtonGroup_SelectedChanged(MyGuiControlRadioButtonGroup obj) => this.RefreshBlueprints();

    private void Assemblers_ItemSelected()
    {
      if (this.m_assemblersByKey.Count <= 0 || !this.m_assemblersByKey.ContainsKey((int) this.m_comboboxAssemblers.GetSelectedKey()))
        return;
      this.SelectAndShowAssembler(this.m_assemblersByKey[(int) this.m_comboboxAssemblers.GetSelectedKey()]);
    }

    private void assembler_CurrentModeChanged(MyAssembler assembler)
    {
      this.SelectModeButton(assembler);
      this.RefreshRepeatMode(assembler.RepeatEnabled);
      this.RefreshSlaveMode(assembler.IsSlave);
      this.RefreshProgress();
      this.RefreshAssemblerModeView();
      this.RefreshMaterialsPreview(false);
    }

    private void assembler_QueueChanged(MyProductionBlock block)
    {
      this.RefreshQueue();
      this.RefreshMaterialsPreview(false);
      this.RefreshProgress();
    }

    private void assembler_CurrentProgressChanged(MyAssembler assembler)
    {
      this.RefreshProgress();
      this.CheckIfAnimationIsNeeded();
    }

    private void assembler_CurrentStateChanged(MyAssembler obj)
    {
      this.RefreshProgress();
      this.CheckIfAnimationIsNeeded();
    }

    private void InputInventory_ContentsChanged(MyInventoryBase obj)
    {
      if (this.CurrentAssemblerMode == MyTerminalProductionController.AssemblerMode.Assembling)
        this.RefreshBlueprintGridColors();
      this.RefreshMaterialsPreview(false);
    }

    private void OutputInventory_ContentsChanged(MyInventoryBase obj)
    {
      this.RefreshInventory();
      this.RefreshMaterialsPreview(false);
    }

    private void OnSearchTextChanged(string text) => this.RefreshBlueprints();

    private void blueprintsGrid_ItemClicked(
      MyGuiControlGrid control,
      MyGuiControlGrid.EventArgs args)
    {
      MyGuiGridItem itemAt = control.GetItemAt(args.ItemIndex);
      if (itemAt == null)
        return;
      MyBlueprintDefinitionBase userData = (MyBlueprintDefinitionBase) itemAt.UserData;
      int num = 1;
      if (MyInput.Static.IsAnyCtrlKeyPressed() || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED))
      {
        if (MyInput.Static.IsAnyShiftKeyPressed() || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED))
          num *= 1000;
        else
          num *= 10;
      }
      else if (MyInput.Static.IsAnyShiftKeyPressed() || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED))
        num *= 100;
      this.EnqueueBlueprint(userData, (MyFixedPoint) num);
    }

    private void inventoryGrid_ItemClicked(
      MyGuiControlGrid control,
      MyGuiControlGrid.EventArgs args)
    {
      if (this.CurrentAssemblerMode == MyTerminalProductionController.AssemblerMode.Assembling)
        return;
      MyGuiGridItem itemAt = control.GetItemAt(args.ItemIndex);
      if (itemAt == null)
        return;
      MyBlueprintDefinitionBase definitionByResultId = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(((MyPhysicalInventoryItem) itemAt.UserData).Content.GetId());
      if (definitionByResultId == null)
        return;
      int num = (MyInput.Static.IsAnyShiftKeyPressed() ? 100 : 1) * (MyInput.Static.IsAnyCtrlKeyPressed() ? 10 : 1);
      this.EnqueueBlueprint(definitionByResultId, (MyFixedPoint) num);
    }

    private void queueGrid_ItemClicked(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
    {
      if (this.CurrentAssemblerMode == MyTerminalProductionController.AssemblerMode.Disassembling && this.SelectedAssembler.RepeatEnabled || args.Button != MySharedButtonsEnum.Secondary)
        return;
      this.SelectedAssembler.RemoveQueueItemRequest(args.ItemIndex, (MyFixedPoint) -1);
    }

    private void queueGrid_ItemDragged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args) => this.StartDragging(MyDropHandleType.MouseRelease, control, ref args);

    private void dragDrop_OnItemDropped(object sender, MyDragAndDropEventArgs eventArgs)
    {
      if (this.SelectedAssembler != null && eventArgs.DropTo != null && eventArgs.DragFrom.Grid == eventArgs.DropTo.Grid)
        this.SelectedAssembler.MoveQueueItemRequest(((MyProductionBlock.QueueItem) eventArgs.Item.UserData).ItemId, eventArgs.DropTo.ItemIndex);
      this.m_dragAndDropInfo = (MyDragAndDropInfo) null;
    }

    private void blueprintsGrid_MouseOverIndexChanged(
      MyGuiControlGrid control,
      MyGuiControlGrid.EventArgs args)
    {
      this.RefreshMaterialsPreview(false);
    }

    private void blueprintsGrid_FocusChanged(
      MyGuiControlGrid control,
      MyGuiControlGrid.EventArgs args)
    {
      this.RefreshMaterialsPreview(true);
    }

    private void inventoryGrid_MouseOverIndexChanged(
      MyGuiControlGrid control,
      MyGuiControlGrid.EventArgs args)
    {
      if (this.CurrentAssemblerMode == MyTerminalProductionController.AssemblerMode.Assembling)
        return;
      this.RefreshMaterialsPreview(false);
    }

    private void inventoryGrid_FocusChanged(
      MyGuiControlGrid control,
      MyGuiControlGrid.EventArgs args)
    {
      if (this.CurrentAssemblerMode == MyTerminalProductionController.AssemblerMode.Assembling)
        return;
      this.RefreshMaterialsPreview(true);
    }

    private void queueGrid_MouseOverIndexChanged(
      MyGuiControlGrid control,
      MyGuiControlGrid.EventArgs args)
    {
      this.RefreshMaterialsPreview(false);
    }

    private void queueGrid_FocusChanged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args) => this.RefreshMaterialsPreview(true);

    private void modeButtonGroup_SelectedChanged(MyGuiControlRadioButtonGroup obj)
    {
      this.SelectedAssembler.CurrentModeChanged -= new Action<MyAssembler>(this.assembler_CurrentModeChanged);
      bool newDisassembleEnabled = obj.SelectedButton.Key == 1;
      this.SelectedAssembler.RequestDisassembleEnabled(newDisassembleEnabled);
      if (newDisassembleEnabled)
      {
        this.m_slaveCheckbox.Enabled = false;
        this.m_slaveCheckbox.Visible = false;
      }
      if (!newDisassembleEnabled && this.SelectedAssembler.SupportsAdvancedFunctions)
      {
        this.m_slaveCheckbox.Enabled = true;
        this.m_slaveCheckbox.Visible = true;
      }
      this.SelectedAssembler.CurrentModeChanged += new Action<MyAssembler>(this.assembler_CurrentModeChanged);
      this.m_repeatCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) null;
      this.m_repeatCheckbox.IsChecked = this.SelectedAssembler.RepeatEnabled;
      this.m_repeatCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.repeatCheckbox_IsCheckedChanged);
      this.m_slaveCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) null;
      this.m_slaveCheckbox.IsChecked = this.SelectedAssembler.IsSlave;
      this.m_slaveCheckbox.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.slaveCheckbox_IsCheckedChanged);
      this.RefreshProgress();
      this.RefreshAssemblerModeView();
      this.m_queueArea.ScrollbarVPosition = 0.0f;
    }

    private void repeatCheckbox_IsCheckedChanged(MyGuiControlCheckbox control)
    {
      this.RefreshRepeatMode(control.IsChecked);
      this.RefreshAssemblerModeView();
    }

    private void slaveCheckbox_IsCheckedChanged(MyGuiControlCheckbox control)
    {
      this.RefreshSlaveMode(control.IsChecked);
      this.RefreshAssemblerModeView();
    }

    private void controlPanelButton_ButtonClicked(MyGuiControlButton control) => MyGuiScreenTerminal.SwitchToControlPanelBlock((MyTerminalBlock) this.SelectedAssembler);

    private void inventoryButton_ButtonClicked(MyGuiControlButton control) => MyGuiScreenTerminal.SwitchToInventory((MyTerminalBlock) this.SelectedAssembler);

    private void TerminalSystem_BlockAdded(MyTerminalBlock obj)
    {
      if (!(obj is MyAssembler myAssembler))
        return;
      if (this.m_assemblersByKey.Count == 0)
        MyTerminalProductionController.HideError(this.m_controlsParent);
      int key = this.m_assemblerKeyCounter++;
      this.m_assemblersByKey.Add(key, myAssembler);
      this.m_comboboxAssemblers.AddItem((long) key, myAssembler.CustomName);
      if (this.m_assemblersByKey.Count == 1)
        this.m_comboboxAssemblers.SelectItemByIndex(0);
      myAssembler.CustomNameChanged += new Action<MyTerminalBlock>(this.assembler_CustomNameChanged);
    }

    private void TerminalSystem_BlockRemoved(MyTerminalBlock obj)
    {
      if (!(obj is MyAssembler myAssembler))
        return;
      myAssembler.CustomNameChanged -= new Action<MyTerminalBlock>(this.assembler_CustomNameChanged);
      int? nullable = new int?();
      foreach (KeyValuePair<int, MyAssembler> keyValuePair in this.m_assemblersByKey)
      {
        if (keyValuePair.Value == myAssembler)
        {
          nullable = new int?(keyValuePair.Key);
          break;
        }
      }
      if (nullable.HasValue)
      {
        this.m_assemblersByKey.Remove(nullable.Value);
        this.m_comboboxAssemblers.RemoveItem((long) nullable.Value);
      }
      if (myAssembler != this.SelectedAssembler)
        return;
      if (this.m_assemblersByKey.Count > 0)
        this.m_comboboxAssemblers.SelectItemByIndex(0);
      else
        MyTerminalProductionController.ShowError(MySpaceTexts.ScreenTerminalError_NoAssemblers, this.m_controlsParent);
    }

    private void assembler_CustomNameChanged(MyTerminalBlock block)
    {
      foreach (KeyValuePair<int, MyAssembler> keyValuePair in this.m_assemblersByKey)
      {
        if (keyValuePair.Value == block)
          this.m_comboboxAssemblers.TryGetItemByKey((long) keyValuePair.Key).Value.Clear().AppendStringBuilder(block.CustomName);
      }
    }

    private void disassembleAllButton_ButtonClicked(MyGuiControlButton obj)
    {
      if (this.CurrentAssemblerMode != MyTerminalProductionController.AssemblerMode.Disassembling || this.SelectedAssembler.RepeatEnabled)
        return;
      this.SelectedAssembler.RequestDisassembleAll();
    }

    public override void UpdateBeforeDraw(MyGuiScreenBase screen)
    {
      base.UpdateBeforeDraw(screen);
      this.CheckIfAnimationIsNeeded();
      if (this.m_isAnimationNeeded)
      {
        this.RefreshProgress();
        if (!Sync.IsServer)
          this.CheckIfAnimationIsNeeded();
      }
      if (!this.m_dirtyDraw)
        return;
      if (MyInput.Static.IsJoystickLastUsed)
        this.m_dirtyDraw = false;
      if (!MyInput.Static.IsJoystickLastUsed)
        return;
      screen.GamepadHelpTextId = this.m_assemblingButton == null || !this.m_assemblingButton.Selected ? (this.m_disassemblingButton == null || !this.m_disassemblingButton.Selected ? new MyStringId?(MySpaceTexts.TerminalProduction_Help_Screen) : new MyStringId?(MySpaceTexts.TerminalProduction_Help_ScreenDisassembling)) : new MyStringId?(MySpaceTexts.TerminalProduction_Help_ScreenAssembling);
      if (this.m_blueprintButtonGroup.SelectedButton != null && this.m_blueprintButtonGroup.SelectedButton.UserData is MyBlueprintClassDefinition userData)
      {
        string str = string.Format(MyTexts.GetString(MySpaceTexts.TerminalProduction_Help_BlueprintFilter), (object) userData.DisplayNameText);
        screen.GamepadHelpText = str;
      }
      screen.UpdateGamepadHelp(screen.FocusedControl);
    }

    public override void HandleInput()
    {
      base.HandleInput();
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_Y) && this.m_disassemblingButton != null && (this.m_assemblingButton != null && this.m_disassemblingButton.Enabled))
      {
        if (this.m_assemblingButton.Selected)
          this.m_disassemblingButton.Selected = true;
        else if (this.m_disassemblingButton.Selected)
          this.m_assemblingButton.Selected = true;
        this.m_dirtyDraw = true;
      }
      if (this.SelectedAssembler != null && !this.m_comboboxAssemblers.IsOpen)
      {
        if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
          MyGuiScreenTerminal.SwitchToInventory((MyTerminalBlock) this.SelectedAssembler);
        if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
          MyGuiScreenTerminal.SwitchToControlPanelBlock((MyTerminalBlock) this.SelectedAssembler);
      }
      int? selectedIndex;
      if (this.m_blueprintButtonGroup != null && this.m_blueprintButtonGroup.Count > 1 && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
      {
        selectedIndex = this.m_blueprintButtonGroup.SelectedIndex;
        int num;
        if (!selectedIndex.HasValue)
        {
          num = -1;
        }
        else
        {
          selectedIndex = this.m_blueprintButtonGroup.SelectedIndex;
          num = selectedIndex.Value;
        }
        int index = num + 1;
        if (index >= this.m_blueprintButtonGroup.Count)
          index = 0;
        this.m_blueprintButtonGroup.SelectByIndex(index);
        this.m_dirtyDraw = true;
      }
      if (this.m_queueGrid != null && this.SelectedAssembler != null && (this.m_queueRemoveStackTimeMs != -1 && this.m_queueRemoveStackTimeMs <= MySandboxGame.TotalGamePlayTimeInMilliseconds))
      {
        this.m_queueRemoveStackTimeMs = -1;
        selectedIndex = this.m_queueGrid.SelectedIndex;
        int removeStackIndex = this.m_queueRemoveStackIndex;
        if (selectedIndex.GetValueOrDefault() == removeStackIndex & selectedIndex.HasValue)
        {
          MyAssembler selectedAssembler = this.SelectedAssembler;
          selectedIndex = this.m_queueGrid.SelectedIndex;
          int idx = selectedIndex.Value;
          MyFixedPoint amount = (MyFixedPoint) -1;
          selectedAssembler.RemoveQueueItemRequest(idx, amount);
        }
      }
      this.QueueReorderHandleInput();
      if (this.m_controlPanelButton != null)
        this.m_controlPanelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      if (this.m_inventoryButton == null)
        return;
      this.m_inventoryButton.Visible = !MyInput.Static.IsJoystickLastUsed;
    }

    private void QueueReorderHandleInput()
    {
      if (this.m_queueGrid == null || this.SelectedAssembler == null)
        return;
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_LEFT) && this.m_queueGrid.HasFocus && (this.m_queueGrid.SelectedIndex.HasValue && this.m_queueGrid.SelectedItem != null))
      {
        int num = this.m_queueGrid.SelectedIndex.Value;
        MyProductionBlock.QueueItem userData = (MyProductionBlock.QueueItem) this.m_queueGrid.SelectedItem.UserData;
        if (num - 1 >= 0)
          this.SelectedAssembler.MoveQueueItemRequest(userData.ItemId, num - 1);
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_RIGHT) && this.m_queueGrid.HasFocus && (this.m_queueGrid.SelectedIndex.HasValue && this.m_queueGrid.SelectedItem != null))
      {
        int num = this.m_queueGrid.SelectedIndex.Value;
        MyProductionBlock.QueueItem userData = (MyProductionBlock.QueueItem) this.m_queueGrid.SelectedItem.UserData;
        if (num + 1 < this.m_queueGrid.Items.Count)
          this.SelectedAssembler.MoveQueueItemRequest(userData.ItemId, num + 1);
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_UP) && this.m_queueGrid.HasFocus && (this.m_queueGrid.SelectedIndex.HasValue && this.m_queueGrid.SelectedItem != null))
      {
        int num = this.m_queueGrid.SelectedIndex.Value;
        MyProductionBlock.QueueItem userData = (MyProductionBlock.QueueItem) this.m_queueGrid.SelectedItem.UserData;
        if (num - 5 >= 0)
          this.SelectedAssembler.MoveQueueItemRequest(userData.ItemId, num - 5);
      }
      if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_DOWN) || !this.m_queueGrid.HasFocus || (!this.m_queueGrid.SelectedIndex.HasValue || this.m_queueGrid.SelectedItem == null))
        return;
      int num1 = this.m_queueGrid.SelectedIndex.Value;
      MyProductionBlock.QueueItem userData1 = (MyProductionBlock.QueueItem) this.m_queueGrid.SelectedItem.UserData;
      if (num1 + 5 >= this.m_queueGrid.Items.Count)
        return;
      this.SelectedAssembler.MoveQueueItemRequest(userData1.ItemId, num1 + 5);
    }

    private enum AssemblerMode
    {
      Assembling,
      Disassembling,
    }
  }
}
