// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyFueledPowerProducer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_FueledPowerProducer))]
  public abstract class MyFueledPowerProducer : MyFunctionalBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock
  {
    public static float FUEL_CONSUMPTION_MULTIPLIER = 1f;
    private MyResourceSourceComponent m_sourceComponent;
    private readonly VRage.Sync.Sync<float, SyncDirection.FromServer> m_capacity;

    public MyFueledPowerProducerDefinition BlockDefinition => (MyFueledPowerProducerDefinition) base.BlockDefinition;

    public MyResourceSourceComponent SourceComp
    {
      get => this.m_sourceComponent;
      set
      {
        if (this.m_sourceComponent != null)
        {
          this.m_sourceComponent.OutputChanged -= new MyResourceOutputChangedDelegate(this.OnCurrentOrMaxOutputChanged);
          this.m_sourceComponent.MaxOutputChanged -= new MyResourceOutputChangedDelegate(this.OnCurrentOrMaxOutputChanged);
        }
        MyEntityComponentContainer components = this.Components;
        if (this.ContainsDebugRenderComponent(typeof (MyDebugRenderComponentDrawPowerSource)))
          this.RemoveDebugRenderComponent(typeof (MyDebugRenderComponentDrawPowerSource));
        this.m_sourceComponent = value;
        components.Remove<MyResourceSourceComponent>();
        components.Add<MyResourceSourceComponent>(value);
        if (this.m_sourceComponent == null)
          return;
        this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawPowerSource(this.m_sourceComponent, (VRage.ModAPI.IMyEntity) this));
        this.m_sourceComponent.OutputChanged += new MyResourceOutputChangedDelegate(this.OnCurrentOrMaxOutputChanged);
        this.m_sourceComponent.MaxOutputChanged += new MyResourceOutputChangedDelegate(this.OnCurrentOrMaxOutputChanged);
      }
    }

    public float Capacity
    {
      get => this.m_capacity.Value;
      set => this.m_capacity.Value = Math.Max(value, 0.0f);
    }

    public virtual float GetMaxCapacity() => float.PositiveInfinity;

    public bool IsSupplied => (double) this.Capacity > 0.0;

    public float CurrentOutput => this.SourceComp.CurrentOutput;

    public virtual float MaxOutput => this.BlockDefinition.MaxPowerOutput;

    protected MyFueledPowerProducer()
    {
      this.SourceComp = new MyResourceSourceComponent();
      this.m_capacity.ValueChanged += new Action<SyncBase>(this.OnCapacityChanged);
      this.m_capacity.AlwaysReject<float, SyncDirection.FromServer>();
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SourceComp.Init(this.BlockDefinition.ResourceSourceGroup, new MyResourceSourceInfo()
      {
        DefinedOutput = this.BlockDefinition.MaxPowerOutput,
        ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
        ProductionToCapacityMultiplier = this.BlockDefinition.FuelProductionToCapacityMultiplier
      });
      if (this.BlockDefinition is MyHydrogenEngineDefinition)
        this.SourceComp.CountTowardsRemainingEnergyTime = false;
      this.SourceComp.Enabled = this.Enabled;
      base.Init(objectBuilder, cubeGrid);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.OnIsFunctionalChanged);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_capacity.SetLocalValue(MathHelper.Clamp(((MyObjectBuilder_FueledPowerProducer) objectBuilder).Capacity, 0.0f, this.GetMaxCapacity()));
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_FueledPowerProducer builderCubeBlock = (MyObjectBuilder_FueledPowerProducer) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.Capacity = this.Capacity;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override bool CheckIsWorking()
    {
      MyResourceSourceComponent sourceComp = this.SourceComp;
      return sourceComp.Enabled && this.IsSupplied && sourceComp.ProductionEnabled && base.CheckIsWorking();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (!this.IsWorking)
        return;
      this.OnStartWorking();
    }

    protected override void Closing()
    {
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(true);
      base.Closing();
    }

    private void OnIsFunctionalChanged()
    {
      this.OnProductionChanged();
      if (this.IsWorking)
        this.OnStartWorking();
      else
        this.OnStopWorking();
    }

    protected virtual void OnCapacityChanged(SyncBase obj)
    {
      this.SourceComp.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, this.Capacity);
      this.UpdateIsWorking();
      this.OnProductionChanged();
    }

    protected override void OnEnabledChanged()
    {
      this.SourceComp.Enabled = this.Enabled;
      base.OnEnabledChanged();
      this.UpdateIsWorking();
      this.OnProductionChanged();
    }

    protected override void OnStartWorking()
    {
      base.OnStartWorking();
      this.OnProductionChanged();
    }

    protected override void OnStopWorking()
    {
      base.OnStopWorking();
      this.OnProductionChanged();
    }

    protected virtual void OnProductionChanged()
    {
      float newMaxOutput = 0.0f;
      if (this.Enabled && this.IsFunctional && this.IsSupplied)
        newMaxOutput = this.ComputeMaxProduction();
      this.SourceComp.SetMaxOutput(newMaxOutput);
    }

    protected virtual float ComputeMaxProduction() => this.CheckIsWorking() || MySession.Static.CreativeMode && base.CheckIsWorking() ? this.MaxOutput : 0.0f;

    protected virtual void OnCurrentOrMaxOutputChanged(
      MyDefinitionId changedResourceId,
      float oldOutput,
      MyResourceSourceComponent source)
    {
      if (source.CurrentOutputByType(changedResourceId).IsEqual(oldOutput, oldOutput * 0.1f))
        return;
      MySandboxGame.Static.Invoke((Action) (() => this.UpdateDisplay()), nameof (OnCurrentOrMaxOutputChanged));
    }

    protected void UpdateDisplay()
    {
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    protected override void UpdateDetailedInfo(StringBuilder sb)
    {
      base.UpdateDetailedInfo(sb);
      MyResourceSourceComponent sourceComp = this.SourceComp;
      sb.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      sb.Append(this.BlockDefinition.DisplayNameText);
      sb.Append('\n');
      sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxOutput));
      MyValueFormatter.AppendWorkInBestUnit(this.MaxOutput, sb);
      sb.Append('\n');
      sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentOutput));
      MyValueFormatter.AppendWorkInBestUnit(sourceComp.CurrentOutput, sb);
      sb.Append('\n');
    }

    public IMyConveyorEndpoint ConveyorEndpoint { get; private set; }

    public void InitializeConveyorEndpoint()
    {
      this.ConveyorEndpoint = (IMyConveyorEndpoint) new MyMultilineConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint(this.ConveyorEndpoint));
    }

    public virtual bool AllowSelfPulling() => false;

    public virtual PullInformation GetPullInformation() => (PullInformation) null;

    public virtual PullInformation GetPushInformation() => (PullInformation) null;

    protected class m_capacity\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.FromServer>(obj1, obj2));
        ((MyFueledPowerProducer) obj0).m_capacity = (VRage.Sync.Sync<float, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
