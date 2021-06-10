// Decompiled with JetBrains decompiler
// Type: VRageMath.BoxCornerEnumerator
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace VRageMath
{
  public struct BoxCornerEnumerator : IEnumerator<Vector3>, IEnumerator, IDisposable, IEnumerable<Vector3>, IEnumerable
  {
    private static Vector3B[] m_indicesCache = new Vector3B[8]
    {
      new Vector3B((sbyte) 0, (sbyte) 4, (sbyte) 5),
      new Vector3B((sbyte) 3, (sbyte) 4, (sbyte) 5),
      new Vector3B((sbyte) 3, (sbyte) 1, (sbyte) 5),
      new Vector3B((sbyte) 0, (sbyte) 1, (sbyte) 5),
      new Vector3B((sbyte) 0, (sbyte) 4, (sbyte) 2),
      new Vector3B((sbyte) 3, (sbyte) 4, (sbyte) 2),
      new Vector3B((sbyte) 3, (sbyte) 1, (sbyte) 2),
      new Vector3B((sbyte) 0, (sbyte) 1, (sbyte) 2)
    };
    private int m_index;
    private unsafe fixed float m_minMax[6];

    public unsafe BoxCornerEnumerator(Vector3 min, Vector3 max)
    {
      for (int i = 0; i < 3; ++i)
      {
        this.m_minMax[i] = min.GetDim(i);
        this.m_minMax[i + 3] = max.GetDim(i);
      }
      this.m_index = -1;
    }

    public void Add(object tmp)
    {
    }

    public unsafe Vector3 Current
    {
      get
      {
        Vector3B vector3B = BoxCornerEnumerator.m_indicesCache[this.m_index];
        return new Vector3(this.m_minMax[vector3B.X], this.m_minMax[vector3B.Y], this.m_minMax[vector3B.Z]);
      }
    }

    public bool MoveNext() => ++this.m_index < 8;

    void IDisposable.Dispose()
    {
    }

    object IEnumerator.Current => (object) this.Current;

    void IEnumerator.Reset() => this.m_index = -1;

    public BoxCornerEnumerator GetEnumerator() => this;

    IEnumerator<Vector3> IEnumerable<Vector3>.GetEnumerator() => (IEnumerator<Vector3>) this;

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this;
  }
}
