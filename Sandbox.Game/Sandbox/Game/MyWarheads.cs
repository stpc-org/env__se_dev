// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyWarheads
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
  internal class MyWarheads : MySessionComponentBase
  {
    private static readonly HashSet<MyWarhead> m_warheads = new HashSet<MyWarhead>();
    private static readonly List<MyWarhead> m_warheadsToExplode = new List<MyWarhead>();
    public static List<BoundingSphere> DebugWarheadShrinks = new List<BoundingSphere>();
    public static List<BoundingSphere> DebugWarheadGroupSpheres = new List<BoundingSphere>();

    public override void BeforeStart() => base.BeforeStart();

    protected override void UnloadData()
    {
      MyWarheads.m_warheads.Clear();
      MyWarheads.m_warheadsToExplode.Clear();
      MyWarheads.DebugWarheadShrinks.Clear();
      MyWarheads.DebugWarheadGroupSpheres.Clear();
    }

    public static void AddWarhead(MyWarhead warhead)
    {
      if (!MyWarheads.m_warheads.Add(warhead))
        return;
      warhead.OnMarkForClose += new Action<MyEntity>(MyWarheads.warhead_OnClose);
    }

    public static void RemoveWarhead(MyWarhead warhead)
    {
      if (!MyWarheads.m_warheads.Remove(warhead))
        return;
      warhead.OnMarkForClose -= new Action<MyEntity>(MyWarheads.warhead_OnClose);
    }

    public static bool Contains(MyWarhead warhead) => MyWarheads.m_warheads.Contains(warhead);

    private static void warhead_OnClose(MyEntity obj) => MyWarheads.m_warheads.Remove(obj as MyWarhead);

    public override void UpdateBeforeSimulation()
    {
      int frameMs = 16;
      foreach (MyWarhead warhead in MyWarheads.m_warheads)
      {
        if (warhead.Countdown(frameMs))
        {
          warhead.RemainingMS -= frameMs;
          if (warhead.RemainingMS <= 0)
            MyWarheads.m_warheadsToExplode.Add(warhead);
        }
      }
      foreach (MyWarhead warhead in MyWarheads.m_warheadsToExplode)
      {
        MyWarheads.RemoveWarhead(warhead);
        if (Sync.IsServer)
          warhead.Explode();
      }
      MyWarheads.m_warheadsToExplode.Clear();
    }

    public override void Draw()
    {
      base.Draw();
      foreach (BoundingSphere debugWarheadShrink in MyWarheads.DebugWarheadShrinks)
        MyRenderProxy.DebugDrawSphere((Vector3D) debugWarheadShrink.Center, debugWarheadShrink.Radius, Color.Blue, depthRead: false);
      foreach (BoundingSphere warheadGroupSphere in MyWarheads.DebugWarheadGroupSpheres)
        MyRenderProxy.DebugDrawSphere((Vector3D) warheadGroupSphere.Center, warheadGroupSphere.Radius, Color.Yellow, depthRead: false);
    }
  }
}
