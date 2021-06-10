// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.AI.MyWolfLogic
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Game;
using Sandbox.Game.AI;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using VRage.Game.Entity;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
  public class MyWolfLogic : MyAgentLogic
  {
    private const int SELF_DESTRUCT_TIME_MS = 4000;
    private const float EXPLOSION_RADIUS = 4f;
    private const int EXPLOSION_DAMAGE = 7500;
    private const int EXPLOSION_PLAYER_DAMAGE = 0;
    private int m_selfDestructStartedInTime;
    private bool m_lastWasAttacking;

    public bool SelfDestructionActivated { get; private set; }

    public MyWolfLogic(MyAnimalBot bot)
      : base((IMyBot) bot)
    {
    }

    public override void Update()
    {
      base.Update();
      if (this.SelfDestructionActivated && MySandboxGame.TotalGamePlayTimeInMilliseconds >= this.m_selfDestructStartedInTime + 4000)
      {
        MyAIComponent.Static.RemoveBot(this.AgentBot.Player.Id.SerialId, true);
        BoundingSphere boundingSphere = new BoundingSphere((Vector3) this.AgentBot.Player.GetPosition(), 4f);
        MyExplosionInfo explosionInfo = new MyExplosionInfo()
        {
          PlayerDamage = 0.0f,
          Damage = 7500f,
          ExplosionType = MyExplosionTypeEnum.BOMB_EXPLOSION,
          ExplosionSphere = (BoundingSphereD) boundingSphere,
          LifespanMiliseconds = 700,
          HitEntity = (MyEntity) this.AgentBot.Player.Character,
          ParticleScale = 0.5f,
          OwnerEntity = (MyEntity) this.AgentBot.Player.Character,
          Direction = new Vector3?(Vector3.Zero),
          VoxelExplosionCenter = this.AgentBot.Player.Character.PositionComp.GetPosition(),
          ExplosionFlags = MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION,
          VoxelCutoutScale = 0.6f,
          PlaySound = true,
          ApplyForceAndDamage = true,
          ObjectsRemoveDelayInMiliseconds = 40
        };
        if (this.AgentBot.Player.Character.Physics != null)
          explosionInfo.Velocity = this.AgentBot.Player.Character.Physics.LinearVelocity;
        MyExplosions.AddExplosion(ref explosionInfo);
      }
      MyWolfTarget aiTarget = this.AiTarget as MyWolfTarget;
      if (this.AgentBot.Player.Character != null && !this.AgentBot.Player.Character.UseNewAnimationSystem && (!aiTarget.IsAttacking && !this.m_lastWasAttacking) && (aiTarget.HasTarget() && !aiTarget.PositionIsNearTarget(this.AgentBot.Player.Character.PositionComp.GetPosition(), 1.5f)))
      {
        if (this.AgentBot.Navigation.Stuck)
        {
          Vector3D position = this.AgentBot.Player.Character.PositionComp.GetPosition();
          Vector3D naturalGravityInPoint = (Vector3D) MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
          Vector3D vector1 = this.AgentBot.Player.Character.AimedPoint - position;
          Vector3D vector3D = vector1 - naturalGravityInPoint * Vector3D.Dot(vector1, naturalGravityInPoint) / naturalGravityInPoint.LengthSquared();
          vector3D.Normalize();
          this.AgentBot.Navigation.AimAt((MyEntity) null, new Vector3D?(position + 100.0 * vector3D));
          this.AgentBot.Player.Character.PlayCharacterAnimation("WolfIdle1", MyBlendOption.Immediate, MyFrameOption.Loop, 0.0f);
          this.AgentBot.Player.Character.DisableAnimationCommands();
        }
        else
          this.AgentBot.Player.Character.EnableAnimationCommands();
      }
      this.m_lastWasAttacking = aiTarget.IsAttacking;
    }

    public override void Cleanup() => base.Cleanup();

    public void ActivateSelfDestruct()
    {
      if (this.SelfDestructionActivated)
        return;
      this.m_selfDestructStartedInTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.SelfDestructionActivated = true;
      this.AgentBot.AgentEntity.SoundComp.StartSecondarySound("ArcBotCyberSelfActDestr", true);
    }

    public void Remove() => MyAIComponent.Static.RemoveBot(this.AgentBot.Player.Id.SerialId, true);
  }
}
