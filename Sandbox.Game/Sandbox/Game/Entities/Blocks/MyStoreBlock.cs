// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyStoreBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_StoreBlock))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyStoreBlock), typeof (Sandbox.ModAPI.Ingame.IMyStoreBlock)})]
  public class MyStoreBlock : MyFunctionalBlock, IMyConveyorEndpointBlock, IMyInventoryOwner, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, Sandbox.ModAPI.IMyStoreBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyStoreBlock
  {
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;
    private Action<List<MyStoreItem>, long, float, float> m_localRequestStoreItemsCallback;
    private Action<MyStoreBuyItemResult> m_localRequestBuyCallback;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_anyoneCanUse;
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;
    private Action<MyStoreSellItemResult> m_localRequestSellItemCallback;
    private Action<List<long>> m_localRequestConnectedInventoriesItemCallback;
    private Action<List<long>> m_localRequestInventoriesItemCallback;
    private Action<MyStoreCreationResult> m_localRequestCreateOfferCallback;
    private Action<MyStoreCreationResult> m_localRequestCreateOrderCallback;
    private Action<bool> m_localRequestCancelStoreItemCallback;
    private MyMultiTextPanelComponent m_multiPanel;
    private MyGuiScreenTextPanel m_textBoxMultiPanel;
    private bool m_isTextPanelOpen;
    private MyResourceStateEnum m_currentState = MyResourceStateEnum.NoPower;
    private Action<MyStoreBuyItemResults> m_localRequestChangeBalanceCallback;

    int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount => this.m_multiPanel == null ? 0 : this.m_multiPanel.SurfaceCount;

    Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(
      int index)
    {
      return this.m_multiPanel == null ? (Sandbox.ModAPI.Ingame.IMyTextSurface) null : this.m_multiPanel.GetSurface(index);
    }

    public bool IsTextPanelOpen
    {
      get => this.m_isTextPanelOpen;
      set
      {
        if (this.m_isTextPanelOpen == value)
          return;
        this.m_isTextPanelOpen = value;
        this.RaisePropertiesChanged();
      }
    }

    protected virtual bool UseConveyorSystem
    {
      get => (bool) this.m_useConveyorSystem;
      set => this.m_useConveyorSystem.Value = value;
    }

    public bool AnyoneCanUse
    {
      get => (bool) this.m_anyoneCanUse;
      set => this.m_anyoneCanUse.Value = value;
    }

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public List<MyStoreItem> PlayerItems { get; private set; }

    public MyStoreBlockDefinition BlockDefinition => (MyStoreBlockDefinition) base.BlockDefinition;

    public override MyCubeBlockHighlightModes HighlightMode => this.AnyoneCanUse ? MyCubeBlockHighlightModes.AlwaysCanUse : MyCubeBlockHighlightModes.Default;

    public MyStoreBlock() => this.Render = (MyRenderComponentBase) new MyRenderComponentScreenAreas((MyEntity) this);

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.m_useConveyorSystem.SetLocalValue(true);
      MyObjectBuilder_StoreBlock builderStoreBlock = objectBuilder as MyObjectBuilder_StoreBlock;
      this.m_anyoneCanUse.SetLocalValue(builderStoreBlock.AnyoneCanUse);
      this.m_useConveyorSystem.SetLocalValue(builderStoreBlock.UseConveyorSystem);
      if (builderStoreBlock.PlayerItems != null)
      {
        this.PlayerItems = new List<MyStoreItem>(builderStoreBlock.PlayerItems.Count);
        foreach (MyObjectBuilder_StoreItem playerItem in builderStoreBlock.PlayerItems)
          this.PlayerItems.Add(new MyStoreItem(playerItem));
      }
      else
        this.PlayerItems = new List<MyStoreItem>();
      this.InitializeConveyorEndpoint();
      if (this.BlockDefinition.ScreenAreas != null && this.BlockDefinition.ScreenAreas.Count > 0)
      {
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
        this.m_multiPanel = new MyMultiTextPanelComponent((MyTerminalBlock) this, this.BlockDefinition.ScreenAreas, builderStoreBlock.TextPanels);
        this.m_multiPanel.Init(new Action<int, int[]>(this.SendAddImagesToSelectionRequest), new Action<int, int[]>(this.SendRemoveSelectedImageRequest), new Action<int, string>(this.ChangeTextRequest), new Action<int, MySerializableSpriteCollection>(this.UpdateSpriteCollection));
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    protected override bool CheckIsWorking() => base.CheckIsWorking() && this.m_currentState == MyResourceStateEnum.Ok;

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_StoreBlock builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_StoreBlock;
      builderCubeBlock.AnyoneCanUse = this.AnyoneCanUse;
      if (this.PlayerItems != null)
      {
        builderCubeBlock.PlayerItems = new List<MyObjectBuilder_StoreItem>(this.PlayerItems.Count);
        foreach (MyStoreItem playerItem in this.PlayerItems)
          builderCubeBlock.PlayerItems.Add(playerItem.GetObjectBuilder());
      }
      else
        builderCubeBlock.PlayerItems = new List<MyObjectBuilder_StoreItem>();
      if (this.m_multiPanel != null)
        builderCubeBlock.TextPanels = this.m_multiPanel.Serialize();
      builderCubeBlock.UseConveyorSystem = (bool) this.m_useConveyorSystem;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override void Closing()
    {
      base.Closing();
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.SetRender((MyRenderComponentScreenAreas) null);
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      if (this.m_multiPanel != null)
        this.m_multiPanel.Reset();
      this.UpdateScreen();
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.UpdateAfterSimulation(this.IsWorking);
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (this.CubeGrid.GridSystems.ResourceDistributor == null)
        return;
      int currentState1 = (int) this.m_currentState;
      this.m_currentState = this.CubeGrid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId);
      int currentState2 = (int) this.m_currentState;
      if (currentState1 == currentState2)
        return;
      this.UpdateIsWorking();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.m_multiPanel != null && this.m_multiPanel.SurfaceCount > 0)
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.AddToScene();
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.UpdateScreen();
    }

    protected override void OnStartWorking() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    protected override void OnStopWorking() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    public void UpdateScreen() => this.m_multiPanel?.UpdateScreen(this.IsWorking);

    private void ChangeTextRequest(int panelIndex, string text) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, int, string>(this, (Func<MyStoreBlock, Action<int, string>>) (x => new Action<int, string>(x.OnChangeTextRequest)), panelIndex, text);

    [Event(null, 314)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnChangeTextRequest(int panelIndex, [Nullable] string text) => this.m_multiPanel?.ChangeText(panelIndex, text);

    private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, int, MySerializableSpriteCollection>(this, (Func<MyStoreBlock, Action<int, MySerializableSpriteCollection>>) (x => new Action<int, MySerializableSpriteCollection>(x.OnUpdateSpriteCollection)), panelIndex, sprites);
    }

    [Event(null, 330)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    [DistanceRadius(32f)]
    private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites) => this.m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);

    private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, int, int[]>(this, (Func<MyStoreBlock, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnSelectImageRequest)), panelIndex, selection);

    private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, int, int[]>(this, (Func<MyStoreBlock, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnRemoveSelectedImageRequest)), panelIndex, selection);

    [Event(null, 346)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
    {
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.RemoveItems(panelIndex, selection);
    }

    [Event(null, 357)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnSelectImageRequest(int panelIndex, int[] selection)
    {
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.SelectItems(panelIndex, selection);
    }

    void IMyMultiTextPanelComponentOwner.SelectPanel(
      List<MyGuiControlListbox.Item> panelItems)
    {
      if (this.m_multiPanel != null)
        this.m_multiPanel.SelectPanel((int) panelItems[0].UserData);
      this.RaisePropertiesChanged();
    }

    MyMultiTextPanelComponent IMyMultiTextPanelComponentOwner.MultiTextPanel => this.m_multiPanel;

    public MyTextPanelComponent PanelComponent => this.m_multiPanel == null ? (MyTextPanelComponent) null : this.m_multiPanel.PanelComponent;

    public void OpenWindow(bool isEditable, bool sync, bool isPublic)
    {
      if (sync)
      {
        this.SendChangeOpenMessage(true, isEditable, Sandbox.Game.Multiplayer.Sync.MyId, isPublic);
      }
      else
      {
        this.CreateTextBox(isEditable, new StringBuilder(this.PanelComponent.Text.ToString()), isPublic);
        MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
        MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) this.m_textBoxMultiPanel);
      }
    }

    private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
    {
      string displayNameText = this.DisplayNameText;
      string displayName = this.PanelComponent.DisplayName;
      string description1 = description.ToString();
      bool flag = isEditable;
      Action<VRage.Game.ModAPI.ResultEnum> resultCallback = new Action<VRage.Game.ModAPI.ResultEnum>(this.OnClosedPanelTextBox);
      int num = flag ? 1 : 0;
      this.m_textBoxMultiPanel = new MyGuiScreenTextPanel(displayNameText, "", displayName, description1, resultCallback, editable: (num != 0));
    }

    public void OnClosedPanelTextBox(VRage.Game.ModAPI.ResultEnum result)
    {
      if (this.m_textBoxMultiPanel == null)
        return;
      if (this.m_textBoxMultiPanel.Description.Text.Length > 100000)
      {
        Action<MyGuiScreenMessageBox.ResultEnum> callback = new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnClosedPanelMessageBox);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText), callback: callback));
      }
      else
        this.CloseWindow(true);
    }

    public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        this.m_textBoxMultiPanel.Description.Text.Remove(100000, this.m_textBoxMultiPanel.Description.Text.Length - 100000);
        this.CloseWindow(true);
      }
      else
      {
        this.CreateTextBox(true, this.m_textBoxMultiPanel.Description.Text, true);
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_textBoxMultiPanel);
      }
    }

    [Event(null, 445)]
    [Reliable]
    [Broadcast]
    private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic) => this.OnChangeOpen(isOpen, editable, user, isPublic);

    private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0, bool isPublic = false) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, bool, bool, ulong, bool>(this, (Func<MyStoreBlock, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenRequest)), isOpen, editable, user, isPublic);

    [Event(null, 456)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      if (((!Sandbox.Game.Multiplayer.Sync.IsServer ? 0 : (this.IsTextPanelOpen ? 1 : 0)) & (isOpen ? 1 : 0)) != 0)
        return;
      this.OnChangeOpen(isOpen, editable, user, isPublic);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, bool, bool, ulong, bool>(this, (Func<MyStoreBlock, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenSuccess)), isOpen, editable, user, isPublic);
    }

    private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      this.IsTextPanelOpen = isOpen;
      if (((Sandbox.Engine.Platform.Game.IsDedicated ? 0 : ((long) user == (long) Sandbox.Game.Multiplayer.Sync.MyId ? 1 : 0)) & (isOpen ? 1 : 0)) == 0)
        return;
      this.OpenWindow(editable, false, isPublic);
    }

    private void CloseWindow(bool isPublic)
    {
      MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
      MyGuiScreenGamePlay.TmpGameplayScreenHolder = (MyGuiScreenBase) null;
      foreach (MySlimBlock cubeBlock in this.CubeGrid.CubeBlocks)
      {
        if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == this.EntityId)
        {
          this.SendChangeDescriptionMessage(this.m_textBoxMultiPanel.Description.Text, isPublic);
          this.SendChangeOpenMessage(false);
          break;
        }
      }
    }

    private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
    {
      if (this.CubeGrid.IsPreview || !this.CubeGrid.SyncFlag)
      {
        this.PanelComponent.Text = description;
      }
      else
      {
        if (description.CompareTo(this.PanelComponent.Text) == 0)
          return;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, string, bool>(this, (Func<MyStoreBlock, Action<string, bool>>) (x => new Action<string, bool>(x.OnChangeDescription)), description.ToString(), isPublic);
      }
    }

    [Event(null, 513)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void OnChangeDescription(string description, bool isPublic)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Clear().Append(description);
      this.PanelComponent.Text = stringBuilder;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyStoreBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlCheckbox<MyStoreBlock> checkbox = new MyTerminalControlCheckbox<MyStoreBlock>("AnyoneCanUse", MySpaceTexts.BlockPropertyText_AnyoneCanUse, MySpaceTexts.BlockPropertyDescription_AnyoneCanUse);
      checkbox.Getter = (MyTerminalValueControl<MyStoreBlock, bool>.GetterDelegate) (x => x.AnyoneCanUse);
      checkbox.Setter = (MyTerminalValueControl<MyStoreBlock, bool>.SetterDelegate) ((x, v) => x.AnyoneCanUse = v);
      checkbox.EnableAction<MyStoreBlock>();
      MyTerminalControlFactory.AddControl<MyStoreBlock>((MyTerminalControl<MyStoreBlock>) checkbox);
      MyTerminalControlOnOffSwitch<MyStoreBlock> onOff = new MyTerminalControlOnOffSwitch<MyStoreBlock>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem);
      onOff.Getter = (MyTerminalValueControl<MyStoreBlock, bool>.GetterDelegate) (x => x.UseConveyorSystem);
      onOff.Setter = (MyTerminalValueControl<MyStoreBlock, bool>.SetterDelegate) ((x, v) => x.UseConveyorSystem = v);
      onOff.Visible = (Func<MyStoreBlock, bool>) (x => x.HasInventory);
      onOff.EnableToggleAction<MyStoreBlock>();
      MyTerminalControlFactory.AddControl<MyStoreBlock>((MyTerminalControl<MyStoreBlock>) onOff);
      MyMultiTextPanelComponent.CreateTerminalControls<MyStoreBlock>();
    }

    public void CreateGetStoreItemsRequest(
      long identityId,
      Action<List<MyStoreItem>, long, float, float> resultCallback)
    {
      this.m_localRequestStoreItemsCallback = resultCallback;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock>(this, (Func<MyStoreBlock, Action>) (x => new Action(x.GetStoreItems)));
    }

    [Event(null, 553)]
    [Reliable]
    [Server]
    private void GetStoreItems()
    {
      MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(this.CubeGrid.EntityId);
      if (stationByGridId != null)
      {
        this.GetStoreItemsForStation(stationByGridId);
      }
      else
      {
        MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
        if (component == null)
        {
          MyLog.Default.WriteToLogAndAssert("GetStoreItems - Economy session component not found.");
        }
        else
        {
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, List<MyStoreItem>, long, float, float>(this, (Func<MyStoreBlock, Action<List<MyStoreItem>, long, float, float>>) (x => new Action<List<MyStoreItem>, long, float, float>(x.OnGetStoreItemsResult)), this.PlayerItems, component.LastEconomyTick.Ticks, 1f, 1f, MyEventContext.Current.Sender);
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, List<MyStoreItem>, long>(this, (Func<MyStoreBlock, Action<List<MyStoreItem>, long>>) (x => new Action<List<MyStoreItem>, long>(x.OnPlayerStoreItemsChanged_Broacast)), this.PlayerItems, component.LastEconomyTick.Ticks, MyEventContext.Current.Sender);
        }
      }
    }

    private void GetStoreItemsForStation(MyStation station)
    {
      if (MySession.Static.Factions.TryGetFactionById(station.FactionId) == null)
      {
        MyLog.Default.WriteToLogAndAssert("GetStoreItemsForStation - Faction not found.");
      }
      else
      {
        MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
        if (component == null)
        {
          MyLog.Default.WriteToLogAndAssert("GetStoreItemsForStation - Economy session component not found.");
        }
        else
        {
          float num1 = 1f;
          float num2 = 1f;
          Tuple<MyRelationsBetweenFactions, int> playerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0), station.FactionId);
          if (playerAndFaction.Item1 == MyRelationsBetweenFactions.Friends)
          {
            int relationValue = playerAndFaction.Item2;
            num2 = component.GetOffersFriendlyBonus(relationValue);
            num1 = component.GetOrdersFriendlyBonus(relationValue);
          }
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, List<MyStoreItem>, long, float, float>(this, (Func<MyStoreBlock, Action<List<MyStoreItem>, long, float, float>>) (x => new Action<List<MyStoreItem>, long, float, float>(x.OnGetStoreItemsResult)), station.StoreItems, component.LastEconomyTick.Ticks, num2, num1, MyEventContext.Current.Sender);
        }
      }
    }

    [Event(null, 619)]
    [Reliable]
    [Client]
    private void OnGetStoreItemsResult(
      List<MyStoreItem> storeItems,
      long lastEconomyTick,
      float offersBonus,
      float ordersBonus)
    {
      Action<List<MyStoreItem>, long, float, float> storeItemsCallback = this.m_localRequestStoreItemsCallback;
      if (storeItemsCallback != null)
        storeItemsCallback(storeItems, lastEconomyTick, offersBonus, ordersBonus);
      this.m_localRequestStoreItemsCallback = (Action<List<MyStoreItem>, long, float, float>) null;
    }

    [Event(null, 626)]
    [Reliable]
    [Server]
    [BroadcastExcept]
    private void OnPlayerStoreItemsChanged_Broacast(
      List<MyStoreItem> storeItems,
      long lastEconomyTick)
    {
      this.OnPlayerStoreItemsChanged(storeItems, lastEconomyTick);
    }

    protected virtual void OnPlayerStoreItemsChanged(
      List<MyStoreItem> storeItems,
      long lastEconomyTick)
    {
    }

    internal void CreateBuyRequest(
      long id,
      int amount,
      long targetEntityId,
      long lastEconomyTick,
      Action<MyStoreBuyItemResult> resultCallback)
    {
      this.m_localRequestBuyCallback = resultCallback;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, long, int, long, long>(this, (Func<MyStoreBlock, Action<long, int, long, long>>) (x => new Action<long, int, long, long>(x.BuyItem)), id, amount, targetEntityId, lastEconomyTick);
    }

    [Event(null, 643)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void BuyItem(long id, int amount, long targetEntityId, long lastEconomyTick)
    {
      if (!this.HasAccess() || amount <= 0)
        return;
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      MyPlayer player = this.GetPlayer(identityId);
      if (player == null || player.Character == null)
      {
        MyLog.Default.WriteToLogAndAssert("BuyItem - Player not found.");
      }
      else
      {
        MyAccountInfo account;
        if (!MyBankingSystem.Static.TryGetAccountInfo(identityId, out account))
        {
          MyLog.Default.WriteToLogAndAssert("BuyItem - Player does not have account.");
        }
        else
        {
          MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(this.CubeGrid.EntityId);
          if (stationByGridId == null)
            this.BuyFromPlayer(id, amount, targetEntityId, player, account);
          else
            this.BuyFromStation(id, amount, player, account, stationByGridId, targetEntityId, lastEconomyTick);
        }
      }
    }

    private void BuyFromPlayer(
      long id,
      int amount,
      long targetEntityId,
      MyPlayer player,
      MyAccountInfo playerAccountInfo)
    {
      MyEntity entity = (MyEntity) null;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(targetEntityId, out entity))
        MyLog.Default.WriteToLogAndAssert("BuyFromPlayer - Entity not found.");
      else if (entity is MyCubeBlock myCubeBlock && !myCubeBlock.CubeGrid.BigOwners.Contains(player.Identity.IdentityId))
        MyLog.Default.WriteToLogAndAssert("BuyFromPlayer - Player is not big owner of the grid.");
      else if (entity is MyCharacter && player.Character != entity)
      {
        MyLog.Default.WriteToLogAndAssert("BuyFromPlayer - Player entity and inventory entity is different.");
      }
      else
      {
        MyInventory inventory;
        if (!entity.TryGetInventory(out inventory))
        {
          MyLog.Default.WriteToLogAndAssert("BuyFromPlayer - Inventory not found.");
        }
        else
        {
          MyStoreItem storeItem = (MyStoreItem) null;
          foreach (MyStoreItem playerItem in this.PlayerItems)
          {
            if (playerItem.Id == id)
            {
              storeItem = playerItem;
              break;
            }
          }
          if (storeItem == null)
            this.SendBuyItemResult(id, amount, MyStoreBuyItemResults.ItemNotFound);
          else if (amount > storeItem.Amount)
          {
            this.SendBuyItemResult(id, amount, MyStoreBuyItemResults.WrongAmount);
          }
          else
          {
            long totalPrice = (long) storeItem.PricePerUnit * (long) amount;
            if (totalPrice > playerAccountInfo.Balance)
              this.SendBuyItemResult(id, amount, MyStoreBuyItemResults.NotEnoughMoney);
            else if (totalPrice < 0L)
            {
              MyLog.Default.WriteToLogAndAssert("BuyFromPlayer - Wrong price for the item.");
            }
            else
            {
              switch (storeItem.ItemType)
              {
                case ItemTypes.PhysicalItem:
                  this.BuyPhysicalItemFromPlayer(id, amount, player, inventory, storeItem, totalPrice);
                  break;
              }
            }
          }
        }
      }
    }

    private void BuyPhysicalItemFromPlayer(
      long id,
      int amount,
      MyPlayer player,
      MyInventory inventory,
      MyStoreItem storeItem,
      long totalPrice)
    {
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component == null)
      {
        MyLog.Default.WriteToLogAndAssert("BuyPhysicalItemFromPlayer - Economy session component not found.");
      }
      else
      {
        MyInventory myInventory1 = inventory;
        SerializableDefinitionId? nullable = storeItem.Item;
        MyDefinitionId contentId1 = (MyDefinitionId) nullable.Value;
        if (!myInventory1.CheckConstraint(contentId1))
        {
          MyLog.Default.WriteToLogAndAssert("BuyPhysicalItemFromPlayer - Item can not be transfered to this inventory.");
        }
        else
        {
          MyInventory myInventory2 = inventory;
          MyFixedPoint amount1 = (MyFixedPoint) amount;
          nullable = storeItem.Item;
          MyDefinitionId contentId2 = (MyDefinitionId) nullable.Value;
          if (!myInventory2.CanItemsBeAdded(amount1, contentId2))
          {
            this.SendBuyItemResult(id, amount, MyStoreBuyItemResults.NotEnoughInventorySpace);
          }
          else
          {
            bool flag1 = false;
            if (this.UseConveyorSystem)
            {
              MyGridConveyorSystem conveyorSystem = this.CubeGrid.GridSystems.ConveyorSystem;
              nullable = storeItem.Item;
              MyDefinitionId itemId = (MyDefinitionId) nullable.Value;
              MyFixedPoint amount2 = (MyFixedPoint) amount;
              MyInventory destinationInventory = inventory;
              int num = MyFakes.CONV_PULL_CACL_IMMIDIATLY_STORE_SAFEZONE ? 1 : 0;
              flag1 = conveyorSystem.PullItem(itemId, amount2, (IMyConveyorEndpointBlock) this, destinationInventory, num != 0);
            }
            bool flag2 = true;
            if (!flag1)
            {
              MyInventory inventory1 = MyEntityExtensions.GetInventory(this);
              if (inventory1 != null)
              {
                MyInventory myInventory3 = inventory1;
                nullable = storeItem.Item;
                MyDefinitionId contentId3 = (MyDefinitionId) nullable.Value;
                MyFixedPoint itemAmount = myInventory3.GetItemAmount(contentId3, MyItemFlags.None, false);
                if ((MyFixedPoint) amount > itemAmount)
                {
                  this.SendBuyItemResult(id, amount, MyStoreBuyItemResults.NotEnoughAmount);
                  return;
                }
                MyInventory myInventory4 = inventory1;
                MyFixedPoint amount2 = (MyFixedPoint) amount;
                nullable = storeItem.Item;
                MyDefinitionId contentId4 = (MyDefinitionId) nullable.Value;
                myInventory4.RemoveItemsOfType(amount2, contentId4, MyItemFlags.None, false);
              }
              else
                flag2 = false;
              nullable = storeItem.Item;
              MyObjectBuilder_Base newObject = MyObjectBuilderSerializer.CreateNewObject(nullable.Value);
              inventory.AddItems((MyFixedPoint) amount, newObject);
            }
            if (flag2)
              storeItem.Amount -= amount;
            if (storeItem.OnTransaction != null)
              storeItem.OnTransaction(amount, storeItem.Amount, totalPrice, this.OwnerId, player.Identity.IdentityId);
            if (storeItem.Amount == 0)
              this.PlayerItems.Remove(storeItem);
            MySession.Static.GetComponent<MySessionComponentEconomy>()?.AddCurrencyDestroyed((long) ((double) totalPrice * (double) component.EconomyDefinition.TransactionFee));
            MyBankingSystem.ChangeBalance(player.Identity.IdentityId, -totalPrice);
            totalPrice = (long) ((double) totalPrice * (1.0 - (double) component.EconomyDefinition.TransactionFee));
            if (flag2 && this.OwnerId != 0L)
              MyBankingSystem.ChangeBalance(this.OwnerId, totalPrice);
            long id1 = id;
            nullable = storeItem.Item;
            string subtypeName = nullable.Value.SubtypeName;
            long price = totalPrice;
            int amount3 = amount;
            this.SendBuyItemResult(id1, subtypeName, price, amount3, MyStoreBuyItemResults.Success);
            MyInventory inventory2 = inventory;
            nullable = storeItem.Item;
            MyDefinitionId definitionId = (MyDefinitionId) nullable.Value;
            long totalPrice1 = totalPrice;
            int amount4 = amount;
            this.OnItemBought(inventory2, definitionId, totalPrice1, amount4);
          }
        }
      }
    }

    protected virtual void OnItemBought(
      MyInventory inventory,
      MyDefinitionId definitionId,
      long totalPrice,
      int amount)
    {
    }

    private void BuyFromStation(
      long id,
      int amount,
      MyPlayer player,
      MyAccountInfo playerAccountInfo,
      MyStation station,
      long targetEntityId,
      long lastEconomyTick)
    {
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component == null)
        MyLog.Default.WriteToLogAndAssert("BuyFromStation - Economy session component not found.");
      else if (lastEconomyTick != component.LastEconomyTick.Ticks)
      {
        this.SendBuyItemResult(id, amount, MyStoreBuyItemResults.ItemsTimeout);
      }
      else
      {
        Tuple<MyRelationsBetweenFactions, int> playerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(player.Identity.IdentityId, station.FactionId);
        float num = 1f;
        if (playerAndFaction.Item1 == MyRelationsBetweenFactions.Friends)
          num = component.GetOffersFriendlyBonus(playerAndFaction.Item2);
        MyEntity entity = (MyEntity) null;
        if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(targetEntityId, out entity))
          MyLog.Default.WriteToLogAndAssert("BuyFromStation - Entity not found.");
        else if (entity is MyCubeBlock myCubeBlock && !myCubeBlock.CubeGrid.BigOwners.Contains(player.Identity.IdentityId))
          MyLog.Default.WriteToLogAndAssert("BuyFromStation - Player is not big owner of the grid.");
        else if (entity is MyCharacter && player.Character != entity)
        {
          MyLog.Default.WriteToLogAndAssert("BuyFromStation - Player entity and inventory entity is different.");
        }
        else
        {
          MyInventory inventory;
          if (!entity.TryGetInventory(out inventory))
          {
            MyLog.Default.WriteToLogAndAssert("BuyFromStation - Inventory not found.");
          }
          else
          {
            MyStoreItem storeItemById = station.GetStoreItemById(id);
            if (storeItemById == null)
              this.SendBuyItemResult(id, amount, MyStoreBuyItemResults.ItemNotFound);
            else if (amount > storeItemById.Amount)
            {
              this.SendBuyItemResult(id, amount, MyStoreBuyItemResults.WrongAmount);
            }
            else
            {
              long totalPrice = (long) ((double) ((long) storeItemById.PricePerUnit * (long) amount) * (double) num);
              if (totalPrice > playerAccountInfo.Balance)
                this.SendBuyItemResult(id, amount, MyStoreBuyItemResults.NotEnoughMoney);
              else if (totalPrice < 0L)
              {
                MyLog.Default.WriteToLogAndAssert("BuyFromStation - Wrong price for the item.");
              }
              else
              {
                switch (storeItemById.ItemType)
                {
                  case ItemTypes.PhysicalItem:
                    this.BuyPhysicalItem(id, amount, player, station, inventory, storeItemById, totalPrice);
                    break;
                  case ItemTypes.Oxygen:
                    this.BuyGas(storeItemById, amount, player, station, entity, totalPrice, MyCharacterOxygenComponent.OxygenId);
                    break;
                  case ItemTypes.Hydrogen:
                    this.BuyGas(storeItemById, amount, player, station, entity, totalPrice, MyCharacterOxygenComponent.HydrogenId);
                    break;
                  case ItemTypes.Grid:
                    this.BuyPrefab(storeItemById, amount, player, station, entity, totalPrice);
                    break;
                }
              }
            }
          }
        }
      }
    }

    private void BuyPhysicalItem(
      long id,
      int amount,
      MyPlayer player,
      MyStation station,
      MyInventory inventory,
      MyStoreItem storeItem,
      long totalPrice)
    {
      if (!inventory.CheckConstraint((MyDefinitionId) storeItem.Item.Value))
        MyLog.Default.WriteToLogAndAssert("BuyPhysicalItem - Item can not be transfered to this inventory.");
      else if (!inventory.CanItemsBeAdded((MyFixedPoint) amount, (MyDefinitionId) storeItem.Item.Value))
      {
        this.SendBuyItemResult(id, amount, MyStoreBuyItemResults.NotEnoughInventorySpace);
      }
      else
      {
        for (int index = 0; index < amount; ++index)
        {
          MyObjectBuilder_Base newObject = MyObjectBuilderSerializer.CreateNewObject(storeItem.Item.Value);
          if (newObject is MyObjectBuilder_Datapad datapad)
          {
            IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(station.FactionId);
            if (factionById == null)
            {
              MyLog.Default.WriteToLogAndAssert("BuyPhysicalItem - Faction not found.");
              return;
            }
            MyFaction friendlyFaction;
            MyStation friendlyStation;
            if (!MySession.Static.Factions.GetRandomFriendlyStation(factionById.FactionId, station.Id, out friendlyFaction, out friendlyStation, true))
            {
              MyLog.Default.WriteToLogAndAssert("BuyPhysicalItem - Friendly Station not found.");
              return;
            }
            MySessionComponentEconomy.PrepareDatapad(ref datapad, friendlyFaction, friendlyStation);
          }
          inventory.AddItems((MyFixedPoint) 1, newObject);
        }
        storeItem.Amount -= amount;
        MyBankingSystem.ChangeBalance(player.Identity.IdentityId, -totalPrice);
        MyBankingSystem.ChangeBalance(station.FactionId, totalPrice);
        this.SendBuyItemResult(id, storeItem.Item.Value.SubtypeName, totalPrice, amount, MyStoreBuyItemResults.Success);
      }
    }

    private void BuyPrefab(
      MyStoreItem storeItem,
      int amount,
      MyPlayer player,
      MyStation station,
      MyEntity entity,
      long totalPrice)
    {
      bool flag = true;
      switch (MySession.Static.BlockLimitsEnabled)
      {
        case MyBlockLimitsEnabledEnum.NONE:
          if (!flag)
          {
            this.SendBuyItemResult(storeItem.Id, amount, MyStoreBuyItemResults.NotEnoughPCU);
            break;
          }
          MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(storeItem.PrefabName);
          MyEntity entity1;
          Sandbox.Game.Entities.MyEntities.TryGetEntityById(station.SafeZoneEntityId, out entity1);
          Vector3D forward = Vector3D.Forward;
          Vector3D vector3D1 = Vector3D.Up;
          Vector3D? nullable1 = new Vector3D?();
          BoundingSphere boundingSphere = new BoundingSphere(Vector3.Zero, float.MinValue);
          BoundingBox invalid = BoundingBox.CreateInvalid();
          foreach (MyObjectBuilder_CubeGrid cubeGrid in prefabDefinition.CubeGrids)
          {
            BoundingBox boundingBox = cubeGrid.CalculateBoundingBox();
            invalid.Include(boundingBox);
          }
          BoundingSphere fromBoundingBox = BoundingSphere.CreateFromBoundingBox(invalid);
          Vector3D? nullable2;
          if (station.Type != MyStationTypeEnum.Outpost)
          {
            nullable2 = Sandbox.Game.Entities.MyEntities.FindFreePlace(station.Position, 60f, ignoreEnt: entity1);
          }
          else
          {
            MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(station.Position);
            SpawnInfo info = new SpawnInfo()
            {
              CollisionRadius = fromBoundingBox.Radius,
              Planet = closestPlanet,
              PlanetDeployAltitude = fromBoundingBox.Radius * 1.05f
            };
            nullable2 = MyRespawnComponentBase.FindPositionAbovePlanet(station.Position, ref info, false, 1, 30, new float?(60f), entity1);
            vector3D1 = station.Position - closestPlanet.PositionComp.GetPosition();
            vector3D1.Normalize();
            forward = Vector3D.CalculatePerpendicularVector(vector3D1);
          }
          if (!nullable2.HasValue)
          {
            this.SendBuyItemResult(storeItem.Id, amount, MyStoreBuyItemResults.FreePositionNotFound);
            break;
          }
          Vector3D position = Vector3D.TransformNormal(fromBoundingBox.Center, MatrixD.CreateWorld(nullable2.Value, forward, vector3D1));
          Vector3D vector3D2 = nullable2.Value - position;
          MyRenderProxy.DebugDrawSphere(position, 0.1f, Color.Green, depthRead: false, persistent: true);
          MySpawnPrefabProperties spawnPrefabProperties = new MySpawnPrefabProperties()
          {
            Position = vector3D2,
            Forward = (Vector3) forward,
            Up = (Vector3) vector3D1,
            SpawningOptions = SpawningOptions.RotateFirstCockpitTowardsDirection | SpawningOptions.SetAuthorship | SpawningOptions.UseOnlyWorldMatrix,
            OwnerId = player.Identity.IdentityId,
            UpdateSync = true,
            PrefabName = storeItem.PrefabName
          };
          EndpointId targetEndpoint = MyEventContext.Current.Sender;
          MyPrefabManager.Static.SpawnPrefabInternal(spawnPrefabProperties, (Action) (() =>
          {
            storeItem.Amount -= amount;
            MyBankingSystem.ChangeBalance(player.Identity.IdentityId, -totalPrice);
            MyBankingSystem.ChangeBalance(station.FactionId, totalPrice);
            this.SendBuyItemResult(storeItem.Id, storeItem.PrefabName, totalPrice, amount, MyStoreBuyItemResults.Success, targetEndpoint);
          }), (Action) (() => this.SendBuyItemResult(storeItem.Id, storeItem.PrefabName, totalPrice, amount, MyStoreBuyItemResults.SpawnFailed, targetEndpoint)));
          break;
        case MyBlockLimitsEnabledEnum.GLOBALLY:
        case MyBlockLimitsEnabledEnum.PER_FACTION:
        case MyBlockLimitsEnabledEnum.PER_PLAYER:
          flag = player.Identity.BlockLimits.PCU >= storeItem.PrefabTotalPcu || MySession.Static.TotalPCU == 0;
          goto case MyBlockLimitsEnabledEnum.NONE;
        default:
          flag = false;
          goto case MyBlockLimitsEnabledEnum.NONE;
      }
    }

    private void BuyGas(
      MyStoreItem storeItem,
      int amount,
      MyPlayer player,
      MyStation station,
      MyEntity entity,
      long totalPrice,
      MyDefinitionId gasId)
    {
      float gasInput = (float) amount * 1000f;
      if (entity == player.Character && player.Character.OxygenComponent != null)
        player.Character.OxygenComponent.TransferSuitGas(ref gasId, gasInput, 0.0f);
      else if (entity is MyGasTank myGasTank && myGasTank.BlockDefinition.StoredGasId == gasId)
      {
        double num = (1.0 - myGasTank.FilledRatio) * (double) myGasTank.Capacity;
        if ((double) gasInput > num)
        {
          this.SendBuyItemResult(storeItem.Id, amount, MyStoreBuyItemResults.NotEnoughSpaceInTank);
          return;
        }
        myGasTank.Transfer((double) gasInput);
      }
      else
      {
        this.SendBuyItemResult(storeItem.Id, amount, MyStoreBuyItemResults.WrongInventory);
        return;
      }
      storeItem.Amount -= amount;
      MyBankingSystem.ChangeBalance(player.Identity.IdentityId, -totalPrice);
      MyBankingSystem.ChangeBalance(station.FactionId, totalPrice);
      this.SendBuyItemResult(storeItem.Id, gasId.SubtypeName, totalPrice, amount, MyStoreBuyItemResults.Success);
    }

    private void SendBuyItemResult(long id, int amount, MyStoreBuyItemResults result) => this.SendBuyItemResult(id, string.Empty, 0L, amount, result, MyEventContext.Current.Sender);

    private void SendBuyItemResult(
      long id,
      string name,
      long price,
      int amount,
      MyStoreBuyItemResults result)
    {
      this.SendBuyItemResult(id, name, price, amount, result, MyEventContext.Current.Sender);
    }

    private void SendBuyItemResult(
      long id,
      string name,
      long price,
      int amount,
      MyStoreBuyItemResults result,
      EndpointId targetEndpoint)
    {
      MyStoreBuyItemResult storeBuyItemResult = new MyStoreBuyItemResult()
      {
        Result = result,
        ItemId = id,
        Amount = amount
      };
      MyLog.Default.WriteLine(string.Format("SendBuyItemResult - {0}, {1}, {2}, {3}, {4}, {5}", (object) result, (object) id, (object) name, (object) price, (object) amount, (object) targetEndpoint));
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, MyStoreBuyItemResult>(this, (Func<MyStoreBlock, Action<MyStoreBuyItemResult>>) (x => new Action<MyStoreBuyItemResult>(x.OnBuyItemResult)), storeBuyItemResult, targetEndpoint);
    }

    [Event(null, 1159)]
    [Reliable]
    [Client]
    private void OnBuyItemResult(MyStoreBuyItemResult result)
    {
      Action<MyStoreBuyItemResult> requestBuyCallback = this.m_localRequestBuyCallback;
      if (requestBuyCallback != null)
        requestBuyCallback(result);
      this.m_localRequestBuyCallback = (Action<MyStoreBuyItemResult>) null;
    }

    public void CreateSellItemRequest(
      long id,
      int amount,
      long sourceEntityId,
      long lastEconomyTick,
      Action<MyStoreSellItemResult> resultCallback)
    {
      this.m_localRequestSellItemCallback = resultCallback;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, long, int, long, long>(this, (Func<MyStoreBlock, Action<long, int, long, long>>) (x => new Action<long, int, long, long>(x.SellItem)), id, amount, sourceEntityId, lastEconomyTick);
    }

    [Event(null, 1172)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void SellItem(long id, int amount, long sourceEntityId, long lastEconomyTick)
    {
      if (!this.HasAccess() || amount <= 0)
        return;
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      MyPlayer player = this.GetPlayer(identityId);
      if (!MyBankingSystem.Static.TryGetAccountInfo(identityId, out MyAccountInfo _))
      {
        MyLog.Default.WriteToLogAndAssert("SellItem - Player does not have account.");
      }
      else
      {
        MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(this.CubeGrid.EntityId);
        if (stationByGridId == null)
          this.SellToPlayer(id, amount, sourceEntityId, player);
        else
          this.SellToStation(id, amount, player, stationByGridId, sourceEntityId, lastEconomyTick);
      }
    }

    private void SellToPlayer(long id, int amount, long sourceEntityId, MyPlayer player)
    {
      MyEntity entity = (MyEntity) null;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(sourceEntityId, out entity))
        MyLog.Default.WriteToLogAndAssert("SellToPlayer - Entity not found.");
      else if (entity is MyCubeBlock myCubeBlock && !myCubeBlock.CubeGrid.BigOwners.Contains(player.Identity.IdentityId))
        MyLog.Default.WriteToLogAndAssert("SellToPlayer - Player is not big owner of the grid.");
      else if (entity is MyCharacter && player.Character != entity)
      {
        MyLog.Default.WriteToLogAndAssert("SellToPlayer - Player entity and inventory entity is different.");
      }
      else
      {
        MyInventory inventory1;
        if (!entity.TryGetInventory(out inventory1))
          MyLog.Default.WriteToLogAndAssert("SellToPlayer - Inventory not found.");
        else if (player == null || player.Character == null)
        {
          MyLog.Default.WriteToLogAndAssert("SellToPlayer - Player not found.");
        }
        else
        {
          MyStoreItem myStoreItem = (MyStoreItem) null;
          foreach (MyStoreItem playerItem in this.PlayerItems)
          {
            if (playerItem.Id == id)
            {
              myStoreItem = playerItem;
              break;
            }
          }
          if (myStoreItem == null)
            this.SendSellItemResult(id, amount, MyStoreSellItemResults.ItemNotFound);
          else if (amount > myStoreItem.Amount)
          {
            this.SendSellItemResult(id, amount, MyStoreSellItemResults.WrongAmount);
          }
          else
          {
            MyAccountInfo account;
            if (!MyBankingSystem.Static.TryGetAccountInfo(this.OwnerId, out account))
            {
              MyLog.Default.WriteToLogAndAssert("SellToPlayer - Owner does not have an account.");
            }
            else
            {
              long amount1 = (long) myStoreItem.PricePerUnit * (long) amount;
              if (amount1 > account.Balance)
                this.SendSellItemResult(id, amount, MyStoreSellItemResults.NotEnoughMoney);
              else if (amount1 < 0L)
              {
                MyLog.Default.WriteToLogAndAssert("SellToPlayer - Wrong price for the item.");
              }
              else
              {
                switch (myStoreItem.ItemType)
                {
                  case ItemTypes.PhysicalItem:
                    MyFixedPoint itemAmount = inventory1.GetItemAmount((MyDefinitionId) myStoreItem.Item.Value, MyItemFlags.None, false);
                    if ((MyFixedPoint) amount > itemAmount)
                    {
                      this.SendSellItemResult(id, amount, MyStoreSellItemResults.NotEnoughAmount);
                      break;
                    }
                    SerializableDefinitionId? nullable;
                    if (this.UseConveyorSystem)
                    {
                      if (!this.CubeGrid.GridSystems.ConveyorSystem.PushGenerateItem((MyDefinitionId) myStoreItem.Item.Value, new MyFixedPoint?((MyFixedPoint) amount), (IMyConveyorEndpointBlock) this, false, true))
                      {
                        MyInventory inventory2 = MyEntityExtensions.GetInventory(this);
                        MyInventory myInventory = inventory2;
                        MyFixedPoint amount2 = (MyFixedPoint) amount;
                        nullable = myStoreItem.Item;
                        MyDefinitionId contentId = (MyDefinitionId) nullable.Value;
                        if (!myInventory.CanItemsBeAdded(amount2, contentId))
                        {
                          this.SendSellItemResult(id, amount, MyStoreSellItemResults.NotEnoughInventorySpace);
                          break;
                        }
                        nullable = myStoreItem.Item;
                        MyObjectBuilder_Base newObject = MyObjectBuilderSerializer.CreateNewObject(nullable.Value);
                        inventory2.AddItems((MyFixedPoint) amount, newObject);
                      }
                    }
                    else
                    {
                      MyInventory inventory2 = MyEntityExtensions.GetInventory(this);
                      MyInventory myInventory = inventory2;
                      MyFixedPoint amount2 = (MyFixedPoint) amount;
                      nullable = myStoreItem.Item;
                      MyDefinitionId contentId = (MyDefinitionId) nullable.Value;
                      if (!myInventory.CanItemsBeAdded(amount2, contentId))
                      {
                        this.SendSellItemResult(id, amount, MyStoreSellItemResults.NotEnoughInventorySpace);
                        break;
                      }
                      nullable = myStoreItem.Item;
                      MyObjectBuilder_Base newObject = MyObjectBuilderSerializer.CreateNewObject(nullable.Value);
                      inventory2.AddItems((MyFixedPoint) amount, newObject);
                    }
                    MyInventory myInventory1 = inventory1;
                    MyFixedPoint amount3 = (MyFixedPoint) amount;
                    nullable = myStoreItem.Item;
                    MyDefinitionId contentId1 = (MyDefinitionId) nullable.Value;
                    myInventory1.RemoveItemsOfType(amount3, contentId1, MyItemFlags.None, false);
                    myStoreItem.Amount -= amount;
                    if (myStoreItem.OnTransaction != null)
                      myStoreItem.OnTransaction(amount, myStoreItem.Amount, amount1, this.OwnerId, player.Identity.IdentityId);
                    MyBankingSystem.ChangeBalance(player.Identity.IdentityId, amount1);
                    MyBankingSystem.ChangeBalance(this.OwnerId, -amount1);
                    long id1 = id;
                    nullable = myStoreItem.Item;
                    string subtypeName = nullable.Value.SubtypeName;
                    long price = amount1;
                    int amount4 = amount;
                    this.SendSellItemResult(id1, subtypeName, price, amount4, MyStoreSellItemResults.Success);
                    break;
                }
              }
            }
          }
        }
      }
    }

    private void SellToStation(
      long id,
      int amount,
      MyPlayer player,
      MyStation station,
      long sourceEntityId,
      long lastEconomyTick)
    {
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component == null)
        MyLog.Default.WriteToLogAndAssert("SellToStation - Economy session component not found.");
      else if (lastEconomyTick != component.LastEconomyTick.Ticks)
      {
        this.SendSellItemResult(id, amount, MyStoreSellItemResults.ItemsTimeout);
      }
      else
      {
        Tuple<MyRelationsBetweenFactions, int> playerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(player.Identity.IdentityId, station.FactionId);
        float num = 1f;
        if (playerAndFaction.Item1 == MyRelationsBetweenFactions.Friends)
          num = component.GetOrdersFriendlyBonus(playerAndFaction.Item2);
        MyEntity entity = (MyEntity) null;
        if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(sourceEntityId, out entity))
          MyLog.Default.WriteToLogAndAssert("SellToStation - Entity not found.");
        else if (entity is MyCubeBlock myCubeBlock && !myCubeBlock.CubeGrid.BigOwners.Contains(player.Identity.IdentityId))
          MyLog.Default.WriteToLogAndAssert("SellToStation - Player is not big owner of the grid.");
        else if (entity is MyCharacter && player.Character != entity)
        {
          MyLog.Default.WriteToLogAndAssert("SellToStation - Player entity and inventory entity is different.");
        }
        else
        {
          MyInventory inventory;
          if (!entity.TryGetInventory(out inventory))
            MyLog.Default.WriteToLogAndAssert("SellToStation - Inventory not found.");
          else if (player == null || player.Character == null)
          {
            MyLog.Default.WriteToLogAndAssert("SellToStation - Player not found.");
          }
          else
          {
            MyStoreItem storeItemById = station.GetStoreItemById(id);
            if (storeItemById == null)
              this.SendSellItemResult(id, amount, MyStoreSellItemResults.ItemNotFound);
            else if (amount > storeItemById.Amount)
            {
              this.SendSellItemResult(id, amount, MyStoreSellItemResults.WrongAmount);
            }
            else
            {
              MyAccountInfo account;
              if (!MyBankingSystem.Static.TryGetAccountInfo(station.FactionId, out account))
              {
                MyLog.Default.WriteToLogAndAssert("SellToStation - Owner does not have account.");
              }
              else
              {
                long amount1 = (long) ((double) ((long) storeItemById.PricePerUnit * (long) amount) * (double) num);
                if (amount1 > account.Balance)
                  this.SendSellItemResult(id, amount, MyStoreSellItemResults.NotEnoughMoney);
                else if (amount1 < 0L)
                {
                  MyLog.Default.WriteToLogAndAssert("SellToStation - Wrong price for the item.");
                }
                else
                {
                  switch (storeItemById.ItemType)
                  {
                    case ItemTypes.PhysicalItem:
                      MyInventory myInventory1 = inventory;
                      SerializableDefinitionId? nullable = storeItemById.Item;
                      MyDefinitionId contentId1 = (MyDefinitionId) nullable.Value;
                      MyFixedPoint itemAmount = myInventory1.GetItemAmount(contentId1, MyItemFlags.None, false);
                      if ((MyFixedPoint) amount > itemAmount)
                      {
                        this.SendSellItemResult(id, amount, MyStoreSellItemResults.NotEnoughAmount);
                        break;
                      }
                      MyInventory myInventory2 = inventory;
                      MyFixedPoint amount2 = (MyFixedPoint) amount;
                      nullable = storeItemById.Item;
                      MyDefinitionId contentId2 = (MyDefinitionId) nullable.Value;
                      myInventory2.RemoveItemsOfType(amount2, contentId2, MyItemFlags.None, false);
                      storeItemById.Amount -= amount;
                      MyBankingSystem.ChangeBalance(player.Identity.IdentityId, amount1);
                      MyBankingSystem.ChangeBalance(station.FactionId, -amount1);
                      long id1 = id;
                      nullable = storeItemById.Item;
                      string subtypeName = nullable.Value.SubtypeName;
                      long price = amount1;
                      int amount3 = amount;
                      this.SendSellItemResult(id1, subtypeName, price, amount3, MyStoreSellItemResults.Success);
                      break;
                  }
                }
              }
            }
          }
        }
      }
    }

    private void SendSellItemResult(long id, int amount, MyStoreSellItemResults result) => this.SendSellItemResult(id, string.Empty, 0L, amount, result);

    private void SendSellItemResult(
      long id,
      string name,
      long price,
      int amount,
      MyStoreSellItemResults result)
    {
      MyStoreSellItemResult storeSellItemResult = new MyStoreSellItemResult()
      {
        Result = result,
        ItemId = id,
        Amount = amount
      };
      MyLog.Default.WriteLine(string.Format("SendSellItemResult - {0}, {1}, {2}, {3}, {4}, {5}", (object) result, (object) id, (object) name, (object) price, (object) amount, (object) MyEventContext.Current.Sender));
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, MyStoreSellItemResult>(this, (Func<MyStoreBlock, Action<MyStoreSellItemResult>>) (x => new Action<MyStoreSellItemResult>(x.OnSellItemResult)), storeSellItemResult, MyEventContext.Current.Sender);
    }

    [Event(null, 1483)]
    [Reliable]
    [Client]
    private void OnSellItemResult(MyStoreSellItemResult result)
    {
      Action<MyStoreSellItemResult> sellItemCallback = this.m_localRequestSellItemCallback;
      if (sellItemCallback != null)
        sellItemCallback(result);
      this.m_localRequestSellItemCallback = (Action<MyStoreSellItemResult>) null;
    }

    private bool HasAccess()
    {
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      this.GetPlayer(identityId);
      MyRelationsBetweenPlayerAndBlock userRelationToOwner = this.GetUserRelationToOwner(identityId);
      bool flag = false;
      IMyFaction playerFaction = MySession.Static.Factions.TryGetPlayerFaction(this.OwnerId);
      if (playerFaction != null)
        flag = MySession.Static.Factions.IsNpcFaction(playerFaction.Tag);
      return this.AnyoneCanUse && (userRelationToOwner != MyRelationsBetweenPlayerAndBlock.Enemies || !flag) || this.HasPlayerAccess(identityId);
    }

    private MyPlayer GetPlayer(long identityId)
    {
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      return identity == null || identity.Character == null ? (MyPlayer) null : MyPlayer.GetPlayerFromCharacter(identity.Character);
    }

    public void CreateGetConnectedGridInventoriesRequest(Action<List<long>> resultCallback)
    {
      this.m_localRequestConnectedInventoriesItemCallback = resultCallback;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock>(this, (Func<MyStoreBlock, Action>) (x => new Action(x.GetConnectedGridInventories)));
    }

    [Event(null, 1522)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void GetConnectedGridInventories()
    {
      if (!this.HasAccess())
        return;
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      List<long> longList = new List<long>();
      foreach (MySlimBlock block in this.CubeGrid.GetBlocks())
      {
        if (block.FatBlock is MyShipConnector fatBlock && fatBlock != null && (fatBlock.Connected && (bool) fatBlock.TradingEnabled) && ((Sandbox.ModAPI.Ingame.IMyShipConnector) fatBlock).IsConnected)
          longList = fatBlock.GetInventoryEntities(identityId);
      }
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, List<long>>(this, (Func<MyStoreBlock, Action<List<long>>>) (x => new Action<List<long>>(x.OnGetConnectedGridInventoriesResult)), longList, MyEventContext.Current.Sender);
    }

    [Event(null, 1550)]
    [Reliable]
    [Client]
    private void OnGetConnectedGridInventoriesResult(List<long> inventories)
    {
      Action<List<long>> inventoriesItemCallback = this.m_localRequestConnectedInventoriesItemCallback;
      if (inventoriesItemCallback != null)
        inventoriesItemCallback(inventories);
      this.m_localRequestConnectedInventoriesItemCallback = (Action<List<long>>) null;
    }

    public void CreateGetGridInventoriesRequest(Action<List<long>> resultCallback)
    {
      this.m_localRequestInventoriesItemCallback = resultCallback;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock>(this, (Func<MyStoreBlock, Action>) (x => new Action(x.GetGridInventories)));
    }

    [Event(null, 1563)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void GetGridInventories()
    {
      if (!this.HasAccess())
        return;
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      List<long> inventoryIds = new List<long>();
      if (this.CubeGrid.GridSystems.ConveyorSystem != null)
        this.CubeGrid.GridSystems.ConveyorSystem.GetGridInventories((MyEntity) this, (List<MyEntity>) null, identityId, inventoryIds);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, List<long>>(this, (Func<MyStoreBlock, Action<List<long>>>) (x => new Action<List<long>>(x.OnGetGridInventoriesResult)), inventoryIds, MyEventContext.Current.Sender);
    }

    [Event(null, 1582)]
    [Reliable]
    [Client]
    private void OnGetGridInventoriesResult(List<long> inventories)
    {
      Action<List<long>> inventoriesItemCallback = this.m_localRequestInventoriesItemCallback;
      if (inventoriesItemCallback != null)
        inventoriesItemCallback(inventories);
      this.m_localRequestInventoriesItemCallback = (Action<List<long>>) null;
    }

    internal void CreateNewOfferRequest(
      SerializableDefinitionId itemId,
      int offerAmount,
      int offerPricePerUnit,
      Action<MyStoreCreationResult> resultCallback)
    {
      this.m_localRequestCreateOfferCallback = resultCallback;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, SerializableDefinitionId, int, int>(this, (Func<MyStoreBlock, Action<SerializableDefinitionId, int, int>>) (x => new Action<SerializableDefinitionId, int, int>(x.CreateNewOffer)), itemId, offerAmount, offerPricePerUnit);
    }

    [Event(null, 1595)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void CreateNewOffer(SerializableDefinitionId itemId, int amount, int pricePerUnit)
    {
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      if ((!this.HasPlayerAccess(identityId) ? 0 : (this.OwnerId == identityId ? 1 : 0)) == 0)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, MyStoreCreationResult>(this, (Func<MyStoreBlock, Action<MyStoreCreationResult>>) (x => new Action<MyStoreCreationResult>(x.OnCreateNewOfferResult)), this.CreateNewOffer_Internal(itemId, amount, pricePerUnit, true, out MyStoreItem _), MyEventContext.Current.Sender);
    }

    private MyStoreCreationResult CreateNewOffer_Internal(
      SerializableDefinitionId itemId,
      int amount,
      int pricePerUnit,
      bool chargeListingFee,
      out MyStoreItem item)
    {
      item = (MyStoreItem) null;
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component == null)
      {
        MyLog.Default.WriteToLogAndAssert("CreateNewOffer_Internal - Economy session component not found.");
        return MyStoreCreationResult.Error;
      }
      if (this.PlayerItems.Count >= component.GetStoreCreationLimitPerPlayer())
        return MyStoreCreationResult.Fail_CreationLimitHard;
      MyAccountInfo account;
      if (!MyBankingSystem.Static.TryGetAccountInfo(this.OwnerId, out account))
      {
        MyLog.Default.WriteToLogAndAssert("CreateNewOffer_Internal - Owner does not have account.");
        return MyStoreCreationResult.Error;
      }
      long num = (long) amount * (long) pricePerUnit;
      if (num <= 0L)
      {
        MyLog.Default.WriteToLogAndAssert("CreateNewOffer_Internal - Wrong price.");
        return MyStoreCreationResult.Error;
      }
      if (component.GetMinimumItemPrice(itemId) > pricePerUnit)
        return MyStoreCreationResult.Fail_PricePerUnitIsLowerThanMinimum;
      if (chargeListingFee)
      {
        long amount1 = (long) ((double) num * (double) component.EconomyDefinition.ListingFee);
        if (amount1 > account.Balance)
          return MyStoreCreationResult.Error;
        component.AddCurrencyDestroyed(amount1);
        MyBankingSystem.ChangeBalance(this.OwnerId, -amount1);
      }
      MyStoreItem myStoreItem = new MyStoreItem(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM), (MyDefinitionId) itemId, amount, pricePerUnit, StoreItemTypes.Offer);
      this.PlayerItems.Add(myStoreItem);
      item = myStoreItem;
      return MyStoreCreationResult.Success;
    }

    [Event(null, 1666)]
    [Reliable]
    [Client]
    private void OnCreateNewOfferResult(MyStoreCreationResult result)
    {
      Action<MyStoreCreationResult> createOfferCallback = this.m_localRequestCreateOfferCallback;
      if (createOfferCallback != null)
        createOfferCallback(result);
      this.m_localRequestCreateOfferCallback = (Action<MyStoreCreationResult>) null;
    }

    internal void CreateNewOrderRequest(
      SerializableDefinitionId itemId,
      int orderAmount,
      int orderPricePerUnit,
      Action<MyStoreCreationResult> resultCallback)
    {
      this.m_localRequestCreateOrderCallback = resultCallback;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, SerializableDefinitionId, int, int>(this, (Func<MyStoreBlock, Action<SerializableDefinitionId, int, int>>) (x => new Action<SerializableDefinitionId, int, int>(x.CreateNewOrder)), itemId, orderAmount, orderPricePerUnit);
    }

    [Event(null, 1679)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void CreateNewOrder(SerializableDefinitionId itemId, int amount, int pricePerUnit)
    {
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      if ((!this.HasPlayerAccess(identityId) ? 0 : (this.OwnerId == identityId ? 1 : 0)) == 0)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, MyStoreCreationResult>(this, (Func<MyStoreBlock, Action<MyStoreCreationResult>>) (x => new Action<MyStoreCreationResult>(x.OnCreateNewOrderResult)), this.CreateNewOrder_Internal(itemId, amount, pricePerUnit, true, out MyStoreItem _), MyEventContext.Current.Sender);
    }

    private MyStoreCreationResult CreateNewOrder_Internal(
      SerializableDefinitionId itemId,
      int amount,
      int pricePerUnit,
      bool chargeListingFee,
      out MyStoreItem item)
    {
      item = (MyStoreItem) null;
      MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
      if (component == null)
      {
        MyLog.Default.WriteToLogAndAssert("CreateNewOrder_Internal - Economy session component not found.");
        return MyStoreCreationResult.Error;
      }
      if (this.PlayerItems.Count >= component.GetStoreCreationLimitPerPlayer())
        return MyStoreCreationResult.Fail_CreationLimitHard;
      MyAccountInfo account;
      if (!MyBankingSystem.Static.TryGetAccountInfo(this.OwnerId, out account))
      {
        MyLog.Default.WriteToLogAndAssert("CreateNewOrder_Internal - Owner does not have account.");
        return MyStoreCreationResult.Error;
      }
      long num = (long) amount * (long) pricePerUnit;
      if (num <= 0L)
      {
        MyLog.Default.WriteToLogAndAssert("CreateNewOrder_Internal - Wrong price.");
        return MyStoreCreationResult.Error;
      }
      if (chargeListingFee)
      {
        long amount1 = (long) ((double) num * (double) component.EconomyDefinition.ListingFee);
        if (amount1 > account.Balance)
          return MyStoreCreationResult.Error;
        component.AddCurrencyDestroyed(amount1);
        MyBankingSystem.ChangeBalance(this.OwnerId, -amount1);
      }
      MyStoreItem myStoreItem = new MyStoreItem(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM), (MyDefinitionId) itemId, amount, pricePerUnit, StoreItemTypes.Order);
      this.PlayerItems.Add(myStoreItem);
      item = myStoreItem;
      return MyStoreCreationResult.Success;
    }

    [Event(null, 1745)]
    [Reliable]
    [Client]
    private void OnCreateNewOrderResult(MyStoreCreationResult result)
    {
      Action<MyStoreCreationResult> createOrderCallback = this.m_localRequestCreateOrderCallback;
      if (createOrderCallback != null)
        createOrderCallback(result);
      this.m_localRequestCreateOrderCallback = (Action<MyStoreCreationResult>) null;
    }

    public void CreateCancelStoreItemRequest(long id, Action<bool> resultCallback)
    {
      this.m_localRequestCancelStoreItemCallback = resultCallback;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, long>(this, (Func<MyStoreBlock, Action<long>>) (x => new Action<long>(x.CancelStoreItemServer)), id);
    }

    [Event(null, 1758)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void CancelStoreItemServer(long id)
    {
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      if ((!this.HasPlayerAccess(identityId) ? 0 : (this.OwnerId == identityId ? 1 : 0)) == 0)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, bool>(this, (Func<MyStoreBlock, Action<bool>>) (x => new Action<bool>(x.OnCancelStoreItemResult)), this.CancelStoreItem(id), MyEventContext.Current.Sender);
    }

    public bool CancelStoreItem(long id)
    {
      MyStoreItem myStoreItem = (MyStoreItem) null;
      foreach (MyStoreItem playerItem in this.PlayerItems)
      {
        if (playerItem.Id == id)
        {
          myStoreItem = playerItem;
          break;
        }
      }
      if (myStoreItem == null)
        return false;
      if (myStoreItem.OnCancel != null)
        myStoreItem.OnCancel();
      this.PlayerItems.Remove(myStoreItem);
      return true;
    }

    [Event(null, 1801)]
    [Reliable]
    [Client]
    private void OnCancelStoreItemResult(bool result)
    {
      Action<bool> storeItemCallback = this.m_localRequestCancelStoreItemCallback;
      if (storeItemCallback != null)
        storeItemCallback(result);
      this.m_localRequestCancelStoreItemCallback = (Action<bool>) null;
    }

    internal void CreateChangeBalanceRequest(
      int amount,
      long targetEntityId,
      Action<MyStoreBuyItemResults> resultCallback)
    {
      this.m_localRequestChangeBalanceCallback = resultCallback;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, int, long>(this, (Func<MyStoreBlock, Action<int, long>>) (x => new Action<int, long>(x.ChangeBalance)), amount, targetEntityId);
    }

    [Event(null, 1814)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void ChangeBalance(int amount, long targetEntityId)
    {
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      MyPlayer player = this.GetPlayer(identityId);
      if (player == null || player.Character == null)
      {
        MyLog.Default.WriteToLogAndAssert("ChangeBalance - Player not found.");
      }
      else
      {
        MyEntity entity = (MyEntity) null;
        if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(targetEntityId, out entity))
          MyLog.Default.WriteToLogAndAssert("ChangeBalance - Entity not found.");
        else if (entity is MyCubeBlock myCubeBlock && !myCubeBlock.CubeGrid.BigOwners.Contains(player.Identity.IdentityId))
          MyLog.Default.WriteToLogAndAssert("ChangeBalance - Player is not big owner of the grid.");
        else if (entity is MyCharacter && player.Character != entity)
        {
          MyLog.Default.WriteToLogAndAssert("ChangeBalance - Player entity and inventory entity is different.");
        }
        else
        {
          MyInventory inventory;
          if (!entity.TryGetInventory(out inventory))
          {
            MyLog.Default.WriteToLogAndAssert("ChangeBalance - Inventory not found.");
          }
          else
          {
            MyAccountInfo account;
            if (!MyBankingSystem.Static.TryGetAccountInfo(identityId, out account))
            {
              MyLog.Default.WriteToLogAndAssert("ChangeBalance - Player does not have account.");
            }
            else
            {
              MyDefinitionId physicalItemId = MyBankingSystem.BankingSystemDefinition.PhysicalItemId;
              if (amount > 0)
              {
                MyFixedPoint itemAmount = inventory.GetItemAmount(physicalItemId, MyItemFlags.None, false);
                if ((MyFixedPoint) amount > itemAmount)
                {
                  Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, MyStoreBuyItemResults>(this, (Func<MyStoreBlock, Action<MyStoreBuyItemResults>>) (x => new Action<MyStoreBuyItemResults>(x.OnChangeBalanceResult)), MyStoreBuyItemResults.WrongAmount, MyEventContext.Current.Sender);
                  return;
                }
                if (MyBankingSystem.ChangeBalance(identityId, (long) amount))
                {
                  MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) physicalItemId);
                  inventory.RemoveItemsOfType((MyFixedPoint) amount, physicalItemId, MyItemFlags.None, false);
                }
              }
              else
              {
                int num = -amount;
                if ((long) num > account.Balance)
                {
                  Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, MyStoreBuyItemResults>(this, (Func<MyStoreBlock, Action<MyStoreBuyItemResults>>) (x => new Action<MyStoreBuyItemResults>(x.OnChangeBalanceResult)), MyStoreBuyItemResults.NotEnoughMoney, MyEventContext.Current.Sender);
                  return;
                }
                if (!inventory.CheckConstraint(physicalItemId))
                {
                  MyLog.Default.WriteToLogAndAssert("ChangeBalance - Item can not be transfered to this inventory.");
                  return;
                }
                if (!inventory.CanItemsBeAdded((MyFixedPoint) num, physicalItemId))
                {
                  Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, MyStoreBuyItemResults>(this, (Func<MyStoreBlock, Action<MyStoreBuyItemResults>>) (x => new Action<MyStoreBuyItemResults>(x.OnChangeBalanceResult)), MyStoreBuyItemResults.NotEnoughInventorySpace, MyEventContext.Current.Sender);
                  return;
                }
                if (MyBankingSystem.ChangeBalance(identityId, (long) amount))
                {
                  MyObjectBuilder_Base newObject = MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) physicalItemId);
                  inventory.AddItems((MyFixedPoint) num, newObject);
                }
              }
              Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, MyStoreBuyItemResults>(this, (Func<MyStoreBlock, Action<MyStoreBuyItemResults>>) (x => new Action<MyStoreBuyItemResults>(x.OnChangeBalanceResult)), MyStoreBuyItemResults.Success, MyEventContext.Current.Sender);
            }
          }
        }
      }
    }

    [Event(null, 1906)]
    [Reliable]
    [Client]
    private void OnChangeBalanceResult(MyStoreBuyItemResults result)
    {
      Action<MyStoreBuyItemResults> changeBalanceCallback = this.m_localRequestChangeBalanceCallback;
      if (changeBalanceCallback != null)
        changeBalanceCallback(result);
      this.m_localRequestChangeBalanceCallback = (Action<MyStoreBuyItemResults>) null;
    }

    internal void ShowPreview(long storeItemId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyStoreBlock, long>(this, (Func<MyStoreBlock, Action<long>>) (x => new Action<long>(x.ShowPreviewImplementation)), storeItemId);

    [Event(null, 1918)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void ShowPreviewImplementation(long storeItemId)
    {
      MyProjectorBase myProjectorBase = (MyProjectorBase) null;
      foreach (MySlimBlock block in this.CubeGrid.GetBlocks())
      {
        if (block.FatBlock is MyProjectorBase fatBlock && fatBlock.DisplayNameText == "Store Preview")
        {
          myProjectorBase = fatBlock;
          break;
        }
      }
      if (myProjectorBase == null)
        return;
      MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(this.CubeGrid.EntityId);
      if (stationByGridId == null)
        return;
      MyStoreItem storeItemById = stationByGridId.GetStoreItemById(storeItemId);
      if (storeItemById == null)
        return;
      myProjectorBase.SelectPrefab(storeItemById.PrefabName);
    }

    int IMyInventoryOwner.InventoryCount => this.InventoryCount;

    long IMyInventoryOwner.EntityId => this.EntityId;

    bool IMyInventoryOwner.HasInventory => this.HasInventory;

    bool IMyInventoryOwner.UseConveyorSystem
    {
      get => this.UseConveyorSystem;
      set => this.UseConveyorSystem = value;
    }

    VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(
      int index)
    {
      return (VRage.Game.ModAPI.Ingame.IMyInventory) MyEntityExtensions.GetInventory(this, index);
    }

    public void InitializeConveyorEndpoint()
    {
      this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
    }

    public bool AllowSelfPulling() => false;

    public PullInformation GetPullInformation() => new PullInformation()
    {
      Inventory = MyEntityExtensions.GetInventory(this),
      OwnerID = this.OwnerId,
      Constraint = new MyInventoryConstraint("Empty Constraint")
    };

    public PullInformation GetPushInformation() => new PullInformation()
    {
      Inventory = MyEntityExtensions.GetInventory(this),
      OwnerID = this.OwnerId,
      Constraint = new MyInventoryConstraint("Empty constraint")
    };

    public MyStoreInsertResults InsertOffer(MyStoreItemData item, out long id)
    {
      id = 0L;
      MyStoreItem myStoreItem;
      int newOfferInternal = (int) this.CreateNewOffer_Internal(item.ItemId, item.Amount, item.PricePerUnit, false, out myStoreItem);
      if (newOfferInternal == 0)
      {
        id = myStoreItem.Id;
        myStoreItem.SetActions(item.OnTransaction, item.OnCancel);
      }
      return MyStoreBlock.Convert((MyStoreCreationResult) newOfferInternal);
    }

    public MyStoreInsertResults InsertOffer(
      MyStoreItemDataSimple item,
      out long id)
    {
      return this.InsertOffer(new MyStoreItemData((SerializableDefinitionId) (MyDefinitionId) item.ItemId, item.Amount, item.PricePerUnit, (Action<int, int, long, long, long>) null, (Action) null), out id);
    }

    public MyStoreInsertResults InsertOrder(MyStoreItemData item, out long id)
    {
      id = 0L;
      MyStoreItem myStoreItem;
      int newOrderInternal = (int) this.CreateNewOrder_Internal(item.ItemId, item.Amount, item.PricePerUnit, false, out myStoreItem);
      if (newOrderInternal == 0)
      {
        id = myStoreItem.Id;
        myStoreItem.SetActions(item.OnTransaction, item.OnCancel);
      }
      return MyStoreBlock.Convert((MyStoreCreationResult) newOrderInternal);
    }

    public MyStoreInsertResults InsertOrder(
      MyStoreItemDataSimple item,
      out long id)
    {
      return this.InsertOrder(new MyStoreItemData((SerializableDefinitionId) (MyDefinitionId) item.ItemId, item.Amount, item.PricePerUnit, (Action<int, int, long, long, long>) null, (Action) null), out id);
    }

    private static MyStoreInsertResults Convert(MyStoreCreationResult input)
    {
      switch (input)
      {
        case MyStoreCreationResult.Success:
          return MyStoreInsertResults.Success;
        case MyStoreCreationResult.Fail_CreationLimitHard:
          return MyStoreInsertResults.Fail_StoreLimitReached;
        case MyStoreCreationResult.Fail_PricePerUnitIsLowerThanMinimum:
          return MyStoreInsertResults.Fail_PricePerUnitIsLessThanMinimum;
        default:
          return MyStoreInsertResults.Error;
      }
    }

    public void GetPlayerStoreItems(List<MyStoreQueryItem> storeItems)
    {
      if (storeItems == null)
        return;
      foreach (MyStoreItem playerItem in this.PlayerItems)
      {
        MyStoreQueryItem myStoreQueryItem = new MyStoreQueryItem()
        {
          Id = playerItem.Id,
          ItemId = playerItem.Item.Value,
          Amount = playerItem.Amount,
          PricePerUnit = playerItem.PricePerUnit
        };
        storeItems.Add(myStoreQueryItem);
      }
    }

    public override void OnRemovedByCubeBuilder()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnRemovedByCubeBuilder();
    }

    public override void OnDestroy()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this), true);
      base.OnDestroy();
    }

    protected sealed class OnChangeTextRequest\u003C\u003ESystem_Int32\u0023System_String : ICallSite<MyStoreBlock, int, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in int panelIndex,
        in string text,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeTextRequest(panelIndex, text);
      }
    }

    protected sealed class OnUpdateSpriteCollection\u003C\u003ESystem_Int32\u0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyStoreBlock, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in int panelIndex,
        in MySerializableSpriteCollection sprites,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnUpdateSpriteCollection(panelIndex, sprites);
      }
    }

    protected sealed class OnRemoveSelectedImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyStoreBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in int panelIndex,
        in int[] selection,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveSelectedImageRequest(panelIndex, selection);
      }
    }

    protected sealed class OnSelectImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyStoreBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in int panelIndex,
        in int[] selection,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSelectImageRequest(panelIndex, selection);
      }
    }

    protected sealed class OnChangeOpenSuccess\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyStoreBlock, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in bool isPublic,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
      }
    }

    protected sealed class OnChangeOpenRequest\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyStoreBlock, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in bool isPublic,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
      }
    }

    protected sealed class OnChangeDescription\u003C\u003ESystem_String\u0023System_Boolean : ICallSite<MyStoreBlock, string, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in string description,
        in bool isPublic,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeDescription(description, isPublic);
      }
    }

    protected sealed class GetStoreItems\u003C\u003E : ICallSite<MyStoreBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetStoreItems();
      }
    }

    protected sealed class OnGetStoreItemsResult\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_Entities_Blocks_MyStoreItem\u003E\u0023System_Int64\u0023System_Single\u0023System_Single : ICallSite<MyStoreBlock, List<MyStoreItem>, long, float, float, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in List<MyStoreItem> storeItems,
        in long lastEconomyTick,
        in float offersBonus,
        in float ordersBonus,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnGetStoreItemsResult(storeItems, lastEconomyTick, offersBonus, ordersBonus);
      }
    }

    protected sealed class OnPlayerStoreItemsChanged_Broacast\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_Entities_Blocks_MyStoreItem\u003E\u0023System_Int64 : ICallSite<MyStoreBlock, List<MyStoreItem>, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in List<MyStoreItem> storeItems,
        in long lastEconomyTick,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnPlayerStoreItemsChanged_Broacast(storeItems, lastEconomyTick);
      }
    }

    protected sealed class BuyItem\u003C\u003ESystem_Int64\u0023System_Int32\u0023System_Int64\u0023System_Int64 : ICallSite<MyStoreBlock, long, int, long, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in long id,
        in int amount,
        in long targetEntityId,
        in long lastEconomyTick,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.BuyItem(id, amount, targetEntityId, lastEconomyTick);
      }
    }

    protected sealed class OnBuyItemResult\u003C\u003ESandbox_Game_Entities_Blocks_MyStoreBuyItemResult : ICallSite<MyStoreBlock, MyStoreBuyItemResult, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in MyStoreBuyItemResult result,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnBuyItemResult(result);
      }
    }

    protected sealed class SellItem\u003C\u003ESystem_Int64\u0023System_Int32\u0023System_Int64\u0023System_Int64 : ICallSite<MyStoreBlock, long, int, long, long, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in long id,
        in int amount,
        in long sourceEntityId,
        in long lastEconomyTick,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SellItem(id, amount, sourceEntityId, lastEconomyTick);
      }
    }

    protected sealed class OnSellItemResult\u003C\u003ESandbox_Game_Entities_Blocks_MyStoreSellItemResult : ICallSite<MyStoreBlock, MyStoreSellItemResult, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in MyStoreSellItemResult result,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSellItemResult(result);
      }
    }

    protected sealed class GetConnectedGridInventories\u003C\u003E : ICallSite<MyStoreBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetConnectedGridInventories();
      }
    }

    protected sealed class OnGetConnectedGridInventoriesResult\u003C\u003ESystem_Collections_Generic_List`1\u003CSystem_Int64\u003E : ICallSite<MyStoreBlock, List<long>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in List<long> inventories,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnGetConnectedGridInventoriesResult(inventories);
      }
    }

    protected sealed class GetGridInventories\u003C\u003E : ICallSite<MyStoreBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetGridInventories();
      }
    }

    protected sealed class OnGetGridInventoriesResult\u003C\u003ESystem_Collections_Generic_List`1\u003CSystem_Int64\u003E : ICallSite<MyStoreBlock, List<long>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in List<long> inventories,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnGetGridInventoriesResult(inventories);
      }
    }

    protected sealed class CreateNewOffer\u003C\u003EVRage_ObjectBuilders_SerializableDefinitionId\u0023System_Int32\u0023System_Int32 : ICallSite<MyStoreBlock, SerializableDefinitionId, int, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in SerializableDefinitionId itemId,
        in int amount,
        in int pricePerUnit,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CreateNewOffer(itemId, amount, pricePerUnit);
      }
    }

    protected sealed class OnCreateNewOfferResult\u003C\u003ESandbox_Game_Entities_Blocks_MyStoreCreationResult : ICallSite<MyStoreBlock, MyStoreCreationResult, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in MyStoreCreationResult result,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnCreateNewOfferResult(result);
      }
    }

    protected sealed class CreateNewOrder\u003C\u003EVRage_ObjectBuilders_SerializableDefinitionId\u0023System_Int32\u0023System_Int32 : ICallSite<MyStoreBlock, SerializableDefinitionId, int, int, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in SerializableDefinitionId itemId,
        in int amount,
        in int pricePerUnit,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CreateNewOrder(itemId, amount, pricePerUnit);
      }
    }

    protected sealed class OnCreateNewOrderResult\u003C\u003ESandbox_Game_Entities_Blocks_MyStoreCreationResult : ICallSite<MyStoreBlock, MyStoreCreationResult, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in MyStoreCreationResult result,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnCreateNewOrderResult(result);
      }
    }

    protected sealed class CancelStoreItemServer\u003C\u003ESystem_Int64 : ICallSite<MyStoreBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in long id,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CancelStoreItemServer(id);
      }
    }

    protected sealed class OnCancelStoreItemResult\u003C\u003ESystem_Boolean : ICallSite<MyStoreBlock, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in bool result,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnCancelStoreItemResult(result);
      }
    }

    protected sealed class ChangeBalance\u003C\u003ESystem_Int32\u0023System_Int64 : ICallSite<MyStoreBlock, int, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in int amount,
        in long targetEntityId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ChangeBalance(amount, targetEntityId);
      }
    }

    protected sealed class OnChangeBalanceResult\u003C\u003ESandbox_Game_Entities_Blocks_MyStoreBuyItemResults : ICallSite<MyStoreBlock, MyStoreBuyItemResults, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in MyStoreBuyItemResults result,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeBalanceResult(result);
      }
    }

    protected sealed class ShowPreviewImplementation\u003C\u003ESystem_Int64 : ICallSite<MyStoreBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyStoreBlock @this,
        in long storeItemId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ShowPreviewImplementation(storeItemId);
      }
    }

    protected class m_useConveyorSystem\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyStoreBlock) obj0).m_useConveyorSystem = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_anyoneCanUse\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyStoreBlock) obj0).m_anyoneCanUse = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyStoreBlock\u003C\u003EActor : IActivator, IActivator<MyStoreBlock>
    {
      object IActivator.CreateInstance() => (object) new MyStoreBlock();

      MyStoreBlock IActivator<MyStoreBlock>.CreateInstance() => new MyStoreBlock();
    }
  }
}
