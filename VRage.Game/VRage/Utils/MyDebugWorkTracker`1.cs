// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyDebugWorkTracker`1
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Diagnostics;
using VRage.Collections;
using VRage.Library.Threading;

namespace VRage.Utils
{
  public class MyDebugWorkTracker<T> where T : new()
  {
    private SpinLockRef m_lock = new SpinLockRef();
    public readonly MyQueue<T> History;
    public T Current;
    private uint m_historyLength;

    public MyDebugWorkTracker(uint historyLength = 10) => this.m_historyLength = historyLength;

    [Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
    public void Wrap()
    {
      using (this.m_lock.Acquire())
      {
        if ((long) this.History.Count >= (long) this.m_historyLength)
          this.History.Dequeue();
        this.History.Enqueue(this.Current);
        this.Current = new T();
      }
    }
  }
}
