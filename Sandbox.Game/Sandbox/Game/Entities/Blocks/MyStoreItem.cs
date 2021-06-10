// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyStoreItem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
  [Serializable]
  public class MyStoreItem
  {
    private int m_amount;
    private int m_removedAmount;
    private byte m_updateCount;

    public long Id { get; set; }

    [Nullable]
    public SerializableDefinitionId? Item { get; set; }

    public ItemTypes ItemType { get; set; }

    public int Amount
    {
      get => this.m_amount;
      set
      {
        if (this.m_amount > value)
          this.m_removedAmount += this.m_amount - value;
        this.m_amount = value;
        this.IsActive = this.m_amount > 0;
      }
    }

    public int RemovedAmount => this.m_removedAmount;

    public byte UpdateCount => this.m_updateCount;

    public int PricePerUnit { get; set; }

    public StoreItemTypes StoreItemType { get; set; }

    public bool IsActive { get; set; } = true;

    [Nullable]
    public string PrefabName { get; set; }

    public int PrefabTotalPcu { get; set; }

    public float PricePerUnitDiscount { get; set; }

    [XmlIgnore]
    public Action<int, int, long, long, long> OnTransaction { get; private set; }

    [XmlIgnore]
    public Action OnCancel { get; private set; }

    public MyStoreItem()
    {
    }

    public MyStoreItem(
      long id,
      MyDefinitionId itemId,
      int amount,
      int pricePerUnit,
      StoreItemTypes storeItemType,
      byte updateCountStart = 0)
    {
      this.Id = id;
      this.Item = new SerializableDefinitionId?((SerializableDefinitionId) itemId);
      this.Amount = amount;
      this.PricePerUnit = pricePerUnit;
      this.StoreItemType = storeItemType;
      this.ItemType = ItemTypes.PhysicalItem;
      this.m_updateCount = updateCountStart;
    }

    public MyStoreItem(
      long id,
      int amount,
      int pricePerUnit,
      StoreItemTypes storeItemType,
      ItemTypes itemType)
    {
      this.Id = id;
      this.Amount = amount;
      this.PricePerUnit = pricePerUnit;
      this.StoreItemType = storeItemType;
      this.ItemType = itemType;
    }

    public MyStoreItem(
      long id,
      string prefabName,
      int amount,
      int pricePerUnit,
      int totalPcu,
      StoreItemTypes storeItemType)
    {
      this.Id = id;
      this.Amount = amount;
      this.PricePerUnit = pricePerUnit;
      this.StoreItemType = storeItemType;
      this.ItemType = ItemTypes.Grid;
      this.PrefabName = prefabName;
      this.PrefabTotalPcu = totalPcu;
    }

    public MyStoreItem(MyObjectBuilder_StoreItem builder)
    {
      this.Id = builder.Id;
      this.Item = builder.Item;
      this.Amount = builder.Amount;
      this.PricePerUnit = builder.PricePerUnit;
      this.StoreItemType = builder.StoreItemType;
      this.m_removedAmount = builder.RemovedAmount;
      this.m_updateCount = builder.UpdateCount;
      this.ItemType = builder.ItemType;
      this.PrefabName = builder.PrefabName;
      this.PrefabTotalPcu = builder.PrefabTotalPcu;
      this.PricePerUnitDiscount = builder.PricePerUnitDiscount;
    }

    public MyObjectBuilder_StoreItem GetObjectBuilder() => new MyObjectBuilder_StoreItem()
    {
      Id = this.Id,
      Item = this.Item,
      Amount = this.Amount,
      PricePerUnit = this.PricePerUnit,
      StoreItemType = this.StoreItemType,
      RemovedAmount = this.m_removedAmount,
      UpdateCount = this.m_updateCount,
      ItemType = this.ItemType,
      PrefabName = this.PrefabName,
      PrefabTotalPcu = this.PrefabTotalPcu,
      PricePerUnitDiscount = this.PricePerUnitDiscount
    };

    private void ResetRemovedAmount() => this.m_removedAmount = 0;

    private void IncrementUpdateCount() => ++this.m_updateCount;

    private void ResetUpdateCount() => this.m_updateCount = (byte) 0;

    internal void UpdateOfferItem(
      MyFactionTypeDefinition definition,
      int minimalPricePerUnit,
      int minimumOfferAmount,
      int maximumOfferAmount)
    {
      if (this.IsActive)
      {
        if (this.RemovedAmount == 0 && (int) this.UpdateCount >= (int) definition.OfferMaxUpdateCount)
        {
          this.Amount = 0;
          this.ResetRemovedAmount();
        }
        else
        {
          float num1 = (float) this.RemovedAmount / (float) (this.Amount + this.RemovedAmount);
          float num2;
          if ((double) num1 > (double) definition.OfferPriceUpDownPoint)
          {
            float amount = (float) (((double) num1 - (double) definition.OfferPriceUpDownPoint) / (1.0 - (double) definition.OfferPriceUpDownPoint));
            num2 = MathHelper.Lerp(definition.OfferPriceUpMultiplierMin, definition.OfferPriceUpMultiplierMax, amount);
          }
          else
          {
            float amount = (definition.OfferPriceUpDownPoint - num1) / definition.OfferPriceUpDownPoint;
            num2 = MathHelper.Lerp(definition.OfferPriceDownMultiplierMin, definition.OfferPriceDownMultiplierMax, amount);
          }
          this.PricePerUnit = (int) Math.Max((float) this.PricePerUnit * num2, (float) minimalPricePerUnit * definition.OfferPriceBellowMinimumMultiplier);
          this.PricePerUnitDiscount = minimalPricePerUnit > this.PricePerUnit ? (float) (1.0 - (double) this.PricePerUnit / (double) minimalPricePerUnit) : 0.0f;
          this.ResetRemovedAmount();
          this.IncrementUpdateCount();
        }
      }
      else
      {
        this.PricePerUnit = (int) ((double) this.PricePerUnit * (double) definition.OfferPriceUpMultiplierMax);
        this.Amount = MyRandom.Instance.Next(minimumOfferAmount, maximumOfferAmount);
        this.ResetRemovedAmount();
        this.ResetUpdateCount();
      }
    }

    internal void UpdateOrderItem(
      MyFactionTypeDefinition definition,
      int minimalPricePerUnit,
      int minimumOrderAmount,
      int maximumOrderAmount,
      long availableBalance)
    {
      if (!this.IsActive)
        return;
      if (this.RemovedAmount == 0 && (int) this.UpdateCount >= (int) definition.OrderMaxUpdateCount)
      {
        this.Amount = 0;
        this.ResetRemovedAmount();
      }
      else
      {
        float num1 = (float) this.RemovedAmount / (float) (this.Amount + this.RemovedAmount);
        float num2;
        if ((double) num1 > (double) definition.OrderPriceUpDownPoint)
        {
          float amount = (float) (((double) num1 - (double) definition.OrderPriceUpDownPoint) / (1.0 - (double) definition.OrderPriceUpDownPoint));
          num2 = MathHelper.Lerp(definition.OrderPriceDownMultiplierMax, definition.OrderPriceDownMultiplierMin, amount);
        }
        else
        {
          float amount = (definition.OrderPriceUpDownPoint - num1) / definition.OrderPriceUpDownPoint;
          num2 = MathHelper.Lerp(definition.OrderPriceUpMultiplierMin, definition.OrderPriceUpMultiplierMax, amount);
        }
        this.PricePerUnit = (int) Math.Min((float) this.PricePerUnit * num2, (float) minimalPricePerUnit * definition.OrderPriceOverMinimumMultiplier);
        this.ResetRemovedAmount();
        this.IncrementUpdateCount();
      }
    }

    internal void SetActions(Action<int, int, long, long, long> onTransaction, Action onCancel)
    {
      this.OnTransaction += onTransaction;
      this.OnCancel += onCancel;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003Em_amount\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in int value) => owner.m_amount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out int value) => value = owner.m_amount;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003Em_removedAmount\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in int value) => owner.m_removedAmount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out int value) => value = owner.m_removedAmount;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003Em_updateCount\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, byte>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in byte value) => owner.m_updateCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out byte value) => value = owner.m_updateCount;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in long value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out long value) => value = owner.Id;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003EItem\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in SerializableDefinitionId? value) => owner.Item = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out SerializableDefinitionId? value) => value = owner.Item;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003EItemType\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, ItemTypes>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in ItemTypes value) => owner.ItemType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out ItemTypes value) => value = owner.ItemType;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003EAmount\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in int value) => owner.Amount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out int value) => value = owner.Amount;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003EPricePerUnit\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in int value) => owner.PricePerUnit = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out int value) => value = owner.PricePerUnit;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003EStoreItemType\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, StoreItemTypes>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in StoreItemTypes value) => owner.StoreItemType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out StoreItemTypes value) => value = owner.StoreItemType;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003EIsActive\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in bool value) => owner.IsActive = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out bool value) => value = owner.IsActive;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003EPrefabName\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in string value) => owner.PrefabName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out string value) => value = owner.PrefabName;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003EPrefabTotalPcu\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in int value) => owner.PrefabTotalPcu = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out int value) => value = owner.PrefabTotalPcu;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003EPricePerUnitDiscount\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in float value) => owner.PricePerUnitDiscount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out float value) => value = owner.PricePerUnitDiscount;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003EOnTransaction\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, Action<int, int, long, long, long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in Action<int, int, long, long, long> value) => owner.OnTransaction = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out Action<int, int, long, long, long> value) => value = owner.OnTransaction;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreItem\u003C\u003EOnCancel\u003C\u003EAccessor : IMemberAccessor<MyStoreItem, Action>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreItem owner, in Action value) => owner.OnCancel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreItem owner, out Action value) => value = owner.OnCancel;
    }
  }
}
