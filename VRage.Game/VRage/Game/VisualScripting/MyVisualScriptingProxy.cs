// Decompiled with JetBrains decompiler
// Type: VRage.Game.VisualScripting.MyVisualScriptingProxy
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Game.Entity;
using VRageMath;

namespace VRage.Game.VisualScripting
{
  public static class MyVisualScriptingProxy
  {
    private static readonly Dictionary<string, MethodInfo> m_visualScriptingMethodsBySignature = new Dictionary<string, MethodInfo>();
    private static readonly Dictionary<Type, HashSet<MethodInfo>> m_whitelistedMethods = new Dictionary<Type, HashSet<MethodInfo>>();
    private static readonly Dictionary<MethodInfo, bool> m_whitelistedMethodsSequenceDependency = new Dictionary<MethodInfo, bool>();
    private static readonly Dictionary<string, FieldInfo> m_visualScriptingEventFields = new Dictionary<string, FieldInfo>();
    private static readonly Dictionary<string, Type> m_registeredTypes = new Dictionary<string, Type>();
    private static readonly List<Type> m_supportedTypes = new List<Type>();
    private static bool m_initialized = false;

    public static IEnumerable<FieldInfo> EventFields => (IEnumerable<FieldInfo>) MyVisualScriptingProxy.m_visualScriptingEventFields.Values;

    public static List<Type> SupportedTypes => MyVisualScriptingProxy.m_supportedTypes;

