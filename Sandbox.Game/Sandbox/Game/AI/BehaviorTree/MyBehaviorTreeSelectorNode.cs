// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeSelectorNode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.AI.BehaviorTree
{
  [MyBehaviorTreeNodeType(typeof (MyObjectBuilder_BehaviorTreeSelectorNode), typeof (MyBehaviorTreeControlNodeMemory))]
  internal class MyBehaviorTreeSelectorNode : MyBehaviorTreeControlBaseNode
  {
    public override MyBehaviorTreeState SearchedValue => MyBehaviorTreeState.SUCCESS;

    public override MyBehaviorTreeState FinalValue => MyBehaviorTreeState.FAILURE;

    public override string DebugSign => "?";

    public override string ToString() => "SEL: " + base.ToString();

    private class Sandbox_Game_AI_BehaviorTree_MyBehaviorTreeSelectorNode\u003C\u003EActor : IActivator, IActivator<MyBehaviorTreeSelectorNode>
    {
      object IActivator.CreateInstance() => (object) new MyBehaviorTreeSelectorNode();

      MyBehaviorTreeSelectorNode IActivator<MyBehaviorTreeSelectorNode>.CreateInstance() => new MyBehaviorTreeSelectorNode();
    }
  }
}
