// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeDecoratorNode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.AI.BehaviorTree.DecoratorLogic;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.AI.BehaviorTree
{
  [MyBehaviorTreeNodeType(typeof (MyObjectBuilder_BehaviorTreeDecoratorNode), typeof (MyBehaviorTreeDecoratorNodeMemory))]
  public class MyBehaviorTreeDecoratorNode : MyBehaviorTreeNode
  {
    private MyBehaviorTreeNode m_child;
    private IMyDecoratorLogic m_decoratorLogic;
    private MyBehaviorTreeState m_defaultReturnValue;
    private string m_decoratorLogicName;

    public string GetName() => this.m_decoratorLogicName;

    private MyDecoratorDefaultReturnValues DecoratorDefaultReturnValue => (MyDecoratorDefaultReturnValues) this.m_defaultReturnValue;

    public MyBehaviorTreeDecoratorNode()
    {
      this.m_child = (MyBehaviorTreeNode) null;
      this.m_decoratorLogic = (IMyDecoratorLogic) null;
    }

    public override void Construct(
      MyObjectBuilder_BehaviorTreeNode nodeDefinition,
      MyBehaviorTree.MyBehaviorTreeDesc treeDesc)
    {
      base.Construct(nodeDefinition, treeDesc);
      MyObjectBuilder_BehaviorTreeDecoratorNode treeDecoratorNode = nodeDefinition as MyObjectBuilder_BehaviorTreeDecoratorNode;
      this.m_defaultReturnValue = (MyBehaviorTreeState) treeDecoratorNode.DefaultReturnValue;
      this.m_decoratorLogicName = treeDecoratorNode.DecoratorLogic.GetType().Name;
      this.m_decoratorLogic = MyBehaviorTreeDecoratorNode.GetDecoratorLogic(treeDecoratorNode.DecoratorLogic);
      this.m_decoratorLogic.Construct(treeDecoratorNode.DecoratorLogic);
      if (treeDecoratorNode.BTNode == null)
        return;
      this.m_child = MyBehaviorTreeNodeFactory.CreateBTNode(treeDecoratorNode.BTNode);
      this.m_child.Construct(treeDecoratorNode.BTNode, treeDesc);
    }

    public override MyBehaviorTreeState Tick(
      IMyBot bot,
      MyPerTreeBotMemory botTreeMemory)
    {
      MyBehaviorTreeDecoratorNodeMemory nodeMemoryByIndex = botTreeMemory.GetNodeMemoryByIndex(this.MemoryIndex) as MyBehaviorTreeDecoratorNodeMemory;
      if (this.m_child == null)
        return this.m_defaultReturnValue;
      if (nodeMemoryByIndex.ChildState == MyBehaviorTreeState.RUNNING)
        return this.TickChild(bot, botTreeMemory, nodeMemoryByIndex);
      this.m_decoratorLogic.Update(nodeMemoryByIndex.DecoratorLogicMemory);
      if (this.m_decoratorLogic.CanRun(nodeMemoryByIndex.DecoratorLogicMemory))
        return this.TickChild(bot, botTreeMemory, nodeMemoryByIndex);
      if (this.IsRunningStateSource)
        bot.BotMemory.ProcessLastRunningNode((MyBehaviorTreeNode) this);
      botTreeMemory.GetNodeMemoryByIndex(this.MemoryIndex).NodeState = this.m_defaultReturnValue;
      if (MyDebugDrawSettings.DEBUG_DRAW_BOTS && this.m_defaultReturnValue == MyBehaviorTreeState.RUNNING)
        this.m_runningActionName = "Par_N" + this.m_decoratorLogicName;
      return this.m_defaultReturnValue;
    }

    private MyBehaviorTreeState TickChild(
      IMyBot bot,
      MyPerTreeBotMemory botTreeMemory,
      MyBehaviorTreeDecoratorNodeMemory thisMemory)
    {
      bot.BotMemory.RememberNode(this.m_child.MemoryIndex);
      MyBehaviorTreeState behaviorTreeState = this.m_child.Tick(bot, botTreeMemory);
      thisMemory.NodeState = behaviorTreeState;
      thisMemory.ChildState = behaviorTreeState;
      if (behaviorTreeState != MyBehaviorTreeState.RUNNING)
        bot.BotMemory.ForgetNode();
      return behaviorTreeState;
    }

    [Conditional("DEBUG")]
    private void RecordRunningNodeName(MyBehaviorTreeState state)
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_BOTS || state != MyBehaviorTreeState.RUNNING)
        return;
      this.m_runningActionName = this.m_child.m_runningActionName;
    }

    public override void PostTick(IMyBot bot, MyPerTreeBotMemory botTreeMemory)
    {
      base.PostTick(bot, botTreeMemory);
      MyBehaviorTreeDecoratorNodeMemory nodeMemoryByIndex = botTreeMemory.GetNodeMemoryByIndex(this.MemoryIndex) as MyBehaviorTreeDecoratorNodeMemory;
      if (nodeMemoryByIndex.ChildState != MyBehaviorTreeState.NOT_TICKED)
      {
        nodeMemoryByIndex.PostTickMemory();
        this.m_child?.PostTick(bot, botTreeMemory);
      }
      else
      {
        if (!this.IsRunningStateSource)
          return;
        nodeMemoryByIndex.PostTickMemory();
      }
    }

    public override void DebugDraw(
      Vector2 position,
      Vector2 size,
      List<MyBehaviorTreeNodeMemory> nodesMemory)
    {
    }

    public override bool IsRunningStateSource => this.m_defaultReturnValue == MyBehaviorTreeState.RUNNING;

    private static IMyDecoratorLogic GetDecoratorLogic(
      MyObjectBuilder_BehaviorTreeDecoratorNode.Logic logicData)
    {
      switch (logicData)
      {
        case MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic _:
          return (IMyDecoratorLogic) new MyBehaviorTreeDecoratorTimerLogic();
        case MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic _:
          return (IMyDecoratorLogic) new MyBehaviorTreeDecoratorCounterLogic();
        default:
          return (IMyDecoratorLogic) null;
      }
    }

    public override MyBehaviorTreeNodeMemory GetNewMemoryObject()
    {
      MyBehaviorTreeDecoratorNodeMemory newMemoryObject = base.GetNewMemoryObject() as MyBehaviorTreeDecoratorNodeMemory;
      newMemoryObject.DecoratorLogicMemory = this.m_decoratorLogic.GetNewMemoryObject();
      return (MyBehaviorTreeNodeMemory) newMemoryObject;
    }

    public override int GetHashCode() => (((base.GetHashCode() * 397 ^ this.m_child.GetHashCode()) * 397 ^ this.m_decoratorLogic.GetHashCode()) * 397 ^ this.m_decoratorLogicName.GetHashCode()) * 397 ^ this.DecoratorDefaultReturnValue.GetHashCode();

    public override string ToString() => "DEC: " + (object) this.m_decoratorLogic;

    private class Sandbox_Game_AI_BehaviorTree_MyBehaviorTreeDecoratorNode\u003C\u003EActor : IActivator, IActivator<MyBehaviorTreeDecoratorNode>
    {
      object IActivator.CreateInstance() => (object) new MyBehaviorTreeDecoratorNode();

      MyBehaviorTreeDecoratorNode IActivator<MyBehaviorTreeDecoratorNode>.CreateInstance() => new MyBehaviorTreeDecoratorNode();
    }
  }
}
