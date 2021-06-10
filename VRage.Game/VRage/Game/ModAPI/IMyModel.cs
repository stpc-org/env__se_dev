// Decompiled with JetBrains decompiler
// Type: VRage.Game.ModAPI.IMyModel
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using VRageMath;

namespace VRage.Game.ModAPI
{
  public interface IMyModel
  {
    int UniqueId { get; }

    int DataVersion { get; }

    BoundingSphere BoundingSphere { get; }

    BoundingBox BoundingBox { get; }

    Vector3 BoundingBoxSize { get; }

    Vector3 BoundingBoxSizeHalf { get; }

    Vector3I[] BoneMapping { get; }

    float PatternScale { get; }

    float ScaleFactor { get; }

    string AssetName { get; }

    int GetTrianglesCount();

    int GetVerticesCount();

    int GetDummies(IDictionary<string, IMyModelDummy> dummies);
  }
}
