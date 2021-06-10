// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridLandingSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Localization;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using VRage;
using VRage.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
  public class MyGridLandingSystem
  {
    private static readonly int GEAR_MODE_COUNT = MyUtils.GetMaxValueFromEnum<LandingGearMode>() + 1;
    private static readonly List<IMyLandingGear> m_gearTmpList = new List<IMyLandingGear>();
    private HashSet<IMyLandingGear>[] m_gearStates;
    private LockModeChangedHandler m_onStateChanged;
    public MyStringId HudMessage = MyStringId.NullOrEmpty;

    public MyMultipleEnabledEnum Locked
    {
      get
      {
        int totalGearCount = this.TotalGearCount;
        if (totalGearCount == 0)
          return MyMultipleEnabledEnum.NoObjects;
        if (totalGearCount == this[LandingGearMode.Locked])
          return MyMultipleEnabledEnum.AllEnabled;
        return totalGearCount == this[LandingGearMode.ReadyToLock] + this[LandingGearMode.Unlocked] ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.Mixed;
      }
    }

    public int TotalGearCount
    {
      get
      {
        int num = 0;
        for (int index = 0; index < MyGridLandingSystem.GEAR_MODE_COUNT; ++index)
          num += this.m_gearStates[index].Count;
        return num;
      }
    }

    public int this[LandingGearMode mode] => this.m_gearStates[(int) mode].Count;

    public MyGridLandingSystem()
    {
      this.m_gearStates = new HashSet<IMyLandingGear>[MyGridLandingSystem.GEAR_MODE_COUNT];
      for (int index = 0; index < MyGridLandingSystem.GEAR_MODE_COUNT; ++index)
        this.m_gearStates[index] = new HashSet<IMyLandingGear>();
      this.m_onStateChanged = new LockModeChangedHandler(this.StateChanged);
    }

    private void StateChanged(IMyLandingGear gear, LandingGearMode oldMode)
    {
      this.HudMessage = oldMode != LandingGearMode.ReadyToLock || gear.LockMode != LandingGearMode.Locked ? (oldMode != LandingGearMode.Locked || gear.LockMode != LandingGearMode.Unlocked ? MyStringId.NullOrEmpty : MySpaceTexts.NotificationLandingGearSwitchUnlocked) : MySpaceTexts.NotificationLandingGearSwitchLocked;
      this.m_gearStates[(int) oldMode].Remove(gear);
      this.m_gearStates[(int) gear.LockMode].Add(gear);
    }

    public void Switch()
    {
      if (this.Locked == MyMultipleEnabledEnum.AllEnabled || this.Locked == MyMultipleEnabledEnum.Mixed)
      {
        this.Switch(false);
      }
      else
      {
        if (this.Locked != MyMultipleEnabledEnum.AllDisabled)
          return;
        this.Switch(true);
      }
    }

    public List<IMyEntity> GetAttachedEntities()
    {
      List<IMyEntity> myEntityList = new List<IMyEntity>();
      foreach (IMyLandingGear myLandingGear in this.m_gearStates[2])
      {
        IMyEntity attachedEntity = myLandingGear.GetAttachedEntity();
        if (attachedEntity != null)
          myEntityList.Add(attachedEntity);
      }
      return myEntityList;
    }

    public void Switch(bool enabled)
    {
      int index = enabled ? 1 : 2;
      bool flag = !enabled && this.m_gearStates[2].Count > 0;
      foreach (IMyLandingGear myLandingGear in this.m_gearStates[index])
        MyGridLandingSystem.m_gearTmpList.Add(myLandingGear);
      if (enabled)
      {
        foreach (IMyLandingGear myLandingGear in this.m_gearStates[0])
          MyGridLandingSystem.m_gearTmpList.Add(myLandingGear);
      }
      foreach (IMyLandingGear gearTmp in MyGridLandingSystem.m_gearTmpList)
        gearTmp.RequestLock(enabled);
      MyGridLandingSystem.m_gearTmpList.Clear();
      if (!flag)
        return;
      foreach (HashSet<IMyLandingGear> gearState in this.m_gearStates)
      {
        foreach (IMyLandingGear myLandingGear in gearState)
        {
          if (myLandingGear.AutoLock)
            myLandingGear.ResetAutolock();
        }
      }
    }

    public void Register(IMyLandingGear gear)
    {
      gear.LockModeChanged += this.m_onStateChanged;
      this.m_gearStates[(int) gear.LockMode].Add(gear);
    }

    public void Unregister(IMyLandingGear gear)
    {
      this.m_gearStates[(int) gear.LockMode].Remove(gear);
      gear.LockModeChanged -= this.m_onStateChanged;
    }
  }
}
