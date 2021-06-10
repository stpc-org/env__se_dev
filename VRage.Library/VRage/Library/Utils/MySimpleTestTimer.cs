// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.MySimpleTestTimer
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Diagnostics;
using System.IO;

namespace VRage.Library.Utils
{
  internal struct MySimpleTestTimer : IDisposable
  {
    private string m_name;
    private Stopwatch m_watch;

    public MySimpleTestTimer(string name)
    {
      this.m_name = name;
      this.m_watch = new Stopwatch();
      this.m_watch.Start();
    }

    public void Dispose() => File.AppendAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "perf.log"), string.Format("{0}: {1:N}ms\n", (object) this.m_name, (object) this.m_watch.ElapsedMilliseconds));
  }
}
