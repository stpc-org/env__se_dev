// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyDroneAI
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.AI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
  public class MyDroneAI : MyRemoteControl.IRemoteControlAutomaticBehaviour
  {
    private MyRemoteControl m_remoteControl;
    private MyEntity3DSoundEmitter m_soundEmitter;
    private bool m_resetSound;
    private int m_frameCounter;
    public float m_maxPlayerDistance;
    public float m_maxPlayerDistanceSq;
    private bool m_rotateToTarget = true;
    private bool m_canRotateToTarget = true;
    private MyDroneAIData m_currentPreset;
    private bool m_avoidCollisions;
    private bool m_alternativebehaviorSwitched;
    private int m_waypointDelayMs;
    private int m_waypointReachedTimeMs;
    private Vector3D m_returnPosition;
    private int m_lostStartTimeMs;
    private int m_waypointStartTimeMs;
    private int m_lastTargetUpdate;
    private int m_lastWeaponUpdate;
    private bool m_farAwayFromTarget;
    private MyEntity m_currentTarget;
    private List<MyUserControllableGun> m_weapons = new List<MyUserControllableGun>();
    private List<MyFunctionalBlock> m_tools = new List<MyFunctionalBlock>();
    private bool m_shooting;
    private bool m_operational = true;
    private bool m_canSkipWaypoint = true;
    private bool m_cycleWaypoints;
    private List<MyEntity> m_forcedWaypoints = new List<MyEntity>();
    private List<DroneTarget> m_targetsList = new List<DroneTarget>();
    private List<DroneTarget> m_targetsFiltered = new List<DroneTarget>();
    private TargetPrioritization m_prioritizationStyle = TargetPrioritization.PriorityRandom;
    public bool m_loadItems = true;
    private bool m_loadEntities;
    private long m_loadCurrentTarget;
    private List<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable> m_loadTargetList;
    private List<long> m_loadWaypointList;
    private MyWeaponBehavior m_currentWeaponBehavior;
    private List<float> m_weaponBehaviorTimes = new List<float>();
    private List<int> m_weaponBehaviorAssignedRules = new List<int>();
    private List<bool> m_weaponBehaviorWeaponLock = new List<bool>();
    private float m_weaponBehaviorCooldown;
    private bool m_weaponBehaviorActive;

    public bool NeedUpdate { get; private set; }

    public bool IsActive { get; private set; }

    public bool RotateToTarget
    {
      get => this.m_canRotateToTarget && this.m_rotateToTarget;
      set => this.m_rotateToTarget = value;
    }

    public bool CollisionAvoidance
    {
      get => this.m_avoidCollisions;
      set => this.m_avoidCollisions = value;
    }

    public Vector3D OriginPoint
    {
      get => this.m_returnPosition;
      set => this.m_returnPosition = value;
    }

    public int PlayerPriority { get; set; }

    public TargetPrioritization PrioritizationStyle
    {
      get => this.m_prioritizationStyle;
      set => this.m_prioritizationStyle = value;
    }

    public MyEntity CurrentTarget
    {
      get => this.m_currentTarget;
      set => this.m_currentTarget = value;
    }

    public string CurrentBehavior => this.m_currentPreset == null ? "" : this.m_currentPreset.Name;

    public List<DroneTarget> TargetList => this.m_targetsFiltered;

    public List<MyEntity> WaypointList => this.m_forcedWaypoints;

    public bool WaypointActive => !this.m_canSkipWaypoint;

    public bool Ambushing { get; set; }

    public bool Operational => this.m_operational;

    public float SpeedLimit { get; set; }

    public float MaxPlayerDistance
    {
      get => this.m_maxPlayerDistance;
      private set
      {
        this.m_maxPlayerDistance = value;
        this.m_maxPlayerDistanceSq = value * value;
      }
    }

    public float PlayerYAxisOffset { get; private set; }

    public float WaypointThresholdDistance { get; private set; }

    public bool ResetStuckDetection => this.IsActive;

    public bool CycleWaypoints
    {
      get => this.m_cycleWaypoints;
      set => this.m_cycleWaypoints = value;
    }

    public MyDroneAI()
    {
    }

    public MyDroneAI(
      MyRemoteControl remoteControl,
      string presetName,
      bool activate,
      List<MyEntity> waypoints,
      List<DroneTarget> targets,
      int playerPriority,
      TargetPrioritization prioritizationStyle,
      float maxPlayerDistance,
      bool cycleWaypoints)
    {
      this.m_remoteControl = remoteControl;
      this.m_returnPosition = this.m_remoteControl.PositionComp.GetPosition();
      this.m_currentPreset = MyDroneAIDataStatic.LoadPreset(presetName);
      this.Ambushing = false;
      this.LoadDroneAIData();
      this.m_lastTargetUpdate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_lastWeaponUpdate = this.m_lastTargetUpdate;
      this.m_waypointReachedTimeMs = this.m_lastTargetUpdate;
      this.m_forcedWaypoints = waypoints ?? new List<MyEntity>();
      this.m_targetsList = targets ?? new List<DroneTarget>();
      this.PlayerPriority = playerPriority;
      this.m_prioritizationStyle = prioritizationStyle;
      this.MaxPlayerDistance = maxPlayerDistance;
      this.m_cycleWaypoints = cycleWaypoints;
      this.NeedUpdate = activate;
    }

    private void LoadDroneAIData()
    {
      if (this.m_currentPreset == null)
        return;
      this.m_avoidCollisions = this.m_currentPreset.AvoidCollisions;
      this.m_rotateToTarget = this.m_currentPreset.RotateToPlayer;
      this.PlayerYAxisOffset = this.m_currentPreset.PlayerYAxisOffset;
      this.WaypointThresholdDistance = this.m_currentPreset.WaypointThresholdDistance;
      this.SpeedLimit = this.m_currentPreset.SpeedLimit;
      if (string.IsNullOrEmpty(this.m_currentPreset.SoundLoop))
      {
        if (this.m_soundEmitter == null)
          return;
        this.m_soundEmitter.StopSound(true);
      }
      else
      {
        if (this.m_soundEmitter == null)
          this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this.m_remoteControl, true);
        MySoundPair soundId = new MySoundPair(this.m_currentPreset.SoundLoop);
        if (soundId == MySoundPair.Empty)
          return;
        this.m_soundEmitter.PlaySound(soundId, true);
      }
    }

    public void Load(
      MyObjectBuilder_AutomaticBehaviour objectBuilder,
      MyRemoteControl remoteControl)
    {
      if (!(objectBuilder is MyObjectBuilder_DroneAI objectBuilderDroneAi))
        return;
      this.m_remoteControl = remoteControl;
      this.m_currentPreset = MyDroneAIDataStatic.LoadPreset(objectBuilderDroneAi.CurrentPreset);
      this.LoadDroneAIData();
      this.m_lastTargetUpdate = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_lastWeaponUpdate = this.m_lastTargetUpdate;
      this.m_waypointReachedTimeMs = this.m_lastTargetUpdate;
      this.m_forcedWaypoints = new List<MyEntity>();
      this.m_loadWaypointList = objectBuilderDroneAi.WaypointList;
      this.m_targetsList = new List<DroneTarget>();
      this.m_loadTargetList = objectBuilderDroneAi.TargetList;
      this.m_currentTarget = (MyEntity) null;
      this.m_loadCurrentTarget = objectBuilderDroneAi.CurrentTarget;
      this.Ambushing = objectBuilderDroneAi.InAmbushMode;
      this.m_returnPosition = (Vector3D) objectBuilderDroneAi.ReturnPosition;
      this.PlayerPriority = objectBuilderDroneAi.PlayerPriority;
      this.m_prioritizationStyle = objectBuilderDroneAi.PrioritizationStyle;
      this.MaxPlayerDistance = objectBuilderDroneAi.MaxPlayerDistance;
      this.m_cycleWaypoints = objectBuilderDroneAi.CycleWaypoints;
      this.m_alternativebehaviorSwitched = objectBuilderDroneAi.AlternativebehaviorSwitched;
      this.CollisionAvoidance = objectBuilderDroneAi.CollisionAvoidance;
      this.m_canSkipWaypoint = objectBuilderDroneAi.CanSkipWaypoint;
      if ((double) objectBuilderDroneAi.SpeedLimit != -3.40282346638529E+38)
        this.SpeedLimit = objectBuilderDroneAi.SpeedLimit;
      this.NeedUpdate = objectBuilderDroneAi.NeedUpdate;
      this.IsActive = objectBuilderDroneAi.IsActive;
      this.m_loadEntities = true;
    }

    public MyObjectBuilder_AutomaticBehaviour GetObjectBuilder()
    {
      MyObjectBuilder_DroneAI objectBuilderDroneAi = new MyObjectBuilder_DroneAI();
      objectBuilderDroneAi.CollisionAvoidance = this.CollisionAvoidance;
      objectBuilderDroneAi.CurrentTarget = this.m_currentTarget != null ? this.m_currentTarget.EntityId : 0L;
      objectBuilderDroneAi.CycleWaypoints = this.m_cycleWaypoints;
      objectBuilderDroneAi.IsActive = this.IsActive;
      objectBuilderDroneAi.MaxPlayerDistance = this.m_maxPlayerDistance;
      objectBuilderDroneAi.NeedUpdate = this.NeedUpdate;
      objectBuilderDroneAi.InAmbushMode = this.Ambushing;
      objectBuilderDroneAi.PlayerPriority = this.PlayerPriority;
      objectBuilderDroneAi.PrioritizationStyle = this.m_prioritizationStyle;
      objectBuilderDroneAi.TargetList = new List<MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable>();
      foreach (DroneTarget targets in this.m_targetsList)
      {
        if (targets.Target != null)
          objectBuilderDroneAi.TargetList.Add(new MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable(targets.Target.EntityId, targets.Priority));
      }
      objectBuilderDroneAi.WaypointList = new List<long>();
      foreach (MyEntity forcedWaypoint in this.m_forcedWaypoints)
      {
        if (forcedWaypoint != null)
          objectBuilderDroneAi.WaypointList.Add(forcedWaypoint.EntityId);
      }
      objectBuilderDroneAi.CurrentPreset = this.m_currentPreset.Name;
      objectBuilderDroneAi.AlternativebehaviorSwitched = this.m_alternativebehaviorSwitched;
      objectBuilderDroneAi.ReturnPosition = (SerializableVector3D) this.m_returnPosition;
      objectBuilderDroneAi.CanSkipWaypoint = this.m_canSkipWaypoint;
      objectBuilderDroneAi.SpeedLimit = this.SpeedLimit;
      return (MyObjectBuilder_AutomaticBehaviour) objectBuilderDroneAi;
    }

    public void LoadShipGear()
    {
      this.m_loadItems = false;
      this.m_remoteControl.CubeGrid.GetBlocks();
      this.m_weapons = new List<MyUserControllableGun>();
      this.m_tools = new List<MyFunctionalBlock>();
      foreach (MyCubeGrid groupNode in MyCubeGridGroups.Static.Logical.GetGroupNodes(this.m_remoteControl.CubeGrid))
      {
        foreach (MySlimBlock block in groupNode.GetBlocks())
        {
          if (block.FatBlock is MyUserControllableGun)
            this.m_weapons.Add(block.FatBlock as MyUserControllableGun);
          if (block.FatBlock is MyShipToolBase)
            this.m_tools.Add(block.FatBlock as MyFunctionalBlock);
          if (block.FatBlock is MyShipDrill)
            this.m_tools.Add(block.FatBlock as MyFunctionalBlock);
        }
      }
    }

    public void LoadEntities()
    {
      this.m_loadEntities = false;
      if (this.m_loadWaypointList != null)
      {
        foreach (long loadWaypoint in this.m_loadWaypointList)
        {
          MyEntity entity;
          if (loadWaypoint > 0L && Sandbox.Game.Entities.MyEntities.TryGetEntityById(loadWaypoint, out entity))
            this.m_forcedWaypoints.Add(entity);
        }
        this.m_loadWaypointList.Clear();
      }
      if (this.m_loadTargetList != null)
      {
        foreach (MyObjectBuilder_AutomaticBehaviour.DroneTargetSerializable loadTarget in this.m_loadTargetList)
        {
          MyEntity entity;
          if (loadTarget.TargetId > 0L && Sandbox.Game.Entities.MyEntities.TryGetEntityById(loadTarget.TargetId, out entity))
            this.m_targetsList.Add(new DroneTarget(entity, loadTarget.Priority));
        }
        this.m_targetsList.Clear();
      }
      if (this.m_loadCurrentTarget <= 0L)
        return;
      MyEntity entity1;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById(this.m_loadCurrentTarget, out entity1);
      this.m_currentTarget = entity1;
    }

    public void StopWorking()
    {
      if (this.m_soundEmitter == null || !this.m_soundEmitter.IsPlaying)
        return;
      this.m_soundEmitter.StopSound(false);
      this.m_resetSound = true;
    }

    public void Update()
    {
      ++this.m_frameCounter;
      if (this.m_resetSound)
      {
        MySoundPair soundId = new MySoundPair(this.m_currentPreset.SoundLoop);
        if (soundId != MySoundPair.Empty)
          this.m_soundEmitter.PlaySound(soundId, true);
        this.m_resetSound = false;
      }
      if (this.m_soundEmitter != null && this.m_frameCounter % 100 == 0)
        this.m_soundEmitter.Update();
      if (!Sync.IsServer)
        return;
      if (this.m_loadItems)
        this.LoadShipGear();
      if (this.m_loadEntities)
        this.LoadEntities();
      if (!this.IsActive && !this.NeedUpdate)
        return;
      this.UpdateWaypoint();
    }

    private void UpdateWaypoint()
    {
      int timeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (this.m_currentTarget != null && timeInMilliseconds - this.m_lastTargetUpdate >= 1000)
      {
        this.m_lastTargetUpdate = timeInMilliseconds;
        if (!this.IsValidTarget(this.m_currentTarget))
          this.m_currentTarget = (MyEntity) null;
      }
      if (this.m_currentTarget == null && timeInMilliseconds - this.m_lastTargetUpdate >= 1000 || timeInMilliseconds - this.m_lostStartTimeMs >= this.m_currentPreset.LostTimeMs)
      {
        this.FindNewTarget();
        this.m_lastTargetUpdate = timeInMilliseconds;
        if (this.m_currentTarget != null)
        {
          this.m_lostStartTimeMs = timeInMilliseconds;
          this.m_farAwayFromTarget = true;
        }
      }
      if (!this.Ambushing && this.m_farAwayFromTarget && (timeInMilliseconds - this.m_lastTargetUpdate >= 5000 && this.m_canSkipWaypoint))
      {
        this.m_lastTargetUpdate = timeInMilliseconds;
        this.NeedUpdate = true;
      }
      float distSq = -1f;
      if ((double) this.m_weaponBehaviorCooldown > 0.0)
        this.m_weaponBehaviorCooldown -= 0.01666667f;
      if (this.m_operational && timeInMilliseconds - this.m_lastWeaponUpdate >= 300)
      {
        this.m_lastWeaponUpdate = timeInMilliseconds;
        distSq = this.m_currentTarget != null ? Vector3.DistanceSquared((Vector3) this.m_currentTarget.PositionComp.GetPosition(), (Vector3) this.m_remoteControl.PositionComp.GetPosition()) : -1f;
        if (!this.m_currentPreset.UsesWeaponBehaviors || (double) this.m_weaponBehaviorCooldown <= 0.0)
          this.WeaponsUpdate(distSq);
        this.m_canRotateToTarget = (double) distSq < (double) this.m_currentPreset.RotationLimitSq && (double) distSq >= 0.0;
      }
      if (!this.m_operational || this.m_shooting)
        this.m_lostStartTimeMs = timeInMilliseconds;
      if (!this.Ambushing && timeInMilliseconds - this.m_waypointReachedTimeMs >= this.m_currentPreset.WaypointMaxTime && this.m_canSkipWaypoint)
        this.NeedUpdate = true;
      if (!this.Ambushing && this.m_remoteControl.CurrentWaypoint == null && this.WaypointList.Count > 0)
        this.NeedUpdate = true;
      if (!this.NeedUpdate)
        return;
      this.IsActive = true;
      if ((double) distSq < 0.0 && this.m_currentTarget != null)
        distSq = Vector3.DistanceSquared((Vector3) this.m_currentTarget.PositionComp.GetPosition(), (Vector3) this.m_remoteControl.PositionComp.GetPosition());
      this.m_farAwayFromTarget = (double) distSq > (double) this.m_currentPreset.MaxManeuverDistanceSq;
      this.m_canRotateToTarget = (double) distSq < (double) this.m_currentPreset.RotationLimitSq && (double) distSq >= 0.0;
      bool needUpdate = this.NeedUpdate;
      if (this.m_remoteControl.HasWaypoints())
        this.m_remoteControl.ClearWaypoints();
      this.m_remoteControl.SetAutoPilotEnabled(true);
      this.NeedUpdate = needUpdate;
      this.m_canSkipWaypoint = true;
      string name = "Player Vicinity";
      Vector3D pos;
      MatrixD worldMatrix;
      if (this.m_forcedWaypoints.Count > 0)
      {
        if (this.m_cycleWaypoints)
          this.m_forcedWaypoints.Add(this.m_forcedWaypoints[0]);
        pos = this.m_forcedWaypoints[0].PositionComp.GetPosition();
        name = this.m_forcedWaypoints[0].Name;
        this.m_forcedWaypoints.RemoveAt(0);
        this.m_canSkipWaypoint = false;
      }
      else if (this.m_currentTarget == null)
      {
        worldMatrix = this.m_remoteControl.WorldMatrix;
        pos = worldMatrix.Translation + Vector3.One * 0.01f;
      }
      else if (!this.m_operational && this.m_currentPreset.UseKamikazeBehavior)
      {
        if (this.m_remoteControl.TargettingAimDelta > 0.0199999995529652)
          return;
        pos = this.m_currentTarget.PositionComp.GetPosition() + this.m_currentTarget.WorldMatrix.Up * (double) this.PlayerYAxisOffset * 2.0 - Vector3D.Normalize(this.m_remoteControl.PositionComp.GetPosition() - this.m_currentTarget.PositionComp.GetPosition()) * (double) this.m_currentPreset.KamikazeBehaviorDistance;
      }
      else if (!this.m_operational && !this.m_currentPreset.UseKamikazeBehavior)
        pos = this.m_returnPosition + Vector3.One * 0.01f;
      else if (this.m_farAwayFromTarget)
      {
        pos = this.m_currentTarget.PositionComp.GetPosition() + Vector3D.Normalize(this.m_remoteControl.PositionComp.GetPosition() - this.m_currentTarget.PositionComp.GetPosition()) * (double) this.m_currentPreset.PlayerTargetDistance;
        if (this.m_currentPreset.UsePlanetHover)
          this.HoverMechanic(ref pos);
      }
      else
      {
        if (timeInMilliseconds - this.m_waypointReachedTimeMs <= this.m_waypointDelayMs)
          return;
        pos = this.GetRandomPoint();
        name = "Strafe";
        if (this.m_currentPreset.UsePlanetHover)
          this.HoverMechanic(ref pos);
      }
      Vector3D vector3D = pos;
      worldMatrix = this.m_remoteControl.WorldMatrix;
      Vector3D translation = worldMatrix.Translation;
      (vector3D - translation).Normalize();
      this.m_waypointReachedTimeMs = timeInMilliseconds;
      bool flag = this.m_currentPreset.UseKamikazeBehavior && !this.m_operational;
      this.m_remoteControl.ChangeFlightMode(FlightMode.OneWay);
      this.m_remoteControl.SetAutoPilotSpeedLimit(flag ? 100f : this.SpeedLimit);
      this.m_remoteControl.SetCollisionAvoidance(!flag && this.m_canSkipWaypoint && this.m_avoidCollisions);
      this.m_remoteControl.ChangeDirection(Base6Directions.Direction.Forward);
      this.m_remoteControl.AddWaypoint(pos, name);
      this.NeedUpdate = false;
      this.IsActive = true;
    }

    public void DebugDraw()
    {
      if (this.m_remoteControl.CurrentWaypoint != null)
        MyRenderProxy.DebugDrawSphere((Vector3D) (Vector3) this.m_remoteControl.CurrentWaypoint.Coords, 0.5f, Color.Aquamarine);
      if (this.m_currentTarget == null)
        return;
      MyRenderProxy.DebugDrawSphere((Vector3D) (Vector3) this.m_currentTarget.PositionComp.GetPosition(), 2f, this.m_canRotateToTarget ? Color.Green : Color.Red);
    }

    private void HoverMechanic(ref Vector3D pos)
    {
      Vector3 naturalGravityInPoint = MyGravityProviderSystem.CalculateNaturalGravityInPoint(pos);
      if ((double) naturalGravityInPoint.LengthSquared() <= 0.0)
        return;
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(pos);
      if (closestPlanet == null)
        return;
      Vector3D surfacePointGlobal = closestPlanet.GetClosestSurfacePointGlobal(ref pos);
      float num = (float) Vector3D.Distance(surfacePointGlobal, pos);
      if (Vector3D.DistanceSquared(closestPlanet.PositionComp.GetPosition(), surfacePointGlobal) > Vector3D.DistanceSquared(closestPlanet.PositionComp.GetPosition(), pos))
        num *= -1f;
      if ((double) num < (double) this.m_currentPreset.PlanetHoverMin)
      {
        pos = surfacePointGlobal - Vector3D.Normalize((Vector3D) naturalGravityInPoint) * (double) this.m_currentPreset.PlanetHoverMin;
      }
      else
      {
        if ((double) num <= (double) this.m_currentPreset.PlanetHoverMax)
          return;
        pos = surfacePointGlobal - Vector3D.Normalize((Vector3D) naturalGravityInPoint) * (double) this.m_currentPreset.PlanetHoverMax;
      }
    }

    private Vector3D GetRandomPoint()
    {
      int num = 0;
      MatrixD fromDir = MatrixD.CreateFromDir(Vector3D.Normalize(this.m_currentTarget.PositionComp.GetPosition() - this.m_remoteControl.PositionComp.GetPosition()));
      Vector3D vector3D1;
      do
      {
        Vector3D vector3D2 = fromDir.Right * (double) MyUtils.GetRandomFloat(-this.m_currentPreset.Width, this.m_currentPreset.Width);
        Vector3D vector3D3 = fromDir.Up * (double) MyUtils.GetRandomFloat(-this.m_currentPreset.Height, this.m_currentPreset.Height);
        Vector3D vector3D4 = fromDir.Forward * (double) MyUtils.GetRandomFloat(-this.m_currentPreset.Depth, this.m_currentPreset.Depth);
        vector3D1 = this.m_remoteControl.PositionComp.GetPosition() + vector3D2 + vector3D3 + vector3D4;
      }
      while ((vector3D1 - this.m_remoteControl.PositionComp.GetPosition()).LengthSquared() <= (double) this.m_currentPreset.MinStrafeDistanceSq && ++num < 10);
      return vector3D1;
    }

    private bool IsValidTarget(MyEntity target)
    {
      switch (target)
      {
        case MyCharacter _ when !((MyCharacter) target).IsDead:
          return true;
        case MyCubeBlock _:
          return ((MyCubeBlock) target).IsFunctional;
        case MyCubeGrid _:
          return ((MyCubeGrid) target).IsPowered;
        default:
          return false;
      }
    }

    private bool FindNewTarget()
    {
      List<DroneTarget> droneTargetList = new List<DroneTarget>();
      if (this.PlayerPriority > 0)
      {
        long ownerId = this.m_remoteControl.OwnerId;
        foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) MySession.Static.Players.GetOnlinePlayers())
        {
          if (MyIDModule.GetRelationPlayerPlayer(ownerId, onlinePlayer.Identity.IdentityId) == MyRelationsBetweenPlayers.Enemies)
          {
            IMyControllableEntity controlledEntity = onlinePlayer.Controller.ControlledEntity;
            if (controlledEntity != null && (!(controlledEntity is MyCharacter) || !((MyCharacter) controlledEntity).IsDead))
            {
              Vector3D translation = controlledEntity.Entity.WorldMatrix.Translation;
              if (Vector3D.DistanceSquared(this.m_remoteControl.PositionComp.GetPosition(), translation) < (double) this.m_maxPlayerDistanceSq)
                droneTargetList.Add(new DroneTarget((MyEntity) controlledEntity, this.PlayerPriority));
            }
          }
        }
      }
      for (int index1 = 0; index1 < this.m_targetsList.Count; ++index1)
      {
        if (this.IsValidTarget(this.m_targetsList[index1].Target))
        {
          if (this.m_targetsList[index1].Target is MyCubeGrid target)
          {
            ListReader<MyCubeBlock> fatBlocks = target.GetFatBlocks();
            if (fatBlocks.Count > 0)
            {
              int index2 = MyRandom.Instance.Next(0, fatBlocks.Count);
              MyCubeBlock myCubeBlock = fatBlocks[index2];
              if (this.IsValidTarget((MyEntity) myCubeBlock))
              {
                DroneTarget droneTarget = new DroneTarget((MyEntity) myCubeBlock, this.m_targetsList[index1].Priority);
                droneTargetList.Add(droneTarget);
              }
            }
          }
          else
            droneTargetList.Add(this.m_targetsList[index1]);
        }
      }
      this.m_targetsFiltered.Clear();
      this.m_targetsFiltered = droneTargetList;
      if (droneTargetList.Count == 0)
        return false;
      bool flag = this.m_prioritizationStyle == TargetPrioritization.Random;
      switch (this.m_prioritizationStyle)
      {
        case TargetPrioritization.ClosestFirst:
          double num1 = double.MaxValue;
          foreach (DroneTarget droneTarget in droneTargetList)
          {
            double num2 = Vector3D.DistanceSquared(this.m_remoteControl.PositionComp.GetPosition(), droneTarget.Target.PositionComp.GetPosition());
            if (num2 < num1)
            {
              num1 = num2;
              this.m_currentTarget = droneTarget.Target;
            }
          }
          return true;
        case TargetPrioritization.PriorityRandom:
        case TargetPrioritization.Random:
          int maxValue = 0;
          foreach (DroneTarget droneTarget in droneTargetList)
            maxValue += flag ? 1 : Math.Max(0, droneTarget.Priority);
          int num3 = MyUtils.GetRandomInt(0, maxValue) + 1;
          foreach (DroneTarget droneTarget in droneTargetList)
          {
            int num2 = flag ? 1 : Math.Max(0, droneTarget.Priority);
            if (num3 <= num2)
            {
              this.m_currentTarget = droneTarget.Target;
              break;
            }
            num3 -= num2;
          }
          return true;
        default:
          droneTargetList.Sort();
          this.m_currentTarget = droneTargetList[droneTargetList.Count - 1].Target;
          return true;
      }
    }

    public void TargetAdd(DroneTarget target)
    {
      if (this.m_targetsList.Contains(target))
        return;
      this.m_targetsList.Add(target);
    }

    public void TargetClear() => this.m_targetsList.Clear();

    public void TargetLoseCurrent() => this.m_currentTarget = (MyEntity) null;

    public void TargetRemove(MyEntity target)
    {
      for (int index = 0; index < this.m_targetsList.Count; ++index)
      {
        if (this.m_targetsList[index].Target == target)
        {
          this.m_targetsList.RemoveAt(index);
          --index;
        }
      }
    }

    public void WaypointAdd(MyEntity target)
    {
      if (target == null || this.m_forcedWaypoints.Contains(target))
        return;
      this.m_forcedWaypoints.Add(target);
    }

    public void WaypointClear() => this.m_forcedWaypoints.Clear();

    public void WaypointAdvanced()
    {
      if (!Sync.IsServer)
        return;
      this.m_waypointReachedTimeMs = MySandboxGame.TotalGamePlayTimeInMilliseconds + MyUtils.GetRandomInt(this.m_currentPreset.WaypointDelayMsMin, this.m_currentPreset.WaypointDelayMsMax);
      if (this.Ambushing || !this.IsActive || this.m_remoteControl.CurrentWaypoint == null && this.m_targetsFiltered.Count <= 0 && this.m_forcedWaypoints.Count <= 0)
        return;
      this.NeedUpdate = true;
    }

    private void RaycastCheck(Vector3 from, out bool hitVoxel, out bool hitGrid)
    {
      hitVoxel = false;
      hitGrid = false;
      Vector3 vector3 = (Vector3) this.m_currentTarget.WorldMatrix.Translation;
      if (this.m_currentTarget is MyCharacter)
        vector3 = (Vector3) (vector3 + this.m_currentTarget.WorldMatrix.Up * (double) this.PlayerYAxisOffset);
      MyPhysics.HitInfo? nullable = MyPhysics.CastRay((Vector3D) from, (Vector3D) vector3, 15);
      IMyEntity myEntity = (IMyEntity) null;
      if (nullable.HasValue && (HkReferenceObject) nullable.Value.HkHitInfo.Body != (HkReferenceObject) null && (nullable.Value.HkHitInfo.Body.UserObject != null && nullable.Value.HkHitInfo.Body.UserObject is MyPhysicsBody))
        myEntity = ((MyPhysicsComponentBase) nullable.Value.HkHitInfo.Body.UserObject).Entity;
      if (myEntity == null || this.m_currentTarget == myEntity || this.m_currentTarget.Parent == myEntity || this.m_currentTarget.Parent != null && this.m_currentTarget.Parent == myEntity.Parent)
        return;
      switch (myEntity)
      {
        case MyMissile _:
          break;
        case MyFloatingObject _:
          break;
        case MyVoxelBase _:
          hitVoxel = true;
          break;
        default:
          hitGrid = true;
          break;
      }
    }

    private void ChangeWeaponBehavior()
    {
      this.m_currentWeaponBehavior = (MyWeaponBehavior) null;
      this.m_weaponBehaviorTimes.Clear();
      this.m_weaponBehaviorAssignedRules.Clear();
      this.m_weaponBehaviorWeaponLock.Clear();
      if (this.m_currentTarget == null)
      {
        this.m_weaponBehaviorCooldown = this.m_currentPreset.WeaponBehaviorNotFoundDelay;
      }
      else
      {
        List<int> intList = new List<int>();
        int maxValue = 0;
        bool hitVoxel = false;
        bool hitGrid = false;
        this.RaycastCheck((Vector3) ((double) this.m_remoteControl.CubeGrid.PositionComp.LocalVolume.Radius * this.m_remoteControl.CubeGrid.WorldMatrix.Forward * 1.10000002384186 + this.m_remoteControl.CubeGrid.PositionComp.WorldAABB.Center), out hitVoxel, out hitGrid);
        foreach (MyWeaponBehavior weaponBehavior in this.m_currentPreset.WeaponBehaviors)
        {
          bool flag1 = true;
          if (!weaponBehavior.IgnoresVoxels & hitVoxel)
            flag1 = false;
          if (!weaponBehavior.IgnoresGrids & hitGrid)
            flag1 = false;
          bool flag2 = false;
          if (flag1 && weaponBehavior.WeaponRules.Count > 0)
          {
            if (weaponBehavior.RequirementsIsWhitelist || weaponBehavior.Requirements.Count > 0)
            {
              foreach (MyUserControllableGun weapon in this.m_weapons)
              {
                if (weapon.Enabled && weapon.IsFunctional && weapon.IsStationary())
                {
                  flag2 = weaponBehavior.Requirements.Contains(weapon.BlockDefinition.Id.TypeId.ToString());
                  if (!weaponBehavior.RequirementsIsWhitelist)
                    flag2 = !flag2;
                  if (flag2)
                    break;
                }
              }
            }
            else
              flag2 = true;
          }
          if (flag2 & flag1 && weaponBehavior.WeaponRules.Count > 0)
          {
            int num = Math.Max(0, weaponBehavior.Priority);
            intList.Add(num);
            maxValue += num;
          }
          else
            intList.Add(0);
        }
        if (maxValue > 0)
        {
          int num = MyUtils.GetRandomInt(0, maxValue) + 1;
          for (int index = 0; index < intList.Count; ++index)
          {
            if (num <= intList[index])
            {
              this.m_currentWeaponBehavior = this.m_currentPreset.WeaponBehaviors[index];
              break;
            }
            num -= intList[index];
          }
          if (this.m_currentWeaponBehavior != null)
          {
            foreach (MyWeaponRule weaponRule in this.m_currentWeaponBehavior.WeaponRules)
              this.m_weaponBehaviorTimes.Add(-1f);
            foreach (MyUserControllableGun weapon1 in this.m_weapons)
            {
              this.m_weaponBehaviorWeaponLock.Add(false);
              bool flag1 = false;
              MyObjectBuilderType typeId;
              if (this.m_currentWeaponBehavior.RequirementsIsWhitelist || this.m_currentWeaponBehavior.Requirements.Count > 0)
              {
                List<string> requirements = this.m_currentWeaponBehavior.Requirements;
                typeId = weapon1.BlockDefinition.Id.TypeId;
                string str = typeId.ToString();
                bool flag2 = requirements.Contains(str);
                if (!this.m_currentWeaponBehavior.RequirementsIsWhitelist)
                  flag2 = !flag2;
                if (!flag2 || !weapon1.IsStationary())
                {
                  this.m_weaponBehaviorAssignedRules.Add(-1);
                  continue;
                }
              }
              for (int index = 0; index < this.m_currentWeaponBehavior.WeaponRules.Count; ++index)
              {
                if (!string.IsNullOrEmpty(this.m_currentWeaponBehavior.WeaponRules[index].Weapon))
                {
                  string weapon2 = this.m_currentWeaponBehavior.WeaponRules[index].Weapon;
                  typeId = weapon1.BlockDefinition.Id.TypeId;
                  string str = typeId.ToString();
                  if (!weapon2.Equals(str))
                    continue;
                }
                flag1 = true;
                this.m_weaponBehaviorAssignedRules.Add(index);
                this.m_weaponBehaviorTimes[index] = MyUtils.GetRandomFloat(this.m_currentWeaponBehavior.WeaponRules[index].TimeMin, this.m_currentWeaponBehavior.WeaponRules[index].TimeMax);
                break;
              }
              if (!flag1)
                this.m_weaponBehaviorAssignedRules.Add(-1);
            }
            this.m_weaponBehaviorActive = true;
            return;
          }
        }
        this.m_weaponBehaviorCooldown = this.m_currentPreset.WeaponBehaviorNotFoundDelay;
      }
    }

    private void WeaponsUpdate(float distSq)
    {
      this.m_shooting = false;
      if (this.m_currentPreset.UsesWeaponBehaviors && this.m_weaponBehaviorActive)
      {
        bool flag = false;
        for (int index = 0; index < this.m_weaponBehaviorTimes.Count; ++index)
        {
          if ((double) this.m_weaponBehaviorTimes[index] >= 0.0)
            flag = true;
        }
        if (!flag)
        {
          this.m_weaponBehaviorActive = false;
          this.m_weaponBehaviorCooldown = MyUtils.GetRandomFloat(this.m_currentWeaponBehavior.TimeMin, this.m_currentWeaponBehavior.TimeMax);
          for (int index = 0; index < this.m_weapons.Count; ++index)
            this.m_weapons[index].SetShooting(false);
          return;
        }
      }
      bool shooting = true;
      if (this.m_currentPreset.UsesWeaponBehaviors && !this.m_weaponBehaviorActive)
      {
        this.ChangeWeaponBehavior();
        if (!this.m_weaponBehaviorActive)
          shooting = false;
      }
      bool flag1 = this.m_currentPreset.CanBeDisabled;
      bool flag2 = false;
      bool hitVoxel = false;
      bool hitGrid = false;
      int num = 0;
      if (this.m_weapons != null && this.m_weapons.Count > 0)
      {
        for (int index = 0; index < this.m_weapons.Count; ++index)
        {
          if (this.m_weapons[index].Closed || this.m_weapons[index].CubeGrid != this.m_remoteControl.CubeGrid && !MyCubeGridGroups.Static.Logical.HasSameGroup(this.m_weapons[index].CubeGrid, this.m_remoteControl.CubeGrid))
          {
            this.m_weapons.RemoveAt(index);
            --index;
          }
          else if (!this.m_weapons[index].Enabled && this.m_weapons[index].IsFunctional)
          {
            flag1 = false;
            if (!this.m_weapons[index].IsStationary())
              flag2 = true;
          }
          else
          {
            MyGunStatusEnum status;
            if (this.m_weapons[index].CanOperate() && this.m_weapons[index].CanShoot(out status) && status == MyGunStatusEnum.OK)
            {
              flag1 = false;
              if (this.m_currentPreset.UseStaticWeaponry && this.m_weapons[index].IsStationary())
              {
                if (this.m_currentPreset.UsesWeaponBehaviors && this.m_weaponBehaviorActive)
                {
                  if (this.m_weaponBehaviorAssignedRules[index] != -1)
                  {
                    if (this.m_weaponBehaviorWeaponLock[index])
                    {
                      this.m_shooting = shooting;
                      continue;
                    }
                    if ((double) this.m_weaponBehaviorTimes[this.m_weaponBehaviorAssignedRules[index]] < 0.0)
                    {
                      this.m_weapons[index].SetShooting(false);
                      continue;
                    }
                  }
                  else
                    continue;
                }
                if (this.m_remoteControl.TargettingAimDelta <= 0.0500000007450581 && (double) distSq < (double) this.m_currentPreset.StaticWeaponryUsageSq && ((double) distSq >= 0.0 && this.m_canRotateToTarget))
                {
                  this.m_shooting = shooting;
                  if (this.m_weaponBehaviorActive)
                  {
                    if (this.m_currentPreset.UsesWeaponBehaviors && (!this.m_currentWeaponBehavior.WeaponRules[this.m_weaponBehaviorAssignedRules[index]].CanGoThroughVoxels || !this.m_currentWeaponBehavior.IgnoresGrids))
                    {
                      if (num < 10)
                      {
                        ++num;
                        this.RaycastCheck((Vector3) this.m_weapons[index].GetWeaponMuzzleWorldPosition(), out hitVoxel, out hitGrid);
                      }
                      if (hitVoxel && !this.m_currentWeaponBehavior.WeaponRules[this.m_weaponBehaviorAssignedRules[index]].CanGoThroughVoxels || hitGrid && !this.m_currentWeaponBehavior.IgnoresGrids)
                      {
                        this.m_weapons[index].SetShooting(false);
                        continue;
                      }
                    }
                    if (this.m_currentPreset.UsesWeaponBehaviors && (double) this.m_weaponBehaviorTimes[this.m_weaponBehaviorAssignedRules[index]] == 0.0)
                      this.m_weapons[index].ShootFromTerminal((Vector3) this.m_weapons[index].WorldMatrix.Forward);
                    else
                      this.m_weapons[index].SetShooting(shooting);
                    this.Ambushing = false;
                    if (this.m_currentPreset.UsesWeaponBehaviors && this.m_currentWeaponBehavior.WeaponRules[this.m_weaponBehaviorAssignedRules[index]].FiringAfterLosingSight)
                      this.m_weaponBehaviorWeaponLock[index] = shooting;
                  }
                  else
                    continue;
                }
                else if (!this.m_currentPreset.UsesWeaponBehaviors || !this.m_weaponBehaviorActive || !this.m_currentWeaponBehavior.WeaponRules[this.m_weaponBehaviorAssignedRules[index]].FiringAfterLosingSight)
                  this.m_weapons[index].SetShooting(false);
              }
              if (!this.m_weapons[index].IsStationary())
              {
                if (this.Ambushing && this.m_weapons[index] is MyLargeTurretBase && ((MyLargeTurretBase) this.m_weapons[index]).IsShooting)
                {
                  this.Ambushing = false;
                  this.m_shooting = true;
                }
                flag2 = true;
              }
            }
          }
        }
      }
      if (this.m_currentPreset.UsesWeaponBehaviors && this.m_shooting)
      {
        for (int index = 0; index < this.m_weaponBehaviorTimes.Count; ++index)
          this.m_weaponBehaviorTimes[index] -= 0.3f;
      }
      if (this.m_tools != null && this.m_tools.Count > 0)
      {
        for (int index = 0; index < this.m_tools.Count; ++index)
        {
          if (this.m_tools[index].IsFunctional)
          {
            flag1 = false;
            if (this.m_currentPreset.UseTools)
            {
              if ((double) distSq < (double) this.m_currentPreset.ToolsUsageSq && (double) distSq >= 0.0 && this.m_canRotateToTarget)
              {
                this.m_tools[index].Enabled = true;
                this.Ambushing = false;
              }
              else
                this.m_tools[index].Enabled = false;
            }
          }
        }
      }
      this.m_operational = !flag1;
      if (flag1)
      {
        this.m_rotateToTarget = true;
        this.m_weapons.Clear();
        this.m_tools.Clear();
        if (this.m_remoteControl.HasWaypoints())
          this.m_remoteControl.ClearWaypoints();
        this.NeedUpdate = true;
        this.m_forcedWaypoints.Clear();
      }
      if (flag2 || this.m_alternativebehaviorSwitched)
        return;
      this.m_rotateToTarget = true;
      if (this.m_currentPreset.AlternativeBehavior.Length > 0)
      {
        this.m_currentPreset = MyDroneAIDataStatic.LoadPreset(this.m_currentPreset.AlternativeBehavior);
        this.LoadDroneAIData();
      }
      this.m_alternativebehaviorSwitched = true;
    }

    public static bool SetAIToGrid(MyCubeGrid grid, string behaviour, float activationDistance)
    {
      using (MyFatBlockReader<MyRemoteControl> fatBlocks = grid.GetFatBlocks<MyRemoteControl>())
      {
        if (!fatBlocks.MoveNext())
          return false;
        MyRemoteControl current = fatBlocks.Current;
        current.SetAutomaticBehaviour((MyRemoteControl.IRemoteControlAutomaticBehaviour) new MyDroneAI(current, behaviour, true, (List<MyEntity>) null, (List<DroneTarget>) null, 1, TargetPrioritization.PriorityRandom, activationDistance, false));
        current.SetAutoPilotEnabled(true);
        return true;
      }
    }
  }
}
