// Decompiled with JetBrains decompiler
// Type: VRage.Generics.MyDynamicObjectPool`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage.Generics
{
  public class MyDynamicObjectPool<T> where T : class, new()
  {
    private readonly Stack<T> m_poolStack;
    private readonly Action<T> m_deallocator;

    public int Count => this.m_poolStack.Count;

    public MyDynamicObjectPool(int capacity, Action<T> deallocator = null)
    {
      this.m_deallocator = deallocator ?? (Action<T>) (_ => {});
      this.m_poolStack = new Stack<T>(capacity);
      this.Preallocate(capacity);
    }

    private void Preallocate(int count)
    {
      for (int index = 0; index < count; ++index)
        this.m_poolStack.Push(new T());
    }

    public T Allocate()
    {
      if (this.m_poolStack.Count == 0)
        this.Preallocate(1);
      return this.m_poolStack.Pop();
    }

    public void Deallocate(T item) => this.m_poolStack.Push(item);

    public void TrimToSize(int size)
    {
      while (this.m_poolStack.Count > size)
        this.m_poolStack.Pop();
      this.m_poolStack.TrimExcess();
    }

    public void SuppressFinalize()
    {
      foreach (T pool in this.m_poolStack)
        GC.SuppressFinalize((object) pool);
    }
  }
}
