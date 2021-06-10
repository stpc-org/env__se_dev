﻿// Decompiled with JetBrains decompiler
// Type: VRageMath.MyMortonCode3D
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System.Diagnostics;

namespace VRageMath
{
  public static class MyMortonCode3D
  {
    public static int Encode(ref Vector3I value) => MyMortonCode3D.splitBits(value.X) | MyMortonCode3D.splitBits(value.Y) << 1 | MyMortonCode3D.splitBits(value.Z) << 2;

    public static void Decode(int code, out Vector3I value)
    {
      value.X = MyMortonCode3D.joinBits(code);
      value.Y = MyMortonCode3D.joinBits(code >> 1);
      value.Z = MyMortonCode3D.joinBits(code >> 2);
    }

    private static int splitBits(int x)
    {
      x = (x | x << 16) & 50331903;
      x = (x | x << 8) & 50393103;
      x = (x | x << 4) & 51130563;
      x = (x | x << 2) & 153391689;
      return x;
    }

    private static int joinBits(int x)
    {
      x &= 153391689;
      x = (x | x >> 2) & 51130563;
      x = (x | x >> 4) & 50393103;
      x = (x | x >> 8) & 50331903;
      x = (x | x >> 16) & 1023;
      return x;
    }

    [Conditional("DEBUG")]
    private static void AssertRange(Vector3I value)
    {
    }
  }
}
