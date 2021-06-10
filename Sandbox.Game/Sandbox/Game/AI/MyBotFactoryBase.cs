// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.MyBotFactoryBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.AI.Actions;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Game;
using VRage.Game.AI;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRageMath;

namespace Sandbox.Game.AI
{
  public abstract class MyBotFactoryBase
  {
    protected Dictionary<string, Type> m_TargetTypeByName;
    protected Dictionary<string, MyBotFactoryBase.BehaviorData> m_botDataByBehaviorType;
    protected Dictionary<string, MyBotFactoryBase.LogicData> m_logicDataByBehaviorSubtype;
    private readonly Type[] m_tmpTypeArray;
    private readonly object[] m_tmpConstructorParamArray;
    private static readonly MyObjectFactory<MyBotTypeAttribute, IMyBot> m_objectFactory = new MyObjectFactory<MyBotTypeAttribute, IMyBot>();

    static MyBotFactoryBase()
    {
      Assembly assembly = Assembly.GetAssembly(typeof (MyAgentBot));
      MyBotFactoryBase.m_objectFactory.RegisterFromAssembly(assembly);
      MyBotFactoryBase.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
      foreach (IPlugin plugin in MyPlugins.Plugins)
        MyBotFactoryBase.m_objectFactory.RegisterFromAssembly(plugin.GetType().Assembly);
    }

    protected MyBotFactoryBase()
    {
      this.m_TargetTypeByName = new Dictionary<string, Type>();
      this.m_botDataByBehaviorType = new Dictionary<string, MyBotFactoryBase.BehaviorData>();
      this.m_logicDataByBehaviorSubtype = new Dictionary<string, MyBotFactoryBase.LogicData>();
      this.m_tmpTypeArray = new Type[1];
      this.m_tmpConstructorParamArray = new object[1];
      this.LoadBotData(Assembly.GetAssembly(typeof (MyAgentBot)));
      this.LoadBotData(MyPlugins.GameAssembly);
      foreach (object plugin in MyPlugins.Plugins)
        this.LoadBotData(plugin.GetType().Assembly);
    }

    protected void LoadBotData(Assembly assembly)
    {
      foreach (Type type in assembly.GetTypes())
      {
        if (!type.IsAbstract && type.IsSubclassOf(typeof (MyBotActionsBase)))
        {
          object[] customAttributes = type.GetCustomAttributes(true);
          string key = "";
          MyBotFactoryBase.BehaviorData behaviorData = new MyBotFactoryBase.BehaviorData(type);
          foreach (object obj in customAttributes)
          {
            switch (obj)
            {
              case MyBehaviorDescriptorAttribute descriptorAttribute:
                key = descriptorAttribute.DescriptorCategory;
                break;
              case BehaviorActionImplAttribute actionImplAttribute:
                behaviorData.LogicType = actionImplAttribute.LogicType;
                break;
            }
          }
          if (!string.IsNullOrEmpty(key) && behaviorData.LogicType != (Type) null)
            this.m_botDataByBehaviorType[key] = behaviorData;
        }
        else if (!type.IsAbstract && type.IsSubclassOf(typeof (MyBotLogic)))
        {
          foreach (object customAttribute in type.GetCustomAttributes(typeof (BehaviorLogicAttribute), true))
            this.m_logicDataByBehaviorSubtype[(customAttribute as BehaviorLogicAttribute).BehaviorSubtype] = new MyBotFactoryBase.LogicData(type);
        }
        else if (!type.IsAbstract && typeof (MyAiTargetBase).IsAssignableFrom(type))
        {
          foreach (object customAttribute in type.GetCustomAttributes(typeof (TargetTypeAttribute), true))
            this.m_TargetTypeByName[(customAttribute as TargetTypeAttribute).TargetType] = type;
        }
      }
    }

    public abstract int MaximumUncontrolledBotCount { get; }

    public abstract int MaximumBotPerPlayer { get; }

    public MyObjectBuilder_Bot GetBotObjectBuilder(IMyBot myAgentBot) => MyBotFactoryBase.m_objectFactory.CreateObjectBuilder<MyObjectBuilder_Bot>(myAgentBot);

