// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Weapons.Guns.MyLargeMissileTurret
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons;
using Sandbox.Game.Weapons.Guns.Barrels;
using Sandbox.Game.World;
using SpaceEngineers.Game.Weapons.Guns.Barrels;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Network;
using VRageMath;

namespace SpaceEngineers.Game.Weapons.Guns
{
  [MyCubeBlockType(typeof (MyObjectBuilder_LargeMissileTurret))]
  public class MyLargeMissileTurret : MyLargeConveyorTurretBase, SpaceEngineers.Game.ModAPI.IMyLargeMissileTurret, SpaceEngineers.Game.ModAPI.IMyLargeConveyorTurretBase, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeMissileTurret, IMyMissileGunObject, IMyGunObject<MyGunBase>
  {
    private static readonly string DUMMY_NAME_BASE1 = "MissileTurretBase1";
    private static readonly string DUMMY_NAME_BARRELS = "MissileTurretBarrels";

    public override IMyMissileGunObject Launcher => (IMyMissileGunObject) this;

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.m_randomStandbyChangeConst_ms = 4000;
      this.m_rotationSpeed = 0.001570796f;
      this.m_elevationSpeed = 0.001570796f;
      if (this.BlockDefinition != null)
      {
        this.m_rotationSpeed = this.BlockDefinition.RotationSpeed;
        this.m_elevationSpeed = this.BlockDefinition.ElevationSpeed;
      }
      if (this.m_gunBase.HasAmmoMagazines)
        this.m_shootingCueEnum = this.m_gunBase.ShootSound;
      this.m_rotatingCueEnum.Init("WepTurretGatlingRotate");
    }

    protected override float ForwardCameraOffset => 0.5f;

    protected override float UpCameraOffset => 1f;

    public override void OnModelChange()
    {
      base.OnModelChange();
      if (this.IsBuilt)
      {
        if (this.Subparts.ContainsKey(MyLargeMissileTurret.DUMMY_NAME_BASE1))
        {
          this.m_base1 = (MyEntity) this.Subparts[MyLargeMissileTurret.DUMMY_NAME_BASE1];
          this.m_base2 = (MyEntity) this.m_base1.Subparts[MyLargeMissileTurret.DUMMY_NAME_BARRELS];
          this.m_barrel = (MyLargeBarrelBase) new MyLargeMissileBarrel();
          this.m_barrel.Init(this.m_base2, (MyLargeTurretBase) this);
          this.GetCameraDummy();
        }
      }
      else
      {
        this.m_base1 = (MyEntity) null;
        this.m_base2 = (MyEntity) null;
        this.m_barrel = (MyLargeBarrelBase) null;
      }
      this.ResetRotation();
    }

    public override void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      if (action != MyShootActionEnum.PrimaryAction || this.m_barrel == null)
        return;
      this.m_barrel.StartShooting();
    }

    public new void MissileShootEffect()
    {
      if (this.m_barrel == null)
        return;
      this.m_barrel.ShootEffect();
    }

    public new void ShootMissile(MyObjectBuilder_Missile builder) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeMissileTurret, MyObjectBuilder_Missile>(this, (Func<MyLargeMissileTurret, Action<MyObjectBuilder_Missile>>) (x => new Action<MyObjectBuilder_Missile>(x.OnShootMissile)), builder);

    [Event(null, 101)]
    [Reliable]
    [Server]
    [Broadcast]
    private void OnShootMissile(MyObjectBuilder_Missile builder) => MyMissiles.Add(builder);

    public new void RemoveMissile(long entityId) => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyLargeMissileTurret, long>(this, (Func<MyLargeMissileTurret, Action<long>>) (x => new Action<long>(x.OnRemoveMissile)), entityId);

    [Event(null, 114)]
    [Reliable]
    [Broadcast]
    private void OnRemoveMissile(long entityId) => MyMissiles.Remove(entityId);

    public override void UpdateAfterSimulationParallel()
    {
      if (!MySession.Static.WeaponsEnabled)
      {
        this.RotateModels();
      }
      else
      {
        base.UpdateAfterSimulationParallel();
        this.DrawLasers();
      }
    }

    public override void ShootFromTerminal(Vector3 direction)
    {
      if (this.m_barrel == null)
        return;
      base.ShootFromTerminal(direction);
      this.m_isControlled = true;
      this.m_barrel.StartShooting();
      this.m_isControlled = false;
    }

    protected new sealed class OnShootMissile\u003C\u003ESandbox_Common_ObjectBuilders_MyObjectBuilder_Missile : ICallSite<MyLargeMissileTurret, MyObjectBuilder_Missile, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeMissileTurret @this,
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

    protected new sealed class OnRemoveMissile\u003C\u003ESystem_Int64 : ICallSite<MyLargeMissileTurret, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLargeMissileTurret @this,
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
  }
}
