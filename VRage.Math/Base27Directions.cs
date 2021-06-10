﻿// Decompiled with JetBrains decompiler
// Type: VRageMath.Base27Directions
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;

namespace VRageMath
{
  public class Base27Directions
  {
    public static readonly Vector3[] Directions = new Vector3[64]
    {
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(0.0f, 0.0f, 1f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(-1f, 0.0f, 0.0f),
      new Vector3(-0.7071068f, 0.0f, -0.7071068f),
      new Vector3(-0.7071068f, 0.0f, 0.7071068f),
      new Vector3(-1f, 0.0f, 0.0f),
      new Vector3(1f, 0.0f, 0.0f),
      new Vector3(0.7071068f, 0.0f, -0.7071068f),
      new Vector3(0.7071068f, 0.0f, 0.7071068f),
      new Vector3(1f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(0.0f, 0.0f, 1f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(0.0f, 0.7071068f, -0.7071068f),
      new Vector3(0.0f, 0.7071068f, 0.7071068f),
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(-0.7071068f, 0.7071068f, 0.0f),
      new Vector3(-0.5773503f, 0.5773503f, -0.5773503f),
      new Vector3(-0.5773503f, 0.5773503f, 0.5773503f),
      new Vector3(-0.7071068f, 0.7071068f, 0.0f),
      new Vector3(0.7071068f, 0.7071068f, 0.0f),
      new Vector3(0.5773503f, 0.5773503f, -0.5773503f),
      new Vector3(0.5773503f, 0.5773503f, 0.5773503f),
      new Vector3(0.7071068f, 0.7071068f, 0.0f),
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(0.0f, 0.7071068f, -0.7071068f),
      new Vector3(0.0f, 0.7071068f, 0.7071068f),
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(0.0f, -1f, 0.0f),
      new Vector3(0.0f, -0.7071068f, -0.7071068f),
      new Vector3(0.0f, -0.7071068f, 0.7071068f),
      new Vector3(0.0f, -1f, 0.0f),
      new Vector3(-0.7071068f, -0.7071068f, 0.0f),
      new Vector3(-0.5773503f, -0.5773503f, -0.5773503f),
      new Vector3(-0.5773503f, -0.5773503f, 0.5773503f),
      new Vector3(-0.7071068f, -0.7071068f, 0.0f),
      new Vector3(0.7071068f, -0.7071068f, 0.0f),
      new Vector3(0.5773503f, -0.5773503f, -0.5773503f),
      new Vector3(0.5773503f, -0.5773503f, 0.5773503f),
      new Vector3(0.7071068f, -0.7071068f, 0.0f),
      new Vector3(0.0f, -1f, 0.0f),
      new Vector3(0.0f, -0.7071068f, -0.7071068f),
      new Vector3(0.0f, -0.7071068f, 0.7071068f),
      new Vector3(0.0f, -1f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(0.0f, 0.0f, 1f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(-1f, 0.0f, 0.0f),
      new Vector3(-0.7071068f, 0.0f, -0.7071068f),
      new Vector3(-0.7071068f, 0.0f, 0.7071068f),
      new Vector3(-1f, 0.0f, 0.0f),
      new Vector3(1f, 0.0f, 0.0f),
      new Vector3(0.7071068f, 0.0f, -0.7071068f),
      new Vector3(0.7071068f, 0.0f, 0.7071068f),
      new Vector3(1f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, -1f),
      new Vector3(0.0f, 0.0f, 1f),
      new Vector3(0.0f, 0.0f, 0.0f)
    };
    public static readonly Vector3I[] DirectionsInt = new Vector3I[64]
    {
      new Vector3I(0, 0, 0),
      new Vector3I(0, 0, -1),
      new Vector3I(0, 0, 1),
      new Vector3I(0, 0, 0),
      new Vector3I(-1, 0, 0),
      new Vector3I(-1, 0, -1),
      new Vector3I(-1, 0, 1),
      new Vector3I(-1, 0, 0),
      new Vector3I(1, 0, 0),
      new Vector3I(1, 0, -1),
      new Vector3I(1, 0, 1),
      new Vector3I(1, 0, 0),
      new Vector3I(0, 0, 0),
      new Vector3I(0, 0, -1),
      new Vector3I(0, 0, 1),
      new Vector3I(0, 0, 0),
      new Vector3I(0, 1, 0),
      new Vector3I(0, 1, -1),
      new Vector3I(0, 1, 1),
      new Vector3I(0, 1, 0),
      new Vector3I(-1, 1, 0),
      new Vector3I(-1, 1, -1),
      new Vector3I(-1, 1, 1),
      new Vector3I(-1, 1, 0),
      new Vector3I(1, 1, 0),
      new Vector3I(1, 1, -1),
      new Vector3I(1, 1, 1),
      new Vector3I(1, 1, 0),
      new Vector3I(0, 1, 0),
      new Vector3I(0, 1, -1),
      new Vector3I(0, 1, 1),
      new Vector3I(0, 1, 0),
      new Vector3I(0, -1, 0),
      new Vector3I(0, -1, -1),
      new Vector3I(0, -1, 1),
      new Vector3I(0, -1, 0),
      new Vector3I(-1, -1, 0),
      new Vector3I(-1, -1, -1),
      new Vector3I(-1, -1, 1),
      new Vector3I(-1, -1, 0),
      new Vector3I(1, -1, 0),
      new Vector3I(1, -1, -1),
      new Vector3I(1, -1, 1),
      new Vector3I(1, -1, 0),
      new Vector3I(0, -1, 0),
      new Vector3I(0, -1, -1),
      new Vector3I(0, -1, 1),
      new Vector3I(0, -1, 0),
      new Vector3I(0, 0, 0),
      new Vector3I(0, 0, -1),
      new Vector3I(0, 0, 1),
      new Vector3I(0, 0, 0),
      new Vector3I(-1, 0, 0),
      new Vector3I(-1, 0, -1),
      new Vector3I(-1, 0, 1),
      new Vector3I(-1, 0, 0),
      new Vector3I(1, 0, 0),
      new Vector3I(1, 0, -1),
      new Vector3I(1, 0, 1),
      new Vector3I(1, 0, 0),
      new Vector3I(0, 0, 0),
      new Vector3I(0, 0, -1),
      new Vector3I(0, 0, 1),
      new Vector3I(0, 0, 0)
    };
    private const float DIRECTION_EPSILON = 1E-05f;
    private static readonly int[] ForwardBackward = new int[3]
    {
      1,
      0,
      2
    };
    private static readonly int[] LeftRight = new int[3]
    {
      4,
      0,
      8
    };
    private static readonly int[] UpDown = new int[3]
    {
      32,
      0,
      16
    };

    public static bool IsBaseDirection(ref Vector3 vec) => (double) vec.X * (double) vec.X + (double) vec.Y * (double) vec.Y + (double) vec.Z * (double) vec.Z - 1.0 < 9.99999974737875E-06;

    public static bool IsBaseDirection(ref Vector3I vec) => vec.X >= -1 && vec.X <= 1 && (vec.Y >= -1 && vec.Y <= 1) && vec.Z >= -1 && vec.Z <= 1;

    public static bool IsBaseDirection(Vector3 vec) => Base27Directions.IsBaseDirection(ref vec);

    public static Vector3 GetVector(int direction) => Base27Directions.Directions[direction];

    public static Vector3I GetVectorInt(int direction) => Base27Directions.DirectionsInt[direction];

    public static Vector3 GetVector(Base27Directions.Direction dir) => Base27Directions.Directions[(int) dir];

    public static Vector3I GetVectorInt(Base27Directions.Direction dir) => Base27Directions.DirectionsInt[(int) dir];

    public static Base27Directions.Direction GetDirection(Vector3 vec) => Base27Directions.GetDirection(ref vec);

    public static Base27Directions.Direction GetDirection(Vector3I vec) => Base27Directions.GetDirection(ref vec);

    public static Base27Directions.Direction GetDirection(ref Vector3 vec) => (Base27Directions.Direction) (0 + Base27Directions.ForwardBackward[(int) Math.Round((double) vec.Z + 1.0)] + Base27Directions.LeftRight[(int) Math.Round((double) vec.X + 1.0)] + Base27Directions.UpDown[(int) Math.Round((double) vec.Y + 1.0)]);

    public static Base27Directions.Direction GetDirection(ref Vector3I vec) => (Base27Directions.Direction) (0 + Base27Directions.ForwardBackward[vec.Z + 1] + Base27Directions.LeftRight[vec.X + 1] + Base27Directions.UpDown[vec.Y + 1]);

    public static Base27Directions.Direction GetForward(ref Quaternion rot)
    {
      Vector3 result;
      Vector3.Transform(ref Vector3.Forward, ref rot, out result);
      return Base27Directions.GetDirection(ref result);
    }

    public static Base27Directions.Direction GetUp(ref Quaternion rot)
    {
      Vector3 result;
      Vector3.Transform(ref Vector3.Up, ref rot, out result);
      return Base27Directions.GetDirection(ref result);
    }

    [Flags]
    public enum Direction : byte
    {
      Forward = 1,
      Backward = 2,
      Left = 4,
      Right = 8,
      Up = 16, // 0x10
      Down = 32, // 0x20
    }
  }
}
