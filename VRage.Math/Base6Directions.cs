// Decompiled with JetBrains decompiler
// Type: VRageMath.Base6Directions
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;

namespace VRageMath
{
  public class Base6Directions
  {
    public static readonly Base6Directions.Direction[] EnumDirections = new Base6Directions.Direction[6]
    {
      Base6Directions.Direction.Forward,
      Base6Directions.Direction.Backward,
      Base6Directions.Direction.Left,
      Base6Directions.Direction.Right,
      Base6Directions.Direction.Up,
      Base6Directions.Direction.Down
    };
    public static readonly Vector3[] Directions = new Vector3[6]
    {
      Vector3.Forward,
      Vector3.Backward,
      Vector3.Left,
      Vector3.Right,
      Vector3.Up,
      Vector3.Down
    };
    public static readonly Vector3I[] IntDirections = new Vector3I[6]
    {
      Vector3I.Forward,
      Vector3I.Backward,
      Vector3I.Left,
      Vector3I.Right,
      Vector3I.Up,
      Vector3I.Down
    };
    private static readonly Base6Directions.Direction[] LeftDirections = new Base6Directions.Direction[36]
    {
      Base6Directions.Direction.Forward,
      Base6Directions.Direction.Forward,
      Base6Directions.Direction.Down,
      Base6Directions.Direction.Up,
      Base6Directions.Direction.Left,
      Base6Directions.Direction.Right,
      Base6Directions.Direction.Forward,
      Base6Directions.Direction.Forward,
      Base6Directions.Direction.Up,
      Base6Directions.Direction.Down,
      Base6Directions.Direction.Right,
      Base6Directions.Direction.Left,
      Base6Directions.Direction.Up,
      Base6Directions.Direction.Down,
      Base6Directions.Direction.Left,
      Base6Directions.Direction.Left,
      Base6Directions.Direction.Backward,
      Base6Directions.Direction.Forward,
      Base6Directions.Direction.Down,
      Base6Directions.Direction.Up,
      Base6Directions.Direction.Left,
      Base6Directions.Direction.Left,
      Base6Directions.Direction.Forward,
      Base6Directions.Direction.Backward,
      Base6Directions.Direction.Right,
      Base6Directions.Direction.Left,
      Base6Directions.Direction.Forward,
      Base6Directions.Direction.Backward,
      Base6Directions.Direction.Left,
      Base6Directions.Direction.Right,
      Base6Directions.Direction.Left,
      Base6Directions.Direction.Right,
      Base6Directions.Direction.Backward,
      Base6Directions.Direction.Forward,
      Base6Directions.Direction.Left,
      Base6Directions.Direction.Right
    };
    private const float DIRECTION_EPSILON = 1E-05f;
    private static readonly int[] ForwardBackward = new int[3]
    {
      0,
      0,
      1
    };
    private static readonly int[] LeftRight = new int[3]
    {
      2,
      0,
      3
    };
    private static readonly int[] UpDown = new int[3]
    {
      5,
      0,
      4
    };

    private Base6Directions()
    {
    }

    public static bool IsBaseDirection(ref Vector3 vec) => (double) vec.X * (double) vec.X + (double) vec.Y * (double) vec.Y + (double) vec.Z * (double) vec.Z - 1.0 < 9.99999974737875E-06;

    public static bool IsBaseDirection(Vector3 vec) => Base6Directions.IsBaseDirection(ref vec);

    public static bool IsBaseDirection(ref Vector3I vec) => vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z - 1 == 0;

    public static Vector3 GetVector(int direction)
    {
      direction %= 6;
      return Base6Directions.Directions[direction];
    }

    public static Vector3 GetVector(Base6Directions.Direction dir) => Base6Directions.GetVector((int) dir);

    public static Vector3I GetIntVector(int direction)
    {
      direction %= 6;
      return Base6Directions.IntDirections[direction];
    }

    public static Vector3I GetIntVector(Base6Directions.Direction dir)
    {
      int index = (int) dir % 6;
      return Base6Directions.IntDirections[index];
    }

    public static void GetVector(Base6Directions.Direction dir, out Vector3 result)
    {
      int index = (int) dir % 6;
      result = Base6Directions.Directions[index];
    }

    public static Base6Directions.DirectionFlags GetDirectionFlag(
      Base6Directions.Direction dir)
    {
      return (Base6Directions.DirectionFlags) (1U << (int) (dir & (Base6Directions.Direction) 31));
    }

    public static Base6Directions.Direction GetPerpendicular(
      Base6Directions.Direction dir)
    {
      return Base6Directions.GetAxis(dir) == Base6Directions.Axis.UpDown ? Base6Directions.Direction.Right : Base6Directions.Direction.Up;
    }

    public static Base6Directions.Direction GetDirection(Vector3 vec) => Base6Directions.GetDirection(ref vec);

    public static Base6Directions.Direction GetDirection(ref Vector3 vec) => (Base6Directions.Direction) (0 + Base6Directions.ForwardBackward[(int) Math.Round((double) vec.Z + 1.0)] + Base6Directions.LeftRight[(int) Math.Round((double) vec.X + 1.0)] + Base6Directions.UpDown[(int) Math.Round((double) vec.Y + 1.0)]);

    public static Base6Directions.Direction GetDirection(Vector3I vec) => Base6Directions.GetDirection(ref vec);

