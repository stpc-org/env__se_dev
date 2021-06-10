// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyCargoContainerDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_CargoContainerDefinition), null)]
  public class MyCargoContainerDefinition : MyCubeBlockDefinition
  {
    public Vector3 InventorySize;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.InventorySize = (builder as MyObjectBuilder_CargoContainerDefinition).InventorySize;
    }

    private class Sandbox_Definitions_MyCargoContainerDefinition\u003C\u003EActor : IActivator, IActivator<MyCargoContainerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyCargoContainerDefinition();

      MyCargoContainerDefinition IActivator<MyCargoContainerDefinition>.CreateInstance() => new MyCargoContainerDefinition();
    }
  }
}
