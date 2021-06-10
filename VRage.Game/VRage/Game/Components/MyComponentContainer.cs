// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyComponentContainer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Utils;

namespace VRage.Game.Components
{
  public class MyComponentContainer
  {
    private readonly Dictionary<Type, MyComponentBase> m_components = new Dictionary<Type, MyComponentBase>();
    private static List<KeyValuePair<Type, MyComponentBase>> m_tmpComponents;
    [ThreadStatic]
    private static List<KeyValuePair<Type, MyComponentBase>> m_tmpSerializedComponents;

    public void Add<T>(T component) where T : MyComponentBase => this.Add(typeof (T), (MyComponentBase) component);

    public void Add(Type type, MyComponentBase component)
    {
      if (!typeof (MyComponentBase).IsAssignableFrom(type) || component != null && !type.IsAssignableFrom(component.GetType()))
        return;
      Type componentType = MyComponentTypeFactory.GetComponentType(type);
      if (componentType != (Type) null)
      {
        int num = componentType != type ? 1 : 0;
      }
      MyComponentBase component1;
      if (this.m_components.TryGetValue(type, out component1))
      {
        if (component1 is IMyComponentAggregate)
        {
          (component1 as IMyComponentAggregate).AddComponent(component);
          return;
        }
        if (component is IMyComponentAggregate)
        {
          this.Remove(type);
          (component as IMyComponentAggregate).AddComponent(component1);
          this.m_components[type] = component;
          component.SetContainer(this);
          this.OnComponentAdded(type, component);
          return;
        }
      }
      this.Remove(type);
      if (component == null)
        return;
      this.m_components[type] = component;
      component.SetContainer(this);
      this.OnComponentAdded(type, component);
    }

    public void Remove<T>() where T : MyComponentBase => this.Remove(typeof (T));

    public void Remove(Type t)
    {
      MyComponentBase c;
      if (!this.m_components.TryGetValue(t, out c))
        return;
      this.RemoveComponentInternal(t, c);
    }

    private void RemoveComponentInternal(Type t, MyComponentBase c)
    {
      c.SetContainer((MyComponentContainer) null);
      this.m_components.Remove(t);
      this.OnComponentRemoved(t, c);
    }

    public void Remove(Type t, MyComponentBase component)
    {
      MyComponentBase myComponentBase = (MyComponentBase) null;
      this.m_components.TryGetValue(t, out myComponentBase);
      if (myComponentBase == null)
        return;
      if (!(myComponentBase is IMyComponentAggregate aggregate))
        this.RemoveComponentInternal(t, component);
      else
        aggregate.RemoveComponent(component);
    }

    public T Get<T>() where T : MyComponentBase
    {
      MyComponentBase myComponentBase;
      this.m_components.TryGetValue(typeof (T), out myComponentBase);
      return (T) myComponentBase;
    }

    public bool TryGet<T>(out T component) where T : MyComponentBase
    {
      MyComponentBase myComponentBase;
      int num = this.m_components.TryGetValue(typeof (T), out myComponentBase) ? 1 : 0;
      component = (T) myComponentBase;
      return num != 0;
    }

    public bool TryGet(Type type, out MyComponentBase component) => this.m_components.TryGetValue(type, out component);

    public bool Has<T>() where T : MyComponentBase => this.m_components.ContainsKey(typeof (T));

    public bool Contains(Type type)
    {
      foreach (Type key in this.m_components.Keys)
      {
        if (type.IsAssignableFrom(key))
          return true;
      }
      return false;
    }

    public void Clear()
    {
      if (this.m_components.Count <= 0)
        return;
      using (MyUtils.ClearCollectionToken<List<KeyValuePair<Type, MyComponentBase>>, KeyValuePair<Type, MyComponentBase>> clearCollectionToken = MyUtils.ReuseCollection<KeyValuePair<Type, MyComponentBase>>(ref MyComponentContainer.m_tmpComponents))
      {
        List<KeyValuePair<Type, MyComponentBase>> collection = clearCollectionToken.Collection;
        foreach (KeyValuePair<Type, MyComponentBase> component in this.m_components)
        {
          collection.Add(component);
          component.Value.SetContainer((MyComponentContainer) null);
        }
        this.m_components.Clear();
        foreach (KeyValuePair<Type, MyComponentBase> keyValuePair in collection)
          this.OnComponentRemoved(keyValuePair.Key, keyValuePair.Value);
      }
    }

    public void OnAddedToScene()
    {
      foreach (KeyValuePair<Type, MyComponentBase> component in this.m_components)
        component.Value.OnAddedToScene();
    }

    public void OnRemovedFromScene()
    {
      foreach (KeyValuePair<Type, MyComponentBase> component in this.m_components)
        component.Value.OnRemovedFromScene();
    }

    public virtual void Init(MyContainerDefinition definition)
    {
    }

    protected virtual void OnComponentAdded(Type t, MyComponentBase component)
    {
    }

    protected virtual void OnComponentRemoved(Type t, MyComponentBase component)
    {
    }

    public MyObjectBuilder_ComponentContainer Serialize(bool copy = false)
    {
      using (MyUtils.ClearRangeToken<KeyValuePair<Type, MyComponentBase>> clearRangeToken = MyUtils.ReuseCollectionNested<KeyValuePair<Type, MyComponentBase>>(ref MyComponentContainer.m_tmpSerializedComponents))
      {
        foreach (KeyValuePair<Type, MyComponentBase> component in this.m_components)
        {
          if (component.Value.IsSerialized())
            clearRangeToken.Add(component);
        }
        if (clearRangeToken.Collection.Count == 0)
          return (MyObjectBuilder_ComponentContainer) null;
        MyObjectBuilder_ComponentContainer componentContainer = new MyObjectBuilder_ComponentContainer();
        foreach (KeyValuePair<Type, MyComponentBase> keyValuePair in clearRangeToken)
        {
          MyObjectBuilder_ComponentBase builderComponentBase = keyValuePair.Value.Serialize(copy);
          if (builderComponentBase != null)
            componentContainer.Components.Add(new MyObjectBuilder_ComponentContainer.ComponentData()
            {
              TypeId = keyValuePair.Key.Name,
              Component = builderComponentBase
            });
        }
        return componentContainer;
      }
    }

    public void Deserialize(MyObjectBuilder_ComponentContainer builder)
    {
      if (builder == null || builder.Components == null)
        return;
      foreach (MyObjectBuilder_ComponentContainer.ComponentData component1 in builder.Components)
      {
        MyComponentBase component2 = (MyComponentBase) null;
        Type createdInstanceType = MyComponentFactory.GetCreatedInstanceType(component1.Component.TypeId);
        Type type = MyComponentTypeFactory.GetType(component1.TypeId);
        Type componentType = MyComponentTypeFactory.GetComponentType(createdInstanceType);
        if (componentType != (Type) null)
          type = componentType;
        bool flag = this.TryGet(type, out component2);
        if (flag && createdInstanceType != component2.GetType() && createdInstanceType != typeof (MyHierarchyComponentBase))
          flag = false;
        if (!flag)
          component2 = MyComponentFactory.CreateInstanceByTypeId(component1.Component.TypeId);
        component2.Deserialize(component1.Component);
        if (!flag)
          this.Add(type, component2);
      }
    }

    public Dictionary<Type, MyComponentBase>.ValueCollection.Enumerator GetEnumerator() => this.m_components.Values.GetEnumerator();

    public Dictionary<Type, MyComponentBase>.KeyCollection GetComponentTypes() => this.m_components.Keys;
  }
}