    public IMyBot CreateBot(
      MyPlayer player,
      MyObjectBuilder_Bot botBuilder,
      MyBotDefinition botDefinition)
    {
      MyObjectBuilderType typeId;
      if (botBuilder == null)
      {
        typeId = botDefinition.Id.TypeId;
        botBuilder = MyBotFactoryBase.m_objectFactory.CreateObjectBuilder<MyObjectBuilder_Bot>(MyBotFactoryBase.m_objectFactory.GetProducedType(typeId));
      }
      else
        typeId = botBuilder.TypeId;
      if (!this.m_botDataByBehaviorType.ContainsKey(botDefinition.BehaviorType))
        return (IMyBot) null;
      MyBotFactoryBase.BehaviorData behaviorData = this.m_botDataByBehaviorType[botDefinition.BehaviorType];
      IMyBot bot = MyBotFactoryBase.CreateBot(MyBotFactoryBase.m_objectFactory.GetProducedType(typeId), player, botDefinition);
      this.CreateActions(bot, behaviorData.BotActionsType);
      this.CreateLogic(bot, behaviorData.LogicType, botDefinition.BehaviorSubtype);
      bot.Init(botBuilder);
      return bot;
    }

    private void CreateLogic(IMyBot output, Type defaultLogicType, string definitionLogicType)
    {
      Type type;
      if (this.m_logicDataByBehaviorSubtype.ContainsKey(definitionLogicType))
      {
        type = this.m_logicDataByBehaviorSubtype[definitionLogicType].LogicType;
        if (!type.IsSubclassOf(defaultLogicType) && type != defaultLogicType)
          type = defaultLogicType;
      }
      else
        type = defaultLogicType;
      MyBotLogic instance = Activator.CreateInstance(type, (object) output) as MyBotLogic;
      output.InitLogic(instance);
    }

    private void CreateActions(IMyBot bot, Type actionImplType)
    {
      this.m_tmpTypeArray[0] = bot.GetType();
      if (actionImplType.GetConstructor(this.m_tmpTypeArray) == (ConstructorInfo) null)
        bot.BotActions = Activator.CreateInstance(actionImplType) as MyBotActionsBase;
      else
        bot.BotActions = Activator.CreateInstance(actionImplType, (object) bot) as MyBotActionsBase;
      this.m_tmpTypeArray[0] = (Type) null;
    }

    public MyAiTargetBase CreateTargetForBot(MyAgentBot bot)
    {
      MyAiTargetBase myAiTargetBase = (MyAiTargetBase) null;
      this.m_tmpConstructorParamArray[0] = (object) bot;
      Type type;
      this.m_TargetTypeByName.TryGetValue(bot.AgentDefinition.TargetType, out type);
      if (type != (Type) null)
        myAiTargetBase = Activator.CreateInstance(type, this.m_tmpConstructorParamArray) as MyAiTargetBase;
      this.m_tmpConstructorParamArray[0] = (object) null;
      return myAiTargetBase;
    }

    private static IMyBot CreateBot(
      Type botType,
      MyPlayer player,
      MyBotDefinition botDefinition)
    {
      return Activator.CreateInstance(botType, (object) player, (object) botDefinition) as IMyBot;
    }

    public abstract bool CanCreateBotOfType(string behaviorType, bool load);

    public abstract bool GetBotSpawnPosition(string behaviorType, out Vector3D spawnPosition);

    public abstract bool GetBotGroupSpawnPositions(
      string behaviorType,
      int count,
      List<Vector3D> spawnPositions);

    protected class BehaviorData
    {
      public readonly Type BotActionsType;
      public Type LogicType;

      public BehaviorData(Type t) => this.BotActionsType = t;
    }

    protected class LogicData
    {
      public readonly Type LogicType;

      public LogicData(Type t) => this.LogicType = t;
    }

    protected class BehaviorTypeData
    {
      public Type BotType;

      public BehaviorTypeData(Type botType) => this.BotType = botType;
    }
  }
}
