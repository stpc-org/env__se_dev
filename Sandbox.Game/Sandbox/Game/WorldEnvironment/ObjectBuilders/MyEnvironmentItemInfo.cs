// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.ObjectBuilders.MyEnvironmentItemInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Xml.Serialization;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
  public class MyEnvironmentItemInfo
  {
    [XmlAttribute]
    public string Type;
    public MyStringHash Subtype;
    [XmlAttribute]
    public float Offset;
    [XmlAttribute]
    public float Density;

    [XmlAttribute("Subtype")]
    public string SubtypeText
    {
      get => this.Subtype.ToString();
      set => this.Subtype = MyStringHash.GetOrCompute(value);
    }
  }
}
