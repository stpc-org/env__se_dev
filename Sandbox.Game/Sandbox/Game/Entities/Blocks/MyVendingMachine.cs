// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyVendingMachine
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ModAPI;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_VendingMachine))]
  public class MyVendingMachine : MyStoreBlock, IMyVendingMachine, Sandbox.ModAPI.IMyStoreBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyStoreBlock
  {
    private readonly VRage.Sync.Sync<int, SyncDirection.BothWays> m_selectedItemIdx;
    private List<MyStoreItem> m_shopItems = new List<MyStoreItem>();
    private long m_lastEcoTick;
    private bool m_firstItemsSync = true;
    private Matrix m_throwOutDummy = Matrix.Identity;

    public event Action OnSelectedItemChanged;

    public event Action<MyStoreBuyItemResult> OnBuyItemResult;

    public int SelectedItemIdx => (int) this.m_selectedItemIdx;

    public MyStoreItem SelectedItem
    {
      get
      {
        if (this.m_shopItems.Count <= 0)
          return (MyStoreItem) null;
        return (int) this.m_selectedItemIdx < 0 || (int) this.m_selectedItemIdx >= this.m_shopItems.Count ? (MyStoreItem) null : this.m_shopItems[(int) this.m_selectedItemIdx];
      }
    }

    protected override bool UseConveyorSystem
    {
      get => false;
      set
      {
      }
    }

    public MyVendingMachineDefinition BlockDefinition => (MyVendingMachineDefinition) base.BlockDefinition;

    public MyVendingMachine() => this.Render = (MyRenderComponentBase) new MyRenderComponentScreenAreas((MyEntity) this);

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.m_selectedItemIdx.SetLocalValue(-1);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_selectedItemIdx.ShouldValidate = false;
      base.Init(objectBuilder, cubeGrid);
      if (this.BlockDefinition.DefaultItems != null && this.BlockDefinition.DefaultItems.Count > 0)
      {
        this.PlayerItems.Clear();
        foreach (MyObjectBuilder_StoreItem defaultItem in this.BlockDefinition.DefaultItems)
          this.PlayerItems.Add(new MyStoreItem(defaultItem));
      }
      MyModelDummy myModelDummy;
      if (!string.IsNullOrEmpty(this.BlockDefinition.ThrowOutDummy) && this.Model.Dummies.TryGetValue(this.BlockDefinition.ThrowOutDummy, out myModelDummy))
        this.m_throwOutDummy = myModelDummy.Matrix;
      this.m_selectedItemIdx.ValueChanged += new Action<SyncBase>(this.OnSelectedItemChangedInternal);
    }

    private void OnShopItemsRecieved(
      List<MyStoreItem> storeItems,
      long lastEconomyTick,
      float offersBonus,
      float ordersBonus)
    {
      this.GenerateShopItems(storeItems);
      this.m_lastEcoTick = lastEconomyTick;
    }

    private void GenerateShopItems(List<MyStoreItem> storeItems)
    {
      if (storeItems.Count == 0)
      {
        this.m_selectedItemIdx.Value = -1;
      }
      else
      {
        this.m_shopItems.Clear();
        foreach (MyStoreItem storeItem in storeItems)
        {
          SerializableDefinitionId? nullable = storeItem.Item;
          if (nullable.HasValue && storeItem.StoreItemType != StoreItemTypes.Order)
          {
            MyPhysicalItemDefinition physicalItemDefinition = (MyPhysicalItemDefinition) null;
            MyDefinitionManager definitionManager = MyDefinitionManager.Static;
            nullable = storeItem.Item;
            MyDefinitionId defId = (MyDefinitionId) nullable.Value;
            ref MyPhysicalItemDefinition local = ref physicalItemDefinition;
            if (definitionManager.TryGetDefinition<MyPhysicalItemDefinition>(defId, out local))
              this.m_shopItems.Add(storeItem);
          }
        }
        this.m_selectedItemIdx.Value = MathHelper.Clamp((int) this.m_selectedItemIdx, 0, this.m_shopItems.Count - 1);
      }
    }

    private void OnSelectedItemChangedInternal(SyncBase obj)
    {
      Action selectedItemChanged = this.OnSelectedItemChanged;
      if (selectedItemChanged == null)
        return;
      selectedItemChanged();
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyVendingMachine>())
        return;
      base.CreateTerminalControls();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      return (MyObjectBuilder_CubeBlock) (base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_VendingMachine);
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (!this.m_firstItemsSync)
        return;
      this.CreateGetStoreItemsRequest(0L, new Action<List<MyStoreItem>, long, float, float>(this.OnShopItemsRecieved));
      this.m_firstItemsSync = false;
    }

    public void SelectNextItem()
    {
      if (this.m_shopItems.Count == 0)
        return;
      if ((int) this.m_selectedItemIdx + 1 >= this.m_shopItems.Count)
        this.m_selectedItemIdx.Value = 0;
      else
        ++this.m_selectedItemIdx.Value;
      this.UpdateStoreContent();
    }

    public void SelectPreviewsItem()
    {
      if (this.m_shopItems.Count == 0)
        return;
      if ((int) this.m_selectedItemIdx - 1 < 0)
        this.m_selectedItemIdx.Value = this.m_shopItems.Count - 1;
      else
        --this.m_selectedItemIdx.Value;
      this.UpdateStoreContent();
    }

    public void Buy()
    {
      if (MySession.Static == null || MySession.Static.LocalCharacter == null || ((int) this.m_selectedItemIdx < 0 || this.SelectedItem == null) || this.SelectedItem.Amount <= 0)
        return;
      this.CreateBuyRequest(this.SelectedItem.Id, 1, MySession.Static.LocalCharacterEntityId, this.m_lastEcoTick, new Action<MyStoreBuyItemResult>(this.OnBuyCallback));
    }

    private void OnBuyCallback(MyStoreBuyItemResult buyItemResult)
    {
      Action<MyStoreBuyItemResult> onBuyItemResult = this.OnBuyItemResult;
      if (onBuyItemResult != null)
        onBuyItemResult(buyItemResult);
      this.UpdateStoreContent();
    }

    public void UpdateStoreContent()
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      this.CreateGetStoreItemsRequest(0L, new Action<List<MyStoreItem>, long, float, float>(this.OnShopItemsRecieved));
    }

    protected override void OnPlayerStoreItemsChanged(
      List<MyStoreItem> storeItems,
      long lastEconomyTick)
    {
      base.OnPlayerStoreItemsChanged(storeItems, lastEconomyTick);
      this.OnShopItemsRecieved(storeItems, lastEconomyTick, 1f, 1f);
    }

    private void OnInventoryContentsChanged(MyInventoryBase obj)
    {
    }

    protected override void OnStartWorking()
    {
      base.OnStartWorking();
      if (this.BlockDefinition.AdditionalEmissiveMaterials == null)
        return;
      foreach (string emissiveMaterial in this.BlockDefinition.AdditionalEmissiveMaterials)
        MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], emissiveMaterial, Color.White, 1f);
    }

    protected override void OnStopWorking()
    {
      base.OnStopWorking();
      if (this.BlockDefinition.AdditionalEmissiveMaterials == null)
        return;
      foreach (string emissiveMaterial in this.BlockDefinition.AdditionalEmissiveMaterials)
        MyEntity.UpdateNamedEmissiveParts(this.Render.RenderObjectIDs[0], emissiveMaterial, Color.White, 0.0f);
    }

    protected override void OnItemBought(
      MyInventory inventory,
      MyDefinitionId definitionId,
      long totalPrice,
      int amount)
    {
      if (string.IsNullOrEmpty(this.BlockDefinition.ThrowOutDummy))
        return;
      Matrix throwOutDummy = this.m_throwOutDummy;
      MatrixD matrixD = (MatrixD) ref throwOutDummy * this.PositionComp.WorldMatrixRef;
      foreach (MyPhysicalInventoryItem physicalInventoryItem in inventory.GetItems())
      {
        if (physicalInventoryItem.Content.GetId() == definitionId && physicalInventoryItem.Amount >= (MyFixedPoint) 1)
        {
          inventory.RemoveItems(physicalInventoryItem.ItemId, new MyFixedPoint?((MyFixedPoint) 1), false, true, new MatrixD?(matrixD), new Action<MyDefinitionId, MyEntity>(this.OnItemSpawned));
          break;
        }
      }
    }

    private void OnItemSpawned(MyDefinitionId definitionId, MyEntity item)
    {
      MatrixD matrixD1 = item.PositionComp.WorldMatrixRef;
      MatrixD worldMatrix = MatrixD.CreateRotationX(1.57079601287842) * matrixD1;
      worldMatrix.Translation = item.PositionComp.WorldMatrixRef.Translation;
      item.PositionComp.SetWorldMatrix(ref worldMatrix);
      float num;
      if (this.BlockDefinition.ThrowOutItems == null || !this.BlockDefinition.ThrowOutItems.TryGetValue(definitionId.SubtypeName, out num) || item.Physics == null)
        return;
      MatrixD matrixD2 = (MatrixD) ref this.m_throwOutDummy * this.PositionComp.WorldMatrixRef;
      item.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, new Vector3?((Vector3) (matrixD2.Right * (double) num)), new Vector3D?(), new Vector3?());
    }

    [Event(null, 307)]
    [Reliable]
    [Broadcast]
    private void PlayActionSound() => this.m_soundEmitter.PlaySound(this.m_actionSound);

    protected sealed class PlayActionSound\u003C\u003E : ICallSite<MyVendingMachine, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyVendingMachine @this,
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

    protected class m_selectedItemIdx\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.BothWays>(obj1, obj2));
        ((MyVendingMachine) obj0).m_selectedItemIdx = (VRage.Sync.Sync<int, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyVendingMachine\u003C\u003EActor : IActivator, IActivator<MyVendingMachine>
    {
      object IActivator.CreateInstance() => (object) new MyVendingMachine();

      MyVendingMachine IActivator<MyVendingMachine>.CreateInstance() => new MyVendingMachine();
    }
  }
}
