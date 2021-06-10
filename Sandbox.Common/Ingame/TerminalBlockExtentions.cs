// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.TerminalBlockExtentions
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
  public static class TerminalBlockExtentions
  {
    public static long GetId(this IMyTerminalBlock block) => block.EntityId;

    public static void ApplyAction(this IMyTerminalBlock block, string actionName) => block.GetActionWithName(actionName).Apply((IMyCubeBlock) block);

    public static void ApplyAction(
      this IMyTerminalBlock block,
      string actionName,
      List<TerminalActionParameter> parameters)
    {
      block.GetActionWithName(actionName).Apply((IMyCubeBlock) block, (ListReader<TerminalActionParameter>) parameters);
    }

    public static bool HasAction(this IMyTerminalBlock block, string actionName) => block.GetActionWithName(actionName) != null;

    [Obsolete("Use the HasInventory property.")]
    public static bool HasInventory(this IMyTerminalBlock block) => block is MyEntity myEntity && block is IMyInventoryOwner && myEntity.HasInventory;

    [Obsolete("Use the GetInventoryBase method.")]
    public static IMyInventory GetInventory(this IMyTerminalBlock block, int index)
    {
      if (!(block is MyEntity myEntity))
        return (IMyInventory) null;
      return !myEntity.HasInventory ? (IMyInventory) null : myEntity.GetInventoryBase(index) as IMyInventory;
    }

    [Obsolete("Use the InventoryCount property.")]
    public static int GetInventoryCount(this IMyTerminalBlock block) => !(block is MyEntity myEntity) ? 0 : myEntity.InventoryCount;

    [Obsolete("Use the blocks themselves, this method is no longer reliable")]
    public static bool GetUseConveyorSystem(this IMyTerminalBlock block) => block is IMyInventoryOwner && ((IMyInventoryOwner) block).UseConveyorSystem;

    [Obsolete("Use the blocks themselves, this method is no longer reliable")]
    public static void SetUseConveyorSystem(this IMyTerminalBlock block, bool use)
    {
      if (!(block is IMyInventoryOwner))
        return;
      ((IMyInventoryOwner) block).UseConveyorSystem = use;
    }
  }
}
