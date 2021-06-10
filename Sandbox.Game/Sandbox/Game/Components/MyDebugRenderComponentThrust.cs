// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentThrust
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.RenderDirect.ActorComponents;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  internal class MyDebugRenderComponentThrust : MyDebugRenderComponent
  {
    private MyThrust m_thrust;

    public MyDebugRenderComponentThrust(MyThrust thrust)
      : base((IMyEntity) thrust)
      => this.m_thrust = thrust;

    public override void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_THRUSTER_DAMAGE)
        return;
      this.DebugDrawDamageArea();
    }

    private void DebugDrawDamageArea()
    {
      if ((double) this.m_thrust.CurrentStrength == 0.0 && !MyFakes.INACTIVE_THRUSTER_DMG)
        return;
      foreach (MyThrustFlameAnimator.FlameInfo flame in this.m_thrust.Flames)
      {
        MatrixD worldMatrix = this.m_thrust.WorldMatrix;
        LineD damageCapsuleLine = this.m_thrust.GetDamageCapsuleLine(flame, ref worldMatrix);
        MyRenderProxy.DebugDrawCapsule(damageCapsuleLine.From, damageCapsuleLine.To, flame.Radius * this.m_thrust.FlameDamageLengthScale, Color.Red, false);
      }
    }
  }
}
