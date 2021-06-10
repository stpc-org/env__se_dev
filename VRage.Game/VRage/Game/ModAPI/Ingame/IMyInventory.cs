// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.Ingame.IMyInventory
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;

namespace VRage.Game.ModAPI.Ingame
{
  public interface IMyInventory
  {
    IMyEntity Owner { get; }

    bool IsFull { get; }

    MyFixedPoint CurrentMass { get; }

    MyFixedPoint MaxVolume { get; }

    MyFixedPoint CurrentVolume { get; }

    int ItemCount { get; }

    bool IsItemAt(int position);

    MyFixedPoint GetItemAmount(MyItemType itemType);

    bool ContainItems(MyFixedPoint amount, MyItemType itemType);

    MyInventoryItem? GetItemAt(int index);

    MyInventoryItem? GetItemByID(uint id);

    MyInventoryItem? FindItem(MyItemType itemType);

    bool CanItemsBeAdded(MyFixedPoint amount, MyItemType itemType);

    void GetItems(List<MyInventoryItem> items, Func<MyInventoryItem, bool> filter = null);

    bool TransferItemTo(IMyInventory dstInventory, MyInventoryItem item, MyFixedPoint? amount = null);

    bool TransferItemFrom(IMyInventory sourceInventory, MyInventoryItem item, MyFixedPoint? amount = null);

    bool TransferItemTo(
      IMyInventory dst,
      int sourceItemIndex,
      int? targetItemIndex = null,
      bool? stackIfPossible = null,
      MyFixedPoint? amount = null);

    bool TransferItemFrom(
      IMyInventory sourceInventory,
      int sourceItemIndex,
      int? targetItemIndex = null,
      bool? stackIfPossible = null,
      MyFixedPoint? amount = null);

    bool IsConnectedTo(IMyInventory otherInventory);

    bool CanTransferItemTo(IMyInventory otherInventory, MyItemType itemType);

    void GetAcceptedItems(List<MyItemType> itemsTypes, Func<MyItemType, bool> filter = null);
  }
}
