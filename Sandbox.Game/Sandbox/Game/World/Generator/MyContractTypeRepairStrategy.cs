// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyContractTypeRepairStrategy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRage.ObjectBuilder;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public class MyContractTypeRepairStrategy : MyContractTypeBaseStrategy
  {
    private static readonly float GRAVITY_SQUARED_EPSILON = 0.0001f;

    public MyContractTypeRepairStrategy(
      MySessionComponentEconomyDefinition economyDefinition)
      : base(economyDefinition)
    {
    }

    public override bool CanBeGenerated(MyStation station, MyFaction faction) => station.Type != MyStationTypeEnum.Outpost;

    public override MyContractCreationResults GenerateContract(
      out MyContract contract,
      long factionId,
      long stationId,
      MyMinimalPriceCalculator calculator,
      MyTimeSpan now)
    {
      MyFactionCollection factions = MySession.Static.Factions;
      contract = (MyContract) null;
      int num1 = 20;
      int num2 = 50;
      MyContractRepair myContractRepair = new MyContractRepair();
      if (!(myContractRepair.GetDefinition() is MyContractTypeRepairDefinition definition))
        return MyContractCreationResults.Error;
      MyStation myStation = factions.TryGetFactionById(factionId) is MyFaction factionById ? factionById.GetStationById(stationId) : (MyStation) null;
      if (myStation == null)
        return MyContractCreationResults.Error;
      if (myStation.Type == MyStationTypeEnum.Outpost)
        return MyContractCreationResults.Fail_Impossible;
      Vector3D zero = Vector3D.Zero;
      BoundingSphereD boundingSphereD = new BoundingSphereD(myStation.Position, definition.MaxGridDistance);
      bool flag = false;
      int num3 = 0;
      do
      {
        Vector3D? sphereWithInnerCutout = boundingSphereD.RandomToUniformPointInSphereWithInnerCutout(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), definition.MinGridDistance);
        if (sphereWithInnerCutout.HasValue)
        {
          List<MyObjectSeed> list = new List<MyObjectSeed>();
          MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(sphereWithInnerCutout.Value, (double) num2), list);
          if (list.Count <= 0)
          {
            if ((double) MyGravityProviderSystem.CalculateNaturalGravityInPoint(sphereWithInnerCutout.Value).LengthSquared() <= (double) MyContractTypeRepairStrategy.GRAVITY_SQUARED_EPSILON)
            {
              flag = true;
              zero = sphereWithInnerCutout.Value;
              break;
            }
            ++num3;
          }
        }
      }
      while (num3 <= num1);
      if (!flag)
        return MyContractCreationResults.Fail_Common;
      double gridDistance = (zero - myStation.Position).Length();
      string prefabName = "";
      int gridPrice = 0;
      if (definition.PrefabNames.Count > 0)
        prefabName = definition.PrefabNames[MyRandom.Instance.Next(0, definition.PrefabNames.Count)];
      if (!this.GetRepairDataFromPrefab(prefabName, calculator, out gridPrice))
      {
        calculator.CalculatePrefabInformation(new string[1]
        {
          prefabName
        });
        if (!this.GetRepairDataFromPrefab(prefabName, calculator, out gridPrice))
          return MyContractCreationResults.Fail_Common;
      }
      int repairComponentTimeInS = (double) definition.TimeToPriceDenominator != 0.0 ? (int) ((double) gridPrice / (double) definition.TimeToPriceDenominator) : 0;
      int num4 = (int) ((1.0 + (double) (MyRandom.Instance.NextFloat(-1f, 1f) * definition.PriceSpread)) * (double) gridPrice);
      MyObjectBuilder_ContractRepair builderContractRepair = new MyObjectBuilder_ContractRepair();
      builderContractRepair.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      builderContractRepair.IsPlayerMade = false;
      builderContractRepair.State = MyContractStateEnum.Inactive;
      builderContractRepair.RewardMoney = this.GetMoneyRewardForRepairContract(definition.MinimumMoney, gridDistance, (long) num4, definition.PriceToRewardCoeficient);
      if (myStation.IsDeepSpaceStation)
        builderContractRepair.RewardMoney = (long) ((double) builderContractRepair.RewardMoney * (double) this.m_economyDefinition.DeepSpaceStationContractBonus);
      builderContractRepair.RewardReputation = this.GetReputationRewardForRepairContract(definition.MinimumReputation);
      builderContractRepair.StartingDeposit = (long) (MyRandom.Instance.NextDouble() * (double) (definition.MaxStartingDeposit - definition.MinStartingDeposit));
      builderContractRepair.FailReputationPrice = definition.FailReputationPrice;
      builderContractRepair.StartFaction = factionId;
      builderContractRepair.StartStation = stationId;
      builderContractRepair.StartBlock = 0L;
      builderContractRepair.GridPosition = (SerializableVector3D) zero;
      builderContractRepair.GridId = 0L;
      builderContractRepair.PrefabName = prefabName;
      builderContractRepair.BlocksToRepair = new MySerializableList<Vector3I>();
      builderContractRepair.KeepGridAtTheEnd = false;
      builderContractRepair.UnrepairedBlockCount = 0;
      builderContractRepair.Creation = now.Ticks;
      builderContractRepair.RemainingTimeInS = new double?(this.GetDurationForRepairContract(definition, gridDistance, repairComponentTimeInS).Seconds);
      builderContractRepair.TicksToDiscard = new int?(MyContractTypeBaseStrategy.TICKS_TO_LIVE);
      myContractRepair.Init((MyObjectBuilder_Contract) builderContractRepair);
      contract = (MyContract) myContractRepair;
      return MyContractCreationResults.Success;
    }

    private bool GetRepairDataFromPrefab(
      string prefabName,
      MyMinimalPriceCalculator calculator,
      out int gridPrice)
    {
      gridPrice = 0;
      return !string.IsNullOrEmpty(prefabName) && calculator.TryGetPrefabMinimalRepairPrice(prefabName, out gridPrice);
    }

    private long GetMoneyRewardForRepairContract(
      long baseRew,
      double gridDistance,
      long gridPrice,
      float gridPriceToRewardcoef)
    {
      return (long) ((double) baseRew * Math.Pow(2.0, Math.Log10(gridDistance))) + (long) ((double) gridPriceToRewardcoef * (double) gridPrice);
    }

    private int GetReputationRewardForRepairContract(int baseRew) => baseRew;

    private MyTimeSpan GetDurationForRepairContract(
      MyContractTypeRepairDefinition def,
      double gridDistance,
      int repairComponentTimeInS)
    {
      return MyTimeSpan.FromSeconds(def.DurationMultiplier * (def.Duration_BaseTime + gridDistance * def.Duration_TimePerMeter + (double) repairComponentTimeInS));
    }
  }
}
