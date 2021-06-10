// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridGasSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
  public class MyGridGasSystem : MyUpdateableGridSystem
  {
    private static bool DEBUG_MODE = false;
    public const float OXYGEN_UNIFORMIZATION_TIME_MS = 1500f;
    private readonly Vector3I[] m_neighbours = new Vector3I[6]
    {
      new Vector3I(1, 0, 0),
      new Vector3I(-1, 0, 0),
      new Vector3I(0, 1, 0),
      new Vector3I(0, -1, 0),
      new Vector3I(0, 0, 1),
      new Vector3I(0, 0, -1)
    };
    private readonly Vector3I[] m_neighboursForDelete = new Vector3I[7]
    {
      new Vector3I(1, 0, 0),
      new Vector3I(-1, 0, 0),
      new Vector3I(0, 1, 0),
      new Vector3I(0, -1, 0),
      new Vector3I(0, 0, 1),
      new Vector3I(0, 0, -1),
      new Vector3I(0, 0, 0)
    };
    private static readonly MySoundPair m_airleakSound = new MySoundPair("EventAirVent");
    private bool m_isProcessingData;
    private MyOxygenCube m_cubeRoom;
    private MyConcurrentList<MyOxygenRoom> m_rooms;
    private int m_lastRoomIndex;
    private System.Collections.Generic.Queue<Vector3I> m_blockQueue = new System.Collections.Generic.Queue<Vector3I>();
    private Vector3I m_storedGridMin;
    private Vector3I m_storedGridMax;
    private Vector3I m_previousGridMin;
    private Vector3I m_previousGridMax;
    private OxygenRoom[] m_savedRooms;
    private List<IMySlimBlock> m_gasBlocks = new List<IMySlimBlock>();
    private List<IMySlimBlock> m_gasBlocksForUpdate = new List<IMySlimBlock>();
    private bool m_generatedDataPending;
    private bool m_gridExpanded;
    private bool m_gridShrinked;
    private List<IMySlimBlock> m_deletedBlocks = new List<IMySlimBlock>();
    private List<IMySlimBlock> m_deletedBlocksSwap = new List<IMySlimBlock>();
    private List<IMySlimBlock> m_addedBlocks = new List<IMySlimBlock>();
    private List<IMySlimBlock> m_addedBlocksSwap = new List<IMySlimBlock>();
    private Task m_backgroundTask;
    private int m_lastUpdateTime;
    private bool isClosing;
    private HashSet<Vector3I> m_visitedBlocks = new HashSet<Vector3I>();
    private HashSet<Vector3I> m_initializedBlocks = new HashSet<Vector3I>();
    private IMyCubeGrid m_cubeGrid;
    private readonly float m_debugTextlineSize = 17f;
    private bool m_debugShowTopRoom;
    private bool m_debugShowRoomIndex = true;
    private bool m_debugShowPositions;
    private int m_debugRoomIndex;
    private bool m_debugShowBlockCount;
    private bool m_debugShowOxygenAmount;
    private bool m_debugToggleView;

    internal IMyCubeGrid CubeGrid => (IMyCubeGrid) this.Grid;

    public MyGridGasSystem(IMyCubeGrid grid)
      : base(grid as MyCubeGrid)
    {
      grid.OnBlockAdded += new Action<IMySlimBlock>(this.cubeGrid_OnBlockAdded);
      grid.OnBlockRemoved += new Action<IMySlimBlock>(this.cubeGrid_OnBlockRemoved);
      this.m_lastUpdateTime = this.GetTotalGamePlayTimeInMilliseconds();
    }

    private int GetTotalGamePlayTimeInMilliseconds() => MySandboxGame.TotalGamePlayTimeInMilliseconds;

    public void OnGridClosing()
    {
      this.isClosing = true;
      if (this.m_isProcessingData)
      {
        try
        {
          this.m_backgroundTask.WaitOrExecute();
        }
        catch (Exception ex)
        {
          MySandboxGame.Log.WriteLineAndConsole("MyGridGasSystem.OnGridClosing: " + ex.Message + ", " + ex.StackTrace);
        }
      }
      this.CubeGrid.OnBlockAdded -= new Action<IMySlimBlock>(this.cubeGrid_OnBlockAdded);
      this.CubeGrid.OnBlockRemoved -= new Action<IMySlimBlock>(this.cubeGrid_OnBlockRemoved);
      if (this.CubeGrid is MyCubeGrid cubeGrid)
      {
        foreach (MyCubeBlock fatBlock in cubeGrid.GetFatBlocks())
        {
          if (fatBlock is Sandbox.ModAPI.IMyDoor myDoor)
            myDoor.OnDoorStateChanged -= new Action<Sandbox.ModAPI.IMyDoor, bool>(this.OnDoorStateChanged);
        }
      }
      this.Clear();
    }

    private void Clear()
    {
      this.m_rooms = (MyConcurrentList<MyOxygenRoom>) null;
      this.m_cubeRoom = (MyOxygenCube) null;
      this.m_lastRoomIndex = 0;
      this.m_visitedBlocks.Clear();
      this.m_initializedBlocks.Clear();
    }

    private void ScheduleUpdate()
    {
      if (this.Grid == null)
        return;
      this.Schedule();
    }

    private void cubeGrid_OnBlockAdded(IMySlimBlock addedBlock)
    {
      if (addedBlock.FatBlock is Sandbox.ModAPI.IMyDoor)
        ((Sandbox.ModAPI.IMyDoor) addedBlock.FatBlock).OnDoorStateChanged += new Action<Sandbox.ModAPI.IMyDoor, bool>(this.OnDoorStateChanged);
      if (addedBlock.FatBlock is IMyGasBlock fatBlock && fatBlock.CanPressurizeRoom)
        this.m_gasBlocks.Add(addedBlock);
      if (this.m_gasBlocks.Count == 0)
        return;
      this.m_addedBlocks.Add(addedBlock);
      Vector3I vector3I1 = !this.m_isProcessingData ? this.m_storedGridMin : this.m_previousGridMin;
      Vector3I vector3I2 = !this.m_isProcessingData ? this.m_storedGridMax : this.m_previousGridMax;
      if (Vector3I.Min(this.GridMin(), vector3I1) != vector3I1 || Vector3I.Max(this.GridMax(), vector3I2) != vector3I2)
        this.m_gridExpanded = true;
      if (this.m_rooms == null)
        this.m_generatedDataPending = true;
      this.ScheduleUpdate();
    }

    internal void OnSlimBlockBuildRatioRaised(IMySlimBlock block)
    {
      if (!(block.BlockDefinition is MyCubeBlockDefinition blockDefinition) || blockDefinition.BuildProgressModels == null || blockDefinition.BuildProgressModels.Length == 0)
        return;
      MyCubeBlockDefinition.BuildProgressModel buildProgressModel = blockDefinition.BuildProgressModels[blockDefinition.BuildProgressModels.Length - 1];
      if ((double) block.BuildLevelRatio < (double) buildProgressModel.BuildRatioUpperBound)
        return;
      this.cubeGrid_OnBlockAdded(block);
    }

    private void cubeGrid_OnBlockRemoved(IMySlimBlock deletedBlock)
    {
      if (deletedBlock.FatBlock is Sandbox.ModAPI.IMyDoor fatBlock)
        fatBlock.OnDoorStateChanged -= new Action<Sandbox.ModAPI.IMyDoor, bool>(this.OnDoorStateChanged);
      if (deletedBlock.FatBlock is IMyGasBlock fatBlock && fatBlock.CanPressurizeRoom)
        this.m_gasBlocks.Remove(deletedBlock);
      if (this.m_gasBlocks.Count == 0 && fatBlock == null)
        return;
      this.m_deletedBlocks.Add(deletedBlock);
      this.ScheduleUpdate();
    }

    internal void OnSlimBlockBuildRatioLowered(IMySlimBlock block)
    {
      if (!(block.BlockDefinition is MyCubeBlockDefinition blockDefinition) || blockDefinition.BuildProgressModels == null || blockDefinition.BuildProgressModels.Length == 0)
        return;
      int num = 0;
      for (int index = blockDefinition.BuildProgressModels.Length - 1; index >= 0; --index)
      {
        if ((double) blockDefinition.BuildProgressModels[index].BuildRatioUpperBound > (double) block.BuildLevelRatio)
          num = index;
      }
      if (num != blockDefinition.BuildProgressModels.Length - 1)
        return;
      this.cubeGrid_OnBlockRemoved(block);
    }

    private void OnDoorStateChanged(Sandbox.ModAPI.IMyDoor door, bool areOpen)
    {
      if (this.m_gasBlocks.Count == 0)
        return;
      if (door.SlimBlock is MySlimBlock slimBlock)
      {
        if (areOpen)
          this.m_deletedBlocks.Add((IMySlimBlock) slimBlock);
        else
          this.m_addedBlocks.Add((IMySlimBlock) slimBlock);
      }
      this.ScheduleUpdate();
    }

    public void OnCubeGridShrinked()
    {
      if (this.m_rooms == null)
        this.m_generatedDataPending = true;
      else
        this.m_gridShrinked = true;
      this.ScheduleUpdate();
    }

    internal void UpdateBeforeSimulation() => this.Update();

    protected override void Update()
    {
      if (!MyFakes.BACKGROUND_OXYGEN || this.m_isProcessingData)
        return;
      bool flag = false;
      MySimpleProfiler.Begin("Gas System", MySimpleProfiler.ProfilingBlockType.BLOCK, nameof (Update));
      if (this.m_generatedDataPending)
      {
        if (MyFakes.BACKGROUND_OXYGEN && this.ShouldPressurize())
          this.StartGenerateAirtightData();
        flag = true;
        this.m_generatedDataPending = false;
      }
      if (this.m_gridShrinked)
      {
        this.StartShrinkData();
        flag = true;
      }
      if (this.m_addedBlocks.Count > 0)
      {
        this.StartRefreshRoomData();
        flag = true;
      }
      if (this.m_deletedBlocks.Count > 0)
      {
        this.StartRemoveBlocks();
        flag = true;
      }
      if (flag)
        this.ScheduleUpdate();
      MySimpleProfiler.End(nameof (Update));
    }

    public void OnAltitudeChanged()
    {
      if (this.m_rooms == null)
        return;
      foreach (MyOxygenRoom room in this.m_rooms)
        room.EnvironmentOxygen = MyOxygenProviderSystem.GetOxygenInPoint(this.CubeGrid.GridIntegerToWorld(room.StartingPosition));
      this.ScheduleUpdate();
    }

    private bool ShouldPressurize()
    {
      if (this.CubeGrid.Physics == null)
        return false;
      if (this.m_gasBlocks.Count > 0)
        return true;
      if (this.m_rooms == null)
        return false;
      for (int index = 0; index < this.m_rooms.Count; ++index)
      {
        MyOxygenRoom room = this.m_rooms[index];
        if (room.IsAirtight && (double) room.OxygenAmount > 1.0 || !room.IsAirtight && (double) (this.GetTotalGamePlayTimeInMilliseconds() - room.DepressurizationTime) < 1500.0)
          return true;
      }
      this.m_rooms = (MyConcurrentList<MyOxygenRoom>) null;
      this.m_lastRoomIndex = 0;
      this.m_cubeRoom = (MyOxygenCube) null;
      return false;
    }

    private void StartShrinkData()
    {
      if (this.m_isProcessingData)
        return;
      this.m_previousGridMin = this.m_storedGridMin;
      this.m_previousGridMax = this.m_storedGridMax;
      this.m_isProcessingData = true;
      this.m_gridShrinked = false;
      this.m_backgroundTask = Parallel.Start(new Action(this.ShrinkData), new Action(this.OnBackgroundTaskFinished));
    }

    private void ShrinkData()
    {
      if (this.m_cubeRoom == null)
        return;
      Vector3I vector3I1 = this.GridMin();
      Vector3I vector3I2 = this.GridMax();
      Vector3I vector3I3 = vector3I1 - this.m_storedGridMin;
      Vector3I vector3I4 = this.m_storedGridMax - vector3I2;
      Vector3I zero = Vector3I.Zero;
      if (!(vector3I3 != zero) && !(vector3I4 != Vector3I.Zero))
        return;
      this.m_storedGridMin = vector3I1;
      this.m_storedGridMax = vector3I2;
      MyOxygenRoom room = this.m_rooms[0];
      HashSet<Vector3I> vector3ISet = new HashSet<Vector3I>();
      foreach (Vector3I block in room.Blocks)
      {
        if (!this.IsInBounds(block))
          vector3ISet.Add(block);
      }
      if (vector3ISet.Count <= 0)
        return;
      room.Blocks.ExceptWith((IEnumerable<Vector3I>) vector3ISet);
      room.BlockCount = room.Blocks.Count;
      room.StartingPosition = this.m_storedGridMin;
    }

    private void StartRefreshRoomData()
    {
      if (this.m_isProcessingData)
        return;
      if (this.m_cubeRoom == null)
      {
        this.m_addedBlocks.Clear();
        this.m_gridExpanded = false;
      }
      else
      {
        if (this.m_gridExpanded)
        {
          this.m_previousGridMin = this.m_storedGridMin;
          this.m_previousGridMax = this.m_storedGridMax;
        }
        List<IMySlimBlock> addedBlocksSwap = this.m_addedBlocksSwap;
        this.m_addedBlocksSwap = this.m_addedBlocks;
        this.m_addedBlocks = addedBlocksSwap;
        this.m_gasBlocksForUpdate.Clear();
        this.m_gasBlocksForUpdate.AddRange((IEnumerable<IMySlimBlock>) this.m_gasBlocks);
        this.m_isProcessingData = true;
        this.m_backgroundTask = Parallel.Start(new Action(this.RefreshRoomData), new Action(this.OnBackgroundTaskFinished));
      }
    }

    private void RefreshRoomData()
    {
      if (this.m_cubeRoom == null)
        return;
      if (this.m_gridExpanded)
      {
        this.m_gridExpanded = false;
        this.ExpandAirtightData();
      }
      foreach (IMySlimBlock block in this.m_addedBlocksSwap)
        this.AddBlock(block);
      this.m_addedBlocksSwap.Clear();
      this.RefreshTopRoom();
      this.RefreshDirtyRooms();
      this.m_initializedBlocks.Clear();
      this.GenerateGasBlockRooms();
      this.GenerateEmptyRooms();
      this.m_initializedBlocks.Clear();
    }

    private void RefreshTopRoom()
    {
      MyOxygenRoom room = this.m_rooms[0];
      if (!room.IsDirty)
        return;
      HashSet<Vector3I> roomBlocks = this.GetRoomBlocks(this.m_storedGridMin, room);
      HashSet<Vector3I> blocks = room.Blocks;
      blocks.ExceptWith((IEnumerable<Vector3I>) roomBlocks);
      if (blocks.Count != 0)
        this.CreateAirtightRoom(blocks, 0.0f, blocks.FirstElement<Vector3I>()).IsDirty = true;
      room.BlockCount = roomBlocks.Count;
      room.Blocks = roomBlocks;
      room.IsDirty = false;
      room.StartingPosition = this.m_storedGridMin;
    }

    private void ExpandAirtightData()
    {
      Vector3I vector3I1 = this.GridMin();
      Vector3I vector3I2 = this.GridMax();
      Vector3I vector3I3 = this.m_storedGridMin - vector3I1;
      Vector3I vector3I4 = vector3I2 - this.m_storedGridMax;
      Vector3I zero = Vector3I.Zero;
      if (!(vector3I3 != zero) && !(vector3I4 != Vector3I.Zero))
        return;
      Vector3I vector3I5 = vector3I2 - vector3I1 + Vector3I.One;
      this.m_rooms[0].IsDirty = true;
      this.m_storedGridMin = vector3I1;
      this.m_storedGridMax = vector3I2;
    }

    private void AddBlock(IMySlimBlock block)
    {
      Vector3I next = block.Min;
      Vector3I min = block.Min;
      Vector3I max = block.Max;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref min, ref max);
      while (vector3IRangeIterator.IsValid())
      {
        MyOxygenRoom cubeGridPosition = this.GetOxygenRoomForCubeGridPosition(ref next);
        if (cubeGridPosition != null)
        {
          cubeGridPosition.IsDirty = true;
          bool flag1 = false;
          if (block.FatBlock is Sandbox.ModAPI.IMyDoor fatBlock)
          {
            flag1 = true;
            if (fatBlock is MyAirtightSlideDoor)
              break;
          }
          MyCubeBlockDefinition blockDefinition = block.BlockDefinition as MyCubeBlockDefinition;
          bool? nullable1 = this.IsAirtightFromDefinition(blockDefinition, block.BuildLevelRatio);
          int num1;
          if (blockDefinition != null)
          {
            bool? nullable2 = nullable1;
            bool flag2 = true;
            num1 = nullable2.GetValueOrDefault() == flag2 & nullable2.HasValue ? 1 : 0;
          }
          else
            num1 = 0;
          int num2 = flag1 ? 1 : 0;
          if ((num1 | num2) != 0)
          {
            Vector3I vector3I = next;
            --cubeGridPosition.BlockCount;
            cubeGridPosition.Blocks.Remove(vector3I);
            this.m_cubeRoom[vector3I.X, vector3I.Y, vector3I.Z].RoomLink = (MyOxygenRoomLink) null;
          }
        }
        vector3IRangeIterator.GetNext(out next);
      }
    }

    private void RefreshDirtyRooms()
    {
      int count = this.m_rooms.Count;
      for (int index = 0; index < count; ++index)
      {
        MyOxygenRoom room = this.m_rooms[index];
        if (room.Index != 0)
          this.RefreshRoomBlocks(room);
      }
    }

    private void RefreshRoomBlocks(MyOxygenRoom room)
    {
      if (room == null || room.IsAirtight && !room.IsDirty)
        return;
      MyOxygenRoom room1 = this.m_rooms[0];
      Vector3I startingPosition = room.StartingPosition;
      Vector3I vector3I1 = startingPosition;
      this.m_blockQueue.Clear();
      this.m_blockQueue.Enqueue(vector3I1);
      HashSet<Vector3I> vector3ISet = new HashSet<Vector3I>();
      vector3ISet.Add(vector3I1);
      bool flag1 = true;
      while (this.m_blockQueue.Count > 0)
      {
        Vector3I startPos = this.m_blockQueue.Dequeue();
        for (int index = 0; index < this.m_neighbours.Length; ++index)
        {
          Vector3I vector3I2 = startPos + this.m_neighbours[index];
          if (!vector3ISet.Contains(vector3I2))
          {
            if (Vector3I.Min(vector3I2, this.m_storedGridMin) != this.m_storedGridMin || Vector3I.Max(vector3I2, this.m_storedGridMax) != this.m_storedGridMax)
            {
              flag1 = false;
              break;
            }
            bool flag2 = this.IsAirtightBetweenPositions(startPos, vector3I2);
            if (!flag2)
            {
              vector3ISet.Add(vector3I2);
              IMySlimBlock cubeBlock = this.CubeGrid.GetCubeBlock(vector3I2);
              if (cubeBlock != null && cubeBlock.FatBlock is Sandbox.ModAPI.IMyDoor fatBlock)
              {
                if (fatBlock.Status == DoorStatus.Open || !flag2)
                  this.m_blockQueue.Enqueue(vector3I2);
              }
              else
              {
                MyOxygenBlock myOxygenBlock = this.m_cubeRoom[vector3I2.X, vector3I2.Y, vector3I2.Z];
                if (myOxygenBlock != null && myOxygenBlock.Room != null || cubeBlock == null)
                  this.m_blockQueue.Enqueue(vector3I2);
              }
            }
          }
        }
      }
      if (flag1)
      {
        if (room1 == room)
        {
          ++this.m_lastRoomIndex;
          room = new MyOxygenRoom(this.m_lastRoomIndex);
          this.m_rooms.Add(room);
          MyOxygenRoomLink myOxygenRoomLink = new MyOxygenRoomLink(room);
          room.StartingPosition = startingPosition;
          foreach (Vector3I vector3I2 in vector3ISet)
          {
            if (this.m_cubeRoom[vector3I2.X, vector3I2.Y, vector3I2.Z].Room == room1)
            {
              this.m_cubeRoom[vector3I2.X, vector3I2.Y, vector3I2.Z].RoomLink = myOxygenRoomLink;
              ++room.BlockCount;
              --room1.BlockCount;
              room1.Blocks.Remove(vector3I2);
            }
          }
        }
        else
        {
          MyOxygenRoomLink link = room.Link;
          int blockCount = room.BlockCount;
          room.BlockCount = vector3ISet.Count;
          foreach (Vector3I key in vector3ISet)
          {
            MyOxygenBlock myOxygenBlock;
            if (!this.m_cubeRoom.TryGetValue(key, out myOxygenBlock))
            {
              myOxygenBlock = new MyOxygenBlock();
              this.m_cubeRoom.Add(key, myOxygenBlock);
            }
            myOxygenBlock.RoomLink = link;
          }
          if (blockCount > vector3ISet.Count)
          {
            HashSet<Vector3I> blocks = room.Blocks;
            blocks.ExceptWith((IEnumerable<Vector3I>) vector3ISet);
            float oxygenAmount = room.OxygenAmount / (float) blockCount * (float) blocks.Count;
            this.CreateAirtightRoom(blocks, oxygenAmount, blocks.FirstElement<Vector3I>());
            room.OxygenAmount -= oxygenAmount;
          }
        }
        room.Blocks = vector3ISet;
      }
      room.IsAirtight = flag1;
      room.IsDirty = false;
    }

    private void OnBackgroundTaskFinished()
    {
      this.m_isProcessingData = false;
      this.ScheduleUpdate();
    }

    private void StartGenerateAirtightData()
    {
      this.m_isProcessingData = true;
      if (this.Grid != null)
        this.DeSchedule();
      this.m_cubeRoom = new MyOxygenCube();
      this.m_previousGridMin = this.m_storedGridMin;
      this.m_previousGridMax = this.m_storedGridMax;
      this.m_storedGridMin = this.GridMin();
      this.m_storedGridMax = this.GridMax();
      this.m_addedBlocks.Clear();
      this.m_deletedBlocks.Clear();
      this.m_gasBlocksForUpdate.Clear();
      this.m_gasBlocksForUpdate.AddRange((IEnumerable<IMySlimBlock>) this.m_gasBlocks);
      this.m_backgroundTask = Parallel.Start(new Action(this.GenerateAirtightData), new Action(this.OnBackgroundTaskFinished));
    }

    private void GenerateAirtightData()
    {
      if (this.m_rooms == null)
      {
        this.m_rooms = new MyConcurrentList<MyOxygenRoom>();
      }
      else
      {
        this.m_lastRoomIndex = 0;
        this.m_rooms.Clear();
      }
      this.m_initializedBlocks.Clear();
      this.GenerateTopRoom();
      this.GenerateGasBlockRooms();
      this.GenerateEmptyRooms();
      if (this.m_savedRooms != null)
      {
        foreach (OxygenRoom savedRoom in this.m_savedRooms)
        {
          if (!(Vector3I.Min(savedRoom.StartingPosition, this.m_storedGridMin) != this.m_storedGridMin) && !(Vector3I.Max(savedRoom.StartingPosition, this.m_storedGridMax) != this.m_storedGridMax))
          {
            MyOxygenBlock myOxygenBlock = this.m_cubeRoom[savedRoom.StartingPosition.X, savedRoom.StartingPosition.Y, savedRoom.StartingPosition.Z];
            if (myOxygenBlock != null && myOxygenBlock.RoomLink != null && myOxygenBlock.RoomLink.Room != null)
              myOxygenBlock.RoomLink.Room.OxygenAmount = savedRoom.OxygenAmount;
          }
        }
        this.m_savedRooms = (OxygenRoom[]) null;
      }
      this.m_initializedBlocks.Clear();
      this.m_gridExpanded = false;
    }

    private void GenerateEmptyRooms()
    {
      Vector3I next = this.m_storedGridMin;
      Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref this.m_storedGridMin, ref this.m_storedGridMax);
      while (vector3IRangeIterator.IsValid())
      {
        this.CheckPositionForEmptyRoom(next);
        vector3IRangeIterator.GetNext(out next);
      }
    }

    private void CheckPositionForEmptyRoom(Vector3I position)
    {
      if (this.m_initializedBlocks.Contains(position))
        return;
      MyOxygenBlock myOxygenBlock;
      if (!this.m_cubeRoom.TryGetValue(position, out myOxygenBlock))
      {
        myOxygenBlock = new MyOxygenBlock();
        this.m_cubeRoom.Add(position, myOxygenBlock);
      }
      if (myOxygenBlock != null && myOxygenBlock.Room != null)
        return;
      Vector3I vector3I = position;
      IMySlimBlock cubeBlock = this.CubeGrid.GetCubeBlock(vector3I);
      if (cubeBlock != null)
      {
        MyCubeBlockDefinition blockDefinition = cubeBlock.BlockDefinition as MyCubeBlockDefinition;
        bool? nullable1 = this.IsAirtightFromDefinition(blockDefinition, cubeBlock.BuildLevelRatio);
        if (blockDefinition != null)
        {
          bool? nullable2 = nullable1;
          bool flag = true;
          if (nullable2.GetValueOrDefault() == flag & nullable2.HasValue)
            return;
        }
        if (cubeBlock.FatBlock is Sandbox.ModAPI.IMyDoor fatBlock && (fatBlock.Status == DoorStatus.Closed || fatBlock.Status == DoorStatus.Closing) && !(fatBlock is MyAirtightSlideDoor))
          return;
      }
      HashSet<Vector3I> roomBlocks = this.GetRoomBlocks(vector3I);
      if (roomBlocks.Count <= 0)
        return;
      this.CreateAirtightRoom(roomBlocks, 0.0f, position);
      this.m_initializedBlocks.UnionWith((IEnumerable<Vector3I>) roomBlocks);
    }

    private void GenerateGasBlockRooms()
    {
      foreach (IMySlimBlock mySlimBlock in this.m_gasBlocksForUpdate)
      {
        Vector3I position = mySlimBlock.Position;
        MyOxygenBlock myOxygenBlock = this.m_cubeRoom[position.X, position.Y, position.Z];
        if (myOxygenBlock == null || myOxygenBlock.Room == null)
        {
          HashSet<Vector3I> roomBlocks = this.GetRoomBlocks(mySlimBlock.Position);
          this.CreateAirtightRoom(roomBlocks, 0.0f, position);
          this.m_initializedBlocks.UnionWith((IEnumerable<Vector3I>) roomBlocks);
        }
      }
    }

    private MyOxygenRoom CreateAirtightRoom(
      HashSet<Vector3I> roomBlocks,
      float oxygenAmount,
      Vector3I startingPosition)
    {
      ++this.m_lastRoomIndex;
      MyOxygenRoom room = new MyOxygenRoom(this.m_lastRoomIndex);
      room.IsAirtight = true;
      room.OxygenAmount = oxygenAmount;
      room.EnvironmentOxygen = MyOxygenProviderSystem.GetOxygenInPoint(this.CubeGrid.GridIntegerToWorld(startingPosition));
      room.DepressurizationTime = this.GetTotalGamePlayTimeInMilliseconds();
      room.BlockCount = roomBlocks.Count;
      room.Blocks = roomBlocks;
      room.StartingPosition = startingPosition;
      float num = room.OxygenLevel(this.CubeGrid.GridSize);
      if ((double) room.EnvironmentOxygen > (double) num)
        room.OxygenAmount = room.MaxOxygen(this.CubeGrid.GridSize) * room.EnvironmentOxygen;
      this.m_rooms.Add(room);
      MyOxygenRoomLink roomPointer = new MyOxygenRoomLink(room);
      foreach (Vector3I roomBlock in roomBlocks)
        this.m_cubeRoom.Add(roomBlock, new MyOxygenBlock(roomPointer));
      return room;
    }

    private void GenerateTopRoom()
    {
      HashSet<Vector3I> roomBlocks = this.GetRoomBlocks(this.m_storedGridMin);
      MyOxygenRoom room = new MyOxygenRoom(0);
      room.IsAirtight = false;
      room.EnvironmentOxygen = MyOxygenProviderSystem.GetOxygenInPoint(this.CubeGrid.GridIntegerToWorld(this.m_storedGridMin));
      room.DepressurizationTime = this.GetTotalGamePlayTimeInMilliseconds();
      room.BlockCount = roomBlocks.Count;
      room.Blocks = roomBlocks;
      room.StartingPosition = this.m_storedGridMin;
      this.m_rooms.Add(room);
      MyOxygenRoomLink roomPointer = new MyOxygenRoomLink(room);
      foreach (Vector3I key in roomBlocks)
      {
        MyOxygenBlock myOxygenBlock = new MyOxygenBlock(roomPointer);
        this.m_cubeRoom.Add(key, myOxygenBlock);
        this.m_initializedBlocks.Add(key);
      }
    }

    private HashSet<Vector3I> GetRoomBlocks(
      Vector3I startPosition,
      MyOxygenRoom initRoom = null)
    {
      this.m_blockQueue.Clear();
      this.m_blockQueue.Enqueue(startPosition);
      this.m_visitedBlocks.Clear();
      this.m_visitedBlocks.Add(startPosition);
      HashSet<Vector3I> vector3ISet = new HashSet<Vector3I>();
      Vector3I key1 = startPosition;
      vector3ISet.Add(key1);
      if (initRoom != null)
      {
        MyOxygenBlock myOxygenBlock;
        if (!this.m_cubeRoom.TryGetValue(key1, out myOxygenBlock))
        {
          myOxygenBlock = new MyOxygenBlock();
          this.m_cubeRoom.Add(key1, myOxygenBlock);
        }
        myOxygenBlock.RoomLink = initRoom.Link;
      }
      while (this.m_blockQueue.Count > 0)
      {
        Vector3I startPos = this.m_blockQueue.Dequeue();
        for (int index = 0; index < this.m_neighbours.Length; ++index)
        {
          Vector3I endPos = startPos + this.m_neighbours[index];
          if (!(Vector3I.Min(endPos, this.m_storedGridMin) != this.m_storedGridMin) && !(Vector3I.Max(endPos, this.m_storedGridMax) != this.m_storedGridMax) && (!this.m_visitedBlocks.Contains(endPos) && !this.IsAirtightBetweenPositions(startPos, endPos)))
          {
            this.m_visitedBlocks.Add(endPos);
            this.m_blockQueue.Enqueue(endPos);
            Vector3I key2 = endPos;
            vector3ISet.Add(key2);
            if (initRoom != null)
            {
              MyOxygenBlock myOxygenBlock;
              if (!this.m_cubeRoom.TryGetValue(key2, out myOxygenBlock))
              {
                myOxygenBlock = new MyOxygenBlock();
                this.m_cubeRoom.Add(key2, myOxygenBlock);
              }
              myOxygenBlock.RoomLink = initRoom.Link;
            }
          }
        }
      }
      return vector3ISet;
    }

    public static MatrixD CreateAxisAlignedMatrix(ref Vector3I vec)
    {
      MatrixD zero = MatrixD.Zero;
      if (vec.X != 0)
      {
        zero.M31 = vec.X <= 0 ? (zero.M22 = -1.0) : (zero.M22 = 1.0);
        zero.M13 = 1.0;
      }
      else if (vec.Y != 0)
      {
        zero.M32 = vec.Y <= 0 ? (zero.M21 = -1.0) : (zero.M21 = 1.0);
        zero.M13 = 1.0;
      }
      else
      {
        if (vec.Z == 0)
          return MatrixD.Identity;
        zero.M33 = vec.Z <= 0 ? (zero.M21 = -1.0) : (zero.M21 = 1.0);
        zero.M12 = 1.0;
      }
      return zero;
    }

    public static void AddDepressurizationEffects(MyCubeGrid grid, Vector3I from, Vector3I to)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated || from == to)
        return;
      Vector3D world1 = grid.GridIntegerToWorld(from);
      Vector3D zero = Vector3D.Zero;
      Vector3I vec = to - from;
      MatrixD effectMatrix;
      if (vec.IsAxisAligned())
      {
        effectMatrix = MyGridGasSystem.CreateAxisAlignedMatrix(ref vec);
        effectMatrix.Translation = (Vector3D) (from * (grid.GridSizeEnum == MyCubeSize.Small ? 0.5f : 2.5f));
      }
      else
      {
        Vector3D world2 = grid.GridIntegerToWorld(to);
        effectMatrix = MatrixD.Normalize(MatrixD.CreateFromDir(world1 - world2));
        effectMatrix.Translation = world1;
        effectMatrix *= grid.PositionComp.WorldMatrixNormalizedInv;
      }
      Vector3D worldPosition = world1;
      MySlimBlock cubeBlock = grid.GetCubeBlock(from);
      if (cubeBlock != null)
      {
        worldPosition = 0.5 * (cubeBlock.CubeGrid.GridIntegerToWorld(cubeBlock.Min) + cubeBlock.CubeGrid.GridIntegerToWorld(cubeBlock.Max));
        Vector3 vector3 = cubeBlock.BlockDefinition.DepressurizationEffectOffset ?? Vector3.Zero;
        effectMatrix.Translation = !vec.IsAxisAligned() ? worldPosition + vector3 : (Vector3D) (0.5f * (cubeBlock.Min + cubeBlock.Max) * (grid.GridSizeEnum == MyCubeSize.Small ? 0.5f : 2.5f) + vector3);
      }
      MyParticleEffect effect;
      if (!MyParticlesManager.TryCreateParticleEffect("OxyLeakLarge", ref effectMatrix, ref worldPosition, grid.Render.GetRenderObjectID(), out effect))
        return;
      MyEntity3DSoundEmitter soundEmitter = MyAudioComponent.TryGetSoundEmitter();
      if (soundEmitter != null)
      {
        soundEmitter.SetPosition(new Vector3D?(world1));
        soundEmitter.PlaySound(MyGridGasSystem.m_airleakSound);
        if (grid.Physics != null)
          soundEmitter.SetVelocity(new Vector3?(grid.Physics.LinearVelocity));
      }
      if (grid.GridSizeEnum != MyCubeSize.Small)
        return;
      effect.UserScale = 0.2f;
    }

    private Vector3I GridMin() => this.CubeGrid.Min - Vector3I.One;

    private Vector3I GridMax() => this.CubeGrid.Max + Vector3I.One;

    private bool IsAirtightBetweenPositions(Vector3I startPos, Vector3I endPos)
    {
      IMySlimBlock cubeBlock1 = this.CubeGrid.GetCubeBlock(startPos);
      IMySlimBlock cubeBlock2 = this.CubeGrid.GetCubeBlock(endPos);
      if (cubeBlock1 == cubeBlock2)
      {
        if (cubeBlock1 == null)
          return false;
        MyCubeBlockDefinition blockDefinition = cubeBlock1.BlockDefinition as MyCubeBlockDefinition;
        bool? nullable1 = this.IsAirtightFromDefinition(blockDefinition, cubeBlock1.BuildLevelRatio);
        if (blockDefinition == null)
          return false;
        bool? nullable2 = nullable1;
        bool flag = true;
        return nullable2.GetValueOrDefault() == flag & nullable2.HasValue;
      }
      if (cubeBlock1 != null && this.IsAirtightBlock(cubeBlock1, startPos, (Vector3) (endPos - startPos)))
        return true;
      return cubeBlock2 != null && this.IsAirtightBlock(cubeBlock2, endPos, (Vector3) (startPos - endPos));
    }

    private bool IsAirtightBlock(IMySlimBlock block, Vector3I pos, Vector3 normal)
    {
      if (!(block.BlockDefinition is MyCubeBlockDefinition blockDefinition))
        return false;
      bool? nullable = this.IsAirtightFromDefinition(blockDefinition, block.BuildLevelRatio);
      if (nullable.HasValue)
        return nullable.Value;
      Matrix result;
      block.Orientation.GetMatrix(out result);
      result.TransposeRotationInPlace();
      Vector3I transformedNormal = Vector3I.Round(Vector3.Transform(normal, result));
      Vector3 position = Vector3.Zero;
      if (block.FatBlock != null)
        position = (Vector3) (pos - block.FatBlock.Position);
      Vector3 vector3 = Vector3.Transform(position, result) + (Vector3) blockDefinition.Center;
      switch (blockDefinition.IsCubePressurized[Vector3I.Round(vector3)][transformedNormal])
      {
        case MyCubeBlockDefinition.MyCubePressurizationMark.PressurizedAlways:
          return true;
        case MyCubeBlockDefinition.MyCubePressurizationMark.PressurizedClosed:
          if (block.FatBlock is Sandbox.ModAPI.IMyDoor fatBlock && (fatBlock.Status == DoorStatus.Closed || fatBlock.Status == DoorStatus.Closing))
            return true;
          break;
      }
      return block.FatBlock is Sandbox.ModAPI.IMyDoor fatBlock && (fatBlock.Status == DoorStatus.Closed || fatBlock.Status == DoorStatus.Closing) && this.IsDoorAirtight(fatBlock, ref transformedNormal, blockDefinition);
    }

    private bool? IsAirtightFromDefinition(
      MyCubeBlockDefinition blockDefinition,
      float buildLevelRatio)
    {
      if (blockDefinition.BuildProgressModels != null && blockDefinition.BuildProgressModels.Length != 0)
      {
        MyCubeBlockDefinition.BuildProgressModel buildProgressModel = blockDefinition.BuildProgressModels[blockDefinition.BuildProgressModels.Length - 1];
        if ((double) buildLevelRatio < (double) buildProgressModel.BuildRatioUpperBound)
          return new bool?(false);
      }
      return blockDefinition.IsAirTight;
    }

    private bool IsDoorAirtight(
      Sandbox.ModAPI.IMyDoor doorBlock,
      ref Vector3I transformedNormal,
      MyCubeBlockDefinition blockDefinition)
    {
      switch (doorBlock)
      {
        case MyAdvancedDoor _:
          if (doorBlock.IsFullyClosed)
          {
            foreach (MyCubeBlockDefinition.MountPoint mountPoint in blockDefinition.MountPoints)
            {
              if (transformedNormal == mountPoint.Normal)
                return false;
            }
            return true;
          }
          break;
        case MyAirtightSlideDoor _:
          if (doorBlock.IsFullyClosed && transformedNormal == Vector3I.Forward)
            return true;
          break;
        case MyAirtightDoorGeneric _:
          if (doorBlock.IsFullyClosed && (transformedNormal == Vector3I.Forward || transformedNormal == Vector3I.Backward))
            return true;
          break;
        default:
          if (doorBlock.IsFullyClosed)
          {
            foreach (MyCubeBlockDefinition.MountPoint mountPoint in blockDefinition.MountPoints)
            {
              if (transformedNormal == mountPoint.Normal)
                return false;
            }
            return true;
          }
          break;
      }
      return false;
    }

    private void StartRemoveBlocks()
    {
      if (this.m_isProcessingData)
        return;
      if (this.m_gasBlocks.Count == 0)
        this.Clear();
      if (this.m_rooms == null)
      {
        this.m_deletedBlocks.Clear();
      }
      else
      {
        this.m_isProcessingData = true;
        List<IMySlimBlock> deletedBlocksSwap = this.m_deletedBlocksSwap;
        this.m_deletedBlocksSwap = this.m_deletedBlocks;
        this.m_deletedBlocks = deletedBlocksSwap;
        this.m_backgroundTask = Parallel.Start(new Action(this.RemoveBlocks), new Action(this.OnBackgroundTaskFinished));
      }
    }

    private void RemoveBlocks()
    {
      bool flag = false;
      Vector3I vector3I1 = Vector3I.Zero;
      Vector3I vector3I2 = Vector3I.Zero;
      foreach (IMySlimBlock mySlimBlock in this.m_deletedBlocksSwap)
      {
        Vector3I next = mySlimBlock.Min;
        Vector3I min = mySlimBlock.Min;
        Vector3I max = mySlimBlock.Max;
        Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref min, ref max);
        while (vector3IRangeIterator.IsValid())
        {
          Vector3I depressFrom;
          Vector3I depressTo;
          if (this.RemoveBlock(next, out depressFrom, out depressTo))
          {
            flag = true;
            vector3I1 = depressFrom;
            vector3I2 = depressTo;
          }
          vector3IRangeIterator.GetNext(out next);
        }
      }
      if (flag)
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseStaticEvent<long, Vector3I, Vector3I>((Func<IMyEventOwner, Action<long, Vector3I, Vector3I>>) (x => new Action<long, Vector3I, Vector3I>(MyCubeGrid.DepressurizeEffect)), ((IMyEntity) this.CubeGrid).EntityId, vector3I1, vector3I2);
      this.m_deletedBlocksSwap.Clear();
    }

    private bool RemoveBlock(
      Vector3I deletedBlockPosition,
      out Vector3I depressFrom,
      out Vector3I depressTo)
    {
      bool flag = false;
      depressFrom = Vector3I.Zero;
      depressTo = Vector3I.Zero;
      Vector3I current = deletedBlockPosition;
      MyOxygenRoom room = this.m_rooms[0];
      MyOxygenRoom myOxygenRoom = this.GetMaxBlockRoom(ref current, room);
      if (myOxygenRoom == null)
        return flag;
      for (int index = 0; index < this.m_neighboursForDelete.Length; ++index)
      {
        Vector3I gridPosition = current + this.m_neighboursForDelete[index];
        if (this.IsInBounds(gridPosition))
        {
          MyOxygenRoom cubeGridPosition = this.GetOxygenRoomForCubeGridPosition(ref gridPosition);
          if (cubeGridPosition != null && cubeGridPosition != myOxygenRoom && (!(current != gridPosition) || !this.IsAirtightBetweenPositions(current, gridPosition)))
          {
            if (myOxygenRoom.IsAirtight && !cubeGridPosition.IsAirtight)
            {
              cubeGridPosition.BlockCount += myOxygenRoom.BlockCount;
              cubeGridPosition.OxygenAmount += myOxygenRoom.OxygenAmount;
              this.MergeRooms(cubeGridPosition, myOxygenRoom, cubeGridPosition.Link);
              if (myOxygenRoom.Blocks != null && cubeGridPosition.Blocks != null)
                cubeGridPosition.Blocks.UnionWith((IEnumerable<Vector3I>) myOxygenRoom.Blocks);
              if ((double) myOxygenRoom.OxygenLevel(this.CubeGrid.GridSize) - (double) cubeGridPosition.EnvironmentOxygen > 0.200000002980232)
              {
                flag = true;
                depressFrom = current;
                depressTo = gridPosition;
              }
              myOxygenRoom.IsAirtight = false;
              myOxygenRoom.OxygenAmount = 0.0f;
              myOxygenRoom.EnvironmentOxygen = Math.Max(myOxygenRoom.EnvironmentOxygen, cubeGridPosition.EnvironmentOxygen);
              myOxygenRoom.DepressurizationTime = this.GetTotalGamePlayTimeInMilliseconds();
              myOxygenRoom.Link.Room = cubeGridPosition;
              if (cubeGridPosition != myOxygenRoom && myOxygenRoom != room)
              {
                myOxygenRoom.BlockCount = 0;
                myOxygenRoom.Blocks = (HashSet<Vector3I>) null;
                this.m_rooms.Remove(myOxygenRoom);
              }
              myOxygenRoom = cubeGridPosition;
            }
            else if (!myOxygenRoom.IsAirtight && cubeGridPosition.IsAirtight)
            {
              myOxygenRoom.BlockCount += cubeGridPosition.BlockCount;
              myOxygenRoom.OxygenAmount += cubeGridPosition.OxygenAmount;
              this.MergeRooms(myOxygenRoom, cubeGridPosition, myOxygenRoom.Link);
              myOxygenRoom.EnvironmentOxygen = Math.Max(myOxygenRoom.EnvironmentOxygen, cubeGridPosition.EnvironmentOxygen);
              if ((double) cubeGridPosition.OxygenLevel(this.CubeGrid.GridSize) - (double) myOxygenRoom.EnvironmentOxygen > 0.200000002980232)
              {
                flag = true;
                depressFrom = current;
                depressTo = gridPosition;
              }
              cubeGridPosition.IsAirtight = false;
              cubeGridPosition.OxygenAmount = 0.0f;
              cubeGridPosition.EnvironmentOxygen = Math.Max(myOxygenRoom.EnvironmentOxygen, cubeGridPosition.EnvironmentOxygen);
              cubeGridPosition.DepressurizationTime = this.GetTotalGamePlayTimeInMilliseconds();
              cubeGridPosition.Link.Room = myOxygenRoom;
              if (cubeGridPosition != myOxygenRoom && cubeGridPosition != room)
              {
                cubeGridPosition.BlockCount = 0;
                cubeGridPosition.Blocks = (HashSet<Vector3I>) null;
                this.m_rooms.Remove(cubeGridPosition);
              }
            }
            else
            {
              myOxygenRoom.BlockCount += cubeGridPosition.BlockCount;
              myOxygenRoom.OxygenAmount += cubeGridPosition.OxygenAmount;
              this.MergeRooms(myOxygenRoom, cubeGridPosition, myOxygenRoom.Link);
              cubeGridPosition.Link.Room = myOxygenRoom;
              if (cubeGridPosition != myOxygenRoom && cubeGridPosition != room)
              {
                cubeGridPosition.BlockCount = 0;
                cubeGridPosition.Blocks = (HashSet<Vector3I>) null;
                this.m_rooms.Remove(cubeGridPosition);
              }
            }
          }
        }
      }
      Vector3I key = current;
      MyOxygenBlock myOxygenBlock = this.m_cubeRoom[key.X, key.Y, key.Z];
      if (myOxygenBlock == null)
      {
        myOxygenBlock = new MyOxygenBlock();
        this.m_cubeRoom.Add(key, myOxygenBlock);
      }
      if (myOxygenBlock.Room == null)
      {
        myOxygenBlock.RoomLink = myOxygenRoom.Link;
        ++myOxygenRoom.BlockCount;
        myOxygenRoom.Blocks.Add(key);
      }
      return flag;
    }

    private void MergeRooms(MyOxygenRoom target, MyOxygenRoom withRoom, MyOxygenRoomLink link)
    {
      if (target.Blocks == null || withRoom.Blocks == null)
        return;
      target.Blocks.UnionWith((IEnumerable<Vector3I>) withRoom.Blocks);
      foreach (Vector3I block in withRoom.Blocks)
        this.m_cubeRoom[block.X, block.Y, block.Z].RoomLink = link;
    }

    private MyOxygenRoom GetMaxBlockRoom(ref Vector3I current, MyOxygenRoom topRoom)
    {
      MyOxygenRoom myOxygenRoom = this.GetOxygenRoomForCubeGridPosition(ref current);
      for (int index = 0; index < this.m_neighbours.Length; ++index)
      {
        Vector3I gridPosition = current + this.m_neighbours[index];
        if (this.IsInBounds(current) && this.IsInBounds(gridPosition) && !this.IsAirtightBetweenPositions(current, gridPosition))
        {
          MyOxygenRoom cubeGridPosition = this.GetOxygenRoomForCubeGridPosition(ref gridPosition);
          if (cubeGridPosition != null)
          {
            if (myOxygenRoom == null)
              myOxygenRoom = cubeGridPosition;
            else if (cubeGridPosition == topRoom)
              myOxygenRoom = topRoom;
            else if (myOxygenRoom.BlockCount < cubeGridPosition.BlockCount && myOxygenRoom != topRoom)
              myOxygenRoom = cubeGridPosition;
          }
        }
      }
      return myOxygenRoom;
    }

    private bool IsInBounds(Vector3I pos) => !(this.m_storedGridMin != Vector3I.Min(pos, this.m_storedGridMin)) && !(this.m_storedGridMax != Vector3I.Max(pos, this.m_storedGridMax));

    public MyOxygenRoom GetOxygenRoomForCubeGridPosition(ref Vector3I gridPosition)
    {
      Vector3I pos = gridPosition;
      if (!this.IsInBounds(pos))
        return (MyOxygenRoom) null;
      if (this.m_cubeRoom != null)
      {
        MyOxygenBlock myOxygenBlock = this.m_cubeRoom[pos.X, pos.Y, pos.Z];
        if (myOxygenBlock != null)
          return myOxygenBlock.Room;
      }
      return (MyOxygenRoom) null;
    }

    public MyOxygenBlock GetOxygenBlock(Vector3D worldPosition)
    {
      Vector3I gridInteger = this.CubeGrid.WorldToGridInteger(worldPosition);
      return this.m_cubeRoom != null && this.IsInBounds(gridInteger) ? this.m_cubeRoom[gridInteger.X, gridInteger.Y, gridInteger.Z] : new MyOxygenBlock();
    }

    public MyOxygenBlock GetSafeOxygenBlock(Vector3D position)
    {
      MyOxygenBlock oxygenBlock1 = this.GetOxygenBlock(position);
      if (oxygenBlock1 == null || oxygenBlock1.Room == null)
      {
        Vector3D vector3D1 = Vector3D.Transform(position, this.CubeGrid.PositionComp.WorldMatrixNormalizedInv) / (double) this.CubeGrid.GridSize;
        List<Vector3D> vector3DList = new List<Vector3D>(3);
        if (vector3D1.X - Math.Floor(vector3D1.X) > 0.5)
          vector3DList.Add(new Vector3D(-1.0, 0.0, 0.0));
        else
          vector3DList.Add(new Vector3D(1.0, 0.0, 0.0));
        if (vector3D1.Y - Math.Floor(vector3D1.Y) > 0.5)
          vector3DList.Add(new Vector3D(0.0, -1.0, 0.0));
        else
          vector3DList.Add(new Vector3D(0.0, 1.0, 0.0));
        if (vector3D1.Z - Math.Floor(vector3D1.Z) > 0.5)
          vector3DList.Add(new Vector3D(0.0, 0.0, -1.0));
        else
          vector3DList.Add(new Vector3D(0.0, 0.0, 1.0));
        foreach (Vector3D vector3D2 in vector3DList)
        {
          MyOxygenBlock oxygenBlock2 = this.GetOxygenBlock(Vector3D.Transform((vector3D1 + vector3D2) * (double) this.CubeGrid.GridSize, this.CubeGrid.PositionComp.WorldMatrixRef));
          if (oxygenBlock2 != null && oxygenBlock2.Room != null && oxygenBlock2.Room.IsAirtight)
            return oxygenBlock2;
        }
      }
      return oxygenBlock1;
    }

    public void DebugDraw()
    {
      if (this.m_isProcessingData || this.m_rooms == null)
        return;
      Vector2 zero = Vector2.Zero;
      MyRenderProxy.DebugDrawText2D(zero, "CTRL+ (T Toggle Top Room) (R Toggle Room Index) (Y Toggle Positions) (U Toggle View) ([ Index Down) (] Index Up) (- Index Reset) (+ Index Last)", Color.Yellow, 0.6f);
      zero.Y += this.m_debugTextlineSize;
      MyRenderProxy.DebugDrawText2D(zero, "Rooms Count: " + (object) this.m_rooms.Count, Color.Yellow, 0.6f);
      zero.Y += this.m_debugTextlineSize;
      MyRenderProxy.DebugDrawText2D(zero, "Selected Room", Color.Yellow, 0.6f);
      zero.Y += this.m_debugTextlineSize;
      MyRenderProxy.DebugDrawText2D(zero, "   Index: " + (object) this.m_debugRoomIndex, Color.Yellow, 0.6f);
      if (MyInput.Static.IsAnyCtrlKeyPressed())
      {
        if (MyInput.Static.IsNewKeyPressed(MyKeys.T))
          this.m_debugShowTopRoom = !this.m_debugShowTopRoom;
        if (MyInput.Static.IsNewKeyPressed(MyKeys.R))
          this.m_debugShowRoomIndex = !this.m_debugShowRoomIndex;
        if (MyInput.Static.IsNewKeyPressed(MyKeys.Y))
          this.m_debugShowPositions = !this.m_debugShowPositions;
        if (MyInput.Static.IsNewKeyPressed(MyKeys.OemOpenBrackets))
          this.m_debugRoomIndex = this.m_debugRoomIndex == 0 ? 0 : this.m_debugRoomIndex - 1;
        if (MyInput.Static.IsNewKeyPressed(MyKeys.OemCloseBrackets))
          this.m_debugRoomIndex = this.m_debugRoomIndex >= this.m_lastRoomIndex ? this.m_lastRoomIndex : this.m_debugRoomIndex + 1;
        if (MyInput.Static.IsNewKeyPressed(MyKeys.OemPlus))
          this.m_debugRoomIndex = this.m_lastRoomIndex;
        if (MyInput.Static.IsNewKeyPressed(MyKeys.OemMinus))
          this.m_debugRoomIndex = 0;
        if (MyInput.Static.IsNewKeyPressed(MyKeys.U))
          this.m_debugToggleView = !this.m_debugToggleView;
      }
      if (this.m_debugToggleView)
      {
        Vector3I next = this.m_storedGridMin;
        Vector3I_RangeIterator vector3IRangeIterator = new Vector3I_RangeIterator(ref this.m_storedGridMin, ref this.m_storedGridMax);
        while (vector3IRangeIterator.IsValid())
        {
          MyOxygenBlock myOxygenBlock;
          if (this.m_cubeRoom.TryGetValue(next, out myOxygenBlock))
          {
            MyOxygenRoom room = myOxygenBlock.Room;
            if (room != null && (room.Index != 0 || this.m_debugShowTopRoom))
              this.DrawBlock(room, next);
          }
          vector3IRangeIterator.GetNext(out next);
        }
      }
      else
        this.DrawRooms(zero);
    }

    private void DrawRooms(Vector2 textPosition)
    {
      foreach (MyOxygenRoom room in this.m_rooms)
      {
        this.DrawRoomInfo(textPosition, room);
        foreach (Vector3I block in room.Blocks)
        {
          if ((room.Index != 0 || this.m_debugShowTopRoom) && (this.m_debugRoomIndex == 0 || room.Index == this.m_debugRoomIndex))
            this.DrawBlock(room, block);
        }
      }
    }

    private void DrawBlock(MyOxygenRoom room, Vector3I blockPosition)
    {
      Color color = room.IsAirtight ? Color.Lerp(Color.Red, Color.Green, room.OxygenLevel(this.CubeGrid.GridSize)) : Color.Blue;
      Vector3D world = this.CubeGrid.GridIntegerToWorld(blockPosition);
      MyRenderProxy.DebugDrawPoint(world, color, false);
      if (this.m_debugShowRoomIndex)
        MyRenderProxy.DebugDrawText3D(world, room.Index.ToString(), Color.LightGray, 0.5f, false);
      if (!this.m_debugShowPositions)
        return;
      string text = string.Format("{0}, {1}, {2}", (object) blockPosition.X, (object) blockPosition.Y, (object) blockPosition.Z);
      MyRenderProxy.DebugDrawText3D(world, text, Color.LightGray, 0.5f, false);
    }

    private void DrawRoomInfo(Vector2 textPosition, MyOxygenRoom room)
    {
      if (room.Index != this.m_debugRoomIndex)
        return;
      string str = string.Format("{0} : {1}", (object) room.BlockCount, (object) room.Blocks.Count);
      textPosition.Y += this.m_debugTextlineSize;
      MyRenderProxy.DebugDrawText2D(textPosition, "   Block Count: " + str, Color.Yellow, 0.6f);
      textPosition.Y += this.m_debugTextlineSize;
      MyRenderProxy.DebugDrawText2D(textPosition, "   Oxygen Amount: " + (object) room.OxygenAmount, Color.Yellow, 0.6f);
      textPosition.Y += this.m_debugTextlineSize;
      MyRenderProxy.DebugDrawText2D(textPosition, "   Min: " + (object) this.m_storedGridMin, Color.Yellow, 0.6f);
      textPosition.Y += this.m_debugTextlineSize;
      MyRenderProxy.DebugDrawText2D(textPosition, "   Max: " + (object) this.m_storedGridMax, Color.Yellow, 0.6f);
    }

    internal OxygenRoom[] GetOxygenAmount()
    {
      if (this.m_rooms == null || this.m_rooms.List == null)
        return (OxygenRoom[]) null;
      int count = this.m_rooms.List.Count;
      List<MyOxygenRoom> list = this.m_rooms.List;
      OxygenRoom[] oxygenRoomArray = new OxygenRoom[count];
      for (int index = 0; index < count; ++index)
      {
        MyOxygenRoom myOxygenRoom = list[index];
        if (myOxygenRoom != null)
        {
          oxygenRoomArray[index].OxygenAmount = myOxygenRoom.OxygenAmount;
          oxygenRoomArray[index].StartingPosition = myOxygenRoom.StartingPosition;
        }
      }
      return oxygenRoomArray;
    }

    internal void Init(OxygenRoom[] oxygenAmount) => this.m_savedRooms = oxygenAmount;

    public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.OnceBeforeSimulation;

    public override int UpdatePriority => 12;
  }
}
