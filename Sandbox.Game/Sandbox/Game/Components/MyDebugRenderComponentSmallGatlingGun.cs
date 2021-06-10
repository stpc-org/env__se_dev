// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentSmallGatlingGun
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.EntityComponents;
using Sandbox.Game.Weapons;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.Components
{
  internal class MyDebugRenderComponentSmallGatlingGun : MyDebugRenderComponent
  {
    private MySmallGatlingGun m_gatlingGun;

    public MyDebugRenderComponentSmallGatlingGun(MySmallGatlingGun gatlingGun)
      : base((IMyEntity) gatlingGun)
      => this.m_gatlingGun = gatlingGun;

    public override void DebugDraw()
    {
      this.m_gatlingGun.ConveyorEndpoint.DebugDraw();
      this.m_gatlingGun.Components.Get<MyResourceSinkComponent>()?.DebugDraw((Matrix) ref this.m_gatlingGun.PositionComp.WorldMatrixRef);
    }
  }
}
