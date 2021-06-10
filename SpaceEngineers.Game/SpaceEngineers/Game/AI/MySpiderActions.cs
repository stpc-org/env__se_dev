// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.AI.MySpiderActions
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Physics;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.AI;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
  [MyBehaviorDescriptor("Spider")]
  [BehaviorActionImpl(typeof (MySpiderLogic))]
  public class MySpiderActions : MyAgentActions
  {
    private MySpiderTarget SpiderTarget => this.AiTargetBase as MySpiderTarget;

    protected MySpiderLogic SpiderLogic => this.Bot.AgentLogic as MySpiderLogic;

    public MySpiderActions(MyAnimalBot bot)
      : base((MyAgentBot) bot)
    {
    }

    protected override MyBehaviorTreeState Idle() => MyBehaviorTreeState.RUNNING;

    [MyBehaviorTreeAction("Burrow", MyBehaviorTreeActionType.INIT)]
    protected void Init_Burrow() => this.SpiderLogic.StartBurrowing();

    [MyBehaviorTreeAction("Burrow")]
    protected MyBehaviorTreeState Burrow()
    {
      if (this.SpiderLogic.IsBurrowing)
        return MyBehaviorTreeState.RUNNING;
      return this.SpiderLogic.CanBurrow && this.SpiderLogic.IsBurrowFinishedSuccessfully ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.FAILURE;
    }

    [MyBehaviorTreeAction("Deburrow", MyBehaviorTreeActionType.INIT)]
    protected void Init_Deburrow() => this.SpiderLogic.StartDeburrowing();

    [MyBehaviorTreeAction("Deburrow")]
    protected MyBehaviorTreeState Deburrow() => !this.SpiderLogic.IsDeburrowing ? MyBehaviorTreeState.NOT_TICKED : MyBehaviorTreeState.RUNNING;

    [MyBehaviorTreeAction("Teleport", ReturnsRunning = false)]
    protected MyBehaviorTreeState Teleport()
    {
      if (this.Bot.Player.Character.HasAnimation("Deburrow"))
      {
        this.Bot.Player.Character.PlayCharacterAnimation("Deburrow", MyBlendOption.Immediate, MyFrameOption.JustFirstFrame, 0.0f, sync: true);
        this.Bot.AgentEntity.DisableAnimationCommands();
      }
      MatrixD spawnPosition;
      int num = MySpaceBotFactory.GetSpiderSpawnPosition(out spawnPosition, new Vector3D?(this.Bot.Player.GetPosition()), this.SpiderLogic.TeleportRadius) ? 1 : 0;
      this.SpiderLogic.TeleportRadius = (double) this.SpiderLogic.TeleportRadius > 5.0 ? this.SpiderLogic.TeleportRadius - 5f : 3f;
      if (num == 0)
        return MyBehaviorTreeState.FAILURE;
      Vector3D translation1 = spawnPosition.Translation;
      MySpiderLogic spiderLogic1 = this.SpiderLogic;
      Vector3D position = translation1;
      MatrixD worldMatrix = this.Bot.AgentEntity.WorldMatrix;
      Vector3 up1 = (Vector3) worldMatrix.Up;
      if (spiderLogic1.IsPositionObstacled(position, up1))
      {
        this.SpiderLogic.TeleportRadius += 5f;
        return MyBehaviorTreeState.NOT_TICKED;
      }
      MySpiderLogic spiderLogic2 = this.SpiderLogic;
      worldMatrix = this.Bot.AgentEntity.WorldMatrix;
      Vector3D translation2 = worldMatrix.Translation;
      worldMatrix = this.Bot.AgentEntity.WorldMatrix;
      Vector3 up2 = (Vector3) worldMatrix.Up;
      if (spiderLogic2.IsPositionObstacled(translation2, up2))
      {
        this.SpiderLogic.TeleportRadius += 5f;
        return MyBehaviorTreeState.NOT_TICKED;
      }
      float radius = (float) this.Bot.AgentEntity.PositionComp.WorldVolume.Radius;
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(translation1);
      if (closestPlanet != null)
      {
        closestPlanet.CorrectSpawnLocation(ref translation1, (double) radius);
        spawnPosition.Translation = translation1;
      }
      else
      {
        Vector3D? freePlace = MyEntities.FindFreePlace(spawnPosition.Translation, radius, stepSize: 0.2f);
        if (freePlace.HasValue)
          spawnPosition.Translation = freePlace.Value;
      }
      MySpiderLogic spiderLogic3 = this.SpiderLogic;
      Vector3D translation3 = spawnPosition.Translation;
      worldMatrix = this.Bot.AgentEntity.WorldMatrix;
      Vector3 up3 = (Vector3) worldMatrix.Up;
      if (spiderLogic3.IsPositionObstacled(translation3, up3))
      {
        this.SpiderLogic.TeleportRadius += 5f;
        return MyBehaviorTreeState.NOT_TICKED;
      }
      this.Bot.AgentEntity.Physics.Enabled = true;
      this.Bot.AgentEntity.UpdateCharacterPhysics(true);
      this.Bot.AgentEntity.WorldMatrix = spawnPosition;
      if (this.Bot.AgentEntity.Physics.CharacterProxy != null)
        this.Bot.AgentEntity.Physics.CharacterProxy.SetForwardAndUp((Vector3) spawnPosition.Forward, (Vector3) spawnPosition.Up);
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("Attack", MyBehaviorTreeActionType.INIT)]
    protected void Init_Attack()
    {
      this.SpiderTarget.AimAtTarget();
      double num = (double) ((Vector3) (this.SpiderTarget.TargetPosition - this.Bot.AgentEntity.PositionComp.GetPosition())).Normalize();
      this.SpiderTarget.Attack();
    }

    [MyBehaviorTreeAction("Attack")]
    protected MyBehaviorTreeState Attack() => !this.SpiderTarget.IsAttacking ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.RUNNING;

    [MyBehaviorTreeAction("Attack", MyBehaviorTreeActionType.POST)]
    protected void Post_Attack()
    {
    }

    [MyBehaviorTreeAction("IsAttacking", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsAttacking() => !this.SpiderTarget.IsAttacking ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;

    [MyBehaviorTreeAction("GetTargetWithPriority")]
    protected MyBehaviorTreeState GetTargetWithPriority(
      [BTParam] float radius,
      [BTInOut] ref MyBBMemoryTarget outTarget,
      [BTInOut] ref MyBBMemoryInt priority)
    {
      BoundingSphereD boundingSphere = new BoundingSphereD(this.Bot.Navigation.PositionAndOrientation.Translation, (double) radius);
      if (priority == null)
        priority = new MyBBMemoryInt();
      int num1 = priority.IntValue;
      if (num1 <= 0)
        num1 = int.MaxValue;
      MyBehaviorTreeState behaviorTreeState = this.IsTargetValid(ref outTarget);
      if (behaviorTreeState == MyBehaviorTreeState.FAILURE)
      {
        num1 = 7;
        MyBBMemoryTarget.UnsetTarget(ref outTarget);
      }
      Vector3D? memoryTargetPosition = this.SpiderTarget.GetMemoryTargetPosition(outTarget);
      if (!memoryTargetPosition.HasValue || Vector3D.Distance(memoryTargetPosition.Value, this.Bot.AgentEntity.PositionComp.GetPosition()) > 400.0)
      {
        num1 = 7;
        MyBBMemoryTarget.UnsetTarget(ref outTarget);
      }
      MyFaction playerFaction1 = MySession.Static.Factions.GetPlayerFaction(this.Bot.AgentEntity.ControllerInfo.ControllingIdentityId);
      List<MyEntity> entitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
      entitiesInSphere.ShuffleList<MyEntity>();
      foreach (MyEntity entity in entitiesInSphere)
      {
        if (entity != this.Bot.AgentEntity && this.SpiderTarget.IsEntityReachable(entity))
        {
          if (entity is MyCharacter myCharacter && myCharacter.ControllerInfo != null)
          {
            MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(myCharacter.ControllerInfo.ControllingIdentityId);
            if ((playerFaction1 == null || playerFaction2 != playerFaction1) && !myCharacter.IsDead)
            {
              MatrixD worldMatrix = myCharacter.WorldMatrix;
              Vector3D translation1 = worldMatrix.Translation;
              worldMatrix = myCharacter.WorldMatrix;
              Vector3D vector3D1 = 3.0 * worldMatrix.Up;
              Vector3D from = translation1 - vector3D1;
              worldMatrix = myCharacter.WorldMatrix;
              Vector3D translation2 = worldMatrix.Translation;
              worldMatrix = myCharacter.WorldMatrix;
              Vector3D vector3D2 = 3.0 * worldMatrix.Up;
              Vector3D to = translation2 + vector3D2;
              MyPhysics.HitInfo? nullable = MyPhysics.CastRay(from, to, 15);
              if (nullable.HasValue && ((IHitInfo) nullable).HitEntity != myCharacter)
              {
                int num2 = 1;
                if (num2 < num1)
                {
                  behaviorTreeState = MyBehaviorTreeState.SUCCESS;
                  num1 = num2;
                  MyBBMemoryTarget.SetTargetEntity(ref outTarget, MyAiTargetEnum.CHARACTER, myCharacter.EntityId);
                }
              }
            }
          }
        }
      }
      entitiesInSphere.Clear();
      priority.IntValue = num1;
      return behaviorTreeState;
    }
  }
}
