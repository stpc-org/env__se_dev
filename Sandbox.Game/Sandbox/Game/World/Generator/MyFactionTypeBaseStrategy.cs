// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyFactionTypeBaseStrategy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.SessionComponents;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.World.Generator
{
  internal class MyFactionTypeBaseStrategy : IMyStoreItemsGeneratorFactionTypeStrategy
  {
    private static readonly MyDefinitionId m_datapadId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_DatapadDefinition), "Datapad");
    protected MyMinimalPriceCalculator m_priceCalculator = new MyMinimalPriceCalculator();
    protected MyFactionTypeDefinition m_definition;

    public MyFactionTypeBaseStrategy(MyDefinitionId factionTypeId)
    {
      this.m_definition = MyDefinitionManager.Static.GetDefinition<MyFactionTypeDefinition>(factionTypeId);
      if (this.m_definition == null)
        return;
      this.m_priceCalculator.CalculateMinimalPrices(this.m_definition.OffersList, this.m_definition.BaseCostProductionSpeedMultiplier);
      this.m_priceCalculator.CalculateMinimalPrices(this.m_definition.OrdersList, this.m_definition.BaseCostProductionSpeedMultiplier);
    }

    public virtual void UpdateStationsStoreItems(MyFaction faction, bool firstGeneration)
    {
      if (this.m_definition == null)
        return;
      DictionaryValuesReader<long, MyStation> stations = faction.Stations;
      MyAccountInfo account;
      if (stations.Count == 0 || !MyBankingSystem.Static.TryGetAccountInfo(faction.FactionId, out account))
        return;
      long balance = account.Balance;
      stations = faction.Stations;
      long count = (long) stations.Count;
      long availableBalance = balance / count;
      stations = faction.Stations;
      foreach (MyStation station in stations)
      {
        int existingOffers = 0;
        int existingOrders = 0;
        bool hasOxygenOffer = false;
        bool hasHydrogenOffer = false;
        List<MyStoreItem> myStoreItemList = new List<MyStoreItem>();
        foreach (MyStoreItem storeItem in station.StoreItems)
        {
          int minimalPrice = 0;
          int minimumOfferAmount = 0;
          int maximumOfferAmount = 0;
          int minimumOrderAmount = 0;
          int maximumOrderAmount = 0;
          MyPhysicalItemDefinition definition = (MyPhysicalItemDefinition) null;
          switch (storeItem.ItemType)
          {
            case ItemTypes.PhysicalItem:
              if (this.m_priceCalculator.TryGetItemMinimalPrice((MyDefinitionId) storeItem.Item.Value, out minimalPrice) && MyDefinitionManager.Static.TryGetDefinition<MyPhysicalItemDefinition>((MyDefinitionId) storeItem.Item.Value, out definition))
              {
                minimumOfferAmount = definition.MinimumOfferAmount;
                maximumOfferAmount = definition.MaximumOfferAmount;
                minimumOrderAmount = definition.MinimumOrderAmount;
                maximumOrderAmount = definition.MaximumOrderAmount;
                break;
              }
              continue;
            case ItemTypes.Oxygen:
              minimumOfferAmount = this.m_definition.MinimumOfferGasAmount;
              maximumOfferAmount = this.m_definition.MaximumOfferGasAmount;
              hasOxygenOffer = true;
              break;
            case ItemTypes.Hydrogen:
              minimumOfferAmount = this.m_definition.MinimumOfferGasAmount;
              maximumOfferAmount = this.m_definition.MaximumOfferGasAmount;
              hasHydrogenOffer = true;
              break;
            case ItemTypes.Grid:
              minimumOfferAmount = 1;
              maximumOfferAmount = 1;
              break;
          }
          switch (storeItem.StoreItemType)
          {
            case StoreItemTypes.Offer:
              storeItem.UpdateOfferItem(this.m_definition, minimalPrice, minimumOfferAmount, maximumOfferAmount);
              if (storeItem.ItemType != ItemTypes.Grid)
              {
                if (storeItem.IsActive)
                {
                  ++existingOffers;
                  continue;
                }
                myStoreItemList.Add(storeItem);
                continue;
              }
              if (!storeItem.IsActive)
              {
                myStoreItemList.Add(storeItem);
                continue;
              }
              continue;
            case StoreItemTypes.Order:
              storeItem.UpdateOrderItem(this.m_definition, minimalPrice, minimumOrderAmount, maximumOrderAmount, availableBalance);
              if (storeItem.IsActive)
              {
                ++existingOrders;
                continue;
              }
              myStoreItemList.Add(storeItem);
              continue;
            default:
              continue;
          }
        }
        foreach (MyStoreItem myStoreItem in myStoreItemList)
          station.StoreItems.Remove(myStoreItem);
        this.GenerateNewStoreItems(station, existingOffers, existingOrders, availableBalance, hasOxygenOffer, hasHydrogenOffer, firstGeneration);
        this.UpdateStationsStoreItems(station, existingOffers, existingOrders);
      }
    }

    protected virtual void UpdateStationsStoreItems(
      MyStation station,
      int existingOffers,
      int existingOrders)
    {
    }

    private void GenerateNewStoreItems(
      MyStation station,
      int existingOffers,
      int existingOrders,
      long availableBalance,
      bool hasOxygenOffer,
      bool hasHydrogenOffer,
      bool firstGeneration)
    {
      this.GenerateOffers(station, existingOffers, hasOxygenOffer, hasHydrogenOffer, firstGeneration);
      this.GenerateOrders(station, existingOrders, availableBalance, firstGeneration);
    }

    private void GenerateOffers(
      MyStation station,
      int existingOffers,
      bool hasOxygenOffer,
      bool hasHydrogenOffer,
      bool firstGeneration)
    {
      int num1 = this.m_definition.OffersList != null ? this.m_definition.OffersList.Length : 0;
      if (num1 == 0)
        return;
      float num2 = 1f;
      if (station.IsDeepSpaceStation)
      {
        MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
        if (component == null)
        {
          MyLog.Default.WriteToLogAndAssert("GenerateOffers - Economy session component not found.");
          return;
        }
        num2 = 1f - component.EconomyDefinition.DeepSpaceStationStoreBonus;
      }
      int num3 = num1 / 2;
      if (num3 > existingOffers)
      {
        int num4 = num3 - existingOffers;
        List<SerializableDefinitionId> serializableDefinitionIdList1 = new List<SerializableDefinitionId>((IEnumerable<SerializableDefinitionId>) this.m_definition.OffersList);
        foreach (MyStoreItem storeItem in station.StoreItems)
        {
          SerializableDefinitionId? nullable = storeItem.Item;
          if (nullable.HasValue)
          {
            List<SerializableDefinitionId> serializableDefinitionIdList2 = serializableDefinitionIdList1;
            nullable = storeItem.Item;
            SerializableDefinitionId serializableDefinitionId = nullable.Value;
            serializableDefinitionIdList2.Remove(serializableDefinitionId);
          }
        }
        for (int index1 = 0; index1 < num4 && serializableDefinitionIdList1.Count != 0; ++index1)
        {
          int index2 = MyRandom.Instance.Next(0, serializableDefinitionIdList1.Count);
          SerializableDefinitionId serializableDefinitionId = serializableDefinitionIdList1[index2];
          long id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM);
          MyPhysicalItemDefinition definition = (MyPhysicalItemDefinition) null;
          int minimalPrice = 0;
          if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalItemDefinition>((MyDefinitionId) serializableDefinitionId, out definition))
          {
            if (this.m_priceCalculator.TryGetItemMinimalPrice((MyDefinitionId) serializableDefinitionId, out minimalPrice))
            {
              if (minimalPrice <= 0)
              {
                MyLog.Default.WriteToLogAndAssert("Wrong price for the item - " + (object) definition.Id);
              }
              else
              {
                int amount = MyRandom.Instance.Next(definition.MinimumOfferAmount, definition.MaximumOfferAmount);
                int pricePerUnit = (int) ((double) minimalPrice * (double) this.m_definition.OfferPriceStartingMultiplier * (double) num2);
                byte updateCountStart = firstGeneration ? (byte) MyRandom.Instance.Next(0, (int) this.m_definition.OfferMaxUpdateCount) : (byte) 0;
                MyStoreItem myStoreItem = new MyStoreItem(id, (MyDefinitionId) serializableDefinitionId, amount, pricePerUnit, StoreItemTypes.Offer, updateCountStart);
                station.StoreItems.Add(myStoreItem);
                serializableDefinitionIdList1.Remove(serializableDefinitionId);
              }
            }
          }
          else
            serializableDefinitionIdList1.Remove(serializableDefinitionId);
        }
      }
      if (!hasHydrogenOffer && this.m_definition.CanSellHydrogen)
      {
        int num4 = MyRandom.Instance.Next(this.m_definition.MinimumOfferGasAmount, this.m_definition.MaximumOfferGasAmount);
        long id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM);
        int num5 = (int) ((double) this.m_definition.MinimumHydrogenPrice * (double) this.m_definition.OfferPriceStartingMultiplier * (double) num2);
        int amount = num4;
        int pricePerUnit = num5;
        MyStoreItem myStoreItem = new MyStoreItem(id, amount, pricePerUnit, StoreItemTypes.Offer, ItemTypes.Hydrogen);
        station.StoreItems.Add(myStoreItem);
      }
      if (hasOxygenOffer || !this.m_definition.CanSellOxygen)
        return;
      int num6 = MyRandom.Instance.Next(this.m_definition.MinimumOfferGasAmount, this.m_definition.MaximumOfferGasAmount);
      long id1 = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM);
      int num7 = (int) ((double) this.m_definition.MinimumOxygenPrice * (double) this.m_definition.OfferPriceStartingMultiplier * (double) num2);
      int amount1 = num6;
      int pricePerUnit1 = num7;
      MyStoreItem myStoreItem1 = new MyStoreItem(id1, amount1, pricePerUnit1, StoreItemTypes.Offer, ItemTypes.Oxygen);
      station.StoreItems.Add(myStoreItem1);
    }

    private void GenerateOrders(
      MyStation station,
      int existingOrders,
      long availableBalance,
      bool firstGeneration)
    {
      int num1 = this.m_definition.OrdersList != null ? this.m_definition.OrdersList.Length : 0;
      if (num1 == 0)
        return;
      int num2 = num1;
      long num3 = availableBalance / (long) num2;
      if (num2 <= existingOrders)
        return;
      int num4 = num2 - existingOrders;
      List<SerializableDefinitionId> serializableDefinitionIdList1 = new List<SerializableDefinitionId>((IEnumerable<SerializableDefinitionId>) this.m_definition.OrdersList);
      foreach (MyStoreItem storeItem in station.StoreItems)
      {
        SerializableDefinitionId? nullable = storeItem.Item;
        if (nullable.HasValue)
        {
          List<SerializableDefinitionId> serializableDefinitionIdList2 = serializableDefinitionIdList1;
          nullable = storeItem.Item;
          SerializableDefinitionId serializableDefinitionId = nullable.Value;
          serializableDefinitionIdList2.Remove(serializableDefinitionId);
        }
      }
      for (int index1 = 0; index1 < num4 && serializableDefinitionIdList1.Count != 0; ++index1)
      {
        int index2 = MyRandom.Instance.Next(0, serializableDefinitionIdList1.Count);
        SerializableDefinitionId serializableDefinitionId = serializableDefinitionIdList1[index2];
        MyPhysicalItemDefinition definition = (MyPhysicalItemDefinition) null;
        int minimalPrice = 0;
        if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalItemDefinition>((MyDefinitionId) serializableDefinitionId, out definition))
        {
          if (this.m_priceCalculator.TryGetItemMinimalPrice((MyDefinitionId) serializableDefinitionId, out minimalPrice))
          {
            float num5 = 1f;
            if (station.IsDeepSpaceStation)
            {
              MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
              if (component == null)
              {
                MyLog.Default.WriteToLogAndAssert("GenerateOffers - Economy session component not found.");
                break;
              }
              num5 = 1f + component.EconomyDefinition.DeepSpaceStationStoreBonus;
            }
            int num6 = (int) ((double) minimalPrice * (double) this.m_definition.OrderPriceStartingMultiplier * (double) num5);
            int maxValue = 0;
            if (num6 > 0)
              maxValue = Math.Min((int) (num3 / (long) num6), definition.MaximumOrderAmount);
            if (definition.MinimumOrderAmount > maxValue)
            {
              serializableDefinitionIdList1.Remove(serializableDefinitionId);
            }
            else
            {
              int num7 = MyRandom.Instance.Next(definition.MinimumOrderAmount, maxValue);
              long id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM);
              byte num8 = firstGeneration ? (byte) MyRandom.Instance.Next(0, (int) this.m_definition.OrderMaxUpdateCount) : (byte) 0;
              MyDefinitionId itemId = (MyDefinitionId) serializableDefinitionId;
              int amount = num7;
              int pricePerUnit = num6;
              int num9 = (int) num8;
              MyStoreItem myStoreItem = new MyStoreItem(id, itemId, amount, pricePerUnit, StoreItemTypes.Order, (byte) num9);
              station.StoreItems.Add(myStoreItem);
              serializableDefinitionIdList1.Remove(serializableDefinitionId);
            }
          }
        }
        else
          serializableDefinitionIdList1.Remove(serializableDefinitionId);
      }
    }
  }
}
