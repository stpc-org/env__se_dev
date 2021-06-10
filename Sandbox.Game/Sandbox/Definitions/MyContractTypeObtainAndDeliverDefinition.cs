// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyContractTypeObtainAndDeliverDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ContractTypeObtainAndDeliverDefinition), null)]
  public class MyContractTypeObtainAndDeliverDefinition : MyContractTypeDefinition
  {
    public List<SerializableDefinitionId> AvailableItems;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_ContractTypeObtainAndDeliverDefinition deliverDefinition))
        return;
      this.AvailableItems = deliverDefinition.AvailableItems;
    }

    private class Sandbox_Definitions_MyContractTypeObtainAndDeliverDefinition\u003C\u003EActor : IActivator, IActivator<MyContractTypeObtainAndDeliverDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyContractTypeObtainAndDeliverDefinition();

      MyContractTypeObtainAndDeliverDefinition IActivator<MyContractTypeObtainAndDeliverDefinition>.CreateInstance() => new MyContractTypeObtainAndDeliverDefinition();
    }
  }
}
