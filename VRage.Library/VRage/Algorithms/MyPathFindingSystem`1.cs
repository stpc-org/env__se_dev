// Decompiled with JetBrains decompiler
// Type: VRage.Algorithms.MyPathFindingSystem`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;

namespace VRage.Algorithms
{
  public class MyPathFindingSystem<V> : IEnumerable<V>, IEnumerable where V : class, IMyPathVertex<V>
  {
    private long m_timestamp;
    private Func<long> m_timestampFunction;
    private Queue<V> m_bfsQueue;
    private List<V> m_reachableList;
    private MyBinaryHeap<float, MyPathfindingData> m_openVertices;
    private MyPathFindingSystem<V>.Enumerator m_enumerator;
    private bool m_enumerating;

    public MyPathFindingSystem(int queueInitSize = 128, Func<long> timestampFunction = null)
    {
      this.m_bfsQueue = new Queue<V>(queueInitSize);
      this.m_reachableList = new List<V>(128);
      this.m_openVertices = new MyBinaryHeap<float, MyPathfindingData>(128);
      this.m_timestamp = 0L;
      this.m_timestampFunction = timestampFunction;
      this.m_enumerating = false;
      this.m_enumerator = new MyPathFindingSystem<V>.Enumerator();
    }

    protected void CalculateNextTimestamp()
    {
      if (this.m_timestampFunction != null)
        this.m_timestamp = this.m_timestampFunction();
      else
        ++this.m_timestamp;
    }

    public MyPath<V> FindPath(
      V start,
      V end,
      Predicate<V> vertexTraversable = null,
      Predicate<IMyPathEdge<V>> edgeTraversable = null)
    {
      this.CalculateNextTimestamp();
      MyPathfindingData pathfindingData1 = start.PathfindingData;
      this.Visit(pathfindingData1);
      pathfindingData1.Predecessor = (MyPathfindingData) null;
      pathfindingData1.PathLength = 0.0f;
      IMyPathVertex<V> myPathVertex = (IMyPathVertex<V>) null;
      float num1 = float.PositiveInfinity;
      this.m_openVertices.Insert(start.PathfindingData, start.EstimateDistanceTo((IMyPathVertex<V>) end));
      while (this.m_openVertices.Count > 0)
      {
        MyPathfindingData myPathfindingData = this.m_openVertices.RemoveMin();
        V parent = myPathfindingData.Parent as V;
        float pathLength = myPathfindingData.PathLength;
        if (myPathVertex == null || (double) pathLength < (double) num1)
        {
          for (int index = 0; index < parent.GetNeighborCount(); ++index)
          {
            IMyPathEdge<V> edge = parent.GetEdge(index);
            if (edge != null && (edgeTraversable == null || edgeTraversable(edge)))
            {
              V otherVertex = edge.GetOtherVertex(parent);
              if ((object) otherVertex != null && (vertexTraversable == null || vertexTraversable(otherVertex)))
              {
                float num2 = myPathfindingData.PathLength + edge.GetWeight();
                MyPathfindingData pathfindingData2 = otherVertex.PathfindingData;
                if ((object) otherVertex == (object) end && (double) num2 < (double) num1)
                {
                  myPathVertex = (IMyPathVertex<V>) otherVertex;
                  num1 = num2;
                }
                if (this.Visited(pathfindingData2))
                {
                  if ((double) num2 < (double) pathfindingData2.PathLength)
                  {
                    pathfindingData2.PathLength = num2;
                    pathfindingData2.Predecessor = myPathfindingData;
                    this.m_openVertices.ModifyUp(pathfindingData2, num2 + otherVertex.EstimateDistanceTo((IMyPathVertex<V>) end));
                  }
                }
                else
                {
                  this.Visit(pathfindingData2);
                  pathfindingData2.PathLength = num2;
                  pathfindingData2.Predecessor = myPathfindingData;
                  this.m_openVertices.Insert(pathfindingData2, num2 + otherVertex.EstimateDistanceTo((IMyPathVertex<V>) end));
                }
              }
            }
          }
        }
        else
          break;
      }
      this.m_openVertices.Clear();
      return myPathVertex == null ? (MyPath<V>) null : this.ReturnPath(myPathVertex.PathfindingData, (MyPathfindingData) null, 0);
    }

