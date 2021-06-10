// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyFixedGrids
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System.Collections.Generic;
using System.Threading;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
  public class MyFixedGrids : MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>, IMySceneComponent
  {
    private static MyFixedGrids m_static;
    private HashSet<MyCubeGrid> m_roots = new HashSet<MyCubeGrid>();

    private static MyFixedGrids Static => MyFixedGrids.m_static;

    public MyFixedGrids()
      : base(groupSelector: new MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>.MajorGroupComparer(MyFixedGrids.MyFixedGridsGroupData.MajorSelector))
      => this.SupportsChildToChild = true;

    public void Load() => MyFixedGrids.m_static = this;

    public void Unload() => MyFixedGrids.m_static = (MyFixedGrids) null;

    private static void AssertThread()
    {
      Thread updateThread = MySandboxGame.Static.UpdateThread;
      Thread currentThread = Thread.CurrentThread;
    }

    public static void MarkGridRoot(MyCubeGrid grid)
    {
      MyFixedGrids.AssertThread();
      if (!MyFixedGrids.Static.m_roots.Add(grid))
        return;
      MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>.Group group = MyFixedGrids.Static.GetGroup(grid);
      if (group == null)
        MyFixedGrids.MyFixedGridsGroupData.ConvertGrid(grid, true);
      else
        group.GroupData.OnRootAdded();
    }

    public static void UnmarkGridRoot(MyCubeGrid grid)
    {
      MyFixedGrids.AssertThread();
      if (!MyFixedGrids.Static.m_roots.Remove(grid))
        return;
      MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>.Group group = MyFixedGrids.Static.GetGroup(grid);
      if (group == null)
        MyFixedGrids.MyFixedGridsGroupData.ConvertGrid(grid, false);
      else
        group.GroupData.OnRootRemoved();
    }

    public static void Link(MyCubeGrid parent, MyCubeGrid child, MyCubeBlock linkingBlock)
    {
      MyFixedGrids.AssertThread();
      MyFixedGrids.Static.CreateLink(linkingBlock.EntityId, parent, child);
    }

    public static void BreakLink(MyCubeGrid parent, MyCubeGrid child, MyCubeBlock linkingBlock)
    {
      MyFixedGrids.AssertThread();
      MyFixedGrids.Static.BreakLink(linkingBlock.EntityId, parent, child);
    }

    public static bool IsRooted(MyCubeGrid grid)
    {
      if (MyFixedGrids.Static.m_roots.Contains(grid))
        return true;
      MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>.Group group = MyFixedGrids.Static.GetGroup(grid);
      return group != null && group.GroupData.IsRooted;
    }

    public class MyFixedGridsGroupData : IGroupData<MyCubeGrid>
    {
      private MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>.Group m_group;
      private int m_rootedGrids;

      public bool IsRooted => this.m_rootedGrids > 0;

      public void OnNodeAdded(MyCubeGrid grid)
      {
        bool flag = false;
        if (MyFixedGrids.Static.m_roots.Contains(grid))
        {
          this.OnRootAdded();
          flag = true;
        }
        if (!(flag | (uint) this.m_rootedGrids > 0U))
          return;
        MyFixedGrids.MyFixedGridsGroupData.ConvertGrid(grid, true);
      }

      public void OnNodeRemoved(MyCubeGrid grid)
      {
        if (MyFixedGrids.Static.m_roots.Contains(grid))
        {
          this.OnRootRemoved();
        }
        else
        {
          if (this.m_rootedGrids == 0)
            return;
          MyFixedGrids.MyFixedGridsGroupData.ConvertGrid(grid, false);
        }
      }

      public void OnRootAdded()
      {
        if (this.m_rootedGrids++ != 0)
          return;
        this.Convert(true);
      }

      public void OnRootRemoved()
      {
        if (--this.m_rootedGrids != 0)
          return;
        this.Convert(false);
      }

      private void Convert(bool @static)
      {
        foreach (MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>.Node node in this.m_group.Nodes)
          MyFixedGrids.MyFixedGridsGroupData.ConvertGrid(node.NodeData, @static);
      }

      public static void ConvertGrid(MyCubeGrid grid, bool @static) => grid.IsMarkedForEarlyDeactivation = @static;

      public void OnCreate<TGroupData>(MyGroups<MyCubeGrid, TGroupData>.Group group) where TGroupData : IGroupData<MyCubeGrid>, new() => this.m_group = group as MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>.Group;

      public void OnRelease() => this.m_group = (MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>.Group) null;

      public static bool MajorSelector(
        MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>.Group major,
        MyGroups<MyCubeGrid, MyFixedGrids.MyFixedGridsGroupData>.Group minor)
      {
        int num = major.GroupData.m_rootedGrids > 0 ? 1 : 0;
        bool flag = minor.GroupData.m_rootedGrids > 0;
        if (num != 0)
        {
          if (!flag)
            return true;
        }
        else if (flag)
          return false;
        return major.Nodes.Count >= minor.Nodes.Count;
      }
    }
  }
}
