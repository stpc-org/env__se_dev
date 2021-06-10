// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MySmallMissileLauncherReload
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using System;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  [MyCubeBlockType(typeof (MyObjectBuilder_SmallMissileLauncherReload))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMySmallMissileLauncherReload), typeof (Sandbox.ModAPI.Ingame.IMySmallMissileLauncherReload)})]
  public class MySmallMissileLauncherReload : MySmallMissileLauncher, Sandbox.ModAPI.IMySmallMissileLauncherReload, Sandbox.ModAPI.IMySmallMissileLauncher, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMySmallMissileLauncher, Sandbox.ModAPI.Ingame.IMySmallMissileLauncherReload
  {
    private const int COOLDOWN_TIME_MILISECONDS = 5000;
    private int m_numRocketsShot;
    private static readonly MyHudNotification MISSILE_RELOAD_NOTIFICATION = new MyHudNotification(MySpaceTexts.MissileLauncherReloadingNotification, 5000, level: MyNotificationLevel.Important);

    public int BurstFireRate => this.GunBase.ShotsInBurst;

    public MySmallMissileLauncherReload() => this.CreateTerminalControls();

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MySmallMissileLauncherReload>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MySmallMissileLauncherReload> onOff = new MyTerminalControlOnOffSwitch<MySmallMissileLauncherReload>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem);
      onOff.Getter = (MyTerminalValueControl<MySmallMissileLauncherReload, bool>.GetterDelegate) (x => x.UseConveyorSystem);
      onOff.Setter = (MyTerminalValueControl<MySmallMissileLauncherReload, bool>.SetterDelegate) ((x, v) => x.UseConveyorSystem = v);
      onOff.Visible = (Func<MySmallMissileLauncherReload, bool>) (x => true);
      onOff.EnableToggleAction<MySmallMissileLauncherReload>();
      MyTerminalControlFactory.AddControl<MySmallMissileLauncherReload>((MyTerminalControl<MySmallMissileLauncherReload>) onOff);
    }

    public override void Shoot(
      MyShootActionEnum action,
      Vector3 direction,
      Vector3D? overrideWeaponPos,
      string gunAction)
    {
      if (this.BurstFireRate == this.m_numRocketsShot && MySandboxGame.TotalGamePlayTimeInMilliseconds < this.m_nextShootTime)
        return;
      if (this.BurstFireRate == this.m_numRocketsShot)
        this.m_numRocketsShot = 0;
      ++this.m_numRocketsShot;
      base.Shoot(action, direction, overrideWeaponPos, gunAction);
    }

    public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
    {
      base.Init(builder, cubeGrid);
      this.m_useConveyorSystem.SetLocalValue(((MyObjectBuilder_SmallMissileLauncher) builder).UseConveyorSystem);
    }

    private class Sandbox_Game_Weapons_MySmallMissileLauncherReload\u003C\u003EActor : IActivator, IActivator<MySmallMissileLauncherReload>
    {
      object IActivator.CreateInstance() => (object) new MySmallMissileLauncherReload();

      MySmallMissileLauncherReload IActivator<MySmallMissileLauncherReload>.CreateInstance() => new MySmallMissileLauncherReload();
    }
  }
}
