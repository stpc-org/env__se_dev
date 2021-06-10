// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyExplosion
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Lights;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Game.WorldEnvironment.Modules;
using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Groups;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game
{
  internal class MyExplosion
  {
    private static readonly MyProjectileAmmoDefinition SHRAPNEL_DATA;
    private BoundingSphereD m_explosionSphere;
    private MyLight m_light;
    public int ElapsedMiliseconds;
    private Vector3 m_velocity;
    private MyParticleEffect m_explosionEffect;
    private HashSet<MySlimBlock> m_explodedBlocksInner = new HashSet<MySlimBlock>();
    private HashSet<MySlimBlock> m_explodedBlocksExact = new HashSet<MySlimBlock>();
    private HashSet<MySlimBlock> m_explodedBlocksOuter = new HashSet<MySlimBlock>();
    private MyGridExplosion m_gridExplosion = new MyGridExplosion();
    private MyExplosionInfo m_explosionInfo;
    private bool m_explosionTriggered;
    private MyGridExplosion m_damageInfo;
    public static bool DEBUG_EXPLOSIONS = false;
    private static readonly HashSet<MyVoxelBase> m_rootVoxelsToCutTmp = new HashSet<MyVoxelBase>();
    private static readonly List<MyVoxelBase> m_overlappingVoxelsTmp = new List<MyVoxelBase>();
    private static MySoundPair m_explPlayer = new MySoundPair("WepExplOnPlay", false);
    private static MySoundPair m_smMissileShip = new MySoundPair("WepSmallMissileExplShip", false);
    private static MySoundPair m_smMissileExpl = new MySoundPair("WepSmallMissileExpl", false);
    private static MySoundPair m_lrgWarheadExpl = new MySoundPair("WepLrgWarheadExpl", false);
    private static MySoundPair m_missileExpl = new MySoundPair("WepMissileExplosion", false);
    public static MySoundPair SmallWarheadExpl = new MySoundPair("WepSmallWarheadExpl", false);
    public static MySoundPair SmallPoofSound = new MySoundPair("PoofExplosionCat1", false);
    public static MySoundPair LargePoofSound = new MySoundPair("PoofExplosionCat3", false);

    static MyExplosion()
    {
      MyExplosion.SHRAPNEL_DATA = new MyProjectileAmmoDefinition();
      MyExplosion.SHRAPNEL_DATA.DesiredSpeed = 100f;
      MyExplosion.SHRAPNEL_DATA.SpeedVar = 0.0f;
      MyExplosion.SHRAPNEL_DATA.MaxTrajectory = 1000f;
      MyExplosion.SHRAPNEL_DATA.ProjectileHitImpulse = 10f;
      MyExplosion.SHRAPNEL_DATA.ProjectileMassDamage = 10f;
      MyExplosion.SHRAPNEL_DATA.ProjectileHealthDamage = 10f;
      MyExplosion.SHRAPNEL_DATA.ProjectileTrailColor = MyProjectilesConstants.GetProjectileTrailColorByType(MyAmmoType.HighSpeed);
      MyExplosion.SHRAPNEL_DATA.AmmoType = MyAmmoType.HighSpeed;
      MyExplosion.SHRAPNEL_DATA.ProjectileTrailScale = 0.1f;
      MyExplosion.SHRAPNEL_DATA.ProjectileOnHitEffectName = "Hit_BasicAmmoSmall";
    }

    public void Start(MyExplosionInfo explosionInfo)
    {
      this.m_explosionInfo = explosionInfo;
      this.ElapsedMiliseconds = 0;
      if (!Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      this.m_explosionInfo.CreateDebris = false;
      this.m_explosionInfo.PlaySound = false;
      this.m_explosionInfo.CreateParticleDebris = false;
      this.m_explosionInfo.CreateParticleEffect = false;
      this.m_explosionInfo.CreateDecals = false;
    }

    private void StartInternal()
    {
      this.m_velocity = this.m_explosionInfo.Velocity;
      this.m_explosionSphere = this.m_explosionInfo.ExplosionSphere;
      if (this.m_explosionInfo.PlaySound)
        this.PlaySound();
      this.m_light = MyLights.AddLight();
      if (this.m_light != null)
      {
        this.m_light.Start(this.m_explosionSphere.Center, MyExplosionsConstants.EXPLOSION_LIGHT_COLOR, Math.Min((float) this.m_explosionSphere.Radius * 8f, 120f), nameof (MyExplosion));
        this.m_light.Intensity = 20f;
        this.m_light.Range = Math.Min((float) this.m_explosionSphere.Radius * 3f, 120f);
      }
      if (this.m_explosionInfo.CreateParticleEffect)
        this.CreateParticleEffectInternal();
      if (this.m_explosionInfo.CreateDebris && this.m_explosionInfo.HitEntity != null)
      {
        BoundingBoxD fromSphere = BoundingBoxD.CreateFromSphere(new BoundingSphereD(this.m_explosionSphere.Center, this.m_explosionSphere.Radius * 0.699999988079071));
        MyDebris.Static.CreateExplosionDebris(ref this.m_explosionSphere, this.m_explosionInfo.HitEntity, ref fromSphere, 0.5f);
      }
      if (this.m_explosionInfo.CreateParticleDebris)
        this.GenerateExplosionParticles("Explosion_Debris", this.m_explosionSphere, 1f);
      this.m_explosionTriggered = false;
    }

    public void Clear()
    {
      this.m_damageInfo?.Clear();
      this.m_gridExplosion?.Clear();
      this.m_explosionInfo = new MyExplosionInfo();
    }

    private void PerformCameraShake(float intensityWeight)
    {
      if (MySector.MainCamera == null)
        return;
      float num1 = MySector.MainCamera.CameraShake.MaxShake * intensityWeight;
      Vector3D vector3D = MySector.MainCamera.Position - this.m_explosionSphere.Center;
      double num2 = 1.0 / vector3D.LengthSquared();
      float num3 = (float) (this.m_explosionSphere.Radius * this.m_explosionSphere.Radius * num2);
      if ((double) num3 <= 9.99999974737875E-06)
        return;
      MySector.MainCamera.CameraShake.AddShake(num1 * num3);
      MySector.MainCamera.CameraSpring.AddCurrentCameraControllerVelocity((Vector3) ((double) num1 * vector3D * num2));
    }

    private void CreateParticleEffectInternal()
    {
      string newParticlesName;
      switch (this.m_explosionInfo.ExplosionType)
      {
        case MyExplosionTypeEnum.MISSILE_EXPLOSION:
          newParticlesName = "Explosion_Missile";
          break;
        case MyExplosionTypeEnum.BOMB_EXPLOSION:
          newParticlesName = "Dummy";
          break;
        case MyExplosionTypeEnum.AMMO_EXPLOSION:
          newParticlesName = "Dummy";
          break;
        case MyExplosionTypeEnum.GRID_DEFORMATION:
          newParticlesName = "Dummy";
          break;
        case MyExplosionTypeEnum.GRID_DESTRUCTION:
          newParticlesName = "Grid_Destruction";
          break;
        case MyExplosionTypeEnum.WARHEAD_EXPLOSION_02:
          newParticlesName = "Explosion_Warhead_02";
          break;
        case MyExplosionTypeEnum.WARHEAD_EXPLOSION_15:
          newParticlesName = "Explosion_Warhead_15";
          break;
        case MyExplosionTypeEnum.WARHEAD_EXPLOSION_30:
          newParticlesName = "Explosion_Warhead_30";
          break;
        case MyExplosionTypeEnum.WARHEAD_EXPLOSION_50:
          newParticlesName = "Explosion_Warhead_50";
          break;
        case MyExplosionTypeEnum.CUSTOM:
          newParticlesName = this.m_explosionInfo.CustomEffect;
          break;
        default:
          throw new NotImplementedException();
      }
      this.GenerateExplosionParticles(newParticlesName, this.m_explosionSphere, this.m_explosionInfo.ParticleScale);
    }

    private void PlaySound()
    {
      MySoundPair cueByExplosionType = this.GetCueByExplosionType(this.m_explosionInfo.ExplosionType);
      if (cueByExplosionType != null && cueByExplosionType != MySoundPair.Empty)
      {
        MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
        if (soundEmitter != null)
        {
          soundEmitter.SetPosition(new Vector3D?(this.m_explosionSphere.Center));
          soundEmitter.Entity = this.m_explosionInfo.HitEntity;
          soundEmitter.PlaySound(cueByExplosionType);
        }
      }
      if (this.m_explosionInfo.HitEntity != MySession.Static.ControlledEntity)
        return;
      MyAudio.Static.PlaySound(MyExplosion.m_explPlayer.SoundId);
    }

    private void RemoveDestroyedObjects()
    {
      if (this.m_explosionInfo.ApplyForceAndDamage && (double) this.m_explosionInfo.Damage > 0.0)
      {
        this.ApplyExplosionOnVoxel(ref this.m_explosionInfo);
        BoundingSphereD explosionSphere = this.m_explosionSphere;
        explosionSphere.Radius *= 2.0;
        this.ApplyVolumetricExplosion(ref this.m_explosionInfo, Sandbox.Game.Entities.MyEntities.GetEntitiesInSphere(ref explosionSphere));
      }
      if (true || !this.m_explosionInfo.CreateShrapnels)
        return;
      for (int index = 0; index < 10; ++index)
      {
        MyProjectiles myProjectiles = MyProjectiles.Static;
        MyProjectileAmmoDefinition shrapnelData = MyExplosion.SHRAPNEL_DATA;
        MyEntity[] ignoreEntities;
        if (!(this.m_explosionInfo.HitEntity is MyWarhead))
          ignoreEntities = (MyEntity[]) null;
        else
          ignoreEntities = new MyEntity[1]
          {
            this.m_explosionInfo.HitEntity
          };
        Vector3 center = (Vector3) this.m_explosionSphere.Center;
        Vector3 zero = Vector3.Zero;
        Vector3 vector3Normalized = MyUtils.GetRandomVector3Normalized();
        myProjectiles.AddShrapnel(shrapnelData, ignoreEntities, center, zero, vector3Normalized, false, 1f, 1f, (MyEntity) null);
      }
    }

    private bool ApplyVolumetricExplosion(
      ref MyExplosionInfo m_explosionInfo,
      List<MyEntity> entities)
    {
      bool flag = false;
      BoundingSphereD explosionSphere = this.m_explosionSphere;
      explosionSphere.Radius *= 1.25;
      MyGridExplosion explosionDamageInfo = this.ApplyVolumetricExplosionOnGrid(ref m_explosionInfo, ref explosionSphere, entities);
      if ((m_explosionInfo.ExplosionFlags & MyExplosionFlags.APPLY_DEFORMATION) == MyExplosionFlags.APPLY_DEFORMATION)
      {
        this.m_damageInfo = explosionDamageInfo;
        this.m_damageInfo.ComputeDamagedBlocks();
        flag = this.m_damageInfo.GridWasHit;
      }
      if (m_explosionInfo.HitEntity is MyWarhead)
      {
        MySlimBlock slimBlock = (m_explosionInfo.HitEntity as MyWarhead).SlimBlock;
        if (!slimBlock.CubeGrid.BlocksDestructionEnabled)
        {
          slimBlock.CubeGrid.RemoveDestroyedBlock(slimBlock, 0L);
          foreach (MySlimBlock neighbour in slimBlock.Neighbours)
            neighbour.CubeGrid.Physics.AddDirtyBlock(neighbour);
          slimBlock.CubeGrid.Physics.AddDirtyBlock(slimBlock);
        }
      }
      this.ApplyVolumetricExplosionOnEntities(ref m_explosionInfo, entities, explosionDamageInfo);
      entities.Clear();
      return flag;
    }

    private void ApplyExplosionOnEntities(
      ref MyExplosionInfo m_explosionInfo,
      List<MyEntity> entities)
    {
      foreach (MyEntity entity in entities)
      {
        if (entity is IMyDestroyableObject)
        {
          float damage = !(entity is MyCharacter) ? m_explosionInfo.Damage : m_explosionInfo.PlayerDamage;
          if ((double) damage != 0.0)
            (entity as IMyDestroyableObject).DoDamage(damage, MyDamageType.Explosion, true, attackerId: (m_explosionInfo.OwnerEntity != null ? m_explosionInfo.OwnerEntity.EntityId : 0L));
        }
      }
    }

    private void ApplyVolumetricExplosionOnEntities(
      ref MyExplosionInfo m_explosionInfo,
      List<MyEntity> entities,
      MyGridExplosion explosionDamageInfo)
    {
      if (entities == null)
        return;
      float radius = (float) explosionDamageInfo.Sphere.Radius * 2f;
      float num1 = 1f / radius;
      float num2 = 1f / (float) explosionDamageInfo.Sphere.Radius;
      HkSphereShape? nullable = new HkSphereShape?();
      foreach (MyEntity entity in entities)
      {
        if (entity != null && !entity.IsPreview)
        {
          float num3 = (float) entity.PositionComp.WorldAABB.Distance(this.m_explosionSphere.Center);
          float num4 = num3 * num1;
          if ((double) num4 <= 1.0)
          {
            float num5 = (float) (1.0 - (double) num4 * (double) num4);
            if (entity is MyAmmoBase myAmmoBase)
              myAmmoBase.MarkForDestroy();
            else if (entity.Physics != null && entity.Physics.Enabled && (!entity.Physics.IsStatic && m_explosionInfo.ApplyForceAndDamage))
            {
              float num6 = m_explosionInfo.StrengthImpulse * 100f * (float) this.m_explosionSphere.Radius;
              m_explosionInfo.HitEntity = m_explosionInfo.HitEntity != null ? m_explosionInfo.HitEntity.GetBaseEntity() : (MyEntity) null;
              Vector3 vector3_1;
              if (m_explosionInfo.Direction.HasValue && m_explosionInfo.HitEntity != null && m_explosionInfo.HitEntity.GetTopMostParent((System.Type) null) == entity)
              {
                vector3_1 = m_explosionInfo.Direction.Value;
              }
              else
              {
                Vector3 vector3_2 = Vector3.Zero;
                if (entity is MyCubeGrid myCubeGrid)
                {
                  if (!nullable.HasValue)
                    nullable = new HkSphereShape?(new HkSphereShape(radius));
                  HkRigidBody rigidBody = myCubeGrid.Physics.RigidBody;
                  Matrix rigidBodyMatrix = rigidBody.GetRigidBodyMatrix();
                  Vector3 translation = rigidBodyMatrix.Translation;
                  Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(rigidBodyMatrix);
                  Vector3 cluster = (Vector3) myCubeGrid.Physics.WorldToCluster(this.m_explosionSphere.Center);
                  using (ClearToken<HkShapeCollision> penetrationsShapeShape = MyPhysics.GetPenetrationsShapeShape((HkShape) nullable.Value, ref cluster, ref Quaternion.Identity, rigidBody.GetShape(), ref translation, ref fromRotationMatrix))
                  {
                    float gridSize = myCubeGrid.GridSize;
                    MyGridShape shape = myCubeGrid.Physics.Shape;
                    int num7 = Math.Min(penetrationsShapeShape.List.Count, 100);
                    BoundingSphere sphere = new BoundingSphere((Vector3) (Vector3D.Transform(this.m_explosionSphere.Center, myCubeGrid.PositionComp.WorldMatrixNormalizedInv) / (double) gridSize), (float) this.m_explosionSphere.Radius / gridSize);
                    BoundingBoxI fromSphere = BoundingBoxI.CreateFromSphere(sphere);
                    fromSphere.Inflate(1);
                    int num8 = 0;
                    Vector3 zero = Vector3.Zero;
                    for (int index = 0; index < num7; ++index)
                    {
                      HkShapeCollision hkShapeCollision = penetrationsShapeShape.List[index];
                      if (hkShapeCollision.ShapeKeyCount != 0U && hkShapeCollision.ShapeKeyCount <= 1U)
                      {
                        Vector3I start;
                        Vector3I end;
                        shape.GetShapeBounds(hkShapeCollision.GetShapeKey(0), out start, out end);
                        if (start != end)
                        {
                          MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(start);
                          if (cubeBlock != null)
                          {
                            if (cubeBlock.FatBlock == null)
                            {
                              Vector3I.Clamp(ref start, ref fromSphere.Min, ref fromSphere.Max, out start);
                              Vector3I.Clamp(ref end, ref fromSphere.Min, ref fromSphere.Max, out end);
                              Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref start, ref end);
                              while (vector3IRangeIterator.IsValid())
                              {
                                Vector3I current = vector3IRangeIterator.Current;
                                if (sphere.Contains((Vector3) current) == ContainmentType.Contains)
                                {
                                  ++num8;
                                  zero += (Vector3) current;
                                }
                                vector3IRangeIterator.MoveNext();
                              }
                            }
                            else
                            {
                              ++num8;
                              zero += new Vector3(end + start) / 2f;
                            }
                          }
                        }
                        else
                        {
                          ++num8;
                          zero += (Vector3) start;
                        }
                      }
                    }
                    if (zero != Vector3.Zero)
                    {
                      Vector3 vector3_3 = zero / (float) num8;
                      vector3_2 = (Vector3) (myCubeGrid.GridIntegerToWorld((Vector3D) vector3_3) - this.m_explosionSphere.Center);
                    }
                  }
                }
                if (vector3_2 == Vector3.Zero)
                  vector3_2 = (Vector3) (entity.PositionComp.WorldAABB.Center - this.m_explosionSphere.Center);
                double num9 = (double) vector3_2.Normalize();
                vector3_1 = vector3_2;
              }
              bool flag1 = !(entity is MyCubeGrid) || MyExplosions.ShouldUseMassScaleForEntity(entity);
              float val1 = (float) ((double) num6 * (double) num5 / (flag1 ? 50.0 : 1.0));
              float mass = entity.Physics.Mass;
              float num10;
              if (flag1)
              {
                float num7 = MathHelper.Lerp(0.1f, 1f, 1f - MyMath.FastTanH(mass / 1000000f));
                num10 = val1 * (mass * num7);
              }
              else
                num10 = Math.Min(val1, mass);
              bool flag2 = true;
              if (entity is MyCubeGrid)
              {
                MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup((MyCubeGrid) entity);
                if (group != null)
                  flag2 = entity == group.GroupData.Root;
              }
              if ((double) num10 > 0.0 & flag2)
                entity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, new Vector3?(vector3_1 * num10), new Vector3D?(this.m_explosionSphere.Center), new Vector3?());
            }
            if (entity is MyEnvironmentSector)
            {
              int itemId = 0;
              MyEnvironmentSector environmentSector = (MyEnvironmentSector) entity;
              if (environmentSector != null && environmentSector.DataView != null && (environmentSector.DataView.Items != null && environmentSector.DataView.Items.Count > 0))
              {
                foreach (Sandbox.Game.WorldEnvironment.ItemInfo itemInfo in environmentSector.DataView.Items.ToArray())
                {
                  Vector3D vector3D = itemInfo.Position + environmentSector.PositionComp.GetPosition();
                  Vector3D hitnormal = explosionDamageInfo.Sphere.Center - vector3D;
                  if (hitnormal.Length() < explosionDamageInfo.Sphere.Radius)
                    ((MyEnvironmentSector) entity).GetModule<MyBreakableEnvironmentProxy>()?.BreakAt(itemId, new Vector3D(), hitnormal, (double) explosionDamageInfo.Damage);
                  ++itemId;
                }
              }
            }
            if (entity is IMyDestroyableObject || myAmmoBase != null || m_explosionInfo.ApplyForceAndDamage)
            {
              if (entity is MyCharacter myCharacter && myCharacter.IsUsing is MyCockpit isUsing)
              {
                if (explosionDamageInfo.DamagedBlocks.ContainsKey(isUsing.SlimBlock))
                {
                  float damageRemaining = explosionDamageInfo.DamageRemaining[isUsing.SlimBlock].DamageRemaining;
                  myCharacter.DoDamage(damageRemaining, MyDamageType.Explosion, true, m_explosionInfo.OwnerEntity != null ? m_explosionInfo.OwnerEntity.EntityId : 0L);
                }
              }
              else if (!(entity is MyCubeGrid) && entity is IMyDestroyableObject destroyableObject)
              {
                float num6 = num3 * num2;
                if ((double) num6 <= 1.0)
                {
                  float num7 = (float) (1.0 - (double) num6 * (double) num6);
                  float num8 = explosionDamageInfo.Damage * num7;
                  if (!MySession.Static.Settings.EnableFriendlyFire && myCharacter != null && (!m_explosionInfo.IgnoreFriendlyFireSetting && entity is MyCharacter victim) && (m_explosionInfo.OwnerEntity is MyCharacter ownerEntity && this.IsFriendly(victim, ownerEntity)))
                    num8 = 0.0f;
                  double num9 = (double) num8;
                  MyStringHash explosion = MyDamageType.Explosion;
                  MyHitInfo? hitInfo = new MyHitInfo?();
                  long attackerId = m_explosionInfo.OwnerEntity != null ? m_explosionInfo.OwnerEntity.EntityId : 0L;
                  destroyableObject.DoDamage((float) num9, explosion, true, hitInfo, attackerId);
                }
              }
            }
          }
        }
      }
      if (!nullable.HasValue)
        return;
      nullable.Value.Base.RemoveReference();
    }

    private void ApplyExplosionOnVoxel(ref MyExplosionInfo explosionInfo)
    {
      if (!explosionInfo.AffectVoxels || !MySession.Static.EnableVoxelDestruction || (!MySession.Static.HighSimulationQuality || (double) explosionInfo.Damage <= 0.0))
        return;
      MySession.Static.VoxelMaps.GetAllOverlappingWithSphere(ref this.m_explosionSphere, MyExplosion.m_overlappingVoxelsTmp);
      for (int index = MyExplosion.m_overlappingVoxelsTmp.Count - 1; index >= 0; --index)
        MyExplosion.m_rootVoxelsToCutTmp.Add(MyExplosion.m_overlappingVoxelsTmp[index].RootVoxel);
      MyExplosion.m_overlappingVoxelsTmp.Clear();
      foreach (MyVoxelBase voxelMap in MyExplosion.m_rootVoxelsToCutTmp)
      {
        bool createDebris = true;
        MyExplosion.CutOutVoxelMap((float) this.m_explosionSphere.Radius * explosionInfo.VoxelCutoutScale, explosionInfo.VoxelExplosionCenter, voxelMap, createDebris);
        voxelMap.RequestVoxelCutoutSphere(explosionInfo.VoxelExplosionCenter, (float) this.m_explosionSphere.Radius * explosionInfo.VoxelCutoutScale, createDebris, false);
      }
      MyExplosion.m_rootVoxelsToCutTmp.Clear();
    }

    public static void CutOutVoxelMap(
      float radius,
      Vector3D center,
      MyVoxelBase voxelMap,
      bool createDebris,
      bool damage = false)
    {
      MyShapeSphere sphereShape = new MyShapeSphere()
      {
        Center = center,
        Radius = radius
      };
      MyVoxelBase.OnCutOutResults results = (MyVoxelBase.OnCutOutResults) null;
      if (createDebris)
        results = (MyVoxelBase.OnCutOutResults) ((x, y, z) => MyExplosion.OnCutOutVoxelMap(x, y, sphereShape, voxelMap));
      voxelMap.CutOutShapeWithPropertiesAsync(results, (MyShape) sphereShape, applyDamageMaterial: damage);
    }

    private static void OnCutOutVoxelMap(
      float voxelsCountInPercent,
      MyVoxelMaterialDefinition voxelMaterial,
      MyShapeSphere sphereShape,
      MyVoxelBase voxelMap)
    {
      if ((double) voxelsCountInPercent <= 0.0 || voxelMaterial == null)
        return;
      BoundingSphereD explosionSphere = new BoundingSphereD(sphereShape.Center, (double) sphereShape.Radius);
      if ((double) MyRenderConstants.RenderQualityProfile.ExplosionDebrisCountMultiplier > 0.0)
      {
        if (voxelMaterial.DamagedMaterial != MyStringHash.NullOrEmpty)
          voxelMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition(voxelMaterial.DamagedMaterial.ToString());
        MyDebris.Static.CreateExplosionDebris(ref explosionSphere, voxelsCountInPercent, voxelMaterial, voxelMap);
      }
      MyParticleEffect effect;
      if (!MyParticlesManager.TryCreateParticleEffect("MaterialExplosion_Destructible", MatrixD.CreateTranslation(explosionSphere.Center), out effect))
        return;
      effect.UserRadiusMultiplier = (float) explosionSphere.Radius;
      effect.UserScale = 0.2f;
    }

    private MyGridExplosion ApplyVolumetricExplosionOnGrid(
      ref MyExplosionInfo explosionInfo,
      ref BoundingSphereD sphere,
      List<MyEntity> entities)
    {
      this.m_gridExplosion.Init(explosionInfo.ExplosionSphere, explosionInfo.Damage);
      Node = (MyCubeGrid) null;
      MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group) null;
      if (!MySession.Static.Settings.EnableTurretsFriendlyFire && explosionInfo.OriginEntity != 0L)
      {
        MyEntity entityById = Sandbox.Game.Entities.MyEntities.GetEntityById(explosionInfo.OriginEntity);
        if (entityById != null && entityById.GetTopMostParent((System.Type) null) is MyCubeGrid Node)
          group = MyCubeGridGroups.Static.Logical.GetGroup(Node);
      }
      foreach (MyEntity entity in entities)
      {
        if (entity != explosionInfo.ExcludedEntity && entity is MyCubeGrid Node && (Node.CreatePhysics && Node != Node) && (group == null || MyCubeGridGroups.Static.Logical.GetGroup(Node) != group))
        {
          this.m_gridExplosion.AffectedCubeGrids.Add(Node);
          float detectionBlockHalfSize = (float) ((double) Node.GridSize / 2.0 / 1.25);
          MatrixD invWorldGrid = Node.PositionComp.WorldMatrixInvScaled;
          BoundingSphereD sphere1 = new BoundingSphereD(sphere.Center, Math.Max(0.100000001490116, sphere.Radius - (double) Node.GridSize));
          BoundingSphereD sphere2 = new BoundingSphereD(sphere.Center, sphere.Radius);
          BoundingSphereD sphere3 = new BoundingSphereD(sphere.Center, sphere.Radius + (double) Node.GridSize * 0.5 * Math.Sqrt(3.0));
          Node.GetBlocksInsideSpheres(ref sphere1, ref sphere2, ref sphere3, this.m_explodedBlocksInner, this.m_explodedBlocksExact, this.m_explodedBlocksOuter, false, detectionBlockHalfSize, ref invWorldGrid);
          this.m_explodedBlocksInner.UnionWith((IEnumerable<MySlimBlock>) this.m_explodedBlocksExact);
          this.m_gridExplosion.AffectedCubeBlocks.UnionWith((IEnumerable<MySlimBlock>) this.m_explodedBlocksInner);
          foreach (MySlimBlock block in this.m_explodedBlocksOuter)
            Node.Physics.AddDirtyBlock(block);
          this.m_explodedBlocksInner.Clear();
          this.m_explodedBlocksExact.Clear();
          this.m_explodedBlocksOuter.Clear();
        }
      }
      return this.m_gridExplosion;
    }

    public void ApplyVolumetricDamageToGrid()
    {
      if (this.m_damageInfo != null)
        this.ApplyVolumetricDamageToGrid(this.m_damageInfo, this.m_explosionInfo.OwnerEntity != null ? this.m_explosionInfo.OwnerEntity.EntityId : 0L);
      this.m_damageInfo = (MyGridExplosion) null;
    }

    private void ApplyVolumetricDamageToGrid(MyGridExplosion damageInfo, long attackerId)
    {
      Dictionary<MySlimBlock, float> damagedBlocks = damageInfo.DamagedBlocks;
      HashSet<MySlimBlock> affectedCubeBlocks = damageInfo.AffectedCubeBlocks;
      HashSet<MyCubeGrid> affectedCubeGrids = damageInfo.AffectedCubeGrids;
      if (MyDebugDrawSettings.DEBUG_DRAW_VOLUMETRIC_EXPLOSION_COLORING)
      {
        foreach (MySlimBlock block in affectedCubeBlocks)
          block.CubeGrid.ChangeColorAndSkin(block, new Vector3?(new Vector3(0.66f, 1f, 1f)));
        foreach (KeyValuePair<MySlimBlock, float> keyValuePair in damagedBlocks)
        {
          float num = (float) (1.0 - (double) keyValuePair.Value / (double) damageInfo.Damage);
          keyValuePair.Key.CubeGrid.ChangeColorAndSkin(keyValuePair.Key, new Vector3?(new Vector3(num / 3f, 1f, 0.5f)));
        }
      }
      else
      {
        bool anyBeforeHandler = MyDamageSystem.Static.HasAnyBeforeHandler;
        foreach (KeyValuePair<MySlimBlock, float> keyValuePair in damagedBlocks)
        {
          MySlimBlock key = keyValuePair.Key;
          if (!key.CubeGrid.MarkedForClose && (key.FatBlock == null || !key.FatBlock.MarkedForClose) && (!key.IsDestroyed && key.CubeGrid.BlocksDestructionEnabled))
          {
            float amount = keyValuePair.Value;
            if (anyBeforeHandler && key.UseDamageSystem)
            {
              MyDamageInformation info = new MyDamageInformation(false, amount, MyDamageType.Explosion, attackerId);
              MyDamageSystem.Static.RaiseBeforeDamageApplied((object) key, ref info);
              if ((double) info.Amount > 0.0)
                amount = info.Amount;
              else
                continue;
            }
            if (affectedCubeBlocks.Contains(keyValuePair.Key) && !this.m_explosionInfo.KeepAffectedBlocks)
            {
              key.CubeGrid.RemoveDestroyedBlock(key, 0L);
            }
            else
            {
              if (key.FatBlock == null && (double) key.Integrity / (double) key.DeformationRatio < (double) amount || key.FatBlock == this.m_explosionInfo.HitEntity)
              {
                key.CubeGrid.RemoveDestroyedBlock(key, 0L);
              }
              else
              {
                if (key.FatBlock != null)
                  amount *= 7f;
                key.DoDamage(amount, MyDamageType.Explosion, true, attackerId: attackerId);
                if (!key.IsDestroyed)
                  key.CubeGrid.ApplyDestructionDeformation(key, 1f, new MyHitInfo?(), 0L);
              }
              foreach (MySlimBlock neighbour in key.Neighbours)
                neighbour.CubeGrid.Physics.AddDirtyBlock(neighbour);
              key.CubeGrid.Physics.AddDirtyBlock(key);
            }
          }
        }
      }
    }

    private MySoundPair GetCueByExplosionType(MyExplosionTypeEnum explosionType)
    {
      MySoundPair mySoundPair = (MySoundPair) null;
      switch (explosionType)
      {
        case MyExplosionTypeEnum.MISSILE_EXPLOSION:
          bool flag = false;
          if (this.m_explosionInfo.HitEntity is MyCubeGrid)
          {
            foreach (MySlimBlock block in (this.m_explosionInfo.HitEntity as MyCubeGrid).GetBlocks())
            {
              if (block.FatBlock is MyCockpit && (block.FatBlock as MyCockpit).Pilot == MySession.Static.ControlledEntity)
              {
                mySoundPair = MyExplosion.m_smMissileShip;
                flag = true;
                break;
              }
            }
          }
          if (!flag)
          {
            mySoundPair = MyExplosion.m_smMissileExpl;
            break;
          }
          break;
        case MyExplosionTypeEnum.BOMB_EXPLOSION:
          mySoundPair = MyExplosion.m_lrgWarheadExpl;
          break;
        case MyExplosionTypeEnum.WARHEAD_EXPLOSION_02:
        case MyExplosionTypeEnum.WARHEAD_EXPLOSION_15:
          mySoundPair = MyExplosion.SmallWarheadExpl;
          break;
        case MyExplosionTypeEnum.WARHEAD_EXPLOSION_30:
        case MyExplosionTypeEnum.WARHEAD_EXPLOSION_50:
          mySoundPair = MyExplosion.m_lrgWarheadExpl;
          break;
        case MyExplosionTypeEnum.CUSTOM:
          if (this.m_explosionInfo.CustomSound != null)
          {
            mySoundPair = this.m_explosionInfo.CustomSound;
            break;
          }
          break;
        default:
          mySoundPair = MyExplosion.m_missileExpl;
          break;
      }
      return mySoundPair;
    }

    private Vector4 GetSmutDecalRandomColor()
    {
      double randomFloat = (double) MyUtils.GetRandomFloat(0.2f, 0.3f);
      return new Vector4((float) randomFloat, (float) randomFloat, (float) randomFloat, 1f);
    }

    public bool Update()
    {
      if (this.ElapsedMiliseconds == 0)
        this.StartInternal();
      this.ElapsedMiliseconds += 16;
      if ((double) this.ElapsedMiliseconds < (double) MyExplosionsConstants.CAMERA_SHAKE_TIME_MS)
        this.PerformCameraShake((float) (1.0 - (double) this.ElapsedMiliseconds / (double) MyExplosionsConstants.CAMERA_SHAKE_TIME_MS));
      if (this.ElapsedMiliseconds > this.m_explosionInfo.ObjectsRemoveDelayInMiliseconds)
      {
        if (Sync.IsServer)
          this.RemoveDestroyedObjects();
        this.m_explosionInfo.ObjectsRemoveDelayInMiliseconds = int.MaxValue;
        this.m_explosionTriggered = true;
      }
      if (this.m_light != null)
      {
        this.m_light.Intensity = 20f * (float) (1.0 - (double) this.ElapsedMiliseconds / (double) this.m_explosionInfo.LifespanMiliseconds);
        this.m_light.UpdateLight();
      }
      if (this.m_explosionEffect != null)
      {
        this.m_explosionSphere.Center += this.m_velocity * 0.01666667f;
        this.m_explosionEffect.WorldMatrix = this.CalculateEffectMatrix(this.m_explosionSphere);
      }
      else if (this.ElapsedMiliseconds >= this.m_explosionInfo.LifespanMiliseconds && this.m_explosionTriggered)
      {
        if (MyExplosion.DEBUG_EXPLOSIONS)
          return true;
        this.Close();
        return false;
      }
      return this.m_explosionEffect == null || !this.m_explosionEffect.IsStopped;
    }

    public void DebugDraw()
    {
      if (!MyExplosion.DEBUG_EXPLOSIONS || this.m_light == null || (double) this.m_light.Intensity <= 0.0)
        return;
      BoundingSphereD boundingSphereD = new BoundingSphereD(this.m_light.Position, (double) this.m_light.Range);
      MyRenderProxy.DebugDrawSphere(boundingSphereD.Center, (float) boundingSphereD.Radius, Color.Red, depthRead: false);
    }

    public void Close()
    {
      if (this.m_light == null)
        return;
      MyLights.RemoveLight(this.m_light);
      this.m_light = (MyLight) null;
    }

    private MatrixD CalculateEffectMatrix(BoundingSphereD explosionSphere)
    {
      Vector3D vec = MySector.MainCamera.Position - explosionSphere.Center;
      return MatrixD.CreateTranslation(this.m_explosionSphere.Center + (!MyUtils.IsZero(vec) ? MyUtils.Normalize(vec) : (Vector3D) MySector.MainCamera.ForwardVector) * 0.899999976158142);
    }

    private void GenerateExplosionParticles(
      string newParticlesName,
      BoundingSphereD explosionSphere,
      float particleScale)
    {
      if (!MyParticlesManager.TryCreateParticleEffect(newParticlesName, this.CalculateEffectMatrix(explosionSphere), out this.m_explosionEffect))
        return;
      this.m_explosionEffect.OnDelete += (Action<MyParticleEffect>) (_param1 =>
      {
        this.m_explosionInfo.LifespanMiliseconds = 0;
        this.m_explosionEffect = (MyParticleEffect) null;
      });
      this.m_explosionEffect.UserScale = particleScale;
    }

    private bool IsFriendly(MyCharacter victim, MyCharacter shooter)
    {
      if (victim != null & shooter != null)
      {
        long playerIdentityId = victim.GetPlayerIdentityId();
        switch (MyIDModule.GetRelationPlayerPlayer(shooter.GetPlayerIdentityId(), playerIdentityId))
        {
          case MyRelationsBetweenPlayers.Self:
          case MyRelationsBetweenPlayers.Allies:
            return true;
        }
      }
      return false;
    }
  }
}
