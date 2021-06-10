// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.IMyGridOverlapTest
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  internal interface IMyGridOverlapTest
  {
    void GetBlocks(
      Vector3I minI,
      Vector3I maxI,
      Dictionary<Vector3I, OverlapResult> outOverlappedBlocks);
  }
}
