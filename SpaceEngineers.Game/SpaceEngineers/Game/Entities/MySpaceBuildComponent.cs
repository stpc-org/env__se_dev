// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.MySpaceBuildComponent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using SpaceEngineers.Game.Entities.Blocks;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;

namespace SpaceEngineers.Game.Entities
{
  [PreloadRequired]
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  internal class MySpaceBuildComponent : MyBuildComponentBase
  {
    public override void LoadData()
    {
      base.LoadData();
      MyCubeBuilder.BuildComponent = (MyBuildComponentBase) this;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyCubeBuilder.BuildComponent = (MyBuildComponentBase) null;
    }

    public override MyInventoryBase GetBuilderInventory(long entityId)
    {
      if (MySession.Static.CreativeMode)
        return (MyInventoryBase) null;
      MyEntity entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity);
      return entity == null ? (MyInventoryBase) null : this.GetBuilderInventory(entity);
    }

    public override MyInventoryBase GetBuilderInventory(MyEntity entity)
    {
      if (MySession.Static.CreativeMode)
        return (MyInventoryBase) null;
      switch (entity)
      {
        case MyCharacter thisEntity:
          return (MyInventoryBase) MyEntityExtensions.GetInventory(thisEntity);
        case MyShipWelder thisEntity:
          return (MyInventoryBase) MyEntityExtensions.GetInventory(thisEntity);
        default:
          return (MyInventoryBase) null;
      }
    }

