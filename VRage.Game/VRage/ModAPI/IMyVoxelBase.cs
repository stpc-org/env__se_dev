// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.IMyVoxelBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRageMath;

namespace VRage.ModAPI
{
  public interface IMyVoxelBase : IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
  {
    IMyStorage Storage { get; }

    Vector3D PositionLeftBottomCorner { get; }

    bool IsBoxIntersectingBoundingBoxOfThisVoxelMap(ref BoundingBoxD boundingBox);

    string StorageName { get; }
  }
}
