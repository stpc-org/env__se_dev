// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyPoweredCargoContainerDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_PoweredCargoContainerDefinition), null)]
  public class MyPoweredCargoContainerDefinition : MyCargoContainerDefinition
  {
    public string ResourceSinkGroup;
    public float RequiredPowerInput;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_PoweredCargoContainerDefinition containerDefinition = builder as MyObjectBuilder_PoweredCargoContainerDefinition;
      this.ResourceSinkGroup = containerDefinition.ResourceSinkGroup;
      this.RequiredPowerInput = containerDefinition.RequiredPowerInput;
    }

    private class Sandbox_Definitions_MyPoweredCargoContainerDefinition\u003C\u003EActor : IActivator, IActivator<MyPoweredCargoContainerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyPoweredCargoContainerDefinition();

      MyPoweredCargoContainerDefinition IActivator<MyPoweredCargoContainerDefinition>.CreateInstance() => new MyPoweredCargoContainerDefinition();
    }
  }
}
