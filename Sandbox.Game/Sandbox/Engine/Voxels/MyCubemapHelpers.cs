// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyCubemapHelpers
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public static class MyCubemapHelpers
  {
    public const int NUM_MAPS = 6;
    public static readonly MyCubemapHelpers.TexcoordCalculator[] TexcoordCalculators = new MyCubemapHelpers.TexcoordCalculator[6]
    {
      new MyCubemapHelpers.TexcoordCalculator(MyCubemapHelpers.CalcFrontTexcoord),
      new MyCubemapHelpers.TexcoordCalculator(MyCubemapHelpers.CalcBackTexcoord),
      new MyCubemapHelpers.TexcoordCalculator(MyCubemapHelpers.CalcLeftTexcoord),
      new MyCubemapHelpers.TexcoordCalculator(MyCubemapHelpers.CalcRightTexcoord),
      new MyCubemapHelpers.TexcoordCalculator(MyCubemapHelpers.CalcUpTexcoord),
      new MyCubemapHelpers.TexcoordCalculator(MyCubemapHelpers.CalcDownTexcoord)
    };
    public const float UshortRecip = 1.525902E-05f;
    public const float Ushort2Recip = 3.051804E-05f;
    public const float ByteRecip = 0.003921569f;

    public static string GetNameForFace(int i)
    {
      switch (i)
      {
        case 0:
          return "front";
        case 1:
          return "back";
        case 2:
          return "left";
        case 3:
          return "right";
        case 4:
          return "up";
        case 5:
          return "down";
        default:
          return "";
      }
    }

    public static void CalculateSamplePosition(
      ref Vector3 localPos,
      out Vector3I samplePosition,
      ref Vector2 texCoord,
      int resolution)
    {
      Vector3 vector3 = Vector3.Abs(localPos);
      if ((double) vector3.X > (double) vector3.Y)
      {
        if ((double) vector3.X > (double) vector3.Z)
        {
          localPos /= vector3.X;
          texCoord.Y = -localPos.Y;
          if ((double) localPos.X > 0.0)
          {
            texCoord.X = -localPos.Z;
            samplePosition.X = 2;
          }
          else
          {
            texCoord.X = localPos.Z;
            samplePosition.X = 3;
          }
        }
        else
        {
          localPos /= vector3.Z;
          texCoord.Y = -localPos.Y;
          if ((double) localPos.Z > 0.0)
          {
            texCoord.X = localPos.X;
            samplePosition.X = 1;
          }
          else
          {
            texCoord.X = -localPos.X;
            samplePosition.X = 0;
          }
        }
      }
      else if ((double) vector3.Y > (double) vector3.Z)
      {
        localPos /= vector3.Y;
        texCoord.Y = -localPos.Z;
        if ((double) localPos.Y > 0.0)
        {
          texCoord.X = -localPos.X;
          samplePosition.X = 4;
        }
        else
        {
          texCoord.X = localPos.X;
          samplePosition.X = 5;
        }
      }
      else
      {
        localPos /= vector3.Z;
        texCoord.Y = -localPos.Y;
        if ((double) localPos.Z > 0.0)
        {
          texCoord.X = localPos.X;
          samplePosition.X = 1;
        }
        else
        {
          texCoord.X = -localPos.X;
          samplePosition.X = 0;
        }
      }
      texCoord = (texCoord + 1f) * 0.5f * (float) resolution;
      samplePosition.Y = (int) Math.Round((double) texCoord.X);
      samplePosition.Z = (int) Math.Round((double) texCoord.Y);
    }

    public static void CalculateSampleTexcoord(
      ref Vector3 localPos,
      out int face,
      out Vector2 texCoord)
    {
      Vector3 vector3 = Vector3.Abs(localPos);
      if ((double) vector3.X > (double) vector3.Y)
      {
        if ((double) vector3.X > (double) vector3.Z)
        {
          float num = 1f / vector3.X;
          texCoord.Y = -localPos.Y * num;
          if ((double) localPos.X > 0.0)
          {
            texCoord.X = -localPos.Z * num;
            face = 2;
          }
          else
          {
            texCoord.X = localPos.Z * num;
            face = 3;
          }
        }
        else
        {
          float num = 1f / vector3.Z;
          texCoord.Y = -localPos.Y * num;
          if ((double) localPos.Z > 0.0)
          {
            texCoord.X = localPos.X * num;
            face = 1;
          }
          else
          {
            texCoord.X = -localPos.X * num;
            face = 0;
          }
        }
      }
      else if ((double) vector3.Y > (double) vector3.Z)
      {
        float num = 1f / vector3.Y;
        texCoord.Y = -localPos.Z * num;
        if ((double) localPos.Y > 0.0)
        {
          texCoord.X = -localPos.X * num;
          face = 4;
        }
        else
        {
          texCoord.X = localPos.X * num;
          face = 5;
        }
      }
      else
      {
        float num = 1f / vector3.Z;
        texCoord.Y = -localPos.Y * num;
        if ((double) localPos.Z > 0.0)
        {
          texCoord.X = localPos.X * num;
          face = 1;
        }
        else
        {
          texCoord.X = -localPos.X * num;
          face = 0;
        }
      }
      texCoord = (texCoord + 1f) * 0.5f;
      if ((double) texCoord.X == 1.0)
        texCoord.X = 0.999999f;
      if ((double) texCoord.Y != 1.0)
        return;
      texCoord.Y = 0.999999f;
    }

    public static void CalculateTexcoordForFace(
      ref Vector3 localPos,
      int face,
      out Vector2 texCoord)
    {
      switch (face)
      {
        case 0:
          float num1 = 1f / Math.Abs(localPos.Z);
          texCoord.X = -localPos.X * num1;
          texCoord.Y = -localPos.Y * num1;
          break;
        case 1:
          float num2 = 1f / Math.Abs(localPos.Z);
          texCoord.X = localPos.X * num2;
          texCoord.Y = -localPos.Y * num2;
          break;
        case 2:
          float num3 = 1f / Math.Abs(localPos.X);
          texCoord.X = -localPos.Z * num3;
          texCoord.Y = -localPos.Y * num3;
          break;
        case 3:
          float num4 = 1f / Math.Abs(localPos.X);
          texCoord.X = localPos.Z * num4;
          texCoord.Y = -localPos.Y * num4;
          break;
        case 4:
          float num5 = 1f / Math.Abs(localPos.Y);
          texCoord.X = -localPos.X * num5;
          texCoord.Y = -localPos.Z * num5;
          break;
        case 5:
          float num6 = 1f / Math.Abs(localPos.Y);
          texCoord.X = localPos.X * num6;
          texCoord.Y = -localPos.Z * num6;
          break;
        default:
          texCoord = Vector2.Zero;
          break;
      }
      texCoord = (texCoord + 1f) * 0.5f;
      if ((double) texCoord.X == 1.0)
        texCoord.X = 0.999999f;
      if ((double) texCoord.Y != 1.0)
        return;
      texCoord.Y = 0.999999f;
    }

    public static void GetCubeFace(ref Vector3 position, out int face)
    {
      Vector3 vector3 = Vector3.Abs(position);
      if ((double) vector3.X > (double) vector3.Y)
      {
        if ((double) vector3.X > (double) vector3.Z)
        {
          if ((double) position.X > 0.0)
            face = 2;
          else
            face = 3;
        }
        else if ((double) position.Z > 0.0)
          face = 1;
        else
          face = 0;
      }
      else if ((double) vector3.Y > (double) vector3.Z)
      {
        if ((double) position.Y > 0.0)
          face = 4;
        else
          face = 5;
      }
      else if ((double) position.Z > 0.0)
        face = 1;
      else
        face = 0;
    }

    public static void GetCubeFaceDirection(ref Vector3 position, out Vector3B face)
    {
      Vector3 vector3 = Vector3.Abs(position);
      if ((double) vector3.X > (double) vector3.Y)
      {
        if ((double) vector3.X > (double) vector3.Z)
        {
          if ((double) position.X > 0.0)
            face = Vector3B.Right;
          else
            face = Vector3B.Left;
        }
        else if ((double) position.Z > 0.0)
          face = Vector3B.Backward;
        else
          face = Vector3B.Forward;
      }
      else if ((double) vector3.Y > (double) vector3.Z)
      {
        if ((double) position.Y > 0.0)
          face = Vector3B.Up;
        else
          face = Vector3B.Down;
      }
      else if ((double) position.Z > 0.0)
        face = Vector3B.Backward;
      else
        face = Vector3B.Forward;
    }

    public static void CalcUpTexcoord(ref Vector3 localPos, out Vector2 texCoord)
    {
      Vector3 vector3 = Vector3.Abs(localPos);
      texCoord.Y = -localPos.Z / vector3.Y;
      texCoord.X = -localPos.X / vector3.Y;
      texCoord = (texCoord + 1f) * 0.5f;
    }

    public static void CalcDownTexcoord(ref Vector3 localPos, out Vector2 texCoord)
    {
      Vector3 vector3 = Vector3.Abs(localPos);
      texCoord.Y = -localPos.Z / vector3.Y;
      texCoord.X = localPos.X / vector3.Y;
      texCoord = (texCoord + 1f) * 0.5f;
    }

    public static void CalcLeftTexcoord(ref Vector3 localPos, out Vector2 texCoord)
    {
      Vector3 vector3 = Vector3.Abs(localPos);
      texCoord.Y = -localPos.Y / vector3.X;
      texCoord.X = -localPos.Z / vector3.X;
      texCoord = (texCoord + 1f) * 0.5f;
    }

    public static void CalcRightTexcoord(ref Vector3 localPos, out Vector2 texCoord)
    {
      Vector3 vector3 = Vector3.Abs(localPos);
      texCoord.Y = -localPos.Y / vector3.X;
      texCoord.X = localPos.Z / vector3.X;
      texCoord = (texCoord + 1f) * 0.5f;
    }

    public static void CalcBackTexcoord(ref Vector3 localPos, out Vector2 texCoord)
    {
      Vector3 vector3 = Vector3.Abs(localPos);
      texCoord.Y = -localPos.Y / vector3.Z;
      texCoord.X = localPos.X / vector3.Z;
      texCoord = (texCoord + 1f) * 0.5f;
    }

    public static void CalcFrontTexcoord(ref Vector3 localPos, out Vector2 texCoord)
    {
      Vector3 vector3 = Vector3.Abs(localPos);
      texCoord.Y = -localPos.Y / vector3.Z;
      texCoord.X = -localPos.X / vector3.Z;
      texCoord = (texCoord + 1f) * 0.5f;
    }

    public static Vector2I GetStep(ref Vector2I start, ref Vector2I end)
    {
      if (start.X > end.X)
        return -Vector2I.UnitX;
      if (start.X < end.X)
        return Vector2I.UnitX;
      if (start.Y > end.Y)
        return -Vector2I.UnitY;
      return start.Y < end.Y ? Vector2I.UnitY : Vector2I.Zero;
    }

    internal enum Faces : byte
    {
      Front,
      Back,
      Left,
      Right,
      Up,
      Down,
    }

    public delegate void TexcoordCalculator(ref Vector3 local, out Vector2 texcoord);
  }
}
