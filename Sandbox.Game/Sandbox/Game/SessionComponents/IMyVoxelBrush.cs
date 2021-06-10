// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.IMyVoxelBrush
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  public interface IMyVoxelBrush
  {
    float MinScale { get; }

    float MaxScale { get; }

    bool AutoRotate { get; }

    void Fill(MyVoxelBase map, byte matId);

    void Paint(MyVoxelBase map, byte matId);

    void CutOut(MyVoxelBase map);

    void Revert(MyVoxelBase map);

    void SetRotation(ref MatrixD rotationMat);

    void SetPosition(ref Vector3D targetPosition);

    Vector3D GetPosition();

    bool ShowRotationGizmo();

    BoundingBoxD PeekWorldBoundingBox(ref Vector3D targetPosition);

    BoundingBoxD GetBoundaries();

    BoundingBoxD GetWorldBoundaries();

    void Draw(ref Color color);

    List<MyGuiControlBase> GetGuiControls();

    string BrushIcon { get; }

    string SubtypeName { get; }

    void ScaleShapeUp();

    void ScaleShapeDown();
  }
}
