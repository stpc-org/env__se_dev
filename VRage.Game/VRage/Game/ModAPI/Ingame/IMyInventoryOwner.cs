// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.IMyInventoryOwner
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;

namespace VRage.Game.ModAPI.Ingame
{
  [Obsolete("IMyInventoryOwner interface and MyInventoryOwnerTypeEnum enum is obsolete. Use type checking and inventory methods on MyEntity.")]
  public interface IMyInventoryOwner
  {
    int InventoryCount { get; }

    IMyInventory GetInventory(int index);

    long EntityId { get; }

    bool UseConveyorSystem { get; set; }

    bool HasInventory { get; }
  }
}
