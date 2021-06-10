// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.AI.MyWolfTarget
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
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
  [TargetType("Wolf")]
  [StaticEventOwner]
  public class MyWolfTarget : MyAiTargetBase
  {
    private int m_attackStart;
    private bool m_attackPerformed;
    private BoundingSphereD m_attackBoundingSphere;
    private const int ATTACK_LENGTH = 1000;
    private const int ATTACK_DAMAGE_TO_CHARACTER = 12;
    private const int ATTACK_DAMAGE_TO_GRID = 8;
    private static HashSet<MySlimBlock> m_tmpBlocks = new HashSet<MySlimBlock>();
    private static MyStringId m_stringIdAttackAction = MyStringId.GetOrCompute("attack");

    public bool IsAttacking { get; private set; }

    public MyWolfTarget(IMyEntityBot bot)
      : base(bot)
    {
    }

    public void Attack(bool playSound)
    {
      MyCharacter agentEntity = this.m_bot.AgentEntity;
      if (agentEntity == null)
        return;
      this.IsAttacking = true;
      this.m_attackPerformed = false;
      this.m_attackStart = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (!agentEntity.UseNewAnimationSystem)
      {
        agentEntity.PlayCharacterAnimation("WolfAttack", MyBlendOption.Immediate, MyFrameOption.PlayOnce, 0.0f, sync: true);
        agentEntity.DisableAnimationCommands();
      }
      agentEntity.SoundComp.StartSecondarySound("ArcBotWolfAttack", true);
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
      else if (num1 > 500 && this.m_bot.AgentEntity.UseNewAnimationSystem)
      {
        this.m_bot.AgentEntity.AnimationController.TriggerAction(MyWolfTarget.m_stringIdAttackAction);
        if (Sync.IsServer)
          MyMultiplayer.RaiseEvent<MyCharacter, string>(this.m_bot.AgentEntity, (Func<MyCharacter, Action<string>>) (x => new Action<string>(x.TriggerAnimationEvent)), MyWolfTarget.m_stringIdAttackAction.String);
      }
      if (num1 <= 500 || this.m_attackPerformed)
        return;
      MyCharacter agentEntity = this.m_bot.AgentEntity;
      if (agentEntity == null)
        return;
      this.m_attackBoundingSphere = new BoundingSphereD(agentEntity.WorldMatrix.Translation + agentEntity.PositionComp.WorldMatrixRef.Forward * 1.10000002384186 + agentEntity.PositionComp.WorldMatrixRef.Up * 0.449999988079071, 0.5);
      this.m_attackPerformed = true;
      List<MyEntity> entitiesInSphere = Sandbox.Game.Entities.MyEntities.GetTopMostEntitiesInSphere(ref this.m_attackBoundingSphere);
      foreach (MyEntity myEntity in entitiesInSphere)
      {
        if (myEntity is MyCharacter && myEntity != agentEntity)
        {
          MyCharacter myCharacter = myEntity as MyCharacter;
          if (!myCharacter.IsSitting)
          {
            BoundingSphereD worldVolume = myCharacter.PositionComp.WorldVolume;
            double num2 = this.m_attackBoundingSphere.Radius + worldVolume.Radius;
            double num3 = num2 * num2;
            if (Vector3D.DistanceSquared(this.m_attackBoundingSphere.Center, worldVolume.Center) <= num3)
              myCharacter.DoDamage(12f, MyDamageType.Wolf, true, agentEntity.EntityId);
          }
        }
      }
      entitiesInSphere.Clear();
    }

    [Event(null, 150)]
    [Broadcast]
    [Reliable]
    private static void PlayAttackAnimation(long entityId)
    {
      if (!Sandbox.Game.Entities.MyEntities.EntityExists(entityId) || !(Sandbox.Game.Entities.MyEntities.GetEntityById(entityId) is MyCharacter entityById))
        return;
      entityById.AnimationController.TriggerAction(MyWolfTarget.m_stringIdAttackAction);
    }

    public override bool IsMemoryTargetValid(MyBBMemoryTarget targetMemory) => targetMemory != null && targetMemory.TargetType != MyAiTargetEnum.GRID && targetMemory.TargetType != MyAiTargetEnum.CUBE && base.IsMemoryTargetValid(targetMemory);

    protected sealed class PlayAttackAnimation\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyWolfTarget.PlayAttackAnimation(entityId);
      }
    }
  }
}
