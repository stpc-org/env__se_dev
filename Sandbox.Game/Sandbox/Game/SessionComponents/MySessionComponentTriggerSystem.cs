// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentTriggerSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.EntityComponents;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MySessionComponentTriggerSystem : MySessionComponentBase
  {
    private readonly Dictionary<MyEntity, CachingHashSet<MyTriggerComponent>> m_triggers = new Dictionary<MyEntity, CachingHashSet<MyTriggerComponent>>();
    private readonly FastResourceLock m_dictionaryLock = new FastResourceLock();
    public static MySessionComponentTriggerSystem Static;

    public override void LoadData()
    {
      base.LoadData();
      MySessionComponentTriggerSystem.Static = this;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MySessionComponentTriggerSystem.Static = (MySessionComponentTriggerSystem) null;
    }

    public MyEntity GetTriggersEntity(
      string triggerName,
      out MyTriggerComponent foundTrigger)
    {
      foundTrigger = (MyTriggerComponent) null;
      foreach (KeyValuePair<MyEntity, CachingHashSet<MyTriggerComponent>> trigger in this.m_triggers)
      {
        foreach (MyTriggerComponent triggerComponent1 in trigger.Value)
        {
          if (triggerComponent1 is MyAreaTriggerComponent triggerComponent && triggerComponent.Name == triggerName)
          {
            foundTrigger = triggerComponent1;
            return trigger.Key;
          }
        }
      }
      return (MyEntity) null;
    }

    public void AddTrigger(MyTriggerComponent trigger)
    {
      if (this.Contains(trigger))
        return;
      using (this.m_dictionaryLock.AcquireExclusiveUsing())
      {
        CachingHashSet<MyTriggerComponent> cachingHashSet;
        if (this.m_triggers.TryGetValue((MyEntity) trigger.Entity, out cachingHashSet))
          cachingHashSet.Add(trigger);
        else
          this.m_triggers[(MyEntity) trigger.Entity] = new CachingHashSet<MyTriggerComponent>()
          {
            trigger
          };
      }
    }

    public static void RemoveTrigger(MyEntity entity, MyTriggerComponent trigger)
    {
      if (MySessionComponentTriggerSystem.Static == null)
        return;
      MySessionComponentTriggerSystem.Static.RemoveTriggerInternal(entity, trigger);
    }

    private void RemoveTriggerInternal(MyEntity entity, MyTriggerComponent trigger)
    {
      using (this.m_dictionaryLock.AcquireExclusiveUsing())
      {
        CachingHashSet<MyTriggerComponent> cachingHashSet;
        if (!this.m_triggers.TryGetValue(entity, out cachingHashSet))
          return;
        cachingHashSet.Remove(trigger);
        cachingHashSet.ApplyChanges();
        if (cachingHashSet.Count != 0)
          return;
        this.m_triggers.Remove(entity);
      }
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      using (this.m_dictionaryLock.AcquireSharedUsing())
      {
        foreach (CachingHashSet<MyTriggerComponent> cachingHashSet in this.m_triggers.Values)
        {
          cachingHashSet.ApplyChanges();
          foreach (MyTriggerComponent triggerComponent in cachingHashSet)
            triggerComponent.Update();
        }
      }
    }

    public override void Draw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER)
        return;
      using (this.m_dictionaryLock.AcquireSharedUsing())
      {
        foreach (CachingHashSet<MyTriggerComponent> cachingHashSet in this.m_triggers.Values)
        {
          foreach (MyTriggerComponent triggerComponent in cachingHashSet)
            triggerComponent.DebugDraw();
        }
      }
    }

    public bool IsAnyTriggerActive(MyEntity entity)
    {
      using (this.m_dictionaryLock.AcquireSharedUsing())
      {
        if (this.m_triggers.ContainsKey(entity))
        {
          foreach (MyTriggerComponent triggerComponent in this.m_triggers[entity])
          {
            if (triggerComponent.Enabled)
              return true;
          }
          return this.m_triggers[entity].Count == 0;
        }
      }
      return true;
    }

    public bool Contains(MyTriggerComponent trigger)
    {
      using (this.m_dictionaryLock.AcquireSharedUsing())
      {
        foreach (CachingHashSet<MyTriggerComponent> cachingHashSet in this.m_triggers.Values)
        {
          if (cachingHashSet.Contains(trigger))
            return true;
        }
      }
      return false;
    }

    public List<MyTriggerComponent> GetIntersectingTriggers(Vector3D position)
    {
      List<MyTriggerComponent> triggerComponentList = new List<MyTriggerComponent>();
      using (this.m_dictionaryLock.AcquireSharedUsing())
      {
        foreach (CachingHashSet<MyTriggerComponent> cachingHashSet in this.m_triggers.Values)
        {
          foreach (MyTriggerComponent triggerComponent in cachingHashSet)
          {
            if (triggerComponent.Contains(position))
              triggerComponentList.Add(triggerComponent);
          }
        }
      }
      return triggerComponentList;
    }

    public List<MyTriggerComponent> GetAllTriggers()
    {
      List<MyTriggerComponent> triggerComponentList = new List<MyTriggerComponent>();
      using (this.m_dictionaryLock.AcquireSharedUsing())
      {
        foreach (CachingHashSet<MyTriggerComponent> cachingHashSet in this.m_triggers.Values)
        {
          cachingHashSet.ApplyChanges();
          foreach (MyTriggerComponent triggerComponent in cachingHashSet)
            triggerComponentList.Add(triggerComponent);
        }
      }
      return triggerComponentList;
    }
  }
}
