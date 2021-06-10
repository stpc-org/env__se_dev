// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyOxygenFarm
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using SpaceEngineers.Game.EntityComponents.DebugRenders;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_OxygenFarm))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyOxygenFarm), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyOxygenFarm)})]
  public class MyOxygenFarm : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyOxygenFarm, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, SpaceEngineers.Game.ModAPI.Ingame.IMyOxygenFarm, IMyGasBlock, IMyConveyorEndpointBlock
  {
    private static readonly string[] m_emissiveTextureNames = new string[4]
    {
      "Emissive0",
      "Emissive1",
      "Emissive2",
      "Emissive3"
    };
    private float m_maxGasOutputFactor;
    private bool firstUpdate = true;
    private readonly MyDefinitionId m_oxygenGasId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Oxygen");
    private MyResourceSourceComponent m_sourceComp;
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;

    public MyOxygenFarmDefinition BlockDefinition => base.BlockDefinition as MyOxygenFarmDefinition;

    public MySolarGameLogicComponent SolarComponent { get; private set; }

    public bool CanProduce => (MySession.Static.Settings.EnableOxygen || this.BlockDefinition.ProducedGas != this.m_oxygenGasId) && (this.Enabled && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId)) && this.IsWorking && this.IsFunctional;

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

    public bool CanPressurizeRoom => false;

    public MyOxygenFarm()
    {
      this.ResourceSink = new MyResourceSinkComponent();
      this.SourceComp = new MyResourceSourceComponent();
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.OnIsWorkingChanged);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.InitializeConveyorEndpoint();
      this.SourceComp.Init(this.BlockDefinition.ResourceSourceGroup, new MyResourceSourceInfo()
      {
        ResourceTypeId = this.BlockDefinition.ProducedGas,
        DefinedOutput = this.BlockDefinition.MaxGasOutput,
        ProductionToCapacityMultiplier = 1f,
        IsInfiniteCapacity = true
      });
      this.SourceComp.Enabled = this.IsWorking;
      this.ResourceSink.Init(this.BlockDefinition.ResourceSinkGroup, new MyResourceSinkInfo()
      {
        ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
        MaxRequiredInput = this.BlockDefinition.OperationalPowerConsumption,
        RequiredInputFunc = new Func<float>(this.ComputeRequiredPower)
      });
      this.ResourceSink.IsPoweredChanged += new Action(this.PowerReceiver_IsPoweredChanged);
      this.ResourceSink.Update();
      this.GameLogic = (MyGameLogicComponent) new MySolarGameLogicComponent();
      this.SolarComponent = this.GameLogic as MySolarGameLogicComponent;
      this.SolarComponent.Initialize(this.BlockDefinition.PanelOrientation, this.BlockDefinition.IsTwoSided, this.BlockDefinition.PanelOffset, (MyFunctionalBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentSolarPanel((MyTerminalBlock) this));
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_OnIsFunctionalChanged);
      this.OnModelChange();
      this.SetDetailedInfoDirty();
      this.UpdateEmissivity();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      return (MyObjectBuilder_CubeBlock) (base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_OxygenFarm);
    }

    private float ComputeRequiredPower() => !this.Enabled || !this.IsFunctional ? 0.0f : this.BlockDefinition.OperationalPowerConsumption;

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.SourceComp.Enabled = this.IsWorking;
      this.ResourceSink.Update();
      this.UpdateEmissivity();
    }

    private void ComponentStack_OnIsFunctionalChanged()
    {
      this.SourceComp.Enabled = this.IsWorking;
      this.ResourceSink.Update();
      this.UpdateEmissivity();
    }

    private void PowerReceiver_IsPoweredChanged() => this.UpdateIsWorking();

    private void OnIsWorkingChanged(MyCubeBlock obj) => MySandboxGame.Static.Invoke((Action) (() =>
    {
      this.SourceComp.Enabled = this.IsWorking;
      this.UpdateEmissivity();
    }), "MyOxygenFarm::OnIsWorkingChanged");

    protected override bool CheckIsWorking() => (MySession.Static.Settings.EnableOxygen || this.BlockDefinition.ProducedGas != this.m_oxygenGasId) && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_OxygenOutput));
      detailedInfo.Append((this.SourceComp.MaxOutputByType(this.BlockDefinition.ProducedGas) * 60f).ToString("F"));
      detailedInfo.Append(" L/min");
    }

    private void UpdateEmissivity()
    {
      if (!this.InScene)
        return;
      Color emissivePartColor1 = Color.Red;
      MyEmissiveColorStateResult result1;
      if (!this.IsWorking)
      {
        if (this.IsFunctional)
        {
          MyEmissiveColorStateResult result2;
          if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result2))
            emissivePartColor1 = result2.EmissiveColor;
          for (int index = 0; index < 4; ++index)
            MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyOxygenFarm.m_emissiveTextureNames[index], emissivePartColor1, 1f);
        }
        else
        {
          Color emissivePartColor2 = Color.Black;
          MyEmissiveColorStateResult result2;
          if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result2))
            emissivePartColor2 = result2.EmissiveColor;
          MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyOxygenFarm.m_emissiveTextureNames[0], emissivePartColor2, 0.0f);
          for (int index = 1; index < 4; ++index)
            MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyOxygenFarm.m_emissiveTextureNames[index], emissivePartColor2, 0.0f);
        }
      }
      else if ((double) this.m_maxGasOutputFactor > 0.0)
      {
        for (int index = 0; index < 4; ++index)
        {
          if ((double) index < (double) this.m_maxGasOutputFactor * 4.0)
          {
            Color emissivePartColor2 = Color.Green;
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result1))
              emissivePartColor2 = result1.EmissiveColor;
            MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyOxygenFarm.m_emissiveTextureNames[index], emissivePartColor2, 1f);
          }
          else
          {
            Color emissivePartColor2 = Color.Black;
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result1))
              emissivePartColor2 = result1.EmissiveColor;
            MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyOxygenFarm.m_emissiveTextureNames[index], emissivePartColor2, 1f);
          }
        }
      }
      else
      {
        Color emissivePartColor2 = Color.Black;
        MyEmissiveColorStateResult result2;
        if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result2))
          emissivePartColor2 = result2.EmissiveColor;
        MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyOxygenFarm.m_emissiveTextureNames[0], emissivePartColor2, 0.0f);
        for (int index = 1; index < 4; ++index)
          MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyOxygenFarm.m_emissiveTextureNames[index], emissivePartColor2, 0.0f);
      }
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateIsWorking();
      this.UpdateEmissivity();
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      if (this.CubeGrid.Physics == null)
        return;
      this.ResourceSink.Update();
      float num = !this.IsWorking || !this.SourceComp.ProductionEnabledByType(this.BlockDefinition.ProducedGas) ? 0.0f : this.SolarComponent.MaxOutput;
      if ((double) num != (double) this.m_maxGasOutputFactor || this.firstUpdate)
      {
        this.m_maxGasOutputFactor = num;
        this.SourceComp.SetMaxOutputByType(this.BlockDefinition.ProducedGas, this.SourceComp.DefinedOutputByType(this.BlockDefinition.ProducedGas) * this.m_maxGasOutputFactor);
        this.UpdateVisual();
        this.SetDetailedInfoDirty();
        this.RaisePropertiesChanged();
        this.UpdateEmissivity();
        this.firstUpdate = false;
      }
      this.ResourceSink.Update();
    }

    bool IMyGasBlock.IsWorking() => MySession.Static.Settings.EnableOxygen && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && this.IsWorking && this.IsFunctional;

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public void InitializeConveyorEndpoint() => this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    float SpaceEngineers.Game.ModAPI.Ingame.IMyOxygenFarm.GetOutput() => this.IsWorking ? this.SolarComponent.MaxOutput : 0.0f;

    bool SpaceEngineers.Game.ModAPI.Ingame.IMyOxygenFarm.CanProduce => this.CanProduce;

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;
  }
}
