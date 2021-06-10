// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyMultiBlockClipboard
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems.CoordinateSystem;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
  public class MyMultiBlockClipboard : MyGridClipboardAdvanced
  {
    private static List<Vector3D> m_tmpCollisionPoints = new List<Vector3D>();
    private static List<MyEntity> m_tmpNearEntities = new List<MyEntity>();
    private MyMultiBlockDefinition m_multiBlockDefinition;
    public MySlimBlock RemoveBlock;
    public ushort? BlockIdInCompound;
    private Vector3I m_addPos;
    public HashSet<Tuple<MySlimBlock, ushort?>> RemoveBlocksInMultiBlock = new HashSet<Tuple<MySlimBlock, ushort?>>();
    private HashSet<Vector3I> m_tmpBlockPositionsSet = new HashSet<Vector3I>();
    private bool m_lastVoxelState;

    protected override bool AnyCopiedGridIsStatic => false;

    public MyMultiBlockClipboard(MyPlacementSettings settings, bool calculateVelocity = true)
      : base(settings, calculateVelocity)
      => this.m_useDynamicPreviews = false;

    public override void Deactivate(bool afterPaste = false)
    {
      this.m_multiBlockDefinition = (MyMultiBlockDefinition) null;
      base.Deactivate(afterPaste);
    }

    public override void Update()
    {
      if (!this.IsActive)
        return;
      this.UpdateHitEntity();
      if (!this.m_visible)
      {
        this.ShowPreview(false);
      }
      else
      {
        if (this.PreviewGrids.Count == 0)
          return;
        if ((double) this.m_dragDistance == 0.0)
          this.SetupDragDistance();
        if ((double) this.m_dragDistance > (double) MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance)
          this.m_dragDistance = MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance;
        this.UpdatePastePosition();
        this.UpdateGridTransformations();
        this.FixSnapTransformationBase6();
        if (this.m_calculateVelocity)
          this.m_objectVelocity = (Vector3) ((this.m_pastePosition - this.m_pastePositionPrevious) / 0.0166666675359011);
        this.m_canBePlaced = this.TestPlacement();
        if (!this.m_visible)
        {
          this.ShowPreview(false);
        }
        else
        {
          this.ShowPreview(true);
          this.TestBuildingMaterials();
          this.m_canBePlaced &= this.CharacterHasEnoughMaterials;
          this.UpdatePreview();
          if (!MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
            return;
          MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 0.0f), "FW: " + this.m_pasteDirForward.ToString(), Color.Red, 1f);
          MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 20f), "UP: " + this.m_pasteDirUp.ToString(), Color.Red, 1f);
          MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 40f), "AN: " + this.m_pasteOrientationAngle.ToString(), Color.Red, 1f);
        }
      }
    }

    public override bool PasteGrid(bool deactivate = true, bool showWarning = true)
    {
      if (this.CopiedGrids.Count > 0 && !this.IsActive)
      {
        this.Activate((Action) null);
        return true;
      }
      if (!this.m_canBePlaced)
      {
        MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
        return false;
      }
      if (this.PreviewGrids.Count == 0)
        return false;
      bool flag = this.RemoveBlock != null && !this.RemoveBlock.CubeGrid.IsStatic;
      return !(MyCubeBuilder.Static.DynamicMode | flag) ? this.PasteGridsInStaticMode(deactivate) : this.PasteGridsInDynamicMode(deactivate);
    }

    public override bool EntityCanPaste(MyEntity pastingEntity)
    {
      if (this.CopiedGrids.Count < 1)
        return false;
      if (MySession.Static.CreativeToolsEnabled(Sync.MyId))
        return true;
      MyCubeBuilder.BuildComponent.GetMultiBlockPlacementMaterials(this.m_multiBlockDefinition);
      return MyCubeBuilder.BuildComponent.HasBuildingMaterials(pastingEntity);
    }

    private bool PasteGridsInDynamicMode(bool deactivate)
    {
      List<bool> boolList = new List<bool>();
      foreach (MyObjectBuilder_CubeGrid copiedGrid in this.CopiedGrids)
      {
        boolList.Add(copiedGrid.IsStatic);
        copiedGrid.IsStatic = false;
        this.BeforeCreateGrid(copiedGrid);
      }
      bool flag = this.PasteGridInternal(deactivate, multiBlock: true);
      for (int index = 0; index < this.CopiedGrids.Count; ++index)
        this.CopiedGrids[index].IsStatic = boolList[index];
      return flag;
    }

    private bool PasteGridsInStaticMode(bool deactivate)
    {
      List<MyObjectBuilder_CubeGrid> objectBuilderCubeGridList = new List<MyObjectBuilder_CubeGrid>();
      List<MatrixD> matrixDList = new List<MatrixD>();
      MyObjectBuilder_CubeGrid copiedGrid1 = this.CopiedGrids[0];
      this.BeforeCreateGrid(copiedGrid1);
      objectBuilderCubeGridList.Add(copiedGrid1);
      MatrixD worldMatrix1 = this.PreviewGrids[0].WorldMatrix;
      this.CopiedGrids[0] = MyCubeBuilder.ConvertGridBuilderToStatic(copiedGrid1, worldMatrix1);
      matrixDList.Add(worldMatrix1);
      for (int index = 1; index < this.CopiedGrids.Count; ++index)
      {
        MyObjectBuilder_CubeGrid copiedGrid2 = this.CopiedGrids[index];
        this.BeforeCreateGrid(copiedGrid2);
        objectBuilderCubeGridList.Add(copiedGrid2);
        MatrixD worldMatrix2 = this.PreviewGrids[index].WorldMatrix;
        matrixDList.Add(worldMatrix2);
        if (this.CopiedGrids[index].IsStatic)
        {
          MyObjectBuilder_CubeGrid objectBuilderCubeGrid = MyCubeBuilder.ConvertGridBuilderToStatic(copiedGrid2, worldMatrix2);
          this.CopiedGrids[index] = objectBuilderCubeGrid;
        }
      }
      bool flag = this.PasteGridInternal(deactivate, touchingGrids: this.m_touchingGrids, multiBlock: true);
      this.CopiedGrids.Clear();
      this.CopiedGrids.AddRange((IEnumerable<MyObjectBuilder_CubeGrid>) objectBuilderCubeGridList);
      for (int index = 0; index < this.PreviewGrids.Count; ++index)
        this.PreviewGrids[index].WorldMatrix = matrixDList[index];
      return flag;
    }

    protected override void UpdatePastePosition()
    {
      this.m_pastePositionPrevious = this.m_pastePosition;
      if (MyCubeBuilder.Static.HitInfo.HasValue)
        this.m_pastePosition = MyCubeBuilder.Static.HitInfo.Value.Position;
      else
        this.m_pastePosition = MyCubeBuilder.Static.FreePlacementTarget;
      double cubeSize = (double) MyDefinitionManager.Static.GetCubeSize(this.CopiedGrids[0].GridSizeEnum);
      MyCoordinateSystem.CoordSystemData closestGrid = MyCoordinateSystem.Static.SnapWorldPosToClosestGrid(ref this.m_pastePosition, cubeSize, this.m_settings.StaticGridAlignToCenter);
      this.EnableStationRotation = MyCubeBuilder.Static.DynamicMode;
      if (MyCubeBuilder.Static.DynamicMode)
      {
        this.AlignClipboardToGravity();
        this.m_visible = true;
        this.IsSnapped = false;
        this.m_lastVoxelState = false;
      }
      else if (this.RemoveBlock != null)
      {
        this.m_pastePosition = Vector3D.Transform(this.m_addPos * this.RemoveBlock.CubeGrid.GridSize, this.RemoveBlock.CubeGrid.WorldMatrix);
        if (!this.IsSnapped && this.RemoveBlock.CubeGrid.IsStatic)
        {
          this.m_pasteOrientationAngle = 0.0f;
          MatrixD worldMatrix = this.RemoveBlock.CubeGrid.WorldMatrix;
          this.m_pasteDirForward = (Vector3) worldMatrix.Forward;
          worldMatrix = this.RemoveBlock.CubeGrid.WorldMatrix;
          this.m_pasteDirUp = (Vector3) worldMatrix.Up;
        }
        this.IsSnapped = true;
        this.m_lastVoxelState = false;
      }
      else
      {
        if (!MyFakes.ENABLE_BLOCK_PLACEMENT_ON_VOXEL || !(this.m_hitEntity is MyVoxelBase))
          return;
        if (MyCoordinateSystem.Static.LocalCoordExist)
        {
          this.m_pastePosition = closestGrid.SnappedTransform.Position;
          if (!this.m_lastVoxelState)
            this.AlignRotationToCoordSys();
        }
        this.IsSnapped = true;
        this.m_lastVoxelState = true;
      }
    }

    public override void MoveEntityFurther()
    {
      if (!MyCubeBuilder.Static.DynamicMode)
        return;
      base.MoveEntityFurther();
      if ((double) this.m_dragDistance <= (double) MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance)
        return;
      this.m_dragDistance = MyBlockBuilderBase.CubeBuilderDefinition.MaxBlockBuildingDistance;
    }

    public override void MoveEntityCloser()
    {
      if (!MyCubeBuilder.Static.DynamicMode)
        return;
      base.MoveEntityCloser();
      if ((double) this.m_dragDistance >= (double) MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance)
        return;
      this.m_dragDistance = MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance;
    }

    protected override void ChangeClipboardPreview(
      bool visible,
      List<MyCubeGrid> previewGrids,
      List<MyObjectBuilder_CubeGrid> copiedGrids)
    {
      base.ChangeClipboardPreview(visible, previewGrids, copiedGrids);
      if (!visible || !MySession.Static.SurvivalMode)
        return;
      foreach (MyCubeGrid previewGrid in this.PreviewGrids)
      {
        foreach (MySlimBlock block1 in previewGrid.GetBlocks())
        {
          if (block1.FatBlock is MyCompoundCubeBlock fatBlock)
          {
            foreach (MySlimBlock block2 in fatBlock.GetBlocks())
              MyMultiBlockClipboard.SetBlockToFullIntegrity(block2);
          }
          else
            MyMultiBlockClipboard.SetBlockToFullIntegrity(block1);
        }
      }
    }

    private static void SetBlockToFullIntegrity(MySlimBlock block)
    {
      float buildRatio = block.ComponentStack.BuildRatio;
      block.ComponentStack.SetIntegrity(block.ComponentStack.MaxIntegrity, block.ComponentStack.MaxIntegrity);
      if (!block.BlockDefinition.ModelChangeIsNeeded(buildRatio, block.ComponentStack.BuildRatio))
        return;
      block.UpdateVisual(true);
    }

    private void UpdateHitEntity()
    {
      this.m_closestHitDistSq = float.MaxValue;
      this.m_hitPos = (Vector3D) new Vector3(0.0f, 0.0f, 0.0f);
      this.m_hitNormal = new Vector3(1f, 0.0f, 0.0f);
      this.m_hitEntity = (IMyEntity) null;
      this.m_addPos = Vector3I.Zero;
      this.RemoveBlock = (MySlimBlock) null;
      this.BlockIdInCompound = new ushort?();
      this.RemoveBlocksInMultiBlock.Clear();
      this.m_dynamicBuildAllowed = false;
      this.m_visible = false;
      this.m_canBePlaced = false;
      if (MyCubeBuilder.Static.DynamicMode)
      {
        this.m_visible = true;
      }
      else
      {
        MatrixD pasteMatrix = MyGridClipboard.GetPasteMatrix();
        if (MyCubeBuilder.Static.CurrentGrid == null && MyCubeBuilder.Static.CurrentVoxelBase == null)
          MyCubeBuilder.Static.ChooseHitObject();
        MyPhysics.HitInfo? hitInfo = MyCubeBuilder.Static.HitInfo;
        if (hitInfo.HasValue)
        {
          float cubeSize = MyDefinitionManager.Static.GetCubeSize(this.CopiedGrids[0].GridSizeEnum);
          hitInfo = MyCubeBuilder.Static.HitInfo;
          bool placingSmallGridOnLargeStatic = hitInfo.Value.HkHitInfo.GetHitEntity() is MyCubeGrid hitEntity && hitEntity.IsStatic && (hitEntity.GridSizeEnum == MyCubeSize.Large && this.CopiedGrids[0].GridSizeEnum == MyCubeSize.Small) && MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE;
          Vector3I addDir;
          if (MyCubeBuilder.Static.GetAddAndRemovePositions(cubeSize, placingSmallGridOnLargeStatic, out this.m_addPos, out Vector3? _, out addDir, out Vector3I _, out this.RemoveBlock, out this.BlockIdInCompound, this.RemoveBlocksInMultiBlock))
          {
            if (this.RemoveBlock != null)
            {
              hitInfo = MyCubeBuilder.Static.HitInfo;
              this.m_hitPos = hitInfo.Value.Position;
              this.m_closestHitDistSq = (float) (this.m_hitPos - pasteMatrix.Translation).LengthSquared();
              this.m_hitNormal = (Vector3) addDir;
              this.m_hitEntity = (IMyEntity) this.RemoveBlock.CubeGrid;
              if ((double) MyDefinitionManager.Static.GetCubeSize(this.RemoveBlock.CubeGrid.GridSizeEnum) / (double) MyDefinitionManager.Static.GetCubeSize(this.CopiedGrids[0].GridSizeEnum) < 1.0)
                this.RemoveBlock = (MySlimBlock) null;
              this.m_visible = this.RemoveBlock != null;
            }
            else
            {
              if (MyFakes.ENABLE_BLOCK_PLACEMENT_ON_VOXEL)
              {
                hitInfo = MyCubeBuilder.Static.HitInfo;
                if (hitInfo.Value.HkHitInfo.GetHitEntity() is MyVoxelBase)
                {
                  hitInfo = MyCubeBuilder.Static.HitInfo;
                  this.m_hitPos = hitInfo.Value.Position;
                  this.m_closestHitDistSq = (float) (this.m_hitPos - pasteMatrix.Translation).LengthSquared();
                  this.m_hitNormal = (Vector3) addDir;
                  hitInfo = MyCubeBuilder.Static.HitInfo;
                  this.m_hitEntity = (IMyEntity) (hitInfo.Value.HkHitInfo.GetHitEntity() as MyVoxelBase);
                  this.m_visible = true;
                  return;
                }
              }
              this.m_visible = false;
            }
          }
          else
            this.m_visible = false;
        }
        else
          this.m_visible = false;
      }
    }

    private new void FixSnapTransformationBase6()
    {
      if (this.CopiedGrids.Count == 0 || !(this.m_hitEntity is MyCubeGrid hitEntity))
        return;
      Matrix deltaMatrixToHitGrid = this.GetRotationDeltaMatrixToHitGrid(hitEntity);
      foreach (MyCubeGrid previewGrid in this.PreviewGrids)
      {
        MatrixD matrixD = previewGrid.WorldMatrix;
        matrixD = matrixD.GetOrientation();
        Matrix matrix = (Matrix) ref matrixD;
        matrix *= deltaMatrixToHitGrid;
        MatrixD world = MatrixD.CreateWorld(this.m_pastePosition, matrix.Forward, matrix.Up);
        previewGrid.PositionComp.SetWorldMatrix(ref world);
      }
      if ((hitEntity.GridSizeEnum != MyCubeSize.Large ? 0 : (this.PreviewGrids[0].GridSizeEnum == MyCubeSize.Small ? 1 : 0)) != 0)
      {
        Vector3 smallGrid = MyCubeBuilder.TransformLargeGridHitCoordToSmallGrid(this.m_hitPos, hitEntity.PositionComp.WorldMatrixNormalizedInv, hitEntity.GridSize);
        this.m_pastePosition = hitEntity.GridIntegerToWorld((Vector3D) smallGrid);
      }
      else
      {
        Vector3I vector1 = Vector3I.Round(this.m_hitNormal);
        Vector3I gridInteger = hitEntity.WorldToGridInteger(this.m_pastePosition);
        Vector3I vector2 = Vector3I.Abs(Vector3I.Round(Vector3D.TransformNormal(Vector3D.TransformNormal((Vector3D) (this.PreviewGrids[0].Max - this.PreviewGrids[0].Min + Vector3I.One), this.PreviewGrids[0].WorldMatrix), hitEntity.PositionComp.WorldMatrixNormalizedInv)));
        int num1 = Math.Abs(Vector3I.Dot(ref vector1, ref vector2));
        int num2;
        for (num2 = 0; num2 < num1 && !hitEntity.CanMergeCubes(this.PreviewGrids[0], gridInteger); ++num2)
          gridInteger += vector1;
        if (num2 == num1)
          gridInteger = hitEntity.WorldToGridInteger(this.m_pastePosition);
        this.m_pastePosition = hitEntity.GridIntegerToWorld(gridInteger);
      }
      for (int index = 0; index < this.PreviewGrids.Count; ++index)
      {
        MyCubeGrid previewGrid = this.PreviewGrids[index];
        MatrixD worldMatrix = previewGrid.WorldMatrix;
        worldMatrix.Translation = this.m_pastePosition + Vector3.Transform(this.m_copiedGridOffsets[index], deltaMatrixToHitGrid);
        previewGrid.PositionComp.SetWorldMatrix(ref worldMatrix);
      }
      if (!MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
        return;
      MyRenderProxy.DebugDrawLine3D(this.m_hitPos, this.m_hitPos + this.m_hitNormal, Color.Red, Color.Green, false);
    }

    public Matrix GetRotationDeltaMatrixToHitGrid(MyCubeGrid hitGrid)
    {
      MatrixD matrixD1 = hitGrid.WorldMatrix;
      matrixD1 = matrixD1.GetOrientation();
      Matrix axisDefinitionMatrix = (Matrix) ref matrixD1;
      MatrixD matrixD2 = this.PreviewGrids[0].WorldMatrix;
      matrixD2 = matrixD2.GetOrientation();
      Matrix toAlign = (Matrix) ref matrixD2;
      Matrix axes = Matrix.AlignRotationToAxes(ref toAlign, ref axisDefinitionMatrix);
      return Matrix.Invert(toAlign) * axes;
    }

    private bool TestPlacement()
    {
      bool flag1 = true;
      this.m_touchingGrids.Clear();
      for (int index = 0; index < this.PreviewGrids.Count; ++index)
      {
        MyCubeGrid previewGrid = this.PreviewGrids[index];
        if (!Sandbox.Game.Entities.MyEntities.IsInsideWorld(previewGrid.PositionComp.GetPosition()))
          return false;
        MyGridPlacementSettings placementSettings1 = this.m_settings.GetGridPlacementSettings(previewGrid.GridSizeEnum);
        this.m_touchingGrids.Add((MyCubeGrid) null);
        if (MySession.Static.SurvivalMode && !MyBlockBuilderBase.SpectatorIsBuilding && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
        {
          if (index == 0 && MyBlockBuilderBase.CameraControllerSpectator)
          {
            this.m_visible = false;
            return false;
          }
          if (index == 0 && !MyCubeBuilder.Static.DynamicMode)
          {
            MatrixD invGridWorldMatrix = previewGrid.PositionComp.WorldMatrixNormalizedInv;
            if (!MyCubeBuilderGizmo.DefaultGizmoCloseEnough(ref invGridWorldMatrix, (BoundingBoxD) previewGrid.PositionComp.LocalAABB, previewGrid.GridSize, MyBlockBuilderBase.IntersectionDistance))
            {
              this.m_visible = false;
              return false;
            }
          }
          if (!flag1)
            return false;
        }
        if (MyCubeBuilder.Static.DynamicMode)
        {
          MyGridPlacementSettings settings = previewGrid.GridSizeEnum == MyCubeSize.Large ? this.m_settings.LargeGrid : this.m_settings.SmallGrid;
          bool flag2 = false;
          foreach (MySlimBlock block in previewGrid.GetBlocks())
          {
            BoundingBoxD localAabb = new BoundingBoxD((Vector3D) (block.Min * this.PreviewGrids[index].GridSize - Vector3.Half * this.PreviewGrids[index].GridSize), (Vector3D) (block.Max * this.PreviewGrids[index].GridSize + Vector3.Half * this.PreviewGrids[index].GridSize));
            if (!flag2)
              flag2 = MyGridClipboardAdvanced.TestVoxelPlacement(block, ref placementSettings1, true);
            flag1 = flag1 && MyCubeGrid.TestPlacementArea(previewGrid, previewGrid.IsStatic, ref settings, localAabb, true, testVoxel: false);
            if (!flag1)
              break;
          }
          flag1 &= flag2;
        }
        else if (index == 0 && this.m_hitEntity is MyCubeGrid && this.IsSnapped)
        {
          MyCubeGrid hitEntity = this.m_hitEntity as MyCubeGrid;
          MyGridPlacementSettings placementSettings2 = this.m_settings.GetGridPlacementSettings(hitEntity.GridSizeEnum, hitEntity.IsStatic);
          flag1 = (hitEntity.GridSizeEnum != MyCubeSize.Large ? 0 : (previewGrid.GridSizeEnum == MyCubeSize.Small ? 1 : 0)) == 0 ? flag1 && this.TestGridPlacementOnGrid(previewGrid, ref placementSettings2, hitEntity) : flag1 && MyCubeGrid.TestPlacementArea(previewGrid, ref placementSettings1, (BoundingBoxD) previewGrid.PositionComp.LocalAABB, false);
          this.m_touchingGrids.Clear();
          this.m_touchingGrids.Add(hitEntity);
        }
        else if (index == 0 && this.m_hitEntity is MyVoxelMap)
        {
          bool flag2 = false;
          foreach (MySlimBlock cubeBlock in previewGrid.CubeBlocks)
          {
            if (cubeBlock.FatBlock is MyCompoundCubeBlock)
            {
              foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
              {
                if (!flag2)
                  flag2 = MyGridClipboardAdvanced.TestVoxelPlacement(block, ref placementSettings1, false);
                flag1 = flag1 && MyGridClipboardAdvanced.TestBlockPlacementArea(block, ref placementSettings1, false, false);
                if (!flag1)
                  break;
              }
            }
            else
            {
              if (!flag2)
                flag2 = MyGridClipboardAdvanced.TestVoxelPlacement(cubeBlock, ref placementSettings1, false);
              flag1 = flag1 && MyGridClipboardAdvanced.TestBlockPlacementArea(cubeBlock, ref placementSettings1, false, false);
            }
            if (!flag1)
              break;
          }
          flag1 &= flag2;
          this.m_touchingGrids[index] = this.DetectTouchingGrid();
        }
        else
        {
          MyGridPlacementSettings placementSettings2 = this.m_settings.GetGridPlacementSettings(previewGrid.GridSizeEnum, previewGrid.IsStatic && !MyCubeBuilder.Static.DynamicMode);
          flag1 = flag1 && MyCubeGrid.TestPlacementArea(previewGrid, previewGrid.IsStatic, ref placementSettings2, (BoundingBoxD) previewGrid.PositionComp.LocalAABB, false);
        }
        BoundingBoxD localAabb1 = (BoundingBoxD) previewGrid.PositionComp.LocalAABB;
        MatrixD matrix = previewGrid.PositionComp.WorldMatrixNormalizedInv;
        if (MySector.MainCamera != null)
        {
          Vector3D point = Vector3D.Transform(MySector.MainCamera.Position, matrix);
          flag1 = flag1 && localAabb1.Contains(point) != ContainmentType.Contains;
        }
        if (flag1)
        {
          MyMultiBlockClipboard.m_tmpCollisionPoints.Clear();
          MyCubeBuilder.PrepareCharacterCollisionPoints(MyMultiBlockClipboard.m_tmpCollisionPoints);
          foreach (Vector3D tmpCollisionPoint in MyMultiBlockClipboard.m_tmpCollisionPoints)
          {
            Vector3D point = Vector3D.Transform(tmpCollisionPoint, matrix);
            flag1 = flag1 && localAabb1.Contains(point) != ContainmentType.Contains;
            if (!flag1)
              break;
          }
        }
      }
      return flag1;
    }

    private MyCubeGrid DetectTouchingGrid()
    {
      if (this.PreviewGrids == null || this.PreviewGrids.Count == 0)
        return (MyCubeGrid) null;
      foreach (MySlimBlock cubeBlock in this.PreviewGrids[0].CubeBlocks)
      {
        MyCubeGrid myCubeGrid = this.DetectTouchingGrid(cubeBlock);
        if (myCubeGrid != null)
          return myCubeGrid;
      }
      return (MyCubeGrid) null;
    }

    private MyCubeGrid DetectTouchingGrid(MySlimBlock block)
    {
      if (MyCubeBuilder.Static.DynamicMode)
        return (MyCubeGrid) null;
      if (block == null)
        return (MyCubeGrid) null;
      if (block.FatBlock is MyCompoundCubeBlock)
      {
        foreach (MySlimBlock block1 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
        {
          MyCubeGrid myCubeGrid = this.DetectTouchingGrid(block1);
          if (myCubeGrid != null)
            return myCubeGrid;
        }
        return (MyCubeGrid) null;
      }
      float gridSize = block.CubeGrid.GridSize;
      BoundingBoxD aabb;
      block.GetWorldBoundingBox(out aabb, false);
      aabb.Inflate((double) gridSize / 2.0);
      MyMultiBlockClipboard.m_tmpNearEntities.Clear();
      Sandbox.Game.Entities.MyEntities.GetElementsInBox(ref aabb, MyMultiBlockClipboard.m_tmpNearEntities);
      MyCubeBlockDefinition.MountPoint[] modelMountPoints = block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio);
      try
      {
        for (int index = 0; index < MyMultiBlockClipboard.m_tmpNearEntities.Count; ++index)
        {
          if (MyMultiBlockClipboard.m_tmpNearEntities[index] is MyCubeGrid tmpNearEntity && tmpNearEntity != block.CubeGrid && (tmpNearEntity.Physics != null && tmpNearEntity.Physics.Enabled) && (tmpNearEntity.IsStatic && tmpNearEntity.GridSizeEnum == block.CubeGrid.GridSizeEnum))
          {
            Vector3I gridInteger = tmpNearEntity.WorldToGridInteger(this.m_pastePosition);
            if (tmpNearEntity.CanMergeCubes(block.CubeGrid, gridInteger))
            {
              MatrixI mergeTransform = tmpNearEntity.CalculateMergeTransform(block.CubeGrid, gridInteger);
              Quaternion result;
              new MyBlockOrientation(mergeTransform.GetDirection(block.Orientation.Forward), mergeTransform.GetDirection(block.Orientation.Up)).GetQuaternion(out result);
              Vector3I position = Vector3I.Transform(block.Position, mergeTransform);
              if (MyCubeGrid.CheckConnectivity((IMyGridConnectivityTest) tmpNearEntity, block.BlockDefinition, modelMountPoints, ref result, ref position))
                return tmpNearEntity;
            }
          }
        }
      }
      finally
      {
        MyMultiBlockClipboard.m_tmpNearEntities.Clear();
      }
      return (MyCubeGrid) null;
    }

    private void UpdatePreview()
    {
      if (this.PreviewGrids == null || !this.m_visible || !this.HasPreviewBBox)
        return;
      MyStringId myStringId = this.m_canBePlaced ? MyGridClipboard.ID_GIZMO_DRAW_LINE : MyGridClipboard.ID_GIZMO_DRAW_LINE_RED;
      Color white = Color.White;
      foreach (MyCubeGrid previewGrid in this.PreviewGrids)
      {
        BoundingBoxD localAabb = (BoundingBoxD) previewGrid.PositionComp.LocalAABB;
        MatrixD worldMatrix = previewGrid.PositionComp.WorldMatrixRef;
        MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localAabb, ref white, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, lineMaterial: new MyStringId?(myStringId));
      }
      Vector4 vector4 = new Vector4(Color.Red.ToVector3() * 0.8f, 1f);
      if (this.RemoveBlocksInMultiBlock.Count > 0)
      {
        this.m_tmpBlockPositionsSet.Clear();
        MyCubeBuilder.GetAllBlocksPositions(this.RemoveBlocksInMultiBlock, this.m_tmpBlockPositionsSet);
        foreach (Vector3I tmpBlockPositions in this.m_tmpBlockPositionsSet)
          MyCubeBuilder.DrawSemiTransparentBox(tmpBlockPositions, tmpBlockPositions, this.RemoveBlock.CubeGrid, (Color) vector4, lineMaterial: new MyStringId?(MyGridClipboard.ID_GIZMO_DRAW_LINE_RED));
        this.m_tmpBlockPositionsSet.Clear();
      }
      else
      {
        if (this.RemoveBlock == null)
          return;
        MyCubeBuilder.DrawSemiTransparentBox(this.RemoveBlock.CubeGrid, this.RemoveBlock, (Color) vector4, lineMaterial: new MyStringId?(MyGridClipboard.ID_GIZMO_DRAW_LINE_RED));
      }
    }

    protected override void SetupDragDistance()
    {
      if (!this.IsActive)
        return;
      base.SetupDragDistance();
      if (!MySession.Static.SurvivalMode || MySession.Static.CreativeToolsEnabled(Sync.MyId))
        return;
      this.m_dragDistance = MyBlockBuilderBase.IntersectionDistance;
    }

    public void SetGridFromBuilder(
      MyMultiBlockDefinition multiBlockDefinition,
      MyObjectBuilder_CubeGrid grid,
      Vector3 dragPointDelta,
      float dragVectorLength)
    {
      this.ChangeClipboardPreview(false, this.m_previewGrids, this.m_copiedGrids);
      this.m_multiBlockDefinition = multiBlockDefinition;
      this.SetGridFromBuilder(grid, dragPointDelta, dragVectorLength);
      this.ChangeClipboardPreview(true, this.m_previewGrids, this.m_copiedGrids);
    }

    public static void TakeMaterialsFromBuilder(
      List<MyObjectBuilder_CubeGrid> blocksToBuild,
      MyEntity builder)
    {
      if (blocksToBuild.Count == 0)
        return;
      MyObjectBuilder_CubeBlock builderCubeBlock = blocksToBuild[0].CubeBlocks.FirstOrDefault<MyObjectBuilder_CubeBlock>();
      if (builderCubeBlock == null)
        return;
      MyDefinitionId id;
      if (builderCubeBlock is MyObjectBuilder_CompoundCubeBlock compoundCubeBlock)
      {
        if (compoundCubeBlock.Blocks == null || compoundCubeBlock.Blocks.Length == 0 || !compoundCubeBlock.Blocks[0].MultiBlockDefinition.HasValue)
          return;
        id = (MyDefinitionId) compoundCubeBlock.Blocks[0].MultiBlockDefinition.Value;
      }
      else
      {
        if (!builderCubeBlock.MultiBlockDefinition.HasValue)
          return;
        id = (MyDefinitionId) builderCubeBlock.MultiBlockDefinition.Value;
      }
      MyMultiBlockDefinition multiBlockDefinition = MyDefinitionManager.Static.TryGetMultiBlockDefinition(id);
      if (multiBlockDefinition == null)
        return;
      MyCubeBuilder.BuildComponent.GetMultiBlockPlacementMaterials(multiBlockDefinition);
      MyCubeBuilder.BuildComponent.AfterSuccessfulBuild(builder, false);
    }
  }
}