    public override bool HasBuildingMaterials(MyEntity builder, bool testTotal)
    {
      if (MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId) && builder == MySession.Static.LocalCharacter)
        return true;
      if (builder == null)
        return false;
      MyInventoryBase builderInventory = this.GetBuilderInventory(builder);
      if (builderInventory == null)
        return false;
      MyInventory destinationInventory = (MyInventory) null;
      thisEntity = (MyCockpit) null;
      long playerId = MySession.Static.LocalPlayerId;
      if (builder is MyCharacter)
      {
        if ((builder as MyCharacter).IsUsing is MyCockpit thisEntity)
        {
          destinationInventory = MyEntityExtensions.GetInventory(thisEntity);
          playerId = thisEntity.ControllerInfo.ControllingIdentityId;
        }
        else if ((builder as MyCharacter).ControllerInfo != null)
          playerId = (builder as MyCharacter).ControllerInfo.ControllingIdentityId;
      }
      bool flag = true;
      if (!testTotal)
      {
        foreach (KeyValuePair<MyDefinitionId, int> requiredMaterial in this.m_materialList.RequiredMaterials)
        {
          flag &= builderInventory.GetItemAmount(requiredMaterial.Key) >= (MyFixedPoint) requiredMaterial.Value;
          if (!flag && destinationInventory != null)
          {
            flag = destinationInventory.GetItemAmount(requiredMaterial.Key, MyItemFlags.None, false) >= (MyFixedPoint) requiredMaterial.Value;
            if (!flag)
              flag = MyGridConveyorSystem.ConveyorSystemItemAmount((IMyConveyorEndpointBlock) thisEntity, destinationInventory, playerId, requiredMaterial.Key) >= (MyFixedPoint) requiredMaterial.Value;
          }
          if (!flag)
            break;
        }
      }
      else
      {
        foreach (KeyValuePair<MyDefinitionId, int> totalMaterial in this.m_materialList.TotalMaterials)
        {
          flag &= builderInventory.GetItemAmount(totalMaterial.Key) >= (MyFixedPoint) totalMaterial.Value;
          if (!flag && destinationInventory != null)
          {
            flag = destinationInventory.GetItemAmount(totalMaterial.Key, MyItemFlags.None, false) >= (MyFixedPoint) totalMaterial.Value;
            if (!flag)
              flag = MyGridConveyorSystem.ConveyorSystemItemAmount((IMyConveyorEndpointBlock) thisEntity, destinationInventory, playerId, totalMaterial.Key) >= (MyFixedPoint) totalMaterial.Value;
          }
          if (!flag)
            break;
        }
      }
      return flag;
    }

    public override void GetGridSpawnMaterials(
      MyCubeBlockDefinition definition,
      MatrixD worldMatrix,
      bool isStatic)
    {
      this.ClearRequiredMaterials();
      MySpaceBuildComponent.GetMaterialsSimple(definition, this.m_materialList);
    }

    public override void GetBlockPlacementMaterials(
      MyCubeBlockDefinition definition,
      Vector3I position,
      MyBlockOrientation orientation,
      MyCubeGrid grid)
    {
      this.ClearRequiredMaterials();
      MySpaceBuildComponent.GetMaterialsSimple(definition, this.m_materialList);
    }

    public override void GetBlocksPlacementMaterials(
      HashSet<MyCubeGrid.MyBlockLocation> hashSet,
      MyCubeGrid grid)
    {
      this.ClearRequiredMaterials();
      foreach (MyCubeGrid.MyBlockLocation hash in hashSet)
      {
        MyCubeBlockDefinition blockDefinition = (MyCubeBlockDefinition) null;
        if (MyDefinitionManager.Static.TryGetCubeBlockDefinition((MyDefinitionId) hash.BlockDefinition, out blockDefinition))
          MySpaceBuildComponent.GetMaterialsSimple(blockDefinition, this.m_materialList);
      }
    }

    public override void GetBlockAmountPlacementMaterials(
      MyCubeBlockDefinition definition,
      int amount)
    {
      this.ClearRequiredMaterials();
      MySpaceBuildComponent.GetMaterialsSimple(definition, this.m_materialList, amount);
    }

    public override void GetGridSpawnMaterials(MyObjectBuilder_CubeGrid grid)
    {
      this.ClearRequiredMaterials();
      foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
      {
        MyComponentStack.GetMountedComponents(this.m_materialList, cubeBlock);
        if (cubeBlock.ConstructionStockpile != null)
        {
          foreach (MyObjectBuilder_StockpileItem builderStockpileItem in cubeBlock.ConstructionStockpile.Items)
          {
            if (builderStockpileItem.PhysicalContent != null)
              this.m_materialList.AddMaterial(builderStockpileItem.PhysicalContent.GetId(), builderStockpileItem.Amount, builderStockpileItem.Amount, false);
          }
        }
      }
    }

    public override void GetMultiBlockPlacementMaterials(MyMultiBlockDefinition multiBlockDefinition)
    {
    }

    public override void BeforeCreateBlock(
      MyCubeBlockDefinition definition,
      MyEntity builder,
      MyObjectBuilder_CubeBlock ob,
      bool buildAsAdmin)
    {
      base.BeforeCreateBlock(definition, builder, ob, buildAsAdmin);
      if (builder == null || !MySession.Static.SurvivalMode || buildAsAdmin)
        return;
      ob.IntegrityPercent = 1.525902E-05f;
      ob.BuildPercent = 1.525902E-05f;
    }

    public override void AfterSuccessfulBuild(MyEntity builder, bool instantBuild)
    {
      if (builder == null | instantBuild || !MySession.Static.SurvivalMode)
        return;
      this.TakeMaterialsFromBuilder(builder);
    }

    private void ClearRequiredMaterials() => this.m_materialList.Clear();

    private static void GetMaterialsSimple(
      MyCubeBlockDefinition definition,
      MyComponentList output,
      int amount = 1)
    {
      for (int index = 0; index < definition.Components.Length; ++index)
      {
        MyCubeBlockDefinition.Component component = definition.Components[index];
        output.AddMaterial(component.Definition.Id, component.Count * amount, index == 0 ? 1 : 0);
      }
    }

    private void TakeMaterialsFromBuilder(MyEntity builder)
    {
      if (builder == null)
        return;
      MyInventoryBase builderInventory = this.GetBuilderInventory(builder);
      if (builderInventory == null)
        return;
      MyInventory destinationInventory = (MyInventory) null;
      thisEntity = (MyCockpit) null;
      if (builder is MyCharacter)
      {
        if ((builder as MyCharacter).IsUsing is MyCockpit thisEntity)
        {
          destinationInventory = MyEntityExtensions.GetInventory(thisEntity);
          long controllingIdentityId = thisEntity.ControllerInfo.ControllingIdentityId;
        }
        else if ((builder as MyCharacter).ControllerInfo != null)
        {
          long controllingIdentityId1 = (builder as MyCharacter).ControllerInfo.ControllingIdentityId;
        }
      }
      foreach (KeyValuePair<MyDefinitionId, int> requiredMaterial in this.m_materialList.RequiredMaterials)
      {
        MyFixedPoint amount = (MyFixedPoint) requiredMaterial.Value;
        MyFixedPoint itemAmount1 = builderInventory.GetItemAmount(requiredMaterial.Key);
        if (itemAmount1 > (MyFixedPoint) requiredMaterial.Value)
        {
          builderInventory.RemoveItemsOfType(amount, requiredMaterial.Key);
        }
        else
        {
          if (itemAmount1 > (MyFixedPoint) 0)
          {
            builderInventory.RemoveItemsOfType(itemAmount1, requiredMaterial.Key);
            amount -= itemAmount1;
          }
          if (destinationInventory != null)
          {
            MyFixedPoint itemAmount2 = destinationInventory.GetItemAmount(requiredMaterial.Key, MyItemFlags.None, false);
            if (itemAmount2 >= amount)
            {
              destinationInventory.RemoveItemsOfType(amount, requiredMaterial.Key, MyItemFlags.None, false);
            }
            else
            {
              if (itemAmount2 > (MyFixedPoint) 0)
              {
                destinationInventory.RemoveItemsOfType(itemAmount2, requiredMaterial.Key, MyItemFlags.None, false);
                amount -= itemAmount2;
              }
              thisEntity.CubeGrid.GridSystems.ConveyorSystem.PullItem(requiredMaterial.Key, new MyFixedPoint?(amount), (IMyConveyorEndpointBlock) thisEntity, destinationInventory, true, false);
            }
          }
        }
      }
    }
  }
}
