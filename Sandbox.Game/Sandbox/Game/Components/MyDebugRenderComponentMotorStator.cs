// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentMotorStator
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
  internal class MyDebugRenderComponentMotorStator : MyDebugRenderComponent
  {
    private MyMotorStator m_motor;

    public MyDebugRenderComponentMotorStator(MyMotorStator motor)
      : base((IMyEntity) motor)
      => this.m_motor = motor;

    public override void DebugDraw()
    {
      if (!this.m_motor.CanDebugDraw() || !MyDebugDrawSettings.DEBUG_DRAW_ROTORS)
        return;
      MatrixD matrixD = this.m_motor.PositionComp.WorldMatrixRef;
      MatrixD worldMatrix = this.m_motor.Rotor.WorldMatrix;
      Vector3 vector3_1 = Vector3.Lerp((Vector3) matrixD.Translation, (Vector3) worldMatrix.Translation, 0.5f);
      Vector3 vector3_2 = Vector3.Normalize(matrixD.Up);
      MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_1, (Vector3D) (vector3_1 + vector3_2), Color.Yellow, Color.Yellow, false);
      MyRenderProxy.DebugDrawLine3D(matrixD.Translation, worldMatrix.Translation, Color.Red, Color.Green, false);
    }
  }
}
