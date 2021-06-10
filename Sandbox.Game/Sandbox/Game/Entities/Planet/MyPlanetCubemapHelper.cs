// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Planet.MyPlanetCubemapHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRageMath;

namespace Sandbox.Game.Entities.Planet
{
  public static class MyPlanetCubemapHelper
  {
    public static uint[] AdjacentFaceTransforms = new uint[36]
    {
      0U,
      0U,
      0U,
      16U,
      10U,
      26U,
      0U,
      0U,
      16U,
      0U,
      6U,
      22U,
      16U,
      0U,
      0U,
      0U,
      3U,
      31U,
      0U,
      16U,
      0U,
      0U,
      15U,
      19U,
      25U,
      5U,
      19U,
      15U,
      0U,
      0U,
      9U,
      21U,
      31U,
      3U,
      0U,
      0U
    };

    public static void ProjectToCube(
      ref Vector3D localPos,
      out int direction,
      out Vector2D texcoords)
    {
      Vector3D abs;
      Vector3D.Abs(ref localPos, out abs);
      if (abs.X > abs.Y)
      {
        if (abs.X > abs.Z)
        {
          localPos /= abs.X;
          texcoords.Y = localPos.Y;
          if (localPos.X > 0.0)
          {
            texcoords.X = -localPos.Z;
            direction = 3;
          }
          else
          {
            texcoords.X = localPos.Z;
            direction = 2;
          }
        }
        else
        {
          localPos /= abs.Z;
          texcoords.Y = localPos.Y;
          if (localPos.Z > 0.0)
          {
            texcoords.X = localPos.X;
            direction = 1;
          }
          else
          {
            texcoords.X = -localPos.X;
            direction = 0;
          }
        }
      }
      else if (abs.Y > abs.Z)
      {
        localPos /= abs.Y;
        texcoords.Y = localPos.X;
        if (localPos.Y > 0.0)
        {
          texcoords.X = localPos.Z;
          direction = 4;
        }
        else
        {
          texcoords.X = -localPos.Z;
          direction = 5;
        }
      }
      else
      {
        localPos /= abs.Z;
        texcoords.Y = localPos.Y;
        if (localPos.Z > 0.0)
        {
          texcoords.X = localPos.X;
          direction = 1;
        }
        else
        {
          texcoords.X = -localPos.X;
          direction = 0;
        }
      }
    }

    public static int FindCubeFace(ref Vector3D localPos)
    {
      Vector3D abs;
      Vector3D.Abs(ref localPos, out abs);
      return abs.X > abs.Y ? (abs.X > abs.Z ? (localPos.X > 0.0 ? 3 : 2) : (localPos.Z > 0.0 ? 1 : 0)) : (abs.Y > abs.Z ? (localPos.Y > 0.0 ? 4 : 5) : (localPos.Z > 0.0 ? 1 : 0));
    }

    public static void ProjectForFace(ref Vector3D localPos, int face, out Vector2D normalCoord)
    {
      Vector3D abs;
      Vector3D.Abs(ref localPos, out abs);
      switch ((byte) face)
      {
        case 0:
          localPos /= abs.Z;
          normalCoord.X = -localPos.X;
          normalCoord.Y = localPos.Y;
          break;
        case 1:
          localPos /= abs.Z;
          normalCoord.X = localPos.X;
          normalCoord.Y = localPos.Y;
          break;
        case 2:
          localPos /= abs.X;
          normalCoord.X = localPos.Z;
          normalCoord.Y = localPos.Y;
          break;
        case 3:
          localPos /= abs.X;
          normalCoord.X = -localPos.Z;
          normalCoord.Y = localPos.Y;
          break;
        case 4:
          localPos /= abs.Y;
          normalCoord.X = localPos.Z;
          normalCoord.Y = localPos.X;
          break;
        case 5:
          localPos /= abs.Y;
          normalCoord.X = -localPos.Z;
          normalCoord.Y = localPos.X;
          break;
        default:
          normalCoord = Vector2D.Zero;
          break;
      }
    }

    public static void GetForwardUp(
      Base6Directions.Direction axis,
      out Vector3D forward,
      out Vector3D up)
    {
      forward = (Vector3D) Base6Directions.Directions[(int) axis];
      up = (Vector3D) Base6Directions.Directions[(int) Base6Directions.GetPerpendicular(axis)];
    }

    public static unsafe void TranslateTexcoordsToFace(
      ref Vector2D texcoords,
      int originalFace,
      int myFace,
      out Vector2D newCoords)
    {
      Vector2D vector2D = texcoords;
      if ((originalFace & -2) != (myFace & -2))
      {
        uint adjacentFaceTransform = MyPlanetCubemapHelper.AdjacentFaceTransforms[myFace * 6 + originalFace];
        double* numPtr = (double*) &vector2D;
        if (((int) adjacentFaceTransform & 1) != ((int) (adjacentFaceTransform >> 1) & 1))
        {
          double num = *numPtr;
          *numPtr = numPtr[1];
          numPtr[1] = num;
        }
        uint num1 = adjacentFaceTransform >> 1 & 1U;
        if (((int) (adjacentFaceTransform >> 2) & 1) != 0)
          *(double*) ((IntPtr) numPtr + (IntPtr) ((long) num1 * 8L)) = -*(double*) ((IntPtr) numPtr + (IntPtr) ((long) num1 * 8L));
        if (((int) (adjacentFaceTransform >> 3) & 1) != 0)
          *(double*) ((IntPtr) numPtr + (IntPtr) ((long) (1U ^ num1) * 8L)) = -*(double*) ((IntPtr) numPtr + (IntPtr) ((long) (1U ^ num1) * 8L));
        if (((int) (adjacentFaceTransform >> 4) & 1) != 0)
        {
          IntPtr num2 = (IntPtr) numPtr + (IntPtr) ((long) num1 * 8L);
          *(double*) num2 = *(double*) num2 - 2.0;
        }
        else
        {
          IntPtr num2 = (IntPtr) numPtr + (IntPtr) ((long) num1 * 8L);
          *(double*) num2 = *(double*) num2 + 2.0;
        }
      }
      newCoords = vector2D;
    }
  }
}
