// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilders.MyObjectBuilderType
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Reflection;

namespace VRage.ObjectBuilders
{
  public struct MyObjectBuilderType
  {
    private const string LEGACY_TYPE_PREFIX = "MyObjectBuilder_";
    public static readonly MyObjectBuilderType Invalid = new MyObjectBuilderType((Type) null);
    private readonly Type m_type;
    public static readonly MyObjectBuilderType.ComparerType Comparer = new MyObjectBuilderType.ComparerType();
    private static Dictionary<string, MyObjectBuilderType> m_typeByName = new Dictionary<string, MyObjectBuilderType>(500);
    private static Dictionary<string, MyObjectBuilderType> m_typeByLegacyName = new Dictionary<string, MyObjectBuilderType>(500);
    private static Dictionary<MyRuntimeObjectBuilderId, MyObjectBuilderType> m_typeById = new Dictionary<MyRuntimeObjectBuilderId, MyObjectBuilderType>(500, (IEqualityComparer<MyRuntimeObjectBuilderId>) MyRuntimeObjectBuilderId.Comparer);
    private static Dictionary<MyObjectBuilderType, MyRuntimeObjectBuilderId> m_idByType = new Dictionary<MyObjectBuilderType, MyRuntimeObjectBuilderId>(500, (IEqualityComparer<MyObjectBuilderType>) MyObjectBuilderType.Comparer);
    private static ushort m_idCounter;
    private const int EXPECTED_TYPE_COUNT = 500;

    public MyObjectBuilderType(Type type) => this.m_type = type;

    public bool IsNull => this.m_type == (Type) null;

    public static implicit operator MyObjectBuilderType(Type t) => new MyObjectBuilderType(t);

    public static implicit operator Type(MyObjectBuilderType t) => t.m_type;

    public static explicit operator MyRuntimeObjectBuilderId(
      MyObjectBuilderType t)
    {
      MyRuntimeObjectBuilderId runtimeObjectBuilderId;
      if (!MyObjectBuilderType.m_idByType.TryGetValue(t, out runtimeObjectBuilderId))
        runtimeObjectBuilderId = new MyRuntimeObjectBuilderId();
      return runtimeObjectBuilderId;
    }

    public static explicit operator MyObjectBuilderType(
      MyRuntimeObjectBuilderId id)
    {
      return MyObjectBuilderType.m_typeById[id];
    }

    public static bool operator ==(MyObjectBuilderType lhs, MyObjectBuilderType rhs) => lhs.m_type == rhs.m_type;

    public static bool operator !=(MyObjectBuilderType lhs, MyObjectBuilderType rhs) => lhs.m_type != rhs.m_type;

    public static bool operator ==(Type lhs, MyObjectBuilderType rhs) => lhs == rhs.m_type;

    public static bool operator !=(Type lhs, MyObjectBuilderType rhs) => lhs != rhs.m_type;

    public static bool operator ==(MyObjectBuilderType lhs, Type rhs) => lhs.m_type == rhs;

    public static bool operator !=(MyObjectBuilderType lhs, Type rhs) => lhs.m_type != rhs;

    public override bool Equals(object obj) => obj != null && obj is MyObjectBuilderType type && this.Equals(type);

    public bool Equals(MyObjectBuilderType type) => type.m_type == this.m_type;

    public override int GetHashCode() => !(this.m_type != (Type) null) ? 0 : this.m_type.GetHashCode();

    public override string ToString() => !(this.m_type != (Type) null) ? (string) null : this.m_type.Name;

    public static MyObjectBuilderType Parse(string value) => MyObjectBuilderType.m_typeByName[value];

    public static MyObjectBuilderType ParseBackwardsCompatible(string value)
    {
      MyObjectBuilderType objectBuilderType;
      return MyObjectBuilderType.m_typeByName.TryGetValue(value, out objectBuilderType) || MyObjectBuilderType.m_typeByLegacyName.TryGetValue(value, out objectBuilderType) ? objectBuilderType : MyObjectBuilderType.Invalid;
    }

