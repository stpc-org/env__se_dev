// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyNullInventoryService
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
  public class MyNullInventoryService : IMyInventoryService
  {
    public IDictionary<int, MyGameInventoryItemDefinition> Definitions { get; }

    public ICollection<MyGameInventoryItem> InventoryItems { get; }

    public int RecycleTokens { get; }

    public event EventHandler InventoryRefreshed;

    public event EventHandler<MyGameItemsEventArgs> ItemsAdded;

    public event EventHandler NoItemsReceived;

    public event EventHandler<MyGameItemsEventArgs> CheckItemDataReady;

    protected virtual void DoInventoryRefreshed()
    {
      EventHandler inventoryRefreshed = this.InventoryRefreshed;
      if (inventoryRefreshed == null)
        return;
      inventoryRefreshed((object) this, EventArgs.Empty);
    }

    protected virtual void DoItemsAdded(MyGameItemsEventArgs e)
    {
      EventHandler<MyGameItemsEventArgs> itemsAdded = this.ItemsAdded;
      if (itemsAdded == null)
        return;
      itemsAdded((object) this, e);
    }

    protected virtual void DoNoItemsReceived()
    {
      EventHandler noItemsReceived = this.NoItemsReceived;
      if (noItemsReceived == null)
        return;
      noItemsReceived((object) this, EventArgs.Empty);
    }

    protected void DoCheckItemDataReady(MyGameItemsEventArgs e)
    {
      EventHandler<MyGameItemsEventArgs> checkItemDataReady = this.CheckItemDataReady;
      if (checkItemDataReady == null)
        return;
      checkItemDataReady((object) this, e);
    }

    public void GetAllItems()
    {
    }

    public MyGameInventoryItemDefinition GetInventoryItemDefinition(
      string assetModifierId)
    {
      return (MyGameInventoryItemDefinition) null;
    }

    public IEnumerable<MyGameInventoryItemDefinition> GetDefinitionsForSlot(
      MyGameInventoryItemSlot slot)
    {
      return (IEnumerable<MyGameInventoryItemDefinition>) new List<MyGameInventoryItemDefinition>();
    }

    public bool HasInventoryItemWithDefinitionId(int id) => false;

    public bool HasInventoryItem(ulong id) => false;

    public bool HasInventoryItem(string assetModifierId) => false;

    public void TriggerPersonalContainer()
    {
    }

    public void TriggerCompetitiveContainer()
    {
    }

    public void GetItemCheckData(MyGameInventoryItem item, Action<byte[]> completedAction)
    {
    }

    public void GetItemsCheckData(List<MyGameInventoryItem> items, Action<byte[]> completedAction)
    {
    }

    public List<MyGameInventoryItem> CheckItemData(
      byte[] checkData,
      out bool checkResult)
    {
      checkResult = false;
      return (List<MyGameInventoryItem>) null;
    }

    public void ConsumeItem(MyGameInventoryItem item)
    {
    }

    public bool RecycleItem(MyGameInventoryItem item) => false;

    public bool CraftSkin(MyGameInventoryItemQuality quality) => false;

    public uint GetCraftingCost(MyGameInventoryItemQuality quality) => 0;

    public uint GetRecyclingReward(MyGameInventoryItemQuality quality) => 0;

    public void AddUnownedItems()
    {
    }

    public void Update()
    {
    }
  }
}
