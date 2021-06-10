// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Modules.MyEnvironmentalEntityCacher
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;

namespace Sandbox.Game.WorldEnvironment.Modules
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  public class MyEnvironmentalEntityCacher : MySessionComponentBase
  {
    private const long EntityPreserveTime = 1000;
    private HashSet<long> m_index;
    private MyBinaryStructHeap<long, MyEnvironmentalEntityCacher.EntityReference> m_entities;

    public void QueueEntity(MyEntity entity)
    {
      this.m_entities.Insert(new MyEnvironmentalEntityCacher.EntityReference()
      {
        Entity = entity
      }, MyEnvironmentalEntityCacher.Time() + 1000L);
      this.m_index.Add(entity.EntityId);
      if (this.UpdateOrder != MyUpdateOrder.NoUpdate)
        return;
      this.SetUpdateOrder(MyUpdateOrder.AfterSimulation);
    }

    public MyEntity GetEntity(long entityId) => this.m_index.Remove(entityId) ? this.m_entities.Remove(entityId).Entity : (MyEntity) null;

    public override void UpdateAfterSimulation()
    {
      long num = MyEnvironmentalEntityCacher.Time();
      while (this.m_entities.Count > 0 && this.m_entities.MinKey() < num)
        this.m_index.Remove(this.m_entities.RemoveMin().Entity.EntityId);
      if (this.m_entities.Count != 0)
        return;
      this.SetUpdateOrder(MyUpdateOrder.NoUpdate);
    }

    private static long Time() => MySession.Static.ElapsedGameTime.Ticks / 10000L;

    private struct EntityReference
    {
      public MyEntity Entity;
    }
  }
}
