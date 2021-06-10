// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyGridClipboardAdvanced
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
  public class MyGridClipboardAdvanced : MyGridClipboard
  {
    private static List<Vector3D> m_tmpCollisionPoints = new List<Vector3D>();
    protected bool m_dynamicBuildAllowed;

    protected override bool AnyCopiedGridIsStatic => false;

    public MyGridClipboardAdvanced(MyPlacementSettings settings, bool calculateVelocity = true)
      : base(settings, calculateVelocity)
    {
      this.m_useDynamicPreviews = false;
      this.m_dragDistance = 0.0f;
    }

    public override void Update()
    {
      if (!this.IsActive || !this.m_visible)
        return;
      bool flag = this.UpdateHitEntity(false);
      if (MyFakes.ENABLE_VR_BUILDING && !flag)
        this.Hide();
      else if (!this.m_visible)
      {
        this.Hide();
      }
      else
      {
        this.Show();
        if ((double) this.m_dragDistance == 0.0)
          this.SetupDragDistance();
        this.UpdatePastePosition();
        this.UpdateGridTransformations();
        if (MyCubeBuilder.Static.CubePlacementMode != MyCubeBuilder.CubePlacementModeEnum.FreePlacement)
          this.FixSnapTransformationBase6();
        if (this.m_calculateVelocity)
          this.m_objectVelocity = (Vector3) ((this.m_pastePosition - this.m_pastePositionPrevious) / 0.0166666675359011);
        this.m_canBePlaced = this.TestPlacement();
        this.TestBuildingMaterials();
        this.UpdatePreview();
        if (!MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
          return;
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 0.0f), "FW: " + this.m_pasteDirForward.ToString(), Color.Red, 1f);
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 20f), "UP: " + this.m_pasteDirUp.ToString(), Color.Red, 1f);
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 40f), "AN: " + this.m_pasteOrientationAngle.ToString(), Color.Red, 1f);
      }
    }

    public override void Activate(Action callback = null)
    {
      base.Activate(callback);
      this.SetupDragDistance();
    }

    private static void ConvertGridBuilderToStatic(
      MyObjectBuilder_CubeGrid originalGrid,
      MatrixD worldMatrix)
    {
      originalGrid.IsStatic = true;
      originalGrid.PositionAndOrientation = new MyPositionAndOrientation?(new MyPositionAndOrientation((Vector3D) originalGrid.PositionAndOrientation.Value.Position, Vector3.Forward, Vector3.Up));
      Vector3 forward = (Vector3) worldMatrix.Forward;
      Vector3 up1 = (Vector3) worldMatrix.Up;
      Base6Directions.Direction closestDirection = Base6Directions.GetClosestDirection(forward);
      Base6Directions.Direction up2 = Base6Directions.GetClosestDirection(up1);
      if (up2 == closestDirection)
        up2 = Base6Directions.GetPerpendicular(closestDirection);
      MatrixI transform = new MatrixI(Vector3I.Zero, closestDirection, up2);
      foreach (MyObjectBuilder_CubeBlock cubeBlock in originalGrid.CubeBlocks)
      {
        if (cubeBlock is MyObjectBuilder_CompoundCubeBlock)
        {
          MyObjectBuilder_CompoundCubeBlock origBlock = cubeBlock as MyObjectBuilder_CompoundCubeBlock;
          MyGridClipboardAdvanced.ConvertRotatedGridCompoundBlockToStatic(ref transform, origBlock);
          for (int index = 0; index < origBlock.Blocks.Length; ++index)
          {
            MyObjectBuilder_CubeBlock block = origBlock.Blocks[index];
            MyGridClipboardAdvanced.ConvertRotatedGridBlockToStatic(ref transform, block);
          }
        }
        else
          MyGridClipboardAdvanced.ConvertRotatedGridBlockToStatic(ref transform, cubeBlock);
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
      bool flag = this.m_hitEntity is MyCubeGrid && !((MyCubeGrid) this.m_hitEntity).IsStatic && !MyCubeBuilder.Static.DynamicMode;
      return !MyCubeBuilder.Static.DynamicMode ? (!flag ? (!MyFakes.ENABLE_VR_BUILDING ? this.PasteGridsInStaticMode(deactivate) : this.PasteGridInternal(deactivate)) : this.PasteGridInternal(deactivate)) : this.PasteGridsInDynamicMode(deactivate);
    }

    private bool PasteGridsInDynamicMode(bool deactivate)
    {
      List<bool> boolList = new List<bool>();
      foreach (MyObjectBuilder_CubeGrid copiedGrid in this.CopiedGrids)
      {
        boolList.Add(copiedGrid.IsStatic);
        copiedGrid.IsStatic = false;
      }
      bool flag = this.PasteGridInternal(deactivate);
      for (int index = 0; index < this.CopiedGrids.Count; ++index)
        this.CopiedGrids[index].IsStatic = boolList[index];
      return flag;
    }

    private bool PasteGridsInStaticMode(bool deactivate)
    {
      MyObjectBuilder_CubeGrid copiedGrid1 = this.CopiedGrids[0];
      MatrixD worldMatrix1 = this.PreviewGrids[0].WorldMatrix;
      MatrixD worldMatrix2 = worldMatrix1;
      MyGridClipboardAdvanced.ConvertGridBuilderToStatic(copiedGrid1, worldMatrix2);
      this.PreviewGrids[0].WorldMatrix = MatrixD.CreateTranslation(worldMatrix1.Translation);
      for (int index = 1; index < this.CopiedGrids.Count; ++index)
      {
        if (this.CopiedGrids[index].IsStatic)
        {
          MyObjectBuilder_CubeGrid copiedGrid2 = this.CopiedGrids[index];
          MatrixD worldMatrix3 = this.PreviewGrids[index].WorldMatrix;
          MatrixD worldMatrix4 = worldMatrix3;
          MyGridClipboardAdvanced.ConvertGridBuilderToStatic(copiedGrid2, worldMatrix4);
          this.PreviewGrids[index].WorldMatrix = MatrixD.CreateTranslation(worldMatrix3.Translation);
        }
      }
      List<MyObjectBuilder_CubeGrid> pastedBuilders = new List<MyObjectBuilder_CubeGrid>();
      int num = this.PasteGridInternal(true, pastedBuilders, this.m_touchingGrids, (MyGridClipboard.UpdateAfterPasteCallback) (pastedBuildersInCallback => this.UpdateAfterPaste(deactivate, pastedBuildersInCallback))) ? 1 : 0;
      if (num == 0)
        return num != 0;
      this.UpdateAfterPaste(deactivate, pastedBuilders);
      return num != 0;
    }

    private void UpdateAfterPaste(bool deactivate, List<MyObjectBuilder_CubeGrid> pastedBuilders)
    {
      if (this.CopiedGrids.Count != pastedBuilders.Count)
        return;
      this.m_copiedGridOffsets.Clear();
      for (int index = 0; index < this.CopiedGrids.Count; ++index)
      {
        this.CopiedGrids[index].PositionAndOrientation = pastedBuilders[index].PositionAndOrientation;
        this.m_copiedGridOffsets.Add((Vector3) ((Vector3D) this.CopiedGrids[index].PositionAndOrientation.Value.Position - (Vector3D) this.CopiedGrids[0].PositionAndOrientation.Value.Position));
      }
      this.m_pasteOrientationAngle = 0.0f;
      this.m_pasteDirForward = (Vector3) Vector3I.Forward;
      this.m_pasteDirUp = (Vector3) Vector3I.Up;
      if (deactivate)
        return;
      this.Activate((Action) null);
    }

    private static void ConvertRotatedGridBlockToStatic(
      ref MatrixI transform,
      MyObjectBuilder_CubeBlock origBlock)
    {
      MyCubeBlockDefinition blockDefinition;
      MyDefinitionManager.Static.TryGetCubeBlockDefinition(new MyDefinitionId(origBlock.TypeId, origBlock.SubtypeName), out blockDefinition);
      if (blockDefinition == null)
        return;
      MyBlockOrientation blockOrientation = (MyBlockOrientation) origBlock.BlockOrientation;
      Vector3I min = (Vector3I) origBlock.Min;
      Vector3I max;
      MySlimBlock.ComputeMax(blockDefinition, blockOrientation, ref min, out max);
      Vector3I result1;
      Vector3I.Transform(ref min, ref transform, out result1);
      Vector3I result2;
      Vector3I.Transform(ref max, ref transform, out result2);
      Quaternion result3;
      new MyBlockOrientation(transform.GetDirection(blockOrientation.Forward), transform.GetDirection(blockOrientation.Up)).GetQuaternion(out result3);
      origBlock.Orientation = (SerializableQuaternion) result3;
      origBlock.Min = (SerializableVector3I) Vector3I.Min(result1, result2);
    }

    private static void ConvertRotatedGridCompoundBlockToStatic(
      ref MatrixI transform,
      MyObjectBuilder_CompoundCubeBlock origBlock)
    {
      MyCubeBlockDefinition blockDefinition;
      MyDefinitionManager.Static.TryGetCubeBlockDefinition(new MyDefinitionId(origBlock.TypeId, origBlock.SubtypeName), out blockDefinition);
      if (blockDefinition == null)
        return;
      MyBlockOrientation blockOrientation = (MyBlockOrientation) origBlock.BlockOrientation;
      Vector3I min = (Vector3I) origBlock.Min;
      Vector3I max;
      MySlimBlock.ComputeMax(blockDefinition, blockOrientation, ref min, out max);
      Vector3I result1;
      Vector3I.Transform(ref min, ref transform, out result1);
      Vector3I result2;
      Vector3I.Transform(ref max, ref transform, out result2);
      origBlock.Min = (SerializableVector3I) Vector3I.Min(result1, result2);
    }

    protected override void UpdatePastePosition()
    {
      this.m_pastePositionPrevious = this.m_pastePosition;
      if (MyCubeBuilder.Static.DynamicMode)
      {
        this.m_visible = true;
        this.IsSnapped = false;
        this.m_pastePosition = MyBlockBuilderBase.IntersectionStart + (double) this.m_dragDistance * MyBlockBuilderBase.IntersectionDirection;
        this.m_pastePosition = this.m_pastePosition + (Vector3D) Vector3.TransformNormal(this.m_dragPointToPositionLocal, this.GetFirstGridOrientationMatrix());
      }
      else
      {
        this.m_visible = true;
        if (!this.IsSnapped)
        {
          this.m_pasteOrientationAngle = 0.0f;
          this.m_pasteDirForward = (Vector3) Vector3I.Forward;
          this.m_pasteDirUp = (Vector3) Vector3I.Up;
        }
        this.IsSnapped = true;
        MatrixD pasteMatrix = MyGridClipboard.GetPasteMatrix();
        Vector3 vector3 = (Vector3) (pasteMatrix.Forward * (double) this.m_dragDistance);
        if (!this.TrySnapToSurface(this.m_settings.GetGridPlacementSettings(this.PreviewGrids[0].GridSizeEnum).SnapMode))
        {
          this.m_pastePosition = pasteMatrix.Translation + vector3;
          this.m_pastePosition = this.m_pastePosition + (Vector3D) Vector3.TransformNormal(this.m_dragPointToPositionLocal, this.GetFirstGridOrientationMatrix());
          this.IsSnapped = true;
        }
        if (MyFakes.ENABLE_VR_BUILDING)
          return;
        double gridSize = (double) this.PreviewGrids[0].GridSize;
        if (this.m_settings.StaticGridAlignToCenter)
          this.m_pastePosition = Vector3I.Round(this.m_pastePosition / gridSize) * gridSize;
        else
          this.m_pastePosition = Vector3I.Round(this.m_pastePosition / gridSize + 0.5) * gridSize - 0.5 * gridSize;
      }
    }

    public void SetDragDistance(float dragDistance) => this.m_dragDistance = dragDistance;

    private static double DistanceFromCharacterPlane(ref Vector3D point) => Vector3D.Dot(point - MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionDirection);

    private new bool TestPlacement()
    {
      bool flag1 = true;
      this.m_touchingGrids.Clear();
      for (int index = 0; index < this.PreviewGrids.Count; ++index)
      {
        MyCubeGrid previewGrid = this.PreviewGrids[index];
        this.m_touchingGrids.Add((MyCubeGrid) null);
        if (MyCubeBuilder.Static.DynamicMode)
        {
          if (!this.m_dynamicBuildAllowed)
          {
            MyGridPlacementSettings placementSettings = this.m_settings.GetGridPlacementSettings(previewGrid.GridSizeEnum, false);
            BoundingBoxD localAabb = (BoundingBoxD) previewGrid.PositionComp.LocalAABB;
            MatrixD worldMatrix = previewGrid.WorldMatrix;
            if (MyFakes.ENABLE_VOXEL_MAP_AABB_CORNER_TEST)
              flag1 = flag1 && MyCubeGrid.TestPlacementVoxelMapOverlap((MyVoxelBase) null, ref placementSettings, ref localAabb, ref worldMatrix);
            flag1 = flag1 && MyCubeGrid.TestPlacementArea(previewGrid, false, ref placementSettings, localAabb, true);
            if (!flag1)
              break;
          }
        }
        else if (index == 0 && this.m_hitEntity is MyCubeGrid && (this.IsSnapped && this.SnapMode == SnapMode.Base6Directions))
        {
          MyGridPlacementSettings settings = previewGrid.GridSizeEnum == MyCubeSize.Large ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeStaticGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid;
          MyCubeGrid hitEntity = this.m_hitEntity as MyCubeGrid;
          if (hitEntity.GridSizeEnum == MyCubeSize.Small && previewGrid.GridSizeEnum == MyCubeSize.Large)
          {
            flag1 = false;
            break;
          }
          bool flag2 = hitEntity.GridSizeEnum == MyCubeSize.Large && previewGrid.GridSizeEnum == MyCubeSize.Small;
          if (MyFakes.ENABLE_STATIC_SMALL_GRID_ON_LARGE & flag2)
          {
            if (!hitEntity.IsStatic)
            {
              flag1 = false;
              break;
            }
            foreach (MySlimBlock cubeBlock in previewGrid.CubeBlocks)
            {
              if (cubeBlock.FatBlock is MyCompoundCubeBlock)
              {
                foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
                {
                  flag1 = flag1 && MyGridClipboardAdvanced.TestBlockPlacement(block, ref settings);
                  if (!flag1)
                    break;
                }
              }
              else
                flag1 = flag1 && MyGridClipboardAdvanced.TestBlockPlacement(cubeBlock, ref settings);
              if (!flag1)
                break;
            }
          }
          else
            flag1 = flag1 && this.TestGridPlacementOnGrid(previewGrid, ref settings, hitEntity);
        }
        else
        {
          MyCubeGrid myCubeGrid = (MyCubeGrid) null;
          MyGridPlacementSettings settings1 = index == 0 ? (previewGrid.GridSizeEnum == MyCubeSize.Large ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeStaticGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid) : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(previewGrid.GridSizeEnum);
          if (previewGrid.IsStatic)
          {
            if (index == 0)
            {
              MatrixD orientation = previewGrid.WorldMatrix.GetOrientation();
              Matrix gridLocalMatrix = (Matrix) ref orientation;
              flag1 = flag1 && MyCubeBuilder.CheckValidBlocksRotation(gridLocalMatrix, previewGrid);
            }
            foreach (MySlimBlock cubeBlock in previewGrid.CubeBlocks)
            {
              if (cubeBlock.FatBlock is MyCompoundCubeBlock)
              {
                foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
                {
                  MyCubeGrid touchingGrid = (MyCubeGrid) null;
                  flag1 = flag1 && MyGridClipboardAdvanced.TestBlockPlacementNoAABBInflate(block, ref settings1, out touchingGrid);
                  if (flag1 && touchingGrid != null && myCubeGrid == null)
                    myCubeGrid = touchingGrid;
                  if (!flag1)
                    break;
                }
              }
              else
              {
                MyCubeGrid touchingGrid = (MyCubeGrid) null;
                flag1 = flag1 && MyGridClipboardAdvanced.TestBlockPlacementNoAABBInflate(cubeBlock, ref settings1, out touchingGrid);
                if (flag1 && touchingGrid != null && myCubeGrid == null)
                  myCubeGrid = touchingGrid;
              }
              if (!flag1)
                break;
            }
            if (flag1 && myCubeGrid != null)
              this.m_touchingGrids[index] = myCubeGrid;
          }
          else
          {
            foreach (MySlimBlock cubeBlock in previewGrid.CubeBlocks)
            {
              BoundingBoxD localAabb = new BoundingBoxD((Vector3D) (cubeBlock.Min * this.PreviewGrids[index].GridSize - Vector3.Half * this.PreviewGrids[index].GridSize), (Vector3D) (cubeBlock.Max * this.PreviewGrids[index].GridSize + Vector3.Half * this.PreviewGrids[index].GridSize));
              flag1 = flag1 && MyCubeGrid.TestPlacementArea(previewGrid, previewGrid.IsStatic, ref settings1, localAabb, false);
              if (!flag1)
                break;
            }
            this.m_touchingGrids[index] = (MyCubeGrid) null;
          }
          if (flag1 && this.m_touchingGrids[index] != null)
          {
            MyGridPlacementSettings settings2 = previewGrid.GridSizeEnum == MyCubeSize.Large ? MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.LargeStaticGrid : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid;
            flag1 = flag1 && this.TestGridPlacementOnGrid(previewGrid, ref settings2, this.m_touchingGrids[index]);
          }
          if (flag1 && index == 0)
          {
            if ((previewGrid.GridSizeEnum != MyCubeSize.Small ? 0 : (previewGrid.IsStatic ? 1 : 0)) != 0 || !previewGrid.IsStatic)
            {
              MyGridPlacementSettings settings2 = index == 0 ? this.m_settings.GetGridPlacementSettings(previewGrid.GridSizeEnum, false) : MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.SmallStaticGrid;
              bool flag2 = true;
              foreach (MySlimBlock cubeBlock in previewGrid.CubeBlocks)
              {
                BoundingBoxD localAabb = new BoundingBoxD((Vector3D) (cubeBlock.Min * this.PreviewGrids[index].GridSize - Vector3.Half * this.PreviewGrids[index].GridSize), (Vector3D) (cubeBlock.Max * this.PreviewGrids[index].GridSize + Vector3.Half * this.PreviewGrids[index].GridSize));
                flag2 = flag2 && MyCubeGrid.TestPlacementArea(previewGrid, false, ref settings2, localAabb, false);
                if (!flag2)
                  break;
              }
              flag1 &= !flag2;
            }
            else if (this.m_touchingGrids[index] == null)
            {
              MyGridPlacementSettings placementSettings = this.m_settings.GetGridPlacementSettings(previewGrid.GridSizeEnum, index == 0 || previewGrid.IsStatic);
              MyCubeGrid touchingGrid = (MyCubeGrid) null;
              bool flag2 = false;
              foreach (MySlimBlock cubeBlock in previewGrid.CubeBlocks)
              {
                if (cubeBlock.FatBlock is MyCompoundCubeBlock)
                {
                  foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
                  {
                    flag2 |= MyGridClipboardAdvanced.TestBlockPlacementNoAABBInflate(block, ref placementSettings, out touchingGrid);
                    if (flag2)
                      break;
                  }
                }
                else
                  flag2 |= MyGridClipboardAdvanced.TestBlockPlacementNoAABBInflate(cubeBlock, ref placementSettings, out touchingGrid);
                if (flag2)
                  break;
              }
              flag1 &= flag2;
            }
          }
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
          MyGridClipboardAdvanced.m_tmpCollisionPoints.Clear();
          MyCubeBuilder.PrepareCharacterCollisionPoints(MyGridClipboardAdvanced.m_tmpCollisionPoints);
          foreach (Vector3D tmpCollisionPoint in MyGridClipboardAdvanced.m_tmpCollisionPoints)
          {
            Vector3D point = Vector3D.Transform(tmpCollisionPoint, matrix);
            flag1 = flag1 && localAabb1.Contains(point) != ContainmentType.Contains;
            if (!flag1)
              break;
          }
        }
        if (!flag1)
          break;
      }
      return flag1;
    }

    protected bool TestGridPlacementOnGrid(
      MyCubeGrid previewGrid,
      ref MyGridPlacementSettings settings,
      MyCubeGrid hitGrid)
    {
      bool flag1 = true;
      Vector3I gridInteger = hitGrid.WorldToGridInteger(previewGrid.PositionComp.WorldMatrixRef.Translation);
      MatrixI mergeTransform = hitGrid.CalculateMergeTransform(previewGrid, gridInteger);
      Matrix floatMatrix = mergeTransform.GetFloatMatrix();
      floatMatrix.Translation *= previewGrid.GridSize;
      if (MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE)
        MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, 60f), "First grid offset: " + gridInteger.ToString(), Color.Red, 1f);
      bool flag2 = flag1 && MyCubeBuilder.CheckValidBlocksRotation(floatMatrix, previewGrid) && hitGrid.GridSizeEnum == previewGrid.GridSizeEnum && hitGrid.CanMergeCubes(previewGrid, gridInteger) && MyCubeGrid.CheckMergeConnectivity(hitGrid, previewGrid, gridInteger);
      if (flag2)
      {
        bool flag3 = false;
        foreach (MySlimBlock cubeBlock in previewGrid.CubeBlocks)
        {
          if (cubeBlock.FatBlock is MyCompoundCubeBlock)
          {
            foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
            {
              flag3 |= MyGridClipboardAdvanced.CheckConnectivityOnGrid(block, ref mergeTransform, ref settings, hitGrid);
              if (flag3)
                break;
            }
          }
          else
            flag3 |= MyGridClipboardAdvanced.CheckConnectivityOnGrid(cubeBlock, ref mergeTransform, ref settings, hitGrid);
          if (flag3)
            break;
        }
        flag2 &= flag3;
      }
      if (flag2)
      {
        foreach (MySlimBlock cubeBlock in previewGrid.CubeBlocks)
        {
          if (cubeBlock.FatBlock is MyCompoundCubeBlock)
          {
            foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
            {
              flag2 = flag2 && MyGridClipboardAdvanced.TestBlockPlacementOnGrid(block, ref mergeTransform, ref settings, hitGrid);
              if (!flag2)
                break;
            }
          }
          else
            flag2 = flag2 && MyGridClipboardAdvanced.TestBlockPlacementOnGrid(cubeBlock, ref mergeTransform, ref settings, hitGrid);
          if (!flag2)
            break;
        }
      }
      return flag2;
    }

    protected static bool CheckConnectivityOnGrid(
      MySlimBlock block,
      ref MatrixI transform,
      ref MyGridPlacementSettings settings,
      MyCubeGrid hitGrid)
    {
      Vector3I result1;
      Vector3I.Transform(ref block.Position, ref transform, out result1);
      Quaternion result2;
      new MyBlockOrientation(transform.GetDirection(block.Orientation.Forward), transform.GetDirection(block.Orientation.Up)).GetQuaternion(out result2);
      MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
      return MyCubeGrid.CheckConnectivity((IMyGridConnectivityTest) hitGrid, blockDefinition, blockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio), ref result2, ref result1);
    }

    protected static bool TestBlockPlacementOnGrid(
      MySlimBlock block,
      ref MatrixI transform,
      ref MyGridPlacementSettings settings,
      MyCubeGrid hitGrid)
    {
      Vector3I result1;
      Vector3I.Transform(ref block.Min, ref transform, out result1);
      Vector3I result2;
      Vector3I.Transform(ref block.Max, ref transform, out result2);
      Vector3I min = Vector3I.Min(result1, result2);
      Vector3I max = Vector3I.Max(result1, result2);
      MyBlockOrientation orientation = new MyBlockOrientation(transform.GetDirection(block.Orientation.Forward), transform.GetDirection(block.Orientation.Up));
      return hitGrid.CanPlaceBlock(min, max, orientation, block.BlockDefinition, ref settings);
    }

    protected static bool TestBlockPlacement(
      MySlimBlock block,
      ref MyGridPlacementSettings settings)
    {
      return MyCubeGrid.TestPlacementAreaCube(block.CubeGrid, ref settings, block.Min, block.Max, block.Orientation, block.BlockDefinition, ignoredEntity: ((MyEntity) block.CubeGrid));
    }

    protected static bool TestBlockPlacement(
      MySlimBlock block,
      ref MyGridPlacementSettings settings,
      out MyCubeGrid touchingGrid)
    {
      return MyCubeGrid.TestPlacementAreaCube(block.CubeGrid, ref settings, block.Min, block.Max, block.Orientation, block.BlockDefinition, out touchingGrid, ignoredEntity: ((MyEntity) block.CubeGrid));
    }

    protected static bool TestBlockPlacementNoAABBInflate(
      MySlimBlock block,
      ref MyGridPlacementSettings settings,
      out MyCubeGrid touchingGrid)
    {
      return MyCubeGrid.TestPlacementAreaCubeNoAABBInflate(block.CubeGrid, ref settings, block.Min, block.Max, block.Orientation, block.BlockDefinition, out touchingGrid, ignoredEntity: ((MyEntity) block.CubeGrid));
    }

    protected static bool TestVoxelPlacement(
      MySlimBlock block,
      ref MyGridPlacementSettings settings,
      bool dynamicMode)
    {
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      invalid.Include((Vector3D) (block.Min * block.CubeGrid.GridSize - block.CubeGrid.GridSize / 2f));
      invalid.Include((Vector3D) (block.Max * block.CubeGrid.GridSize + block.CubeGrid.GridSize / 2f));
      return MyCubeGrid.TestVoxelPlacement(block.BlockDefinition, settings, dynamicMode, block.CubeGrid.WorldMatrix, invalid);
    }

    protected static bool TestBlockPlacementArea(
      MySlimBlock block,
      ref MyGridPlacementSettings settings,
      bool dynamicMode,
      bool testVoxel = true)
    {
      BoundingBoxD invalid = BoundingBoxD.CreateInvalid();
      invalid.Include((Vector3D) (block.Min * block.CubeGrid.GridSize - block.CubeGrid.GridSize / 2f));
      invalid.Include((Vector3D) (block.Max * block.CubeGrid.GridSize + block.CubeGrid.GridSize / 2f));
      return MyCubeGrid.TestBlockPlacementArea(block.BlockDefinition, new MyBlockOrientation?(block.Orientation), block.CubeGrid.WorldMatrix, ref settings, invalid, dynamicMode, (MyEntity) block.CubeGrid, testVoxel);
    }

    private void UpdatePreview()
    {
      if (this.PreviewGrids == null || !this.m_visible || !this.HasPreviewBBox)
        return;
      MyStringId myStringId = this.m_canBePlaced ? MyGridClipboard.ID_GIZMO_DRAW_LINE : MyGridClipboard.ID_GIZMO_DRAW_LINE_RED;
      if (MyFakes.ENABLE_VR_BUILDING && this.m_canBePlaced)
        return;
      Color white = Color.White;
      foreach (MyCubeGrid previewGrid in this.PreviewGrids)
      {
        BoundingBoxD localAabb = (BoundingBoxD) previewGrid.PositionComp.LocalAABB;
        MatrixD worldMatrix = previewGrid.PositionComp.WorldMatrixRef;
        MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localAabb, ref white, MySimpleObjectRasterizer.Wireframe, 1, 0.04f, lineMaterial: new MyStringId?(myStringId));
      }
    }

    internal void DynamicModeChanged()
    {
      if (!MyCubeBuilder.Static.DynamicMode)
        return;
      this.SetupDragDistance();
    }

    protected virtual void SetupDragDistance()
    {
      if (!this.IsActive)
        return;
      if (this.PreviewGrids.Count > 0)
      {
        double? currentRayIntersection = MyCubeBuilder.GetCurrentRayIntersection();
        if (currentRayIntersection.HasValue && (double) this.m_dragDistance > currentRayIntersection.Value)
          this.m_dragDistance = (float) currentRayIntersection.Value;
        float num = 2.5f * (float) this.PreviewGrids[0].PositionComp.WorldAABB.HalfExtents.Length();
        if ((double) this.m_dragDistance >= (double) num)
          return;
        this.m_dragDistance = num;
      }
      else
        this.m_dragDistance = 0.0f;
    }

    public override void MoveEntityCloser()
    {
      base.MoveEntityCloser();
      if ((double) this.m_dragDistance >= (double) MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance)
        return;
      this.m_dragDistance = MyBlockBuilderBase.CubeBuilderDefinition.MinBlockBuildingDistance;
    }
  }
}
