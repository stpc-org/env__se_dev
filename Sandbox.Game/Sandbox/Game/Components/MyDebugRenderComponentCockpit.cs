// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentCockpit
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
  internal class MyDebugRenderComponentCockpit : MyDebugRenderComponent
  {
    private MyCockpit m_cockpit;

    public MyDebugRenderComponentCockpit(MyCockpit cockpit)
      : base((IMyEntity) cockpit)
      => this.m_cockpit = cockpit;

    public override void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_COCKPIT)
        return;
      if (this.m_cockpit.AiPilot != null)
        this.m_cockpit.AiPilot.DebugDraw();
      MyRenderProxy.DebugDrawText3D(this.m_cockpit.PositionComp.WorldMatrixRef.Translation, this.m_cockpit.IsShooting() ? "PEW!" : "", Color.Red, 2f, false);
      if (this.m_cockpit.Pilot == null)
        return;
      foreach (Vector3I neighbourPosition in this.m_cockpit.NeighbourPositions)
      {
        Vector3D translation;
        if (this.m_cockpit.IsNeighbourPositionFree(neighbourPosition, out translation))
          MyRenderProxy.DebugDrawSphere(translation, 0.3f, Color.Green, depthRead: false);
        else
          MyRenderProxy.DebugDrawSphere(translation, 0.3f, Color.Red, depthRead: false);
      }
    }
  }
}
