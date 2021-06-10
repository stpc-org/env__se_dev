// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyBatteryBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
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
using VRage.Game.Graphics;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_BatteryBlock))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyBatteryBlock), typeof (Sandbox.ModAPI.Ingame.IMyBatteryBlock)})]
  public class MyBatteryBlock : MyFunctionalBlock, Sandbox.ModAPI.IMyBatteryBlock, Sandbox.ModAPI.IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyBatteryBlock
  {
    private static readonly string[] m_emissiveTextureNames = new string[4]
    {
      "Emissive0",
      "Emissive1",
      "Emissive2",
      "Emissive3"
    };
    private bool m_hasRemainingCapacity;
    private float m_maxOutput;
    private float m_currentOutput;
    private float m_currentStoredPower;
    private float m_maxStoredPower;
    private int m_lastUpdateTime;
    private float m_timeRemaining;
    private bool m_sourceDirty;
    private const int m_productionUpdateInterval = 100;
    private readonly VRage.Sync.Sync<ChargeMode, SyncDirection.BothWays> m_chargeMode;
    private readonly VRage.Sync.Sync<bool, SyncDirection.FromServer> m_isFull;
    private readonly VRage.Sync.Sync<float, SyncDirection.FromServer> m_storedPower;
    private Color m_prevEmissiveColor = Color.Black;
    private int m_prevFillCount = -1;
    private MyResourceSourceComponent m_sourceComp;

    public MyBatteryBlockDefinition BlockDefinition => base.BlockDefinition as MyBatteryBlockDefinition;

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

    public float TimeRemaining
    {
      get => this.m_timeRemaining;
      set
      {
        this.m_timeRemaining = value;
        this.SetDetailedInfoDirty();
        this.RaisePropertiesChanged();
      }
    }

    public bool HasCapacityRemaining => this.SourceComp.HasCapacityRemainingByType(MyResourceDistributorComponent.ElectricityId);

    public float MaxStoredPower
    {
      get => this.m_maxStoredPower;
      private set
      {
        if ((double) this.m_maxStoredPower == (double) value)
          return;
        this.m_maxStoredPower = value;
      }
    }

    private bool ProducerEnabled => (ChargeMode) this.m_chargeMode != ChargeMode.Recharge;

    public float CurrentStoredPower
    {
      get => this.SourceComp.RemainingCapacityByType(MyResourceDistributorComponent.ElectricityId);
      set
      {
        this.SourceComp.SetRemainingCapacityByType(MyResourceDistributorComponent.ElectricityId, MathHelper.Clamp(value, 0.0f, this.MaxStoredPower));
        this.UpdateMaxOutputAndEmissivity();
      }
    }

    public float CurrentOutput => this.SourceComp != null ? this.SourceComp.CurrentOutput : 0.0f;

    public float MaxOutput => this.SourceComp != null ? this.SourceComp.MaxOutput : 0.0f;

    public float CurrentInput => this.ResourceSink != null ? this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) : 0.0f;

    public float MaxInput => this.ResourceSink != null ? this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0.0f;

    public bool IsCharging => (double) this.CurrentInput > (double) this.CurrentOutput && (double) this.CurrentInput > 0.0;

    public bool SemiautoEnabled
    {
      get => (ChargeMode) this.m_chargeMode == ChargeMode.Auto;
      set
      {
        if (!value)
          return;
        this.m_chargeMode.Value = ChargeMode.Auto;
      }
    }

    public bool OnlyRecharge
    {
      get => (ChargeMode) this.m_chargeMode == ChargeMode.Recharge;
      set => this.m_chargeMode.Value = value ? ChargeMode.Recharge : ChargeMode.Auto;
    }

    public bool OnlyDischarge
    {
      get => (ChargeMode) this.m_chargeMode == ChargeMode.Discharge;
      set => this.m_chargeMode.Value = value ? ChargeMode.Discharge : ChargeMode.Auto;
    }

    public ChargeMode ChargeMode
    {
      get => this.m_chargeMode.Value;
      set
      {
        if (this.m_chargeMode.Value == value)
          return;
        this.m_chargeMode.Value = value;
      }
    }

    protected override bool CheckIsWorking() => this.Enabled && this.SourceComp.HasCapacityRemainingByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public MyBatteryBlock()
    {
      this.CreateTerminalControls();
      this.SourceComp = new MyResourceSourceComponent();
      this.ResourceSink = new MyResourceSinkComponent();
      this.SourceComp.OutputChanged += (MyResourceOutputChangedDelegate) ((x, y, z) =>
      {
        this.SetDetailedInfoDirty();
        this.RaisePropertiesChanged();
      });
      this.m_chargeMode.ValueChanged += (Action<SyncBase>) (x =>
      {
        this.SourceComp.SetProductionEnabledByType(MyResourceDistributorComponent.ElectricityId, (ChargeMode) this.m_chargeMode != ChargeMode.Recharge);
        this.UpdateMaxOutputAndEmissivity();
        this.m_sourceDirty = true;
      });
      this.m_storedPower.ValueChanged += (Action<SyncBase>) (x => this.CapacityChanged());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyBatteryBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlCombobox<MyBatteryBlock> terminalControlCombobox = new MyTerminalControlCombobox<MyBatteryBlock>("ChargeMode", MySpaceTexts.BlockPropertyTitle_ChargeMode, MySpaceTexts.Blank);
      terminalControlCombobox.ComboBoxContent = new Action<List<MyTerminalControlComboBoxItem>>(MyBatteryBlock.FillChargeModeCombo);
      terminalControlCombobox.Getter = (MyTerminalValueControl<MyBatteryBlock, long>.GetterDelegate) (x => (long) x.ChargeMode);
      terminalControlCombobox.Setter = (MyTerminalValueControl<MyBatteryBlock, long>.SetterDelegate) ((x, v) => x.ChargeMode = (ChargeMode) v);
      terminalControlCombobox.SetSerializerRange((int) MyEnum<ChargeMode>.Range.Min, (int) MyEnum<ChargeMode>.Range.Max);
      MyTerminalControlFactory.AddControl<MyBatteryBlock>((MyTerminalControl<MyBatteryBlock>) terminalControlCombobox);
      MyTerminalControlFactory.AddAction<MyBatteryBlock>(new MyTerminalAction<MyBatteryBlock>("Recharge", MyTexts.Get(MySpaceTexts.BlockActionTitle_RechargeToggle), new Action<MyBatteryBlock>(MyBatteryBlock.OnRechargeToggle), new MyTerminalControl<MyBatteryBlock>.WriterDelegate(MyBatteryBlock.WriteChargeModeValue), MyTerminalActionIcons.TOGGLE));
      MyTerminalControlFactory.AddAction<MyBatteryBlock>(new MyTerminalAction<MyBatteryBlock>("Discharge", MyTexts.Get(MySpaceTexts.BlockActionTitle_DischargeToggle), new Action<MyBatteryBlock>(MyBatteryBlock.OnDischargeToggle), new MyTerminalControl<MyBatteryBlock>.WriterDelegate(MyBatteryBlock.WriteChargeModeValue), MyTerminalActionIcons.TOGGLE));
      MyTerminalControlFactory.AddAction<MyBatteryBlock>(new MyTerminalAction<MyBatteryBlock>("Auto", MyTexts.Get(MySpaceTexts.BlockActionTitle_AutoEnable), new Action<MyBatteryBlock>(MyBatteryBlock.OnAutoEnabled), new MyTerminalControl<MyBatteryBlock>.WriterDelegate(MyBatteryBlock.WriteChargeModeValue), MyTerminalActionIcons.TOGGLE));
    }

    private static void OnRechargeToggle(MyBatteryBlock block) => block.OnlyRecharge = !block.OnlyRecharge;

    private static void OnDischargeToggle(MyBatteryBlock block) => block.OnlyDischarge = !block.OnlyDischarge;

    private static void OnAutoEnabled(MyBatteryBlock block) => block.ChargeMode = ChargeMode.Auto;

    private static void WriteChargeModeValue(MyBatteryBlock block, StringBuilder writeTo)
    {
      switch (block.ChargeMode)
      {
        case ChargeMode.Auto:
          writeTo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyTitle_Auto));
          break;
        case ChargeMode.Recharge:
          writeTo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyTitle_Recharge));
          break;
        case ChargeMode.Discharge:
          writeTo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyTitle_Discharge));
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private static void FillChargeModeCombo(List<MyTerminalControlComboBoxItem> list)
    {
      list.Add(new MyTerminalControlComboBoxItem()
      {
        Key = 0L,
        Value = MySpaceTexts.BlockPropertyTitle_Auto
      });
      list.Add(new MyTerminalControlComboBoxItem()
      {
        Key = 1L,
        Value = MySpaceTexts.BlockPropertyTitle_Recharge
      });
      list.Add(new MyTerminalControlComboBoxItem()
      {
        Key = 2L,
        Value = MySpaceTexts.BlockPropertyTitle_Discharge
      });
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SourceComp.Init(this.BlockDefinition.ResourceSourceGroup, new List<MyResourceSourceInfo>()
      {
        new MyResourceSourceInfo()
        {
          ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
          DefinedOutput = this.BlockDefinition.MaxPowerOutput,
          ProductionToCapacityMultiplier = 3600f
        }
      });
      this.SourceComp.HasCapacityRemainingChanged += (MyResourceCapacityRemainingChangedDelegate) ((id, source) => this.UpdateIsWorking());
      this.SourceComp.ProductionEnabledChanged += new MyResourceCapacityRemainingChangedDelegate(this.Source_ProductionEnabledChanged);
      MyObjectBuilder_BatteryBlock builderBatteryBlock = (MyObjectBuilder_BatteryBlock) objectBuilder;
      this.SourceComp.SetProductionEnabledByType(MyResourceDistributorComponent.ElectricityId, builderBatteryBlock.ProducerEnabled);
      this.MaxStoredPower = this.BlockDefinition.MaxStoredPower;
      this.ResourceSink.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.RequiredPowerInput, new Func<float>(this.Sink_ComputeRequiredPower));
      this.SourceComp.Enabled = this.Enabled;
      base.Init(objectBuilder, cubeGrid);
      this.CurrentStoredPower = (double) builderBatteryBlock.CurrentStoredPower < 0.0 ? this.BlockDefinition.InitialStoredPowerRatio * this.BlockDefinition.MaxStoredPower : builderBatteryBlock.CurrentStoredPower;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_storedPower.Value = this.CurrentStoredPower;
      if (builderBatteryBlock.OnlyDischargeEnabled)
        this.m_chargeMode.SetLocalValue(ChargeMode.Discharge);
      else
        this.m_chargeMode.SetLocalValue((ChargeMode) builderBatteryBlock.ChargeMode);
      this.UpdateMaxOutputAndEmissivity();
      this.SetDetailedInfoDirty();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyBatteryBlock_IsWorkingChanged);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_lastUpdateTime = MySession.Static.GameplayFrameCounter;
      if (this.IsWorking)
        this.OnStartWorking();
      this.ResourceSink.Update();
    }

    private void MyBatteryBlock_IsWorkingChanged(MyCubeBlock obj)
    {
      this.UpdateMaxOutputAndEmissivity();
      this.ResourceSink.Update();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_BatteryBlock builderCubeBlock = (MyObjectBuilder_BatteryBlock) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.CurrentStoredPower = this.CurrentStoredPower;
      builderCubeBlock.ProducerEnabled = this.SourceComp.ProductionEnabled;
      builderCubeBlock.SemiautoEnabled = false;
      builderCubeBlock.OnlyDischargeEnabled = false;
      builderCubeBlock.ChargeMode = (int) this.ChargeMode;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.m_prevEmissiveColor = Color.White;
      this.UpdateEmissivity();
    }

    protected override void Closing()
    {
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(true);
      base.Closing();
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateEmissivity();
    }

    private float Sink_ComputeRequiredPower()
    {
      int num1 = !this.Enabled || !this.IsFunctional ? 0 : (!(bool) this.m_isFull ? 1 : 0);
      bool flag = this.ChargeMode != ChargeMode.Discharge;
      float num2 = (float) (((double) this.MaxStoredPower - (double) this.CurrentStoredPower) * 60.0 / 100.0) * this.SourceComp.ProductionToCapacityMultiplierByType(MyResourceDistributorComponent.ElectricityId);
      float num3 = this.SourceComp.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId);
      float num4 = 0.0f;
      int num5 = flag ? 1 : 0;
      if ((num1 & num5) != 0)
      {
        float val2 = this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);
        num4 = Math.Min(num2 + num3, val2);
      }
      return num4;
    }

    private float ComputeMaxPowerOutput() => !this.CheckIsWorking() || !this.SourceComp.ProductionEnabledByType(MyResourceDistributorComponent.ElectricityId) ? 0.0f : this.BlockDefinition.MaxPowerOutput;

    private void CalculateOutputTimeRemaining()
    {
      if ((double) this.CurrentStoredPower != 0.0 && (double) this.SourceComp.CurrentOutput != 0.0)
        this.TimeRemaining = this.CurrentStoredPower / ((this.SourceComp.CurrentOutput - this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId)) / this.SourceComp.ProductionToCapacityMultiplier);
      else
        this.TimeRemaining = 0.0f;
    }

    private void CalculateInputTimeRemaining()
    {
      if ((double) this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) != 0.0)
        this.TimeRemaining = (float) (((double) this.MaxStoredPower - (double) this.CurrentStoredPower) / (((double) this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) - (double) this.SourceComp.CurrentOutput) / (double) this.SourceComp.ProductionToCapacityMultiplierByType(MyResourceDistributorComponent.ElectricityId)));
      else
        this.TimeRemaining = 0.0f;
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      float lastUpdateTime = (float) this.m_lastUpdateTime;
      this.m_lastUpdateTime = MySession.Static.GameplayFrameCounter;
      if (!this.IsFunctional)
        return;
      this.UpdateMaxOutputAndEmissivity();
      float timeDeltaMs = (float) (((double) MySession.Static.GameplayFrameCounter - (double) lastUpdateTime) * 0.0166666675359011 * 1000.0);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        if (!MySession.Static.CreativeMode)
        {
          switch (this.ChargeMode)
          {
            case ChargeMode.Auto:
              this.TransferPower(timeDeltaMs, this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId), this.SourceComp.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId));
              break;
            case ChargeMode.Recharge:
              this.StorePower(timeDeltaMs, this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId));
              break;
            case ChargeMode.Discharge:
              this.ConsumePower(timeDeltaMs, this.SourceComp.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId));
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
        else if (this.IsFunctional)
        {
          if (this.ChargeMode != ChargeMode.Discharge)
          {
            float input = (float) ((double) this.SourceComp.ProductionToCapacityMultiplierByType(MyResourceDistributorComponent.ElectricityId) * (double) this.MaxStoredPower / 8.0 * (!this.Enabled || !this.IsFunctional ? 0.0 : 1.0));
            this.StorePower(timeDeltaMs, input);
          }
          else
          {
            this.UpdateIsWorking();
            if (!this.SourceComp.HasCapacityRemainingByType(MyResourceDistributorComponent.ElectricityId))
              return;
            this.CalculateOutputTimeRemaining();
          }
        }
      }
      this.ResourceSink.Update();
      if (this.m_sourceDirty)
        this.SourceComp.OnProductionEnabledChanged(new MyDefinitionId?(MyResourceDistributorComponent.ElectricityId));
      this.m_sourceDirty = false;
      switch (this.ChargeMode)
      {
        case ChargeMode.Auto:
          if ((double) this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId) > (double) this.SourceComp.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId))
          {
            this.CalculateInputTimeRemaining();
            break;
          }
          this.CalculateOutputTimeRemaining();
          break;
        case ChargeMode.Recharge:
          this.CalculateInputTimeRemaining();
          break;
        case ChargeMode.Discharge:
          this.CalculateOutputTimeRemaining();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    protected override void OnEnabledChanged()
    {
      this.SourceComp.Enabled = this.Enabled;
      this.UpdateMaxOutputAndEmissivity();
      this.ResourceSink.Update();
      base.OnEnabledChanged();
    }

    private void UpdateMaxOutputAndEmissivity()
    {
      this.ResourceSink.Update();
      this.SourceComp.SetMaxOutputByType(MyResourceDistributorComponent.ElectricityId, this.ComputeMaxPowerOutput());
      this.UpdateEmissivity();
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BatteryBlock));
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxOutput));
      MyValueFormatter.AppendWorkInBestUnit(this.BlockDefinition.MaxPowerOutput, detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.BlockDefinition.RequiredPowerInput, detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxStoredPower));
      MyValueFormatter.AppendWorkHoursInBestUnit(this.MaxStoredPower, detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentOutput));
      MyValueFormatter.AppendWorkInBestUnit(this.SourceComp.CurrentOutput, detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_StoredPower));
      MyValueFormatter.AppendWorkHoursInBestUnit(this.CurrentStoredPower, detailedInfo);
      detailedInfo.Append("\n");
      float num1 = this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId);
      float num2 = this.SourceComp.CurrentOutputByType(MyResourceDistributorComponent.ElectricityId);
      if ((double) num1 > (double) num2)
      {
        detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_RechargedIn));
        MyValueFormatter.AppendTimeInBestUnit(this.m_timeRemaining, detailedInfo);
      }
      else if ((double) num1 == (double) num2)
      {
        detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_DepletedIn));
        MyValueFormatter.AppendTimeInBestUnit(float.PositiveInfinity, detailedInfo);
      }
      else
      {
        detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_DepletedIn));
        MyValueFormatter.AppendTimeInBestUnit(this.m_timeRemaining, detailedInfo);
      }
    }

    private void TransferPower(float timeDeltaMs, float input, float output)
    {
      float input1 = input - output;
      if ((double) input1 < 0.0)
      {
        this.ConsumePower(timeDeltaMs, -input1);
      }
      else
      {
        if ((double) input1 <= 0.0)
          return;
        this.StorePower(timeDeltaMs, input1);
      }
    }

    private void StorePower(float timeDeltaMs, float input)
    {
      float num1 = input / (this.SourceComp.ProductionToCapacityMultiplierByType(MyResourceDistributorComponent.ElectricityId) * 1000f);
      float num2 = (float) ((double) timeDeltaMs * (double) num1 * 0.800000011920929);
      if ((double) num2 > 0.0)
      {
        if ((double) this.CurrentStoredPower + (double) num2 < (double) this.MaxStoredPower)
        {
          this.CurrentStoredPower += num2;
        }
        else
        {
          this.CurrentStoredPower = this.MaxStoredPower;
          this.TimeRemaining = 0.0f;
          if (Sandbox.Game.Multiplayer.Sync.IsServer && !(bool) this.m_isFull)
            this.m_isFull.Value = true;
        }
      }
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_storedPower.Value = this.CurrentStoredPower;
    }

    private void ConsumePower(float timeDeltaMs, float output)
    {
      if (!this.SourceComp.HasCapacityRemainingByType(MyResourceDistributorComponent.ElectricityId))
        return;
      float num1 = output / (this.SourceComp.ProductionToCapacityMultiplier * 1000f);
      float num2 = timeDeltaMs * num1;
      if ((double) num2 == 0.0)
        return;
      if ((double) this.CurrentStoredPower - (double) num2 <= 0.0)
      {
        this.SourceComp.SetOutput(0.0f);
        this.CurrentStoredPower = 0.0f;
        this.TimeRemaining = 0.0f;
      }
      else
      {
        this.CurrentStoredPower -= num2;
        if (Sandbox.Game.Multiplayer.Sync.IsServer && (bool) this.m_isFull)
          this.m_isFull.Value = false;
      }
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_storedPower.Value = this.CurrentStoredPower;
    }

    public override bool SetEmissiveStateWorking() => false;

    public override bool SetEmissiveStateDamaged() => false;

    public override bool SetEmissiveStateDisabled() => false;

    internal void UpdateEmissivity()
    {
      if (!this.InScene)
        return;
      float fill = 1f;
      Color color = Color.Red;
      if (this.IsFunctional && this.Enabled)
      {
        if (this.IsWorking)
        {
          fill = this.CurrentStoredPower / this.MaxStoredPower;
          if (this.ChargeMode == ChargeMode.Auto)
          {
            color = Color.Green;
            MyEmissiveColorStateResult result;
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
              color = result.EmissiveColor;
          }
          else if (this.ChargeMode == ChargeMode.Discharge)
          {
            color = Color.SteelBlue;
            MyEmissiveColorStateResult result;
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Alternative, out result))
              color = result.EmissiveColor;
          }
          else
          {
            color = Color.Yellow;
            MyEmissiveColorStateResult result;
            if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result))
              color = result.EmissiveColor;
          }
        }
        else
        {
          fill = 0.25f;
          MyEmissiveColorStateResult result;
          if (MyEmissiveColorPresets.LoadPresetState(this.BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
            color = result.EmissiveColor;
        }
      }
      else if (this.IsFunctional)
      {
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
      if (this.BlockDefinition.Id.SubtypeName == "SmallBlockSmallBatteryBlock")
        MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyBatteryBlock.m_emissiveTextureNames[0], color, 1f);
      else
        this.SetEmissive(color, fill);
    }

    private void SetEmissive(Color color, float fill)
    {
      int num = (int) ((double) fill * (double) MyBatteryBlock.m_emissiveTextureNames.Length);
      if (this.Render.RenderObjectIDs[0] == uint.MaxValue || !(color != this.m_prevEmissiveColor) && num == this.m_prevFillCount)
        return;
      for (int index = 0; index < MyBatteryBlock.m_emissiveTextureNames.Length; ++index)
      {
        if (index < num)
          MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyBatteryBlock.m_emissiveTextureNames[index], color, 1f);
        else
          MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], MyBatteryBlock.m_emissiveTextureNames[index], Color.Black, 0.0f);
      }
      this.m_prevEmissiveColor = color;
      this.m_prevFillCount = num;
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.m_prevFillCount = -1;
    }

    private void ComponentStack_IsFunctionalChanged() => this.UpdateMaxOutputAndEmissivity();

    private void ProducerEnadChanged() => this.SourceComp.SetProductionEnabledByType(MyResourceDistributorComponent.ElectricityId, this.ProducerEnabled);

    private void Source_ProductionEnabledChanged(
      MyDefinitionId changedResourceId,
      MyResourceSourceComponent source)
    {
      this.UpdateIsWorking();
    }

    private void CapacityChanged() => this.CurrentStoredPower = this.m_storedPower.Value;

    protected class m_chargeMode\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<ChargeMode, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<ChargeMode, SyncDirection.BothWays>(obj1, obj2));
        ((MyBatteryBlock) obj0).m_chargeMode = (VRage.Sync.Sync<ChargeMode, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_isFull\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.FromServer>(obj1, obj2));
        ((MyBatteryBlock) obj0).m_isFull = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_storedPower\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.FromServer>(obj1, obj2));
        ((MyBatteryBlock) obj0).m_storedPower = (VRage.Sync.Sync<float, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyBatteryBlock\u003C\u003EActor : IActivator, IActivator<MyBatteryBlock>
    {
      object IActivator.CreateInstance() => (object) new MyBatteryBlock();

      MyBatteryBlock IActivator<MyBatteryBlock>.CreateInstance() => new MyBatteryBlock();
    }
  }
}
