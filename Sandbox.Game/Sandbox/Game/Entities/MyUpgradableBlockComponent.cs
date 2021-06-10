// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyUpgradableBlockComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GameSystems.Conveyors;
using System.Collections.Generic;

namespace Sandbox.Game.Entities
{
  public class MyUpgradableBlockComponent
  {
    public HashSet<ConveyorLinePosition> ConnectionPositions { get; private set; }

    public MyUpgradableBlockComponent(MyCubeBlock parent)
    {
      this.ConnectionPositions = new HashSet<ConveyorLinePosition>();
      this.Refresh(parent);
    }

    public void Refresh(MyCubeBlock parent)
    {
      if (parent.BlockDefinition.Model == null)
        return;
      this.ConnectionPositions.Clear();
      foreach (ConveyorLinePosition linePosition in MyMultilineConveyorEndpoint.GetLinePositions(parent, "detector_upgrade"))
        this.ConnectionPositions.Add(MyMultilineConveyorEndpoint.PositionToGridCoords(linePosition, parent));
    }
  }
}
