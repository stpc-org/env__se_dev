// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MySmallGatlingGun
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
  [MyCubeBlockType(typeof (MyObjectBuilder_SmallGatlingGun))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMySmallGatlingGun), typeof (Sandbox.ModAPI.Ingame.IMySmallGatlingGun)})]
  public class MySmallGatlingGun : MyUserControllableGun, IMyGunObject<MyGunBase>, IMyInventoryOwner, IMyConveyorEndpointBlock, IMyGunBaseUser, Sandbox.ModAPI.IMySmallGatlingGun, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMySmallGatlingGun, IMyMissileGunObject
  {
    public const int SMOKE_OVERTIME_LENGTH = 120;
    private const string BAREL_SUBPART_NAME = "Barrel";
    private int m_lastTimeShoot;
    private float m_rotationTimeout;
    private bool m_cannonMotorEndPlayed;
    private ShootStateEnum currentState;
    private int m_shootOvertime;
    private int m_smokeOvertime;
    private readonly VRage.Sync.Sync<int, SyncDirection.FromServer> m_lateStartRandom;
    private VRage.Sync.Sync<int, SyncDirection.FromServer> m_cachedAmmunitionAmount;
    private int m_currentLateStart;
    private float m_muzzleFlashLength;
    private float m_muzzleFlashRadius;
    private int m_smokesToGenerate;
    private MyEntity3DSoundEmitter m_soundEmitterRotor;
    private MyEntity m_barrel;
    private MyParticleEffect m_smokeEffect;
    private MyParticleEffect m_flashEffect;
    private MyGunBase m_gunBase;
    private Vector3D m_targetLocal = (Vector3D) Vector3.Zero;
    private List<HkHitInfo> m_hits = new List<HkHitInfo>();
    private MyHudNotification m_safezoneNotification;
    private MyMultilineConveyorEndpoint m_conveyorEndpoint;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;
    private MyEntity[] m_shootIgnoreEntities;
    private float m_inventoryFillFactor = 0.5f;

    public int LastTimeShoot => this.m_lastTimeShoot;

    public int LateStartRandom => this.m_lateStartRandom.Value;

    public float MuzzleFlashLength => this.m_muzzleFlashLength;

    public float MuzzleFlashRadius => this.m_muzzleFlashRadius;

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_conveyorEndpoint;

    public bool IsSkinnable => false;

    public void InitializeConveyorEndpoint()
    {
      this.m_conveyorEndpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_conveyorEndpoint));
    }

    public override bool IsStationary() => true;

    public override Vector3D GetWeaponMuzzleWorldPosition()
    {
      if (this.m_gunBase == null)
        return base.GetWeaponMuzzleWorldPosition();
      this.UpdateMuzzlePosition();
      return this.m_gunBase.GetMuzzleWorldPosition();
    }

    public MySmallGatlingGun()
    {
      this.m_shootIgnoreEntities = new MyEntity[1]
      {
        (MyEntity) this
      };
      this.CreateTerminalControls();
      this.m_lastTimeShoot = -60000;
      this.m_smokesToGenerate = 0;
      this.m_cannonMotorEndPlayed = true;
      this.m_rotationTimeout = 2000f + MyUtils.GetRandomFloat(-500f, 500f);
      this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this, true);
      this.m_gunBase = new MyGunBase();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_gunBase.OnAmmoAmountChanged += new Action(this.OnAmmoAmountChangedOnServer);
      else
        this.m_cachedAmmunitionAmount.ValueChanged += new Action<SyncBase>(this.OnAmmoAmountChangedFromServer);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentSmallGatlingGun(this));
      this.SyncType.Append((object) this.m_gunBase);
    }

    private void OnAmmoAmountChangedFromServer(SyncBase obj) => this.GunBase.CurrentAmmo = this.m_cachedAmmunitionAmount.Value;

    private void OnAmmoAmountChangedOnServer()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_cachedAmmunitionAmount.Value = this.GunBase.CurrentAmmo;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MySmallGatlingGun>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MySmallGatlingGun> onOff = new MyTerminalControlOnOffSwitch<MySmallGatlingGun>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem);
      onOff.Getter = (MyTerminalValueControl<MySmallGatlingGun, bool>.GetterDelegate) (x => x.UseConveyorSystem);
      onOff.Setter = (MyTerminalValueControl<MySmallGatlingGun, bool>.SetterDelegate) ((x, v) => x.UseConveyorSystem = v);
      onOff.EnableToggleAction<MySmallGatlingGun>();
      MyTerminalControlFactory.AddControl<MySmallGatlingGun>((MyTerminalControl<MySmallGatlingGun>) onOff);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_SmallGatlingGun builderCubeBlock = (MyObjectBuilder_SmallGatlingGun) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.GunBase = this.m_gunBase.GetObjectBuilder();
      builderCubeBlock.UseConveyorSystem = (bool) this.m_useConveyorSystem;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyObjectBuilder_SmallGatlingGun builderSmallGatlingGun = objectBuilder as MyObjectBuilder_SmallGatlingGun;
      MyWeaponBlockDefinition blockDefinition = this.BlockDefinition as MyWeaponBlockDefinition;
      if (MyFakes.ENABLE_INVENTORY_FIX)
        this.FixSingleInventory();
      this.m_soundEmitterRotor = new MyEntity3DSoundEmitter((MyEntity) this);
      if (blockDefinition != null)
        this.m_inventoryFillFactor = blockDefinition.InventoryFillFactorMin;
      if (MyEntityExtensions.GetInventory(this) == null)
      {
        MyInventory myInventory = blockDefinition == null ? new MyInventory(0.064f, new Vector3(0.4f, 0.4f, 0.4f), MyInventoryFlags.CanReceive) : new MyInventory(blockDefinition.InventoryMaxVolume, new Vector3(0.4f, 0.4f, 0.4f), MyInventoryFlags.CanReceive);
        this.Components.Add<MyInventoryBase>((MyInventoryBase) myInventory);
        myInventory.Init(builderSmallGatlingGun.Inventory);
      }
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(blockDefinition.ResourceSinkGroup, 0.0002f, (Func<float>) (() => this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      resourceSinkComponent.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink = resourceSinkComponent;
      this.m_gunBase.Init(builderSmallGatlingGun.GunBase, this.BlockDefinition, (IMyGunBaseUser) this);
      base.Init(objectBuilder, cubeGrid);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_lateStartRandom.Value = MyUtils.GetRandomInt(0, 30);
      this.ResourceSink.Update();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawPowerReciever(this.ResourceSink, (VRage.ModAPI.IMyEntity) this));
      this.m_useConveyorSystem.SetLocalValue(builderSmallGatlingGun.UseConveyorSystem);
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MySmallGatlingGun_IsWorkingChanged);
      this.NeedsWorldMatrix = false;
    }

    protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
    {
      base.OnInventoryComponentAdded(inventory);
      if (MyEntityExtensions.GetInventory(this) == null)
        return;
      MyEntityExtensions.GetInventory(this).ContentsChanged += new Action<MyInventoryBase>(this.AmmoInventory_ContentsChanged);
    }

    protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
    {
      base.OnInventoryComponentRemoved(inventory);
      if (!(inventory is MyInventory myInventory))
        return;
      myInventory.ContentsChanged -= new Action<MyInventoryBase>(this.AmmoInventory_ContentsChanged);
    }

    private void Receiver_IsPoweredChanged() => this.UpdateIsWorking();

    private void AmmoInventory_ContentsChanged(MyInventoryBase obj) => this.m_gunBase.RefreshAmmunitionAmount(true);

    protected override void Closing()
    {
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(true);
      if (this.m_soundEmitterRotor != null)
        this.m_soundEmitterRotor.StopSound(true);
      if (this.m_smokeEffect != null)
      {
        this.m_smokeEffect.Stop(false);
        this.m_smokeEffect = (MyParticleEffect) null;
      }
      if (this.m_flashEffect != null)
      {
        this.m_flashEffect.Stop();
        this.m_flashEffect = (MyParticleEffect) null;
      }
      base.Closing();
    }

    public override void OnRemovedByCubeBuilder()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this));
      base.OnRemovedByCubeBuilder();
    }

    public override void OnDestroy()
    {
      this.ReleaseInventory(MyEntityExtensions.GetInventory(this), true);
      base.OnDestroy();
    }

    public override void InitComponents()
    {
      this.Render = (MyRenderComponentBase) new MyRenderComponentCubeBlockWithParentedSubpart();
      base.InitComponents();
    }

    protected override MyEntitySubpart InstantiateSubpart(
      MyModelDummy subpartDummy,
      ref MyEntitySubpart.Data data)
    {
      MyEntitySubpart myEntitySubpart = base.InstantiateSubpart(subpartDummy, ref data);
      myEntitySubpart.Render = (MyRenderComponentBase) new MyParentedSubpartRenderComponent();
      return myEntitySubpart;
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      MyEntitySubpart myEntitySubpart;
      if (!this.Subparts.TryGetValue("Barrel", out myEntitySubpart))
        return;
      this.m_barrel = (MyEntity) myEntitySubpart;
    }

    private void MySmallGatlingGun_IsWorkingChanged(MyCubeBlock obj)
    {
      if (this.IsWorking)
      {
        if (this.currentState != ShootStateEnum.Continuous)
          return;
        this.StartLoopSound();
      }
      else
        this.StopLoopSound();
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.PositionComp == null)
        return;
      bool flag1 = this.m_flashEffect == null;
      bool flag2 = (uint) this.currentState > 0U;
      if (!Sandbox.Game.Multiplayer.Sync.IsDedicated && flag1 == flag2)
      {
        if (flag1)
        {
          MyParticlesManager.TryCreateParticleEffect("Muzzle_Flash_Large", this.PositionComp.WorldMatrixRef, out this.m_flashEffect);
          if (this.currentState == ShootStateEnum.Once)
          {
            this.m_smokesToGenerate = 10;
            this.m_shootOvertime = 5;
          }
        }
        else if (this.m_shootOvertime <= 0)
        {
          if (this.m_flashEffect != null)
          {
            this.m_flashEffect.Stop();
            this.m_flashEffect = (MyParticleEffect) null;
          }
        }
        else
          --this.m_shootOvertime;
      }
      if (this.currentState == ShootStateEnum.Once)
        this.currentState = ShootStateEnum.Off;
      if (!Sandbox.Game.Multiplayer.Sync.IsDedicated)
      {
        float radians = (float) ((double) MathHelper.SmoothStep(0.0f, 1f, 1f - MathHelper.Clamp((float) (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeShoot) / this.m_rotationTimeout, 0.0f, 1f)) * 12.5663709640503 * 0.0166666675359011);
        if ((double) radians != 0.0 && this.m_barrel != null && this.m_barrel.PositionComp != null)
        {
          Matrix localMatrix = Matrix.CreateRotationZ(radians) * this.m_barrel.PositionComp.LocalMatrixRef;
          Matrix renderLocal = localMatrix * this.PositionComp.LocalMatrixRef;
          this.m_barrel.PositionComp.SetLocalMatrix(ref localMatrix, (object) null, true, ref renderLocal);
        }
        if ((double) radians == 0.0 && !this.HasDamageEffect && (this.m_smokeOvertime <= 0 && this.currentState == ShootStateEnum.Off))
          this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
        else
          this.UpdateMuzzlePosition();
        this.SmokesToGenerateDecrease();
        if (this.m_smokesToGenerate > 0)
        {
          this.m_smokeOvertime = 120;
          if (MySector.MainCamera.GetDistanceFromPoint(this.PositionComp.GetPosition()) < 150.0)
          {
            if (this.m_smokeEffect == null)
              MyParticlesManager.TryCreateParticleEffect("Smoke_Autocannon", this.PositionComp.WorldMatrixRef, out this.m_smokeEffect);
            else if (this.m_smokeEffect.IsEmittingStopped)
            {
              this.m_smokeEffect.Play();
              this.m_smokeEffect.WorldMatrix = this.PositionComp.WorldMatrixRef;
            }
          }
        }
        else
        {
          --this.m_smokeOvertime;
          if (this.m_smokeEffect != null && !this.m_smokeEffect.IsEmittingStopped)
            this.m_smokeEffect.StopEmitting();
          if (this.m_flashEffect != null)
          {
            this.m_flashEffect.Stop();
            this.m_flashEffect = (MyParticleEffect) null;
          }
        }
        if (this.m_smokeEffect != null)
        {
          this.m_smokeEffect.WorldMatrix = MatrixD.CreateTranslation(this.GetWeaponMuzzleWorldPosition());
          this.m_smokeEffect.UserBirthMultiplier = (float) (this.m_smokesToGenerate / 10 * 10);
        }
        if (this.m_flashEffect == null)
          return;
        MatrixD matrixD = this.PositionComp.WorldMatrixRef;
        matrixD.Translation = this.GetWeaponMuzzleWorldPosition();
        this.m_flashEffect.WorldMatrix = matrixD;
      }
      else
      {
        if (this.currentState != ShootStateEnum.Off || (bool) this.m_isShooting || (bool) this.m_forceShoot)
          return;
        this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
      }
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (MySession.Static.SurvivalMode && Sandbox.Game.Multiplayer.Sync.IsServer && (this.IsWorking && (bool) this.m_useConveyorSystem))
      {
        MyInventory inventory = MyEntityExtensions.GetInventory(this);
        if ((double) inventory.VolumeFillFactor < (double) this.m_inventoryFillFactor)
        {
          MyAmmoMagazineDefinition magazineDefinition = this.m_gunBase.CurrentAmmoMagazineDefinition;
          if (magazineDefinition != null)
          {
            MyFixedPoint myFixedPoint = MyFixedPoint.Floor((inventory.MaxVolume - inventory.CurrentVolume) * (1f / magazineDefinition.Volume));
            if (myFixedPoint == (MyFixedPoint) 0)
              return;
            this.CubeGrid.GridSystems.ConveyorSystem.PullItem(this.m_gunBase.CurrentAmmoMagazineId, new MyFixedPoint?(myFixedPoint), (IMyConveyorEndpointBlock) this, inventory, false, false);
          }
        }
      }
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || this.m_gunBase.CurrentAmmo != 0)
        return;
      this.m_gunBase.ConsumeMagazine();
    }

    private void ClampSmokesToGenerate() => this.m_smokesToGenerate = MyUtils.GetClampInt(this.m_smokesToGenerate, 0, 50);

    private void SmokesToGenerateIncrease()
    {
      this.m_smokesToGenerate += 19;
      this.ClampSmokesToGenerate();
    }

    private void SmokesToGenerateDecrease()
    {
      --this.m_smokesToGenerate;
      this.ClampSmokesToGenerate();
    }

    public float BackkickForcePerSecond => this.m_gunBase.BackkickForcePerSecond;

    public float ShakeAmount { get; protected set; }

    public float ProjectileCountMultiplier => 0.0f;

    public bool EnabledInWorldRules => MySession.Static.WeaponsEnabled;

    public MyDefinitionId DefinitionId => this.BlockDefinition.Id;

    public Vector3 DirectionToTarget(Vector3D target) => (Vector3) this.PositionComp.WorldMatrixRef.Forward;

    public override bool CanShoot(
      MyShootActionEnum action,
      long shooter,
      out MyGunStatusEnum status)
    {
      status = MyGunStatusEnum.OK;
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.CubeGrid, MySafeZoneAction.Shooting))
      {
        status = MyGunStatusEnum.SafeZoneDenied;
        return false;
      }
      if (action != MyShootActionEnum.PrimaryAction)
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (this.Parent == null || this.Parent.Physics == null)
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (!this.m_gunBase.HasAmmoMagazines)
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeShoot < this.m_gunBase.ShootIntervalInMiliseconds)
      {
        status = MyGunStatusEnum.Cooldown;
        return false;
      }
      if (!this.HasPlayerAccess(shooter))
      {
        status = MyGunStatusEnum.AccessDenied;
        return false;
      }
      if (!this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
      {
        status = MyGunStatusEnum.OutOfPower;
        return false;
      }
      if (!this.IsFunctional)
      {
        status = MyGunStatusEnum.NotFunctional;
        return false;
      }
      if (!this.Enabled)
      {
        status = MyGunStatusEnum.Disabled;
        return false;
      }
      if (!MySession.Static.CreativeMode && !this.m_gunBase.HasEnoughAmmunition())
      {
        status = MyGunStatusEnum.OutOfAmmo;
        return false;
      }
      if (this.m_barrel != null)
        return true;
      status = MyGunStatusEnum.Failed;
      return false;
    }

    public void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      if (this.Parent.Physics == null)
        return;
      if (this.m_shootingBegun && (int) this.m_lateStartRandom > this.m_currentLateStart && this.currentState == ShootStateEnum.Continuous)
      {
        ++this.m_currentLateStart;
      }
      else
      {
        if (this.currentState == ShootStateEnum.Off)
        {
          this.currentState = ShootStateEnum.Once;
          this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
        }
        this.m_muzzleFlashLength = MyUtils.GetRandomFloat(3f, 4f) * this.CubeGrid.GridSize;
        this.m_muzzleFlashRadius = MyUtils.GetRandomFloat(0.9f, 1.5f) * this.CubeGrid.GridSize;
        this.Render.NeedsDrawFromParent = true;
        this.SmokesToGenerateIncrease();
        this.PlayShotSound();
        this.UpdateMuzzlePosition();
        this.m_gunBase.Shoot(this.Parent.Physics.LinearVelocity);
        this.m_gunBase.ConsumeAmmo();
        if ((double) this.BackkickForcePerSecond > 0.0 && !this.CubeGrid.Physics.IsStatic)
          this.CubeGrid.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, new Vector3?(-direction * this.BackkickForcePerSecond), new Vector3D?(this.PositionComp.GetPosition()), new Vector3?(), new float?(), true, false);
        this.m_cannonMotorEndPlayed = false;
        this.m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
      }
    }

    public override void BeginShoot(MyShootActionEnum action)
    {
      this.currentState = ShootStateEnum.Continuous;
      base.BeginShoot(action);
      this.StartLoopSound();
    }

    public override void EndShoot(MyShootActionEnum action)
    {
      this.currentState = ShootStateEnum.Off;
      base.EndShoot(action);
      this.m_currentLateStart = 0;
      this.StopLoopSound();
      if (this.m_flashEffect == null)
        return;
      this.m_flashEffect.Stop();
      this.m_flashEffect = (MyParticleEffect) null;
    }

    public bool IsShooting => MySandboxGame.TotalGamePlayTimeInMilliseconds - this.m_lastTimeShoot < this.m_gunBase.ShootIntervalInMiliseconds * 2;

    public int ShootDirectionUpdateTime => 0;

    public bool NeedsShootDirectionWhileAiming => false;

    public float MaximumShotLength => 0.0f;

    public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (status == MyGunStatusEnum.OutOfAmmo && !MySession.Static.CreativeMode && MyEntityExtensions.GetInventory(this).GetItemAmount(this.m_gunBase.CurrentAmmoMagazineId, MyItemFlags.None, false) < (MyFixedPoint) 1)
        this.StartNoAmmoSound();
      if (status != MyGunStatusEnum.SafeZoneDenied)
        return;
      MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
    }

    public void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (status != MyGunStatusEnum.SafeZoneDenied)
        return;
      if (this.m_safezoneNotification == null)
        this.m_safezoneNotification = new MyHudNotification(MyCommonTexts.SafeZone_ShootingDisabled, 2000, "Red");
      MyHud.Notifications.Add((MyHudNotificationBase) this.m_safezoneNotification);
    }

    public void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status)
    {
    }

    public override void UpdateBeforeSimulation() => base.UpdateBeforeSimulation();

    public void OnControlAcquired(MyCharacter owner)
    {
      if (owner == null)
        return;
      MyHud.BlockInfo.AddDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
    }

    public void OnControlReleased()
    {
      if (MySession.Static.TopMostControlledEntity != this.CubeGrid)
        return;
      MyHud.BlockInfo.RemoveDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
    }

    public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
    {
      MyGunStatusEnum status;
      this.CanShoot(MyShootActionEnum.PrimaryAction, playerId, out status);
      if (status == MyGunStatusEnum.OK || status == MyGunStatusEnum.Cooldown)
      {
        if (fullUpdate)
        {
          Vector3D from = this.PositionComp.GetPosition() + this.PositionComp.WorldMatrixRef.Forward;
          Vector3D vector3D = this.PositionComp.GetPosition() + 200.0 * this.PositionComp.WorldMatrixRef.Forward;
          Vector3D zero = Vector3D.Zero;
          Vector3D to = vector3D;
          ref Vector3D local = ref zero;
          if (MyHudCrosshair.GetTarget(from, to, ref local))
          {
            MatrixD worldMatrix = this.WorldMatrix;
            MatrixD result;
            MatrixD.Invert(ref worldMatrix, out result);
            Vector3D.Transform(ref zero, ref result, out this.m_targetLocal);
          }
          else
            this.m_targetLocal = (Vector3D) Vector3.Zero;
        }
        if (!Vector3.IsZero((Vector3) this.m_targetLocal))
        {
          Vector3D result = (Vector3D) Vector3.Zero;
          MatrixD worldMatrix = this.WorldMatrix;
          Vector3D.Transform(ref this.m_targetLocal, ref worldMatrix, out result);
          float num = (float) Vector3D.Distance(MySector.MainCamera.Position, result);
          MyTransparentGeometry.AddBillboardOriented(MyUserControllableGun.ID_RED_DOT, fullUpdate ? Vector4.One : new Vector4(0.6f, 0.6f, 0.6f, 0.6f), result, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, num / 300f, MyBillboard.BlendTypeEnum.LDR);
        }
      }
      MyHud.BlockInfo.MissingComponentIndex = -1;
      MyHud.BlockInfo.DefinitionId = this.BlockDefinition.Id;
      MyHud.BlockInfo.BlockName = this.BlockDefinition.DisplayNameText;
      MyHud.BlockInfo.SetContextHelp((MyDefinitionBase) this.BlockDefinition);
      MyHud.BlockInfo.PCUCost = 0;
      MyHud.BlockInfo.BlockIcons = this.BlockDefinition.Icons;
      MyHud.BlockInfo.BlockIntegrity = 1f;
      MyHud.BlockInfo.CriticalIntegrity = 0.0f;
      MyHud.BlockInfo.CriticalComponentIndex = 0;
      MyHud.BlockInfo.OwnershipIntegrity = 0.0f;
      MyHud.BlockInfo.BlockBuiltBy = 0L;
      MyHud.BlockInfo.GridSize = MyCubeSize.Small;
      MyHud.BlockInfo.Components.Clear();
    }

    public void DrawHud(IMyCameraController camera, long playerId)
    {
      MyGunStatusEnum status;
      this.CanShoot(MyShootActionEnum.PrimaryAction, playerId, out status);
      if (status != MyGunStatusEnum.OK && status != MyGunStatusEnum.Cooldown)
        return;
      Vector3D from = this.PositionComp.GetPosition() + this.PositionComp.WorldMatrixRef.Forward;
      Vector3D vector3D = this.PositionComp.GetPosition() + 200.0 * this.PositionComp.WorldMatrixRef.Forward;
      Vector3D zero = Vector3D.Zero;
      Vector3D to = vector3D;
      ref Vector3D local = ref zero;
      if (!MyHudCrosshair.GetTarget(from, to, ref local))
        return;
      float num = (float) Vector3D.Distance(MySector.MainCamera.Position, zero);
      MyTransparentGeometry.AddBillboardOriented(MyUserControllableGun.ID_RED_DOT, new Vector4(1f, 1f, 1f, 1f), zero, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, num / 300f, MyBillboard.BlendTypeEnum.LDR);
    }

    private void UpdatePower()
    {
      this.ResourceSink.Update();
      if (this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
        return;
      this.EndShoot(MyShootActionEnum.PrimaryAction);
    }

    public void StartNoAmmoSound() => this.m_gunBase.StartNoAmmoSound(this.m_soundEmitter);

    private void StopLoopSound()
    {
      if (this.m_soundEmitter != null && this.m_soundEmitter.IsPlaying && this.m_soundEmitter.Loop)
        this.m_soundEmitter.StopSound(true);
      if (this.m_soundEmitterRotor == null || !this.m_soundEmitterRotor.IsPlaying || !this.m_soundEmitterRotor.Loop)
        return;
      this.m_soundEmitterRotor.StopSound(true);
      this.m_soundEmitterRotor.PlaySound(this.m_gunBase.SecondarySound, skipToEnd: true);
    }

    private void PlayShotSound() => this.m_gunBase.StartShootSound(this.m_soundEmitter);

    private void StartLoopSound()
    {
      if (this.m_soundEmitterRotor == null || this.m_gunBase.SecondarySound == MySoundPair.Empty || this.m_soundEmitterRotor.IsPlaying && this.m_soundEmitterRotor.Loop || !this.IsWorking)
        return;
      if (this.m_soundEmitterRotor.IsPlaying)
        this.m_soundEmitterRotor.StopSound(true);
      this.m_soundEmitterRotor.PlaySound(this.m_gunBase.SecondarySound, true);
    }

    private bool UseConveyorSystem
    {
      get => (bool) this.m_useConveyorSystem;
      set => this.m_useConveyorSystem.Value = value;
    }

    public int GetTotalAmmunitionAmount() => this.m_gunBase.GetTotalAmmunitionAmount();

    public int GetAmmunitionAmount() => this.m_gunBase.GetAmmunitionAmount();

    public int GetMagazineAmount() => this.m_gunBase.GetMagazineAmount();

    public MyGunBase GunBase => this.m_gunBase;

    public override void OnModelChange()
    {
      base.OnModelChange();
      if (this.IsBuilt)
        this.GetBarrelAndMuzzle();
      else
        this.m_barrel = (MyEntity) null;
    }

    private void GetBarrelAndMuzzle()
    {
      MyEntitySubpart myEntitySubpart;
      if (!this.Subparts.TryGetValue("Barrel", out myEntitySubpart))
        return;
      this.m_barrel = (MyEntity) myEntitySubpart;
      MyModel model = this.m_barrel.Model;
      this.m_gunBase.LoadDummies(model.Dummies);
      if (this.m_gunBase.HasDummies)
        return;
      if (model.Dummies.ContainsKey("Muzzle"))
        this.m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, model.Dummies["Muzzle"].Matrix);
      else
        this.m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, -1f)));
    }

    private void UpdateMuzzlePosition()
    {
      if (this.m_gunBase == null || this.m_barrel == null)
        return;
      this.m_gunBase.WorldMatrix = this.m_barrel.PositionComp.WorldMatrixRef;
    }

    MyEntity[] IMyGunBaseUser.IgnoreEntities => this.m_shootIgnoreEntities;

    MyEntity IMyGunBaseUser.Weapon => this.Parent;

    MyEntity IMyGunBaseUser.Owner => this.Parent;

    IMyMissileGunObject IMyGunBaseUser.Launcher => (IMyMissileGunObject) this;

    MyInventory IMyGunBaseUser.AmmoInventory => MyEntityExtensions.GetInventory(this);

    MyDefinitionId IMyGunBaseUser.PhysicalItemId => new MyDefinitionId();

    MyInventory IMyGunBaseUser.WeaponInventory => (MyInventory) null;

    long IMyGunBaseUser.OwnerId => this.OwnerId;

    string IMyGunBaseUser.ConstraintDisplayName => this.BlockDefinition.DisplayNameText;

    bool Sandbox.ModAPI.Ingame.IMySmallGatlingGun.UseConveyorSystem => (bool) this.m_useConveyorSystem;

    public override bool CanOperate() => this.CheckIsWorking();

    public override void ShootFromTerminal(Vector3 direction)
    {
      base.ShootFromTerminal(direction);
      this.Shoot(MyShootActionEnum.PrimaryAction, direction, new Vector3D?(), (string) null);
    }

    public override void StopShootFromTerminal()
    {
    }

    public void UpdateSoundEmitter()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.Update();
    }

    public bool SupressShootAnimation() => false;

    int IMyInventoryOwner.InventoryCount => this.InventoryCount;

    long IMyInventoryOwner.EntityId => this.EntityId;

    bool IMyInventoryOwner.HasInventory => this.HasInventory;

    bool IMyInventoryOwner.UseConveyorSystem
    {
      get => this.UseConveyorSystem;
      set => this.UseConveyorSystem = value;
    }

    VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(
      int index)
    {
      return (VRage.Game.ModAPI.Ingame.IMyInventory) MyEntityExtensions.GetInventory(this, index);
    }

    public PullInformation GetPullInformation()
    {
      PullInformation pullInformation = new PullInformation()
      {
        Inventory = MyEntityExtensions.GetInventory(this),
        OwnerID = this.OwnerId
      };
      pullInformation.Constraint = pullInformation.Inventory.Constraint;
      return pullInformation;
    }

    public PullInformation GetPushInformation() => (PullInformation) null;

    public bool AllowSelfPulling() => false;

    public void MissileShootEffect() => this.m_gunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);

    public void ShootMissile(MyObjectBuilder_Missile builder) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySmallGatlingGun, MyObjectBuilder_Missile>(this, (Func<MySmallGatlingGun, Action<MyObjectBuilder_Missile>>) (x => new Action<MyObjectBuilder_Missile>(x.OnShootMissile)), builder);

    [Event(null, 1144)]
    [Reliable]
    [Server]
    [Broadcast]
    private void OnShootMissile(MyObjectBuilder_Missile builder) => MyMissiles.Add(builder);

    public void RemoveMissile(long entityId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySmallGatlingGun, long>(this, (Func<MySmallGatlingGun, Action<long>>) (x => new Action<long>(x.OnRemoveMissile)), entityId);

    [Event(null, 1157)]
    [Reliable]
    [Broadcast]
    private void OnRemoveMissile(long entityId) => MyMissiles.Remove(entityId);

    public Vector3D GetMuzzlePosition() => this.m_gunBase.GetMuzzleWorldPosition();

    protected sealed class OnShootMissile\u003C\u003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<MySmallGatlingGun, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySmallGatlingGun @this,
        in MyObjectBuilder_Missile builder,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnShootMissile(builder);
      }
    }

    protected sealed class OnRemoveMissile\u003C\u003ESystem_Int64 : ICallSite<MySmallGatlingGun, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySmallGatlingGun @this,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveMissile(entityId);
      }
    }

    protected class m_lateStartRandom\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.FromServer>(obj1, obj2));
        ((MySmallGatlingGun) obj0).m_lateStartRandom = (VRage.Sync.Sync<int, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_cachedAmmunitionAmount\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.FromServer>(obj1, obj2));
        ((MySmallGatlingGun) obj0).m_cachedAmmunitionAmount = (VRage.Sync.Sync<int, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_useConveyorSystem\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MySmallGatlingGun) obj0).m_useConveyorSystem = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Weapons_MySmallGatlingGun\u003C\u003EActor : IActivator, IActivator<MySmallGatlingGun>
    {
      object IActivator.CreateInstance() => (object) new MySmallGatlingGun();

      MySmallGatlingGun IActivator<MySmallGatlingGun>.CreateInstance() => new MySmallGatlingGun();
    }
  }
}
