// Decompiled with JetBrains decompiler
// Type: VRage.Collections.MySinglyLinkedList`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
  public class MySinglyLinkedList<V> : IList<V>, ICollection<V>, IEnumerable<V>, IEnumerable
  {
    private MySinglyLinkedList<V>.Node m_rootNode;
    private MySinglyLinkedList<V>.Node m_lastNode;
    private int m_count;

    public MySinglyLinkedList()
    {
      this.m_rootNode = (MySinglyLinkedList<V>.Node) null;
      this.m_lastNode = (MySinglyLinkedList<V>.Node) null;
      this.m_count = 0;
    }

    public int IndexOf(V item)
    {
      int num = 0;
      foreach (V v in this)
      {
        if (v.Equals((object) item))
          return num;
        ++num;
      }
      return -1;
    }

    public void Insert(int index, V item)
    {
      if (index < 0 || index > this.m_count)
        throw new IndexOutOfRangeException();
      if (index == 0)
        this.Prepend(item);
      else if (index == this.m_count)
      {
        this.Add(item);
      }
      else
      {
        MySinglyLinkedList<V>.Enumerator enumerator = this.GetEnumerator();
        for (int index1 = 0; index1 < index; ++index1)
          enumerator.MoveNext();
        MySinglyLinkedList<V>.Node node = new MySinglyLinkedList<V>.Node(enumerator.m_currentNode.Next, item);
        enumerator.m_currentNode.Next = node;
        ++this.m_count;
      }
    }

    public void RemoveAt(int index)
    {
      if (index < 0 || index >= this.m_count)
        throw new IndexOutOfRangeException();
      if (index == 0)
      {
        this.m_rootNode = this.m_rootNode.Next;
        --this.m_count;
        if (this.m_count != 0)
          return;
        this.m_lastNode = (MySinglyLinkedList<V>.Node) null;
      }
      else
      {
        MySinglyLinkedList<V>.Enumerator enumerator = this.GetEnumerator();
        for (int index1 = 0; index1 < index; ++index1)
          enumerator.MoveNext();
        enumerator.m_currentNode.Next = enumerator.m_currentNode.Next.Next;
        --this.m_count;
        if (this.m_count != index)
          return;
        this.m_lastNode = enumerator.m_currentNode;
      }
    }

    public MySinglyLinkedList<V> Split(
      MySinglyLinkedList<V>.Enumerator newLastPosition,
      int newCount = -1)
    {
      if (newCount == -1)
      {
        newCount = 1;
        for (MySinglyLinkedList<V>.Node node = this.m_rootNode; node != newLastPosition.m_currentNode; node = node.Next)
          ++newCount;
      }
      MySinglyLinkedList<V> singlyLinkedList = new MySinglyLinkedList<V>()
      {
        m_rootNode = newLastPosition.m_currentNode.Next
      };
      singlyLinkedList.m_lastNode = singlyLinkedList.m_rootNode == null ? (MySinglyLinkedList<V>.Node) null : this.m_lastNode;
      singlyLinkedList.m_count = this.m_count - newCount;
      this.m_lastNode = newLastPosition.m_currentNode;
      this.m_lastNode.Next = (MySinglyLinkedList<V>.Node) null;
      this.m_count = newCount;
      return singlyLinkedList;
    }

    public V this[int index]
    {
      get
      {
        if (index < 0 || index >= this.m_count)
          throw new IndexOutOfRangeException();
        MySinglyLinkedList<V>.Enumerator enumerator = this.GetEnumerator();
        for (int index1 = -1; index1 < index; ++index1)
          enumerator.MoveNext();
        return enumerator.Current;
      }
      set
      {
        if (index < 0 || index >= this.m_count)
          throw new IndexOutOfRangeException();
        MySinglyLinkedList<V>.Enumerator enumerator = this.GetEnumerator();
        for (int index1 = -1; index1 < index; ++index1)
          enumerator.MoveNext();
        enumerator.m_currentNode.Data = value;
      }
    }

    public void Add(V item)
    {
      if (this.m_lastNode == null)
      {
        this.Prepend(item);
      }
      else
      {
        this.m_lastNode.Next = new MySinglyLinkedList<V>.Node((MySinglyLinkedList<V>.Node) null, item);
        ++this.m_count;
        this.m_lastNode = this.m_lastNode.Next;
      }
    }

    public void Append(V item) => this.Add(item);

    public void Prepend(V item)
    {
      this.m_rootNode = new MySinglyLinkedList<V>.Node(this.m_rootNode, item);
      ++this.m_count;
      if (this.m_count != 1)
        return;
      this.m_lastNode = this.m_rootNode;
    }

    public void Merge(MySinglyLinkedList<V> otherList)
    {
      if (this.m_lastNode == null)
      {
        this.m_rootNode = otherList.m_rootNode;
        this.m_lastNode = otherList.m_lastNode;
      }
      else if (otherList.m_lastNode != null)
      {
        this.m_lastNode.Next = otherList.m_rootNode;
        this.m_lastNode = otherList.m_lastNode;
      }
      this.m_count += otherList.m_count;
      otherList.m_count = 0;
      otherList.m_lastNode = (MySinglyLinkedList<V>.Node) null;
      otherList.m_rootNode = (MySinglyLinkedList<V>.Node) null;
    }

    public V PopFirst()
    {
      if (this.m_count == 0)
        throw new InvalidOperationException();
      MySinglyLinkedList<V>.Node rootNode = this.m_rootNode;
      if (rootNode == this.m_lastNode)
        this.m_lastNode = (MySinglyLinkedList<V>.Node) null;
      this.m_rootNode = rootNode.Next;
      --this.m_count;
      return rootNode.Data;
    }

    public V First()
    {
      if (this.m_count == 0)
        throw new InvalidOperationException();
      return this.m_rootNode.Data;
    }

    public V Last()
    {
      if (this.m_count == 0)
        throw new InvalidOperationException();
      return this.m_lastNode.Data;
    }

    public void Clear()
    {
      this.m_rootNode = (MySinglyLinkedList<V>.Node) null;
      this.m_lastNode = (MySinglyLinkedList<V>.Node) null;
      this.m_count = 0;
    }

    public bool Contains(V item)
    {
      foreach (V v in this)
      {
        if (v.Equals((object) item))
          return true;
      }
      return false;
    }

    public void CopyTo(V[] array, int arrayIndex)
    {
      foreach (V v in this)
      {
        array[arrayIndex] = v;
        ++arrayIndex;
      }
    }

    public int Count => this.m_count;

    public void Reverse()
    {
      if (this.m_count <= 1)
        return;
      MySinglyLinkedList<V>.Node node1 = (MySinglyLinkedList<V>.Node) null;
      MySinglyLinkedList<V>.Node next;
      for (MySinglyLinkedList<V>.Node node2 = this.m_rootNode; node2 != this.m_lastNode; node2 = next)
      {
        next = node2.Next;
        node2.Next = node1;
        node1 = node2;
      }
      MySinglyLinkedList<V>.Node rootNode = this.m_rootNode;
      this.m_rootNode = this.m_lastNode;
      this.m_lastNode = rootNode;
    }

    public bool IsReadOnly => throw new NotImplementedException();

    public bool VerifyConsistency()
    {
      bool flag = true;
      if (this.m_lastNode == null)
        flag = flag && this.m_rootNode == null && this.m_count == 0;
      if (this.m_rootNode == null)
        flag = flag && this.m_lastNode == null && this.m_count == 0;
      if (this.m_rootNode == this.m_lastNode)
        flag = flag && (this.m_rootNode == null || this.m_count == 1);
      int num = 0;
      MySinglyLinkedList<V>.Node node = this.m_rootNode;
      while (node != null)
      {
        node = node.Next;
        ++num;
        flag = flag && num <= this.m_count;
      }
      return flag && num == this.m_count;
    }

    public bool Remove(V item)
    {
      MySinglyLinkedList<V>.Node node = this.m_rootNode;
      if (node == null)
        return false;
      if (this.m_rootNode.Data.Equals((object) item))
      {
        this.m_rootNode = this.m_rootNode.Next;
        --this.m_count;
        if (this.m_count == 0)
          this.m_lastNode = (MySinglyLinkedList<V>.Node) null;
        return true;
      }
      for (MySinglyLinkedList<V>.Node next = node.Next; next != null; next = next.Next)
      {
        if (next.Data.Equals((object) item))
        {
          node.Next = next.Next;
          --this.m_count;
          if (next == this.m_lastNode)
            this.m_lastNode = node;
          return true;
        }
        node = next;
      }
      return false;
    }

    public MySinglyLinkedList<V>.Enumerator GetEnumerator() => new MySinglyLinkedList<V>.Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new MySinglyLinkedList<V>.Enumerator(this);

    IEnumerator<V> IEnumerable<V>.GetEnumerator() => (IEnumerator<V>) new MySinglyLinkedList<V>.Enumerator(this);

    internal class Node
    {
      public MySinglyLinkedList<V>.Node Next;
      public V Data;

      public Node(MySinglyLinkedList<V>.Node next, V data)
      {
        this.Next = next;
        this.Data = data;
      }
    }

    public struct Enumerator : IEnumerator<V>, IEnumerator, IDisposable
    {
      internal MySinglyLinkedList<V>.Node m_previousNode;
      internal MySinglyLinkedList<V>.Node m_currentNode;
      internal MySinglyLinkedList<V> m_list;

      public Enumerator(MySinglyLinkedList<V> parentList)
      {
        this.m_list = parentList;
        this.m_currentNode = (MySinglyLinkedList<V>.Node) null;
        this.m_previousNode = (MySinglyLinkedList<V>.Node) null;
      }

      public V Current => this.m_currentNode.Data;

      public bool HasCurrent => this.m_currentNode != null;

      public void Dispose()
      {
      }

      object IEnumerator.Current => (object) this.m_currentNode.Data;

      public bool MoveNext()
      {
        if (this.m_currentNode == null)
        {
          if (this.m_previousNode != null)
            return false;
          this.m_currentNode = this.m_list.m_rootNode;
          this.m_previousNode = (MySinglyLinkedList<V>.Node) null;
        }
        else
        {
          this.m_previousNode = this.m_currentNode;
          this.m_currentNode = this.m_currentNode.Next;
        }
        return this.m_currentNode != null;
      }

      public V RemoveCurrent()
      {
        if (this.m_currentNode == null)
          throw new InvalidOperationException();
        if (this.m_previousNode == null)
        {
          this.m_currentNode = this.m_currentNode.Next;
          return this.m_list.PopFirst();
        }
        this.m_previousNode.Next = this.m_currentNode.Next;
        if (this.m_list.m_lastNode == this.m_currentNode)
          this.m_list.m_lastNode = this.m_previousNode;
        MySinglyLinkedList<V>.Node currentNode = this.m_currentNode;
        this.m_currentNode = this.m_currentNode.Next;
        --this.m_list.m_count;
        return currentNode.Data;
      }

      public void InsertBeforeCurrent(V toInsert)
      {
        MySinglyLinkedList<V>.Node node = new MySinglyLinkedList<V>.Node(this.m_currentNode, toInsert);
        if (this.m_currentNode == null)
        {
          if (this.m_previousNode == null)
          {
            if (this.m_list.m_count != 0)
              throw new InvalidOperationException("Inserting into a MySinglyLinkedList using an uninitialized enumerator!");
            this.m_list.m_rootNode = node;
            this.m_list.m_lastNode = node;
          }
          else
          {
            this.m_previousNode.Next = node;
            this.m_list.m_lastNode = node;
          }
        }
        else if (this.m_previousNode == null)
          this.m_list.m_rootNode = node;
        else
          this.m_previousNode.Next = node;
        this.m_previousNode = node;
        ++this.m_list.m_count;
      }

      public void Reset()
      {
        this.m_currentNode = (MySinglyLinkedList<V>.Node) null;
        this.m_previousNode = (MySinglyLinkedList<V>.Node) null;
      }
    }
  }
}
