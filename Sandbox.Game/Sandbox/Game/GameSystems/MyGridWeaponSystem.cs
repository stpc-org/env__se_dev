// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridWeaponSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;

namespace Sandbox.Game.GameSystems
{
  public class MyGridWeaponSystem
  {
    private Dictionary<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>> m_gunsByDefId;

    public event Action<MyGridWeaponSystem, MyGridWeaponSystem.EventArgs> WeaponRegistered;

    public event Action<MyGridWeaponSystem, MyGridWeaponSystem.EventArgs> WeaponUnregistered;

    public MyGridWeaponSystem() => this.m_gunsByDefId = new Dictionary<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>>(5, (IEqualityComparer<MyDefinitionId>) MyDefinitionId.Comparer);

    public IMyGunObject<MyDeviceBase> GetGun(MyDefinitionId defId) => this.m_gunsByDefId.ContainsKey(defId) ? this.m_gunsByDefId[defId].FirstOrDefault<IMyGunObject<MyDeviceBase>>() : (IMyGunObject<MyDeviceBase>) null;

    public Dictionary<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>> GetGunSets() => this.m_gunsByDefId;

    public bool HasGunsOfId(MyDefinitionId defId) => this.m_gunsByDefId.ContainsKey(defId) && this.m_gunsByDefId[defId].Count > 0;

    internal void Register(IMyGunObject<MyDeviceBase> gun)
    {
      if (!this.m_gunsByDefId.ContainsKey(gun.DefinitionId))
        this.m_gunsByDefId.Add(gun.DefinitionId, new HashSet<IMyGunObject<MyDeviceBase>>());
      this.m_gunsByDefId[gun.DefinitionId].Add(gun);
      if (this.WeaponRegistered == null)
        return;
      this.WeaponRegistered(this, new MyGridWeaponSystem.EventArgs()
      {
        Weapon = gun
      });
    }

    internal void Unregister(IMyGunObject<MyDeviceBase> gun)
    {
      if (!this.m_gunsByDefId.ContainsKey(gun.DefinitionId))
      {
        MyDebug.FailRelease("definition ID " + (object) gun.DefinitionId + " not in m_gunsByDefId", "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\GameSystems\\MyGridWeaponSystem.cs", 81);
      }
      else
      {
        this.m_gunsByDefId[gun.DefinitionId].Remove(gun);
        if (this.WeaponUnregistered == null)
          return;
        this.WeaponUnregistered(this, new MyGridWeaponSystem.EventArgs()
        {
          Weapon = gun
        });
      }
    }

    public HashSet<IMyGunObject<MyDeviceBase>> GetGunsById(
      MyDefinitionId gunId)
    {
      return this.m_gunsByDefId.ContainsKey(gunId) ? this.m_gunsByDefId[gunId] : (HashSet<IMyGunObject<MyDeviceBase>>) null;
    }

    internal IMyGunObject<MyDeviceBase> GetGunWithAmmo(
      MyDefinitionId gunId,
      long shooter)
    {
      if (!this.m_gunsByDefId.ContainsKey(gunId))
        return (IMyGunObject<MyDeviceBase>) null;
      IMyGunObject<MyDeviceBase> myGunObject1 = this.m_gunsByDefId[gunId].FirstOrDefault<IMyGunObject<MyDeviceBase>>();
      foreach (IMyGunObject<MyDeviceBase> myGunObject2 in this.m_gunsByDefId[gunId])
      {
        if (myGunObject2.CanShoot(MyShootActionEnum.PrimaryAction, shooter, out MyGunStatusEnum _))
        {
          myGunObject1 = myGunObject2;
          break;
        }
      }
      return myGunObject1;
    }

    public struct EventArgs
    {
      public IMyGunObject<MyDeviceBase> Weapon;
    }
  }
}
