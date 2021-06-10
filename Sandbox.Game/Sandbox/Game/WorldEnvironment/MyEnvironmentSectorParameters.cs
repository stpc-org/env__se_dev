// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MyEnvironmentSectorParameters
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.WorldEnvironment.Definitions;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment
{
  public struct MyEnvironmentSectorParameters
  {
    public long EntityId;
    public BoundingBox2I DataRange;
    public Vector3 SurfaceBasisX;
    public Vector3 SurfaceBasisY;
    public Vector3D Center;
    public Vector3D[] Bounds;
    public MyWorldEnvironmentDefinition Environment;
    public IMyEnvironmentDataProvider Provider;
    public long SectorId;
  }
}
