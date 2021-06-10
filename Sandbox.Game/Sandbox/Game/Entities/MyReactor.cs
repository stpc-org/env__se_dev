// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyReactor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Reactor))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyReactor), typeof (Sandbox.ModAPI.Ingame.IMyReactor)})]
  public class MyReactor : MyFueledPowerProducer, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyReactor, Sandbox.ModAPI.Ingame.IMyReactor, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, IMyInventoryOwner
  {
    private readonly uint TIMER_NORMAL_IN_FRAMES = 900;
    private readonly uint TIMER_TIER1_IN_FRAMES = 1800;
    private readonly uint TIMER_TIER2_IN_FRAMES = 3600;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;
    private float m_powerOutputMultiplier = 1f;
    private float m_actualCheckFillValue;

    public MyReactorDefinition BlockDefinition => (MyReactorDefinition) base.BlockDefinition;

    public override float MaxOutput => base.MaxOutput * this.m_powerOutputMultiplier;

    public bool UseConveyorSystem
    {
      get => (bool) this.m_useConveyorSystem;
      set => this.m_useConveyorSystem.Value = value;
    }

    public override bool IsTieredUpdateSupported => true;

    public MyReactor() => this.CreateTerminalControls();

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyReactor>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MyReactor> onOff = new MyTerminalControlOnOffSwitch<MyReactor>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem);
      onOff.Getter = (MyTerminalValueControl<MyReactor, bool>.GetterDelegate) (x => x.UseConveyorSystem);
      onOff.Setter = (MyTerminalValueControl<MyReactor, bool>.SetterDelegate) ((x, v) => x.UseConveyorSystem = v);
      onOff.EnableToggleAction<MyReactor>();
      MyTerminalControlFactory.AddControl<MyReactor>((MyTerminalControl<MyReactor>) onOff);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_Reactor objectBuilderReactor = (MyObjectBuilder_Reactor) objectBuilder;
      if (MyFakes.ENABLE_INVENTORY_FIX)
        this.FixSingleInventory();
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory == null)
      {
        inventory = new MyInventory(this.BlockDefinition.InventoryMaxVolume, this.BlockDefinition.InventorySize, MyInventoryFlags.CanReceive);
        this.Components.Add<MyInventoryBase>((MyInventoryBase) inventory);
        inventory.Init(objectBuilderReactor.Inventory);
      }
      this.ResetActualCheckFillValue(inventory);
      inventory.Constraint = this.BlockDefinition.InventoryConstraint;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.RefreshRemainingCapacity();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_useConveyorSystem.SetLocalValue(objectBuilderReactor.UseConveyorSystem);
      this.CreateUpdateTimer(this.GetTimerTime(0), MyTimerTypes.Frame100, true);
    }

    private void ResetActualCheckFillValue(MyInventory inventory)
    {
      if ((double) this.BlockDefinition.InventoryFillFactorMin > (double) inventory.VolumeFillFactor)
        this.m_actualCheckFillValue = this.BlockDefinition.InventoryFillFactorMax;
      else
        this.m_actualCheckFillValue = this.BlockDefinition.InventoryFillFactorMin;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_Reactor builderCubeBlock = (MyObjectBuilder_Reactor) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.UseConveyorSystem = (bool) this.m_useConveyorSystem;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || MySession.Static.SimplifiedSimulation)
        return;
      this.GetFuelFromConveyorSystem();
    }

    public override bool GetTimerEnabledState() => this.IsWorking && this.Enabled && !MySession.Static.CreativeMode && !MySession.Static.SimplifiedSimulation;

    private void GetFuelFromConveyorSystem()
    {
      if (!(bool) this.m_useConveyorSystem || !this.IsFunctional || !this.Enabled)
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory == null)
        return;
      if ((double) this.m_actualCheckFillValue > (double) inventory.VolumeFillFactor)
      {
        foreach (MyReactorDefinition.FuelInfo fuelInfo in this.BlockDefinition.FuelInfos)
        {
          float num = fuelInfo.ConsumptionPerSecond_Items * 60f * this.BlockDefinition.FuelPullAmountFromConveyorInMinutes;
          this.CubeGrid.GridSystems.ConveyorSystem.PullItem(fuelInfo.FuelId, new MyFixedPoint?((MyFixedPoint) num), (IMyConveyorEndpointBlock) this, inventory, false, false);
        }
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

    public override void DoUpdateTimerTick()
    {
      base.DoUpdateTimerTick();
      if (!this.IsWorking)
        return;
      this.ConsumeFuel((float) Math.Round((double) this.GetFramesFromLastTrigger() * 16.6666660308838));
    }

    private void ConsumeFuel(float timeDeltaMilliseconds)
    {
      if (!this.SourceComp.HasCapacityRemaining)
        return;
      float currentOutput = this.SourceComp.CurrentOutput;
      if ((double) currentOutput == 0.0)
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      float num1 = currentOutput / this.BlockDefinition.MaxPowerOutput;
      foreach (MyReactorDefinition.FuelInfo fuelInfo in this.BlockDefinition.FuelInfos)
      {
        float num2 = (float) ((double) num1 * (double) fuelInfo.ConsumptionPerSecond_Items / 1000.0);
        MyFixedPoint b = (MyFixedPoint) (timeDeltaMilliseconds * num2);
        if (b == (MyFixedPoint) 0)
          b = MyFixedPoint.SmallestPossibleValue;
        MyFixedPoint amount = MyFixedPoint.Min(inventory.GetItemAmount(fuelInfo.FuelId, MyItemFlags.None, false), b);
        inventory.RemoveItemsOfType(amount, fuelInfo.FuelId, MyItemFlags.None, false);
        if (MyFakes.ENABLE_INFINITE_REACTOR_FUEL && !inventory.ContainItems(new MyFixedPoint?(b), fuelInfo.FuelId))
          inventory.AddItems(50 * b, (MyObjectBuilder_Base) fuelInfo.FuelItem);
      }
    }

    public override void OnDestroy()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this), true);
      base.OnDestroy();
    }

    public override void OnRemovedByCubeBuilder()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnRemovedByCubeBuilder();
    }

    private void RefreshRemainingCapacity()
    {
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory == null || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      float val1 = float.MaxValue;
      foreach (MyReactorDefinition.FuelInfo fuelInfo in this.BlockDefinition.FuelInfos)
      {
        float val2 = (float) inventory.GetItemAmount(fuelInfo.FuelId, MyItemFlags.None, false) / fuelInfo.Ratio;
        val1 = Math.Min(val1, val2);
      }
      if ((double) val1 == 0.0 && MySession.Static.CreativeMode)
      {
        MyReactorDefinition.FuelInfo fuelInfo = this.BlockDefinition.FuelInfos[0];
        val1 = fuelInfo.FuelDefinition.Mass / fuelInfo.Ratio;
      }
      this.Capacity = val1;
    }

    protected override void OnCurrentOrMaxOutputChanged(
      MyDefinitionId resourceTypeId,
      float oldOutput,
      MyResourceSourceComponent source)
    {
      base.OnCurrentOrMaxOutputChanged(resourceTypeId, oldOutput, source);
      if (this.SoundEmitter == null || this.SoundEmitter.Sound == null || !this.SoundEmitter.Sound.IsPlaying)
        return;
      if ((double) this.SourceComp.MaxOutput != 0.0)
        this.SoundEmitter.Sound.FrequencyRatio = MyAudio.Static.SemitonesToFrequencyRatio((float) (4.0 * ((double) this.SourceComp.CurrentOutput - 0.5 * (double) this.SourceComp.MaxOutput)) / this.SourceComp.MaxOutput);
      else
        this.SoundEmitter.Sound.FrequencyRatio = 1f;
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.CheckEmissiveState(true);
    }

    public override void CheckEmissiveState(bool force = false)
    {
      if (this.IsWorking)
        this.SetEmissiveStateWorking();
      else if (this.IsFunctional)
      {
        if (this.Enabled)
          this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, this.Render.RenderObjectIDs[0], "Emissive");
        else
          this.SetEmissiveStateDisabled();
      }
      else
        this.SetEmissiveStateDamaged();
    }

    protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
    {
      base.OnInventoryComponentAdded(inventory);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || MyEntityExtensions.GetInventory(this) == null)
        return;
      MyEntityExtensions.GetInventory(this).ContentsChanged += new Action<MyInventoryBase>(this.OnInventoryContentChanged);
    }

    protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
    {
      base.OnInventoryComponentRemoved(inventory);
      MyInventory myInventory = inventory as MyInventory;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || myInventory == null)
        return;
      myInventory.ContentsChanged -= new Action<MyInventoryBase>(this.OnInventoryContentChanged);
    }

    private void OnInventoryContentChanged(MyInventoryBase obj)
    {
      int num1 = this.IsWorking ? 1 : 0;
      this.RefreshRemainingCapacity();
      bool isWorking = this.IsWorking;
      int num2 = isWorking ? 1 : 0;
      if (num1 == num2)
        return;
      if (isWorking)
        this.OnStartWorking();
      else
        this.OnStopWorking();
    }

    bool Sandbox.ModAPI.Ingame.IMyReactor.UseConveyorSystem
    {
      get => this.UseConveyorSystem;
      set => this.UseConveyorSystem = value;
    }

    float Sandbox.ModAPI.IMyReactor.PowerOutputMultiplier
    {
      get => this.m_powerOutputMultiplier;
      set
      {
        this.m_powerOutputMultiplier = value;
        if ((double) this.m_powerOutputMultiplier < 0.00999999977648258)
          this.m_powerOutputMultiplier = 0.01f;
        this.OnProductionChanged();
      }
    }

    VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(
      int index)
    {
      return (VRage.Game.ModAPI.Ingame.IMyInventory) MyEntityExtensions.GetInventory(this, index);
    }

    public override PullInformation GetPullInformation() => new PullInformation()
    {
      OwnerID = this.OwnerId,
      Inventory = MyEntityExtensions.GetInventory(this),
      Constraint = this.BlockDefinition.InventoryConstraint
    };

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

    [SpecialName]
    int IMyInventoryOwner.get_InventoryCount() => this.InventoryCount;

    [SpecialName]
    bool IMyInventoryOwner.get_HasInventory() => this.HasInventory;

    protected class m_useConveyorSystem\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyReactor) obj0).m_useConveyorSystem = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyReactor\u003C\u003EActor : IActivator, IActivator<MyReactor>
    {
      object IActivator.CreateInstance() => (object) new MyReactor();

      MyReactor IActivator<MyReactor>.CreateInstance() => new MyReactor();
    }
  }
}
