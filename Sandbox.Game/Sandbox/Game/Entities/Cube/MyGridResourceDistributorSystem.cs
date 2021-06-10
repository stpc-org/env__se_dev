// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyGridResourceDistributorSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.EntityComponents;
using System;
using VRage.Game;

namespace Sandbox.Game.Entities.Cube
{
  public class MyGridResourceDistributorSystem : MyResourceDistributorComponent
  {
    private readonly MyGridLogicalGroupData m_gridLogicalGroupData;
    private bool m_scheduled;

    public MyGridResourceDistributorSystem(
      string debugName,
      MyGridLogicalGroupData gridLogicalGroupData)
      : base(debugName)
    {
      this.m_gridLogicalGroupData = gridLogicalGroupData;
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      if (!this.m_scheduled)
        return;
      bool flag = false;
      foreach (MyDefinitionId initializedType in this.m_initializedTypes)
      {
        MyResourceStateEnum state = this.ResourceStateByType(initializedType);
        flag |= this.PowerStateWorks(state);
      }
      if (flag)
        return;
      this.DeSchedule();
    }

    public void Schedule(MyCubeGrid root = null)
    {
      if (this.m_scheduled)
        return;
      root = root ?? this.m_gridLogicalGroupData.Root;
      if (root == null)
        return;
      root.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(((MyResourceDistributorComponent) this).UpdateBeforeSimulation), 14, true);
      this.m_scheduled = true;
    }

    protected void DeSchedule(MyCubeGrid root = null)
    {
      root = root ?? this.m_gridLogicalGroupData.Root;
      root.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(((MyResourceDistributorComponent) this).UpdateBeforeSimulation));
      this.m_scheduled = false;
    }

    public void OnRootChanged(MyCubeGrid oldRoot, MyCubeGrid newRoot)
    {
      if (!this.m_scheduled)
        return;
      this.DeSchedule(oldRoot);
      if (newRoot == null)
        return;
      this.Schedule(newRoot);
    }

    private class Sandbox_Game_Entities_Cube_MyGridResourceDistributorSystem\u003C\u003EActor
    {
    }
  }
}
