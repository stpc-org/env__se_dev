// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyTieredUpdateSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage.Game.Entities;
using VRage.Game.ModAPI;

namespace Sandbox.Game.GameSystems
{
  public class MyTieredUpdateSystem : MyUpdateableGridSystem
  {
    private readonly int TIER2_PLAYER_PRESENCE_TIME_FRAMES = 36000;
    private ConcurrentDictionary<long, IMyTieredUpdateBlock> m_blocks = new ConcurrentDictionary<long, IMyTieredUpdateBlock>();
    private bool m_isReplicated;
    private bool? m_isGridPresent;
    private int m_playerPresenceTierTimer;

    public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.OnceBeforeSimulation;

    public MyTieredUpdateSystem(MyCubeGrid grid)
      : base(grid)
    {
      MyCubeGrid grid1 = this.Grid;
      grid1.ReplicationStarted = grid1.ReplicationStarted + new Action(this.ReplicationStarted);
      MyCubeGrid grid2 = this.Grid;
      grid2.ReplicationEnded = grid2.ReplicationEnded + new Action(this.ReplicationEnded);
      this.Grid.GridPresenceUpdate += new Action<bool>(this.GridPresenceUpdate);
      this.Schedule();
    }

    private void GridPresenceUpdate(bool isAnyGridPresent)
    {
      if (!Sync.IsServer)
        return;
      if (this.m_isGridPresent.HasValue)
      {
        int num1 = isAnyGridPresent ? 1 : 0;
        bool? isGridPresent = this.m_isGridPresent;
        int num2 = isGridPresent.GetValueOrDefault() ? 1 : 0;
        if (num1 == num2 & isGridPresent.HasValue)
          return;
      }
      if (isAnyGridPresent)
      {
        this.m_isGridPresent = new bool?(true);
        this.ChangeGridTier(MyUpdateTiersGridPresence.Normal);
      }
      else
      {
        this.m_isGridPresent = new bool?(false);
        this.ChangeGridTier(MyUpdateTiersGridPresence.Tier1);
      }
    }

    private void ChangeGridTier(MyUpdateTiersGridPresence newTier)
    {
      if (!Sync.IsServer || this.Grid.GridPresenceTier == newTier)
        return;
      this.Grid.GridPresenceTier = newTier;
      foreach (IMyTieredUpdateBlock tieredUpdateBlock in (IEnumerable<IMyTieredUpdateBlock>) this.m_blocks.Values)
        tieredUpdateBlock.ChangeTier();
    }

    private void ReplicationEnded()
    {
      this.ChangePlayerTier(MyUpdateTiersPlayerPresence.Tier1);
      this.m_playerPresenceTierTimer = this.TIER2_PLAYER_PRESENCE_TIME_FRAMES;
    }

    private void ReplicationStarted() => this.ChangePlayerTier(MyUpdateTiersPlayerPresence.Normal);

    private void ChangePlayerTier(MyUpdateTiersPlayerPresence newTier)
    {
      if (this.Grid.PlayerPresenceTier == newTier)
        return;
      this.Grid.PlayerPresenceTier = newTier;
      foreach (IMyTieredUpdateBlock tieredUpdateBlock in (IEnumerable<IMyTieredUpdateBlock>) this.m_blocks.Values)
        tieredUpdateBlock.ChangeTier();
    }

    internal void Register(IMyTieredUpdateBlock tieredBlock, long id)
    {
      if (this.m_blocks.ContainsKey(id))
        return;
      this.m_blocks.TryAdd(id, tieredBlock);
      if (this.m_blocks.Count == 1)
        MySession.Static.GetComponent<MySessionComponentSmartUpdater>()?.RegisterToUpdater(this.Grid);
      tieredBlock.ChangeTier();
    }

    internal void Unregister(IMyTieredUpdateBlock tieredBlock, long id)
    {
      if (!this.m_blocks.ContainsKey(id))
        return;
      this.m_blocks.Remove<long, IMyTieredUpdateBlock>(id);
      if (this.m_blocks.Count != 0)
        return;
      MySession.Static.GetComponent<MySessionComponentSmartUpdater>()?.UnregisterFromUpdater(this.Grid);
    }

    protected override void Update()
    {
      this.m_playerPresenceTierTimer = 0;
      if (this.Grid.PlayerPresenceTier != MyUpdateTiersPlayerPresence.Normal || MySession.Static.Players.GetOnlinePlayerCount() != 0)
        return;
      this.ChangePlayerTier(MyUpdateTiersPlayerPresence.Tier1);
      this.m_playerPresenceTierTimer = this.TIER2_PLAYER_PRESENCE_TIME_FRAMES;
    }

    internal void UpdateAfterSimulation100()
    {
      if (this.Grid.PlayerPresenceTier != MyUpdateTiersPlayerPresence.Tier1 || this.m_playerPresenceTierTimer <= 0)
        return;
      this.m_playerPresenceTierTimer -= 100;
      if (this.m_playerPresenceTierTimer > 0)
        return;
      this.ChangePlayerTier(MyUpdateTiersPlayerPresence.Tier2);
    }
  }
}
