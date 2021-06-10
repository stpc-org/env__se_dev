// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyAggregateComponentList
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Game.Components
{
  public sealed class MyAggregateComponentList
  {
    private List<MyComponentBase> m_components = new List<MyComponentBase>();

    public ListReader<MyComponentBase> Reader => new ListReader<MyComponentBase>(this.m_components);

    public void AddComponent(MyComponentBase component) => this.m_components.Add(component);

    public void RemoveComponentAt(int index) => this.m_components.RemoveAtFast<MyComponentBase>(index);

    public int GetComponentIndex(MyComponentBase component) => this.m_components.IndexOf(component);

    public bool RemoveComponent(MyComponentBase component)
    {
      if (!this.Contains(component))
        return false;
      component.OnBeforeRemovedFromContainer();
      if (this.m_components.Remove(component))
        return true;
      foreach (MyComponentBase component1 in this.m_components)
      {
        if (component1 is IMyComponentAggregate && (component1 as IMyComponentAggregate).ChildList.RemoveComponent(component))
          return true;
      }
      return false;
    }

    public bool Contains(MyComponentBase component)
    {
      if (this.m_components.Contains(component))
        return true;
      foreach (MyComponentBase component1 in this.m_components)
      {
        if (component1 is IMyComponentAggregate && (component1 as IMyComponentAggregate).ChildList.Contains(component))
          return true;
      }
      return false;
    }
  }
}
