// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyProductionBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyProductionBlock), typeof (Sandbox.ModAPI.Ingame.IMyProductionBlock)})]
  public abstract class MyProductionBlock : MyFunctionalBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyProductionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProductionBlock, IMyInventoryOwner
  {
    private readonly uint TIMER_NORMAL_IN_FRAMES = 60;
    private readonly uint TIMER_TIER1_IN_FRAMES = 120;
    private readonly uint TIMER_TIER2_IN_FRAMES = 240;
    protected static Dictionary<MyDefinitionId, MyFixedPoint> m_tmpInventoryCounts = new Dictionary<MyDefinitionId, MyFixedPoint>();
    private MyInventoryAggregate m_inventoryAggregate;
    protected uint? m_realProductionStart;
    private MyInventory m_inputInventory;
    private MyInventory m_outputInventory;
    private string m_string;
    private IMyConveyorEndpoint m_multilineConveyorEndpoint;
    private uint m_nextItemId;
    protected readonly VRage.Sync.Sync<bool, SyncDirection.FromServer> m_isProducing;
    protected readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;
    protected MyProductionBlock.QueueItem? m_currentQueueItem;
    protected MySoundPair m_processSound = new MySoundPair();
    protected List<MyProductionBlock.QueueItem> m_queue;
    public Action OnQueueItemRemoved;

    protected MyProductionBlockDefinition ProductionBlockDefinition => (MyProductionBlockDefinition) this.BlockDefinition;

    public MyInventoryAggregate InventoryAggregate
    {
      get => this.m_inventoryAggregate;
      set
      {
        if (value == null)
          return;
        this.Components.Remove<MyInventoryBase>();
        this.Components.Add<MyInventoryBase>((MyInventoryBase) value);
      }
    }

    public MyInventory InputInventory
    {
      get => this.m_inputInventory;
      protected set
      {
        if (this.InventoryAggregate.ChildList.Contains((MyComponentBase) value))
          return;
        if (this.m_inputInventory != null)
          this.InventoryAggregate.ChildList.RemoveComponent((MyComponentBase) this.m_inputInventory);
        this.InventoryAggregate.AddComponent((MyComponentBase) value);
      }
    }

    public MyInventory OutputInventory
    {
      get => this.m_outputInventory;
      protected set
      {
        if (this.InventoryAggregate.ChildList.Contains((MyComponentBase) value))
          return;
        if (this.m_outputInventory != null)
          this.InventoryAggregate.ChildList.RemoveComponent((MyComponentBase) this.m_outputInventory);
        this.InventoryAggregate.AddComponent((MyComponentBase) value);
      }
    }

    public bool IsQueueEmpty => this.m_queue.Count == 0;

    public bool IsProducing
    {
      get => this.m_isProducing.Value;
      protected set
      {
        if (value == this.m_isProducing.Value || !Sandbox.Game.Multiplayer.Sync.IsServer)
          return;
        this.m_isProducing.Value = value;
        this.IsProducing_ValueChanged((SyncBase) null);
      }
    }

    public IEnumerable<MyProductionBlock.QueueItem> Queue => (IEnumerable<MyProductionBlock.QueueItem>) this.m_queue;

    public uint NextItemId => this.m_nextItemId++;

    public bool UseConveyorSystem
    {
      get => (bool) this.m_useConveyorSystem;
      set => this.m_useConveyorSystem.Value = value;
    }

    public override bool AllowTimerForceUpdate => false;

    public event Action<MyProductionBlock> QueueChanged;

    public event Action StartedProducing;

    public event Action StoppedProducing;

    public MyProductionBlock()
    {
      this.CreateTerminalControls();
      this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this, true);
      this.m_queue = new List<MyProductionBlock.QueueItem>();
      this.m_isProducing.ValueChanged += new Action<SyncBase>(this.IsProducing_ValueChanged);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.IsProducing = false;
      this.Components.ComponentAdded += new Action<Type, MyEntityComponentBase>(this.OnComponentAdded);
    }

    private void IsProducing_ValueChanged(SyncBase obj)
    {
      if ((bool) this.m_isProducing)
        this.OnStartProducing();
      else
        this.OnStopProducing();
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        if (this.Closed)
          return;
        this.UpdatePower();
      }), "IsProducing");
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyProductionBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MyProductionBlock> onOff = new MyTerminalControlOnOffSwitch<MyProductionBlock>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem);
      onOff.Getter = (MyTerminalValueControl<MyProductionBlock, bool>.GetterDelegate) (x => x.UseConveyorSystem);
      onOff.Setter = (MyTerminalValueControl<MyProductionBlock, bool>.SetterDelegate) ((x, v) => x.UseConveyorSystem = v);
      onOff.EnableToggleAction<MyProductionBlock>();
      MyTerminalControlFactory.AddControl<MyProductionBlock>((MyTerminalControl<MyProductionBlock>) onOff);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.ProductionBlockDefinition.ResourceSinkGroup, this.ProductionBlockDefinition.OperationalPowerConsumption, new Func<float>(this.ComputeRequiredPower));
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_ProductionBlock builderProductionBlock = (MyObjectBuilder_ProductionBlock) objectBuilder;
      if (this.InventoryAggregate == null)
        this.InventoryAggregate = new MyInventoryAggregate();
      if (this.InputInventory == null)
      {
        this.InputInventory = new MyInventory(this.ProductionBlockDefinition.InventoryMaxVolume, this.ProductionBlockDefinition.InventorySize, MyInventoryFlags.CanReceive);
        if (builderProductionBlock.InputInventory != null)
          this.InputInventory.Init(builderProductionBlock.InputInventory);
      }
      if (this.OutputInventory == null)
      {
        this.OutputInventory = new MyInventory(this.ProductionBlockDefinition.InventoryMaxVolume, this.ProductionBlockDefinition.InventorySize, MyInventoryFlags.CanSend);
        if (builderProductionBlock.OutputInventory != null)
          this.OutputInventory.Init(builderProductionBlock.OutputInventory);
      }
      this.m_nextItemId = builderProductionBlock.NextItemId;
      int nextItemId = (int) this.m_nextItemId;
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.CubeBlock_IsWorkingChanged);
      this.ResourceSink.Update();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      if (builderProductionBlock.Queue != null)
      {
        this.m_queue.Clear();
        if (this.m_queue.Capacity < builderProductionBlock.Queue.Length)
          this.m_queue.Capacity = builderProductionBlock.Queue.Length;
        for (int index = 0; index < builderProductionBlock.Queue.Length; ++index)
        {
          MyObjectBuilder_ProductionBlock.QueueItem itemOb = builderProductionBlock.Queue[index];
          MyProductionBlock.QueueItem queueItem = this.DeserializeQueueItem(itemOb);
          if (queueItem.Blueprint != null)
            this.m_queue.Add(queueItem);
          else
            MySandboxGame.Log.WriteLine(string.Format("Could not add item into production block's queue: Blueprint {0} was not found.", (object) itemOb.Id));
        }
        this.UpdatePower();
      }
      this.m_useConveyorSystem.SetLocalValue(builderProductionBlock.UseConveyorSystem);
      this.CreateUpdateTimer(this.GetTimerTime(0), MyTimerTypes.Frame10);
    }

    protected virtual void UpdatePower()
    {
      if (this.ResourceSink == null)
        return;
      this.ResourceSink.Update();
    }

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_ProductionBlock builderCubeBlock = (MyObjectBuilder_ProductionBlock) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.UseConveyorSystem = (bool) this.m_useConveyorSystem;
      builderCubeBlock.NextItemId = this.m_nextItemId;
      if (this.m_queue.Count > 0)
      {
        builderCubeBlock.Queue = new MyObjectBuilder_ProductionBlock.QueueItem[this.m_queue.Count];
        for (int index = 0; index < this.m_queue.Count; ++index)
        {
          builderCubeBlock.Queue[index].Id = (SerializableDefinitionId) this.m_queue[index].Blueprint.Id;
          builderCubeBlock.Queue[index].Amount = this.m_queue[index].Amount;
          builderCubeBlock.Queue[index].ItemId = new uint?(this.m_queue[index].ItemId);
        }
      }
      else
        builderCubeBlock.Queue = (MyObjectBuilder_ProductionBlock.QueueItem[]) null;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    public bool CanUseBlueprint(MyBlueprintDefinitionBase blueprint)
    {
      foreach (MyBlueprintClassDefinition blueprintClass in this.ProductionBlockDefinition.BlueprintClasses)
      {
        if (blueprintClass.ContainsBlueprint(blueprint))
          return true;
      }
      return false;
    }

    protected void InitializeInventoryCounts(bool inputInventory = true)
    {
      MyProductionBlock.m_tmpInventoryCounts.Clear();
      foreach (MyPhysicalInventoryItem physicalInventoryItem in inputInventory ? this.InputInventory.GetItems() : this.OutputInventory.GetItems())
      {
        MyFixedPoint myFixedPoint = (MyFixedPoint) 0;
        MyDefinitionId key = new MyDefinitionId(physicalInventoryItem.Content.TypeId, physicalInventoryItem.Content.SubtypeId);
        MyProductionBlock.m_tmpInventoryCounts.TryGetValue(key, out myFixedPoint);
        MyProductionBlock.m_tmpInventoryCounts[key] = myFixedPoint + physicalInventoryItem.Amount;
      }
    }

    public void AddQueueItemRequest(
      MyBlueprintDefinitionBase blueprint,
      MyFixedPoint ammount,
      int idx = -1)
    {
      if (!Sandbox.Engine.Platform.Game.IsDedicated && this is MyAssembler)
        ((MyAssembler) this).UpdateCurrentState(blueprint);
      SerializableDefinitionId id = (SerializableDefinitionId) blueprint.Id;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProductionBlock, int, SerializableDefinitionId, MyFixedPoint>(this, (Func<MyProductionBlock, Action<int, SerializableDefinitionId, MyFixedPoint>>) (x => new Action<int, SerializableDefinitionId, MyFixedPoint>(x.OnAddQueueItemRequest)), idx, id, ammount);
    }

    [Event(null, 433)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnAddQueueItemRequest(
      int idx,
      SerializableDefinitionId defId,
      MyFixedPoint ammount)
    {
      MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition((MyDefinitionId) defId);
      if (blueprintDefinition == null)
        return;
      this.InsertQueueItem(idx, blueprintDefinition, ammount);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProductionBlock, int, SerializableDefinitionId, MyFixedPoint>(this, (Func<MyProductionBlock, Action<int, SerializableDefinitionId, MyFixedPoint>>) (x => new Action<int, SerializableDefinitionId, MyFixedPoint>(x.OnAddQueueItemSuccess)), idx, defId, ammount);
    }

    [Event(null, 446)]
    [Reliable]
    [Broadcast]
    private void OnAddQueueItemSuccess(
      int idx,
      SerializableDefinitionId defId,
      MyFixedPoint ammount)
    {
      this.InsertQueueItem(idx, MyDefinitionManager.Static.GetBlueprintDefinition((MyDefinitionId) defId), ammount);
    }

    public void MoveQueueItemRequest(uint srcItemId, int dstIdx) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProductionBlock, uint, int>(this, (Func<MyProductionBlock, Action<uint, int>>) (x => new Action<uint, int>(x.OnMoveQueueItemCallback)), srcItemId, dstIdx);

    [Event(null, 463)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnMoveQueueItemCallback(uint srcItemId, int dstIdx) => this.MoveQueueItem(srcItemId, dstIdx);

    [Event(null, 469)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    protected void ClearQueueRequest()
    {
      for (int idx = this.m_queue.Count - 1; idx >= 0; --idx)
      {
        if (this.RemoveQueueItemTests(idx))
        {
          MyFixedPoint myFixedPoint = (MyFixedPoint) -1;
          Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProductionBlock, int, MyFixedPoint, float>(this, (Func<MyProductionBlock, Action<int, MyFixedPoint, float>>) (x => new Action<int, MyFixedPoint, float>(x.OnRemoveQueueItem)), idx, myFixedPoint, 0.0f);
        }
      }
    }

    public void RemoveQueueItemRequest(int idx, MyFixedPoint amount, float progress = 0.0f) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProductionBlock, int, MyFixedPoint, float>(this, (Func<MyProductionBlock, Action<int, MyFixedPoint, float>>) (x => new Action<int, MyFixedPoint, float>(x.OnRemoveQueueItemRequest)), idx, amount, progress);

    [Event(null, 495)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnRemoveQueueItemRequest(int idx, MyFixedPoint amount, float progress)
    {
      if (!this.RemoveQueueItemTests(idx))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyProductionBlock, int, MyFixedPoint, float>(this, (Func<MyProductionBlock, Action<int, MyFixedPoint, float>>) (x => new Action<int, MyFixedPoint, float>(x.OnRemoveQueueItem)), idx, amount, progress);
    }

    private bool RemoveQueueItemTests(int idx)
    {
      if (this.m_queue.IsValidIndex<MyProductionBlock.QueueItem>(idx) || idx == -1)
        return true;
      MySandboxGame.Log.WriteLine("Invalid queue index in the remove item message!");
      return false;
    }

    [Event(null, 520)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private void OnRemoveQueueItem(int idx, MyFixedPoint amount, float progress)
    {
      if (amount >= (MyFixedPoint) 0)
      {
        this.RemoveFirstQueueItem(idx, amount, progress);
        this.OnQueueItemRemoved.InvokeIfNotNull();
      }
      else
        this.RemoveQueueItem(idx);
    }

    protected virtual void OnQueueChanged()
    {
      if (this.QueueChanged == null)
        return;
      this.QueueChanged(this);
    }

    private MyProductionBlock.QueueItem DeserializeQueueItem(
      MyObjectBuilder_ProductionBlock.QueueItem itemOb)
    {
      return new MyProductionBlock.QueueItem()
      {
        Amount = itemOb.Amount,
        Blueprint = !MyDefinitionManager.Static.HasBlueprint((MyDefinitionId) itemOb.Id) ? MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId((MyDefinitionId) itemOb.Id) : MyDefinitionManager.Static.GetBlueprintDefinition((MyDefinitionId) itemOb.Id),
        ItemId = itemOb.ItemId.HasValue ? itemOb.ItemId.Value : this.NextItemId
      };
    }

    protected void RemoveFirstQueueItemAnnounce(MyFixedPoint amount, float progress = 0.0f) => this.RemoveQueueItemRequest(0, amount, progress);

    protected virtual void RemoveFirstQueueItem(int index, MyFixedPoint amount, float progress = 0.0f)
    {
      if (!this.m_queue.IsValidIndex<MyProductionBlock.QueueItem>(index))
        return;
      MyProductionBlock.QueueItem queueItem = this.m_queue[index];
      amount = MathHelper.Clamp(amount, (MyFixedPoint) 0, queueItem.Amount);
      queueItem.Amount -= amount;
      this.m_queue[index] = queueItem;
      if (queueItem.Amount <= (MyFixedPoint) 0)
      {
        if (this is MyAssembler myAssembler && Sandbox.Game.Multiplayer.Sync.IsServer)
          myAssembler.CurrentProgress = 0.0f;
        this.m_queue.RemoveAt(index);
      }
      this.UpdatePower();
      this.OnQueueChanged();
    }

    public void InsertQueueItemRequest(int idx, MyBlueprintDefinitionBase blueprint) => this.InsertQueueItemRequest(idx, blueprint, (MyFixedPoint) 1);

    public void InsertQueueItemRequest(
      int idx,
      MyBlueprintDefinitionBase blueprint,
      MyFixedPoint amount)
    {
      this.AddQueueItemRequest(blueprint, amount, idx);
    }

    protected virtual void InsertQueueItem(
      int idx,
      MyBlueprintDefinitionBase blueprint,
      MyFixedPoint amount)
    {
      if (!this.CanUseBlueprint(blueprint))
        return;
      MyProductionBlock.QueueItem queueItem = new MyProductionBlock.QueueItem();
      queueItem.Amount = amount;
      queueItem.Blueprint = blueprint;
      int num = this.m_queue.Count == 0 ? 1 : 0;
      if (this.m_queue.IsValidIndex<MyProductionBlock.QueueItem>(idx) && this.m_queue[idx].Blueprint == queueItem.Blueprint)
      {
        queueItem.Amount += this.m_queue[idx].Amount;
        queueItem.ItemId = this.m_queue[idx].ItemId;
        if (this.m_currentQueueItem.HasValue && (int) this.m_queue[idx].ItemId == (int) this.m_currentQueueItem.Value.ItemId)
          this.m_currentQueueItem = new MyProductionBlock.QueueItem?(queueItem);
        this.m_queue[idx] = queueItem;
      }
      else if (this.m_queue.Count > 0 && (idx >= this.m_queue.Count || idx == -1) && this.m_queue[this.m_queue.Count - 1].Blueprint == queueItem.Blueprint)
      {
        queueItem.Amount += this.m_queue[this.m_queue.Count - 1].Amount;
        queueItem.ItemId = this.m_queue[this.m_queue.Count - 1].ItemId;
        if (this.m_currentQueueItem.HasValue && (int) this.m_queue[this.m_queue.Count - 1].ItemId == (int) this.m_currentQueueItem.Value.ItemId)
          this.m_currentQueueItem = new MyProductionBlock.QueueItem?(queueItem);
        this.m_queue[this.m_queue.Count - 1] = queueItem;
      }
      else
      {
        if (idx == -1)
          idx = this.m_queue.Count;
        if (idx > this.m_queue.Count)
        {
          MyLog.Default.WriteLine("Production block.InsertQueueItem: Index out of bounds, desync!");
          idx = this.m_queue.Count;
        }
        queueItem.ItemId = this.NextItemId;
        this.m_queue.Insert(idx, queueItem);
      }
      if (num != 0 && Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_realProductionStart = new uint?(this.GetFramesFromLastTrigger());
      this.UpdatePower();
      this.OnQueueChanged();
    }

    public MyProductionBlock.QueueItem GetQueueItem(int idx) => this.m_queue[idx];

    public MyProductionBlock.QueueItem? TryGetQueueItem(int idx) => !this.m_queue.IsValidIndex<MyProductionBlock.QueueItem>(idx) ? new MyProductionBlock.QueueItem?() : new MyProductionBlock.QueueItem?(this.m_queue[idx]);

    public MyProductionBlock.QueueItem? TryGetQueueItemById(uint itemId)
    {
      for (int index = 0; index < this.m_queue.Count; ++index)
      {
        if ((int) this.m_queue[index].ItemId == (int) itemId)
          return new MyProductionBlock.QueueItem?(this.m_queue[index]);
      }
      return new MyProductionBlock.QueueItem?();
    }

    protected virtual void RemoveQueueItem(int itemIdx)
    {
      if (itemIdx >= this.m_queue.Count)
      {
        MyLog.Default.WriteLine("Production block.RemoveQueueItem: Index out of bounds!");
      }
      else
      {
        this.m_queue.RemoveAt(itemIdx);
        this.UpdatePower();
        this.OnQueueChanged();
      }
    }

    protected virtual void MoveQueueItem(uint queueItemId, int targetIdx)
    {
      for (int index1 = 0; index1 < this.m_queue.Count; ++index1)
      {
        if ((int) this.m_queue[index1].ItemId == (int) queueItemId)
        {
          MyProductionBlock.QueueItem queueItem1 = this.m_queue[index1];
          targetIdx = Math.Min(this.m_queue.Count - 1, targetIdx);
          if (index1 == targetIdx)
            return;
          this.m_queue.RemoveAt(index1);
          int index2 = -1;
          if (this.m_queue.IsValidIndex<MyProductionBlock.QueueItem>(targetIdx - 1) && this.m_queue[targetIdx - 1].Blueprint == queueItem1.Blueprint)
            index2 = targetIdx - 1;
          if (this.m_queue.IsValidIndex<MyProductionBlock.QueueItem>(targetIdx) && this.m_queue[targetIdx].Blueprint == queueItem1.Blueprint)
            index2 = targetIdx;
          if (index2 != -1)
          {
            MyProductionBlock.QueueItem queueItem2 = this.m_queue[index2];
            queueItem2.Amount += queueItem1.Amount;
            this.m_queue[index2] = queueItem2;
            break;
          }
          this.m_queue.Insert(targetIdx, queueItem1);
          break;
        }
      }
      this.OnQueueChanged();
    }

    public MyProductionBlock.QueueItem? TryGetFirstQueueItem() => this.TryGetQueueItem(0);

    public void ClearQueue(bool sendEvent = true)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.ClearQueueRequest();
      if (!sendEvent)
        return;
      this.OnQueueChanged();
    }

    protected void SwapQueue(ref List<MyProductionBlock.QueueItem> otherQueue)
    {
      List<MyProductionBlock.QueueItem> queue = this.m_queue;
      this.m_queue = otherQueue;
      otherQueue = queue;
      this.OnQueueChanged();
    }

    protected override void Closing()
    {
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(true);
      base.Closing();
    }

    public override bool GetTimerEnabledState() => this.IsWorking && this.Enabled;

    public override MyInventoryBase GetInventoryBase(int index = 0)
    {
      if (index == 0)
        return (MyInventoryBase) this.InputInventory;
      if (index == 1)
        return (MyInventoryBase) this.OutputInventory;
      throw new InvalidBranchException();
    }

    public override void OnRemovedByCubeBuilder()
    {
      this.ReleaseInventory(this.InputInventory);
      this.ReleaseInventory(this.OutputInventory);
      base.OnRemovedByCubeBuilder();
    }

    public override void OnDestroy()
    {
      this.ReleaseInventory(this.InputInventory, true);
      this.ReleaseInventory(this.OutputInventory, true);
      base.OnDestroy();
    }

    private void OnComponentAdded(Type type, MyEntityComponentBase component)
    {
      if (!(component is MyInventoryAggregate aggregate))
        return;
      this.m_inventoryAggregate = aggregate;
      this.m_inventoryAggregate.BeforeRemovedFromContainer += new Action<MyEntityComponentBase>(this.OnInventoryAggregateRemoved);
      this.m_inventoryAggregate.OnAfterComponentAdd += new Action<MyInventoryAggregate, MyInventoryBase>(this.OnInventoryAddedToAggregate);
      this.m_inventoryAggregate.OnBeforeComponentRemove += new Action<MyInventoryAggregate, MyInventoryBase>(this.OnBeforeInventoryRemovedFromAggregate);
      foreach (MyComponentBase myComponentBase in this.m_inventoryAggregate.ChildList.Reader)
      {
        MyInventory myInventory = myComponentBase as MyInventory;
        this.OnInventoryAddedToAggregate(aggregate, (MyInventoryBase) myInventory);
      }
    }

    protected virtual void OnInventoryAddedToAggregate(
      MyInventoryAggregate aggregate,
      MyInventoryBase inventory)
    {
      if (this.m_inputInventory == null)
      {
        this.m_inputInventory = inventory as MyInventory;
      }
      else
      {
        if (this.m_outputInventory != null)
          return;
        this.m_outputInventory = inventory as MyInventory;
      }
    }

    private void OnInventoryAggregateRemoved(MyEntityComponentBase component)
    {
      this.m_inputInventory = (MyInventory) null;
      this.m_outputInventory = (MyInventory) null;
      this.m_inventoryAggregate.BeforeRemovedFromContainer -= new Action<MyEntityComponentBase>(this.OnInventoryAggregateRemoved);
      this.m_inventoryAggregate.OnAfterComponentAdd -= new Action<MyInventoryAggregate, MyInventoryBase>(this.OnInventoryAddedToAggregate);
      this.m_inventoryAggregate.OnBeforeComponentRemove -= new Action<MyInventoryAggregate, MyInventoryBase>(this.OnBeforeInventoryRemovedFromAggregate);
      this.m_inventoryAggregate = (MyInventoryAggregate) null;
    }

    protected virtual void OnBeforeInventoryRemovedFromAggregate(
      MyInventoryAggregate aggregate,
      MyInventoryBase inventory)
    {
      if (inventory == this.m_inputInventory)
      {
        this.m_inputInventory = (MyInventory) null;
      }
      else
      {
        if (inventory != this.m_outputInventory)
          return;
        this.m_outputInventory = (MyInventory) null;
      }
    }

    protected override void OnEnabledChanged()
    {
      this.UpdatePower();
      base.OnEnabledChanged();
      if (!this.IsWorking || !this.IsProducing)
        return;
      this.OnStartProducing();
    }

    private float ComputeRequiredPower()
    {
      if (!this.Enabled || !this.IsFunctional)
        return 0.0f;
      return !this.IsProducing || this.IsQueueEmpty ? this.ProductionBlockDefinition.StandbyPowerConsumption : this.GetOperationalPowerConsumption();
    }

    protected virtual float GetOperationalPowerConsumption() => this.ProductionBlockDefinition.OperationalPowerConsumption;

    private void Receiver_IsPoweredChanged()
    {
      if (!this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
        this.IsProducing = false;
      this.UpdateIsWorking();
    }

    private void CubeBlock_IsWorkingChanged(MyCubeBlock block)
    {
      if (!this.IsWorking || !this.IsProducing)
        return;
      this.OnStartProducing();
    }

    protected void OnStartProducing()
    {
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.PlaySound(this.m_processSound, true);
      this.StartedProducing.InvokeIfNotNull();
    }

    protected void OnStopProducing()
    {
      if (this.m_soundEmitter != null)
      {
        if (this.IsWorking)
        {
          this.m_soundEmitter.StopSound(false);
          this.m_soundEmitter.PlaySound(this.m_baseIdleSound, skipIntro: true);
        }
        else
          this.m_soundEmitter.StopSound(false);
      }
      this.StoppedProducing.InvokeIfNotNull();
    }

    public void FixInputOutputInventories(
      MyInventoryConstraint inputInventoryConstraint,
      MyInventoryConstraint outputInventoryConstraint)
    {
      if (this.m_inventoryAggregate.InventoryCount == 2)
        return;
      MyInventoryAggregate inventoryAggregate = MyInventoryAggregate.FixInputOutputInventories(this.m_inventoryAggregate, inputInventoryConstraint, outputInventoryConstraint);
      this.Components.Remove<MyInventoryBase>();
      this.m_outputInventory = (MyInventory) null;
      this.m_inputInventory = (MyInventory) null;
      this.Components.Add<MyInventoryBase>((MyInventoryBase) inventoryAggregate);
    }

    protected override void TiersChanged()
    {
      switch (this.CubeGrid.PlayerPresenceTier)
      {
        case MyUpdateTiersPlayerPresence.Normal:
          this.ChangeTimerTick(this.GetTimerTime(0));
          break;
        case MyUpdateTiersPlayerPresence.Tier1:
          this.ChangeTimerTick(this.GetTimerTime(1));
          break;
        case MyUpdateTiersPlayerPresence.Tier2:
          this.ChangeTimerTick(this.GetTimerTime(2));
          break;
      }
    }

    protected override uint GetDefaultTimeForUpdateTimer(int index)
    {
      switch (index)
      {
        case 0:
          return this.TIMER_NORMAL_IN_FRAMES;
        case 1:
          return this.TIMER_TIER1_IN_FRAMES;
        case 2:
          return this.TIMER_TIER2_IN_FRAMES;
        default:
          return 0;
      }
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

    public virtual PullInformation GetPullInformation() => new PullInformation()
    {
      OwnerID = this.OwnerId,
      Inventory = this.InputInventory,
      Constraint = this.InputInventory.Constraint
    };

    public virtual PullInformation GetPushInformation() => new PullInformation()
    {
      OwnerID = this.OwnerId,
      Inventory = this.OutputInventory,
      Constraint = this.OutputInventory.Constraint
    };

    public virtual bool AllowSelfPulling() => false;

    public IMyConveyorEndpoint ConveyorEndpoint => this.m_multilineConveyorEndpoint;

    public void InitializeConveyorEndpoint()
    {
      this.m_multilineConveyorEndpoint = (IMyConveyorEndpoint) new MyMultilineConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint(this.m_multilineConveyorEndpoint));
    }

    bool Sandbox.ModAPI.IMyProductionBlock.CanUseBlueprint(
      MyDefinitionBase blueprint)
    {
      return this.CanUseBlueprint((MyBlueprintDefinitionBase) (blueprint as MyBlueprintDefinition));
    }

    void Sandbox.ModAPI.IMyProductionBlock.AddQueueItem(
      MyDefinitionBase blueprint,
      MyFixedPoint amount)
    {
      this.AddQueueItemRequest((MyBlueprintDefinitionBase) (blueprint as MyBlueprintDefinition), amount);
    }

    void Sandbox.ModAPI.IMyProductionBlock.InsertQueueItem(
      int idx,
      MyDefinitionBase blueprint,
      MyFixedPoint amount)
    {
      this.InsertQueueItemRequest(idx, (MyBlueprintDefinitionBase) (blueprint as MyBlueprintDefinition), amount);
    }

    List<MyProductionQueueItem> Sandbox.ModAPI.IMyProductionBlock.GetQueue()
    {
      List<MyProductionQueueItem> productionQueueItemList = new List<MyProductionQueueItem>(this.m_queue.Count);
      foreach (MyProductionBlock.QueueItem queueItem in this.m_queue)
        productionQueueItemList.Add(new MyProductionQueueItem()
        {
          Amount = queueItem.Amount,
          Blueprint = (MyDefinitionBase) queueItem.Blueprint,
          ItemId = queueItem.ItemId
        });
      return productionQueueItemList;
    }

    VRage.Game.ModAPI.IMyInventory Sandbox.ModAPI.IMyProductionBlock.InputInventory => (VRage.Game.ModAPI.IMyInventory) this.InputInventory;

    VRage.Game.ModAPI.IMyInventory Sandbox.ModAPI.IMyProductionBlock.OutputInventory => (VRage.Game.ModAPI.IMyInventory) this.OutputInventory;

    VRage.Game.ModAPI.Ingame.IMyInventory Sandbox.ModAPI.Ingame.IMyProductionBlock.InputInventory => (VRage.Game.ModAPI.Ingame.IMyInventory) this.InputInventory;

    VRage.Game.ModAPI.Ingame.IMyInventory Sandbox.ModAPI.Ingame.IMyProductionBlock.OutputInventory => (VRage.Game.ModAPI.Ingame.IMyInventory) this.OutputInventory;

    bool Sandbox.ModAPI.Ingame.IMyProductionBlock.CanUseBlueprint(
      MyDefinitionId blueprint)
    {
      MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(blueprint);
      return blueprintDefinition != null && this.CanUseBlueprint(blueprintDefinition);
    }

    void Sandbox.ModAPI.Ingame.IMyProductionBlock.AddQueueItem(
      MyDefinitionId blueprint,
      MyFixedPoint amount)
    {
      this.AddQueueItemRequest(MyDefinitionManager.Static.GetBlueprintDefinition(blueprint), amount);
    }

    void Sandbox.ModAPI.Ingame.IMyProductionBlock.AddQueueItem(
      MyDefinitionId blueprint,
      Decimal amount)
    {
      this.AddQueueItemRequest(MyDefinitionManager.Static.GetBlueprintDefinition(blueprint), (MyFixedPoint) amount);
    }

    void Sandbox.ModAPI.Ingame.IMyProductionBlock.AddQueueItem(
      MyDefinitionId blueprint,
      double amount)
    {
      this.AddQueueItemRequest(MyDefinitionManager.Static.GetBlueprintDefinition(blueprint), (MyFixedPoint) amount);
    }

    void Sandbox.ModAPI.Ingame.IMyProductionBlock.InsertQueueItem(
      int idx,
      MyDefinitionId blueprint,
      MyFixedPoint amount)
    {
      MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(blueprint);
      this.InsertQueueItemRequest(idx, blueprintDefinition, amount);
    }

    void Sandbox.ModAPI.Ingame.IMyProductionBlock.InsertQueueItem(
      int idx,
      MyDefinitionId blueprint,
      Decimal amount)
    {
      MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(blueprint);
      this.InsertQueueItemRequest(idx, blueprintDefinition, (MyFixedPoint) amount);
    }

    void Sandbox.ModAPI.Ingame.IMyProductionBlock.InsertQueueItem(
      int idx,
      MyDefinitionId blueprint,
      double amount)
    {
      MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(blueprint);
      this.InsertQueueItemRequest(idx, blueprintDefinition, (MyFixedPoint) amount);
    }

    void Sandbox.ModAPI.Ingame.IMyProductionBlock.RemoveQueueItem(
      int idx,
      MyFixedPoint amount)
    {
      this.RemoveQueueItemRequest(idx, amount);
    }

    void Sandbox.ModAPI.Ingame.IMyProductionBlock.RemoveQueueItem(
      int idx,
      Decimal amount)
    {
      this.RemoveQueueItemRequest(idx, (MyFixedPoint) amount);
    }

    void Sandbox.ModAPI.Ingame.IMyProductionBlock.RemoveQueueItem(
      int idx,
      double amount)
    {
      this.RemoveQueueItemRequest(idx, (MyFixedPoint) amount);
    }

    void Sandbox.ModAPI.Ingame.IMyProductionBlock.ClearQueue() => this.ClearQueueRequest();

    void Sandbox.ModAPI.Ingame.IMyProductionBlock.GetQueue(
      List<MyProductionItem> items)
    {
      items.Clear();
      for (int index = 0; index < this.m_queue.Count; ++index)
      {
        MyProductionBlock.QueueItem queueItem = this.m_queue[index];
        items.Add(new MyProductionItem(queueItem.ItemId, queueItem.Blueprint.Id, queueItem.Amount));
      }
    }

    public struct QueueItem
    {
      public MyFixedPoint Amount;
      public MyBlueprintDefinitionBase Blueprint;
      public uint ItemId;
    }

    protected sealed class OnAddQueueItemRequest\u003C\u003ESystem_Int32\u0023VRage_ObjectBuilders_SerializableDefinitionId\u0023VRage_MyFixedPoint : ICallSite<MyProductionBlock, int, SerializableDefinitionId, MyFixedPoint, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProductionBlock @this,
        in int idx,
        in SerializableDefinitionId defId,
        in MyFixedPoint ammount,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnAddQueueItemRequest(idx, defId, ammount);
      }
    }

    protected sealed class OnAddQueueItemSuccess\u003C\u003ESystem_Int32\u0023VRage_ObjectBuilders_SerializableDefinitionId\u0023VRage_MyFixedPoint : ICallSite<MyProductionBlock, int, SerializableDefinitionId, MyFixedPoint, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProductionBlock @this,
        in int idx,
        in SerializableDefinitionId defId,
        in MyFixedPoint ammount,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnAddQueueItemSuccess(idx, defId, ammount);
      }
    }

    protected sealed class OnMoveQueueItemCallback\u003C\u003ESystem_UInt32\u0023System_Int32 : ICallSite<MyProductionBlock, uint, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProductionBlock @this,
        in uint srcItemId,
        in int dstIdx,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnMoveQueueItemCallback(srcItemId, dstIdx);
      }
    }

    protected sealed class ClearQueueRequest\u003C\u003E : ICallSite<MyProductionBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProductionBlock @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ClearQueueRequest();
      }
    }

    protected sealed class OnRemoveQueueItemRequest\u003C\u003ESystem_Int32\u0023VRage_MyFixedPoint\u0023System_Single : ICallSite<MyProductionBlock, int, MyFixedPoint, float, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProductionBlock @this,
        in int idx,
        in MyFixedPoint amount,
        in float progress,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveQueueItemRequest(idx, amount, progress);
      }
    }

    protected sealed class OnRemoveQueueItem\u003C\u003ESystem_Int32\u0023VRage_MyFixedPoint\u0023System_Single : ICallSite<MyProductionBlock, int, MyFixedPoint, float, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyProductionBlock @this,
        in int idx,
        in MyFixedPoint amount,
        in float progress,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveQueueItem(idx, amount, progress);
      }
    }

    protected class m_isProducing\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.FromServer>(obj1, obj2));
        ((MyProductionBlock) obj0).m_isProducing = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_useConveyorSystem\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyProductionBlock) obj0).m_useConveyorSystem = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
