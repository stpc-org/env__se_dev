// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentEngineerTool
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Weapons;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyDebugRenderComponentEngineerTool : MyDebugRenderComponent
  {
    private MyEngineerToolBase m_tool;

    public MyDebugRenderComponentEngineerTool(MyEngineerToolBase tool)
      : base((IMyEntity) tool)
      => this.m_tool = tool;

    public override void DebugDraw()
    {
      if (MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC && this.m_tool.GetTargetGrid() != null)
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 0.0f), this.m_tool.TargetCube.ToString(), Color.White, 1f);
      if (!MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC)
        return;
      MyRenderProxy.DebugDrawSphere(this.m_tool.GunBase.GetMuzzleWorldPosition(), 0.01f, Color.Green, depthRead: false);
    }
  }
}
