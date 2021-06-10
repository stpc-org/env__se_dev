// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTree
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System.Collections.Generic;
using System.Text;
using VRage.Utils;

namespace Sandbox.Game.AI.BehaviorTree
{
  public class MyBehaviorTree
  {
    private static readonly List<MyStringId> m_tmpHelper = new List<MyStringId>();
    private MyBehaviorTreeNode m_root;
    private readonly MyBehaviorTree.MyBehaviorTreeDesc m_treeDesc;

    public int TotalNodeCount => this.m_treeDesc.Nodes.Count;

    public MyBehaviorDefinition BehaviorDefinition { get; private set; }

    public string BehaviorTreeName => this.BehaviorDefinition.Id.SubtypeName;

    public MyStringHash BehaviorTreeId => this.BehaviorDefinition.Id.SubtypeId;

    public MyBehaviorTree(MyBehaviorDefinition def)
    {
      this.BehaviorDefinition = def;
      this.m_treeDesc = new MyBehaviorTree.MyBehaviorTreeDesc();
    }

    public void ReconstructTree(MyBehaviorDefinition def)
    {
      this.BehaviorDefinition = def;
      this.Construct();
    }

    public void Construct()
    {
      this.ClearData();
      this.m_root = (MyBehaviorTreeNode) new MyBehaviorTreeRoot();
      this.m_root.Construct(this.BehaviorDefinition.FirstNode, this.m_treeDesc);
    }

    public void ClearData()
    {
      this.m_treeDesc.MemorableNodesCounter = 0;
      this.m_treeDesc.ActionIds.Clear();
      this.m_treeDesc.Nodes.Clear();
    }

    public void Tick(IMyBot bot)
    {
      int num = (int) this.m_root.Tick(bot, bot.BotMemory.CurrentTreeBotMemory);
    }

    public void CallPostTickOnPath(
      IMyBot bot,
      MyPerTreeBotMemory botTreeMemory,
      IEnumerable<int> postTickNodes)
    {
      foreach (int postTickNode in postTickNodes)
        this.m_treeDesc.Nodes[postTickNode].PostTick(bot, botTreeMemory);
    }

    public bool IsCompatibleWithBot(ActionCollection botActions)
    {
      foreach (MyStringId actionId in this.m_treeDesc.ActionIds)
      {
        if (!botActions.ContainsActionDesc(actionId))
          MyBehaviorTree.m_tmpHelper.Add(actionId);
      }
      if (MyBehaviorTree.m_tmpHelper.Count <= 0)
        return true;
      StringBuilder stringBuilder = new StringBuilder("Error! The behavior tree is not compatible with the bot. Missing bot actions: ");
      foreach (MyStringId myStringId in MyBehaviorTree.m_tmpHelper)
      {
        stringBuilder.Append(myStringId.ToString());
        stringBuilder.Append(", ");
      }
      MyBehaviorTree.m_tmpHelper.Clear();
      return false;
    }

    public MyBehaviorTreeNode GetNodeByIndex(int index) => index >= this.m_treeDesc.Nodes.Count ? (MyBehaviorTreeNode) null : this.m_treeDesc.Nodes[index];

    public override int GetHashCode() => this.m_root.GetHashCode();

    public class MyBehaviorTreeDesc
    {
      public List<MyBehaviorTreeNode> Nodes { get; }

      public HashSet<MyStringId> ActionIds { get; }

      public int MemorableNodesCounter { get; set; }

      public MyBehaviorTreeDesc()
      {
        this.Nodes = new List<MyBehaviorTreeNode>(20);
        this.ActionIds = new HashSet<MyStringId>((IEqualityComparer<MyStringId>) MyStringId.Comparer);
        this.MemorableNodesCounter = 0;
      }
    }
  }
}
