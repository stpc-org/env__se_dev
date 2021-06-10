// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyUtils
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRageMath;

namespace VRage.Utils
{
  public static class MyUtils
  {
    private const int HashSeed = -2128831035;
    [ThreadStatic]
    private static Random m_secretRandom;
    private static byte[] m_randomBuffer = new byte[8];
    public const string C_CRLF = "\r\n";
    public static Tuple<string, float>[] DefaultNumberSuffix = new Tuple<string, float>[4]
    {
      new Tuple<string, float>("k", 1000f),
      new Tuple<string, float>("m", 1000000f),
      new Tuple<string, float>("g", 1E+09f),
      new Tuple<string, float>("b", 1E+09f)
    };
    public static readonly StringBuilder EmptyStringBuilder = new StringBuilder();
    public static readonly Matrix ZeroMatrix = new Matrix(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
    private static readonly string[] BYTE_SIZE_PREFIX = new string[5]
    {
      "",
      "K",
      "M",
      "G",
      "T"
    };

    public static void CreateFolder(string folderPath) => Directory.CreateDirectory(folderPath);

    public static void CopyDirectory(string source, string destination)
    {
      if (!Directory.Exists(source))
        return;
      if (!Directory.Exists(destination))
        Directory.CreateDirectory(destination);
      foreach (string file in Directory.GetFiles(source))
      {
        string fileName = Path.GetFileName(file);
        File.Copy(file, Path.Combine(destination, fileName), true);
      }
    }

    public static string StripInvalidChars(string filename) => ((IEnumerable<char>) Path.GetInvalidFileNameChars()).Aggregate<char, string>(filename, (Func<string, char, string>) ((current, c) => current.Replace(c.ToString(), string.Empty)));

    private static int HashStep(int value, int hash)
    {
      hash ^= value;
      hash *= 16777619;
      return hash;
    }

    public static unsafe int GetHash(double d, int hash = -2128831035)
    {
      if (d == 0.0)
        return hash;
      ulong num = (ulong) *(long*) &d;
      hash = MyUtils.HashStep((int) num, MyUtils.HashStep((int) (num >> 32), hash));
      return hash;
    }

    public static int GetHash(string str, int hash = -2128831035)
    {
      if (str != null)
      {
        int index;
        for (index = 0; index < str.Length - 1; index += 2)
          hash = MyUtils.HashStep(((int) str[index] << 16) + (int) str[index + 1], hash);
        if ((str.Length & 1) != 0)
          hash = MyUtils.HashStep((int) str[index], hash);
      }
      return hash;
    }

    public static int GetHash(string str, int start, int length, int hash = -2128831035)
    {
      if (str == null)
        return 0;
      if (length < 0)
        length = str.Length - start;
      if (length <= 0)
        return 0;
      int num = start + length - 1;
      int index;
      for (index = start; index < num; index += 2)
        hash = MyUtils.HashStep(((int) str[index] << 16) + (int) str[index + 1], hash);
      if ((length & 1) != 0)
        hash = MyUtils.HashStep((int) str[index], hash);
      return hash;
    }

    public static int GetHashUpperCase(string str, int start, int length, int hash = -2128831035)
    {
      if (str == null)
        return 0;
      if (length < 0)
        length = str.Length - start;
      if (length <= 0)
        return 0;
      int num = start + length - 1;
      int index;
      for (index = start; index < num; index += 2)
        hash = MyUtils.HashStep(((int) char.ToUpperInvariant(str[index]) << 16) + (int) char.ToUpperInvariant(str[index + 1]), hash);
      if ((length & 1) != 0)
        hash = MyUtils.HashStep((int) char.ToUpperInvariant(str[index]), hash);
      return hash;
    }

    public static void GetOpenBoundaries(
      Vector3[] vertices,
      int[] indices,
      List<Vector3> openBoundaries)
    {
      Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
      for (int index = 0; index < vertices.Length; ++index)
      {
        for (int key = 0; key < index; ++key)
        {
          if (MyUtils.IsEqual(vertices[key], vertices[index]))
          {
            if (!dictionary.ContainsKey(key))
              dictionary[key] = new List<int>();
            dictionary[key].Add(index);
            break;
          }
        }
      }
      foreach (KeyValuePair<int, List<int>> keyValuePair in dictionary)
      {
        foreach (int num in keyValuePair.Value)
        {
          for (int index = 0; index < indices.Length; ++index)
          {
            if (indices[index] == num)
              indices[index] = keyValuePair.Key;
          }
        }
      }
      Dictionary<MyUtils.Edge, int> edgeCounts = new Dictionary<MyUtils.Edge, int>();
      for (int index = 0; index < indices.Length; index += 3)
      {
        MyUtils.AddEdge(indices[index], indices[index + 1], edgeCounts);
        MyUtils.AddEdge(indices[index + 1], indices[index + 2], edgeCounts);
        MyUtils.AddEdge(indices[index + 2], indices[index], edgeCounts);
      }
      openBoundaries.Clear();
      foreach (KeyValuePair<MyUtils.Edge, int> keyValuePair in edgeCounts)
      {
        if (keyValuePair.Value == 1)
        {
          openBoundaries.Add(vertices[keyValuePair.Key.I0]);
          openBoundaries.Add(vertices[keyValuePair.Key.I1]);
        }
      }
    }

    private static void AddEdge(int i0, int i1, Dictionary<MyUtils.Edge, int> edgeCounts)
    {
      MyUtils.Edge key = new MyUtils.Edge()
      {
        I0 = i0,
        I1 = i1
      };
      key.GetHashCode();
      if (edgeCounts.ContainsKey(key))
        ++edgeCounts[key];
      else
        edgeCounts[key] = 1;
    }

    private static Random Instance
    {
      get
      {
        if (MyUtils.m_secretRandom == null)
          MyUtils.m_secretRandom = !MyRandom.EnableDeterminism ? new Random() : new Random(1);
        return MyUtils.m_secretRandom;
      }
    }

    public static int GetRandomInt(int maxValue) => MyUtils.Instance.Next(maxValue);

    public static int GetRandomInt(int minValue, int maxValue) => MyUtils.Instance.Next(Math.Min(minValue, maxValue), Math.Max(minValue, maxValue));

    public static Vector3 GetRandomVector3() => new Vector3(MyUtils.GetRandomFloat(-1f, 1f), MyUtils.GetRandomFloat(-1f, 1f), MyUtils.GetRandomFloat(-1f, 1f));

    public static Vector3D GetRandomVector3D() => new Vector3D(MyUtils.GetRandomDouble(-1.0, 1.0), MyUtils.GetRandomDouble(-1.0, 1.0), MyUtils.GetRandomDouble(-1.0, 1.0));

    public static Vector3 GetRandomPerpendicularVector(in Vector3 axis)
    {
      Vector3D axis1 = (Vector3D) axis;
      return (Vector3) MyUtils.GetRandomPerpendicularVector(ref axis1);
    }

    public static Vector3D GetRandomPerpendicularVector(ref Vector3D axis)
    {
      Vector3D perpendicularVector = Vector3D.CalculatePerpendicularVector(axis);
      Vector3D result;
      Vector3D.Cross(ref axis, ref perpendicularVector, out result);
      double randomDouble = MyUtils.GetRandomDouble(0.0, 6.2831859588623);
      return Math.Cos(randomDouble) * perpendicularVector + Math.Sin(randomDouble) * result;
    }

    public static Vector3D GetRandomDiscPosition(
      ref Vector3D center,
      double radius,
      ref Vector3D tangent,
      ref Vector3D bitangent)
    {
      double num = Math.Sqrt(MyUtils.GetRandomDouble(0.0, 1.0) * radius * radius);
      double randomDouble = MyUtils.GetRandomDouble(0.0, 6.2831859588623);
      return center + num * (Math.Cos(randomDouble) * tangent + Math.Sin(randomDouble) * bitangent);
    }

    public static Vector3D GetRandomDiscPosition(
      ref Vector3D center,
      double minRadius,
      double maxRadius,
      ref Vector3D tangent,
      ref Vector3D bitangent)
    {
      double num = Math.Sqrt(MyUtils.GetRandomDouble(minRadius * minRadius, maxRadius * maxRadius));
      double randomDouble = MyUtils.GetRandomDouble(0.0, 6.2831859588623);
      return center + num * (Math.Cos(randomDouble) * tangent + Math.Sin(randomDouble) * bitangent);
    }

    public static Vector3 GetRandomBorderPosition(ref BoundingSphere sphere) => sphere.Center + MyUtils.GetRandomVector3Normalized() * sphere.Radius;

    public static Vector3D GetRandomBorderPosition(ref BoundingSphereD sphere) => sphere.Center + MyUtils.GetRandomVector3Normalized() * (float) sphere.Radius;

    public static Vector3 GetRandomPosition(ref BoundingBox box) => box.Center + MyUtils.GetRandomVector3() * box.HalfExtents;

    public static Vector3D GetRandomPosition(ref BoundingBoxD box) => box.Center + (Vector3D) MyUtils.GetRandomVector3() * box.HalfExtents;

    public static Vector3 GetRandomBorderPosition(ref BoundingBox box)
    {
      BoundingBoxD box1 = (BoundingBoxD) box;
      return (Vector3) MyUtils.GetRandomBorderPosition(ref box1);
    }

    public static Vector3D GetRandomBorderPosition(ref BoundingBoxD box)
    {
      Vector3D size = box.Size;
      double num1 = 2.0 / box.SurfaceArea;
      double num2 = size.X * size.Y * num1;
      double num3 = size.X * size.Z * num1;
      double num4 = 1.0 - num2 - num3;
      double num5 = MyUtils.Instance.NextDouble();
      if (num5 < num2)
      {
        size.Z = num5 >= num2 * 0.5 ? box.Max.Z : box.Min.Z;
        size.X = MyUtils.GetRandomDouble(box.Min.X, box.Max.X);
        size.Y = MyUtils.GetRandomDouble(box.Min.Y, box.Max.Y);
        return size;
      }
      double num6 = num5 - num2;
      if (num6 < num3)
      {
        size.Y = num6 >= num3 * 0.5 ? box.Max.Y : box.Min.Y;
        size.X = MyUtils.GetRandomDouble(box.Min.X, box.Max.X);
        size.Z = MyUtils.GetRandomDouble(box.Min.Z, box.Max.Z);
        return size;
      }
      size.X = num6 - num4 >= num4 * 0.5 ? box.Max.X : box.Min.X;
      size.Y = MyUtils.GetRandomDouble(box.Min.Y, box.Max.Y);
      size.Z = MyUtils.GetRandomDouble(box.Min.Z, box.Max.Z);
      return size;
    }

    public static Vector3 GetRandomVector3Normalized()
    {
      float randomRadian = MyUtils.GetRandomRadian();
      float randomFloat = MyUtils.GetRandomFloat(-1f, 1f);
      float num = (float) Math.Sqrt(1.0 - (double) randomFloat * (double) randomFloat);
      return new Vector3((double) num * Math.Cos((double) randomRadian), (double) num * Math.Sin((double) randomRadian), (double) randomFloat);
    }

    public static Vector3 GetRandomVector3HemisphereNormalized(Vector3 normal)
    {
      Vector3 vector3Normalized = MyUtils.GetRandomVector3Normalized();
      return (double) Vector3.Dot(vector3Normalized, normal) < 0.0 ? -vector3Normalized : vector3Normalized;
    }

    public static Vector3 GetRandomVector3MaxAngle(float maxAngle)
    {
      float randomFloat1 = MyUtils.GetRandomFloat(-maxAngle, maxAngle);
      float randomFloat2 = MyUtils.GetRandomFloat(0.0f, 6.283185f);
      return -new Vector3(MyMath.FastSin(randomFloat1) * MyMath.FastCos(randomFloat2), MyMath.FastSin(randomFloat1) * MyMath.FastSin(randomFloat2), MyMath.FastCos(randomFloat1));
    }

    public static Vector3 GetRandomVector3CircleNormalized()
    {
      float randomRadian = MyUtils.GetRandomRadian();
      return new Vector3((float) Math.Sin((double) randomRadian), 0.0f, (float) Math.Cos((double) randomRadian));
    }

    public static float GetRandomSign() => (float) Math.Sign((float) MyUtils.Instance.NextDouble() - 0.5f);

    public static float GetRandomFloat() => (float) MyUtils.Instance.NextDouble();

    public static float GetRandomFloat(float minValue, float maxValue) => MyRandom.Instance.NextFloat() * (maxValue - minValue) + minValue;

    public static double GetRandomDouble(double minValue, double maxValue) => MyUtils.Instance.NextDouble() * (maxValue - minValue) + minValue;

    public static float GetRandomRadian() => MyUtils.GetRandomFloat(0.0f, 6.283186f);

    public static long GetRandomLong()
    {
      MyUtils.Instance.NextBytes(MyUtils.m_randomBuffer);
      return BitConverter.ToInt64(MyUtils.m_randomBuffer, 0);
    }

    public static TimeSpan GetRandomTimeSpan(TimeSpan begin, TimeSpan end)
    {
      long randomLong = MyUtils.GetRandomLong();
      return new TimeSpan(begin.Ticks + randomLong % (end.Ticks - begin.Ticks));
    }

    public static Vector3? GetEdgeSphereCollision(
      ref Vector3 sphereCenter,
      float sphereRadius,
      ref MyTriangle_Vertices triangle)
    {
      Vector3 closestPointOnLine1 = MyUtils.GetClosestPointOnLine(ref triangle.Vertex0, ref triangle.Vertex1, ref sphereCenter);
      if ((double) Vector3.Distance(closestPointOnLine1, sphereCenter) < (double) sphereRadius)
        return new Vector3?(closestPointOnLine1);
      Vector3 closestPointOnLine2 = MyUtils.GetClosestPointOnLine(ref triangle.Vertex1, ref triangle.Vertex2, ref sphereCenter);
      if ((double) Vector3.Distance(closestPointOnLine2, sphereCenter) < (double) sphereRadius)
        return new Vector3?(closestPointOnLine2);
      Vector3 closestPointOnLine3 = MyUtils.GetClosestPointOnLine(ref triangle.Vertex2, ref triangle.Vertex0, ref sphereCenter);
      return (double) Vector3.Distance(closestPointOnLine3, sphereCenter) < (double) sphereRadius ? new Vector3?(closestPointOnLine3) : new Vector3?();
    }

    public static bool GetInsidePolygonForSphereCollision(
      ref Vector3D point,
      ref MyTriangle_Vertices triangle)
    {
      return 0.0 + (double) MyUtils.GetAngleBetweenVectorsForSphereCollision(triangle.Vertex0 - (Vector3) point, triangle.Vertex1 - (Vector3) point) + (double) MyUtils.GetAngleBetweenVectorsForSphereCollision(triangle.Vertex1 - (Vector3) point, triangle.Vertex2 - (Vector3) point) + (double) MyUtils.GetAngleBetweenVectorsForSphereCollision(triangle.Vertex2 - (Vector3) point, triangle.Vertex0 - (Vector3) point) >= 6.22035415919481;
    }

    public static bool GetInsidePolygonForSphereCollision(
      ref Vector3 point,
      ref MyTriangle_Vertices triangle)
    {
      return 0.0 + (double) MyUtils.GetAngleBetweenVectorsForSphereCollision(triangle.Vertex0 - point, triangle.Vertex1 - point) + (double) MyUtils.GetAngleBetweenVectorsForSphereCollision(triangle.Vertex1 - point, triangle.Vertex2 - point) + (double) MyUtils.GetAngleBetweenVectorsForSphereCollision(triangle.Vertex2 - point, triangle.Vertex0 - point) >= 6.22035415919481;
    }

    public static float GetAngleBetweenVectorsForSphereCollision(Vector3 vector1, Vector3 vector2)
    {
      float f = (float) Math.Acos((double) Vector3.Dot(vector1, vector2) / (double) (vector1.Length() * vector2.Length()));
      return float.IsNaN(f) ? 0.0f : f;
    }

    public static float? GetLineTriangleIntersection(
      ref Line line,
      ref MyTriangle_Vertices triangle)
    {
      Vector3 result1;
      Vector3.Subtract(ref triangle.Vertex1, ref triangle.Vertex0, out result1);
      Vector3 result2;
      Vector3.Subtract(ref triangle.Vertex2, ref triangle.Vertex0, out result2);
      Vector3 result3;
      Vector3.Cross(ref line.Direction, ref result2, out result3);
      float result4;
      Vector3.Dot(ref result1, ref result3, out result4);
      if ((double) result4 > -1.40129846432482E-45 && (double) result4 < 1.40129846432482E-45)
        return new float?();
      float num1 = 1f / result4;
      Vector3 result5;
      Vector3.Subtract(ref line.From, ref triangle.Vertex0, out result5);
      float result6;
      Vector3.Dot(ref result5, ref result3, out result6);
      float num2 = result6 * num1;
      if ((double) num2 < 0.0 || (double) num2 > 1.0)
        return new float?();
      Vector3 result7;
      Vector3.Cross(ref result5, ref result1, out result7);
      float result8;
      Vector3.Dot(ref line.Direction, ref result7, out result8);
      float num3 = result8 * num1;
      if ((double) num3 < 0.0 || (double) num2 + (double) num3 > 1.0)
        return new float?();
      float result9;
      Vector3.Dot(ref result2, ref result7, out result9);
      float num4 = result9 * num1;
      if ((double) num4 < 0.0)
        return new float?();
      return (double) num4 > (double) line.Length ? new float?() : new float?(num4);
    }

    public static Vector3 GetNormalVectorFromTriangle(ref MyTriangle_Vertices inputTriangle) => Vector3.Normalize(Vector3.Cross(inputTriangle.Vertex2 - inputTriangle.Vertex0, inputTriangle.Vertex1 - inputTriangle.Vertex0));

    public static Vector3? GetSphereTriangleIntersection(
      ref BoundingSphere sphere,
      ref Plane trianglePlane,
      ref MyTriangle_Vertices triangle)
    {
      float distanceFromPlaneToSphere;
      if (MyUtils.GetSpherePlaneIntersection(ref sphere, ref trianglePlane, out distanceFromPlaneToSphere) == MySpherePlaneIntersectionEnum.INTERSECTS)
      {
        Vector3 vector3 = trianglePlane.Normal * distanceFromPlaneToSphere;
        Vector3 point;
        point.X = sphere.Center.X - vector3.X;
        point.Y = sphere.Center.Y - vector3.Y;
        point.Z = sphere.Center.Z - vector3.Z;
        if (MyUtils.GetInsidePolygonForSphereCollision(ref point, ref triangle))
          return new Vector3?(point);
        Vector3? edgeSphereCollision = MyUtils.GetEdgeSphereCollision(ref sphere.Center, sphere.Radius / 1f, ref triangle);
        if (edgeSphereCollision.HasValue)
          return new Vector3?(edgeSphereCollision.Value);
      }
      return new Vector3?();
    }

    public static string AlignIntToRight(int value, int charsCount, char ch)
    {
      string str = value.ToString();
      int length = str.Length;
      return length > charsCount ? str : new string(ch, charsCount - length) + str;
    }

    public static bool TryParseWithSuffix(
      this string text,
      NumberStyles numberStyle,
      IFormatProvider formatProvider,
      out float value,
      Tuple<string, float>[] suffix = null)
    {
      foreach (Tuple<string, float> tuple in suffix ?? MyUtils.DefaultNumberSuffix)
      {
        if (text.EndsWith(tuple.Item1, StringComparison.InvariantCultureIgnoreCase))
        {
          int num = float.TryParse(text.Substring(0, text.Length - tuple.Item1.Length), numberStyle, formatProvider, out value) ? 1 : 0;
          value *= tuple.Item2;
          return num != 0;
        }
      }
      return float.TryParse(text, out value);
    }

    public static Vector2 GetCoordAligned(
      Vector2 coordScreen,
      Vector2 size,
      MyGuiDrawAlignEnum drawAlign)
    {
      switch (drawAlign)
      {
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
          return coordScreen;
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
          return coordScreen - size * new Vector2(0.0f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
          return coordScreen - size * new Vector2(0.0f, 1f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
          return coordScreen - size * new Vector2(0.5f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
          return coordScreen - size * 0.5f;
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
          return coordScreen - size * new Vector2(0.5f, 1f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
          return coordScreen - size * new Vector2(1f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
          return coordScreen - size * new Vector2(1f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
          return coordScreen - size;
        default:
          throw new InvalidBranchException();
      }
    }

    public static Vector2 AlignCoord(
      Vector2 coordScreen,
      Vector2 size,
      MyGuiDrawAlignEnum drawAlignEnum)
    {
      switch (drawAlignEnum)
      {
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
          return coordScreen;
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
          return coordScreen + size * new Vector2(0.0f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
          return coordScreen + size * new Vector2(0.0f, 1f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
          return coordScreen + size * new Vector2(0.5f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
          return coordScreen + size * new Vector2(0.5f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
          return coordScreen + size * new Vector2(0.5f, 1f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
          return coordScreen + size * new Vector2(1f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
          return coordScreen + size * new Vector2(1f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
          return coordScreen + size * new Vector2(1f, 1f);
        default:
          throw new ArgumentOutOfRangeException(nameof (drawAlignEnum), (object) drawAlignEnum, (string) null);
      }
    }

    public static Vector2 GetCoordAlignedFromCenter(
      Vector2 coordCenter,
      Vector2 size,
      MyGuiDrawAlignEnum drawAlign)
    {
      switch (drawAlign)
      {
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
          return coordCenter + size * new Vector2(-0.5f, -0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
          return coordCenter + size * new Vector2(-0.5f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
          return coordCenter + size * new Vector2(-0.5f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
          return coordCenter + size * new Vector2(0.0f, -0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
          return coordCenter;
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
          return coordCenter + size * new Vector2(0.0f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
          return coordCenter + size * new Vector2(0.5f, -0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
          return coordCenter + size * new Vector2(0.5f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
          return coordCenter + size * new Vector2(0.5f, 0.5f);
        default:
          throw new InvalidBranchException();
      }
    }

    public static Vector2 GetCoordAlignedFromTopLeft(
      Vector2 topLeft,
      Vector2 size,
      MyGuiDrawAlignEnum drawAlign)
    {
      switch (drawAlign)
      {
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
          return topLeft;
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
          return topLeft + size * new Vector2(0.0f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
          return topLeft + size * new Vector2(0.0f, 1f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
          return topLeft + size * new Vector2(0.5f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
          return topLeft + size * new Vector2(0.5f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
          return topLeft + size * new Vector2(0.5f, 1f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
          return topLeft + size * new Vector2(1f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
          return topLeft + size * new Vector2(1f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
          return topLeft + size * new Vector2(1f, 1f);
        default:
          return topLeft;
      }
    }

    public static Vector2 GetCoordTopLeftFromAligned(
      Vector2 alignedCoord,
      Vector2 size,
      MyGuiDrawAlignEnum drawAlign)
    {
      switch (drawAlign)
      {
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
          return alignedCoord;
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
          return alignedCoord - size * new Vector2(0.0f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
          return alignedCoord - size * new Vector2(0.0f, 1f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
          return alignedCoord - size * new Vector2(0.5f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
          return alignedCoord - size * 0.5f;
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
          return alignedCoord - size * new Vector2(0.5f, 1f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
          return alignedCoord - size * new Vector2(1f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
          return alignedCoord - size * new Vector2(1f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
          return alignedCoord - size;
        default:
          throw new InvalidBranchException();
      }
    }

    public static Vector2I GetCoordTopLeftFromAligned(
      Vector2I alignedCoord,
      Vector2I size,
      MyGuiDrawAlignEnum drawAlign)
    {
      switch (drawAlign)
      {
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
          return alignedCoord;
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
          return new Vector2I(alignedCoord.X, alignedCoord.Y - size.Y / 2);
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
          return new Vector2I(alignedCoord.X, alignedCoord.Y - size.Y);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
          return new Vector2I(alignedCoord.X - size.X / 2, alignedCoord.Y);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
          return new Vector2I(alignedCoord.X - size.X / 2, alignedCoord.Y - size.Y / 2);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
          return new Vector2I(alignedCoord.X - size.X / 2, alignedCoord.Y - size.Y);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
          return new Vector2I(alignedCoord.X - size.X, alignedCoord.Y);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
          return new Vector2I(alignedCoord.X - size.X, alignedCoord.Y - size.Y / 2);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
          return new Vector2I(alignedCoord.X - size.X, alignedCoord.Y - size.Y);
        default:
          throw new InvalidBranchException();
      }
    }

    public static Vector2 GetCoordCenterFromAligned(
      Vector2 alignedCoord,
      Vector2 size,
      MyGuiDrawAlignEnum drawAlign)
    {
      switch (drawAlign)
      {
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
          return alignedCoord + size * 0.5f;
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
          return alignedCoord + size * new Vector2(0.5f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
          return alignedCoord + size * new Vector2(0.5f, -0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
          return alignedCoord + size * new Vector2(0.0f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
          return alignedCoord;
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
          return alignedCoord - size * new Vector2(0.0f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
          return alignedCoord + size * new Vector2(-0.5f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
          return alignedCoord - size * new Vector2(0.5f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
          return alignedCoord - size * 0.5f;
        default:
          throw new InvalidBranchException();
      }
    }

    public static Vector2 GetCoordAlignedFromRectangle(
      ref RectangleF rect,
      MyGuiDrawAlignEnum drawAlign)
    {
      switch (drawAlign)
      {
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP:
          return rect.Position;
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
          return rect.Position + rect.Size * new Vector2(0.0f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
          return rect.Position + rect.Size * new Vector2(0.0f, 1f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
          return rect.Position + rect.Size * new Vector2(0.5f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
          return rect.Position + rect.Size * 0.5f;
        case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
          return rect.Position + rect.Size * new Vector2(0.5f, 1f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
          return rect.Position + rect.Size * new Vector2(1f, 0.0f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
          return rect.Position + rect.Size * new Vector2(1f, 0.5f);
        case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
          return rect.Position + rect.Size * 1f;
        default:
          throw new InvalidBranchException();
      }
    }

    public static Thread MainThread { get; set; }

    [Conditional("DEBUG")]
    public static void AssertIsValid(Matrix matrix)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertIsValid(MatrixD matrix)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertIsValid(Vector3 vec)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertIsValid(Vector3D vec)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertIsValid(Vector3? vec)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertIsValid(Vector2 vec)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertIsValid(float f)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertIsValid(double f)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertIsValid(Quaternion q)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertIsValidOrZero(Matrix matrix)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertLengthValid(ref Vector3 vec)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertLengthValid(ref Vector3D vec)
    {
    }

    public static bool HasValidLength(Vector3 vec) => (double) vec.Length() > 9.99999997475243E-07;

    public static bool HasValidLength(Vector3D vec) => vec.Length() > 9.99999997475243E-07;

    public static bool IsEqual(float value1, float value2) => MyUtils.IsZero(value1 - value2, 1E-05f);

    public static bool IsEqual(Vector2 value1, Vector2 value2) => MyUtils.IsZero(value1.X - value2.X, 1E-05f) && MyUtils.IsZero(value1.Y - value2.Y, 1E-05f);

    public static bool IsEqual(Vector3 value1, Vector3 value2) => MyUtils.IsZero(value1.X - value2.X, 1E-05f) && MyUtils.IsZero(value1.Y - value2.Y, 1E-05f) && MyUtils.IsZero(value1.Z - value2.Z, 1E-05f);

    public static bool IsEqual(Quaternion value1, Quaternion value2) => MyUtils.IsZero(value1.X - value2.X, 1E-05f) && MyUtils.IsZero(value1.Y - value2.Y, 1E-05f) && MyUtils.IsZero(value1.Z - value2.Z, 1E-05f) && MyUtils.IsZero(value1.W - value2.W, 1E-05f);

    public static bool IsEqual(QuaternionD value1, QuaternionD value2) => MyUtils.IsZero(value1.X - value2.X) && MyUtils.IsZero(value1.Y - value2.Y) && MyUtils.IsZero(value1.Z - value2.Z) && MyUtils.IsZero(value1.W - value2.W);

    public static bool IsEqual(Matrix value1, Matrix value2) => MyUtils.IsZero(value1.Left - value2.Left) && MyUtils.IsZero(value1.Up - value2.Up) && MyUtils.IsZero(value1.Forward - value2.Forward) && MyUtils.IsZero(value1.Translation - value2.Translation);

    public static bool IsValid(Matrix matrix) => matrix.Up.IsValid() && matrix.Left.IsValid() && (matrix.Forward.IsValid() && matrix.Translation.IsValid()) && matrix != Matrix.Zero;

    public static bool IsValid(MatrixD matrix) => matrix.Up.IsValid() && matrix.Left.IsValid() && (matrix.Forward.IsValid() && matrix.Translation.IsValid()) && matrix != MatrixD.Zero;

    public static bool IsValid(Vector3 vec) => MyUtils.IsValid(vec.X) && MyUtils.IsValid(vec.Y) && MyUtils.IsValid(vec.Z);

    public static bool IsValid(Vector3D vec) => MyUtils.IsValid(vec.X) && MyUtils.IsValid(vec.Y) && MyUtils.IsValid(vec.Z);

    public static bool IsValid(Vector2 vec) => MyUtils.IsValid(vec.X) && MyUtils.IsValid(vec.Y);

    public static bool IsValid(float f) => !float.IsNaN(f) && !float.IsInfinity(f);

    public static bool IsValid(double f) => !double.IsNaN(f) && !double.IsInfinity(f);

    public static bool IsValid(Vector3? vec)
    {
      if (!vec.HasValue)
        return true;
      return MyUtils.IsValid(vec.Value.X) && MyUtils.IsValid(vec.Value.Y) && MyUtils.IsValid(vec.Value.Z);
    }

    public static bool IsValid(Quaternion q) => MyUtils.IsValid(q.X) && MyUtils.IsValid(q.Y) && (MyUtils.IsValid(q.Z) && MyUtils.IsValid(q.W)) && !MyUtils.IsZero(q);

    public static bool IsValidNormal(Vector3 vec)
    {
      float num = vec.LengthSquared();
      return vec.IsValid() && (double) num > 0.999000012874603 && (double) num < 1.00100004673004;
    }

    public static bool IsValidOrZero(Matrix matrix) => MyUtils.IsValid(matrix.Up) && MyUtils.IsValid(matrix.Left) && MyUtils.IsValid(matrix.Forward) && MyUtils.IsValid(matrix.Translation);

    public static bool IsZero(float value, float epsilon = 1E-05f) => (double) value > -(double) epsilon && (double) value < (double) epsilon;

    public static bool IsZero(double value, float epsilon = 1E-05f) => value > -(double) epsilon && value < (double) epsilon;

    public static bool IsZero(Vector3 value, float epsilon = 1E-05f) => MyUtils.IsZero(value.X, epsilon) && MyUtils.IsZero(value.Y, epsilon) && MyUtils.IsZero(value.Z, epsilon);

    public static bool IsZero(ref Vector3 value, float epsilon = 1E-05f) => MyUtils.IsZero(value.X, epsilon) && MyUtils.IsZero(value.Y, epsilon) && MyUtils.IsZero(value.Z, epsilon);

    public static bool IsZero(Vector3D value, float epsilon = 1E-05f) => MyUtils.IsZero(value.X, epsilon) && MyUtils.IsZero(value.Y, epsilon) && MyUtils.IsZero(value.Z, epsilon);

    public static bool IsZero(ref Vector3D value, float epsilon = 1E-05f) => MyUtils.IsZero(value.X, epsilon) && MyUtils.IsZero(value.Y, epsilon) && MyUtils.IsZero(value.Z, epsilon);

    public static bool IsZero(Quaternion value, float epsilon = 1E-05f) => MyUtils.IsZero(value.X, epsilon) && MyUtils.IsZero(value.Y, epsilon) && MyUtils.IsZero(value.Z, epsilon) && MyUtils.IsZero(value.W, epsilon);

    public static bool IsZero(ref Quaternion value, float epsilon = 1E-05f) => MyUtils.IsZero(value.X, epsilon) && MyUtils.IsZero(value.Y, epsilon) && MyUtils.IsZero(value.Z, epsilon) && MyUtils.IsZero(value.W, epsilon);

    public static bool IsZero(Vector4 value) => MyUtils.IsZero(value.X, 1E-05f) && MyUtils.IsZero(value.Y, 1E-05f) && MyUtils.IsZero(value.Z, 1E-05f) && MyUtils.IsZero(value.W, 1E-05f);

    public static void CheckFloatValues(
      object graph,
      string name,
      ref double? min,
      ref double? max)
    {
      int frameCount = new StackTrace().FrameCount;
      if (graph == null)
        return;
      if (graph is float f)
      {
        if (float.IsInfinity(f) || float.IsNaN(f))
          throw new InvalidOperationException("Invalid value: " + name);
        if (min.HasValue)
        {
          double num = (double) f;
          double? nullable = min;
          double valueOrDefault = nullable.GetValueOrDefault();
          if (!(num < valueOrDefault & nullable.HasValue))
            goto label_8;
        }
        min = new double?((double) f);
label_8:
        if (max.HasValue)
        {
          double num = (double) f;
          double? nullable = max;
          double valueOrDefault = nullable.GetValueOrDefault();
          if (!(num > valueOrDefault & nullable.HasValue))
            goto label_11;
        }
        max = new double?((double) f);
      }
label_11:
      if (graph is double d)
      {
        if (double.IsInfinity(d) || double.IsNaN(d))
          throw new InvalidOperationException("Invalid value: " + name);
        double? nullable;
        if (min.HasValue)
        {
          double num = d;
          nullable = min;
          double valueOrDefault = nullable.GetValueOrDefault();
          if (!(num < valueOrDefault & nullable.HasValue))
            goto label_17;
        }
        min = new double?(d);
label_17:
        if (max.HasValue)
        {
          double num = d;
          nullable = max;
          double valueOrDefault = nullable.GetValueOrDefault();
          if (!(num > valueOrDefault & nullable.HasValue))
            goto label_20;
        }
        max = new double?(d);
      }
label_20:
      if (graph.GetType().IsPrimitive)
        return;
      switch (graph)
      {
        case string _:
          break;
        case DateTime _:
          break;
        case IEnumerable _:
          IEnumerator enumerator = (graph as IEnumerable).GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
              MyUtils.CheckFloatValues(enumerator.Current, name + "[]", ref min, ref max);
            break;
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
        default:
          foreach (FieldInfo field in graph.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
            MyUtils.CheckFloatValues(field.GetValue(graph), name + "." + field.Name, ref min, ref max);
          foreach (PropertyInfo property in graph.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            MyUtils.CheckFloatValues(property.GetValue(graph, (object[]) null), name + "." + property.Name, ref min, ref max);
          break;
      }
    }

    public static void DeserializeValue(XmlReader reader, out Vector3 value) => value = new Vector3()
    {
      X = reader.ReadElementContentAsFloat(),
      Y = reader.ReadElementContentAsFloat(),
      Z = reader.ReadElementContentAsFloat()
    };

    public static void DeserializeValue(XmlReader reader, out Vector4 value) => value = new Vector4()
    {
      W = reader.ReadElementContentAsFloat(),
      X = reader.ReadElementContentAsFloat(),
      Y = reader.ReadElementContentAsFloat(),
      Z = reader.ReadElementContentAsFloat()
    };

    public static string FormatByteSizePrefix(ref double byteSize)
    {
      long num = 1;
      for (int index = 0; index < MyUtils.BYTE_SIZE_PREFIX.Length; ++index)
      {
        num *= 1024L;
        if (byteSize < (double) num)
        {
          byteSize /= (double) (num / 1024L);
          return MyUtils.BYTE_SIZE_PREFIX[index];
        }
      }
      return string.Empty;
    }

    public static Color[] GenerateBoxColors()
    {
      List<Color> list = new List<Color>();
      for (float amount1 = 0.0f; (double) amount1 < 1.0; amount1 += 0.2f)
      {
        for (float amount2 = 0.0f; (double) amount2 < 1.0; amount2 += 0.33f)
        {
          for (float amount3 = 0.0f; (double) amount3 < 1.0; amount3 += 0.33f)
          {
            float x = MathHelper.Lerp(0.5f, 0.5833333f, amount1);
            float y = MathHelper.Lerp(0.4f, 0.9f, amount2);
            float z = MathHelper.Lerp(0.4f, 1f, amount3);
            list.Add(new Vector3(x, y, z).HSVtoColor());
          }
        }
      }
      list.ShuffleList<Color>();
      return list.ToArray();
    }

    public static void GenerateQuad(
      out MyQuadD quad,
      ref Vector3D position,
      float width,
      float height,
      ref MatrixD matrix)
    {
      Vector3D vector3D1 = matrix.Left * (double) width;
      Vector3D vector3D2 = matrix.Up * (double) height;
      quad.Point0 = position + vector3D1 + vector3D2;
      quad.Point1 = position + vector3D1 - vector3D2;
      quad.Point2 = position - vector3D1 - vector3D2;
      quad.Point3 = position - vector3D1 + vector3D2;
    }

    public static float GetAngleBetweenVectors(Vector3 vectorA, Vector3 vectorB)
    {
      float num = Vector3.Dot(vectorA, vectorB);
      if ((double) num > 1.0 && (double) num <= 1.00010001659393)
        num = 1f;
      if ((double) num < -1.0 && (double) num >= -1.00010001659393)
        num = -1f;
      return (float) Math.Acos((double) num);
    }

    public static float GetAngleBetweenVectorsAndNormalise(Vector3 vectorA, Vector3 vectorB) => MyUtils.GetAngleBetweenVectors(Vector3.Normalize(vectorA), Vector3.Normalize(vectorB));

    public static bool GetBillboardQuadAdvancedRotated(
      out MyQuadD quad,
      Vector3D position,
      float radiusX,
      float radiusY,
      float angle,
      Vector3D cameraPosition)
    {
      Vector3D normalized1;
      normalized1.X = position.X - cameraPosition.X;
      normalized1.Y = position.Y - cameraPosition.Y;
      normalized1.Z = position.Z - cameraPosition.Z;
      if (normalized1.LengthSquared() <= 9.99999974737875E-06)
      {
        quad = new MyQuadD();
        return false;
      }
      MyUtils.Normalize(ref normalized1, out normalized1);
      Vector3D result1;
      Vector3D.Reject(ref Vector3D.Up, ref normalized1, out result1);
      Vector3D normalized2;
      if (result1.LengthSquared() <= 9.99999943962493E-11)
        normalized2 = Vector3D.Forward;
      else
        MyUtils.Normalize(ref result1, out normalized2);
      Vector3D result2;
      Vector3D.Cross(ref normalized2, ref normalized1, out result2);
      Vector3D.Normalize(ref result2, out result2);
      float num1 = (float) Math.Cos((double) angle);
      float num2 = (float) Math.Sin((double) angle);
      float num3 = radiusX * num1;
      float num4 = radiusX * num2;
      float num5 = radiusY * num1;
      float num6 = radiusY * num2;
      Vector3D vector3D1;
      vector3D1.X = (double) num3 * result2.X + (double) num6 * normalized2.X;
      vector3D1.Y = (double) num3 * result2.Y + (double) num6 * normalized2.Y;
      vector3D1.Z = (double) num3 * result2.Z + (double) num6 * normalized2.Z;
      Vector3D vector3D2;
      vector3D2.X = -(double) num4 * result2.X + (double) num5 * normalized2.X;
      vector3D2.Y = -(double) num4 * result2.Y + (double) num5 * normalized2.Y;
      vector3D2.Z = -(double) num4 * result2.Z + (double) num5 * normalized2.Z;
      quad.Point0.X = position.X + vector3D1.X + vector3D2.X;
      quad.Point0.Y = position.Y + vector3D1.Y + vector3D2.Y;
      quad.Point0.Z = position.Z + vector3D1.Z + vector3D2.Z;
      quad.Point1.X = position.X - vector3D1.X + vector3D2.X;
      quad.Point1.Y = position.Y - vector3D1.Y + vector3D2.Y;
      quad.Point1.Z = position.Z - vector3D1.Z + vector3D2.Z;
      quad.Point2.X = position.X - vector3D1.X - vector3D2.X;
      quad.Point2.Y = position.Y - vector3D1.Y - vector3D2.Y;
      quad.Point2.Z = position.Z - vector3D1.Z - vector3D2.Z;
      quad.Point3.X = position.X + vector3D1.X - vector3D2.X;
      quad.Point3.Y = position.Y + vector3D1.Y - vector3D2.Y;
      quad.Point3.Z = position.Z + vector3D1.Z - vector3D2.Z;
      return true;
    }

    public static bool GetBillboardQuadAdvancedRotated(
      out MyQuadD quad,
      Vector3D position,
      float radius,
      float angle,
      Vector3D cameraPosition)
    {
      return MyUtils.GetBillboardQuadAdvancedRotated(out quad, position, radius, radius, angle, cameraPosition);
    }

    public static void GetBillboardQuadOriented(
      out MyQuadD quad,
      ref Vector3D position,
      float width,
      float height,
      ref Vector3 leftVector,
      ref Vector3 upVector)
    {
      Vector3D vector3D1 = (Vector3D) (leftVector * width);
      Vector3D vector3D2 = (Vector3D) (upVector * height);
      quad.Point0 = position + vector3D2 + vector3D1;
      quad.Point1 = position + vector3D2 - vector3D1;
      quad.Point2 = position - vector3D2 - vector3D1;
      quad.Point3 = position - vector3D2 + vector3D1;
    }

    public static bool? GetBoolFromString(string s)
    {
      bool result;
      return !bool.TryParse(s, out result) ? new bool?() : new bool?(result);
    }

    public static bool GetBoolFromString(string s, bool defaultValue) => MyUtils.GetBoolFromString(s) ?? defaultValue;

    public static BoundingSphereD GetBoundingSphereFromBoundingBox(
      ref BoundingBoxD box)
    {
      BoundingSphereD boundingSphereD;
      boundingSphereD.Center = (box.Max + box.Min) / 2.0;
      boundingSphereD.Radius = Vector3D.Distance(boundingSphereD.Center, box.Max);
      return boundingSphereD;
    }

    public static byte? GetByteFromString(string s)
    {
      byte result;
      return !byte.TryParse(s, out result) ? new byte?() : new byte?(result);
    }

    public static Vector3 GetCartesianCoordinatesFromSpherical(
      float angleHorizontal,
      float angleVertical,
      float radius)
    {
      angleVertical = 1.570796f - angleVertical;
      angleHorizontal = 3.141593f - angleHorizontal;
      return new Vector3((float) ((double) radius * Math.Sin((double) angleVertical) * Math.Sin((double) angleHorizontal)), radius * (float) Math.Cos((double) angleVertical), (float) ((double) radius * Math.Sin((double) angleVertical) * Math.Cos((double) angleHorizontal)));
    }

    public static int GetClampInt(int value, int min, int max)
    {
      if (value < min)
        return min;
      return value > max ? max : value;
    }

    public static Vector3 GetClosestPointOnLine(
      ref Vector3 linePointA,
      ref Vector3 linePointB,
      ref Vector3 point)
    {
      float dist = 0.0f;
      return MyUtils.GetClosestPointOnLine(ref linePointA, ref linePointB, ref point, out dist);
    }

    public static Vector3D GetClosestPointOnLine(
      ref Vector3D linePointA,
      ref Vector3D linePointB,
      ref Vector3D point)
    {
      double dist = 0.0;
      return MyUtils.GetClosestPointOnLine(ref linePointA, ref linePointB, ref point, out dist);
    }

    public static Vector3 GetClosestPointOnLine(
      ref Vector3 linePointA,
      ref Vector3 linePointB,
      ref Vector3 point,
      out float dist)
    {
      Vector3 vector2 = point - linePointA;
      Vector3 vector1 = MyUtils.Normalize(linePointB - linePointA);
      float num1 = Vector3.Distance(linePointA, linePointB);
      float num2 = Vector3.Dot(vector1, vector2);
      dist = num2;
      if ((double) num2 <= 0.0)
        return linePointA;
      if ((double) num2 >= (double) num1)
        return linePointB;
      Vector3 vector3 = vector1 * num2;
      return linePointA + vector3;
    }

    public static Vector3D GetClosestPointOnLine(
      ref Vector3D linePointA,
      ref Vector3D linePointB,
      ref Vector3D point,
      out double dist)
    {
      Vector3D vector2 = point - linePointA;
      Vector3D vector1 = MyUtils.Normalize(linePointB - linePointA);
      double num1 = Vector3D.Distance(linePointA, linePointB);
      double num2 = Vector3D.Dot(vector1, vector2);
      dist = num2;
      if (num2 <= 0.0)
        return linePointA;
      if (num2 >= num1)
        return linePointB;
      Vector3D vector3D = vector1 * num2;
      return linePointA + vector3D;
    }

    public static float? GetFloatFromString(string s)
    {
      float result;
      return !float.TryParse(s, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result) ? new float?() : new float?(result);
    }

    public static float GetFloatFromString(string s, float defaultValue)
    {
      float? floatFromString = MyUtils.GetFloatFromString(s);
      return floatFromString.HasValue ? floatFromString.Value : defaultValue;
    }

    public static int? GetInt32FromString(string s)
    {
      int result;
      return int.TryParse(s, out result) ? new int?(result) : new int?();
    }

    public static int? GetIntFromString(string s)
    {
      int result;
      return !int.TryParse(s, out result) ? new int?() : new int?(result);
    }

    public static uint? GetUIntFromString(string s)
    {
      uint result;
      return !uint.TryParse(s, out result) ? new uint?() : new uint?(result);
    }

    public static int GetIntFromString(string s, int defaultValue)
    {
      int? intFromString = MyUtils.GetIntFromString(s);
      return intFromString.HasValue ? intFromString.Value : defaultValue;
    }

    public static uint GetUIntFromString(string s, uint defaultValue)
    {
      uint? uintFromString = MyUtils.GetUIntFromString(s);
      return uintFromString.HasValue ? uintFromString.Value : defaultValue;
    }

    public static double GetLargestDistanceToSphere(ref Vector3D from, ref BoundingSphereD sphere) => Vector3D.Distance(from, sphere.Center) + sphere.Radius;

    public static float? GetLineBoundingBoxIntersection(ref Line line, ref BoundingBox boundingBox)
    {
      Ray ray = new Ray(line.From, line.Direction);
      float? nullable = boundingBox.Intersects(ray);
      if (!nullable.HasValue)
        return new float?();
      return (double) nullable.Value <= (double) line.Length ? new float?(nullable.Value) : new float?();
    }

    public static int GetMaxValueFromEnum<T>()
    {
      Array values = Enum.GetValues(typeof (T));
      int num1 = int.MinValue;
      Type underlyingType = Enum.GetUnderlyingType(typeof (T));
      if (underlyingType == typeof (byte))
      {
        foreach (byte num2 in values)
        {
          if ((int) num2 > num1)
            num1 = (int) num2;
        }
      }
      else if (underlyingType == typeof (short))
      {
        foreach (short num2 in values)
        {
          if ((int) num2 > num1)
            num1 = (int) num2;
        }
      }
      else if (underlyingType == typeof (ushort))
      {
        foreach (ushort num2 in values)
        {
          if ((int) num2 > num1)
            num1 = (int) num2;
        }
      }
      else
      {
        if (!(underlyingType == typeof (int)))
          throw new InvalidBranchException();
        foreach (int num2 in values)
        {
          if (num2 > num1)
            num1 = num2;
        }
      }
      return num1;
    }

    public static double GetPointLineDistance(
      ref Vector3D linePointA,
      ref Vector3D linePointB,
      ref Vector3D point)
    {
      Vector3D vector1 = linePointB - linePointA;
      return Vector3D.Cross(vector1, point - linePointA).Length() / vector1.Length();
    }

    public static void GetPolyLineQuad(
      out MyQuadD retQuad,
      ref MyPolyLineD polyLine,
      Vector3D cameraPosition)
    {
      Vector3D vector2 = MyUtils.Normalize(cameraPosition - polyLine.Point0);
      Vector3D vector3Scaled = MyUtils.GetVector3Scaled(Vector3D.Cross((Vector3D) polyLine.LineDirectionNormalized, vector2), polyLine.Thickness);
      retQuad.Point0 = polyLine.Point0 - vector3Scaled;
      retQuad.Point1 = polyLine.Point1 - vector3Scaled;
      retQuad.Point2 = polyLine.Point1 + vector3Scaled;
      retQuad.Point3 = polyLine.Point0 + vector3Scaled;
    }

    public static T GetRandomItem<T>(this T[] list) => list[MyUtils.GetRandomInt(list.Length)];

    public static T GetRandomItemFromList<T>(this List<T> list) => list[MyUtils.GetRandomInt(list.Count)];

    public static double GetSmallestDistanceToSphere(ref Vector3D from, ref BoundingSphereD sphere) => Vector3D.Distance(from, sphere.Center) - sphere.Radius;

    public static double GetSmallestDistanceToSphereAlwaysPositive(
      ref Vector3D from,
      ref BoundingSphereD sphere)
    {
      double num = MyUtils.GetSmallestDistanceToSphere(ref from, ref sphere);
      if (num < 0.0)
        num = 0.0;
      return num;
    }

    public static MySpherePlaneIntersectionEnum GetSpherePlaneIntersection(
      ref BoundingSphereD sphere,
      ref PlaneD plane,
      out double distanceFromPlaneToSphere)
    {
      double d = plane.D;
      distanceFromPlaneToSphere = plane.Normal.X * sphere.Center.X + plane.Normal.Y * sphere.Center.Y + plane.Normal.Z * sphere.Center.Z + d;
      if (Math.Abs(distanceFromPlaneToSphere) < sphere.Radius)
        return MySpherePlaneIntersectionEnum.INTERSECTS;
      return distanceFromPlaneToSphere >= sphere.Radius ? MySpherePlaneIntersectionEnum.FRONT : MySpherePlaneIntersectionEnum.BEHIND;
    }

    public static MySpherePlaneIntersectionEnum GetSpherePlaneIntersection(
      ref BoundingSphere sphere,
      ref Plane plane,
      out float distanceFromPlaneToSphere)
    {
      float d = plane.D;
      distanceFromPlaneToSphere = (float) ((double) plane.Normal.X * (double) sphere.Center.X + (double) plane.Normal.Y * (double) sphere.Center.Y + (double) plane.Normal.Z * (double) sphere.Center.Z) + d;
      if ((double) Math.Abs(distanceFromPlaneToSphere) < (double) sphere.Radius)
        return MySpherePlaneIntersectionEnum.INTERSECTS;
      return (double) distanceFromPlaneToSphere >= (double) sphere.Radius ? MySpherePlaneIntersectionEnum.FRONT : MySpherePlaneIntersectionEnum.BEHIND;
    }

    public static Vector3D GetTransformNormalNormalized(Vector3D vec, ref MatrixD matrix)
    {
      Vector3D result;
      Vector3D.TransformNormal(ref vec, ref matrix, out result);
      return MyUtils.Normalize(result);
    }

    public static Vector3D GetVector3Scaled(Vector3D originalVector, float newLength)
    {
      if ((double) newLength == 0.0)
        return Vector3D.Zero;
      double num1 = originalVector.Length();
      if (num1 == 0.0)
        return Vector3D.Zero;
      double num2 = (double) newLength / num1;
      return new Vector3D(originalVector.X * num2, originalVector.Y * num2, originalVector.Z * num2);
    }

    public static bool IsLineIntersectingBoundingSphere(
      ref LineD line,
      ref BoundingSphereD boundingSphere)
    {
      RayD ray = new RayD(ref line.From, ref line.Direction);
      double? nullable = boundingSphere.Intersects(ray);
      return nullable.HasValue && nullable.Value <= line.Length;
    }

    public static bool IsWrongTriangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2) => (double) (vertex2 - vertex0).LengthSquared() <= 9.99999943962493E-11 || (double) (vertex1 - vertex0).LengthSquared() <= 9.99999943962493E-11 || (double) (vertex1 - vertex2).LengthSquared() <= 9.99999943962493E-11;

    public static Vector3D LinePlaneIntersection(
      Vector3D planePoint,
      Vector3 planeNormal,
      Vector3D lineStart,
      Vector3 lineDir)
    {
      double num1 = Vector3D.Dot(planePoint - lineStart, planeNormal);
      float num2 = Vector3.Dot(lineDir, planeNormal);
      return lineStart + (Vector3D) lineDir * (num1 / (double) num2);
    }

    public static Vector3 Normalize(Vector3 vec) => Vector3.Normalize(vec);

    public static Vector3D Normalize(Vector3D vec) => Vector3D.Normalize(vec);

    public static void Normalize(ref Vector3 vec, out Vector3 normalized) => Vector3.Normalize(ref vec, out normalized);

    public static void Normalize(ref Vector3D vec, out Vector3D normalized) => Vector3D.Normalize(ref vec, out normalized);

    public static void Normalize(ref Matrix m, out Matrix normalized) => normalized = Matrix.CreateWorld(m.Translation, MyUtils.Normalize(m.Forward), MyUtils.Normalize(m.Up));

    public static void Normalize(ref MatrixD m, out MatrixD normalized) => normalized = MatrixD.CreateWorld(m.Translation, MyUtils.Normalize(m.Forward), MyUtils.Normalize(m.Up));

    public static void RotationMatrixToYawPitchRoll(
      ref Matrix mx,
      out float yaw,
      out float pitch,
      out float roll)
    {
      float num1 = mx.M32;
      if ((double) num1 > 1.0)
        num1 = 1f;
      else if ((double) num1 < -1.0)
        num1 = -1f;
      pitch = (float) Math.Asin(-(double) num1);
      float num2 = 1f / 1000f;
      if (Math.Cos((double) pitch) > (double) num2)
      {
        roll = (float) Math.Atan2((double) mx.M12, (double) mx.M22);
        yaw = (float) Math.Atan2((double) mx.M31, (double) mx.M33);
      }
      else
      {
        roll = (float) Math.Atan2(-(double) mx.M21, (double) mx.M11);
        yaw = 0.0f;
      }
    }

    public static void SerializeValue(XmlWriter writer, Vector3 v) => writer.WriteValue(v.X.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " " + v.Y.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " " + v.Z.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public static void SerializeValue(XmlWriter writer, Vector4 v) => writer.WriteValue(v.X.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " " + v.Y.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " " + v.Z.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " " + v.W.ToString((IFormatProvider) CultureInfo.InvariantCulture));

    public static void ShuffleList<T>(this IList<T> list, int offset = 0, int? count = null)
    {
      int num = count ?? list.Count - offset;
      while (num > 1)
      {
        --num;
        int randomInt = MyUtils.GetRandomInt(num + 1);
        T obj = list[offset + randomInt];
        list[offset + randomInt] = list[offset + num];
        list[offset + num] = obj;
      }
    }

    public static void Swap<T>(ref T lhs, ref T rhs)
    {
      T obj = lhs;
      lhs = rhs;
      rhs = obj;
    }

    public static void VectorPlaneRotation(
      Vector3D xVector,
      Vector3D yVector,
      out Vector3D xOut,
      out Vector3D yOut,
      float angle)
    {
      Vector3D vector3D1 = xVector * Math.Cos((double) angle) + yVector * Math.Sin((double) angle);
      Vector3D vector3D2 = xVector * Math.Cos((double) angle + Math.PI / 2.0) + yVector * Math.Sin((double) angle + Math.PI / 2.0);
      xOut = vector3D1;
      yOut = vector3D2;
    }

    public static T Init<T>(ref T location) where T : class, new()
    {
      T obj = location;
      return (object) obj != null ? obj : (location = new T());
    }

    public static TCollection PrepareCollection<TCollection, TElement>(ref TCollection collection) where TCollection : class, ICollection<TElement>, new()
    {
      if ((object) collection == null)
        collection = new TCollection();
      else if (collection.Count != 0)
        collection.Clear();
      return collection;
    }

    public static MyUtils.ClearCollectionToken<List<TElement>, TElement> ReuseCollection<TElement>(
      ref List<TElement> collection)
    {
      return MyUtils.ReuseCollection<List<TElement>, TElement>(ref collection);
    }

    public static MyUtils.ClearCollectionToken<MyList<TElement>, TElement> ReuseCollection<TElement>(
      ref MyList<TElement> collection)
    {
      return MyUtils.ReuseCollection<MyList<TElement>, TElement>(ref collection);
    }

    public static MyUtils.ClearCollectionToken<HashSet<TElement>, TElement> ReuseCollection<TElement>(
      ref HashSet<TElement> collection)
    {
      return MyUtils.ReuseCollection<HashSet<TElement>, TElement>(ref collection);
    }

    public static MyUtils.ClearCollectionToken<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>> ReuseCollection<TKey, TValue>(
      ref Dictionary<TKey, TValue> collection)
    {
      return MyUtils.ReuseCollection<Dictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>(ref collection);
    }

    public static MyUtils.ClearCollectionToken<TCollection, TElement> ReuseCollection<TCollection, TElement>(
      ref TCollection collection)
      where TCollection : class, ICollection<TElement>, new()
    {
      MyUtils.PrepareCollection<TCollection, TElement>(ref collection);
      return new MyUtils.ClearCollectionToken<TCollection, TElement>(collection);
    }

    public static MyUtils.ClearRangeToken<TElement> ReuseCollectionNested<TElement>(
      ref List<TElement> collection)
    {
      if (collection == null)
        collection = new List<TElement>();
      return new MyUtils.ClearRangeToken<TElement>(collection);
    }

    public static int InterlockedMax(ref int storage, int value)
    {
      int num1 = storage;
      int num2;
      while (true)
      {
        int num3 = Math.Max(num1, value);
        num2 = Interlocked.CompareExchange(ref storage, num3, num1);
        if (num2 != num1)
          num1 = num2;
        else
          break;
      }
      return num2;
    }

    public static void InterlockedMax(ref long storage, long value)
    {
      for (long comparand = Interlocked.Read(ref storage); value > comparand; comparand = Interlocked.Read(ref storage))
        Interlocked.CompareExchange(ref storage, value, comparand);
    }

    [Conditional("DEBUG")]
    public static void CheckMainThread()
    {
    }

    private struct Edge : IEquatable<MyUtils.Edge>
    {
      public int I0;
      public int I1;

      public bool Equals(MyUtils.Edge other) => object.Equals((object) other.GetHashCode(), (object) this.GetHashCode());

      public override int GetHashCode() => this.I0 >= this.I1 ? this.I1.GetHashCode() * 397 ^ this.I0.GetHashCode() : this.I0.GetHashCode() * 397 ^ this.I1.GetHashCode();
    }

    public struct ClearCollectionToken<TCollection, TElement> : IDisposable where TCollection : class, ICollection<TElement>, new()
    {
      public readonly TCollection Collection;

      public ClearCollectionToken(TCollection collection) => this.Collection = collection;

      public void Dispose() => this.Collection.Clear();
    }

    public struct ClearRangeToken<T> : IDisposable
    {
      public readonly int Begin;
      public readonly List<T> Collection;

      public ClearRangeToken(List<T> collection)
      {
        this.Collection = collection;
        this.Begin = collection.Count;
      }

      public void Dispose() => this.Collection.RemoveRange(this.Begin, this.Collection.Count - this.Begin);

      public void Add(T element) => this.Collection.Add(element);

      public MyUtils.ClearRangeToken<T>.OffsetEnumerator GetEnumerator() => new MyUtils.ClearRangeToken<T>.OffsetEnumerator(this.Collection, this.Begin);

      public struct OffsetEnumerator : IEnumerator<T>, IEnumerator, IDisposable
      {
        private readonly int End;
        private int Index;
        private readonly List<T> List;

        public T Current => this.List[this.Index];

        object IEnumerator.Current => (object) this.List[this.Index];

        public OffsetEnumerator(List<T> list, int begin)
        {
          this.List = list;
          this.End = list.Count;
          this.Index = begin - 1;
        }

        public bool MoveNext()
        {
          ++this.Index;
          return this.Index < this.End;
        }

        public void Dispose()
        {
        }

        public void Reset() => throw new NotImplementedException();
      }
    }
  }
}
