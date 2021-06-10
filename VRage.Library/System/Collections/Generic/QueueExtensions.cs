// Decompiled with JetBrains decompiler
// Type: System.Collections.Generic.QueueExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections.Concurrent;
using System.Reflection;

namespace System.Collections.Generic
{
  public static class QueueExtensions
  {
    public static bool TryDequeue<T>(this Queue<T> queue, out T result)
    {
      if (queue.Count > 0)
      {
        result = queue.Dequeue();
        return true;
      }
      result = default (T);
      return false;
    }

    public static bool TryDequeueSync<T>(this Queue<T> queue, out T result)
    {
      lock (((ICollection) queue).SyncRoot)
        return queue.TryDequeue<T>(out result);
    }

    private static class QueueReflector<T>
    {
      public static Func<ConcurrentQueue<T>, List<T>> ToList = (Func<ConcurrentQueue<T>, List<T>>) Delegate.CreateDelegate(typeof (Func<ConcurrentQueue<T>, List<T>>), typeof (ConcurrentQueue<T>).GetMethod(nameof (ToList), BindingFlags.Instance | BindingFlags.NonPublic));
    }
  }
}
