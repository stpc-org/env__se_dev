// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyGridCreateToolDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GridCreateToolDefinition), null)]
  public class MyGridCreateToolDefinition : MyDefinitionBase
  {
    protected override void Init(MyObjectBuilder_DefinitionBase builder) => base.Init(builder);

    private class Sandbox_Definitions_MyGridCreateToolDefinition\u003C\u003EActor : IActivator, IActivator<MyGridCreateToolDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGridCreateToolDefinition();

      MyGridCreateToolDefinition IActivator<MyGridCreateToolDefinition>.CreateInstance() => new MyGridCreateToolDefinition();
    }
  }
}
