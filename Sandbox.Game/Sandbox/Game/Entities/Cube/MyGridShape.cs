// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyGridShape
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Models;
using VRage.Groups;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
  public class MyGridShape : IDisposable
  {
    public const int MAX_SHAPE_COUNT = 64879;
    private MyVoxelSegmentation m_segmenter;
    private MyCubeBlockCollector m_blockCollector = new MyCubeBlockCollector();
    private HkMassProperties m_massProperties;
    private HkMassProperties m_originalMassProperties;
    private bool m_originalMassPropertiesSet;
    private bool m_shapeUpdateInProgress;
    private Action m_shapeUpdateCallback;
    private HashSet<MySlimBlock> m_tmpRemovedBlocks = new HashSet<MySlimBlock>();
    private HashSet<Vector3I> m_tmpRemovedCubes = new HashSet<Vector3I>();
    private HashSet<Vector3I> m_tmpAdditionalCubes = new HashSet<Vector3I>();
    private MyCubeGrid m_grid;
    private HkGridShape m_root;
    private Dictionary<Vector3I, HkdShapeInstanceInfo> m_blocksShapes = new Dictionary<Vector3I, HkdShapeInstanceInfo>();
    private const int MassCellSize = 4;
    private MyGridMassComputer m_massElements;
    public static uint INVALID_COMPOUND_ID = uint.MaxValue;
    [ThreadStatic]
    private static List<Vector3S> m_removalMins;
    [ThreadStatic]
    private static List<Vector3S> m_removalMaxes;
    [ThreadStatic]
    private static List<bool> m_removalResults;
    private HashSet<Vector3I> m_updateConnections = new HashSet<Vector3I>();
    private List<HkBodyCollision> m_penetrations = new List<HkBodyCollision>();
    private List<MyVoxelBase> m_overlappingVoxels = new List<MyVoxelBase>();
    public HashSet<Vector3I> BlocksConnectedToWorld = new HashSet<Vector3I>();
    private List<HkdShapeInstanceInfo> m_shapeInfosList = new List<HkdShapeInstanceInfo>();
    private List<HkdShapeInstanceInfo> m_shapeInfosList2 = new List<HkdShapeInstanceInfo>();
    private List<HkdConnection> m_connectionsToAddCache = new List<HkdConnection>();
    private List<HkShape> m_khpShapeList = new List<HkShape>();
    private static List<HkdShapeInstanceInfo> m_tmpChildren = new List<HkdShapeInstanceInfo>();
    private HashSet<MySlimBlock> m_processedBlock = new HashSet<MySlimBlock>();
    private static List<HkdShapeInstanceInfo> m_shapeInfosList3 = new List<HkdShapeInstanceInfo>();
    private Dictionary<Vector3I, List<HkdConnection>> m_connections = new Dictionary<Vector3I, List<HkdConnection>>();
    private static object m_sharedParentLock = new object();
    private bool m_isSharedTensorDirty;

    public float BreakImpulse => this.m_grid == null || this.m_grid.Physics == null || this.m_grid.Physics.IsStatic ? 1E+07f : Math.Max(this.m_grid.Physics.Mass * MyFakes.DEFORMATION_MINIMUM_VELOCITY, MyFakes.DEFORMATION_MIN_BREAK_IMPULSE);

    public HkdBreakableShape BreakableShape { get; set; }

    public HkMassProperties? MassProperties => !this.m_grid.IsStatic ? new HkMassProperties?(this.m_massProperties) : new HkMassProperties?();

    public HkMassProperties? BaseMassProperties => !this.m_grid.IsStatic && this.m_originalMassPropertiesSet ? new HkMassProperties?(this.m_originalMassProperties) : new HkMassProperties?();

    public int ShapeCount => this.m_root.ShapeCount;

    public MyGridShape(MyCubeGrid grid)
    {
      this.m_grid = grid;
      this.m_shapeUpdateCallback = new Action(this.UpdateDirtyBlocksST);
      if (MyPerGameSettings.Destruction)
        return;
      if (MyPerGameSettings.UseGridSegmenter)
        this.m_segmenter = new MyVoxelSegmentation();
      this.m_massElements = new MyGridMassComputer(4);
      try
      {
        this.m_blockCollector.Collect(grid, this.m_segmenter, MyVoxelSegmentationType.Simple, (IDictionary<Vector3I, HkMassElement>) this.m_massElements);
        this.m_root = new HkGridShape(this.m_grid.GridSize, HkReferencePolicy.None);
        this.AddShapesFromCollector();
        if (this.m_grid.IsStatic)
          return;
        this.UpdateMassProperties();
      }
      finally
      {
        this.m_blockCollector.Clear();
      }
    }

    public void GetShapesInInterval(Vector3I min, Vector3I max, List<HkShape> shapeList) => this.m_root.GetShapesInInterval((Vector3) min, (Vector3) max, shapeList);

    public List<HkShape> GetShapesFromPosition(Vector3I pos)
    {
      List<HkShape> resultList = new List<HkShape>();
      this.m_root.GetShape(pos, resultList);
      return resultList;
    }

    private unsafe void AddShapesFromCollector()
    {
      int num = 0;
      for (int index1 = 0; index1 < this.m_blockCollector.ShapeInfos.Count; ++index1)
      {
        MyCubeBlockCollector.ShapeInfo shapeInfo = this.m_blockCollector.ShapeInfos[index1];
        HkShape[] hkShapeArray = (HkShape[]) null;
        Span<HkShape> span = new Span<HkShape>();
        Span<HkShape> shapes;
        if (shapeInfo.Count < 256)
        {
          int count = shapeInfo.Count;
          // ISSUE: untyped stack allocation
          shapes = new Span<HkShape>((void*) __untypedstackalloc(checked (unchecked ((IntPtr) (uint) count) * sizeof (HkShape))), count);
        }
        else
          shapes = (Span<HkShape>) (hkShapeArray = new HkShape[shapeInfo.Count]);
        for (int index2 = 0; index2 < shapeInfo.Count; ++index2)
          shapes[index2] = this.m_blockCollector.Shapes[num + index2];
        num += shapeInfo.Count;
        if (this.m_root.ShapeCount + shapeInfo.Count > 64879)
          MyHud.Notifications.Add(MyNotificationSingletons.GridReachedPhysicalLimit);
        if (this.m_root.ShapeCount + shapeInfo.Count < 65536)
          this.m_root.AddShapes(shapes, new Vector3S(shapeInfo.Min), new Vector3S(shapeInfo.Max));
        GC.KeepAlive((object) hkShapeArray);
      }
    }

    private void UpdateMassProperties()
    {
      this.m_massProperties = this.m_massElements.UpdateMass();
      if (this.m_originalMassPropertiesSet)
        return;
      this.m_originalMassProperties = this.m_massProperties;
      this.m_originalMassPropertiesSet = true;
    }

    public void Dispose()
    {
      foreach (List<HkdConnection> hkdConnectionList in this.m_connections.Values)
      {
        foreach (HkReferenceObject hkReferenceObject in hkdConnectionList)
          hkReferenceObject.RemoveReference();
      }
      this.m_connections.Clear();
      HkdBreakableShape breakableShape = this.BreakableShape;
      if (((object) breakableShape != null ? (breakableShape.IsValid() ? 1 : 0) : 0) != 0)
      {
        this.BreakableShape.RemoveReference();
        this.BreakableShape.ClearHandle();
      }
      foreach (HkdShapeInstanceInfo shapeInstanceInfo in this.m_blocksShapes.Values)
      {
        if (!shapeInstanceInfo.IsReferenceValid())
          MyLog.Default.WriteLine("Block shape was disposed already in MyGridShape.Dispose!");
        if (shapeInstanceInfo.Shape.IsValid())
          shapeInstanceInfo.Shape.RemoveReference();
        shapeInstanceInfo.RemoveReference();
      }
      this.m_blocksShapes.Clear();
      if (MyPerGameSettings.Destruction)
        return;
      this.m_root.Base.RemoveReference();
    }

    public void UnmarkBreakable(HkRigidBody rigidBody)
    {
      if (this.m_grid.GetPhysicsBody().HavokWorld == null || !this.m_grid.BlocksDestructionEnabled)
        return;
      this.UnmarkBreakable(this.m_grid.GetPhysicsBody().HavokWorld, rigidBody);
    }

    public void MarkBreakable(HkRigidBody rigidBody)
    {
      if (this.m_grid.GetPhysicsBody().HavokWorld == null || !this.m_grid.BlocksDestructionEnabled)
        return;
      this.MarkBreakable(this.m_grid.GetPhysicsBody().HavokWorld, rigidBody);
    }

    public void RefreshBlocks(HkRigidBody rigidBody, HashSet<Vector3I> dirtyBlocks)
    {
      this.m_originalMassPropertiesSet = false;
      this.UpdateDirtyBlocks(dirtyBlocks);
      if (rigidBody.GetMotionType() == HkMotionType.Keyframed || MyPerGameSettings.Destruction)
        return;
      this.UpdateMassProperties();
    }

    [Conditional("DEBUG")]
    private void CheckShapePositions(List<MyCubeBlockCollector.ShapeInfo> infos)
    {
      foreach (MyCubeBlockCollector.ShapeInfo info in infos)
      {
        Vector3I vector3I;
        for (vector3I.X = info.Min.X; vector3I.X <= info.Max.X; ++vector3I.X)
        {
          for (vector3I.Y = info.Min.Y; vector3I.Y <= info.Max.Y; ++vector3I.Y)
          {
            vector3I.Z = info.Min.Z;
            while (vector3I.Z <= info.Max.Z)
              ++vector3I.Z;
          }
        }
      }
    }

    private static void ExpandBlock(
      Vector3I cubePos,
      MyCubeGrid grid,
      HashSet<MySlimBlock> existingBlocks,
      HashSet<Vector3I> checkList,
      HashSet<Vector3I> expandResult)
    {
      MySlimBlock cubeBlock = grid.GetCubeBlock(cubePos);
      if (cubeBlock == null || !existingBlocks.Add(cubeBlock))
        return;
      Vector3I vector3I;
      for (vector3I.X = cubeBlock.Min.X; vector3I.X <= cubeBlock.Max.X; ++vector3I.X)
      {
        for (vector3I.Y = cubeBlock.Min.Y; vector3I.Y <= cubeBlock.Max.Y; ++vector3I.Y)
        {
          for (vector3I.Z = cubeBlock.Min.Z; vector3I.Z <= cubeBlock.Max.Z; ++vector3I.Z)
          {
            if (!checkList.Contains(vector3I))
              expandResult.Add(vector3I);
          }
        }
      }
    }

    private static void ExpandBlock(
      Vector3I cubePos,
      MyCubeGrid grid,
      HashSet<MySlimBlock> existingBlocks,
      HashSet<Vector3I> expandResult)
    {
      MySlimBlock cubeBlock = grid.GetCubeBlock(cubePos);
      if (cubeBlock == null || !existingBlocks.Add(cubeBlock))
        return;
      Vector3I vector3I;
      for (vector3I.X = cubeBlock.Min.X; vector3I.X <= cubeBlock.Max.X; ++vector3I.X)
      {
        for (vector3I.Y = cubeBlock.Min.Y; vector3I.Y <= cubeBlock.Max.Y; ++vector3I.Y)
        {
          for (vector3I.Z = cubeBlock.Min.Z; vector3I.Z <= cubeBlock.Max.Z; ++vector3I.Z)
            expandResult.Add(vector3I);
        }
      }
    }

    internal void UpdateDirtyBlocks(HashSet<Vector3I> dirtyCubes, bool recreateShape = true)
    {
      if (dirtyCubes.Count <= 0)
        return;
      if (MyPerGameSettings.Destruction && this.BreakableShape.IsValid())
      {
        int num = 0;
        HashSet<MySlimBlock> blocks = new HashSet<MySlimBlock>();
        foreach (Vector3I dirtyCube in dirtyCubes)
        {
          this.UpdateConnections(dirtyCube);
          this.BlocksConnectedToWorld.Remove(dirtyCube);
          if (this.m_blocksShapes.ContainsKey(dirtyCube))
          {
            HkdShapeInstanceInfo blocksShape = this.m_blocksShapes[dirtyCube];
            blocksShape.Shape.RemoveReference();
            blocksShape.RemoveReference();
            this.m_blocksShapes.Remove(dirtyCube);
          }
          MySlimBlock cubeBlock = this.m_grid.GetCubeBlock(dirtyCube);
          if (cubeBlock != null && !blocks.Contains(cubeBlock))
          {
            if (cubeBlock.Position != dirtyCube && this.m_blocksShapes.ContainsKey(cubeBlock.Position))
            {
              HkdShapeInstanceInfo blocksShape = this.m_blocksShapes[cubeBlock.Position];
              blocksShape.Shape.RemoveReference();
              blocksShape.RemoveReference();
              this.m_blocksShapes.Remove(cubeBlock.Position);
            }
            blocks.Add(cubeBlock);
            ++num;
          }
        }
        foreach (MySlimBlock b in blocks)
        {
          Matrix blockTransform;
          HkdBreakableShape blockShape = this.CreateBlockShape(b, out blockTransform);
          if (blockShape != (HkdBreakableShape) null)
            this.m_blocksShapes[b.Position] = new HkdShapeInstanceInfo(blockShape, blockTransform);
        }
        foreach (HkdShapeInstanceInfo shapeInstanceInfo in this.m_blocksShapes.Values)
          this.m_shapeInfosList.Add(shapeInstanceInfo);
        if (blocks.Count > 0)
          this.FindConnectionsToWorld(blocks);
        if (recreateShape)
        {
          this.BreakableShape.RemoveReference();
          this.BreakableShape = (HkdBreakableShape) new HkdCompoundBreakableShape((HkdBreakableShape) null, this.m_shapeInfosList);
          this.BreakableShape.SetChildrenParent(this.BreakableShape);
          this.BreakableShape.BuildMassProperties(ref this.m_massProperties);
          this.BreakableShape.SetStrenghtRecursively(MyDestructionConstants.STRENGTH, 0.7f);
        }
        this.UpdateConnectionsManually(this.BreakableShape, this.m_updateConnections);
        this.m_updateConnections.Clear();
        this.AddConnections();
        this.m_shapeInfosList.Clear();
      }
      else
      {
        try
        {
          if (MyGridShape.m_removalMins == null)
            MyGridShape.m_removalMins = new List<Vector3S>();
          if (MyGridShape.m_removalMaxes == null)
            MyGridShape.m_removalMaxes = new List<Vector3S>();
          foreach (Vector3I dirtyCube in dirtyCubes)
          {
            if (this.m_tmpRemovedCubes.Add(dirtyCube))
              MyGridShape.ExpandBlock(dirtyCube, this.m_grid, this.m_tmpRemovedBlocks, this.m_tmpRemovedCubes);
          }
          MyGridShape.m_removalMins.Clear();
          MyGridShape.m_removalMaxes.Clear();
          this.m_root.CollectCellRanges(this.m_tmpRemovedCubes, MyGridShape.m_removalMins, MyGridShape.m_removalMaxes);
          Vector3I cubePos;
          for (int index = 0; index < MyGridShape.m_removalMins.Count; ++index)
          {
            for (cubePos.X = (int) MyGridShape.m_removalMins[index].X; cubePos.X <= (int) MyGridShape.m_removalMaxes[index].X; ++cubePos.X)
            {
              for (cubePos.Y = (int) MyGridShape.m_removalMins[index].Y; cubePos.Y <= (int) MyGridShape.m_removalMaxes[index].Y; ++cubePos.Y)
              {
                for (cubePos.Z = (int) MyGridShape.m_removalMins[index].Z; cubePos.Z <= (int) MyGridShape.m_removalMaxes[index].Z; ++cubePos.Z)
                {
                  if (this.m_tmpRemovedCubes.Add(cubePos))
                    MyGridShape.ExpandBlock(cubePos, this.m_grid, this.m_tmpRemovedBlocks, this.m_tmpRemovedCubes, this.m_tmpAdditionalCubes);
                }
              }
            }
          }
          while (this.m_tmpAdditionalCubes.Count > 0)
          {
            MyGridShape.m_removalMins.Clear();
            MyGridShape.m_removalMaxes.Clear();
            this.m_root.CollectCellRanges(this.m_tmpAdditionalCubes, MyGridShape.m_removalMins, MyGridShape.m_removalMaxes);
            this.m_tmpAdditionalCubes.Clear();
            for (int index = 0; index < MyGridShape.m_removalMins.Count; ++index)
            {
              for (cubePos.X = (int) MyGridShape.m_removalMins[index].X; cubePos.X <= (int) MyGridShape.m_removalMaxes[index].X; ++cubePos.X)
              {
                for (cubePos.Y = (int) MyGridShape.m_removalMins[index].Y; cubePos.Y <= (int) MyGridShape.m_removalMaxes[index].Y; ++cubePos.Y)
                {
                  for (cubePos.Z = (int) MyGridShape.m_removalMins[index].Z; cubePos.Z <= (int) MyGridShape.m_removalMaxes[index].Z; ++cubePos.Z)
                  {
                    if (this.m_tmpRemovedCubes.Add(cubePos))
                      MyGridShape.ExpandBlock(cubePos, this.m_grid, this.m_tmpRemovedBlocks, this.m_tmpRemovedCubes, this.m_tmpAdditionalCubes);
                  }
                }
              }
            }
          }
          this.m_blockCollector.CollectArea(this.m_grid, this.m_tmpRemovedCubes, this.m_segmenter, MyVoxelSegmentationType.Simple, (IDictionary<Vector3I, HkMassElement>) this.m_massElements);
          this.m_shapeUpdateInProgress = true;
          if (Sandbox.Game.Entities.MyEntities.IsAsyncUpdateInProgress)
            Sandbox.Game.Entities.MyEntities.InvokeLater(this.m_shapeUpdateCallback);
          else
            this.UpdateDirtyBlocksST();
        }
        finally
        {
          MyGridShape.m_removalMins.Clear();
          MyGridShape.m_removalMaxes.Clear();
          this.m_tmpRemovedBlocks.Clear();
          this.m_tmpAdditionalCubes.Clear();
        }
      }
    }

    private void UpdateDirtyBlocksST()
    {
      try
      {
        this.m_root.RemoveShapes(this.m_tmpRemovedCubes);
        this.AddShapesFromCollector();
      }
      finally
      {
        this.m_blockCollector.Clear();
        this.m_tmpRemovedCubes.Clear();
        this.m_shapeUpdateInProgress = false;
      }
    }

    private void UpdateConnections(Vector3I dirty)
    {
      foreach (Vector3I vector3I in new List<Vector3I>(7)
      {
        dirty,
        dirty + Vector3I.Up,
        dirty + Vector3I.Down,
        dirty + Vector3I.Left,
        dirty + Vector3I.Right,
        dirty + Vector3I.Forward,
        dirty + Vector3I.Backward
      })
      {
        if (this.m_connections.ContainsKey(vector3I))
        {
          foreach (HkReferenceObject hkReferenceObject in this.m_connections[vector3I])
            hkReferenceObject.RemoveReference();
          this.m_connections[vector3I].Clear();
        }
        MySlimBlock cubeBlock = this.m_grid.GetCubeBlock(vector3I);
        if (cubeBlock != null)
        {
          if (this.m_connections.ContainsKey(cubeBlock.Position))
          {
            foreach (HkReferenceObject hkReferenceObject in this.m_connections[cubeBlock.Position])
              hkReferenceObject.RemoveReference();
            this.m_connections[cubeBlock.Position].Clear();
          }
          this.m_updateConnections.Add(cubeBlock.Position);
        }
        this.m_updateConnections.Add(vector3I);
      }
    }

    public void UpdateShape(
      HkRigidBody rigidBody,
      HkRigidBody rigidBody2,
      HkdBreakableBody destructionBody)
    {
      if ((HkReferenceObject) destructionBody != (HkReferenceObject) null)
      {
        destructionBody.BreakableShape = this.BreakableShape;
        this.CreateConnectionToWorld(destructionBody, this.m_grid.Physics.HavokWorld);
      }
      else
      {
        int num1 = (int) rigidBody.SetShape((HkShape) this.m_root);
        if (!((HkReferenceObject) rigidBody2 != (HkReferenceObject) null))
          return;
        int num2 = (int) rigidBody2.SetShape((HkShape) this.m_root);
      }
    }

    private void FindConnectionsToWorld(HashSet<MySlimBlock> blocks)
    {
      if (!this.m_grid.IsStatic || this.m_grid.Physics != null && (double) this.m_grid.Physics.LinearVelocity.LengthSquared() > 0.0)
        return;
      int num = 0;
      Quaternion identity = Quaternion.Identity;
      MatrixD worldMatrix = this.m_grid.WorldMatrix;
      BoundingBoxD worldAabb = this.m_grid.PositionComp.WorldAABB;
      MyGamePruningStructure.GetAllVoxelMapsInBox(ref worldAabb, this.m_overlappingVoxels);
      foreach (MySlimBlock block in blocks)
      {
        BoundingBox geometryLocalBox = block.FatBlock.GetGeometryLocalBox();
        Vector3 halfExtents = geometryLocalBox.Size / 2f;
        Vector3D scaledCenter;
        block.ComputeScaledCenter(out scaledCenter);
        scaledCenter = Vector3D.Transform(scaledCenter + geometryLocalBox.Center, worldMatrix);
        Matrix result;
        block.Orientation.GetMatrix(out result);
        MatrixD matrix = result * worldMatrix.GetOrientation();
        Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
        MyPhysics.GetPenetrationsBox(ref halfExtents, ref scaledCenter, ref fromRotationMatrix, this.m_penetrations, 14);
        ++num;
        bool flag = false;
        foreach (HkBodyCollision penetration in this.m_penetrations)
        {
          IMyEntity collisionEntity = penetration.GetCollisionEntity();
          if (collisionEntity != null && collisionEntity is MyVoxelBase)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          BoundingBoxD aabb = (BoundingBoxD) geometryLocalBox + (Vector3D) block.Position;
          foreach (MyVoxelBase overlappingVoxel in this.m_overlappingVoxels)
          {
            if (overlappingVoxel.IsAnyAabbCornerInside(ref worldMatrix, aabb))
            {
              flag = true;
              break;
            }
          }
        }
        this.m_penetrations.Clear();
        if (flag && !this.BlocksConnectedToWorld.Contains(block.Position))
        {
          this.m_blocksShapes[block.Position].GetChildren(this.m_shapeInfosList2);
          for (int index = 0; index < this.m_shapeInfosList2.Count; ++index)
          {
            HkdShapeInstanceInfo shapeInstanceInfo = this.m_shapeInfosList2[index];
            if (shapeInstanceInfo.Shape.GetChildrenCount() > 0)
              shapeInstanceInfo.Shape.GetChildren(this.m_shapeInfosList2);
            else
              shapeInstanceInfo.Shape.SetFlagRecursively(HkdBreakableShape.Flags.IS_FIXED);
          }
          this.m_shapeInfosList2.Clear();
          this.BlocksConnectedToWorld.Add(block.Position);
        }
      }
      this.m_overlappingVoxels.Clear();
    }

    public void FindConnectionsToWorld() => this.FindConnectionsToWorld(this.m_grid.GetBlocks());

    public void RecalculateConnectionsToWorld(HashSet<MySlimBlock> blocks)
    {
      this.BlocksConnectedToWorld.Clear();
      this.FindConnectionsToWorld(blocks);
    }

    public void CreateConnectionToWorld(HkdBreakableBody destructionBody, HkWorld havokWorld)
    {
      if (this.BlocksConnectedToWorld.Count == 0)
        return;
      HkdFixedConnectivity connectivity = HkdFixedConnectivity.Create();
      foreach (Vector3I key in this.BlocksConnectedToWorld)
      {
        HkdFixedConnectivity.Connection c = new HkdFixedConnectivity.Connection(Vector3.Zero, Vector3.Up, 1f, this.m_blocksShapes[key].Shape, havokWorld.GetFixedBody(), 0);
        connectivity.AddConnection(ref c);
        c.RemoveReference();
      }
      destructionBody.SetFixedConnectivity(connectivity);
      connectivity.RemoveReference();
    }

    public HkdBreakableShape CreateBreakableShape()
    {
      this.m_blocksShapes.Clear();
      foreach (MySlimBlock block in this.m_grid.GetBlocks())
      {
        Matrix blockTransform;
        HkdBreakableShape blockShape = this.CreateBlockShape(block, out blockTransform);
        if (blockShape != (HkdBreakableShape) null)
        {
          HkdShapeInstanceInfo shapeInstanceInfo = new HkdShapeInstanceInfo(blockShape, blockTransform);
          this.m_shapeInfosList.Add(shapeInstanceInfo);
          this.m_blocksShapes[block.Position] = shapeInstanceInfo;
        }
      }
      if (this.m_blocksShapes.Count == 0)
        return (HkdBreakableShape) null;
      if (this.BreakableShape.IsValid())
        this.BreakableShape.RemoveReference();
      this.BreakableShape = (HkdBreakableShape) new HkdCompoundBreakableShape((HkdBreakableShape) null, this.m_shapeInfosList);
      this.BreakableShape.SetChildrenParent(this.BreakableShape);
      try
      {
        this.BreakableShape.SetStrenghtRecursively(MyDestructionConstants.STRENGTH, 0.7f);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(ex);
        MyLog.Default.WriteLine("BS Valid: " + this.BreakableShape.IsValid().ToString());
        MyLog.Default.WriteLine("BS Child count: " + (object) this.BreakableShape.GetChildrenCount());
        MyLog.Default.WriteLine("Grid shapes: " + (object) this.m_shapeInfosList.Count);
        foreach (HkdShapeInstanceInfo shapeInfos in this.m_shapeInfosList)
        {
          if (!shapeInfos.Shape.IsValid())
            MyLog.Default.WriteLine("Invalid child!");
          else
            MyLog.Default.WriteLine("Child strength: " + (object) shapeInfos.Shape.GetStrenght());
        }
        MyLog.Default.WriteLine("Grid Blocks count: " + (object) this.m_grid.GetBlocks().Count);
        MyLog.Default.WriteLine("Grid MarkedForClose: " + this.m_grid.MarkedForClose.ToString());
        HashSet<MyDefinitionId> myDefinitionIdSet = new HashSet<MyDefinitionId>();
        foreach (MySlimBlock block1 in this.m_grid.GetBlocks())
        {
          if (block1.FatBlock != null && block1.FatBlock.MarkedForClose)
            MyLog.Default.WriteLine("Block marked for close: " + (object) block1.BlockDefinition.Id);
          if (myDefinitionIdSet.Count < 50)
          {
            if (block1.FatBlock is MyCompoundCubeBlock)
            {
              foreach (MySlimBlock block2 in (block1.FatBlock as MyCompoundCubeBlock).GetBlocks())
              {
                myDefinitionIdSet.Add(block2.BlockDefinition.Id);
                if (block2.FatBlock != null && block2.FatBlock.MarkedForClose)
                  MyLog.Default.WriteLine("Block in compound marked for close: " + (object) block2.BlockDefinition.Id);
              }
            }
            else
              myDefinitionIdSet.Add(block1.BlockDefinition.Id);
          }
          else
            break;
        }
        foreach (MyDefinitionId myDefinitionId in myDefinitionIdSet)
          MyLog.Default.WriteLine("Block definition: " + (object) myDefinitionId);
        throw new InvalidOperationException();
      }
      this.CreateConnectionsManually(this.BreakableShape);
      this.m_shapeInfosList.Clear();
      return this.BreakableShape;
    }

    private static bool HasBreakableShape(string model, MyCubeBlockDefinition block)
    {
      MyModel modelOnlyData = MyModels.GetModelOnlyData(model);
      return modelOnlyData != null && modelOnlyData.HavokBreakableShapes != null && (uint) modelOnlyData.HavokBreakableShapes.Length > 0U;
    }

    private static HkdBreakableShape GetBreakableShape(
      string model,
      MyCubeBlockDefinition block,
      bool forceLoadDestruction = false)
    {
      if (MyFakes.LAZY_LOAD_DESTRUCTION | forceLoadDestruction)
      {
        MyModel modelOnlyData = MyModels.GetModelOnlyData(model);
        if (modelOnlyData.HavokBreakableShapes == null)
          MyDestructionData.Static.LoadModelDestruction(model, (MyPhysicalModelDefinition) block, modelOnlyData.BoundingBoxSize);
      }
      return MyDestructionData.Static.BlockShapePool.GetBreakableShape(model, block);
    }

    private HkdBreakableShape CreateBlockShape(
      MySlimBlock b,
      out Matrix blockTransform)
    {
      blockTransform = Matrix.Identity;
      if (b.FatBlock == null)
        return (HkdBreakableShape) null;
      HkdBreakableShape hkdBreakableShape1 = new HkdBreakableShape();
      Matrix result = Matrix.Identity;
      if (b.FatBlock is MyCompoundCubeBlock)
      {
        blockTransform.Translation = b.FatBlock.PositionComp.LocalMatrixRef.Translation;
        MyCompoundCubeBlock fatBlock = b.FatBlock as MyCompoundCubeBlock;
        if (fatBlock.GetBlocksCount() == 1)
        {
          MySlimBlock block = fatBlock.GetBlocks()[0];
          ushort? blockId = fatBlock.GetBlockId(block);
          MyFractureComponentBase fractureComponent = (MyFractureComponentBase) block.GetFractureComponent();
          if (fractureComponent != null)
          {
            hkdBreakableShape1 = fractureComponent.Shape;
            hkdBreakableShape1.AddReference();
          }
          else
          {
            MyCubeBlockDefinition blockDefinition = block.FatBlock.BlockDefinition;
            string currentModel = block.CalculateCurrentModel(out Matrix _);
            if (!MyFakes.LAZY_LOAD_DESTRUCTION && !MyGridShape.HasBreakableShape(currentModel, blockDefinition))
            {
              MySandboxGame.Log.WriteLine("Breakable shape not preallocated: " + currentModel + " definition: " + (object) blockDefinition);
              MyGridShape.GetBreakableShape(currentModel, blockDefinition, true);
            }
            if (MyFakes.LAZY_LOAD_DESTRUCTION || MyGridShape.HasBreakableShape(currentModel, blockDefinition))
              hkdBreakableShape1 = MyGridShape.GetBreakableShape(currentModel, blockDefinition);
          }
          if (hkdBreakableShape1.IsValid())
          {
            HkPropertyBase prop = (HkPropertyBase) new HkSimpleValueProperty((uint) blockId.Value);
            hkdBreakableShape1.SetPropertyRecursively(256, prop);
            prop.RemoveReference();
          }
          block.Orientation.GetMatrix(out result);
          blockTransform = result * blockTransform;
        }
        else
        {
          Vector3 vector3 = b.Position * this.m_grid.GridSize;
          float num = 0.0f;
          foreach (MySlimBlock block in fatBlock.GetBlocks())
          {
            block.Orientation.GetMatrix(out result);
            result.Translation = Vector3.Zero;
            ushort? blockId = fatBlock.GetBlockId(block);
            MyFractureComponentBase fractureComponent = (MyFractureComponentBase) block.GetFractureComponent();
            if (fractureComponent != null)
            {
              hkdBreakableShape1 = fractureComponent.Shape;
              hkdBreakableShape1.UserObject |= 1U;
              hkdBreakableShape1.AddReference();
              this.m_shapeInfosList2.Add(new HkdShapeInstanceInfo(hkdBreakableShape1, result));
            }
            else
            {
              MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
              string currentModel = block.CalculateCurrentModel(out Matrix _);
              if (!MyFakes.LAZY_LOAD_DESTRUCTION && !MyGridShape.HasBreakableShape(currentModel, blockDefinition))
              {
                MySandboxGame.Log.WriteLine("Breakable shape not preallocated: " + currentModel + " definition: " + (object) blockDefinition);
                MyGridShape.GetBreakableShape(currentModel, blockDefinition, true);
              }
              if (MyFakes.LAZY_LOAD_DESTRUCTION || MyGridShape.HasBreakableShape(currentModel, blockDefinition))
              {
                hkdBreakableShape1 = MyGridShape.GetBreakableShape(currentModel, blockDefinition);
                hkdBreakableShape1.UserObject |= 1U;
                num += blockDefinition.Mass;
                this.m_shapeInfosList2.Add(new HkdShapeInstanceInfo(hkdBreakableShape1, result));
              }
            }
            if (hkdBreakableShape1.IsValid())
            {
              HkPropertyBase prop = (HkPropertyBase) new HkSimpleValueProperty((uint) blockId.Value);
              hkdBreakableShape1.SetPropertyRecursively(256, prop);
              prop.RemoveReference();
            }
          }
          if (this.m_shapeInfosList2.Count == 0)
            return (HkdBreakableShape) null;
          HkdBreakableShape hkdBreakableShape2 = (HkdBreakableShape) new HkdCompoundBreakableShape((HkdBreakableShape) null, this.m_shapeInfosList2);
          ((HkdCompoundBreakableShape) hkdBreakableShape2).RecalcMassPropsFromChildren();
          HkMassProperties hkMassProperties = new HkMassProperties();
          hkdBreakableShape2.BuildMassProperties(ref hkMassProperties);
          hkdBreakableShape1 = new HkdBreakableShape(hkdBreakableShape2.GetShape(), ref hkMassProperties);
          hkdBreakableShape2.RemoveReference();
          foreach (HkdShapeInstanceInfo shapeInfo in this.m_shapeInfosList2)
            hkdBreakableShape1.AddShape(ref shapeInfo);
          for (int index1 = 0; index1 < this.m_shapeInfosList2.Count; ++index1)
          {
            for (int index2 = 0; index2 < this.m_shapeInfosList2.Count; ++index2)
            {
              if (index1 != index2)
                MyGridShape.ConnectShapesWithChildren(hkdBreakableShape1, this.m_shapeInfosList2[index1].Shape, this.m_shapeInfosList2[index2].Shape);
            }
          }
          foreach (HkdShapeInstanceInfo shapeInstanceInfo in this.m_shapeInfosList2)
          {
            shapeInstanceInfo.Shape.RemoveReference();
            shapeInstanceInfo.RemoveReference();
          }
          this.m_shapeInfosList2.Clear();
        }
      }
      else
      {
        b.Orientation.GetMatrix(out blockTransform);
        blockTransform.Translation = b.FatBlock.PositionComp.LocalMatrixRef.Translation;
        string currentModel = b.CalculateCurrentModel(out Matrix _);
        if (b.FatBlock is MyFracturedBlock)
        {
          hkdBreakableShape1 = (b.FatBlock as MyFracturedBlock).Shape;
          if (!hkdBreakableShape1.IsValid())
            throw new Exception("Fractured block Breakable shape invalid!");
          hkdBreakableShape1.AddReference();
        }
        else
        {
          MyFractureComponentBase fractureComponent = (MyFractureComponentBase) b.GetFractureComponent();
          if (fractureComponent != null)
          {
            hkdBreakableShape1 = fractureComponent.Shape;
            hkdBreakableShape1.AddReference();
          }
          else
          {
            if (!MyFakes.LAZY_LOAD_DESTRUCTION && !MyGridShape.HasBreakableShape(currentModel, b.BlockDefinition))
            {
              MySandboxGame.Log.WriteLine("Breakable shape not preallocated: " + currentModel + " definition: " + (object) b.BlockDefinition);
              MyGridShape.GetBreakableShape(currentModel, b.BlockDefinition, true);
            }
            if (MyFakes.LAZY_LOAD_DESTRUCTION || MyGridShape.HasBreakableShape(currentModel, b.BlockDefinition))
              hkdBreakableShape1 = MyGridShape.GetBreakableShape(currentModel, b.BlockDefinition);
          }
        }
      }
      HkPropertyBase prop1 = (HkPropertyBase) new HkVec3IProperty(b.Position);
      if (!hkdBreakableShape1.IsValid())
      {
        MySandboxGame.Log.WriteLine("BreakableShape not valid: " + (object) b.BlockDefinition.Id + " pos: " + (object) b.Min + " grid cubes: " + (object) b.CubeGrid.BlocksCount);
        if (b.FatBlock is MyCompoundCubeBlock)
        {
          MyCompoundCubeBlock fatBlock = b.FatBlock as MyCompoundCubeBlock;
          MySandboxGame.Log.WriteLine("Compound blocks count: " + (object) fatBlock.GetBlocksCount());
          foreach (MySlimBlock block in fatBlock.GetBlocks())
            MySandboxGame.Log.WriteLine("Block in compound: " + (object) block.BlockDefinition.Id);
        }
      }
      hkdBreakableShape1.SetPropertyRecursively((int) byte.MaxValue, prop1);
      prop1.RemoveReference();
      return hkdBreakableShape1;
    }

    private void UpdateConnectionsManually(HkdBreakableShape shape, HashSet<Vector3I> dirtyCubes)
    {
      uint num = 0;
      foreach (Vector3I dirtyCube in dirtyCubes)
      {
        MySlimBlock cubeBlock = this.m_grid.GetCubeBlock(dirtyCube);
        if (cubeBlock != null && !this.m_processedBlock.Contains(cubeBlock))
        {
          if (!this.m_connections.ContainsKey(cubeBlock.Position))
            this.m_connections[cubeBlock.Position] = new List<HkdConnection>();
          List<HkdConnection> connection = this.m_connections[cubeBlock.Position];
          foreach (MySlimBlock neighbour in cubeBlock.Neighbours)
          {
            this.ConnectBlocks(shape, cubeBlock, neighbour, connection);
            ++num;
          }
          this.m_processedBlock.Add(cubeBlock);
        }
      }
      this.m_processedBlock.Clear();
    }

    public void CreateConnectionsManually(HkdBreakableShape shape)
    {
      this.m_connections.Clear();
      foreach (MySlimBlock cubeBlock in this.m_grid.CubeBlocks)
      {
        if (this.m_blocksShapes.ContainsKey(cubeBlock.Position))
        {
          if (!this.m_connections.ContainsKey(cubeBlock.Position))
            this.m_connections[cubeBlock.Position] = new List<HkdConnection>();
          List<HkdConnection> connection = this.m_connections[cubeBlock.Position];
          foreach (MySlimBlock neighbour in cubeBlock.Neighbours)
          {
            if (this.m_blocksShapes.ContainsKey(neighbour.Position))
              this.ConnectBlocks(shape, cubeBlock, neighbour, connection);
          }
        }
      }
      this.AddConnections();
    }

    private void AddConnections()
    {
      int count = 0;
      foreach (List<HkdConnection> hkdConnectionList in this.m_connections.Values)
        count += hkdConnectionList.Count;
      this.BreakableShape.ClearConnections();
      this.BreakableShape.ReplaceConnections(this.m_connections, count);
    }

    private bool CheckConnection(HkdConnection c)
    {
      HkdBreakableShape hkdBreakableShape1 = c.ShapeA;
      while (hkdBreakableShape1.HasParent)
        hkdBreakableShape1 = hkdBreakableShape1.GetParent();
      if (hkdBreakableShape1 != this.BreakableShape)
        return false;
      HkdBreakableShape hkdBreakableShape2 = c.ShapeB;
      while (hkdBreakableShape2.HasParent)
        hkdBreakableShape2 = hkdBreakableShape2.GetParent();
      return !(hkdBreakableShape2 != this.BreakableShape);
    }

    private void ConnectBlocks(
      HkdBreakableShape parent,
      MySlimBlock blockA,
      MySlimBlock blockB,
      List<HkdConnection> blockConnections)
    {
      if (!this.m_blocksShapes.ContainsKey(blockA.Position) || !this.m_blocksShapes.ContainsKey(blockB.Position))
        return;
      HkdShapeInstanceInfo blocksShape1 = this.m_blocksShapes[blockA.Position];
      HkdShapeInstanceInfo blocksShape2 = this.m_blocksShapes[blockB.Position];
      blocksShape2.GetChildren(this.m_shapeInfosList2);
      bool flag = blocksShape2.Shape.GetChildrenCount() == 0;
      foreach (HkdShapeInstanceInfo shapeInstanceInfo in this.m_shapeInfosList2)
        shapeInstanceInfo.DynamicParent = HkdShapeInstanceInfo.INVALID_INDEX;
      Vector3 vector3_1 = blockB.Position * this.m_grid.GridSize;
      Vector3 pivotA = blockA.Position * this.m_grid.GridSize;
      Vector3 vector3_2 = Vector3.Normalize((Vector3) (blockB.Position - blockA.Position));
      Matrix orientation = blocksShape2.GetTransform().GetOrientation();
      for (int index1 = 0; index1 < this.m_shapeInfosList2.Count; ++index1)
      {
        HkdShapeInstanceInfo shapeInstanceInfo1 = this.m_shapeInfosList2[index1];
        Matrix matrix1 = shapeInstanceInfo1.GetTransform();
        HkdShapeInstanceInfo shapeInstanceInfo2;
        for (ushort dynamicParent = shapeInstanceInfo1.DynamicParent; (int) dynamicParent != (int) HkdShapeInstanceInfo.INVALID_INDEX; dynamicParent = shapeInstanceInfo2.DynamicParent)
        {
          Matrix matrix2 = matrix1;
          shapeInstanceInfo2 = this.m_shapeInfosList2[(int) dynamicParent];
          Matrix transform = shapeInstanceInfo2.GetTransform();
          matrix1 = matrix2 * transform;
          shapeInstanceInfo2 = this.m_shapeInfosList2[(int) dynamicParent];
        }
        Matrix matrix3 = matrix1 * orientation;
        Vector4 min;
        Vector4 max;
        shapeInstanceInfo1.Shape.GetShape().GetLocalAABB(0.1f, out min, out max);
        Vector3 vector3_3 = vector3_1 + Vector3.Transform(new Vector3(min), matrix3);
        Vector3 vector3_4 = (pivotA - vector3_3) * vector3_2;
        if ((double) vector3_4.AbsMax() > 1.35000002384186)
        {
          Vector3 vector3_5 = vector3_1 + Vector3.Transform(new Vector3(max), matrix3);
          vector3_4 = (pivotA - vector3_5) * vector3_2;
          if ((double) vector3_4.AbsMax() > 1.35000002384186)
            continue;
        }
        flag = true;
        HkdConnection connection = MyGridShape.CreateConnection(blocksShape1.Shape, shapeInstanceInfo1.Shape, pivotA, vector3_1 + Vector3.Transform(shapeInstanceInfo1.CoM, matrix3));
        blockConnections.Add(connection);
        shapeInstanceInfo1.GetChildren(this.m_shapeInfosList2);
        for (int index2 = this.m_shapeInfosList2.Count - shapeInstanceInfo1.Shape.GetChildrenCount(); index2 < this.m_shapeInfosList2.Count; ++index2)
          this.m_shapeInfosList2[index2].DynamicParent = (ushort) index1;
      }
      if (flag)
      {
        HkdConnection connection = MyGridShape.CreateConnection(blocksShape1.Shape, blocksShape2.Shape, blockA.Position * this.m_grid.GridSize, blockB.Position * this.m_grid.GridSize);
        blockConnections.Add(connection);
      }
      this.m_shapeInfosList2.Clear();
    }

    public static void ConnectShapesWithChildren(
      HkdBreakableShape parent,
      HkdBreakableShape shapeA,
      HkdBreakableShape shapeB)
    {
      lock (MyGridShape.m_sharedParentLock)
      {
        HkdConnection connection1 = MyGridShape.CreateConnection(shapeA, shapeB, shapeA.CoM, shapeB.CoM);
        connection1.AddToCommonParent();
        connection1.RemoveReference();
        shapeB.GetChildren(MyGridShape.m_shapeInfosList3);
        foreach (HkdShapeInstanceInfo shapeInstanceInfo in MyGridShape.m_shapeInfosList3)
        {
          HkdConnection connection2 = MyGridShape.CreateConnection(shapeA, shapeInstanceInfo.Shape, shapeA.CoM, shapeB.CoM);
          connection2.AddToCommonParent();
          connection2.RemoveReference();
        }
        MyGridShape.m_shapeInfosList3.Clear();
      }
    }

    private static HkdConnection CreateConnection(
      HkdBreakableShape aShape,
      HkdBreakableShape bShape,
      Vector3 pivotA,
      Vector3 pivotB)
    {
      Vector3 normal = bShape.CoM - aShape.CoM;
      return new HkdConnection(aShape, bShape, pivotA, pivotB, normal, 6.25f);
    }

    private void UpdateMass(HkRigidBody rigidBody, bool setMass = true)
    {
      if (rigidBody.GetMotionType() == HkMotionType.Keyframed)
        return;
      if (!MyPerGameSettings.Destruction)
        this.UpdateMassProperties();
      if (!setMass)
        return;
      this.SetMass(rigidBody);
    }

    public void SetMass(HkRigidBody rigidBody)
    {
      if (this.m_grid.Physics.IsWelded || this.m_grid.GetPhysicsBody().WeldInfo.Children.Count != 0)
      {
        this.m_grid.GetPhysicsBody().WeldedRigidBody.SetMassProperties(ref this.m_massProperties);
        this.m_grid.GetPhysicsBody().WeldInfo.SetMassProps(this.m_massProperties);
        this.m_grid.Physics.UpdateMassProps();
      }
      else
      {
        rigidBody.Mass = this.m_massProperties.Mass;
        rigidBody.SetMassProperties(ref this.m_massProperties);
      }
      this.m_grid.NotifyMassPropertiesChanged();
      MySharedTensorsGroups.MarkGroupDirty(this.m_grid);
      MyGridPhysicalGroupData.InvalidateSharedMassPropertiesCache(this.m_grid);
    }

    public void MarkBreakable(HkWorld world, HkRigidBody rigidBody)
    {
      if (MyPerGameSettings.Destruction)
        return;
      world.BreakOffPartsUtil.MarkEntityBreakable((HkEntity) rigidBody, this.BreakImpulse);
    }

    public void UnmarkBreakable(HkWorld world, HkRigidBody rigidBody)
    {
      if (MyPerGameSettings.Destruction)
        return;
      world.BreakOffPartsUtil.UnmarkEntityBreakable((HkEntity) rigidBody);
    }

    public void RefreshMass()
    {
      this.m_blockCollector.CollectMassElements(this.m_grid, (IDictionary<Vector3I, HkMassElement>) this.m_massElements);
      this.UpdateMass(this.m_grid.Physics.RigidBody);
      this.m_grid.SetInventoryMassDirty();
    }

    public void UpdateMassFromInventories(HashSet<MyCubeBlock> blocks, MyPhysicsBody rb)
    {
      if ((HkReferenceObject) rb.RigidBody == (HkReferenceObject) null || !rb.RigidBody.IsFixed && rb.RigidBody.IsFixedOrKeyframed)
        return;
      float cargoMassMultiplier = 1f / MySession.Static.BlocksInventorySizeMultiplier;
      if (MyFakes.ENABLE_STATIC_INVENTORY_MASS)
        cargoMassMultiplier = 0.0f;
      HkMassElement[] hkMassElementArray1 = ArrayPool<HkMassElement>.Shared.Rent(blocks.Count + 1);
      int num = MyGridShape.CollectBlockInventories(blocks, cargoMassMultiplier, hkMassElementArray1);
      int length;
      if (MyPerGameSettings.Destruction)
      {
        HkMassProperties massProperties = new HkMassProperties();
        this.BreakableShape.BuildMassProperties(ref massProperties);
        HkMassElement[] hkMassElementArray2 = hkMassElementArray1;
        int index = num;
        length = index + 1;
        HkMassElement hkMassElement = new HkMassElement()
        {
          Properties = massProperties,
          Tranform = Matrix.Identity
        };
        hkMassElementArray2[index] = hkMassElement;
      }
      else
      {
        HkMassElement[] hkMassElementArray2 = hkMassElementArray1;
        int index = num;
        length = index + 1;
        HkMassElement hkMassElement = new HkMassElement()
        {
          Properties = this.m_originalMassProperties,
          Tranform = Matrix.Identity
        };
        hkMassElementArray2[index] = hkMassElement;
      }
      this.SetMassProperties(rb, new Span<HkMassElement>(hkMassElementArray1, 0, length));
      ArrayPool<HkMassElement>.Shared.Return(hkMassElementArray1);
    }

    private static int CollectBlockInventories(
      HashSet<MyCubeBlock> blocks,
      float cargoMassMultiplier,
      HkMassElement[] massElementsOut)
    {
      int num = 0;
      foreach (MyCubeBlock block in blocks)
      {
        float m = 0.0f;
        if (block is MyCockpit)
        {
          MyCockpit myCockpit = block as MyCockpit;
          if (myCockpit.Pilot != null)
            m += myCockpit.Pilot.BaseMass;
        }
        if (block.HasInventory)
        {
          for (int index = 0; index < block.InventoryCount; ++index)
          {
            MyInventory inventory = MyEntityExtensions.GetInventory(block, index);
            if (inventory != null)
              m += (float) inventory.CurrentMass * cargoMassMultiplier;
          }
        }
        if ((double) m > 0.0)
        {
          Vector3 vector3 = (block.Max - block.Min + Vector3I.One) * block.CubeGrid.GridSize;
          Vector3 position = (block.Min + block.Max) * 0.5f * block.CubeGrid.GridSize;
          HkMassProperties hkMassProperties = new HkMassProperties();
          HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(vector3 / 2f, MyPerGameSettings.Destruction ? MyDestructionHelper.MassToHavok(m) : m);
          massElementsOut[num++] = new HkMassElement()
          {
            Properties = volumeMassProperties,
            Tranform = Matrix.CreateTranslation(position)
          };
        }
      }
      return num;
    }

    private void SetMassProperties(MyPhysicsBody rb, Span<HkMassElement> massElements)
    {
      HkMassProperties massProperties;
      HkInertiaTensorComputer.CombineMassProperties(massElements, out massProperties);
      if ((double) Math.Abs(massProperties.Mass - this.m_massProperties.Mass) / (double) this.m_massProperties.Mass < (double) this.m_massElements.UpdateThreshold)
        return;
      this.m_massProperties = massProperties;
      if (Sandbox.Game.Entities.MyEntities.IsAsyncUpdateInProgress)
        this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, (Action) (() => this.SetMassPropertiesST(rb)));
      else
        this.SetMassPropertiesST(rb);
    }

    private void SetMassPropertiesST(MyPhysicsBody rb)
    {
      if (rb.IsWelded || rb.WeldInfo.Children.Count != 0)
      {
        rb.WeldedRigidBody.SetMassProperties(ref this.m_massProperties);
        rb.WeldInfo.SetMassProps(this.m_massProperties);
        rb.UpdateMassProps();
      }
      else
      {
        HkRigidBody rigidBody = rb.RigidBody;
        if (!rigidBody.IsFixedOrKeyframed || (double) Vector3.Distance(rigidBody.CenterOfMassLocal, this.m_massProperties.CenterOfMass) > 1.0)
          rigidBody.SetMassProperties(ref this.m_massProperties);
        MySharedTensorsGroups.MarkGroupDirty(this.m_grid);
        MyGridPhysicalGroupData.InvalidateSharedMassPropertiesCache(this.m_grid);
        rb.ActivateIfNeeded();
      }
      this.m_grid.NotifyMassPropertiesChanged();
    }

    public void MarkSharedTensorDirty()
    {
      this.m_isSharedTensorDirty = true;
      this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceBeforeSimulation, new Action(this.RecomputeSharedTensorIfNeeded), 25, true);
    }

    public void RecomputeSharedTensorIfNeeded()
    {
      if (!this.m_isSharedTensorDirty)
        return;
      this.m_isSharedTensorDirty = false;
      HashSetReader<MyGroups<MyCubeGrid, MySharedTensorData>.Node> gridsInSameGroup = MySharedTensorsGroups.GetGridsInSameGroup(this.m_grid);
      int num1 = gridsInSameGroup.IsValid ? gridsInSameGroup.Count : 1;
      HkMassElement[] array = ArrayPool<HkMassElement>.Shared.Rent(num1);
      Span<HkMassElement> elements = new Span<HkMassElement>(array, 0, num1);
      int num2 = 0;
      if (gridsInSameGroup.IsValid)
      {
        MatrixD matrixD1 = this.m_grid.PositionComp.WorldMatrixNormalizedInv;
        foreach (MyGroups<MyCubeGrid, MySharedTensorData>.Node node in gridsInSameGroup)
        {
          MyCubeGrid nodeData = node.NodeData;
          if (nodeData != this.m_grid)
          {
            MyGridPhysics physics = nodeData.Physics;
            if (physics != null)
            {
              HkMassProperties massProperties1 = physics.Shape.m_massProperties;
              massProperties1.Mass /= 10f;
              ref HkMassElement local1 = ref elements[num2++];
              HkMassElement hkMassElement1 = new HkMassElement();
              hkMassElement1.Properties = massProperties1;
              ref HkMassElement local2 = ref hkMassElement1;
              MatrixD matrixD2 = nodeData.PositionComp.WorldMatrixRef * matrixD1;
              Matrix matrix = (Matrix) ref matrixD2;
              local2.Tranform = matrix;
              HkMassElement hkMassElement2 = hkMassElement1;
              local1 = hkMassElement2;
            }
          }
        }
      }
      ref Span<HkMassElement> local = ref elements;
      int index = num2;
      int length = index + 1;
      local[index] = new HkMassElement()
      {
        Tranform = Matrix.Identity,
        Properties = this.m_massProperties
      };
      elements = elements.Slice(0, length);
      HkMassProperties massProperties2;
      HkInertiaTensorComputer.CombineMassProperties(elements, out massProperties2);
      ArrayPool<HkMassElement>.Shared.Return(array);
      HkMassProperties massProperties = this.m_massProperties;
      massProperties.InertiaTensor = massProperties2.InertiaTensor;
      Sandbox.Game.Entities.MyEntities.InvokeLater((Action) (() => this.SetSharedTensorST(massProperties)));
    }

    private void SetSharedTensorST(HkMassProperties massProperties)
    {
      this.m_grid.Physics.RigidBody.SetMassProperties(ref massProperties);
      this.m_grid.NotifyMassPropertiesChanged();
    }

    public static implicit operator HkShape(MyGridShape shape) => (HkShape) shape.m_root;

    public float GetBlockMass(Vector3I position)
    {
      if (this.m_blocksShapes.ContainsKey(position))
        return MyDestructionHelper.MassFromHavok(this.m_blocksShapes[position].Shape.GetMass());
      return this.m_grid.CubeExists(position) ? this.m_grid.GetCubeBlock(position).GetMass() : 1f;
    }

    internal void DebugDraw()
    {
      if (MyDebugDrawSettings.BREAKABLE_SHAPE_CHILD_COUNT)
      {
        foreach (KeyValuePair<Vector3I, HkdShapeInstanceInfo> blocksShape in this.m_blocksShapes)
        {
          Vector3D world = this.m_grid.GridIntegerToWorld((Vector3D) ((blocksShape.Value.GetTransform().Translation + blocksShape.Value.CoM) / this.m_grid.GridSize));
          if ((world - MySector.MainCamera.Position).Length() <= 20.0)
            MyRenderProxy.DebugDrawText3D(world, MyValueFormatter.GetFormatedInt(blocksShape.Value.Shape.GetChildrenCount()), Color.White, 0.65f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
        }
      }
      if (MyHonzaInputComponent.DefaultComponent.ShowRealBlockMass == MyHonzaInputComponent.DefaultComponent.ShownMassEnum.None || (this.m_grid.PositionComp.GetPosition() - MySector.MainCamera.Position).Length() > 20.0 + this.m_grid.PositionComp.WorldVolume.Radius)
        return;
      foreach (KeyValuePair<Vector3I, HkdShapeInstanceInfo> blocksShape1 in this.m_blocksShapes)
      {
        MyCubeGrid grid = this.m_grid;
        HkdShapeInstanceInfo blocksShape2 = blocksShape1.Value;
        Vector3 translation = blocksShape2.GetTransform().Translation;
        blocksShape2 = blocksShape1.Value;
        Vector3 coM = blocksShape2.CoM;
        Vector3D gridCoords = (Vector3D) ((translation + coM) / this.m_grid.GridSize);
        Vector3D world = grid.GridIntegerToWorld(gridCoords);
        if ((world - MySector.MainCamera.Position).Length() <= 20.0)
        {
          MySlimBlock cubeBlock = this.m_grid.GetCubeBlock(blocksShape1.Key);
          if (cubeBlock != null)
          {
            float num = cubeBlock.GetMass();
            if (cubeBlock.FatBlock is MyFracturedBlock)
            {
              blocksShape2 = this.m_blocksShapes[cubeBlock.Position];
              num = blocksShape2.Shape.GetMass();
            }
            switch (MyHonzaInputComponent.DefaultComponent.ShowRealBlockMass)
            {
              case MyHonzaInputComponent.DefaultComponent.ShownMassEnum.Real:
                num = MyDestructionHelper.MassFromHavok(num);
                break;
              case MyHonzaInputComponent.DefaultComponent.ShownMassEnum.SI:
                num = MyDestructionHelper.MassFromHavok(num);
                break;
            }
            MyRenderProxy.DebugDrawText3D(world, MyValueFormatter.GetFormatedFloat(num, (double) num < 10.0 ? 2 : 0), Color.White, 0.6f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
          }
        }
      }
    }

    internal void RemoveBlock(MySlimBlock block)
    {
      this.m_tmpRemovedCubes.Add(block.Min);
      this.m_root.RemoveShapes(this.m_tmpRemovedCubes);
      this.m_tmpRemovedCubes.Clear();
    }

    public void GetShapeBounds(uint shapeKey, out Vector3I min, out Vector3I max) => this.m_root.GetShapeBounds(shapeKey, out min, out max);

    public MySlimBlock GetBlockFromShapeKey(uint shapeKey)
    {
      Vector3S min;
      this.m_root.GetShapeMin(shapeKey, out min);
      return this.m_grid.GetCubeBlock((Vector3I) min);
    }
  }
}
