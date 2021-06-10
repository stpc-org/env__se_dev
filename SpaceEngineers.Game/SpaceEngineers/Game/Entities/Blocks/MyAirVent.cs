// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyAirVent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_AirVent))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyAirVent), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent)})]
  public class MyAirVent : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyAirVent, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent, IMyGasBlock, IMyConveyorEndpointBlock
  {
    private static readonly string[] m_emissiveTextureNames = new string[4]
    {
      "Emissive0",
      "Emissive1",
      "Emissive2",
      "Emissive3"
    };
    private MyStringHash m_prevColor = MyStringHash.NullOrEmpty;
    private int m_prevFillCount = -1;
    private bool m_isProducing;
    private bool m_producedSinceLastUpdate;
    private bool m_isPlayingVentEffect;
    private MyParticleEffect m_effect;
    private MyToolbarItem m_onFullAction;
    private MyToolbarItem m_onEmptyAction;
    private MyToolbar m_actionToolbar;
    private bool? m_wasRoomFull;
    private bool? m_wasRoomEmpty;
    private readonly MyDefinitionId m_oxygenGasId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Oxygen");
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_isDepressurizing;
    private readonly VRage.Sync.Sync<MyAirVentBlockRoomInfo, SyncDirection.FromServer> m_blockRoomInfo;
    private float m_oxygenModifier = 1f;
    private MyResourceSourceComponent m_sourceComp;
    private MyResourceSinkInfo OxygenSinkInfo;
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;
    private bool m_syncing;

    private MyModelDummy VentDummy
    {
      get
      {
        if (this.Model == null || this.Model.Dummies == null)
          return (MyModelDummy) null;
        MyModelDummy myModelDummy;
        this.Model.Dummies.TryGetValue("vent_001", out myModelDummy);
        return myModelDummy;
      }
    }

    public bool CanVent => MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && this.IsWorking;

    public bool CanVentToRoom => this.CanVent && !this.IsDepressurizing;

    public bool CanVentFromRoom => this.CanVent && this.IsDepressurizing;

    public float GasOutputPerSecond => !this.SourceComp.ProductionEnabledByType(this.m_oxygenGasId) ? 0.0f : this.SourceComp.CurrentOutputByType(this.m_oxygenGasId);

    public float GasInputPerSecond => !this.IsDepressurizing ? this.ResourceSink.CurrentInputByType(this.m_oxygenGasId) : 0.0f;

    public float GasOutputPerUpdate => this.GasOutputPerSecond * 0.01666667f;

    public float GasInputPerUpdate => this.GasInputPerSecond * 0.01666667f;

    public bool IsDepressurizing
    {
      get => (bool) this.m_isDepressurizing;
      set => this.m_isDepressurizing.Value = value;
    }

    public VentStatus Status { get; private set; }

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

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public bool CanPressurizeRoom => true;

    private MyAirVentDefinition BlockDefinition => (MyAirVentDefinition) base.BlockDefinition;

    public MyAirVent()
    {
      this.CreateTerminalControls();
      this.ResourceSink = new MyResourceSinkComponent(2);
      this.SourceComp = new MyResourceSourceComponent();
      this.m_isDepressurizing.ValueChanged += (Action<SyncBase>) (x => this.SetDepressurizing());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyAirVent>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MyAirVent> onOff = new MyTerminalControlOnOffSwitch<MyAirVent>("Depressurize", MySpaceTexts.BlockPropertyTitle_Depressurize, MySpaceTexts.BlockPropertyDescription_Depressurize);
      onOff.Getter = (MyTerminalValueControl<MyAirVent, bool>.GetterDelegate) (x => x.IsDepressurizing);
      onOff.Setter = (MyTerminalValueControl<MyAirVent, bool>.SetterDelegate) ((x, v) =>
      {
        x.IsDepressurizing = v;
        x.UpdateEmissivity();
      });
      onOff.EnableToggleAction<MyAirVent>();
      onOff.EnableOnOffActions<MyAirVent>();
      MyTerminalControlFactory.AddControl<MyAirVent>((MyTerminalControl<MyAirVent>) onOff);
      MyTerminalControlButton<MyAirVent> terminalControlButton = new MyTerminalControlButton<MyAirVent>("Open Toolbar", MySpaceTexts.BlockPropertyTitle_SensorToolbarOpen, MySpaceTexts.BlockPropertyDescription_SensorToolbarOpen, (Action<MyAirVent>) (self =>
      {
        if (self.m_onFullAction != null)
          self.m_actionToolbar.SetItemAtIndex(0, self.m_onFullAction);
        if (self.m_onEmptyAction != null)
          self.m_actionToolbar.SetItemAtIndex(1, self.m_onEmptyAction);
        self.m_actionToolbar.ItemChanged += new Action<MyToolbar, MyToolbar.IndexArgs, bool>(self.Toolbar_ItemChanged);
        if (MyGuiScreenToolbarConfigBase.Static != null)
          return;
        MyToolbarComponent.CurrentToolbar = self.m_actionToolbar;
        MyGuiScreenBase screen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) 0, (object) self, null);
        MyToolbarComponent.AutoUpdate = false;
        screen.Closed += (MyGuiScreenBase.ScreenHandler) ((source, isUnloading) =>
        {
          MyToolbarComponent.AutoUpdate = true;
          self.m_actionToolbar.ItemChanged -= new Action<MyToolbar, MyToolbar.IndexArgs, bool>(self.Toolbar_ItemChanged);
          self.m_actionToolbar.Clear();
        });
        MyGuiSandbox.AddScreen(screen);
      }));
      terminalControlButton.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MyAirVent>((MyTerminalControl<MyAirVent>) terminalControlButton);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_AirVent objectBuilderAirVent = (MyObjectBuilder_AirVent) objectBuilder;
      this.InitializeConveyorEndpoint();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.SourceComp.Init(this.BlockDefinition.ResourceSourceGroup, new MyResourceSourceInfo()
      {
        ResourceTypeId = this.m_oxygenGasId,
        DefinedOutput = this.BlockDefinition.VentilationCapacityPerSecond,
        ProductionToCapacityMultiplier = 1f
      });
      this.SourceComp.OutputChanged += new MyResourceOutputChangedDelegate(this.Source_OutputChanged);
      this.OxygenSinkInfo = new MyResourceSinkInfo()
      {
        ResourceTypeId = this.m_oxygenGasId,
        MaxRequiredInput = this.BlockDefinition.VentilationCapacityPerSecond,
        RequiredInputFunc = new Func<float>(this.Sink_ComputeRequiredGas)
      };
      this.ResourceSink.Init(this.BlockDefinition.ResourceSinkGroup, new List<MyResourceSinkInfo>()
      {
        new MyResourceSinkInfo()
        {
          ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
          MaxRequiredInput = this.BlockDefinition.OperationalPowerConsumption,
          RequiredInputFunc = new Func<float>(this.ComputeRequiredPower)
        }
      });
      this.ResourceSink.IsPoweredChanged += new Action(this.PowerReceiver_IsPoweredChanged);
      this.ResourceSink.CurrentInputChanged += new MyCurrentResourceInputChangedDelegate(this.Sink_CurrentInputChanged);
      this.m_actionToolbar = new MyToolbar(MyToolbarType.ButtonPanel, 2, 1);
      this.m_actionToolbar.DrawNumbers = false;
      this.m_actionToolbar.Init((MyObjectBuilder_Toolbar) null, (MyEntity) this);
      if (objectBuilderAirVent.OnFullAction != null)
        this.m_onFullAction = MyToolbarItemFactory.CreateToolbarItem(objectBuilderAirVent.OnFullAction);
      if (objectBuilderAirVent.OnEmptyAction != null)
        this.m_onEmptyAction = MyToolbarItemFactory.CreateToolbarItem(objectBuilderAirVent.OnEmptyAction);
      this.UpdateEmissivity();
      this.UpdateStatus();
      this.SetDetailedInfoDirty();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyAirVent_IsWorkingChanged);
      this.m_isDepressurizing.SetLocalValue(objectBuilderAirVent.IsDepressurizing);
      this.SetDepressurizing();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_AirVent builderCubeBlock = (MyObjectBuilder_AirVent) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.IsDepressurizing = this.IsDepressurizing;
      if (this.m_onFullAction != null)
        builderCubeBlock.OnFullAction = this.m_onFullAction.GetObjectBuilder();
      if (this.m_onEmptyAction != null)
        builderCubeBlock.OnEmptyAction = this.m_onEmptyAction.GetObjectBuilder();
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public void InitializeConveyorEndpoint() => this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.m_isProducing = this.m_producedSinceLastUpdate;
      this.m_producedSinceLastUpdate = false;
      this.ExecuteGasTransfer();
      this.UpdateStatus();
      this.UpdateEmissivity();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private void ExecuteGasTransfer()
    {
      float transferAmount = this.GasInputPerUpdate - this.GasOutputPerUpdate;
      if ((double) transferAmount != 0.0)
      {
        this.Transfer(transferAmount);
        this.SourceComp.OnProductionEnabledChanged(new MyDefinitionId?(this.m_oxygenGasId));
      }
      else
      {
        if (!this.HasDamageEffect)
          this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
        this.StopVentEffect();
      }
      this.ResourceSink.Update();
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      MyOxygenBlock oxygenBlock = this.GetOxygenBlock();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        if (this.IsWorking)
          this.UpdateActions();
        bool isRoomAirtight = oxygenBlock != null && oxygenBlock.Room != null && oxygenBlock.Room.IsAirtight;
        float roomEnvironmentOxygen = oxygenBlock == null || oxygenBlock.Room == null ? 0.0f : oxygenBlock.Room.EnvironmentOxygen;
        float oxygenLevel = oxygenBlock != null ? oxygenBlock.OxygenLevel(this.CubeGrid.GridSize) : 0.0f;
        this.m_blockRoomInfo.Value = new MyAirVentBlockRoomInfo(isRoomAirtight, oxygenLevel, roomEnvironmentOxygen);
      }
      this.SourceComp.SetRemainingCapacityByType(this.m_oxygenGasId, oxygenBlock == null || oxygenBlock.Room == null || !oxygenBlock.Room.IsAirtight ? ((double) MyOxygenProviderSystem.GetOxygenInPoint(this.WorldMatrix.Translation) != 0.0 ? this.BlockDefinition.VentilationCapacityPerSecond : 0.0f) : oxygenBlock.Room.OxygenAmount);
      this.UpdateStatus();
      this.UpdateEmissivity();
      this.SetDetailedInfoDirty();
      this.ResourceSink.Update();
      if (MyFakes.ENABLE_OXYGEN_SOUNDS)
        this.UpdateSound();
      this.m_oxygenModifier = MySession.Static.GetComponent<MySectorWeatherComponent>().GetOxygenMultiplier(this.PositionComp.GetPosition());
    }

    private void StopVentEffect()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.m_isPlayingVentEffect)
        return;
      this.m_isPlayingVentEffect = false;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyAirVent>(this, (Func<MyAirVent, Action>) (x => new Action(x.StopVentEffectImplementation)));
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      this.StopVentEffectImplementation();
    }

    [Event(null, 342)]
    [Reliable]
    [Broadcast]
    private void StopVentEffectImplementation()
    {
      this.m_isPlayingVentEffect = false;
      if (this.m_effect == null)
        return;
      this.m_effect.Stop(false);
      this.m_effect = (MyParticleEffect) null;
    }

    private void Source_OutputChanged(
      MyDefinitionId changedResourceId,
      float oldOutput,
      MyResourceSourceComponent source)
    {
      if (changedResourceId != this.m_oxygenGasId)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    private void Sink_CurrentInputChanged(
      MyDefinitionId resourceTypeId,
      float oldInput,
      MyResourceSinkComponent sink)
    {
      if (resourceTypeId != this.m_oxygenGasId)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    private void Transfer(float transferAmount)
    {
      if ((double) transferAmount > 0.0)
      {
        this.VentToRoom(transferAmount);
      }
      else
      {
        if ((double) transferAmount >= 0.0)
          return;
        this.DrainFromRoom(-transferAmount);
      }
    }

    private void UpdateSound()
    {
      if (this.m_soundEmitter == null)
        return;
      if (this.IsWorking)
      {
        if (this.m_isPlayingVentEffect)
        {
          if (this.IsDepressurizing)
          {
            if (!this.m_soundEmitter.IsPlaying || !this.m_soundEmitter.SoundPair.Equals((object) this.BlockDefinition.DepressurizeSound))
              this.m_soundEmitter.PlaySound(this.BlockDefinition.DepressurizeSound, true);
          }
          else if (!this.m_soundEmitter.IsPlaying || !this.m_soundEmitter.SoundPair.Equals((object) this.BlockDefinition.PressurizeSound))
            this.m_soundEmitter.PlaySound(this.BlockDefinition.PressurizeSound, true);
        }
        else if (!this.m_soundEmitter.IsPlaying || !this.m_soundEmitter.SoundPair.Equals((object) this.BlockDefinition.IdleSound))
        {
          if (this.m_soundEmitter.IsPlaying && (this.m_soundEmitter.SoundPair.Equals((object) this.BlockDefinition.PressurizeSound) || this.m_soundEmitter.SoundPair.Equals((object) this.BlockDefinition.DepressurizeSound)))
            this.m_soundEmitter.StopSound(false);
          this.m_soundEmitter.PlaySound(this.BlockDefinition.IdleSound, true);
        }
      }
      else if (this.m_soundEmitter.IsPlaying)
        this.m_soundEmitter.StopSound(false);
      this.m_soundEmitter.Update();
    }

    private void CreateEffect()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_isPlayingVentEffect = true;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyAirVent>(this, (Func<MyAirVent, Action>) (x => new Action(x.CreateVentEffectImplementation)));
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      this.CreateVentEffectImplementation();
    }

    [Event(null, 442)]
    [Reliable]
    [Broadcast]
    private void CreateVentEffectImplementation()
    {
      this.StopVentEffectImplementation();
      MatrixD effectMatrix = (MatrixD) ref this.PositionComp.LocalMatrixRef;
      if (this.IsDepressurizing)
      {
        effectMatrix.Left = effectMatrix.Right;
        effectMatrix.Forward = effectMatrix.Backward;
      }
      effectMatrix.Translation += this.PositionComp.LocalMatrixRef.Forward * (this.BlockDefinition.CubeSize == MyCubeSize.Large ? 1f : 0.1f);
      Vector3D zero = Vector3D.Zero;
      if (MyParticlesManager.TryCreateParticleEffect("OxyVent", ref effectMatrix, ref zero, this.Render.ParentIDs[0], out this.m_effect))
        this.m_effect.UserScale = this.BlockDefinition.CubeSize != MyCubeSize.Large ? 0.5f : 3f;
      this.m_isPlayingVentEffect = true;
    }

    private void UpdateActions()
    {
      float oxygenLevel = this.GetOxygenLevel();
      if (!this.m_wasRoomEmpty.HasValue || !this.m_wasRoomFull.HasValue)
      {
        this.m_wasRoomEmpty = new bool?(false);
        this.m_wasRoomFull = new bool?(false);
        if ((double) oxygenLevel > 0.990000009536743)
        {
          this.m_wasRoomFull = new bool?(true);
        }
        else
        {
          if ((double) oxygenLevel >= 0.00999999977648258)
            return;
          this.m_wasRoomEmpty = new bool?(true);
        }
      }
      else if ((double) oxygenLevel > 0.990000009536743)
      {
        this.m_wasRoomEmpty = new bool?(false);
        if (this.m_wasRoomFull.Value)
          return;
        this.ExecuteAction(this.m_onFullAction);
        this.m_wasRoomFull = new bool?(true);
      }
      else if ((double) oxygenLevel < 0.00999999977648258)
      {
        this.m_wasRoomFull = new bool?(false);
        if (this.m_wasRoomEmpty.Value)
          return;
        this.ExecuteAction(this.m_onEmptyAction);
        this.m_wasRoomEmpty = new bool?(true);
      }
      else
      {
        this.m_wasRoomFull = new bool?(false);
        this.m_wasRoomEmpty = new bool?(false);
      }
    }

    private void ExecuteAction(MyToolbarItem action)
    {
      this.m_actionToolbar.SetItemAtIndex(0, action);
      this.m_actionToolbar.UpdateItem(0);
      this.m_actionToolbar.ActivateItemAtSlot(0);
      this.m_actionToolbar.Clear();
    }

    private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index, bool isGamepad)
    {
      if (this.m_syncing)
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyAirVent, ToolbarItem, int>(this, (Func<MyAirVent, Action<ToolbarItem, int>>) (x => new Action<ToolbarItem, int>(x.SendToolbarItemChanged)), ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex)), index.ItemIndex);
    }

    private float ComputeRequiredPower()
    {
      if (!MySession.Static.Settings.EnableOxygen || !this.Enabled || (!this.IsFunctional || !MySession.Static.Settings.EnableOxygenPressurization))
        return 0.0f;
      return !this.m_isProducing ? this.BlockDefinition.StandbyPowerConsumption : this.BlockDefinition.OperationalPowerConsumption;
    }

    private float Sink_ComputeRequiredGas()
    {
      if (!this.CanVentToRoom)
        return 0.0f;
      MyOxygenBlock oxygenBlock = this.GetOxygenBlock();
      if (oxygenBlock == null || oxygenBlock.Room == null || !oxygenBlock.Room.IsAirtight)
        return 0.0f;
      float num = oxygenBlock.Room.MissingOxygen(this.CubeGrid.GridSize);
      if ((double) num < 9.99999974737875E-05)
      {
        oxygenBlock.Room.OxygenAmount = oxygenBlock.Room.MaxOxygen(this.CubeGrid.GridSize);
        num = 0.0f;
      }
      else
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      return Math.Min(num * 60f * this.SourceComp.ProductionToCapacityMultiplierByType(this.m_oxygenGasId) + this.SourceComp.CurrentOutputByType(this.m_oxygenGasId), this.BlockDefinition.VentilationCapacityPerSecond);
    }

    private void PowerReceiver_IsPoweredChanged() => this.UpdateIsWorking();

    private void ComponentStack_IsFunctionalChanged()
    {
      if (this.CubeGrid == null || this.SourceComp == null || (this.CubeGrid.GridSystems == null || this.CubeGrid.GridSystems.ResourceDistributor == null) || this.CubeGrid.Closed)
        return;
      this.SourceComp.Enabled = this.IsWorking;
      this.ResourceSink.Update();
      this.CubeGrid.GridSystems.ResourceDistributor.ConveyorSystem_OnPoweredChanged();
      this.UpdateEmissivity();
      this.UpdateStatus();
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.SourceComp.Enabled = this.IsWorking;
      this.ResourceSink.Update();
      this.UpdateEmissivity();
      this.UpdateStatus();
    }

    private void MyAirVent_IsWorkingChanged(MyCubeBlock obj)
    {
      this.SourceComp.Enabled = this.IsWorking;
      this.UpdateEmissivity();
      this.UpdateStatus();
    }

    private bool SetEmissiveStateForVent(MyStringHash state, float fillLevel)
    {
      int num = (int) ((double) fillLevel * (double) MyAirVent.m_emissiveTextureNames.Length);
      bool flag = false;
      if (this.Render.RenderObjectIDs[0] != uint.MaxValue && (state != this.m_prevColor || num != this.m_prevFillCount))
      {
        for (int index = 0; index < MyAirVent.m_emissiveTextureNames.Length; ++index)
          flag |= this.SetEmissiveState(index <= num ? state : MyCubeBlock.m_emissiveNames.Damaged, this.Render.RenderObjectIDs[0], MyAirVent.m_emissiveTextureNames[index]);
        this.m_prevColor = state;
        this.m_prevFillCount = num;
      }
      return flag;
    }

    public override bool SetEmissiveStateWorking() => this.UpdateEmissivity();

    public override bool SetEmissiveStateDamaged() => this.SetEmissiveStateForVent(MyCubeBlock.m_emissiveNames.Damaged, 1f);

    public override bool SetEmissiveStateDisabled() => this.SetEmissiveStateForVent(MyCubeBlock.m_emissiveNames.Disabled, 1f);

    protected override bool CheckIsWorking() => base.CheckIsWorking() && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.m_prevFillCount = -1;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.UpdateStatus();
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.UpdateStatus();
    }

    private void UpdateStatus()
    {
      MyOxygenBlock oxygenBlock = this.GetOxygenBlock();
      if (oxygenBlock == null || oxygenBlock.Room == null)
        this.Status = VentStatus.Depressurized;
      else if (oxygenBlock.Room.IsAirtight)
      {
        if ((double) oxygenBlock.Room.OxygenLevel(this.CubeGrid.GridSize) >= 1.0)
        {
          if (Sandbox.Game.Multiplayer.Sync.IsServer && MyVisualScriptLogicProvider.RoomFullyPressurized != null && this.Status != VentStatus.Pressurized)
            MyVisualScriptLogicProvider.RoomFullyPressurized(this.EntityId, this.CubeGrid.EntityId, this.Name, this.CubeGrid.Name);
          this.Status = VentStatus.Pressurized;
        }
        else
          this.Status = this.IsDepressurizing ? VentStatus.Depressurizing : VentStatus.Pressurizing;
      }
      else
        this.Status = (double) oxygenBlock.Room.OxygenLevel(this.CubeGrid.GridSize) <= 0.00999999977648258 ? VentStatus.Depressurized : VentStatus.Depressurizing;
      this.CheckEmissiveState();
    }

    public bool IsRoomAirtight()
    {
      MyOxygenBlock oxygenBlock = this.GetOxygenBlock();
      return (oxygenBlock == null ? 0 : (oxygenBlock.Room != null ? 1 : 0)) != 0 && oxygenBlock.Room.IsAirtight;
    }

    private bool UpdateEmissivity()
    {
      if (!this.IsWorking)
        return false;
      MyOxygenBlock oxygenBlock = this.GetOxygenBlock();
      bool flag1 = oxygenBlock != null && oxygenBlock.Room != null;
      bool flag2 = flag1 && oxygenBlock.Room.IsAirtight;
      float val2 = flag1 ? oxygenBlock.Room.EnvironmentOxygen : 0.0f;
      float num = flag1 ? oxygenBlock.OxygenLevel(this.CubeGrid.GridSize) : 0.0f;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        flag2 = this.m_blockRoomInfo.Value.IsRoomAirtight;
        val2 = this.m_blockRoomInfo.Value.RoomEnvironmentOxygen;
        num = this.m_blockRoomInfo.Value.OxygenLevel;
        flag1 = true;
      }
      if (flag1)
      {
        if (flag2)
          return this.SetEmissiveStateForVent(this.IsDepressurizing ? MyCubeBlock.m_emissiveNames.Alternative : MyCubeBlock.m_emissiveNames.Working, num);
        MyStringHash state = MyCubeBlock.m_emissiveNames.Warning;
        if (this.IsDepressurizing && (double) MyOxygenProviderSystem.GetOxygenInPoint(this.WorldMatrix.Translation) > 0.0)
          state = MyCubeBlock.m_emissiveNames.Alternative;
        return this.SetEmissiveStateForVent(state, Math.Max(num, val2));
      }
      float fillLevel = (float) (int) ((double) MyOxygenProviderSystem.GetOxygenInPoint(this.WorldMatrix.Translation) * (double) MyAirVent.m_emissiveTextureNames.Length);
      return this.SetEmissiveStateForVent((double) fillLevel == 0.0 ? MyCubeBlock.m_emissiveNames.Warning : (this.IsDepressurizing ? MyCubeBlock.m_emissiveNames.Alternative : MyCubeBlock.m_emissiveNames.Working), fillLevel);
    }

    private void SetDepressurizing()
    {
      this.StopVentEffect();
      if (this.IsDepressurizing)
      {
        MyDefinitionId oxygenGasId = this.m_oxygenGasId;
        this.ResourceSink.RemoveType(ref oxygenGasId);
      }
      else
        this.ResourceSink.AddType(ref this.OxygenSinkInfo);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      this.SourceComp.SetProductionEnabledByType(this.m_oxygenGasId, this.IsDepressurizing);
      this.ResourceSink.Update();
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
      if (!MySession.Static.Settings.EnableOxygen || !MySession.Static.Settings.EnableOxygenPressurization)
      {
        detailedInfo.Append((object) MyTexts.Get(MySpaceTexts.Oxygen_Disabled));
      }
      else
      {
        MyOxygenBlock oxygenBlock = this.GetOxygenBlock();
        bool flag1 = oxygenBlock != null && oxygenBlock.Room != null;
        bool flag2 = flag1 && oxygenBlock.Room.IsAirtight;
        float num = flag1 ? oxygenBlock.OxygenLevel(this.CubeGrid.GridSize) : 0.0f;
        if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        {
          flag2 = this.m_blockRoomInfo.Value.IsRoomAirtight;
          num = this.m_blockRoomInfo.Value.OxygenLevel;
          flag1 = true;
        }
        if (!flag1 || !flag2)
          detailedInfo.Append((object) MyTexts.Get(MySpaceTexts.Oxygen_NotPressurized));
        else
          detailedInfo.Append(MyTexts.Get(MySpaceTexts.Oxygen_Pressure).ToString() + (num * 100f).ToString("F") + "%");
      }
    }

    protected override void Closing()
    {
      base.Closing();
      this.StopVentEffect();
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.StopSound(true);
    }

    private MyOxygenBlock GetOxygenBlock() => !MySession.Static.Settings.EnableOxygen || !MySession.Static.Settings.EnableOxygenPressurization || (this.VentDummy == null || this.CubeGrid.GridSystems.GasSystem == null) ? new MyOxygenBlock() : this.CubeGrid.GridSystems.GasSystem.GetOxygenBlock(MatrixD.Multiply(MatrixD.Normalize((MatrixD) ref this.VentDummy.Matrix), this.WorldMatrix).Translation);

    bool IMyGasBlock.IsWorking() => this.CanVentToRoom;

    private void VentToRoom(float amount)
    {
      if ((double) amount == 0.0 || this.IsDepressurizing)
        return;
      MyOxygenBlock oxygenBlock = this.GetOxygenBlock();
      if (oxygenBlock == null || oxygenBlock.Room == null || !oxygenBlock.Room.IsAirtight)
        return;
      oxygenBlock.Room.OxygenAmount += amount;
      if ((double) oxygenBlock.Room.OxygenLevel(this.CubeGrid.GridSize) > 1.0)
        oxygenBlock.Room.OxygenAmount = oxygenBlock.Room.MaxOxygen(this.CubeGrid.GridSize);
      this.ResourceSink.Update();
      this.SourceComp.SetRemainingCapacityByType(this.m_oxygenGasId, oxygenBlock.Room.OxygenAmount);
      this.CheckForVentEffect(amount);
    }

    private void DrainFromRoom(float amount)
    {
      if ((double) amount == 0.0 || !this.IsDepressurizing)
        return;
      MyOxygenBlock oxygenBlock = this.GetOxygenBlock();
      if (oxygenBlock == null || oxygenBlock.Room == null)
        return;
      double oxygenAmount = (double) oxygenBlock.Room.OxygenAmount;
      if (oxygenBlock.Room.IsAirtight)
      {
        oxygenBlock.Room.OxygenAmount -= amount;
        if ((double) oxygenBlock.Room.OxygenAmount < 0.0)
          oxygenBlock.Room.OxygenAmount = 0.0f;
        this.SourceComp.SetRemainingCapacityByType(this.m_oxygenGasId, oxygenBlock.Room.OxygenAmount);
      }
      else
      {
        this.SourceComp.SetRemainingCapacityByType(this.m_oxygenGasId, (double) MyOxygenProviderSystem.GetOxygenInPoint(this.WorldMatrix.Translation) * (double) this.m_oxygenModifier != 0.0 ? this.BlockDefinition.VentilationCapacityPerSecond * 100f : 0.0f);
        this.m_producedSinceLastUpdate = true;
      }
      this.ResourceSink.Update();
      this.CheckForVentEffect(amount);
    }

    private void CheckForVentEffect(float amount)
    {
      if ((double) amount <= 0.0)
        return;
      this.m_producedSinceLastUpdate = true;
      if (this.Status != VentStatus.Pressurizing && this.Status != VentStatus.Depressurizing || this.m_isPlayingVentEffect)
        return;
      this.CreateEffect();
    }

    public bool IsPressurized() => this.CanPressurize;

    public bool CanPressurize
    {
      get
      {
        MyOxygenBlock oxygenBlock = this.GetOxygenBlock();
        return oxygenBlock != null && oxygenBlock.Room != null && oxygenBlock.Room.IsAirtight;
      }
    }

    public float GetOxygenLevel()
    {
      if (this.IsWorking)
      {
        MyOxygenBlock oxygenBlock = this.GetOxygenBlock();
        if (oxygenBlock != null && oxygenBlock.Room != null)
        {
          float num = oxygenBlock.OxygenLevel(this.CubeGrid.GridSize);
          return oxygenBlock.Room.IsAirtight ? num : oxygenBlock.Room.EnvironmentOxygen;
        }
      }
      return 0.0f;
    }

    [Event(null, 983)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void SendToolbarItemChanged(ToolbarItem sentItem, int index)
    {
      this.m_syncing = true;
      MyToolbarItem myToolbarItem = (MyToolbarItem) null;
      if (sentItem.EntityID != 0L)
      {
        if (string.IsNullOrEmpty(sentItem.GroupName))
        {
          MyTerminalBlock entity;
          if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyTerminalBlock>(sentItem.EntityID, out entity))
          {
            MyObjectBuilder_ToolbarItemTerminalBlock itemTerminalBlock = MyToolbarItemFactory.TerminalBlockObjectBuilderFromBlock(entity);
            itemTerminalBlock._Action = sentItem.Action;
            itemTerminalBlock.Parameters = sentItem.Parameters;
            myToolbarItem = MyToolbarItemFactory.CreateToolbarItem((MyObjectBuilder_ToolbarItem) itemTerminalBlock);
          }
        }
        else
        {
          MyCubeGrid cubeGrid = this.CubeGrid;
          string groupName = sentItem.GroupName;
          MyBlockGroup group = cubeGrid.GridSystems.TerminalSystem.BlockGroups.Find((Predicate<MyBlockGroup>) (x => x.Name.ToString() == groupName));
          if (group != null)
          {
            MyObjectBuilder_ToolbarItemTerminalGroup itemTerminalGroup = MyToolbarItemFactory.TerminalGroupObjectBuilderFromGroup(group);
            itemTerminalGroup._Action = sentItem.Action;
            itemTerminalGroup.BlockEntityId = sentItem.EntityID;
            itemTerminalGroup.Parameters = sentItem.Parameters;
            myToolbarItem = MyToolbarItemFactory.CreateToolbarItem((MyObjectBuilder_ToolbarItem) itemTerminalGroup);
          }
        }
      }
      if (index == 0)
        this.m_onFullAction = myToolbarItem;
      else
        this.m_onEmptyAction = myToolbarItem;
      this.RaisePropertiesChanged();
      this.m_syncing = false;
    }

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    MyResourceSinkInfo SpaceEngineers.Game.ModAPI.IMyAirVent.OxygenSinkInfo
    {
      get => this.OxygenSinkInfo;
      set => this.OxygenSinkInfo = value;
    }

    MyResourceSourceComponent SpaceEngineers.Game.ModAPI.IMyAirVent.SourceComp
    {
      get => this.SourceComp;
      set => this.SourceComp = value;
    }

    float SpaceEngineers.Game.ModAPI.IMyAirVent.GasOutputPerSecond => this.GasOutputPerSecond;

    float SpaceEngineers.Game.ModAPI.IMyAirVent.GasInputPerSecond => this.GasInputPerSecond;

    VentStatus SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent.Status => this.Status;

    bool SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent.Depressurize
    {
      get => this.IsDepressurizing;
      set => this.IsDepressurizing = value;
    }

    bool SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent.PressurizationEnabled => MySession.Static.Settings.EnableOxygenPressurization;

    protected sealed class StopVentEffectImplementation\u003C\u003E : ICallSite<MyAirVent, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyAirVent @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.StopVentEffectImplementation();
      }
    }

    protected sealed class CreateVentEffectImplementation\u003C\u003E : ICallSite<MyAirVent, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyAirVent @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.CreateVentEffectImplementation();
      }
    }

    protected sealed class SendToolbarItemChanged\u003C\u003ESandbox_Game_Entities_Blocks_ToolbarItem\u0023System_Int32 : ICallSite<MyAirVent, ToolbarItem, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyAirVent @this,
        in ToolbarItem sentItem,
        in int index,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SendToolbarItemChanged(sentItem, index);
      }
    }

    protected class m_isDepressurizing\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyAirVent) obj0).m_isDepressurizing = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_blockRoomInfo\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyAirVentBlockRoomInfo, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyAirVentBlockRoomInfo, SyncDirection.FromServer>(obj1, obj2));
        ((MyAirVent) obj0).m_blockRoomInfo = (VRage.Sync.Sync<MyAirVentBlockRoomInfo, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
