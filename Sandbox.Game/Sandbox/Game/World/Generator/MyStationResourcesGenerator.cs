// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyStationResourcesGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Game.World.Generator
{
  internal class MyStationResourcesGenerator
  {
    private MyObjectBuilder_PhysicalObject m_iceObjectBuilder;
    private List<MyCubeBlock> m_blocksCache;
    private MyDefinitionId? m_generatedItemsContainerTypeId;

    internal MyStationResourcesGenerator(MyDefinitionId? generatedItemsContainerTypeId)
    {
      this.m_iceObjectBuilder = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>("Ice");
      this.m_generatedItemsContainerTypeId = generatedItemsContainerTypeId;
    }

    internal void UpdateStation(long stationEntityId)
    {
      MyCubeGrid entity;
      if (!Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(stationEntityId, out entity))
        return;
      this.UpdateStation(entity);
    }

    internal void UpdateStation(MyCubeGrid grid)
    {
      if (grid.MarkedForClose || grid.Closed)
      {
        this.ClearBlocksCache();
      }
      else
      {
        if (this.m_blocksCache == null)
          this.InitBlocksCache(grid);
        grid.IsGenerated = true;
        foreach (MyCubeBlock thisEntity1 in this.m_blocksCache)
        {
          if (!thisEntity1.MarkedForClose)
          {
            switch (thisEntity1)
            {
              case MyGasGenerator thisEntity:
                MyEntityExtensions.GetInventory(thisEntity).AddItems((MyFixedPoint) 10000, (MyObjectBuilder_Base) this.m_iceObjectBuilder);
                continue;
              case MyReactor thisEntity:
                MyInventory inventory1 = MyEntityExtensions.GetInventory(thisEntity);
                MyFixedPoint myFixedPoint = inventory1.MaxVolume - inventory1.CurrentVolume;
                foreach (MyReactorDefinition.FuelInfo fuelInfo in thisEntity.BlockDefinition.FuelInfos)
                {
                  int num = (int) ((double) (float) myFixedPoint / (double) fuelInfo.FuelDefinition.Volume);
                  inventory1.AddItems((MyFixedPoint) num, (MyObjectBuilder_Base) fuelInfo.FuelItem);
                }
                continue;
              case MyBatteryBlock myBatteryBlock:
                myBatteryBlock.CurrentStoredPower = myBatteryBlock.MaxStoredPower;
                continue;
              case MyLargeTurretBase thisEntity:
                MyInventory inventory2 = MyEntityExtensions.GetInventory(thisEntity);
                int num1 = (int) ((double) (float) (inventory2.MaxVolume - inventory2.CurrentVolume) / (double) thisEntity.GunBase.WeaponProperties.AmmoMagazineDefinition.Volume);
                MyObjectBuilder_Base newObject = MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) thisEntity.GunBase.CurrentAmmoMagazineId);
                inventory2.AddItems((MyFixedPoint) num1, newObject);
                continue;
              case MyGasTank myGasTank:
                double transferAmount = (1.0 - myGasTank.FilledRatio) * (double) myGasTank.Capacity;
                myGasTank.Transfer(transferAmount);
                continue;
              case MyCargoContainer _:
              case MyCryoChamber _:
                if (this.m_generatedItemsContainerTypeId.HasValue && thisEntity1.InventoryCount != 0)
                {
                  MyInventory inventory3 = MyEntityExtensions.GetInventory(thisEntity1);
                  if (inventory3.ItemCount == 0)
                  {
                    MyContainerTypeDefinition containerTypeDefinition = MyDefinitionManager.Static.GetContainerTypeDefinition(this.m_generatedItemsContainerTypeId.Value);
                    if (containerTypeDefinition != null && containerTypeDefinition.Items.Length != 0)
                    {
                      inventory3.GenerateContent(containerTypeDefinition);
                      continue;
                    }
                    continue;
                  }
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
      }
    }

    private void InitBlocksCache(MyCubeGrid grid)
    {
      this.m_blocksCache = new List<MyCubeBlock>();
      foreach (MySlimBlock block in grid.GetBlocks())
      {
        if (block.FatBlock is MyGasGenerator fatBlock)
          this.m_blocksCache.Add((MyCubeBlock) fatBlock);
        else if (block.FatBlock is MyReactor fatBlock)
          this.m_blocksCache.Add((MyCubeBlock) fatBlock);
        else if (block.FatBlock is MyBatteryBlock fatBlock)
          this.m_blocksCache.Add((MyCubeBlock) fatBlock);
        else if (block.FatBlock is MyLargeTurretBase fatBlock)
          this.m_blocksCache.Add((MyCubeBlock) fatBlock);
        else if (block.FatBlock is MyGasTank fatBlock)
          this.m_blocksCache.Add((MyCubeBlock) fatBlock);
        else if (block.FatBlock is MyCryoChamber fatBlock)
        {
          this.m_blocksCache.Add((MyCubeBlock) fatBlock);
          fatBlock.ShowInInventory = false;
        }
        else if (block.FatBlock is MyCargoContainer fatBlock)
        {
          this.m_blocksCache.Add((MyCubeBlock) fatBlock);
          fatBlock.ShowInInventory = false;
        }
      }
    }

    internal void ClearBlocksCache()
    {
      if (this.m_blocksCache == null)
        return;
      this.m_blocksCache.Clear();
      this.m_blocksCache = (List<MyCubeBlock>) null;
    }
  }
}
