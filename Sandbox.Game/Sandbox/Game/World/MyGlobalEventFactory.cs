// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyGlobalEventFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.World
{
  public class MyGlobalEventFactory
  {
    private static readonly Dictionary<MyDefinitionId, MethodInfo> m_typesToHandlers = new Dictionary<MyDefinitionId, MethodInfo>();
    private static MyObjectFactory<MyEventTypeAttribute, MyGlobalEventBase> m_globalEventFactory = new MyObjectFactory<MyEventTypeAttribute, MyGlobalEventBase>();

    static MyGlobalEventFactory()
    {
      MyGlobalEventFactory.RegisterEventTypesAndHandlers(Assembly.GetAssembly(typeof (MyGlobalEventBase)));
      MyGlobalEventFactory.RegisterEventTypesAndHandlers(MyPlugins.GameAssembly);
      MyGlobalEventFactory.RegisterEventTypesAndHandlers(MyPlugins.SandboxAssembly);
    }

    private static void RegisterEventTypesAndHandlers(Assembly assembly)
    {
      if (assembly == (Assembly) null)
        return;
      foreach (Type type in assembly.GetTypes())
      {
        foreach (MethodInfo method in type.GetMethods())
        {
          if (method.IsPublic && method.IsStatic)
          {
            object[] customAttributes = method.GetCustomAttributes(typeof (MyGlobalEventHandler), false);
            if (customAttributes != null && customAttributes.Length != 0)
            {
              foreach (MyGlobalEventHandler globalEventHandler in customAttributes)
                MyGlobalEventFactory.RegisterHandler(globalEventHandler.EventDefinitionId, method);
            }
          }
        }
      }
      MyGlobalEventFactory.m_globalEventFactory.RegisterFromAssembly(assembly);
    }

    private static void RegisterHandler(MyDefinitionId eventDefinitionId, MethodInfo handler) => MyGlobalEventFactory.m_typesToHandlers[eventDefinitionId] = handler;

    public static MethodInfo GetEventHandler(MyDefinitionId eventDefinitionId)
    {
      MethodInfo methodInfo = (MethodInfo) null;
      MyGlobalEventFactory.m_typesToHandlers.TryGetValue(eventDefinitionId, out methodInfo);
      return methodInfo;
    }

    public static EventDataType CreateEvent<EventDataType>(MyDefinitionId id) where EventDataType : MyGlobalEventBase, new()
    {
      MyGlobalEventDefinition eventDefinition = MyDefinitionManager.Static.GetEventDefinition(id);
      if (eventDefinition == null)
        return default (EventDataType);
      EventDataType eventDataType = new EventDataType();
      eventDataType.InitFromDefinition(eventDefinition);
      return eventDataType;
    }

    public static MyGlobalEventBase CreateEvent(MyDefinitionId id)
    {
      MyGlobalEventDefinition eventDefinition = MyDefinitionManager.Static.GetEventDefinition(id);
      if (eventDefinition == null)
        return (MyGlobalEventBase) null;
      MyGlobalEventBase instance = MyGlobalEventFactory.m_globalEventFactory.CreateInstance(id.TypeId);
      if (instance == null)
        return instance;
      instance.InitFromDefinition(eventDefinition);
      return instance;
    }

    public static MyGlobalEventBase CreateEvent(MyObjectBuilder_GlobalEventBase ob)
    {
      if (ob.DefinitionId.HasValue)
      {
        if (ob.DefinitionId.Value.TypeId == MyObjectBuilderType.Invalid)
          return MyGlobalEventFactory.CreateEventObsolete(ob);
        ob.SubtypeName = ob.DefinitionId.Value.SubtypeName;
      }
      if (MyDefinitionManager.Static.GetEventDefinition(ob.GetId()) == null)
        return (MyGlobalEventBase) null;
      MyGlobalEventBase myGlobalEventBase = MyGlobalEventFactory.CreateEvent(ob.GetId());
      myGlobalEventBase.Init(ob);
      return myGlobalEventBase;
    }

    private static MyGlobalEventBase CreateEventObsolete(
      MyObjectBuilder_GlobalEventBase ob)
    {
      MyGlobalEventBase myGlobalEventBase = MyGlobalEventFactory.CreateEvent(MyGlobalEventFactory.GetEventDefinitionObsolete(ob.EventType));
      myGlobalEventBase.SetActivationTime(TimeSpan.FromMilliseconds((double) ob.ActivationTimeMs));
      myGlobalEventBase.Enabled = ob.Enabled;
      return myGlobalEventBase;
    }

    private static MyDefinitionId GetEventDefinitionObsolete(
      MyGlobalEventTypeEnum eventType)
    {
      switch (eventType)
      {
        case MyGlobalEventTypeEnum.SpawnNeutralShip:
        case MyGlobalEventTypeEnum.SpawnCargoShip:
          return new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "SpawnCargoShip");
        case MyGlobalEventTypeEnum.MeteorWave:
          return new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "MeteorWave");
        case MyGlobalEventTypeEnum.April2014:
          return new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase), "April2014");
        default:
          return new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GlobalEventBase));
      }
    }
  }
}
