// Decompiled with JetBrains decompiler
// Type: VRage.GameServices.MyMockingInventory
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using LitJson;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage.FileSystem;

namespace VRage.GameServices
{
  public class MyMockingInventory : IMyInventoryService
  {
    private readonly ConcurrentQueue<Action> m_updateThreadInvocationQueue = new ConcurrentQueue<Action>();
    private readonly ConcurrentDictionary<ulong, MyGameInventoryItem> m_inventory = new ConcurrentDictionary<ulong, MyGameInventoryItem>();
    private readonly ConcurrentDictionary<int, MyGameInventoryItemDefinition> m_itemDefinitions = new ConcurrentDictionary<int, MyGameInventoryItemDefinition>();
    private readonly Dictionary<int, uint> m_itemToDlc = new Dictionary<int, uint>();
    private readonly Dictionary<uint, List<MyMockingInventory.MyGameInventoryItemDefinitionRaw>> m_dlcDefinitions = new Dictionary<uint, List<MyMockingInventory.MyGameInventoryItemDefinitionRaw>>();
    private readonly Dictionary<string, List<MyMockingInventory.MyGameInventoryItemDefinitionRaw>> m_achievementDefinitions = new Dictionary<string, List<MyMockingInventory.MyGameInventoryItemDefinitionRaw>>();
    private readonly Dictionary<int, MyMockingInventory.MyGameInventoryItemDefinitionRaw> m_inventoryDefinitionsDictionary = new Dictionary<int, MyMockingInventory.MyGameInventoryItemDefinitionRaw>();
    private readonly HashSet<int> m_achievementItems = new HashSet<int>();
    private uint m_alwaysSupportedDlcId;
    private IMyGameService m_service;

    public ICollection<MyGameInventoryItem> InventoryItems => this.m_inventory.Values;

    public IDictionary<int, MyGameInventoryItemDefinition> Definitions => (IDictionary<int, MyGameInventoryItemDefinition>) this.m_itemDefinitions;

    public int RecycleTokens { get; }

    public event EventHandler InventoryRefreshed;

    public event EventHandler NoItemsReceived;

    public event EventHandler<MyGameItemsEventArgs> CheckItemDataReady;

    public event EventHandler<MyGameItemsEventArgs> ItemsAdded;

    public MyMockingInventory(IMyGameService service) => this.m_service = service;

    public void InitInventory()
    {
      this.InitDefinitions();
      this.PopulateInventoryFromDLCs();
    }

    public void InitInventoryForUser() => this.PopulateInventoryFromDLCs();

    public void GetAllItems() => this.InvokeOnMainThread((Action) (() =>
    {
      EventHandler inventoryRefreshed = this.InventoryRefreshed;
      if (inventoryRefreshed == null)
        return;
      inventoryRefreshed((object) this, EventArgs.Empty);
    }));

    public MyGameInventoryItemDefinition GetInventoryItemDefinition(
      string assetModifierId)
    {
      return this.m_itemDefinitions.Values.FirstOrDefault<MyGameInventoryItemDefinition>((Func<MyGameInventoryItemDefinition, bool>) (x => x.AssetModifierId == assetModifierId));
    }

    public bool HasInventoryItem(ulong id) => this.m_inventory.ContainsKey(id);

    public void TriggerPersonalContainer()
    {
    }

    public void TriggerCompetitiveContainer()
    {
    }

    public void GetItemCheckData(MyGameInventoryItem item)
    {
    }

    public List<MyGameInventoryItem> CheckItemData(
      byte[] checkData,
      out bool checkResult)
    {
      List<MyGameInventoryItem> gameInventoryItemList = MyInventoryHelper.CheckItemData(checkData, out checkResult);
      if (checkResult)
      {
        foreach (MyGameInventoryItem gameInventoryItem in gameInventoryItemList)
          this.m_itemDefinitions.TryAdd(gameInventoryItem.ItemDefinition.ID, gameInventoryItem.ItemDefinition);
      }
      return gameInventoryItemList;
    }

    public void GetItemsCheckData(List<MyGameInventoryItem> items, Action<byte[]> completedAction)
    {
      foreach (MyGameInventoryItem gameInventoryItem in items)
        this.m_itemDefinitions.TryAdd(gameInventoryItem.ItemDefinition.ID, gameInventoryItem.ItemDefinition);
      byte[] data = MyInventoryHelper.GetItemsCheckData(items);
      if (completedAction == null)
        return;
      this.InvokeOnMainThread((Action) (() => completedAction(data)));
    }

