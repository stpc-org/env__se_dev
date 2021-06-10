// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyCubeBlockCollector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  internal class MyCubeBlockCollector : IDisposable
  {
    public const bool SHRINK_CONVEX_SHAPE = true;
    private const bool ADD_INNER_BONES_TO_CONVEX = true;
    private const float MAX_BOX_EXTENT = 40f;
    public List<MyCubeBlockCollector.ShapeInfo> ShapeInfos = new List<MyCubeBlockCollector.ShapeInfo>();
    public List<HkShape> Shapes = new List<HkShape>();
    private HashSet<MySlimBlock> m_tmpRefreshSet = new HashSet<MySlimBlock>();
    private MyList<Vector3> m_tmpHelperVerts = new MyList<Vector3>();
    private List<Vector3I> m_tmpCubes = new List<Vector3I>();
    private HashSet<Vector3I> m_tmpCheck;

    public void Dispose() => this.Clear();

    public void Clear()
    {
      this.ShapeInfos.Clear();
      foreach (HkShape shape in this.Shapes)
        shape.RemoveReference();
      this.Shapes.Clear();
    }

    private bool IsValid()
    {
      if (this.m_tmpCheck == null)
        this.m_tmpCheck = new HashSet<Vector3I>();
      try
      {
        foreach (MyCubeBlockCollector.ShapeInfo shapeInfo in this.ShapeInfos)
        {
          Vector3I vector3I;
          for (vector3I.X = shapeInfo.Min.X; vector3I.X <= shapeInfo.Max.X; ++vector3I.X)
          {
            for (vector3I.Y = shapeInfo.Min.Y; vector3I.Y <= shapeInfo.Max.Y; ++vector3I.Y)
            {
              for (vector3I.Z = shapeInfo.Min.Z; vector3I.Z <= shapeInfo.Max.Z; ++vector3I.Z)
              {
                if (!this.m_tmpCheck.Add(vector3I))
                  return false;
              }
            }
          }
        }
        return true;
      }
      finally
      {
        this.m_tmpCheck.Clear();
      }
    }

    public void Collect(
      MyCubeGrid grid,
      MyVoxelSegmentation segmenter,
      MyVoxelSegmentationType segmentationType,
      IDictionary<Vector3I, HkMassElement> massResults)
    {
      foreach (MySlimBlock block in grid.GetBlocks())
      {
        if (block.FatBlock is MyCompoundCubeBlock)
          this.CollectCompoundBlock((MyCompoundCubeBlock) block.FatBlock, massResults);
        else
          this.CollectBlock(block, block.BlockDefinition.PhysicsOption, massResults);
      }
      this.AddSegmentedParts(grid.GridSize, segmenter, segmentationType);
      this.m_tmpCubes.Clear();
    }

    public void CollectArea(
      MyCubeGrid grid,
      HashSet<Vector3I> dirtyBlocks,
      MyVoxelSegmentation segmenter,
      MyVoxelSegmentationType segmentationType,
      IDictionary<Vector3I, HkMassElement> massResults)
    {
      using (MyUtils.ReuseCollection<MySlimBlock>(ref this.m_tmpRefreshSet))
      {
        foreach (Vector3I dirtyBlock in dirtyBlocks)
        {
          massResults?.Remove(dirtyBlock);
          MySlimBlock cubeBlock = grid.GetCubeBlock(dirtyBlock);
          if (cubeBlock != null)
            this.m_tmpRefreshSet.Add(cubeBlock);
        }
        foreach (MySlimBlock tmpRefresh in this.m_tmpRefreshSet)
          this.CollectBlock(tmpRefresh, tmpRefresh.BlockDefinition.PhysicsOption, massResults);
        this.AddSegmentedParts(grid.GridSize, segmenter, segmentationType);
        this.m_tmpCubes.Clear();
      }
    }

    public void CollectMassElements(
      MyCubeGrid grid,
      IDictionary<Vector3I, HkMassElement> massResults)
    {
      if (massResults == null)
        return;
      foreach (MySlimBlock block1 in grid.GetBlocks())
      {
        if (block1.FatBlock is MyCompoundCubeBlock)
        {
          foreach (MySlimBlock block2 in ((MyCompoundCubeBlock) block1.FatBlock).GetBlocks())
          {
            if (block2.BlockDefinition.BlockTopology == MyBlockTopology.TriangleMesh)
              this.AddMass(block2, massResults);
          }
        }
        else
          this.AddMass(block1, massResults);
      }
    }

    private void CollectCompoundBlock(
      MyCompoundCubeBlock compoundBlock,
      IDictionary<Vector3I, HkMassElement> massResults)
    {
      int count = this.ShapeInfos.Count;
      foreach (MySlimBlock block in compoundBlock.GetBlocks())
      {
        if (block.BlockDefinition.BlockTopology == MyBlockTopology.TriangleMesh)
          this.CollectBlock(block, block.BlockDefinition.PhysicsOption, massResults, false);
      }
      if (this.ShapeInfos.Count <= count + 1)
        return;
      MyCubeBlockCollector.ShapeInfo shapeInfo = this.ShapeInfos[count];
      while (this.ShapeInfos.Count > count + 1)
      {
        int index = this.ShapeInfos.Count - 1;
        shapeInfo.Count += this.ShapeInfos[index].Count;
        this.ShapeInfos.RemoveAt(index);
      }
      this.ShapeInfos[count] = shapeInfo;
    }

    private void AddSegmentedParts(
      float gridSize,
      MyVoxelSegmentation segmenter,
      MyVoxelSegmentationType segmentationType)
    {
      int num = (int) Math.Floor(40.0 / (double) gridSize);
      Vector3 vector3 = new Vector3(gridSize * 0.5f);
      if (segmenter != null)
      {
        int mergeIterations = segmentationType == MyVoxelSegmentationType.Optimized ? 1 : 0;
        segmenter.ClearInput();
        foreach (Vector3I tmpCube in this.m_tmpCubes)
          segmenter.AddInput(tmpCube);
        foreach (MyVoxelSegmentation.Segment segment in segmenter.FindSegments(segmentationType, mergeIterations))
        {
          Vector3I minPos;
          for (minPos.X = segment.Min.X; minPos.X <= segment.Max.X; minPos.X += num)
          {
            for (minPos.Y = segment.Min.Y; minPos.Y <= segment.Max.Y; minPos.Y += num)
            {
              for (minPos.Z = segment.Min.Z; minPos.Z <= segment.Max.Z; minPos.Z += num)
              {
                Vector3I maxPos = Vector3I.Min(minPos + num - 1, segment.Max);
                Vector3 min = minPos * gridSize - vector3;
                Vector3 max = maxPos * gridSize + vector3;
                this.AddBox(minPos, maxPos, ref min, ref max);
              }
            }
          }
        }
      }
      else
      {
        foreach (Vector3I tmpCube in this.m_tmpCubes)
        {
          Vector3 min = tmpCube * gridSize - vector3;
          Vector3 max = tmpCube * gridSize + vector3;
          this.AddBox(tmpCube, tmpCube, ref min, ref max);
        }
      }
    }

    private void AddBox(Vector3I minPos, Vector3I maxPos, ref Vector3 min, ref Vector3 max)
    {
      Vector3 translation = (min + max) * 0.5f;
      this.Shapes.Add((HkShape) new HkConvexTranslateShape((HkConvexShape) new HkBoxShape(max - translation - MyPerGameSettings.PhysicsConvexRadius, MyPerGameSettings.PhysicsConvexRadius), translation, HkReferencePolicy.TakeOwnership));
      this.ShapeInfos.Add(new MyCubeBlockCollector.ShapeInfo()
      {
        Count = 1,
        Min = minPos,
        Max = maxPos
      });
    }

    private void CollectBlock(
      MySlimBlock block,
      MyPhysicsOption physicsOption,
      IDictionary<Vector3I, HkMassElement> massResults,
      bool allowSegmentation = true)
    {
      if (!block.BlockDefinition.HasPhysics || block.CubeGrid == null)
        return;
      if (massResults != null)
        this.AddMass(block, massResults);
      if (block.BlockDefinition.BlockTopology == MyBlockTopology.Cube)
      {
        this.AddShapesCube(block, physicsOption);
      }
      else
      {
        if (physicsOption == MyPhysicsOption.None)
          return;
        HkShape[] shapes = (HkShape[]) null;
        if (block.FatBlock != null)
          shapes = block.FatBlock.ModelCollision.HavokCollisionShapes;
        if (shapes != null && shapes.Length != 0 && !MyFakes.ENABLE_SIMPLE_GRID_PHYSICS)
          this.AddShapesCustom(block, shapes);
        else
          this.AddShapesBox(block, allowSegmentation);
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void AddShapesCustom(MySlimBlock block, HkShape[] shapes)
    {
      Vector3 translation = !block.FatBlock.ModelCollision.ExportedWrong ? block.FatBlock.PositionComp.LocalMatrixRef.Translation : block.Position * block.CubeGrid.GridSize;
      Quaternion result;
      block.Orientation.GetQuaternion(out result);
      Vector3 scale = Vector3.One * block.FatBlock.ModelCollision.ScaleFactor;
      if (shapes.Length == 1 && shapes[0].ShapeType == HkShapeType.List)
      {
        HkListShape shape = (HkListShape) shapes[0];
        for (int index = 0; index < shape.TotalChildrenCount; ++index)
          this.Shapes.Add((HkShape) new HkConvexTransformShape((HkConvexShape) shape.GetChildByIndex(index), ref translation, ref result, ref scale, HkReferencePolicy.None));
      }
      else if (shapes.Length == 1 && shapes[0].ShapeType == HkShapeType.Mopp)
      {
        HkMoppBvTreeShape shape = (HkMoppBvTreeShape) shapes[0];
        int num1 = 0;
        while (true)
        {
          int num2 = num1;
          HkShapeCollection shapeCollection = shape.ShapeCollection;
          int shapeCount = shapeCollection.ShapeCount;
          if (num2 < shapeCount)
          {
            shapeCollection = shape.ShapeCollection;
            this.Shapes.Add((HkShape) new HkConvexTransformShape((HkConvexShape) shapeCollection.GetShape((uint) num1, (HkShapeBuffer) null), ref translation, ref result, ref scale, HkReferencePolicy.None));
            ++num1;
          }
          else
            break;
        }
      }
      else
      {
        for (int index = 0; index < shapes.Length; ++index)
          this.Shapes.Add((HkShape) new HkConvexTransformShape((HkConvexShape) shapes[index], ref translation, ref result, ref scale, HkReferencePolicy.None));
      }
      this.ShapeInfos.Add(new MyCubeBlockCollector.ShapeInfo()
      {
        Count = shapes.Length,
        Min = block.Min,
        Max = block.Max
      });
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void AddShapesBox(MySlimBlock block, bool allowSegmentation)
    {
      for (int x = block.Min.X; x <= block.Max.X; ++x)
      {
        for (int y = block.Min.Y; y <= block.Max.Y; ++y)
        {
          for (int z = block.Min.Z; z <= block.Max.Z; ++z)
          {
            Vector3I vector3I = new Vector3I(x, y, z);
            if (allowSegmentation)
            {
              this.m_tmpCubes.Add(vector3I);
            }
            else
            {
              Vector3 min = vector3I * block.CubeGrid.GridSize - new Vector3(block.CubeGrid.GridSize / 2f);
              Vector3 max = vector3I * block.CubeGrid.GridSize + new Vector3(block.CubeGrid.GridSize / 2f);
              this.AddBox(vector3I, vector3I, ref min, ref max);
            }
          }
        }
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void AddShapesCube(MySlimBlock block, MyPhysicsOption physicsOption)
    {
      MyCubeTopology myCubeTopology = block.BlockDefinition.CubeDefinition != null ? block.BlockDefinition.CubeDefinition.CubeTopology : MyCubeTopology.Box;
      if (MyFakes.ENABLE_SIMPLE_GRID_PHYSICS)
        physicsOption = MyPhysicsOption.Box;
      else if (myCubeTopology == MyCubeTopology.Box)
      {
        if (!block.ShowParts)
          physicsOption = MyPhysicsOption.Box;
        else if (block.BlockDefinition.CubeDefinition != null && block.CubeGrid.Skeleton.IsDeformed(block.Min, 0.05f, block.CubeGrid, false))
          physicsOption = MyPhysicsOption.Convex;
      }
      if (physicsOption != MyPhysicsOption.Box)
      {
        if (physicsOption != MyPhysicsOption.Convex)
          return;
        this.AddConvexShape(block, block.ShowParts);
      }
      else
        this.AddBoxes(block);
    }

    private void AddMass(MySlimBlock block, IDictionary<Vector3I, HkMassElement> massResults)
    {
      float mass = block.BlockDefinition.Mass;
      if (MyFakes.ENABLE_COMPOUND_BLOCKS && block.FatBlock is MyCompoundCubeBlock)
      {
        mass = 0.0f;
        foreach (MySlimBlock block1 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
          mass += block1.GetMass();
      }
      Vector3 vector3 = (block.Max - block.Min + Vector3I.One) * block.CubeGrid.GridSize;
      Vector3 position = (block.Min + block.Max) * 0.5f * block.CubeGrid.GridSize;
      HkMassProperties hkMassProperties = new HkMassProperties();
      HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(vector3 / 2f, mass);
      massResults[block.Position] = new HkMassElement()
      {
        Properties = volumeMassProperties,
        Tranform = Matrix.CreateTranslation(position)
      };
    }

    private void AddConvexShape(MySlimBlock block, bool applySkeleton)
    {
      this.m_tmpHelperVerts.Clear();
      Vector3 vector3_1 = block.Min * block.CubeGrid.GridSize;
      Vector3I vector3I = block.Min * 2 + 1;
      MyGridSkeleton skeleton = block.CubeGrid.Skeleton;
      foreach (Vector3 blockVertex in MyBlockVerticesCache.GetBlockVertices(block.BlockDefinition.CubeDefinition.CubeTopology, block.Orientation))
      {
        Vector3I pos = vector3I + Vector3I.Round(blockVertex);
        Vector3 vector3_2 = blockVertex * block.CubeGrid.GridSizeHalf;
        Vector3 bone;
        if (applySkeleton && skeleton.TryGetBone(ref pos, out bone))
          vector3_2.Add(bone);
        this.m_tmpHelperVerts.Add(vector3_2 + vector3_1);
      }
      if (block.BlockDefinition.CubeDefinition.CubeTopology == MyCubeTopology.CornerSquareInverted)
      {
        MyList<Vector3> range1 = this.m_tmpHelperVerts.GetRange(0, this.m_tmpHelperVerts.Count / 2);
        MyList<Vector3> range2 = this.m_tmpHelperVerts.GetRange(this.m_tmpHelperVerts.Count / 2, this.m_tmpHelperVerts.Count / 2);
        this.Shapes.Add((HkShape) new HkConvexVerticesShape(range1.GetInternalArray(), range1.Count));
        this.Shapes.Add((HkShape) new HkConvexVerticesShape(range2.GetInternalArray(), range2.Count));
        this.ShapeInfos.Add(new MyCubeBlockCollector.ShapeInfo()
        {
          Count = 2,
          Min = block.Min,
          Max = block.Max
        });
      }
      else
      {
        this.Shapes.Add((HkShape) new HkConvexVerticesShape(this.m_tmpHelperVerts.GetInternalArray(), this.m_tmpHelperVerts.Count, true, MyPerGameSettings.PhysicsConvexRadius));
        this.ShapeInfos.Add(new MyCubeBlockCollector.ShapeInfo()
        {
          Count = 1,
          Min = block.Min,
          Max = block.Max
        });
      }
    }

    private void AddBoxes(MySlimBlock block)
    {
      for (int x = block.Min.X; x <= block.Max.X; ++x)
      {
        for (int y = block.Min.Y; y <= block.Max.Y; ++y)
        {
          for (int z = block.Min.Z; z <= block.Max.Z; ++z)
            this.m_tmpCubes.Add(new Vector3I(x, y, z));
        }
      }
    }

    public struct ShapeInfo
    {
      public int Count;
      public Vector3I Min;
      public Vector3I Max;
    }
  }
}
