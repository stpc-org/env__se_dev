// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.MyDestinationSphere
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
  public class MyDestinationSphere : IMyDestinationShape
  {
    private float m_radius;
    private Vector3D m_center;
    private Vector3D m_relativeCenter;

    public MyDestinationSphere(ref Vector3D worldCenter, float radius) => this.Init(ref worldCenter, radius);

    public void Init(ref Vector3D worldCenter, float radius)
    {
      this.m_radius = radius;
      this.m_center = worldCenter;
    }

    public void SetRelativeTransform(MatrixD invWorldTransform) => Vector3D.Transform(ref this.m_center, ref invWorldTransform, out this.m_relativeCenter);

    public void UpdateWorldTransform(MatrixD worldTransform) => Vector3D.Transform(ref this.m_relativeCenter, ref worldTransform, out this.m_center);

    public float PointAdmissibility(Vector3D position, float tolerance)
    {
      float num = (float) Vector3D.Distance(position, this.m_center);
      return (double) num <= (double) this.m_radius + (double) tolerance ? num : float.PositiveInfinity;
    }

    public Vector3D GetClosestPoint(Vector3D queryPoint)
    {
      Vector3D vector3D = queryPoint - this.m_center;
      double num = vector3D.Length();
      return num < (double) this.m_radius ? queryPoint : this.m_center + vector3D / num * (double) this.m_radius;
    }

    public Vector3D GetBestPoint(Vector3D queryPoint) => this.m_center;

    public Vector3D GetDestination() => this.m_center;

    public void DebugDraw()
    {
      MyRenderProxy.DebugDrawSphere(this.m_center, Math.Max(this.m_radius, 0.05f), Color.Pink, depthRead: false);
      MyRenderProxy.DebugDrawSphere(this.m_center, this.m_radius, Color.Pink, depthRead: false);
      MyRenderProxy.DebugDrawText3D(this.m_center, "Destination", Color.Pink, 1f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
    }
  }
}
