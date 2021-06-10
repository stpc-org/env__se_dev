// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyBlockVerticesCache
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  internal class MyBlockVerticesCache
  {
    private const bool ADD_INNER_BONES_TO_CONVEX = false;
    private static List<Vector3>[][] Cache;

    static MyBlockVerticesCache() => MyBlockVerticesCache.GenerateConvexVertices();

    public static ListReader<Vector3> GetBlockVertices(
      MyCubeTopology topology,
      MyBlockOrientation orientation)
    {
      return new ListReader<Vector3>(MyBlockVerticesCache.Cache[(int) topology][(int) ((byte) ((int) orientation.Forward * 6) + orientation.Up)]);
    }

    private static void GenerateConvexVertices()
    {
      List<Vector3> verts = new List<Vector3>(27);
      Array values = Enum.GetValues(typeof (MyCubeTopology));
      MyBlockVerticesCache.Cache = new List<Vector3>[values.Length][];
      foreach (MyCubeTopology topology in values)
      {
        MyBlockVerticesCache.GetTopologySwitch(topology, verts);
        MyBlockVerticesCache.Cache[(int) topology] = new List<Vector3>[36];
        foreach (Base6Directions.Direction enumDirection1 in Base6Directions.EnumDirections)
        {
          foreach (Base6Directions.Direction enumDirection2 in Base6Directions.EnumDirections)
          {
            if (enumDirection1 != enumDirection2 && !(Base6Directions.GetIntVector(enumDirection1) == -Base6Directions.GetIntVector(enumDirection2)))
            {
              List<Vector3> vector3List = new List<Vector3>(verts.Count);
              MyBlockVerticesCache.Cache[(int) topology][(int) ((byte) ((int) enumDirection1 * 6) + enumDirection2)] = vector3List;
              MyBlockOrientation orientation = new MyBlockOrientation(enumDirection1, enumDirection2);
              foreach (Vector3 normal in verts)
                vector3List.Add(Vector3.TransformNormal(normal, orientation));
            }
          }
        }
        verts.Clear();
      }
    }

    public static void GetTopologySwitch(MyCubeTopology topology, List<Vector3> verts)
    {
      if (topology == MyCubeTopology.CornerSquareInverted)
      {
        verts.Add(new Vector3(-1f, 1f, -1f));
        verts.Add(new Vector3(1f, -1f, 1f));
        verts.Add(new Vector3(-1f, 1f, 1f));
        verts.Add(new Vector3(-1f, -1f, 1f));
        verts.Add(new Vector3(-1f, -1f, -1f));
        verts.Add(new Vector3(-1f, 1f, -1f));
        verts.Add(new Vector3(1f, -1f, 1f));
        verts.Add(new Vector3(1f, 1f, -1f));
        verts.Add(new Vector3(1f, -1f, -1f));
        verts.Add(new Vector3(-1f, -1f, -1f));
      }
      else
      {
        switch (topology)
        {
          case MyCubeTopology.Box:
          case MyCubeTopology.RoundedSlope:
            verts.Add(new Vector3(1f, 1f, 1f));
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(-1f, 1f, 1f));
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            break;
          case MyCubeTopology.Slope:
          case MyCubeTopology.RotatedSlope:
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            break;
          case MyCubeTopology.Corner:
          case MyCubeTopology.RotatedCorner:
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            break;
          case MyCubeTopology.InvCorner:
            verts.Add(new Vector3(1f, 1f, 1f));
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(-1f, 1f, 1f));
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            break;
          case MyCubeTopology.RoundSlope:
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(-1f, 0.414f, 0.414f));
            verts.Add(new Vector3(1f, 0.414f, 0.414f));
            break;
          case MyCubeTopology.RoundCorner:
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(-0.414f, 0.414f, -1f));
            verts.Add(new Vector3(-0.414f, -1f, 0.414f));
            verts.Add(new Vector3(1f, 0.414f, 0.414f));
            break;
          case MyCubeTopology.RoundInvCorner:
            verts.Add(new Vector3(1f, 1f, 1f));
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(-1f, 1f, 1f));
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(0.414f, -0.414f, -1f));
            verts.Add(new Vector3(0.414f, -1f, -0.414f));
            verts.Add(new Vector3(1f, -0.414f, -0.414f));
            break;
          case MyCubeTopology.Slope2Base:
            verts.Add(new Vector3(1f, 0.0f, 1f));
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(-1f, 0.0f, 1f));
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            break;
          case MyCubeTopology.Slope2Tip:
            verts.Add(new Vector3(-1f, 0.0f, -1f));
            verts.Add(new Vector3(1f, 0.0f, -1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            break;
          case MyCubeTopology.Corner2Base:
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(1f, 0.0f, -1f));
            verts.Add(new Vector3(1f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            break;
          case MyCubeTopology.Corner2Tip:
            verts.Add(new Vector3(1f, 0.0f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(0.0f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            break;
          case MyCubeTopology.InvCorner2Base:
            verts.Add(new Vector3(1f, 1f, 1f));
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(1f, 0.0f, -1f));
            verts.Add(new Vector3(0.0f, -1f, -1f));
            verts.Add(new Vector3(-1f, 1f, 1f));
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            break;
          case MyCubeTopology.InvCorner2Tip:
            verts.Add(new Vector3(1f, 1f, 1f));
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(-1f, 1f, 1f));
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(1f, 0.0f, 1f));
            verts.Add(new Vector3(0.0f, -1f, 1f));
            break;
          case MyCubeTopology.HalfBox:
            verts.Add(new Vector3(1f, 1f, 0.0f));
            verts.Add(new Vector3(1f, -1f, 0.0f));
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(-1f, 1f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            break;
          case MyCubeTopology.HalfSlopeBox:
            verts.Add(new Vector3(-1f, 0.0f, -1f));
            verts.Add(new Vector3(1f, 0.0f, -1f));
            verts.Add(new Vector3(-1f, -1f, 0.0f));
            verts.Add(new Vector3(1f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            break;
          case MyCubeTopology.HalfSlopeInverted:
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, 0.0f, 1f));
            verts.Add(new Vector3(-1f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(-1f, 0.0f, -1f));
            verts.Add(new Vector3(-1f, 1f, 1f));
            verts.Add(new Vector3(-1f, 1f, 0.0f));
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(0.0f, -1f, 1f));
            verts.Add(new Vector3(1f, -1f, 0.0f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(0.0f, -1f, -1f));
            verts.Add(new Vector3(0.0f, 1f, 1f));
            verts.Add(new Vector3(1f, 0.0f, 1f));
            verts.Add(new Vector3(0.0f, 1f, -1f));
            verts.Add(new Vector3(1f, 0.0f, -1f));
            break;
          case MyCubeTopology.HalfSlopeCorner:
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(0.0f, -1f, 1f));
            verts.Add(new Vector3(1f, -1f, 0.0f));
            verts.Add(new Vector3(1f, 0.0f, 1f));
            break;
          case MyCubeTopology.HalfSlopeCornerInverted:
            verts.Add(new Vector3(0.0f, 1f, -1f));
            verts.Add(new Vector3(-1f, 0.0f, -1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(0.0f, -1f, -1f));
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(1f, 0.0f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(-0.5f, 0.5f, -1f));
            verts.Add(new Vector3(-1f, 1f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, 1f, 1f));
            verts.Add(new Vector3(-1f, 0.0f, 1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, 0.5f, -0.5f));
            verts.Add(new Vector3(1f, 1f, 0.0f));
            verts.Add(new Vector3(0.0f, 1f, 1f));
            verts.Add(new Vector3(1f, 1f, 1f));
            verts.Add(new Vector3(-0.5f, 1f, -0.5f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(0.0f, -1f, 1f));
            verts.Add(new Vector3(1f, -1f, 0.0f));
            verts.Add(new Vector3(1f, 0.0f, 1f));
            break;
          case MyCubeTopology.SlopedCornerTip:
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, 0.0f, 1f));
            verts.Add(new Vector3(0.0f, -0.5f, 1f));
            verts.Add(new Vector3(-1f, -0.5f, 1f));
            verts.Add(new Vector3(0.0f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(0.0f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, -0.5f, 0.0f));
            break;
          case MyCubeTopology.SlopedCornerBase:
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(0.0f, 1f, -1f));
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(1f, 1f, 0.0f));
            verts.Add(new Vector3(0.0f, 1f, 0.0f));
            verts.Add(new Vector3(1f, 1f, 1f));
            verts.Add(new Vector3(-1f, 0.0f, 1f));
            verts.Add(new Vector3(-1f, 0.5f, 0.0f));
            verts.Add(new Vector3(0.0f, 0.5f, 1f));
            verts.Add(new Vector3(1f, 0.0f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, 0.0f));
            verts.Add(new Vector3(1f, 0.0f, 1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(-1f, 0.0f, -1f));
            verts.Add(new Vector3(0.0f, -1f, -1f));
            verts.Add(new Vector3(0.0f, -1f, 1f));
            break;
          case MyCubeTopology.SlopedCorner:
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(1f, 0.0f, -1f));
            verts.Add(new Vector3(-1f, 0.0f, 1f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(1f, -0.5f, 0.0f));
            verts.Add(new Vector3(0.0f, 0.5f, -1f));
            verts.Add(new Vector3(-1f, 0.5f, 0.0f));
            verts.Add(new Vector3(0.0f, -0.5f, 1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(-1f, 0.0f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, 0.0f));
            verts.Add(new Vector3(0.0f, -1f, 1f));
            verts.Add(new Vector3(0.0f, -1f, -1f));
            break;
          case MyCubeTopology.HalfSlopedCornerBase:
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, 0.0f, 1f));
            verts.Add(new Vector3(0.0f, -0.5f, 1f));
            verts.Add(new Vector3(0.0f, -1f, 1f));
            verts.Add(new Vector3(1f, 0.0f, -1f));
            verts.Add(new Vector3(1f, -0.5f, 0.0f));
            verts.Add(new Vector3(0.0f, 0.0f, 0.0f));
            verts.Add(new Vector3(-1f, 0.0f, -1f));
            verts.Add(new Vector3(-1f, 0.0f, 0.0f));
            verts.Add(new Vector3(0.0f, 0.0f, -1f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(0.0f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, 0.0f));
            break;
          case MyCubeTopology.HalfCorner:
            verts.Add(new Vector3(-1f, 0.0f, -1f));
            verts.Add(new Vector3(1f, 0.0f, -1f));
            verts.Add(new Vector3(-1f, 0.0f, 1f));
            verts.Add(new Vector3(-1f, 0.0f, 0.0f));
            verts.Add(new Vector3(0.0f, 0.0f, 0.0f));
            verts.Add(new Vector3(0.0f, 0.0f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(0.0f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(0.0f, -1f, -1f));
            verts.Add(new Vector3(-1f, -1f, 0.0f));
            break;
          case MyCubeTopology.CornerSquare:
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(0.0f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(-1f, 0.0f, -1f));
            verts.Add(new Vector3(0.0f, 0.0f, -1f));
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(-1f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, 0.0f, 0.0f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(0.0f, 0.0f, 0.0f));
            verts.Add(new Vector3(1f, -1f, 0.0f));
            verts.Add(new Vector3(0.0f, -1f, 1f));
            break;
          case MyCubeTopology.CornerSquareInverted:
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(1f, -1f, 0.0f));
            verts.Add(new Vector3(1f, -1f, 1f));
            verts.Add(new Vector3(1f, 0.0f, -1f));
            verts.Add(new Vector3(1f, 0.0f, 0.0f));
            verts.Add(new Vector3(1f, 1f, -1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, 0.0f, 1f));
            verts.Add(new Vector3(-1f, -1f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(-1f, 0.0f, -1f));
            verts.Add(new Vector3(-1f, 1f, 1f));
            verts.Add(new Vector3(-1f, 1f, 0.0f));
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(0.0f, -1f, 1f));
            verts.Add(new Vector3(0.0f, 0.0f, 1f));
            verts.Add(new Vector3(0.0f, -1f, -1f));
            verts.Add(new Vector3(0.0f, 1f, -1f));
            verts.Add(new Vector3(0.0f, 0.0f, 0.0f));
            break;
          case MyCubeTopology.HalfSlopedCorner:
            verts.Add(new Vector3(1f, 0.0f, -1f));
            verts.Add(new Vector3(1f, -1f, -1f));
            verts.Add(new Vector3(-1f, -1f, 1f));
            verts.Add(new Vector3(-1f, 0.0f, 1f));
            verts.Add(new Vector3(0.0f, -1f, 0.0f));
            verts.Add(new Vector3(0.0f, 0.0f, 0.0f));
            verts.Add(new Vector3(-1f, 1f, -1f));
            verts.Add(new Vector3(0.0f, 0.5f, -1f));
            verts.Add(new Vector3(-1f, 0.5f, 0.0f));
            verts.Add(new Vector3(-1f, -1f, -1f));
            verts.Add(new Vector3(-1f, 0.0f, -1f));
            verts.Add(new Vector3(0.0f, -1f, -1f));
            verts.Add(new Vector3(-1f, -1f, 0.0f));
            break;
        }
      }
    }
  }
}
