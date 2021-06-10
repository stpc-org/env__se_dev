// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalInfoController
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Input;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  internal class MyTerminalInfoController : MyTerminalController
  {
    private static MyGuiControlTabPage m_infoPage;
    private static MyCubeGrid m_grid;
    private static List<MyBlockLimits.MyGridLimitData> m_infoGrids = new List<MyBlockLimits.MyGridLimitData>();
    private static List<MyPlayer.PlayerId> m_playerIds = new List<MyPlayer.PlayerId>();
    private static bool m_controlsDirty;
    private MyGuiControlButton m_convertToShipBtn;
    private MyGuiControlButton m_convertToStationBtn;
    private bool m_gamepadHelpDirty;

    internal void Close()
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(MySession.Static.LocalPlayerId);
      if (identity != null)
        identity.BlockLimits.BlockLimitsChanged -= new Action(this.grid_OnAuthorshipChanged);
      Sandbox.Game.Entities.MyEntities.OnEntityDelete -= new Action<MyEntity>(this.grid_OnClose);
      MySessionComponentSafeZones.OnSafeZoneUpdated -= new Action<MySafeZone>(this.OnSafeZoneUpdated);
      if (MyTerminalInfoController.m_infoPage != null)
      {
        MyGuiControlButton controlByName1 = (MyGuiControlButton) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("ConvertBtn");
        if (controlByName1 != null)
          controlByName1.ButtonClicked -= new Action<MyGuiControlButton>(this.convertBtn_ButtonClicked);
        MyGuiControlButton controlByName2 = (MyGuiControlButton) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("ConvertToStationBtn");
        if (controlByName2 != null)
          controlByName2.ButtonClicked -= new Action<MyGuiControlButton>(this.convertToStationBtn_ButtonClicked);
        MyTerminalInfoController.m_infoPage = (MyGuiControlTabPage) null;
      }
      if (MyTerminalInfoController.m_grid == null)
        return;
      MyTerminalInfoController.m_grid.OnBlockAdded -= new Action<MySlimBlock>(this.grid_OnBlockAdded);
      MyTerminalInfoController.m_grid.OnBlockRemoved -= new Action<MySlimBlock>(this.grid_OnBlockRemoved);
      MyTerminalInfoController.m_grid.OnPhysicsChanged -= new Action<MyEntity>(this.grid_OnPhysicsChanged);
      MyTerminalInfoController.m_grid.OnBlockOwnershipChanged -= new Action<MyCubeGrid>(this.grid_OnBlockOwnershipChanged);
      MyTerminalInfoController.m_grid.PositionComp.OnPositionChanged -= new Action<MyPositionComponentBase>(this.OnGridPositionChanged);
      MyTerminalInfoController.m_grid = (MyCubeGrid) null;
    }

    internal void Init(MyGuiControlTabPage infoPage, MyCubeGrid grid)
    {
      MyTerminalInfoController.m_grid = grid;
      MyTerminalInfoController.m_infoPage = infoPage;
      MyTerminalInfoController.m_playerIds.Clear();
      MyTerminalInfoController.m_controlsDirty = false;
      MySession.Static.Players.TryGetIdentity(MySession.Static.LocalPlayerId).BlockLimits.BlockLimitsChanged += new Action(this.grid_OnAuthorshipChanged);
      MyTerminalInfoController.RecreateControls();
      Sandbox.Game.Entities.MyEntities.OnEntityDelete += new Action<MyEntity>(this.grid_OnClose);
      if (grid == null)
        return;
      grid.OnBlockAdded += new Action<MySlimBlock>(this.grid_OnBlockAdded);
      grid.OnBlockRemoved += new Action<MySlimBlock>(this.grid_OnBlockRemoved);
      grid.OnPhysicsChanged += new Action<MyEntity>(this.grid_OnPhysicsChanged);
      grid.OnBlockOwnershipChanged += new Action<MyCubeGrid>(this.grid_OnBlockOwnershipChanged);
      grid.PositionComp.OnPositionChanged += new Action<MyPositionComponentBase>(this.OnGridPositionChanged);
      if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
      {
        MyGuiControlButton controlByName = (MyGuiControlButton) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("RenameShipButton");
        if (controlByName != null)
          controlByName.ButtonClicked += new Action<MyGuiControlButton>(this.renameBtn_ButtonClicked);
      }
      MyGuiControlButton controlByName1 = (MyGuiControlButton) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("ConvertBtn");
      if (controlByName1 != null)
        controlByName1.ButtonClicked += new Action<MyGuiControlButton>(this.convertBtn_ButtonClicked);
      MyGuiControlButton controlByName2 = (MyGuiControlButton) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("ConvertToStationBtn");
      if (controlByName2 != null)
      {
        controlByName2.ButtonClicked += new Action<MyGuiControlButton>(this.convertToStationBtn_ButtonClicked);
        controlByName2.Enabled = MySessionComponentSafeZones.IsActionAllowed((MyEntity) MyTerminalInfoController.m_grid, MySafeZoneAction.ConvertToStation);
      }
      MySessionComponentSafeZones.OnSafeZoneUpdated += new Action<MySafeZone>(this.OnSafeZoneUpdated);
    }

    private static void RecreateControls()
    {
      if (MyTerminalInfoController.m_infoPage == null)
        return;
      MyTerminalInfoController.m_controlsDirty = true;
    }

    public override void UpdateBeforeDraw(MyGuiScreenBase screen)
    {
      if (this.m_gamepadHelpDirty)
        this.UpdateGamepadHelp(screen);
      if (!MyTerminalInfoController.m_controlsDirty)
        return;
      MyTerminalInfoController.m_controlsDirty = false;
      MyGuiControlCheckbox controlByName1 = (MyGuiControlCheckbox) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("CenterBtn");
      if (MyFakes.ENABLE_CENTER_OF_MASS)
      {
        controlByName1.IsChecked = MyCubeGrid.ShowCenterOfMass;
        controlByName1.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.centerBtn_IsCheckedChanged);
        MyGuiControlCheckbox controlByName2 = (MyGuiControlCheckbox) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("PivotBtn");
        controlByName2.IsChecked = MyCubeGrid.ShowGridPivot;
        controlByName2.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.pivotBtn_IsCheckedChanged);
      }
      MyGuiControlCheckbox controlByName3 = (MyGuiControlCheckbox) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("ShowGravityGizmo");
      controlByName3.IsChecked = MyCubeGrid.ShowGravityGizmos;
      controlByName3.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.showGravityGizmos_IsCheckedChanged);
      MyGuiControlCheckbox controlByName4 = (MyGuiControlCheckbox) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("ShowSenzorGizmo");
      controlByName4.IsChecked = MyCubeGrid.ShowSenzorGizmos;
      controlByName4.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.showSenzorGizmos_IsCheckedChanged);
      MyGuiControlCheckbox controlByName5 = (MyGuiControlCheckbox) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("ShowAntenaGizmo");
      controlByName5.IsChecked = MyCubeGrid.ShowAntennaGizmos;
      controlByName5.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.showAntenaGizmos_IsCheckedChanged);
      MyGuiControlSlider controlByName6 = (MyGuiControlSlider) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("FriendAntennaRange");
      controlByName6.Value = MyHudMarkerRender.FriendAntennaRange;
      controlByName6.ValueChanged += (Action<MyGuiControlSlider>) (s => MyHudMarkerRender.FriendAntennaRange = s.Value);
      controlByName6.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_FriendlyAntennaRange_ToolTip));
      MyGuiControlSlider controlByName7 = (MyGuiControlSlider) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("EnemyAntennaRange");
      controlByName7.Value = MyHudMarkerRender.EnemyAntennaRange;
      controlByName7.ValueChanged += (Action<MyGuiControlSlider>) (s => MyHudMarkerRender.EnemyAntennaRange = s.Value);
      controlByName7.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_EnemyAntennaRange_ToolTip));
      MyGuiControlSlider controlByName8 = (MyGuiControlSlider) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("OwnedAntennaRange");
      controlByName8.Value = MyHudMarkerRender.OwnerAntennaRange;
      controlByName8.ValueChanged += (Action<MyGuiControlSlider>) (s => MyHudMarkerRender.OwnerAntennaRange = s.Value);
      controlByName8.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_OwnedAntennaRange_ToolTip));
      MyGuiControlTextbox controlByName9 = (MyGuiControlTextbox) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("RenameShipText");
      controlByName9.MaxLength = 64;
      if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
      {
        bool flag = this.IsPlayerOwner(MyTerminalInfoController.m_grid);
        MyGuiControlLabel controlByName2 = (MyGuiControlLabel) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("RenameShipLabel");
        MyGuiControlButton controlByName10 = (MyGuiControlButton) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("RenameShipButton");
        if (controlByName9 != null)
        {
          if (MyTerminalInfoController.m_grid != null)
            controlByName9.Text = MyTerminalInfoController.m_grid.DisplayName;
          controlByName9.Enabled = flag;
        }
        controlByName2.Enabled = flag;
        int num = flag ? 1 : 0;
        controlByName10.Enabled = num != 0;
      }
      this.m_convertToShipBtn = (MyGuiControlButton) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("ConvertBtn");
      this.m_convertToStationBtn = (MyGuiControlButton) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("ConvertToStationBtn");
      MyGuiControlList controlByName11 = (MyGuiControlList) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("InfoList");
      controlByName11.Controls.Clear();
      MyGuiControlCheckbox controlByName12 = (MyGuiControlCheckbox) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("SetDestructibleBlocks");
      controlByName12.Visible = MySession.Static.Settings.ScenarioEditMode || MySession.Static.IsScenario;
      controlByName12.Enabled = MySession.Static.Settings.ScenarioEditMode;
      if (MyTerminalInfoController.m_grid == null || MyTerminalInfoController.m_grid.Physics == null)
      {
        this.m_convertToShipBtn.Enabled = false;
        this.m_convertToStationBtn.Enabled = false;
        ((MyGuiControlLabel) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("Infolabel")).Text = MyTexts.GetString(MySpaceTexts.TerminalTab_Info_Overview);
        MyTerminalInfoController.RequestServerLimitInfo(MySession.Static.LocalPlayerId);
      }
      else
      {
        this.UpdateConvertButtons();
        controlByName12.IsChecked = MyTerminalInfoController.m_grid.DestructibleBlocks;
        controlByName12.IsCheckedChanged = new Action<MyGuiControlCheckbox>(this.setDestructibleBlocks_IsCheckedChanged);
        int number1 = 0;
        if (MyTerminalInfoController.m_grid.BlocksCounters.ContainsKey((MyObjectBuilderType) typeof (MyObjectBuilder_GravityGenerator)))
          number1 = MyTerminalInfoController.m_grid.BlocksCounters[(MyObjectBuilderType) typeof (MyObjectBuilder_GravityGenerator)];
        int number2 = 0;
        if (MyTerminalInfoController.m_grid.BlocksCounters.ContainsKey((MyObjectBuilderType) typeof (MyObjectBuilder_VirtualMass)))
          number2 = MyTerminalInfoController.m_grid.BlocksCounters[(MyObjectBuilderType) typeof (MyObjectBuilder_VirtualMass)];
        int number3 = 0;
        if (MyTerminalInfoController.m_grid.BlocksCounters.ContainsKey((MyObjectBuilderType) typeof (MyObjectBuilder_InteriorLight)))
          number3 = MyTerminalInfoController.m_grid.BlocksCounters[(MyObjectBuilderType) typeof (MyObjectBuilder_InteriorLight)];
        int number4 = 0;
        foreach (MyObjectBuilderType key in MyTerminalInfoController.m_grid.BlocksCounters.Keys)
        {
          Type producedType = MyCubeBlockFactory.GetProducedType(key);
          if (typeof (IMyConveyorSegmentBlock).IsAssignableFrom(producedType) || typeof (IMyConveyorEndpointBlock).IsAssignableFrom(producedType))
            number4 += MyTerminalInfoController.m_grid.BlocksCounters[key];
        }
        int number5 = 0;
        foreach (MySlimBlock block in MyTerminalInfoController.m_grid.GetBlocks())
        {
          if (block.FatBlock != null)
            number5 += block.FatBlock.Model.GetTrianglesCount();
        }
        foreach (MyCubeGridRenderCell cubeGridRenderCell in (IEnumerable<MyCubeGridRenderCell>) MyTerminalInfoController.m_grid.RenderData.Cells.Values)
        {
          foreach (KeyValuePair<MyCubePart, ConcurrentDictionary<uint, bool>> cubePart in cubeGridRenderCell.CubeParts)
            number5 += cubePart.Key.Model.GetTrianglesCount();
        }
        int number6 = 0;
        MyEntityThrustComponent entityThrustComponent = MyTerminalInfoController.m_grid.Components.Get<MyEntityThrustComponent>();
        if (entityThrustComponent != null)
          number6 = entityThrustComponent.ThrustCount;
        MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(text: new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Thrusters)).AppendInt32(number6).ToString());
        MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(text: new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Triangles)).AppendInt32(number5).ToString());
        myGuiControlLabel2.SetToolTip(MySpaceTexts.TerminalTab_Info_TrianglesTooltip);
        MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Blocks)).AppendInt32(MyTerminalInfoController.m_grid.GetBlocks().Count).ToString());
        myGuiControlLabel3.SetToolTip(MySpaceTexts.TerminalTab_Info_BlocksTooltip);
        MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(text: new StringBuilder().AppendStringBuilder(new StringBuilder("PCU: ")).AppendInt32(MyTerminalInfoController.m_grid.BlocksPCU).ToString());
        myGuiControlLabel3.SetToolTip(MySpaceTexts.TerminalTab_Info_BlocksTooltip);
        MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(text: new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_NonArmor)).AppendInt32(MyTerminalInfoController.m_grid.Hierarchy.Children.Count).ToString());
        MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel(text: new StringBuilder().Clear().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Lights)).AppendInt32(number3).ToString());
        MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel(text: new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Reflectors)).AppendInt32(MyTerminalInfoController.m_grid.GridSystems.ReflectorLightSystem.ReflectorCount).ToString());
        MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel(text: new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_GravGens)).AppendInt32(number1).ToString());
        MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(text: new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_VirtualMass)).AppendInt32(number2).ToString());
        MyGuiControlLabel myGuiControlLabel10 = new MyGuiControlLabel(text: new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Conveyors)).AppendInt32(number4).ToString());
        MyGuiControlLabel myGuiControlLabel11 = new MyGuiControlLabel(text: new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.TerminalTab_Info_GridMass)).AppendInt32(MyTerminalInfoController.m_grid.GetCurrentMass()).ToString());
        MyGuiControlLabel myGuiControlLabel12 = new MyGuiControlLabel(text: string.Format(MyTexts.Get(MySpaceTexts.TerminalTab_Info_Shapes).ToString(), (object) MyTerminalInfoController.m_grid.ShapeCount, (object) 65536));
        controlByName11.InitControls((IEnumerable<MyGuiControlBase>) new MyGuiControlBase[12]
        {
          (MyGuiControlBase) myGuiControlLabel3,
          (MyGuiControlBase) myGuiControlLabel5,
          (MyGuiControlBase) myGuiControlLabel4,
          (MyGuiControlBase) myGuiControlLabel10,
          (MyGuiControlBase) myGuiControlLabel1,
          (MyGuiControlBase) myGuiControlLabel6,
          (MyGuiControlBase) myGuiControlLabel7,
          (MyGuiControlBase) myGuiControlLabel8,
          (MyGuiControlBase) myGuiControlLabel9,
          (MyGuiControlBase) myGuiControlLabel2,
          (MyGuiControlBase) myGuiControlLabel11,
          (MyGuiControlBase) myGuiControlLabel12
        });
        this.UpdateGamepadHelp(screen);
      }
    }

    private void setDestructibleBlocks_IsCheckedChanged(MyGuiControlCheckbox obj) => MyTerminalInfoController.m_grid.DestructibleBlocks = obj.IsChecked;

    public void MarkControlsDirty() => MyTerminalInfoController.m_controlsDirty = true;

    public static void RequestServerLimitInfo(long identityId) => MyMultiplayer.RaiseStaticEvent<long, ulong>((Func<IMyEventOwner, Action<long, ulong>>) (x => new Action<long, ulong>(MyTerminalInfoController.ServerLimitInfo_Implementation)), identityId, MySession.Static.LocalHumanPlayer.Id.SteamId);

    [Event(null, 341)]
    [Reliable]
    [Server]
    public static void ServerLimitInfo_Implementation(long identityId, ulong clientId)
    {
      if (MySession.Static == null)
        return;
      List<MyTerminalInfoController.GridBuiltByIdInfo> gridBuiltByIdInfoList = new List<MyTerminalInfoController.GridBuiltByIdInfo>();
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      if (identity != null)
      {
        foreach (KeyValuePair<long, MyBlockLimits.MyGridLimitData> keyValuePair in identity.BlockLimits.BlocksBuiltByGrid)
        {
          HashSet<MySlimBlock> source = (HashSet<MySlimBlock>) null;
          MyCubeGrid entity;
          if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(keyValuePair.Key, out entity))
          {
            source = entity.FindBlocksBuiltByID(identity.IdentityId);
            if (!source.Any<MySlimBlock>())
              continue;
          }
          MyTerminalInfoController.GridBuiltByIdInfo gridBuiltByIdInfo = new MyTerminalInfoController.GridBuiltByIdInfo()
          {
            GridName = keyValuePair.Value.GridName,
            EntityId = keyValuePair.Key,
            UnsafeBlocks = new List<string>()
          };
          if (source != null)
          {
            gridBuiltByIdInfo.BlockCount = source.Count;
            gridBuiltByIdInfo.PCUBuilt = source.Sum<MySlimBlock>((Func<MySlimBlock, int>) (x => x.BlockDefinition.PCU));
          }
          if (MyUnsafeGridsSessionComponent.UnsafeGrids.TryGetValue(keyValuePair.Key, out entity))
          {
            foreach (MyCubeBlock unsafeBlock in entity.UnsafeBlocks)
              gridBuiltByIdInfo.UnsafeBlocks.Add(unsafeBlock.DisplayNameText);
          }
          gridBuiltByIdInfoList.Add(gridBuiltByIdInfo);
        }
      }
      MyMultiplayer.RaiseStaticEvent<List<MyTerminalInfoController.GridBuiltByIdInfo>>((Func<IMyEventOwner, Action<List<MyTerminalInfoController.GridBuiltByIdInfo>>>) (x => new Action<List<MyTerminalInfoController.GridBuiltByIdInfo>>(MyTerminalInfoController.ServerLimitInfo_Received)), gridBuiltByIdInfoList, new EndpointId(clientId));
    }

    [Event(null, 396)]
    [Reliable]
    [Client]
    private static void ServerLimitInfo_Received(
      List<MyTerminalInfoController.GridBuiltByIdInfo> gridsWithBuiltById)
    {
      if (MyTerminalInfoController.m_infoPage == null)
        return;
      MyGuiControlList controlByName = (MyGuiControlList) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("InfoList");
      if (controlByName == null)
        return;
      controlByName.Controls.Clear();
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(MySession.Static.LocalPlayerId);
      if (identity == null)
        return;
      if (MySession.Static.MaxBlocksPerPlayer > 0)
      {
        MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(text: string.Format("{0} {1}/{2} {3}", (object) MyTexts.Get(MySpaceTexts.TerminalTab_Info_YouBuilt), (object) identity.BlockLimits.BlocksBuilt, (object) identity.BlockLimits.MaxBlocks, (object) MyTexts.Get(MySpaceTexts.TerminalTab_Info_BlocksLower)));
        controlByName.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      }
      foreach (KeyValuePair<string, short> blockTypeLimit in MySession.Static.BlockTypeLimits)
      {
        MyBlockLimits.MyTypeLimitData myTypeLimitData;
        identity.BlockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit.Key, out myTypeLimitData);
        MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.TryGetDefinitionGroup(blockTypeLimit.Key);
        if (definitionGroup != null && myTypeLimitData != null)
        {
          MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(text: string.Format("{0} {1}/{2} {3}", (object) MyTexts.Get(MySpaceTexts.TerminalTab_Info_YouBuilt), (object) myTypeLimitData.BlocksBuilt, (object) MySession.Static.GetBlockTypeLimit(blockTypeLimit.Key), (object) definitionGroup.Any.DisplayNameText));
          controlByName.Controls.Add((MyGuiControlBase) myGuiControlLabel);
        }
      }
      MyTerminalInfoController.m_infoGrids.Clear();
      if (gridsWithBuiltById == null)
        return;
      foreach (MyTerminalInfoController.GridBuiltByIdInfo gridBuiltByIdInfo in gridsWithBuiltById)
      {
        MyGuiControlParent guiControlParent1 = new MyGuiControlParent();
        bool flag = gridBuiltByIdInfo.UnsafeBlocks.Count > 0;
        guiControlParent1.Size = new Vector2(guiControlParent1.Size.X, 0.1f);
        if (MyTerminalInfoController.m_infoGrids.Count == 0)
        {
          MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
          controlSeparatorList.AddHorizontal(new Vector2(-0.15f, -0.052f), 0.3f, 1f / 500f);
          guiControlParent1.Controls.Add((MyGuiControlBase) controlSeparatorList);
        }
        string text1 = gridBuiltByIdInfo.GridName;
        if (text1 != null && text1.Length >= 16)
        {
          string str = text1.Substring(0, 15);
          text1 = str.Insert(str.Length, "...");
        }
        MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(text: text1, textScale: 0.7005405f);
        MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(text: string.Format("{0} {1} ({2} PCU)", (object) gridBuiltByIdInfo.BlockCount, (object) MyTexts.Get(MySpaceTexts.TerminalTab_Info_BlocksLower), (object) gridBuiltByIdInfo.PCUBuilt), textScale: 0.7005405f);
        MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.TerminalTab_Info_Assign), textScale: 0.7005405f, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
        MyGuiControlCombobox assignCombobox = new MyGuiControlCombobox(size: new Vector2?(new Vector2(0.11f, 0.008f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
        MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
        myGuiControlLabel1.Position = new Vector2(-0.12f, -0.025f);
        myGuiControlLabel2.Position = new Vector2(-0.12f, 0.0f);
        myGuiControlLabel3.Position = new Vector2(0.0f, 0.035f);
        assignCombobox.Position = new Vector2(0.121f, 0.055f);
        MyTerminalInfoController.GridBuiltByIdInfo gridSelected = gridBuiltByIdInfo;
        assignCombobox.ItemSelected += (MyGuiControlCombobox.ItemSelectedDelegate) (() => MyTerminalInfoController.assignCombobox_ItemSelected(identity, gridSelected.EntityId, MyTerminalInfoController.m_playerIds[(int) assignCombobox.GetSelectedKey()]));
        MyTerminalInfoController.m_playerIds.Clear();
        foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
        {
          if (MySession.Static.LocalHumanPlayer != onlinePlayer)
          {
            assignCombobox.AddItem((long) MyTerminalInfoController.m_playerIds.Count, onlinePlayer.DisplayName);
            MyTerminalInfoController.m_playerIds.Add(onlinePlayer.Id);
          }
        }
        if (MySession.Static.Settings.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
        {
          assignCombobox.Enabled = false;
          assignCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.Terminal_AuthorshipNotAvailable));
          assignCombobox.ShowTooltipWhenDisabled = true;
        }
        else if (assignCombobox.GetItemsCount() == 0)
          assignCombobox.Enabled = false;
        controlSeparatorList1.AddHorizontal(new Vector2(-0.15f, 0.063f), 0.3f, flag ? 1f / 500f : 3f / 1000f);
        guiControlParent1.Controls.Add((MyGuiControlBase) myGuiControlLabel1);
        guiControlParent1.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
        guiControlParent1.Controls.Add((MyGuiControlBase) myGuiControlLabel3);
        guiControlParent1.Controls.Add((MyGuiControlBase) assignCombobox);
        guiControlParent1.Controls.Add((MyGuiControlBase) controlSeparatorList1);
        if (MySession.Static.EnableRemoteBlockRemoval)
        {
          MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.buttonRemove), textScale: 0.7005405f, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
          Vector2? position = new Vector2?();
          Vector2? size = new Vector2?();
          Vector4? colorMask = new Vector4?();
          StringBuilder stringBuilder = new StringBuilder("X");
          Action<MyGuiControlButton> action = new Action<MyGuiControlButton>(MyTerminalInfoController.deleteBuiltByIdBlocksButton_ButtonClicked);
          int? nullable = new int?(MyTerminalInfoController.m_infoGrids.Count);
          string toolTip = MyTexts.GetString(MySpaceTexts.TerminalTab_Info_RemoveGrid);
          StringBuilder text2 = stringBuilder;
          Action<MyGuiControlButton> onButtonClick = action;
          int? buttonIndex = nullable;
          MyGuiControlButton guiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.SquareSmall, size, colorMask, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, toolTip, text2, onButtonClick: onButtonClick, buttonIndex: buttonIndex);
          myGuiControlLabel4.Position = new Vector2(0.082f, -0.02f);
          guiControlButton.Position = new Vector2(0.1215f, -0.02f);
          guiControlParent1.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
          guiControlParent1.Controls.Add((MyGuiControlBase) guiControlButton);
        }
        if (identity.BlockLimits.BlocksBuiltByGrid.ContainsKey(gridBuiltByIdInfo.EntityId))
          MyTerminalInfoController.m_infoGrids.Add(identity.BlockLimits.BlocksBuiltByGrid[gridBuiltByIdInfo.EntityId]);
        else if (MySession.Static.Settings.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
          MyTerminalInfoController.m_infoGrids.Add(new MyBlockLimits.MyGridLimitData()
          {
            EntityId = gridBuiltByIdInfo.EntityId,
            GridName = gridBuiltByIdInfo.GridName
          });
        controlByName.Controls.Add((MyGuiControlBase) guiControlParent1);
        if (flag)
        {
          MyGuiControlMultilineText controlMultilineText1 = new MyGuiControlMultilineText();
          controlMultilineText1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          controlMultilineText1.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
          controlMultilineText1.TextScale = 0.7f;
          controlMultilineText1.TextColor = Color.Red;
          MyGuiControlMultilineText controlMultilineText2 = controlMultilineText1;
          myGuiControlLabel1.ColorMask = (Vector4) Color.Red;
          myGuiControlLabel2.ColorMask = (Vector4) Color.Red;
          StringBuilder text2 = controlMultilineText2.Text;
          text2.AppendLine(MyTexts.GetString(MyCommonTexts.ScreenTerminalInfo_UnsafeBlocks));
          foreach (string unsafeBlock in gridBuiltByIdInfo.UnsafeBlocks)
            text2.AppendLine(unsafeBlock);
          controlMultilineText2.RefreshText(false);
          controlMultilineText2.Size = new Vector2(1f, controlMultilineText2.TextSize.Y);
          MyGuiControlParent guiControlParent2 = new MyGuiControlParent();
          guiControlParent2.Size = new Vector2(1f, controlMultilineText2.TextSize.Y - 0.01f);
          MyGuiControlParent guiControlParent3 = guiControlParent2;
          guiControlParent3.Controls.Add((MyGuiControlBase) controlMultilineText2);
          controlByName.Controls.Add((MyGuiControlBase) guiControlParent3);
          controlMultilineText2.PositionX -= 0.12f;
          controlMultilineText2.PositionY -= (float) ((double) guiControlParent3.Size.Y / 2.0 - 0.0120000001043081);
          MyGuiControlParent guiControlParent4 = new MyGuiControlParent();
          guiControlParent4.Size = new Vector2(1f, 0.02f);
          MyGuiControlParent guiControlParent5 = guiControlParent4;
          MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
          controlSeparatorList2.AddHorizontal(new Vector2(-0.15f, 0.0f), 0.3f, 3f / 1000f);
          guiControlParent5.Controls.Add((MyGuiControlBase) controlSeparatorList2);
          controlByName.Controls.Add((MyGuiControlBase) guiControlParent5);
        }
      }
    }

    private void UpdateGamepadHelp(MyGuiScreenBase screen)
    {
      this.m_gamepadHelpDirty = false;
      screen.GamepadHelpTextId = !this.m_convertToShipBtn.Enabled ? (!this.m_convertToStationBtn.Enabled ? new MyStringId?(MySpaceTexts.TerminalInfo_Help_Screen) : new MyStringId?(MySpaceTexts.TerminalInfo_Help_ScreenConvertStation)) : new MyStringId?(MySpaceTexts.TerminalInfo_Help_ScreenConvertShip);
      screen.UpdateGamepadHelp(screen.FocusedControl);
    }

    private void UpdateConvertButtons()
    {
      if (this.m_convertToShipBtn == null || this.m_convertToStationBtn == null)
        return;
      if (MyTerminalInfoController.m_grid == null || MyTerminalInfoController.m_grid.Physics == null || (double) MyTerminalInfoController.m_grid.Physics.AngularVelocity.LengthSquared() > 0.0001 || (double) MyTerminalInfoController.m_grid.Physics.LinearVelocity.LengthSquared() > 0.0001)
      {
        this.m_convertToShipBtn.Enabled = false;
        this.m_convertToStationBtn.Enabled = false;
      }
      else
      {
        if (!MyTerminalInfoController.m_grid.IsStatic)
        {
          this.m_convertToShipBtn.Enabled = false;
          this.m_convertToStationBtn.Enabled = MySessionComponentSafeZones.IsActionAllowed((MyEntity) MyTerminalInfoController.m_grid, MySafeZoneAction.ConvertToStation, user: Sync.MyId);
        }
        else
        {
          this.m_convertToShipBtn.Enabled = true;
          this.m_convertToStationBtn.Enabled = false;
        }
        if (MyTerminalInfoController.m_grid.GridSizeEnum == MyCubeSize.Small)
          this.m_convertToStationBtn.Enabled = false;
        if (!MyTerminalInfoController.m_grid.BigOwners.Contains(MySession.Static.LocalPlayerId) && !MySession.Static.IsUserSpaceMaster(Sync.MyId))
        {
          this.m_convertToShipBtn.Enabled = false;
          this.m_convertToStationBtn.Enabled = false;
        }
        this.m_gamepadHelpDirty = true;
      }
    }

    private bool IsPlayerOwner(MyCubeGrid grid) => grid != null && grid.BigOwners.Contains(MySession.Static.LocalPlayerId);

    private void showAntenaGizmos_IsCheckedChanged(MyGuiControlCheckbox obj)
    {
      MyCubeGrid.ShowAntennaGizmos = obj.IsChecked;
      Sandbox.Game.Entities.MyEntities.GetEntities().Where<MyEntity>((Func<MyEntity, bool>) (x => x is MyCubeGrid)).Cast<MyCubeGrid>().ForEach<MyCubeGrid>((Action<MyCubeGrid>) (x => x.MarkForDraw()));
    }

    private void showSenzorGizmos_IsCheckedChanged(MyGuiControlCheckbox obj)
    {
      MyCubeGrid.ShowSenzorGizmos = obj.IsChecked;
      Sandbox.Game.Entities.MyEntities.GetEntities().Where<MyEntity>((Func<MyEntity, bool>) (x => x is MyCubeGrid)).Cast<MyCubeGrid>().ForEach<MyCubeGrid>((Action<MyCubeGrid>) (x => x.MarkForDraw()));
    }

    private void showGravityGizmos_IsCheckedChanged(MyGuiControlCheckbox obj)
    {
      MyCubeGrid.ShowGravityGizmos = obj.IsChecked;
      Sandbox.Game.Entities.MyEntities.GetEntities().Where<MyEntity>((Func<MyEntity, bool>) (x => x is MyCubeGrid)).Cast<MyCubeGrid>().ForEach<MyCubeGrid>((Action<MyCubeGrid>) (x => x.MarkForDraw()));
    }

    private void centerBtn_IsCheckedChanged(MyGuiControlCheckbox obj)
    {
      MyCubeGrid.ShowCenterOfMass = obj.IsChecked;
      Sandbox.Game.Entities.MyEntities.GetEntities().Where<MyEntity>((Func<MyEntity, bool>) (x => x is MyCubeGrid)).Cast<MyCubeGrid>().ForEach<MyCubeGrid>((Action<MyCubeGrid>) (x => x.MarkForDraw()));
    }

    private void pivotBtn_IsCheckedChanged(MyGuiControlCheckbox obj)
    {
      MyCubeGrid.ShowGridPivot = obj.IsChecked;
      Sandbox.Game.Entities.MyEntities.GetEntities().Where<MyEntity>((Func<MyEntity, bool>) (x => x is MyCubeGrid)).Cast<MyCubeGrid>().ForEach<MyCubeGrid>((Action<MyCubeGrid>) (x => x.MarkForDraw()));
    }

    private void setDestructibleBlocksBtn_IsCheckedChanged(MyGuiControlCheckbox obj) => MyTerminalInfoController.m_grid.DestructibleBlocks = obj.IsChecked;

    private void convertBtn_Fail() => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextConvertToShipFail), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), canHideOthers: false));

    private void convertBtn_ButtonClicked(MyGuiControlButton obj) => MyTerminalInfoController.m_grid.RequestConversionToShip(new Action(this.convertBtn_Fail));

    private void convertToStationBtn_ButtonClicked(MyGuiControlButton obj) => MyTerminalInfoController.m_grid.RequestConversionToStation();

    private void OnSafeZoneUpdated(MySafeZone obj) => this.UpdateConvertButtons();

    private void OnGridPositionChanged(MyPositionComponentBase obj) => this.OnSafeZoneUpdated((MySafeZone) null);

    private void renameBtn_Update(MyGuiControlTextbox obj)
    {
      if (!obj.Enabled)
        return;
      MyGuiControlTextbox controlByName = (MyGuiControlTextbox) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("RenameShipText");
      MyTerminalInfoController.m_grid.ChangeDisplayNameRequest(controlByName.Text);
    }

    private static void deleteBuiltByIdBlocksButton_ButtonClicked(MyGuiControlButton obj)
    {
      if (obj.Index >= MyTerminalInfoController.m_infoGrids.Count)
        return;
      MyBlockLimits.MyGridLimitData gridInfo = MyTerminalInfoController.m_infoGrids[obj.Index];
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextConfirmDeleteGrid, (object) gridInfo.GridName), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        MyMultiplayer.RaiseStaticEvent<long, long>((Func<IMyEventOwner, Action<long, long>>) (x => new Action<long, long>(MyBlockLimits.RemoveBlocksBuiltByID)), gridInfo.EntityId, MySession.Static.LocalPlayerId);
      })), canHideOthers: false));
    }

    private static void assignCombobox_ItemSelected(
      MyIdentity locallIdentity,
      long entityId,
      MyPlayer.PlayerId playerId)
    {
      MyBlockLimits.MyGridLimitData gridLimitData;
      if (!locallIdentity.BlockLimits.BlocksBuiltByGrid.TryGetValue(entityId, out gridLimitData))
        return;
      ulong steamId = playerId.SteamId;
      MyIdentity identity = MySession.Static.Players.TryGetPlayerIdentity(playerId);
      if (identity == null)
        return;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextConfirmTransferGrid), new object[2]
      {
        (object) gridLimitData.GridName,
        (object) identity.DisplayName
      }), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
      {
        if (result != MyGuiScreenMessageBox.ResultEnum.YES)
          return;
        if (MySession.Static.Players.GetOnlinePlayers().Contains(MySession.Static.Players.GetPlayerById(playerId)))
          MyMultiplayer.RaiseStaticEvent<MyBlockLimits.MyGridLimitData, long, long, ulong>((Func<IMyEventOwner, Action<MyBlockLimits.MyGridLimitData, long, long, ulong>>) (x => new Action<MyBlockLimits.MyGridLimitData, long, long, ulong>(MyBlockLimits.SendTransferRequestMessage)), gridLimitData, MySession.Static.LocalPlayerId, identity.IdentityId, steamId);
        else
          MyTerminalInfoController.ShowPlayerNotOnlineMessage(identity);
      })), canHideOthers: false));
    }

    private static void ShowPlayerNotOnlineMessage(MyIdentity identity) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextPlayerNotOnline, (object) identity.DisplayName), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result => MyTerminalInfoController.RecreateControls())), canHideOthers: false));

    private void renameBtn_ButtonClicked(MyGuiControlButton obj)
    {
      MyGuiControlTextbox controlByName = (MyGuiControlTextbox) MyTerminalInfoController.m_infoPage.Controls.GetControlByName("RenameShipText");
      MyTerminalInfoController.m_grid.ChangeDisplayNameRequest(controlByName.Text);
    }

    private void grid_OnClose(MyEntity obj)
    {
      if (!(obj is MyCubeGrid))
        return;
      foreach (MyBlockLimits.MyGridLimitData infoGrid in MyTerminalInfoController.m_infoGrids)
      {
        if (infoGrid.EntityId == obj.EntityId)
        {
          MyTerminalInfoController.RecreateControls();
          break;
        }
      }
    }

    private void grid_OnBlockRemoved(MySlimBlock obj) => MyTerminalInfoController.RecreateControls();

    private void grid_OnBlockAdded(MySlimBlock obj) => MyTerminalInfoController.RecreateControls();

    private void grid_OnPhysicsChanged(MyEntity obj) => MyTerminalInfoController.RecreateControls();

    private void grid_OnBlockOwnershipChanged(MyEntity obj) => MyTerminalInfoController.RecreateControls();

    private void grid_OnAuthorshipChanged() => MyTerminalInfoController.RecreateControls();

    public override void HandleInput()
    {
      base.HandleInput();
      if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
      {
        if (this.m_convertToShipBtn.Enabled)
          MyTerminalInfoController.m_grid.RequestConversionToShip(new Action(this.convertBtn_Fail));
        else if (this.m_convertToStationBtn.Enabled)
          MyTerminalInfoController.m_grid.RequestConversionToStation();
      }
      this.m_convertToShipBtn.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_convertToStationBtn.Visible = !MyInput.Static.IsJoystickLastUsed;
    }

    [Serializable]
    private struct GridBuiltByIdInfo
    {
      public string GridName;
      public long EntityId;
      public int PCUBuilt;
      public int BlockCount;
      public List<string> UnsafeBlocks;

      protected class Sandbox_Game_Gui_MyTerminalInfoController\u003C\u003EGridBuiltByIdInfo\u003C\u003EGridName\u003C\u003EAccessor : IMemberAccessor<MyTerminalInfoController.GridBuiltByIdInfo, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyTerminalInfoController.GridBuiltByIdInfo owner,
          in string value)
        {
          owner.GridName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyTerminalInfoController.GridBuiltByIdInfo owner,
          out string value)
        {
          value = owner.GridName;
        }
      }

      protected class Sandbox_Game_Gui_MyTerminalInfoController\u003C\u003EGridBuiltByIdInfo\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyTerminalInfoController.GridBuiltByIdInfo, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyTerminalInfoController.GridBuiltByIdInfo owner,
          in long value)
        {
          owner.EntityId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyTerminalInfoController.GridBuiltByIdInfo owner,
          out long value)
        {
          value = owner.EntityId;
        }
      }

      protected class Sandbox_Game_Gui_MyTerminalInfoController\u003C\u003EGridBuiltByIdInfo\u003C\u003EPCUBuilt\u003C\u003EAccessor : IMemberAccessor<MyTerminalInfoController.GridBuiltByIdInfo, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyTerminalInfoController.GridBuiltByIdInfo owner,
          in int value)
        {
          owner.PCUBuilt = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyTerminalInfoController.GridBuiltByIdInfo owner,
          out int value)
        {
          value = owner.PCUBuilt;
        }
      }

      protected class Sandbox_Game_Gui_MyTerminalInfoController\u003C\u003EGridBuiltByIdInfo\u003C\u003EBlockCount\u003C\u003EAccessor : IMemberAccessor<MyTerminalInfoController.GridBuiltByIdInfo, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyTerminalInfoController.GridBuiltByIdInfo owner,
          in int value)
        {
          owner.BlockCount = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyTerminalInfoController.GridBuiltByIdInfo owner,
          out int value)
        {
          value = owner.BlockCount;
        }
      }

      protected class Sandbox_Game_Gui_MyTerminalInfoController\u003C\u003EGridBuiltByIdInfo\u003C\u003EUnsafeBlocks\u003C\u003EAccessor : IMemberAccessor<MyTerminalInfoController.GridBuiltByIdInfo, List<string>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyTerminalInfoController.GridBuiltByIdInfo owner,
          in List<string> value)
        {
          owner.UnsafeBlocks = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyTerminalInfoController.GridBuiltByIdInfo owner,
          out List<string> value)
        {
          value = owner.UnsafeBlocks;
        }
      }
    }

    protected sealed class ServerLimitInfo_Implementation\u003C\u003ESystem_Int64\u0023System_UInt64 : ICallSite<IMyEventOwner, long, ulong, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long identityId,
        in ulong clientId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTerminalInfoController.ServerLimitInfo_Implementation(identityId, clientId);
      }
    }

    protected sealed class ServerLimitInfo_Received\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_Gui_MyTerminalInfoController\u003C\u003EGridBuiltByIdInfo\u003E : ICallSite<IMyEventOwner, List<MyTerminalInfoController.GridBuiltByIdInfo>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<MyTerminalInfoController.GridBuiltByIdInfo> gridsWithBuiltById,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyTerminalInfoController.ServerLimitInfo_Received(gridsWithBuiltById);
      }
    }
  }
}
