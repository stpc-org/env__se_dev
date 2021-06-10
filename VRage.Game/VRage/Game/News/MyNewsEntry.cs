// Decompiled with JetBrains decompiler
// Type: VRage.Game.News.MyNewsEntry
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Xml.Serialization;

namespace VRage.Game.News
{
  public class MyNewsEntry
  {
    [XmlAttribute(AttributeName = "title")]
    public string Title;
    [XmlAttribute(AttributeName = "date")]
    public string Date;
    [XmlAttribute(AttributeName = "version")]
    public string Version;
    [XmlAttribute(AttributeName = "public")]
    public bool Public = true;
    [XmlAttribute(AttributeName = "dev")]
    public bool Dev;
    [XmlArrayItem("Link")]
    public MyNewsLink[] Links;
    [XmlText]
    public string Text;
  }
}
