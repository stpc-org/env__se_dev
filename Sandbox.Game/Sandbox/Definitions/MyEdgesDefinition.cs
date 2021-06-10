// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyEdgesDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_EdgesDefinition), null)]
  public class MyEdgesDefinition : MyDefinitionBase
  {
    public MyEdgesModelSet Large;
    public MyEdgesModelSet Small;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_EdgesDefinition builderEdgesDefinition = builder as MyObjectBuilder_EdgesDefinition;
      this.Large = builderEdgesDefinition.Large;
      this.Small = builderEdgesDefinition.Small;
    }

    private class Sandbox_Definitions_MyEdgesDefinition\u003C\u003EActor : IActivator, IActivator<MyEdgesDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyEdgesDefinition();

      MyEdgesDefinition IActivator<MyEdgesDefinition>.CreateInstance() => new MyEdgesDefinition();
    }
  }
}
