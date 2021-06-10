// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Conveyors.ConveyorLineEnumerator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Sandbox.Game.GameSystems.Conveyors
{
  public struct ConveyorLineEnumerator : IEnumerator<MyConveyorLine>, IEnumerator, IDisposable
  {
    private int index;
    private IMyConveyorEndpoint m_enumerated;
    private MyConveyorLine m_line;

    public ConveyorLineEnumerator(IMyConveyorEndpoint enumerated)
    {
      this.index = -1;
      this.m_enumerated = enumerated;
      this.m_line = (MyConveyorLine) null;
    }

    public MyConveyorLine Current => this.m_line;

    public void Dispose()
    {
      this.m_enumerated = (IMyConveyorEndpoint) null;
      this.m_line = (MyConveyorLine) null;
    }

    object IEnumerator.Current => (object) this.m_line;

    public bool MoveNext()
    {
      do
        ;
      while (this.MoveNextInternal());
      return this.index < this.m_enumerated.GetLineCount();
    }

    private bool MoveNextInternal()
    {
      ++this.index;
      if (this.index >= this.m_enumerated.GetLineCount())
        return false;
      this.m_line = this.m_enumerated.GetConveyorLine(this.index);
      return !this.m_line.IsWorking;
    }

    public void Reset() => this.index = 0;
  }
}
