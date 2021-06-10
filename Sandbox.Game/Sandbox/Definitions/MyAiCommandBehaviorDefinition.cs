// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyAiCommandBehaviorDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_AiCommandBehaviorDefinition), null)]
  public class MyAiCommandBehaviorDefinition : MyAiCommandDefinition
  {
    public string BehaviorTreeName;
    public MyAiCommandEffect CommandEffect;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_AiCommandBehaviorDefinition behaviorDefinition = builder as MyObjectBuilder_AiCommandBehaviorDefinition;
      this.BehaviorTreeName = behaviorDefinition.BehaviorTreeName;
      this.CommandEffect = behaviorDefinition.CommandEffect;
    }

    private class Sandbox_Definitions_MyAiCommandBehaviorDefinition\u003C\u003EActor : IActivator, IActivator<MyAiCommandBehaviorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAiCommandBehaviorDefinition();

      MyAiCommandBehaviorDefinition IActivator<MyAiCommandBehaviorDefinition>.CreateInstance() => new MyAiCommandBehaviorDefinition();
    }
  }
}
