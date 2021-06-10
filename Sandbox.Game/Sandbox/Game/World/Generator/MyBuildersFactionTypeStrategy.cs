// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyBuildersFactionTypeStrategy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.World.Generator
{
  internal class MyBuildersFactionTypeStrategy : MyFactionTypeBaseStrategy
  {
    private static MyDefinitionId BUILDER_ID = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_FactionTypeDefinition), "Builder");

    public MyBuildersFactionTypeStrategy()
      : base(MyBuildersFactionTypeStrategy.BUILDER_ID)
    {
      MyFactionTypeDefinition definition = this.m_definition;
      int num1;
      if (definition == null)
      {
        num1 = 0;
      }
      else
      {
        int? length = definition.GridsForSale?.Length;
        int num2 = 0;
        num1 = length.GetValueOrDefault() > num2 & length.HasValue ? 1 : 0;
      }
      if (num1 == 0)
        return;
      this.m_priceCalculator.CalculatePrefabInformation(this.m_definition.GridsForSale, this.m_definition.BaseCostProductionSpeedMultiplier);
    }

    public override void UpdateStationsStoreItems(MyFaction faction, bool firstGeneration) => base.UpdateStationsStoreItems(faction, firstGeneration);

    protected override void UpdateStationsStoreItems(
      MyStation station,
      int existingOffers,
      int existingOrders)
    {
      base.UpdateStationsStoreItems(station, existingOffers, existingOrders);
      this.GenerateGridOffers(station, existingOffers);
    }

    private void GenerateGridOffers(MyStation station, int existingOffers)
    {
      int num1 = 0;
      foreach (MyStoreItem storeItem in station.StoreItems)
      {
        if (storeItem.StoreItemType == StoreItemTypes.Offer && storeItem.ItemType == ItemTypes.Grid)
          ++num1;
      }
      int num2 = this.m_definition.GridsForSale != null ? this.m_definition.GridsForSale.Length : 0;
      int num3 = num2 / 2;
      if (num2 == 0 || num3 <= num1)
        return;
      int num4 = num3 - num1;
      List<string> stringList = new List<string>((IEnumerable<string>) this.m_definition.GridsForSale);
      foreach (MyStoreItem storeItem in station.StoreItems)
      {
        if (!string.IsNullOrEmpty(storeItem.PrefabName) && storeItem.ItemType == ItemTypes.Grid)
          stringList.Remove(storeItem.PrefabName);
      }
      for (int index1 = 0; index1 < num4 && stringList.Count != 0; ++index1)
      {
        int index2 = MyRandom.Instance.Next(0, stringList.Count);
        string prefabName = stringList[index2];
        int minimalPrice = 0;
        if (this.m_priceCalculator.TryGetPrefabMinimalPrice(prefabName, out minimalPrice))
        {
          if (minimalPrice <= 0)
          {
            MyLog.Default.WriteToLogAndAssert("Wrong price for the prefab - " + prefabName);
          }
          else
          {
            int totalPcu = 0;
            this.m_priceCalculator.TryGetPrefabTotalPcu(prefabName, out totalPcu);
            MyEnvironmentTypes environmentType;
            this.m_priceCalculator.TryGetPrefabEnvironmentType(prefabName, out environmentType);
            if (environmentType != MyEnvironmentTypes.None)
            {
              switch (station.Type)
              {
                case MyStationTypeEnum.MiningStation:
                case MyStationTypeEnum.OrbitalStation:
                case MyStationTypeEnum.SpaceStation:
                  if ((environmentType & MyEnvironmentTypes.Space) != MyEnvironmentTypes.Space)
                  {
                    stringList.Remove(prefabName);
                    continue;
                  }
                  break;
                case MyStationTypeEnum.Outpost:
                  if (station.IsOnPlanetWithAtmosphere)
                  {
                    if ((environmentType & MyEnvironmentTypes.PlanetWithAtmosphere) != MyEnvironmentTypes.PlanetWithAtmosphere)
                    {
                      stringList.Remove(prefabName);
                      continue;
                    }
                    break;
                  }
                  if ((environmentType & MyEnvironmentTypes.PlanetWithoutAtmosphere) != MyEnvironmentTypes.PlanetWithoutAtmosphere)
                  {
                    stringList.Remove(prefabName);
                    continue;
                  }
                  break;
              }
            }
            MyStoreItem myStoreItem = new MyStoreItem(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STORE_ITEM), prefabName, 1, minimalPrice, totalPcu, StoreItemTypes.Offer);
            station.StoreItems.Add(myStoreItem);
            stringList.Remove(prefabName);
          }
        }
      }
    }
  }
}
