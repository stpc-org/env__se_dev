// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyMissile
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons
{
  [MyEntityType(typeof (MyObjectBuilder_Missile), true)]
  public sealed class MyMissile : MyAmmoBase, IMyEventProxy, IMyEventOwner, IMyDestroyableObject
  {
    private MyMissileAmmoDefinition m_missileAmmoDefinition;
    private float m_maxTrajectory;
    private MyParticleEffect m_smokeEffect;
    private MyExplosionTypeEnum m_explosionType;
    private MyEntity m_collidedEntity;
    private Vector3D? m_collisionPoint;
    private Vector3 m_collisionNormal;
    private long m_owner;
    private readonly float m_smokeEffectOffsetMultiplier = 0.4f;
    private Vector3 m_linearVelocity;
    private MyWeaponPropertiesWrapper m_weaponProperties;
    private long m_launcherId;
    public static bool DEBUG_DRAW_MISSILE_TRAJECTORY;
    internal int m_pruningProxyId = -1;
    private readonly MyEntity3DSoundEmitter m_soundEmitter;
    private bool m_removed;

    public SerializableDefinitionId AmmoMagazineId => (SerializableDefinitionId) this.m_weaponProperties.AmmoMagazineId;

    public SerializableDefinitionId WeaponDefinitionId => (SerializableDefinitionId) this.m_weaponProperties.WeaponDefinitionId;

    public MyMissile()
    {
      this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this);
      if (MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS)
      {
        Func<bool> func = (Func<bool>) (() => MySession.Static.ControlledEntity?.Entity is MyCharacter entity && entity == this.m_collidedEntity);
        this.m_soundEmitter.EmitterMethods[1].Add((Delegate) func);
        this.m_soundEmitter.EmitterMethods[0].Add((Delegate) func);
      }
      this.Flags |= EntityFlags.IsNotGamePrunningStructureObject;
      if (!Sync.IsDedicated)
        return;
      this.Flags &= ~EntityFlags.UpdateRender;
      this.InvalidateOnMove = false;
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      MyObjectBuilder_Missile objectBuilderMissile = (MyObjectBuilder_Missile) objectBuilder;
      base.Init(objectBuilder);
      this.m_weaponProperties = new MyWeaponPropertiesWrapper((MyDefinitionId) objectBuilderMissile.WeaponDefinitionId);
      this.m_weaponProperties.ChangeAmmoMagazine((MyDefinitionId) objectBuilderMissile.AmmoMagazineId);
      this.m_missileAmmoDefinition = this.m_weaponProperties.GetCurrentAmmoDefinitionAs<MyMissileAmmoDefinition>();
      this.Init(this.m_weaponProperties, this.m_missileAmmoDefinition.MissileModelName, false, true, true, Sync.IsServer);
      this.UseDamageSystem = true;
      this.m_maxTrajectory = this.m_missileAmmoDefinition.MaxTrajectory;
      this.SyncFlag = true;
      this.m_collisionPoint = new Vector3D?();
      this.m_owner = objectBuilderMissile.Owner;
      this.m_originEntity = objectBuilderMissile.OriginEntity;
      this.m_linearVelocity = objectBuilderMissile.LinearVelocity;
      this.m_launcherId = objectBuilderMissile.LauncherId;
      this.OnPhysicsChanged += new Action<MyEntity>(this.OnMissilePhysicsChanged);
    }

    private void OnMissilePhysicsChanged(MyEntity entity)
    {
      if (this.Physics == null || !((HkReferenceObject) this.Physics.RigidBody != (HkReferenceObject) null))
        return;
      this.Physics.RigidBody.CallbackLimit = 1;
    }

    public void UpdateData(MyObjectBuilder_EntityBase objectBuilder)
    {
      MyObjectBuilder_Missile objectBuilderMissile = (MyObjectBuilder_Missile) objectBuilder;
      if (objectBuilder.PositionAndOrientation.HasValue)
      {
        MyPositionAndOrientation positionAndOrientation = objectBuilder.PositionAndOrientation.Value;
        MatrixD world = MatrixD.CreateWorld((Vector3D) positionAndOrientation.Position, (Vector3) positionAndOrientation.Forward, (Vector3) positionAndOrientation.Up);
        this.PositionComp.SetWorldMatrix(ref world, ignoreAssert: true);
      }
      this.EntityId = objectBuilderMissile.EntityId;
      this.m_owner = objectBuilderMissile.Owner;
      this.m_originEntity = objectBuilderMissile.OriginEntity;
      this.m_linearVelocity = objectBuilderMissile.LinearVelocity;
      this.m_launcherId = objectBuilderMissile.LauncherId;
      this.m_collisionPoint = new Vector3D?();
      this.m_markedToDestroy = false;
      this.m_removed = false;
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_Missile objectBuilder = (MyObjectBuilder_Missile) base.GetObjectBuilder(copy);
      objectBuilder.LinearVelocity = this.Physics.LinearVelocity;
      objectBuilder.AmmoMagazineId = (SerializableDefinitionId) this.m_weaponProperties.AmmoMagazineId;
      objectBuilder.WeaponDefinitionId = (SerializableDefinitionId) this.m_weaponProperties.WeaponDefinitionId;
      objectBuilder.Owner = this.m_owner;
      objectBuilder.OriginEntity = this.m_originEntity;
      objectBuilder.LauncherId = this.m_launcherId;
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    public static MyObjectBuilder_Missile PrepareBuilder(
      MyWeaponPropertiesWrapper weaponProperties,
      Vector3D position,
      Vector3D initialVelocity,
      Vector3D direction,
      long owner,
      long originEntity,
      long launcherId)
    {
      MyObjectBuilder_Missile newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Missile>();
      newObject.LinearVelocity = (Vector3) initialVelocity;
      newObject.AmmoMagazineId = (SerializableDefinitionId) weaponProperties.AmmoMagazineId;
      newObject.WeaponDefinitionId = (SerializableDefinitionId) weaponProperties.WeaponDefinitionId;
      newObject.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
      Vector3D to = position + direction * 4.0;
      if (!MyPhysics.CastRay(position, to).HasValue)
        position = to;
      newObject.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation(position, (Vector3) direction, (Vector3) Vector3D.CalculatePerpendicularVector(direction)));
      newObject.Owner = owner;
      newObject.OriginEntity = originEntity;
      newObject.LauncherId = launcherId;
      newObject.EntityId = MyEntityIdentifier.AllocateId();
      return newObject;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.m_shouldExplode = false;
      this.Start(this.PositionComp.GetPosition(), (Vector3D) this.m_linearVelocity, this.WorldMatrix.Forward);
      if (this.m_physicsEnabled)
      {
        this.Physics.RigidBody.MaxLinearVelocity = this.m_missileAmmoDefinition.DesiredSpeed;
        this.Physics.RigidBody.Layer = 8;
        this.Physics.CanUpdateAccelerations = false;
      }
      this.m_explosionType = MyExplosionTypeEnum.MISSILE_EXPLOSION;
      if (Sync.IsDedicated)
        return;
      MySoundPair shootSound = this.m_weaponDefinition.WeaponAmmoDatas[1].ShootSound;
      if (shootSound != null)
        this.m_soundEmitter.PlaySingleSound(shootSound, true);
      MatrixD matrixD = this.PositionComp.WorldMatrixRef;
      matrixD.Translation -= matrixD.Forward * (double) this.m_smokeEffectOffsetMultiplier;
      Vector3D translation = matrixD.Translation;
      MyParticlesManager.TryCreateParticleEffect("Smoke_Missile", ref MatrixD.Identity, ref translation, this.Render.GetRenderObjectID(), out this.m_smokeEffect);
      if (!(MyEntities.GetEntityById(this.m_launcherId) is IMyMissileGunObject entityById))
        return;
      entityById.MissileShootEffect();
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      if (this.m_smokeEffect != null)
      {
        this.m_smokeEffect.Stop(false);
        this.m_smokeEffect = (MyParticleEffect) null;
      }
      this.m_soundEmitter.StopSound(true);
    }

    public override void UpdateBeforeSimulation()
    {
      if (this.m_shouldExplode)
      {
        this.ExecuteExplosion();
      }
      else
      {
        base.UpdateBeforeSimulation();
        Vector3D position = this.PositionComp.GetPosition();
        if (this.m_physicsEnabled)
        {
          this.m_linearVelocity = this.Physics.LinearVelocity;
          this.Physics.AngularVelocity = Vector3.Zero;
        }
        this.m_linearVelocity = !this.m_missileAmmoDefinition.MissileSkipAcceleration ? (Vector3) (this.m_linearVelocity + this.PositionComp.WorldMatrixRef.Forward * (double) this.m_missileAmmoDefinition.MissileAcceleration * 0.0166666675359011) : (Vector3) (this.WorldMatrix.Forward * (double) this.m_missileAmmoDefinition.DesiredSpeed);
        if (this.m_physicsEnabled)
        {
          this.Physics.LinearVelocity = this.m_linearVelocity;
        }
        else
        {
          Vector3.ClampToSphere(ref this.m_linearVelocity, this.m_missileAmmoDefinition.DesiredSpeed);
          this.PositionComp.SetPosition(this.PositionComp.GetPosition() + this.m_linearVelocity * 0.01666667f);
        }
        if ((double) Vector3.DistanceSquared((Vector3) this.PositionComp.GetPosition(), (Vector3) this.m_origin) >= (double) this.m_maxTrajectory * (double) this.m_maxTrajectory)
          this.MarkForExplosion();
        if (MyMissile.DEBUG_DRAW_MISSILE_TRAJECTORY)
          MyRenderProxy.DebugDrawLine3D(position, this.PositionComp.GetPosition(), Color.AliceBlue, Color.AliceBlue, true);
        MyMissiles.OnMissileMoved(this, ref this.m_linearVelocity);
      }
    }

    private void ExecuteExplosion()
    {
      if (!Sync.IsServer)
      {
        this.Return();
      }
      else
      {
        this.PlaceDecal();
        float missileExplosionRadius = this.m_missileAmmoDefinition.MissileExplosionRadius;
        BoundingSphereD boundingSphereD = new BoundingSphereD(this.PositionComp.GetPosition(), (double) missileExplosionRadius);
        MyIdentity identity = Sync.Players.TryGetIdentity(this.m_owner);
        MyEntity myEntity;
        if (identity != null)
        {
          myEntity = (MyEntity) identity.Character;
        }
        else
        {
          MyEntity entity = (MyEntity) null;
          MyEntities.TryGetEntityById(this.m_owner, out entity);
          myEntity = entity;
        }
        MyEntity entityById = MyEntities.GetEntityById(this.m_launcherId, true);
        MyExplosionInfo explosionInfo = new MyExplosionInfo()
        {
          PlayerDamage = 0.0f,
          Damage = this.m_missileAmmoDefinition.MissileExplosionDamage,
          ExplosionType = this.m_explosionType,
          ExplosionSphere = boundingSphereD,
          LifespanMiliseconds = 700,
          HitEntity = this.m_collidedEntity,
          ParticleScale = 1f,
          OwnerEntity = myEntity,
          Direction = new Vector3?(Vector3.Normalize(this.PositionComp.GetPosition() - this.m_origin)),
          VoxelExplosionCenter = boundingSphereD.Center + (double) missileExplosionRadius * this.WorldMatrix.Forward * 0.25,
          ExplosionFlags = MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION | MyExplosionFlags.CREATE_PARTICLE_DEBRIS,
          VoxelCutoutScale = 0.3f,
          PlaySound = true,
          ApplyForceAndDamage = true,
          OriginEntity = this.m_originEntity,
          KeepAffectedBlocks = true,
          IgnoreFriendlyFireSetting = entityById == null || !(entityById is MyAutomaticRifleGun)
        };
        if (this.m_collidedEntity != null && this.m_collidedEntity.Physics != null)
          explosionInfo.Velocity = this.m_collidedEntity.Physics.LinearVelocity;
        if (!this.m_markedToDestroy)
          explosionInfo.ExplosionFlags |= MyExplosionFlags.CREATE_PARTICLE_EFFECT;
        MyExplosions.AddExplosion(ref explosionInfo);
        if (this.m_collidedEntity != null && !(this.m_collidedEntity is MyAmmoBase) && (this.m_collidedEntity.Physics != null && !this.m_collidedEntity.Physics.IsStatic))
          this.m_collidedEntity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, new Vector3?(100f * this.Physics.LinearVelocity), this.m_collisionPoint, new Vector3?());
        this.Return();
      }
    }

    private void Done()
    {
      if (this.m_collidedEntity == null)
        return;
      this.m_collidedEntity.Unpin();
      this.m_collidedEntity = (MyEntity) null;
    }

    private void Return()
    {
      this.Done();
      MyMissiles.Return(this);
    }

    private void PlaceDecal()
    {
      if (this.m_collidedEntity == null || !this.m_collisionPoint.HasValue)
        return;
      MyDecals.HandleAddDecal((IMyEntity) this.m_collidedEntity, new MyHitInfo()
      {
        Position = this.m_collisionPoint.Value,
        Normal = this.m_collisionNormal
      }, Vector3.Zero, this.m_missileAmmoDefinition.PhysicalMaterial);
    }

    private void MarkForExplosion()
    {
      if (this.m_markedToDestroy)
        this.Return();
      else
        this.m_shouldExplode = true;
      if (!Sync.IsServer || this.m_removed)
        return;
      if (MyEntities.GetEntityById(this.m_launcherId) is IMyMissileGunObject entityById)
        entityById.RemoveMissile(this.EntityId);
      this.m_removed = true;
    }

    public override void MarkForDestroy() => this.Return();

    protected override void Closing()
    {
      base.Closing();
      this.Done();
    }

    protected override void OnContactStart(ref MyPhysics.MyContactPointEvent value)
    {
      if (this.MarkedForClose || this.m_collidedEntity != null || !(value.ContactPointEvent.GetOtherEntity((IMyEntity) this) is MyEntity otherEntity))
        return;
      otherEntity.Pin();
      this.m_collidedEntity = otherEntity;
      this.m_collisionPoint = new Vector3D?(value.Position);
      this.m_collisionNormal = value.Normal;
      if (!Sync.IsServer)
        this.PlaceDecal();
      else
        MySandboxGame.Static.Invoke((Action) (() => this.MarkForExplosion()), "MyMissile - collision invoke");
    }

    private void DoDamage(float damage, MyStringHash damageType, bool sync, long attackerId)
    {
      if (sync)
      {
        if (!Sync.IsServer)
          return;
        MySyncDamage.DoDamageSynced((MyEntity) this, damage, damageType, attackerId);
      }
      else
      {
        if (this.UseDamageSystem)
          MyDamageSystem.Static.RaiseDestroyed((object) this, new MyDamageInformation(false, damage, damageType, attackerId));
        this.MarkForExplosion();
      }
    }

    private bool UseDamageSystem { get; set; }

    public long Owner => this.m_owner;

    public bool IsCharacterIdFriendly(long charId)
    {
      switch (MyIDModule.GetRelationPlayerPlayer(this.Owner, charId))
      {
        case MyRelationsBetweenPlayers.Self:
        case MyRelationsBetweenPlayers.Allies:
          return true;
        default:
          return false;
      }
    }

    void IMyDestroyableObject.OnDestroy()
    {
    }

    bool IMyDestroyableObject.DoDamage(
      float damage,
      MyStringHash damageType,
      bool sync,
      MyHitInfo? hitInfo,
      long attackerId,
      long realHitEntityId = 0)
    {
      this.DoDamage(damage, damageType, sync, attackerId);
      return true;
    }

    float IMyDestroyableObject.Integrity => 1f;

    bool IMyDestroyableObject.UseDamageSystem => this.UseDamageSystem;

    private class Sandbox_Game_Weapons_MyMissile\u003C\u003EActor : IActivator, IActivator<MyMissile>
    {
      object IActivator.CreateInstance() => (object) new MyMissile();

      MyMissile IActivator<MyMissile>.CreateInstance() => new MyMissile();
    }
  }
}
