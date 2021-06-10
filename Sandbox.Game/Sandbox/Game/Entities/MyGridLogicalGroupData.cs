// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyGridLogicalGroupData
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Groups;

namespace Sandbox.Game.Entities
{
  public class MyGridLogicalGroupData : IGroupData<MyCubeGrid>
  {
    internal readonly MyGridTerminalSystem TerminalSystem;
    internal readonly MyGridWeaponSystem WeaponSystem = new MyGridWeaponSystem();
    internal readonly MyGridResourceDistributorSystem ResourceDistributor;

    public MyCubeGrid Root { get; private set; }

    public MyGridLogicalGroupData()
      : this((string) null)
    {
    }

    public MyGridLogicalGroupData(string debugName)
    {
      this.TerminalSystem = new MyGridTerminalSystem(this);
      this.ResourceDistributor = new MyGridResourceDistributorSystem(debugName, this);
    }

    public void OnRelease() => this.ResourceDistributor.ClearData();

    public void OnNodeAdded(MyCubeGrid entity)
    {
      entity.OnAddedToGroup(this);
      if (this.Root != null)
        return;
      this.Root = entity;
    }

    public void OnNodeRemoved(MyCubeGrid entity)
    {
      entity.OnRemovedFromGroup(this);
      if (this.Root != entity)
        return;
      MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(entity);
      MyCubeGrid newRoot = group != null ? group.Nodes.FirstOrDefault<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node>((Func<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node, bool>) (x => x.NodeData != entity))?.NodeData : (MyCubeGrid) null;
      this.ResourceDistributor.OnRootChanged(this.Root, newRoot);
      this.TerminalSystem.OnRootChanged(this.Root, newRoot);
      this.Root = newRoot;
    }

    public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new()
    {
    }

    internal void UpdateGridOwnership(List<MyCubeGrid> grids, long ownerID)
    {
      foreach (MyCubeGrid grid in grids)
        grid.IsAccessibleForProgrammableBlock = grid.BigOwners.Contains(ownerID);
    }
  }
}
