// Decompiled with JetBrains decompiler
// Type: VRageMath.Vector3Extensions
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

namespace VRageMath
{
  public static class Vector3Extensions
  {
    public static Vector3 Project(this Vector3 projectedOntoVector, Vector3 projectedVector)
    {
      float num = projectedOntoVector.LengthSquared();
      return (double) num == 0.0 ? Vector3.Zero : Vector3.Dot(projectedVector, projectedOntoVector) / num * projectedOntoVector;
    }
  }
}
