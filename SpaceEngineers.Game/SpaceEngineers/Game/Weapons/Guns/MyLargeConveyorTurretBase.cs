// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Weapons.Guns.MyLargeConveyorTurretBase
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Runtime.InteropServices;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;

namespace SpaceEngineers.Game.Weapons.Guns
{
  [MyCubeBlockType(typeof (MyObjectBuilder_ConveyorTurretBase))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyLargeConveyorTurretBase), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase)})]
  public abstract class MyLargeConveyorTurretBase : MyLargeTurretBase, IMyConveyorEndpointBlock, SpaceEngineers.Game.ModAPI.IMyLargeConveyorTurretBase, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase, IMyInventoryOwner
  {
    protected readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;
    private float m_actualCheckFillValue;
    private MyMultilineConveyorEndpoint m_endpoint;

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_endpoint;

    public MyLargeConveyorTurretBase()
    {
      this.CreateTerminalControls();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyLargeConveyorTurretBase>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlFactory.AddControl<MyLargeConveyorTurretBase>((MyTerminalControl<MyLargeConveyorTurretBase>) new MyTerminalControlSeparator<MyLargeConveyorTurretBase>());
      MyTerminalControlOnOffSwitch<MyLargeConveyorTurretBase> onOff = new MyTerminalControlOnOffSwitch<MyLargeConveyorTurretBase>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem);
      onOff.Getter = (MyTerminalValueControl<MyLargeConveyorTurretBase, bool>.GetterDelegate) (x => x.UseConveyorSystem);
      onOff.Setter = (MyTerminalValueControl<MyLargeConveyorTurretBase, bool>.SetterDelegate) ((x, v) => x.UseConveyorSystem = v);
      onOff.EnableToggleAction<MyLargeConveyorTurretBase>();
      MyTerminalControlFactory.AddControl<MyLargeConveyorTurretBase>((MyTerminalControl<MyLargeConveyorTurretBase>) onOff);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.m_useConveyorSystem.SetLocalValue(true);
      if (objectBuilder is MyObjectBuilder_ConveyorTurretBase conveyorTurretBase)
        this.m_useConveyorSystem.SetLocalValue(conveyorTurretBase.UseConveyorSystem);
      this.ResetActualCheckFillValue(MyEntityExtensions.GetInventory(this));
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_ConveyorTurretBase builderCubeBlock = (MyObjectBuilder_ConveyorTurretBase) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.UseConveyorSystem = (bool) this.m_useConveyorSystem;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      if (!(bool) this.m_useConveyorSystem || !MySession.Static.SurvivalMode || (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.IsWorking))
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if ((double) this.m_actualCheckFillValue > (double) inventory.VolumeFillFactor)
      {
        this.CubeGrid.GridSystems.ConveyorSystem.PullItem(this.m_gunBase.CurrentAmmoMagazineId, new MyFixedPoint?((MyFixedPoint) this.BlockDefinition.AmmoPullAmount), (IMyConveyorEndpointBlock) this, inventory, false, false);
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

    private void ResetActualCheckFillValue(MyInventory inventory)
    {
      if ((double) this.BlockDefinition.InventoryFillFactorMin > (double) inventory.VolumeFillFactor)
        this.m_actualCheckFillValue = this.BlockDefinition.InventoryFillFactorMax;
      else
        this.m_actualCheckFillValue = this.BlockDefinition.InventoryFillFactorMin;
    }

    public void InitializeConveyorEndpoint() => this.m_endpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    bool SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase.UseConveyorSystem => (bool) this.m_useConveyorSystem;

    private bool UseConveyorSystem
    {
      get => (bool) this.m_useConveyorSystem;
      set => this.m_useConveyorSystem.Value = value;
    }

    int IMyInventoryOwner.InventoryCount => this.InventoryCount;

    long IMyInventoryOwner.EntityId => this.EntityId;

    bool IMyInventoryOwner.HasInventory => this.HasInventory;

    bool IMyInventoryOwner.UseConveyorSystem
    {
      get => this.UseConveyorSystem;
      set => this.UseConveyorSystem = value;
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

    protected class m_useConveyorSystem\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyLargeConveyorTurretBase) obj0).m_useConveyorSystem = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