    public void GetItemCheckData(MyGameInventoryItem item, Action<byte[]> completedAction)
    {
      byte[] data = MyInventoryHelper.GetItemCheckData(item);
      if (item != null)
      {
        this.m_itemDefinitions.TryAdd(item.ItemDefinition.ID, item.ItemDefinition);
        MyGameItemsEventArgs args = new MyGameItemsEventArgs()
        {
          CheckData = data,
          NewItems = new List<MyGameInventoryItem>()
          {
            item
          }
        };
        if (this.CheckItemDataReady != null)
          this.InvokeOnMainThread((Action) (() =>
          {
            EventHandler<MyGameItemsEventArgs> checkItemDataReady = this.CheckItemDataReady;
            if (checkItemDataReady == null)
              return;
            checkItemDataReady((object) this, args);
          }));
      }
      if (completedAction == null)
        return;
      this.InvokeOnMainThread((Action) (() => completedAction(data)));
    }

    public void ConsumeItem(MyGameInventoryItem item)
    {
    }

    public bool RecycleItem(MyGameInventoryItem item) => false;

    public bool CraftSkin(MyGameInventoryItemQuality quality) => false;

    public uint GetCraftingCost(MyGameInventoryItemQuality quality) => 0;

    public uint GetRecyclingReward(MyGameInventoryItemQuality quality) => 0;

    public bool HasInventoryItemWithDefinitionId(int id) => this.m_inventory.Values.Any<MyGameInventoryItem>((Func<MyGameInventoryItem, bool>) (x => x.ItemDefinition.ID == id));

    public bool HasInventoryItem(string assetModifierId) => this.m_inventory.Values.Any<MyGameInventoryItem>((Func<MyGameInventoryItem, bool>) (x => x.ItemDefinition.AssetModifierId == assetModifierId));

    private void InitDefinitions()
    {
      this.m_dlcDefinitions.Clear();
      this.m_achievementDefinitions.Clear();
      this.m_inventoryDefinitionsDictionary.Clear();
      this.m_itemDefinitions.Clear();
      this.m_itemToDlc.Clear();
      this.m_achievementItems.Clear();
      MyMockingInventory.MyItemDefinitions myItemDefinitions1 = this.AddDefinitionsFromFile("DataPlatform/InventoryItems.json");
      MyMockingInventory.MyItemDefinitions myItemDefinitions2 = this.AddDefinitionsFromFile("DataPlatform/InventoryItemsXBL.json");
      myItemDefinitions1.items.AddRange((IEnumerable<MyMockingInventory.MyGameInventoryItemDefinitionRaw>) myItemDefinitions2.items);
      foreach (MyMockingInventory.MyGameInventoryItemDefinitionRaw def in myItemDefinitions1.items)
      {
        if (!string.IsNullOrEmpty(def.promo))
        {
          string[] strArray = def.promo.Split(':');
          if (strArray.Length == 2)
          {
            if (strArray[0] == "owns")
            {
              uint result;
              if (uint.TryParse(strArray[1], out result))
              {
                bool supported = (int) result == (int) this.m_alwaysSupportedDlcId || this.m_service.IsDlcSupported(result);
                this.AddDlcItem(result, def, supported);
                if (supported)
                {
                  List<MyMockingInventory.MyGameInventoryItemDefinitionRaw> itemDefinitionRawList;
                  if (this.m_dlcDefinitions.TryGetValue(result, out itemDefinitionRawList))
                    itemDefinitionRawList.Add(def);
                  else
                    this.m_dlcDefinitions.Add(result, new List<MyMockingInventory.MyGameInventoryItemDefinitionRaw>()
                    {
                      def
                    });
                }
              }
            }
            else if (strArray[0] == "ach")
            {
              List<MyMockingInventory.MyGameInventoryItemDefinitionRaw> itemDefinitionRawList;
              if (this.m_achievementDefinitions.TryGetValue(strArray[1], out itemDefinitionRawList))
                itemDefinitionRawList.Add(def);
              else
                this.m_achievementDefinitions.Add(strArray[1], new List<MyMockingInventory.MyGameInventoryItemDefinitionRaw>()
                {
                  def
                });
              this.AddAchievementItem(def);
            }
          }
        }
      }
      foreach (MyMockingInventory.MyGameInventoryItemDefinitionRaw itemDefinitionRaw in myItemDefinitions1.items)
      {
        if (!this.m_itemToDlc.ContainsKey(itemDefinitionRaw.itemdefid) && !this.m_achievementItems.Contains(itemDefinitionRaw.itemdefid))
        {
          this.m_itemDefinitions.Remove<int, MyGameInventoryItemDefinition>(itemDefinitionRaw.itemdefid);
          this.m_inventoryDefinitionsDictionary.Remove(itemDefinitionRaw.itemdefid);
        }
      }
    }

