// Decompiled with JetBrains decompiler
// Type: VRage.Game.SessionComponents.MyPhysicsComponentSystem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.ComponentSystem;

namespace VRage.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  internal class MyPhysicsComponentSystem : MySessionComponentBase
  {
    public static MyPhysicsComponentSystem Static;
    private List<MyPhysicsComponentBase> m_physicsComponents = new List<MyPhysicsComponentBase>();

    public override void LoadData()
    {
      base.LoadData();
      MyPhysicsComponentSystem.Static = this;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyPhysicsComponentSystem.Static = (MyPhysicsComponentSystem) null;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      foreach (MyPhysicsComponentBase physicsComponent in this.m_physicsComponents)
      {
        if (physicsComponent.Definition != null && physicsComponent.Definition.UpdateFlags != (MyObjectBuilder_PhysicsComponentDefinitionBase.MyUpdateFlags) 0)
          physicsComponent.UpdateFromSystem();
      }
    }

    public void Register(MyPhysicsComponentBase component) => this.m_physicsComponents.Add(component);

    public void Unregister(MyPhysicsComponentBase component) => this.m_physicsComponents.Remove(component);
  }
}
