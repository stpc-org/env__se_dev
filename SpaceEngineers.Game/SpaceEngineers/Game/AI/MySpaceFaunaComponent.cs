// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.AI.MySpaceFaunaComponent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.AI.Bot;
using VRage.Game.ObjectBuilders.Components;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageMath.Spatial;
using VRageRender;

namespace SpaceEngineers.Game.AI
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 502, typeof (MyObjectBuilder_SpaceFaunaComponent), null, false)]
  public class MySpaceFaunaComponent : MySessionComponentBase
  {
    private const string WOLF_SUBTYPE_ID = "Wolf";
    private const int UPDATE_DELAY = 120;
    private const int CLEAN_DELAY = 2400;
    private const int ABANDON_DELAY = 45000;
    private const float DESPAWN_DIST = 1000f;
    private const float SPHERE_SPAWN_DIST = 150f;
    private const float PROXIMITY_DIST = 50f;
    private const float TIMEOUT_DIST = 150f;
    private const int MAX_BOTS_PER_PLANET = 10;
    private int m_waitForUpdate = 120;
    private int m_waitForClean = 2400;
    private Action<MyCharacter> m_botCharacterDied;
    private readonly Dictionary<long, MySpaceFaunaComponent.PlanetAIInfo> m_planets = new Dictionary<long, MySpaceFaunaComponent.PlanetAIInfo>();
    private readonly List<Vector3D> m_tmpPlayerPositions = new List<Vector3D>();
    private readonly MyVector3DGrid<MySpaceFaunaComponent.SpawnInfo> m_spawnInfoGrid = new MyVector3DGrid<MySpaceFaunaComponent.SpawnInfo>(150.0);
    private readonly List<MySpaceFaunaComponent.SpawnInfo> m_allSpawnInfos = new List<MySpaceFaunaComponent.SpawnInfo>();
    private readonly MyVector3DGrid<MySpaceFaunaComponent.SpawnTimeoutInfo> m_timeoutInfoGrid = new MyVector3DGrid<MySpaceFaunaComponent.SpawnTimeoutInfo>(150.0);
    private readonly List<MySpaceFaunaComponent.SpawnTimeoutInfo> m_allTimeoutInfos = new List<MySpaceFaunaComponent.SpawnTimeoutInfo>();
    private MyObjectBuilder_SpaceFaunaComponent m_obForLoading;

    public override Type[] Dependencies => new Type[1]
    {
      typeof (MyAIComponent)
    };

    public override bool IsRequiredByGame => MyPerGameSettings.Game == GameEnum.SE_GAME && MyPerGameSettings.EnableAi;

    public override void LoadData()
    {
      base.LoadData();
      if (!Sync.IsServer)
        return;
      MyEntities.OnEntityAdd += new Action<MyEntity>(this.EntityAdded);
      MyEntities.OnEntityRemove += new Action<MyEntity>(this.EntityRemoved);
      MyAIComponent.Static.BotCreatedEvent += new Action<int, MyBotDefinition>(this.OnBotCreatedEvent);
      this.m_botCharacterDied = new Action<MyCharacter>(this.BotCharacterDied);
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      if (!Sync.IsServer)
        return;
      MyEntities.OnEntityAdd -= new Action<MyEntity>(this.EntityAdded);
      MyEntities.OnEntityRemove -= new Action<MyEntity>(this.EntityRemoved);
      MyAIComponent.Static.BotCreatedEvent -= new Action<int, MyBotDefinition>(this.OnBotCreatedEvent);
      this.m_botCharacterDied = (Action<MyCharacter>) null;
      this.m_planets.Clear();
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      this.m_obForLoading = sessionComponent as MyObjectBuilder_SpaceFaunaComponent;
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_SpaceFaunaComponent objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_SpaceFaunaComponent;
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      int num = 0;
      foreach (MySpaceFaunaComponent.SpawnInfo allSpawnInfo in this.m_allSpawnInfos)
      {
        if (!allSpawnInfo.SpawnDone)
          ++num;
      }
      objectBuilder.SpawnInfos.Capacity = num;
      foreach (MySpaceFaunaComponent.SpawnInfo allSpawnInfo in this.m_allSpawnInfos)
      {
        if (!allSpawnInfo.SpawnDone)
        {
          MyObjectBuilder_SpaceFaunaComponent.SpawnInfo spawnInfo = new MyObjectBuilder_SpaceFaunaComponent.SpawnInfo()
          {
            X = allSpawnInfo.Position.X,
            Y = allSpawnInfo.Position.Y,
            Z = allSpawnInfo.Position.Z,
            AbandonTime = Math.Max(0, allSpawnInfo.AbandonTime - timeInMilliseconds),
            SpawnTime = Math.Max(0, allSpawnInfo.SpawnTime - timeInMilliseconds)
          };
          objectBuilder.SpawnInfos.Add(spawnInfo);
        }
      }
      objectBuilder.TimeoutInfos.Capacity = this.m_allTimeoutInfos.Count;
      foreach (MySpaceFaunaComponent.SpawnTimeoutInfo allTimeoutInfo in this.m_allTimeoutInfos)
      {
        MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo timeoutInfo = new MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo()
        {
          X = allTimeoutInfo.Position.X,
          Y = allTimeoutInfo.Position.Y,
          Z = allTimeoutInfo.Position.Z,
          Timeout = Math.Max(0, allTimeoutInfo.TimeoutTime - timeInMilliseconds)
        };
        objectBuilder.TimeoutInfos.Add(timeoutInfo);
      }
      return (MyObjectBuilder_SessionComponent) objectBuilder;
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      if (this.m_obForLoading == null)
        return;
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_allSpawnInfos.Capacity = this.m_obForLoading.SpawnInfos.Count;
      foreach (MyObjectBuilder_SpaceFaunaComponent.SpawnInfo spawnInfo in this.m_obForLoading.SpawnInfos)
      {
        MySpaceFaunaComponent.SpawnInfo data = new MySpaceFaunaComponent.SpawnInfo(spawnInfo, timeInMilliseconds);
        this.m_allSpawnInfos.Add(data);
        this.m_spawnInfoGrid.AddPoint(ref data.Position, data);
      }
      this.m_allTimeoutInfos.Capacity = this.m_obForLoading.TimeoutInfos.Count;
      foreach (MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo timeoutInfo in this.m_obForLoading.TimeoutInfos)
      {
        MySpaceFaunaComponent.SpawnTimeoutInfo data = new MySpaceFaunaComponent.SpawnTimeoutInfo(timeoutInfo, timeInMilliseconds);
        if (data.AnimalSpawnInfo != null)
        {
          this.m_allTimeoutInfos.Add(data);
          this.m_timeoutInfoGrid.AddPoint(ref data.Position, data);
        }
      }
      this.m_obForLoading = (MyObjectBuilder_SpaceFaunaComponent) null;
    }

    private void EntityAdded(MyEntity entity)
    {
      if (!(entity is MyPlanet planet) || !this.PlanetHasFauna(planet))
        return;
      this.m_planets.Add(entity.EntityId, new MySpaceFaunaComponent.PlanetAIInfo(planet));
    }

    private void EntityRemoved(MyEntity entity)
    {
      if (!(entity is MyPlanet))
        return;
      this.m_planets.Remove(entity.EntityId);
    }

    private bool PlanetHasFauna(MyPlanet planet) => planet.Generator.AnimalSpawnInfo?.Animals != null && (uint) planet.Generator.AnimalSpawnInfo.Animals.Length > 0U;

    private void SpawnBot(
      MySpaceFaunaComponent.SpawnInfo spawnInfo,
      MyPlanet planet,
      MyPlanetAnimalSpawnInfo animalSpawnInfo)
    {
      MySpaceFaunaComponent.PlanetAIInfo planetAiInfo;
      if (!this.m_planets.TryGetValue(planet.EntityId, out planetAiInfo) || planetAiInfo.BotNumber >= 10)
        return;
      double spawnDistMin = (double) animalSpawnInfo.SpawnDistMin;
      double spawnDistMax = (double) animalSpawnInfo.SpawnDistMax;
      Vector3D position = spawnInfo.Position;
      Vector3D vector3D1 = (Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
      if (vector3D1 == Vector3D.Zero)
        vector3D1 = Vector3D.Up;
      vector3D1.Normalize();
      Vector3D perpendicularVector = Vector3D.CalculatePerpendicularVector(vector3D1);
      Vector3D bitangent = Vector3D.Cross(vector3D1, perpendicularVector);
      perpendicularVector.Normalize();
      bitangent.Normalize();
      Vector3D vector3D2 = MyUtils.GetRandomDiscPosition(ref position, spawnDistMin, spawnDistMax, ref perpendicularVector, ref bitangent);
      vector3D2 = planet.GetClosestSurfacePointGlobal(ref vector3D2);
      Vector3D? freePlace = MyEntities.FindFreePlace(vector3D2, 2f);
      if (freePlace.HasValue)
        vector3D2 = freePlace.Value;
      planet.CorrectSpawnLocation(ref vector3D2, 2.0);
      if (!(MySpaceFaunaComponent.GetAnimalDefinition(animalSpawnInfo) is MyAgentDefinition animalDefinition))
        return;
      if (animalDefinition.Id.SubtypeName == "Wolf" && MySession.Static.EnableWolfs)
      {
        MyAIComponent.Static.SpawnNewBot(animalDefinition, vector3D2);
      }
      else
      {
        if (!(animalDefinition.Id.SubtypeName != "Wolf") || !MySession.Static.EnableSpiders)
          return;
        MyAIComponent.Static.SpawnNewBot(animalDefinition, vector3D2);
      }
    }

    private void OnBotCreatedEvent(int botSerialNum, MyBotDefinition botDefinition)
    {
      MyPlayer player;
      if (!(botDefinition is MyAgentDefinition myAgentDefinition) || !(myAgentDefinition.FactionTag == "SPID") || !Sync.Players.TryGetPlayerById(new MyPlayer.PlayerId(Sync.MyId, botSerialNum), out player))
        return;
      player.Controller.ControlledEntityChanged += new Action<IMyControllableEntity, IMyControllableEntity>(this.OnBotControlledEntityChanged);
      if (!(player.Controller.ControlledEntity is MyCharacter controlledEntity))
        return;
      controlledEntity.CharacterDied += new Action<MyCharacter>(this.BotCharacterDied);
    }

    private void OnBotControlledEntityChanged(
      IMyControllableEntity oldControllable,
      IMyControllableEntity newControllable)
    {
      if (oldControllable is MyCharacter myCharacter)
        myCharacter.CharacterDied -= new Action<MyCharacter>(this.BotCharacterDied);
      if (!(newControllable is MyCharacter myCharacter))
        return;
      myCharacter.CharacterDied += new Action<MyCharacter>(this.BotCharacterDied);
    }

    private void BotCharacterDied(MyCharacter obj)
    {
      Vector3D position = obj.PositionComp.GetPosition();
      obj.CharacterDied -= new Action<MyCharacter>(this.BotCharacterDied);
      int num = 0;
      MyVector3DGrid<MySpaceFaunaComponent.SpawnTimeoutInfo>.Enumerator pointsCloserThan = this.m_timeoutInfoGrid.GetPointsCloserThan(ref position, 150.0);
      while (pointsCloserThan.MoveNext())
      {
        ++num;
        pointsCloserThan.Current.AddKillTimeout();
      }
      if (num != 0)
        return;
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      MySpaceFaunaComponent.SpawnTimeoutInfo data = new MySpaceFaunaComponent.SpawnTimeoutInfo(position, timeInMilliseconds);
      data.AddKillTimeout();
      this.m_timeoutInfoGrid.AddPoint(ref position, data);
      this.m_allTimeoutInfos.Add(data);
    }

    private static MyBotDefinition GetAnimalDefinition(
      MyPlanetAnimalSpawnInfo animalSpawnInfo)
    {
      int randomInt = MyUtils.GetRandomInt(0, animalSpawnInfo.Animals.Length);
      return (MyBotDefinition) (MyDefinitionManager.Static.GetBotDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AnimalBot), animalSpawnInfo.Animals[randomInt].AnimalType)) as MyAgentDefinition);
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (!Sync.IsServer)
        return;
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_FAUNA_COMPONENT)
        this.DebugDraw();
      --this.m_waitForUpdate;
      if (this.m_waitForUpdate > 0)
        return;
      this.m_waitForUpdate = 120;
      ICollection<MyPlayer> onlinePlayers = Sync.Players.GetOnlinePlayers();
      this.m_tmpPlayerPositions.Capacity = Math.Max(this.m_tmpPlayerPositions.Capacity, onlinePlayers.Count);
      this.m_tmpPlayerPositions.Clear();
      foreach (KeyValuePair<long, MySpaceFaunaComponent.PlanetAIInfo> planet in this.m_planets)
        planet.Value.BotNumber = 0;
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
      {
        if (myPlayer.Id.SerialId == 0)
        {
          if (myPlayer.Controller.ControlledEntity != null)
            this.m_tmpPlayerPositions.Add(myPlayer.GetPosition());
        }
        else if (myPlayer.Controller.ControlledEntity != null)
        {
          MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(myPlayer.GetPosition());
          MySpaceFaunaComponent.PlanetAIInfo planetAiInfo;
          if (closestPlanet != null && this.m_planets.TryGetValue(closestPlanet.EntityId, out planetAiInfo))
            ++planetAiInfo.BotNumber;
        }
      }
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (MyFakes.SPAWN_SPACE_FAUNA_IN_CREATIVE)
      {
        foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
        {
          if (myPlayer.Controller.ControlledEntity != null)
          {
            Vector3D position = myPlayer.GetPosition();
            MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
            MySpaceFaunaComponent.PlanetAIInfo planetAiInfo;
            if (closestPlanet != null && this.PlanetHasFauna(closestPlanet) && this.m_planets.TryGetValue(closestPlanet.EntityId, out planetAiInfo))
            {
              if (myPlayer.Id.SerialId == 0)
              {
                if ((closestPlanet.GetClosestSurfacePointGlobal(ref position) - position).LengthSquared() < 2500.0 && planetAiInfo.BotNumber < 10)
                {
                  int num = 0;
                  MyVector3DGrid<MySpaceFaunaComponent.SpawnInfo>.Enumerator pointsCloserThan1 = this.m_spawnInfoGrid.GetPointsCloserThan(ref position, 150.0);
                  while (pointsCloserThan1.MoveNext())
                  {
                    MySpaceFaunaComponent.SpawnInfo current = pointsCloserThan1.Current;
                    ++num;
                    if (!current.SpawnDone)
                    {
                      if (current.ShouldSpawn(timeInMilliseconds))
                      {
                        current.SpawnDone = true;
                        MyVector3DGrid<MySpaceFaunaComponent.SpawnTimeoutInfo>.Enumerator pointsCloserThan2 = this.m_timeoutInfoGrid.GetPointsCloserThan(ref position, 150.0);
                        bool flag = false;
                        while (pointsCloserThan2.MoveNext())
                        {
                          if (!pointsCloserThan2.Current.IsTimedOut(timeInMilliseconds))
                          {
                            flag = true;
                            break;
                          }
                        }
                        if (!flag)
                        {
                          MyPlanetAnimalSpawnInfo nightAnimalSpawnInfo = MySpaceBotFactory.GetDayOrNightAnimalSpawnInfo(closestPlanet, current.Position);
                          if (nightAnimalSpawnInfo != null)
                          {
                            int randomInt = MyUtils.GetRandomInt(nightAnimalSpawnInfo.WaveCountMin, nightAnimalSpawnInfo.WaveCountMax);
                            for (int index = 0; index < randomInt; ++index)
                              this.SpawnBot(current, closestPlanet, nightAnimalSpawnInfo);
                          }
                        }
                      }
                      else
                        current.UpdateAbandoned(timeInMilliseconds);
                    }
                  }
                  if (num == 0)
                  {
                    MySpaceFaunaComponent.SpawnInfo data = new MySpaceFaunaComponent.SpawnInfo(position, timeInMilliseconds, closestPlanet);
                    this.m_spawnInfoGrid.AddPoint(ref position, data);
                    this.m_allSpawnInfos.Add(data);
                  }
                }
              }
              else
              {
                double val2 = double.MaxValue;
                foreach (Vector3D tmpPlayerPosition in this.m_tmpPlayerPositions)
                  val2 = Math.Min(Vector3D.DistanceSquared(position, tmpPlayerPosition), val2);
                if (val2 > 1000000.0)
                  MyAIComponent.Static.RemoveBot(myPlayer.Id.SerialId, true);
              }
            }
          }
        }
      }
      this.m_tmpPlayerPositions.Clear();
      this.m_waitForClean -= 120;
      if (this.m_waitForClean > 0)
        return;
      MyAIComponent.Static.CleanUnusedIdentities();
      this.m_waitForClean = 2400;
      for (int index = 0; index < this.m_allSpawnInfos.Count; ++index)
      {
        MySpaceFaunaComponent.SpawnInfo allSpawnInfo = this.m_allSpawnInfos[index];
        if (allSpawnInfo.IsAbandoned(timeInMilliseconds) || allSpawnInfo.SpawnDone)
        {
          this.m_allSpawnInfos.RemoveAtFast<MySpaceFaunaComponent.SpawnInfo>(index);
          Vector3D position = allSpawnInfo.Position;
          this.m_spawnInfoGrid.RemovePoint(ref position);
          --index;
        }
      }
      for (int index = 0; index < this.m_allTimeoutInfos.Count; ++index)
      {
        MySpaceFaunaComponent.SpawnTimeoutInfo allTimeoutInfo = this.m_allTimeoutInfos[index];
        if (allTimeoutInfo.IsTimedOut(timeInMilliseconds))
        {
          this.m_allTimeoutInfos.RemoveAtFast<MySpaceFaunaComponent.SpawnTimeoutInfo>(index);
          Vector3D position = allTimeoutInfo.Position;
          this.m_timeoutInfoGrid.RemovePoint(ref position);
          --index;
        }
      }
    }

    private void EraseAllInfos()
    {
      foreach (MySpaceFaunaComponent.SpawnInfo allSpawnInfo in this.m_allSpawnInfos)
        allSpawnInfo.SpawnTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      foreach (MySpaceFaunaComponent.SpawnTimeoutInfo allTimeoutInfo in this.m_allTimeoutInfos)
        this.m_timeoutInfoGrid.RemovePoint(ref allTimeoutInfo.Position);
      this.m_allTimeoutInfos.Clear();
    }

    public void DebugDraw()
    {
      int num1 = 0;
      int num2 = num1 + 1;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) ((double) num1 * 13.0)), "Cleanup in " + (object) this.m_waitForClean, Color.Red, 0.5f);
      int num3 = num2;
      int num4 = num3 + 1;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) ((double) num3 * 13.0)), "Planet infos:", Color.GreenYellow, 0.5f);
      foreach (KeyValuePair<long, MySpaceFaunaComponent.PlanetAIInfo> planet in this.m_planets)
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) num4++ * 13f), "  Name: " + planet.Value.Planet.Generator.FolderName + ", Id: " + (object) planet.Key + ", Bots: " + (object) planet.Value.BotNumber, Color.LightYellow, 0.5f);
      int num5 = num4;
      int num6 = num5 + 1;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, (float) ((double) num5 * 13.0)), "Num. of spawn infos: " + (object) this.m_allSpawnInfos.Count + "/" + (object) this.m_timeoutInfoGrid.Count, Color.GreenYellow, 0.5f);
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      foreach (MySpaceFaunaComponent.SpawnInfo allSpawnInfo in this.m_allSpawnInfos)
      {
        Vector3D position = allSpawnInfo.Position;
        Vector3 vector3 = (Vector3) (allSpawnInfo.Planet.PositionComp.GetPosition() - position);
        double num7 = (double) vector3.Normalize();
        int num8 = Math.Max(0, (allSpawnInfo.SpawnTime - timeInMilliseconds) / 1000);
        int num9 = Math.Max(0, (allSpawnInfo.AbandonTime - timeInMilliseconds) / 1000);
        if (num8 != 0 && num9 != 0)
        {
          MyRenderProxy.DebugDrawSphere(position, 150f, Color.Yellow, depthRead: false);
          MyRenderProxy.DebugDrawText3D(position, "Spawning in: " + (object) num8, Color.Yellow, 0.5f, false);
          MyRenderProxy.DebugDrawText3D(position - vector3 * 0.5f, "Abandoned in: " + (object) num9, Color.Yellow, 0.5f, false);
        }
      }
      foreach (MySpaceFaunaComponent.SpawnTimeoutInfo allTimeoutInfo in this.m_allTimeoutInfos)
      {
        Vector3D position = allTimeoutInfo.Position;
        int num7 = Math.Max(0, (allTimeoutInfo.TimeoutTime - timeInMilliseconds) / 1000);
        MyRenderProxy.DebugDrawSphere(position, 150f, Color.Blue, depthRead: false);
        MyRenderProxy.DebugDrawText3D(position, "Timeout: " + (object) num7, Color.Blue, 0.5f, false);
      }
    }

    private class PlanetAIInfo
    {
      public readonly MyPlanet Planet;
      public int BotNumber;

      public PlanetAIInfo(MyPlanet planet)
      {
        this.Planet = planet;
        this.BotNumber = 0;
      }
    }

    private class SpawnInfo
    {
      public int SpawnTime;
      public int AbandonTime;
      public Vector3D Position;
      public readonly MyPlanet Planet;
      public bool SpawnDone;

      public SpawnInfo(Vector3D position, int gameTime, MyPlanet planet)
      {
        MyPlanetAnimalSpawnInfo nightAnimalSpawnInfo = MySpaceBotFactory.GetDayOrNightAnimalSpawnInfo(planet, position);
        this.SpawnTime = gameTime + MyUtils.GetRandomInt(nightAnimalSpawnInfo.SpawnDelayMin, nightAnimalSpawnInfo.SpawnDelayMax);
        this.AbandonTime = gameTime + 45000;
        this.Position = position;
        this.Planet = planet;
        this.SpawnDone = false;
      }

      public SpawnInfo(MyObjectBuilder_SpaceFaunaComponent.SpawnInfo info, int currentTime)
      {
        this.SpawnTime = currentTime + info.SpawnTime;
        this.AbandonTime = currentTime + info.SpawnTime;
        this.Position = new Vector3D(info.X, info.Y, info.Z);
        this.Planet = MyGamePruningStructure.GetClosestPlanet(this.Position);
        this.SpawnDone = false;
      }

      public bool ShouldSpawn(int currentTime) => this.SpawnTime - currentTime < 0;

      public bool IsAbandoned(int currentTime) => this.AbandonTime - currentTime < 0;

      public void UpdateAbandoned(int currentTime) => this.AbandonTime = currentTime + 45000;
    }

    private class SpawnTimeoutInfo
    {
      public int TimeoutTime;
      public Vector3D Position;
      public readonly MyPlanetAnimalSpawnInfo AnimalSpawnInfo;

      public SpawnTimeoutInfo(Vector3D position, int currentTime)
      {
        this.TimeoutTime = currentTime;
        this.Position = position;
        this.AnimalSpawnInfo = MySpaceBotFactory.GetDayOrNightAnimalSpawnInfo(MyGamePruningStructure.GetClosestPlanet(this.Position), this.Position);
        if (this.AnimalSpawnInfo != null)
          return;
        this.TimeoutTime = currentTime;
      }

      public SpawnTimeoutInfo(
        MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo info,
        int currentTime)
      {
        this.TimeoutTime = currentTime + info.Timeout;
        this.Position = new Vector3D(info.X, info.Y, info.Z);
        this.AnimalSpawnInfo = MySpaceBotFactory.GetDayOrNightAnimalSpawnInfo(MyGamePruningStructure.GetClosestPlanet(this.Position), this.Position);
        if (this.AnimalSpawnInfo != null)
          return;
        this.TimeoutTime = currentTime;
      }

      internal void AddKillTimeout()
      {
        if (this.AnimalSpawnInfo == null)
          return;
        this.TimeoutTime += this.AnimalSpawnInfo.KillDelay;
      }

      internal bool IsTimedOut(int currentTime) => this.TimeoutTime - currentTime < 0;
    }
  }
}
