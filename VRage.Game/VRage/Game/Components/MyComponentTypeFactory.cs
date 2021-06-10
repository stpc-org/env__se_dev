// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyComponentTypeFactory
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Plugins;
using VRage.Utils;

namespace VRage.Game.Components
{
  [PreloadRequired]
  public static class MyComponentTypeFactory
  {
    private static Dictionary<MyStringId, Type> m_idToType = new Dictionary<MyStringId, Type>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
    private static Dictionary<Type, MyStringId> m_typeToId = new Dictionary<Type, MyStringId>();
    private static Dictionary<Type, Type> m_typeToContainerComponentType = new Dictionary<Type, Type>();

    static MyComponentTypeFactory()
    {
      MyComponentTypeFactory.RegisterFromAssembly(typeof (MyComponentTypeFactory).Assembly);
      MyComponentTypeFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
      MyComponentTypeFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
      MyComponentTypeFactory.RegisterFromAssembly(MyPlugins.SandboxGameAssembly);
      MyComponentTypeFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
    }

    private static void RegisterFromAssembly(Assembly[] assemblies)
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        MyComponentTypeFactory.RegisterFromAssembly(assembly);
    }

    private static void RegisterFromAssembly(Assembly assembly)
    {
      if (assembly == (Assembly) null)
        return;
      Type type1 = typeof (MyComponentBase);
      foreach (Type type2 in assembly.GetTypes())
      {
        if (type1.IsAssignableFrom(type2))
        {
          MyComponentTypeFactory.AddId(type2, MyStringId.GetOrCompute(type2.Name));
          MyComponentTypeFactory.RegisterComponentTypeAttribute(type2);
        }
      }
    }

    private static void RegisterComponentTypeAttribute(Type type)
    {
      object[] customAttributes = type.GetCustomAttributes(typeof (MyComponentTypeAttribute), true);
      Type type1 = (Type) null;
      foreach (MyComponentTypeAttribute componentTypeAttribute in customAttributes)
      {
        if (componentTypeAttribute.ComponentType != (Type) null && type1 == (Type) null)
        {
          type1 = componentTypeAttribute.ComponentType;
          break;
        }
      }
      if (!(type1 != (Type) null))
        return;
      MyComponentTypeFactory.m_typeToContainerComponentType.Add(type, type1);
    }

    private static void AddId(Type type, MyStringId id)
    {
      MyComponentTypeFactory.m_idToType[id] = type;
      MyComponentTypeFactory.m_typeToId[type] = id;
    }

    public static MyStringId GetId(Type type) => MyComponentTypeFactory.m_typeToId[type];

    public static Type GetType(MyStringId id) => MyComponentTypeFactory.m_idToType[id];

    public static Type GetType(string typeId)
    {
      MyStringId id;
      if (MyStringId.TryGet(typeId, out id))
        return MyComponentTypeFactory.m_idToType[id];
      throw new Exception("Unregistered component typeId! : " + typeId);
    }

    public static Type GetComponentType(Type type)
    {
      Type type1;
      return MyComponentTypeFactory.m_typeToContainerComponentType.TryGetValue(type, out type1) ? type1 : (Type) null;
    }
  }
}
