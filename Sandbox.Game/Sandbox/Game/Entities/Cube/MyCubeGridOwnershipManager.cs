// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyCubeGridOwnershipManager
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Sandbox.Game.Entities.Cube
{
  internal class MyCubeGridOwnershipManager
  {
    private MyCubeGrid m_grid;
    public Dictionary<long, int> PlayerOwnedBlocks;
    public Dictionary<long, int> PlayerOwnedValidBlocks;
    public List<long> BigOwners;
    public List<long> SmallOwners;
    public int MaxBlocks;
    public long gridEntityId;
    public bool m_needRecalculateOwners;

    public bool NeedRecalculateOwners
    {
      get => this.m_needRecalculateOwners;
      set
      {
        if (value == this.m_needRecalculateOwners)
          return;
        this.m_needRecalculateOwners = value;
        if (!value)
          return;
        this.m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, new Action(this.RecalculateOwnersThreadSafe), 20);
      }
    }

    private bool IsValidBlock(MyCubeBlock block) => block.IsFunctional;

    public void Init(MyCubeGrid grid)
    {
      this.m_grid = grid;
      this.PlayerOwnedBlocks = new Dictionary<long, int>();
      this.PlayerOwnedValidBlocks = new Dictionary<long, int>();
      this.BigOwners = new List<long>();
      this.SmallOwners = new List<long>();
      this.MaxBlocks = 0;
      this.gridEntityId = grid.EntityId;
      foreach (MyCubeBlock fatBlock in grid.GetFatBlocks())
      {
        long ownerId = fatBlock.OwnerId;
        if (ownerId != 0L)
        {
          if (!this.PlayerOwnedBlocks.ContainsKey(ownerId))
            this.PlayerOwnedBlocks.Add(ownerId, 0);
          this.PlayerOwnedBlocks[ownerId]++;
          if (this.IsValidBlock(fatBlock))
          {
            if (!this.PlayerOwnedValidBlocks.ContainsKey(ownerId))
              this.PlayerOwnedValidBlocks.Add(ownerId, 0);
            if (++this.PlayerOwnedValidBlocks[fatBlock.OwnerId] > this.MaxBlocks)
              this.MaxBlocks = this.PlayerOwnedValidBlocks[ownerId];
          }
        }
      }
      this.NeedRecalculateOwners = true;
    }

    internal void RecalculateOwnersThreadSafe()
    {
      if (!Sandbox.Game.Entities.MyEntities.IsAsyncUpdateInProgress && Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
        return;
      if (Sandbox.Game.Entities.MyEntities.IsAsyncUpdateInProgress)
      {
        this.RecalculateOwnersInternal(false);
        Sandbox.Game.Entities.MyEntities.InvokeLater(new Action(this.UpdatePlayerGrids));
      }
      else
        this.RecalculateOwnersInternal();
    }

    private void RecalculateOwnersInternal(bool updatePlayerGrids = true)
    {
      this.NeedRecalculateOwners = false;
      this.MaxBlocks = 0;
      foreach (long key in this.PlayerOwnedValidBlocks.Keys)
      {
        if (this.PlayerOwnedValidBlocks[key] > this.MaxBlocks)
          this.MaxBlocks = this.PlayerOwnedValidBlocks[key];
      }
      this.BigOwners.Clear();
      foreach (long key in this.PlayerOwnedValidBlocks.Keys)
      {
        if (this.PlayerOwnedValidBlocks[key] == this.MaxBlocks)
          this.BigOwners.Add(key);
      }
      if (updatePlayerGrids)
        this.UpdatePlayerGrids();
      this.m_grid.NotifyBlockOwnershipChange();
    }

    private void UpdatePlayerGrids()
    {
      if (this.SmallOwners.Contains(MySession.Static.LocalPlayerId))
        MySession.Static.LocalHumanPlayer.RemoveGrid(this.gridEntityId);
      this.SmallOwners.Clear();
      foreach (long key in this.PlayerOwnedBlocks.Keys)
      {
        this.SmallOwners.Add(key);
        if (key == MySession.Static.LocalPlayerId)
          MySession.Static.LocalHumanPlayer.AddGrid(this.gridEntityId);
      }
    }

    public void ChangeBlockOwnership(MyCubeBlock block, long oldOwner, long newOwner)
    {
      this.DecreaseValue(ref this.PlayerOwnedBlocks, oldOwner);
      this.IncreaseValue(ref this.PlayerOwnedBlocks, newOwner);
      if (this.IsValidBlock(block))
      {
        this.DecreaseValue(ref this.PlayerOwnedValidBlocks, oldOwner);
        this.IncreaseValue(ref this.PlayerOwnedValidBlocks, newOwner);
      }
      this.NeedRecalculateOwners = true;
    }

    public void UpdateOnFunctionalChange(long ownerId, bool newFunctionalValue)
    {
      if (!newFunctionalValue)
        this.DecreaseValue(ref this.PlayerOwnedValidBlocks, ownerId);
      else
        this.IncreaseValue(ref this.PlayerOwnedValidBlocks, ownerId);
      this.NeedRecalculateOwners = true;
    }

    public void IncreaseValue(ref Dictionary<long, int> dict, long key)
    {
      if (key == 0L)
        return;
      if (!dict.ContainsKey(key))
        dict.Add(key, 0);
      dict[key]++;
    }

    public void DecreaseValue(ref Dictionary<long, int> dict, long key)
    {
      if (key == 0L || !dict.ContainsKey(key))
        return;
      dict[key]--;
      if (dict[key] != 0)
        return;
      dict.Remove(key);
    }
  }
}
