// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyAnimalBotDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_AnimalBotDefinition), null)]
  public class MyAnimalBotDefinition : MyAgentDefinition
  {
    private class Sandbox_Definitions_MyAnimalBotDefinition\u003C\u003EActor : IActivator, IActivator<MyAnimalBotDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyAnimalBotDefinition();

      MyAnimalBotDefinition IActivator<MyAnimalBotDefinition>.CreateInstance() => new MyAnimalBotDefinition();
    }
  }
}
