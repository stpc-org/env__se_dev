// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyVoxelConnectionHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRageMath;
using VRageMath.Spatial;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyVoxelConnectionHelper
  {
    private readonly Dictionary<MyVoxelConnectionHelper.InnerEdgeIndex, int> m_innerEdges = new Dictionary<MyVoxelConnectionHelper.InnerEdgeIndex, int>();
    private readonly MyVector3Grid<MyVoxelConnectionHelper.OuterEdgePoint> m_outerEdgePoints = new MyVector3Grid<MyVoxelConnectionHelper.OuterEdgePoint>(1f);
    private readonly Dictionary<int, int> m_innerMultiedges = new Dictionary<int, int>();
    private readonly Dictionary<MyVoxelConnectionHelper.InnerEdgeIndex, int> m_edgeClassifier = new Dictionary<MyVoxelConnectionHelper.InnerEdgeIndex, int>();
    private List<MyVoxelConnectionHelper.OuterEdgePoint> m_tmpOuterEdgePointList = new List<MyVoxelConnectionHelper.OuterEdgePoint>();
    public static float OUTER_EDGE_EPSILON = 0.05f;
    public static float OUTER_EDGE_EPSILON_SQ = MyVoxelConnectionHelper.OUTER_EDGE_EPSILON * MyVoxelConnectionHelper.OUTER_EDGE_EPSILON;

    public void ClearCell()
    {
      this.m_innerEdges.Clear();
      this.m_innerMultiedges.Clear();
      this.m_edgeClassifier.Clear();
    }

    public void PreprocessInnerEdge(ushort a, ushort b)
    {
      MyVoxelConnectionHelper.InnerEdgeIndex key1 = new MyVoxelConnectionHelper.InnerEdgeIndex(a, b);
      MyVoxelConnectionHelper.InnerEdgeIndex key2 = new MyVoxelConnectionHelper.InnerEdgeIndex(b, a);
      int num1;
      int num2 = this.m_edgeClassifier.TryGetValue(key1, out num1) ? num1 + 1 : 1;
      this.m_edgeClassifier[key1] = num2;
      if (!this.m_edgeClassifier.TryGetValue(key2, out num2))
        num2 = -1;
      else
        --num2;
      this.m_edgeClassifier[key2] = num2;
    }

    public bool IsInnerEdge(ushort v0, ushort v1) => this.IsInnerEdge(new MyVoxelConnectionHelper.InnerEdgeIndex(v0, v1));

    private bool IsInnerEdge(MyVoxelConnectionHelper.InnerEdgeIndex edgeIndex) => this.m_edgeClassifier[edgeIndex] == 0;

    public int TryGetAndRemoveEdgeIndex(
      ushort iv0,
      ushort iv1,
      ref Vector3 posv0,
      ref Vector3 posv1)
    {
      int edgeIndex = -1;
      MyVoxelConnectionHelper.InnerEdgeIndex innerEdgeIndex = new MyVoxelConnectionHelper.InnerEdgeIndex(iv0, iv1);
      if (this.IsInnerEdge(new MyVoxelConnectionHelper.InnerEdgeIndex(iv1, iv0)))
      {
        if (!this.m_innerEdges.TryGetValue(innerEdgeIndex, out edgeIndex))
          edgeIndex = -1;
        else
          this.RemoveInnerEdge(edgeIndex, innerEdgeIndex);
      }
      else
        this.TryRemoveOuterEdge(ref posv0, ref posv1, ref edgeIndex);
      return edgeIndex;
    }

    public void AddEdgeIndex(
      ushort iv0,
      ushort iv1,
      ref Vector3 posv0,
      ref Vector3 posv1,
      int edgeIndex)
    {
      MyVoxelConnectionHelper.InnerEdgeIndex innerEdgeIndex = new MyVoxelConnectionHelper.InnerEdgeIndex(iv0, iv1);
      if (this.IsInnerEdge(innerEdgeIndex))
      {
        int num;
        if (this.m_innerEdges.TryGetValue(innerEdgeIndex, out num))
        {
          this.m_innerMultiedges.Add(edgeIndex, num);
          this.m_innerEdges[innerEdgeIndex] = edgeIndex;
        }
        else
          this.m_innerEdges.Add(innerEdgeIndex, edgeIndex);
      }
      else
        this.AddOuterEdgeIndex(ref posv0, ref posv1, edgeIndex);
    }

    public void AddOuterEdgeIndex(ref Vector3 posv0, ref Vector3 posv1, int edgeIndex)
    {
      this.m_outerEdgePoints.AddPoint(ref posv0, new MyVoxelConnectionHelper.OuterEdgePoint(edgeIndex, true));
      this.m_outerEdgePoints.AddPoint(ref posv1, new MyVoxelConnectionHelper.OuterEdgePoint(edgeIndex, false));
    }

    public void FixOuterEdge(int edgeIndex, bool firstPoint, Vector3 currentPosition)
    {
      MyVoxelConnectionHelper.OuterEdgePoint outerEdgePoint = new MyVoxelConnectionHelper.OuterEdgePoint(edgeIndex, firstPoint);
      MyVector3Grid<MyVoxelConnectionHelper.OuterEdgePoint>.SphereQuery sphereQuery = this.m_outerEdgePoints.QueryPointsSphere(ref currentPosition, MyVoxelConnectionHelper.OUTER_EDGE_EPSILON * 3f);
      while (sphereQuery.MoveNext())
      {
        if (sphereQuery.Current.EdgeIndex == edgeIndex && sphereQuery.Current.FirstPoint == firstPoint)
          this.m_outerEdgePoints.MovePoint(sphereQuery.StorageIndex, ref currentPosition);
      }
    }

    private MyVoxelConnectionHelper.InnerEdgeIndex RemoveInnerEdge(
      int formerEdgeIndex,
      MyVoxelConnectionHelper.InnerEdgeIndex innerIndex)
    {
      int num;
      if (this.m_innerMultiedges.TryGetValue(formerEdgeIndex, out num))
      {
        this.m_innerMultiedges.Remove(formerEdgeIndex);
        this.m_innerEdges[innerIndex] = num;
      }
      else
        this.m_innerEdges.Remove(innerIndex);
      return innerIndex;
    }

    public bool TryRemoveOuterEdge(ref Vector3 posv0, ref Vector3 posv1, ref int edgeIndex)
    {
      if (edgeIndex == -1)
      {
        MyVector3Grid<MyVoxelConnectionHelper.OuterEdgePoint>.SphereQuery en0 = this.m_outerEdgePoints.QueryPointsSphere(ref posv0, MyVoxelConnectionHelper.OUTER_EDGE_EPSILON);
        while (en0.MoveNext())
        {
          MyVector3Grid<MyVoxelConnectionHelper.OuterEdgePoint>.SphereQuery en1 = this.m_outerEdgePoints.QueryPointsSphere(ref posv1, MyVoxelConnectionHelper.OUTER_EDGE_EPSILON);
          while (en1.MoveNext())
          {
            MyVoxelConnectionHelper.OuterEdgePoint current1 = en0.Current;
            MyVoxelConnectionHelper.OuterEdgePoint current2 = en1.Current;
            if (current1.EdgeIndex == current2.EdgeIndex && current1.FirstPoint && !current2.FirstPoint)
            {
              edgeIndex = current1.EdgeIndex;
              this.m_outerEdgePoints.RemoveTwo(ref en0, ref en1);
              return true;
            }
          }
        }
        edgeIndex = -1;
      }
      else
      {
        int num = 0;
        MyVector3Grid<MyVoxelConnectionHelper.OuterEdgePoint>.SphereQuery en0 = this.m_outerEdgePoints.QueryPointsSphere(ref posv0, MyVoxelConnectionHelper.OUTER_EDGE_EPSILON);
        while (en0.MoveNext())
        {
          if (en0.Current.EdgeIndex == edgeIndex && en0.Current.FirstPoint)
          {
            ++num;
            break;
          }
        }
        MyVector3Grid<MyVoxelConnectionHelper.OuterEdgePoint>.SphereQuery en1 = this.m_outerEdgePoints.QueryPointsSphere(ref posv1, MyVoxelConnectionHelper.OUTER_EDGE_EPSILON);
        while (en1.MoveNext())
        {
          if (en1.Current.EdgeIndex == edgeIndex && !en1.Current.FirstPoint)
          {
            ++num;
            break;
          }
        }
        if (num == 2)
        {
          this.m_outerEdgePoints.RemoveTwo(ref en0, ref en1);
          return true;
        }
        edgeIndex = -1;
      }
      return false;
    }

    public void DebugDraw(ref Matrix drawMatrix, MyWingedEdgeMesh mesh)
    {
      Dictionary<Vector3I, int>.Enumerator enumerator = this.m_outerEdgePoints.EnumerateBins();
      int num = 0;
      while (enumerator.MoveNext())
      {
        int binIndex = MyCestmirDebugInputComponent.BinIndex;
        if (binIndex == this.m_outerEdgePoints.InvalidIndex || num == binIndex)
        {
          KeyValuePair<Vector3I, int> current = enumerator.Current;
          Vector3I key = current.Key;
          current = enumerator.Current;
          int nextBinIndex = current.Value;
          BoundingBoxD output;
          this.m_outerEdgePoints.GetLocalBinBB(ref key, out output);
          BoundingBoxD aabb;
          aabb.Min = Vector3D.Transform(output.Min, drawMatrix);
          aabb.Max = Vector3D.Transform(output.Max, drawMatrix);
          for (; nextBinIndex != this.m_outerEdgePoints.InvalidIndex; nextBinIndex = this.m_outerEdgePoints.GetNextBinIndex(nextBinIndex))
          {
            Vector3 point = this.m_outerEdgePoints.GetPoint(nextBinIndex);
            MyWingedEdgeMesh.Edge edge = mesh.GetEdge(this.m_outerEdgePoints.GetData(nextBinIndex).EdgeIndex);
            MyRenderProxy.DebugDrawArrow3D(Vector3D.Transform((Vector3D) ((mesh.GetVertexPosition(edge.Vertex1) + mesh.GetVertexPosition(edge.Vertex2)) * 0.5f), drawMatrix), Vector3D.Transform((Vector3D) point, drawMatrix), Color.Yellow, new Color?(Color.Yellow));
          }
          MyRenderProxy.DebugDrawAABB(aabb, Color.PowderBlue, depthRead: false);
        }
        ++num;
      }
    }

    [Conditional("DEBUG")]
    public void CollectOuterEdges(
      List<MyTuple<MyVoxelConnectionHelper.OuterEdgePoint, Vector3>> output)
    {
      Dictionary<Vector3I, int>.Enumerator enumerator = this.m_outerEdgePoints.EnumerateBins();
      while (enumerator.MoveNext())
      {
        for (int nextBinIndex = enumerator.Current.Value; nextBinIndex != -1; nextBinIndex = this.m_outerEdgePoints.GetNextBinIndex(nextBinIndex))
          output.Add(new MyTuple<MyVoxelConnectionHelper.OuterEdgePoint, Vector3>(this.m_outerEdgePoints.GetData(nextBinIndex), this.m_outerEdgePoints.GetPoint(nextBinIndex)));
      }
    }

    private struct InnerEdgeIndex : IEquatable<MyVoxelConnectionHelper.InnerEdgeIndex>
    {
      private readonly ushort V0;
      private readonly ushort V1;

      public InnerEdgeIndex(ushort vert0, ushort vert1)
      {
        this.V0 = vert0;
        this.V1 = vert1;
      }

      public override int GetHashCode() => (int) this.V0 + (int) this.V1 << 16;

      public override bool Equals(object obj) => obj is MyVoxelConnectionHelper.InnerEdgeIndex other && this.Equals(other);

      public override string ToString() => "{" + (object) this.V0 + ", " + (object) this.V1 + "}";

      public bool Equals(MyVoxelConnectionHelper.InnerEdgeIndex other) => (int) other.V0 == (int) this.V0 && (int) other.V1 == (int) this.V1;
    }

    public struct OuterEdgePoint
    {
      public int EdgeIndex;
      public bool FirstPoint;

      public OuterEdgePoint(int edgeIndex, bool firstPoint)
      {
        this.EdgeIndex = edgeIndex;
        this.FirstPoint = firstPoint;
      }

      public override string ToString() => "{" + (object) this.EdgeIndex + (this.FirstPoint ? (object) " O--->" : (object) " <---O") + "}";
    }
  }
}
