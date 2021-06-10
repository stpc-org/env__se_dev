// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyGasGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
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
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
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

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_OxygenGenerator))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyGasGenerator), typeof (Sandbox.ModAPI.Ingame.IMyGasGenerator), typeof (Sandbox.ModAPI.IMyOxygenGenerator), typeof (Sandbox.ModAPI.Ingame.IMyOxygenGenerator)})]
  public class MyGasGenerator : MyFunctionalBlock, IMyGasBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyOxygenGenerator, Sandbox.ModAPI.IMyGasGenerator, Sandbox.ModAPI.Ingame.IMyGasGenerator, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyOxygenGenerator, IMyInventoryOwner, IMyEventProxy, IMyEventOwner
  {
    private static readonly int NUMBER_PULLS_BOTTLES = 3;
    private readonly uint TIMER_NORMAL_IN_FRAMES = 300;
    private readonly uint TIMER_TIER1_IN_FRAMES = 600;
    private readonly uint TIMER_TIER2_IN_FRAMES = 1200;
    private float m_productionCapacityMultiplier = 1f;
    private float m_powerConsumptionMultiplier = 1f;
    private float m_actualCheckFillValue;
    private float m_iceAmount;
    private int m_numberOfPullsForBottles = MyGasGenerator.NUMBER_PULLS_BOTTLES;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_autoRefill;
    private bool m_isProducing;
    private MyInventoryConstraint m_oreConstraint;
    private MyInventoryConstraint m_containersConstraint;
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;
    private MyResourceSourceComponent m_sourceComp;

    private MyOxygenGeneratorDefinition BlockDefinition => (MyOxygenGeneratorDefinition) base.BlockDefinition;

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public bool UseConveyorSystem
    {
      get => (bool) this.m_useConveyorSystem;
      set => this.m_useConveyorSystem.Value = value;
    }

    public bool CanPressurizeRoom => false;

    public bool CanProduce => (MySession.Static != null && MySession.Static.Settings.EnableOxygen || !this.BlockDefinition.IsOxygenOnly) && (this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && this.IsWorking && this.Enabled) && this.IsFunctional;

    public bool AutoRefill
    {
      get => (bool) this.m_autoRefill;
      set => this.m_autoRefill.Value = value;
    }

    public MyResourceSourceComponent SourceComp
    {
      get => this.m_sourceComp;
      set
      {
        if (this.Components.Contains(typeof (MyResourceSourceComponent)))
          this.Components.Remove<MyResourceSourceComponent>();
        this.Components.Add<MyResourceSourceComponent>(value);
        this.m_sourceComp = value;
      }
    }

    public override bool IsTieredUpdateSupported => true;

    public MyGasGenerator()
    {
      this.CreateTerminalControls();
      this.SourceComp = new MyResourceSourceComponent(2);
      this.ResourceSink = new MyResourceSinkComponent();
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyGasGenerator>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MyGasGenerator> onOff = new MyTerminalControlOnOffSwitch<MyGasGenerator>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem);
      onOff.Getter = (MyTerminalValueControl<MyGasGenerator, bool>.GetterDelegate) (x => x.UseConveyorSystem);
      onOff.Setter = (MyTerminalValueControl<MyGasGenerator, bool>.SetterDelegate) ((x, v) => x.UseConveyorSystem = v);
      onOff.EnableToggleAction<MyGasGenerator>();
      MyTerminalControlFactory.AddControl<MyGasGenerator>((MyTerminalControl<MyGasGenerator>) onOff);
      MyTerminalControlButton<MyGasGenerator> button = new MyTerminalControlButton<MyGasGenerator>("Refill", MySpaceTexts.BlockPropertyTitle_Refill, MySpaceTexts.BlockPropertyTitle_Refill, new Action<MyGasGenerator>(MyGasGenerator.OnRefillButtonPressed));
      button.Enabled = (Func<MyGasGenerator, bool>) (x => x.CanRefill());
      button.EnableAction<MyGasGenerator>();
      MyTerminalControlFactory.AddControl<MyGasGenerator>((MyTerminalControl<MyGasGenerator>) button);
      MyTerminalControlCheckbox<MyGasGenerator> checkbox = new MyTerminalControlCheckbox<MyGasGenerator>("Auto-Refill", MySpaceTexts.BlockPropertyTitle_AutoRefill, MySpaceTexts.BlockPropertyTitle_AutoRefill);
      checkbox.Getter = (MyTerminalValueControl<MyGasGenerator, bool>.GetterDelegate) (x => x.AutoRefill);
      checkbox.Setter = (MyTerminalValueControl<MyGasGenerator, bool>.SetterDelegate) ((x, v) => x.AutoRefill = v);
      checkbox.EnableAction<MyGasGenerator>();
      MyTerminalControlFactory.AddControl<MyGasGenerator>((MyTerminalControl<MyGasGenerator>) checkbox);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      List<MyResourceSourceInfo> sourceResourceData = new List<MyResourceSourceInfo>();
      foreach (MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo producedGase in this.BlockDefinition.ProducedGases)
        sourceResourceData.Add(new MyResourceSourceInfo()
        {
          ResourceTypeId = producedGase.Id,
          DefinedOutput = (float) ((double) this.BlockDefinition.IceConsumptionPerSecond * (double) producedGase.IceToGasRatio * (MySession.Static.CreativeMode ? 10.0 : 1.0)),
          ProductionToCapacityMultiplier = 1f
        });
      this.SourceComp.Init(this.BlockDefinition.ResourceSourceGroup, sourceResourceData);
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_OxygenGenerator builderOxygenGenerator = objectBuilder as MyObjectBuilder_OxygenGenerator;
      this.InitializeConveyorEndpoint();
      this.m_useConveyorSystem.SetLocalValue(builderOxygenGenerator.UseConveyorSystem);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory == null)
      {
        inventory = new MyInventory(this.BlockDefinition.InventoryMaxVolume, this.BlockDefinition.InventorySize, MyInventoryFlags.CanReceive);
        inventory.Constraint = this.BlockDefinition.InputInventoryConstraint;
        this.Components.Add<MyInventoryBase>((MyInventoryBase) inventory);
      }
      else
        inventory.Constraint = this.BlockDefinition.InputInventoryConstraint;
      this.ResetActualCheckFillValue(inventory);
      this.m_oreConstraint = new MyInventoryConstraint(inventory.Constraint.Description, inventory.Constraint.Icon, inventory.Constraint.IsWhitelist);
      this.m_containersConstraint = new MyInventoryConstraint(inventory.Constraint.Description, inventory.Constraint.Icon, inventory.Constraint.IsWhitelist);
      foreach (MyDefinitionId constrainedId in inventory.Constraint.ConstrainedIds)
      {
        if (constrainedId.TypeId == typeof (MyObjectBuilder_Ore))
          this.m_oreConstraint.Add(constrainedId);
        else if (constrainedId.TypeId == typeof (MyObjectBuilder_GasContainerObject) || constrainedId.TypeId == typeof (MyObjectBuilder_OxygenContainerObject))
          this.m_containersConstraint.Add(constrainedId);
      }
      if (MyFakes.ENABLE_INVENTORY_FIX)
        this.FixSingleInventory();
      this.AutoRefill = builderOxygenGenerator.AutoRefill;
      this.SourceComp.Enabled = this.Enabled;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.SourceComp.OutputChanged += new MyResourceOutputChangedDelegate(this.Source_OutputChanged);
      this.ResourceSink.Init(this.BlockDefinition.ResourceSinkGroup, new MyResourceSinkInfo()
      {
        ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
        MaxRequiredInput = this.BlockDefinition.OperationalPowerConsumption,
        RequiredInputFunc = new Func<float>(this.ComputeRequiredPower)
      });
      this.ResourceSink.IsPoweredChanged += new Action(this.PowerReceiver_IsPoweredChanged);
      inventory?.Init(builderOxygenGenerator.Inventory);
      this.ResourceSink.Update();
      this.SetDetailedInfoDirty();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyGasGenerator_IsWorkingChanged);
      this.CreateUpdateTimer(this.GetTimerTime(0), MyTimerTypes.Frame10);
      this.m_iceAmount = this.IceAmount();
    }

    private void ResetActualCheckFillValue(MyInventory inventory)
    {
      if ((double) this.BlockDefinition.InventoryFillFactorMin > (double) inventory.VolumeFillFactor)
        this.m_actualCheckFillValue = this.BlockDefinition.InventoryFillFactorMax;
      else
        this.m_actualCheckFillValue = this.BlockDefinition.InventoryFillFactorMin;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.CubeGrid == null || !Sandbox.Game.Multiplayer.Sync.IsServer || MySession.Static.SimplifiedSimulation)
        return;
      this.CubeGrid.OnAnyBlockInventoryChanged += new Action<MyInventoryBase>(this.OnAnyBlockInventoryChanged);
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      if (this.CubeGrid == null || !Sandbox.Game.Multiplayer.Sync.IsServer || MySession.Static.SimplifiedSimulation)
        return;
      this.CubeGrid.OnAnyBlockInventoryChanged -= new Action<MyInventoryBase>(this.OnAnyBlockInventoryChanged);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_OxygenGenerator builderCubeBlock = (MyObjectBuilder_OxygenGenerator) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.UseConveyorSystem = (bool) this.m_useConveyorSystem;
      builderCubeBlock.AutoRefill = this.AutoRefill;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public void RefillBottles()
    {
      List<MyPhysicalInventoryItem> items = MyEntityExtensions.GetInventory(this).GetItems();
      foreach (MyDefinitionId resourceType in this.SourceComp.ResourceTypes)
      {
        MyDefinitionId myDefinitionId = resourceType;
        double val2 = 0.0;
        if (MySession.Static.CreativeMode)
        {
          val2 = 3.40282346638529E+38;
        }
        else
        {
          foreach (MyPhysicalInventoryItem physicalInventoryItem in items)
          {
            if (!(physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject))
              val2 += this.IceToGas(ref myDefinitionId, (double) (float) physicalInventoryItem.Amount) * (double) ((Sandbox.ModAPI.IMyGasGenerator) this).ProductionCapacityMultiplier;
          }
        }
        double gasAmount = 0.0;
        foreach (MyPhysicalInventoryItem physicalInventoryItem in items)
        {
          if (val2 <= 0.0)
            return;
          if (physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject content && (double) content.GasLevel < 1.0)
          {
            MyOxygenContainerDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) content) as MyOxygenContainerDefinition;
            if (!(physicalItemDefinition.StoredGasId != resourceType))
            {
              float num1 = content.GasLevel * physicalItemDefinition.Capacity;
              double num2 = Math.Min((double) physicalItemDefinition.Capacity - (double) num1, val2);
              content.GasLevel = (float) Math.Min(((double) num1 + num2) / (double) physicalItemDefinition.Capacity, 1.0);
              gasAmount += num2;
              val2 -= num2;
            }
          }
        }
        if (gasAmount > 0.0)
          this.ConsumeFuel(ref myDefinitionId, gasAmount);
      }
    }

    private static void OnRefillButtonPressed(MyGasGenerator generator)
    {
      if (!generator.IsWorking)
        return;
      generator.SendRefillRequest();
    }

    private bool CanRefill()
    {
      if (!this.CanProduce)
        return false;
      bool flag1 = false;
      bool flag2 = false;
      foreach (MyPhysicalInventoryItem physicalInventoryItem in MyEntityExtensions.GetInventory(this).GetItems())
      {
        if (physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject content)
        {
          if ((double) content.GasLevel < 1.0)
            flag1 = true;
        }
        else if (physicalInventoryItem.Content is MyObjectBuilder_Ore)
          flag2 = true;
      }
      return flag1 & flag2;
    }

    public void InitializeConveyorEndpoint() => this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.SetRemainingCapacities();
      this.ResourceSink.Update();
      this.SetEmissiveStateWorking();
      if (MyFakes.ENABLE_OXYGEN_SOUNDS)
        this.UpdateSounds();
      this.CheckProducigState();
      if (this.m_isProducing || this.HasDamageEffect)
        return;
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
    }

    private void CheckProducigState()
    {
      this.m_isProducing = false;
      foreach (MyDefinitionId resourceType in this.SourceComp.ResourceTypes)
        this.m_isProducing |= (double) this.SourceComp.CurrentOutputByType(resourceType) > 0.0;
    }

    private void UpdateSounds()
    {
      if (this.m_soundEmitter == null || Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      if (this.IsWorking)
      {
        if (this.m_isProducing)
        {
          if (this.m_soundEmitter.SoundId != this.BlockDefinition.GenerateSound.Arcade && this.m_soundEmitter.SoundId != this.BlockDefinition.GenerateSound.Realistic)
            this.m_soundEmitter.PlaySound(this.BlockDefinition.GenerateSound, true);
        }
        else if (this.m_soundEmitter.SoundId != this.BlockDefinition.IdleSound.Arcade && this.m_soundEmitter.SoundId != this.BlockDefinition.IdleSound.Realistic && (this.m_soundEmitter.SoundId == this.BlockDefinition.GenerateSound.Arcade || this.m_soundEmitter.SoundId == this.BlockDefinition.GenerateSound.Realistic) && this.m_soundEmitter.Loop)
          this.m_soundEmitter.StopSound(false);
        if (this.m_soundEmitter.IsPlaying)
          return;
        this.m_soundEmitter.PlaySound(this.BlockDefinition.IdleSound, true);
      }
      else
      {
        if (!this.m_soundEmitter.IsPlaying)
          return;
        this.m_soundEmitter.StopSound(false);
      }
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (Sandbox.Game.Multiplayer.Sync.IsServer && !MySession.Static.SimplifiedSimulation && (!this.Enabled || !this.IsWorking ? 0 : (!MySession.Static.SimplifiedSimulation ? 1 : 0)) != 0)
      {
        if ((bool) this.m_useConveyorSystem)
        {
          this.GetIceFromConveyorSystem();
          this.GetBottlesFromConveyorSystem();
        }
        if (this.AutoRefill && this.CanRefill())
          this.RefillBottles();
      }
      if (!this.HasDamageEffect)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    private void GetBottlesFromConveyorSystem()
    {
      if (this.m_numberOfPullsForBottles == 0)
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if ((double) inventory.VolumeFillFactor >= 0.600000023841858 || !(this.CubeGrid.GridSystems.ConveyorSystem.PullItems(this.m_containersConstraint, (MyFixedPoint) 2, (IMyConveyorEndpointBlock) this, inventory) == (MyFixedPoint) 0))
        return;
      --this.m_numberOfPullsForBottles;
    }

    private void OnAnyBlockInventoryChanged(MyInventoryBase inv)
    {
      if (inv == null || inv.Entity == this)
        return;
      this.m_numberOfPullsForBottles = MyGasGenerator.NUMBER_PULLS_BOTTLES;
    }

    protected override bool CheckIsWorking() => base.CheckIsWorking() && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

    private float ComputeRequiredPower()
    {
      if (!MySession.Static.Settings.EnableOxygen && this.BlockDefinition.IsOxygenOnly || (!this.Enabled || !this.IsFunctional))
        return 0.0f;
      bool flag = false;
      foreach (MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo producedGase in this.BlockDefinition.ProducedGases)
        flag = flag || (double) this.SourceComp.CurrentOutputByType(producedGase.Id) > 0.0 && (MySession.Static.Settings.EnableOxygen || producedGase.Id != MyOxygenGeneratorDefinition.OxygenGasId);
      return (flag ? this.BlockDefinition.OperationalPowerConsumption : this.BlockDefinition.StandbyPowerConsumption) * this.m_powerConsumptionMultiplier;
    }

    private void SetRemainingCapacities()
    {
      foreach (MyDefinitionId resourceType in this.SourceComp.ResourceTypes)
      {
        MyDefinitionId gasId = resourceType;
        this.m_sourceComp.SetRemainingCapacityByType(resourceType, (float) this.IceToGas(ref gasId, (double) this.m_iceAmount));
      }
      if (MySession.Static == null || MySession.Static.Settings.EnableOxygen)
        return;
      this.m_sourceComp.SetMaxOutputByType(MyOxygenGeneratorDefinition.OxygenGasId, 0.0f);
    }

    private float IceAmount()
    {
      if (MySession.Static.CreativeMode)
        return 10000f;
      List<MyPhysicalInventoryItem> items = MyEntityExtensions.GetInventory(this).GetItems();
      MyFixedPoint myFixedPoint = (MyFixedPoint) 0;
      foreach (MyPhysicalInventoryItem physicalInventoryItem in items)
      {
        if (!(physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject))
          myFixedPoint += physicalInventoryItem.Amount;
      }
      return (float) myFixedPoint;
    }

    private void Inventory_ContentsChanged(MyInventoryBase obj)
    {
      this.m_iceAmount = this.IceAmount();
      this.SetRemainingCapacities();
      this.RaisePropertiesChanged();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    private void MyGasGenerator_IsWorkingChanged(MyCubeBlock obj) => MySandboxGame.Static.Invoke((Action) (() =>
    {
      if (this.Closed)
        return;
      this.SourceComp.Enabled = this.CanProduce;
      this.SetEmissiveStateWorking();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }), nameof (MyGasGenerator_IsWorkingChanged));

    private void PowerReceiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    private void ComponentStack_IsFunctionalChanged()
    {
      this.SourceComp.Enabled = this.CanProduce;
      this.ResourceSink.Update();
      if (this.CubeGrid.GridSystems.ResourceDistributor != null)
        this.CubeGrid.GridSystems.ResourceDistributor.ConveyorSystem_OnPoweredChanged();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.SourceComp.Enabled = this.CanProduce;
      this.ResourceSink.Update();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    private void Source_OutputChanged(
      MyDefinitionId changedResourceId,
      float oldOutput,
      MyResourceSourceComponent source)
    {
      if (this.BlockDefinition.ProducedGases.TrueForAll((Predicate<MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo>) (info => info.Id != changedResourceId)))
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    protected override void Closing()
    {
      base.Closing();
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.StopSound(true);
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.SetEmissiveStateWorking();
    }

    public override bool SetEmissiveStateWorking()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || !this.CanProduce)
        return false;
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory == null)
        return false;
      if (!inventory.FindItem((Func<MyPhysicalInventoryItem, bool>) (item => !(item.Content is MyObjectBuilder_GasContainerObject))).HasValue)
        return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, this.Render.RenderObjectIDs[0]);
      return this.m_isProducing ? this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, this.Render.RenderObjectIDs[0]) : this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]);
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
      if (MySession.Static.Settings.EnableOxygen)
        return;
      detailedInfo.Append("\n");
      detailedInfo.Append("Oxygen disabled in world settings!");
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.CheckEmissiveState(true);
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

    public void SetInventory(MyInventory inventory, int index) => throw new NotImplementedException("TODO Dusan inventory sync");

    protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
    {
      base.OnInventoryComponentAdded(inventory);
      if (!(inventory is MyInventory myInventory))
        return;
      myInventory.ContentsChanged += new Action<MyInventoryBase>(this.Inventory_ContentsChanged);
    }

    protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
    {
      base.OnInventoryComponentRemoved(inventory);
      if (!(inventory is MyInventory myInventory))
        return;
      myInventory.ContentsChanged -= new Action<MyInventoryBase>(this.Inventory_ContentsChanged);
    }

    bool IMyGasBlock.IsWorking() => this.CanProduce;

    public override bool GetTimerEnabledState() => this.Enabled && this.IsWorking && this.m_isProducing && !MySession.Static.SimplifiedSimulation;

    public override void DoUpdateTimerTick()
    {
      base.DoUpdateTimerTick();
      if (MySession.Static.CreativeMode || !this.IsWorking)
        return;
      foreach (MyDefinitionId resourceType in this.SourceComp.ResourceTypes)
      {
        uint framesFromLastTrigger = this.GetFramesFromLastTrigger();
        double gasAmount = this.GasOutputPerSecond(ref resourceType) * ((double) framesFromLastTrigger / 60.0);
        this.ConsumeFuel(ref resourceType, gasAmount);
      }
    }

    private void GetIceFromConveyorSystem()
    {
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory == null)
        return;
      if ((double) this.m_actualCheckFillValue > (double) inventory.VolumeFillFactor)
      {
        foreach (MyDefinitionId constrainedId in this.m_oreConstraint.ConstrainedIds)
          this.CubeGrid.GridSystems.ConveyorSystem.PullItem(constrainedId, new MyFixedPoint?((MyFixedPoint) (this.BlockDefinition.IceConsumptionPerSecond * 60f * this.BlockDefinition.FuelPullAmountFromConveyorInMinutes)), (IMyConveyorEndpointBlock) this, inventory, false, false);
        if ((double) this.m_actualCheckFillValue != (double) this.BlockDefinition.InventoryFillFactorMin)
          return;
        this.m_actualCheckFillValue = this.BlockDefinition.InventoryFillFactorMax;
      }
      else
      {
        if ((double) this.m_actualCheckFillValue != (double) this.BlockDefinition.InventoryFillFactorMax)
          return;
        this.m_actualCheckFillValue = this.BlockDefinition.InventoryFillFactorMin;
      }
    }

    private void ConsumeFuel(ref MyDefinitionId gasTypeId, double gasAmount)
    {
      if (gasAmount <= 0.0 || !Sandbox.Game.Multiplayer.Sync.IsServer || MySession.Static.CreativeMode)
        return;
      double ice = this.GasToIce(ref gasTypeId, gasAmount);
      if (ice <= 0.0)
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory == null)
        return;
      List<MyPhysicalInventoryItem> items = inventory.GetItems();
      if (items.Count <= 0)
        return;
      int index = 0;
      while (index < items.Count)
      {
        MyPhysicalInventoryItem physicalInventoryItem = items[index];
        if (physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject)
        {
          ++index;
        }
        else
        {
          if (ice < (double) (float) physicalInventoryItem.Amount)
          {
            MyFixedPoint myFixedPoint = MyFixedPoint.Max((MyFixedPoint) ice, MyFixedPoint.SmallestPossibleValue);
            inventory.RemoveItems(physicalInventoryItem.ItemId, new MyFixedPoint?(myFixedPoint), true, false, new MatrixD?(), (Action<MyDefinitionId, MyEntity>) null);
            break;
          }
          ice -= (double) (float) physicalInventoryItem.Amount;
          inventory.RemoveItems(physicalInventoryItem.ItemId, new MyFixedPoint?(), true, false, new MatrixD?(), (Action<MyDefinitionId, MyEntity>) null);
        }
      }
    }

    private double GasOutputPerSecond(ref MyDefinitionId gasId) => (double) this.SourceComp.CurrentOutputByType(gasId) * (double) ((Sandbox.ModAPI.IMyGasGenerator) this).ProductionCapacityMultiplier;

    private double GasOutputPerUpdate(ref MyDefinitionId gasId) => this.GasOutputPerSecond(ref gasId) * 0.0166666675359011;

    private double IceToGas(ref MyDefinitionId gasId, double iceAmount) => iceAmount * this.IceToGasRatio(ref gasId);

    private double GasToIce(ref MyDefinitionId gasId, double gasAmount) => gasAmount / this.IceToGasRatio(ref gasId);

    private double IceToGasRatio(ref MyDefinitionId gasId) => (double) this.SourceComp.DefinedOutputByType(gasId) / (double) this.BlockDefinition.IceConsumptionPerSecond;

    public void SendRefillRequest() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyGasGenerator>(this, (Func<MyGasGenerator, Action>) (x => new Action(x.OnRefillCallback)));

    [Event(null, 887)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnRefillCallback() => this.RefillBottles();

    float Sandbox.ModAPI.IMyGasGenerator.ProductionCapacityMultiplier
    {
      get => this.m_productionCapacityMultiplier;
      set
      {
        this.m_productionCapacityMultiplier = value;
        if ((double) this.m_productionCapacityMultiplier >= 0.00999999977648258)
          return;
        this.m_productionCapacityMultiplier = 0.01f;
      }
    }

    float Sandbox.ModAPI.IMyGasGenerator.PowerConsumptionMultiplier
    {
      get => this.m_powerConsumptionMultiplier;
      set
      {
        this.m_powerConsumptionMultiplier = value;
        if ((double) this.m_powerConsumptionMultiplier < 0.00999999977648258)
          this.m_powerConsumptionMultiplier = 0.01f;
        if (this.ResourceSink == null)
          return;
        this.ResourceSink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, this.BlockDefinition.OperationalPowerConsumption * this.m_powerConsumptionMultiplier);
        this.ResourceSink.Update();
      }
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

    public PullInformation GetPullInformation()
    {
      PullInformation pullInformation = new PullInformation()
      {
        Inventory = MyEntityExtensions.GetInventory(this),
        OwnerID = this.OwnerId
      };
      pullInformation.Constraint = pullInformation.Inventory.Constraint;
      return pullInformation;
    }

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    protected sealed class OnRefillCallback\u003C\u003E : ICallSite<MyGasGenerator, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyGasGenerator @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRefillCallback();
      }
    }

    protected class m_useConveyorSystem\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyGasGenerator) obj0).m_useConveyorSystem = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_autoRefill\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyGasGenerator) obj0).m_autoRefill = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyGasGenerator\u003C\u003EActor : IActivator, IActivator<MyGasGenerator>
    {
      object IActivator.CreateInstance() => (object) new MyGasGenerator();

      MyGasGenerator IActivator<MyGasGenerator>.CreateInstance() => new MyGasGenerator();
    }
  }
}
