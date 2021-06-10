// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyEdgeOrientationInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Definitions
{
  public class MyEdgeOrientationInfo
  {
    public readonly Matrix Orientation;
    public readonly MyCubeEdgeType EdgeType;

    public MyEdgeOrientationInfo(Matrix localMatrix, MyCubeEdgeType edgeType)
    {
      this.Orientation = localMatrix;
      this.EdgeType = edgeType;
    }
  }
}
