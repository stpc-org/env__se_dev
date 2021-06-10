// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.MyGameLogic
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity.EntityComponents.Interfaces;
using VRage.ModAPI;

namespace VRage.Game.Entity
{
  public static class MyGameLogic
  {
    private static CachingList<MyGameLogicComponent> m_componentsForUpdateOnce = new CachingList<MyGameLogicComponent>();
    private static MyDistributedUpdater<CachingList<MyGameLogicComponent>, MyGameLogicComponent> m_componentsForUpdate = new MyDistributedUpdater<CachingList<MyGameLogicComponent>, MyGameLogicComponent>(1);
    private static MyDistributedUpdater<CachingList<MyGameLogicComponent>, MyGameLogicComponent> m_componentsForUpdate10 = new MyDistributedUpdater<CachingList<MyGameLogicComponent>, MyGameLogicComponent>(10);
    private static MyDistributedUpdater<CachingList<MyGameLogicComponent>, MyGameLogicComponent> m_componentsForUpdate100 = new MyDistributedUpdater<CachingList<MyGameLogicComponent>, MyGameLogicComponent>(100);

    public static void RegisterForUpdate(MyGameLogicComponent component)
    {
      if (component.EntityUpdate)
        return;
      int needsUpdate = (int) component.NeedsUpdate;
      if ((needsUpdate & 8) != 0)
        MyGameLogic.m_componentsForUpdateOnce.Add(component);
      if ((needsUpdate & 1) != 0)
        MyGameLogic.m_componentsForUpdate.List.Add(component);
      if ((needsUpdate & 2) != 0)
        MyGameLogic.m_componentsForUpdate10.List.Add(component);
      if ((needsUpdate & 4) == 0)
        return;
      MyGameLogic.m_componentsForUpdate100.List.Add(component);
    }

    public static void UnregisterForUpdate(MyGameLogicComponent component)
    {
      int needsUpdate = (int) component.NeedsUpdate;
      if ((needsUpdate & 8) != 0)
        MyGameLogic.m_componentsForUpdateOnce.Remove(component);
      if ((needsUpdate & 1) != 0)
        MyGameLogic.m_componentsForUpdate.List.Remove(component);
      if ((needsUpdate & 2) != 0)
        MyGameLogic.m_componentsForUpdate10.List.Remove(component);
      if ((needsUpdate & 4) == 0)
        return;
      MyGameLogic.m_componentsForUpdate100.List.Remove(component);
    }

    public static void ChangeUpdate(
      MyGameLogicComponent component,
      MyEntityUpdateEnum newUpdate,
      bool immediate = false)
    {
      if (component.EntityUpdate)
        return;
      MyEntityUpdateEnum needsUpdate = component.NeedsUpdate;
      if (needsUpdate == newUpdate)
        return;
      if ((needsUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) != MyEntityUpdateEnum.NONE)
      {
        if ((newUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) == MyEntityUpdateEnum.NONE)
        {
          if (immediate)
            MyGameLogic.m_componentsForUpdateOnce.ApplyChanges();
          MyGameLogic.m_componentsForUpdateOnce.Remove(component, immediate);
        }
      }
      else if ((newUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) != MyEntityUpdateEnum.NONE)
        MyGameLogic.m_componentsForUpdateOnce.Add(component);
      if ((needsUpdate & MyEntityUpdateEnum.EACH_FRAME) != MyEntityUpdateEnum.NONE)
      {
        if ((newUpdate & MyEntityUpdateEnum.EACH_FRAME) == MyEntityUpdateEnum.NONE)
        {
          if (immediate)
            MyGameLogic.m_componentsForUpdate.List.ApplyChanges();
          MyGameLogic.m_componentsForUpdate.List.Remove(component, immediate);
        }
      }
      else if ((newUpdate & MyEntityUpdateEnum.EACH_FRAME) != MyEntityUpdateEnum.NONE)
        MyGameLogic.m_componentsForUpdate.List.Add(component);
      if ((needsUpdate & MyEntityUpdateEnum.EACH_10TH_FRAME) != MyEntityUpdateEnum.NONE)
      {
        if ((newUpdate & MyEntityUpdateEnum.EACH_10TH_FRAME) == MyEntityUpdateEnum.NONE)
        {
          if (immediate)
            MyGameLogic.m_componentsForUpdate10.List.ApplyChanges();
          MyGameLogic.m_componentsForUpdate10.List.Remove(component, immediate);
        }
      }
      else if ((newUpdate & MyEntityUpdateEnum.EACH_10TH_FRAME) != MyEntityUpdateEnum.NONE)
        MyGameLogic.m_componentsForUpdate10.List.Add(component);
      if ((needsUpdate & MyEntityUpdateEnum.EACH_100TH_FRAME) != MyEntityUpdateEnum.NONE)
      {
        if ((newUpdate & MyEntityUpdateEnum.EACH_100TH_FRAME) != MyEntityUpdateEnum.NONE)
          return;
        if (immediate)
          MyGameLogic.m_componentsForUpdate100.List.ApplyChanges();
        MyGameLogic.m_componentsForUpdate100.List.Remove(component, immediate);
      }
      else
      {
        if ((newUpdate & MyEntityUpdateEnum.EACH_100TH_FRAME) == MyEntityUpdateEnum.NONE)
          return;
        MyGameLogic.m_componentsForUpdate100.List.Add(component);
      }
    }

