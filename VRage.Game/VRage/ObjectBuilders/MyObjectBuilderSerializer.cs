// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilders.MyObjectBuilderSerializer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Library.Collections;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.ObjectBuilders
{
  public class MyObjectBuilderSerializer
  {
    public const string ProtobufferExtension = "B5";
    private static readonly bool ENABLE_PROTOBUFFERS_CLONING = true;
    private static MyObjectFactory<MyObjectBuilderDefinitionAttribute, MyObjectBuilder_Base> m_objectFactory;
    public static TypeModel Serializer;
    public static readonly MySerializeInfo Dynamic = new MySerializeInfo(MyObjectFlags.Dynamic, MyPrimitiveFlags.None, (ushort) 0, new DynamicSerializerDelegate(MyObjectBuilderSerializer.SerializeDynamic), (MySerializeInfo) null, (MySerializeInfo) null);
    private static IProtoTypeModel m_typeModel = MyVRage.Platform?.GetTypeModel();

    static MyObjectBuilderSerializer()
    {
      MyObjectBuilderSerializer.Serializer = MyObjectBuilderSerializer.m_typeModel?.Model;
      MyObjectBuilderSerializer.m_objectFactory = new MyObjectFactory<MyObjectBuilderDefinitionAttribute, MyObjectBuilder_Base>();
    }

    public static void RegisterFromAssembly(Assembly assembly) => MyObjectBuilderSerializer.m_objectFactory.RegisterFromAssembly(assembly);

    private static ushort Get16BitHash(string s)
    {
      using (MD5 md5 = MD5.Create())
        return BitConverter.ToUInt16(md5.ComputeHash(Encoding.UTF8.GetBytes(s)), 0);
    }

    public static void LoadSerializers() => MyObjectBuilderSerializer.m_typeModel?.RegisterTypes(MyObjectBuilderSerializer.m_objectFactory.Attributes.Select<MyObjectBuilderDefinitionAttribute, Type>((Func<MyObjectBuilderDefinitionAttribute, Type>) (x => x.ProducedType)));

    private static void SerializeXMLInternal(
      Stream writeTo,
      MyObjectBuilder_Base objectBuilder,
      Type serializeAsType = null)
    {
      Type type = serializeAsType;
      if ((object) type == null)
        type = objectBuilder.GetType();
      MyXmlSerializerManager.GetSerializer(type).Serialize(writeTo, (object) objectBuilder);
    }

    private static void SerializeGZippedXMLInternal(
      Stream writeTo,
      MyObjectBuilder_Base objectBuilder,
      Type serializeAsType = null)
    {
      using (GZipStream gzipStream = new GZipStream(writeTo, CompressionMode.Compress, true))
      {
        using (BufferedStream bufferedStream = new BufferedStream((Stream) gzipStream, 32768))
          MyObjectBuilderSerializer.SerializeXMLInternal((Stream) bufferedStream, objectBuilder, serializeAsType);
      }
    }

    public static bool SerializeXML(
      Stream writeTo,
      MyObjectBuilder_Base objectBuilder,
      MyObjectBuilderSerializer.XmlCompression compress = MyObjectBuilderSerializer.XmlCompression.Uncompressed,
      Type serializeAsType = null)
    {
      try
      {
        switch (compress)
        {
          case MyObjectBuilderSerializer.XmlCompression.Uncompressed:
            MyObjectBuilderSerializer.SerializeXMLInternal(writeTo, objectBuilder, serializeAsType);
            break;
          case MyObjectBuilderSerializer.XmlCompression.Gzip:
            MyObjectBuilderSerializer.SerializeGZippedXMLInternal(writeTo, objectBuilder, serializeAsType);
            break;
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Error during serialization.");
        MyLog.Default.WriteLine(ex.ToString());
        return false;
      }
      return true;
    }

    public static bool SerializeXML(
      string path,
      bool compress,
      MyObjectBuilder_Base objectBuilder,
      Type serializeAsType = null)
    {
      return MyObjectBuilderSerializer.SerializeXML(path, compress, objectBuilder, out ulong _, serializeAsType);
    }

    public static bool SerializeXML(
      string path,
      bool compress,
      MyObjectBuilder_Base objectBuilder,
      out ulong sizeInBytes,
      Type serializeAsType = null)
    {
      try
      {
        using (Stream stream1 = MyFileSystem.OpenWrite(path))
        {
          using (Stream stream2 = compress ? stream1.WrapGZip() : stream1)
          {
            long position = stream1.Position;
            Type type = serializeAsType;
            if ((object) type == null)
              type = objectBuilder.GetType();
            MyXmlSerializerManager.GetSerializer(type).Serialize(stream2, (object) objectBuilder);
            sizeInBytes = (ulong) (stream1.Position - position);
          }
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Error: " + path + " failed to serialize.");
        MyLog.Default.WriteLine(ex.ToString());
        sizeInBytes = 0UL;
        return false;
      }
      return true;
    }

    public static bool DeserializeXML<T>(string path, out T objectBuilder) where T : MyObjectBuilder_Base => MyObjectBuilderSerializer.DeserializeXML<T>(path, out objectBuilder, out ulong _);

    public static bool DeserializeXML<T>(string path, out T objectBuilder, out ulong fileSize) where T : MyObjectBuilder_Base
    {
      bool flag = false;
      fileSize = 0UL;
      objectBuilder = default (T);
      using (Stream stream = MyFileSystem.OpenRead(path))
      {
        if (stream != null)
        {
          using (Stream reader = stream.UnwrapGZip())
          {
            if (reader != null)
            {
              fileSize = (ulong) stream.Length;
              flag = MyObjectBuilderSerializer.DeserializeXML<T>(reader, out objectBuilder);
            }
          }
        }
      }
      if (!flag)
        MyLog.Default.WriteLine(string.Format("Failed to deserialize file '{0}'", (object) path));
      return flag;
    }

    public static bool DeserializeXML<T>(byte[] xmlData, out T objectBuilder, out ulong fileSize) where T : MyObjectBuilder_Base
    {
      bool flag = false;
      fileSize = 0UL;
      objectBuilder = default (T);
      using (MemoryStream stream = new MemoryStream(xmlData))
      {
        if (stream != null)
        {
          using (Stream reader = stream.UnwrapGZip())
          {
            if (reader != null)
            {
              fileSize = (ulong) stream.Length;
              flag = MyObjectBuilderSerializer.DeserializeXML<T>(reader, out objectBuilder);
            }
          }
        }
      }
      if (!flag)
        MyLog.Default.WriteLine("Failed to deserialize a memory file ");
      return flag;
    }

    public static bool DeserializeXML<T>(Stream reader, out T objectBuilder) where T : MyObjectBuilder_Base
    {
      MyObjectBuilder_Base objectBuilder1;
      int num = MyObjectBuilderSerializer.DeserializeXML(reader, out objectBuilder1, typeof (T)) ? 1 : 0;
      objectBuilder = (T) objectBuilder1;
      return num != 0;
    }

    public static bool DeserializeXML(
      string path,
      out MyObjectBuilder_Base objectBuilder,
      Type builderType)
    {
      bool flag = false;
      objectBuilder = (MyObjectBuilder_Base) null;
      using (Stream stream = MyFileSystem.OpenRead(path))
      {
        if (stream != null)
        {
          using (Stream reader = stream.UnwrapGZip())
          {
            if (reader != null)
              flag = MyObjectBuilderSerializer.DeserializeXML(reader, out objectBuilder, builderType);
          }
        }
      }
      if (!flag)
        MyLog.Default.WriteLine(string.Format("Failed to deserialize file '{0}'", (object) path));
      return flag;
    }

    public static bool DeserializeXML(
      Stream reader,
      out MyObjectBuilder_Base objectBuilder,
      Type builderType)
    {
      return MyObjectBuilderSerializer.DeserializeXML(reader, out objectBuilder, builderType, (Dictionary<string, string>) null);
    }

    internal static bool DeserializeXML(
      Stream reader,
      out MyObjectBuilder_Base objectBuilder,
      Type builderType,
      Dictionary<string, string> typeOverrideMap)
    {
      objectBuilder = (MyObjectBuilder_Base) null;
      try
      {
        XmlSerializer serializer = MyXmlSerializerManager.GetSerializer(builderType);
        XmlReaderSettings settings = new XmlReaderSettings()
        {
          CheckCharacters = false
        };
        objectBuilder = (MyObjectBuilder_Base) serializer.Deserialize((XmlReader) new MyXmlTextReader(reader, settings)
        {
          DefinitionTypeOverrideMap = typeOverrideMap
        });
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("ERROR: Exception during objectbuilder read! (xml): " + builderType.Name);
        MyLog.Default.WriteLine(ex);
        return false;
      }
      return true;
    }

    public static bool DeserializeGZippedXML<T>(Stream reader, out T objectBuilder) where T : MyObjectBuilder_Base
    {
      objectBuilder = default (T);
      try
      {
        using (GZipStream gzipStream = new GZipStream(reader, CompressionMode.Decompress))
        {
          using (BufferedStream bufferedStream = new BufferedStream((Stream) gzipStream, 32768))
          {
            XmlSerializer serializer = MyXmlSerializerManager.GetSerializer(typeof (T));
            objectBuilder = (T) serializer.Deserialize((Stream) bufferedStream);
          }
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("ERROR: Exception during objectbuilder read! (xml): " + typeof (T).Name);
        MyLog.Default.WriteLine(ex);
        if (Debugger.IsAttached)
          Debugger.Break();
        return false;
      }
      return true;
    }

    public static void SerializeDynamic(BitStream stream, Type baseType, ref Type obj)
    {
      if (stream.Reading)
      {
        MyRuntimeObjectBuilderId runtimeObjectBuilderId = new MyRuntimeObjectBuilderId(stream.ReadUInt16());
        obj = (Type) (MyObjectBuilderType) runtimeObjectBuilderId;
      }
      else
      {
        MyRuntimeObjectBuilderId runtimeObjectBuilderId = (MyRuntimeObjectBuilderId) (MyObjectBuilderType) obj;
        stream.WriteUInt16(runtimeObjectBuilderId.Value);
      }
    }

    public static bool DeserializePB<T>(string path, out T objectBuilder) where T : MyObjectBuilder_Base => MyObjectBuilderSerializer.DeserializePB<T>(path, out objectBuilder, out ulong _);

    public static bool DeserializePB<T>(string path, out T objectBuilder, out ulong fileSize) where T : MyObjectBuilder_Base
    {
      bool flag = false;
      fileSize = 0UL;
      objectBuilder = default (T);
      using (Stream stream = MyFileSystem.OpenRead(path))
      {
        if (stream != null)
        {
          using (Stream reader = stream.UnwrapGZip())
          {
            if (reader != null)
            {
              fileSize = (ulong) stream.Length;
              flag = MyObjectBuilderSerializer.DeserializePB<T>(reader, out objectBuilder);
            }
          }
        }
      }
      if (!flag)
        MyLog.Default.WriteLine(string.Format("Failed to deserialize file '{0}'", (object) path));
      return flag;
    }

    public static bool DeserializePB<T>(byte[] data, out T objectBuilder, out ulong fileSize) where T : MyObjectBuilder_Base
    {
      bool flag = false;
      fileSize = 0UL;
      objectBuilder = default (T);
      using (MemoryStream stream = new MemoryStream(data))
      {
        if (stream != null)
        {
          using (Stream reader = stream.UnwrapGZip())
          {
            if (reader != null)
            {
              fileSize = (ulong) stream.Length;
              flag = MyObjectBuilderSerializer.DeserializePB<T>(reader, out objectBuilder);
            }
          }
        }
      }
      if (!flag)
        MyLog.Default.WriteLine("Failed to deserialize file from memory");
      return flag;
    }

    public static bool DeserializePB<T>(Stream reader, out T objectBuilder) where T : MyObjectBuilder_Base
    {
      MyObjectBuilder_Base objectBuilder1;
      int num = MyObjectBuilderSerializer.DeserializePB(reader, out objectBuilder1, typeof (T)) ? 1 : 0;
      objectBuilder = (T) objectBuilder1;
      return num != 0;
    }

    internal static bool DeserializePB(
      Stream reader,
      out MyObjectBuilder_Base objectBuilder,
      Type builderType)
    {
      objectBuilder = (MyObjectBuilder_Base) null;
      try
      {
        objectBuilder = MyObjectBuilderSerializer.Serializer.Deserialize(reader, (object) null, builderType) as MyObjectBuilder_Base;
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("ERROR: Exception during objectbuilder read! (pb): " + builderType.Name);
        MyLog.Default.WriteLine(ex);
        return false;
      }
      return true;
    }

    public static bool SerializePB(string path, bool compress, MyObjectBuilder_Base objectBuilder) => MyObjectBuilderSerializer.SerializePB(path, compress, objectBuilder, out ulong _);

    public static bool SerializePB(
      string path,
      bool compress,
      MyObjectBuilder_Base objectBuilder,
      out ulong sizeInBytes)
    {
      try
      {
        using (Stream stream = MyFileSystem.OpenWrite(path))
          return MyObjectBuilderSerializer.SerializePB(stream, compress, objectBuilder, out sizeInBytes);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Error: " + path + " failed to serialize.");
        MyLog.Default.WriteLine(ex.ToString());
        sizeInBytes = 0UL;
        return false;
      }
    }

    public static bool SerializePB(
      Stream stream,
      bool compress,
      MyObjectBuilder_Base objectBuilder,
      out ulong sizeInBytes)
    {
      try
      {
        using (Stream dest = compress ? stream.WrapGZip() : stream)
        {
          long position = stream.Position;
          MyObjectBuilderSerializer.Serializer.Serialize(dest, (object) objectBuilder);
          sizeInBytes = (ulong) (stream.Position - position);
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine("Error: failed to serialize.");
        MyLog.Default.WriteLine(ex.ToString());
        sizeInBytes = 0UL;
        return false;
      }
      return true;
    }

    public static bool SerializePB(Stream stream, MyObjectBuilder_Base objectBuilder)
    {
      try
      {
        MyObjectBuilderSerializer.Serializer.Serialize(stream, (object) objectBuilder);
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(ex.ToString());
        return false;
      }
      return true;
    }

    public static MyObjectBuilder_Base CreateNewObject(
      SerializableDefinitionId id)
    {
      return MyObjectBuilderSerializer.CreateNewObject(id.TypeId, id.SubtypeId);
    }

    public static MyObjectBuilder_Base CreateNewObject(
      MyObjectBuilderType type,
      string subtypeName)
    {
      MyObjectBuilder_Base newObject = MyObjectBuilderSerializer.CreateNewObject(type);
      newObject.SubtypeName = subtypeName;
      return newObject;
    }

    public static MyObjectBuilder_Base CreateNewObject(MyObjectBuilderType type) => MyObjectBuilderSerializer.m_objectFactory.CreateInstance(type);

    public static T CreateNewObject<T>(string subtypeName) where T : MyObjectBuilder_Base, new()
    {
      T newObject = MyObjectBuilderSerializer.CreateNewObject<T>();
      newObject.SubtypeName = subtypeName;
      return newObject;
    }

    public static T CreateNewObject<T>() where T : MyObjectBuilder_Base, new() => MyObjectBuilderSerializer.m_objectFactory.CreateInstance<T>();

    public static MyObjectBuilder_Base Clone(MyObjectBuilder_Base toClone)
    {
      MyObjectBuilder_Base objectBuilder = (MyObjectBuilder_Base) null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        if (MyObjectBuilderSerializer.ENABLE_PROTOBUFFERS_CLONING && MyObjectBuilderSerializer.Serializer != null)
        {
          MyObjectBuilderSerializer.Serializer.Serialize((Stream) memoryStream, (object) toClone);
          memoryStream.Position = 0L;
          MyObjectBuilderSerializer.DeserializePB((Stream) memoryStream, out objectBuilder, toClone.GetType());
        }
        else
        {
          MyObjectBuilderSerializer.SerializeXMLInternal((Stream) memoryStream, toClone);
          memoryStream.Position = 0L;
          MyObjectBuilderSerializer.DeserializeXML((Stream) memoryStream, out objectBuilder, toClone.GetType());
        }
      }
      return objectBuilder;
    }

    public static void UnregisterAssembliesAndSerializers()
    {
      MyObjectBuilderSerializer.m_objectFactory = new MyObjectFactory<MyObjectBuilderDefinitionAttribute, MyObjectBuilder_Base>();
      MyObjectBuilderSerializer.m_typeModel.FlushCaches();
    }

    public static MemoryStream SerializePB(
      bool compressed,
      MyObjectBuilder_Base objectBuilder)
    {
      MemoryStream memoryStream = new MemoryStream();
      return MyObjectBuilderSerializer.SerializePB((Stream) memoryStream, compressed, objectBuilder, out ulong _) ? memoryStream : (MemoryStream) null;
    }

    public enum XmlCompression
    {
      Uncompressed,
      Gzip,
    }
  }
}
