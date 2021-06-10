// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyFactionTypeDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_FactionTypeDefinition), null)]
  public class MyFactionTypeDefinition : MyDefinitionBase
  {
    public SerializableDefinitionId[] OffersList { get; set; }

    public SerializableDefinitionId[] OrdersList { get; set; }

    public float OfferPriceUpMultiplierMax { get; set; }

    public float OfferPriceUpMultiplierMin { get; set; }

    public float OfferPriceDownMultiplierMax { get; set; }

    public float OfferPriceDownMultiplierMin { get; set; }

    public float OfferPriceUpDownPoint { get; set; }

    public float OfferPriceBellowMinimumMultiplier { get; set; }

    public float OfferPriceStartingMultiplier { get; set; }

    public byte OfferMaxUpdateCount { get; set; }

    public float OrderPriceStartingMultiplier { get; set; }

    public float OrderPriceUpMultiplierMax { get; set; }

    public float OrderPriceUpMultiplierMin { get; set; }

    public float OrderPriceDownMultiplierMax { get; set; }

    public float OrderPriceDownMultiplierMin { get; set; }

    public float OrderPriceOverMinimumMultiplier { get; set; }

    public float OrderPriceUpDownPoint { get; set; }

    public byte OrderMaxUpdateCount { get; set; }

    public bool CanSellOxygen { get; set; }

    public bool CanSellHydrogen { get; set; }

    public int MinimumOfferGasAmount { get; set; }

    public int MaximumOfferGasAmount { get; set; }

    public float BaseCostProductionSpeedMultiplier { get; set; }

    public int MinimumOxygenPrice { get; set; }

    public int MinimumHydrogenPrice { get; set; }

    public string[] GridsForSale { get; set; }

    public int MaxContractCount { get; set; }

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_FactionTypeDefinition factionTypeDefinition = (MyObjectBuilder_FactionTypeDefinition) builder;
      if (factionTypeDefinition.OffersList != null)
      {
        this.OffersList = new SerializableDefinitionId[factionTypeDefinition.OffersList.Length];
        for (int index = 0; index < factionTypeDefinition.OffersList.Length; ++index)
          this.OffersList[index] = factionTypeDefinition.OffersList[index];
      }
      if (factionTypeDefinition.OrdersList != null)
      {
        this.OrdersList = new SerializableDefinitionId[factionTypeDefinition.OrdersList.Length];
        for (int index = 0; index < factionTypeDefinition.OrdersList.Length; ++index)
          this.OrdersList[index] = factionTypeDefinition.OrdersList[index];
      }
      if (factionTypeDefinition.GridsForSale != null)
      {
        this.GridsForSale = new string[factionTypeDefinition.GridsForSale.Length];
        for (int index = 0; index < factionTypeDefinition.GridsForSale.Length; ++index)
          this.GridsForSale[index] = factionTypeDefinition.GridsForSale[index];
      }
      this.MaxContractCount = factionTypeDefinition.MaxContractCount;
      this.OfferPriceUpMultiplierMax = factionTypeDefinition.OfferPriceUpMultiplierMax;
      this.OfferPriceUpMultiplierMin = factionTypeDefinition.OfferPriceUpMultiplierMin;
      this.OfferPriceDownMultiplierMax = factionTypeDefinition.OfferPriceDownMultiplierMax;
      this.OfferPriceDownMultiplierMin = factionTypeDefinition.OfferPriceDownMultiplierMin;
      this.OfferPriceUpDownPoint = factionTypeDefinition.OfferPriceUpDownPoint;
      this.OfferPriceBellowMinimumMultiplier = factionTypeDefinition.OfferPriceBellowMinimumMultiplier;
      this.OfferPriceStartingMultiplier = factionTypeDefinition.OfferPriceStartingMultiplier;
      this.OfferMaxUpdateCount = factionTypeDefinition.OfferMaxUpdateCount;
      this.OrderPriceStartingMultiplier = factionTypeDefinition.OrderPriceStartingMultiplier;
      this.OrderPriceUpMultiplierMax = factionTypeDefinition.OrderPriceUpMultiplierMax;
      this.OrderPriceUpMultiplierMin = factionTypeDefinition.OrderPriceUpMultiplierMin;
      this.OrderPriceDownMultiplierMax = factionTypeDefinition.OrderPriceDownMultiplierMax;
      this.OrderPriceDownMultiplierMin = factionTypeDefinition.OrderPriceDownMultiplierMin;
      this.OrderPriceOverMinimumMultiplier = factionTypeDefinition.OrderPriceOverMinimumMultiplier;
      this.OrderPriceUpDownPoint = factionTypeDefinition.OrderPriceUpDownPoint;
      this.OrderMaxUpdateCount = factionTypeDefinition.OrderMaxUpdateCount;
      this.CanSellOxygen = factionTypeDefinition.CanSellOxygen;
      this.MinimumOxygenPrice = factionTypeDefinition.MinimumOxygenPrice;
      this.CanSellHydrogen = factionTypeDefinition.CanSellHydrogen;
      this.MinimumHydrogenPrice = factionTypeDefinition.MinimumHydrogenPrice;
      this.MinimumOfferGasAmount = factionTypeDefinition.MinimumOfferGasAmount;
      this.MaximumOfferGasAmount = factionTypeDefinition.MaximumOfferGasAmount;
      this.BaseCostProductionSpeedMultiplier = factionTypeDefinition.BaseCostProductionSpeedMultiplier;
    }

    private class Sandbox_Definitions_MyFactionTypeDefinition\u003C\u003EActor : IActivator, IActivator<MyFactionTypeDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyFactionTypeDefinition();

      MyFactionTypeDefinition IActivator<MyFactionTypeDefinition>.CreateInstance() => new MyFactionTypeDefinition();
    }
  }
}
