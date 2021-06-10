// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Navigation.MyCollisionDetectionSteering
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Physics;
using System.Collections.Generic;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Navigation
{
  public class MyCollisionDetectionSteering : MySteeringBase
  {
    private bool m_hitLeft;
    private bool m_hitRight;
    private float m_hitLeftFraction;
    private float m_hitRightFraction;

    public MyCollisionDetectionSteering(MyBotNavigation parent)
      : base(parent, 1f)
    {
    }

    public override string GetName() => "Collision detection steering";

    public override void AccumulateCorrection(ref Vector3 correction, ref float weight)
    {
      this.m_hitLeft = false;
      this.m_hitRight = false;
      MatrixD positionAndOrientation = this.Parent.PositionAndOrientation;
      Vector3 forwardVector = this.Parent.ForwardVector;
      Vector3 vector3 = Vector3.Cross((Vector3) positionAndOrientation.Up, forwardVector);
      List<MyPhysics.HitInfo> toList = new List<MyPhysics.HitInfo>();
      MyPhysics.CastRay(positionAndOrientation.Translation + positionAndOrientation.Up, positionAndOrientation.Translation + positionAndOrientation.Up + forwardVector * 0.1f + vector3 * 1.3f, toList);
      if (toList.Count > 0)
      {
        this.m_hitLeft = true;
        this.m_hitLeftFraction = toList[0].HkHitInfo.HitFraction;
      }
      toList.Clear();
      MyPhysics.CastRay(positionAndOrientation.Translation + positionAndOrientation.Up, positionAndOrientation.Translation + positionAndOrientation.Up + forwardVector * 0.1f - vector3 * 1.3f, toList);
      if (toList.Count > 0)
      {
        this.m_hitRight = true;
        this.m_hitRightFraction = toList[0].HkHitInfo.HitFraction;
      }
      toList.Clear();
      float num1 = (float) ((double) this.Weight * 0.00999999977648258 * (1.0 - (double) this.m_hitLeftFraction));
      float num2 = (float) ((double) this.Weight * 0.00999999977648258 * (1.0 - (double) this.m_hitRightFraction));
      if (this.m_hitLeft)
      {
        correction -= vector3 * num1;
        weight += num1;
      }
      if (this.m_hitRight)
      {
        correction += vector3 * num2;
        weight += num2;
      }
      if (!this.m_hitLeft || !this.m_hitRight)
        return;
      correction -= vector3;
      weight += num1;
    }

    public override void DebugDraw()
    {
      MatrixD positionAndOrientation = this.Parent.PositionAndOrientation;
      Vector3 forwardVector = this.Parent.ForwardVector;
      Vector3 vector3 = Vector3.Cross((Vector3) positionAndOrientation.Up, forwardVector);
      Color color1 = this.m_hitLeft ? Color.Orange : Color.Green;
      MyRenderProxy.DebugDrawLine3D(positionAndOrientation.Translation + positionAndOrientation.Up, positionAndOrientation.Translation + positionAndOrientation.Up + forwardVector * 0.1f + vector3 * 1.3f, color1, color1, true);
      MyRenderProxy.DebugDrawText3D(positionAndOrientation.Translation + positionAndOrientation.Up * 3.0, "Hit LT: " + this.m_hitLeftFraction.ToString(), color1, 0.7f, false);
      Color color2 = this.m_hitRight ? Color.Orange : Color.Green;
      MyRenderProxy.DebugDrawLine3D(positionAndOrientation.Translation + positionAndOrientation.Up, positionAndOrientation.Translation + positionAndOrientation.Up + forwardVector * 0.1f - vector3 * 1.3f, color2, color2, true);
      MyRenderProxy.DebugDrawText3D(positionAndOrientation.Translation + positionAndOrientation.Up * 3.20000004768372, "Hit RT: " + this.m_hitRightFraction.ToString(), color2, 0.7f, false);
    }
  }
}
