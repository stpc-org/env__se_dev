// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyGasFueledPowerProducer
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Sync;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  public abstract class MyGasFueledPowerProducer : MyFueledPowerProducer, IMyGasTank
  {
    private MyResourceSinkComponent m_sinkComponent;
    private bool m_needsUpdate;

    public MyGasFueledPowerProducerDefinition BlockDefinition => (MyGasFueledPowerProducerDefinition) base.BlockDefinition;

    public MyResourceSinkComponent SinkComp
    {
      get => this.m_sinkComponent;
      set
      {
        if (this.m_sinkComponent != null)
          this.m_sinkComponent.CurrentInputChanged -= new MyCurrentResourceInputChangedDelegate(this.OnFuelInputChanged);
        MyEntityComponentContainer components = this.Components;
        this.m_sinkComponent = value;
        components.Remove<MyResourceSinkComponent>();
        components.Add<MyResourceSinkComponent>(value);
        if (this.m_sinkComponent == null)
          return;
        this.m_sinkComponent.CurrentInputChanged += new MyCurrentResourceInputChangedDelegate(this.OnFuelInputChanged);
      }
    }

    public double FilledRatio => (double) this.Capacity / (double) this.BlockDefinition.FuelCapacity;

    public Action FilledRatioChanged { get; set; }

    public float GasCapacity => this.BlockDefinition.FuelCapacity;

    public override float GetMaxCapacity() => this.GasCapacity;

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      MyGasFueledPowerProducerDefinition.FuelInfo fuel = this.BlockDefinition.Fuel;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, new MyResourceSinkInfo()
      {
        ResourceTypeId = fuel.FuelId,
        MaxRequiredInput = (float) ((double) fuel.Ratio * (double) this.BlockDefinition.FuelProductionToCapacityMultiplier * 2.0)
      });
      this.SinkComp = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      this.SourceComp.OutputChanged += new MyResourceOutputChangedDelegate(this.OnElectricityOutputChanged);
      this.MarkForUpdate();
    }

    protected override void OnCapacityChanged(SyncBase obj)
    {
      base.OnCapacityChanged(obj);
      this.FilledRatioChanged.InvokeIfNotNull();
    }

    protected override void OnStartWorking()
    {
      base.OnStartWorking();
      this.MarkForUpdate();
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      if (this.Enabled)
        this.MarkForUpdate();
      this.CheckEmissiveState();
    }

    protected override void OnStopWorking()
    {
      base.OnStopWorking();
      if (!this.Enabled || !this.IsFunctional)
      {
        foreach (MyDefinitionId acceptedResource in this.SinkComp.AcceptedResources)
          this.SinkComp.SetRequiredInputByType(acceptedResource, 0.0f);
      }
      this.DisableUpdate();
      this.CheckEmissiveState();
    }

    private void OnFuelInputChanged(
      MyDefinitionId resourceTypeId,
      float oldInput,
      MyResourceSinkComponent sink)
    {
      this.MarkForUpdate();
    }

    private void OnElectricityOutputChanged(
      MyDefinitionId resourceTypeId,
      float oldInput,
      MyResourceSourceComponent source)
    {
      this.MarkForUpdate();
    }

    private void MarkForUpdate()
    {
      if (this.m_needsUpdate)
        return;
      this.m_needsUpdate = true;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    private void DisableUpdate()
    {
      if (!this.m_needsUpdate)
        return;
      this.m_needsUpdate = false;
      if (this.HasDamageEffect)
        return;
      this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (!this.m_needsUpdate)
        return;
      int num1 = this.IsWorking ? 1 : 0;
      this.UpdateCapacity();
      bool isWorking = this.IsWorking;
      int num2 = isWorking ? 1 : 0;
      if (num1 == num2)
        return;
      if (isWorking)
        this.OnStartWorking();
      else
        this.OnStopWorking();
    }

    private void UpdateCapacity()
    {
      MyGasFueledPowerProducerDefinition blockDefinition = this.BlockDefinition;
      MyDefinitionId fuelId = blockDefinition.Fuel.FuelId;
      float num1 = (float) ((double) this.SourceComp.CurrentOutput / (double) blockDefinition.FuelProductionToCapacityMultiplier / 60.0);
      float num2 = this.SinkComp.CurrentInputByType(fuelId) / 60f / MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER;
      if ((double) num2 == 0.0 && MySession.Static.CreativeMode)
        num2 = num1 + this.GetFillingOffset();
      bool flag = (double) num2 == 0.0 && (double) this.SinkComp.RequiredInputByType(fuelId) > 0.0;
      float num3 = num2 - num1;
      int num4 = (double) num3 != 0.0 ? 1 : 0;
      if (num4 != 0)
      {
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          this.Capacity += num3;
        this.UpdateDisplay();
      }
      float fillingOffset = this.GetFillingOffset();
      if (num4 == 0 && (flag || (double) fillingOffset == 0.0))
        this.DisableUpdate();
      float num5 = num1 + fillingOffset * MyFueledPowerProducer.FUEL_CONSUMPTION_MULTIPLIER;
      this.SinkComp.SetRequiredInputByType(fuelId, num5 * 60f);
      this.CheckEmissiveState();
    }

    public override bool SetEmissiveStateWorking() => this.SetEmissiveState(this.IsSupplied ? MyCubeBlock.m_emissiveNames.Working : MyCubeBlock.m_emissiveNames.Warning, this.Render.RenderObjectIDs[0]);

    public override bool SetEmissiveStateDisabled() => this.Enabled ? this.SetEmissiveState(this.IsSupplied ? MyCubeBlock.m_emissiveNames.Disabled : MyCubeBlock.m_emissiveNames.Warning, this.Render.RenderObjectIDs[0]) : this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0]);

    private float GetFillingOffset()
    {
      if (!this.Enabled || !this.IsFunctional)
        return 0.0f;
      float capacity = this.Capacity;
      float fuelCapacity = this.BlockDefinition.FuelCapacity;
      return MathHelper.Clamp(fuelCapacity - capacity, 0.0f, fuelCapacity / 20f);
    }

    protected override void UpdateDetailedInfo(StringBuilder sb)
    {
      base.UpdateDetailedInfo(sb);
      float fuelCapacity = this.BlockDefinition.FuelCapacity;
      float num1 = Math.Min(this.Capacity, fuelCapacity);
      float num2 = (float) ((double) num1 / (double) fuelCapacity * 100.0);
      sb.Append(string.Format(MyTexts.GetString(MySpaceTexts.Oxygen_Filled), (object) num2.ToString("F1"), (object) num1.ToString("0"), (object) fuelCapacity.ToString("0")));
    }

    bool IMyGasTank.IsResourceStorage(MyDefinitionId resourceDefinition)
    {
      foreach (MyDefinitionId acceptedResource in this.SinkComp.AcceptedResources)
      {
        if (acceptedResource == resourceDefinition)
          return true;
      }
      return false;
    }
  }
}
