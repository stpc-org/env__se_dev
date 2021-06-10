// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyCubeGridSmallToLargeConnection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Groups;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  public class MyCubeGridSmallToLargeConnection : MySessionComponentBase
  {
    private static readonly HashSet<MyCubeBlock> m_tmpBlocks = new HashSet<MyCubeBlock>();
    private static readonly HashSet<MySlimBlock> m_tmpSlimBlocks = new HashSet<MySlimBlock>();
    private static readonly HashSet<MySlimBlock> m_tmpSlimBlocks2 = new HashSet<MySlimBlock>();
    private static readonly List<MySlimBlock> m_tmpSlimBlocksList = new List<MySlimBlock>();
    private static readonly HashSet<MyCubeGrid> m_tmpGrids = new HashSet<MyCubeGrid>();
    private static readonly List<MyCubeGrid> m_tmpGridList = new List<MyCubeGrid>();
    private static bool m_smallToLargeCheckEnabled = true;
    private static readonly List<MyCubeGridSmallToLargeConnection.MySlimBlockPair> m_tmpBlockConnections = new List<MyCubeGridSmallToLargeConnection.MySlimBlockPair>();
    public static MyCubeGridSmallToLargeConnection Static;
    private readonly Dictionary<MyCubeGrid, HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair>> m_mapLargeGridToConnectedBlocks = new Dictionary<MyCubeGrid, HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair>>();
    private readonly Dictionary<MyCubeGrid, HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair>> m_mapSmallGridToConnectedBlocks = new Dictionary<MyCubeGrid, HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair>>();

    public override bool IsRequiredByGame => base.IsRequiredByGame && MyFakes.ENABLE_SMALL_BLOCK_TO_LARGE_STATIC_CONNECTIONS;

    public override void LoadData()
    {
      base.LoadData();
      MyCubeGridSmallToLargeConnection.Static = this;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyCubeGridSmallToLargeConnection.Static = (MyCubeGridSmallToLargeConnection) null;
    }

    private void GetSurroundingBlocksFromStaticGrids(
      MySlimBlock block,
      MyCubeSize cubeSizeEnum,
      HashSet<MyCubeBlock> outBlocks)
    {
      outBlocks.Clear();
      BoundingBoxD boundingBox = new BoundingBoxD((Vector3D) (block.Min * block.CubeGrid.GridSize - block.CubeGrid.GridSize / 2f), (Vector3D) (block.Max * block.CubeGrid.GridSize + block.CubeGrid.GridSize / 2f));
      if (block.FatBlock != null)
      {
        Vector3D center = boundingBox.Center;
        boundingBox = (BoundingBoxD) block.FatBlock.Model.BoundingBox;
        Matrix result;
        block.FatBlock.Orientation.GetMatrix(out result);
        boundingBox = boundingBox.TransformFast((MatrixD) ref result);
        boundingBox.Translate(center);
      }
      boundingBox = boundingBox.TransformFast(block.CubeGrid.WorldMatrix);
      boundingBox.Inflate(0.125);
      List<MyEntity> foundElements = new List<MyEntity>();
      Sandbox.Game.Entities.MyEntities.GetElementsInBox(ref boundingBox, foundElements);
      for (int index = 0; index < foundElements.Count; ++index)
      {
        if (foundElements[index] is MyCubeBlock myCubeBlock && myCubeBlock.SlimBlock != block && (myCubeBlock.CubeGrid.IsStatic && myCubeBlock.CubeGrid.EnableSmallToLargeConnections) && (myCubeBlock.CubeGrid.SmallToLargeConnectionsInitialized && myCubeBlock.CubeGrid != block.CubeGrid && (myCubeBlock.CubeGrid.GridSizeEnum == cubeSizeEnum && !(myCubeBlock is MyFracturedBlock))) && !myCubeBlock.Components.Has<MyFractureComponentBase>())
        {
          if (myCubeBlock is MyCompoundCubeBlock compoundCubeBlock)
          {
            foreach (MySlimBlock block1 in compoundCubeBlock.GetBlocks())
            {
              if (block1 != block && !block1.FatBlock.Components.Has<MyFractureComponentBase>())
                outBlocks.Add(block1.FatBlock);
            }
          }
          else
            outBlocks.Add(myCubeBlock);
        }
      }
      foundElements.Clear();
    }

    private void GetSurroundingBlocksFromStaticGrids(
      MySlimBlock block,
      MyCubeSize cubeSizeEnum,
      HashSet<MySlimBlock> outBlocks)
    {
      outBlocks.Clear();
      BoundingBoxD aabbForNeighbors = new BoundingBoxD((Vector3D) (block.Min * block.CubeGrid.GridSize), (Vector3D) (block.Max * block.CubeGrid.GridSize));
      BoundingBoxD boundingBoxD = new BoundingBoxD((Vector3D) (block.Min * block.CubeGrid.GridSize - block.CubeGrid.GridSize / 2f), (Vector3D) (block.Max * block.CubeGrid.GridSize + block.CubeGrid.GridSize / 2f));
      if (block.FatBlock != null)
      {
        Vector3D center = boundingBoxD.Center;
        BoundingBoxD boundingBox = (BoundingBoxD) block.FatBlock.Model.BoundingBox;
        Matrix result;
        block.FatBlock.Orientation.GetMatrix(out result);
        boundingBoxD = boundingBox.TransformFast((MatrixD) ref result);
        boundingBoxD.Translate(center);
      }
      boundingBoxD.Inflate(0.125);
      BoundingBoxD boundingBox1 = boundingBoxD.TransformFast(block.CubeGrid.WorldMatrix);
      List<MyEntity> foundElements = new List<MyEntity>();
      Sandbox.Game.Entities.MyEntities.GetElementsInBox(ref boundingBox1, foundElements);
      for (int index = 0; index < foundElements.Count; ++index)
      {
        if (foundElements[index] is MyCubeGrid cubeGrid && cubeGrid.IsStatic && (cubeGrid != block.CubeGrid && cubeGrid.EnableSmallToLargeConnections) && (cubeGrid.SmallToLargeConnectionsInitialized && cubeGrid.GridSizeEnum == cubeSizeEnum))
        {
          MyCubeGridSmallToLargeConnection.m_tmpSlimBlocksList.Clear();
          MyCubeGrid myCubeGrid = cubeGrid;
          ref BoundingBoxD local1 = ref boundingBoxD;
          MatrixD worldMatrix = block.CubeGrid.WorldMatrix;
          ref MatrixD local2 = ref worldMatrix;
          List<MySlimBlock> tmpSlimBlocksList = MyCubeGridSmallToLargeConnection.m_tmpSlimBlocksList;
          myCubeGrid.GetBlocksIntersectingOBB(in local1, in local2, tmpSlimBlocksList);
          MyCubeGridSmallToLargeConnection.CheckNeighborBlocks(block, aabbForNeighbors, cubeGrid, MyCubeGridSmallToLargeConnection.m_tmpSlimBlocksList);
          foreach (MySlimBlock tmpSlimBlocks in MyCubeGridSmallToLargeConnection.m_tmpSlimBlocksList)
          {
            if (tmpSlimBlocks.FatBlock != null)
            {
              if (!(tmpSlimBlocks.FatBlock is MyFracturedBlock) && !tmpSlimBlocks.FatBlock.Components.Has<MyFractureComponentBase>())
              {
                if (tmpSlimBlocks.FatBlock is MyCompoundCubeBlock)
                {
                  foreach (MySlimBlock block1 in (tmpSlimBlocks.FatBlock as MyCompoundCubeBlock).GetBlocks())
                  {
                    if (!block1.FatBlock.Components.Has<MyFractureComponentBase>())
                      outBlocks.Add(block1);
                  }
                }
                else
                  outBlocks.Add(tmpSlimBlocks);
              }
            }
            else
              outBlocks.Add(tmpSlimBlocks);
          }
          MyCubeGridSmallToLargeConnection.m_tmpSlimBlocksList.Clear();
        }
      }
      foundElements.Clear();
    }

    private static void CheckNeighborBlocks(
      MySlimBlock block,
      BoundingBoxD aabbForNeighbors,
      MyCubeGrid cubeGrid,
      List<MySlimBlock> blocks)
    {
      MatrixD m = block.CubeGrid.WorldMatrix * cubeGrid.PositionComp.WorldMatrixNormalizedInv;
      BoundingBoxD boundingBoxD = aabbForNeighbors.TransformFast(ref m);
      Vector3I vector3I1 = Vector3I.Round((double) cubeGrid.GridSizeR * boundingBoxD.Min);
      Vector3I vector3I2 = Vector3I.Round((double) cubeGrid.GridSizeR * boundingBoxD.Max);
      Vector3I start = Vector3I.Min(vector3I1, vector3I2);
      Vector3I end = Vector3I.Max(vector3I1, vector3I2);
      for (int index = blocks.Count - 1; index >= 0; --index)
      {
        MySlimBlock block1 = blocks[index];
        bool flag = false;
        Vector3I_RangeIterator vector3IRangeIterator1 = new Vector3I_RangeIterator(ref block1.Min, ref block1.Max);
        Vector3I next1 = vector3IRangeIterator1.Current;
        while (vector3IRangeIterator1.IsValid())
        {
          Vector3I_RangeIterator vector3IRangeIterator2 = new Vector3I_RangeIterator(ref start, ref end);
          Vector3I next2 = vector3IRangeIterator2.Current;
          while (vector3IRangeIterator2.IsValid())
          {
            Vector3I vector3I3 = Vector3I.Abs(next1 - next2);
            if (next2 == next1 || vector3I3.X + vector3I3.Y + vector3I3.Z == 1)
            {
              flag = true;
              break;
            }
            vector3IRangeIterator2.GetNext(out next2);
          }
          if (!flag)
            vector3IRangeIterator1.GetNext(out next1);
          else
            break;
        }
        if (!flag)
          blocks.RemoveAt(index);
      }
    }

    private void ConnectSmallToLargeBlock(MySlimBlock smallBlock, MySlimBlock largeBlock)
    {
      if (MyCubeGridSmallToLargeConnection.GetCubeSize(smallBlock) != MyCubeSize.Small || MyCubeGridSmallToLargeConnection.GetCubeSize(largeBlock) != MyCubeSize.Large || (smallBlock.FatBlock is MyCompoundCubeBlock || largeBlock.FatBlock is MyCompoundCubeBlock))
        return;
      long linkId = ((long) largeBlock.UniqueId << 32) + (long) smallBlock.UniqueId;
      if (MyCubeGridGroups.Static.SmallToLargeBlockConnections.LinkExists(linkId, largeBlock, (MySlimBlock) null))
        return;
      MyCubeGridGroups.Static.SmallToLargeBlockConnections.CreateLink(linkId, largeBlock, smallBlock);
      MyCubeGridGroups.Static.Physical.CreateLink(linkId, largeBlock.CubeGrid, smallBlock.CubeGrid);
      MyCubeGridGroups.Static.Logical.CreateLink(linkId, largeBlock.CubeGrid, smallBlock.CubeGrid);
      MyCubeGridSmallToLargeConnection.MySlimBlockPair mySlimBlockPair = new MyCubeGridSmallToLargeConnection.MySlimBlockPair();
      mySlimBlockPair.Parent = largeBlock;
      mySlimBlockPair.Child = smallBlock;
      HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> mySlimBlockPairSet1;
      if (!this.m_mapLargeGridToConnectedBlocks.TryGetValue(largeBlock.CubeGrid, out mySlimBlockPairSet1))
      {
        mySlimBlockPairSet1 = new HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair>();
        this.m_mapLargeGridToConnectedBlocks.Add(largeBlock.CubeGrid, mySlimBlockPairSet1);
        largeBlock.CubeGrid.OnClosing += new Action<MyEntity>(this.CubeGrid_OnClosing);
      }
      mySlimBlockPairSet1.Add(mySlimBlockPair);
      HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> mySlimBlockPairSet2;
      if (!this.m_mapSmallGridToConnectedBlocks.TryGetValue(smallBlock.CubeGrid, out mySlimBlockPairSet2))
      {
        mySlimBlockPairSet2 = new HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair>();
        this.m_mapSmallGridToConnectedBlocks.Add(smallBlock.CubeGrid, mySlimBlockPairSet2);
        smallBlock.CubeGrid.OnClosing += new Action<MyEntity>(this.CubeGrid_OnClosing);
      }
      mySlimBlockPairSet2.Add(mySlimBlockPair);
    }

    private void DisconnectSmallToLargeBlock(
      MySlimBlock smallBlock,
      MyCubeGrid smallGrid,
      MySlimBlock largeBlock,
      MyCubeGrid largeGrid)
    {
      if (MyCubeGridSmallToLargeConnection.GetCubeSize(smallBlock) != MyCubeSize.Small || MyCubeGridSmallToLargeConnection.GetCubeSize(largeBlock) != MyCubeSize.Large || (smallBlock.FatBlock is MyCompoundCubeBlock || largeBlock.FatBlock is MyCompoundCubeBlock))
        return;
      long linkId = ((long) largeBlock.UniqueId << 32) + (long) smallBlock.UniqueId;
      MyCubeGridGroups.Static.SmallToLargeBlockConnections.BreakLink(linkId, largeBlock, (MySlimBlock) null);
      MyCubeGridGroups.Static.Physical.BreakLink(linkId, largeGrid, (MyCubeGrid) null);
      MyCubeGridGroups.Static.Logical.BreakLink(linkId, largeGrid, (MyCubeGrid) null);
      MyCubeGridSmallToLargeConnection.MySlimBlockPair mySlimBlockPair = new MyCubeGridSmallToLargeConnection.MySlimBlockPair();
      mySlimBlockPair.Parent = largeBlock;
      mySlimBlockPair.Child = smallBlock;
      HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> mySlimBlockPairSet1;
      if (this.m_mapLargeGridToConnectedBlocks.TryGetValue(largeGrid, out mySlimBlockPairSet1))
      {
        mySlimBlockPairSet1.Remove(mySlimBlockPair);
        if (mySlimBlockPairSet1.Count == 0)
        {
          this.m_mapLargeGridToConnectedBlocks.Remove(largeGrid);
          largeGrid.OnClosing -= new Action<MyEntity>(this.CubeGrid_OnClosing);
        }
      }
      HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> mySlimBlockPairSet2;
      if (!this.m_mapSmallGridToConnectedBlocks.TryGetValue(smallGrid, out mySlimBlockPairSet2))
        return;
      mySlimBlockPairSet2.Remove(mySlimBlockPair);
      if (mySlimBlockPairSet2.Count != 0)
        return;
      this.m_mapSmallGridToConnectedBlocks.Remove(smallGrid);
      smallGrid.OnClosing -= new Action<MyEntity>(this.CubeGrid_OnClosing);
    }

    private void DisconnectSmallToLargeBlock(MySlimBlock smallBlock, MySlimBlock largeBlock) => this.DisconnectSmallToLargeBlock(smallBlock, smallBlock.CubeGrid, largeBlock, largeBlock.CubeGrid);

    internal bool AddGridSmallToLargeConnection(MyCubeGrid grid)
    {
      if (!grid.IsStatic || !grid.EnableSmallToLargeConnections || !grid.SmallToLargeConnectionsInitialized)
        return false;
      bool flag = false;
      foreach (MySlimBlock block in grid.GetBlocks())
      {
        if (!(block.FatBlock is MyFracturedBlock))
        {
          bool largeConnection = this.AddBlockSmallToLargeConnection(block);
          flag |= largeConnection;
        }
      }
      return flag;
    }

    public bool AddBlockSmallToLargeConnection(MySlimBlock block)
    {
      if (!MyCubeGridSmallToLargeConnection.m_smallToLargeCheckEnabled)
        return true;
      if (!block.CubeGrid.IsStatic || !block.CubeGrid.EnableSmallToLargeConnections || !block.CubeGrid.SmallToLargeConnectionsInitialized || block.FatBlock != null && block.FatBlock.Components.Has<MyFractureComponentBase>())
        return false;
      bool flag = false;
      if (block.FatBlock is MyCompoundCubeBlock)
      {
        foreach (MySlimBlock block1 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
        {
          bool largeConnection = this.AddBlockSmallToLargeConnection(block1);
          flag |= largeConnection;
        }
        return flag;
      }
      MyCubeSize cubeSizeEnum = MyCubeGridSmallToLargeConnection.GetCubeSize(block) == MyCubeSize.Large ? MyCubeSize.Small : MyCubeSize.Large;
      this.GetSurroundingBlocksFromStaticGrids(block, cubeSizeEnum, MyCubeGridSmallToLargeConnection.m_tmpSlimBlocks2);
      if (MyCubeGridSmallToLargeConnection.m_tmpSlimBlocks2.Count == 0)
        return false;
      double cubeSize = (double) MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Small);
      BoundingBoxD aabb1;
      block.GetWorldBoundingBox(out aabb1, false);
      aabb1.Inflate(0.05);
      if (MyCubeGridSmallToLargeConnection.GetCubeSize(block) == MyCubeSize.Large)
      {
        foreach (MySlimBlock smallBlock in MyCubeGridSmallToLargeConnection.m_tmpSlimBlocks2)
        {
          BoundingBoxD aabb2;
          smallBlock.GetWorldBoundingBox(out aabb2, false);
          if (aabb2.Intersects(aabb1) && this.SmallBlockConnectsToLarge(smallBlock, ref aabb2, block, ref aabb1))
          {
            this.ConnectSmallToLargeBlock(smallBlock, block);
            flag = true;
          }
        }
      }
      else
      {
        foreach (MySlimBlock largeBlock in MyCubeGridSmallToLargeConnection.m_tmpSlimBlocks2)
        {
          BoundingBoxD aabb2;
          largeBlock.GetWorldBoundingBox(out aabb2, false);
          if (aabb2.Intersects(aabb1) && this.SmallBlockConnectsToLarge(block, ref aabb1, largeBlock, ref aabb2))
          {
            this.ConnectSmallToLargeBlock(block, largeBlock);
            flag = true;
          }
        }
      }
      return flag;
    }

    internal void RemoveBlockSmallToLargeConnection(MySlimBlock block)
    {
      if (!MyCubeGridSmallToLargeConnection.m_smallToLargeCheckEnabled || !block.CubeGrid.IsStatic)
        return;
      if (block.FatBlock is MyCompoundCubeBlock fatBlock)
      {
        foreach (MySlimBlock block1 in fatBlock.GetBlocks())
          this.RemoveBlockSmallToLargeConnection(block1);
      }
      else
      {
        MyCubeGridSmallToLargeConnection.m_tmpGrids.Clear();
        if (MyCubeGridSmallToLargeConnection.GetCubeSize(block) == MyCubeSize.Large)
        {
          this.RemoveChangedLargeBlockConnectionToSmallBlocks(block, MyCubeGridSmallToLargeConnection.m_tmpGrids);
          if (Sync.IsServer)
          {
            foreach (MyCubeGrid tmpGrid in MyCubeGridSmallToLargeConnection.m_tmpGrids)
            {
              if (tmpGrid.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason && !this.SmallGridIsStatic(tmpGrid))
                tmpGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
            }
          }
          MyCubeGridSmallToLargeConnection.m_tmpGrids.Clear();
        }
        else
        {
          MyGroups<MySlimBlock, MyBlockGroupData>.Group group = MyCubeGridGroups.Static.SmallToLargeBlockConnections.GetGroup(block);
          if (group == null)
          {
            if (!Sync.IsServer || block.CubeGrid.GetBlocks().Count <= 0 || (block.CubeGrid.TestDynamic != MyCubeGrid.MyTestDynamicReason.NoReason || this.SmallGridIsStatic(block.CubeGrid)))
              return;
            block.CubeGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
          }
          else
          {
            MyCubeGridSmallToLargeConnection.m_tmpSlimBlocks.Clear();
            foreach (MyGroups<MySlimBlock, MyBlockGroupData>.Node node in group.Nodes)
            {
              foreach (MyGroups<MySlimBlock, MyBlockGroupData>.Node child in node.Children)
              {
                if (child.NodeData == block)
                {
                  MyCubeGridSmallToLargeConnection.m_tmpSlimBlocks.Add(node.NodeData);
                  break;
                }
              }
            }
            foreach (MySlimBlock tmpSlimBlock in MyCubeGridSmallToLargeConnection.m_tmpSlimBlocks)
              this.DisconnectSmallToLargeBlock(block, tmpSlimBlock);
            MyCubeGridSmallToLargeConnection.m_tmpSlimBlocks.Clear();
            if (!Sync.IsServer || this.m_mapSmallGridToConnectedBlocks.TryGetValue(block.CubeGrid, out HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> _) || (block.CubeGrid.GetBlocks().Count <= 0 || block.CubeGrid.TestDynamic != MyCubeGrid.MyTestDynamicReason.NoReason) || this.SmallGridIsStatic(block.CubeGrid))
              return;
            block.CubeGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
          }
        }
      }
    }

    internal void ConvertToDynamic(MyCubeGrid grid)
    {
      if (grid.GridSizeEnum == MyCubeSize.Small)
        this.RemoveSmallGridConnections(grid);
      else
        this.RemoveLargeGridConnections(grid);
    }

    private void RemoveLargeGridConnections(MyCubeGrid grid)
    {
      MyCubeGridSmallToLargeConnection.m_tmpGrids.Clear();
      HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> mySlimBlockPairSet;
      if (!this.m_mapLargeGridToConnectedBlocks.TryGetValue(grid, out mySlimBlockPairSet))
        return;
      MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Clear();
      MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.AddRange((IEnumerable<MyCubeGridSmallToLargeConnection.MySlimBlockPair>) mySlimBlockPairSet);
      foreach (MyCubeGridSmallToLargeConnection.MySlimBlockPair tmpBlockConnection in MyCubeGridSmallToLargeConnection.m_tmpBlockConnections)
      {
        this.DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
        MyCubeGridSmallToLargeConnection.m_tmpGrids.Add(tmpBlockConnection.Child.CubeGrid);
      }
      MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Clear();
      if (Sync.IsServer)
      {
        MyCubeGridSmallToLargeConnection.m_tmpGridList.Clear();
        foreach (MyCubeGrid tmpGrid in MyCubeGridSmallToLargeConnection.m_tmpGrids)
        {
          if (this.m_mapSmallGridToConnectedBlocks.ContainsKey(tmpGrid))
            MyCubeGridSmallToLargeConnection.m_tmpGridList.Add(tmpGrid);
        }
        foreach (MyCubeGrid tmpGrid in MyCubeGridSmallToLargeConnection.m_tmpGridList)
          MyCubeGridSmallToLargeConnection.m_tmpGrids.Remove(tmpGrid);
        MyCubeGridSmallToLargeConnection.m_tmpGridList.Clear();
        foreach (MyCubeGrid tmpGrid in MyCubeGridSmallToLargeConnection.m_tmpGrids)
        {
          if (tmpGrid.IsStatic && tmpGrid.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason && !this.SmallGridIsStatic(tmpGrid))
            tmpGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
        }
      }
      MyCubeGridSmallToLargeConnection.m_tmpGrids.Clear();
    }

    private void RemoveSmallGridConnections(MyCubeGrid grid)
    {
      HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> mySlimBlockPairSet;
      if (!this.m_mapSmallGridToConnectedBlocks.TryGetValue(grid, out mySlimBlockPairSet))
        return;
      MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Clear();
      MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.AddRange((IEnumerable<MyCubeGridSmallToLargeConnection.MySlimBlockPair>) mySlimBlockPairSet);
      foreach (MyCubeGridSmallToLargeConnection.MySlimBlockPair tmpBlockConnection in MyCubeGridSmallToLargeConnection.m_tmpBlockConnections)
        this.DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
      MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Clear();
    }

    public bool TestGridSmallToLargeConnection(MyCubeGrid smallGrid)
    {
      HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> mySlimBlockPairSet;
      return smallGrid.IsStatic && (!Sync.IsServer || this.m_mapSmallGridToConnectedBlocks.TryGetValue(smallGrid, out mySlimBlockPairSet) && mySlimBlockPairSet.Count > 0);
    }

    private Vector3I GetSmallBlockAddDirection(
      ref BoundingBoxD smallBlockWorldAabb,
      ref BoundingBoxD smallBlockWorldAabbReduced,
      ref BoundingBoxD largeBlockWorldAabb)
    {
      if (smallBlockWorldAabbReduced.Min.X > largeBlockWorldAabb.Max.X && smallBlockWorldAabb.Min.X <= largeBlockWorldAabb.Max.X)
        return Vector3I.UnitX;
      if (smallBlockWorldAabbReduced.Max.X < largeBlockWorldAabb.Min.X && smallBlockWorldAabb.Max.X >= largeBlockWorldAabb.Min.X)
        return -Vector3I.UnitX;
      if (smallBlockWorldAabbReduced.Min.Y > largeBlockWorldAabb.Max.Y && smallBlockWorldAabb.Min.Y <= largeBlockWorldAabb.Max.Y)
        return Vector3I.UnitY;
      if (smallBlockWorldAabbReduced.Max.Y < largeBlockWorldAabb.Min.Y && smallBlockWorldAabb.Max.Y >= largeBlockWorldAabb.Min.Y)
        return -Vector3I.UnitY;
      return smallBlockWorldAabbReduced.Min.Z > largeBlockWorldAabb.Max.Z && smallBlockWorldAabb.Min.Z <= largeBlockWorldAabb.Max.Z ? Vector3I.UnitZ : -Vector3I.UnitZ;
    }

    private bool SmallBlockConnectsToLarge(
      MySlimBlock smallBlock,
      ref BoundingBoxD smallBlockWorldAabb,
      MySlimBlock largeBlock,
      ref BoundingBoxD largeBlockWorldAabb)
    {
      BoundingBoxD smallBlockWorldAabbReduced = smallBlockWorldAabb;
      smallBlockWorldAabbReduced.Inflate(-(double) smallBlock.CubeGrid.GridSize / 4.0);
      if (!largeBlockWorldAabb.Intersects(smallBlockWorldAabbReduced))
      {
        Vector3I blockAddDirection = this.GetSmallBlockAddDirection(ref smallBlockWorldAabb, ref smallBlockWorldAabbReduced, ref largeBlockWorldAabb);
        Quaternion result;
        smallBlock.Orientation.GetQuaternion(out result);
        MatrixD matrix = smallBlock.CubeGrid.WorldMatrix;
        Quaternion rotation = Quaternion.CreateFromRotationMatrix(in matrix) * result;
        if (!MyCubeGrid.CheckConnectivitySmallBlockToLargeGrid(largeBlock.CubeGrid, smallBlock.BlockDefinition, ref rotation, ref blockAddDirection))
          return false;
      }
      BoundingBoxD boundingBoxD1 = smallBlockWorldAabb;
      boundingBoxD1.Inflate(2.0 * (double) smallBlock.CubeGrid.GridSize / 3.0);
      BoundingBoxD boundingBoxD2 = boundingBoxD1.Intersect(largeBlockWorldAabb);
      Vector3D center = boundingBoxD2.Center;
      HkShape shape1 = (HkShape) new HkBoxShape((Vector3) boundingBoxD2.HalfExtents);
      Quaternion result1;
      largeBlock.Orientation.GetQuaternion(out result1);
      MatrixD matrix1 = largeBlock.CubeGrid.WorldMatrix;
      result1 = Quaternion.CreateFromRotationMatrix(in matrix1) * result1;
      Vector3D worldCenter;
      largeBlock.ComputeWorldCenter(out worldCenter);
      bool flag = false;
      try
      {
        if (largeBlock.FatBlock != null)
        {
          MyModel model = largeBlock.FatBlock.Model;
          if (model != null && model.HavokCollisionShapes != null)
          {
            foreach (HkShape havokCollisionShape in model.HavokCollisionShapes)
            {
              flag = MyPhysics.IsPenetratingShapeShape(shape1, ref center, ref Quaternion.Identity, havokCollisionShape, ref worldCenter, ref result1);
              if (flag)
                break;
            }
          }
          else
          {
            HkShape shape2 = (HkShape) new HkBoxShape(largeBlock.BlockDefinition.Size * largeBlock.CubeGrid.GridSize / 2f);
            flag = MyPhysics.IsPenetratingShapeShape(shape1, ref center, ref Quaternion.Identity, shape2, ref worldCenter, ref result1);
            shape2.RemoveReference();
          }
        }
        else
        {
          HkShape shape2 = (HkShape) new HkBoxShape(largeBlock.BlockDefinition.Size * largeBlock.CubeGrid.GridSize / 2f);
          flag = MyPhysics.IsPenetratingShapeShape(shape1, ref center, ref Quaternion.Identity, shape2, ref worldCenter, ref result1);
          shape2.RemoveReference();
        }
      }
      finally
      {
        shape1.RemoveReference();
      }
      return flag;
    }

    private void RemoveChangedLargeBlockConnectionToSmallBlocks(
      MySlimBlock block,
      HashSet<MyCubeGrid> outSmallGrids)
    {
      MyGroups<MySlimBlock, MyBlockGroupData>.Group group = MyCubeGridGroups.Static.SmallToLargeBlockConnections.GetGroup(block);
      if (group == null)
        return;
      MyCubeGridSmallToLargeConnection.m_tmpSlimBlocks.Clear();
      foreach (MyGroups<MySlimBlock, MyBlockGroupData>.Node node in group.Nodes)
      {
        if (node.NodeData == block)
        {
          using (SortedDictionary<long, MyGroups<MySlimBlock, MyBlockGroupData>.Node>.ValueCollection.Enumerator enumerator = node.Children.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              MyGroups<MySlimBlock, MyBlockGroupData>.Node current = enumerator.Current;
              MyCubeGridSmallToLargeConnection.m_tmpSlimBlocks.Add(current.NodeData);
            }
            break;
          }
        }
      }
      foreach (MySlimBlock tmpSlimBlock in MyCubeGridSmallToLargeConnection.m_tmpSlimBlocks)
      {
        this.DisconnectSmallToLargeBlock(tmpSlimBlock, block);
        outSmallGrids.Add(tmpSlimBlock.CubeGrid);
      }
      MyCubeGridSmallToLargeConnection.m_tmpSlimBlocks.Clear();
      MyCubeGridSmallToLargeConnection.m_tmpGridList.Clear();
      foreach (MyCubeGrid outSmallGrid in outSmallGrids)
      {
        if (this.m_mapSmallGridToConnectedBlocks.TryGetValue(outSmallGrid, out HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> _))
          MyCubeGridSmallToLargeConnection.m_tmpGridList.Add(outSmallGrid);
      }
      foreach (MyCubeGrid tmpGrid in MyCubeGridSmallToLargeConnection.m_tmpGridList)
        outSmallGrids.Remove(tmpGrid);
      MyCubeGridSmallToLargeConnection.m_tmpGridList.Clear();
    }

    private bool SmallGridIsStatic(MyCubeGrid smallGrid) => this.TestGridSmallToLargeConnection(smallGrid);

    internal void BeforeGridSplit_SmallToLargeGridConnectivity(MyCubeGrid originalGrid) => MyCubeGridSmallToLargeConnection.m_smallToLargeCheckEnabled = false;

    internal void AfterGridSplit_SmallToLargeGridConnectivity(
      MyCubeGrid originalGrid,
      List<MyCubeGrid> gridSplits)
    {
      MyCubeGridSmallToLargeConnection.m_smallToLargeCheckEnabled = true;
      if (originalGrid.GridSizeEnum == MyCubeSize.Small)
        this.AfterGridSplit_Small(originalGrid, gridSplits);
      else
        this.AfterGridSplit_Large(originalGrid, gridSplits);
    }

    private void AfterGridSplit_Small(MyCubeGrid originalGrid, List<MyCubeGrid> gridSplits)
    {
      if (!originalGrid.IsStatic)
        return;
      HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> mySlimBlockPairSet;
      if (this.m_mapSmallGridToConnectedBlocks.TryGetValue(originalGrid, out mySlimBlockPairSet))
      {
        MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Clear();
        foreach (MyCubeGridSmallToLargeConnection.MySlimBlockPair mySlimBlockPair in mySlimBlockPairSet)
        {
          if (mySlimBlockPair.Child.CubeGrid != originalGrid)
            MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Add(mySlimBlockPair);
        }
        foreach (MyCubeGridSmallToLargeConnection.MySlimBlockPair tmpBlockConnection in MyCubeGridSmallToLargeConnection.m_tmpBlockConnections)
        {
          this.DisconnectSmallToLargeBlock(tmpBlockConnection.Child, originalGrid, tmpBlockConnection.Parent, tmpBlockConnection.Parent.CubeGrid);
          this.ConnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
        }
        MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Clear();
      }
      if (!Sync.IsServer)
        return;
      if (!this.m_mapSmallGridToConnectedBlocks.TryGetValue(originalGrid, out mySlimBlockPairSet) || mySlimBlockPairSet.Count == 0)
        originalGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
      foreach (MyCubeGrid gridSplit in gridSplits)
      {
        if (!this.m_mapSmallGridToConnectedBlocks.TryGetValue(gridSplit, out mySlimBlockPairSet) || mySlimBlockPairSet.Count == 0)
          gridSplit.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
      }
    }

    private void AfterGridSplit_Large(MyCubeGrid originalGrid, List<MyCubeGrid> gridSplits)
    {
      HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> mySlimBlockPairSet;
      if (!originalGrid.IsStatic || !this.m_mapLargeGridToConnectedBlocks.TryGetValue(originalGrid, out mySlimBlockPairSet))
        return;
      MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Clear();
      foreach (MyCubeGridSmallToLargeConnection.MySlimBlockPair mySlimBlockPair in mySlimBlockPairSet)
      {
        if (mySlimBlockPair.Parent.CubeGrid != originalGrid)
          MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Add(mySlimBlockPair);
      }
      foreach (MyCubeGridSmallToLargeConnection.MySlimBlockPair tmpBlockConnection in MyCubeGridSmallToLargeConnection.m_tmpBlockConnections)
      {
        this.DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Child.CubeGrid, tmpBlockConnection.Parent, originalGrid);
        this.ConnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
      }
      MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Clear();
    }

    internal void BeforeGridMerge_SmallToLargeGridConnectivity(
      MyCubeGrid originalGrid,
      MyCubeGrid mergedGrid)
    {
      MyCubeGridSmallToLargeConnection.m_tmpGrids.Clear();
      if (originalGrid.IsStatic && mergedGrid.IsStatic)
        MyCubeGridSmallToLargeConnection.m_tmpGrids.Add(mergedGrid);
      MyCubeGridSmallToLargeConnection.m_smallToLargeCheckEnabled = false;
    }

    internal void AfterGridMerge_SmallToLargeGridConnectivity(MyCubeGrid originalGrid)
    {
      MyCubeGridSmallToLargeConnection.m_smallToLargeCheckEnabled = true;
      if (MyCubeGridSmallToLargeConnection.m_tmpGrids.Count == 0 || !originalGrid.IsStatic)
        return;
      if (originalGrid.GridSizeEnum == MyCubeSize.Large)
      {
        foreach (MyCubeGrid tmpGrid in MyCubeGridSmallToLargeConnection.m_tmpGrids)
        {
          HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> mySlimBlockPairSet;
          if (this.m_mapLargeGridToConnectedBlocks.TryGetValue(tmpGrid, out mySlimBlockPairSet))
          {
            MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Clear();
            MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.AddRange((IEnumerable<MyCubeGridSmallToLargeConnection.MySlimBlockPair>) mySlimBlockPairSet);
            foreach (MyCubeGridSmallToLargeConnection.MySlimBlockPair tmpBlockConnection in MyCubeGridSmallToLargeConnection.m_tmpBlockConnections)
            {
              this.DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Child.CubeGrid, tmpBlockConnection.Parent, tmpGrid);
              this.ConnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
            }
          }
        }
      }
      else
      {
        foreach (MyCubeGrid tmpGrid in MyCubeGridSmallToLargeConnection.m_tmpGrids)
        {
          HashSet<MyCubeGridSmallToLargeConnection.MySlimBlockPair> mySlimBlockPairSet;
          if (this.m_mapSmallGridToConnectedBlocks.TryGetValue(tmpGrid, out mySlimBlockPairSet))
          {
            MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Clear();
            MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.AddRange((IEnumerable<MyCubeGridSmallToLargeConnection.MySlimBlockPair>) mySlimBlockPairSet);
            foreach (MyCubeGridSmallToLargeConnection.MySlimBlockPair tmpBlockConnection in MyCubeGridSmallToLargeConnection.m_tmpBlockConnections)
            {
              this.DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpGrid, tmpBlockConnection.Parent, tmpBlockConnection.Parent.CubeGrid);
              this.ConnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
            }
          }
        }
      }
      MyCubeGridSmallToLargeConnection.m_tmpGrids.Clear();
      MyCubeGridSmallToLargeConnection.m_tmpBlockConnections.Clear();
    }

    private void CubeGrid_OnClosing(MyEntity entity)
    {
      MyCubeGrid grid = (MyCubeGrid) entity;
      if (grid.GridSizeEnum == MyCubeSize.Small)
        this.RemoveSmallGridConnections(grid);
      else
        this.RemoveLargeGridConnections(grid);
    }

    private static MyCubeSize GetCubeSize(MySlimBlock block)
    {
      if (block.CubeGrid != null)
        return block.CubeGrid.GridSizeEnum;
      MyCubeBlockDefinition blockDefinition;
      return block.FatBlock is MyFracturedBlock fatBlock && fatBlock.OriginalBlocks.Count > 0 && MyDefinitionManager.Static.TryGetCubeBlockDefinition(fatBlock.OriginalBlocks[0], out blockDefinition) ? blockDefinition.CubeSize : block.BlockDefinition.CubeSize;
    }

    private struct MySlimBlockPair : IEquatable<MyCubeGridSmallToLargeConnection.MySlimBlockPair>
    {
      public MySlimBlock Parent;
      public MySlimBlock Child;

      public override int GetHashCode() => this.Parent.GetHashCode() ^ this.Child.GetHashCode();

      public override bool Equals(object obj) => obj is MyCubeGridSmallToLargeConnection.MySlimBlockPair mySlimBlockPair && this.Parent == mySlimBlockPair.Parent && this.Child == mySlimBlockPair.Child;

      public bool Equals(
        MyCubeGridSmallToLargeConnection.MySlimBlockPair other)
      {
        return this.Parent == other.Parent && this.Child == other.Child;
      }
    }
  }
}
