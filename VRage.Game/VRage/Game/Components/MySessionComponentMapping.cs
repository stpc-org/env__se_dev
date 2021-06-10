// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MySessionComponentMapping
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.ObjectBuilders;

namespace VRage.Game.Components
{
  public static class MySessionComponentMapping
  {
    private static Dictionary<Type, MyObjectBuilderType> m_objectBuilderTypeByType = new Dictionary<Type, MyObjectBuilderType>();
    private static Dictionary<MyObjectBuilderType, Type> m_typeByObjectBuilderType = new Dictionary<MyObjectBuilderType, Type>();
    private static Dictionary<Type, MyObjectBuilder_SessionComponent> m_sessionObjectBuilderByType = new Dictionary<Type, MyObjectBuilder_SessionComponent>();

    public static bool Map(Type type, MyObjectBuilderType objectBuilderType)
    {
      if (!type.IsSubclassOf(typeof (MySessionComponentBase)) || MySessionComponentMapping.m_objectBuilderTypeByType.ContainsKey(type))
        return false;
      MySessionComponentMapping.m_objectBuilderTypeByType.Add(type, objectBuilderType);
      if (MySessionComponentMapping.m_typeByObjectBuilderType.ContainsKey(objectBuilderType))
        return false;
      MySessionComponentMapping.m_typeByObjectBuilderType.Add(objectBuilderType, type);
      return true;
    }

    public static Type TryGetMappedSessionComponentType(MyObjectBuilderType objectBuilderType)
    {
      Type type = (Type) null;
      MySessionComponentMapping.m_typeByObjectBuilderType.TryGetValue(objectBuilderType, out type);
      return type;
    }

    public static MyObjectBuilderType TryGetMappedObjectBuilderType(Type type)
    {
      MyObjectBuilderType objectBuilderType = (MyObjectBuilderType) (Type) null;
      MySessionComponentMapping.m_objectBuilderTypeByType.TryGetValue(type, out objectBuilderType);
      return objectBuilderType;
    }

    public static void Clear()
    {
      MySessionComponentMapping.m_objectBuilderTypeByType.Clear();
      MySessionComponentMapping.m_typeByObjectBuilderType.Clear();
      MySessionComponentMapping.m_sessionObjectBuilderByType.Clear();
    }

    public static Dictionary<Type, MyObjectBuilder_SessionComponent> GetMappedSessionObjectBuilders(
      List<MyObjectBuilder_SessionComponent> objectBuilders)
    {
      MySessionComponentMapping.m_sessionObjectBuilderByType.Clear();
      foreach (MyObjectBuilder_SessionComponent objectBuilder in objectBuilders)
      {
        if (MySessionComponentMapping.m_typeByObjectBuilderType.ContainsKey((MyObjectBuilderType) objectBuilder.GetType()))
        {
          Type key = MySessionComponentMapping.m_typeByObjectBuilderType[(MyObjectBuilderType) objectBuilder.GetType()];
          MySessionComponentMapping.m_sessionObjectBuilderByType[key] = objectBuilder;
        }
      }
      return MySessionComponentMapping.m_sessionObjectBuilderByType;
    }
  }
}
