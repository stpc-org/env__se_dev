// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyStationCellGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public class MyStationCellGenerator : MyProceduralWorldModule
  {
    private HashSet<MyStation> m_spawnInProgress = new HashSet<MyStation>();
    private HashSet<MyStation> m_removeRequested = new HashSet<MyStation>();

    public MyStationCellGenerator(
      double cellSize,
      int radiusMultiplier,
      int seed,
      double density,
      MyProceduralWorldModule parent = null)
      : base(cellSize, radiusMultiplier, seed, density, parent)
    {
    }

    protected override MyProceduralCell GenerateProceduralCell(ref Vector3I cellId)
    {
      MyProceduralCell cell = new MyProceduralCell(cellId, this.CELL_SIZE);
      bool flag = false;
      foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
      {
        foreach (MyStation station in faction.Value.Stations)
        {
          if (cell.BoundingVolume.Contains(station.Position) == ContainmentType.Contains)
          {
            double stationSpawnDistance = this.GetStationSpawnDistance(station.Type);
            cell.AddObject(new MyObjectSeed(cell, station.Position, stationSpawnDistance)
            {
              UserData = (object) station,
              Params = {
                Type = MyObjectSeedType.Station,
                Generated = (ulong) station.StationEntityId > 0UL
              }
            });
            flag = true;
          }
        }
      }
      return !flag ? (MyProceduralCell) null : cell;
    }

    private double GetStationSpawnDistance(MyStationTypeEnum stationType)
    {
      MyDefinitionId subtypeId = new MyDefinitionId();
      switch (stationType)
      {
        case MyStationTypeEnum.MiningStation:
          subtypeId = MyStationGenerator.MINING_STATIONS_ID;
          break;
        case MyStationTypeEnum.OrbitalStation:
          subtypeId = MyStationGenerator.ORBITAL_STATIONS_ID;
          break;
        case MyStationTypeEnum.Outpost:
          subtypeId = MyStationGenerator.OUTPOST_STATIONS_ID;
          break;
        case MyStationTypeEnum.SpaceStation:
          subtypeId = MyStationGenerator.SPACE_STATIONS_ID;
          break;
        default:
          MyLog.Default.Error(string.Format("Stations list for type {0} not defined. Go to Economy_Stations.sbc to add definition.", (object) stationType));
          break;
      }
      MyStationsListDefinition definition = MyDefinitionManager.Static.GetDefinition<MyStationsListDefinition>(subtypeId);
      return definition == null ? this.CELL_SIZE : (double) definition.SpawnDistance;
    }

    public override void GenerateObjects(
      List<MyObjectSeed> list,
      HashSet<MyObjectSeedParams> existingObjectsSeeds)
    {
      foreach (MyObjectSeed myObjectSeed in list)
      {
        MyObjectSeed seed = myObjectSeed;
        MyStation station = seed.UserData as MyStation;
        if (station.StationEntityId == 0L)
        {
          IMyFaction faction = MySession.Static.Factions.TryGetFactionById(station.FactionId);
          if (faction != null && !this.m_spawnInProgress.Contains(station))
          {
            MySafeZone safeZone = station.CreateSafeZone(faction);
            safeZone.AccessTypeGrids = MySafeZoneAccess.Blacklist;
            safeZone.AccessTypeFloatingObjects = MySafeZoneAccess.Blacklist;
            safeZone.AccessTypePlayers = MySafeZoneAccess.Blacklist;
            safeZone.AccessTypeFactions = MySafeZoneAccess.Blacklist;
            safeZone.DisplayName = safeZone.Name = string.Format(MyTexts.GetString(MySpaceTexts.SafeZone_Name_Station), (object) faction.Tag, (object) station.Id);
            MySpawnPrefabProperties spawnProperties = new MySpawnPrefabProperties()
            {
              Position = station.Position,
              Forward = (Vector3) station.Forward,
              Up = (Vector3) station.Up,
              PrefabName = station.PrefabName,
              OwnerId = faction.FounderId,
              Color = faction.CustomColor,
              SpawningOptions = SpawningOptions.SetAuthorship | SpawningOptions.ReplaceColor | SpawningOptions.UseOnlyWorldMatrix,
              UpdateSync = true
            };
            this.m_spawnInProgress.Add(station);
            seed.Params.Generated = true;
            BoundingSphereD boundingSphere = new BoundingSphereD(station.Position, (double) safeZone.Radius);
            using (ClearToken<MyEntity> clearToken = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere).GetClearToken<MyEntity>())
            {
              foreach (MyEntity myEntity in clearToken.List)
              {
                if (myEntity is MyFloatingObject)
                  myEntity.Close();
              }
            }
            MyPrefabManager.Static.SpawnPrefabInternal(spawnProperties, (Action) (() =>
            {
              this.m_spawnInProgress.Remove(station);
              if (spawnProperties.ResultList == null || spawnProperties.ResultList.Count == 0 || spawnProperties.ResultList.Count > 1)
                return;
              MyCubeGrid result = spawnProperties.ResultList[0];
              if (this.m_removeRequested.Contains(station))
              {
                this.RemoveStationGrid(station, result);
                this.m_removeRequested.Remove(station);
              }
              else
              {
                station.StationEntityId = result.EntityId;
                result.IsGenerated = true;
                MyCubeGrid myCubeGrid1 = result;
                MyCubeGrid myCubeGrid2 = result;
                string format = MyTexts.GetString(MySpaceTexts.Grid_Name_Station);
                string tag = faction.Tag;
                string str1 = station.Type.ToString();
                // ISSUE: variable of a boxed type
                __Boxed<long> id = (ValueType) station.Id;
                string str2;
                string str3 = str2 = string.Format(format, (object) tag, (object) str1, (object) id);
                myCubeGrid2.Name = str2;
                string str4 = str3;
                myCubeGrid1.DisplayName = str4;
                station.ResourcesGenerator.UpdateStation(result);
                station.StationGridSpawned();
                if (!Sync.IsServer)
                  return;
                MySession.Static.GetComponent<MySessionComponentEconomy>()?.AddStationGrid(result.EntityId);
                MyPlanetEnvironmentSessionComponent component = MySession.Static.GetComponent<MyPlanetEnvironmentSessionComponent>();
                if (component == null)
                  return;
                BoundingBoxD worldBBox = new BoundingBoxD(station.Position - (double) safeZone.Radius, station.Position + safeZone.Radius);
                component.ClearEnvironmentItems((MyEntity) safeZone, worldBBox);
              }
            }), (Action) (() =>
            {
              station.StationEntityId = 0L;
              this.m_spawnInProgress.Remove(station);
              seed.Params.Generated = false;
            }));
          }
        }
      }
    }

    protected override void CloseObjectSeed(MyObjectSeed objectSeed)
    {
      MyStation userData = objectSeed.UserData as MyStation;
      MySafeZone entity1;
      if (!MyEntities.TryGetEntityById<MySafeZone>(userData.SafeZoneEntityId, out entity1))
        return;
      if (this.m_spawnInProgress.Contains(userData))
      {
        this.m_removeRequested.Add(userData);
        entity1?.Close();
      }
      else
      {
        entity1.Close();
        userData.SafeZoneEntityId = 0L;
        objectSeed.Params.Generated = false;
        if (userData.StationEntityId == 0L)
          return;
        MyCubeGrid entity2;
        if (!MyEntities.TryGetEntityById<MyCubeGrid>(userData.StationEntityId, out entity2))
        {
          MySession.Static.GetComponent<MySessionComponentEconomy>()?.RemoveStationGrid(userData.StationEntityId);
          userData.StationEntityId = 0L;
        }
        else
          this.RemoveStationGrid(userData, entity2);
      }
    }

    private void RemoveStationGrid(MyStation station, MyCubeGrid stationGrid)
    {
      stationGrid.Close();
      MySession.Static.GetComponent<MySessionComponentEconomy>()?.RemoveStationGrid(station.StationEntityId);
      station.StationEntityId = 0L;
      station.ResourcesGenerator.ClearBlocksCache();
    }

    public override void ReclaimObject(object reclaimedObject)
    {
    }
  }
}
