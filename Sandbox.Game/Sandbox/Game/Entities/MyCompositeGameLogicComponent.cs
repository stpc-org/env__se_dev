// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyCompositeGameLogicComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.EntityComponents.Interfaces;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
  public class MyCompositeGameLogicComponent : MyGameLogicComponent, IMyGameLogicComponent
  {
    private ICollection<MyGameLogicComponent> m_logicComponents;

    private MyCompositeGameLogicComponent(ICollection<MyGameLogicComponent> logicComponents) => this.m_logicComponents = logicComponents;

    public static MyGameLogicComponent Create(
      ICollection<MyGameLogicComponent> logicComponents,
      MyEntity entity)
    {
      foreach (MyComponentBase logicComponent in (IEnumerable<MyGameLogicComponent>) logicComponents)
        logicComponent.SetContainer((MyComponentContainer) entity.Components);
      switch (logicComponents.Count)
      {
        case 0:
          return (MyGameLogicComponent) null;
        case 1:
          return logicComponents.First<MyGameLogicComponent>();
        default:
          return (MyGameLogicComponent) new MyCompositeGameLogicComponent(logicComponents);
      }
    }

    void IMyGameLogicComponent.UpdateOnceBeforeFrame(bool entityUpdate)
    {
      foreach (IMyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.UpdateOnceBeforeFrame(entityUpdate);
    }

    void IMyGameLogicComponent.UpdateBeforeSimulation(bool entityUpdate)
    {
      foreach (IMyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.UpdateBeforeSimulation(entityUpdate);
    }

    void IMyGameLogicComponent.UpdateBeforeSimulation10(bool entityUpdate)
    {
      foreach (IMyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.UpdateBeforeSimulation10(entityUpdate);
    }

    void IMyGameLogicComponent.UpdateBeforeSimulation100(bool entityUpdate)
    {
      foreach (IMyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.UpdateBeforeSimulation100(entityUpdate);
    }

    void IMyGameLogicComponent.UpdateAfterSimulation(bool entityUpdate)
    {
      foreach (IMyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.UpdateAfterSimulation(entityUpdate);
    }

    void IMyGameLogicComponent.UpdateAfterSimulation10(bool entityUpdate)
    {
      foreach (IMyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.UpdateAfterSimulation10(entityUpdate);
    }

    void IMyGameLogicComponent.UpdateAfterSimulation100(bool entityUpdate)
    {
      foreach (IMyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.UpdateAfterSimulation100(entityUpdate);
    }

    void IMyGameLogicComponent.Close()
    {
      foreach (IMyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.Close();
    }

    void IMyGameLogicComponent.RegisterForUpdate()
    {
      foreach (IMyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.RegisterForUpdate();
    }

    void IMyGameLogicComponent.UnregisterForUpdate()
    {
      foreach (IMyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.UnregisterForUpdate();
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      foreach (MyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.Init(objectBuilder);
    }

    public override void MarkForClose()
    {
      foreach (MyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.MarkForClose();
    }

    public override void Close()
    {
      foreach (MyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
        logicComponent.Close();
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
    {
      foreach (MyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
      {
        MyObjectBuilder_EntityBase objectBuilder = logicComponent.GetObjectBuilder(copy);
        if (objectBuilder != null)
          return objectBuilder;
      }
      return (MyObjectBuilder_EntityBase) null;
    }

    public override T GetAs<T>()
    {
      foreach (MyGameLogicComponent logicComponent in (IEnumerable<MyGameLogicComponent>) this.m_logicComponents)
      {
        if (logicComponent is T)
          return logicComponent as T;
      }
      return default (T);
    }

    public MyGameLogicComponent GetAs(string typeName) => this.m_logicComponents.FirstOrDefault<MyGameLogicComponent>((Func<MyGameLogicComponent, bool>) (c => c.GetType().FullName == typeName));

    private class Sandbox_Game_Entities_MyCompositeGameLogicComponent\u003C\u003EActor
    {
    }
  }
}
