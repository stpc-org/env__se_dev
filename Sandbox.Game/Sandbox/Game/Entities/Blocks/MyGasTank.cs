// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyGasTank
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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_OxygenTank))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyGasTank), typeof (Sandbox.ModAPI.Ingame.IMyGasTank), typeof (Sandbox.ModAPI.IMyOxygenTank), typeof (Sandbox.ModAPI.Ingame.IMyOxygenTank)})]
  public class MyGasTank : MyFunctionalBlock, IMyGasBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyOxygenTank, Sandbox.ModAPI.IMyGasTank, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyGasTank, Sandbox.ModAPI.Ingame.IMyOxygenTank, IMyInventoryOwner, Sandbox.Game.Entities.Interfaces.IMyGasTank
  {
    private readonly uint TIMER_NORMAL_IN_FRAMES = 100;
    private readonly uint TIMER_TIER1_IN_FRAMES = 300;
    private readonly uint TIMER_TIER2_IN_FRAMES = 600;
    private static readonly string[] m_emissiveTextureNames = new string[4]
    {
      "Emissive0",
      "Emissive1",
      "Emissive2",
      "Emissive3"
    };
    private Color m_prevColor = Color.White;
    private int m_prevFillCount = -1;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_autoRefill;
    private const float m_maxFillPerSecond = 0.05f;
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;
    private bool m_isStockpiling;
    private double m_FilledRatio;
    private MyResourceSourceComponent m_sourceComp;
    private readonly MyDefinitionId m_oxygenGasId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Oxygen");

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public bool IsStockpiling
    {
      get => this.m_isStockpiling;
      private set => this.SetStockpilingState(value);
    }

    bool Sandbox.ModAPI.Ingame.IMyGasTank.Stockpile
    {
      get => this.IsStockpiling;
      set => this.ChangeStockpileMode(value);
    }

    bool Sandbox.ModAPI.Ingame.IMyGasTank.AutoRefillBottles
    {
      get => (bool) this.m_autoRefill;
      set => this.m_autoRefill.Value = value;
    }

    public bool CanStore => (MySession.Static != null && MySession.Static.Settings.EnableOxygen || this.BlockDefinition.StoredGasId != this.m_oxygenGasId) && (this.IsWorking && this.Enabled) && this.IsFunctional;

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

    public MyGasTankDefinition BlockDefinition => (MyGasTankDefinition) base.BlockDefinition;

    private float GasOutputPerSecond => !this.SourceComp.ProductionEnabledByType(this.BlockDefinition.StoredGasId) ? 0.0f : this.SourceComp.CurrentOutputByType(this.BlockDefinition.StoredGasId);

    private float GasInputPerSecond => this.ResourceSink.CurrentInputByType(this.BlockDefinition.StoredGasId);

    private float GasOutputPerUpdate => this.GasOutputPerSecond * 0.01666667f;

    private float GasInputPerUpdate => this.GasInputPerSecond * 0.01666667f;

    public float Capacity => this.BlockDefinition.Capacity;

    public float GasCapacity => this.Capacity;

    public double FilledRatio
    {
      get => this.m_FilledRatio;
      private set
      {
        if (this.m_FilledRatio != value)
          this.FilledRatioChanged.InvokeIfNotNull();
        this.m_FilledRatio = value;
      }
    }

    public Action FilledRatioChanged { get; set; }

    public bool CanPressurizeRoom => false;

    public override bool IsTieredUpdateSupported => true;

    public MyGasTank()
    {
      this.CreateTerminalControls();
      this.SourceComp = new MyResourceSourceComponent();
      this.ResourceSink = new MyResourceSinkComponent(2);
      this.m_autoRefill.ValueChanged += (Action<SyncBase>) (x => this.OnAutoRefillChanged());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyGasTank>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MyGasTank> onOff = new MyTerminalControlOnOffSwitch<MyGasTank>("Stockpile", MySpaceTexts.BlockPropertyTitle_Stockpile, MySpaceTexts.BlockPropertyDescription_Stockpile);
      onOff.Getter = (MyTerminalValueControl<MyGasTank, bool>.GetterDelegate) (x => x.IsStockpiling);
      onOff.Setter = (MyTerminalValueControl<MyGasTank, bool>.SetterDelegate) ((x, v) => x.ChangeStockpileMode(v));
      onOff.EnableToggleAction<MyGasTank>();
      onOff.EnableOnOffActions<MyGasTank>();
      MyTerminalControlFactory.AddControl<MyGasTank>((MyTerminalControl<MyGasTank>) onOff);
      MyTerminalControlButton<MyGasTank> button = new MyTerminalControlButton<MyGasTank>("Refill", MySpaceTexts.BlockPropertyTitle_Refill, MySpaceTexts.BlockPropertyTitle_Refill, new Action<MyGasTank>(MyGasTank.OnRefillButtonPressed));
      button.Enabled = (Func<MyGasTank, bool>) (x => x.CanRefill());
      button.EnableAction<MyGasTank>();
      MyTerminalControlFactory.AddControl<MyGasTank>((MyTerminalControl<MyGasTank>) button);
      MyTerminalControlCheckbox<MyGasTank> checkbox = new MyTerminalControlCheckbox<MyGasTank>("Auto-Refill", MySpaceTexts.BlockPropertyTitle_AutoRefill, MySpaceTexts.BlockPropertyTitle_AutoRefill);
      checkbox.Getter = (MyTerminalValueControl<MyGasTank, bool>.GetterDelegate) (x => (bool) x.m_autoRefill);
      checkbox.Setter = (MyTerminalValueControl<MyGasTank, bool>.SetterDelegate) ((x, v) => x.m_autoRefill.Value = v);
      checkbox.EnableAction<MyGasTank>();
      MyTerminalControlFactory.AddControl<MyGasTank>((MyTerminalControl<MyGasTank>) checkbox);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_OxygenTank builderOxygenTank = (MyObjectBuilder_OxygenTank) objectBuilder;
      this.InitializeConveyorEndpoint();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      if (MyFakes.ENABLE_INVENTORY_FIX)
      {
        this.FixSingleInventory();
        if (MyEntityExtensions.GetInventory(this) != null)
          MyEntityExtensions.GetInventory(this).Constraint = this.BlockDefinition.InputInventoryConstraint;
      }
      MyInventory myInventory = MyEntityExtensions.GetInventory(this);
      if (myInventory == null)
      {
        myInventory = new MyInventory(this.BlockDefinition.InventoryMaxVolume, this.BlockDefinition.InventorySize, MyInventoryFlags.CanReceive);
        myInventory.Constraint = this.BlockDefinition.InputInventoryConstraint;
        this.Components.Add<MyInventoryBase>((MyInventoryBase) myInventory);
        myInventory.Init(builderOxygenTank.Inventory);
      }
      myInventory.ContentsChanged += new Action<MyInventoryBase>(this.MyGasTank_ContentsChanged);
      this.m_autoRefill.SetLocalValue(builderOxygenTank.AutoRefill);
      this.SourceComp.Init(this.BlockDefinition.ResourceSourceGroup, new List<MyResourceSourceInfo>()
      {
        new MyResourceSourceInfo()
        {
          ResourceTypeId = this.BlockDefinition.StoredGasId,
          DefinedOutput = 0.05f * this.BlockDefinition.Capacity
        }
      });
      this.SourceComp.OutputChanged += new MyResourceOutputChangedDelegate(this.Source_OutputChanged);
      this.SourceComp.Enabled = this.Enabled;
      this.IsStockpiling = builderOxygenTank.IsStockpiling;
      List<MyResourceSinkInfo> sinkData = new List<MyResourceSinkInfo>();
      MyResourceSinkInfo resourceSinkInfo = new MyResourceSinkInfo();
      resourceSinkInfo.ResourceTypeId = MyResourceDistributorComponent.ElectricityId;
      resourceSinkInfo.MaxRequiredInput = this.BlockDefinition.OperationalPowerConsumption;
      resourceSinkInfo.RequiredInputFunc = new Func<float>(this.ComputeRequiredPower);
      sinkData.Add(resourceSinkInfo);
      resourceSinkInfo = new MyResourceSinkInfo();
      resourceSinkInfo.ResourceTypeId = this.BlockDefinition.StoredGasId;
      resourceSinkInfo.MaxRequiredInput = this.Capacity;
      resourceSinkInfo.RequiredInputFunc = new Func<float>(this.ComputeRequiredGas);
      sinkData.Add(resourceSinkInfo);
      this.ResourceSink.Init(this.BlockDefinition.ResourceSinkGroup, sinkData);
      this.ResourceSink.IsPoweredChanged += new Action(this.PowerReceiver_IsPoweredChanged);
      this.ResourceSink.CurrentInputChanged += new MyCurrentResourceInputChangedDelegate(this.Sink_CurrentInputChanged);
      float num = builderOxygenTank.FilledRatio;
      if (MySession.Static.CreativeMode && (double) num == 0.0)
        num = 0.5f;
      this.ChangeFilledRatio((double) MathHelper.Clamp(num, 0.0f, 1f));
      this.ResourceSink.Update();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyOxygenTank_IsWorkingChanged);
    }

    private void MyGasTank_ContentsChanged(MyInventoryBase obj)
    {
      if (!(bool) this.m_autoRefill || !this.CanRefill())
        return;
      this.RefillBottles();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_OxygenTank builderCubeBlock = (MyObjectBuilder_OxygenTank) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.IsStockpiling = this.IsStockpiling;
      builderCubeBlock.FilledRatio = (float) this.FilledRatio;
      builderCubeBlock.AutoRefill = (bool) this.m_autoRefill;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public void RefillBottles()
    {
      List<MyPhysicalInventoryItem> items = MyEntityExtensions.GetInventory(this).GetItems();
      bool flag = false;
      double newFilledRatio = this.FilledRatio;
      foreach (MyPhysicalInventoryItem physicalInventoryItem in items)
      {
        if (newFilledRatio > 0.0)
        {
          if (physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject content && (double) content.GasLevel < 1.0)
          {
            MyOxygenContainerDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) content) as MyOxygenContainerDefinition;
            float num1 = content.GasLevel * physicalItemDefinition.Capacity;
            float val2 = (float) newFilledRatio * this.Capacity;
            float num2 = Math.Min(physicalItemDefinition.Capacity - num1, val2);
            content.GasLevel = Math.Min((num1 + num2) / physicalItemDefinition.Capacity, 1f);
            newFilledRatio = Math.Max(newFilledRatio - (double) num2 / (double) this.Capacity, 0.0);
            flag = true;
          }
        }
        else
          break;
      }
      if (!flag)
        return;
      this.ChangeFilledRatio(newFilledRatio, Sandbox.Game.Multiplayer.Sync.IsServer);
    }

    private static void OnRefillButtonPressed(MyGasTank tank)
    {
      if (!tank.IsWorking)
        return;
      tank.SendRefillRequest();
    }

    void Sandbox.ModAPI.Ingame.IMyGasTank.RefillBottles()
    {
      if (!this.IsWorking)
        return;
      this.SendRefillRequest();
    }

    private bool CanRefill()
    {
      if (!this.CanStore || !this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) || this.FilledRatio == 0.0)
        return false;
      foreach (MyPhysicalInventoryItem physicalInventoryItem in MyEntityExtensions.GetInventory(this).GetItems())
      {
        if (physicalInventoryItem.Content is MyObjectBuilder_GasContainerObject content && (double) content.GasLevel < 1.0)
          return true;
      }
      return false;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || MySession.Static.SimplifiedSimulation)
        return;
      this.ExecuteGasTransfer((double) this.GasInputPerUpdate - (double) this.GasOutputPerUpdate);
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || MySession.Static.SimplifiedSimulation || (!this.GetTimerEnabledState() || !(bool) this.m_autoRefill) || !this.CanRefill())
        return;
      this.RefillBottles();
    }

    public override bool GetTimerEnabledState() => this.Enabled && this.IsWorking && !MySession.Static.SimplifiedSimulation;

    private void ExecuteGasTransfer(double totalTransfer)
    {
      if (totalTransfer != 0.0)
      {
        this.Transfer(totalTransfer);
        this.ResourceSink.Update();
        this.SourceComp.OnProductionEnabledChanged(new MyDefinitionId?(this.BlockDefinition.StoredGasId));
      }
      else
      {
        if (this.HasDamageEffect)
          return;
        this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
      }
    }

    public override void DoUpdateTimerTick()
    {
      base.DoUpdateTimerTick();
      this.ExecuteGasTransfer(((double) this.GasInputPerSecond - (double) this.GasOutputPerSecond) * ((double) this.GetFramesFromLastTrigger() / 60.0));
    }

    protected override bool CheckIsWorking() => base.CheckIsWorking() && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

    private float ComputeRequiredPower()
    {
      if (!MySession.Static.Settings.EnableOxygen && this.BlockDefinition.StoredGasId == this.m_oxygenGasId || (!this.Enabled || !this.IsFunctional))
        return 0.0f;
      return (double) this.SourceComp.CurrentOutputByType(this.BlockDefinition.StoredGasId) <= 0.0 && (double) this.ResourceSink.CurrentInputByType(this.BlockDefinition.StoredGasId) <= 0.0 ? this.BlockDefinition.StandbyPowerConsumption : this.BlockDefinition.OperationalPowerConsumption;
    }

    private float ComputeRequiredGas() => !this.CanStore ? 0.0f : Math.Min((float) ((1.0 - this.FilledRatio) * 60.0) * this.SourceComp.ProductionToCapacityMultiplierByType(this.BlockDefinition.StoredGasId) * this.Capacity + this.SourceComp.CurrentOutputByType(this.BlockDefinition.StoredGasId), 0.05f * this.Capacity);

    private void m_inventory_ContentsChanged(MyInventoryBase obj) => this.RaisePropertiesChanged();

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.m_prevColor = Color.White;
      this.m_prevFillCount = -1;
      this.UpdateEmissivity();
      this.SetDetailedInfoDirty();
    }

    private void PowerReceiver_IsPoweredChanged() => MySandboxGame.Static.Invoke((Action) (() =>
    {
      if (this.Closed)
        return;
      this.UpdateIsWorking();
      this.UpdateEmissivity();
    }), "MyGasTank::PowerReceiver_IsPoweredChanged");

    private void ComponentStack_IsFunctionalChanged()
    {
      this.SourceComp.Enabled = this.CanStore;
      this.ResourceSink.Update();
      this.FilledRatio = 0.0;
      if (MySession.Static.CreativeMode)
        this.SourceComp.SetRemainingCapacityByType(this.BlockDefinition.StoredGasId, this.Capacity);
      else
        this.SourceComp.SetRemainingCapacityByType(this.BlockDefinition.StoredGasId, (float) this.FilledRatio * this.Capacity);
      if (this.CubeGrid != null && this.CubeGrid.GridSystems != null && this.CubeGrid.GridSystems.ResourceDistributor != null)
        this.CubeGrid.GridSystems.ResourceDistributor.ConveyorSystem_OnPoweredChanged();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      this.UpdateEmissivity();
    }

    private void MyOxygenTank_IsWorkingChanged(MyCubeBlock obj)
    {
      this.SourceComp.Enabled = this.CanStore;
      this.SetStockpilingState(this.m_isStockpiling);
      this.UpdateEmissivity();
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.SourceComp.Enabled = this.CanStore;
      this.ResourceSink.Update();
      this.UpdateEmissivity();
    }

    private void Source_OutputChanged(
      MyDefinitionId changedResourceId,
      float oldOutput,
      MyResourceSourceComponent source)
    {
      if (changedResourceId != this.BlockDefinition.StoredGasId || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    private void Sink_CurrentInputChanged(
      MyDefinitionId resourceTypeId,
      float oldInput,
      MyResourceSinkComponent sink)
    {
      if (resourceTypeId != this.BlockDefinition.StoredGasId || !Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateEmissivity();
    }

    public override bool SetEmissiveStateWorking() => false;

    public override bool SetEmissiveStateDamaged() => false;

    public override bool SetEmissiveStateDisabled() => false;

    private void UpdateEmissivity()
    {
      Color color = Color.Red;
      bool flag = true;
      if (this.CanStore)
      {
        if (this.IsStockpiling)
        {
          color = Color.Teal;
          flag = false;
          MyEmissiveColorStateResult result;
          if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Alternative, out result))
            color = result.EmissiveColor;
        }
        else if (this.FilledRatio <= 9.99999974737875E-06)
        {
          color = Color.Yellow;
          MyEmissiveColorStateResult result;
          if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result))
            color = result.EmissiveColor;
        }
        else
        {
          color = Color.Green;
          MyEmissiveColorStateResult result;
          if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
            color = result.EmissiveColor;
        }
      }
      else if (this.IsFunctional)
      {
        flag = false;
        MyEmissiveColorStateResult result;
        if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
          color = result.EmissiveColor;
      }
      else
      {
        MyEmissiveColorStateResult result;
        if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
          color = result.EmissiveColor;
      }
      this.SetEmissive(color, flag ? (float) this.FilledRatio : 1f);
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
      detailedInfo.Append("\n");
      if (!MySession.Static.Settings.EnableOxygen && !(this.BlockDefinition.StoredGasId != this.m_oxygenGasId))
        detailedInfo.Append((object) MyTexts.Get(MySpaceTexts.Oxygen_Disabled));
      else
        detailedInfo.Append(string.Format(MyTexts.GetString(MySpaceTexts.Oxygen_Filled), (object) (this.FilledRatio * 100.0).ToString("F1"), (object) (int) (this.FilledRatio * (double) this.Capacity), (object) this.Capacity));
    }

    private void SetEmissive(Color color, float fill)
    {
      int num = (int) ((double) fill * (double) MyGasTank.m_emissiveTextureNames.Length);
      if (this.Render.RenderObjectIDs[0] == uint.MaxValue || color == this.m_prevColor && num == this.m_prevFillCount)
        return;
      for (int index = 0; index < MyGasTank.m_emissiveTextureNames.Length; ++index)
        MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyGasTank.m_emissiveTextureNames[index], index < num ? color : Color.Black, 1f);
      this.m_prevColor = color;
      this.m_prevFillCount = num;
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.m_prevFillCount = -1;
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
      myInventory.ContentsChanged += new Action<MyInventoryBase>(this.m_inventory_ContentsChanged);
    }

    protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
    {
      base.OnInventoryComponentRemoved(inventory);
      if (!(inventory is MyInventory myInventory))
        return;
      myInventory.ContentsChanged -= new Action<MyInventoryBase>(this.m_inventory_ContentsChanged);
    }

    bool IMyGasBlock.IsWorking() => this.CanStore;

    private void SetStockpilingState(bool newState)
    {
      this.m_isStockpiling = newState;
      this.SourceComp.SetProductionEnabledByType(this.BlockDefinition.StoredGasId, !this.m_isStockpiling && this.CanStore);
      this.ResourceSink.Update();
      this.RaisePropertiesChanged();
    }

    internal void Transfer(double transferAmount)
    {
      if (transferAmount > 0.0)
      {
        this.Fill(transferAmount);
      }
      else
      {
        if (transferAmount >= 0.0)
          return;
        this.Drain(-transferAmount);
      }
    }

    internal void Fill(double amount)
    {
      if (amount == 0.0)
        return;
      this.ChangeFilledRatio(Math.Min(1.0, this.FilledRatio + amount / (double) this.Capacity), Sandbox.Game.Multiplayer.Sync.IsServer);
    }

    internal void Drain(double amount)
    {
      if (amount == 0.0)
        return;
      this.ChangeFilledRatio(Math.Max(0.0, this.FilledRatio - amount / (double) this.Capacity), Sandbox.Game.Multiplayer.Sync.IsServer);
    }

    internal bool ChangeFilledRatio(double newFilledRatio, bool updateSync = false)
    {
      double filledRatio = this.FilledRatio;
      if (filledRatio == newFilledRatio && !MySession.Static.CreativeMode)
        return false;
      double num1 = Math.Round(filledRatio * (double) this.Capacity, 1);
      double num2 = Math.Round(newFilledRatio * (double) this.Capacity, 1);
      if (updateSync && num1 != num2)
      {
        this.ChangeFillRatioAmount(newFilledRatio);
        return false;
      }
      this.FilledRatio = newFilledRatio;
      if (MySession.Static.CreativeMode && newFilledRatio > filledRatio)
        this.SourceComp.SetRemainingCapacityByType(this.BlockDefinition.StoredGasId, this.Capacity);
      else
        this.SourceComp.SetRemainingCapacityByType(this.BlockDefinition.StoredGasId, (float) this.FilledRatio * this.Capacity);
      this.ResourceSink.Update();
      this.UpdateEmissivity();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
      return true;
    }

    public double GetOxygenLevel() => this.FilledRatio;

    public bool IsResourceStorage(MyDefinitionId resourceDefinition) => this.SourceComp.ResourceTypes.Any<MyDefinitionId>((Func<MyDefinitionId, bool>) (x => x == resourceDefinition));

    public void ChangeStockpileMode(bool newStockpileMode)
    {
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyGasTank, bool>(this, (Func<MyGasTank, Action<bool>>) (x => new Action<bool>(x.OnStockipleModeCallback)), newStockpileMode);
      this.UpdateEmissivity();
    }

    [Event(null, 843)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnStockipleModeCallback(bool newStockpileMode) => this.IsStockpiling = newStockpileMode;

    private void OnAutoRefillChanged()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !(bool) this.m_autoRefill || !this.CanRefill())
        return;
      this.RefillBottles();
    }

    public void ChangeFillRatioAmount(double newFilledRatio) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyGasTank, double>(this, (Func<MyGasTank, Action<double>>) (x => new Action<double>(x.OnFilledRatioCallback)), newFilledRatio);

    [Event(null, 862)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnFilledRatioCallback(double newFilledRatio) => this.ChangeFilledRatio(newFilledRatio);

    public void SendRefillRequest() => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyGasTank>(this, (Func<MyGasTank, Action>) (x => new Action(x.OnRefillCallback)));

    [Event(null, 873)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnRefillCallback() => this.RefillBottles();

    int IMyInventoryOwner.InventoryCount => this.InventoryCount;

    long IMyInventoryOwner.EntityId => this.EntityId;

    bool IMyInventoryOwner.HasInventory => this.HasInventory;

    bool IMyInventoryOwner.UseConveyorSystem
    {
      get => false;
      set
      {
      }
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

    public void InitializeConveyorEndpoint() => this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    protected sealed class OnStockipleModeCallback\u003C\u003ESystem_Boolean : ICallSite<MyGasTank, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyGasTank @this,
        in bool newStockpileMode,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnStockipleModeCallback(newStockpileMode);
      }
    }

    protected sealed class OnFilledRatioCallback\u003C\u003ESystem_Double : ICallSite<MyGasTank, double, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyGasTank @this,
        in double newFilledRatio,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnFilledRatioCallback(newFilledRatio);
      }
    }

    protected sealed class OnRefillCallback\u003C\u003E : ICallSite<MyGasTank, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyGasTank @this,
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

    protected class m_autoRefill\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyGasTank) obj0).m_autoRefill = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyGasTank\u003C\u003EActor : IActivator, IActivator<MyGasTank>
    {
      object IActivator.CreateInstance() => (object) new MyGasTank();

      MyGasTank IActivator<MyGasTank>.CreateInstance() => new MyGasTank();
    }
  }
}