    public static bool IsValidTypeName(string value)
    {
      if (value == null)
        return false;
      return MyObjectBuilderType.m_typeByName.ContainsKey(value) || MyObjectBuilderType.m_typeByLegacyName.ContainsKey(value);
    }

    public static bool TryParse(string value, out MyObjectBuilderType result)
    {
      if (value != null)
        return MyObjectBuilderType.m_typeByName.TryGetValue(value, out result);
      result = MyObjectBuilderType.Invalid;
      return false;
    }

    public static bool IsReady() => MyObjectBuilderType.m_typeByName.Count > 0;

    public static void RegisterFromAssembly(Assembly assembly, bool registerLegacyNames = false)
    {
      if (assembly == (Assembly) null)
        return;
      Type type1 = typeof (MyObjectBuilder_Base);
      Type[] types = assembly.GetTypes();
      Array.Sort<Type>(types, (IComparer<Type>) FullyQualifiedNameComparer.Default);
      foreach (Type type2 in types)
      {
        if (type1.IsAssignableFrom(type2) && !MyObjectBuilderType.m_typeByName.ContainsKey(type2.Name))
        {
          MyObjectBuilderType objectBuilderType = new MyObjectBuilderType(type2);
          MyRuntimeObjectBuilderId key = new MyRuntimeObjectBuilderId(++MyObjectBuilderType.m_idCounter);
          MyObjectBuilderType.m_typeById.Add(key, objectBuilderType);
          MyObjectBuilderType.m_idByType.Add(objectBuilderType, key);
          MyObjectBuilderType.m_typeByName.Add(type2.Name, objectBuilderType);
          if (registerLegacyNames && type2.Name.StartsWith("MyObjectBuilder_"))
            MyObjectBuilderType.RegisterLegacyName(objectBuilderType, type2.Name.Substring("MyObjectBuilder_".Length));
          object[] customAttributes = type2.GetCustomAttributes(typeof (MyObjectBuilderDefinitionAttribute), true);
          if (customAttributes.Length != 0)
          {
            MyObjectBuilderDefinitionAttribute definitionAttribute = (MyObjectBuilderDefinitionAttribute) customAttributes[0];
            if (!string.IsNullOrEmpty(definitionAttribute.LegacyName))
              MyObjectBuilderType.RegisterLegacyName(objectBuilderType, definitionAttribute.LegacyName);
          }
        }
      }
    }

    internal static void RegisterLegacyName(MyObjectBuilderType type, string legacyName) => MyObjectBuilderType.m_typeByLegacyName.Add(legacyName, type);

    internal static void RemapType(
      ref SerializableDefinitionId id,
      Dictionary<string, string> typeOverrideMap)
    {
      string str;
      bool flag = typeOverrideMap.TryGetValue(id.TypeIdString, out str);
      if (!flag && id.TypeIdString.StartsWith("MyObjectBuilder_"))
        flag = typeOverrideMap.TryGetValue(id.TypeIdString.Substring("MyObjectBuilder_".Length), out str);
      if (!flag)
        return;
      id.TypeIdString = str;
    }

    public static void UnregisterAssemblies()
    {
      if (MyObjectBuilderType.m_typeByLegacyName != null)
        MyObjectBuilderType.m_typeByLegacyName.Clear();
      if (MyObjectBuilderType.m_typeById != null)
        MyObjectBuilderType.m_typeById.Clear();
      if (MyObjectBuilderType.m_idByType != null)
        MyObjectBuilderType.m_idByType.Clear();
      if (MyObjectBuilderType.m_typeByName != null)
        MyObjectBuilderType.m_typeByName.Clear();
      MyObjectBuilderType.m_idCounter = (ushort) 0;
    }

    public class ComparerType : IEqualityComparer<MyObjectBuilderType>
    {
      public bool Equals(MyObjectBuilderType x, MyObjectBuilderType y) => x == y;

      public int GetHashCode(MyObjectBuilderType obj) => obj.GetHashCode();
    }
  }
}
