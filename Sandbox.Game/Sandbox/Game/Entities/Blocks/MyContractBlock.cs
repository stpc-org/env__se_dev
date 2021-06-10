// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyContractBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Components;
using Sandbox.Game.Contracts;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Sync;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_ContractBlock))]
  public class MyContractBlock : MyFunctionalBlock, IMyConveyorEndpointBlock, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner, IMyTextSurfaceProvider
  {
    private MyMultiTextPanelComponent m_multiPanel;
    private MyGuiScreenTextPanel m_textBoxMultiPanel;
    private static Action<List<MyObjectBuilder_Contract>> m_localStaticRequestActiveContractsCallback;
    private static Action<MyContractResults> m_localStaticRequestAbandonCallback;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_anyoneCanUse;
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;
    private Action<bool> m_localRequestContractBlockStatucCallback;
    private Action<List<MyObjectBuilder_Contract>> m_localRequestAvailableContractsCallback;
    private Action<List<MyObjectBuilder_Contract>> m_localRequestAdministrableContractsCallback;
    private Action<List<MyObjectBuilder_Contract>, long, long> m_localRequestActiveContractsCallback;
    private Action<List<MyObjectBuilder_ContractCondition>> m_localRequestActiveConditionsCallback;
    private Action<MyContractResults> m_localRequestAcceptCallback;
    private Action<MyContractResults> m_localRequestFinishCallback;
    private Action<MyContractResults> m_localRequestAbandonCallback;
    private Action<bool, List<MyContractBlock.MyTargetEntityInfoWrapper>, long> m_localRequestConnectedEntitiesCallback;
    private long m_localRequestConnectedentitiesContractId;
    private Action<List<MyContractBlock.MyEntityInfoWrapper>> m_localRequestOwnedContractBlocksCallback;
    private Action<List<MyContractBlock.MyEntityInfoWrapper>> m_localRequestOwnedGridsCallback;
    private Action<MyContractCreationResults> m_localRequestCreateCustomContractCallback;
    private Action<bool> m_localRequestDeleteCustomContractCallback;
    private MyResourceStateEnum m_currentState = MyResourceStateEnum.NoPower;
    private bool m_isTextPanelOpen;

    public MyContractBlockDefinition BlockDefinition => (MyContractBlockDefinition) base.BlockDefinition;

    public bool AnyoneCanUse
    {
      get => (bool) this.m_anyoneCanUse;
      set => this.m_anyoneCanUse.Value = value;
    }

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public override MyCubeBlockHighlightModes HighlightMode => this.AnyoneCanUse ? MyCubeBlockHighlightModes.AlwaysCanUse : MyCubeBlockHighlightModes.Default;

    public MyContractBlock() => this.Render = (MyRenderComponentBase) new MyRenderComponentScreenAreas((MyEntity) this);

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_ContractBlock builderContractBlock = objectBuilder as MyObjectBuilder_ContractBlock;
      this.AnyoneCanUse = builderContractBlock.AnyoneCanUse;
      this.InitializeConveyorEndpoint();
      if (this.BlockDefinition.ScreenAreas != null && this.BlockDefinition.ScreenAreas.Count > 0)
      {
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
        this.m_multiPanel = new MyMultiTextPanelComponent((MyTerminalBlock) this, this.BlockDefinition.ScreenAreas, builderContractBlock.TextPanels);
        this.m_multiPanel.Init(new Action<int, int[]>(this.SendAddImagesToSelectionRequest), new Action<int, int[]>(this.SendRemoveSelectedImageRequest), new Action<int, string>(this.ChangeTextRequest), new Action<int, MySerializableSpriteCollection>(this.UpdateSpriteCollection));
      }
      this.OnClose += new Action<MyEntity>(this.OnClose_Callback);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    private void OnClose_Callback(MyEntity obj)
    {
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return;
      component.ContractBlockDestroyed(this.EntityId);
      this.OnClose -= new Action<MyEntity>(this.OnClose_Callback);
    }

    protected override bool CheckIsWorking() => base.CheckIsWorking() && this.m_currentState == MyResourceStateEnum.Ok;

    protected override void OnStartWorking()
    {
      base.OnStartWorking();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected override void OnStopWorking()
    {
      base.OnStopWorking();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_ContractBlock builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_ContractBlock;
      builderCubeBlock.AnyoneCanUse = this.AnyoneCanUse;
      if (this.m_multiPanel != null)
        builderCubeBlock.TextPanels = this.m_multiPanel.Serialize();
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

    public void UpdateScreen() => this.m_multiPanel?.UpdateAfterSimulation(this.IsWorking);

    private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection) => MyMultiplayer.RaiseEvent<MyContractBlock, int, int[]>(this, (Func<MyContractBlock, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnSelectImageRequest)), panelIndex, selection);

    private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection) => MyMultiplayer.RaiseEvent<MyContractBlock, int, int[]>(this, (Func<MyContractBlock, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnRemoveSelectedImageRequest)), panelIndex, selection);

    private void ChangeTextRequest(int panelIndex, string text) => MyMultiplayer.RaiseEvent<MyContractBlock, int, string>(this, (Func<MyContractBlock, Action<int, string>>) (x => new Action<int, string>(x.OnChangeTextRequest)), panelIndex, text);

    [Event(null, 267)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnChangeTextRequest(int panelIndex, [Nullable] string text) => this.m_multiPanel?.ChangeText(panelIndex, text);

    private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyMultiplayer.RaiseEvent<MyContractBlock, int, MySerializableSpriteCollection>(this, (Func<MyContractBlock, Action<int, MySerializableSpriteCollection>>) (x => new Action<int, MySerializableSpriteCollection>(x.OnUpdateSpriteCollection)), panelIndex, sprites);
    }

    [Event(null, 283)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    [DistanceRadius(32f)]
    private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites) => this.m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);

    int IMyTextSurfaceProvider.SurfaceCount => this.m_multiPanel == null ? 0 : this.m_multiPanel.SurfaceCount;

    IMyTextSurface IMyTextSurfaceProvider.GetSurface(int index) => this.m_multiPanel == null ? (IMyTextSurface) null : this.m_multiPanel.GetSurface(index);

    [Event(null, 299)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
    {
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.RemoveItems(panelIndex, selection);
    }

    [Event(null, 308)]
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

    [Event(null, 388)]
    [Reliable]
    [Broadcast]
    private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic) => this.OnChangeOpen(isOpen, editable, user, isPublic);

    private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0, bool isPublic = false) => MyMultiplayer.RaiseEvent<MyContractBlock, bool, bool, ulong, bool>(this, (Func<MyContractBlock, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenRequest)), isOpen, editable, user, isPublic);

    [Event(null, 399)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      if (((!Sandbox.Game.Multiplayer.Sync.IsServer ? 0 : (this.IsTextPanelOpen ? 1 : 0)) & (isOpen ? 1 : 0)) != 0)
        return;
      this.OnChangeOpen(isOpen, editable, user, isPublic);
      MyMultiplayer.RaiseEvent<MyContractBlock, bool, bool, ulong, bool>(this, (Func<MyContractBlock, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenSuccess)), isOpen, editable, user, isPublic);
    }

    private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      this.IsTextPanelOpen = isOpen;
      if (((Sandbox.Engine.Platform.Game.IsDedicated ? 0 : ((long) user == (long) Sandbox.Game.Multiplayer.Sync.MyId ? 1 : 0)) & (isOpen ? 1 : 0)) == 0)
        return;
      this.OpenWindow(editable, false, isPublic);
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
        MyMultiplayer.RaiseEvent<MyContractBlock, string, bool>(this, (Func<MyContractBlock, Action<string, bool>>) (x => new Action<string, bool>(x.OnChangeDescription)), description.ToString(), isPublic);
      }
    }

    [Event(null, 467)]
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
      if (MyTerminalControlFactory.AreControlsCreated<MyContractBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlCheckbox<MyContractBlock> checkbox = new MyTerminalControlCheckbox<MyContractBlock>("AnyoneCanUse", MySpaceTexts.BlockPropertyText_AnyoneCanUse, MySpaceTexts.BlockPropertyDescription_AnyoneCanUse);
      checkbox.Getter = (MyTerminalValueControl<MyContractBlock, bool>.GetterDelegate) (x => x.AnyoneCanUse);
      checkbox.Setter = (MyTerminalValueControl<MyContractBlock, bool>.SetterDelegate) ((x, v) => x.AnyoneCanUse = v);
      checkbox.EnableAction<MyContractBlock>();
      MyTerminalControlFactory.AddControl<MyContractBlock>((MyTerminalControl<MyContractBlock>) checkbox);
      MyMultiTextPanelComponent.CreateTerminalControls<MyContractBlock>();
    }

    public void InitializeConveyorEndpoint() => this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    public bool AllowSelfPulling() => false;

    public PullInformation GetPullInformation() => (PullInformation) null;

    internal void GetContractBlockStatus(Action<bool> resultCallback)
    {
      this.m_localRequestContractBlockStatucCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock>(this, (Func<MyContractBlock, Action>) (x => new Action(x.GetContractBlockStatus)));
    }

    internal void GetAvailableContracts(
      Action<List<MyObjectBuilder_Contract>> resultCallback)
    {
      this.m_localRequestAvailableContractsCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock>(this, (Func<MyContractBlock, Action>) (x => new Action(x.GetAvailibleContracts)));
    }

    internal void GetAdministrableContracts(
      Action<List<MyObjectBuilder_Contract>> resultCallback)
    {
      this.m_localRequestAdministrableContractsCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock>(this, (Func<MyContractBlock, Action>) (x => new Action(x.GetAdministrableContracts)));
    }

    internal void GetActiveContracts(
      long localPlayerId,
      Action<List<MyObjectBuilder_Contract>, long, long> resultCallback)
    {
      this.m_localRequestActiveContractsCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock, long>(this, (Func<MyContractBlock, Action<long>>) (x => new Action<long>(x.GetActiveContracts)), localPlayerId);
    }

    internal static void GetActiveContractsStatic(
      Action<List<MyObjectBuilder_Contract>> resultCallback)
    {
      MyContractBlock.m_localStaticRequestActiveContractsCallback = resultCallback;
      MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action>) (x => new Action(MyContractBlock.GetActiveContractsStatic)));
    }

    internal void GetAllOwnedContractBlocks(
      long localPlayerId,
      Action<List<MyContractBlock.MyEntityInfoWrapper>> resultCallback)
    {
      this.m_localRequestOwnedContractBlocksCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock, long>(this, (Func<MyContractBlock, Action<long>>) (x => new Action<long>(x.GetAllOwnedContractBlocks)), localPlayerId);
    }

    internal void GetAllOwnedGrids(
      long localPlayerId,
      Action<List<MyContractBlock.MyEntityInfoWrapper>> resultCallback)
    {
      this.m_localRequestOwnedGridsCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock, long>(this, (Func<MyContractBlock, Action<long>>) (x => new Action<long>(x.GetAllOwnedGrids)), localPlayerId);
    }

    internal void AcceptContract(
      long localPlayerId,
      long contractId,
      Action<MyContractResults> resultCallback)
    {
      this.m_localRequestAcceptCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock, long, long>(this, (Func<MyContractBlock, Action<long, long>>) (x => new Action<long, long>(x.AcceptContract)), localPlayerId, contractId);
    }

    internal void FinishContract(
      long localPlayerId,
      long contractId,
      long targetEntityId,
      Action<MyContractResults> resultCallback)
    {
      this.m_localRequestFinishCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock, long, long, long>(this, (Func<MyContractBlock, Action<long, long, long>>) (x => new Action<long, long, long>(x.FinishContract)), localPlayerId, contractId, targetEntityId);
    }

    internal void AbandonContract(
      long localPlayerId,
      long contractId,
      Action<MyContractResults> resultCallback)
    {
      this.m_localRequestAbandonCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock, long, long>(this, (Func<MyContractBlock, Action<long, long>>) (x => new Action<long, long>(x.AbandonContract)), localPlayerId, contractId);
    }

    internal static void AbandonContractStatic(
      long contractId,
      Action<MyContractResults> resultCallback)
    {
      MyContractBlock.m_localStaticRequestAbandonCallback = resultCallback;
      MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyContractBlock.AbandonContractStatic)), contractId);
    }

    internal void GetConnectedEntities(
      long localPlayerId,
      long contractId,
      Action<bool, List<MyContractBlock.MyTargetEntityInfoWrapper>, long> resultCallback)
    {
      this.m_localRequestConnectedEntitiesCallback = resultCallback;
      this.m_localRequestConnectedentitiesContractId = contractId;
      MyMultiplayer.RaiseEvent<MyContractBlock, long>(this, (Func<MyContractBlock, Action<long>>) (x => new Action<long>(x.GetConnectedEntities)), localPlayerId);
    }

    internal void CreateCustomContractDeliver(
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long targetBlockId,
      Action<MyContractCreationResults> resultCallback)
    {
      MyContractBlock.MyContractCreationDataWrapper_Deliver dataWrapperDeliver = new MyContractBlock.MyContractCreationDataWrapper_Deliver()
      {
        RewardMoney = rewardMoney,
        StartingDeposit = startingDeposit,
        DurationInMin = durationInMin,
        TargetBlockId = targetBlockId
      };
      this.m_localRequestCreateCustomContractCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock, MyContractBlock.MyContractCreationDataWrapper_Deliver>(this, (Func<MyContractBlock, Action<MyContractBlock.MyContractCreationDataWrapper_Deliver>>) (x => new Action<MyContractBlock.MyContractCreationDataWrapper_Deliver>(x.CreateCustomContractDeliver)), dataWrapperDeliver);
    }

    internal void DeleteCustomContract(long contractId, Action<bool> resultCallback)
    {
      this.m_localRequestDeleteCustomContractCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock, long>(this, (Func<MyContractBlock, Action<long>>) (x => new Action<long>(x.DeleteCustomContract)), contractId);
    }

    internal void CreateCustomContractObtainAndDeliver(
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long targetBlockId,
      MyDefinitionId itemTypeId,
      int itemAmount,
      Action<MyContractCreationResults> resultCallback)
    {
      MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver obtainAndDeliver = new MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver()
      {
        RewardMoney = rewardMoney,
        StartingDeposit = startingDeposit,
        DurationInMin = durationInMin,
        TargetBlockId = targetBlockId,
        ItemTypeId = (SerializableDefinitionId) itemTypeId,
        ItemAmount = itemAmount
      };
      this.m_localRequestCreateCustomContractCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock, MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver>(this, (Func<MyContractBlock, Action<MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver>>) (x => new Action<MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver>(x.CreateCustomContractObtainAndDeliver)), obtainAndDeliver);
    }

    internal void CreateCustomContractFind(
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long targetGridId,
      double searchRadius,
      Action<MyContractCreationResults> resultCallback)
    {
      MyContractBlock.MyContractCreationDataWrapper_Find creationDataWrapperFind = new MyContractBlock.MyContractCreationDataWrapper_Find()
      {
        RewardMoney = rewardMoney,
        StartingDeposit = startingDeposit,
        DurationInMin = durationInMin,
        TargetGridId = targetGridId,
        SearchRadius = searchRadius
      };
      this.m_localRequestCreateCustomContractCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock, MyContractBlock.MyContractCreationDataWrapper_Find>(this, (Func<MyContractBlock, Action<MyContractBlock.MyContractCreationDataWrapper_Find>>) (x => new Action<MyContractBlock.MyContractCreationDataWrapper_Find>(x.CreateCustomContractFind)), creationDataWrapperFind);
    }

    internal void CreateCustomContractRepair(
      int rewardMoney,
      int startingDeposit,
      int durationInMin,
      long targetGridId,
      Action<MyContractCreationResults> resultCallback)
    {
      MyContractBlock.MyContractCreationDataWrapper_Repair dataWrapperRepair = new MyContractBlock.MyContractCreationDataWrapper_Repair()
      {
        RewardMoney = rewardMoney,
        StartingDeposit = startingDeposit,
        DurationInMin = durationInMin,
        TargetGridId = targetGridId
      };
      this.m_localRequestCreateCustomContractCallback = resultCallback;
      MyMultiplayer.RaiseEvent<MyContractBlock, MyContractBlock.MyContractCreationDataWrapper_Repair>(this, (Func<MyContractBlock, Action<MyContractBlock.MyContractCreationDataWrapper_Repair>>) (x => new Action<MyContractBlock.MyContractCreationDataWrapper_Repair>(x.CreateCustomContractRepair)), dataWrapperRepair);
    }

    [Event(null, 647)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void GetContractBlockStatus()
    {
      if (!this.HasAccess())
        return;
      bool flag = false;
      if (MySession.Static.Factions.GetStationByGridId(this.CubeGrid.EntityId) != null)
        flag = true;
      MyMultiplayer.RaiseEvent<MyContractBlock, bool>(this, (Func<MyContractBlock, Action<bool>>) (x => new Action<bool>(x.ReceiveContractBlockStatus)), flag, MyEventContext.Current.Sender);
    }

    [Event(null, 668)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void GetAvailibleContracts()
    {
      if (!this.HasAccess())
        return;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return;
      MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(this.CubeGrid.EntityId);
      if (stationByGridId != null)
        MyMultiplayer.RaiseEvent<MyContractBlock, List<MyObjectBuilder_Contract>>(this, (Func<MyContractBlock, Action<List<MyObjectBuilder_Contract>>>) (x => new Action<List<MyObjectBuilder_Contract>>(x.ReceiveAvailableContracts)), component.GetAvailableContractsForStation_OB(stationByGridId.Id), MyEventContext.Current.Sender);
      else
        MyMultiplayer.RaiseEvent<MyContractBlock, List<MyObjectBuilder_Contract>>(this, (Func<MyContractBlock, Action<List<MyObjectBuilder_Contract>>>) (x => new Action<List<MyObjectBuilder_Contract>>(x.ReceiveAvailableContracts)), component.GetAvailableContractsForBlock_OB(this.EntityId), MyEventContext.Current.Sender);
    }

    [Event(null, 698)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void GetAdministrableContracts()
    {
      if (!this.HasAccess())
        return;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null || MySession.Static.Factions.GetStationByGridId(this.CubeGrid.EntityId) != null)
        return;
      MyMultiplayer.RaiseEvent<MyContractBlock, List<MyObjectBuilder_Contract>>(this, (Func<MyContractBlock, Action<List<MyObjectBuilder_Contract>>>) (x => new Action<List<MyObjectBuilder_Contract>>(x.ReceiveAdministrableContracts)), component.GetAvailableContractsForBlock_OB(this.EntityId), MyEventContext.Current.Sender);
    }

    [Event(null, 725)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void GetActiveContracts(long identityId)
    {
      if (!this.HasAccess())
        return;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return;
      long num = 0;
      MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(this.CubeGrid.EntityId);
      if (stationByGridId != null)
        num = stationByGridId.Id;
      long entityId = this.EntityId;
      MyMultiplayer.RaiseEvent<MyContractBlock, List<MyObjectBuilder_Contract>, long, long>(this, (Func<MyContractBlock, Action<List<MyObjectBuilder_Contract>, long, long>>) (x => new Action<List<MyObjectBuilder_Contract>, long, long>(x.ReceiveActiveContracts)), component.GetActiveContractsForPlayer_OB(identityId), num, entityId, MyEventContext.Current.Sender);
    }

    [Event(null, 753)]
    [Reliable]
    [Server(ValidationType.Access)]
    private static void GetActiveContractsStatic()
    {
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return;
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      if (identityId == 0L)
        return;
      MyMultiplayer.RaiseStaticEvent<List<MyObjectBuilder_Contract>>((Func<IMyEventOwner, Action<List<MyObjectBuilder_Contract>>>) (x => new Action<List<MyObjectBuilder_Contract>>(MyContractBlock.ReceiveActiveContractsStatic)), component.GetActiveContractsForPlayer_OB(identityId), MyEventContext.Current.Sender);
    }

    [Event(null, 770)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void GetAllOwnedContractBlocks(long identityId)
    {
      if (!this.HasAccess())
        return;
      List<MyContractBlock.MyEntityInfoWrapper> entityInfoWrapperList = new List<MyContractBlock.MyEntityInfoWrapper>();
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      if (identity != null)
      {
        foreach (KeyValuePair<long, MyBlockLimits.MyGridLimitData> keyValuePair in identity.BlockLimits.BlocksBuiltByGrid)
        {
          MyCubeGrid entity;
          if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(keyValuePair.Key, out entity) || entity.BigOwners.Contains(identityId))
          {
            foreach (MySlimBlock block in entity.GetBlocks())
            {
              if (block.FatBlock != null && block.FatBlock is MyContractBlock)
                entityInfoWrapperList.Add(new MyContractBlock.MyEntityInfoWrapper()
                {
                  NamePrefix = string.IsNullOrEmpty(entity.DisplayName) ? string.Empty : entity.DisplayName,
                  NameSuffix = string.IsNullOrEmpty(block.FatBlock.DisplayNameText) ? string.Empty : block.FatBlock.DisplayNameText,
                  Id = block.FatBlock.EntityId
                });
            }
          }
        }
      }
      MyMultiplayer.RaiseEvent<MyContractBlock, List<MyContractBlock.MyEntityInfoWrapper>>(this, (Func<MyContractBlock, Action<List<MyContractBlock.MyEntityInfoWrapper>>>) (x => new Action<List<MyContractBlock.MyEntityInfoWrapper>>(x.ReceiveAllOwnedContractBlocks)), entityInfoWrapperList, MyEventContext.Current.Sender);
    }

    [Event(null, 806)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void GetAllOwnedGrids(long identityId)
    {
      if (!this.HasAccess())
        return;
      List<MyContractBlock.MyEntityInfoWrapper> entityInfoWrapperList = new List<MyContractBlock.MyEntityInfoWrapper>();
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      if (identity != null)
      {
        foreach (KeyValuePair<long, MyBlockLimits.MyGridLimitData> keyValuePair in identity.BlockLimits.BlocksBuiltByGrid)
        {
          MyCubeGrid entity;
          if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(keyValuePair.Key, out entity) || entity.BigOwners.Contains(identityId))
            entityInfoWrapperList.Add(new MyContractBlock.MyEntityInfoWrapper()
            {
              NamePrefix = string.IsNullOrEmpty(entity.DisplayName) ? string.Empty : entity.DisplayName,
              NameSuffix = string.Empty,
              Id = entity.EntityId
            });
        }
      }
      MyMultiplayer.RaiseEvent<MyContractBlock, List<MyContractBlock.MyEntityInfoWrapper>>(this, (Func<MyContractBlock, Action<List<MyContractBlock.MyEntityInfoWrapper>>>) (x => new Action<List<MyContractBlock.MyEntityInfoWrapper>>(x.ReceiveAllOwnedGrids)), entityInfoWrapperList, MyEventContext.Current.Sender);
    }

    [Event(null, 838)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void AcceptContract(long identityId, long contractId)
    {
      if (!this.HasAccess())
      {
        MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveAcceptContract)), MyContractResults.Fail_CannotAccess, MyEventContext.Current.Sender);
      }
      else
      {
        MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
        if (component == null)
        {
          MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveAcceptContract)), MyContractResults.Error_MissingKeyStructure, MyEventContext.Current.Sender);
        }
        else
        {
          MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(this.CubeGrid.EntityId);
          MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveAcceptContract)), component.ActivateContract(identityId, contractId, stationByGridId != null ? stationByGridId.Id : 0L, this.EntityId), MyEventContext.Current.Sender);
        }
      }
    }

    [Event(null, 865)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void FinishContract(long identityId, long contractId, long targetEntityId)
    {
      if (!this.HasAccess())
      {
        MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveFinishContract)), MyContractResults.Fail_CannotAccess, MyEventContext.Current.Sender);
      }
      else
      {
        MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
        if (component == null)
          MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveFinishContract)), MyContractResults.Error_MissingKeyStructure, MyEventContext.Current.Sender);
        else if (!component.IsContractActive(contractId))
        {
          MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveFinishContract)), MyContractResults.Fail_ContractNotFound_Finish, MyEventContext.Current.Sender);
        }
        else
        {
          MyContract activeContractById = component.GetActiveContractById(contractId);
          if (activeContractById == null)
            MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveFinishContract)), MyContractResults.Fail_ContractNotFound_Finish, MyEventContext.Current.Sender);
          else if (!activeContractById.Owners.Contains(identityId))
          {
            MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveFinishContract)), MyContractResults.Error_InvalidData, MyEventContext.Current.Sender);
          }
          else
          {
            MyContractCondition contractCondition = activeContractById.ContractCondition;
            MyStation stationByGridId = MySession.Static.Factions.GetStationByGridId(this.CubeGrid.EntityId);
            if (contractCondition != null && !contractCondition.IsFinished && (stationByGridId != null && stationByGridId.Id == contractCondition.StationEndId || this.EntityId == contractCondition.BlockEndId))
            {
              if (!this.CheckConnectedEntityOwnership(MyEventContext.Current.Sender.Value, targetEntityId))
              {
                MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveFinishContract)), MyContractResults.Fail_CannotAccess, MyEventContext.Current.Sender);
              }
              else
              {
                MyContractResults myContractResults = component.FinishContractCondition(identityId, activeContractById, contractCondition, targetEntityId);
                if (myContractResults == MyContractResults.Success && activeContractById.CanBeFinished)
                {
                  int num = (int) component.FinishContract(identityId, activeContractById);
                }
                MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveFinishContract)), myContractResults, MyEventContext.Current.Sender);
              }
            }
            else if (activeContractById.CanBeFinished)
              MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveFinishContract)), component.FinishContract(identityId, activeContractById), MyEventContext.Current.Sender);
            else
              MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveFinishContract)), MyContractResults.Fail_FinishConditionsNotMet, MyEventContext.Current.Sender);
          }
        }
      }
    }

    [Event(null, 935)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void AbandonContract(long identityId, long contractId)
    {
      if (!this.HasAccess())
        return;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return;
      MyMultiplayer.RaiseEvent<MyContractBlock, MyContractResults>(this, (Func<MyContractBlock, Action<MyContractResults>>) (x => new Action<MyContractResults>(x.ReceiveAbandonContract)), component.AbandonContract(identityId, contractId), MyEventContext.Current.Sender);
    }

    [Event(null, 957)]
    [Reliable]
    [Server(ValidationType.Access)]
    private static void AbandonContractStatic(long contractId)
    {
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      if (identityId == 0L)
      {
        MyMultiplayer.RaiseStaticEvent<MyContractResults>((Func<IMyEventOwner, Action<MyContractResults>>) (x => new Action<MyContractResults>(MyContractBlock.ReceiveAbandonContractStatic)), MyContractResults.Fail_CannotAccess, MyEventContext.Current.Sender);
      }
      else
      {
        MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
        if (component == null)
          MyMultiplayer.RaiseStaticEvent<MyContractResults>((Func<IMyEventOwner, Action<MyContractResults>>) (x => new Action<MyContractResults>(MyContractBlock.ReceiveAbandonContractStatic)), MyContractResults.Error_MissingKeyStructure, MyEventContext.Current.Sender);
        else
          MyMultiplayer.RaiseStaticEvent<MyContractResults>((Func<IMyEventOwner, Action<MyContractResults>>) (x => new Action<MyContractResults>(MyContractBlock.ReceiveAbandonContractStatic)), component.AbandonContract(identityId, contractId), MyEventContext.Current.Sender);
      }
    }

    [Event(null, 979)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void GetConnectedEntities(long identityId)
    {
      List<MyContractBlock.MyTargetEntityInfoWrapper> entityInfoWrapperList1 = new List<MyContractBlock.MyTargetEntityInfoWrapper>();
      if (!this.HasAccess())
        return;
      MyPlayer.PlayerId result;
      MyContractBlock.MyTargetEntityInfoWrapper entityInfoWrapper1;
      if (MySession.Static.Players.TryGetPlayerId(identityId, out result))
      {
        MyCharacter character = MySession.Static.Players.GetPlayerById(result)?.Character;
        if (character != null)
        {
          List<MyContractBlock.MyTargetEntityInfoWrapper> entityInfoWrapperList2 = entityInfoWrapperList1;
          entityInfoWrapper1 = new MyContractBlock.MyTargetEntityInfoWrapper();
          entityInfoWrapper1.Id = character.EntityId;
          entityInfoWrapper1.Name = string.IsNullOrEmpty(character.Name) ? string.Empty : character.Name;
          entityInfoWrapper1.DisplayName = MyTexts.GetString(MySpaceTexts.Economy_CharacterSelection);
          MyContractBlock.MyTargetEntityInfoWrapper entityInfoWrapper2 = entityInfoWrapper1;
          entityInfoWrapperList2.Add(entityInfoWrapper2);
        }
      }
      foreach (MyCubeGrid connectedTradingShip in this.GetAllConnectedTradingShips())
      {
        if (connectedTradingShip.BigOwners.Contains(identityId))
        {
          List<MyContractBlock.MyTargetEntityInfoWrapper> entityInfoWrapperList2 = entityInfoWrapperList1;
          entityInfoWrapper1 = new MyContractBlock.MyTargetEntityInfoWrapper();
          entityInfoWrapper1.Id = connectedTradingShip.EntityId;
          entityInfoWrapper1.Name = string.IsNullOrEmpty(connectedTradingShip.Name) ? string.Empty : connectedTradingShip.Name;
          entityInfoWrapper1.DisplayName = string.IsNullOrEmpty(connectedTradingShip.DisplayName) ? string.Empty : connectedTradingShip.DisplayName;
          MyContractBlock.MyTargetEntityInfoWrapper entityInfoWrapper2 = entityInfoWrapper1;
          entityInfoWrapperList2.Add(entityInfoWrapper2);
        }
      }
      MyMultiplayer.RaiseEvent<MyContractBlock, bool, List<MyContractBlock.MyTargetEntityInfoWrapper>>(this, (Func<MyContractBlock, Action<bool, List<MyContractBlock.MyTargetEntityInfoWrapper>>>) (x => new Action<bool, List<MyContractBlock.MyTargetEntityInfoWrapper>>(x.ReceiveGetConnectedGrids)), true, entityInfoWrapperList1, MyEventContext.Current.Sender);
    }

    private bool CheckConnectedEntityOwnership(ulong steamId, long entityId)
    {
      switch (Sandbox.Game.Entities.MyEntities.GetEntityById(entityId))
      {
        case MyCharacter myCharacter:
          if (myCharacter.ControllerInfo != null && myCharacter.ControllerInfo.Controller != null && (myCharacter.ControllerInfo.Controller.Player != null && (long) myCharacter.ControllerInfo.Controller.Player.Id.SteamId == (long) steamId))
            return true;
          break;
        case MyCubeGrid myCubeGrid:
          long identityId = MySession.Static.Players.TryGetIdentityId(steamId, 0);
          if (identityId != 0L && myCubeGrid.BigOwners.Contains(identityId))
            return true;
          break;
      }
      return false;
    }

    [Event(null, 1075)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void CreateCustomContractDeliver(
      MyContractBlock.MyContractCreationDataWrapper_Deliver data)
    {
      if (!this.HasAccess())
      {
        MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Fail_NoAccess, MyEventContext.Current.Sender);
      }
      else
      {
        MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
        if (component == null)
        {
          MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Error_MissingKeyStructure, MyEventContext.Current.Sender);
        }
        else
        {
          int count = component.GetAvailableContractsForBlock_OB(this.EntityId).Count;
          if (component.GetContractCreationLimitPerPlayer() <= count)
          {
            MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Fail_CreationLimitHard, MyEventContext.Current.Sender);
          }
          else
          {
            long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
            if (identityId == 0L || this.OwnerId != identityId)
              MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Fail_NotAnOwnerOfBlock, MyEventContext.Current.Sender);
            else
              MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), component.GenerateCustomContract_Deliver(this, data.RewardMoney, data.StartingDeposit, data.DurationInMin, data.TargetBlockId, out long _, out long _), MyEventContext.Current.Sender);
          }
        }
      }
    }

    [Event(null, 1112)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void CreateCustomContractObtainAndDeliver(
      MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver data)
    {
      if (!this.HasAccess())
      {
        MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Fail_NoAccess, MyEventContext.Current.Sender);
      }
      else
      {
        MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
        if (component == null)
        {
          MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Error_MissingKeyStructure, MyEventContext.Current.Sender);
        }
        else
        {
          int count = component.GetAvailableContractsForBlock_OB(this.EntityId).Count;
          if (component.GetContractCreationLimitPerPlayer() <= count)
          {
            MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Fail_CreationLimitHard, MyEventContext.Current.Sender);
          }
          else
          {
            long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
            if (identityId == 0L || this.OwnerId != identityId)
              MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Fail_NotAnOwnerOfBlock, MyEventContext.Current.Sender);
            else
              MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), component.GenerateCustomContract_ObtainAndDeliver(this, data.RewardMoney, data.StartingDeposit, data.DurationInMin, data.TargetBlockId, (MyDefinitionId) data.ItemTypeId, data.ItemAmount, out long _, out long _), MyEventContext.Current.Sender);
          }
        }
      }
    }

    [Event(null, 1149)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void CreateCustomContractRepair(
      MyContractBlock.MyContractCreationDataWrapper_Repair data)
    {
      if (!this.HasAccess())
        return;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return;
      int count = component.GetAvailableContractsForBlock_OB(this.EntityId).Count;
      if (component.GetContractCreationLimitPerPlayer() <= count)
      {
        MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Fail_CreationLimitHard, MyEventContext.Current.Sender);
      }
      else
      {
        long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
        if (identityId == 0L || this.OwnerId != identityId)
          MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Fail_NotAnOwnerOfBlock, MyEventContext.Current.Sender);
        else
          MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), component.GenerateCustomContract_Repair(this, data.RewardMoney, data.StartingDeposit, data.DurationInMin, data.TargetGridId, out long _, out long _), MyEventContext.Current.Sender);
      }
    }

    [Event(null, 1187)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void CreateCustomContractFind(
      MyContractBlock.MyContractCreationDataWrapper_Find data)
    {
      if (!this.HasAccess())
        return;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return;
      int count = component.GetAvailableContractsForBlock_OB(this.EntityId).Count;
      if (component.GetContractCreationLimitPerPlayer() <= count)
      {
        MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Fail_CreationLimitHard, MyEventContext.Current.Sender);
      }
      else
      {
        long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
        if (identityId == 0L || this.OwnerId != identityId)
          MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Fail_NotAnOwnerOfBlock, MyEventContext.Current.Sender);
        else
          MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), component.GenerateCustomContract_Find(this, data.RewardMoney, data.StartingDeposit, data.DurationInMin, data.TargetGridId, data.SearchRadius, out long _, out long _), MyEventContext.Current.Sender);
      }
    }

    [Event(null, 1224)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void DeleteCustomContract(long contractId)
    {
      if (!this.HasAccess())
        return;
      MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
      if (component == null)
        return;
      long identityId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0);
      if (identityId == 0L || this.OwnerId != identityId)
      {
        MyMultiplayer.RaiseEvent<MyContractBlock, MyContractCreationResults>(this, (Func<MyContractBlock, Action<MyContractCreationResults>>) (x => new Action<MyContractCreationResults>(x.ReceiveCreateContractResult)), MyContractCreationResults.Fail_NotAnOwnerOfBlock, MyEventContext.Current.Sender);
      }
      else
      {
        component.DeleteCustomContract(this, contractId);
        MyMultiplayer.RaiseEvent<MyContractBlock, bool>(this, (Func<MyContractBlock, Action<bool>>) (x => new Action<bool>(x.ReceiveDeleteCustomContractResult)), true, MyEventContext.Current.Sender);
      }
    }

    [Event(null, 1253)]
    [Reliable]
    [Client]
    private void ReceiveContractBlockStatus(bool isNpc)
    {
      Action<bool> blockStatucCallback = this.m_localRequestContractBlockStatucCallback;
      if (blockStatucCallback != null)
        blockStatucCallback(isNpc);
      this.m_localRequestContractBlockStatucCallback = (Action<bool>) null;
    }

    [Event(null, 1260)]
    [Reliable]
    [Client]
    private void ReceiveAvailableContracts([DynamicItem(typeof (MyObjectBuilderDynamicSerializer), false)] List<MyObjectBuilder_Contract> availableContracts)
    {
      Action<List<MyObjectBuilder_Contract>> contractsCallback = this.m_localRequestAvailableContractsCallback;
      if (contractsCallback != null)
        contractsCallback(availableContracts);
      this.m_localRequestAvailableContractsCallback = (Action<List<MyObjectBuilder_Contract>>) null;
    }

    [Event(null, 1267)]
    [Reliable]
    [Client]
    private void ReceiveAdministrableContracts(
      [DynamicItem(typeof (MyObjectBuilderDynamicSerializer), false)] List<MyObjectBuilder_Contract> administrableContracts)
    {
      Action<List<MyObjectBuilder_Contract>> contractsCallback = this.m_localRequestAdministrableContractsCallback;
      if (contractsCallback != null)
        contractsCallback(administrableContracts);
      this.m_localRequestAdministrableContractsCallback = (Action<List<MyObjectBuilder_Contract>>) null;
    }

    [Event(null, 1274)]
    [Reliable]
    [Client]
    private void ReceiveActiveContracts(
      [DynamicItem(typeof (MyObjectBuilderDynamicSerializer), false)] List<MyObjectBuilder_Contract> activeContracts,
      long stationId,
      long blockId)
    {
      Action<List<MyObjectBuilder_Contract>, long, long> contractsCallback = this.m_localRequestActiveContractsCallback;
      if (contractsCallback != null)
        contractsCallback(activeContracts, stationId, blockId);
      this.m_localRequestActiveContractsCallback = (Action<List<MyObjectBuilder_Contract>, long, long>) null;
    }

    [Event(null, 1281)]
    [Reliable]
    [Client]
    private static void ReceiveActiveContractsStatic([DynamicItem(typeof (MyObjectBuilderDynamicSerializer), false)] List<MyObjectBuilder_Contract> activeContracts)
    {
      Action<List<MyObjectBuilder_Contract>> contractsCallback = MyContractBlock.m_localStaticRequestActiveContractsCallback;
      if (contractsCallback != null)
        contractsCallback(activeContracts);
      MyContractBlock.m_localStaticRequestActiveContractsCallback = (Action<List<MyObjectBuilder_Contract>>) null;
    }

    [Event(null, 1288)]
    [Reliable]
    [Client]
    private void ReceiveAllOwnedContractBlocks(List<MyContractBlock.MyEntityInfoWrapper> data)
    {
      Action<List<MyContractBlock.MyEntityInfoWrapper>> contractBlocksCallback = this.m_localRequestOwnedContractBlocksCallback;
      if (contractBlocksCallback != null)
        contractBlocksCallback(data);
      this.m_localRequestOwnedContractBlocksCallback = (Action<List<MyContractBlock.MyEntityInfoWrapper>>) null;
    }

    [Event(null, 1295)]
    [Reliable]
    [Client]
    private void ReceiveAllOwnedGrids(List<MyContractBlock.MyEntityInfoWrapper> data)
    {
      Action<List<MyContractBlock.MyEntityInfoWrapper>> ownedGridsCallback = this.m_localRequestOwnedGridsCallback;
      if (ownedGridsCallback != null)
        ownedGridsCallback(data);
      this.m_localRequestOwnedGridsCallback = (Action<List<MyContractBlock.MyEntityInfoWrapper>>) null;
    }

    [Event(null, 1302)]
    [Reliable]
    [Client]
    private void ReceiveActiveConditions(
      [DynamicItem(typeof (MyObjectBuilderDynamicSerializer), false)] List<MyObjectBuilder_ContractCondition> activeConditions)
    {
      Action<List<MyObjectBuilder_ContractCondition>> conditionsCallback = this.m_localRequestActiveConditionsCallback;
      if (conditionsCallback != null)
        conditionsCallback(activeConditions);
      this.m_localRequestActiveConditionsCallback = (Action<List<MyObjectBuilder_ContractCondition>>) null;
    }

    [Event(null, 1309)]
    [Reliable]
    [Client]
    private void ReceiveAcceptContract(MyContractResults result)
    {
      Action<MyContractResults> requestAcceptCallback = this.m_localRequestAcceptCallback;
      if (requestAcceptCallback != null)
        requestAcceptCallback(result);
      this.m_localRequestAcceptCallback = (Action<MyContractResults>) null;
    }

    [Event(null, 1316)]
    [Reliable]
    [Client]
    private void ReceiveFinishContract(MyContractResults result)
    {
      Action<MyContractResults> requestFinishCallback = this.m_localRequestFinishCallback;
      if (requestFinishCallback != null)
        requestFinishCallback(result);
      this.m_localRequestFinishCallback = (Action<MyContractResults>) null;
    }

    [Event(null, 1323)]
    [Reliable]
    [Client]
    private void ReceiveAbandonContract(MyContractResults result)
    {
      Action<MyContractResults> requestAbandonCallback = this.m_localRequestAbandonCallback;
      if (requestAbandonCallback != null)
        requestAbandonCallback(result);
      this.m_localRequestAbandonCallback = (Action<MyContractResults>) null;
    }

    [Event(null, 1330)]
    [Reliable]
    [Client]
    private static void ReceiveAbandonContractStatic(MyContractResults result)
    {
      Action<MyContractResults> requestAbandonCallback = MyContractBlock.m_localStaticRequestAbandonCallback;
      if (requestAbandonCallback != null)
        requestAbandonCallback(result);
      MyContractBlock.m_localStaticRequestAbandonCallback = (Action<MyContractResults>) null;
    }

    [Event(null, 1337)]
    [Reliable]
    [Client]
    private void ReceiveGetConnectedGrids(
      bool isSuccessful,
      List<MyContractBlock.MyTargetEntityInfoWrapper> connectedGrids)
    {
      Action<bool, List<MyContractBlock.MyTargetEntityInfoWrapper>, long> entitiesCallback = this.m_localRequestConnectedEntitiesCallback;
      if (entitiesCallback != null)
        entitiesCallback(isSuccessful, connectedGrids, this.m_localRequestConnectedentitiesContractId);
      this.m_localRequestConnectedEntitiesCallback = (Action<bool, List<MyContractBlock.MyTargetEntityInfoWrapper>, long>) null;
    }

    [Event(null, 1344)]
    [Reliable]
    [Client]
    private void ReceiveCreateContractResult(MyContractCreationResults result)
    {
      Action<MyContractCreationResults> contractCallback = this.m_localRequestCreateCustomContractCallback;
      if (contractCallback != null)
        contractCallback(result);
      this.m_localRequestCreateCustomContractCallback = (Action<MyContractCreationResults>) null;
    }

    [Event(null, 1351)]
    [Reliable]
    [Client]
    private void ReceiveDeleteCustomContractResult(bool success)
    {
      Action<bool> contractCallback = this.m_localRequestDeleteCustomContractCallback;
      if (contractCallback != null)
        contractCallback(success);
      this.m_localRequestDeleteCustomContractCallback = (Action<bool>) null;
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

    public PullInformation GetPushInformation() => new PullInformation()
    {
      Inventory = MyEntityExtensions.GetInventory(this),
      OwnerID = this.OwnerId,
      Constraint = new MyInventoryConstraint("Empty constraint")
    };

    public List<MyCubeGrid> GetAllConnectedTradingShips()
    {
      List<MyCubeGrid> myCubeGridList = new List<MyCubeGrid>();
      foreach (MyShipConnector fatBlock in this.CubeGrid.GetFatBlocks<MyShipConnector>())
      {
        if (fatBlock != null && fatBlock.Connected && (bool) fatBlock.TradingEnabled)
        {
          MyShipConnector other = fatBlock.Other;
          if (other != null)
            myCubeGridList.Add(other.CubeGrid);
        }
      }
      return myCubeGridList;
    }

    [Serializable]
    public struct MyTargetEntityInfoWrapper
    {
      public long Id;
      public string Name;
      public string DisplayName;

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyTargetEntityInfoWrapper\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyTargetEntityInfoWrapper, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyTargetEntityInfoWrapper owner,
          in long value)
        {
          owner.Id = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyTargetEntityInfoWrapper owner,
          out long value)
        {
          value = owner.Id;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyTargetEntityInfoWrapper\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyTargetEntityInfoWrapper, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyTargetEntityInfoWrapper owner,
          in string value)
        {
          owner.Name = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyTargetEntityInfoWrapper owner,
          out string value)
        {
          value = owner.Name;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyTargetEntityInfoWrapper\u003C\u003EDisplayName\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyTargetEntityInfoWrapper, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyTargetEntityInfoWrapper owner,
          in string value)
        {
          owner.DisplayName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyTargetEntityInfoWrapper owner,
          out string value)
        {
          value = owner.DisplayName;
        }
      }
    }

    [Serializable]
    public struct MyEntityInfoWrapper
    {
      public string NamePrefix;
      public string NameSuffix;
      public long Id;

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyEntityInfoWrapper\u003C\u003ENamePrefix\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyEntityInfoWrapper, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyContractBlock.MyEntityInfoWrapper owner, in string value) => owner.NamePrefix = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyContractBlock.MyEntityInfoWrapper owner, out string value) => value = owner.NamePrefix;
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyEntityInfoWrapper\u003C\u003ENameSuffix\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyEntityInfoWrapper, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyContractBlock.MyEntityInfoWrapper owner, in string value) => owner.NameSuffix = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyContractBlock.MyEntityInfoWrapper owner, out string value) => value = owner.NameSuffix;
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyEntityInfoWrapper\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyEntityInfoWrapper, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyContractBlock.MyEntityInfoWrapper owner, in long value) => owner.Id = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyContractBlock.MyEntityInfoWrapper owner, out long value) => value = owner.Id;
      }
    }

    [Serializable]
    private struct MyContractCreationDataWrapper_Deliver
    {
      public int RewardMoney;
      public int StartingDeposit;
      public int DurationInMin;
      public long TargetBlockId;

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Deliver\u003C\u003ERewardMoney\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Deliver, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Deliver owner,
          in int value)
        {
          owner.RewardMoney = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Deliver owner,
          out int value)
        {
          value = owner.RewardMoney;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Deliver\u003C\u003EStartingDeposit\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Deliver, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Deliver owner,
          in int value)
        {
          owner.StartingDeposit = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Deliver owner,
          out int value)
        {
          value = owner.StartingDeposit;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Deliver\u003C\u003EDurationInMin\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Deliver, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Deliver owner,
          in int value)
        {
          owner.DurationInMin = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Deliver owner,
          out int value)
        {
          value = owner.DurationInMin;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Deliver\u003C\u003ETargetBlockId\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Deliver, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Deliver owner,
          in long value)
        {
          owner.TargetBlockId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Deliver owner,
          out long value)
        {
          value = owner.TargetBlockId;
        }
      }
    }

    [Serializable]
    private struct MyContractCreationDataWrapper_ObtainAndDeliver
    {
      public int RewardMoney;
      public int StartingDeposit;
      public int DurationInMin;
      public long TargetBlockId;
      public SerializableDefinitionId ItemTypeId;
      public int ItemAmount;

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_ObtainAndDeliver\u003C\u003ERewardMoney\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver owner,
          in int value)
        {
          owner.RewardMoney = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver owner,
          out int value)
        {
          value = owner.RewardMoney;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_ObtainAndDeliver\u003C\u003EStartingDeposit\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver owner,
          in int value)
        {
          owner.StartingDeposit = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver owner,
          out int value)
        {
          value = owner.StartingDeposit;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_ObtainAndDeliver\u003C\u003EDurationInMin\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver owner,
          in int value)
        {
          owner.DurationInMin = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver owner,
          out int value)
        {
          value = owner.DurationInMin;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_ObtainAndDeliver\u003C\u003ETargetBlockId\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver owner,
          in long value)
        {
          owner.TargetBlockId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver owner,
          out long value)
        {
          value = owner.TargetBlockId;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_ObtainAndDeliver\u003C\u003EItemTypeId\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver owner,
          in SerializableDefinitionId value)
        {
          owner.ItemTypeId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver owner,
          out SerializableDefinitionId value)
        {
          value = owner.ItemTypeId;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_ObtainAndDeliver\u003C\u003EItemAmount\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver owner,
          in int value)
        {
          owner.ItemAmount = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver owner,
          out int value)
        {
          value = owner.ItemAmount;
        }
      }
    }

    [Serializable]
    private struct MyContractCreationDataWrapper_Find
    {
      public int RewardMoney;
      public int StartingDeposit;
      public int DurationInMin;
      public long TargetGridId;
      public double SearchRadius;

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Find\u003C\u003ERewardMoney\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Find, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Find owner,
          in int value)
        {
          owner.RewardMoney = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Find owner,
          out int value)
        {
          value = owner.RewardMoney;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Find\u003C\u003EStartingDeposit\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Find, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Find owner,
          in int value)
        {
          owner.StartingDeposit = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Find owner,
          out int value)
        {
          value = owner.StartingDeposit;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Find\u003C\u003EDurationInMin\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Find, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Find owner,
          in int value)
        {
          owner.DurationInMin = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Find owner,
          out int value)
        {
          value = owner.DurationInMin;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Find\u003C\u003ETargetGridId\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Find, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Find owner,
          in long value)
        {
          owner.TargetGridId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Find owner,
          out long value)
        {
          value = owner.TargetGridId;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Find\u003C\u003ESearchRadius\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Find, double>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Find owner,
          in double value)
        {
          owner.SearchRadius = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Find owner,
          out double value)
        {
          value = owner.SearchRadius;
        }
      }
    }

    [Serializable]
    private struct MyContractCreationDataWrapper_Repair
    {
      public int RewardMoney;
      public int StartingDeposit;
      public int DurationInMin;
      public long TargetGridId;

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Repair\u003C\u003ERewardMoney\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Repair, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Repair owner,
          in int value)
        {
          owner.RewardMoney = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Repair owner,
          out int value)
        {
          value = owner.RewardMoney;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Repair\u003C\u003EStartingDeposit\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Repair, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Repair owner,
          in int value)
        {
          owner.StartingDeposit = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Repair owner,
          out int value)
        {
          value = owner.StartingDeposit;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Repair\u003C\u003EDurationInMin\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Repair, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Repair owner,
          in int value)
        {
          owner.DurationInMin = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Repair owner,
          out int value)
        {
          value = owner.DurationInMin;
        }
      }

      protected class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Repair\u003C\u003ETargetGridId\u003C\u003EAccessor : IMemberAccessor<MyContractBlock.MyContractCreationDataWrapper_Repair, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyContractBlock.MyContractCreationDataWrapper_Repair owner,
          in long value)
        {
          owner.TargetGridId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyContractBlock.MyContractCreationDataWrapper_Repair owner,
          out long value)
        {
          value = owner.TargetGridId;
        }
      }
    }

    protected sealed class OnChangeTextRequest\u003C\u003ESystem_Int32\u0023System_String : ICallSite<MyContractBlock, int, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
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

    protected sealed class OnUpdateSpriteCollection\u003C\u003ESystem_Int32\u0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyContractBlock, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
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

    protected sealed class OnRemoveSelectedImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyContractBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
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

    protected sealed class OnSelectImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyContractBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
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

    protected sealed class OnChangeOpenSuccess\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyContractBlock, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
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

    protected sealed class OnChangeOpenRequest\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyContractBlock, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
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

    protected sealed class OnChangeDescription\u003C\u003ESystem_String\u0023System_Boolean : ICallSite<MyContractBlock, string, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
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

    protected sealed class GetContractBlockStatus\u003C\u003E : ICallSite<MyContractBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetContractBlockStatus();
      }
    }

    protected sealed class GetAvailibleContracts\u003C\u003E : ICallSite<MyContractBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetAvailibleContracts();
      }
    }

    protected sealed class GetAdministrableContracts\u003C\u003E : ICallSite<MyContractBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetAdministrableContracts();
      }
    }

    protected sealed class GetActiveContracts\u003C\u003ESystem_Int64 : ICallSite<MyContractBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in long identityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetActiveContracts(identityId);
      }
    }

    protected sealed class GetActiveContractsStatic\u003C\u003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyContractBlock.GetActiveContractsStatic();
      }
    }

    protected sealed class GetAllOwnedContractBlocks\u003C\u003ESystem_Int64 : ICallSite<MyContractBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in long identityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetAllOwnedContractBlocks(identityId);
      }
    }

    protected sealed class GetAllOwnedGrids\u003C\u003ESystem_Int64 : ICallSite<MyContractBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in long identityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetAllOwnedGrids(identityId);
      }
    }

    protected sealed class AcceptContract\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<MyContractBlock, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in long identityId,
        in long contractId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.AcceptContract(identityId, contractId);
      }
    }

    protected sealed class FinishContract\u003C\u003ESystem_Int64\u0023System_Int64\u0023System_Int64 : ICallSite<MyContractBlock, long, long, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in long identityId,
        in long contractId,
        in long targetEntityId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.FinishContract(identityId, contractId, targetEntityId);
      }
    }

    protected sealed class AbandonContract\u003C\u003ESystem_Int64\u0023System_Int64 : ICallSite<MyContractBlock, long, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in long identityId,
        in long contractId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.AbandonContract(identityId, contractId);
      }
    }

    protected sealed class AbandonContractStatic\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long contractId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyContractBlock.AbandonContractStatic(contractId);
      }
    }

    protected sealed class GetConnectedEntities\u003C\u003ESystem_Int64 : ICallSite<MyContractBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in long identityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.GetConnectedEntities(identityId);
      }
    }

    protected sealed class CreateCustomContractDeliver\u003C\u003ESandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Deliver : ICallSite<MyContractBlock, MyContractBlock.MyContractCreationDataWrapper_Deliver, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in MyContractBlock.MyContractCreationDataWrapper_Deliver data,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CreateCustomContractDeliver(data);
      }
    }

    protected sealed class CreateCustomContractObtainAndDeliver\u003C\u003ESandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_ObtainAndDeliver : ICallSite<MyContractBlock, MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in MyContractBlock.MyContractCreationDataWrapper_ObtainAndDeliver data,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CreateCustomContractObtainAndDeliver(data);
      }
    }

    protected sealed class CreateCustomContractRepair\u003C\u003ESandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Repair : ICallSite<MyContractBlock, MyContractBlock.MyContractCreationDataWrapper_Repair, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in MyContractBlock.MyContractCreationDataWrapper_Repair data,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CreateCustomContractRepair(data);
      }
    }

    protected sealed class CreateCustomContractFind\u003C\u003ESandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyContractCreationDataWrapper_Find : ICallSite<MyContractBlock, MyContractBlock.MyContractCreationDataWrapper_Find, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in MyContractBlock.MyContractCreationDataWrapper_Find data,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CreateCustomContractFind(data);
      }
    }

    protected sealed class DeleteCustomContract\u003C\u003ESystem_Int64 : ICallSite<MyContractBlock, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in long contractId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DeleteCustomContract(contractId);
      }
    }

    protected sealed class ReceiveContractBlockStatus\u003C\u003ESystem_Boolean : ICallSite<MyContractBlock, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in bool isNpc,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveContractBlockStatus(isNpc);
      }
    }

    protected sealed class ReceiveAvailableContracts\u003C\u003ESystem_Collections_Generic_List`1\u003CVRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003E : ICallSite<MyContractBlock, List<MyObjectBuilder_Contract>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in List<MyObjectBuilder_Contract> availableContracts,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveAvailableContracts(availableContracts);
      }
    }

    protected sealed class ReceiveAdministrableContracts\u003C\u003ESystem_Collections_Generic_List`1\u003CVRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003E : ICallSite<MyContractBlock, List<MyObjectBuilder_Contract>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in List<MyObjectBuilder_Contract> administrableContracts,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveAdministrableContracts(administrableContracts);
      }
    }

    protected sealed class ReceiveActiveContracts\u003C\u003ESystem_Collections_Generic_List`1\u003CVRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003E\u0023System_Int64\u0023System_Int64 : ICallSite<MyContractBlock, List<MyObjectBuilder_Contract>, long, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in List<MyObjectBuilder_Contract> activeContracts,
        in long stationId,
        in long blockId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveActiveContracts(activeContracts, stationId, blockId);
      }
    }

    protected sealed class ReceiveActiveContractsStatic\u003C\u003ESystem_Collections_Generic_List`1\u003CVRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_Contract\u003E : ICallSite<IMyEventOwner, List<MyObjectBuilder_Contract>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<MyObjectBuilder_Contract> activeContracts,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyContractBlock.ReceiveActiveContractsStatic(activeContracts);
      }
    }

    protected sealed class ReceiveAllOwnedContractBlocks\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyEntityInfoWrapper\u003E : ICallSite<MyContractBlock, List<MyContractBlock.MyEntityInfoWrapper>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in List<MyContractBlock.MyEntityInfoWrapper> data,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveAllOwnedContractBlocks(data);
      }
    }

    protected sealed class ReceiveAllOwnedGrids\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyEntityInfoWrapper\u003E : ICallSite<MyContractBlock, List<MyContractBlock.MyEntityInfoWrapper>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in List<MyContractBlock.MyEntityInfoWrapper> data,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveAllOwnedGrids(data);
      }
    }

    protected sealed class ReceiveActiveConditions\u003C\u003ESystem_Collections_Generic_List`1\u003CVRage_Game_ObjectBuilders_Components_Contracts_MyObjectBuilder_ContractCondition\u003E : ICallSite<MyContractBlock, List<MyObjectBuilder_ContractCondition>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in List<MyObjectBuilder_ContractCondition> activeConditions,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveActiveConditions(activeConditions);
      }
    }

    protected sealed class ReceiveAcceptContract\u003C\u003ESandbox_Game_Entities_Blocks_MyContractResults : ICallSite<MyContractBlock, MyContractResults, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in MyContractResults result,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveAcceptContract(result);
      }
    }

    protected sealed class ReceiveFinishContract\u003C\u003ESandbox_Game_Entities_Blocks_MyContractResults : ICallSite<MyContractBlock, MyContractResults, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in MyContractResults result,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveFinishContract(result);
      }
    }

    protected sealed class ReceiveAbandonContract\u003C\u003ESandbox_Game_Entities_Blocks_MyContractResults : ICallSite<MyContractBlock, MyContractResults, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in MyContractResults result,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveAbandonContract(result);
      }
    }

    protected sealed class ReceiveAbandonContractStatic\u003C\u003ESandbox_Game_Entities_Blocks_MyContractResults : ICallSite<IMyEventOwner, MyContractResults, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyContractResults result,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyContractBlock.ReceiveAbandonContractStatic(result);
      }
    }

    protected sealed class ReceiveGetConnectedGrids\u003C\u003ESystem_Boolean\u0023System_Collections_Generic_List`1\u003CSandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EMyTargetEntityInfoWrapper\u003E : ICallSite<MyContractBlock, bool, List<MyContractBlock.MyTargetEntityInfoWrapper>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in bool isSuccessful,
        in List<MyContractBlock.MyTargetEntityInfoWrapper> connectedGrids,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveGetConnectedGrids(isSuccessful, connectedGrids);
      }
    }

    protected sealed class ReceiveCreateContractResult\u003C\u003ESandbox_Game_World_Generator_MyContractCreationResults : ICallSite<MyContractBlock, MyContractCreationResults, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in MyContractCreationResults result,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveCreateContractResult(result);
      }
    }

    protected sealed class ReceiveDeleteCustomContractResult\u003C\u003ESystem_Boolean : ICallSite<MyContractBlock, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyContractBlock @this,
        in bool success,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ReceiveDeleteCustomContractResult(success);
      }
    }

    protected class m_anyoneCanUse\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyContractBlock) obj0).m_anyoneCanUse = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyContractBlock\u003C\u003EActor : IActivator, IActivator<MyContractBlock>
    {
      object IActivator.CreateInstance() => (object) new MyContractBlock();

      MyContractBlock IActivator<MyContractBlock>.CreateInstance() => new MyContractBlock();
    }
  }
}
