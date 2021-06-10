// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyBlockGroup
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  public class MyBlockGroup : Sandbox.ModAPI.IMyBlockGroup, Sandbox.ModAPI.Ingame.IMyBlockGroup
  {
    public StringBuilder Name = new StringBuilder();
    internal readonly HashSet<MyTerminalBlock> Blocks = new HashSet<MyTerminalBlock>();

    internal MyBlockGroup()
    {
    }

    internal void Init(MyCubeGrid grid, MyObjectBuilder_BlockGroup builder)
    {
      this.Name.Clear().Append(builder.Name);
      foreach (Vector3I block in builder.Blocks)
      {
        MySlimBlock cubeBlock = grid.GetCubeBlock(block);
        if (cubeBlock != null && cubeBlock.FatBlock is MyTerminalBlock fatBlock)
          this.Blocks.Add(fatBlock);
      }
    }

    internal MyObjectBuilder_BlockGroup GetObjectBuilder()
    {
      MyObjectBuilder_BlockGroup builderBlockGroup = new MyObjectBuilder_BlockGroup();
      builderBlockGroup.Name = this.Name.ToString();
      foreach (MyTerminalBlock block in this.Blocks)
        builderBlockGroup.Blocks.Add(block.Position);
      return builderBlockGroup;
    }

    public override string ToString() => string.Format("{0} - {1} blocks", (object) this.Name, (object) this.Blocks.Count);

    void Sandbox.ModAPI.Ingame.IMyBlockGroup.GetBlocks(
      List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks,
      Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
    {
      blocks?.Clear();
      foreach (MyTerminalBlock block in this.Blocks)
      {
        if (block.IsAccessibleForProgrammableBlock && (collect == null || collect((Sandbox.ModAPI.Ingame.IMyTerminalBlock) block)) && blocks != null)
          blocks.Add((Sandbox.ModAPI.Ingame.IMyTerminalBlock) block);
      }
    }

    void Sandbox.ModAPI.Ingame.IMyBlockGroup.GetBlocksOfType<T>(
      List<Sandbox.ModAPI.Ingame.IMyTerminalBlock> blocks,
      Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, bool> collect)
    {
      blocks?.Clear();
      foreach (MyTerminalBlock block in this.Blocks)
      {
        if ((object) (block as T) != null && block.IsAccessibleForProgrammableBlock && (collect == null || collect((Sandbox.ModAPI.Ingame.IMyTerminalBlock) block)) && blocks != null)
          blocks.Add((Sandbox.ModAPI.Ingame.IMyTerminalBlock) block);
      }
    }

    void Sandbox.ModAPI.Ingame.IMyBlockGroup.GetBlocksOfType<T>(
      List<T> blocks,
      Func<T, bool> collect)
    {
      blocks?.Clear();
      foreach (MyTerminalBlock block in this.Blocks)
      {
        if (block is T obj && block.IsAccessibleForProgrammableBlock && (collect == null || collect(obj)) && blocks != null)
          blocks.Add(obj);
      }
    }

    string Sandbox.ModAPI.Ingame.IMyBlockGroup.Name => this.Name.ToString();

    void Sandbox.ModAPI.IMyBlockGroup.GetBlocks(
      List<Sandbox.ModAPI.IMyTerminalBlock> blocks,
      Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect)
    {
      blocks?.Clear();
      foreach (MyTerminalBlock block in this.Blocks)
      {
        if ((collect == null || collect((Sandbox.ModAPI.IMyTerminalBlock) block)) && blocks != null)
          blocks.Add((Sandbox.ModAPI.IMyTerminalBlock) block);
      }
    }

    void Sandbox.ModAPI.IMyBlockGroup.GetBlocksOfType<T>(
      List<Sandbox.ModAPI.IMyTerminalBlock> blocks,
      Func<Sandbox.ModAPI.IMyTerminalBlock, bool> collect)
    {
      blocks?.Clear();
      foreach (MyTerminalBlock block in this.Blocks)
      {
        if ((object) (block as T) != null && (collect == null || collect((Sandbox.ModAPI.IMyTerminalBlock) block)) && blocks != null)
          blocks.Add((Sandbox.ModAPI.IMyTerminalBlock) block);
      }
    }
  }
}
