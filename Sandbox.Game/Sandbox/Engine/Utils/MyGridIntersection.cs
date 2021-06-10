// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyGridIntersection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using BulletXNA;
using System;
using System.Collections.Generic;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Utils
{
  public class MyGridIntersection
  {
    private static bool IsPointInside(Vector3 p, Vector3I min, Vector3I max) => (double) p.X >= (double) min.X && (double) p.X < (double) (max.X + 1) && ((double) p.Y >= (double) min.Y && (double) p.Y < (double) (max.Y + 1)) && (double) p.Z >= (double) min.Z && (double) p.Z < (double) (max.Z + 1);

    private static bool IntersectionT(double n, double d, ref double tE, ref double tL)
    {
      if (MyUtils.IsZero(d))
        return n <= 0.0;
      double num = n / d;
      if (d > 0.0)
      {
        if (num > tL)
          return false;
        if (num > tE)
          tE = num;
      }
      else
      {
        if (num < tE)
          return false;
        if (num < tL)
          tL = num;
      }
      return true;
    }

    private static bool ClipLine(ref Vector3D start, ref Vector3D end, Vector3I min, Vector3I max)
    {
      Vector3D vector3D = end - start;
      if (MyUtils.IsZero(vector3D))
        return MyGridIntersection.IsPointInside((Vector3) start, min, max);
      double tE = 0.0;
      double tL = 1.0;
      if (!MyGridIntersection.IntersectionT((double) min.X - start.X, vector3D.X, ref tE, ref tL) || !MyGridIntersection.IntersectionT(start.X - (double) max.X - 1.0, -vector3D.X, ref tE, ref tL) || (!MyGridIntersection.IntersectionT((double) min.Y - start.Y, vector3D.Y, ref tE, ref tL) || !MyGridIntersection.IntersectionT(start.Y - (double) max.Y - 1.0, -vector3D.Y, ref tE, ref tL)) || (!MyGridIntersection.IntersectionT((double) min.Z - start.Z, vector3D.Z, ref tE, ref tL) || !MyGridIntersection.IntersectionT(start.Z - (double) max.Z - 1.0, -vector3D.Z, ref tE, ref tL)))
        return false;
      if (tL < 1.0)
        end = start + tL * vector3D;
      if (tE > 0.0)
        start += tE * vector3D;
      return true;
    }

    private static Vector3I SignInt(Vector3 v) => new Vector3I((double) v.X >= 0.0 ? 1 : -1, (double) v.Y >= 0.0 ? 1 : -1, (double) v.Z >= 0.0 ? 1 : -1);

    private static Vector3 Sign(Vector3 v) => new Vector3((double) v.X >= 0.0 ? 1f : -1f, (double) v.Y >= 0.0 ? 1f : -1f, (double) v.Z >= 0.0 ? 1f : -1f);

    private static Vector3I GetGridPoint(ref Vector3D v, Vector3I min, Vector3I max)
    {
      Vector3I vector3I = new Vector3I();
      if (v.X < (double) min.X)
        v.X = (double) (vector3I.X = min.X);
      else if (v.X >= (double) (max.X + 1))
      {
        v.X = (double) MathUtil.NextAfter((float) (max.X + 1), float.NegativeInfinity);
        vector3I.X = max.X;
      }
      else
        vector3I.X = (int) Math.Floor(v.X);
      if (v.Y < (double) min.Y)
        v.Y = (double) (vector3I.Y = min.Y);
      else if (v.Y >= (double) (max.Y + 1))
      {
        v.Y = (double) MathUtil.NextAfter((float) (max.Y + 1), float.NegativeInfinity);
        vector3I.Y = max.Y;
      }
      else
        vector3I.Y = (int) Math.Floor(v.Y);
      if (v.Z < (double) min.Z)
        v.Z = (double) (vector3I.Z = min.Z);
      else if (v.Z >= (double) (max.Z + 1))
      {
        v.Z = (double) MathUtil.NextAfter((float) (max.Z + 1), float.NegativeInfinity);
        vector3I.Z = max.Z;
      }
      else
        vector3I.Z = (int) Math.Floor(v.Z);
      return vector3I;
    }

    public static void CalculateHavok(
      List<Vector3I> result,
      float gridSize,
      Vector3D lineStart,
      Vector3D lineEnd,
      Vector3I min,
      Vector3I max)
    {
      Vector3D vector3D1 = Vector3D.Normalize(lineEnd - lineStart);
      Vector3D vector2 = Vector3D.Normalize(Vector3D.CalculatePerpendicularVector(vector3D1)) * 0.0599999986588955;
      Vector3D vector3D2 = Vector3D.Normalize(Vector3D.Cross(vector3D1, vector2)) * 0.06;
      MyGridIntersection.Calculate(result, gridSize, lineStart + vector2, lineEnd + vector2, min, max);
      MyGridIntersection.Calculate(result, gridSize, lineStart - vector2, lineEnd - vector2, min, max);
      MyGridIntersection.Calculate(result, gridSize, lineStart + vector3D2, lineEnd + vector3D2, min, max);
      MyGridIntersection.Calculate(result, gridSize, lineStart - vector3D2, lineEnd - vector3D2, min, max);
    }

    public static void Calculate(
      List<Vector3I> result,
      float gridSize,
      Vector3D lineStart,
      Vector3D lineEnd,
      Vector3I min,
      Vector3I max)
    {
      Vector3D vector3D1 = lineEnd - lineStart;
      Vector3D vector3D2 = lineStart / (double) gridSize;
      if (MyUtils.IsZero(vector3D1))
      {
        if (!MyGridIntersection.IsPointInside((Vector3) vector3D2, min, max))
          return;
        result.Add(MyGridIntersection.GetGridPoint(ref vector3D2, min, max));
      }
      else
      {
        Vector3D vector3D3 = lineEnd / (double) gridSize;
        if (!MyGridIntersection.ClipLine(ref vector3D2, ref vector3D3, min, max))
          return;
        Vector3 vector3 = MyGridIntersection.Sign((Vector3) vector3D1);
        Vector3I vector3I1 = MyGridIntersection.SignInt((Vector3) vector3D1);
        Vector3I vector3I2 = MyGridIntersection.GetGridPoint(ref vector3D2, min, max) * vector3I1;
        Vector3I vector3I3 = MyGridIntersection.GetGridPoint(ref vector3D3, min, max) * vector3I1;
        Vector3D vector3D4 = vector3D1 * vector3;
        vector3D2 *= vector3;
        double num1 = 1.0 / vector3D4.X;
        double num2 = num1 * (Math.Floor(vector3D2.X + 1.0) - vector3D2.X);
        double num3 = 1.0 / vector3D4.Y;
        double num4 = num3 * (Math.Floor(vector3D2.Y + 1.0) - vector3D2.Y);
        double num5 = 1.0 / vector3D4.Z;
        double num6 = num5 * (Math.Floor(vector3D2.Z + 1.0) - vector3D2.Z);
        do
        {
          result.Add(vector3I2 * vector3I1);
          if (num2 < num6)
          {
            if (num2 < num4)
            {
              num2 += num1;
              if (++vector3I2.X > vector3I3.X)
                break;
            }
            else
            {
              num4 += num3;
              if (++vector3I2.Y > vector3I3.Y)
                break;
            }
          }
          else if (num6 < num4)
          {
            num6 += num5;
            if (++vector3I2.Z > vector3I3.Z)
              break;
          }
          else
            num4 += num3;
        }
        while (++vector3I2.Y <= vector3I3.Y);
      }
    }
  }
}
