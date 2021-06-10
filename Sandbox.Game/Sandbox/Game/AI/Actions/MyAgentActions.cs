// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Actions.MyAgentActions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.AI.Pathfinding;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.AI;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.AI.Actions
{
  public abstract class MyAgentActions : MyBotActionsBase
  {
    private string m_animationName;
    private int m_animationStart;
    private bool m_isPlayingAnimation;
    private readonly MyRandomLocationSphere m_locationSphere;
    private MyStringId m_animationStringId;

    protected MyAgentBot Bot { get; }

    public MyAiTargetBase AiTargetBase => this.Bot.AgentLogic.AiTarget;

    protected MyAgentActions(MyAgentBot bot)
    {
      this.Bot = bot;
      this.m_locationSphere = new MyRandomLocationSphere(Vector3D.Zero, 30f, Vector3D.UnitX);
    }

    [MyBehaviorTreeAction("AimWithMovement", ReturnsRunning = false)]
    protected MyBehaviorTreeState AimWithMovement()
    {
      this.Bot.Navigation.AimWithMovement();
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("GotoTarget", MyBehaviorTreeActionType.INIT)]
    protected virtual void Init_GotoTarget()
    {
      if (!this.AiTargetBase.HasTarget())
        return;
      this.AiTargetBase.GotoTarget();
    }

    [MyBehaviorTreeAction("GotoTarget")]
    protected MyBehaviorTreeState GotoTarget()
    {
      if (!this.AiTargetBase.HasTarget() || !this.Bot.Navigation.Navigating)
        return MyBehaviorTreeState.FAILURE;
      if (this.Bot.Navigation.Stuck)
      {
        this.AiTargetBase.GotoFailed();
        return MyBehaviorTreeState.FAILURE;
      }
      Vector3D targetPosition = this.AiTargetBase.GetTargetPosition(this.Bot.Navigation.PositionAndOrientation.Translation);
      Vector3D targetPoint = this.Bot.Navigation.TargetPoint;
      if ((targetPoint - targetPosition).Length() > 0.100000001490116)
        this.CheckReplanningOfPath(targetPosition, targetPoint, true);
      return MyBehaviorTreeState.RUNNING;
    }

    [MyBehaviorTreeAction("GotoTarget", MyBehaviorTreeActionType.POST)]
    protected void Post_GotoTarget() => this.Bot.Navigation.StopImmediate(true);

    [MyBehaviorTreeAction("GotoTargetNoPathfinding", MyBehaviorTreeActionType.INIT)]
    protected virtual void Init_GotoTargetNoPathfinding()
    {
      if (!this.AiTargetBase.HasTarget())
        return;
      this.AiTargetBase.GotoTargetNoPath(1f);
    }

    [MyBehaviorTreeAction("GotoTargetNoPathfinding")]
    protected MyBehaviorTreeState GotoTargetNoPathfinding(
      [BTParam] float radius,
      [BTParam] bool resetStuckDetection)
    {
      if (!this.AiTargetBase.HasTarget())
        return MyBehaviorTreeState.FAILURE;
      if (!this.Bot.Navigation.Navigating)
        return MyBehaviorTreeState.SUCCESS;
      if (this.Bot.Navigation.Stuck)
      {
        this.AiTargetBase.GotoFailed();
        return MyBehaviorTreeState.FAILURE;
      }
      this.AiTargetBase.GotoTargetNoPath(radius, resetStuckDetection);
      return MyBehaviorTreeState.RUNNING;
    }

    [MyBehaviorTreeAction("AimAtTarget", MyBehaviorTreeActionType.INIT)]
    protected void Init_AimAtTarget() => this.Init_AimAtTargetCustom();

    [MyBehaviorTreeAction("AimAtTarget")]
    protected MyBehaviorTreeState AimAtTarget() => this.AimAtTargetCustom(2f);

    [MyBehaviorTreeAction("AimAtTarget", MyBehaviorTreeActionType.POST)]
    protected void Post_AimAtTarget() => this.Post_AimAtTargetCustom();

    [MyBehaviorTreeAction("AimAtTargetCustom", MyBehaviorTreeActionType.INIT)]
    protected void Init_AimAtTargetCustom()
    {
      if (!this.AiTargetBase.HasTarget())
        return;
      this.AiTargetBase.AimAtTarget();
    }

    [MyBehaviorTreeAction("AimAtTargetCustom")]
    protected MyBehaviorTreeState AimAtTargetCustom([BTParam] float tolerance)
    {
      if (!this.AiTargetBase.HasTarget())
        return MyBehaviorTreeState.FAILURE;
      return this.Bot.Navigation.HasRotation(MathHelper.ToRadians(tolerance)) ? MyBehaviorTreeState.RUNNING : MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("AimAtTargetCustom", MyBehaviorTreeActionType.POST)]
    protected void Post_AimAtTargetCustom() => this.Bot.Navigation.StopAiming();

    [MyBehaviorTreeAction("PlayAnimation", ReturnsRunning = false)]
    protected MyBehaviorTreeState PlayAnimation(
      [BTParam] string animationName,
      [BTParam] bool immediate)
    {
      if (!this.Bot.Player.Character.HasAnimation(animationName))
        return MyBehaviorTreeState.FAILURE;
      this.m_animationName = animationName;
      this.Bot.Player.Character.PlayCharacterAnimation(animationName, immediate ? MyBlendOption.Immediate : MyBlendOption.WaitForPreviousEnd, MyFrameOption.PlayOnce, 0.0f);
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("PlayAnimationWithLength")]
    protected MyBehaviorTreeState PlayAnimation(
      [BTParam] string animationName,
      [BTParam] int animationLength)
    {
      if (this.m_isPlayingAnimation)
      {
        if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_animationStart > animationLength)
          return MyBehaviorTreeState.SUCCESS;
        this.Bot.AgentEntity.AnimationController.TriggerAction(this.m_animationStringId);
        if (Sync.IsServer)
          MyMultiplayer.RaiseEvent<MyCharacter, string>(this.Bot.AgentEntity, (Func<MyCharacter, Action<string>>) (x => new Action<string>(x.TriggerAnimationEvent)), this.m_animationStringId.String);
        return MyBehaviorTreeState.RUNNING;
      }
      this.m_isPlayingAnimation = true;
      this.m_animationStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_animationStringId = MyStringId.GetOrCompute(animationName);
      return MyBehaviorTreeState.RUNNING;
    }

    [MyBehaviorTreeAction("IsAtTargetPosition", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsAtTargetPosition([BTParam] float radius) => !this.AiTargetBase.HasTarget() || !this.AiTargetBase.PositionIsNearTarget(this.Bot.Player.Character.PositionComp.GetPosition(), radius) ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;

    [MyBehaviorTreeAction("IsAtTargetPositionCylinder", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsAtTargetPositionCylinder(
      [BTParam] float radius,
      [BTParam] float height)
    {
      if (!this.AiTargetBase.HasTarget())
        return MyBehaviorTreeState.FAILURE;
      Vector3D position = this.Bot.Player.Character.PositionComp.GetPosition();
      Vector3D targetPosition;
      this.AiTargetBase.GetTargetPosition(position, out targetPosition, out float _);
      Vector2 vector2_1 = new Vector2((float) position.X, (float) position.Z);
      Vector2 vector2_2 = new Vector2((float) targetPosition.X, (float) targetPosition.Z);
      return (double) Vector2.Distance(vector2_1, vector2_2) > (double) radius || (double) vector2_1.Y >= (double) vector2_2.Y || (double) vector2_1.Y + (double) height <= (double) vector2_2.Y ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("IsNotAtTargetPosition", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsNotAtTargetPosition([BTParam] float radius) => !this.AiTargetBase.HasTarget() || this.AiTargetBase.PositionIsNearTarget(this.Bot.Player.Character.PositionComp.GetPosition(), radius) ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;

    [MyBehaviorTreeAction("IsLookingAtTarget", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsLookingAtTarget() => this.Bot.Navigation.HasRotation(MathHelper.ToRadians(2f)) ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;

    [MyBehaviorTreeAction("SetTarget", ReturnsRunning = false)]
    protected MyBehaviorTreeState SetTarget([BTIn] ref MyBBMemoryTarget inTarget)
    {
      if (inTarget != null)
        return this.AiTargetBase.SetTargetFromMemory(inTarget) ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.FAILURE;
      this.AiTargetBase.UnsetTarget();
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("ClearTarget", ReturnsRunning = false)]
    protected MyBehaviorTreeState ClearTarget([BTInOut] ref MyBBMemoryTarget inTarget)
    {
      if (inTarget != null)
      {
        inTarget.TargetType = MyAiTargetEnum.NO_TARGET;
        inTarget.Position = new Vector3D?();
        inTarget.EntityId = new long?();
        inTarget.TreeId = new int?();
      }
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("IsTargetValid", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsTargetValid([BTIn] ref MyBBMemoryTarget inTarget) => inTarget == null || !this.AiTargetBase.IsMemoryTargetValid(inTarget) ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;

    [MyBehaviorTreeAction("HasPlaceArea", ReturnsRunning = false)]
    protected MyBehaviorTreeState HasTargetArea([BTIn] ref MyBBMemoryTarget inTarget) => MyBehaviorTreeState.FAILURE;

    [MyBehaviorTreeAction("HasTarget", ReturnsRunning = false)]
    protected MyBehaviorTreeState HasTarget() => this.AiTargetBase.TargetType != MyAiTargetEnum.NO_TARGET ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.FAILURE;

    [MyBehaviorTreeAction("HasNoTarget", ReturnsRunning = false)]
    protected MyBehaviorTreeState HasNoTarget() => this.HasTarget() != MyBehaviorTreeState.SUCCESS ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.FAILURE;

    [MyBehaviorTreeAction("Stand", ReturnsRunning = false)]
    protected MyBehaviorTreeState Stand()
    {
      this.Bot.AgentEntity.Stand();
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("SwitchToWalk", ReturnsRunning = false)]
    protected MyBehaviorTreeState SwitchToWalk()
    {
      if (!this.Bot.AgentEntity.WantsWalk)
        this.Bot.AgentEntity.SwitchWalk();
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("SwitchToRun", ReturnsRunning = false)]
    protected MyBehaviorTreeState SwitchToRun()
    {
      if (this.Bot.AgentEntity.WantsWalk)
        this.Bot.AgentEntity.SwitchWalk();
      return MyBehaviorTreeState.SUCCESS;
    }

    private void SetRandomLocationTarget()
    {
      Vector3D position = this.Bot.AgentEntity.PositionComp.GetPosition();
      MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
      if (closestPlanet == null)
        return;
      Vector3D vector3D = (Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
      if (vector3D == Vector3D.Zero)
        vector3D = Vector3D.Up;
      vector3D.Normalize();
      Vector3D perpendicularVector = Vector3D.CalculatePerpendicularVector(vector3D);
      Vector3D bitangent = Vector3D.Cross(vector3D, perpendicularVector);
      perpendicularVector.Normalize();
      bitangent.Normalize();
      Vector3D globalPos = MyUtils.GetRandomDiscPosition(ref position, 10.0, 20.0, ref perpendicularVector, ref bitangent);
      globalPos = closestPlanet.GetClosestSurfacePointGlobal(ref globalPos);
      Vector3D? freePlace = Sandbox.Game.Entities.MyEntities.FindFreePlace(globalPos, 2f);
      if (!freePlace.HasValue)
        return;
      this.Bot.Navigation.Goto((IMyDestinationShape) this.m_locationSphere);
      this.AiTargetBase.SetTargetPosition(freePlace.Value + vector3D);
      this.Bot.Navigation.AimAt((MyEntity) null, new Vector3D?(this.AiTargetBase.TargetPosition));
      this.AiTargetBase.GotoTarget();
    }

    private static Vector3D GetRandomPerpendicularVector(ref Vector3D axis)
    {
      Vector3D perpendicularVector = Vector3D.CalculatePerpendicularVector(axis);
      Vector3D result;
      Vector3D.Cross(ref axis, ref perpendicularVector, out result);
      double randomDouble = MyUtils.GetRandomDouble(0.0, 6.2831859588623);
      return Math.Cos(randomDouble) * perpendicularVector + Math.Sin(randomDouble) * result;
    }

    [MyBehaviorTreeAction("GotoRandomLocation", MyBehaviorTreeActionType.INIT)]
    protected void Init_GotoRandomLocation()
    {
      int num = (int) this.SetRandomLocationAsTarget();
    }

    [MyBehaviorTreeAction("GotoRandomLocation")]
    protected MyBehaviorTreeState GotoRandomLocation() => this.GotoTarget();

    [MyBehaviorTreeAction("GotoRandomLocation", MyBehaviorTreeActionType.POST)]
    protected void Post_GotoRandomLocation()
    {
      this.AiTargetBase.UnsetTarget();
      this.Post_GotoTarget();
    }

    [MyBehaviorTreeAction("GotoAndAimTarget", MyBehaviorTreeActionType.INIT)]
    protected void Init_GotoAndAimTarget()
    {
      if (!this.AiTargetBase.HasTarget())
        return;
      this.AiTargetBase.GotoTarget();
      this.AiTargetBase.AimAtTarget();
    }

    [MyBehaviorTreeAction("GotoAndAimTarget")]
    protected MyBehaviorTreeState GotoAndAimTarget()
    {
      if (!this.AiTargetBase.HasTarget())
        return MyBehaviorTreeState.FAILURE;
      if (this.Bot.Navigation.Navigating || this.Bot.Navigation.HasRotation(MathHelper.ToRadians(2f)))
      {
        if (this.Bot.Navigation.Stuck)
        {
          this.AiTargetBase.GotoFailed();
          return MyBehaviorTreeState.FAILURE;
        }
        Vector3D targetPosition = this.AiTargetBase.GetTargetPosition(this.Bot.Navigation.PositionAndOrientation.Translation);
        Vector3D targetPoint = this.Bot.Navigation.TargetPoint;
        if ((targetPoint - targetPosition).Length() > 0.100000001490116)
          this.CheckReplanningOfPath(targetPosition, targetPoint, true);
        return MyBehaviorTreeState.RUNNING;
      }
      if (this.Bot.Navigation.IsWaitingForTileGeneration)
        return MyBehaviorTreeState.FAILURE;
      this.AiTargetBase.GotoFailed();
      return MyBehaviorTreeState.FAILURE;
    }

    private void CheckReplanningOfPath(
      Vector3D targetPos,
      Vector3D navigationTarget,
      bool aimAtTarget)
    {
      Vector3D vector3D = targetPos - this.Bot.Navigation.PositionAndOrientation.Translation;
      Vector3D v = navigationTarget - this.Bot.Navigation.PositionAndOrientation.Translation;
      double num1 = vector3D.Length();
      double num2 = v.Length();
      if (num1 == 0.0 || num2 == 0.0)
        return;
      double num3 = Math.Acos(vector3D.Dot(v) / (num1 * num2));
      double num4 = num1 / num2;
      if (num3 <= Math.PI / 5.0 && num4 >= 0.8 && (num4 <= 1.0 || num2 >= 2.0))
        return;
      this.AiTargetBase.GotoTarget();
      if (!aimAtTarget)
        return;
      this.AiTargetBase.AimAtTarget();
    }

    [MyBehaviorTreeAction("GotoAndAimTarget", MyBehaviorTreeActionType.POST)]
    protected void Post_GotoAndAimTarget()
    {
      this.Bot.Navigation.StopImmediate(true);
      this.Bot.Navigation.StopAiming();
    }

    [MyBehaviorTreeAction("StopAiming", ReturnsRunning = false)]
    protected MyBehaviorTreeState StopAiming()
    {
      this.Bot.Navigation.StopAiming();
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("GotoFailed", ReturnsRunning = false)]
    protected MyBehaviorTreeState GotoFailed() => this.AiTargetBase.HasGotoFailed ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.FAILURE;

    [MyBehaviorTreeAction("ResetGotoFailed", ReturnsRunning = false)]
    protected MyBehaviorTreeState ResetGotoFailed()
    {
      this.AiTargetBase.HasGotoFailed = false;
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("IsMoving", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsMoving() => !this.Bot.Navigation.Navigating ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;

    [MyBehaviorTreeAction("FindClosestPlaceAreaInRadius", ReturnsRunning = false)]
    protected MyBehaviorTreeState FindClosestPlaceAreaInRadius(
      [BTParam] float radius,
      [BTParam] string typeName,
      [BTOut] ref MyBBMemoryTarget outTarget)
    {
      return MyBehaviorTreeState.FAILURE;
    }

    [MyBehaviorTreeAction("IsTargetBlock", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsTargetBlock([BTIn] ref MyBBMemoryTarget inTarget) => inTarget.TargetType == MyAiTargetEnum.COMPOUND_BLOCK || inTarget.TargetType == MyAiTargetEnum.CUBE ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.FAILURE;

    [MyBehaviorTreeAction("IsTargetNonBlock", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsTargetNonBlock([BTIn] ref MyBBMemoryTarget inTarget) => inTarget.TargetType == MyAiTargetEnum.COMPOUND_BLOCK || inTarget.TargetType == MyAiTargetEnum.CUBE ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;

    [MyBehaviorTreeAction("FindClosestBlock", ReturnsRunning = false)]
    protected MyBehaviorTreeState FindClosestBlock([BTOut] ref MyBBMemoryTarget outBlock)
    {
      if (!this.AiTargetBase.IsTargetGridOrBlock(this.AiTargetBase.TargetType))
      {
        outBlock = (MyBBMemoryTarget) null;
        return MyBehaviorTreeState.FAILURE;
      }
      MyCubeGrid targetGrid = this.AiTargetBase.TargetGrid;
      Vector3 vector3 = (Vector3) Vector3D.Transform(this.Bot.BotEntity.PositionComp.GetPosition(), targetGrid.PositionComp.WorldMatrixNormalizedInv);
      float num1 = float.MaxValue;
      MySlimBlock mySlimBlock = (MySlimBlock) null;
      foreach (MySlimBlock block in targetGrid.GetBlocks())
      {
        float num2 = Vector3.DistanceSquared((Vector3) block.Position * targetGrid.GridSize, vector3);
        if ((double) num2 < (double) num1)
        {
          mySlimBlock = block;
          num1 = num2;
        }
      }
      if (mySlimBlock == null)
        return MyBehaviorTreeState.FAILURE;
      MyBBMemoryTarget.SetTargetCube(ref outBlock, mySlimBlock.Position, mySlimBlock.CubeGrid.EntityId);
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("SetRandomLocationAsTarget", ReturnsRunning = false)]
    protected MyBehaviorTreeState SetRandomLocationAsTarget()
    {
      this.SetRandomLocationTarget();
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("SetAndAimTarget", ReturnsRunning = false)]
    protected MyBehaviorTreeState SetAndAimTarget([BTIn] ref MyBBMemoryTarget inTarget) => this.SetTarget(true, ref inTarget);

    protected MyBehaviorTreeState SetTarget(
      bool aim,
      ref MyBBMemoryTarget inTarget)
    {
      if (inTarget == null || !this.AiTargetBase.SetTargetFromMemory(inTarget))
        return MyBehaviorTreeState.FAILURE;
      if (aim)
        this.AiTargetBase.AimAtTarget();
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("FindCharacterInRadius", ReturnsRunning = false)]
    protected MyBehaviorTreeState FindCharacterInRadius(
      [BTParam] int radius,
      [BTOut] ref MyBBMemoryTarget outCharacter)
    {
      MyCharacter characterInRadius = this.FindCharacterInRadius(radius);
      if (characterInRadius == null)
        return MyBehaviorTreeState.FAILURE;
      MyBBMemoryTarget.SetTargetEntity(ref outCharacter, MyAiTargetEnum.CHARACTER, characterInRadius.EntityId);
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("IsCharacterInRadius", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsCharacterInRadius([BTParam] int radius)
    {
      MyCharacter characterInRadius = this.FindCharacterInRadius(radius);
      return characterInRadius != null && !characterInRadius.IsDead ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.FAILURE;
    }

    [MyBehaviorTreeAction("IsNoCharacterInRadius", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsNoCharacterInRadius([BTParam] int radius)
    {
      MyCharacter characterInRadius = this.FindCharacterInRadius(radius);
      return characterInRadius != null && !characterInRadius.IsDead ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;
    }

    protected MyCharacter FindCharacterInRadius(int radius, bool ignoreReachability = false)
    {
      Vector3D translation = this.Bot.Navigation.PositionAndOrientation.Translation;
      ICollection<MyPlayer> onlinePlayers = Sync.Players.GetOnlinePlayers();
      MyCharacter myCharacter = (MyCharacter) null;
      double num1 = 3.40282346638529E+38;
      foreach (MyPlayer myPlayer in (IEnumerable<MyPlayer>) onlinePlayers)
      {
        if (myPlayer.Id.SerialId != 0)
        {
          MyHumanoidBot bot = MyAIComponent.Static.Bots.TryGetBot<MyHumanoidBot>(myPlayer.Id.SerialId);
          if (bot == null || bot.BotDefinition.BehaviorType == "Barbarian")
            continue;
        }
        if (myPlayer.Character != null && (ignoreReachability || this.AiTargetBase.IsEntityReachable((MyEntity) myPlayer.Character)) && !myPlayer.Character.IsDead)
        {
          double num2 = Vector3D.DistanceSquared(myPlayer.Character.PositionComp.GetPosition(), translation);
          if (num2 < (double) (radius * radius) && num2 < num1)
          {
            myCharacter = myPlayer.Character;
            num1 = num2;
          }
        }
      }
      return myCharacter;
    }

    [MyBehaviorTreeAction("HasCharacter", ReturnsRunning = false)]
    protected MyBehaviorTreeState HasCharacter() => this.Bot.AgentEntity == null ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;

    [MyBehaviorTreeAction("CallMoveAndRotate")]
    protected MyBehaviorTreeState CallMoveAndRotate()
    {
      if (this.Bot.AgentEntity == null)
        return MyBehaviorTreeState.FAILURE;
      this.Bot.AgentEntity.MoveAndRotate(Vector3.Zero, Vector2.One, 0.0f);
      return MyBehaviorTreeState.RUNNING;
    }

    [MyBehaviorTreeAction("ClearUnreachableEntities")]
    protected MyBehaviorTreeState ClearUnreachableEntities()
    {
      this.AiTargetBase.ClearUnreachableEntities();
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("IsHealthAboveThreshold", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsHealthAboveThreshold([BTParam] float healthThreshold)
    {
      MyAgentBot bot = this.Bot;
      int num1;
      if (bot == null)
      {
        num1 = 0;
      }
      else
      {
        float? nullable = bot.Player?.Character?.StatComp?.Health?.Value;
        float num2 = healthThreshold;
        num1 = (double) nullable.GetValueOrDefault() > (double) num2 & nullable.HasValue ? 1 : 0;
      }
      return num1 == 0 ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("IsWaitingForTileGeneration", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsWaitingForTileGeneration() => !this.Bot.Navigation.IsWaitingForTileGeneration ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;
  }
}
