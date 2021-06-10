// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.Ingame.MyIGCMessage
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

namespace Sandbox.ModAPI.Ingame
{
  public struct MyIGCMessage
  {
    public readonly object Data;
    public readonly string Tag;
    public readonly long Source;

    public MyIGCMessage(object data, string tag, long source)
    {
      this.Tag = tag;
      this.Data = data;
      this.Source = source;
    }

    public TData As<TData>() => (TData) this.Data;
  }
}