    private MyMockingInventory.MyItemDefinitions AddDefinitionsFromFile(
      string dataplatformInventoryitemsJson)
    {
      MyMockingInventory.MyItemDefinitions myItemDefinitions = JsonMapper.ToObject<MyMockingInventory.MyItemDefinitions>(new JsonReader((TextReader) new StreamReader(Path.Combine(MyFileSystem.ContentPath, dataplatformInventoryitemsJson))));
      this.m_alwaysSupportedDlcId = myItemDefinitions.appid;
      foreach (MyMockingInventory.MyGameInventoryItemDefinitionRaw itemDefinitionRaw in myItemDefinitions.items)
      {
        this.m_inventoryDefinitionsDictionary.ContainsKey(itemDefinitionRaw.itemdefid);
        this.m_inventoryDefinitionsDictionary[itemDefinitionRaw.itemdefid] = itemDefinitionRaw;
        MyGameInventoryItemDefinition definition = itemDefinitionRaw.CreateDefinition();
        this.m_itemDefinitions.TryAdd(definition.ID, definition);
      }
      return myItemDefinitions;
    }

    private void AddAchievementItem(
      MyMockingInventory.MyGameInventoryItemDefinitionRaw def)
    {
      this.m_achievementItems.Add(def.itemdefid);
      this.ParseBundle(def, (Action<MyMockingInventory.MyGameInventoryItemDefinitionRaw>) (x => this.AddAchievementItem(x)));
    }

    private void AddDlcItem(
      uint dlcId,
      MyMockingInventory.MyGameInventoryItemDefinitionRaw def,
      bool supported)
    {
      if (supported)
        this.m_itemToDlc[def.itemdefid] = dlcId;
      this.ParseBundle(def, (Action<MyMockingInventory.MyGameInventoryItemDefinitionRaw>) (x => this.AddDlcItem(dlcId, x, supported)));
    }

    private void PopulateInventory(
      MyMockingInventory.MyGameInventoryItemDefinitionRaw item)
    {
      MyGameInventoryItemDefinition definition = item.CreateDefinition();
      MyGameInventoryItem gameInventoryItem = new MyGameInventoryItem()
      {
        ID = (ulong) definition.ID,
        ItemDefinition = definition,
        Quantity = 1
      };
      this.m_inventory.TryAdd(gameInventoryItem.ID, gameInventoryItem);
      this.ParseBundle(item, new Action<MyMockingInventory.MyGameInventoryItemDefinitionRaw>(this.PopulateInventory));
    }

    private void ParseBundle(
      MyMockingInventory.MyGameInventoryItemDefinitionRaw item,
      Action<MyMockingInventory.MyGameInventoryItemDefinitionRaw> process)
    {
      if (string.IsNullOrEmpty(item.bundle))
        return;
      string bundle = item.bundle;
      char[] chArray = new char[1]{ ';' };
      foreach (string s in bundle.Split(chArray))
      {
        int result;
        MyMockingInventory.MyGameInventoryItemDefinitionRaw itemDefinitionRaw;
        if (int.TryParse(s, out result) && this.m_inventoryDefinitionsDictionary.TryGetValue(result, out itemDefinitionRaw))
          process(itemDefinitionRaw);
      }
    }

    public void PopulateInventoryFromDLCs()
    {
      this.m_inventory.Clear();
      List<MyMockingInventory.MyGameInventoryItemDefinitionRaw> itemDefinitionRawList;
      if (this.m_dlcDefinitions.TryGetValue(this.m_alwaysSupportedDlcId, out itemDefinitionRawList))
      {
        foreach (MyMockingInventory.MyGameInventoryItemDefinitionRaw itemDefinitionRaw in itemDefinitionRawList)
          this.PopulateInventory(itemDefinitionRaw);
      }
      int dlcCount = this.m_service.GetDLCCount();
      for (int index = 0; index < dlcCount; ++index)
      {
        uint dlcId;
        if (this.m_service.GetDLCDataByIndex(index, out dlcId, out bool _, out string _, 128) && this.m_service.IsDlcInstalled(dlcId) && this.m_dlcDefinitions.TryGetValue(dlcId, out itemDefinitionRawList))
        {
          foreach (MyMockingInventory.MyGameInventoryItemDefinitionRaw itemDefinitionRaw in itemDefinitionRawList)
            this.PopulateInventory(itemDefinitionRaw);
        }
      }
      foreach (KeyValuePair<string, List<MyMockingInventory.MyGameInventoryItemDefinitionRaw>> achievementDefinition in this.m_achievementDefinitions)
      {
        IMyAchievement achievement = this.m_service.GetAchievement(achievementDefinition.Key);
        if (achievement != null)
        {
          if (achievement.IsUnlocked)
          {
            foreach (MyMockingInventory.MyGameInventoryItemDefinitionRaw itemDefinitionRaw in achievementDefinition.Value)
              this.PopulateInventory(itemDefinitionRaw);
          }
          else
          {
            string achievementName = achievementDefinition.Key;
            achievement.OnUnlocked += (Action) (() =>
            {
              foreach (MyMockingInventory.MyGameInventoryItemDefinitionRaw itemDefinitionRaw in this.m_achievementDefinitions[achievementName])
                this.PopulateInventory(itemDefinitionRaw);
              this.InvokeOnMainThread((Action) (() =>
              {
                EventHandler inventoryRefreshed = this.InventoryRefreshed;
                if (inventoryRefreshed == null)
                  return;
                inventoryRefreshed((object) this, EventArgs.Empty);
              }));
            });
          }
        }
      }
      this.InvokeOnMainThread((Action) (() =>
      {
        EventHandler inventoryRefreshed = this.InventoryRefreshed;
        if (inventoryRefreshed == null)
          return;
        inventoryRefreshed((object) this, EventArgs.Empty);
      }));
    }

