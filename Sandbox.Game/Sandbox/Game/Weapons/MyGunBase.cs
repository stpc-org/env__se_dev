// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyGunBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
  public class MyGunBase : MyDeviceBase
  {
    private static readonly bool DEBUG_PROJECTILE_VIEW_TRAJECTORIES;
    public const int AMMO_PER_SHOOT = 1;
    protected MyWeaponPropertiesWrapper m_weaponProperties;
    protected Dictionary<MyDefinitionId, int> m_remainingAmmos;
    protected Dictionary<int, MyGunBase.DummyContainer> m_dummiesByAmmoType;
    protected MatrixD m_worldMatrix;
    protected IMyGunBaseUser m_user;
    private List<MyGunBase.WeaponEffect> m_activeEffects = new List<MyGunBase.WeaponEffect>();
    public Matrix m_holdingDummyMatrix;
    private int m_shotProjectiles;
    private Dictionary<string, MyModelDummy> m_dummies;
    private int m_currentAmmo;

    public int CurrentAmmo
    {
      get => this.m_currentAmmo;
      set
      {
        if (this.m_currentAmmo == value)
          return;
        this.m_currentAmmo = value;
        Action ammoAmountChanged = this.OnAmmoAmountChanged;
        if (ammoAmountChanged == null)
          return;
        ammoAmountChanged();
      }
    }

    public event Action OnAmmoAmountChanged;

    public int CurrentMagazines { get; set; }

    public MyWeaponPropertiesWrapper WeaponProperties => this.m_weaponProperties;

    public MyAmmoMagazineDefinition CurrentAmmoMagazineDefinition => this.WeaponProperties.AmmoMagazineDefinition;

    public MyDefinitionId CurrentAmmoMagazineId => this.WeaponProperties.AmmoMagazineId;

    public MyAmmoDefinition CurrentAmmoDefinition => this.WeaponProperties.AmmoDefinition;

    public float BackkickForcePerSecond => this.WeaponProperties != null && this.WeaponProperties.AmmoDefinition != null ? this.WeaponProperties.AmmoDefinition.BackkickForce : 0.0f;

    public bool HasMissileAmmoDefined => this.m_weaponProperties.WeaponDefinition.HasMissileAmmoDefined;

    public bool HasProjectileAmmoDefined => this.m_weaponProperties.WeaponDefinition.HasProjectileAmmoDefined;

    public int MuzzleFlashLifeSpan => this.m_weaponProperties.WeaponDefinition.MuzzleFlashLifeSpan;

    public int ShootIntervalInMiliseconds => (double) this.ShootIntervalModifier != 1.0 ? (int) ((double) this.ShootIntervalModifier * (double) this.m_weaponProperties.CurrentWeaponShootIntervalInMiliseconds) : this.m_weaponProperties.CurrentWeaponShootIntervalInMiliseconds;

    public float ShootIntervalModifier { get; set; }

    public float ReleaseTimeAfterFire => this.m_weaponProperties.WeaponDefinition.ReleaseTimeAfterFire;

    public MySoundPair ShootSound => this.m_weaponProperties.CurrentWeaponShootSound;

    public MySoundPair NoAmmoSound => this.m_weaponProperties.WeaponDefinition.NoAmmoSound;

    public MySoundPair ReloadSound => this.m_weaponProperties.WeaponDefinition.ReloadSound;

    public MySoundPair SecondarySound => this.m_weaponProperties.WeaponDefinition.SecondarySound;

    public bool UseDefaultMuzzleFlash => this.m_weaponProperties.WeaponDefinition.UseDefaultMuzzleFlash;

    public float MechanicalDamage => this.WeaponProperties.AmmoDefinition != null ? this.m_weaponProperties.AmmoDefinition.GetDamageForMechanicalObjects() : 0.0f;

    public float DeviateAngle => this.m_weaponProperties.WeaponDefinition.DeviateShotAngle;

    public float DeviateAngleWhileAiming => this.m_weaponProperties.WeaponDefinition.DeviateShotAngleAiming;

    public bool HasAmmoMagazines => this.m_weaponProperties.WeaponDefinition.HasAmmoMagazines();

    public bool IsAmmoProjectile => this.m_weaponProperties.IsAmmoProjectile;

    public bool IsAmmoMissile => this.m_weaponProperties.IsAmmoMissile;

    public int ShotsInBurst => this.WeaponProperties.ShotsInBurst;

    public int ReloadTime => this.WeaponProperties.ReloadTime;

    public bool HasDummies => this.m_dummiesByAmmoType.Count > 0;

    public int DummiesPerType(MyAmmoType ammoType) => this.m_dummiesByAmmoType.ContainsKey((int) ammoType) ? this.m_dummiesByAmmoType[(int) ammoType].Dummies.Count : 0;

    public MatrixD WorldMatrix
    {
      set
      {
        this.m_worldMatrix = value;
        this.RecalculateMuzzles();
        this.UpdateEffectPositions();
      }
      get => this.m_worldMatrix;
    }

    public DateTime LastShootTime { get; private set; }

    public bool HasIronSightsActive { get; set; }

    public MyGunBase()
    {
      this.m_dummiesByAmmoType = new Dictionary<int, MyGunBase.DummyContainer>();
      this.m_remainingAmmos = new Dictionary<MyDefinitionId, int>();
      this.ShootIntervalModifier = 1f;
    }

    public MyObjectBuilder_GunBase GetObjectBuilder()
    {
      MyObjectBuilder_GunBase newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_GunBase>();
      newObject.CurrentAmmoMagazineName = this.CurrentAmmoMagazineId.SubtypeName;
      newObject.RemainingAmmo = this.CurrentAmmo;
      newObject.RemainingMagazines = this.CurrentMagazines;
      newObject.LastShootTime = this.LastShootTime.Ticks;
      newObject.RemainingAmmosList = new List<MyObjectBuilder_GunBase.RemainingAmmoIns>();
      foreach (KeyValuePair<MyDefinitionId, int> remainingAmmo in this.m_remainingAmmos)
        newObject.RemainingAmmosList.Add(new MyObjectBuilder_GunBase.RemainingAmmoIns()
        {
          SubtypeName = remainingAmmo.Key.SubtypeName,
          Amount = remainingAmmo.Value
        });
      newObject.InventoryItemId = this.InventoryItemId;
      return newObject;
    }

    public void Init(
      MyObjectBuilder_GunBase objectBuilder,
      MyCubeBlockDefinition cubeBlockDefinition,
      IMyGunBaseUser gunBaseUser)
    {
      if (cubeBlockDefinition is MyWeaponBlockDefinition)
      {
        MyWeaponBlockDefinition weaponBlockDefinition = cubeBlockDefinition as MyWeaponBlockDefinition;
        this.Init(objectBuilder, weaponBlockDefinition.WeaponDefinitionId, gunBaseUser);
      }
      else
      {
        MyDefinitionId compatibleDefinitionId = this.GetBackwardCompatibleDefinitionId(cubeBlockDefinition.Id.TypeId);
        this.Init(objectBuilder, compatibleDefinitionId, gunBaseUser);
      }
    }

    public void Init(
      MyObjectBuilder_GunBase objectBuilder,
      MyDefinitionId weaponDefinitionId,
      IMyGunBaseUser gunBaseUser)
    {
      if (objectBuilder != null)
        this.Init((MyObjectBuilder_DeviceBase) objectBuilder);
      this.m_user = gunBaseUser;
      this.m_weaponProperties = new MyWeaponPropertiesWrapper(weaponDefinitionId);
      this.PerformCheck(weaponDefinitionId, this.m_weaponProperties);
      this.m_remainingAmmos = new Dictionary<MyDefinitionId, int>(this.WeaponProperties.AmmoMagazinesCount);
      if (objectBuilder != null)
      {
        MyDefinitionId newAmmoMagazineId = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AmmoMagazine), objectBuilder.CurrentAmmoMagazineName);
        if (this.m_weaponProperties.CanChangeAmmoMagazine(newAmmoMagazineId))
        {
          this.CurrentAmmo = objectBuilder.RemainingAmmo;
          this.CurrentMagazines = objectBuilder.RemainingMagazines;
          this.m_weaponProperties.ChangeAmmoMagazine(newAmmoMagazineId);
        }
        else if (this.WeaponProperties.WeaponDefinition.HasAmmoMagazines())
          this.m_weaponProperties.ChangeAmmoMagazine(this.m_weaponProperties.WeaponDefinition.AmmoMagazinesId[0]);
        foreach (MyObjectBuilder_GunBase.RemainingAmmoIns remainingAmmos in objectBuilder.RemainingAmmosList)
          this.m_remainingAmmos.Add(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_AmmoMagazine), remainingAmmos.SubtypeName), remainingAmmos.Amount);
        this.LastShootTime = new DateTime(objectBuilder.LastShootTime);
      }
      else
      {
        if (this.WeaponProperties.WeaponDefinition.HasAmmoMagazines())
          this.m_weaponProperties.ChangeAmmoMagazine(this.m_weaponProperties.WeaponDefinition.AmmoMagazinesId[0]);
        if (MySession.Static.CreativeMode)
          this.CurrentAmmo = this.WeaponProperties.AmmoMagazineDefinition.Capacity;
        this.LastShootTime = new DateTime(0L);
      }
      if (this.m_user.AmmoInventory != null)
      {
        if (this.m_user.PutConstraint())
          this.m_user.AmmoInventory.Constraint = this.CreateAmmoInventoryConstraints(this.m_user.ConstraintDisplayName);
        this.RefreshAmmunitionAmount();
      }
      if (this.m_user.Weapon == null)
        return;
      this.m_user.Weapon.OnClosing += new Action<MyEntity>(this.Weapon_OnClosing);
    }

    private void PerformCheck(MyDefinitionId blockId, MyWeaponPropertiesWrapper weaponProperties)
    {
      if (weaponProperties != null)
      {
        MyDefinitionId weaponDefinitionId = weaponProperties.WeaponDefinitionId;
        if (weaponProperties.WeaponDefinition != null && (!weaponProperties.WeaponDefinition.HasAmmoMagazines() || weaponProperties.WeaponDefinition.AmmoMagazinesId != null && weaponProperties.WeaponDefinition.AmmoMagazinesId.Length != 0))
          return;
      }
      string empty = string.Empty;
      if (weaponProperties == null)
      {
        empty += "\nWeaponProperties is missing";
      }
      else
      {
        MyDefinitionId weaponDefinitionId = weaponProperties.WeaponDefinitionId;
        if (weaponProperties.WeaponDefinition == null)
          empty += "\nWeapon definition is missing";
        else if (weaponProperties.WeaponDefinition.HasAmmoMagazines() && (weaponProperties.WeaponDefinition.AmmoMagazinesId == null || weaponProperties.WeaponDefinition.AmmoMagazinesId.Length == 0))
          empty += "\nWeapon definition say it has ammo magazines defined, but no Ids are assigned.";
      }
      MyLog.Default.Error(string.Format("Weapon definition '{0}' is missing important data: {1}", (object) blockId, (object) empty));
    }

    private void Weapon_OnClosing(MyEntity obj)
    {
      if (this.m_user.Weapon == null)
        return;
      this.m_user.Weapon.OnClosing -= new Action<MyEntity>(this.Weapon_OnClosing);
    }

    public Vector3 GetDeviatedVector(float deviateAngle, Vector3 direction) => MyUtilRandomVector3ByDeviatingVector.GetRandom(direction, deviateAngle);

    private void AddProjectile(
      MyWeaponPropertiesWrapper weaponProperties,
      Vector3D initialPosition,
      Vector3D initialVelocity,
      Vector3D direction,
      MyEntity owner)
    {
      Vector3 directionNormalized = (Vector3) direction;
      if (weaponProperties.IsDeviated)
      {
        float deviateAngle = weaponProperties.WeaponDefinition.DeviateShotAngle;
        if (owner is MyCharacter && (((MyCharacter) owner).IsCrouching || ((MyCharacter) owner).ZoomMode == MyZoomModeEnum.IronSight))
          deviateAngle = weaponProperties.WeaponDefinition.DeviateShotAngleAiming;
        directionNormalized = this.GetDeviatedVector(deviateAngle, (Vector3) direction);
        double num = (double) directionNormalized.Normalize();
      }
      if (!directionNormalized.IsValid())
        return;
      ++this.m_shotProjectiles;
      if (MyGunBase.DEBUG_PROJECTILE_VIEW_TRAJECTORIES)
      {
        MyAdvancedDebugDraw.DebugDrawLine3DSync(initialPosition, initialPosition + 10.0 * direction, Color.Blue, Color.CornflowerBlue);
        if (!Sync.IsServer)
          MyRenderProxy.DebugDrawLine3D(MySector.MainCamera.Position, MySector.MainCamera.Position + 10f * MySector.MainCamera.ForwardVector, Color.Red, Color.Red, true, true);
      }
      MyProjectiles.Static.Add(weaponProperties, initialPosition, (Vector3) initialVelocity, directionNormalized, this.m_user, owner);
    }

    private void AddMissile(
      MyWeaponPropertiesWrapper weaponProperties,
      Vector3D initialPosition,
      Vector3 initialVelocity,
      Vector3 direction,
      MyEntity controller)
    {
      if (!Sync.IsServer)
        return;
      MyMissileAmmoDefinition ammoDefinitionAs = weaponProperties.GetCurrentAmmoDefinitionAs<MyMissileAmmoDefinition>();
      Vector3 vector3 = direction;
      if (weaponProperties.IsDeviated)
      {
        float deviateAngle = weaponProperties.WeaponDefinition.DeviateShotAngle;
        if (controller is MyCharacter && (((MyCharacter) controller).IsCrouching || ((MyCharacter) controller).ZoomMode == MyZoomModeEnum.IronSight))
          deviateAngle = weaponProperties.WeaponDefinition.DeviateShotAngleAiming;
        vector3 = this.GetDeviatedVector(deviateAngle, direction);
        double num = (double) vector3.Normalize();
      }
      if (!vector3.IsValid())
        return;
      initialVelocity += vector3 * ammoDefinitionAs.MissileInitialSpeed;
      long owner = 0;
      if (this.m_user.OwnerId != 0L)
        owner = this.m_user.OwnerId;
      else if (controller != null)
        owner = controller.EntityId;
      this.m_user.Launcher.ShootMissile(MyMissile.PrepareBuilder(weaponProperties, initialPosition, (Vector3D) initialVelocity, (Vector3D) vector3, owner, this.m_user.Owner.EntityId, (this.m_user.Launcher as MyEntity).EntityId));
    }

    public void Shoot(Vector3 initialVelocity, MyEntity owner = null)
    {
      MatrixD muzzleWorldMatrix = this.GetMuzzleWorldMatrix();
      this.Shoot(muzzleWorldMatrix.Translation, initialVelocity, (Vector3) muzzleWorldMatrix.Forward, owner);
    }

    public void Shoot(Vector3 initialVelocity, Vector3 direction, MyEntity owner = null) => this.Shoot(this.GetMuzzleWorldMatrix().Translation, initialVelocity, direction, owner);

    public void ShootWithOffset(
      Vector3 initialVelocity,
      Vector3 direction,
      float offset,
      MyEntity owner = null)
    {
      this.Shoot(this.GetMuzzleWorldMatrix().Translation + direction * offset, initialVelocity, direction, owner);
    }

    public void Shoot(
      Vector3D initialPosition,
      Vector3 initialVelocity,
      Vector3 direction,
      MyEntity owner = null)
    {
      MyAmmoDefinition ammoDefinition = this.m_weaponProperties.AmmoDefinition;
      switch (ammoDefinition.AmmoType)
      {
        case MyAmmoType.HighSpeed:
          int projectileCount = (ammoDefinition as MyProjectileAmmoDefinition).ProjectileCount;
          for (int index = 0; index < projectileCount; ++index)
            this.AddProjectile(this.m_weaponProperties, initialPosition, (Vector3D) initialVelocity, (Vector3D) direction, owner);
          break;
        case MyAmmoType.Missile:
          if (Sync.IsServer)
          {
            this.AddMissile(this.m_weaponProperties, initialPosition, initialVelocity, direction, owner);
            break;
          }
          break;
      }
      this.MoveToNextMuzzle(ammoDefinition.AmmoType);
      this.CreateEffects(MyWeaponDefinition.WeaponEffectAction.Shoot);
      this.LastShootTime = DateTime.UtcNow;
    }

    public void CreateEffects(MyWeaponDefinition.WeaponEffectAction action)
    {
      if (this.m_dummies == null || this.m_dummies.Count <= 0 || this.WeaponProperties.WeaponDefinition.WeaponEffects.Length == 0)
        return;
      for (int index1 = 0; index1 < this.WeaponProperties.WeaponDefinition.WeaponEffects.Length; ++index1)
      {
        MyModelDummy myModelDummy;
        if (this.WeaponProperties.WeaponDefinition.WeaponEffects[index1].Action == action && this.m_dummies.TryGetValue(this.WeaponProperties.WeaponDefinition.WeaponEffects[index1].Dummy, out myModelDummy))
        {
          bool flag = true;
          string empty = string.Empty;
          string particle = this.WeaponProperties.WeaponDefinition.WeaponEffects[index1].Particle;
          if (this.WeaponProperties.WeaponDefinition.WeaponEffects[index1].Loop)
          {
            for (int index2 = 0; index2 < this.m_activeEffects.Count; ++index2)
            {
              if (this.m_activeEffects[index2].DummyName == myModelDummy.Name && this.m_activeEffects[index2].EffectName == particle)
              {
                flag = false;
                break;
              }
            }
          }
          if (flag)
          {
            MatrixD matrix1 = MatrixD.Normalize((MatrixD) ref myModelDummy.Matrix);
            MyParticleEffect effect;
            if (MyParticlesManager.TryCreateParticleEffect(particle, MatrixD.Multiply(matrix1, this.WorldMatrix), out effect) && this.WeaponProperties.WeaponDefinition.WeaponEffects[index1].Loop)
              this.m_activeEffects.Add(new MyGunBase.WeaponEffect(particle, myModelDummy.Name, (Matrix) ref matrix1, action, effect, this.WeaponProperties.WeaponDefinition.WeaponEffects[index1].InstantStop));
          }
        }
      }
    }

    public void UpdateEffects()
    {
      for (int index = 0; index < this.m_activeEffects.Count; ++index)
      {
        if (this.m_activeEffects[index].Effect.IsStopped)
        {
          this.m_activeEffects.RemoveAt(index);
          --index;
        }
      }
    }

    public void UpdateEffectPositions()
    {
      for (int index = 0; index < this.m_activeEffects.Count; ++index)
      {
        if (!this.m_activeEffects[index].Effect.IsStopped)
          this.m_activeEffects[index].Effect.WorldMatrix = MatrixD.Multiply((MatrixD) ref this.m_activeEffects[index].LocalMatrix, this.WorldMatrix);
      }
    }

    public void RemoveOldEffects(MyWeaponDefinition.WeaponEffectAction action = MyWeaponDefinition.WeaponEffectAction.Shoot)
    {
      for (int index = 0; index < this.m_activeEffects.Count; ++index)
      {
        if (this.m_activeEffects[index].Action == action)
        {
          this.m_activeEffects[index].Effect.Stop(this.m_activeEffects[index].InstantStop);
          this.m_activeEffects.RemoveAt(index);
          --index;
        }
      }
    }

    public MyInventoryConstraint CreateAmmoInventoryConstraints(
      string displayName)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat(MyTexts.GetString(MySpaceTexts.ToolTipItemFilter_AmmoMagazineInput), (object) displayName);
      MyInventoryConstraint inventoryConstraint = new MyInventoryConstraint(stringBuilder.ToString());
      foreach (MyDefinitionId id in this.m_weaponProperties.WeaponDefinition.AmmoMagazinesId)
        inventoryConstraint.Add(id);
      return inventoryConstraint;
    }

    public bool IsAmmoMagazineCompatible(MyDefinitionId ammoMagazineId) => this.WeaponProperties.CanChangeAmmoMagazine(ammoMagazineId);

    public override bool CanSwitchAmmoMagazine() => this.m_weaponProperties != null && this.m_weaponProperties.WeaponDefinition.HasAmmoMagazines();

    public bool SwitchAmmoMagazine(MyDefinitionId ammoMagazineId)
    {
      this.m_remainingAmmos[this.CurrentAmmoMagazineId] = this.CurrentAmmo;
      this.WeaponProperties.ChangeAmmoMagazine(ammoMagazineId);
      int num = 0;
      this.m_remainingAmmos.TryGetValue(ammoMagazineId, out num);
      this.CurrentAmmo = num;
      this.RefreshAmmunitionAmount();
      return ammoMagazineId == this.WeaponProperties.AmmoMagazineId;
    }

    public override bool SwitchAmmoMagazineToNextAvailable()
    {
      MyWeaponDefinition weaponDefinition = this.WeaponProperties.WeaponDefinition;
      if (!weaponDefinition.HasAmmoMagazines())
        return false;
      int magazineIdArrayIndex = weaponDefinition.GetAmmoMagazineIdArrayIndex(this.CurrentAmmoMagazineId);
      int length = weaponDefinition.AmmoMagazinesId.Length;
      int index1 = magazineIdArrayIndex + 1;
      for (int index2 = 0; index2 != length; ++index2)
      {
        if (index1 == length)
          index1 = 0;
        if (weaponDefinition.AmmoMagazinesId[index1].SubtypeId != this.CurrentAmmoMagazineId.SubtypeId)
        {
          if (MySession.Static.InfiniteAmmo)
            return this.SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[index1]);
          int num = 0;
          if (this.m_remainingAmmos.TryGetValue(weaponDefinition.AmmoMagazinesId[index1], out num) && num > 0 || this.m_user.AmmoInventory.GetItemAmount(weaponDefinition.AmmoMagazinesId[index1], MyItemFlags.None, false) > (MyFixedPoint) 0)
            return this.SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[index1]);
        }
        ++index1;
      }
      return false;
    }

    public override bool SwitchToNextAmmoMagazine()
    {
      MyWeaponDefinition weaponDefinition = this.WeaponProperties.WeaponDefinition;
      int magazineIdArrayIndex = weaponDefinition.GetAmmoMagazineIdArrayIndex(this.CurrentAmmoMagazineId);
      int length = weaponDefinition.AmmoMagazinesId.Length;
      int index = magazineIdArrayIndex + 1;
      if (index == length)
        index = 0;
      return this.SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[index]);
    }

    public bool SwitchAmmoMagazineToFirstAvailable()
    {
      MyWeaponDefinition weaponDefinition = this.WeaponProperties.WeaponDefinition;
      for (int index = 0; index < this.WeaponProperties.AmmoMagazinesCount; ++index)
      {
        int num = 0;
        if (this.m_remainingAmmos.TryGetValue(weaponDefinition.AmmoMagazinesId[index], out num) && num > 0 || this.m_user.AmmoInventory.GetItemAmount(weaponDefinition.AmmoMagazinesId[index], MyItemFlags.None, false) > (MyFixedPoint) 0)
          return this.SwitchAmmoMagazine(weaponDefinition.AmmoMagazinesId[index]);
      }
      return false;
    }

    public bool HasEnoughMagazines()
    {
      if (MySession.Static.InfiniteAmmo)
        return true;
      if (!Sync.IsServer)
        return this.CurrentMagazines > 0;
      return this.m_user != null && this.m_user.AmmoInventory != null && this.m_user.AmmoInventory.GetItemAmount(this.CurrentAmmoMagazineId, MyItemFlags.None, false) > (MyFixedPoint) 0;
    }

    public bool HasEnoughAmmunition()
    {
      if (MySession.Static.InfiniteAmmo)
        return true;
      if (!Sync.IsServer)
        return this.CurrentAmmo > 0;
      if (this.CurrentAmmo >= 1)
        return true;
      if (this.m_user != null && this.m_user.AmmoInventory != null)
        return this.m_user.AmmoInventory.GetItemAmount(this.CurrentAmmoMagazineId, MyItemFlags.None, false) > (MyFixedPoint) 0;
      MyLog.Default.WriteLine(string.Format("Error: {0} should not be null!", this.m_user == null ? (object) "User" : (object) "AmmoInventory"));
      return false;
    }

    public bool IsAmmoFull() => this.CurrentAmmo == this.WeaponProperties.AmmoMagazineDefinition.Capacity;

    public void EmptyMagazine()
    {
      this.CurrentAmmo = 0;
      this.SaveAmmoCount();
    }

    public void ConsumeMagazine()
    {
      if (MySession.Static.SimplifiedSimulation)
      {
        if (!Sync.IsServer || !this.HasEnoughMagazines())
          return;
        this.CurrentAmmo = this.WeaponProperties.AmmoMagazineDefinition.Capacity;
      }
      else
      {
        if (Sync.IsServer && this.HasEnoughMagazines())
        {
          this.CurrentAmmo = this.WeaponProperties.AmmoMagazineDefinition.Capacity;
          if (!MySession.Static.InfiniteAmmo)
            this.m_user.AmmoInventory.RemoveItemsOfType((MyFixedPoint) 1, this.CurrentAmmoMagazineId, MyItemFlags.None, false);
        }
        this.SaveAmmoCount();
      }
    }

    public void ConsumeAmmo()
    {
      if (MySession.Static.SimplifiedSimulation)
        return;
      if (Sync.IsServer)
      {
        --this.CurrentAmmo;
        if (this.CurrentAmmo < 0 && this.HasEnoughAmmunition())
        {
          this.CurrentAmmo = this.WeaponProperties.AmmoMagazineDefinition.Capacity - 1;
          if (!MySession.Static.InfiniteAmmo)
            this.m_user.AmmoInventory.RemoveItemsOfType((MyFixedPoint) 1, this.CurrentAmmoMagazineId, MyItemFlags.None, false);
        }
        this.RefreshAmmunitionAmount();
      }
      this.SaveAmmoCount();
    }

    private void SaveAmmoCount()
    {
      MyInventory ammoInventory = this.m_user.AmmoInventory;
      if (ammoInventory == null)
        return;
      MyPhysicalInventoryItem? nullable1 = new MyPhysicalInventoryItem?();
      uint? inventoryItemId = this.InventoryItemId;
      MyPhysicalInventoryItem? nullable2;
      if (inventoryItemId.HasValue)
      {
        MyInventory myInventory = ammoInventory;
        inventoryItemId = this.InventoryItemId;
        int num = (int) inventoryItemId.Value;
        nullable2 = myInventory.GetItemByID((uint) num);
      }
      else
      {
        nullable2 = ammoInventory.FindUsableItem(this.m_user.PhysicalItemId);
        if (nullable2.HasValue)
          this.InventoryItemId = new uint?(nullable2.Value.ItemId);
      }
      if (!nullable2.HasValue || !(nullable2.Value.Content is MyObjectBuilder_PhysicalGunObject content) || !(content.GunEntity is IMyObjectBuilder_GunObject<MyObjectBuilder_GunBase> gunEntity))
        return;
      if (gunEntity.DeviceBase == null)
        gunEntity.InitializeDeviceBase<MyObjectBuilder_GunBase>((MyObjectBuilder_DeviceBase) this.GetObjectBuilder());
      else
        gunEntity.GetDevice<MyObjectBuilder_GunBase>().RemainingAmmo = this.CurrentAmmo;
    }

    public void StopShoot() => this.m_shotProjectiles = 0;

    public int GetTotalAmmunitionAmount() => this.CurrentAmmo + this.CurrentMagazines * this.CurrentAmmoMagazineDefinition.Capacity;

    public int GetAmmunitionAmount() => this.CurrentAmmo;

    public int GetMagazineAmount() => this.CurrentMagazines;

    public int GetInventoryAmmoMagazinesCount() => (int) this.m_user.AmmoInventory.GetItemAmount(this.CurrentAmmoMagazineId, MyItemFlags.None, false);

    public void RefreshAmmunitionAmount(bool forceUpdate = false)
    {
      if (!Sync.IsServer && !forceUpdate)
        return;
      if (MySession.Static.InfiniteAmmo)
        this.CurrentMagazines = 0;
      else if (this.m_user != null && this.m_user.AmmoInventory != null && this.m_weaponProperties.WeaponDefinition.HasAmmoMagazines())
      {
        if (!this.HasEnoughAmmunition())
          this.SwitchAmmoMagazineToFirstAvailable();
        this.CurrentMagazines = (int) this.m_user.AmmoInventory.GetItemAmount(this.CurrentAmmoMagazineId, MyItemFlags.None, false);
      }
      else
      {
        this.CurrentAmmo = 0;
        this.CurrentMagazines = 0;
      }
    }

    private MyDefinitionId GetBackwardCompatibleDefinitionId(
      MyObjectBuilderType typeId)
    {
      if (typeId == typeof (MyObjectBuilder_LargeGatlingTurret))
        return new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_WeaponDefinition), "LargeGatlingTurret");
      if (typeId == typeof (MyObjectBuilder_LargeMissileTurret))
        return new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_WeaponDefinition), "LargeMissileTurret");
      if (typeId == typeof (MyObjectBuilder_InteriorTurret))
        return new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_WeaponDefinition), "LargeInteriorTurret");
      if (typeId == typeof (MyObjectBuilder_SmallMissileLauncher) || typeId == typeof (MyObjectBuilder_SmallMissileLauncherReload))
        return new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_WeaponDefinition), "SmallMissileLauncher");
      return typeId == typeof (MyObjectBuilder_SmallGatlingGun) ? new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_WeaponDefinition), "GatlingGun") : new MyDefinitionId();
    }

    public void AddMuzzleMatrix(MyAmmoType ammoType, Matrix localMatrix)
    {
      int key = (int) ammoType;
      if (!this.m_dummiesByAmmoType.ContainsKey(key))
        this.m_dummiesByAmmoType[key] = new MyGunBase.DummyContainer();
      this.m_dummiesByAmmoType[key].Dummies.Add(MatrixD.Normalize((MatrixD) ref localMatrix));
    }

    public void LoadDummies(Dictionary<string, MyModelDummy> dummies)
    {
      this.m_dummies = dummies;
      this.m_dummiesByAmmoType.Clear();
      foreach (KeyValuePair<string, MyModelDummy> dummy in dummies)
      {
        if (dummy.Key.ToLower().Contains("muzzle_projectile"))
        {
          this.AddMuzzleMatrix(MyAmmoType.HighSpeed, dummy.Value.Matrix);
          this.m_holdingDummyMatrix = dummy.Value.Matrix;
          this.m_holdingDummyMatrix = Matrix.CreateScale(1f / dummy.Value.Matrix.Scale) * this.m_holdingDummyMatrix;
          this.m_holdingDummyMatrix = Matrix.Invert(this.m_holdingDummyMatrix);
        }
        else if (dummy.Key.ToLower().Contains("muzzle_missile"))
          this.AddMuzzleMatrix(MyAmmoType.Missile, dummy.Value.Matrix);
        else if (dummy.Key.ToLower().Contains("holding_dummy") || dummy.Key.ToLower().Contains("holdingdummy"))
        {
          this.m_holdingDummyMatrix = dummy.Value.Matrix;
          this.m_holdingDummyMatrix = Matrix.Normalize(this.m_holdingDummyMatrix);
        }
      }
    }

    public override Vector3D GetMuzzleLocalPosition()
    {
      MyGunBase.DummyContainer dummyContainer;
      return this.m_weaponProperties.AmmoDefinition == null || !this.m_dummiesByAmmoType.TryGetValue((int) this.m_weaponProperties.AmmoDefinition.AmmoType, out dummyContainer) ? Vector3D.Zero : dummyContainer.DummyToUse.Translation;
    }

    public MatrixD GetMuzzleLocalMatrix()
    {
      MyGunBase.DummyContainer dummyContainer;
      return this.m_weaponProperties.AmmoDefinition == null || !this.m_dummiesByAmmoType.TryGetValue((int) this.m_weaponProperties.AmmoDefinition.AmmoType, out dummyContainer) ? MatrixD.Identity : dummyContainer.DummyToUse;
    }

    public override Vector3D GetMuzzleWorldPosition()
    {
      MyGunBase.DummyContainer dummyContainer;
      if (this.m_weaponProperties.AmmoDefinition == null || !this.m_dummiesByAmmoType.TryGetValue((int) this.m_weaponProperties.AmmoDefinition.AmmoType, out dummyContainer))
        return this.m_worldMatrix.Translation;
      if (dummyContainer.Dirty)
      {
        dummyContainer.DummyInWorld = dummyContainer.DummyToUse * this.m_worldMatrix;
        dummyContainer.Dirty = false;
      }
      return dummyContainer.DummyInWorld.Translation;
    }

    public MatrixD GetMuzzleWorldMatrix()
    {
      MyGunBase.DummyContainer dummyContainer;
      if (this.m_weaponProperties.AmmoDefinition == null || !this.m_dummiesByAmmoType.TryGetValue((int) this.m_weaponProperties.AmmoDefinition.AmmoType, out dummyContainer))
        return this.m_worldMatrix;
      if (dummyContainer.Dirty)
      {
        dummyContainer.DummyInWorld = dummyContainer.DummyToUse * this.m_worldMatrix;
        dummyContainer.Dirty = false;
      }
      return dummyContainer.DummyInWorld;
    }

    private void MoveToNextMuzzle(MyAmmoType ammoType)
    {
      MyGunBase.DummyContainer dummyContainer;
      if (!this.m_dummiesByAmmoType.TryGetValue((int) ammoType, out dummyContainer) || dummyContainer.Dummies.Count <= 1)
        return;
      ++dummyContainer.DummyIndex;
      if (dummyContainer.DummyIndex == dummyContainer.Dummies.Count)
        dummyContainer.DummyIndex = 0;
      dummyContainer.Dirty = true;
    }

    private void RecalculateMuzzles()
    {
      foreach (MyGunBase.DummyContainer dummyContainer in this.m_dummiesByAmmoType.Values)
        dummyContainer.Dirty = true;
    }

    public void StartShootSound(MyEntity3DSoundEmitter soundEmitter, bool force2D = false)
    {
      if (this.ShootSound == null || soundEmitter == null)
        return;
      if (soundEmitter.IsPlaying)
      {
        if (soundEmitter.Loop)
          return;
        soundEmitter.PlaySound(this.ShootSound, force2D: force2D);
      }
      else
        soundEmitter.PlaySound(this.ShootSound, true, force2D: force2D);
    }

    internal void StartNoAmmoSound(MyEntity3DSoundEmitter soundEmitter)
    {
      if (this.NoAmmoSound == null || soundEmitter == null)
        return;
      soundEmitter.StopSound(true);
      soundEmitter.PlaySingleSound(this.NoAmmoSound, true);
    }

    internal void StartReloadSound(MyEntity3DSoundEmitter soundEmitter)
    {
      if (this.ReloadSound == null || soundEmitter == null)
        return;
      soundEmitter.StopSound(true);
      soundEmitter.PlaySingleSound(this.ReloadSound, true);
    }

    public class DummyContainer
    {
      public List<MatrixD> Dummies = new List<MatrixD>();
      public int DummyIndex;
      public MatrixD DummyInWorld = (MatrixD) ref Matrix.Identity;
      public bool Dirty = true;

      public MatrixD DummyToUse => this.Dummies[this.DummyIndex];
    }

    private class WeaponEffect
    {
      public string EffectName;
      public string DummyName;
      public Matrix LocalMatrix;
      public MyWeaponDefinition.WeaponEffectAction Action;
      public MyParticleEffect Effect;
      public bool InstantStop;

      public WeaponEffect(
        string effectName,
        string dummyName,
        Matrix localMatrix,
        MyWeaponDefinition.WeaponEffectAction action,
        MyParticleEffect effect,
        bool instantStop)
      {
        this.EffectName = effectName;
        this.DummyName = dummyName;
        this.Effect = effect;
        this.Action = action;
        this.LocalMatrix = localMatrix;
        this.InstantStop = instantStop;
      }
    }
  }
}
