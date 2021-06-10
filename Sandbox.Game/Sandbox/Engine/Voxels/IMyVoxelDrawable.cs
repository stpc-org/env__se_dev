// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.IMyVoxelDrawable
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game.Components;
using VRage.Game.Voxels;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public interface IMyVoxelDrawable
  {
    IMyStorage Storage { get; }

    Vector3I Size { get; }

    Vector3D PositionLeftBottomCorner { get; }

    Matrix Orientation { get; }

    Vector3I StorageMin { get; }

    MyRenderComponentBase Render { get; }

    MyClipmapScaleEnum ScaleGroup { get; }
  }
}
