// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.IMyUpdateOrchestrator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game.Entity;

namespace Sandbox.Game.Entities
{
  public interface IMyUpdateOrchestrator
  {
    void AddEntity(MyEntity entity);

    void RemoveEntity(MyEntity entity, bool immediate);

    void EntityFlagsChanged(MyEntity entity);

    void InvokeLater(Action action, string debugName = null);

    void DispatchOnceBeforeFrame();

    void DispatchBeforeSimulation();

    void DispatchSimulate();

    void DispatchAfterSimulation();

    void DispatchUpdatingStopped();

    void Unload();

    void DebugDraw();
  }
}
