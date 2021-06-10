// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyCubeGridUpdateQueueExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;

namespace Sandbox.Game.Entities
{
  public static class MyCubeGridUpdateQueueExtensions
  {
    public static bool IsExecutedOnce(this MyCubeGrid.UpdateQueue queue)
    {
      switch (queue)
      {
        case MyCubeGrid.UpdateQueue.BeforeSimulation:
          return false;
        case MyCubeGrid.UpdateQueue.OnceBeforeSimulation:
          return true;
        case MyCubeGrid.UpdateQueue.AfterSimulation:
          return false;
        case MyCubeGrid.UpdateQueue.OnceAfterSimulation:
          return true;
        default:
          throw new ArgumentOutOfRangeException(nameof (queue), (object) queue, (string) null);
      }
    }

    public static bool IsBeforeSimulation(this MyCubeGrid.UpdateQueue queue)
    {
      switch (queue)
      {
        case MyCubeGrid.UpdateQueue.BeforeSimulation:
          return true;
        case MyCubeGrid.UpdateQueue.OnceBeforeSimulation:
          return true;
        case MyCubeGrid.UpdateQueue.AfterSimulation:
          return false;
        case MyCubeGrid.UpdateQueue.OnceAfterSimulation:
          return false;
        default:
          throw new ArgumentOutOfRangeException(nameof (queue), (object) queue, (string) null);
      }
    }

    public static bool IsAfterSimulation(this MyCubeGrid.UpdateQueue queue)
    {
      switch (queue)
      {
        case MyCubeGrid.UpdateQueue.BeforeSimulation:
          return false;
        case MyCubeGrid.UpdateQueue.OnceBeforeSimulation:
          return false;
        case MyCubeGrid.UpdateQueue.AfterSimulation:
          return true;
        case MyCubeGrid.UpdateQueue.OnceAfterSimulation:
          return true;
        default:
          throw new ArgumentOutOfRangeException(nameof (queue), (object) queue, (string) null);
      }
    }
  }
}
