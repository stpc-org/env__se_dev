// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudChangedInventoryItems
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Gui
{
  public class MyHudChangedInventoryItems
  {
    private const int GROUP_ITEMS_COUNT = 6;
    private const float TIME_TO_REMOVE_ITEM_SEC = 5f;
    private List<MyHudChangedInventoryItems.MyItemInfo> m_items = new List<MyHudChangedInventoryItems.MyItemInfo>();

    public ListReader<MyHudChangedInventoryItems.MyItemInfo> Items => new ListReader<MyHudChangedInventoryItems.MyItemInfo>(this.m_items);

    public bool Visible { get; private set; }

    public MyHudChangedInventoryItems() => this.Visible = false;

    public void Show() => this.Visible = true;

    public void Hide() => this.Visible = false;

    private void AddItem(MyHudChangedInventoryItems.MyItemInfo item)
    {
      double val1 = MySession.Static.ElapsedGameTime.TotalSeconds;
      if (this.m_items.Count > 0)
      {
        MyHudChangedInventoryItems.MyItemInfo myItemInfo = this.m_items[this.m_items.Count - 1];
        if (myItemInfo.DefinitionId == item.DefinitionId && myItemInfo.Added == item.Added)
        {
          myItemInfo.ChangedAmount += item.ChangedAmount;
          myItemInfo.TotalAmount = item.TotalAmount;
          if (this.m_items.Count <= 6)
            myItemInfo.RemoveTime = val1 + 5.0;
          this.m_items[this.m_items.Count - 1] = myItemInfo;
          return;
        }
        if (this.m_items.Count >= 6)
        {
          double val2 = this.m_items[this.m_items.Count - 6].AddTime + 5.0;
          val1 = Math.Max(val1, val2);
        }
      }
      item.AddTime = val1;
      item.RemoveTime = val1 + 5.0;
      this.m_items.Add(item);
    }

    public void AddChangedPhysicalInventoryItem(
      MyPhysicalInventoryItem intentoryItem,
      MyFixedPoint changedAmount,
      bool added)
    {
      MyDefinitionBase itemDefinition = intentoryItem.GetItemDefinition();
      if (itemDefinition == null)
        return;
      if (changedAmount < (MyFixedPoint) 0)
        changedAmount = -changedAmount;
      this.AddItem(new MyHudChangedInventoryItems.MyItemInfo()
      {
        DefinitionId = itemDefinition.Id,
        Icons = itemDefinition.Icons,
        TotalAmount = intentoryItem.Amount,
        ChangedAmount = changedAmount,
        Added = added
      });
    }

    public void Update()
    {
      double totalSeconds = MySession.Static.ElapsedGameTime.TotalSeconds;
      for (int index = this.m_items.Count - 1; index >= 0; --index)
      {
        if (totalSeconds - this.Items[index].RemoveTime > 0.0)
          this.m_items.RemoveAt(index);
      }
    }

    public void Clear() => this.m_items.Clear();

    public struct MyItemInfo
    {
      public MyDefinitionId DefinitionId;
      public string[] Icons;
      public MyFixedPoint ChangedAmount;
      public MyFixedPoint TotalAmount;
      public bool Added;
      public double AddTime;
      public double RemoveTime;
    }
  }
}
