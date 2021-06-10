// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.IMyEnvironmentDataProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
  public interface IMyEnvironmentDataProvider
  {
    MyEnvironmentDataView GetItemView(
      int lod,
      ref Vector2I start,
      ref Vector2I end,
      ref Vector3D localOrigin);

    MyObjectBuilder_EnvironmentDataProvider GetObjectBuilder();

    void DebugDraw();

    IEnumerable<MyLogicalEnvironmentSectorBase> LogicalSectors { get; }

    MyLogicalEnvironmentSectorBase GetLogicalSector(long sectorId);

    void RevalidateItem(long sectorId, int itemId);
  }
}
