// Decompiled with JetBrains decompiler
// Type: VRageMath.Point
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [Serializable]
  public struct Point : IEquatable<Point>
  {
    private static Point _zero;
    public int X;
    public int Y;

    public static Point Zero => Point._zero;

    public Point(int x, int y)
    {
      this.X = x;
      this.Y = y;
    }

    public static bool operator ==(Point a, Point b) => a.Equals(b);

    public static bool operator !=(Point a, Point b) => a.X != b.X || a.Y != b.Y;

    public bool Equals(Point other) => this.X == other.X && this.Y == other.Y;

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj is Point other)
        flag = this.Equals(other);
      return flag;
    }

    public override int GetHashCode() => this.X.GetHashCode() + this.Y.GetHashCode();

    public override string ToString()
    {
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      return string.Format((IFormatProvider) currentCulture, "{{X:{0} Y:{1}}}", new object[2]
      {
        (object) this.X.ToString((IFormatProvider) currentCulture),
        (object) this.Y.ToString((IFormatProvider) currentCulture)
      });
    }

    protected class VRageMath_Point\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<Point, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Point owner, in int value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Point owner, out int value) => value = owner.X;
    }

    protected class VRageMath_Point\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<Point, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref Point owner, in int value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref Point owner, out int value) => value = owner.Y;
    }
  }
}
