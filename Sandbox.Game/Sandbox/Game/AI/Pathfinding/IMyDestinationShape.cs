// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.IMyDestinationShape
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.AI.Pathfinding
{
  public interface IMyDestinationShape
  {
    void SetRelativeTransform(MatrixD invWorldTransform);

    void UpdateWorldTransform(MatrixD worldTransform);

    float PointAdmissibility(Vector3D position, float tolerance);

    Vector3D GetClosestPoint(Vector3D queryPoint);

    Vector3D GetBestPoint(Vector3D queryPoint);

    Vector3D GetDestination();

    void DebugDraw();
  }
}
