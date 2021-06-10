// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyProjectorClipboard
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.CoordinateSystem;
using System;
using System.Linq;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public class MyProjectorClipboard : MyGridClipboard
  {
    private MyProjectorBase m_projector;
    private Vector3I m_oldProjectorRotation;
    private Vector3I m_oldProjectorOffset;
    private float m_oldScale;
    private MatrixD m_oldProjectorMatrix;
    private bool m_firstUpdateAfterNewBlueprint;
    private bool m_hasPreviewBBox;
    private bool m_projectionCanBePlaced;

    public MyProjectorClipboard(MyProjectorBase projector, MyPlacementSettings settings)
      : base(settings)
    {
      this.m_enableUpdateHitEntity = false;
      this.m_projector = projector;
      this.m_calculateVelocity = false;
    }

    public override bool HasPreviewBBox
    {
      get => this.m_hasPreviewBBox;
      set => this.m_hasPreviewBBox = value;
    }

    protected override float Transparency => 0.0f;

    protected override bool CanBePlaced => this.m_projectionCanBePlaced;

    public void Clear()
    {
      this.CopiedGrids.Clear();
      this.m_copiedGridOffsets.Clear();
      this.m_oldScale = 1f;
    }

    protected override void TestBuildingMaterials() => this.m_characterHasEnoughMaterials = true;

    public bool HasGridsLoaded() => this.CopiedGrids != null && this.CopiedGrids.Count > 0;

    public void ProcessCubeGrid(MyObjectBuilder_CubeGrid gridBuilder)
    {
      gridBuilder.IsStatic = false;
      gridBuilder.DestructibleBlocks = false;
      foreach (MyObjectBuilder_CubeBlock cubeBlock in gridBuilder.CubeBlocks)
      {
        cubeBlock.Owner = 0L;
        cubeBlock.ShareMode = MyOwnershipShareModeEnum.None;
        cubeBlock.EntityId = 0L;
        if (cubeBlock is MyObjectBuilder_FunctionalBlock builderFunctionalBlock)
          builderFunctionalBlock.Enabled = false;
      }
    }

    protected override void UpdatePastePosition()
    {
      this.m_pastePositionPrevious = this.m_pastePosition;
      this.m_pastePosition = this.m_projector.WorldMatrix.Translation;
    }

    protected override bool TestPlacement() => Sandbox.Game.Entities.MyEntities.IsInsideWorld(this.m_pastePosition);

    public bool ActuallyTestPlacement()
    {
      this.m_projectionCanBePlaced = base.TestPlacement();
      MyCoordinateSystem.Static.Visible = false;
      return this.m_projectionCanBePlaced;
    }

    protected override MyEntity GetClipboardBuilder() => (MyEntity) null;

    public void ResetGridOrientation()
    {
      this.m_pasteDirForward = Vector3.Forward;
      this.m_pasteDirUp = Vector3.Up;
      this.m_pasteOrientationAngle = 0.0f;
    }

    protected override void UpdateGridTransformations()
    {
      if (this.PreviewGrids == null || this.PreviewGrids.Count == 0)
        return;
      MatrixD other = this.m_projector.WorldMatrix;
      if (!this.m_firstUpdateAfterNewBlueprint && !(this.m_oldProjectorRotation != this.m_projector.ProjectionRotation) && (!(this.m_oldProjectorOffset != this.m_projector.ProjectionOffset) && this.m_oldProjectorMatrix.EqualsFast(ref other)) && (double) this.m_projector.Scale == (double) this.m_oldScale)
        return;
      this.m_oldProjectorRotation = this.m_projector.ProjectionRotation;
      this.m_oldProjectorMatrix = other;
      this.m_oldProjectorOffset = this.m_projector.ProjectionOffset;
      Matrix fromQuaternion = Matrix.CreateFromQuaternion(this.m_projector.ProjectionRotationQuaternion);
      other = MatrixD.Multiply((MatrixD) ref fromQuaternion, other);
      Vector3 vector3 = (Vector3) Vector3D.Transform(this.m_projector.GetProjectionTranslationOffset(), this.m_projector.WorldMatrix.GetOrientation());
      other.Translation -= vector3;
      float scale = this.m_projector.Scale;
      MatrixD matrixD1 = MatrixD.Invert(this.PreviewGrids[0].WorldMatrix);
      Vector3D vector3D = Vector3D.Zero;
      for (int index = 0; index < this.PreviewGrids.Count; ++index)
      {
        MatrixD worldMatrix = other;
        if (index != 0)
        {
          MatrixD matrixD2 = this.PreviewGrids[index].WorldMatrix * matrixD1;
          matrixD2.Translation *= (double) this.m_projector.Scale;
          worldMatrix = matrixD2 * other;
        }
        if (!this.m_projector.AllowScaling && index == 0)
        {
          MySlimBlock mySlimBlock = this.PreviewGrids[index].CubeBlocks.First<MySlimBlock>();
          Vector3D world = MyCubeGrid.GridIntegerToWorld(this.PreviewGrids[index].GridSize, mySlimBlock.Position, worldMatrix);
          vector3D = worldMatrix.Translation - world;
        }
        worldMatrix.Translation += vector3D;
        this.PreviewGrids[index].PositionComp.Scale = new float?(scale);
        this.PreviewGrids[index].PositionComp.SetWorldMatrix(ref worldMatrix, skipTeleportCheck: true);
      }
      this.m_oldScale = this.m_projector.Scale;
      this.m_firstUpdateAfterNewBlueprint = false;
    }

    public float GridSize => this.CopiedGrids != null && this.CopiedGrids.Count > 0 ? MyDefinitionManager.Static.GetCubeSize(this.CopiedGrids[0].GridSizeEnum) : 0.0f;

    public override void Activate(Action callback = null)
    {
      this.ActivateNoAlign(callback);
      this.m_firstUpdateAfterNewBlueprint = true;
    }
  }
}
