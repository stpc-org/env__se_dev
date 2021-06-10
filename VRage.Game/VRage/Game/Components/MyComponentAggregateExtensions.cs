// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyComponentAggregateExtensions
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;

namespace VRage.Game.Components
{
  public static class MyComponentAggregateExtensions
  {
    public static void AddComponent(this IMyComponentAggregate aggregate, MyComponentBase component)
    {
      if (component.ContainerBase != null)
        component.OnBeforeRemovedFromContainer();
      aggregate.ChildList.AddComponent(component);
      component.SetContainer(aggregate.ContainerBase);
      aggregate.AfterComponentAdd(component);
    }

    public static void AttachComponent(
      this IMyComponentAggregate aggregate,
      MyComponentBase component)
    {
      aggregate.ChildList.AddComponent(component);
    }

    public static bool RemoveComponent(
      this IMyComponentAggregate aggregate,
      MyComponentBase component)
    {
      int componentIndex = aggregate.ChildList.GetComponentIndex(component);
      if (componentIndex != -1)
      {
        aggregate.BeforeComponentRemove(component);
        component.SetContainer((MyComponentContainer) null);
        aggregate.ChildList.RemoveComponentAt(componentIndex);
        return true;
      }
      foreach (MyComponentBase myComponentBase in aggregate.ChildList.Reader)
      {
        if (myComponentBase is IMyComponentAggregate aggregate1 && aggregate1.RemoveComponent(component))
          return true;
      }
      return false;
    }

    public static void DetachComponent(
      this IMyComponentAggregate aggregate,
      MyComponentBase component)
    {
      int componentIndex = aggregate.ChildList.GetComponentIndex(component);
      if (componentIndex == -1)
        return;
      aggregate.ChildList.RemoveComponentAt(componentIndex);
    }

    public static void GetComponentsFlattened(
      this IMyComponentAggregate aggregate,
      List<MyComponentBase> output)
    {
      foreach (MyComponentBase myComponentBase in aggregate.ChildList.Reader)
      {
        if (myComponentBase is IMyComponentAggregate aggregate1)
          aggregate1.GetComponentsFlattened(output);
        else
          output.Add(myComponentBase);
      }
    }
  }
}
