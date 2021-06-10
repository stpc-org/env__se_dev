// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyContractTypeRepairDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_ContractTypeRepairDefinition), null)]
  internal class MyContractTypeRepairDefinition : MyContractTypeDefinition
  {
    public double MaxGridDistance;
    public double MinGridDistance;
    public double Duration_BaseTime;
    public double Duration_TimePerMeter;
    public float PriceToRewardCoeficient;
    public float PriceSpread;
    public float TimeToPriceDenominator = 60f;
    public List<string> PrefabNames;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_ContractTypeRepairDefinition repairDefinition))
        return;
      this.MaxGridDistance = repairDefinition.MaxGridDistance;
      this.MinGridDistance = repairDefinition.MinGridDistance;
      this.Duration_BaseTime = repairDefinition.Duration_BaseTime;
      this.Duration_TimePerMeter = repairDefinition.Duration_TimePerMeter;
      this.PrefabNames = (List<string>) repairDefinition.PrefabNames;
      this.PriceToRewardCoeficient = repairDefinition.PriceToRewardCoeficient;
      this.PriceSpread = repairDefinition.PriceSpread;
      this.TimeToPriceDenominator = repairDefinition.TimeToPriceDenominator;
    }

    private class Sandbox_Definitions_MyContractTypeRepairDefinition\u003C\u003EActor : IActivator, IActivator<MyContractTypeRepairDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyContractTypeRepairDefinition();

      MyContractTypeRepairDefinition IActivator<MyContractTypeRepairDefinition>.CreateInstance() => new MyContractTypeRepairDefinition();
    }
  }
}
