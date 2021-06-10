// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Navigation.MyPathSteering
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.AI.Pathfinding;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.AI.Navigation
{
  public class MyPathSteering : MyTargetSteering
  {
    private IMyPath m_path;
    private float m_weight;
    private const float END_RADIUS = 0.5f;
    private const float DISTANCE_FOR_FINAL_APPROACH = 2f;

    public bool PathFinished { get; private set; }

    public bool IsWaitingForTileGeneration => this.m_path != null && this.m_path.IsWaitingForTileGeneration;

    public MyPathSteering(MyBotNavigation navigation)
      : base(navigation)
    {
    }

    public override string GetName() => "Path steering";

    public void SetPath(IMyPath path, float weight = 1f)
    {
      if (path == null || !path.IsValid)
      {
        this.UnsetPath();
      }
      else
      {
        this.m_path?.Invalidate();
        this.m_path = path;
        this.m_weight = weight;
        this.PathFinished = false;
        this.SetNextTarget();
      }
    }

    public void UnsetPath()
    {
      this.m_path?.Invalidate();
      this.m_path = (IMyPath) null;
      this.UnsetTarget();
      this.PathFinished = true;
    }

    private void SetNextTarget()
    {
      Vector3D? targetWorld = this.TargetWorld;
      if (this.m_path == null || !this.m_path.IsValid)
      {
        this.UnsetTarget();
      }
      else
      {
        Vector3D closestPoint = this.m_path.Destination.GetClosestPoint(this.CapsuleCenter());
        double num1 = this.TargetDistanceSq(ref closestPoint);
        if (num1 > 0.25)
        {
          Vector3D translation = this.Parent.PositionAndOrientation.Translation;
          if (this.m_path.PathCompleted)
          {
            if (num1 < 4.0)
            {
              MyEntity endEntity = this.m_path.EndEntity as MyEntity;
              this.UnsetPath();
              this.SetTarget(closestPoint, 0.5f, endEntity, this.m_weight);
              return;
            }
            if (targetWorld.HasValue)
              this.m_path.ReInit(targetWorld.Value);
            else
              this.m_path.ReInit(translation);
          }
          Vector3D target;
          float targetRadius;
          IMyEntity relativeEntity1;
          int num2 = this.m_path.GetNextTarget(this.Parent.PositionAndOrientation.Translation, out target, out targetRadius, out relativeEntity1) ? 1 : 0;
          MyEntity relativeEntity2 = relativeEntity1 as MyEntity;
          if (num2 != 0)
          {
            this.SetTarget(target, targetRadius, relativeEntity2, this.m_weight);
            return;
          }
        }
        if (this.IsWaitingForTileGeneration)
          return;
        this.UnsetPath();
      }
    }

    public override void Update()
    {
      if (this.m_path == null)
        base.Update();
      else if (!this.m_path.IsValid)
      {
        this.UnsetPath();
      }
      else
      {
        if (!this.TargetReached())
          return;
        this.SetNextTarget();
      }
    }

    public override void Cleanup()
    {
      base.Cleanup();
      if (this.m_path == null || !this.m_path.IsValid)
        return;
      this.m_path.Invalidate();
    }

    public override void DebugDraw()
    {
      if (this.m_path == null || !this.m_path.IsValid || (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyFakes.DEBUG_DRAW_FOUND_PATH))
        return;
      this.m_path.DebugDraw();
    }
  }
}
