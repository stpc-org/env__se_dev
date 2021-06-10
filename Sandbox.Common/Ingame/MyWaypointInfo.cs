// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.MyWaypointInfo
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
  public struct MyWaypointInfo : IEquatable<MyWaypointInfo>
  {
    public static MyWaypointInfo Empty;
    public readonly string Name;
    public Vector3D Coords;

    private static bool IsPrecededByWhitespace(ref TextPtr ptr)
    {
      TextPtr textPtr = ptr - 1;
      char c = textPtr.Char;
      return textPtr.IsOutOfBounds() || char.IsWhiteSpace(c) || !char.IsLetterOrDigit(c);
    }

    public static void FindAll(string source, List<MyWaypointInfo> gpsList)
    {
      TextPtr ptr = new TextPtr(source);
      gpsList.Clear();
      while (!ptr.IsOutOfBounds())
      {
        MyWaypointInfo gps;
        if (char.ToUpperInvariant(ptr.Char) == 'G' && MyWaypointInfo.IsPrecededByWhitespace(ref ptr) && MyWaypointInfo.TryParse(ref ptr, out gps))
          gpsList.Add(gps);
        else
          ++ptr;
      }
    }

    public static bool TryParse(string text, out MyWaypointInfo gps)
    {
      if (text == null)
      {
        gps = MyWaypointInfo.Empty;
        return false;
      }
      TextPtr ptr = new TextPtr(text);
      bool flag = MyWaypointInfo.TryParse(ref ptr, out gps);
      if (!flag || ptr.IsOutOfBounds())
        return flag;
      gps = MyWaypointInfo.Empty;
      return false;
    }

    private static bool TryParse(ref TextPtr ptr, out MyWaypointInfo gps)
    {
      while (char.IsWhiteSpace(ptr.Char))
        ++ptr;
      if (!ptr.StartsWithCaseInsensitive("gps:"))
      {
        gps = MyWaypointInfo.Empty;
        return false;
      }
      ptr += 4;
      StringSegment segment1;
      if (!MyWaypointInfo.GrabSegment(ref ptr, out segment1))
      {
        gps = MyWaypointInfo.Empty;
        return false;
      }
      StringSegment segment2;
      if (!MyWaypointInfo.GrabSegment(ref ptr, out segment2))
      {
        gps = MyWaypointInfo.Empty;
        return false;
      }
      StringSegment segment3;
      if (!MyWaypointInfo.GrabSegment(ref ptr, out segment3))
      {
        gps = MyWaypointInfo.Empty;
        return false;
      }
      StringSegment segment4;
      if (!MyWaypointInfo.GrabSegment(ref ptr, out segment4))
      {
        gps = MyWaypointInfo.Empty;
        return false;
      }
      while (char.IsWhiteSpace(ptr.Char))
        ++ptr;
      double result1;
      if (!double.TryParse(segment2.ToString(), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
      {
        gps = MyWaypointInfo.Empty;
        return false;
      }
      double result2;
      if (!double.TryParse(segment3.ToString(), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
      {
        gps = MyWaypointInfo.Empty;
        return false;
      }
      double result3;
      if (!double.TryParse(segment4.ToString(), NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result3))
      {
        gps = MyWaypointInfo.Empty;
        return false;
      }
      string name = segment1.ToString();
      gps = new MyWaypointInfo(name, result1, result2, result3);
      return true;
    }

    private static bool GrabSegment(ref TextPtr ptr, out StringSegment segment)
    {
      if (ptr.IsOutOfBounds())
      {
        segment = new StringSegment();
        return false;
      }
      TextPtr textPtr = ptr;
      while (!ptr.IsOutOfBounds() && ptr.Char != ':')
        ++ptr;
      if (ptr.Char != ':')
      {
        segment = new StringSegment();
        return false;
      }
      segment = new StringSegment(textPtr.Content, textPtr.Index, ptr.Index - textPtr.Index);
      ++ptr;
      return true;
    }

    public MyWaypointInfo(string name, double x, double y, double z)
    {
      this.Name = name ?? "";
      this.Coords = new Vector3D(x, y, z);
    }

    public MyWaypointInfo(string name, Vector3D coords)
      : this(name, coords.X, coords.Y, coords.Z)
    {
    }

    public bool IsEmpty() => this.Name == null;

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "GPS:{0}:{1:R}:{2:R}:{3:R}:", (object) this.Name, (object) this.Coords.X, (object) this.Coords.Y, (object) this.Coords.Z);

    public bool Equals(MyWaypointInfo other) => this.Equals(other, 0.0001);

    public bool Equals(MyWaypointInfo other, double epsilon) => string.Equals(this.Name, other.Name) && Math.Abs(this.Coords.X - other.Coords.X) < epsilon && Math.Abs(this.Coords.Y - other.Coords.Y) < epsilon && Math.Abs(this.Coords.Z - other.Coords.Z) < epsilon;

    public override bool Equals(object obj) => obj != null && obj is MyWaypointInfo other && this.Equals(other);

    public override int GetHashCode() => (((this.Name != null ? this.Name.GetHashCode() : 0) * 397 ^ this.Coords.X.GetHashCode()) * 397 ^ this.Coords.Y.GetHashCode()) * 397 ^ this.Coords.Z.GetHashCode();
  }
}
