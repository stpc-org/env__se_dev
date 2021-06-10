// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.IMyWrappedCubemapFace
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public interface IMyWrappedCubemapFace : IDisposable
  {
    void CopyRange(
      Vector2I start,
      Vector2I end,
      IMyWrappedCubemapFace other,
      Vector2I oStart,
      Vector2I oEnd);

    void FinishFace(string name);

    int Resolution { get; }

    int ResolutionMinusOne { get; }
  }
}
