// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeCollection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Debugging;
using System;
using System.Collections.Generic;
using System.IO;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.SessionComponents;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.AI.BehaviorTree
{
  public class MyBehaviorTreeCollection
  {
    private IntPtr m_toolWindowHandle = IntPtr.Zero;
    public const int UPDATE_COUNTER = 10;
    public const int INIT_UPDATE_COUNTER = 8;
    public static readonly string DEFAULT_EXTENSION = ".sbc";
    private readonly Dictionary<MyStringHash, MyBehaviorTreeCollection.BTData> m_BTDataByName;
    private readonly Dictionary<IMyBot, MyStringHash> m_botBehaviorIds;
    private IMyBot m_debugBot;

    public bool TryGetValidToolWindow(out IntPtr windowHandle)
    {
      windowHandle = IntPtr.Zero;
      windowHandle = MyVRage.Platform.Windows.FindWindowInParent("VRageEditor", "BehaviorTreeWindow");
      if (windowHandle == IntPtr.Zero)
        windowHandle = MyVRage.Platform.Windows.FindWindowInParent("Behavior tree tool", "BehaviorTreeWindow");
      return windowHandle != IntPtr.Zero;
    }

    private void SendSelectedTreeForDebug(MyBehaviorTree behaviorTree)
    {
      if (MySessionComponentExtDebug.Static == null)
        return;
      this.DebugSelectedTreeHashSent = true;
      this.DebugCurrentBehaviorTree = behaviorTree.BehaviorTreeName;
      SelectedTreeMsg msg = new SelectedTreeMsg()
      {
        BehaviorTreeName = behaviorTree.BehaviorTreeName
      };
      MySessionComponentExtDebug.Static.SendMessageToClients<SelectedTreeMsg>(msg);
    }

    private void SendDataToTool(IMyBot bot, MyPerTreeBotMemory botTreeMemory)
    {
      if (!this.DebugIsCurrentTreeVerified || this.DebugLastWindowHandle.ToInt32() != this.m_toolWindowHandle.ToInt32())
      {
        MyVRage.Platform.Windows.PostMessage(this.m_toolWindowHandle, 1027U, new IntPtr(this.m_BTDataByName[this.m_botBehaviorIds[bot]].BehaviorTree.GetHashCode()), IntPtr.Zero);
        this.DebugIsCurrentTreeVerified = true;
        this.DebugLastWindowHandle = new IntPtr(this.m_toolWindowHandle.ToInt32());
      }
      MyVRage.Platform.Windows.PostMessage(this.m_toolWindowHandle, 1025U, IntPtr.Zero, IntPtr.Zero);
      for (int index = 0; index < botTreeMemory.NodesMemoryCount; ++index)
      {
        MyBehaviorTreeState nodeState = botTreeMemory.GetNodeMemoryByIndex(index).NodeState;
        if (nodeState != MyBehaviorTreeState.NOT_TICKED)
          MyVRage.Platform.Windows.PostMessage(this.m_toolWindowHandle, 1024U, new IntPtr((long) (uint) index), new IntPtr((int) nodeState));
      }
      MyVRage.Platform.Windows.PostMessage(this.m_toolWindowHandle, 1026U, IntPtr.Zero, IntPtr.Zero);
    }

    public bool DebugSelectedTreeHashSent { get; private set; }

    public IntPtr DebugLastWindowHandle { get; private set; }

    public bool DebugIsCurrentTreeVerified { get; private set; }

    public IMyBot DebugBot
    {
      get => this.m_debugBot;
      set
      {
        this.m_debugBot = value;
        this.DebugSelectedTreeHashSent = false;
      }
    }

    public bool DebugBreakDebugging { get; set; }

    public string DebugCurrentBehaviorTree { get; private set; }

    public MyBehaviorTreeCollection()
    {
      this.m_BTDataByName = new Dictionary<MyStringHash, MyBehaviorTreeCollection.BTData>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
      this.m_botBehaviorIds = new Dictionary<IMyBot, MyStringHash>();
      this.DebugIsCurrentTreeVerified = false;
      foreach (MyBehaviorDefinition behaviorDefinition in MyDefinitionManager.Static.GetBehaviorDefinitions())
        this.BuildBehaviorTree(behaviorDefinition);
    }

    public void Update()
    {
      foreach (MyBehaviorTreeCollection.BTData btData in this.m_BTDataByName.Values)
      {
        MyBehaviorTree behaviorTree = btData.BehaviorTree;
        foreach (MyBehaviorTreeCollection.BotData botData in btData.BotsData)
        {
          IMyBot bot = botData.Bot;
          if (bot.IsValidForUpdate && ++botData.UpdateCounter > 10)
          {
            if (MyFakes.DEBUG_BEHAVIOR_TREE)
            {
              if (MyFakes.DEBUG_BEHAVIOR_TREE_ONE_STEP)
                MyFakes.DEBUG_BEHAVIOR_TREE_ONE_STEP = false;
              else
                continue;
            }
            botData.UpdateCounter = 0;
            bot.BotMemory.PreTickClear();
            try
            {
              behaviorTree.Tick(bot);
            }
            catch (ArgumentException ex)
            {
              MyLog.Default.WriteLine(string.Format("AI BehaviorTree Id: {0}, name: {1} caused ArgumentException: {2}", (object) behaviorTree.BehaviorTreeId, (object) behaviorTree.BehaviorTreeName, (object) ex));
              throw;
            }
            if (MyPlatformGameSettings.ENABLE_BEHAVIOR_TREE_TOOL_COMMUNICATION && this.DebugBot == botData.Bot && (!this.DebugBreakDebugging && MyDebugDrawSettings.DEBUG_DRAW_BOTS) && this.TryGetValidToolWindow(out this.m_toolWindowHandle))
            {
              if (!this.DebugSelectedTreeHashSent || this.m_toolWindowHandle != this.DebugLastWindowHandle || this.DebugCurrentBehaviorTree != this.m_botBehaviorIds[this.DebugBot].String)
                this.SendSelectedTreeForDebug(behaviorTree);
              this.SendDataToTool(botData.Bot, botData.Bot.BotMemory.CurrentTreeBotMemory);
            }
          }
        }
      }
    }

    public bool AssignBotToBehaviorTree(string behaviorName, IMyBot bot)
    {
      MyStringHash key = MyStringHash.TryGet(behaviorName);
      return !(key == MyStringHash.NullOrEmpty) && this.m_BTDataByName.ContainsKey(key) && this.AssignBotToBehaviorTree(this.m_BTDataByName[key].BehaviorTree, bot);
    }

    public bool AssignBotToBehaviorTree(MyBehaviorTree behaviorTree, IMyBot bot)
    {
      if (!behaviorTree.IsCompatibleWithBot(bot.ActionCollection))
        return false;
      this.AssignBotBehaviorTreeInternal(behaviorTree, bot);
      return true;
    }

    private void AssignBotBehaviorTreeInternal(MyBehaviorTree behaviorTree, IMyBot bot)
    {
      bot.BotMemory.AssignBehaviorTree(behaviorTree);
      this.m_BTDataByName[behaviorTree.BehaviorTreeId].BotsData.Add(new MyBehaviorTreeCollection.BotData(bot));
      this.m_botBehaviorIds[bot] = behaviorTree.BehaviorTreeId;
    }

    public void UnassignBotBehaviorTree(IMyBot bot)
    {
      this.m_BTDataByName[this.m_botBehaviorIds[bot]].RemoveBot(bot);
      bot.BotMemory.UnassignCurrentBehaviorTree();
      this.m_botBehaviorIds[bot] = MyStringHash.NullOrEmpty;
    }

    public MyBehaviorTree TryGetBehaviorTreeForBot(IMyBot bot)
    {
      MyBehaviorTreeCollection.BTData btData = (MyBehaviorTreeCollection.BTData) null;
      this.m_BTDataByName.TryGetValue(this.m_botBehaviorIds[bot], out btData);
      return btData?.BehaviorTree;
    }

    public string GetBehaviorName(IMyBot bot)
    {
      MyStringHash myStringHash;
      this.m_botBehaviorIds.TryGetValue(bot, out myStringHash);
      return myStringHash.String;
    }

    public void SetBehaviorName(IMyBot bot, string behaviorName) => this.m_botBehaviorIds[bot] = MyStringHash.GetOrCompute(behaviorName);

    private bool BuildBehaviorTree(MyBehaviorDefinition behaviorDefinition)
    {
      if (this.m_BTDataByName.ContainsKey(behaviorDefinition.Id.SubtypeId))
        return false;
      MyBehaviorTree behaviorTree = new MyBehaviorTree(behaviorDefinition);
      behaviorTree.Construct();
      MyBehaviorTreeCollection.BTData btData = new MyBehaviorTreeCollection.BTData(behaviorTree);
      this.m_BTDataByName.Add(behaviorDefinition.Id.SubtypeId, btData);
      return true;
    }

    public bool ChangeBehaviorTree(string behaviorTreeName, IMyBot bot)
    {
      MyBehaviorTree behaviorTree;
      if (!this.TryGetBehaviorTreeByName(behaviorTreeName, out behaviorTree) || !behaviorTree.IsCompatibleWithBot(bot.ActionCollection))
        return false;
      MyBehaviorTree behaviorTreeForBot = this.TryGetBehaviorTreeForBot(bot);
      bool flag;
      if (behaviorTreeForBot != null)
      {
        if (behaviorTreeForBot.BehaviorTreeId == behaviorTree.BehaviorTreeId)
        {
          flag = false;
        }
        else
        {
          this.UnassignBotBehaviorTree(bot);
          flag = true;
        }
      }
      else
        flag = true;
      if (flag)
        this.AssignBotBehaviorTreeInternal(behaviorTree, bot);
      return flag;
    }

    public bool RebuildBehaviorTree(
      MyBehaviorDefinition newDefinition,
      out MyBehaviorTree outBehaviorTree)
    {
      if (this.m_BTDataByName.ContainsKey(newDefinition.Id.SubtypeId))
      {
        outBehaviorTree = this.m_BTDataByName[newDefinition.Id.SubtypeId].BehaviorTree;
        outBehaviorTree.ReconstructTree(newDefinition);
        return true;
      }
      outBehaviorTree = (MyBehaviorTree) null;
      return false;
    }

    public bool HasBehavior(MyStringHash id) => this.m_BTDataByName.ContainsKey(id);

    public bool TryGetBehaviorTreeByName(string name, out MyBehaviorTree behaviorTree)
    {
      MyStringHash id;
      MyStringHash.TryGet(name, out id);
      if (id != MyStringHash.NullOrEmpty && this.m_BTDataByName.ContainsKey(id))
      {
        behaviorTree = this.m_BTDataByName[id].BehaviorTree;
        return behaviorTree != null;
      }
      behaviorTree = (MyBehaviorTree) null;
      return false;
    }

    public static bool LoadUploadedBehaviorTree(out MyBehaviorDefinition definition)
    {
      MyBehaviorDefinition behaviorDefinition = MyBehaviorTreeCollection.LoadBehaviorTreeFromFile(Path.Combine(MyFileSystem.UserDataPath, "UploadTree" + MyBehaviorTreeCollection.DEFAULT_EXTENSION));
      definition = behaviorDefinition;
      return definition != null;
    }

    private static MyBehaviorDefinition LoadBehaviorTreeFromFile(string path)
    {
      MyObjectBuilder_Definitions objectBuilder;
      MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Definitions>(path, out objectBuilder);
      if (objectBuilder?.AIBehaviors == null || objectBuilder.AIBehaviors.Length == 0)
        return (MyBehaviorDefinition) null;
      MyObjectBuilder_BehaviorTreeDefinition aiBehavior = objectBuilder.AIBehaviors[0];
      MyBehaviorDefinition behaviorDefinition = new MyBehaviorDefinition();
      MyModContext modContext = new MyModContext();
      modContext.Init("BehaviorDefinition", Path.GetFileName(path));
      behaviorDefinition.Init((MyObjectBuilder_DefinitionBase) aiBehavior, modContext);
      return behaviorDefinition;
    }

    private class BotData
    {
      public IMyBot Bot;
      public int UpdateCounter = 8;

      public BotData(IMyBot bot) => this.Bot = bot;
    }

    private class BTData : IEqualityComparer<MyBehaviorTreeCollection.BotData>
    {
      private static readonly MyBehaviorTreeCollection.BotData m_searchData = new MyBehaviorTreeCollection.BotData((IMyBot) null);
      public readonly MyBehaviorTree BehaviorTree;
      public readonly HashSet<MyBehaviorTreeCollection.BotData> BotsData;

      public BTData(MyBehaviorTree behaviorTree)
      {
        this.BehaviorTree = behaviorTree;
        this.BotsData = new HashSet<MyBehaviorTreeCollection.BotData>((IEqualityComparer<MyBehaviorTreeCollection.BotData>) this);
      }

      public void RemoveBot(IMyBot bot)
      {
        MyBehaviorTreeCollection.BTData.m_searchData.Bot = bot;
        this.BotsData.Remove(MyBehaviorTreeCollection.BTData.m_searchData);
      }

      public bool ContainsBot(IMyBot bot)
      {
        MyBehaviorTreeCollection.BTData.m_searchData.Bot = bot;
        return this.BotsData.Contains(MyBehaviorTreeCollection.BTData.m_searchData);
      }

      bool IEqualityComparer<MyBehaviorTreeCollection.BotData>.Equals(
        MyBehaviorTreeCollection.BotData x,
        MyBehaviorTreeCollection.BotData y)
      {
        return x.Bot == y.Bot;
      }

      int IEqualityComparer<MyBehaviorTreeCollection.BotData>.GetHashCode(
        MyBehaviorTreeCollection.BotData obj)
      {
        return obj.Bot.GetHashCode();
      }
    }
  }
}
