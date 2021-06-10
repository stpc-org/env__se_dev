// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyAntennaSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Groups;

namespace Sandbox.Game.GameSystems
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 588, typeof (MyObjectBuilder_AntennaSessionComponent), null, false)]
  public class MyAntennaSystem : MySessionComponentBase
  {
    private static MyAntennaSystem m_static;
    private List<long> m_addedItems = new List<long>();
    private HashSet<MyAntennaSystem.BroadcasterInfo> m_output = new HashSet<MyAntennaSystem.BroadcasterInfo>((IEqualityComparer<MyAntennaSystem.BroadcasterInfo>) new MyAntennaSystem.BroadcasterInfoComparer());
    private HashSet<MyDataBroadcaster> m_tempPlayerRelayedBroadcasters = new HashSet<MyDataBroadcaster>();
    private List<MyDataBroadcaster> m_tempGridBroadcastersFromPlayer = new List<MyDataBroadcaster>();
    private HashSet<MyDataReceiver> m_tmpReceivers = new HashSet<MyDataReceiver>();
    private HashSet<MyDataBroadcaster> m_tmpBroadcasters = new HashSet<MyDataBroadcaster>();
    private HashSet<MyDataBroadcaster> m_tmpRelayedBroadcasters = new HashSet<MyDataBroadcaster>();
    private Dictionary<long, MyLaserBroadcaster> m_laserAntennas = new Dictionary<long, MyLaserBroadcaster>();
    private Dictionary<long, MyProxyAntenna> m_proxyAntennas = new Dictionary<long, MyProxyAntenna>();
    private Dictionary<long, HashSet<MyDataBroadcaster>> m_proxyGrids = new Dictionary<long, HashSet<MyDataBroadcaster>>();

    public static MyAntennaSystem Static => MyAntennaSystem.m_static;

    public Dictionary<long, MyLaserBroadcaster> LaserAntennas => this.m_laserAntennas;

    public override void LoadData()
    {
      MyAntennaSystem.m_static = this;
      base.LoadData();
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      this.m_addedItems.Clear();
      this.m_addedItems = (List<long>) null;
      this.m_output.Clear();
      this.m_output = (HashSet<MyAntennaSystem.BroadcasterInfo>) null;
      this.m_tempGridBroadcastersFromPlayer.Clear();
      this.m_tempGridBroadcastersFromPlayer = (List<MyDataBroadcaster>) null;
      this.m_tempPlayerRelayedBroadcasters.Clear();
      this.m_tempPlayerRelayedBroadcasters = (HashSet<MyDataBroadcaster>) null;
      MyAntennaSystem.m_static = (MyAntennaSystem) null;
    }

    public HashSet<MyAntennaSystem.BroadcasterInfo> GetConnectedGridsInfo(
      MyEntity interactedEntityRepresentative,
      MyPlayer player = null,
      bool mutual = true,
      bool accessible = false)
    {
      this.m_output.Clear();
      if (player == null)
      {
        player = MySession.Static.LocalHumanPlayer;
        if (player == null)
          return this.m_output;
      }
      MyIdentity identity = player.Identity;
      this.m_tmpReceivers.Clear();
      this.m_tmpRelayedBroadcasters.Clear();
      if (interactedEntityRepresentative == null)
        return this.m_output;
      this.m_output.Add(new MyAntennaSystem.BroadcasterInfo()
      {
        EntityId = interactedEntityRepresentative.EntityId,
        Name = interactedEntityRepresentative.DisplayName
      });
      this.GetAllRelayedBroadcasters(interactedEntityRepresentative, identity.IdentityId, mutual, this.m_tmpRelayedBroadcasters);
      foreach (MyDataBroadcaster relayedBroadcaster in this.m_tmpRelayedBroadcasters)
      {
        if (!accessible || relayedBroadcaster.CanBeUsedByPlayer(identity.IdentityId))
          this.m_output.Add(relayedBroadcaster.Info);
      }
      return this.m_output;
    }

    public MyEntity GetBroadcasterParentEntity(MyDataBroadcaster broadcaster) => broadcaster.Entity is MyCubeBlock ? (MyEntity) (broadcaster.Entity as MyCubeBlock).CubeGrid : broadcaster.Entity as MyEntity;

    public MyCubeGrid GetLogicalGroupRepresentative(MyCubeGrid grid)
    {
      MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(grid);
      if (group == null || group.Nodes.Count == 0)
        return grid;
      MyCubeGrid nodeData = group.Nodes.First().NodeData;
      foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
      {
        if (node.NodeData.GetBlocks().Count > nodeData.GetBlocks().Count)
          nodeData = node.NodeData;
      }
      return nodeData;
    }

    public void GetEntityBroadcasters(
      MyEntity entity,
      ref HashSet<MyDataBroadcaster> output,
      long playerId = 0)
    {
      switch (entity)
      {
        case MyCharacter myCharacter:
          output.Add((MyDataBroadcaster) myCharacter.RadioBroadcaster);
          if (!(myCharacter.GetTopMostParent((Type) null) is MyCubeGrid topMostParent))
            break;
          MyAntennaSystem.GetCubeGridGroupBroadcasters(topMostParent, output, playerId);
          break;
        case MyCubeBlock myCubeBlock:
          MyAntennaSystem.GetCubeGridGroupBroadcasters(myCubeBlock.CubeGrid, output, playerId);
          break;
        case MyCubeGrid grid:
          MyAntennaSystem.GetCubeGridGroupBroadcasters(grid, output, playerId);
          break;
        case MyProxyAntenna proxy:
          this.GetProxyGridBroadcasters(proxy, ref output, playerId);
          break;
      }
    }

    public static void GetCubeGridGroupBroadcasters(
      MyCubeGrid grid,
      HashSet<MyDataBroadcaster> output,
      long playerId = 0)
    {
      MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(grid);
      if (group != null)
      {
        foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node member in group.m_members)
        {
          foreach (MyDataBroadcaster broadcaster in member.NodeData.GridSystems.RadioSystem.Broadcasters)
          {
            if (playerId == 0L || broadcaster.CanBeUsedByPlayer(playerId))
              output.Add(broadcaster);
          }
        }
      }
      else
      {
        foreach (MyDataBroadcaster broadcaster in grid.GridSystems.RadioSystem.Broadcasters)
        {
          if (playerId == 0L || broadcaster.CanBeUsedByPlayer(playerId))
            output.Add(broadcaster);
        }
      }
    }

    private void GetProxyGridBroadcasters(
      MyProxyAntenna proxy,
      ref HashSet<MyDataBroadcaster> output,
      long playerId = 0)
    {
      HashSet<MyDataBroadcaster> myDataBroadcasterSet;
      if (!this.m_proxyGrids.TryGetValue(proxy.Info.EntityId, out myDataBroadcasterSet))
        return;
      foreach (MyDataBroadcaster myDataBroadcaster in myDataBroadcasterSet)
      {
        if (playerId == 0L || myDataBroadcaster.CanBeUsedByPlayer(playerId))
          output.Add(myDataBroadcaster);
      }
    }

    public void GetEntityReceivers(
      MyEntity entity,
      ref HashSet<MyDataReceiver> output,
      long playerId = 0)
    {
      switch (entity)
      {
        case MyCharacter myCharacter:
          output.Add((MyDataReceiver) myCharacter.RadioReceiver);
          if (!(myCharacter.GetTopMostParent((Type) null) is MyCubeGrid topMostParent))
            break;
          this.GetCubeGridGroupReceivers(topMostParent, ref output, playerId);
          break;
        case MyCubeBlock myCubeBlock:
          this.GetCubeGridGroupReceivers(myCubeBlock.CubeGrid, ref output, playerId);
          break;
        case MyCubeGrid grid:
          this.GetCubeGridGroupReceivers(grid, ref output, playerId);
          break;
        case MyProxyAntenna proxy:
          this.GetProxyGridReceivers(proxy, ref output, playerId);
          break;
      }
    }

    private void GetCubeGridGroupReceivers(
      MyCubeGrid grid,
      ref HashSet<MyDataReceiver> output,
      long playerId = 0)
    {
      MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(grid);
      if (group != null)
      {
        foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node member in group.m_members)
        {
          foreach (MyDataReceiver receiver in member.NodeData.GridSystems.RadioSystem.Receivers)
          {
            if (playerId == 0L || receiver.CanBeUsedByPlayer(playerId))
              output.Add(receiver);
          }
        }
      }
      else
      {
        foreach (MyDataReceiver receiver in grid.GridSystems.RadioSystem.Receivers)
        {
          if (playerId == 0L || receiver.CanBeUsedByPlayer(playerId))
            output.Add(receiver);
        }
      }
    }

    private void GetProxyGridReceivers(
      MyProxyAntenna proxy,
      ref HashSet<MyDataReceiver> output,
      long playerId = 0)
    {
      HashSet<MyDataBroadcaster> myDataBroadcasterSet;
      if (!this.m_proxyGrids.TryGetValue(proxy.Info.EntityId, out myDataBroadcasterSet))
        return;
      foreach (MyDataBroadcaster myDataBroadcaster in myDataBroadcasterSet)
      {
        if (myDataBroadcaster.Receiver != null && (playerId == 0L || myDataBroadcaster.CanBeUsedByPlayer(playerId)))
          output.Add(myDataBroadcaster.Receiver);
      }
    }

    public HashSet<MyDataBroadcaster> GetAllRelayedBroadcasters(
      MyDataReceiver receiver,
      long identityId,
      bool mutual,
      HashSet<MyDataBroadcaster> output = null)
    {
      if (output == null)
      {
        output = this.m_tmpBroadcasters;
        output.Clear();
      }
      foreach (MyDataBroadcaster myDataBroadcaster in receiver.BroadcastersInRange)
      {
        if (!output.Contains(myDataBroadcaster) && !myDataBroadcaster.Closed && (!mutual || myDataBroadcaster.Receiver != null && receiver.Broadcaster != null && myDataBroadcaster.Receiver.BroadcastersInRange.Contains(receiver.Broadcaster)))
        {
          output.Add(myDataBroadcaster);
          if (myDataBroadcaster.Receiver != null && myDataBroadcaster.CanBeUsedByPlayer(identityId))
            this.GetAllRelayedBroadcasters(myDataBroadcaster.Receiver, identityId, mutual, output);
        }
      }
      return output;
    }

    public HashSet<MyDataBroadcaster> GetAllRelayedBroadcasters(
      MyEntity entity,
      long identityId,
      bool mutual = true,
      HashSet<MyDataBroadcaster> output = null)
    {
      if (output == null)
      {
        output = this.m_tmpBroadcasters;
        output.Clear();
      }
      this.m_tmpReceivers.Clear();
      this.GetEntityReceivers(entity, ref this.m_tmpReceivers, identityId);
      foreach (MyDataReceiver tmpReceiver in this.m_tmpReceivers)
        this.GetAllRelayedBroadcasters(tmpReceiver, identityId, mutual, output);
      return output;
    }

    public bool CheckConnection(MyIdentity sender, MyIdentity receiver)
    {
      if (sender == receiver)
        return true;
      return sender.Character != null && receiver.Character != null && this.CheckConnection((MyDataReceiver) receiver.Character.RadioReceiver, (MyDataBroadcaster) sender.Character.RadioBroadcaster, receiver.IdentityId, false);
    }

    public bool CheckConnection(
      MyDataReceiver receiver,
      MyDataBroadcaster broadcaster,
      long playerIdentityId,
      bool mutual)
    {
      return receiver != null && broadcaster != null && this.GetAllRelayedBroadcasters(receiver, playerIdentityId, mutual).Contains(broadcaster);
    }

    public bool CheckConnection(
      MyEntity receivingEntity,
      MyDataBroadcaster broadcaster,
      long playerIdentityId,
      bool mutual)
    {
      return receivingEntity != null && broadcaster != null && this.GetAllRelayedBroadcasters(receivingEntity, playerIdentityId, mutual).Contains(broadcaster);
    }

    public bool CheckConnection(
      MyDataReceiver receiver,
      MyEntity broadcastingEntity,
      long playerIdentityId,
      bool mutual)
    {
      if (receiver == null || broadcastingEntity == null)
        return false;
      this.m_tmpBroadcasters.Clear();
      this.m_tmpRelayedBroadcasters.Clear();
      this.GetAllRelayedBroadcasters(receiver, playerIdentityId, mutual, this.m_tmpRelayedBroadcasters);
      this.GetEntityBroadcasters(broadcastingEntity, ref this.m_tmpBroadcasters, playerIdentityId);
      foreach (MyDataBroadcaster relayedBroadcaster in this.m_tmpRelayedBroadcasters)
      {
        if (this.m_tmpBroadcasters.Contains(relayedBroadcaster))
          return true;
      }
      return false;
    }

    public bool CheckConnection(
      MyEntity broadcastingEntity,
      MyEntity receivingEntity,
      MyPlayer player,
      bool mutual = true)
    {
      if (broadcastingEntity is MyCubeGrid grid)
        broadcastingEntity = (MyEntity) this.GetLogicalGroupRepresentative(grid);
      if (receivingEntity is MyCubeGrid grid)
        receivingEntity = (MyEntity) this.GetLogicalGroupRepresentative(grid);
      foreach (MyAntennaSystem.BroadcasterInfo broadcasterInfo in this.GetConnectedGridsInfo(receivingEntity, player, mutual))
      {
        if (broadcasterInfo.EntityId == broadcastingEntity.EntityId)
          return true;
      }
      return false;
    }

    public void RegisterAntenna(MyDataBroadcaster broadcaster)
    {
      if (broadcaster.Entity is MyProxyAntenna)
      {
        MyProxyAntenna entity = broadcaster.Entity as MyProxyAntenna;
        this.m_proxyAntennas[broadcaster.AntennaEntityId] = entity;
        this.RegisterProxyGrid(broadcaster);
        if (MyEntities.GetEntityById(broadcaster.AntennaEntityId) != null)
          return;
        entity.Active = true;
      }
      else
      {
        MyProxyAntenna myProxyAntenna;
        if (!this.m_proxyAntennas.TryGetValue(broadcaster.AntennaEntityId, out myProxyAntenna))
          return;
        myProxyAntenna.Active = false;
      }
    }

    public void UnregisterAntenna(MyDataBroadcaster broadcaster)
    {
      if (broadcaster.Entity is MyProxyAntenna)
      {
        MyProxyAntenna entity = broadcaster.Entity as MyProxyAntenna;
        this.m_proxyAntennas.Remove(broadcaster.AntennaEntityId);
        this.UnregisterProxyGrid(broadcaster);
        entity.Active = false;
      }
      else
      {
        MyProxyAntenna myProxyAntenna;
        if (!this.m_proxyAntennas.TryGetValue(broadcaster.AntennaEntityId, out myProxyAntenna))
          return;
        myProxyAntenna.Active = true;
      }
    }

    private void RegisterProxyGrid(MyDataBroadcaster broadcaster)
    {
      HashSet<MyDataBroadcaster> myDataBroadcasterSet;
      if (!this.m_proxyGrids.TryGetValue(broadcaster.Info.EntityId, out myDataBroadcasterSet))
      {
        myDataBroadcasterSet = new HashSet<MyDataBroadcaster>();
        this.m_proxyGrids.Add(broadcaster.Info.EntityId, myDataBroadcasterSet);
      }
      myDataBroadcasterSet.Add(broadcaster);
    }

    private void UnregisterProxyGrid(MyDataBroadcaster broadcaster)
    {
      HashSet<MyDataBroadcaster> myDataBroadcasterSet;
      if (!this.m_proxyGrids.TryGetValue(broadcaster.Info.EntityId, out myDataBroadcasterSet))
        return;
      myDataBroadcasterSet.Remove(broadcaster);
      if (myDataBroadcasterSet.Count != 0)
        return;
      this.m_proxyGrids.Remove(broadcaster.Info.EntityId);
    }

    public void AddLaser(long id, MyLaserBroadcaster laser, bool register = true)
    {
      if (register)
        this.RegisterAntenna((MyDataBroadcaster) laser);
      this.m_laserAntennas.Add(id, laser);
    }

    public void RemoveLaser(long id, bool register = true)
    {
      MyLaserBroadcaster laserBroadcaster;
      if (!this.m_laserAntennas.TryGetValue(id, out laserBroadcaster))
        return;
      this.m_laserAntennas.Remove(id);
      if (!register)
        return;
      this.UnregisterAntenna((MyDataBroadcaster) laserBroadcaster);
    }

    public struct BroadcasterInfo
    {
      public long EntityId;
      public string Name;
    }

    public class BroadcasterInfoComparer : IEqualityComparer<MyAntennaSystem.BroadcasterInfo>
    {
      public bool Equals(MyAntennaSystem.BroadcasterInfo x, MyAntennaSystem.BroadcasterInfo y) => x.EntityId == y.EntityId && string.Equals(x.Name, y.Name);

      public int GetHashCode(MyAntennaSystem.BroadcasterInfo obj)
      {
        int num = obj.EntityId.GetHashCode();
        if (obj.Name != null)
          num = num * 397 ^ obj.Name.GetHashCode();
        return num;
      }
    }
  }
}
