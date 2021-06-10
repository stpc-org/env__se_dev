// Decompiled with JetBrains decompiler
// Type: World.MySpaceSessionCompatHelper
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;

namespace World
{
  internal class MySpaceSessionCompatHelper : MySessionCompatHelper
  {
    public override void FixSessionObjectBuilders(
      MyObjectBuilder_Checkpoint checkpoint,
      MyObjectBuilder_Sector sector)
    {
      base.FixSessionObjectBuilders(checkpoint, sector);
      if (sector.SectorObjects == null)
        return;
      if (sector.AppVersion == 0)
      {
        HashSet<string> stringSet = new HashSet<string>();
        stringSet.Add("LargeBlockArmorBlock");
        stringSet.Add("LargeBlockArmorSlope");
        stringSet.Add("LargeBlockArmorCorner");
        stringSet.Add("LargeBlockArmorCornerInv");
        stringSet.Add("LargeRoundArmor_Slope");
        stringSet.Add("LargeRoundArmor_Corner");
        stringSet.Add("LargeRoundArmor_CornerInv");
        stringSet.Add("LargeHeavyBlockArmorBlock");
        stringSet.Add("LargeHeavyBlockArmorSlope");
        stringSet.Add("LargeHeavyBlockArmorCorner");
        stringSet.Add("LargeHeavyBlockArmorCornerInv");
        stringSet.Add("SmallBlockArmorBlock");
        stringSet.Add("SmallBlockArmorSlope");
        stringSet.Add("SmallBlockArmorCorner");
        stringSet.Add("SmallBlockArmorCornerInv");
        stringSet.Add("SmallHeavyBlockArmorBlock");
        stringSet.Add("SmallHeavyBlockArmorSlope");
        stringSet.Add("SmallHeavyBlockArmorCorner");
        stringSet.Add("SmallHeavyBlockArmorCornerInv");
        stringSet.Add("LargeBlockInteriorWall");
        foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
        {
          if (sectorObject is MyObjectBuilder_CubeGrid objectBuilderCubeGrid)
          {
            foreach (MyObjectBuilder_CubeBlock cubeBlock in objectBuilderCubeGrid.CubeBlocks)
            {
              if (cubeBlock.TypeId != typeof (MyObjectBuilder_CubeBlock) || !stringSet.Contains(cubeBlock.SubtypeName))
                cubeBlock.ColorMaskHSV = (SerializableVector3) MyRenderComponentBase.OldGrayToHSV;
            }
          }
        }
      }
      if (sector.AppVersion <= 1100001)
        this.CheckOxygenContainers(sector);
      if (sector.AppVersion <= 1185000)
        this.CheckPistonsMaxImpulse(sector);
      if (sector.AppVersion > 1195000)
        return;
      this.CheckOldWaypoints(sector);
    }

    private void CheckPistonsMaxImpulse(MyObjectBuilder_Sector sector)
    {
      foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
      {
        if (sectorObject is MyObjectBuilder_CubeGrid objectBuilderCubeGrid)
        {
          foreach (MyObjectBuilder_CubeBlock cubeBlock in objectBuilderCubeGrid.CubeBlocks)
          {
            if (cubeBlock is MyObjectBuilder_PistonBase builderPistonBase)
            {
              if (!builderPistonBase.MaxImpulseAxis.HasValue)
                builderPistonBase.MaxImpulseAxis = new float?(3.402823E+37f);
              if (!builderPistonBase.MaxImpulseNonAxis.HasValue)
                builderPistonBase.MaxImpulseNonAxis = new float?(3.402823E+37f);
            }
          }
        }
      }
    }

    private void CheckOxygenContainers(MyObjectBuilder_Sector sector)
    {
      foreach (MyObjectBuilder_EntityBase sectorObject in sector.SectorObjects)
      {
        if (sectorObject is MyObjectBuilder_CubeGrid objectBuilderCubeGrid)
        {
          foreach (MyObjectBuilder_CubeBlock cubeBlock in objectBuilderCubeGrid.CubeBlocks)
          {
            if (cubeBlock is MyObjectBuilder_OxygenTank builderOxygenTank)
              this.CheckOxygenInventory(builderOxygenTank.Inventory);
            else if (cubeBlock is MyObjectBuilder_OxygenGenerator builderOxygenGenerator)
              this.CheckOxygenInventory(builderOxygenGenerator.Inventory);
          }
        }
        if (sectorObject is MyObjectBuilder_FloatingObject builderFloatingObject && builderFloatingObject.Item.PhysicalContent is MyObjectBuilder_OxygenContainerObject physicalContent)
          this.FixOxygenContainer(physicalContent);
      }
    }

    private void CheckOxygenInventory(MyObjectBuilder_Inventory inventory)
    {
      if (inventory == null)
        return;
      foreach (MyObjectBuilder_InventoryItem builderInventoryItem in inventory.Items)
      {
        if (builderInventoryItem.PhysicalContent is MyObjectBuilder_OxygenContainerObject physicalContent)
          this.FixOxygenContainer(physicalContent);
      }
    }

    private void FixOxygenContainer(
      MyObjectBuilder_OxygenContainerObject oxygenContainer)
    {
      oxygenContainer.GasLevel = oxygenContainer.OxygenLevel;
    }

    private void CheckInventoryBagEntity(MyObjectBuilder_Sector sector)
    {
      List<int> intList = new List<int>();
      for (int index = 0; index < sector.SectorObjects.Count; ++index)
      {
        MyObjectBuilder_EntityBase sectorObject = sector.SectorObjects[index];
        switch (sectorObject)
        {
          case MyObjectBuilder_ReplicableEntity _:
          case MyObjectBuilder_InventoryBagEntity _:
            MyObjectBuilder_EntityBase entityBase = this.ConvertInventoryBagToEntityBase(sectorObject);
            if (entityBase != null)
            {
              sector.SectorObjects[index] = entityBase;
              break;
            }
            intList.Add(index);
            break;
        }
      }
      for (int index = intList.Count - 1; index >= 0; --index)
        sector.SectorObjects.RemoveAtFast<MyObjectBuilder_EntityBase>(intList[index]);
    }

    private void CheckOldWaypoints(MyObjectBuilder_Sector sector)
    {
      List<int> intList = new List<int>();
      for (int index = 0; index < sector.SectorObjects.Count; ++index)
      {
        MyObjectBuilder_EntityBase sectorObject = sector.SectorObjects[index];
        if (sectorObject.Name != null && sectorObject.Name.StartsWith(MyVisualScriptManagerSessionComponent.WAYPOINT_NAME_PREFIX) && !(sectorObject is MyObjectBuilder_Waypoint))
        {
          MyObjectBuilder_Waypoint objectBuilderWaypoint = new MyObjectBuilder_Waypoint();
          objectBuilderWaypoint.Name = sectorObject.Name;
          objectBuilderWaypoint.ComponentContainer = sectorObject.ComponentContainer;
          objectBuilderWaypoint.EntityId = sectorObject.EntityId;
          objectBuilderWaypoint.PositionAndOrientation = sectorObject.PositionAndOrientation;
          objectBuilderWaypoint.PersistentFlags = sectorObject.PersistentFlags;
          sector.SectorObjects[index] = (MyObjectBuilder_EntityBase) objectBuilderWaypoint;
        }
      }
    }
  }
}
