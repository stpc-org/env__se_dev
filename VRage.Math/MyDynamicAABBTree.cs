// Decompiled with JetBrains decompiler
// Type: VRageMath.MyDynamicAABBTree
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;

namespace VRageMath
{
  public class MyDynamicAABBTree
  {
    public const int NullNode = -1;
    private int m_freeList;
    private int m_nodeCapacity;
    private int m_nodeCount;
    private MyDynamicAABBTree.DynamicTreeNode[] m_nodes;
    private Dictionary<int, MyDynamicAABBTree.DynamicTreeNode> m_leafElementCache;
    private int m_root;
    [ThreadStatic]
    private static Stack<int> m_queryStack;
    private static List<Stack<int>> m_StackCacheCollection = new List<Stack<int>>();
    private Vector3 m_extension;
    private float m_aabbMultiplier;
    private FastResourceLock m_rwLock = new FastResourceLock();

    public DictionaryValuesReader<int, MyDynamicAABBTree.DynamicTreeNode> Leaves => new DictionaryValuesReader<int, MyDynamicAABBTree.DynamicTreeNode>(this.m_leafElementCache);

    public ReadOnlySpan<MyDynamicAABBTree.DynamicTreeNode> Nodes => new ReadOnlySpan<MyDynamicAABBTree.DynamicTreeNode>(this.m_nodes, 0, this.m_nodeCapacity);

    private Stack<int> CurrentThreadStack
    {
      get
      {
        if (MyDynamicAABBTree.m_queryStack == null)
        {
          MyDynamicAABBTree.m_queryStack = new Stack<int>(32);
          lock (MyDynamicAABBTree.m_StackCacheCollection)
            MyDynamicAABBTree.m_StackCacheCollection.Add(MyDynamicAABBTree.m_queryStack);
        }
        return MyDynamicAABBTree.m_queryStack;
      }
    }

    public MyDynamicAABBTree() => this.Init(Vector3.One, 1f);

    public MyDynamicAABBTree(Vector3 extension, float aabbMultiplier = 1f) => this.Init(extension, aabbMultiplier);

    private void Init(Vector3 extension, float aabbMultiplier)
    {
      this.m_extension = extension;
      this.m_aabbMultiplier = aabbMultiplier;
      Stack<int> currentThreadStack = this.CurrentThreadStack;
      this.Clear();
    }

    private Stack<int> GetStack()
    {
      Stack<int> currentThreadStack = this.CurrentThreadStack;
      currentThreadStack.Clear();
      return currentThreadStack;
    }

    private void PushStack(Stack<int> stack)
    {
    }

    public int AddProxy(ref BoundingBox aabb, object userData, uint userFlags, bool rebalance = true)
    {
      using (this.m_rwLock.AcquireExclusiveUsing())
      {
        int index = this.AllocateNode();
        this.m_nodes[index].Aabb = aabb;
        this.m_nodes[index].Aabb.Min -= this.m_extension;
        this.m_nodes[index].Aabb.Max += this.m_extension;
        this.m_nodes[index].UserData = userData;
        this.m_nodes[index].UserFlag = userFlags;
        this.m_nodes[index].Height = 0;
        this.m_leafElementCache[index] = this.m_nodes[index];
        this.InsertLeaf(index, rebalance);
        return index;
      }
    }

    public void RemoveProxy(int proxyId)
    {
      using (this.m_rwLock.AcquireExclusiveUsing())
      {
        this.m_leafElementCache.Remove(proxyId);
        this.RemoveLeaf(proxyId);
        this.FreeNode(proxyId);
      }
    }

    public bool MoveProxy(int proxyId, ref BoundingBox aabb, Vector3 displacement)
    {
      using (this.m_rwLock.AcquireExclusiveUsing())
      {
        if (this.m_nodes[proxyId].Aabb.Contains(aabb) == ContainmentType.Contains)
          return false;
        this.RemoveLeaf(proxyId);
        BoundingBox boundingBox = aabb;
        Vector3 extension = this.m_extension;
        boundingBox.Min -= extension;
        boundingBox.Max += extension;
        Vector3 vector3 = this.m_aabbMultiplier * displacement;
        if ((double) vector3.X < 0.0)
          boundingBox.Min.X += vector3.X;
        else
          boundingBox.Max.X += vector3.X;
        if ((double) vector3.Y < 0.0)
          boundingBox.Min.Y += vector3.Y;
        else
          boundingBox.Max.Y += vector3.Y;
        if ((double) vector3.Z < 0.0)
          boundingBox.Min.Z += vector3.Z;
        else
          boundingBox.Max.Z += vector3.Z;
        this.m_nodes[proxyId].Aabb = boundingBox;
        this.InsertLeaf(proxyId, true);
      }
      return true;
    }

    public T GetUserData<T>(int proxyId) => (T) this.m_nodes[proxyId].UserData;

    public int GetRoot() => this.m_root;

    public int GetLeafCount() => this.m_leafElementCache.Count;

