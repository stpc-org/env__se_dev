// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyEntityFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Entities
{
  internal static class MyEntityFactory
  {
    private static MyObjectFactory<MyEntityTypeAttribute, MyEntity> m_objectFactory = new MyObjectFactory<MyEntityTypeAttribute, MyEntity>();
    private static readonly HashSet<Type> m_emptySet = new HashSet<Type>();

    public static void RegisterDescriptor(MyEntityTypeAttribute descriptor, Type type)
    {
      if (!(type != (Type) null) || descriptor == null)
        return;
      MyEntityFactory.m_objectFactory.RegisterDescriptor(descriptor, type);
    }

    public static void RegisterDescriptorsFromAssembly(Assembly[] assemblies)
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        MyEntityFactory.RegisterDescriptorsFromAssembly(assembly);
    }

    public static void RegisterDescriptorsFromAssembly(Assembly assembly)
    {
      if (!(assembly != (Assembly) null))
        return;
      MyEntityFactory.m_objectFactory.RegisterFromAssembly(assembly);
    }

    public static MyEntity CreateEntity(MyObjectBuilder_Base builder) => MyEntityFactory.CreateEntity(builder.TypeId, builder.SubtypeName);

    public static MyEntity CreateEntity(MyObjectBuilderType typeId, string subTypeName = null)
    {
      MyEntity instance = MyEntityFactory.m_objectFactory.CreateInstance(typeId);
      MyEntityFactory.AddScriptGameLogic(instance, typeId, subTypeName);
      MyEntities.RaiseEntityCreated(instance);
      return instance;
    }

    public static T CreateEntity<T>(MyObjectBuilder_Base builder) where T : MyEntity
    {
      T instance = MyEntityFactory.m_objectFactory.CreateInstance<T>(builder.TypeId);
      MyEntityFactory.AddScriptGameLogic((MyEntity) instance, (MyObjectBuilderType) builder.GetType(), builder.SubtypeName);
      MyEntities.RaiseEntityCreated((MyEntity) instance);
      return instance;
    }

    public static void AddScriptGameLogic(
      MyEntity entity,
      MyObjectBuilderType builderType,
      string subTypeName = null)
    {
      MyScriptManager myScriptManager = MyScriptManager.Static;
      if (myScriptManager == null || entity == null)
        return;
      HashSet<Type> typeSet;
      if (subTypeName != null)
      {
        Tuple<Type, string> key = new Tuple<Type, string>((Type) builderType, subTypeName);
        typeSet = myScriptManager.SubEntityScripts.GetValueOrDefault<Tuple<Type, string>, HashSet<Type>>(key, MyEntityFactory.m_emptySet);
      }
      else
        typeSet = MyEntityFactory.m_emptySet;
      HashSet<Type> valueOrDefault = myScriptManager.EntityScripts.GetValueOrDefault<Type, HashSet<Type>>((Type) builderType, MyEntityFactory.m_emptySet);
      int capacity = typeSet.Count + valueOrDefault.Count;
      if (capacity == 0)
        return;
      List<MyGameLogicComponent> gameLogicComponentList = new List<MyGameLogicComponent>(capacity);
      foreach (Type type in valueOrDefault.Concat<Type>((IEnumerable<Type>) typeSet))
      {
        MyGameLogicComponent instance = (MyGameLogicComponent) Activator.CreateInstance(type);
        MyEntityComponentDescriptor customAttribute = (MyEntityComponentDescriptor) type.GetCustomAttribute(typeof (MyEntityComponentDescriptor), false);
        if (!customAttribute.EntityUpdate.HasValue)
          instance.EntityUpdate = true;
        else if (customAttribute.EntityUpdate.Value)
          instance.EntityUpdate = true;
        gameLogicComponentList.Add(instance);
      }
      MyGameLogicComponent gameLogicComponent = MyCompositeGameLogicComponent.Create((ICollection<MyGameLogicComponent>) gameLogicComponentList, entity);
      entity.GameLogic = gameLogicComponent;
    }

    public static MyObjectBuilder_EntityBase CreateObjectBuilder(
      MyEntity entity)
    {
      return MyEntityFactory.m_objectFactory.CreateObjectBuilder<MyObjectBuilder_EntityBase>(entity);
    }
  }
}
