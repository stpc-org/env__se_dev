// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyMeshHelper
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRageMath;

namespace VRage.Game
{
  public class MyMeshHelper
  {
    private static readonly int C_BUFFER_CAPACITY = 5000;
    private static List<Vector3D> m_tmpVectorBuffer = new List<Vector3D>(MyMeshHelper.C_BUFFER_CAPACITY);

    public static void GenerateSphere(
      ref MatrixD worldMatrix,
      float radius,
      int steps,
      List<Vector3D> vertices)
    {
      MyMeshHelper.m_tmpVectorBuffer.Clear();
      int num1 = 0;
      float num2 = (float) (360 / steps);
      float num3 = 90f - num2;
      float num4 = 360f - num2;
      for (float degrees1 = 0.0f; (double) degrees1 <= (double) num3; degrees1 += num2)
      {
        for (float degrees2 = 0.0f; (double) degrees2 <= (double) num4; degrees2 += num2)
        {
          Vector3D vector3D;
          vector3D.X = (double) radius * Math.Sin((double) MathHelper.ToRadians(degrees2)) * Math.Sin((double) MathHelper.ToRadians(degrees1));
          vector3D.Y = (double) radius * Math.Cos((double) MathHelper.ToRadians(degrees2)) * Math.Sin((double) MathHelper.ToRadians(degrees1));
          vector3D.Z = (double) (radius * (float) Math.Cos((double) MathHelper.ToRadians(degrees1)));
          MyMeshHelper.m_tmpVectorBuffer.Add(vector3D);
          int num5 = num1 + 1;
          vector3D.X = (double) radius * Math.Sin((double) MathHelper.ToRadians(degrees2)) * Math.Sin((double) MathHelper.ToRadians(degrees1 + num2));
          vector3D.Y = (double) radius * Math.Cos((double) MathHelper.ToRadians(degrees2)) * Math.Sin((double) MathHelper.ToRadians(degrees1 + num2));
          vector3D.Z = (double) (radius * (float) Math.Cos((double) MathHelper.ToRadians(degrees1 + num2)));
          MyMeshHelper.m_tmpVectorBuffer.Add(vector3D);
          int num6 = num5 + 1;
          vector3D.X = (double) radius * Math.Sin((double) MathHelper.ToRadians(degrees2 + num2)) * Math.Sin((double) MathHelper.ToRadians(degrees1));
          vector3D.Y = (double) radius * Math.Cos((double) MathHelper.ToRadians(degrees2 + num2)) * Math.Sin((double) MathHelper.ToRadians(degrees1));
          vector3D.Z = (double) (radius * (float) Math.Cos((double) MathHelper.ToRadians(degrees1)));
          MyMeshHelper.m_tmpVectorBuffer.Add(vector3D);
          int num7 = num6 + 1;
          vector3D.X = (double) radius * Math.Sin((double) MathHelper.ToRadians(degrees2 + num2)) * Math.Sin((double) MathHelper.ToRadians(degrees1 + num2));
          vector3D.Y = (double) radius * Math.Cos((double) MathHelper.ToRadians(degrees2 + num2)) * Math.Sin((double) MathHelper.ToRadians(degrees1 + num2));
          vector3D.Z = (double) (radius * (float) Math.Cos((double) MathHelper.ToRadians(degrees1 + num2)));
          MyMeshHelper.m_tmpVectorBuffer.Add(vector3D);
          num1 = num7 + 1;
        }
      }
      int count = MyMeshHelper.m_tmpVectorBuffer.Count;
      foreach (Vector3D vector3D in MyMeshHelper.m_tmpVectorBuffer)
        vertices.Add(vector3D);
      foreach (Vector3D vector3D1 in MyMeshHelper.m_tmpVectorBuffer)
      {
        Vector3D vector3D2 = new Vector3D(vector3D1.X, vector3D1.Y, -vector3D1.Z);
        vertices.Add(vector3D2);
      }
      for (int index = 0; index < vertices.Count; ++index)
        vertices[index] = Vector3D.Transform(vertices[index], worldMatrix);
    }
  }
}