    public static Base6Directions.Direction GetDirection(ref Vector3I vec) => (Base6Directions.Direction) (0 + Base6Directions.ForwardBackward[vec.Z + 1] + Base6Directions.LeftRight[vec.X + 1] + Base6Directions.UpDown[vec.Y + 1]);

    public static Base6Directions.Direction GetClosestDirection(Vector3 vec) => Base6Directions.GetClosestDirection(ref vec);

    public static Base6Directions.Direction GetClosestDirection(ref Vector3 vec)
    {
      Vector3 vec1 = Vector3.Sign(Vector3.DominantAxisProjection(vec));
      return Base6Directions.GetDirection(ref vec1);
    }

    public static Base6Directions.Direction GetDirectionInAxis(
      Vector3 vec,
      Base6Directions.Axis axis)
    {
      return Base6Directions.GetDirectionInAxis(ref vec, axis);
    }

    public static Base6Directions.Direction GetDirectionInAxis(
      ref Vector3 vec,
      Base6Directions.Axis axis)
    {
      Base6Directions.Direction baseAxisDirection = Base6Directions.GetBaseAxisDirection(axis);
      Vector3 vector3 = (Vector3) Base6Directions.IntDirections[(int) baseAxisDirection] * vec;
      return (double) vector3.X + (double) vector3.Y + (double) vector3.Z >= 1.0 ? baseAxisDirection : Base6Directions.GetFlippedDirection(baseAxisDirection);
    }

    public static Base6Directions.Direction GetForward(Quaternion rot)
    {
      Vector3 result;
      Vector3.Transform(ref Vector3.Forward, ref rot, out result);
      return Base6Directions.GetDirection(ref result);
    }

    public static Base6Directions.Direction GetForward(ref Quaternion rot)
    {
      Vector3 result;
      Vector3.Transform(ref Vector3.Forward, ref rot, out result);
      return Base6Directions.GetDirection(ref result);
    }

    public static Base6Directions.Direction GetForward(ref Matrix rotation)
    {
      Vector3 result;
      Vector3.TransformNormal(ref Vector3.Forward, ref rotation, out result);
      return Base6Directions.GetDirection(ref result);
    }

    public static Base6Directions.Direction GetUp(Quaternion rot)
    {
      Vector3 result;
      Vector3.Transform(ref Vector3.Up, ref rot, out result);
      return Base6Directions.GetDirection(ref result);
    }

    public static Base6Directions.Direction GetUp(ref Quaternion rot)
    {
      Vector3 result;
      Vector3.Transform(ref Vector3.Up, ref rot, out result);
      return Base6Directions.GetDirection(ref result);
    }

    public static Base6Directions.Direction GetUp(ref Matrix rotation)
    {
      Vector3 result;
      Vector3.TransformNormal(ref Vector3.Up, ref rotation, out result);
      return Base6Directions.GetDirection(ref result);
    }

    public static Base6Directions.Axis GetAxis(Base6Directions.Direction direction) => (Base6Directions.Axis) ((uint) direction >> 1);

    public static Base6Directions.Direction GetBaseAxisDirection(
      Base6Directions.Axis axis)
    {
      return (Base6Directions.Direction) ((uint) axis << 1);
    }

    public static Base6Directions.Direction GetFlippedDirection(
      Base6Directions.Direction toFlip)
    {
      return toFlip ^ Base6Directions.Direction.Backward;
    }

    public static Base6Directions.Direction GetCross(
      Base6Directions.Direction dir1,
      Base6Directions.Direction dir2)
    {
      return Base6Directions.GetLeft(dir1, dir2);
    }

    public static Base6Directions.Direction GetLeft(
      Base6Directions.Direction up,
      Base6Directions.Direction forward)
    {
      return Base6Directions.LeftDirections[(int) ((byte) ((int) forward * 6) + up)];
    }

    public static Base6Directions.Direction GetOppositeDirection(
      Base6Directions.Direction dir)
    {
      switch (dir)
      {
        case Base6Directions.Direction.Backward:
          return Base6Directions.Direction.Forward;
        case Base6Directions.Direction.Left:
          return Base6Directions.Direction.Right;
        case Base6Directions.Direction.Right:
          return Base6Directions.Direction.Left;
        case Base6Directions.Direction.Up:
          return Base6Directions.Direction.Down;
        case Base6Directions.Direction.Down:
          return Base6Directions.Direction.Up;
        default:
          return Base6Directions.Direction.Backward;
      }
    }

    public static Quaternion GetOrientation(
      Base6Directions.Direction forward,
      Base6Directions.Direction up)
    {
      return Quaternion.CreateFromForwardUp(Base6Directions.GetVector(forward), Base6Directions.GetVector(up));
    }

    public static bool IsValidBlockOrientation(
      Base6Directions.Direction forward,
      Base6Directions.Direction up)
    {
      return forward <= Base6Directions.Direction.Down && up <= Base6Directions.Direction.Down && (double) Vector3.Dot(Base6Directions.GetVector(forward), Base6Directions.GetVector(up)) == 0.0;
    }

    public enum Direction : byte
    {
      Forward,
      Backward,
      Left,
      Right,
      Up,
      Down,
    }

    [Flags]
    public enum DirectionFlags : byte
    {
      Forward = 1,
      Backward = 2,
      Left = 4,
      Right = 8,
      Up = 16, // 0x10
      Down = 32, // 0x20
      All = Down | Up | Right | Left | Backward | Forward, // 0x3F
    }

    public enum Axis : byte
    {
      ForwardBackward,
      LeftRight,
      UpDown,
    }
  }
}
