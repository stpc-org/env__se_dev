// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.Disposable
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage.Library.Utils
{
  public class Disposable : IDisposable
  {
    public Disposable(bool collectStack = false)
    {
    }

    public virtual void Dispose() => GC.SuppressFinalize((object) this);

    ~Disposable() => string.Format("Dispose was not called for '{0}'", (object) this.GetType().FullName);
  }
}
