// Decompiled with JetBrains decompiler
// Type: VRage.Replication.MyReplicablesBase
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage.Collections;
using VRage.Network;
using VRageMath;

namespace VRage.Replication
{
  public abstract class MyReplicablesBase
  {
    private static readonly HashSet<IMyReplicable> m_empty = new HashSet<IMyReplicable>();
    private readonly Stack<HashSet<IMyReplicable>> m_hashSetPool = new Stack<HashSet<IMyReplicable>>();
    private readonly ConcurrentDictionary<IMyReplicable, HashSet<IMyReplicable>> m_parentToChildren = new ConcurrentDictionary<IMyReplicable, HashSet<IMyReplicable>>();
    private readonly ConcurrentDictionary<IMyReplicable, IMyReplicable> m_childToParent = new ConcurrentDictionary<IMyReplicable, IMyReplicable>();
    private readonly Thread m_mainThread;

    public event Action<IMyReplicable> OnChildAdded;

    protected MyReplicablesBase(Thread mainThread) => this.m_mainThread = mainThread;

    public void GetAllChildren(IMyReplicable replicable, List<IMyReplicable> resultList)
    {
      foreach (IMyReplicable child in this.GetChildren(replicable))
      {
        resultList.Add(child);
        this.GetAllChildren(child, resultList);
      }
    }

    public void Add(IMyReplicable replicable, out IMyReplicable parent)
    {
      if (replicable.HasToBeChild && MyReplicablesBase.TryGetParent(replicable, out parent))
        this.AddChild(replicable, parent);
      else if (!replicable.HasToBeChild)
      {
        parent = (IMyReplicable) null;
        this.AddRoot(replicable);
      }
      else
        parent = (IMyReplicable) null;
    }

    public void RemoveHierarchy(IMyReplicable replicable)
    {
      HashSet<IMyReplicable> valueOrDefault = this.m_parentToChildren.GetValueOrDefault<IMyReplicable, HashSet<IMyReplicable>>(replicable, MyReplicablesBase.m_empty);
      while (valueOrDefault.Count > 0)
      {
        HashSet<IMyReplicable>.Enumerator enumerator = valueOrDefault.GetEnumerator();
        enumerator.MoveNext();
        this.RemoveHierarchy(enumerator.Current);
      }
      this.Remove(replicable);
    }

    private HashSet<IMyReplicable> Obtain() => this.m_hashSetPool.Count <= 0 ? new HashSet<IMyReplicable>() : this.m_hashSetPool.Pop();

    public HashSetReader<IMyReplicable> GetChildren(
      IMyReplicable replicable)
    {
      return (HashSetReader<IMyReplicable>) this.m_parentToChildren.GetValueOrDefault<IMyReplicable, HashSet<IMyReplicable>>(replicable, MyReplicablesBase.m_empty);
    }

    private static bool TryGetParent(IMyReplicable replicable, out IMyReplicable parent)
    {
      parent = replicable.GetParent();
      return parent != null;
    }

    public void Refresh(IMyReplicable replicable)
    {
      IMyReplicable parent1;
      if (replicable.HasToBeChild && MyReplicablesBase.TryGetParent(replicable, out parent1))
      {
        IMyReplicable parent2;
        if (this.m_childToParent.TryGetValue(replicable, out parent2))
        {
          if (parent2 == parent1)
            return;
          this.RemoveChild(replicable, parent2);
          this.AddChild(replicable, parent1);
        }
        else
        {
          this.RemoveRoot(replicable);
          this.AddChild(replicable, parent1);
        }
      }
      else
      {
        IMyReplicable parent2;
        if (!this.m_childToParent.TryGetValue(replicable, out parent2))
          return;
        this.RemoveChild(replicable, parent2);
        this.AddRoot(replicable);
      }
    }

    private void Remove(IMyReplicable replicable)
    {
      IMyReplicable parent;
      if (this.m_childToParent.TryGetValue(replicable, out parent))
        this.RemoveChild(replicable, parent);
      this.RemoveRoot(replicable);
    }

    protected virtual void AddChild(IMyReplicable replicable, IMyReplicable parent)
    {
      HashSet<IMyReplicable> myReplicableSet;
      if (!this.m_parentToChildren.TryGetValue(parent, out myReplicableSet))
      {
        myReplicableSet = this.Obtain();
        this.m_parentToChildren[parent] = myReplicableSet;
      }
      myReplicableSet.Add(replicable);
      this.m_childToParent[replicable] = parent;
      this.OnChildAdded.InvokeIfNotNull<IMyReplicable>(replicable);
    }

    protected virtual void RemoveChild(IMyReplicable replicable, IMyReplicable parent)
    {
      this.m_childToParent.Remove<IMyReplicable, IMyReplicable>(replicable);
      HashSet<IMyReplicable> parentToChild = this.m_parentToChildren[parent];
      parentToChild.Remove(replicable);
      if (parentToChild.Count != 0)
        return;
      this.m_parentToChildren.Remove<IMyReplicable, HashSet<IMyReplicable>>(parent);
      this.m_hashSetPool.Push(parentToChild);
    }

    public abstract void IterateRoots(Action<IMyReplicable> p);

    public abstract void GetReplicablesInBox(BoundingBoxD aabb, List<IMyReplicable> list);

    protected abstract void AddRoot(IMyReplicable replicable);

    protected abstract void RemoveRoot(IMyReplicable replicable);

    [Conditional("DEBUG")]
    protected void CheckThread()
    {
    }
  }
}
