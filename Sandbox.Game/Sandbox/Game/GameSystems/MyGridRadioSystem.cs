// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridRadioSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Sync;

namespace Sandbox.Game.GameSystems
{
  public class MyGridRadioSystem
  {
    private HashSet<MyDataBroadcaster> m_broadcasters;
    private HashSet<MyDataReceiver> m_receivers;
    private HashSet<MyRadioBroadcaster> m_radioBroadcasters;
    private HashSet<MyRadioReceiver> m_radioReceivers;
    private HashSet<MyLaserBroadcaster> m_laserBroadcasters;
    private HashSet<MyLaserReceiver> m_laserReceivers;
    public bool IsClosing;
    private MyMultipleEnabledEnum m_antennasBroadcasterEnabled;
    private bool m_antennasBroadcasterEnabledNeedsRefresh;
    private MyCubeGrid m_grid;

    public MyMultipleEnabledEnum AntennasBroadcasterEnabled
    {
      get
      {
        if (this.m_antennasBroadcasterEnabledNeedsRefresh)
          this.RefreshAntennasBroadcasterEnabled();
        return this.m_antennasBroadcasterEnabled;
      }
      set
      {
        if (this.m_antennasBroadcasterEnabled == value || this.m_antennasBroadcasterEnabled == MyMultipleEnabledEnum.NoObjects || this.IsClosing)
          return;
        this.BroadcasterStateChanged(value);
      }
    }

    public HashSet<MyDataBroadcaster> Broadcasters => this.m_broadcasters;

    public HashSet<MyDataReceiver> Receivers => this.m_receivers;

    public HashSet<MyLaserBroadcaster> LaserBroadcasters => this.m_laserBroadcasters;

    public HashSet<MyLaserReceiver> LaserReceivers => this.m_laserReceivers;

    public MyGridRadioSystem(MyCubeGrid grid)
    {
      this.m_broadcasters = new HashSet<MyDataBroadcaster>();
      this.m_receivers = new HashSet<MyDataReceiver>();
      this.m_radioBroadcasters = new HashSet<MyRadioBroadcaster>();
      this.m_radioReceivers = new HashSet<MyRadioReceiver>();
      this.m_laserBroadcasters = new HashSet<MyLaserBroadcaster>();
      this.m_laserReceivers = new HashSet<MyLaserReceiver>();
      this.m_antennasBroadcasterEnabled = MyMultipleEnabledEnum.NoObjects;
      this.m_grid = grid;
    }

    public void BroadcasterStateChanged(MyMultipleEnabledEnum enabledState)
    {
      this.m_antennasBroadcasterEnabled = enabledState;
      bool flag = enabledState == MyMultipleEnabledEnum.AllEnabled;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        foreach (MyRadioBroadcaster radioBroadcaster in this.m_radioBroadcasters)
        {
          if (radioBroadcaster.Entity is MyRadioAntenna)
            (radioBroadcaster.Entity as MyRadioAntenna).EnableBroadcasting.Value = flag;
        }
      }
      this.m_antennasBroadcasterEnabledNeedsRefresh = false;
    }

    public void Register(MyDataBroadcaster broadcaster)
    {
      this.m_broadcasters.Add(broadcaster);
      switch (broadcaster)
      {
        case MyLaserBroadcaster laserBroadcaster:
          this.m_laserBroadcasters.Add(laserBroadcaster);
          break;
        case MyRadioBroadcaster radioBroadcaster:
          if (!(broadcaster.Entity is MyRadioAntenna))
            break;
          this.m_radioBroadcasters.Add(radioBroadcaster);
          (broadcaster.Entity as MyRadioAntenna).EnableBroadcasting.ValueChanged += (Action<SyncBase>) (obj => this.broadcaster_EnabledChanged());
          if (this.m_radioBroadcasters.Count == 1)
          {
            this.m_antennasBroadcasterEnabled = radioBroadcaster.Enabled ? MyMultipleEnabledEnum.AllEnabled : MyMultipleEnabledEnum.AllDisabled;
            break;
          }
          if ((this.AntennasBroadcasterEnabled != MyMultipleEnabledEnum.AllEnabled || radioBroadcaster.Enabled) && (this.AntennasBroadcasterEnabled != MyMultipleEnabledEnum.AllDisabled || !radioBroadcaster.Enabled))
            break;
          this.m_antennasBroadcasterEnabled = MyMultipleEnabledEnum.Mixed;
          break;
      }
    }

