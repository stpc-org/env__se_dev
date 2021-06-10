// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyOxygenContainerDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_OxygenContainerDefinition), null)]
  public class MyOxygenContainerDefinition : MyPhysicalItemDefinition
  {
    public float Capacity;
    public MyDefinitionId StoredGasId;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_OxygenContainerDefinition containerDefinition = builder as MyObjectBuilder_OxygenContainerDefinition;
      this.Capacity = containerDefinition.Capacity;
      this.StoredGasId = !containerDefinition.StoredGasId.IsNull() ? (MyDefinitionId) containerDefinition.StoredGasId : new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Oxygen");
    }

    private class Sandbox_Definitions_MyOxygenContainerDefinition\u003C\u003EActor : IActivator, IActivator<MyOxygenContainerDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyOxygenContainerDefinition();

      MyOxygenContainerDefinition IActivator<MyOxygenContainerDefinition>.CreateInstance() => new MyOxygenContainerDefinition();
    }
  }
}
