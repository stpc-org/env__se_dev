// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.IMyGravityProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRageMath;

namespace Sandbox.Game.Entities
{
  public interface IMyGravityProvider
  {
    bool IsWorking { get; }

    Vector3 GetWorldGravity(Vector3D worldPoint);

    bool IsPositionInRange(Vector3D worldPoint);

    float GetGravityMultiplier(Vector3D worldPoint);

    void GetProxyAABB(out BoundingBoxD aabb);
  }
}
