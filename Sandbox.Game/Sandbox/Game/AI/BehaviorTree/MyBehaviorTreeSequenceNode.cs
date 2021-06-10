// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeSequenceNode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.AI.BehaviorTree
{
  [MyBehaviorTreeNodeType(typeof (MyObjectBuilder_BehaviorTreeSequenceNode), typeof (MyBehaviorTreeControlNodeMemory))]
  internal class MyBehaviorTreeSequenceNode : MyBehaviorTreeControlBaseNode
  {
    public override MyBehaviorTreeState SearchedValue => MyBehaviorTreeState.FAILURE;

    public override MyBehaviorTreeState FinalValue => MyBehaviorTreeState.SUCCESS;

    public override string DebugSign => "->";

    public override string ToString() => "SEQ: " + base.ToString();

    private class Sandbox_Game_AI_BehaviorTree_MyBehaviorTreeSequenceNode\u003C\u003EActor : IActivator, IActivator<MyBehaviorTreeSequenceNode>
    {
      object IActivator.CreateInstance() => (object) new MyBehaviorTreeSequenceNode();

      MyBehaviorTreeSequenceNode IActivator<MyBehaviorTreeSequenceNode>.CreateInstance() => new MyBehaviorTreeSequenceNode();
    }
  }
}
