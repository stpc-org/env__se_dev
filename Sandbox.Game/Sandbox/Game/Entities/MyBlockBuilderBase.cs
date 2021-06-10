// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyBlockBuilderBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
  public abstract class MyBlockBuilderBase : MySessionComponentBase
  {
    protected static readonly MyStringId[] m_rotationControls = new MyStringId[6]
    {
      MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE,
      MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE,
      MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE,
      MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE,
      MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE,
      MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE
    };
    protected static MyCubeBuilderDefinition m_cubeBuilderDefinition;
    private static float m_intersectionDistance;
    protected static readonly int[] m_rotationDirections = new int[6]
    {
      -1,
      1,
      1,
      -1,
      1,
      -1
    };
    protected MyCubeGrid m_currentGrid;
    protected MatrixD m_invGridWorldMatrix = MatrixD.Identity;
    protected MyVoxelBase m_currentVoxelBase;
    protected MyPhysics.HitInfo? m_hitInfo;
    private static IMyPlacementProvider m_placementProvider;

    public static float IntersectionDistance
    {
      get => MyBlockBuilderBase.m_intersectionDistance;
      set
      {
        MyBlockBuilderBase.m_intersectionDistance = value;
        if (MyBlockBuilderBase.PlacementProvider == null)
          return;
        MyBlockBuilderBase.PlacementProvider.IntersectionDistance = value;
      }
    }

    protected internal abstract MyCubeGrid CurrentGrid { get; protected set; }

    protected internal abstract MyVoxelBase CurrentVoxelBase { get; protected set; }

    public abstract MyCubeBlockDefinition CurrentBlockDefinition { get; protected set; }

    public MyPhysics.HitInfo? HitInfo => this.m_hitInfo;

    private static bool AdminSpectatorIsBuilding => MyFakes.ENABLE_ADMIN_SPECTATOR_BUILDING && MySession.Static != null && (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator && MyMultiplayer.Static != null) && MySession.Static.IsUserAdmin(Sync.MyId);

    private static bool DeveloperSpectatorIsBuilding => MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Spectator && !MySession.Static.SurvivalMode;

    public static bool SpectatorIsBuilding => MyBlockBuilderBase.DeveloperSpectatorIsBuilding || MyBlockBuilderBase.AdminSpectatorIsBuilding;

    public static bool CameraControllerSpectator
    {
      get
      {
        MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
        switch (cameraControllerEnum)
        {
          case MyCameraControllerEnum.Spectator:
          case MyCameraControllerEnum.SpectatorDelta:
            return true;
          default:
            return cameraControllerEnum == MyCameraControllerEnum.SpectatorOrbit;
        }
      }
    }

    public static Vector3D IntersectionStart => MyBlockBuilderBase.PlacementProvider.RayStart;

    public static Vector3D IntersectionDirection => MyBlockBuilderBase.PlacementProvider.RayDirection;

    public Vector3D FreePlacementTarget => MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * (double) MyBlockBuilderBase.IntersectionDistance;

    public static IMyPlacementProvider PlacementProvider
    {
      get => MyBlockBuilderBase.m_placementProvider;
      set => MyBlockBuilderBase.m_placementProvider = value ?? (IMyPlacementProvider) new MyDefaultPlacementProvider(MyBlockBuilderBase.IntersectionDistance);
    }

    public static MyCubeBuilderDefinition CubeBuilderDefinition => MyBlockBuilderBase.m_cubeBuilderDefinition;

    static MyBlockBuilderBase() => MyBlockBuilderBase.PlacementProvider = (IMyPlacementProvider) new MyDefaultPlacementProvider(MyBlockBuilderBase.IntersectionDistance);

    public override void InitFromDefinition(MySessionComponentDefinition definition)
    {
      base.InitFromDefinition(definition);
      MyBlockBuilderBase.m_cubeBuilderDefinition = definition as MyCubeBuilderDefinition;
      MyCubeBuilderDefinition builderDefinition = MyBlockBuilderBase.m_cubeBuilderDefinition;
      MyBlockBuilderBase.IntersectionDistance = MyBlockBuilderBase.m_cubeBuilderDefinition.DefaultBlockBuildingDistance;
    }

    public abstract bool IsActivated { get; }

    public abstract void Activate(MyDefinitionId? blockDefinitionId = null);

    public abstract void Deactivate();

    protected internal virtual void ChooseHitObject()
    {
      MyCubeGrid closestGrid;
      MyVoxelBase closestVoxelMap;
      this.FindClosestPlacementObject(out closestGrid, out closestVoxelMap);
      this.CurrentGrid = closestGrid;
      this.CurrentVoxelBase = closestVoxelMap;
      this.m_invGridWorldMatrix = this.CurrentGrid != null ? this.CurrentGrid.PositionComp.WorldMatrixInvScaled : MatrixD.Identity;
    }

    protected static void AddFastBuildModelWithSubparts(
      ref MatrixD matrix,
      List<MatrixD> matrices,
      List<string> models,
      MyCubeBlockDefinition blockDefinition,
      float gridScale)
    {
      if (string.IsNullOrEmpty(blockDefinition.Model))
        return;
      matrices.Add(matrix);
      models.Add(blockDefinition.Model);
      MyEntitySubpart.Data outData = new MyEntitySubpart.Data();
      MyModel modelOnlyData = MyModels.GetModelOnlyData(blockDefinition.Model);
      modelOnlyData.Rescale(gridScale);
      foreach (KeyValuePair<string, MyModelDummy> dummy in modelOnlyData.Dummies)
      {
        if (MyEntitySubpart.GetSubpartFromDummy(blockDefinition.Model, dummy.Key, dummy.Value, ref outData))
        {
          MyModels.GetModelOnlyData(outData.File)?.Rescale(gridScale);
          MatrixD matrixD = MatrixD.Multiply((MatrixD) ref outData.InitialTransform, matrix);
          matrices.Add(matrixD);
          models.Add(outData.File);
        }
        else
        {
          MyCubeBlockDefinition subBlockDefinition;
          MatrixD subBlockMatrix;
          if (MyFakes.ENABLE_SUBBLOCKS && MyCubeBlock.GetSubBlockDataFromDummy(blockDefinition, dummy.Key, dummy.Value, false, out subBlockDefinition, out subBlockMatrix, out Vector3 _) && !string.IsNullOrEmpty(subBlockDefinition.Model))
          {
            MyModels.GetModelOnlyData(subBlockDefinition.Model)?.Rescale(gridScale);
            Vector3I vector2 = Vector3I.Round(Vector3.DominantAxisProjection((Vector3) subBlockMatrix.Forward));
            Vector3I vector3I = Vector3I.One - Vector3I.Abs(vector2);
            Vector3I vector1 = Vector3I.Round(Vector3.DominantAxisProjection((Vector3) subBlockMatrix.Right * vector3I));
            Vector3I result;
            Vector3I.Cross(ref vector1, ref vector2, out result);
            subBlockMatrix.Forward = (Vector3D) vector2;
            subBlockMatrix.Right = (Vector3D) vector1;
            subBlockMatrix.Up = (Vector3D) result;
            MatrixD matrixD = MatrixD.Multiply(subBlockMatrix, matrix);
            matrices.Add(matrixD);
            models.Add(subBlockDefinition.Model);
          }
        }
      }
      if (!MyFakes.ENABLE_GENERATED_BLOCKS || blockDefinition.IsGeneratedBlock || blockDefinition.GeneratedBlockDefinitions == null)
        return;
      foreach (MyDefinitionId generatedBlockDefinition in blockDefinition.GeneratedBlockDefinitions)
      {
        MyCubeBlockDefinition blockDefinition1;
        if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(generatedBlockDefinition, out blockDefinition1))
          MyModels.GetModelOnlyData(blockDefinition1.Model)?.Rescale(gridScale);
      }
    }

    public MyCubeGrid FindClosestGrid() => MyBlockBuilderBase.PlacementProvider.ClosestGrid;

    public bool FindClosestPlacementObject(
      out MyCubeGrid closestGrid,
      out MyVoxelBase closestVoxelMap)
    {
      closestGrid = (MyCubeGrid) null;
      closestVoxelMap = (MyVoxelBase) null;
      this.m_hitInfo = new MyPhysics.HitInfo?();
      if (MySession.Static.ControlledEntity == null)
        return false;
      closestGrid = MyBlockBuilderBase.PlacementProvider.ClosestGrid;
      closestVoxelMap = MyBlockBuilderBase.PlacementProvider.ClosestVoxelMap;
      this.m_hitInfo = MyBlockBuilderBase.PlacementProvider.HitInfo;
      return closestGrid != null || closestVoxelMap != null;
    }

    protected Vector3I? IntersectCubes(MyCubeGrid grid, out double distance)
    {
      distance = 3.40282346638529E+38;
      LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * (double) MyBlockBuilderBase.IntersectionDistance);
      Vector3I zero = Vector3I.Zero;
      double maxValue = double.MaxValue;
      if (!grid.GetLineIntersectionExactGrid(ref line, ref zero, ref maxValue))
        return new Vector3I?();
      distance = Math.Sqrt(maxValue);
      return new Vector3I?(zero);
    }

    protected Vector3D? IntersectExact(
      MyCubeGrid grid,
      ref MatrixD inverseGridWorldMatrix,
      out Vector3D intersection,
      out MySlimBlock intersectedBlock)
    {
      intersection = Vector3D.Zero;
      LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * (double) MyBlockBuilderBase.IntersectionDistance);
      double distance;
      Vector3D? intersectionExactAll = grid.GetLineIntersectionExactAll(ref line, out distance, out intersectedBlock);
      if (intersectionExactAll.HasValue)
      {
        Vector3D vector3D1 = Vector3D.Transform(MyBlockBuilderBase.IntersectionStart, inverseGridWorldMatrix);
        Vector3D vector3D2 = Vector3D.Normalize(Vector3D.TransformNormal(MyBlockBuilderBase.IntersectionDirection, inverseGridWorldMatrix));
        intersection = vector3D1 + distance * vector3D2;
        intersection *= 1.0 / (double) grid.GridSize;
      }
      return intersectionExactAll;
    }

    protected Vector3D? GetIntersectedBlockData(
      ref MatrixD inverseGridWorldMatrix,
      out Vector3D intersection,
      out MySlimBlock intersectedBlock,
      out ushort? compoundBlockId)
    {
      intersection = Vector3D.Zero;
      intersectedBlock = (MySlimBlock) null;
      compoundBlockId = new ushort?();
      if (this.CurrentGrid == null)
        return new Vector3D?();
      double distanceSquared = double.MaxValue;
      Vector3D? nullable1 = new Vector3D?();
      LineD line = new LineD(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * (double) MyBlockBuilderBase.IntersectionDistance);
      Vector3I zero = Vector3I.Zero;
      if (!this.CurrentGrid.GetLineIntersectionExactGrid(ref line, ref zero, ref distanceSquared, new MyPhysics.HitInfo?(this.m_hitInfo.Value)))
        return new Vector3D?();
      distanceSquared = Math.Sqrt(distanceSquared);
      nullable1 = new Vector3D?((Vector3D) zero);
      intersectedBlock = this.CurrentGrid.GetCubeBlock(zero);
      if (intersectedBlock == null)
        return new Vector3D?();
      if (intersectedBlock.FatBlock is MyCompoundCubeBlock)
      {
        MyCompoundCubeBlock fatBlock = intersectedBlock.FatBlock as MyCompoundCubeBlock;
        ushort? nullable2 = new ushort?();
        ushort blockId;
        if (fatBlock.GetIntersectionWithLine(ref line, out MyIntersectionResultLineTriangleEx? _, out blockId))
          nullable2 = new ushort?(blockId);
        else if (fatBlock.GetBlocksCount() == 1)
          nullable2 = fatBlock.GetBlockId(fatBlock.GetBlocks()[0]);
        compoundBlockId = nullable2;
      }
      Vector3D vector3D1 = Vector3D.Transform(MyBlockBuilderBase.IntersectionStart, inverseGridWorldMatrix);
      Vector3D vector3D2 = Vector3D.Normalize(Vector3D.TransformNormal(MyBlockBuilderBase.IntersectionDirection, inverseGridWorldMatrix));
      intersection = vector3D1 + distanceSquared * vector3D2;
      intersection *= 1.0 / (double) this.CurrentGrid.GridSize;
      return nullable1;
    }

    protected void IntersectInflated(List<Vector3I> outHitPositions, MyCubeGrid grid)
    {
      float maxDist = 2000f;
      Vector3I gridSizeInflate = new Vector3I(100, 100, 100);
      if (grid != null)
      {
        MyBlockBuilderBase.PlacementProvider.RayCastGridCells(grid, outHitPositions, gridSizeInflate, maxDist);
      }
      else
      {
        float cubeSize = MyDefinitionManager.Static.GetCubeSize(this.CurrentBlockDefinition.CubeSize);
        MyCubeGrid.RayCastStaticCells(MyBlockBuilderBase.IntersectionStart, MyBlockBuilderBase.IntersectionStart + MyBlockBuilderBase.IntersectionDirection * (double) maxDist, outHitPositions, cubeSize, new Vector3I?(gridSizeInflate));
      }
    }

    protected BoundingBoxD GetCubeBoundingBox(Vector3I cubePos)
    {
      Vector3D vector3D = (Vector3D) (cubePos * this.CurrentGrid.GridSize);
      Vector3 vector3 = new Vector3(0.06f, 0.06f, 0.06f);
      return new BoundingBoxD(vector3D - new Vector3D((double) this.CurrentGrid.GridSize / 2.0) - vector3, vector3D + new Vector3D((double) this.CurrentGrid.GridSize / 2.0) + vector3);
    }

    protected bool GetCubeAddAndRemovePositions(
      Vector3I intersectedCube,
      bool placingSmallGridOnLargeStatic,
      out Vector3I addPos,
      out Vector3I addDir,
      out Vector3I removePos)
    {
      bool flag = false;
      addPos = new Vector3I();
      addDir = new Vector3I();
      removePos = new Vector3I();
      MatrixD matrix = this.CurrentGrid.PositionComp.WorldMatrixInvScaled;
      addPos = intersectedCube;
      addDir = Vector3I.Forward;
      Vector3D vector3D = Vector3D.Transform(MyBlockBuilderBase.IntersectionStart, matrix);
      Vector3D direction = Vector3D.Normalize(Vector3D.TransformNormal(MyBlockBuilderBase.IntersectionDirection, matrix));
      RayD ray = new RayD(vector3D, direction);
      for (int index = 0; index < 100; ++index)
      {
        BoundingBoxD cubeBoundingBox = this.GetCubeBoundingBox(addPos);
        if (placingSmallGridOnLargeStatic || cubeBoundingBox.Contains(vector3D) != ContainmentType.Contains)
        {
          double? nullable = cubeBoundingBox.Intersects(ray);
          if (nullable.HasValue)
          {
            removePos = addPos;
            Vector3I vector3I = Vector3I.Sign(Vector3.DominantAxisProjection((Vector3) (vector3D + direction * nullable.Value - removePos * this.CurrentGrid.GridSize)));
            addPos = removePos + vector3I;
            addDir = vector3I;
            if (!this.CurrentGrid.CubeExists(addPos))
            {
              flag = true;
              break;
            }
          }
          else
            break;
        }
        else
          break;
      }
      return flag;
    }

    protected bool GetBlockAddPosition(
      float gridSize,
      bool placingSmallGridOnLargeStatic,
      out MySlimBlock intersectedBlock,
      out Vector3D intersectedBlockPos,
      out Vector3D intersectExactPos,
      out Vector3I addPositionBlock,
      out Vector3I addDirectionBlock,
      out ushort? compoundBlockId)
    {
      intersectedBlock = (MySlimBlock) null;
      intersectedBlockPos = new Vector3D();
      intersectExactPos = (Vector3D) new Vector3();
      addDirectionBlock = new Vector3I();
      addPositionBlock = new Vector3I();
      compoundBlockId = new ushort?();
      if (this.CurrentVoxelBase != null)
      {
        Vector3I intVector = Base6Directions.GetIntVector(Base6Directions.GetClosestDirection(this.m_hitInfo.Value.HkHitInfo.Normal));
        double num = (double) MyBlockBuilderBase.IntersectionDistance * (double) this.m_hitInfo.Value.HkHitInfo.HitFraction;
        Vector3D intersectionStart = MyBlockBuilderBase.IntersectionStart;
        Vector3D vector3D1 = Vector3D.Normalize(MyBlockBuilderBase.IntersectionDirection);
        Vector3D vector3D2 = num * vector3D1;
        Vector3D worldPos = intersectionStart + vector3D2;
        addPositionBlock = MyCubeGrid.StaticGlobalGrid_WorldToUGInt(worldPos + 0.1f * Vector3.Half * intVector * gridSize, gridSize, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter);
        addDirectionBlock = intVector;
        intersectedBlockPos = (Vector3D) (addPositionBlock - intVector);
        intersectExactPos = MyCubeGrid.StaticGlobalGrid_WorldToUG(worldPos, gridSize, MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.StaticGridAlignToCenter);
        intersectExactPos = (Vector3D.One - Vector3.Abs((Vector3) intVector)) * intersectExactPos + (intersectedBlockPos + 0.5f * intVector) * Vector3.Abs((Vector3) intVector);
        return true;
      }
      Vector3D? intersectedBlockData = this.GetIntersectedBlockData(ref this.m_invGridWorldMatrix, out intersectExactPos, out intersectedBlock, out compoundBlockId);
      if (!intersectedBlockData.HasValue)
        return false;
      intersectedBlockPos = intersectedBlockData.Value;
      Vector3I removePos;
      if (!this.GetCubeAddAndRemovePositions(Vector3I.Round(intersectedBlockPos), placingSmallGridOnLargeStatic, out addPositionBlock, out addDirectionBlock, out removePos))
        return false;
      if (!placingSmallGridOnLargeStatic)
      {
        if (MyFakes.ENABLE_BLOCK_PLACING_ON_INTERSECTED_POSITION)
        {
          Vector3I vector3I = Vector3I.Round(intersectedBlockPos);
          if (vector3I != removePos)
          {
            if (this.m_hitInfo.HasValue)
            {
              Vector3I intVector = Base6Directions.GetIntVector(Base6Directions.GetClosestDirection(this.m_hitInfo.Value.HkHitInfo.Normal));
              addDirectionBlock = intVector;
            }
            removePos = vector3I;
            addPositionBlock = removePos + addDirectionBlock;
          }
        }
        else if (this.CurrentGrid.CubeExists(addPositionBlock))
          return false;
      }
      if (placingSmallGridOnLargeStatic)
        removePos = Vector3I.Round(intersectedBlockPos);
      intersectedBlockPos = (Vector3D) removePos;
      intersectedBlock = this.CurrentGrid.GetCubeBlock(removePos);
      return intersectedBlock != null;
    }

    public static void ComputeSteps(
      Vector3I start,
      Vector3I end,
      Vector3I rotatedSize,
      out Vector3I stepDelta,
      out Vector3I counter,
      out int stepCount)
    {
      Vector3I vector3I = end - start;
      stepDelta = Vector3I.Sign(vector3I) * rotatedSize;
      counter = Vector3I.Abs(end - start) / rotatedSize + Vector3I.One;
      stepCount = counter.Size;
    }
  }
}
