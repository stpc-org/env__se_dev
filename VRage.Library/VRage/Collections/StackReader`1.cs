// Decompiled with JetBrains decompiler
// Type: VRage.Collections.StackReader`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
  public struct StackReader<T> : IEnumerable<T>, IEnumerable
  {
    private readonly Stack<T> m_collection;

    public StackReader(Stack<T> collection) => this.m_collection = collection;

    public Stack<T>.Enumerator GetEnumerator() => this.m_collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this.GetEnumerator();
  }
}