    public int GetLeafCount(int proxyId)
    {
      int num = 0;
      Stack<int> stack = this.GetStack();
      stack.Push(proxyId);
      while (stack.Count > 0)
      {
        int index = stack.Pop();
        if (index != -1)
        {
          MyDynamicAABBTree.DynamicTreeNode node = this.m_nodes[index];
          if (node.IsLeaf())
          {
            ++num;
          }
          else
          {
            stack.Push(node.Child1);
            stack.Push(node.Child2);
          }
        }
      }
      this.PushStack(stack);
      return num;
    }

    public void GetNodeLeaves(int proxyId, List<int> children)
    {
      Stack<int> stack = this.GetStack();
      stack.Push(proxyId);
      while (stack.Count > 0)
      {
        int index = stack.Pop();
        if (index != -1)
        {
          MyDynamicAABBTree.DynamicTreeNode node = this.m_nodes[index];
          if (node.IsLeaf())
          {
            children.Add(index);
          }
          else
          {
            stack.Push(node.Child1);
            stack.Push(node.Child2);
          }
        }
      }
      this.PushStack(stack);
    }

    public BoundingBox GetAabb(int proxyId) => this.m_nodes[proxyId].Aabb;

    public void GetChildren(int proxyId, out int left, out int right)
    {
      left = this.m_nodes[proxyId].Child1;
      right = this.m_nodes[proxyId].Child2;
    }

    private uint GetUserFlag(int proxyId) => this.m_nodes[proxyId].UserFlag;

    public void GetFatAABB(int proxyId, out BoundingBox fatAABB)
    {
      using (this.m_rwLock.AcquireSharedUsing())
        fatAABB = this.m_nodes[proxyId].Aabb;
    }

    public void Query(Func<int, bool> callback, ref BoundingBox aabb)
    {
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        while (stack.Count > 0)
        {
          int index = stack.Pop();
          if (index != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node = this.m_nodes[index];
            if (node.Aabb.Intersects(aabb))
            {
              if (node.IsLeaf())
              {
                if (!callback(index))
                  break;
              }
              else
              {
                stack.Push(node.Child1);
                stack.Push(node.Child2);
              }
            }
          }
        }
      }
    }

    public int CountLeaves(int nodeId)
    {
      using (this.m_rwLock.AcquireSharedUsing())
      {
        if (nodeId == -1)
          return 0;
        MyDynamicAABBTree.DynamicTreeNode node = this.m_nodes[nodeId];
        return node.IsLeaf() ? 1 : this.CountLeaves(node.Child1) + this.CountLeaves(node.Child2);
      }
    }

    private int AllocateNode()
    {
      if (this.m_freeList == -1)
      {
        MyDynamicAABBTree.DynamicTreeNode[] nodes1 = this.m_nodes;
        this.m_nodeCapacity *= 2;
        this.m_nodes = new MyDynamicAABBTree.DynamicTreeNode[this.m_nodeCapacity];
        MyDynamicAABBTree.DynamicTreeNode[] nodes2 = this.m_nodes;
        int nodeCount1 = this.m_nodeCount;
        Array.Copy((Array) nodes1, (Array) nodes2, nodeCount1);
        for (int nodeCount2 = this.m_nodeCount; nodeCount2 < this.m_nodeCapacity - 1; ++nodeCount2)
          this.m_nodes[nodeCount2] = new MyDynamicAABBTree.DynamicTreeNode()
          {
            ParentOrNext = nodeCount2 + 1,
            Height = 1
          };
        this.m_nodes[this.m_nodeCapacity - 1] = new MyDynamicAABBTree.DynamicTreeNode()
        {
          ParentOrNext = -1,
          Height = 1
        };
        this.m_freeList = this.m_nodeCount;
      }
      int freeList = this.m_freeList;
      this.m_freeList = this.m_nodes[freeList].ParentOrNext;
      this.m_nodes[freeList].ParentOrNext = -1;
      this.m_nodes[freeList].Child1 = -1;
      this.m_nodes[freeList].Child2 = -1;
      this.m_nodes[freeList].Height = 0;
      this.m_nodes[freeList].UserData = (object) null;
      ++this.m_nodeCount;
      return freeList;
    }

    private void FreeNode(int nodeId)
    {
      this.m_nodes[nodeId].ParentOrNext = this.m_freeList;
      this.m_nodes[nodeId].Height = -1;
      this.m_nodes[nodeId].UserData = (object) null;
      this.m_freeList = nodeId;
      --this.m_nodeCount;
    }

