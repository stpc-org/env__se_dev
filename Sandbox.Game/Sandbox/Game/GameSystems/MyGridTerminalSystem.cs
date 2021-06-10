// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridTerminalSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
  public class MyGridTerminalSystem : Sandbox.ModAPI.IMyGridTerminalSystem, Sandbox.ModAPI.Ingame.IMyGridTerminalSystem
  {
    private readonly MyGridLogicalGroupData m_gridLogicalGroupData;
    private readonly int m_oreDetectorCounterValue = 50;
    private readonly HashSet<MyTerminalBlock> m_blocks = new HashSet<MyTerminalBlock>();
    private readonly List<MyTerminalBlock> m_blockList = new List<MyTerminalBlock>();
    private readonly Dictionary<long, MyTerminalBlock> m_blockTable = new Dictionary<long, MyTerminalBlock>();
    private readonly List<MyBlockGroup> m_blockGroups = new List<MyBlockGroup>();
    private readonly HashSet<MyTerminalBlock> m_blocksForHud = new HashSet<MyTerminalBlock>();
    private List<string> m_debugChanges = new List<string>();
    private int m_lastHudIndex;
    private int m_oreDetectorUpdateCounter;
    private bool m_needsHudUpdate;
    private bool m_scheduled;

    public event Action<MyTerminalBlock> BlockAdded;

    public event Action<MyTerminalBlock> BlockRemoved;

    public event Action BlockManipulationFinished;

    public event Action<MyBlockGroup> GroupAdded;

    public event Action<MyBlockGroup> GroupRemoved;

    public MyGridTerminalSystem(MyGridLogicalGroupData gridLogicalGroupData) => this.m_gridLogicalGroupData = gridLogicalGroupData;

    public void Schedule(MyCubeGrid root = null)
    {
      if (this.m_scheduled)
        return;
      root = root ?? this.m_gridLogicalGroupData.Root;
      if (root == null)
        return;
      root.Schedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.UpdateHud), 8);
      this.m_scheduled = true;
    }

    protected void DeSchedule(MyCubeGrid root = null)
    {
      root = root ?? this.m_gridLogicalGroupData.Root;
      root?.DeSchedule(MyCubeGrid.UpdateQueue.BeforeSimulation, new Action(this.UpdateHud));
      this.m_scheduled = false;
    }

    public void OnRootChanged(MyCubeGrid oldRoot, MyCubeGrid newRoot)
    {
      if (!this.m_scheduled)
        return;
      this.DeSchedule(oldRoot);
      if (newRoot == null)
        return;
      this.Schedule(newRoot);
    }

    public bool NeedsHudUpdate
    {
      get => this.m_needsHudUpdate;
      set
      {
        if (this.m_needsHudUpdate == value)
          return;
        this.m_needsHudUpdate = value;
        if (value)
          this.Schedule();
        else
          this.DeSchedule();
      }
    }

    public HashSetReader<MyTerminalBlock> Blocks => new HashSetReader<MyTerminalBlock>(this.m_blocks);

    public HashSetReader<MyTerminalBlock> HudBlocks => new HashSetReader<MyTerminalBlock>(this.m_blocksForHud);

    public List<MyBlockGroup> BlockGroups => this.m_blockGroups;

    public void Add(MyTerminalBlock block)
    {
      if (block.MarkedForClose || block.IsBeingRemoved || (Sandbox.Game.Entities.MyEntities.IsClosingAll || this.m_blockTable.ContainsKey(block.EntityId)))
        return;
      this.m_blockTable.Add(block.EntityId, block);
      this.m_blocks.Add(block);
      this.m_blockList.Add(block);
      Action<MyTerminalBlock> blockAdded = this.BlockAdded;
      if (blockAdded == null)
        return;
      blockAdded(block);
    }

    public void Remove(MyTerminalBlock block)
    {
      if (block.MarkedForClose || Sandbox.Game.Entities.MyEntities.IsClosingAll)
        return;
      this.m_blockTable.Remove(block.EntityId);
      this.m_blocks.Remove(block);
      this.m_blockList.Remove(block);
      this.m_blocksForHud.Remove(block);
      for (int index = 0; index < this.BlockGroups.Count; ++index)
      {
        MyBlockGroup blockGroup = this.BlockGroups[index];
        blockGroup.Blocks.Remove(block);
        if (blockGroup.Blocks.Count == 0)
        {
          this.RemoveGroup(blockGroup, !block.IsBeingRemoved);
          --index;
        }
      }
      Action<MyTerminalBlock> blockRemoved = this.BlockRemoved;
      if (blockRemoved == null)
        return;
      blockRemoved(block);
    }

    public MyBlockGroup AddUpdateGroup(
      MyBlockGroup gridGroup,
      bool fireEvent,
      bool modify = false)
    {
      if (gridGroup.Blocks.Count == 0)
        return (MyBlockGroup) null;
      MyBlockGroup myBlockGroup = this.BlockGroups.Find((Predicate<MyBlockGroup>) (x => x.Name.CompareTo(gridGroup.Name) == 0));
      if (myBlockGroup == null)
      {
        myBlockGroup = new MyBlockGroup();
        myBlockGroup.Name.Clear().AppendStringBuilder(gridGroup.Name);
        this.BlockGroups.Add(myBlockGroup);
      }
      if (modify)
        myBlockGroup.Blocks.Clear();
      myBlockGroup.Blocks.UnionWith((IEnumerable<MyTerminalBlock>) gridGroup.Blocks);
      if (fireEvent && this.GroupAdded != null)
        this.GroupAdded(gridGroup);
      return gridGroup;
    }

    public void RemoveGroup(MyBlockGroup gridGroup, bool fireEvent)
    {
      MyBlockGroup myBlockGroup = this.BlockGroups.Find((Predicate<MyBlockGroup>) (x => x.Name.CompareTo(gridGroup.Name) == 0));
      if (myBlockGroup != null)
      {
        List<MyTerminalBlock> myTerminalBlockList = new List<MyTerminalBlock>();
        foreach (MyTerminalBlock block in gridGroup.Blocks)
        {
          if (myBlockGroup.Blocks.Contains(block))
            myTerminalBlockList.Add(block);
        }
        foreach (MyTerminalBlock myTerminalBlock in myTerminalBlockList)
          myBlockGroup.Blocks.Remove(myTerminalBlock);
        if (myBlockGroup.Blocks.Count == 0)
          this.BlockGroups.Remove(myBlockGroup);
      }
      if (!fireEvent || this.GroupRemoved == null)
        return;
      this.GroupRemoved(gridGroup);
    }

    public void CopyBlocksTo(List<MyTerminalBlock> result)
    {
      foreach (MyTerminalBlock block in this.m_blocks)
        result.Add(block);
    }

    public void UpdateGridBlocksOwnership(long ownerID)
    {
      foreach (MyTerminalBlock block in this.m_blocks)
        block.IsAccessibleForProgrammableBlock = block.HasPlayerAccess(ownerID);
    }

    public void UpdateHud()
    {
      if (!this.NeedsHudUpdate)
        return;
      if (this.m_lastHudIndex < this.m_blocks.Count)
      {
        MyTerminalBlock block = this.m_blockList[this.m_lastHudIndex];
        if (this.MeetsHudConditions(block))
          this.m_blocksForHud.Add(block);
        else
          this.m_blocksForHud.Remove(block);
        ++this.m_lastHudIndex;
      }
      else
      {
        this.m_lastHudIndex = 0;
        this.NeedsHudUpdate = false;
      }
    }

    private bool MeetsHudConditions(MyTerminalBlock terminalBlock)
    {
      if (terminalBlock is MyRadioAntenna)
        return false;
      if (terminalBlock.HasLocalPlayerAccess() && (terminalBlock.ShowOnHUD || terminalBlock.IsBeingHacked && terminalBlock.IDModule != null && terminalBlock.IDModule.Owner != 0L || terminalBlock is MyCockpit && (terminalBlock as MyCockpit).Pilot != null))
        return true;
      if (terminalBlock.HasLocalPlayerAccess() && terminalBlock.IDModule != null && terminalBlock.IDModule.Owner != 0L)
      {
        IMyComponentOwner<MyOreDetectorComponent> myComponentOwner = terminalBlock as IMyComponentOwner<MyOreDetectorComponent>;
      }
      return false;
    }

    internal void BlockManipulationFinishedFunction()
    {
      Action manipulationFinished = this.BlockManipulationFinished;
      if (manipulationFinished == null)
        return;
      manipulationFinished();
    }

    [Conditional("DEBUG")]
    private void RecordChange(string text)
    {
      this.m_debugChanges.Add(DateTime.Now.ToLongTimeString() + ": " + text);
      if (this.m_debugChanges.Count <= 10)
        return;
      this.m_debugChanges.RemoveAt(0);
    }

    public void DebugDraw(MyEntity entity)
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_BLOCK_GROUPS)
        return;
      double num1 = 6.5;
      double num2 = num1 * 0.045;
      Vector3D worldCoord = entity.WorldMatrix.Translation;
      if (entity is MyCubeGrid myCubeGrid)
      {
        myCubeGrid.GetPhysicalGroupAABB();
        worldCoord = myCubeGrid.GetPhysicalGroupAABB().Center;
        if (myCubeGrid.GridSizeEnum == MyCubeSize.Large)
          worldCoord -= new Vector3D(0.0, 5.0, 0.0);
      }
      Vector3D position = MySector.MainCamera.Position;
      Vector3D up = MySector.MainCamera.WorldMatrix.Up;
      Vector3D right = MySector.MainCamera.WorldMatrix.Right;
      float scale = (float) Math.Atan(num1 / Math.Max(Vector3D.Distance(worldCoord, position), 0.001));
      if ((double) scale <= 0.270000010728836)
        return;
      MyRenderProxy.DebugDrawText3D(worldCoord, entity.ToString(), Color.Yellow, scale, true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      int num3 = -1;
      MyRenderProxy.DebugDrawText3D(worldCoord + (double) num3 * up * num2 + right * 0.0649999976158142, string.Format("Blocks: {0}", (object) this.m_blocks.Count), Color.LightYellow, scale, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      int num4 = num3 - 1;
      MyRenderProxy.DebugDrawText3D(worldCoord + (double) num4 * up * num2 + right * 0.0649999976158142, string.Format("Groups: {0}", (object) this.m_blockGroups.Count), Color.LightYellow, scale, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      int num5 = num4 - 1;
      MyRenderProxy.DebugDrawText3D(worldCoord + (double) num5 * up * num2 + right * 0.0649999976158142, "Recent group changes:", Color.LightYellow, scale, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      int num6 = num5 - 1;
      foreach (string debugChange in this.m_debugChanges)
      {
        MyRenderProxy.DebugDrawText3D(worldCoord + (double) num6 * up * num2 + right * 0.0649999976158142, debugChange, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
        --num6;
      }
    }

    void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocks(
      List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks)
    {
      blocks.Clear();
      foreach (MyTerminalBlock block in this.m_blocks)
      {
        if (block.IsAccessibleForProgrammableBlock)
          blocks.Add((Sandbox.ModAPI.Ingame.IMyTerminalBlock) block);
      }
    }

    void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockGroups(
      List<Sandbox.ModAPI.Ingame.IMyBlockGroup> blockGroups,
      Func<Sandbox.ModAPI.Ingame.IMyBlockGroup, bool> collect)
    {
      blockGroups?.Clear();
      for (int index = 0; index < this.BlockGroups.Count; ++index)
      {
        MyBlockGroup blockGroup = this.BlockGroups[index];
        if ((collect == null || collect((Sandbox.ModAPI.Ingame.IMyBlockGroup) blockGroup)) && blockGroups != null)
          blockGroups.Add((Sandbox.ModAPI.Ingame.IMyBlockGroup) blockGroup);
      }
    }

    void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocksOfType<T>(
      List<T> blocks,
      Func<T, bool> collect)
    {
      blocks?.Clear();
      foreach (MyTerminalBlock block in this.m_blocks)
      {
        if (block is T obj && block.IsAccessibleForProgrammableBlock && (collect == null || collect(obj)) && blocks != null)
          blocks.Add(obj);
      }
    }

    void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlocksOfType<T>(
      List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks,
      Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
    {
      blocks?.Clear();
      foreach (MyTerminalBlock block in this.m_blocks)
      {
        if ((object) (block as T) != null && block.IsAccessibleForProgrammableBlock && (collect == null || collect((Sandbox.ModAPI.Ingame.IMyTerminalBlock) block)) && blocks != null)
          blocks.Add((Sandbox.ModAPI.Ingame.IMyTerminalBlock) block);
      }
    }

    void Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.SearchBlocksOfName(
      string name,
      List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks,
      Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
    {
      blocks?.Clear();
      foreach (MyTerminalBlock block in this.m_blocks)
      {
        if (block.CustomName.ToString().Contains(name, StringComparison.OrdinalIgnoreCase) && block.IsAccessibleForProgrammableBlock && (collect == null || collect((Sandbox.ModAPI.Ingame.IMyTerminalBlock) block)) && blocks != null)
          blocks.Add((Sandbox.ModAPI.Ingame.IMyTerminalBlock) block);
      }
    }

    Sandbox.ModAPI.Ingame.IMyTerminalBlock Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockWithName(
      string name)
    {
      foreach (MyTerminalBlock block in this.m_blocks)
      {
        if (block.CustomName.CompareTo(name) == 0 && block.IsAccessibleForProgrammableBlock)
          return (Sandbox.ModAPI.Ingame.IMyTerminalBlock) block;
      }
      return (Sandbox.ModAPI.Ingame.IMyTerminalBlock) null;
    }

    Sandbox.ModAPI.Ingame.IMyBlockGroup Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockGroupWithName(
      string name)
    {
      for (int index = 0; index < this.BlockGroups.Count; ++index)
      {
        MyBlockGroup blockGroup = this.BlockGroups[index];
        if (blockGroup.Name.CompareTo(name) == 0)
          return (Sandbox.ModAPI.Ingame.IMyBlockGroup) blockGroup;
      }
      return (Sandbox.ModAPI.Ingame.IMyBlockGroup) null;
    }

    Sandbox.ModAPI.Ingame.IMyTerminalBlock Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.GetBlockWithId(
      long id)
    {
      MyTerminalBlock myTerminalBlock;
      return this.m_blockTable.TryGetValue(id, out myTerminalBlock) && myTerminalBlock.IsAccessibleForProgrammableBlock ? (Sandbox.ModAPI.Ingame.IMyTerminalBlock) myTerminalBlock : (Sandbox.ModAPI.Ingame.IMyTerminalBlock) null;
    }

    void Sandbox.ModAPI.IMyGridTerminalSystem.GetBlocks(
      List<Sandbox.ModAPI.IMyTerminalBlock> blocks)
    {
      blocks.Clear();
      foreach (MyTerminalBlock block in this.m_blocks)
        blocks.Add((Sandbox.ModAPI.IMyTerminalBlock) block);
    }

    void Sandbox.ModAPI.IMyGridTerminalSystem.GetBlockGroups(
      List<Sandbox.ModAPI.IMyBlockGroup> blockGroups)
    {
      blockGroups.Clear();
      foreach (MyBlockGroup blockGroup in this.BlockGroups)
        blockGroups.Add((Sandbox.ModAPI.IMyBlockGroup) blockGroup);
    }

    void Sandbox.ModAPI.IMyGridTerminalSystem.GetBlocksOfType<T>(
      List<Sandbox.ModAPI.IMyTerminalBlock> blocks,
      Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect)
    {
      blocks.Clear();
      foreach (MyTerminalBlock block in this.m_blocks)
      {
        if (block is T && (collect == null || collect((Sandbox.ModAPI.IMyTerminalBlock) block)))
          blocks.Add((Sandbox.ModAPI.IMyTerminalBlock) block);
      }
    }

    void Sandbox.ModAPI.IMyGridTerminalSystem.SearchBlocksOfName(
      string name,
      List<Sandbox.ModAPI.IMyTerminalBlock> blocks,
      Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect)
    {
      blocks.Clear();
      foreach (MyTerminalBlock block in this.m_blocks)
      {
        if (block.CustomName.ToString().Contains(name, StringComparison.OrdinalIgnoreCase) && (collect == null || collect((Sandbox.ModAPI.IMyTerminalBlock) block)))
          blocks.Add((Sandbox.ModAPI.IMyTerminalBlock) block);
      }
    }

    Sandbox.ModAPI.IMyTerminalBlock Sandbox.ModAPI.IMyGridTerminalSystem.GetBlockWithName(
      string name)
    {
      foreach (MyTerminalBlock block in this.m_blocks)
      {
        if (block.CustomName.ToString() == name)
          return (Sandbox.ModAPI.IMyTerminalBlock) block;
      }
      return (Sandbox.ModAPI.IMyTerminalBlock) null;
    }

    Sandbox.ModAPI.IMyBlockGroup Sandbox.ModAPI.IMyGridTerminalSystem.GetBlockGroupWithName(
      string name)
    {
      foreach (MyBlockGroup blockGroup in this.BlockGroups)
      {
        if (blockGroup.Name.ToString() == name)
          return (Sandbox.ModAPI.IMyBlockGroup) blockGroup;
      }
      return (Sandbox.ModAPI.IMyBlockGroup) null;
    }
  }
}
