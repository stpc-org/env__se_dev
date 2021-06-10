// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyDisconnectHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public class MyDisconnectHelper
  {
    private HashSet<MySlimBlock> m_disconnectHelper = new HashSet<MySlimBlock>();
    private Queue<MySlimBlock> m_neighborSearchBaseStack = new Queue<MySlimBlock>();
    private List<MySlimBlock> m_sortedBlocks = new List<MySlimBlock>();
    private List<MyDisconnectHelper.Group> m_groups = new List<MyDisconnectHelper.Group>();
    private MyDisconnectHelper.Group m_largestGroupWithPhysics;

    public bool Disconnect(
      MyCubeGrid grid,
      MyCubeGrid.MyTestDisconnectsReason reason,
      MySlimBlock testBlock = null,
      bool testDisconnect = false)
    {
      this.m_largestGroupWithPhysics = new MyDisconnectHelper.Group();
      this.m_groups.Clear();
      this.m_sortedBlocks.Clear();
      this.m_disconnectHelper.Clear();
      foreach (MySlimBlock block in grid.GetBlocks())
      {
        if (block != testBlock)
          this.m_disconnectHelper.Add(block);
      }
      while (this.m_disconnectHelper.Count > 0)
      {
        MyDisconnectHelper.Group group = new MyDisconnectHelper.Group();
        group.FirstBlockIndex = this.m_sortedBlocks.Count;
        this.AddNeighbours(this.m_disconnectHelper.FirstElement<MySlimBlock>(), out group.IsValid, testBlock);
        group.BlockCount = this.m_sortedBlocks.Count - group.FirstBlockIndex;
        if (group.IsValid && group.BlockCount > this.m_largestGroupWithPhysics.BlockCount)
        {
          if (this.m_largestGroupWithPhysics.BlockCount > 0)
          {
            int index;
            for (index = 0; index < this.m_groups.Count; ++index)
            {
              if (this.m_groups[index].FirstBlockIndex > this.m_largestGroupWithPhysics.FirstBlockIndex)
              {
                this.m_groups.Insert(index, this.m_largestGroupWithPhysics);
                break;
              }
            }
            if (index == this.m_groups.Count)
              this.m_groups.Add(this.m_largestGroupWithPhysics);
          }
          this.m_largestGroupWithPhysics = group;
        }
        else
          this.m_groups.Add(group);
      }
      bool flag = this.m_groups.Count > 0;
      if (this.m_groups.Count > 0 && !testDisconnect)
      {
        grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, (Action) (() => this.DoDisconnects(grid, reason)));
        return true;
      }
      this.m_groups.Clear();
      this.m_sortedBlocks.Clear();
      this.m_disconnectHelper.Clear();
      return flag;
    }

    private void DoDisconnects(MyCubeGrid grid, MyCubeGrid.MyTestDisconnectsReason reason)
    {
      this.m_sortedBlocks.RemoveRange(this.m_largestGroupWithPhysics.FirstBlockIndex, this.m_largestGroupWithPhysics.BlockCount);
      for (int index = 0; index < this.m_groups.Count; ++index)
      {
        MyDisconnectHelper.Group group = this.m_groups[index];
        if (group.FirstBlockIndex > this.m_largestGroupWithPhysics.FirstBlockIndex)
        {
          group.FirstBlockIndex -= this.m_largestGroupWithPhysics.BlockCount;
          this.m_groups[index] = group;
        }
      }
      MyCubeGrid.CreateSplits(grid, this.m_sortedBlocks, this.m_groups, reason);
      this.m_groups.Clear();
      this.m_sortedBlocks.Clear();
      this.m_disconnectHelper.Clear();
    }

    private void AddNeighbours(
      MySlimBlock firstBlock,
      out bool anyWithPhysics,
      MySlimBlock testBlock)
    {
      anyWithPhysics = false;
      if (this.m_disconnectHelper.Remove(firstBlock))
      {
        anyWithPhysics |= firstBlock.BlockDefinition.HasPhysics;
        this.m_sortedBlocks.Add(firstBlock);
        this.m_neighborSearchBaseStack.Enqueue(firstBlock);
      }
      while (this.m_neighborSearchBaseStack.Count > 0)
      {
        foreach (MySlimBlock neighbour in this.m_neighborSearchBaseStack.Dequeue().Neighbours)
        {
          if (neighbour != testBlock && this.m_disconnectHelper.Remove(neighbour))
          {
            anyWithPhysics |= neighbour.BlockDefinition.HasPhysics;
            this.m_sortedBlocks.Add(neighbour);
            this.m_neighborSearchBaseStack.Enqueue(neighbour);
          }
        }
      }
    }

    public static bool IsDestroyedInVoxels(MySlimBlock block)
    {
      if (block == null || block.CubeGrid.IsStatic)
        return false;
      MyCubeGrid cubeGrid = block.CubeGrid;
      Vector3D pos = Vector3D.Transform((block.Max + block.Min) * 0.5f * cubeGrid.GridSize, cubeGrid.WorldMatrix);
      return Sandbox.Game.Entities.MyEntities.IsInsideVoxel(pos, pos - cubeGrid.Physics.LinearVelocity * 1.5f, out Vector3D _);
    }

    public bool TryDisconnect(MySlimBlock testBlock) => this.Disconnect(testBlock.CubeGrid, MyCubeGrid.MyTestDisconnectsReason.NoReason, testBlock, true);

    [Serializable]
    public struct Group
    {
      public int FirstBlockIndex;
      public int BlockCount;
      public bool IsValid;
      public long EntityId;

      protected class Sandbox_Game_Entities_Cube_MyDisconnectHelper\u003C\u003EGroup\u003C\u003EFirstBlockIndex\u003C\u003EAccessor : IMemberAccessor<MyDisconnectHelper.Group, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyDisconnectHelper.Group owner, in int value) => owner.FirstBlockIndex = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyDisconnectHelper.Group owner, out int value) => value = owner.FirstBlockIndex;
      }

      protected class Sandbox_Game_Entities_Cube_MyDisconnectHelper\u003C\u003EGroup\u003C\u003EBlockCount\u003C\u003EAccessor : IMemberAccessor<MyDisconnectHelper.Group, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyDisconnectHelper.Group owner, in int value) => owner.BlockCount = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyDisconnectHelper.Group owner, out int value) => value = owner.BlockCount;
      }

      protected class Sandbox_Game_Entities_Cube_MyDisconnectHelper\u003C\u003EGroup\u003C\u003EIsValid\u003C\u003EAccessor : IMemberAccessor<MyDisconnectHelper.Group, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyDisconnectHelper.Group owner, in bool value) => owner.IsValid = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyDisconnectHelper.Group owner, out bool value) => value = owner.IsValid;
      }

      protected class Sandbox_Game_Entities_Cube_MyDisconnectHelper\u003C\u003EGroup\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<MyDisconnectHelper.Group, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyDisconnectHelper.Group owner, in long value) => owner.EntityId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyDisconnectHelper.Group owner, out long value) => value = owner.EntityId;
      }
    }
  }
}
