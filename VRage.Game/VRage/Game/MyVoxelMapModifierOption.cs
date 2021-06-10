// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyVoxelMapModifierOption
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Xml.Serialization;

namespace VRage.Game
{
  public class MyVoxelMapModifierOption
  {
    [XmlAttribute(AttributeName = "Chance")]
    public float Chance;
    [XmlElement("Change")]
    public MyVoxelMapModifierChange[] Changes;
  }
}
