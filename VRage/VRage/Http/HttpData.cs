// Decompiled with JetBrains decompiler
// Type: VRage.Http.HttpData
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Http
{
  public struct HttpData
  {
    public string Name;
    public object Value;
    public HttpDataType Type;

    public HttpData(string name, object value, HttpDataType type)
    {
      this.Name = name;
      this.Value = value;
      this.Type = type;
    }
  }
}
