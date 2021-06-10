// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderCompoonentShipConnector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyDebugRenderCompoonentShipConnector : MyDebugRenderComponent
  {
    private MyShipConnector m_shipConnector;

    public MyDebugRenderCompoonentShipConnector(MyShipConnector shipConnector)
      : base((IMyEntity) shipConnector)
      => this.m_shipConnector = shipConnector;

    public override void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_CONNECTORS_AND_MERGE_BLOCKS)
        return;
      MyRenderProxy.DebugDrawSphere(this.m_shipConnector.ConstraintPositionWorld(), 0.05f, Color.Red, depthRead: false);
      MyRenderProxy.DebugDrawText3D(this.m_shipConnector.PositionComp.WorldMatrixRef.Translation, this.m_shipConnector.DetectedGridCount.ToString(), Color.Red, 1f, false);
    }
  }
}
