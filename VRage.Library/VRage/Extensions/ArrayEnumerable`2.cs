// Decompiled with JetBrains decompiler
// Type: VRage.Extensions.ArrayEnumerable`2
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace VRage.Extensions
{
  public struct ArrayEnumerable<T, TEnumerator> : IEnumerable<T>, IEnumerable where TEnumerator : struct, IEnumerator<T>
  {
    private TEnumerator m_enumerator;

    public ArrayEnumerable(TEnumerator enumerator) => this.m_enumerator = enumerator;

    public TEnumerator GetEnumerator() => this.m_enumerator;

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
