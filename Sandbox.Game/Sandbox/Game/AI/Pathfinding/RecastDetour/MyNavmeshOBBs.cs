// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.RecastDetour.MyNavmeshOBBs
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
  public class MyNavmeshOBBs
  {
    private const int NEIGHBOUR_OVERLAP_TILES = 2;
    private MyOrientedBoundingBoxD?[][] m_obbs;
    private readonly float m_tileHalfSize;
    private readonly float m_tileHalfHeight;
    private readonly MyPlanet m_planet;
    private readonly int m_middleCoord;

    public int OBBsPerLine { get; }

    public MyOrientedBoundingBoxD BaseOBB { get; }

    public MyOrientedBoundingBoxD CenterOBB
    {
      get => this.m_obbs[this.m_middleCoord][this.m_middleCoord].Value;
      private set => this.m_obbs[this.m_middleCoord][this.m_middleCoord] = new MyOrientedBoundingBoxD?(value);
    }

    public List<Vector3D> NeighboursCenters { get; private set; }

    public MyNavmeshOBBs(
      MyPlanet planet,
      Vector3D centerPoint,
      Vector3D forwardDirection,
      int obbsPerLine,
      int tileSize,
      int tileHeight)
    {
      this.m_planet = planet;
      this.OBBsPerLine = obbsPerLine;
      if (this.OBBsPerLine % 2 == 0)
        this.OBBsPerLine = this.OBBsPerLine + 1;
      this.m_middleCoord = (this.OBBsPerLine - 1) / 2;
      this.m_tileHalfSize = (float) tileSize * 0.5f;
      this.m_tileHalfHeight = (float) tileHeight * 0.5f;
      this.m_obbs = new MyOrientedBoundingBoxD?[this.OBBsPerLine][];
      for (int index = 0; index < this.OBBsPerLine; ++index)
        this.m_obbs[index] = new MyOrientedBoundingBoxD?[this.OBBsPerLine];
      this.Initialize(centerPoint, forwardDirection);
      this.BaseOBB = this.GetBaseOBB();
    }

    public MyOrientedBoundingBoxD? GetOBB(int coordX, int coordY) => coordX < 0 || coordX >= this.OBBsPerLine || (coordY < 0 || coordY >= this.OBBsPerLine) ? new MyOrientedBoundingBoxD?() : this.m_obbs[coordX][coordY];

    public MyOrientedBoundingBoxD? GetOBB(Vector3D worldPosition)
    {
      foreach (MyOrientedBoundingBoxD?[] obb in this.m_obbs)
      {
        foreach (MyOrientedBoundingBoxD? nullable in obb)
        {
          if (nullable.Value.Contains(ref worldPosition))
            return nullable;
        }
      }
      return new MyOrientedBoundingBoxD?();
    }

    public MyNavmeshOBBs.OBBCoords? GetOBBCoord(int coordX, int coordY)
    {
      if (coordX < 0 || coordX >= this.OBBsPerLine || (coordY < 0 || coordY >= this.OBBsPerLine))
        return new MyNavmeshOBBs.OBBCoords?();
      return new MyNavmeshOBBs.OBBCoords?(new MyNavmeshOBBs.OBBCoords()
      {
        OBB = this.m_obbs[coordX][coordY].Value,
        Coords = new Vector2I(coordX, coordY)
      });
    }

    public MyNavmeshOBBs.OBBCoords? GetOBBCoord(Vector3D worldPosition)
    {
      for (int x = 0; x < this.m_obbs.Length; ++x)
      {
        for (int y = 0; y < this.m_obbs[0].Length; ++y)
        {
          MyOrientedBoundingBoxD orientedBoundingBoxD = this.m_obbs[x][y].Value;
          if (orientedBoundingBoxD.Contains(ref worldPosition))
            return new MyNavmeshOBBs.OBBCoords?(new MyNavmeshOBBs.OBBCoords()
            {
              OBB = orientedBoundingBoxD,
              Coords = new Vector2I(x, y)
            });
        }
      }
      return new MyNavmeshOBBs.OBBCoords?();
    }

    public List<MyNavmeshOBBs.OBBCoords> GetIntersectedOBB(LineD line)
    {
      Dictionary<MyNavmeshOBBs.OBBCoords, double> source = new Dictionary<MyNavmeshOBBs.OBBCoords, double>();
      for (int x = 0; x < this.m_obbs.Length; ++x)
      {
        for (int y = 0; y < this.m_obbs[0].Length; ++y)
        {
          MyOrientedBoundingBoxD orientedBoundingBoxD = this.m_obbs[x][y].Value;
          if (!orientedBoundingBoxD.Contains(ref line.From))
          {
            orientedBoundingBoxD = this.m_obbs[x][y].Value;
            if (!orientedBoundingBoxD.Contains(ref line.To))
            {
              orientedBoundingBoxD = this.m_obbs[x][y].Value;
              if (!orientedBoundingBoxD.Intersects(ref line).HasValue)
                continue;
            }
          }
          source.Add(new MyNavmeshOBBs.OBBCoords()
          {
            OBB = this.m_obbs[x][y].Value,
            Coords = new Vector2I(x, y)
          }, Vector3D.Distance(line.From, this.m_obbs[x][y].Value.Center));
        }
      }
      return source.OrderBy<KeyValuePair<MyNavmeshOBBs.OBBCoords, double>, double>((Func<KeyValuePair<MyNavmeshOBBs.OBBCoords, double>, double>) (d => d.Value)).Select<KeyValuePair<MyNavmeshOBBs.OBBCoords, double>, MyNavmeshOBBs.OBBCoords>((Func<KeyValuePair<MyNavmeshOBBs.OBBCoords, double>, MyNavmeshOBBs.OBBCoords>) (kvp => kvp.Key)).ToList<MyNavmeshOBBs.OBBCoords>();
    }

    public void DebugDraw()
    {
      for (int index1 = 0; index1 < this.m_obbs.Length; ++index1)
      {
        for (int index2 = 0; index2 < this.m_obbs[0].Length; ++index2)
        {
          if (this.m_obbs[index1][index2].HasValue)
            MyRenderProxy.DebugDrawOBB(this.m_obbs[index1][index2].Value, Color.Red, 0.0f, true, false);
        }
      }
      MyRenderProxy.DebugDrawOBB(this.BaseOBB, Color.White, 0.0f, true, false);
      if (this.m_obbs[0][0].HasValue)
        MyRenderProxy.DebugDrawSphere(this.m_obbs[0][0].Value.Center, 5f, Color.Yellow, 0.0f);
      if (this.m_obbs[0][this.OBBsPerLine - 1].HasValue)
        MyRenderProxy.DebugDrawSphere(this.m_obbs[0][this.OBBsPerLine - 1].Value.Center, 5f, Color.Green, 0.0f);
      if (this.m_obbs[this.OBBsPerLine - 1][this.OBBsPerLine - 1].HasValue)
        MyRenderProxy.DebugDrawSphere(this.m_obbs[this.OBBsPerLine - 1][this.OBBsPerLine - 1].Value.Center, 5f, Color.Blue, 0.0f);
      if (this.m_obbs[this.OBBsPerLine - 1][0].HasValue)
        MyRenderProxy.DebugDrawSphere(this.m_obbs[this.OBBsPerLine - 1][0].Value.Center, 5f, Color.White, 0.0f);
      MyOrientedBoundingBoxD? nullable1 = this.m_obbs[0][0];
      MyOrientedBoundingBoxD? nullable2 = this.m_obbs[this.OBBsPerLine - 1][0];
      MyOrientedBoundingBoxD? nullable3 = this.m_obbs[this.OBBsPerLine - 1][this.OBBsPerLine - 1];
      MyRenderProxy.DebugDrawSphere(MyNavmeshOBBs.GetOBBCorner(nullable1.Value, MyNavmeshOBBs.OBBCorner.LowerBackLeft), 5f, Color.White, 0.0f);
      MyRenderProxy.DebugDrawSphere(MyNavmeshOBBs.GetOBBCorner(nullable2.Value, MyNavmeshOBBs.OBBCorner.LowerBackRight), 5f, Color.White, 0.0f);
      MyRenderProxy.DebugDrawSphere(MyNavmeshOBBs.GetOBBCorner(nullable3.Value, MyNavmeshOBBs.OBBCorner.LowerFrontRight), 5f, Color.White, 0.0f);
    }

    public void Clear()
    {
      for (int index = 0; index < this.m_obbs.Length; ++index)
        Array.Clear((Array) this.m_obbs[index], 0, this.m_obbs.Length);
      Array.Clear((Array) this.m_obbs, 0, this.m_obbs.Length);
      this.m_obbs = (MyOrientedBoundingBoxD?[][]) null;
    }

    private void Initialize(Vector3D initialPoint, Vector3D forwardDirection)
    {
      double angle;
      this.CenterOBB = this.GetCenterOBB(initialPoint, forwardDirection, out angle);
      this.m_obbs[this.m_middleCoord][this.m_middleCoord] = new MyOrientedBoundingBoxD?(this.CenterOBB);
      this.Fill(angle);
      this.SetNeigboursCenter(angle * (double) Math.Max(2 * this.m_middleCoord - 1, 1));
    }

    private void Fill(double angle)
    {
      Vector2I currentIndex = new Vector2I(this.m_middleCoord, 0);
      for (int index = 0; index < this.OBBsPerLine; ++index)
      {
        this.FillOBBHorizontalLine(!this.m_obbs[currentIndex.Y][currentIndex.X].HasValue ? this.CreateOBB(this.NewTransformedPoint(this.CenterOBB.Center, this.CenterOBB.Orientation.Forward, (float) angle * (float) (index - this.m_middleCoord)), (Vector3D) this.CenterOBB.Orientation.Forward) : this.m_obbs[currentIndex.Y][currentIndex.X].Value, currentIndex, angle);
        ++currentIndex.Y;
      }
    }

    private void FillOBBHorizontalLine(
      MyOrientedBoundingBoxD lineCenterOBB,
      Vector2I currentIndex,
      double angle)
    {
      this.m_obbs[currentIndex.Y][currentIndex.X] = new MyOrientedBoundingBoxD?(lineCenterOBB);
      for (int index = 0; index < this.OBBsPerLine; ++index)
      {
        if (index != currentIndex.X)
        {
          MyOrientedBoundingBoxD obb = this.CreateOBB(this.NewTransformedPoint(lineCenterOBB.Center, lineCenterOBB.Orientation.Right, (float) angle * (float) (index - this.m_middleCoord)), (Vector3D) lineCenterOBB.Orientation.Right);
          this.m_obbs[currentIndex.Y][index] = new MyOrientedBoundingBoxD?(obb);
        }
      }
    }

    private MyOrientedBoundingBoxD GetCenterOBB(
      Vector3D initialPoint,
      Vector3D forwardDirection,
      out double angle)
    {
      Vector3D center = this.m_planet.PositionComp.WorldAABB.Center;
      double num = Math.Asin((double) this.m_tileHalfSize / (initialPoint - center).Length());
      angle = num * 2.0;
      return this.CreateOBB(initialPoint, forwardDirection);
    }

    private Vector3D NewTransformedPoint(
      Vector3D point,
      Vector3 rotationVector,
      float angle)
    {
      Vector3D center = this.m_planet.PositionComp.WorldAABB.Center;
      Quaternion fromAxisAngle = Quaternion.CreateFromAxisAngle(rotationVector, angle);
      return Vector3D.Transform(point - center, fromAxisAngle) + center;
    }

    private MyOrientedBoundingBoxD CreateOBB(
      Vector3D center,
      Vector3D perpedicularVector)
    {
      Vector3D vector3D = -Vector3D.Normalize((Vector3D) MyGravityProviderSystem.CalculateTotalGravityInPoint(center));
      return new MyOrientedBoundingBoxD(center, new Vector3D((double) this.m_tileHalfSize, (double) this.m_tileHalfHeight, (double) this.m_tileHalfSize), Quaternion.CreateFromForwardUp((Vector3) perpedicularVector, (Vector3) vector3D));
    }

    private MyOrientedBoundingBoxD GetBaseOBB()
    {
      MyOrientedBoundingBoxD? nullable1 = this.m_obbs[0][0];
      MyOrientedBoundingBoxD? nullable2 = this.m_obbs[this.OBBsPerLine - 1][0];
      MyOrientedBoundingBoxD? nullable3 = this.m_obbs[this.OBBsPerLine - 1][this.OBBsPerLine - 1];
      Vector3D obbCorner1 = MyNavmeshOBBs.GetOBBCorner(nullable1.Value, MyNavmeshOBBs.OBBCorner.LowerBackLeft);
      Vector3D obbCorner2 = MyNavmeshOBBs.GetOBBCorner(nullable2.Value, MyNavmeshOBBs.OBBCorner.LowerBackRight);
      Vector3D center = (obbCorner1 + MyNavmeshOBBs.GetOBBCorner(nullable3.Value, MyNavmeshOBBs.OBBCorner.LowerFrontRight)) / 2.0;
      double num = (obbCorner1 - obbCorner2).Length() / 2.0;
      double y = 0.01;
      return new MyOrientedBoundingBoxD(center, new Vector3D(num, y, num), this.CenterOBB.Orientation);
    }

    private void SetNeigboursCenter(double angle)
    {
      this.NeighboursCenters = new List<Vector3D>();
      Vector3D vector3D1 = this.NewTransformedPoint(this.CenterOBB.Center, this.CenterOBB.Orientation.Forward, (float) angle);
      Vector3D vector3D2 = this.NewTransformedPoint(this.CenterOBB.Center, this.CenterOBB.Orientation.Forward, (float) -angle);
      Vector3D point1 = this.NewTransformedPoint(this.CenterOBB.Center, this.CenterOBB.Orientation.Right, (float) angle);
      Vector3D point2 = this.NewTransformedPoint(this.CenterOBB.Center, this.CenterOBB.Orientation.Right, (float) -angle);
      this.NeighboursCenters.Add(vector3D1);
      this.NeighboursCenters.Add(vector3D2);
      this.NeighboursCenters.Add(point1);
      this.NeighboursCenters.Add(point2);
      Vector3D vector3D3 = this.NewTransformedPoint(point2, this.CenterOBB.Orientation.Forward, (float) -angle);
      Vector3D vector3D4 = this.NewTransformedPoint(point2, this.CenterOBB.Orientation.Forward, (float) angle);
      Vector3D vector3D5 = this.NewTransformedPoint(point1, this.CenterOBB.Orientation.Forward, (float) -angle);
      Vector3D vector3D6 = this.NewTransformedPoint(point1, this.CenterOBB.Orientation.Forward, (float) angle);
      this.NeighboursCenters.Add(vector3D3);
      this.NeighboursCenters.Add(vector3D4);
      this.NeighboursCenters.Add(vector3D5);
      this.NeighboursCenters.Add(vector3D6);
    }

    public static Vector3D GetOBBCorner(
      MyOrientedBoundingBoxD obb,
      MyNavmeshOBBs.OBBCorner corner)
    {
      Vector3D[] corners = new Vector3D[8];
      obb.GetCorners(corners, 0);
      return corners[(int) corner];
    }

    public struct OBBCoords
    {
      public Vector2I Coords;
      public MyOrientedBoundingBoxD OBB;
    }

    public enum OBBCorner
    {
      UpperFrontLeft,
      UpperBackLeft,
      LowerBackLeft,
      LowerFrontLeft,
      UpperFrontRight,
      UpperBackRight,
      LowerBackRight,
      LowerFrontRight,
    }
  }
}
