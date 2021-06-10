// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.CacheList`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public class CacheList<T> : List<T>, IDisposable
  {
    public CacheList<T> Empty => this;

    public CacheList()
    {
    }

    public CacheList(int capacity)
      : base(capacity)
    {
    }

    void IDisposable.Dispose() => this.Clear();
  }
}
