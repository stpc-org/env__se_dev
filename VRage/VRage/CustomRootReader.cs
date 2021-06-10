// Decompiled with JetBrains decompiler
// Type: VRage.CustomRootReader
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Xml;
using VRage.Network;

namespace VRage
{
  [GenerateActivator]
  public class CustomRootReader : XmlReader
  {
    private XmlReader m_source;
    private string m_customRootName;
    private int m_rootDepth;

    internal void Init(string customRootName, XmlReader source)
    {
      this.m_source = source;
      this.m_customRootName = customRootName;
      this.m_rootDepth = source.Depth;
    }

    internal void Release()
    {
      this.m_source = (XmlReader) null;
      this.m_customRootName = (string) null;
      this.m_rootDepth = -1;
    }

    public override int AttributeCount => this.m_source.AttributeCount;

    public override string BaseURI => this.m_source.BaseURI;

    public override void Close() => this.m_source.Close();

    public override int Depth => this.m_source.Depth;

    public override bool EOF => this.m_source.EOF;

    public override string GetAttribute(int i) => this.m_source.GetAttribute(i);

    public override string GetAttribute(string name) => this.m_source.GetAttribute(name);

    public override bool IsEmptyElement => this.m_source.IsEmptyElement;

    public override string LookupNamespace(string prefix) => this.m_source.LookupNamespace(prefix);

    public override bool MoveToAttribute(string name, string ns) => this.m_source.MoveToAttribute(name, ns);

    public override bool MoveToAttribute(string name) => this.m_source.MoveToAttribute(name);

    public override bool MoveToElement() => this.m_source.MoveToElement();

    public override bool MoveToFirstAttribute() => this.m_source.MoveToFirstAttribute();

    public override bool MoveToNextAttribute() => this.m_source.MoveToNextAttribute();

    public override XmlNameTable NameTable => this.m_source.NameTable;

    public override XmlNodeType NodeType => this.m_source.NodeType;

    public override string Prefix => this.m_source.Prefix;

    public override bool Read() => this.m_source.Read();

    public override bool ReadAttributeValue() => this.m_source.ReadAttributeValue();

    public override ReadState ReadState => this.m_source.ReadState;

    public override void ResolveEntity() => this.m_source.ResolveEntity();

    public override string Value => this.m_source.Value;

    public override string LocalName => this.m_source.Depth != this.m_rootDepth ? this.m_source.LocalName : this.m_source.NameTable.Get(this.m_customRootName);

    public override string NamespaceURI => this.m_source.Depth != this.m_rootDepth ? this.m_source.NamespaceURI : this.m_source.NameTable.Get("");

    public override string GetAttribute(string name, string namespaceURI) => this.m_source.Depth != this.m_rootDepth ? this.m_source.GetAttribute(name, namespaceURI) : (string) null;

    private class VRage_CustomRootReader\u003C\u003EActor : IActivator, IActivator<CustomRootReader>
    {
      object IActivator.CreateInstance() => (object) new CustomRootReader();

      CustomRootReader IActivator<CustomRootReader>.CreateInstance() => new CustomRootReader();
    }
  }
}