    public void Register(MyDataReceiver reciever)
    {
      this.m_receivers.Add(reciever);
      switch (reciever)
      {
        case MyLaserReceiver myLaserReceiver:
          this.m_laserReceivers.Add(myLaserReceiver);
          break;
        case MyRadioReceiver myRadioReceiver:
          this.m_radioReceivers.Add(myRadioReceiver);
          break;
      }
    }

    public void Unregister(MyDataBroadcaster broadcaster)
    {
      this.m_broadcasters.Remove(broadcaster);
      switch (broadcaster)
      {
        case MyLaserBroadcaster laserBroadcaster:
          this.m_laserBroadcasters.Remove(laserBroadcaster);
          break;
        case MyRadioBroadcaster radioBroadcaster:
          if (!(broadcaster.Entity is MyRadioAntenna))
            break;
          this.m_radioBroadcasters.Remove(radioBroadcaster);
          (broadcaster.Entity as MyRadioAntenna).EnableBroadcasting.ValueChanged -= (Action<SyncBase>) (obj => this.broadcaster_EnabledChanged());
          if (this.m_radioBroadcasters.Count == 0)
          {
            this.m_antennasBroadcasterEnabled = MyMultipleEnabledEnum.NoObjects;
            break;
          }
          if (this.m_radioBroadcasters.Count == 1)
          {
            this.AntennasBroadcasterEnabled = this.m_radioBroadcasters.First<MyRadioBroadcaster>().Enabled ? MyMultipleEnabledEnum.AllEnabled : MyMultipleEnabledEnum.AllDisabled;
            break;
          }
          if (this.AntennasBroadcasterEnabled != MyMultipleEnabledEnum.Mixed)
            break;
          this.m_antennasBroadcasterEnabledNeedsRefresh = true;
          break;
      }
    }

    public void Unregister(MyDataReceiver reciever)
    {
      this.m_receivers.Remove(reciever);
      switch (reciever)
      {
        case MyLaserReceiver myLaserReceiver:
          this.m_laserReceivers.Remove(myLaserReceiver);
          break;
        case MyRadioReceiver myRadioReceiver:
          this.m_radioReceivers.Remove(myRadioReceiver);
          break;
      }
    }

    private void RefreshAntennasBroadcasterEnabled()
    {
      this.m_antennasBroadcasterEnabledNeedsRefresh = false;
      bool flag1 = true;
      bool flag2 = true;
      foreach (MyRadioBroadcaster radioBroadcaster in this.m_radioBroadcasters)
      {
        if (radioBroadcaster.Entity is MyRadioAntenna)
        {
          flag1 = flag1 && radioBroadcaster.Enabled;
          flag2 = flag2 && !radioBroadcaster.Enabled;
          if (!flag1 && !flag2)
          {
            this.m_antennasBroadcasterEnabled = MyMultipleEnabledEnum.Mixed;
            return;
          }
        }
      }
      this.AntennasBroadcasterEnabled = flag1 ? MyMultipleEnabledEnum.AllEnabled : MyMultipleEnabledEnum.AllDisabled;
    }

    private void broadcaster_EnabledChanged() => this.m_antennasBroadcasterEnabledNeedsRefresh = true;

    public void UpdateRemoteControlInfo()
    {
      foreach (MyDataBroadcaster broadcaster in this.m_broadcasters)
        broadcaster.UpdateRemoteControlInfo();
    }
  }
}
