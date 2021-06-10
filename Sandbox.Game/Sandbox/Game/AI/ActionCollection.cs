// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.ActionCollection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.AI.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage;
using VRage.Game;
using VRage.Game.AI;
using VRage.Utils;

namespace Sandbox.Game.AI
{
  public class ActionCollection
  {
    private readonly Dictionary<MyStringId, ActionCollection.BotActionDesc> m_actions = new Dictionary<MyStringId, ActionCollection.BotActionDesc>((IEqualityComparer<MyStringId>) MyStringId.Comparer);

    private ActionCollection()
    {
    }

    public void AddInitAction(string actionName, Action<IMyBot> action) => this.AddInitAction(MyStringId.GetOrCompute(actionName), action);

    public void AddInitAction(MyStringId actionName, Action<IMyBot> action)
    {
      if (!this.m_actions.ContainsKey(actionName))
        this.AddBotActionDesc(actionName);
      this.m_actions[actionName].InitAction = action;
    }

    public void AddAction(
      string actionName,
      MethodInfo methodInfo,
      bool returnsRunning,
      Func<IMyBot, object[], MyBehaviorTreeState> action)
    {
      this.AddAction(MyStringId.GetOrCompute(actionName), methodInfo, returnsRunning, action);
    }

    public void AddAction(
      MyStringId actionId,
      MethodInfo methodInfo,
      bool returnsRunning,
      Func<IMyBot, object[], MyBehaviorTreeState> action)
    {
      if (!this.m_actions.ContainsKey(actionId))
        this.AddBotActionDesc(actionId);
      ActionCollection.BotActionDesc action1 = this.m_actions[actionId];
      ParameterInfo[] parameters = methodInfo.GetParameters();
      action1._Action = action;
      action1.ActionParams = new object[parameters.Length];
      action1.ParametersDesc = new Dictionary<int, MyTuple<Type, MyMemoryParameterType>>();
      action1.ReturnsRunning = returnsRunning;
      for (int key = 0; key < parameters.Length; ++key)
      {
        BTMemParamAttribute customAttribute = parameters[key].GetCustomAttribute<BTMemParamAttribute>(true);
        if (customAttribute != null)
          action1.ParametersDesc.Add(key, new MyTuple<Type, MyMemoryParameterType>(parameters[key].ParameterType.GetElementType(), customAttribute.MemoryType));
      }
    }

    public void AddPostAction(string actionName, Action<IMyBot> action) => this.AddPostAction(MyStringId.GetOrCompute(actionName), action);

    public void AddPostAction(MyStringId actionId, Action<IMyBot> action)
    {
      if (!this.m_actions.ContainsKey(actionId))
        this.AddBotActionDesc(actionId);
      this.m_actions[actionId].PostAction = action;
    }

    private void AddBotActionDesc(MyStringId actionId) => this.m_actions.Add(actionId, new ActionCollection.BotActionDesc());

    public void PerformInitAction(IMyBot bot, MyStringId actionId)
    {
      ActionCollection.BotActionDesc action = this.m_actions[actionId];
      if (action == null)
        return;
      action.InitAction(bot);
    }

    public MyBehaviorTreeState PerformAction(
      IMyBot bot,
      MyStringId actionId,
      object[] args)
    {
      ActionCollection.BotActionDesc action = this.m_actions[actionId];
      if (action == null)
        return MyBehaviorTreeState.ERROR;
      MyPerTreeBotMemory currentTreeBotMemory = bot.BotMemory.CurrentTreeBotMemory;
      if (action.ParametersDesc.Count == 0)
        return action._Action(bot, args);
      if (args == null)
        return MyBehaviorTreeState.FAILURE;
      ActionCollection.LoadActionParams(action, args, currentTreeBotMemory);
      int num = (int) action._Action(bot, action.ActionParams);
      ActionCollection.SaveActionParams(action, args, currentTreeBotMemory);
      return (MyBehaviorTreeState) num;
    }

