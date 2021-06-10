// Decompiled with JetBrains decompiler
// Type: VRage.Partition
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Linq;

namespace VRage
{
  public static class Partition
  {
    private static readonly string[] m_letters = Enumerable.Range(65, 26).Select<int, string>((Func<int, string>) (s => new string((char) s, 1))).ToArray<string>();

    public static T Select<T>(int num, T a, T b) => num % 2 != 0 ? b : a;

    public static T Select<T>(int num, T a, T b, T c)
    {
      switch ((uint) num % 3U)
      {
        case 0:
          return a;
        case 1:
          return b;
        default:
          return c;
      }
    }

    public static T Select<T>(int num, T a, T b, T c, T d)
    {
      switch ((uint) num % 4U)
      {
        case 0:
          return a;
        case 1:
          return b;
        case 2:
          return c;
        default:
          return d;
      }
    }

    public static T Select<T>(int num, T a, T b, T c, T d, T e)
    {
      switch ((uint) num % 5U)
      {
        case 0:
          return a;
        case 1:
          return b;
        case 2:
          return c;
        case 3:
          return d;
        default:
          return e;
      }
    }

    public static T Select<T>(int num, T a, T b, T c, T d, T e, T f)
    {
      switch ((uint) num % 6U)
      {
        case 0:
          return a;
        case 1:
          return b;
        case 2:
          return c;
        case 3:
          return d;
        case 4:
          return e;
        default:
          return f;
      }
    }

    public static T Select<T>(int num, T a, T b, T c, T d, T e, T f, T g)
    {
      switch ((uint) num % 7U)
      {
        case 0:
          return a;
        case 1:
          return b;
        case 2:
          return c;
        case 3:
          return d;
        case 4:
          return e;
        case 5:
          return f;
        default:
          return g;
      }
    }

    public static T Select<T>(int num, T a, T b, T c, T d, T e, T f, T g, T h)
    {
      switch ((uint) num % 8U)
      {
        case 0:
          return a;
        case 1:
          return b;
        case 2:
          return c;
        case 3:
          return d;
        case 4:
          return e;
        case 5:
          return f;
        case 6:
          return g;
        default:
          return h;
      }
    }

    public static T Select<T>(int num, T a, T b, T c, T d, T e, T f, T g, T h, T i)
    {
      switch ((uint) num % 9U)
      {
        case 0:
          return a;
        case 1:
          return b;
        case 2:
          return c;
        case 3:
          return d;
        case 4:
          return e;
        case 5:
          return f;
        case 6:
          return g;
        case 7:
          return h;
        default:
          return i;
      }
    }

    public static string SelectStringByLetter(char c)
    {
      if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z')
      {
        c = char.ToUpperInvariant(c);
        return Partition.m_letters[(int) c - 65];
      }
      return c >= '0' && c <= '9' ? "0-9" : "Non-letter";
    }

    public static string SelectStringGroupOfTenByLetter(char c)
    {
      c = char.ToUpperInvariant(c);
      if (c >= '0' && c <= '9')
        return "0-9";
      if (c == 'A' || c == 'B' || c == 'C')
        return "A-C";
      if (c == 'D' || c == 'E' || c == 'F')
        return "D-F";
      if (c == 'G' || c == 'H' || c == 'I')
        return "G-I";
      if (c == 'J' || c == 'K' || c == 'L')
        return "J-L";
      if (c == 'M' || c == 'N' || c == 'O')
        return "M-O";
      if (c == 'P' || c == 'Q' || c == 'R')
        return "P-R";
      if (c == 'S' || c == 'T' || (c == 'U' || c == 'V'))
        return "S-V";
      return c == 'W' || c == 'X' || (c == 'Y' || c == 'Z') ? "W-Z" : "Non-letter";
    }
  }
}
