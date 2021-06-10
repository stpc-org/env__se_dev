// Decompiled with JetBrains decompiler
// Type: VRage.Algorithms.MyPath`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace VRage.Algorithms
{
  public class MyPath<V> : IEnumerable<MyPath<V>.PathNode>, IEnumerable where V : class, IMyPathVertex<V>, IEnumerable<IMyPathEdge<V>>
  {
    private List<MyPath<V>.PathNode> m_vertices;

    public int Count => this.m_vertices.Count;

    internal MyPath(int size) => this.m_vertices = new List<MyPath<V>.PathNode>(size);

    public MyPath<V>.PathNode this[int position]
    {
      get => this.m_vertices[position];
      set => this.m_vertices[position] = value;
    }

    internal void Add(IMyPathVertex<V> vertex, IMyPathVertex<V> nextVertex)
    {
      MyPath<V>.PathNode pathNode = new MyPath<V>.PathNode();
      pathNode.Vertex = vertex;
      if (nextVertex == null)
      {
        this.m_vertices.Add(pathNode);
      }
      else
      {
        int neighborCount = vertex.GetNeighborCount();
        for (int index = 0; index < neighborCount; ++index)
        {
          if (vertex.GetNeighbor(index) == nextVertex)
          {
            pathNode.nextVertex = index;
            this.m_vertices.Add(pathNode);
            break;
          }
        }
      }
    }

    public IEnumerator<MyPath<V>.PathNode> GetEnumerator() => (IEnumerator<MyPath<V>.PathNode>) this.m_vertices.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.m_vertices.GetEnumerator();

    public struct PathNode
    {
      public IMyPathVertex<V> Vertex;
      public int nextVertex;
    }
  }
}
