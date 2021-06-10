// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentWaypoint
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyDebugRenderComponentWaypoint : MyDebugRenderComponent
  {
    private MyWaypoint m_waypoint;

    public MyDebugRenderComponentWaypoint(MyWaypoint waypoint)
      : base((IMyEntity) waypoint)
      => this.m_waypoint = waypoint;

    public override void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_WAYPOINTS || !this.m_waypoint.Visible)
        return;
      Color color = this.m_waypoint.Freeze ? Color.Gray : Color.Yellow;
      MatrixD matrix1 = this.m_waypoint.PositionComp.WorldMatrixRef;
      MyRenderProxy.DebugDrawAxis(matrix1, 0.5f, false);
      Vector3D translation = matrix1.Translation;
      MatrixD matrix2 = -matrix1;
      matrix2.Translation = translation;
      MyRenderProxy.DebugDrawAxis(matrix2, 0.5f, false, customColor: new Color?(color));
      MyRenderProxy.DebugDrawText3D(this.m_waypoint.PositionComp.GetPosition() + new Vector3D(0.0500000007450581), this.m_waypoint.Name, color, 0.7f, false);
    }
  }
}
