// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridReflectorLightSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage;

namespace Sandbox.Game.GameSystems
{
  public class MyGridReflectorLightSystem
  {
    private HashSet<MyReflectorLight> m_reflectors;
    public bool IsClosing;
    private MyMultipleEnabledEnum m_reflectorsEnabled;
    private bool m_reflectorsEnabledNeedsRefresh;
    private MyCubeGrid m_grid;

    public int ReflectorCount => this.m_reflectors.Count;

    public MyMultipleEnabledEnum ReflectorsEnabled
    {
      get
      {
        if (this.m_reflectorsEnabledNeedsRefresh)
          this.RefreshReflectorsEnabled();
        return this.m_reflectorsEnabled;
      }
      set
      {
        if (this.m_reflectorsEnabled == value || this.m_reflectorsEnabled == MyMultipleEnabledEnum.NoObjects || this.IsClosing)
          return;
        this.m_grid.SendReflectorState(value);
      }
    }

    public MyGridReflectorLightSystem(MyCubeGrid grid)
    {
      this.m_reflectors = new HashSet<MyReflectorLight>();
      this.m_reflectorsEnabled = MyMultipleEnabledEnum.NoObjects;
      this.m_grid = grid;
    }

    public void ReflectorStateChanged(MyMultipleEnabledEnum enabledState)
    {
      this.m_reflectorsEnabled = enabledState;
      if (!Sync.IsServer)
        return;
      bool flag = enabledState == MyMultipleEnabledEnum.AllEnabled;
      foreach (MyReflectorLight reflector in this.m_reflectors)
      {
        reflector.EnabledChanged -= new Action<MyTerminalBlock>(this.reflector_EnabledChanged);
        reflector.Enabled = flag;
        reflector.EnabledChanged += new Action<MyTerminalBlock>(this.reflector_EnabledChanged);
      }
      this.m_reflectorsEnabledNeedsRefresh = false;
    }

    public void Register(MyReflectorLight reflector)
    {
      this.m_reflectors.Add(reflector);
      reflector.EnabledChanged += new Action<MyTerminalBlock>(this.reflector_EnabledChanged);
      if (this.m_reflectors.Count == 1)
      {
        this.m_reflectorsEnabled = reflector.Enabled ? MyMultipleEnabledEnum.AllEnabled : MyMultipleEnabledEnum.AllDisabled;
      }
      else
      {
        if ((this.ReflectorsEnabled != MyMultipleEnabledEnum.AllEnabled || reflector.Enabled) && (this.ReflectorsEnabled != MyMultipleEnabledEnum.AllDisabled || !reflector.Enabled))
          return;
        this.m_reflectorsEnabled = MyMultipleEnabledEnum.Mixed;
      }
    }

    public void Unregister(MyReflectorLight reflector)
    {
      this.m_reflectors.Remove(reflector);
      reflector.EnabledChanged -= new Action<MyTerminalBlock>(this.reflector_EnabledChanged);
      if (this.m_reflectors.Count == 0)
        this.m_reflectorsEnabled = MyMultipleEnabledEnum.NoObjects;
      else if (this.m_reflectors.Count == 1)
      {
        this.m_reflectorsEnabled = this.m_reflectors.First<MyReflectorLight>().Enabled ? MyMultipleEnabledEnum.AllEnabled : MyMultipleEnabledEnum.AllDisabled;
      }
      else
      {
        if (this.ReflectorsEnabled != MyMultipleEnabledEnum.Mixed)
          return;
        this.m_reflectorsEnabledNeedsRefresh = true;
      }
    }

    private void RefreshReflectorsEnabled()
    {
      this.m_reflectorsEnabledNeedsRefresh = false;
      if (!Sync.IsServer)
        return;
      bool flag1 = true;
      bool flag2 = true;
      foreach (MyReflectorLight reflector in this.m_reflectors)
      {
        flag1 = flag1 && reflector.Enabled;
        flag2 = flag2 && !reflector.Enabled;
        if (!flag1 && !flag2)
        {
          this.m_reflectorsEnabled = MyMultipleEnabledEnum.Mixed;
          return;
        }
      }
      this.ReflectorsEnabled = flag1 ? MyMultipleEnabledEnum.AllEnabled : MyMultipleEnabledEnum.AllDisabled;
    }

    private void reflector_EnabledChanged(MyTerminalBlock obj) => this.m_reflectorsEnabledNeedsRefresh = true;
  }
}
