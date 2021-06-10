// Decompiled with JetBrains decompiler
// Type: LitJson.JsonMapper
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace LitJson
{
  public class JsonMapper
  {
    private static readonly int max_nesting_depth;
    private static readonly IFormatProvider datetime_format;
    private static readonly IDictionary<Type, ExporterFunc> base_exporters_table;
    private static readonly IDictionary<Type, ExporterFunc> custom_exporters_table;
    private static readonly IDictionary<Type, IDictionary<Type, ImporterFunc>> base_importers_table;
    private static readonly IDictionary<Type, IDictionary<Type, ImporterFunc>> custom_importers_table;
    private static readonly IDictionary<Type, ArrayMetadata> array_metadata;
    private static readonly object array_metadata_lock = new object();
    private static readonly IDictionary<Type, IDictionary<Type, MethodInfo>> conv_ops;
    private static readonly object conv_ops_lock = new object();
    private static readonly IDictionary<Type, ObjectMetadata> object_metadata;
    private static readonly object object_metadata_lock = new object();
    private static readonly IDictionary<Type, IList<PropertyMetadata>> type_properties;
    private static readonly object type_properties_lock = new object();
    private static readonly JsonWriter static_writer;
    private static readonly object static_writer_lock = new object();

    static JsonMapper()
    {
      JsonMapper.max_nesting_depth = 100;
      JsonMapper.array_metadata = (IDictionary<Type, ArrayMetadata>) new Dictionary<Type, ArrayMetadata>();
      JsonMapper.conv_ops = (IDictionary<Type, IDictionary<Type, MethodInfo>>) new Dictionary<Type, IDictionary<Type, MethodInfo>>();
      JsonMapper.object_metadata = (IDictionary<Type, ObjectMetadata>) new Dictionary<Type, ObjectMetadata>();
      JsonMapper.type_properties = (IDictionary<Type, IList<PropertyMetadata>>) new Dictionary<Type, IList<PropertyMetadata>>();
      JsonMapper.static_writer = new JsonWriter();
      JsonMapper.datetime_format = (IFormatProvider) DateTimeFormatInfo.InvariantInfo;
      JsonMapper.base_exporters_table = (IDictionary<Type, ExporterFunc>) new Dictionary<Type, ExporterFunc>();
      JsonMapper.custom_exporters_table = (IDictionary<Type, ExporterFunc>) new Dictionary<Type, ExporterFunc>();
      JsonMapper.base_importers_table = (IDictionary<Type, IDictionary<Type, ImporterFunc>>) new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
      JsonMapper.custom_importers_table = (IDictionary<Type, IDictionary<Type, ImporterFunc>>) new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
      JsonMapper.RegisterBaseExporters();
      JsonMapper.RegisterBaseImporters();
    }

    private static void AddArrayMetadata(Type type)
    {
      if (JsonMapper.array_metadata.ContainsKey(type))
        return;
      ArrayMetadata arrayMetadata = new ArrayMetadata();
      arrayMetadata.IsArray = type.IsArray;
      if (type.GetInterface("System.Collections.IList") != (Type) null)
        arrayMetadata.IsList = true;
      if (type == typeof (object))
      {
        arrayMetadata.IsArray = true;
        arrayMetadata.ElementType = (Type) null;
      }
      else
      {
        foreach (PropertyInfo property in type.GetProperties())
        {
          if (!(property.Name != "Item"))
          {
            ParameterInfo[] indexParameters = property.GetIndexParameters();
            if (indexParameters.Length == 1 && indexParameters[0].ParameterType == typeof (int))
              arrayMetadata.ElementType = property.PropertyType;
          }
        }
      }
      lock (JsonMapper.array_metadata_lock)
      {
        try
        {
          JsonMapper.array_metadata.Add(type, arrayMetadata);
        }
        catch (ArgumentException ex)
        {
        }
      }
    }

    private static void AddObjectMetadata(Type type)
    {
      if (JsonMapper.object_metadata.ContainsKey(type))
        return;
      ObjectMetadata objectMetadata = new ObjectMetadata();
      if (type.GetInterface("System.Collections.IDictionary") != (Type) null)
        objectMetadata.IsDictionary = true;
      objectMetadata.Properties = (IDictionary<string, PropertyMetadata>) new Dictionary<string, PropertyMetadata>();
      foreach (PropertyInfo property in type.GetProperties())
      {
        if (property.Name == "Item")
        {
          ParameterInfo[] indexParameters = property.GetIndexParameters();
          if (indexParameters.Length == 1 && indexParameters[0].ParameterType == typeof (string))
            objectMetadata.ElementType = property.PropertyType;
        }
        else
          objectMetadata.Properties.Add(property.Name, new PropertyMetadata()
          {
            Info = (MemberInfo) property,
            Type = property.PropertyType
          });
      }
      foreach (FieldInfo field in type.GetFields())
        objectMetadata.Properties.Add(field.Name, new PropertyMetadata()
        {
          Info = (MemberInfo) field,
          IsField = true,
          Type = field.FieldType
        });
      lock (JsonMapper.object_metadata_lock)
      {
        try
        {
          JsonMapper.object_metadata.Add(type, objectMetadata);
        }
        catch (ArgumentException ex)
        {
        }
      }
    }

    private static void AddTypeProperties(Type type)
    {
      if (JsonMapper.type_properties.ContainsKey(type))
        return;
      IList<PropertyMetadata> propertyMetadataList = (IList<PropertyMetadata>) new List<PropertyMetadata>();
      foreach (PropertyInfo property in type.GetProperties())
      {
        if (!(property.Name == "Item"))
          propertyMetadataList.Add(new PropertyMetadata()
          {
            Info = (MemberInfo) property,
            IsField = false
          });
      }
      foreach (FieldInfo field in type.GetFields())
        propertyMetadataList.Add(new PropertyMetadata()
        {
          Info = (MemberInfo) field,
          IsField = true
        });
      lock (JsonMapper.type_properties_lock)
      {
        try
        {
          JsonMapper.type_properties.Add(type, propertyMetadataList);
        }
        catch (ArgumentException ex)
        {
        }
      }
    }

    private static MethodInfo GetConvOp(Type t1, Type t2)
    {
      lock (JsonMapper.conv_ops_lock)
      {
        if (!JsonMapper.conv_ops.ContainsKey(t1))
          JsonMapper.conv_ops.Add(t1, (IDictionary<Type, MethodInfo>) new Dictionary<Type, MethodInfo>());
      }
      if (JsonMapper.conv_ops[t1].ContainsKey(t2))
        return JsonMapper.conv_ops[t1][t2];
      MethodInfo method = t1.GetMethod("op_Implicit", new Type[1]
      {
        t2
      });
      lock (JsonMapper.conv_ops_lock)
      {
        try
        {
          JsonMapper.conv_ops[t1].Add(t2, method);
        }
        catch (ArgumentException ex)
        {
          return JsonMapper.conv_ops[t1][t2];
        }
      }
      return method;
    }

    private static object ReadValue(Type inst_type, JsonReader reader)
    {
      reader.Read();
      if (reader.Token == JsonToken.ArrayEnd)
        return (object) null;
      Type underlyingType = Nullable.GetUnderlyingType(inst_type);
      Type type1 = underlyingType;
      if ((object) type1 == null)
        type1 = inst_type;
      Type type2 = type1;
      if (reader.Token == JsonToken.Null)
      {
        if (inst_type.IsClass || underlyingType != (Type) null)
          return (object) null;
        throw new JsonException(string.Format("Can't assign null to an instance of type {0}", (object) inst_type));
      }
      if (reader.Token == JsonToken.Double || reader.Token == JsonToken.Int || (reader.Token == JsonToken.Long || reader.Token == JsonToken.String) || reader.Token == JsonToken.Boolean)
      {
        Type type3 = reader.Value.GetType();
        if (type2.IsAssignableFrom(type3))
          return reader.Value;
        if (JsonMapper.custom_importers_table.ContainsKey(type3) && JsonMapper.custom_importers_table[type3].ContainsKey(type2))
          return JsonMapper.custom_importers_table[type3][type2](reader.Value);
        if (JsonMapper.base_importers_table.ContainsKey(type3) && JsonMapper.base_importers_table[type3].ContainsKey(type2))
          return JsonMapper.base_importers_table[type3][type2](reader.Value);
        if (type2.IsEnum)
          return Enum.ToObject(type2, reader.Value);
        MethodInfo convOp = JsonMapper.GetConvOp(type2, type3);
        if (!(convOp != (MethodInfo) null))
          throw new JsonException(string.Format("Can't assign value '{0}' (type {1}) to type {2}", reader.Value, (object) type3, (object) inst_type));
        return convOp.Invoke((object) null, new object[1]
        {
          reader.Value
        });
      }
      object obj1 = (object) null;
      if (reader.Token == JsonToken.ArrayStart)
      {
        JsonMapper.AddArrayMetadata(inst_type);
        ArrayMetadata arrayMetadata = JsonMapper.array_metadata[inst_type];
        if (!arrayMetadata.IsArray && !arrayMetadata.IsList)
          throw new JsonException(string.Format("Type {0} can't act as an array", (object) inst_type));
        IList list;
        Type elementType;
        if (arrayMetadata.IsArray)
        {
          list = (IList) new ArrayList();
          elementType = inst_type.GetElementType();
        }
        else
        {
          list = (IList) Activator.CreateInstance(inst_type);
          elementType = arrayMetadata.ElementType;
        }
        while (true)
        {
          Type inst_type1 = elementType;
          if ((object) inst_type1 == null)
            inst_type1 = typeof (object);
          object obj2 = JsonMapper.ReadValue(inst_type1, reader);
          if (obj2 != null || reader.Token != JsonToken.ArrayEnd)
            list.Add(obj2);
          else
            break;
        }
        if (arrayMetadata.IsArray)
        {
          if (elementType == (Type) null)
          {
            foreach (object obj2 in (IEnumerable) list)
            {
              Type type3 = obj2.GetType();
              if (elementType == (Type) null)
                elementType = type3;
              else if (elementType != type3)
              {
                elementType = typeof (object);
                break;
              }
            }
            if (elementType == (Type) null)
              elementType = typeof (object);
          }
          int count = list.Count;
          obj1 = (object) Array.CreateInstance(elementType, count);
          for (int index = 0; index < count; ++index)
            ((Array) obj1).SetValue(list[index], index);
        }
        else
          obj1 = (object) list;
      }
      else if (reader.Token == JsonToken.ObjectStart)
      {
        JsonMapper.AddObjectMetadata(type2);
        ObjectMetadata objectMetadata = JsonMapper.object_metadata[type2];
        obj1 = Activator.CreateInstance(type2);
        string key;
        while (true)
        {
          reader.Read();
          if (reader.Token != JsonToken.ObjectEnd)
          {
            key = (string) reader.Value;
            if (objectMetadata.Properties.ContainsKey(key))
            {
              PropertyMetadata property = objectMetadata.Properties[key];
              if (property.IsField)
              {
                ((FieldInfo) property.Info).SetValue(obj1, JsonMapper.ReadValue(property.Type, reader));
              }
              else
              {
                PropertyInfo info = (PropertyInfo) property.Info;
                if (info.CanWrite)
                  info.SetValue(obj1, JsonMapper.ReadValue(property.Type, reader), (object[]) null);
                else
                  JsonMapper.ReadValue(property.Type, reader);
              }
            }
            else if (!objectMetadata.IsDictionary)
            {
              if (reader.SkipNonMembers)
                JsonMapper.ReadSkip(reader);
              else
                break;
            }
            else
              ((IDictionary) obj1).Add((object) key, JsonMapper.ReadValue(objectMetadata.ElementType, reader));
          }
          else
            goto label_62;
        }
        throw new JsonException(string.Format("The type {0} doesn't have the property '{1}'", (object) inst_type, (object) key));
      }
label_62:
      return obj1;
    }

    private static IJsonWrapper ReadValue(WrapperFactory factory, JsonReader reader)
    {
      reader.Read();
      if (reader.Token == JsonToken.ArrayEnd || reader.Token == JsonToken.Null)
        return (IJsonWrapper) null;
      IJsonWrapper jsonWrapper1 = factory();
      if (reader.Token == JsonToken.String)
      {
        jsonWrapper1.SetString((string) reader.Value);
        return jsonWrapper1;
      }
      if (reader.Token == JsonToken.Double)
      {
        jsonWrapper1.SetDouble((double) reader.Value);
        return jsonWrapper1;
      }
      if (reader.Token == JsonToken.Int)
      {
        jsonWrapper1.SetInt((int) reader.Value);
        return jsonWrapper1;
      }
      if (reader.Token == JsonToken.Long)
      {
        jsonWrapper1.SetLong((long) reader.Value);
        return jsonWrapper1;
      }
      if (reader.Token == JsonToken.Boolean)
      {
        jsonWrapper1.SetBoolean((bool) reader.Value);
        return jsonWrapper1;
      }
      if (reader.Token == JsonToken.ArrayStart)
      {
        jsonWrapper1.SetJsonType(JsonType.Array);
        while (true)
        {
          IJsonWrapper jsonWrapper2 = JsonMapper.ReadValue(factory, reader);
          if (jsonWrapper2 != null || reader.Token != JsonToken.ArrayEnd)
            jsonWrapper1.Add((object) jsonWrapper2);
          else
            break;
        }
      }
      else if (reader.Token == JsonToken.ObjectStart)
      {
        jsonWrapper1.SetJsonType(JsonType.Object);
        while (true)
        {
          reader.Read();
          if (reader.Token != JsonToken.ObjectEnd)
          {
            string str = (string) reader.Value;
            jsonWrapper1[(object) str] = (object) JsonMapper.ReadValue(factory, reader);
          }
          else
            break;
        }
      }
      return jsonWrapper1;
    }

    private static void ReadSkip(JsonReader reader) => JsonMapper.ToWrapper((WrapperFactory) (() => (IJsonWrapper) new JsonMockWrapper()), reader);

    private static void RegisterBaseExporters()
    {
      JsonMapper.base_exporters_table[typeof (byte)] = (ExporterFunc) ((obj, writer) => writer.Write(Convert.ToInt32((byte) obj)));
      JsonMapper.base_exporters_table[typeof (char)] = (ExporterFunc) ((obj, writer) => writer.Write(Convert.ToString((char) obj)));
      JsonMapper.base_exporters_table[typeof (DateTime)] = (ExporterFunc) ((obj, writer) => writer.Write(Convert.ToString((DateTime) obj, JsonMapper.datetime_format)));
      JsonMapper.base_exporters_table[typeof (Decimal)] = (ExporterFunc) ((obj, writer) => writer.Write((Decimal) obj));
      JsonMapper.base_exporters_table[typeof (sbyte)] = (ExporterFunc) ((obj, writer) => writer.Write(Convert.ToInt32((sbyte) obj)));
      JsonMapper.base_exporters_table[typeof (short)] = (ExporterFunc) ((obj, writer) => writer.Write(Convert.ToInt32((short) obj)));
      JsonMapper.base_exporters_table[typeof (ushort)] = (ExporterFunc) ((obj, writer) => writer.Write(Convert.ToInt32((ushort) obj)));
      JsonMapper.base_exporters_table[typeof (uint)] = (ExporterFunc) ((obj, writer) => writer.Write(Convert.ToUInt64((uint) obj)));
      JsonMapper.base_exporters_table[typeof (ulong)] = (ExporterFunc) ((obj, writer) => writer.Write((ulong) obj));
    }

    private static void RegisterBaseImporters()
    {
      ImporterFunc importer1 = (ImporterFunc) (input => (object) Convert.ToByte((int) input));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (int), typeof (byte), importer1);
      ImporterFunc importer2 = (ImporterFunc) (input => (object) Convert.ToUInt64((int) input));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (int), typeof (ulong), importer2);
      ImporterFunc importer3 = (ImporterFunc) (input => (object) Convert.ToInt64((int) input));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (int), typeof (long), importer3);
      ImporterFunc importer4 = (ImporterFunc) (input => (object) Convert.ToSByte((int) input));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (int), typeof (sbyte), importer4);
      ImporterFunc importer5 = (ImporterFunc) (input => (object) Convert.ToInt16((int) input));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (int), typeof (short), importer5);
      ImporterFunc importer6 = (ImporterFunc) (input => (object) Convert.ToUInt16((int) input));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (int), typeof (ushort), importer6);
      ImporterFunc importer7 = (ImporterFunc) (input => (object) Convert.ToUInt32((int) input));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (int), typeof (uint), importer7);
      ImporterFunc importer8 = (ImporterFunc) (input => (object) Convert.ToSingle((int) input));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (int), typeof (float), importer8);
      ImporterFunc importer9 = (ImporterFunc) (input => (object) Convert.ToDouble((int) input));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (int), typeof (double), importer9);
      ImporterFunc importer10 = (ImporterFunc) (input => (object) Convert.ToDecimal((double) input));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (double), typeof (Decimal), importer10);
      ImporterFunc importer11 = (ImporterFunc) (input => (object) Convert.ToUInt32((long) input));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (long), typeof (uint), importer11);
      ImporterFunc importer12 = (ImporterFunc) (input => (object) Convert.ToChar((string) input));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (string), typeof (char), importer12);
      ImporterFunc importer13 = (ImporterFunc) (input => (object) Convert.ToDateTime((string) input, JsonMapper.datetime_format));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (string), typeof (DateTime), importer13);
      ImporterFunc importer14 = (ImporterFunc) (input => (object) DateTimeOffset.Parse((string) input, JsonMapper.datetime_format));
      JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof (string), typeof (DateTimeOffset), importer14);
    }

    private static void RegisterImporter(
      IDictionary<Type, IDictionary<Type, ImporterFunc>> table,
      Type json_type,
      Type value_type,
      ImporterFunc importer)
    {
      if (!table.ContainsKey(json_type))
        table.Add(json_type, (IDictionary<Type, ImporterFunc>) new Dictionary<Type, ImporterFunc>());
      table[json_type][value_type] = importer;
    }

    private static void WriteValue(
      object obj,
      JsonWriter writer,
      bool writer_is_private,
      int depth)
    {
      if (depth > JsonMapper.max_nesting_depth)
        throw new JsonException(string.Format("Max allowed object depth reached while trying to export from type {0}", (object) obj.GetType()));
      switch (obj)
      {
        case null:
          writer.Write((string) null);
          break;
        case IJsonWrapper _:
          if (writer_is_private)
          {
            writer.TextWriter.Write(((IJsonWrapper) obj).ToJson());
            break;
          }
          ((IJsonWrapper) obj).ToJson(writer);
          break;
        case string _:
          writer.Write((string) obj);
          break;
        case float num:
          writer.Write((double) num);
          break;
        case double number:
          writer.Write(number);
          break;
        case int number:
          writer.Write(number);
          break;
        case bool boolean:
          writer.Write(boolean);
          break;
        case long number:
          writer.Write(number);
          break;
        case Array _:
          writer.WriteArrayStart();
          foreach (object obj1 in (Array) obj)
            JsonMapper.WriteValue(obj1, writer, writer_is_private, depth + 1);
          writer.WriteArrayEnd();
          break;
        case IList _:
          writer.WriteArrayStart();
          foreach (object obj1 in (IEnumerable) obj)
            JsonMapper.WriteValue(obj1, writer, writer_is_private, depth + 1);
          writer.WriteArrayEnd();
          break;
        case IDictionary _:
          writer.WriteObjectStart();
          foreach (DictionaryEntry dictionaryEntry in (IDictionary) obj)
          {
            writer.WritePropertyName((string) dictionaryEntry.Key);
            JsonMapper.WriteValue(dictionaryEntry.Value, writer, writer_is_private, depth + 1);
          }
          writer.WriteObjectEnd();
          break;
        default:
          Type type = obj.GetType();
          if (JsonMapper.custom_exporters_table.ContainsKey(type))
          {
            JsonMapper.custom_exporters_table[type](obj, writer);
            break;
          }
          if (JsonMapper.base_exporters_table.ContainsKey(type))
          {
            JsonMapper.base_exporters_table[type](obj, writer);
            break;
          }
          if (obj is Enum)
          {
            Type underlyingType = Enum.GetUnderlyingType(type);
            if (underlyingType == typeof (long) || underlyingType == typeof (uint) || underlyingType == typeof (ulong))
            {
              writer.Write((ulong) obj);
              break;
            }
            writer.Write((int) obj);
            break;
          }
          JsonMapper.AddTypeProperties(type);
          IList<PropertyMetadata> typeProperty = JsonMapper.type_properties[type];
          writer.WriteObjectStart();
          foreach (PropertyMetadata propertyMetadata in (IEnumerable<PropertyMetadata>) typeProperty)
          {
            if (propertyMetadata.IsField)
            {
              writer.WritePropertyName(propertyMetadata.Info.Name);
              JsonMapper.WriteValue(((FieldInfo) propertyMetadata.Info).GetValue(obj), writer, writer_is_private, depth + 1);
            }
            else
            {
              PropertyInfo info = (PropertyInfo) propertyMetadata.Info;
              if (info.CanRead)
              {
                writer.WritePropertyName(propertyMetadata.Info.Name);
                JsonMapper.WriteValue(info.GetValue(obj, (object[]) null), writer, writer_is_private, depth + 1);
              }
            }
          }
          writer.WriteObjectEnd();
          break;
      }
    }

    public static string ToJson(object obj)
    {
      lock (JsonMapper.static_writer_lock)
      {
        JsonMapper.static_writer.Reset();
        JsonMapper.WriteValue(obj, JsonMapper.static_writer, true, 0);
        return JsonMapper.static_writer.ToString();
      }
    }

    public static void ToJson(object obj, JsonWriter writer) => JsonMapper.WriteValue(obj, writer, false, 0);

    public static JsonData ToObject(JsonReader reader) => (JsonData) JsonMapper.ToWrapper((WrapperFactory) (() => (IJsonWrapper) new JsonData()), reader);

    public static JsonData ToObject(TextReader reader) => (JsonData) JsonMapper.ToWrapper((WrapperFactory) (() => (IJsonWrapper) new JsonData()), new JsonReader(reader));

    public static JsonData ToObject(string json) => (JsonData) JsonMapper.ToWrapper((WrapperFactory) (() => (IJsonWrapper) new JsonData()), json);

    public static T ToObject<T>(JsonReader reader) => (T) JsonMapper.ReadValue(typeof (T), reader);

    public static T ToObject<T>(TextReader reader) => (T) JsonMapper.ReadValue(typeof (T), new JsonReader(reader));

    public static T ToObject<T>(string json) => (T) JsonMapper.ReadValue(typeof (T), new JsonReader(json));

    public static object ToObject(string json, Type ConvertType)
    {
      JsonReader reader = new JsonReader(json);
      return JsonMapper.ReadValue(ConvertType, reader);
    }

    public static IJsonWrapper ToWrapper(WrapperFactory factory, JsonReader reader) => JsonMapper.ReadValue(factory, reader);

    public static IJsonWrapper ToWrapper(WrapperFactory factory, string json)
    {
      JsonReader reader = new JsonReader(json);
      return JsonMapper.ReadValue(factory, reader);
    }

    public static void RegisterExporter<T>(ExporterFunc<T> exporter)
    {
      ExporterFunc exporterFunc = (ExporterFunc) ((obj, writer) => exporter((T) obj, writer));
      JsonMapper.custom_exporters_table[typeof (T)] = exporterFunc;
    }

    public static void RegisterImporter<TJson, TValue>(ImporterFunc<TJson, TValue> importer)
    {
      ImporterFunc importer1 = (ImporterFunc) (input => (object) importer((TJson) input));
      JsonMapper.RegisterImporter(JsonMapper.custom_importers_table, typeof (TJson), typeof (TValue), importer1);
    }

    public static void UnregisterExporters() => JsonMapper.custom_exporters_table.Clear();

    public static void UnregisterImporters() => JsonMapper.custom_importers_table.Clear();
  }
}
