// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyContractTypeEscortStrategy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public class MyContractTypeEscortStrategy : MyContractTypeBaseStrategy
  {
    private static readonly int TOO_MANY_TRIES = 10;
    private static readonly float GRAVITY_SQUARED_EPSILON = 0.0001f;
    private static readonly float MAX_ESCORT_SHIP_RADIUS = 75f;
    private static readonly int ESCORT_GRID_START_OFFSET = 550;

    public MyContractTypeEscortStrategy(
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
      MyContractEscort myContractEscort = new MyContractEscort();
      if (!(myContractEscort.GetDefinition() is MyContractTypeEscortDefinition definition))
        return MyContractCreationResults.Error;
      MyStation myStation = factions.TryGetFactionById(factionId) is MyFaction factionById ? factionById.GetStationById(stationId) : (MyStation) null;
      if (myStation == null)
        return MyContractCreationResults.Error;
      if (myStation.Type == MyStationTypeEnum.Outpost)
        return MyContractCreationResults.Fail_Impossible;
      List<MyPlanet> myPlanetList = new List<MyPlanet>();
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (entity is MyPlanet myPlanet)
          myPlanetList.Add(myPlanet);
      }
      BoundingSphereD boundingSphereD = new BoundingSphereD(myStation.Position, definition.TravelDistanceMax);
      Vector3D vector3D = Vector3D.Zero;
      Vector3D? nullable = new Vector3D?();
      bool flag1 = false;
      int num1 = -1;
      Vector3D? sphereWithInnerCutout;
      do
      {
        ++num1;
        sphereWithInnerCutout = boundingSphereD.RandomToUniformPointInSphereWithInnerCutout(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), definition.TravelDistanceMin);
        if (sphereWithInnerCutout.HasValue)
        {
          vector3D = myStation.Position + Vector3.Normalize(sphereWithInnerCutout.Value - myStation.Position) * (float) MyContractTypeEscortStrategy.ESCORT_GRID_START_OFFSET;
          if ((double) MyGravityProviderSystem.CalculateNaturalGravityInPoint(sphereWithInnerCutout.Value).LengthSquared() <= (double) MyContractTypeEscortStrategy.GRAVITY_SQUARED_EPSILON && (double) MyGravityProviderSystem.CalculateNaturalGravityInPoint(vector3D).LengthSquared() <= (double) MyContractTypeEscortStrategy.GRAVITY_SQUARED_EPSILON)
          {
            List<MyObjectSeed> list = new List<MyObjectSeed>();
            MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(sphereWithInnerCutout.Value, (double) MyContractTypeEscortStrategy.MAX_ESCORT_SHIP_RADIUS), list);
            if (list.Count <= 0)
            {
              list.Clear();
              MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(vector3D, (double) MyContractTypeEscortStrategy.MAX_ESCORT_SHIP_RADIUS), list);
              if (list.Count <= 0)
              {
                bool flag2 = true;
                foreach (MyPlanet myPlanet in myPlanetList)
                {
                  if (myPlanet.Components.Get<MyGravityProviderComponent>() is MySphericalNaturalGravityComponent gravityComponent)
                  {
                    Vector3D vector2 = myPlanet.PositionComp.GetPosition() - vector3D;
                    Vector3D vector1 = Vector3D.Normalize(sphereWithInnerCutout.Value - vector3D);
                    if ((vector2 - vector1 * Vector3D.Dot(vector1, vector2)).LengthSquared() <= (double) gravityComponent.GravityLimitSq)
                    {
                      flag2 = false;
                      break;
                    }
                  }
                }
                if (flag2)
                {
                  flag1 = true;
                  break;
                }
              }
            }
          }
        }
      }
      while (num1 < MyContractTypeEscortStrategy.TOO_MANY_TRIES);
      if (!flag1)
        return MyContractCreationResults.Fail_Common;
      double num2 = (vector3D - sphereWithInnerCutout.Value).Length();
      long factionId1 = 0;
      IMyFaction factionById1 = factions.TryGetFactionById(myStation.FactionId);
      if (factionById1 != null)
        factionId1 = factionById1.FounderId;
      long factionId2 = this.GetPirateFactionId();
      if (factionId2 == 0L)
        return MyContractCreationResults.Error;
      if (factionById1.FactionId == factionId2 || MySession.Static.Factions.GetRelationBetweenFactions(factionId1, factionId2).Item1 != MyRelationsBetweenFactions.Enemies)
      {
        MyFaction randomEnemyFaction = this.FindRandomEnemyFaction(factionById1.FactionId);
        if (randomEnemyFaction != null)
          factionId2 = randomEnemyFaction.FactionId;
      }
      MyObjectBuilder_ContractEscort builderContractEscort = new MyObjectBuilder_ContractEscort();
      builderContractEscort.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
      builderContractEscort.IsPlayerMade = false;
      builderContractEscort.State = MyContractStateEnum.Inactive;
      builderContractEscort.RewardMoney = this.GetMoneyReward_Escort(definition.MinimumMoney, num2);
      if (myStation.IsDeepSpaceStation)
        builderContractEscort.RewardMoney = (long) ((double) builderContractEscort.RewardMoney * (double) this.m_economyDefinition.DeepSpaceStationContractBonus);
      builderContractEscort.RewardReputation = this.GetReputationReward_Escort(definition.MinimumReputation, num2);
      builderContractEscort.StartingDeposit = (long) (MyRandom.Instance.NextDouble() * (double) (definition.MaxStartingDeposit - definition.MinStartingDeposit));
      builderContractEscort.FailReputationPrice = definition.FailReputationPrice;
      builderContractEscort.StartFaction = factionId;
      builderContractEscort.StartStation = stationId;
      builderContractEscort.StartBlock = 0L;
      builderContractEscort.GridId = 0L;
      builderContractEscort.StartPosition = (SerializableVector3D) vector3D;
      builderContractEscort.EndPosition = (SerializableVector3D) sphereWithInnerCutout.Value;
      builderContractEscort.PathLength = num2;
      builderContractEscort.RewardRadius = definition.RewardRadius;
      builderContractEscort.TriggerEntityId = 0L;
      builderContractEscort.TriggerRadius = definition.TriggerRadius;
      builderContractEscort.DroneFirstDelay = MyTimeSpan.FromSeconds(factionId2 == 0L ? (double) int.MaxValue : (double) definition.DroneFirstDelayInS).Ticks;
      builderContractEscort.DroneAttackPeriod = MyTimeSpan.FromSeconds(factionId2 == 0L ? (double) int.MaxValue : (double) definition.DroneAttackPeriodInS).Ticks;
      builderContractEscort.DronesPerWave = factionId2 == 0L ? 0 : definition.DronesPerWave;
      builderContractEscort.InitialDelay = MyTimeSpan.FromSeconds((double) definition.DroneFirstDelayInS).Ticks;
      builderContractEscort.WaveFactionId = factionId2;
      builderContractEscort.EscortShipOwner = factionId1;
      builderContractEscort.Creation = now.Ticks;
      builderContractEscort.RemainingTimeInS = new double?(this.GetDurationForEscortContract(definition, num2, MyContractEscort.DRONE_SPEED_LIMIT).Seconds);
      builderContractEscort.TicksToDiscard = new int?(MyContractTypeBaseStrategy.TICKS_TO_LIVE);
      myContractEscort.Init((MyObjectBuilder_Contract) builderContractEscort);
      contract = (MyContract) myContractEscort;
      return MyContractCreationResults.Success;
    }

    private MyTimeSpan GetDurationForEscortContract(
      MyContractTypeEscortDefinition def,
      double distanceInM,
      float maxSpeed)
    {
      return MyTimeSpan.FromSeconds(def.DurationMultiplier * ((double) def.Duration_BaseTime + (double) (long) ((double) def.Duration_FlightTimeMultiplier * distanceInM / (double) maxSpeed)));
    }

    private long GetMoneyReward_Escort(long baseRew, double distance) => (long) ((double) baseRew * Math.Pow(3.0, Math.Log10(distance)));

    private int GetReputationReward_Escort(int baseRew, double distance) => baseRew;

    public long GetPirateFactionId()
    {
      if (!(MyDefinitionManager.Static.GetDefinition(this.m_economyDefinition.PirateId) is MyFactionDefinition definition))
        return 0;
      MyFaction myFaction = (MyFaction) null;
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        if (faction.Value.Tag == definition.Tag)
        {
          myFaction = faction.Value;
          break;
        }
      }
      return myFaction == null ? 0L : myFaction.FactionId;
    }

    private MyFaction FindRandomEnemyFaction(long factionId)
    {
      MyFactionCollection factions = MySession.Static.Factions;
      List<MyFaction> myFactionList = new List<MyFaction>();
      foreach (KeyValuePair<long, MyFaction> keyValuePair in factions)
      {
        if (keyValuePair.Value.FactionType != MyFactionTypes.None && keyValuePair.Value.FactionType != MyFactionTypes.PlayerMade && factions.GetRelationBetweenFactions(factionId, keyValuePair.Value.FactionId).Item1 == MyRelationsBetweenFactions.Enemies)
          myFactionList.Add(keyValuePair.Value);
      }
      return myFactionList.Count <= 0 ? (MyFaction) null : myFactionList[MyRandom.Instance.Next(0, myFactionList.Count)];
    }
  }
}
