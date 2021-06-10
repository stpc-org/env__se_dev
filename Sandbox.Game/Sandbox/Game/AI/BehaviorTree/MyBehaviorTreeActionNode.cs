// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeActionNode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.BehaviorTree
{
  [MyBehaviorTreeNodeType(typeof (MyObjectBuilder_BehaviorTreeActionNode), typeof (MyBehaviorTreeNodeMemory))]
  internal class MyBehaviorTreeActionNode : MyBehaviorTreeNode
  {
    private MyStringId m_actionName;
    private object[] m_parameters;

    public bool ReturnsRunning { get; }

    public override bool IsRunningStateSource => this.ReturnsRunning;

    public MyBehaviorTreeActionNode()
    {
      this.m_actionName = MyStringId.NullOrEmpty;
      this.m_parameters = (object[]) null;
      this.ReturnsRunning = true;
    }

    public override void Construct(
      MyObjectBuilder_BehaviorTreeNode nodeDefinition,
      MyBehaviorTree.MyBehaviorTreeDesc treeDesc)
    {
      base.Construct(nodeDefinition, treeDesc);
      MyObjectBuilder_BehaviorTreeActionNode behaviorTreeActionNode = (MyObjectBuilder_BehaviorTreeActionNode) nodeDefinition;
      if (!string.IsNullOrEmpty(behaviorTreeActionNode.ActionName))
      {
        this.m_actionName = MyStringId.GetOrCompute(behaviorTreeActionNode.ActionName);
        treeDesc.ActionIds.Add(this.m_actionName);
      }
      if (behaviorTreeActionNode.Parameters == null)
        return;
      MyObjectBuilder_BehaviorTreeActionNode.TypeValue[] parameters = behaviorTreeActionNode.Parameters;
      this.m_parameters = new object[parameters.Length];
      for (int index = 0; index < this.m_parameters.Length; ++index)
      {
        MyObjectBuilder_BehaviorTreeActionNode.TypeValue typeValue = parameters[index];
        if (typeValue is MyObjectBuilder_BehaviorTreeActionNode.MemType)
        {
          string str = (string) typeValue.GetValue();
          this.m_parameters[index] = (object) (Boxed<MyStringId>) MyStringId.GetOrCompute(str);
        }
        else
          this.m_parameters[index] = typeValue.GetValue();
      }
    }

    public override MyBehaviorTreeState Tick(
      IMyBot bot,
      MyPerTreeBotMemory botTreeMemory)
    {
      if (bot.ActionCollection.ReturnsRunning(this.m_actionName))
        bot.BotMemory.ProcessLastRunningNode((MyBehaviorTreeNode) this);
      MyBehaviorTreeNodeMemory nodeMemoryByIndex = botTreeMemory.GetNodeMemoryByIndex(this.MemoryIndex);
      if (!nodeMemoryByIndex.InitCalled)
      {
        nodeMemoryByIndex.InitCalled = true;
        if (bot.ActionCollection.ContainsInitAction(this.m_actionName))
          bot.ActionCollection.PerformInitAction(bot, this.m_actionName);
      }
      MyBehaviorTreeState behaviorTreeState = bot.ActionCollection.PerformAction(bot, this.m_actionName, this.m_parameters);
      nodeMemoryByIndex.NodeState = behaviorTreeState;
      return behaviorTreeState;
    }

    public override void PostTick(IMyBot bot, MyPerTreeBotMemory botTreeMemory)
    {
      MyBehaviorTreeNodeMemory nodeMemoryByIndex = botTreeMemory.GetNodeMemoryByIndex(this.MemoryIndex);
      if (!nodeMemoryByIndex.InitCalled)
        return;
      if (bot.ActionCollection.ContainsPostAction(this.m_actionName))
        bot.ActionCollection.PerformPostAction(bot, this.m_actionName);
      nodeMemoryByIndex.InitCalled = false;
    }

    public override void DebugDraw(
      Vector2 position,
      Vector2 size,
      List<MyBehaviorTreeNodeMemory> nodesMemory)
    {
      MyRenderProxy.DebugDrawText2D(position, "A:" + (object) this.m_actionName, nodesMemory[this.MemoryIndex].NodeStateColor, MyBehaviorTreeNode.DEBUG_TEXT_SCALE, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
    }

    public override int GetHashCode()
    {
      int num = base.GetHashCode() * 397 ^ this.m_actionName.ToString().GetHashCode();
      if (this.m_parameters != null)
      {
        foreach (object parameter in this.m_parameters)
          num = num * 397 ^ parameter.ToString().GetHashCode();
      }
      return num;
    }

    public override string ToString() => "ACTION: " + (object) this.m_actionName;

    public string GetActionName() => this.m_actionName.ToString();

    private class Sandbox_Game_AI_BehaviorTree_MyBehaviorTreeActionNode\u003C\u003EActor : IActivator, IActivator<MyBehaviorTreeActionNode>
    {
      object IActivator.CreateInstance() => (object) new MyBehaviorTreeActionNode();

      MyBehaviorTreeActionNode IActivator<MyBehaviorTreeActionNode>.CreateInstance() => new MyBehaviorTreeActionNode();
    }
  }
}
