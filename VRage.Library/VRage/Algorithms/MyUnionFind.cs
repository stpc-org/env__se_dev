// Decompiled with JetBrains decompiler
// Type: VRage.Algorithms.MyUnionFind
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Algorithms
{
  public class MyUnionFind
  {
    private MyUnionFind.UF[] m_indices;
    private int m_size;

    private bool IsInRange(int index) => index >= 0 && index < this.m_size;

    public MyUnionFind()
    {
    }

    public MyUnionFind(int initialSize) => this.Resize(initialSize);

    public void Resize(int count = 0)
    {
      if (this.m_indices == null || this.m_indices.Length < count)
        this.m_indices = new MyUnionFind.UF[count];
      this.m_size = count;
      this.Clear();
    }

    public unsafe void Clear()
    {
      fixed (MyUnionFind.UF* ufPtr = this.m_indices)
      {
        for (int index = 0; index < this.m_size; ++index)
        {
          ufPtr[index].Parent = index;
          ufPtr[index].Rank = 0;
        }
      }
    }

    public unsafe void Union(int a, int b)
    {
      fixed (MyUnionFind.UF* uf = this.m_indices)
      {
        int index1 = this.Find(uf, a);
        int index2 = this.Find(uf, b);
        if (index1 == index2)
          return;
        if (uf[index1].Rank < uf[index2].Rank)
          uf[index1].Parent = index2;
        else if (uf[index1].Rank > uf[index2].Rank)
        {
          uf[index2].Parent = index1;
        }
        else
        {
          uf[index2].Parent = index1;
          ++uf[index1].Rank;
        }
      }
    }

    private unsafe int Find(MyUnionFind.UF* uf, int a)
    {
      MyUnionFind.step* stepPtr1 = (MyUnionFind.step*) null;
      for (; uf[a].Parent != a; a = uf[a].Parent)
      {
        MyUnionFind.step* stepPtr2 = stackalloc MyUnionFind.step[1];
        stepPtr2->Element = a;
        stepPtr2->Prev = stepPtr1;
        stepPtr1 = stepPtr2;
      }
      for (; (IntPtr) stepPtr1 != IntPtr.Zero; stepPtr1 = stepPtr1->Prev)
        uf[stepPtr1->Element].Parent = a;
      return a;
    }

    public unsafe int Find(int a)
    {
      fixed (MyUnionFind.UF* uf = this.m_indices)
        return this.Find(uf, a);
    }

    private struct UF
    {
      public int Parent;
      public int Rank;
    }

    private struct step
    {
      public unsafe MyUnionFind.step* Prev;
      public int Element;
    }
  }
}