    public void AddUnownedItems()
    {
      foreach (KeyValuePair<int, MyGameInventoryItemDefinition> definition in (IEnumerable<KeyValuePair<int, MyGameInventoryItemDefinition>>) this.Definitions)
      {
        MyGameInventoryItem gameInventoryItem = new MyGameInventoryItem()
        {
          ID = (ulong) definition.Key,
          ItemDefinition = definition.Value,
          Quantity = 1
        };
        this.m_inventory.TryAdd(gameInventoryItem.ID, gameInventoryItem);
      }
    }

    private void InvokeOnMainThread(Action action) => this.m_updateThreadInvocationQueue.Enqueue(action);

    public void Update()
    {
      Action result;
      while (this.m_updateThreadInvocationQueue.TryDequeue(out result))
        result();
    }

    public IEnumerable<MyGameInventoryItemDefinition> GetDefinitionsForSlot(
      MyGameInventoryItemSlot slot)
    {
      return this.m_itemDefinitions.Values.Where<MyGameInventoryItemDefinition>((Func<MyGameInventoryItemDefinition, bool>) (e => e.ItemSlot == slot));
    }

    public bool ItemToDlc(int itemId, out uint dlcId) => this.m_itemToDlc.TryGetValue(itemId, out dlcId);

    protected virtual void OnNoItemsReceived()
    {
      EventHandler noItemsReceived = this.NoItemsReceived;
      if (noItemsReceived == null)
        return;
      noItemsReceived((object) this, EventArgs.Empty);
    }

    protected virtual void OnItemsAdded(MyGameItemsEventArgs e)
    {
      EventHandler<MyGameItemsEventArgs> itemsAdded = this.ItemsAdded;
      if (itemsAdded == null)
        return;
      itemsAdded((object) this, e);
    }

    private class MyItemDefinitions
    {
      public uint appid;
      public List<MyMockingInventory.MyGameInventoryItemDefinitionRaw> items;
    }

    private class MyGameInventoryItemDefinitionRaw
    {
      public int itemdefid;
      public string name;
      public string type;
      public string name_color;
      public string background_color;
      public string icon_texture;
      public bool hidden;
      public bool store_hidden;
      public string item_slot;
      public int item_quality;
      public string asset_modifier_id;
      public string tool_name;
      public string exchange;
      public string promo;
      public string bundle;

      public MyGameInventoryItemDefinition CreateDefinition()
      {
        MyGameInventoryItemDefinitionType result1;
        MyGameInventoryItemSlot result2;
        return new MyGameInventoryItemDefinition()
        {
          ID = this.itemdefid,
          Name = this.name,
          DisplayType = this.type,
          AssetModifierId = this.asset_modifier_id,
          BackgroundColor = this.background_color,
          IconTexture = this.icon_texture,
          NameColor = this.name_color,
          ToolName = this.tool_name,
          IsStoreHidden = this.store_hidden,
          Hidden = this.hidden,
          Exchange = this.exchange,
          DefinitionType = !Enum.TryParse<MyGameInventoryItemDefinitionType>(this.type, out result1) ? MyGameInventoryItemDefinitionType.none : result1,
          ItemSlot = !Enum.TryParse<MyGameInventoryItemSlot>(this.item_slot, out result2) ? MyGameInventoryItemSlot.None : result2,
          ItemQuality = (MyGameInventoryItemQuality) this.item_quality
        };
      }
    }
  }
}
