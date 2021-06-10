// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.AI.MySpiderTarget
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
  [TargetType("Spider")]
  public class MySpiderTarget : MyAiTargetBase
  {
    private int m_attackStart;
    private int m_attackCtr;
    private bool m_attackPerformed;
    private BoundingSphereD m_attackBoundingSphere;
    private const int ATTACK_LENGTH = 1000;
    private const int ATTACK_ACTIVATION = 700;
    private const int ATTACK_DAMAGE_TO_CHARACTER = 35;
    private const int ATTACK_DAMAGE_TO_GRID = 50;
    private static HashSet<MySlimBlock> m_tmpBlocks = new HashSet<MySlimBlock>();

    public bool IsAttacking { get; private set; }

    public MySpiderTarget(IMyEntityBot bot)
      : base(bot)
    {
    }

    public void Attack()
    {
      MyCharacter agentEntity = this.m_bot.AgentEntity;
      if (agentEntity == null)
        return;
      this.IsAttacking = true;
      this.m_attackPerformed = false;
      this.m_attackStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      string animation;
      string sound;
      this.ChooseAttackAnimationAndSound(out animation, out sound);
      agentEntity.PlayCharacterAnimation(animation, MyBlendOption.Immediate, MyFrameOption.PlayOnce, 0.0f, sync: true);
      agentEntity.DisableAnimationCommands();
      agentEntity.SoundComp.StartSecondarySound(sound, true);
    }

    public override void Update()
    {
      base.Update();
      if (!this.IsAttacking)
        return;
      int num1 = MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_attackStart;
      if (num1 > 1000)
      {
        this.IsAttacking = false;
        this.m_bot.AgentEntity?.EnableAnimationCommands();
      }
      else if (num1 > 500 && this.m_bot.AgentEntity.UseNewAnimationSystem && !this.m_attackPerformed)
      {
        this.m_bot.AgentEntity.TriggerAnimationEvent("attack");
        if (Sync.IsServer)
          MyMultiplayer.RaiseEvent<MyCharacter, string>(this.m_bot.AgentEntity, (Func<MyCharacter, Action<string>>) (x => new Action<string>(x.TriggerAnimationEvent)), "attack");
      }
      if (num1 <= 750 || this.m_attackPerformed)
        return;
      MyCharacter agentEntity = this.m_bot.AgentEntity;
      if (agentEntity == null)
        return;
      this.m_attackBoundingSphere = new BoundingSphereD(agentEntity.WorldMatrix.Translation + agentEntity.PositionComp.WorldMatrixRef.Forward * 2.5 + agentEntity.PositionComp.WorldMatrixRef.Up * 1.0, 0.9);
      this.m_attackPerformed = true;
      List<MyEntity> entitiesInSphere = Sandbox.Game.Entities.MyEntities.GetTopMostEntitiesInSphere(ref this.m_attackBoundingSphere);
      foreach (MyEntity myEntity in entitiesInSphere)
      {
        if (myEntity is MyCharacter myCharacter && myEntity != agentEntity && !myCharacter.IsSitting)
        {
          BoundingSphereD worldVolume = myCharacter.PositionComp.WorldVolume;
          double num2 = this.m_attackBoundingSphere.Radius + worldVolume.Radius;
          double num3 = num2 * num2;
          if (Vector3D.DistanceSquared(this.m_attackBoundingSphere.Center, worldVolume.Center) <= num3)
            myCharacter.DoDamage(35f, MyDamageType.Spider, true, agentEntity.EntityId);
        }
      }
      entitiesInSphere.Clear();
    }

    private void ChooseAttackAnimationAndSound(out string animation, out string sound)
    {
      ++this.m_attackCtr;
      switch (this.TargetType)
      {
        case MyAiTargetEnum.CHARACTER:
          if (this.TargetEntity is MyCharacter targetEntity && targetEntity.IsDead)
          {
            if (this.m_attackCtr % 3 == 0)
            {
              animation = "AttackFrontLegs";
              sound = "ArcBotSpiderAttackClaw";
              break;
            }
            animation = "AttackBite";
            sound = "ArcBotSpiderAttackBite";
            break;
          }
          if (this.m_attackCtr % 2 == 0)
          {
            animation = "AttackStinger";
            sound = "ArcBotSpiderAttackSting";
            break;
          }
          animation = "AttackBite";
          sound = "ArcBotSpiderAttackBite";
          break;
        default:
          animation = "AttackFrontLegs";
          sound = "ArcBotSpiderAttackClaw";
          break;
      }
    }

    public override bool IsMemoryTargetValid(MyBBMemoryTarget targetMemory) => targetMemory != null && targetMemory.TargetType != MyAiTargetEnum.GRID && targetMemory.TargetType != MyAiTargetEnum.CUBE && base.IsMemoryTargetValid(targetMemory);
  }
}
