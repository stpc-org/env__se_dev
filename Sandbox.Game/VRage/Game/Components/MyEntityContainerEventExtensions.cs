// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyEntityContainerEventExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Components
{
  public static class MyEntityContainerEventExtensions
  {
    private static Dictionary<long, MyEntityContainerEventExtensions.RegisteredEvents> RegisteredListeners = new Dictionary<long, MyEntityContainerEventExtensions.RegisteredEvents>();
    private static Dictionary<MyComponentBase, List<long>> ExternalListeners = new Dictionary<MyComponentBase, List<long>>();
    private static HashSet<long> ExternalyListenedEntities = new HashSet<long>();
    private static List<MyEntityContainerEventExtensions.RegisteredComponent> m_tmpList = new List<MyEntityContainerEventExtensions.RegisteredComponent>();
    private static List<MyComponentBase> m_tmpCompList = new List<MyComponentBase>();
    private static bool ProcessingEvents;
    private static bool HasPostponedOperations;
    private static List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash, MyEntityContainerEventExtensions.EntityEventHandler>> PostponedRegistration = new List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash, MyEntityContainerEventExtensions.EntityEventHandler>>();
    private static List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash>> PostponedUnregistration = new List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash>>();
    private static List<long> PostPonedRegisteredListenersRemoval = new List<long>();
    private static int m_debugCounter;

    public static void InitEntityEvents()
    {
      MyEntityContainerEventExtensions.RegisteredListeners = new Dictionary<long, MyEntityContainerEventExtensions.RegisteredEvents>();
      MyEntityContainerEventExtensions.ExternalListeners = new Dictionary<MyComponentBase, List<long>>();
      MyEntityContainerEventExtensions.ExternalyListenedEntities = new HashSet<long>();
      MyEntityContainerEventExtensions.PostponedRegistration = new List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash, MyEntityContainerEventExtensions.EntityEventHandler>>();
      MyEntityContainerEventExtensions.PostponedUnregistration = new List<Tuple<MyEntityComponentBase, MyEntity, MyStringHash>>();
      MyEntityContainerEventExtensions.ProcessingEvents = false;
      MyEntityContainerEventExtensions.HasPostponedOperations = false;
    }

    public static void RegisterForEntityEvent(
      this MyEntityComponentBase component,
      MyStringHash eventType,
      MyEntityContainerEventExtensions.EntityEventHandler handler)
    {
      if (MyEntityContainerEventExtensions.ProcessingEvents)
      {
        MyEntityContainerEventExtensions.AddPostponedRegistration(component, component.Entity as MyEntity, eventType, handler);
      }
      else
      {
        if (component.Entity == null)
          return;
        component.BeforeRemovedFromContainer += new Action<MyEntityComponentBase>(MyEntityContainerEventExtensions.RegisteredComponentBeforeRemovedFromContainer);
        component.Entity.OnClose += new Action<IMyEntity>(MyEntityContainerEventExtensions.RegisteredEntityOnClose);
        if (MyEntityContainerEventExtensions.RegisteredListeners.ContainsKey(component.Entity.EntityId))
        {
          MyEntityContainerEventExtensions.RegisteredEvents registeredListener = MyEntityContainerEventExtensions.RegisteredListeners[component.Entity.EntityId];
          if (registeredListener.ContainsKey(eventType))
          {
            if (registeredListener[eventType].Find((Predicate<MyEntityContainerEventExtensions.RegisteredComponent>) (x => x.Handler == handler)) != null)
              return;
            registeredListener[eventType].Add(new MyEntityContainerEventExtensions.RegisteredComponent((MyComponentBase) component, handler));
          }
          else
          {
            registeredListener[eventType] = new List<MyEntityContainerEventExtensions.RegisteredComponent>();
            registeredListener[eventType].Add(new MyEntityContainerEventExtensions.RegisteredComponent((MyComponentBase) component, handler));
          }
        }
        else
          MyEntityContainerEventExtensions.RegisteredListeners[component.Entity.EntityId] = new MyEntityContainerEventExtensions.RegisteredEvents(eventType, (MyComponentBase) component, handler);
      }
    }

    public static void RegisterForEntityEvent(
      this MyEntityComponentBase component,
      MyEntity entity,
      MyStringHash eventType,
      MyEntityContainerEventExtensions.EntityEventHandler handler)
    {
      if (MyEntityContainerEventExtensions.ProcessingEvents)
        MyEntityContainerEventExtensions.AddPostponedRegistration(component, entity, eventType, handler);
      else if (component.Entity == entity)
      {
        component.RegisterForEntityEvent(eventType, handler);
      }
      else
      {
        if (entity == null)
          return;
        component.BeforeRemovedFromContainer += new Action<MyEntityComponentBase>(MyEntityContainerEventExtensions.RegisteredComponentBeforeRemovedFromContainer);
        entity.OnClose += new Action<MyEntity>(MyEntityContainerEventExtensions.RegisteredEntityOnClose);
        if (MyEntityContainerEventExtensions.RegisteredListeners.ContainsKey(entity.EntityId))
        {
          MyEntityContainerEventExtensions.RegisteredEvents registeredListener = MyEntityContainerEventExtensions.RegisteredListeners[entity.EntityId];
          if (registeredListener.ContainsKey(eventType))
          {
            if (registeredListener[eventType].Find((Predicate<MyEntityContainerEventExtensions.RegisteredComponent>) (x => x.Handler == handler)) == null)
              registeredListener[eventType].Add(new MyEntityContainerEventExtensions.RegisteredComponent((MyComponentBase) component, handler));
          }
          else
          {
            registeredListener[eventType] = new List<MyEntityContainerEventExtensions.RegisteredComponent>();
            registeredListener[eventType].Add(new MyEntityContainerEventExtensions.RegisteredComponent((MyComponentBase) component, handler));
          }
        }
        else
          MyEntityContainerEventExtensions.RegisteredListeners[entity.EntityId] = new MyEntityContainerEventExtensions.RegisteredEvents(eventType, (MyComponentBase) component, handler);
        if (MyEntityContainerEventExtensions.ExternalListeners.ContainsKey((MyComponentBase) component) && !MyEntityContainerEventExtensions.ExternalListeners[(MyComponentBase) component].Contains(entity.EntityId))
          MyEntityContainerEventExtensions.ExternalListeners[(MyComponentBase) component].Add(entity.EntityId);
        else
          MyEntityContainerEventExtensions.ExternalListeners[(MyComponentBase) component] = new List<long>()
          {
            entity.EntityId
          };
        MyEntityContainerEventExtensions.ExternalyListenedEntities.Add(entity.EntityId);
      }
    }

    public static void UnregisterForEntityEvent(
      this MyEntityComponentBase component,
      MyEntity entity,
      MyStringHash eventType)
    {
      if (MyEntityContainerEventExtensions.ProcessingEvents)
      {
        MyEntityContainerEventExtensions.AddPostponedUnregistration(component, entity, eventType);
      }
      else
      {
        if (entity == null)
          return;
        bool flag = true;
        if (MyEntityContainerEventExtensions.RegisteredListeners.ContainsKey(entity.EntityId))
        {
          if (MyEntityContainerEventExtensions.RegisteredListeners[entity.EntityId].ContainsKey(eventType))
          {
            MyEntityContainerEventExtensions.RegisteredListeners[entity.EntityId][eventType].RemoveAll((Predicate<MyEntityContainerEventExtensions.RegisteredComponent>) (x => x.Component == component));
            if (MyEntityContainerEventExtensions.RegisteredListeners[entity.EntityId][eventType].Count == 0)
              MyEntityContainerEventExtensions.RegisteredListeners[entity.EntityId].Remove(eventType);
          }
          if (MyEntityContainerEventExtensions.RegisteredListeners[entity.EntityId].Count == 0)
          {
            MyEntityContainerEventExtensions.RegisteredListeners.Remove(entity.EntityId);
            MyEntityContainerEventExtensions.ExternalyListenedEntities.Remove(entity.EntityId);
            flag = false;
          }
        }
        if (MyEntityContainerEventExtensions.ExternalListeners.ContainsKey((MyComponentBase) component) && MyEntityContainerEventExtensions.ExternalListeners[(MyComponentBase) component].Contains(entity.EntityId))
        {
          MyEntityContainerEventExtensions.ExternalListeners[(MyComponentBase) component].Remove(entity.EntityId);
          if (MyEntityContainerEventExtensions.ExternalListeners[(MyComponentBase) component].Count == 0)
            MyEntityContainerEventExtensions.ExternalListeners.Remove((MyComponentBase) component);
        }
        if (flag)
          return;
        entity.OnClose -= new Action<MyEntity>(MyEntityContainerEventExtensions.RegisteredEntityOnClose);
      }
    }

    private static void RegisteredEntityOnClose(IMyEntity entity)
    {
      entity.OnClose -= new Action<IMyEntity>(MyEntityContainerEventExtensions.RegisteredEntityOnClose);
      if (MyEntityContainerEventExtensions.RegisteredListeners.ContainsKey(entity.EntityId))
      {
        if (MyEntityContainerEventExtensions.ProcessingEvents)
          MyEntityContainerEventExtensions.AddPostponedListenerRemoval(entity.EntityId);
        else
          MyEntityContainerEventExtensions.RegisteredListeners.Remove(entity.EntityId);
      }
      if (!MyEntityContainerEventExtensions.ExternalyListenedEntities.Contains(entity.EntityId))
        return;
      MyEntityContainerEventExtensions.ExternalyListenedEntities.Remove(entity.EntityId);
      MyEntityContainerEventExtensions.m_tmpCompList.Clear();
      foreach (KeyValuePair<MyComponentBase, List<long>> externalListener in MyEntityContainerEventExtensions.ExternalListeners)
      {
        externalListener.Value.Remove(entity.EntityId);
        if (externalListener.Value.Count == 0)
          MyEntityContainerEventExtensions.m_tmpCompList.Add(externalListener.Key);
      }
      foreach (MyComponentBase tmpComp in MyEntityContainerEventExtensions.m_tmpCompList)
        MyEntityContainerEventExtensions.ExternalListeners.Remove(tmpComp);
    }

    private static void RegisteredComponentBeforeRemovedFromContainer(
      MyEntityComponentBase component)
    {
      component.BeforeRemovedFromContainer -= new Action<MyEntityComponentBase>(MyEntityContainerEventExtensions.RegisteredComponentBeforeRemovedFromContainer);
      if (component.Entity == null)
        return;
      if (MyEntityContainerEventExtensions.RegisteredListeners.ContainsKey(component.Entity.EntityId))
      {
        MyEntityContainerEventExtensions.m_tmpList.Clear();
        foreach (KeyValuePair<MyStringHash, List<MyEntityContainerEventExtensions.RegisteredComponent>> keyValuePair in (Dictionary<MyStringHash, List<MyEntityContainerEventExtensions.RegisteredComponent>>) MyEntityContainerEventExtensions.RegisteredListeners[component.Entity.EntityId])
          keyValuePair.Value.RemoveAll((Predicate<MyEntityContainerEventExtensions.RegisteredComponent>) (x => x.Component == component));
      }
      if (!MyEntityContainerEventExtensions.ExternalListeners.ContainsKey((MyComponentBase) component))
        return;
      foreach (long key in MyEntityContainerEventExtensions.ExternalListeners[(MyComponentBase) component])
      {
        if (MyEntityContainerEventExtensions.RegisteredListeners.ContainsKey(key))
        {
          foreach (KeyValuePair<MyStringHash, List<MyEntityContainerEventExtensions.RegisteredComponent>> keyValuePair in (Dictionary<MyStringHash, List<MyEntityContainerEventExtensions.RegisteredComponent>>) MyEntityContainerEventExtensions.RegisteredListeners[key])
            keyValuePair.Value.RemoveAll((Predicate<MyEntityContainerEventExtensions.RegisteredComponent>) (x => x.Component == component));
        }
      }
      MyEntityContainerEventExtensions.ExternalListeners.Remove((MyComponentBase) component);
    }

    public static void RaiseEntityEvent(
      this MyEntity entity,
      MyStringHash eventType,
      MyEntityContainerEventExtensions.EntityEventParams eventParams)
    {
      if (entity.Components == null)
        return;
      MyEntityContainerEventExtensions.InvokeEventOnListeners(entity.EntityId, eventType, eventParams);
    }

    public static void RaiseEntityEventOn(
      MyEntity entity,
      MyStringHash eventType,
      MyEntityContainerEventExtensions.EntityEventParams eventParams)
    {
      if (entity.Components == null)
        return;
      MyEntityContainerEventExtensions.InvokeEventOnListeners(entity.EntityId, eventType, eventParams);
    }

    public static void RaiseEntityEvent(
      this MyEntityComponentBase component,
      MyStringHash eventType,
      MyEntityContainerEventExtensions.EntityEventParams eventParams)
    {
      if (component.Entity == null)
        return;
      MyEntityContainerEventExtensions.InvokeEventOnListeners(component.Entity.EntityId, eventType, eventParams);
    }

    private static void InvokeEventOnListeners(
      long entityId,
      MyStringHash eventType,
      MyEntityContainerEventExtensions.EntityEventParams eventParams)
    {
      bool processingEvents = MyEntityContainerEventExtensions.ProcessingEvents;
      if (processingEvents)
        ++MyEntityContainerEventExtensions.m_debugCounter;
      if (MyEntityContainerEventExtensions.m_debugCounter > 5)
        return;
      MyEntityContainerEventExtensions.ProcessingEvents = true;
      MyEntityContainerEventExtensions.RegisteredEvents registeredEvents;
      List<MyEntityContainerEventExtensions.RegisteredComponent> registeredComponentList;
      if (MyEntityContainerEventExtensions.RegisteredListeners.TryGetValue(entityId, out registeredEvents) && registeredEvents.TryGetValue(eventType, out registeredComponentList))
      {
        foreach (MyEntityContainerEventExtensions.RegisteredComponent registeredComponent in registeredComponentList)
        {
          try
          {
            registeredComponent.Handler(eventParams);
          }
          catch (Exception ex)
          {
          }
        }
      }
      MyEntityContainerEventExtensions.ProcessingEvents = processingEvents;
      if (!MyEntityContainerEventExtensions.ProcessingEvents)
        MyEntityContainerEventExtensions.m_debugCounter = 0;
      if (!MyEntityContainerEventExtensions.HasPostponedOperations || MyEntityContainerEventExtensions.ProcessingEvents)
        return;
      MyEntityContainerEventExtensions.ProcessPostponedRegistrations();
    }

    private static void ProcessPostponedRegistrations()
    {
      foreach (Tuple<MyEntityComponentBase, MyEntity, MyStringHash, MyEntityContainerEventExtensions.EntityEventHandler> tuple in MyEntityContainerEventExtensions.PostponedRegistration)
        tuple.Item1.RegisterForEntityEvent(tuple.Item2, tuple.Item3, tuple.Item4);
      foreach (Tuple<MyEntityComponentBase, MyEntity, MyStringHash> tuple in MyEntityContainerEventExtensions.PostponedUnregistration)
        tuple.Item1.UnregisterForEntityEvent(tuple.Item2, tuple.Item3);
      foreach (long key in MyEntityContainerEventExtensions.PostPonedRegisteredListenersRemoval)
        MyEntityContainerEventExtensions.RegisteredListeners.Remove(key);
      MyEntityContainerEventExtensions.PostponedRegistration.Clear();
      MyEntityContainerEventExtensions.PostponedUnregistration.Clear();
      MyEntityContainerEventExtensions.PostPonedRegisteredListenersRemoval.Clear();
      MyEntityContainerEventExtensions.HasPostponedOperations = false;
    }

    private static void AddPostponedRegistration(
      MyEntityComponentBase component,
      MyEntity entity,
      MyStringHash eventType,
      MyEntityContainerEventExtensions.EntityEventHandler handler)
    {
      MyEntityContainerEventExtensions.PostponedRegistration.Add(new Tuple<MyEntityComponentBase, MyEntity, MyStringHash, MyEntityContainerEventExtensions.EntityEventHandler>(component, entity, eventType, handler));
      MyEntityContainerEventExtensions.HasPostponedOperations = true;
    }

    private static void AddPostponedUnregistration(
      MyEntityComponentBase component,
      MyEntity entity,
      MyStringHash eventType)
    {
      MyEntityContainerEventExtensions.PostponedUnregistration.Add(new Tuple<MyEntityComponentBase, MyEntity, MyStringHash>(component, entity, eventType));
      MyEntityContainerEventExtensions.HasPostponedOperations = true;
    }

    private static void AddPostponedListenerRemoval(long id)
    {
      MyEntityContainerEventExtensions.PostPonedRegisteredListenersRemoval.Add(id);
      MyEntityContainerEventExtensions.HasPostponedOperations = true;
    }

    public static void SkipProcessingEvents(bool state)
    {
      MyEntityContainerEventExtensions.ProcessingEvents = state;
      if (state || !MyEntityContainerEventExtensions.HasPostponedOperations)
        return;
      MyEntityContainerEventExtensions.ProcessPostponedRegistrations();
    }

    public class EntityEventParams
    {
    }

    public class ControlAcquiredParams : MyEntityContainerEventExtensions.EntityEventParams
    {
      public MyEntity Owner;

      public ControlAcquiredParams(MyEntity owner) => this.Owner = owner;
    }

    public class ControlReleasedParams : MyEntityContainerEventExtensions.EntityEventParams
    {
      public MyEntity Owner;

      public ControlReleasedParams(MyEntity owner) => this.Owner = owner;
    }

    public class ModelChangedParams : MyEntityContainerEventExtensions.EntityEventParams
    {
      public Vector3 Size;
      public float Mass;
      public float Volume;
      public string Model;
      public string DisplayName;
      public string[] Icons;

      public ModelChangedParams(
        string model,
        Vector3 size,
        float mass,
        float volume,
        string displayName,
        string[] icons)
      {
        this.Model = model;
        this.Size = size;
        this.Mass = mass;
        this.Volume = volume;
        this.DisplayName = displayName;
        this.Icons = icons;
      }
    }

    public class InventoryChangedParams : MyEntityContainerEventExtensions.EntityEventParams
    {
      public uint ItemId;
      public float Amount;
      public MyInventoryBase Inventory;

      public InventoryChangedParams(uint itemId, MyInventoryBase inventory, float amount)
      {
        this.ItemId = itemId;
        this.Inventory = inventory;
        this.Amount = amount;
      }
    }

    public class HitParams : MyEntityContainerEventExtensions.EntityEventParams
    {
      public MyStringHash HitEntity;
      public MyStringHash HitAction;

      public HitParams(MyStringHash hitAction, MyStringHash hitEntity)
      {
        this.HitEntity = hitEntity;
        this.HitAction = hitAction;
      }
    }

    public delegate void EntityEventHandler(
      MyEntityContainerEventExtensions.EntityEventParams eventParams);

    private class RegisteredComponent
    {
      public MyComponentBase Component;
      public MyEntityContainerEventExtensions.EntityEventHandler Handler;

      public RegisteredComponent(
        MyComponentBase component,
        MyEntityContainerEventExtensions.EntityEventHandler handler)
      {
        this.Component = component;
        this.Handler = handler;
      }
    }

    private class RegisteredEvents : Dictionary<MyStringHash, List<MyEntityContainerEventExtensions.RegisteredComponent>>
    {
      public RegisteredEvents(
        MyStringHash eventType,
        MyComponentBase component,
        MyEntityContainerEventExtensions.EntityEventHandler handler)
      {
        this[eventType] = new List<MyEntityContainerEventExtensions.RegisteredComponent>();
        this[eventType].Add(new MyEntityContainerEventExtensions.RegisteredComponent(component, handler));
      }
    }
  }
}
