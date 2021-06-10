// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyConstructionStockpile
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
  public class MyConstructionStockpile
  {
    private List<MyStockpileItem> m_items = new List<MyStockpileItem>();
    private static List<MyStockpileItem> m_syncItems = new List<MyStockpileItem>();

    public int LastChangeStamp { get; private set; }

    public MyConstructionStockpile() => this.LastChangeStamp = 0;

    public MyObjectBuilder_ConstructionStockpile GetObjectBuilder()
    {
      MyObjectBuilder_ConstructionStockpile newObject1 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ConstructionStockpile>();
      newObject1.Items = new MyObjectBuilder_StockpileItem[this.m_items.Count];
      for (int index = 0; index < this.m_items.Count; ++index)
      {
        MyStockpileItem myStockpileItem = this.m_items[index];
        MyObjectBuilder_StockpileItem newObject2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_StockpileItem>();
        newObject2.Amount = myStockpileItem.Amount;
        newObject2.PhysicalContent = myStockpileItem.Content;
        newObject1.Items[index] = newObject2;
      }
      return newObject1;
    }

    public void Init(
      MyObjectBuilder_ConstructionStockpile objectBuilder)
    {
      this.m_items.Clear();
      if (objectBuilder == null)
        return;
      foreach (MyObjectBuilder_StockpileItem builderStockpileItem in objectBuilder.Items)
      {
        if (builderStockpileItem.Amount > 0)
          this.m_items.Add(new MyStockpileItem()
          {
            Amount = builderStockpileItem.Amount,
            Content = builderStockpileItem.PhysicalContent
          });
      }
      ++this.LastChangeStamp;
    }

    public void Init(MyObjectBuilder_Inventory objectBuilder)
    {
      this.m_items.Clear();
      if (objectBuilder == null)
        return;
      foreach (MyObjectBuilder_InventoryItem builderInventoryItem in objectBuilder.Items)
      {
        if (builderInventoryItem.Amount > (MyFixedPoint) 0)
          this.m_items.Add(new MyStockpileItem()
          {
            Amount = (int) builderInventoryItem.Amount,
            Content = builderInventoryItem.PhysicalContent
          });
      }
      ++this.LastChangeStamp;
    }

    public bool IsEmpty() => this.m_items.Count == 0;

    public void ClearSyncList() => MyConstructionStockpile.m_syncItems.Clear();

    public List<MyStockpileItem> GetSyncList() => MyConstructionStockpile.m_syncItems;

    public bool AddItems(int count, MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None)
    {
      MyObjectBuilder_PhysicalObject newObject = (MyObjectBuilder_PhysicalObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) contentId);
      if (newObject == null)
        return false;
      newObject.Flags = flags;
      return this.AddItems(count, newObject);
    }

    public bool AddItems(int count, MyObjectBuilder_PhysicalObject physicalObject)
    {
      int index = 0;
      foreach (MyStockpileItem myStockpileItem in this.m_items)
      {
        if (!myStockpileItem.Content.CanStack(physicalObject))
          ++index;
        else
          break;
      }
      if (index == this.m_items.Count)
      {
        if (count >= int.MaxValue)
          return false;
        MyStockpileItem diffItem = new MyStockpileItem();
        diffItem.Amount = count;
        diffItem.Content = physicalObject;
        this.m_items.Add(diffItem);
        this.AddSyncItem(diffItem);
        ++this.LastChangeStamp;
        return true;
      }
      if ((long) this.m_items[index].Amount + (long) count >= (long) int.MaxValue)
        return false;
      this.m_items[index] = new MyStockpileItem()
      {
        Amount = this.m_items[index].Amount + count,
        Content = this.m_items[index].Content
      };
      this.AddSyncItem(new MyStockpileItem()
      {
        Content = this.m_items[index].Content,
        Amount = count
      });
      ++this.LastChangeStamp;
      return true;
    }

    public bool RemoveItems(int count, MyObjectBuilder_PhysicalObject physicalObject) => this.RemoveItems(count, physicalObject.GetId(), physicalObject.Flags);

    public bool RemoveItems(int count, MyDefinitionId id, MyItemFlags flags = MyItemFlags.None)
    {
      int index = 0;
      foreach (MyStockpileItem myStockpileItem in this.m_items)
      {
        if (!myStockpileItem.Content.CanStack(id.TypeId, id.SubtypeId, flags))
          ++index;
        else
          break;
      }
      return this.RemoveItemsInternal(index, count);
    }

    private bool RemoveItemsInternal(int index, int count)
    {
      if (index >= this.m_items.Count)
        return false;
      if (this.m_items[index].Amount == count)
      {
        MyStockpileItem diffItem = this.m_items[index];
        diffItem.Amount = -diffItem.Amount;
        this.AddSyncItem(diffItem);
        this.m_items.RemoveAt(index);
        ++this.LastChangeStamp;
        return true;
      }
      if (count >= this.m_items[index].Amount)
        return false;
      MyStockpileItem myStockpileItem = new MyStockpileItem();
      myStockpileItem.Amount = this.m_items[index].Amount - count;
      myStockpileItem.Content = this.m_items[index].Content;
      this.m_items[index] = myStockpileItem;
      this.AddSyncItem(new MyStockpileItem()
      {
        Content = myStockpileItem.Content,
        Amount = -count
      });
      ++this.LastChangeStamp;
      return true;
    }

    private void AddSyncItem(MyStockpileItem diffItem)
    {
      int index = 0;
      foreach (MyStockpileItem syncItem in MyConstructionStockpile.m_syncItems)
      {
        if (syncItem.Content.CanStack(diffItem.Content))
        {
          MyConstructionStockpile.m_syncItems[index] = new MyStockpileItem()
          {
            Amount = syncItem.Amount + diffItem.Amount,
            Content = syncItem.Content
          };
          return;
        }
        ++index;
      }
      MyConstructionStockpile.m_syncItems.Add(diffItem);
    }

    public List<MyStockpileItem> GetItems() => this.m_items;

    public int GetItemAmount(MyDefinitionId contentId, MyItemFlags flags = MyItemFlags.None)
    {
      foreach (MyStockpileItem myStockpileItem in this.m_items)
      {
        if (myStockpileItem.Content.CanStack(contentId.TypeId, contentId.SubtypeId, flags))
          return myStockpileItem.Amount;
      }
      return 0;
    }

    internal void Change(List<MyStockpileItem> items)
    {
      int count = this.m_items.Count;
      foreach (MyStockpileItem myStockpileItem1 in items)
      {
        int index;
        for (index = 0; index < count; ++index)
        {
          if (this.m_items[index].Content.CanStack(myStockpileItem1.Content))
          {
            MyStockpileItem myStockpileItem2 = this.m_items[index];
            myStockpileItem2.Amount += myStockpileItem1.Amount;
            this.m_items[index] = myStockpileItem2;
            break;
          }
        }
        if (index == count)
          this.m_items.Add(new MyStockpileItem()
          {
            Amount = myStockpileItem1.Amount,
            Content = myStockpileItem1.Content
          });
      }
      for (int index = this.m_items.Count - 1; index >= 0; --index)
      {
        if (this.m_items[index].Amount == 0)
          this.m_items.RemoveAtFast<MyStockpileItem>(index);
      }
      ++this.LastChangeStamp;
    }
  }
}
