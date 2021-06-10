// Decompiled with JetBrains decompiler
// Type: VRage.MyXmlSerializerBase`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using VRage.Generics;

namespace VRage
{
  public abstract class MyXmlSerializerBase<TAbstractBase> : IMyXmlSerializable, IXmlSerializable
  {
    [ThreadStatic]
    private static MyObjectsPool<CustomRootReader> m_readerPool;
    [ThreadStatic]
    private static MyObjectsPool<CustomRootWriter> m_writerPool;
    protected TAbstractBase m_data;

    protected static MyObjectsPool<CustomRootReader> ReaderPool
    {
      get
      {
        if (MyXmlSerializerBase<TAbstractBase>.m_readerPool == null)
          MyXmlSerializerBase<TAbstractBase>.m_readerPool = new MyObjectsPool<CustomRootReader>(2);
        return MyXmlSerializerBase<TAbstractBase>.m_readerPool;
      }
    }

    protected static MyObjectsPool<CustomRootWriter> WriterPool
    {
      get
      {
        if (MyXmlSerializerBase<TAbstractBase>.m_writerPool == null)
          MyXmlSerializerBase<TAbstractBase>.m_writerPool = new MyObjectsPool<CustomRootWriter>(2);
        return MyXmlSerializerBase<TAbstractBase>.m_writerPool;
      }
    }

    public static implicit operator TAbstractBase(MyXmlSerializerBase<TAbstractBase> o) => o.Data;

    public TAbstractBase Data => this.m_data;

    public XmlSchema GetSchema() => (XmlSchema) null;

    public abstract void ReadXml(XmlReader reader);

    protected object Deserialize(XmlReader reader, XmlSerializer serializer, string customRootName)
    {
      CustomRootReader customRootReader;
      MyXmlSerializerBase<TAbstractBase>.ReaderPool.AllocateOrCreate(out customRootReader);
      customRootReader.Init(customRootName, reader);
      object obj = serializer.Deserialize((XmlReader) customRootReader);
      customRootReader.Release();
      MyXmlSerializerBase<TAbstractBase>.ReaderPool.Deallocate(customRootReader);
      return obj;
    }

    public void WriteXml(XmlWriter writer)
    {
      Type type = this.m_data.GetType();
      XmlSerializer serializer = MyXmlSerializerManager.GetOrCreateSerializer(type);
      string serializedName = MyXmlSerializerManager.GetSerializedName(type);
      CustomRootWriter customRootWriter;
      MyXmlSerializerBase<TAbstractBase>.WriterPool.AllocateOrCreate(out customRootWriter);
      customRootWriter.Init(serializedName, writer);
      serializer.Serialize((XmlWriter) customRootWriter, (object) this.m_data);
      customRootWriter.Release();
      MyXmlSerializerBase<TAbstractBase>.WriterPool.Deallocate(customRootWriter);
    }

    object IMyXmlSerializable.Data => (object) this.m_data;
  }
}
