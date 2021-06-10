// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.AI.MySpaceBotFactory
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
  public class MySpaceBotFactory : MyBotFactoryBase
  {
    public override int MaximumUncontrolledBotCount => 10;

    public override int MaximumBotPerPlayer => 32;

    public override bool CanCreateBotOfType(string behaviorType, bool load) => true;

    public override bool GetBotSpawnPosition(string behaviorType, out Vector3D spawnPosition)
    {
      if (behaviorType == "Spider")
      {
        MatrixD spawnPosition1;
        int num = MySpaceBotFactory.GetSpiderSpawnPosition(out spawnPosition1, new Vector3D?(), 20f) ? 1 : 0;
        spawnPosition = spawnPosition1.Translation;
        return num != 0;
      }
      if (MySession.Static.LocalCharacter != null)
      {
        Vector3D position = MySession.Static.LocalCharacter.PositionComp.GetPosition();
        Vector3 naturalGravityInPoint = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
        Vector3 vector3 = (double) naturalGravityInPoint.LengthSquared() >= 9.99999974737875E-05 ? (Vector3) Vector3D.Normalize((Vector3D) naturalGravityInPoint) : Vector3.Up;
        Vector3D perpendicularVector = (Vector3D) Vector3.CalculatePerpendicularVector(vector3);
        Vector3D bitangent = (Vector3D) Vector3.Cross((Vector3) perpendicularVector, vector3);
        spawnPosition = MyUtils.GetRandomDiscPosition(ref position, 5.0, ref perpendicularVector, ref bitangent);
        return true;
      }
      spawnPosition = Vector3D.Zero;
      return false;
    }

    public static bool GetSpiderSpawnPosition(
      out MatrixD spawnPosition,
      Vector3D? oldPosition,
      float spawnRadius)
    {
      spawnPosition = MatrixD.Identity;
      Vector3D? nullable = new Vector3D?();
      MyPlanet planet = (MyPlanet) null;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if (onlinePlayer.Id.SerialId == 0 && onlinePlayer.Character != null)
        {
          nullable = new Vector3D?(onlinePlayer.GetPosition());
          planet = MyGamePruningStructure.GetClosestPlanet(nullable.Value);
          MyPlanetAnimalSpawnInfo nightAnimalSpawnInfo = MySpaceBotFactory.GetDayOrNightAnimalSpawnInfo(planet, nullable.Value);
          if (nightAnimalSpawnInfo?.Animals == null || !((IEnumerable<MyPlanetAnimal>) nightAnimalSpawnInfo.Animals).Any<MyPlanetAnimal>((Func<MyPlanetAnimal, bool>) (x => x.AnimalType.Contains("Spider"))))
          {
            nullable = new Vector3D?();
            planet = (MyPlanet) null;
          }
          else if (oldPosition.HasValue)
          {
            MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(oldPosition.Value);
            if (planet != closestPlanet)
            {
              nullable = new Vector3D?();
              planet = (MyPlanet) null;
            }
            else
              break;
          }
          else
            break;
        }
      }
      if (!nullable.HasValue || planet == null)
        return false;
      Vector3D vector3D = (Vector3D) planet.Components.Get<MyGravityProviderComponent>().GetWorldGravity(nullable.Value);
      if (Vector3D.IsZero(vector3D))
        vector3D = Vector3D.Down;
      else
        vector3D.Normalize();
      Vector3D result;
      vector3D.CalculatePerpendicularVector(out result);
      Vector3D bitangent = Vector3D.Cross(vector3D, result);
      Vector3D randomDiscPosition = nullable.Value;
      randomDiscPosition = MyUtils.GetRandomDiscPosition(ref randomDiscPosition, (double) spawnRadius, ref result, ref bitangent);
      randomDiscPosition -= vector3D * 500.0;
      Vector3D surfacePointGlobal = planet.GetClosestSurfacePointGlobal(ref randomDiscPosition);
      Vector3D forward = nullable.Value - surfacePointGlobal;
      if (!Vector3D.IsZero(forward))
        forward.Normalize();
      else
        forward = Vector3D.CalculatePerpendicularVector(vector3D);
      spawnPosition = MatrixD.CreateWorld(surfacePointGlobal, forward, -vector3D);
      return true;
    }

    public override bool GetBotGroupSpawnPositions(
      string behaviorType,
      int count,
      List<Vector3D> spawnPositions)
    {
      throw new NotImplementedException();
    }

    public static MyPlanetAnimalSpawnInfo GetDayOrNightAnimalSpawnInfo(
      MyPlanet planet,
      Vector3D position)
    {
      if (planet == null)
        return (MyPlanetAnimalSpawnInfo) null;
      if (planet.Generator.NightAnimalSpawnInfo?.Animals != null && planet.Generator.NightAnimalSpawnInfo.Animals.Length != 0 && MySectorWeatherComponent.IsThereNight(planet, ref position))
        return planet.Generator.NightAnimalSpawnInfo;
      return planet.Generator.AnimalSpawnInfo?.Animals != null && planet.Generator.AnimalSpawnInfo.Animals.Length != 0 ? planet.Generator.AnimalSpawnInfo : (MyPlanetAnimalSpawnInfo) null;
    }
  }
}
