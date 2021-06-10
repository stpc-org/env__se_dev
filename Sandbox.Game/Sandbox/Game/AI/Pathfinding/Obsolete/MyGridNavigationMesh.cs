// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyGridNavigationMesh
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyGridNavigationMesh : MyNavigationMesh
  {
    private readonly MyCubeGrid m_grid;
    private readonly Dictionary<Vector3I, List<int>> m_smallTriangleRegistry;
    private MyVector3ISet m_cubeSet;
    private Dictionary<MyGridNavigationMesh.EdgeIndex, int> m_connectionHelper;
    private readonly MyNavmeshCoordinator m_coordinator;
    private readonly MyHighLevelGroup m_higherLevel;
    private readonly MyGridHighLevelHelper m_higherLevelHelper;
    private MyGridNavigationMesh.Component m_component;
    private static readonly HashSet<Vector3I> m_mergeHelper = new HashSet<Vector3I>();
    private static readonly List<KeyValuePair<MyNavigationTriangle, Vector3I>> m_tmpTriangleList = new List<KeyValuePair<MyNavigationTriangle, Vector3I>>();
    private bool m_static;

    public bool HighLevelDirty => this.m_higherLevelHelper.IsDirty;

    public MyGridNavigationMesh(
      MyCubeGrid grid,
      MyNavmeshCoordinator coordinator,
      int triPrealloc = 32,
      Func<long> timestampFunction = null)
      : base(coordinator?.Links, triPrealloc, timestampFunction)
    {
      this.m_connectionHelper = new Dictionary<MyGridNavigationMesh.EdgeIndex, int>();
      this.m_smallTriangleRegistry = new Dictionary<Vector3I, List<int>>();
      this.m_cubeSet = new MyVector3ISet();
      this.m_coordinator = coordinator;
      this.m_static = false;
      if (grid == null)
        return;
      this.m_higherLevel = new MyHighLevelGroup((IMyNavigationGroup) this, coordinator.HighLevelLinks, timestampFunction);
      this.m_higherLevelHelper = new MyGridHighLevelHelper(this, this.m_smallTriangleRegistry, new Vector3I(8, 8, 8));
      this.m_grid = grid;
      grid.OnBlockAdded += new Action<MySlimBlock>(this.grid_OnBlockAdded);
      grid.OnBlockRemoved += new Action<MySlimBlock>(this.grid_OnBlockRemoved);
      float num = 1f / (float) grid.CubeBlocks.Count;
      Vector3 zero = Vector3.Zero;
      foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
      {
        this.OnBlockAddedInternal(cubeBlock);
        zero += cubeBlock.Position * grid.GridSize * num;
      }
    }

    public override string ToString() => "Grid NavMesh: " + this.m_grid.DisplayName;

    public void UpdateHighLevel() => this.m_higherLevelHelper.ProcessChangedCellComponents();

    public MyNavigationTriangle AddTriangle(
      ref Vector3 a,
      ref Vector3 b,
      ref Vector3 c)
    {
      return this.m_grid != null ? (MyNavigationTriangle) null : this.AddTriangleInternal(a, b, c);
    }

    private MyNavigationTriangle AddTriangleInternal(
      Vector3 a,
      Vector3 b,
      Vector3 c)
    {
      Vector3I vector3I1 = Vector3I.Round(a * 256f);
      Vector3I vector3I2 = Vector3I.Round(b * 256f);
      Vector3I vector3I3 = Vector3I.Round(c * 256f);
      Vector3 A = (Vector3) vector3I1 / 256f;
      Vector3 C = (Vector3) vector3I2 / 256f;
      Vector3 B = (Vector3) vector3I3 / 256f;
      int edgeCA;
      if (!this.m_connectionHelper.TryGetValue(new MyGridNavigationMesh.EdgeIndex(ref vector3I2, ref vector3I1), out edgeCA))
        edgeCA = -1;
      int edgeBC;
      if (!this.m_connectionHelper.TryGetValue(new MyGridNavigationMesh.EdgeIndex(ref vector3I3, ref vector3I2), out edgeBC))
        edgeBC = -1;
      int edgeAB;
      if (!this.m_connectionHelper.TryGetValue(new MyGridNavigationMesh.EdgeIndex(ref vector3I1, ref vector3I3), out edgeAB))
        edgeAB = -1;
      int num1 = edgeCA;
      int num2 = edgeBC;
      int num3 = edgeAB;
      MyNavigationTriangle navigationTriangle = this.AddTriangle(ref A, ref B, ref C, ref edgeAB, ref edgeBC, ref edgeCA);
      if (num1 == -1)
        this.m_connectionHelper.Add(new MyGridNavigationMesh.EdgeIndex(ref vector3I1, ref vector3I2), edgeCA);
      else
        this.m_connectionHelper.Remove(new MyGridNavigationMesh.EdgeIndex(ref vector3I2, ref vector3I1));
      if (num2 == -1)
        this.m_connectionHelper.Add(new MyGridNavigationMesh.EdgeIndex(ref vector3I2, ref vector3I3), edgeBC);
      else
        this.m_connectionHelper.Remove(new MyGridNavigationMesh.EdgeIndex(ref vector3I3, ref vector3I2));
      if (num3 == -1)
        this.m_connectionHelper.Add(new MyGridNavigationMesh.EdgeIndex(ref vector3I3, ref vector3I1), edgeAB);
      else
        this.m_connectionHelper.Remove(new MyGridNavigationMesh.EdgeIndex(ref vector3I1, ref vector3I3));
      return navigationTriangle;
    }

    public void RegisterTriangle(MyNavigationTriangle tri, ref Vector3I gridPos)
    {
      if (this.m_grid != null)
        return;
      this.RegisterTriangleInternal(tri, ref gridPos);
    }

    private void RegisterTriangleInternal(MyNavigationTriangle tri, ref Vector3I gridPos)
    {
      List<int> intList = (List<int>) null;
      if (!this.m_smallTriangleRegistry.TryGetValue(gridPos, out intList))
      {
        intList = new List<int>();
        this.m_smallTriangleRegistry.Add(gridPos, intList);
      }
      intList.Add(tri.Index);
      tri.Registered = true;
    }

    public MyVector3ISet.Enumerator GetCubes() => this.m_cubeSet.GetEnumerator();

    public void GetCubeTriangles(Vector3I gridPos, List<MyNavigationTriangle> trianglesOut)
    {
      List<int> intList = (List<int>) null;
      if (!this.m_smallTriangleRegistry.TryGetValue(gridPos, out intList))
        return;
      for (int index = 0; index < intList.Count; ++index)
        trianglesOut.Add(this.GetTriangle(intList[index]));
    }

    private void MergeFromAnotherMesh(MyGridNavigationMesh otherMesh, ref MatrixI transform)
    {
      MyGridNavigationMesh.m_mergeHelper.Clear();
      foreach (Vector3I key in otherMesh.m_smallTriangleRegistry.Keys)
      {
        bool flag = false;
        foreach (Vector3I intDirection in Base6Directions.IntDirections)
        {
          Vector3I position = Vector3I.Transform(key + intDirection, transform);
          if (this.m_cubeSet.Contains(ref position))
          {
            MyGridNavigationMesh.m_mergeHelper.Add(key + intDirection);
            flag = true;
          }
        }
        if (flag)
          MyGridNavigationMesh.m_mergeHelper.Add(key);
      }
      foreach (KeyValuePair<Vector3I, List<int>> keyValuePair in otherMesh.m_smallTriangleRegistry)
      {
        Vector3I key1 = keyValuePair.Key;
        Vector3I result;
        Vector3I.Transform(ref key1, ref transform, out result);
        if (MyGridNavigationMesh.m_mergeHelper.Contains(key1))
        {
          MyGridNavigationMesh.m_tmpTriangleList.Clear();
          foreach (Base6Directions.Direction enumDirection in Base6Directions.EnumDirections)
          {
            Vector3I intVector1 = Base6Directions.GetIntVector((int) enumDirection);
            Vector3I intVector2 = Base6Directions.GetIntVector((int) Base6Directions.GetFlippedDirection(transform.GetDirection(enumDirection)));
            if (MyGridNavigationMesh.m_mergeHelper.Contains(key1 + intVector1))
            {
              List<int> intList = (List<int>) null;
              if (this.m_smallTriangleRegistry.TryGetValue(result - intVector2, out intList))
              {
                foreach (int index in intList)
                {
                  MyNavigationTriangle triangle = this.GetTriangle(index);
                  if (this.IsFaceTriangle(triangle, result - intVector2, intVector2))
                    MyGridNavigationMesh.m_tmpTriangleList.Add(new KeyValuePair<MyNavigationTriangle, Vector3I>(triangle, result - intVector2));
                }
              }
            }
          }
          foreach (KeyValuePair<MyNavigationTriangle, Vector3I> tmpTriangle in MyGridNavigationMesh.m_tmpTriangleList)
            this.RemoveTriangle(tmpTriangle.Key, tmpTriangle.Value);
          MyGridNavigationMesh.m_tmpTriangleList.Clear();
          int num = 0;
          foreach (int index in keyValuePair.Value)
          {
            MyNavigationTriangle triangle = otherMesh.GetTriangle(index);
            Vector3I key2 = keyValuePair.Key;
            bool flag = true;
            foreach (int enumDirection in Base6Directions.EnumDirections)
            {
              Vector3I intVector = Base6Directions.GetIntVector(enumDirection);
              if (MyGridNavigationMesh.m_mergeHelper.Contains(key2 + intVector) && this.IsFaceTriangle(triangle, key2, intVector))
              {
                flag = false;
                break;
              }
            }
            if (flag)
            {
              this.CopyTriangle(triangle, key2, ref transform);
              ++num;
            }
          }
        }
        else
        {
          foreach (int index in keyValuePair.Value)
            this.CopyTriangle(otherMesh.GetTriangle(index), keyValuePair.Key, ref transform);
        }
      }
      MyGridNavigationMesh.m_mergeHelper.Clear();
    }

    private bool IsFaceTriangle(
      MyNavigationTriangle triangle,
      Vector3I cubePosition,
      Vector3I direction)
    {
      MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = triangle.GetVertexEnumerator();
      vertexEnumerator.MoveNext();
      Vector3I vector3I1 = Vector3I.Round(vertexEnumerator.Current * 256f);
      vertexEnumerator.MoveNext();
      Vector3I vector3I2 = Vector3I.Round(vertexEnumerator.Current * 256f);
      vertexEnumerator.MoveNext();
      Vector3I vector3I3 = Vector3I.Round(vertexEnumerator.Current * 256f);
      cubePosition *= 256;
      Vector3I vector3I4 = cubePosition + direction * 128;
      Vector3I vector3I5 = vector3I1 - vector3I4;
      Vector3I vector3I6 = vector3I2 - vector3I4;
      Vector3I vector3I7 = vector3I3 - vector3I4;
      return !(vector3I5 * direction != Vector3I.Zero) && !(vector3I6 * direction != Vector3I.Zero) && (!(vector3I7 * direction != Vector3I.Zero) && vector3I5.AbsMax() <= 128) && vector3I6.AbsMax() <= 128 && vector3I7.AbsMax() <= 128;
    }

    private void RemoveTriangle(MyNavigationTriangle triangle, Vector3I cube)
    {
      MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = triangle.GetVertexEnumerator();
      vertexEnumerator.MoveNext();
      Vector3I vector3I1 = Vector3I.Round(vertexEnumerator.Current * 256f);
      vertexEnumerator.MoveNext();
      Vector3I vector3I2 = Vector3I.Round(vertexEnumerator.Current * 256f);
      vertexEnumerator.MoveNext();
      Vector3I vector3I3 = Vector3I.Round(vertexEnumerator.Current * 256f);
      int edgeIndex1 = triangle.GetEdgeIndex(0);
      int edgeIndex2 = triangle.GetEdgeIndex(1);
      int edgeIndex3 = triangle.GetEdgeIndex(2);
      int registeredEdgeIndex1;
      if (!this.m_connectionHelper.TryGetValue(new MyGridNavigationMesh.EdgeIndex(ref vector3I1, ref vector3I3), out registeredEdgeIndex1))
        registeredEdgeIndex1 = -1;
      int registeredEdgeIndex2;
      if (!this.m_connectionHelper.TryGetValue(new MyGridNavigationMesh.EdgeIndex(ref vector3I3, ref vector3I2), out registeredEdgeIndex2))
        registeredEdgeIndex2 = -1;
      int registeredEdgeIndex3;
      if (!this.m_connectionHelper.TryGetValue(new MyGridNavigationMesh.EdgeIndex(ref vector3I2, ref vector3I1), out registeredEdgeIndex3))
        registeredEdgeIndex3 = -1;
      if (registeredEdgeIndex1 != -1 && edgeIndex3 == registeredEdgeIndex1)
        this.m_connectionHelper.Remove(new MyGridNavigationMesh.EdgeIndex(ref vector3I1, ref vector3I3));
      else
        this.m_connectionHelper.Add(new MyGridNavigationMesh.EdgeIndex(vector3I3, vector3I1), edgeIndex3);
      if (registeredEdgeIndex2 != -1 && edgeIndex2 == registeredEdgeIndex2)
        this.m_connectionHelper.Remove(new MyGridNavigationMesh.EdgeIndex(ref vector3I3, ref vector3I2));
      else
        this.m_connectionHelper.Add(new MyGridNavigationMesh.EdgeIndex(vector3I2, vector3I3), edgeIndex2);
      if (registeredEdgeIndex3 != -1 && edgeIndex1 == registeredEdgeIndex3)
        this.m_connectionHelper.Remove(new MyGridNavigationMesh.EdgeIndex(ref vector3I2, ref vector3I1));
      else
        this.m_connectionHelper.Add(new MyGridNavigationMesh.EdgeIndex(vector3I1, vector3I2), edgeIndex1);
      List<int> list = (List<int>) null;
      this.m_smallTriangleRegistry.TryGetValue(cube, out list);
      for (int index = 0; index < list.Count; ++index)
      {
        if (list[index] == triangle.Index)
        {
          list.RemoveAtFast<int>(index);
          break;
        }
      }
      if (list.Count == 0)
        this.m_smallTriangleRegistry.Remove(cube);
      this.RemoveTriangle(triangle);
      if (registeredEdgeIndex1 != -1 && edgeIndex3 != registeredEdgeIndex1)
        this.RemoveAndAddTriangle(ref vector3I1, ref vector3I3, registeredEdgeIndex1);
      if (registeredEdgeIndex2 != -1 && edgeIndex2 != registeredEdgeIndex2)
        this.RemoveAndAddTriangle(ref vector3I3, ref vector3I2, registeredEdgeIndex2);
      if (registeredEdgeIndex3 == -1 || edgeIndex1 == registeredEdgeIndex3)
        return;
      this.RemoveAndAddTriangle(ref vector3I2, ref vector3I1, registeredEdgeIndex3);
    }

    private void RemoveAndAddTriangle(
      ref Vector3I positionA,
      ref Vector3I positionB,
      int registeredEdgeIndex)
    {
      MyNavigationTriangle edgeTriangle = this.GetEdgeTriangle(registeredEdgeIndex);
      MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = edgeTriangle.GetVertexEnumerator();
      vertexEnumerator.MoveNext();
      Vector3 current1 = vertexEnumerator.Current;
      vertexEnumerator.MoveNext();
      Vector3 current2 = vertexEnumerator.Current;
      vertexEnumerator.MoveNext();
      Vector3 current3 = vertexEnumerator.Current;
      Vector3I triangleCube = this.FindTriangleCube(edgeTriangle.Index, ref positionA, ref positionB);
      this.RemoveTriangle(edgeTriangle, triangleCube);
      this.RegisterTriangleInternal(this.AddTriangleInternal(current1, current3, current2), ref triangleCube);
    }

    private Vector3I FindTriangleCube(
      int triIndex,
      ref Vector3I edgePositionA,
      ref Vector3I edgePositionB)
    {
      Vector3I result1;
      Vector3I.Min(ref edgePositionA, ref edgePositionB, out result1);
      Vector3I result2;
      Vector3I.Max(ref edgePositionA, ref edgePositionB, out result2);
      Vector3I next = Vector3I.Round(new Vector3(result1) / 256f - Vector3.Half);
      Vector3I end = Vector3I.Round(new Vector3(result2) / 256f + Vector3.Half);
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref next, ref end);
      while (vector3IRangeIterator.IsValid())
      {
        List<int> intList;
        this.m_smallTriangleRegistry.TryGetValue(next, out intList);
        if (intList != null && intList.Contains(triIndex))
          return next;
        vector3IRangeIterator.GetNext(out next);
      }
      return Vector3I.Zero;
    }

    private void CopyTriangle(
      MyNavigationTriangle otherTri,
      Vector3I triPosition,
      ref MatrixI transform)
    {
      Vector3 newA;
      Vector3 newB;
      Vector3 newC;
      otherTri.GetTransformed(ref transform, out newA, out newB, out newC);
      if (MyPerGameSettings.NavmeshPresumesDownwardGravity)
      {
        Vector3 vector1 = Vector3.Cross(newC - newA, newB - newA);
        double num = (double) vector1.Normalize();
        if ((double) Vector3.Dot(vector1, Base6Directions.GetVector(Base6Directions.Direction.Up)) < 0.699999988079071)
          return;
      }
      Vector3I.Transform(ref triPosition, ref transform, out triPosition);
      this.RegisterTriangleInternal(this.AddTriangleInternal(newA, newC, newB), ref triPosition);
    }

    public void MakeStatic()
    {
      if (this.m_static)
        return;
      this.m_static = true;
      this.m_connectionHelper = (Dictionary<MyGridNavigationMesh.EdgeIndex, int>) null;
      this.m_cubeSet = (MyVector3ISet) null;
    }

    public List<Vector4D> FindPath(Vector3 start, Vector3 end)
    {
      start /= this.m_grid.GridSize;
      end /= this.m_grid.GridSize;
      float closestDistSq1 = float.PositiveInfinity;
      MyNavigationTriangle navigationTriangle1 = this.GetClosestNavigationTriangle(ref start, ref closestDistSq1);
      if (navigationTriangle1 == null)
        return (List<Vector4D>) null;
      float closestDistSq2 = float.PositiveInfinity;
      MyNavigationTriangle navigationTriangle2 = this.GetClosestNavigationTriangle(ref end, ref closestDistSq2);
      if (navigationTriangle2 == null)
        return (List<Vector4D>) null;
      List<Vector4D> refinedPath = this.FindRefinedPath(navigationTriangle1, navigationTriangle2, ref start, ref end);
      if (refinedPath != null)
      {
        for (int index = 0; index < refinedPath.Count; ++index)
        {
          Vector4D vector4D = refinedPath[index] * (double) this.m_grid.GridSize;
          refinedPath[index] = vector4D;
        }
      }
      return refinedPath;
    }

    private MyNavigationTriangle GetClosestNavigationTriangle(
      ref Vector3 point,
      ref float closestDistSq)
    {
      Vector3I r;
      Vector3I.Round(ref point, out r);
      MyNavigationTriangle navigationTriangle = (MyNavigationTriangle) null;
      Vector3I next = r - new Vector3I(4, 4, 4);
      Vector3I end = r + new Vector3I(4, 4, 4);
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref next, ref end);
      while (vector3IRangeIterator.IsValid())
      {
        List<int> intList;
        this.m_smallTriangleRegistry.TryGetValue(next, out intList);
        if (intList != null)
        {
          foreach (int index in intList)
          {
            MyNavigationTriangle triangle = this.GetTriangle(index);
            MyWingedEdgeMesh.FaceVertexEnumerator vertexEnumerator = triangle.GetVertexEnumerator();
            vertexEnumerator.MoveNext();
            Vector3 current1 = vertexEnumerator.Current;
            vertexEnumerator.MoveNext();
            Vector3 current2 = vertexEnumerator.Current;
            vertexEnumerator.MoveNext();
            Vector3 current3 = vertexEnumerator.Current;
            Vector3 vector2_1 = (current1 + current2 + current3) / 3f;
            Vector3 vector1_1 = current2 - current1;
            Vector3 vector3_1 = current3 - current2;
            float num1 = Vector3.DistanceSquared(vector2_1, point);
            if ((double) num1 < (double) vector1_1.LengthSquared() + (double) vector3_1.LengthSquared())
            {
              Vector3 vector1_2 = current1 - current3;
              Vector3 vector3_2 = Vector3.Cross(vector1_1, vector3_1);
              double num2 = (double) vector3_2.Normalize();
              Vector3 vector1_3 = Vector3.Cross(vector1_1, vector3_2);
              Vector3 vector1_4 = Vector3.Cross(vector3_1, vector3_2);
              Vector3 vector2_2 = vector3_2;
              Vector3 vector1_5 = Vector3.Cross(vector1_2, vector2_2);
              float num3 = -Vector3.Dot(vector1_3, current1);
              float num4 = -Vector3.Dot(vector1_4, current2);
              float num5 = -Vector3.Dot(vector1_5, current3);
              float num6 = Vector3.Dot(vector1_3, point) + num3;
              float num7 = Vector3.Dot(vector1_4, point) + num4;
              float num8 = Vector3.Dot(vector1_5, point) + num5;
              float num9 = Vector3.Dot(vector3_2, point) - Vector3.Dot(vector3_2, vector2_1);
              num1 = num9 * num9;
              if ((double) num6 > 0.0)
              {
                if ((double) num7 > 0.0)
                {
                  if ((double) num8 < 0.0)
                    num1 += num8 * num8;
                }
                else if ((double) num8 > 0.0)
                  num1 += num7 * num7;
                else
                  num1 += Vector3.DistanceSquared(current3, point);
              }
              else if ((double) num7 > 0.0)
              {
                if ((double) num8 > 0.0)
                  num1 += num6 * num6;
                else
                  num1 += Vector3.DistanceSquared(current1, point);
              }
              else if ((double) num8 > 0.0)
                num1 += Vector3.DistanceSquared(current2, point);
            }
            if ((double) num1 < (double) closestDistSq)
            {
              navigationTriangle = triangle;
              closestDistSq = num1;
            }
          }
        }
        vector3IRangeIterator.GetNext(out next);
      }
      return navigationTriangle;
    }

    public override MyNavigationPrimitive FindClosestPrimitive(
      Vector3D point,
      bool highLevel,
      ref double closestDistanceSq)
    {
      if (highLevel)
        return (MyNavigationPrimitive) null;
      Vector3 point1 = (Vector3) Vector3D.Transform(point, this.m_grid.PositionComp.WorldMatrixNormalizedInv) / this.m_grid.GridSize;
      float closestDistSq = (float) closestDistanceSq / this.m_grid.GridSize;
      MyNavigationTriangle navigationTriangle = this.GetClosestNavigationTriangle(ref point1, ref closestDistSq);
      if (navigationTriangle == null)
        return (MyNavigationPrimitive) navigationTriangle;
      closestDistanceSq = (double) closestDistSq * (double) this.m_grid.GridSize;
      return (MyNavigationPrimitive) navigationTriangle;
    }

    private void grid_OnBlockAdded(MySlimBlock block) => this.OnBlockAddedInternal(block);

    private void OnBlockAddedInternal(MySlimBlock block)
    {
      MyCompoundCubeBlock fatBlock = this.m_grid.GetCubeBlock(block.Position).FatBlock as MyCompoundCubeBlock;
      if (!(block.FatBlock is MyCompoundCubeBlock) && block.BlockDefinition.NavigationDefinition == null)
        return;
      bool flag1 = false;
      bool flag2 = false;
      if (fatBlock != null)
      {
        ListReader<MySlimBlock> blocks = fatBlock.GetBlocks();
        if (blocks.Count == 0)
          return;
        foreach (MySlimBlock mySlimBlock in blocks)
        {
          if (mySlimBlock.BlockDefinition.NavigationDefinition != null)
          {
            if (mySlimBlock.BlockDefinition.NavigationDefinition.NoEntry | flag2)
            {
              flag2 = false;
              flag1 = true;
              break;
            }
            block = mySlimBlock;
            flag2 = true;
          }
        }
      }
      else if (block.BlockDefinition.NavigationDefinition != null)
      {
        if (block.BlockDefinition.NavigationDefinition.NoEntry)
        {
          flag2 = false;
          flag1 = true;
        }
        else
          flag2 = true;
      }
      if (!flag1 && !flag2)
        return;
      if (flag1)
      {
        if (this.m_cubeSet.Contains(block.Position))
          this.RemoveBlock(block.Min, block.Max, true);
        Vector3I next = new Vector3I();
        for (next.X = block.Min.X; next.X <= block.Max.X; ++next.X)
        {
          for (next.Y = block.Min.Y; next.Y <= block.Max.Y; ++next.Y)
          {
            next.Z = block.Min.Z - 1;
            if (this.m_cubeSet.Contains(ref next))
              this.EraseFaceTriangles(next, Base6Directions.Direction.Backward);
            next.Z = block.Max.Z + 1;
            if (this.m_cubeSet.Contains(ref next))
              this.EraseFaceTriangles(next, Base6Directions.Direction.Forward);
          }
          for (next.Z = block.Min.Z; next.Z <= block.Max.Z; ++next.Z)
          {
            next.Y = block.Min.Y - 1;
            if (this.m_cubeSet.Contains(ref next))
              this.EraseFaceTriangles(next, Base6Directions.Direction.Up);
            next.Y = block.Max.Y + 1;
            if (this.m_cubeSet.Contains(ref next))
              this.EraseFaceTriangles(next, Base6Directions.Direction.Down);
          }
        }
        for (next.Y = block.Min.Y; next.Y <= block.Max.Y; ++next.Y)
        {
          for (next.Z = block.Min.Z; next.Z <= block.Max.Z; ++next.Z)
          {
            next.X = block.Min.X - 1;
            if (this.m_cubeSet.Contains(ref next))
              this.EraseFaceTriangles(next, Base6Directions.Direction.Right);
            next.X = block.Max.X + 1;
            if (this.m_cubeSet.Contains(ref next))
              this.EraseFaceTriangles(next, Base6Directions.Direction.Left);
          }
        }
        next = block.Min;
        Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref next, ref block.Max);
        while (vector3IRangeIterator.IsValid())
        {
          this.m_cubeSet.Add(next);
          vector3IRangeIterator.GetNext(out next);
        }
      }
      else
      {
        if (this.m_cubeSet.Contains(block.Position))
          this.RemoveBlock(block.Min, block.Max, true);
        this.AddBlock(block);
      }
      BoundingBoxD aabb;
      block.GetWorldBoundingBox(out aabb, false);
      aabb.Inflate(5.09999990463257);
      this.m_coordinator.InvalidateVoxelsBBox(ref aabb);
      this.MarkBlockChanged(block);
    }

    private void grid_OnBlockRemoved(MySlimBlock block)
    {
      bool flag1 = true;
      bool flag2 = false;
      bool flag3 = false;
      MySlimBlock cubeBlock = this.m_grid.GetCubeBlock(block.Position);
      MyCompoundCubeBlock fatBlock = cubeBlock?.FatBlock as MyCompoundCubeBlock;
      if (!(block.FatBlock is MyCompoundCubeBlock) && block.BlockDefinition.NavigationDefinition == null)
        return;
      if (fatBlock == null)
      {
        flag1 = false;
        if (cubeBlock != null)
        {
          if (block.BlockDefinition.NavigationDefinition.NoEntry)
            flag2 = true;
          else
            flag3 = true;
        }
      }
      else
      {
        ListReader<MySlimBlock> blocks = fatBlock.GetBlocks();
        if (blocks.Count != 0)
        {
          foreach (MySlimBlock mySlimBlock in blocks)
          {
            if (mySlimBlock.BlockDefinition.NavigationDefinition != null)
            {
              if (mySlimBlock.BlockDefinition.NavigationDefinition.NoEntry | flag3)
              {
                flag1 = false;
                flag2 = true;
                break;
              }
              flag1 = false;
              flag3 = true;
              block = mySlimBlock;
            }
          }
        }
      }
      BoundingBoxD aabb;
      block.GetWorldBoundingBox(out aabb, false);
      aabb.Inflate(5.09999990463257);
      this.m_coordinator.InvalidateVoxelsBBox(ref aabb);
      this.MarkBlockChanged(block);
      MyCestmirPathfindingShorts.Pathfinding.GridPathfinding.MarkHighLevelDirty();
      if (flag1)
      {
        this.RemoveBlock(block.Min, block.Max, true);
        this.FixBlockFaces(block);
      }
      else if (flag2)
        this.RemoveBlock(block.Min, block.Max, false);
      else if (flag3)
      {
        this.RemoveBlock(block.Min, block.Max, true);
        this.AddBlock(block);
      }
      else
      {
        if (!this.m_cubeSet.Contains(block.Position))
          return;
        this.RemoveBlock(block.Min, block.Max, true);
        this.FixBlockFaces(block);
      }
    }

    private void MarkBlockChanged(MySlimBlock block)
    {
      this.m_higherLevelHelper.MarkBlockChanged(block);
      MyCestmirPathfindingShorts.Pathfinding.GridPathfinding.MarkHighLevelDirty();
    }

    private void AddBlock(MySlimBlock block)
    {
      Vector3I next = block.Min;
      Vector3I max = block.Max;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref next, ref max);
      while (vector3IRangeIterator.IsValid())
      {
        this.m_cubeSet.Add(ref next);
        vector3IRangeIterator.GetNext(out next);
      }
      MatrixI transform = new MatrixI(block.Position, block.Orientation.Forward, block.Orientation.Up);
      this.MergeFromAnotherMesh(block.BlockDefinition.NavigationDefinition.Mesh, ref transform);
    }

    private void RemoveBlock(Vector3I min, Vector3I max, bool eraseCubeSet)
    {
      Vector3I next = min;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref next, ref max);
      while (vector3IRangeIterator.IsValid())
      {
        if (eraseCubeSet)
          this.m_cubeSet.Remove(ref next);
        this.EraseCubeTriangles(next);
        vector3IRangeIterator.GetNext(out next);
      }
    }

    private void EraseCubeTriangles(Vector3I pos)
    {
      List<int> intList;
      if (!this.m_smallTriangleRegistry.TryGetValue(pos, out intList))
        return;
      MyGridNavigationMesh.m_tmpTriangleList.Clear();
      foreach (int index in intList)
      {
        MyNavigationTriangle triangle = this.GetTriangle(index);
        MyGridNavigationMesh.m_tmpTriangleList.Add(new KeyValuePair<MyNavigationTriangle, Vector3I>(triangle, pos));
      }
      foreach (KeyValuePair<MyNavigationTriangle, Vector3I> tmpTriangle in MyGridNavigationMesh.m_tmpTriangleList)
        this.RemoveTriangle(tmpTriangle.Key, tmpTriangle.Value);
      MyGridNavigationMesh.m_tmpTriangleList.Clear();
      this.m_smallTriangleRegistry.Remove(pos);
    }

    private void EraseFaceTriangles(Vector3I pos, Base6Directions.Direction direction)
    {
      MyGridNavigationMesh.m_tmpTriangleList.Clear();
      Vector3I intVector = Base6Directions.GetIntVector((int) direction);
      List<int> intList = (List<int>) null;
      if (this.m_smallTriangleRegistry.TryGetValue(pos, out intList))
      {
        foreach (int index in intList)
        {
          MyNavigationTriangle triangle = this.GetTriangle(index);
          if (this.IsFaceTriangle(triangle, pos, intVector))
            MyGridNavigationMesh.m_tmpTriangleList.Add(new KeyValuePair<MyNavigationTriangle, Vector3I>(triangle, pos));
        }
      }
      foreach (KeyValuePair<MyNavigationTriangle, Vector3I> tmpTriangle in MyGridNavigationMesh.m_tmpTriangleList)
        this.RemoveTriangle(tmpTriangle.Key, tmpTriangle.Value);
      MyGridNavigationMesh.m_tmpTriangleList.Clear();
    }

    private void FixBlockFaces(MySlimBlock block)
    {
      Vector3I pos;
      Vector3I dir;
      for (pos.X = block.Min.X; pos.X <= block.Max.X; ++pos.X)
      {
        for (pos.Y = block.Min.Y; pos.Y <= block.Max.Y; ++pos.Y)
        {
          dir = Vector3I.Backward;
          pos.Z = block.Min.Z - 1;
          this.FixCubeFace(ref pos, ref dir);
          dir = Vector3I.Forward;
          pos.Z = block.Max.Z + 1;
          this.FixCubeFace(ref pos, ref dir);
        }
      }
      for (pos.X = block.Min.X; pos.X <= block.Max.X; ++pos.X)
      {
        for (pos.Z = block.Min.Z; pos.Z <= block.Max.Z; ++pos.Z)
        {
          dir = Vector3I.Up;
          pos.Y = block.Min.Y - 1;
          this.FixCubeFace(ref pos, ref dir);
          dir = Vector3I.Down;
          pos.Y = block.Max.Y + 1;
          this.FixCubeFace(ref pos, ref dir);
        }
      }
      for (pos.Y = block.Min.Y; pos.Y <= block.Max.Y; ++pos.Y)
      {
        for (pos.Z = block.Min.Z; pos.Z <= block.Max.Z; ++pos.Z)
        {
          dir = Vector3I.Right;
          pos.X = block.Min.X - 1;
          this.FixCubeFace(ref pos, ref dir);
          dir = Vector3I.Left;
          pos.X = block.Max.X + 1;
          this.FixCubeFace(ref pos, ref dir);
        }
      }
    }

    private void FixCubeFace(ref Vector3I pos, ref Vector3I dir)
    {
      if (!this.m_cubeSet.Contains(ref pos))
        return;
      MySlimBlock mySlimBlock1 = this.m_grid.GetCubeBlock(pos);
      if (mySlimBlock1.FatBlock is MyCompoundCubeBlock fatBlock)
      {
        ListReader<MySlimBlock> blocks = fatBlock.GetBlocks();
        MySlimBlock mySlimBlock2 = (MySlimBlock) null;
        foreach (MySlimBlock mySlimBlock3 in blocks)
        {
          if (mySlimBlock3.BlockDefinition.NavigationDefinition != null)
          {
            mySlimBlock2 = mySlimBlock3;
            break;
          }
        }
        if (mySlimBlock2 != null)
          mySlimBlock1 = mySlimBlock2;
      }
      if (mySlimBlock1.BlockDefinition.NavigationDefinition == null)
        return;
      MatrixI matrixI = new MatrixI(mySlimBlock1.Position, mySlimBlock1.Orientation.Forward, mySlimBlock1.Orientation.Up);
      MatrixI result1;
      MatrixI.Invert(ref matrixI, out result1);
      Vector3I result2;
      Vector3I.Transform(ref pos, ref result1, out result2);
      Vector3I result3;
      Vector3I.TransformNormal(ref dir, ref result1, out result3);
      MyGridNavigationMesh mesh = mySlimBlock1.BlockDefinition.NavigationDefinition.Mesh;
      List<int> intList;
      if (mesh == null || !mesh.m_smallTriangleRegistry.TryGetValue(result2, out intList))
        return;
      foreach (int index in intList)
      {
        MyNavigationTriangle triangle = mesh.GetTriangle(index);
        if (this.IsFaceTriangle(triangle, result2, result3))
          this.CopyTriangle(triangle, result2, ref matrixI);
      }
    }

    public override MatrixD GetWorldMatrix()
    {
      MatrixD worldMatrix = this.m_grid.WorldMatrix;
      MatrixD.Rescale(ref worldMatrix, this.m_grid.GridSize);
      return worldMatrix;
    }

    public override Vector3 GlobalToLocal(Vector3D globalPos) => (Vector3) Vector3D.Transform(globalPos, this.m_grid.PositionComp.WorldMatrixNormalizedInv) / this.m_grid.GridSize;

    public override Vector3D LocalToGlobal(Vector3 localPos)
    {
      localPos *= this.m_grid.GridSize;
      return Vector3D.Transform(localPos, this.m_grid.WorldMatrix);
    }

    public override MyHighLevelGroup HighLevelGroup => this.m_higherLevel;

    public override MyHighLevelPrimitive GetHighLevelPrimitive(
      MyNavigationPrimitive myNavigationTriangle)
    {
      return this.m_higherLevelHelper.GetHighLevelNavigationPrimitive(myNavigationTriangle as MyNavigationTriangle);
    }

    public override IMyHighLevelComponent GetComponent(
      MyHighLevelPrimitive highLevelPrimitive)
    {
      return (IMyHighLevelComponent) new MyGridNavigationMesh.Component(this, highLevelPrimitive.Index);
    }

    public override void DebugDraw(ref Matrix drawMatrix)
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
        return;
      if ((MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.EDGES) != MyWEMDebugDrawMode.NONE && this.m_connectionHelper != null)
      {
        foreach (KeyValuePair<MyGridNavigationMesh.EdgeIndex, int> keyValuePair in this.m_connectionHelper)
          MyRenderProxy.DebugDrawLine3D((Vector3D) Vector3.Transform(keyValuePair.Key.A / 256f, drawMatrix), (Vector3D) Vector3.Transform(keyValuePair.Key.B / 256f, drawMatrix), Color.Red, Color.Yellow, false);
      }
      if ((MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES & MyWEMDebugDrawMode.NORMALS) != MyWEMDebugDrawMode.NONE)
      {
        foreach (KeyValuePair<Vector3I, List<int>> keyValuePair in this.m_smallTriangleRegistry)
        {
          foreach (int index in keyValuePair.Value)
          {
            MyNavigationTriangle triangle = this.GetTriangle(index);
            Vector3 vector3 = Vector3.Transform(triangle.Center + triangle.Normal * 0.2f, drawMatrix);
            MyRenderProxy.DebugDrawLine3D((Vector3D) Vector3.Transform(triangle.Center, drawMatrix), (Vector3D) vector3, Color.Blue, Color.Blue, true);
          }
        }
      }
      if (!MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY)
        return;
      this.m_higherLevel?.DebugDraw(MyFakes.DEBUG_DRAW_NAVMESH_HIERARCHY_LITE);
    }

    private struct EdgeIndex : IEquatable<MyGridNavigationMesh.EdgeIndex>
    {
      public Vector3I A;
      public Vector3I B;

      public EdgeIndex(Vector3I PointA, Vector3I PointB)
      {
        this.A = PointA;
        this.B = PointB;
      }

      public EdgeIndex(ref Vector3I PointA, ref Vector3I PointB)
      {
        this.A = PointA;
        this.B = PointB;
      }

      public override int GetHashCode() => this.A.GetHashCode() * 1610612741 + this.B.GetHashCode();

      public override bool Equals(object obj) => obj is MyGridNavigationMesh.EdgeIndex other && this.Equals(other);

      public override string ToString() => "(" + (object) this.A + ", " + (object) this.B + ")";

      public bool Equals(MyGridNavigationMesh.EdgeIndex other) => this.A == other.A && this.B == other.B;
    }

    public class Component : IMyHighLevelComponent
    {
      private readonly MyGridNavigationMesh m_parent;
      private readonly int m_componentIndex;

      public Component(MyGridNavigationMesh parent, int componentIndex)
      {
        this.m_parent = parent;
        this.m_componentIndex = componentIndex;
      }

      public bool Contains(MyNavigationPrimitive primitive) => primitive.Group == this.m_parent && primitive is MyNavigationTriangle navigationTriangle && navigationTriangle.ComponentIndex == this.m_componentIndex;

      public bool IsFullyExplored => true;
    }
  }
}
