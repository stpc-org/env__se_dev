// Decompiled with JetBrains decompiler
// Type: VRageMath.BoundingFrustumExtensions
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;

namespace VRageMath
{
  public static class BoundingFrustumExtensions
  {
    public static BoundingSphere ToBoundingSphere(
      this BoundingFrustum frustum,
      Vector3[] corners)
    {
      if (corners.Length < 8)
        throw new ArgumentException("Corners length must be at least 8");
      frustum.GetCorners(corners);
      Vector3 corner1;
      Vector3 vector3_1 = corner1 = corners[0];
      Vector3 vector3_2 = corner1;
      Vector3 vector3_3 = corner1;
      Vector3 vector3_4 = corner1;
      Vector3 vector3_5 = corner1;
      Vector3 vector3_6 = corner1;
      for (int index = 0; index < corners.Length; ++index)
      {
        Vector3 corner2 = corners[index];
        if ((double) corner2.X < (double) vector3_6.X)
          vector3_6 = corner2;
        if ((double) corner2.X > (double) vector3_5.X)
          vector3_5 = corner2;
        if ((double) corner2.Y < (double) vector3_4.Y)
          vector3_4 = corner2;
        if ((double) corner2.Y > (double) vector3_3.Y)
          vector3_3 = corner2;
        if ((double) corner2.Z < (double) vector3_2.Z)
          vector3_2 = corner2;
        if ((double) corner2.Z > (double) vector3_1.Z)
          vector3_1 = corner2;
      }
      float result1;
      Vector3.Distance(ref vector3_5, ref vector3_6, out result1);
      float result2;
      Vector3.Distance(ref vector3_3, ref vector3_4, out result2);
      float result3;
      Vector3.Distance(ref vector3_1, ref vector3_2, out result3);
      Vector3 result4;
      float num1;
      if ((double) result1 > (double) result2)
      {
        if ((double) result1 > (double) result3)
        {
          Vector3.Lerp(ref vector3_5, ref vector3_6, 0.5f, out result4);
          num1 = result1 * 0.5f;
        }
        else
        {
          Vector3.Lerp(ref vector3_1, ref vector3_2, 0.5f, out result4);
          num1 = result3 * 0.5f;
        }
      }
      else if ((double) result2 > (double) result3)
      {
        Vector3.Lerp(ref vector3_3, ref vector3_4, 0.5f, out result4);
        num1 = result2 * 0.5f;
      }
      else
      {
        Vector3.Lerp(ref vector3_1, ref vector3_2, 0.5f, out result4);
        num1 = result3 * 0.5f;
      }
      for (int index = 0; index < corners.Length; ++index)
      {
        Vector3 corner2 = corners[index];
        Vector3 vector3_7;
        vector3_7.X = corner2.X - result4.X;
        vector3_7.Y = corner2.Y - result4.Y;
        vector3_7.Z = corner2.Z - result4.Z;
        float num2 = vector3_7.Length();
        if ((double) num2 > (double) num1)
        {
          num1 = (float) (((double) num1 + (double) num2) * 0.5);
          result4 += (float) (1.0 - (double) num1 / (double) num2) * vector3_7;
        }
      }
      BoundingSphere boundingSphere;
      boundingSphere.Center = result4;
      boundingSphere.Radius = num1;
      return boundingSphere;
    }
  }
}
