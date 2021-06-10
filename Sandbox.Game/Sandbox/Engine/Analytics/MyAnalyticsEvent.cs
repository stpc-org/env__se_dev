// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Analytics.MyAnalyticsEvent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using VRage.Analytics;

namespace Sandbox.Engine.Analytics
{
  public abstract class MyAnalyticsEvent : IMyAnalyticsEvent
  {
    private static readonly List<TypeInfo> m_subtypes = MyAnalyticsEvent.GetSubtypes();
    private static readonly HashSet<string> m_allEventProperties = MyAnalyticsEvent.GetAllEventProperties();
    private static readonly List<Type> m_supportedTypes = new List<Type>()
    {
      typeof (string),
      typeof (bool?),
      typeof (bool),
      typeof (int?),
      typeof (uint?),
      typeof (long?),
      typeof (ulong?),
      typeof (int),
      typeof (uint),
      typeof (long),
      typeof (ulong),
      typeof (double?),
      typeof (double),
      typeof (DateTime?),
      typeof (byte?),
      typeof (byte)
    };

    static MyAnalyticsEvent()
    {
      foreach (Type type in MyAnalyticsEvent.EnumerateTypesWithSupportedTypeAttribute())
        MyAnalyticsEvent.m_supportedTypes.Add(type);
    }

    private static IEnumerable<Type> EnumerateTypesWithSupportedTypeAttribute()
    {
      Type[] typeArray = Assembly.GetAssembly(typeof (MyAnalyticsEvent)).GetTypes();
      for (int index = 0; index < typeArray.Length; ++index)
      {
        Type type = typeArray[index];
        if (type.GetCustomAttributes(typeof (SupportedTypeAttribute), true).Length != 0)
          yield return type;
      }
      typeArray = (Type[]) null;
    }

    private static List<TypeInfo> GetSubtypes()
    {
      List<TypeInfo> typeInfoList = new List<TypeInfo>();
      foreach (Type type in typeof (MyAnalyticsEvent).Assembly.GetTypes())
      {
        if (type.IsSubclassOf(typeof (MyAnalyticsEvent)))
          typeInfoList.Add(type.GetTypeInfo());
      }
      return typeInfoList;
    }

    private static HashSet<string> GetAllEventProperties()
    {
      HashSet<string> stringSet = new HashSet<string>();
      foreach (Type subtype in MyAnalyticsEvent.m_subtypes)
      {
        foreach (PropertyInfo property in subtype.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
        {
          if (stringSet.Contains(property.Name) && !MyAnalyticsEvent.m_subtypes.Contains(property.PropertyType.GetTypeInfo()))
            throw new MyAnalyticsSpecificationException("Duplicate declaration of MyAnalyticsEvent property '" + property.Name + "'. All event properties should by unique");
          stringSet.Add(property.Name);
        }
      }
      return stringSet;
    }

    public abstract string GetEventName();

    public virtual MyReportTypeData GetReportTypeAndArgs() => new MyReportTypeData(MyReportType.ProgressionUndefined, "Game", this.GetEventName());

    public Dictionary<string, object> GetPropertiesDictionary()
    {
      PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      foreach (PropertyInfo propertyInfo in properties)
      {
        object propertyValue = propertyInfo.GetValue((object) this, (object[]) null);
        if (propertyValue == null)
        {
          if (Attribute.IsDefined((MemberInfo) propertyInfo, typeof (RequiredAttribute)))
            throw new MyAnalyticsSpecificationException("The [Required] property '" + propertyInfo.Name + "' is null");
        }
        else
        {
          foreach (KeyValuePair<string, object> keyValuePair in this.FlattenAndValidateProperty(propertyInfo, propertyValue))
          {
            if (keyValuePair.Value != null)
              dictionary.Add(keyValuePair.Key, keyValuePair.Value);
          }
        }
      }
      return dictionary;
    }

    private Dictionary<string, object> FlattenAndValidateProperty(
      PropertyInfo propertyInfo,
      object propertyValue)
    {
      if (MyAnalyticsEvent.m_subtypes.Contains(propertyInfo.PropertyType.GetTypeInfo()))
        return ((MyAnalyticsEvent) propertyValue).GetPropertiesDictionary();
      if (MyAnalyticsEvent.m_supportedTypes.Contains(propertyInfo.PropertyType))
        return new Dictionary<string, object>()
        {
          {
            propertyInfo.Name,
            propertyValue
          }
        };
      if (propertyInfo.PropertyType.IsEnum)
        return new Dictionary<string, object>()
        {
          {
            propertyInfo.Name,
            (object) propertyValue.ToString()
          }
        };
      if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof (Dictionary<,>) && (propertyInfo.PropertyType.GetGenericArguments()[0] == typeof (string) && MyAnalyticsEvent.m_supportedTypes.Contains(propertyInfo.PropertyType.GetGenericArguments()[1])))
      {
        IDictionary dictionary1 = propertyValue as IDictionary;
        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
        foreach (DictionaryEntry dictionaryEntry in dictionary1)
          dictionary2[propertyInfo.Name + "." + dictionaryEntry.Key] = dictionaryEntry.Value;
        return dictionary2;
      }
      if (propertyInfo.PropertyType.IsArray && MyAnalyticsEvent.m_supportedTypes.Contains(propertyInfo.PropertyType.GetElementType()))
      {
        object obj = propertyValue;
        return new Dictionary<string, object>()
        {
          {
            propertyInfo.Name,
            obj
          }
        };
      }
      if (!propertyInfo.PropertyType.IsGenericType || !typeof (IEnumerable<object>).IsAssignableFrom(propertyInfo.PropertyType) || !MyAnalyticsEvent.m_supportedTypes.Contains(propertyInfo.PropertyType.GetGenericArguments()[0]))
        throw new MyAnalyticsSpecificationException(string.Format("Property '{0}' is of unsupported type '{1}'", (object) propertyInfo.Name, (object) propertyInfo.PropertyType));
      IEnumerable<object> objects = (IEnumerable<object>) propertyValue;
      return new Dictionary<string, object>()
      {
        {
          propertyInfo.Name,
          (object) objects
        }
      };
    }
  }
}
