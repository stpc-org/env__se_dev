// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyBehaviorDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_BehaviorTreeDefinition), null)]
  public class MyBehaviorDefinition : MyDefinitionBase
  {
    public MyObjectBuilder_BehaviorTreeNode FirstNode;
    public string Behavior;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_BehaviorTreeDefinition behaviorTreeDefinition = (MyObjectBuilder_BehaviorTreeDefinition) builder;
      this.FirstNode = behaviorTreeDefinition.FirstNode;
      this.Behavior = behaviorTreeDefinition.Behavior;
    }

    private class Sandbox_Definitions_MyBehaviorDefinition\u003C\u003EActor : IActivator, IActivator<MyBehaviorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyBehaviorDefinition();

      MyBehaviorDefinition IActivator<MyBehaviorDefinition>.CreateInstance() => new MyBehaviorDefinition();
    }
  }
}
