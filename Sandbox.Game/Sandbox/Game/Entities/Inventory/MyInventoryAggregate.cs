// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Inventory.MyInventoryAggregate
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Entities.Inventory
{
  [MyComponentBuilder(typeof (MyObjectBuilder_InventoryAggregate), true)]
  [StaticEventOwner]
  public class MyInventoryAggregate : MyInventoryBase, IMyComponentAggregate, IMyEventProxy, IMyEventOwner
  {
    private MyAggregateComponentList m_children = new MyAggregateComponentList();
    private List<MyComponentBase> tmp_list = new List<MyComponentBase>();
    private List<MyPhysicalInventoryItem> m_allItems = new List<MyPhysicalInventoryItem>();
    private float? m_forcedPriority;
    private int m_inventoryCount;

    public virtual event Action<MyInventoryAggregate, MyInventoryBase> OnAfterComponentAdd;

    public virtual event Action<MyInventoryAggregate, MyInventoryBase> OnBeforeComponentRemove;

    public override MyFixedPoint CurrentMass
    {
      get
      {
        float num = 0.0f;
        foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
          num += (float) myInventoryBase.CurrentMass;
        return (MyFixedPoint) num;
      }
    }

    public override MyFixedPoint MaxMass
    {
      get
      {
        float num = 0.0f;
        foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
          num += (float) myInventoryBase.MaxMass;
        return (MyFixedPoint) num;
      }
    }

    public override MyFixedPoint CurrentVolume
    {
      get
      {
        float num = 0.0f;
        foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
          num += (float) myInventoryBase.CurrentVolume;
        return (MyFixedPoint) num;
      }
    }

    public override MyFixedPoint MaxVolume
    {
      get
      {
        float num = 0.0f;
        foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
          num += (float) myInventoryBase.MaxVolume;
        return (MyFixedPoint) num;
      }
    }

    public override int MaxItemCount
    {
      get
      {
        int num1 = 0;
        foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
        {
          long num2 = (long) num1 + (long) myInventoryBase.MaxItemCount;
          num1 = num2 <= (long) int.MaxValue ? (int) num2 : int.MaxValue;
        }
        return num1;
      }
    }

    public override float? ForcedPriority
    {
      get => this.m_forcedPriority;
      set
      {
        this.m_forcedPriority = value;
        foreach (MyComponentBase myComponentBase in this.m_children.Reader)
          (myComponentBase as MyInventoryBase).ForcedPriority = value;
      }
    }

    public event Action<MyInventoryAggregate, int> OnInventoryCountChanged;

    public int InventoryCount
    {
      get => this.m_inventoryCount;
      private set
      {
        if (this.m_inventoryCount == value)
          return;
        int num = value - this.m_inventoryCount;
        this.m_inventoryCount = value;
        if (this.OnInventoryCountChanged == null)
          return;
        this.OnInventoryCountChanged(this, num);
      }
    }

    public MyInventoryAggregate()
      : base("Inventory")
    {
    }

    public MyInventoryAggregate(string inventoryId)
      : base(inventoryId)
    {
    }

    public void Init()
    {
      foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
        myInventoryBase.ContentsChanged += new Action<MyInventoryBase>(this.child_OnContentsChanged);
    }

    public void DetachCallbacks()
    {
      foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
        myInventoryBase.ContentsChanged -= new Action<MyInventoryBase>(this.child_OnContentsChanged);
    }

    ~MyInventoryAggregate() => this.DetachCallbacks();

    public override MyFixedPoint ComputeAmountThatFits(
      MyDefinitionId contentId,
      float volumeRemoved = 0.0f,
      float massRemoved = 0.0f)
    {
      float num = 0.0f;
      foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
        num += (float) myInventoryBase.ComputeAmountThatFits(contentId, volumeRemoved, massRemoved);
      return (MyFixedPoint) num;
    }

    public override MyFixedPoint GetItemAmount(
      MyDefinitionId contentId,
      MyItemFlags flags = MyItemFlags.None,
      bool substitute = false)
    {
      float num = 0.0f;
      foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
        num += (float) myInventoryBase.GetItemAmount(contentId, flags, substitute);
      return (MyFixedPoint) num;
    }

    public override bool AddItems(MyFixedPoint amount, MyObjectBuilder_Base objectBuilder)
    {
      MyFixedPoint amountThatFits = this.ComputeAmountThatFits(objectBuilder.GetId(), 0.0f, 0.0f);
      MyFixedPoint myFixedPoint = amount;
      if (amount <= amountThatFits)
      {
        foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
        {
          MyFixedPoint amount1 = myInventoryBase.ComputeAmountThatFits(objectBuilder.GetId());
          if (amount1 > myFixedPoint)
            amount1 = myFixedPoint;
          if (amount1 > (MyFixedPoint) 0 && myInventoryBase.AddItems(amount1, objectBuilder))
            myFixedPoint -= amount1;
          if (myFixedPoint == (MyFixedPoint) 0)
            break;
        }
      }
      return myFixedPoint == (MyFixedPoint) 0;
    }

    public override MyFixedPoint RemoveItemsOfType(
      MyFixedPoint amount,
      MyDefinitionId contentId,
      MyItemFlags flags = MyItemFlags.None,
      bool spawn = false)
    {
      MyFixedPoint amount1 = amount;
      foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
        amount1 -= myInventoryBase.RemoveItemsOfType(amount1, contentId, flags, spawn);
      return amount - amount1;
    }

    public MyInventoryBase GetInventory(MyStringHash id)
    {
      foreach (MyComponentBase myComponentBase in this.m_children.Reader)
      {
        MyInventoryBase myInventoryBase = myComponentBase as MyInventoryBase;
        if (myInventoryBase.InventoryId == id)
          return myInventoryBase;
      }
      return (MyInventoryBase) null;
    }

    public MyAggregateComponentList ChildList => this.m_children;

    public void AfterComponentAdd(MyComponentBase component)
    {
      MyInventoryBase myInventoryBase = component as MyInventoryBase;
      myInventoryBase.ForcedPriority = this.ForcedPriority;
      myInventoryBase.ContentsChanged += new Action<MyInventoryBase>(this.child_OnContentsChanged);
      if (component is MyInventory)
        ++this.InventoryCount;
      else if (component is MyInventoryAggregate)
      {
        (component as MyInventoryAggregate).OnInventoryCountChanged += new Action<MyInventoryAggregate, int>(this.OnChildAggregateCountChanged);
        this.InventoryCount += (component as MyInventoryAggregate).InventoryCount;
      }
      if (this.OnAfterComponentAdd == null)
        return;
      this.OnAfterComponentAdd(this, myInventoryBase);
    }

    private void OnChildAggregateCountChanged(MyInventoryAggregate obj, int change) => this.InventoryCount += change;

    public void BeforeComponentRemove(MyComponentBase component)
    {
      MyInventoryBase myInventoryBase = component as MyInventoryBase;
      myInventoryBase.ForcedPriority = new float?();
      myInventoryBase.ContentsChanged -= new Action<MyInventoryBase>(this.child_OnContentsChanged);
      if (this.OnBeforeComponentRemove != null)
        this.OnBeforeComponentRemove(this, myInventoryBase);
      switch (component)
      {
        case MyInventory _:
          --this.InventoryCount;
          break;
        case MyInventoryAggregate _:
          (component as MyInventoryAggregate).OnInventoryCountChanged -= new Action<MyInventoryAggregate, int>(this.OnChildAggregateCountChanged);
          this.InventoryCount -= (component as MyInventoryAggregate).InventoryCount;
          break;
      }
    }

    private void child_OnContentsChanged(MyComponentBase obj) => this.OnContentsChanged();

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_InventoryAggregate inventoryAggregate = base.Serialize(false) as MyObjectBuilder_InventoryAggregate;
      ListReader<MyComponentBase> reader = this.m_children.Reader;
      if (reader.Count > 0)
      {
        inventoryAggregate.Inventories = new List<MyObjectBuilder_InventoryBase>(reader.Count);
        foreach (MyComponentBase myComponentBase in reader)
        {
          if (myComponentBase.Serialize() is MyObjectBuilder_InventoryBase builderInventoryBase)
            inventoryAggregate.Inventories.Add(builderInventoryBase);
        }
      }
      return (MyObjectBuilder_ComponentBase) inventoryAggregate;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      base.Deserialize(builder);
      if (!(builder is MyObjectBuilder_InventoryAggregate inventoryAggregate) || inventoryAggregate.Inventories == null)
        return;
      foreach (MyObjectBuilder_InventoryBase inventory in inventoryAggregate.Inventories)
      {
        MyComponentBase instanceByTypeId = MyComponentFactory.CreateInstanceByTypeId(inventory.TypeId);
        instanceByTypeId.Deserialize((MyObjectBuilder_ComponentBase) inventory);
        this.AddComponent(instanceByTypeId);
      }
    }

    public override void CountItems(
      Dictionary<MyDefinitionId, MyFixedPoint> itemCounts)
    {
      foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
        myInventoryBase.CountItems(itemCounts);
    }

    public override void ApplyChanges(List<MyComponentChange> changes)
    {
      foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
        myInventoryBase.ApplyChanges(changes);
    }

    public override bool ItemsCanBeAdded(MyFixedPoint amount, IMyInventoryItem item)
    {
      foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
      {
        if (myInventoryBase.ItemsCanBeAdded(amount, item))
          return true;
      }
      return false;
    }

    public override bool ItemsCanBeRemoved(MyFixedPoint amount, IMyInventoryItem item)
    {
      foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
      {
        if (myInventoryBase.ItemsCanBeRemoved(amount, item))
          return true;
      }
      return false;
    }

    public override bool Add(IMyInventoryItem item, MyFixedPoint amount)
    {
      foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
      {
        if (myInventoryBase.ItemsCanBeAdded(amount, item) && myInventoryBase.Add(item, amount))
          return true;
      }
      return false;
    }

    public override bool Remove(IMyInventoryItem item, MyFixedPoint amount)
    {
      foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
      {
        if (myInventoryBase.ItemsCanBeRemoved(amount, item) && myInventoryBase.Remove(item, amount))
          return true;
      }
      return false;
    }

    public override List<MyPhysicalInventoryItem> GetItems()
    {
      this.m_allItems.Clear();
      foreach (MyInventoryBase myInventoryBase in this.m_children.Reader)
        this.m_allItems.AddRange((IEnumerable<MyPhysicalInventoryItem>) myInventoryBase.GetItems());
      return this.m_allItems;
    }

    public override void OnContentsChanged()
    {
      this.RaiseContentsChanged();
      if (!Sync.IsServer || !this.RemoveEntityOnEmpty || this.GetItemsCount() != 0)
        return;
      this.Container.Entity.Close();
    }

    public override void OnBeforeContentsChanged() => this.RaiseBeforeContentsChanged();

    public override void OnContentsAdded(MyPhysicalInventoryItem item, MyFixedPoint amount)
    {
      this.RaiseContentsAdded(item, amount);
      this.RaiseInventoryContentChanged(item, amount);
    }

    public override void OnContentsRemoved(MyPhysicalInventoryItem item, MyFixedPoint amount)
    {
      this.RaiseContentsRemoved(item, amount);
      this.RaiseInventoryContentChanged(item, -amount);
    }

    public override void ConsumeItem(
      MyDefinitionId itemId,
      MyFixedPoint amount,
      long consumerEntityId = 0)
    {
      SerializableDefinitionId serializableDefinitionId = (SerializableDefinitionId) itemId;
      MyMultiplayer.RaiseEvent<MyInventoryAggregate, MyFixedPoint, SerializableDefinitionId, long>(this, (Func<MyInventoryAggregate, Action<MyFixedPoint, SerializableDefinitionId, long>>) (x => new Action<MyFixedPoint, SerializableDefinitionId, long>(x.InventoryConsumeItem_Implementation)), amount, serializableDefinitionId, consumerEntityId);
    }

    public override int GetInventoryCount() => this.InventoryCount;

    public override MyInventoryBase IterateInventory(
      int searchIndex,
      int currentIndex)
    {
      foreach (MyComponentBase myComponentBase in this.ChildList.Reader)
      {
        if (myComponentBase is MyInventoryBase myInventoryBase)
        {
          MyInventoryBase myInventoryBase = myInventoryBase.IterateInventory(searchIndex, currentIndex);
          if (myInventoryBase != null)
            return myInventoryBase;
          if (myInventoryBase is MyInventory)
            ++currentIndex;
        }
      }
      return (MyInventoryBase) null;
    }

    [Event(null, 479)]
    [Reliable]
    [Server]
    private void InventoryConsumeItem_Implementation(
      MyFixedPoint amount,
      SerializableDefinitionId itemId,
      long consumerEntityId)
    {
      if (consumerEntityId != 0L && !MyEntities.EntityExists(consumerEntityId))
        return;
      MyFixedPoint itemAmount = this.GetItemAmount((MyDefinitionId) itemId, MyItemFlags.None, false);
      if (itemAmount < amount)
        amount = itemAmount;
      MyEntity myEntity = (MyEntity) null;
      if (consumerEntityId != 0L)
      {
        myEntity = MyEntities.GetEntityById(consumerEntityId);
        if (myEntity == null)
          return;
      }
      if (myEntity.Components != null && MyDefinitionManager.Static.GetDefinition((MyDefinitionId) itemId) is MyUsableItemDefinition definition && myEntity is MyCharacter myCharacter)
        myCharacter.SoundComp.StartSecondarySound(definition.UseSound, true);
      if (false)
        return;
      this.RemoveItemsOfType(amount, (MyDefinitionId) itemId, MyItemFlags.None, false);
    }

    public static MyInventoryAggregate FixInputOutputInventories(
      MyInventoryAggregate inventoryAggregate,
      MyInventoryConstraint inputInventoryConstraint,
      MyInventoryConstraint outputInventoryConstraint)
    {
      MyInventory myInventory1 = (MyInventory) null;
      MyInventory myInventory2 = (MyInventory) null;
      foreach (MyComponentBase myComponentBase in inventoryAggregate.ChildList.Reader)
      {
        if (myComponentBase is MyInventory myInventory3 && myInventory3.GetItemsCount() > 0)
        {
          if (myInventory1 == null)
          {
            bool flag = true;
            if (inputInventoryConstraint != null)
            {
              foreach (MyPhysicalInventoryItem self in myInventory3.GetItems())
                flag &= inputInventoryConstraint.Check(self.GetDefinitionId());
            }
            if (flag)
              myInventory1 = myInventory3;
          }
          if (myInventory2 == null && myInventory1 != myInventory3)
          {
            bool flag = true;
            if (outputInventoryConstraint != null)
            {
              foreach (MyPhysicalInventoryItem self in myInventory3.GetItems())
                flag &= outputInventoryConstraint.Check(self.GetDefinitionId());
            }
            if (flag)
              myInventory2 = myInventory3;
          }
        }
      }
      if (myInventory1 == null || myInventory2 == null)
      {
        foreach (MyComponentBase myComponentBase in inventoryAggregate.ChildList.Reader)
        {
          if (myComponentBase is MyInventory myInventory3)
          {
            if (myInventory1 == null)
              myInventory1 = myInventory3;
            else if (myInventory2 == null)
              myInventory2 = myInventory3;
            else
              break;
          }
        }
      }
      inventoryAggregate.RemoveComponent((MyComponentBase) myInventory1);
      inventoryAggregate.RemoveComponent((MyComponentBase) myInventory2);
      MyInventoryAggregate aggregate = new MyInventoryAggregate();
      aggregate.AddComponent((MyComponentBase) myInventory1);
      aggregate.AddComponent((MyComponentBase) myInventory2);
      return aggregate;
    }

    [SpecialName]
    MyComponentContainer IMyComponentAggregate.get_ContainerBase() => this.ContainerBase;

    protected sealed class InventoryConsumeItem_Implementation\u003C\u003EVRage_MyFixedPoint\u0023VRage_ObjectBuilders_SerializableDefinitionId\u0023System_Int64 : ICallSite<MyInventoryAggregate, MyFixedPoint, SerializableDefinitionId, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyInventoryAggregate @this,
        in MyFixedPoint amount,
        in SerializableDefinitionId itemId,
        in long consumerEntityId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.InventoryConsumeItem_Implementation(amount, itemId, consumerEntityId);
      }
    }

    private class Sandbox_Game_Entities_Inventory_MyInventoryAggregate\u003C\u003EActor : IActivator, IActivator<MyInventoryAggregate>
    {
      object IActivator.CreateInstance() => (object) new MyInventoryAggregate();

      MyInventoryAggregate IActivator<MyInventoryAggregate>.CreateInstance() => new MyInventoryAggregate();
    }
  }
}
