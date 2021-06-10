// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyObjectSeed
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  public class MyObjectSeed
  {
    public MyObjectSeedParams Params = new MyObjectSeedParams();
    public int m_proxyId = -1;

    public BoundingBoxD BoundingVolume { get; private set; }

    public float Size { get; private set; }

    public MyProceduralCell Cell { get; private set; }

    public Vector3I CellId => this.Cell.CellId;

    public object UserData { get; set; }

    public MyObjectSeed()
    {
    }

    public MyObjectSeed(MyProceduralCell cell, Vector3D position, double size)
    {
      this.Cell = cell;
      this.Size = (float) size;
      this.BoundingVolume = new BoundingBoxD(position - (double) this.Size, position + this.Size);
    }
  }
}
