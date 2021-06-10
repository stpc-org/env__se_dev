// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.MyRandomLocationSphere
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.AI.Pathfinding;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI
{
  public class MyRandomLocationSphere : IMyDestinationShape
  {
    private Vector3D m_center;
    private Vector3D m_relativeCenter;
    private Vector3D m_desiredDirection;
    private float m_radius;

    public MyRandomLocationSphere(Vector3D worldCenter, float radius, Vector3D direction) => this.Init(ref worldCenter, radius, direction);

    public void Init(ref Vector3D worldCenter, float radius, Vector3D direction)
    {
      this.m_center = worldCenter;
      this.m_radius = radius;
      this.m_desiredDirection = direction;
    }

    public void SetRelativeTransform(MatrixD invWorldTransform) => Vector3D.Transform(ref this.m_center, ref invWorldTransform, out this.m_relativeCenter);

    public void UpdateWorldTransform(MatrixD worldTransform) => Vector3D.Transform(ref this.m_relativeCenter, ref worldTransform, out this.m_center);

    public float PointAdmissibility(Vector3D position, float tolerance)
    {
      Vector3D vector3D = position - this.m_center;
      float num = (float) vector3D.Normalize();
      return (double) num < (double) this.m_radius + (double) tolerance || vector3D.Dot(ref this.m_desiredDirection) < 0.9 ? float.PositiveInfinity : num;
    }

    public Vector3D GetClosestPoint(Vector3D queryPoint)
    {
      Vector3D v = queryPoint - this.m_center;
      if (v.Normalize() > (double) this.m_radius)
        return queryPoint;
      return this.m_desiredDirection.Dot(ref v) > 0.9 ? this.m_center + v * (double) this.m_radius : this.m_center + this.m_desiredDirection * (double) this.m_radius;
    }

    public Vector3D GetBestPoint(Vector3D queryPoint) => (queryPoint - this.m_center).Length() > (double) this.m_radius ? queryPoint : this.m_center + this.m_desiredDirection * (double) this.m_radius;

    public Vector3D GetDestination() => this.m_center + this.m_desiredDirection * (double) this.m_radius;

    public void DebugDraw()
    {
      MyRenderProxy.DebugDrawSphere(this.m_center, this.m_radius, Color.Gainsboro);
      MyRenderProxy.DebugDrawSphere(this.m_center + this.m_desiredDirection * (double) this.m_radius, 4f, Color.Aqua);
    }
  }
}
