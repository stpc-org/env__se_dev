// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyHandDrill
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Utils;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.ModAPI.Weapons;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  [MyEntityType(typeof (MyObjectBuilder_HandDrill), true)]
  public class MyHandDrill : MyEntity, IMyHandheldGunObject<MyToolBase>, IMyGunObject<MyToolBase>, IMyGunBaseUser, IMyHandDrill, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
  {
    private const float SPIKE_THRUST_DISTANCE_HALF = 0.03f;
    private const float SPIKE_THRUST_PERIOD_IN_SECONDS = 0.06f;
    private const float SPIKE_SLOWDOWN_TIME_IN_SECONDS = 0.5f;
    private const float SPIKE_MAX_ROTATION_SPEED = -25f;
    private int m_lastTimeDrilled;
    private MyDrillBase m_drillBase;
    private MyCharacter m_owner;
    private MyDefinitionId m_handItemDefId;
    private MyStringHash m_drillMat;
    private MyEntitySubpart m_spike;
    private Vector3 m_spikeBasePos;
    private float m_spikeRotationAngle;
    private float m_spikeThrustPosition;
    private int m_spikeLastUpdateTime;
    private MyOreDetectorComponent m_oreDetectorBase = new MyOreDetectorComponent();
    private MyEntity[] m_shootIgnoreEntities;
    protected Dictionary<MyShootActionEnum, bool> m_isActionDoubleClicked = new Dictionary<MyShootActionEnum, bool>();
    private float m_speedMultiplier = 1f;
    private MyHudNotification m_safezoneNotification;
    private bool m_firstDraw;
    private MyResourceSinkComponent m_sinkComp;
    private MyPhysicalItemDefinition m_physItemDef;
    private static MyDefinitionId m_physicalItemId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), "HandDrillItem");
    private bool m_tryingToDrill;
    private bool m_firstTimeHeatup = true;
    private bool m_objectInDrillingRange;

    public MyResourceSinkComponent SinkComp
    {
      get => this.m_sinkComp;
      set
      {
        if (this.Components.Contains(typeof (MyResourceSinkComponent)))
          this.Components.Remove<MyResourceSinkComponent>();
        this.Components.Add<MyResourceSinkComponent>(value);
        this.m_sinkComp = value;
      }
    }

    public float BackkickForcePerSecond => 0.0f;

    public float ShakeAmount
    {
      get => 2.5f;
      protected set
      {
      }
    }

    public MyCharacter Owner => this.m_owner;

    public long OwnerId => this.m_owner != null ? this.m_owner.EntityId : 0L;

    public long OwnerIdentityId => this.m_owner != null ? this.m_owner.GetPlayerIdentityId() : 0L;

    public bool EnabledInWorldRules => true;

    public MyObjectBuilder_PhysicalGunObject PhysicalObject { get; private set; }

    public bool IsShooting => this.m_drillBase.IsDrilling;

    public MyEntity DrilledEntity => this.m_drillBase.DrilledEntity;

    public bool CollectingOre => this.m_drillBase.CollectingOre;

    public bool ForceAnimationInsteadOfIK => false;

    public bool IsBlocking => false;

    public int ShootDirectionUpdateTime => 200;

    public bool NeedsShootDirectionWhileAiming => true;

    public float MaximumShotLength => Vector3.Distance((Vector3) this.m_drillBase.Sensor.Center, (Vector3) this.m_drillBase.Sensor.FrontPoint);

    public bool IsSkinnable => true;

    public MyHandDrill()
    {
      this.m_shootIgnoreEntities = new MyEntity[1]
      {
        (MyEntity) this
      };
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      MyHandDrill.m_physicalItemId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), "HandDrillItem");
      if (objectBuilder.SubtypeName != null && objectBuilder.SubtypeName.Length > 0)
        MyHandDrill.m_physicalItemId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), objectBuilder.SubtypeName + "Item");
      this.PhysicalObject = (MyObjectBuilder_PhysicalGunObject) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) MyHandDrill.m_physicalItemId);
      (this.PositionComp as MyPositionComponent).WorldPositionChanged = new Action<object>(this.WorldPositionChanged);
      this.m_handItemDefId = objectBuilder.GetId();
      MyHandItemDefinition handItemDefinition = MyDefinitionManager.Static.TryGetHandItemDefinition(ref this.m_handItemDefId);
      MyHandDrillDefinition handDrillDefinition = handItemDefinition as MyHandDrillDefinition;
      this.m_drillMat = handDrillDefinition.ToolMaterial;
      this.m_speedMultiplier = 1f / handDrillDefinition.SpeedMultiplier;
      this.m_drillBase = new MyDrillBase((MyEntity) this, "Smoke_HandDrillDust", "Smoke_HandDrillDustStones", "Collision_Sparks_HandDrill", (MyDrillSensorBase) new MyDrillSensorRayCast(-0.5f, 2.15f, (MyDefinitionBase) this.PhysicalItemDefinition), new MyDrillCutOut(1f, 0.35f * handDrillDefinition.DistanceMultiplier), 0.5f, -0.25f, 0.35f);
      this.m_drillBase.VoxelHarvestRatio = 0.009f * handDrillDefinition.HarvestRatioMultiplier * MySession.Static.Settings.HarvestRatioMultiplier;
      this.m_drillBase.ParticleOffset = handDrillDefinition.ParticleOffset;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderCompomentDrawDrillBase(this.m_drillBase));
      base.Init(objectBuilder);
      this.m_physItemDef = MyDefinitionManager.Static.GetPhysicalItemDefinition(MyHandDrill.m_physicalItemId);
      this.Init((StringBuilder) null, this.m_physItemDef.Model, (MyEntity) null, new float?());
      this.Render.CastShadows = true;
      this.Render.NeedsResolveCastShadow = false;
      this.m_spike = this.Subparts["Spike"];
      this.m_spikeBasePos = this.m_spike.PositionComp.LocalMatrixRef.Translation;
      this.m_drillBase.IgnoredEntities.Add((MyEntity) this);
      this.m_drillBase.UpdatePosition(this.PositionComp.WorldMatrixRef);
      this.PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase) objectBuilder.Clone();
      this.PhysicalObject.GunEntity.EntityId = this.EntityId;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.m_oreDetectorBase.DetectionRadius = 20f;
      this.m_oreDetectorBase.OnCheckControl = new MyOreDetectorComponent.CheckControlDelegate(this.OnCheckControl);
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(MyStringHash.GetOrCompute("Utility"), 4E-05f, (Func<float>) (() => !this.m_tryingToDrill ? 1E-06f : this.SinkComp.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      this.SinkComp = resourceSinkComponent;
      foreach (ToolSound toolSound in handItemDefinition.ToolSounds)
      {
        if (toolSound.type != null && toolSound.subtype != null && (toolSound.sound != null && toolSound.type.Equals("Main")))
        {
          if (toolSound.subtype.Equals("Idle"))
            this.m_drillBase.m_idleSoundLoop = new MySoundPair(toolSound.sound);
          if (toolSound.subtype.Equals("Soundset"))
            this.m_drillBase.m_drillMaterial = MyStringHash.GetOrCompute(toolSound.sound);
        }
      }
    }

    public bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
    {
      if ((double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeDrilled) < 1000.0 * (double) this.m_speedMultiplier)
      {
        status = MyGunStatusEnum.Cooldown;
        this.m_firstTimeHeatup = false;
        return false;
      }
      if (this.Owner == null)
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.Owner, MySafeZoneAction.Drilling))
      {
        status = MyGunStatusEnum.SafeZoneDenied;
        return false;
      }
      this.SinkComp.Update();
      if (!this.SinkComp.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
      {
        status = MyGunStatusEnum.OutOfPower;
        return false;
      }
      status = MyGunStatusEnum.OK;
      return true;
    }

    public void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      if (this.DoDrillAction(action == MyShootActionEnum.PrimaryAction) || !this.IsShooting || this.Owner == null)
        return;
      this.Owner.EndShoot(action);
    }

    public void BeginShoot(MyShootActionEnum action)
    {
    }

    public void EndShoot(MyShootActionEnum action)
    {
      this.m_drillBase.StopDrill();
      this.m_tryingToDrill = false;
      this.m_firstTimeHeatup = true;
      this.m_objectInDrillingRange = false;
      this.SinkComp.Update();
      this.m_lastTimeDrilled = MySandboxGame.TotalGamePlayTimeInMilliseconds - (int) (1000.0 * (double) this.m_speedMultiplier);
      this.m_isActionDoubleClicked[action] = false;
    }

    private bool DoDrillAction(bool collectOre)
    {
      this.m_tryingToDrill = true;
      this.SinkComp.Update();
      if (!this.SinkComp.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, 4E-05f))
      {
        this.m_tryingToDrill = false;
        return false;
      }
      this.m_lastTimeDrilled = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      MyDrillBase drillBase = this.m_drillBase;
      int num1 = collectOre ? 1 : 0;
      float speedMultiplier = this.m_speedMultiplier;
      int num2 = !this.m_firstTimeHeatup ? 1 : 0;
      double num3 = (double) speedMultiplier;
      this.m_objectInDrillingRange = drillBase.Drill(num1 != 0, num2 != 0, true, (float) num3);
      this.m_spikeLastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      return true;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.m_drillBase.UpdateAfterSimulation();
      if (this.IsShooting)
        this.CreateCollisionSparks();
      if (!this.m_tryingToDrill && (double) this.m_drillBase.AnimationMaxSpeedRatio <= 0.0)
        return;
      float num = (float) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_spikeLastUpdateTime) / 1000f;
      if (this.m_objectInDrillingRange && this.Owner != null && (this.Owner.ControllerInfo.IsLocallyControlled() && !MySession.Static.IsCameraUserAnySpectator()))
        this.m_drillBase.PerformCameraShake();
      this.m_spikeRotationAngle += (float) ((double) num * (double) this.m_drillBase.AnimationMaxSpeedRatio * -25.0);
      if ((double) this.m_spikeRotationAngle > 6.28318548202515)
        this.m_spikeRotationAngle -= 6.283185f;
      if ((double) this.m_spikeRotationAngle < 6.28318548202515)
        this.m_spikeRotationAngle += 6.283185f;
      this.m_spikeThrustPosition += (float) ((double) num * (double) this.m_drillBase.AnimationMaxSpeedRatio / 0.0599999986588955);
      if ((double) this.m_spikeThrustPosition > 1.0)
      {
        this.m_spikeThrustPosition -= 2f;
        if (this.Owner != null && this.m_objectInDrillingRange)
          this.Owner.WeaponPosition.AddBackkick(0.035f);
      }
      this.m_spikeLastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      Matrix localMatrix = Matrix.CreateRotationZ(this.m_spikeRotationAngle) * Matrix.CreateTranslation(this.m_spikeBasePos + Math.Abs(this.m_spikeThrustPosition) * Vector3.UnitZ * 0.03f);
      this.m_spike.PositionComp.SetLocalMatrix(ref localMatrix);
    }

    private void CreateCollisionSparks()
    {
      Vector3D center = this.m_drillBase.Sensor.Center;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      if (this.m_drillBase.DrilledEntity != null)
      {
        if (this.m_drillBase.DrilledEntity is MyCubeGrid drilledEntity)
        {
          if (drilledEntity.Immune)
          {
            ulong? steamId1 = this.Owner.ControllerInfo?.Controller?.Player?.Id.SteamId;
            ulong? steamId2 = MySession.Static?.LocalHumanPlayer?.Id.SteamId;
            if ((long) steamId1.GetValueOrDefault() == (long) steamId2.GetValueOrDefault() & steamId1.HasValue == steamId2.HasValue)
              MyHud.Notifications.Add(MyNotificationSingletons.GridIsImmune);
          }
          flag1 = true;
          Vector3D position = Vector3D.Transform(this.m_drillBase.ParticleOffset, this.WorldMatrix);
          if (this.m_drillBase.SparkEffect != null)
          {
            if (this.m_drillBase.SparkEffect.IsEmittingStopped)
              this.m_drillBase.SparkEffect.Play();
            this.m_drillBase.SparkEffect.WorldMatrix = MatrixD.CreateWorld(position, this.PositionComp.WorldMatrixRef.Forward, this.PositionComp.WorldMatrixRef.Up);
          }
          else
            MyParticlesManager.TryCreateParticleEffect("Collision_Sparks_HandDrill", MatrixD.CreateWorld(position, this.PositionComp.WorldMatrixRef.Forward, this.PositionComp.WorldMatrixRef.Up), out this.m_drillBase.SparkEffect);
        }
        else if (this.m_drillBase.DrilledEntity is MyVoxelBase drilledEntity)
        {
          flag2 = true;
          Vector3D position = Vector3D.Transform(this.m_drillBase.ParticleOffset, this.WorldMatrix);
          string collisionEffect = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, this.m_drillMat, drilledEntity.Physics.GetMaterialAt(this.m_drillBase.DrilledEntityPoint));
          this.m_drillBase.CurrentDustEffectName = !string.IsNullOrEmpty(collisionEffect) ? collisionEffect : "Smoke_HandDrillDustStones";
          if ((this.m_drillBase.DustParticles == null ? 1 : (this.m_drillBase.DustParticles.GetName() != this.m_drillBase.CurrentDustEffectName ? 1 : 0)) != 0 && this.m_drillBase.DustParticles != null)
          {
            this.m_drillBase.DustParticles.Stop(false);
            this.m_drillBase.DustParticles = (MyParticleEffect) null;
          }
          if (this.m_drillBase.DustParticles != null)
          {
            if (this.m_drillBase.DustParticles.IsEmittingStopped)
              this.m_drillBase.DustParticles.Play();
            this.m_drillBase.DustParticles.WorldMatrix = MatrixD.CreateWorld(position, this.PositionComp.WorldMatrixRef.Forward, this.PositionComp.WorldMatrixRef.Up);
          }
          else
            MyParticlesManager.TryCreateParticleEffect(this.m_drillBase.CurrentDustEffectName, MatrixD.CreateWorld(position, this.PositionComp.WorldMatrixRef.Forward, this.PositionComp.WorldMatrixRef.Up), out this.m_drillBase.DustParticles);
        }
        else if (this.m_drillBase.DrilledEntity is MyEnvironmentSector)
        {
          flag3 = true;
          Vector3D position = Vector3D.Transform(this.m_drillBase.ParticleOffset, this.WorldMatrix);
          string currentDustEffectName1 = this.m_drillBase.CurrentDustEffectName;
          this.m_drillBase.CurrentDustEffectName = "Tree_Drill";
          string currentDustEffectName2 = this.m_drillBase.CurrentDustEffectName;
          if (currentDustEffectName1 != currentDustEffectName2 && this.m_drillBase.DustParticles != null)
          {
            this.m_drillBase.DustParticles.Stop(false);
            this.m_drillBase.DustParticles = (MyParticleEffect) null;
          }
          if (this.m_drillBase.DustParticles != null)
          {
            if (this.m_drillBase.DustParticles.IsEmittingStopped)
              this.m_drillBase.DustParticles.Play();
            this.m_drillBase.DustParticles.WorldMatrix = MatrixD.CreateWorld(position, this.PositionComp.WorldMatrixRef.Forward, this.PositionComp.WorldMatrixRef.Up);
          }
          else
            MyParticlesManager.TryCreateParticleEffect(this.m_drillBase.CurrentDustEffectName, MatrixD.CreateWorld(position, this.PositionComp.WorldMatrixRef.Forward, this.PositionComp.WorldMatrixRef.Up), out this.m_drillBase.DustParticles);
        }
      }
      if (this.m_drillBase.SparkEffect != null && !flag1)
        this.m_drillBase.SparkEffect.StopEmitting();
      if (this.m_drillBase.DustParticles == null || flag2 || flag3)
        return;
      this.m_drillBase.DustParticles.StopEmitting();
    }

    public void WorldPositionChanged(object source)
    {
      if (this.m_owner == null)
        return;
      MatrixD identity = MatrixD.Identity;
      identity.Right = this.m_owner.WorldMatrix.Right;
      identity.Forward = this.m_owner.WeaponPosition.LogicalOrientationWorld;
      identity.Up = Vector3D.Normalize(identity.Right.Cross(identity.Forward));
      identity.Translation = this.m_owner.WeaponPosition.LogicalPositionWorld;
      this.m_drillBase.UpdatePosition(identity);
    }

    protected override void Closing()
    {
      base.Closing();
      this.m_drillBase.Close();
    }

    private Vector3 ComputeDrillSensorCenter()
    {
      MatrixD matrixD = this.PositionComp.WorldMatrixRef;
      Vector3D vector3D = matrixD.Forward * 1.29999995231628;
      matrixD = this.PositionComp.WorldMatrixRef;
      Vector3D translation = matrixD.Translation;
      return (Vector3) (vector3D + translation);
    }

    public void OnControlAcquired(MyCharacter owner)
    {
      this.m_owner = owner;
      if (owner != null)
        this.m_shootIgnoreEntities = new MyEntity[2]
        {
          (MyEntity) this,
          (MyEntity) owner
        };
      this.m_drillBase.OutputInventory = (MyInventory) null;
      this.m_drillBase.IgnoredEntities.Add((MyEntity) this.m_owner);
      this.m_firstDraw = true;
      if (this.Owner != MySession.Static.LocalCharacter)
        return;
      MyHud.BlockInfo.AddDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
    }

    public void OnControlReleased()
    {
      if (this.m_drillBase != null)
      {
        this.m_drillBase.IgnoredEntities.Remove((MyEntity) this.m_owner);
        this.m_drillBase.StopDrill();
        this.m_tryingToDrill = false;
        this.SinkComp.Update();
        this.m_drillBase.OutputInventory = (MyInventory) null;
      }
      if (this.m_owner != null && this.m_owner.ControllerInfo != null)
        this.m_oreDetectorBase.Clear();
      if (this.Owner == MySession.Static.LocalCharacter)
        MyHud.BlockInfo.RemoveDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
      this.m_owner = (MyCharacter) null;
    }

    public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate) => this.DrawHud(camera, playerId);

    public void DrawHud(IMyCameraController camera, long playerId)
    {
      if (!this.m_firstDraw)
        return;
      MyHud.Crosshair.Recenter();
      MyHud.BlockInfo.MissingComponentIndex = -1;
      MyHud.BlockInfo.DefinitionId = this.PhysicalItemDefinition.Id;
      MyHud.BlockInfo.BlockName = this.PhysicalItemDefinition.DisplayNameText;
      MyHud.BlockInfo.PCUCost = 0;
      MyHud.BlockInfo.BlockIcons = this.PhysicalItemDefinition.Icons;
      MyHud.BlockInfo.BlockIntegrity = 1f;
      MyHud.BlockInfo.CriticalIntegrity = 0.0f;
      MyHud.BlockInfo.CriticalComponentIndex = 0;
      MyHud.BlockInfo.OwnershipIntegrity = 0.0f;
      MyHud.BlockInfo.BlockBuiltBy = 0L;
      MyHud.BlockInfo.GridSize = MyCubeSize.Small;
      MyHud.BlockInfo.Components.Clear();
      MyHud.BlockInfo.SetContextHelp((MyDefinitionBase) this.PhysicalItemDefinition);
      this.m_firstDraw = false;
    }

    public MyDefinitionId DefinitionId => this.m_handItemDefId;

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      this.m_drillBase.Force2DSound = this.m_owner != null && this.m_owner.IsInFirstPersonView && this.m_owner == MySession.Static.LocalCharacter;
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      this.m_oreDetectorBase.Update(this.PositionComp.GetPosition(), this.EntityId);
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      this.UpdateSoundEmitter();
    }

    public void UpdateSoundEmitter()
    {
      Vector3 zero = Vector3.Zero;
      if (this.m_owner != null)
        this.m_owner.GetLinearVelocity(ref zero);
      this.m_drillBase.UpdateSoundEmitter(zero);
    }

    private bool OnCheckControl() => MySession.Static.ControlledEntity != null && (MyEntity) MySession.Static.ControlledEntity == this.Owner;

    public Vector3 DirectionToTarget(Vector3D target) => (Vector3) this.PositionComp.WorldMatrixRef.Forward;

    public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (this.Owner != MySession.Static.LocalCharacter)
        return;
      MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
    }

    public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (status != MyGunStatusEnum.SafeZoneDenied)
        return;
      if (this.m_safezoneNotification == null)
        this.m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_DrillingDisabled, 2000, "Red");
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_safezoneNotification);
    }

    public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (!this.IsShooting || this.Owner == null)
        return;
      this.Owner.EndShoot(action);
    }

    public int GetTotalAmmunitionAmount() => 0;

    public int GetAmmunitionAmount() => 0;

    public int GetMagazineAmount() => 0;

    public MyToolBase GunBase => (MyToolBase) null;

    public MyPhysicalItemDefinition PhysicalItemDefinition => this.m_physItemDef;

    public int CurrentAmmunition { set; get; }

    public int CurrentMagazineAmmunition { set; get; }

    public int CurrentMagazineAmount { set; get; }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_EntityBase objectBuilder = base.GetObjectBuilder(copy);
      objectBuilder.SubtypeName = this.m_handItemDefId.SubtypeName;
      return objectBuilder;
    }

    MyEntity[] IMyGunBaseUser.IgnoreEntities => this.m_shootIgnoreEntities;

    MyEntity IMyGunBaseUser.Weapon => (MyEntity) this;

    MyEntity IMyGunBaseUser.Owner => (MyEntity) this.m_owner;

    IMyMissileGunObject IMyGunBaseUser.Launcher => (IMyMissileGunObject) null;

    MyInventory IMyGunBaseUser.AmmoInventory => this.m_owner != null ? MyEntityExtensions.GetInventory(this.m_owner) : (MyInventory) null;

    MyDefinitionId IMyGunBaseUser.PhysicalItemId => new MyDefinitionId();

    MyInventory IMyGunBaseUser.WeaponInventory => (MyInventory) null;

    long IMyGunBaseUser.OwnerId => this.m_owner != null ? this.m_owner.ControllerInfo.ControllingIdentityId : 0L;

    string IMyGunBaseUser.ConstraintDisplayName => (string) null;

    public bool Reloadable => false;

    public bool IsReloading => false;

    public bool IsRecoiling => false;

    public bool NeedsReload => false;

    public bool SupressShootAnimation() => false;

    public bool ShouldEndShootOnPause(MyShootActionEnum action) => !this.m_isActionDoubleClicked.ContainsKey(action) || !this.m_isActionDoubleClicked[action];

    public bool CanDoubleClickToStick(MyShootActionEnum action) => true;

    public void DoubleClicked(MyShootActionEnum action) => this.m_isActionDoubleClicked[action] = true;

    public bool CanReload() => false;

    public bool Reload() => false;

    public float GetReloadDuration() => 0.0f;

    public Vector3D GetMuzzlePosition() => this.PositionComp.GetPosition();

    public void PlayReloadSound()
    {
    }

    public bool GetShakeOnAction(MyShootActionEnum action) => true;

    private class Sandbox_Game_Weapons_MyHandDrill\u003C\u003EActor : IActivator, IActivator<MyHandDrill>
    {
      object IActivator.CreateInstance() => (object) new MyHandDrill();

      MyHandDrill IActivator<MyHandDrill>.CreateInstance() => new MyHandDrill();
    }
  }
}
