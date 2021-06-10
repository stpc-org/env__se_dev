// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeNode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.AI.BehaviorTree
{
  [GenerateActivator]
  [MyBehaviorTreeNodeType(typeof (MyObjectBuilder_BehaviorTreeNode))]
  public abstract class MyBehaviorTreeNode
  {
    protected static float DEBUG_TEXT_SCALE = 0.5f;
    protected static float DEBUG_TEXT_Y_OFFSET = 60f;
    protected static float DEBUG_SCALE = 0.4f;
    protected static float DEBUG_ROOT_OFFSET = 20f;
    protected static float DEBUG_LINE_OFFSET_MULT = 25f;
    public const string PARENT_NAME = "Par_N";
    public string m_runningActionName = "";

    public int MemoryIndex { get; private set; }

    public Type MemoryType { get; }

    protected MyBehaviorTreeNode()
    {
      foreach (object customAttribute in this.GetType().GetCustomAttributes(false))
      {
        if (customAttribute.GetType() == typeof (MyBehaviorTreeNodeTypeAttribute))
          this.MemoryType = (customAttribute as MyBehaviorTreeNodeTypeAttribute).MemoryType;
      }
    }

    public virtual void Construct(
      MyObjectBuilder_BehaviorTreeNode nodeDefinition,
      MyBehaviorTree.MyBehaviorTreeDesc treeDesc)
    {
      this.MemoryIndex = treeDesc.MemorableNodesCounter++;
      treeDesc.Nodes.Add(this);
    }

    public abstract MyBehaviorTreeState Tick(
      IMyBot bot,
      MyPerTreeBotMemory nodesMemory);

    public virtual void PostTick(IMyBot bot, MyPerTreeBotMemory nodesMemory)
    {
    }

    public abstract void DebugDraw(
      Vector2 position,
      Vector2 size,
      List<MyBehaviorTreeNodeMemory> nodesMemory);

    public abstract bool IsRunningStateSource { get; }

    public virtual MyBehaviorTreeNodeMemory GetNewMemoryObject() => this.MemoryType != (Type) null && (this.MemoryType.IsSubclassOf(typeof (MyBehaviorTreeNodeMemory)) || this.MemoryType == typeof (MyBehaviorTreeNodeMemory)) ? Activator.CreateInstance(this.MemoryType) as MyBehaviorTreeNodeMemory : new MyBehaviorTreeNodeMemory();

    public override int GetHashCode() => this.MemoryIndex;
  }
}
