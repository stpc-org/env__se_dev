// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyShipConnector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_ShipConnector))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyShipConnector), typeof (Sandbox.ModAPI.Ingame.IMyShipConnector)})]
  public class MyShipConnector : MyFunctionalBlock, IMyInventoryOwner, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyShipConnector, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyShipConnector
  {
    private readonly uint TIMER_NORMAL_IN_FRAMES = 80;
    private readonly uint TIMER_TIER1_IN_FRAMES = 160;
    private readonly uint TIMER_TIER2_IN_FRAMES = 320;
    private static readonly MyTimeSpan DisconnectSleepTime = MyTimeSpan.FromSeconds(4.0);
    private const float MinStrength = 1E-06f;
    public readonly VRage.Sync.Sync<bool, SyncDirection.FromServer> TradingEnabled;
    public readonly VRage.Sync.Sync<int, SyncDirection.FromServer> TimeOfConnection;
    public readonly VRage.Sync.Sync<float, SyncDirection.BothWays> AutoUnlockTime;
    public readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> ThrowOut;
    public readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> CollectAll;
    public readonly VRage.Sync.Sync<float, SyncDirection.BothWays> Strength;
    private readonly VRage.Sync.Sync<MyShipConnector.State, SyncDirection.FromServer> m_connectionState;
    private MyAttachableConveyorEndpoint m_attachableConveyorEndpoint;
    private int m_update10Counter;
    private bool m_canReloadDummies = true;
    private Vector3 m_connectionPosition;
    private float m_detectorRadius;
    private static readonly int TRADING_FRAMES_TO_WAIT_AFTER_DISCONNECT = 9;
    private int m_tradingBlockTimer;
    private HkConstraint m_constraint;
    private MyShipConnector m_other;
    private bool m_defferedDisconnect;
    private static HashSet<MySlimBlock> m_tmpBlockSet = new HashSet<MySlimBlock>();
    private int m_manualDisconnectTime;
    private MyPhysicsBody m_connectorDummy;
    private MyShipConnector.Mode m_connectorMode;
    private bool m_hasConstraint;
    private MyConcurrentHashSet<MyEntity> m_detectedFloaters = new MyConcurrentHashSet<MyEntity>();
    private MyConcurrentHashSet<MyEntity> m_detectedGrids = new MyConcurrentHashSet<MyEntity>();
    protected HkConstraint m_connectorConstraint;
    protected HkFixedConstraintData m_connectorConstraintsData;
    protected HkConstraint m_ejectorConstraint;
    protected HkFixedConstraintData m_ejectorConstraintsData;
    private Matrix m_connectorDummyLocal;
    private Vector3 m_connectorCenter;
    private Vector3 m_connectorHalfExtents;
    private bool m_isMaster;
    private bool m_welded;
    private bool m_welding;
    private bool m_isInitOnceBeforeFrameUpdate;
    private long? m_lastAttachedOther;
    private long? m_lastWeldedOther;

    public MyShipConnector Other => this.m_other;

    private MyShipConnectorDefinition ConnectorDefinition => this.BlockDefinition as MyShipConnectorDefinition;

    private bool IsMaster
    {
      get => !Sandbox.Game.Multiplayer.Sync.IsServer ? this.m_connectionState.Value.IsMaster : this.m_isMaster;
      set => this.m_isMaster = value;
    }

    public bool IsReleasing => (double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_manualDisconnectTime) < MyShipConnector.DisconnectSleepTime.Milliseconds;

    public bool InConstraint => (HkReferenceObject) this.m_constraint != (HkReferenceObject) null;

    public bool Connected { get; set; }

    private Vector3 ConnectionPosition => Vector3.Transform(this.m_connectionPosition, this.PositionComp.LocalMatrixRef);

    public int DetectedGridCount => this.m_detectedGrids.Count;

    public override bool IsTieredUpdateSupported => true;

    public MyShipConnector()
    {
      this.CreateTerminalControls();
      this.m_connectionState.ValueChanged += (Action<SyncBase>) (o => this.OnConnectionStateChanged());
      this.m_connectionState.AlwaysReject<MyShipConnector.State, SyncDirection.FromServer>();
      this.m_manualDisconnectTime = -(int) MyShipConnector.DisconnectSleepTime.Milliseconds;
      this.Strength.Validate = (SyncValidate<float>) (o => (double) (float) this.Strength >= 0.0 && (double) (float) this.Strength <= 1.0);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.NeedsWorldMatrix = true;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyShipConnector>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MyShipConnector> onOff1 = new MyTerminalControlOnOffSwitch<MyShipConnector>("ThrowOut", MySpaceTexts.Terminal_ThrowOut);
      onOff1.Getter = (MyTerminalValueControl<MyShipConnector, bool>.GetterDelegate) (block => (bool) block.ThrowOut);
      onOff1.Setter = (MyTerminalValueControl<MyShipConnector, bool>.SetterDelegate) ((block, value) => block.ThrowOut.Value = value);
      onOff1.EnableToggleAction<MyShipConnector>();
      MyTerminalControlFactory.AddControl<MyShipConnector>((MyTerminalControl<MyShipConnector>) onOff1);
      MyTerminalControlOnOffSwitch<MyShipConnector> onOff2 = new MyTerminalControlOnOffSwitch<MyShipConnector>("CollectAll", MySpaceTexts.Terminal_CollectAll);
      onOff2.Getter = (MyTerminalValueControl<MyShipConnector, bool>.GetterDelegate) (block => (bool) block.CollectAll);
      onOff2.Setter = (MyTerminalValueControl<MyShipConnector, bool>.SetterDelegate) ((block, value) => block.CollectAll.Value = value);
      onOff2.EnableToggleAction<MyShipConnector>();
      MyTerminalControlFactory.AddControl<MyShipConnector>((MyTerminalControl<MyShipConnector>) onOff2);
      MyTerminalControlOnOffSwitch<MyShipConnector> onOff3 = new MyTerminalControlOnOffSwitch<MyShipConnector>("Trading", MySpaceTexts.Terminal_Trading, MySpaceTexts.Terminal_Trading_Tooltip);
      onOff3.Getter = (MyTerminalValueControl<MyShipConnector, bool>.GetterDelegate) (block => (bool) block.TradingEnabled);
      onOff3.Setter = new MyTerminalValueControl<MyShipConnector, bool>.SetterDelegate(MyShipConnector.TradingEnabled_UIChanged);
      onOff3.Enabled = (Func<MyShipConnector, bool>) (block => !block.Connected && block.m_connectorMode == MyShipConnector.Mode.Connector);
      onOff3.Visible = (Func<MyShipConnector, bool>) (b => b.m_connectorMode == MyShipConnector.Mode.Connector);
      onOff3.EnableToggleAction<MyShipConnector>();
      MyTerminalControlFactory.AddControl<MyShipConnector>((MyTerminalControl<MyShipConnector>) onOff3);
      MyTerminalControlSlider<MyShipConnector> slider1 = new MyTerminalControlSlider<MyShipConnector>("AutoUnlockTime", MySpaceTexts.BlockPropertyTitle_Connector_AutoUnlockTime, MySpaceTexts.BlockPropertyDescription_Connector_AutoUnlockTime, true, true);
      slider1.Getter = (MyTerminalValueControl<MyShipConnector, float>.GetterDelegate) (x => (float) x.AutoUnlockTime);
      slider1.Setter = (MyTerminalValueControl<MyShipConnector, float>.SetterDelegate) ((x, v) => x.AutoUnlockTime.Value = v);
      slider1.DefaultValue = new float?(0.0f);
      slider1.EnableActions<MyShipConnector>(enabled: ((Func<MyShipConnector, bool>) (b => b.m_connectorMode == MyShipConnector.Mode.Connector)));
      slider1.Enabled = (Func<MyShipConnector, bool>) (b => b.m_connectorMode == MyShipConnector.Mode.Connector);
      slider1.Visible = (Func<MyShipConnector, bool>) (b => b.m_connectorMode == MyShipConnector.Mode.Connector);
      slider1.SetLimits((MyTerminalValueControl<MyShipConnector, float>.GetterDelegate) (x => x.ConnectorDefinition == null ? 0.0f : x.ConnectorDefinition.AutoUnlockTime_Min), (MyTerminalValueControl<MyShipConnector, float>.GetterDelegate) (x => x.ConnectorDefinition == null ? 3600f : x.ConnectorDefinition.AutoUnlockTime_Max));
      slider1.Writer = (MyTerminalControl<MyShipConnector>.WriterDelegate) ((x, result) =>
      {
        int autoUnlockTime = (int) (float) x.AutoUnlockTime;
        if (autoUnlockTime == 0)
          result.Append("Never");
        else
          MyValueFormatter.AppendTimeExact(autoUnlockTime, result);
      });
      MyTerminalControlFactory.AddControl<MyShipConnector>((MyTerminalControl<MyShipConnector>) slider1);
      MyTerminalControlButton<MyShipConnector> button1 = new MyTerminalControlButton<MyShipConnector>("Lock", MySpaceTexts.BlockActionTitle_Lock, MySpaceTexts.Blank, (Action<MyShipConnector>) (b => b.TryConnect()));
      button1.Enabled = (Func<MyShipConnector, bool>) (b => b.IsWorking && b.InConstraint);
      button1.Visible = (Func<MyShipConnector, bool>) (b => b.m_connectorMode == MyShipConnector.Mode.Connector);
      button1.EnableAction<MyShipConnector>().Enabled = (Func<MyShipConnector, bool>) (b => b.m_connectorMode == MyShipConnector.Mode.Connector);
      MyTerminalControlFactory.AddControl<MyShipConnector>((MyTerminalControl<MyShipConnector>) button1);
      MyTerminalControlButton<MyShipConnector> button2 = new MyTerminalControlButton<MyShipConnector>("Unlock", MySpaceTexts.BlockActionTitle_Unlock, MySpaceTexts.Blank, (Action<MyShipConnector>) (b => b.TryDisconnect()));
      button2.Enabled = (Func<MyShipConnector, bool>) (b => b.IsWorking && b.InConstraint);
      button2.Visible = (Func<MyShipConnector, bool>) (b => b.m_connectorMode == MyShipConnector.Mode.Connector);
      button2.EnableAction<MyShipConnector>().Enabled = (Func<MyShipConnector, bool>) (b => b.m_connectorMode == MyShipConnector.Mode.Connector);
      MyTerminalControlFactory.AddControl<MyShipConnector>((MyTerminalControl<MyShipConnector>) button2);
      MyTerminalControlFactory.AddAction<MyShipConnector>(new MyTerminalAction<MyShipConnector>("SwitchLock", MyTexts.Get(MySpaceTexts.BlockActionTitle_SwitchLock), MyTerminalActionIcons.TOGGLE)
      {
        Action = (Action<MyShipConnector>) (b => b.TrySwitch()),
        Writer = (MyTerminalControl<MyShipConnector>.WriterDelegate) ((b, sb) => b.WriteLockStateValue(sb)),
        Enabled = (Func<MyShipConnector, bool>) (b => b.m_connectorMode == MyShipConnector.Mode.Connector)
      });
      MyTerminalControlSlider<MyShipConnector> slider2 = new MyTerminalControlSlider<MyShipConnector>("Strength", MySpaceTexts.BlockPropertyTitle_Connector_Strength, MySpaceTexts.BlockPropertyDescription_Connector_Strength);
      slider2.Getter = (MyTerminalValueControl<MyShipConnector, float>.GetterDelegate) (x => (float) x.Strength * 100f);
      slider2.Setter = (MyTerminalValueControl<MyShipConnector, float>.SetterDelegate) ((x, v) => x.Strength.Value = v * 0.01f);
      slider2.DefaultValue = new float?(0.00015f);
      slider2.SetLogLimits(1E-06f, 1f);
      slider2.EnableActions<MyShipConnector>(enabled: ((Func<MyShipConnector, bool>) (b => b.m_connectorMode == MyShipConnector.Mode.Connector)));
      slider2.Enabled = (Func<MyShipConnector, bool>) (b => b.m_connectorMode == MyShipConnector.Mode.Connector);
      slider2.Visible = (Func<MyShipConnector, bool>) (b => b.m_connectorMode == MyShipConnector.Mode.Connector);
      slider2.SetLimits((MyTerminalValueControl<MyShipConnector, float>.GetterDelegate) (x => 0.0f), (MyTerminalValueControl<MyShipConnector, float>.GetterDelegate) (x => 100f));
      slider2.Writer = (MyTerminalControl<MyShipConnector>.WriterDelegate) ((x, result) =>
      {
        if ((double) (float) x.Strength <= 9.99999997475243E-07)
          result.Append((object) MyTexts.Get(MyCommonTexts.Disabled));
        else
          result.AppendFormatedDecimal("", (float) x.Strength * 100f, 4, " %");
      });
      MyTerminalControlFactory.AddControl<MyShipConnector>((MyTerminalControl<MyShipConnector>) slider2);
    }

    private static void TradingEnabled_UIChanged(MyShipConnector block, bool value) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipConnector, bool>(block, (Func<MyShipConnector, Action<bool>>) (x => new Action<bool>(x.TradingEnabled_RequestChange)), value);

    [Event(null, 274)]
    [Reliable]
    [Server]
    public void TradingEnabled_RequestChange(bool value)
    {
      if (this.Connected)
        return;
      this.TradingEnabled.ValidateAndSet(value);
    }

    private void OnConnectionStateChanged()
    {
      this.RaisePropertiesChanged();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (!this.Connected && !this.InConstraint || !this.m_connectionState.Value.MasterToSlave.HasValue)
        return;
      this.Detach(false);
    }

    public void WriteLockStateValue(StringBuilder sb)
    {
      if (this.InConstraint && this.Connected)
        sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_Locked));
      else if (this.InConstraint)
        sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_ReadyToLock));
      else
        sb.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyValue_Unlocked));
    }

    public void TrySwitch()
    {
      if (!this.InConstraint)
        return;
      if (this.Connected)
        this.TryDisconnect();
      else
        this.TryConnect();
    }

    [Event(null, 319)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    public void TryConnect()
    {
      if (!this.InConstraint || this.Connected)
        return;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        if (this.m_tradingBlockTimer > 0)
          return;
        this.Weld();
      }
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipConnector>(this, (Func<MyShipConnector, Action>) (x => new Action(x.TryConnect)));
    }

    public bool IsProtectedFromLockingByTrading() => this.m_tradingBlockTimer > 0;

    public int GetProtectionFromLockingTime() => (int) (100.0 * (double) this.m_tradingBlockTimer / 60.0);

    [Event(null, 344)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    public void TryDisconnect()
    {
      if (!this.InConstraint || !this.Connected)
        return;
      this.m_manualDisconnectTime = this.m_other.m_manualDisconnectTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        bool flag = false;
        if (this.TradingEnabled.Value || this.Other.TradingEnabled.Value)
        {
          this.SetTradingProtection();
          this.Other.SetTradingProtection();
          flag = true;
        }
        this.Detach();
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipConnector, bool>(this, (Func<MyShipConnector, Action<bool>>) (x => new Action<bool>(x.NotifyDisconnectTime)), flag);
      }
      else
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipConnector>(this, (Func<MyShipConnector, Action>) (x => new Action(x.TryDisconnect)));
    }

    private void SetTradingProtection() => this.m_tradingBlockTimer = MyShipConnector.TRADING_FRAMES_TO_WAIT_AFTER_DISCONNECT;

    [Event(null, 374)]
    [Reliable]
    [Broadcast]
    public void NotifyDisconnectTime(bool setTradingProtection = false)
    {
      if (setTradingProtection)
        this.SetTradingProtection();
      if (this.m_other == null)
        return;
      if (setTradingProtection)
        this.Other.SetTradingProtection();
      this.m_manualDisconnectTime = this.m_other.m_manualDisconnectTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    protected float GetEffectiveStrength(MyShipConnector otherConnector)
    {
      float num = 0.0f;
      if (!this.IsReleasing && this.m_tradingBlockTimer <= 0)
      {
        num = Math.Min((float) this.Strength, (float) otherConnector.Strength);
        if ((double) num < 9.99999997475243E-07)
          num = 1E-06f;
      }
      return num;
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      float maxRequiredInput = 1f / 1000f;
      if (cubeGrid.GridSizeEnum == MyCubeSize.Small)
        maxRequiredInput *= 0.01f;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(MyStringHash.GetOrCompute("Conveyors"), maxRequiredInput, (Func<float>) (() => !base.CheckIsWorking() ? 0.0f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_ShipConnector builderShipConnector = objectBuilder as MyObjectBuilder_ShipConnector;
      Vector3 size = this.BlockDefinition.Size * this.CubeGrid.GridSize * 0.8f;
      if (MyFakes.ENABLE_INVENTORY_FIX)
        this.FixSingleInventory();
      if (MyEntityExtensions.GetInventory(this) == null)
      {
        MyInventory myInventory = new MyInventory(size.Volume, size, MyInventoryFlags.CanReceive | MyInventoryFlags.CanSend);
        this.Components.Add<MyInventoryBase>((MyInventoryBase) myInventory);
        myInventory.Init(builderShipConnector.Inventory);
      }
      this.ThrowOut.SetLocalValue(builderShipConnector.ThrowOut);
      this.CollectAll.SetLocalValue(builderShipConnector.CollectAll);
      this.TradingEnabled.SetLocalValue(builderShipConnector.TradingEnabled);
      this.AutoUnlockTime.SetLocalValue(builderShipConnector.AutoUnlockTime);
      this.TimeOfConnection.SetLocalValue(builderShipConnector.TimeOfConnection);
      this.SlimBlock.DeformationRatio = builderShipConnector.DeformationRatio;
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.UpdateReceiver);
      this.EnabledChanged += new Action<MyTerminalBlock>(this.UpdateReceiver);
      this.ResourceSink.Update();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      if (this.CubeGrid.CreatePhysics)
        this.LoadDummies();
      this.Strength.SetLocalValue(MathHelper.Clamp(builderShipConnector.Strength, 0.0f, 1f));
      if (builderShipConnector.ConnectedEntityId != 0L)
      {
        MyDeltaTransform? nullable = builderShipConnector.MasterToSlaveTransform.HasValue ? new MyDeltaTransform?((MyDeltaTransform) builderShipConnector.MasterToSlaveTransform.Value) : new MyDeltaTransform?();
        if (builderShipConnector.Connected)
          nullable = new MyDeltaTransform?(new MyDeltaTransform());
        if (!builderShipConnector.IsMaster.HasValue)
          builderShipConnector.IsMaster = new bool?(builderShipConnector.ConnectedEntityId < this.EntityId);
        this.IsMaster = builderShipConnector.IsMaster.Value;
        this.m_connectionState.SetLocalValue(new MyShipConnector.State()
        {
          IsMaster = builderShipConnector.IsMaster.Value,
          OtherEntityId = builderShipConnector.ConnectedEntityId,
          MasterToSlave = nullable,
          MasterToSlaveGrid = builderShipConnector.MasterToSlaveGrid
        });
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        this.m_isInitOnceBeforeFrameUpdate = true;
      }
      if (this.BlockDefinition.EmissiveColorPreset == MyStringHash.NullOrEmpty)
        this.BlockDefinition.EmissiveColorPreset = MyStringHash.GetOrCompute("ConnectBlock");
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyShipConnector_IsWorkingChanged);
      this.OnPhysicsChanged += new Action<MyEntity>(this.MyShipConnector_OnPhysicsChanged);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderCompoonentShipConnector(this));
      this.CreateUpdateTimer(this.GetTimerTime(0), MyTimerTypes.Frame10);
    }

    private void MyShipConnector_OnPhysicsChanged(MyEntity obj)
    {
      if (this.MarkedForClose || !this.CubeGrid.CreatePhysics || (this.m_connectorMode != MyShipConnector.Mode.Connector || !this.m_canReloadDummies))
        return;
      this.LoadDummies(true);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyShipConnector.State state = this.m_connectionState.Value;
      MyObjectBuilder_ShipConnector builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_ShipConnector;
      builderCubeBlock.ThrowOut = (bool) this.ThrowOut;
      builderCubeBlock.TradingEnabled = (bool) this.TradingEnabled;
      builderCubeBlock.AutoUnlockTime = (float) this.AutoUnlockTime;
      builderCubeBlock.TimeOfConnection = (int) this.TimeOfConnection;
      builderCubeBlock.CollectAll = (bool) this.CollectAll;
      builderCubeBlock.Strength = (float) this.Strength;
      builderCubeBlock.ConnectedEntityId = state.OtherEntityId;
      builderCubeBlock.IsMaster = new bool?(state.IsMaster);
      builderCubeBlock.MasterToSlaveTransform = state.MasterToSlave.HasValue ? new MyPositionAndOrientation?((MyPositionAndOrientation) state.MasterToSlave.Value) : new MyPositionAndOrientation?();
      builderCubeBlock.MasterToSlaveGrid = state.MasterToSlaveGrid;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      if (!this.Connected)
        return;
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_ConnectorDetail_Part1));
      MyValueFormatter.AppendTimeExact((MySession.Static.GameplayFrameCounter - (int) this.TimeOfConnection) / 60, detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_ConnectorDetail_Part2));
      float num = Math.Min((double) (float) this.AutoUnlockTime == 0.0 ? float.MaxValue : (float) this.AutoUnlockTime, (double) (float) this.m_other.AutoUnlockTime == 0.0 ? float.MaxValue : (float) this.m_other.AutoUnlockTime);
      if ((double) num == 3.40282346638529E+38)
        detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_ConnectorDetail_Part3));
      else
        MyValueFormatter.AppendTimeExact((int) num, detailedInfo);
    }

    protected override void OnInventoryComponentAdded(MyInventoryBase inventory) => base.OnInventoryComponentAdded(inventory);

    protected override void OnInventoryComponentRemoved(MyInventoryBase inventory) => base.OnInventoryComponentRemoved(inventory);

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (Sandbox.Game.Multiplayer.Sync.IsServer && this.Connected && (!this.IsFunctional || !this.Enabled))
        this.m_connectionState.Value = !this.IsMaster ? MyShipConnector.State.Detached : MyShipConnector.State.DetachedMaster;
      this.DisposeBodyConstraint(ref this.m_connectorConstraint, ref this.m_connectorConstraintsData);
      this.DisposeBodyConstraint(ref this.m_ejectorConstraint, ref this.m_ejectorConstraintsData);
      if (this.Physics != null)
        this.Physics.Enabled = true;
      if (this.m_connectorDummy != null)
      {
        this.m_connectorDummy.Close();
        this.m_connectorDummy = this.CreatePhysicsBody(MyShipConnector.Mode.Connector, ref this.m_connectorDummyLocal, ref this.m_connectorCenter, ref this.m_connectorHalfExtents);
      }
      this.CreateBodyConstraint();
      this.UpdateConnectionState();
      this.TryReattachAfterMerge();
      this.RaisePropertiesChanged();
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (Sandbox.Game.Multiplayer.Sync.IsServer && this.Connected && ((double) (float) this.AutoUnlockTime > 0.0 && (double) (int) this.TimeOfConnection + 60.0 * (double) (float) this.AutoUnlockTime <= (double) MySession.Static.GameplayFrameCounter))
        this.TryDisconnect();
      --this.m_tradingBlockTimer;
      if (this.m_tradingBlockTimer == 0)
        this.SetEmissiveStateWorking();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private void TryReattachAfterMerge()
    {
      if (this.Enabled && !this.InConstraint && (!this.m_connectionState.Value.MasterToSlave.HasValue && this.m_lastAttachedOther.HasValue))
      {
        this.TryAttach(this.m_lastAttachedOther);
        if (this.m_lastWeldedOther.HasValue)
          this.TryConnect();
      }
      this.m_lastAttachedOther = new long?();
      this.m_lastWeldedOther = new long?();
    }

    private void CreateBodyConstraint()
    {
      if (this.m_connectorDummy != null)
      {
        this.m_canReloadDummies = false;
        this.m_connectorDummy.Enabled = true;
        this.m_canReloadDummies = true;
        this.CreateBodyConstraint(this.m_connectorDummy, out this.m_connectorConstraintsData, out this.m_connectorConstraint);
        this.CubeGrid.Physics.AddConstraint(this.m_connectorConstraint);
      }
      if (this.Physics != null)
      {
        this.CreateBodyConstraint(this.Physics, out this.m_ejectorConstraintsData, out this.m_ejectorConstraint);
        this.CubeGrid.Physics.AddConstraint(this.m_ejectorConstraint);
      }
      this.CubeGrid.OnPhysicsChanged -= new Action<MyEntity>(this.CubeGrid_OnBodyPhysicsChanged);
      this.CubeGrid.OnPhysicsChanged += new Action<MyEntity>(this.CubeGrid_OnBodyPhysicsChanged);
      this.CubeGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      this.CubeGrid.OnHavokSystemIDChanged += new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      if (this.CubeGrid.Physics == null)
        return;
      this.UpdateHavokCollisionSystemID(this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
    }

    internal void UpdateHavokCollisionSystemID(int HavokCollisionSystemID)
    {
      if (this.m_connectorDummy != null)
      {
        this.m_connectorDummy.RigidBody.SetCollisionFilterInfo(HkGroupFilter.CalcFilterInfo(24, HavokCollisionSystemID, 1, 1));
        if (this.m_connectorDummy.HavokWorld != null && this.m_connectorDummy.IsInWorld)
          MyPhysics.RefreshCollisionFilter(this.m_connectorDummy);
      }
      if (this.Physics == null)
        return;
      this.Physics.RigidBody.SetCollisionFilterInfo(HkGroupFilter.CalcFilterInfo(26, HavokCollisionSystemID, 1, 1));
      if (this.Physics.HavokWorld == null)
        return;
      MyPhysics.RefreshCollisionFilter(this.Physics);
    }

    private void MyShipConnector_IsWorkingChanged(MyCubeBlock obj)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.Connected || this.IsFunctional && this.IsWorking)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void LoadDummies(bool recreateOnlyConnector = false)
    {
      foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(this.BlockDefinition.Model).Dummies)
      {
        bool flag1 = dummy.Key.ToLower().Contains("connector");
        bool flag2 = flag1 || dummy.Key.ToLower().Contains("ejector");
        if (flag1 || flag2)
        {
          Matrix dummyLocal = Matrix.Normalize(dummy.Value.Matrix);
          this.m_connectionPosition = dummyLocal.Translation;
          dummyLocal *= this.PositionComp.LocalMatrixRef;
          Vector3 halfExtents = dummy.Value.Matrix.Scale / 2f;
          halfExtents = new Vector3(halfExtents.X, halfExtents.Y, halfExtents.Z);
          this.m_detectorRadius = halfExtents.AbsMax();
          Vector3 center = dummy.Value.Matrix.Translation;
          if (flag1)
            MySandboxGame.Static.Invoke((Action) (() =>
            {
              if (this.MarkedForClose)
                return;
              this.RecreateConnectorDummy(ref dummyLocal, ref center, ref halfExtents);
            }), "MyShipConnector::RecreateConnectorDummy");
          if (flag2 && !recreateOnlyConnector)
          {
            this.DisposePhysicsBody(this.Physics);
            this.Physics = this.CreatePhysicsBody(MyShipConnector.Mode.Ejector, ref dummyLocal, ref center, ref halfExtents);
          }
          if (flag1)
          {
            this.m_connectorMode = MyShipConnector.Mode.Connector;
            break;
          }
          this.m_connectorMode = MyShipConnector.Mode.Ejector;
          break;
        }
      }
    }

    private void RecreateConnectorDummy(
      ref Matrix dummyLocal,
      ref Vector3 center,
      ref Vector3 halfExtents)
    {
      this.DisposeBodyConstraint(ref this.m_connectorConstraint, ref this.m_connectorConstraintsData);
      if (this.m_connectorDummy != null)
        this.m_connectorDummy.Close();
      this.m_connectorDummyLocal = dummyLocal;
      this.m_connectorCenter = center;
      this.m_connectorHalfExtents = halfExtents;
      this.m_connectorDummy = this.CreatePhysicsBody(MyShipConnector.Mode.Connector, ref dummyLocal, ref center, ref halfExtents);
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private MyPhysicsBody CreatePhysicsBody(
      MyShipConnector.Mode mode,
      ref Matrix dummyLocal,
      ref Vector3 center,
      ref Vector3 halfExtents)
    {
      MyPhysicsBody myPhysicsBody = (MyPhysicsBody) null;
      if (mode == MyShipConnector.Mode.Ejector || Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        HkBvShape detectorShape = this.CreateDetectorShape(halfExtents, mode);
        int collisionFilter = mode != MyShipConnector.Mode.Connector ? 26 : 24;
        myPhysicsBody = new MyPhysicsBody((VRage.ModAPI.IMyEntity) this, RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
        myPhysicsBody.IsPhantom = true;
        myPhysicsBody.CreateFromCollisionObject((HkShape) detectorShape, center, (MatrixD) ref dummyLocal, collisionFilter: collisionFilter);
        myPhysicsBody.RigidBody.ContactPointCallbackEnabled = true;
        detectorShape.Base.RemoveReference();
      }
      return myPhysicsBody;
    }

    private void DisposePhysicsBody(MyPhysicsBody body) => this.DisposePhysicsBody(ref body);

    private void DisposePhysicsBody(ref MyPhysicsBody body)
    {
      if (body == null)
        return;
      body.Close();
      body = (MyPhysicsBody) null;
    }

    private HkBvShape CreateDetectorShape(Vector3 extents, MyShipConnector.Mode mode)
    {
      if (mode == MyShipConnector.Mode.Ejector)
      {
        HkPhantomCallbackShape phantomCallbackShape = new HkPhantomCallbackShape(new HkPhantomHandler(this.phantom_EnterEjector), new HkPhantomHandler(this.phantom_LeaveEjector));
        return new HkBvShape((HkShape) new HkBoxShape(extents), (HkShape) phantomCallbackShape, HkReferencePolicy.TakeOwnership);
      }
      HkPhantomCallbackShape phantomCallbackShape1 = new HkPhantomCallbackShape(new HkPhantomHandler(this.phantom_EnterConnector), new HkPhantomHandler(this.phantom_LeaveConnector));
      return new HkBvShape((HkShape) new HkSphereShape(extents.AbsMax()), (HkShape) phantomCallbackShape1, HkReferencePolicy.TakeOwnership);
    }

    private void phantom_LeaveEjector(HkPhantomCallbackShape shape, HkRigidBody body)
    {
      bool flag = this.m_detectedFloaters.Count == 2;
      List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
      foreach (MyEntity myEntity in allEntities)
        this.m_detectedFloaters.Remove(myEntity);
      allEntities.Clear();
      if (!flag)
        return;
      this.SetEmissiveStateWorking();
    }

    private void phantom_LeaveConnector(HkPhantomCallbackShape shape, HkRigidBody body)
    {
      List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
      foreach (VRage.ModAPI.IMyEntity myEntity in allEntities)
        this.m_detectedGrids.Remove((MyEntity) (myEntity as MyCubeGrid));
      allEntities.Clear();
    }

    private void phantom_EnterEjector(HkPhantomCallbackShape shape, HkRigidBody body)
    {
      bool flag = false;
      List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
      foreach (VRage.ModAPI.IMyEntity myEntity in allEntities)
      {
        if (myEntity is MyFloatingObject)
        {
          flag |= this.m_detectedFloaters.Count == 1;
          this.m_detectedFloaters.Add((MyEntity) myEntity);
        }
      }
      allEntities.Clear();
      if (!flag)
        return;
      this.SetEmissiveStateWorking();
    }

    private void phantom_EnterConnector(HkPhantomCallbackShape shape, HkRigidBody body)
    {
      List<VRage.ModAPI.IMyEntity> allEntities = body.GetAllEntities();
      using (allEntities.GetClearToken<VRage.ModAPI.IMyEntity>())
      {
        foreach (VRage.ModAPI.IMyEntity myEntity in allEntities)
        {
          if (myEntity.GetTopMostParent() is MyCubeGrid topMostParent && topMostParent != this.CubeGrid)
            this.m_detectedGrids.Add((MyEntity) topMostParent);
        }
      }
    }

    private void GetBoxFromMatrix(
      Matrix m,
      out Vector3 halfExtents,
      out Vector3 position,
      out Quaternion orientation)
    {
      halfExtents = Vector3.Zero;
      position = Vector3.Zero;
      orientation = Quaternion.Identity;
    }

    private void UpdateReceiver(MyTerminalBlock block) => this.ResourceSink.Update();

    private void UpdateReceiver() => this.ResourceSink.Update();

    private void Receiver_IsPoweredChanged() => this.UpdateIsWorking();

    public override void OnRemovedByCubeBuilder()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnRemovedByCubeBuilder();
    }

    public override void OnDestroy()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnDestroy();
    }

    public override bool SetEmissiveStateWorking()
    {
      if (this.InConstraint)
      {
        MyShipConnector myShipConnector = this;
        if (this.m_other != null && this.m_other.IsMaster)
          myShipConnector = this.m_other;
        if (myShipConnector.Connected)
          return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Locked, this.Render.RenderObjectIDs[0], "Emissive");
        if (this.m_tradingBlockTimer > 0)
          return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0], "Emissive");
        return myShipConnector.IsReleasing ? this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Autolock, this.Render.RenderObjectIDs[0], "Emissive") : this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Constraint, this.Render.RenderObjectIDs[0], "Emissive");
      }
      if (this.m_tradingBlockTimer > 0)
        return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0], "Emissive");
      if (this.IsWorking)
        return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0], "Emissive");
      if (this.IsFunctional)
        return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0], "Emissive");
      this.SetEmissiveStateDamaged();
      return false;
    }

    public override bool SetEmissiveStateDamaged() => this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Damaged, this.Render.RenderObjectIDs[0], "Emissive");

    public override bool SetEmissiveStateDisabled() => this.SetEmissiveStateWorking();

    private void TryAttach(long? otherConnectorId = null)
    {
      MyShipConnector otherConnector = this.FindOtherConnector(otherConnectorId);
      if (otherConnector != null && otherConnector.FriendlyWithBlock((MyCubeBlock) this) && (this.CubeGrid.Physics != null && otherConnector.CubeGrid.Physics != null))
      {
        Vector3D vector3D1 = this.ConstraintPositionWorld();
        Vector3D vector3D2 = otherConnector.ConstraintPositionWorld();
        (vector3D2 - vector3D1).LengthSquared();
        if (otherConnector.m_connectorMode != MyShipConnector.Mode.Connector || !otherConnector.IsFunctional || (vector3D2 - vector3D1).LengthSquared() >= 0.349999994039536)
          return;
        MyShipConnector master = this.GetMaster(this, otherConnector);
        master.IsMaster = true;
        if (master == this)
        {
          this.CreateConstraint(otherConnector);
          otherConnector.IsMaster = false;
        }
        else
        {
          otherConnector.CreateConstraint(this);
          this.IsMaster = false;
        }
      }
      else
        this.m_connectionState.Value = MyShipConnector.State.DetachedMaster;
    }

    public override void DoUpdateTimerTick()
    {
      base.DoUpdateTimerTick();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.IsWorking || !this.Enabled)
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if ((bool) this.CollectAll && (double) inventory.VolumeFillFactor < 0.899999976158142)
      {
        float maxVolumeFillFactor = inventory.VolumeFillFactor + 0.05f;
        MyGridConveyorSystem.PullAllItemsForConnector((IMyConveyorEndpointBlock) this, inventory, this.OwnerId, maxVolumeFillFactor);
      }
      if (this.InConstraint || !(bool) this.ThrowOut || this.m_detectedFloaters.Count >= 2)
        return;
      this.TryThrowOutItem();
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      if (Sandbox.Game.Multiplayer.Sync.IsServer && this.IsWorking)
      {
        ++this.m_update10Counter;
        if (this.m_detectedFloaters.Count == 0 && this.m_connectorMode == MyShipConnector.Mode.Connector && (this.m_update10Counter % 4 == 0 && this.Enabled) && !this.InConstraint && !this.m_connectionState.Value.MasterToSlave.HasValue)
          this.TryAttach();
      }
      else if (Sandbox.Game.Multiplayer.Sync.IsServer && !this.IsWorking)
      {
        if (this.InConstraint && !this.Connected)
          this.Detach();
        else if (this.InConstraint && this.Connected && (!this.IsFunctional || !this.Enabled))
          this.Detach();
      }
      if (this.IsWorking && this.InConstraint && !this.Connected)
      {
        float effectiveStrength = this.GetEffectiveStrength(this.m_other);
        HkMalleableConstraintData constraintData = this.m_constraint.ConstraintData as HkMalleableConstraintData;
        if ((HkReferenceObject) constraintData != (HkReferenceObject) null && (double) constraintData.Strength != (double) effectiveStrength && this.IsMaster)
        {
          constraintData.Strength = effectiveStrength;
          this.CubeGrid.Physics.RigidBody.Activate();
          this.SetEmissiveStateWorking();
          this.m_other.SetEmissiveStateWorking();
        }
      }
      if (Sandbox.Game.Multiplayer.Sync.IsServer && this.InConstraint && (!this.Connected && this.m_connectorMode == MyShipConnector.Mode.Connector))
      {
        Vector3D vector3D = this.ConstraintPositionWorld();
        if ((this.m_other.ConstraintPositionWorld() - vector3D).LengthSquared() > 0.5)
          this.Detach();
      }
      this.UpdateConnectionState();
    }

    private void UpdateConnectionState()
    {
      if (this.m_isInitOnceBeforeFrameUpdate)
        this.m_isInitOnceBeforeFrameUpdate = false;
      else if (this.m_other == null && this.m_connectionState.Value.OtherEntityId != 0L && Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_connectionState.Value = MyShipConnector.State.Detached;
      if (!this.IsMaster || this.CubeGrid.Physics == null)
        return;
      MyShipConnector.State state = this.m_connectionState.Value;
      if (state.OtherEntityId == 0L)
      {
        if (!this.InConstraint)
          return;
        this.Detach(false);
        this.SetEmissiveStateWorking();
        if (this.m_other == null)
          return;
        this.m_other.SetEmissiveStateWorking();
      }
      else if (!state.MasterToSlave.HasValue)
      {
        if (this.Connected || this.InConstraint && this.m_other.EntityId != state.OtherEntityId)
        {
          this.Detach(false);
          this.SetEmissiveStateWorking();
          if (this.m_other != null)
            this.m_other.SetEmissiveStateWorking();
        }
        MyShipConnector entity;
        if (this.InConstraint || !Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyShipConnector>(state.OtherEntityId, out entity) || (!entity.FriendlyWithBlock((MyCubeBlock) this) || entity.Closed) || (entity.MarkedForClose || this.Physics == null || entity.Physics == null))
          return;
        if (!Sandbox.Game.Multiplayer.Sync.IsServer && state.MasterToSlaveGrid.HasValue && this.CubeGrid != entity.CubeGrid)
        {
          if (this.CubeGrid.IsStatic)
            entity.WorldMatrix = MatrixD.Multiply(MatrixD.Invert((MatrixD) state.MasterToSlaveGrid.Value), this.CubeGrid.WorldMatrix);
          else
            this.CubeGrid.WorldMatrix = MatrixD.Multiply((MatrixD) state.MasterToSlaveGrid.Value, entity.WorldMatrix);
        }
        this.CreateConstraintNosync(entity);
        this.SetEmissiveStateWorking();
        if (this.m_other == null)
          return;
        this.m_other.SetEmissiveStateWorking();
      }
      else
      {
        if (this.Connected && this.m_other.EntityId != state.OtherEntityId)
        {
          this.Detach(false);
          this.SetEmissiveStateWorking();
          if (this.m_other != null)
            this.m_other.SetEmissiveStateWorking();
        }
        MyShipConnector entity;
        Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyShipConnector>(state.OtherEntityId, out entity);
        if (this.Connected || entity == null || !entity.FriendlyWithBlock((MyCubeBlock) this))
          return;
        this.m_other = entity;
        MyDeltaTransform? nullable1 = state.MasterToSlave;
        if (nullable1.HasValue && nullable1.Value.IsZero)
          nullable1 = new MyDeltaTransform?();
        if (!Sandbox.Game.Multiplayer.Sync.IsServer && state.MasterToSlaveGrid.HasValue && this.CubeGrid != entity.CubeGrid)
        {
          if (this.CubeGrid.IsStatic)
            entity.WorldMatrix = MatrixD.Multiply(MatrixD.Invert((MatrixD) state.MasterToSlaveGrid.Value), this.CubeGrid.WorldMatrix);
          else
            this.CubeGrid.WorldMatrix = MatrixD.Multiply((MatrixD) state.MasterToSlaveGrid.Value, entity.WorldMatrix);
        }
        MyDeltaTransform? nullable2 = nullable1;
        this.Weld(nullable2.HasValue ? new MatrixD?((MatrixD) nullable2.GetValueOrDefault()) : new MatrixD?());
        this.SetEmissiveStateWorking();
        if (this.m_other == null)
          return;
        this.m_other.SetEmissiveStateWorking();
      }
    }

    private void TryThrowOutItem()
    {
      float num1 = this.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 2.5f : 0.5f;
      List<MyPhysicalInventoryItem> items = MyEntityExtensions.GetInventory(this).GetItems();
      int index = 0;
      while (index < MyEntityExtensions.GetInventory(this).GetItems().Count)
      {
        MyPhysicalInventoryItem physicalInventoryItem1 = items[index];
        MyPhysicalItemDefinition definition;
        if (MyDefinitionManager.Static.TryGetPhysicalItemDefinition(physicalInventoryItem1.Content.GetId(), out definition))
        {
          float randomFloat = MyUtils.GetRandomFloat(0.0f, this.CubeGrid.GridSizeEnum == MyCubeSize.Large ? 0.5f : 0.07f);
          Vector3 circleNormalized = MyUtils.GetRandomVector3CircleNormalized();
          Vector3D vector3D1 = Vector3D.Transform(this.ConnectionPosition, this.CubeGrid.PositionComp.WorldMatrixRef) + this.PositionComp.WorldMatrixRef.Right * (double) circleNormalized.X * (double) randomFloat + this.PositionComp.WorldMatrixRef.Up * (double) circleNormalized.Z * (double) randomFloat;
          MyFixedPoint a = (MyFixedPoint) (num1 / definition.Volume);
          if (physicalInventoryItem1.Content.TypeId != typeof (MyObjectBuilder_Ore) && physicalInventoryItem1.Content.TypeId != typeof (MyObjectBuilder_Ingot))
            a = MyFixedPoint.Ceiling(a);
          MyFixedPoint myFixedPoint1 = (MyFixedPoint) 0;
          float num2;
          MyFixedPoint myFixedPoint2;
          MyPhysicalInventoryItem physicalInventoryItem2;
          if (physicalInventoryItem1.Amount < a)
          {
            num2 = num1 - (float) physicalInventoryItem1.Amount * definition.Volume;
            myFixedPoint2 = physicalInventoryItem1.Amount;
            physicalInventoryItem2 = physicalInventoryItem1;
            MyEntityExtensions.GetInventory(this).RemoveItems(physicalInventoryItem1.ItemId, new MyFixedPoint?(), true, false, new MatrixD?(), (Action<MyDefinitionId, MyEntity>) null);
            int num3 = index + 1;
          }
          else
          {
            num2 = 0.0f;
            physicalInventoryItem2 = new MyPhysicalInventoryItem(physicalInventoryItem1.GetObjectBuilder())
            {
              Amount = a
            };
            myFixedPoint2 = a;
            MyEntityExtensions.GetInventory(this).RemoveItems(physicalInventoryItem1.ItemId, new MyFixedPoint?(a), true, false, new MatrixD?(), (Action<MyDefinitionId, MyEntity>) null);
          }
          if (!(myFixedPoint2 > (MyFixedPoint) 0))
            break;
          float num4 = (float) ((double) definition.Size.Max() * (double) physicalInventoryItem1.Scale * 0.5);
          if (physicalInventoryItem1.Content.TypeId == typeof (MyObjectBuilder_Ore))
          {
            string basedDebrisVoxel = MyDebris.GetAmountBasedDebrisVoxel(Math.Max((float) myFixedPoint2, 50f));
            if (basedDebrisVoxel != null)
            {
              MyModel modelOnlyData = MyModels.GetModelOnlyData(basedDebrisVoxel);
              if (modelOnlyData != null)
                num4 = modelOnlyData.BoundingBoxSizeHalf.Max();
            }
          }
          Vector3D vector3D2 = vector3D1 + this.PositionComp.WorldMatrixRef.Forward * (double) num4;
          MyPhysicalInventoryItem physicalInventoryItem3 = physicalInventoryItem2;
          Vector3D position = vector3D2;
          MatrixD matrixD = this.PositionComp.WorldMatrixRef;
          Vector3D forward = matrixD.Forward;
          matrixD = this.PositionComp.WorldMatrixRef;
          Vector3D up = matrixD.Up;
          MyGridPhysics physics1 = this.CubeGrid.Physics;
          Action<MyEntity> completionCallback = (Action<MyEntity>) (entity =>
          {
            MyPhysicsComponentBase physics = entity.Physics;
            physics.LinearVelocity = (Vector3) (physics.LinearVelocity + this.PositionComp.WorldMatrixRef.Forward);
            if (this.m_soundEmitter != null)
              this.m_soundEmitter.PlaySound(this.m_actionSound);
            Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyShipConnector>(this, (Func<MyShipConnector, Action>) (x => new Action(x.PlayActionSound)));
            MyParticleEffect effect;
            if (!MyParticlesManager.TryCreateParticleEffect("Smoke_Collector", entity.WorldMatrix, out effect))
              return;
            effect.Velocity = this.CubeGrid.Physics.LinearVelocity;
          });
          MyFloatingObjects.Spawn(physicalInventoryItem3, position, forward, up, (MyPhysicsComponentBase) physics1, completionCallback);
          break;
        }
      }
    }

    [Event(null, 1271)]
    [Reliable]
    [Broadcast]
    private void PlayActionSound() => this.m_soundEmitter.PlaySound(this.m_actionSound);

    private MyShipConnector FindOtherConnector(long? otherConnectorId = null)
    {
      MyShipConnector entity = (MyShipConnector) null;
      BoundingSphereD sphere = new BoundingSphereD((Vector3D) this.ConnectionPosition, (double) this.m_detectorRadius);
      if (otherConnectorId.HasValue)
      {
        Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyShipConnector>(otherConnectorId.Value, out entity);
      }
      else
      {
        sphere = sphere.Transform(this.CubeGrid.PositionComp.WorldMatrixRef);
        entity = this.TryFindConnectorInGrid(ref sphere, this.CubeGrid);
      }
      if (entity != null)
        return entity;
      foreach (MyEntity detectedGrid in this.m_detectedGrids)
      {
        if (!detectedGrid.MarkedForClose && detectedGrid is MyCubeGrid)
        {
          MyCubeGrid grid = detectedGrid as MyCubeGrid;
          if (grid != this.CubeGrid)
          {
            entity = this.TryFindConnectorInGrid(ref sphere, grid);
            if (entity != null)
              return entity;
          }
        }
      }
      return (MyShipConnector) null;
    }

    private MyShipConnector TryFindConnectorInGrid(
      ref BoundingSphereD sphere,
      MyCubeGrid grid)
    {
      MyShipConnector.m_tmpBlockSet.Clear();
      grid.GetBlocksInsideSphereInternal(ref sphere, MyShipConnector.m_tmpBlockSet, useOptimization: false);
      foreach (MySlimBlock tmpBlock in MyShipConnector.m_tmpBlockSet)
      {
        if (tmpBlock.FatBlock != null && tmpBlock.FatBlock is MyShipConnector)
        {
          MyShipConnector fatBlock = tmpBlock.FatBlock as MyShipConnector;
          if (!fatBlock.InConstraint && fatBlock != this && (fatBlock.IsWorking && fatBlock.FriendlyWithBlock((MyCubeBlock) this)))
          {
            MyShipConnector.m_tmpBlockSet.Clear();
            return fatBlock;
          }
        }
      }
      MyShipConnector.m_tmpBlockSet.Clear();
      return (MyShipConnector) null;
    }

    private void CreateConstraint(MyShipConnector otherConnector)
    {
      this.CreateConstraintNosync(otherConnector);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MatrixD matrixD = this.CubeGrid.WorldMatrix * MatrixD.Invert(this.m_other.WorldMatrix);
      VRage.Sync.Sync<MyShipConnector.State, SyncDirection.FromServer> connectionState1 = this.m_connectionState;
      MyShipConnector.State state1 = new MyShipConnector.State();
      state1.IsMaster = true;
      state1.OtherEntityId = otherConnector.EntityId;
      state1.MasterToSlave = new MyDeltaTransform?();
      state1.MasterToSlaveGrid = new MyDeltaTransform?((MyDeltaTransform) matrixD);
      MyShipConnector.State state2 = state1;
      connectionState1.Value = state2;
      VRage.Sync.Sync<MyShipConnector.State, SyncDirection.FromServer> connectionState2 = otherConnector.m_connectionState;
      state1 = new MyShipConnector.State();
      state1.IsMaster = false;
      state1.OtherEntityId = this.EntityId;
      state1.MasterToSlave = new MyDeltaTransform?();
      MyShipConnector.State state3 = state1;
      connectionState2.Value = state3;
    }

    private void CreateConstraintNosync(MyShipConnector otherConnector)
    {
      Vector3 posA = this.ConstraintPositionInGridSpace();
      Vector3 posB = otherConnector.ConstraintPositionInGridSpace();
      Vector3 axisA = this.ConstraintAxisGridSpace();
      Vector3 axisB = -otherConnector.ConstraintAxisGridSpace();
      HkHingeConstraintData data = new HkHingeConstraintData();
      data.SetInBodySpace(posA, posB, axisA, axisB, (MyPhysicsBody) this.CubeGrid.Physics, (MyPhysicsBody) otherConnector.CubeGrid.Physics);
      HkMalleableConstraintData malleableConstraintData = new HkMalleableConstraintData();
      malleableConstraintData.SetData((HkConstraintData) data);
      data.ClearHandle();
      malleableConstraintData.Strength = this.GetEffectiveStrength(otherConnector);
      HkConstraint newConstraint = new HkConstraint(this.CubeGrid.Physics.RigidBody, otherConnector.CubeGrid.Physics.RigidBody, (HkConstraintData) malleableConstraintData);
      this.SetConstraint(otherConnector, newConstraint);
      otherConnector.SetConstraint(this, newConstraint);
      this.AddConstraint(newConstraint);
    }

    private void SetConstraint(MyShipConnector other, HkConstraint newConstraint)
    {
      this.m_other = other;
      this.m_constraint = newConstraint;
      this.SetEmissiveStateWorking();
    }

    private void UnsetConstraint()
    {
      this.m_other = (MyShipConnector) null;
      this.m_constraint = (HkConstraint) null;
      this.SetEmissiveStateWorking();
    }

    public Vector3D ConstraintPositionWorld() => Vector3D.Transform(this.ConstraintPositionInGridSpace(), this.CubeGrid.PositionComp.WorldMatrixRef);

    private Vector3 ConstraintPositionInGridSpace()
    {
      Vector3 vector3 = (this.Max + this.Min) * this.CubeGrid.GridSize * 0.5f;
      Vector3 position = Vector3.DominantAxisProjection(this.ConnectionPosition - vector3);
      MatrixI matrix = new MatrixI(Vector3I.Zero, this.Orientation.Forward, this.Orientation.Up);
      Vector3.Transform(ref position, ref matrix, out Vector3 _);
      return vector3 + position;
    }

    private Vector3 ConstraintAxisGridSpace() => Vector3.Normalize(Vector3.DominantAxisProjection(this.ConnectionPosition - (this.Max + this.Min) * this.CubeGrid.GridSize * 0.5f));

    private Vector3 ProjectPerpendicularFromWorld(Vector3 worldPerpAxis)
    {
      Vector3 vector2 = this.ConstraintAxisGridSpace();
      Vector3 vector1 = Vector3.TransformNormal(worldPerpAxis, this.CubeGrid.PositionComp.WorldMatrixNormalizedInv);
      float num = Vector3.Dot(vector1, vector2);
      Vector3.Normalize(vector1 - num * vector2);
      return Vector3.Normalize(vector1 - num * vector2);
    }

    private void Weld()
    {
      (this.IsMaster ? this : this.m_other).Weld(new MatrixD?());
      if (MyVisualScriptLogicProvider.ConnectorStateChanged == null || this.m_other == null)
        return;
      MyVisualScriptLogicProvider.ConnectorStateChanged(this.EntityId, this.CubeGrid.EntityId, this.Name, this.CubeGrid.Name, this.m_other.EntityId, this.m_other.CubeGrid.EntityId, this.m_other.Name, this.m_other.CubeGrid.Name, true);
    }

    private void Weld(MatrixD? masterToSlave)
    {
      this.m_welding = true;
      this.m_welded = true;
      this.m_other.m_welded = true;
      if (!masterToSlave.HasValue)
        masterToSlave = new MatrixD?(this.WorldMatrix * MatrixD.Invert(this.m_other.WorldMatrix));
      if ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null)
      {
        this.RemoveConstraint(this.m_other, this.m_constraint);
        this.m_constraint = (HkConstraint) null;
        this.m_other.m_constraint = (HkConstraint) null;
      }
      this.WeldInternal();
      this.CubeGrid.NotifyBlockAdded(this.m_other.SlimBlock);
      this.m_other.CubeGrid.NotifyBlockAdded(this.SlimBlock);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        MatrixD matrixD = this.CubeGrid.WorldMatrix * MatrixD.Invert(this.m_other.WorldMatrix);
        VRage.Sync.Sync<MyShipConnector.State, SyncDirection.FromServer> connectionState1 = this.m_connectionState;
        MyShipConnector.State state1 = new MyShipConnector.State();
        state1.IsMaster = true;
        state1.OtherEntityId = this.m_other.EntityId;
        state1.MasterToSlave = new MyDeltaTransform?((MyDeltaTransform) masterToSlave.Value);
        state1.MasterToSlaveGrid = new MyDeltaTransform?((MyDeltaTransform) matrixD);
        MyShipConnector.State state2 = state1;
        connectionState1.Value = state2;
        VRage.Sync.Sync<MyShipConnector.State, SyncDirection.FromServer> connectionState2 = this.m_other.m_connectionState;
        state1 = new MyShipConnector.State();
        state1.IsMaster = false;
        state1.OtherEntityId = this.EntityId;
        state1.MasterToSlave = new MyDeltaTransform?((MyDeltaTransform) masterToSlave.Value);
        MyShipConnector.State state3 = state1;
        connectionState2.Value = state3;
      }
      this.m_other.m_other = this;
      this.m_welding = false;
    }

    private void RecreateConstraintInternal()
    {
      if ((HkReferenceObject) this.m_constraint != (HkReferenceObject) null)
      {
        this.RemoveConstraint(this.m_other, this.m_constraint);
        this.m_constraint = (HkConstraint) null;
        this.m_other.m_constraint = (HkConstraint) null;
      }
      HkFixedConstraintData fixedConstraintData = new HkFixedConstraintData();
      MatrixD world = MatrixD.CreateWorld(Vector3D.Transform(this.ConnectionPosition, this.CubeGrid.WorldMatrix));
      MatrixD matrixD1 = world * this.CubeGrid.PositionComp.WorldMatrixNormalizedInv;
      Matrix pivotA = (Matrix) ref matrixD1;
      MatrixD matrixD2 = world * this.m_other.CubeGrid.PositionComp.WorldMatrixNormalizedInv;
      Matrix pivotB = (Matrix) ref matrixD2;
      fixedConstraintData.SetInBodySpaceInternal(ref pivotA, ref pivotB);
      fixedConstraintData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
      HkConstraint newConstraint = new HkConstraint(this.CubeGrid.Physics.RigidBody, this.m_other.CubeGrid.Physics.RigidBody, (HkConstraintData) fixedConstraintData);
      this.SetConstraint(this.m_other, newConstraint);
      this.m_other.SetConstraint(this, newConstraint);
      this.AddConstraint(newConstraint);
    }

    private void WeldInternal()
    {
      if (this.m_attachableConveyorEndpoint.AlreadyAttached())
        this.m_attachableConveyorEndpoint.DetachAll();
      this.m_attachableConveyorEndpoint.Attach(this.m_other.m_attachableConveyorEndpoint);
      this.Connected = true;
      this.m_other.Connected = true;
      this.RecreateConstraintInternal();
      this.SetEmissiveStateWorking();
      this.m_other.SetEmissiveStateWorking();
      if (this.CubeGrid != this.m_other.CubeGrid)
      {
        if (!(bool) this.TradingEnabled && !(bool) this.m_other.TradingEnabled)
          this.OnConstraintAdded(GridLinkTypeEnum.Logical, (VRage.ModAPI.IMyEntity) this.m_other.CubeGrid);
        this.OnConstraintAdded(GridLinkTypeEnum.Physical, (VRage.ModAPI.IMyEntity) this.m_other.CubeGrid);
        MyFixedGrids.Link(this.CubeGrid, this.m_other.CubeGrid, (MyCubeBlock) this);
        MyGridPhysicalHierarchy.Static.CreateLink(this.EntityId, this.CubeGrid, this.m_other.CubeGrid);
      }
      this.CubeGrid.OnPhysicsChanged -= new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      this.CubeGrid.OnPhysicsChanged += new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      this.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
      this.m_other.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.TimeOfConnection.ValidateAndSet(MySession.Static.GameplayFrameCounter);
      this.m_other.TimeOfConnection.ValidateAndSet(MySession.Static.GameplayFrameCounter);
    }

    private void CubeGrid_OnBodyPhysicsChanged(MyEntity obj)
    {
      this.DisposeBodyConstraint(ref this.m_connectorConstraint, ref this.m_connectorConstraintsData);
      this.DisposeBodyConstraint(ref this.m_ejectorConstraint, ref this.m_ejectorConstraintsData);
      if (Sandbox.Game.Multiplayer.Sync.IsServer && !this.m_welding && (this.InConstraint && this.m_hasConstraint))
      {
        this.RemoveConstraint(this.m_other, this.m_constraint);
        this.m_constraint = (HkConstraint) null;
        this.m_other.m_constraint = (HkConstraint) null;
        this.m_hasConstraint = false;
        this.m_other.m_hasConstraint = false;
        if (this.m_welded)
          this.RecreateConstraintInternal();
        else if (!this.m_connectionState.Value.MasterToSlave.HasValue && this.CubeGrid.Physics != null && this.m_other.CubeGrid.Physics != null)
          this.CreateConstraintNosync(this.m_other);
      }
      if (this.CubeGrid.Physics != null)
      {
        MyGridPhysics physics1 = this.CubeGrid.Physics;
        physics1.EnabledChanged = physics1.EnabledChanged - new Action(this.OnPhysicsEnabledChanged);
        MyGridPhysics physics2 = this.CubeGrid.Physics;
        physics2.EnabledChanged = physics2.EnabledChanged + new Action(this.OnPhysicsEnabledChanged);
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      oldGrid.OnPhysicsChanged -= new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      this.CubeGrid.OnPhysicsChanged += new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      oldGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      this.CubeGrid.OnHavokSystemIDChanged += new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      if (oldGrid.Physics != null)
      {
        MyGridPhysics physics = oldGrid.Physics;
        physics.EnabledChanged = physics.EnabledChanged - new Action(this.OnPhysicsEnabledChanged);
      }
      if (this.CubeGrid.Physics != null)
      {
        MyGridPhysics physics = this.CubeGrid.Physics;
        physics.EnabledChanged = physics.EnabledChanged + new Action(this.OnPhysicsEnabledChanged);
      }
      base.OnCubeGridChanged(oldGrid);
    }

    private void CubeGrid_OnHavokSystemIDChanged(int id) => MySandboxGame.Static.Invoke((Action) (() =>
    {
      if (this.CubeGrid.Physics == null)
        return;
      this.UpdateHavokCollisionSystemID(this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID);
    }), "MyShipConnector::CubeGrid_OnHavokSystemIDChanged");

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    private void CubeGrid_OnPhysicsChanged(MyEntity obj) => this.CubeGrid_OnBodyPhysicsChanged(obj);

    private void AddConstraint(HkConstraint newConstraint)
    {
      this.m_hasConstraint = true;
      if (!((HkReferenceObject) newConstraint.RigidBodyA != (HkReferenceObject) newConstraint.RigidBodyB))
        return;
      this.CubeGrid.Physics.AddConstraint(newConstraint);
    }

    public void Detach(bool synchronize = true)
    {
      if (!this.IsMaster)
      {
        if (this.m_other == null || !this.m_other.IsMaster)
          return;
        if (this.IsWorking && !this.Connected)
          this.CubeGrid.Physics.RigidBody.Activate();
        this.m_other.Detach(synchronize);
      }
      else
      {
        if (!this.InConstraint || this.m_other == null)
          return;
        if (synchronize && Sandbox.Game.Multiplayer.Sync.IsServer)
        {
          this.m_connectionState.Value = MyShipConnector.State.DetachedMaster;
          this.m_other.m_connectionState.Value = MyShipConnector.State.Detached;
        }
        if (this.IsWorking && !this.Connected)
          this.CubeGrid.Physics.RigidBody.Activate();
        MyShipConnector other = this.m_other;
        if (!this.DetachInternal())
          return;
        this.CubeGrid.NotifyBlockRemoved(other.SlimBlock);
        other.CubeGrid.NotifyBlockRemoved(this.SlimBlock);
        if (MyVisualScriptLogicProvider.ConnectorStateChanged != null && other != null)
          MyVisualScriptLogicProvider.ConnectorStateChanged(this.EntityId, this.CubeGrid.EntityId, this.Name, this.CubeGrid.Name, other.EntityId, other.CubeGrid.EntityId, other.Name, other.CubeGrid.Name, false);
        if (!this.m_welded)
          return;
        this.m_welding = true;
        this.m_welded = false;
        other.m_welded = false;
        this.SetEmissiveStateWorking();
        other.SetEmissiveStateWorking();
        this.m_welding = false;
        if (((!Sandbox.Game.Multiplayer.Sync.IsServer || other.Closed ? 0 : (!other.MarkedForClose ? 1 : 0)) & (synchronize ? 1 : 0)) == 0)
          return;
        this.TryAttach(new long?(other.EntityId));
      }
    }

    private bool DetachInternal()
    {
      if (!this.IsMaster)
      {
        this.m_other.DetachInternal();
        return true;
      }
      if (!this.InConstraint || this.m_other == null || (!this.m_other.InConstraint || this.m_other.m_other == null))
        return true;
      MyShipConnector other = this.m_other;
      HkConstraint constraint = this.m_constraint;
      bool connected = this.Connected;
      if ((HkReferenceObject) constraint != (HkReferenceObject) null)
      {
        bool? nullable = this.RemoveConstraint(other, constraint);
        if (nullable.HasValue)
        {
          bool flag = nullable.Value;
          this.Connected = false;
          this.UnsetConstraint();
          other.Connected = false;
          other.UnsetConstraint();
          if (connected)
          {
            if (flag)
              this.RemoveLinks(other);
            else
              other.RemoveLinks(this);
          }
        }
        else
        {
          MyLog.Default.WriteLine("Unable to detach Ship connector");
          return false;
        }
      }
      return true;
    }

    private void RemoveLinks(MyShipConnector otherConnector)
    {
      this.m_attachableConveyorEndpoint.Detach(otherConnector.m_attachableConveyorEndpoint);
      if (this.CubeGrid != otherConnector.CubeGrid)
      {
        if (!(bool) this.TradingEnabled && !(bool) otherConnector.TradingEnabled)
          this.OnConstraintRemoved(GridLinkTypeEnum.Logical, (VRage.ModAPI.IMyEntity) otherConnector.CubeGrid);
        this.OnConstraintRemoved(GridLinkTypeEnum.Physical, (VRage.ModAPI.IMyEntity) otherConnector.CubeGrid);
        MyFixedGrids.BreakLink(this.CubeGrid, otherConnector.CubeGrid, (MyCubeBlock) this);
        MyGridPhysicalHierarchy.Static.BreakLink(this.EntityId, this.CubeGrid, otherConnector.CubeGrid);
      }
      this.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
      otherConnector.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.TimeOfConnection.ValidateAndSet(0);
      otherConnector.TimeOfConnection.ValidateAndSet(0);
    }

    private bool? RemoveConstraint(MyShipConnector otherConnector, HkConstraint constraint)
    {
      if (this.m_hasConstraint)
      {
        if (this.CubeGrid.Physics != null && !this.CubeGrid.Physics.RemoveConstraint(constraint))
          return new bool?();
        this.m_hasConstraint = false;
        constraint.Dispose();
        return new bool?(true);
      }
      return otherConnector.m_hasConstraint && otherConnector.RemoveConstraint(this, constraint).HasValue ? new bool?(false) : new bool?();
    }

    public override void OnAddedToScene(object source)
    {
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      base.OnAddedToScene(source);
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (this.m_connectorDummy != null)
        this.m_connectorDummy.Activate();
      this.SetEmissiveStateWorking();
      if (this.CubeGrid.Physics == null)
        return;
      MyGridPhysics physics = this.CubeGrid.Physics;
      physics.EnabledChanged = physics.EnabledChanged + new Action(this.OnPhysicsEnabledChanged);
    }

    private void OnPhysicsEnabledChanged()
    {
      if (this.m_connectorDummy == null)
        return;
      this.m_connectorDummy.Enabled = this.CubeGrid.Physics.Enabled;
    }

    public override void OnRemovedFromScene(object source)
    {
      this.DisposeBodyConstraint(ref this.m_connectorConstraint, ref this.m_connectorConstraintsData);
      this.DisposeBodyConstraint(ref this.m_ejectorConstraint, ref this.m_ejectorConstraintsData);
      this.CubeGrid.OnPhysicsChanged -= new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
      if (this.CubeGrid.Physics != null)
      {
        MyGridPhysics physics = this.CubeGrid.Physics;
        physics.EnabledChanged = physics.EnabledChanged - new Action(this.OnPhysicsEnabledChanged);
      }
      base.OnRemovedFromScene(source);
      if (this.Physics != null)
      {
        MyPhysicsBody physics = this.Physics;
        physics.EnabledChanged = physics.EnabledChanged - new Action(this.OnPhysicsEnabledChanged);
      }
      if (this.m_connectorDummy != null)
        this.m_connectorDummy.Deactivate();
      if (this.InConstraint)
      {
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        this.m_lastAttachedOther = this.m_other != null ? new long?(this.m_other.EntityId) : new long?();
        this.m_lastWeldedOther = this.m_welded ? this.m_lastAttachedOther : new long?();
        this.Detach(false);
      }
      this.ResourceSink.IsPoweredChanged -= new Action(this.Receiver_IsPoweredChanged);
    }

    protected void CreateBodyConstraint(
      MyPhysicsBody body,
      out HkFixedConstraintData constraintData,
      out HkConstraint constraint)
    {
      constraintData = new HkFixedConstraintData();
      constraintData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
      constraintData.SetInertiaStabilizationFactor(1f);
      constraintData.SetInBodySpace(this.PositionComp.LocalMatrixRef, Matrix.CreateTranslation(-this.m_connectionPosition), (MyPhysicsBody) this.CubeGrid.Physics, body);
      constraint = new HkConstraint(!((HkReferenceObject) this.CubeGrid.Physics.RigidBody2 != (HkReferenceObject) null) || !this.CubeGrid.Physics.Flags.HasFlag((Enum) RigidBodyFlag.RBF_DOUBLED_KINEMATIC) ? this.CubeGrid.Physics.RigidBody : this.CubeGrid.Physics.RigidBody2, body.RigidBody, (HkConstraintData) constraintData);
      uint info = HkGroupFilter.CalcFilterInfo(this.CubeGrid.Physics.RigidBody.Layer, HkGroupFilter.GetSystemGroupFromFilterInfo(this.CubeGrid.Physics.RigidBody.GetCollisionFilterInfo()), 1, 1);
      constraint.WantRuntime = true;
      this.m_canReloadDummies = false;
      body.Enabled = true;
      this.m_canReloadDummies = true;
      body.RigidBody.SetCollisionFilterInfo(info);
      MyPhysics.RefreshCollisionFilter((MyPhysicsBody) this.CubeGrid.Physics);
    }

    protected void DisposeBodyConstraint(
      ref HkConstraint constraint,
      ref HkFixedConstraintData constraintData)
    {
      if ((HkReferenceObject) constraint == (HkReferenceObject) null)
        return;
      this.CubeGrid.Physics.RemoveConstraint(constraint);
      constraint.Dispose();
      constraint = (HkConstraint) null;
      constraintData = (HkFixedConstraintData) null;
    }

    protected override void OnOwnershipChanged()
    {
      base.OnOwnershipChanged();
      if (!this.InConstraint || this.m_other.FriendlyWithBlock((MyCubeBlock) this))
        return;
      this.Detach();
    }

    protected override void Closing()
    {
      this.CubeGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      if (this.Connected)
        this.Detach();
      this.m_lastAttachedOther = new long?();
      this.m_lastWeldedOther = new long?();
      base.Closing();
    }

    protected override void BeforeDelete()
    {
      this.DisposeBodyConstraint(ref this.m_connectorConstraint, ref this.m_connectorConstraintsData);
      this.DisposeBodyConstraint(ref this.m_ejectorConstraint, ref this.m_ejectorConstraintsData);
      this.CubeGrid.OnHavokSystemIDChanged -= new Action<int>(this.CubeGrid_OnHavokSystemIDChanged);
      base.BeforeDelete();
      this.DisposePhysicsBody(ref this.m_connectorDummy);
    }

    public override void DebugDrawPhysics()
    {
      base.DebugDrawPhysics();
      if (this.m_connectorDummy == null)
        return;
      this.m_connectorDummy.DebugDraw();
    }

    private MyShipConnector GetMaster(MyShipConnector first, MyShipConnector second)
    {
      MyCubeGrid cubeGrid1 = first.CubeGrid;
      MyCubeGrid cubeGrid2 = second.CubeGrid;
      if (cubeGrid1.IsStatic != cubeGrid2.IsStatic)
      {
        if (cubeGrid1.IsStatic)
          return second;
        if (cubeGrid2.IsStatic)
          return first;
      }
      else if ((double) cubeGrid1.GridSize != (double) cubeGrid2.GridSize)
      {
        if (cubeGrid1.GridSizeEnum == MyCubeSize.Large)
          return second;
        if (cubeGrid2.GridSizeEnum == MyCubeSize.Large)
          return first;
      }
      return first.EntityId >= second.EntityId ? first : second;
    }

    internal List<long> GetInventoryEntities(long identityId)
    {
      List<long> longList = new List<long>();
      if (this.m_other == null)
        return longList;
      List<MyCubeGrid> groupNodes = MyCubeGridGroups.Static.GetGroups(GridLinkTypeEnum.Mechanical).GetGroupNodes(this.m_other.CubeGrid);
      if (groupNodes == null || groupNodes.Count == 0 || this.m_other == null)
        return longList;
      foreach (MyCubeGrid myCubeGrid in groupNodes)
      {
        if (myCubeGrid.BigOwners.Contains(identityId))
        {
          foreach (MySlimBlock cubeBlock in myCubeGrid.CubeBlocks)
          {
            if (cubeBlock.FatBlock != null)
            {
              if (cubeBlock.FatBlock is MyCargoContainer fatBlock)
              {
                if (fatBlock.HasPlayerAccess(identityId))
                {
                  if (fatBlock.GetInventoryCount() != 0)
                    longList.Add(fatBlock.EntityId);
                }
                else
                  continue;
              }
              if (cubeBlock.FatBlock is MyGasTank fatBlock && fatBlock.HasPlayerAccess(identityId))
                longList.Add(fatBlock.EntityId);
            }
          }
        }
      }
      return longList;
    }

    public override void OnTeleport()
    {
      if (!this.IsWorking || !this.InConstraint || this.Connected)
        return;
      this.Detach(false);
    }

    IMyConveyorEndpoint IMyConveyorEndpointBlock.ConveyorEndpoint => (IMyConveyorEndpoint) this.m_attachableConveyorEndpoint;

    void IMyConveyorEndpointBlock.InitializeConveyorEndpoint()
    {
      this.m_attachableConveyorEndpoint = new MyAttachableConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_attachableConveyorEndpoint));
    }

    bool Sandbox.ModAPI.Ingame.IMyShipConnector.ThrowOut
    {
      get => (bool) this.ThrowOut;
      set => this.ThrowOut.Value = value;
    }

    bool Sandbox.ModAPI.Ingame.IMyShipConnector.CollectAll
    {
      get => (bool) this.CollectAll;
      set => this.CollectAll.Value = value;
    }

    float Sandbox.ModAPI.Ingame.IMyShipConnector.PullStrength
    {
      get => (float) this.Strength;
      set
      {
        if (this.m_connectorMode != MyShipConnector.Mode.Connector)
          return;
        value = MathHelper.Clamp(value, 1E-06f, 1f);
        this.Strength.Value = value;
      }
    }

    bool Sandbox.ModAPI.Ingame.IMyShipConnector.IsLocked => this.IsWorking && this.InConstraint;

    bool Sandbox.ModAPI.Ingame.IMyShipConnector.IsConnected => this.Connected;

    MyShipConnectorStatus Sandbox.ModAPI.Ingame.IMyShipConnector.Status
    {
      get
      {
        if (this.Connected)
          return MyShipConnectorStatus.Connected;
        return this.IsWorking && this.InConstraint ? MyShipConnectorStatus.Connectable : MyShipConnectorStatus.Unconnected;
      }
    }

    Sandbox.ModAPI.Ingame.IMyShipConnector Sandbox.ModAPI.Ingame.IMyShipConnector.OtherConnector => (Sandbox.ModAPI.Ingame.IMyShipConnector) this.m_other;

    Sandbox.ModAPI.IMyShipConnector Sandbox.ModAPI.IMyShipConnector.OtherConnector => (Sandbox.ModAPI.IMyShipConnector) this.m_other;

    void Sandbox.ModAPI.Ingame.IMyShipConnector.Connect()
    {
      if (this.m_connectorMode != MyShipConnector.Mode.Connector)
        return;
      this.TryConnect();
    }

    void Sandbox.ModAPI.Ingame.IMyShipConnector.Disconnect()
    {
      if (this.m_connectorMode != MyShipConnector.Mode.Connector)
        return;
      this.TryDisconnect();
    }

    void Sandbox.ModAPI.Ingame.IMyShipConnector.ToggleConnect()
    {
      if (this.m_connectorMode != MyShipConnector.Mode.Connector)
        return;
      this.TrySwitch();
    }

    public bool UseConveyorSystem
    {
      get => true;
      set
      {
      }
    }

    int IMyInventoryOwner.InventoryCount => this.InventoryCount;

    long IMyInventoryOwner.EntityId => this.EntityId;

    bool IMyInventoryOwner.HasInventory => this.HasInventory;

    bool IMyInventoryOwner.UseConveyorSystem
    {
      get => this.UseConveyorSystem;
      set => throw new NotSupportedException();
    }

    VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(
      int index)
    {
      return (VRage.Game.ModAPI.Ingame.IMyInventory) MyEntityExtensions.GetInventory(this, index);
    }

    public PullInformation GetPullInformation() => new PullInformation()
    {
      Inventory = MyEntityExtensions.GetInventory(this),
      OwnerID = this.OwnerId,
      Constraint = new MyInventoryConstraint("Empty Constraint")
    };

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    protected override void TiersChanged()
    {
      MyUpdateTiersPlayerPresence playerPresenceTier = this.CubeGrid.PlayerPresenceTier;
      MyUpdateTiersGridPresence gridPresenceTier = this.CubeGrid.GridPresenceTier;
      if (playerPresenceTier == MyUpdateTiersPlayerPresence.Normal || gridPresenceTier == MyUpdateTiersGridPresence.Normal)
        this.ChangeTimerTick(this.GetTimerTime(0));
      else if (playerPresenceTier == MyUpdateTiersPlayerPresence.Tier2 && gridPresenceTier == MyUpdateTiersGridPresence.Tier1)
      {
        this.ChangeTimerTick(this.GetTimerTime(1));
      }
      else
      {
        if (playerPresenceTier != MyUpdateTiersPlayerPresence.Tier1 && gridPresenceTier != MyUpdateTiersGridPresence.Tier1)
          return;
        this.ChangeTimerTick(this.GetTimerTime(2));
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

    public override bool GetTimerEnabledState() => this.IsWorking && this.Enabled;

    [Serializable]
    private struct State
    {
      public static readonly MyShipConnector.State Detached = new MyShipConnector.State();
      public static readonly MyShipConnector.State DetachedMaster = new MyShipConnector.State()
      {
        IsMaster = true
      };
      public bool IsMaster;
      public long OtherEntityId;
      public MyDeltaTransform? MasterToSlave;
      public MyDeltaTransform? MasterToSlaveGrid;

      protected class Sandbox_Game_Entities_Cube_MyShipConnector\u003C\u003EState\u003C\u003EIsMaster\u003C\u003EAccessor : IMemberAccessor<MyShipConnector.State, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyShipConnector.State owner, in bool value) => owner.IsMaster = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyShipConnector.State owner, out bool value) => value = owner.IsMaster;
      }

      protected class Sandbox_Game_Entities_Cube_MyShipConnector\u003C\u003EState\u003C\u003EOtherEntityId\u003C\u003EAccessor : IMemberAccessor<MyShipConnector.State, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyShipConnector.State owner, in long value) => owner.OtherEntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyShipConnector.State owner, out long value) => value = owner.OtherEntityId;
      }

      protected class Sandbox_Game_Entities_Cube_MyShipConnector\u003C\u003EState\u003C\u003EMasterToSlave\u003C\u003EAccessor : IMemberAccessor<MyShipConnector.State, MyDeltaTransform?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyShipConnector.State owner, in MyDeltaTransform? value) => owner.MasterToSlave = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyShipConnector.State owner, out MyDeltaTransform? value) => value = owner.MasterToSlave;
      }

      protected class Sandbox_Game_Entities_Cube_MyShipConnector\u003C\u003EState\u003C\u003EMasterToSlaveGrid\u003C\u003EAccessor : IMemberAccessor<MyShipConnector.State, MyDeltaTransform?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyShipConnector.State owner, in MyDeltaTransform? value) => owner.MasterToSlaveGrid = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyShipConnector.State owner, out MyDeltaTransform? value) => value = owner.MasterToSlaveGrid;
      }
    }

    private enum Mode
    {
      Ejector,
      Connector,
    }

    protected sealed class TradingEnabled_RequestChange\u003C\u003ESystem_Boolean : ICallSite<MyShipConnector, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipConnector @this,
        in bool value,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.TradingEnabled_RequestChange(value);
      }
    }

    protected sealed class TryConnect\u003C\u003E : ICallSite<MyShipConnector, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipConnector @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.TryConnect();
      }
    }

    protected sealed class TryDisconnect\u003C\u003E : ICallSite<MyShipConnector, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipConnector @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.TryDisconnect();
      }
    }

    protected sealed class NotifyDisconnectTime\u003C\u003ESystem_Boolean : ICallSite<MyShipConnector, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipConnector @this,
        in bool setTradingProtection,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.NotifyDisconnectTime(setTradingProtection);
      }
    }

    protected sealed class PlayActionSound\u003C\u003E : ICallSite<MyShipConnector, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyShipConnector @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PlayActionSound();
      }
    }

    protected class TradingEnabled\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.FromServer>(obj1, obj2));
        ((MyShipConnector) obj0).TradingEnabled = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class TimeOfConnection\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.FromServer>(obj1, obj2));
        ((MyShipConnector) obj0).TimeOfConnection = (VRage.Sync.Sync<int, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class AutoUnlockTime\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyShipConnector) obj0).AutoUnlockTime = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class ThrowOut\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyShipConnector) obj0).ThrowOut = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class CollectAll\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyShipConnector) obj0).CollectAll = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class Strength\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyShipConnector) obj0).Strength = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_connectionState\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<MyShipConnector.State, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<MyShipConnector.State, SyncDirection.FromServer>(obj1, obj2));
        ((MyShipConnector) obj0).m_connectionState = (VRage.Sync.Sync<MyShipConnector.State, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Cube_MyShipConnector\u003C\u003EActor : IActivator, IActivator<MyShipConnector>
    {
      object IActivator.CreateInstance() => (object) new MyShipConnector();

      MyShipConnector IActivator<MyShipConnector>.CreateInstance() => new MyShipConnector();
    }
  }
}
