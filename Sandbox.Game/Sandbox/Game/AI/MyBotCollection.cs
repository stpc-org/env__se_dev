// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.MyBotCollection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.AI.BehaviorTree;
using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Graphics;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI
{
  public class MyBotCollection
  {
    private readonly Dictionary<Type, ActionCollection> m_botActions;
    private readonly MyBehaviorTreeCollection m_behaviorTreeCollection;
    private readonly Dictionary<string, int> m_botsCountPerBehavior;
    private readonly List<int> m_botsQueue;
    private int m_botIndex = -1;

    public bool HasBot => this.m_botsQueue.Count > 0;

    public int TotalBotCount { get; set; }

    public Dictionary<int, IMyBot> BotsDictionary { get; }

    public MyBotCollection(MyBehaviorTreeCollection behaviorTreeCollection)
    {
      this.m_behaviorTreeCollection = behaviorTreeCollection;
      this.BotsDictionary = new Dictionary<int, IMyBot>(8);
      this.m_botActions = new Dictionary<Type, ActionCollection>(8);
      this.m_botsQueue = new List<int>(8);
      this.m_botsCountPerBehavior = new Dictionary<string, int>();
    }

    public void UnloadData()
    {
      foreach (KeyValuePair<int, IMyBot> bots in this.BotsDictionary)
        bots.Value.Cleanup();
    }

    public void Update()
    {
      foreach (KeyValuePair<int, IMyBot> bots in this.BotsDictionary)
        bots.Value.Update();
    }

    public void AddBot(int botHandler, IMyBot newBot)
    {
      if (this.BotsDictionary.ContainsKey(botHandler))
        return;
      ActionCollection actionCollection;
      if (!this.m_botActions.ContainsKey(newBot.BotActions.GetType()))
      {
        actionCollection = ActionCollection.CreateActionCollection(newBot);
        this.m_botActions[newBot.GetType()] = actionCollection;
      }
      else
        actionCollection = this.m_botActions[newBot.GetType()];
      newBot.InitActions(actionCollection);
      if (string.IsNullOrEmpty(newBot.BehaviorSubtypeName))
        this.m_behaviorTreeCollection.AssignBotToBehaviorTree(newBot.BotDefinition.BotBehaviorTree.SubtypeName, newBot);
      else
        this.m_behaviorTreeCollection.AssignBotToBehaviorTree(newBot.BehaviorSubtypeName, newBot);
      this.BotsDictionary.Add(botHandler, newBot);
      this.m_botsQueue.Add(botHandler);
      if (this.m_botsCountPerBehavior.ContainsKey(newBot.BotDefinition.BehaviorType))
        this.m_botsCountPerBehavior[newBot.BotDefinition.BehaviorType]++;
      else
        this.m_botsCountPerBehavior[newBot.BotDefinition.BehaviorType] = 1;
    }

    public void ResetBots(string treeName)
    {
      foreach (IMyBot myBot in this.BotsDictionary.Values)
      {
        if (myBot.BehaviorSubtypeName == treeName)
          myBot.Reset();
      }
    }

    public void CheckCompatibilityWithBots(MyBehaviorTree behaviorTree)
    {
      foreach (IMyBot bot in this.BotsDictionary.Values)
      {
        if (behaviorTree.BehaviorTreeName.CompareTo(bot.BehaviorSubtypeName) == 0)
        {
          if (!behaviorTree.IsCompatibleWithBot(bot.ActionCollection))
            this.m_behaviorTreeCollection.UnassignBotBehaviorTree(bot);
          else
            bot.BotMemory.ResetMemory();
        }
      }
    }

    public int GetHandleToFirstBot() => this.m_botsQueue.Count > 0 ? this.m_botsQueue[0] : -1;

    public int GetHandleToFirstBot(string behaviorType)
    {
      foreach (int bots in this.m_botsQueue)
      {
        if (this.BotsDictionary[bots].BotDefinition.BehaviorType == behaviorType)
          return bots;
      }
      return -1;
    }

    public BotType GetBotType(int botHandler)
    {
      if (this.BotsDictionary.ContainsKey(botHandler))
      {
        MyBotLogic botLogic = this.BotsDictionary[botHandler].BotLogic;
        if (botLogic != null)
          return botLogic.BotType;
      }
      return BotType.UNKNOWN;
    }

    public void TryRemoveBot(int botHandler)
    {
      IMyBot myBot;
      this.BotsDictionary.TryGetValue(botHandler, out myBot);
      if (myBot == null)
        return;
      string behaviorType = myBot.BotDefinition.BehaviorType;
      myBot.Cleanup();
      if (this.m_botIndex != -1)
      {
        if (this.m_behaviorTreeCollection.DebugBot == myBot)
          this.m_behaviorTreeCollection.DebugBot = (IMyBot) null;
        int num = this.m_botsQueue.IndexOf(botHandler);
        if (num < this.m_botIndex)
          --this.m_botIndex;
        else if (num == this.m_botIndex)
          this.m_botIndex = -1;
      }
      this.BotsDictionary.Remove(botHandler);
      this.m_botsQueue.Remove(botHandler);
      this.m_botsCountPerBehavior[behaviorType]--;
    }

    public int GetCurrentBotsCount(string behaviorType) => !this.m_botsCountPerBehavior.ContainsKey(behaviorType) ? 0 : this.m_botsCountPerBehavior[behaviorType];

    public BotType TryGetBot<BotType>(int botHandler) where BotType : class, IMyBot
    {
      IMyBot myBot;
      this.BotsDictionary.TryGetValue(botHandler, out myBot);
      return myBot as BotType;
    }

    public DictionaryReader<int, IMyBot> GetAllBots() => new DictionaryReader<int, IMyBot>(this.BotsDictionary);

    public void GetBotsData(
      List<MyObjectBuilder_AIComponent.BotData> botDataList,
      MyObjectBuilder_AIComponent ob)
    {
      if (ob.BotBrains == null)
        ob.BotBrains = new List<MyObjectBuilder_AIComponent.BotData>();
      foreach (KeyValuePair<int, IMyBot> bots in this.BotsDictionary)
      {
        MyObjectBuilder_AIComponent.BotData botData = new MyObjectBuilder_AIComponent.BotData()
        {
          BotBrain = bots.Value.GetObjectBuilder(),
          PlayerHandle = bots.Key
        };
        botDataList.Add(botData);
        ob.BotBrains.Add(botData);
      }
    }

    public int GetCreatedBotCount()
    {
      int num = 0;
      foreach (IMyBot myBot in this.BotsDictionary.Values)
      {
        if (myBot.CreatedByPlayer)
          ++num;
      }
      return num;
    }

    public int GetGeneratedBotCount()
    {
      int num = 0;
      foreach (IMyBot myBot in this.BotsDictionary.Values)
      {
        if (!myBot.CreatedByPlayer)
          ++num;
      }
      return num;
    }

    internal void SelectBotForDebugging(IMyBot bot)
    {
      this.m_behaviorTreeCollection.DebugBot = bot;
      for (int index = 0; index < this.m_botsQueue.Count; ++index)
      {
        if (this.BotsDictionary[this.m_botsQueue[index]] == bot)
        {
          this.m_botIndex = index;
          break;
        }
      }
    }

    public bool IsBotSelectedForDebugging(IMyBot bot) => this.m_behaviorTreeCollection.DebugBot == bot;

    internal void SelectBotForDebugging(int index)
    {
      if (this.m_botIndex == -1)
        return;
      this.m_behaviorTreeCollection.DebugBot = this.BotsDictionary[this.m_botsQueue[index]];
    }

    internal void DebugSelectNextBot()
    {
      ++this.m_botIndex;
      if (this.m_botIndex == this.m_botsQueue.Count)
        this.m_botIndex = this.m_botsQueue.Count != 0 ? 0 : -1;
      this.SelectBotForDebugging(this.m_botIndex);
    }

    internal void DebugSelectPreviousBot()
    {
      --this.m_botIndex;
      if (this.m_botIndex < 0)
        this.m_botIndex = this.m_botsQueue.Count <= 0 ? -1 : this.m_botsQueue.Count - 1;
      this.SelectBotForDebugging(this.m_botIndex);
    }

    internal void DebugDrawBots()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_BOTS)
        return;
      Vector2 normalizedCoord = new Vector2(0.01f, 0.4f);
      for (int index = 0; index < this.m_botsQueue.Count; ++index)
      {
        if (this.BotsDictionary[this.m_botsQueue[index]] is IMyEntityBot bots)
        {
          Color color1 = Color.Green;
          if (this.m_botIndex == -1 || index != this.m_botIndex)
            color1 = Color.Red;
          Vector2 fromNormalizedCoord = MyGuiManager.GetHudPixelCoordFromNormalizedCoord(normalizedCoord);
          string str = string.Format("Bot[{0}]: {1}", (object) index, (object) bots.BehaviorSubtypeName);
          if (bots is MyAgentBot)
            str += (bots as MyAgentBot).LastActions.GetLastActionsString();
          string text = str;
          Color color2 = color1;
          MyRenderProxy.DebugDrawText2D(fromNormalizedCoord, text, color2, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
          MyCharacter botEntity = bots.BotEntity as MyCharacter;
          IMyFaction myFaction = (IMyFaction) null;
          if (botEntity != null)
            myFaction = MySession.Static.Factions.TryGetPlayerFaction(botEntity.ControllerInfo.Controller.Player.Identity.IdentityId);
          if (bots.BotEntity != null)
          {
            Vector3D center = bots.BotEntity.PositionComp.WorldAABB.Center;
            center.Y += bots.BotEntity.PositionComp.WorldAABB.HalfExtents.Y;
            MyRenderProxy.DebugDrawText3D(center, string.Format("Bot:{0}", (object) index), color1, 1f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
            MyRenderProxy.DebugDrawText3D(center - new Vector3(0.0f, -0.5f, 0.0f), myFaction == null ? "NO_FACTION" : myFaction.Tag, color1, 1f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
          }
          normalizedCoord.Y += 0.02f;
        }
      }
    }

    internal void DebugDraw()
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyDebugDrawSettings.DEBUG_DRAW_BOTS)
        return;
      foreach (KeyValuePair<int, IMyBot> bots in this.BotsDictionary)
        bots.Value.DebugDraw();
    }
  }
}
