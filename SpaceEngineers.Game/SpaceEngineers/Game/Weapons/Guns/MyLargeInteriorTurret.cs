// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Weapons.Guns.MyLargeInteriorTurret
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
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Weapons.Guns
{
  [MyCubeBlockType(typeof (MyObjectBuilder_InteriorTurret))]
  [MyTerminalInterface(new Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyLargeInteriorTurret), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyLargeInteriorTurret)})]
  public class MyLargeInteriorTurret : MyLargeTurretBase, SpaceEngineers.Game.ModAPI.IMyLargeInteriorTurret, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeInteriorTurret
  {
    public int Burst { get; private set; }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.m_randomStandbyChangeConst_ms = MyUtils.GetRandomInt(3500, 4500);
      if (this.m_gunBase.HasAmmoMagazines)
        this.m_shootingCueEnum = this.m_gunBase.ShootSound;
      this.m_rotatingCueEnum.Init("WepTurretInteriorRotate");
    }

    protected override float ForwardCameraOffset => 0.2f;

    protected override float UpCameraOffset => 0.45f;

    public override void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      if (action != MyShootActionEnum.PrimaryAction)
        return;
      this.m_gunBase.Shoot(Vector3.Zero);
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      if (this.IsBuilt)
      {
        MyEntitySubpart myEntitySubpart1;
        if (!this.Subparts.TryGetValue("InteriorTurretBase1", out myEntitySubpart1))
          return;
        this.m_base1 = (MyEntity) myEntitySubpart1;
        MyEntitySubpart myEntitySubpart2;
        if (!this.m_base1.Subparts.TryGetValue("InteriorTurretBase2", out myEntitySubpart2))
          return;
        this.m_base2 = (MyEntity) myEntitySubpart2;
        this.m_barrel = (MyLargeBarrelBase) new MyLargeInteriorBarrel();
        this.m_barrel.Init(this.m_base2, (MyLargeTurretBase) this);
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

    public MyLargeInteriorTurret() => this.Render = (MyRenderComponentBase) new MyRenderComponentLargeTurret();
  }
}
