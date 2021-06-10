// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyCubeGridSystems
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entities;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Groups;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
  public class MyCubeGridSystems
  {
    private readonly MyCubeGrid m_cubeGrid;
    private Action<MyBlockGroup> m_terminalSystem_GroupAdded;
    private Action<MyBlockGroup> m_terminalSystem_GroupRemoved;
    private bool m_blocksRegistered;
    private readonly HashSet<MyResourceSinkComponent> m_tmpSinks = new HashSet<MyResourceSinkComponent>();
    public Action<long, bool, string> GridPowerStateChanged;
    public const int PowerStateRequestPlayerId_Trash = -1;
    public const int PowerStateRequestPlayerId_SpecialContent = -2;

    public MyGridResourceDistributorSystem ResourceDistributor { get; private set; }

    public MyGridTerminalSystem TerminalSystem { get; private set; }

    public MyGridConveyorSystem ConveyorSystem { get; private set; }

    public MyGridGyroSystem GyroSystem { get; private set; }

    public MyGridWeaponSystem WeaponSystem { get; private set; }

    public MyGridReflectorLightSystem ReflectorLightSystem { get; private set; }

    public MyGridRadioSystem RadioSystem { get; private set; }

    public MyGridWheelSystem WheelSystem { get; private set; }

    public MyGridLandingSystem LandingSystem { get; private set; }

    public MyGroupControlSystem ControlSystem { get; private set; }

    public MyGridCameraSystem CameraSystem { get; private set; }

    public MyShipSoundComponent ShipSoundComponent { get; private set; }

    public MyGridOreDetectorSystem OreDetectorSystem { get; private set; }

    public MyShipMiningSystem MiningSystem { get; private set; }

    public MyGridGasSystem GasSystem { get; private set; }

    public MyGridJumpDriveSystem JumpSystem { get; private set; }

    public MyGridEmissiveSystem EmissiveSystem { get; private set; }

    public MyTieredUpdateSystem TieredUpdateSystem { get; private set; }

    protected MyCubeGrid CubeGrid => this.m_cubeGrid;

    public MyCubeGridSystems(MyCubeGrid grid)
    {
      this.m_cubeGrid = grid;
      this.m_terminalSystem_GroupAdded = new Action<MyBlockGroup>(this.TerminalSystem_GroupAdded);
      this.m_terminalSystem_GroupRemoved = new Action<MyBlockGroup>(this.TerminalSystem_GroupRemoved);
      this.GyroSystem = new MyGridGyroSystem(this.m_cubeGrid);
      this.WeaponSystem = new MyGridWeaponSystem();
      this.ReflectorLightSystem = new MyGridReflectorLightSystem(this.m_cubeGrid);
      this.RadioSystem = new MyGridRadioSystem(this.m_cubeGrid);
      if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
        this.WheelSystem = new MyGridWheelSystem(this.m_cubeGrid);
      this.ConveyorSystem = new MyGridConveyorSystem(this.m_cubeGrid);
      this.LandingSystem = new MyGridLandingSystem();
      this.ControlSystem = new MyGroupControlSystem();
      this.CameraSystem = new MyGridCameraSystem(this.m_cubeGrid);
      this.OreDetectorSystem = new MyGridOreDetectorSystem(this.m_cubeGrid);
      if (Sync.IsServer && !grid.IsPreview)
        this.TieredUpdateSystem = new MyTieredUpdateSystem(this.m_cubeGrid);
      if (Sync.IsServer && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization)
        this.GasSystem = new MyGridGasSystem((IMyCubeGrid) this.m_cubeGrid);
      if (MyPerGameSettings.EnableJumpDrive)
        this.JumpSystem = new MyGridJumpDriveSystem(this.m_cubeGrid);
      if (MyPerGameSettings.EnableShipSoundSystem && (MyFakes.ENABLE_NEW_SMALL_SHIP_SOUNDS || MyFakes.ENABLE_NEW_LARGE_SHIP_SOUNDS) && !Sandbox.Engine.Platform.Game.IsDedicated)
        this.ShipSoundComponent = new MyShipSoundComponent();
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        this.EmissiveSystem = new MyGridEmissiveSystem(this.m_cubeGrid);
      this.m_blocksRegistered = true;
    }

    public virtual void Init(MyObjectBuilder_CubeGrid builder)
    {
      MyEntityThrustComponent entityThrustComponent = this.CubeGrid.Components.Get<MyEntityThrustComponent>();
      if (entityThrustComponent != null)
        entityThrustComponent.DampenersEnabled = builder.DampenersEnabled;
      if (this.WheelSystem != null)
        this.m_cubeGrid.SetHandbrakeRequest(builder.Handbrake);
      if (this.GasSystem != null && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization)
        this.GasSystem.Init(builder.OxygenRooms);
      if (this.ShipSoundComponent != null)
      {
        this.CubeGrid.Components.Add<MyShipSoundComponent>(this.ShipSoundComponent);
        if (!this.ShipSoundComponent.InitComponent(this.m_cubeGrid))
        {
          this.ShipSoundComponent.DestroyComponent();
          this.ShipSoundComponent = (MyShipSoundComponent) null;
        }
      }
      if (MyPerGameSettings.EnableJumpDrive)
        this.JumpSystem.Init(builder.JumpDriveDirection, builder.JumpRemainingTime);
      this.CubeGrid.Components.Get<MyEntityThrustComponent>()?.MergeAllGroupsDirty();
    }

    public virtual void BeforeBlockDeserialization(MyObjectBuilder_CubeGrid builder) => this.ConveyorSystem.BeforeBlockDeserialization(builder.ConveyorLines);

    public virtual void AfterBlockDeserialization()
    {
      this.ConveyorSystem.AfterBlockDeserialization();
      this.ConveyorSystem.ResourceSink.Update();
    }

    public bool NeedsPerFrameDraw => false;

    public void UpdateBeforeSimulation()
    {
    }

    public virtual void PrepareForDraw()
    {
    }

    public void UpdatePower()
    {
      if (this.ResourceDistributor == null)
        return;
      this.ResourceDistributor.UpdateBeforeSimulation();
    }

    public virtual void UpdateOnceBeforeFrame() => this.EmissiveSystem?.UpdateEmissivity();

    public virtual void UpdateBeforeSimulation10()
    {
      if (this.CubeGrid.Closed)
      {
        MyLog.Default.Error("Updating closed Entity!");
      }
      else
      {
        this.CameraSystem.UpdateBeforeSimulation10();
        this.ConveyorSystem.UpdateBeforeSimulation10();
        this.MiningSystem?.UpdateBeforeSimulation10();
      }
    }

    public virtual void UpdateBeforeSimulation100()
    {
      if (this.ControlSystem != null)
        this.ControlSystem.UpdateBeforeSimulation100();
      if (this.ShipSoundComponent != null)
        this.ShipSoundComponent.Update100();
      if (this.ResourceDistributor != null)
        this.ResourceDistributor.UpdateBeforeSimulation100();
      this.EmissiveSystem?.UpdateBeforeSimulation100();
    }

    public virtual void UpdateAfterSimulation()
    {
    }

    public virtual void UpdateAfterSimulation100()
    {
      this.ConveyorSystem.UpdateAfterSimulation100();
      this.TieredUpdateSystem?.UpdateAfterSimulation100();
    }

    public virtual void GetObjectBuilder(MyObjectBuilder_CubeGrid ob)
    {
      MyEntityThrustComponent entityThrustComponent = this.CubeGrid.Components.Get<MyEntityThrustComponent>();
      ob.DampenersEnabled = entityThrustComponent == null || entityThrustComponent.DampenersEnabled;
      this.ConveyorSystem.SerializeLines(ob.ConveyorLines);
      if (ob.ConveyorLines.Count == 0)
        ob.ConveyorLines = (List<MyObjectBuilder_ConveyorLine>) null;
      if (this.WheelSystem != null)
        ob.Handbrake = this.WheelSystem.HandBrake;
      if (this.GasSystem != null && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization)
        ob.OxygenRooms = this.GasSystem.GetOxygenAmount();
      if (!MyPerGameSettings.EnableJumpDrive)
        return;
      ob.JumpDriveDirection = this.JumpSystem.GetJumpDriveDirection();
      ob.JumpRemainingTime = this.JumpSystem.GetRemainingJumpTime();
    }

    public virtual void AddGroup(MyBlockGroup group)
    {
      if (this.TerminalSystem == null)
        return;
      this.TerminalSystem.AddUpdateGroup(group, false);
    }

    public virtual void RemoveGroup(MyBlockGroup group)
    {
      if (this.TerminalSystem == null)
        return;
      this.TerminalSystem.RemoveGroup(group, false);
    }

    public virtual void OnAddedToGroup(MyGridLogicalGroupData group)
    {
      this.TerminalSystem = group.TerminalSystem;
      this.ResourceDistributor = group.ResourceDistributor;
      this.WeaponSystem = group.WeaponSystem;
      if (string.IsNullOrEmpty(this.ResourceDistributor.DebugName))
        this.ResourceDistributor.DebugName = this.m_cubeGrid.ToString();
      this.m_cubeGrid.OnFatBlockAdded += new Action<MyCubeBlock>(((MyResourceDistributorComponent) this.ResourceDistributor).CubeGrid_OnFatBlockAddedOrRemoved);
      this.m_cubeGrid.OnFatBlockRemoved += new Action<MyCubeBlock>(((MyResourceDistributorComponent) this.ResourceDistributor).CubeGrid_OnFatBlockAddedOrRemoved);
      this.ResourceDistributor.AddSink(this.GyroSystem.ResourceSink);
      this.ResourceDistributor.AddSink(this.ConveyorSystem.ResourceSink);
      if (this.EmissiveSystem != null)
        this.ResourceDistributor.AddSink(this.EmissiveSystem.ResourceSink);
      this.ConveyorSystem.ResourceSink.IsPoweredChanged += new Action(((MyResourceDistributorComponent) this.ResourceDistributor).ConveyorSystem_OnPoweredChanged);
      foreach (MyBlockGroup blockGroup in this.m_cubeGrid.BlockGroups)
        this.TerminalSystem.AddUpdateGroup(blockGroup, false);
      this.TerminalSystem.GroupAdded += this.m_terminalSystem_GroupAdded;
      this.TerminalSystem.GroupRemoved += this.m_terminalSystem_GroupRemoved;
      foreach (MyCubeBlock fatBlock in this.m_cubeGrid.GetFatBlocks())
      {
        if (!fatBlock.MarkedForClose)
        {
          if (fatBlock is MyTerminalBlock block)
            this.TerminalSystem.Add(block);
          MyResourceSourceComponent source = fatBlock.Components.Get<MyResourceSourceComponent>();
          if (source != null)
            this.ResourceDistributor.AddSource(source);
          MyResourceSinkComponent sink = fatBlock.Components.Get<MyResourceSinkComponent>();
          if (sink != null)
            this.ResourceDistributor.AddSink(sink);
          if (fatBlock is IMyRechargeSocketOwner rechargeSocketOwner)
            rechargeSocketOwner.RechargeSocket.ResourceDistributor = (MyResourceDistributorComponent) group.ResourceDistributor;
          if (fatBlock is IMyGunObject<MyDeviceBase> gun)
            this.WeaponSystem.Register(gun);
        }
      }
      MyGridResourceDistributorSystem resourceDistributor = this.ResourceDistributor;
      resourceDistributor.OnPowerGenerationChanged = resourceDistributor.OnPowerGenerationChanged + new Action<bool>(this.OnPowerGenerationChanged);
      this.TerminalSystem.BlockManipulationFinishedFunction();
      this.ResourceDistributor.UpdateBeforeSimulation();
    }

    public virtual void OnRemovedFromGroup(MyGridLogicalGroupData group)
    {
      if (this.m_blocksRegistered)
      {
        this.TerminalSystem.GroupAdded -= this.m_terminalSystem_GroupAdded;
        this.TerminalSystem.GroupRemoved -= this.m_terminalSystem_GroupRemoved;
        foreach (MyBlockGroup blockGroup in this.m_cubeGrid.BlockGroups)
          this.TerminalSystem.RemoveGroup(blockGroup, false);
        foreach (MyCubeBlock fatBlock in this.m_cubeGrid.GetFatBlocks())
        {
          if (fatBlock is MyTerminalBlock block)
            this.TerminalSystem.Remove(block);
          MyResourceSourceComponent source = fatBlock.Components.Get<MyResourceSourceComponent>();
          if (source != null)
            this.ResourceDistributor.RemoveSource(source);
          MyResourceSinkComponent sink = fatBlock.Components.Get<MyResourceSinkComponent>();
          if (sink != null)
            this.ResourceDistributor.RemoveSink(sink, false, fatBlock.MarkedForClose);
          if (fatBlock is IMyRechargeSocketOwner rechargeSocketOwner)
            rechargeSocketOwner.RechargeSocket.ResourceDistributor = (MyResourceDistributorComponent) null;
          if (fatBlock is IMyGunObject<MyDeviceBase> gun)
            this.WeaponSystem.Unregister(gun);
        }
        this.TerminalSystem.BlockManipulationFinishedFunction();
      }
      this.ConveyorSystem.ResourceSink.IsPoweredChanged -= new Action(((MyResourceDistributorComponent) this.ResourceDistributor).ConveyorSystem_OnPoweredChanged);
      group.ResourceDistributor.RemoveSink(this.ConveyorSystem.ResourceSink, false);
      group.ResourceDistributor.RemoveSink(this.GyroSystem.ResourceSink, false);
      if (this.EmissiveSystem != null)
        group.ResourceDistributor.RemoveSink(this.EmissiveSystem.ResourceSink, false);
      group.ResourceDistributor.UpdateBeforeSimulation();
      this.m_cubeGrid.OnFatBlockAdded -= new Action<MyCubeBlock>(((MyResourceDistributorComponent) this.ResourceDistributor).CubeGrid_OnFatBlockAddedOrRemoved);
      this.m_cubeGrid.OnFatBlockRemoved -= new Action<MyCubeBlock>(((MyResourceDistributorComponent) this.ResourceDistributor).CubeGrid_OnFatBlockAddedOrRemoved);
      MyGridResourceDistributorSystem resourceDistributor = this.ResourceDistributor;
      resourceDistributor.OnPowerGenerationChanged = resourceDistributor.OnPowerGenerationChanged - new Action<bool>(this.OnPowerGenerationChanged);
      this.ResourceDistributor = (MyGridResourceDistributorSystem) null;
      this.TerminalSystem = (MyGridTerminalSystem) null;
      this.WeaponSystem = (MyGridWeaponSystem) null;
    }

    private void OnPowerGenerationChanged(bool powerIsGenerated)
    {
      if (MyVisualScriptLogicProvider.GridPowerGenerationStateChanged != null)
        MyVisualScriptLogicProvider.GridPowerGenerationStateChanged(this.CubeGrid.EntityId, this.CubeGrid.Name, powerIsGenerated);
      if (this.GridPowerStateChanged == null)
        return;
      this.GridPowerStateChanged(this.CubeGrid.EntityId, powerIsGenerated, this.CubeGrid.Name);
    }

    public void OnAddedToGroup(MyGridPhysicalGroupData group)
    {
      this.ControlSystem = group.ControlSystem;
      foreach (MyShipController fatBlock in this.m_cubeGrid.GetFatBlocks<MyShipController>())
      {
        if (fatBlock != null && (fatBlock.ControllerInfo.Controller != null || fatBlock.Pilot != null && MySessionComponentReplay.Static.HasEntityReplayData(this.CubeGrid.EntityId)) && (fatBlock.EnableShipControl && (!(fatBlock is MyCockpit) || fatBlock.IsMainCockpit || !fatBlock.CubeGrid.HasMainCockpit())))
          this.ControlSystem.AddControllerBlock(fatBlock);
      }
      this.ControlSystem.AddGrid(this.CubeGrid);
    }

    public void OnRemovedFromGroup(MyGridPhysicalGroupData group)
    {
      this.ControlSystem.RemoveGrid(this.CubeGrid);
      if (this.m_blocksRegistered)
      {
        foreach (MyShipController fatBlock in this.m_cubeGrid.GetFatBlocks<MyShipController>())
        {
          if (fatBlock != null && fatBlock.ControllerInfo.Controller != null && fatBlock.EnableShipControl && (!(fatBlock is MyCockpit) || fatBlock.IsMainCockpit || !fatBlock.CubeGrid.HasMainCockpit()))
            this.ControlSystem.RemoveControllerBlock(fatBlock);
        }
      }
      this.ControlSystem = (MyGroupControlSystem) null;
    }

    public virtual void BeforeGridClose()
    {
      this.ConveyorSystem.IsClosing = true;
      this.ReflectorLightSystem.IsClosing = true;
      this.RadioSystem.IsClosing = true;
      if (this.ShipSoundComponent != null)
      {
        this.ShipSoundComponent.DestroyComponent();
        this.ShipSoundComponent = (MyShipSoundComponent) null;
      }
      if (this.GasSystem == null)
        return;
      this.GasSystem.OnGridClosing();
    }

    public virtual void AfterGridClose()
    {
      this.ConveyorSystem.AfterGridClose();
      if (MyPerGameSettings.EnableJumpDrive)
        this.JumpSystem.AfterGridClose();
      this.m_blocksRegistered = false;
      if (this.ControlSystem != null)
        this.ControlSystem.RemoveGrid(this.CubeGrid);
      this.GasSystem = (MyGridGasSystem) null;
    }

    public virtual void DebugDraw()
    {
      if (MyDebugDrawSettings.DEBUG_DRAW_GRID_TERMINAL_SYSTEMS)
        MyRenderProxy.DebugDrawText3D(this.m_cubeGrid.WorldMatrix.Translation, this.TerminalSystem.GetHashCode().ToString(), Color.NavajoWhite, 1f, false);
      if (MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS)
      {
        this.ConveyorSystem.DebugDraw(this.m_cubeGrid);
        this.ConveyorSystem.DebugDrawLinePackets();
      }
      if (this.GyroSystem != null && MyDebugDrawSettings.DEBUG_DRAW_GYROS)
        this.GyroSystem.DebugDraw();
      if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS && this.ResourceDistributor != null)
        this.ResourceDistributor.DebugDraw((MyEntity) this.m_cubeGrid);
      if (MyDebugDrawSettings.DEBUG_DRAW_BLOCK_GROUPS && this.TerminalSystem != null)
        this.TerminalSystem.DebugDraw((MyEntity) this.m_cubeGrid);
      if (MySession.Static != null && this.GasSystem != null && (MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization) && MyDebugDrawSettings.DEBUG_DRAW_OXYGEN)
        this.GasSystem.DebugDraw();
      this.MiningSystem?.DebugDraw();
    }

    public virtual bool IsTrash() => this.ResourceDistributor.ResourceState == MyResourceStateEnum.NoPower && !this.ControlSystem.IsControlled;

    public virtual void RegisterInSystems(MyCubeBlock block)
    {
      if (block.GetType() != typeof (MyCubeBlock))
      {
        if (this.ResourceDistributor != null)
        {
          MyResourceSourceComponent source = block.Components.Get<MyResourceSourceComponent>();
          if (source != null)
            this.ResourceDistributor.AddSource(source);
          MyResourceSinkComponent sink = block.Components.Get<MyResourceSinkComponent>();
          if (!(block is MyThrust) && sink != null)
            this.ResourceDistributor.AddSink(sink);
          if (block is IMyRechargeSocketOwner rechargeSocketOwner)
            rechargeSocketOwner.RechargeSocket.ResourceDistributor = (MyResourceDistributorComponent) this.ResourceDistributor;
        }
        if (this.WeaponSystem != null && block is IMyGunObject<MyDeviceBase> gun)
          this.WeaponSystem.Register(gun);
        if (this.TerminalSystem != null && block is MyTerminalBlock block1)
          this.TerminalSystem.Add(block1);
        MyCubeBlock block2 = block == null || !block.HasInventory ? (MyCubeBlock) null : block;
        if (block2 != null)
          this.ConveyorSystem.Add(block2);
        if (block is IMyConveyorEndpointBlock endpointBlock)
        {
          endpointBlock.InitializeConveyorEndpoint();
          this.ConveyorSystem.AddConveyorBlock(endpointBlock);
        }
        if (block is IMyConveyorSegmentBlock segmentBlock)
        {
          segmentBlock.InitializeConveyorSegment();
          this.ConveyorSystem.AddSegmentBlock(segmentBlock);
        }
        if (block is MyReflectorLight reflector)
          this.ReflectorLightSystem.Register(reflector);
        if (block.Components.Contains(typeof (MyDataBroadcaster)))
          this.RadioSystem.Register(block.Components.Get<MyDataBroadcaster>());
        if (block.Components.Contains(typeof (MyDataReceiver)))
          this.RadioSystem.Register(block.Components.Get<MyDataReceiver>());
        if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT && block is MyMotorSuspension motor)
          this.WheelSystem.Register(motor);
        if (block is IMyTieredUpdateBlock tieredBlock && this.TieredUpdateSystem != null && tieredBlock.IsTieredUpdateSupported)
          this.TieredUpdateSystem.Register(tieredBlock, block.EntityId);
        if (block is IMyLandingGear gear)
          this.LandingSystem.Register(gear);
        if (block is MyGyro gyro)
          this.GyroSystem.Register(gyro);
        if (block is MyCameraBlock camera)
          this.CameraSystem.Register(camera);
        if (this.EmissiveSystem != null && block is MyEmissiveBlock emissiveBlock)
          this.EmissiveSystem.Register(emissiveBlock);
      }
      block.OnRegisteredToGridSystems();
    }

    public virtual void UnregisterFromSystems(MyCubeBlock block)
    {
      if (block.GetType() != typeof (MyCubeBlock))
      {
        if (this.ResourceDistributor != null)
        {
          MyResourceSourceComponent source = block.Components.Get<MyResourceSourceComponent>();
          if (source != null)
            this.ResourceDistributor.RemoveSource(source);
          MyResourceSinkComponent sink = block.Components.Get<MyResourceSinkComponent>();
          if (sink != null)
            this.ResourceDistributor.RemoveSink(sink);
          if (block is IMyRechargeSocketOwner rechargeSocketOwner)
            rechargeSocketOwner.RechargeSocket.ResourceDistributor = (MyResourceDistributorComponent) null;
        }
        if (this.WeaponSystem != null && block is IMyGunObject<MyDeviceBase> gun)
          this.WeaponSystem.Unregister(gun);
        if (this.TerminalSystem != null && block is MyTerminalBlock block1)
          this.TerminalSystem.Remove(block1);
        if (block.HasInventory)
          this.ConveyorSystem.Remove(block);
        if (block is IMyConveyorEndpointBlock block1)
          this.ConveyorSystem.RemoveConveyorBlock(block1);
        if (block is IMyConveyorSegmentBlock segmentBlock)
          this.ConveyorSystem.RemoveSegmentBlock(segmentBlock);
        if (block is MyReflectorLight reflector)
          this.ReflectorLightSystem.Unregister(reflector);
        MyDataBroadcaster broadcaster = block.Components.Get<MyDataBroadcaster>();
        if (broadcaster != null)
          this.RadioSystem.Unregister(broadcaster);
        MyDataReceiver reciever = block.Components.Get<MyDataReceiver>();
        if (reciever != null)
          this.RadioSystem.Unregister(reciever);
        if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT && block is MyMotorSuspension motor)
          this.WheelSystem.Unregister(motor);
        if (block is IMyTieredUpdateBlock tieredBlock && this.TieredUpdateSystem != null && tieredBlock.IsTieredUpdateSupported)
          this.TieredUpdateSystem.Unregister(tieredBlock, block.EntityId);
        if (block is IMyLandingGear gear)
          this.LandingSystem.Unregister(gear);
        if (block is MyGyro gyro)
          this.GyroSystem.Unregister(gyro);
        if (block is MyCameraBlock camera)
          this.CameraSystem.Unregister(camera);
        if (this.EmissiveSystem != null && block is MyEmissiveBlock emissiveBlock)
          this.EmissiveSystem.Unregister(emissiveBlock);
      }
      block.OnUnregisteredFromGridSystems();
    }

    public void SyncObject_PowerProducerStateChanged(
      MyMultipleEnabledEnum enabledState,
      long playerId)
    {
      if (Sync.IsServer)
      {
        MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(this.CubeGrid);
        if (group != null)
        {
          foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
          {
            MyCubeGrid nodeData = node.NodeData;
            if (nodeData != null && nodeData.Physics != null && nodeData.Physics.Shape != null)
            {
              foreach (MyCubeBlock fatBlock in nodeData.GetFatBlocks())
              {
                if (fatBlock is IMyPowerProducer myPowerProducer)
                {
                  bool flag = false;
                  if (playerId >= 0L)
                  {
                    if (fatBlock is MyFunctionalBlock myFunctionalBlock && myFunctionalBlock.HasPlayerAccess(playerId))
                      flag = true;
                  }
                  else if (playerId == -1L)
                    flag = true;
                  else if (playerId == -2L)
                  {
                    string str = (fatBlock as MyTerminalBlock).CustomName.ToString();
                    if (str == "Special Content Power" || str == "Special Content")
                      flag = true;
                  }
                  if (flag)
                    myPowerProducer.Enabled = enabledState == MyMultipleEnabledEnum.AllEnabled;
                }
              }
            }
          }
        }
      }
      if (this.ResourceDistributor != null)
        this.ResourceDistributor.ChangeSourcesState(MyResourceDistributorComponent.ElectricityId, enabledState, playerId);
      this.CubeGrid.ActivatePhysics();
    }

    private void TerminalSystem_GroupRemoved(MyBlockGroup group)
    {
      MyBlockGroup group1 = this.m_cubeGrid.BlockGroups.Find((Predicate<MyBlockGroup>) (x => x.Name.CompareTo(group.Name) == 0));
      if (group1 == null)
        return;
      group1.Blocks.Clear();
      this.m_cubeGrid.BlockGroups.Remove(group1);
      this.m_cubeGrid.ModifyGroup(group1);
    }

    private void TerminalSystem_GroupAdded(MyBlockGroup group)
    {
      MyBlockGroup group1 = this.m_cubeGrid.BlockGroups.Find((Predicate<MyBlockGroup>) (x => x.Name.CompareTo(group.Name) == 0));
      if (group.Blocks.FirstOrDefault<MyTerminalBlock>((Func<MyTerminalBlock, bool>) (x => this.m_cubeGrid.GetFatBlocks().IndexOf((MyCubeBlock) x) != -1)) == null)
        return;
      if (group1 == null)
      {
        group1 = new MyBlockGroup();
        group1.Name.AppendStringBuilder(group.Name);
        this.m_cubeGrid.BlockGroups.Add(group1);
      }
      group1.Blocks.Clear();
      foreach (MyTerminalBlock block in group.Blocks)
      {
        if (block.CubeGrid == this.m_cubeGrid)
          group1.Blocks.Add(block);
      }
      this.m_cubeGrid.ModifyGroup(group1);
    }

    public virtual void OnBlockAdded(MySlimBlock block)
    {
      if (block.FatBlock is MyThrust)
      {
        if (this.ShipSoundComponent != null)
          this.ShipSoundComponent.ShipHasChanged = true;
        this.CubeGrid.Components.Get<MyEntityThrustComponent>()?.MarkDirty();
      }
      if (this.ConveyorSystem == null)
        return;
      this.ConveyorSystem.UpdateLines();
    }

    public virtual void OnBlockRemoved(MySlimBlock block)
    {
      if (block.FatBlock is MyThrust)
      {
        if (this.ShipSoundComponent != null)
          this.ShipSoundComponent.ShipHasChanged = true;
        this.CubeGrid.Components.Get<MyEntityThrustComponent>()?.MarkDirty();
      }
      if (this.ConveyorSystem == null)
        return;
      this.ConveyorSystem.UpdateLines();
    }

    public virtual void OnBlockIntegrityChanged(MySlimBlock block)
    {
    }

    public virtual void OnBlockOwnershipChanged(MyCubeGrid cubeGrid) => this.ConveyorSystem.FlagForRecomputation();

    public MyShipMiningSystem GetOrCreateMiningSystem()
    {
      this.MiningSystem = this.MiningSystem ?? new MyShipMiningSystem(this.CubeGrid);
      return this.MiningSystem;
    }
  }
}
