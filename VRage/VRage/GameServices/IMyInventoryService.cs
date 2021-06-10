// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.IMyInventoryService
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
  public interface IMyInventoryService
  {
    IDictionary<int, MyGameInventoryItemDefinition> Definitions { get; }

    ICollection<MyGameInventoryItem> InventoryItems { get; }

    int RecycleTokens { get; }

    event EventHandler InventoryRefreshed;

    event EventHandler<MyGameItemsEventArgs> ItemsAdded;

    event EventHandler NoItemsReceived;

    event EventHandler<MyGameItemsEventArgs> CheckItemDataReady;

    MyGameInventoryItemDefinition GetInventoryItemDefinition(
      string assetModifierId);

    IEnumerable<MyGameInventoryItemDefinition> GetDefinitionsForSlot(
      MyGameInventoryItemSlot slot);

    void GetAllItems();

    bool HasInventoryItemWithDefinitionId(int id);

    bool HasInventoryItem(ulong id);

    bool HasInventoryItem(string assetModifierId);

    void TriggerPersonalContainer();

    void TriggerCompetitiveContainer();

    void GetItemCheckData(MyGameInventoryItem item, Action<byte[]> completedAction);

    void GetItemsCheckData(List<MyGameInventoryItem> items, Action<byte[]> completedAction);

    List<MyGameInventoryItem> CheckItemData(
      byte[] checkData,
      out bool checkResult);

    void ConsumeItem(MyGameInventoryItem item);

    bool RecycleItem(MyGameInventoryItem item);

    bool CraftSkin(MyGameInventoryItemQuality quality);

    uint GetCraftingCost(MyGameInventoryItemQuality quality);

    uint GetRecyclingReward(MyGameInventoryItemQuality quality);

    void AddUnownedItems();

    void Update();
  }
}
