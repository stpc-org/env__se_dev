// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeRoot
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.BehaviorTree
{
  internal class MyBehaviorTreeRoot : MyBehaviorTreeNode
  {
    private MyBehaviorTreeNode m_child;

    public override bool IsRunningStateSource => false;

    public override void Construct(
      MyObjectBuilder_BehaviorTreeNode nodeDefinition,
      MyBehaviorTree.MyBehaviorTreeDesc treeDesc)
    {
      base.Construct(nodeDefinition, treeDesc);
      this.m_child = MyBehaviorTreeNodeFactory.CreateBTNode(nodeDefinition);
      this.m_child.Construct(nodeDefinition, treeDesc);
    }

    public override MyBehaviorTreeState Tick(
      IMyBot bot,
      MyPerTreeBotMemory botTreeMemory)
    {
      bot.BotMemory.RememberNode(this.m_child.MemoryIndex);
      if (MyDebugDrawSettings.DEBUG_DRAW_BOTS)
        bot.LastBotMemory = bot.BotMemory.Clone();
      MyBehaviorTreeState behaviorTreeState = this.m_child.Tick(bot, botTreeMemory);
      botTreeMemory.GetNodeMemoryByIndex(this.MemoryIndex).NodeState = behaviorTreeState;
      if (behaviorTreeState != MyBehaviorTreeState.RUNNING)
        bot.BotMemory.ForgetNode();
      return behaviorTreeState;
    }

    [Conditional("DEBUG")]
    private void RecordRunningNodeName(IMyBot bot, MyBehaviorTreeState state)
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_BOTS || !(bot is MyAgentBot))
        return;
      switch (state)
      {
        case MyBehaviorTreeState.ERROR:
          (bot as MyAgentBot).LastActions.AddLastAction("error");
          break;
        case MyBehaviorTreeState.NOT_TICKED:
          (bot as MyAgentBot).LastActions.AddLastAction("not ticked");
          break;
        case MyBehaviorTreeState.SUCCESS:
          (bot as MyAgentBot).LastActions.AddLastAction("failure");
          break;
        case MyBehaviorTreeState.FAILURE:
          (bot as MyAgentBot).LastActions.AddLastAction("failure");
          break;
        case MyBehaviorTreeState.RUNNING:
          (bot as MyAgentBot).LastActions.AddLastAction(this.m_child.m_runningActionName);
          break;
      }
    }

    public override void DebugDraw(
      Vector2 pos,
      Vector2 size,
      List<MyBehaviorTreeNodeMemory> nodesMemory)
    {
      MyRenderProxy.DebugDrawText2D(pos, "ROOT", nodesMemory[this.MemoryIndex].NodeStateColor, MyBehaviorTreeNode.DEBUG_TEXT_SCALE, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      pos.Y += MyBehaviorTreeNode.DEBUG_ROOT_OFFSET;
      this.m_child.DebugDraw(pos, size, nodesMemory);
    }

    public override MyBehaviorTreeNodeMemory GetNewMemoryObject() => new MyBehaviorTreeNodeMemory();

    public override int GetHashCode() => this.m_child.GetHashCode();

    private class Sandbox_Game_AI_BehaviorTree_MyBehaviorTreeRoot\u003C\u003EActor : IActivator, IActivator<MyBehaviorTreeRoot>
    {
      object IActivator.CreateInstance() => (object) new MyBehaviorTreeRoot();

      MyBehaviorTreeRoot IActivator<MyBehaviorTreeRoot>.CreateInstance() => new MyBehaviorTreeRoot();
    }
  }
}
