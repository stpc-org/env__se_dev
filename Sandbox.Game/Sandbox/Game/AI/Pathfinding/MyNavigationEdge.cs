// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.MyNavigationEdge
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Algorithms;

namespace Sandbox.Game.AI.Pathfinding
{
  public class MyNavigationEdge : IMyPathEdge<MyNavigationPrimitive>
  {
    public static MyNavigationEdge Static = new MyNavigationEdge();
    private MyNavigationPrimitive m_triA;
    private MyNavigationPrimitive m_triB;

    public int Index { get; private set; }

    public void Init(MyNavigationPrimitive triA, MyNavigationPrimitive triB, int index)
    {
      this.m_triA = triA;
      this.m_triB = triB;
      this.Index = index;
    }

    public float GetWeight() => (this.m_triA.Position - this.m_triB.Position).Length() * 1f;

    public MyNavigationPrimitive GetOtherVertex(MyNavigationPrimitive vertex1) => vertex1 == this.m_triA ? this.m_triB : this.m_triA;
  }
}
