// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyContractTypeSearchStrategy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public class MyContractTypeSearchStrategy : MyContractTypeBaseStrategy
  {
    private static readonly float GRAVITY_SQUARED_EPSILON = 0.0001f;

    public MyContractTypeSearchStrategy(
      MySessionComponentEconomyDefinition economyDefinition)
      : base(economyDefinition)
    {
    }

    public override bool CanBeGenerated(MyStation station, MyFaction faction) => true;

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
      MyContractFind myContractFind = new MyContractFind();
      if (!(myContractFind.GetDefinition() is MyContractTypeFindDefinition definition))
        return MyContractCreationResults.Error;
      MyStation myStation = factions.TryGetFactionById(factionId) is MyFaction factionById ? factionById.GetStationById(stationId) : (MyStation) null;
      if (myStation == null)
        return MyContractCreationResults.Error;
      Vector3D center = Vector3D.Zero;
      Vector3D vector3D = Vector3D.Zero;
      if (myStation.Type == MyStationTypeEnum.Outpost)
      {
        BoundingSphereD boundingSphereD = new BoundingSphereD(myStation.Position, definition.MaxGridDistance);
        bool flag = false;
        int num3 = 0;
        do
        {
          Vector3D? sphereWithInnerCutout = boundingSphereD.RandomToUniformPointInSphereWithInnerCutout(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), definition.MinGridDistance);
          if (sphereWithInnerCutout.HasValue)
          {
            MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(sphereWithInnerCutout.Value);
            if (closestPlanet != null)
            {
              float num4 = 10f;
              Vector3D surfacePointGlobal = closestPlanet.GetClosestSurfacePointGlobal(sphereWithInnerCutout.Value);
              center = surfacePointGlobal + Vector3.Normalize(surfacePointGlobal - closestPlanet.PositionComp.GetPosition()) * num4;
              boundingSphereD = new BoundingSphereD(center, definition.MaxGpsOffset);
              Vector3D? nullable = new Vector3D?(boundingSphereD.RandomToUniformPointInSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble()));
              if (!nullable.HasValue)
                return MyContractCreationResults.Fail_Common;
              vector3D = closestPlanet.GetClosestSurfacePointGlobal(nullable.Value);
              flag = true;
              break;
            }
          }
          ++num3;
        }
        while (num3 <= num1);
        if (!flag)
          return MyContractCreationResults.Fail_Common;
      }
      else
      {
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
              if ((double) MyGravityProviderSystem.CalculateNaturalGravityInPoint(sphereWithInnerCutout.Value).LengthSquared() <= (double) MyContractTypeSearchStrategy.GRAVITY_SQUARED_EPSILON)
              {
                flag = true;
                center = sphereWithInnerCutout.Value;
                break;
              }
              ++num3;
            }
          }
        }
        while (num3 <= num1);
        if (!flag)
          return MyContractCreationResults.Fail_Common;
        boundingSphereD = new BoundingSphereD(center, definition.MaxGpsOffset);
        Vector3D? nullable = new Vector3D?(boundingSphereD.RandomToUniformPointInSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble()));
        if (!nullable.HasValue)
          return MyContractCreationResults.Fail_Common;
        vector3D = nullable.Value;
      }
      double maxGpsOffset = definition.MaxGpsOffset;
      double num5 = (center - myStation.Position).Length();
      double num6 = (vector3D - myStation.Position).Length();
      MyObjectBuilder_ContractFind builderContractFind = new MyObjectBuilder_ContractFind();
      builderContractFind.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      builderContractFind.IsPlayerMade = false;
      builderContractFind.State = MyContractStateEnum.Inactive;
      builderContractFind.RewardMoney = this.GetMoneyRewardForSearchContract(definition.MinimumMoney, num5, definition.MinGridDistance);
      if (myStation.IsDeepSpaceStation)
        builderContractFind.RewardMoney = (long) ((double) builderContractFind.RewardMoney * (double) this.m_economyDefinition.DeepSpaceStationContractBonus);
      builderContractFind.RewardReputation = this.GetReputationRewardForSearchContract(definition.MinimumReputation, num5, maxGpsOffset);
      builderContractFind.StartingDeposit = (long) (MyRandom.Instance.NextDouble() * (double) (definition.MaxStartingDeposit - definition.MinStartingDeposit));
      builderContractFind.FailReputationPrice = definition.FailReputationPrice;
      builderContractFind.StartFaction = factionId;
      builderContractFind.StartStation = stationId;
      builderContractFind.StartBlock = 0L;
      builderContractFind.GridPosition = (SerializableVector3D) center;
      builderContractFind.GpsPosition = (SerializableVector3D) vector3D;
      builderContractFind.GpsDistance = num6;
      builderContractFind.MaxGpsOffset = (float) definition.MaxGpsOffset;
      builderContractFind.TriggerRadius = definition.TriggerRadius;
      builderContractFind.GridId = 0L;
      builderContractFind.KeepGridAtTheEnd = false;
      builderContractFind.Creation = now.Ticks;
      builderContractFind.RemainingTimeInS = new double?(this.GetDurationForSearchContract(definition, num5, maxGpsOffset).Seconds);
      builderContractFind.TicksToDiscard = new int?(MyContractTypeBaseStrategy.TICKS_TO_LIVE);
      myContractFind.Init((MyObjectBuilder_Contract) builderContractFind);
      contract = (MyContract) myContractFind;
      return MyContractCreationResults.Success;
    }

    private long GetMoneyRewardForSearchContract(long baseRew, double distance, double minDistance) => (long) ((double) baseRew + 1000.0 * Math.Pow(distance / minDistance, 2.2));

    private int GetReputationRewardForSearchContract(
      int baseRew,
      double distance,
      double searchAreaRadius)
    {
      return baseRew;
    }

    private MyTimeSpan GetDurationForSearchContract(
      MyContractTypeFindDefinition def,
      double distanceInM,
      double searchAreaRadius,
      bool cutOutJumps = true)
    {
      double num = searchAreaRadius / 1000.0;
      return MyTimeSpan.FromSeconds(def.DurationMultiplier * (def.Duration_BaseTime + (distanceInM * def.Duration_TimePerMeter + 3.14159297943115 * num * num * num * def.Duration_TimePerCubicKm)));
    }
  }
}
