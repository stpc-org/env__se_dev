// Decompiled with JetBrains decompiler
// Type: VRage.Groups.MyGroups`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Collections;

namespace VRage.Groups
{
  public class MyGroups<TNode, TGroupData> : MyGroupsBase<TNode>
    where TNode : class
    where TGroupData : IGroupData<TNode>, new()
  {
    private Stack<MyGroups<TNode, TGroupData>.Group> m_groupPool = new Stack<MyGroups<TNode, TGroupData>.Group>(32);
    private Stack<MyGroups<TNode, TGroupData>.Node> m_nodePool = new Stack<MyGroups<TNode, TGroupData>.Node>(32);
    private Dictionary<TNode, MyGroups<TNode, TGroupData>.Node> m_nodes = new Dictionary<TNode, MyGroups<TNode, TGroupData>.Node>();
    private HashSet<MyGroups<TNode, TGroupData>.Group> m_groups = new HashSet<MyGroups<TNode, TGroupData>.Group>();
    private HashSet<MyGroups<TNode, TGroupData>.Node> m_disconnectHelper = new HashSet<MyGroups<TNode, TGroupData>.Node>();
    private MyGroups<TNode, TGroupData>.MajorGroupComparer m_groupSelector;
    private bool m_isRecalculating;
    private HashSet<MyGroups<TNode, TGroupData>.Node> m_tmpClosed = new HashSet<MyGroups<TNode, TGroupData>.Node>();
    private Queue<MyGroups<TNode, TGroupData>.Node> m_tmpOpen = new Queue<MyGroups<TNode, TGroupData>.Node>();
    private List<MyGroups<TNode, TGroupData>.Node> m_tmpList = new List<MyGroups<TNode, TGroupData>.Node>();

    public bool SupportsOphrans { get; protected set; }

    protected bool SupportsChildToChild { get; set; }

    public MyGroups(
      bool supportOphrans = false,
      MyGroups<TNode, TGroupData>.MajorGroupComparer groupSelector = null)
    {
      this.SupportsOphrans = supportOphrans;
      this.m_groupSelector = groupSelector ?? new MyGroups<TNode, TGroupData>.MajorGroupComparer(MyGroups<TNode, TGroupData>.IsMajorGroup);
    }

    public void ApplyOnNodes(
      Action<TNode, MyGroups<TNode, TGroupData>.Node> action)
    {
      foreach (KeyValuePair<TNode, MyGroups<TNode, TGroupData>.Node> node in this.m_nodes)
        action(node.Key, node.Value);
    }

    public override bool HasSameGroup(TNode a, TNode b)
    {
      MyGroups<TNode, TGroupData>.Group group1 = this.GetGroup(a);
      MyGroups<TNode, TGroupData>.Group group2 = this.GetGroup(b);
      return group1 != null && group1 == group2;
    }

    public MyGroups<TNode, TGroupData>.Group GetGroup(TNode Node)
    {
      MyGroups<TNode, TGroupData>.Node node;
      return this.m_nodes.TryGetValue(Node, out node) ? node.m_group : (MyGroups<TNode, TGroupData>.Group) null;
    }

    public HashSetReader<MyGroups<TNode, TGroupData>.Group> Groups => new HashSetReader<MyGroups<TNode, TGroupData>.Group>(this.m_groups);

    public override void AddNode(TNode nodeToAdd)
    {
      if (!this.SupportsOphrans)
        throw new InvalidOperationException("Cannot add/remove node when ophrans are not supported");
      MyGroups<TNode, TGroupData>.Node node = this.GetOrCreateNode(nodeToAdd);
      if (node.m_group != null)
        return;
      node.m_group = this.AcquireGroup();
      node.m_group.m_members.Add(node);
    }

    public override void RemoveNode(TNode nodeToRemove)
    {
      if (!this.SupportsOphrans)
        throw new InvalidOperationException("Cannot add/remove node when ophrans are not supported");
      MyGroups<TNode, TGroupData>.Node node;
      if (!this.m_nodes.TryGetValue(nodeToRemove, out node))
        return;
      this.BreakAllLinks(node);
      this.TryReleaseNode(node);
    }

    private void BreakAllLinks(MyGroups<TNode, TGroupData>.Node node)
    {
      while (node.m_parents.Count > 0)
      {
        Dictionary<long, MyGroups<TNode, TGroupData>.Node>.Enumerator enumerator = node.m_parents.GetEnumerator();
        enumerator.MoveNext();
        KeyValuePair<long, MyGroups<TNode, TGroupData>.Node> current = enumerator.Current;
        this.BreakLinkInternal(current.Key, current.Value, node);
      }
      while (node.m_children.Count > 0)
      {
        SortedDictionary<long, MyGroups<TNode, TGroupData>.Node>.Enumerator enumerator = node.m_children.GetEnumerator();
        enumerator.MoveNext();
        KeyValuePair<long, MyGroups<TNode, TGroupData>.Node> current = enumerator.Current;
        this.BreakLinkInternal(current.Key, node, current.Value);
      }
    }

    public override void CreateLink(long linkId, TNode parentNode, TNode childNode)
    {
      MyGroups<TNode, TGroupData>.Node node1 = this.GetOrCreateNode(parentNode);
      MyGroups<TNode, TGroupData>.Node node2 = this.GetOrCreateNode(childNode);
      if (node1.m_group != null && node2.m_group != null)
      {
        if (node1.m_group == node2.m_group)
        {
          this.AddLink(linkId, node1, node2);
        }
        else
        {
          this.MergeGroups(node1.m_group, node2.m_group);
          this.AddLink(linkId, node1, node2);
        }
      }
      else if (node1.m_group != null)
      {
        node2.m_group = node1.m_group;
        node1.m_group.m_members.Add(node2);
        this.AddLink(linkId, node1, node2);
      }
      else if (node2.m_group != null)
      {
        node1.m_group = node2.m_group;
        node2.m_group.m_members.Add(node1);
        this.AddLink(linkId, node1, node2);
      }
      else
      {
        MyGroups<TNode, TGroupData>.Group group = this.AcquireGroup();
        node1.m_group = group;
        group.m_members.Add(node1);
        node2.m_group = group;
        group.m_members.Add(node2);
        this.AddLink(linkId, node1, node2);
      }
    }

    public override bool BreakLink(long linkId, TNode parentNode, TNode childNode = null)
    {
      MyGroups<TNode, TGroupData>.Node parent;
      MyGroups<TNode, TGroupData>.Node child;
      return this.m_nodes.TryGetValue(parentNode, out parent) && parent.m_children.TryGetValue(linkId, out child) && this.BreakLinkInternal(linkId, parent, child);
    }

    public void BreakAllLinks(TNode node)
    {
      MyGroups<TNode, TGroupData>.Node node1;
      if (!this.m_nodes.TryGetValue(node, out node1))
        return;
      this.BreakAllLinks(node1);
    }

    public MyGroups<TNode, TGroupData>.Node GetNode(TNode node) => this.m_nodes.GetValueOrDefault<TNode, MyGroups<TNode, TGroupData>.Node>(node);

    public override bool LinkExists(long linkId, TNode parentNode, TNode childNode = null)
    {
      MyGroups<TNode, TGroupData>.Node node1;
      MyGroups<TNode, TGroupData>.Node node2;
      if (!this.m_nodes.TryGetValue(parentNode, out node1) || !node1.m_children.TryGetValue(linkId, out node2))
        return false;
      return (object) childNode == null || (object) childNode == (object) node2.m_node;
    }

    private bool BreakLinkInternal(
      long linkId,
      MyGroups<TNode, TGroupData>.Node parent,
      MyGroups<TNode, TGroupData>.Node child)
    {
      bool flag = parent.m_children.Remove(linkId) & child.m_parents.Remove(linkId);
      if (!flag && this.SupportsChildToChild)
        flag &= child.m_children.Remove(linkId);
      this.RecalculateConnectivity(parent, child);
      return flag;
    }

    [Conditional("DEBUG")]
    private void DebugCheckConsistency(
      long linkId,
      MyGroups<TNode, TGroupData>.Node parent,
      MyGroups<TNode, TGroupData>.Node child)
    {
    }

    private void AddNeighbours(
      HashSet<MyGroups<TNode, TGroupData>.Node> nodes,
      MyGroups<TNode, TGroupData>.Node nodeToAdd)
    {
      if (nodes.Contains(nodeToAdd))
        return;
      nodes.Add(nodeToAdd);
      foreach (KeyValuePair<long, MyGroups<TNode, TGroupData>.Node> child in nodeToAdd.m_children)
        this.AddNeighbours(nodes, child.Value);
      foreach (KeyValuePair<long, MyGroups<TNode, TGroupData>.Node> parent in nodeToAdd.m_parents)
        this.AddNeighbours(nodes, parent.Value);
    }

    private bool TryReleaseNode(MyGroups<TNode, TGroupData>.Node node)
    {
      if ((object) node.m_node == null || node.m_group == null || (node.m_children.Count != 0 || node.m_parents.Count != 0))
        return false;
      MyGroups<TNode, TGroupData>.Group mGroup = node.m_group;
      node.m_group.m_members.Remove(node);
      this.m_nodes.Remove(node.m_node);
      node.m_group = (MyGroups<TNode, TGroupData>.Group) null;
      node.m_node = default (TNode);
      this.ReturnNode(node);
      if (mGroup.m_members.Count == 0)
        this.ReturnGroup(mGroup);
      return true;
    }

    private void RecalculateConnectivity(
      MyGroups<TNode, TGroupData>.Node parent,
      MyGroups<TNode, TGroupData>.Node child)
    {
      if (this.m_isRecalculating || parent == null || (parent.Group == null || child == null))
        return;
      if (child.Group == null)
        return;
      try
      {
        this.m_isRecalculating = true;
        if (!this.SupportsOphrans && !(!this.TryReleaseNode(parent) & !this.TryReleaseNode(child)))
          return;
        this.AddNeighbours(this.m_disconnectHelper, parent);
        if (this.m_disconnectHelper.Contains(child))
          return;
        if ((double) this.m_disconnectHelper.Count > (double) parent.Group.m_members.Count / 2.0)
        {
          foreach (MyGroups<TNode, TGroupData>.Node member in parent.Group.m_members)
          {
            if (!this.m_disconnectHelper.Add(member))
              this.m_disconnectHelper.Remove(member);
          }
        }
        MyGroups<TNode, TGroupData>.Group group = this.AcquireGroup();
        foreach (MyGroups<TNode, TGroupData>.Node node in this.m_disconnectHelper)
        {
          if (node.m_group != null && node.m_group.m_members != null)
          {
            node.m_group.m_members.Remove(node);
            node.m_group = group;
            group.m_members.Add(node);
          }
        }
      }
      finally
      {
        this.m_disconnectHelper.Clear();
        this.m_isRecalculating = false;
      }
    }

    public static bool IsMajorGroup(
      MyGroups<TNode, TGroupData>.Group groupA,
      MyGroups<TNode, TGroupData>.Group groupB)
    {
      return groupA.m_members.Count >= groupB.m_members.Count;
    }

    private void MergeGroups(
      MyGroups<TNode, TGroupData>.Group groupA,
      MyGroups<TNode, TGroupData>.Group groupB)
    {
      if (!this.m_groupSelector(groupA, groupB))
      {
        MyGroups<TNode, TGroupData>.Group group = groupA;
        groupA = groupB;
        groupB = group;
      }
      if (this.m_tmpList.Capacity < groupB.m_members.Count)
        this.m_tmpList.Capacity = groupB.m_members.Count;
      this.m_tmpList.AddRange((IEnumerable<MyGroups<TNode, TGroupData>.Node>) groupB.m_members);
      foreach (MyGroups<TNode, TGroupData>.Node tmp in this.m_tmpList)
      {
        groupB.m_members.Remove(tmp);
        tmp.m_group = groupA;
        groupA.m_members.Add(tmp);
      }
      this.m_tmpList.Clear();
      groupB.m_members.Clear();
      this.ReturnGroup(groupB);
    }

    private void AddLink(
      long linkId,
      MyGroups<TNode, TGroupData>.Node parent,
      MyGroups<TNode, TGroupData>.Node child)
    {
      parent.m_children[linkId] = child;
      child.m_parents[linkId] = parent;
    }

    private MyGroups<TNode, TGroupData>.Node GetOrCreateNode(TNode nodeData)
    {
      MyGroups<TNode, TGroupData>.Node node;
      if (!this.m_nodes.TryGetValue(nodeData, out node))
      {
        node = this.AcquireNode();
        node.m_node = nodeData;
        this.m_nodes[nodeData] = node;
      }
      return node;
    }

    private MyGroups<TNode, TGroupData>.Group GetNodeOrNull(TNode Node)
    {
      MyGroups<TNode, TGroupData>.Node node;
      this.m_nodes.TryGetValue(Node, out node);
      return node?.m_group;
    }

    private MyGroups<TNode, TGroupData>.Group AcquireGroup()
    {
      MyGroups<TNode, TGroupData>.Group group = this.m_groupPool.Count > 0 ? this.m_groupPool.Pop() : new MyGroups<TNode, TGroupData>.Group();
      this.m_groups.Add(group);
      group.GroupData.OnCreate<TGroupData>(group);
      return group;
    }

    private void ReturnGroup(MyGroups<TNode, TGroupData>.Group group)
    {
      group.GroupData.OnRelease();
      this.m_groups.Remove(group);
      this.m_groupPool.Push(group);
    }

    private MyGroups<TNode, TGroupData>.Node AcquireNode() => this.m_nodePool.Count > 0 ? this.m_nodePool.Pop() : new MyGroups<TNode, TGroupData>.Node();

    private void ReturnNode(MyGroups<TNode, TGroupData>.Node node) => this.m_nodePool.Push(node);

    public override List<TNode> GetGroupNodes(TNode nodeInGroup)
    {
      MyGroups<TNode, TGroupData>.Group group = this.GetGroup(nodeInGroup);
      if (group != null)
      {
        List<TNode> nodeList = new List<TNode>(group.Nodes.Count);
        foreach (MyGroups<TNode, TGroupData>.Node node in group.Nodes)
          nodeList.Add(node.NodeData);
        return nodeList;
      }
      return new List<TNode>(1) { nodeInGroup };
    }

    public override void GetGroupNodes(TNode nodeInGroup, List<TNode> result)
    {
      MyGroups<TNode, TGroupData>.Group group = this.GetGroup(nodeInGroup);
      if (group != null)
      {
        foreach (MyGroups<TNode, TGroupData>.Node node in group.Nodes)
          result.Add(node.NodeData);
      }
      else
        result.Add(nodeInGroup);
    }

    public void ReplaceRoot(TNode newRoot)
    {
      foreach (MyGroups<TNode, TGroupData>.Node member in this.GetGroup(newRoot).m_members)
      {
        foreach (KeyValuePair<long, MyGroups<TNode, TGroupData>.Node> parent in member.m_parents)
          member.m_children[parent.Key] = parent.Value;
        member.m_parents.Clear();
      }
      MyGroups<TNode, TGroupData>.Node node = this.GetNode(newRoot);
      node.ChainLength = 0;
      this.ReplaceParents(node);
    }

    private void ReplaceParents(MyGroups<TNode, TGroupData>.Node newParent)
    {
      this.m_tmpOpen.Enqueue(newParent);
      this.m_tmpClosed.Add(newParent);
      while (this.m_tmpOpen.Count > 0)
      {
        MyGroups<TNode, TGroupData>.Node node = this.m_tmpOpen.Dequeue();
        foreach (KeyValuePair<long, MyGroups<TNode, TGroupData>.Node> child in node.m_children)
        {
          child.Value.ChainLength = node.ChainLength + 1;
          if (!this.m_tmpClosed.Contains(child.Value) && !child.Value.m_parents.ContainsKey(child.Key))
          {
            child.Value.m_parents.Add(child.Key, node);
            child.Value.m_children.Remove(child.Key);
            this.m_tmpOpen.Enqueue(child.Value);
            this.m_tmpClosed.Add(child.Value);
          }
        }
      }
      this.m_tmpOpen.Clear();
      this.m_tmpClosed.Clear();
    }

    public delegate bool MajorGroupComparer(
      MyGroups<TNode, TGroupData>.Group major,
      MyGroups<TNode, TGroupData>.Group minor)
      where TNode : class
      where TGroupData : IGroupData<TNode>, new();

    public class Node
    {
      private MyGroups<TNode, TGroupData>.Group m_currentGroup;
      internal TNode m_node;
      internal readonly SortedDictionary<long, MyGroups<TNode, TGroupData>.Node> m_children = new SortedDictionary<long, MyGroups<TNode, TGroupData>.Node>();
      internal readonly Dictionary<long, MyGroups<TNode, TGroupData>.Node> m_parents = new Dictionary<long, MyGroups<TNode, TGroupData>.Node>();

      internal MyGroups<TNode, TGroupData>.Group m_group
      {
        get => this.m_currentGroup;
        set
        {
          this.m_currentGroup?.GroupData.OnNodeRemoved(this.m_node);
          this.m_currentGroup = value;
          if (this.m_currentGroup == null)
            return;
          this.m_currentGroup.GroupData.OnNodeAdded(this.m_node);
        }
      }

      public int LinkCount => this.m_children.Count + this.m_parents.Count;

      public TNode NodeData => this.m_node;

      public MyGroups<TNode, TGroupData>.Group Group => this.m_group;

      public int ChainLength { get; set; }

      public SortedDictionaryValuesReader<long, MyGroups<TNode, TGroupData>.Node> Children => new SortedDictionaryValuesReader<long, MyGroups<TNode, TGroupData>.Node>(this.m_children);

      public SortedDictionaryReader<long, MyGroups<TNode, TGroupData>.Node> ChildLinks => new SortedDictionaryReader<long, MyGroups<TNode, TGroupData>.Node>(this.m_children);

      public DictionaryReader<long, MyGroups<TNode, TGroupData>.Node> ParentLinks => new DictionaryReader<long, MyGroups<TNode, TGroupData>.Node>(this.m_parents);

      public override string ToString() => this.m_node.ToString();
    }

    public class Group
    {
      internal readonly HashSet<MyGroups<TNode, TGroupData>.Node> m_members = new HashSet<MyGroups<TNode, TGroupData>.Node>();
      public readonly TGroupData GroupData = new TGroupData();

      public HashSetReader<MyGroups<TNode, TGroupData>.Node> Nodes => new HashSetReader<MyGroups<TNode, TGroupData>.Node>(this.m_members);
    }
  }
}
