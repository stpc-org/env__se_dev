// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.MyIndexedComponentContainer`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public class MyIndexedComponentContainer<T> where T : class
  {
    private static readonly IndexHost Host = new IndexHost();
    private ComponentIndex m_componentIndex;
    private readonly List<T> m_components = new List<T>();

    public MyIndexedComponentContainer() => this.m_componentIndex = MyIndexedComponentContainer<T>.Host.GetEmptyComponentIndex();

    public MyIndexedComponentContainer(MyComponentContainerTemplate<T> template)
    {
      this.m_components.Capacity = template.Components.Count;
      for (int index = 0; index < template.Components.Count; ++index)
        this.m_components.Add(template.Components[index](template.Components.m_componentIndex.Types[index]));
      this.m_componentIndex = template.Components.m_componentIndex;
    }

    public T this[int index] => this.m_components[index];

    public T this[Type type] => this.m_components[this.m_componentIndex.Index[type]];

    public int Count => this.m_components.Count;

    public TComponent GetComponent<TComponent>() where TComponent : T => (TComponent) (object) this.m_components[this.m_componentIndex.Index[typeof (TComponent)]];

    public TComponent TryGetComponent<TComponent>() where TComponent : class, T => (TComponent) (object) this.TryGetComponent(typeof (TComponent));

    public T TryGetComponent(Type t)
    {
      int index;
      return this.m_componentIndex.Index.TryGetValue(t, out index) ? this.m_components[index] : default (T);
    }

    public void Add(Type slot, T component)
    {
      if (MyIndexedComponentContainer<T>.Host == null)
        throw new NullReferenceException("Host is null.");
      if (this.m_componentIndex == null)
        throw new NullReferenceException("m_componentIndex is null.");
      if (this.m_componentIndex.Types == null)
        throw new NullReferenceException("m_componentIndex.Types is null.");
      if ((object) component == null)
        throw new NullReferenceException("component is null.");
      if (this.m_componentIndex.Index.ContainsKey(slot))
        return;
      int insertionPoint;
      this.m_componentIndex = MyIndexedComponentContainer<T>.Host.GetAfterInsert(this.m_componentIndex, slot, out insertionPoint);
      if (this.m_componentIndex == null)
        throw new NullReferenceException("After adding new component m_componentIndex became null.");
      this.m_components.Insert(insertionPoint, component);
    }

    public void Remove(Type slot)
    {
      if (!this.m_componentIndex.Index.ContainsKey(slot))
        return;
      int removalPoint;
      this.m_componentIndex = MyIndexedComponentContainer<T>.Host.GetAfterRemove(this.m_componentIndex, slot, out removalPoint);
      this.m_components.RemoveAt(removalPoint);
    }

    public void Clear()
    {
      this.m_components.Clear();
      this.m_componentIndex = MyIndexedComponentContainer<T>.Host.GetEmptyComponentIndex();
    }

    public bool Contains<TComponent>() where TComponent : T => this.m_componentIndex.Index.ContainsKey(typeof (TComponent));
  }
}
