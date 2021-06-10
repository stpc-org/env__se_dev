// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.MyBotMemory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.AI.BehaviorTree;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.AI
{
  public class MyBotMemory
  {
    private readonly IMyBot m_memoryUser;
    private MyBehaviorTree m_behaviorTree;
    private readonly Stack<int> m_newNodePath;
    private readonly HashSet<int> m_oldNodePath;

    public MyPerTreeBotMemory CurrentTreeBotMemory { get; private set; }

    public bool HasOldPath => this.m_oldNodePath.Count > 0;

    public int LastRunningNodeIndex { get; private set; }

    public bool HasPathToSave => this.m_newNodePath.Count > 0;

    public int TickCounter { get; private set; }

    public MyBotMemory Clone()
    {
      MyBotMemory myBotMemory = new MyBotMemory(this.m_memoryUser);
      myBotMemory.m_behaviorTree = this.m_behaviorTree;
      myBotMemory.Init(this.GetObjectBuilder());
      return myBotMemory;
    }

    public MyBotMemory(IMyBot bot)
    {
      this.LastRunningNodeIndex = -1;
      this.m_memoryUser = bot;
      this.m_newNodePath = new Stack<int>(20);
      this.m_oldNodePath = new HashSet<int>();
    }

    public void Init(MyObjectBuilder_BotMemory builder)
    {
      if (builder.BehaviorTreeMemory != null)
      {
        MyPerTreeBotMemory perTreeBotMemory = new MyPerTreeBotMemory();
        foreach (MyObjectBuilder_BehaviorTreeNodeMemory builder1 in builder.BehaviorTreeMemory.Memory)
        {
          MyBehaviorTreeNodeMemory nodeMemory = MyBehaviorTreeNodeMemoryFactory.CreateNodeMemory(builder1);
          nodeMemory.Init(builder1);
          perTreeBotMemory.AddNodeMemory(nodeMemory);
        }
        if (builder.BehaviorTreeMemory.BlackboardMemory != null)
        {
          foreach (MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory blackboardMemory in builder.BehaviorTreeMemory.BlackboardMemory)
            perTreeBotMemory.AddBlackboardMemoryInstance(blackboardMemory.MemberName, blackboardMemory.Value);
        }
        this.CurrentTreeBotMemory = perTreeBotMemory;
      }
      if (builder.OldPath != null)
      {
        for (int index = 0; index < builder.OldPath.Count; ++index)
          this.m_oldNodePath.Add(index);
      }
      if (builder.NewPath != null)
      {
        for (int index = 0; index < builder.NewPath.Count; ++index)
          this.m_newNodePath.Push(builder.NewPath[index]);
      }
      this.LastRunningNodeIndex = builder.LastRunningNodeIndex;
      this.TickCounter = 0;
    }

    public MyObjectBuilder_BotMemory GetObjectBuilder()
    {
      MyObjectBuilder_BotMemory builderBotMemory = new MyObjectBuilder_BotMemory()
      {
        LastRunningNodeIndex = this.LastRunningNodeIndex,
        NewPath = this.m_newNodePath.ToList<int>(),
        OldPath = this.m_oldNodePath.ToList<int>()
      };
      MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory behaviorTreeNodesMemory = new MyObjectBuilder_BotMemory.BehaviorTreeNodesMemory();
      behaviorTreeNodesMemory.BehaviorName = this.m_behaviorTree.BehaviorTreeName;
      behaviorTreeNodesMemory.Memory = new List<MyObjectBuilder_BehaviorTreeNodeMemory>(this.CurrentTreeBotMemory.NodesMemoryCount);
      foreach (MyBehaviorTreeNodeMemory behaviorTreeNodeMemory in this.CurrentTreeBotMemory.NodesMemory)
        behaviorTreeNodesMemory.Memory.Add(behaviorTreeNodeMemory.GetObjectBuilder());
      behaviorTreeNodesMemory.BlackboardMemory = new List<MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory>();
      foreach (KeyValuePair<MyStringId, MyBBMemoryValue> keyValuePair in this.CurrentTreeBotMemory.BBMemory)
        behaviorTreeNodesMemory.BlackboardMemory.Add(new MyObjectBuilder_BotMemory.BehaviorTreeBlackboardMemory()
        {
          MemberName = keyValuePair.Key.ToString(),
          Value = keyValuePair.Value
        });
      builderBotMemory.BehaviorTreeMemory = behaviorTreeNodesMemory;
      return builderBotMemory;
    }

    public void AssignBehaviorTree(MyBehaviorTree behaviorTree)
    {
      if (this.CurrentTreeBotMemory == null && (this.m_behaviorTree == null || behaviorTree.BehaviorTreeId == this.m_behaviorTree.BehaviorTreeId))
        this.CurrentTreeBotMemory = this.CreateBehaviorTreeMemory(behaviorTree);
      else if (!this.ValidateMemoryForBehavior(behaviorTree))
      {
        this.CurrentTreeBotMemory.Clear();
        this.ClearPathMemory(false);
        MyBotMemory.ResetMemoryInternal(behaviorTree, this.CurrentTreeBotMemory);
      }
      this.m_behaviorTree = behaviorTree;
    }

    private MyPerTreeBotMemory CreateBehaviorTreeMemory(
      MyBehaviorTree behaviorTree)
    {
      MyPerTreeBotMemory treeMemory = new MyPerTreeBotMemory();
      MyBotMemory.ResetMemoryInternal(behaviorTree, treeMemory);
      return treeMemory;
    }

    public bool ValidateMemoryForBehavior(MyBehaviorTree behaviorTree)
    {
      bool flag = true;
      if (this.CurrentTreeBotMemory.NodesMemoryCount != behaviorTree.TotalNodeCount)
      {
        flag = false;
      }
      else
      {
        for (int index = 0; index < this.CurrentTreeBotMemory.NodesMemoryCount; ++index)
        {
          if (this.CurrentTreeBotMemory.GetNodeMemoryByIndex(index).GetType() != behaviorTree.GetNodeByIndex(index).MemoryType)
          {
            flag = false;
            break;
          }
        }
      }
      return flag;
    }

    public void PreTickClear()
    {
      if (this.HasPathToSave)
        this.PrepareForNewNodePath();
      this.CurrentTreeBotMemory.ClearNodesData();
      ++this.TickCounter;
    }

    public void ClearPathMemory(bool postTick)
    {
      if (postTick)
        this.PostTickPaths();
      this.m_newNodePath.Clear();
      this.m_oldNodePath.Clear();
      this.LastRunningNodeIndex = -1;
    }

    public void ResetMemory(bool clearMemory = false)
    {
      if (this.m_behaviorTree == null)
        return;
      if (clearMemory)
        this.ClearPathMemory(true);
      this.CurrentTreeBotMemory.Clear();
      MyBotMemory.ResetMemoryInternal(this.m_behaviorTree, this.CurrentTreeBotMemory);
    }

    public void UnassignCurrentBehaviorTree()
    {
      this.ClearPathMemory(true);
      this.CurrentTreeBotMemory = (MyPerTreeBotMemory) null;
      this.m_behaviorTree = (MyBehaviorTree) null;
    }

    private static void ResetMemoryInternal(
      MyBehaviorTree behaviorTree,
      MyPerTreeBotMemory treeMemory)
    {
      for (int index = 0; index < behaviorTree.TotalNodeCount; ++index)
        treeMemory.AddNodeMemory(behaviorTree.GetNodeByIndex(index).GetNewMemoryObject());
    }

    private void ClearOldPath()
    {
      this.m_oldNodePath.Clear();
      this.LastRunningNodeIndex = -1;
    }

    private void PostTickPaths()
    {
      if (this.m_behaviorTree == null)
        return;
      this.m_behaviorTree.CallPostTickOnPath(this.m_memoryUser, this.CurrentTreeBotMemory, (IEnumerable<int>) this.m_oldNodePath);
      this.m_behaviorTree.CallPostTickOnPath(this.m_memoryUser, this.CurrentTreeBotMemory, (IEnumerable<int>) this.m_newNodePath);
    }

    private void PostTickOldPath()
    {
      if (!this.HasOldPath)
        return;
      this.m_oldNodePath.ExceptWith((IEnumerable<int>) this.m_newNodePath);
      this.m_behaviorTree.CallPostTickOnPath(this.m_memoryUser, this.CurrentTreeBotMemory, (IEnumerable<int>) this.m_oldNodePath);
      this.ClearOldPath();
    }

    public void RememberNode(int nodeIndex) => this.m_newNodePath.Push(nodeIndex);

    public void ForgetNode() => this.m_newNodePath.Pop();

    public void PrepareForNewNodePath()
    {
      this.m_oldNodePath.Clear();
      this.m_oldNodePath.UnionWith((IEnumerable<int>) this.m_newNodePath);
      this.LastRunningNodeIndex = this.m_newNodePath.Peek();
      this.m_newNodePath.Clear();
    }

    public void ProcessLastRunningNode(MyBehaviorTreeNode node)
    {
      if (this.LastRunningNodeIndex == -1)
        return;
      if (this.LastRunningNodeIndex != node.MemoryIndex)
        this.PostTickOldPath();
      else
        this.ClearOldPath();
    }
  }
}
