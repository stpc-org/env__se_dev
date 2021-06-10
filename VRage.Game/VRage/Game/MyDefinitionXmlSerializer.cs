// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyDefinitionXmlSerializer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Xml;

namespace VRage.Game
{
  public class MyDefinitionXmlSerializer : MyAbstractXmlSerializer<MyObjectBuilder_DefinitionBase>
  {
    public const string DEFINITION_ELEMENT_NAME = "Definition";

    public MyDefinitionXmlSerializer()
    {
    }

    public MyDefinitionXmlSerializer(MyObjectBuilder_DefinitionBase data) => this.m_data = data;

    protected override string GetTypeAttribute(XmlReader reader)
    {
      string typeAttribute = base.GetTypeAttribute(reader);
      if (typeAttribute == null)
        return (string) null;
      string str;
      return reader is MyXmlTextReader myXmlTextReader && myXmlTextReader.DefinitionTypeOverrideMap != null && myXmlTextReader.DefinitionTypeOverrideMap.TryGetValue(typeAttribute, out str) ? str : typeAttribute;
    }

    public static implicit operator MyDefinitionXmlSerializer(
      MyObjectBuilder_DefinitionBase builder)
    {
      return builder != null ? new MyDefinitionXmlSerializer(builder) : (MyDefinitionXmlSerializer) null;
    }
  }
}
