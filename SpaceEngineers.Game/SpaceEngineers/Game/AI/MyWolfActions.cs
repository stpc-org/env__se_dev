// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.AI.MyWolfActions
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.AI;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.AI.Navigation;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.AI;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
  [MyBehaviorDescriptor("Wolf")]
  [BehaviorActionImpl(typeof (MyWolfLogic))]
  public class MyWolfActions : MyAgentActions
  {
    private Vector3D? m_runAwayPos;
    private Vector3D? m_lastTargetedEntityPosition;
    private Vector3D? m_debugTarget;

    private MyWolfTarget WolfTarget => this.AiTargetBase as MyWolfTarget;

    protected MyWolfLogic WolfLogic => this.Bot.AgentLogic as MyWolfLogic;

    public MyWolfActions(MyAnimalBot bot)
      : base((MyAgentBot) bot)
    {
    }

    protected override MyBehaviorTreeState Idle() => MyBehaviorTreeState.RUNNING;

    [MyBehaviorTreeAction("GoToPlayerDefinedTarget", ReturnsRunning = true)]
    protected MyBehaviorTreeState GoToPlayerDefinedTarget()
    {
      Vector3D? debugTarget1 = this.m_debugTarget;
      Vector3D? debugTarget2 = MyAIComponent.Static.DebugTarget;
      if ((debugTarget1.HasValue == debugTarget2.HasValue ? (debugTarget1.HasValue ? (debugTarget1.GetValueOrDefault() != debugTarget2.GetValueOrDefault() ? 1 : 0) : 0) : 1) != 0)
      {
        this.m_debugTarget = MyAIComponent.Static.DebugTarget;
        debugTarget2 = MyAIComponent.Static.DebugTarget;
        if (!debugTarget2.HasValue)
          return MyBehaviorTreeState.FAILURE;
      }
      Vector3D position = this.Bot.Player.Character.PositionComp.GetPosition();
      if (this.m_debugTarget.HasValue)
      {
        if (Vector3D.Distance(position, this.m_debugTarget.Value) <= 1.0)
          return MyBehaviorTreeState.SUCCESS;
        Vector3D worldCenter = this.m_debugTarget.Value;
        MyDestinationSphere destinationSphere = new MyDestinationSphere(ref worldCenter, 1f);
        Vector3D target;
        if (!MyAIComponent.Static.Pathfinding.FindPathGlobal(position, (IMyDestinationShape) destinationSphere, (MyEntity) null).GetNextTarget(position, out target, out float _, out IMyEntity _))
          return MyBehaviorTreeState.FAILURE;
        if (this.WolfTarget.TargetPosition != target)
          this.WolfTarget.SetTargetPosition(target);
        this.WolfTarget.AimAtTarget();
        this.WolfTarget.GotoTargetNoPath(0.0f, false);
      }
      return MyBehaviorTreeState.RUNNING;
    }

    [MyBehaviorTreeAction("Attack", MyBehaviorTreeActionType.INIT)]
    protected void Init_Attack()
    {
      this.WolfTarget.AimAtTarget();
      double num = (double) ((Vector3) (this.WolfTarget.TargetPosition - this.Bot.AgentEntity.PositionComp.GetPosition())).Normalize();
      this.WolfTarget.Attack(!this.WolfLogic.SelfDestructionActivated);
    }

    [MyBehaviorTreeAction("Attack")]
    protected MyBehaviorTreeState Attack() => !this.WolfTarget.IsAttacking ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.RUNNING;

    [MyBehaviorTreeAction("Attack", MyBehaviorTreeActionType.POST)]
    protected void Post_Attack()
    {
    }

    [MyBehaviorTreeAction("IsAttacking", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsAttacking() => !this.WolfTarget.IsAttacking ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;

    [MyBehaviorTreeAction("Explode")]
    protected MyBehaviorTreeState Explode()
    {
      this.WolfLogic.ActivateSelfDestruct();
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("GetTargetWithPriority")]
    protected MyBehaviorTreeState GetTargetWithPriority(
      [BTParam] float radius,
      [BTInOut] ref MyBBMemoryTarget outTarget,
      [BTInOut] ref MyBBMemoryInt priority)
    {
      if (this.WolfLogic.SelfDestructionActivated)
        return MyBehaviorTreeState.SUCCESS;
      if (this.Bot?.AgentEntity == null)
        return MyBehaviorTreeState.FAILURE;
      BoundingSphereD boundingSphere = new BoundingSphereD(this.Bot.Navigation.PositionAndOrientation.Translation, (double) radius);
      if (priority == null)
        priority = new MyBBMemoryInt();
      int num1 = priority.IntValue;
      if (num1 <= 0 || this.Bot.Navigation.Stuck)
        num1 = int.MaxValue;
      MyBehaviorTreeState behaviorTreeState = this.IsTargetValid(ref outTarget);
      if (behaviorTreeState == MyBehaviorTreeState.FAILURE)
      {
        num1 = 7;
        MyBBMemoryTarget.UnsetTarget(ref outTarget);
      }
      if (this.WolfTarget == null)
        return MyBehaviorTreeState.FAILURE;
      Vector3D? memoryTargetPosition = this.WolfTarget.GetMemoryTargetPosition(outTarget);
      if (!memoryTargetPosition.HasValue || Vector3D.DistanceSquared(memoryTargetPosition.Value, this.Bot.AgentEntity.PositionComp.GetPosition()) > 160000.0)
      {
        num1 = 7;
        MyBBMemoryTarget.UnsetTarget(ref outTarget);
      }
      if (memoryTargetPosition.HasValue)
      {
        Vector3D globalPos = memoryTargetPosition.Value;
        MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(globalPos);
        if (closestPlanet != null)
        {
          Vector3D surfacePointGlobal = closestPlanet.GetClosestSurfacePointGlobal(ref globalPos);
          if (Vector3D.DistanceSquared(surfacePointGlobal, globalPos) > 2.25 && Vector3D.DistanceSquared(surfacePointGlobal, this.Bot.AgentEntity.PositionComp.GetPosition()) < 25.0)
          {
            num1 = 7;
            MyBBMemoryTarget.UnsetTarget(ref outTarget);
          }
        }
      }
      MyFaction playerFaction1 = MySession.Static.Factions.GetPlayerFaction(this.Bot.AgentEntity.ControllerInfo.ControllingIdentityId);
      List<MyEntity> entitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
      entitiesInSphere.ShuffleList<MyEntity>();
      foreach (MyEntity entity in entitiesInSphere)
      {
        if (entity != this.Bot.AgentEntity && !(entity is MyVoxelBase) && this.WolfTarget.IsEntityReachable(entity))
        {
          if (entity is MyCharacter myCharacter)
          {
            MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
            if ((playerFaction1 == null || playerFaction2 != playerFaction1) && !myCharacter.IsDead)
            {
              int num2 = 1;
              if (num2 < num1)
              {
                behaviorTreeState = MyBehaviorTreeState.SUCCESS;
                num1 = num2;
                MyBBMemoryTarget.SetTargetEntity(ref outTarget, MyAiTargetEnum.CHARACTER, myCharacter.EntityId);
                this.m_lastTargetedEntityPosition = new Vector3D?(myCharacter.PositionComp.GetPosition());
              }
            }
          }
        }
      }
      entitiesInSphere.Clear();
      priority.IntValue = num1;
      if (outTarget.TargetType == MyAiTargetEnum.NO_TARGET)
        behaviorTreeState = MyBehaviorTreeState.FAILURE;
      return behaviorTreeState;
    }

    [MyBehaviorTreeAction("IsRunningAway", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsRunningAway() => !this.m_runAwayPos.HasValue ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;

    [MyBehaviorTreeAction("RunAway", MyBehaviorTreeActionType.INIT)]
    protected MyBehaviorTreeState RunAway_Init() => MyBehaviorTreeState.RUNNING;

    [MyBehaviorTreeAction("RunAway")]
    protected MyBehaviorTreeState RunAway([BTParam] float distance)
    {
      if (!this.m_runAwayPos.HasValue)
      {
        MySteeringBase steeringOfType = this.Bot.Navigation.GetSteeringOfType(typeof (MyCharacterAvoidanceSteering));
        if (steeringOfType != null)
          ((MyCharacterAvoidanceSteering) steeringOfType).AvoidPlayer = true;
        Vector3D position = this.Bot.Player.Character.PositionComp.GetPosition();
        Vector3D naturalGravityInPoint = (Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
        MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
        if (closestPlanet == null)
          return MyBehaviorTreeState.FAILURE;
        if (this.m_lastTargetedEntityPosition.HasValue)
        {
          Vector3D surfacePointGlobal = this.m_lastTargetedEntityPosition.Value;
          surfacePointGlobal = closestPlanet.GetClosestSurfacePointGlobal(ref surfacePointGlobal);
          Vector3D vector3D = position - surfacePointGlobal;
          Vector3D globalPos = position + Vector3D.Normalize(vector3D) * (double) distance;
          this.m_runAwayPos = new Vector3D?(closestPlanet.GetClosestSurfacePointGlobal(ref globalPos));
        }
        else
        {
          naturalGravityInPoint.Normalize();
          Vector3D perpendicularVector = Vector3D.CalculatePerpendicularVector(naturalGravityInPoint);
          Vector3D bitangent = Vector3D.Cross(naturalGravityInPoint, perpendicularVector);
          perpendicularVector.Normalize();
          bitangent.Normalize();
          Vector3D randomDiscPosition = MyUtils.GetRandomDiscPosition(ref position, (double) distance, (double) distance, ref perpendicularVector, ref bitangent);
          this.m_runAwayPos = closestPlanet == null ? new Vector3D?(randomDiscPosition) : new Vector3D?(closestPlanet.GetClosestSurfacePointGlobal(ref randomDiscPosition));
        }
        this.AiTargetBase.SetTargetPosition(this.m_runAwayPos.Value);
        int num = (int) this.AimWithMovement();
      }
      else if (this.Bot.Navigation.Stuck)
        return MyBehaviorTreeState.FAILURE;
      this.AiTargetBase.GotoTarget();
      if (Vector3D.DistanceSquared(this.m_runAwayPos.Value, this.Bot.Player.Character.PositionComp.GetPosition()) >= 100.0)
        return MyBehaviorTreeState.RUNNING;
      this.WolfLogic.Remove();
      return MyBehaviorTreeState.SUCCESS;
    }
  }
}
