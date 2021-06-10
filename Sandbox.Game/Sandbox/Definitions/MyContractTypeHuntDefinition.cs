// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyContractTypeHuntDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ContractTypeHuntDefinition), null)]
  public class MyContractTypeHuntDefinition : MyContractTypeDefinition
  {
    public int RemarkPeriodInS;
    public float RemarkVariance;
    public double KillRange;
    public float KillRangeMultiplier;
    public int ReputationLossForTarget;
    public double RewardRadius;
    public double Duration_BaseTime;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_ContractTypeHuntDefinition typeHuntDefinition))
        return;
      this.RemarkPeriodInS = typeHuntDefinition.RemarkPeriodInS;
      this.RemarkVariance = typeHuntDefinition.RemarkVariance;
      this.KillRange = typeHuntDefinition.KillRange;
      this.KillRangeMultiplier = typeHuntDefinition.KillRangeMultiplier;
      this.ReputationLossForTarget = typeHuntDefinition.ReputationLossForTarget;
      this.RewardRadius = typeHuntDefinition.RewardRadius;
      this.Duration_BaseTime = typeHuntDefinition.Duration_BaseTime;
    }

    private class Sandbox_Definitions_MyContractTypeHuntDefinition\u003C\u003EActor : IActivator, IActivator<MyContractTypeHuntDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyContractTypeHuntDefinition();

      MyContractTypeHuntDefinition IActivator<MyContractTypeHuntDefinition>.CreateInstance() => new MyContractTypeHuntDefinition();
    }
  }
}
