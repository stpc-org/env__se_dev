// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.Systems.MyTimerComponentSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;

namespace Sandbox.Game.EntityComponents.Systems
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MyTimerComponentSystem : MySessionComponentBase
  {
    private object m_lockObject = new object();
    private const int UPDATE_FRAME_100 = 100;
    private const int UPDATE_FRAME_10 = 10;
    public static MyTimerComponentSystem Static;
    private List<MyTimerComponent> m_timerComponents10 = new List<MyTimerComponent>();
    private List<MyTimerComponent> m_timerComponents100 = new List<MyTimerComponent>();
    private int m_frameCounter10;
    private int m_frameCounter100;

    public override void LoadData()
    {
      base.LoadData();
      MyTimerComponentSystem.Static = this;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyTimerComponentSystem.Static = (MyTimerComponentSystem) null;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      if (++this.m_frameCounter10 % 10 == 0)
      {
        this.m_frameCounter10 = 0;
        this.UpdateTimerComponents10();
      }
      if (++this.m_frameCounter100 % 100 != 0)
        return;
      this.m_frameCounter100 = 0;
      this.UpdateTimerComponents100();
    }

    private void UpdateTimerComponents10()
    {
      lock (this.m_lockObject)
      {
        foreach (MyTimerComponent myTimerComponent in this.m_timerComponents10)
        {
          if (myTimerComponent != null && myTimerComponent.IsSessionUpdateEnabled)
            myTimerComponent.Update();
        }
      }
    }

    internal bool IsRegisteredAny(MyTimerComponent timer) => timer.IsSessionUpdateEnabled && (this.m_timerComponents10.Contains(timer) || this.m_timerComponents100.Contains(timer));

    private void UpdateTimerComponents100()
    {
      lock (this.m_lockObject)
      {
        foreach (MyTimerComponent myTimerComponent in this.m_timerComponents100)
        {
          if (myTimerComponent != null && myTimerComponent.IsSessionUpdateEnabled)
            myTimerComponent.Update();
        }
      }
    }

    public void Register(MyTimerComponent timerComponent)
    {
      if (!timerComponent.IsSessionUpdateEnabled)
        return;
      lock (this.m_lockObject)
      {
        switch (timerComponent.TimerType)
        {
          case MyTimerTypes.Frame10:
            this.m_timerComponents10.Add(timerComponent);
            break;
          case MyTimerTypes.Frame100:
            this.m_timerComponents100.Add(timerComponent);
            break;
        }
      }
    }

    public void Unregister(MyTimerComponent timerComponent)
    {
      lock (this.m_lockObject)
      {
        switch (timerComponent.TimerType)
        {
          case MyTimerTypes.Frame10:
            this.m_timerComponents10.Remove(timerComponent);
            break;
          case MyTimerTypes.Frame100:
            this.m_timerComponents100.Remove(timerComponent);
            break;
        }
      }
    }
  }
}
