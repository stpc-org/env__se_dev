// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyAssembler
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Assembler))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyAssembler), typeof (Sandbox.ModAPI.Ingame.IMyAssembler)})]
  public class MyAssembler : MyProductionBlock, Sandbox.ModAPI.IMyAssembler, Sandbox.ModAPI.IMyProductionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProductionBlock, Sandbox.ModAPI.Ingame.IMyAssembler, IMyEventProxy, IMyEventOwner
  {
    private static readonly List<IMyConveyorEndpoint> m_conveyorEndpoints = new List<IMyConveyorEndpoint>();
    private static MyAssembler m_assemblerForPathfinding;
    private static readonly Predicate<IMyConveyorEndpoint> m_vertexPredicate = new Predicate<IMyConveyorEndpoint>(MyAssembler.VertexRules);
    private static readonly Predicate<IMyConveyorEndpoint> m_edgePredicate = new Predicate<IMyConveyorEndpoint>(MyAssembler.EdgeRules);
    private MyEntity m_currentUser;
    private MyAssemblerDefinition m_assemblerDef;
    private readonly VRage.Sync.Sync<float, SyncDirection.FromServer> m_currentProgress;
    private readonly VRage.Sync.Sync<int, SyncDirection.FromServer> m_currentItemIndex;
    private readonly VRage.Sync.Sync<MyAssembler.StateEnum, SyncDirection.FromServer> m_currentState;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_slave;
    private bool m_repeatDisassembleEnabled;
    private bool m_repeatAssembleEnabled;
    private bool m_disassembleEnabled;
    private readonly List<MyEntity> m_inventoryOwners = new List<MyEntity>();
    private List<MyTuple<MyFixedPoint, MyBlueprintDefinitionBase.Item>> m_requiredComponents;
    private const float TIME_IN_ADVANCE = 5f;
    private bool m_isProcessing;
    private bool m_soundStartedFromInventory;
    private List<MyProductionBlock.QueueItem> m_otherQueue;
    private List<MyAssembler> m_assemblers = new List<MyAssembler>();
    private int m_assemblerKeyCounter;
    private MyCubeGrid m_cubeGrid;
    private bool m_inventoryOwnersDirty = true;
    private MyTimeSpan m_lastAnimationUpdate;

    public bool InventoryOwnersDirty
    {
      get => this.m_inventoryOwnersDirty;
      set => this.m_inventoryOwnersDirty = value;
    }

    public bool IsSlave
    {
      get => (bool) this.m_slave;
      set
      {
        if (!this.SupportsAdvancedFunctions & value)
          return;
        this.m_slave.Value = value;
      }
    }

    public float CurrentProgress
    {
      get => (float) this.m_currentProgress;
      set => this.m_currentProgress.Value = value;
    }

    public int CurrentItemIndex
    {
      get => (int) this.m_currentItemIndex;
      set => this.m_currentItemIndex.Value = value;
    }

    public MyAssembler.StateEnum CurrentState
    {
      get => this.m_currentState.Value;
      private set => this.m_currentState.Value = value;
    }

    public int CurrentItemIndexServer => !this.m_currentQueueItem.HasValue ? -1 : this.m_queue.FindIndex((Predicate<MyProductionBlock.QueueItem>) (x => (int) x.ItemId == (int) this.m_currentQueueItem.Value.ItemId));

    public override bool IsTieredUpdateSupported => true;

    public bool RepeatEnabled
    {
      get => !this.m_disassembleEnabled ? this.m_repeatAssembleEnabled : this.m_repeatDisassembleEnabled;
      private set
      {
        if (!this.SupportsAdvancedFunctions & value)
          return;
        if (this.m_disassembleEnabled)
          this.SetRepeat(ref this.m_repeatDisassembleEnabled, value);
        else
          this.SetRepeat(ref this.m_repeatAssembleEnabled, value);
      }
    }

    public virtual bool SupportsAdvancedFunctions => true;

    public bool DisassembleEnabled
    {
      get => this.m_disassembleEnabled;
      private set
      {
        if (this.m_disassembleEnabled == value || !this.SupportsAdvancedFunctions & value)
          return;
        this.m_disassembleEnabled = value;
        this.SwapQueue(ref this.m_otherQueue);
        this.RebuildQueueInRepeatDisassembling();
        this.UpdateInventoryFlags();
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
        {
          this.CurrentState = MyAssembler.StateEnum.Ok;
          this.CurrentProgress = 0.0f;
        }
        if (this.CurrentModeChanged != null)
          this.CurrentModeChanged(this);
        if (this.CurrentStateChanged == null)
          return;
        this.CurrentStateChanged(this);
      }
    }

    public event Action<MyAssembler> CurrentProgressChanged;

    public event Action<MyAssembler> CurrentStateChanged;

    public event Action<MyAssembler> CurrentModeChanged;

    public MyAssembler()
    {
      this.CreateTerminalControls();
      this.m_otherQueue = new List<MyProductionBlock.QueueItem>();
      this.m_slave.ValueChanged += (Action<SyncBase>) (x => this.OnSlaveChanged());
      this.m_currentProgress.ValueChanged += new Action<SyncBase>(this.OnProgressValueChanged);
      this.m_currentState.ValueChanged += new Action<SyncBase>(this.OnCurrentStateChanged);
    }

    private void OnCurrentStateChanged(SyncBase obj) => this.CurrentStateChanged.InvokeIfNotNull<MyAssembler>(this);

    private void OnProgressValueChanged(SyncBase obj)
    {
      this.CurrentProgressChanged.InvokeIfNotNull<MyAssembler>(this);
      if (Sandbox.Game.Multiplayer.Sync.IsDedicated)
        return;
      this.m_lastAnimationUpdate = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyAssembler>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlCheckbox<MyAssembler> checkbox = new MyTerminalControlCheckbox<MyAssembler>("slaveMode", MySpaceTexts.Assembler_SlaveMode, MySpaceTexts.Assembler_SlaveMode);
      checkbox.Enabled = (Func<MyAssembler, bool>) (x => x.SupportsAdvancedFunctions);
      checkbox.Visible = checkbox.Enabled;
      checkbox.Getter = (MyTerminalValueControl<MyAssembler, bool>.GetterDelegate) (x => x.IsSlave);
      checkbox.Setter = (MyTerminalValueControl<MyAssembler, bool>.SetterDelegate) ((x, v) =>
      {
        if (x.RepeatEnabled)
          x.RequestRepeatEnabled(false);
        x.IsSlave = v;
      });
      checkbox.EnableAction<MyAssembler>();
      MyTerminalControlFactory.AddControl<MyAssembler>((MyTerminalControl<MyAssembler>) checkbox);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.UpgradeValues.Add("Productivity", 0.0f);
      this.UpgradeValues.Add("PowerEfficiency", 1f);
      base.Init(objectBuilder, cubeGrid);
      this.m_cubeGrid = cubeGrid;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_assemblerDef = this.BlockDefinition as MyAssemblerDefinition;
      if (this.InventoryAggregate.InventoryCount > 2)
        this.FixInputOutputInventories(this.m_assemblerDef.InputInventoryConstraint, this.m_assemblerDef.OutputInventoryConstraint);
      this.InputInventory.Constraint = this.m_assemblerDef.InputInventoryConstraint;
      this.OutputInventory.Constraint = this.m_assemblerDef.OutputInventoryConstraint;
      this.InputInventory.FilterItemsUsingConstraint();
      MyObjectBuilder_Assembler builderAssembler = (MyObjectBuilder_Assembler) objectBuilder;
      if (builderAssembler.OtherQueue != null)
      {
        this.m_otherQueue.Clear();
        if (this.m_otherQueue.Capacity < builderAssembler.OtherQueue.Length)
          this.m_otherQueue.Capacity = builderAssembler.OtherQueue.Length;
        for (int index = 0; index < builderAssembler.OtherQueue.Length; ++index)
        {
          MyObjectBuilder_ProductionBlock.QueueItem other = builderAssembler.OtherQueue[index];
          MyBlueprintDefinitionBase definitionByResultId = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId((MyDefinitionId) other.Id);
          if (definitionByResultId != null)
            this.m_otherQueue.Add(new MyProductionBlock.QueueItem()
            {
              Blueprint = definitionByResultId,
              Amount = other.Amount
            });
          else
            MySandboxGame.Log.WriteLine(string.Format("No blueprint that produces a single result with Id '{0}'", (object) other.Id));
        }
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.CurrentProgress = builderAssembler.CurrentProgress;
      this.m_disassembleEnabled = builderAssembler.DisassembleEnabled;
      this.m_repeatAssembleEnabled = builderAssembler.RepeatAssembleEnabled;
      this.m_repeatDisassembleEnabled = builderAssembler.RepeatDisassembleEnabled;
      this.m_slave.SetLocalValue(builderAssembler.SlaveEnabled);
      this.UpdateInventoryFlags();
      this.m_baseIdleSound = this.BlockDefinition.PrimarySound;
      this.m_processSound = this.BlockDefinition.ActionSound;
      this.OnUpgradeValuesChanged += (Action) (() =>
      {
        this.SetDetailedInfoDirty();
        this.RaisePropertiesChanged();
      });
      this.ResourceSink.RequiredInputChanged += new MyRequiredResourceChangeDelegate(this.PowerReceiver_RequiredInputChanged);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_Assembler builderCubeBlock = (MyObjectBuilder_Assembler) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.CurrentProgress = this.CurrentProgress;
      builderCubeBlock.DisassembleEnabled = this.m_disassembleEnabled;
      builderCubeBlock.RepeatAssembleEnabled = this.m_repeatAssembleEnabled;
      builderCubeBlock.RepeatDisassembleEnabled = this.m_repeatDisassembleEnabled;
      builderCubeBlock.SlaveEnabled = (bool) this.m_slave;
      if (this.m_otherQueue.Count > 0)
      {
        builderCubeBlock.OtherQueue = new MyObjectBuilder_ProductionBlock.QueueItem[this.m_otherQueue.Count];
        for (int index = 0; index < this.m_otherQueue.Count; ++index)
          builderCubeBlock.OtherQueue[index] = new MyObjectBuilder_ProductionBlock.QueueItem()
          {
            Amount = this.m_otherQueue[index].Amount,
            Id = (SerializableDefinitionId) this.m_otherQueue[index].Blueprint.Id
          };
      }
      else
        builderCubeBlock.OtherQueue = (MyObjectBuilder_ProductionBlock.QueueItem[]) null;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.AppendFormat("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.GetOperationalPowerConsumption(), detailedInfo);
      detailedInfo.AppendFormat("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_RequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
      detailedInfo.AppendFormat("\n\n");
      detailedInfo.Append(MyTexts.Get(MySpaceTexts.BlockPropertiesText_Productivity).ToString() + " ");
      detailedInfo.Append(((float) (((double) this.UpgradeValues["Productivity"] + 1.0) * 100.0)).ToString("F0"));
      detailedInfo.Append("%\n");
      detailedInfo.Append(MyTexts.Get(MySpaceTexts.BlockPropertiesText_Efficiency).ToString() + " ");
      detailedInfo.Append((this.UpgradeValues["PowerEfficiency"] * 100f).ToString("F0"));
      detailedInfo.Append("%\n\n");
      this.PrintUpgradeModuleInfo(detailedInfo);
    }

    private void PowerReceiver_RequiredInputChanged(
      MyDefinitionId resourceTypeId,
      MyResourceSinkComponent receiver,
      float oldRequirement,
      float newRequirement)
    {
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private static bool VertexRules(IMyConveyorEndpoint vertex) => vertex.CubeBlock is MyAssembler && vertex.CubeBlock != MyAssembler.m_assemblerForPathfinding;

    private static bool EdgeRules(IMyConveyorEndpoint edge) => edge.CubeBlock.OwnerId == 0L || MyAssembler.m_assemblerForPathfinding.FriendlyWithBlock(edge.CubeBlock);

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      if (!this.m_inventoryOwnersDirty)
        return;
      this.GetCoveyorInventoryOwners();
    }

    public override void DoUpdateTimerTick()
    {
      base.DoUpdateTimerTick();
      if (this.IsWorking && this.Enabled && (bool) this.m_useConveyorSystem)
      {
        if (this.DisassembleEnabled)
          this.UpdateDisassembleMode();
        else
          this.UpdateAssembleMode();
      }
      this.UpdateProduction(this.GetFramesFromLastTrigger());
    }

    private void UpdateAssembleMode()
    {
      this.GetComponentsFromConveyor();
      if ((double) this.OutputInventory.VolumeFillFactor <= 0.75)
        return;
      MyGridConveyorSystem.PushAnyRequest((IMyConveyorEndpointBlock) this, this.OutputInventory);
    }

    private void GetComponentsFromConveyor()
    {
      if ((double) this.InputInventory.VolumeFillFactor >= 0.990000009536743)
        return;
      bool flag1 = false;
      int idx = 0;
      float num1 = 0.0f;
      do
      {
        using (MyUtils.ReuseCollection<MyTuple<MyFixedPoint, MyBlueprintDefinitionBase.Item>>(ref this.m_requiredComponents))
        {
          float num2 = num1;
          MyProductionBlock.QueueItem? queueItem = this.TryGetQueueItem(idx);
          float num3 = 5f - num1;
          if (queueItem.HasValue)
          {
            float num4 = MySession.Static.AssemblerSpeedMultiplier * (((MyAssemblerDefinition) this.BlockDefinition).AssemblySpeed + this.UpgradeValues["Productivity"]);
            int num5 = 1;
            if ((double) queueItem.Value.Blueprint.BaseProductionTimeInSeconds / (double) num4 < (double) num3)
              num5 = Math.Min((int) queueItem.Value.Amount, Convert.ToInt32(Math.Ceiling((double) num3 / ((double) queueItem.Value.Blueprint.BaseProductionTimeInSeconds / (double) num4))));
            num1 += (float) num5 * queueItem.Value.Blueprint.BaseProductionTimeInSeconds / num4;
            if ((double) num1 < 5.0)
              flag1 = true;
            MyFixedPoint myFixedPoint1 = (MyFixedPoint) (1f / this.GetEfficiencyMultiplierForBlueprint(queueItem.Value.Blueprint));
            foreach (MyBlueprintDefinitionBase.Item prerequisite in queueItem.Value.Blueprint.Prerequisites)
            {
              MyFixedPoint myFixedPoint2 = prerequisite.Amount * myFixedPoint1;
              MyFixedPoint myFixedPoint3 = myFixedPoint2 * num5;
              bool flag2 = false;
              for (int index = 0; index < this.m_requiredComponents.Count; ++index)
              {
                MyBlueprintDefinitionBase.Item obj = this.m_requiredComponents[index].Item2;
                if (obj.Id == prerequisite.Id)
                {
                  obj.Amount += myFixedPoint3;
                  MyFixedPoint myFixedPoint4 = this.m_requiredComponents[index].Item1 + myFixedPoint2;
                  this.m_requiredComponents[index] = MyTuple.Create<MyFixedPoint, MyBlueprintDefinitionBase.Item>(myFixedPoint4, obj);
                  flag2 = true;
                  break;
                }
              }
              if (!flag2)
                this.m_requiredComponents.Add(MyTuple.Create<MyFixedPoint, MyBlueprintDefinitionBase.Item>(myFixedPoint2, new MyBlueprintDefinitionBase.Item()
                {
                  Amount = myFixedPoint3,
                  Id = prerequisite.Id
                }));
            }
          }
          foreach (MyTuple<MyFixedPoint, MyBlueprintDefinitionBase.Item> requiredComponent in this.m_requiredComponents)
          {
            MyBlueprintDefinitionBase.Item obj = requiredComponent.Item2;
            MyFixedPoint itemAmount = this.InputInventory.GetItemAmount(obj.Id, MyItemFlags.None, false);
            MyFixedPoint myFixedPoint = obj.Amount - itemAmount;
            if (!(myFixedPoint <= (MyFixedPoint) 0) && this.CubeGrid.GridSystems.ConveyorSystem.PullItem(obj.Id, new MyFixedPoint?(myFixedPoint), (IMyConveyorEndpointBlock) this, this.InputInventory, false, false) == (MyFixedPoint) 0 && requiredComponent.Item1 > itemAmount)
            {
              flag1 = true;
              num1 = num2;
            }
          }
          ++idx;
          if (idx >= this.m_queue.Count)
            flag1 = false;
        }
      }
      while (flag1);
      if (!this.IsSlave || this.RepeatEnabled)
        return;
      float remainingTime = 5f - num1;
      if ((double) remainingTime <= 0.0)
        return;
      this.GetItemFromOtherAssemblers(remainingTime);
    }

    private void GetItemFromOtherAssemblers(float remainingTime)
    {
      float num1 = MySession.Static.AssemblerSpeedMultiplier * (((MyAssemblerDefinition) this.BlockDefinition).AssemblySpeed + this.UpgradeValues["Productivity"]);
      MyAssembler masterAssembler = this.GetMasterAssembler();
      if (masterAssembler == null)
        return;
      if (masterAssembler.m_repeatAssembleEnabled)
      {
        if (this.m_queue.Count != 0)
          return;
        foreach (MyProductionBlock.QueueItem queueItem in masterAssembler.m_queue)
        {
          if (this.CanUseBlueprint(queueItem.Blueprint))
          {
            remainingTime -= (float) (queueItem.Blueprint.BaseProductionTimeInSeconds / num1 * queueItem.Amount);
            this.InsertQueueItemRequest(this.m_queue.Count, queueItem.Blueprint, queueItem.Amount);
            if ((double) remainingTime < 0.0)
              break;
          }
        }
      }
      else
      {
        if (masterAssembler.m_queue.Count <= 0)
          return;
        MyProductionBlock.QueueItem? queueItem = masterAssembler.TryGetQueueItem(0);
        if (!queueItem.HasValue || !(queueItem.Value.Amount > (MyFixedPoint) 1) || !this.CanUseBlueprint(queueItem.Value.Blueprint))
          return;
        int num2 = Math.Min((int) queueItem.Value.Amount - 1, Convert.ToInt32(Math.Ceiling((double) remainingTime / ((double) queueItem.Value.Blueprint.BaseProductionTimeInSeconds / (double) num1))));
        if (num2 <= 0)
          return;
        masterAssembler.RemoveFirstQueueItemAnnounce((MyFixedPoint) num2, masterAssembler.CurrentProgress);
        this.InsertQueueItemRequest(this.m_queue.Count, queueItem.Value.Blueprint, (MyFixedPoint) num2);
      }
    }

    private MyAssembler GetMasterAssembler()
    {
      MyAssembler.m_conveyorEndpoints.Clear();
      MyAssembler.m_assemblerForPathfinding = this;
      MyGridConveyorSystem.FindReachable(this.ConveyorEndpoint, MyAssembler.m_conveyorEndpoints, MyAssembler.m_vertexPredicate, MyAssembler.m_edgePredicate);
      MyAssembler.m_conveyorEndpoints.ShuffleList<IMyConveyorEndpoint>();
      foreach (IMyConveyorEndpoint conveyorEndpoint in MyAssembler.m_conveyorEndpoints)
      {
        if (conveyorEndpoint.CubeBlock is MyAssembler cubeBlock && !cubeBlock.DisassembleEnabled && (!cubeBlock.IsSlave && cubeBlock.m_queue.Count > 0))
          return cubeBlock;
      }
      return (MyAssembler) null;
    }

    private void UpdateDisassembleMode()
    {
      if ((double) this.OutputInventory.VolumeFillFactor < 0.990000009536743)
      {
        MyProductionBlock.QueueItem? firstQueueItem = this.TryGetFirstQueueItem();
        if (firstQueueItem.HasValue && !this.OutputInventory.ContainItems(new MyFixedPoint?(firstQueueItem.Value.Amount), firstQueueItem.Value.Blueprint.Results[0].Id))
          this.CubeGrid.GridSystems.ConveyorSystem.PullItem(firstQueueItem.Value.Blueprint.Results[0].Id, new MyFixedPoint?(firstQueueItem.Value.Amount), (IMyConveyorEndpointBlock) this, this.OutputInventory, false, false);
      }
      if ((double) this.InputInventory.VolumeFillFactor <= 0.75)
        return;
      MyGridConveyorSystem.PushAnyRequest((IMyConveyorEndpointBlock) this, this.InputInventory);
    }

    public void UpdateCurrentState(MyBlueprintDefinitionBase blueprint = null)
    {
      if (!this.Enabled)
      {
        this.CurrentState = MyAssembler.StateEnum.Disabled;
        this.IsProducing = false;
      }
      else if ((!this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) || (double) this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) < (double) this.GetOperationalPowerConsumption()) && !this.ResourceSink.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, this.GetOperationalPowerConsumption()))
      {
        this.CurrentState = MyAssembler.StateEnum.NotEnoughPower;
        this.IsProducing = false;
      }
      else if (!this.IsWorking)
      {
        this.CurrentState = MyAssembler.StateEnum.NotWorking;
        this.IsProducing = false;
      }
      else if (blueprint == null && this.IsQueueEmpty)
      {
        this.CurrentState = MyAssembler.StateEnum.Ok;
        this.IsProducing = false;
      }
      else
      {
        if (blueprint == null)
        {
          ref MyProductionBlock.QueueItem? local = ref this.m_currentQueueItem;
          if ((local.HasValue ? local.GetValueOrDefault().Blueprint : (MyBlueprintDefinitionBase) null) != null)
            blueprint = this.m_currentQueueItem.Value.Blueprint;
        }
        if (blueprint == null)
          return;
        this.CurrentState = this.CheckInventory(blueprint);
      }
    }

    public MyAssembler.StateEnum GetCurrentState(MyBlueprintDefinitionBase blueprint = null)
    {
      if (!this.Enabled)
        return MyAssembler.StateEnum.Disabled;
      if ((!this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) || (double) this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) < (double) this.GetOperationalPowerConsumption()) && !this.ResourceSink.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, this.GetOperationalPowerConsumption()))
        return MyAssembler.StateEnum.NotEnoughPower;
      if (!this.IsWorking)
        return MyAssembler.StateEnum.NotWorking;
      if (blueprint == null && this.IsQueueEmpty)
        return MyAssembler.StateEnum.Ok;
      if (blueprint == null)
      {
        ref MyProductionBlock.QueueItem? local = ref this.m_currentQueueItem;
        if ((local.HasValue ? local.GetValueOrDefault().Blueprint : (MyBlueprintDefinitionBase) null) != null)
          blueprint = this.m_currentQueueItem.Value.Blueprint;
      }
      return blueprint != null ? this.CheckInventory(blueprint) : this.CurrentState;
    }

    private void UpdateProduction(uint framesFromLastTrigger, bool forceUpdate = false)
    {
      this.UpdateCurrentState();
      int idx = 0;
      float num1;
      if (this.m_realProductionStart.HasValue)
      {
        num1 = (float) Math.Round((double) (framesFromLastTrigger - this.m_realProductionStart.Value) * 16.6666660308838);
        this.m_realProductionStart = new uint?();
      }
      else
        num1 = (float) Math.Round((double) framesFromLastTrigger * 16.6666660308838);
      bool flag = false;
      while (((double) num1 > 0.0 || forceUpdate && !flag) && idx < this.m_queue.Count)
      {
        flag = true;
        if (this.IsQueueEmpty)
        {
          this.CurrentProgress = 0.0f;
          this.IsProducing = false;
          return;
        }
        if (!this.m_currentQueueItem.HasValue)
          this.m_currentQueueItem = this.TryGetQueueItem(idx);
        MyBlueprintDefinitionBase blueprint = this.m_currentQueueItem.Value.Blueprint;
        this.CurrentState = this.CheckInventory(blueprint);
        if (this.CurrentState != MyAssembler.StateEnum.Ok)
        {
          ++idx;
          this.m_currentQueueItem = new MyProductionBlock.QueueItem?();
        }
        else
        {
          float blueprintProductionTime = this.CalculateBlueprintProductionTime(blueprint);
          float num2 = (float) Math.Round((1.0 - (double) this.CurrentProgress) * (double) blueprintProductionTime);
          if ((double) num1 >= (double) num2)
          {
            if (Sandbox.Game.Multiplayer.Sync.IsServer)
            {
              if (this.DisassembleEnabled)
              {
                this.FinishDisassembling(blueprint);
              }
              else
              {
                if (this.RepeatEnabled)
                  this.InsertQueueItemRequest(-1, blueprint);
                this.FinishAssembling(blueprint);
              }
              this.m_currentItemIndex.Value = this.CurrentItemIndexServer;
              this.RemoveQueueItemRequest(this.m_queue.IndexOf(this.m_currentQueueItem.Value), (MyFixedPoint) 1);
              this.m_currentQueueItem = new MyProductionBlock.QueueItem?();
            }
            num1 -= num2;
            this.CurrentProgress = 0.0f;
            this.m_currentQueueItem = new MyProductionBlock.QueueItem?();
          }
          else
          {
            this.CurrentProgress += num1 / blueprintProductionTime;
            num1 = 0.0f;
          }
        }
      }
      if (this.CurrentState != MyAssembler.StateEnum.Ok || this.CurrentItemIndexServer != -1)
        this.m_currentItemIndex.Value = this.CurrentItemIndexServer;
      this.IsProducing = this.IsWorking && !this.IsQueueEmpty && this.CurrentState == MyAssembler.StateEnum.Ok;
    }

    private float CalculateBlueprintProductionTime(MyBlueprintDefinitionBase currentBlueprint) => (float) Math.Round((double) currentBlueprint.BaseProductionTimeInSeconds * 1000.0 / ((double) MySession.Static.AssemblerSpeedMultiplier * (double) ((MyAssemblerDefinition) this.BlockDefinition).AssemblySpeed + (double) this.UpgradeValues["Productivity"]));

    private void FinishAssembling(MyBlueprintDefinitionBase blueprint)
    {
      MyFixedPoint myFixedPoint = (MyFixedPoint) (1f / this.GetEfficiencyMultiplierForBlueprint(blueprint));
      for (int index = 0; index < blueprint.Prerequisites.Length; ++index)
      {
        MyBlueprintDefinitionBase.Item prerequisite = blueprint.Prerequisites[index];
        this.InputInventory.RemoveItemsOfType(prerequisite.Amount * myFixedPoint, prerequisite.Id, MyItemFlags.None, false);
      }
      foreach (MyBlueprintDefinitionBase.Item result in blueprint.Results)
      {
        MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject(result.Id.TypeId, result.Id.SubtypeName);
        this.OutputInventory.AddItems(result.Amount, (MyObjectBuilder_Base) newObject);
        if (MyVisualScriptLogicProvider.NewItemBuilt != null)
          MyVisualScriptLogicProvider.NewItemBuilt(this.EntityId, this.CubeGrid.EntityId, this.Name, this.CubeGrid.Name, newObject.TypeId.ToString(), newObject.SubtypeName, result.Amount.ToIntSafe());
      }
    }

    private void FinishDisassembling(MyBlueprintDefinitionBase blueprint)
    {
      if (this.RepeatEnabled && Sandbox.Game.Multiplayer.Sync.IsServer)
        this.OutputInventory.ContentsChanged -= new Action<MyInventoryBase>(this.OutputInventory_ContentsChanged);
      foreach (MyBlueprintDefinitionBase.Item result in blueprint.Results)
        this.OutputInventory.RemoveItemsOfType(result.Amount, result.Id, MyItemFlags.None, false);
      if (this.RepeatEnabled && Sandbox.Game.Multiplayer.Sync.IsServer)
        this.OutputInventory.ContentsChanged += new Action<MyInventoryBase>(this.OutputInventory_ContentsChanged);
      MyFixedPoint myFixedPoint = (MyFixedPoint) (1f / this.GetEfficiencyMultiplierForBlueprint(blueprint));
      for (int index = 0; index < blueprint.Prerequisites.Length; ++index)
      {
        MyBlueprintDefinitionBase.Item prerequisite = blueprint.Prerequisites[index];
        MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject(prerequisite.Id.TypeId, prerequisite.Id.SubtypeName);
        this.InputInventory.AddItems(prerequisite.Amount * myFixedPoint, (MyObjectBuilder_Base) newObject);
      }
    }

    private MyAssembler.StateEnum CheckInventory(MyBlueprintDefinitionBase blueprint)
    {
      MyFixedPoint amountMultiplier = (MyFixedPoint) (1f / this.GetEfficiencyMultiplierForBlueprint(blueprint));
      if (this.DisassembleEnabled)
      {
        if (!this.CheckInventoryCapacity(this.InputInventory, blueprint.Prerequisites, amountMultiplier))
          return MyAssembler.StateEnum.InventoryFull;
        if (!this.CheckInventoryContents(this.OutputInventory, blueprint.Results, (MyFixedPoint) 1))
          return MyAssembler.StateEnum.MissingItems;
      }
      else
      {
        if (!this.CheckInventoryCapacity(this.OutputInventory, blueprint.Results, (MyFixedPoint) 1))
          return MyAssembler.StateEnum.InventoryFull;
        if (!this.CheckInventoryContents(this.InputInventory, blueprint.Prerequisites, amountMultiplier))
          return MyAssembler.StateEnum.MissingItems;
      }
      return MyAssembler.StateEnum.Ok;
    }

    private bool CheckInventoryCapacity(
      MyInventory inventory,
      MyBlueprintDefinitionBase.Item item,
      MyFixedPoint amountMultiplier)
    {
      return inventory.CanItemsBeAdded(item.Amount * amountMultiplier, item.Id);
    }

    private bool CheckInventoryCapacity(
      MyInventory inventory,
      MyBlueprintDefinitionBase.Item[] items,
      MyFixedPoint amountMultiplier)
    {
      MyFixedPoint myFixedPoint = (MyFixedPoint) 0;
      foreach (MyBlueprintDefinitionBase.Item obj in items)
      {
        MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(obj.Id);
        if (physicalItemDefinition != null)
          myFixedPoint += (MyFixedPoint) physicalItemDefinition.Volume * obj.Amount * amountMultiplier;
      }
      return inventory.CurrentVolume + myFixedPoint <= inventory.MaxVolume;
    }

    private bool CheckInventoryContents(
      MyInventory inventory,
      MyBlueprintDefinitionBase.Item item,
      MyFixedPoint amountMultiplier)
    {
      return inventory.ContainItems(new MyFixedPoint?(item.Amount * amountMultiplier), item.Id);
    }

    private bool CheckInventoryContents(
      MyInventory inventory,
      MyBlueprintDefinitionBase.Item[] item,
      MyFixedPoint amountMultiplier)
    {
      for (int index = 0; index < item.Length; ++index)
      {
        if (!inventory.ContainItems(new MyFixedPoint?(item[index].Amount * amountMultiplier), item[index].Id))
          return false;
      }
      return true;
    }

    protected override void OnQueueChanged()
    {
      if (this.CurrentState == MyAssembler.StateEnum.MissingItems && this.IsQueueEmpty && Sandbox.Game.Multiplayer.Sync.IsServer)
        this.CurrentState = !this.Enabled ? MyAssembler.StateEnum.Disabled : (!this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? MyAssembler.StateEnum.NotEnoughPower : (!this.IsFunctional ? MyAssembler.StateEnum.NotWorking : MyAssembler.StateEnum.Ok));
      this.IsProducing = this.IsWorking && !this.IsQueueEmpty;
      base.OnQueueChanged();
    }

    protected override void RemoveFirstQueueItem(int index, MyFixedPoint amount, float progress = 0.0f)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.CurrentProgress = progress;
      if (this.CurrentItemIndex == index)
        this.m_currentQueueItem = new MyProductionBlock.QueueItem?();
      base.RemoveFirstQueueItem(index, amount);
    }

    protected override void RemoveQueueItem(int itemIdx)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer && this.CurrentItemIndex == itemIdx)
      {
        this.CurrentProgress = 0.0f;
        this.m_currentQueueItem = new MyProductionBlock.QueueItem?();
      }
      base.RemoveQueueItem(itemIdx);
    }

    protected override void InsertQueueItem(
      int idx,
      MyBlueprintDefinitionBase blueprint,
      MyFixedPoint amount)
    {
      if (idx == 0)
      {
        MyProductionBlock.QueueItem? firstQueueItem = this.TryGetFirstQueueItem();
        if (firstQueueItem.HasValue && firstQueueItem.Value.Blueprint != blueprint && Sandbox.Game.Multiplayer.Sync.IsServer)
          this.CurrentProgress = 0.0f;
      }
      base.InsertQueueItem(idx, blueprint, amount);
    }

    private void SetRepeat(ref bool currentValue, bool newValue)
    {
      if (currentValue == newValue)
        return;
      currentValue = newValue;
      this.RebuildQueueInRepeatDisassembling();
      if (this.CurrentModeChanged == null)
        return;
      this.CurrentModeChanged(this);
    }

    private void OnSlaveChanged() => this.CurrentModeChanged.InvokeIfNotNull<MyAssembler>(this);

    private void OutputInventory_ContentsChanged(MyInventoryBase inventory)
    {
      if (!this.DisassembleEnabled || !this.RepeatEnabled || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.RebuildQueueInRepeatDisassembling();
    }

    public void RequestDisassembleEnabled(bool newDisassembleEnabled)
    {
      if (newDisassembleEnabled == this.DisassembleEnabled)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyAssembler, bool>(this, (Func<MyAssembler, Action<bool>>) (x => new Action<bool>(x.ModeSwitchCallback)), newDisassembleEnabled);
    }

    [Event(null, 1022)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void ModeSwitchCallback(bool disassembleEnabled)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyAssembler, bool>(this, (Func<MyAssembler, Action<bool>>) (x => new Action<bool>(x.ModeSwitchClient)), disassembleEnabled);
      this.DisassembleEnabled = disassembleEnabled;
    }

    [Event(null, 1029)]
    [Reliable]
    [Broadcast]
    private void ModeSwitchClient(bool disassembleEnabled) => this.DisassembleEnabled = disassembleEnabled;

    public void RequestRepeatEnabled(bool newRepeatEnable)
    {
      if (newRepeatEnable == this.RepeatEnabled)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyAssembler, bool, bool>(this, (Func<MyAssembler, Action<bool, bool>>) (x => new Action<bool, bool>(x.RepeatEnabledCallback)), this.DisassembleEnabled, newRepeatEnable);
    }

    [Event(null, 1043)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void RepeatEnabledCallback(bool disassembleEnabled, bool repeatEnable) => this.RepeatEnabledSuccess(disassembleEnabled, repeatEnable);

    public void RequestDisassembleAll() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyAssembler>(this, (Func<MyAssembler, Action>) (x => new Action(x.DisassembleAllCallback)));

    [Event(null, 1054)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void DisassembleAllCallback() => this.DisassembleAllInOutput();

    private void RepeatEnabledSuccess(bool disassembleMode, bool repeatEnabled)
    {
      if (disassembleMode)
        this.SetRepeat(ref this.m_repeatDisassembleEnabled, repeatEnabled);
      else
        this.SetRepeat(ref this.m_repeatAssembleEnabled, repeatEnabled);
    }

    private void RebuildQueueInRepeatDisassembling()
    {
      if (!this.DisassembleEnabled || !this.RepeatEnabled)
        return;
      this.RequestDisassembleAll();
    }

    private void UpdateInventoryFlags()
    {
      this.OutputInventory.SetFlags(this.DisassembleEnabled ? MyInventoryFlags.CanReceive : MyInventoryFlags.CanSend);
      this.InputInventory.SetFlags(this.DisassembleEnabled ? MyInventoryFlags.CanSend : MyInventoryFlags.CanReceive);
    }

    private void DisassembleAllInOutput()
    {
      this.ClearQueue(false);
      List<MyPhysicalInventoryItem> items = this.OutputInventory.GetItems();
      List<Tuple<MyBlueprintDefinitionBase, MyFixedPoint>> tupleList = new List<Tuple<MyBlueprintDefinitionBase, MyFixedPoint>>();
      bool flag = true;
      foreach (MyPhysicalInventoryItem physicalInventoryItem in items)
      {
        MyBlueprintDefinitionBase definitionByResultId = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(physicalInventoryItem.Content.GetId());
        if (definitionByResultId != null)
        {
          Tuple<MyBlueprintDefinitionBase, MyFixedPoint> tuple = Tuple.Create<MyBlueprintDefinitionBase, MyFixedPoint>(definitionByResultId, physicalInventoryItem.Amount);
          tupleList.Add(tuple);
        }
        else
        {
          flag = false;
          tupleList.Clear();
          break;
        }
      }
      if (flag)
      {
        foreach (Tuple<MyBlueprintDefinitionBase, MyFixedPoint> tuple in tupleList)
          this.InsertQueueItemRequest(-1, tuple.Item1, tuple.Item2);
      }
      else
      {
        this.InitializeInventoryCounts(false);
        for (int index = 0; index < this.m_assemblerDef.BlueprintClasses.Count; ++index)
        {
          foreach (MyBlueprintDefinitionBase blueprint in this.m_assemblerDef.BlueprintClasses[index])
          {
            MyFixedPoint myFixedPoint1 = MyFixedPoint.MaxValue;
            MyFixedPoint myFixedPoint2;
            foreach (MyBlueprintDefinitionBase.Item result in blueprint.Results)
            {
              myFixedPoint2 = (MyFixedPoint) 0;
              MyProductionBlock.m_tmpInventoryCounts.TryGetValue(result.Id, out myFixedPoint2);
              if (myFixedPoint2 == (MyFixedPoint) 0)
              {
                myFixedPoint1 = (MyFixedPoint) 0;
                break;
              }
              myFixedPoint1 = MyFixedPoint.Min((MyFixedPoint) ((double) myFixedPoint2 / (double) result.Amount), myFixedPoint1);
            }
            if (blueprint.Atomic)
              myFixedPoint1 = MyFixedPoint.Floor(myFixedPoint1);
            if (myFixedPoint1 > (MyFixedPoint) 0)
            {
              this.InsertQueueItemRequest(-1, blueprint, myFixedPoint1);
              foreach (MyBlueprintDefinitionBase.Item result in blueprint.Results)
              {
                MyProductionBlock.m_tmpInventoryCounts.TryGetValue(result.Id, out myFixedPoint2);
                myFixedPoint2 -= result.Amount * myFixedPoint1;
                if (myFixedPoint2 == (MyFixedPoint) 0)
                  MyProductionBlock.m_tmpInventoryCounts.Remove(result.Id);
                else
                  MyProductionBlock.m_tmpInventoryCounts[result.Id] = myFixedPoint2;
              }
            }
          }
        }
        MyProductionBlock.m_tmpInventoryCounts.Clear();
      }
    }

    public void GetCoveyorInventoryOwners()
    {
      this.m_inventoryOwners.Clear();
      List<IMyConveyorEndpoint> reachableVertices = new List<IMyConveyorEndpoint>();
      MyGridConveyorSystem.FindReachable(this.ConveyorEndpoint, reachableVertices, (Predicate<IMyConveyorEndpoint>) (vertex => vertex.CubeBlock != null && this.FriendlyWithBlock(vertex.CubeBlock) && vertex.CubeBlock.HasInventory));
      foreach (IMyConveyorEndpoint conveyorEndpoint in reachableVertices)
        this.m_inventoryOwners.Add((MyEntity) conveyorEndpoint.CubeBlock);
      this.m_inventoryOwnersDirty = false;
    }

    public bool CheckConveyorResources(MyFixedPoint? amount, MyDefinitionId contentId)
    {
      foreach (MyEntity inventoryOwner in this.m_inventoryOwners)
      {
        if (inventoryOwner != null)
        {
          MyEntity thisEntity = inventoryOwner;
          if (thisEntity != null && thisEntity.HasInventory)
          {
            MyInventoryFlags flags = MyEntityExtensions.GetInventory(thisEntity).GetFlags();
            MyInventoryFlags myInventoryFlags = MyInventoryFlags.CanReceive | MyInventoryFlags.CanSend;
            List<MyInventory> myInventoryList = new List<MyInventory>();
            for (int index = 0; index < thisEntity.InventoryCount; ++index)
              myInventoryList.Add(MyEntityExtensions.GetInventory(thisEntity, index));
            foreach (MyInventory myInventory in myInventoryList)
            {
              if (myInventory.ContainItems(amount, contentId) && (flags == myInventoryFlags || flags == MyInventoryFlags.CanSend || thisEntity == this))
                return true;
            }
          }
        }
      }
      return false;
    }

    bool Sandbox.ModAPI.Ingame.IMyAssembler.DisassembleEnabled => this.DisassembleEnabled;

    protected override float GetOperationalPowerConsumption() => (float) ((double) base.GetOperationalPowerConsumption() * (1.0 + (double) this.UpgradeValues["Productivity"]) * (1.0 / (double) this.UpgradeValues["PowerEfficiency"]));

    protected override void OnInventoryAddedToAggregate(
      MyInventoryAggregate aggregate,
      MyInventoryBase inventory)
    {
      base.OnInventoryAddedToAggregate(aggregate, inventory);
      if (inventory != this.OutputInventory || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.OutputInventory.ContentsChanged += new Action<MyInventoryBase>(this.OutputInventory_ContentsChanged);
    }

    protected override void OnBeforeInventoryRemovedFromAggregate(
      MyInventoryAggregate aggregate,
      MyInventoryBase inventory)
    {
      base.OnBeforeInventoryRemovedFromAggregate(aggregate, inventory);
      if (inventory != this.OutputInventory || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.OutputInventory.ContentsChanged -= new Action<MyInventoryBase>(this.OutputInventory_ContentsChanged);
    }

    private Action<MyAssembler> GetDelegate(Action<Sandbox.ModAPI.IMyAssembler> value) => (Action<MyAssembler>) Delegate.CreateDelegate(typeof (Action<MyAssembler>), value.Target, value.Method);

    event Action<Sandbox.ModAPI.IMyAssembler> Sandbox.ModAPI.IMyAssembler.CurrentProgressChanged
    {
      add => this.CurrentProgressChanged += this.GetDelegate(value);
      remove => this.CurrentProgressChanged -= this.GetDelegate(value);
    }

    event Action<Sandbox.ModAPI.IMyAssembler> Sandbox.ModAPI.IMyAssembler.CurrentStateChanged
    {
      add => this.CurrentStateChanged += this.GetDelegate(value);
      remove => this.CurrentStateChanged -= this.GetDelegate(value);
    }

    event Action<Sandbox.ModAPI.IMyAssembler> Sandbox.ModAPI.IMyAssembler.CurrentModeChanged
    {
      add => this.CurrentModeChanged += this.GetDelegate(value);
      remove => this.CurrentModeChanged -= this.GetDelegate(value);
    }

    public float GetCurrentBlueprintProductionTime() => this.m_currentItemIndex.Value >= 0 && this.m_queue.Count > 0 ? this.CalculateBlueprintProductionTime(this.m_queue[(int) this.m_currentItemIndex].Blueprint) : 0.0f;

    public MyTimeSpan GetLastProgressUpdateTime() => this.m_lastAnimationUpdate;

    public bool IsProductionRunning() => this.m_currentState.Value == MyAssembler.StateEnum.Ok;

    MyAssemblerMode Sandbox.ModAPI.Ingame.IMyAssembler.Mode
    {
      get => !this.DisassembleEnabled ? MyAssemblerMode.Assembly : MyAssemblerMode.Disassembly;
      set => this.RequestDisassembleEnabled(value == MyAssemblerMode.Disassembly);
    }

    bool Sandbox.ModAPI.Ingame.IMyAssembler.CooperativeMode
    {
      get => this.IsSlave;
      set => this.IsSlave = value;
    }

    bool Sandbox.ModAPI.Ingame.IMyAssembler.Repeating
    {
      get => this.RepeatEnabled;
      set => this.RequestRepeatEnabled(value);
    }

    public virtual int GUIPriority => (int) MathHelper.Lerp(200f, 500f, 1f - this.m_assemblerDef.AssemblySpeed);

    public virtual float GetEfficiencyMultiplierForBlueprint(MyBlueprintDefinitionBase blueprint) => MySession.Static.AssemblerEfficiencyMultiplier;

    public enum StateEnum
    {
      Ok,
      Disabled,
      NotWorking,
      NotEnoughPower,
      MissingItems,
      InventoryFull,
    }

    protected sealed class ModeSwitchCallback\u003C\u003ESystem_Boolean : ICallSite<MyAssembler, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyAssembler @this,
        in bool disassembleEnabled,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ModeSwitchCallback(disassembleEnabled);
      }
    }

    protected sealed class ModeSwitchClient\u003C\u003ESystem_Boolean : ICallSite<MyAssembler, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyAssembler @this,
        in bool disassembleEnabled,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ModeSwitchClient(disassembleEnabled);
      }
    }

    protected sealed class RepeatEnabledCallback\u003C\u003ESystem_Boolean\u0023System_Boolean : ICallSite<MyAssembler, bool, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyAssembler @this,
        in bool disassembleEnabled,
        in bool repeatEnable,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RepeatEnabledCallback(disassembleEnabled, repeatEnable);
      }
    }

    protected sealed class DisassembleAllCallback\u003C\u003E : ICallSite<MyAssembler, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyAssembler @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DisassembleAllCallback();
      }
    }

    protected class m_currentProgress\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.FromServer>(obj1, obj2));
        ((MyAssembler) obj0).m_currentProgress = (VRage.Sync.Sync<float, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_currentItemIndex\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.FromServer>(obj1, obj2));
        ((MyAssembler) obj0).m_currentItemIndex = (VRage.Sync.Sync<int, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_currentState\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyAssembler.StateEnum, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyAssembler.StateEnum, SyncDirection.FromServer>(obj1, obj2));
        ((MyAssembler) obj0).m_currentState = (VRage.Sync.Sync<MyAssembler.StateEnum, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_slave\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyAssembler) obj0).m_slave = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Cube_MyAssembler\u003C\u003EActor : IActivator, IActivator<MyAssembler>
    {
      object IActivator.CreateInstance() => (object) new MyAssembler();

      MyAssembler IActivator<MyAssembler>.CreateInstance() => new MyAssembler();
    }
  }
}
