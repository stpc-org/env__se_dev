// Decompiled with JetBrains decompiler
// Type: VRage.Library.FastPriorityQueue`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Library
{
  public sealed class FastPriorityQueue<T> : IEnumerable where T : FastPriorityQueue<T>.Node
  {
    private int m_numNodes;
    private T[] m_nodes;

    public FastPriorityQueue(int maxNodes)
    {
      this.m_numNodes = 0;
      this.m_nodes = new T[maxNodes + 1];
    }

    public int Count => this.m_numNodes;

    public int MaxSize => this.m_nodes.Length - 1;

    public void Clear()
    {
      Array.Clear((Array) this.m_nodes, 1, this.m_numNodes);
      this.m_numNodes = 0;
    }

    public bool Contains(T node) => (object) this.m_nodes[node.QueueIndex] == (object) node;

    public void Enqueue(T node, long priority)
    {
      if (this.m_numNodes >= this.m_nodes.Length - 1)
        this.Resize(this.m_numNodes > 0 ? this.m_numNodes * 2 : 16);
      node.Priority = priority;
      ++this.m_numNodes;
      this.m_nodes[this.m_numNodes] = node;
      node.QueueIndex = this.m_numNodes;
      this.CascadeUp(node);
    }

    private void CascadeUp(T node)
    {
      if (node.QueueIndex <= 1)
        return;
      int index = node.QueueIndex >> 1;
      T node1 = this.m_nodes[index];
      if (this.HasHigherOrEqualPriority(node1, node))
        return;
      this.m_nodes[node.QueueIndex] = node1;
      node1.QueueIndex = node.QueueIndex;
      node.QueueIndex = index;
      while (index > 1)
      {
        index >>= 1;
        T node2 = this.m_nodes[index];
        if (!this.HasHigherOrEqualPriority(node2, node))
        {
          this.m_nodes[node.QueueIndex] = node2;
          node2.QueueIndex = node.QueueIndex;
          node.QueueIndex = index;
        }
        else
          break;
      }
      this.m_nodes[node.QueueIndex] = node;
    }

    private void CascadeDown(T node)
    {
      int queueIndex = node.QueueIndex;
      int index1 = 2 * queueIndex;
      if (index1 > this.m_numNodes)
        return;
      int index2 = index1 + 1;
      T node1 = this.m_nodes[index1];
      int index3;
      if (node1.Priority < node.Priority)
      {
        if (index2 > this.m_numNodes)
        {
          node.QueueIndex = index1;
          node1.QueueIndex = queueIndex;
          this.m_nodes[queueIndex] = node1;
          this.m_nodes[index1] = node;
          return;
        }
        T node2 = this.m_nodes[index2];
        if (node1.Priority < node2.Priority)
        {
          node1.QueueIndex = queueIndex;
          this.m_nodes[queueIndex] = node1;
          index3 = index1;
        }
        else
        {
          node2.QueueIndex = queueIndex;
          this.m_nodes[queueIndex] = node2;
          index3 = index2;
        }
      }
      else
      {
        if (index2 > this.m_numNodes)
          return;
        T node2 = this.m_nodes[index2];
        if (node2.Priority >= node.Priority)
          return;
        node2.QueueIndex = queueIndex;
        this.m_nodes[queueIndex] = node2;
        index3 = index2;
      }
      int index4;
      T node3;
      while (true)
      {
        index4 = 2 * index3;
        if (index4 <= this.m_numNodes)
        {
          int index5 = index4 + 1;
          node3 = this.m_nodes[index4];
          if (node3.Priority < node.Priority)
          {
            if (index5 <= this.m_numNodes)
            {
              T node2 = this.m_nodes[index5];
              if (node3.Priority < node2.Priority)
              {
                node3.QueueIndex = index3;
                this.m_nodes[index3] = node3;
                index3 = index4;
              }
              else
              {
                node2.QueueIndex = index3;
                this.m_nodes[index3] = node2;
                index3 = index5;
              }
            }
            else
              goto label_16;
          }
          else if (index5 <= this.m_numNodes)
          {
            T node2 = this.m_nodes[index5];
            if (node2.Priority < node.Priority)
            {
              node2.QueueIndex = index3;
              this.m_nodes[index3] = node2;
              index3 = index5;
            }
            else
              goto label_24;
          }
          else
            goto label_21;
        }
        else
          break;
      }
      node.QueueIndex = index3;
      this.m_nodes[index3] = node;
      return;
label_16:
      node.QueueIndex = index4;
      node3.QueueIndex = index3;
      this.m_nodes[index3] = node3;
      this.m_nodes[index4] = node;
      return;
label_21:
      node.QueueIndex = index3;
      this.m_nodes[index3] = node;
      return;
label_24:
      node.QueueIndex = index3;
      this.m_nodes[index3] = node;
    }

    private bool HasHigherOrEqualPriority(T higher, T lower) => higher.Priority <= lower.Priority;

    public T Dequeue()
    {
      T node1 = this.m_nodes[1];
      if (this.m_numNodes == 1)
      {
        this.m_nodes[1] = default (T);
        this.m_numNodes = 0;
        return node1;
      }
      T node2 = this.m_nodes[this.m_numNodes];
      this.m_nodes[1] = node2;
      node2.QueueIndex = 1;
      this.m_nodes[this.m_numNodes] = default (T);
      --this.m_numNodes;
      this.CascadeDown(node2);
      return node1;
    }

    public void Resize(int maxNodes)
    {
      T[] objArray = new T[maxNodes + 1];
      int num = Math.Min(maxNodes, this.m_numNodes);
      Array.Copy((Array) this.m_nodes, (Array) objArray, num + 1);
      this.m_nodes = objArray;
    }

    public T First => this.m_nodes[1];

    public void UpdatePriority(T node, long priority)
    {
      node.Priority = priority;
      this.OnNodeUpdated(node);
    }

    private void OnNodeUpdated(T node)
    {
      int index = node.QueueIndex >> 1;
      if (index > 0 && node.Priority < this.m_nodes[index].Priority)
        this.CascadeUp(node);
      else
        this.CascadeDown(node);
    }

    public void Remove(T node)
    {
      if (node.QueueIndex == this.m_numNodes)
      {
        this.m_nodes[this.m_numNodes] = default (T);
        --this.m_numNodes;
      }
      else
      {
        T node1 = this.m_nodes[this.m_numNodes];
        this.m_nodes[node.QueueIndex] = node1;
        node1.QueueIndex = node.QueueIndex;
        this.m_nodes[this.m_numNodes] = default (T);
        --this.m_numNodes;
        this.OnNodeUpdated(node1);
      }
    }

    public IEnumerator<T> GetEnumerator()
    {
      for (int i = 1; i <= this.m_numNodes; ++i)
        yield return this.m_nodes[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public bool IsValidQueue()
    {
      for (int index1 = 1; index1 < this.m_nodes.Length; ++index1)
      {
        if ((object) this.m_nodes[index1] != null)
        {
          int index2 = 2 * index1;
          if (index2 < this.m_nodes.Length && (object) this.m_nodes[index2] != null && this.m_nodes[index2].Priority < this.m_nodes[index1].Priority)
            return false;
          int index3 = index2 + 1;
          if (index3 < this.m_nodes.Length && (object) this.m_nodes[index3] != null && this.m_nodes[index3].Priority < this.m_nodes[index1].Priority)
            return false;
        }
      }
      return true;
    }

    public class Node
    {
      internal long Priority;
      internal int QueueIndex;
    }
  }
}
