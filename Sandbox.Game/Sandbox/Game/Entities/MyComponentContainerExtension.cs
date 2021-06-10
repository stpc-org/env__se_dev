// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyComponentContainerExtension
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
  public static class MyComponentContainerExtension
  {
    public static void InitComponents(
      this MyComponentContainer container,
      MyObjectBuilderType type,
      MyStringHash subtypeName,
      MyObjectBuilder_ComponentContainer builder)
    {
      if (MyDefinitionManager.Static == null)
        return;
      MyContainerDefinition definition = (MyContainerDefinition) null;
      bool flag1 = builder == null;
      if (MyComponentContainerExtension.TryGetContainerDefinition(type, subtypeName, out definition))
      {
        container.Init(definition);
        if (definition.DefaultComponents != null)
        {
          foreach (MyContainerDefinition.DefaultComponent defaultComponent in definition.DefaultComponents)
          {
            MyComponentDefinitionBase componentDefinition = (MyComponentDefinitionBase) null;
            MyObjectBuilder_ComponentBase componentBuilder = MyComponentContainerExtension.FindComponentBuilder(defaultComponent, builder);
            bool flag2 = componentBuilder != null;
            Type type1 = (Type) null;
            MyComponentBase component = (MyComponentBase) null;
            MyStringHash subtypeName1 = subtypeName;
            if (defaultComponent.SubtypeId.HasValue)
              subtypeName1 = defaultComponent.SubtypeId.Value;
            if (MyComponentContainerExtension.TryGetComponentDefinition(defaultComponent.BuilderType, subtypeName1, out componentDefinition))
            {
              component = MyComponentFactory.CreateInstanceByTypeId(componentDefinition.Id.TypeId);
              component.Init(componentDefinition);
            }
            else if (defaultComponent.IsValid())
              component = defaultComponent.BuilderType.IsNull ? MyComponentFactory.CreateInstanceByType(defaultComponent.InstanceType) : MyComponentFactory.CreateInstanceByTypeId(defaultComponent.BuilderType);
            if (component != null)
            {
              Type componentType = MyComponentTypeFactory.GetComponentType(component.GetType());
              if (componentType != (Type) null)
                type1 = componentType;
            }
            if (type1 == (Type) null && component != null)
              type1 = component.GetType();
            if (component != null && type1 != (Type) null && (flag1 | flag2 ? 1 : (defaultComponent.ForceCreate ? 1 : 0)) != 0)
            {
              if (componentBuilder != null)
                component.Deserialize(componentBuilder);
              container.Add(type1, component);
            }
          }
        }
      }
      container.Deserialize(builder);
    }

    public static MyObjectBuilder_ComponentBase FindComponentBuilder(
      MyContainerDefinition.DefaultComponent component,
      MyObjectBuilder_ComponentContainer builder)
    {
      MyObjectBuilder_ComponentBase builderComponentBase = (MyObjectBuilder_ComponentBase) null;
      if (builder != null && component.IsValid())
      {
        MyObjectBuilderType objectBuilderType = (MyObjectBuilderType) (Type) null;
        if (!component.BuilderType.IsNull)
        {
          MyObjectBuilder_ComponentContainer.ComponentData componentData = builder.Components.Find((Predicate<MyObjectBuilder_ComponentContainer.ComponentData>) (x => x.Component.TypeId == component.BuilderType));
          if (componentData != null)
            builderComponentBase = componentData.Component;
        }
      }
      return builderComponentBase;
    }

    public static bool TryGetContainerDefinition(
      MyObjectBuilderType type,
      MyStringHash subtypeName,
      out MyContainerDefinition definition)
    {
      definition = (MyContainerDefinition) null;
      return MyDefinitionManager.Static != null && (MyDefinitionManager.Static.TryGetContainerDefinition(new MyDefinitionId(type, subtypeName), out definition) || subtypeName != MyStringHash.NullOrEmpty && MyDefinitionManager.Static.TryGetContainerDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_EntityBase), subtypeName), out definition) || MyDefinitionManager.Static.TryGetContainerDefinition(new MyDefinitionId(type), out definition));
    }

    public static bool TryGetComponentDefinition(
      MyObjectBuilderType type,
      MyStringHash subtypeName,
      out MyComponentDefinitionBase componentDefinition)
    {
      componentDefinition = (MyComponentDefinitionBase) null;
      return MyDefinitionManager.Static != null && (MyDefinitionManager.Static.TryGetEntityComponentDefinition(new MyDefinitionId(type, subtypeName), out componentDefinition) || subtypeName != MyStringHash.NullOrEmpty && MyDefinitionManager.Static.TryGetEntityComponentDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_EntityBase), subtypeName), out componentDefinition) || MyDefinitionManager.Static.TryGetEntityComponentDefinition(new MyDefinitionId(type), out componentDefinition));
    }

    public static bool TryGetEntityComponentTypes(long entityId, out List<Type> components)
    {
      components = (List<Type>) null;
      MyEntity entity;
      if (MyEntities.TryGetEntityById(entityId, out entity))
      {
        components = new List<Type>();
        foreach (Type componentType in entity.Components.GetComponentTypes())
        {
          if (componentType != (Type) null)
            components.Add(componentType);
        }
        if (components.Count > 0)
          return true;
      }
      return false;
    }

    public static bool TryRemoveComponent(long entityId, Type componentType)
    {
      MyEntity entity;
      if (!MyEntities.TryGetEntityById(entityId, out entity))
        return false;
      entity.Components.Remove(componentType);
      return true;
    }

    public static bool TryAddComponent(long entityId, MyDefinitionId componentDefinitionId)
    {
      MyEntity entity;
      if (!MyEntities.TryGetEntityById(entityId, out entity))
        return false;
      MyComponentDefinitionBase componentDefinition;
      if (MyComponentContainerExtension.TryGetComponentDefinition(componentDefinitionId.TypeId, componentDefinitionId.SubtypeId, out componentDefinition))
      {
        MyComponentBase instanceByTypeId = MyComponentFactory.CreateInstanceByTypeId(componentDefinition.Id.TypeId);
        Type componentType = MyComponentTypeFactory.GetComponentType(instanceByTypeId.GetType());
        if (componentType == (Type) null)
          return false;
        instanceByTypeId.Init(componentDefinition);
        entity.Components.Add(componentType, instanceByTypeId);
      }
      return true;
    }

    public static bool TryAddComponent(
      long entityId,
      string instanceTypeStr,
      string componentTypeStr)
    {
      Type type1 = (Type) null;
      Type type2 = (Type) null;
      try
      {
        type1 = Type.GetType(instanceTypeStr, true);
      }
      catch (Exception ex)
      {
      }
      try
      {
        type1 = Type.GetType(componentTypeStr, true);
      }
      catch (Exception ex)
      {
      }
      MyEntity entity;
      if (!MyEntities.TryGetEntityById(entityId, out entity) || !(type1 != (Type) null))
        return false;
      MyComponentBase instanceByType = MyComponentFactory.CreateInstanceByType(type1);
      MyComponentDefinitionBase componentDefinition;
      if (entity.DefinitionId.HasValue && MyComponentContainerExtension.TryGetComponentDefinition((MyObjectBuilderType) instanceByType.GetType(), entity.DefinitionId.Value.SubtypeId, out componentDefinition))
        instanceByType.Init(componentDefinition);
      entity.Components.Add(type2, instanceByType);
      return true;
    }
  }
}
