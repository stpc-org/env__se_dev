// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeDecoratorNodeMemory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Diagnostics;
using VRage.Game;

namespace Sandbox.Game.AI.BehaviorTree
{
  [MyBehaviorTreeNodeMemoryType(typeof (MyObjectBuilder_BehaviorTreeDecoratorNodeMemory))]
  public class MyBehaviorTreeDecoratorNodeMemory : MyBehaviorTreeNodeMemory
  {
    public MyBehaviorTreeState ChildState { get; set; }

    public MyBehaviorTreeDecoratorNodeMemory.LogicMemory DecoratorLogicMemory { get; set; }

    public override void Init(MyObjectBuilder_BehaviorTreeNodeMemory builder)
    {
      base.Init(builder);
      MyObjectBuilder_BehaviorTreeDecoratorNodeMemory decoratorNodeMemory = builder as MyObjectBuilder_BehaviorTreeDecoratorNodeMemory;
      this.ChildState = decoratorNodeMemory.ChildState;
      this.DecoratorLogicMemory = MyBehaviorTreeDecoratorNodeMemory.GetLogicMemoryByBuilder(decoratorNodeMemory.Logic);
    }

    public override MyObjectBuilder_BehaviorTreeNodeMemory GetObjectBuilder()
    {
      MyObjectBuilder_BehaviorTreeDecoratorNodeMemory objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_BehaviorTreeDecoratorNodeMemory;
      objectBuilder.ChildState = this.ChildState;
      objectBuilder.Logic = this.DecoratorLogicMemory.GetObjectBuilder();
      return (MyObjectBuilder_BehaviorTreeNodeMemory) objectBuilder;
    }

    public override void ClearMemory()
    {
      base.ClearMemory();
      this.ChildState = MyBehaviorTreeState.NOT_TICKED;
      this.DecoratorLogicMemory.ClearMemory();
    }

    public override void PostTickMemory()
    {
      base.PostTickMemory();
      this.ChildState = MyBehaviorTreeState.NOT_TICKED;
      this.DecoratorLogicMemory.PostTickMemory();
    }

    private static MyBehaviorTreeDecoratorNodeMemory.LogicMemory GetLogicMemoryByBuilder(
      MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder builder)
    {
      switch (builder)
      {
        case MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder _:
          return (MyBehaviorTreeDecoratorNodeMemory.LogicMemory) new MyBehaviorTreeDecoratorNodeMemory.TimerLogicMemory();
        case MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder _:
          return (MyBehaviorTreeDecoratorNodeMemory.LogicMemory) new MyBehaviorTreeDecoratorNodeMemory.CounterLogicMemory();
        default:
          return (MyBehaviorTreeDecoratorNodeMemory.LogicMemory) null;
      }
    }

    public abstract class LogicMemory
    {
      public abstract void ClearMemory();

      public abstract void Init(
        MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder logicMemoryBuilder);

      public abstract MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder GetObjectBuilder();

      public abstract void PostTickMemory();
    }

    public class TimerLogicMemory : MyBehaviorTreeDecoratorNodeMemory.LogicMemory
    {
      public bool TimeLimitReached { get; set; }

      public long CurrentTime { get; set; }

      public override void ClearMemory()
      {
        this.TimeLimitReached = true;
        this.CurrentTime = Stopwatch.GetTimestamp();
      }

      public override void Init(
        MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder logicMemoryBuilder)
      {
        MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder logicMemoryBuilder1 = logicMemoryBuilder as MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder;
        this.CurrentTime = Stopwatch.GetTimestamp() - logicMemoryBuilder1.CurrentTime;
        this.TimeLimitReached = logicMemoryBuilder1.TimeLimitReached;
      }

      public override MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder GetObjectBuilder() => (MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder) new MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.TimerLogicMemoryBuilder()
      {
        CurrentTime = (Stopwatch.GetTimestamp() - this.CurrentTime),
        TimeLimitReached = this.TimeLimitReached
      };

      public override void PostTickMemory()
      {
        this.TimeLimitReached = false;
        this.CurrentTime = Stopwatch.GetTimestamp();
      }
    }

    public class CounterLogicMemory : MyBehaviorTreeDecoratorNodeMemory.LogicMemory
    {
      public int CurrentCount { get; set; }

      public override void ClearMemory() => this.CurrentCount = 0;

      public override void Init(
        MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder logicMemoryBuilder)
      {
        this.CurrentCount = (logicMemoryBuilder as MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder).CurrentCount;
      }

      public override MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder GetObjectBuilder() => (MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.LogicMemoryBuilder) new MyObjectBuilder_BehaviorTreeDecoratorNodeMemory.CounterLogicMemoryBuilder()
      {
        CurrentCount = this.CurrentCount
      };

      public override void PostTickMemory() => this.CurrentCount = 0;
    }
  }
}
