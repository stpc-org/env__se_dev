// Decompiled with JetBrains decompiler
// Type: VRage.Stats.MyStatToken
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using VRage.Library.Utils;

namespace VRage.Stats
{
  public struct MyStatToken : IDisposable
  {
    private readonly MyGameTimer m_timer;
    private readonly MyTimeSpan m_startTime;
    private readonly MyStat m_stat;

    internal MyStatToken(MyGameTimer timer, MyStat stat)
    {
      this.m_timer = timer;
      this.m_startTime = timer.Elapsed;
      this.m_stat = stat;
    }

    public void Dispose() => this.m_stat.Write((float) (this.m_timer.Elapsed - this.m_startTime).Milliseconds);
  }
}
