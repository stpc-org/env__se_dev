// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalInventoryController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyTerminalInventoryController : MyTerminalController
  {
    public static readonly MyTimeSpan TRANSFER_TIMER_TIME = MyTimeSpan.FromMilliseconds(600.0);
    private static int m_persistentRadioSelectionLeft = 0;
    private static int m_persistentRadioSelectionRight = 0;
    private static readonly Vector2 m_controlListFullSize = new Vector2(0.437f, 0.618f);
    private static readonly Vector2 m_controlListSizeWithSearch = new Vector2(0.437f, 0.569f);
    private static readonly Vector2 m_leftControlListPosition = new Vector2(-0.452f, -0.276f);
    private static readonly Vector2 m_rightControlListPosition = new Vector2(1265f / (884f * Math.PI), -0.276f);
    private static readonly Vector2 m_leftControlListPosWithSearch = new Vector2(-0.452f, -0.227f);
    private static readonly Vector2 m_rightControlListPosWithSearch = new Vector2(1265f / (884f * Math.PI), -0.227f);
    private bool m_isTransferTimerActive;
    private MyTimeSpan m_transferTimer;
    private MyTerminalInventoryController.MyGamepadTransferCollection m_transferData;
    private MyGuiControlList m_leftOwnersControl;
    private MyGuiControlRadioButton m_leftSuitButton;
    private MyGuiControlRadioButton m_leftGridButton;
    private MyGuiControlLabel m_leftFilterGamepadHelp;
    private MyGuiControlRadioButton m_leftFilterShipButton;
    private MyGuiControlRadioButton m_leftFilterStorageButton;
    private MyGuiControlRadioButton m_leftFilterSystemButton;
    private MyGuiControlRadioButton m_leftFilterEnergyButton;
    private MyGuiControlRadioButton m_leftFilterAllButton;
    private MyGuiControlRadioButtonGroup m_leftTypeGroup;
    private MyGuiControlRadioButtonGroup m_leftFilterGroup;
    private MyGuiControlList m_rightOwnersControl;
    private MyGuiControlRadioButton m_rightSuitButton;
    private MyGuiControlRadioButton m_rightGridButton;
    private MyGuiControlLabel m_rightFilterGamepadHelp;
    private MyGuiControlRadioButton m_rightFilterShipButton;
    private MyGuiControlRadioButton m_rightFilterStorageButton;
    private MyGuiControlRadioButton m_rightFilterSystemButton;
    private MyGuiControlRadioButton m_rightFilterEnergyButton;
    private MyGuiControlRadioButton m_rightFilterAllButton;
    private MyGuiControlRadioButtonGroup m_rightTypeGroup;
    private MyGuiControlRadioButtonGroup m_rightFilterGroup;
    private MyGuiControlButton m_throwOutButton;
    private MyGuiControlButton m_withdrawButton;
    private MyGuiControlButton m_depositAllButton;
    private MyGuiControlButton m_addToProductionButton;
    private MyGuiControlButton m_selectedToProductionButton;
    private MyDragAndDropInfo m_dragAndDropInfo;
    private MyGuiControlGridDragAndDrop m_dragAndDrop;
    private List<MyGuiControlGrid> m_controlsDisabledWhileDragged;
    private MyEntity m_userAsEntity;
    private MyEntity m_interactedAsEntity;
    private MyEntity m_openInventoryInteractedAsEntity;
    private MyEntity m_userAsOwner;
    private MyEntity m_interactedAsOwner;
    private List<MyEntity> m_interactedGridOwners = new List<MyEntity>();
    private List<MyEntity> m_interactedGridOwnersMechanical = new List<MyEntity>();
    private List<IMyConveyorEndpoint> m_reachableInventoryOwners = new List<IMyConveyorEndpoint>();
    private List<MyGridConveyorSystem> m_registeredConveyorSystems = new List<MyGridConveyorSystem>();
    private List<MyGridConveyorSystem> m_registeredConveyorMechanicalSystems = new List<MyGridConveyorSystem>();
    private MyGuiControlInventoryOwner m_focusedOwnerControl;
    private MyGuiControlGrid m_focusedGridControl;
    private MyPhysicalInventoryItem? m_selectedInventoryItem;
    private MyInventory m_selectedInventory;
    private MyInventoryOwnerTypeEnum? m_leftFilterType;
    private MyInventoryOwnerTypeEnum? m_rightFilterType;
    private MyGridColorHelper m_colorHelper;
    private MyGuiControlSearchBox m_searchBoxLeft;
    private MyGuiControlSearchBox m_searchBoxRight;
    private MyGuiControlCheckbox m_hideEmptyLeft;
    private MyGuiControlLabel m_hideEmptyLeftLabel;
    private MyGuiControlCheckbox m_hideEmptyRight;
    private MyGuiControlLabel m_hideEmptyRightLabel;
    private MyGuiControlGrid m_leftFocusedInventory;
    private MyGuiControlGrid m_rightFocusedInventory;
    private Predicate<IMyConveyorEndpoint> m_endpointPredicate;
    private IMyConveyorEndpointBlock m_interactedEndpointBlock;

    public int LeftFilterTypeIndex
    {
      get => MySession.Static != null && MySession.Static.LocalHumanPlayer != null ? MySession.Static.LocalHumanPlayer.LeftFilterTypeIndex : 0;
      set
      {
        if (MySession.Static == null || MySession.Static.LocalHumanPlayer == null)
          return;
        MySession.Static.LocalHumanPlayer.LeftFilterTypeIndex = value;
      }
    }

    public int RightFilterTypeIndex
    {
      get => MySession.Static != null && MySession.Static.LocalHumanPlayer != null ? MySession.Static.LocalHumanPlayer.RightFilterTypeIndex : 0;
      set
      {
        if (MySession.Static == null || MySession.Static.LocalHumanPlayer == null)
          return;
        MySession.Static.LocalHumanPlayer.RightFilterTypeIndex = value;
      }
    }

    public MyGuiControlRadioButtonStyleEnum LeftFilter
    {
      get => MySession.Static != null && MySession.Static.LocalHumanPlayer != null ? MySession.Static.LocalHumanPlayer.LeftFilter : MyGuiControlRadioButtonStyleEnum.FilterCharacter;
      set
      {
        if (MySession.Static == null || MySession.Static.LocalHumanPlayer == null)
          return;
        MySession.Static.LocalHumanPlayer.LeftFilter = value;
      }
    }

    public MyGuiControlRadioButtonStyleEnum RightFilter
    {
      get => MySession.Static != null && MySession.Static.LocalHumanPlayer != null ? MySession.Static.LocalHumanPlayer.RightFilter : MyGuiControlRadioButtonStyleEnum.FilterCharacter;
      set
      {
        if (MySession.Static == null || MySession.Static.LocalHumanPlayer == null)
          return;
        MySession.Static.LocalHumanPlayer.RightFilter = value;
      }
    }

    private MyGuiControlInventoryOwner FocusedOwnerControl
    {
      get => this.m_focusedOwnerControl;
      set
      {
        if (this.m_focusedOwnerControl == value)
          return;
        this.m_focusedOwnerControl = value;
      }
    }

    private MyGuiControlGrid FocusedGridControl
    {
      get => this.m_focusedGridControl;
      set
      {
        if (this.m_focusedGridControl == value)
          return;
        this.m_focusedGridControl = value;
      }
    }

    private MyGuiControlGrid LeftFocusedInventory
    {
      get => this.m_leftFocusedInventory;
      set => this.LeftFocusChanged(value);
    }

    private void LeftFocusChanged(MyGuiControlGrid grid)
    {
      if (this.m_leftFocusedInventory != null && this.m_leftFocusedInventory != grid)
      {
        this.m_leftFocusedInventory.BorderSize = 1;
        this.m_leftFocusedInventory.BorderColor = MyGuiConstants.ACTIVE_BACKGROUND_COLOR;
      }
      this.m_leftFocusedInventory = grid;
      this.LeftFocusChangeBorder();
    }

    private void LeftFocusChangeBorder()
    {
      if (this.m_leftFocusedInventory == null)
        return;
      this.m_leftFocusedInventory.BorderSize = 3;
      this.m_leftFocusedInventory.BorderColor = this.m_leftFocusedInventory.HasFocus ? MyGuiConstants.FOCUS_BACKGROUND_COLOR : MyGuiConstants.ACTIVE_BACKGROUND_COLOR;
    }

    private MyGuiControlGrid RightFocusedInventory
    {
      get => this.m_rightFocusedInventory;
      set => this.RightFocusChanged(value);
    }

    private void RightFocusChanged(MyGuiControlGrid grid)
    {
      if (this.m_rightFocusedInventory != null && this.m_rightFocusedInventory != grid)
      {
        this.m_rightFocusedInventory.BorderSize = 1;
        this.m_rightFocusedInventory.BorderColor = MyGuiConstants.ACTIVE_BACKGROUND_COLOR;
      }
      this.m_rightFocusedInventory = grid;
      this.RightFocusChangeBorder();
    }

    private void RightFocusChangeBorder()
    {
      if (this.m_rightFocusedInventory == null)
        return;
      this.m_rightFocusedInventory.BorderSize = 3;
      this.m_rightFocusedInventory.BorderColor = this.m_rightFocusedInventory.HasFocus ? MyGuiConstants.FOCUS_BACKGROUND_COLOR : MyGuiConstants.ACTIVE_BACKGROUND_COLOR;
    }

    public MyTerminalInventoryController()
    {
      this.m_leftTypeGroup = new MyGuiControlRadioButtonGroup();
      this.m_leftFilterGroup = new MyGuiControlRadioButtonGroup();
      this.m_rightTypeGroup = new MyGuiControlRadioButtonGroup();
      this.m_rightFilterGroup = new MyGuiControlRadioButtonGroup();
      this.m_controlsDisabledWhileDragged = new List<MyGuiControlGrid>();
      this.m_endpointPredicate = new Predicate<IMyConveyorEndpoint>(this.EndpointPredicate);
    }

    public void Refresh()
    {
      MyCubeGrid Node = this.m_interactedAsEntity != null ? this.m_interactedAsEntity.Parent as MyCubeGrid : (MyCubeGrid) null;
      this.m_interactedGridOwners.Clear();
      if (Node != null)
      {
        foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in MyCubeGridGroups.Static.Logical.GetGroup(Node).Nodes)
        {
          MyTerminalInventoryController.GetGridInventories(node.NodeData, this.m_interactedGridOwners, this.m_interactedAsEntity, MySession.Static.LocalPlayerId);
          node.NodeData.GridSystems.ConveyorSystem.BlockAdded += new Action<MyCubeBlock>(this.ConveyorSystem_BlockAdded);
          node.NodeData.GridSystems.ConveyorSystem.BlockRemoved += new Action<MyCubeBlock>(this.ConveyorSystem_BlockRemoved);
          this.m_registeredConveyorSystems.Add(node.NodeData.GridSystems.ConveyorSystem);
        }
      }
      this.m_interactedGridOwnersMechanical.Clear();
      if (Node != null)
      {
        foreach (MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node node in MyCubeGridGroups.Static.Mechanical.GetGroup(Node).Nodes)
        {
          MyTerminalInventoryController.GetGridInventories(node.NodeData, this.m_interactedGridOwnersMechanical, this.m_interactedAsEntity, MySession.Static.LocalPlayerId);
          node.NodeData.GridSystems.ConveyorSystem.BlockAdded += new Action<MyCubeBlock>(this.ConveyorSystemMechanical_BlockAdded);
          node.NodeData.GridSystems.ConveyorSystem.BlockRemoved += new Action<MyCubeBlock>(this.ConveyorSystemMechanical_BlockRemoved);
          this.m_registeredConveyorMechanicalSystems.Add(node.NodeData.GridSystems.ConveyorSystem);
        }
      }
      this.m_leftTypeGroup.SelectedIndex = new int?(MyTerminalInventoryController.m_persistentRadioSelectionLeft);
      this.m_rightTypeGroup.SelectedIndex = new int?(MyTerminalInventoryController.m_persistentRadioSelectionRight);
      this.m_leftFilterGroup.SelectedIndex = new int?(this.LeftFilterTypeIndex);
      this.m_rightFilterGroup.SelectedIndex = new int?(this.RightFilterTypeIndex);
      this.LeftTypeGroup_SelectedChanged(this.m_leftTypeGroup);
      this.RightTypeGroup_SelectedChanged(this.m_rightTypeGroup);
      this.SetLeftFilter(this.m_leftFilterType);
      this.SetRightFilter(this.m_rightFilterType);
      this.SetFilterGamepadHelp(this.m_leftTypeGroup, this.m_leftFilterGroup, this.m_leftFilterGamepadHelp);
      this.SetFilterGamepadHelp(this.m_rightTypeGroup, this.m_rightFilterGroup, this.m_rightFilterGamepadHelp);
    }

    public void Init(
      IMyGuiControlsParent controlsParent,
      MyEntity thisEntity,
      MyEntity interactedEntity,
      MyGridColorHelper colorHelper,
      MyGuiScreenBase screen)
    {
      this.m_userAsEntity = thisEntity;
      this.m_interactedAsEntity = interactedEntity;
      this.m_colorHelper = colorHelper;
      screen.FocusChanged += new Action<MyGuiControlBase, MyGuiControlBase>(this.ParentsFocusChanged);
      this.m_leftOwnersControl = (MyGuiControlList) controlsParent.Controls.GetControlByName("LeftInventory");
      this.m_rightOwnersControl = (MyGuiControlList) controlsParent.Controls.GetControlByName("RightInventory");
      this.m_leftSuitButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("LeftSuitButton");
      this.m_leftGridButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("LeftGridButton");
      this.m_leftFilterGamepadHelp = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("LeftFilterGamepadHelp");
      this.m_leftFilterShipButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("LeftFilterShipButton");
      this.m_leftFilterStorageButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("LeftFilterStorageButton");
      this.m_leftFilterSystemButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("LeftFilterSystemButton");
      this.m_leftFilterEnergyButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("LeftFilterEnergyButton");
      this.m_leftFilterAllButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("LeftFilterAllButton");
      this.m_rightSuitButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("RightSuitButton");
      this.m_rightGridButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("RightGridButton");
      this.m_rightFilterGamepadHelp = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("RightFilterGamepadHelp");
      this.m_rightFilterShipButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("RightFilterShipButton");
      this.m_rightFilterStorageButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("RightFilterStorageButton");
      this.m_rightFilterSystemButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("RightFilterSystemButton");
      this.m_rightFilterEnergyButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("RightFilterEnergyButton");
      this.m_rightFilterAllButton = (MyGuiControlRadioButton) controlsParent.Controls.GetControlByName("RightFilterAllButton");
      this.m_throwOutButton = (MyGuiControlButton) controlsParent.Controls.GetControlByName("ThrowOutButton");
      this.m_withdrawButton = (MyGuiControlButton) controlsParent.Controls.GetControlByName("WithdrawButton");
      this.m_depositAllButton = (MyGuiControlButton) controlsParent.Controls.GetControlByName("DepositAllButton");
      this.m_addToProductionButton = (MyGuiControlButton) controlsParent.Controls.GetControlByName("AddToProductionButton");
      this.m_selectedToProductionButton = (MyGuiControlButton) controlsParent.Controls.GetControlByName("SelectedToProductionButton");
      if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.BuildPlanner != null)
      {
        this.m_withdrawButton.Enabled = MySession.Static.LocalCharacter.BuildPlanner.Count > 0;
        this.m_addToProductionButton.Enabled = MySession.Static.LocalCharacter.BuildPlanner.Count > 0;
      }
      else
      {
        this.m_withdrawButton.Enabled = false;
        this.m_addToProductionButton.Enabled = false;
      }
      this.m_selectedToProductionButton.Enabled = false;
      this.m_hideEmptyLeft = (MyGuiControlCheckbox) controlsParent.Controls.GetControlByName("CheckboxHideEmptyLeft");
      this.m_hideEmptyLeftLabel = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("LabelHideEmptyLeft");
      this.m_hideEmptyRight = (MyGuiControlCheckbox) controlsParent.Controls.GetControlByName("CheckboxHideEmptyRight");
      this.m_hideEmptyRightLabel = (MyGuiControlLabel) controlsParent.Controls.GetControlByName("LabelHideEmptyRight");
      this.m_searchBoxLeft = (MyGuiControlSearchBox) controlsParent.Controls.GetControlByName("BlockSearchLeft");
      this.m_searchBoxRight = (MyGuiControlSearchBox) controlsParent.Controls.GetControlByName("BlockSearchRight");
      this.m_hideEmptyLeft.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_HideEmpty);
      this.m_hideEmptyRight.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_HideEmpty);
      this.m_hideEmptyLeft.Visible = false;
      this.m_hideEmptyLeftLabel.Visible = false;
      this.m_hideEmptyRight.Visible = true;
      this.m_hideEmptyRightLabel.Visible = true;
      this.m_searchBoxLeft.Visible = false;
      this.m_searchBoxRight.Visible = false;
      this.m_hideEmptyLeft.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.HideEmptyLeft_Checked);
      this.m_hideEmptyRight.IsCheckedChanged += new Action<MyGuiControlCheckbox>(this.HideEmptyRight_Checked);
      this.m_searchBoxLeft.OnTextChanged += new MyGuiControlSearchBox.TextChangedDelegate(this.BlockSearchLeft_TextChanged);
      this.m_searchBoxRight.OnTextChanged += new MyGuiControlSearchBox.TextChangedDelegate(this.BlockSearchRight_TextChanged);
      this.m_leftSuitButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowCharacter);
      this.m_leftGridButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowConnected);
      this.m_leftGridButton.ShowTooltipWhenDisabled = true;
      this.m_rightSuitButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowInteracted);
      this.m_rightGridButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowConnected);
      this.m_rightGridButton.ShowTooltipWhenDisabled = true;
      this.m_leftFilterAllButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterAll);
      this.m_leftFilterEnergyButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterEnergy);
      this.m_leftFilterShipButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterShip);
      this.m_leftFilterStorageButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterStorage);
      this.m_leftFilterSystemButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterSystem);
      this.m_rightFilterAllButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterAll);
      this.m_rightFilterEnergyButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterEnergy);
      this.m_rightFilterShipButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterShip);
      this.m_rightFilterStorageButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterStorage);
      this.m_rightFilterSystemButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterSystem);
      this.m_throwOutButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ThrowOut);
      this.m_throwOutButton.ShowTooltipWhenDisabled = true;
      this.m_throwOutButton.CueEnum = GuiSounds.None;
      MyControl gameControl = MyInput.Static.GetGameControl(MyControlsSpace.BUILD_PLANNER);
      this.m_withdrawButton.SetToolTip(string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_Withdraw).ToString(), (object) gameControl));
      this.m_withdrawButton.ShowTooltipWhenDisabled = true;
      this.m_withdrawButton.CueEnum = GuiSounds.None;
      this.m_withdrawButton.DrawCrossTextureWhenDisabled = false;
      this.m_depositAllButton.SetToolTip(string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_Deposit).ToString(), (object) gameControl));
      this.m_depositAllButton.ShowTooltipWhenDisabled = true;
      this.m_depositAllButton.CueEnum = GuiSounds.None;
      this.m_depositAllButton.DrawCrossTextureWhenDisabled = false;
      this.m_addToProductionButton.SetToolTip(string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_AddComponents).ToString(), (object) gameControl));
      this.m_addToProductionButton.ShowTooltipWhenDisabled = true;
      this.m_addToProductionButton.CueEnum = GuiSounds.None;
      this.m_addToProductionButton.DrawCrossTextureWhenDisabled = false;
      this.m_selectedToProductionButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_AddSelectedComponent);
      this.m_selectedToProductionButton.ShowTooltipWhenDisabled = true;
      this.m_selectedToProductionButton.CueEnum = GuiSounds.None;
      this.m_selectedToProductionButton.DrawCrossTextureWhenDisabled = false;
      this.m_leftTypeGroup.Add(this.m_leftSuitButton);
      this.m_leftTypeGroup.Add(this.m_leftGridButton);
      this.m_rightTypeGroup.Add(this.m_rightSuitButton);
      this.m_rightTypeGroup.Add(this.m_rightGridButton);
      this.m_leftFilterGroup.Add(this.m_leftFilterAllButton);
      this.m_leftFilterGroup.Add(this.m_leftFilterEnergyButton);
      this.m_leftFilterGroup.Add(this.m_leftFilterShipButton);
      this.m_leftFilterGroup.Add(this.m_leftFilterStorageButton);
      this.m_leftFilterGroup.Add(this.m_leftFilterSystemButton);
      this.m_rightFilterGroup.Add(this.m_rightFilterAllButton);
      this.m_rightFilterGroup.Add(this.m_rightFilterEnergyButton);
      this.m_rightFilterGroup.Add(this.m_rightFilterShipButton);
      this.m_rightFilterGroup.Add(this.m_rightFilterStorageButton);
      this.m_rightFilterGroup.Add(this.m_rightFilterSystemButton);
      this.m_throwOutButton.DrawCrossTextureWhenDisabled = false;
      this.m_dragAndDrop = new MyGuiControlGridDragAndDrop(MyGuiConstants.DRAG_AND_DROP_BACKGROUND_COLOR, MyGuiConstants.DRAG_AND_DROP_TEXT_COLOR, 0.7f, MyGuiConstants.DRAG_AND_DROP_TEXT_OFFSET, true);
      controlsParent.Controls.Add((MyGuiControlBase) this.m_dragAndDrop);
      this.m_dragAndDrop.DrawBackgroundTexture = false;
      this.m_throwOutButton.ButtonClicked += new Action<MyGuiControlButton>(this.ThrowOutButton_OnButtonClick);
      this.m_withdrawButton.ButtonClicked += new Action<MyGuiControlButton>(this.WithdrawButton_ButtonClicked);
      this.m_depositAllButton.ButtonClicked += new Action<MyGuiControlButton>(this.DepositAllButton_ButtonClicked);
      this.m_addToProductionButton.ButtonClicked += new Action<MyGuiControlButton>(this.AddToProductionButton_ButtonClicked);
      this.m_selectedToProductionButton.ButtonClicked += new Action<MyGuiControlButton>(this.selectedToProductionButton_ButtonClicked);
      this.m_dragAndDrop.ItemDropped += new OnItemDropped(this.dragDrop_OnItemDropped);
      MyEntity myEntity1 = this.m_userAsEntity == null || !this.m_userAsEntity.HasInventory ? (MyEntity) null : this.m_userAsEntity;
      if (myEntity1 != null)
        this.m_userAsOwner = myEntity1;
      MyEntity myEntity2 = this.m_interactedAsEntity == null || !this.m_interactedAsEntity.HasInventory ? (MyEntity) null : this.m_interactedAsEntity;
      if (myEntity2 != null)
        this.m_interactedAsOwner = myEntity2;
      MyCubeGrid Node = this.m_interactedAsEntity != null ? this.m_interactedAsEntity.Parent as MyCubeGrid : (MyCubeGrid) null;
      this.m_interactedGridOwners.Clear();
      if (Node != null)
      {
        foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in MyCubeGridGroups.Static.Logical.GetGroup(Node).Nodes)
        {
          MyTerminalInventoryController.GetGridInventories(node.NodeData, this.m_interactedGridOwners, this.m_interactedAsEntity, MySession.Static.LocalPlayerId);
          node.NodeData.GridSystems.ConveyorSystem.BlockAdded += new Action<MyCubeBlock>(this.ConveyorSystem_BlockAdded);
          node.NodeData.GridSystems.ConveyorSystem.BlockRemoved += new Action<MyCubeBlock>(this.ConveyorSystem_BlockRemoved);
          this.m_registeredConveyorSystems.Add(node.NodeData.GridSystems.ConveyorSystem);
        }
      }
      this.m_interactedGridOwnersMechanical.Clear();
      if (Node != null)
      {
        foreach (MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node node in MyCubeGridGroups.Static.Mechanical.GetGroup(Node).Nodes)
        {
          MyTerminalInventoryController.GetGridInventories(node.NodeData, this.m_interactedGridOwnersMechanical, this.m_interactedAsEntity, MySession.Static.LocalPlayerId);
          node.NodeData.GridSystems.ConveyorSystem.BlockAdded += new Action<MyCubeBlock>(this.ConveyorSystemMechanical_BlockAdded);
          node.NodeData.GridSystems.ConveyorSystem.BlockRemoved += new Action<MyCubeBlock>(this.ConveyorSystemMechanical_BlockRemoved);
          this.m_registeredConveyorMechanicalSystems.Add(node.NodeData.GridSystems.ConveyorSystem);
        }
      }
      if (this.m_interactedAsEntity is MyCharacter || this.m_interactedAsEntity is MyInventoryBagEntity)
        MyTerminalInventoryController.m_persistentRadioSelectionRight = 0;
      if (this.LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterCharacter)
        this.m_leftTypeGroup.SelectedIndex = new int?(0);
      else if (this.LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
        this.m_leftTypeGroup.SelectedIndex = new int?(1);
      if (this.RightFilter == MyGuiControlRadioButtonStyleEnum.FilterCharacter)
        this.m_rightTypeGroup.SelectedIndex = new int?(0);
      else if (this.RightFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
        this.m_rightTypeGroup.SelectedIndex = new int?(1);
      this.LeftTypeGroup_SelectedChanged(this.m_leftTypeGroup);
      this.RightTypeGroup_SelectedChanged(this.m_rightTypeGroup);
      this.m_leftFilterGroup.SelectByIndex(this.LeftFilterTypeIndex);
      this.m_rightFilterGroup.SelectByIndex(this.RightFilterTypeIndex);
      this.m_leftTypeGroup.SelectedChanged += new Action<MyGuiControlRadioButtonGroup>(this.LeftTypeGroup_SelectedChanged);
      this.m_rightTypeGroup.SelectedChanged += new Action<MyGuiControlRadioButtonGroup>(this.RightTypeGroup_SelectedChanged);
      this.m_leftFilterAllButton.SelectedChanged += (Action<MyGuiControlRadioButton>) (button =>
      {
        if (!button.Selected)
          return;
        this.LeftFilterTypeIndex = 0;
        this.SetLeftFilter(new MyInventoryOwnerTypeEnum?());
      });
      this.m_leftFilterEnergyButton.SelectedChanged += (Action<MyGuiControlRadioButton>) (button =>
      {
        if (!button.Selected)
          return;
        this.LeftFilterTypeIndex = 1;
        this.SetLeftFilter(new MyInventoryOwnerTypeEnum?(MyInventoryOwnerTypeEnum.Energy));
      });
      this.m_leftFilterStorageButton.SelectedChanged += (Action<MyGuiControlRadioButton>) (button =>
      {
        if (!button.Selected)
          return;
        this.LeftFilterTypeIndex = 3;
        this.SetLeftFilter(new MyInventoryOwnerTypeEnum?(MyInventoryOwnerTypeEnum.Storage));
      });
      this.m_leftFilterSystemButton.SelectedChanged += (Action<MyGuiControlRadioButton>) (button =>
      {
        if (!button.Selected)
          return;
        this.LeftFilterTypeIndex = 4;
        this.SetLeftFilter(new MyInventoryOwnerTypeEnum?(MyInventoryOwnerTypeEnum.System));
      });
      this.m_leftFilterShipButton.SelectedChanged += (Action<MyGuiControlRadioButton>) (button =>
      {
        if (!button.Selected)
          return;
        this.LeftFilterTypeIndex = 2;
        this.SetLeftFilter(new MyInventoryOwnerTypeEnum?());
      });
      this.m_rightFilterAllButton.SelectedChanged += (Action<MyGuiControlRadioButton>) (button =>
      {
        if (!button.Selected)
          return;
        this.RightFilterTypeIndex = 0;
        this.SetRightFilter(new MyInventoryOwnerTypeEnum?());
      });
      this.m_rightFilterEnergyButton.SelectedChanged += (Action<MyGuiControlRadioButton>) (button =>
      {
        if (!button.Selected)
          return;
        this.RightFilterTypeIndex = 1;
        this.SetRightFilter(new MyInventoryOwnerTypeEnum?(MyInventoryOwnerTypeEnum.Energy));
      });
      this.m_rightFilterStorageButton.SelectedChanged += (Action<MyGuiControlRadioButton>) (button =>
      {
        if (!button.Selected)
          return;
        this.RightFilterTypeIndex = 3;
        this.SetRightFilter(new MyInventoryOwnerTypeEnum?(MyInventoryOwnerTypeEnum.Storage));
      });
      this.m_rightFilterSystemButton.SelectedChanged += (Action<MyGuiControlRadioButton>) (button =>
      {
        if (!button.Selected)
          return;
        this.RightFilterTypeIndex = 4;
        this.SetRightFilter(new MyInventoryOwnerTypeEnum?(MyInventoryOwnerTypeEnum.System));
      });
      this.m_rightFilterShipButton.SelectedChanged += (Action<MyGuiControlRadioButton>) (button =>
      {
        if (!button.Selected)
          return;
        this.RightFilterTypeIndex = 2;
        this.SetRightFilter(new MyInventoryOwnerTypeEnum?());
      });
      this.m_leftFilterGroup.SelectByIndex(this.LeftFilterTypeIndex);
      this.m_rightFilterGroup.SelectByIndex(this.RightFilterTypeIndex);
      if (this.m_interactedAsEntity == null)
      {
        MyTerminalInventoryController.m_persistentRadioSelectionLeft = 0;
        this.m_leftGridButton.Enabled = false;
        this.m_leftGridButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowConnectedDisabled);
        this.m_rightGridButton.Enabled = false;
        this.m_rightGridButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowConnectedDisabled);
      }
      this.RefreshSelectedInventoryItem();
    }

    private void ParentsFocusChanged(MyGuiControlBase prev, MyGuiControlBase next)
    {
      this.LeftFocusChangeBorder();
      this.RightFocusChangeBorder();
    }

    private void selectedToProductionButton_ButtonClicked(MyGuiControlButton obj)
    {
      if (!this.m_selectedInventoryItem.HasValue || this.m_interactedAsEntity == null)
        return;
      Queue<MyTerminalInventoryController.QueueComponent> queuedComponents = new Queue<MyTerminalInventoryController.QueueComponent>();
      int num = 1;
      if (MyInput.Static.IsJoystickLastUsed)
        num = !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED) ? (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED) ? 1 : 100) : (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED) ? 10 : 1000);
      else if (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed())
        num = 100;
      else if (MyInput.Static.IsAnyCtrlKeyPressed())
        num = 10;
      queuedComponents.Enqueue(new MyTerminalInventoryController.QueueComponent()
      {
        Id = this.m_selectedInventoryItem.Value.Content.GetId(),
        Count = num
      });
      MyTerminalInventoryController.AddComponentsToProduction(queuedComponents, this.m_interactedAsEntity);
    }

    private static bool FilterAssemblerFunc(Sandbox.ModAPI.IMyTerminalBlock block)
    {
      if (!(block is Sandbox.ModAPI.IMyAssembler) || block != null && !block.IsWorking || (!(block is MyEntity myEntity) || !myEntity.HasInventory))
        return false;
      switch (((VRage.Game.ModAPI.IMyCubeBlock) block).GetUserRelationToOwner(MySession.Static.LocalPlayerId))
      {
        case MyRelationsBetweenPlayerAndBlock.NoOwnership:
        case MyRelationsBetweenPlayerAndBlock.Owner:
        case MyRelationsBetweenPlayerAndBlock.FactionShare:
          return true;
        default:
          return false;
      }
    }

    private static int SortAssemberBlockFunc(Sandbox.ModAPI.IMyTerminalBlock x, Sandbox.ModAPI.IMyTerminalBlock y)
    {
      if (!(x.SlimBlock?.BlockDefinition is MyAssemblerDefinition blockDefinition))
        return 0;
      return !(y.SlimBlock?.BlockDefinition is MyAssemblerDefinition blockDefinition) ? 1 : blockDefinition.AssemblySpeed.CompareTo(blockDefinition.AssemblySpeed);
    }

    private void AddToProductionButton_ButtonClicked(MyGuiControlButton obj)
    {
      if (MyInput.Static.IsJoystickLastUsed)
      {
        MyTerminalInventoryController.MyBuildPlannerAction buildPlannerAction = !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED) ? MyTerminalInventoryController.MyBuildPlannerAction.AddProduction1 : MyTerminalInventoryController.MyBuildPlannerAction.AddProduction10;
        int num1 = 0;
        switch (buildPlannerAction)
        {
          case MyTerminalInventoryController.MyBuildPlannerAction.AddProduction1:
            int num2 = 1;
            if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
            {
              num1 = MyTerminalInventoryController.AddComponentsToProduction(this.m_interactedAsEntity, new int?(num2));
              break;
            }
            MyGuiScreenGamePlay.ShowEmptyBuildPlannerNotification();
            break;
          case MyTerminalInventoryController.MyBuildPlannerAction.AddProduction10:
            int num3 = 10;
            if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
            {
              num1 = MyTerminalInventoryController.AddComponentsToProduction(this.m_interactedAsEntity, new int?(num3));
              break;
            }
            MyGuiScreenGamePlay.ShowEmptyBuildPlannerNotification();
            break;
        }
        if (num1 <= 0)
          return;
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(string.Format(MyTexts.GetString(MySpaceTexts.NotificationPutToProductionFailed), (object) num1)), messageCaption: messageCaption));
      }
      else
      {
        int num = 1;
        if (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed())
          num = 100;
        else if (MyInput.Static.IsAnyCtrlKeyPressed())
          num = 10;
        Queue<MyTerminalInventoryController.QueueComponent> queuedComponents = new Queue<MyTerminalInventoryController.QueueComponent>();
        for (; num > 0; --num)
        {
          foreach (MyIdentity.BuildPlanItem buildPlanItem in (IEnumerable<MyIdentity.BuildPlanItem>) MySession.Static.LocalCharacter.BuildPlanner)
          {
            foreach (MyIdentity.BuildPlanItem.Component component in buildPlanItem.Components)
            {
              if (component.ComponentDefinition != null)
                queuedComponents.Enqueue(new MyTerminalInventoryController.QueueComponent()
                {
                  Id = component.ComponentDefinition.Id,
                  Count = component.Count
                });
            }
          }
        }
        int production = MyTerminalInventoryController.AddComponentsToProduction(queuedComponents, this.m_interactedAsEntity);
        if (production <= 0)
          return;
        StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(string.Format(MyTexts.GetString(MySpaceTexts.NotificationPutToProductionFailed), (object) production)), messageCaption: messageCaption));
      }
    }

    public static int AddComponentsToProduction(MyEntity interactedEntity, int? persistentMultiple)
    {
      Queue<MyTerminalInventoryController.QueueComponent> queuedComponents = new Queue<MyTerminalInventoryController.QueueComponent>();
      foreach (MyIdentity.BuildPlanItem buildPlanItem in (IEnumerable<MyIdentity.BuildPlanItem>) MySession.Static.LocalCharacter.BuildPlanner)
      {
        for (int index = persistentMultiple.HasValue ? persistentMultiple.Value : 1; index > 0; --index)
        {
          foreach (MyIdentity.BuildPlanItem.Component component in buildPlanItem.Components)
          {
            if (component.ComponentDefinition != null)
              queuedComponents.Enqueue(new MyTerminalInventoryController.QueueComponent()
              {
                Id = component.ComponentDefinition.Id,
                Count = component.Count
              });
          }
        }
      }
      return MyTerminalInventoryController.AddComponentsToProduction(queuedComponents, interactedEntity);
    }

    private static int AddComponentsToProduction(
      Queue<MyTerminalInventoryController.QueueComponent> queuedComponents,
      MyEntity interactedEntity)
    {
      if (interactedEntity == null || !(interactedEntity.Parent is MyCubeGrid parent))
        return 0;
      MyGridTerminalSystem terminalSystem = parent.GridSystems.TerminalSystem;
      int num = 0;
      List<Sandbox.ModAPI.IMyTerminalBlock> source = new List<Sandbox.ModAPI.IMyTerminalBlock>();
      List<Sandbox.ModAPI.IMyTerminalBlock> blocks = source;
      Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect = new Func<Sandbox.ModAPI.IMyTerminalBlock, bool>(MyTerminalInventoryController.FilterAssemblerFunc);
      ((Sandbox.ModAPI.IMyGridTerminalSystem) terminalSystem).GetBlocksOfType<Sandbox.ModAPI.IMyTerminalBlock>(blocks, collect);
      List<Sandbox.ModAPI.IMyAssembler> list = source.Cast<Sandbox.ModAPI.IMyAssembler>().ToList<Sandbox.ModAPI.IMyAssembler>();
      list.SortNoAlloc<Sandbox.ModAPI.IMyAssembler>(new Comparison<Sandbox.ModAPI.IMyAssembler>(MyTerminalInventoryController.SortAssemberBlockFunc));
      int count = queuedComponents.Count;
      while (queuedComponents.Count > 0)
      {
        MyTerminalInventoryController.QueueComponent queueComponent = queuedComponents.Dequeue();
        bool flag = false;
        foreach (Sandbox.ModAPI.IMyAssembler myAssembler in list)
        {
          if (myAssembler.Mode != MyAssemblerMode.Disassembly && myAssembler.UseConveyorSystem && !myAssembler.CooperativeMode)
          {
            Sandbox.ModAPI.IMyProductionBlock myProductionBlock = (Sandbox.ModAPI.IMyProductionBlock) myAssembler;
            MyBlueprintDefinitionBase definitionByResultId = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(queueComponent.Id);
            if (definitionByResultId != null && myProductionBlock.CanUseBlueprint((MyDefinitionBase) definitionByResultId))
            {
              myProductionBlock.AddQueueItem((MyDefinitionBase) definitionByResultId, (MyFixedPoint) queueComponent.Count);
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          ++num;
      }
      return num;
    }

    private MyInventory[] GetSourceInventories()
    {
      ObservableCollection<MyGuiControlBase>.Enumerator enumerator = this.m_leftOwnersControl.Controls.GetEnumerator();
      List<MyInventory> myInventoryList = new List<MyInventory>();
      while (enumerator.MoveNext())
      {
        if (enumerator.Current.Visible && enumerator.Current is MyGuiControlInventoryOwner current && current.Enabled)
        {
          MyEntity inventoryOwner = current.InventoryOwner;
          for (int index = 0; index < inventoryOwner.InventoryCount; ++index)
          {
            MyInventory inventory = MyEntityExtensions.GetInventory(inventoryOwner, index);
            if (inventory != null)
              myInventoryList.Add(inventory);
          }
        }
      }
      return myInventoryList.ToArray();
    }

    private void DepositAllButton_ButtonClicked(MyGuiControlButton obj)
    {
      int num = MyTerminalInventoryController.depositAllFrom(this.GetSourceInventories(), this.m_interactedAsEntity, new Action<MyEntity, MyDefinitionId, List<MyInventory>, MyEntity, bool>(this.GetAvailableInventories));
      if (num <= 0)
        return;
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(string.Format(MyTexts.GetString(MySpaceTexts.NotificationDepositFailed), (object) num)), messageCaption: messageCaption));
    }

    public static int DepositAll(MyInventory srcInventory, MyEntity interactedEntity) => MyTerminalInventoryController.depositAllFrom(new MyInventory[1]
    {
      srcInventory
    }, interactedEntity, new Action<MyEntity, MyDefinitionId, List<MyInventory>, MyEntity, bool>(MyTerminalInventoryController.GetAvailableInventoriesStatic));

    private static bool ShouldStoreMagazine(MyObjectBuilder_AmmoMagazine magazine) => magazine.SubtypeName == "NATO_25x184mm";

    private static int depositAllFrom(
      MyInventory[] srcInventories,
      MyEntity interactedEntity,
      Action<MyEntity, MyDefinitionId, List<MyInventory>, MyEntity, bool> getInventoriesMethod)
    {
      int num = 0;
      Dictionary<MyInventory, Dictionary<MyDefinitionId, MyFixedPoint>> dictionary1 = new Dictionary<MyInventory, Dictionary<MyDefinitionId, MyFixedPoint>>();
      foreach (MyInventory srcInventory in srcInventories)
      {
        foreach (MyPhysicalInventoryItem physicalInventoryItem in srcInventory.GetItems())
        {
          if (physicalInventoryItem.Content is MyObjectBuilder_Ore || physicalInventoryItem.Content is MyObjectBuilder_Ingot || physicalInventoryItem.Content is MyObjectBuilder_Component || physicalInventoryItem.Content is MyObjectBuilder_AmmoMagazine content && MyTerminalInventoryController.ShouldStoreMagazine(content))
          {
            if (!dictionary1.ContainsKey(srcInventory))
              dictionary1[srcInventory] = new Dictionary<MyDefinitionId, MyFixedPoint>();
            if (!dictionary1[srcInventory].ContainsKey(physicalInventoryItem.Content.GetId()))
              dictionary1[srcInventory][physicalInventoryItem.Content.GetId()] = (MyFixedPoint) 0;
            dictionary1[srcInventory][physicalInventoryItem.Content.GetId()] += physicalInventoryItem.Amount;
          }
        }
      }
      foreach (MyInventory srcInventory in srcInventories)
      {
        if (dictionary1.ContainsKey(srcInventory))
        {
          Dictionary<MyInventory, MyFixedPoint> dictionary2 = new Dictionary<MyInventory, MyFixedPoint>();
          List<MyInventory> myInventoryList = new List<MyInventory>();
          foreach (MyDefinitionId myDefinitionId in dictionary1[srcInventory].Keys.ToList<MyDefinitionId>())
          {
            getInventoriesMethod(srcInventory.Owner, myDefinitionId, myInventoryList, interactedEntity, false);
            if (myInventoryList.Count == 0)
            {
              ++num;
            }
            else
            {
              MyFixedPoint myFixedPoint1 = (MyFixedPoint) 0;
              foreach (MyInventory myInventory in myInventoryList)
              {
                if (srcInventory != myInventory)
                {
                  if (!dictionary2.ContainsKey(myInventory))
                    dictionary2.Add(myInventory, myInventory.MaxVolume - myInventory.CurrentVolume);
                  MyFixedPoint myFixedPoint2 = dictionary2[myInventory];
                  if (!(myFixedPoint2 == (MyFixedPoint) 0))
                  {
                    float itemVolume;
                    MyInventory.GetItemVolumeAndMass(myDefinitionId, out float _, out itemVolume);
                    MyFixedPoint myFixedPoint3 = dictionary1[srcInventory][myDefinitionId] * itemVolume;
                    MyFixedPoint myFixedPoint4 = myFixedPoint3;
                    MyFixedPoint myFixedPoint5 = dictionary1[srcInventory][myDefinitionId];
                    if (myFixedPoint2 < myFixedPoint3)
                    {
                      myFixedPoint4 = myFixedPoint2;
                      MyInventoryItemAdapter inventoryItemAdapter = MyInventoryItemAdapter.Static;
                      inventoryItemAdapter.Adapt(myDefinitionId);
                      if (inventoryItemAdapter.HasIntegralAmounts)
                      {
                        myFixedPoint5 = MyFixedPoint.Floor((MyFixedPoint) (Math.Round((double) myFixedPoint4 * 1000.0 / (double) itemVolume) / 1000.0));
                      }
                      else
                      {
                        MyFixedPoint myFixedPoint6 = (MyFixedPoint) ((double) myFixedPoint4 / (double) itemVolume);
                        if ((double) Math.Abs((float) myFixedPoint6 - (float) myFixedPoint5) > 1.0 / 1000.0)
                          myFixedPoint5 = myFixedPoint6;
                      }
                    }
                    if (myFixedPoint5 > (MyFixedPoint) 0)
                    {
                      MyInventory.TransferByPlanner(srcInventory, myInventory, (SerializableDefinitionId) myDefinitionId, amount: new MyFixedPoint?(myFixedPoint5));
                      dictionary1[srcInventory][myDefinitionId] -= myFixedPoint5;
                      dictionary2[myInventory] -= myFixedPoint4;
                    }
                    myFixedPoint1 += myFixedPoint5;
                  }
                }
              }
              if (myFixedPoint1 == (MyFixedPoint) 0)
                ++num;
            }
          }
        }
      }
      return num;
    }

    private void WithdrawButton_ButtonClicked(MyGuiControlButton obj)
    {
      if (MyInput.Static.IsJoystickLastUsed)
      {
        MyTerminalInventoryController.MyBuildPlannerAction buildPlannerAction = !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED) ? (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED) ? MyTerminalInventoryController.MyBuildPlannerAction.DefaultWithdraw : MyTerminalInventoryController.MyBuildPlannerAction.WithdrawKeep10) : MyTerminalInventoryController.MyBuildPlannerAction.WithdrawKeep1;
        HashSet<MyInventory> usedTargetInventories = new HashSet<MyInventory>();
        List<MyIdentity.BuildPlanItem.Component> missingComponents = (List<MyIdentity.BuildPlanItem.Component>) null;
        switch (buildPlannerAction)
        {
          case MyTerminalInventoryController.MyBuildPlannerAction.DefaultWithdraw:
            int? multiplier1 = new int?();
            missingComponents = MyTerminalInventoryController.ProcessWithdraw(this.m_interactedAsEntity, MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter), ref usedTargetInventories, multiplier1);
            MySession.Static.LocalCharacter.CleanFinishedBuildPlanner();
            break;
          case MyTerminalInventoryController.MyBuildPlannerAction.WithdrawKeep1:
            int? multiplier2 = new int?(1);
            missingComponents = MyTerminalInventoryController.ProcessWithdraw(this.m_interactedAsEntity, MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter), ref usedTargetInventories, multiplier2);
            break;
          case MyTerminalInventoryController.MyBuildPlannerAction.WithdrawKeep10:
            int? multiplier3 = new int?(10);
            missingComponents = MyTerminalInventoryController.ProcessWithdraw(this.m_interactedAsEntity, MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter), ref usedTargetInventories, multiplier3);
            break;
        }
        if (missingComponents != null && missingComponents.Count > 0)
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(MyTerminalInventoryController.GetMissingComponentsText(missingComponents)), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo)));
        if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
        {
          this.m_withdrawButton.Enabled = true;
          this.m_addToProductionButton.Enabled = true;
        }
        else
        {
          if (this.m_withdrawButton.HasFocus || this.m_addToProductionButton.HasFocus)
          {
            IMyGuiControlsOwner guiControlsOwner = (IMyGuiControlsOwner) obj;
            while (guiControlsOwner.Owner != null)
              guiControlsOwner = guiControlsOwner.Owner;
            if (guiControlsOwner is MyGuiScreenBase screen)
              this.RefocusInventories(screen, MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter), this.m_interactedAsEntity, usedTargetInventories);
          }
          this.m_withdrawButton.Enabled = false;
          this.m_addToProductionButton.Enabled = false;
        }
      }
      else
      {
        MyInventory[] sourceInventories = this.GetSourceInventories();
        int? nullable = new int?();
        if (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed())
          nullable = new int?(100);
        else if (MyInput.Static.IsAnyCtrlKeyPressed())
          nullable = new int?(10);
        else if (MyInput.Static.IsAnyShiftKeyPressed())
          nullable = new int?(1);
        HashSet<MyInventory> actualSourceInventories = new HashSet<MyInventory>();
        Action<MyEntity, MyDefinitionId, List<MyInventory>, MyEntity, bool> getInventoriesMethod = new Action<MyEntity, MyDefinitionId, List<MyInventory>, MyEntity, bool>(this.GetAvailableInventories);
        MyEntity interactedAsEntity = this.m_interactedAsEntity;
        ref HashSet<MyInventory> local = ref actualSourceInventories;
        int? persistentMultiple = nullable;
        List<MyIdentity.BuildPlanItem.Component> inventories = MyTerminalInventoryController.WithdrawToInventories(sourceInventories, getInventoriesMethod, interactedAsEntity, ref local, persistentMultiple);
        if (nullable.HasValue)
          return;
        if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
        {
          this.m_withdrawButton.Enabled = true;
          this.m_addToProductionButton.Enabled = true;
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder(MyTerminalInventoryController.GetMissingComponentsText(inventories)), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo)));
        }
        else
        {
          if (this.m_withdrawButton.HasFocus || this.m_addToProductionButton.HasFocus)
          {
            IMyGuiControlsOwner guiControlsOwner = (IMyGuiControlsOwner) obj;
            while (guiControlsOwner.Owner != null)
              guiControlsOwner = guiControlsOwner.Owner;
            if (guiControlsOwner is MyGuiScreenBase screen)
              this.RefocusInventories(screen, MyEntityExtensions.GetInventory(MySession.Static.LocalCharacter), this.m_interactedAsEntity, actualSourceInventories);
          }
          this.m_withdrawButton.Enabled = false;
          this.m_addToProductionButton.Enabled = false;
        }
      }
    }

    private void RefocusInventories(
      MyGuiScreenBase screen,
      MyInventory characterInventory,
      MyEntity interactedEntity,
      HashSet<MyInventory> actualSourceInventories)
    {
      if (screen == null)
        return;
      HashSet<MyInventory> sourceInventories = new HashSet<MyInventory>();
      for (int index = 0; index < interactedEntity.InventoryCount; ++index)
        sourceInventories.Add(MyEntityExtensions.GetInventory(interactedEntity, index));
      MyGuiControlGrid interactedEntityGrid = (MyGuiControlGrid) null;
      MyGuiControlGrid actualInteractedEntityGrid = (MyGuiControlGrid) null;
      if (SiftThroughInventories(this.m_leftOwnersControl) || SiftThroughInventories(this.m_rightOwnersControl))
        return;
      if (interactedEntityGrid != null)
        screen.FocusedControl = (MyGuiControlBase) interactedEntityGrid;
      else if (interactedEntityGrid != null)
        screen.FocusedControl = (MyGuiControlBase) actualInteractedEntityGrid;
      else
        this.ForceSelectFirstInList(this.m_leftOwnersControl);

      bool SiftThroughInventories(MyGuiControlList list)
      {
        foreach (MyGuiControlBase control in list.Controls)
        {
          if (control is MyGuiControlInventoryOwner controlInventoryOwner && controlInventoryOwner.Visible)
          {
            foreach (MyGuiControlGrid contentGrid in controlInventoryOwner.ContentGrids)
            {
              if (contentGrid.UserData == characterInventory)
              {
                screen.FocusedControl = (MyGuiControlBase) contentGrid;
                return true;
              }
              if (((IEnumerable<object>) actualSourceInventories).Contains<object>(contentGrid.UserData))
              {
                actualInteractedEntityGrid = contentGrid;
                if (((IEnumerable<object>) sourceInventories).Contains<object>(contentGrid.UserData))
                  interactedEntityGrid = contentGrid;
              }
            }
          }
        }
        return false;
      }
    }

    private static List<MyIdentity.BuildPlanItem.Component> ProcessWithdraw(
      MyEntity owner,
      MyInventory inventory,
      ref HashSet<MyInventory> usedTargetInventories,
      int? multiplier)
    {
      if (MySession.Static.LocalCharacter.BuildPlanner.Count == 0)
      {
        MyGuiScreenGamePlay.ShowEmptyBuildPlannerNotification();
        return (List<MyIdentity.BuildPlanItem.Component>) null;
      }
      List<MyIdentity.BuildPlanItem.Component> missingComponents = MyTerminalInventoryController.Withdraw(owner, inventory, ref usedTargetInventories, multiplier);
      if (missingComponents.Count == 0)
      {
        MyHud.Notifications.Add(MyNotificationSingletons.WithdrawSuccessful);
      }
      else
      {
        string missingComponentsText = MyTerminalInventoryController.GetMissingComponentsText(missingComponents);
        MyHud.Notifications.Add(MyNotificationSingletons.WithdrawFailed).SetTextFormatArguments((object) missingComponentsText);
      }
      return missingComponents;
    }

    public static string GetMissingComponentsText(
      List<MyIdentity.BuildPlanItem.Component> missingComponents)
    {
      string str;
      switch (missingComponents.Count)
      {
        case 0:
          return string.Empty;
        case 1:
          str = string.Format(MyTexts.Get(MySpaceTexts.NotificationWithdrawFailed1).ToString(), (object) missingComponents[0].Count, (object) missingComponents[0].ComponentDefinition.DisplayNameText);
          break;
        case 2:
          str = string.Format(MyTexts.Get(MySpaceTexts.NotificationWithdrawFailed2).ToString(), (object) missingComponents[0].Count, (object) missingComponents[0].ComponentDefinition.DisplayNameText, (object) missingComponents[1].Count, (object) missingComponents[1].ComponentDefinition.DisplayNameText);
          break;
        case 3:
          str = string.Format(MyTexts.Get(MySpaceTexts.NotificationWithdrawFailed3).ToString(), (object) missingComponents[0].Count, (object) missingComponents[0].ComponentDefinition.DisplayNameText, (object) missingComponents[1].Count, (object) missingComponents[1].ComponentDefinition.DisplayNameText, (object) missingComponents[2].Count, (object) missingComponents[2].ComponentDefinition.DisplayNameText);
          break;
        default:
          int num = 0;
          for (int index = 3; index < missingComponents.Count; ++index)
            num += missingComponents[index].Count;
          str = string.Format(MyTexts.Get(MySpaceTexts.NotificationWithdrawFailed4More).ToString(), (object) missingComponents[0].Count, (object) missingComponents[0].ComponentDefinition.DisplayNameText, (object) missingComponents[1].Count, (object) missingComponents[1].ComponentDefinition.DisplayNameText, (object) missingComponents[2].Count, (object) missingComponents[2].ComponentDefinition.DisplayNameText, (object) num);
          break;
      }
      return str;
    }

    public static List<MyIdentity.BuildPlanItem.Component> Withdraw(
      MyEntity interactedEntity,
      MyInventory toInventory,
      ref HashSet<MyInventory> usedTargetInventories,
      int? persistentMultiple)
    {
      return MyTerminalInventoryController.WithdrawToInventories(new MyInventory[1]
      {
        toInventory
      }, new Action<MyEntity, MyDefinitionId, List<MyInventory>, MyEntity, bool>(MyTerminalInventoryController.GetAvailableInventoriesStatic), interactedEntity, ref usedTargetInventories, persistentMultiple);
    }

    private static void GetAvailableInventoriesStatic(
      MyEntity inventoryOwner,
      MyDefinitionId id,
      List<MyInventory> availableInventories,
      MyEntity interactedEntity,
      bool requireAmount)
    {
      availableInventories.Clear();
      List<MyEntity> outputInventories = new List<MyEntity>();
      MyCubeGrid Node = interactedEntity is MyCubeBlock myCubeBlock ? myCubeBlock.CubeGrid : (MyCubeGrid) null;
      MyInventoryBagEntity inventoryBagEntity = interactedEntity as MyInventoryBagEntity;
      if (Node != null)
      {
        foreach (MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node node in MyCubeGridGroups.Static.Mechanical.GetGroup(Node).Nodes)
          MyTerminalInventoryController.GetGridInventories(node.NodeData, outputInventories, interactedEntity, MySession.Static.LocalPlayerId);
      }
      if (inventoryBagEntity != null)
        outputInventories.Add((MyEntity) inventoryBagEntity);
      foreach (MyEntity thisEntity in outputInventories)
      {
        IMyConveyorEndpointBlock conveyorEndpointBlock1 = thisEntity as IMyConveyorEndpointBlock;
        IMyConveyorEndpointBlock conveyorEndpointBlock2 = interactedEntity as IMyConveyorEndpointBlock;
        if (conveyorEndpointBlock1 != null && conveyorEndpointBlock2 != null && (!(conveyorEndpointBlock2 is VRage.Game.ModAPI.Ingame.IMyCubeBlock) || ((VRage.Game.ModAPI.Ingame.IMyCubeBlock) conveyorEndpointBlock2).IsFunctional) && ((!(conveyorEndpointBlock1 is VRage.Game.ModAPI.Ingame.IMyCubeBlock) || ((VRage.Game.ModAPI.Ingame.IMyCubeBlock) conveyorEndpointBlock1).IsFunctional) && (conveyorEndpointBlock1 == conveyorEndpointBlock2 || MyGridConveyorSystem.Reachable(conveyorEndpointBlock1.ConveyorEndpoint, conveyorEndpointBlock2.ConveyorEndpoint, MySession.Static.LocalPlayerId, id, new Predicate<IMyConveyorEndpoint>(MyTerminalInventoryController.EndpointPredicateStatic)))))
        {
          for (int index = 0; index < thisEntity.InventoryCount; ++index)
          {
            MyInventory inventory = MyEntityExtensions.GetInventory(thisEntity, index);
            if (inventory.CheckConstraint(id) && (!requireAmount || inventory.GetItemAmount(id, MyItemFlags.None, false) > (MyFixedPoint) 0))
              availableInventories.Add(inventory);
          }
        }
      }
    }

    private static List<MyIdentity.BuildPlanItem.Component> WithdrawToInventories(
      MyInventory[] toInventories,
      Action<MyEntity, MyDefinitionId, List<MyInventory>, MyEntity, bool> getInventoriesMethod,
      MyEntity interactedEntity,
      ref HashSet<MyInventory> usedTargetInventories,
      int? persistentMultiple = null)
    {
      Dictionary<MyInventory, Dictionary<MyDefinitionId, int>> dictionary = new Dictionary<MyInventory, Dictionary<MyDefinitionId, int>>();
      List<MyInventory> myInventoryList = new List<MyInventory>();
      IReadOnlyList<MyIdentity.BuildPlanItem> buildPlanItemList1 = MySession.Static.LocalCharacter.BuildPlanner;
      if (persistentMultiple.HasValue)
      {
        List<MyIdentity.BuildPlanItem> buildPlanItemList2 = new List<MyIdentity.BuildPlanItem>();
        for (int index = persistentMultiple.Value; index > 0; --index)
        {
          foreach (MyIdentity.BuildPlanItem buildPlanItem1 in (IEnumerable<MyIdentity.BuildPlanItem>) MySession.Static.LocalCharacter.BuildPlanner)
          {
            MyIdentity.BuildPlanItem buildPlanItem2 = buildPlanItem1.Clone();
            buildPlanItemList2.Add(buildPlanItem2);
          }
        }
        buildPlanItemList1 = (IReadOnlyList<MyIdentity.BuildPlanItem>) buildPlanItemList2;
      }
      foreach (MyInventory toInventory in toInventories)
      {
        MyFixedPoint myFixedPoint1 = toInventory.MaxVolume - toInventory.CurrentVolume;
        foreach (MyIdentity.BuildPlanItem buildPlanItem in (IEnumerable<MyIdentity.BuildPlanItem>) buildPlanItemList1)
        {
          foreach (MyIdentity.BuildPlanItem.Component component in buildPlanItem.Components)
          {
            if (component.ComponentDefinition != null && (double) component.ComponentDefinition.Volume > 0.0 && toInventory.CheckConstraint(component.ComponentDefinition.Id))
            {
              getInventoriesMethod(toInventory.Owner, component.ComponentDefinition.Id, myInventoryList, interactedEntity, true);
              foreach (MyInventory myInventory in myInventoryList)
              {
                MyFixedPoint itemAmount = myInventory.GetItemAmount(component.ComponentDefinition.Id, MyItemFlags.None, false);
                if (!Sync.IsServer)
                {
                  if (dictionary.ContainsKey(myInventory) && dictionary[myInventory].ContainsKey(component.ComponentDefinition.Id))
                  {
                    int num = dictionary[myInventory][component.ComponentDefinition.Id];
                    itemAmount -= (MyFixedPoint) num;
                  }
                  if (itemAmount == (MyFixedPoint) 0)
                    continue;
                }
                MyFixedPoint myFixedPoint2 = itemAmount;
                if (itemAmount > (MyFixedPoint) component.Count)
                  myFixedPoint2 = (MyFixedPoint) component.Count;
                float num1 = (float) myFixedPoint2 * component.ComponentDefinition.Volume;
                if ((double) num1 > (double) (float) myFixedPoint1)
                {
                  num1 = (float) myFixedPoint1;
                  myFixedPoint2 = (MyFixedPoint) (int) ((double) num1 / (double) component.ComponentDefinition.Volume);
                }
                if (!Sync.IsServer)
                {
                  if (!dictionary.ContainsKey(myInventory))
                    dictionary.Add(myInventory, new Dictionary<MyDefinitionId, int>());
                  if (!dictionary[myInventory].ContainsKey(component.ComponentDefinition.Id))
                    dictionary[myInventory].Add(component.ComponentDefinition.Id, 0);
                  dictionary[myInventory][component.ComponentDefinition.Id] += (int) myFixedPoint2;
                }
                MyInventory.TransferByPlanner(myInventory, toInventory, (SerializableDefinitionId) component.ComponentDefinition.Id, amount: new MyFixedPoint?(myFixedPoint2));
                if (usedTargetInventories != null && !usedTargetInventories.Contains(myInventory))
                  usedTargetInventories.Add(myInventory);
                myFixedPoint1 -= (MyFixedPoint) num1;
                buildPlanItem.IsInProgress = true;
                component.Count -= (int) myFixedPoint2;
                break;
              }
            }
          }
          buildPlanItem.Components.RemoveAll((Predicate<MyIdentity.BuildPlanItem.Component>) (x => x.Count == 0));
        }
        if (!persistentMultiple.HasValue)
          MySession.Static.LocalCharacter.CleanFinishedBuildPlanner();
      }
      List<MyIdentity.BuildPlanItem.Component> componentList = new List<MyIdentity.BuildPlanItem.Component>();
      foreach (MyIdentity.BuildPlanItem buildPlanItem in (IEnumerable<MyIdentity.BuildPlanItem>) buildPlanItemList1)
      {
        foreach (MyIdentity.BuildPlanItem.Component component in buildPlanItem.Components)
          componentList.Add(component);
      }
      return componentList;
    }

    private void GetAvailableInventories(
      MyEntity inventoryOwner,
      MyDefinitionId id,
      List<MyInventory> availableInventories,
      MyEntity interactedEntity,
      bool requireAmount)
    {
      ObservableCollection<MyGuiControlBase>.Enumerator enumerator = this.m_rightOwnersControl.Controls.GetEnumerator();
      availableInventories.Clear();
      while (enumerator.MoveNext())
      {
        if (enumerator.Current.Visible && enumerator.Current is MyGuiControlInventoryOwner current && current.Enabled)
        {
          if ((inventoryOwner == this.m_userAsOwner || inventoryOwner == this.m_interactedAsOwner ? (current.InventoryOwner == this.m_userAsOwner ? 1 : (current.InventoryOwner == this.m_interactedAsOwner ? 1 : 0)) : 0) == 0)
          {
            bool flag1 = inventoryOwner is MyCharacter;
            bool flag2 = current.InventoryOwner is MyCharacter;
            IMyConveyorEndpointBlock conveyorEndpointBlock1 = inventoryOwner == null ? (IMyConveyorEndpointBlock) null : (flag1 ? this.m_interactedAsOwner : inventoryOwner) as IMyConveyorEndpointBlock;
            IMyConveyorEndpointBlock conveyorEndpointBlock2 = current.InventoryOwner == null ? (IMyConveyorEndpointBlock) null : (flag2 ? this.m_interactedAsOwner : current.InventoryOwner) as IMyConveyorEndpointBlock;
            if (conveyorEndpointBlock1 != null)
            {
              if (conveyorEndpointBlock2 != null)
              {
                try
                {
                  MyGridConveyorSystem.AppendReachableEndpoints(conveyorEndpointBlock1.ConveyorEndpoint, MySession.Static.LocalPlayerId, this.m_reachableInventoryOwners, id, this.m_endpointPredicate);
                  if (!this.m_reachableInventoryOwners.Contains(conveyorEndpointBlock2.ConveyorEndpoint))
                    continue;
                }
                finally
                {
                  this.m_reachableInventoryOwners.Clear();
                }
                if (!MyGridConveyorSystem.Reachable(conveyorEndpointBlock1.ConveyorEndpoint, conveyorEndpointBlock2.ConveyorEndpoint))
                  continue;
              }
              else
                continue;
            }
            else
              continue;
          }
          MyEntity inventoryOwner1 = current.InventoryOwner;
          for (int index = 0; index < inventoryOwner1.InventoryCount; ++index)
          {
            MyInventory inventory = MyEntityExtensions.GetInventory(inventoryOwner1, index);
            if (inventory.CheckConstraint(id) && (!requireAmount || inventory.GetItemAmount(id, MyItemFlags.None, false) > (MyFixedPoint) 0))
              availableInventories.Add(inventory);
          }
        }
      }
    }

    public void Close()
    {
      foreach (MyGridConveyorSystem registeredConveyorSystem in this.m_registeredConveyorSystems)
      {
        registeredConveyorSystem.BlockAdded -= new Action<MyCubeBlock>(this.ConveyorSystem_BlockAdded);
        registeredConveyorSystem.BlockRemoved -= new Action<MyCubeBlock>(this.ConveyorSystem_BlockRemoved);
      }
      this.m_registeredConveyorSystems.Clear();
      foreach (MyGridConveyorSystem mechanicalSystem in this.m_registeredConveyorMechanicalSystems)
      {
        mechanicalSystem.BlockAdded -= new Action<MyCubeBlock>(this.ConveyorSystemMechanical_BlockAdded);
        mechanicalSystem.BlockRemoved -= new Action<MyCubeBlock>(this.ConveyorSystemMechanical_BlockRemoved);
      }
      this.m_registeredConveyorMechanicalSystems.Clear();
      this.m_leftTypeGroup.Clear();
      this.m_leftFilterGroup.Clear();
      this.m_rightTypeGroup.Clear();
      this.m_rightFilterGroup.Clear();
      this.m_controlsDisabledWhileDragged.Clear();
      this.m_leftOwnersControl = (MyGuiControlList) null;
      this.m_leftSuitButton = (MyGuiControlRadioButton) null;
      this.m_leftGridButton = (MyGuiControlRadioButton) null;
      this.m_leftFilterStorageButton = (MyGuiControlRadioButton) null;
      this.m_leftFilterSystemButton = (MyGuiControlRadioButton) null;
      this.m_leftFilterEnergyButton = (MyGuiControlRadioButton) null;
      this.m_leftFilterAllButton = (MyGuiControlRadioButton) null;
      this.m_leftFilterShipButton = (MyGuiControlRadioButton) null;
      this.m_rightOwnersControl = (MyGuiControlList) null;
      this.m_rightSuitButton = (MyGuiControlRadioButton) null;
      this.m_rightGridButton = (MyGuiControlRadioButton) null;
      this.m_rightFilterShipButton = (MyGuiControlRadioButton) null;
      this.m_rightFilterStorageButton = (MyGuiControlRadioButton) null;
      this.m_rightFilterSystemButton = (MyGuiControlRadioButton) null;
      this.m_rightFilterEnergyButton = (MyGuiControlRadioButton) null;
      this.m_rightFilterAllButton = (MyGuiControlRadioButton) null;
      this.m_throwOutButton = (MyGuiControlButton) null;
      this.m_dragAndDrop = (MyGuiControlGridDragAndDrop) null;
      this.m_dragAndDropInfo = (MyDragAndDropInfo) null;
      this.FocusedOwnerControl = (MyGuiControlInventoryOwner) null;
      this.FocusedGridControl = (MyGuiControlGrid) null;
      this.m_selectedInventory = (MyInventory) null;
      this.m_hideEmptyLeft.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.HideEmptyLeft_Checked);
      this.m_hideEmptyRight.IsCheckedChanged -= new Action<MyGuiControlCheckbox>(this.HideEmptyRight_Checked);
      this.m_searchBoxLeft.OnTextChanged -= new MyGuiControlSearchBox.TextChangedDelegate(this.BlockSearchLeft_TextChanged);
      this.m_searchBoxRight.OnTextChanged -= new MyGuiControlSearchBox.TextChangedDelegate(this.BlockSearchRight_TextChanged);
      this.m_hideEmptyLeft = (MyGuiControlCheckbox) null;
      this.m_hideEmptyLeftLabel = (MyGuiControlLabel) null;
      this.m_hideEmptyRight = (MyGuiControlCheckbox) null;
      this.m_hideEmptyRightLabel = (MyGuiControlLabel) null;
      this.m_searchBoxLeft = (MyGuiControlSearchBox) null;
      this.m_searchBoxRight = (MyGuiControlSearchBox) null;
    }

    public void SetSearch(string text, bool interactedSide = true)
    {
      MyGuiControlSearchBox controlSearchBox = interactedSide ? this.m_searchBoxRight : this.m_searchBoxLeft;
      if (controlSearchBox != null)
        controlSearchBox.SearchText = text;
      if (interactedSide)
        this.SetRightFilter(new MyInventoryOwnerTypeEnum?());
      else
        this.SetLeftFilter(new MyInventoryOwnerTypeEnum?());
    }

    private void StartDragging(
      MyDropHandleType dropHandlingType,
      MyGuiControlGrid gridControl,
      ref MyGuiControlGrid.EventArgs args)
    {
      this.m_dragAndDropInfo = new MyDragAndDropInfo();
      this.m_dragAndDropInfo.Grid = gridControl;
      this.m_dragAndDropInfo.ItemIndex = args.ItemIndex;
      this.DisableInvalidWhileDragging();
      MyGuiGridItem itemAt = this.m_dragAndDropInfo.Grid.GetItemAt(this.m_dragAndDropInfo.ItemIndex);
      this.m_dragAndDrop.StartDragging(dropHandlingType, args.Button, itemAt, this.m_dragAndDropInfo, false);
    }

    private void DisableInvalidWhileDragging()
    {
      MyGuiGridItem itemAt = this.m_dragAndDropInfo.Grid.GetItemAt(this.m_dragAndDropInfo.ItemIndex);
      if (itemAt == null)
        return;
      MyPhysicalInventoryItem userData1 = (MyPhysicalInventoryItem) itemAt.UserData;
      MyInventory userData2 = (MyInventory) this.m_dragAndDropInfo.Grid.UserData;
      this.DisableUnacceptingInventoryControls(userData1, this.m_leftOwnersControl);
      this.DisableUnacceptingInventoryControls(userData1, this.m_rightOwnersControl);
      this.DisableUnreachableInventoryControls(userData2, userData1, this.m_leftOwnersControl);
      this.DisableUnreachableInventoryControls(userData2, userData1, this.m_rightOwnersControl);
    }

    private void DisableUnacceptingInventoryControls(
      MyPhysicalInventoryItem item,
      MyGuiControlList list)
    {
      foreach (MyGuiControlBase visibleControl in list.Controls.GetVisibleControls())
      {
        if (visibleControl.Enabled)
        {
          MyGuiControlInventoryOwner controlInventoryOwner = (MyGuiControlInventoryOwner) visibleControl;
          MyEntity inventoryOwner = controlInventoryOwner.InventoryOwner;
          for (int index = 0; index < inventoryOwner.InventoryCount; ++index)
          {
            if (!MyEntityExtensions.GetInventory(inventoryOwner, index).CanItemsBeAdded((MyFixedPoint) 0, item.Content.GetId()))
            {
              controlInventoryOwner.ContentGrids[index].Enabled = false;
              this.m_controlsDisabledWhileDragged.Add(controlInventoryOwner.ContentGrids[index]);
            }
          }
        }
      }
    }

    private bool EndpointPredicate(IMyConveyorEndpoint endpoint) => endpoint.CubeBlock != null && endpoint.CubeBlock.HasInventory || endpoint.CubeBlock == this.m_interactedEndpointBlock;

    private static bool EndpointPredicateStatic(IMyConveyorEndpoint endpoint) => endpoint.CubeBlock != null && endpoint.CubeBlock.HasInventory;

    private void DisableUnreachableInventoryControls(
      MyInventory srcInventory,
      MyPhysicalInventoryItem item,
      MyGuiControlList list)
    {
      bool flag1 = srcInventory.Owner == this.m_userAsOwner;
      bool flag2 = srcInventory.Owner == this.m_interactedAsOwner;
      MyEntity owner = srcInventory.Owner;
      IMyConveyorEndpointBlock conveyorEndpointBlock1 = (IMyConveyorEndpointBlock) null;
      if (flag1)
        conveyorEndpointBlock1 = this.m_interactedAsEntity as IMyConveyorEndpointBlock;
      else if (owner != null)
        conveyorEndpointBlock1 = owner as IMyConveyorEndpointBlock;
      IMyConveyorEndpointBlock conveyorEndpointBlock2 = (IMyConveyorEndpointBlock) null;
      if (this.m_interactedAsEntity != null)
        conveyorEndpointBlock2 = this.m_interactedAsEntity as IMyConveyorEndpointBlock;
      if (conveyorEndpointBlock1 != null)
      {
        long localPlayerId = MySession.Static.LocalPlayerId;
        this.m_interactedEndpointBlock = conveyorEndpointBlock2;
        MyGridConveyorSystem.AppendReachableEndpoints(conveyorEndpointBlock1.ConveyorEndpoint, localPlayerId, this.m_reachableInventoryOwners, item.Content.GetId(), this.m_endpointPredicate);
      }
      foreach (MyGuiControlBase visibleControl in list.Controls.GetVisibleControls())
      {
        if (visibleControl.Enabled)
        {
          MyGuiControlInventoryOwner controlInventoryOwner = (MyGuiControlInventoryOwner) visibleControl;
          MyEntity inventoryOwner = controlInventoryOwner.InventoryOwner;
          IMyConveyorEndpoint conveyorEndpoint = (IMyConveyorEndpoint) null;
          if (inventoryOwner is IMyConveyorEndpointBlock conveyorEndpointBlock3)
            conveyorEndpoint = conveyorEndpointBlock3.ConveyorEndpoint;
          int num1 = inventoryOwner == owner ? 1 : 0;
          bool flag3 = flag1 && inventoryOwner == this.m_interactedAsOwner || flag2 && inventoryOwner == this.m_userAsOwner;
          int num2 = num1 != 0 ? 0 : (!flag3 ? 1 : 0);
          bool flag4 = !this.m_reachableInventoryOwners.Contains(conveyorEndpoint);
          bool flag5 = conveyorEndpointBlock2 != null && this.m_reachableInventoryOwners.Contains(conveyorEndpointBlock2.ConveyorEndpoint);
          bool flag6 = inventoryOwner == this.m_userAsOwner & flag5;
          int num3 = flag4 ? 1 : 0;
          if ((num2 & num3) != 0 && !flag6)
          {
            for (int index = 0; index < inventoryOwner.InventoryCount; ++index)
            {
              if (controlInventoryOwner.ContentGrids[index].Enabled)
              {
                controlInventoryOwner.ContentGrids[index].Enabled = false;
                this.m_controlsDisabledWhileDragged.Add(controlInventoryOwner.ContentGrids[index]);
              }
            }
          }
        }
      }
      this.m_reachableInventoryOwners.Clear();
    }

    private static void GetGridInventories(
      MyCubeGrid grid,
      List<MyEntity> outputInventories,
      MyEntity interactedEntity,
      long identityId)
    {
      grid?.GridSystems.ConveyorSystem.GetGridInventories(interactedEntity, outputInventories, identityId);
    }

    private void CreateInventoryControlInList(MyEntity owner, MyGuiControlList listControl)
    {
      List<MyEntity> owners = new List<MyEntity>();
      if (owner != null)
        owners.Add(owner);
      this.CreateInventoryControlsInList(owners, listControl);
    }

    private void CreateInventoryControlsInList(
      List<MyEntity> owners,
      MyGuiControlList listControl,
      MyInventoryOwnerTypeEnum? filterType = null)
    {
      if (listControl.Controls.Contains((MyGuiControlBase) this.FocusedOwnerControl))
        this.FocusedOwnerControl = (MyGuiControlInventoryOwner) null;
      List<MyGuiControlBase> myGuiControlBaseList = new List<MyGuiControlBase>();
      foreach (MyEntity owner in owners)
      {
        if (owner != null && owner.HasInventory)
        {
          if (filterType.HasValue)
          {
            int num = (int) owner.InventoryOwnerType();
            MyInventoryOwnerTypeEnum? nullable = filterType;
            int valueOrDefault = (int) nullable.GetValueOrDefault();
            if (!(num == valueOrDefault & nullable.HasValue))
              continue;
          }
          Vector4 labelColorMask = Color.White.ToVector4();
          if (owner is MyCubeBlock)
          {
            Color? gridColor = this.m_colorHelper.GetGridColor((owner as MyCubeBlock).CubeGrid);
            labelColorMask = gridColor.HasValue ? gridColor.Value.ToVector4() : Vector4.One;
          }
          MyGuiControlInventoryOwner controlInventoryOwner = new MyGuiControlInventoryOwner(owner, labelColorMask);
          controlInventoryOwner.Size = new Vector2(listControl.Size.X - 0.05f, controlInventoryOwner.Size.Y);
          controlInventoryOwner.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
          foreach (MyGuiControlGrid contentGrid in controlInventoryOwner.ContentGrids)
          {
            contentGrid.FocusChanged += new Action<MyGuiControlBase, bool>(this.grid_focusChanged);
            contentGrid.ItemSelected += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_ItemSelected);
            contentGrid.ItemDragged += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_ItemDragged);
            contentGrid.ItemDoubleClicked += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_ItemDoubleClicked);
            contentGrid.ItemClicked += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_ItemClicked);
            contentGrid.ItemAccepted += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_ItemDoubleClicked);
            contentGrid.ItemReleased += new Action<MyGuiControlGrid, MyGuiControlGrid.EventArgs>(this.grid_ItemReleased);
            contentGrid.ReleasedWithoutItem += new Action<MyGuiControlGrid>(this.grid_ReleasedWithoutItem);
            contentGrid.ItemControllerAction += new Func<MyGuiControlGrid, int, MyGridItemAction, bool, bool>(this.grid_ItemControllerAction);
          }
          controlInventoryOwner.SizeChanged += new Action<MyGuiControlBase>(this.inventoryControl_SizeChanged);
          controlInventoryOwner.InventoryContentsChanged += new Action<MyGuiControlInventoryOwner>(this.ownerControl_InventoryContentsChanged);
          if (owner is MyCubeBlock)
            controlInventoryOwner.Enabled = (owner as MyCubeBlock).IsFunctional;
          if (owner == this.m_interactedAsOwner || owner == this.m_userAsOwner)
            myGuiControlBaseList.Insert(0, (MyGuiControlBase) controlInventoryOwner);
          else
            myGuiControlBaseList.Add((MyGuiControlBase) controlInventoryOwner);
        }
      }
      listControl.InitControls((IEnumerable<MyGuiControlBase>) myGuiControlBaseList);
    }

    private void grid_focusChanged(MyGuiControlBase sender, bool focus)
    {
      if (!focus)
        return;
      MyGuiControlGrid myGuiControlGrid = sender as MyGuiControlGrid;
      this.FocusedGridControl = myGuiControlGrid;
      this.FocusedOwnerControl = (MyGuiControlInventoryOwner) myGuiControlGrid.Owner;
      this.RefreshSelectedInventoryItem();
      if (this.m_focusedOwnerControl == null)
        return;
      if (this.m_selectedInventoryItem.HasValue && this.CanTransferItem(this.m_selectedInventoryItem.Value, this.m_focusedGridControl, out MyInventory _, out MyInventory _))
        this.m_focusedOwnerControl.ResetGamepadHelp(this.m_userAsOwner, true);
      else
        this.m_focusedOwnerControl.ResetGamepadHelp(this.m_userAsOwner, false);
    }

    private bool grid_ItemControllerAction(
      MyGuiControlGrid sender,
      int index,
      MyGridItemAction action,
      bool pressed)
    {
      return this.ActivateItemGamepad(sender, index, action, pressed);
    }

    private void grid_ReleasedWithoutItem(MyGuiControlGrid obj)
    {
      this.FocusedGridControl = obj;
      this.FocusedOwnerControl = (MyGuiControlInventoryOwner) obj.Owner;
      this.RefreshSelectedInventoryItem();
    }

    private void ShowAmountTransferDialog(
      MyPhysicalInventoryItem inventoryItem,
      Action<float> onConfirmed)
    {
      MyFixedPoint amount = inventoryItem.Amount;
      MyObjectBuilderType typeId = inventoryItem.Content.TypeId;
      int minMaxDecimalDigits = 0;
      bool parseAsInteger = true;
      if (typeId == typeof (MyObjectBuilder_Ore) || typeId == typeof (MyObjectBuilder_Ingot))
      {
        minMaxDecimalDigits = 2;
        parseAsInteger = false;
      }
      MyGuiScreenDialogAmount dialog = new MyGuiScreenDialogAmount(0.0f, (float) amount, MyCommonTexts.DialogAmount_AddAmountCaption, minMaxDecimalDigits, parseAsInteger, backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity);
      dialog.OnConfirmed += onConfirmed;
      if (this.m_interactedAsEntity != null)
      {
        Action<MyEntity> entityCloseAction = (Action<MyEntity>) null;
        entityCloseAction = (Action<MyEntity>) (entity => dialog.CloseScreen());
        this.m_interactedAsEntity.OnClose += entityCloseAction;
        dialog.Closed += (MyGuiScreenBase.ScreenHandler) ((source, isUnloading) =>
        {
          if (this.m_interactedAsEntity == null)
            return;
          this.m_interactedAsEntity.OnClose -= entityCloseAction;
        });
      }
      MyGuiSandbox.AddScreen((MyGuiScreenBase) dialog);
    }

    private bool CanTransferItem(
      MyPhysicalInventoryItem item,
      MyGuiControlGrid sender,
      out MyInventory srcInventory,
      out MyInventory dstInventory)
    {
      srcInventory = (MyInventory) null;
      dstInventory = (MyInventory) null;
      if (sender == null || sender.Owner == null)
        return false;
      MyGuiControlInventoryOwner owner = sender.Owner as MyGuiControlInventoryOwner;
      ObservableCollection<MyGuiControlBase>.Enumerator enumerator = (owner.Owner == this.m_leftOwnersControl ? (MyGuiControlParent) this.m_rightOwnersControl : (MyGuiControlParent) this.m_leftOwnersControl).Controls.GetEnumerator();
      MyGuiControlInventoryOwner controlInventoryOwner = owner.Owner == this.m_leftOwnersControl ? this.RightFocusedInventory?.Owner as MyGuiControlInventoryOwner : this.LeftFocusedInventory?.Owner as MyGuiControlInventoryOwner;
      if (controlInventoryOwner == null)
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.Visible)
          {
            controlInventoryOwner = enumerator.Current as MyGuiControlInventoryOwner;
            break;
          }
        }
      }
      if (controlInventoryOwner == null || !controlInventoryOwner.Enabled)
        return false;
      if ((owner.InventoryOwner == this.m_userAsOwner || owner.InventoryOwner == this.m_interactedAsOwner ? (controlInventoryOwner.InventoryOwner == this.m_userAsOwner ? 1 : (controlInventoryOwner.InventoryOwner == this.m_interactedAsOwner ? 1 : 0)) : 0) == 0)
      {
        bool flag1 = owner.InventoryOwner is MyCharacter;
        bool flag2 = controlInventoryOwner.InventoryOwner is MyCharacter;
        IMyConveyorEndpointBlock conveyorEndpointBlock1 = owner.InventoryOwner == null ? (IMyConveyorEndpointBlock) null : (flag1 ? this.m_interactedAsOwner : owner.InventoryOwner) as IMyConveyorEndpointBlock;
        IMyConveyorEndpointBlock conveyorEndpointBlock2 = controlInventoryOwner.InventoryOwner == null ? (IMyConveyorEndpointBlock) null : (flag2 ? this.m_interactedAsOwner : controlInventoryOwner.InventoryOwner) as IMyConveyorEndpointBlock;
        if (conveyorEndpointBlock1 != null)
        {
          if (conveyorEndpointBlock2 != null)
          {
            try
            {
              MyGridConveyorSystem.AppendReachableEndpoints(conveyorEndpointBlock1.ConveyorEndpoint, MySession.Static.LocalPlayerId, this.m_reachableInventoryOwners, item.Content.GetId(), this.m_endpointPredicate);
              if (!this.m_reachableInventoryOwners.Contains(conveyorEndpointBlock2.ConveyorEndpoint))
                return false;
            }
            finally
            {
              this.m_reachableInventoryOwners.Clear();
            }
            if (!MyGridConveyorSystem.Reachable(conveyorEndpointBlock1.ConveyorEndpoint, conveyorEndpointBlock2.ConveyorEndpoint))
              return false;
            goto label_17;
          }
        }
        return false;
      }
