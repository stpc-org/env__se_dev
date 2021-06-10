// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyDrillBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Game.WorldEnvironment.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons
{
  public sealed class MyDrillBase
  {
    public const float DISCARDING_RADIUS_MULTIPLIER = 3f;
    public static readonly float BASE_DRILL_DAMAGE = 20f;
    public MyInventory OutputInventory;
    public float VoxelHarvestRatio = 0.009f;
    private MyEntity m_drillEntity;
    private MyFixedPoint m_inventoryCollectionRatio;
    private MyDrillSensorBase m_sensor;
    public MyStringHash m_drillMaterial = MyStringHash.GetOrCompute("HandDrill");
    public MySoundPair m_idleSoundLoop = new MySoundPair("ToolPlayDrillIdle");
    private MyStringHash m_metalMaterial = MyStringHash.GetOrCompute("Metal");
    private MyStringHash m_rockMaterial = MyStringHash.GetOrCompute("Rock");
    private int m_lastContactTime;
    private int m_lastItemId;
    private string m_currentDustEffectName = "";
    public MyParticleEffect DustParticles;
    private MySlimBlock m_target;
    private string m_dustEffectName;
    private string m_dustEffectStonesName;
    private string m_sparksEffectName;
    private bool m_particleEffectsEnabled = true;
    private float m_animationMaxSpeedRatio;
    private float m_animationLastUpdateTime;
    private readonly float m_animationSlowdownTimeInSeconds;
    private float m_floatingObjectSpawnOffset;
    private float m_floatingObjectSpawnRadius;
    private bool m_previousDust;
    private bool m_previousSparks;
    private MyEntity m_drilledEntity;
    private MyEntity3DSoundEmitter m_soundEmitter;
    private bool m_initialHeatup = true;
    private MyDrillCutOut m_cutOut;
    private readonly float m_drillCameraMeanShakeIntensity = 0.85f;
    public static float DRILL_MAX_SHAKE = 2f;
    private bool force2DSound;
    private Action<float, string, string> m_onOreCollected;
    private string m_drilledVoxelMaterial;
    private bool m_drilledVoxelMaterialValid;
    public MyParticleEffect SparkEffect;
    private readonly List<MyPhysics.HitInfo> m_castList = new List<MyPhysics.HitInfo>();

    public HashSet<MyEntity> IgnoredEntities => this.m_sensor.IgnoredEntities;

    public string CurrentDustEffectName
    {
      get => this.m_currentDustEffectName;
      set => this.m_currentDustEffectName = value;
    }

    public MySoundPair CurrentLoopCueEnum { get; set; }

    public Vector3D ParticleOffset { get; set; }

    public bool IsDrilling { get; private set; }

    public MyEntity DrilledEntity
    {
      get => this.m_drilledEntity;
      private set
      {
        if (this.m_drilledEntity != null)
          this.m_drilledEntity.OnClose -= new Action<MyEntity>(this.OnDrilledEntityClose);
        this.m_drilledEntity = value;
        if (this.m_drilledEntity == null)
          return;
        this.m_drilledEntity.OnClose += new Action<MyEntity>(this.OnDrilledEntityClose);
      }
    }

    public bool CollectingOre { get; protected set; }

    public Vector3D DrilledEntityPoint { get; private set; }

    public float AnimationMaxSpeedRatio => this.m_animationMaxSpeedRatio;

    public MyDrillSensorBase Sensor => this.m_sensor;

    public MyDrillCutOut CutOut => this.m_cutOut;

    public bool Force2DSound
    {
      get => this.force2DSound;
      set
      {
        bool flag = this.m_soundEmitter != null && this.m_soundEmitter.IsPlaying;
        MySoundPair soundPair = this.m_soundEmitter.SoundPair;
        int num1 = value != this.force2DSound ? 1 : 0;
        this.force2DSound = value;
        int num2 = flag ? 1 : 0;
        if ((num1 & num2) == 0 || soundPair == null || this.m_soundEmitter == null)
          return;
        this.m_soundEmitter.PlaySound(soundPair, true, true, this.Force2DSound);
      }
    }

    public MyDrillBase(
      MyEntity drillEntity,
      string dustEffectName,
      string dustEffectStonesName,
      string sparksEffectName,
      MyDrillSensorBase sensor,
      MyDrillCutOut cutOut,
      float animationSlowdownTimeInSeconds,
      float floatingObjectSpawnOffset,
      float floatingObjectSpawnRadius,
      float inventoryCollectionRatio = 0.0f,
      Action<float, string, string> onOreCollected = null)
    {
      this.m_drillEntity = drillEntity;
      this.m_sensor = sensor;
      this.m_cutOut = cutOut;
      this.m_dustEffectName = dustEffectName;
      this.m_dustEffectStonesName = dustEffectStonesName;
      this.m_sparksEffectName = sparksEffectName;
      this.m_animationSlowdownTimeInSeconds = animationSlowdownTimeInSeconds;
      this.m_floatingObjectSpawnOffset = floatingObjectSpawnOffset;
      this.m_floatingObjectSpawnRadius = floatingObjectSpawnRadius;
      this.m_inventoryCollectionRatio = (MyFixedPoint) inventoryCollectionRatio;
      this.m_soundEmitter = new MyEntity3DSoundEmitter(this.m_drillEntity, true);
      this.m_onOreCollected = onOreCollected;
    }

    private bool DrillVoxel(
      MyDrillSensorBase.DetectionInfo entry,
      bool collectOre,
      bool performCutout,
      bool assignDamagedMaterial,
      ref MyStringHash targetMaterial)
    {
      MyVoxelBase entity = entry.Entity as MyVoxelBase;
      Vector3D worldPosition = entry.DetectionPoint;
      bool flag1 = false;
      if (!Sync.IsDedicated)
      {
        MyVoxelMaterialDefinition material = (MyVoxelMaterialDefinition) null;
        Vector3D center = this.m_cutOut.Sphere.Center;
        MatrixD worldMatrix = this.m_drillEntity.WorldMatrix;
        Vector3D forward = worldMatrix.Forward;
        Vector3D from = center - forward;
        worldMatrix = this.m_drillEntity.WorldMatrix;
        MyPhysics.CastRay(from, from + worldMatrix.Forward * (this.m_cutOut.Sphere.Radius + 1.0), this.m_castList, 28);
        bool flag2 = false;
        foreach (MyPhysics.HitInfo cast in this.m_castList)
        {
          if (cast.HkHitInfo.GetHitEntity() is MyVoxelBase)
          {
            worldPosition = cast.Position;
            material = entity.GetMaterialAt(ref worldPosition) ?? MyDefinitionManager.Static.GetVoxelMaterialDefinitions().FirstOrDefault<MyVoxelMaterialDefinition>();
            flag2 = true;
            break;
          }
        }
        if (!flag2 && this.m_drilledVoxelMaterialValid)
          material = MyDefinitionManager.Static.GetVoxelMaterialDefinition(this.m_drilledVoxelMaterial);
        if (material != null)
        {
          this.CollectingOre = collectOre;
          this.DrilledEntity = (MyEntity) entity;
          this.DrilledEntityPoint = worldPosition;
          targetMaterial = material.MaterialTypeNameHash;
          this.SpawnVoxelParticles(material);
          flag1 = true;
        }
      }
      if (Sync.IsServer & performCutout)
        this.TryDrillVoxels(entity, worldPosition, collectOre, assignDamagedMaterial);
      return flag1;
    }

    private bool DrillGrid(
      MyDrillSensorBase.DetectionInfo entry,
      bool performCutout,
      ref MyStringHash targetMaterial)
    {
      bool flag = false;
      MyCubeGrid entity = entry.Entity as MyCubeGrid;
      if (entity.Physics != null && entity.Physics.Enabled)
        flag = this.TryDrillBlocks(entity, entry.DetectionPoint, !Sync.IsServer || !performCutout, out targetMaterial);
      if (flag)
      {
        this.DrilledEntity = (MyEntity) entity;
        this.DrilledEntityPoint = entry.DetectionPoint;
        Vector3D position = Vector3D.Transform(this.ParticleOffset, this.m_drillEntity.WorldMatrix);
        MatrixD matrixD = this.m_drillEntity.WorldMatrix;
        if (this.m_drillEntity is MyFunctionalBlock drillEntity)
        {
          matrixD.Translation = position;
          matrixD = MatrixD.Multiply(matrixD, drillEntity.CubeGrid.PositionComp.WorldMatrixInvScaled);
        }
        this.CreateParticles(position, false, true, false, matrixD, this.m_drillEntity.Render.ParentIDs[0], targetMaterial);
      }
      return flag;
    }

    private bool DrillFloatingObject(MyDrillSensorBase.DetectionInfo entry)
    {
      MyFloatingObject entity = entry.Entity as MyFloatingObject;
      BoundingSphereD sphere = this.m_cutOut.Sphere;
      sphere.Radius *= 1.33000004291534;
      if (!entity.GetIntersectionWithSphere(ref sphere))
        return false;
      this.DrilledEntity = (MyEntity) entity;
      this.DrilledEntityPoint = entry.DetectionPoint;
      if (Sync.IsServer)
      {
        if (entity.Item.Content.TypeId == typeof (MyObjectBuilder_Ore))
        {
          MyEntity thisEntity = this.m_drillEntity == null || !this.m_drillEntity.HasInventory ? (MyEntity) null : this.m_drillEntity;
          if (thisEntity == null && this.m_drillEntity is MyHandDrill drillEntity)
            thisEntity = (MyEntity) drillEntity.Owner;
          if (thisEntity != null)
            MyEntityExtensions.GetInventory(thisEntity).TakeFloatingObject(entity);
        }
        else
          entity.DoDamage(70f, MyDamageType.Drill, true, this.m_drillEntity != null ? this.m_drillEntity.EntityId : 0L);
      }
      this.m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      return true;
    }

    protected bool IsFriendlyFireReduced(MyCharacter target)
    {
      if (MySession.Static.Settings.EnableFriendlyFire || target == null || this.m_drillEntity == null)
        return false;
      MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(this.m_drillEntity);
      ulong steamId = controllingPlayer != null ? controllingPlayer.Id.SteamId : 0UL;
      if (steamId != 0UL)
      {
        long playerIdentityId = target.GetPlayerIdentityId();
        switch (MyIDModule.GetRelationPlayerPlayer(MySession.Static.Players.TryGetIdentityId(steamId, 0), playerIdentityId))
        {
          case MyRelationsBetweenPlayers.Self:
          case MyRelationsBetweenPlayers.Allies:
            return true;
        }
      }
      return false;
    }

    private bool DrillCharacter(
      MyDrillSensorBase.DetectionInfo entry,
      out MyStringHash targetMaterial)
    {
      BoundingSphereD sphere = this.m_cutOut.Sphere;
      sphere.Radius *= 0.800000011920929;
      MyCharacter entity = entry.Entity as MyCharacter;
      float damage = this.IsFriendlyFireReduced(entity) ? 0.0f : MyDrillBase.BASE_DRILL_DAMAGE;
      MyHandDrill drillEntity = this.m_drillEntity as MyHandDrill;
      targetMaterial = MyStringHash.GetOrCompute(entity.Definition.PhysicalMaterial);
      if (entity.GetIntersectionWithSphere(ref sphere))
      {
        this.DrilledEntity = (MyEntity) entity;
        this.DrilledEntityPoint = entry.DetectionPoint;
        if (drillEntity != null && drillEntity.Owner == MySession.Static.LocalCharacter && (entity != MySession.Static.LocalCharacter && !entity.IsDead))
          MySession.Static.TotalDamageDealt += (uint) damage;
        if (Sync.IsServer)
          entity.DoDamage(damage, MyDamageType.Drill, true, this.m_drillEntity != null ? this.m_drillEntity.EntityId : 0L);
        this.m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        return true;
      }
      BoundingSphereD boundingSphereD;
      ref BoundingSphereD local = ref boundingSphereD;
      MatrixD matrixD = entity.PositionComp.WorldMatrixRef;
      Vector3D translation = matrixD.Translation;
      matrixD = entity.WorldMatrix;
      Vector3D vector3D = matrixD.Up * 1.25;
      Vector3D center = translation + vector3D;
      local = new BoundingSphereD(center, 0.600000023841858);
      if (!boundingSphereD.Intersects(sphere))
        return false;
      this.DrilledEntity = (MyEntity) entity;
      this.DrilledEntityPoint = entry.DetectionPoint;
      if (drillEntity != null && drillEntity.Owner == MySession.Static.LocalCharacter && (entity != MySession.Static.LocalCharacter && !entity.IsDead))
        MySession.Static.TotalDamageDealt += (uint) damage;
      if (Sync.IsServer)
        entity.DoDamage(damage, MyDamageType.Drill, true, this.m_drillEntity != null ? this.m_drillEntity.EntityId : 0L);
      this.m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      return true;
    }

    private bool DrillEnvironmentSector(
      MyDrillSensorBase.DetectionInfo entry,
      float speedMultiplier,
      out MyStringHash targetMaterial)
    {
      targetMaterial = MyStringHash.GetOrCompute("Wood");
      this.DrilledEntity = entry.Entity;
      this.DrilledEntityPoint = entry.DetectionPoint;
      if (Sync.IsServer)
      {
        if (this.m_lastItemId != entry.ItemId)
        {
          this.m_lastItemId = entry.ItemId;
          this.m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
        }
        if ((double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastContactTime) > 1500.0 * (double) speedMultiplier)
        {
          MyBreakableEnvironmentProxy module = (entry.Entity as MyEnvironmentSector).GetModule<MyBreakableEnvironmentProxy>();
          Vector3D vector3D = this.m_drillEntity.WorldMatrix.Forward + this.m_drillEntity.WorldMatrix.Right;
          vector3D.Normalize();
          double num1 = 10.0;
          float num2 = (float) (num1 * num1) * 100f;
          int itemId = entry.ItemId;
          Vector3D detectionPoint = entry.DetectionPoint;
          Vector3D hitnormal = vector3D;
          double impactEnergy = (double) num2;
          module.BreakAt(itemId, detectionPoint, hitnormal, impactEnergy);
          this.m_lastContactTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
          this.m_lastItemId = 0;
        }
      }
      return true;
    }

    public bool Drill(
      bool collectOre = true,
      bool performCutout = true,
      bool assignDamagedMaterial = false,
      float speedMultiplier = 1f)
    {
      bool flag1 = false;
      bool newDust = false;
      bool newSparks = false;
      MySoundPair cueEnum = (MySoundPair) null;
      if (this.m_drillEntity.Parent != null && this.m_drillEntity.Parent.Physics != null && !this.m_drillEntity.Parent.Physics.Enabled)
        return false;
      this.DrilledEntity = (MyEntity) null;
      this.CollectingOre = false;
      Dictionary<long, MyDrillSensorBase.DetectionInfo> cachedEntitiesInRange = this.m_sensor.CachedEntitiesInRange;
      MyStringHash targetMaterial = MyStringHash.NullOrEmpty;
      MyStringHash materialType2 = MyStringHash.NullOrEmpty;
      float num1 = float.MaxValue;
      bool flag2 = false;
      foreach (KeyValuePair<long, MyDrillSensorBase.DetectionInfo> keyValuePair in cachedEntitiesInRange)
      {
        flag1 = false;
        MyEntity entity = keyValuePair.Value.Entity;
        if (!entity.MarkedForClose)
        {
          switch (entity)
          {
            case MyCubeGrid _:
              if (this.DrillGrid(keyValuePair.Value, performCutout, ref targetMaterial))
              {
                int num2;
                newSparks = (num2 = 1) != 0;
                flag2 = num2 != 0;
                flag1 = num2 != 0;
                break;
              }
              break;
            case MyVoxelBase _:
              if (this.DrillVoxel(keyValuePair.Value, collectOre, performCutout, assignDamagedMaterial, ref targetMaterial))
              {
                flag1 = newDust = true;
                break;
              }
              break;
            case MyFloatingObject _:
              flag1 = this.DrillFloatingObject(keyValuePair.Value);
              break;
            case MyCharacter _:
              flag1 = this.DrillCharacter(keyValuePair.Value, out targetMaterial);
              break;
            case MyEnvironmentSector _:
              flag1 = this.DrillEnvironmentSector(keyValuePair.Value, speedMultiplier, out targetMaterial);
              break;
          }
          if (flag1)
          {
            float num2 = Vector3.DistanceSquared((Vector3) keyValuePair.Value.DetectionPoint, (Vector3) this.Sensor.Center);
            if (targetMaterial != MyStringHash.NullOrEmpty && (double) num2 < (double) num1)
            {
              materialType2 = targetMaterial;
              num1 = num2;
            }
          }
        }
      }
      if (materialType2 != MyStringHash.NullOrEmpty)
      {
        MySoundPair collisionCue = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Start, this.m_drillMaterial, materialType2);
        if (collisionCue == null || collisionCue == MySoundPair.Empty)
          materialType2 = !flag2 ? this.m_rockMaterial : this.m_metalMaterial;
        cueEnum = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Start, this.m_drillMaterial, materialType2);
      }
      if (cueEnum != null && cueEnum != MySoundPair.Empty)
        this.StartLoopSound(cueEnum, this.Force2DSound);
      else
        this.StartIdleSound(this.m_idleSoundLoop, this.Force2DSound);
      this.StartDrillingAnimation(false);
      this.CheckParticles(newDust, newSparks);
      return flag1;
    }

    public void StartDrillingAnimation(bool startSound)
    {
      if (startSound)
        this.StartIdleSound(this.m_idleSoundLoop, this.Force2DSound);
      if (this.IsDrilling)
        return;
      this.IsDrilling = true;
      this.m_animationLastUpdateTime = (float) MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    private void SpawnVoxelParticles(MyVoxelMaterialDefinition material)
    {
      Vector3D position = Vector3D.Transform(this.ParticleOffset, this.m_drillEntity.WorldMatrix);
      MatrixD matrixD = this.m_drillEntity.WorldMatrix;
      if (this.m_drillEntity is MyFunctionalBlock drillEntity)
      {
        matrixD.Translation = position;
        matrixD = MatrixD.Multiply(matrixD, drillEntity.CubeGrid.PositionComp.WorldMatrixInvScaled);
      }
      this.CreateParticles(position, true, false, true, matrixD, this.m_drillEntity.Render.ParentIDs[0], material.MaterialTypeNameHash);
    }

    private void CheckParticles(bool newDust, bool newSparks)
    {
      if (this.m_previousDust != newDust)
      {
        if (this.m_previousDust)
          this.StopDustParticles();
        this.m_previousDust = newDust;
      }
      if (this.m_previousSparks == newSparks)
        return;
      if (this.m_previousSparks)
        this.StopSparkParticles();
      this.m_previousSparks = newSparks;
    }

    public void Close()
    {
      this.IsDrilling = false;
      this.StopDustParticles();
      this.StopSparkParticles();
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.StopSound(true);
    }

    public void StopDrill()
    {
      this.m_drilledVoxelMaterial = "";
      this.m_drilledVoxelMaterialValid = false;
      this.IsDrilling = false;
      this.m_initialHeatup = true;
      this.StopDustParticles();
      this.StopSparkParticles();
      this.StopLoopSound();
    }

    public void UpdateAfterSimulation()
    {
      if (!this.IsDrilling && (double) this.m_animationMaxSpeedRatio > 1.40129846432482E-45)
      {
        this.m_animationMaxSpeedRatio -= (float) (((double) MySandboxGame.TotalGamePlayTimeInMilliseconds - (double) this.m_animationLastUpdateTime) / 1000.0) / this.m_animationSlowdownTimeInSeconds;
        if ((double) this.m_animationMaxSpeedRatio < 1.40129846432482E-45)
          this.m_animationMaxSpeedRatio = 0.0f;
      }
      if (this.IsDrilling)
      {
        this.m_animationMaxSpeedRatio += (float) (2.0 * (((double) MySandboxGame.TotalGamePlayTimeInMilliseconds - (double) this.m_animationLastUpdateTime) / 1000.0)) / this.m_animationSlowdownTimeInSeconds;
        if ((double) this.m_animationMaxSpeedRatio > 1.0)
          this.m_animationMaxSpeedRatio = 1f;
        if (this.m_sensor.CachedEntitiesInRange.Count == 0)
        {
          this.DrilledEntity = (MyEntity) null;
          this.CheckParticles(false, false);
        }
      }
      this.m_animationLastUpdateTime = (float) MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    public void UpdatePosition(MatrixD worldMatrix)
    {
      this.m_sensor.OnWorldPositionChanged(ref worldMatrix);
      this.m_cutOut.UpdatePosition(ref worldMatrix);
    }

    private void StartIdleSound(MySoundPair cuePair, bool force2D)
    {
      if (this.m_soundEmitter == null)
        return;
      if (!this.m_soundEmitter.IsPlaying)
      {
        this.m_soundEmitter.PlaySound(cuePair, force2D: force2D);
      }
      else
      {
        if (this.m_soundEmitter.SoundPair.Equals((object) cuePair))
          return;
        this.m_soundEmitter.StopSound(false);
        this.m_soundEmitter.PlaySound(cuePair, skipIntro: true, force2D: force2D);
      }
    }

    private void StartLoopSound(MySoundPair cueEnum, bool force2D)
    {
      if (this.m_soundEmitter == null)
        return;
      if (!this.m_soundEmitter.IsPlaying)
      {
        this.m_soundEmitter.PlaySound(cueEnum, force2D: force2D);
      }
      else
      {
        if (this.m_soundEmitter.SoundPair.Equals((object) cueEnum))
          return;
        if (this.m_soundEmitter.SoundPair.Equals((object) this.m_idleSoundLoop))
        {
          this.m_soundEmitter.StopSound(true);
          this.m_soundEmitter.PlaySound(cueEnum, force2D: force2D);
        }
        else
        {
          this.m_soundEmitter.StopSound(false);
          this.m_soundEmitter.PlaySound(cueEnum, skipIntro: true, force2D: force2D);
        }
      }
    }

    public void StopLoopSound()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.StopSound(false);
    }

    private void CreateParticles(
      Vector3D position,
      bool createDust,
      bool createSparks,
      bool createStones,
      MatrixD parent,
      uint parentId,
      MyStringHash materialName)
    {
      if (!this.m_particleEffectsEnabled || Sync.IsDedicated)
        return;
      if (createDust)
      {
        string collisionEffect = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, MyStringHash.GetOrCompute("ShipDrill"), materialName);
        if (!this.m_previousDust || this.DustParticles == null || !this.m_currentDustEffectName.Equals(collisionEffect))
        {
          this.CurrentDustEffectName = !string.IsNullOrEmpty(collisionEffect) ? collisionEffect : (createStones ? this.m_dustEffectStonesName : this.m_dustEffectName);
          if ((this.DustParticles == null ? 1 : (this.DustParticles.GetName() != this.m_currentDustEffectName ? 1 : 0)) != 0 && this.DustParticles != null)
          {
            this.DustParticles.Stop(false);
            this.DustParticles = (MyParticleEffect) null;
          }
          if (this.DustParticles == null)
            MyParticlesManager.TryCreateParticleEffect(this.m_currentDustEffectName, ref parent, ref position, parentId, out this.DustParticles);
        }
      }
      if (!createSparks || this.m_previousSparks && this.SparkEffect != null)
        return;
      if (this.SparkEffect != null)
        this.SparkEffect.Stop(false);
      MyParticlesManager.TryCreateParticleEffect(this.m_sparksEffectName, ref parent, ref position, parentId, out this.SparkEffect);
    }

    private void StopDustParticles()
    {
      if (this.DustParticles == null)
        return;
      this.DustParticles.Stop(false);
      this.DustParticles = (MyParticleEffect) null;
    }

    public void StopSparkParticles()
    {
      if (this.SparkEffect == null)
        return;
      this.SparkEffect.Stop(false);
      this.SparkEffect = (MyParticleEffect) null;
    }

    private bool TryDrillBlocks(
      MyCubeGrid grid,
      Vector3D worldPoint,
      bool onlyCheck,
      out MyStringHash blockMaterial)
    {
      MatrixD matrix = grid.PositionComp.WorldMatrixNormalizedInv;
      Vector3D vector3D1 = Vector3D.Transform(this.m_sensor.Center, matrix);
      Vector3D vector3D2 = Vector3D.Transform(this.m_sensor.FrontPoint, matrix);
      Vector3D vector3D3 = Vector3D.Transform(worldPoint, matrix);
      Vector3I pos1 = Vector3I.Round(vector3D1 / (double) grid.GridSize);
      MySlimBlock cubeBlock = grid.GetCubeBlock(pos1);
      if (cubeBlock == null)
      {
        Vector3I pos2 = Vector3I.Round(vector3D2 / (double) grid.GridSize);
        cubeBlock = grid.GetCubeBlock(pos2);
      }
      blockMaterial = cubeBlock == null ? MyStringHash.NullOrEmpty : (!(cubeBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeId == MyStringHash.NullOrEmpty) ? cubeBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeId : this.m_metalMaterial);
      bool flag1 = false;
      if (!onlyCheck && cubeBlock != null && (cubeBlock != null && cubeBlock.CubeGrid.BlocksDestructionEnabled))
      {
        MySlimBlock mySlimBlock = cubeBlock;
        float drillDamage = MyFakes.DRILL_DAMAGE;
        double num1 = (double) drillDamage;
        MyStringHash drill = MyDamageType.Drill;
        int num2 = Sync.IsServer ? 1 : 0;
        MyHitInfo? hitInfo = new MyHitInfo?();
        long attackerId = this.m_drillEntity != null ? this.m_drillEntity.EntityId : 0L;
        mySlimBlock.DoDamage((float) num1, drill, num2 != 0, hitInfo, attackerId, 0L);
        Vector3 localNormal = Vector3.Normalize(vector3D2 - vector3D1);
        if (cubeBlock.BlockDefinition.BlockTopology == MyBlockTopology.Cube)
        {
          float deformationOffset = MyFakes.DEFORMATION_DRILL_OFFSET_RATIO * drillDamage;
          float num3 = 0.011904f * drillDamage;
          float num4 = 0.008928f * drillDamage;
          float softAreaPlanar = MathHelper.Clamp(num3, grid.GridSize * 0.75f, grid.GridSize * 1.3f);
          float softAreaVertical = MathHelper.Clamp(num4, grid.GridSize * 0.9f, grid.GridSize * 1.3f);
          flag1 = grid.Physics.ApplyDeformation(deformationOffset, softAreaPlanar, softAreaVertical, (Vector3) vector3D3, localNormal, MyDamageType.Drill, attackerId: (this.m_drillEntity != null ? this.m_drillEntity.EntityId : 0L));
        }
      }
      this.m_target = flag1 ? (MySlimBlock) null : cubeBlock;
      bool flag2 = false;
      if (cubeBlock != null)
      {
        if (flag1)
        {
          BoundingSphereD sphere = this.m_cutOut.Sphere;
          BoundingBoxD fromSphere = BoundingBoxD.CreateFromSphere(sphere);
          MyDebris.Static.CreateExplosionDebris(ref sphere, (MyEntity) cubeBlock.CubeGrid, ref fromSphere, 0.3f);
        }
        flag2 = true;
      }
      return flag2;
    }

    private void TryDrillVoxels(
      MyVoxelBase voxels,
      Vector3D hitPosition,
      bool collectOre,
      bool applyDamagedMaterial)
    {
      if (voxels.GetOrePriority() == -1)
        return;
      if (!MyShipMiningSystem.DebugDisable && this.m_drillEntity is MyShipDrill drillEntity)
      {
        drillEntity.TryDrillVoxel(voxels, hitPosition, collectOre, applyDamagedMaterial);
      }
      else
      {
        MyShapeSphere myShapeSphere = new MyShapeSphere()
        {
          Center = this.m_cutOut.Sphere.Center,
          Radius = (float) this.m_cutOut.Sphere.Radius
        };
        if (!collectOre)
          myShapeSphere.Radius *= 3f;
        MyVoxelBase.OnCutOutResults results = (MyVoxelBase.OnCutOutResults) ((x, y, z) => this.OnDrillResults(z, hitPosition, collectOre));
        voxels.CutOutShapeWithPropertiesAsync(results, (MyShape) myShapeSphere, Sync.IsServer, applyDamageMaterial: applyDamagedMaterial);
      }
    }

    internal void OnDrillResults(
      Dictionary<MyVoxelMaterialDefinition, int> materials,
      Vector3D hitPosition,
      bool collectOre)
    {
      int num = 0;
      this.m_drilledVoxelMaterial = "";
      this.m_drilledVoxelMaterialValid = true;
      foreach (KeyValuePair<MyVoxelMaterialDefinition, int> material in materials)
      {
        int removedAmount = material.Value;
        if (collectOre && !this.TryHarvestOreMaterial(material.Key, (Vector3) hitPosition, removedAmount, false))
          removedAmount = 0;
        if (removedAmount > num)
        {
          num = removedAmount;
          this.m_drilledVoxelMaterial = material.Key.DamagedMaterial != MyStringHash.NullOrEmpty ? material.Key.DamagedMaterial.ToString() : material.Key.Id.SubtypeName;
        }
      }
    }

    public void PerformCameraShake(float multiplier = 1f)
    {
      if (MySector.MainCamera == null)
        return;
      MySector.MainCamera.CameraShake.AddShake(MathHelper.Clamp((float) -Math.Log(MyRandom.Instance.NextDouble()) * this.m_drillCameraMeanShakeIntensity * MyDrillBase.DRILL_MAX_SHAKE, 0.0f, MyDrillBase.DRILL_MAX_SHAKE) * multiplier);
    }

    private bool TryHarvestOreMaterial(
      MyVoxelMaterialDefinition material,
      Vector3 hitPosition,
      int removedAmount,
      bool onlyCheck)
    {
      if (string.IsNullOrEmpty(material.MinedOre))
        return false;
      if (!onlyCheck)
      {
        MyObjectBuilder_Ore newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>(material.MinedOre);
        newObject.MaterialTypeName = new MyStringHash?(material.Id.SubtypeId);
        float num = (float) ((double) removedAmount / (double) byte.MaxValue * 1.0) * this.VoxelHarvestRatio * material.MinedOreRatio;
        if (!MySession.Static.AmountMined.ContainsKey(material.MinedOre))
          MySession.Static.AmountMined[material.MinedOre] = (MyFixedPoint) 0;
        MySession.Static.AmountMined[material.MinedOre] += (MyFixedPoint) num;
        MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) newObject);
        MyFixedPoint amountItems1 = (MyFixedPoint) (num / physicalItemDefinition.Volume);
        MyFixedPoint maxAmountPerDrop = (MyFixedPoint) (float) (0.150000005960464 / (double) physicalItemDefinition.Volume);
        if (this.OutputInventory != null)
        {
          MyFixedPoint b = amountItems1 * ((MyFixedPoint) 1 - this.m_inventoryCollectionRatio);
          MyFixedPoint amountItems2 = MyFixedPoint.Min(maxAmountPerDrop * 10 - (MyFixedPoint) 0.001, b);
          this.OutputInventory.AddItems(amountItems1 * this.m_inventoryCollectionRatio - amountItems2, (MyObjectBuilder_Base) newObject);
          this.SpawnOrePieces(amountItems2, maxAmountPerDrop, hitPosition, (MyObjectBuilder_PhysicalObject) newObject, material);
          if (this.m_onOreCollected != null)
            this.m_onOreCollected((float) amountItems2, newObject.TypeId.ToString(), newObject.SubtypeId.ToString());
        }
        else
          this.SpawnOrePieces(amountItems1, maxAmountPerDrop, hitPosition, (MyObjectBuilder_PhysicalObject) newObject, material);
      }
      return true;
    }

    private void SpawnOrePieces(
      MyFixedPoint amountItems,
      MyFixedPoint maxAmountPerDrop,
      Vector3 hitPosition,
      MyObjectBuilder_PhysicalObject oreObjBuilder,
      MyVoxelMaterialDefinition voxelMaterial)
    {
      if (!Sync.IsServer)
        return;
      Vector3 forward = Vector3.Normalize(this.m_sensor.FrontPoint - this.m_sensor.Center);
      BoundingSphere boundingSphere = new BoundingSphere(hitPosition - forward * this.m_floatingObjectSpawnRadius, this.m_floatingObjectSpawnRadius);
      while (amountItems > (MyFixedPoint) 0)
      {
        float randomFloat = MyRandom.Instance.GetRandomFloat((float) maxAmountPerDrop / 10f, (float) maxAmountPerDrop);
        MyFixedPoint amount = (MyFixedPoint) MathHelper.Min((float) amountItems, randomFloat);
        amountItems -= amount;
        MyPhysicalInventoryItem physicalInventoryItem = new MyPhysicalInventoryItem(amount, oreObjBuilder);
        if (MyFakes.ENABLE_DRILL_ROCKS)
          MyFloatingObjects.Spawn(physicalInventoryItem, (BoundingSphereD) boundingSphere, (MyPhysicsComponentBase) null, voxelMaterial, (Action<MyEntity>) (entity =>
          {
            entity.Physics.LinearVelocity = MyUtils.GetRandomVector3HemisphereNormalized(forward) * MyUtils.GetRandomFloat(1.5f, 4f);
            entity.Physics.AngularVelocity = MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(4f, 8f);
          }));
      }
    }

    public void DebugDraw()
    {
      this.m_sensor.DebugDraw();
      MyRenderProxy.DebugDrawSphere((Vector3D) (Vector3) this.m_cutOut.Sphere.Center, (float) this.m_cutOut.Sphere.Radius, Color.Red, 0.6f);
    }

    private Vector3 ComputeDebrisDirection()
    {
      Vector3D vector3D = this.m_sensor.Center - this.m_sensor.FrontPoint;
      vector3D.Normalize();
      return (Vector3) vector3D;
    }

    public void UpdateSoundEmitter(Vector3 velocity)
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.SetVelocity(new Vector3?(velocity));
      this.m_soundEmitter.Update();
    }

    private void OnDrilledEntityClose(MyEntity entity) => this.DrilledEntity = (MyEntity) null;

    public struct Sounds
    {
      public MySoundPair IdleLoop;
      public MySoundPair MetalLoop;
      public MySoundPair RockLoop;
    }
  }
}
