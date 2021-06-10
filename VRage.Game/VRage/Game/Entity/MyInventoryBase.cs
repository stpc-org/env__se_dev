// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.MyInventoryBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.VisualScripting;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Entity
{
  [MyComponentType(typeof (MyInventoryBase))]
  [StaticEventOwner]
  public abstract class MyInventoryBase : MyEntityComponentBase, IMyEventProxy, IMyEventOwner
  {
    public bool RemoveEntityOnEmpty;

    public MyStringHash InventoryId { get; private set; }

    public abstract MyFixedPoint CurrentMass { get; }

    public abstract MyFixedPoint MaxMass { get; }

    public abstract int MaxItemCount { get; }

    public abstract MyFixedPoint CurrentVolume { get; }

    public abstract MyFixedPoint MaxVolume { get; }

    public abstract float? ForcedPriority { get; set; }

    public override string ComponentTypeDebugString => "Inventory";

    public override bool AttachSyncToEntity => false;

    public event Action<MyInventoryBase> ContentsChanged;

    public event Action<MyInventoryBase> BeforeContentsChanged;

    public event Action<MyPhysicalInventoryItem, MyFixedPoint> ContentsAdded;

    public event Action<MyPhysicalInventoryItem, MyFixedPoint> ContentsRemoved;

    public event Action<MyInventoryBase, MyPhysicalInventoryItem, MyFixedPoint> InventoryContentChanged;

    public event Action<MyInventoryBase, MyComponentContainer> OwnerChanged;

    public MyInventoryBase(string inventoryId) => this.InventoryId = MyStringHash.GetOrCompute(inventoryId);

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      base.Deserialize(builder);
      this.InventoryId = MyStringHash.GetOrCompute((builder as MyObjectBuilder_InventoryBase).InventoryId ?? "Inventory");
    }

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_InventoryBase builderInventoryBase = base.Serialize() as MyObjectBuilder_InventoryBase;
      builderInventoryBase.InventoryId = this.InventoryId.ToString();
      return (MyObjectBuilder_ComponentBase) builderInventoryBase;
    }

    public override string ToString() => base.ToString() + " - " + this.InventoryId.ToString();

    public abstract MyFixedPoint ComputeAmountThatFits(
      MyDefinitionId contentId,
      float volumeRemoved = 0.0f,
      float massRemoved = 0.0f);

    public abstract MyFixedPoint GetItemAmount(
      MyDefinitionId contentId,
      MyItemFlags flags = MyItemFlags.None,
      bool substitute = false);

    public abstract bool ItemsCanBeAdded(MyFixedPoint amount, IMyInventoryItem item);

    public abstract bool ItemsCanBeRemoved(MyFixedPoint amount, IMyInventoryItem item);

    public abstract bool Add(IMyInventoryItem item, MyFixedPoint amount);

    public abstract bool Remove(IMyInventoryItem item, MyFixedPoint amount);

    public abstract void CountItems(
      Dictionary<MyDefinitionId, MyFixedPoint> itemCounts);

    public abstract void ApplyChanges(List<MyComponentChange> changes);

    public abstract List<MyPhysicalInventoryItem> GetItems();

    public abstract bool AddItems(MyFixedPoint amount, MyObjectBuilder_Base objectBuilder);

    public abstract MyFixedPoint RemoveItemsOfType(
      MyFixedPoint amount,
      MyDefinitionId contentId,
      MyItemFlags flags = MyItemFlags.None,
      bool spawn = false);

    public abstract void OnContentsChanged();

    public abstract void OnBeforeContentsChanged();

    public abstract void OnContentsAdded(MyPhysicalInventoryItem item, MyFixedPoint amount);

    public abstract void OnContentsRemoved(MyPhysicalInventoryItem item, MyFixedPoint amount);

    public virtual int GetItemsCount() => 0;

    public abstract int GetInventoryCount();

    public abstract MyInventoryBase IterateInventory(
      int searchIndex,
      int currentIndex = 0);

    protected void OnOwnerChanged()
    {
      Action<MyInventoryBase, MyComponentContainer> ownerChanged = this.OwnerChanged;
      if (ownerChanged == null)
        return;
      ownerChanged(this, (MyComponentContainer) this.Container);
    }

    public override bool IsSerialized() => true;

    public abstract void ConsumeItem(
      MyDefinitionId itemId,
      MyFixedPoint amount,
      long consumerEntityId = 0);

    public void RaiseContentsChanged() => this.ContentsChanged.InvokeIfNotNull<MyInventoryBase>(this);

    public void RaiseInventoryContentChanged(MyPhysicalInventoryItem item, MyFixedPoint amount)
    {
      this.InventoryContentChanged.InvokeIfNotNull<MyInventoryBase, MyPhysicalInventoryItem, MyFixedPoint>(this, item, amount);
      if (this.Entity == null || item.Content == null)
        return;
      InventoryChangedEvent inventoryChanged = MyVisualScriptLogicProvider.InventoryChanged;
      if (inventoryChanged == null)
        return;
      inventoryChanged(this.Entity.Name, item.Content.TypeId.ToString(), item.Content.SubtypeName, (float) amount);
    }

    public void RaiseBeforeContentsChanged() => this.BeforeContentsChanged.InvokeIfNotNull<MyInventoryBase>(this);

    public void RaiseContentsAdded(MyPhysicalInventoryItem item, MyFixedPoint amount) => this.ContentsAdded.InvokeIfNotNull<MyPhysicalInventoryItem, MyFixedPoint>(item, amount);

    public void RaiseContentsRemoved(MyPhysicalInventoryItem item, MyFixedPoint amount) => this.ContentsRemoved.InvokeIfNotNull<MyPhysicalInventoryItem, MyFixedPoint>(item, amount);
  }
}
