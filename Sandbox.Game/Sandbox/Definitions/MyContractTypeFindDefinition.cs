// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyContractTypeFindDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_ContractTypeFindDefinition), null)]
  public class MyContractTypeFindDefinition : MyContractTypeDefinition
  {
    public double MaxGridDistance;
    public double MinGridDistance;
    public double MaxGpsOffset;
    public double TriggerRadius;
    public double Duration_BaseTime;
    public double Duration_TimePerMeter;
    public double Duration_TimePerCubicKm;
    public List<string> PrefabsSearchableGrids;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_ContractTypeFindDefinition typeFindDefinition))
        return;
      this.MaxGridDistance = typeFindDefinition.MaxGridDistance;
      this.MinGridDistance = typeFindDefinition.MinGridDistance;
      this.MaxGpsOffset = typeFindDefinition.MaxGpsOffset;
      this.TriggerRadius = typeFindDefinition.TriggerRadius;
      this.Duration_BaseTime = typeFindDefinition.Duration_BaseTime;
      this.Duration_TimePerMeter = typeFindDefinition.Duration_TimePerMeter;
      this.Duration_TimePerCubicKm = typeFindDefinition.Duration_TimePerCubicKm;
      this.PrefabsSearchableGrids = (List<string>) typeFindDefinition.PrefabsSearchableGrids;
    }

    private class Sandbox_Definitions_MyContractTypeFindDefinition\u003C\u003EActor : IActivator, IActivator<MyContractTypeFindDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyContractTypeFindDefinition();

      MyContractTypeFindDefinition IActivator<MyContractTypeFindDefinition>.CreateInstance() => new MyContractTypeFindDefinition();
    }
  }
}
