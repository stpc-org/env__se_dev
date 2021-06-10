// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyStationGenerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World.Generator
{
  internal class MyStationGenerator
  {
    public static readonly double ASTEROID_CHECK_RADIUS = 30000.0;
    public static readonly double MIN_STATION_SPACING = 5000.0;
    public static readonly int NUMBER_OF_PLACEMENT_TRIES = 40;
    public static readonly int OUTPOST_NUMBER_OF_PLACEMENT_TRIES = 3;
    public static readonly int OUTPOST_NUMBER_OF_PLACEMENT_TRIES_PLANET_SPECIFIC = 20;
    public static readonly float MAX_STATION_RADIUS = 150f;
    internal static MyDefinitionId MINING_STATIONS_ID = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_StationsListDefinition), "MiningStations");
    internal static MyDefinitionId ORBITAL_STATIONS_ID = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_StationsListDefinition), "OrbitalStations");
    internal static MyDefinitionId OUTPOST_STATIONS_ID = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_StationsListDefinition), "Outposts");
    internal static MyDefinitionId SPACE_STATIONS_ID = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_StationsListDefinition), "SpaceStations");
    private MySessionComponentEconomyDefinition m_def;

    public MyStationGenerator(MySessionComponentEconomyDefinition def) => this.m_def = def;

    public bool GenerateStations(MyFactionCollection factions)
    {
      if (!Sync.IsServer)
        return false;
      List<MyStationGenerator.MyStationCountsPerFaction> minerCounts = new List<MyStationGenerator.MyStationCountsPerFaction>();
      List<MyStationGenerator.MyStationCountsPerFaction> traderCounts = new List<MyStationGenerator.MyStationCountsPerFaction>();
      List<MyStationGenerator.MyStationCountsPerFaction> builderCounts = new List<MyStationGenerator.MyStationCountsPerFaction>();
      foreach (KeyValuePair<long, MyFaction> faction in factions)
      {
        switch (faction.Value.FactionType)
        {
          case MyFactionTypes.Miner:
            minerCounts.Add(new MyStationGenerator.MyStationCountsPerFaction()
            {
              Faction = faction.Value,
              Outpost_req = this.m_def.Station_Rule_Miner_Min_Outpost,
              Mining_req = this.m_def.Station_Rule_Miner_Min_StationM,
              Outpost_add = MyRandom.Instance.Next(0, this.m_def.Station_Rule_Miner_Max_Outpost - this.m_def.Station_Rule_Miner_Min_Outpost + 1),
              Mining_add = MyRandom.Instance.Next(0, this.m_def.Station_Rule_Miner_Max_StationM - this.m_def.Station_Rule_Miner_Min_StationM + 1),
              PreferDeep = false
            });
            continue;
          case MyFactionTypes.Trader:
            traderCounts.Add(new MyStationGenerator.MyStationCountsPerFaction()
            {
              Faction = faction.Value,
              Outpost_req = this.m_def.Station_Rule_Trader_Min_Outpost,
              Orbit_req = this.m_def.Station_Rule_Trader_Min_Orbit,
              Deep_req = this.m_def.Station_Rule_Trader_Min_Deep,
              Outpost_add = MyRandom.Instance.Next(0, this.m_def.Station_Rule_Trader_Max_Outpost - this.m_def.Station_Rule_Trader_Min_Outpost + 1),
              Orbit_add = MyRandom.Instance.Next(0, this.m_def.Station_Rule_Trader_Max_Orbit - this.m_def.Station_Rule_Trader_Min_Orbit + 1),
              Deep_add = MyRandom.Instance.Next(0, this.m_def.Station_Rule_Trader_Max_Deep - this.m_def.Station_Rule_Trader_Min_Deep + 1),
              PreferDeep = true
            });
            continue;
          case MyFactionTypes.Builder:
            builderCounts.Add(new MyStationGenerator.MyStationCountsPerFaction()
            {
              Faction = faction.Value,
              Outpost_req = this.m_def.Station_Rule_Trader_Min_Outpost,
              Orbit_req = this.m_def.Station_Rule_Trader_Min_Orbit,
              Station_req = this.m_def.Station_Rule_Builder_Min_Station,
              Outpost_add = MyRandom.Instance.Next(0, this.m_def.Station_Rule_Builder_Max_Outpost - this.m_def.Station_Rule_Builder_Min_Outpost + 1),
              Orbit_add = MyRandom.Instance.Next(0, this.m_def.Station_Rule_Builder_Max_Orbit - this.m_def.Station_Rule_Builder_Min_Orbit + 1),
              Station_add = MyRandom.Instance.Next(0, this.m_def.Station_Rule_Builder_Max_Station - this.m_def.Station_Rule_Builder_Min_Station + 1),
              PreferDeep = false
            });
            continue;
          default:
            continue;
        }
      }
      HashSet<Vector3D> usedLocations = new HashSet<Vector3D>();
      this.GenerateSpecificStations(minerCounts, traderCounts, builderCounts, usedLocations, true);
      this.GenerateSpecificStations(minerCounts, traderCounts, builderCounts, usedLocations, false);
      return true;
    }

    private void GenerateSpecificStations(
      List<MyStationGenerator.MyStationCountsPerFaction> minerCounts,
      List<MyStationGenerator.MyStationCountsPerFaction> traderCounts,
      List<MyStationGenerator.MyStationCountsPerFaction> builderCounts,
      HashSet<Vector3D> usedLocations,
      bool isRequired)
    {
      for (int index = 0; index < Math.Max(minerCounts.Count, Math.Max(traderCounts.Count, builderCounts.Count)); ++index)
      {
        if (index < minerCounts.Count)
          this.GenerateStationsForFaction(isRequired, minerCounts[index], usedLocations);
        if (index < traderCounts.Count)
          this.GenerateStationsForFaction(isRequired, traderCounts[index], usedLocations);
        if (index < builderCounts.Count)
          this.GenerateStationsForFaction(isRequired, builderCounts[index], usedLocations);
      }
    }

    private bool GenerateStationsForFaction(
      bool required,
      MyStationGenerator.MyStationCountsPerFaction stationCounts,
      HashSet<Vector3D> usedLocations)
    {
      MyStationGenerator.MyStationCountsPerFaction missings = new MyStationGenerator.MyStationCountsPerFaction();
      List<MyPlanet> planets = new List<MyPlanet>();
      foreach (MyEntity entity in MyEntities.GetEntities())
      {
        if (entity is MyPlanet myPlanet)
          planets.Add(myPlanet);
      }
      bool someAsteroids = (double) MySession.Static.Settings.ProceduralDensity > 0.0;
      bool somePlanets = planets.Count > 0;
      if (stationCounts.Outpost(required) > 0 & somePlanets)
        this.GenerateOutposts(required, stationCounts, usedLocations, missings, planets);
      if (stationCounts.Orbit(required) > 0 & somePlanets)
        this.GenerateOrbitalStations(required, stationCounts, usedLocations, missings, planets);
      if (stationCounts.Mining(required) > 0 & someAsteroids)
        this.GenerateMiningStations(required, stationCounts, usedLocations, missings);
      if (stationCounts.Station(required) > 0 || (!someAsteroids || !somePlanets) && !stationCounts.PreferDeep)
        this.GenerateSpaceStations(required, stationCounts, usedLocations, missings, someAsteroids, somePlanets);
      if (stationCounts.Deep(required) > 0 || (!someAsteroids || !somePlanets) && stationCounts.PreferDeep)
        this.GenerateDeepSpaceStations(required, stationCounts, usedLocations, missings, someAsteroids, somePlanets);
      return missings.Outpost(required) + missings.Orbit(required) + missings.Mining(required) + missings.Station(required) + missings.Deep(required) == 0;
    }

    private void GenerateDeepSpaceStations(
      bool required,
      MyStationGenerator.MyStationCountsPerFaction stationCounts,
      HashSet<Vector3D> usedLocations,
      MyStationGenerator.MyStationCountsPerFaction missings,
      bool someAsteroids,
      bool somePlanets)
    {
      if (MySession.Static.Settings.StationsDistanceOuterRadiusEnd <= MySession.Static.Settings.StationsDistanceOuterRadiusStart)
        MyLog.Default.WriteLine("Deep space stations were not spawned. 'Outer Radius End' must be higher than 'Outer Radius Start'.");
      int num = stationCounts.Deep(required);
      if (stationCounts.PreferDeep)
      {
        if (somePlanets)
          num += stationCounts.Outpost(required) + stationCounts.Orbit(required);
        if (someAsteroids)
          num += stationCounts.Mining(required);
      }
      if (num <= 0)
        return;
      for (int index1 = 0; index1 < num; ++index1)
      {
        bool flag = false;
        Vector3D position = Vector3D.Zero;
        for (int index2 = 0; index2 < MyStationGenerator.NUMBER_OF_PLACEMENT_TRIES; ++index2)
        {
          if (this.PlaceRandomStation_Deep(out position) && this.IsStationFarFromOthers(position, usedLocations))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          missings.Deep_Inc(required);
          usedLocations.Add(position);
        }
        else
        {
          MyStationsListDefinition stationTypeDefinition = MyStationGenerator.GetStationTypeDefinition(MyStationTypeEnum.SpaceStation);
          if (stationTypeDefinition == null)
            break;
          MyStation station = new MyStation(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STATION), position, MyStationTypeEnum.SpaceStation, stationCounts.Faction, this.GetRandomStationName(stationTypeDefinition), stationTypeDefinition.GeneratedItemsContainerType, isDeep: true);
          stationCounts.Faction.AddStation(station);
        }
      }
    }

    private void GenerateSpaceStations(
      bool required,
      MyStationGenerator.MyStationCountsPerFaction stationCounts,
      HashSet<Vector3D> usedLocations,
      MyStationGenerator.MyStationCountsPerFaction missings,
      bool someAsteroids,
      bool somePlanets)
    {
      int num = stationCounts.Station(required);
      if (!stationCounts.PreferDeep)
      {
        if (!somePlanets)
          num += stationCounts.Outpost(required) + stationCounts.Orbit(required);
        if (!someAsteroids)
          num += stationCounts.Mining(required);
      }
      if (num <= 0)
        return;
      for (int index1 = 0; index1 < num; ++index1)
      {
        bool flag = false;
        Vector3D position = Vector3D.Zero;
        for (int index2 = 0; index2 < MyStationGenerator.NUMBER_OF_PLACEMENT_TRIES; ++index2)
        {
          if (this.PlaceRandomStation_Station(out position) && this.IsStationFarFromOthers(position, usedLocations))
          {
            flag = true;
            usedLocations.Add(position);
            break;
          }
        }
        if (!flag)
        {
          missings.Station_Inc(required);
        }
        else
        {
          MyStationsListDefinition stationTypeDefinition = MyStationGenerator.GetStationTypeDefinition(MyStationTypeEnum.SpaceStation);
          if (stationTypeDefinition == null)
            break;
          MyStation station = new MyStation(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STATION), position, MyStationTypeEnum.SpaceStation, stationCounts.Faction, this.GetRandomStationName(stationTypeDefinition), stationTypeDefinition.GeneratedItemsContainerType);
          stationCounts.Faction.AddStation(station);
        }
      }
    }

    private void GenerateMiningStations(
      bool required,
      MyStationGenerator.MyStationCountsPerFaction stationCounts,
      HashSet<Vector3D> usedLocations,
      MyStationGenerator.MyStationCountsPerFaction missings)
    {
      for (int index1 = 0; index1 < stationCounts.Mining(required); ++index1)
      {
        bool flag = false;
        Vector3D position = Vector3D.Zero;
        for (int index2 = 0; index2 < MyStationGenerator.NUMBER_OF_PLACEMENT_TRIES; ++index2)
        {
          if (this.PlaceRandomStation_Mining(out position) && this.IsStationFarFromOthers(position, usedLocations))
          {
            flag = true;
            usedLocations.Add(position);
            break;
          }
        }
        if (!flag)
        {
          missings.Mining_Inc(required);
        }
        else
        {
          MyStationsListDefinition stationTypeDefinition = MyStationGenerator.GetStationTypeDefinition(MyStationTypeEnum.MiningStation);
          if (stationTypeDefinition == null)
            break;
          MyStation station = new MyStation(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STATION), position, MyStationTypeEnum.MiningStation, stationCounts.Faction, this.GetRandomStationName(stationTypeDefinition), stationTypeDefinition.GeneratedItemsContainerType);
          stationCounts.Faction.AddStation(station);
        }
      }
    }

    private void GenerateOrbitalStations(
      bool required,
      MyStationGenerator.MyStationCountsPerFaction stationCounts,
      HashSet<Vector3D> usedLocations,
      MyStationGenerator.MyStationCountsPerFaction missings,
      List<MyPlanet> planets)
    {
      for (int index1 = 0; index1 < stationCounts.Orbit(required); ++index1)
      {
        bool flag = false;
        Vector3D position = Vector3D.Zero;
        Vector3 up = Vector3.Zero;
        for (int index2 = 0; index2 < MyStationGenerator.NUMBER_OF_PLACEMENT_TRIES; ++index2)
        {
          if (this.PlaceRandomStation_Orbital(planets[MyRandom.Instance.Next(0, planets.Count)], out position, out up) && this.IsStationFarFromOthers(position, usedLocations))
          {
            flag = true;
            usedLocations.Add(position);
            break;
          }
        }
        if (!flag)
        {
          missings.Orbit_Inc(required);
        }
        else
        {
          MyStationsListDefinition stationTypeDefinition = MyStationGenerator.GetStationTypeDefinition(MyStationTypeEnum.OrbitalStation);
          if (stationTypeDefinition == null)
            break;
          MyStation station = new MyStation(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STATION), position, MyStationTypeEnum.OrbitalStation, stationCounts.Faction, this.GetRandomStationName(stationTypeDefinition), stationTypeDefinition.GeneratedItemsContainerType, new Vector3?(up));
          stationCounts.Faction.AddStation(station);
        }
      }
    }

    private void GenerateOutposts(
      bool required,
      MyStationGenerator.MyStationCountsPerFaction stationCounts,
      HashSet<Vector3D> usedLocations,
      MyStationGenerator.MyStationCountsPerFaction missings,
      List<MyPlanet> planets)
    {
      List<double> doubleList = new List<double>();
      double num1 = 0.0;
      foreach (MyPlanet planet in planets)
      {
        num1 += (double) planet.AverageRadius;
        doubleList.Add(num1);
      }
      if (num1 == 0.0)
        return;
      double num2 = 1.0 / num1;
      for (int index = 0; index < doubleList.Count; ++index)
        doubleList[index] *= num2;
      for (int index1 = 0; index1 < stationCounts.Outpost(required); ++index1)
      {
        bool flag = false;
        Vector3D position = Vector3D.Zero;
        Vector3 up = Vector3.Zero;
        Vector3 forward = Vector3.Zero;
        MyStationsListDefinition stationTypeDefinition = MyStationGenerator.GetStationTypeDefinition(MyStationTypeEnum.Outpost);
        if (stationTypeDefinition == null)
          break;
        string randomStationName = this.GetRandomStationName(stationTypeDefinition);
        MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(randomStationName);
        BoundingBox invalid = BoundingBox.CreateInvalid();
        foreach (MyObjectBuilder_CubeGrid cubeGrid in prefabDefinition.CubeGrids)
        {
          BoundingBox boundingBox = cubeGrid.CalculateBoundingBox();
          invalid.Include(boundingBox);
        }
        MyPlanet planet = (MyPlanet) null;
        for (int index2 = 0; index2 < MyStationGenerator.OUTPOST_NUMBER_OF_PLACEMENT_TRIES; ++index2)
        {
          float num3 = MyRandom.Instance.NextFloat();
          for (int index3 = 0; index3 < planets.Count; ++index3)
          {
            if ((double) num3 < doubleList[index3])
            {
              planet = planets[index3];
              break;
            }
          }
          if (planet == null)
            return;
          for (int index3 = 0; index3 < MyStationGenerator.OUTPOST_NUMBER_OF_PLACEMENT_TRIES_PLANET_SPECIFIC; ++index3)
          {
            if (this.PlaceRandomStation_Outpost(planet, invalid, out position, out up, out forward) && this.IsStationFarFromOthers(position, usedLocations))
            {
              flag = true;
              usedLocations.Add(position);
              break;
            }
          }
          if (flag)
            break;
        }
        if (!flag)
          missings.Outpost_Inc(required);
        else
          stationCounts.Faction.AddStation(new MyStation(MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.STATION), position, MyStationTypeEnum.Outpost, stationCounts.Faction, randomStationName, stationTypeDefinition.GeneratedItemsContainerType, new Vector3?(up), new Vector3?(forward))
          {
            IsOnPlanetWithAtmosphere = planet.HasAtmosphere
          });
      }
    }

    private string GetRandomStationName(MyStationsListDefinition stationsListDef)
    {
      if (stationsListDef == null)
        return "Economy_SpaceStation_1";
      int index = MyRandom.Instance.Next(0, stationsListDef.StationNames.Count);
      return stationsListDef.StationNames[index].ToString();
    }

    internal static MyStationsListDefinition GetStationTypeDefinition(
      MyStationTypeEnum stationType)
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
      return MyDefinitionManager.Static.GetDefinition<MyStationsListDefinition>(subtypeId);
    }

    private bool IsStationFarFromOthers(Vector3D position, HashSet<Vector3D> usedLocations)
    {
      double num = MyStationGenerator.MIN_STATION_SPACING * MyStationGenerator.MIN_STATION_SPACING;
      foreach (Vector3D usedLocation in usedLocations)
      {
        if ((usedLocation - position).LengthSquared() < num)
          return false;
      }
      return true;
    }

    private bool PlaceRandomStation_Outpost(
      MyPlanet planet,
      BoundingBox prefabLocalBBox,
      out Vector3D position,
      out Vector3 up,
      out Vector3 forward)
    {
      position = (Vector3D) Vector3.Zero;
      forward = Vector3.Forward;
      up = Vector3.Up;
      if (!(planet.Components.Get<MyGravityProviderComponent>() is MySphericalNaturalGravityComponent gravityComponent))
        return false;
      Vector3D position1 = planet.PositionComp.GetPosition();
      Vector3D uniformPointOnSphere = new BoundingSphereD(position1, (double) gravityComponent.GravityLimit).RandomToUniformPointOnSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble());
      up = Vector3.Normalize(uniformPointOnSphere - position1);
      position = planet.GetClosestSurfacePointGlobal(uniformPointOnSphere);
      Vector3 localPos = (Vector3) Vector3.Transform((Vector3) position, planet.PositionComp.WorldMatrixInvScaled);
      int face;
      Vector2 texCoord;
      MyCubemapHelpers.CalculateSampleTexcoord(ref localPos, out face, out texCoord);
      Vector3 localNormal;
      double positionCacheless = (double) planet.Provider.Shape.GetValueForPositionCacheless(face, ref texCoord, out localNormal);
      double num1 = (double) localNormal.Normalize();
      float degrees = MathHelper.ToDegrees(MyMath.AngleBetween(Vector3.UnitZ, localNormal));
      if ((double) degrees > 16.0)
        return false;
      Vector3 perpendicularVector = Vector3.CalculatePerpendicularVector(up);
      double num2 = (double) perpendicularVector.Normalize();
      forward = Vector3.Cross(up, perpendicularVector);
      double num3 = (double) forward.Normalize();
      double num4 = (position - position1).Length();
      double num5 = 0.0;
      MatrixD world = MatrixD.CreateWorld(position, forward, up);
      Vector3D globalPos = Vector3D.Transform(prefabLocalBBox.Center, world);
      double num6 = (planet.GetClosestSurfacePointGlobal(globalPos + perpendicularVector * prefabLocalBBox.HalfExtents.X) - position1).Length() - num4;
      if (num6 < 0.0)
        num5 = num6;
      double num7 = (planet.GetClosestSurfacePointGlobal(globalPos - perpendicularVector * prefabLocalBBox.HalfExtents.X) - position1).Length() - num4;
      if (num7 < num5)
        num5 = num7;
      double num8 = (planet.GetClosestSurfacePointGlobal(globalPos + forward * prefabLocalBBox.HalfExtents.Z) - position1).Length() - num4;
      if (num8 < num5)
        num5 = num8;
      double num9 = (planet.GetClosestSurfacePointGlobal(globalPos - forward * prefabLocalBBox.HalfExtents.Z) - position1).Length() - num4;
      if (num9 < num5)
        num5 = num9;
      Vector3D surfacePointGlobal = planet.GetClosestSurfacePointGlobal(globalPos);
      Vector3 vector3 = up * ((float) num5 - 0.25f);
      Vector3D vector3D1 = globalPos + (vector3 - up * prefabLocalBBox.HalfExtents.Y * 0.5f) - position1;
      double num10 = vector3D1.Length();
      vector3D1 = surfacePointGlobal - position1;
      double num11 = vector3D1.Length();
      if (num10 - num11 < 0.0)
        return false;
      position += vector3;
      Vector3D vector3D2 = position - up * 2f;
      if (MyFakes.ENABLE_STATION_GENERATOR_DEBUG_DRAW)
      {
        MyRenderProxy.DebugDrawText3D(position, degrees.ToString(), Color.Red, 1f, false, persistent: true);
        MyRenderProxy.DebugDrawArrow3D(position, position + up * 10f, Color.Red, tipScale: 0.5, persistent: true);
        MyRenderProxy.DebugDrawSphere(position, MyStationGenerator.MAX_STATION_RADIUS, Color.Yellow, depthRead: false, cull: false, persistent: true);
        MyRenderProxy.DebugDrawLine3D(position, planet.PositionComp.GetPosition(), Color.Yellow, Color.Yellow, false, true);
      }
      return true;
    }

    private bool IsMaterialAtPositionBlacklistedForStations(Vector3D position, MyPlanet planet)
    {
      Vector3I voxelCoord;
      MyVoxelCoordSystems.WorldPositionToVoxelCoord(planet.RootVoxel.PositionLeftBottomCorner, ref position, out voxelCoord);
      Vector3I voxelCoords = voxelCoord + planet.RootVoxel.StorageMin;
      MyVoxelMaterialDefinition materialAt = planet.Storage.GetMaterialAt(ref voxelCoords);
      return planet.RootVoxel.Storage.DataProvider is MyPlanetStorageProvider dataProvider && materialAt != null && dataProvider.Material.IsMaterialBlacklistedForStation(materialAt.Id);
    }

    public bool PlaceRandomStation_Orbital(MyPlanet planet, out Vector3D position, out Vector3 up)
    {
      position = (Vector3D) Vector3.Zero;
      up = Vector3.Zero;
      if (!(planet.Components.Get<MyGravityProviderComponent>() is MySphericalNaturalGravityComponent gravityComponent))
        return false;
      Vector3D uniformPointOnSphere = new BoundingSphereD(planet.PositionComp.GetPosition(), (double) gravityComponent.GravityLimit).RandomToUniformPointOnSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble());
      position = uniformPointOnSphere;
      up = Vector3.Normalize(uniformPointOnSphere - planet.PositionComp.GetPosition());
      position += MyStation.SAFEZONE_SIZE * up;
      if (MyFakes.ENABLE_STATION_GENERATOR_DEBUG_DRAW)
      {
        MyRenderProxy.DebugDrawSphere(position, MyStationGenerator.MAX_STATION_RADIUS, Color.CornflowerBlue, depthRead: false, cull: false, persistent: true);
        MyRenderProxy.DebugDrawLine3D(position, planet.PositionComp.GetPosition(), Color.CornflowerBlue, Color.CornflowerBlue, false, true);
      }
      return true;
    }

    public bool PlaceRandomStation_Mining(out Vector3D position)
    {
      position = Vector3D.Zero;
      Vector3D uniformPointInSphere = new BoundingSphereD(Vector3D.Zero, MySession.Static.Settings.StationsDistanceInnerRadius).RandomToUniformPointInSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble());
      Vector3D zero = Vector3D.Zero;
      bool flag = false;
      double asteroidCheckRadius = MyStationGenerator.ASTEROID_CHECK_RADIUS;
      Vector3D? locationCloseToAsteroid = MyProceduralWorldModule.FindFreeLocationCloseToAsteroid(new BoundingSphereD(uniformPointInSphere, asteroidCheckRadius), new BoundingSphereD?(), false, true, MyStationGenerator.MAX_STATION_RADIUS, 0.0f, out Vector3 _, out Vector3 _);
      if (locationCloseToAsteroid.HasValue)
      {
        zero = locationCloseToAsteroid.Value;
        flag = true;
      }
      if (!flag)
        return false;
      if (MyFakes.ENABLE_STATION_GENERATOR_DEBUG_DRAW)
      {
        MyRenderProxy.DebugDrawSphere(zero, MyStationGenerator.MAX_STATION_RADIUS, Color.Red, depthRead: false, cull: false, persistent: true);
        MyRenderProxy.DebugDrawLine3D(zero, Vector3D.Zero, Color.Red, Color.Red, false, true);
      }
      position = zero;
      return true;
    }

    public bool PlaceRandomStation_Station(out Vector3D position)
    {
      position = Vector3D.Zero;
      Vector3D uniformPointInSphere = new BoundingSphereD(Vector3D.Zero, MySession.Static.Settings.StationsDistanceInnerRadius).RandomToUniformPointInSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble());
      List<MyObjectSeed> list = new List<MyObjectSeed>();
      MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(uniformPointInSphere, (double) MyStationGenerator.MAX_STATION_RADIUS), list);
      if (list.Count > 0)
        return false;
      if (MyFakes.ENABLE_STATION_GENERATOR_DEBUG_DRAW)
      {
        MyRenderProxy.DebugDrawSphere(uniformPointInSphere, MyStationGenerator.MAX_STATION_RADIUS, Color.Green, depthRead: false, cull: false, persistent: true);
        MyRenderProxy.DebugDrawLine3D(uniformPointInSphere, Vector3D.Zero, Color.Green, Color.Green, false, true);
      }
      position = uniformPointInSphere;
      return true;
    }

    public bool PlaceRandomStation_Deep(out Vector3D position)
    {
      position = Vector3D.Zero;
      if (MySession.Static.Settings.StationsDistanceOuterRadiusEnd <= MySession.Static.Settings.StationsDistanceOuterRadiusStart)
        return false;
      Vector3D? sphereWithInnerCutout = new BoundingSphereD(Vector3D.Zero, MySession.Static.Settings.StationsDistanceOuterRadiusEnd).RandomToUniformPointInSphereWithInnerCutout(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MySession.Static.Settings.StationsDistanceOuterRadiusStart);
      if (!sphereWithInnerCutout.HasValue)
        return false;
      List<MyObjectSeed> list = new List<MyObjectSeed>();
      MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(sphereWithInnerCutout.Value, (double) MyStationGenerator.MAX_STATION_RADIUS), list);
      if (list.Count > 0)
        return false;
      if (MyFakes.ENABLE_STATION_GENERATOR_DEBUG_DRAW)
      {
        MyRenderProxy.DebugDrawSphere(sphereWithInnerCutout.Value, MyStationGenerator.MAX_STATION_RADIUS, Color.Purple, depthRead: false, cull: false, persistent: true);
        MyRenderProxy.DebugDrawLine3D(sphereWithInnerCutout.Value, Vector3D.Zero, Color.Purple, Color.Purple, false, true);
      }
      position = sphereWithInnerCutout.Value;
      return true;
    }

    private bool GetAsteroidsInSphere(BoundingSphereD sphere, List<MyObjectSeed> output)
    {
      MyProceduralWorldGenerator component = MySession.Static.GetComponent<MyProceduralWorldGenerator>();
      if (component == null)
        return false;
      output.Clear();
      component.GetAllInSphere(sphere, output);
      return true;
    }

    private class MyStationCountsPerFaction
    {
      public MyFaction Faction;
      public int Outpost_req;
      public int Outpost_add;
      public int Orbit_req;
      public int Orbit_add;
      public int Mining_req;
      public int Mining_add;
      public int Station_req;
      public int Station_add;
      public int Deep_req;
      public int Deep_add;
      public bool PreferDeep;

      public int Outpost(bool req = false) => !req ? this.Outpost_add : this.Outpost_req;

      public int Orbit(bool req = false) => !req ? this.Orbit_add : this.Orbit_req;

      public int Mining(bool req = false) => !req ? this.Mining_add : this.Mining_req;

      public int Station(bool req = false) => !req ? this.Station_add : this.Station_req;

      public int Deep(bool req = false) => !req ? this.Deep_add : this.Deep_req;

      public void Outpost_Inc(bool req = false)
      {
        if (req)
          ++this.Outpost_req;
        else
          ++this.Outpost_add;
      }

      public void Orbit_Inc(bool req = false)
      {
        if (req)
          ++this.Orbit_req;
        else
          ++this.Orbit_add;
      }

      public void Mining_Inc(bool req = false)
      {
        if (req)
          ++this.Mining_req;
        else
          ++this.Mining_add;
      }

      public void Station_Inc(bool req = false)
      {
        if (req)
          ++this.Station_req;
        else
          ++this.Station_add;
      }

      public void Deep_Inc(bool req = false)
      {
        if (req)
          ++this.Deep_req;
        else
          ++this.Deep_add;
      }

      internal void PrintSum()
      {
        int num = 0 + (this.Outpost_req + this.Outpost_add) + (this.Orbit_req + this.Orbit_add) + (this.Mining_req + this.Mining_add) + (this.Station_req + this.Station_add);
        int deepReq = this.Deep_req;
        int deepAdd = this.Deep_add;
      }
    }
  }
}
