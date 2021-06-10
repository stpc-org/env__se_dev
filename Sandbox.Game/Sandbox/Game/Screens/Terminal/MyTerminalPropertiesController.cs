// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Terminal.MyTerminalPropertiesController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
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
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Terminal
{
  internal class MyTerminalPropertiesController
  {
    private MyGuiControlCombobox m_shipsInRange;
    private MyGuiControlButton m_button;
    private MyGuiControlTable m_shipsData;
    private MyEntity m_interactedEntityRepresentative;
    private MyEntity m_openInventoryInteractedEntityRepresentative;
    private MyEntity m_interactedEntity;
    private bool m_isRemote;
    private int m_columnToSort;
    private HashSet<MyDataReceiver> m_tmpAntennas = new HashSet<MyDataReceiver>();
    private Dictionary<long, MyTerminalPropertiesController.CubeGridInfo> m_tmpGridInfoOutput = new Dictionary<long, MyTerminalPropertiesController.CubeGridInfo>();
    private HashSet<MyDataBroadcaster> m_tmpBroadcasters = new HashSet<MyDataBroadcaster>();
    private HashSet<MyAntennaSystem.BroadcasterInfo> m_previousMutualConnectionGrids;
    private HashSet<MyTerminalPropertiesController.CubeGridInfo> m_previousShipInfo;
    private int m_cnt;

    public event Action ButtonClicked;

    public void Init(
      MyGuiControlParent menuParent,
      MyGuiControlParent panelParent,
      MyEntity interactedEntity,
      MyEntity openInventoryInteractedEntity,
      bool isRemote)
    {
      this.m_interactedEntityRepresentative = this.GetInteractedEntityRepresentative(interactedEntity);
      this.m_openInventoryInteractedEntityRepresentative = this.GetInteractedEntityRepresentative(openInventoryInteractedEntity);
      this.m_interactedEntity = interactedEntity ?? (MyEntity) MySession.Static.LocalCharacter;
      this.m_isRemote = isRemote;
      if (menuParent == null)
        MySandboxGame.Log.WriteLine("menuParent is null");
      if (panelParent == null)
        MySandboxGame.Log.WriteLine("panelParent is null");
      if (menuParent == null || panelParent == null)
        return;
      this.m_shipsInRange = (MyGuiControlCombobox) menuParent.Controls.GetControlByName("ShipsInRange");
      this.m_button = (MyGuiControlButton) menuParent.Controls.GetControlByName("SelectShip");
      this.m_shipsData = (MyGuiControlTable) panelParent.Controls.GetControlByName("ShipsData");
      this.m_columnToSort = 1;
      this.m_button.ButtonClicked += new Action<MyGuiControlButton>(this.Menu_ButtonClicked);
      this.m_shipsData.ColumnClicked += new Action<MyGuiControlTable, int>(this.shipsData_ColumnClicked);
      this.m_shipsInRange.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.shipsInRange_ItemSelected);
      this.Refresh();
    }

    public void Refresh()
    {
      this.PopulateMutuallyConnectedCubeGrids(MyAntennaSystem.Static.GetConnectedGridsInfo(this.m_openInventoryInteractedEntityRepresentative, accessible: true));
      this.PopulateOwnedCubeGrids(this.GetAllCubeGridsInfo());
    }

    private void PopulateMutuallyConnectedCubeGrids(
      HashSet<MyAntennaSystem.BroadcasterInfo> playerMutualConnection)
    {
      if (playerMutualConnection == null || this.m_openInventoryInteractedEntityRepresentative == null || (this.m_shipsInRange == null || this.m_interactedEntityRepresentative == null))
        return;
      this.m_shipsInRange.ClearItems();
      this.m_shipsInRange.AddItem(this.m_openInventoryInteractedEntityRepresentative.EntityId, new StringBuilder(this.m_openInventoryInteractedEntityRepresentative.DisplayName));
      foreach (MyAntennaSystem.BroadcasterInfo broadcasterInfo in playerMutualConnection)
      {
        if (this.m_shipsInRange.TryGetItemByKey(broadcasterInfo.EntityId) == null)
          this.m_shipsInRange.AddItem(broadcasterInfo.EntityId, new StringBuilder(broadcasterInfo.Name));
      }
      this.m_shipsInRange.Visible = true;
      if (this.m_button != null)
        this.m_button.Visible = true;
      this.m_shipsInRange.SortItemsByValueText();
      if (this.m_shipsInRange.TryGetItemByKey(this.m_interactedEntityRepresentative.EntityId) == null && this.m_interactedEntityRepresentative is MyCubeGrid)
        this.m_shipsInRange.AddItem(this.m_interactedEntityRepresentative.EntityId, new StringBuilder((this.m_interactedEntityRepresentative as MyCubeGrid).DisplayName));
      this.m_shipsInRange.SelectItemByKey(this.m_interactedEntityRepresentative.EntityId);
    }

    private void PopulateOwnedCubeGrids(
      HashSet<MyTerminalPropertiesController.CubeGridInfo> gridInfoList)
    {
      if (gridInfoList == null)
        return;
      long num = 0;
      if (this.m_shipsData.SelectedRow?.UserData != null)
        num = ((MyTerminalPropertiesController.UserData) this.m_shipsData.SelectedRow?.UserData).GridEntityId;
      float amount = this.m_shipsData.ScrollBar.Value;
      this.m_shipsData.Clear();
      this.m_shipsData.Controls.Clear();
      MyGuiControlTable.Row row1 = (MyGuiControlTable.Row) null;
      foreach (MyTerminalPropertiesController.CubeGridInfo gridInfo in gridInfoList)
      {
        MyTerminalPropertiesController.UserData userData;
        userData.GridEntityId = gridInfo.EntityId;
        userData.RemoteEntityId = gridInfo.RemoteId;
        string collectiveTooltip = string.Empty;
        MyGuiControlTable.Cell cell1;
        MyGuiControlTable.Cell controlCell;
        MyGuiControlTable.Cell cell2;
        MyGuiControlTable.Cell statusIcons;
        MyGuiControlTable.Cell terminalCell;
        if (gridInfo.Status == MyTerminalPropertiesController.MyCubeGridConnectionStatus.Connected || gridInfo.Status == MyTerminalPropertiesController.MyCubeGridConnectionStatus.PhysicallyConnected || gridInfo.Status == MyTerminalPropertiesController.MyCubeGridConnectionStatus.Me)
        {
          StringBuilder stringBuilder = new StringBuilder();
          if (gridInfo.Status == MyTerminalPropertiesController.MyCubeGridConnectionStatus.Connected)
            stringBuilder = gridInfo.AppendedDistance;
          userData.IsSelectable = true;
          StringBuilder text1 = new StringBuilder(gridInfo.Name);
          Color? nullable = new Color?(Color.White);
          string name = gridInfo.Name;
          Color? textColor1 = nullable;
          MyGuiHighlightTexture? icon1 = new MyGuiHighlightTexture?();
          cell1 = new MyGuiControlTable.Cell(text1, toolTip: name, textColor: textColor1, icon: icon1);
          controlCell = this.CreateControlCell(gridInfo, true);
          StringBuilder text2 = stringBuilder;
          string str = stringBuilder.ToString();
          // ISSUE: variable of a boxed type
          __Boxed<double> distance = (ValueType) gridInfo.Distance;
          string toolTip = str;
          Color? textColor2 = new Color?(Color.White);
          MyGuiHighlightTexture? icon2 = new MyGuiHighlightTexture?();
          cell2 = new MyGuiControlTable.Cell(text2, (object) distance, toolTip, textColor2, icon2);
          statusIcons = this.CreateStatusIcons(gridInfo, true, out collectiveTooltip);
          terminalCell = this.CreateTerminalCell(gridInfo, true);
        }
        else
        {
          userData.IsSelectable = false;
          StringBuilder text = new StringBuilder(gridInfo.Name);
          Color? nullable = new Color?(Color.Gray);
          string name = gridInfo.Name;
          Color? textColor = nullable;
          MyGuiHighlightTexture? icon = new MyGuiHighlightTexture?();
          cell1 = new MyGuiControlTable.Cell(text, toolTip: name, textColor: textColor, icon: icon);
          controlCell = this.CreateControlCell(gridInfo, false);
          cell2 = new MyGuiControlTable.Cell(MyTexts.Get(MySpaceTexts.NotAvailable), (object) double.MaxValue, MyTexts.GetString(MySpaceTexts.NotAvailable), new Color?(Color.Gray));
          statusIcons = this.CreateStatusIcons(gridInfo, true, out collectiveTooltip);
          terminalCell = this.CreateTerminalCell(gridInfo, false);
        }
        MyGuiControlTable.Row row2 = new MyGuiControlTable.Row((object) userData, collectiveTooltip);
        row2.AddCell(cell1);
        row2.AddCell(cell2);
        row2.AddCell(statusIcons);
        row2.AddCell(controlCell);
        row2.AddCell(terminalCell);
        this.m_shipsData.Add(row2);
        if (num == userData.GridEntityId)
          row1 = row2;
      }
      this.m_shipsData.SortByColumn(this.m_columnToSort, new MyGuiControlTable.SortStateEnum?(MyGuiControlTable.SortStateEnum.Ascending), false);
      this.m_shipsData.ScrollBar.ChangeValue(amount);
      this.m_shipsData.GamepadHelpTextId = MySpaceTexts.TerminalRemote_Help_ShipsTable;
      if (row1 == null)
        return;
      this.m_shipsData.SelectedRow = row1;
    }

    private MyGuiControlTable.Cell CreateControlCell(
      MyTerminalPropertiesController.CubeGridInfo gridInfo,
      bool isActive)
    {
      MyGuiControlTable.Cell cell1 = new MyGuiControlTable.Cell();
      Vector2 vector2 = new Vector2(0.1f, this.m_shipsData.RowHeight * 0.8f);
      switch (gridInfo.RemoteStatus)
      {
        case MyTerminalPropertiesController.MyRefuseReason.NoRemoteControl:
        case MyTerminalPropertiesController.MyRefuseReason.NoMainRemoteControl:
        case MyTerminalPropertiesController.MyRefuseReason.Forbidden:
          isActive = false;
          break;
      }
      isActive &= this.CanTakeTerminalOuter(gridInfo);
      MyGuiControlTable.Cell cell2 = cell1;
      Vector2? position = new Vector2?();
      StringBuilder stringBuilder = MyTexts.Get(MySpaceTexts.BroadcastScreen_TakeControlButton);
      Vector2? size = new Vector2?(vector2);
      Vector4? colorMask = new Vector4?();
      StringBuilder text = stringBuilder;
      Action<MyGuiControlButton> onButtonClick = new Action<MyGuiControlButton>(this.OnButtonClicked_TakeControl);
      int? buttonIndex = new int?();
      MyGuiControlButton guiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Rectangular, size, colorMask, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, text: text, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
      cell2.Control = (MyGuiControlBase) guiControlButton;
      cell1.Control.ShowTooltipWhenDisabled = true;
      cell1.Control.Enabled = isActive;
      if (cell1.Control.Enabled)
        cell1.Control.SetToolTip(MySpaceTexts.BroadcastScreen_TakeControlButton_ToolTip);
      else
        cell1.Control.SetToolTip(MySpaceTexts.BroadcastScreen_TakeControlButtonDisabled_ToolTip);
      this.m_shipsData.Controls.Add(cell1.Control);
      return cell1;
    }

    private bool CanTakeTerminalOuter(
      MyTerminalPropertiesController.CubeGridInfo gridInfo)
    {
      bool flag = true;
      switch (this.CanTakeTerminal(gridInfo))
      {
        case MyTerminalPropertiesController.MyRefuseReason.NoStableConnection:
        case MyTerminalPropertiesController.MyRefuseReason.NoOwner:
        case MyTerminalPropertiesController.MyRefuseReason.PlayerBroadcastOff:
        case MyTerminalPropertiesController.MyRefuseReason.Forbidden:
          flag = false;
          break;
      }
      return flag;
    }

    private MyGuiControlTable.Cell CreateTerminalCell(
      MyTerminalPropertiesController.CubeGridInfo gridInfo,
      bool isActive)
    {
      MyGuiControlTable.Cell cell1 = new MyGuiControlTable.Cell();
      Vector2 vector2 = new Vector2(0.1f, this.m_shipsData.RowHeight * 0.8f);
      isActive &= this.CanTakeTerminalOuter(gridInfo);
      MyGuiControlTable.Cell cell2 = cell1;
      Vector2? position = new Vector2?();
      StringBuilder stringBuilder = MyTexts.Get(MySpaceTexts.BroadcastScreen_TerminalButton);
      Vector2? size = new Vector2?(vector2);
      Vector4? colorMask = new Vector4?();
      StringBuilder text = stringBuilder;
      Action<MyGuiControlButton> onButtonClick = new Action<MyGuiControlButton>(this.OnButtonClicked_OpenTerminal);
      int? buttonIndex = new int?();
      MyGuiControlButton guiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Rectangular, size, colorMask, text: text, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
      cell2.Control = (MyGuiControlBase) guiControlButton;
      cell1.Control.ShowTooltipWhenDisabled = true;
      cell1.Control.Enabled = isActive;
      if (cell1.Control.Enabled)
        cell1.Control.SetToolTip(MySpaceTexts.BroadcastScreen_TerminalButton_ToolTip);
      else
        cell1.Control.SetToolTip(MySpaceTexts.BroadcastScreen_TerminalButtonDisabled_ToolTip);
      this.m_shipsData.Controls.Add(cell1.Control);
      return cell1;
    }

    private MyGuiControlTable.Cell CreateStatusIcons(
      MyTerminalPropertiesController.CubeGridInfo gridInfo,
      bool isActive,
      out string collectiveTooltip)
    {
      collectiveTooltip = string.Empty;
      MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell();
      float y = this.m_shipsData.RowHeight * 0.7f;
      int num;
      bool flag1 = (num = isActive ? 1 : 0) != 0;
      bool flag2 = num != 0;
      bool flag3 = num != 0;
      MyStringId myStringId1;
      MyStringId myStringId2 = myStringId1 = MyStringId.NullOrEmpty;
      StringBuilder stringBuilder = new StringBuilder();
      MyGuiControlParent guiControlParent = new MyGuiControlParent();
      guiControlParent.CanPlaySoundOnMouseOver = false;
      MyTerminalPropertiesController.MyRefuseReason terminal = this.CanTakeTerminal(gridInfo);
      MyTerminalPropertiesController.MyRefuseReason remoteStatus = gridInfo.RemoteStatus;
      switch (terminal)
      {
        case MyTerminalPropertiesController.MyRefuseReason.NoStableConnection:
          flag3 = false;
          myStringId2 = MySpaceTexts.BroadcastScreen_TerminalButton_NoStableConnectionToolTip;
          break;
        case MyTerminalPropertiesController.MyRefuseReason.NoProblem:
          myStringId2 = MySpaceTexts.BroadcastScreen_TerminalButton_StableConnectionToolTip;
          break;
        case MyTerminalPropertiesController.MyRefuseReason.PlayerBroadcastOff:
          flag3 = false;
          myStringId2 = MySpaceTexts.BroadcastScreen_TerminalButton_PlayerBroadcastOffToolTip;
          break;
        case MyTerminalPropertiesController.MyRefuseReason.Forbidden:
          flag3 = false;
          myStringId2 = MySpaceTexts.BroadcastScreen_NoOwnership;
          break;
      }
      MyGuiControlImage myGuiControlImage1 = new MyGuiControlImage(new Vector2?(new Vector2(-1.25f * y, 0.0f)), new Vector2?(new Vector2(y * 0.78f, y)), backgroundTexture: (flag3 ? "Textures\\GUI\\Icons\\BroadcastStatus\\AntennaOn.png" : "Textures\\GUI\\Icons\\BroadcastStatus\\AntennaOff.png"), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      myGuiControlImage1.SetToolTip(myStringId2);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlImage1);
      switch (remoteStatus)
      {
        case MyTerminalPropertiesController.MyRefuseReason.NoRemoteControl:
          myStringId1 = MySpaceTexts.BroadcastScreen_TakeControlButton_NoRemoteToolTip;
          flag1 = false;
          break;
        case MyTerminalPropertiesController.MyRefuseReason.NoMainRemoteControl:
          myStringId1 = MySpaceTexts.BroadcastScreen_TakeControlButton_NoMainRemoteControl;
          flag1 = false;
          break;
        case MyTerminalPropertiesController.MyRefuseReason.NoOwner:
        case MyTerminalPropertiesController.MyRefuseReason.NoProblem:
          myStringId1 = MySpaceTexts.BroadcastScreen_TakeControlButton_RemoteToolTip;
          break;
      }
      MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(new Vector2?(new Vector2(-0.25f * y, 0.0f)), new Vector2?(new Vector2(y * 0.78f, y)), backgroundTexture: (flag1 ? "Textures\\GUI\\Icons\\BroadcastStatus\\RemoteOn.png" : "Textures\\GUI\\Icons\\BroadcastStatus\\RemoteOff.png"), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      myGuiControlImage2.SetToolTip(myStringId1);
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlImage2);
      if ((terminal == MyTerminalPropertiesController.MyRefuseReason.NoStableConnection || terminal == MyTerminalPropertiesController.MyRefuseReason.PlayerBroadcastOff) && remoteStatus == MyTerminalPropertiesController.MyRefuseReason.NoRemoteControl)
      {
        stringBuilder.Append((object) MyTexts.Get(MySpaceTexts.BroadcastScreen_UnavailableControlButton));
        flag2 = false;
      }
      if (flag2 && (terminal == MyTerminalPropertiesController.MyRefuseReason.NoOwner || remoteStatus == MyTerminalPropertiesController.MyRefuseReason.Forbidden || (terminal == MyTerminalPropertiesController.MyRefuseReason.NoStableConnection || terminal == MyTerminalPropertiesController.MyRefuseReason.PlayerBroadcastOff)))
      {
        flag2 = false;
        stringBuilder.Append((object) MyTexts.Get(MySpaceTexts.BroadcastScreen_NoOwnership));
      }
      if (terminal == MyTerminalPropertiesController.MyRefuseReason.NoOwner)
      {
        stringBuilder.AppendLine();
        stringBuilder.Append((object) MyTexts.Get(MySpaceTexts.BroadcastScreen_Antenna));
      }
      if (remoteStatus == MyTerminalPropertiesController.MyRefuseReason.Forbidden)
      {
        stringBuilder.AppendLine();
        stringBuilder.Append((object) MyTexts.Get(MySpaceTexts.BroadcastScreen_RemoteControl));
      }
      if (flag2)
        stringBuilder.Append((object) MyTexts.Get(MySpaceTexts.BroadcastScreen_Ownership));
      MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage(new Vector2?(new Vector2(0.75f * y, 0.0f)), new Vector2?(new Vector2(y * 0.78f, y)), backgroundTexture: (flag2 ? "Textures\\GUI\\Icons\\BroadcastStatus\\KeyOn.png" : "Textures\\GUI\\Icons\\BroadcastStatus\\KeyOff.png"), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      myGuiControlImage3.SetToolTip(stringBuilder.ToString());
      guiControlParent.Controls.Add((MyGuiControlBase) myGuiControlImage3);
      cell.Control = (MyGuiControlBase) guiControlParent;
      this.m_shipsData.Controls.Add((MyGuiControlBase) guiControlParent);
      collectiveTooltip = string.Format("{0}{3}{1}\n{2}", (object) MyTexts.GetString(myStringId2), (object) MyTexts.GetString(myStringId1), (object) stringBuilder.ToString(), string.IsNullOrEmpty(myStringId2.ToString()) ? (object) "" : (object) "\n");
      return cell;
    }

    private HashSet<MyTerminalPropertiesController.CubeGridInfo> GetAllCubeGridsInfo()
    {
      HashSet<MyTerminalPropertiesController.CubeGridInfo> cubeGridInfoSet = new HashSet<MyTerminalPropertiesController.CubeGridInfo>();
      this.m_tmpGridInfoOutput.Clear();
      this.m_tmpBroadcasters.Clear();
      if (MySession.Static.LocalCharacter == null)
        return cubeGridInfoSet;
      foreach (MyDataBroadcaster relayedBroadcaster in MyAntennaSystem.Static.GetAllRelayedBroadcasters(this.m_interactedEntityRepresentative, MySession.Static.LocalPlayerId, false, this.m_tmpBroadcasters))
      {
        if (relayedBroadcaster != MySession.Static.LocalCharacter.RadioBroadcaster && relayedBroadcaster.ShowInTerminal)
        {
          double broadcasterDistance = this.GetPlayerBroadcasterDistance(relayedBroadcaster);
          MyTerminalPropertiesController.MyCubeGridConnectionStatus broadcasterStatus = this.GetBroadcasterStatus(relayedBroadcaster);
          MyTerminalPropertiesController.CubeGridInfo cubeGridInfo;
          if (this.m_tmpGridInfoOutput.TryGetValue(relayedBroadcaster.Info.EntityId, out cubeGridInfo))
          {
            if (cubeGridInfo.Status > broadcasterStatus)
              cubeGridInfo.Status = broadcasterStatus;
            if (cubeGridInfo.Distance > broadcasterDistance)
            {
              cubeGridInfo.Distance = broadcasterDistance;
              cubeGridInfo.AppendedDistance = new StringBuilder().AppendDecimal(broadcasterDistance, 0).Append(" m");
            }
            if (!cubeGridInfo.Owned && relayedBroadcaster.CanBeUsedByPlayer(MySession.Static.LocalPlayerId))
              cubeGridInfo.Owned = true;
          }
          else
            this.m_tmpGridInfoOutput.Add(relayedBroadcaster.Info.EntityId, new MyTerminalPropertiesController.CubeGridInfo()
            {
              EntityId = relayedBroadcaster.Info.EntityId,
              Distance = broadcasterDistance,
              AppendedDistance = new StringBuilder().AppendDecimal(broadcasterDistance, 0).Append(" m"),
              Name = relayedBroadcaster.Info.Name,
              Status = broadcasterStatus,
              Owned = relayedBroadcaster.CanBeUsedByPlayer(MySession.Static.LocalPlayerId),
              RemoteStatus = this.GetRemoteStatus(relayedBroadcaster),
              RemoteId = relayedBroadcaster.MainRemoteControlId
            });
        }
      }
      foreach (MyTerminalPropertiesController.CubeGridInfo cubeGridInfo in this.m_tmpGridInfoOutput.Values)
        cubeGridInfoSet.Add(cubeGridInfo);
      return cubeGridInfoSet;
    }

    private MyTerminalPropertiesController.MyCubeGridConnectionStatus GetBroadcasterStatus(
      MyDataBroadcaster broadcaster)
    {
      if (!MyAntennaSystem.Static.CheckConnection(broadcaster.Receiver, this.m_openInventoryInteractedEntityRepresentative, MySession.Static.LocalPlayerId, false))
        return MyTerminalPropertiesController.MyCubeGridConnectionStatus.OutOfBroadcastingRange;
      return !MyAntennaSystem.Static.CheckConnection(this.m_openInventoryInteractedEntityRepresentative, broadcaster, MySession.Static.LocalPlayerId, false) ? MyTerminalPropertiesController.MyCubeGridConnectionStatus.OutOfReceivingRange : MyTerminalPropertiesController.MyCubeGridConnectionStatus.Connected;
    }

    private MyTerminalPropertiesController.MyCubeGridConnectionStatus GetShipStatus(
      MyCubeGrid grid)
    {
      HashSet<MyDataBroadcaster> output = new HashSet<MyDataBroadcaster>();
      MyAntennaSystem.Static.GetEntityBroadcasters((MyEntity) grid, ref output, MySession.Static.LocalPlayerId);
      MyTerminalPropertiesController.MyCubeGridConnectionStatus connectionStatus = MyTerminalPropertiesController.MyCubeGridConnectionStatus.OutOfReceivingRange;
      foreach (MyDataBroadcaster broadcaster in output)
      {
        MyTerminalPropertiesController.MyCubeGridConnectionStatus broadcasterStatus = this.GetBroadcasterStatus(broadcaster);
        switch (broadcasterStatus)
        {
          case MyTerminalPropertiesController.MyCubeGridConnectionStatus.Connected:
            return broadcasterStatus;
          case MyTerminalPropertiesController.MyCubeGridConnectionStatus.OutOfBroadcastingRange:
            connectionStatus = broadcasterStatus;
            continue;
          default:
            continue;
        }
      }
      return connectionStatus;
    }

    private MyTerminalPropertiesController.MyRefuseReason GetRemoteStatus(
      MyDataBroadcaster broadcaster)
    {
      if (!broadcaster.HasRemoteControl)
        return MyTerminalPropertiesController.MyRefuseReason.NoRemoteControl;
      long? remoteControlOwner = broadcaster.MainRemoteControlOwner;
      if (!remoteControlOwner.HasValue)
        return MyTerminalPropertiesController.MyRefuseReason.NoMainRemoteControl;
      MyRelationsBetweenPlayers relationPlayerPlayer = MyIDModule.GetRelationPlayerPlayer(remoteControlOwner.Value, MySession.Static.LocalHumanPlayer.Identity.IdentityId);
      if (relationPlayerPlayer == MyRelationsBetweenPlayers.Self)
        return MyTerminalPropertiesController.MyRefuseReason.NoProblem;
      switch (broadcaster.MainRemoteControlSharing)
      {
        case MyOwnershipShareModeEnum.Faction:
          if (relationPlayerPlayer != MyRelationsBetweenPlayers.Allies)
            break;
          goto case MyOwnershipShareModeEnum.All;
        case MyOwnershipShareModeEnum.All:
          return MyTerminalPropertiesController.MyRefuseReason.NoProblem;
      }
      return remoteControlOwner.Value == 0L ? MyTerminalPropertiesController.MyRefuseReason.NoOwner : MyTerminalPropertiesController.MyRefuseReason.Forbidden;
    }

    private MyEntity GetInteractedEntityRepresentative(MyEntity controlledEntity) => controlledEntity is MyCubeBlock ? (MyEntity) MyAntennaSystem.Static.GetLogicalGroupRepresentative((controlledEntity as MyCubeBlock).CubeGrid) : (MyEntity) MySession.Static.LocalCharacter;

    private double GetPlayerBroadcasterDistance(MyDataBroadcaster broadcaster) => MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity != null ? Vector3D.Distance(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), broadcaster.BroadcastPosition) : double.MaxValue;

    private MyTerminalPropertiesController.MyRefuseReason CanTakeTerminal(
      MyTerminalPropertiesController.CubeGridInfo gridInfo)
    {
      if (!gridInfo.Owned)
        return MyTerminalPropertiesController.MyRefuseReason.NoOwner;
      if (gridInfo.Status == MyTerminalPropertiesController.MyCubeGridConnectionStatus.OutOfBroadcastingRange && MySession.Static.ControlledEntity.Entity is MyCharacter && !(MySession.Static.ControlledEntity.Entity as MyCharacter).RadioBroadcaster.Enabled)
        return MyTerminalPropertiesController.MyRefuseReason.PlayerBroadcastOff;
      return gridInfo.Status == MyTerminalPropertiesController.MyCubeGridConnectionStatus.OutOfBroadcastingRange || gridInfo.Status == MyTerminalPropertiesController.MyCubeGridConnectionStatus.OutOfReceivingRange ? MyTerminalPropertiesController.MyRefuseReason.NoStableConnection : MyTerminalPropertiesController.MyRefuseReason.NoProblem;
    }

    private void OnButtonClicked_TakeControl(MyGuiControlButton obj)
    {
      if (this.m_shipsData.SelectedRow == null)
        return;
      MyTerminalPropertiesController.UserData userData = (MyTerminalPropertiesController.UserData) this.m_shipsData.SelectedRow.UserData;
      if (!userData.IsSelectable || !userData.RemoteEntityId.HasValue)
        return;
      this.FindRemoteControlAndTakeControl(userData.GridEntityId, userData.RemoteEntityId.Value);
    }

    private void Menu_ButtonClicked(MyGuiControlButton button)
    {
      if (this.ButtonClicked == null)
        return;
      this.ButtonClicked();
    }

    private void OnButtonClicked_OpenTerminal(MyGuiControlButton obj)
    {
      MyGuiControlTable.EventArgs args;
      args.MouseButton = MyMouseButtonsEnum.None;
      args.RowIndex = -1;
      this.shipsData_ItemDoubleClicked((MyGuiControlTable) null, args);
    }

    private void shipsData_ItemDoubleClicked(
      MyGuiControlTable sender,
      MyGuiControlTable.EventArgs args)
    {
      if (this.m_shipsData.SelectedRow == null)
        return;
      MyTerminalPropertiesController.UserData userData = (MyTerminalPropertiesController.UserData) this.m_shipsData.SelectedRow.UserData;
      if (!userData.IsSelectable)
        return;
      this.OpenPropertiesByEntityId(userData.GridEntityId);
    }

    private void shipsData_ColumnClicked(MyGuiControlTable sender, int column) => this.m_columnToSort = column;

    private void shipsInRange_ItemSelected()
    {
      if (!this.m_shipsInRange.IsMouseOver && !this.m_shipsInRange.HasFocus || this.m_shipsInRange.GetSelectedKey() == this.m_interactedEntityRepresentative.EntityId)
        return;
      this.OpenPropertiesByEntityId(this.m_shipsInRange.GetSelectedKey());
    }

    private void OpenPropertiesByEntityId(long entityId)
    {
      MyEntity entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity);
      if (entity == null && !Sync.IsServer)
        MyGuiScreenTerminal.RequestReplicable(entityId, entityId, new Action<long>(this.OpenPropertiesByEntityId));
      else if (entity is MyCharacter)
      {
        MyGuiScreenTerminal.ChangeInteractedEntity((MyEntity) null, false);
      }
      else
      {
        if (entity == null || !(entity is MyCubeGrid))
          return;
        MyCubeGrid myCubeGrid = entity as MyCubeGrid;
        if (this.m_openInventoryInteractedEntityRepresentative == myCubeGrid && MySession.Static.LocalCharacter?.Parent != null)
        {
          MyGuiScreenTerminal.ChangeInteractedEntity(MySession.Static.LocalCharacter?.Parent, false);
        }
        else
        {
          if (!MyAntennaSystem.Static.CheckConnection((MyEntity) myCubeGrid, this.m_openInventoryInteractedEntityRepresentative, MySession.Static.LocalHumanPlayer))
            return;
          this.m_tmpAntennas.Clear();
          MyAntennaSystem.Static.GetEntityReceivers((MyEntity) myCubeGrid, ref this.m_tmpAntennas, MySession.Static.LocalPlayerId);
          foreach (MyDataReceiver tmpAntenna in this.m_tmpAntennas)
            tmpAntenna.UpdateBroadcastersInRange();
          if (this.m_tmpAntennas.Count <= 0)
            MyGuiScreenTerminal.ChangeInteractedEntity(MySession.Static.LocalCharacter?.Parent, false);
          else
            MyGuiScreenTerminal.ChangeInteractedEntity((MyEntity) (this.m_tmpAntennas.ElementAt<MyDataReceiver>(0).Entity as MyTerminalBlock), true);
        }
      }
    }

    private void FindRemoteControlAndTakeControl(long gridEntityId, long remoteEntityId)
    {
      MyRemoteControl entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyRemoteControl>(remoteEntityId, out entity);
      if (entity == null)
      {
        if (Sync.IsServer)
          return;
        MyGuiScreenTerminal.RequestReplicable(gridEntityId, remoteEntityId, (Action<long>) (x => this.FindRemoteControlAndTakeControl(gridEntityId, x)));
      }
      else
      {
        this.m_tmpAntennas.Clear();
        MyAntennaSystem.Static.GetEntityReceivers((MyEntity) entity, ref this.m_tmpAntennas, MySession.Static.LocalPlayerId);
        foreach (MyDataReceiver tmpAntenna in this.m_tmpAntennas)
          tmpAntenna.UpdateBroadcastersInRange();
        entity.RequestControl();
      }
    }

    public bool TestConnection()
    {
      if (this.m_openInventoryInteractedEntityRepresentative == null || this.m_interactedEntityRepresentative == null || MySession.Static == null)
        return false;
      if (this.m_openInventoryInteractedEntityRepresentative.EntityId == this.m_interactedEntityRepresentative.EntityId && !this.m_isRemote)
      {
        MyCharacter localCharacter = MySession.Static.LocalCharacter;
        if (this.m_interactedEntity != null && localCharacter != null)
          return this.m_interactedEntity.PositionComp.WorldAABB.DistanceSquared(localCharacter.PositionComp.GetPosition()) < (double) MyConstants.DEFAULT_INTERACTIVE_DISTANCE * (double) MyConstants.DEFAULT_INTERACTIVE_DISTANCE;
      }
      return !(this.m_interactedEntityRepresentative is MyCubeGrid) || this.GetShipStatus(this.m_interactedEntityRepresentative as MyCubeGrid) == MyTerminalPropertiesController.MyCubeGridConnectionStatus.Connected;
    }

    public void Close()
    {
      if (this.m_shipsInRange != null)
      {
        this.m_shipsInRange.ItemSelected -= new MyGuiControlCombobox.ItemSelectedDelegate(this.shipsInRange_ItemSelected);
        this.m_shipsInRange.ClearItems();
        this.m_shipsInRange = (MyGuiControlCombobox) null;
      }
      if (this.m_shipsData != null)
      {
        this.m_shipsData.ColumnClicked -= new Action<MyGuiControlTable, int>(this.shipsData_ColumnClicked);
        this.m_shipsData.Clear();
        this.m_shipsData = (MyGuiControlTable) null;
      }
      if (this.m_button == null)
        return;
      this.m_button.ButtonClicked -= new Action<MyGuiControlButton>(this.Menu_ButtonClicked);
      this.m_button = (MyGuiControlButton) null;
    }

    public void Update(bool isScreenActive)
    {
      this.m_cnt = ++this.m_cnt % 30;
      if (this.m_cnt != 0)
        return;
      if (this.m_previousMutualConnectionGrids == null)
        this.m_previousMutualConnectionGrids = MyAntennaSystem.Static.GetConnectedGridsInfo(this.m_openInventoryInteractedEntityRepresentative);
      if (this.m_previousShipInfo == null)
        this.m_previousShipInfo = this.GetAllCubeGridsInfo();
      HashSet<MyAntennaSystem.BroadcasterInfo> connectedGridsInfo = MyAntennaSystem.Static.GetConnectedGridsInfo(this.m_openInventoryInteractedEntityRepresentative);
      HashSet<MyTerminalPropertiesController.CubeGridInfo> allCubeGridsInfo = this.GetAllCubeGridsInfo();
      if (!this.m_previousMutualConnectionGrids.SetEquals((IEnumerable<MyAntennaSystem.BroadcasterInfo>) connectedGridsInfo))
      {
        this.PopulateMutuallyConnectedCubeGrids(connectedGridsInfo);
        this.m_previousMutualConnectionGrids = connectedGridsInfo;
      }
      if (!isScreenActive || this.m_previousShipInfo.SequenceEqual<MyTerminalPropertiesController.CubeGridInfo>((IEnumerable<MyTerminalPropertiesController.CubeGridInfo>) allCubeGridsInfo))
        return;
      this.PopulateOwnedCubeGrids(allCubeGridsInfo);
      this.m_previousShipInfo = allCubeGridsInfo;
    }

    public void HandleInput()
    {
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT) && this.m_shipsData.GetInnerControlsFromCurrentCell(3) is MyGuiControlButton controlsFromCurrentCell)
        controlsFromCurrentCell.PressButton();
      if (!MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X) || !(this.m_shipsData.GetInnerControlsFromCurrentCell(4) is MyGuiControlButton controlsFromCurrentCell))
        return;
      controlsFromCurrentCell.PressButton();
    }

    private enum MyCubeGridConnectionStatus
    {
      PhysicallyConnected,
      Connected,
      OutOfBroadcastingRange,
      OutOfReceivingRange,
      Me,
      IsPreviewGrid,
    }

    private enum MyRefuseReason
    {
      NoRemoteControl,
      NoMainRemoteControl,
      NoStableConnection,
      NoOwner,
      NoProblem,
      PlayerBroadcastOff,
      Forbidden,
    }

    private struct UserData
    {
      public long GridEntityId;
      public long? RemoteEntityId;
      public bool IsSelectable;
    }

    private class CubeGridInfo
    {
      public long EntityId;
      public double Distance;
      public string Name;
      public StringBuilder AppendedDistance;
      public MyTerminalPropertiesController.MyCubeGridConnectionStatus Status;
      public bool Owned;
      public MyTerminalPropertiesController.MyRefuseReason RemoteStatus;
      public long? RemoteId;

      public override bool Equals(object obj)
      {
        if (!(obj is MyTerminalPropertiesController.CubeGridInfo))
          return false;
        MyTerminalPropertiesController.CubeGridInfo cubeGridInfo = obj as MyTerminalPropertiesController.CubeGridInfo;
        string str1 = this.Name == null ? "" : this.Name;
        string str2 = cubeGridInfo.Name == null ? "" : cubeGridInfo.Name;
        return this.EntityId.Equals(cubeGridInfo.EntityId) && str1.Equals(str2) && this.AppendedDistance.Equals(cubeGridInfo.AppendedDistance) && this.Status == cubeGridInfo.Status;
      }

      public override int GetHashCode() => (int) ((MyTerminalPropertiesController.MyCubeGridConnectionStatus) (((this.EntityId.GetHashCode() * 397 ^ (this.Name == null ? "" : this.Name).GetHashCode()) * 397 ^ this.AppendedDistance.GetHashCode()) * 397) ^ this.Status);
    }
  }
}
