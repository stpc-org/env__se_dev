// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentLadder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.ModAPI;
using VRageRender;

namespace Sandbox.Game.Components
{
  public class MyDebugRenderComponentLadder : MyDebugRenderComponent
  {
    private IMyEntity m_ladder;

    public MyDebugRenderComponentLadder(IMyEntity ladder)
      : base(ladder)
      => this.m_ladder = ladder;

    public override void DebugDraw()
    {
      MyRenderProxy.DebugDrawAxis(this.m_ladder.PositionComp.WorldMatrixRef, 1f, false);
      base.DebugDraw();
    }
  }
}
