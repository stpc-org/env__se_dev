// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MySmallMissileLauncher
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
  [MyCubeBlockType(typeof (MyObjectBuilder_SmallMissileLauncher))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMySmallMissileLauncher), typeof (Sandbox.ModAPI.Ingame.IMySmallMissileLauncher)})]
  public class MySmallMissileLauncher : MyUserControllableGun, IMyMissileGunObject, IMyGunObject<MyGunBase>, IMyInventoryOwner, IMyConveyorEndpointBlock, IMyGunBaseUser, Sandbox.ModAPI.IMySmallMissileLauncher, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMySmallMissileLauncher
  {
    protected int m_shotsLeftInBurst;
    protected int m_nextShootTime;
    private int m_nextNotificationTime;
    private MyHudNotification m_reloadNotification;
    private MyGunBase m_gunBase;
    private bool m_shoot;
    private Vector3 m_shootDirection;
    private int m_currentBarrel;
    private int m_lateStartRandom = MyUtils.GetRandomInt(0, 3);
    private int m_currentLateStart;
    private Vector3D m_targetLocal = (Vector3D) Vector3.Zero;
    private MyEntity[] m_shootIgnoreEntities;
    private MyHudNotification m_safezoneNotification;
    private VRage.Sync.Sync<int, SyncDirection.FromServer> m_cachedAmmunitionAmount;
    private MyMultilineConveyorEndpoint m_endpoint;
    protected VRage.Sync.Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

    protected MyHudNotification ReloadNotification
    {
      get
      {
        if (this.m_reloadNotification == null)
          this.m_reloadNotification = new MyHudNotification(MySpaceTexts.MissileLauncherReloadingNotification, this.m_gunBase.ReloadTime - 250, level: MyNotificationLevel.Important);
        return this.m_reloadNotification;
      }
    }

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_endpoint;

    public bool IsSkinnable => false;

    public void InitializeConveyorEndpoint()
    {
      this.m_endpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawConveyorEndpoint((IMyConveyorEndpoint) this.m_endpoint));
    }

    public override bool IsStationary() => true;

    public override Vector3D GetWeaponMuzzleWorldPosition() => this.m_gunBase != null ? this.m_gunBase.GetMuzzleWorldPosition() : base.GetWeaponMuzzleWorldPosition();

    public MySmallMissileLauncher()
    {
      this.m_shootIgnoreEntities = new MyEntity[1]
      {
        (MyEntity) this
      };
      this.CreateTerminalControls();
      this.m_gunBase = new MyGunBase();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_gunBase.OnAmmoAmountChanged += new Action(this.OnAmmoAmountChangedOnServer);
      else
        this.m_cachedAmmunitionAmount.ValueChanged += new Action<SyncBase>(this.OnAmmoAmountChangedFromServer);
      this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this, true);
      this.SyncType.Append((object) this.m_gunBase);
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MySmallMissileLauncher>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MySmallMissileLauncher> onOff = new MyTerminalControlOnOffSwitch<MySmallMissileLauncher>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem);
      onOff.Getter = (MyTerminalValueControl<MySmallMissileLauncher, bool>.GetterDelegate) (x => x.UseConveyorSystem);
      onOff.Setter = (MyTerminalValueControl<MySmallMissileLauncher, bool>.SetterDelegate) ((x, v) => x.UseConveyorSystem = v);
      onOff.Visible = (Func<MySmallMissileLauncher, bool>) (x => x.CubeGrid.GridSizeEnum == MyCubeSize.Large);
      onOff.EnableToggleAction<MySmallMissileLauncher>();
      MyTerminalControlFactory.AddControl<MySmallMissileLauncher>((MyTerminalControl<MySmallMissileLauncher>) onOff);
    }

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      MyObjectBuilder_SmallMissileLauncher smallMissileLauncher = builder as MyObjectBuilder_SmallMissileLauncher;
      MyStringHash group;
      if (this.BlockDefinition is MyWeaponBlockDefinition blockDefinition && MyEntityExtensions.GetInventory(this) == null)
      {
        this.Components.Add<MyInventoryBase>((MyInventoryBase) new MyInventory(blockDefinition.InventoryMaxVolume, new Vector3(1.2f, 0.98f, 0.98f), MyInventoryFlags.CanReceive));
        group = blockDefinition.ResourceSinkGroup;
      }
      else
      {
        if (MyEntityExtensions.GetInventory(this) == null)
          this.Components.Add<MyInventory>(cubeGrid.GridSizeEnum != MyCubeSize.Small ? new MyInventory(1.14f, new Vector3(1.2f, 0.98f, 0.98f), MyInventoryFlags.CanReceive) : new MyInventory(0.24f, new Vector3(1.2f, 0.45f, 0.45f), MyInventoryFlags.CanReceive));
        group = MyStringHash.GetOrCompute("Defense");
      }
      MyEntityExtensions.GetInventory(this);
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(group, 0.0002f, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      this.ResourceSink = resourceSinkComponent;
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.m_gunBase.Init(smallMissileLauncher.GunBase, this.BlockDefinition, (IMyGunBaseUser) this);
      base.Init(builder, cubeGrid);
      if (MyFakes.ENABLE_INVENTORY_FIX)
        this.FixSingleInventory();
      this.ResourceSink.Update();
      MyEntityExtensions.GetInventory(this).Init(smallMissileLauncher.Inventory);
      this.m_shotsLeftInBurst = this.m_gunBase.ShotsInBurst;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentDrawPowerReciever(this.ResourceSink, (VRage.ModAPI.IMyEntity) this));
      if (this.CubeGrid.GridSizeEnum == MyCubeSize.Large)
        this.m_useConveyorSystem.SetLocalValue(smallMissileLauncher.UseConveyorSystem);
      else
        this.m_useConveyorSystem.SetLocalValue(false);
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.LoadDummies();
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.SyncType.Append((object) this.m_gunBase);
    }

    private void OnAmmoAmountChangedFromServer(SyncBase obj) => this.GunBase.CurrentAmmo = this.m_cachedAmmunitionAmount.Value;

    private void OnAmmoAmountChangedOnServer()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_cachedAmmunitionAmount.Value = this.GunBase.CurrentAmmo;
    }

    protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
    {
      base.OnInventoryComponentAdded(inventory);
      if (MyEntityExtensions.GetInventory(this) == null)
        return;
      MyEntityExtensions.GetInventory(this).ContentsChanged += new Action<MyInventoryBase>(this.m_ammoInventory_ContentsChanged);
    }

    protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
    {
      base.OnInventoryComponentRemoved(inventory);
      if (!(inventory is MyInventory myInventory))
        return;
      myInventory.ContentsChanged -= new Action<MyInventoryBase>(this.m_ammoInventory_ContentsChanged);
    }

    private void LoadDummies()
    {
      MyModel modelOnlyDummies = MyModels.GetModelOnlyDummies(this.BlockDefinition.Model);
      this.m_gunBase.LoadDummies(modelOnlyDummies.Dummies);
      if (this.m_gunBase.HasDummies)
        return;
      foreach (KeyValuePair<string, MyModelDummy> dummy in modelOnlyDummies.Dummies)
      {
        if (dummy.Key.ToLower().Contains("barrel"))
          this.m_gunBase.AddMuzzleMatrix(MyAmmoType.Missile, dummy.Value.Matrix);
      }
    }

    private void Receiver_IsPoweredChanged() => this.UpdateIsWorking();

    private void m_ammoInventory_ContentsChanged(MyInventoryBase obj) => this.m_gunBase.RefreshAmmunitionAmount(true);

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_SmallMissileLauncher builderCubeBlock = (MyObjectBuilder_SmallMissileLauncher) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.UseConveyorSystem = (bool) this.m_useConveyorSystem;
      builderCubeBlock.GunBase = this.m_gunBase.GetObjectBuilder();
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.ResourceSink.Update();
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

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

    private Vector3 GetSmokePosition() => (Vector3) (this.m_gunBase.GetMuzzleWorldPosition() - this.WorldMatrix.Forward * 0.5);

    public float BackkickForcePerSecond => this.m_gunBase.BackkickForcePerSecond;

    public float ShakeAmount { get; protected set; }

    public bool IsControlled => this.Controller != null;

    public MyCharacter Controller { get; protected set; }

    public void OnControlAcquired(MyCharacter owner)
    {
      this.Controller = owner;
      if (owner != MySession.Static.LocalCharacter)
        return;
      MyHud.BlockInfo.AddDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
    }

    public void OnControlReleased()
    {
      if (MySession.Static.TopMostControlledEntity == this.CubeGrid)
        MyHud.BlockInfo.RemoveDisplayer(MyHudBlockInfo.WhoWantsInfoDisplayed.Tool);
      this.Controller = (MyCharacter) null;
    }

    public bool EnabledInWorldRules => MySession.Static.WeaponsEnabled;

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || !this.IsFunctional || (!this.UseConveyorSystem || !MySession.Static.SurvivalMode))
        return;
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if ((double) inventory.VolumeFillFactor * (double) MySession.Static.BlocksInventorySizeMultiplier >= 1.0)
        return;
      this.CubeGrid.GridSystems.ConveyorSystem.PullItem(this.m_gunBase.CurrentAmmoMagazineId, new MyFixedPoint?((MyFixedPoint) (this.m_gunBase.WeaponProperties.CurrentWeaponRateOfFire / 36 + 1)), (IMyConveyorEndpointBlock) this, inventory, false, false);
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (this.m_shoot)
        this.ShootMissile();
      this.UpdateReloadNotification();
      this.m_shoot = false;
      this.NeedsUpdate &= ~MyEntityUpdateEnum.NONE;
    }

    public bool Zoom(bool newKeyPress) => false;

    public void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate)
    {
      MyGunStatusEnum status;
      this.CanShoot(MyShootActionEnum.PrimaryAction, playerId, out status);
      if (status == MyGunStatusEnum.OK || status == MyGunStatusEnum.Cooldown)
      {
        if (fullUpdate)
        {
          MatrixD muzzleWorldMatrix = this.m_gunBase.GetMuzzleWorldMatrix();
          Vector3D translation = muzzleWorldMatrix.Translation;
          Vector3D to = translation + 200.0 * muzzleWorldMatrix.Forward;
          Vector3D zero = Vector3D.Zero;
          if (MyHudCrosshair.GetTarget(translation, to, ref zero))
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
      MatrixD muzzleWorldMatrix = this.m_gunBase.GetMuzzleWorldMatrix();
      Vector3D translation = muzzleWorldMatrix.Translation;
      Vector3D to = translation + 200.0 * muzzleWorldMatrix.Forward;
      Vector3D zero = Vector3D.Zero;
      if (!MyHudCrosshair.GetTarget(translation, to, ref zero))
        return;
      float num = (float) Vector3D.Distance(MySector.MainCamera.Position, zero);
      MyTransparentGeometry.AddBillboardOriented(MyUserControllableGun.ID_RED_DOT, Vector4.One, zero, MySector.MainCamera.LeftVector, MySector.MainCamera.UpVector, num / 300f, MyBillboard.BlendTypeEnum.LDR);
    }

    public MyDefinitionId DefinitionId => this.BlockDefinition.Id;

    public void UpdateSoundEmitter()
    {
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.Update();
    }

    private void StartSound(MySoundPair cueEnum) => this.m_gunBase.StartShootSound(this.m_soundEmitter);

    protected override void Closing()
    {
      if (this.m_soundEmitter != null)
        this.m_soundEmitter.StopSound(true);
      base.Closing();
    }

    public override void OnAddedToScene(object source) => base.OnAddedToScene(source);

    public bool UseConveyorSystem
    {
      get => (bool) this.m_useConveyorSystem;
      set => this.m_useConveyorSystem.Value = value;
    }

    public int GetTotalAmmunitionAmount() => this.m_gunBase.GetTotalAmmunitionAmount();

    public int GetAmmunitionAmount() => this.m_gunBase.GetAmmunitionAmount();

    public int GetMagazineAmount() => this.m_gunBase.GetMagazineAmount();

    public Vector3 DirectionToTarget(Vector3D target) => (Vector3) this.WorldMatrix.Forward;

    public override bool CanShoot(
      MyShootActionEnum action,
      long shooter,
      out MyGunStatusEnum status)
    {
      status = MyGunStatusEnum.OK;
      if (action != MyShootActionEnum.PrimaryAction)
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (!MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.CubeGrid, MySafeZoneAction.Shooting))
      {
        status = MyGunStatusEnum.SafeZoneDenied;
        return false;
      }
      if (!this.m_gunBase.HasAmmoMagazines)
      {
        status = MyGunStatusEnum.Failed;
        return false;
      }
      if (this.m_nextShootTime > MySandboxGame.TotalGamePlayTimeInMilliseconds)
      {
        status = MyGunStatusEnum.Cooldown;
        return false;
      }
      if (this.m_shotsLeftInBurst == 0 && this.m_gunBase.ShotsInBurst > 0)
      {
        status = MyGunStatusEnum.Failed;
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
      if (MySession.Static.CreativeMode || this.m_gunBase.HasEnoughAmmunition())
        return true;
      status = MyGunStatusEnum.OutOfAmmo;
      return false;
    }

    public virtual void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      if (this.m_shootingBegun && this.m_lateStartRandom > this.m_currentLateStart)
      {
        ++this.m_currentLateStart;
      }
      else
      {
        this.m_shoot = true;
        this.m_shootDirection = direction;
        this.m_gunBase.ConsumeAmmo();
        this.m_nextShootTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + this.m_gunBase.ShootIntervalInMiliseconds;
        if (this.m_gunBase.ShotsInBurst > 0)
        {
          --this.m_shotsLeftInBurst;
          if (this.m_shotsLeftInBurst <= 0)
          {
            this.m_nextShootTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + this.m_gunBase.ReloadTime;
            this.m_shotsLeftInBurst = this.m_gunBase.ShotsInBurst;
          }
        }
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      }
    }

    public new void EndShoot(MyShootActionEnum action)
    {
      base.EndShoot(action);
      this.m_currentLateStart = 0;
    }

    public bool IsShooting => this.m_nextShootTime > MySandboxGame.TotalGamePlayTimeInMilliseconds;

    public int ShootDirectionUpdateTime => 0;

    public bool NeedsShootDirectionWhileAiming => false;

    public float MaximumShotLength => 0.0f;

    public void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status)
    {
      if (status == MyGunStatusEnum.OutOfAmmo && !MySession.Static.CreativeMode)
        this.m_gunBase.StartNoAmmoSound(this.m_soundEmitter);
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

    public void ShootMissile()
    {
      if (this.m_gunBase == null)
        MySandboxGame.Log.WriteLine("Missile launcher barrel null");
      else if (this.Parent.Physics == null || (HkReferenceObject) this.Parent.Physics.RigidBody == (HkReferenceObject) null)
        MySandboxGame.Log.WriteLine("Missile launcher parent physics null");
      else
        this.ShootMissile(this.Parent.Physics.LinearVelocity);
    }

    public void ShootMissile(Vector3 velocity)
    {
      this.StartSound(this.m_gunBase.ShootSound);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.m_gunBase.Shoot(velocity, this.Controller != null ? this.Controller.IsUsing : (MyEntity) null);
    }

    public MyGunBase GunBase => this.m_gunBase;

    protected override void WorldPositionChanged(object source)
    {
      base.WorldPositionChanged(source);
      this.m_gunBase.WorldMatrix = this.WorldMatrix;
    }

    private void UpdateReloadNotification()
    {
      if (MySandboxGame.TotalGamePlayTimeInMilliseconds > this.m_nextNotificationTime)
        this.m_reloadNotification = (MyHudNotification) null;
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      if (this.Controller != MySession.Static.LocalCharacter)
      {
        if (this.m_reloadNotification == null)
          return;
        MyHud.Notifications.Remove((MyHudNotificationBase) this.m_reloadNotification);
        this.m_reloadNotification = (MyHudNotification) null;
      }
      else
      {
        if (this.m_nextShootTime <= MySandboxGame.TotalGamePlayTimeInMilliseconds || this.m_nextShootTime - MySandboxGame.TotalGamePlayTimeInMilliseconds <= this.m_gunBase.ShootIntervalInMiliseconds)
          return;
        this.ShowReloadNotification(this.m_nextShootTime - MySandboxGame.TotalGamePlayTimeInMilliseconds);
      }
    }

    private void ShowReloadNotification(int duration)
    {
      int num = MySandboxGame.TotalGamePlayTimeInMilliseconds + duration;
      if (this.m_reloadNotification == null)
      {
        duration = Math.Max(0, duration - 250);
        if (duration == 0)
          return;
        this.m_reloadNotification = new MyHudNotification(MySpaceTexts.LargeMissileTurretReloadingNotification, duration, level: MyNotificationLevel.Important);
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_reloadNotification);
        this.m_nextNotificationTime = num;
      }
      else
      {
        this.m_reloadNotification.AddAliveTime(num - this.m_nextNotificationTime);
        this.m_nextNotificationTime = num;
      }
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

    bool Sandbox.ModAPI.Ingame.IMySmallMissileLauncher.UseConveyorSystem => (bool) this.m_useConveyorSystem;

    public override bool CanOperate() => this.CheckIsWorking();

    public override void ShootFromTerminal(Vector3 direction)
    {
      base.ShootFromTerminal(direction);
      this.Shoot(MyShootActionEnum.PrimaryAction, direction, new Vector3D?(), (string) null);
    }

    public override void StopShootFromTerminal()
    {
    }

    public bool SupressShootAnimation() => false;

    public void MissileShootEffect() => this.m_gunBase.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);

    public void ShootMissile(MyObjectBuilder_Missile builder) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySmallMissileLauncher, MyObjectBuilder_Missile>(this, (Func<MySmallMissileLauncher, Action<MyObjectBuilder_Missile>>) (x => new Action<MyObjectBuilder_Missile>(x.OnShootMissile)), builder);

    [Event(null, 906)]
    [Reliable]
    [Server]
    [Broadcast]
    private void OnShootMissile(MyObjectBuilder_Missile builder) => MyMissiles.Add(builder);

    public void RemoveMissile(long entityId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MySmallMissileLauncher, long>(this, (Func<MySmallMissileLauncher, Action<long>>) (x => new Action<long>(x.OnRemoveMissile)), entityId);

    [Event(null, 917)]
    [Reliable]
    [Broadcast]
    private void OnRemoveMissile(long entityId) => MyMissiles.Remove(entityId);

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

    public Vector3D GetMuzzlePosition() => this.m_gunBase.GetMuzzleWorldPosition();

    protected sealed class OnShootMissile\u003C\u003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<MySmallMissileLauncher, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySmallMissileLauncher @this,
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

    protected sealed class OnRemoveMissile\u003C\u003ESystem_Int64 : ICallSite<MySmallMissileLauncher, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySmallMissileLauncher @this,
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

    protected class m_cachedAmmunitionAmount\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.FromServer>(obj1, obj2));
        ((MySmallMissileLauncher) obj0).m_cachedAmmunitionAmount = (VRage.Sync.Sync<int, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_useConveyorSystem\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MySmallMissileLauncher) obj0).m_useConveyorSystem = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Weapons_MySmallMissileLauncher\u003C\u003EActor : IActivator, IActivator<MySmallMissileLauncher>
    {
      object IActivator.CreateInstance() => (object) new MySmallMissileLauncher();

      MySmallMissileLauncher IActivator<MySmallMissileLauncher>.CreateInstance() => new MySmallMissileLauncher();
    }
  }
}
