// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_RadialMenuSection
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.ComponentModel;
using System.Xml.Serialization;
using VRage.Utils;

namespace VRage.Game
{
  public class MyObjectBuilder_RadialMenuSection
  {
    public MyStringId Label;
    [DefaultValue(null)]
    [XmlArrayItem("Item", typeof (MyAbstractXmlSerializer<MyObjectBuilder_RadialMenuItem>))]
    public MyObjectBuilder_RadialMenuItem[] Items;
    public bool m_IsEnabledCreative = true;
    public bool m_IsEnabledSurvival = true;

    public bool IsEnabledCreative
    {
      get => this.m_IsEnabledCreative;
      set => this.m_IsEnabledCreative = value;
    }

    public bool IsEnabledSurvival
    {
      get => this.m_IsEnabledSurvival;
      set => this.m_IsEnabledSurvival = value;
    }
  }
}
