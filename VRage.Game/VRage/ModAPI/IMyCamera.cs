// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.IMyCamera
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRageMath;

namespace VRage.ModAPI
{
  public interface IMyCamera
  {
    Vector3D Position { get; }

    Vector3D PreviousPosition { get; }

    Vector2 ViewportOffset { get; }

    Vector2 ViewportSize { get; }

    MatrixD ViewMatrix { get; }

    MatrixD WorldMatrix { get; }

    MatrixD ProjectionMatrix { get; }

    float NearPlaneDistance { get; }

    float FarPlaneDistance { get; }

    float FieldOfViewAngle { get; }

    float FovWithZoom { get; }

    double GetDistanceWithFOV(Vector3D position);

    bool IsInFrustum(ref BoundingBoxD boundingBox);

    bool IsInFrustum(ref BoundingSphereD boundingSphere);

    bool IsInFrustum(BoundingBoxD boundingBox);

    Vector3D WorldToScreen(ref Vector3D worldPos);

    LineD WorldLineFromScreen(Vector2 screenCoords);

    [Obsolete]
    MatrixD ProjectionMatrixForNearObjects { get; }

    [Obsolete]
    float FieldOfViewAngleForNearObjects { get; }

    [Obsolete]
    float FovWithZoomForNearObjects { get; }
  }
}
