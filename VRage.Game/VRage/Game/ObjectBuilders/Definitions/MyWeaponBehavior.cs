// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyWeaponBehavior
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.Xml.Serialization;

namespace VRage.Game.ObjectBuilders.Definitions
{
  public class MyWeaponBehavior
  {
    public string Name = "No name";
    public int Priority = 10;
    public float TimeMin = 2f;
    public float TimeMax = 4f;
    public bool IgnoresVoxels;
    public bool IgnoresGrids;
    [XmlArrayItem("WeaponRule")]
    public List<MyWeaponRule> WeaponRules;
    [XmlArrayItem("Weapon")]
    public List<string> Requirements;
    public bool RequirementsIsWhitelist;
  }
}
