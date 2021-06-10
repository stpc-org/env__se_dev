// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Weapons.Guns.MyLargeGatlingTurret
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons;
using Sandbox.Game.Weapons.Guns.Barrels;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using SpaceEngineers.Game.EntityComponents.Renders;
using SpaceEngineers.Game.Weapons.Guns.Barrels;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Weapons.Guns
{
  [MyCubeBlockType(typeof (MyObjectBuilder_LargeGatlingTurret))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyLargeGatlingTurret), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyLargeGatlingTurret)})]
  public class MyLargeGatlingTurret : MyLargeConveyorTurretBase, SpaceEngineers.Game.ModAPI.IMyLargeGatlingTurret, SpaceEngineers.Game.ModAPI.IMyLargeConveyorTurretBase, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeGatlingTurret
  {
    public int Burst { get; private set; }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.m_randomStandbyChangeConst_ms = MyUtils.GetRandomInt(3500, 4500);
      if (this.m_gunBase.HasAmmoMagazines)
        this.m_shootingCueEnum = this.m_gunBase.ShootSound;
      this.m_rotatingCueEnum.Init("WepTurretGatlingRotate");
    }

    protected override float ForwardCameraOffset => 0.5f;

    protected override float UpCameraOffset => 0.75f;

    public override void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      if (action != MyShootActionEnum.PrimaryAction)
        return;
      this.m_gunBase.Shoot(this.Parent.Physics.LinearVelocity);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      if (this.IsBuilt)
      {
        this.m_base1 = (MyEntity) this.Subparts["GatlingTurretBase1"];
        this.m_base2 = (MyEntity) this.m_base1.Subparts["GatlingTurretBase2"];
        this.m_barrel = (MyLargeBarrelBase) new MyLargeGatlingBarrel();
        this.m_barrel.Init((MyEntity) this.m_base2.Subparts["GatlingBarrel"], (MyLargeTurretBase) this);
        this.GetCameraDummy();
      }
      else
      {
        this.m_base1 = (MyEntity) null;
        this.m_base2 = (MyEntity) null;
        this.m_barrel = (MyLargeBarrelBase) null;
      }
      this.ResetRotation();
    }

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

    public MyLargeGatlingTurret() => this.Render = (MyRenderComponentBase) new MyRenderComponentLargeTurret();
  }
}
