// Decompiled with JetBrains decompiler
// Type: ParallelTasks.Singleton`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Threading;

namespace ParallelTasks
{
  public abstract class Singleton<T> where T : class, new()
  {
    private static T instance;

    public static T Instance
    {
      get
      {
        if ((object) Singleton<T>.instance == null)
        {
          T obj = new T();
          Interlocked.CompareExchange<T>(ref Singleton<T>.instance, obj, default (T));
        }
        return Singleton<T>.instance;
      }
    }
  }
}