    public MyPath<V> FindPath(
      V start,
      Func<V, float> heuristic,
      Func<V, float> terminationCriterion,
      Predicate<V> vertexTraversable = null,
      bool returnClosest = true)
    {
      this.CalculateNextTimestamp();
      MyPathfindingData pathfindingData1 = start.PathfindingData;
      this.Visit(pathfindingData1);
      pathfindingData1.Predecessor = (MyPathfindingData) null;
      pathfindingData1.PathLength = 0.0f;
      IMyPathVertex<V> myPathVertex1 = (IMyPathVertex<V>) null;
      float num1 = float.PositiveInfinity;
      IMyPathVertex<V> myPathVertex2 = (IMyPathVertex<V>) null;
      float num2 = float.PositiveInfinity;
      float num3 = terminationCriterion(start);
      if ((double) num3 != double.PositiveInfinity)
      {
        myPathVertex1 = (IMyPathVertex<V>) start;
        num1 = heuristic(start) + num3;
      }
      this.m_openVertices.Insert(start.PathfindingData, heuristic(start));
      while (this.m_openVertices.Count > 0)
      {
        MyPathfindingData myPathfindingData = this.m_openVertices.RemoveMin();
        V parent = myPathfindingData.Parent as V;
        float pathLength = myPathfindingData.PathLength;
        if (myPathVertex1 == null || (double) pathLength + (double) heuristic(parent) < (double) num1)
        {
          for (int index = 0; index < parent.GetNeighborCount(); ++index)
          {
            IMyPathEdge<V> edge = parent.GetEdge(index);
            if (edge != null)
            {
              V otherVertex = edge.GetOtherVertex(parent);
              if ((object) otherVertex != null && (vertexTraversable == null || vertexTraversable(otherVertex)))
              {
                float num4 = myPathfindingData.PathLength + edge.GetWeight();
                MyPathfindingData pathfindingData2 = otherVertex.PathfindingData;
                float num5 = num4 + heuristic(otherVertex);
                if ((double) num5 < (double) num2)
                {
                  myPathVertex2 = (IMyPathVertex<V>) otherVertex;
                  num2 = num5;
                }
                float num6 = terminationCriterion(otherVertex);
                if ((double) num5 + (double) num6 < (double) num1)
                {
                  myPathVertex1 = (IMyPathVertex<V>) otherVertex;
                  num1 = num5 + num6;
                }
                if (this.Visited(pathfindingData2))
                {
                  if ((double) num4 < (double) pathfindingData2.PathLength)
                  {
                    pathfindingData2.PathLength = num4;
                    pathfindingData2.Predecessor = myPathfindingData;
                    this.m_openVertices.ModifyUp(pathfindingData2, num5);
                  }
                }
                else
                {
                  this.Visit(pathfindingData2);
                  pathfindingData2.PathLength = num4;
                  pathfindingData2.Predecessor = myPathfindingData;
                  this.m_openVertices.Insert(pathfindingData2, num5);
                }
              }
            }
          }
        }
        else
          break;
      }
      this.m_openVertices.Clear();
      if (myPathVertex1 != null)
        return this.ReturnPath(myPathVertex1.PathfindingData, (MyPathfindingData) null, 0);
      return !returnClosest || myPathVertex2 == null ? (MyPath<V>) null : this.ReturnPath(myPathVertex2.PathfindingData, (MyPathfindingData) null, 0);
    }

    private MyPath<V> ReturnPath(
      MyPathfindingData vertexData,
      MyPathfindingData successor,
      int remainingVertices)
    {
      if (vertexData.Predecessor == null)
        return new MyPath<V>(remainingVertices + 1)
        {
          {
            (IMyPathVertex<V>) (vertexData.Parent as V),
            (IMyPathVertex<V>) (successor != null ? successor.Parent as V : default (V))
          }
        };
      MyPath<V> myPath = this.ReturnPath(vertexData.Predecessor, vertexData, remainingVertices + 1);
      myPath.Add((IMyPathVertex<V>) (vertexData.Parent as V), (IMyPathVertex<V>) (successor != null ? successor.Parent as V : default (V)));
      return myPath;
    }

    public bool Reachable(
      V from,
      V to,
      Predicate<V> vertexFilter = null,
      Predicate<V> vertexTraversable = null,
      Predicate<IMyPathEdge<V>> edgeTraversable = null)
    {
      this.PrepareTraversal(from, vertexFilter, vertexTraversable, edgeTraversable);
      foreach (V v in this)
      {
        if (v.Equals((object) to))
          return true;
      }
      return false;
    }

    public void FindReachable(
      IEnumerable<V> fromSet,
      List<V> reachableVertices,
      Predicate<V> vertexFilter = null,
      Predicate<V> vertexTraversable = null,
      Predicate<IMyPathEdge<V>> edgeTraversable = null)
    {
      this.CalculateNextTimestamp();
      foreach (V from in fromSet)
      {
        if (!this.Visited(from))
          this.FindReachableInternal(from, reachableVertices, vertexFilter, vertexTraversable, edgeTraversable);
      }
    }

    public void FindReachable(
      V from,
      List<V> reachableVertices,
      Predicate<V> vertexFilter = null,
      Predicate<V> vertexTraversable = null,
      Predicate<IMyPathEdge<V>> edgeTraversable = null)
    {
      this.FindReachableInternal(from, reachableVertices, vertexFilter, vertexTraversable, edgeTraversable);
    }

    public long GetCurrentTimestamp() => this.m_timestamp;

