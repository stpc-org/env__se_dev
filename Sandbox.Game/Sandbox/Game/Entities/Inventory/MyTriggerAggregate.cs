// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Inventory.MyTriggerAggregate
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Components;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.Entities.Inventory
{
  [MyComponentBuilder(typeof (MyObjectBuilder_TriggerAggregate), true)]
  public class MyTriggerAggregate : MyEntityComponentBase, IMyComponentAggregate
  {
    private int m_triggerCount;
    private MyAggregateComponentList m_children = new MyAggregateComponentList();

    public event Action<MyTriggerAggregate, int> OnTriggerCountChanged;

    public override string ComponentTypeDebugString => "TriggerAggregate";

    public int TriggerCount
    {
      get => this.m_triggerCount;
      private set
      {
        if (this.m_triggerCount == value)
          return;
        int num = value - this.m_triggerCount;
        this.m_triggerCount = value;
        if (this.OnTriggerCountChanged == null)
          return;
        this.OnTriggerCountChanged(this, num);
      }
    }

    public override bool IsSerialized() => true;

    public MyAggregateComponentList ChildList => this.m_children;

    public void AfterComponentAdd(MyComponentBase component)
    {
      switch (component)
      {
        case MyTriggerComponent _:
          ++this.TriggerCount;
          break;
        case MyTriggerAggregate _:
          (component as MyTriggerAggregate).OnTriggerCountChanged += new Action<MyTriggerAggregate, int>(this.OnChildAggregateCountChanged);
          this.TriggerCount += (component as MyTriggerAggregate).TriggerCount;
          break;
      }
    }

    private void OnChildAggregateCountChanged(MyTriggerAggregate obj, int change) => this.TriggerCount += change;

    public void BeforeComponentRemove(MyComponentBase component)
    {
      switch (component)
      {
        case MyTriggerComponent _:
          --this.TriggerCount;
          break;
        case MyTriggerAggregate _:
          (component as MyTriggerAggregate).OnTriggerCountChanged -= new Action<MyTriggerAggregate, int>(this.OnChildAggregateCountChanged);
          this.TriggerCount -= (component as MyTriggerAggregate).TriggerCount;
          break;
      }
    }

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_TriggerAggregate triggerAggregate = base.Serialize() as MyObjectBuilder_TriggerAggregate;
      ListReader<MyComponentBase> reader = this.m_children.Reader;
      if (reader.Count > 0)
      {
        triggerAggregate.AreaTriggers = new List<MyObjectBuilder_TriggerBase>(reader.Count);
        foreach (MyComponentBase myComponentBase in reader)
        {
          if (myComponentBase.Serialize() is MyObjectBuilder_TriggerBase builderTriggerBase)
            triggerAggregate.AreaTriggers.Add(builderTriggerBase);
        }
      }
      return (MyObjectBuilder_ComponentBase) triggerAggregate;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      base.Deserialize(builder);
      if (!(builder is MyObjectBuilder_TriggerAggregate triggerAggregate) || triggerAggregate.AreaTriggers == null)
        return;
      foreach (MyObjectBuilder_TriggerBase areaTrigger in triggerAggregate.AreaTriggers)
      {
        MyComponentBase instanceByTypeId = MyComponentFactory.CreateInstanceByTypeId(areaTrigger.TypeId);
        instanceByTypeId.Deserialize((MyObjectBuilder_ComponentBase) areaTrigger);
        this.AddComponent(instanceByTypeId);
      }
    }

    public override void OnAddedToScene()
    {
      base.OnAddedToScene();
      foreach (MyComponentBase myComponentBase in this.ChildList.Reader)
        myComponentBase.OnAddedToScene();
    }

    public override void OnRemovedFromScene()
    {
      base.OnRemovedFromScene();
      foreach (MyComponentBase myComponentBase in this.ChildList.Reader)
        myComponentBase.OnRemovedFromScene();
    }

    [SpecialName]
    MyComponentContainer IMyComponentAggregate.get_ContainerBase() => this.ContainerBase;

    private class Sandbox_Game_Entities_Inventory_MyTriggerAggregate\u003C\u003EActor : IActivator, IActivator<MyTriggerAggregate>
    {
      object IActivator.CreateInstance() => (object) new MyTriggerAggregate();

      MyTriggerAggregate IActivator<MyTriggerAggregate>.CreateInstance() => new MyTriggerAggregate();
    }
  }
}
