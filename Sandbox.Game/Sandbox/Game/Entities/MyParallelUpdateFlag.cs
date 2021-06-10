// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyParallelUpdateFlag
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Entity;

namespace Sandbox.Game.Entities
{
  public struct MyParallelUpdateFlag
  {
    private bool m_needsUpdate;

    public MyParallelUpdateFlag(bool needsUpdate) => this.m_needsUpdate = needsUpdate;

    public void Enable(MyEntity entity) => this.Set(entity, true);

    public void Disable(MyEntity entity) => this.Set(entity, false);

    public void Set(MyEntity entity, bool value)
    {
      if (value == this.m_needsUpdate)
        return;
      this.m_needsUpdate = value;
      if (!entity.InScene)
        return;
      MyEntities.Orchestrator.EntityFlagsChanged(entity);
    }

    public MyParallelUpdateFlags GetFlags(MyEntity entity)
    {
      MyParallelUpdateFlags parallel = entity.NeedsUpdate.GetParallel();
      if (this.m_needsUpdate)
        parallel |= MyParallelUpdateFlags.EACH_FRAME_PARALLEL;
      return parallel;
    }
  }
}
