// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentMotorBase
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
  internal class MyDebugRenderComponentMotorBase : MyDebugRenderComponent
  {
    private MyMotorBase m_motor;

    public MyDebugRenderComponentMotorBase(MyMotorBase motor)
      : base((IMyEntity) motor)
      => this.m_motor = motor;

    public override void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_ROTORS)
        return;
      Vector3D pos;
      Vector3 halfExtents;
      Quaternion orientation;
      this.m_motor.ComputeTopQueryBox(out pos, out halfExtents, out orientation);
      MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(pos, (Vector3D) halfExtents, orientation), (Color) Color.Green.ToVector3(), 1f, false, false);
      if (this.m_motor.Rotor == null)
        return;
      MyRenderProxy.DebugDrawSphere(Vector3D.Transform(this.m_motor.DummyPosition, this.m_motor.CubeGrid.WorldMatrix) + (Vector3D.Transform((this.m_motor.Rotor as MyMotorRotor).WheelDummy, this.m_motor.RotorGrid.WorldMatrix) - this.m_motor.RotorGrid.WorldMatrix.Translation), 0.1f, Color.Green, depthRead: false);
      BoundingSphere boundingSphere = this.m_motor.Rotor.Model.BoundingSphere;
      boundingSphere.Center = (Vector3) Vector3D.Transform(boundingSphere.Center, this.m_motor.Rotor.WorldMatrix);
      MyRenderProxy.DebugDrawSphere((Vector3D) boundingSphere.Center, boundingSphere.Radius, Color.Red, depthRead: false);
    }
  }
}
