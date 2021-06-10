// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Actions.MyAnimalBotActions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game;
using VRage.Game.AI;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.AI.Actions
{
  [MyBehaviorDescriptor("Animal")]
  [BehaviorActionImpl(typeof (MyAnimalBotLogic))]
  public class MyAnimalBotActions : MyAgentActions
  {
    private readonly MyAnimalBot m_bot;
    private const long m_eatTimeInS = 10;
    private long m_eatCounter;
    private long m_soundCounter;
    private bool m_usingPathfinding;
    private static readonly double COS15 = Math.Cos((double) MathHelper.ToRadians(15f));

    private MyAnimalBotLogic AnimalLogic => this.m_bot.BotLogic as MyAnimalBotLogic;

    public MyAnimalBotActions(MyAnimalBot bot)
      : base((MyAgentBot) bot)
      => this.m_bot = bot;

    [MyBehaviorTreeAction("IdleDanger", ReturnsRunning = false)]
    protected MyBehaviorTreeState IdleDanger()
    {
      this.m_bot.AgentEntity.SoundComp.StartSecondarySound("BotDeerBark", true);
      return MyBehaviorTreeState.SUCCESS;
    }

    protected override void Init_Idle()
    {
      this.m_bot.Navigation.StopImmediate(true);
      this.m_eatCounter = Stopwatch.GetTimestamp() + 10L * Stopwatch.Frequency;
      if (MyUtils.GetRandomInt(2) == 0)
      {
        long num = MyUtils.GetRandomLong() % 8L + 1L;
        this.m_soundCounter = Stopwatch.GetTimestamp() + num * Stopwatch.Frequency;
      }
      else
        this.m_soundCounter = 0L;
      this.m_bot.AgentEntity.PlayCharacterAnimation("Idle", MyBlendOption.Immediate, MyFrameOption.Loop, 0.5f);
    }

    protected override MyBehaviorTreeState Idle()
    {
      long timestamp = Stopwatch.GetTimestamp();
      if (this.m_soundCounter != 0L && this.m_soundCounter < timestamp)
      {
        if ((double) MyRandom.Instance.NextFloat() > 0.699999988079071)
          this.m_bot.AgentEntity.SoundComp.StartSecondarySound("BotDeerRoar", true);
        else
          this.m_bot.AgentEntity.SoundComp.StartSecondarySound("BotDeerBark", true);
        this.m_soundCounter = 0L;
      }
      return this.m_eatCounter > timestamp ? MyBehaviorTreeState.RUNNING : MyBehaviorTreeState.SUCCESS;
    }

    protected override void Init_GotoTarget()
    {
      if (!this.AiTargetBase.HasTarget())
        return;
      this.AiTargetBase.GotoTargetNoPath(0.0f);
      this.m_bot.Navigation.AimWithMovement();
    }

    [MyBehaviorTreeAction("FindWanderLocation", ReturnsRunning = false)]
    protected MyBehaviorTreeState FindWanderLocation(
      [BTOut] ref MyBBMemoryTarget outTarget)
    {
      return MyBehaviorTreeState.FAILURE;
    }

    [MyBehaviorTreeAction("IsHumanInArea", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsHumanInArea(
      [BTParam] int standingRadius,
      [BTParam] int crouchingRadius,
      [BTOut] ref MyBBMemoryTarget outTarget)
    {
      MyCharacter foundCharacter = (MyCharacter) null;
      if (!this.TryFindValidHumanInArea(standingRadius, crouchingRadius, out foundCharacter))
        return MyBehaviorTreeState.FAILURE;
      MyBBMemoryTarget.SetTargetEntity(ref outTarget, MyAiTargetEnum.CHARACTER, foundCharacter.EntityId);
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("AmIBeingFollowed", ReturnsRunning = false)]
    protected MyBehaviorTreeState AmIBeingFollowed([BTIn] ref MyBBMemoryTarget inTarget) => inTarget != null ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.FAILURE;

    [MyBehaviorTreeAction("IsHumanNotInArea", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsHumanNotInArea(
      [BTParam] int standingRadius,
      [BTParam] int crouchingRadius,
      [BTOut] ref MyBBMemoryTarget outTarget)
    {
      return this.InvertState(this.IsHumanInArea(standingRadius, crouchingRadius, ref outTarget));
    }

    private MyBehaviorTreeState InvertState(MyBehaviorTreeState state)
    {
      if (state == MyBehaviorTreeState.SUCCESS)
        return MyBehaviorTreeState.FAILURE;
      return state == MyBehaviorTreeState.FAILURE ? MyBehaviorTreeState.SUCCESS : state;
    }

    private bool TryFindValidHumanInArea(
      int standingRadius,
      int crouchingRadius,
      out MyCharacter foundCharacter)
    {
      Vector3D position = this.m_bot.AgentEntity.PositionComp.GetPosition();
      Vector3D forward = this.m_bot.AgentEntity.PositionComp.WorldMatrixRef.Forward;
      foreach (MyPlayer onlinePlayer in (IEnumerable<MyPlayer>) Sync.Players.GetOnlinePlayers())
      {
        if ((onlinePlayer.Id.SerialId == 0 || MyAIComponent.Static.Bots.GetBotType(onlinePlayer.Id.SerialId) == BotType.HUMANOID) && (onlinePlayer.Character != null && !onlinePlayer.Character.MarkedForClose) && !onlinePlayer.Character.IsDead)
        {
          Vector3D vector1 = onlinePlayer.Character.PositionComp.GetPosition() - position;
          vector1.Y = 0.0;
          double num = vector1.Normalize();
          bool flag = false;
          if (num < (double) standingRadius)
            flag = Vector3D.Dot(vector1, forward) > MyAnimalBotActions.COS15 || (!onlinePlayer.Character.IsCrouching || onlinePlayer.Character.IsSprinting || num < (double) crouchingRadius);
          if (flag)
          {
            foundCharacter = onlinePlayer.Character;
            return true;
          }
        }
      }
      foundCharacter = (MyCharacter) null;
      return false;
    }

    [MyBehaviorTreeAction("FindRandomSafeLocation", ReturnsRunning = false)]
    protected MyBehaviorTreeState FindRandomSafeLocation(
      [BTIn] ref MyBBMemoryTarget inTargetEnemy,
      [BTOut] ref MyBBMemoryTarget outTargetLocation)
    {
      MyBBMemoryTarget myBbMemoryTarget = inTargetEnemy;
      MyEntity entity;
      if ((myBbMemoryTarget != null ? (!myBbMemoryTarget.EntityId.HasValue ? 1 : 0) : 1) != 0 || Sandbox.Engine.Platform.Game.IsDedicated || !MyEntities.TryGetEntityById(inTargetEnemy.EntityId.Value, out entity))
        return MyBehaviorTreeState.FAILURE;
      Vector3D position = this.m_bot.AgentEntity.PositionComp.GetPosition();
      Vector3D direction = position - entity.PositionComp.GetPosition();
      direction.Normalize();
      Vector3D outPosition;
      if (!this.AiTargetBase.GetRandomDirectedPosition(position, direction, out outPosition))
        outPosition = position + direction * 30.0;
      MyBBMemoryTarget.SetTargetPosition(ref outTargetLocation, outPosition);
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("RunAway", MyBehaviorTreeActionType.INIT)]
    protected void Init_RunAway()
    {
      this.AnimalLogic.EnableCharacterAvoidance(true);
      this.m_bot.Navigation.AimWithMovement();
      this.AiTargetBase.GotoTargetNoPath(0.0f);
    }

    [MyBehaviorTreeAction("RunAway")]
    protected MyBehaviorTreeState RunAway()
    {
      if (!this.m_bot.Navigation.Navigating)
        return MyBehaviorTreeState.SUCCESS;
      if (!this.m_bot.Navigation.Stuck)
        return MyBehaviorTreeState.RUNNING;
      if (this.m_usingPathfinding)
        return MyBehaviorTreeState.FAILURE;
      this.m_usingPathfinding = true;
      this.AnimalLogic.EnableCharacterAvoidance(false);
      this.AiTargetBase.GotoTarget();
      return MyBehaviorTreeState.RUNNING;
    }

    [MyBehaviorTreeAction("RunAway", MyBehaviorTreeActionType.POST)]
    protected void Post_RunAway()
    {
      this.m_usingPathfinding = false;
      this.m_bot.Navigation.StopImmediate(true);
    }

    [MyBehaviorTreeAction("PlaySound", ReturnsRunning = false)]
    protected MyBehaviorTreeState PlaySound([BTParam] string soundtrack)
    {
      this.m_bot.AgentEntity.SoundComp.StartSecondarySound(soundtrack, true);
      return MyBehaviorTreeState.SUCCESS;
    }
  }
}
