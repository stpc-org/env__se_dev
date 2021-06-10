// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEntityInventoryOwnerExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons;
using System;
using VRage.Game.Entity;

namespace Sandbox.Game.Entities
{
  public static class MyEntityInventoryOwnerExtensions
  {
    [Obsolete("IMyInventoryOwner interface and MyInventoryOwnerTypeEnum enum is obsolete. Use type checking and inventory methods on MyEntity or MyInventory. Inventories will have this attribute as member.")]
    public static MyInventoryOwnerTypeEnum InventoryOwnerType(
      this MyEntity entity)
    {
      if (MyEntityInventoryOwnerExtensions.IsSameOrSubclass(typeof (MyUserControllableGun), entity.GetType()) || MyEntityInventoryOwnerExtensions.IsSameOrSubclass(typeof (MyProductionBlock), entity.GetType()))
        return MyInventoryOwnerTypeEnum.System;
      if (MyEntityInventoryOwnerExtensions.IsSameOrSubclass(typeof (MyConveyorSorter), entity.GetType()))
        return MyInventoryOwnerTypeEnum.Storage;
      if (MyEntityInventoryOwnerExtensions.IsSameOrSubclass(typeof (MyGasGenerator), entity.GetType()) || MyEntityInventoryOwnerExtensions.IsSameOrSubclass(typeof (MyShipToolBase), entity.GetType()) || MyEntityInventoryOwnerExtensions.IsSameOrSubclass(typeof (MyGasTank), entity.GetType()))
        return MyInventoryOwnerTypeEnum.System;
      if (MyEntityInventoryOwnerExtensions.IsSameOrSubclass(typeof (MyReactor), entity.GetType()))
        return MyInventoryOwnerTypeEnum.Energy;
      if (MyEntityInventoryOwnerExtensions.IsSameOrSubclass(typeof (MyCollector), entity.GetType()) || MyEntityInventoryOwnerExtensions.IsSameOrSubclass(typeof (MyCargoContainer), entity.GetType()))
        return MyInventoryOwnerTypeEnum.Storage;
      if (MyEntityInventoryOwnerExtensions.IsSameOrSubclass(typeof (MyShipDrill), entity.GetType()))
        return MyInventoryOwnerTypeEnum.System;
      return MyEntityInventoryOwnerExtensions.IsSameOrSubclass(typeof (MyCharacter), entity.GetType()) ? MyInventoryOwnerTypeEnum.Character : MyInventoryOwnerTypeEnum.Storage;
    }

    private static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant) => potentialDescendant.IsSubclassOf(potentialBase) || potentialDescendant == potentialBase;
  }
}