    private void InsertLeaf(int leaf, bool rebalance)
    {
      if (this.m_root == -1)
      {
        this.m_root = leaf;
        this.m_nodes[this.m_root].ParentOrNext = -1;
      }
      else
      {
        BoundingBox aabb = this.m_nodes[leaf].Aabb;
        int index1 = this.m_root;
        while (!this.m_nodes[index1].IsLeaf())
        {
          int child1 = this.m_nodes[index1].Child1;
          int child2 = this.m_nodes[index1].Child2;
          if (rebalance)
          {
            float perimeter1 = this.m_nodes[index1].Aabb.Perimeter;
            float perimeter2 = BoundingBox.CreateMerged(this.m_nodes[index1].Aabb, aabb).Perimeter;
            double num1 = 2.0 * (double) perimeter2;
            float num2 = (float) (2.0 * ((double) perimeter2 - (double) perimeter1));
            float num3;
            if (this.m_nodes[child1].IsLeaf())
            {
              BoundingBox result;
              BoundingBox.CreateMerged(ref aabb, ref this.m_nodes[child1].Aabb, out result);
              num3 = result.Perimeter + num2;
            }
            else
            {
              BoundingBox result;
              BoundingBox.CreateMerged(ref aabb, ref this.m_nodes[child1].Aabb, out result);
              float perimeter3 = this.m_nodes[child1].Aabb.Perimeter;
              num3 = result.Perimeter - perimeter3 + num2;
            }
            float num4;
            if (this.m_nodes[child2].IsLeaf())
            {
              BoundingBox result;
              BoundingBox.CreateMerged(ref aabb, ref this.m_nodes[child2].Aabb, out result);
              num4 = result.Perimeter + num2;
            }
            else
            {
              BoundingBox result;
              BoundingBox.CreateMerged(ref aabb, ref this.m_nodes[child2].Aabb, out result);
              float perimeter3 = this.m_nodes[child2].Aabb.Perimeter;
              num4 = result.Perimeter - perimeter3 + num2;
            }
            double num5 = (double) num3;
            if (num1 >= num5 || (double) num3 >= (double) num4)
              index1 = (double) num3 >= (double) num4 ? child2 : child1;
            else
              break;
          }
          else
          {
            BoundingBox result1;
            BoundingBox.CreateMerged(ref aabb, ref this.m_nodes[child1].Aabb, out result1);
            BoundingBox result2;
            BoundingBox.CreateMerged(ref aabb, ref this.m_nodes[child2].Aabb, out result2);
            index1 = (double) (this.m_nodes[child1].Height + 1) * (double) result1.Perimeter >= (double) ((float) (this.m_nodes[child2].Height + 1) * result2.Perimeter) ? child2 : child1;
          }
        }
        int index2 = index1;
        int parentOrNext = this.m_nodes[index1].ParentOrNext;
        int index3 = this.AllocateNode();
        this.m_nodes[index3].ParentOrNext = parentOrNext;
        this.m_nodes[index3].UserData = (object) null;
        this.m_nodes[index3].Aabb = BoundingBox.CreateMerged(aabb, this.m_nodes[index2].Aabb);
        this.m_nodes[index3].Height = this.m_nodes[index2].Height + 1;
        if (parentOrNext != -1)
        {
          if (this.m_nodes[parentOrNext].Child1 == index2)
            this.m_nodes[parentOrNext].Child1 = index3;
          else
            this.m_nodes[parentOrNext].Child2 = index3;
          this.m_nodes[index3].Child1 = index2;
          this.m_nodes[index3].Child2 = leaf;
          this.m_nodes[index1].ParentOrNext = index3;
          this.m_nodes[leaf].ParentOrNext = index3;
        }
        else
        {
          this.m_nodes[index3].Child1 = index2;
          this.m_nodes[index3].Child2 = leaf;
          this.m_nodes[index1].ParentOrNext = index3;
          this.m_nodes[leaf].ParentOrNext = index3;
          this.m_root = index3;
        }
        for (int iA = this.m_nodes[leaf].ParentOrNext; iA != -1; iA = this.m_nodes[iA].ParentOrNext)
        {
          if (rebalance)
            iA = this.Balance(iA);
          int child1 = this.m_nodes[iA].Child1;
          int child2 = this.m_nodes[iA].Child2;
          this.m_nodes[iA].Height = 1 + Math.Max(this.m_nodes[child1].Height, this.m_nodes[child2].Height);
          BoundingBox.CreateMerged(ref this.m_nodes[child1].Aabb, ref this.m_nodes[child2].Aabb, out this.m_nodes[iA].Aabb);
        }
      }
    }

    private void RemoveLeaf(int leaf)
    {
      if (this.m_root == -1)
        return;
      if (leaf == this.m_root)
      {
        this.m_root = -1;
      }
      else
      {
        int parentOrNext1 = this.m_nodes[leaf].ParentOrNext;
        int parentOrNext2 = this.m_nodes[parentOrNext1].ParentOrNext;
        int index1 = this.m_nodes[parentOrNext1].Child1 != leaf ? this.m_nodes[parentOrNext1].Child1 : this.m_nodes[parentOrNext1].Child2;
        if (parentOrNext2 != -1)
        {
          if (this.m_nodes[parentOrNext2].Child1 == parentOrNext1)
            this.m_nodes[parentOrNext2].Child1 = index1;
          else
            this.m_nodes[parentOrNext2].Child2 = index1;
          this.m_nodes[index1].ParentOrNext = parentOrNext2;
          this.FreeNode(parentOrNext1);
          int index2;
          for (int iA = parentOrNext2; iA != -1; iA = this.m_nodes[index2].ParentOrNext)
          {
            index2 = this.Balance(iA);
            int child1 = this.m_nodes[index2].Child1;
            int child2 = this.m_nodes[index2].Child2;
            this.m_nodes[index2].Aabb = BoundingBox.CreateMerged(this.m_nodes[child1].Aabb, this.m_nodes[child2].Aabb);
            this.m_nodes[index2].Height = 1 + Math.Max(this.m_nodes[child1].Height, this.m_nodes[child2].Height);
          }
        }
        else
        {
          this.m_root = index1;
          this.m_nodes[index1].ParentOrNext = -1;
          this.FreeNode(parentOrNext1);
        }
      }
    }

