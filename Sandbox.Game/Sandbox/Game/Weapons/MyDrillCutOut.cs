// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyDrillCutOut
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.Weapons
{
  public class MyDrillCutOut
  {
    public readonly float CenterOffset;
    public readonly float Radius;
    protected BoundingSphereD m_sphere;

    public BoundingSphereD Sphere => this.m_sphere;

    public MyDrillCutOut(float centerOffset, float radius)
    {
      this.CenterOffset = centerOffset;
      this.Radius = radius;
      this.m_sphere = new BoundingSphereD(Vector3D.Zero, (double) this.Radius);
    }

    public void UpdatePosition(ref MatrixD worldMatrix) => this.m_sphere.Center = worldMatrix.Translation + worldMatrix.Forward * (double) this.CenterOffset;
  }
}
