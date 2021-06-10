// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.DecoratorLogic.MyBehaviorTreeDecoratorCounterLogic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;

namespace Sandbox.Game.AI.BehaviorTree.DecoratorLogic
{
  public class MyBehaviorTreeDecoratorCounterLogic : IMyDecoratorLogic
  {
    public int CounterLimit { get; private set; }

    public MyBehaviorTreeDecoratorCounterLogic() => this.CounterLimit = 0;

    public void Construct(
      MyObjectBuilder_BehaviorTreeDecoratorNode.Logic logicData)
    {
      this.CounterLimit = (logicData as MyObjectBuilder_BehaviorTreeDecoratorNode.CounterLogic).Count;
    }

    public void Update(
      MyBehaviorTreeDecoratorNodeMemory.LogicMemory logicMemory)
    {
      MyBehaviorTreeDecoratorNodeMemory.CounterLogicMemory counterLogicMemory = logicMemory as MyBehaviorTreeDecoratorNodeMemory.CounterLogicMemory;
      if (counterLogicMemory.CurrentCount == this.CounterLimit)
        counterLogicMemory.CurrentCount = 0;
      else
        ++counterLogicMemory.CurrentCount;
    }

    public bool CanRun(
      MyBehaviorTreeDecoratorNodeMemory.LogicMemory logicMemory)
    {
      return (logicMemory as MyBehaviorTreeDecoratorNodeMemory.CounterLogicMemory).CurrentCount == this.CounterLimit;
    }

    public MyBehaviorTreeDecoratorNodeMemory.LogicMemory GetNewMemoryObject() => (MyBehaviorTreeDecoratorNodeMemory.LogicMemory) new MyBehaviorTreeDecoratorNodeMemory.CounterLogicMemory();

    public override int GetHashCode() => this.CounterLimit.GetHashCode();

    public override string ToString() => "Counter";
  }
}
