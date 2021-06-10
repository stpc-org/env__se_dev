// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Contracts.MyContractEscort
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.AI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Contracts
{
  [MyContractDescriptor(typeof (MyObjectBuilder_ContractEscort))]
  public class MyContractEscort : MyContract
  {
    public static readonly int START_PREFAB_TEST_RADIUS = 100;
    public static readonly int MAX_PREFAB_TEST_SPAWN = 20;
    public static readonly int MAX_PREFAB_TEST_PER_DIST = 10;
    public static readonly int DISPOSE_TIME_IN_S = 10;
    public static readonly float DRONE_SPEED_LIMIT = 30f;
    public static readonly float SPAWN_AHEAD_DISTANCE = 1000f;
    public static readonly float SIZE_OF_DRONES = 30f;
    public static readonly float SPAWN_RANDOMIZATION_RADIUS = 100f;
    public static readonly int DRONE_MAX_COUNT = 15;
    private bool m_isBeingDisposed;
    private float m_disposeTime;
    private MyTimeSpan? DisposeTime;
    private List<long> m_drones;

    public long GridId { get; private set; }

    public Vector3D StartPosition { get; private set; }

    public Vector3D EndPosition { get; private set; }

    public double PathLength { get; private set; }

    public double RewardRadius { get; private set; }

    public long TriggerEntityId { get; private set; }

    public double TriggerRadius { get; private set; }

    public MyTimeSpan DroneFirstDelay { get; private set; }

    public MyTimeSpan DroneAttackPeriod { get; private set; }

    public MyTimeSpan InnerTimer { get; private set; }

    public bool IsBehaviorAttached { get; private set; }

    public int DronesPerWave { get; private set; }

    public MyTimeSpan InitialDelay { get; private set; }

    public long WaveFactionId { get; private set; }

    public bool DestinationReached { get; private set; }

    public long EscortShipOwner { get; private set; }

    public override MyObjectBuilder_Contract GetObjectBuilder()
    {
      MyObjectBuilder_Contract objectBuilder = base.GetObjectBuilder();
      MyObjectBuilder_ContractEscort builderContractEscort = objectBuilder as MyObjectBuilder_ContractEscort;
      builderContractEscort.GridId = this.GridId;
      builderContractEscort.StartPosition = (SerializableVector3D) this.StartPosition;
      builderContractEscort.EndPosition = (SerializableVector3D) this.EndPosition;
      builderContractEscort.PathLength = this.PathLength;
      builderContractEscort.RewardRadius = this.RewardRadius;
      builderContractEscort.TriggerEntityId = this.TriggerEntityId;
      builderContractEscort.TriggerRadius = this.TriggerRadius;
      builderContractEscort.DroneFirstDelay = this.DroneFirstDelay.Ticks;
      builderContractEscort.DroneAttackPeriod = this.DroneAttackPeriod.Ticks;
      builderContractEscort.InnerTimer = this.InnerTimer.Ticks;
      builderContractEscort.InitialDelay = this.InitialDelay.Ticks;
      builderContractEscort.DronesPerWave = this.DronesPerWave;
      builderContractEscort.IsBehaviorAttached = this.IsBehaviorAttached;
      builderContractEscort.WaveFactionId = this.WaveFactionId;
      builderContractEscort.EscortShipOwner = this.EscortShipOwner;
      builderContractEscort.Drones = new MySerializableList<long>((IEnumerable<long>) this.m_drones);
      builderContractEscort.DestinationReached = this.DestinationReached;
      return objectBuilder;
    }

    public override void Init(MyObjectBuilder_Contract ob)
    {
      base.Init(ob);
      if (!(ob is MyObjectBuilder_ContractEscort builderContractEscort))
        return;
      this.GridId = builderContractEscort.GridId;
      this.StartPosition = (Vector3D) builderContractEscort.StartPosition;
      this.EndPosition = (Vector3D) builderContractEscort.EndPosition;
      this.PathLength = builderContractEscort.PathLength;
      this.RewardRadius = builderContractEscort.RewardRadius;
      this.TriggerEntityId = builderContractEscort.TriggerEntityId;
      this.TriggerRadius = builderContractEscort.TriggerRadius;
      this.DroneFirstDelay = new MyTimeSpan(builderContractEscort.DroneFirstDelay);
      this.DroneAttackPeriod = new MyTimeSpan(builderContractEscort.DroneAttackPeriod);
      this.InnerTimer = new MyTimeSpan(builderContractEscort.InnerTimer);
      this.InitialDelay = new MyTimeSpan(builderContractEscort.InitialDelay);
      this.DronesPerWave = builderContractEscort.DronesPerWave;
      this.IsBehaviorAttached = builderContractEscort.IsBehaviorAttached;
      this.WaveFactionId = builderContractEscort.WaveFactionId;
      this.EscortShipOwner = builderContractEscort.EscortShipOwner;
      this.m_drones = (List<long>) builderContractEscort.Drones;
      this.DestinationReached = builderContractEscort.DestinationReached;
      if (this.m_drones != null)
        return;
      this.m_drones = new List<long>();
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      if (this.State != MyContractStateEnum.Active)
        return;
      this.SubscribeToTrigger();
      this.SubscribePowerChange();
      if (!this.IsBehaviorAttached || !(MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById))
        return;
      MyRemoteControl firstBlockOfType = entityById.GetFirstBlockOfType<MyRemoteControl>();
      if (firstBlockOfType == null)
        return;
      firstBlockOfType.IsWorkingChanged += new Action<MyCubeBlock>(this.RemoteDestroyedCallback);
    }

    public override MyDefinitionId? GetDefinitionId() => new MyDefinitionId?(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Escort"));

    protected override bool CanBeShared_Internal() => this.State == MyContractStateEnum.Active;

    protected override void Activate_Internal(MyTimeSpan timeOfActivation)
    {
      base.Activate_Internal(timeOfActivation);
      long num = MyEntityIdentifier.AllocateId();
      MyEntity entity = new MyEntity()
      {
        WorldMatrix = MatrixD.Identity,
        EntityId = num,
        DisplayName = "EscortTriggerDummy_" + (object) this.Id,
        Name = "EscortTriggerDummy_" + (object) this.Id
      };
      entity.PositionComp.SetPosition(this.EndPosition);
      entity.Components.Remove<MyPhysicsComponentBase>();
      MyEntities.Add(entity);
      this.TriggerEntityId = num;
      this.AttachTrigger(entity);
      MyContractTypeEscortDefinition definition = this.GetDefinition() as MyContractTypeEscortDefinition;
      if (definition.PrefabsEscortShips.Count > 0)
        this.SpawnPrefab(definition.PrefabsEscortShips[MyRandom.Instance.Next(0, definition.PrefabsEscortShips.Count)]);
      else
        this.SpawnPrefab("Eradicator_mk.1");
      if (MyFakes.CONTRACT_ESCORT_DEBUGDRAW)
      {
        MyRenderProxy.DebugDrawSphere(this.StartPosition, 50f, Color.LimeGreen, depthRead: false, cull: false, persistent: true);
        MyRenderProxy.DebugDrawSphere(this.EndPosition, 50f, Color.Red, depthRead: false, cull: false, persistent: true);
        MyRenderProxy.DebugDrawLine3D(this.StartPosition, this.EndPosition, Color.LimeGreen, Color.Red, false, true);
      }
      this.IsBehaviorAttached = false;
      this.InnerTimer = timeOfActivation + this.InitialDelay;
    }

    protected void SpawnPrefab(string name)
    {
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(this.StartFaction);
      if (factionById == null && this.EscortShipOwner == 0L)
      {
        MyLog.Default.Error("Contract - Escort: Starting faction is not in factions and EscortShipOwner is not specified!!!\n Cannot spawn prefab.");
      }
      else
      {
        Vector3 v = Vector3.Normalize(Vector3.Normalize(this.EndPosition - this.StartPosition));
        Vector3 perpendicularVector = Vector3.CalculatePerpendicularVector(v);
        Vector3D? nullable = MyEntities.FindFreePlace(this.StartPosition, (float) MyContractEscort.START_PREFAB_TEST_RADIUS, MyContractEscort.MAX_PREFAB_TEST_SPAWN, MyContractEscort.MAX_PREFAB_TEST_PER_DIST);
        if (!nullable.HasValue)
          nullable = new Vector3D?(this.StartPosition);
        MySpawnPrefabProperties spawnProperties = new MySpawnPrefabProperties()
        {
          Position = nullable.Value,
          Forward = v,
          Up = perpendicularVector,
          PrefabName = name,
          OwnerId = this.EscortShipOwner > 0L ? this.EscortShipOwner : factionById.FounderId,
          Color = factionById != null ? factionById.CustomColor : Vector3.Zero,
          SpawningOptions = SpawningOptions.SetAuthorship | SpawningOptions.ReplaceColor | SpawningOptions.UseOnlyWorldMatrix,
          UpdateSync = true
        };
        MyPrefabManager.Static.SpawnPrefabInternal(spawnProperties, (Action) (() =>
        {
          if (spawnProperties.ResultList == null || spawnProperties.ResultList.Count == 0 || spawnProperties.ResultList.Count > 1)
            return;
          MyCubeGrid result = spawnProperties.ResultList[0];
          this.GridId = result.EntityId;
          result.GridSystems.GridPowerStateChanged += new Action<long, bool, string>(this.GridPowerStateChanged);
          MyRemoteControl firstBlockOfType = result.GetFirstBlockOfType<MyRemoteControl>();
          if (firstBlockOfType != null)
            firstBlockOfType.IsWorkingChanged += new Action<MyCubeBlock>(this.RemoteDestroyedCallback);
          MyGps gps = this.PrepareGPS();
          foreach (long owner in this.Owners)
            MySession.Static.Gpss.SendAddGps(owner, ref gps, this.GridId);
        }));
      }
    }

    public string GetGpsName() => MyTexts.GetString(MyCommonTexts.Contract_Escort_GpsName) + this.Id.ToString();

    private MyGps PrepareGPS() => new MyGps()
    {
      DisplayName = MyTexts.GetString(MyCommonTexts.Contract_Escort_GpsName),
      Name = this.GetGpsName(),
      Description = MyTexts.GetString(MyCommonTexts.Contract_Escort_GpsDescription),
      Coords = this.StartPosition,
      ShowOnHud = true,
      DiscardAt = new TimeSpan?(),
      GPSColor = Color.DarkOrange,
      ContractId = this.Id
    };

    protected override void Share_Internal(long identityId)
    {
      base.Share_Internal(identityId);
      MyEntities.GetEntityById(this.GridId);
      MyGps gps = this.PrepareGPS();
      MySession.Static.Gpss.SendAddGps(identityId, ref gps, this.GridId);
    }

    private string GetTriggerName() => "Contract_Escort_Trig_" + (object) this.Id;

    protected void AttachTrigger(MyEntity entity)
    {
      MyAreaTriggerComponent triggerComponent = new MyAreaTriggerComponent(this.GetTriggerName());
      if (!entity.Components.Contains(typeof (MyTriggerAggregate)))
        entity.Components.Add(typeof (MyTriggerAggregate), (MyComponentBase) new MyTriggerAggregate());
      entity.Components.Get<MyTriggerAggregate>().AddComponent((MyComponentBase) triggerComponent);
      triggerComponent.Radius = this.TriggerRadius;
      triggerComponent.Center = this.EndPosition;
      triggerComponent.EntityEntered += new Action<long, string>(this.EntityEnteredTrigger);
    }

    private void EntityEnteredTrigger(long entityId, string entityName)
    {
      if (entityId != this.GridId)
        return;
      this.DestinationReached = true;
      int num = (int) this.Finish();
    }

    private void SubscribeToTrigger()
    {
      if (this.GridId == 0L)
        return;
      this.GetTrigger().EntityEntered += new Action<long, string>(this.EntityEnteredTrigger);
    }

    private void UnsubscribeFromTrigger()
    {
      if (this.GridId == 0L)
        return;
      MyAreaTriggerComponent trigger = this.GetTrigger();
      if (trigger == null)
        return;
      trigger.EntityEntered -= new Action<long, string>(this.EntityEnteredTrigger);
    }

    private MyAreaTriggerComponent GetTrigger()
    {
      if (this.TriggerEntityId == 0L)
        return (MyAreaTriggerComponent) null;
      MyEntity entityById = MyEntities.GetEntityById(this.TriggerEntityId);
      if (entityById == null)
        return (MyAreaTriggerComponent) null;
      if (!entityById.Components.Contains(typeof (MyTriggerAggregate)))
        return (MyAreaTriggerComponent) null;
      string triggerName = this.GetTriggerName();
      MyAggregateComponentList childList = entityById.Components.Get<MyTriggerAggregate>().ChildList;
      triggerComponent = (MyAreaTriggerComponent) null;
      foreach (MyComponentBase myComponentBase in childList.Reader)
      {
        if (myComponentBase is MyAreaTriggerComponent triggerComponent)
        {
          if (triggerComponent.Name == triggerName)
            break;
        }
        triggerComponent = (MyAreaTriggerComponent) null;
      }
      return triggerComponent ?? (MyAreaTriggerComponent) null;
    }

    protected override void FailFor_Internal(long player, bool abandon = false)
    {
      base.FailFor_Internal(player, abandon);
      this.RemoveGpsForPlayer(player);
    }

    protected override void FinishFor_Internal(long player, int rewardeeCount)
    {
      base.FinishFor_Internal(player, rewardeeCount);
      this.RemoveGpsForPlayer(player);
    }

    private void CloseDrones()
    {
      foreach (long drone in this.m_drones)
        MyEntities.GetEntityById(drone)?.Close();
    }

    protected override void CleanUp_Internal()
    {
      float num = 0.0f;
      this.UnsubscribePowerChange();
      this.UnsubscribeFromTrigger();
      foreach (long owner in this.Owners)
        this.RemoveGpsForPlayer(owner);
      MyEntity entityById = MyEntities.GetEntityById(this.GridId);
      if (entityById != null)
      {
        if (this.State == MyContractStateEnum.Finished)
        {
          this.CreateParticleEffectOnEntity("Warp", entityById.EntityId, true);
          num = 10f;
        }
        else
        {
          this.CreateParticleEffectOnEntity("Explosion_Warhead_50", entityById.EntityId, false);
          num = 2f;
        }
      }
      if (entityById is MyCubeGrid myCubeGrid)
      {
        MyRemoteControl firstBlockOfType = myCubeGrid.GetFirstBlockOfType<MyRemoteControl>();
        if (firstBlockOfType != null)
          firstBlockOfType.IsWorkingChanged -= new Action<MyCubeBlock>(this.RemoteDestroyedCallback);
      }
      this.CloseDrones();
      MyEntities.GetEntityById(this.TriggerEntityId)?.Close();
      this.m_disposeTime = num;
      this.m_isBeingDisposed = true;
      this.State = MyContractStateEnum.ToBeDisposed;
    }

    private void RemoveGpsForPlayer(long identityId)
    {
      MyGps gpsByContractId = MySession.Static.Gpss.GetGpsByContractId(identityId, this.Id);
      if (gpsByContractId == null)
        return;
      MySession.Static.Gpss.SendDelete(identityId, gpsByContractId.Hash);
    }

    private void UnsubscribePowerChange()
    {
      if (!(MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById))
        return;
      entityById.GridSystems.GridPowerStateChanged -= new Action<long, bool, string>(this.GridPowerStateChanged);
    }

    private void SubscribePowerChange()
    {
      if (!(MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById))
        return;
      entityById.GridSystems.GridPowerStateChanged += new Action<long, bool, string>(this.GridPowerStateChanged);
    }

    private void GridPowerStateChanged(long entityId, bool isPowered, string entityName)
    {
      if (this.State != MyContractStateEnum.Active || isPowered)
        return;
      this.Fail();
    }

    protected override bool CanPlayerReceiveReward(long identityId)
    {
      MyPlayer.PlayerId result;
      if (!MySession.Static.Players.TryGetPlayerId(identityId, out result))
        return false;
      MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
      if (playerById == null || playerById.Controller == null || playerById.Controller.ControlledEntity == null)
        return false;
      MyEntity controlledEntity = playerById.Controller.ControlledEntity as MyEntity;
      double num1 = this.RewardRadius + this.TriggerRadius;
      Vector3D vector3D = controlledEntity.PositionComp.WorldAABB.HalfExtents;
      double num2 = vector3D.Length();
      double num3 = num1 + num2;
      vector3D = controlledEntity.PositionComp.GetPosition() - this.EndPosition;
      return vector3D.LengthSquared() <= num3 * num3;
    }

    public override void Update(MyTimeSpan currentTime)
    {
      base.Update(currentTime);
      switch (this.State)
      {
        case MyContractStateEnum.Active:
          if (!(this.InnerTimer < currentTime))
            break;
          if (this.IsBehaviorAttached)
          {
            this.InnerTimer = currentTime + this.DroneAttackPeriod;
            this.SpawnDroneWave();
            break;
          }
          this.IsBehaviorAttached = this.AttachBehaviorToEscortShip();
          if (!this.IsBehaviorAttached)
            break;
          this.InnerTimer = currentTime + this.DroneFirstDelay;
          break;
        case MyContractStateEnum.ToBeDisposed:
          bool flag = false;
          if (this.m_isBeingDisposed)
          {
            if (!this.DisposeTime.HasValue)
              this.DisposeTime = new MyTimeSpan?(currentTime + MyTimeSpan.FromSeconds((double) this.m_disposeTime));
            if (this.DisposeTime.Value <= currentTime)
              flag = true;
          }
          else
            flag = true;
          if (!flag)
            break;
          this.State = MyContractStateEnum.Disposed;
          if (!(MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById))
            break;
          entityById.DismountAllCockpits();
          entityById.Close();
          break;
      }
    }

    private bool AttachBehaviorToEscortShip()
    {
      if (!(MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById))
        return false;
      MyRemoteControl firstBlockOfType = entityById.GetFirstBlockOfType<MyRemoteControl>();
      if (firstBlockOfType == null)
        return false;
      firstBlockOfType.AddWaypoint(new MyWaypointInfo("endWaypoint", this.EndPosition));
      firstBlockOfType.SetCollisionAvoidance(true);
      firstBlockOfType.ChangeDirection(Base6Directions.Direction.Forward);
      firstBlockOfType.SetAutoPilotSpeedLimit(MyContractEscort.DRONE_SPEED_LIMIT);
      firstBlockOfType.SetWaypointThresholdDistance(1f);
      firstBlockOfType.ChangeFlightMode(FlightMode.OneWay);
      firstBlockOfType.SetWaitForFreeWay(true);
      firstBlockOfType.SetAutoPilotEnabled(true);
      return true;
    }

    private void RemoteDestroyedCallback(MyCubeBlock obj)
    {
      if (this.State != MyContractStateEnum.Active || obj.IsWorking)
        return;
      this.Fail();
      obj.IsWorkingChanged -= new Action<MyCubeBlock>(this.RemoteDestroyedCallback);
    }

    protected override bool NeedsUpdate_Internal() => true;

    private void SpawnDroneWave()
    {
      MyEntity entityById = MyEntities.GetEntityById(this.GridId);
      if (entityById == null || !(MySession.Static.Factions.TryGetFactionById(this.WaveFactionId) is MyFaction factionById))
        return;
      Vector3D position = entityById.PositionComp.GetPosition();
      Vector3D vector3D = Vector3D.Normalize(this.EndPosition - position);
      Vector3D center = position + vector3D * (double) MyContractEscort.SPAWN_AHEAD_DISTANCE;
      MyContractTypeEscortDefinition definition = this.GetDefinition() as MyContractTypeEscortDefinition;
      if (this.m_drones.Count >= MyContractEscort.DRONE_MAX_COUNT)
        return;
      for (int index = 0; index < this.DronesPerWave; ++index)
      {
        string str = "Barb_mk.1";
        if (definition != null && definition.PrefabsAttackDrones != null && definition.PrefabsAttackDrones.Count > 0)
          str = definition.PrefabsAttackDrones[MyRandom.Instance.Next(0, definition.PrefabsAttackDrones.Count)];
        string behaviorName = definition.DroneBehaviours[MyRandom.Instance.Next(0, definition.DroneBehaviours.Count)];
        Vector3D? freePlace = MyEntities.FindFreePlace(new BoundingSphereD(center, (double) MyContractEscort.SPAWN_RANDOMIZATION_RADIUS).RandomToUniformPointInSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble()), MyContractEscort.SIZE_OF_DRONES);
        if (freePlace.HasValue)
        {
          MySpawnPrefabProperties spawnProperties = new MySpawnPrefabProperties()
          {
            Position = freePlace.Value,
            Forward = (Vector3) -vector3D,
            Up = Vector3.CalculatePerpendicularVector((Vector3) vector3D),
            PrefabName = str,
            OwnerId = factionById.FounderId,
            Color = factionById.CustomColor,
            SpawningOptions = SpawningOptions.SetAuthorship | SpawningOptions.ReplaceColor | SpawningOptions.UseOnlyWorldMatrix,
            UpdateSync = true
          };
          List<DroneTarget> targets = new List<DroneTarget>();
          targets.Add(new DroneTarget(entityById, 30));
          MyPrefabManager.Static.SpawnPrefabInternal(spawnProperties, (Action) (() =>
          {
            if (spawnProperties.ResultList == null || spawnProperties.ResultList.Count == 0 || spawnProperties.ResultList.Count > 1)
              return;
            MyCubeGrid result = spawnProperties.ResultList[0];
            MyRemoteControl firstBlockOfType = result.GetFirstBlockOfType<MyRemoteControl>();
            if (firstBlockOfType == null)
              return;
            MyDroneAI myDroneAi = new MyDroneAI(firstBlockOfType, behaviorName, true, (List<MyEntity>) null, targets, 0, TargetPrioritization.PriorityRandom, 10000f, false);
            firstBlockOfType.SetAutomaticBehaviour((MyRemoteControl.IRemoteControlAutomaticBehaviour) myDroneAi);
            firstBlockOfType.SetAutoPilotEnabled(true);
            this.m_drones.Add(result.EntityId);
          }));
        }
      }
    }

    public override bool CanBeFinished_Internal() => base.CanBeFinished_Internal() && this.DestinationReached;

    public override string ToDebugString() => new StringBuilder(base.ToDebugString()).ToString();
  }
}
