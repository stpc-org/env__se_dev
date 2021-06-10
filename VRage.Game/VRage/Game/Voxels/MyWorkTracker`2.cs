// Decompiled with JetBrains decompiler
// Type: VRage.Game.Voxels.MyWorkTracker`2
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace VRage.Game.Voxels
{
  public class MyWorkTracker<TWorkId, TWork> : IEnumerable<KeyValuePair<TWorkId, TWork>>, IEnumerable
    where TWork : MyPrecalcJob
  {
    private readonly Dictionary<TWorkId, TWork> m_worksById;

    public MyWorkTracker(IEqualityComparer<TWorkId> comparer = null) => this.m_worksById = new Dictionary<TWorkId, TWork>(comparer ?? (IEqualityComparer<TWorkId>) EqualityComparer<TWorkId>.Default);

    public void Add(TWorkId id, TWork work)
    {
      lock (this.m_worksById)
      {
        work.IsValid = true;
        this.m_worksById.Add(id, work);
      }
    }

    public bool TryAdd(TWorkId id, TWork work)
    {
      lock (this.m_worksById)
      {
        if (this.m_worksById.ContainsKey(id))
          return false;
        work.IsValid = true;
        this.m_worksById.Add(id, work);
        return true;
      }
    }

    public bool Invalidate(TWorkId id)
    {
      lock (this.m_worksById)
      {
        TWork work;
        if (this.m_worksById.TryGetValue(id, out work))
        {
          work.IsValid = false;
          return true;
        }
      }
      return false;
    }

    public void InvalidateAll()
    {
      lock (this.m_worksById)
      {
        foreach (TWork work in this.m_worksById.Values)
          work.IsValid = false;
      }
    }

    public void CancelAll()
    {
      lock (this.m_worksById)
      {
        foreach (KeyValuePair<TWorkId, TWork> keyValuePair in this.m_worksById)
          keyValuePair.Value.Cancel();
        this.m_worksById.Clear();
      }
    }

    public TWork Cancel(TWorkId id)
    {
      TWork work;
      lock (this.m_worksById)
      {
        if (this.m_worksById.TryGetValue(id, out work))
        {
          if (this.m_worksById.Remove(id))
            work.Cancel();
        }
      }
      return work;
    }

    public TWork CancelIfStarted(TWorkId id)
    {
      TWork work;
      lock (this.m_worksById)
      {
        if (this.m_worksById.TryGetValue(id, out work))
        {
          if (work.Started)
          {
            if (this.m_worksById.Remove(id))
              work.Cancel();
          }
        }
      }
      return work;
    }

    public bool Exists(TWorkId id)
    {
      lock (this.m_worksById)
        return this.m_worksById.ContainsKey(id);
    }

    public bool HasAny
    {
      get
      {
        lock (this.m_worksById)
          return this.m_worksById.Count > 0;
      }
    }

    public bool TryGet(TWorkId id, out TWork work)
    {
      lock (this.m_worksById)
        return this.m_worksById.TryGetValue(id, out work);
    }

    public void Complete(TWorkId id)
    {
      lock (this.m_worksById)
        this.m_worksById.Remove(id);
    }

    public MyWorkTracker<TWorkId, TWork>.Enumerator GetEnumerator() => new MyWorkTracker<TWorkId, TWork>.Enumerator(this.m_worksById);

    IEnumerator<KeyValuePair<TWorkId, TWork>> IEnumerable<KeyValuePair<TWorkId, TWork>>.GetEnumerator() => (IEnumerator<KeyValuePair<TWorkId, TWork>>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public struct Enumerator : IEnumerator<KeyValuePair<TWorkId, TWork>>, IEnumerator, IDisposable
    {
      private Dictionary<TWorkId, TWork>.Enumerator m_enumerator;
      private readonly Dictionary<TWorkId, TWork> m_syncRoot;

      public Enumerator(Dictionary<TWorkId, TWork> dictionary)
      {
        this.m_syncRoot = dictionary;
        Monitor.Enter((object) this.m_syncRoot);
        this.m_enumerator = dictionary.GetEnumerator();
      }

      public void Dispose()
      {
        try
        {
          this.m_enumerator.Dispose();
        }
        finally
        {
          Monitor.Exit((object) this.m_syncRoot);
        }
      }

      public bool MoveNext() => this.m_enumerator.MoveNext();

      public void Reset() => this.m_enumerator = this.m_syncRoot.GetEnumerator();

      public KeyValuePair<TWorkId, TWork> Current => this.m_enumerator.Current;

      object IEnumerator.Current => (object) this.m_enumerator.Current;
    }
  }
}
