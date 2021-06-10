// Decompiled with JetBrains decompiler
// Type: VRage.MyXmlSerializerManager
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using VRage.Utils;

namespace VRage
{
  public class MyXmlSerializerManager
  {
    private static readonly HashSet<Type> m_serializableBaseTypes = new HashSet<Type>();
    private static readonly Dictionary<Type, XmlSerializer> m_serializersByType = new Dictionary<Type, XmlSerializer>();
    private static readonly Dictionary<string, XmlSerializer> m_serializersBySerializedName = new Dictionary<string, XmlSerializer>();
    private static readonly Dictionary<Type, string> m_serializedNameByType = new Dictionary<Type, string>();
    private static HashSet<Assembly> m_registeredAssemblies = new HashSet<Assembly>();

    public static void RegisterSerializer(Type type)
    {
      if (MyXmlSerializerManager.m_serializersByType.ContainsKey(type))
        return;
      MyXmlSerializerManager.RegisterType(type, true, false);
    }

    public static void RegisterSerializableBaseType(Type type) => MyXmlSerializerManager.m_serializableBaseTypes.Add(type);

    public static void RegisterFromAssembly(Assembly assembly)
    {
      if (assembly == (Assembly) null || MyXmlSerializerManager.m_registeredAssemblies.Contains(assembly))
        return;
      MyXmlSerializerManager.m_registeredAssemblies.Add(assembly);
      foreach (Type type in assembly.GetTypes())
      {
        try
        {
          if (!MyXmlSerializerManager.m_serializersByType.ContainsKey(type))
            MyXmlSerializerManager.RegisterType(type);
        }
        catch (Exception ex)
        {
          throw new InvalidOperationException("Error creating XML serializer for type " + type.Name, ex);
        }
      }
    }

    public static XmlSerializer GetSerializer(Type type) => MyXmlSerializerManager.m_serializersByType[type];

    public static XmlSerializer GetOrCreateSerializer(Type type)
    {
      XmlSerializer xmlSerializer;
      if (!MyXmlSerializerManager.m_serializersByType.TryGetValue(type, out xmlSerializer))
        xmlSerializer = MyXmlSerializerManager.RegisterType(type, true);
      return xmlSerializer;
    }

    public static string GetSerializedName(Type type) => MyXmlSerializerManager.m_serializedNameByType[type];

    public static bool TryGetSerializer(string serializedName, out XmlSerializer serializer) => MyXmlSerializerManager.m_serializersBySerializedName.TryGetValue(serializedName, out serializer);

    public static XmlSerializer GetSerializer(string serializedName) => MyXmlSerializerManager.m_serializersBySerializedName[serializedName];

    public static bool IsSerializerAvailable(string name) => MyXmlSerializerManager.m_serializersBySerializedName.ContainsKey(name);

    private static XmlSerializer RegisterType(
      Type type,
      bool forceRegister = false,
      bool checkAttributes = true)
    {
      string key = (string) null;
      if (checkAttributes)
      {
        object[] customAttributes = type.GetCustomAttributes(typeof (XmlTypeAttribute), false);
        if (customAttributes.Length != 0)
        {
          XmlTypeAttribute xmlTypeAttribute = (XmlTypeAttribute) customAttributes[0];
          key = type.Name;
          if (!string.IsNullOrEmpty(xmlTypeAttribute.TypeName))
            key = xmlTypeAttribute.TypeName;
        }
        else
        {
          foreach (Type serializableBaseType in MyXmlSerializerManager.m_serializableBaseTypes)
          {
            if (serializableBaseType.IsAssignableFrom(type))
            {
              key = type.Name;
              break;
            }
          }
        }
      }
      if (key == null)
      {
        if (!forceRegister)
          return (XmlSerializer) null;
        key = type.Name;
      }
      XmlSerializer xmlSerializer = (XmlSerializer) null;
      foreach (Attribute andParentAttribute in MyXmlSerializerManager.EnumerateThisAndParentAttributes(type))
      {
        Type type1 = andParentAttribute.GetType();
        if (type1.Name == "XmlSerializerAssemblyAttribute")
        {
          xmlSerializer = MyXmlSerializerManager.TryLoadSerializerFrom((string) type1.GetProperty("AssemblyName").GetValue((object) andParentAttribute), type.Name);
          if (xmlSerializer != null)
            break;
        }
      }
      if (xmlSerializer == null)
      {
        string assemblyName = type.Assembly.GetName().Name + ".XmlSerializers";
        MyLog.Default.Error("Type {0} is missing missing XmlSerializerAssemblyAttribute. Falling back to default {1}", (object) type.Name, (object) assemblyName);
        xmlSerializer = MyXmlSerializerManager.TryLoadSerializerFrom(assemblyName, type.Name);
      }
      if (xmlSerializer == null)
        xmlSerializer = new XmlSerializer(type);
      MyXmlSerializerManager.m_serializersByType.Add(type, xmlSerializer);
      MyXmlSerializerManager.m_serializersBySerializedName.Add(key, xmlSerializer);
      MyXmlSerializerManager.m_serializedNameByType.Add(type, key);
      return xmlSerializer;
    }

    private static XmlSerializer TryLoadSerializerFrom(
      string assemblyName,
      string typeName)
    {
      Assembly assembly = (Assembly) null;
      try
      {
        assembly = Assembly.Load(new AssemblyName(assemblyName));
      }
      catch
      {
      }
      if (assembly == (Assembly) null)
      {
        try
        {
          assembly = Assembly.Load(assemblyName);
        }
        catch
        {
        }
      }
      if (assembly == (Assembly) null)
        return (XmlSerializer) null;
      Type type = assembly.GetType("Microsoft.Xml.Serialization.GeneratedAssembly." + typeName + "Serializer");
      return type != (Type) null ? (XmlSerializer) Activator.CreateInstance(type) : (XmlSerializer) null;
    }

    private static IEnumerable<Attribute> EnumerateThisAndParentAttributes(
      Type type)
    {
      if (!(type == (Type) null))
      {
        foreach (Attribute customAttribute in type.GetCustomAttributes())
          yield return customAttribute;
        foreach (Attribute andParentAttribute in MyXmlSerializerManager.EnumerateThisAndParentAttributes(type.DeclaringType))
          yield return andParentAttribute;
      }
    }
  }
}
