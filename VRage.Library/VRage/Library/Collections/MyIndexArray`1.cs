// Decompiled with JetBrains decompiler
// Type: VRage.Library.Collections.MyIndexArray`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
  public class MyIndexArray<T>
  {
    private T[] m_internalArray;
    public float MinimumGrowFactor = 2f;

    public T[] InternalArray => this.m_internalArray;

    public int Length => this.m_internalArray.Length;

    public T this[int index]
    {
      get => index < this.m_internalArray.Length ? this.m_internalArray[index] : default (T);
      set
      {
        int length = this.m_internalArray.Length;
        if (index >= length)
          Array.Resize<T>(ref this.m_internalArray, Math.Max((int) Math.Ceiling((double) this.MinimumGrowFactor * (double) length), index + 1));
        this.m_internalArray[index] = value;
      }
    }

    public MyIndexArray(int defaultCapacity = 0) => this.m_internalArray = defaultCapacity > 0 ? new T[defaultCapacity] : EmptyArray<T>.Value;

    public void Clear() => Array.Clear((Array) this.m_internalArray, 0, this.m_internalArray.Length);

    public void ClearItem(int index) => this.m_internalArray[index] = default (T);

    public void TrimExcess(float minimumShrinkFactor = 0.5f, IEqualityComparer<T> comparer = null)
    {
      comparer = comparer ?? (IEqualityComparer<T>) EqualityComparer<T>.Default;
      int index = this.m_internalArray.Length - 1;
      while (index >= 0 && comparer.Equals(this.m_internalArray[index], default (T)))
        --index;
      int newSize = index + 1;
      if ((double) newSize > (double) this.m_internalArray.Length * (double) minimumShrinkFactor)
        return;
      Array.Resize<T>(ref this.m_internalArray, newSize);
    }
  }
}
