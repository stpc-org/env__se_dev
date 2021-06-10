// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyEnvironmentItemDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_EnvironmentItemDefinition), null)]
  public class MyEnvironmentItemDefinition : MyPhysicalModelDefinition
  {
    protected override void Init(MyObjectBuilder_DefinitionBase builder) => base.Init(builder);

    private class Sandbox_Definitions_MyEnvironmentItemDefinition\u003C\u003EActor : IActivator, IActivator<MyEnvironmentItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyEnvironmentItemDefinition();

      MyEnvironmentItemDefinition IActivator<MyEnvironmentItemDefinition>.CreateInstance() => new MyEnvironmentItemDefinition();
    }
  }
}
