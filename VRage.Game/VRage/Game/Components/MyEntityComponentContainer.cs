// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyEntityComponentContainer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.ModAPI;

namespace VRage.Game.Components
{
  public class MyEntityComponentContainer : MyComponentContainer, IMyComponentContainer
  {
    private IMyEntity m_entity;

    public IMyEntity Entity
    {
      get => this.m_entity;
      private set => this.m_entity = value;
    }

    public event Action<Type, MyEntityComponentBase> ComponentAdded;

    public event Action<Type, MyEntityComponentBase> ComponentRemoved;

    public MyEntityComponentContainer(IMyEntity entity) => this.Entity = entity;

    public override void Init(MyContainerDefinition definition)
    {
      if (!definition.Flags.HasValue)
        return;
      this.Entity.Flags |= definition.Flags.Value;
    }

    protected override void OnComponentAdded(Type t, MyComponentBase component)
    {
      base.OnComponentAdded(t, component);
      MyEntityComponentBase entityComponentBase = component as MyEntityComponentBase;
      Action<Type, MyEntityComponentBase> componentAdded = this.ComponentAdded;
      if (componentAdded == null || entityComponentBase == null)
        return;
      componentAdded(t, entityComponentBase);
    }

    protected override void OnComponentRemoved(Type t, MyComponentBase component)
    {
      base.OnComponentRemoved(t, component);
      if (!(component is MyEntityComponentBase entityComponentBase))
        return;
      this.ComponentRemoved.InvokeIfNotNull<Type, MyEntityComponentBase>(t, entityComponentBase);
    }
  }
}
