// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MyProceduralDataView
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
  public class MyProceduralDataView : MyEnvironmentDataView
  {
    private readonly MyProceduralEnvironmentProvider m_provider;

    public MyProceduralDataView(
      MyProceduralEnvironmentProvider provider,
      int lod,
      ref Vector2I start,
      ref Vector2I end)
    {
      this.m_provider = provider;
      this.Start = start;
      this.End = end;
      this.Lod = lod;
      int capacity = (end - start + 1).Size();
      this.SectorOffsets = new List<int>(capacity);
      this.LogicalSectors = new List<MyLogicalEnvironmentSectorBase>(capacity);
      this.IntraSectorOffsets = new List<int>(capacity);
      this.Items = new MyList<ItemInfo>();
    }

    public override void Close() => this.m_provider.CloseView(this);

    public int GetSectorIndex(int x, int y) => x - this.Start.X + (y - this.Start.Y) * (this.End.X - this.Start.X + 1);

    public void AddSector(MyProceduralLogicalSector sector)
    {
      this.SectorOffsets.Add(this.Items.Count);
      this.LogicalSectors.Add((MyLogicalEnvironmentSectorBase) sector);
      this.IntraSectorOffsets.Add(0);
    }
  }
}
