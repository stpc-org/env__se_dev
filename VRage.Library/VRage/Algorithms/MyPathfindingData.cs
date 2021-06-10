// Decompiled with JetBrains decompiler
// Type: VRage.Algorithms.MyPathfindingData
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Generic;
using System.Threading;
using VRage.Collections;

namespace VRage.Algorithms
{
  public class MyPathfindingData : HeapItem<float>
  {
    private object m_lockObject = new object();
    private Dictionary<Thread, long> threadedTimestamp = new Dictionary<Thread, long>();
    internal MyPathfindingData Predecessor;
    internal float PathLength;

    public object Parent { get; private set; }

    internal long Timestamp
    {
      get
      {
        long num = 0;
        lock (this.m_lockObject)
        {
          if (!this.threadedTimestamp.TryGetValue(Thread.CurrentThread, out num))
            num = 0L;
        }
        return num;
      }
      set
      {
        lock (this.m_lockObject)
          this.threadedTimestamp[Thread.CurrentThread] = value;
      }
    }

    public MyPathfindingData(object parent) => this.Parent = parent;

    public long GetTimestamp() => this.Timestamp;
  }
}
