// Decompiled with JetBrains decompiler
// Type: VRage.TypeExtensions
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Collections;

namespace VRage
{
  public static class TypeExtensions
  {
    public static readonly HashSetReader<Type> CoreTypes = (HashSetReader<Type>) new HashSet<Type>((IEnumerable<Type>) new Type[22]
    {
      typeof (object),
      typeof (string),
      typeof (int),
      typeof (short),
      typeof (long),
      typeof (uint),
      typeof (ushort),
      typeof (ulong),
      typeof (double),
      typeof (float),
      typeof (bool),
      typeof (char),
      typeof (byte),
      typeof (sbyte),
      typeof (Decimal),
      typeof (Enum),
      typeof (ValueType),
      typeof (Delegate),
      typeof (MulticastDelegate),
      typeof (Type),
      typeof (Attribute),
      typeof (Exception)
    });

    public static bool IsStruct(this Type type) => type.IsValueType && !type.IsPrimitive && !type.IsEnum && type != typeof (Decimal);

    public static bool IsAccessible(this Type type)
    {
      while (!type.IsPublic)
      {
        if ((type.Attributes & TypeAttributes.NestedPublic) == TypeAttributes.NotPublic)
          return false;
        type = type.DeclaringType;
        if (!(type != (Type) null))
          return false;
      }
      return true;
    }

    public static IEnumerable<MemberInfo> GetDataMembers(
      this Type t,
      bool fields,
      bool properties,
      bool nonPublic,
      bool inherited,
      bool _static,
      bool instance,
      bool read,
      bool write)
    {
      BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public;
      if (nonPublic)
        bindingAttr |= BindingFlags.NonPublic;
      if (_static)
        bindingAttr |= BindingFlags.Static;
      if (instance)
        bindingAttr |= BindingFlags.Instance;
      IEnumerable<MemberInfo> first = (IEnumerable<MemberInfo>) t.GetMembers(bindingAttr);
      if (inherited && t.IsClass && t != typeof (object))
      {
        for (Type baseType = t.BaseType; baseType != typeof (object) && baseType != (Type) null; baseType = baseType.BaseType)
          first = first.Concat<MemberInfo>((IEnumerable<MemberInfo>) baseType.GetMembers(bindingAttr));
      }
      SortedDictionary<string, MemberInfo> sortedDictionary = new SortedDictionary<string, MemberInfo>();
      foreach (MemberInfo info in first)
      {
        if ((fields ? (info.MemberType == MemberTypes.Field ? 1 : 0) : 0) != 0 || (properties ? (TypeExtensions.CheckProperty(info, read, write) ? 1 : 0) : 0) != 0)
          sortedDictionary.Add(info.DeclaringType.Name + info.Name, info);
      }
      return (IEnumerable<MemberInfo>) sortedDictionary.Values;
    }

    private static bool CheckProperty(MemberInfo info, bool read, bool write)
    {
      PropertyInfo propertyInfo = info as PropertyInfo;
      if (!(propertyInfo != (PropertyInfo) null) || read && !propertyInfo.CanRead)
        return false;
      return !write || propertyInfo.CanWrite;
    }

    public static Type FindGenericBaseTypeArgument(this Type type, Type genericTypeDefinition)
    {
      Type[] baseTypeArguments = type.FindGenericBaseTypeArguments(genericTypeDefinition);
      return baseTypeArguments.Length == 0 ? (Type) null : baseTypeArguments[0];
    }

    public static Type[] FindGenericBaseTypeArguments(
      this Type type,
      Type genericTypeDefinition)
    {
      if (type.IsValueType || type.IsInterface)
        return Type.EmptyTypes;
      for (; type != typeof (object); type = type.BaseType)
      {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeDefinition)
          return type.GetGenericArguments();
      }
      return Type.EmptyTypes;
    }

    public static bool IsInstanceOfGenericType(this Type subtype, Type genericType)
    {
      for (Type type = subtype; type != (Type) null; type = type.BaseType)
      {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
          return true;
      }
      return false;
    }

    public static bool ImplementsGenericInterface(this Type subtype, Type genericInterface) => ((IEnumerable<Type>) subtype.GetInterfaces()).Any<Type>((Func<Type, bool>) (x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterface));

    public static bool HasDefaultConstructor(this Type type) => !type.IsAbstract && type.GetConstructor(Type.EmptyTypes) != (ConstructorInfo) null;

    public static string PrettyName(this Type type)
    {
      Type underlyingType = Nullable.GetUnderlyingType(type);
      if (underlyingType != (Type) null)
        return underlyingType.PrettyName() + "?";
      if (type.IsByRef)
        return string.Format("{0}&", (object) type.GetElementType().PrettyName());
      if (type.IsArray)
        return string.Format("{0}[{1}]", (object) type.GetElementType().PrettyName(), (object) new string(',', type.GetArrayRank() - 1));
      if (!type.IsGenericType)
      {
        switch (type.Name)
        {
          case "Byte":
            return "byte";
          case "Decimal":
            return "decimal";
          case "Int16":
            return "short";
          case "Int32":
            return "int";
          case "Int64":
            return "long";
          case "Object":
            return "object";
          case "SByte":
            return "sbyte";
          case "String":
            return "string";
          case "UInt16":
            return "ushort";
          case "UInt32":
            return "uint";
          case "UInt64":
            return "ulong";
          case "Void":
            return "void";
          default:
            return !string.IsNullOrWhiteSpace(type.FullName) ? type.FullName : type.Name;
        }
      }
      else
      {
        StringBuilder stringBuilder = new StringBuilder();
        int length = type.Name.IndexOf('`');
        if (length >= 0)
          stringBuilder.Append(type.Name.Substring(0, length));
        else
          stringBuilder.Append(type.Name);
        stringBuilder.Append('<');
        bool flag = true;
        foreach (Type genericArgument in type.GetGenericArguments())
        {
          if (!flag)
            stringBuilder.Append(',');
          stringBuilder.Append(genericArgument.PrettyName());
          flag = false;
        }
        stringBuilder.Append('>');
        return stringBuilder.ToString();
      }
    }

    public static int SizeOf<T>() => Unsafe.SizeOf<T>();
  }
}
