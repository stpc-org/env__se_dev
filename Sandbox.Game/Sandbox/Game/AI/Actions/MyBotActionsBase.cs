// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Actions.MyBotActionsBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.AI;

namespace Sandbox.Game.AI.Actions
{
  public abstract class MyBotActionsBase
  {
    [MyBehaviorTreeAction("DummyRunningNode")]
    protected MyBehaviorTreeState DummyRunningNode() => MyBehaviorTreeState.RUNNING;

    [MyBehaviorTreeAction("DummySucceedingNode", ReturnsRunning = false)]
    protected MyBehaviorTreeState DummySucceedingNode() => MyBehaviorTreeState.SUCCESS;

    [MyBehaviorTreeAction("DummyFailingNode", ReturnsRunning = false)]
    protected MyBehaviorTreeState DummyFailingNode() => MyBehaviorTreeState.FAILURE;

    [MyBehaviorTreeAction("IsSurvivalGame", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsSurvivalGame() => MySession.Static.SurvivalMode ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.FAILURE;

    [MyBehaviorTreeAction("IsCreativeGame", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsCreativeGame() => MySession.Static.CreativeMode ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.FAILURE;

    [MyBehaviorTreeAction("Idle", MyBehaviorTreeActionType.INIT)]
    protected virtual void Init_Idle()
    {
    }

    [MyBehaviorTreeAction("Idle")]
    protected virtual MyBehaviorTreeState Idle() => MyBehaviorTreeState.RUNNING;

    [MyBehaviorTreeAction("SetBoolean", ReturnsRunning = false)]
    protected MyBehaviorTreeState SetBoolean(
      [BTOut] ref MyBBMemoryBool variable,
      [BTParam] bool value)
    {
      if (variable == null)
        variable = new MyBBMemoryBool();
      variable.BoolValue = value;
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("IsTrue", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsTrue([BTIn] ref MyBBMemoryBool variable) => variable == null || !variable.BoolValue ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;

    [MyBehaviorTreeAction("IsFalse", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsFalse([BTIn] ref MyBBMemoryBool variable) => variable != null && !variable.BoolValue ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.FAILURE;

    [MyBehaviorTreeAction("SetInt", ReturnsRunning = false)]
    protected MyBehaviorTreeState SetInt([BTOut] ref MyBBMemoryInt variable, [BTParam] int value)
    {
      if (variable == null)
        variable = new MyBBMemoryInt();
      variable.IntValue = value;
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("IsIntLargerThan", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsIntLargerThan(
      [BTIn] ref MyBBMemoryInt variable,
      [BTParam] int value)
    {
      if (variable == null)
        variable = new MyBBMemoryInt();
      return variable.IntValue <= value ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("Increment", ReturnsRunning = false)]
    protected MyBehaviorTreeState Increment([BTInOut] ref MyBBMemoryInt variable)
    {
      if (variable == null)
        variable = new MyBBMemoryInt();
      ++variable.IntValue;
      return MyBehaviorTreeState.SUCCESS;
    }
  }
}
