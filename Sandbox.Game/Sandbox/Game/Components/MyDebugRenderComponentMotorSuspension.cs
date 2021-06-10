// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentMotorSuspension
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using VRage.ModAPI;

namespace Sandbox.Game.Components
{
  internal class MyDebugRenderComponentMotorSuspension : MyDebugRenderComponent
  {
    private MyMotorSuspension m_motor;

    public MyDebugRenderComponentMotorSuspension(MyMotorSuspension motor)
      : base((IMyEntity) motor)
      => this.m_motor = motor;

    public override void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_CONSTRAINTS || (MySector.MainCamera.Position - this.m_motor.PositionComp.GetPosition()).LengthSquared() >= 10000.0)
        return;
      this.m_motor.DebugDrawConstraint();
    }
  }
}
