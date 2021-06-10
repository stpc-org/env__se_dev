// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyConveyorSorter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_ConveyorSorter))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyConveyorSorter), typeof (Sandbox.ModAPI.Ingame.IMyConveyorSorter)})]
  public class MyConveyorSorter : MyFunctionalBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyConveyorSorter, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyConveyorSorter, IMyInventoryOwner
  {
    private readonly int MAX_ITEMS_TO_PULL_IN_ONE_TICK = 10;
    private MyStringHash m_prevColor = MyStringHash.NullOrEmpty;
    private readonly MyInventoryConstraint m_inventoryConstraint = new MyInventoryConstraint(string.Empty);
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_drainAll;
    private MyConveyorSorterDefinition m_conveyorSorterDefinition;
    private int m_pushRequestFrameCounter;
    private int m_pullRequestFrameCounter;
    private static readonly StringBuilder m_helperSB = new StringBuilder();
    private static MyTerminalControlOnOffSwitch<MyConveyorSorter> drainAll;
    private static MyTerminalControlCombobox<MyConveyorSorter> blacklistWhitelist;
    private static MyTerminalControlListbox<MyConveyorSorter> currentList;
    private static MyTerminalControlButton<MyConveyorSorter> removeFromSelectionButton;
    private static MyTerminalControlListbox<MyConveyorSorter> candidates;
    private static MyTerminalControlButton<MyConveyorSorter> addToSelectionButton;
    private static readonly Dictionary<byte, Tuple<MyObjectBuilderType, StringBuilder>> CandidateTypes = new Dictionary<byte, Tuple<MyObjectBuilderType, StringBuilder>>();
    private static readonly Dictionary<MyObjectBuilderType, byte> CandidateTypesToId = new Dictionary<MyObjectBuilderType, byte>();
    private bool m_allowCurrentListUpdate = true;
    private List<MyGuiControlListbox.Item> m_selectedForDelete;
    private List<MyGuiControlListbox.Item> m_selectedForAdd;

    public bool IsWhitelist
    {
      get => this.m_inventoryConstraint.IsWhitelist;
      private set
      {
        if (this.m_inventoryConstraint.IsWhitelist == value)
          return;
        this.m_inventoryConstraint.IsWhitelist = value;
        this.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
      }
    }

    public bool IsAllowed(MyDefinitionId itemId) => this.Enabled && this.IsFunctional && (this.IsWorking && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId)) && this.m_inventoryConstraint.Check(itemId);

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public bool DrainAll
    {
      get => (bool) this.m_drainAll;
      set => this.m_drainAll.Value = value;
    }

    public MyConveyorSorter()
    {
      this.CreateTerminalControls();
      this.m_drainAll.ValueChanged += (Action<SyncBase>) (x => this.DoChangeDrainAll());
    }

    public MyConveyorSorterDefinition BlockDefinition => (MyConveyorSorterDefinition) base.BlockDefinition;

    static MyConveyorSorter()
    {
      byte num1 = 0;
      byte num2;
      MyConveyorSorter.CandidateTypes.Add(num2 = (byte) ((uint) num1 + 1U), new Tuple<MyObjectBuilderType, StringBuilder>((MyObjectBuilderType) typeof (MyObjectBuilder_AmmoMagazine), MyTexts.Get(MySpaceTexts.DisplayName_ConvSorterTypes_Ammo)));
      byte num3;
      MyConveyorSorter.CandidateTypes.Add(num3 = (byte) ((uint) num2 + 1U), new Tuple<MyObjectBuilderType, StringBuilder>((MyObjectBuilderType) typeof (MyObjectBuilder_Component), MyTexts.Get(MySpaceTexts.DisplayName_ConvSorterTypes_Component)));
      byte num4;
      MyConveyorSorter.CandidateTypes.Add(num4 = (byte) ((uint) num3 + 1U), new Tuple<MyObjectBuilderType, StringBuilder>((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), MyTexts.Get(MySpaceTexts.DisplayName_ConvSorterTypes_HandTool)));
      byte num5;
      MyConveyorSorter.CandidateTypes.Add(num5 = (byte) ((uint) num4 + 1U), new Tuple<MyObjectBuilderType, StringBuilder>((MyObjectBuilderType) typeof (MyObjectBuilder_Ingot), MyTexts.Get(MySpaceTexts.DisplayName_ConvSorterTypes_Ingot)));
      byte num6;
      MyConveyorSorter.CandidateTypes.Add(num6 = (byte) ((uint) num5 + 1U), new Tuple<MyObjectBuilderType, StringBuilder>((MyObjectBuilderType) typeof (MyObjectBuilder_Ore), MyTexts.Get(MySpaceTexts.DisplayName_ConvSorterTypes_Ore)));
      foreach (KeyValuePair<byte, Tuple<MyObjectBuilderType, StringBuilder>> candidateType in MyConveyorSorter.CandidateTypes)
        MyConveyorSorter.CandidateTypesToId.Add(candidateType.Value.Item1, candidateType.Key);
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyConveyorSorter>())
        return;
      base.CreateTerminalControls();
      MyConveyorSorter.drainAll = new MyTerminalControlOnOffSwitch<MyConveyorSorter>("DrainAll", MySpaceTexts.Terminal_DrainAll);
      MyConveyorSorter.drainAll.Getter = (MyTerminalValueControl<MyConveyorSorter, bool>.GetterDelegate) (block => block.DrainAll);
      MyConveyorSorter.drainAll.Setter = (MyTerminalValueControl<MyConveyorSorter, bool>.SetterDelegate) ((block, val) => block.DrainAll = val);
      MyConveyorSorter.drainAll.EnableToggleAction<MyConveyorSorter>();
      MyTerminalControlFactory.AddControl<MyConveyorSorter>((MyTerminalControl<MyConveyorSorter>) MyConveyorSorter.drainAll);
      MyTerminalControlFactory.AddControl<MyConveyorSorter>((MyTerminalControl<MyConveyorSorter>) new MyTerminalControlSeparator<MyConveyorSorter>());
      MyConveyorSorter.blacklistWhitelist = new MyTerminalControlCombobox<MyConveyorSorter>("blacklistWhitelist", MySpaceTexts.BlockPropertyTitle_ConveyorSorterFilterMode, MySpaceTexts.Blank);
      MyConveyorSorter.blacklistWhitelist.ComboBoxContent = (Action<List<MyTerminalControlComboBoxItem>>) (block => MyConveyorSorter.FillBlWlCombo(block));
      MyConveyorSorter.blacklistWhitelist.Getter = (MyTerminalValueControl<MyConveyorSorter, long>.GetterDelegate) (block => block.IsWhitelist ? 1L : 0L);
      MyConveyorSorter.blacklistWhitelist.Setter = (MyTerminalValueControl<MyConveyorSorter, long>.SetterDelegate) ((block, val) => block.ChangeBlWl(val == 1L));
      MyConveyorSorter.blacklistWhitelist.SetSerializerBit();
      MyConveyorSorter.blacklistWhitelist.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyConveyorSorter>((MyTerminalControl<MyConveyorSorter>) MyConveyorSorter.blacklistWhitelist);
      MyConveyorSorter.currentList = new MyTerminalControlListbox<MyConveyorSorter>("CurrentList", MySpaceTexts.BlockPropertyTitle_ConveyorSorterFilterItemsList, MySpaceTexts.Blank, true);
      MyConveyorSorter.currentList.ListContent = (MyTerminalControlListbox<MyConveyorSorter>.ListContentDelegate) ((block, list1, list2, focusedItem) => block.FillCurrentList(list1, list2));
      MyConveyorSorter.currentList.ItemSelected = (MyTerminalControlListbox<MyConveyorSorter>.SelectItemDelegate) ((block, val) => block.SelectFromCurrentList(val));
      MyConveyorSorter.currentList.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyConveyorSorter>((MyTerminalControl<MyConveyorSorter>) MyConveyorSorter.currentList);
      MyConveyorSorter.removeFromSelectionButton = new MyTerminalControlButton<MyConveyorSorter>("removeFromSelectionButton", MySpaceTexts.BlockPropertyTitle_ConveyorSorterRemove, MySpaceTexts.Blank, (Action<MyConveyorSorter>) (block => block.RemoveFromCurrentList()));
      MyConveyorSorter.removeFromSelectionButton.Enabled = (Func<MyConveyorSorter, bool>) (x => x.m_selectedForDelete != null && x.m_selectedForDelete.Count > 0);
      MyConveyorSorter.removeFromSelectionButton.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyConveyorSorter>((MyTerminalControl<MyConveyorSorter>) MyConveyorSorter.removeFromSelectionButton);
      MyConveyorSorter.candidates = new MyTerminalControlListbox<MyConveyorSorter>("candidatesList", MySpaceTexts.BlockPropertyTitle_ConveyorSorterCandidatesList, MySpaceTexts.Blank, true);
      MyConveyorSorter.candidates.ListContent = (MyTerminalControlListbox<MyConveyorSorter>.ListContentDelegate) ((block, list1, list2, focusedItem) => block.FillCandidatesList(list1, list2));
      MyConveyorSorter.candidates.ItemSelected = (MyTerminalControlListbox<MyConveyorSorter>.SelectItemDelegate) ((block, val) => block.SelectCandidate(val));
      MyConveyorSorter.candidates.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyConveyorSorter>((MyTerminalControl<MyConveyorSorter>) MyConveyorSorter.candidates);
      MyConveyorSorter.addToSelectionButton = new MyTerminalControlButton<MyConveyorSorter>("addToSelectionButton", MySpaceTexts.BlockPropertyTitle_ConveyorSorterAdd, MySpaceTexts.Blank, (Action<MyConveyorSorter>) (x => x.AddToCurrentList()));
      MyConveyorSorter.addToSelectionButton.SupportsMultipleBlocks = false;
      MyConveyorSorter.addToSelectionButton.Enabled = (Func<MyConveyorSorter, bool>) (x => x.m_selectedForAdd != null && x.m_selectedForAdd.Count > 0);
      MyTerminalControlFactory.AddControl<MyConveyorSorter>((MyTerminalControl<MyConveyorSorter>) MyConveyorSorter.addToSelectionButton);
    }

    private static void FillBlWlCombo(List<MyTerminalControlComboBoxItem> list)
    {
      list.Add(new MyTerminalControlComboBoxItem()
      {
        Key = 0L,
        Value = MySpaceTexts.BlockPropertyTitle_ConveyorSorterFilterModeBlacklist
      });
      list.Add(new MyTerminalControlComboBoxItem()
      {
        Key = 1L,
        Value = MySpaceTexts.BlockPropertyTitle_ConveyorSorterFilterModeWhitelist
      });
    }

    private void FillCurrentList(
      ICollection<MyGuiControlListbox.Item> content,
      ICollection<MyGuiControlListbox.Item> selectedItems)
    {
      foreach (MyObjectBuilderType constrainedType in this.m_inventoryConstraint.ConstrainedTypes)
      {
        byte key;
        Tuple<MyObjectBuilderType, StringBuilder> tuple;
        if (MyConveyorSorter.CandidateTypesToId.TryGetValue(constrainedType, out key) && MyConveyorSorter.CandidateTypes.TryGetValue(key, out tuple))
        {
          MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(tuple.Item2, userData: ((object) key));
          content.Add(obj);
        }
      }
      foreach (MyDefinitionId constrainedId in this.m_inventoryConstraint.ConstrainedIds)
      {
        MyPhysicalItemDefinition definition;
        if (MyDefinitionManager.Static.TryGetPhysicalItemDefinition(constrainedId, out definition))
          MyConveyorSorter.m_helperSB.Clear().Append(definition.DisplayNameText);
        else
          MyConveyorSorter.m_helperSB.Clear().Append(constrainedId.ToString());
        MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(MyConveyorSorter.m_helperSB, userData: ((object) constrainedId));
        content.Add(obj);
      }
    }

    private void SelectFromCurrentList(List<MyGuiControlListbox.Item> val)
    {
      this.m_selectedForDelete = val;
      MyConveyorSorter.removeFromSelectionButton.UpdateVisual();
    }

    private void ModifyCurrentList(ref List<MyGuiControlListbox.Item> list, bool Add)
    {
      this.m_allowCurrentListUpdate = false;
      if (list != null)
      {
        foreach (MyGuiControlListbox.Item obj in list)
        {
          MyDefinitionId? userData1 = obj.UserData as MyDefinitionId?;
          if (userData1.HasValue)
          {
            this.ChangeListId((SerializableDefinitionId) userData1.Value, Add);
          }
          else
          {
            byte? userData2 = obj.UserData as byte?;
            if (userData2.HasValue)
              this.ChangeListType(userData2.Value, Add);
          }
        }
      }
      this.m_allowCurrentListUpdate = true;
      MyConveyorSorter.currentList.UpdateVisual();
      MyConveyorSorter.addToSelectionButton.UpdateVisual();
      MyConveyorSorter.removeFromSelectionButton.UpdateVisual();
    }

    private void RemoveFromCurrentList() => this.ModifyCurrentList(ref this.m_selectedForDelete, false);

    private void FillCandidatesList(
      ICollection<MyGuiControlListbox.Item> content,
      ICollection<MyGuiControlListbox.Item> selectedItems)
    {
      foreach (KeyValuePair<byte, Tuple<MyObjectBuilderType, StringBuilder>> candidateType in MyConveyorSorter.CandidateTypes)
      {
        MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(candidateType.Value.Item2, userData: ((object) candidateType.Key));
        content.Add(obj);
      }
      foreach (MyDefinitionBase myDefinitionBase in (IEnumerable<MyDefinitionBase>) MyDefinitionManager.Static.GetAllDefinitions().OrderBy<MyDefinitionBase, string>((Func<MyDefinitionBase, string>) (x => this.sorter(x))))
      {
        if (myDefinitionBase.Public && myDefinitionBase is MyPhysicalItemDefinition physicalItemDefinition && (myDefinitionBase.Public && physicalItemDefinition.CanSpawnFromScreen))
        {
          MyConveyorSorter.m_helperSB.Clear().Append(myDefinitionBase.DisplayNameText);
          MyGuiControlListbox.Item obj = new MyGuiControlListbox.Item(MyConveyorSorter.m_helperSB, userData: ((object) physicalItemDefinition.Id));
          content.Add(obj);
        }
      }
    }

    private string sorter(MyDefinitionBase def) => def is MyPhysicalItemDefinition physicalItemDefinition ? physicalItemDefinition.DisplayNameText : (string) null;

    private void SelectCandidate(List<MyGuiControlListbox.Item> val)
    {
      this.m_selectedForAdd = val;
      MyConveyorSorter.addToSelectionButton.UpdateVisual();
    }

    private void AddToCurrentList() => this.ModifyCurrentList(ref this.m_selectedForAdd, true);

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0.0f, detailedInfo);
      detailedInfo.Append("\n");
    }

    internal void DoChangeDrainAll()
    {
      this.DrainAll = (bool) this.m_drainAll;
      MyConveyorSorter.drainAll.UpdateVisual();
    }

    public void ChangeBlWl(bool IsWl) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyConveyorSorter, bool>(this, (Func<MyConveyorSorter, Action<bool>>) (x => new Action<bool>(x.DoChangeBlWl)), IsWl);

    [Event(null, 345)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void DoChangeBlWl(bool IsWl)
    {
      this.IsWhitelist = IsWl;
      MyConveyorSorter.blacklistWhitelist.UpdateVisual();
    }

    private void ChangeListId(SerializableDefinitionId id, bool wasAdded) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyConveyorSorter, SerializableDefinitionId, bool>(this, (Func<MyConveyorSorter, Action<SerializableDefinitionId, bool>>) (x => new Action<SerializableDefinitionId, bool>(x.DoChangeListId)), id, wasAdded);

    [Event(null, 357)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void DoChangeListId(SerializableDefinitionId id, bool add)
    {
      if (add)
        this.m_inventoryConstraint.Add((MyDefinitionId) id);
      else
        this.m_inventoryConstraint.Remove((MyDefinitionId) id);
      this.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
      if (!this.m_allowCurrentListUpdate)
        return;
      MyConveyorSorter.currentList.UpdateVisual();
    }

    private void ChangeListType(byte type, bool wasAdded) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyConveyorSorter, byte, bool>(this, (Func<MyConveyorSorter, Action<byte, bool>>) (x => new Action<byte, bool>(x.DoChangeListType)), type, wasAdded);

    [Event(null, 377)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void DoChangeListType(byte type, bool add)
    {
      Tuple<MyObjectBuilderType, StringBuilder> tuple;
      if (!MyConveyorSorter.CandidateTypes.TryGetValue(type, out tuple))
        return;
      if (add)
        this.m_inventoryConstraint.AddObjectBuilderType(tuple.Item1);
      else
        this.m_inventoryConstraint.RemoveObjectBuilderType(tuple.Item1);
      this.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
      if (!this.m_allowCurrentListUpdate)
        return;
      MyConveyorSorter.currentList.UpdateVisual();
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.m_conveyorSorterDefinition = (MyConveyorSorterDefinition) MyDefinitionManager.Static.GetCubeBlockDefinition(objectBuilder.GetId());
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.m_conveyorSorterDefinition.ResourceSinkGroup, this.BlockDefinition.PowerInput, new Func<float>(this.UpdatePowerInput));
      resourceSinkComponent.IsPoweredChanged += new Action(this.IsPoweredChanged);
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_ConveyorSorter builderConveyorSorter = (MyObjectBuilder_ConveyorSorter) objectBuilder;
      this.m_drainAll.SetLocalValue(builderConveyorSorter.DrainAll);
      this.IsWhitelist = builderConveyorSorter.IsWhiteList;
      foreach (SerializableDefinitionId definitionId in builderConveyorSorter.DefinitionIds)
        this.m_inventoryConstraint.Add((MyDefinitionId) definitionId);
      foreach (byte definitionType in builderConveyorSorter.DefinitionTypes)
      {
        Tuple<MyObjectBuilderType, StringBuilder> tuple;
        if (MyConveyorSorter.CandidateTypes.TryGetValue(definitionType, out tuple))
          this.m_inventoryConstraint.AddObjectBuilderType(tuple.Item1);
      }
      if (MyFakes.ENABLE_INVENTORY_FIX)
        this.FixSingleInventory();
      if (MyEntityExtensions.GetInventory(this) == null)
      {
        MyInventory myInventory = new MyInventory(this.m_conveyorSorterDefinition.InventorySize.Volume, this.m_conveyorSorterDefinition.InventorySize, MyInventoryFlags.CanSend);
        this.Components.Add<MyInventoryBase>((MyInventoryBase) myInventory);
        myInventory.Init(builderConveyorSorter.Inventory);
      }
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.ResourceSink.Update();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_ConveyorSorter builderCubeBlock = (MyObjectBuilder_ConveyorSorter) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.DrainAll = this.DrainAll;
      builderCubeBlock.IsWhiteList = this.IsWhitelist;
      foreach (MyDefinitionId constrainedId in this.m_inventoryConstraint.ConstrainedIds)
        builderCubeBlock.DefinitionIds.Add((SerializableDefinitionId) constrainedId);
      foreach (MyObjectBuilderType constrainedType in this.m_inventoryConstraint.ConstrainedTypes)
      {
        byte num;
        if (MyConveyorSorter.CandidateTypesToId.TryGetValue(constrainedType, out num))
          builderCubeBlock.DefinitionTypes.Add(num);
      }
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.UpdateEmissivity();
    }

    private float UpdatePowerInput() => !this.Enabled || !this.IsFunctional ? 0.0f : this.BlockDefinition.PowerInput;

    protected override void OnEnabledChanged()
    {
      this.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
      this.ResourceSink.Update();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      base.OnEnabledChanged();
    }

    private void UpdateEmissivity()
    {
      if (this.IsFunctional)
      {
        bool flag = this.CheckIsWorking() && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
        if (this.m_prevColor != MyCubeBlock.m_emissiveNames.Working & flag)
        {
          this.SetEmissiveStateWorking();
          this.m_prevColor = MyCubeBlock.m_emissiveNames.Working;
        }
        else
        {
          if (!(this.m_prevColor != MyCubeBlock.m_emissiveNames.Disabled) || flag)
            return;
          this.SetEmissiveStateDisabled();
          this.m_prevColor = MyCubeBlock.m_emissiveNames.Disabled;
        }
      }
      else
      {
        if (!(this.m_prevColor != MyCubeBlock.m_emissiveNames.Damaged))
          return;
        this.SetEmissiveStateDamaged();
        this.m_prevColor = MyCubeBlock.m_emissiveNames.Damaged;
      }
    }

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    private void IsPoweredChanged()
    {
      this.ResourceSink.Update();
      this.UpdateIsWorking();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      this.UpdateEmissivity();
    }

    protected override void OnInventoryComponentAdded(MyInventoryBase inventory) => base.OnInventoryComponentAdded(inventory);

    protected override void OnInventoryComponentRemoved(MyInventoryBase inventory) => base.OnInventoryComponentRemoved(inventory);

    private bool UseConveyorSystem
    {
      get => true;
      set
      {
      }
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.DrainAll || (!this.Enabled || !this.IsFunctional) || !this.IsWorking)
        return;
      ++this.m_pushRequestFrameCounter;
      if (this.m_pushRequestFrameCounter >= 4)
      {
        this.m_pushRequestFrameCounter = 0;
        MyInventory inventory = MyEntityExtensions.GetInventory(this);
        if (inventory.GetItemsCount() > 0)
          MyGridConveyorSystem.PushAnyRequest((IMyConveyorEndpointBlock) this, inventory);
      }
      ++this.m_pullRequestFrameCounter;
      if (this.m_pullRequestFrameCounter < 10)
        return;
      this.m_pullRequestFrameCounter = 0;
      MyInventory inventory1 = MyEntityExtensions.GetInventory(this);
      double volumeFillFactor = (double) inventory1.VolumeFillFactor;
      if ((double) inventory1.VolumeFillFactor >= 0.990000009536743 || !MyGridConveyorSystem.PullAllRequestForSorter((IMyConveyorEndpointBlock) this, inventory1, this.m_inventoryConstraint, this.MAX_ITEMS_TO_PULL_IN_ONE_TICK))
        return;
      this.m_pullRequestFrameCounter = 10;
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

    public void InitializeConveyorEndpoint()
    {
      this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      this.ResourceSink.Update();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      this.UpdateEmissivity();
    }

    [Event(null, 633)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void DoSetupFilter(MyConveyorSorterMode mode, List<MyInventoryItemFilter> items)
    {
      this.IsWhitelist = mode == MyConveyorSorterMode.Whitelist;
      this.m_inventoryConstraint.Clear();
      if (items != null)
      {
        this.m_allowCurrentListUpdate = false;
        try
        {
          foreach (MyInventoryItemFilter inventoryItemFilter in items)
          {
            if (inventoryItemFilter.AllSubTypes)
              this.m_inventoryConstraint.AddObjectBuilderType(inventoryItemFilter.ItemId.TypeId);
            else
              this.m_inventoryConstraint.Add(inventoryItemFilter.ItemId);
          }
        }
        finally
        {
          this.m_allowCurrentListUpdate = true;
        }
      }
      this.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
      MyConveyorSorter.currentList.UpdateVisual();
    }

    int IMyInventoryOwner.InventoryCount => this.InventoryCount;

    long IMyInventoryOwner.EntityId => this.EntityId;

    bool IMyInventoryOwner.HasInventory => this.HasInventory;

    bool IMyInventoryOwner.UseConveyorSystem
    {
      get => this.UseConveyorSystem;
      set => throw new NotImplementedException();
    }

    VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(
      int index)
    {
      return (VRage.Game.ModAPI.Ingame.IMyInventory) MyEntityExtensions.GetInventory(this, index);
    }

    public PullInformation GetPullInformation() => new PullInformation()
    {
      Inventory = MyEntityExtensions.GetInventory(this),
      OwnerID = this.OwnerId,
      Constraint = this.m_inventoryConstraint
    };

    public PullInformation GetPushInformation() => new PullInformation()
    {
      Inventory = MyEntityExtensions.GetInventory(this),
      OwnerID = this.OwnerId,
      Constraint = new MyInventoryConstraint("Empty constraint")
    };

    public bool AllowSelfPulling() => false;

    MyConveyorSorterMode Sandbox.ModAPI.Ingame.IMyConveyorSorter.Mode => !this.m_inventoryConstraint.IsWhitelist ? MyConveyorSorterMode.Blacklist : MyConveyorSorterMode.Whitelist;

    void Sandbox.ModAPI.Ingame.IMyConveyorSorter.GetFilterList(
      List<MyInventoryItemFilter> items)
    {
      items.Clear();
      foreach (MyObjectBuilderType constrainedType in this.m_inventoryConstraint.ConstrainedTypes)
        items.Add(new MyInventoryItemFilter(new MyDefinitionId(constrainedType), true));
      foreach (MyDefinitionId constrainedId in this.m_inventoryConstraint.ConstrainedIds)
        items.Add(new MyInventoryItemFilter(constrainedId));
    }

    void Sandbox.ModAPI.Ingame.IMyConveyorSorter.SetFilter(
      MyConveyorSorterMode mode,
      List<MyInventoryItemFilter> items)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyConveyorSorter, MyConveyorSorterMode, List<MyInventoryItemFilter>>(this, (Func<MyConveyorSorter, Action<MyConveyorSorterMode, List<MyInventoryItemFilter>>>) (x => new Action<MyConveyorSorterMode, List<MyInventoryItemFilter>>(x.DoSetupFilter)), mode, items);
    }

    void Sandbox.ModAPI.Ingame.IMyConveyorSorter.AddItem(MyInventoryItemFilter item)
    {
      if (item.AllSubTypes)
      {
        byte type;
        if (!MyConveyorSorter.CandidateTypesToId.TryGetValue(item.ItemId.TypeId, out type))
          return;
        this.ChangeListType(type, true);
      }
      else
        this.ChangeListId((SerializableDefinitionId) item.ItemId, true);
    }

    void Sandbox.ModAPI.Ingame.IMyConveyorSorter.RemoveItem(
      MyInventoryItemFilter item)
    {
      if (item.AllSubTypes)
      {
        byte type;
        if (!MyConveyorSorter.CandidateTypesToId.TryGetValue(item.ItemId.TypeId, out type))
          return;
        this.ChangeListType(type, false);
      }
      else
        this.ChangeListId((SerializableDefinitionId) item.ItemId, false);
    }

    protected sealed class DoChangeBlWl\u003C\u003ESystem_Boolean : ICallSite<MyConveyorSorter, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyConveyorSorter @this,
        in bool IsWl,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DoChangeBlWl(IsWl);
      }
    }

    protected sealed class DoChangeListId\u003C\u003EVRage_ObjectBuilders_SerializableDefinitionId\u0023System_Boolean : ICallSite<MyConveyorSorter, SerializableDefinitionId, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyConveyorSorter @this,
        in SerializableDefinitionId id,
        in bool add,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DoChangeListId(id, add);
      }
    }

    protected sealed class DoChangeListType\u003C\u003ESystem_Byte\u0023System_Boolean : ICallSite<MyConveyorSorter, byte, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyConveyorSorter @this,
        in byte type,
        in bool add,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DoChangeListType(type, add);
      }
    }

    protected sealed class DoSetupFilter\u003C\u003ESandbox_ModAPI_Ingame_MyConveyorSorterMode\u0023System_Collections_Generic_List`1\u003CSandbox_ModAPI_Ingame_MyInventoryItemFilter\u003E : ICallSite<MyConveyorSorter, MyConveyorSorterMode, List<MyInventoryItemFilter>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyConveyorSorter @this,
        in MyConveyorSorterMode mode,
        in List<MyInventoryItemFilter> items,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DoSetupFilter(mode, items);
      }
    }

    protected class m_drainAll\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyConveyorSorter) obj0).m_drainAll = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyConveyorSorter\u003C\u003EActor : IActivator, IActivator<MyConveyorSorter>
    {
      object IActivator.CreateInstance() => (object) new MyConveyorSorter();

      MyConveyorSorter IActivator<MyConveyorSorter>.CreateInstance() => new MyConveyorSorter();
    }
  }
}
