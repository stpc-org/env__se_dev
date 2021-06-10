// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyEnvironmentalPowerProducer
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace SpaceEngineers.Game.Entities.Blocks
{
  public abstract class MyEnvironmentalPowerProducer : MyFunctionalBlock, Sandbox.ModAPI.IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock
  {
    protected MySoundPair m_processSound = new MySoundPair();
    private MyResourceSourceComponent m_sourceComponent;

    public MyResourceSourceComponent SourceComp
    {
      get => this.m_sourceComponent;
      set
      {
        if (this.m_sourceComponent != null)
          this.m_sourceComponent.OutputChanged -= new MyResourceOutputChangedDelegate(this.OnCurrentOutputChanged);
        if (this.ContainsDebugRenderComponent(typeof (MyDebugRenderComponentDrawPowerSource)))
          this.RemoveDebugRenderComponent(typeof (MyDebugRenderComponentDrawPowerSource));
        if (this.Components.Contains(typeof (MyResourceSourceComponent)))
          this.Components.Remove<MyResourceSourceComponent>();
        this.Components.Add<MyResourceSourceComponent>(value);
        this.m_sourceComponent = value;
        if (this.m_sourceComponent == null)
          return;
        this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawPowerSource(this.m_sourceComponent, (VRage.ModAPI.IMyEntity) this));
        this.m_sourceComponent.OutputChanged += new MyResourceOutputChangedDelegate(this.OnCurrentOutputChanged);
      }
    }

    public MyPowerProducerDefinition BlockDefinition => (MyPowerProducerDefinition) base.BlockDefinition;

    public float CurrentOutput => this.SourceComp.CurrentOutput;

    public float MaxOutput => this.SourceComp.MaxOutput;

    protected abstract float CurrentProductionRatio { get; }

    protected MyEnvironmentalPowerProducer()
    {
      this.SourceComp = new MyResourceSourceComponent();
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.OnIsWorkingChanged);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SourceComp.Init(this.BlockDefinition.ResourceSourceGroup, new MyResourceSourceInfo()
      {
        ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
        DefinedOutput = this.BlockDefinition.MaxPowerOutput,
        IsInfiniteCapacity = true,
        ProductionToCapacityMultiplier = 3600f
      });
      this.m_processSound = this.BlockDefinition.ActionSound;
      this.SourceComp.SetMaxOutput(0.0f);
      base.Init(objectBuilder, cubeGrid);
    }

    protected void UpdateDisplay()
    {
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    protected override void UpdateDetailedInfo(StringBuilder sb)
    {
      base.UpdateDetailedInfo(sb);
      double maxOutput = (double) this.SourceComp.MaxOutput;
      float workInMegaWatts = Math.Min((float) maxOutput, this.SourceComp.CurrentOutput);
      sb.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      sb.Append(this.BlockDefinition.DisplayNameText);
      sb.Append('\n');
      sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxOutput));
      MyValueFormatter.AppendWorkInBestUnit((float) maxOutput, sb);
      sb.Append('\n');
      sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentOutput));
      MyValueFormatter.AppendWorkInBestUnit(workInMegaWatts, sb);
      sb.Append('\n');
    }

    private void OnIsWorkingChanged(MyCubeBlock obj) => this.OnProductionChanged();

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.SourceComp.Enabled = this.Enabled;
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
      if (!this.InScene || this.CubeGrid.IsPreview)
        return;
      float newMaxOutput = this.CurrentProductionRatio * this.BlockDefinition.MaxPowerOutput;
      this.SourceComp.SetMaxOutput(newMaxOutput);
      this.SourceComp.SetProductionEnabledByType(MyResourceDistributorComponent.ElectricityId, (double) newMaxOutput > 0.0);
      this.UpdateDisplay();
      this.RaisePropertiesChanged();
      if (this.m_soundEmitter == null || this.m_processSound == null)
        return;
      if ((double) newMaxOutput > 0.0)
        this.m_soundEmitter.PlaySound(this.m_processSound, true);
      else
        this.m_soundEmitter.StopSound(true);
    }

    protected void OnCurrentOutputChanged(
      MyDefinitionId changedResourceId,
      float oldOutput,
      MyResourceSourceComponent source)
    {
      if (source.CurrentOutputByType(changedResourceId).IsEqual(oldOutput, oldOutput * 0.1f))
        return;
      this.UpdateDisplay();
    }

    protected override void Closing()
    {
      base.Closing();
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.StopSound(true);
      this.m_soundEmitter = (MyEntity3DSoundEmitter) null;
    }
  }
}
