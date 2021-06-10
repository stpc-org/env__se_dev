// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyContractTypeEscortDefinition
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
  [MyDefinitionType(typeof (MyObjectBuilder_ContractTypeEscortDefinition), null)]
  public class MyContractTypeEscortDefinition : MyContractTypeDefinition
  {
    public double RewardRadius;
    public double TriggerRadius;
    public double TravelDistanceMax;
    public double TravelDistanceMin;
    public int DroneFirstDelayInS;
    public int DroneAttackPeriodInS;
    public int InitialDelayInS;
    public int DronesPerWave;
    public float Duration_BaseTime;
    public float Duration_FlightTimeMultiplier;
    public List<string> PrefabsAttackDrones;
    public List<string> PrefabsEscortShips;
    public List<string> DroneBehaviours;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_ContractTypeEscortDefinition escortDefinition))
        return;
      this.RewardRadius = escortDefinition.RewardRadius;
      this.TriggerRadius = escortDefinition.TriggerRadius;
      this.TravelDistanceMax = escortDefinition.TravelDistanceMax;
      this.TravelDistanceMin = escortDefinition.TravelDistanceMin;
      this.DroneAttackPeriodInS = escortDefinition.DroneAttackPeriodInS;
      this.DroneFirstDelayInS = escortDefinition.DroneFirstDelayInS;
      this.InitialDelayInS = escortDefinition.InitialDelayInS;
      this.DronesPerWave = escortDefinition.DronesPerWave;
      this.Duration_BaseTime = escortDefinition.Duration_BaseTime;
      this.Duration_FlightTimeMultiplier = escortDefinition.Duration_FlightTimeMultiplier;
      this.PrefabsAttackDrones = (List<string>) escortDefinition.PrefabsAttackDrones;
      this.PrefabsEscortShips = (List<string>) escortDefinition.PrefabsEscortShips;
      this.DroneBehaviours = (List<string>) escortDefinition.DroneBehaviours;
    }

    private class Sandbox_Definitions_MyContractTypeEscortDefinition\u003C\u003EActor : IActivator, IActivator<MyContractTypeEscortDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyContractTypeEscortDefinition();

      MyContractTypeEscortDefinition IActivator<MyContractTypeEscortDefinition>.CreateInstance() => new MyContractTypeEscortDefinition();
    }
  }
}
