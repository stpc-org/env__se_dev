// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyCargoContainer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.ModAPI;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Network;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_CargoContainer))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyCargoContainer), typeof (Sandbox.ModAPI.Ingame.IMyCargoContainer)})]
  public class MyCargoContainer : MyTerminalBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyCargoContainer, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyCargoContainer, IMyInventoryOwner
  {
    private MyCargoContainerDefinition m_cargoDefinition;
    private bool m_useConveyorSystem = true;
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;
    private string m_containerType;

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public void InitializeConveyorEndpoint() => this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    public string ContainerType
    {
      get => this.m_containerType;
      set => this.m_containerType = value;
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.m_cargoDefinition = (MyCargoContainerDefinition) MyDefinitionManager.Static.GetCubeBlockDefinition(objectBuilder.GetId());
      this.m_containerType = ((MyObjectBuilder_CargoContainer) objectBuilder).ContainerType;
      if (MyFakes.ENABLE_INVENTORY_FIX)
        this.FixSingleInventory();
      if (MyEntityExtensions.GetInventory(this) == null)
        this.Components.Add<MyInventoryBase>((MyInventoryBase) new MyInventory(this.m_cargoDefinition.InventorySize.Volume, this.m_cargoDefinition.InventorySize, MyInventoryFlags.CanReceive | MyInventoryFlags.CanSend));
      MyEntityExtensions.GetInventory(this).SetFlags(MyInventoryFlags.CanReceive | MyInventoryFlags.CanSend);
      this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
      this.UpdateIsWorking();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_CargoContainer builderCubeBlock = (MyObjectBuilder_CargoContainer) base.GetObjectBuilderCubeBlock(copy);
      if (this.m_containerType != null)
        builderCubeBlock.ContainerType = this.m_containerType;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public void SpawnRandomCargo()
    {
      if (this.m_containerType == null)
        return;
      MyContainerTypeDefinition containerTypeDefinition = MyDefinitionManager.Static.GetContainerTypeDefinition(this.m_containerType);
      if (containerTypeDefinition == null || containerTypeDefinition.Items.Length == 0)
        return;
      MyEntityExtensions.GetInventory(this).GenerateContent(containerTypeDefinition);
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      MyContainerDropComponent component;
      if (!this.Components.TryGet<MyContainerDropComponent>(out component))
        return;
      component.UpdateSound();
    }

    private bool UseConveyorSystem
    {
      get => this.m_useConveyorSystem;
      set => this.m_useConveyorSystem = value;
    }

    protected override void OnInventoryComponentAdded(MyInventoryBase inventory) => base.OnInventoryComponentAdded(inventory);

    protected override void OnInventoryComponentRemoved(MyInventoryBase inventory) => base.OnInventoryComponentRemoved(inventory);

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

    public PullInformation GetPullInformation() => (PullInformation) null;

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    private class Sandbox_Game_Entities_MyCargoContainer\u003C\u003EActor : IActivator, IActivator<MyCargoContainer>
    {
      object IActivator.CreateInstance() => (object) new MyCargoContainer();

      MyCargoContainer IActivator<MyCargoContainer>.CreateInstance() => new MyCargoContainer();
    }
  }
}
