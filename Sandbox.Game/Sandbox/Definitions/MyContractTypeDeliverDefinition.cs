// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyContractTypeDeliverDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ContractTypeDeliverDefinition), null)]
  public class MyContractTypeDeliverDefinition : MyContractTypeDefinition
  {
    public double Duration_BaseTime;
    public double Duration_TimePerJumpDist;
    public double Duration_TimePerMeter;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_ContractTypeDeliverDefinition deliverDefinition))
        return;
      this.Duration_BaseTime = deliverDefinition.Duration_BaseTime;
      this.Duration_TimePerJumpDist = deliverDefinition.Duration_TimePerJumpDist;
      this.Duration_TimePerMeter = deliverDefinition.Duration_TimePerMeter;
    }

    private class Sandbox_Definitions_MyContractTypeDeliverDefinition\u003C\u003EActor : IActivator, IActivator<MyContractTypeDeliverDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyContractTypeDeliverDefinition();

      MyContractTypeDeliverDefinition IActivator<MyContractTypeDeliverDefinition>.CreateInstance() => new MyContractTypeDeliverDefinition();
    }
  }
}
