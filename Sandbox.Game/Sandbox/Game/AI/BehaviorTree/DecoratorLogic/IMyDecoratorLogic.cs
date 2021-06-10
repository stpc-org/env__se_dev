// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.DecoratorLogic.IMyDecoratorLogic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;

namespace Sandbox.Game.AI.BehaviorTree.DecoratorLogic
{
  public interface IMyDecoratorLogic
  {
    void Construct(
      MyObjectBuilder_BehaviorTreeDecoratorNode.Logic logicData);

    void Update(
      MyBehaviorTreeDecoratorNodeMemory.LogicMemory memory);

    bool CanRun(
      MyBehaviorTreeDecoratorNodeMemory.LogicMemory memory);

    MyBehaviorTreeDecoratorNodeMemory.LogicMemory GetNewMemoryObject();
  }
}
