// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MyEnvironmentSectorExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
  public static class MyEnvironmentSectorExtensions
  {
    public static bool HasWorkPending(this MyEnvironmentSector self) => self.HasSerialWorkPending || self.HasParallelWorkPending || self.HasParallelWorkInProgress;

    public static void DisableItemsInBox(this MyEnvironmentSector sector, ref BoundingBoxD box)
    {
      if (sector.DataView == null)
        return;
      for (int index = 0; index < sector.DataView.LogicalSectors.Count; ++index)
        sector.DataView.LogicalSectors[index].DisableItemsInBox(sector.SectorCenter, ref box);
    }

    public static void GetItemsInAabb(
      this MyEnvironmentSector sector,
      ref BoundingBoxD aabb,
      List<int> itemsInBox)
    {
      if (sector.DataView == null)
        return;
      aabb.Translate(-sector.SectorCenter);
      for (int index = 0; index < sector.DataView.LogicalSectors.Count; ++index)
        sector.DataView.LogicalSectors[index].GetItemsInAabb(ref aabb, itemsInBox);
    }
  }
}