    public static void UpdateOnceBeforeFrame()
    {
      MyGameLogic.m_componentsForUpdateOnce.ApplyChanges();
      foreach (MyGameLogicComponent gameLogicComponent in MyGameLogic.m_componentsForUpdateOnce)
      {
        gameLogicComponent.NeedsUpdate &= ~MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        if (!gameLogicComponent.MarkedForClose && !gameLogicComponent.Closed)
          ((IMyGameLogicComponent) gameLogicComponent).UpdateOnceBeforeFrame(false);
      }
    }

    public static void UpdateBeforeSimulation()
    {
      MyGameLogic.UpdateOnceBeforeFrame();
      MyGameLogic.m_componentsForUpdate.List.ApplyChanges();
      MyGameLogic.m_componentsForUpdate.Update();
      MyGameLogic.m_componentsForUpdate.Iterate((Action<MyGameLogicComponent>) (c =>
      {
        if (c.MarkedForClose || c.Closed)
          return;
        ((IMyGameLogicComponent) c).UpdateBeforeSimulation(false);
      }));
      MyGameLogic.m_componentsForUpdate10.List.ApplyChanges();
      MyGameLogic.m_componentsForUpdate10.Update();
      MyGameLogic.m_componentsForUpdate10.Iterate((Action<MyGameLogicComponent>) (c =>
      {
        if (c.MarkedForClose || c.Closed)
          return;
        ((IMyGameLogicComponent) c).UpdateBeforeSimulation10(false);
      }));
      MyGameLogic.m_componentsForUpdate100.List.ApplyChanges();
      MyGameLogic.m_componentsForUpdate100.Update();
      MyGameLogic.m_componentsForUpdate100.Iterate((Action<MyGameLogicComponent>) (c =>
      {
        if (c.MarkedForClose || c.Closed)
          return;
        ((IMyGameLogicComponent) c).UpdateBeforeSimulation100(false);
      }));
    }

    public static void UpdateAfterSimulation()
    {
      MyGameLogic.m_componentsForUpdate.List.ApplyChanges();
      MyGameLogic.m_componentsForUpdate.Iterate((Action<MyGameLogicComponent>) (c =>
      {
        if (c.MarkedForClose || c.Closed)
          return;
        ((IMyGameLogicComponent) c).UpdateAfterSimulation(false);
      }));
      MyGameLogic.m_componentsForUpdate10.List.ApplyChanges();
      MyGameLogic.m_componentsForUpdate10.Iterate((Action<MyGameLogicComponent>) (c =>
      {
        if (c.MarkedForClose || c.Closed)
          return;
        ((IMyGameLogicComponent) c).UpdateAfterSimulation10(false);
      }));
      MyGameLogic.m_componentsForUpdate100.List.ApplyChanges();
      MyGameLogic.m_componentsForUpdate100.Iterate((Action<MyGameLogicComponent>) (c =>
      {
        if (c.MarkedForClose || c.Closed)
          return;
        ((IMyGameLogicComponent) c).UpdateAfterSimulation100(false);
      }));
    }

    public static void UpdatingStopped()
    {
      foreach (MyGameLogicComponent gameLogicComponent in MyGameLogic.m_componentsForUpdate.List)
      {
        if (!gameLogicComponent.MarkedForClose && !gameLogicComponent.Closed)
          gameLogicComponent.UpdatingStopped();
      }
    }

    public static void UnloadData()
    {
      MyGameLogic.m_componentsForUpdateOnce.ClearImmediate();
      MyGameLogic.m_componentsForUpdate.List.ClearImmediate();
      MyGameLogic.m_componentsForUpdate10.List.ClearImmediate();
      MyGameLogic.m_componentsForUpdate100.List.ClearImmediate();
    }
  }
}
