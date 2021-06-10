// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyComponentBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Game.ObjectBuilders.ComponentSystem;

namespace VRage.Game.Components
{
  public abstract class MyComponentBase
  {
    private MyComponentContainer m_container;

    public MyComponentContainer ContainerBase => this.m_container;

    public virtual void SetContainer(MyComponentContainer container)
    {
      if (this.m_container != null)
        this.OnBeforeRemovedFromContainer();
      this.m_container = container;
      if (this is IMyComponentAggregate componentAggregate)
      {
        foreach (MyComponentBase myComponentBase in componentAggregate.ChildList.Reader)
          myComponentBase.SetContainer(container);
      }
      if (container == null)
        return;
      this.OnAddedToContainer();
    }

    public virtual T GetAs<T>() where T : MyComponentBase => this as T;

    public virtual void OnAddedToContainer()
    {
    }

    public virtual void OnBeforeRemovedFromContainer()
    {
    }

    public virtual void OnAddedToScene()
    {
    }

    public virtual void OnRemovedFromScene()
    {
    }

    public virtual MyObjectBuilder_ComponentBase Serialize(bool copy = false) => MyComponentFactory.CreateObjectBuilder(this);

    public virtual void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
    }

    public virtual void Init(MyComponentDefinitionBase definition)
    {
    }

    public virtual bool IsSerialized() => false;
  }
}
