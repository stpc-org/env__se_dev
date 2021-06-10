// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeNodeMemory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRageMath;

namespace Sandbox.Game.AI.BehaviorTree
{
  [MyBehaviorTreeNodeMemoryType(typeof (MyObjectBuilder_BehaviorTreeNodeMemory))]
  public class MyBehaviorTreeNodeMemory
  {
    public MyBehaviorTreeState NodeState { get; set; }

    public Color NodeStateColor => MyBehaviorTreeNodeMemory.GetColorByState(this.NodeState);

    public bool InitCalled { get; set; }

    public bool IsTicked => (uint) this.NodeState > 0U;

    public MyBehaviorTreeNodeMemory()
    {
      this.InitCalled = false;
      this.ClearNodeState();
    }

    public virtual void Init(MyObjectBuilder_BehaviorTreeNodeMemory builder) => this.InitCalled = builder.InitCalled;

    public virtual MyObjectBuilder_BehaviorTreeNodeMemory GetObjectBuilder()
    {
      MyObjectBuilder_BehaviorTreeNodeMemory objectBuilder = MyBehaviorTreeNodeMemoryFactory.CreateObjectBuilder(this);
      objectBuilder.InitCalled = this.InitCalled;
      return objectBuilder;
    }

    public virtual void ClearMemory()
    {
      this.NodeState = MyBehaviorTreeState.NOT_TICKED;
      this.InitCalled = false;
    }

    public virtual void PostTickMemory()
    {
    }

    public void ClearNodeState() => this.NodeState = MyBehaviorTreeState.NOT_TICKED;

    private static Color GetColorByState(MyBehaviorTreeState state)
    {
      switch (state)
      {
        case MyBehaviorTreeState.ERROR:
          return Color.Bisque;
        case MyBehaviorTreeState.NOT_TICKED:
          return Color.White;
        case MyBehaviorTreeState.SUCCESS:
          return Color.Green;
        case MyBehaviorTreeState.FAILURE:
          return Color.Red;
        case MyBehaviorTreeState.RUNNING:
          return Color.Yellow;
        default:
          return Color.Black;
      }
    }
  }
}
