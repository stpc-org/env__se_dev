// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyFatBlockReader`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Sandbox.Game.Entities.Cube
{
  public struct MyFatBlockReader<TBlock> : IEnumerator<TBlock>, IEnumerator, IDisposable
    where TBlock : MyCubeBlock
  {
    private HashSet<MySlimBlock>.Enumerator m_enumerator;

    public MyFatBlockReader(MyCubeGrid grid)
      : this(grid.GetBlocks().GetEnumerator())
    {
    }

    public MyFatBlockReader(HashSet<MySlimBlock> set)
      : this(set.GetEnumerator())
    {
    }

    public MyFatBlockReader(HashSet<MySlimBlock>.Enumerator enumerator) => this.m_enumerator = enumerator;

    public MyFatBlockReader<TBlock> GetEnumerator() => this;

    public TBlock Current => (TBlock) this.m_enumerator.Current.FatBlock;

    public void Dispose() => this.m_enumerator.Dispose();

    object IEnumerator.Current => (object) this.Current;

    public bool MoveNext()
    {
      while (this.m_enumerator.MoveNext())
      {
        if ((object) (this.m_enumerator.Current.FatBlock as TBlock) != null)
          return true;
      }
      return false;
    }

    public void Reset()
    {
      IEnumerator<MySlimBlock> enumerator = (IEnumerator<MySlimBlock>) this.m_enumerator;
      enumerator.Reset();
      this.m_enumerator = (HashSet<MySlimBlock>.Enumerator) enumerator;
    }
  }
}
