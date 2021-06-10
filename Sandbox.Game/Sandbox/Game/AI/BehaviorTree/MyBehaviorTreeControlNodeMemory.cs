// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeControlNodeMemory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;

namespace Sandbox.Game.AI.BehaviorTree
{
  [MyBehaviorTreeNodeMemoryType(typeof (MyObjectBuilder_BehaviorTreeControlNodeMemory))]
  public class MyBehaviorTreeControlNodeMemory : MyBehaviorTreeNodeMemory
  {
    public int InitialIndex { get; set; }

    public MyBehaviorTreeControlNodeMemory() => this.InitialIndex = 0;

    public override void Init(MyObjectBuilder_BehaviorTreeNodeMemory builder)
    {
      base.Init(builder);
      this.InitialIndex = (builder as MyObjectBuilder_BehaviorTreeControlNodeMemory).InitialIndex;
    }

    public override MyObjectBuilder_BehaviorTreeNodeMemory GetObjectBuilder()
    {
      MyObjectBuilder_BehaviorTreeControlNodeMemory objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_BehaviorTreeControlNodeMemory;
      objectBuilder.InitialIndex = this.InitialIndex;
      return (MyObjectBuilder_BehaviorTreeNodeMemory) objectBuilder;
    }

    public override void ClearMemory()
    {
      base.ClearMemory();
      this.InitialIndex = 0;
    }

    public override void PostTickMemory()
    {
      base.PostTickMemory();
      this.InitialIndex = 0;
    }
  }
}
