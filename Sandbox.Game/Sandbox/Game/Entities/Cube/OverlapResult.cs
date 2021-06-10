// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.OverlapResult
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  internal struct OverlapResult
  {
    public Vector3I Position;
    public MyCubeBlock FatBlock;
    public MyBlockOrientation Orientation;
    public MyCubeBlockDefinition Definition;
  }
}
