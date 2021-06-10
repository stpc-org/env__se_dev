// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MyEnvironmentDataView
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
  public abstract class MyEnvironmentDataView
  {
    public Vector2I Start;
    public Vector2I End;
    public int Lod;
    public MyList<ItemInfo> Items;
    public MyEnvironmentSector Listener;
    public List<int> SectorOffsets;
    public List<int> IntraSectorOffsets;
    public List<MyLogicalEnvironmentSectorBase> LogicalSectors;

    public abstract void Close();

    public void GetLogicalSector(
      int item,
      out int logicalItem,
      out MyLogicalEnvironmentSectorBase sector)
    {
      int index = this.SectorOffsets.BinaryIntervalSearch<int>(item) - 1;
      logicalItem = item - this.SectorOffsets[index] + this.IntraSectorOffsets[index];
      sector = this.LogicalSectors[index];
    }
  }
}
