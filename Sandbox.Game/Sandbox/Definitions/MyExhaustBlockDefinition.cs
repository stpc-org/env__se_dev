// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyExhaustBlockDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ExhaustBlockDefinition), null)]
  public class MyExhaustBlockDefinition : MyCubeBlockDefinition
  {
    public float RequiredPowerInput;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.RequiredPowerInput = ((MyObjectBuilder_ExhaustBlockDefinition) builder).RequiredPowerInput;
    }

    private class Sandbox_Definitions_MyExhaustBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyExhaustBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyExhaustBlockDefinition();

      MyExhaustBlockDefinition IActivator<MyExhaustBlockDefinition>.CreateInstance() => new MyExhaustBlockDefinition();
    }
  }
}
