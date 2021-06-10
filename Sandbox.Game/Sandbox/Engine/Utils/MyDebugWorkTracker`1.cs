// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyDebugWorkTracker`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Diagnostics;
using VRage.Collections;
using VRage.Library.Threading;

namespace Sandbox.Engine.Utils
{
  public class MyDebugWorkTracker<T> where T : new()
  {
    private SpinLockRef m_lock = new SpinLockRef();
    public readonly MyQueue<T> History;
    public T Current;
    private uint m_historyLength;

    public MyDebugWorkTracker(uint historyLength = 10)
    {
      this.m_historyLength = historyLength;
      this.History = new MyQueue<T>((int) this.m_historyLength);
    }

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
