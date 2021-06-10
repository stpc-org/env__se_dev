// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyDynamicObstacles
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Collections;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyDynamicObstacles
  {
    private readonly CachingList<IMyObstacle> m_obstacles;

    public MyDynamicObstacles() => this.m_obstacles = new CachingList<IMyObstacle>();

    public void Clear() => this.m_obstacles.ClearImmediate();

    public void Update()
    {
      foreach (IMyObstacle obstacle in this.m_obstacles)
        obstacle.Update();
      this.m_obstacles.ApplyChanges();
    }

    public bool IsInObstacle(Vector3D point)
    {
      foreach (IMyObstacle obstacle in this.m_obstacles)
      {
        if (obstacle.Contains(ref point))
          return true;
      }
      return false;
    }

    public void DebugDraw()
    {
      foreach (IMyObstacle obstacle in this.m_obstacles)
        obstacle.DebugDraw();
    }

    public void TryCreateObstacle(MyEntity newEntity)
    {
      if (newEntity.Physics == null || !(newEntity is MyCubeGrid) || newEntity.PositionComp == null)
        return;
      IMyObstacle obstacleForEntity = MyObstacleFactory.CreateObstacleForEntity(newEntity);
      if (obstacleForEntity == null)
        return;
      this.m_obstacles.Add(obstacleForEntity);
    }
  }
}
