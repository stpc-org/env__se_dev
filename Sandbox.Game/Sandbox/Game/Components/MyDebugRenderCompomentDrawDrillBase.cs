// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderCompomentDrawDrillBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Weapons;
using VRage.ModAPI;

namespace Sandbox.Game.Components
{
  public class MyDebugRenderCompomentDrawDrillBase : MyDebugRenderComponent
  {
    private MyDrillBase m_drillBase;

    public MyDebugRenderCompomentDrawDrillBase(MyDrillBase drillBase)
      : base((IMyEntity) null)
      => this.m_drillBase = drillBase;

    public override void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_DRILLS)
        return;
      this.m_drillBase.DebugDraw();
    }
  }
}