    public int GetHeight()
    {
      using (this.m_rwLock.AcquireSharedUsing())
        return this.m_root == -1 ? 0 : this.m_nodes[this.m_root].Height;
    }

    public int Balance(int iA)
    {
      MyDynamicAABBTree.DynamicTreeNode node1 = this.m_nodes[iA];
      if (node1.IsLeaf() || node1.Height < 2)
        return iA;
      int child1_1 = node1.Child1;
      int child2_1 = node1.Child2;
      MyDynamicAABBTree.DynamicTreeNode node2 = this.m_nodes[child1_1];
      MyDynamicAABBTree.DynamicTreeNode node3 = this.m_nodes[child2_1];
      int num = node3.Height - node2.Height;
      if (num > 1)
      {
        int child1_2 = node3.Child1;
        int child2_2 = node3.Child2;
        MyDynamicAABBTree.DynamicTreeNode node4 = this.m_nodes[child1_2];
        MyDynamicAABBTree.DynamicTreeNode node5 = this.m_nodes[child2_2];
        node3.Child1 = iA;
        node3.ParentOrNext = node1.ParentOrNext;
        node1.ParentOrNext = child2_1;
        if (node3.ParentOrNext != -1)
        {
          if (this.m_nodes[node3.ParentOrNext].Child1 == iA)
            this.m_nodes[node3.ParentOrNext].Child1 = child2_1;
          else
            this.m_nodes[node3.ParentOrNext].Child2 = child2_1;
        }
        else
          this.m_root = child2_1;
        if (node4.Height > node5.Height)
        {
          node3.Child2 = child1_2;
          node1.Child2 = child2_2;
          node5.ParentOrNext = iA;
          BoundingBox.CreateMerged(ref node2.Aabb, ref node5.Aabb, out node1.Aabb);
          BoundingBox.CreateMerged(ref node1.Aabb, ref node4.Aabb, out node3.Aabb);
          node1.Height = 1 + Math.Max(node2.Height, node5.Height);
          node3.Height = 1 + Math.Max(node1.Height, node4.Height);
        }
        else
        {
          node3.Child2 = child2_2;
          node1.Child2 = child1_2;
          node4.ParentOrNext = iA;
          BoundingBox.CreateMerged(ref node2.Aabb, ref node4.Aabb, out node1.Aabb);
          BoundingBox.CreateMerged(ref node1.Aabb, ref node5.Aabb, out node3.Aabb);
          node1.Height = 1 + Math.Max(node2.Height, node4.Height);
          node3.Height = 1 + Math.Max(node1.Height, node5.Height);
        }
        return child2_1;
      }
      if (num >= -1)
        return iA;
      int child1_3 = node2.Child1;
      int child2_3 = node2.Child2;
      MyDynamicAABBTree.DynamicTreeNode node6 = this.m_nodes[child1_3];
      MyDynamicAABBTree.DynamicTreeNode node7 = this.m_nodes[child2_3];
      node2.Child1 = iA;
      node2.ParentOrNext = node1.ParentOrNext;
      node1.ParentOrNext = child1_1;
      if (node2.ParentOrNext != -1)
      {
        if (this.m_nodes[node2.ParentOrNext].Child1 == iA)
          this.m_nodes[node2.ParentOrNext].Child1 = child1_1;
        else
          this.m_nodes[node2.ParentOrNext].Child2 = child1_1;
      }
      else
        this.m_root = child1_1;
      if (node6.Height > node7.Height)
      {
        node2.Child2 = child1_3;
        node1.Child1 = child2_3;
        node7.ParentOrNext = iA;
        BoundingBox.CreateMerged(ref node3.Aabb, ref node7.Aabb, out node1.Aabb);
        BoundingBox.CreateMerged(ref node1.Aabb, ref node6.Aabb, out node2.Aabb);
        node1.Height = 1 + Math.Max(node3.Height, node7.Height);
        node2.Height = 1 + Math.Max(node1.Height, node6.Height);
      }
      else
      {
        node2.Child2 = child2_3;
        node1.Child1 = child1_3;
        node6.ParentOrNext = iA;
        BoundingBox.CreateMerged(ref node3.Aabb, ref node6.Aabb, out node1.Aabb);
        BoundingBox.CreateMerged(ref node1.Aabb, ref node7.Aabb, out node2.Aabb);
        node1.Height = 1 + Math.Max(node3.Height, node6.Height);
        node2.Height = 1 + Math.Max(node1.Height, node7.Height);
      }
      return child1_1;
    }

