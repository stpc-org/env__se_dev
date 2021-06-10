// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyRankServer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Xml.Serialization;

namespace VRage.Game
{
  public class MyRankServer
  {
    private string m_connectionString;

    [XmlAttribute]
    public int Rank { get; set; }

    [XmlAttribute]
    public string Address { get; set; }

    [XmlAttribute]
    public string ServicePrefix { get; set; }

    [XmlIgnore]
    public string ConnectionString => this.m_connectionString ?? (this.m_connectionString = this.ServicePrefix + this.Address);
  }
}
