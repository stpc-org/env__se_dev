// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Conveyors.IMyConveyorEndpoint
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System.Collections;
using System.Collections.Generic;
using VRage.Algorithms;

namespace Sandbox.Game.GameSystems.Conveyors
{
  public interface IMyConveyorEndpoint : IMyPathVertex<IMyConveyorEndpoint>, IEnumerable<IMyPathEdge<IMyConveyorEndpoint>>, IEnumerable
  {
    MyConveyorLine GetConveyorLine(ConveyorLinePosition position);

    MyConveyorLine GetConveyorLine(int index);

    ConveyorLinePosition GetPosition(int index);

    void DebugDraw();

    void SetConveyorLine(ConveyorLinePosition position, MyConveyorLine newLine);

    int GetLineCount();

    MyCubeBlock CubeBlock { get; }
  }
}