    private static void LoadActionParams(
      ActionCollection.BotActionDesc action,
      object[] args,
      MyPerTreeBotMemory botMemory)
    {
      for (int key = 0; key < args.Length; ++key)
      {
        object obj = args[key];
        if (obj is Boxed<MyStringId> && action.ParametersDesc.ContainsKey(key))
        {
          MyTuple<Type, MyMemoryParameterType> myTuple = action.ParametersDesc[key];
          Boxed<MyStringId> boxed = obj as Boxed<MyStringId>;
          MyBBMemoryValue myBbMemoryValue = (MyBBMemoryValue) null;
          if (botMemory.TryGetFromBlackboard<MyBBMemoryValue>((MyStringId) boxed, out myBbMemoryValue))
          {
            if (myBbMemoryValue == null || myBbMemoryValue.GetType() == myTuple.Item1 && myTuple.Item2 != MyMemoryParameterType.OUT)
            {
              action.ActionParams[key] = (object) myBbMemoryValue;
            }
            else
            {
              int num = myBbMemoryValue.GetType() != myTuple.Item1 ? 1 : 0;
              action.ActionParams[key] = (object) null;
            }
          }
          else
            action.ActionParams[key] = (object) null;
        }
        else
          action.ActionParams[key] = obj;
      }
    }

    private static void SaveActionParams(
      ActionCollection.BotActionDesc action,
      object[] args,
      MyPerTreeBotMemory botMemory)
    {
      foreach (int key in action.ParametersDesc.Keys)
      {
        MyStringId id = (MyStringId) (args[key] as Boxed<MyStringId>);
        if (action.ParametersDesc[key].Item2 != MyMemoryParameterType.IN)
          botMemory.SaveToBlackboard(id, action.ActionParams[key] as MyBBMemoryValue);
      }
    }

    public void PerformPostAction(IMyBot bot, MyStringId actionId)
    {
      ActionCollection.BotActionDesc action = this.m_actions[actionId];
      if (action == null)
        return;
      action.PostAction(bot);
    }

    public bool ContainsInitAction(MyStringId actionId) => this.m_actions[actionId].InitAction != null;

    public bool ContainsPostAction(MyStringId actionId) => this.m_actions[actionId].PostAction != null;

    public bool ContainsAction(MyStringId actionId) => this.m_actions[actionId]._Action != null;

    public bool ContainsActionDesc(MyStringId actionId) => this.m_actions.ContainsKey(actionId);

    public bool ReturnsRunning(MyStringId actionId) => this.m_actions[actionId].ReturnsRunning;

    public static ActionCollection CreateActionCollection(IMyBot bot)
    {
      ActionCollection actions = new ActionCollection();
      foreach (MethodInfo method in bot.BotActions.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
        ActionCollection.ExtractAction(actions, method);
      return actions;
    }

    private static void ExtractAction(ActionCollection actions, MethodInfo methodInfo)
    {
      MyBehaviorTreeActionAttribute customAttribute = methodInfo.GetCustomAttribute<MyBehaviorTreeActionAttribute>();
      if (customAttribute == null)
        return;
      switch (customAttribute.ActionType)
      {
        case MyBehaviorTreeActionType.INIT:
          actions.AddInitAction(customAttribute.ActionName, (Action<IMyBot>) (x => methodInfo.Invoke((object) x.BotActions, (object[]) null)));
          break;
        case MyBehaviorTreeActionType.BODY:
          actions.AddAction(customAttribute.ActionName, methodInfo, customAttribute.ReturnsRunning, (Func<IMyBot, object[], MyBehaviorTreeState>) ((x, y) => (MyBehaviorTreeState) methodInfo.Invoke((object) x.BotActions, y)));
          break;
        case MyBehaviorTreeActionType.POST:
          actions.AddPostAction(customAttribute.ActionName, (Action<IMyBot>) (x => methodInfo.Invoke((object) x.BotActions, (object[]) null)));
          break;
      }
    }

    public class BotActionDesc
    {
      public Action<IMyBot> InitAction;
      public object[] ActionParams;
      public Dictionary<int, MyTuple<Type, MyMemoryParameterType>> ParametersDesc;
      public Func<IMyBot, object[], MyBehaviorTreeState> _Action;
      public Action<IMyBot> PostAction;
      public bool ReturnsRunning;
    }
  }
}
