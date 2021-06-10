// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyAutomaticRifleGun
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Weapons;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
  [StaticEventOwner]
  [MyEntityType(typeof (MyObjectBuilder_AutomaticRifle), true)]
  public class MyAutomaticRifleGun : MyEntity, IMyHandheldGunObject<MyGunBase>, IMyGunObject<MyGunBase>, IMyGunBaseUser, IMyEventProxy, IMyEventOwner, IMyAutomaticRifleGun, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyMissileGunObject, IMySyncedEntity
  {
    public static string NAME_SUBPART_MAGAZINE = "magazine";
    public static string NAME_DUMMY_MAGAZINE = "subpart_magazine";
    private static readonly string STATE_KEY_STANDING = "Standing";
    private static readonly string STATE_KEY_WALKING = "Walking";
    private static readonly string STATE_KEY_RUNNING = "Running";
    private static readonly string STATE_KEY_CROUCHING = "Crouching";
    private static readonly string STATE_KEY_AIMING = "Aiming";
    public static readonly int BASE_SHOOT_DIRECTION_UPDATE_TIME = 200;
    public static float RIFLE_MAX_SHAKE = 0.5f;
    public static float RIFLE_FOV_SHAKE = 0.0065f;
    public static float MAX_HORIZONTAL_RECOIL_DEVIATION = 10f;
    public static readonly float HORIZONTAL_RECOIL_OFFSET = 0.5f;
    public static readonly float RECOIL_RETURN_SPEED = 0.106f;
    public static readonly float RECOIL_SPEED = 1f;
    private readonly float RECOIL_COMPENSATION_MULTIPLIER = 5f;
    private static float TO_RAD = 0.01744444f;
    private int m_lastRecoil;
    private int m_lastTimeShoot;
    private float m_startingVertAngle;
    private float m_nextVertAngle;
    private float m_currentVertAngle;
    private float m_recoilTimer;
    private float m_backRecoilTimer;
    private float m_lastVertRecoilDiff;
    private float m_horizontalAngle;
    private float m_horizontalAngleOriginal;
    private bool m_isRecoiling;
    private MyRandom random = new MyRandom();
    private int m_lastDirectionChangeAnnounce;
    private MyParticleEffect m_smokeEffect;
    private bool m_firstDraw;
    private MyGunBase m_gunBase;
    private MyDefinitionId m_handItemDefId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalGunObject), "AutomaticRifleGun");
    private MyPhysicalItemDefinition m_physicalItemDef;
    private MyCharacter m_owner;
    protected Dictionary<MyShootActionEnum, bool> m_isActionDoubleClicked = new Dictionary<MyShootActionEnum, bool>();
    private int m_shootingCounter;
    private bool m_canZoom = true;
    private MyEntity3DSoundEmitter m_soundEmitter;
    private MyEntity3DSoundEmitter m_reloadSoundEmitter;
    private int m_shotsFiredInBurst;
    private MyHudNotification m_outOfAmmoNotification;
    private MyHudNotification m_safezoneNotification;
    private bool m_isAfterReleaseFire;
    private MyWeaponDefinition m_definition;
    private int m_weaponEquipDelay;
    private MyEntity[] m_shootIgnoreEntities;
    private int m_shootDirectionUpdateTime = MyAutomaticRifleGun.BASE_SHOOT_DIRECTION_UPDATE_TIME;
    private MyTimeSpan m_reloadEndTime;
    private bool m_isReloading;

    public int LastTimeShoot => this.m_lastTimeShoot;

    public MyCharacter Owner => this.m_owner;

    public long OwnerId => this.m_owner != null ? this.m_owner.EntityId : 0L;

    public long OwnerIdentityId => this.m_owner != null ? this.m_owner.GetPlayerIdentityId() : 0L;

    public bool IsShooting { get; private set; }

    public int ShootDirectionUpdateTime
    {
      get => this.m_shootDirectionUpdateTime;
      private set
      {
        if (this.m_shootDirectionUpdateTime == value)
          return;
        if (value >= 0)
          this.m_shootDirectionUpdateTime = value;
        else
          this.m_shootDirectionUpdateTime = MyAutomaticRifleGun.BASE_SHOOT_DIRECTION_UPDATE_TIME;
      }
    }

    public bool NeedsShootDirectionWhileAiming => false;

    public float MaximumShotLength => this.m_gunBase.CurrentAmmoDefinition.MaxTrajectory;

    public bool ForceAnimationInsteadOfIK => false;

    public bool IsBlocking => false;

    public MyObjectBuilder_PhysicalGunObject PhysicalObject { get; set; }

    public SyncType SyncType { get; set; }

    public bool IsSkinnable => true;

    public MyAutomaticRifleGun()
    {
      this.m_shootIgnoreEntities = new MyEntity[1]
      {
        (MyEntity) this
      };
      this.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME;
      this.Render.NeedsDraw = true;
      this.m_gunBase = new MyGunBase();
      this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this);
      this.m_reloadSoundEmitter = new MyEntity3DSoundEmitter((MyEntity) this);
      (this.PositionComp as MyPositionComponent).WorldPositionChanged = new Action<object>(this.WorldPositionChanged);
      this.Render = (MyRenderComponentBase) new MyRenderComponentAutomaticRifle();
      this.SyncType = SyncHelpers.Compose((object) this);
      this.SyncType.Append((object) this.m_gunBase);
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      if (objectBuilder.SubtypeName != null && objectBuilder.SubtypeName.Length > 0)
        this.m_handItemDefId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AutomaticRifle), objectBuilder.SubtypeName);
      MyObjectBuilder_AutomaticRifle builderAutomaticRifle = (MyObjectBuilder_AutomaticRifle) objectBuilder;
      MyHandItemDefinition handItemDefinition = MyDefinitionManager.Static.TryGetHandItemDefinition(ref this.m_handItemDefId);
      this.m_physicalItemDef = MyDefinitionManager.Static.GetPhysicalItemForHandItem(this.m_handItemDefId);
      MyDefinitionId myDefinitionId = !(this.m_physicalItemDef is MyWeaponItemDefinition) ? new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_WeaponDefinition), "AutomaticRifleGun") : (this.m_physicalItemDef as MyWeaponItemDefinition).WeaponDefinitionId;
      this.m_gunBase.Init(builderAutomaticRifle.GunBase, myDefinitionId, (IMyGunBaseUser) this);
      base.Init(objectBuilder);
      this.Init(MyTexts.Get(MySpaceTexts.DisplayName_Rifle), this.m_physicalItemDef.Model, (MyEntity) null, new float?());
      this.m_gunBase.LoadDummies(MyModels.GetModelOnlyDummies(this.m_physicalItemDef.Model).Dummies);
      if (!this.m_gunBase.HasDummies)
        this.m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, Matrix.CreateTranslation(handItemDefinition.MuzzlePosition));
      this.PhysicalObject = (MyObjectBuilder_PhysicalGunObject) MyObjectBuilderSerializer.CreateNewObject(this.m_physicalItemDef.Id.TypeId, this.m_physicalItemDef.Id.SubtypeName);
      this.PhysicalObject.GunEntity = (MyObjectBuilder_EntityBase) builderAutomaticRifle.Clone();
      this.PhysicalObject.GunEntity.EntityId = this.EntityId;
      MyWeaponDefinition definition;
      MyDefinitionManager.Static.TryGetWeaponDefinition(myDefinitionId, out definition);
      this.m_definition = definition;
      if (this.m_definition == null)
        return;
      this.ShootDirectionUpdateTime = this.m_definition.ShootDirectionUpdateTime;
      this.m_weaponEquipDelay = MySandboxGame.TotalGamePlayTimeInMilliseconds + (int) ((double) this.m_definition.EquipDuration * 1000.0);
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      MyObjectBuilder_AutomaticRifle objectBuilder = (MyObjectBuilder_AutomaticRifle) base.GetObjectBuilder(copy);
      objectBuilder.SubtypeName = this.DefinitionId.SubtypeName;
      objectBuilder.GunBase = this.m_gunBase.GetObjectBuilder();
      objectBuilder.CurrentAmmo = this.CurrentMagazineAmmunition;
      return (MyObjectBuilder_EntityBase) objectBuilder;
    }

    public float BackkickForcePerSecond => this.m_gunBase.BackkickForcePerSecond;

    public float ShakeAmount { get; protected set; }

    public bool EnabledInWorldRules => MySession.Static.WeaponsEnabled;

    public Vector3 DirectionToTarget(Vector3D target)
    {
      MyCharacterWeaponPositionComponent positionComponent = this.Owner.Components.Get<MyCharacterWeaponPositionComponent>();
      return (Vector3) (positionComponent == null || !Sandbox.Engine.Platform.Game.IsDedicated ? Vector3D.Normalize(target - this.PositionComp.WorldMatrixRef.Translation) : Vector3D.Normalize(target - positionComponent.LogicalPositionWorld));
    }

    public bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status)
    {
      status = MyGunStatusEnum.OK;
      if (this.m_owner == null)
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.m_owner, MySafeZoneAction.Shooting))
      {
        status = MyGunStatusEnum.SafeZoneDenied;
        return false;
      }
      switch (action)
      {
        case MyShootActionEnum.PrimaryAction:
          if (!this.m_gunBase.HasAmmoMagazines)
          {
            status = MyGunStatusEnum.Failed;
            return false;
          }
          if (this.m_gunBase.ShotsInBurst > 0 && this.m_shotsFiredInBurst >= this.m_gunBase.ShotsInBurst)
          {
            status = MyGunStatusEnum.BurstLimit;
            return false;
          }
          if (this.IsReloading)
          {
            status = MyGunStatusEnum.Reloading;
            return false;
          }
          if (this.NeedsReload)
          {
            status = !this.m_gunBase.HasEnoughMagazines() ? MyGunStatusEnum.OutOfAmmo : MyGunStatusEnum.Reloading;
            return false;
          }
          if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeShoot < this.m_gunBase.ShootIntervalInMiliseconds)
          {
            status = MyGunStatusEnum.Cooldown;
            return false;
          }
          if (!MySession.Static.CreativeMode && (!(this.m_owner.CurrentWeapon is MyAutomaticRifleGun) || !this.m_gunBase.HasEnoughAmmunition()))
          {
            status = MyGunStatusEnum.OutOfAmmo;
            return false;
          }
          status = MyGunStatusEnum.OK;
          return true;
        case MyShootActionEnum.SecondaryAction:
          if (this.m_canZoom)
            return true;
          status = MyGunStatusEnum.Cooldown;
          return false;
        default:
          status = MyGunStatusEnum.Failed;
          return false;
      }
    }

    private void DebugDrawShots(Vector3 direction, int order)
    {
      Vector3D position = this.PositionComp.GetPosition();
      Vector3D vector3D = position + direction * (float) (10 + order);
      MyRenderProxy.DebugDrawLine3D(position, vector3D, Color.Green, Color.Green, false, true);
      MyRenderProxy.DebugDrawText3D(vector3D, order.ToString(), Color.Red, 1f, false, persistent: true);
    }

    public void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      switch (action)
      {
        case MyShootActionEnum.PrimaryAction:
          this.Shoot(direction, overrideWeaponPos);
          ++this.m_shotsFiredInBurst;
          this.IsShooting = true;
          if (!this.m_owner.ControllerInfo.IsLocallyControlled() || MySession.Static.IsCameraUserAnySpectator())
            break;
          MySector.MainCamera.CameraShake.AddShake(MyAutomaticRifleGun.RIFLE_MAX_SHAKE);
          MySector.MainCamera.AddFovSpring(MyAutomaticRifleGun.RIFLE_FOV_SHAKE);
          break;
        case MyShootActionEnum.SecondaryAction:
          if (MySession.Static.ControlledEntity != this.m_owner || !this.m_canZoom)
            break;
          this.m_owner.Zoom(true);
          this.m_canZoom = false;
          this.HasIronSightsActive = this.Owner.ZoomMode == MyZoomModeEnum.IronSight;
          break;
      }
    }

    public void BeginShoot(MyShootActionEnum action)
    {
    }

    public void EndShoot(MyShootActionEnum action)
    {
      switch (action)
      {
        case MyShootActionEnum.PrimaryAction:
          this.IsShooting = false;
          this.m_shotsFiredInBurst = 0;
          this.m_gunBase.StopShoot();
          break;
        case MyShootActionEnum.SecondaryAction:
          this.m_canZoom = true;
          break;
      }
      this.m_isActionDoubleClicked[action] = false;
    }

    private void Shoot(Vector3 direction, Vector3D? overrideWeaponPos)
    {
      if (!overrideWeaponPos.HasValue || this.m_gunBase.DummiesPerType(MyAmmoType.HighSpeed) > 1)
      {
        if (this.m_owner != null)
          this.m_gunBase.ShootWithOffset(this.m_owner.Physics.LinearVelocity, direction, -0.25f, (MyEntity) this.m_owner);
        else
          this.m_gunBase.ShootWithOffset(Vector3.Zero, direction, -0.25f);
      }
      else
        this.m_gunBase.Shoot(overrideWeaponPos.Value + direction * -0.25f, this.m_owner.Physics.LinearVelocity, direction, (MyEntity) this.m_owner);
      if (Sandbox.Game.Multiplayer.Sync.IsServer && MySession.Static.Settings.EnableRecoil)
      {
        this.ApplyRecoilServer();
        this.m_lastRecoil = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      }
      this.m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      this.m_isAfterReleaseFire = false;
      if (this.m_gunBase.ShootSound != null)
        this.StartLoopSound(this.m_gunBase.ShootSound);
      this.m_gunBase.ConsumeAmmo();
    }

    private void CreateSmokeEffect()
    {
      if (this.m_smokeEffect != null || MySector.MainCamera.GetDistanceFromPoint(this.PositionComp.GetPosition()) >= 150.0 || !MyParticlesManager.TryCreateParticleEffect("Smoke_Autocannon", this.PositionComp.WorldMatrixRef, out this.m_smokeEffect))
        return;
      this.m_smokeEffect.OnDelete += new Action<MyParticleEffect>(this.OnSmokeEffectDelete);
    }

    private void OnSmokeEffectDelete(MyParticleEffect _) => this.m_smokeEffect = (MyParticleEffect) null;

    public void SetMagazinePosition(MatrixD mat)
    {
      if (!this.Subparts.ContainsKey(MyAutomaticRifleGun.NAME_SUBPART_MAGAZINE))
        return;
      MyEntitySubpart subpart = this.Subparts[MyAutomaticRifleGun.NAME_SUBPART_MAGAZINE];
      subpart.PositionComp.SetWorldMatrix(mat, (object) subpart.Parent);
    }

    public void ResetMagazinePosition()
    {
      Dictionary<string, MyModelDummy> dummies = this.Render.GetModel().Dummies;
      if (!dummies.ContainsKey(MyAutomaticRifleGun.NAME_DUMMY_MAGAZINE) || !this.Subparts.ContainsKey(MyAutomaticRifleGun.NAME_SUBPART_MAGAZINE))
        return;
      MyEntitySubpart subpart = this.Subparts[MyAutomaticRifleGun.NAME_SUBPART_MAGAZINE];
      Matrix localMatrix = Matrix.Normalize(dummies[MyAutomaticRifleGun.NAME_DUMMY_MAGAZINE].Matrix);
      subpart.PositionComp.SetLocalMatrix(ref localMatrix, (object) subpart.Parent);
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.m_smokeEffect != null)
      {
        float num = 0.2f;
        this.m_smokeEffect.WorldMatrix = MatrixD.CreateTranslation(this.m_gunBase.GetMuzzleWorldPosition() + this.PositionComp.WorldMatrixRef.Forward * (double) num);
        this.m_smokeEffect.UserBirthMultiplier = 50f;
      }
      this.m_gunBase.UpdateEffects();
      if ((double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeShoot) > (double) this.m_gunBase.ReleaseTimeAfterFire && !this.m_isAfterReleaseFire)
      {
        this.StopLoopSound();
        if (this.m_smokeEffect != null)
          this.m_smokeEffect.Stop();
        this.m_isAfterReleaseFire = true;
        this.m_gunBase.RemoveOldEffects();
      }
      if (!Sandbox.Game.Multiplayer.Sync.IsDedicated)
        this.AnimateRecoil();
      this.AnimateRecoilHorizontal();
    }

    public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (status != MyGunStatusEnum.OutOfAmmo)
        return;
      this.m_gunBase.StartNoAmmoSound(this.m_soundEmitter);
    }

    public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (status == MyGunStatusEnum.Failed || status == MyGunStatusEnum.SafeZoneDenied)
        MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
      if (status == MyGunStatusEnum.OutOfAmmo)
      {
        if (this.m_outOfAmmoNotification == null)
          this.m_outOfAmmoNotification = new MyHudNotification(MyCommonTexts.OutOfAmmo, 2000, "Red");
        this.m_outOfAmmoNotification.SetTextFormatArguments((object) this.DisplayName);
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_outOfAmmoNotification);
      }
      if (status != MyGunStatusEnum.SafeZoneDenied)
        return;
      if (this.m_safezoneNotification == null)
        this.m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_ShootingDisabled, 2000, "Red");
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_safezoneNotification);
    }

    public void StartLoopSound(MySoundPair cueEnum)
    {
      this.m_gunBase.StartShootSound(this.m_soundEmitter, this.m_owner != null && this.m_owner.IsInFirstPersonView && this.m_owner == MySession.Static.LocalCharacter);
      this.UpdateSoundEmitter();
    }

    public void StopLoopSound()
    {
      if (!this.m_soundEmitter.Loop)
        return;
      this.m_soundEmitter.StopSound(false);
    }

    private void WorldPositionChanged(object source) => this.m_gunBase.WorldMatrix = this.WorldMatrix;

    protected override void Closing()
    {
      this.IsShooting = false;
      this.m_gunBase.RemoveOldEffects();
      if (this.m_smokeEffect != null)
      {
        this.m_smokeEffect.Stop();
        this.m_smokeEffect = (MyParticleEffect) null;
      }
      if (this.m_soundEmitter.Loop)
        this.m_soundEmitter.StopSound(false);
      if (this.m_reloadSoundEmitter.Loop)
        this.m_reloadSoundEmitter.StopSound(false);
      base.Closing();
    }

    public void OnControlAcquired(MyCharacter owner)
    {
      this.m_owner = owner;
      if (this.m_owner != null)
      {
        this.m_shootIgnoreEntities = new MyEntity[2]
        {
          (MyEntity) this,
          (MyEntity) this.m_owner
        };
        MyInventory inventory = MyEntityExtensions.GetInventory(this.m_owner);
        if (inventory != null)
          inventory.ContentsChanged += new Action<MyInventoryBase>(this.MyAutomaticRifleGun_ContentsChanged);
        this.Owner.RegisterRecoilDataChange(new Action<SyncBase>(this.RecoilValueChangedCallback));
        this.Owner.IsReloading.ValueChanged += new Action<SyncBase>(this.IsReloadingSynced);
      }
      this.m_gunBase.RefreshAmmunitionAmount();
      this.m_firstDraw = true;
      if (this.Owner == MySession.Static.LocalCharacter)
        MyHud.BlockInfo.AddDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
      this.m_startingVertAngle = this.Owner.HeadLocalXAngle;
      this.m_backRecoilTimer = 1f;
    }

    private void MyAutomaticRifleGun_ContentsChanged(MyInventoryBase obj) => this.m_gunBase.RefreshAmmunitionAmount();

    public void OnControlReleased()
    {
      if (this.m_owner != null)
      {
        this.Owner.IsReloading.ValueChanged -= new Action<SyncBase>(this.IsReloadingSynced);
        this.Owner.UnregisterRecoilDataChange(new Action<SyncBase>(this.RecoilValueChangedCallback));
        MyInventory inventory = MyEntityExtensions.GetInventory(this.m_owner);
        if (inventory != null)
          inventory.ContentsChanged -= new Action<MyInventoryBase>(this.MyAutomaticRifleGun_ContentsChanged);
      }
      if (this.Owner == MySession.Static.LocalCharacter)
        MyHud.BlockInfo.RemoveDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
      this.m_owner = (MyCharacter) null;
    }

    private void IsReloadingSynced(SyncBase obj)
    {
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.IsReloading = ((VRage.Sync.Sync<bool, SyncDirection.FromServer>) obj).Value;
    }

    public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate) => this.DrawHud(camera, playerId);

    public void DrawHud(IMyCameraController camera, long playerId)
    {
      if (!this.m_firstDraw)
        return;
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

    public int GetTotalAmmunitionAmount() => this.m_gunBase.GetTotalAmmunitionAmount();

    public int GetAmmunitionAmount() => this.m_gunBase.GetAmmunitionAmount();

    public int GetMagazineAmount() => this.m_gunBase.GetMagazineAmount();

    public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public MyGunBase GunBase => this.m_gunBase;

    MyEntity[] IMyGunBaseUser.IgnoreEntities => this.m_shootIgnoreEntities;

    MyEntity IMyGunBaseUser.Weapon => (MyEntity) this;

    MyEntity IMyGunBaseUser.Owner => (MyEntity) this.m_owner;

    IMyMissileGunObject IMyGunBaseUser.Launcher => (IMyMissileGunObject) this;

    MyInventory IMyGunBaseUser.AmmoInventory => this.m_owner != null ? MyEntityExtensions.GetInventory(this.m_owner) : (MyInventory) null;

    MyDefinitionId IMyGunBaseUser.PhysicalItemId => this.m_physicalItemDef.Id;

    MyInventory IMyGunBaseUser.WeaponInventory => this.m_owner != null ? MyEntityExtensions.GetInventory(this.m_owner) : (MyInventory) null;

    long IMyGunBaseUser.OwnerId => this.m_owner != null ? this.m_owner.ControllerInfo.ControllingIdentityId : 0L;

    string IMyGunBaseUser.ConstraintDisplayName => (string) null;

    public MyPhysicalItemDefinition PhysicalItemDefinition => this.m_physicalItemDef;

    public int CurrentAmmunition
    {
      set => this.m_gunBase.CurrentAmmo = value;
      get => this.m_gunBase.GetTotalAmmunitionAmount();
    }

    public int CurrentMagazineAmmunition
    {
      set => this.m_gunBase.CurrentAmmo = value;
      get => this.m_gunBase.CurrentAmmo;
    }

    public int CurrentMagazineAmount
    {
      set => this.m_gunBase.CurrentMagazines = value;
      get => this.m_gunBase.CurrentMagazines;
    }

    public bool HasIronSightsActive
    {
      get => this.m_gunBase.HasIronSightsActive;
      set => this.m_gunBase.HasIronSightsActive = value;
    }

    public bool Reloadable => true;

    public bool IsRecoiling => this.m_isRecoiling;

    public bool IsReloading
    {
      get => this.m_isReloading;
      set
      {
        if (this.m_isReloading == value)
          return;
        this.m_isReloading = value;
        if (!Sandbox.Game.Multiplayer.Sync.IsServer)
          return;
        this.Owner.IsReloading.Value = value;
      }
    }

    public bool NeedsReload => !this.IsReloading && this.CurrentMagazineAmmunition <= 0;

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation10();
      this.UpdateSoundEmitter();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.IsReloading || !(this.m_reloadEndTime < MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds)))
        return;
      if (this.m_gunBase.HasAmmoMagazines)
        this.m_gunBase.ConsumeMagazine();
      this.m_gunBase.RefreshAmmunitionAmount();
      this.IsReloading = false;
    }

    public void UpdateSoundEmitter()
    {
      this.UpdateSingleEmitter(this.m_soundEmitter);
      this.UpdateSingleEmitter(this.m_reloadSoundEmitter);
    }

    private void UpdateSingleEmitter(MyEntity3DSoundEmitter soundEmitter)
    {
      if (soundEmitter == null)
        return;
      if (this.m_owner != null)
      {
        Vector3 zero = Vector3.Zero;
        this.m_owner.GetLinearVelocity(ref zero);
        soundEmitter.SetVelocity(new Vector3?(zero));
      }
      soundEmitter.Update();
    }

    public bool SupressShootAnimation() => false;

    public bool ShouldEndShootOnPause(MyShootActionEnum action) => true;

    public bool CanDoubleClickToStick(MyShootActionEnum action) => false;

    public void DoubleClicked(MyShootActionEnum action) => this.m_isActionDoubleClicked[action] = true;

    private void AnimateRecoilHorizontal()
    {
      if ((double) this.m_horizontalAngle == 0.0 || (double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastRecoil) < (double) this.m_gunBase.WeaponProperties.RecoilResetTimeMilliseconds)
        return;
      if ((double) this.m_horizontalAngleOriginal == 0.0)
        this.m_horizontalAngleOriginal = this.m_horizontalAngle;
      if ((double) this.m_backRecoilTimer >= 1.0)
      {
        this.Owner.SetHeadLocalYAngle(this.Owner.HeadLocalYAngle - this.m_horizontalAngle);
        this.m_horizontalAngle = 0.0f;
        this.m_horizontalAngleOriginal = 0.0f;
      }
      else
      {
        float num = this.m_horizontalAngle - MathHelper.Lerp(this.m_horizontalAngleOriginal, 0.0f, this.m_backRecoilTimer);
        this.Owner.SetHeadLocalYAngle(this.Owner.HeadLocalYAngle - num);
        this.m_horizontalAngle -= num;
      }
    }

    private void HorizontalRotation(float angle)
    {
      MatrixD worldMatrix = this.Owner.PositionComp.WorldMatrix;
      this.Owner.PositionComp.WorldMatrix = MatrixD.CreateRotationY((double) angle) * worldMatrix;
    }

    private void AnimateRecoil()
    {
      if ((double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastRecoil) > (double) this.m_gunBase.WeaponProperties.RecoilResetTimeMilliseconds)
      {
        float headLocalXangle = this.Owner.HeadLocalXAngle;
        if ((double) this.m_backRecoilTimer >= 1.0 | (double) Math.Abs(headLocalXangle - this.m_startingVertAngle) < 1.0 / 1000.0)
        {
          if (!this.m_isRecoiling)
            return;
          this.m_backRecoilTimer = 1f;
          this.m_currentVertAngle = this.m_startingVertAngle;
          this.Owner.SetHeadLocalXAngle(this.m_currentVertAngle);
          this.m_isRecoiling = false;
          return;
        }
        this.m_currentVertAngle = MathHelper.Lerp(headLocalXangle, this.m_startingVertAngle, this.m_backRecoilTimer);
        this.m_backRecoilTimer += MyAutomaticRifleGun.RECOIL_RETURN_SPEED;
      }
      else
      {
        if ((double) this.m_recoilTimer >= 1.0)
        {
          this.m_lastVertRecoilDiff = float.MaxValue;
          return;
        }
        float headLocalXangle = this.Owner.HeadLocalXAngle;
        this.m_recoilTimer += MyAutomaticRifleGun.RECOIL_SPEED / ((float) this.m_gunBase.WeaponProperties.CurrentWeaponRateOfFire / 60f);
        float num = MathHelper.Lerp(headLocalXangle, this.m_nextVertAngle, this.m_recoilTimer);
        this.m_lastVertRecoilDiff = num - headLocalXangle;
        this.m_currentVertAngle = num;
      }
      this.Owner.SetHeadLocalXAngle(this.m_currentVertAngle);
    }

    private void ApplyRecoilServer() => this.Owner.SetRecoilData(1f, this.random.NextFloat(-1f, 1f));

    private void RecoilValueChangedCallback(SyncBase obj)
    {
      if (this.Owner == null)
        return;
      float verticalValue;
      float horizontalValue;
      this.Owner.GetRecoilData(out verticalValue, out horizontalValue);
      if (this.Owner.ShouldRecoilRotate && this.Owner.JetpackRunning)
      {
        this.m_startingVertAngle = this.m_currentVertAngle = this.m_nextVertAngle = this.Owner.HeadLocalXAngle;
        Vector3 right = (Vector3) this.Owner.PositionComp.WorldMatrix.Right;
        Vector3 up = (Vector3) this.Owner.PositionComp.WorldMatrix.Up;
        Vector3 angularVelocity = this.Owner.Physics.AngularVelocity;
        float num1 = Vector3.Dot(angularVelocity, right);
        double num2 = (double) Vector3.Dot(angularVelocity, up);
        float num3 = (double) num1 >= (double) verticalValue * (double) this.m_definition.RecoilJetpackVertical * (double) MyAutomaticRifleGun.TO_RAD ? num1 : verticalValue * this.m_definition.RecoilJetpackVertical * MyAutomaticRifleGun.TO_RAD;
        float num4 = (float) num2;
        float num5 = horizontalValue * (1f - MyAutomaticRifleGun.HORIZONTAL_RECOIL_OFFSET);
        float num6 = (double) num5 >= 0.0 ? num5 + MyAutomaticRifleGun.HORIZONTAL_RECOIL_OFFSET : num5 - MyAutomaticRifleGun.HORIZONTAL_RECOIL_OFFSET;
        float num7 = num4 + this.m_definition.RecoilJetpackHorizontal * num6 * MyAutomaticRifleGun.TO_RAD;
        this.Owner.Physics.AngularVelocity = right * num3 + up * num7;
        this.m_isRecoiling = true;
        this.m_backRecoilTimer = 0.0f;
        this.m_startingVertAngle = this.Owner.HeadLocalXAngle;
        this.m_nextVertAngle = this.m_startingVertAngle;
      }
      else
      {
        double recoilGroundVertical = (double) this.m_definition.RecoilGroundVertical;
        float groundHorizontal = this.m_definition.RecoilGroundHorizontal;
        float recoilVertical;
        float recoilHorizontal;
        this.GetRecoilMultipliers(this.Owner, out recoilVertical, out recoilHorizontal);
        if ((double) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastRecoil) > (double) this.m_gunBase.WeaponProperties.RecoilResetTimeMilliseconds)
        {
          if ((double) this.m_backRecoilTimer >= 1.0)
          {
            this.m_startingVertAngle = this.Owner.HeadLocalXAngle;
            this.m_nextVertAngle = this.m_startingVertAngle;
          }
          else
            this.m_nextVertAngle = this.Owner.HeadLocalXAngle;
          this.m_backRecoilTimer = 0.0f;
        }
        this.m_recoilTimer = 0.0f;
        double num = (double) verticalValue;
        this.m_nextVertAngle += (float) (recoilGroundVertical * num) * recoilVertical;
        this.m_isRecoiling = true;
        this.AddHorizontalRecoil(groundHorizontal * horizontalValue * recoilHorizontal);
      }
      this.m_lastRecoil = MySandboxGame.TotalGamePlayTimeInMilliseconds;
    }

    private void GetRecoilMultipliers(
      MyCharacter owner,
      out float recoilVertical,
      out float recoilHorizontal)
    {
      recoilVertical = 1f;
      recoilHorizontal = 1f;
      if (this.m_definition == null)
        return;
      string key = "";
      if (owner.IsIronSighted)
      {
        key = MyAutomaticRifleGun.STATE_KEY_AIMING;
      }
      else
      {
        MyCharacterMovementEnum currentMovementState = owner.GetCurrentMovementState();
        if ((uint) currentMovementState <= 128U)
        {
          if ((uint) currentMovementState <= 34U)
          {
            switch (currentMovementState)
            {
              case MyCharacterMovementEnum.Standing:
                key = MyAutomaticRifleGun.STATE_KEY_STANDING;
                goto label_35;
              case MyCharacterMovementEnum.Sitting:
              case MyCharacterMovementEnum.Flying:
              case MyCharacterMovementEnum.Falling:
                goto label_35;
              case MyCharacterMovementEnum.Crouching:
                key = MyAutomaticRifleGun.STATE_KEY_CROUCHING;
                goto label_35;
              case MyCharacterMovementEnum.Jump:
                key = MyAutomaticRifleGun.STATE_KEY_WALKING;
                goto label_35;
              default:
                if ((uint) currentMovementState <= 18U)
                {
                  switch (currentMovementState)
                  {
                    case MyCharacterMovementEnum.Walking:
                      break;
                    case MyCharacterMovementEnum.CrouchWalking:
                      goto label_34;
                    default:
                      goto label_35;
                  }
                }
                else
                {
                  switch (currentMovementState)
                  {
                    case MyCharacterMovementEnum.BackWalking:
                      break;
                    case MyCharacterMovementEnum.CrouchBackWalking:
                      goto label_34;
                    default:
                      goto label_35;
                  }
                }
                break;
            }
          }
          else if ((uint) currentMovementState <= 80U)
          {
            switch (currentMovementState)
            {
              case MyCharacterMovementEnum.WalkStrafingLeft:
              case MyCharacterMovementEnum.WalkingLeftFront:
                break;
              case MyCharacterMovementEnum.CrouchStrafingLeft:
                goto label_34;
              default:
                goto label_35;
            }
          }
          else if ((uint) currentMovementState <= 96U)
          {
            switch (currentMovementState)
            {
              case MyCharacterMovementEnum.CrouchWalkingLeftFront:
                goto label_34;
              case MyCharacterMovementEnum.WalkingLeftBack:
                break;
              default:
                goto label_35;
            }
          }
          else
          {
            switch (currentMovementState)
            {
              case MyCharacterMovementEnum.CrouchWalkingLeftBack:
                goto label_34;
              case MyCharacterMovementEnum.WalkStrafingRight:
                break;
              default:
                goto label_35;
            }
          }
        }
        else
        {
          if ((uint) currentMovementState <= 1056U)
          {
            if ((uint) currentMovementState <= 146U)
            {
              switch (currentMovementState)
              {
                case MyCharacterMovementEnum.CrouchStrafingRight:
                case MyCharacterMovementEnum.CrouchWalkingRightFront:
                  goto label_34;
                case MyCharacterMovementEnum.WalkingRightFront:
                  goto label_32;
                default:
                  goto label_35;
              }
            }
            else if ((uint) currentMovementState <= 162U)
            {
              switch (currentMovementState)
              {
                case MyCharacterMovementEnum.WalkingRightBack:
                  goto label_32;
                case MyCharacterMovementEnum.CrouchWalkingRightBack:
                  goto label_34;
                default:
                  goto label_35;
              }
            }
            else if (currentMovementState != MyCharacterMovementEnum.Running && currentMovementState != MyCharacterMovementEnum.Backrunning)
              goto label_35;
          }
          else if ((uint) currentMovementState <= 1152U)
          {
            if ((uint) currentMovementState <= 1104U)
            {
              if (currentMovementState != MyCharacterMovementEnum.RunStrafingLeft && currentMovementState != MyCharacterMovementEnum.RunningLeftFront)
                goto label_35;
            }
            else if (currentMovementState != MyCharacterMovementEnum.RunningLeftBack && currentMovementState != MyCharacterMovementEnum.RunStrafingRight)
              goto label_35;
          }
          else if ((uint) currentMovementState <= 1184U)
          {
            if (currentMovementState != MyCharacterMovementEnum.RunningRightFront && currentMovementState != MyCharacterMovementEnum.RunningRightBack)
              goto label_35;
          }
          else if (currentMovementState == MyCharacterMovementEnum.CrouchRotatingLeft || currentMovementState == MyCharacterMovementEnum.CrouchRotatingRight)
            goto label_34;
          else
            goto label_35;
          key = MyAutomaticRifleGun.STATE_KEY_RUNNING;
          goto label_35;
        }
label_32:
        key = MyAutomaticRifleGun.STATE_KEY_WALKING;
        goto label_35;
label_34:
        key = MyAutomaticRifleGun.STATE_KEY_CROUCHING;
      }
label_35:
      if (string.IsNullOrEmpty(key) || !this.m_definition.RecoilMultiplierData.ContainsKey(key))
        return;
      Tuple<float, float> tuple = this.m_definition.RecoilMultiplierData[key];
      recoilVertical = tuple.Item1;
      recoilHorizontal = tuple.Item2;
    }

    private void AddHorizontalRecoil(float angleAddition)
    {
      this.m_horizontalAngleOriginal = 0.0f;
      float num1 = this.m_horizontalAngle + angleAddition;
      if ((double) Math.Abs(num1) < (double) MyAutomaticRifleGun.MAX_HORIZONTAL_RECOIL_DEVIATION)
      {
        this.Owner.SetHeadLocalYAngle(this.Owner.HeadLocalYAngle + angleAddition);
        this.m_horizontalAngle += angleAddition;
      }
      else
      {
        float num2 = MyAutomaticRifleGun.MAX_HORIZONTAL_RECOIL_DEVIATION - Math.Abs(this.m_horizontalAngle);
        if ((double) num1 < 0.0)
          num2 *= -1f;
        this.Owner.SetHeadLocalYAngle(this.Owner.HeadLocalYAngle + num2);
        this.m_horizontalAngle += num2;
      }
    }

    public void ChangeRecoilVertAngles(float diffAngle)
    {
      double recoilGroundVertical = (double) this.m_definition.RecoilGroundVertical;
      this.m_nextVertAngle += diffAngle;
      this.m_startingVertAngle = this.m_nextVertAngle;
    }

    public void MissileShootEffect() => this.m_gunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);

    public void ShootMissile(MyObjectBuilder_Missile builder) => MyMultiplayer.RaiseStaticEvent<MyObjectBuilder_Missile>((Func<IMyEventOwner, Action<MyObjectBuilder_Missile>>) (x => new Action<MyObjectBuilder_Missile>(MyAutomaticRifleGun.OnShootMissile)), builder);

    [Event(null, 1197)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void OnShootMissile(MyObjectBuilder_Missile builder) => MyMissiles.Add(builder);

    public void RemoveMissile(long entityId) => MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (x => new Action<long>(MyAutomaticRifleGun.OnRemoveMissile)), entityId);

    [Event(null, 1210)]
    [Reliable]
    [Broadcast]
    private static void OnRemoveMissile(long entityId) => MyMissiles.Remove(entityId);

    public bool CanReload()
    {
      bool flag = !this.IsReloading && !this.m_gunBase.IsAmmoFull() && (this.m_gunBase.HasAmmoMagazines && this.m_gunBase.HasEnoughMagazines()) && MySandboxGame.TotalGamePlayTimeInMilliseconds > this.m_weaponEquipDelay;
      long? entityId1 = this.Owner?.EntityId;
      long? entityId2 = MySession.Static?.LocalCharacter?.EntityId;
      if (entityId1.GetValueOrDefault() == entityId2.GetValueOrDefault() & entityId1.HasValue == entityId2.HasValue)
        flag = flag && MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay && !MyScreenManager.IsAnyScreenOpening();
      return flag;
    }

    public bool Reload()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || this.IsReloading || (this.m_gunBase.IsAmmoFull() || !this.m_gunBase.HasAmmoMagazines))
        return false;
      if (this.Owner.IsIronSighted)
        this.Owner.EnableIronsight(false, true, true, forceChange: true);
      this.m_reloadEndTime = MyTimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds + (double) this.m_definition.ReloadTime);
      this.CurrentAmmunition = 0;
      this.m_gunBase.EmptyMagazine();
      this.IsReloading = true;
      return true;
    }

    public float GetReloadDuration() => (float) this.m_definition.ReloadTime * (1f / 1000f);

    public Vector3D GetMuzzlePosition() => this.m_gunBase.GetMuzzleWorldPosition();

    void IMyHandheldGunObject<MyGunBase>.PlayReloadSound() => this.m_gunBase.StartReloadSound(this.m_reloadSoundEmitter);

    public bool GetShakeOnAction(MyShootActionEnum action)
    {
      bool flag;
      return !this.m_definition.ShakeOnAction.TryGetValue(action, out flag) || flag;
    }

    protected sealed class OnShootMissile\u003C\u003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<IMyEventOwner, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyObjectBuilder_Missile builder,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAutomaticRifleGun.OnShootMissile(builder);
      }
    }

    protected sealed class OnRemoveMissile\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
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
        MyAutomaticRifleGun.OnRemoveMissile(entityId);
      }
    }

    private class Sandbox_Game_Weapons_MyAutomaticRifleGun\u003C\u003EActor : IActivator, IActivator<MyAutomaticRifleGun>
    {
      object IActivator.CreateInstance() => (object) new MyAutomaticRifleGun();

      MyAutomaticRifleGun IActivator<MyAutomaticRifleGun>.CreateInstance() => new MyAutomaticRifleGun();
    }
  }
}
