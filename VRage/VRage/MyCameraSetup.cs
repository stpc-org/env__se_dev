// Decompiled with JetBrains decompiler
// Type: VRage.MyCameraSetup
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;

namespace VRage
{
  public struct MyCameraSetup
  {
    public MatrixD ViewMatrix;
    public Vector3D Position;
    public float FarPlane;
    public float FOV;
    public float NearPlane;
    public Matrix ProjectionMatrix;
    public float ProjectionOffsetX;
    public float ProjectionOffsetY;
    public float AspectRatio;
  }
}
