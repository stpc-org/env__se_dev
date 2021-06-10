// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyProjectile
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.EnvironmentItems;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment.Definitions;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Generics;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
  internal struct MyProjectile
  {
    private static MyDynamicObjectPool<List<MySafeZone>> m_safeZonePool = new MyDynamicObjectPool<List<MySafeZone>>(100);
    internal static int CHECK_INTERSECTION_INTERVAL = 5;
    private MyProjectile.MyProjectileStateEnum m_state;
    private Vector3D m_origin;
    private Vector3D m_velocity_Projectile;
    private Vector3D m_velocity_Combined;
    private Vector3D m_directionNormalized;
    private float m_speed;
    private float m_maxTrajectory;
    private Vector3D m_position;
    private MyEntity[] m_ignoredEntities;
    private MyEntity m_weapon;
    private List<MySafeZone> m_safeZonesInTrajectory;
    private bool m_supressHitIndicator;
    private MyCharacterHitInfo m_charHitInfo;
    private MyCubeGrid.MyCubeGridHitInfo m_cubeGridHitInfo;
    public float LengthMultiplier;
    private MyProjectileAmmoDefinition m_projectileAmmoDefinition;
    private MyWeaponDefinition m_weaponDefinition;
    private MyStringId m_projectileTrailMaterialId;
    public ulong OwningPlayer;
    public MyEntity OwnerEntity;
    public MyEntity OwnerEntityAbsolute;
    private int m_checkIntersectionIndex;
    private static int checkIntersectionCounter = 0;
    private bool m_positionChecked;
    private static List<MyPhysics.HitInfo> m_raycastResult;
    private static List<MyLineSegmentOverlapResult<MyEntity>> m_entityRaycastResult;
    private const float m_impulseMultiplier = 0.5f;
    private static MyStringHash m_hashBolt = MyStringHash.GetOrCompute("Bolt");
    private static MyStringId ID_PROJECTILE_TRAIL_LINE = MyStringId.GetOrCompute("ProjectileTrailLine");
    public static readonly MyTimedItemCache CollisionSoundsTimedCache = new MyTimedItemCache(60);
    public static readonly MyTimedItemCache CollisionParticlesTimedCache = new MyTimedItemCache(200);
    public static double CollisionSoundSpaceMapping = 0.0399999991059303;
    public static double CollisionParticlesSpaceMapping = 0.800000011920929;
    public static double MaxImpactSoundDistanceSq = 10000.0;
    private static readonly MyTimedItemCache m_prefetchedVoxelRaysTimedCache = new MyTimedItemCache(4000);
    private const double m_prefetchedVoxelRaysSourceMapping = 0.5;
    private const double m_prefetchedVoxelRaysDirectionMapping = 50.0;
    public static bool DEBUG_DRAW_PROJECTILE_TRAJECTORY = false;

    public void Start(
      MyProjectileAmmoDefinition ammoDefinition,
      MyWeaponDefinition weaponDefinition,
      MyEntity[] ignoreEntities,
      Vector3D origin,
      Vector3 initialVelocity,
      Vector3 directionNormalized,
      MyEntity weapon,
      bool supressHitIndicator = false)
    {
      this.m_projectileAmmoDefinition = ammoDefinition;
      this.m_weaponDefinition = weaponDefinition;
      this.m_state = MyProjectile.MyProjectileStateEnum.ACTIVE;
      this.m_ignoredEntities = ignoreEntities;
      this.m_origin = origin + 0.1 * (Vector3D) directionNormalized;
      this.m_position = this.m_origin;
      this.m_weapon = weapon;
      this.m_supressHitIndicator = supressHitIndicator;
      if (ammoDefinition.ProjectileTrailMaterial != null)
        this.m_projectileTrailMaterialId = MyStringId.GetOrCompute(ammoDefinition.ProjectileTrailMaterial);
      this.LengthMultiplier = (double) ammoDefinition.ProjectileTrailProbability < (double) MyUtils.GetRandomFloat(0.0f, 1f) ? 0.0f : 40f;
      this.m_directionNormalized = (Vector3D) directionNormalized;
      this.m_speed = ammoDefinition.DesiredSpeed * ((double) ammoDefinition.SpeedVar > 0.0 ? MyUtils.GetRandomFloat(1f - ammoDefinition.SpeedVar, 1f + ammoDefinition.SpeedVar) : 1f);
      this.m_velocity_Projectile = this.m_directionNormalized * (double) this.m_speed;
      this.m_velocity_Combined = initialVelocity + this.m_velocity_Projectile;
      this.m_maxTrajectory = ammoDefinition.MaxTrajectory;
      bool flag = true;
      if (weaponDefinition != null)
      {
        this.m_maxTrajectory *= weaponDefinition.RangeMultiplier;
        flag = weaponDefinition.UseRandomizedRange;
      }
      if (flag)
        this.m_maxTrajectory *= MyUtils.GetRandomFloat(0.8f, 1f);
      this.m_checkIntersectionIndex = MyProjectile.checkIntersectionCounter++ % MyProjectile.CHECK_INTERSECTION_INTERVAL;
      this.m_positionChecked = false;
      using (MyUtils.ReuseCollection<MyLineSegmentOverlapResult<MyEntity>>(ref MyProjectile.m_entityRaycastResult))
      {
        LineD lineD = new LineD(this.m_origin, this.m_origin + this.m_directionNormalized * (double) this.m_maxTrajectory, (double) this.m_maxTrajectory);
        MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref lineD, MyProjectile.m_entityRaycastResult);
        this.PrefetchSafezones(MyProjectile.m_entityRaycastResult);
        this.PrefetchVoxelPhysicsIfNeeded(MyProjectile.m_entityRaycastResult, ref lineD);
      }
    }

    private void PrefetchVoxelPhysicsIfNeeded(
      List<MyLineSegmentOverlapResult<MyEntity>> entities,
      ref LineD line)
    {
      int num = MyTuple.CombineHashCodes((Vector3D.Floor(line.From) * 0.5).GetHashCode(), Vector3D.Floor(this.m_directionNormalized * 50.0).GetHashCode());
      if (MyProjectile.m_prefetchedVoxelRaysTimedCache.IsItemPresent((long) num, MySandboxGame.TotalSimulationTimeInMilliseconds))
        return;
      foreach (MyLineSegmentOverlapResult<MyEntity> entity in entities)
      {
        if (entity.Element is MyVoxelPhysics element)
          element.PrefetchShapeOnRay(ref line);
      }
    }

    private void PrefetchSafezones(
      List<MyLineSegmentOverlapResult<MyEntity>> entities)
    {
      foreach (MyLineSegmentOverlapResult<MyEntity> entity in entities)
      {
        if (entity.Element is MySafeZone element && (element.AllowedActions & MySafeZoneAction.Shooting) == (MySafeZoneAction) 0)
        {
          if (this.m_safeZonesInTrajectory == null)
            this.m_safeZonesInTrajectory = MyProjectile.m_safeZonePool.Allocate();
          this.m_safeZonesInTrajectory.Add(element);
        }
      }
    }

    private bool IsIgnoredEntity(VRage.ModAPI.IMyEntity entity)
    {
      if (this.m_ignoredEntities != null)
      {
        foreach (MyEntity ignoredEntity in this.m_ignoredEntities)
        {
          if (entity == ignoredEntity)
            return true;
        }
      }
      return false;
    }

    public bool Update()
    {
      if (this.m_state == MyProjectile.MyProjectileStateEnum.KILLED)
        return false;
      Vector3D position1 = this.m_position;
      this.m_position += this.m_velocity_Combined * 0.0166666675359011 * (double) MyFakes.SIMULATION_SPEED;
      if (MyProjectile.DEBUG_DRAW_PROJECTILE_TRAJECTORY)
        MyRenderProxy.DebugDrawLine3D(position1, this.m_position, Color.AliceBlue, Color.AliceBlue, true);
      Vector3 vector3 = (Vector3) (this.m_position - this.m_origin);
      if ((double) Vector3.Dot(vector3, vector3) >= (double) this.m_maxTrajectory * (double) this.m_maxTrajectory)
      {
        this.StopEffect();
        this.m_state = MyProjectile.MyProjectileStateEnum.KILLED;
        return false;
      }
      this.m_checkIntersectionIndex = ++this.m_checkIntersectionIndex % MyProjectile.CHECK_INTERSECTION_INTERVAL;
      if (this.m_checkIntersectionIndex != 0 && this.m_positionChecked)
        return true;
      Vector3D to = position1 + (double) MyProjectile.CHECK_INTERSECTION_INTERVAL * (this.m_velocity_Projectile * 0.0166666675359011 * (double) MyFakes.SIMULATION_SPEED);
      LineD line = new LineD(this.m_positionChecked ? position1 : this.m_origin, to);
      this.m_positionChecked = true;
      VRage.ModAPI.IMyEntity entity;
      MyHitInfo hitInfoRet;
      object customdata;
      if (!this.GetHitEntityAndPosition(line, out entity, out hitInfoRet, out customdata))
        return true;
      if (MyDebugDrawSettings.DEBUG_DRAW_PROJECTILES)
      {
        MyRenderProxy.DebugDrawSphere(hitInfoRet.Position, 0.2f, Color.Red, persistent: true);
        MyRenderProxy.DebugDrawLine3D(line.From, line.To, Color.Yellow, Color.Red, true, true);
      }
      if (entity != null && this.IsIgnoredEntity(entity))
        return true;
      this.m_position = hitInfoRet.Position;
      this.m_position += line.Direction * 0.01;
      if (entity != null)
      {
        bool isHeadshot = false;
        if (entity is MyCharacter myCharacter)
        {
          if (myCharacter.CurrentWeapon is IStoppableAttackingTool currentWeapon)
            currentWeapon.StopShooting(this.OwnerEntity);
          if (customdata != null)
            isHeadshot = ((MyCharacterHitInfo) customdata).HitHead && this.m_projectileAmmoDefinition.HeadShot;
        }
        float num = 1f;
        if (this.m_weapon is IMyHandheldGunObject<MyGunBase> weapon)
        {
          MyGunBase gunBase = weapon.GunBase;
          if (gunBase?.WeaponProperties?.WeaponDefinition != null)
            num = gunBase.WeaponProperties.WeaponDefinition.DamageMultiplier;
        }
        else if (this.m_weaponDefinition != null)
          num = this.m_weaponDefinition.DamageMultiplier;
        MySurfaceImpactEnum surfaceImpact;
        MyStringHash materialType;
        MyProjectile.GetSurfaceAndMaterial(entity, ref line, ref this.m_position, hitInfoRet.ShapeKey, out surfaceImpact, out materialType);
        if (!Sandbox.Engine.Platform.Game.IsDedicated)
          this.PlayHitSound(materialType, entity, hitInfoRet.Position, this.m_projectileAmmoDefinition.PhysicalMaterial);
        hitInfoRet.Velocity = this.m_velocity_Combined;
        float damage = (entity is IMyCharacter ? (isHeadshot ? this.m_projectileAmmoDefinition.ProjectileHeadShotDamage : this.m_projectileAmmoDefinition.ProjectileHealthDamage) : this.m_projectileAmmoDefinition.ProjectileMassDamage) * num;
        if (!MySessionComponentSafeZones.IsActionAllowed(hitInfoRet.Position, MySafeZoneAction.Damage, user: this.OwningPlayer))
        {
          damage = 0.0f;
          long owningPlayer = (long) this.OwningPlayer;
          ulong? steamId = MySession.Static?.LocalHumanPlayer?.Id.SteamId;
          long valueOrDefault = (long) steamId.GetValueOrDefault();
          if (owningPlayer == valueOrDefault & steamId.HasValue)
            MyHud.Notifications.Add(MyNotificationSingletons.DamageTurnedOff);
        }
        if ((double) damage > 0.0)
        {
          this.DoDamage(damage, hitInfoRet, customdata, entity, isHeadshot: isHeadshot);
          if (entity is MyCharacter myCharacter && myCharacter.ControllerInfo != null && (myCharacter.ControllerInfo.IsLocallyHumanControlled() && MyGuiScreenHudSpace.Static != null))
            MyGuiScreenHudSpace.Static.AddDamageIndicator(damage, hitInfoRet, (Vector3) this.m_origin);
        }
        MyStringHash voxelMaterial = materialType;
        if (entity is MyVoxelBase self)
        {
          Vector3D position2 = hitInfoRet.Position;
          MyVoxelMaterialDefinition materialAt = self.GetMaterialAt(ref position2);
          if (materialAt != null)
            voxelMaterial = materialAt.Id.SubtypeId;
        }
        MyDecals.HandleAddDecal(entity, hitInfoRet, Vector3.Zero, materialType, this.m_projectileAmmoDefinition.PhysicalMaterial, (object) (customdata as MyCharacterHitInfo), damage, voxelMaterial);
        if (!Sandbox.Engine.Platform.Game.IsDedicated && !MyProjectile.CollisionParticlesTimedCache.IsPlaceUsed(hitInfoRet.Position, MyProjectile.CollisionParticlesSpaceMapping, MySandboxGame.TotalSimulationTimeInMilliseconds + MyRandom.Instance.Next(0, MyProjectile.CollisionParticlesTimedCache.EventTimeoutMs / 2)))
        {
          Vector3 zero = Vector3.Zero;
          MyCubeBlock myCubeBlock = entity as MyCubeBlock;
          VRage.ModAPI.IMyEntity myEntity = entity;
          if (myCubeBlock != null && entity.Parent != null)
            myEntity = entity.Parent;
          if (!MyMaterialPropertiesHelper.Static.TryCreateCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Hit, hitInfoRet.Position, hitInfoRet.Normal, this.m_projectileAmmoDefinition.PhysicalMaterial, materialType, (VRage.Game.ModAPI.Ingame.IMyEntity) myEntity) && surfaceImpact != MySurfaceImpactEnum.CHARACTER)
            MyProjectile.CreateBasicHitParticles(this.m_projectileAmmoDefinition.ProjectileOnHitEffectName, ref hitInfoRet.Position, ref hitInfoRet.Normal, ref line.Direction, entity, this.m_weapon, 1f, this.OwnerEntity);
        }
        if ((double) damage > 0.0 && (this.m_weapon == null || entity.GetTopMostParent() != this.m_weapon.GetTopMostParent((Type) null)))
          MyProjectile.ApplyProjectileForce(entity, hitInfoRet.Position, (Vector3) this.m_directionNormalized, false, this.m_projectileAmmoDefinition.ProjectileHitImpulse * 0.5f);
      }
      this.StopEffect();
      this.m_state = MyProjectile.MyProjectileStateEnum.KILLED;
      return false;
    }

    private static void CreateBasicHitParticles(
      string effectName,
      ref Vector3D hitPoint,
      ref Vector3 normal,
      ref Vector3D direction,
      VRage.ModAPI.IMyEntity physObject,
      MyEntity weapon,
      float scale,
      MyEntity ownerEntity = null)
    {
      MyUtilRandomVector3ByDeviatingVector byDeviatingVector = new MyUtilRandomVector3ByDeviatingVector((Vector3) Vector3D.Reflect(direction, (Vector3D) normal));
      MatrixD fromDir = MatrixD.CreateFromDir((Vector3D) normal);
      MyParticleEffect effect;
      if (!MyParticlesManager.TryCreateParticleEffect(effectName, MatrixD.CreateWorld(hitPoint, fromDir.Forward, fromDir.Up), out effect))
        return;
      effect.UserScale = scale;
    }

    private bool GetHitEntityAndPosition(
      LineD line,
      out VRage.ModAPI.IMyEntity entity,
      out MyHitInfo hitInfoRet,
      out object customdata)
    {
      entity = (VRage.ModAPI.IMyEntity) null;
      customdata = (object) null;
      hitInfoRet = new MyHitInfo();
      bool safezoneHit = false;
      float safezoneDistSq = float.MaxValue;
      VRage.ModAPI.IMyEntity myEntity = (VRage.ModAPI.IMyEntity) null;
      Vector3 vector3 = Vector3.UnitX;
      Vector3 zero = Vector3.Zero;
      if (this.m_safeZonesInTrajectory != null)
      {
        foreach (MySafeZone mySafeZone in this.m_safeZonesInTrajectory)
        {
          Vector3D? v;
          if (mySafeZone.Enabled && mySafeZone.GetIntersectionWithLine(ref line, out v, true, IntersectionFlags.ALL_TRIANGLES))
          {
            float num = (float) (v.Value - line.From).LengthSquared();
            if ((double) num < (double) safezoneDistSq)
            {
              safezoneHit = true;
              myEntity = (VRage.ModAPI.IMyEntity) mySafeZone;
              vector3 = (Vector3) -line.Direction;
              zero = (Vector3) v.Value;
              safezoneDistSq = num;
            }
          }
        }
      }
      int index = 0;
      using (MyUtils.ReuseCollection<MyPhysics.HitInfo>(ref MyProjectile.m_raycastResult))
      {
        MyPhysics.CastRay(line.From, line.To, MyProjectile.m_raycastResult, 15);
        do
        {
          if (index < MyProjectile.m_raycastResult.Count)
          {
            MyPhysics.HitInfo hitInfo = MyProjectile.m_raycastResult[index];
            entity = (VRage.ModAPI.IMyEntity) (hitInfo.HkHitInfo.GetHitEntity() as MyEntity);
            hitInfoRet.Position = hitInfo.Position;
            hitInfoRet.Normal = hitInfo.HkHitInfo.Normal;
            hitInfoRet.ShapeKey = hitInfo.HkHitInfo.GetShapeKey(0);
          }
          if (this.IsIgnoredEntity(entity))
            entity = (VRage.ModAPI.IMyEntity) null;
          if (entity is MyCharacter myCharacter)
          {
            if (myCharacter.GetIntersectionWithLine(ref line, ref this.m_charHitInfo))
            {
              if (IsHitCloserThanSafezone(this.m_charHitInfo.Triangle.IntersectionPointInWorldSpace))
              {
                hitInfoRet.Position = this.m_charHitInfo.Triangle.IntersectionPointInWorldSpace;
                hitInfoRet.Normal = this.m_charHitInfo.Triangle.NormalInWorldSpace;
                customdata = (object) this.m_charHitInfo;
                safezoneHit = false;
              }
            }
            else
              entity = (VRage.ModAPI.IMyEntity) null;
          }
          else if (entity is MyCubeGrid myCubeGrid)
          {
            bool intersectionWithLine = myCubeGrid.GetIntersectionWithLine(ref line, ref this.m_cubeGridHitInfo);
            if (MyDebugDrawSettings.DEBUG_DRAW_PROJECTILES && intersectionWithLine && this.m_cubeGridHitInfo != null)
            {
              MyTriangle_Vertices inputTriangle = this.m_cubeGridHitInfo.Triangle.Triangle.InputTriangle;
              MySlimBlock cubeBlock = ((MyCubeGrid) entity).GetCubeBlock(this.m_cubeGridHitInfo.Position);
              Vector3 worldCoordinates1 = toWorldCoordinates((VRage.Game.ModAPI.IMySlimBlock) cubeBlock, inputTriangle.Vertex0);
              Vector3 worldCoordinates2 = toWorldCoordinates((VRage.Game.ModAPI.IMySlimBlock) cubeBlock, inputTriangle.Vertex1);
              Vector3 worldCoordinates3 = toWorldCoordinates((VRage.Game.ModAPI.IMySlimBlock) cubeBlock, inputTriangle.Vertex2);
              MyRenderProxy.DebugDrawLine3D((Vector3D) worldCoordinates1, (Vector3D) worldCoordinates2, Color.Purple, Color.Purple, true, true);
              MyRenderProxy.DebugDrawLine3D((Vector3D) worldCoordinates2, (Vector3D) worldCoordinates3, Color.Purple, Color.Purple, true, true);
              MyRenderProxy.DebugDrawLine3D((Vector3D) worldCoordinates3, (Vector3D) worldCoordinates1, Color.Purple, Color.Purple, true, true);
            }
            if (intersectionWithLine && IsHitCloserThanSafezone(this.m_cubeGridHitInfo.Triangle.IntersectionPointInWorldSpace))
            {
              hitInfoRet.Position = this.m_cubeGridHitInfo.Triangle.IntersectionPointInWorldSpace;
              hitInfoRet.Normal = this.m_cubeGridHitInfo.Triangle.NormalInWorldSpace;
              if ((double) Vector3.Dot(hitInfoRet.Normal, (Vector3) line.Direction) > 0.0)
                hitInfoRet.Normal = -hitInfoRet.Normal;
              safezoneHit = false;
            }
            if (this.m_cubeGridHitInfo != null && this.m_cubeGridHitInfo.Triangle.UserObject is MyCube userObject && (userObject.CubeBlock.FatBlock != null && userObject.CubeBlock.FatBlock.Physics == null))
              entity = (VRage.ModAPI.IMyEntity) userObject.CubeBlock.FatBlock;
          }
          else
          {
            MyIntersectionResultLineTriangleEx? t;
            if (entity is MyVoxelBase myVoxelBase && myVoxelBase.GetIntersectionWithLine(ref line, out t, IntersectionFlags.DIRECT_TRIANGLES) && IsHitCloserThanSafezone(t.Value.IntersectionPointInWorldSpace))
            {
              hitInfoRet.Position = t.Value.IntersectionPointInWorldSpace;
              hitInfoRet.Normal = t.Value.NormalInWorldSpace;
              hitInfoRet.ShapeKey = 0U;
              safezoneHit = false;
            }
          }
          if (entity != null)
            break;
        }
        while (++index < MyProjectile.m_raycastResult.Count);
      }
      if (!safezoneHit)
        return entity != null;
      entity = myEntity;
      hitInfoRet.Normal = vector3;
      hitInfoRet.Position = (Vector3D) zero;
      return true;

      bool IsHitCloserThanSafezone(Vector3D otherHit) => !safezoneHit || (otherHit - line.From).LengthSquared() < (double) safezoneDistSq;

      Vector3 toWorldCoordinates(VRage.Game.ModAPI.IMySlimBlock block, Vector3 localCoords)
      {
        Matrix result;
        block.Orientation.GetMatrix(out result);
        return (Vector3) Vector3.Transform(Vector3.Transform(localCoords, result) + block.Position * block.CubeGrid.GridSize, ((VRage.ModAPI.IMyEntity) block.CubeGrid).WorldMatrix);
      }
    }

    private void DoDamage(
      float damage,
      MyHitInfo hitInfo,
      object customdata,
      VRage.ModAPI.IMyEntity damagedEntity,
      VRage.ModAPI.IMyEntity realHitEntity = null,
      bool isHeadshot = false)
    {
      MyEntity controlledEntity = (MyEntity) MySession.Static.ControlledEntity;
      MySession.MyHitIndicatorTarget type = MySession.MyHitIndicatorTarget.None;
      bool flag1 = false;
      bool friendly = false;
      if (damagedEntity is MyCharacter character)
      {
        friendly = this.IsFriendly(character);
        flag1 = friendly && !MySession.Static.Settings.EnableFriendlyFire && this.m_weapon is MyAutomaticRifleGun;
      }
      if (this.OwnerEntityAbsolute != null && this.OwnerEntityAbsolute.Equals((object) MySession.Static.ControlledEntity) && (damagedEntity is IMyDestroyableObject || damagedEntity is MyCubeGrid))
        MySession.Static.TotalDamageDealt += (uint) damage;
      if (Sync.IsServer)
      {
        if (character != null)
          type = this.GetCharacterHitTarget((IMyDestroyableObject) character, isHeadshot, friendly);
        if (this.m_projectileAmmoDefinition.PhysicalMaterial == MyProjectile.m_hashBolt)
        {
          if (damagedEntity is IMyDestroyableObject destroyableObject && character != null)
          {
            float integrity = destroyableObject.Integrity;
            destroyableObject.DoDamage(damage, MyDamageType.Bolt, true, new MyHitInfo?(hitInfo), this.m_weapon != null ? this.GetSubpartOwner(this.m_weapon).EntityId : 0L, realHitEntity != null ? realHitEntity.EntityId : 0L);
            if (character != null && !friendly && ((double) destroyableObject.Integrity <= 0.0 && (double) integrity > 0.0))
              type = MySession.MyHitIndicatorTarget.Kill;
          }
        }
        else
        {
          MyCubeGrid grid = damagedEntity as MyCubeGrid;
          MyCubeBlock myCubeBlock = damagedEntity as MyCubeBlock;
          MySlimBlock mySlimBlock = (MySlimBlock) null;
          if (myCubeBlock != null)
          {
            grid = myCubeBlock.CubeGrid;
            mySlimBlock = myCubeBlock.SlimBlock;
          }
          else if (grid != null)
          {
            mySlimBlock = grid.GetTargetedBlock(hitInfo.Position - 1f / 1000f * hitInfo.Normal);
            if (mySlimBlock != null)
              myCubeBlock = mySlimBlock.FatBlock;
          }
          if (grid != null)
          {
            type = MySession.MyHitIndicatorTarget.Grid;
            if (grid.Physics != null && grid.Physics.Enabled && (grid.BlocksDestructionEnabled || MyFakes.ENABLE_VR_FORCE_BLOCK_DESTRUCTIBLE))
            {
              bool flag2 = false;
              if (mySlimBlock != null && (grid.BlocksDestructionEnabled || mySlimBlock.ForceBlockDestructible))
              {
                mySlimBlock.DoDamage(damage, MyDamageType.Bullet, true, new MyHitInfo?(hitInfo), this.m_weapon != null ? this.GetSubpartOwner(this.m_weapon).EntityId : 0L, realHitEntity != null ? realHitEntity.EntityId : 0L);
                if (myCubeBlock == null)
                  flag2 = true;
              }
              if (grid.BlocksDestructionEnabled & flag2)
                this.ApllyDeformationCubeGrid(damage, hitInfo.Position, grid);
            }
          }
          else if (damagedEntity is MyEntitySubpart)
          {
            type = MySession.MyHitIndicatorTarget.Grid;
            VRage.ModAPI.IMyEntity myEntity = damagedEntity;
            while (myEntity.Parent != null && myEntity.Parent is MyEntitySubpart)
              myEntity = myEntity.Parent;
            if (myEntity.Parent != null && myEntity.Parent.Parent is MyCubeGrid)
            {
              hitInfo.Position = myEntity.Parent.WorldAABB.Center;
              this.DoDamage(damage, hitInfo, customdata, myEntity.Parent.Parent, realHitEntity != null ? realHitEntity : damagedEntity);
            }
          }
          else if (damagedEntity is IMyDestroyableObject destroyableObject)
          {
            float integrity = destroyableObject.Integrity;
            destroyableObject.DoDamage(flag1 ? 0.0f : damage, MyDamageType.Bullet, true, new MyHitInfo?(hitInfo), this.m_weapon != null ? this.GetSubpartOwner(this.m_weapon).EntityId : 0L, realHitEntity != null ? realHitEntity.EntityId : 0L);
            if (character != null && !friendly && ((double) destroyableObject.Integrity <= 0.0 && (double) integrity > 0.0))
              type = MySession.MyHitIndicatorTarget.Kill;
          }
        }
      }
      if (this.m_supressHitIndicator || type == MySession.MyHitIndicatorTarget.None)
        return;
      MySession.HitIndicatorActivation(type, this.OwningPlayer);
    }

    private MySession.MyHitIndicatorTarget GetCharacterHitTarget(
      IMyDestroyableObject destroyable,
      bool headshot,
      bool friendly)
    {
      if (destroyable == null)
        return MySession.MyHitIndicatorTarget.None;
      if (friendly)
        return MySession.MyHitIndicatorTarget.Friend;
      return (double) destroyable.Integrity > 0.0 && headshot ? MySession.MyHitIndicatorTarget.Headshot : MySession.MyHitIndicatorTarget.Character;
    }

    private bool IsFriendly(MyCharacter character)
    {
      if (character != null)
      {
        long playerIdentityId = character.GetPlayerIdentityId();
        switch (MyIDModule.GetRelationPlayerPlayer(MySession.Static.Players.TryGetIdentityId(this.OwningPlayer, 0), playerIdentityId))
        {
          case MyRelationsBetweenPlayers.Self:
          case MyRelationsBetweenPlayers.Allies:
            return true;
        }
      }
      return false;
    }

    private MyEntity GetSubpartOwner(MyEntity entity)
    {
      if (entity == null)
        return (MyEntity) null;
      if (!(entity is MyEntitySubpart))
        return entity;
      MyEntity myEntity = entity;
      while (myEntity is MyEntitySubpart && myEntity != null)
        myEntity = myEntity.Parent;
      return myEntity ?? entity;
    }

    private static void GetSurfaceAndMaterial(
      VRage.ModAPI.IMyEntity entity,
      ref LineD line,
      ref Vector3D hitPosition,
      uint shapeKey,
      out MySurfaceImpactEnum surfaceImpact,
      out MyStringHash materialType)
    {
      switch (entity)
      {
        case MyVoxelBase self:
          materialType = VRage.Game.MyMaterialType.ROCK;
          surfaceImpact = MySurfaceImpactEnum.DESTRUCTIBLE;
          MyVoxelMaterialDefinition materialAt = self.GetMaterialAt(ref hitPosition);
          if (materialAt == null)
            break;
          materialType = materialAt.MaterialTypeNameHash;
          break;
        case MyCharacter _:
          surfaceImpact = MySurfaceImpactEnum.CHARACTER;
          materialType = VRage.Game.MyMaterialType.CHARACTER;
          if ((entity as MyCharacter).Definition.PhysicalMaterial == null)
            break;
          materialType = MyStringHash.GetOrCompute((entity as MyCharacter).Definition.PhysicalMaterial);
          break;
        case MyFloatingObject _:
          MyFloatingObject myFloatingObject = entity as MyFloatingObject;
          materialType = myFloatingObject.VoxelMaterial != null ? VRage.Game.MyMaterialType.ROCK : (myFloatingObject.ItemDefinition == null || !(myFloatingObject.ItemDefinition.PhysicalMaterial != MyStringHash.NullOrEmpty) ? VRage.Game.MyMaterialType.METAL : myFloatingObject.ItemDefinition.PhysicalMaterial);
          surfaceImpact = MySurfaceImpactEnum.METAL;
          break;
        case Sandbox.Game.WorldEnvironment.MyEnvironmentSector _:
          surfaceImpact = MySurfaceImpactEnum.METAL;
          materialType = VRage.Game.MyMaterialType.METAL;
          Sandbox.Game.WorldEnvironment.MyEnvironmentSector environmentSector = entity as Sandbox.Game.WorldEnvironment.MyEnvironmentSector;
          int itemFromShapeKey = environmentSector.GetItemFromShapeKey(shapeKey);
          if (itemFromShapeKey < 0 || environmentSector.DataView == null || (environmentSector.DataView.Items == null || environmentSector.DataView.Items.Count <= itemFromShapeKey))
            break;
          Sandbox.Game.WorldEnvironment.ItemInfo itemInfo = environmentSector.DataView.Items[itemFromShapeKey];
          MyRuntimeEnvironmentItemInfo environmentItemInfo = (MyRuntimeEnvironmentItemInfo) null;
          if (!environmentSector.EnvironmentDefinition.Items.TryGetValue<MyRuntimeEnvironmentItemInfo>((int) itemInfo.DefinitionIndex, out environmentItemInfo) || !environmentItemInfo.Type.Name.Equals("Tree"))
            break;
          surfaceImpact = MySurfaceImpactEnum.DESTRUCTIBLE;
          materialType = VRage.Game.MyMaterialType.WOOD;
          break;
        case MyTrees _:
          surfaceImpact = MySurfaceImpactEnum.DESTRUCTIBLE;
          materialType = VRage.Game.MyMaterialType.WOOD;
          break;
        case IMyHandheldGunObject<MyGunBase> _:
          surfaceImpact = MySurfaceImpactEnum.METAL;
          materialType = VRage.Game.MyMaterialType.METAL;
          MyGunBase gunBase = (entity as IMyHandheldGunObject<MyGunBase>).GunBase;
          if (gunBase == null || gunBase.WeaponProperties == null || gunBase.WeaponProperties.WeaponDefinition == null)
            break;
          materialType = gunBase.WeaponProperties.WeaponDefinition.PhysicalMaterial;
          break;
        default:
          surfaceImpact = MySurfaceImpactEnum.METAL;
          materialType = VRage.Game.MyMaterialType.METAL;
          MyCubeGrid myCubeGrid = entity as MyCubeGrid;
          MyCubeBlock myCubeBlock = entity as MyCubeBlock;
          MySlimBlock mySlimBlock = (MySlimBlock) null;
          if (myCubeBlock != null)
          {
            myCubeGrid = myCubeBlock.CubeGrid;
            mySlimBlock = myCubeBlock.SlimBlock;
          }
          else if (myCubeGrid != null)
          {
            mySlimBlock = myCubeGrid.GetTargetedBlock(hitPosition);
            if (mySlimBlock != null)
              myCubeBlock = mySlimBlock.FatBlock;
          }
          if (myCubeGrid == null || mySlimBlock == null)
            break;
          if (mySlimBlock.BlockDefinition.PhysicalMaterial != null && !mySlimBlock.BlockDefinition.PhysicalMaterial.Id.TypeId.IsNull)
          {
            materialType = MyStringHash.GetOrCompute(mySlimBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeName);
            break;
          }
          if (myCubeBlock == null)
            break;
          MyIntersectionResultLineTriangleEx? t = new MyIntersectionResultLineTriangleEx?();
          myCubeBlock.GetIntersectionWithLine(ref line, out t, IntersectionFlags.ALL_TRIANGLES);
          if (!t.HasValue)
            break;
          switch (myCubeBlock.ModelCollision.GetDrawTechnique(t.Value.Triangle.TriangleIndex))
          {
            case MyMeshDrawTechnique.HOLO:
              materialType = MyStringHash.GetOrCompute("Glass");
              return;
            case MyMeshDrawTechnique.GLASS:
              materialType = MyStringHash.GetOrCompute("Glass");
              return;
            case MyMeshDrawTechnique.SHIELD:
              materialType = MyStringHash.GetOrCompute("Shield");
              return;
            case MyMeshDrawTechnique.SHIELD_LIT:
              materialType = MyStringHash.GetOrCompute("ShieldLit");
              return;
            default:
              return;
          }
      }
    }

    private void StopEffect()
    {
    }

    private void PlayHitSound(
      MyStringHash materialType,
      VRage.ModAPI.IMyEntity entity,
      Vector3D position,
      MyStringHash thisType)
    {
      bool flag = MyProjectile.CollisionSoundsTimedCache.IsPlaceUsed(position, MyProjectile.CollisionSoundSpaceMapping, MySandboxGame.TotalSimulationTimeInMilliseconds);
      if (this.OwnerEntity is MyWarhead || flag)
        return;
      MyEntity entity1 = MySession.Static.ControlledEntity?.Entity;
      if (entity1 == null || Vector3D.DistanceSquared(entity1.PositionComp.GetPosition(), position) > MyProjectile.MaxImpactSoundDistanceSq)
        return;
      MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
      if (soundEmitter == null)
        return;
      MySoundPair collisionCue = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Hit, thisType, materialType);
      if (collisionCue == null || collisionCue == MySoundPair.Empty)
        collisionCue = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Start, thisType, materialType);
      if (collisionCue.SoundId.IsNull && entity is MyVoxelBase)
      {
        materialType = VRage.Game.MyMaterialType.ROCK;
        collisionCue = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Start, thisType, materialType);
      }
      if (collisionCue == null || collisionCue == MySoundPair.Empty)
        return;
      soundEmitter.Entity = (MyEntity) entity;
      soundEmitter.SetPosition(new Vector3D?(this.m_position));
      soundEmitter.SetVelocity(new Vector3?(Vector3.Zero));
      if (MySession.Static != null && MyFakes.ENABLE_NEW_SOUNDS && (MySession.Static.Settings.RealisticSound && entity == entity1))
      {
        Func<bool> canHear = (Func<bool>) (() => true);
        soundEmitter.StoppedPlaying += (Action<MyEntity3DSoundEmitter>) (e => e.EmitterMethods[0].Remove((Delegate) canHear, true));
        soundEmitter.EmitterMethods[0].Add((Delegate) canHear);
      }
      soundEmitter.PlaySound(collisionCue);
    }

    private void ApllyDeformationCubeGrid(float damage, Vector3D hitPosition, MyCubeGrid grid)
    {
      MatrixD matrix = grid.PositionComp.WorldMatrixNormalizedInv;
      Vector3D vector3D1 = Vector3D.Transform(hitPosition, matrix);
      Vector3D vector3D2 = Vector3D.TransformNormal(this.m_directionNormalized, matrix);
      float deformationOffset = MyFakes.DEFORMATION_PROJECTILE_OFFSET_RATIO * damage;
      float num1 = 0.011904f * damage;
      float num2 = 0.008928f * damage;
      float softAreaPlanar = MathHelper.Clamp(num1, grid.GridSize * 0.75f, grid.GridSize * 1.3f);
      float softAreaVertical = MathHelper.Clamp(num2, grid.GridSize * 0.9f, grid.GridSize * 1.3f);
      grid.Physics.ApplyDeformation(deformationOffset, softAreaPlanar, softAreaVertical, (Vector3) vector3D1, (Vector3) vector3D2, MyDamageType.Bullet);
    }

    public static void ApplyProjectileForce(
      VRage.ModAPI.IMyEntity entity,
      Vector3D intersectionPosition,
      Vector3 normalizedDirection,
      bool isPlayerShip,
      float impulse)
    {
      if (entity.Physics == null || !entity.Physics.Enabled || entity.Physics.IsStatic)
        return;
      if (entity is MyCharacter)
        impulse *= 100f;
      entity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, new Vector3?(normalizedDirection * impulse), new Vector3D?(intersectionPosition), new Vector3?(Vector3.Zero));
    }

    public void Draw()
    {
      if (this.m_state == MyProjectile.MyProjectileStateEnum.KILLED || !this.m_positionChecked)
        return;
      double num1 = Vector3D.Distance(this.m_position, this.m_origin);
      if (num1 <= 0.0)
        return;
      Vector3D vector3D = Vector3D.Normalize(this.m_position - (this.m_position - this.m_directionNormalized * 120.0 * 0.0166666675359011));
      double num2 = (double) this.LengthMultiplier * (double) this.m_projectileAmmoDefinition.ProjectileTrailScale * (MyParticlesManager.Paused ? 0.600000023841858 : (double) MyUtils.GetRandomFloat(0.6f, 0.8f));
      if (num1 < num2)
        num2 = num1;
      Vector3D origin = this.m_state == MyProjectile.MyProjectileStateEnum.ACTIVE || num1 * num1 >= this.m_velocity_Combined.LengthSquared() * 0.0166666675359011 * (double) MyProjectile.CHECK_INTERSECTION_INTERVAL ? this.m_position - num2 * vector3D : this.m_position - ((num1 - num2) * (double) MyUtils.GetRandomFloat(0.0f, 1f) + num2) * vector3D;
      if (Vector3D.DistanceSquared(origin, this.m_origin) < 4.0)
        return;
      float num3 = MyParticlesManager.Paused ? 1f : MyUtils.GetRandomFloat(1f, 2f);
      float thickness = (MyParticlesManager.Paused ? 0.2f : MyUtils.GetRandomFloat(0.2f, 0.3f)) * this.m_projectileAmmoDefinition.ProjectileTrailScale * MathHelper.Lerp(0.2f, 0.8f, MySector.MainCamera.Zoom.GetZoomLevel());
      float num4 = 1f;
      float num5 = 10f;
      if (num2 <= 0.0)
        return;
      if (this.m_projectileAmmoDefinition.ProjectileTrailMaterial != null)
        MyTransparentGeometry.AddLineBillboard(this.m_projectileTrailMaterialId, new Vector4(this.m_projectileAmmoDefinition.ProjectileTrailColor * num5, 1f), origin, (Vector3) vector3D, (float) num2, thickness);
      else
        MyTransparentGeometry.AddLineBillboard(MyProjectile.ID_PROJECTILE_TRAIL_LINE, new Vector4(this.m_projectileAmmoDefinition.ProjectileTrailColor * num3 * num5, 1f) * num4, origin, (Vector3) vector3D, (float) num2, thickness);
    }

    public void Close()
    {
      this.m_weapon = (MyEntity) null;
      this.OwnerEntity = (MyEntity) null;
      this.m_ignoredEntities = (MyEntity[]) null;
      if (this.m_safeZonesInTrajectory == null)
        return;
      this.m_safeZonesInTrajectory.Clear();
      MyProjectile.m_safeZonePool.Deallocate(this.m_safeZonesInTrajectory);
      this.m_safeZonesInTrajectory = (List<MySafeZone>) null;
    }

    private enum MyProjectileStateEnum : byte
    {
      ACTIVE,
      KILLED,
    }
  }
}
