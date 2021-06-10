// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyDefinitionManagerBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using VRage.Game.Common;
using VRage.Game.Definitions;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  public abstract class MyDefinitionManagerBase
  {
    protected MyDefinitionSet m_definitions = new MyDefinitionSet();
    private static MyObjectFactory<MyDefinitionTypeAttribute, MyDefinitionBase> m_definitionFactory = new MyObjectFactory<MyDefinitionTypeAttribute, MyDefinitionBase>();
    protected static Dictionary<Type, MyDefinitionPostprocessor> m_postprocessorsByType = new Dictionary<Type, MyDefinitionPostprocessor>();
    protected static List<MyDefinitionPostprocessor> m_postProcessors = new List<MyDefinitionPostprocessor>();
    protected static HashSet<Assembly> m_registeredAssemblies = new HashSet<Assembly>();
    public static MyDefinitionManagerBase Static;
    private static readonly Dictionary<Type, HashSet<Type>> m_childDefinitionMap = new Dictionary<Type, HashSet<Type>>();
    private static ConcurrentDictionary<Type, Type> m_objectBuilderTypeCache = new ConcurrentDictionary<Type, Type>();
    private static HashSet<Assembly> m_registered = new HashSet<Assembly>();

    public static MyObjectFactory<MyDefinitionTypeAttribute, MyDefinitionBase> GetObjectFactory() => MyDefinitionManagerBase.m_definitionFactory;

    public static void RegisterTypesFromAssembly(Assembly assembly)
    {
      if (assembly == (Assembly) null || MyDefinitionManagerBase.m_registeredAssemblies.Contains(assembly))
        return;
      MyDefinitionManagerBase.m_registeredAssemblies.Add(assembly);
      if (MyDefinitionManagerBase.m_registered.Contains(assembly))
        return;
      MyDefinitionManagerBase.m_registered.Add(assembly);
      foreach (Type type in assembly.GetTypes())
      {
        object[] customAttributes = type.GetCustomAttributes(typeof (MyDefinitionTypeAttribute), false);
        if (customAttributes.Length != 0)
        {
          if (!type.IsSubclassOf(typeof (MyDefinitionBase)) && type != typeof (MyDefinitionBase))
          {
            MyLog.Default.Error("Type {0} is not a definition.", (object) type.Name);
          }
          else
          {
            foreach (MyDefinitionTypeAttribute descriptor in customAttributes)
            {
              MyDefinitionManagerBase.m_definitionFactory.RegisterDescriptor(descriptor, type);
              MyDefinitionPostprocessor instance = (MyDefinitionPostprocessor) Activator.CreateInstance(descriptor.PostProcessor);
              instance.DefinitionType = descriptor.ObjectBuilderType;
              MyDefinitionManagerBase.m_postProcessors.Add(instance);
              MyDefinitionManagerBase.m_postprocessorsByType.Add(descriptor.ObjectBuilderType, instance);
              MyXmlSerializerManager.RegisterSerializer(descriptor.ObjectBuilderType);
            }
            Type key = type;
            while (key != typeof (MyDefinitionBase))
            {
              key = key.BaseType;
              HashSet<Type> typeSet;
              if (!MyDefinitionManagerBase.m_childDefinitionMap.TryGetValue(key, out typeSet))
              {
                typeSet = new HashSet<Type>();
                MyDefinitionManagerBase.m_childDefinitionMap[key] = typeSet;
                typeSet.Add(key);
              }
              typeSet.Add(type);
            }
          }
        }
      }
      MyDefinitionManagerBase.m_postProcessors.Sort((IComparer<MyDefinitionPostprocessor>) MyDefinitionPostprocessor.Comparer);
    }

    public static MyDefinitionPostprocessor GetPostProcessor(Type obType)
    {
      MyDefinitionPostprocessor definitionPostprocessor;
      MyDefinitionManagerBase.m_postprocessorsByType.TryGetValue(obType, out definitionPostprocessor);
      return definitionPostprocessor;
    }

    public static Type GetObjectBuilderType(Type defType)
    {
      Type type;
      if (MyDefinitionManagerBase.m_objectBuilderTypeCache.TryGetValue(defType, out type))
        return type;
      object[] customAttributes = defType.GetCustomAttributes(typeof (MyDefinitionTypeAttribute), false);
      int index = 0;
      if (index >= customAttributes.Length)
        return (Type) null;
      Type objectBuilderType = ((MyFactoryTagAttribute) customAttributes[index]).ObjectBuilderType;
      MyDefinitionManagerBase.m_objectBuilderTypeCache.TryAdd(defType, objectBuilderType);
      return objectBuilderType;
    }

    public T GetDefinition<T>(string subtypeId) where T : MyDefinitionBase => this.m_definitions.GetDefinition<T>(MyStringHash.GetOrCompute(subtypeId));

    public T GetDefinition<T>(MyStringHash subtypeId) where T : MyDefinitionBase => this.m_definitions.GetDefinition<T>(subtypeId);

    public T GetDefinition<T>(MyDefinitionId subtypeId) where T : MyDefinitionBase => this.m_definitions.GetDefinition<T>(subtypeId);

    public IEnumerable<T> GetDefinitions<T>() where T : MyDefinitionBase => this.m_definitions.GetDefinitionsOfType<T>();

    public IEnumerable<T> GetAllDefinitions<T>() where T : MyDefinitionBase => this.m_definitions.GetDefinitionsOfTypeAndSubtypes<T>();

    public bool TryGetDefinition<T>(MyStringHash subtypeId, out T def) where T : MyDefinitionBase => (object) (def = this.m_definitions.GetDefinition<T>(subtypeId)) != null;

    public abstract MyDefinitionSet GetLoadingSet();

    public MyDefinitionSet Definitions => this.m_definitions;

    public HashSet<Type> GetSubtypes<T>()
    {
      HashSet<Type> typeSet;
      MyDefinitionManagerBase.m_childDefinitionMap.TryGetValue(typeof (T), out typeSet);
      return typeSet;
    }
  }
}