label_17:
      MyEntity inventoryOwner1 = controlInventoryOwner.InventoryOwner;
      MyEntity inventoryOwner2 = owner.InventoryOwner;
      srcInventory = (MyInventory) sender.UserData;
      dstInventory = owner.Owner == this.m_leftOwnersControl ? this.RightFocusedInventory?.UserData as MyInventory : this.LeftFocusedInventory?.UserData as MyInventory;
      if (dstInventory == null)
      {
        for (int index = 0; index < inventoryOwner1.InventoryCount; ++index)
        {
          MyInventory inventory = MyEntityExtensions.GetInventory(inventoryOwner1, index);
          if (inventory.CheckConstraint(item.Content.GetId()))
          {
            dstInventory = inventory;
            break;
          }
        }
      }
      else if (!dstInventory.CheckConstraint(item.Content.GetId()))
        return false;
      return dstInventory != null;
    }

    private bool TransferToOppositeFirst(MyPhysicalInventoryItem item, MyGuiControlGrid sender)
    {
      MyInventory srcInventory;
      MyInventory dstInventory;
      if (!this.CanTransferItem(item, sender, out srcInventory, out dstInventory))
        return false;
      MyFixedPoint myFixedPoint = item.Amount;
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED))
        myFixedPoint = !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED) ? (MyFixedPoint) 10 : (MyFixedPoint) 1000;
      else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED))
        myFixedPoint = (MyFixedPoint) 100;
      List<MyPhysicalInventoryItem> items = srcInventory.GetItems();
      int index = sender.SelectedIndex.Value;
      if (index < 0 || index >= items.Count)
        return false;
      MyInventory.TransferByUser(srcInventory, dstInventory, items[index].ItemId, amount: new MyFixedPoint?(myFixedPoint));
      return true;
    }

    private void SetLeftFilter(MyInventoryOwnerTypeEnum? filterType)
    {
      this.m_leftFilterType = this.LeftFilter != MyGuiControlRadioButtonStyleEnum.FilterCharacter ? filterType : new MyInventoryOwnerTypeEnum?();
      this.m_leftFilterType = filterType;
      if (this.LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
      {
        this.CreateInventoryControlsInList(this.LeftFilterTypeIndex == 2 ? this.m_interactedGridOwnersMechanical : this.m_interactedGridOwners, this.m_leftOwnersControl, this.m_leftFilterType);
        this.m_searchBoxLeft.SearchText = this.m_searchBoxLeft.SearchText;
      }
      this.LeftFocusedInventory = this.m_leftOwnersControl.Controls.Count > 0 ? (this.m_leftOwnersControl.Controls[0] as MyGuiControlInventoryOwner).ContentGrids[0] : (MyGuiControlGrid) null;
      this.RefreshSelectedInventoryItem();
      this.ForceSelectSearchBox(this.m_searchBoxLeft);
      this.LeftFilterTypeIndex = this.m_leftFilterGroup.SelectedIndex.Value;
    }

    private void SetRightFilter(MyInventoryOwnerTypeEnum? filterType)
    {
      this.m_rightFilterType = this.RightFilter != MyGuiControlRadioButtonStyleEnum.FilterCharacter ? filterType : new MyInventoryOwnerTypeEnum?();
      if (this.RightFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
      {
        this.CreateInventoryControlsInList(this.RightFilterTypeIndex == 2 ? this.m_interactedGridOwnersMechanical : this.m_interactedGridOwners, this.m_rightOwnersControl, this.m_rightFilterType);
        this.m_searchBoxRight.SearchText = this.m_searchBoxRight.SearchText;
      }
      this.RightFocusedInventory = this.m_rightOwnersControl.Controls.Count > 0 ? (this.m_rightOwnersControl.Controls[0] as MyGuiControlInventoryOwner).ContentGrids[0] : (MyGuiControlGrid) null;
      this.RefreshSelectedInventoryItem();
      this.ForceSelectSearchBox(this.m_searchBoxRight);
      this.RightFilterTypeIndex = this.m_rightFilterGroup.SelectedIndex.Value;
    }

    private void RefreshSelectedInventoryItem()
    {
      if (this.FocusedGridControl != null)
      {
        this.m_selectedInventory = (MyInventory) this.FocusedGridControl.UserData;
        MyGuiGridItem selectedItem = this.FocusedGridControl.SelectedItem;
        this.m_selectedInventoryItem = selectedItem != null ? (MyPhysicalInventoryItem?) selectedItem.UserData : new MyPhysicalInventoryItem?();
        if (this.FocusedGridControl?.Owner?.Owner == this.m_leftOwnersControl)
          this.LeftFocusedInventory = this.FocusedGridControl;
        else if (this.FocusedGridControl?.Owner?.Owner == this.m_rightOwnersControl)
          this.RightFocusedInventory = this.FocusedGridControl;
      }
      else
      {
        this.m_selectedInventory = (MyInventory) null;
        this.m_selectedInventoryItem = new MyPhysicalInventoryItem?();
      }
      if (this.m_throwOutButton != null)
      {
        this.m_throwOutButton.Enabled = this.m_selectedInventoryItem.HasValue && this.m_selectedInventoryItem.HasValue && (this.FocusedOwnerControl != null && this.FocusedOwnerControl.InventoryOwner == this.m_userAsOwner);
        if (this.m_throwOutButton.Enabled)
          this.m_throwOutButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ThrowOut);
        else
          this.m_throwOutButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ThrowOutDisabled);
      }
      if (this.m_selectedToProductionButton != null)
      {
        if (this.m_selectedInventoryItem.HasValue && this.m_selectedInventoryItem.Value.Content != null && this.m_interactedAsEntity != null)
          this.m_selectedToProductionButton.Enabled = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(this.m_selectedInventoryItem.Value.Content.GetId()) != null;
        else
          this.m_selectedToProductionButton.Enabled = false;
      }
      if (this.m_depositAllButton != null)
        this.m_depositAllButton.Enabled = this.m_interactedAsEntity != null;
      if (MySession.Static == null || MySession.Static.LocalCharacter == null)
        return;
      if (this.m_addToProductionButton != null)
      {
        MyGuiControlButton productionButton = this.m_addToProductionButton;
        int num;
        if (this.m_interactedAsEntity != null && this.m_interactedAsEntity.Parent is MyCubeGrid)
        {
          IReadOnlyList<MyIdentity.BuildPlanItem> buildPlanner = MySession.Static.LocalCharacter.BuildPlanner;
          num = buildPlanner != null ? (buildPlanner.Count > 0 ? 1 : 0) : 0;
        }
        else
          num = 0;
        productionButton.Enabled = num != 0;
      }
      if (this.m_withdrawButton == null)
        return;
      MyGuiControlButton withdrawButton = this.m_withdrawButton;
      int num1;
      if (this.m_interactedAsEntity != null)
      {
        IReadOnlyList<MyIdentity.BuildPlanItem> buildPlanner = MySession.Static.LocalCharacter.BuildPlanner;
        num1 = buildPlanner != null ? (buildPlanner.Count > 0 ? 1 : 0) : 0;
      }
      else
        num1 = 0;
      withdrawButton.Enabled = num1 != 0;
    }

    private MyCubeGrid GetInteractedGrid() => this.m_interactedAsEntity == null ? (MyCubeGrid) null : this.m_interactedAsEntity.Parent as MyCubeGrid;

    private void ForceSelectSearchBox(MyGuiControlSearchBox searchBox)
    {
      IMyGuiControlsOwner guiControlsOwner = (IMyGuiControlsOwner) searchBox;
      while (guiControlsOwner.Owner != null)
        guiControlsOwner = guiControlsOwner.Owner;
      if (!(guiControlsOwner is MyGuiScreenBase myGuiScreenBase))
        return;
      myGuiScreenBase.FocusedControl = (MyGuiControlBase) searchBox.TextBox;
    }

    private void ForceSelectFirstInList(MyGuiControlList list)
    {
      if (list.Controls.Count <= 0)
        return;
      IMyGuiControlsOwner guiControlsOwner = (IMyGuiControlsOwner) list;
      while (guiControlsOwner.Owner != null)
        guiControlsOwner = guiControlsOwner.Owner;
      if (!(guiControlsOwner is MyGuiScreenBase myGuiScreenBase))
        return;
      foreach (MyGuiControlBase control in list.Controls)
      {
        if (control is MyGuiControlInventoryOwner controlInventoryOwner && controlInventoryOwner.Visible && controlInventoryOwner.ContentGrids.Count > 0)
        {
          myGuiScreenBase.FocusedControl = (MyGuiControlBase) controlInventoryOwner.ContentGrids[0];
          break;
        }
      }
    }

    private void ApplyTypeGroupSelectionChange(
      MyGuiControlRadioButtonGroup obj,
      MyGuiControlList targetControlList,
      MyInventoryOwnerTypeEnum? filterType,
      MyGuiControlRadioButtonGroup filterButtonGroup,
      MyGuiControlCheckbox showEmpty,
      MyGuiControlLabel showEmptyLabel,
      MyGuiControlSearchBox searchBox,
      bool isLeftControllist)
    {
      bool flag1 = false;
      switch (obj.SelectedButton.VisualStyle)
      {
        case MyGuiControlRadioButtonStyleEnum.FilterCharacter:
          flag1 = false;
          showEmpty.Visible = false;
          showEmptyLabel.Visible = false;
          searchBox.Visible = false;
          targetControlList.Position = isLeftControllist ? new Vector2(-0.46f, -0.276f) : new Vector2(0.4595f, -0.276f);
          targetControlList.Size = MyTerminalInventoryController.m_controlListFullSize;
          if (targetControlList == this.m_leftOwnersControl)
            this.CreateInventoryControlInList(this.m_userAsOwner, targetControlList);
          else
            this.CreateInventoryControlInList(this.m_interactedAsOwner, targetControlList);
          this.ForceSelectFirstInList(targetControlList);
          break;
        case MyGuiControlRadioButtonStyleEnum.FilterGrid:
          flag1 = true;
          this.CreateInventoryControlsInList((targetControlList == this.m_leftOwnersControl ? this.LeftFilterTypeIndex == 2 : this.RightFilterTypeIndex == 2) ? this.m_interactedGridOwnersMechanical : this.m_interactedGridOwners, targetControlList, filterType);
          showEmpty.Visible = true;
          showEmptyLabel.Visible = true;
          searchBox.Visible = true;
          searchBox.SearchText = searchBox.SearchText;
          targetControlList.Position = isLeftControllist ? new Vector2(-0.46f, -0.227f) : new Vector2(0.4595f, -0.227f);
          targetControlList.Size = MyTerminalInventoryController.m_controlListSizeWithSearch;
          this.ForceSelectSearchBox(searchBox);
          break;
      }
      foreach (MyGuiControlRadioButton controlRadioButton in filterButtonGroup)
      {
        int num;
        bool flag2 = (num = flag1 ? 1 : 0) != 0;
        controlRadioButton.Enabled = num != 0;
        controlRadioButton.Visible = flag2;
      }
      if (isLeftControllist)
        this.LeftFilter = obj.SelectedButton.VisualStyle;
      else
        this.RightFilter = obj.SelectedButton.VisualStyle;
      this.RefreshSelectedInventoryItem();
    }

    private void ConveyorSystem_BlockAdded(MyCubeBlock obj)
    {
      this.m_interactedGridOwners.Add((MyEntity) obj);
      if (this.LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterCharacter)
        this.LeftTypeGroup_SelectedChanged(this.m_leftTypeGroup);
      if (this.RightFilter == MyGuiControlRadioButtonStyleEnum.FilterCharacter)
        this.RightTypeGroup_SelectedChanged(this.m_rightTypeGroup);
      if (this.m_dragAndDropInfo == null)
        return;
      this.ClearDisabledControls();
      this.DisableInvalidWhileDragging();
    }

    private void ConveyorSystem_BlockRemoved(MyCubeBlock obj)
    {
      this.m_interactedGridOwners.Remove((MyEntity) obj);
      this.UpdateSelection();
      if (this.m_dragAndDropInfo == null)
        return;
      this.ClearDisabledControls();
      this.DisableInvalidWhileDragging();
    }

    private void ConveyorSystemMechanical_BlockAdded(MyCubeBlock obj)
    {
      this.m_interactedGridOwnersMechanical.Add((MyEntity) obj);
      if (this.LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
        this.LeftTypeGroup_SelectedChanged(this.m_leftTypeGroup);
      if (this.RightFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
        this.RightTypeGroup_SelectedChanged(this.m_rightTypeGroup);
      if (this.m_dragAndDropInfo == null)
        return;
      this.ClearDisabledControls();
      this.DisableInvalidWhileDragging();
    }

    private void ConveyorSystemMechanical_BlockRemoved(MyCubeBlock obj)
    {
      this.m_interactedGridOwnersMechanical.Remove((MyEntity) obj);
      this.UpdateSelection();
      if (this.m_dragAndDropInfo == null)
        return;
      this.ClearDisabledControls();
      this.DisableInvalidWhileDragging();
    }

    private void UpdateSelection()
    {
      if (this.LeftFilter != MyGuiControlRadioButtonStyleEnum.FilterGrid && this.RightFilter != MyGuiControlRadioButtonStyleEnum.FilterGrid)
        return;
      this.InvalidateBeforeDraw();
    }

    public override void UpdateBeforeDraw(MyGuiScreenBase screen)
    {
      base.UpdateBeforeDraw(screen);
      if (!this.m_dirtyDraw)
        return;
      this.m_dirtyDraw = false;
      if (this.LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
        this.LeftTypeGroup_SelectedChanged(this.m_leftTypeGroup);
      if (this.RightFilter != MyGuiControlRadioButtonStyleEnum.FilterGrid)
        return;
      this.RightTypeGroup_SelectedChanged(this.m_rightTypeGroup);
    }

    private void LeftTypeGroup_SelectedChanged(MyGuiControlRadioButtonGroup obj)
    {
      this.ApplyTypeGroupSelectionChange(obj, this.m_leftOwnersControl, this.m_leftFilterType, this.m_leftFilterGroup, this.m_hideEmptyLeft, this.m_hideEmptyLeftLabel, this.m_searchBoxLeft, true);
      this.m_leftOwnersControl.SetScrollBarPage();
      if (obj.SelectedIndex.HasValue)
        MyTerminalInventoryController.m_persistentRadioSelectionLeft = obj.SelectedIndex.Value;
      if (!this.CheckFocusedInventoryVisibilityLeft())
        this.SelectFirstLeftInventory();
      if (this.m_dragAndDropInfo == null)
        return;
      this.DisableInvalidWhileDragging();
    }

    private void RightTypeGroup_SelectedChanged(MyGuiControlRadioButtonGroup obj)
    {
      this.ApplyTypeGroupSelectionChange(obj, this.m_rightOwnersControl, this.m_rightFilterType, this.m_rightFilterGroup, this.m_hideEmptyRight, this.m_hideEmptyRightLabel, this.m_searchBoxRight, false);
      this.m_rightOwnersControl.SetScrollBarPage();
      if (obj.SelectedIndex.HasValue)
        MyTerminalInventoryController.m_persistentRadioSelectionRight = obj.SelectedIndex.Value;
      if (!this.CheckFocusedInventoryVisibilityRight())
        this.SelectFirstRightInventory();
      if (this.m_dragAndDropInfo == null)
        return;
      this.DisableInvalidWhileDragging();
    }

    private void ThrowOutButton_OnButtonClick(MyGuiControlButton sender)
    {
      MyEntity inventoryOwner = this.FocusedOwnerControl.InventoryOwner;
      if (this.m_selectedInventoryItem.HasValue && inventoryOwner != null && this.FocusedOwnerControl.InventoryOwner == this.m_userAsOwner)
      {
        MyPhysicalInventoryItem physicalInventoryItem = this.m_selectedInventoryItem.Value;
        if (this.FocusedGridControl.SelectedIndex.HasValue)
          this.m_selectedInventory.DropItem(this.FocusedGridControl.SelectedIndex.Value, physicalInventoryItem.Amount);
      }
      this.RefreshSelectedInventoryItem();
    }

    private void interactedObjectButton_OnButtonClick(MyGuiControlButton sender) => this.CreateInventoryControlInList(this.m_interactedAsOwner, this.m_rightOwnersControl);

    private void grid_ItemSelected(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
    {
      MyGuiControlGrid myGuiControlGrid = sender;
      this.FocusedGridControl = myGuiControlGrid;
      this.FocusedOwnerControl = (MyGuiControlInventoryOwner) myGuiControlGrid.Owner;
      this.RefreshSelectedInventoryItem();
    }

    private void grid_ItemDragged(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
    {
      if (MyInput.Static.IsAnyShiftKeyPressed() || MyInput.Static.IsAnyCtrlKeyPressed())
        return;
      this.StartDragging(MyDropHandleType.MouseRelease, sender, ref eventArgs);
    }

    private void grid_ItemDoubleClicked(
      MyGuiControlGrid sender,
      MyGuiControlGrid.EventArgs eventArgs)
    {
      if (MyInput.Static.IsAnyShiftKeyPressed() || MyInput.Static.IsAnyCtrlKeyPressed())
        return;
      this.TransferToOppositeFirst((MyPhysicalInventoryItem) sender.GetItemAt(eventArgs.ItemIndex).UserData, sender);
      this.RefreshSelectedInventoryItem();
    }

    private void grid_ItemClicked(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
    {
      bool flag1 = MyInput.Static.IsAnyCtrlKeyPressed();
      bool flag2 = MyInput.Static.IsAnyShiftKeyPressed();
      if (flag1 | flag2)
      {
        MyPhysicalInventoryItem userData = (MyPhysicalInventoryItem) sender.GetItemAt(eventArgs.ItemIndex).UserData;
        userData.Amount = MyFixedPoint.Min((MyFixedPoint) ((flag2 ? 100 : 1) * (flag1 ? 10 : 1)), userData.Amount);
        this.TransferToOppositeFirst(userData, sender);
        this.RefreshSelectedInventoryItem();
      }
      else
      {
        if (((MyPhysicalInventoryItem) sender.GetItemAt(eventArgs.ItemIndex).UserData).Content == null)
          return;
        MyInventory userData = this.FocusedGridControl.UserData as MyInventory;
        MyCharacter myCharacter = (MyCharacter) null;
        if (userData != null)
          myCharacter = userData.Owner as MyCharacter;
        if (myCharacter != null || userData.Owner != MySession.Static.ControlledEntity || !(userData.Owner is MyShipController owner))
          return;
        MyCharacter pilot = owner.Pilot;
      }
    }

    private void grid_ItemReleased(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs) => this.ActivateItemKeyboard(sender, eventArgs.ItemIndex, eventArgs.Button);

    private void ActivateItemKeyboard(
      MyGuiControlGrid sender,
      int index,
      MySharedButtonsEnum button)
    {
      MyPhysicalInventoryItem userData1 = (MyPhysicalInventoryItem) sender.GetItemAt(index).UserData;
      if (userData1.Content == null)
        return;
      MyInventory userData2 = sender.UserData as MyInventory;
      MyCharacter character = (MyCharacter) null;
      if (userData2 != null)
        character = userData2.Owner as MyCharacter;
      if (character == null && userData2.Owner == MySession.Static.ControlledEntity && userData2.Owner is MyShipController owner)
        character = owner.Pilot;
      if (character == null)
        return;
      MyUsableItemHelper.ItemActivatedGridKeyboard(userData1, userData2, character, button);
    }

    private bool ActivateItemGamepad(
      MyGuiControlGrid sender,
      int index,
      MyGridItemAction action,
      bool pressed)
    {
      if (sender.GetItemAt(index) == null)
        return false;
      MyPhysicalInventoryItem userData1 = (MyPhysicalInventoryItem) sender.GetItemAt(index).UserData;
      if (action == MyGridItemAction.Button_A)
      {
        if (pressed)
        {
          if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED) && !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED) && (this.RightFocusedInventory != null || this.LeftFocusedInventory != null))
          {
            if (this.RightFocusedInventory == null)
              this.RightFocusedInventory = this.LeftFocusedInventory;
            if (this.LeftFocusedInventory == null)
              this.LeftFocusedInventory = this.RightFocusedInventory;
            this.m_transferTimer = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
            this.m_isTransferTimerActive = true;
            this.m_transferData.Item = userData1;
            this.m_transferData.ToGrid = (sender.Owner as MyGuiControlInventoryOwner).Owner == this.m_leftOwnersControl ? this.RightFocusedInventory : this.LeftFocusedInventory;
            this.m_transferData.From = (MyInventory) sender.UserData;
            this.m_transferData.To = (MyInventory) this.m_transferData.ToGrid.UserData;
            this.m_transferData.IndexFrom = index;
            this.m_transferData.IndexTo = this.m_transferData.ToGrid.GetFirstEmptySlotIndex();
            return true;
          }
        }
        else
        {
          this.m_isTransferTimerActive = false;
          MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
          if (this.TransferToOppositeFirst(userData1, sender))
          {
            this.RefreshSelectedInventoryItem();
            return true;
          }
        }
      }
      if (userData1.Content == null || !pressed)
        return false;
      MyInventory userData2 = sender.UserData as MyInventory;
      MyCharacter character = (MyCharacter) null;
      if (userData2 != null)
        character = userData2.Owner as MyCharacter;
      if (character == null && userData2.Owner == MySession.Static.ControlledEntity && userData2.Owner is MyShipController owner)
        character = owner.Pilot;
      return character != null && MyUsableItemHelper.ItemActivatedGridGamepad(userData1, userData2, character, action);
    }

    private void dragDrop_OnItemDropped(object sender, MyDragAndDropEventArgs eventArgs)
    {
      if (eventArgs.DropTo != null)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
        MyPhysicalInventoryItem inventoryItem = (MyPhysicalInventoryItem) eventArgs.Item.UserData;
        MyGuiControlGrid grid = eventArgs.DragFrom.Grid;
        MyGuiControlGrid dstGrid = eventArgs.DropTo.Grid;
        MyGuiControlInventoryOwner owner = (MyGuiControlInventoryOwner) grid.Owner;
        if (!(dstGrid.Owner is MyGuiControlInventoryOwner))
          return;
        MyInventory srcInventory = (MyInventory) grid.UserData;
        MyInventory dstInventory = (MyInventory) dstGrid.UserData;
        if (grid == dstGrid)
        {
          if (eventArgs.DragButton == MySharedButtonsEnum.Secondary)
          {
            this.ShowAmountTransferDialog(inventoryItem, (Action<float>) (amount =>
            {
              if ((double) amount == 0.0 || !srcInventory.IsItemAt(eventArgs.DragFrom.ItemIndex))
                return;
              inventoryItem.Amount = (MyFixedPoint) amount;
              MyTerminalInventoryController.CorrectItemAmount(ref inventoryItem);
              MyInventory.TransferByUser(srcInventory, srcInventory, inventoryItem.ItemId, eventArgs.DropTo.ItemIndex, new MyFixedPoint?(inventoryItem.Amount));
              if (dstGrid.IsValidIndex(eventArgs.DropTo.ItemIndex))
                dstGrid.SelectedIndex = new int?(eventArgs.DropTo.ItemIndex);
              else
                dstGrid.SelectLastItem();
              this.RefreshSelectedInventoryItem();
            }));
          }
          else
          {
            MyInventory.TransferByUser(srcInventory, srcInventory, inventoryItem.ItemId, eventArgs.DropTo.ItemIndex);
            if (dstGrid.IsValidIndex(eventArgs.DropTo.ItemIndex))
              dstGrid.SelectedIndex = new int?(eventArgs.DropTo.ItemIndex);
            else
              dstGrid.SelectLastItem();
            this.RefreshSelectedInventoryItem();
          }
        }
        else if (eventArgs.DragButton == MySharedButtonsEnum.Secondary)
        {
          this.ShowAmountTransferDialog(inventoryItem, (Action<float>) (amount =>
          {
            if ((double) amount == 0.0 || !srcInventory.IsItemAt(eventArgs.DragFrom.ItemIndex))
              return;
            inventoryItem.Amount = (MyFixedPoint) amount;
            MyTerminalInventoryController.CorrectItemAmount(ref inventoryItem);
            MyInventory.TransferByUser(srcInventory, dstInventory, inventoryItem.ItemId, eventArgs.DropTo.ItemIndex, new MyFixedPoint?(inventoryItem.Amount));
            this.RefreshSelectedInventoryItem();
          }));
        }
        else
        {
          MyInventory.TransferByUser(srcInventory, dstInventory, inventoryItem.ItemId, eventArgs.DropTo.ItemIndex);
          this.RefreshSelectedInventoryItem();
        }
      }
      else
      {
        MyGuiControlGridDragAndDrop dragDrop = (MyGuiControlGridDragAndDrop) sender;
        if (this.m_throwOutButton.Enabled && (dragDrop.IsEmptySpace() || this.IsDroppedOnThrowOutButton(dragDrop)))
          this.ThrowOutButton_OnButtonClick(this.m_throwOutButton);
      }
      this.ClearDisabledControls();
      this.m_dragAndDropInfo = (MyDragAndDropInfo) null;
    }

    private void ClearDisabledControls()
    {
      foreach (MyGuiControlBase myGuiControlBase in this.m_controlsDisabledWhileDragged)
        myGuiControlBase.Enabled = true;
      this.m_controlsDisabledWhileDragged.Clear();
    }

    private bool IsDroppedOnThrowOutButton(MyGuiControlGridDragAndDrop dragDrop)
    {
      foreach (MyGuiControlBase dropToControl in dragDrop.DropToControls)
      {
        if (dropToControl.Name.Equals("ThrowOutButton", StringComparison.InvariantCultureIgnoreCase))
          return true;
      }
      return false;
    }

    private static void CorrectItemAmount(ref MyPhysicalInventoryItem dragItem)
    {
      MyObjectBuilderType typeId = dragItem.Content.TypeId;
    }

    private void inventoryControl_SizeChanged(MyGuiControlBase obj) => ((MyGuiControlList) obj.Owner).Recalculate();

    private void ownerControl_InventoryContentsChanged(MyGuiControlInventoryOwner control)
    {
      if (control == this.FocusedOwnerControl)
        this.RefreshSelectedInventoryItem();
      this.UpdateDisabledControlsWhileDragging(control);
    }

    private void UpdateDisabledControlsWhileDragging(MyGuiControlInventoryOwner control)
    {
      if (this.m_controlsDisabledWhileDragged.Count == 0)
        return;
      MyEntity inventoryOwner = control.InventoryOwner;
      for (int index = 0; index < inventoryOwner.InventoryCount; ++index)
      {
        MyGuiControlGrid contentGrid = control.ContentGrids[index];
        if (this.m_controlsDisabledWhileDragged.Contains(contentGrid) && contentGrid.Enabled)
          contentGrid.Enabled = false;
      }
    }

    private void HideEmptyLeft_Checked(MyGuiControlCheckbox obj)
    {
      MyInventoryOwnerTypeEnum? leftFilterType = this.m_leftFilterType;
      MyInventoryOwnerTypeEnum inventoryOwnerTypeEnum = MyInventoryOwnerTypeEnum.Character;
      if (leftFilterType.GetValueOrDefault() == inventoryOwnerTypeEnum & leftFilterType.HasValue)
        return;
      this.SearchInList(this.m_searchBoxLeft.TextBox, this.m_leftOwnersControl, obj.IsChecked);
      this.CheckFocusedInventoryVisibilityLeft();
    }

    private bool CheckFocusedInventoryVisibilityLeft()
    {
      if (this.LeftFocusedInventory != null && this.LeftFocusedInventory.Owner != null && this.LeftFocusedInventory.Owner.Visible)
        return true;
      this.LeftFocusedInventory = (MyGuiControlGrid) null;
      return false;
    }

    private void SelectFirstLeftInventory()
    {
      foreach (MyGuiControlBase control in this.m_leftOwnersControl.Controls)
      {
        if (control.Visible)
        {
          MyGuiControlInventoryOwner controlInventoryOwner = control as MyGuiControlInventoryOwner;
          if (controlInventoryOwner.ContentGrids.Count > 0)
            this.LeftFocusedInventory = controlInventoryOwner.ContentGrids[0];
        }
      }
    }

    private void SelectFirstRightInventory()
    {
      foreach (MyGuiControlBase control in this.m_rightOwnersControl.Controls)
      {
        if (control.Visible)
        {
          MyGuiControlInventoryOwner controlInventoryOwner = control as MyGuiControlInventoryOwner;
          if (controlInventoryOwner.ContentGrids.Count > 0)
            this.RightFocusedInventory = controlInventoryOwner.ContentGrids[0];
        }
      }
    }

    private void HideEmptyRight_Checked(MyGuiControlCheckbox obj)
    {
      MyInventoryOwnerTypeEnum? rightFilterType = this.m_rightFilterType;
      MyInventoryOwnerTypeEnum inventoryOwnerTypeEnum = MyInventoryOwnerTypeEnum.Character;
      if (rightFilterType.GetValueOrDefault() == inventoryOwnerTypeEnum & rightFilterType.HasValue)
        return;
      this.SearchInList(this.m_searchBoxRight.TextBox, this.m_rightOwnersControl, obj.IsChecked);
      this.CheckFocusedInventoryVisibilityRight();
    }

    private bool CheckFocusedInventoryVisibilityRight()
    {
      if (this.RightFocusedInventory != null && this.RightFocusedInventory.Owner != null && this.RightFocusedInventory.Owner.Visible)
        return true;
      this.RightFocusedInventory = (MyGuiControlGrid) null;
      return false;
    }

    private void BlockSearchLeft_TextChanged(string obj)
    {
      MyInventoryOwnerTypeEnum? leftFilterType = this.m_leftFilterType;
      MyInventoryOwnerTypeEnum inventoryOwnerTypeEnum = MyInventoryOwnerTypeEnum.Character;
      if (leftFilterType.GetValueOrDefault() == inventoryOwnerTypeEnum & leftFilterType.HasValue)
        return;
      this.SearchInList(this.m_searchBoxLeft.TextBox, this.m_leftOwnersControl, this.m_hideEmptyLeft.IsChecked);
      MyGuiControlInventoryOwner controlInventoryOwner1 = (MyGuiControlInventoryOwner) null;
      foreach (MyGuiControlBase control in this.m_leftOwnersControl.Controls)
      {
        if (control.Visible && control is MyGuiControlInventoryOwner controlInventoryOwner2)
        {
          controlInventoryOwner1 = controlInventoryOwner2;
          break;
        }
      }
      if (controlInventoryOwner1 == null || controlInventoryOwner1.ContentGrids.Count <= 0)
        return;
      this.FocusedGridControl = controlInventoryOwner1.ContentGrids[0];
      this.FocusedOwnerControl = controlInventoryOwner1;
      this.RefreshSelectedInventoryItem();
    }

    private void BlockSearchRight_TextChanged(string obj)
    {
      MyInventoryOwnerTypeEnum? rightFilterType = this.m_rightFilterType;
      MyInventoryOwnerTypeEnum inventoryOwnerTypeEnum = MyInventoryOwnerTypeEnum.Character;
      if (rightFilterType.GetValueOrDefault() == inventoryOwnerTypeEnum & rightFilterType.HasValue)
        return;
      this.SearchInList(this.m_searchBoxRight.TextBox, this.m_rightOwnersControl, this.m_hideEmptyRight.IsChecked);
      MyGuiControlInventoryOwner controlInventoryOwner1 = (MyGuiControlInventoryOwner) null;
      foreach (MyGuiControlBase control in this.m_rightOwnersControl.Controls)
      {
        if (control.Visible && control is MyGuiControlInventoryOwner controlInventoryOwner2)
        {
          controlInventoryOwner1 = controlInventoryOwner2;
          break;
        }
      }
      if (controlInventoryOwner1 == null || controlInventoryOwner1.ContentGrids.Count <= 0)
        return;
      this.FocusedGridControl = controlInventoryOwner1.ContentGrids[0];
      this.FocusedOwnerControl = controlInventoryOwner1;
      this.RefreshSelectedInventoryItem();
    }

    private void SearchInList(
      MyGuiControlTextbox searchText,
      MyGuiControlList list,
      bool hideEmpty)
    {
      if (searchText.Text != "")
      {
        string[] strArray = searchText.Text.ToLower().Split(' ');
        foreach (MyGuiControlBase control in list.Controls)
        {
          MyEntity inventoryOwner = (control as MyGuiControlInventoryOwner).InventoryOwner;
          string lower1 = inventoryOwner.DisplayNameText.ToString().ToLower();
          bool flag1 = true;
          bool flag2 = true;
          foreach (string str in strArray)
          {
            if (!lower1.Contains(str))
            {
              flag1 = false;
              break;
            }
          }
          if (!flag1)
          {
            for (int index = 0; index < inventoryOwner.InventoryCount; ++index)
            {
              foreach (MyPhysicalInventoryItem physicalInventoryItem in MyEntityExtensions.GetInventory(inventoryOwner, index).GetItems())
              {
                bool flag3 = true;
                MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) physicalInventoryItem.Content);
                if (physicalItemDefinition != null && !string.IsNullOrEmpty(physicalItemDefinition.DisplayNameText))
                {
                  string lower2 = physicalItemDefinition.DisplayNameText.ToString().ToLower();
                  foreach (string str in strArray)
                  {
                    if (!lower2.Contains(str))
                    {
                      flag3 = false;
                      break;
                    }
                  }
                  if (flag3)
                  {
                    flag1 = true;
                    break;
                  }
                }
              }
              if (flag1)
                break;
            }
          }
          if (flag1)
          {
            for (int index = 0; index < inventoryOwner.InventoryCount; ++index)
            {
              if (MyEntityExtensions.GetInventory(inventoryOwner, index).CurrentMass != (MyFixedPoint) 0)
              {
                flag2 = false;
                break;
              }
            }
            control.Visible = !(hideEmpty & flag2);
          }
          else
            control.Visible = false;
        }
      }
      else
      {
        foreach (MyGuiControlBase control in list.Controls)
        {
          bool flag = true;
          MyEntity inventoryOwner = (control as MyGuiControlInventoryOwner).InventoryOwner;
          for (int index = 0; index < inventoryOwner.InventoryCount; ++index)
          {
            if (MyEntityExtensions.GetInventory(inventoryOwner, index).CurrentMass != (MyFixedPoint) 0)
            {
              flag = false;
              break;
            }
          }
          control.Visible = !(hideEmpty & flag);
        }
      }
      list.SetScrollBarPage();
    }

    private void MoveItems(int direction, MyGuiControlGrid grid, int index)
    {
      MyPhysicalInventoryItem userData1 = (MyPhysicalInventoryItem) grid.GetItemAt(index).UserData;
      MyInventory userData2 = (MyInventory) grid.UserData;
      MyTerminalInventoryController.CorrectItemAmount(ref userData1);
      int num = index;
      switch (direction)
      {
        case 0:
          num = index - grid.ColumnsCount;
          break;
        case 1:
          num = index - 1;
          break;
        case 2:
          num = index + 1;
          break;
        case 3:
          num = index + grid.ColumnsCount;
          break;
      }
      MyInventory.TransferByUser(userData2, userData2, userData1.ItemId, num, new MyFixedPoint?(userData1.Amount));
      if (!grid.IsValidIndex(num))
        grid.SelectLastItem();
      this.RefreshSelectedInventoryItem();
    }

    public override void HandleInput()
    {
      base.HandleInput();
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
        this.selectedToProductionButton_ButtonClicked((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.VIEW))
        this.DepositAllButton_ButtonClicked((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MENU) && this.m_throwOutButton.Enabled)
        this.ThrowOutButton_OnButtonClick((MyGuiControlButton) null);
      MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      if (this.m_isTransferTimerActive && this.m_transferTimer + MyTerminalInventoryController.TRANSFER_TIMER_TIME <= myTimeSpan)
      {
        this.OpenTransferDialog();
        this.m_isTransferTimerActive = false;
      }
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
        this.SwitchFilter(this.m_leftTypeGroup, this.m_leftFilterGroup, this.m_leftFilterGamepadHelp);
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
        this.SwitchFilter(this.m_rightTypeGroup, this.m_rightFilterGroup, this.m_rightFilterGamepadHelp);
      if (this.m_throwOutButton != null)
        this.m_throwOutButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      if (this.m_selectedToProductionButton != null)
        this.m_selectedToProductionButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      if (this.m_depositAllButton != null)
        this.m_depositAllButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_leftFilterGamepadHelp.Visible = this.m_rightFilterGamepadHelp.Visible = MyInput.Static.IsJoystickLastUsed;
      int? nullable = new int?();
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_UP))
        nullable = new int?(0);
      else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_LEFT))
        nullable = new int?(1);
      else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_RIGHT))
        nullable = new int?(2);
      else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_DOWN))
        nullable = new int?(3);
      if (!nullable.HasValue)
        return;
      MyEntity inventoryOwner = this.FocusedOwnerControl.InventoryOwner;
      if (this.m_selectedInventoryItem.HasValue && inventoryOwner != null)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
        MyPhysicalInventoryItem physicalInventoryItem = this.m_selectedInventoryItem.Value;
        int? selectedIndex = this.FocusedGridControl.SelectedIndex;
        if (selectedIndex.HasValue)
        {
          int direction = nullable.Value;
          MyGuiControlGrid focusedGridControl = this.FocusedGridControl;
          selectedIndex = this.FocusedGridControl.SelectedIndex;
          int index = selectedIndex.Value;
          this.MoveItems(direction, focusedGridControl, index);
        }
      }
      this.RefreshSelectedInventoryItem();
    }

    private void SwitchFilter(
      MyGuiControlRadioButtonGroup typeGroup,
      MyGuiControlRadioButtonGroup filterGroup,
      MyGuiControlLabel gamepadHelp)
    {
      int? selectedIndex;
      int num1;
      if (!typeGroup.SelectedIndex.HasValue)
      {
        num1 = 0;
      }
      else
      {
        selectedIndex = typeGroup.SelectedIndex;
        num1 = selectedIndex.Value;
      }
      int num2 = num1;
      if (num2 != typeGroup.Count - 1 && this.m_interactedAsEntity != null)
      {
        typeGroup.SelectByIndex(num2 + 1);
      }
      else
      {
        selectedIndex = filterGroup.SelectedIndex;
        int num3;
        if (!selectedIndex.HasValue)
        {
          num3 = 0;
        }
        else
        {
          selectedIndex = filterGroup.SelectedIndex;
          num3 = selectedIndex.Value;
        }
        int num4 = num3;
        if (num4 != filterGroup.Count - 1)
        {
          filterGroup.SelectByIndex(num4 + 1);
        }
        else
        {
          filterGroup.SelectByIndex(0);
          typeGroup.SelectByIndex(0);
        }
      }
      this.SetFilterGamepadHelp(typeGroup, filterGroup, gamepadHelp);
    }

    private void SetFilterGamepadHelp(
      MyGuiControlRadioButtonGroup typeGroup,
      MyGuiControlRadioButtonGroup filterGroup,
      MyGuiControlLabel gamepadHelp)
    {
      string str1 = MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_ActiveFilter) + "\n";
      switch (typeGroup.SelectedButton.VisualStyle)
      {
        case MyGuiControlRadioButtonStyleEnum.FilterCharacter:
          gamepadHelp.Text = str1 + MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_Character);
          return;
        case MyGuiControlRadioButtonStyleEnum.FilterGrid:
          str1 += MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_ShipOrStation);
          break;
      }
      string str2 = str1 + "\n";
      switch (filterGroup.SelectedButton.VisualStyle)
      {
        case MyGuiControlRadioButtonStyleEnum.FilterAll:
          str2 += MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_AllInventories);
          break;
        case MyGuiControlRadioButtonStyleEnum.FilterEnergy:
          str2 += MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_EnergyInventories);
          break;
        case MyGuiControlRadioButtonStyleEnum.FilterShip:
          str2 += MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_CurrentShip);
          break;
        case MyGuiControlRadioButtonStyleEnum.FilterStorage:
          str2 += MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_StorageInventories);
          break;
        case MyGuiControlRadioButtonStyleEnum.FilterSystem:
          str2 += MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_SystemInventories);
          break;
      }
      gamepadHelp.Text = str2;
    }

    private void OpenTransferDialog()
    {
      if (!this.CanTransferItem(this.m_transferData.Item, this.m_transferData.ToGrid, out MyInventory _, out MyInventory _))
        return;
      this.ShowAmountTransferDialog(this.m_transferData.Item, (Action<float>) (amount =>
      {
        if ((double) amount == 0.0 || !this.m_transferData.From.IsItemAt(this.m_transferData.IndexFrom))
          return;
        this.m_transferData.Item.Amount = (MyFixedPoint) amount;
        MyTerminalInventoryController.CorrectItemAmount(ref this.m_transferData.Item);
        MyInventory.TransferByUser(this.m_transferData.From, this.m_transferData.To, this.m_transferData.Item.ItemId, this.m_transferData.IndexTo, new MyFixedPoint?(this.m_transferData.Item.Amount));
        if (this.m_transferData.ToGrid.IsValidIndex(this.m_transferData.IndexTo))
          this.m_transferData.ToGrid.SelectedIndex = new int?(this.m_transferData.IndexTo);
        else
          this.m_transferData.ToGrid.SelectLastItem();
        this.RefreshSelectedInventoryItem();
      }));
    }

    public MyGuiControlGrid GetDefaultFocus() => this.LeftFocusedInventory;

    private struct MyGamepadTransferCollection
    {
      public MyPhysicalInventoryItem Item;
      public MyInventory From;
      public MyInventory To;
      public MyGuiControlGrid ToGrid;
      public int IndexFrom;
      public int IndexTo;
    }

    private struct QueueComponent
    {
      public MyDefinitionId Id;
      public int Count;
    }

    private enum MyBuildPlannerAction
    {
      None,
      DefaultWithdraw,
      WithdrawKeep1,
      WithdrawKeep10,
      AddProduction1,
      AddProduction10,
    }
  }
}