    public static void Init()
    {
      if (MyVisualScriptingProxy.m_initialized)
        return;
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (int));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (float));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (double));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (string));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (Vector3D));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (bool));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (long));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (ulong));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (List<bool>));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (List<int>));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (List<float>));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (List<double>));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (List<string>));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (List<long>));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (List<ulong>));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (List<Vector3D>));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (List<MyEntity>));
      MyVisualScriptingProxy.m_supportedTypes.Add(typeof (MyEntity));
      MyVisualScriptLogicProvider.Init();
      MyVisualScriptingProxy.m_initialized = true;
    }

    private static void RegisterMethod(
      Type declaringType,
      MethodInfo method,
      VisualScriptingMember attribute,
      bool? overrideSequenceDependency = null)
    {
      if (declaringType.IsGenericType)
        declaringType = declaringType.GetGenericTypeDefinition();
      if (!MyVisualScriptingProxy.m_whitelistedMethods.ContainsKey(declaringType))
        MyVisualScriptingProxy.m_whitelistedMethods[declaringType] = new HashSet<MethodInfo>();
      MyVisualScriptingProxy.m_whitelistedMethods[declaringType].Add(method);
      Dictionary<MethodInfo, bool> sequenceDependency = MyVisualScriptingProxy.m_whitelistedMethodsSequenceDependency;
      MethodInfo key = method;
      bool? nullable = overrideSequenceDependency;
      int num = nullable.HasValue ? (nullable.GetValueOrDefault() ? 1 : 0) : (attribute.Sequential ? 1 : 0);
      sequenceDependency[key] = num != 0;
      foreach (KeyValuePair<Type, HashSet<MethodInfo>> whitelistedMethod1 in MyVisualScriptingProxy.m_whitelistedMethods)
      {
        if (whitelistedMethod1.Key.IsAssignableFrom(declaringType))
          whitelistedMethod1.Value.Add(method);
        else if (declaringType.IsAssignableFrom(whitelistedMethod1.Key))
        {
          HashSet<MethodInfo> whitelistedMethod2 = MyVisualScriptingProxy.m_whitelistedMethods[declaringType];
          foreach (MethodInfo methodInfo in whitelistedMethod1.Value)
            whitelistedMethod2.Add(methodInfo);
        }
      }
    }

    public static void RegisterType(Type type)
    {
      string key = type.Signature();
      if (MyVisualScriptingProxy.m_registeredTypes.ContainsKey(key))
        return;
      MyVisualScriptingProxy.m_registeredTypes.Add(key, type);
    }

    public static void WhitelistExtensions(Type type)
    {
      foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
      {
        VisualScriptingMember customAttribute = method.GetCustomAttribute<VisualScriptingMember>();
        if (customAttribute != null && method.IsDefined(typeof (ExtensionAttribute), false))
          MyVisualScriptingProxy.RegisterMethod(method.GetParameters()[0].ParameterType, method, customAttribute);
      }
      MyVisualScriptingProxy.m_registeredTypes[type.Signature()] = type;
    }

    public static void WhitelistMethod(MethodInfo method, bool sequenceDependent)
    {
      Type declaringType = method.DeclaringType;
      if (declaringType == (Type) null)
        return;
      MyVisualScriptingProxy.RegisterMethod(declaringType, method, (VisualScriptingMember) null, new bool?(sequenceDependent));
    }

    public static IEnumerable<MethodInfo> GetWhitelistedMethods(Type type)
    {
      if (type == (Type) null)
      {
        HashSet<MethodInfo> methodInfoSet1 = new HashSet<MethodInfo>();
        foreach (HashSet<MethodInfo> methodInfoSet2 in MyVisualScriptingProxy.m_whitelistedMethods.Values)
        {
          foreach (MethodInfo methodInfo in methodInfoSet2)
            methodInfoSet1.Add(methodInfo);
        }
        return (IEnumerable<MethodInfo>) methodInfoSet1;
      }
      HashSet<MethodInfo> methodInfoSet3;
      if (MyVisualScriptingProxy.m_whitelistedMethods.TryGetValue(type, out methodInfoSet3))
        return (IEnumerable<MethodInfo>) methodInfoSet3;
      if (type.IsGenericType)
      {
        Type genericTypeDefinition = type.GetGenericTypeDefinition();
        Type[] genericArguments = type.GetGenericArguments();
        if (MyVisualScriptingProxy.m_whitelistedMethods.TryGetValue(genericTypeDefinition, out methodInfoSet3))
        {
          HashSet<MethodInfo> methodInfoSet1 = new HashSet<MethodInfo>();
          MyVisualScriptingProxy.m_whitelistedMethods[type] = methodInfoSet1;
          foreach (MethodInfo methodInfo1 in methodInfoSet3)
          {
            MethodInfo methodInfo2 = !methodInfo1.IsDefined(typeof (ExtensionAttribute)) ? type.GetMethod(methodInfo1.Name) : methodInfo1.MakeGenericMethod(genericArguments);
            methodInfoSet1.Add(methodInfo2);
            bool flag = MyVisualScriptingProxy.m_whitelistedMethodsSequenceDependency[methodInfo1];
            MyVisualScriptingProxy.m_whitelistedMethodsSequenceDependency[methodInfo2] = flag;
            MyVisualScriptingProxy.m_visualScriptingMethodsBySignature[methodInfo2.Signature()] = methodInfo2;
          }
          return (IEnumerable<MethodInfo>) methodInfoSet1;
        }
      }
      return (IEnumerable<MethodInfo>) null;
    }

    public static void RegisterLogicProvider(Type type)
    {
      foreach (MethodInfo method in type.GetMethods())
      {
        if (method.GetCustomAttribute<VisualScriptingMember>() != null)
        {
          string key = method.Signature();
          if (!MyVisualScriptingProxy.m_visualScriptingMethodsBySignature.ContainsKey(key))
            MyVisualScriptingProxy.m_visualScriptingMethodsBySignature.Add(key, method);
        }
      }
      foreach (FieldInfo field in type.GetFields())
      {
        if (field.FieldType.GetCustomAttribute<VisualScriptingEvent>() != null && field.FieldType.IsSubclassOf(typeof (MulticastDelegate)) && !MyVisualScriptingProxy.m_visualScriptingEventFields.ContainsKey(field.Signature()))
          MyVisualScriptingProxy.m_visualScriptingEventFields.Add(field.Signature(), field);
      }
    }

    public static Type GetType(string typeFullName)
    {
      switch (typeFullName)
      {
        case "":
        case null:
          throw new Exception("Null type signature!");
        default:
          Type type1;
          if (MyVisualScriptingProxy.m_registeredTypes.TryGetValue(typeFullName, out type1))
            return type1;
          Type type2 = Type.GetType(typeFullName);
          return type2 != (Type) null ? type2 : typeof (Vector3D).Assembly.GetType(typeFullName);
      }
    }

    public static MethodInfo GetMethod(string signature)
    {
      MethodInfo methodInfo1 = MyVisualScriptingProxy.GetWhitelistedMethods((Type) null).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (x => x.Signature() == signature));
      if (methodInfo1 != (MethodInfo) null)
        return methodInfo1;
      MethodInfo methodInfo2;
      MyVisualScriptingProxy.m_visualScriptingMethodsBySignature.TryGetValue(signature, out methodInfo2);
      return methodInfo2;
    }

    public static MethodInfo GetMethodCaseInvariant(string signature)
    {
      string signatureLowercase = signature.ToLower();
      MethodInfo methodInfo1 = MyVisualScriptingProxy.GetWhitelistedMethods((Type) null).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (x => x.Signature().ToLower() == signatureLowercase));
      if (methodInfo1 != (MethodInfo) null)
        return methodInfo1;
      MethodInfo methodInfo2 = (MethodInfo) null;
      foreach (KeyValuePair<string, MethodInfo> keyValuePair in MyVisualScriptingProxy.m_visualScriptingMethodsBySignature)
      {
        if (keyValuePair.Key.ToLower() == signatureLowercase)
          return keyValuePair.Value;
      }
      return methodInfo2;
    }

    public static MethodInfo GetMethod(Type type, string signature)
    {
      if (!MyVisualScriptingProxy.m_whitelistedMethods.ContainsKey(type))
        MyVisualScriptingProxy.GetWhitelistedMethods(type);
      return MyVisualScriptingProxy.GetMethod(signature);
    }

    public static List<MethodInfo> GetMethods()
    {
      List<MethodInfo> methodInfoList = new List<MethodInfo>();
      foreach (KeyValuePair<string, MethodInfo> keyValuePair in MyVisualScriptingProxy.m_visualScriptingMethodsBySignature)
        methodInfoList.Add(keyValuePair.Value);
      return methodInfoList;
    }

    public static FieldInfo GetField(string signature)
    {
      FieldInfo fieldInfo;
      MyVisualScriptingProxy.m_visualScriptingEventFields.TryGetValue(signature, out fieldInfo);
      return fieldInfo;
    }

    public static string Signature(this FieldInfo info) => info.DeclaringType.Namespace + "." + info.DeclaringType.Name + "." + info.Name;

    public static bool TryToRecoverMethodInfo(
      ref string oldSignature,
      Type declaringType,
      Type extensionType,
      out MethodInfo info)
    {
      info = (MethodInfo) null;
      int index = 0;
      while (index < oldSignature.Length && index < declaringType.FullName.Length && (int) oldSignature[index] == (int) declaringType.FullName[index])
        ++index;
      oldSignature = oldSignature.Remove(0, index + 1);
      oldSignature = oldSignature.Remove(oldSignature.IndexOf('('));
      if (extensionType != (Type) null && extensionType.IsGenericType)
      {
        Type[] genericArguments = extensionType.GetGenericArguments();
        MethodInfo method = declaringType.GetMethod(oldSignature);
        if (method != (MethodInfo) null)
          info = method.MakeGenericMethod(genericArguments);
      }
      else
        info = declaringType.GetMethod(oldSignature);
      if (info != (MethodInfo) null)
        oldSignature = info.Signature();
      return info != (MethodInfo) null;
    }

    public static bool TryToRecoverMethodInfo(ref string oldSignature, out MethodInfo info)
    {
      info = (MethodInfo) null;
      List<string> stringList = new List<string>();
      string str = oldSignature.Replace(")", "");
      foreach (KeyValuePair<string, MethodInfo> keyValuePair in MyVisualScriptingProxy.m_visualScriptingMethodsBySignature)
      {
        if (keyValuePair.Value.Signature().StartsWith(str))
        {
          info = keyValuePair.Value;
          break;
        }
      }
      return info != (MethodInfo) null;
    }

    public static string Signature(this MethodInfo info)
    {
      StringBuilder stringBuilder = new StringBuilder(info.DeclaringType.Signature());
      ParameterInfo[] parameters = info.GetParameters();
      stringBuilder.Append('.').Append(info.Name).Append('(');
      for (int index = 0; index < parameters.Length; ++index)
      {
        if (parameters[index].ParameterType.IsGenericType)
          stringBuilder.Append(parameters[index].ParameterType.Signature());
        else
          stringBuilder.Append(parameters[index].ParameterType.Name);
        stringBuilder.Append(' ').Append(parameters[index].Name);
        if (index < parameters.Length - 1)
          stringBuilder.Append(", ");
      }
      stringBuilder.Append(')');
      return stringBuilder.ToString();
    }

    public static string MethodGroup(this MethodInfo info) => info.GetCustomAttribute<VisualScriptingMiscData>()?.Group;

    public static string Signature(this Type type)
    {
      if (type.IsEnum)
        return type.FullName.Replace("+", ".");
      return type.IsByRef ? "ref " + type.FullName.Replace("&", "") : type.FullName;
    }

    public static bool IsSequenceDependent(this MethodInfo method)
    {
      VisualScriptingMember customAttribute = method.GetCustomAttribute<VisualScriptingMember>();
      if (customAttribute == null && !method.IsStatic)
      {
        bool flag = true;
        return !MyVisualScriptingProxy.m_whitelistedMethodsSequenceDependency.TryGetValue(method, out flag) || flag;
      }
      return customAttribute == null || customAttribute.Sequential;
    }

    public static string ReadableName(this Type type)
    {
      if (type == (Type) null)
      {
        Debugger.Break();
        return (string) null;
      }
      if (type == typeof (bool))
        return "Bool";
      if (type == typeof (int))
        return "Int";
      if (type == typeof (string))
        return "String";
      if (type == typeof (float))
        return "Float";
      if (type == typeof (long))
        return "Long";
      if (type == typeof (ulong))
        return "ULong";
      if (!type.IsGenericType)
        return type.Name;
      StringBuilder stringBuilder = new StringBuilder(type.Name.Remove(type.Name.IndexOf('`')));
      Type[] genericArguments = type.GetGenericArguments();
      stringBuilder.Append(" - ");
      foreach (Type type1 in genericArguments)
      {
        stringBuilder.Append(type1.ReadableName());
        stringBuilder.Append(",");
      }
      stringBuilder.Remove(stringBuilder.Length - 1, 1);
      return stringBuilder.ToString();
    }
  }
}
