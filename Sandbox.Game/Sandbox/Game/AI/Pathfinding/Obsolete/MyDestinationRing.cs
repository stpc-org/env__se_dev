// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyDestinationRing
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyDestinationRing : IMyDestinationShape
  {
    private float m_innerRadius;
    private float m_outerRadius;
    private Vector3D m_center;
    private Vector3D m_relativeCenter;

    public MyDestinationRing(ref Vector3D worldCenter, float innerRadius, float outerRadius) => this.Init(ref worldCenter, innerRadius, outerRadius);

    public void Init(ref Vector3D worldCenter, float innerRadius, float outerRadius)
    {
      this.m_center = worldCenter;
      this.m_innerRadius = innerRadius;
      this.m_outerRadius = outerRadius;
    }

    public void ReInit(ref Vector3D worldCenter) => this.m_center = worldCenter;

    public void SetRelativeTransform(MatrixD invWorldTransform) => Vector3D.Transform(ref this.m_center, ref invWorldTransform, out this.m_relativeCenter);

    public void UpdateWorldTransform(MatrixD worldTransform) => Vector3D.Transform(ref this.m_relativeCenter, ref worldTransform, out this.m_center);

    public float PointAdmissibility(Vector3D position, float tolerance)
    {
      float num = (float) Vector3D.Distance(position, this.m_center);
      return (double) num < (double) Math.Min(this.m_innerRadius - tolerance, 0.0f) || (double) num > (double) this.m_outerRadius + (double) tolerance ? float.PositiveInfinity : num;
    }

    public Vector3D GetClosestPoint(Vector3D queryPoint)
    {
      Vector3D vector3D = queryPoint - this.m_center;
      double num = vector3D.Length();
      if (num < (double) this.m_innerRadius)
        return this.m_center + vector3D / num * (double) this.m_innerRadius;
      return num > (double) this.m_outerRadius ? this.m_center + vector3D / num * (double) this.m_outerRadius : queryPoint;
    }

    public Vector3D GetBestPoint(Vector3D queryPoint) => this.m_center + Vector3D.Normalize(queryPoint - this.m_center) * (((double) this.m_innerRadius + (double) this.m_outerRadius) * 0.5);

    public Vector3D GetDestination() => this.m_center;

    public void DebugDraw()
    {
      MyRenderProxy.DebugDrawSphere(this.m_center, this.m_innerRadius, Color.RoyalBlue, 0.4f);
      MyRenderProxy.DebugDrawSphere(this.m_center, this.m_outerRadius, Color.Aqua, 0.4f);
    }
  }
}
