// Decompiled with JetBrains decompiler
// Type: VRage.MyAbstractXmlSerializer`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Xml;
using System.Xml.Serialization;

namespace VRage
{
  public class MyAbstractXmlSerializer<TAbstractBase> : MyXmlSerializerBase<TAbstractBase>
  {
    public MyAbstractXmlSerializer()
    {
    }

    public MyAbstractXmlSerializer(TAbstractBase data) => this.m_data = data;

    public override void ReadXml(XmlReader reader)
    {
      string customRootName;
      XmlSerializer serializer = this.GetSerializer(reader, out customRootName);
      this.m_data = (TAbstractBase) this.Deserialize(reader, serializer, customRootName);
    }

    private XmlSerializer GetSerializer(XmlReader reader, out string customRootName)
    {
      string serializedName = this.GetTypeAttribute(reader);
      XmlSerializer serializer;
      if (serializedName == null || !MyXmlSerializerManager.TryGetSerializer(serializedName, out serializer))
      {
        serializedName = MyXmlSerializerManager.GetSerializedName(typeof (TAbstractBase));
        serializer = MyXmlSerializerManager.GetSerializer(serializedName);
      }
      customRootName = serializedName;
      return serializer;
    }

    protected virtual string GetTypeAttribute(XmlReader reader) => reader.GetAttribute("xsi:type");

    public static implicit operator MyAbstractXmlSerializer<TAbstractBase>(
      TAbstractBase builder)
    {
      return (object) builder != null ? new MyAbstractXmlSerializer<TAbstractBase>(builder) : (MyAbstractXmlSerializer<TAbstractBase>) null;
    }
  }
}
