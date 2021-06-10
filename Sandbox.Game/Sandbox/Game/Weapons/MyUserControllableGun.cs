// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyUserControllableGun
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using System;
using System.Runtime.InteropServices;
using VRage.Game;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  [MyCubeBlockType(typeof (MyObjectBuilder_UserControllableGun))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyUserControllableGun), typeof (Sandbox.ModAPI.Ingame.IMyUserControllableGun)})]
  public abstract class MyUserControllableGun : MyFunctionalBlock, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun
  {
    protected VRage.Sync.Sync<bool, SyncDirection.FromServer> m_isShooting;
    protected static readonly MyStringId ID_RED_DOT = MyStringId.GetOrCompute("RedDot");
    protected readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_forceShoot;
    protected bool m_shootingBegun;

    public event Action<int> ReloadStarted;

    public MyUserControllableGun()
    {
      this.CreateTerminalControls();
      this.m_isShooting.ValueChanged += (Action<SyncBase>) (x => this.ShootingChanged());
      this.NeedsWorldMatrix = true;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyUserControllableGun>())
        return;
      base.CreateTerminalControls();
      if (!MyFakes.ENABLE_WEAPON_TERMINAL_CONTROL)
        return;
      MyTerminalControlButton<MyUserControllableGun> button = new MyTerminalControlButton<MyUserControllableGun>("ShootOnce", MySpaceTexts.Terminal_ShootOnce, MySpaceTexts.Blank, (Action<MyUserControllableGun>) (b => b.OnShootOncePressed()));
      button.EnableAction<MyUserControllableGun>();
      MyTerminalControlFactory.AddControl<MyUserControllableGun>((MyTerminalControl<MyUserControllableGun>) button);
      MyTerminalControlOnOffSwitch<MyUserControllableGun> onOff = new MyTerminalControlOnOffSwitch<MyUserControllableGun>("Shoot", MySpaceTexts.Terminal_Shoot);
      onOff.Getter = (MyTerminalValueControl<MyUserControllableGun, bool>.GetterDelegate) (x => (bool) x.m_forceShoot);
      onOff.Setter = (MyTerminalValueControl<MyUserControllableGun, bool>.SetterDelegate) ((x, v) => x.OnShootPressed(v));
      onOff.EnableToggleAction<MyUserControllableGun>();
      onOff.EnableOnOffActions<MyUserControllableGun>();
      MyTerminalControlFactory.AddControl<MyUserControllableGun>((MyTerminalControl<MyUserControllableGun>) onOff);
      MyTerminalControlFactory.AddControl<MyUserControllableGun>((MyTerminalControl<MyUserControllableGun>) new MyTerminalControlSeparator<MyUserControllableGun>());
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_UserControllableGun userControllableGun = objectBuilder as MyObjectBuilder_UserControllableGun;
      this.m_forceShoot.SetLocalValue(userControllableGun.IsShootingFromTerminal);
      this.m_isShooting.SetLocalValue(userControllableGun.IsShooting);
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.m_forceShoot.ValueChanged += new Action<SyncBase>(this.OnForceShootChanged);
    }

    private void OnForceShootChanged(SyncBase obj) => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_UserControllableGun builderCubeBlock = (MyObjectBuilder_UserControllableGun) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.IsShooting = (bool) this.m_isShooting;
      builderCubeBlock.IsShootingFromTerminal = (bool) this.m_forceShoot;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public virtual bool IsStationary() => false;

    public virtual Vector3D GetWeaponMuzzleWorldPosition() => this.WorldMatrix.Translation;

    private void OnShootOncePressed()
    {
      this.SyncRotationAndOrientation();
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyUserControllableGun>(this, (Func<MyUserControllableGun, Action>) (x => new Action(x.ShootOncePressedEvent)));
    }

    [Event(null, 110)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void ShootOncePressedEvent() => this.Shoot();

    public void SetShooting(bool shooting) => this.OnShootPressed(shooting);

    private void OnShootPressed(bool isShooting)
    {
      this.m_forceShoot.Value = isShooting;
      if (isShooting)
      {
        this.BeginShoot(MyShootActionEnum.PrimaryAction);
        this.SyncRotationAndOrientation();
      }
      else
        this.EndShoot(MyShootActionEnum.PrimaryAction);
    }

    private void Shoot()
    {
      MyGunStatusEnum status;
      if (!this.CanShoot(MyShootActionEnum.PrimaryAction, this.OwnerId, out status) || !this.CanShoot(out status) || !this.CanOperate())
        return;
      this.ShootFromTerminal((Vector3) this.WorldMatrix.Forward);
    }

    public virtual void BeginShoot(MyShootActionEnum action)
    {
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      this.m_shootingBegun = true;
      this.Shoot();
      this.RememberIdle();
      this.TakeControlFromTerminal();
      if (MyVisualScriptLogicProvider.WeaponBlockActivated == null)
        return;
      MyVisualScriptLogicProvider.WeaponBlockActivated(this.EntityId, this.CubeGrid.EntityId, this.Name, this.CubeGrid.Name, this.BlockDefinition.Id.TypeId.ToString(), this.BlockDefinition.Id.SubtypeId.ToString());
    }

    public virtual void EndShoot(MyShootActionEnum action)
    {
      this.m_shootingBegun = false;
      this.RestoreIdle();
      this.Render.NeedsDrawFromParent = false;
      this.StopShootFromTerminal();
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.Render.NeedsDrawFromParent = (bool) this.m_isShooting || (bool) this.m_forceShoot;
      if (!(bool) this.m_isShooting && !(bool) this.m_forceShoot)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (!this.IsWorking || !(bool) this.m_isShooting && !(bool) this.m_forceShoot || this.CubeGrid.IsPreview)
        return;
      this.TakeControlFromTerminal();
      this.Shoot();
      this.RotateModels();
    }

    public virtual void ShootFromTerminal(Vector3 direction)
    {
      if (MyVisualScriptLogicProvider.WeaponBlockActivated == null)
        return;
      MyVisualScriptLogicProvider.WeaponBlockActivated(this.EntityId, this.CubeGrid.EntityId, this.Name, this.CubeGrid.Name, this.BlockDefinition.Id.TypeId.ToString(), this.BlockDefinition.Id.SubtypeId.ToString());
    }

    public abstract void StopShootFromTerminal();

    public abstract bool CanShoot(
      MyShootActionEnum action,
      long shooter,
      out MyGunStatusEnum status);

    public virtual bool CanShoot(out MyGunStatusEnum status)
    {
      status = MyGunStatusEnum.OK;
      return true;
    }

    public abstract bool CanOperate();

    public virtual void TakeControlFromTerminal()
    {
    }

    bool Sandbox.ModAPI.Ingame.IMyUserControllableGun.IsShooting => (bool) this.m_isShooting;

    public virtual void SyncRotationAndOrientation()
    {
    }

    protected virtual void RotateModels()
    {
    }

    protected virtual void RememberIdle()
    {
    }

    protected virtual void RestoreIdle()
    {
    }

    protected void ShootingChanged()
    {
      if ((bool) this.m_isShooting)
        this.BeginShoot(MyShootActionEnum.PrimaryAction);
      else
        this.EndShoot(MyShootActionEnum.PrimaryAction);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
    }

    public override void OnRemovedByCubeBuilder()
    {
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory != null)
        this.ReleaseInventory(inventory);
      base.OnRemovedByCubeBuilder();
    }

    public override void OnDestroy()
    {
      MyInventory inventory = MyEntityExtensions.GetInventory(this);
      if (inventory != null)
        this.ReleaseInventory(inventory);
      base.OnDestroy();
    }

    public void OnReloadStarted(int reloadTime)
    {
      Action<int> reloadStarted = this.ReloadStarted;
      if (reloadStarted == null)
        return;
      reloadStarted(reloadTime);
    }

    protected sealed class ShootOncePressedEvent\u003C\u003E : ICallSite<MyUserControllableGun, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyUserControllableGun @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.ShootOncePressedEvent();
      }
    }

    protected class m_isShooting\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.FromServer>(obj1, obj2));
        ((MyUserControllableGun) obj0).m_isShooting = (VRage.Sync.Sync<bool, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_forceShoot\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyUserControllableGun) obj0).m_forceShoot = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