    public bool VisitedBetween(V vertex, long start, long end) => vertex.PathfindingData.Timestamp >= start && vertex.PathfindingData.Timestamp <= end;

    private void FindReachableInternal(
      V from,
      List<V> reachableVertices,
      Predicate<V> vertexFilter = null,
      Predicate<V> vertexTraversable = null,
      Predicate<IMyPathEdge<V>> edgeTraversable = null)
    {
      this.PrepareTraversal(from, vertexFilter, vertexTraversable, edgeTraversable);
      foreach (V v in this)
        reachableVertices.Add(v);
    }

    private void Visit(V vertex) => vertex.PathfindingData.Timestamp = this.m_timestamp;

    private void Visit(MyPathfindingData vertexData) => vertexData.Timestamp = this.m_timestamp;

    private bool Visited(V vertex) => vertex.PathfindingData.Timestamp == this.m_timestamp;

    private bool Visited(MyPathfindingData vertexData) => vertexData.Timestamp == this.m_timestamp;

    public void PrepareTraversal(
      V startingVertex,
      Predicate<V> vertexFilter = null,
      Predicate<V> vertexTraversable = null,
      Predicate<IMyPathEdge<V>> edgeTraversable = null)
    {
      this.m_enumerator.Init(this, startingVertex, vertexFilter, vertexTraversable, edgeTraversable);
    }

    public void PerformTraversal()
    {
      do
        ;
      while (this.m_enumerator.MoveNext());
      this.m_enumerator.Dispose();
    }

    private MyPathFindingSystem<V>.Enumerator GetEnumeratorInternal() => this.m_enumerator;

    public MyPathFindingSystem<V>.Enumerator GetEnumerator() => this.GetEnumeratorInternal();

    IEnumerator<V> IEnumerable<V>.GetEnumerator() => (IEnumerator<V>) this.GetEnumeratorInternal();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumeratorInternal();

    public class Enumerator : IEnumerator<V>, IEnumerator, IDisposable
    {
      private V m_currentVertex;
      private MyPathFindingSystem<V> m_parent;
      private Predicate<V> m_vertexFilter;
      private Predicate<V> m_vertexTraversable;
      private Predicate<IMyPathEdge<V>> m_edgeTraversable;

      public void Init(
        MyPathFindingSystem<V> parent,
        V startingVertex,
        Predicate<V> vertexFilter = null,
        Predicate<V> vertexTraversable = null,
        Predicate<IMyPathEdge<V>> edgeTraversable = null)
      {
        this.m_parent = parent;
        this.m_vertexFilter = vertexFilter;
        this.m_vertexTraversable = vertexTraversable;
        this.m_edgeTraversable = edgeTraversable;
        this.m_parent.CalculateNextTimestamp();
        this.m_parent.m_enumerating = true;
        this.m_parent.m_bfsQueue.Enqueue(startingVertex);
        startingVertex.PathfindingData.Timestamp = this.m_parent.m_timestamp;
      }

      public V Current => this.m_currentVertex;

      public void Dispose()
      {
        this.m_vertexFilter = (Predicate<V>) null;
        this.m_currentVertex = default (V);
        this.m_edgeTraversable = (Predicate<IMyPathEdge<V>>) null;
        this.m_vertexTraversable = (Predicate<V>) null;
        this.m_parent.m_enumerating = false;
        this.m_parent.m_bfsQueue.Clear();
        this.m_parent = (MyPathFindingSystem<V>) null;
      }

      object IEnumerator.Current => (object) this.m_currentVertex;

      public bool MoveNext()
      {
        while (this.m_parent.m_bfsQueue.Count<V>() != 0)
        {
          this.m_currentVertex = this.m_parent.m_bfsQueue.Dequeue();
          V v1 = default (V);
          for (int index = 0; index < this.m_currentVertex.GetNeighborCount(); ++index)
          {
            V v2;
            if (this.m_edgeTraversable == null)
            {
              v2 = (V) this.m_currentVertex.GetNeighbor(index);
              if ((object) v2 == null)
                continue;
            }
            else
            {
              IMyPathEdge<V> edge = this.m_currentVertex.GetEdge(index);
              if (this.m_edgeTraversable(edge))
              {
                v2 = edge.GetOtherVertex(this.m_currentVertex);
                if ((object) v2 == null)
                  continue;
              }
              else
                continue;
            }
            if (v2.PathfindingData.Timestamp != this.m_parent.m_timestamp && (this.m_vertexTraversable == null || this.m_vertexTraversable(v2)))
            {
              this.m_parent.m_bfsQueue.Enqueue(v2);
              v2.PathfindingData.Timestamp = this.m_parent.m_timestamp;
            }
          }
          if (this.m_vertexFilter == null || this.m_vertexFilter(this.m_currentVertex))
            return true;
        }
        return false;
      }

      public void Reset() => throw new NotImplementedException();
    }
  }
}
