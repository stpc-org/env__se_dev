// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyGridPhysicalHierarchy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Physics
{
  public class MyGridPhysicalHierarchy : MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>, IMySceneComponent
  {
    public static MyGridPhysicalHierarchy Static;
    private readonly Dictionary<long, HashSet<MyEntity>> m_nonGridChildren = new Dictionary<long, HashSet<MyEntity>>();

    public void Load()
    {
      MyGridPhysicalHierarchy.Static = this;
      this.SupportsOphrans = true;
      this.SupportsChildToChild = true;
    }

    public void Unload() => MyGridPhysicalHierarchy.Static = (MyGridPhysicalHierarchy) null;

    public override void AddNode(MyCubeGrid nodeToAdd)
    {
      base.AddNode(nodeToAdd);
      this.UpdateRoot(nodeToAdd);
    }

    public override void CreateLink(long linkId, MyCubeGrid parentNode, MyCubeGrid childNode)
    {
      base.CreateLink(linkId, parentNode, childNode);
      this.UpdateRoot(parentNode);
    }

    public override bool BreakLink(long linkId, MyCubeGrid parentNode, MyCubeGrid childNode = null)
    {
      if (childNode == null)
        childNode = this.GetNode(parentNode).m_children[linkId].NodeData;
      bool flag = base.BreakLink(linkId, parentNode, childNode);
      if (!flag)
        flag = base.BreakLink(linkId, childNode, parentNode);
      if (flag)
      {
        this.UpdateRoot(parentNode);
        if (this.GetGroup(parentNode) != this.GetGroup(childNode))
          this.UpdateRoot(childNode);
      }
      return flag;
    }

    public MyCubeGrid GetParent(MyCubeGrid grid)
    {
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node = this.GetNode(grid);
      return node == null ? (MyCubeGrid) null : this.GetParent(node);
    }

    public MyCubeGrid GetParent(
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node)
    {
      return node.m_parents.Count == 0 ? (MyCubeGrid) null : node.m_parents.FirstPair<long, MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node>().Value.NodeData;
    }

    public long GetParentLinkId(
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node)
    {
      return node.m_parents.Count == 0 ? 0L : node.m_parents.FirstPair<long, MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node>().Key;
    }

    public bool IsEntityParent(MyEntity entity) => !(entity is MyCubeGrid grid) || this.GetParent(grid) == null;

    public MyCubeGrid GetRoot(MyCubeGrid grid)
    {
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group group = this.GetGroup(grid);
      return group == null ? grid : group.GroupData.m_root ?? grid;
    }

    public MyEntity GetEntityConnectingToParent(MyCubeGrid grid)
    {
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node = this.GetNode(grid);
      if (node == null)
        return (MyEntity) null;
      return node.m_parents.Count == 0 ? (MyEntity) null : MyEntities.GetEntityById(node.m_parents.FirstPair<long, MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node>().Key);
    }

    public bool HasChildren(MyCubeGrid grid)
    {
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node = this.GetNode(grid);
      return node != null && node.Children.Count > 0;
    }

    public bool IsCyclic(MyCubeGrid grid)
    {
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node = this.GetNode(grid);
      if (node != null && node.Children.Count > 0)
      {
        foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node> childLink in node.ChildLinks)
        {
          if (this.GetParentLinkId(childLink.Value) != childLink.Key || this.IsCyclic(childLink.Value.NodeData))
            return true;
        }
      }
      return false;
    }

    public void ApplyOnChildren(MyCubeGrid grid, Action<MyCubeGrid> action)
    {
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node = this.GetNode(grid);
      if (node == null || node.Children.Count <= 0)
        return;
      foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node> childLink in node.ChildLinks)
      {
        if (this.GetParentLinkId(childLink.Value) == childLink.Key)
          action(childLink.Value.NodeData);
      }
    }

    public void ApplyOnChildren(
      MyCubeGrid grid,
      ref object data,
      MyGridPhysicalHierarchy.MyActionWithData action)
    {
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node = this.GetNode(grid);
      if (node == null || node.Children.Count <= 0)
        return;
      foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node> childLink in node.ChildLinks)
      {
        if (this.GetParentLinkId(childLink.Value) == childLink.Key)
          action(childLink.Value.NodeData, ref data);
      }
    }

    public void ApplyOnAllChildren(MyEntity entity, Action<MyEntity> action)
    {
      if (!(entity is MyCubeGrid node))
        return;
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node1 = this.GetNode(node);
      if (node1 != null && node1.Children.Count > 0)
      {
        foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node> childLink in node1.ChildLinks)
        {
          if (this.GetParentLinkId(childLink.Value) == childLink.Key)
            action((MyEntity) childLink.Value.NodeData);
        }
      }
      HashSet<MyEntity> myEntitySet;
      if (node1 == null || !this.m_nonGridChildren.TryGetValue(node.EntityId, out myEntitySet))
        return;
      foreach (MyEntity myEntity in myEntitySet)
        action(myEntity);
    }

    public bool InSameHierarchy(MyCubeGrid first, MyCubeGrid second) => this.GetRoot(first) == this.GetRoot(second);

    public bool IsChildOf(MyCubeGrid parentGrid, MyEntity entity)
    {
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node = this.GetNode(parentGrid);
      if (node != null && node.Children.Count > 0)
      {
        foreach (KeyValuePair<long, MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node> childLink in node.ChildLinks)
        {
          if (this.GetParentLinkId(childLink.Value) == childLink.Key && childLink.Value.NodeData == entity)
            return true;
        }
      }
      HashSet<MyEntity> myEntitySet;
      return node != null && this.m_nonGridChildren.TryGetValue(parentGrid.EntityId, out myEntitySet) && myEntitySet.Contains(entity);
    }

    public void UpdateRoot(MyCubeGrid node)
    {
      if (MyEntities.IsClosingAll)
        return;
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group group = this.GetGroup(node);
      if (group == null)
        return;
      MyCubeGrid newRoot = this.CalculateNewRoot(group);
      group.GroupData.m_root = newRoot;
      if (newRoot == null)
        return;
      this.ReplaceRoot(newRoot);
      foreach (MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node1 in group.Nodes)
        node1.NodeData.HierarchyUpdated(newRoot);
    }

    private MyCubeGrid CalculateNewRoot(
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Group group)
    {
      if (group.m_members.Count == 1)
        return group.m_members.FirstElement<MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node>().NodeData;
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node1 = (MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node) null;
      float num1 = 0.0f;
      List<MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node> nodeList = new List<MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node>();
      if (group.m_members.Count == 1)
        return group.m_members.FirstElement<MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node>().NodeData;
      bool flag = false;
      long num2 = long.MaxValue;
      foreach (MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node2 in group.Nodes)
      {
        if (node2.NodeData.IsStatic || MyFixedGrids.IsRooted(node2.NodeData))
        {
          if (!flag)
          {
            nodeList.Clear();
            node1 = (MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node) null;
            flag = true;
          }
          nodeList.Add(node2);
        }
        if (!flag)
        {
          if (this.IsGridControlled(node2.NodeData) && node2.NodeData.EntityId < num2)
          {
            node1 = node2;
            num2 = node2.NodeData.EntityId;
          }
          if (node2.NodeData.Physics != null)
          {
            float num3 = 0.0f;
            HkMassProperties? massProperties = node2.NodeData.Physics.Shape.MassProperties;
            if (massProperties.HasValue)
              num3 = massProperties.Value.Mass;
            if ((double) num3 > (double) num1)
            {
              num1 = num3;
              nodeList.Clear();
              nodeList.Add(node2);
            }
            else if ((double) num3 == (double) num1)
              nodeList.Add(node2);
          }
        }
      }
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node3 = (MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node) null;
      if (nodeList.Count == 1)
        node3 = nodeList[0];
      else if (nodeList.Count > 1)
      {
        long entityId = nodeList[0].NodeData.EntityId;
        MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node2 = nodeList[0];
        foreach (MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node4 in nodeList)
        {
          if (MyWeldingGroups.Static.IsEntityParent((MyEntity) node4.NodeData) && entityId > node4.NodeData.EntityId)
          {
            entityId = node4.NodeData.EntityId;
            node2 = node4;
          }
        }
        node3 = node2;
      }
      if (node1 != null)
      {
        if (node3 == null)
          node3 = node1;
        else if (node1.NodeData.Physics != null && node3.NodeData.Physics != null)
        {
          float num3 = 0.0f;
          HkMassProperties? massProperties = node1.NodeData.Physics.Shape.MassProperties;
          if (massProperties.HasValue)
            num3 = massProperties.Value.Mass;
          float num4 = 0.0f;
          massProperties = node3.NodeData.Physics.Shape.MassProperties;
          if (massProperties.HasValue)
            num4 = massProperties.Value.Mass;
          if ((double) num4 / (double) num3 < 2.0)
            node3 = node1;
        }
        else
          node3 = node1;
      }
      return node3?.NodeData;
    }

    private bool IsGridControlled(MyCubeGrid grid)
    {
      MyShipController shipController = grid.GridSystems.ControlSystem.GetShipController();
      return shipController != null && shipController.CubeGrid == grid;
    }

    public Vector3? GetPivot(MyCubeGrid grid, bool parent = false) => !(this.GetEntityConnectingToParent(grid) is MyMechanicalConnectionBlockBase connectingToParent) ? new Vector3?() : connectingToParent.GetConstraintPosition(grid, parent);

    public void AddNonGridNode(MyCubeGrid parent, MyEntity entity)
    {
      if (this.GetGroup(parent) == null)
        return;
      HashSet<MyEntity> myEntitySet;
      if (!this.m_nonGridChildren.TryGetValue(parent.EntityId, out myEntitySet))
      {
        myEntitySet = new HashSet<MyEntity>();
        this.m_nonGridChildren.Add(parent.EntityId, myEntitySet);
        parent.OnClose += new Action<MyEntity>(this.RemoveAllNonGridNodes);
      }
      myEntitySet.Add(entity);
    }

    public void RemoveNonGridNode(MyCubeGrid parent, MyEntity entity)
    {
      HashSet<MyEntity> myEntitySet;
      if (this.GetGroup(parent) == null || !this.m_nonGridChildren.TryGetValue(parent.EntityId, out myEntitySet))
        return;
      myEntitySet.Remove(entity);
      if (myEntitySet.Count != 0)
        return;
      this.m_nonGridChildren.Remove(parent.EntityId);
      parent.OnClose -= new Action<MyEntity>(this.RemoveAllNonGridNodes);
    }

    private void RemoveAllNonGridNodes(MyEntity parent)
    {
      this.m_nonGridChildren.Remove(parent.EntityId);
      parent.OnClose -= new Action<MyEntity>(this.RemoveAllNonGridNodes);
    }

    public bool NonGridLinkExists(long parentId, MyEntity child)
    {
      HashSet<MyEntity> myEntitySet;
      return this.m_nonGridChildren.TryGetValue(parentId, out myEntitySet) && myEntitySet.Contains(child);
    }

    public int GetNodeChainLength(MyCubeGrid grid)
    {
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node = this.GetNode(grid);
      return node != null ? node.ChainLength : 0;
    }

    public void Log(MyCubeGrid grid)
    {
      MyLog.Default.IncreaseIndent();
      MyLog myLog = MyLog.Default;
      object[] objArray = new object[6]
      {
        (object) grid.EntityId,
        (object) grid.DisplayName,
        (object) (grid.Physics != null),
        null,
        null,
        null
      };
      string str;
      if (grid.Physics != null)
      {
        HkMassProperties? massProperties = grid.Physics.Shape.MassProperties;
        if (massProperties.HasValue)
        {
          massProperties = grid.Physics.Shape.MassProperties;
          str = massProperties.Value.Mass.ToString();
          goto label_4;
        }
      }
      str = "None";
label_4:
      objArray[3] = (object) str;
      objArray[4] = (object) grid.IsStatic;
      objArray[5] = (object) this.IsGridControlled(grid);
      string msg = string.Format("{0}: name={1} physics={2} mass={3} static={4} controlled={5}", objArray);
      myLog.WriteLine(msg);
      this.ApplyOnChildren(grid, new Action<MyCubeGrid>(this.Log));
      MyLog.Default.DecreaseIndent();
    }

    public void Draw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_GRID_HIERARCHY)
        return;
      this.ApplyOnNodes(new Action<MyCubeGrid, MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node>(this.DrawNode));
    }

    private void DrawNode(
      MyCubeGrid grid,
      MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node node)
    {
      if (node.m_parents.Count > 0)
        MyRenderProxy.DebugDrawArrow3D(grid.PositionComp.GetPosition(), node.m_parents.FirstPair<long, MyGroups<MyCubeGrid, MyGridPhysicalHierarchyData>.Node>().Value.NodeData.PositionComp.GetPosition(), Color.Orange);
      else
        MyRenderProxy.DebugDrawAxis(grid.PositionComp.WorldMatrixRef, 1f, false);
    }

    public MyGridPhysicalHierarchy()
      : base()
    {
    }

    public delegate void MyActionWithData(MyCubeGrid grid, ref object data);
  }
}
