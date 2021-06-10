// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.EntityComponents.DebugRenders.MyDebugRenderComponentGravityGeneratorSphere
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.EntityComponents.DebugRenders
{
  public class MyDebugRenderComponentGravityGeneratorSphere : MyDebugRenderComponent
  {
    private MyGravityGeneratorSphere m_gravityGenerator;

    public MyDebugRenderComponentGravityGeneratorSphere(MyGravityGeneratorSphere gravityGenerator)
      : base((IMyEntity) gravityGenerator)
      => this.m_gravityGenerator = gravityGenerator;

    public override void DebugDraw()
    {
      if (MyDebugDrawSettings.DEBUG_DRAW_MISCELLANEOUS && this.m_gravityGenerator.IsWorking)
        MyRenderProxy.DebugDrawSphere(this.m_gravityGenerator.PositionComp.WorldMatrixRef.Translation, this.m_gravityGenerator.Radius, Color.CadetBlue, depthRead: false);
      if (!MyDebugDrawSettings.DEBUG_DRAW_MISCELLANEOUS)
        return;
      MyRenderProxy.DebugDrawAxis(this.m_gravityGenerator.PositionComp.WorldMatrixRef, 2f, false);
    }
  }
}
