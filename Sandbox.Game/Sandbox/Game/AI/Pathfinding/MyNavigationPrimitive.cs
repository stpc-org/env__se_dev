// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.MyNavigationPrimitive
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
  public abstract class MyNavigationPrimitive : IMyPathVertex<MyNavigationPrimitive>, IEnumerable<IMyPathEdge<MyNavigationPrimitive>>, IEnumerable
  {
    private bool m_externalNeighbors;

    public MyPathfindingData PathfindingData { get; }

    public bool HasExternalNeighbors
    {
      set => this.m_externalNeighbors = value;
    }

    protected MyNavigationPrimitive() => this.PathfindingData = new MyPathfindingData((object) this);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    float IMyPathVertex<MyNavigationPrimitive>.EstimateDistanceTo(
      IMyPathVertex<MyNavigationPrimitive> other)
    {
      MyNavigationPrimitive navigationPrimitive = other as MyNavigationPrimitive;
      return this.Group != navigationPrimitive.Group ? (float) Vector3D.Distance(this.WorldPosition, navigationPrimitive.WorldPosition) : Vector3.Distance(this.Position, navigationPrimitive.Position);
    }

    int IMyPathVertex<MyNavigationPrimitive>.GetNeighborCount()
    {
      int ownNeighborCount = this.GetOwnNeighborCount();
      return !this.m_externalNeighbors ? ownNeighborCount : ownNeighborCount + this.Group.GetExternalNeighborCount(this);
    }

    IMyPathVertex<MyNavigationPrimitive> IMyPathVertex<MyNavigationPrimitive>.GetNeighbor(
      int index)
    {
      int ownNeighborCount = this.GetOwnNeighborCount();
      return index >= ownNeighborCount ? (IMyPathVertex<MyNavigationPrimitive>) this.Group.GetExternalNeighbor(this, index - ownNeighborCount) : this.GetOwnNeighbor(index);
    }

    IMyPathEdge<MyNavigationPrimitive> IMyPathVertex<MyNavigationPrimitive>.GetEdge(
      int index)
    {
      int ownNeighborCount = this.GetOwnNeighborCount();
      return index >= ownNeighborCount ? this.Group.GetExternalEdge(this, index - ownNeighborCount) : this.GetOwnEdge(index);
    }

    public abstract Vector3 Position { get; }

    public abstract Vector3D WorldPosition { get; }

    public virtual Vector3 ProjectLocalPoint(Vector3 point) => this.Position;

    public abstract IMyNavigationGroup Group { get; }

    public abstract int GetOwnNeighborCount();

    public abstract IMyPathVertex<MyNavigationPrimitive> GetOwnNeighbor(
      int index);

    public abstract IMyPathEdge<MyNavigationPrimitive> GetOwnEdge(
      int index);

    public abstract MyHighLevelPrimitive GetHighLevelPrimitive();

    public IEnumerator<IMyPathEdge<MyNavigationPrimitive>> GetEnumerator() => throw new NotImplementedException();
  }
}