    public void OverlapAllFrustum<T>(ref BoundingFrustum frustum, List<T> elementsList, bool clear = true) => this.OverlapAllFrustum<T>(ref frustum, elementsList, 0U, clear);

    public void OverlapAllFrustum<T>(
      ref BoundingFrustum frustum,
      List<T> elementsList,
      uint requiredFlags,
      bool clear = true)
    {
      if (clear)
        elementsList.Clear();
      if (this.m_root == -1)
        return;
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        while (stack.Count > 0)
        {
          int proxyId1 = stack.Pop();
          if (proxyId1 != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node1 = this.m_nodes[proxyId1];
            ContainmentType result;
            frustum.Contains(ref node1.Aabb, out result);
            switch (result)
            {
              case ContainmentType.Contains:
                int count = stack.Count;
                stack.Push(proxyId1);
                while (stack.Count > count)
                {
                  int proxyId2 = stack.Pop();
                  MyDynamicAABBTree.DynamicTreeNode node2 = this.m_nodes[proxyId2];
                  if (node2.IsLeaf())
                  {
                    if (((int) this.GetUserFlag(proxyId2) & (int) requiredFlags) == (int) requiredFlags)
                      elementsList.Add(this.GetUserData<T>(proxyId2));
                  }
                  else
                  {
                    if (node2.Child1 != -1)
                      stack.Push(node2.Child1);
                    if (node2.Child2 != -1)
                      stack.Push(node2.Child2);
                  }
                }
                continue;
              case ContainmentType.Intersects:
                if (node1.IsLeaf())
                {
                  if (((int) this.GetUserFlag(proxyId1) & (int) requiredFlags) == (int) requiredFlags)
                  {
                    elementsList.Add(this.GetUserData<T>(proxyId1));
                    continue;
                  }
                  continue;
                }
                stack.Push(node1.Child1);
                stack.Push(node1.Child2);
                continue;
              default:
                continue;
            }
          }
        }
        this.PushStack(stack);
      }
    }

    public void OverlapAllFrustum<T>(
      ref BoundingFrustum frustum,
      List<T> elementsList,
      List<bool> isInsideList,
      bool clear = true)
    {
      if (clear)
      {
        elementsList.Clear();
        isInsideList.Clear();
      }
      this.OverlapAllFrustum<T>(ref frustum, (Action<T, bool>) ((x, y) =>
      {
        elementsList.Add(x);
        isInsideList.Add(y);
      }));
    }

    public void OverlapAllFrustum<T>(ref BoundingFrustum frustum, Action<T, bool> add)
    {
      if (this.m_root == -1)
        return;
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        while (stack.Count > 0)
        {
          int proxyId1 = stack.Pop();
          if (proxyId1 != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node1 = this.m_nodes[proxyId1];
            ContainmentType result;
            frustum.Contains(ref node1.Aabb, out result);
            switch (result)
            {
              case ContainmentType.Contains:
                int count = stack.Count;
                stack.Push(proxyId1);
                while (stack.Count > count)
                {
                  int proxyId2 = stack.Pop();
                  MyDynamicAABBTree.DynamicTreeNode node2 = this.m_nodes[proxyId2];
                  if (node2.IsLeaf())
                  {
                    add(this.GetUserData<T>(proxyId2), true);
                  }
                  else
                  {
                    if (node2.Child1 != -1)
                      stack.Push(node2.Child1);
                    if (node2.Child2 != -1)
                      stack.Push(node2.Child2);
                  }
                }
                continue;
              case ContainmentType.Intersects:
                if (node1.IsLeaf())
                {
                  add(this.GetUserData<T>(proxyId1), false);
                  continue;
                }
                stack.Push(node1.Child1);
                stack.Push(node1.Child2);
                continue;
              default:
                continue;
            }
          }
        }
        this.PushStack(stack);
      }
    }

    public void OverlapAllFrustum<T, Op>(ref BoundingFrustum frustum, ref Op add) where Op : struct, AddOp<T>
    {
      if (this.m_root == -1)
        return;
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        while (stack.Count > 0)
        {
          int proxyId1 = stack.Pop();
          if (proxyId1 != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node1 = this.m_nodes[proxyId1];
            ContainmentType result;
            frustum.Contains(ref node1.Aabb, out result);
            switch (result)
            {
              case ContainmentType.Contains:
                int count = stack.Count;
                stack.Push(proxyId1);
                while (stack.Count > count)
                {
                  int proxyId2 = stack.Pop();
                  MyDynamicAABBTree.DynamicTreeNode node2 = this.m_nodes[proxyId2];
                  if (node2.IsLeaf())
                  {
                    add.Add(this.GetUserData<T>(proxyId2), true);
                  }
                  else
                  {
                    if (node2.Child1 != -1)
                      stack.Push(node2.Child1);
                    if (node2.Child2 != -1)
                      stack.Push(node2.Child2);
                  }
                }
                continue;
              case ContainmentType.Intersects:
                if (node1.IsLeaf())
                {
                  add.Add(this.GetUserData<T>(proxyId1), false);
                  continue;
                }
                stack.Push(node1.Child1);
                stack.Push(node1.Child2);
                continue;
              default:
                continue;
            }
          }
        }
        this.PushStack(stack);
      }
    }

    public void OverlapAllFrustum<T>(
      ref BoundingFrustum frustum,
      List<T> elementsList,
      List<bool> isInsideList,
      float tSqr,
      bool clear = true)
    {
      if (clear)
      {
        elementsList.Clear();
        isInsideList.Clear();
      }
      this.OverlapAllFrustum<T>(ref frustum, (Action<T, bool>) ((x, y) =>
      {
        elementsList.Add(x);
        isInsideList.Add(y);
      }), tSqr);
    }

    public void OverlapAllFrustum<T>(ref BoundingFrustum frustum, Action<T, bool> add, float tSqr)
    {
      if (this.m_root == -1)
        return;
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        while (stack.Count > 0)
        {
          int proxyId1 = stack.Pop();
          if (proxyId1 != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node1 = this.m_nodes[proxyId1];
            ContainmentType result;
            frustum.Contains(ref node1.Aabb, out result);
            Vector3 size;
            switch (result)
            {
              case ContainmentType.Contains:
                int count = stack.Count;
                stack.Push(proxyId1);
                while (stack.Count > count)
                {
                  int proxyId2 = stack.Pop();
                  MyDynamicAABBTree.DynamicTreeNode node2 = this.m_nodes[proxyId2];
                  if (node2.IsLeaf())
                  {
                    size = node1.Aabb.Size;
                    if ((double) size.LengthSquared() > (double) tSqr)
                      add(this.GetUserData<T>(proxyId2), true);
                  }
                  else
                  {
                    if (node2.Child1 != -1)
                      stack.Push(node2.Child1);
                    if (node2.Child2 != -1)
                      stack.Push(node2.Child2);
                  }
                }
                continue;
              case ContainmentType.Intersects:
                if (node1.IsLeaf())
                {
                  size = node1.Aabb.Size;
                  if ((double) size.LengthSquared() > (double) tSqr)
                  {
                    add(this.GetUserData<T>(proxyId1), false);
                    continue;
                  }
                  continue;
                }
                stack.Push(node1.Child1);
                stack.Push(node1.Child2);
                continue;
              default:
                continue;
            }
          }
        }
        this.PushStack(stack);
      }
    }

    public void OverlapAllFrustum<T, Op>(ref BoundingFrustum frustum, float tSqr, ref Op add) where Op : struct, AddOp<T>
    {
      if (this.m_root == -1)
        return;
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        while (stack.Count > 0)
        {
          int proxyId1 = stack.Pop();
          if (proxyId1 != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node1 = this.m_nodes[proxyId1];
            ContainmentType result;
            frustum.Contains(ref node1.Aabb, out result);
            Vector3 size;
            switch (result)
            {
              case ContainmentType.Contains:
                int count = stack.Count;
                stack.Push(proxyId1);
                while (stack.Count > count)
                {
                  int proxyId2 = stack.Pop();
                  MyDynamicAABBTree.DynamicTreeNode node2 = this.m_nodes[proxyId2];
                  if (node2.IsLeaf())
                  {
                    size = node1.Aabb.Size;
                    if ((double) size.LengthSquared() > (double) tSqr)
                      add.Add(this.GetUserData<T>(proxyId2), true);
                  }
                  else
                  {
                    if (node2.Child1 != -1)
                      stack.Push(node2.Child1);
                    if (node2.Child2 != -1)
                      stack.Push(node2.Child2);
                  }
                }
                continue;
              case ContainmentType.Intersects:
                if (node1.IsLeaf())
                {
                  size = node1.Aabb.Size;
                  if ((double) size.LengthSquared() > (double) tSqr)
                  {
                    add.Add(this.GetUserData<T>(proxyId1), false);
                    continue;
                  }
                  continue;
                }
                stack.Push(node1.Child1);
                stack.Push(node1.Child2);
                continue;
              default:
                continue;
            }
          }
        }
        this.PushStack(stack);
      }
    }

    public void OverlapAllFrustumConservative<T>(
      ref BoundingFrustum frustum,
      List<T> elementsList,
      uint requiredFlags,
      bool clear = true)
    {
      if (clear)
        elementsList.Clear();
      if (this.m_root == -1)
        return;
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        BoundingBox invalid = BoundingBox.CreateInvalid();
        invalid.Include(ref frustum);
        while (stack.Count > 0)
        {
          int proxyId1 = stack.Pop();
          if (proxyId1 != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node1 = this.m_nodes[proxyId1];
            if (node1.Aabb.Intersects(invalid))
            {
              ContainmentType result;
              frustum.Contains(ref node1.Aabb, out result);
              switch (result)
              {
                case ContainmentType.Contains:
                  int count = stack.Count;
                  stack.Push(proxyId1);
                  while (stack.Count > count)
                  {
                    int proxyId2 = stack.Pop();
                    MyDynamicAABBTree.DynamicTreeNode node2 = this.m_nodes[proxyId2];
                    if (node2.IsLeaf())
                    {
                      if (((int) this.GetUserFlag(proxyId2) & (int) requiredFlags) == (int) requiredFlags)
                        elementsList.Add(this.GetUserData<T>(proxyId2));
                    }
                    else
                    {
                      if (node2.Child1 != -1)
                        stack.Push(node2.Child1);
                      if (node2.Child2 != -1)
                        stack.Push(node2.Child2);
                    }
                  }
                  continue;
                case ContainmentType.Intersects:
                  if (node1.IsLeaf())
                  {
                    if (((int) this.GetUserFlag(proxyId1) & (int) requiredFlags) == (int) requiredFlags)
                    {
                      elementsList.Add(this.GetUserData<T>(proxyId1));
                      continue;
                    }
                    continue;
                  }
                  stack.Push(node1.Child1);
                  stack.Push(node1.Child2);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
        this.PushStack(stack);
      }
    }

    public void OverlapAllFrustumAny<T>(
      ref BoundingFrustum frustum,
      List<T> elementsList,
      bool clear = true)
    {
      if (clear)
        elementsList.Clear();
      if (this.m_root == -1)
        return;
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        while (stack.Count > 0)
        {
          int proxyId1 = stack.Pop();
          if (proxyId1 != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node1 = this.m_nodes[proxyId1];
            ContainmentType result;
            frustum.Contains(ref node1.Aabb, out result);
            switch (result)
            {
              case ContainmentType.Contains:
                int count = stack.Count;
                stack.Push(proxyId1);
                while (stack.Count > count)
                {
                  int proxyId2 = stack.Pop();
                  MyDynamicAABBTree.DynamicTreeNode node2 = this.m_nodes[proxyId2];
                  if (node2.IsLeaf())
                  {
                    T userData = this.GetUserData<T>(proxyId2);
                    elementsList.Add(userData);
                  }
                  else
                  {
                    if (node2.Child1 != -1)
                      stack.Push(node2.Child1);
                    if (node2.Child2 != -1)
                      stack.Push(node2.Child2);
                  }
                }
                continue;
              case ContainmentType.Intersects:
                if (node1.IsLeaf())
                {
                  T userData = this.GetUserData<T>(proxyId1);
                  elementsList.Add(userData);
                  continue;
                }
                stack.Push(node1.Child1);
                stack.Push(node1.Child2);
                continue;
              default:
                continue;
            }
          }
        }
        this.PushStack(stack);
      }
    }

    public void OverlapAllLineSegment<T>(
      ref Line line,
      List<MyLineSegmentOverlapResult<T>> elementsList)
    {
      this.OverlapAllLineSegment<T>(ref line, elementsList, 0U);
    }

    public void OverlapAllLineSegment<T>(
      ref Line line,
      List<MyLineSegmentOverlapResult<T>> elementsList,
      uint requiredFlags)
    {
      elementsList.Clear();
      if (this.m_root == -1)
        return;
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        BoundingBox invalid = BoundingBox.CreateInvalid();
        invalid.Include(ref line);
        Ray ray = new Ray(line.From, line.Direction);
        while (stack.Count > 0)
        {
          int proxyId = stack.Pop();
          if (proxyId != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node = this.m_nodes[proxyId];
            if (node.Aabb.Intersects(invalid))
            {
              float? nullable = node.Aabb.Intersects(ray);
              if (nullable.HasValue && (double) nullable.Value <= (double) line.Length && (double) nullable.Value >= 0.0)
              {
                if (node.IsLeaf())
                {
                  if (((int) this.GetUserFlag(proxyId) & (int) requiredFlags) == (int) requiredFlags)
                    elementsList.Add(new MyLineSegmentOverlapResult<T>()
                    {
                      Element = this.GetUserData<T>(proxyId),
                      Distance = (double) nullable.Value
                    });
                }
                else
                {
                  stack.Push(node.Child1);
                  stack.Push(node.Child2);
                }
              }
            }
          }
        }
        this.PushStack(stack);
      }
    }

    public void OverlapAllBoundingBox<T>(
      ref BoundingBox bbox,
      List<T> elementsList,
      uint requiredFlags = 0,
      bool clear = true)
    {
      if (clear)
        elementsList.Clear();
      if (this.m_root == -1)
        return;
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        while (stack.Count > 0)
        {
          int proxyId = stack.Pop();
          if (proxyId != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node = this.m_nodes[proxyId];
            if (node.Aabb.Intersects(bbox))
            {
              if (node.IsLeaf())
              {
                if (((int) this.GetUserFlag(proxyId) & (int) requiredFlags) == (int) requiredFlags)
                  elementsList.Add(this.GetUserData<T>(proxyId));
              }
              else
              {
                stack.Push(node.Child1);
                stack.Push(node.Child2);
              }
            }
          }
        }
        this.PushStack(stack);
      }
    }

    public bool OverlapsAnyLeafBoundingBox(ref BoundingBox bbox)
    {
      if (this.m_root == -1)
        return false;
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        while (stack.Count > 0)
        {
          int index = stack.Pop();
          if (index != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node = this.m_nodes[index];
            if (node.Aabb.Intersects(bbox))
            {
              if (node.IsLeaf())
                return true;
              stack.Push(node.Child1);
              stack.Push(node.Child2);
            }
          }
        }
        this.PushStack(stack);
      }
      return false;
    }

    public void OverlapSizeableClusters(
      ref BoundingBox bbox,
      List<BoundingBox> boundList,
      double minSize)
    {
      if (this.m_root == -1)
        return;
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        while (stack.Count > 0)
        {
          int index = stack.Pop();
          if (index != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node = this.m_nodes[index];
            if (node.Aabb.Intersects(bbox))
            {
              if (node.IsLeaf() || (double) node.Aabb.Size.Max() <= minSize)
              {
                boundList.Add(node.Aabb);
              }
              else
              {
                stack.Push(node.Child1);
                stack.Push(node.Child2);
              }
            }
          }
        }
        this.PushStack(stack);
      }
    }

    public void OverlapAllBoundingSphere<T>(
      ref BoundingSphere sphere,
      List<T> overlapElementsList,
      bool clear = true)
    {
      if (clear)
        overlapElementsList.Clear();
      if (this.m_root == -1)
        return;
      using (this.m_rwLock.AcquireSharedUsing())
      {
        Stack<int> stack = this.GetStack();
        stack.Push(this.m_root);
        while (stack.Count > 0)
        {
          int proxyId = stack.Pop();
          if (proxyId != -1)
          {
            MyDynamicAABBTree.DynamicTreeNode node = this.m_nodes[proxyId];
            if (node.Aabb.Intersects(sphere))
            {
              if (node.IsLeaf())
              {
                overlapElementsList.Add(this.GetUserData<T>(proxyId));
              }
              else
              {
                stack.Push(node.Child1);
                stack.Push(node.Child2);
              }
            }
          }
        }
        this.PushStack(stack);
      }
    }

    public void GetAll<T>(List<T> elementsList, bool clear, List<BoundingBox> boxsList = null)
    {
      if (clear)
      {
        elementsList.Clear();
        boxsList?.Clear();
      }
      using (this.m_rwLock.AcquireSharedUsing())
      {
        foreach (KeyValuePair<int, MyDynamicAABBTree.DynamicTreeNode> keyValuePair in this.m_leafElementCache)
          elementsList.Add((T) keyValuePair.Value.UserData);
        if (boxsList == null)
          return;
        foreach (KeyValuePair<int, MyDynamicAABBTree.DynamicTreeNode> keyValuePair in this.m_leafElementCache)
          boxsList.Add(keyValuePair.Value.Aabb);
      }
    }

    public void GetAllNodeBounds(List<BoundingBox> boxsList)
    {
      using (this.m_rwLock.AcquireSharedUsing())
      {
        int index1 = 0;
        for (int index2 = 0; index1 < this.m_nodeCapacity && index2 < this.m_nodeCount; ++index1)
        {
          if (this.m_nodes[index1].Height != -1)
          {
            ++index2;
            boxsList.Add(this.m_nodes[index1].Aabb);
          }
        }
      }
    }

    private void ResetNodes()
    {
      this.m_leafElementCache.Clear();
      this.m_root = -1;
      this.m_nodeCount = 0;
      for (int index = 0; index < this.m_nodeCapacity - 1; ++index)
      {
        this.m_nodes[index].ParentOrNext = index + 1;
        this.m_nodes[index].Height = 1;
        this.m_nodes[index].UserData = (object) null;
      }
      this.m_nodes[this.m_nodeCapacity - 1].ParentOrNext = -1;
      this.m_nodes[this.m_nodeCapacity - 1].Height = 1;
      this.m_freeList = 0;
    }

    public void Clear()
    {
      using (this.m_rwLock.AcquireExclusiveUsing())
      {
        if (this.m_nodeCapacity < 256 || this.m_nodeCapacity > 512)
        {
          this.m_nodeCapacity = 256;
          this.m_nodes = new MyDynamicAABBTree.DynamicTreeNode[this.m_nodeCapacity];
          this.m_leafElementCache = new Dictionary<int, MyDynamicAABBTree.DynamicTreeNode>(this.m_nodeCapacity / 4);
          for (int index = 0; index < this.m_nodeCapacity; ++index)
            this.m_nodes[index] = new MyDynamicAABBTree.DynamicTreeNode();
        }
        this.ResetNodes();
      }
    }

    public static void Dispose()
    {
      lock (MyDynamicAABBTree.m_StackCacheCollection)
        MyDynamicAABBTree.m_StackCacheCollection.Clear();
    }

    public class DynamicTreeNode
    {
      public BoundingBox Aabb;
      public int Child1;
      public int Child2;
      public int Height;
      public int ParentOrNext;
      public object UserData;
      public uint UserFlag;

      public bool IsLeaf() => this.Child1 == -1;
    }
  }
}
