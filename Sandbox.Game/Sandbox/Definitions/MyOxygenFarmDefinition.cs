// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyOxygenFarmDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_OxygenFarmDefinition), null)]
  public class MyOxygenFarmDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public MyStringHash ResourceSourceGroup;
    public Vector3 PanelOrientation;
    public bool IsTwoSided;
    public float PanelOffset;
    public MyDefinitionId ProducedGas;
    public float MaxGasOutput;
    public float OperationalPowerConsumption;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_OxygenFarmDefinition oxygenFarmDefinition = builder as MyObjectBuilder_OxygenFarmDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(oxygenFarmDefinition.ResourceSinkGroup);
      this.ResourceSourceGroup = MyStringHash.GetOrCompute(oxygenFarmDefinition.ResourceSourceGroup);
      this.PanelOrientation = oxygenFarmDefinition.PanelOrientation;
      this.IsTwoSided = oxygenFarmDefinition.TwoSidedPanel;
      this.PanelOffset = oxygenFarmDefinition.PanelOffset;
      this.ProducedGas = !oxygenFarmDefinition.ProducedGas.Id.IsNull() ? (MyDefinitionId) oxygenFarmDefinition.ProducedGas.Id : new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_GasProperties), "Oxygen");
      this.MaxGasOutput = oxygenFarmDefinition.ProducedGas.MaxOutputPerSecond;
      this.OperationalPowerConsumption = oxygenFarmDefinition.OperationalPowerConsumption;
    }

    private class Sandbox_Definitions_MyOxygenFarmDefinition\u003C\u003EActor : IActivator, IActivator<MyOxygenFarmDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyOxygenFarmDefinition();

      MyOxygenFarmDefinition IActivator<MyOxygenFarmDefinition>.CreateInstance() => new MyOxygenFarmDefinition();
    }
  }
}
