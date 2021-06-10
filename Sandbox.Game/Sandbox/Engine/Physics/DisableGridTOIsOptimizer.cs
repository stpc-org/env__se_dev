// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.DisableGridTOIsOptimizer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Library.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Physics
{
  internal class DisableGridTOIsOptimizer : PhysicsStepOptimizerBase
  {
    public static DisableGridTOIsOptimizer Static;
    private HashSet<MyGridPhysics> m_optimizedGrids = new HashSet<MyGridPhysics>();

    public DisableGridTOIsOptimizer() => DisableGridTOIsOptimizer.Static = this;

    public override void Unload() => DisableGridTOIsOptimizer.Static = (DisableGridTOIsOptimizer) null;

    public override void EnableOptimizations(List<MyTuple<HkWorld, MyTimeSpan>> timings) => PhysicsStepOptimizerBase.ForEverySignificantWorld(timings, (Action<HkWorld>) (world => PhysicsStepOptimizerBase.ForEveryActivePhysicsBodyOfType<MyGridPhysics>(world, (Action<MyGridPhysics>) (body => body.ConsiderDisablingTOIs()))));

    public override void DisableOptimizations()
    {
      while (this.m_optimizedGrids.Count > 0)
        this.m_optimizedGrids.FirstElement<MyGridPhysics>().DisableTOIOptimization();
    }

    public void Register(MyGridPhysics grid) => this.m_optimizedGrids.Add(grid);

    public void Unregister(MyGridPhysics grid) => this.m_optimizedGrids.Remove(grid);

    public void DebugDraw()
    {
      foreach (MyGridPhysics optimizedGrid in DisableGridTOIsOptimizer.Static.m_optimizedGrids)
        MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD((BoundingBoxD) optimizedGrid.Entity.LocalAABB, optimizedGrid.Entity.WorldMatrix), Color.Yellow, 1f, false, false);
    }
  }
}
