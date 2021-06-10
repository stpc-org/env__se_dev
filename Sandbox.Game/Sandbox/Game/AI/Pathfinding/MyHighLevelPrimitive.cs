// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.MyHighLevelPrimitive
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Algorithms;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
  public class MyHighLevelPrimitive : MyNavigationPrimitive
  {
    private readonly List<int> m_neighbors;
    private Vector3 m_position;

    public bool IsExpanded { get; set; }

    public int Index { get; }

    public override Vector3 Position => this.m_position;

    public override Vector3D WorldPosition => this.Parent.LocalToGlobal(this.m_position);

    public MyHighLevelGroup Parent { get; }

    public override IMyNavigationGroup Group => (IMyNavigationGroup) this.Parent;

    public MyHighLevelPrimitive(MyHighLevelGroup parent, int index, Vector3 position)
    {
      this.Parent = parent;
      this.m_neighbors = new List<int>(4);
      this.Index = index;
      this.m_position = position;
      this.IsExpanded = false;
    }

    public override string ToString() => "(" + (object) this.Parent + ")[" + (object) this.Index + "]";

    public void GetNeighbours(List<int> output)
    {
      output.Clear();
      output.AddRange((IEnumerable<int>) this.m_neighbors);
    }

    public void Connect(int other) => this.m_neighbors.Add(other);

    public void Disconnect(int other) => this.m_neighbors.Remove(other);

    public void UpdatePosition(Vector3 position) => this.m_position = position;

    public IMyHighLevelComponent GetComponent() => this.Parent.LowLevelGroup.GetComponent(this);

    public override int GetOwnNeighborCount() => this.m_neighbors.Count;

    public override IMyPathVertex<MyNavigationPrimitive> GetOwnNeighbor(
      int index)
    {
      return (IMyPathVertex<MyNavigationPrimitive>) this.Parent.GetPrimitive(this.m_neighbors[index]);
    }

    public override IMyPathEdge<MyNavigationPrimitive> GetOwnEdge(
      int index)
    {
      MyNavigationEdge.Static.Init((MyNavigationPrimitive) this, this.GetOwnNeighbor(index) as MyNavigationPrimitive, 0);
      return (IMyPathEdge<MyNavigationPrimitive>) MyNavigationEdge.Static;
    }

    public override MyHighLevelPrimitive GetHighLevelPrimitive() => (MyHighLevelPrimitive) null;
  }
}
