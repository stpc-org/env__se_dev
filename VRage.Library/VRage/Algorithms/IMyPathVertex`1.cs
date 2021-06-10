// Decompiled with JetBrains decompiler
// Type: VRage.Algorithms.IMyPathVertex`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace VRage.Algorithms
{
  public interface IMyPathVertex<V> : IEnumerable<IMyPathEdge<V>>, IEnumerable
  {
    MyPathfindingData PathfindingData { get; }

    float EstimateDistanceTo(IMyPathVertex<V> other);

    int GetNeighborCount();

    IMyPathVertex<V> GetNeighbor(int index);

    IMyPathEdge<V> GetEdge(int index);
  }
}
