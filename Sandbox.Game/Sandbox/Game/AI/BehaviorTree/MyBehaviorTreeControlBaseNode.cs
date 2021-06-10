// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeControlBaseNode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.BehaviorTree
{
  [MyBehaviorTreeNodeType(typeof (MyObjectBuilder_BehaviorControlBaseNode), typeof (MyBehaviorTreeControlNodeMemory))]
  internal abstract class MyBehaviorTreeControlBaseNode : MyBehaviorTreeNode
  {
    protected List<MyBehaviorTreeNode> m_children;
    protected bool m_isMemorable;
    protected string m_name;

    public abstract MyBehaviorTreeState SearchedValue { get; }

    public abstract MyBehaviorTreeState FinalValue { get; }

    public abstract string DebugSign { get; }

    public override bool IsRunningStateSource => false;

    public override void Construct(
      MyObjectBuilder_BehaviorTreeNode nodeDefinition,
      MyBehaviorTree.MyBehaviorTreeDesc treeDesc)
    {
      base.Construct(nodeDefinition, treeDesc);
      MyObjectBuilder_BehaviorControlBaseNode behaviorControlBaseNode = (MyObjectBuilder_BehaviorControlBaseNode) nodeDefinition;
      this.m_children = new List<MyBehaviorTreeNode>(behaviorControlBaseNode.BTNodes.Length);
      this.m_isMemorable = behaviorControlBaseNode.IsMemorable;
      this.m_name = behaviorControlBaseNode.Name;
      foreach (MyObjectBuilder_BehaviorTreeNode btNode1 in behaviorControlBaseNode.BTNodes)
      {
        MyBehaviorTreeNode btNode2 = MyBehaviorTreeNodeFactory.CreateBTNode(btNode1);
        btNode2.Construct(btNode1, treeDesc);
        this.m_children.Add(btNode2);
      }
    }

    public override MyBehaviorTreeState Tick(
      IMyBot bot,
      MyPerTreeBotMemory botTreeMemory)
    {
      MyBehaviorTreeControlNodeMemory nodeMemoryByIndex = botTreeMemory.GetNodeMemoryByIndex(this.MemoryIndex) as MyBehaviorTreeControlNodeMemory;
      for (int initialIndex = nodeMemoryByIndex.InitialIndex; initialIndex < this.m_children.Count; ++initialIndex)
      {
        bot.BotMemory.RememberNode(this.m_children[initialIndex].MemoryIndex);
        if (MyDebugDrawSettings.DEBUG_DRAW_BOTS)
        {
          if (!(this.m_children[initialIndex] is MyBehaviorTreeControlBaseNode))
          {
            if (!(this.m_children[initialIndex] is MyBehaviorTreeActionNode))
            {
              if (this.m_children[initialIndex] is MyBehaviorTreeDecoratorNode)
                (this.m_children[initialIndex] as MyBehaviorTreeDecoratorNode).GetName();
            }
            else
              (this.m_children[initialIndex] as MyBehaviorTreeActionNode).GetActionName();
          }
          else
          {
            string name = (this.m_children[initialIndex] as MyBehaviorTreeControlBaseNode).m_name;
          }
          this.m_runningActionName = "";
        }
        MyBehaviorTreeState behaviorTreeState = this.m_children[initialIndex].Tick(bot, botTreeMemory);
        if (behaviorTreeState == this.SearchedValue || behaviorTreeState == this.FinalValue)
          this.m_children[initialIndex].PostTick(bot, botTreeMemory);
        if (behaviorTreeState == MyBehaviorTreeState.RUNNING || behaviorTreeState == this.SearchedValue)
        {
          nodeMemoryByIndex.NodeState = behaviorTreeState;
          if (behaviorTreeState == MyBehaviorTreeState.RUNNING)
          {
            if (this.m_isMemorable)
              nodeMemoryByIndex.InitialIndex = initialIndex;
          }
          else
            bot.BotMemory.ForgetNode();
          return behaviorTreeState;
        }
        bot.BotMemory.ForgetNode();
      }
      nodeMemoryByIndex.NodeState = this.FinalValue;
      nodeMemoryByIndex.InitialIndex = 0;
      return this.FinalValue;
    }

    [Conditional("DEBUG")]
    private void RecordRunningNodeName(MyBehaviorTreeState state, MyBehaviorTreeNode node)
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_BOTS)
        return;
      this.m_runningActionName = "";
      if (state != MyBehaviorTreeState.RUNNING)
        return;
      if (node is MyBehaviorTreeActionNode behaviorTreeActionNode)
      {
        this.m_runningActionName = behaviorTreeActionNode.GetActionName();
      }
      else
      {
        string str = node.m_runningActionName;
        if (str.Contains("Par_N"))
          str = str.Replace("Par_N", this.m_name + "-");
        this.m_runningActionName = str;
      }
    }

    public override void PostTick(IMyBot bot, MyPerTreeBotMemory botTreeMemory)
    {
      botTreeMemory.GetNodeMemoryByIndex(this.MemoryIndex).PostTickMemory();
      foreach (MyBehaviorTreeNode child in this.m_children)
        child.PostTick(bot, botTreeMemory);
    }

    public override void DebugDraw(
      Vector2 pos,
      Vector2 size,
      List<MyBehaviorTreeNodeMemory> nodesMemory)
    {
      MyRenderProxy.DebugDrawText2D(pos, this.DebugSign, nodesMemory[this.MemoryIndex].NodeStateColor, MyBehaviorTreeNode.DEBUG_TEXT_SCALE, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      size.X *= MyBehaviorTreeNode.DEBUG_SCALE;
      Vector2 position = this.m_children.Count > 1 ? pos - size * 0.5f : pos;
      position.Y += MyBehaviorTreeNode.DEBUG_TEXT_Y_OFFSET;
      size.X /= (float) Math.Max(this.m_children.Count - 1, 1);
      foreach (MyBehaviorTreeNode child in this.m_children)
      {
        Vector2 vector2 = position - pos;
        vector2.Normalize();
        MyRenderProxy.DebugDrawLine2D(pos + vector2 * MyBehaviorTreeNode.DEBUG_LINE_OFFSET_MULT, position - vector2 * MyBehaviorTreeNode.DEBUG_LINE_OFFSET_MULT, nodesMemory[child.MemoryIndex].NodeStateColor, nodesMemory[child.MemoryIndex].NodeStateColor);
        child.DebugDraw(position, size, nodesMemory);
        position.X += size.X;
      }
    }

    public override int GetHashCode()
    {
      int num = ((base.GetHashCode() * 397 ^ this.m_isMemorable.GetHashCode()) * 397 ^ this.SearchedValue.GetHashCode()) * 397 ^ this.FinalValue.GetHashCode();
      for (int index = 0; index < this.m_children.Count; ++index)
        num = num * 397 ^ this.m_children[index].GetHashCode();
      return num;
    }

    public override string ToString() => this.m_name;
  }
}
