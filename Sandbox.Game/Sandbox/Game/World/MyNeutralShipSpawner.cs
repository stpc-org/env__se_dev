// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyNeutralShipSpawner
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI.Autopilots;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 500, typeof (MyObjectBuilder_NeutralShipSpawner), null, false)]
  internal class MyNeutralShipSpawner : MySessionComponentBase
  {
    public const float NEUTRAL_SHIP_SPAWN_DISTANCE = 8000f;
    public const float NEUTRAL_SHIP_FORBIDDEN_RADIUS = 2000f;
    public const float NEUTRAL_SHIP_DIRECTION_SPREAD = 0.5f;
    public const float NEUTRAL_SHIP_MINIMAL_ROUTE_LENGTH = 10000f;
    public const float NEUTRAL_SHIP_SPAWN_OFFSET = 500f;
    public static TimeSpan NEUTRAL_SHIP_RESCHEDULE_TIME = TimeSpan.FromSeconds(10.0);
    public static TimeSpan NEUTRAL_SHIP_MIN_TIME = TimeSpan.FromMinutes(13.0);
    public static TimeSpan NEUTRAL_SHIP_MAX_TIME = TimeSpan.FromMinutes(17.0);
    public static TimeSpan NEUTRAL_SHIP_LIFE_TIME = TimeSpan.FromMinutes(30.0);
    private const int EVENT_SPAWN_TRY_MAX = 3;
    private static List<MyPhysics.HitInfo> m_raycastHits = new List<MyPhysics.HitInfo>();
    private static List<float> m_spawnGroupCumulativeFrequencies = new List<float>();
    private static float m_spawnGroupTotalFrequencies = 0.0f;
    private static float[] m_upVecMultipliers = new float[4]
    {
      1f,
      1f,
      -1f,
      -1f
    };
    private static float[] m_rightVecMultipliers = new float[4]
    {
      1f,
      -1f,
      -1f,
      1f
    };
    private static List<MySpawnGroupDefinition> m_spawnGroups = new List<MySpawnGroupDefinition>();
    private static int m_eventSpawnTry = 0;
    private static List<MyEntity> m_tempFoundEnts = new List<MyEntity>();
    private static List<MyEntity> m_tempFoundEntsDespawn = new List<MyEntity>();
    private static List<Tuple<List<long>, TimeSpan>> m_shipsInProgress = new List<Tuple<List<long>, TimeSpan>>();

    public override bool IsRequiredByGame => MyPerGameSettings.Game == GameEnum.SE_GAME;

    public override void LoadData()
    {
      MySandboxGame.Log.WriteLine("Pre-loading neutral ship spawn groups...");
      foreach (MySpawnGroupDefinition spawnGroupDefinition in MyDefinitionManager.Static.GetSpawnGroupDefinitions())
      {
        if (spawnGroupDefinition.IsCargoShip)
          MyNeutralShipSpawner.m_spawnGroups.Add(spawnGroupDefinition);
      }
      MyNeutralShipSpawner.m_spawnGroupTotalFrequencies = 0.0f;
      MyNeutralShipSpawner.m_spawnGroupCumulativeFrequencies.Clear();
      foreach (MySpawnGroupDefinition spawnGroup in MyNeutralShipSpawner.m_spawnGroups)
      {
        MyNeutralShipSpawner.m_spawnGroupTotalFrequencies += spawnGroup.Frequency;
        MyNeutralShipSpawner.m_spawnGroupCumulativeFrequencies.Add(MyNeutralShipSpawner.m_spawnGroupTotalFrequencies);
      }
      MySandboxGame.Log.WriteLine("End pre-loading neutral ship spawn groups.");
    }

    protected override void UnloadData()
    {
      MyNeutralShipSpawner.m_spawnGroupTotalFrequencies = 0.0f;
      MyNeutralShipSpawner.m_spawnGroupCumulativeFrequencies.Clear();
      for (int index = MyNeutralShipSpawner.m_shipsInProgress.Count - 1; index >= 0; --index)
      {
        foreach (long entityId in MyNeutralShipSpawner.m_shipsInProgress[index].Item1)
        {
          MyCubeGrid entity;
          if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(entityId, out entity))
          {
            entity.OnGridChanged -= new Action<MyCubeGrid>(MyNeutralShipSpawner.OnGridChanged);
            entity.OnBlockAdded -= new Action<MySlimBlock>(MyNeutralShipSpawner.OnBlockAddedRemovedOrChanged);
            entity.OnBlockRemoved -= new Action<MySlimBlock>(MyNeutralShipSpawner.OnBlockAddedRemovedOrChanged);
            entity.OnBlockIntegrityChanged -= new Action<MySlimBlock>(MyNeutralShipSpawner.OnBlockAddedRemovedOrChanged);
          }
        }
      }
      MyNeutralShipSpawner.m_shipsInProgress.Clear();
      MyNeutralShipSpawner.m_spawnGroups.Clear();
      MyNeutralShipSpawner.m_raycastHits.Clear();
      this.Session = (IMySession) null;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_NeutralShipSpawner objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_NeutralShipSpawner;
      objectBuilder.ShipsInProgress = new List<MyObjectBuilder_NeutralShipSpawner.ShipTimePair>(MyNeutralShipSpawner.m_shipsInProgress.Count);
      foreach (Tuple<List<long>, TimeSpan> tuple in MyNeutralShipSpawner.m_shipsInProgress)
      {
        MyObjectBuilder_NeutralShipSpawner.ShipTimePair shipTimePair = new MyObjectBuilder_NeutralShipSpawner.ShipTimePair();
        shipTimePair.EntityIds = new List<long>(tuple.Item1.Count);
        shipTimePair.TimeTicks = tuple.Item2.Ticks;
        foreach (long num in tuple.Item1)
          shipTimePair.EntityIds.Add(num);
        objectBuilder.ShipsInProgress.Add(shipTimePair);
      }
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      MyObjectBuilder_NeutralShipSpawner neutralShipSpawner = sessionComponent as MyObjectBuilder_NeutralShipSpawner;
      if (neutralShipSpawner.ShipsInProgress == null)
        return;
      MyNeutralShipSpawner.m_shipsInProgress.Clear();
      foreach (MyObjectBuilder_NeutralShipSpawner.ShipTimePair shipTimePair in neutralShipSpawner.ShipsInProgress)
      {
        List<long> longList = new List<long>(shipTimePair.EntityIds.Count);
        foreach (long entityId in shipTimePair.EntityIds)
          longList.Add(entityId);
        Tuple<List<long>, TimeSpan> tuple = new Tuple<List<long>, TimeSpan>(longList, TimeSpan.FromTicks(shipTimePair.TimeTicks));
        MyNeutralShipSpawner.m_shipsInProgress.Add(tuple);
      }
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      if (!Sync.IsServer)
        return;
      bool flag = MyFakes.ENABLE_CARGO_SHIPS && MySession.Static.CargoShipsEnabled;
      MyGlobalEventBase eventById = MyGlobalEvents.GetEventById(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "SpawnCargoShip"));
      if (eventById == null & flag)
        MyGlobalEvents.AddGlobalEvent(MyGlobalEventFactory.CreateEvent(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "SpawnCargoShip")));
      else if (eventById != null)
        eventById.Enabled = flag;
      for (int index = MyNeutralShipSpawner.m_shipsInProgress.Count - 1; index >= 0; --index)
      {
        foreach (long entityId in MyNeutralShipSpawner.m_shipsInProgress[index].Item1)
        {
          MyCubeGrid entity;
          if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(entityId, out entity))
          {
            entity.OnGridChanged += new Action<MyCubeGrid>(MyNeutralShipSpawner.OnGridChanged);
            entity.OnBlockAdded += new Action<MySlimBlock>(MyNeutralShipSpawner.OnBlockAddedRemovedOrChanged);
            entity.OnBlockRemoved += new Action<MySlimBlock>(MyNeutralShipSpawner.OnBlockAddedRemovedOrChanged);
            entity.OnBlockIntegrityChanged += new Action<MySlimBlock>(MyNeutralShipSpawner.OnBlockAddedRemovedOrChanged);
          }
        }
      }
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      for (int index = MyNeutralShipSpawner.m_shipsInProgress.Count - 1; index >= 0; --index)
      {
        bool flag = false;
        foreach (long entityId in MyNeutralShipSpawner.m_shipsInProgress[index].Item1)
        {
          MyCubeGrid entity;
          if (Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCubeGrid>(entityId, out entity) && entity != null)
          {
            MyRemoteControl firstBlockOfType = entity.GetFirstBlockOfType<MyRemoteControl>();
            if (firstBlockOfType != null && !firstBlockOfType.IsAutopilotEnabled())
            {
              flag = true;
              break;
            }
          }
        }
        MyEntity entity1;
        if (MyNeutralShipSpawner.m_shipsInProgress[index].Item2 < MySession.Static.ElapsedGameTime | flag && (!Sandbox.Game.Entities.MyEntities.TryGetEntityById(MyNeutralShipSpawner.m_shipsInProgress[index].Item1[0], out entity1) || !MyNeutralShipSpawner.IsPlayerNearby(entity1.PositionComp.GetPosition())))
        {
          foreach (long entityId in MyNeutralShipSpawner.m_shipsInProgress[index].Item1)
          {
            MyEntity entity2;
            if (Sandbox.Game.Entities.MyEntities.TryGetEntityById(entityId, out entity2))
              entity2.Close();
          }
          MyNeutralShipSpawner.m_shipsInProgress.RemoveAtFast<Tuple<List<long>, TimeSpan>>(index);
        }
      }
      if (!MyDebugDrawSettings.DEBUG_DRAW_NEUTRAL_SHIPS)
        return;
      Vector2 screenCoord = new Vector2(400f, 10f);
      for (int index = MyNeutralShipSpawner.m_shipsInProgress.Count - 1; index >= 0; --index)
      {
        Tuple<List<long>, TimeSpan> tuple = MyNeutralShipSpawner.m_shipsInProgress[index];
        long num = tuple.Item1[0];
        TimeSpan timeSpan = tuple.Item2 - MySession.Static.ElapsedGameTime;
        MyRenderProxy.DebugDrawText2D(screenCoord, "GridId: " + (object) num + "TimeLeft: " + timeSpan.ToString(), Color.White, 0.5f);
        screenCoord.Y += 10f;
      }
    }

    private static bool IsPlayerNearby(Vector3D nearbyPos)
    {
      using (MyUtils.ReuseCollection<MyEntity>(ref MyNeutralShipSpawner.m_tempFoundEntsDespawn))
      {
        BoundingSphereD sphere = new BoundingSphereD(nearbyPos, 2000.0);
        MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, MyNeutralShipSpawner.m_tempFoundEntsDespawn, MyEntityQueryType.Dynamic);
        foreach (MyEntity myEntity in MyNeutralShipSpawner.m_tempFoundEntsDespawn)
        {
          if (myEntity is MyCharacter)
            return true;
        }
        return false;
      }
    }

    private static MySpawnGroupDefinition PickRandomSpawnGroup()
    {
      if (MyNeutralShipSpawner.m_spawnGroupCumulativeFrequencies.Count == 0)
        return (MySpawnGroupDefinition) null;
      float randomFloat = MyUtils.GetRandomFloat(0.0f, MyNeutralShipSpawner.m_spawnGroupTotalFrequencies);
      int index = 0;
      while (index < MyNeutralShipSpawner.m_spawnGroupCumulativeFrequencies.Count && ((double) randomFloat > (double) MyNeutralShipSpawner.m_spawnGroupCumulativeFrequencies[index] || !MyNeutralShipSpawner.m_spawnGroups[index].Enabled))
        ++index;
      if (index >= MyNeutralShipSpawner.m_spawnGroupCumulativeFrequencies.Count)
        index = MyNeutralShipSpawner.m_spawnGroupCumulativeFrequencies.Count - 1;
      return MyNeutralShipSpawner.m_spawnGroups[index];
    }

    private static void GetSafeBoundingBoxForPlayers(
      Vector3D start,
      double spawnDistance,
      out BoundingBoxD output)
    {
      double radius = 10.0;
      BoundingSphereD boundingSphereD = new BoundingSphereD(start, radius);
      ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
      bool flag = true;
      while (flag)
      {
        flag = false;
        foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
        {
          Vector3D position = myPlayer.GetPosition();
          double num = (boundingSphereD.Center - position).Length() - boundingSphereD.Radius;
          if (num > 0.0 && num <= spawnDistance * 2.0)
          {
            boundingSphereD.Include(new BoundingSphereD(position, radius));
            flag = true;
          }
        }
      }
      boundingSphereD.Radius += spawnDistance;
      output = new BoundingBoxD(boundingSphereD.Center - new Vector3D(boundingSphereD.Radius), boundingSphereD.Center + new Vector3D(boundingSphereD.Radius));
      List<MyEntity> entitiesInAabb = Sandbox.Game.Entities.MyEntities.GetEntitiesInAABB(ref output);
      foreach (MyEntity myEntity in entitiesInAabb)
      {
        if (myEntity is MyCubeGrid)
        {
          MyCubeGrid myCubeGrid = myEntity as MyCubeGrid;
          if (myCubeGrid.IsStatic)
          {
            Vector3D position = myCubeGrid.PositionComp.GetPosition();
            output.Include(new BoundingBoxD(position - spawnDistance, position + spawnDistance));
          }
        }
      }
      entitiesInAabb.Clear();
    }

    [MyGlobalEventHandler(typeof (MyObjectBuilder_GlobalEventBase), "SpawnCargoShip")]
    public static void OnGlobalSpawnEvent(object senderEvent)
    {
      MySpawnGroupDefinition spawnGroupDefinition = MyNeutralShipSpawner.PickRandomSpawnGroup();
      if (spawnGroupDefinition == null)
        return;
      spawnGroupDefinition.ReloadPrefabs();
      SpawningOptions spawningOptions = SpawningOptions.None;
      long ownerId = 0;
      bool visitStationIfPossible = false;
      if (spawnGroupDefinition.IsPirate)
      {
        ownerId = MyPirateAntennas.GetPiratesId();
      }
      else
      {
        spawningOptions |= SpawningOptions.SetNeutralOwner;
        visitStationIfPossible = true;
      }
      if (!MySession.Static.NPCBlockLimits.HasRemainingPCU)
      {
        MySandboxGame.Log.Log(MyLogSeverity.Info, "Pirate PCUs exhausted. Cargo ship will not spawn.");
      }
      else
      {
        double num1 = 8000.0;
        Vector3D start = Vector3D.Zero;
        bool flag = Sandbox.Game.Entities.MyEntities.IsWorldLimited();
        if (flag)
        {
          num1 = Math.Min(num1, (double) Sandbox.Game.Entities.MyEntities.WorldSafeHalfExtent() - (double) spawnGroupDefinition.SpawnRadius);
        }
        else
        {
          ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
          int randomInt = MyUtils.GetRandomInt(0, Math.Max(0, onlinePlayers.Count - 1));
          int num2 = 0;
          foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
          {
            if (num2 == randomInt)
            {
              if (myPlayer.Character != null)
              {
                start = myPlayer.GetPosition();
                break;
              }
              break;
            }
            ++num2;
          }
        }
        if (num1 < 0.0)
        {
          MySandboxGame.Log.WriteLine("Not enough space in the world to spawn such a huge spawn group!");
        }
        else
        {
          double num2 = 2000.0;
          Vector3D vector3D1;
          BoundingBoxD spawnBox;
          if (flag)
          {
            spawnBox = new BoundingBoxD(start - num1, start + num1);
          }
          else
          {
            MyNeutralShipSpawner.GetSafeBoundingBoxForPlayers(start, num1, out spawnBox);
            double num3 = num2;
            vector3D1 = spawnBox.HalfExtents;
            double num4 = vector3D1.Max() - 2000.0;
            num2 = num3 + num4;
          }
          Vector3D? nullable1 = new Vector3D?();
          for (int index = 0; index < 10; ++index)
          {
            nullable1 = Sandbox.Game.Entities.MyEntities.TestPlaceInSpace(new Vector3D?(MyUtils.GetRandomBorderPosition(ref spawnBox)).Value, spawnGroupDefinition.SpawnRadius);
            if (nullable1.HasValue)
              break;
          }
          if (!nullable1.HasValue)
          {
            MyNeutralShipSpawner.RetryEventWithMaxTry(senderEvent as MyGlobalEventBase);
          }
          else
          {
            Vector3D zero = Vector3D.Zero;
            Vector3 direction = (Vector3) Vector3D.One;
            double num3 = num2;
            vector3D1 = nullable1.Value - spawnBox.Center;
            double num4 = vector3D1.Length();
            double num5 = Math.Atan(num3 / num4);
            direction = -Vector3.Normalize(nullable1.Value);
            float randomFloat = MyUtils.GetRandomFloat((float) num5, (float) (num5 + 0.5));
            float randomRadian = MyUtils.GetRandomRadian();
            Vector3 perpendicularVector1 = Vector3.CalculatePerpendicularVector(direction);
            Vector3 vector3_1 = Vector3.Cross(direction, perpendicularVector1);
            Vector3 vector3_2 = perpendicularVector1 * (float) (Math.Sin((double) randomFloat) * Math.Cos((double) randomRadian));
            Vector3 vector3_3 = vector3_1 * (float) (Math.Sin((double) randomFloat) * Math.Sin((double) randomRadian));
            direction = direction * (float) Math.Cos((double) randomFloat) + vector3_2 + vector3_3;
            double? nullable2 = new RayD(nullable1.Value, (Vector3D) direction).Intersects(spawnBox);
            Vector3D vector3D2 = !nullable2.HasValue || nullable2.Value < 10000.0 ? (Vector3D) (direction * 10000f) : (Vector3D) (direction * (float) nullable2.Value);
            Vector3D vector3D3 = nullable1.Value + vector3D2;
            Vector3 perpendicularVector2 = Vector3.CalculatePerpendicularVector(direction);
            Vector3 vector3_4 = Vector3.Cross(direction, perpendicularVector2);
            MatrixD world = MatrixD.CreateWorld(nullable1.Value, direction, perpendicularVector2);
            MyNeutralShipSpawner.m_raycastHits.Clear();
            foreach (MySpawnGroupDefinition.SpawnGroupPrefab prefab in spawnGroupDefinition.Prefabs)
            {
              MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(prefab.SubtypeId);
              Vector3D vector3D4 = Vector3.Transform(prefab.Position, world);
              Vector3D vector3D5 = vector3D4 + vector3D2;
              float num6 = prefabDefinition == null ? 10f : prefabDefinition.BoundingSphere.Radius;
              if (MyGravityProviderSystem.IsPositionInNaturalGravity(vector3D4, (double) spawnGroupDefinition.SpawnRadius))
              {
                MyNeutralShipSpawner.RetryEventWithMaxTry(senderEvent as MyGlobalEventBase);
                return;
              }
              if (MyGravityProviderSystem.IsPositionInNaturalGravity(vector3D5, (double) spawnGroupDefinition.SpawnRadius))
              {
                MyNeutralShipSpawner.RetryEventWithMaxTry(senderEvent as MyGlobalEventBase);
                return;
              }
              if (MyGravityProviderSystem.DoesTrajectoryIntersectNaturalGravity(vector3D4, vector3D5, (double) spawnGroupDefinition.SpawnRadius + 500.0))
              {
                MyNeutralShipSpawner.RetryEventWithMaxTry(senderEvent as MyGlobalEventBase);
                return;
              }
              MyPhysics.CastRay(vector3D4, vector3D5, MyNeutralShipSpawner.m_raycastHits, 24);
              if (MyNeutralShipSpawner.m_raycastHits.Count > 0)
              {
                MyNeutralShipSpawner.RetryEventWithMaxTry(senderEvent as MyGlobalEventBase);
                return;
              }
              for (int index = 0; index < 4; ++index)
              {
                Vector3D vector3D6 = (Vector3D) (perpendicularVector2 * MyNeutralShipSpawner.m_upVecMultipliers[index] * num6 + vector3_4 * MyNeutralShipSpawner.m_rightVecMultipliers[index] * num6);
                MyPhysics.CastRay(vector3D4 + vector3D6, vector3D5 + vector3D6, MyNeutralShipSpawner.m_raycastHits, 24);
                if (MyNeutralShipSpawner.m_raycastHits.Count > 0)
                {
                  MyNeutralShipSpawner.RetryEventWithMaxTry(senderEvent as MyGlobalEventBase);
                  return;
                }
              }
            }
            foreach (MySpawnGroupDefinition.SpawnGroupPrefab prefab in spawnGroupDefinition.Prefabs)
            {
              MySpawnGroupDefinition.SpawnGroupPrefab shipPrefab = prefab;
              Vector3D position = Vector3D.Transform((Vector3D) shipPrefab.Position, world);
              Vector3D shipDestination = position + vector3D2;
              Vector3 perpendicularVector3 = Vector3.CalculatePerpendicularVector(-direction);
              List<MyCubeGrid> tmpGridList = new List<MyCubeGrid>();
              Stack<Action> callbacks = new Stack<Action>();
              callbacks.Push((Action) (() => MyNeutralShipSpawner.InitCargoShip(shipDestination, direction, spawnBox, visitStationIfPossible, shipPrefab, tmpGridList)));
              spawningOptions |= SpawningOptions.RotateFirstCockpitTowardsDirection | SpawningOptions.SpawnRandomCargo | SpawningOptions.DisableDampeners | SpawningOptions.SetAuthorship;
              MyPrefabManager.Static.SpawnPrefab(tmpGridList, shipPrefab.SubtypeId, position, direction, perpendicularVector3, shipPrefab.Speed * direction, beaconName: shipPrefab.BeaconText, spawningOptions: spawningOptions, ownerId: ownerId, callbacks: callbacks);
            }
            MyNeutralShipSpawner.m_eventSpawnTry = 0;
          }
        }
      }
    }

    private static void InitCargoShip(
      Vector3D shipDestination,
      Vector3 direction,
      BoundingBoxD spawnBox,
      bool visitStationIfPossible,
      MySpawnGroupDefinition.SpawnGroupPrefab shipPrefab,
      List<MyCubeGrid> tmpGridList)
    {
      bool flag = false;
      if (tmpGridList == null || tmpGridList != null && tmpGridList.Count == 0)
      {
        MyLog.Default.WriteLine("Cargo Ship failed to spawn - " + shipPrefab.SubtypeId + " ");
      }
      else
      {
        MyRemoteControl firstBlockOfType = tmpGridList[0].GetFirstBlockOfType<MyRemoteControl>();
        if (visitStationIfPossible && firstBlockOfType != null)
          flag = MyNeutralShipSpawner.SetVisitStationWaypoints(shipDestination, spawnBox, tmpGridList, firstBlockOfType, shipPrefab);
        if (!flag && firstBlockOfType != null)
        {
          MyNeutralShipSpawner.SetOneDestinationPoint(shipDestination, tmpGridList, firstBlockOfType, shipPrefab);
          flag = true;
        }
        if (!flag)
        {
          MyNeutralShipSpawner.InitAutopilot(tmpGridList, shipDestination, (Vector3D) direction, shipPrefab.SubtypeId);
        }
        else
        {
          foreach (MyCubeGrid tmpGrid in tmpGridList)
          {
            tmpGrid.OnGridChanged += new Action<MyCubeGrid>(MyNeutralShipSpawner.OnGridChanged);
            tmpGrid.OnBlockAdded += new Action<MySlimBlock>(MyNeutralShipSpawner.OnBlockAddedRemovedOrChanged);
            tmpGrid.OnBlockRemoved += new Action<MySlimBlock>(MyNeutralShipSpawner.OnBlockAddedRemovedOrChanged);
            tmpGrid.OnBlockIntegrityChanged += new Action<MySlimBlock>(MyNeutralShipSpawner.OnBlockAddedRemovedOrChanged);
          }
        }
        foreach (MyCubeGrid tmpGrid in tmpGridList)
          tmpGrid.ActivatePhysics();
      }
    }

    private static void OnBlockAddedRemovedOrChanged(MySlimBlock block) => MyNeutralShipSpawner.OnGridChanged(block.CubeGrid);

    private static void OnGridChanged(MyCubeGrid grid)
    {
      for (int index = MyNeutralShipSpawner.m_shipsInProgress.Count - 1; index >= 0; --index)
      {
        if (MyNeutralShipSpawner.m_shipsInProgress[index].Item1.Contains(grid.EntityId))
        {
          MyNeutralShipSpawner.m_shipsInProgress.RemoveAtFast<Tuple<List<long>, TimeSpan>>(index);
          grid.OnGridChanged -= new Action<MyCubeGrid>(MyNeutralShipSpawner.OnGridChanged);
          grid.OnBlockAdded -= new Action<MySlimBlock>(MyNeutralShipSpawner.OnBlockAddedRemovedOrChanged);
          grid.OnBlockRemoved -= new Action<MySlimBlock>(MyNeutralShipSpawner.OnBlockAddedRemovedOrChanged);
          grid.OnBlockIntegrityChanged -= new Action<MySlimBlock>(MyNeutralShipSpawner.OnBlockAddedRemovedOrChanged);
        }
      }
    }

    private static void SetOneDestinationPoint(
      Vector3D shipDestination,
      List<MyCubeGrid> tmpGridList,
      MyRemoteControl remoteBlock,
      MySpawnGroupDefinition.SpawnGroupPrefab shipPrefab)
    {
      Vector3D coords = shipDestination;
      remoteBlock.AddWaypoint(new MyWaypointInfo("endWaypoint", coords));
      MyNeutralShipSpawner.SetupRemoteBlock(remoteBlock, shipPrefab);
      TimeSpan timeSpan = MySession.Static.ElapsedGameTime + MyNeutralShipSpawner.NEUTRAL_SHIP_LIFE_TIME;
      List<long> longList = new List<long>(tmpGridList.Count);
      foreach (MyCubeGrid tmpGrid in tmpGridList)
        longList.Add(tmpGrid.EntityId);
      Tuple<List<long>, TimeSpan> tuple = new Tuple<List<long>, TimeSpan>(longList, timeSpan);
      MyNeutralShipSpawner.m_shipsInProgress.Add(tuple);
    }

    private static bool SetVisitStationWaypoints(
      Vector3D shipDestination,
      BoundingBoxD spawnBox,
      List<MyCubeGrid> tmpGridList,
      MyRemoteControl remoteBlock,
      MySpawnGroupDefinition.SpawnGroupPrefab shipPrefab)
    {
      tmpGridList[0].RecalculateOwners();
      long playerId1 = tmpGridList[0].BigOwners.Count > 0 ? tmpGridList[0].BigOwners[0] : 0L;
      if (playerId1 != 0L)
      {
        using (MyUtils.ReuseCollection<MyEntity>(ref MyNeutralShipSpawner.m_tempFoundEnts))
        {
          spawnBox.Inflate(-400.0);
          MyGamePruningStructure.GetTopMostEntitiesInBox(ref spawnBox, MyNeutralShipSpawner.m_tempFoundEnts, MyEntityQueryType.Static);
          foreach (MyEntity tempFoundEnt in MyNeutralShipSpawner.m_tempFoundEnts)
          {
            if (tempFoundEnt is MyCubeGrid myCubeGrid)
            {
              MyStoreBlock firstBlockOfType = myCubeGrid.GetFirstBlockOfType<MyStoreBlock>();
              if (firstBlockOfType != null && firstBlockOfType.IsFunctional)
              {
                MatrixD worldMatrix = tempFoundEnt.WorldMatrix;
                if (!MyGravityProviderSystem.IsPositionInNaturalGravity(worldMatrix.Translation) || (double) MyGravityProviderSystem.CalculateNaturalGravityInPoint(worldMatrix.Translation).LengthSquared() <= 0.036)
                {
                  long playerId2 = myCubeGrid.BigOwners.Count > 0 ? myCubeGrid.BigOwners[0] : 0L;
                  if (playerId2 != 0L)
                  {
                    IMyFaction playerFaction1 = MySession.Static.Factions.TryGetPlayerFaction(playerId1);
                    if (playerFaction1 != null)
                    {
                      IMyFaction playerFaction2 = MySession.Static.Factions.TryGetPlayerFaction(playerId2);
                      if (playerFaction2 != null)
                      {
                        Tuple<MyRelationsBetweenFactions, int> relationBetweenFactions = MySession.Static.Factions.GetRelationBetweenFactions(playerFaction1.FactionId, playerFaction2.FactionId);
                        if (relationBetweenFactions.Item1 != MyRelationsBetweenFactions.Enemies && relationBetweenFactions.Item1 != MyRelationsBetweenFactions.Neutral)
                        {
                          float radius = tempFoundEnt.PositionComp.LocalVolume.Radius;
                          Vector3D coords1 = worldMatrix.Translation + worldMatrix.Backward * 500.0;
                          Vector3D coords2 = worldMatrix.Translation + worldMatrix.Backward * (double) radius * 2.0 + worldMatrix.Left * (double) radius * 1.5;
                          Vector3D coords3 = coords2 + worldMatrix.Forward * (double) radius * 2.0;
                          Vector3D coords4 = coords3 + worldMatrix.Forward * 500.0;
                          Vector3D vector3D1 = worldMatrix.Translation - tmpGridList[0].PositionComp.GetPosition();
                          if (MyGravityProviderSystem.IsPositionInNaturalGravity(worldMatrix.Translation + vector3D1))
                          {
                            vector3D1 = tmpGridList[0].PositionComp.GetPosition() - worldMatrix.Translation;
                            Vector3D vector3D2 = worldMatrix.Translation + vector3D1;
                          }
                          Vector3D coords5 = worldMatrix.Translation + vector3D1;
                          remoteBlock.AddWaypoint(new MyWaypointInfo("wp1", coords1));
                          remoteBlock.AddWaypoint(new MyWaypointInfo("wp2", coords2));
                          remoteBlock.AddWaypoint(new MyWaypointInfo("wp3", coords3));
                          remoteBlock.AddWaypoint(new MyWaypointInfo("wp4", coords4));
                          remoteBlock.AddWaypoint(new MyWaypointInfo("wp5", coords5));
                          MyNeutralShipSpawner.SetupRemoteBlock(remoteBlock, shipPrefab);
                          TimeSpan timeSpan = MySession.Static.ElapsedGameTime + MyNeutralShipSpawner.NEUTRAL_SHIP_LIFE_TIME;
                          List<long> longList = new List<long>(tmpGridList.Count);
                          foreach (MyCubeGrid tmpGrid in tmpGridList)
                            longList.Add(tmpGrid.EntityId);
                          Tuple<List<long>, TimeSpan> tuple = new Tuple<List<long>, TimeSpan>(longList, timeSpan);
                          MyNeutralShipSpawner.m_shipsInProgress.Add(tuple);
                          return true;
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      return false;
    }

    private static void SetupRemoteBlock(
      MyRemoteControl remoteBlock,
      MySpawnGroupDefinition.SpawnGroupPrefab shipPrefab)
    {
      remoteBlock.SetCollisionAvoidance(true);
      remoteBlock.ChangeDirection(Base6Directions.Direction.Forward);
      remoteBlock.SetAutoPilotSpeedLimit(shipPrefab.Speed);
      remoteBlock.SetWaypointThresholdDistance(1f);
      remoteBlock.ChangeFlightMode(FlightMode.OneWay);
      remoteBlock.SetWaitForFreeWay(true);
      remoteBlock.SetAutoPilotEnabled(true);
    }

    private static void InitAutopilot(
      List<MyCubeGrid> tmpGridList,
      Vector3D shipDestination,
      Vector3D direction,
      string prefabSubtype)
    {
      foreach (MyCubeGrid tmpGrid in tmpGridList)
      {
        MyCockpit firstBlockOfType = tmpGrid.GetFirstBlockOfType<MyCockpit>();
        if (firstBlockOfType != null)
        {
          MySimpleAutopilot mySimpleAutopilot = new MySimpleAutopilot(shipDestination, (Vector3) direction, tmpGridList.ToArray<MyCubeGrid, long>((Func<MyCubeGrid, long>) (x => x.EntityId)));
          firstBlockOfType.AttachAutopilot((MyAutopilotBase) mySimpleAutopilot);
          return;
        }
      }
      MyLog.Default.Error("Missing cockpit on spawngroup " + prefabSubtype);
      foreach (MyEntity tmpGrid in tmpGridList)
        tmpGrid.Close();
    }

    private static void RetryEventWithMaxTry(MyGlobalEventBase evt)
    {
      if (evt == null)
        return;
      if (++MyNeutralShipSpawner.m_eventSpawnTry <= 3)
      {
        MySandboxGame.Log.WriteLine("Could not spawn event. Try " + (object) MyNeutralShipSpawner.m_eventSpawnTry + " of " + (object) 3);
        MyGlobalEvents.RescheduleEvent(evt, MyNeutralShipSpawner.NEUTRAL_SHIP_RESCHEDULE_TIME);
      }
      else
        MyNeutralShipSpawner.m_eventSpawnTry = 0;
    }

    public bool IsEncounter(long entityId)
    {
      foreach (Tuple<List<long>, TimeSpan> tuple in MyNeutralShipSpawner.m_shipsInProgress)
      {
        foreach (long num in tuple.Item1)
        {
          if (num == entityId)
            return true;
        }
      }
      return false;
    }
  }
}
