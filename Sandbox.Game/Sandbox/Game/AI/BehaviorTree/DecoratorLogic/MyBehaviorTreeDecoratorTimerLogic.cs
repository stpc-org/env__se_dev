// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.DecoratorLogic.MyBehaviorTreeDecoratorTimerLogic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Diagnostics;
using VRage.Game;

namespace Sandbox.Game.AI.BehaviorTree.DecoratorLogic
{
  public class MyBehaviorTreeDecoratorTimerLogic : IMyDecoratorLogic
  {
    public long TimeInMs { get; private set; }

    public MyBehaviorTreeDecoratorTimerLogic() => this.TimeInMs = 0L;

    public void Construct(
      MyObjectBuilder_BehaviorTreeDecoratorNode.Logic logicData)
    {
      this.TimeInMs = (logicData as MyObjectBuilder_BehaviorTreeDecoratorNode.TimerLogic).TimeInMs;
    }

    public void Update(
      MyBehaviorTreeDecoratorNodeMemory.LogicMemory logicMemory)
    {
      MyBehaviorTreeDecoratorNodeMemory.TimerLogicMemory timerLogicMemory = logicMemory as MyBehaviorTreeDecoratorNodeMemory.TimerLogicMemory;
      if ((Stopwatch.GetTimestamp() - timerLogicMemory.CurrentTime) / Stopwatch.Frequency * 1000L > this.TimeInMs)
      {
        timerLogicMemory.CurrentTime = Stopwatch.GetTimestamp();
        timerLogicMemory.TimeLimitReached = true;
      }
      else
        timerLogicMemory.TimeLimitReached = false;
    }

    public bool CanRun(
      MyBehaviorTreeDecoratorNodeMemory.LogicMemory logicMemory)
    {
      return (logicMemory as MyBehaviorTreeDecoratorNodeMemory.TimerLogicMemory).TimeLimitReached;
    }

    public MyBehaviorTreeDecoratorNodeMemory.LogicMemory GetNewMemoryObject() => (MyBehaviorTreeDecoratorNodeMemory.LogicMemory) new MyBehaviorTreeDecoratorNodeMemory.TimerLogicMemory();

    public override int GetHashCode() => ((int) this.TimeInMs).GetHashCode();

    public override string ToString() => "Timer";
  }
}
