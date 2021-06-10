// Decompiled with JetBrains decompiler
// Type: VRage.Trace.ITrace
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Trace
{
  public interface ITrace
  {
    bool Enabled { get; set; }

    void Watch(string name, object value);

    void Send(string msg, string comment = null);

    void Flush();
  }
}
