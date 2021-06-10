// Decompiled with JetBrains decompiler
// Type: VRage.MyDebugUtils
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Utils;
using VRageMath;

namespace VRage
{
  public class MyDebugUtils
  {
    private static readonly ConcurrentDictionary<MemberInfo, string> m_debugNames = new ConcurrentDictionary<MemberInfo, string>();

    public static bool IsValid(float f) => !float.IsNaN(f) && !float.IsInfinity(f);

    public static bool IsValid(double d) => !double.IsNaN(d) && !double.IsInfinity(d);

    public static bool IsValid(Vector3 vec) => MyDebugUtils.IsValid(vec.X) && MyDebugUtils.IsValid(vec.Y) && MyDebugUtils.IsValid(vec.Z);

    public static bool IsValid(Vector3? vec)
    {
      if (!vec.HasValue)
        return true;
      return MyDebugUtils.IsValid(vec.Value.X) && MyDebugUtils.IsValid(vec.Value.Y) && MyDebugUtils.IsValid(vec.Value.Z);
    }

    public static bool IsValid(Vector3D vec) => MyDebugUtils.IsValid(vec.X) && MyDebugUtils.IsValid(vec.Y) && MyDebugUtils.IsValid(vec.Z);

    public static bool IsValid(Vector3D? vec)
    {
      if (!vec.HasValue)
        return true;
      return MyDebugUtils.IsValid(vec.Value.X) && MyDebugUtils.IsValid(vec.Value.Y) && MyDebugUtils.IsValid(vec.Value.Z);
    }

    public static bool IsValidNormal(Vector3 vec)
    {
      float num = vec.LengthSquared();
      return MyDebugUtils.IsValid(vec) && (double) num > 0.999000012874603 && (double) num < 1.00100004673004;
    }

    public static bool IsValid(Vector2 vec) => MyDebugUtils.IsValid(vec.X) && MyDebugUtils.IsValid(vec.Y);

    public static bool IsValid(Matrix matrix) => MyDebugUtils.IsValid(matrix.Up) && MyDebugUtils.IsValid(matrix.Left) && (MyDebugUtils.IsValid(matrix.Forward) && MyDebugUtils.IsValid(matrix.Translation)) && matrix != Matrix.Zero;

    public static bool IsValid(Quaternion q) => MyDebugUtils.IsValid(q.X) && MyDebugUtils.IsValid(q.Y) && (MyDebugUtils.IsValid(q.Z) && MyDebugUtils.IsValid(q.W)) && !MyUtils.IsZero(q);

    [Conditional("DEBUG")]
    public static void AssertIsValid(Vector3D vec)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertIsValid(Vector3D? vec)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertIsValid(Vector3 vec)
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
    public static void AssertIsValid(Matrix matrix)
    {
    }

    [Conditional("DEBUG")]
    public static void AssertIsValid(Quaternion q)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetDebugName(MemberInfo member)
    {
      string name;
      if (!MyDebugUtils.m_debugNames.TryGetValue(member, out name))
        MyDebugUtils.m_debugNames.TryAdd(member, name = member.Name);
      return name;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetDebugName(object @object) => MyDebugUtils.GetDebugName((MemberInfo) @object.GetType());
  }
}
