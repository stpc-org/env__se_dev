// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Conveyors.MyAttachableConveyorEndpoint
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Algorithms;

namespace Sandbox.Game.GameSystems.Conveyors
{
  public class MyAttachableConveyorEndpoint : MyMultilineConveyorEndpoint
  {
    private List<MyAttachableConveyorEndpoint.MyAttachableLine> m_lines;

    public MyAttachableConveyorEndpoint(MyCubeBlock block)
      : base(block)
      => this.m_lines = new List<MyAttachableConveyorEndpoint.MyAttachableLine>();

    public void Attach(MyAttachableConveyorEndpoint other)
    {
      MyAttachableConveyorEndpoint.MyAttachableLine line = new MyAttachableConveyorEndpoint.MyAttachableLine(this, other);
      this.AddAttachableLine(line);
      other.AddAttachableLine(line);
    }

    public void Detach(MyAttachableConveyorEndpoint other)
    {
      for (int index = 0; index < this.m_lines.Count; ++index)
      {
        MyAttachableConveyorEndpoint.MyAttachableLine line = this.m_lines[index];
        if (line.Contains(other))
        {
          this.RemoveAttachableLine(line);
          other.RemoveAttachableLine(line);
          break;
        }
      }
    }

    public void DetachAll()
    {
      for (int index = 0; index < this.m_lines.Count; ++index)
      {
        MyAttachableConveyorEndpoint.MyAttachableLine line = this.m_lines[index];
        (line.GetOtherVertex((IMyConveyorEndpoint) this) as MyAttachableConveyorEndpoint).RemoveAttachableLine(line);
      }
      this.m_lines.Clear();
    }

    private void AddAttachableLine(MyAttachableConveyorEndpoint.MyAttachableLine line) => this.m_lines.Add(line);

    private void RemoveAttachableLine(MyAttachableConveyorEndpoint.MyAttachableLine line) => this.m_lines.Remove(line);

    public bool AlreadyAttachedTo(MyAttachableConveyorEndpoint other)
    {
      foreach (MyAttachableConveyorEndpoint.MyAttachableLine line in this.m_lines)
      {
        if (line.GetOtherVertex((IMyConveyorEndpoint) this) == other)
          return true;
      }
      return false;
    }

    public bool AlreadyAttached() => (uint) this.m_lines.Count > 0U;

    protected override int GetNeighborCount() => base.GetNeighborCount() + this.m_lines.Count;

    protected override IMyPathVertex<IMyConveyorEndpoint> GetNeighbor(
      int index)
    {
      int neighborCount = base.GetNeighborCount();
      return index < neighborCount ? base.GetNeighbor(index) : (IMyPathVertex<IMyConveyorEndpoint>) this.m_lines[index - neighborCount].GetOtherVertex((IMyConveyorEndpoint) this);
    }

    protected override IMyPathEdge<IMyConveyorEndpoint> GetEdge(
      int index)
    {
      int neighborCount = base.GetNeighborCount();
      return index < neighborCount ? base.GetEdge(index) : (IMyPathEdge<IMyConveyorEndpoint>) this.m_lines[index - neighborCount];
    }

    private class MyAttachableLine : IMyPathEdge<IMyConveyorEndpoint>
    {
      private MyAttachableConveyorEndpoint m_endpoint1;
      private MyAttachableConveyorEndpoint m_endpoint2;

      public MyAttachableLine(
        MyAttachableConveyorEndpoint endpoint1,
        MyAttachableConveyorEndpoint endpoint2)
      {
        this.m_endpoint1 = endpoint1;
        this.m_endpoint2 = endpoint2;
      }

      public float GetWeight() => 2f;

      public IMyConveyorEndpoint GetOtherVertex(IMyConveyorEndpoint vertex1) => vertex1 == this.m_endpoint1 ? (IMyConveyorEndpoint) this.m_endpoint2 : (IMyConveyorEndpoint) this.m_endpoint1;

      public bool Contains(MyAttachableConveyorEndpoint endpoint) => endpoint == this.m_endpoint1 || endpoint == this.m_endpoint2;
    }
  }
}
