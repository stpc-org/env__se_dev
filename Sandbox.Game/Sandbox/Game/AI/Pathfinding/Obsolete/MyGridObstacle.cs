// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyGridObstacle
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyGridObstacle : IMyObstacle
  {
    private List<BoundingBox> m_segments;
    private static readonly MyVoxelSegmentation m_segmentation = new MyVoxelSegmentation();
    private MatrixD m_worldInv;
    private readonly MyCubeGrid m_grid;

    public MyGridObstacle(MyCubeGrid grid)
    {
      this.m_grid = grid;
      this.Segment();
      this.Update();
    }

    private void Segment()
    {
      MyGridObstacle.m_segmentation.ClearInput();
      foreach (MySlimBlock cubeBlock in this.m_grid.CubeBlocks)
      {
        Vector3I min = cubeBlock.Min;
        Vector3I max = cubeBlock.Max;
        Vector3I next = min;
        Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref min, ref max);
        while (vector3IRangeIterator.IsValid())
        {
          MyGridObstacle.m_segmentation.AddInput(next);
          vector3IRangeIterator.GetNext(out next);
        }
      }
      List<MyVoxelSegmentation.Segment> segments = MyGridObstacle.m_segmentation.FindSegments(MyVoxelSegmentationType.Simple2);
      this.m_segments = new List<BoundingBox>(segments.Count);
      for (int index = 0; index < segments.Count; ++index)
        this.m_segments.Add(new BoundingBox()
        {
          Min = (new Vector3(segments[index].Min) - Vector3.Half) * this.m_grid.GridSize - Vector3.Half,
          Max = (new Vector3(segments[index].Max) + Vector3.Half) * this.m_grid.GridSize + Vector3.Half
        });
      MyGridObstacle.m_segmentation.ClearInput();
    }

    public bool Contains(ref Vector3D point)
    {
      Vector3D result;
      Vector3D.Transform(ref point, ref this.m_worldInv, out result);
      Vector3 vector3_1 = Vector3.TransformNormal(MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.m_grid.PositionComp.WorldAABB.Center), this.m_worldInv);
      if (!Vector3.IsZero(vector3_1))
      {
        Vector3 vector3_2 = Vector3.Normalize(vector3_1);
        Ray ray = new Ray((Vector3) result, -vector3_2 * 2f);
        foreach (BoundingBox segment in this.m_segments)
        {
          if (segment.Intersects(ray).HasValue)
            return true;
        }
      }
      else
      {
        foreach (BoundingBox segment in this.m_segments)
        {
          if (segment.Contains(result) == ContainmentType.Contains)
            return true;
        }
      }
      return false;
    }

    public void Update()
    {
      this.Segment();
      this.m_worldInv = this.m_grid.PositionComp.WorldMatrixNormalizedInv;
    }

    public void DebugDraw()
    {
      MatrixD matrix1 = MatrixD.Invert(this.m_worldInv);
      MatrixD matrix2 = matrix1.GetOrientation();
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix2);
      foreach (BoundingBox segment in this.m_segments)
      {
        Vector3D halfExtents = new Vector3D(segment.Size) * 0.51;
        MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(Vector3D.Transform(new Vector3D(segment.Min + segment.Max) * 0.5, matrix1), halfExtents, fromRotationMatrix), Color.Red, 0.5f, false, false);
      }
    }
  }
}
