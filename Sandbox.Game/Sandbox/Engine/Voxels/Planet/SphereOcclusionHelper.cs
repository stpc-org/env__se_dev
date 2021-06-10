// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.Planet.SphereOcclusionHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels.Planet
{
  public class SphereOcclusionHelper
  {
    private float m_minRadius;
    private float m_maxRadius;
    private float m_baseRadius;
    private Vector3 m_lastUpdatePosition;

    public float OcclusionRange { get; private set; }

    public float OcclusionAngleCosine { get; private set; }

    public float OcclusionDistance { get; private set; }

    public SphereOcclusionHelper(float minRadius, float maxRadius)
    {
      this.m_minRadius = minRadius;
      this.m_maxRadius = maxRadius;
    }

    public void CalculateOcclusion(Vector3 position)
    {
    }

    public void DebugDraw(MatrixD worldMatrix)
    {
      Vector3D translation1 = worldMatrix.Translation;
      MyRenderProxy.DebugDrawSphere(translation1, this.m_minRadius, Color.Red, 0.2f, smooth: true);
      MyRenderProxy.DebugDrawSphere(translation1, this.m_maxRadius, Color.Red, 0.2f, smooth: true);
      float num = this.m_lastUpdatePosition.Length();
      Vector3 vector3 = this.m_lastUpdatePosition / num;
      MyRenderProxy.DebugDrawLine3D(translation1, translation1 + this.OcclusionDistance * vector3, Color.Green, Color.Green, true);
      Vector3D translation2 = translation1 + vector3 * (num - this.OcclusionDistance);
      Vector3D vector3D = Vector3D.CalculatePerpendicularVector((Vector3D) vector3) * (double) this.m_baseRadius;
      Vector3D directionVec = (Vector3D) (vector3 * this.OcclusionDistance);
      Vector3D baseVec = vector3D;
      Color blue = Color.Blue;
      MyRenderProxy.DebugDrawCone(translation2, directionVec, baseVec, blue, true);
    }
  }
}
