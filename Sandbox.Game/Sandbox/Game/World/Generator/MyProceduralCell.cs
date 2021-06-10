// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyProceduralCell
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public class MyProceduralCell
  {
    public int proxyId = -1;
    private MyDynamicAABBTreeD m_tree = new MyDynamicAABBTreeD(Vector3D.Zero);

    public Vector3I CellId { get; private set; }

    public BoundingBoxD BoundingVolume { get; private set; }

    public void AddObject(MyObjectSeed objectSeed)
    {
      BoundingBoxD boundingVolume = objectSeed.BoundingVolume;
      objectSeed.m_proxyId = this.m_tree.AddProxy(ref boundingVolume, (object) objectSeed, 0U);
    }

    public MyProceduralCell(Vector3I cellId, double cellSize)
    {
      this.CellId = cellId;
      this.BoundingVolume = new BoundingBoxD(this.CellId * cellSize, (this.CellId + 1) * cellSize);
    }

    public void OverlapAllBoundingSphere(
      ref BoundingSphereD sphere,
      List<MyObjectSeed> list,
      bool clear = false)
    {
      this.m_tree.OverlapAllBoundingSphere<MyObjectSeed>(ref sphere, list, clear);
    }

    public void OverlapAllBoundingBox(ref BoundingBoxD box, List<MyObjectSeed> list, bool clear = false) => this.m_tree.OverlapAllBoundingBox<MyObjectSeed>(ref box, list, clear: clear);

    public void GetAll(List<MyObjectSeed> list, bool clear = true) => this.m_tree.GetAll<MyObjectSeed>(list, clear);

    public override int GetHashCode() => this.CellId.GetHashCode();

    public override string ToString() => this.CellId.ToString();
  }
}
